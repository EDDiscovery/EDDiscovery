/*
 * Copyright © 2015 - 2023 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 */

using BaseUtils;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.EDDN;
using EliteDangerousCore.EDSM;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;

namespace EDDiscovery
{
    public partial class EDDiscoveryController
    {
        #region Variables
        public HistoryList History { get; private set; } = new HistoryList();       // we always have a history
        public EDSMLogFetcher EdsmLogFetcher { get; private set; }

        public CancellationTokenSource PendingClose { get; private set; } = new CancellationTokenSource();                    // we want to close boys!      set once, then we close

        public CAPI.CompanionAPI FrontierCAPI;
        public BaseUtils.DDE.DDEServer DDEServer;
        public EliteDangerousCore.UIEvents.UIOverallStatus UIOverallStatus { get; private set; } = new EliteDangerousCore.UIEvents.UIOverallStatus();

        #endregion

        #region Events

        // IN ORDER OF CALLING DURING A REFRESH

        public event Action OnRefreshStarting;                              // UI. Called before worker thread starts, processing history (EDDiscoveryForm uses this to disable buttons and action refreshstart)
        public event Action OnRefreshCommanders;                            // UI. Called when refresh worker completes before final history is made And when a loadgame is seen.
                                                                            // Commanders may have been added. 
        public event Action OnHistoryChange;                                // UI. MAJOR. UC. Mirrored. Called AFTER history is complete, or via RefreshDisplays if a forced refresh is needed.  UC's use this
        public event Action OnRefreshComplete;                              // UI. Called AFTER history is complete.. Form uses this to know the whole process is over, and buttons may be turned on, actions may be run, etc

        // DURING A new Journal entry by the monitor, in order..

        public event Action<UIEvent> OnNewUIEvent;                          // UI. MAJOR. UC. Mirrored. Always called irrespective of commander

        // In order. Current commander only

        public event Action OnNewCommanderDuringPlayDetected;               // UI. Called during play when a new commander has been found (not during history load)
        public event Action<JournalEntry> OnNewJournalEntryUnfiltered;      // UI. Called when a new journal entry is read.  Not filtered by history merge system
        public event Action<HistoryEntry> OnNewHistoryEntryUnfiltered;      // UI. Called when a new history entry is created and databases into it updated, but before adding.  Not filtered by history merging system
        public event Action<HistoryEntry> OnNewEntry;                       // UI. MAJOR. UC. Mirrored. Called after HE has been added to the history list.  Post merging/filtering
        public event Action<HistoryEntry> OnNewEntrySecond;                 // UI. Called after OnNewEntry for more processing. Post filtering
        // If a UC is a Cursor Control type, then OnNewEntry should also fire the cursor control OnChangedSelection, OnTravelSelectionChanged after onNewEntry has been received by the cursor UC

        // Status/Logging updates. These can be run in a thread.  EDF invokes them

        public event Action<string> LogLineHighlight;                       
        public event Action<string> LogLine;
        public event Action<EDDiscoveryForm.StatusLineUpdateType, int, string> StatusLineUpdate;             

        // During a Close

        public event Action OnFinalClose;                                   // UI. Final close, in UI thread

        // During SYNC events

        public event Action OnSyncStarting;                                 // UI. EDSM sync starting
        public event Action<long,long> OnSyncComplete;                      // UI. SYNC has completed, full count, update count


        // Due to background taskc completing async to the rest

        public event Action<bool> OnExpeditionsDownloaded;                  // UI, true if changed entries
        public event Action OnHelpDownloaded;                               // UI

        #endregion

        #region Variables
        private EDJournalUIScanner journalmonitor;

        private Thread backgroundWorker;
        private Thread backgroundRefreshWorker;

        private AutoResetEvent resyncRequestedEvent = new AutoResetEvent(false);

        private int commanderaddorupdatecount;                         // this is set at history read and allows detection of new commanders during reading

        #endregion

        #region Accessors
        private Action<Action> InvokeAsyncOnUiThread;
        #endregion


        #region Refreshing

        public void RefreshDisplays()
        {
            OnHistoryChange?.Invoke();
        }

        #endregion

        #region Initialisation

        public EDDiscoveryController( Action<Action> invokeAsyncOnUiThread)
        {
            InvokeAsyncOnUiThread = invokeAsyncOnUiThread;
            journalqueuedelaytimer = new Timer(DelayPlay, null, Timeout.Infinite, Timeout.Infinite);
        }

        // ED Discovery calls this during its init to allow the controller to set some things up
        public void Init()      
        {
            EdsmLogFetcher = new EDSMLogFetcher(LogLine, (s) => StatusLineUpdate(EDDiscoveryForm.StatusLineUpdateType.EDSMLogFetcher, -1,s));
            EdsmLogFetcher.OnDownloadedSystems += () => RefreshHistoryAsync();

            journalmonitor = new EDJournalUIScanner(InvokeAsyncOnUiThread);
            journalmonitor.OnNewJournalEntry += NewJournalEntryFromScanner;
            journalmonitor.OnNewUIEvent += NewUIEventFromScanner;
            
            FrontierCAPI = new CAPI.CompanionAPI(EDDOptions.Instance.CAPIDirectory(), CAPI.CapiClientIdentity.id, EDDApplicationContext.UserAgent, "eddiscovery");
            DDEServer = new BaseUtils.DDE.DDEServer();          // will be started in shown
        }

