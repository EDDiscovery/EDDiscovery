using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;
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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Data.Common;
using Newtonsoft.Json.Linq;

namespace EDDiscovery
{
    public class NetLogClass
    {
        public delegate void NetLogEventHandler(JournalLocOrJump vsc);

        public event NetLogEventHandler OnNewPosition;          // called in foreground, no need for invoke

        Dictionary<string, NetLogFileReader> netlogreaders = new Dictionary<string, NetLogFileReader>();

        FileSystemWatcher m_Watcher;
        ConcurrentQueue<string> m_netLogFileQueue;
        System.Windows.Forms.Timer m_scantimer;
        System.ComponentModel.BackgroundWorker m_worker;

        NetLogFileReader lastnfi = null;          // last one read..


        public NetLogClass(EDDiscoveryForm ds)
        {
        }

        public string GetNetLogDir()
        {
            try
            {
                string netlogdirstored = EDDConfig.Instance.NetLogDir;
                string datapath = null;
                if (EDDConfig.Instance.NetLogDirAutoMode)
                {
                    if (EliteDangerousClass.EDDirectory != null && EliteDangerousClass.EDDirectory.Length > 0)
                    {
                        datapath = Path.Combine(EliteDangerousClass.EDDirectory, "Logs");
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
                    string cmdrpath = EDDConfig.Instance.CurrentCommander.NetLogDir;
                    if (cmdrpath != null && cmdrpath != "" && Directory.Exists(cmdrpath))
                    {
                        datapath = cmdrpath;
                    }
                }

                return datapath;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("GetNetLogDir exception: " + ex.Message);
                return null;
            }
        }

        // called during start up and if refresh history is pressed

        public List<JournalEntry> ParseFiles(out string error, int defaultMapColour, Func<bool> cancelRequested, Action<int, string> updateProgress, bool forceReload = false)
        {
            string datapath = GetNetLogDir();

            return ParseFiles(datapath, out error, defaultMapColour, cancelRequested, updateProgress, forceReload, netlogreaders);
        }

        public static List<JournalEntry> ParseFiles(string datapath, out string error, int defaultMapColour, Func<bool> cancelRequested, Action<int, string> updateProgress, bool forceReload = false, Dictionary<string, NetLogFileReader> netlogreaders = null, int currentcmdrid = -1)
        {
            error = null;

            if (datapath == null)
            {
                error = "Netlog directory not set!";
                return null;
            }

            if (!Directory.Exists(datapath))   // if logfiles directory is not found
            {
                error = "Netlog directory is not present!";
                return null;
            }

            if (netlogreaders == null)
            {
                netlogreaders = new Dictionary<string, NetLogFileReader>();
            }

            if (currentcmdrid < 0)
            {
                currentcmdrid = EDDConfig.Instance.CurrentCmdrID;
            }

            List<JournalLocOrJump> visitedSystems = new List<JournalLocOrJump>();
            Dictionary<string, TravelLogUnit> m_travelogUnits = TravelLogUnit.GetAll().Where(t => t.type == 1).GroupBy(t => t.Name).Select(g => g.First()).ToDictionary(t => t.Name);
            Dictionary<int, string> travellogunitid2name = m_travelogUnits.Values.ToDictionary(t => (int)t.id, t => t.Name);
            Dictionary<string, List<JournalLocOrJump>> vsc_lookup = JournalEntry.GetAll().OfType<JournalLocOrJump>().GroupBy(v => v.JournalId).Where(g => travellogunitid2name.ContainsKey(g.Key)).ToDictionary(g => travellogunitid2name[g.Key], g => g.ToList());
            HashSet<int> journalids = new HashSet<int>(m_travelogUnits.Values.Select(t => (int)t.id).ToList());
            List<JournalLocOrJump> vsSystemsList = JournalEntry.GetAll(currentcmdrid).OfType<JournalLocOrJump>().Where(j => journalids.Contains(j.JournalId)).ToList();

            if (vsSystemsList != null)
            {
                foreach (JournalLocOrJump vs in vsSystemsList)
                {
                    if (visitedSystems.Count == 0)
                        visitedSystems.Add(vs);
                    else if (!visitedSystems.Last().StarSystem.Equals(vs.StarSystem, StringComparison.CurrentCultureIgnoreCase))  // Avoid duplicate if times exist in same system from different files.
                        visitedSystems.Add(vs);
                    else
                    {
                        /*
                        VisitedSystemsClass vs2 = (VisitedSystemsClass)visitedSystems.Last<VisitedSystemsClass>();
                        if (vs2.id != vs.id)
                        {
                            vs.Commander = -2; // Move to dupe user
                            vs.Update();
                        }
                         */
                    }
                }
            }

            // order by file write time so we end up on the last one written
            FileInfo[] allFiles = Directory.EnumerateFiles(datapath, "netLog.*.log", SearchOption.AllDirectories).Select(f => new FileInfo(f)).OrderBy(p => p.LastWriteTime).ToArray();

            List<NetLogFileReader> readersToUpdate = new List<NetLogFileReader>();

            for (int i = 0; i < allFiles.Length; i++)
            {
                FileInfo fi = allFiles[i];

                var reader = OpenFileReader(fi, m_travelogUnits, vsc_lookup, netlogreaders);

                if (!m_travelogUnits.ContainsKey(reader.TravelLogUnit.Name))
                {
                    m_travelogUnits[reader.TravelLogUnit.Name] = reader.TravelLogUnit;
                    reader.TravelLogUnit.Add();
                }

                if (!netlogreaders.ContainsKey(reader.TravelLogUnit.Name))
                {
                    netlogreaders[reader.TravelLogUnit.Name] = reader;
                }

                if (forceReload)
                {
                    // Force a reload of the travel log
                    reader.TravelLogUnit.Size = 0;
                }

                if (reader.filePos != fi.Length || i == allFiles.Length - 1)  // File not already in DB, or is the last one
                {
                    readersToUpdate.Add(reader);
                }
            }

            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                for (int i = 0; i < readersToUpdate.Count; i++)
                {
                    NetLogFileReader reader = readersToUpdate[i];
                    updateProgress(i * 100 / readersToUpdate.Count, reader.TravelLogUnit.Name);

                    using (DbTransaction tn = cn.BeginTransaction())
                    {
                        foreach (JObject jo in reader.ReadSystems(cancelRequested, currentcmdrid))
                        {
                            jo["EDDMapColor"] = defaultMapColour;

                            JournalLocOrJump je = new JournalFSDJump(jo)
                            {
                                JournalId = (int)reader.TravelLogUnit.id,
                                SyncedEDSM = false,
                                CommanderId = currentcmdrid,
                            };

                            je.Add(cn, tn);
                            visitedSystems.Add(je);
                        }

                        reader.TravelLogUnit.Update(cn, tn);

                        tn.Commit();
                    }

                    if (updateProgress != null)
                    {
                        updateProgress((i + 1) * 100 / readersToUpdate.Count, reader.TravelLogUnit.Name);
                    }
                }
            }

            return visitedSystems.ToList<JournalEntry>();
        }

