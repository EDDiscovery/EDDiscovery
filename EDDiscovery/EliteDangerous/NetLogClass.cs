using EDDiscovery.DB;
using EDDiscovery2;
using EDDiscovery2.DB;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
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
        public List<VisitedSystemsClass> visitedSystems = new List<VisitedSystemsClass>();
        Dictionary<string, NetLogFileInfo> netlogfiles = new Dictionary<string, NetLogFileInfo>();
        FileSystemWatcher m_Watcher;
        Thread ThreadNetLog;
        bool Exit = false;
        bool NoEvents = false;
        object EventLogLock = new object();
        AutoResetEvent NewLogEvent = new AutoResetEvent(false);
        ConcurrentQueue<NetLogFileInfo> NetLogFileQueue = new ConcurrentQueue<NetLogFileInfo>();
        public event NetLogEventHandler OnNewPosition;

        public List<TravelLogUnit> tlUnits;

        public string GetNetLogPath()
        {
            try
            {
                string netlogdirstored = EDDConfig.Instance.NetLogDir;
                string datapath = null;
                if (EDDConfig.Instance.NetLogDirAutoMode)
                {
                    if (EliteDangerous.EDDirectory != null && EliteDangerous.EDDirectory.Length > 0)
                    {
                        datapath = Path.Combine(EliteDangerous.EDDirectory, "Logs");
                        if (!netlogdirstored.Equals(datapath))
                            EDDConfig.Instance.NetLogDir = datapath;
                        return datapath;
                    }

                    datapath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Frontier_Developments", "Products"); // \\FORC-FDEV-D-1001\\Logs\\";

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
                        EDDConfig.Instance.NetLogDir = newfi.DirectoryName;
                        EDDConfig.Instance.NetLogDirAutoMode = false;
                        datapath = newfi.DirectoryName;
                    }
                }
                else
                {
                    datapath = EDDConfig.Instance.NetLogDir;
                    string cmdrpath = EDDConfig.Instance.CurrentCommander.NetLogPath;
                    if (cmdrpath != null && cmdrpath != "" && Directory.Exists(cmdrpath))
                    {
                        datapath = cmdrpath;
                    }
                }

                return datapath;
            }
            catch (Exception ex)
            {
                MessageBox.Show("GetNetLogPath exception: " + ex.Message);
                return null;
            }
        }

        // called during start up and if refresh history is pressed.. 

        public List<VisitedSystemsClass> ParseFiles(ExtendedControls.RichTextBoxScroll richTextBox_History, int defaultMapColour)
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

            List<VisitedSystemsClass> vsSystemsList = VisitedSystemsClass.GetAll(EDDConfig.Instance.CurrentCmdrID);

            lock (EventLogLock)
            {
                visitedSystems.Clear();
                // Add systems in local DB.
                if (vsSystemsList != null)
                    foreach (VisitedSystemsClass vs in vsSystemsList)
                    {
                        if (visitedSystems.Count == 0)
                            visitedSystems.Add(vs);
                        else if (!visitedSystems.Last<VisitedSystemsClass>().Name.Equals(vs.Name))  // Avoid duplicate if times exist in same system from different files.
                            visitedSystems.Add(vs);
                        else
                        {
                            VisitedSystemsClass vs2 = (VisitedSystemsClass)visitedSystems.Last<VisitedSystemsClass>();
                            if (vs2.id != vs.id)
                            {
                                vs.Commander = -2; // Move to dupe user
                                vs.Update();
                            }
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
                    if (tlUnits != null)
                        lu = (from c in tlUnits where c.Name == fi.Name select c).FirstOrDefault<TravelLogUnit>();

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
                        List<VisitedSystemsClass> tempVisitedSystems = new List<VisitedSystemsClass>();
                        ParseFile(fi, tempVisitedSystems);


                        foreach (VisitedSystemsClass ps in tempVisitedSystems)
                        {
                            VisitedSystemsClass ps2;
                            ps2 = (from c in visitedSystems where c.Name == ps.Name && c.Time == ps.Time select c).FirstOrDefault<VisitedSystemsClass>();
                            if (ps2 == null)
                            {
                                //VisitedSystemsClass dbsys = new VisitedSystemsClass();

                                ps.Source = lu.id;
                                ps.EDSM_sync = false;
                                ps.Unit = fi.Name;
                                ps.MapColour = defaultMapColour;
                                ps.Commander = EDDConfig.Instance.CurrentCmdrID;

                                //if (!lu.Beta)  // dont store  history in DB for beta (YET)
                                {
                                    VisitedSystemsClass last = VisitedSystemsClass.GetLast();

                                    if (last == null || !last.Name.Equals(ps.Name))  // If same name as last system. Dont Add.  otherwise we get a duplet with last from logfile before with different time. 
                                    {
                                        if (!VisitedSystemsClass.Exist(ps.Name, ps.Time))
                                        {
                                            ps.Add();
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

                ReloadMonitor();

                NoEvents = false;
            }

            //var result = visitedSystems.OrderByDescending(a => a.time).ToList<VisitedSystemsClass>();

            return visitedSystems;
        }


        private int ParseFile(FileInfo fi, List<VisitedSystemsClass> visitedSystems)
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
            catch( Exception ex )
            {
                System.Diagnostics.Trace.WriteLine("Parse File exception: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
                return 0;
            }

            return count;
        }

        private NetLogFileInfo lastnfi = null;
        private int ReadData(FileInfo fi, List<VisitedSystemsClass> visitedSystems, int count, StreamReader sr)
        {
            DateTime gammastart = new DateTime(2014, 11, 22, 13, 00, 00);

            NetLogFileInfo nfi = null;
            bool CQC = false;

            string FirstLine = sr.ReadLine();                  // read first line from file, may be null if file has not been written to yet

            if (netlogfiles.ContainsKey(fi.FullName))
            {
                nfi = netlogfiles[fi.FullName];
                sr.BaseStream.Position = nfi.filePos;
                sr.DiscardBufferedData();
                CQC = nfi.CQC;
            }
            
            if (FirstLine != null)  // may be empty if we read it too fast.. don't worry, monitor will pick it up
            {
                string str = "20" + FirstLine.Substring(0, 8) + " " + FirstLine.Substring(9, 5);

                DateTime filetime = DateTime.Now.AddDays(-500);
                filetime = DateTime.Parse(str);

                string line;
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

                        VisitedSystemsClass ps = VisitedSystemsClass.Parse(filetime, line);
                        if (ps != null)
                        {   // Remove some training systems
                            if (ps.Name.Equals("Training"))
                                continue;
                            if (ps.Name.Equals("Destination"))
                                continue;
                            if (ps.Name.Equals("Altiris"))
                                continue;
                            filetime = ps.Time;

                            if (visitedSystems.Count > 0)
                                if (visitedSystems[visitedSystems.Count - 1].Name.Equals(ps.Name))
                                    continue;

                            if (ps.Time.Subtract(gammastart).TotalMinutes > 0)  // Ta bara med efter gamma. 
                            {


                                visitedSystems.Add(ps);
                                count++;

                                //System.Diagnostics.Trace.WriteLine("Added system: " + ps.Name);
                            }

                            //Console.WriteLine(line);
                        }
                    }
                }
            }
            else
            {
                System.Diagnostics.Trace.WriteLine("File was empty (for now) " + fi.FullName);
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


        private void AppendText(ExtendedControls.RichTextBoxScroll box, string text, Color color)
        {
            box.AppendText(text, color);
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
            NewLogEvent.Set();
            ThreadNetLog.Join();
        }

        public void ReloadMonitor()
        {
            lock (EventLogLock)
            {
                string logdir = GetNetLogPath();
                if (m_Watcher != null && m_Watcher.Path != logdir + Path.DirectorySeparatorChar && Directory.Exists(logdir))
                {
                    try
                    {
                        m_Watcher.EnableRaisingEvents = false;
                        m_Watcher.Path = logdir + Path.DirectorySeparatorChar;
                        m_Watcher.EnableRaisingEvents = true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine("Exception while updating netlog path: " + ex.Message);
                        System.Diagnostics.Trace.WriteLine(ex.StackTrace);
                    }
                }
            }
        }

        private void NetLogMain()               // THREAD watching the files..
        {
            using (m_Watcher = new System.IO.FileSystemWatcher())
            {

                try
                {
                    string logpath = GetNetLogPath();
                    if (Directory.Exists(logpath))
                    {
                        m_Watcher.Path = logpath + Path.DirectorySeparatorChar;
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

                EDDConfig.Instance.NetLogDirChanged += EDDConfig_NetLogDirChanged;

                List<TravelLogUnit> travelogUnits;
                // Get TravelLogUnits;
                travelogUnits = null;
                TravelLogUnit tlUnit = null;

                int ii = 0;


                while (!Exit)
                {
                    try
                    {
                        ii++;
                        //Thread.Sleep(2000);
                        NewLogEvent.WaitOne(2000);

                        if (Exit)
                        {
                            return;
                        }

                        EliteDangerous.CheckED();

                        lock (EventLogLock)
                        {
                            if (NoEvents == false)
                            {
                                NetLogFileInfo nfi;
                                if (!NetLogFileQueue.TryDequeue(out nfi))
                                {
                                    nfi = lastnfi;
                                }

                                if (nfi != null)
                                {
                                    FileInfo fi = new FileInfo(nfi.FileName);

                                    if (fi.Length != nfi.fileSize || ii % 5 == 0)
                                    {
                                        if (tlUnit == null || !tlUnit.Name.Equals(Path.GetFileName(nfi.FileName)))  // Create / find new travellog unit
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
                                                VisitedSystemsClass dbsys = visitedSystems[nr];
                                                dbsys.Source = tlUnit.id;
                                                dbsys.EDSM_sync = false;
                                                dbsys.MapColour = EDDConfig.Instance.DefaultMapColour;
                                                dbsys.Unit = fi.Name;
                                                dbsys.Commander = EDDConfig.Instance.CurrentCmdrID;

                                                dbsys.Add();
                                            }

                                            OnNewPosition(this);    // NoEvents= false
                                        }
                                        else
                                        {
                                            //System.Diagnostics.Trace.WriteLine("No change");
                                        }
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

            m_Watcher = null;
        }

        private void EDDConfig_NetLogDirChanged()
        {
            ReloadMonitor();
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

                lock (EventLogLock)
                {
                    ParseFile(new FileInfo(filename), visitedSystems);
                    NetLogFileQueue.Enqueue(lastnfi);
                }

                NewLogEvent.Set();
            }
            finally
            {
                m_Watcher.EnableRaisingEvents = true;

            }
        }


    }
}
