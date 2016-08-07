using EDDiscovery.DB;
using EDDiscovery2;
using EDDiscovery2.DB;
using EDDiscovery2.EDDB;
using EDDiscovery2.EDSM;
using EDDiscovery2.Forms;
using EDDiscovery2.PlanetSystems;
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

        public AutoCompleteStringCollection SystemNames;       

        public string CommanderName { get; private set; }
        static public EDDConfig EDDConfig { get; private set; }

        public TravelHistoryControl TravelControl { get { return travelHistoryControl1; } }
        public RouteControl RouteControl { get { return routeControl1;  } }
        public List<VisitedSystemsClass> VisitedSystems { get { return travelHistoryControl1.visitedSystems; } }

        public bool option_nowindowreposition { get; set;  }  = false;                             // Cmd line options
        public bool option_debugoptions { get; set; } = false;

        public EDDiscovery2._3DMap.MapManager Map { get; private set; }

        public GalacticMapping galacticMapping;

        public CancellationTokenSource CancellationTokenSource { get; private set; } = new CancellationTokenSource();

        private ManualResetEvent _syncWorkerCompletedEvent = new ManualResetEvent(false);
        private ManualResetEvent _checkSystemsWorkerCompletedEvent = new ManualResetEvent(false);

        Action cancelDownloadMaps = null;
        Task<bool> downloadMapsTask = null;
        private string logname;
        private bool logsetupfailed;

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
            InitializeComponent();
            ProcessCommandLineOptions();

            theme = new EDDTheme();

            EDDConfig = EDDConfig.Instance;
            galacticMapping = new GalacticMapping();

            string logpath = "";
            try
            {
                logpath = Path.Combine(Tools.GetAppDataDirectory(), "Log");
                if (!Directory.Exists(logpath))
                {
                    Directory.CreateDirectory(logpath);
                }

                logname = Path.Combine(Tools.GetAppDataDirectory(), "Log", $"Trace_{DateTime.Now.ToString("yyyyMMddHHmmss")}.log");

                System.Diagnostics.Trace.AutoFlush = true;
                // Log trace events to the above file
                System.Diagnostics.Trace.Listeners.Add(new System.Diagnostics.TextWriterTraceListener(logname));
                // Log first-chance exceptions to help diagnose errors
                AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
                // Log unhandled exceptions
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                // Log unhandled UI exceptions
                Application.ThreadException += Application_ThreadException;
                // Always direct unhandled exceptions to the above handler
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                // Redirect console to trace
                Console.SetOut(new TraceLogWriter());
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Unable to create the folder '{logpath}'");
                Trace.WriteLine($"Exception: {ex.Message}");
                logsetupfailed = true;
            }

            ToolStripManager.Renderer = theme.toolstripRenderer;
            theme.LoadThemes();                                         // default themes and ones on disk loaded
            theme.RestoreSettings();                                    // theme, remember your saved settings

            trilaterationControl.InitControl(this);
            travelHistoryControl1.InitControl(this);
            imageHandler1.InitControl(this);
            settings.InitControl(this);
            routeControl1.InitControl(this);
            savedRouteExpeditionControl1.InitControl(this);

            SystemNames = new AutoCompleteStringCollection();
            Map = new EDDiscovery2._3DMap.MapManager(option_nowindowreposition,travelHistoryControl1);

            this.TopMost = EDDConfig.KeepOnTop;

            ApplyTheme(false);
        }

        // We can't prevent an unhandled exception from killing the application.
        // See https://blog.codinghorror.com/improved-unhandled-exception-behavior-in-net-20/
        // Log the exception info if we can, and ask the user to report it.
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

            System.Diagnostics.Trace.WriteLine($"First chance exception: {e.Exception.ToString()}");
        }

        private void EDDiscoveryForm_Layout(object sender, LayoutEventArgs e)       // Manually position, could not get gripper under tab control with it sizing for the life of me
        {
            /*
            if (panel_grip.Visible)
            {
                panel_grip.Location = new Point(this.ClientSize.Width - panel_grip.Size.Width, this.ClientSize.Height - panel_grip.Size.Height);
                tabControl1.Size = new Size(this.ClientSize.Width - panel_grip.Size.Width, this.ClientSize.Height - panel_grip.Size.Height - tabControl1.Location.Y);
            }
            else
                tabControl1.Size = new Size(this.ClientSize.Width, this.ClientSize.Height - tabControl1.Location.Y);
             */
        }

        private void ProcessCommandLineOptions()
        {
            string cmdline = Environment.CommandLine;
            option_nowindowreposition = (cmdline.IndexOf("-NoRepositionWindow", 0, StringComparison.InvariantCultureIgnoreCase) != -1 || cmdline.IndexOf("-NRW", 0, StringComparison.InvariantCultureIgnoreCase) != -1);

            int pos = cmdline.IndexOf("-Appfolder", 0, StringComparison.InvariantCultureIgnoreCase);
            if ( pos != -1 )
            {
                string[] nextwords = cmdline.Substring(pos + 10).Trim().Split(' ');
                if (nextwords.Length > 0)
                    Tools.appfolder = nextwords[0];
            }

            option_debugoptions = cmdline.IndexOf("-Debug", 0, StringComparison.InvariantCultureIgnoreCase) != -1;
        }

        private void EDDiscoveryForm_Load(object sender, EventArgs e)
        {
            try
            {
                EliteDangerous.CheckED();
                EDDConfig.Update();
                RepositionForm();
                InitFormControls();
                settings.InitSettingsTab();

                CheckIfEliteDangerousIsRunning();

                if (option_debugoptions)
                {
                    button_test.Visible = true;
                    prospectingToolStripMenuItem.Visible = true;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("EDDiscoveryForm_Load exception: " + ex.Message);
                System.Windows.Forms.MessageBox.Show("Trace: " + ex.StackTrace);
            }
        }

        private void EDDiscoveryForm_Shown(object sender, EventArgs e)
        {
            _checkSystemsWorker.RunWorkerAsync();
            downloadMapsTask = DownloadMaps((cb) => cancelDownloadMaps = cb);
        }

        private void CheckForNewInstaller()
        {
            {
                EDDiscoveryServer eds = new EDDiscoveryServer();

                string inst = eds.GetLastestInstaller();
                if (inst != null)
                {
                    JObject jo = (JObject)JObject.Parse(inst);

                    string newVersion = jo["Version"].Value<string>();
                    string newInstaller = jo["Filename"].Value<string>();

                    var currentVersion = Application.ProductVersion;

                    Version v1, v2;
                    v1 = new Version(newVersion);
                    v2 = new Version(currentVersion);

                    if (v1.CompareTo(v2) > 0) // Test if newver installer exists:
                    {
                        LogLineHighlight("New EDDiscovery installer available " + "http://eddiscovery.astronet.se/release/" + newInstaller);
                    }

                }
            }
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
            if (top >= 0 && option_nowindowreposition == false )
            {
                var left = SQLiteDBClass.GetSettingInt("FormLeft", 0);
                var height = SQLiteDBClass.GetSettingInt("FormHeight", 800);
                var width = SQLiteDBClass.GetSettingInt("FormWidth", 800);

                // Adjust so window fits on screen; just in case user unplugged a monitor or something

                var screen = SystemInformation.VirtualScreen; 
                if( height > screen.Height ) height = screen.Height;
                if( top + height > screen.Height + screen.Top) top = screen.Height + screen.Top - height;
                if( width > screen.Width ) width = screen.Width;
                if( left + width > screen.Width + screen.Left ) left = screen.Width + screen.Left - width;
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
        }

        private void CheckIfEliteDangerousIsRunning()
        {
            if (EliteDangerous.EDRunning)
            {
                LogLine("EliteDangerous is running.");
            }
            else
            {
                LogLine("EliteDangerous is not running.");
            }
        }

        private void EDDiscoveryForm_Activated(object sender, EventArgs e)
        {
        }

        public void ApplyTheme(bool refreshhistory)
        {
            ToolStripManager.Renderer = theme.toolstripRenderer;
            this.FormBorderStyle = theme.WindowsFrame ? FormBorderStyle.Sizable : FormBorderStyle.None;
            //panel_grip.Visible = !theme.WindowsFrame;
            panel_close.Visible = !theme.WindowsFrame;
            panel_minimize.Visible = !theme.WindowsFrame;
            label_version.Visible = !theme.WindowsFrame;
            label_version.Text = "Version " + Assembly.GetExecutingAssembly().FullName.Split(',')[1].Split('=')[1];
            if (Tools.appfolder != "EDDiscovery")
                label_version.Text += " (Using " + Tools.appfolder +")";

            this.Text = "EDDiscovery " + label_version.Text;            // note in no border mode, this is not visible on the title bar but it is in the taskbar..

            theme.ApplyColors(this);

            if (refreshhistory)
                travelHistoryControl1.RefreshHistoryAsync();             // so we repaint this with correct colours.

            TravelControl.RedrawSummary();
        }

        #endregion

        #region Information Downloads

        public Task<bool> DownloadMaps(Action<Action> registerCancelCallback)          // ASYNC process
        {
            if (CanSkipSlowUpdates())
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
                (s) => LogLine("Map check complete."),
                registerCancelCallback);

                /*
                if (DownloadMapFile("SC-01.jpg"))  // If server down only try one.
                {
                    DownloadMapFile("SC-02.jpg");
                    DownloadMapFile("SC-03.jpg");
                    DownloadMapFile("SC-04.jpg");

                    DownloadMapFile("SC-L4.jpg");
                    DownloadMapFile("SC-U4.jpg");

                    DownloadMapFile("SC-00.png");
                    DownloadMapFile("SC-00.json");


                    DownloadMapFile("Galaxy_L.jpg");
                    DownloadMapFile("Galaxy_L.json");
                    DownloadMapFile("Galaxy_L_Grid.jpg");
                    DownloadMapFile("Galaxy_L_Grid.json");

                    DownloadMapFile("DW1.jpg");
                    DownloadMapFile("DW1.json");
                    DownloadMapFile("DW2.jpg");
                    DownloadMapFile("DW2.json");
                    DownloadMapFile("DW3.jpg");
                    DownloadMapFile("DW3.json");
                    DownloadMapFile("DW4.jpg");
                    DownloadMapFile("DW4.json");

                    DownloadMapFile("Formidine.png");
                    DownloadMapFile("Formidine.json");
                    DownloadMapFile("Formidine trans.png");
                    DownloadMapFile("Formidine trans.json");
                    
                }
                */
            }
            catch (Exception ex)
            {
                MessageBox.Show("DownloadImages exception: " + ex.Message, "ERROR", MessageBoxButtons.OK);
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
                var task = EDDBClass.BeginDownloadFile(
                    "http://eddiscovery.astronet.se/Maps/" + file,
                    Path.Combine(Tools.GetAppDataDirectory(), "Maps", file),
                    (n) =>
                    {
                        if (n) LogLine("Downloaded map: " + file);
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
            if (EDDBClass.DownloadFile("http://eddiscovery.astronet.se/Maps/" + file, Path.Combine(Tools.GetAppDataDirectory(), "Maps", file), out newfile))
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

        bool performedsmsync = false;
        bool performeddbsync = false;
        bool performedsmdistsync = false;
        bool performhistoryrefresh = false;
        bool syncwasfirstrun = false;
        bool syncwaseddboredsm = false;

        private void CheckSystems(Func<bool> cancelRequested, Action<int, string> reportProgress)  // ASYNC process, done via start up, must not be too slow.
        {
            reportProgress(-1, "");
            CommanderName = EDDConfig.CurrentCommander.Name;

            EDSMClass edsm = new EDSMClass();
            string rwsystime = SQLiteDBClass.GetSettingString("EDSMLastSystems", "2000-01-01 00:00:00"); // Latest time from RW file.
            DateTime edsmdate = DateTime.Parse(rwsystime, new CultureInfo("sv-SE"));

            if (DateTime.Now.Subtract(edsmdate).TotalDays > 7)  // Over 7 days do a sync from EDSM
            {
                performedsmsync = true;

                // Also update galactic mapping from EDSM (MOVED here for now since we don't use this yet..)
                LogLine("Get galactic mapping from EDSM.");
                galacticMapping.DownloadFromEDSM();
            }
            else
            {
                if (CanSkipSlowUpdates())
                {
                    LogLine("Skipping loading updates (DEBUG option). Need to turn this back on again? Look in the Settings tab.");
                }
                else
                {
                    LogLine("Checking for new EDSM systems (may take a few moments).");
                    long updates = edsm.GetNewSystems(this, cancelRequested, reportProgress);
                    LogLine("EDSM updated " + updates + " systems.");
                }
            }

            if (!cancelRequested())
            {

                SystemNoteClass.GetAllSystemNotes();                                // fill up memory with notes, bookmarks, galactic mapping
                BookmarkClass.GetAllBookmarks();
                galacticMapping.ParseData();                            // at this point, EDSM data is loaded..

                LogLine("Loaded Notes, Bookmarks and Galactic mapping.");

                string timestr = SQLiteDBClass.GetSettingString("EDDBSystemsTime", "0");
                DateTime time = new DateTime(Convert.ToInt64(timestr), DateTimeKind.Utc);
                if (DateTime.UtcNow.Subtract(time).TotalDays > 6.5)     // Get EDDB data once every week.
                    performeddbsync = true;

                string lstdist = SQLiteDBClass.GetSettingString("EDSCLastDist", "2010-01-01 00:00:00");
                DateTime timed = DateTime.Parse(lstdist, new CultureInfo("sv-SE"));
                if (DateTime.UtcNow.Subtract(timed).TotalDays > 28)     // Get EDDB data once every month
                    performedsmdistsync = true;
            }
        }

        private void _checkSystemsWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            ReportProgress(-1, "");
            if (e.Error != null)
            {
                System.Windows.Forms.MessageBox.Show("Check Systems exception: " + e.Error.Message);
                System.Windows.Forms.MessageBox.Show("Trace: " + e.Error.StackTrace);
            }
            else if (!e.Cancelled && !PendingClose)
            {
                SystemClass.GetSystemNames(ref SystemNames);            // fill this up, used to speed up if system is present..

                Console.WriteLine("Systems Loaded");

                routeControl1.textBox_From.AutoCompleteCustomSource = SystemNames;
                routeControl1.textBox_To.AutoCompleteCustomSource = SystemNames;
                travelHistoryControl1.textBoxTarget.AutoCompleteCustomSource = SystemNames;
                settings.textBoxHomeSystem.AutoCompleteCustomSource = SystemNames;

                imageHandler1.StartWatcher();
                routeControl1.EnableRouteTab(); // now we have systems, we can update this..

                routeControl1.travelhistorycontrol1 = travelHistoryControl1;
                travelHistoryControl1.netlog.OnNewPosition += new NetLogClass.NetLogEventHandler(travelHistoryControl1.NewPosition);
                travelHistoryControl1.sync.OnNewEDSMTravelLog += new EDSMNewSystemEventHandler(travelHistoryControl1.RefreshEDSMEvent);

                panelInfo.Visible = false;

                CheckForNewInstaller();

                if (performedsmsync || performeddbsync || EDDConfig.UseDistances)
                {
                    AsyncPerformSync();                              // perform any async synchronisations

                    if (performeddbsync || performedsmsync)
                    {
                        string databases = (performedsmsync && performeddbsync) ? "EDSM and EDDB" : ((performedsmsync) ? "EDSM" : "EDDB");

                        /*
                        MessageBox.Show("ED Discovery will now sycnronise to the " + databases + " databases to obtain star information." + Environment.NewLine + Environment.NewLine +
                                        "This will take a while, up to 15 minutes, please be patient." + Environment.NewLine + Environment.NewLine +
                                        "Please continue running ED Discovery until refresh is complete.",
                                        "WARNING - Synchronisation to " + databases);
                         */
                    }
                }
                else
                {
                    long totalsystems = SystemClass.GetTotalSystems();
                    LogLineSuccess("Loading completed, total of " + totalsystems + " systems");

                    //long tickc = Environment.TickCount;
                    LogLine("Reading travel history");
                    travelHistoryControl1.RefreshHistoryAsync();
                    //LogLine("Time " + (Environment.TickCount-tickc) );
                }
            }
        }

        private void _checkSystemsWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                var worker = (System.ComponentModel.BackgroundWorker)sender;

                CheckSystems(() => worker.CancellationPending, (p, s) => worker.ReportProgress(p, s));

                if (worker.CancellationPending)
                    e.Cancel = true;
            }
            finally
            {
                _checkSystemsWorkerCompletedEvent.Set();
            }
        }

        private void _checkSystemsWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            ReportProgress(e.ProgressPercentage, (string)e.UserState);
        }

        private void AsyncPerformSync()
        {
            if (!_syncWorker.IsBusy)
            {
                _syncWorker.RunWorkerAsync();
            }
        }

        private void _syncWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                var worker = (System.ComponentModel.BackgroundWorker)sender;

                PerformFullSync(() => worker.CancellationPending, (p, s) => worker.ReportProgress(p, s));
                if (worker.CancellationPending)
                    e.Cancel = true;
            }
            finally
            {
                _syncWorkerCompletedEvent.Set();
            }
        }

        private void _syncWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            ReportProgress(e.ProgressPercentage, (string)e.UserState);
        }

        private bool PerformEDSMFullSync(EDDiscoveryForm discoveryform, Func<bool> cancelRequested, Action<int, string> reportProgress)
        {
            string rwsystime = SQLiteDBClass.GetSettingString("EDSMLastSystems", "2000-01-01 00:00:00"); // Latest time from RW file.
            DateTime edsmdate = DateTime.Parse(rwsystime, new CultureInfo("sv-SE"));
            long updates = 0;

            try
            {
                EDSMClass edsm = new EDSMClass();

                LogLine("Get hidden systems from EDSM and remove from database");

                SystemClass.RemoveHiddenSystems();

                if (cancelRequested())
                    return false;

                LogLine("Download systems file from EDSM.");

                string edsmsystems = Path.Combine(Tools.GetAppDataDirectory(), "edsmsystems.json");

                LogLine("Resyncing all downloaded EDSM systems with local database." + Environment.NewLine + "This will take a while.");
                bool newfile;
                bool success = EDDBClass.DownloadFile("https://www.edsm.net/dump/systemsWithCoordinates.json", edsmsystems, out newfile, (n, s) =>
                {
                    string rwsysfiletime = "2014-01-01 00:00:00";
                    using (var reader = new StreamReader(s))
                        updates = SystemClass.ParseEDSMUpdateSystemsStream(reader, ref rwsysfiletime, true, discoveryform, cancelRequested, reportProgress);
                    if (!cancelRequested())       // abort, without saving time, to make it do it again
                        SQLiteDBClass.PutSettingString("EDSMLastSystems", rwsysfiletime);

                });

                if (!success)
                {
                    LogLine("Failed to download EDSM system file from server, will check next time");
                    return false;
                }

                LogLine("Now checking for recent EDSM systems.");
                updates += edsm.GetNewSystems(this, cancelRequested, reportProgress);

                LogLine("Local database updated with EDSM data, " + updates + " systems updated.");

                performedsmsync = false;
                GC.Collect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("GetAllEDSMSystems exception:" + ex.Message);
            }

            return (updates > 0);
        }

        private void PerformEDDBFullSync(Func<bool> cancelRequested, Action<int, string> reportProgress)
        {
            try
            {
                EDDBClass eddb = new EDDBClass();

                LogLine("Get systems from EDDB.");

                if (eddb.GetSystems())
                {
                    if (cancelRequested())
                        return;

                    LogLine("Resyncing all downloaded EDDB data with local database." + Environment.NewLine + "This will take a while.");

                    long number = SystemClass.ParseEDDBUpdateSystems(eddb.SystemFileName);

                    LogLine("Local database updated with EDDB data, " + number + " systems updated");
                    SQLiteDBClass.PutSettingString("EDDBSystemsTime", DateTime.UtcNow.Ticks.ToString());
                }
                else
                    LogLineHighlight("Failed to download EDDB Systems. Will try again next run.");

                GC.Collect();
                performeddbsync = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("GetEDDBUpdate exception: " + ex.Message, "ERROR", MessageBoxButtons.OK);
            }
        }

        private bool PerformDistanceFullSync(Func<bool> cancelRequested, Action<int, string> reportProgress)
        {
            long numbertotal = 0;

            try
            {
                string lstdist = SQLiteDBClass.GetSettingString("EDSCLastDist", "2010-01-01 00:00:00");
                EDSMClass edsm = new EDSMClass();

                if (performedsmdistsync)
                {
                    LogLine("Downloading full EDSM distance data.");
                    string filename = edsm.GetEDSMDistances();

                    if (cancelRequested())
                        return false;

                    if (filename != null)
                    {
                        LogLine("Updating all distances with EDSM distance data.");
                        long numberx = DistanceClass.ParseEDSMUpdateDistancesFile(filename, ref lstdist, true);
                        numbertotal += numberx;
                        SQLiteDBClass.PutSettingString("EDSCLastDist", lstdist);
                        LogLine("Local database updated with EDSM Distance data, " + numberx + " distances updated.");
                    }
                }

                if (cancelRequested())
                    return false;

                LogLine("Updating distances with latest EDSM data.");

                string json = edsm.RequestDistances(lstdist);
                if (json == null)
                    LogLine("No response from EDSM Distance server.");
                else
                {
                    long number = DistanceClass.ParseEDSMUpdateDistancesString(json, ref lstdist, false);
                    numbertotal += number;
                }

                LogLine("Local database updated with EDSM Distance data, " + numbertotal + " distances updated.");
                SQLiteDBClass.PutSettingString("EDSCLastDist", lstdist);

                performedsmdistsync = false;
                GC.Collect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("GetEDSMDistances exception: " + ex.Message, "ERROR", MessageBoxButtons.OK);
            }

            return (numbertotal != 0);
        }

        private void PerformFullSync(Func<bool> cancelRequested, Action<int, string> reportProgress)           // big check.. done in a thread.
        {
            reportProgress(-1, "");
            syncwasfirstrun = SystemClass.GetTotalSystems() == 0;                 // remember if DB is empty
            bool edsmoreddbsync = performedsmsync || performeddbsync;           // remember if we are syncing

            if (performedsmsync || performeddbsync)
            {
                if (performedsmsync && !cancelRequested())
                {
                    // Drop indexes on Systems table
                    SQLiteDBClass.DropSystemsTableIndexes();

                    // Delete all old systems
                    SQLiteDBClass.PutSettingString("EDSMLastSystems", "2010-01-01 00:00:00");
                    SQLiteDBClass.PutSettingString("EDDBSystemsTime", "0");
                    using (SQLiteConnectionED cn = new SQLiteConnectionED())
                    {
                        using (DbCommand cmd = cn.CreateCommand("DELETE FROM Systems"))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Download new systems
                    performhistoryrefresh |= PerformEDSMFullSync(this, cancelRequested, reportProgress);

                    LogLine("Indexing systems table");
                    SQLiteDBClass.CreateSystemsTableIndexes();
                }

                if (!cancelRequested())
                {
                    PerformEDDBFullSync(cancelRequested, reportProgress);
                }
            }

            if (EDDConfig.UseDistances && !cancelRequested())
            {
                performhistoryrefresh |= PerformDistanceFullSync(cancelRequested, reportProgress);
            }
            else
                performedsmdistsync = false;

            syncwaseddboredsm = edsmoreddbsync;
            reportProgress(-1, "");
        }

        private void _syncWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            long totalsystems = SystemClass.GetTotalSystems();
            LogLineSuccess("Loading completed, total of " + totalsystems + " systems");

            travelHistoryControl1.HistoryRefreshed += TravelHistoryControl1_HistoryRefreshed;
            travelHistoryControl1.RefreshHistoryAsync();
        }

        private void TravelHistoryControl1_HistoryRefreshed(object sender, EventArgs e)
        {
            ReportProgress(-1, "");
            travelHistoryControl1.HistoryRefreshed -= TravelHistoryControl1_HistoryRefreshed;
            LogLine("Refreshing complete.");
            /*
            if (syncwasfirstrun)
            {
                MessageBox.Show("ESDM and EDDB update complete. Please restart ED Discovery to complete the synchronisation " + Environment.NewLine,
                                "Restart ED Discovery");
            }
            else if (syncwaseddboredsm)
                MessageBox.Show("ESDM and/or EDDB update complete.", "Completed update");
             */
        }

        internal void AsyncRefreshHistory()
        {
        }
        