        // called by EDDForm during shown
        public void PostInit_Shown()        
        {
            EDDConfig.Instance.Update();    // lost in the midst of time why  

            if (FrontierCAPI.ClientIDAvailable && Environment.OSVersion.Platform == PlatformID.Win32NT)     // if we have a clientid, and on WIN32, we can do a login
            {
                string ddeservername = "edd-dde-server";
                string appPath = System.Reflection.Assembly.GetEntryAssembly()?.Location;

                // we go into the registry, rummage around, and poke the right entries to make the shell do a DDE callback on the System topic to this application
                // and to the DDE server ddeservername
                BaseUtils.DDE.DDEServer.RegisterURICallback(FrontierCAPI.URI, appPath, ddeservername);

                bool ok = false;

                // start the DDE server
                if (DDEServer.Start(ddeservername))
                {
                    // register the system topic.  the callback will get a DDE handle, convert to string and vector to the FrontierCAPI URL callback handler

                    if (DDEServer.AddTopic("System", (hurl) => { string url = BaseUtils.DDE.DDEServer.FromDdeStringHandle(hurl); FrontierCAPI.URLCallBack(url); }))
                    {
                        if (DDEServer.Register())       // and register
                        {
                            ok = true;
                        }
                    }
                }

                LogLine(ok ? "CAPI Installed" : "CAPI Failed to register");

                FrontierCAPI.StatusChange += (s) => { LogLine("CAPI " + s.ToString().SplitCapsWord()); };
            }

            backgroundWorker = new Thread(BackgroundWorkerThread);
            backgroundWorker.IsBackground = true;
            backgroundWorker.Name = "Background Worker Thread";
            backgroundWorker.Start();                                   
        }

        public void Shutdown()      // called to request a shutdown.. background thread co-ords the shutdown.
        {
            if (!PendingClose.IsCancellationRequested)
            {
                PendingClose.Cancel();
                EDDNSync.StopSync();
                EDSMJournalSync.StopSync();
                EdsmLogFetcher.AsyncStop();
                journalmonitor.StopMonitor();
                LogLineHighlight("Closing down, please wait..".T(EDTx.EDDiscoveryController_CD));
                journalqueuedelaytimer.Change(Timeout.Infinite, Timeout.Infinite);
                journalqueuedelaytimer.Dispose();
            }
        }

        public void ResetUIStatus()
        {
            journalmonitor.ResetUIStatus();
        }

        #endregion

        #region Background Worker Thread - kicked off by PostInit_Shown
        private void BackgroundWorkerThread()
        {
            Debug.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDC Background worker thread start");

            // check first and download items

            string desigmapfile = Path.Combine(EDDOptions.Instance.AppDataDirectory, "bodydesignations.csv");

            if (!File.Exists(desigmapfile))
            {
                desigmapfile = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "bodydesignations.csv");
            }

            BodyDesignations.LoadBodyDesignationMap(desigmapfile);

            ReportSyncProgress("");

            if (EDDOptions.Instance.CheckGithubFiles)      // not normal in debug, due to git hub choking
            {
                DateTime lastdownloadtime = UserDatabase.Instance.GetSetting("DownloadFilesLastTime", DateTime.MinValue);
                
                if (DateTime.UtcNow - lastdownloadtime >= new TimeSpan(24, 0, 0))       // only update once per day
                {
                    // Expedition data
                    DownloadExpeditions(PendingClose.Token);

                    // and Help files
                    DownloadHelp(PendingClose.Token);

                    UserDatabase.Instance.PutSetting("DownloadFilesLastTime", DateTime.UtcNow);
                }
            }

            if (!EDDOptions.Instance.NoSystemsLoad)         // if normal operation, see if the EDSM/GEC files need a refresh for next time
            {
                DownloadEDSMGEC(PendingClose.Token);
            }

            SystemNoteClass.GetAllSystemNotes();

            LogLine("Loaded Notes, Bookmarks and Galactic mapping.".T(EDTx.EDDiscoveryController_LN));

            if (!EDDOptions.Instance.NoLoad)        // here in this thread, we do a refresh of history. 
            {
                Debug.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDC Background Worker Load history");

                LogLine("Reading travel history".T(EDTx.EDDiscoveryController_RTH));

                if (EDDOptions.Instance.Commander != null)
                {
                    EDCommander switchto = EDCommander.GetCommander(EDDOptions.Instance.Commander);
                    if (switchto != null)
                        EDCommander.CurrentCmdrID = switchto.Id;
                }

                DoRefreshHistory(new RefreshWorkerArgs { CurrentCommander = EDCommander.CurrentCmdrID });      // load history the first time in this thread
            }

            CheckForSync();     // see if any EDSM sync is needed - this just sets some variables up

            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDC Background worker starting background history refresh worker");

            backgroundRefreshWorker = new Thread(BackgroundHistoryRefreshWorkerThread) { Name = "Background Refresh Worker", IsBackground = true };
            backgroundRefreshWorker.Start();        // start the refresh worker, another thread which does subsequenct (not the primary one) refresh work in the background..

            try
            {
                if (!EDDOptions.Instance.NoSystemsLoad)
                {
                    DoPerformSync();        // this is done after the initial history load..
                }

                while (!PendingClose.IsCancellationRequested)
                {
                    int wh = WaitHandle.WaitAny(new WaitHandle[] { PendingClose.Token.WaitHandle, resyncRequestedEvent });

                    System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDC Background worker kicked by {wh}");

                    if (PendingClose.IsCancellationRequested)
                        break;

                    if (wh == 1)
                    {
                        if (!EDDOptions.Instance.NoSystemsLoad && EDDConfig.Instance.SystemDBDownload)      // if no system off, and EDSM download on
                            DoPerformSync();
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }

            backgroundRefreshWorker.Join();     // this should terminate due to closeRequested..

            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDC Background Worker closing down background worker");

            // Now we have been ordered to close down, so go thru the process

            InvokeAsyncOnUiThread(() =>
            {
                System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDC Background worker invoke Final Close");
                OnFinalClose?.Invoke();
            });
        }

        #endregion



    }
}
