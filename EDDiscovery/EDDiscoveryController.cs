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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BaseUtils;
using EliteDangerousCore;
using EliteDangerousCore.DB;

namespace EDDiscovery
{
    public class EDDiscoveryController : IDiscoveryController
    {
        #region Public Interface
        #region Variables
        public HistoryList history { get; private set; } = new HistoryList();
        public EDSMSync EdsmSync { get; private set; }
        public EDSMLogFetcher EdsmLogFetcher { get; private set; }
        public string LogText { get { return logtext; } }
        public bool PendingClose { get; private set; }           // we want to close boys!
        public GalacticMapping galacticMapping { get; private set; }
        public bool ReadyForFinalClose { get; private set; }
        #endregion

        #region Events

        // IN ORDER OF CALLING DURING A REFRESH

        public event Action OnRefreshStarting;                              // UI. Called before worker thread starts, processing history (EDDiscoveryForm uses this to disable buttons and action refreshstart)
        public event Action OnRefreshCommanders;                            // UI. Called when refresh worker completes before final history is made (EDDiscoveryForm uses this to refresh commander stuff).  History is not valid here.
                                                                            // ALSO called if Loadgame is received.

        public event Action<HistoryList> OnHistoryChange;                   // UI. MAJOR. UC. Mirrored. Called AFTER history is complete, or via RefreshDisplays if a forced refresh is needed.  UC's use this
        public event Action OnRefreshComplete;                              // UI. Called AFTER history is complete.. Form uses this to know the whole process is over, and buttons may be turned on, actions may be run, etc
        public event Action OnInitialisationComplete;                       // UI.  Called AFTER first initial history load only

        // DURING A new Journal entry by the monitor, in order..

        public event Action<string, bool> OnNewUIEvent;                     // UI. MAJOR. UC. Mirrored. Always called irrespective of commander

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

        // During SYNC events and on start up

        public event Action OnInitialSyncComplete;                          // UI. Called during startup after CheckSystems done.
        public event Action OnSyncStarting;                                 // BK. EDSM/EDDB sync starting
        public event Action OnSyncComplete;                                 // BK. SYNC has completed
        public event Action<int, string> OnReportProgress;                  // UI. SYNC progress reporter

        #endregion

        #region Private vars
        private List<JournalEntry> journalqueue = new List<JournalEntry>();
        private System.Threading.Timer journalqueuedelaytimer;

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
            InitializeConfig();

            Trace.WriteLine($"*** Elite Dangerous Discovery Initializing - {EDDOptions.Instance.VersionDisplayString}, Platform: {Environment.OSVersion.Platform.ToString()}");

            msg.Invoke("Scanning Memory Banks");
            InitializeDatabases();

            msg.Invoke("Locating Crew Members");
            EDDConfig.Instance.Update(false);
        }

        public void Init()
        {
            if (!Debugger.IsAttached || EDDOptions.Instance.TraceLog)
            {
                TraceLog.LogFileWriterException += ex =>
                {
                    LogLineHighlight($"Log Writer Exception: {ex}");
                };
            }

            backgroundWorker = new Thread(BackgroundWorkerThread);
            backgroundWorker.IsBackground = true;
            backgroundWorker.Name = "Background Worker Thread";
            backgroundWorker.Start();

            galacticMapping = new GalacticMapping();

            EdsmSync = new EDSMSync(Logger);

            EdsmLogFetcher = new EDSMLogFetcher(LogLine);
            EdsmLogFetcher.OnDownloadedSystems += () => RefreshHistoryAsync();

            journalmonitor = new EDJournalClass(InvokeAsyncOnUiThread);
            journalmonitor.OnNewJournalEntry += NewEntry;
        }

        public void Logger(string s)
        {
            LogLine(s);
        }

        public void PostInit_Loaded()
        {
            EDDConfig.Instance.Update();
        }

