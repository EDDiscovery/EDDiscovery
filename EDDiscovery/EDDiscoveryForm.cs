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

namespace EDDiscovery
{

    public delegate void DistancesLoaded();

    public partial class EDDiscoveryForm : Form
    {
        #region Variables

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public const int WM_NCL_RESIZE = 0x112;
        public const int HT_RESIZE = 61448;

        private IntPtr SendMessage(int msg, IntPtr wparam, IntPtr lparam)
        {
            Message message = Message.Create(this.Handle, msg, wparam, lparam);
            this.WndProc(ref message);
            return message.Result;
        }

        private EDSMSync _edsmSync;
        public EDDTheme theme;

        public AutoCompleteStringCollection SystemNames;       

        public string CommanderName { get; private set; }
        static public EDDConfig EDDConfig { get; private set; }

        public TravelHistoryControl TravelControl { get { return travelHistoryControl1; } }
        public List<VisitedSystemsClass> VisitedSystems { get { return travelHistoryControl1.visitedSystems; } }

        public bool option_nowindowreposition { get; set;  }  = false;                             // Cmd line options

        public EDDiscovery2._3DMap.MapManager Map { get; private set; }

        public GalacticMapping galacticMapping;

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

            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Unable to create the folder '{logpath}'");
                Trace.WriteLine($"Exception: {ex.Message}");
            }
            _edsmSync = new EDSMSync(this);

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

        private void EDDiscoveryForm_Layout(object sender, LayoutEventArgs e)       // Manually position, could not get gripper under tab control with it sizing for the life of me
        {
            if (panel_grip.Visible)
            {
                panel_grip.Location = new Point(this.ClientSize.Width - panel_grip.Size.Width, this.ClientSize.Height - panel_grip.Size.Height);
                tabControl1.Size = new Size(this.ClientSize.Width - panel_grip.Size.Width, this.ClientSize.Height - panel_grip.Size.Height - tabControl1.Location.Y);
            }
            else
                tabControl1.Size = new Size(this.ClientSize.Width, this.ClientSize.Height - tabControl1.Location.Y);
        }

