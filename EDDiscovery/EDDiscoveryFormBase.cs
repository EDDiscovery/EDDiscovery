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

        #region Properties, Fields and Events

        #region Public Properties
        public int DisplayedCommander { get; set; } = 0;
        public HistoryList history { get; protected set; } = new HistoryList();
        public bool option_nowindowreposition { get; protected set; } = false;                             // Cmd line options
        public bool option_debugoptions { get; protected set; } = false;
        public EDSMSync EdsmSync { get; protected set; }
        public string LogText { get { return logtext; } }
        public bool PendingClose { get { return safeClose != null; } }           // we want to close boys!
        public string VersionDisplayString { get; protected set; }

        public static EDDConfig EDDConfig { get; protected set; }
        public static GalacticMapping galacticMapping { get; protected set; }
        #endregion

        #region Events
        public event Action HistoryRefreshed; // this is an internal hook
        public event Action<HistoryList> OnHistoryChange;
        public event Action<HistoryEntry, HistoryList> OnNewEntry;
        public event Action<string, Color> OnNewLogEntry;
        #endregion

        #region Protected Properties or Fields
        protected ManualResetEvent _syncWorkerCompletedEvent = new ManualResetEvent(false);
        protected ManualResetEvent _checkSystemsWorkerCompletedEvent = new ManualResetEvent(false);
        protected Task<bool> downloadMapsTask = null;
        protected Task checkInstallerTask = null;
        protected string logname = "";
        protected BackgroundWorker dbinitworker = null;
        protected EDJournalClass journalmonitor;
        protected GitHubRelease newRelease;
        protected bool performedsmsync = false;
        protected bool performeddbsync = false;
        protected string logtext = "";     // to keep in case of no logs..
        protected bool performhistoryrefresh = false;
        protected bool syncwasfirstrun = false;
        protected bool syncwaseddboredsm = false;
        protected Thread safeClose;
        protected System.Windows.Forms.Timer closeTimer;
        protected Thread backgroundWorker;
        protected AutoResetEvent readyForInitialLoad;
        protected AutoResetEvent refreshRequested;
        protected RefreshWorkerArgs refreshWorkerArgs;
        protected AutoResetEvent resyncRequested;

        protected bool CanSkipSlowUpdates
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

        #endregion

        #region Event Callers
        protected void InvokeOnNewLogEntry(string text, Color color)
        {
            OnNewLogEntry?.Invoke(text, color);
        }

        protected void InvokeOnNewEntry(HistoryEntry l, HistoryList hl)
        {
            OnNewEntry?.Invoke(l, hl);
        }

        protected void InvokeOnHistoryChange(HistoryList hl)
        {
            OnHistoryChange?.Invoke(hl);
        }

        protected void InvokeHistoryRefreshed()
        {
            HistoryRefreshed?.Invoke();
        }
        #endregion

        #region Initialization
        public EDDiscoveryFormBase()
        {

        }

        protected void Init()
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

            EDDConfig = EDDConfig.Instance;
            galacticMapping = new GalacticMapping();
            EdsmSync = new EDSMSync((EDDiscoveryForm)this);
            journalmonitor = new EDJournalClass();
            DisplayedCommander = EDDiscoveryForm.EDDConfig.CurrentCommander.Nr;
        }

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
            InitializeDatabases();
            readyForInitialLoad.WaitOne();
        }

        protected void InvokeAsyncOnUIThread(Action a)
        {
            BeginInvoke(a);
        }

        protected void InvokeSyncOnUIThread(Action a)
        {
            Invoke(a);
        }

        private void InitializeDatabases()
        {
            Trace.WriteLine("Initializing database");
            SQLiteConnectionOld.Initialize();
            SQLiteConnectionUser.Initialize();
            SQLiteConnectionSystem.Initialize();
            Trace.WriteLine("Database initialization complete");
            InvokeAsyncOnUIThread(() => DatabaseInitializationComplete());
        }

        protected abstract void DatabaseInitializationComplete();
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
                    InvokeOnNewLogEntry(text + Environment.NewLine, color);
                });
            }
            catch { }
        }

        protected abstract Color GetLogNormalColour();
        protected abstract Color GetLogHighlightColour();
        protected abstract Color GetLogSuccessColour();

        public abstract void ReportProgress(int percentComplete, string message);

        protected void DeleteOldLogFiles()
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
        public Task<bool> DownloadMaps()          // ASYNC process
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
    }
}
