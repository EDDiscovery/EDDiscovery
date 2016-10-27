using EDDiscovery.DB;
using EDDiscovery2;
using EDDiscovery2.DB;
using EDDiscovery2.EDSM;
using EDDiscovery2.Forms;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Configuration;
using EDDiscovery.EDSM;
using System.Threading.Tasks;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;
using EDDiscovery.HTTP;
using EDDiscovery.Forms;

namespace EDDiscovery
{

    public delegate void DistancesLoaded();

    public partial class EDDiscoveryForm : Form
    {
        #region Variables

        public const int WM_MOVE = 3;
        public const int WM_SIZE = 5;
        public const int WM_MOUSEMOVE = 0x200;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int WM_NCLBUTTONUP = 0xA2;
        public const int WM_NCMOUSEMOVE = 0xA0;
        public const int HT_CLIENT = 0x1;
        public const int HT_CAPTION = 0x2;
        public const int HT_BOTTOMRIGHT = 0x11;
        public const int WM_NCL_RESIZE = 0x112;
        public const int HT_RESIZE = 61448;
        public const int WM_NCHITTEST = 0x84;

        private IntPtr SendMessage(int msg, IntPtr wparam, IntPtr lparam)
        {
            Message message = Message.Create(this.Handle, msg, wparam, lparam);
            this.WndProc(ref message);
            return message.Result;
        }

        // Mono compatibility
        private bool _window_dragging = false;
        private Point _window_dragMousePos = Point.Empty;
        private Point _window_dragWindowPos = Point.Empty;
        public EDDTheme theme;

        public string CommanderName { get; private set; }
        public int DisplayedCommander = 0;

        public HistoryList history = new HistoryList();

        static public EDDConfig EDDConfig { get; private set; }

        public TravelHistoryControl TravelControl { get { return travelHistoryControl1; } }
        public RouteControl RouteControl { get { return routeControl1; } }

        public bool option_nowindowreposition { get; set; } = false;                             // Cmd line options
        public bool option_debugoptions { get; set; } = false;

        public EDDiscovery2._3DMap.MapManager Map { get; private set; }

        public event EventHandler HistoryRefreshed;

        static public GalacticMapping galacticMapping;

        public CancellationTokenSource CancellationTokenSource { get; private set; } = new CancellationTokenSource();

        private ManualResetEvent _syncWorkerCompletedEvent = new ManualResetEvent(false);
        private ManualResetEvent _checkSystemsWorkerCompletedEvent = new ManualResetEvent(false);

        public EDSMSync EdsmSync;

        Action cancelDownloadMaps = null;
        Task<bool> downloadMapsTask = null;
        Task checkInstallerTask = null;
        private string logname = "";

        EliteDangerous.EDJournalClass journalmonitor;
        GitHubRelease newRelease;

        private bool CanSkipSlowUpdates()
        {
#if DEBUG
            return EDDConfig.CanSkipSlowUpdates;
#else
            return false;
#endif
        }

        #endregion

        #region Initialisation

        private class TraceLogWriter : TextWriter
        {
            public override Encoding Encoding { get { return Encoding.UTF8; } }
            public override IFormatProvider FormatProvider { get { return CultureInfo.InvariantCulture; } }
            public override void Write(string value) { Trace.Write(value); }
            public override void WriteLine(string value) { Trace.WriteLine(value); }
            public override void WriteLine() { Trace.WriteLine(""); }
        }

        public EDDiscoveryForm()
        {
            var splashform = Forms.SplashForm.ShowAsync();
            InitializeComponent();

            label_version.Text = "Version " + Assembly.GetExecutingAssembly().FullName.Split(',')[1].Split('=')[1];

            ProcessCommandLineOptions();

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
                }
                // Log first-chance exceptions to help diagnose errors
                Register_FirstChanceException_Handler();
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Unable to create the folder '{logpath}'");
                Trace.WriteLine($"Exception: {ex.Message}");
            }
            theme = new EDDTheme();

            EDDConfig = EDDConfig.Instance;
            galacticMapping = new GalacticMapping();

            ToolStripManager.Renderer = theme.toolstripRenderer;
            theme.LoadThemes();                                         // default themes and ones on disk loaded
            theme.RestoreSettings();                                    // theme, remember your saved settings

            trilaterationControl.InitControl(this);
            travelHistoryControl1.InitControl(this);
            imageHandler1.InitControl(this);
            settings.InitControl(this);
            journalViewControl1.InitControl(this);
            routeControl1.InitControl(this);
            savedRouteExpeditionControl1.InitControl(this);
            exportControl1.InitControl(this);


            EdsmSync = new EDSMSync(this);

            Map = new EDDiscovery2._3DMap.MapManager(option_nowindowreposition, travelHistoryControl1);

            journalmonitor = new EliteDangerous.EDJournalClass();

            this.TopMost = EDDConfig.KeepOnTop;

            ApplyTheme();

            if (splashform != null)
            {
                splashform.CloseForm();
            }

