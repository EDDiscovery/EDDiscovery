using EDDiscovery.DB;
using EDDiscovery2;
using EDDiscovery2.DB;
using EDDiscovery2.EDDB;
using EDDiscovery2.EDSM;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace EDDiscovery
{
    public delegate void DistancesLoaded();

    public partial class EDDiscoveryForm : Form
    {
        readonly string _fileTgcSystems;
        readonly string _fileEDSMDistances;
        private EDSMSync _edsmSync;
        private SQLiteDBClass _db = new SQLiteDBClass();

        public AutoCompleteStringCollection SystemNames { get; private set; }
        public string CommanderName { get; private set; }
        static public EDDConfig EDDConfig { get; private set; }
        public EDDiscovery2._3DMap.MapManager Map { get; private set; }

        public event DistancesLoaded OnDistancesLoaded;

        public EDDiscoveryForm()
        {
            InitializeComponent();

            EDDConfig = new EDDConfig();

            _fileTgcSystems = Path.Combine(Tools.GetAppDataDirectory(), "tgcsystems.json");
            _fileEDSMDistances = Path.Combine(Tools.GetAppDataDirectory(), "EDSMDistances.json");        

            string logpath="";
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

            trilaterationControl.InitControl(this);
            travelHistoryControl1.InitControl(this);
            imageHandler1.InitControl(this);

            SystemNames = new AutoCompleteStringCollection();
            Map = new EDDiscovery2._3DMap.MapManager();
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
                InitSettingsTab();
                CheckIfEliteDangerousIsRunning();
                CheckIfVerboseLoggingIsTurnedOn();

                if (File.Exists("test.txt"))
                {
                    button1.Visible = true;
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

                var redWizzardThread = new Thread(GetRedWizzardFiles) {Name = "Downloading Red Wizzard Files"};
                var edsmThread = new Thread(GetEDSMSystems) {Name = "Downloading EDSM Systems"};
                var downloadmapsThread = new Thread(DownloadMaps) { Name = "Downloading map Files" };
                redWizzardThread.Start();
                edsmThread.Start();
                downloadmapsThread.Start();

                while (redWizzardThread.IsAlive || edsmThread.IsAlive || downloadmapsThread.IsAlive)
                {
                    Thread.Sleep(50);
                    Application.DoEvents();
                }

                redWizzardThread.Join();
                edsmThread.Join();
                downloadmapsThread.Join();

                OnDistancesLoaded += new DistancesLoaded(this.DistancesLoaded);

                 GetEDSMDistancesAsync();

                //Application.DoEvents();
                GetEDDBAsync(false);

                routeControl1.textBox_From.AutoCompleteCustomSource = SystemNames;
                routeControl1.textBox_To.AutoCompleteCustomSource = SystemNames;

                Text += "         Systems:  " + SystemData.SystemList.Count;

                routeControl1.travelhistorycontrol1 = travelHistoryControl1;
                travelHistoryControl1.netlog.OnNewPosition += new NetLogEventHandler(routeControl1.NewPosition);
                travelHistoryControl1.netlog.OnNewPosition += new NetLogEventHandler(travelHistoryControl1.NewPosition);
                travelHistoryControl1.sync.OnNewEDSMTravelLog += new EDSMNewSystemEventHandler(travelHistoryControl1.RefreshEDSMEvent);

                TravelHistoryControl.LogText("Reading travelhistory ");
                travelHistoryControl1.RefreshHistory();
                travelHistoryControl1.netlog.StartMonitor();

                travelHistoryControl1.Enabled = true;
                if (EliteDangerous.CheckStationLogging())
                {
                    panelInfo.Visible = false;
                }


                // Check for a new installer    
                CheckForNewInstaller();

                LogLine($"{Environment.NewLine}Loading completed!");
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

        private bool CanSkipSlowUpdates()
        {
#if DEBUG
            return EDDConfig.CanSkipSlowUpdates;
#else
            return false;
#endif
        }

        private void GetRedWizzardFiles()
        {
            WebClient web = new WebClient();

            try
            {
                LogText("Checking for new EDDiscovery data" + Environment.NewLine);

                GetNewRedWizzardFile(_fileTgcSystems, "http://robert.astronet.se/Elite/ed-systems/tgcsystems.json");
                //GetNewRedWizzardFile(fileTgcDistances, "http://robert.astronet.se/Elite/ed-systems/tgcdistances.json");
            }
            catch (Exception ex)
            {
                LogText("GetRedWizzardFiles exception:" + ex.Message + Environment.NewLine);
                return;
            }
        }
        
        private void GetNewRedWizzardFile(string filename, string url)
        {
            string etagFilename = filename + ".etag";

            var request = (HttpWebRequest) HttpWebRequest.Create(url);
            request.UserAgent = "EDDiscovery v" + Assembly.GetExecutingAssembly().FullName.Split(',')[1].Split('=')[1];
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            if (File.Exists(etagFilename))
            {
                var etag = File.ReadAllText(etagFilename);
                if (etag != "")
                {
                   request.Headers[HttpRequestHeader.IfNoneMatch] = etag;
                }
            }

            try {
                var response = (HttpWebResponse) request.GetResponse();
                
                LogText("Downloading " + filename + "..." + Environment.NewLine);

                File.WriteAllText(filename + ".etag.tmp", response.Headers[HttpResponseHeader.ETag]);
                var destFileStream = File.Open(filename + ".tmp", FileMode.Create, FileAccess.Write);
                response.GetResponseStream().CopyTo(destFileStream);
                
                destFileStream.Close();
                response.Close();

                if (File.Exists(filename))
                    File.Delete(filename);
                if (File.Exists(etagFilename))
                    File.Delete(etagFilename);

                File.Move(filename + ".tmp", filename);
                File.Move(etagFilename + ".tmp", etagFilename);
            } catch (WebException e)
            {
                var code = ((HttpWebResponse) e.Response).StatusCode;
                if (code == HttpStatusCode.NotModified)
                {
                    LogText(filename + " is up to date." + Environment.NewLine);
                } else
                {
                    throw e;
                }
            }
        }


        private void GetEDSMSystems()
        {
            try
            {
                EDSMClass edsm = new EDSMClass();


                string json;

                string rwsystime = _db.GetSettingString("RWLastSystems", "2000-01-01 00:00:00"); // Latest time from RW file.
                string rwsysfiletime = "";

                CommanderName = _db.GetSettingString("CommanderName", "");
                Invoke((MethodInvoker) delegate {
                    travelHistoryControl1.textBoxCmdrName.Text = CommanderName;
                });


                json = LoadJsonFile(_fileTgcSystems);
                List<SystemClass> systems = SystemClass.ParseEDSC(json, ref rwsysfiletime);


                if (!rwsystime.Equals(rwsysfiletime))  // New distance file from Redwizzard
                {
                    SystemClass.Delete(SystemStatusEnum.EDSC); // Remove all EDSC systems.
                    
                    _db.PutSettingString("RWLastSystems", rwsysfiletime);
                    _db.PutSettingString("EDSMLastSystems", rwsysfiletime);
                    Invoke((MethodInvoker) delegate {
                        TravelHistoryControl.LogText("Adding data from tgcsystems.json " + Environment.NewLine);
                    });
                    SystemClass.Store(systems);
                    EDDBClass eddb = new EDDBClass();
                    DBUpdateEDDB(eddb);
                }

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


                _db.GetAllSystemNotes();
                _db.GetAllSystems();



                SystemNames.Clear();
                foreach (SystemClass system in SystemData.SystemList)
                {
                    SystemNames.Add(system.name);
                }

            }
            catch (Exception ex)
            {
                Invoke((MethodInvoker) delegate {
                    TravelHistoryControl.LogText("GetEDSMSystems exception:" + ex.Message + Environment.NewLine);
                    TravelHistoryControl.LogText(ex.StackTrace + Environment.NewLine);
                });
            }

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
                        LogText("New EDDiscovery installer availble  " + "http://eddiscovery.astronet.se/release/" + newInstaller + Environment.NewLine, Color.Salmon);
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


        private void  GetEDDBUpdate()
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




                if (DateTime.UtcNow.Subtract(time).TotalDays > 0.5)
                {
                    LogText("Get systems from EDDB. ");

                    if (eddb.GetSystems())
                    {
                        LogText("OK." + Environment.NewLine);

                        _db.PutSettingString("EDDBSystemsTime", DateTime.UtcNow.Ticks.ToString());
                        updatedb = true;
                    }
                    else
                        LogText("Failed." + Environment.NewLine, Color.Red);


                    eddb.GetCommodities();
                }


                timestr = _db.GetSettingString("EDDBStationsLiteTime", "0");
                time = new DateTime(Convert.ToInt64(timestr), DateTimeKind.Utc);

                if (DateTime.UtcNow.Subtract(time).TotalDays > 0.5)
                {

                    LogText("Get stations from EDDB. ");
                    if (eddb.GetStationsLite())
                    {
                        LogText("OK." + Environment.NewLine);
                        _db.PutSettingString("EDDBStationsLiteTime", DateTime.UtcNow.Ticks.ToString());
                        updatedb = true;
                    }
                    else
                        LogText("Failed." + Environment.NewLine, Color.Red);

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

        public void LogText(string text, Color col)
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    TravelHistoryControl.LogText(text, col);

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

        public void LogLine(string text, Color col)
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    TravelHistoryControl.LogText(text + Environment.NewLine, col);

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

                string filename = appdata + "\\"+jfile;
                
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




        private void button_Browse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dirdlg = new FolderBrowserDialog();


            DialogResult dlgResult = dirdlg.ShowDialog();

            if (dlgResult == DialogResult.OK)
            {
                textBoxNetLogDir.Text = dirdlg.SelectedPath;
            }
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            SaveSettings();

            tabControl1.SelectedTab = tabPageTravelHistory;
            travelHistoryControl1.RefreshHistory();
        }

        private void SaveSettings()
        {
            _db.PutSettingBool("NetlogDirAutoMode", radioButton_Auto.Checked);
            _db.PutSettingString("Netlogdir", textBoxNetLogDir.Text);
            _db.PutSettingString("EDSMApiKey", textBoxEDSMApiKey.Text);
            _db.PutSettingInt("FormWidth", this.Width);
            _db.PutSettingInt("FormHeight", this.Height);
            _db.PutSettingInt("FormTop", this.Top);
            _db.PutSettingInt("FormLeft", this.Left);
            EDDConfig.UseDistances = checkBox_Distances.Checked;
            EDDConfig.EDSMLog = checkBoxEDSMLog.Checked;
            EDDConfig.CanSkipSlowUpdates = checkboxSkipSlowUpdates.Checked;
        }

        private void routeControl1_Load(object sender, EventArgs e)
        {

        }

        private void EDDiscoveryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            travelHistoryControl1.netlog.StopMonitor();
            _edsmSync.StopSync();
            SaveSettings();
        }

        private void travelHistoryControl1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //FormSagCarinaMission frm = new FormSagCarinaMission(this);
            //            frm.Show();

            SystemViewForm frm = new SystemViewForm();
            frm.Show();
          
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

        private void label1_Click(object sender, EventArgs e)
        {

        }



        private void TestTrileteration()
        {
            foreach (SystemClass System in SQLiteDBClass.globalSystems)
            {
                if (DateTime.Now.Subtract(System.CreateDate).TotalDays < 60)
                {
                    //var Distances = from SQLiteDBClass.globalDistances

                    var distances1 = from p in SQLiteDBClass.dictDistances where p.Value.NameA.ToLower() == System.SearchName select p.Value;
                    var distances2 = from p in SQLiteDBClass.dictDistances where p.Value.NameB.ToLower() == System.SearchName select p.Value;

                    int nr = distances1.Count();
                    //nr = distances2.Count();


                    if (nr > 4)
                    {
                        var trilateration = new Trilateration();
                        //                    trilateration.Logger = (s) => System.Console.WriteLine(s);

                        foreach (var item in distances1)
                        {
                            SystemClass distsys = SystemData.GetSystem(item.NameB);
                            if (distsys != null)
                            {
                                if (distsys.HasCoordinate)
                                {
                                    Trilateration.Entry entry = new Trilateration.Entry(distsys.x, distsys.y, distsys.z, item.Dist);
                                    trilateration.AddEntry(entry);
                                }
                            }
                        }

                        foreach (var item in distances2)
                        {
                            SystemClass distsys = SystemData.GetSystem(item.NameA);
                            if (distsys != null)
                            {
                                if (distsys.HasCoordinate)
                                {
                                    Trilateration.Entry entry = new Trilateration.Entry(distsys.x, distsys.y, distsys.z, item.Dist);
                                    trilateration.AddEntry(entry);
                                }
                            }
                        }


                        var csharpResult = trilateration.Run(Trilateration.Algorithm.RedWizzard_Native);
                        var javascriptResult = trilateration.Run(Trilateration.Algorithm.RedWizzard_Emulated);
                        if (javascriptResult.State == Trilateration.ResultState.Exact)
                            nr++;
                    }
                }
            }
        }

        private void show2DMapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSagCarinaMission frm = new FormSagCarinaMission(this);

            frm.Show();
        }

        private void setDefaultMapColourToolStripMenuItem_Click(object sender, EventArgs e)
        {
            travelHistoryControl1.setDefaultMapColour();
        }

        private void forceEDDBUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetEDDBAsync(true);
        }

        //Pleiades Sector WU-O B16-0
        //Pleiades Sector WU-O b6-0

        private void InitFormControls()
        {
            UpdateTitle();

            labelPanelText.Text = "Loading. Please wait!";
            panelInfo.Visible = true;
            panelInfo.BackColor = Color.Gold;

            routeControl1.travelhistorycontrol1 = travelHistoryControl1;
        }

        private void UpdateTitle()
        {
            var assemblyFullName = Assembly.GetExecutingAssembly().FullName;
            var version = assemblyFullName.Split(',')[1].Split('=')[1];
            Text = string.Format("EDDiscovery v{0}", version);
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

        private void InitSettingsTab()
        {
            // Default directory
            bool auto = _db.GetSettingBool("NetlogDirAutoMode", true);
            if (auto)
            {
                radioButton_Auto.Checked = auto;
            }
            else
            {
                radioButton_Manual.Checked = true;
            }
            string datapath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Frontier_Developments\\Products"); // \\FORC-FDEV-D-1001\\Logs\\";
            textBoxNetLogDir.Text = _db.GetSettingString("Netlogdir", datapath);

            textBoxEDSMApiKey.Text = _db.GetSettingString("EDSMApiKey", "");
            checkBox_Distances.Checked = EDDConfig.UseDistances;
            checkBoxEDSMLog.Checked = EDDConfig.EDSMLog;

            checkboxSkipSlowUpdates.Checked = EDDConfig.CanSkipSlowUpdates;
#if DEBUG
            checkboxSkipSlowUpdates.Visible = true;
#endif
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
                TravelHistoryControl.LogText("Elite Dangerous is not logging system names!!! ", Color.Red);
                TravelHistoryControl.LogText("Add ");
                TravelHistoryControl.LogText("VerboseLogging=\"1\" ", Color.Blue);
                TravelHistoryControl.LogText("to <Network  section in File: " + Path.Combine(EliteDangerous.EDDirectory, "AppConfig.xml") + " or AppConfigLocal.xml  Remember to restart Elite!" + Environment.NewLine);

                labelPanelText.Text = "Elite Dangerous is not logging system names!";
                panelInfo.BackColor = Color.Salmon;
            }
        }

        private void prospectingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormProspecting frm = new FormProspecting();

            frm.Show();
        }
    }
}
