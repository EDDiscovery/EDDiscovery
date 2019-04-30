/*
 * Copyright © 2015 - 2017 EDDiscovery development team
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

using EliteDangerousCore.EDSM;
using EliteDangerousCore.EDDN;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using BaseUtils;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using System.IO;

namespace EDDiscovery
{
    public partial class EDDiscoveryController
    {
        #region Variables
        public HistoryList history { get; private set; } = new HistoryList();
        public EDSMLogFetcher EdsmLogFetcher { get; private set; }
        public string LogText { get { return logtext; } }
        public bool PendingClose { get; private set; }           // we want to close boys!
        public GalacticMapping galacticMapping { get; private set; }
        public bool ReadyForFinalClose { get; private set; }
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

        public event Action OnBgSafeClose;                                  // BK. Background close, in BCK thread
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

        #region Refreshing

        public void RefreshDisplays()
        {
            OnHistoryChange?.Invoke(history);
        }

        public void RecalculateHistoryDBs()         // call when you need to recalc the history dbs - not the whole history. Use RefreshAsync for that
        {
            history.ProcessUserHistoryListEntries(h => h.EntryOrder);
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

            msg.Invoke("Checking Databases");

            Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Initializing database");
            SQLiteConnectionUser.Initialize();
            SQLiteConnectionSystem.Initialize();
            Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Database initialization complete");

            HttpCom.LogPath = logpath;

            Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Init config finished");

            Trace.WriteLine($"*** Elite Dangerous Discovery Initializing - {EDDOptions.Instance.VersionDisplayString}, Platform: {Environment.OSVersion.Platform.ToString()}");

            GlobalBookMarkList.LoadBookmarks();
            GlobalCaptainsLogList.LoadLog();

            msg.Invoke("Loading Icons");
            Icons.IconSet.ResetIcons();     // start with a clean slate loaded up from default icons

            msg.Invoke("Loading Configuration");
            EDDConfig.Instance.Update(false);
            EDDProfiles.Instance.LoadProfiles();

            string path = EDDOptions.Instance.IconsPath ?? System.IO.Path.Combine(EDDOptions.Instance.IconsAppDirectory(), "*.zip");

            Icons.IconSet.LoadIconPack(path, EDDOptions.Instance.AppDataDirectory, EDDOptions.ExeDirectory());
        }

// TBD why two inits - remove PostInit_Shown, call this instead, remove readyforinitialload.

        public void Init()      // ED Discovery calls this during its init
        {
            TraceLog.LogFileWriterException += ex =>            // now we can attach the log writing highter into it
            {
                LogLineHighlight($"Log Writer Exception: {ex}");
            };

            backgroundWorker = new Thread(BackgroundWorkerThread);
            backgroundWorker.IsBackground = true;
            backgroundWorker.Name = "Background Worker Thread";
            backgroundWorker.Start();                                   // TBD later, get rid of readyforInitialLoad, and start this thread at PostInit_Shown

            galacticMapping = new GalacticMapping();

            EdsmLogFetcher = new EDSMLogFetcher(LogLine);
            EdsmLogFetcher.OnDownloadedSystems += () => RefreshHistoryAsync();

            journalmonitor = new EDJournalClass(InvokeAsyncOnUiThread);
            journalmonitor.OnNewJournalEntry += NewEntry;
            journalmonitor.OnNewUIEvent += NewUIEvent;
        }

// TBD think this is also out of date.. DoRefreshHistory is waiting on this, its set at the end of main.. logic seems iffy

        public void InitComplete()          // called by EDD Init at end
        {
            initComplete.Set();    
        }

        public void PostInit_Loaded()       // called by EDDForm during loaded
        {
//TBD remove this step - its silly.  only different is writing the new commander to the db.. why?

            EDDConfig.Instance.Update();    // lost in the midst of time why  
        }

        public void PostInit_Shown()        // called by EDDForm during shown
        {
//TBD work
            readyForInitialLoad.Set();
        }


        #endregion

        #region Shutdown
        public void Shutdown()
        {
            if (!PendingClose)
            {
                PendingClose = true;
                EDDNSync.StopSync();
                EDSMJournalSync.StopSync();
                EdsmLogFetcher.AsyncStop();
                journalmonitor.StopMonitor();
                LogLineHighlight("Closing down, please wait..".Tx(this,"CD"));
                closeRequested.Set();
                journalqueuedelaytimer.Change(Timeout.Infinite, Timeout.Infinite);
                journalqueuedelaytimer.Dispose();
            }
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


        #region Variables
        private string logtext = "";     // to keep in case of no logs..

        private EDJournalClass journalmonitor;

        private Thread backgroundWorker;
        private Thread backgroundRefreshWorker;

        private ManualResetEvent closeRequested = new ManualResetEvent(false);
        private ManualResetEvent readyForInitialLoad = new ManualResetEvent(false);
        private ManualResetEvent initComplete = new ManualResetEvent(false);
        private AutoResetEvent resyncRequestedEvent = new AutoResetEvent(false);

        #endregion

        #region Accessors
        private Func<Color> GetNormalTextColour;
        private Func<Color> GetHighlightTextColour;
        private Func<Color> GetSuccessTextColour;
        private Action<Action> InvokeAsyncOnUiThread;
        #endregion

        #region Background Worker Thread - kicked off by Controller.Init, which itself is kicked by DiscoveryForm Init.


// I think we rework so we have a SyncBackgroundworker, and a refresh background worker.
// loading of history is done on refresh one..


        private void BackgroundWorkerThread()     
        {
            readyForInitialLoad.WaitOne();      // wait for shown in form

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

                DateTime galmaptime = SQLiteConnectionSystem.GetSettingDate("EDSMGalMapLast", DateTime.MinValue); // Latest time from RW file.

                if (DateTime.Now.Subtract(galmaptime).TotalDays > 14 || !galacticMapping.GalMapFilePresent())  // Over 14 days do a sync from EDSM for galmap
                {
                    LogLine("Get galactic mapping from EDSM.".Tx(this, "EDSM"));
                    if (galacticMapping.DownloadFromEDSM())
                        SQLiteConnectionSystem.PutSettingDate("EDSMGalMapLast", DateTime.UtcNow);
                }

                Debug.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Check systems complete");
            }

            galacticMapping.ParseData();                            // at this point, gal map data has been uploaded - get it into memory
            SystemCache.AddToAutoCompleteList(galacticMapping.GetGMONames());
            SystemNoteClass.GetAllSystemNotes();

            LogLine("Loaded Notes, Bookmarks and Galactic mapping.".Tx(this, "LN"));

            if (PendingClose) return;

            if (EliteDangerousCore.EDDN.EDDNClass.CheckforEDMC()) // EDMC is running
            {
                if (EDCommander.Current.SyncToEddn)  // Both EDD and EDMC should not sync to EDDN.
                {
                    LogLineHighlight("EDDiscovery and EDMarketConnector should not both sync to EDDN. Stop EDMC or uncheck 'send to EDDN' in settings tab!".Tx(this, "EDMC"));
                }
            }

            if (!EDDOptions.Instance.NoLoad)        // here in this thread, we do a refresh of history. 
            {
                LogLine("Reading travel history".Tx(this, "RTH"));
                DoRefreshHistory(new RefreshWorkerArgs { CurrentCommander = EDCommander.CurrentCmdrID });       // kick the background refresh worker thread into action
            }

            if (PendingClose) return;

            CheckForSync();     // see if any EDSM/EDDB sync is needed

            if (PendingClose) return;

            // Now stay in loop services stuff

            backgroundRefreshWorker = new Thread(BackgroundHistoryRefreshWorkerThread) { Name = "Background Refresh Worker", IsBackground = true };
            backgroundRefreshWorker.Start();        // start the refresh worker, another thread which does subsequenct (not the primary one) refresh work in the background..

            try
            {
                if (!EDDOptions.Instance.NoSystemsLoad && EDDConfig.Instance.EDSMEDDBDownload)      // if no system off, and EDSM download on
                    DoPerformSync();        // this is done after the initial history load..

                while (!PendingClose)
                {
                    int wh = WaitHandle.WaitAny(new WaitHandle[] { closeRequested, resyncRequestedEvent });

                    if (PendingClose) break;

                    switch (wh)
                    {
                        case 0:  // Close Requested
                            break;
                        case 1:  // Resync Requested
                            if (!EDDOptions.Instance.NoSystemsLoad && EDDConfig.Instance.EDSMEDDBDownload)      // if no system off, and EDSM download on
                                DoPerformSync();
                            break;
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }

            backgroundRefreshWorker.Join();

            // Now we have been ordered to close down, so go thru the process

            closeRequested.WaitOne();      

            OnBgSafeClose?.Invoke();
            ReadyForFinalClose = true;
            InvokeAsyncOnUiThread(() =>
            {
                OnFinalClose?.Invoke();
            });
        }

        #endregion

        #region Aux file downloads

        // in its own thread..
        public void DownloadMaps(Func<bool> cancelRequested)
        {
            LogLine("Checking for new EDDiscovery maps".Tx(this,"Maps"));

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
            LogLine("Checking for new Expedition data".Tx(this,"EXPD"));

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
            LogLine("Checking for new Exploration data".Tx(this, "EXPL"));

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