            DisplayedCommander = EDDiscoveryForm.EDDConfig.CurrentCommander.Nr;
        }

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

            System.Diagnostics.Trace.WriteLine($"First chance exception: {e.Exception.Message}\n{trace.ToString()}");
        }

        private void EDDiscoveryForm_Layout(object sender, LayoutEventArgs e)       // Manually position, could not get gripper under tab control with it sizing for the life of me
        {
        }

        private void ProcessCommandLineOptions()
        {
            List<string> parts = Environment.GetCommandLineArgs().ToList();

            option_nowindowreposition = parts.FindIndex(x => x.Equals("-NoRepositionWindow", StringComparison.InvariantCultureIgnoreCase)) != -1 ||
                parts.FindIndex(x => x.Equals("-NRW", StringComparison.InvariantCultureIgnoreCase)) != -1;

            int ai = parts.FindIndex(x => x.Equals("-Appfolder", StringComparison.InvariantCultureIgnoreCase));
            if ( ai != -1 && ai < parts.Count - 1)
            {
                Tools.appfolder = parts[ai + 1];
                label_version.Text += " (Using " + Tools.appfolder + ")";
            }

            option_debugoptions = parts.FindIndex(x => x.Equals("-Debug", StringComparison.InvariantCultureIgnoreCase)) != -1;

            if (parts.FindIndex(x => x.Equals("-EDSMBeta", StringComparison.InvariantCultureIgnoreCase)) != -1)
            {
                EDSMClass.ServerAddress = "http://beta.edsm.net:8080/";
                label_version.Text += " (EDSMBeta)";
            }

            if (parts.FindIndex(x => x.Equals("-EDSMNull", StringComparison.InvariantCultureIgnoreCase)) != -1)
            {
                EDSMClass.ServerAddress = "";
                label_version.Text += " (EDSM No server)";
            }

            if (parts.FindIndex(x => x.Equals("-DISABLEBETACHECK", StringComparison.InvariantCultureIgnoreCase)) != -1)
            {
                EliteDangerous.EDJournalReader.disable_beta_commander_check = true;
                label_version.Text += " (no BETA detect)";
            }

            int jr = parts.FindIndex(x => x.Equals("-READJOURNAL", StringComparison.InvariantCultureIgnoreCase));   // use this so much to check journal decoding
            if (jr != -1)
            {
                string file = parts[jr + 1];
                System.IO.StreamReader filejr = new System.IO.StreamReader(file);
                string line;
                while ((line = filejr.ReadLine()) != null)
                {
                    if (line.Equals("END"))
                        break;
                    System.Diagnostics.Trace.WriteLine(line);
                    if (line.Length > 0 )
                    {
                        JObject jo = (JObject)JObject.Parse(line);

                        JSONPrettyPrint jpp = new JSONPrettyPrint(EliteDangerous.JournalEntry.StandardConverters(), "event;timestamp", "_Localised", (string)jo["event"]);
                        string s = jpp.PrettyPrint(line, 80);
                        System.Diagnostics.Trace.WriteLine(s);

                        EliteDangerous.JournalEntry je = EliteDangerous.JournalEntry.CreateJournalEntry(line);
                        System.Diagnostics.Trace.WriteLine(je.EventTypeStr);
                    }
                }
            }
        }

        private void EDDiscoveryForm_Load(object sender, EventArgs e)
        {
            try
            {
                EliteDangerousClass.CheckED();
                EDDConfig.Update();
                RepositionForm();
                InitFormControls();
                settings.InitSettingsTab();

                CheckIfEliteDangerousIsRunning();

                if (option_debugoptions)
                {
                    button_test.Visible = true;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("EDDiscoveryForm_Load exception: " + ex.Message + "\n" + "Trace: " + ex.StackTrace);
            }
        }

        private void EDDiscoveryForm_Shown(object sender, EventArgs e)
        {
            _checkSystemsWorker.RunWorkerAsync();
            downloadMapsTask = DownloadMaps((cb) => cancelDownloadMaps = cb);
        }

        private Task CheckForNewInstaller()
        {
            return Task.Factory.StartNew(() =>
            {
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
                            this.BeginInvoke(new Action(() => travelHistoryControl1.LogLineHighlight("New EDDiscovery installer available: " + rel.ReleaseName)));
                            this.BeginInvoke(new Action(() => PanelInfoNewRelease()));

                        }
                    }
                }
                catch (Exception )
                {

                }
            });
        }


        private void PanelInfoNewRelease()
        {
            panelInfo.BackColor = Color.Green;
            labelPanelText.Text = "Download new release!";
            panelInfo.Visible = true;
        }


        private void InitFormControls()
        {
            labelPanelText.Text = "Loading. Please wait!";
            panelInfo.Visible = true;
            panelInfo.BackColor = Color.Gold;

            routeControl1.travelhistorycontrol1 = travelHistoryControl1;
        }

        private void RepositionForm()
        {
            var top = SQLiteDBClass.GetSettingInt("FormTop", -1);
            if (top >= 0 && option_nowindowreposition == false)
            {
                var left = SQLiteDBClass.GetSettingInt("FormLeft", 0);
                var height = SQLiteDBClass.GetSettingInt("FormHeight", 800);
                var width = SQLiteDBClass.GetSettingInt("FormWidth", 800);

                // Adjust so window fits on screen; just in case user unplugged a monitor or something

                var screen = SystemInformation.VirtualScreen;
                if (height > screen.Height) height = screen.Height;
                if (top + height > screen.Height + screen.Top) top = screen.Height + screen.Top - height;
                if (width > screen.Width) width = screen.Width;
                if (left + width > screen.Width + screen.Left) left = screen.Width + screen.Left - width;
                if (top < screen.Top) top = screen.Top;
                if (left < screen.Left) left = screen.Left;

                this.Top = top;
                this.Left = left;
                this.Height = height;
                this.Width = width;

                this.CreateParams.X = this.Left;
                this.CreateParams.Y = this.Top;
                this.StartPosition = FormStartPosition.Manual;

            }

            travelHistoryControl1.LoadLayoutSettings();
            journalViewControl1.LoadLayoutSettings();
        }

        private void CheckIfEliteDangerousIsRunning()
        {
            if (EliteDangerousClass.EDRunning)
            {
                travelHistoryControl1.LogLine("EliteDangerous is running.");
            }
            else
            {
                travelHistoryControl1.LogLine("EliteDangerous is not running.");
            }
        }

        private void EDDiscoveryForm_Activated(object sender, EventArgs e)
        {
        }

        public void ApplyTheme()
        {
            ToolStripManager.Renderer = theme.toolstripRenderer;
            this.FormBorderStyle = theme.WindowsFrame ? FormBorderStyle.Sizable : FormBorderStyle.None;
            panel_close.Visible = !theme.WindowsFrame;
            panel_minimize.Visible = !theme.WindowsFrame;
            label_version.Visible = !theme.WindowsFrame;

            this.Text = "EDDiscovery " + label_version.Text;            // note in no border mode, this is not visible on the title bar but it is in the taskbar..

            theme.ApplyColors(this);

            travelHistoryControl1.Display();                         // so we repaint this with correct colours.

            TravelControl.RedrawSummary();
        }

