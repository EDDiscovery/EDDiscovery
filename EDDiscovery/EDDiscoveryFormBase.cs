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
    public abstract class EDDiscoveryFormBase : Form
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

        protected class TraceLogWriter : TextWriter
        {
            public override Encoding Encoding { get { return Encoding.UTF8; } }
            public override IFormatProvider FormatProvider { get { return CultureInfo.InvariantCulture; } }
            public override void Write(string value) { Trace.Write(value); }
            public override void WriteLine(string value) { Trace.WriteLine(value); }
            public override void WriteLine() { Trace.WriteLine(""); }
        }

        #region Public Interface
        #region Public Properties
        public int DisplayedCommander { get; set; } = 0;
        public HistoryList history { get; protected set; } = new HistoryList();
        public bool option_nowindowreposition { get; protected set; } = false;                             // Cmd line options
        public bool option_debugoptions { get; protected set; } = false;
        public EDSMSync EdsmSync { get; protected set; }
        public string LogText { get { return logtext; } }
        public bool PendingClose { get; private set; }           // we want to close boys!
        public string VersionDisplayString { get; protected set; }
        public GalacticMapping GalacticMapping { get; protected set; }

        public static EDDConfig EDDConfig { get { return EDDConfig.Instance; } }
        public static GalacticMapping galacticMapping { get { return Instance.GalacticMapping; } }
        #endregion

        #region Events
        public event Action HistoryRefreshed; // this is an internal hook
        public event Action<HistoryList> OnHistoryChange;
        public event Action<HistoryEntry, HistoryList> OnNewEntry;
        public event Action<string, Color> OnNewLogEntry;
        #endregion

        #region Methods
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
                    OnRefreshHistoryRequested();
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
            Debug.Assert(Application.MessageLoop);              // ensure.. paranoia

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
                    he.ProcessWithUserDb(je, last, conn);           // let some processes which need the user db to work

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

                if (je.EventTypeID == EliteDangerous.JournalTypeEnum.Scan)
                    OnNewBodyScan(je as JournalScan);
            }
            else if (je.EventTypeID == JournalTypeEnum.LoadGame)
            {
                OnRefreshCommanders();
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
        #endregion
        #endregion

        #region Protected Interface
        #region Protected Properties or Fields
        protected bool ReadyForClose = false;
        protected bool IsDatabaseInitialized { get { return SQLiteConnectionUser.IsInitialized && SQLiteConnectionSystem.IsInitialized; } }
        #endregion

        #region Event handlers to be overridden by subclasses
        protected virtual void OnCheckSystemsCompleted() { }
        protected virtual void OnDatabaseInitializationComplete() { }
        protected virtual void OnSafeClose() { }
        protected virtual void OnFinalClose() { }
        protected virtual void OnNewBodyScan(JournalScan scan) { }
        protected virtual void OnRefreshCommanders() { }
        protected virtual void OnNewReleaseAvailable(GitHubRelease rel) { }
        protected virtual void OnPerformSyncCompleted() { }
        protected virtual void OnRefreshHistoryRequested() { }
        protected virtual void OnRefreshHistoryWorkerCompleted(RefreshWorkerResults res) { }
        protected virtual void OnReportProgress(int percentComplete, string message) { }
        #endregion

        #region Methods requesting information from subclasses
        protected abstract Color GetLogNormalColour();
        protected abstract Color GetLogHighlightColour();
        protected abstract Color GetLogSuccessColour();
        #endregion

        #region Methods called by subclasses
        protected void Init()
        {
            Instance = this;
            VersionDisplayString = "Version " + Assembly.GetExecutingAssembly().FullName.Split(',')[1].Split('=')[1];

            ProcessCommandLineOptions();
            InitLogging();

            SQLiteConnectionUser.EarlyReadRegister();
            EDDConfig.Instance.Update(write: false);

            backgroundWorker = new Thread(BackgroundWorkerThread);
            backgroundWorker.IsBackground = true;
            backgroundWorker.Name = "Background Worker Thread";
            backgroundWorker.Start();

            GalacticMapping = new GalacticMapping();
            EdsmSync = new EDSMSync((EDDiscoveryForm)this);
            EdsmSync.OnDownloadedSystems += () => RefreshHistoryAsync();
            journalmonitor = new EDJournalClass();
            journalmonitor.OnNewJournalEntry += NewPosition;
            DisplayedCommander = EDDiscoveryForm.EDDConfig.CurrentCommander.Nr;
        }

        protected void PostInit_Load()
        {
            EliteDangerousClass.CheckED();
            EDDConfig.Update();
            CheckIfEliteDangerousIsRunning();
        }

        protected void PostInit_Shown()
        {
            readyForInitialLoad.Set();
            downloadMapsTask = DownloadMaps();
        }

        protected bool CheckForNewinstaller(out GitHubRelease newRelease)
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

        protected void Shutdown()
        {
            PendingClose = true;
            EDDNSync.StopSync();
            journalmonitor.StopMonitor();
            EdsmSync.StopSync();

            LogLineHighlight("Closing down, please wait..");
            Console.WriteLine("Close.. safe close launched");
            closeRequested.Set();
            safeClose = new Thread(SafeClose) { Name = "Close Down", IsBackground = true };
            safeClose.Start();
        }
        #endregion
        #endregion

        #region Implementation
        #region Private Properties of Fields
        private static EDDiscoveryFormBase Instance;

        private ManualResetEvent closeRequested = new ManualResetEvent(false);
        private Task<bool> downloadMapsTask = null;
        private string logname = "";
        private EDJournalClass journalmonitor;
        private bool performedsmsync = false;
        private bool performeddbsync = false;
        private string logtext = "";     // to keep in case of no logs..
        private bool performhistoryrefresh = false;
        private bool syncwasfirstrun = false;
        private bool syncwaseddboredsm = false;
        private Thread safeClose;
        private Thread backgroundWorker;
        private Thread backgroundRefreshWorker;
        private AutoResetEvent readyForInitialLoad = new AutoResetEvent(false);
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
                return EDDConfig.CanSkipSlowUpdates;
#else
                return false;
#endif
            }
        }
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

                if (!Debugger.IsAttached)
                {
                    logname = Path.Combine(Tools.GetAppDataDirectory(), "Log", $"Trace_{DateTime.Now.ToString("yyyyMMddHHmmss")}.log");

                    System.Diagnostics.Trace.AutoFlush = true;
                    // Log trace events to the above file
                    System.Diagnostics.Trace.Listeners.Add(new System.Diagnostics.TextWriterTraceListener(logname));
                    // Log unhandled exceptions
                    AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                    // Log unhandled UI exceptions
                    Application.ThreadException += Application_ThreadException;
                    // Redirect console to trace
                    Console.SetOut(new TraceLogWriter());
                    // Log first-chance exceptions to help diagnose errors
                    Register_FirstChanceException_Handler();
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

        #region Unexpected exception handling
        // We can't prevent an unhandled exception from killing the application.
        // See https://blog.codinghorror.com/improved-unhandled-exception-behavior-in-net-20/
        // Log the exception info if we can, and ask the user to report it.
        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions]
        [System.Security.SecurityCritical]
        [System.Runtime.ConstrainedExecution.ReliabilityContract(
            System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState,
            System.Runtime.ConstrainedExecution.Cer.Success)]
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                System.Diagnostics.Trace.WriteLine($"\n==== UNHANDLED EXCEPTION ====\n{e.ExceptionObject.ToString()}\n==== cut ====");
                MessageBox.Show($"There was an unhandled exception.\nPlease report this at https://github.com/EDDiscovery/EDDiscovery/issues and attach {logname}\nException: {e.ExceptionObject.ToString()}\n\nThis application must now close", "Unhandled Exception");
            }
            catch
            {
            }

            Environment.Exit(1);
        }

        // Handling a ThreadException leaves the application in an undefined state.
        // See https://msdn.microsoft.com/en-us/library/system.windows.forms.application.threadexception(v=vs.100).aspx
        // Log the exception, ask the user to report it, and exit.
        private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                System.Diagnostics.Trace.WriteLine($"\n==== UNHANDLED EXCEPTION ON {Thread.CurrentThread.Name} THREAD ====\n{e.Exception.ToString()}\n==== cut ====");
                MessageBox.Show($"There was an unhandled exception.\nPlease report this at https://github.com/EDDiscovery/EDDiscovery/issues and attach {logname}\nException: {e.Exception.Message}\n{e.Exception.StackTrace}\n\nThis application must now close", "Unhandled Exception");
            }
            catch
            {
            }

            Environment.Exit(1);
        }

        // Mono does not implement AppDomain.CurrentDomain.FirstChanceException
        private static void Register_FirstChanceException_Handler()
        {
            try
            {
                Type adtype = AppDomain.CurrentDomain.GetType();
                EventInfo fcexevent = adtype.GetEvent("FirstChanceException");
                if (fcexevent != null)
                {
                    fcexevent.AddEventHandler(AppDomain.CurrentDomain, new EventHandler<System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs>(CurrentDomain_FirstChanceException));
                }
            }
            catch
            {
            }
        }

        // Log exceptions were they occur so we can try to  some
        // hard to debug issues.
        private static void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            // Ignore HTTP NotModified exceptions
            if (e.Exception is System.Net.WebException)
            {
                var webex = (WebException)e.Exception;
                if (webex.Response != null && webex.Response is HttpWebResponse)
                {
                    var resp = (HttpWebResponse)webex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotModified)
                    {
                        return;
                    }
                }
            }
            // Ignore DLL Not Found exceptions from OpenTK
            else if (e.Exception is DllNotFoundException && e.Exception.Source == "OpenTK")
            {
                return;
            }

            var trace = new StackTrace(1, true);

            // Ignore first-chance exceptions in threads outside our code
            bool ourcode = false;
            foreach (var frame in trace.GetFrames())
            {
                if (frame.GetMethod().DeclaringType.Assembly == Assembly.GetExecutingAssembly())
                {
                    ourcode = true;
                    break;
                }
            }

            if (ourcode)
                System.Diagnostics.Trace.WriteLine($"First chance exception: {e.Exception.Message}\n{trace.ToString()}");
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

                DoPerformSync();

                while (!PendingClose)
                {
                    switch (WaitHandle.WaitAny(new WaitHandle[] { closeRequested, resyncRequestedEvent }))
                    {
                        case 0:  // Close Requested
                            break;
                        case 1:  // Resync Requested
                            DoPerformSync();
                            break;
                    }
                }

                backgroundRefreshWorker.Join();
            }

            closeRequested.WaitOne();
        }

        private void BackgroundRefreshWorkerThread()
        {
            while (!PendingClose)
            {
                switch (WaitHandle.WaitAny(new WaitHandle[] { closeRequested, refreshRequested }))
                {
                    case 0:  // Close Requested
                        break;
                    case 1:  // Refresh Requested
                        DoRefreshHistory();
                        break;
                }
            }
        }

        private void InvokeAsyncOnUIThread(Action a)
        {
            BeginInvoke(a);
        }

        private void InvokeSyncOnUIThread(Action a)
        {
            Invoke(a);
        }

        private void BackgroundInit()
        {
            InitializeDatabases();
            InvokeAsyncOnUIThread(() => OnDatabaseInitializationComplete());
            readyForInitialLoad.WaitOne();
            CheckSystems(() => PendingClose, (p, s) => InvokeAsyncOnUIThread(() => ReportProgress(p, s)));
            ReportProgress(-1, "");
            InvokeSyncOnUIThread(() => OnCheckSystemsCompleted());
            if (PendingClose) return;

            if (EDDN.EDDNClass.CheckforEDMC()) // EDMC is running
            {
                if (EDDConfig.Instance.CurrentCommander.SyncToEddn)  // Both EDD and EDMC should not sync to EDDN.
                {
                    LogLineHighlight("EDDiscovery and EDMarketConnector should not both sync to EDDN. Stop EDMC or uncheck 'send to EDDN' in settings tab!");
                }
            }

            DeleteOldLogFiles();
            if (PendingClose) return;
            GitHubRelease rel;
            if (CheckForNewinstaller(out rel))
            {
                InvokeAsyncOnUIThread(() => OnNewReleaseAvailable(rel));
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
            InvokeAsyncOnUIThread(() => OnReportProgress(percentComplete, message));
        }

        private void DeleteOldLogFiles()
        {
            try
            {
                // Create a reference to the Log directory.
                DirectoryInfo di = new DirectoryInfo(Path.Combine(Tools.GetAppDataDirectory(), "Log"));

                Trace.WriteLine("Running logfile age check");
                // Create an array representing the files in the current directory.
                FileInfo[] fi = di.GetFiles("*.log");

                System.Collections.IEnumerator myEnum = fi.GetEnumerator();

                while (myEnum.MoveNext())
                {
                    FileInfo fiTemp = (FileInfo)(myEnum.Current);

                    DateTime time = fiTemp.CreationTime;

                    //Trace.WriteLine(String.Format("File {0}  time {1}", fiTemp.Name, __box(time)));

                    TimeSpan maxage = new TimeSpan(30, 0, 0, 0);
                    TimeSpan fileage = DateTime.Now - time;

                    if (fileage > maxage)
                    {
                        Trace.WriteLine(String.Format("File {0} is older then maximum age. Removing file from Logs.", fiTemp.Name));
                        fiTemp.Delete();
                    }
                }
            }
            catch
            {
            }
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
                GalacticMapping.DownloadFromEDSM();

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
                GalacticMapping.ParseData();                            // at this point, EDSM data is loaded..
                SystemClass.AddToAutoComplete(GalacticMapping.GetGMONames());
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
                cmdr = EDDConfig.Commander(args.CurrentCommander);
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

            MaterialCommoditiesLedger matcommodledger = new MaterialCommoditiesLedger();
            StarScan starscan = new StarScan();

            ProcessUserHistoryListEntries(hl, matcommodledger, starscan);      // here, we update the DBs in HistoryEntry and any global DBs in historylist

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

            return new RefreshWorkerResults { rethistory = hl, retledger = matcommodledger, retstarscan = starscan };
        }

        private void RefreshHistoryWorkerCompleted(RefreshWorkerResults res)
        {
            OnRefreshCommanders();

            history.Clear();

            foreach (var ent in res.rethistory)
            {
                history.Add(ent);
                Debug.Assert(ent.MaterialCommodity != null);
            }

            history.materialcommodititiesledger = res.retledger;
            history.starscan = res.retstarscan;

            ReportProgress(-1, "");
            LogLine("Refresh Complete.");

            OnHistoryChange?.Invoke(history);

            OnRefreshHistoryWorkerCompleted(res);

            HistoryRefreshed?.Invoke();

            journalmonitor.StartMonitor();
            refreshRequestedFlag = 0;
        }

        // go thru the hisotry list and reworkout the materials ledge and the materials count
        private void ProcessUserHistoryListEntries(List<HistoryEntry> hl, MaterialCommoditiesLedger ledger, StarScan scan)
        {
            using (SQLiteConnectionUser conn = new SQLiteConnectionUser())      // splitting the update into two, one using system, one using user helped
            {
                for (int i = 0; i < hl.Count; i++)
                {
                    HistoryEntry he = hl[i];
                    JournalEntry je = he.journalEntry;
                    he.ProcessWithUserDb(je, (i > 0) ? hl[i - 1] : null, conn);        // let the HE do what it wants to with the user db

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
                        long updates = edsm.GetNewSystems((EDDiscoveryForm)this, cancelRequested, reportProgress);
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

            OnPerformSyncCompleted();
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
                        updates = SystemClass.ParseEDSMUpdateSystemsStream(reader, ref rwsysfiletime, ref outoforder, true, (EDDiscoveryForm)this, cancelRequested, reportProgress, useCache: false, useTempSystems: true);
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

        #region Shutdown
        private void SafeClose()
        {
            OnSafeClose();
            backgroundWorker.Join();
            ReadyForClose = true;
            InvokeAsyncOnUIThread(() =>
            {
                OnFinalClose();
            });
        }
        #endregion
        #endregion
    }
}