#endregion

#region Logging

        public void LogLine(string text)
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    travelHistoryControl1.LogText(text + Environment.NewLine);
                });
            }
            catch
            {
            }
        }

        public void LogLineHighlight(string text)
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    travelHistoryControl1.LogTextHighlight(text + Environment.NewLine);

                });
            }
            catch
            {
            }
        }

        public void LogLineSuccess(string text)
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    travelHistoryControl1.LogTextSuccess(text + Environment.NewLine);

                });
            }
            catch
            {
            }
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

            SQLiteDBClass.PutSettingBool("EDSMSyncTo", travelHistoryControl1.checkBoxEDSMSyncTo.Checked);
            SQLiteDBClass.PutSettingBool("EDSMSyncFrom", travelHistoryControl1.checkBoxEDSMSyncFrom.Checked);
        }

        Thread safeClose;
        System.Windows.Forms.Timer closeTimer;

        public bool PendingClose { get { return safeClose != null; }  }           // we want to close boys!

        private void EDDiscoveryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (safeClose == null)                  // so a close is a request now, and it launches a thread which cleans up the system..
            {
                e.Cancel = true;
                CancellationTokenSource.Cancel();
                travelHistoryControl1.CancelHistoryRefresh();
                _syncWorker.CancelAsync();
                _checkSystemsWorker.CancelAsync();
                if (cancelDownloadMaps != null)
                {
                    cancelDownloadMaps();
                }
                labelPanelText.Text = "Closing, please wait!";
                panelInfo.Visible = true;
                LogLineHighlight("Closing down, please wait..");
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
            travelHistoryControl1.netlog.StopMonitor();

            if (travelHistoryControl1.sync != null)
                travelHistoryControl1.sync.StopSync();

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

        void CloseItFinally(Object sender, EventArgs e )        
        {
            if (safeClose.IsAlive)      // still alive, try again
                closeTimer.Start();
            else
            {
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
                if (EliteDangerous.EDDirectory != null && !EliteDangerous.EDDirectory.Equals(""))
                    Process.Start(EliteDangerous.EDDirectory);

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
                Process.Start(travelHistoryControl1.netlog.GetNetLogPath());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Show log files exception: " + ex.Message);
            }
        }

        private void statisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StatsForm frm = new StatsForm();

            frm.travelhistoryctrl = travelHistoryControl1;
            frm.Show();

        }

        private void show2DMapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSagCarinaMission frm = new FormSagCarinaMission(this);
            frm.Nowindowreposition = option_nowindowreposition;
            frm.Show();
        }

        private void show3DMapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TravelControl.buttonMap_Click(sender, e);
        }

        private void prospectingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlanetsForm frm = new PlanetsForm();

            frm.InitForm(this);
            frm.Show();
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

        private void synchroniseWithEDSMDistancesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EDDConfig.UseDistances)
                MessageBox.Show("EDSM Distances are turned off, please turn on first in settings");
            else if (!_syncWorker.IsBusy)      // we want it to have run, to completion, to allow another go..
            {
                performedsmdistsync = true;
                AsyncPerformSync();
            }
            else
                MessageBox.Show("Synchronisation to databases is in operation or pending, please wait");
        }

        public bool RequestDistanceSync()
        {
            if (performedsmdistsync == false)
            {
                performedsmdistsync = true;
                AsyncPerformSync();
                return true;
            }
            else
                return false;
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

        /*
        private void panel_grip_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                panel_grip.Captured();           // tell it, doing this royally screws up the MD/MU/ME/ML calls to it
                panel_grip.Capture = false;
                SendMessage(WM_NCL_RESIZE, (IntPtr)HT_RESIZE, IntPtr.Zero);
            }
        }
         */

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

        private void dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm you wish to reset all travelled history entries to the first commander", "WARNING", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                List<VisitedSystemsClass> vsall = VisitedSystemsClass.GetAll();

                foreach (VisitedSystemsClass vs in vsall)
                {
                    if (vs.Commander != 0)
                    {
                        vs.Commander = 0;
                        vs.Update();
                    }
                }
            }
        }

        private void debugBetaFixHiddenLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm you wish to reset all hidden entries after 5th may 2016 to the first commander", "WARNING", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                List<VisitedSystemsClass> vsall = VisitedSystemsClass.GetAll();

                foreach (VisitedSystemsClass vs in vsall)
                {
                    if (vs.Commander == -2 && vs.Time > new DateTime(2016, 5, 5))
                    {
                        vs.Commander = 0;
                        vs.Update();
                    }
                }
            }
        }
#endregion
    }
}