#endregion

#region Information Downloads

        public Task<bool> DownloadMaps(Action<Action> registerCancelCallback)          // ASYNC process
        {
            if (CanSkipSlowUpdates())
            {
                travelHistoryControl1.LogLine("Skipping checking for new maps (DEBUG option).");
                var tcs = new TaskCompletionSource<bool>();
                tcs.SetResult(false);
                return tcs.Task;
            }

            try
            {
                if (!Directory.Exists(Path.Combine(Tools.GetAppDataDirectory(), "Maps")))
                    Directory.CreateDirectory(Path.Combine(Tools.GetAppDataDirectory(), "Maps"));

                travelHistoryControl1.LogLine("Checking for new EDDiscovery maps");

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
                (s) => travelHistoryControl1.LogLine("Map check complete."),
                registerCancelCallback);
            }
            catch (Exception ex)
            {
                travelHistoryControl1.LogLineHighlight("DownloadImages exception: " + ex.Message);
                var tcs = new TaskCompletionSource<bool>();
                tcs.SetException(ex);
                return tcs.Task;
            }
        }

        private Task<bool> DownloadMapFiles(string[] files, Action<bool> callback, Action<Action> registerCancelCallback)
        {
            List<Task<bool>> tasks = new List<Task<bool>>();
            List<Action> cancelCallbacks = new List<Action>();

            foreach (string file in files)
            {
                var task = EDDiscovery2.HTTP.DownloadFileHandler.BeginDownloadFile(
                    "http://eddiscovery.astronet.se/Maps/" + file,
                    Path.Combine(Tools.GetAppDataDirectory(), "Maps", file),
                    (n) =>
                    {
                        if (n) travelHistoryControl1.LogLine("Downloaded map: " + file);
                    }, cb => cancelCallbacks.Add(cb));
                tasks.Add(task);
            }

            registerCancelCallback(() => { foreach (var cb in cancelCallbacks) cb(); });

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
                    travelHistoryControl1.LogLine("Downloaded map: " + file);
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
                travelHistoryControl1.LogLine("Exception in DeleteMapFile:" + ex.Message);
            }
        }

#endregion

