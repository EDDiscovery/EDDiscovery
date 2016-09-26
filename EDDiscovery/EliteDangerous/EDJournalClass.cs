using EDDiscovery2;
using EDDiscovery2.DB;
using EDDiscovery.DB;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data.Common;
using System.Runtime.InteropServices;

namespace EDDiscovery.EliteDangerous
{
    public class EDJournalClass
    {
        public delegate void NewJournalEntryHandler(JournalEntry je);
        public event NewJournalEntryHandler OnNewJournalEntry;

        Dictionary<string, EDJournalReader> netlogreaders = new Dictionary<string, EDJournalReader>();
        FileSystemWatcher m_Watcher;
        string m_watcherfolder;
        ConcurrentQueue<string> m_netLogFileQueue;
        System.Windows.Forms.Timer m_scantimer;
        System.ComponentModel.BackgroundWorker m_worker;
        EDJournalReader lastnfi = null;          // last one read..
        private static Regex journalNamePrefixRe = new Regex("(?<prefix>.*)[.]0*(?<part>[0-9][0-9]*)[.]log");

        public EDJournalClass()
        {
        }

        public static string GetDefaultJournalDir()
        {
            string path;

            // Windows Saved Games path (Vista and above)
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 6)
            {
                IntPtr pszPath;
                if (SHGetKnownFolderPath(Win32FolderId_SavedGames, 0, IntPtr.Zero, out pszPath) == 0)
                {
                    path = Marshal.PtrToStringUni(pszPath);
                    Marshal.FreeCoTaskMem(pszPath);
                    return Path.Combine(path, "Frontier Developments", "Elite Dangerous");
                }
            }

            // OS X ApplicationSupportDirectory path (Darwin 12.0 == OS X 10.8)
            if (Environment.OSVersion.Platform == PlatformID.Unix && Environment.OSVersion.Version.Major >= 12)
            {
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Library", "Application Support", "Frontier Developments", "Elite Dangerous");

                if (Directory.Exists(path))
                {
                    return path;
                }
            }

            return null;
        }

        public static string GetJournalDir()        // MAY return null if folder does not exist..
        {
            try
            {
                string journaldirstored = EDDConfig.Instance.JournalDir;
                string autopath = GetDefaultJournalDir();

                string datapath;

                if (EDDConfig.Instance.JournalDirAutoMode && autopath != null )
                {
                    datapath = autopath;
                }
                else
                {
                    string cmdrpath = EDDConfig.Instance.CurrentCommander.NetLogDir;

                    if (cmdrpath != null && cmdrpath != "" && Directory.Exists(cmdrpath))
                        datapath = cmdrpath;
                    else
                        datapath = journaldirstored;
                }

                if (Directory.Exists(datapath))   // if logfiles directory is not found
                    return datapath;
                else
                    return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("GetJournalDir exception: " + ex.Message);
                return null;
            }
        }

        // called during start up and if refresh history is pressed
        public List<JournalEntry> ParseJournalFiles(bool forceReload = false)
        {
            return ParseJournalFiles(EDDConfig.Instance.DefaultMapColour, () => false, (p, s) => { }, forceReload);
        }


