using EDDiscovery.DB;
using EDDiscovery.EDSM;
using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;
using EDDiscovery.HTTP;
using EDDiscovery2;
using EDDiscovery2.DB;
using EDDiscovery2.EDSM;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery
{
    public interface IDiscoveryController
    {
        #region Properties
        int DisplayedCommander { get; set; }
        HistoryList history { get; }
        bool option_nowindowreposition { get; }
        bool option_debugoptions { get; }
        EDSMSync EdsmSync { get; }
        string LogText { get; }
        bool PendingClose { get; }
        string VersionDisplayString { get; }
        GalacticMapping galacticMapping { get; }
        bool ReadyForClose { get; }
        bool IsDatabaseInitialized { get; }
        #endregion

        #region Events
        event Action<HistoryList> OnHistoryChange;
        event Action<HistoryEntry, HistoryList> OnNewEntry;
        event Action<string, Color> OnNewLogEntry;
        event Action OnNewTarget;
        #endregion

        #region Logging
        void LogLine(string text);
        void LogLineHighlight(string text);
        void LogLineSuccess(string text);
        void LogLineColor(string text, Color color);
        #endregion

        #region History
        bool RefreshHistoryAsync(string netlogpath = null, bool forcenetlogreload = false, bool forcejournalreload = false, bool checkedsm = false, int? currentcmdr = null);
        void RefreshDisplays();
        void NewPosition(EliteDangerous.JournalEntry je);
        void RecalculateHistoryDBs();
        #endregion

        #region Target
        void NewTargetSet();
        #endregion
    }

    public class EDDiscoveryController : IDiscoveryController
    {
        protected class RefreshWorkerArgs
        {
            public string NetLogPath;
            public bool ForceNetLogReload;
            public bool ForceJournalReload;
            public bool CheckEdsm;
            public int CurrentCommander;
        }

        protected class RefreshWorkerResults
        {
            public List<HistoryEntry> rethistory;
            public MaterialCommoditiesLedger retledger;
            public StarScan retstarscan;
        }

        #region Public Interface
        #region Public Properties
        public int DisplayedCommander { get; set; } = 0;
        public HistoryList history { get; private set; } = new HistoryList();
        public bool option_nowindowreposition { get; private set; } = false;                             // Cmd line options
        public bool option_debugoptions { get; private set; } = false;
        public bool option_tracelog { get; private set; } = false;
        public bool option_fcexcept { get; private set; } = false;
        public EDSMSync EdsmSync { get; private set; }
        public string LogText { get { return logtext; } }
        public bool PendingClose { get; private set; }           // we want to close boys!
        public string VersionDisplayString { get; private set; }
        public GalacticMapping galacticMapping { get; private set; }
        public bool ReadyForClose { get; private set; } = false;
        public bool IsDatabaseInitialized { get { return SQLiteConnectionUser.IsInitialized && SQLiteConnectionSystem.IsInitialized; } }
        #endregion

        #region Events
        public event Action HistoryRefreshed; // this is an internal hook
        public event Action<HistoryList> OnHistoryChange;
        public event Action<HistoryEntry, HistoryList> OnNewEntry;
        public event Action<string, Color> OnNewLogEntry;
        public event Action OnCheckSystemsCompleted;
        public event Action OnDatabaseInitializationComplete;
        public event Action OnBgSafeClose;
        public event Action OnFinalClose;
        public event Action OnRefreshCommanders;
        public event Action<GitHubRelease> OnNewReleaseAvailable;
        public event Action OnPerformSyncCompleted;
        public event Action OnRefreshHistoryRequested;
        public event Action<List<HistoryEntry>, MaterialCommoditiesLedger, StarScan> OnRefreshHistoryWorkerCompleted;
        public event Action<int, string> OnReportProgress;
        public event Action OnNewTarget;
        #endregion

        #region Methods
        #region Init
        public EDDiscoveryController(Func<Color> getNormalColor, Func<Color> getHighlightColor, Func<Color> getSuccessColor, Action<Action> asyncInvoker, Action<Action> syncInvoker)
        {
            GetLogNormalColour = getNormalColor;
            GetLogHighlightColour = getHighlightColor;
            GetLogSuccessColour = getSuccessColor;
            InvokeAsyncOnUIThread = asyncInvoker;
            InvokeSyncOnUIThread = syncInvoker;
        }

        public void Init()
        {
            VersionDisplayString = "Version " + Assembly.GetExecutingAssembly().FullName.Split(',')[1].Split('=')[1];

            ProcessCommandLineOptions();
            InitLogging();

            SQLiteConnectionUser.EarlyReadRegister();
            EDDConfig.Instance.Update(write: false);

            backgroundWorker = new Thread(BackgroundWorkerThread);
            backgroundWorker.IsBackground = true;
            backgroundWorker.Name = "Background Worker Thread";
            backgroundWorker.Start();

            galacticMapping = new GalacticMapping();
            EdsmSync = new EDSMSync(this);
            EdsmSync.OnDownloadedSystems += () => RefreshHistoryAsync();
            journalmonitor = new EDJournalClass();
            journalmonitor.OnNewJournalEntry += NewPosition;
            DisplayedCommander = EDDConfig.Instance.CurrentCommander.Nr;
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
            readyForInitialLoad.Set();
            downloadMapsTask = DownloadMaps();
        }
        #endregion

        #region Version Check
        public bool CheckForNewinstaller(out GitHubRelease newRelease)
        {
            newRelease = null;

            try
            {

                GitHubClass github = new GitHubClass();

                GitHubRelease rel = github.GetLatestRelease();

                if (rel != null)
                {
                    //string newInstaller = jo["Filename"].Value<string>();

                    var currentVersion = Application.ProductVersion;

                    Version v1, v2;
                    v1 = new Version(rel.ReleaseVersion);
                    v2 = new Version(currentVersion);

                    if (v1.CompareTo(v2) > 0) // Test if newer installer exists:
                    {
                        newRelease = rel;
                        LogLineHighlight("New EDDiscovery installer available: " + rel.ReleaseName);
                        return true;
                    }
                }
            }
            catch (Exception)
            {

            }

            return false;
        }
        #endregion

        #region Shutdown
        public void Shutdown()
        {
            PendingClose = true;
            EDDNSync.StopSync();
            journalmonitor.StopMonitor();
            EdsmSync.StopSync();

            LogLineHighlight("Closing down, please wait..");
            Console.WriteLine("Close.. safe close launched");
            closeRequested.Set();
        }
        #endregion

        #region Logging
        public void LogLine(string text)
        {
            LogLineColor(text, GetLogNormalColour());
        }

        public void LogLineHighlight(string text)
        {
            LogLineColor(text, GetLogHighlightColour());
        }

        public void LogLineSuccess(string text)
        {
            LogLineColor(text, GetLogSuccessColour());
        }

        public void LogLineColor(string text, Color color)
        {
            try
            {
                InvokeAsyncOnUIThread(() =>
                {
                    logtext += text + Environment.NewLine;      // keep this, may be the only log showing
                    OnNewLogEntry?.Invoke(text + Environment.NewLine, color);
                });
            }
            catch { }
        }
        #endregion

        #region History
        public bool RefreshHistoryAsync(string netlogpath = null, bool forcenetlogreload = false, bool forcejournalreload = false, bool checkedsm = false, int? currentcmdr = null)
        {
            if (Interlocked.CompareExchange(ref refreshRequestedFlag, 1, 0) == 0)
            {
                InvokeSyncOnUIThread(() =>
                {
                    OnRefreshHistoryRequested?.Invoke();
                    journalmonitor.StopMonitor();
                });

                refreshWorkerArgs = new RefreshWorkerArgs
                {
                    NetLogPath = netlogpath,
                    ForceNetLogReload = forcenetlogreload,
                    ForceJournalReload = forcejournalreload,
                    CheckEdsm = checkedsm,
                    CurrentCommander = currentcmdr ?? DisplayedCommander
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

        public void NewPosition(EliteDangerous.JournalEntry je)
        {
            if (je.CommanderId == DisplayedCommander)     // we are only interested at this point accepting ones for the display commander
            {
                HistoryEntry last = history.GetLast;

                bool journalupdate = false;
                HistoryEntry he = HistoryEntry.FromJournalEntry(je, last, true, out journalupdate);

                if (journalupdate)
                {
                    EliteDangerous.JournalEvents.JournalFSDJump jfsd = je as EliteDangerous.JournalEvents.JournalFSDJump;

                    if (jfsd != null)
                    {
                        EliteDangerous.JournalEntry.UpdateEDSMIDPosJump(jfsd.Id, he.System, !jfsd.HasCoordinate && he.System.HasCoordinate, jfsd.JumpDist);
                    }
                }

                using (SQLiteConnectionUser conn = new SQLiteConnectionUser())
                {
                    he.ProcessWithUserDb(je, last, history, conn);           // let some processes which need the user db to work

                    history.materialcommodititiesledger.Process(je, conn);
                }

                history.Add(he);

                if (je.EventTypeID == JournalTypeEnum.Scan)
                {
                    JournalScan js = je as JournalScan;
                    if (!AddScanToBestSystem(history.starscan, js, history.Count - 1, history.EntryOrder))
                    {
                        LogLineHighlight("Cannot add scan to system - alert the EDDiscovery developers using either discord or Github (see help)" + Environment.NewLine +
                                         "Scan object " + js.BodyName + " in " + he.System.name);
                    }
                }

                OnNewEntry?.Invoke(he, history);
            }
            else if (je.EventTypeID == JournalTypeEnum.LoadGame)
            {
                OnRefreshCommanders?.Invoke();
            }
        }

        public void RecalculateHistoryDBs()         // call when you need to recalc the history dbs - not the whole history. Use RefreshAsync for that
        {
            MaterialCommoditiesLedger matcommodledger = new MaterialCommoditiesLedger();
            StarScan starscan = new StarScan();

            ProcessUserHistoryListEntries(history.EntryOrder, matcommodledger, starscan);

            history.materialcommodititiesledger = matcommodledger; ;
            history.starscan = starscan;

            OnHistoryChange?.Invoke(history);
        }
        #endregion

        #region Sync
        public bool AsyncPerformSync(bool edsmsync = false, bool eddbsync = false)
        {
            if (Interlocked.CompareExchange(ref resyncRequestedFlag, 1, 0) == 0)
            {
                performeddbsync |= eddbsync;
                performedsmsync |= edsmsync;
                resyncRequestedEvent.Set();
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Targets
        public void NewTargetSet()
        {
            System.Diagnostics.Debug.WriteLine("New target set");
            OnNewTarget?.Invoke();
        }
        #endregion
        #endregion
        #endregion

        #region Implementation
        #region Private Properties of Fields
        private ManualResetEvent closeRequested = new ManualResetEvent(false);
        private ManualResetEvent readyForInitialLoad = new ManualResetEvent(false);
        private ManualResetEvent readyForUiInvoke = new ManualResetEvent(false);
        private Task<bool> downloadMapsTask = null;
        private string logname = "";
        private EDJournalClass journalmonitor;
        private bool performedsmsync = false;
        private bool performeddbsync = false;
        private string logtext = "";     // to keep in case of no logs..
        private bool performhistoryrefresh = false;
        private bool syncwasfirstrun = false;
        private bool syncwaseddboredsm = false;
        private Thread backgroundWorker;
        private Thread backgroundRefreshWorker;
        private AutoResetEvent refreshRequested = new AutoResetEvent(false);
        private int refreshRequestedFlag = 0;
        private RefreshWorkerArgs refreshWorkerArgs;
        private AutoResetEvent resyncRequestedEvent = new AutoResetEvent(false);
        private int resyncRequestedFlag = 0;
        private bool CanSkipSlowUpdates
        {
            get
            {
#if DEBUG
                return EDDConfig.Instance.CanSkipSlowUpdates;
#else
                return false;
#endif
            }
        }
        #endregion

        #region Methods requesting information from consumer
        private Func<Color> GetLogNormalColour;
        private Func<Color> GetLogHighlightColour;
        private Func<Color> GetLogSuccessColour;
        #endregion

        #region Methods invoking actions in consumer thread
        private Action<Action> InvokeAsyncOnUIThread;
        private Action<Action> InvokeSyncOnUIThread;
        #endregion

        #region Initialization
        private void InitLogging()
        {
            string logpath = "";
            try
            {
                logpath = Path.Combine(Tools.GetAppDataDirectory(), "Log");
                if (!Directory.Exists(logpath))
                {
                    Directory.CreateDirectory(logpath);
                }

                if (!Debugger.IsAttached || option_tracelog)
                {
                    TraceLog.LogFileWriterException += ex =>
                    {
                        LogLineHighlight($"Log Writer Exception: {ex}");
                    };
                    TraceLog.Init(option_fcexcept);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Unable to create the folder '{logpath}'");
                Trace.WriteLine($"Exception: {ex.Message}");
            }
        }

        private void ProcessCommandLineOptions()
        {
            List<string> parts = Environment.GetCommandLineArgs().ToList();

            option_nowindowreposition = parts.FindIndex(x => x.Equals("-NoRepositionWindow", StringComparison.InvariantCultureIgnoreCase)) != -1 ||
                parts.FindIndex(x => x.Equals("-NRW", StringComparison.InvariantCultureIgnoreCase)) != -1;

            int ai = parts.FindIndex(x => x.Equals("-Appfolder", StringComparison.InvariantCultureIgnoreCase));
            if (ai != -1 && ai < parts.Count - 1)
            {
                Tools.appfolder = parts[ai + 1];
                VersionDisplayString += " (Using " + Tools.appfolder + ")";
            }

            option_debugoptions = parts.FindIndex(x => x.Equals("-Debug", StringComparison.InvariantCultureIgnoreCase)) != -1;
            option_tracelog = parts.FindIndex(x => x.Equals("-TraceLog", StringComparison.InvariantCultureIgnoreCase)) != -1;
            option_fcexcept = parts.FindIndex(x => x.Equals("-LogExceptions", StringComparison.InvariantCultureIgnoreCase)) != -1;

            if (parts.FindIndex(x => x.Equals("-EDSMBeta", StringComparison.InvariantCultureIgnoreCase)) != -1)
            {
                EDSMClass.ServerAddress = "http://beta.edsm.net:8080/";
                VersionDisplayString += " (EDSMBeta)";
            }

            if (parts.FindIndex(x => x.Equals("-EDSMNull", StringComparison.InvariantCultureIgnoreCase)) != -1)
            {
                EDSMClass.ServerAddress = "";
                VersionDisplayString += " (EDSM No server)";
            }

            if (parts.FindIndex(x => x.Equals("-DISABLEBETACHECK", StringComparison.InvariantCultureIgnoreCase)) != -1)
            {
                EliteDangerous.EDJournalReader.disable_beta_commander_check = true;
                VersionDisplayString += " (no BETA detect)";
            }

            int jr = parts.FindIndex(x => x.Equals("-READJOURNAL", StringComparison.InvariantCultureIgnoreCase));   // use this so much to check journal decoding
            if (jr != -1)
            {
                string file = parts[jr + 1];
                System.IO.StreamReader filejr = new System.IO.StreamReader(file);
                string line;
                string system = "";
                StarScan ss = new StarScan();

                while ((line = filejr.ReadLine()) != null)
                {
                    if (line.Equals("END"))
                        break;
                    //System.Diagnostics.Trace.WriteLine(line);
                    if (line.Length > 0)
                    {
                        JObject jo = (JObject)JObject.Parse(line);
                        JSONPrettyPrint jpp = new JSONPrettyPrint(EliteDangerous.JournalEntry.StandardConverters(), "event;timestamp", "_Localised", (string)jo["event"]);
                        string s = jpp.PrettyPrint(line, 80);
                        //System.Diagnostics.Trace.WriteLine(s);

                        EliteDangerous.JournalEntry je = EliteDangerous.JournalEntry.CreateJournalEntry(line);
                        //System.Diagnostics.Trace.WriteLine(je.EventTypeStr);

                        if (je.EventTypeID == JournalTypeEnum.Location)
                        {
                            EDDiscovery.EliteDangerous.JournalEvents.JournalLocOrJump jl = je as EDDiscovery.EliteDangerous.JournalEvents.JournalLocOrJump;
                            system = jl.StarSystem;
                        }
                        else if (je.EventTypeID == JournalTypeEnum.FSDJump)
                        {
                            EDDiscovery.EliteDangerous.JournalEvents.JournalFSDJump jfsd = je as EDDiscovery.EliteDangerous.JournalEvents.JournalFSDJump;
                            system = jfsd.StarSystem;

                        }
                        else if (je.EventTypeID == JournalTypeEnum.Scan)
                        {
                            ss.Process(je as JournalScan, new SystemClass(system));
                        }
                    }
                }
            }
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

        #region Background Worker Thread
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
            ReadyForClose = true;
            InvokeAsyncOnUIThread(() =>
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
                        DoRefreshHistory();
                        break;
                }
            }
        }

        private void BackgroundInit()
        {
            InitializeDatabases();
            readyForUiInvoke.WaitOne();
            InvokeAsyncOnUIThread(() => OnDatabaseInitializationComplete?.Invoke());
            readyForInitialLoad.WaitOne();
            CheckSystems(() => PendingClose, (p, s) => InvokeAsyncOnUIThread(() => ReportProgress(p, s)));
            ReportProgress(-1, "");
            InvokeSyncOnUIThread(() => OnCheckSystemsCompleted?.Invoke());
            if (PendingClose) return;

            if (EDDN.EDDNClass.CheckforEDMC()) // EDMC is running
            {
                if (EDDConfig.Instance.CurrentCommander.SyncToEddn)  // Both EDD and EDMC should not sync to EDDN.
                {
                    LogLineHighlight("EDDiscovery and EDMarketConnector should not both sync to EDDN. Stop EDMC or uncheck 'send to EDDN' in settings tab!");
                }
            }

            if (PendingClose) return;
            GitHubRelease rel;
            if (CheckForNewinstaller(out rel))
            {
                InvokeAsyncOnUIThread(() => OnNewReleaseAvailable?.Invoke(rel));
            }

            if (PendingClose) return;
            LogLine("Reading travel history");
            refreshWorkerArgs = new RefreshWorkerArgs();
            DoRefreshHistory();

            if (PendingClose) return;
            if (performeddbsync || performedsmsync)
            {
                string databases = (performedsmsync && performeddbsync) ? "EDSM and EDDB" : ((performedsmsync) ? "EDSM" : "EDDB");

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
        #endregion

        #region Logging
        private void ReportProgress(int percentComplete, string message)
        {
            InvokeAsyncOnUIThread(() => OnReportProgress?.Invoke(percentComplete, message));
        }
        #endregion

        #region Map Download
        private Task<bool> DownloadMaps()          // ASYNC process
        {
            if (CanSkipSlowUpdates)
            {
                LogLine("Skipping checking for new maps (DEBUG option).");
                var tcs = new TaskCompletionSource<bool>();
                tcs.SetResult(false);
                return tcs.Task;
            }

            try
            {
                if (!Directory.Exists(Path.Combine(Tools.GetAppDataDirectory(), "Maps")))
                    Directory.CreateDirectory(Path.Combine(Tools.GetAppDataDirectory(), "Maps"));

                LogLine("Checking for new EDDiscovery maps");

                DeleteMapFile("DW4.png");
                DeleteMapFile("SC-00.jpg");
                return DownloadMapFiles(new[]
                {
                    "SC-01.jpg",
                    "SC-02.jpg",
                    "SC-03.jpg",
                    "SC-04.jpg",
                    "SC-L4.jpg",
                    "SC-U4.jpg",
                    "SC-00.png",
                    "SC-00.json",
                    "Galaxy_L.jpg",
                    "Galaxy_L.json",
                    "Galaxy_L_Grid.jpg",
                    "Galaxy_L_Grid.json",
                    "DW1.jpg",
                    "DW1.json",
                    "DW2.jpg",
                    "DW2.json",
                    "DW3.jpg",
                    "DW3.json",
                    "DW4.jpg",
                    "DW4.json",
                    "Formidine.png",
                    "Formidine.json",
                    "Formidine trans.png",
                    "Formidine trans.json"
                },
                (s) => LogLine("Map check complete."));
            }
            catch (Exception ex)
            {
                LogLineHighlight("DownloadImages exception: " + ex.Message);
                var tcs = new TaskCompletionSource<bool>();
                tcs.SetException(ex);
                return tcs.Task;
            }
        }

        private Task<bool> DownloadMapFiles(string[] files, Action<bool> callback)
        {
            List<Task<bool>> tasks = new List<Task<bool>>();

            foreach (string file in files)
            {
                var task = EDDiscovery2.HTTP.DownloadFileHandler.DownloadFileAsync(
                    "http://eddiscovery.astronet.se/Maps/" + file,
                    Path.Combine(Tools.GetAppDataDirectory(), "Maps", file),
                    (n) =>
                    {
                        if (n) LogLine("Downloaded map: " + file);
                    }, () => PendingClose);
                tasks.Add(task);
            }

            return Task<bool>.Factory.ContinueWhenAll<bool>(tasks.ToArray(), (ta) =>
            {
                bool success = ta.All(t => t.IsCompleted && t.Result);
                callback(success);
                return success;
            });
        }

        private bool DownloadMapFile(string file)
        {
            bool newfile = false;
            if (EDDiscovery2.HTTP.DownloadFileHandler.DownloadFile("http://eddiscovery.astronet.se/Maps/" + file, Path.Combine(Tools.GetAppDataDirectory(), "Maps", file), out newfile))
            {
                if (newfile)
                    LogLine("Downloaded map: " + file);
                return true;
            }
            else
                return false;
        }

        private void DeleteMapFile(string file)
        {
            string filename = Path.Combine(Tools.GetAppDataDirectory(), "Maps", file);

            try
            {
                if (File.Exists(filename))
                    File.Delete(filename);
            }
            catch (Exception ex)
            {
                LogLine("Exception in DeleteMapFile:" + ex.Message);
            }
        }
        #endregion

        #region Initial Data Load
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
                    performedsmsync = true;
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
                    performeddbsync = true;
            }
        }
        #endregion

        #region History Refresh
        private void DoRefreshHistory()
        {
            RefreshWorkerResults res = RefreshHistoryWorker(refreshWorkerArgs);
            InvokeAsyncOnUIThread(() =>
            {
                RefreshHistoryWorkerCompleted(res);
            });
        }

        private RefreshWorkerResults RefreshHistoryWorker(RefreshWorkerArgs args)
        {
            List<HistoryEntry> hl = new List<HistoryEntry>();
            EDCommander cmdr = null;

            if (args.CurrentCommander >= 0)
            {
                cmdr = EDDConfig.Instance.Commander(args.CurrentCommander);
                journalmonitor.ParseJournalFiles(() => PendingClose, (p, s) => ReportProgress(p, s), forceReload: args.ForceJournalReload);   // Parse files stop monitor..

                if (args != null)
                {
                    if (args.NetLogPath != null)
                    {
                        string errstr = null;
                        NetLogClass.ParseFiles(args.NetLogPath, out errstr, EDDConfig.Instance.DefaultMapColour, () => PendingClose, (p, s) => ReportProgress(p, s), args.ForceNetLogReload, currentcmdrid: args.CurrentCommander);
                    }
                }
            }

            ReportProgress(-1, "Resolving systems");

            List<EliteDangerous.JournalEntry> jlist = EliteDangerous.JournalEntry.GetAll(args.CurrentCommander).OrderBy(x => x.EventTimeUTC).ThenBy(x => x.Id).ToList();
            List<Tuple<EliteDangerous.JournalEntry, HistoryEntry>> jlistUpdated = new List<Tuple<EliteDangerous.JournalEntry, HistoryEntry>>();

            using (SQLiteConnectionSystem conn = new SQLiteConnectionSystem())
            {
                HistoryEntry prev = null;
                foreach (EliteDangerous.JournalEntry je in jlist)
                {
                    bool journalupdate = false;
                    HistoryEntry he = HistoryEntry.FromJournalEntry(je, prev, args.CheckEdsm, out journalupdate, conn, cmdr);
                    prev = he;

                    hl.Add(he);                        // add to the history list here..

                    if (journalupdate)
                    {
                        jlistUpdated.Add(new Tuple<EliteDangerous.JournalEntry, HistoryEntry>(je, he));
                    }
                }
            }

            if (PendingClose)
            {
                return null;
            }

            if (jlistUpdated.Count > 0)
            {
                ReportProgress(-1, "Updating journal entries");

                using (SQLiteConnectionUser conn = new SQLiteConnectionUser(utc: true))
                {
                    using (DbTransaction txn = conn.BeginTransaction())
                    {
                        foreach (Tuple<EliteDangerous.JournalEntry, HistoryEntry> jehe in jlistUpdated)
                        {
                            EliteDangerous.JournalEntry je = jehe.Item1;
                            HistoryEntry he = jehe.Item2;
                            EliteDangerous.JournalEvents.JournalFSDJump jfsd = je as EliteDangerous.JournalEvents.JournalFSDJump;
                            if (jfsd != null)
                            {
                                EliteDangerous.JournalEntry.UpdateEDSMIDPosJump(jfsd.Id, he.System, !jfsd.HasCoordinate && he.System.HasCoordinate, jfsd.JumpDist, conn, txn);
                            }
                        }

                        txn.Commit();
                    }
                }
            }

            // now database has been updated due to initial fill, now fill in stuff which needs the user database

            MaterialCommoditiesLedger matcommodledger = new MaterialCommoditiesLedger();
            StarScan starscan = new StarScan();

            ProcessUserHistoryListEntries(hl, matcommodledger, starscan);      // here, we update the DBs in HistoryEntry and any global DBs in historylist

            return new RefreshWorkerResults { rethistory = hl, retledger = matcommodledger, retstarscan = starscan };
        }

        private void RefreshHistoryWorkerCompleted(RefreshWorkerResults res)
        {
            OnRefreshCommanders?.Invoke();

            if (res != null)
            {
                history.Clear();

                foreach (var ent in res.rethistory)
                {
                    history.Add(ent);
                    Debug.Assert(ent.MaterialCommodity != null);
                }

                history.materialcommodititiesledger = res.retledger;
                history.starscan = res.retstarscan;
            }

            ReportProgress(-1, "");
            LogLine("Refresh Complete.");

            OnHistoryChange?.Invoke(history);

            OnRefreshHistoryWorkerCompleted?.Invoke(res.rethistory, res.retledger, res.retstarscan);

            HistoryRefreshed?.Invoke();

            journalmonitor.StartMonitor();
            refreshRequestedFlag = 0;
        }

        // go thru the history list and reworkout the materials ledger and the materials count, plus any other stuff..
        private void ProcessUserHistoryListEntries(List<HistoryEntry> hl, MaterialCommoditiesLedger ledger, StarScan scan)
        {
            using (SQLiteConnectionUser conn = new SQLiteConnectionUser())      // splitting the update into two, one using system, one using user helped
            {
                for (int i = 0; i < hl.Count; i++)
                {
                    HistoryEntry he = hl[i];
                    JournalEntry je = he.journalEntry;
                    he.ProcessWithUserDb(je, (i > 0) ? hl[i - 1] : null, history, conn);        // let the HE do what it wants to with the user db

                    Debug.Assert(he.MaterialCommodity != null);

                    ledger.Process(je, conn);            // update the ledger

                    if (je.EventTypeID == JournalTypeEnum.Scan)
                    {
                        if (!AddScanToBestSystem(scan, je as JournalScan, i, hl))
                        {
                            System.Diagnostics.Debug.WriteLine("******** Cannot add scan to system " + (je as JournalScan).BodyName + " in " + he.System.name);
                        }
                    }
                }
            }
        }

        private bool AddScanToBestSystem(StarScan starscan, JournalScan je, int startindex, List<HistoryEntry> hl)
        {
            for (int j = startindex; j >= 0; j--)
            {
                if (je.IsStarNameRelated(hl[j].System.name))       // if its part of the name, use it
                {
                    return starscan.Process(je, hl[j].System);
                }
            }

            return starscan.Process(je, hl[startindex].System);         // no relationship, add..
        }
        #endregion

        #region EDSM / EDDB sync
        private void DoPerformSync()
        {
            PerformSync(() => PendingClose, (p, s) => ReportProgress(p, s));
            InvokeAsyncOnUIThread(() => PerformSyncCompleted());
        }

        private void PerformSync(Func<bool> cancelRequested, Action<int, string> reportProgress)           // big check.. done in a thread.
        {
            reportProgress(-1, "");

            performhistoryrefresh = false;
            syncwasfirstrun = SystemClass.IsSystemsTableEmpty();                 // remember if DB is empty

            // Force a full sync if newest data is more than 14 days old

            bool outoforder = SQLiteConnectionSystem.GetSettingBool("EDSMSystemsOutOfOrder", true);
            DateTime lastmod = outoforder ? SystemClass.GetLastSystemModifiedTime() : SystemClass.GetLastSystemModifiedTimeFast();
            if (DateTime.UtcNow.Subtract(lastmod).TotalDays >= 14)
            {
                performedsmsync = true;
            }

            bool edsmoreddbsync = performedsmsync || performeddbsync;           // remember if we are syncing
            syncwaseddboredsm = edsmoreddbsync;

            if (performedsmsync || performeddbsync)
            {
                if (performedsmsync && !cancelRequested())
                {
                    // Download new systems
                    performhistoryrefresh |= PerformEDSMFullSync(cancelRequested, reportProgress);
                }

                if (!cancelRequested())
                {
                    LogLine("Indexing systems table");
                    SQLiteConnectionSystem.CreateSystemsTableIndexes();

                    PerformEDDBFullSync(cancelRequested, reportProgress);
                    performhistoryrefresh = true;
                }
            }

            if (!cancelRequested())
            {
                LogLine("Indexing systems table");
                SQLiteConnectionSystem.CreateSystemsTableIndexes();

                if (CanSkipSlowUpdates)
                {
                    LogLine("Skipping loading updates (DEBUG option). Need to turn this back on again? Look in the Settings tab.");
                }
                else
                {
                    lastmod = outoforder ? SystemClass.GetLastSystemModifiedTime() : SystemClass.GetLastSystemModifiedTimeFast();
                    if (DateTime.UtcNow.Subtract(lastmod).TotalHours >= 1)
                    {
                        LogLine("Checking for new EDSM systems (may take a few moments).");
                        EDSMClass edsm = new EDSMClass();
                        long updates = edsm.GetNewSystems(this, cancelRequested, reportProgress);
                        LogLine("EDSM updated " + updates + " systems.");
                        performhistoryrefresh |= (updates > 0);
                    }
                }
            }

            reportProgress(-1, "");
        }

        private void PerformSyncCompleted()
        {
            long totalsystems = SystemClass.GetTotalSystems();
            LogLineSuccess("Loading completed, total of " + totalsystems + " systems");

            if (performhistoryrefresh)
            {
                LogLine("Refresh due to updating systems");
                HistoryRefreshed += HistoryFinishedRefreshing;
                RefreshHistoryAsync();
            }

            OnPerformSyncCompleted?.Invoke();
            resyncRequestedFlag = 0;
        }

        private void HistoryFinishedRefreshing()
        {
            HistoryRefreshed -= HistoryFinishedRefreshing;
            LogLine("Refreshing complete.");

            if (syncwasfirstrun)
            {
                LogLine("EDSM and EDDB update complete. Please restart ED Discovery to complete the synchronisation ");
            }
            else if (syncwaseddboredsm)
                LogLine("EDSM and/or EDDB update complete.");
        }

        private bool PerformEDSMFullSync(Func<bool> cancelRequested, Action<int, string> reportProgress)
        {
            string rwsystime = SQLiteConnectionSystem.GetSettingString("EDSMLastSystems", "2000-01-01 00:00:00"); // Latest time from RW file.
            DateTime edsmdate;

            if (!DateTime.TryParse(rwsystime, CultureInfo.InvariantCulture, DateTimeStyles.None, out edsmdate))
            {
                edsmdate = new DateTime(2000, 1, 1);
            }

            long updates = 0;

            try
            {
                // Delete all old systems
                SQLiteConnectionSystem.PutSettingString("EDSMLastSystems", "2010-01-01 00:00:00");
                SQLiteConnectionSystem.PutSettingString("EDDBSystemsTime", "0");

                EDSMClass edsm = new EDSMClass();

                LogLine("Get hidden systems from EDSM and remove from database");

                SystemClass.RemoveHiddenSystems();

                if (cancelRequested())
                    return false;

                LogLine("Download systems file from EDSM.");

                string edsmsystems = Path.Combine(Tools.GetAppDataDirectory(), "edsmsystems.json");

                LogLine("Resyncing all downloaded EDSM systems with local database." + Environment.NewLine + "This will take a while.");

                bool newfile;
                bool success = EDDiscovery2.HTTP.DownloadFileHandler.DownloadFile(EDSMClass.ServerAddress + "dump/systemsWithCoordinates.json", edsmsystems, out newfile, (n, s) =>
                {
                    SQLiteConnectionSystem.CreateTempSystemsTable();

                    string rwsysfiletime = "2014-01-01 00:00:00";
                    bool outoforder = false;
                    using (var reader = new StreamReader(s))
                        updates = SystemClass.ParseEDSMUpdateSystemsStream(reader, ref rwsysfiletime, ref outoforder, true, this, cancelRequested, reportProgress, useCache: false, useTempSystems: true);
                    if (!cancelRequested())       // abort, without saving time, to make it do it again
                    {
                        SQLiteConnectionSystem.PutSettingString("EDSMLastSystems", rwsysfiletime);
                        LogLine("Replacing old systems table with new systems table and re-indexing - please wait");
                        reportProgress(-1, "Replacing old systems table with new systems table and re-indexing - please wait");
                        SQLiteConnectionSystem.ReplaceSystemsTable();
                        SQLiteConnectionSystem.PutSettingBool("EDSMSystemsOutOfOrder", outoforder);
                        reportProgress(-1, "");
                    }
                    else
                    {
                        throw new OperationCanceledException();
                    }
                });

                if (!success)
                {
                    LogLine("Failed to download EDSM system file from server, will check next time");
                    return false;
                }

                // Stop if requested
                if (cancelRequested())
                    return false;

                LogLine("Local database updated with EDSM data, " + updates + " systems updated.");

                performedsmsync = false;
                GC.Collect();
            }
            catch (Exception ex)
            {
                LogLineHighlight("GetAllEDSMSystems exception:" + ex.Message);
            }

            return (updates > 0);
        }

        private void PerformEDDBFullSync(Func<bool> cancelRequested, Action<int, string> reportProgress)
        {
            try
            {
                LogLine("Get systems from EDDB.");

                string eddbdir = Path.Combine(Tools.GetAppDataDirectory(), "eddb");
                if (!Directory.Exists(eddbdir))
                    Directory.CreateDirectory(eddbdir);

                string systemFileName = Path.Combine(eddbdir, "systems_populated.jsonl");

                bool success = EDDiscovery2.HTTP.DownloadFileHandler.DownloadFile("http://robert.astronet.se/Elite/eddb/v5/systems_populated.jsonl", systemFileName);

                if (success)
                {
                    if (cancelRequested())
                        return;

                    LogLine("Resyncing all downloaded EDDB data with local database." + Environment.NewLine + "This will take a while.");

                    long number = SystemClass.ParseEDDBUpdateSystems(systemFileName, LogLineHighlight);

                    LogLine("Local database updated with EDDB data, " + number + " systems updated");
                    SQLiteConnectionSystem.PutSettingString("EDDBSystemsTime", DateTime.UtcNow.Ticks.ToString());
                }
                else
                    LogLineHighlight("Failed to download EDDB Systems. Will try again next run.");

                GC.Collect();
                performeddbsync = false;
            }
            catch (Exception ex)
            {
                LogLineHighlight("GetEDDBUpdate exception: " + ex.Message);
            }
        }
        #endregion
        #endregion
    }
}
