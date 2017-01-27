using EDDiscovery.DB;
using EDDiscovery.EDSM;
using EDDiscovery.EliteDangerous;
using EDDiscovery2;
using EDDiscovery2.DB;
using EDDiscovery2.EDSM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EDDiscovery
{
    public class EDDiscoveryController : IDiscoveryController
    {
        #region Public Interface
        #region Variables
        public HistoryList history { get; private set; } = new HistoryList();
        public EDSMSync EdsmSync { get; private set; }
        public string LogText { get { return logtext; } }
        public bool PendingClose { get; private set; }           // we want to close boys!
        public GalacticMapping galacticMapping { get; private set; }
        public bool ReadyForFinalClose { get; private set; }
        #endregion

        #region Events
        public event Action<HistoryList> OnHistoryChange;
        public event Action<HistoryEntry, HistoryList> OnNewEntry;
        public event Action<JournalEntry> OnNewJournalEntry;
        public event Action<string, Color> OnNewLogEntry;

        public event Action OnDbInitComplete;
        public event Action OnBgSafeClose;
        public event Action OnFinalClose;
        public event Action OnRefreshStarting;
        public event Action OnRefreshCommanders;
        public event Action OnRefreshComplete;
        public event Action OnInitialSyncComplete;
        public event Action OnSyncStarting;
        public event Action OnSyncComplete;
        public event Action<int, string> OnReportProgress;
        #endregion

        #region Initialisation

        public EDDiscoveryController(Func<Color> getNormalTextColor, Func<Color> getHighlightTextColor, Func<Color> getSuccessTextColor, Action<Action> invokeSyncOnUiThread, Action<Action> invokeAsyncOnUiThread)
        {
            GetNormalTextColour = getNormalTextColor;
            GetHighlightTextColour = getHighlightTextColor;
            GetSuccessTextColour = getSuccessTextColor;
            InvokeSyncOnUiThread = invokeSyncOnUiThread;
            InvokeAsyncOnUiThread = invokeAsyncOnUiThread;
        }

        public void Init(bool noreposition)
        {
            EDDConfig.Options.Init(noreposition);

            if (EDDConfig.Options.ReadJournal != null)
            {
                EDJournalClass.ReadCmdLineJournal(EDDConfig.Options.ReadJournal);
            }

            string logpath = "";
            try
            {
                logpath = Path.Combine(Tools.GetAppDataDirectory(), "Log");
                if (!Directory.Exists(logpath))
                {
                    Directory.CreateDirectory(logpath);
                }

                if (!Debugger.IsAttached || EDDConfig.Options.TraceLog)
                {
                    TraceLog.LogFileWriterException += ex =>
                    {
                        LogLineHighlight($"Log Writer Exception: {ex}");
                    };
                    TraceLog.Init(EDDConfig.Options.LogExceptions);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Unable to create the folder '{logpath}'");
                Trace.WriteLine($"Exception: {ex.Message}");
            }

            SQLiteConnectionUser.EarlyReadRegister();
            EDDConfig.Instance.Update(write: false);

            backgroundWorker = new Thread(BackgroundWorkerThread);
            backgroundWorker.IsBackground = true;
            backgroundWorker.Name = "Background Worker Thread";
            backgroundWorker.Start();

            galacticMapping = new GalacticMapping();

            EdsmSync = new EDSMSync(this);
            EdsmSync.OnDownloadedSystems += () => RefreshHistoryAsync();

            journalmonitor = new EliteDangerous.EDJournalClass();
            journalmonitor.OnNewJournalEntry += NewPosition;

            history.CommanderId = EDDiscoveryForm.EDDConfig.CurrentCommander.Nr;
        }

        public void PostInit_Loading()
        {
            readyForUiInvoke.Set();
        }

        public void PostInit_Loaded()
        {
            EliteDangerousClass.CheckED();
            EDDConfig.Instance.Update();
            CheckIfEliteDangerousIsRunning();
        }

        public void PostInit_Shown()
        {
            downloadMapsTask = FGEImage.DownloadMaps(this, () => PendingClose, LogLine, LogLineHighlight);
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
                InvokeSyncOnUiThread(() =>
                {
                    logtext += text + Environment.NewLine;      // keep this, may be the only log showing

                    OnNewLogEntry?.Invoke(text + Environment.NewLine, color);
                });
            }
            catch { }
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

            if (Interlocked.CompareExchange(ref refreshRequestedFlag, 1, 0) == 0)
            {
                InvokeSyncOnUiThread(() =>
                {
                    OnRefreshStarting?.Invoke();
                    journalmonitor.StopMonitor();          // this is called by the foreground.  Ensure background is stopped.  Foreground must restart it.
                });

                refreshWorkerArgs = new RefreshWorkerArgs
                {
                    NetLogPath = netlogpath,
                    ForceNetLogReload = forcenetlogreload,
                    ForceJournalReload = forcejournalreload,
                    CheckEdsm = checkedsm,
                    CurrentCommander = currentcmdr ?? history.CommanderId
                };

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
        #endregion

        #region Implementation
        #region Variables
        private string logtext = "";     // to keep in case of no logs..
        private event EventHandler HistoryRefreshed; // this is an internal hook

        private Task<bool> downloadMapsTask = null;

        private EliteDangerous.EDJournalClass journalmonitor;

        private RefreshWorkerArgs refreshWorkerArgs = new RefreshWorkerArgs();
        private SystemClass.SystemsSyncState syncstate = new SystemClass.SystemsSyncState();

        private Thread backgroundWorker;
        private Thread backgroundRefreshWorker;

        private ManualResetEvent closeRequested = new ManualResetEvent(false);
        private ManualResetEvent readyForInitialLoad = new ManualResetEvent(false);
        private ManualResetEvent readyForUiInvoke = new ManualResetEvent(false);
        private AutoResetEvent refreshRequested = new AutoResetEvent(false);
        private AutoResetEvent resyncRequestedEvent = new AutoResetEvent(false);
        private int refreshRequestedFlag = 0;
        private int resyncRequestedFlag = 0;
        #endregion

        #region Accessors
        private Func<Color> GetNormalTextColour;
        private Func<Color> GetHighlightTextColour;
        private Func<Color> GetSuccessTextColour;
        private Action<Action> InvokeSyncOnUiThread;
        private Action<Action> InvokeAsyncOnUiThread;
        #endregion

        #region Initialization
        private void BackgroundInit()
        {
            InitializeDatabases();
            readyForUiInvoke.WaitOne();
            InvokeAsyncOnUiThread(() => OnDbInitComplete?.Invoke());
            readyForInitialLoad.WaitOne();
            CheckSystems(() => PendingClose, (p, s) => ReportProgress(p, s));
            ReportProgress(-1, "");
            InvokeSyncOnUiThread(() => OnInitialSyncComplete?.Invoke());
            if (PendingClose) return;

            if (EDDN.EDDNClass.CheckforEDMC()) // EDMC is running
            {
                if (EDDConfig.Instance.CurrentCommander.SyncToEddn)  // Both EDD and EDMC should not sync to EDDN.
                {
                    LogLineHighlight("EDDiscovery and EDMarketConnector should not both sync to EDDN. Stop EDMC or uncheck 'send to EDDN' in settings tab!");
                }
            }

            if (PendingClose) return;
            LogLine("Reading travel history");
            DoRefreshHistory(new RefreshWorkerArgs { CurrentCommander = EDDConfig.Instance.CurrentCmdrID });

            if (PendingClose) return;
            if (syncstate.performeddbsync || syncstate.performedsmsync)
            {
                string databases = (syncstate.performedsmsync && syncstate.performeddbsync) ? "EDSM and EDDB" : ((syncstate.performedsmsync) ? "EDSM" : "EDDB");

                LogLine("ED Discovery will now synchronise to the " + databases + " databases to obtain star information." + Environment.NewLine +
                                "This will take a while, up to 15 minutes, please be patient." + Environment.NewLine +
                                "Please continue running ED Discovery until refresh is complete.");
            }
        }

        private void InitializeDatabases()
        {
            Trace.WriteLine("Initializing database");
            SQLiteConnectionOld.Initialize();
            SQLiteConnectionUser.Initialize();
            SQLiteConnectionSystem.Initialize();
            Trace.WriteLine("Database initialization complete");
        }

        private void CheckIfEliteDangerousIsRunning()
        {
            if (EliteDangerousClass.EDRunning)
            {
                LogLine("EliteDangerous is running.");
            }
            else
            {
                LogLine("EliteDangerous is not running.");
            }
        }

        #endregion

        #region Initial Check Systems

        private void CheckSystems(Func<bool> cancelRequested, Action<int, string> reportProgress)  // ASYNC process, done via start up, must not be too slow.
        {
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
                DateTime lastmod = outoforder ? SystemClass.GetLastSystemModifiedTime() : SystemClass.GetLastSystemModifiedTimeFast();

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
                SQLiteConnectionUser.TranferVisitedSystemstoJournalTableIfRequired();
                SQLiteConnectionSystem.CreateSystemsTableIndexes();
                SystemNoteClass.GetAllSystemNotes();                                // fill up memory with notes, bookmarks, galactic mapping
                BookmarkClass.GetAllBookmarks();
                galacticMapping.ParseData();                            // at this point, EDSM data is loaded..
                SystemClass.AddToAutoComplete(galacticMapping.GetGMONames());
                EDDiscovery2.DB.MaterialCommodities.SetUpInitialTable();

                LogLine("Loaded Notes, Bookmarks and Galactic mapping.");

                string timestr = SQLiteConnectionSystem.GetSettingString("EDDBSystemsTime", "0");
                DateTime time = new DateTime(Convert.ToInt64(timestr), DateTimeKind.Utc);
                if (DateTime.UtcNow.Subtract(time).TotalDays > 6.5)     // Get EDDB data once every week.
                    syncstate.performeddbsync = true;
            }
        }

        #endregion

        #region Async EDSM/EDDB Full Sync

        private void DoPerformSync()
        {
            try
            {
                SystemClass.PerformSync(() => PendingClose, (p, s) => ReportProgress(p, s), LogLine, LogLineHighlight, syncstate);
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
                long totalsystems = SystemClass.GetTotalSystems();
                LogLineSuccess("Loading completed, total of " + totalsystems + " systems");

                if (syncstate.performhistoryrefresh)
                {
                    LogLine("Refresh due to updating systems");
                    HistoryRefreshed += HistoryFinishedRefreshing;
                    RefreshHistoryAsync();
                }

                OnSyncComplete?.Invoke();

                resyncRequestedFlag = 0;
            }
        }

        private void HistoryFinishedRefreshing(object sender, EventArgs e)
        {
            HistoryRefreshed -= HistoryFinishedRefreshing;
            LogLine("Refreshing complete.");

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
                hist = HistoryList.LoadHistory(journalmonitor, () => PendingClose, (p, s) => ReportProgress(p, $"Processing log file {s}"), args.NetLogPath, args.ForceJournalReload, args.ForceJournalReload, args.CheckEdsm, args.CurrentCommander);
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
                    OnRefreshCommanders?.Invoke();

                    history.Clear();

                    foreach (var ent in hist.EntryOrder)
                    {
                        history.Add(ent);
                        Debug.Assert(ent.MaterialCommodity != null);
                    }

                    history.materialcommodititiesledger = hist.materialcommodititiesledger;
                    history.starscan = hist.starscan;
                    history.CommanderId = hist.CommanderId;

                    ReportProgress(-1, "");
                    LogLine("Refresh Complete.");

                    RefreshDisplays();
                }

                HistoryRefreshed?.Invoke(this, EventArgs.Empty);

                journalmonitor.StartMonitor();

                OnRefreshComplete?.Invoke();

                refreshRequestedFlag = 0;
            }
        }

        private void NewPosition(EliteDangerous.JournalEntry je)
        {
            if (je.CommanderId == history.CommanderId)     // we are only interested at this point accepting ones for the display commander
            {
                HistoryEntry he = history.AddJournalEntry(je, h => LogLineHighlight(h));

                if (he != null)
                    OnNewEntry?.Invoke(he, history);
            }

            OnNewJournalEntry?.Invoke(je);

            if (je.EventTypeID == JournalTypeEnum.LoadGame)
            {
                OnRefreshCommanders?.Invoke();
            }
        }
        #endregion

        #region Background Worker Threads
        private void BackgroundWorkerThread()
        {
            BackgroundInit();
            if (!PendingClose)
            {
                backgroundRefreshWorker = new Thread(BackgroundRefreshWorkerThread) { Name = "Background Refresh Worker", IsBackground = true };
                backgroundRefreshWorker.Start();

                try
                {
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

        private void BackgroundRefreshWorkerThread()
        {
            while (!PendingClose)
            {
                int wh = WaitHandle.WaitAny(new WaitHandle[] { closeRequested, refreshRequested });

                if (PendingClose) break;

                switch (wh)
                {
                    case 0:  // Close Requested
                        break;
                    case 1:  // Refresh Requested
                        DoRefreshHistory(refreshWorkerArgs);
                        break;
                }
            }
        }

        #endregion

        #endregion

    }
}