#region Initial Check Systems

        bool performedsmsync = false;
        bool performeddbsync = false;

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

            CommanderName = EDDConfig.CurrentCommander.Name;

            string rwsystime = SQLiteConnectionSystem.GetSettingString("EDSMLastSystems", "2000-01-01 00:00:00"); // Latest time from RW file.
            DateTime edsmdate;

            if (!DateTime.TryParse(rwsystime, CultureInfo.InvariantCulture, DateTimeStyles.None, out edsmdate))
            {
                edsmdate = new DateTime(2000, 1, 1);
            }

            if (DateTime.Now.Subtract(edsmdate).TotalDays > 7)  // Over 7 days do a sync from EDSM
            {
                // Also update galactic mapping from EDSM 
                travelHistoryControl1.LogLine("Get galactic mapping from EDSM.");
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
                SQLiteDBUserClass.TranferVisitedSystemstoJournalTableIfRequired();
                SQLiteDBSystemClass.CreateSystemsTableIndexes();
                SystemNoteClass.GetAllSystemNotes();                                // fill up memory with notes, bookmarks, galactic mapping
                BookmarkClass.GetAllBookmarks();
                galacticMapping.ParseData();                            // at this point, EDSM data is loaded..
                SystemClass.AddToAutoComplete(galacticMapping.GetGMONames());

                travelHistoryControl1.LogLine("Loaded Notes, Bookmarks and Galactic mapping.");

                string timestr = SQLiteConnectionSystem.GetSettingString("EDDBSystemsTime", "0");
                DateTime time = new DateTime(Convert.ToInt64(timestr), DateTimeKind.Utc);
                if (DateTime.UtcNow.Subtract(time).TotalDays > 6.5)     // Get EDDB data once every week.
                    performeddbsync = true;
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
                    travelHistoryControl1.LogLineHighlight("Check Systems exception: " + ex.Message + Environment.NewLine + "Trace: " + ex.StackTrace);
                }

                imageHandler1.StartWatcher();
                routeControl1.EnableRouteTab(); // now we have systems, we can update this..

                routeControl1.travelhistorycontrol1 = travelHistoryControl1;
                journalmonitor.OnNewJournalEntry += NewPosition;
                EdsmSync.OnDownloadedSystems += RefreshDueToEDSMDownloadedSystems;


                travelHistoryControl1.LogLine("Reading travel history");
                HistoryRefreshed += _travelHistoryControl1_InitialRefreshDone;

                RefreshHistoryAsync();

                DeleteOldLogFiles();


                panelInfo.Visible = false;

                checkInstallerTask = CheckForNewInstaller();
            }
        }

        private void RefreshDueToEDSMDownloadedSystems()
        {
            Invoke((MethodInvoker)delegate
            {
                RefreshHistoryAsync();
            });
        }


        private void _travelHistoryControl1_InitialRefreshDone(object sender, EventArgs e)
        {
            HistoryRefreshed -= _travelHistoryControl1_InitialRefreshDone;

            if (!PendingClose)
            {
                AsyncPerformSync();                              // perform any async synchronisations

                if (performeddbsync || performedsmsync)
                {
                    string databases = (performedsmsync && performeddbsync) ? "EDSM and EDDB" : ((performedsmsync) ? "EDSM" : "EDDB");

                    travelHistoryControl1.LogLine("ED Discovery will now synchronise to the " + databases + " databases to obtain star information." + Environment.NewLine +
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

        private void AsyncPerformSync()
        {
            if (!_syncWorker.IsBusy)
            {
                edsmRefreshTimer.Enabled = false;
                _syncWorker.RunWorkerAsync();
            }
        }

        private void _syncWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                var worker = (System.ComponentModel.BackgroundWorker)sender;

                PerformSync(() => worker.CancellationPending, (p, s) => worker.ReportProgress(p, s));
                if (worker.CancellationPending)
                    e.Cancel = true;
            }
            catch (Exception ex) { e.Result = ex; }       // ignore any excepctions
            finally
            {
                _syncWorkerCompletedEvent.Set();
            }
        }

        bool performhistoryrefresh = false;
        bool syncwasfirstrun = false;
        bool syncwaseddboredsm = false;

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
                    performhistoryrefresh |= PerformEDSMFullSync(this, cancelRequested, reportProgress);
                }

                if (!cancelRequested())
                {
                    travelHistoryControl1.LogLine("Indexing systems table");
                    SQLiteDBSystemClass.CreateSystemsTableIndexes();

                    PerformEDDBFullSync(cancelRequested, reportProgress);
                    performhistoryrefresh = true;
                }
            }

            if (!cancelRequested())
            {
                travelHistoryControl1.LogLine("Indexing systems table");
                SQLiteDBSystemClass.CreateSystemsTableIndexes();

                if (CanSkipSlowUpdates())
                {
                    travelHistoryControl1.LogLine("Skipping loading updates (DEBUG option). Need to turn this back on again? Look in the Settings tab.");
                }
                else
                {
                    travelHistoryControl1.LogLine("Checking for new EDSM systems (may take a few moments).");
                    EDSMClass edsm = new EDSMClass();
                    long updates = edsm.GetNewSystems(this, cancelRequested, reportProgress);
                    travelHistoryControl1.LogLine("EDSM updated " + updates + " systems.");
                    performhistoryrefresh |= (updates > 0);
                }
            }

            reportProgress(-1, "");
        }

        private void _syncWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Exception ex = e.Cancelled ? null : (e.Error ?? e.Result as Exception);
            ReportProgress(-1, "");

            if (!e.Cancelled && !PendingClose)
            {
                if (ex != null)
                {
                    travelHistoryControl1.LogLineHighlight("Check Systems exception: " + ex.Message + Environment.NewLine + "Trace: " + ex.StackTrace);
                }

                long totalsystems = SystemClass.GetTotalSystems();
                travelHistoryControl1.LogLineSuccess("Loading completed, total of " + totalsystems + " systems");

                if (performhistoryrefresh)
                {
                    travelHistoryControl1.LogLine("Refresh due to updating systems");
                    HistoryRefreshed += HistoryFinishedRefreshing;
                    RefreshHistoryAsync();
                }

                edsmRefreshTimer.Enabled = true;
            }
        }

        private void HistoryFinishedRefreshing(object sender, EventArgs e)
        {
            HistoryRefreshed -= HistoryFinishedRefreshing;
            travelHistoryControl1.LogLine("Refreshing complete.");

            if (syncwasfirstrun)
            {
                travelHistoryControl1.LogLine("EDSM and EDDB update complete. Please restart ED Discovery to complete the synchronisation ");
            }
            else if (syncwaseddboredsm)
                travelHistoryControl1.LogLine("EDSM and/or EDDB update complete.");
        }

        private void _syncWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            ReportProgress(e.ProgressPercentage, (string)e.UserState);
        }

#endregion

