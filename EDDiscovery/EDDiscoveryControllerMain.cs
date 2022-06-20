﻿/*
 * Copyright © 2015 - 2019 EDDiscovery development team
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
 *
 * EDDiscovery is not affiliated with Frontier Developments plc.
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
using System.Threading.Tasks;

namespace EDDiscovery
{
    public partial class EDDiscoveryController
    {
        #region Variables
        public HistoryList history { get; private set; } = new HistoryList();
        public EDSMLogFetcher EdsmLogFetcher { get; private set; }
        public string LogText { get { return logtext; } }

        public bool PendingClose { get; private set; }                      // we want to close boys!      set once, then we close

        public CAPI.CompanionAPI FrontierCAPI;
        public BaseUtils.DDE.DDEServer DDEServer;

        #endregion

        #region Events

        // IN ORDER OF CALLING DURING A REFRESH

        public event Action OnRefreshCommanders;                            // UI. Called when refresh worker completes before final history is made (EDDiscoveryForm uses this to refresh commander stuff).  History is not valid here.
                                                                            // ALSO called if Loadgame is received.

        public event Action OnRefreshStarting;                              // UI. Called before worker thread starts, processing history (EDDiscoveryForm uses this to disable buttons and action refreshstart)
        public event Action<HistoryList> OnHistoryChange;                   // UI. MAJOR. UC. Mirrored. Called AFTER history is complete, or via RefreshDisplays if a forced refresh is needed.  UC's use this
        public event Action OnRefreshComplete;                              // UI. Called AFTER history is complete.. Form uses this to know the whole process is over, and buttons may be turned on, actions may be run, etc
        public event Action<int, string> OnReportRefreshProgress;           // UI. Refresh progress reporter

        // DURING A new Journal entry by the monitor, in order..

        public event Action<UIEvent> OnNewUIEvent;                          // UI. MAJOR. UC. Mirrored. Always called irrespective of commander


        // In order. Current commander only

        public event Action<JournalEntry> OnNewJournalEntryUnfiltered;      // UI. Called when a new journal entry is read.  Not filtered by history system
        public event Action<HistoryEntry> OnNewHistoryEntryUnfiltered;      // UI. Called when a new history entry is created and databases into it updated, but before adding.  Not filtered by history system
        public event Action<HistoryEntry, HistoryList> OnNewEntry;          // UI. MAJOR. UC. Mirrored. Called after HE has been added to the history list.  Post filtering
        public event Action<HistoryEntry, HistoryList> OnNewEntrySecond;    // UI. Called after OnNewEntry for more processing. Post filtering

        // If a UC is a Cursor Control type, then OnNewEntry should also fire the cursor control OnChangedSelection, OnTravelSelectionChanged after onNewEntry has been received by the cursor UC

        // IF a log print occurs

        public event Action<string, Color> OnNewLogEntry;                   // UI. MAJOR. UC. Mirrored. New log entry generated.

        // During a Close

        public event Action OnFinalClose;                                   // UI. Final close, in UI thread

        // During SYNC events

        public event Action OnSyncStarting;                                 // UI. EDSM sync starting
        public event Action<long,long> OnSyncComplete;                      // UI. SYNC has completed, full count, update count
        public event Action<int, string> OnReportSyncProgress;              // UI. SYNC progress reporter

        // Due to background taskc completing async to the rest

        public event Action<bool> OnExpeditionsDownloaded;                  // UI, true if changed entries
        public event Action OnHelpDownloaded;                               // UI

        #endregion

        #region Variables
        private string logtext = "";     // to keep in case of no logs..

        private EDJournalUIScanner journalmonitor;

        private Thread backgroundWorker;
        private Thread backgroundRefreshWorker;

        private ManualResetEvent closeRequested = new ManualResetEvent(false);
        private AutoResetEvent resyncRequestedEvent = new AutoResetEvent(false);

        #endregion

        #region Accessors
        private Action<Action> InvokeAsyncOnUiThread;
        #endregion


        #region Refreshing

        public void RefreshDisplays()
        {
            OnHistoryChange?.Invoke(history);
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
            TraceLog.LogFileWriterException += ex =>            // now we can attach the log writing highter into it
            {
                LogLineHighlight($"Log Writer Exception: {ex}");
            };

            EdsmLogFetcher = new EDSMLogFetcher(LogLine);
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
            if (!PendingClose)
            {
                PendingClose = true;
                EDDNSync.StopSync();
                EDSMJournalSync.StopSync();
                EliteDangerousCore.IGAU.IGAUSync.StopSync();
                EdsmLogFetcher.AsyncStop();
                journalmonitor.StopMonitor();
                LogLineHighlight("Closing down, please wait..".T(EDTx.EDDiscoveryController_CD));
                closeRequested.Set();
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
            // check first and download items

            string desigmapfile = Path.Combine(EDDOptions.Instance.AppDataDirectory, "bodydesignations.csv");

            if (!File.Exists(desigmapfile))
            {
                desigmapfile = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "bodydesignations.csv");
            }

            BodyDesignations.LoadBodyDesignationMap(desigmapfile);

            Debug.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Check systems");
            ReportSyncProgress("");

            bool checkGithub = EDDOptions.Instance.CheckGithubFiles;
            if (checkGithub)      // not normal in debug, due to git hub choking
            {
                DateTime lastdownloadtime = UserDatabase.Instance.GetSettingDate("DownloadFilesLastTime", DateTime.MinValue);

                if (DateTime.UtcNow - lastdownloadtime >= new TimeSpan(24, 0, 0))       // only update once per day
                {
                    // Expedition data
                    DownloadExpeditions(() => PendingClose);

                    // and Help files
                    DownloadHelp(() => PendingClose);

                    UserDatabase.Instance.PutSettingDate("DownloadFilesLastTime", DateTime.UtcNow);
                }
            }

            // if we have a gmo file, but its out of date, refresh it for next time, do it in background thread since not critical.
            string gmofile = Path.Combine(EDDOptions.Instance.AppDataDirectory, "galacticmapping.json");

            if (!EDDOptions.Instance.NoSystemsLoad && File.Exists(gmofile) && DateTime.UtcNow.Subtract(SystemsDatabase.Instance.GetEDSMGalMapLast()).TotalDays > 14 )
            {
                LogLine("Get galactic mapping from EDSM.".T(EDTx.EDDiscoveryController_EDSM));
                if (EDSMClass.DownloadGMOFileFromEDSM(gmofile))
                    SystemsDatabase.Instance.SetEDSMGalMapLast(DateTime.UtcNow);

            }

            Debug.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Check systems complete");

            SystemNoteClass.GetAllSystemNotes();

            LogLine("Loaded Notes, Bookmarks and Galactic mapping.".T(EDTx.EDDiscoveryController_LN));

            if (!EDDOptions.Instance.NoLoad)        // here in this thread, we do a refresh of history. 
            {
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

            System.Diagnostics.Debug.WriteLine("Background worker setting up refresh worker");

            backgroundRefreshWorker = new Thread(BackgroundHistoryRefreshWorkerThread) { Name = "Background Refresh Worker", IsBackground = true };
            backgroundRefreshWorker.Start();        // start the refresh worker, another thread which does subsequenct (not the primary one) refresh work in the background..

            try
            {
                if (!EDDOptions.Instance.NoSystemsLoad)
                {
                    DoPerformSync();        // this is done after the initial history load..
                }

                while (!PendingClose)
                {
                    int wh = WaitHandle.WaitAny(new WaitHandle[] { closeRequested, resyncRequestedEvent });

                    System.Diagnostics.Debug.WriteLine("Background worker kicked by " + wh);

                    if (PendingClose)
                        break;

                    if (wh == 1)
                    {
                        if (!EDDOptions.Instance.NoSystemsLoad && EDDConfig.Instance.EDSMDownload)      // if no system off, and EDSM download on
                            DoPerformSync();
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }

            backgroundRefreshWorker.Join();     // this should terminate due to closeRequested..

            System.Diagnostics.Debug.WriteLine("BW Refresh joined closing down background worker");

            // Now we have been ordered to close down, so go thru the process

            closeRequested.WaitOne();

            InvokeAsyncOnUiThread(() =>
            {
                System.Diagnostics.Debug.WriteLine("Call Final close");
                OnFinalClose?.Invoke();
            });
        }

        #endregion



        #region Logging
        public void LogLine(string text)
        {
            LogLineColor(text, ExtendedControls.Theme.Current.TextBlockColor);
        }

        public void LogLineHighlight(string text)
        {
            TraceLog.WriteLine(text);
            LogLineColor(text, ExtendedControls.Theme.Current.TextBlockHighlightColor);
        }

        public void LogLineSuccess(string text)
        {
            LogLineColor(text, ExtendedControls.Theme.Current.TextBlockSuccessColor);
        }

        public void LogLineColor(string text, Color color)
        {
            try
            {
                InvokeAsyncOnUiThread(() =>
                {
                    logtext += text + Environment.NewLine;      // keep this, may be the only log showing

                    OnNewLogEntry?.Invoke(text + Environment.NewLine, color);
                });
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("******* Exception trying to write to ui thread log");
            }
        }

        public void ReportSyncProgress(string message)
        {
            InvokeAsyncOnUiThread(() => OnReportSyncProgress?.Invoke(-1, message));
        }
        public void ReportRefreshProgress(int percent, string message)
        {
            InvokeAsyncOnUiThread(() => OnReportRefreshProgress?.Invoke(percent, message));
        }

        #endregion

        #region Aux file downloads

        // in its own thread..
        public void DownloadExpeditions(Func<bool> cancelRequested)
        {
            LogLine("Checking for new Expedition data".T(EDTx.EDDiscoveryController_EXPD));

            Task.Factory.StartNew(() =>
            {
                BaseUtils.GitHubClass github = new BaseUtils.GitHubClass(EDDiscovery.Properties.Resources.URLGithubDataDownload, LogLine);
                var files = github.ReadDirectory("Expeditions");
                if (files != null)        // may be empty, unlikely, but
                {
                    string expeditiondir = EDDOptions.Instance.ExpeditionsAppDirectory();

                    if (github.DownloadFiles(files, expeditiondir))
                    {
                        if (!cancelRequested())
                        {
                            bool changed = SavedRouteClass.UpdateDBFromExpeditionFiles(expeditiondir);
                            InvokeAsyncOnUiThread(() => { OnExpeditionsDownloaded?.Invoke(changed); });
                        }
                    }
                }
            });
        }

        public void DownloadHelp(Func<bool> cancelRequested)
        {
            Task.Factory.StartNew(() =>
            {
                string helpdir = EDDOptions.Instance.HelpDirectory();

                BaseUtils.GitHubClass github = new BaseUtils.GitHubClass(EDDiscovery.Properties.Resources.URLGithubDataDownload, LogLine);
                var files = github.ReadDirectory("Help");
                if (files != null)        // may be empty, unlikely, but
                {
                    if (github.DownloadFiles(files, helpdir))
                    {
                        if (!cancelRequested())
                        {
                            InvokeAsyncOnUiThread(() => { OnHelpDownloaded?.Invoke(); });
                        }
                    }
                }
            });
        }

        #endregion
    }
}
