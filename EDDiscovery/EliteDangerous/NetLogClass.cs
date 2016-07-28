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
using System.Text.RegularExpressions;
using System.Globalization;

namespace EDDiscovery
{
    public class NetLogFileReader : LogReaderBase
    {
        // Header line regular expression
        private static Regex netlogHeaderRe = new Regex(@"^(?<Localtime>\d\d-\d\d-\d\d-\d\d:\d\d) (?<Timezone>.*) [(](?<GMT>\d\d:\d\d) GMT[)]");

        // Close Quarters Combat
        public bool CQC { get; set; }

        // Time and timezone
        public DateTime LastLogTime { get; set; }
        public TimeZoneInfo TimeZone { get; set; }
        public TimeSpan TimeZoneOffset { get; set; }

        public NetLogFileReader(string filename) : base(filename) { }
        public NetLogFileReader(TravelLogUnit tlu) : base(tlu) { }

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
                        ps.Source = TravelLogUnit.id;
                        ps.Unit = TravelLogUnit.Name;
                        vsc = ps;
                        return true;
                    }
                }
            }

            vsc = null;
            return false;
        }

        public bool ReadHeader()
        {
            string line = null;

            // Try to read the first line of the log file
            try
            {
                using (Stream stream = File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (TextReader reader = new StreamReader(stream))
                    {
                        line = reader.ReadLine();
                    }
                }
            }
            catch
            {
            }

            // File may not have been written yet.
            if (line == null)
            {
                return false;
            }

            // Extract the start time from the first line
            Match match = netlogHeaderRe.Match(line);
            if (match != null && match.Success)
            {
                string localtimestr = match.Groups["Localtime"].Value;
                string timezonename = match.Groups["Timezone"].Value.Trim();
                string gmtimestr = match.Groups["GMT"].Value;

                DateTime localtime = DateTime.MinValue;
                TimeSpan gmtime = TimeSpan.MinValue;
                TimeZoneInfo tzi = TimeZoneInfo.GetSystemTimeZones().FirstOrDefault(t => t.DaylightName.Trim() == timezonename || t.StandardName.Trim() == timezonename);

                if (DateTime.TryParseExact(localtimestr, "yy-MM-dd-HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out localtime) &&
                    TimeSpan.TryParseExact(gmtimestr, "h\\:mm", CultureInfo.InvariantCulture, out gmtime))
                {
                    // Grab the timezone offset
                    TimeSpan tzoffset = localtime.TimeOfDay - gmtime;

                    if (tzi != null)
                    {
                        // Correct for wildly inaccurate values
                        if (tzoffset > tzi.BaseUtcOffset + TimeSpan.FromHours(18))
                        {
                            tzoffset -= TimeSpan.FromHours(24);
                        }
                        else if (tzoffset < tzi.BaseUtcOffset - TimeSpan.FromHours(18))
                        {
                            tzoffset += TimeSpan.FromHours(24);
                        }
                    }

                    // Set the start time, timezone info and timezone offset
                    LastLogTime = localtime;
                    TimeZone = tzi;
                    TimeZoneOffset = tzoffset;
                    TravelLogUnit.type = 1;
                }

                return true;
            }

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

        NetLogFileReader lastnfi = null;          // last one read..

        DateTime gammastart = new DateTime(2014, 11, 22, 13, 00, 00);

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

                lastnfi = OpenFileReader(fi);

                if (lastnfi.TimeZone == null)
                {
                    lastnfi.ReadHeader();
                }

                if (!m_travelogUnits.ContainsKey(lastnfi.TravelLogUnit.Name))
                {
                    m_travelogUnits[lastnfi.TravelLogUnit.Name] = lastnfi.TravelLogUnit;
                    lastnfi.TravelLogUnit.Add();
                }

                if (!netlogreaders.ContainsKey(lastnfi.TravelLogUnit.Name))
                {
                    netlogreaders[lastnfi.TravelLogUnit.Name] = lastnfi;
                }

                if (lastnfi.filePos != fi.Length || i == allFiles.Length - 1)  // File not already in DB, or is the last one
                {
                    foreach (VisitedSystemsClass ps in ReadData(lastnfi))
                    {
                        if (!VisitedSystemsClass.Exist(ps.Name, ps.Time))
                        {
                            ps.EDSM_sync = false;
                            ps.MapColour = defaultMapColour;
                            ps.Commander = EDDConfig.Instance.CurrentCmdrID;

                            ps.Add();
                            visitedSystems.Add(ps);
                        }
                    }

                    lastnfi.TravelLogUnit.Update();
                }
            }

            // update the VSC with data from the db
            VisitedSystemsClass.UpdateSys(visitedSystems, EDDiscoveryForm.EDDConfig.UseDistances);
            return visitedSystems;
        }

        private NetLogFileReader OpenFileReader(FileInfo fi)
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

            return reader;
        }

        private IEnumerable<VisitedSystemsClass> ReadData(NetLogFileReader sr)
        {
            long startpos = sr.filePos;

            if (sr.TimeZone == null)
            {
                if (!sr.ReadHeader())  // may be empty if we read it too fast.. don't worry, monitor will pick it up
                {
                    System.Diagnostics.Trace.WriteLine("File was empty (for now) " + sr.FileName);
                    yield break;
                }
            }

            VisitedSystemsClass ps;
            while (sr.ReadNetLogSystem(out ps))
            {
                if (ps.Name.Equals(VisitedSystemsClass.GetLast(EDDConfig.Instance.CurrentCmdrID, ps.Time).Name, StringComparison.InvariantCultureIgnoreCase))
                    continue;

                if (ps.Time.Subtract(gammastart).TotalMinutes > 0)  // Ta bara med efter gamma.
                    yield return ps;
            }

            Console.WriteLine("Parse ReadData " + sr.FileName + " from " + startpos + " to " + sr.filePos);
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
                    nfi = OpenFileReader(new FileInfo(filename));
                    lastnfi = nfi;
                }

                if (lastnfi != null)
                {
                    if (lastnfi.TimeZone == null)
                    {
                        lastnfi.ReadHeader();
                        lastnfi.TravelLogUnit.Add();
                    }

                    foreach(VisitedSystemsClass dbsys in ReadData(lastnfi))
                    {
                        dbsys.EDSM_sync = false;
                        dbsys.MapColour = EDDConfig.Instance.DefaultMapColour;
                        dbsys.Commander = EDDConfig.Instance.CurrentCmdrID;
                        dbsys.Add();

                        // here we need to make sure the cursystem is set up.. need to do it here because OnNewPosition expects all cursystems to be non null..

                        VisitedSystemsClass item2 = visitedSystems.LastOrDefault();
                        VisitedSystemsClass.UpdateVisitedSystemsEntries(dbsys, item2, EDDiscoveryForm.EDDConfig.UseDistances);       // ensure they have system classes behind them..
                        visitedSystems.Add(dbsys);
                        OnNewPosition(dbsys);
                        lastnfi.TravelLogUnit.Update();
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
