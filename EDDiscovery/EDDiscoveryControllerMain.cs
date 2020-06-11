/*
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

        public GalacticMapping galacticMapping { get; private set; }

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

                                                                            // Next two ONLY called if its for the current commander, and its not a screened out event (uievent)
        public event Action<HistoryEntry, HistoryList> OnNewEntry;          // UI. MAJOR. UC. Mirrored. Current commander. Called before OnNewJournalEntry, when NewEntry is called with a new item for the CURRENT commander
        public event Action<HistoryEntry, HistoryList> OnNewEntrySecond;    // UI. Current commander. After onNewEntry, Use if you want to do something after the main UI has been updated

        // If a UC is a Cursor Control type, then OnNewEntry should also fire the cursor control OnChangedSelection, OnTravelSelectionChanged after onNewEntry has been received by the cursor UC

        public event Action<JournalEntry> OnNewJournalEntry;                // UI. MAJOR. UC. Mirrored. Called after OnNewEntry, and when ANY new journal entry is created by the journal monitor

        // IF a log print occurs

        public event Action<string, Color> OnNewLogEntry;                   // UI. MAJOR. UC. Mirrored. New log entry generated.

        // During a Close

        public event Action OnFinalClose;                                   // UI. Final close, in UI thread

        // During SYNC events

        public event Action OnSyncStarting;                                 // UI. EDSM/EDDB sync starting
        public event Action OnSyncComplete;                                 // UI. SYNC has completed
        public event Action<int, string> OnReportSyncProgress;              // UI. SYNC progress reporter

        // Due to background taskc completing async to the rest

        public event Action OnMapsDownloaded;                               // UI
        public event Action<bool> OnExpeditionsDownloaded;                  // UI, true if changed entries
        public event Action OnExplorationDownloaded;                        // UI

        #endregion

        #region Variables
        private string logtext = "";     // to keep in case of no logs..

        private EDJournalClass journalmonitor;

        private Thread backgroundWorker;
        private Thread backgroundRefreshWorker;

        private ManualResetEvent closeRequested = new ManualResetEvent(false);
        private AutoResetEvent resyncRequestedEvent = new AutoResetEvent(false);

        #endregion

        #region Accessors
        private Func<Color> GetNormalTextColour;
        private Func<Color> GetHighlightTextColour;
        private Func<Color> GetSuccessTextColour;
        private Action<Action> InvokeAsyncOnUiThread;
        #endregion


        #region Refreshing

        public void RefreshDisplays()
        {
            OnHistoryChange?.Invoke(history);
        }

        #endregion

        #region Initialisation

        public EDDiscoveryController(Func<Color> getNormalTextColor, Func<Color> getHighlightTextColor, Func<Color> getSuccessTextColor, Action<Action> invokeAsyncOnUiThread)
        {
            GetNormalTextColour = getNormalTextColor;
            GetHighlightTextColour = getHighlightTextColor;
            GetSuccessTextColour = getSuccessTextColor;
            InvokeAsyncOnUiThread = invokeAsyncOnUiThread;
            journalqueuedelaytimer = new Timer(DelayPlay, null, Timeout.Infinite, Timeout.Infinite);
        }

        public static void Initialize(Action<string> msg)    // called from EDDApplicationContext to initialize config and dbs
        {
            Thread.CurrentThread.Name = "EDD Main Thread";
            msg.Invoke("Checking Config");

            string logpath = EDDOptions.Instance.LogAppDirectory();

            BaseUtils.LogClean.DeleteOldLogFiles(logpath, "*.hlog", 2, 256);        // Remove hlogs faster
            BaseUtils.LogClean.DeleteOldLogFiles(logpath, "*.log", 10, 256);        

            if (!Debugger.IsAttached || EDDOptions.Instance.TraceLog != null)
            {
                TraceLog.RedirectTrace(logpath, EDDOptions.Instance.TraceLog);
            }

            if (!Debugger.IsAttached || EDDOptions.Instance.LogExceptions)
            {
                ExceptionCatcher.RedirectExceptions(Properties.Resources.URLProjectFeedback);
            }

            if (EDDOptions.Instance.LogExceptions)
            {
                FirstChanceExceptionCatcher.RegisterFirstChanceExceptionHandler();
            }

            if (EDDOptions.Instance.BackgroundPriority)
            {
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Idle;
            }
            else if (EDDOptions.Instance.LowPriority)
            {
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.BelowNormal;
            }

            if (EDDOptions.Instance.ForceTLS12)
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11;
            }

            msg.Invoke("Checking Databases");

            Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Initializing database");

            UserDatabase.Instance.Start("UserDB");
            SystemsDatabase.Instance.Start("SystemDB");
            UserDatabase.Instance.Initialize();
            SystemsDatabase.Instance.Initialize();

            Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Database initialization complete");

            HttpCom.LogPath = logpath;

            Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Init config finished");

            Trace.WriteLine($"*** Elite Dangerous Discovery Initializing - {EDDOptions.Instance.VersionDisplayString}, Platform: {Environment.OSVersion.Platform.ToString()}");

            GlobalBookMarkList.LoadBookmarks();
            GlobalCaptainsLogList.LoadLog();

            msg.Invoke("Loading Icons");
            EDDiscovery.Icons.ForceInclusion.Include();      // Force the assembly into the project by a empty call
            BaseUtils.Icons.IconSet.CreateSingleton();
            System.Reflection.Assembly iconasm = BaseUtils.ResourceHelpers.GetAssemblyByName("EDDiscovery.Icons");
            BaseUtils.Icons.IconSet.Instance.LoadIconsFromAssembly(iconasm);
            BaseUtils.Icons.IconSet.Instance.AddAlias("settings", "Controls.Main.Tools.Settings");             // from use by action system..
            BaseUtils.Icons.IconSet.Instance.AddAlias("missioncompleted", "Journal.MissionCompleted");
            BaseUtils.Icons.IconSet.Instance.AddAlias("speaker", "Legacy.speaker");
            BaseUtils.Icons.IconSet.Instance.AddAlias("Default", "Legacy.star");        // MUST be present

            msg.Invoke("Loading Configuration");
            EDDConfig.Instance.Update(false);
            EDDProfiles.Instance.LoadProfiles(EDDOptions.Instance.Profile);

            string path = EDDOptions.Instance.IconsPath ?? System.IO.Path.Combine(EDDOptions.Instance.IconsAppDirectory(), "*.zip");
            BaseUtils.Icons.IconSet.Instance.LoadIconPack(path, EDDOptions.Instance.AppDataDirectory, EDDOptions.ExeDirectory());
        }

        public void Init()      // ED Discovery calls this during its init
        {
            TraceLog.LogFileWriterException += ex =>            // now we can attach the log writing highter into it
            {
                LogLineHighlight($"Log Writer Exception: {ex}");
            };

            galacticMapping = new GalacticMapping();

            EdsmLogFetcher = new EDSMLogFetcher(LogLine);
            EdsmLogFetcher.OnDownloadedSystems += () => RefreshHistoryAsync();

            journalmonitor = new EDJournalClass(InvokeAsyncOnUiThread);
            journalmonitor.OnNewJournalEntry += NewEntry;
            journalmonitor.OnNewUIEvent += NewUIEvent;
        }

        public void PostInit_Shown()        // called by EDDForm during shown
        {
            EDDConfig.Instance.Update();    // lost in the midst of time why  

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

        #endregion

        #region Background Worker Thread - kicked off by PostInit_Shown
        private void BackgroundWorkerThread()
        {
            // check first and download items

            StarScan.LoadBodyDesignationMap();

            Debug.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Check systems");
            ReportSyncProgress("");

            bool checkGithub = EDDOptions.Instance.CheckGithubFiles;
            if (checkGithub)      // not normall in debug, due to git hub chokeing
            {
                // Async load of maps in another thread
                DownloadMaps(() => PendingClose);

                // and Expedition data
                DownloadExpeditions(() => PendingClose);

                // and Exploration data
                DownloadExploration(() => PendingClose);
            }

            if (!EDDOptions.Instance.NoSystemsLoad)
            {
                // New Galmap load - it was not doing a refresh if EDSM sync kept on happening. Now has its own timer

                DateTime galmaptime = SystemsDatabase.Instance.GetEDSMGalMapLast(); // Latest time from RW file.

                if (DateTime.Now.Subtract(galmaptime).TotalDays > 14 || !galacticMapping.GalMapFilePresent())  // Over 14 days do a sync from EDSM for galmap
                {
                    LogLine("Get galactic mapping from EDSM.".T(EDTx.EDDiscoveryController_EDSM));
                    if (galacticMapping.DownloadFromEDSM())
                        SystemsDatabase.Instance.SetEDSMGalMapLast(DateTime.UtcNow);
                }

                Debug.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Check systems complete");
            }

            galacticMapping.ParseData();                            // at this point, gal map data has been uploaded - get it into memory
            SystemCache.AddToAutoCompleteList(galacticMapping.GetGMONames());
            SystemNoteClass.GetAllSystemNotes();

            LogLine("Loaded Notes, Bookmarks and Galactic mapping.".T(EDTx.EDDiscoveryController_LN));

            if (EliteDangerousCore.EDDN.EDDNClass.CheckforEDMC()) // EDMC is running
            {
                if (EDCommander.Current.SyncToEddn)  // Both EDD and EDMC should not sync to EDDN.
                {
                    LogLineHighlight("EDDiscovery and EDMarketConnector should not both sync to EDDN. Stop EDMC or uncheck 'send to EDDN' in settings tab!".T(EDTx.EDDiscoveryController_EDMC));
                }
            }

            if (!EDDOptions.Instance.NoLoad)        // here in this thread, we do a refresh of history. 
            {
                LogLine("Reading travel history".T(EDTx.EDDiscoveryController_RTH));

                if (EDDOptions.Instance.Commander != null)
                {
                    EDCommander switchto = EDCommander.GetCommander(EDDOptions.Instance.Commander);
                    if (switchto != null)
                        EDCommander.CurrentCmdrID = switchto.Nr;
                }

                DoRefreshHistory(new RefreshWorkerArgs { CurrentCommander = EDCommander.CurrentCmdrID });       // kick the background refresh worker thread into action
            }

            CheckForSync();     // see if any EDSM/EDDB sync is needed - this just sets some variables up

            System.Diagnostics.Debug.WriteLine("Background worker setting up refresh worker");

            backgroundRefreshWorker = new Thread(BackgroundHistoryRefreshWorkerThread) { Name = "Background Refresh Worker", IsBackground = true };
            backgroundRefreshWorker.Start();        // start the refresh worker, another thread which does subsequenct (not the primary one) refresh work in the background..

            try
            {
                if (!EDDOptions.Instance.NoSystemsLoad)
                {
                    SystemsDatabase.Instance.WithReadWrite(() => DoPerformSync());        // this is done after the initial history load..
                }

                SystemsDatabase.Instance.SetReadOnly();

                while (!PendingClose)
                {
                    int wh = WaitHandle.WaitAny(new WaitHandle[] { closeRequested, resyncRequestedEvent });

                    System.Diagnostics.Debug.WriteLine("Background worker kicked by " + wh);

                    if (PendingClose)
                        break;

                    if (wh == 1)
                    {
                        if (!EDDOptions.Instance.NoSystemsLoad && EDDConfig.Instance.EDSMEDDBDownload)      // if no system off, and EDSM download on
                            SystemsDatabase.Instance.WithReadWrite(() => DoPerformSync());
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }

            backgroundRefreshWorker.Join();     // this should terminate due to closeRequested..

            System.Diagnostics.Debug.WriteLine("BW Refresh joined");

            // Now we have been ordered to close down, so go thru the process

            closeRequested.WaitOne();

            InvokeAsyncOnUiThread(() =>
            {
                System.Diagnostics.Debug.WriteLine("Final close");
                OnFinalClose?.Invoke();
            });
        }

        #endregion



        #region Logging
        public void LogLine(string text)
        {
            LogLineColor(text, GetNormalTextColour());
        }

        public void LogLineHighlight(string text)
        {
            TraceLog.WriteLine(text);
            LogLineColor(text, GetHighlightTextColour());
        }

        public void LogLineSuccess(string text)
        {
            LogLineColor(text, GetSuccessTextColour());
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
        public void DownloadMaps(Func<bool> cancelRequested)
        {
            LogLine("Checking for new EDDiscovery maps".T(EDTx.EDDiscoveryController_Maps));

            Task.Factory.StartNew(() =>
            {
                BaseUtils.GitHubClass github = new BaseUtils.GitHubClass(EDDiscovery.Properties.Resources.URLGithubDataDownload, LogLine);
                var files = github.ReadDirectory("Maps/V1");
                if (files != null)
                {
                    string mapsdir = EDDOptions.Instance.MapsAppDirectory();

                    if ( github.DownloadFiles(files, mapsdir) )
                    {
                        if (!cancelRequested())
                            InvokeAsyncOnUiThread(() => { OnMapsDownloaded?.Invoke(); });
                    }
                }
            });
        }

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

        public void DownloadExploration(Func<bool> cancelRequested)
        {
            LogLine("Checking for new Exploration data".T(EDTx.EDDiscoveryController_EXPL));

            Task.Factory.StartNew(() =>
            {
                string explorationdir = EDDOptions.Instance.ExploreAppDirectory();

                BaseUtils.GitHubClass github = new BaseUtils.GitHubClass(EDDiscovery.Properties.Resources.URLGithubDataDownload, LogLine);
                var files = github.ReadDirectory("Exploration");
                if (files != null)        // may be empty, unlikely, but
                {
                    if (github.DownloadFiles(files, explorationdir))
                    {
                        if (!cancelRequested())
                        {
                            InvokeAsyncOnUiThread(() => { OnExplorationDownloaded?.Invoke(); });
                        }
                    }
                }
            });
        }

        #endregion
    }
}