        private void ProcessCommandLineOptions()
        {
            string cmdline = Environment.CommandLine;
            option_nowindowreposition = (cmdline.IndexOf("-NoRepositionWindow", 0, StringComparison.InvariantCultureIgnoreCase) != -1 || cmdline.IndexOf("-NRW", 0, StringComparison.InvariantCultureIgnoreCase) != -1 );

            int pos = cmdline.IndexOf("-Appfolder", 0, StringComparison.InvariantCultureIgnoreCase);
            if ( pos != -1 )
            {
                string[] nextwords = cmdline.Substring(pos + 10).Trim().Split(' ');
                if (nextwords.Length > 0)
                    Tools.appfolder = nextwords[0];
            }

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
                CheckIfVerboseLoggingIsTurnedOn();

                if (File.Exists("test.txt"))
                {
                    button_test.Visible = true;
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
            try
            {
                CommanderName = EDDConfig.CurrentCommander.Name;

                var edsmThread = new Thread(CheckSystems) { Name = "Check Systems", IsBackground = true };
                var downloadmapsThread = new Thread(DownloadMaps) { Name = "Downloading map Files", IsBackground = true };
                edsmThread.Start();
                downloadmapsThread.Start();

                while (edsmThread.IsAlive || downloadmapsThread.IsAlive)
                {
                    Thread.Sleep(50);
                    Application.DoEvents();
                }

                edsmThread.Join();
                downloadmapsThread.Join();

                SystemClass.GetSystemNames(ref SystemNames);            // fill this up, used to speed up if system is present..
                Console.WriteLine("Systems Loaded");

                routeControl1.textBox_From.AutoCompleteCustomSource = SystemNames;
                routeControl1.textBox_To.AutoCompleteCustomSource = SystemNames;
                settings.textBoxHomeSystem.AutoCompleteCustomSource = SystemNames;

                imageHandler1.StartWatcher();
                routeControl1.EnableRouteTab(); // now we have systems, we can update this..

                routeControl1.travelhistorycontrol1 = travelHistoryControl1;
                travelHistoryControl1.netlog.OnNewPosition += new NetLogEventHandler(routeControl1.NewPosition);
                travelHistoryControl1.netlog.OnNewPosition += new NetLogEventHandler(travelHistoryControl1.NewPosition);
                travelHistoryControl1.sync.OnNewEDSMTravelLog += new EDSMNewSystemEventHandler(travelHistoryControl1.RefreshEDSMEvent);

                //long tickc = Environment.TickCount;
                LogLine("Reading travel history");
                travelHistoryControl1.RefreshHistory();
                //LogLine("Time " + (Environment.TickCount-tickc) );

                travelHistoryControl1.netlog.StartMonitor(this);

                if (EliteDangerous.CheckStationLogging())
                {
                    panelInfo.Visible = false;
                }

                CheckForNewInstaller();

                long totalsystems = SystemClass.GetTotalSystems();
                LogLineSuccess("Loading completed, total of " + totalsystems + " systems");

                AsyncPerformSync();                              // perform any async synchronisations

                if ( performeddbsync || performedsmsync )
                {
                    string databases = (performedsmsync && performeddbsync) ? "EDSM and EDDB" : ((performedsmsync) ? "EDSM" : "EDDB");

                    MessageBox.Show("ED Discovery will now sycnronise to the " + databases + " databases to obtain star information." + Environment.NewLine + Environment.NewLine +
                                    "This will take a while, up to 15 minutes, please be patient." + Environment.NewLine + Environment.NewLine +
                                    "Please continue running ED Discovery until refresh is complete.",
                                    "WARNING - Synchronisation to " + databases);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("EDDiscovery_Load exception: " + ex.Message);
                System.Windows.Forms.MessageBox.Show("Trace: " + ex.StackTrace);
            }
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
                var screen = Screen.FromControl(this).Bounds;
                if( height > screen.Height ) height = screen.Height;
                if( top + height > screen.Height) top = screen.Height - height;
                if( width > screen.Width ) width = screen.Width;
                if( left + width > screen.Width ) left = screen.Width - width;

                this.Top = top;
                this.Left = left;
                this.Height = height;
                this.Width = width;
            }
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

        private void CheckIfVerboseLoggingIsTurnedOn()
        {
            if (!EliteDangerous.CheckStationLogging())
            {
                LogLineHighlight("Elite Dangerous is not logging system names!!! ");
                LogLine("Add VerboseLogging =\"1\" to <Network  section in File: " + Path.Combine(EliteDangerous.EDDirectory, "AppConfig.xml") + " or AppConfigLocal.xml  Remember to restart Elite!");
                labelPanelText.Text = "Elite Dangerous is not logging system names!";
                panelInfo.BackColor = Color.Salmon;
            }
        }

        private void EDDiscoveryForm_Activated(object sender, EventArgs e)
        {
            /* TODO: Add setting to determine -which- field should be focussed */
            /* DISABLED FOR NOW
            if (tabControl1.SelectedTab == tabPageTravelHistory)
            {
                travelHistoryControl1.textBoxDistanceToNextSystem.Focus();
            }
            */
        }

        public void ApplyTheme(bool refreshhistory)
        {
            this.FormBorderStyle = theme.WindowsFrame ? FormBorderStyle.Sizable : FormBorderStyle.None;
            panel_grip.Visible = !theme.WindowsFrame;
            panel_close.Visible = !theme.WindowsFrame;
            panel_minimize.Visible = !theme.WindowsFrame;
            label_version.Visible = !theme.WindowsFrame;
            label_version.Text = "Version " + Assembly.GetExecutingAssembly().FullName.Split(',')[1].Split('=')[1];
            if (Tools.appfolder != "EDDiscovery")
                label_version.Text += " (Using " + Tools.appfolder +")";

            this.Text = "EDDiscovery " + label_version.Text;            // note in no border mode, this is not visible on the title bar but it is in the taskbar..

            theme.ApplyColors(this);

            if (refreshhistory)
                travelHistoryControl1.RefreshHistory();             // so we repaint this with correct colours.
        }

#endregion

#region Information Downloads

        public void DownloadMaps()          // ASYNC process
        {
            if (CanSkipSlowUpdates())
            {
                LogLine("Skipping checking for new maps (DEBUG option).");
                return;
            }

            try
            {
                if (!Directory.Exists(Path.Combine(Tools.GetAppDataDirectory(), "Maps")))
                    Directory.CreateDirectory(Path.Combine(Tools.GetAppDataDirectory(), "Maps"));

                LogLine("Checking for new EDDiscovery maps");

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
                    
                    DeleteMapFile("DW4.png");
                    DeleteMapFile("SC-00.jpg");
                }

                LogLine("Map check complete.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("DownloadImages exception: " + ex.Message, "ERROR", MessageBoxButtons.OK);
            }
        }

        private bool DownloadMapFile(string file)
        {
            EDDBClass eddb = new EDDBClass();
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

        private void CheckSystems()  // ASYNC process, done via start up, must not be too slow.
        {
            try
            {
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
                        long updates = edsm.GetNewSystems();
                        LogLine("EDSM updated " + updates + " systems.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("GetEDSMSystems exception: " + ex.Message, "ERROR", MessageBoxButtons.OK);
            }

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

            GC.Collect();
        }

        private void AsyncPerformSync()
        {
            var EDSMThread = new Thread(PerformSync) { Name = "Downloading EDSM system", IsBackground = true };
            EDSMThread.Start();
        }

        private void PerformSync()           // big check.. done in a thread.
        {
            bool refreshhistory = false;
            bool firstrun = SystemClass.GetTotalSystems() == 0;                 // remember if DB is empty
            bool edsmoreddbsync = performedsmsync || performeddbsync;           // remember if we are syncing

            if (performedsmsync)
            {
                string rwsystime = SQLiteDBClass.GetSettingString("EDSMLastSystems", "2000-01-01 00:00:00"); // Latest time from RW file.
                DateTime edsmdate = DateTime.Parse(rwsystime, new CultureInfo("sv-SE"));

                try
                {
                    EDSMClass edsm = new EDSMClass();

                    LogLine("Get hidden systems and remove from EDSM.");

                    string strhiddensystems = edsm.GetHiddenSystems();

                    if (strhiddensystems != null && strhiddensystems.Length >= 6)
                        SystemClass.RemoveHiddenSystems(strhiddensystems);

                    LogLine("Get systems from EDSM.");

                    bool newfile = false;
                    string edsmsystems = Path.Combine(Tools.GetAppDataDirectory(), "edsmsystems.json");
                    EDDBClass.DownloadFile("https://www.edsm.net/dump/systemsWithCoordinates.json", edsmsystems, out newfile);

                    long updates = 0;

                    if (newfile)
                    {
                        LogLine("Resyncing all downloaded EDSM systems with local database." + Environment.NewLine + "This will take a while.");

                        string rwsysfiletime = "2014-01-01 00:00:00";
                        updates = SystemClass.ParseEDSMUpdateSystemsFile(edsmsystems, ref rwsysfiletime, true);

                        SQLiteDBClass.PutSettingString("EDSMLastSystems", rwsysfiletime);
                    }

                    LogLine("Now checking for recent EDSM systems.");
                    updates += edsm.GetNewSystems();

                    LogLine("Local database updated with EDSM data, " + updates + " systems updated.");

                    if (updates > 0)
                        refreshhistory = true;

                    performedsmsync = false;
                    GC.Collect();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("GetAllEDSMSystems exception:" + ex.Message);
                }
            }

            if ( performeddbsync )
            {
                try
                {
                    EDDBClass eddb = new EDDBClass();

                    LogLine("Get systems from EDDB.");

                    if (eddb.GetSystems())
                    {
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

            if (EDDConfig.UseDistances)
            {
                try
                {
                    long numbertotal = 0;
                    string lstdist = SQLiteDBClass.GetSettingString("EDSCLastDist", "2010-01-01 00:00:00");
                    EDSMClass edsm = new EDSMClass();

                    if (performedsmdistsync)
                    {
                        LogLine("Downloading full EDSM distance data.");
                        string filename = edsm.GetEDSMDistances();

                        if (filename != null)
                        {
                            LogLine("Updating all distances with EDSM distance data.");
                            long numberx = DistanceClass.ParseEDSMUpdateDistancesFile(filename, ref lstdist, true);
                            numbertotal += numberx;
                            SQLiteDBClass.PutSettingString("EDSCLastDist", lstdist);
                            LogLine("Local database updated with EDSM Distance data, " + numberx + " distances updated.");
                        }
                    }

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

                    if (numbertotal > 0)                          // if we've done something
                        refreshhistory = true;

                    performedsmdistsync = false;
                    GC.Collect();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("GetEDSMDistances exception: " + ex.Message, "ERROR", MessageBoxButtons.OK);
                }
            }
            else
                performedsmdistsync = false;

            if ( refreshhistory )
            {
                Invoke((MethodInvoker)delegate
                {
                    LogLine("Refreshing history due changes in distances or star data.");
                    travelHistoryControl1.RefreshHistory();
                    LogLine("Refreshing complete.");

                    if (firstrun)
                    {
                        MessageBox.Show("ESDM and EDDB update complete. Please restart ED Discovery to complete the synchronisation " + Environment.NewLine,
                                        "Restart ED Discovery");
                    }
                    else if (edsmoreddbsync)
                        MessageBox.Show("ESDM and/or EDDB update complete.", "Completed update");

                });
            }
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

            SQLiteDBClass.PutSettingBool("EDSMSyncTo", travelHistoryControl1.checkBoxEDSMSyncTo.Checked);
            SQLiteDBClass.PutSettingBool("EDSMSyncFrom", travelHistoryControl1.checkBoxEDSMSyncFrom.Checked);
        }

        private void EDDiscoveryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            travelHistoryControl1.netlog.StopMonitor();
            _edsmSync.StopSync();
            SaveSettings();
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
            Process.Start("http://eddiscovery.astronet.se/");
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

        private void prospectingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlanetsForm frm = new PlanetsForm();

            frm.InitForm(this);
            frm.Show();
        }

        private void forceEDDBUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (performeddbsync == false)           // meaning its not running
            {
                performeddbsync = true;
                AsyncPerformSync();
            }
            else
                MessageBox.Show("EDDB Sync is in operation, please wait");
        }

        private void syncEDSMSystemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (performedsmsync == false)      // meaning we are not running this..
            {
                performedsmsync = true;
                AsyncPerformSync();
            }
            else
                MessageBox.Show("EDSM Sync is in operation, please wait");
        }

        private void synchroniseWithEDSMDistancesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EDDConfig.UseDistances)
                MessageBox.Show("EDSM Distances are turned off, please turn on first in settings");
            else if (performedsmdistsync == false)
            {
                performedsmdistsync = true;
                AsyncPerformSync();
            }
            else
                MessageBox.Show("EDSM Distances Sync is in operation, please wait");
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

        private void panel_grip_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                panel_grip.Captured();           // tell it, doing this royally screws up the MD/MU/ME/ML calls to it
                panel_grip.Capture = false;
                SendMessage(WM_NCL_RESIZE, (IntPtr)HT_RESIZE, IntPtr.Zero);
            }
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
            Application.Exit();
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
            Application.Exit();
        }

        private void dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void debugBetaFixHiddenLogToolStripMenuItem_Click(object sender, EventArgs e)
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

        #endregion

    }
}
