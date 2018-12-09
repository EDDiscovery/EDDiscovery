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

namespace EDDiscovery
{
    public class EDDiscoveryController
    {
        #region Public Interface
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

        #region Private vars
        private Queue<JournalEntry> journalqueue = new Queue<JournalEntry>();
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

            EDDOptions.Instance.Init();

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

            HttpCom.LogPath = logpath;

            SQLiteConnectionUser.EarlyReadRegister();

            Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Init config finished");

            Trace.WriteLine($"*** Elite Dangerous Discovery Initializing - {EDDOptions.Instance.VersionDisplayString}, Platform: {Environment.OSVersion.Platform.ToString()}");

            msg.Invoke("Scanning Memory Banks");
            InitializeDatabases();

            GlobalBookMarkList.LoadBookmarks();

            msg.Invoke("Locating Crew Members");
            EDDConfig.Instance.Update(false);
            EDDProfiles.Instance.LoadProfiles();

            msg.Invoke("Decoding Symbols");
            Icons.IconSet.ResetIcons();     // start with a clean slate loaded up from default icons

            string path = EDDOptions.Instance.IconsPath ?? System.IO.Path.Combine(EDDOptions.Instance.IconsAppDirectory(), "*.zip");

            Icons.IconSet.LoadIconPack(path, EDDOptions.Instance.AppDataDirectory, EDDOptions.ExeDirectory());
        }

