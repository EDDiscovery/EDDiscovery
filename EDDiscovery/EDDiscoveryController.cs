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
using EDDiscovery.DB;
using EDDiscovery.EDSM;
using EDDiscovery.EliteDangerous;
using EDDiscovery2;
using EDDiscovery2.DB;
using EDDiscovery2.EDSM;
using System;
using System.Collections.Concurrent;
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

        public static Task Initialize(bool noreposition)
        {
            InitializeConfig(noreposition);

            return Task.Factory.StartNew(InitializeDatabases);
        }

        public void Init()
        {
            if (!Debugger.IsAttached || EDDConfig.Options.TraceLog)
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

            EdsmSync = new EDSMSync(this);
            EdsmSync.OnDownloadedSystems += () => RefreshHistoryAsync();

            journalmonitor = new EliteDangerous.EDJournalClass(InvokeSyncOnUiThread);
            journalmonitor.OnNewJournalEntry += NewPosition;

            history.CommanderId = EDDiscoveryForm.EDDConfig.CurrentCommander.Nr;
        }

        public void PostInit_Loaded()
        {
            EliteDangerousClass.CheckED();
            EDDConfig.Instance.Update();
            CheckIfEliteDangerousIsRunning();
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
                InvokeSyncOnUiThread(() =>
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

        #region Closest Systems
        public void CalculateClosestSystems(ISystem sys, Action<ISystem, SortedList<double, ISystem>> callback, bool ignoreDuplicates = true)
        {
            closestsystem_queue.Enqueue(new StardistRequest { System = sys, Callback = callback, IgnoreOnDuplicate = ignoreDuplicates });
            stardistRequested.Set();
        }
        #endregion
        #endregion

        #region Implementation
        #region Variables
        private string logtext = "";     // to keep in case of no logs..
        private event EventHandler HistoryRefreshed; // this is an internal hook

        private Task<bool> downloadMapsTask = null;

        private EliteDangerous.EDJournalClass journalmonitor;

        private ConcurrentQueue<RefreshWorkerArgs> refreshWorkerQueue = new ConcurrentQueue<RefreshWorkerArgs>();
        private SystemClass.SystemsSyncState syncstate = new SystemClass.SystemsSyncState();
        private ConcurrentQueue<StardistRequest> closestsystem_queue = new ConcurrentQueue<StardistRequest>();
        private RefreshWorkerArgs refreshWorkerArgs = new RefreshWorkerArgs();

        private Thread backgroundWorker;
        private Thread backgroundRefreshWorker;
        private Thread backgroundStardistWorker;

        private ManualResetEvent closeRequested = new ManualResetEvent(false);
        private ManualResetEvent readyForInitialLoad = new ManualResetEvent(false);
        private ManualResetEvent readyForNewRefresh = new ManualResetEvent(false);
        private AutoResetEvent refreshRequested = new AutoResetEvent(false);
        private AutoResetEvent resyncRequestedEvent = new AutoResetEvent(false);
        private AutoResetEvent stardistRequested = new AutoResetEvent(false);
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
            readyForInitialLoad.WaitOne();
            downloadMapsTask = FGEImage.DownloadMaps(this, () => PendingClose, LogLine, LogLineHighlight);
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

        private static void InitializeDatabases()
        {
            Trace.WriteLine("Initializing database");
            SQLiteConnectionOld.Initialize();
            SQLiteConnectionUser.Initialize();
            SQLiteConnectionSystem.Initialize();
            Trace.WriteLine("Database initialization complete");
        }

        private static void InitializeConfig(bool noreposition)
        {
            EDDConfig.Options.Init(noreposition);

            if (EDDConfig.Options.ReadJournal != null && File.Exists(EDDConfig.Options.ReadJournal))
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
                    TraceLog.Init();
                }

                if (EDDConfig.Options.LogExceptions)
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
            EDDConfig.Instance.Update(false);
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
                EDDiscovery2.DB.MaterialCommodity.SetUpInitialTable();

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
                refreshWorkerArgs = args;
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
                readyForNewRefresh.Set();
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
                backgroundStardistWorker = new Thread(BackgroundStardistWorkerThread) { Name = "Star Distance Worker", IsBackground = true };
                backgroundStardistWorker.Start();

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

                backgroundStardistWorker.Join();
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
                        InvokeSyncOnUiThread(() =>
                        {
                            OnRefreshStarting?.Invoke();
                            journalmonitor.StopMonitor();          // this is called by the foreground.  Ensure background is stopped.  Foreground must restart it.
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

        private class StardistRequest
        {
            public ISystem System;
            public bool IgnoreOnDuplicate;
            public Action<ISystem, SortedList<double, ISystem>> Callback;
        }

        private class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable      // special compare for sortedlist
        {
            public int Compare(TKey x, TKey y)
            {
                int result = x.CompareTo(y);
                return (result == 0) ? 1 : result;      // for this, equals just means greater than, to allow duplicate distance values to be added.
            }
        }

        private void BackgroundStardistWorkerThread()
        {
            while (!PendingClose)
            {
                int wh = WaitHandle.WaitAny(new WaitHandle[] { closeRequested, stardistRequested });

                if (PendingClose) break;

                StardistRequest stardistreq = null;

                switch (wh)
                {
                    case 0:  // Close Requested
                        break;
                    case 1:  // Star Distances Requested
                        while (!PendingClose && closestsystem_queue.TryDequeue(out stardistreq))
                        {
                            if (!stardistreq.IgnoreOnDuplicate || closestsystem_queue.Count == 0)
                            {
                                ISystem sys = stardistreq.System;
                                SortedList<double, ISystem> closestsystemlist = new SortedList<double, ISystem>(new DuplicateKeyComparer<double>()); //lovely list allowing duplicate keys - can only iterate in it.
                                SystemClass.GetSystemSqDistancesFrom(closestsystemlist, sys.x, sys.y, sys.z, 50, true, 1000);
                                if (!PendingClose)
                                {
                                    InvokeSyncOnUiThread(() =>
                                    {
                                        history.CalculateSqDistances(closestsystemlist, sys.x, sys.y, sys.z, 50, true);
                                        stardistreq.Callback(sys, closestsystemlist);
                                    });
                                }
                            }
                        }

                        break;
                }
            }
        }

        #endregion

        #endregion

    }
}
