using EDDiscovery.DB;
using EDDiscovery2;
using EDDiscovery2.DB;
using EDDiscovery2.EDDB;
using EDDiscovery2.EDSM;
using EMK.Cartography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery
{
    public delegate void DistancesLoaded();

    public partial class EDDiscoveryForm : Form
    {
        public readonly AutoCompleteStringCollection SystemNames = new AutoCompleteStringCollection();
        public string CommanderName;

        readonly string fileTgcSystems ;
        readonly string fileTgcDistances;

        public event DistancesLoaded OnDistancesLoaded;

        public EDDiscoveryForm()
        {
            InitializeComponent();
           
            fileTgcSystems = Path.Combine(Tools.GetAppDataDirectory(), "tgcsystems.json");
            fileTgcDistances = Path.Combine(Tools.GetAppDataDirectory(), "tgcdistances.json");

            trilaterationControl.InitControl(this);
            travelHistoryControl1.InitControl(this);
 
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

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Click once   System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVe‌​rsion
                var assemblyFullName = Assembly.GetExecutingAssembly().FullName;
                var version = assemblyFullName.Split(',')[1].Split('=')[1];
                Text = string.Format("EDDiscovery v{0}", version);
                EliteDangerous.CheckED();

                labelPanelText.Text = "Loading. Please wait!";
                panelInfo.Visible = true;
                panelInfo.BackColor = Color.Gold;

                SystemData sdata = new SystemData();
                routeControl1.travelhistorycontrol1 = travelHistoryControl1;

                string datapath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Frontier_Developments\\Products"); // \\FORC-FDEV-D-1001\\Logs\\";

                EDDiscovery2.Properties.Settings.Default.Upgrade();

                if (EDDiscovery2.Properties.Settings.Default.Netlogdir.Equals(""))
                    EDDiscovery2.Properties.Settings.Default.Netlogdir = datapath;

                if (EDDiscovery2.Properties.Settings.Default.NetlogDirAutoMode)
                {
                    textBoxNetLogDir.Text = datapath;
                    radioButton_Auto.Checked = true;
                }
                else
                {
                    radioButton_Manual.Checked = true;
                    textBoxNetLogDir.Text = EDDiscovery2.Properties.Settings.Default.Netlogdir;
                }


                if (EliteDangerous.EDRunning)
                {
                    TravelHistoryControl.LogText("EliteDangerous " + EliteDangerous.EDVersion + " is running." + Environment.NewLine);
                }
                else
                    TravelHistoryControl.LogText("EliteDangerous is not running ." + Environment.NewLine);

                if (!EliteDangerous.CheckStationLogging())
                {
                    TravelHistoryControl.LogText("Elite Dangerous is not logging system names!!! ", Color.Red);
                    TravelHistoryControl.LogText("Add ");
                    TravelHistoryControl.LogText("VerboseLogging=\"1\" ", Color.Blue);
                    TravelHistoryControl.LogText("to <Network  section in File: " + Path.Combine(EliteDangerous.EDDirectory, "AppConfig.xml") + " or AppConfigLocal.xml  Remeber to restart Elite!" + Environment.NewLine);

                    labelPanelText.Text = "Elite Dangerous is not logging system names!";
                    panelInfo.BackColor = Color.Salmon;
                }


                if (File.Exists("test.txt"))
                    button1.Visible = true;


            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Form1_Load exception: " + ex.Message);
                System.Windows.Forms.MessageBox.Show("Trace: " + ex.StackTrace);
            }
        }



        private void EDDiscoveryForm_Shown(object sender, EventArgs e)
        {
            try
            {
                travelHistoryControl1.Enabled = false;

                var redWizzardThread = new Thread(GetRedWizzardFiles) {Name = "Downloading Red Wizzard Files"};
                var edscThread = new Thread(GetEDSCSystems) {Name = "Downloading EDSC Systems"};

                redWizzardThread.Start();
                edscThread.Start();

                while (redWizzardThread.IsAlive || edscThread.IsAlive)
                {
                    Thread.Sleep(50);
                    Application.DoEvents();
                }

                redWizzardThread.Join();
                edscThread.Join();


                OnDistancesLoaded += new DistancesLoaded(this.DistancesLoaded);


                GetEDSCDistancesAsync();
                //Application.DoEvents();
                GetEDDBAsync();


                if (SystemData.SystemList.Count == 0)
                {
                    //sdata.ReadData();
                }



                routeControl1.textBox_From.AutoCompleteCustomSource = SystemNames;
                routeControl1.textBox_To.AutoCompleteCustomSource = SystemNames;

                Text += "         Systems:  " + SystemData.SystemList.Count;

                routeControl1.travelhistorycontrol1 = travelHistoryControl1;
                travelHistoryControl1.netlog.OnNewPosition += new NetLogEventHandler(routeControl1.NewPosition);
                travelHistoryControl1.netlog.OnNewPosition += new NetLogEventHandler(travelHistoryControl1.NewPosition);

                TravelHistoryControl.LogText("Reading travelhistory ");
                travelHistoryControl1.RefreshHistory();
                travelHistoryControl1.netlog.StartMonitor();

                travelHistoryControl1.Enabled = true;
                if (EliteDangerous.CheckStationLogging())
                {
                    panelInfo.Visible = false;
                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Form1_Load exception: " + ex.Message);
                System.Windows.Forms.MessageBox.Show("Trace: " + ex.StackTrace);
                travelHistoryControl1.Enabled = true;

            }
        }


        private void GetRedWizzardFiles()
        {
            WebClient web = new WebClient();

            try
            {
                LogText("Checking for new EDDiscovery data" + Environment.NewLine);

                GetNewRedWizzardFile(fileTgcSystems, "http://robert.astronet.se/Elite/ed-systems/tgcsystems.json");
                GetNewRedWizzardFile(fileTgcDistances, "http://robert.astronet.se/Elite/ed-systems/tgcdistances.json");
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


        private void GetEDSCSystems()
        {
            try
            {
                SQLiteDBClass db = new SQLiteDBClass();
                EDSMClass edsm = new EDSMClass();


                string json;

                string rwsystime = db.GetSettingString("RWLastSystems", "2000-01-01 00:00:00"); // Latest time from RW file.
                string rwsysfiletime = "";

                CommanderName = db.GetSettingString("CommanderName", "");
                Invoke((MethodInvoker) delegate {
                    travelHistoryControl1.textBoxCmdrName.Text = CommanderName;
                });


                json = LoadJsonArray(fileTgcSystems);
                List<SystemClass> systems = SystemClass.ParseEDSC(json, ref rwsysfiletime);


                if (!rwsystime.Equals(rwsysfiletime))  // New distance file from Redwizzard
                {
                    SystemClass.Delete(SystemStatusEnum.EDSC); // Remove all EDSC systems.
                    
                    db.PutSettingString("RWLastSystems", rwsysfiletime);
                    db.PutSettingString("EDSMLastSystems", rwsysfiletime);
                    Invoke((MethodInvoker) delegate {
                        TravelHistoryControl.LogText("Adding data from tgcsystems.json " + Environment.NewLine);
                    });
                    SystemClass.Store(systems);
                    EDDBClass eddb = new EDDBClass();
                    DBUpdateEDDB(eddb);
                }

                string retstr = edsm.GetNewSystems(db);
                Invoke((MethodInvoker)delegate
                {
                    TravelHistoryControl.LogText(retstr);
                });



                db.GetAllSystemNotes();
                db.GetAllSystems();



                SystemNames.Clear();
                foreach (SystemClass system in SystemData.SystemList)
                {
                    SystemNames.Add(system.name);
                }

            }
            catch (Exception ex)
            {
                Invoke((MethodInvoker) delegate {
                    TravelHistoryControl.LogText("GetEDSCSystems exception:" + ex.Message + Environment.NewLine);
                });
            }

        }

        private Thread ThreadEDSCDistances;
        private void GetEDSCDistancesAsync()
        {
            ThreadEDSCDistances = new System.Threading.Thread(new System.Threading.ThreadStart(GetEDSCDistances));
            ThreadEDSCDistances.Name = "Get Distances";
            ThreadEDSCDistances.Start();
        }

        private Thread ThreadEDDB;

        public List<SystemPosition> visitedSystems
        {
            get { return travelHistoryControl1.visitedSystems; }
        }

        private void GetEDDBAsync()
        {
            ThreadEDDB = new System.Threading.Thread(new System.Threading.ThreadStart(GetEDDBUpdate));
            ThreadEDDB.Name = "Get EDDB Update";
            ThreadEDDB.Start();
        }

   
        private void GetEDSCDistances()
        {
            try
            {
                SQLiteDBClass db = new SQLiteDBClass();
                EDSCClass edsc = new EDSCClass();
                string lstdist = db.GetSettingString("EDSCLastDist", "2010-01-01 00:00:00");
                string json;

                // Get distances
                string rwdisttime = db.GetSettingString("RWLastDist", "2000-01-01 00:00:00"); // Latest time from RW file.
                string rwdistfiletime = "";
                lstdist = db.GetSettingString("EDSCLastDist", "2010-01-01 00:00:00");
                List<DistanceClass> dists = new List<DistanceClass>();

                json = LoadJsonArray(fileTgcDistances);
                dists = DistanceClass.ParseEDSC(json, ref rwdistfiletime);

                if (!rwdisttime.Equals(rwdistfiletime))  // New distance file from Redwizzard
                {
                    DistanceClass.Delete(DistancsEnum.EDSC); // Remove all EDSC distances.
                    lstdist = "2010-01-01 00:00:00";
                    db.PutSettingString("RWLastDist", rwdistfiletime);
                }

                if (lstdist.Equals("2010-01-01 00:00:00"))
                {
                    LogText("Adding data from tgcdistances.json " + Environment.NewLine);
                   

                    lstdist = rwdistfiletime;

                    if (json == null)
                        LogText("Couldn't read file." + Environment.NewLine);
                        
                    else
                    {
                        LogText("Found " + dists.Count.ToString() + " new distances." + Environment.NewLine);
                        
                        DistanceClass.Store(dists);
                        db.PutSettingString("EDSCLastDist", lstdist);
                    }

                }

                LogText("Checking for new distances from EDSC. ");
               
                Application.DoEvents();
                json = edsc.RequestDistances(lstdist);

                dists = new List<DistanceClass>();
                dists = DistanceClass.ParseEDSC(json, ref lstdist);

                if (json == null)
                    LogText("No response from server." + Environment.NewLine);
                   
                else
                    LogText("Found " + dists.Count.ToString() + " new distances." + Environment.NewLine);
                    
                Application.DoEvents();
                DistanceClass.Store(dists);
                db.PutSettingString("EDSCLastDist", lstdist);
                db.GetAllDistances();
                OnDistancesLoaded();

                // Check for a new installer    
                CheckForNewInstaller();

            }
            catch (Exception ex)
            {
                LogText("GetEDSCDistances exception:" + ex.Message + Environment.NewLine);
                
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

                SQLiteDBClass db = new SQLiteDBClass();
                timestr = db.GetSettingString("EDDBSystemsTime", "0");
                time = new DateTime(Convert.ToInt64(timestr), DateTimeKind.Utc);
                bool updatedb = false;

                if (DateTime.UtcNow.Subtract(time).TotalDays > 0.5)
                {
                    LogText("Get systems from EDDB. ");

                    if (eddb.GetSystems())
                    {
                        LogText("OK." + Environment.NewLine);

                        db.PutSettingString("EDDBSystemsTime", DateTime.UtcNow.Ticks.ToString());
                        updatedb = true;
                    }
                    else
                        LogText("Failed." + Environment.NewLine, Color.Red);


                    eddb.GetCommodities();
                }


                timestr = db.GetSettingString("EDDBStationsLiteTime", "0");
                time = new DateTime(Convert.ToInt64(timestr), DateTimeKind.Utc);

                if (DateTime.UtcNow.Subtract(time).TotalDays > 0.5)
                {

                    LogText("Get stations from EDDB. ");
                    if (eddb.GetStationsLite())
                    {
                        LogText("OK." + Environment.NewLine);
                        db.PutSettingString("EDDBStationsLiteTime", DateTime.UtcNow.Ticks.ToString());
                        updatedb = true;
                    }
                    else
                        LogText("Failed." + Environment.NewLine, Color.Red);

                }


                if (updatedb)
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

        private void LogText(string text, Color col)
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

        private string LoadJsonArray(string filename)
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
            EDDiscovery2.Properties.Settings.Default.Netlogdir = textBoxNetLogDir.Text;
            EDDiscovery2.Properties.Settings.Default.NetlogDirAutoMode = radioButton_Auto.Checked;
            EDDiscovery2.Properties.Settings.Default.Save();
        }

        private void routeControl1_Load(object sender, EventArgs e)
        {

        }

        private void EDDiscoveryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            travelHistoryControl1.netlog.StopMonitor();
            SaveSettings();
        }

        private void travelHistoryControl1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
//            EDDiscoveryServer srv = new EDDiscoveryServer() ;
//            List<SystemClass> systems;
//            string date = "";
//            string json;
//            json = srv.RequestSystems("2011-01-01");

////            srv.PostSystem();


            //            json = LoadJsonArray(fileTgcSystems);
     

//            systems = SystemClass.ParseEDSC(json, ref date);

//            foreach (SystemClass system in systems)
//            {
//                srv.AddSystem(system);
//            }


            //EDDBClass eddb = new EDDBClass();

            //List<SystemClass> eddbsystems = eddb.ReadSystems();
            //List<StationClass> eddbstations = eddb.ReadStations();

            //eddb.Add2DB(eddbsystems, eddbstations);


            TestTrileteration();


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






        //Pleiades Sector WU-O B16-0
        //Pleiades Sector WU-O b6-0

    }
}
