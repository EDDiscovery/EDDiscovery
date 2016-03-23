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

namespace EDDiscovery
{

    public delegate void DistancesLoaded();

    public partial class EDDiscoveryForm : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public const int WM_NCL_RESIZE = 0x112;
        public const int HT_RESIZE = 61448;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        //readonly string _fileTgcSystems;
        readonly string _fileEDSMDistances;
        private EDSMSync _edsmSync;
        private SQLiteDBClass _db = new SQLiteDBClass();
        public EDDTheme theme = new EDDTheme();

        public AutoCompleteStringCollection SystemNames { get; private set; }
        public string CommanderName { get; private set; }
        static public EDDConfig EDDConfig { get; private set; }

        public EDDiscovery2._3DMap.MapManager Map { get; private set; }


        public event DistancesLoaded OnDistancesLoaded;

        public EDDiscoveryForm()
        {
            InitializeComponent();
            panel_close.Enabled = false;                            // no closing until we are ready for it..
            tabControl1.Enabled = false;

            EDDConfig = new EDDConfig();

            //_fileTgcSystems = Path.Combine(Tools.GetAppDataDirectory(), "tgcsystems.json");
            _fileEDSMDistances = Path.Combine(Tools.GetAppDataDirectory(), "EDSMDistances.json");

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


            theme.RestoreSettings();                                    // theme, remember your saved settings

            trilaterationControl.InitControl(this);
            travelHistoryControl1.InitControl(this);
            imageHandler1.InitControl(this);
            settings.InitControl(this);

            SystemNames = new AutoCompleteStringCollection();
            Map = new EDDiscovery2._3DMap.MapManager();

            ApplyTheme(false);
        }

        public void ApplyTheme(bool refreshhistory)
        {
            this.FormBorderStyle = theme.WindowsFrame ? FormBorderStyle.Sizable : FormBorderStyle.None;
            panel_grip.Visible = !theme.WindowsFrame;
            panel_close.Visible = !theme.WindowsFrame;
            panel_minimize.Visible = !theme.WindowsFrame;
            label_version.Visible = !theme.WindowsFrame;
            label_version.Text = "Version " + Assembly.GetExecutingAssembly().FullName.Split(',')[1].Split('=')[1];
            this.Text = "EDDiscovery " + label_version.Text;            // note in no border mode, this is not visible on the title bar but it is in the taskbar..

            theme.ApplyColors(this);

            if (refreshhistory)
                travelHistoryControl1.RefreshHistory();             // so we repaint this with correct colours.


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

        public TravelHistoryControl TravelControl
        {
            get { return travelHistoryControl1; }
        }


        internal void ShowTrilaterationTab()
        {
            tabControl1.SelectedIndex = 1;
        }

        internal void ShowHistoryTab()
        {
            tabControl1.SelectedIndex = 0;
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
                travelHistoryControl1.Enabled = false;

                var edsmThread = new Thread(GetEDSMSystems) { Name = "Downloading EDSM Systems" };
                var downloadmapsThread = new Thread(DownloadMaps) { Name = "Downloading map Files" };
                edsmThread.Start();
                downloadmapsThread.Start();

                while (edsmThread.IsAlive || downloadmapsThread.IsAlive)
                {
                    Thread.Sleep(50);
                    Application.DoEvents();
                }

                edsmThread.Join();
                downloadmapsThread.Join();

                OnDistancesLoaded += new DistancesLoaded(this.DistancesLoaded);

                GetEDSMDistancesAsync();

                //Application.DoEvents();
                GetEDDBAsync(false);

                routeControl1.textBox_From.AutoCompleteCustomSource = SystemNames;
                routeControl1.textBox_To.AutoCompleteCustomSource = SystemNames;

                imageHandler1.StartWatcher();
                routeControl1.EnableRouteTab(); // now we have systems, we can update this..

                routeControl1.travelhistorycontrol1 = travelHistoryControl1;
                travelHistoryControl1.netlog.OnNewPosition += new NetLogEventHandler(routeControl1.NewPosition);
                travelHistoryControl1.netlog.OnNewPosition += new NetLogEventHandler(travelHistoryControl1.NewPosition);
                travelHistoryControl1.sync.OnNewEDSMTravelLog += new EDSMNewSystemEventHandler(travelHistoryControl1.RefreshEDSMEvent);

                TravelHistoryControl.LogText("Reading travel history " + Environment.NewLine);
                travelHistoryControl1.RefreshHistory();
                travelHistoryControl1.netlog.StartMonitor(this);

                travelHistoryControl1.Enabled = true;
                if (EliteDangerous.CheckStationLogging())
                {
                    panelInfo.Visible = false;
                }


                // Check for a new installer    
                CheckForNewInstaller();

                LogLine("Total number of systems " + SystemData.SystemList.Count().ToString() + Environment.NewLine);
                LogLine("Loading completed!" + Environment.NewLine);

                panel_close.Enabled = true;                            // now we can safely close
                tabControl1.Enabled = true;

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("EDDiscovery_Load exception: " + ex.Message);
                System.Windows.Forms.MessageBox.Show("Trace: " + ex.StackTrace);
                travelHistoryControl1.Enabled = true;

            }
        }


