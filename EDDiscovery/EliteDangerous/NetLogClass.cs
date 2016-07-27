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
using System.Diagnostics;

namespace EDDiscovery
{
    public class NetLogFileInfo
    {
        public string FileName;
        public DateTime lastchanged;
        public long filePos, fileSize;
        public bool CQC;
    }

    public class NetLogClass
    {
        public delegate void NetLogEventHandler(VisitedSystemsClass vsc);

        public event NetLogEventHandler OnNewPosition;          // called in foreground, no need for invoke

        public List<VisitedSystemsClass> visitedSystems = new List<VisitedSystemsClass>();

        Dictionary<string, NetLogFileInfo> netlogfiles = new Dictionary<string, NetLogFileInfo>();

        FileSystemWatcher m_Watcher;
        ConcurrentQueue<NetLogFileInfo> m_netLogFileQueue;
        System.Windows.Forms.Timer m_scantimer;
        List<TravelLogUnit> m_travelogUnits;

        public List<TravelLogUnit> tlUnits;

        NetLogFileInfo lastnfi = null;          // last one read..

        public NetLogClass(EDDiscoveryForm ds)
        {
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

        // called during start up and if refresh history is pressed..  foreground call

        public List<VisitedSystemsClass> ParseFiles(out string error, int defaultMapColour)
        {
            StopMonitor();          // this is called by the foreground.  Ensure background is stopped.  Foreground must restart it.

            error = null;
            DirectoryInfo dirInfo;

            string datapath = GetNetLogPath();

            if (datapath == null)
            {
                error = "Netlog directory not found!" + Environment.NewLine + "Specify location in settings tab";
                return null;
            }

            if (!Directory.Exists(datapath))   // if logfiles directory is not found
            {
                error = "Netlog directory is not present!" + Environment.NewLine + "Specify location in settings tab";
                return null;
            }

            try
            {
                dirInfo = new DirectoryInfo(datapath);
            }
            catch (Exception ex)
            {
                error = "Could not create Directory info: " + ex.Message;
                return null;
            }

            tlUnits = TravelLogUnit.GetAll();

            List<VisitedSystemsClass> vsSystemsList = VisitedSystemsClass.GetAll(EDDConfig.Instance.CurrentCmdrID);

            visitedSystems.Clear();

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
            // order by file write time so we end up on the last one written
            FileInfo[] allFiles = dirInfo.GetFiles("netLog.*.log", SearchOption.AllDirectories).OrderBy(p => p.LastWriteTime).ToArray();

            for (int i = 0; i < allFiles.Length; i++)
            {
                FileInfo fi = allFiles[i];
                TravelLogUnit lu = null;
                bool parsefile = true;

                // Check if we alreade have parse the file and stored in DB.
                if (tlUnits != null)
                    lu = (from c in tlUnits where c.Name == fi.Name select c).FirstOrDefault<TravelLogUnit>();

                if (lu != null)
                {
                    if (lu.Size == fi.Length && fi.Length != 0 && i < allFiles.Length - 1)  // File is already in DB, and NOT the last one
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
                    List<VisitedSystemsClass> tempVisitedSystems = new List<VisitedSystemsClass>();

                    lastnfi = ParseFile(fi, tempVisitedSystems);      // we always end up scanning the whole thing since we don't have netlogfiles on first go..

                    foreach (VisitedSystemsClass ps in tempVisitedSystems)
                    {
                        VisitedSystemsClass ps2 = (from c in visitedSystems where c.Name == ps.Name && c.Time == ps.Time select c).FirstOrDefault<VisitedSystemsClass>();

                        if (ps2 == null)
                        {
                            ps.Source = lu.id;
                            ps.EDSM_sync = false;
                            ps.Unit = fi.Name;
                            ps.MapColour = defaultMapColour;
                            ps.Commander = EDDConfig.Instance.CurrentCmdrID;

                            VisitedSystemsClass last = VisitedSystemsClass.GetLast();

                            if (last == null || !last.Name.Equals(ps.Name))  // If same name as last system. Dont Add.  otherwise we get a duplet with last from logfile before with different time.
                            {
                                if (!VisitedSystemsClass.Exist(ps.Name, ps.Time))
                                {
                                    ps.Add();
                                    visitedSystems.Add(ps);
                                }
                            }
                        }
                    }

                    lu.Size = (int)fi.Length;
                    lu.Update();
                }
            }

            // update the VSC with data from the db
            VisitedSystemsClass.UpdateSys(visitedSystems, EDDiscoveryForm.EDDConfig.UseDistances);
            return visitedSystems;
        }

        private NetLogFileInfo ParseFile(FileInfo fi, List<VisitedSystemsClass> visitedSystems)
        {
            NetLogFileInfo ret = null;

            try
            {
                using (Stream fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        ret = ReadData(fi, visitedSystems, sr);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Parse File exception: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }

            return ret;
        }
        // returns info on what its read..
        private NetLogFileInfo ReadData(FileInfo fi, List<VisitedSystemsClass> visitedSystems, StreamReader sr)
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

            long startpos = sr.BaseStream.Position;

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
                        //Console.WriteLine(" RD:" + line );
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
                                    //Console.WriteLine("Repeat " + ps.Name);
                                    continue;
                                }
                            }

