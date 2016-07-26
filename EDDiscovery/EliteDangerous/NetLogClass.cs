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
    public delegate void NetLogEventHandler(object source, int entry);

    public class NetLogFileInfo
    {
        public string FileName;
        public DateTime lastchanged;
        public long filePos, fileSize;
        public bool CQC;
    }

    public class NetLogClass
    {
        public event NetLogEventHandler OnNewPosition;

        public List<VisitedSystemsClass> visitedSystems = new List<VisitedSystemsClass>();
        Dictionary<string, NetLogFileInfo> netlogfiles = new Dictionary<string, NetLogFileInfo>();
        FileSystemWatcher m_Watcher;
        Thread ThreadNetLog;
        bool Exit = false;
        bool NoEvents = false;
        object EventLogLock = new object();
        AutoResetEvent NewLogEvent = new AutoResetEvent(false);
        ConcurrentQueue<string> NetLogFileQueue = new ConcurrentQueue<string>();
        private EDDiscoveryForm _discoveryform;
        public List<TravelLogUnit> tlUnits;

        public NetLogClass(EDDiscoveryForm ed )
        {
            _discoveryform = ed;
        }

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
                AppendText(richTextBox_History, "Netlog directory not found!" + Environment.NewLine + "Specify location in settings tab" + Environment.NewLine, _discoveryform.theme.TextBlockHighlightColor);
                return null;
            }

            if (!Directory.Exists(datapath))   // if logfiles directory is not found
            {
                AppendText(richTextBox_History, "Netlog directory is not present!" + Environment.NewLine + "Specify location in settings tab" + Environment.NewLine, _discoveryform.theme.TextBlockHighlightColor);
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
                {
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
                }

                FileInfo[] allFiles = dirInfo.GetFiles("netLog.*.log", SearchOption.AllDirectories).OrderBy(p => p.Name).ToArray();

                NoEvents = true;

                for (int i = 0; i < allFiles.Length; i++)
                {
                    FileInfo fi = allFiles[i];
                    TravelLogUnit lu = null;
                    bool parsefile = true;

                    if (fi.Name.Equals("netLog.1510280152.01.log"))
                        parsefile = true;

                    // Check if we alreade have parse the file and stored in DB.
                    if (tlUnits != null)
                        lu = (from c in tlUnits where c.Name == fi.Name select c).FirstOrDefault<TravelLogUnit>();

                    if (lu != null)
                    {
                        if (lu.Size == fi.Length && fi.Length != 0 && i < allFiles.Length - 1)  // File is already in DB:
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
                        AppendText(richTextBox_History, fi.Name + " " + nr.ToString() + " added to local database." + Environment.NewLine, _discoveryform.theme.TextBlockColor);
                    }
                }

                NoEvents = false;
            }

            // update the VSC with data from the db
            VisitedSystemsClass.UpdateSys(visitedSystems, EDDiscoveryForm.EDDConfig.UseDistances);  
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
                Console.WriteLine("Move pos on " + fi.FullName + " to " + nfi.filePos);
            }

            if (FirstLine != null)  // may be empty if we read it too fast.. don't worry, monitor will pick it up
            {
                string str = "20" + FirstLine.Substring(0, 8) + " " + FirstLine.Substring(9, 5);

                DateTime filetime = DateTime.Now.AddDays(-500);
                filetime = DateTime.Parse(str);

                long curpos = sr.BaseStream.Position;
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
                        Console.WriteLine("..at " + curpos + " Read " + line );
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
                            {
                                if (visitedSystems[visitedSystems.Count - 1].Name.Equals(ps.Name))
                                {
                                    Console.WriteLine("Repeat " + ps.Name);
                                    continue;
                                }
                            }

                            if (ps.Time.Subtract(gammastart).TotalMinutes > 0)  // Ta bara med efter gamma.
                            {
                                visitedSystems.Add(ps);
                                Console.WriteLine("..  Add " + ps.Name);
                                count++;
                            }
                        }

                    }

                    curpos = sr.BaseStream.Position;
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

            Console.WriteLine(" Finished " + fi.FullName + " at " + nfi.filePos);

            netlogfiles[nfi.FileName] = nfi;
            return count;
        }

        private void AppendText(ExtendedControls.RichTextBoxScroll box, string text, Color color)
        {
            box.AppendText(text, color);
            Application.DoEvents();
        }

        public void StartMonitor()
        {
            if (ThreadNetLog == null)
            {
                Exit = false;
                Console.WriteLine("Netthread start Monitor");

                ThreadNetLog = new System.Threading.Thread(new System.Threading.ThreadStart(NetLogMain));
                ThreadNetLog.Name = "Net log";
                ThreadNetLog.Start();
            }
        }

        public void StopMonitor()
        {
            if (ThreadNetLog != null)
            {
                Exit = true;
                NewLogEvent.Set();
                Console.WriteLine("Netthread stop Monitor");
                ThreadNetLog.Join();
                ThreadNetLog = null;
            }
        }

        private void EDDConfig_NetLogDirChanged()
        {
            if (ThreadNetLog != null)       // get this during close down, so only do it if we are running already.
            {
                StopMonitor();
                StartMonitor();
            }
        }

        private void NetLogMain()               // THREAD watching the files..
        {
            Console.WriteLine("Started netlogmain " + Thread.CurrentThread.ManagedThreadId);
            using (m_Watcher = new System.IO.FileSystemWatcher())
            {

                try
                {
                    string logpath = GetNetLogPath();
                    if (Directory.Exists(logpath))
                    {
                        m_Watcher.Path = logpath + Path.DirectorySeparatorChar;
                        m_Watcher.Filter = "netLog*.log";
                        m_Watcher.IncludeSubdirectories = false;
                        m_Watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime;
                        m_Watcher.Changed += new FileSystemEventHandler(OnChanged);
                        m_Watcher.Created += new FileSystemEventHandler(OnChanged);
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

                while (!Exit)
                {
                    try
                    {
                        NewLogEvent.WaitOne(2000);

                        if (Exit)
                        {
                            Console.WriteLine("Closing netlogmain " + Thread.CurrentThread.ManagedThreadId);
                            return;
                        }

                        EliteDangerous.CheckED();

                        lock (EventLogLock)
                        {
                            string filename = null;

                            if (NoEvents == false && NetLogFileQueue.TryDequeue(out filename) )
                            {
                                FileInfo fi = new FileInfo(filename);

                                Console.WriteLine("file " + filename + " len " + fi.Length );

                                if (tlUnit == null || !tlUnit.Name.Equals(Path.GetFileName(filename)))  // Create / find new travellog unit
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
                                    Console.WriteLine("Parsing changed system count, from " + nrsystems + " to " + visitedSystems.Count);

                                    for (int nr = nrsystems; nr < visitedSystems.Count; nr++)  // Lägg till nya i locala databaslogen
                                    {
                                        VisitedSystemsClass dbsys = visitedSystems[nr];
                                        dbsys.Source = tlUnit.id;
                                        dbsys.EDSM_sync = false;
                                        dbsys.MapColour = EDDConfig.Instance.DefaultMapColour;
                                        dbsys.Unit = fi.Name;
                                        dbsys.Commander = EDDConfig.Instance.CurrentCmdrID;
                                        dbsys.Add();

                                        // here we need to make sure the cursystem is set up.. need to do it here because OnNewPosition expects all cursystems to be non null..

                                        VisitedSystemsClass item = visitedSystems[nr];
                                        VisitedSystemsClass item2 = (nr > 0) ? visitedSystems[nr - 1] : null;
                                        VisitedSystemsClass.UpdateVisitedSystemsEntries(item, item2, EDDiscoveryForm.EDDConfig.UseDistances);       // ensure they have system classes behind them..
                                    }

                                    // now the visiting class has been set up, now tell the display of these new systems
                                    for (int nr = nrsystems; nr < visitedSystems.Count; nr++)  // Lägg till nya i locala databaslogen
                                    {
                                        OnNewPosition(this, nr);    // add record nr to the list
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No systems added");
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

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            string filename = e.FullPath;

            m_Watcher.EnableRaisingEvents = false;

            System.Diagnostics.Trace.WriteLine("Log Watcher " + e.ChangeType + " " + filename);
            NetLogFileQueue.Enqueue(filename);
            NewLogEvent.Set();

            m_Watcher.EnableRaisingEvents = true;
        }
    }
}
