using EDDiscovery.DB;
using EDDiscovery2;
using EDDiscovery2.EDDB;
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
    public partial class EDDiscoveryForm : Form
    {
        static public  AutoCompleteStringCollection SystemNames = new AutoCompleteStringCollection();
        static public string CommanderName;

        const string filetgcSystems = "tgcsystems.json";
        const string filetgcdistances ="tgcdistances.json";
        public EDDiscoveryForm()
        {
            InitializeComponent();

        }


  

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Click once   System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVe‌​rsion
                var assemblyFullName = Assembly.GetExecutingAssembly().FullName;
                var version = assemblyFullName.Split(',')[1].Split('=')[1];
                Text = "EDDiscovery v" + version;
                EliteDangerous.CheckED();


                SystemData sdata = new SystemData();
                routeControl1.travelhistorycontrol1 = travelHistoryControl1;

                string datapath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Frontier_Developments\\Products"; // \\FORC-FDEV-D-1001\\Logs\\";

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
                    TravelHistoryControl.LogText("EliteDangerous is not logging station names!!! ", Color.Red);
                    TravelHistoryControl.LogText("Add ");
                    TravelHistoryControl.LogText("VerboseLogging=\"1\" ", Color.Blue);
                    TravelHistoryControl.LogText("to <Network  section in File: " + Path.Combine(EliteDangerous.EDDirectory, "AppConfig.xml") + Environment.NewLine);

                }

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
                GetRedWizzardFiles();
                GetEDSCSystems();
                GetEDSCDistancesAsync();
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

                travelHistoryControl1.RefreshHistory();
                travelHistoryControl1.netlog.StartMonitor();

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Form1_Load exception: " + ex.Message);
                System.Windows.Forms.MessageBox.Show("Trace: " + ex.StackTrace);
            }
        }


        private void GetRedWizzardFiles()
        {
            WebClient web = new WebClient();

            try
            {
                string webstr =  web.DownloadString("http://robert.astronet.se/Elite/ed-systems/eddisc.php");

                string[] filesizes = webstr.Split(' ');
                string remotetgcsystemslen = filesizes[0];
                string remotetgcdistanceslen = filesizes[1];

                GetNewRedWizzardFile(remotetgcsystemslen, filetgcSystems, "http://robert.astronet.se/Elite/ed-systems/tgcsystems.json");
                GetNewRedWizzardFile(remotetgcdistanceslen, filetgcdistances, "http://robert.astronet.se/Elite/ed-systems/tgcdistances.json");
            }
            catch (Exception ex)
            {
                TravelHistoryControl.LogText("GetRedWizzardFiles exception:" + ex.Message + Environment.NewLine);
                return;
            }
        }

        private static void GetNewRedWizzardFile(string remotetgcsystemslen, string filename, string url)
        {
            bool downloadfile = false;
            if (File.Exists(filename))
            {
                FileInfo fi = new FileInfo(filename);
                if (!fi.Length.ToString().Equals(remotetgcsystemslen))
                    downloadfile = true;
            }
            else
                downloadfile = true;

            if (downloadfile)
            {
                WebClient webclient = new WebClient();

                TravelHistoryControl.LogText("Downloading " + filename + " " +  (Convert.ToInt32(remotetgcsystemslen)/1000000.0).ToString("0.0") + "MB" + Environment.NewLine);
                webclient.DownloadFile(url, filename + ".tmp");

                if (File.Exists(filename))
                    File.Delete(filename);

                File.Copy(filename + ".tmp", filename);
            }
        }


        private void GetEDSCSystems()
        {
            try
            {
                SQLiteDBClass db = new SQLiteDBClass();
                EDSCClass edsc = new EDSCClass();

                string json;

                string rwsystime = db.GetSettingString("RWLastSystems", "2000-01-01 00:00:00"); // Latest time from RW file.
                string rwsysfiletime = "";

                CommanderName = db.GetSettingString("CommanderName", "");
                travelHistoryControl1.textBoxCmdrName.Text = CommanderName;


                json = LoadJsonArray(filetgcSystems);
                List<SystemClass> systems = SystemClass.ParseEDSC(json, ref rwsysfiletime);


                if (!rwsystime.Equals(rwsysfiletime))  // New distance file from Redwizzard
                {
                    SystemClass.Delete(SystemStatusEnum.EDSC); // Remove all EDSC systems.
                    
                    db.PutSettingString("RWLastSystems", rwsysfiletime);
                    db.PutSettingString("EDSCLastSystems", rwsysfiletime);
                    TravelHistoryControl.LogText("Adding data from tgcsystems.json " + Environment.NewLine);
                    SystemClass.Store(systems);
                }

                edsc.EDSCGetNewSystems(db);

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
                TravelHistoryControl.LogText("GetEDSCSystems exception:" + ex.Message + Environment.NewLine);
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

                json = LoadJsonArray("tgcdistances.json");
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
                    LogText("No responce from server." + Environment.NewLine);
                   
                else
                    LogText("Found " + dists.Count.ToString() + " new distances." + Environment.NewLine);
                    
                Application.DoEvents();
                DistanceClass.Store(dists);
                db.PutSettingString("EDSCLastDist", lstdist);
                db.GetAllDistances();
            }
            catch (Exception ex)
            {
                LogText("GetEDSCDistances exception:" + ex.Message + Environment.NewLine);
                
            }

        }

        private void  GetEDDBUpdate()
        {
           EDDBClass eddb = new EDDBClass();
            string timestr;
            DateTime time;

            Thread.Sleep(1000);

            SQLiteDBClass db = new SQLiteDBClass();
            timestr = db.GetSettingString("EDDBSystemsTime", "0");
            time = new DateTime(Convert.ToInt64(timestr), DateTimeKind.Utc);

            if (DateTime.UtcNow.Subtract(time).TotalDays > 1)
            {
                LogText("Get systems from EDDB. ");
                  
                if (eddb.GetSystems())
                {
                   LogText("OK." + Environment.NewLine);
                      
                    db.PutSettingString("EDDBSystemsTime", DateTime.UtcNow.Ticks.ToString());
                }
                else
                    LogText("Failed." + Environment.NewLine, Color.Red);
                    
            }


            timestr = db.GetSettingString("EDDBStationsLiteTime", "0");
            time = new DateTime(Convert.ToInt64(timestr), DateTimeKind.Utc);

            if (DateTime.UtcNow.Subtract(time).TotalDays > 1)
            {

                LogText("Get stations from EDDB. ");
                if (eddb.GetStationsLite())
                {
                    LogText("OK." + Environment.NewLine);
                    db.PutSettingString("EDDBStationsLiteTime", DateTime.UtcNow.Ticks.ToString());

                }
                else
                    LogText("Failed." + Environment.NewLine, Color.Red);

            }

            return ;
        }


        private void LogText(string text)
        {
            Invoke((MethodInvoker)delegate
                       {
                           TravelHistoryControl.LogText(text);

                       });
        }

        private void LogText(string text, Color col)
        {
            Invoke((MethodInvoker)delegate
            {
                TravelHistoryControl.LogText(text, col);

            });
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
            EDDiscovery2.Properties.Settings.Default.Netlogdir = textBoxNetLogDir.Text;
            EDDiscovery2.Properties.Settings.Default.NetlogDirAutoMode = radioButton_Auto.Checked;

            EDDiscovery2.Properties.Settings.Default.Save();

            tabControl1.SelectedTab = tabPage1;
            travelHistoryControl1.RefreshHistory();
        }

        private void routeControl1_Load(object sender, EventArgs e)
        {

        }

        private void EDDiscoveryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            travelHistoryControl1.netlog.StopMonitor();
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


            //            json = LoadJsonArray(filetgcSystems);
     

//            systems = SystemClass.ParseEDSC(json, ref date);

//            foreach (SystemClass system in systems)
//            {
//                srv.AddSystem(system);
//            }

            EDDBClass eddb = new EDDBClass();

            eddb.ReadSystems();
            eddb.ReadStations();


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





        //Pleiades Sector WU-O B16-0
        //Pleiades Sector WU-O b6-0

    }
}