        public List<JournalEntry> ParseJournalFiles(int defaultMapColour, Func<bool> cancelRequested, Action<int, string> updateProgress, bool forceReload = false)
        {
            string datapath = GetJournalDir();  // ensures folder is there, else null.

            if (datapath == null)
                return null;

            Dictionary<long, List<JournalEntry>> journalentries = JournalEntry.GetAll(EDDConfig.Instance.CurrentCmdrID).GroupBy(e => e.JournalId).ToDictionary(g => g.Key, g => g.ToList());
            Dictionary<string, TravelLogUnit> m_travelogUnits = TravelLogUnit.GetAll().Where(t => t.type == 3).GroupBy(t => t.Name).Select(g => g.First()).ToDictionary(t => t.Name);

            // order by file write time so we end up on the last one written
            FileInfo[] allFiles = Directory.EnumerateFiles(datapath, "Journal.*.log", SearchOption.AllDirectories).Select(f => new FileInfo(f)).OrderBy(p => p.LastWriteTime).ToArray();

            List<EDJournalReader> readersToUpdate = new List<EDJournalReader>();

            for (int i = 0; i < allFiles.Length; i++)
            {
                FileInfo fi = allFiles[i];

                var reader = OpenFileReader(fi, m_travelogUnits);

                if (!m_travelogUnits.ContainsKey(reader.TravelLogUnit.Name))
                {
                    m_travelogUnits[reader.TravelLogUnit.Name] = reader.TravelLogUnit;
                    reader.TravelLogUnit.type = 3;
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
                    EDJournalReader reader = readersToUpdate[i];
                    updateProgress(i * 100 / readersToUpdate.Count, reader.TravelLogUnit.Name);

                    List<JournalEntry> entries = reader.ReadJournalLog().ToList();

                    using (DbTransaction tn = cn.BeginTransaction())
                    {
                        foreach (JournalEntry je in entries)
                        {
                            if (!journalentries.ContainsKey(je.JournalId))                      //Rob added
                                journalentries.Add(je.JournalId, new List<JournalEntry>());

                            System.Diagnostics.Trace.WriteLine(string.Format("Write Journal to db {0} {1}", je.EventTimeUTC, je.EventTypeStr));
                            journalentries[je.JournalId].Add(je);
                            je.Add(cn, tn);
                        }

                        reader.TravelLogUnit.Update(cn, tn);

                        tn.Commit();
                    }

                    if (updateProgress != null)
                    {
                        updateProgress((i + 1) * 100 / readersToUpdate.Count, reader.TravelLogUnit.Name);
                    }

                    lastnfi = reader;
                }
            }

            return journalentries.OrderBy(kvp => kvp.Key).SelectMany(kvp => kvp.Value).OrderBy(j => j.EventTimeUTC).OrderBy(j=>j.Id).ToList();
        }

