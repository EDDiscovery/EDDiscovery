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
    public class NetLogFileReader
    {
        // File buffer
        protected byte[] buffer;
        protected long fileptr;
        protected int bufferpos;
        protected int bufferlen;

        protected Stream stream;

        // File Information
        public string FileName { get { return Path.Combine(TravelLogUnit.Path, TravelLogUnit.Name); } }
        public long filePos { get { return TravelLogUnit.Size; } }
        public bool CQC { get; set; }
        public TravelLogUnit TravelLogUnit { get; protected set; }
        public DateTime LastLogTime { get; set; }

        public NetLogFileReader(string filename)
        {
            FileInfo fi = new FileInfo(filename);

            this.TravelLogUnit = new TravelLogUnit
            {
                Name = fi.Name,
                Path = fi.DirectoryName,
                Size = 0
            };

            this.stream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }

        public NetLogFileReader(TravelLogUnit tlu)
        {
            this.TravelLogUnit = tlu;
            this.stream = File.Open(Path.Combine(tlu.Path, tlu.Name), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }

        public bool ReadLine(out string line)
        {
            // Initialize buffer if not yet allocated
            if (buffer == null)
            {
                buffer = new byte[16384];
                bufferpos = 0;
                bufferlen = 0;
                fileptr = TravelLogUnit.Size;
            }

            // Loop while no end-of-line is found
            while (true)
            {
                int endlinepos = -1;

                // Only look for an end-of-line if not already at end of buffer
                if (bufferpos < bufferlen)
                {
                    // Find the next end-of-line
                    endlinepos = Array.IndexOf(buffer, (byte)'\n', bufferpos, bufferlen - bufferpos) - bufferpos;

                    // End-of-line found
                    if (endlinepos >= 0)
                    {
                        // Include the end-of-line in the line length
                        int linelen = endlinepos + 1;

                        // Trim any trailing carriage-return
                        if (endlinepos > 0 && buffer[bufferpos + endlinepos - 1] == '\r')
                        {
                            endlinepos--;
                        }

                        // Return the trimmed string
                        byte[] buf = new byte[endlinepos];
                        Buffer.BlockCopy(buffer, bufferpos, buf, 0, endlinepos);
                        bufferpos += linelen;
                        TravelLogUnit.Size += linelen;
                        line = new String(buf.Select(c => (char)c).ToArray());

                        return true;
                    }
                }

                // No end-of-line found
                // Move remaining data to start of buffer
                if (bufferpos != 0)
                {
                    Buffer.BlockCopy(buffer, bufferpos, buffer, 0, bufferlen - bufferpos);
                    bufferlen -= bufferpos;
                    bufferpos = 0;
                }

                // Expand the buffer if buffer is full
                if (bufferlen == buffer.Length)
                {
                    Array.Resize(ref buffer, buffer.Length * 2);
                }

                // Read the data into the buffer
                stream.Seek(fileptr, SeekOrigin.Begin);
                int bytesread = stream.Read(buffer, bufferlen, buffer.Length - bufferlen);

                // Return false if end-of-file is encountered
                if (bytesread == 0)
                {
                    // Free the buffer if the buffer was also exhausted
                    if (bufferpos == bufferlen)
                    {
                        buffer = null;
                    }

                    line = null;
                    return false;
                }

                // Update the buffer length and next read pointer
                bufferlen += bytesread;
                fileptr += bytesread;

                // No end-of-line encountered - try reading a line again
            }
        }

        public bool ReadNetLogSystem(out VisitedSystemsClass vsc)
        {
            string line;
            while (this.ReadLine(out line))
            {
                if (line.Contains("[PG] [Notification] Left a playlist lobby"))
                    this.CQC = false;

                if (line.Contains("[PG] Destroying playlist lobby."))
                    this.CQC = false;

                if (line.Contains("[PG] [Notification] Joined a playlist lobby"))
                    this.CQC = true;
                if (line.Contains("[PG] Created playlist lobby"))
                    this.CQC = true;
                if (line.Contains("[PG] Found matchmaking lobby object"))
                    this.CQC = true;

                if (line.Contains(" System:") && this.CQC == false)
                {
                    //Console.WriteLine(" RD:" + line );
                    if (line.Contains("ProvingGround"))
                        continue;

                    VisitedSystemsClass ps = VisitedSystemsClass.Parse(this.LastLogTime, line);
                    if (ps != null)
                    {   // Remove some training systems
                        if (ps.Name.Equals("Training"))
                            continue;
                        if (ps.Name.Equals("Destination"))
                            continue;
                        if (ps.Name.Equals("Altiris"))
                            continue;
                        this.LastLogTime = ps.Time;
                        vsc = ps;
                        return true;
                    }
                }
            }

            vsc = null;
            return false;
        }
    }

    public class NetLogClass
    {
        public delegate void NetLogEventHandler(VisitedSystemsClass vsc);

        public event NetLogEventHandler OnNewPosition;          // called in foreground, no need for invoke

        public List<VisitedSystemsClass> visitedSystems = new List<VisitedSystemsClass>();

        Dictionary<string, NetLogFileReader> netlogreaders = new Dictionary<string, NetLogFileReader>();

        FileSystemWatcher m_Watcher;
        ConcurrentQueue<string> m_netLogFileQueue;
        System.Windows.Forms.Timer m_scantimer;
        Dictionary<string, TravelLogUnit> m_travelogUnits;

        public List<TravelLogUnit> tlUnits;

        NetLogFileReader lastnfi = null;          // last one read..

        public NetLogClass(EDDiscoveryForm ds)
        {
            m_travelogUnits = TravelLogUnit.GetAll().ToDictionary(t => t.Name);
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

        private NetLogFileReader ParseFile(FileInfo fi, List<VisitedSystemsClass> visitedSystems)
        {
            NetLogFileReader ret = null;

            try
            {
                NetLogFileReader reader;

                if (netlogreaders.ContainsKey(fi.Name))
                {
                    reader = netlogreaders[fi.Name];
                }
                else if (m_travelogUnits.ContainsKey(fi.Name))
                {
                    reader = new NetLogFileReader(m_travelogUnits[fi.Name]);
                }
                else
                {
                    reader = new NetLogFileReader(fi.FullName);
                }

                ret = ReadData(reader, visitedSystems);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Parse File exception: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }

            return ret;
        }
        // returns info on what its read..
        private NetLogFileReader ReadData(NetLogFileReader sr, List<VisitedSystemsClass> visitedSystems)
        {

            DateTime gammastart = new DateTime(2014, 11, 22, 13, 00, 00);

            long startpos = sr.filePos;

            string line = null;
            if (sr.filePos == 0)
            {
                if (sr.ReadLine(out line))  // may be empty if we read it too fast.. don't worry, monitor will pick it up
                {
                    string str = "20" + line.Substring(0, 8) + " " + line.Substring(9, 5);

                    sr.LastLogTime = DateTime.Parse(str);
                }
                else
                {
                    System.Diagnostics.Trace.WriteLine("File was empty (for now) " + sr.FileName);
                    return sr;
                }
            }

            VisitedSystemsClass ps;
            while (sr.ReadNetLogSystem(out ps))
            {
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

            Console.WriteLine("Parse ReadData " + sr.FileName + " from " + startpos + " to " + sr.filePos);

            return sr;
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
                        m_netLogFileQueue = new ConcurrentQueue<string>();
                        m_Watcher = new System.IO.FileSystemWatcher();
                        m_Watcher.Path = logpath + Path.DirectorySeparatorChar;
                        m_Watcher.Filter = "netLog*.log";
                        m_Watcher.IncludeSubdirectories = false;
                        m_Watcher.NotifyFilter = NotifyFilters.FileName;
                        m_Watcher.Changed += new FileSystemEventHandler(OnNewFile);
                        m_Watcher.Created += new FileSystemEventHandler(OnNewFile);
                        m_Watcher.EnableRaisingEvents = true;

                        EDDConfig.Instance.NetLogDirChanged += EDDConfig_NetLogDirChanged;

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

                string filename = null;
                NetLogFileReader nfi = null;

                int nrsystems = visitedSystems.Count;
                if (m_netLogFileQueue.TryDequeue(out filename))      // if a new one queued, we swap to using it
                {
                    nfi = ParseFile(new FileInfo(filename), visitedSystems);
                    lastnfi = nfi;
                }
                else if (lastnfi != null)
                {
                    nfi = ReadData(lastnfi, visitedSystems);                              // else use the last..
                }

                    //Console.WriteLine("File pos " + nfi.filePos +" vs " +fi.Length);

                        //Console.WriteLine("Reading file " + nfi.FileName + " from " + nfi.filePos + " to " + fi.Length);

                        //Console.WriteLine("Parsing file in foreground " + fi.FullName);

                        if (nrsystems < visitedSystems.Count) // Om vi har fler system
                        {
                            //Console.WriteLine("Parsing changed system count, from " + nrsystems + " to " + visitedSystems.Count + " from tlunit " + tlUnit.id);

                            for (int nr = nrsystems; nr < visitedSystems.Count; nr++)  // Lägg till nya i locala databaslogen
                            {
                                VisitedSystemsClass dbsys = visitedSystems[nr];
                                dbsys.Source = nfi.TravelLogUnit.id;
                                dbsys.EDSM_sync = false;
                                dbsys.MapColour = EDDConfig.Instance.DefaultMapColour;
                                dbsys.Unit = nfi.TravelLogUnit.Name;
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
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Net tick exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }
        }

        private void OnNewFile(object sender, FileSystemEventArgs e)        // only picks up new files
        {                                                                   // and it can kick in before any data has had time to be written to it...
            string filename = e.FullPath;
            m_netLogFileQueue.Enqueue(filename);
        }
    }
}