#region EDSM and EDDB syncs code

        private bool PerformEDSMFullSync(EDDiscoveryForm discoveryform, Func<bool> cancelRequested, Action<int, string> reportProgress)
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

                travelHistoryControl1.LogLine("Get hidden systems from EDSM and remove from database");

                SystemClass.RemoveHiddenSystems();

                if (cancelRequested())
                    return false;

                travelHistoryControl1.LogLine("Download systems file from EDSM.");

                string edsmsystems = Path.Combine(Tools.GetAppDataDirectory(), "edsmsystems.json");

                travelHistoryControl1.LogLine("Resyncing all downloaded EDSM systems with local database." + Environment.NewLine + "This will take a while.");

                bool newfile;
                bool success = EDDiscovery2.HTTP.DownloadFileHandler.DownloadFile(EDSMClass.ServerAddress + "dump/systemsWithCoordinates.json", edsmsystems, out newfile, (n, s) =>
                {
                    SQLiteDBSystemClass.CreateTempSystemsTable();

                    string rwsysfiletime = "2014-01-01 00:00:00";
                    bool outoforder = false;
                    using (var reader = new StreamReader(s))
                        updates = SystemClass.ParseEDSMUpdateSystemsStream(reader, ref rwsysfiletime, ref outoforder, true, discoveryform, cancelRequested, reportProgress, useCache: false, useTempSystems: true);
                    if (!cancelRequested())       // abort, without saving time, to make it do it again
                    {
                        SQLiteConnectionSystem.PutSettingString("EDSMLastSystems", rwsysfiletime);
                        travelHistoryControl1.LogLine("Replacing old systems table with new systems table and re-indexing - please wait");
                        reportProgress(-1, "Replacing old systems table with new systems table and re-indexing - please wait");
                        SQLiteDBSystemClass.ReplaceSystemsTable();
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
                    travelHistoryControl1.LogLine("Failed to download EDSM system file from server, will check next time");
                    return false;
                }

                // Stop if requested
                if (cancelRequested())
                    return false;

                travelHistoryControl1.LogLine("Local database updated with EDSM data, " + updates + " systems updated.");

                performedsmsync = false;
                GC.Collect();
            }
            catch (Exception ex)
            {
                travelHistoryControl1.LogLineHighlight("GetAllEDSMSystems exception:" + ex.Message);
            }

            return (updates > 0);
        }

        private void edsmRefreshTimer_Tick(object sender, EventArgs e)
        {
            AsyncPerformSync();
        }

        private void PerformEDDBFullSync(Func<bool> cancelRequested, Action<int, string> reportProgress)
        {
            try
            {
                travelHistoryControl1.LogLine("Get systems from EDDB.");

                string systemFileName = Path.Combine(Tools.GetAppDataDirectory(), "eddbsystems.json");
                bool success = EDDiscovery2.HTTP.DownloadFileHandler.DownloadFile("http://robert.astronet.se/Elite/eddb/v4/systems_populated.json", systemFileName);

                if (success)
                {
                    if (cancelRequested())
                        return;

                    travelHistoryControl1.LogLine("Resyncing all downloaded EDDB data with local database." + Environment.NewLine + "This will take a while.");

                    long number = SystemClass.ParseEDDBUpdateSystems(systemFileName, travelHistoryControl1.LogLineHighlight);

                    travelHistoryControl1.LogLine("Local database updated with EDDB data, " + number + " systems updated");
                    SQLiteConnectionSystem.PutSettingString("EDDBSystemsTime", DateTime.UtcNow.Ticks.ToString());
                }
                else
                    travelHistoryControl1.LogLineHighlight("Failed to download EDDB Systems. Will try again next run.");

                GC.Collect();
                performeddbsync = false;
            }
            catch (Exception ex)
            {
                travelHistoryControl1.LogLineHighlight("GetEDDBUpdate exception: " + ex.Message);
            }
        }


        #endregion

        #region Logging

        public void LogLine(string s)
        {
            travelHistoryControl1.LogLine(s);
        }

        public void LogLineHighlight(string s)
        {
            travelHistoryControl1.LogLineHighlight(s);
        }

        public void ReportProgress(int percentComplete, string message)
        {
            if (!PendingClose)
            {
                if (percentComplete >= 0)
                {
                    this.toolStripProgressBar1.Visible = true;
                    this.toolStripProgressBar1.Value = percentComplete;
                }
                else
                {
                    this.toolStripProgressBar1.Visible = false;
                }

                this.toolStripStatusLabel1.Text = message;
            }
        }

        void DeleteOldLogFiles()
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

#region JSONandMisc
        static public string LoadJsonFile(string filename)
        {
            string json = null;
            try
            {
                if (!File.Exists(filename))
                    return null;

                StreamReader reader = new StreamReader(filename);
                json = reader.ReadToEnd();
                reader.Close();
            }
            catch
            {
            }

            return json;
        }

        internal void ShowTrilaterationTab()
        {
            tabControl1.SelectedIndex = 1;
        }

#endregion