        public void Init()      // ED Discovery calls this during its init
        {
            TraceLog.LogFileWriterException += ex =>            // now we can attach the log writing highter into it
            {
                LogLineHighlight($"Log Writer Exception: {ex}");
            };

            backgroundWorker = new Thread(BackgroundWorkerThread);
            backgroundWorker.IsBackground = true;
            backgroundWorker.Name = "Background Worker Thread";
            backgroundWorker.Start();

            galacticMapping = new GalacticMapping();

            EdsmLogFetcher = new EDSMLogFetcher(LogLine);
            EdsmLogFetcher.OnDownloadedSystems += () => RefreshHistoryAsync();

            journalmonitor = new EDJournalClass(InvokeAsyncOnUiThread);
            journalmonitor.OnNewJournalEntry += NewEntry;
            journalmonitor.OnNewUIEvent += NewUIEvent;
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

        public void InitComplete()
        {
            initComplete.Set();
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

        public void ReportSyncProgress(int percent, string message)
        {
            InvokeAsyncOnUiThread(() => OnReportSyncProgress?.Invoke(percent, message));
        }
        public void ReportRefreshProgress(int percent, string message)
        {
            InvokeAsyncOnUiThread(() => OnReportRefreshProgress?.Invoke(percent, message));
        }

        #endregion

        #region History

        // indicate change commander, indicate netlogpath load (with forced refresh), indicate forced journal load

        public bool RefreshHistoryAsync(string netlogpath = null, bool forcenetlogreload = false, bool forcejournalreload = false, int? currentcmdr = null)
        {
            if (PendingClose)
            {
                return false;
            }

            bool newrefresh = false;

            RefreshWorkerArgs curargs = refreshWorkerArgs;
            if (refreshHistoryRequestedFlag == 0)                                                           // if we are not operating
            {
                if (curargs == null ||                                                                      // and we have mateirlally changed comething important
                    curargs.ForceNetLogReload != forcenetlogreload ||
                    curargs.ForceJournalReload != forcejournalreload ||
                    curargs.CurrentCommander != (currentcmdr ?? history.CommanderId) ||
                    curargs.NetLogPath != netlogpath)
                {
                    newrefresh = true;                                                                      // we queue the refresh, even if we have a async refresh pending..
                }
            }

            if (Interlocked.CompareExchange(ref refreshHistoryRequestedFlag, 1, 0) == 0 || newrefresh)      // set the refresh requested to 1 in all circumstances, to stop a 
            {
                refreshWorkerQueue.Enqueue(new RefreshWorkerArgs
                {
                    NetLogPath = netlogpath,
                    ForceNetLogReload = forcenetlogreload,
                    ForceJournalReload = forcejournalreload,
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
            OnHistoryChange?.Invoke(history);
        }

        #endregion

        #region EDSM / EDDB
        public bool AsyncPerformSync(bool eddbsync = false, bool edsmfullsync = false)      // UI thread.
        {
            Debug.Assert(System.Windows.Forms.Application.MessageLoop);

            if (Interlocked.CompareExchange(ref resyncEDSMEDDBRequestedFlag, 1, 0) == 0)
            {
                syncstate.perform_eddb_sync |= eddbsync;
                syncstate.perform_edsm_fullsync |= edsmfullsync;
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

        private EDJournalClass journalmonitor;

        private RefreshWorkerArgs refreshWorkerArgs = new RefreshWorkerArgs();
        private ConcurrentQueue<RefreshWorkerArgs> refreshWorkerQueue = new ConcurrentQueue<RefreshWorkerArgs>();           // QUEUE of refreshes pending, each with their own args..
        private int refreshHistoryRequestedFlag = 0;            // flag gets set during History refresh, cleared at end, interlocked exchange during request..

        private EliteDangerousCore.EDSM.SystemClassEDSM.SystemsSyncState syncstate = new EliteDangerousCore.EDSM.SystemClassEDSM.SystemsSyncState();

        private Thread backgroundWorker;
        private Thread backgroundRefreshWorker;

        private ManualResetEvent closeRequested = new ManualResetEvent(false);
        private ManualResetEvent readyForInitialLoad = new ManualResetEvent(false);
        private ManualResetEvent initComplete = new ManualResetEvent(false);
        private ManualResetEvent readyForNewRefresh = new ManualResetEvent(false);
        private AutoResetEvent refreshRequested = new AutoResetEvent(false);
        private AutoResetEvent resyncRequestedEvent = new AutoResetEvent(false);

        private int resyncEDSMEDDBRequestedFlag = 0;            // flag gets set during EDSM refresh, cleared at end, interlocked exchange during request..
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
            Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Initializing database");
            SQLiteConnectionUser.Initialize();
            SQLiteConnectionSystem.Initialize();
            Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Database initialization complete");
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
                journalqueue.Enqueue(je);
                journalqueuedelaytimer.Change(playdelay, Timeout.Infinite);
            }
            else
            {
                journalqueuedelaytimer.Change(Timeout.Infinite, Timeout.Infinite);  // stop the timer, but if it occurs before this, not the end of the world
                journalqueue.Enqueue(je);  // add it to the play list.
                //System.Diagnostics.Debug.WriteLine(Environment.TickCount + " No delay, issue " + je.EventTypeID );
                PlayJournalList();    // and play
            }
        }

        public void PlayJournalList()                 // play delay list out..
        {
            Debug.Assert(System.Windows.Forms.Application.MessageLoop);
            //System.Diagnostics.Debug.WriteLine(Environment.TickCount + " Play out list");

            JournalEntry prev = null;  // we start afresh from the point of merging so we don't merge with previous ones already shown

            while( journalqueue.Count > 0 )
            {
                JournalEntry je = journalqueue.Dequeue();

                if (!HistoryList.MergeEntries(prev, je))                // if not merged
                {
                    if (prev != null)                       // no merge, so if we have a merge candidate on top, run actions on it.
                        ActionEntry(prev);

                    prev = je;                              // record
                }
            }

            if (prev != null)                               // any left.. action it
                ActionEntry(prev);
        }

        void ActionEntry(JournalEntry je)               // issue the JE to the system
        {
            if (je.IsUIEvent)            // give windows time to set up for OnNewEvent, and tell them if its coming via showuievents
            {
                if (je is EliteDangerousCore.JournalEvents.JournalMusic)
                {
                    //System.Diagnostics.Debug.WriteLine("Dispatch from controller Journal UI event ");
                    OnNewUIEvent?.Invoke(new EliteDangerousCore.UIEvents.UIJournalMusic((je as EliteDangerousCore.JournalEvents.JournalMusic).MusicTrack, EDDConfig.Instance.ShowUIEvents, DateTime.UtcNow, false));
                }
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

        #region New UI event

        void NewUIEvent(UIEvent u)
        {
            Debug.Assert(System.Windows.Forms.Application.MessageLoop);
            //System.Diagnostics.Debug.WriteLine("Dispatch from controller UI event " + u.EventTypeStr);

            OnNewUIEvent?.Invoke(u);
        }

        #endregion


        #region Background Worker Threads - kicked off by Controller.Init, which itself is kicked by DiscoveryForm Init.

        private void BackgroundWorkerThread()     
        {
            readyForInitialLoad.WaitOne();      // wait for shown in form

            BackgroundInit();       // main init code

            if (!PendingClose)
            {
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
            }

            // Now we have been ordered to close down, so go thru the process

            closeRequested.WaitOne();      

            OnBgSafeClose?.Invoke();
            ReadyForFinalClose = true;
            InvokeAsyncOnUiThread(() =>
            {
                OnFinalClose?.Invoke();
            });
        }

        // Called from Background Thread Worker at Init() 

        private void BackgroundInit()
        {
            StarScan.LoadBodyDesignationMap();

            SQLiteConnectionSystem.CreateSystemsTableIndexes();     // just make sure they are there..

            Debug.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Check systems");
            ReportSyncProgress(-1, "");

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

                // Former CheckSystems, reworked to accomodate new switches..
                // Check to see what sync refreshes we need

                // New Galmap load - it was not doing a refresh if EDSM sync kept on happening. Now has its own timer

                string rwgalmaptime = SQLiteConnectionSystem.GetSettingString("EDSMGalMapLast", "2000-01-01 00:00:00"); // Latest time from RW file.
                DateTime galmaptime;
                if (!DateTime.TryParse(rwgalmaptime, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out galmaptime))
                    galmaptime = new DateTime(2000, 1, 1);

                if (DateTime.Now.Subtract(galmaptime).TotalDays > 14)  // Over 14 days do a sync from EDSM for galmap
                {
                    LogLine("Get galactic mapping from EDSM.".Tx(this,"EDSM"));
                    galacticMapping.DownloadFromEDSM();
                    SQLiteConnectionSystem.PutSettingString("EDSMGalMapLast", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                Debug.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Check systems complete");
            }

            galacticMapping.ParseData();                            // at this point, gal map data has been uploaded - get it into memory
            SystemClassDB.AddToAutoComplete(galacticMapping.GetGMONames());
            SystemNoteClass.GetAllSystemNotes();

            LogLine("Loaded Notes, Bookmarks and Galactic mapping.".Tx(this,"LN"));

            if (PendingClose) return;

            if (EliteDangerousCore.EDDN.EDDNClass.CheckforEDMC()) // EDMC is running
            {
                if (EDCommander.Current.SyncToEddn)  // Both EDD and EDMC should not sync to EDDN.
                {
                    LogLineHighlight("EDDiscovery and EDMarketConnector should not both sync to EDDN. Stop EDMC or uncheck 'send to EDDN' in settings tab!".Tx(this,"EDMC"));
                }
            }

            if (!EDDOptions.Instance.NoLoad)        // here in this thread, we do a refresh of history. 
            {
                LogLine("Reading travel history".Tx(this,"RTH"));
                DoRefreshHistory(new RefreshWorkerArgs { CurrentCommander = EDCommander.CurrentCmdrID });       // kick the background refresh worker thread into action
            }

            if (PendingClose) return;

            if (!EDDOptions.Instance.NoSystemsLoad && EDDConfig.Instance.EDSMEDDBDownload)        // if enabled
            {
                SystemClassEDSM.DetermineIfFullEDSMSyncRequired(syncstate);                         // ask EDSM and EDDB if they want to do a Full sync..
                EliteDangerousCore.EDDB.SystemClassEDDB.DetermineIfEDDBSyncRequired(syncstate);

                if (syncstate.perform_eddb_sync || syncstate.perform_edsm_fullsync)
                {
                    string databases = (syncstate.perform_edsm_fullsync && syncstate.perform_eddb_sync) ? "EDSM and EDDB" : ((syncstate.perform_edsm_fullsync) ? "EDSM" : "EDDB");

                    LogLine(string.Format("Full synchronisation to the {0} databases required."+ Environment.NewLine +
                                    "This will take a while, up to 15 minutes, please be patient." + Environment.NewLine +
                                    "Please continue running ED Discovery until refresh is complete.".Tx(this,"SyncEDSM"), databases ));
                }

            }
            else
            {
                LogLine("Synchronisation to EDSM and EDDB disabled. Use Settings panel to reenable".Tx(this,"SyncOff"));
            }
        }

        #endregion


        #region Sync Perform to EDSM/EDDB

        private void DoPerformSync()        // in Background worker
        {
            InvokeAsyncOnUiThread.Invoke(() => OnSyncStarting?.Invoke());       // tell listeners sync is starting

            resyncEDSMEDDBRequestedFlag = 1;     // sync is happening, stop any async requests..

            Debug.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Perform EDSM/EDDB sync");
            try
            {
                bool[] grids = new bool[GridId.MaxGridID];
                foreach (int i in GridId.FromString(EDDConfig.Instance.EDSMGridIDs))
                    grids[i] = true;

                syncstate.ClearCounters();

                if (syncstate.perform_edsm_fullsync || syncstate.perform_eddb_sync)
                {
                    if (syncstate.perform_edsm_fullsync && !PendingClose)
                    {
                        // Download new systems
                        try
                        {
                            syncstate.edsm_fullsync_count = SystemClassEDSM.PerformEDSMFullSync(grids, () => PendingClose, ReportSyncProgress, LogLine, LogLineHighlight);
                            syncstate.perform_edsm_fullsync = false;
                        }
                        catch (Exception ex)
                        {
                            LogLineHighlight("GetAllEDSMSystems exception:" + ex.Message);
                        }

                    }

                    if (!PendingClose)
                    {
                        SQLiteConnectionSystem.CreateSystemsTableIndexes();  // again check indexes.. sometimes SQL does not create them due to schema change

                        try
                        {
                            syncstate.eddb_sync_count = EliteDangerousCore.EDDB.SystemClassEDDB.PerformEDDBFullSync(() => PendingClose, ReportSyncProgress, LogLine, LogLineHighlight);
                            syncstate.perform_eddb_sync = false;
                        }
                        catch (Exception ex)
                        {
                            LogLineHighlight("GetEDDBUpdate exception: " + ex.Message);
                        }
                    }
                }

                if (!PendingClose)
                {
                    SQLiteConnectionSystem.CreateSystemsTableIndexes();         // again check indexes.. sometimes SQL does not create them due to schema change

                    syncstate.edsm_updatesync_count = EliteDangerousCore.EDSM.SystemClassEDSM.PerformEDSMUpdateSync(grids, () => PendingClose, ReportSyncProgress, LogLine, LogLineHighlight);
                }

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

        // Done in UI thread after DoPerformSync completes

        private void PerformSyncCompleted()
        {
            Debug.Assert(System.Windows.Forms.Application.MessageLoop);

            if (syncstate.edsm_fullsync_count > 0 || syncstate.edsm_updatesync_count > 0)
                LogLine(string.Format("EDSM update complete with {0} systems".Tx(this, "EDSMU"), syncstate.edsm_fullsync_count + syncstate.edsm_updatesync_count));

            if (syncstate.eddb_sync_count > 0)
                LogLine(string.Format("EDDB update complete with {0} systems".Tx(this, "EDDBU"), syncstate.eddb_sync_count));

            long totalsystems = SystemClassDB.GetTotalSystems();
            LogLineSuccess(string.Format("Loading completed, total of {0:N0} systems stored".Tx(this, "SYST"), totalsystems ));

            if (syncstate.edsm_fullsync_count > 0 || syncstate.eddb_sync_count > 0 || syncstate.edsm_updatesync_count > 20000)   // if we have done a resync, or a major update sync (arb no)
            {
                LogLine("Refresh due to updating EDSM or EDDB data".Tx(this,"Refresh"));
                RefreshHistoryAsync();
            }

            OnSyncComplete?.Invoke();

            ReportSyncProgress(-1, "");

            resyncEDSMEDDBRequestedFlag = 0;        // releases flag and allow another async to happen

            Debug.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Perform sync completed");
        }

        #endregion


        #region History Refresh System

        protected class RefreshWorkerArgs
        {
            public string NetLogPath;
            public bool ForceNetLogReload;
            public bool ForceJournalReload;
            public int CurrentCommander;
        }

        // this thread waits around until told to do a refresh then performs it.  
        // ONLY used for subsequent refreshes, first one done on background worker

        private void BackgroundHistoryRefreshWorkerThread()
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

        // this function does the history refresh, executes on Background worker or background history refresh thread
        private void DoRefreshHistory(RefreshWorkerArgs args)
        {
            HistoryList hist = null;
            try
            {
                refreshWorkerArgs = args;

                Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Load history for Cmdr " + args.CurrentCommander + " " + EDCommander.Current.Name);

                hist = HistoryList.LoadHistory(journalmonitor, 
                    () => PendingClose, 
                    (p, s) => ReportRefreshProgress(p, string.Format("Processing log file {0}".Tx(this,"PLF") , s)), args.NetLogPath,
                    args.ForceJournalReload, args.ForceJournalReload, args.CurrentCommander, 
                    EDDConfig.Instance.ShowUIEvents, EDDConfig.Instance.FullHistoryLoadDayLimit);

                Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Load history complete with " + hist.Count + " records");
            }
            catch (Exception ex)
            {
                LogLineHighlight("History Refresh Error: " + ex);
            }

            initComplete.WaitOne();

            ReportRefreshProgress(-1, "Refresh Displays".Tx(this,"RD"));

            InvokeAsyncOnUiThread(() => ForegroundHistoryRefreshComplete(hist));
        }

        // Called on foreground after history has refreshed

        private void ForegroundHistoryRefreshComplete(HistoryList hist)
        {
            Debug.Assert(System.Windows.Forms.Application.MessageLoop);

            if (!PendingClose)
            {
                Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Refresh history worker completed");

                if (hist != null)
                {
                    history.Copy(hist);

                    OnRefreshCommanders?.Invoke();

                    EdsmLogFetcher.StopCheck();

                    Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Refresh Displays");

                    OnHistoryChange?.Invoke(history);

                    Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Refresh Displays Completed");
                }

                Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " JM On");

                journalmonitor.StartMonitor();

                Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Call Refresh Complete");

                OnRefreshComplete?.Invoke();                            // History is completed

                if (history.CommanderId >= 0)
                    EdsmLogFetcher.Start(EDCommander.Current);

                refreshHistoryRequestedFlag = 0;
                readyForNewRefresh.Set();

                LogLine("History refresh complete.".Tx(this,"HRC"));

                ReportRefreshProgress(-1, "");

                Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Refresh history complete");
            }
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

        #endregion
    }
}