        public void DownloadMaps()
        {
            try
            {
                if (!Directory.Exists(Path.Combine(Tools.GetAppDataDirectory(), "Maps")))
                    Directory.CreateDirectory(Path.Combine(Tools.GetAppDataDirectory(), "Maps"));


                LogText("Checking for new EDDiscovery maps" + Environment.NewLine);

                if (DownloadMapFile("SC-01.jpg"))  // If server down only try one.
                {
                    DownloadMapFile("SC-02.jpg");
                    DownloadMapFile("SC-03.jpg");
                    DownloadMapFile("SC-04.jpg");

                    DownloadMapFile("SC-L4.jpg");
                    DownloadMapFile("SC-U4.jpg");

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
                    DeleteMapFile("DW4.png");

                    //for (int ii = -10; ii <= 60; ii += 10)
                    //{
                    //    DownloadMapFile("Map A+00" + ii.ToString("+00;-00") + ".png");
                    //    DownloadMapFile("Map A+00" + ii.ToString("+00;-00") + ".json");
                    //}
                }
            }
            catch (Exception ex)
            {
                LogText("Exception in DownloadImages:" + ex.Message + Environment.NewLine);
            }

        }

        public void updateMapData()
        {
            Map.Instance.SystemNames = SystemNames;
            Map.Instance.VisitedSystems = VisitedSystems;
        }

        private bool DownloadMapFile(string file)
        {
            EDDBClass eddb = new EDDBClass();
            bool newfile = false;
            if (eddb.DownloadFile("http://eddiscovery.astronet.se/Maps/" + file, Path.Combine(Tools.GetAppDataDirectory(), "Maps\\" + file), out newfile))
            {
                if (newfile)
                    LogText("Downloaded map: " + file + Environment.NewLine);
                return true;

            }
            else
                return false;
        }

        private void DeleteMapFile(string file)
        {
            string filename = Path.Combine(Tools.GetAppDataDirectory(), "Maps\\" + file);

            try
            {
                if (File.Exists(filename))
                    File.Delete(filename);
            }
            catch (Exception ex)
            {
                LogText("Exception in DeleteMapFile:" + ex.Message + Environment.NewLine);

            }

        }


        private bool CanSkipSlowUpdates()
        {
#if DEBUG
            return EDDConfig.CanSkipSlowUpdates;
#else
            return false;
#endif
        }

