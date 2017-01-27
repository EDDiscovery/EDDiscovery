using EDDiscovery.DB;
using EDDiscovery.EDSM;
using EDDiscovery.EliteDangerous;
using EDDiscovery2;
using EDDiscovery2.DB;
using EDDiscovery2.EDSM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery
{
    public class EDDiscoveryController : IDiscoveryController
    {
        #region Public Interface
        #region Variables
        public HistoryList history { get; private set; } = new HistoryList();
        public EDSMSync EdsmSync { get; private set; }
        public string LogText { get { return logtext; } }
        public bool PendingClose { get { return safeClose != null; } }           // we want to close boys!
        public GalacticMapping galacticMapping { get; private set; }
        public bool ReadyForFinalClose { get { return safeClose != null && !safeClose.IsAlive; } }
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

            dbinitworker = new BackgroundWorker();
            dbinitworker.DoWork += Dbinitworker_DoWork;
            dbinitworker.RunWorkerCompleted += Dbinitworker_RunWorkerCompleted;
            dbinitworker.RunWorkerAsync();

            galacticMapping = new GalacticMapping();

            EdsmSync = new EDSMSync(this);

            journalmonitor = new EliteDangerous.EDJournalClass();

            history.CommanderId = EDDiscoveryForm.EDDConfig.CurrentCommander.Nr;

            this._syncWorker = new System.ComponentModel.BackgroundWorker();
            this._checkSystemsWorker = new System.ComponentModel.BackgroundWorker();
            this._refreshWorker = new System.ComponentModel.BackgroundWorker();

            // 
            // _syncWorker
            // 
            this._syncWorker.WorkerReportsProgress = true;
            this._syncWorker.WorkerSupportsCancellation = true;
            this._syncWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this._syncWorker_DoWork);
            this._syncWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this._syncWorker_ProgressChanged);
            this._syncWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this._syncWorker_RunWorkerCompleted);
            // 
            // _checkSystemsWorker
            // 
            this._checkSystemsWorker.WorkerReportsProgress = true;
            this._checkSystemsWorker.WorkerSupportsCancellation = true;
            this._checkSystemsWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this._checkSystemsWorker_DoWork);
            this._checkSystemsWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this._checkSystemsWorker_ProgressChanged);
            this._checkSystemsWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this._checkSystemsWorker_RunWorkerCompleted);
            // 
            // _refreshWorker
            // 
            this._refreshWorker.WorkerReportsProgress = true;
            this._refreshWorker.WorkerSupportsCancellation = true;
            this._refreshWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.RefreshHistoryWorker);
            this._refreshWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.RefreshHistoryWorkerProgressChanged);
            this._refreshWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.RefreshHistoryWorkerCompleted);

        }

        public void PostInit_Loading()
        {
        }

        public void PostInit_Loaded()
        {
            EliteDangerousClass.CheckED();
            EDDConfig.Instance.Update();
            CheckIfEliteDangerousIsRunning();
        }

        public void PostInit_Shown()
        {
            _checkSystemsWorker.RunWorkerAsync();
            downloadMapsTask = FGEImage.DownloadMaps(this, () => PendingClose, LogLine, LogLineHighlight);
        }
        #endregion

        #region Shutdown
        public void Shutdown()
        {
            if (safeClose == null)                  // so a close is a request now, and it launches a thread which cleans up the system..
            {
                CancelHistoryRefresh();
                EDDNSync.StopSync();
                _syncWorker.CancelAsync();
                _checkSystemsWorker.CancelAsync();
                if (cancelDownloadMaps != null)
                {
                    cancelDownloadMaps();
                }
                LogLineHighlight("Closing down, please wait..");
                Console.WriteLine("Close.. safe close launched");
                safeClose = new Thread(SafeClose) { Name = "Close Down", IsBackground = true };
                safeClose.Start();
            }
            else if (!safeClose.IsAlive)   // still working, cancel again..
            {
                Console.WriteLine("go for close");
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
        public void RefreshHistoryAsync(string netlogpath = null, bool forcenetlogreload = false, bool forcejournalreload = false, bool checkedsm = false, int? currentcmdr = null)
        {
            if (PendingClose)
            {
                return;
            }

            if (!_refreshWorker.IsBusy)
            {
                OnRefreshStarting?.Invoke();

                journalmonitor.StopMonitor();          // this is called by the foreground.  Ensure background is stopped.  Foreground must restart it.

                RefreshWorkerArgs args = new RefreshWorkerArgs
                {
                    NetLogPath = netlogpath,
                    ForceNetLogReload = forcenetlogreload,
                    ForceJournalReload = forcejournalreload,
                    CheckEdsm = checkedsm,
                    CurrentCommander = currentcmdr ?? history.CommanderId
                };

                _refreshWorker.RunWorkerAsync(args);
            }
        }

        public void RefreshDisplays()
        {
            if (OnHistoryChange != null)
                OnHistoryChange(history);
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
            if (!_syncWorker.IsBusy)
            {
                OnSyncStarting?.Invoke();
                syncstate.performeddbsync |= eddbsync;
                syncstate.performedsmsync |= edsmsync;
                _syncWorker.RunWorkerAsync();
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

        private ManualResetEvent _syncWorkerCompletedEvent = new ManualResetEvent(false);
        private ManualResetEvent _checkSystemsWorkerCompletedEvent = new ManualResetEvent(false);

        private Action cancelDownloadMaps = null;
        private Task<bool> downloadMapsTask = null;
        private BackgroundWorker dbinitworker = null;

        private EliteDangerous.EDJournalClass journalmonitor;

        private System.ComponentModel.BackgroundWorker _syncWorker;
        private System.ComponentModel.BackgroundWorker _checkSystemsWorker;
        private System.ComponentModel.BackgroundWorker _refreshWorker;

        private Thread safeClose;
        private System.Windows.Forms.Timer closeTimer;
        #endregion

        #region Accessors
        private Func<Color> GetNormalTextColour;
        private Func<Color> GetHighlightTextColour;
        private Func<Color> GetSuccessTextColour;
        private Action<Action> InvokeSyncOnUiThread;
        private Action<Action> InvokeAsyncOnUiThread;
        #endregion

        #region Initialization
        private void Dbinitworker_DoWork(object sender, DoWorkEventArgs e)
        {
            Trace.WriteLine("Initializing database");
            SQLiteConnectionOld.Initialize();
            SQLiteConnectionUser.Initialize();
            SQLiteConnectionSystem.Initialize();
            Trace.WriteLine("Database initialization complete");
        }

        private void Dbinitworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OnDbInitComplete?.Invoke();
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

        private void _checkSystemsWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                var worker = (System.ComponentModel.BackgroundWorker)sender;

                CheckSystems(() => worker.CancellationPending, (p, s) => worker.ReportProgress(p, s));

                if (worker.CancellationPending)
                    e.Cancel = true;
            }
            catch (Exception ex) { e.Result = ex; }       // any exceptions, ignore
            finally
            {
                _checkSystemsWorkerCompletedEvent.Set();
            }
        }

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

        private void _checkSystemsWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Exception ex = e.Cancelled ? null : (e.Error ?? e.Result as Exception);
            ReportProgress(-1, "");
            if (!e.Cancelled && !PendingClose)
            {
                if (ex != null)
                {
                    LogLineHighlight("Check Systems exception: " + ex.Message + Environment.NewLine + "Trace: " + ex.StackTrace);
                }

                journalmonitor.OnNewJournalEntry += NewPosition;
                EdsmSync.OnDownloadedSystems += RefreshDueToEDSMDownloadedSystems;

                OnInitialSyncComplete?.Invoke();

                LogLine("Reading travel history");
                HistoryRefreshed += _travelHistoryControl1_InitialRefreshDone;

                RefreshHistoryAsync();

                if (EDDN.EDDNClass.CheckforEDMC()) // EDMC is running
                {
                    if (EDDiscoveryForm.EDDConfig.CurrentCommander.SyncToEddn)  // Both EDD and EDMC should not sync to EDDN.
                    {
                        LogLineHighlight("EDDiscovery and EDMarketConnector should not both sync to EDDN. Stop EDMC or uncheck 'send to EDDN' in settings tab!");
                    }
                }
            }
        }

        private void RefreshDueToEDSMDownloadedSystems()
        {
            InvokeSyncOnUiThread(() => RefreshHistoryAsync());
        }

        private void _travelHistoryControl1_InitialRefreshDone(object sender, EventArgs e)
        {
            HistoryRefreshed -= _travelHistoryControl1_InitialRefreshDone;

            if (!PendingClose)
            {
                AsyncPerformSync();                              // perform any async synchronisations

                if (syncstate.performeddbsync || syncstate.performedsmsync)
                {
                    string databases = (syncstate.performedsmsync && syncstate.performeddbsync) ? "EDSM and EDDB" : ((syncstate.performedsmsync) ? "EDSM" : "EDDB");

                    LogLine("ED Discovery will now synchronise to the " + databases + " databases to obtain star information." + Environment.NewLine +
                                    "This will take a while, up to 15 minutes, please be patient." + Environment.NewLine +
                                    "Please continue running ED Discovery until refresh is complete.");
                }
            }
        }

        private void _checkSystemsWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            ReportProgress(e.ProgressPercentage, (string)e.UserState);
        }

        #endregion

        #region Async EDSM/EDDB Full Sync

        private void _syncWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                var worker = (System.ComponentModel.BackgroundWorker)sender;

                SystemClass.PerformSync(() => worker.CancellationPending, (p, s) => worker.ReportProgress(p, s), LogLine, LogLineHighlight, syncstate);
                if (worker.CancellationPending)
                    e.Cancel = true;
            }
            catch (Exception ex) { e.Result = ex; }       // ignore any excepctions
            finally
            {
                _syncWorkerCompletedEvent.Set();
            }
        }

        private SystemClass.SystemsSyncState syncstate = new SystemClass.SystemsSyncState();

        private void _syncWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Exception ex = e.Cancelled ? null : (e.Error ?? e.Result as Exception);
            ReportProgress(-1, "");

            if (!e.Cancelled && !PendingClose)
            {
                if (ex != null)
                {
                    LogLineHighlight("Check Systems exception: " + ex.Message + Environment.NewLine + "Trace: " + ex.StackTrace);
                }

                long totalsystems = SystemClass.GetTotalSystems();
                LogLineSuccess("Loading completed, total of " + totalsystems + " systems");

                if (syncstate.performhistoryrefresh)
                {
                    LogLine("Refresh due to updating systems");
                    HistoryRefreshed += HistoryFinishedRefreshing;
                    RefreshHistoryAsync();
                }

                OnSyncComplete?.Invoke();
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

        private void _syncWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            ReportProgress(e.ProgressPercentage, (string)e.UserState);
        }

        #endregion

        #region Closing
        private void SafeClose()        // ASYNC thread..
        {
            Thread.Sleep(1000);
            Console.WriteLine("Waiting for check systems to close");
            if (_checkSystemsWorker.IsBusy)
                _checkSystemsWorkerCompletedEvent.WaitOne();

            Console.WriteLine("Waiting for full sync to close");
            if (_syncWorker.IsBusy)
                _syncWorkerCompletedEvent.WaitOne();

            Console.WriteLine("Stopping discrete threads");
            journalmonitor.StopMonitor();

            if (EdsmSync != null)
                EdsmSync.StopSync();

            OnBgSafeClose?.Invoke();

            Console.WriteLine("Go for close timer!");

            InvokeSyncOnUiThread(() =>          // we need this thread to die so close will work, so kick off a timer
            {
                closeTimer = new System.Windows.Forms.Timer();
                closeTimer.Interval = 100;
                closeTimer.Tick += new EventHandler(CloseItFinally);
                closeTimer.Start();
            });
        }

        void CloseItFinally(Object sender, EventArgs e)
        {
            if (safeClose.IsAlive)      // still alive, try again
                closeTimer.Start();
            else
            {
                closeTimer.Stop();      // stop timer now. So it won't try to save it multiple times during close down if it takes a while - this caused a bug in saving some settings
                OnFinalClose?.Invoke();
            }
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

        private void RefreshHistoryWorker(object sender, DoWorkEventArgs e)
        {
            RefreshWorkerArgs args = e.Argument as RefreshWorkerArgs;
            var worker = (BackgroundWorker)sender;

            HistoryList hist = HistoryList.LoadHistory(journalmonitor, () => worker.CancellationPending, (p, s) => worker.ReportProgress(p, s), args.NetLogPath, args.ForceJournalReload, args.ForceJournalReload, args.CheckEdsm, args.CurrentCommander);

            if (worker.CancellationPending)
            {
                e.Cancel = true;
            }
            else
            {
                e.Result = hist;
            }
        }

        private void CancelHistoryRefresh()
        {
            _refreshWorker.CancelAsync();
        }

        private void RefreshHistoryWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled && !PendingClose)
            {
                if (e.Error != null)
                {
                    LogLineHighlight("History Refresh Error: " + e.Error.Message);
                }
                else
                {
                    OnRefreshCommanders?.Invoke();

                    history.Clear();

                    HistoryList hist = (HistoryList)e.Result;

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
            }
        }

        private void RefreshHistoryWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string name = (string)e.UserState;
            ReportProgress(e.ProgressPercentage, $"Processing log file {name}");
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

        #endregion

    }
}
