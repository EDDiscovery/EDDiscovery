using EDDiscovery.DB;
using EDDiscovery2;
using EDDiscovery2.DB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace EDDiscovery
{
    public delegate void NetLogEventHandler(object source);

    public class NetLogFileInfo
    {
        public string FileName;
        public DateTime lastchanged;
        public long filePos, fileSize;
        public bool CQC;

    }

    public class NetLogClass
    {
        public List<SystemPosition> visitedSystems = new List<SystemPosition>();
        Dictionary<string, NetLogFileInfo> netlogfiles = new Dictionary<string, NetLogFileInfo>();
        FileSystemWatcher m_Watcher;
        Thread ThreadNetLog;
        bool Exit = false;
        bool NoEvents = false;
        public event NetLogEventHandler OnNewPosition;

        SQLiteDBClass db=null;
        public List<TravelLogUnit> tlUnits;

        public string GetNetLogPath()
        {
            try
            {
                if (db == null)
                    db = new SQLiteDBClass();

                string netlogdirstored = db.GetSettingString("Netlogdir", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Frontier_Developments\\Products");
                string datapath = null;
                if (db.GetSettingBool("NetlogDirAutoMode", true))
                {
                    if (EliteDangerous.EDDirectory != null && EliteDangerous.EDDirectory.Length > 0)
                    {
                        datapath = Path.Combine(EliteDangerous.EDDirectory, "Logs");
                        if (!netlogdirstored.Equals(datapath))
                            db.PutSettingString("Netlogdir", datapath);
                        return datapath;
                    }

                    datapath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Frontier_Developments\\Products"; // \\FORC-FDEV-D-1001\\Logs\\";

                    // Find the right subdirectory....

                    DirectoryInfo dirInfo = new DirectoryInfo(datapath);
                    FileInfo[] allFiles = null;

                    try
                    {
                        allFiles = dirInfo.GetFiles("netLog*.log", SearchOption.AllDirectories);
                    }
                    catch
                    {
                    }

                    if (allFiles == null)
                    {
                        return null;
                    }

                    DateTime newtime = new DateTime(2000, 10, 10);
                    FileInfo newfi = null;


                    foreach (FileInfo fi in allFiles)
                    {
                        if (fi.CreationTimeUtc > newtime)
                        {
                            newtime = fi.CreationTimeUtc;
                            newfi = fi;
                        }
                    }

                    if (newfi != null)
                    {
                        db.PutSettingString("Netlogdir" , newfi.DirectoryName);
                        db.PutSettingBool("NetlogDirAutoMode" , false);
                        datapath = newfi.DirectoryName;
                    }



                }
                else
                {
                    datapath = db.GetSettingString("Netlogdir", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Frontier_Developments\\Products");
                }

                return datapath;
            }
            catch (Exception ex)
            {
                MessageBox.Show("GetNetLogPath exception: " + ex.Message);
                return null;
            }
        }



        public List<SystemPosition> ParseFiles(RichTextBox richTextBox_History, int defaultMapColour, int commander)
        {
            string datapath;
            DirectoryInfo dirInfo;

            datapath = GetNetLogPath();

            if (datapath == null)
            {
                AppendText(richTextBox_History, "Netlog directory not found!" + Environment.NewLine + "Specify location in settings tab" + Environment.NewLine, Color.Red);
                return null;
            }


            if (!Directory.Exists(datapath))   // if logfiles directory is not found
            {
                if (richTextBox_History != null)
                {
                    richTextBox_History.Clear();
                    AppendText(richTextBox_History, "Netlog directory not found!" + Environment.NewLine + "Specify location in settings tab" + Environment.NewLine, Color.Red);
                    //MessageBox.Show("Netlog directory not found!" + Environment.NewLine + "Specify location in settings tab", "EDDiscovery Error", MessageBoxButtons.OK);
                }
                return null;
            }
            try
            {
                dirInfo = new DirectoryInfo(datapath);
            }
            catch (Exception ex)
            {
                AppendText(richTextBox_History, "Could not create Directory info: " + ex.Message + Environment.NewLine, Color.Red);
                return null;
            }

            // Get TravelLogUnits;

            tlUnits =  TravelLogUnit.GetAll();

            List<VisitedSystemsClass> vsSystemsList = VisitedSystemsClass.GetAll(commander);

            visitedSystems.Clear();
            // Add systems in local DB.
            if (vsSystemsList != null)
                foreach (VisitedSystemsClass vs in vsSystemsList)
                {
                    if (visitedSystems.Count == 0)
                        visitedSystems.Add(new SystemPosition(vs));
                    else if (!visitedSystems.Last<SystemPosition>().Name.Equals(vs.Name))  // Avoid duplicate if times exist in same system from different files.
                        visitedSystems.Add(new SystemPosition(vs));
                    else
                    {
                        VisitedSystemsClass vs2 = (VisitedSystemsClass)visitedSystems.Last<SystemPosition>().vs;
                        vs.Commander = -2; // Move to dupe user
                        vs.Update();
                    }

                }

            FileInfo[] allFiles = dirInfo.GetFiles("netLog.*.log", SearchOption.AllDirectories).OrderBy(p => p.Name).ToArray();

            NoEvents = true;

            foreach (FileInfo fi in allFiles)
            {
                TravelLogUnit lu = null;
                bool parsefile = true;

                if (fi.Name.Equals("netLog.1510280152.01.log"))
                    parsefile = true;

                // Check if we alreade have parse the file and stored in DB.
                if (tlUnits!=null)
                    lu= (from c in tlUnits where c.Name == fi.Name select c).FirstOrDefault<TravelLogUnit>();

                if (lu != null)
                {
                    if (lu.Size == fi.Length)  // File is already in DB:
                        parsefile = false;
                }
                else
                {
                    lu = new TravelLogUnit();
                    lu.Name = fi.Name;
                    lu.Path = Path.GetDirectoryName(fi.FullName);
                    lu.Size = 0;  // Add real size after data is in DB //;(int)fi.Length;
                    lu.type = 1;
                    lu.Add();
                }


                if (parsefile)
                {
                    int nr = 0;
                    List<SystemPosition> tempVisitedSystems = new List<SystemPosition>();
                    ParseFile(fi, tempVisitedSystems);


                    foreach (SystemPosition ps in tempVisitedSystems)
                    {
                        SystemPosition ps2;
                        ps2 = (from c in visitedSystems where c.Name == ps.Name && c.time == ps.time select c).FirstOrDefault<SystemPosition>();
                        if (ps2 == null)
                        {
                            VisitedSystemsClass dbsys = new VisitedSystemsClass();

                            dbsys.Name = ps.Name;
                            dbsys.Time = ps.time;
                            dbsys.Source = lu.id;
                            dbsys.EDSM_sync = false;
                            dbsys.Unit = fi.Name;
                            dbsys.MapColour = defaultMapColour;

                            if (!lu.Beta)  // dont store  history in DB for beta (YET)
                            {
                                VisitedSystemsClass last = VisitedSystemsClass.GetLast();

                                if (last == null || !last.Name.Equals(dbsys.Name))  // If same name as last system. Dont Add.  otherwise we get a duplet with last from logfile before with different time. 
                                {
                                    if (!VisitedSystemsClass.Exist(dbsys.Name, dbsys.Time))
                                    {
                                        dbsys.Add();
                                        visitedSystems.Add(ps);
                                        nr++;
                                    }
                                }
                            }
                            
                        }


                    
                    }

                    lu.Size = (int)fi.Length;
                    lu.Update();
                    AppendText(richTextBox_History, fi.Name + " " + nr.ToString() + " added to local database." + Environment.NewLine, Color.Black);
                }
            }
            NoEvents = false;

            //var result = visitedSystems.OrderByDescending(a => a.time).ToList<SystemPosition>();

            return visitedSystems;
        }


        private int ParseFile(FileInfo fi, List<SystemPosition> visitedSystems)
        {

            int count = 0, nrsystems=visitedSystems.Count;
            try
            {
                using (Stream fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        count = ReadData(fi, visitedSystems, count, sr);
                    }
                }
            }
            catch
            {
                return 0;
            }

            if (nrsystems < visitedSystems.Count)
            {
                if (!NoEvents)
                    OnNewPosition(this);
            }
            return count;
        }

        private NetLogFileInfo lastnfi = null;
        private int ReadData(FileInfo fi, List<SystemPosition> visitedSystems, int count, StreamReader sr)
        {
            DateTime gammastart = new DateTime(2014, 11, 22, 13, 00, 00);

            DateTime filetime = DateTime.Now.AddDays(-500);
            string FirstLine = sr.ReadLine();
            string line, str;
            NetLogFileInfo nfi = null;
            bool CQC = false;

            str = "20" + FirstLine.Substring(0, 8) + " " + FirstLine.Substring(9, 5);

            filetime = DateTime.Parse(str);

            if (netlogfiles.ContainsKey(fi.FullName))
            {
                nfi = netlogfiles[fi.FullName];
                sr.BaseStream.Position = nfi.filePos;
                sr.DiscardBufferedData();
                CQC = nfi.CQC;
            }

            while ((line = sr.ReadLine()) != null)
            {
                if (line.Contains("[PG] [Notification] Left a playlist lobby"))
                    CQC = false;

                if (line.Contains("[PG] Destroying playlist lobby."))
                    CQC = false;

                if (line.Contains("[PG] [Notification] Joined a playlist lobby"))
                    CQC = true;
                if (line.Contains("[PG] Created playlist lobby"))
                    CQC = true;
                if (line.Contains("[PG] Found matchmaking lobby object"))
                    CQC = true;

                if (line.Contains(" System:") && CQC == false)
                {
                    if (line.Contains("ProvingGround"))
                        continue;

                    SystemPosition ps = SystemPosition.Parse(filetime, line);
                    if (ps != null)
                    {   // Remove some training systems
                        if (ps.Name.Equals("Training"))
                            continue;
                        if (ps.Name.Equals("Destination"))
                            continue;
                        if (ps.Name.Equals("Altiris"))
                            continue;
                        filetime = ps.time;

                        if (visitedSystems.Count > 0)
                            if (visitedSystems[visitedSystems.Count - 1].Name.Equals(ps.Name))
                                continue;

                        if (ps.time.Subtract(gammastart).TotalMinutes > 0)  // Ta bara med efter gamma. 
                        {

                            
                            visitedSystems.Add(ps);
                            count++;

                            //System.Diagnostics.Trace.WriteLine("Added system: " + ps.Name);
                        }

                        //Console.WriteLine(line);
                    }
                }
            }


            if (nfi ==null)
                nfi = new NetLogFileInfo();

            nfi.FileName = fi.FullName;
            nfi.lastchanged = File.GetLastWriteTimeUtc(nfi.FileName);
            nfi.filePos = sr.BaseStream.Position;
            nfi.fileSize = fi.Length;
            nfi.CQC = CQC;

            netlogfiles[nfi.FileName] = nfi;
            lastnfi = nfi;

            return count;
        }


        private void AppendText(RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
            box.ScrollToCaret();
            box.Invalidate();
            Application.DoEvents();

        }

        private EDDiscoveryForm _discoveryform;

        public bool StartMonitor(EDDiscoveryForm ed)
        {
            _discoveryform = ed;
            ThreadNetLog = new System.Threading.Thread(new System.Threading.ThreadStart(NetLogMain));
            ThreadNetLog.Name = "Net log";
            ThreadNetLog.Start();

            return true;
        }


        public void StopMonitor()
        {
            Exit = true;
        }


        private void NetLogMain()
        {
            try
            {
                m_Watcher = new System.IO.FileSystemWatcher();

                if (Directory.Exists(GetNetLogPath()))
                {
                    m_Watcher.Path = GetNetLogPath() + "\\";
                    m_Watcher.Filter = "netLog*.log";
                    m_Watcher.IncludeSubdirectories = true;
                    m_Watcher.NotifyFilter = NotifyFilters.FileName; // | NotifyFilters.Size; 

                    m_Watcher.Changed += new FileSystemEventHandler(OnChanged);
                    m_Watcher.Created += new FileSystemEventHandler(OnChanged);
                    m_Watcher.Deleted += new FileSystemEventHandler(OnChanged);
                    m_Watcher.EnableRaisingEvents = true;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Net log watcher exception : " + ex.Message, "EDDiscovery Error");
                System.Diagnostics.Trace.WriteLine("NetlogMAin exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);

            }

            List<TravelLogUnit> travelogUnits;
            // Get TravelLogUnits;
            travelogUnits = null;
            TravelLogUnit  tlUnit=null;
            SQLiteDBClass db = new SQLiteDBClass();

            int ii =0;


            while (!Exit)
            {
                try
                {


                    ii++;
                    Thread.Sleep(2000);

                    EliteDangerous.CheckED();

                    if (NoEvents == false)
                    {
                        if (lastnfi != null)
                        {
                            FileInfo fi = new FileInfo(lastnfi.FileName);

                            if (fi.Length != lastnfi.fileSize || ii % 5 == 0)
                            {
                                if (tlUnit == null || !tlUnit.Name.Equals(Path.GetFileName(lastnfi.FileName)))  // Create / find new travellog unit
                                {
                                    travelogUnits = TravelLogUnit.GetAll();
                                    // Check if we alreade have parse the file and stored in DB.
                                    if (tlUnit == null)
                                        tlUnit = (from c in travelogUnits where c.Name == fi.Name select c).FirstOrDefault<TravelLogUnit>();

                                    if (tlUnit == null)
                                    {
                                        tlUnit = new TravelLogUnit();
                                        tlUnit.Name = fi.Name;
                                        tlUnit.Path = Path.GetDirectoryName(fi.FullName);
                                        tlUnit.Size = 0;  // Add real size after data is in DB //;(int)fi.Length;
                                        tlUnit.type = 1;
                                        tlUnit.Add();
                                        travelogUnits.Add(tlUnit);
                                    }
                                }


                                int nrsystems = visitedSystems.Count;
                                ParseFile(fi, visitedSystems);
                                if (nrsystems < visitedSystems.Count) // Om vi har fler system
                                {
                                    System.Diagnostics.Trace.WriteLine("New systems " + nrsystems.ToString() + ":" + visitedSystems.Count.ToString());
                                    for (int nr = nrsystems; nr < visitedSystems.Count; nr++)  // Lägg till nya i locala databaslogen
                                    {
                                        VisitedSystemsClass dbsys = new VisitedSystemsClass();

                                        dbsys.Name = visitedSystems[nr].Name;
                                        dbsys.Time = visitedSystems[nr].time;
                                        dbsys.Source = tlUnit.id;
                                        dbsys.EDSM_sync = false;
                                        dbsys.Unit = fi.Name;
                                        dbsys.MapColour = db.GetSettingInt("DefaultMap", Color.Red.ToArgb());
                                        dbsys.Unit = fi.Name;
                                        
                                        if (!tlUnit.Beta)  // dont store  history in DB for beta (YET)
                                        {
                                            dbsys.Add();
                                        }
                                        visitedSystems[nr].vs = dbsys;
                                    }
                                }
                                else
                                {
                                    //System.Diagnostics.Trace.WriteLine("No change");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("NetlogMAin exception : " + ex.Message);
                    System.Diagnostics.Trace.WriteLine(ex.StackTrace);
                }

        }
        }


        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            string filename = e.FullPath;

            try
            {
                m_Watcher.EnableRaisingEvents = false;


                if (!netlogfiles.ContainsKey(filename))
                {
                    System.Diagnostics.Trace.WriteLine("NEW FILE !!!" + filename);
                }
                else
                {
                    System.Diagnostics.Trace.WriteLine("A CHANGE has occured with " + filename);
                }

                ParseFile(new FileInfo(filename), visitedSystems);

            }
            finally
            {
                m_Watcher.EnableRaisingEvents = true;

            }
        }


    }
}