        private void GetEDSMSystems()
        {
            try
            {
                EDSMClass edsm = new EDSMClass();
                string rwsystime = _db.GetSettingString("EDSMLastSystems", "2000-01-01 00:00:00"); // Latest time from RW file.

                CommanderName = EDDConfig.CurrentCommander.Name;
                //Invoke((MethodInvoker) delegate {
                //    travelHistoryControl1.textBoxCmdrName.Text = CommanderName;
                //});


                //                List<SystemClass> systems = SystemClass.ParseEDSC(json, ref rwsysfiletime);
                DateTime edsmdate = DateTime.Parse(rwsystime, new CultureInfo("sv-SE"));

                if (DateTime.Now.Subtract(edsmdate).TotalDays > 7)  // Over 7 days do a sync from EDSM
                {
                    SyncAllEDSMSystems();
                }
                else
                {
                    if (CanSkipSlowUpdates())
                    {
                        LogLine("Skipping loading updates (DEBUG option).");
                        LogLine("  Need to turn this back on again? Look in the Settings tab.");
                    }
                    else
                    {
                        string retstr = edsm.GetNewSystems(_db);
                        Invoke((MethodInvoker)delegate
                        {
                            TravelHistoryControl.LogText(retstr);
                        });
                    }

                }

                _db.GetAllSystemNotes();
                _db.GetAllSystems();


                Invoke((MethodInvoker)delegate
                {
                    SystemNames.Clear();
                    foreach (SystemClass system in SystemData.SystemList)
                    {
                        SystemNames.Add(system.name);
                    }
                });

            }
            catch (Exception ex)
            {
                Invoke((MethodInvoker)delegate
                {
                    TravelHistoryControl.LogText("GetEDSMSystems exception:" + ex.Message + Environment.NewLine);
                    TravelHistoryControl.LogText(ex.StackTrace + Environment.NewLine);
                });
            }

            GC.Collect();

        }

        private Thread ThreadEDSMDistances;
        private void GetEDSMDistancesAsync()
        {
            ThreadEDSMDistances = new System.Threading.Thread(new System.Threading.ThreadStart(GetEDSMDistances));
            ThreadEDSMDistances.Name = "Get Distances";
            ThreadEDSMDistances.Start();
        }

        private Thread ThreadEDDB;

        public List<SystemPosition> VisitedSystems
        {
            get { return travelHistoryControl1.visitedSystems; }
        }


        private bool eddbforceupdate;
        private void GetEDDBAsync(bool force)
        {
            ThreadEDDB = new System.Threading.Thread(new System.Threading.ThreadStart(GetEDDBUpdate));
            ThreadEDDB.Name = "Get EDDB Update";
            eddbforceupdate = force;
            ThreadEDDB.Start();
        }