        private static NetLogFileReader OpenFileReader(FileInfo fi, Dictionary<string, TravelLogUnit> tlu_lookup = null, Dictionary<string, List<JournalLocOrJump>> vsc_lookup = null, Dictionary<string, NetLogFileReader> netlogreaders = null)
        {
            NetLogFileReader reader;
            TravelLogUnit tlu;
            List<JournalLocOrJump> vsclist = null;

            if (vsc_lookup != null && vsc_lookup.ContainsKey(fi.Name))
            {
                vsclist = vsc_lookup[fi.Name];
            }

            if (netlogreaders != null && netlogreaders.ContainsKey(fi.Name))
            {
                return netlogreaders[fi.Name];
            }
            else if (tlu_lookup != null && tlu_lookup.ContainsKey(fi.Name))
            {
                tlu = tlu_lookup[fi.Name];
                tlu.Path = fi.DirectoryName;
                reader = new NetLogFileReader(tlu, vsclist);
            }
            else if (TravelLogUnit.TryGet(fi.Name, out tlu))
            {
                tlu.Path = fi.DirectoryName;
                reader = new NetLogFileReader(tlu, vsclist);
            }
            else
            {
                reader = new NetLogFileReader(fi.FullName);
            }

            if (netlogreaders != null)
            {
                netlogreaders[fi.Name] = reader;
            }

            return reader;
        }

        public void StartMonitor()
        {
            Debug.Assert(Application.MessageLoop);              // ensure.. paranoia

            if (m_Watcher == null)
            {
                try
                {
                    string logpath = GetNetLogDir();

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

                        m_worker = new System.ComponentModel.BackgroundWorker();
                        m_worker.DoWork += ScanTickWorker;
                        m_worker.RunWorkerCompleted += ScanTickDone;
                        m_worker.WorkerSupportsCancellation = true;

                        m_scantimer = new System.Windows.Forms.Timer();
                        m_scantimer.Interval = 2000;
                        m_scantimer.Tick += ScanTick;
                        m_scantimer.Start();

                        Console.WriteLine("Start Monitor");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("Start Monitor exception : " + ex.Message);
                    System.Diagnostics.Trace.WriteLine(ex.StackTrace);
                }
            }
        }