        private EDJournalReader OpenFileReader(FileInfo fi, Dictionary<string, TravelLogUnit> tlu_lookup = null)
        {
            EDJournalReader reader;
            TravelLogUnit tlu;

            System.Diagnostics.Trace.WriteLine(string.Format("File Read {0}", fi.FullName));

            if (netlogreaders.ContainsKey(fi.Name))
            {
                reader = netlogreaders[fi.Name];
            }
            else if (tlu_lookup != null && tlu_lookup.ContainsKey(fi.Name))
            {
                tlu = tlu_lookup[fi.Name];
                tlu.Path = fi.DirectoryName;
                reader = new EDJournalReader(tlu);
                netlogreaders[fi.Name] = reader;
            }
            else if (TravelLogUnit.TryGet(fi.Name, out tlu))
            {
                tlu.Path = fi.DirectoryName;
                reader = new EDJournalReader(tlu);
                netlogreaders[fi.Name] = reader;
            }
            else
            {
                reader = new EDJournalReader(fi.FullName);

                // Bring over the commander from the previous log if possible
                Match match = journalNamePrefixRe.Match(fi.Name);
                if (match.Success)
                {
                    string prefix = match.Groups["prefix"].Value;
                    string partstr = match.Groups["part"].Value;
                    int part;
                    if (Int32.TryParse(partstr, NumberStyles.Integer, CultureInfo.InvariantCulture, out part) && part > 1)
                    {
                        EDCommander lastcmdr = EDDConfig.Instance.CurrentCommander;
                        var lastreader = netlogreaders.Where(kvp => kvp.Key.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                                                      .Select(k => k.Value)
                                                      .FirstOrDefault();
                        if (lastreader != null)
                        {
                            lastcmdr = lastreader.Commander;
                        }

                        reader.Commander = lastcmdr;
                    }
                }

                netlogreaders[fi.Name] = reader;
            }

            return reader;
        }

        public void StartMonitor()
        {
            return;

            // BUGS in scanning.. basically the reader does not know the commander ID if it has just reopened the file.

            Debug.Assert(Application.MessageLoop);              // ensure.. paranoia

            if (m_Watcher == null)
            {
                try
                {
                    string logpath = GetJournalDir();

                    if (logpath!=null)
                    {
                        m_watcherfolder = string.Copy(logpath); // being paranoid.  keep our own copy, in case it gets changed out from under us
                        m_netLogFileQueue = new ConcurrentQueue<string>();
                        m_Watcher = new System.IO.FileSystemWatcher();
                        m_Watcher.Path = logpath + Path.DirectorySeparatorChar;
                        m_Watcher.Filter = "Journal.*.log";
                        m_Watcher.IncludeSubdirectories = false;
                        m_Watcher.NotifyFilter = NotifyFilters.FileName;
                        m_Watcher.Changed += new FileSystemEventHandler(OnNewFile);
                        m_Watcher.Created += new FileSystemEventHandler(OnNewFile);
                        m_Watcher.EnableRaisingEvents = true;

                        m_worker = new System.ComponentModel.BackgroundWorker();
                        m_worker.DoWork += ScanTickWorker;
                        m_worker.RunWorkerCompleted += ScanTickDone;
                        m_worker.WorkerSupportsCancellation = true;

                        m_scantimer = new System.Windows.Forms.Timer();
                        m_scantimer.Interval = 2000;
                        m_scantimer.Tick += ScanTick;
                        m_scantimer.Start();

                        System.Diagnostics.Trace.WriteLine("Start Monitor");
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
                m_Watcher.EnableRaisingEvents = false;
                m_Watcher.Dispose();
                m_Watcher = null;

                System.Diagnostics.Trace.WriteLine("Stop Monitor");
            }
        }

        public void NetLogDirChanged() // call from owner of scanner.
        {
            if (m_Watcher != null)       
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
                List<JournalEntry> entries = (List<JournalEntry>)e.Result;

                foreach (var ent in entries)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("New entry {0} {1}", ent.EventTimeUTC, ent.EventTypeStr));
                    OnNewJournalEntry(ent);
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
            var entries = new List<JournalEntry>();
            e.Result = entries;
            int netlogpos = 0;
            EDJournalReader nfi = null;

            try
            {
                if (EDDConfig.Instance.JournalDirAutoMode)
                {
                    EliteDangerousClass.CheckED();
                }

                string filename = null;

                if (m_netLogFileQueue.TryDequeue(out filename))      // if a new one queued, we swap to using it
                {
                    nfi = OpenFileReader(new FileInfo(filename));
                    lastnfi = nfi;
                    System.Diagnostics.Trace.WriteLine(string.Format("Change in file, scan {0}", lastnfi.FileName));
                }
                else if (lastnfi != null && (!File.Exists(lastnfi.FileName) || lastnfi.filePos >= new FileInfo(lastnfi.FileName).Length))
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("Change in length {0}, scan files {1} {2}", lastnfi.FileName, lastnfi.filePos, new FileInfo(lastnfi.FileName).Length));

                    HashSet<string> tlunames = new HashSet<string>(TravelLogUnit.GetAllNames());
                    string[] filenames = Directory.EnumerateFiles(m_watcherfolder, "Journal.*.log", SearchOption.AllDirectories)
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
                    if (nfi.TravelLogUnit.id == 0)
                    {
                        nfi.TravelLogUnit.type = 3;
                        nfi.TravelLogUnit.Add();
                    }

                    netlogpos = nfi.TravelLogUnit.Size;

                    using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
                    {
                        using (DbTransaction txn = cn.BeginTransaction())
                        {
                            foreach (JournalEntry je in nfi.ReadJournalLog())
                            {
                                entries.Add(je);
                                je.Add(cn, txn);
                            }

                            txn.Commit();
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

        private static Guid Win32FolderId_SavedGames = new Guid("4C5C32FF-BB9D-43b0-B5B4-2D72E54EAAA4");
        [DllImport("Shell32.dll")]
        private static extern uint SHGetKnownFolderPath(
            [MarshalAs(UnmanagedType.LPStruct)] Guid rfid,
            uint dwFlags,
            IntPtr hToken,
            out IntPtr pszPath  // API uses CoTaskMemAlloc
        );

    }
}