                            if (ps.Time.Subtract(gammastart).TotalMinutes > 0)  // Ta bara med efter gamma.
                            {
                                visitedSystems.Add(ps);
                                //Console.WriteLine("Add System " + ps.Name);
                            }
                        }

                    }
                }
            }
            else
            {
                System.Diagnostics.Trace.WriteLine("File was empty (for now) " + fi.FullName);
            }

            if (nfi == null)
                nfi = new NetLogFileInfo();

            nfi.FileName = fi.FullName;
            nfi.lastchanged = File.GetLastWriteTimeUtc(nfi.FileName);
            nfi.filePos = sr.BaseStream.Position;
            nfi.fileSize = fi.Length;
            nfi.CQC = CQC;

            Console.WriteLine("Parse ReadData " + fi.FullName + " from " + startpos + " to " + nfi.filePos);

            netlogfiles[nfi.FileName] = nfi;

            return nfi;
        }

        public void StartMonitor()
        {
            if (m_Watcher == null)
            {
                try
                {
                    string logpath = GetNetLogPath();

                    if (Directory.Exists(logpath))
                    {
                        m_netLogFileQueue = new ConcurrentQueue<NetLogFileInfo>();
                        m_Watcher = new System.IO.FileSystemWatcher();
                        m_Watcher.Path = logpath + Path.DirectorySeparatorChar;
                        m_Watcher.Filter = "netLog*.log";
                        m_Watcher.IncludeSubdirectories = false;
                        m_Watcher.NotifyFilter = NotifyFilters.FileName;
                        m_Watcher.Changed += new FileSystemEventHandler(OnNewFile);
                        m_Watcher.Created += new FileSystemEventHandler(OnNewFile);
                        m_Watcher.EnableRaisingEvents = true;

                        EDDConfig.Instance.NetLogDirChanged += EDDConfig_NetLogDirChanged;

                        m_travelogUnits = TravelLogUnit.GetAll();
                        m_scantimer = new System.Windows.Forms.Timer();
                        m_scantimer.Interval = 2000;
                        m_scantimer.Tick += ScanTick;
                        m_scantimer.Start();

                        Console.WriteLine("Start Monitor");
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Start Monitor exception : " + ex.Message, "EDDiscovery Error");
                    System.Diagnostics.Trace.WriteLine("Start Monitor exception : " + ex.Message);
                    System.Diagnostics.Trace.WriteLine(ex.StackTrace);
                }
            }
        }

        public void StopMonitor()
        {
            if (m_Watcher != null)
            {
                EDDConfig.Instance.NetLogDirChanged -= EDDConfig_NetLogDirChanged;

                m_scantimer.Stop();
                m_scantimer = null;
                m_Watcher.EnableRaisingEvents = false;
                m_Watcher.Dispose();
                m_Watcher = null;
                m_netLogFileQueue = null;
                m_travelogUnits = null;

                Console.WriteLine("Stop Monitor");
            }
        }

        private void EDDConfig_NetLogDirChanged()
        {
            if (m_Watcher != null)       // get this during close down, so only do it if we are running already.
            {
                StopMonitor();
                StartMonitor();
            }
        }

        private void ScanTick(object sender, EventArgs e)
        {
            Debug.Assert(Application.MessageLoop);              // ensure.. paranoia

            try
            { 
                EliteDangerous.CheckED();

                NetLogFileInfo nfi;

                if (m_netLogFileQueue.TryDequeue(out nfi))      // if a new one queued, we swap to using it
                    lastnfi = nfi;                              // if MAY be empty note due to us picking it up before being written, in which case, we get it next tick around
                else
                    nfi = lastnfi;                              // else use the last..

                if (nfi != null)                                // if we have a last.
                {
                    FileInfo fi = new FileInfo(nfi.FileName);

                    //Console.WriteLine("File pos " + nfi.filePos +" vs " +fi.Length);

                    if (nfi.filePos < fi.Length)        // if we have no written to the end
                    {                                   // find it in list..
                        //Console.WriteLine("Reading file " + nfi.FileName + " from " + nfi.filePos + " to " + fi.Length);

                        TravelLogUnit tlUnit = (from c in m_travelogUnits where c.Name == fi.Name select c).FirstOrDefault<TravelLogUnit>();

                        if (tlUnit == null)            // if not in list, make one and add..
                        {
                            tlUnit = new TravelLogUnit();
                            tlUnit.Name = fi.Name;
                            tlUnit.Path = Path.GetDirectoryName(fi.FullName);
                            tlUnit.Size = 0;  // Add real size after data is in DB //;(int)fi.Length;
                            tlUnit.type = 1;
                            tlUnit.Add();
                            m_travelogUnits.Add(tlUnit);
                            Console.WriteLine("New travellog unit " + tlUnit.id);
                        }

                        int nrsystems = visitedSystems.Count;
                        //Console.WriteLine("Parsing file in foreground " + fi.FullName);

                        lastnfi = ParseFile(fi, visitedSystems);        // read file, return updated info..

                        if (nrsystems < visitedSystems.Count) // Om vi har fler system
                        {
                            //Console.WriteLine("Parsing changed system count, from " + nrsystems + " to " + visitedSystems.Count + " from tlunit " + tlUnit.id);

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
                                OnNewPosition(visitedSystems[nr]);    // add record nr to the list
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Net tick exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }
        }

        private void OnNewFile(object sender, FileSystemEventArgs e)        // only picks up new files
        {                                                                   // and it can kick in before any data has had time to be written to it...
            string filename = e.FullPath;

            m_Watcher.EnableRaisingEvents = false;

            System.Diagnostics.Trace.WriteLine("Log Watcher " + e.ChangeType + " " + filename);

            NetLogFileInfo nfi = new NetLogFileInfo();      // just queue up a new object, this will be replaced in readdata by the right one
            nfi.FileName = filename;                        // so no need to find the right object.. just a way of getting it to trip
            nfi.lastchanged = File.GetLastWriteTimeUtc(nfi.FileName);
            nfi.filePos = 0;
            nfi.fileSize = 0;                               // both zero, when we get around to reading it real size will be picked up
            nfi.CQC = false;        

            m_netLogFileQueue.Enqueue(nfi);       

            m_Watcher.EnableRaisingEvents = true;
        }
    }
}