        private void GetEDSMDistances()
        {
            try
            {
                if (EDDConfig.UseDistances)
                {
                    EDSMClass edsm = new EDSMClass();
                    EDDBClass eddb = new EDDBClass();
                    string lstdist = _db.GetSettingString("EDSCLastDist", "2010-01-01 00:00:00");
                    string json;

                    // Get distances
                    lstdist = _db.GetSettingString("EDSCLastDist", "2010-01-01 00:00:00");
                    List<DistanceClass> dists = new List<DistanceClass>();

                    if (lstdist.Equals("2010-01-01 00:00:00"))
                    {
                        LogText("Downloading mirrored EDSM distance data. (Might take some time)" + Environment.NewLine);
                        eddb.GetEDSMDistances();
                        json = LoadJsonFile(_fileEDSMDistances);
                        if (json != null)
                        {
                            LogText("Adding mirrored EDSM distance data." + Environment.NewLine);

                            dists = new List<DistanceClass>();
                            dists = DistanceClass.ParseEDSM(json, ref lstdist);
                            LogText("Found " + dists.Count.ToString() + " distances." + Environment.NewLine);

                            Application.DoEvents();
                            DistanceClass.Store(dists);
                            _db.PutSettingString("EDSCLastDist", lstdist);
                        }
                    }


                    LogText("Checking for new distances from EDSM. ");


                    Application.DoEvents();
                    json = edsm.RequestDistances(lstdist);

                    dists = new List<DistanceClass>();
                    dists = DistanceClass.ParseEDSM(json, ref lstdist);

                    if (json == null)
                        LogText("No response from server." + Environment.NewLine);

                    else
                        LogText("Found " + dists.Count.ToString() + " new distances." + Environment.NewLine);

                    Application.DoEvents();
                    DistanceClass.Store(dists);
                    _db.PutSettingString("EDSCLastDist", lstdist);
                }
                _db.GetAllDistances(EDDConfig.UseDistances);  // Load user added distances
                updateMapData();
                OnDistancesLoaded();
                GC.Collect();
            }
            catch (Exception ex)
            {
                LogText("GetEDSMDistances exception:" + ex.Message + Environment.NewLine);
                LogText(ex.StackTrace + Environment.NewLine);
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
                        LogTextHighlight("New EDDiscovery installer availble  " + "http://eddiscovery.astronet.se/release/" + newInstaller + Environment.NewLine);
                    }

                }
            }
        }


        internal void DistancesLoaded()
        {
            Invoke((MethodInvoker)delegate
            {
                travelHistoryControl1.RefreshHistory();
            });
        }


        private void GetEDDBUpdate()
        {
            try
            {
                EDDBClass eddb = new EDDBClass();
                string timestr;
                DateTime time;

                Thread.Sleep(1000);

                timestr = _db.GetSettingString("EDDBSystemsTime", "0");
                time = new DateTime(Convert.ToInt64(timestr), DateTimeKind.Utc);
                bool updatedb = false;



                // Get EDDB data once every week.
                if (DateTime.UtcNow.Subtract(time).TotalDays > 6.5)
                {
                    LogText("Get systems from EDDB. ");

                    if (eddb.GetSystems())
                    {
                        LogText("OK." + Environment.NewLine);

                        _db.PutSettingString("EDDBSystemsTime", DateTime.UtcNow.Ticks.ToString());
                        updatedb = true;
                    }
                    else
                        LogTextHighlight("Failed." + Environment.NewLine);


                    eddb.GetCommodities();
                    eddb.ReadCommodities();
                }


                timestr = _db.GetSettingString("EDDBStationsLiteTime", "0");
                time = new DateTime(Convert.ToInt64(timestr), DateTimeKind.Utc);

                if (DateTime.UtcNow.Subtract(time).TotalDays > 6.5)
                {

                    LogText("Get stations from EDDB. ");
                    if (eddb.GetStationsLite())
                    {
                        LogText("OK." + Environment.NewLine);
                        _db.PutSettingString("EDDBStationsLiteTime", DateTime.UtcNow.Ticks.ToString());
                        updatedb = true;
                    }
                    else
                        LogTextHighlight("Failed." + Environment.NewLine);

                }



                if (updatedb || eddbforceupdate)
                {
                    DBUpdateEDDB(eddb);
                }

                return;

            }
            catch (Exception ex)
            {
                Invoke((MethodInvoker)delegate
                {
                    TravelHistoryControl.LogText("GetEDSCSystems exception:" + ex.Message + Environment.NewLine);
                });
            }

        }

        private void DBUpdateEDDB(EDDBClass eddb)
        {
            List<SystemClass> eddbsystems = eddb.ReadSystems();
            List<StationClass> eddbstations = eddb.ReadStations();

            LogText("Add new EDDB data to database." + Environment.NewLine);
            eddb.Add2DB(eddbsystems, eddbstations);

            eddbsystems.Clear();
            eddbstations.Clear();
            eddbsystems = null;
            GC.Collect();
            LogText("EDDB update done." + Environment.NewLine);
        }


        private void LogText(string text)
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    TravelHistoryControl.LogText(text);
                });
            }
            catch
            {
            }
        }

        public void LogTextHighlight(string text)
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    TravelHistoryControl.LogTextHighlight(text);

                });
            }
            catch
            {
            }
        }

        public void LogLine(string text)
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    TravelHistoryControl.LogText(text + Environment.NewLine);
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
                    TravelHistoryControl.LogTextHighlight(text + Environment.NewLine);

                });
            }
            catch
            {
            }
        }

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


        private string LoadJSON(string jfile)
        {
            string json = null;
            try
            {
                string appdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\EDDiscovery";

                if (!Directory.Exists(appdata))
                    Directory.CreateDirectory(appdata);

                string filename = appdata + "\\" + jfile;

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


        private void SaveSettings()
        {
            settings.SaveSettings();

            _db.PutSettingInt("FormWidth", this.Width);
            _db.PutSettingInt("FormHeight", this.Height);
            _db.PutSettingInt("FormTop", this.Top);
            _db.PutSettingInt("FormLeft", this.Left);
            routeControl1.SaveSettings();
            theme.SaveSettings(null);

            _db.PutSettingBool("EDSMSyncTo", travelHistoryControl1.checkBoxEDSMSyncTo.Checked);
            _db.PutSettingBool("EDSMSyncFrom", travelHistoryControl1.checkBoxEDSMSyncFrom.Checked);
        }

        private void EDDiscoveryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            travelHistoryControl1.netlog.StopMonitor();
            _edsmSync.StopSync();
            SaveSettings();
        }

        private void button_test_Click(object sender, EventArgs e)
        {
            //FormSagCarinaMission frm = new FormSagCarinaMission(this);
            //            frm.Show();

            //SystemViewForm frm = new SystemViewForm();
            //frm.Show();

            //EdMaterializer mat = new EdMaterializer();

            //mat.GetAllWorlds(null);


            //EDWorld obj = new EDWorld();

            //obj.updater = "Test";
            //obj.system = "Fine Ring Sector JH-V C2-4";
            //obj.objectName = "A 3";
            //obj.ObjectType = ObjectTypesEnum.HighMetalContent;
            //obj.arrivalPoint = 0;
            //obj.gravity = 0.13f;

            //obj.materials[MaterialEnum.Carbon] = true;
            //obj.materials[MaterialEnum.Iron] = true;
            //obj.materials[MaterialEnum.Nickel] = true;
            //obj.materials[MaterialEnum.Phosphorus] = true;
            //obj.materials[MaterialEnum.Sulphur] = true;
            //obj.materials[MaterialEnum.Germanium] = true;
            //obj.materials[MaterialEnum.Selenium] = true;
            //obj.materials[MaterialEnum.Vanadium] = true;
            //obj.materials[MaterialEnum.Cadmium] = true;
            //obj.materials[MaterialEnum.Molybdenum] = true;
            //obj.materials[MaterialEnum.Tin] = true;
            //obj.materials[MaterialEnum.Polonium] = true;

            //mat.DeletePlanetID(5);
            //mat.DeletePlanetID(6);
            //mat.DeletePlanetID(7);

            //mat.StorePlanet(obj);
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

            frm.Show();
        }

        private void forceEDDBUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetEDDBAsync(true);
        }

        //Pleiades Sector WU-O B16-0
        //Pleiades Sector WU-O b6-0

        private void InitFormControls()
        {
            labelPanelText.Text = "Loading. Please wait!";
            panelInfo.Visible = true;
            panelInfo.BackColor = Color.Gold;

            routeControl1.travelhistorycontrol1 = travelHistoryControl1;
        }

        private void RepositionForm()
        {
            var top = _db.GetSettingInt("FormTop", -1);
            if (top > 0)
            {
                var left = _db.GetSettingInt("FormLeft", -1);
                var height = _db.GetSettingInt("FormHeight", -1);
                var width = _db.GetSettingInt("FormWidth", -1);
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
                TravelHistoryControl.LogText("EliteDangerous is running." + Environment.NewLine);
            }
            else
            {
                TravelHistoryControl.LogText("EliteDangerous is not running ." + Environment.NewLine);
            }
        }

        private void CheckIfVerboseLoggingIsTurnedOn()
        {
            if (!EliteDangerous.CheckStationLogging())
            {
                TravelHistoryControl.LogTextHighlight("Elite Dangerous is not logging system names!!! ");
                TravelHistoryControl.LogText("Add ");
                TravelHistoryControl.LogText("VerboseLogging=\"1\" ");
                TravelHistoryControl.LogText("to <Network  section in File: " + Path.Combine(EliteDangerous.EDDirectory, "AppConfig.xml") + " or AppConfigLocal.xml  Remember to restart Elite!" + Environment.NewLine);

                labelPanelText.Text = "Elite Dangerous is not logging system names!";
                panelInfo.BackColor = Color.Salmon;
            }
        }

        private void prospectingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlanetsForm frm = new PlanetsForm();

            frm.InitForm(this);
            frm.Show();
        }

        
        private void syncEDSMSystemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AsyncSyncEDSMSystems();
        }

        private void AsyncSyncEDSMSystems()
        {
            var EDSMThread = new Thread(SyncAllEDSMSystems) { Name = "Downloading EDSM system" };
            EDSMThread.Start();
        }

        private void SyncAllEDSMSystems()
        {
            try
            {
                EDDBClass eddb = new EDDBClass();
                EDSMClass edsm = new EDSMClass();

                string edsmsystems = Path.Combine(Tools.GetAppDataDirectory(), "edsmsystems.json");
                bool newfile = false;
                string rwsysfiletime = "2014-01-01 00:00:00";
                LogText("Get systems from EDSM." + Environment.NewLine);

                eddb.DownloadFile("http://www.edsm.net/dump/systemsWithCoordinates.json", edsmsystems, out newfile);

                if (newfile)
                {
                    LogText("Adding EDSM systems." + Environment.NewLine);
                    _db.GetAllSystems();
                    string json = LoadJsonFile(edsmsystems);
                    List<SystemClass> systems = SystemClass.ParseEDSM(json, ref rwsysfiletime);


                    List<SystemClass> systems2Store = new List<SystemClass>();

                    foreach (SystemClass system in systems)
                    {
                        // Check if sys exists first
                        SystemClass sys = SystemData.GetSystem(system.name);
                        if (sys == null)
                            systems2Store.Add(system);
                        else if (!sys.name.Equals(system.name) || sys.x != system.x || sys.y != system.y || sys.z != system.z)  // Case or position changed
                            systems2Store.Add(system);
                    }
                    SystemClass.Store(systems2Store);
                    systems.Clear();
                    systems = null;
                    systems2Store.Clear();
                    systems2Store = null;
                    json = null;

                    _db.PutSettingString("EDSMLastSystems", rwsysfiletime);
                    _db.GetAllSystems();
                }
                else
                    LogText("No new file." + Environment.NewLine);

                string retstr = edsm.GetNewSystems(_db);
                Invoke((MethodInvoker)delegate
                {
                    TravelHistoryControl.LogText(retstr);
                });

                GC.Collect();
            }
            catch (Exception ex)
            {
                Invoke((MethodInvoker)delegate
                {
                    TravelHistoryControl.LogText("GetAllEDSMSystems exception:" + ex.Message + Environment.NewLine);
                });
            }

        }

        private void gitHubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/EDDiscovery/EDDiscovery");
        }

        private void reportIssueIdeasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/EDDiscovery/EDDiscovery/issues");
        }

        private void keepOnTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem mi = sender as ToolStripMenuItem;
            mi.Checked = !mi.Checked;
            this.TopMost = mi.Checked;
        }

        private void EDDiscoveryForm_Activated(object sender, EventArgs e)
        {
            /* TODO: Only focus the field if we're on the correct tab! */
            if (fastTravelToolStripMenuItem.Checked && tabControl1.SelectedTab == tabPageTravelHistory)
            {
                travelHistoryControl1.textBoxDistanceToNextSystem.Focus();
            }
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


        private void AboutBox()
        {
            AboutForm frm = new AboutForm();
            string atext = Assembly.GetExecutingAssembly().FullName;
            atext = atext.Split(',')[1].Split('=')[1];
            frm.labelVersion.Text = this.Text + " " + atext;
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
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void menuStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            EDDiscoveryForm_MouseDown(sender, e);
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            AboutBox();
        }

        private void panel_close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel_grip_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCL_RESIZE, HT_RESIZE, 0);
            }
        }


    }
}
