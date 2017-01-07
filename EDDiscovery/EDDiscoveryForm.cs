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
using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;
using EDDiscovery.EDDN;

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
        public const int HT_LEFT = 0xA;
        public const int HT_RIGHT = 0xB;
        public const int HT_BOTTOM = 0xF;
        public const int HT_BOTTOMRIGHT = 0x11;
        public const int WM_NCL_RESIZE = 0x112;
        public const int HT_RESIZE = 61448;
        public const int WM_NCHITTEST = 0x84;

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
        public ExportControl ExportControl { get { return exportControl1; } }
        public EDDiscovery2.ImageHandler.ImageHandler ImageHandler { get { return imageHandler1; } }

        public bool option_nowindowreposition { get; set; } = false;                             // Cmd line options
        public bool option_debugoptions { get; set; } = false;
        public bool option_tracelog { get; set; } = false;
        public bool option_fcexcept { get; set; } = false;

        public EDDiscovery2._3DMap.MapManager Map { get; private set; }

        public event EventHandler HistoryRefreshed; // this is an internal hook


        public delegate void HistoryChange(HistoryList l);          // subscribe to get events
        public event HistoryChange OnHistoryChange;
        public delegate void NewEntry(HistoryEntry l, HistoryList hl);
        public event NewEntry OnNewEntry;
        public delegate void NewLogEntry(string txt, Color c);
        public event NewLogEntry OnNewLogEntry;
        public delegate void NewTarget();
        public event NewTarget OnNewTarget;
        
        public GalacticMapping galacticMapping;

        public CancellationTokenSource CancellationTokenSource { get; private set; } = new CancellationTokenSource();

        private ManualResetEvent _syncWorkerCompletedEvent = new ManualResetEvent(false);
        private ManualResetEvent _checkSystemsWorkerCompletedEvent = new ManualResetEvent(false);

        public EDSMSync EdsmSync;

        Action cancelDownloadMaps = null;
        Task<bool> downloadMapsTask = null;
        Task checkInstallerTask = null;
        private string logname = "";
        private bool themeok = true;
        private Forms.SplashForm splashform = null;
        BackgroundWorker dbinitworker = null;

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

        public EDDiscoveryForm()
        {
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

            SQLiteConnectionUser.EarlyReadRegister();
            EDDConfig.Instance.Update(write: false);

            dbinitworker = new BackgroundWorker();
            dbinitworker.DoWork += Dbinitworker_DoWork;
            dbinitworker.RunWorkerCompleted += Dbinitworker_RunWorkerCompleted;
            dbinitworker.RunWorkerAsync();

            theme = new EDDTheme();

            EDDConfig = EDDConfig.Instance;
            galacticMapping = new GalacticMapping();

            ToolStripManager.Renderer = theme.toolstripRenderer;
            theme.LoadThemes();                                         // default themes and ones on disk loaded
            themeok = theme.RestoreSettings();                                    // theme, remember your saved settings

            trilaterationControl.InitControl(this);
            travelHistoryControl1.InitControl(this);
            imageHandler1.InitControl(this);
            settings.InitControl(this);
            journalViewControl1.InitControl(this,0);
            routeControl1.InitControl(this);
            savedRouteExpeditionControl1.InitControl(this);
            exportControl1.InitControl(this);


            EdsmSync = new EDSMSync(this);

            Map = new EDDiscovery2._3DMap.MapManager(option_nowindowreposition, this);

            journalmonitor = new EliteDangerous.EDJournalClass();

            this.TopMost = EDDConfig.KeepOnTop;

            ApplyTheme();

            DisplayedCommander = EDDiscoveryForm.EDDConfig.CurrentCommander.Nr;
        }

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
            if (splashform != null)
            {
                splashform.Close();
            }
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
            option_tracelog = parts.FindIndex(x => x.Equals("-TraceLog", StringComparison.InvariantCultureIgnoreCase)) != -1;
            option_fcexcept = parts.FindIndex(x => x.Equals("-LogExceptions", StringComparison.InvariantCultureIgnoreCase)) != -1;

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

        private void EDDiscoveryForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (!(SQLiteConnectionUser.IsInitialized && SQLiteConnectionSystem.IsInitialized))
                {
                    splashform = new SplashForm();
                    splashform.ShowDialog(this);
                }

                EliteDangerousClass.CheckED();
                EDDConfig.Update();
                RepositionForm();
                InitFormControls();
                settings.InitSettingsTab();
                savedRouteExpeditionControl1.LoadControl();
                travelHistoryControl1.LoadControl();

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

            if (!themeok)
            {
                LogLineHighlight("The theme stored has missing colors or other missing information");
                LogLineHighlight("Correct the missing colors or other information manually using the Theme Editor in Settings");
            }
        }

        private Task CheckForNewInstallerAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                CheckForNewinstaller();
            });
        }

        private bool CheckForNewinstaller()
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
                            this.BeginInvoke(new Action(() => LogLineHighlight("New EDDiscovery installer available: " + rel.ReleaseName)));
                            this.BeginInvoke(new Action(() => PanelInfoNewRelease()));
                        return true;
                        }
                    }
                }
                catch (Exception )
                {

                }

            return false;
        }

        private void PanelInfoNewRelease()
        {
            ShowInfoPanel("Download new release!", true, Color.Green);
        }


        private void InitFormControls()
        {
            ShowInfoPanel("Loading. Please wait!", true, Color.Gold);
            
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
            else
            {
                var Max = SQLiteDBClass.GetSettingBool("FormMax", false);
                if (Max) this.WindowState = FormWindowState.Maximized;
            }

            travelHistoryControl1.LoadLayoutSettings();
            journalViewControl1.LoadLayoutSettings();
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

        private void EDDiscoveryForm_Activated(object sender, EventArgs e)
        {
        }

        public void ApplyTheme()
        {
            ToolStripManager.Renderer = theme.toolstripRenderer;
            panel_close.Visible = !theme.WindowsFrame;
            panel_minimize.Visible = !theme.WindowsFrame;
            label_version.Visible = !theme.WindowsFrame;

            this.Text = "EDDiscovery " + label_version.Text;            // note in no border mode, this is not visible on the title bar but it is in the taskbar..

            theme.ApplyToForm(this);

            if (OnHistoryChange!=null)
                OnHistoryChange(history);
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
            }
            catch (Exception ex)
            {
                LogLineHighlight("DownloadImages exception: " + ex.Message);
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

                imageHandler1.StartWatcher();
                routeControl1.EnableRouteTab(); // now we have systems, we can update this..

                routeControl1.travelhistorycontrol1 = travelHistoryControl1;
                journalmonitor.OnNewJournalEntry += NewPosition;
                EdsmSync.OnDownloadedSystems += RefreshDueToEDSMDownloadedSystems;


                LogLine("Reading travel history");
                HistoryRefreshed += _travelHistoryControl1_InitialRefreshDone;

                RefreshHistoryAsync();

                ShowInfoPanel("", false);

                checkInstallerTask = CheckForNewInstallerAsync();

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

                if (CanSkipSlowUpdates())
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

                if (performhistoryrefresh)
                {
                    LogLine("Refresh due to updating systems");
                    HistoryRefreshed += HistoryFinishedRefreshing;
                    RefreshHistoryAsync();
                }

                edsmRefreshTimer.Enabled = true;
            }
        }

        private void HistoryFinishedRefreshing(object sender, EventArgs e)
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
                        updates = SystemClass.ParseEDSMUpdateSystemsStream(reader, ref rwsysfiletime, ref outoforder, true, discoveryform, cancelRequested, reportProgress, useCache: false, useTempSystems: true);
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

        private void edsmRefreshTimer_Tick(object sender, EventArgs e)
        {
            AsyncPerformSync();
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

        #region Logging

        private string logtext = "";     // to keep in case of no logs..

        public string LogText { get { return logtext; } }

        public void LogLine(string text)
        {
            LogLineColor(text, theme.TextBlockColor);
        }

        public void LogLineHighlight(string text)
        {
            LogLineColor(text, theme.TextBlockHighlightColor);
        }

        public void LogLineSuccess(string text)
        {
            LogLineColor(text, theme.TextBlockSuccessColor);
        }

        public void LogLineColor(string text, Color color)
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    logtext += text + Environment.NewLine;      // keep this, may be the only log showing

                    if (OnNewLogEntry != null)
                        OnNewLogEntry(text + Environment.NewLine, color);
                });
            }
            catch { }
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

            SQLiteDBClass.PutSettingBool("FormMax", this.WindowState == FormWindowState.Maximized);
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

        public void ShowInfoPanel(string message, bool visible, Color? backColour = null)
        {
            labelPanelText.Text = message;
            panelInfo.Visible = visible;
            if (backColour.HasValue) panelInfo.BackColor = backColour.Value;
        }

        private void EDDiscoveryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (safeClose == null)                  // so a close is a request now, and it launches a thread which cleans up the system..
            {
                e.Cancel = true;
                edsmRefreshTimer.Enabled = false;
                CancellationTokenSource.Cancel();
                CancelHistoryRefresh();
                EDDNSync.StopSync();
                _syncWorker.CancelAsync();
                _checkSystemsWorker.CancelAsync();
                if (cancelDownloadMaps != null)
                {
                    cancelDownloadMaps();
                }
                ShowInfoPanel("Closing, please wait!", true);
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

#region Buttons, Mouse, Menus

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
            frm.TopMost = EDDiscoveryForm.EDDConfig.KeepOnTop;
            frm.ShowDialog(this);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox();
        }

        private void eDDiscoveryChatDiscordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/0qIqfCQbziTWzsQu");
        }

        private void showAllInTaskBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            travelHistoryControl1.ShowAllPopOutsInTaskBar();
        }

        private void turnOffAllTransparencyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            travelHistoryControl1.MakeAllPopoutsOpaque();
        }

        private void clearEDSMIDAssignedToAllRecordsForCurrentCommanderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm you wish to reset the assigned EDSM IDs to all the current commander history entries," + 
                                " and clear all the assigned EDSM IDs in all your notes for all commanders\r\n\r\n" +
                                "This will not change your history, but when you next refresh, it will try and reassign EDSM systems to " +
                                "your history and notes.  Use only if you think that the assignment of EDSM systems to entries is grossly wrong," +
                                "or notes are going missing\r\n" + 
                                "\r\n" +
                                "You can manually change one EDSM assigned system by right clicking on the travel history and selecting the option"
                                , "WARNING", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                EliteDangerous.JournalEntry.ClearEDSMID(EDDConfig.CurrentCommander.Nr);
                SystemNoteClass.ClearEDSMID();
            }

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
            Read21Folders(false);
        }

        private void read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Read21Folders(true);
        }

        private void Read21Folders(bool force)
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
                            EDDConfig.UpdateCommanders(new List<EDCommander> { cmdr }, true);
                        }

                        //string logpath = "c:\\games\\edlaunch\\products\\elite-dangerous-64\\logs";
                        RefreshHistoryAsync(netlogpath: logpath, forcenetlogreload: force, currentcmdr: cmdr.Nr);
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

        private void checkForNewReleaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckForNewinstaller())
            {
                if (newRelease != null)
                {
                    NewReleaseForm frm = new NewReleaseForm();
                    frm.release = newRelease;

                    frm.ShowDialog(this);
                }
            }
            else
            {
                MessageBox.Show("No new release found", "EDDiscovery", MessageBoxButtons.OK);
            }
        }

        private void deleteDuplicateFSDJumpEntriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm you remove any duplicate FSD entries from the current commander", "WARNING", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                int n = EliteDangerous.JournalEntry.RemoveDuplicateFSDEntries(EDDConfig.CurrentCommander.Nr);
                LogLine("Removed " + n + " FSD entries");
                RefreshHistoryAsync();
            }
        }

        private void panelInfo_Click(object sender, EventArgs e)
        {
            if (newRelease != null)
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

        public void Open3DMap(HistoryEntry he)
        {
            this.Cursor = Cursors.WaitCursor;

            string HomeSystem = settings.MapHomeSystem;

            history.FillInPositionsFSDJumps();

            Map.Prepare(he?.System, HomeSystem,
                        settings.MapCentreOnSelection ? he?.System : SystemClass.GetSystem(String.IsNullOrEmpty(HomeSystem) ? "Sol" : HomeSystem),
                        settings.MapZoom, history.FilterByTravel);
            Map.Show();
            this.Cursor = Cursors.Default;
        }

        private void sendUnsuncedEDDNEventsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<HistoryEntry> hlsyncunsyncedlist = history.FilterByScanNotEDDNSynced;        // first entry is oldest
            EDDNSync.SendEDDNEvents(this, hlsyncunsyncedlist);
        }

        private void materialSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindMaterialsForm frm = new FindMaterialsForm();

            frm.Show(this);
        }

        #endregion

        #region Window Control

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
                //System.Diagnostics.Debug.WriteLine( Environment.TickCount + " Res " + ((int)m.Result));

                if ((int)m.Result == HT_CLIENT)
                {
                    int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                    int y = unchecked((short)((uint)m.LParam >> 16));
                    Point p = PointToClient(new Point(x, y));

                    if (p.X > this.ClientSize.Width - statusStrip1.Height && p.Y > this.ClientSize.Height - statusStrip1.Height)
                    {
                        m.Result = (IntPtr)HT_BOTTOMRIGHT;
                    }
                    else if (p.Y > this.ClientSize.Height - statusStrip1.Height)
                    {
                        m.Result = (IntPtr)HT_BOTTOM;
                    }
                    else if (p.X > this.ClientSize.Width - 5)       // 5 is generous.. really only a few pixels gets thru before the subwindows grabs them
                    {
                        m.Result = (IntPtr)HT_RIGHT;
                    }
                    else if (p.X < 5)
                    {
                        m.Result = (IntPtr)HT_LEFT;
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

        protected class RefreshWorkerResults
        {
            public List<HistoryEntry> rethistory;
            public MaterialCommoditiesLedger retledger;
            public StarScan retstarscan;
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

            List<HistoryEntry> hl = new List<HistoryEntry>();
            EDCommander cmdr = null;

            if (args.CurrentCommander >= 0)
            {
                cmdr = EDDConfig.Commander(args.CurrentCommander);
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

            if (jlistUpdated.Count > 0)
            {
                worker.ReportProgress(-1, "Updating journal entries");

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

            if (worker.CancellationPending)
            {
                e.Cancel = true;
            }
            else
            {
                e.Result = new RefreshWorkerResults { rethistory = hl, retledger = matcommodledger, retstarscan = starscan };
            }
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
                    travelHistoryControl1.LoadCommandersListBox();             // in case a new commander has been detected
                    exportControl1.PopulateCommanders();
                    settings.UpdateCommandersListBox();

                    history.Clear();

                    foreach (var ent in ((RefreshWorkerResults)e.Result).rethistory)
                    {
                        history.Add(ent);
                        Debug.Assert(ent.MaterialCommodity != null);
                    }

                    history.materialcommodititiesledger = ((RefreshWorkerResults)e.Result).retledger;
                    history.starscan = ((RefreshWorkerResults)e.Result).retstarscan;

                    ReportProgress(-1, "");
                    LogLine("Refresh Complete." );

                    if (OnHistoryChange != null)
                        OnHistoryChange(history);
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

        // go thru the hisotry list and reworkout the materials ledge and the materials count, plus any other stuff..
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

        public void RefreshDisplays()
        {
            if (OnHistoryChange != null)
                OnHistoryChange(history);
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

                if (OnNewEntry != null)
                    OnNewEntry(he,history);
            }

            travelHistoryControl1.LoadCommandersListBox();  // because we may have new commanders
            settings.UpdateCommandersListBox();
            exportControl1.PopulateCommanders();
        }

        public void RecalculateHistoryDBs()         // call when you need to recalc the history dbs - not the whole history. Use RefreshAsync for that
        {
            MaterialCommoditiesLedger matcommodledger = new MaterialCommoditiesLedger();
            StarScan starscan = new StarScan();

            ProcessUserHistoryListEntries(history.EntryOrder, matcommodledger, starscan);

            history.materialcommodititiesledger = matcommodledger; ;
            history.starscan = starscan;

            if (OnHistoryChange != null)
                OnHistoryChange(history);
        }


        #endregion

        #region Targets

        public void NewTargetSet()
        {
            System.Diagnostics.Debug.WriteLine("New target set");
            if (OnNewTarget != null)
                OnNewTarget();
        }

        #endregion

    }
}