#region Closing

        private void SaveSettings()
        {
            settings.SaveSettings();

            SQLiteDBClass.PutSettingInt("FormWidth", this.Width);
            SQLiteDBClass.PutSettingInt("FormHeight", this.Height);
            SQLiteDBClass.PutSettingInt("FormTop", this.Top);
            SQLiteDBClass.PutSettingInt("FormLeft", this.Left);
            routeControl1.SaveSettings();
            theme.SaveSettings(null);
            travelHistoryControl1.SaveSettings();
            journalViewControl1.SaveSettings();

        }

        Thread safeClose;
        System.Windows.Forms.Timer closeTimer;

        public bool PendingClose { get { return safeClose != null; } }           // we want to close boys!

        private void EDDiscoveryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (safeClose == null)                  // so a close is a request now, and it launches a thread which cleans up the system..
            {
                e.Cancel = true;
                edsmRefreshTimer.Enabled = false;
                CancellationTokenSource.Cancel();
                CancelHistoryRefresh();
                _syncWorker.CancelAsync();
                _checkSystemsWorker.CancelAsync();
                if (cancelDownloadMaps != null)
                {
                    cancelDownloadMaps();
                }
                labelPanelText.Text = "Closing, please wait!";
                panelInfo.Visible = true;
                travelHistoryControl1.LogLineHighlight("Closing down, please wait..");
                Console.WriteLine("Close.. safe close launched");
                safeClose = new Thread(SafeClose) { Name = "Close Down", IsBackground = true };
                safeClose.Start();
            }
            else if (safeClose.IsAlive)   // still working, cancel again..
            {
                e.Cancel = true;
            }
            else
            {
                Console.WriteLine("go for close");
            }
        }

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

            travelHistoryControl1.CloseClosestSystemThread();

            Console.WriteLine("Go for close timer!");

            Invoke((MethodInvoker)delegate          // we need this thread to die so close will work, so kick off a timer
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
                SaveSettings();         // do close now
                Close();
                Application.Exit();
            }
        }

#endregion