        public void StopMonitor()
        {
            if (m_scantimer != null)
            {
                m_scantimer.Stop();
                m_scantimer = null;
            }

            if (m_worker != null)
            {
                m_worker.CancelAsync();
                m_worker = null;
            }

            if (m_Watcher != null)
            {
                EDDConfig.Instance.NetLogDirChanged -= EDDConfig_NetLogDirChanged;

                m_Watcher.EnableRaisingEvents = false;
                m_Watcher.Dispose();
                m_Watcher = null;

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

        private void ScanTickDone(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Debug.Assert(Application.MessageLoop);              // ensure.. paranoia

            if (e.Error == null && !e.Cancelled)
            {
                List<JournalLocOrJump> entries = (List<JournalLocOrJump>)e.Result;

                foreach (var ent in entries)
                {
                    OnNewPosition(ent);
                }
            }
        }

        private void ScanTick(object sender, EventArgs e)
        {
            Debug.Assert(Application.MessageLoop);              // ensure.. paranoia

            if (m_worker != null && !m_worker.IsBusy)
            {
                m_worker.RunWorkerAsync();
            }
        }

        private void ScanTickWorker(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var worker = sender as System.ComponentModel.BackgroundWorker;
            var entries = new List<JournalLocOrJump>();
            e.Result = entries;
            int netlogpos = 0;
            NetLogFileReader nfi = null;

            try
            {
                if (EDDConfig.Instance.NetLogDirAutoMode)
                {
                    EliteDangerousClass.CheckED();
                }

                string filename = null;

                if (m_netLogFileQueue.TryDequeue(out filename))      // if a new one queued, we swap to using it
                {
                    nfi = OpenFileReader(new FileInfo(filename));
                    lastnfi = nfi;
                }
                else if (lastnfi != null && (!File.Exists(lastnfi.FileName) || lastnfi.filePos >= new FileInfo(lastnfi.FileName).Length))
                {
                    HashSet<string> tlunames = new HashSet<string>(TravelLogUnit.GetAllNames());
                    string[] filenames = Directory.EnumerateFiles(GetNetLogDir(), "netLog.*.log", SearchOption.AllDirectories)
                                                  .Select(s => new { name = Path.GetFileName(s), fullname = s })
                                                  .Where(s => !tlunames.Contains(s.name))
                                                  .OrderBy(s => s.name)
                                                  .Select(s => s.fullname)
                                                  .ToArray();
                    foreach (var name in filenames)
                    {
                        nfi = OpenFileReader(new FileInfo(name));
                        lastnfi = nfi;
                        break;
                    }
                }
                else
                {
                    nfi = lastnfi;
                }

                if (nfi != null)
                {
                    if (nfi.TimeZone == null)
                    {
                        nfi.ReadHeader();
                        if (nfi.TravelLogUnit.id == 0)
                            nfi.TravelLogUnit.Add();
                    }

                    netlogpos = nfi.TravelLogUnit.Size;

                    foreach(JObject jo in nfi.ReadSystems())
                    {
                        jo["EDDMapColor"] = EDDConfig.Instance.DefaultMapColour;
                        JournalLocOrJump je = new JournalFSDJump(jo);
                        je.SyncedEDSM = false;
                        je.CommanderId = EDDConfig.Instance.CurrentCmdrID;
                        je.Add();

                        // here we need to make sure the cursystem is set up.. need to do it here because OnNewPosition expects all cursystems to be non null..

                        //JournalLocOrJump item2 = JournalEntry.GetLast<JournalLocOrJump>(je.CommanderId, je.EventTimeUTC);
                        //VisitedSystemsClass.UpdateVisitedSystemsEntries(dbsys, item2, EDDiscoveryForm.EDDConfig.UseDistances);       // ensure they have system classes behind them..
                        entries.Add(je);

                        if (worker.CancellationPending)
                        {
                            break;
                        }
                    }
                    nfi.TravelLogUnit.Update();
                }

                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                // Revert and re-read the failed entries
                if (nfi != null && nfi.TravelLogUnit != null)
                {
                    nfi.TravelLogUnit.Size = netlogpos;
                }

                System.Diagnostics.Trace.WriteLine("Net tick exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
                throw;
            }
        }

        private void OnNewFile(object sender, FileSystemEventArgs e)        // only picks up new files
        {                                                                   // and it can kick in before any data has had time to be written to it...
            string filename = e.FullPath;
            m_netLogFileQueue.Enqueue(filename);
        }
    }
}