        public void PostInit_Shown()
        {
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
                EdsmSync.StopSync();
                EdsmLogFetcher.AsyncStop();
                journalmonitor.StopMonitor();
                LogLineHighlight("Closing down, please wait..");
                Console.WriteLine("Close.. safe close launched");
                closeRequested.Set();
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

        public void ReportProgress(int percent, string message)
        {
            InvokeAsyncOnUiThread(() => OnReportProgress?.Invoke(percent, message));
        }
        #endregion

        #region History
        public bool RefreshHistoryAsync(string netlogpath = null, bool forcenetlogreload = false, bool forcejournalreload = false, bool checkedsm = false, int? currentcmdr = null)
        {
            if (PendingClose)
            {
                return false;
            }

            bool newrefresh = false;

            RefreshWorkerArgs curargs = refreshWorkerArgs;
            if (refreshRequestedFlag == 0)
            {
                if (curargs == null ||
                    curargs.CheckEdsm != checkedsm ||
                    curargs.ForceNetLogReload != forcenetlogreload ||
                    curargs.ForceJournalReload != forcejournalreload ||
                    curargs.CurrentCommander != (currentcmdr ?? history.CommanderId) ||
                    curargs.NetLogPath != netlogpath)
                {
                    newrefresh = true;
                }
            }

            if (Interlocked.CompareExchange(ref refreshRequestedFlag, 1, 0) == 0 || newrefresh)
            {
                refreshWorkerQueue.Enqueue(new RefreshWorkerArgs
                {
                    NetLogPath = netlogpath,
                    ForceNetLogReload = forcenetlogreload,
                    ForceJournalReload = forcejournalreload,
                    CheckEdsm = checkedsm,
                    CurrentCommander = currentcmdr ?? history.CommanderId
                });

                refreshRequested.Set();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RefreshDisplays()
        {
            OnHistoryChange?.Invoke(history);
        }

        public void RecalculateHistoryDBs()         // call when you need to recalc the history dbs - not the whole history. Use RefreshAsync for that
        {
            history.ProcessUserHistoryListEntries(h => h.EntryOrder);

            RefreshDisplays();
        }
        #endregion

        #region EDSM / EDDB
        public bool AsyncPerformSync(bool eddbsync = false, bool edsmsync = false)
        {
            if (Interlocked.CompareExchange(ref resyncRequestedFlag, 1, 0) == 0)
            {
                OnSyncStarting?.Invoke();
                syncstate.performeddbsync |= eddbsync;
                syncstate.performedsmsync |= edsmsync;
                resyncRequestedEvent.Set();
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #endregion  // MAJOR REGION

        #region Implementation
        #region Variables
        private string logtext = "";     // to keep in case of no logs..
        private event EventHandler HistoryRefreshed; // this is an internal hook

        private Task<bool> downloadMapsTask = null;

        private EDJournalClass journalmonitor;

        private ConcurrentQueue<RefreshWorkerArgs> refreshWorkerQueue = new ConcurrentQueue<RefreshWorkerArgs>();
        private EliteDangerousCore.EDSM.SystemClassEDSM.SystemsSyncState syncstate = new EliteDangerousCore.EDSM.SystemClassEDSM.SystemsSyncState();
        private RefreshWorkerArgs refreshWorkerArgs = new RefreshWorkerArgs();

        private Thread backgroundWorker;
        private Thread backgroundRefreshWorker;

        private ManualResetEvent closeRequested = new ManualResetEvent(false);
        private ManualResetEvent readyForInitialLoad = new ManualResetEvent(false);
        private ManualResetEvent readyForNewRefresh = new ManualResetEvent(false);
        private AutoResetEvent refreshRequested = new AutoResetEvent(false);
        private AutoResetEvent resyncRequestedEvent = new AutoResetEvent(false);
        private int refreshRequestedFlag = 0;
        private int resyncRequestedFlag = 0;
        #endregion

        #region Accessors
        private Func<Color> GetNormalTextColour;
        private Func<Color> GetHighlightTextColour;
        private Func<Color> GetSuccessTextColour;
        private Action<Action> InvokeAsyncOnUiThread;
        #endregion

        #region Initialization

        private static void InitializeDatabases()
        {
            Debug.WriteLine((Environment.TickCount % 100000) + " Initializing database");
            SQLiteConnectionUser.Initialize();
            SQLiteConnectionSystem.Initialize();
            Debug.WriteLine((Environment.TickCount % 100000) + " Database initialization complete");
        }

        private static void InitializeConfig()
        {
            EDDOptions.Instance.Init();

            if (EDDOptions.Instance.ReadJournal != null && File.Exists(EDDOptions.Instance.ReadJournal))
            {
                DebugCode.ReadCmdLineJournal(EDDOptions.Instance.ReadJournal);
            }

            string logpath = "";
            try
            {
                logpath = Path.Combine(EDDOptions.Instance.AppDataDirectory, "Log");
                if (!Directory.Exists(logpath))
                {
                    Directory.CreateDirectory(logpath);
                }

                TraceLog.logroot = EDDOptions.Instance.AppDataDirectory;
                TraceLog.urlfeedback = Properties.Resources.URLProjectFeedback;

                if (!Debugger.IsAttached || EDDOptions.Instance.TraceLog)
                {
                    TraceLog.Init();
                }

                if (EDDOptions.Instance.LogExceptions)
                {
                    TraceLog.RegisterFirstChanceExceptionHandler();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Unable to create the folder '{logpath}'");
                Trace.WriteLine($"Exception: {ex.Message}");
            }

            SQLiteConnectionUser.EarlyReadRegister();

            Debug.WriteLine((Environment.TickCount % 100000) + " Init config finished");
        }

        #endregion

        #region Initial Check Systems

        private void CheckSystems(Func<bool> cancelRequested, Action<int, string> reportProgress)  // ASYNC process, done via start up, must not be too slow.
        {
            Debug.WriteLine((Environment.TickCount % 100000) + " Check systems");
            reportProgress(-1, "");

            string rwsystime = SQLiteConnectionSystem.GetSettingString("EDSMLastSystems", "2000-01-01 00:00:00"); // Latest time from RW file.
            DateTime edsmdate;

            if (!DateTime.TryParse(rwsystime, CultureInfo.InvariantCulture, DateTimeStyles.None, out edsmdate))
            {
                edsmdate = new DateTime(2000, 1, 1);
            }

            if (DateTime.Now.Subtract(edsmdate).TotalDays > 7)  // Over 7 days do a sync from EDSM
            {
                // Also update galactic mapping from EDSM
                LogLine("Get galactic mapping from EDSM.");
                galacticMapping.DownloadFromEDSM();

                // Skip EDSM full update if update has been performed in last 4 days
                bool outoforder = SQLiteConnectionSystem.GetSettingBool("EDSMSystemsOutOfOrder", true);
                DateTime lastmod = outoforder ? SystemClassDB.GetLastSystemModifiedTime() : SystemClassDB.GetLastSystemModifiedTimeFast();

                if (DateTime.UtcNow.Subtract(lastmod).TotalDays > 4 ||
                    DateTime.UtcNow.Subtract(edsmdate).TotalDays > 28)
                {
                    syncstate.performedsmsync = true;
                }
                else
                {
                    SQLiteConnectionSystem.PutSettingString("EDSMLastSystems", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                }
            }

            if (!cancelRequested())
            {
                SQLiteConnectionSystem.CreateSystemsTableIndexes();
                galacticMapping.ParseData();                            // at this point, EDSM data is loaded..
                SystemClassDB.AddToAutoComplete(galacticMapping.GetGMONames());

                LogLine("Loaded Notes, Bookmarks and Galactic mapping.");

                string timestr = SQLiteConnectionSystem.GetSettingString("EDDBSystemsTime", "0");
                DateTime time = new DateTime(Convert.ToInt64(timestr), DateTimeKind.Utc);
                if (DateTime.UtcNow.Subtract(time).TotalDays > 6.5)     // Get EDDB data once every week.
                    syncstate.performeddbsync = true;
            }
            Debug.WriteLine((Environment.TickCount % 100000) + " Check systems complete");
        }

        #endregion

        #region Async EDSM/EDDB Full Sync

        private void DoPerformSync()
        {
            Debug.WriteLine((Environment.TickCount % 100000) + " Perform sync");
            try
            {
                EliteDangerousCore.EDSM.SystemClassEDSM.PerformSync(() => PendingClose, (p, s) => ReportProgress(p, s), LogLine, LogLineHighlight, syncstate);
            }
            catch (OperationCanceledException)
            {
                // Swallow Operation Cancelled exceptions
            }
            catch (Exception ex)
            {
                LogLineHighlight("Check Systems exception: " + ex.Message + Environment.NewLine + "Trace: " + ex.StackTrace);
            }

            InvokeAsyncOnUiThread(() => PerformSyncCompleted());
        }


        private void PerformSyncCompleted()
        {
            ReportProgress(-1, "");

            if (!PendingClose)
            {
                long totalsystems = SystemClassDB.GetTotalSystems();
                LogLineSuccess($"Loading completed, total of {totalsystems:N0} systems");

                if (syncstate.performhistoryrefresh)
                {
                    LogLine("Refresh due to updating systems");
                    HistoryRefreshed += HistoryFinishedRefreshing;
                    RefreshHistoryAsync();
                }

                OnSyncComplete?.Invoke();

                resyncRequestedFlag = 0;
            }
            Debug.WriteLine((Environment.TickCount % 100000) + " Perform sync completed");
        }

        private void HistoryFinishedRefreshing(object sender, EventArgs e)
        {
            HistoryRefreshed -= HistoryFinishedRefreshing;
            LogLine("Refreshing complete.");
            Debug.WriteLine((Environment.TickCount % 100000) + " Refresh complete");

            if (syncstate.syncwasfirstrun)
            {
                LogLine("EDSM and EDDB update complete. Please restart ED Discovery to complete the synchronisation ");
            }
            else if (syncstate.syncwaseddboredsm)
                LogLine("EDSM and/or EDDB update complete.");
        }

        #endregion

        #region Update Data

        protected class RefreshWorkerArgs
        {
            public string NetLogPath;
            public bool ForceNetLogReload;
            public bool ForceJournalReload;
            public bool CheckEdsm;
            public int CurrentCommander;
        }

        private void DoRefreshHistory(RefreshWorkerArgs args)
        {
            HistoryList hist = null;
            try
            {
                refreshWorkerArgs = args;
                Debug.WriteLine((Environment.TickCount % 100000) + " Load history");
                hist = HistoryList.LoadHistory(journalmonitor, () => PendingClose, (p, s) => ReportProgress(p, $"Processing log file {s}"), args.NetLogPath, 
                    args.ForceJournalReload, args.ForceJournalReload, args.CheckEdsm, args.CurrentCommander , EDDConfig.Instance.ShowUIEvents );
                Debug.WriteLine((Environment.TickCount % 100000) + " Load history complete");
            }
            catch (Exception ex)
            {
                LogLineHighlight("History Refresh Error: " + ex);
            }

            InvokeAsyncOnUiThread(() => RefreshHistoryWorkerCompleted(hist));
        }

        private void RefreshHistoryWorkerCompleted(HistoryList hist)
        {
            if (!PendingClose)
            {
                if (hist != null)
                {
                    history.Copy(hist);

                    OnRefreshCommanders?.Invoke();

                    EdsmLogFetcher.StopCheck();


                    ReportProgress(-1, "");
                    LogLine("Refresh Complete.");

                    RefreshDisplays();
                }

                HistoryRefreshed?.Invoke(this, EventArgs.Empty);        // Internal hook call

                journalmonitor.StartMonitor();

                OnRefreshComplete?.Invoke();                            // History is completed

                if (history.CommanderId >= 0)
                    EdsmLogFetcher.Start(EDCommander.Current);

                refreshRequestedFlag = 0;
                readyForNewRefresh.Set();

                Debug.WriteLine((Environment.TickCount % 100000) + " refresh history complete");
            }
        }

        #endregion

        #region New Entry with merge

        public void NewEntry(JournalEntry je)        // on UI thread. hooked into journal monitor and receives new entries.. Also call if you programatically add an entry
        {
            Debug.Assert(System.Windows.Forms.Application.MessageLoop);

            int playdelay = HistoryList.MergeTypeDelay(je); // see if there is a delay needed..

            if (playdelay > 0)  // if delaying to see if a companion event occurs. add it to list. Set timer so we pick it up
            {
                System.Diagnostics.Debug.WriteLine(Environment.TickCount + " Delay Play queue " + je.EventTypeID + " Delay for " + playdelay);
                journalqueue.Add(je);
                journalqueuedelaytimer.Change(playdelay, Timeout.Infinite);
            }
            else
            {
                journalqueuedelaytimer.Change(Timeout.Infinite, Timeout.Infinite);  // stop the timer, but if it occurs before this, not the end of the world
                journalqueue.Add(je);  // add it to the play list.
                //System.Diagnostics.Debug.WriteLine(Environment.TickCount + " No delay, issue " + je.EventTypeID );
                PlayJournalList();    // and play
            }
        }

        public void PlayJournalList()                 // play delay list out..
        {
            Debug.Assert(System.Windows.Forms.Application.MessageLoop);
            //System.Diagnostics.Debug.WriteLine(Environment.TickCount + " Play out list");

            JournalEntry prev = null;  // we start afresh from the point of merging so we don't merge with previous ones already shown

            foreach (JournalEntry je in journalqueue)
            {
                if (!HistoryList.MergeEntries(prev, je))                // if not merged
                {
                    if (prev != null)                       // no merge, so if we have a merge candidate on top, run actions on it.
                        ActionEntry(prev);

                    prev = je;                              // record
                }
            }

            if (prev != null)                               // any left.. action it
                ActionEntry(prev);

            journalqueue.Clear();
        }

        void ActionEntry(JournalEntry je)               // issue the JE to the system
        {
            if (je.IsUIEvent)            // give windows time to set up for OnNewEvent, and tell them if its coming via showuievents
            {
                if (je is EliteDangerousCore.JournalEvents.JournalMusic)
                    OnNewUIEvent?.Invoke((je as EliteDangerousCore.JournalEvents.JournalMusic).MusicTrack, EDDConfig.Instance.ShowUIEvents);
            }

            OnNewJournalEntry?.Invoke(je);          // Always call this on all entries...

            // filter out commanders, and filter out any UI events
            if (je.CommanderId == history.CommanderId && (!je.IsUIEvent || EDDConfig.Instance.ShowUIEvents))  
            {
                HistoryEntry he = history.AddJournalEntry(je, h => LogLineHighlight(h));        // add a new one on top
                //System.Diagnostics.Debug.WriteLine("Add HE " + he.EventSummary);
                OnNewEntry?.Invoke(he, history);            // major hook
                OnNewEntrySecond?.Invoke(he, history);      // secondary hook..
            }

            if (je.EventTypeID == JournalTypeEnum.LoadGame) // and issue this on Load game
            {
                OnRefreshCommanders?.Invoke();
            }
        }

        public void DelayPlay(Object s)             // timeout after play delay.. 
        {
            System.Diagnostics.Debug.WriteLine(Environment.TickCount + " Delay Play timer executed");
            journalqueuedelaytimer.Change(Timeout.Infinite, Timeout.Infinite);
            InvokeAsyncOnUiThread(() =>
            {
                PlayJournalList();
            });
        }

        #endregion

        #region Background Worker Threads
        private void BackgroundWorkerThread()
        {
            readyForInitialLoad.WaitOne();

            BackgroundInit();

            if (!PendingClose)
            {
                backgroundRefreshWorker = new Thread(BackgroundRefreshWorkerThread) { Name = "Background Refresh Worker", IsBackground = true };
                backgroundRefreshWorker.Start();

                try
                {
                    if (!EDDOptions.Instance.NoSystemsLoad)
                        DoPerformSync();

                    while (!PendingClose)
                    {
                        int wh = WaitHandle.WaitAny(new WaitHandle[] { closeRequested, resyncRequestedEvent });

                        if (PendingClose) break;

                        switch (wh)
                        {
                            case 0:  // Close Requested
                                break;
                            case 1:  // Resync Requested
                                DoPerformSync();
                                break;
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                }

                backgroundRefreshWorker.Join();
            }

            closeRequested.WaitOne();

            OnBgSafeClose?.Invoke();
            ReadyForFinalClose = true;
            InvokeAsyncOnUiThread(() =>
            {
                OnFinalClose?.Invoke();
            });
        }


        private void BackgroundInit()
        {
            StarScan.LoadBodyDesignationMap();
            MaterialCommodityDB.SetUpInitialTable();

            if (!EDDOptions.Instance.NoSystemsLoad)
            {
                downloadMapsTask = FGEImage.DownloadMaps(this, () => PendingClose, LogLine, LogLineHighlight);
                CheckSystems(() => PendingClose, (p, s) => ReportProgress(p, s));
            }

            SystemNoteClass.GetAllSystemNotes();                                // fill up memory with notes, bookmarks, galactic mapping
            BookmarkClass.GetAllBookmarks();

            ReportProgress(-1, "");
            InvokeAsyncOnUiThread(() => OnInitialSyncComplete?.Invoke());

            if (PendingClose) return;

            if (EliteDangerousCore.EDDN.EDDNClass.CheckforEDMC()) // EDMC is running
            {
                if (EDCommander.Current.SyncToEddn)  // Both EDD and EDMC should not sync to EDDN.
                {
                    LogLineHighlight("EDDiscovery and EDMarketConnector should not both sync to EDDN. Stop EDMC or uncheck 'send to EDDN' in settings tab!");
                }
            }

            if (PendingClose) return;
            LogLine("Reading travel history");

            if (!EDDOptions.Instance.NoLoad)
            {
                DoRefreshHistory(new RefreshWorkerArgs { CurrentCommander = EDCommander.CurrentCmdrID });
            }

            if (PendingClose) return;

            if (syncstate.performeddbsync || syncstate.performedsmsync)
            {
                string databases = (syncstate.performedsmsync && syncstate.performeddbsync) ? "EDSM and EDDB" : ((syncstate.performedsmsync) ? "EDSM" : "EDDB");

                LogLine("ED Discovery will now synchronise to the " + databases + " databases to obtain star information." + Environment.NewLine +
                                "This will take a while, up to 15 minutes, please be patient." + Environment.NewLine +
                                "Please continue running ED Discovery until refresh is complete.");
            }
            InvokeAsyncOnUiThread(() => OnInitialisationComplete?.Invoke());
        }

        private void BackgroundRefreshWorkerThread()
        {
            WaitHandle.WaitAny(new WaitHandle[] { closeRequested, readyForNewRefresh }); // Wait to be ready for new refresh after initial refresh
            while (!PendingClose)
            {
                int wh = WaitHandle.WaitAny(new WaitHandle[] { closeRequested, refreshRequested });
                RefreshWorkerArgs argstemp = null;
                RefreshWorkerArgs args = null;

                if (PendingClose) break;

                switch (wh)
                {
                    case 0:  // Close Requested
                        break;
                    case 1:  // Refresh Requested
                        journalmonitor.StopMonitor();          // this is called by the foreground.  Ensure background is stopped.  Foreground must restart it.
                        EdsmLogFetcher.AsyncStop();     
                        InvokeAsyncOnUiThread(() =>
                        {
                            OnRefreshStarting?.Invoke();
                        });


                        while (refreshWorkerQueue.TryDequeue(out argstemp)) // Get the most recent refresh
                        {
                            args = argstemp;
                        }

                        if (args != null)
                        {
                            readyForNewRefresh.Reset();
                            DoRefreshHistory(args);
                            WaitHandle.WaitAny(new WaitHandle[] { closeRequested, readyForNewRefresh }); // Wait to be ready for new refresh
                        }
                        break;
                }
            }
        }

        #endregion

        #endregion

    }
}