#region ButtonsAndMouse

        private void button_test_Click(object sender, EventArgs e)
        {
        }

        private void addNewStarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://robert.astronet.se/Elite/ed-systems/entry.html");
        }

        private void frontierForumThreadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://forums.frontier.co.uk/showthread.php?t=138155&p=2113535#post2113535");
        }

        private void eDDiscoveryFGESupportThreadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://firstgreatexpedition.org/mybb/showthread.php?tid=1406");
        }

        private void eDDiscoveryHomepageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/EDDiscovery/EDDiscovery/wiki");
        }

        private void openEliteDangerousDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (EliteDangerousClass.EDDirectory != null && !EliteDangerousClass.EDDirectory.Equals(""))
                    Process.Start(EliteDangerousClass.EDDirectory);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Open EliteDangerous directory exception: " + ex.Message);
            }

        }

        private void showLogfilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                EDCommander cmdr = EDDConfig.Instance.ListOfCommanders.Find(x => x.Nr == EDDConfig.Instance.CurrentCmdrID);

                if (cmdr != null)
                {
                    string cmdrfolder = cmdr.JournalDir;
                    if (cmdrfolder.Length < 1)
                        cmdrfolder = EliteDangerous.EDJournalClass.GetDefaultJournalDir();
                    Process.Start(cmdrfolder);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Show log files exception: " + ex.Message);
            }
        }

        private void statisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StatsForm frm = new StatsForm();

            frm._discoveryForm = this;
            frm.Show();

        }

        private void show2DMapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSagCarinaMission frm = new FormSagCarinaMission(history.FilterByFSDAndPosition);
            frm.Nowindowreposition = option_nowindowreposition;
            frm.Show();
        }

        private void show3DMapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TravelControl.buttonMap_Click(sender, e);
        }



        private void forceEDDBUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_syncWorker.IsBusy)      // we want it to have run, to completion, to allow another go..
            {
                performeddbsync = true;
                AsyncPerformSync();
            }
            else
                MessageBox.Show("Synchronisation to databases is in operation or pending, please wait");
        }

        private void syncEDSMSystemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_syncWorker.IsBusy)      // we want it to have run, to completion, to allow another go..
            {
                performedsmsync = true;
                AsyncPerformSync();
            }
            else
                MessageBox.Show("Synchronisation to databases is in operation or pending, please wait");
        }

        private void gitHubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/EDDiscovery/EDDiscovery");
        }

        private void reportIssueIdeasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/EDDiscovery/EDDiscovery/issues");
        }

        internal void keepOnTopChanged(bool keepOnTop)
        {
            this.TopMost = keepOnTop;
        }

        private void panel_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void changeMapColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settings.panel_defaultmapcolor_Click(sender, e);
        }

        private void editThemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settings.button_edittheme_Click(this, null);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AboutBox()
        {
            AboutForm frm = new AboutForm();
            frm.labelVersion.Text = this.Text;
            frm.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox();
        }

        private void eDDiscoveryChatDiscordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/0qIqfCQbziTWzsQu");
        }

        private void EDDiscoveryForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (!theme.WindowsFrame && e.Button == MouseButtons.Left)           // only if theme is borderless
            {
                this.Capture = false;
                SendMessage(WM_NCLBUTTONDOWN, (IntPtr)HT_CAPTION, IntPtr.Zero);
            }
        }

        protected override void WndProc(ref Message m)
        {
            // Compatibility movement for Mono
            if (m.Msg == WM_LBUTTONDOWN && (int)m.WParam == 1 && !theme.WindowsFrame)
            {
                int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                int y = unchecked((short)((uint)m.LParam >> 16));
                _window_dragMousePos = new Point(x, y);
                _window_dragWindowPos = this.Location;
                _window_dragging = true;
                m.Result = IntPtr.Zero;
                this.Capture = true;
            }
            else if (m.Msg == WM_MOUSEMOVE && (int)m.WParam == 1 && _window_dragging)
            {
                int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                int y = unchecked((short)((uint)m.LParam >> 16));
                Point delta = new Point(x - _window_dragMousePos.X, y - _window_dragMousePos.Y);
                _window_dragWindowPos = new Point(_window_dragWindowPos.X + delta.X, _window_dragWindowPos.Y + delta.Y);
                this.Location = _window_dragWindowPos;
                this.Update();
                m.Result = IntPtr.Zero;
            }
            else if (m.Msg == WM_LBUTTONUP)
            {
                _window_dragging = false;
                _window_dragMousePos = Point.Empty;
                _window_dragWindowPos = Point.Empty;
                m.Result = IntPtr.Zero;
                this.Capture = false;
            }
            // Windows honours NCHITTEST; Mono does not
            else if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m);

                if ((int)m.Result == HT_CLIENT)
                {
                    int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                    int y = unchecked((short)((uint)m.LParam >> 16));
                    Point p = PointToClient(new Point(x, y));

                    if (p.X > this.ClientSize.Width - statusStrip1.Height && p.Y > this.ClientSize.Height - statusStrip1.Height)
                    {
                        m.Result = (IntPtr)HT_BOTTOMRIGHT;
                    }
                    else if (!theme.WindowsFrame)
                    {
                        m.Result = (IntPtr)HT_CAPTION;
                    }
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        private void menuStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            EDDiscoveryForm_MouseDown(sender, e);
        }

        private void paneleddiscovery_Click(object sender, EventArgs e)
        {
            AboutBox();
        }

        private void panel_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void read21AndFormerLogFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            adminToolStripMenuItem.DropDown.Close();
            if (DisplayedCommander >= 0)
            {
                EDCommander cmdr = EDDConfig.ListOfCommanders.Find(c => c.Nr == DisplayedCommander);
                if (cmdr != null)
                {
                    string netlogpath = cmdr.NetLogDir;
                    FolderBrowserDialog dirdlg = new FolderBrowserDialog();
                    if (netlogpath != null && Directory.Exists(netlogpath))
                    {
                        dirdlg.SelectedPath = netlogpath;
                    }

                    DialogResult dlgResult = dirdlg.ShowDialog();

                    if (dlgResult == DialogResult.OK)
                    {
                        string logpath = dirdlg.SelectedPath;

                        if (logpath != netlogpath)
                        {
                            cmdr.NetLogDir = logpath;
                            EDDConfig.UpdateCommanders(new List<EDCommander> { cmdr });
                        }

                        //string logpath = "c:\\games\\edlaunch\\products\\elite-dangerous-64\\logs";
                        RefreshHistoryAsync(netlogpath: logpath, forcenetlogreload: false, currentcmdr: cmdr.Nr);
                    }
                }
            }
        }

        private void read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DisplayedCommander >= 0)
            {
                EDCommander cmdr = EDDConfig.ListOfCommanders.Find(c => c.Nr == DisplayedCommander);
                if (cmdr != null)
                {
                    string netlogpath = cmdr.NetLogDir;
                    FolderBrowserDialog dirdlg = new FolderBrowserDialog();
                    if (netlogpath != null && Directory.Exists(netlogpath))
                    {
                        dirdlg.SelectedPath = netlogpath;
                    }

                    DialogResult dlgResult = dirdlg.ShowDialog();

                    if (dlgResult == DialogResult.OK)
                    {
                        string logpath = dirdlg.SelectedPath;

                        if (logpath != netlogpath)
                        {
                            cmdr.NetLogDir = logpath;
                            EDDConfig.UpdateCommanders(new List<EDCommander> { cmdr });
                        }

                        //string logpath = "c:\\games\\edlaunch\\products\\elite-dangerous-64\\logs";
                        RefreshHistoryAsync(netlogpath: logpath, forcenetlogreload: true, currentcmdr: cmdr.Nr);
                    }
                }
            }
        }

        private void dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm you wish to reset all history entries to the current commander", "WARNING", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                EliteDangerous.JournalEntry.ResetCommanderID(-1, EDDConfig.CurrentCommander.Nr);
                RefreshHistoryAsync();
            }
        }


        private void rescanAllJournalFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshHistoryAsync(forcejournalreload: true, checkedsm: true);
        }


        #endregion

        #region Update Views with new commander 

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
        }

        public void RefreshHistoryAsync(string netlogpath = null, bool forcenetlogreload = false, bool forcejournalreload = false, bool checkedsm = false, int? currentcmdr = null)
        {
            if (PendingClose)
            {
                return;
            }

            if (!_refreshWorker.IsBusy)
            {
                travelHistoryControl1.RefreshButton(false);
                journalViewControl1.RefreshButton(false);

                journalmonitor.StopMonitor();          // this is called by the foreground.  Ensure background is stopped.  Foreground must restart it.

                RefreshWorkerArgs args = new RefreshWorkerArgs
                {
                    NetLogPath = netlogpath,
                    ForceNetLogReload = forcenetlogreload,
                    ForceJournalReload = forcejournalreload,
                    CheckEdsm = checkedsm,
                    CurrentCommander = currentcmdr ?? DisplayedCommander
                };
                _refreshWorker.RunWorkerAsync(args);
            }
        }

        public void CancelHistoryRefresh()
        {
            _refreshWorker.CancelAsync();
        }

        private void RefreshHistoryWorker(object sender, DoWorkEventArgs e)
        {
            RefreshWorkerArgs args = e.Argument as RefreshWorkerArgs;
            var worker = (BackgroundWorker)sender;

            List<HistoryEntry> history = new List<HistoryEntry>();
            MaterialCommoditiesLedger matcommodledger = new MaterialCommoditiesLedger();

            if (args.CurrentCommander >= 0)
            {
                journalmonitor.ParseJournalFiles(() => worker.CancellationPending, (p, s) => worker.ReportProgress(p, s), forceReload: args.ForceJournalReload);   // Parse files stop monitor..

                if (args != null)
                {
                    if (args.NetLogPath != null)
                    {
                        string errstr = null;
                        NetLogClass.ParseFiles(args.NetLogPath, out errstr, EDDConfig.Instance.DefaultMapColour, () => worker.CancellationPending, (p, s) => worker.ReportProgress(p, s), args.ForceNetLogReload, currentcmdrid: args.CurrentCommander);
                    }
                }
            }

            worker.ReportProgress(-1, "Resolving systems");

            List<EliteDangerous.JournalEntry> jlist = EliteDangerous.JournalEntry.GetAll(DisplayedCommander).OrderBy(x => x.EventTimeUTC).ThenBy(x => x.Id).ToList();
            List<Tuple<EliteDangerous.JournalEntry, HistoryEntry>> jlistUpdated = new List<Tuple<EliteDangerous.JournalEntry, HistoryEntry>>();

            //ISystem isys = new SystemClass("Unknown");

            using (SQLiteConnectionSystem conn = new SQLiteConnectionSystem())
            {
                HistoryEntry prev = null;
                foreach (EliteDangerous.JournalEntry je in jlist)
                {
                    bool journalupdate = false;
                    HistoryEntry he = HistoryEntry.FromJournalEntry(je, prev, args.CheckEdsm, out journalupdate, conn);
                    prev = he;

                    history.Add(he);                        // add to the history list here..
                    matcommodledger.Process(je);            // update the ledger

                    if (journalupdate)
                    {
                        jlistUpdated.Add(new Tuple<EliteDangerous.JournalEntry, HistoryEntry>(je, he));
                    }
                }
            }

            if (worker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            if (jlistUpdated.Count > 0)
            {
                worker.ReportProgress(-1, "Updating journal entries");

                using (SQLiteConnectionUserUTC conn = new SQLiteConnectionUserUTC())
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

            if (worker.CancellationPending)
            {
                e.Cancel = true;
            }
            else
            {
                e.Result = new RefreshWorkerResults { rethistory = history, retledger = matcommodledger };
            }
        }

        private void RefreshHistoryWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled && !PendingClose)
            {
                if (e.Error != null)
                {
                    travelHistoryControl1.LogLineHighlight("History Refresh Error: " + e.Error.Message);
                }
                else
                {
                    travelHistoryControl1.CheckCommandersListBox();             // in case a new commander has been detected
                    settings.UpdateCommandersListBox();

                    history.Clear();

                    foreach (var ent in ((RefreshWorkerResults)e.Result).rethistory)
                    {
                        history.Add(ent);
                    }

                    history.materialcommodititiesledger = ((RefreshWorkerResults)e.Result).retledger;

                    ReportProgress(-1, "");
                    travelHistoryControl1.LogLine("Refresh Complete." );

                    RefreshDisplays();
                }

                travelHistoryControl1.RefreshButton(true);
                journalViewControl1.RefreshButton(true);

                if (HistoryRefreshed != null)
                    HistoryRefreshed(this, EventArgs.Empty);

                journalmonitor.StartMonitor();
            }
        }

        private void RefreshHistoryWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string name = (string)e.UserState;
            ReportProgress(e.ProgressPercentage, $"Processing log file {name}");
        }

        public void RefreshFrontEnd()           // this just does another fetch and redisplay, does not check log files, or edsm systems
        {
            RefreshDisplays();
        }

        public void RefreshDisplays()
        {
            travelHistoryControl1.Display();
            journalViewControl1.Display();
        }

        public void NewPosition(EliteDangerous.JournalEntry je)
        {
            Debug.Assert(Application.MessageLoop);              // ensure.. paranoia

            if (je.CommanderId == DisplayedCommander)     // we are only interested at this point accepting ones for the display commander
            {
                bool journalupdate = false;
                HistoryEntry he = HistoryEntry.FromJournalEntry(je, history.GetLast, true, out journalupdate);

                if (journalupdate)
                {
                    EliteDangerous.JournalEvents.JournalFSDJump jfsd = je as EliteDangerous.JournalEvents.JournalFSDJump;

                    if (jfsd != null)
                    {
                        EliteDangerous.JournalEntry.UpdateEDSMIDPosJump(jfsd.Id, he.System, !jfsd.HasCoordinate && he.System.HasCoordinate, jfsd.JumpDist);
                    }
                }

                history.Add(he);
//TBD
                travelHistoryControl1.AddNewEntry(he);
                journalViewControl1.AddNewEntry(he);
            }

            travelHistoryControl1.CheckCommandersListBox();
            settings.UpdateCommandersListBox();
        }


        #endregion



        private void panelInfo_Click(object sender, EventArgs e)
        {
            if (newRelease!=null)
            {
                NewReleaseForm frm = new NewReleaseForm();
                frm.release = newRelease;

                frm.ShowDialog(this);
            }
        }

        private void labelPanelText_Click(object sender, EventArgs e)
        {
            if (newRelease != null)
            {
                NewReleaseForm frm = new NewReleaseForm();
                frm.release = newRelease;

                frm.ShowDialog(this);
            }
        }
    }
}

