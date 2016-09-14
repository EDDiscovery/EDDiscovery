﻿using EDDiscovery2;
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
        private static Guid Win32FolderId_SavedGames = new Guid("4C5C32FF-BB9D-43b0-B5B4-2D72E54EAAA4");
        [DllImport("Shell32.dll")]
        private static extern uint SHGetKnownFolderPath(
            [MarshalAs(UnmanagedType.LPStruct)] Guid rfid,
            uint dwFlags,
            IntPtr hToken,
            out IntPtr pszPath  // API uses CoTaskMemAlloc
        );

        private static string journalPath;

        public delegate void NewSystemEventHandler(VisitedSystemsClass vsc);

        public event NewSystemEventHandler OnNewPosition;          // called in foreground, no need for invoke

        Dictionary<string, EDJournalReader> netlogreaders = new Dictionary<string, EDJournalReader>();

        FileSystemWatcher m_Watcher;
        ConcurrentQueue<string> m_netLogFileQueue;
        System.Windows.Forms.Timer m_scantimer;
        System.ComponentModel.BackgroundWorker m_worker;

        EDJournalReader lastnfi = null;          // last one read..

        private static Regex journalNamePrefixRe = new Regex("(?<prefix>.*)[.]0*(?<part>[0-9][0-9]*)[.]log");


        public EDJournalClass(EDDiscoveryForm ds)
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

        public string GetJournalDir()
        {
            try
            {
                string journaldirstored = EDDConfig.Instance.JournalDir;
                string datapath = journaldirstored;

                if (EDDConfig.Instance.JournalDirAutoMode)
                {
                    if (journalPath == null)
                    {
                        journalPath = GetDefaultJournalDir();
                    }

                    if (journalPath != null)
                    {
                        datapath = journalPath;
                    }

                    if (datapath != journaldirstored)
                    {
                        EDDConfig.Instance.JournalDir = datapath;
                    }
                }
                else
                {
                    datapath = journaldirstored;
                }

                return datapath;
            }
            catch (Exception ex)
            {
                MessageBox.Show("GetJournalDir exception: " + ex.Message);
                return null;
            }
        }

        // called during start up and if refresh history is pressed

        public List<VisitedSystemsClass> ParseFiles(out string error, int defaultMapColour, Func<bool> cancelRequested, Action<int, string> updateProgress, bool forceReload = false)
        {
            error = null;

            string datapath = GetJournalDir();

            if (datapath == null)
            {
                error = "Journal directory not found!" + Environment.NewLine + "Specify location in settings tab";
                return null;
            }

            if (!Directory.Exists(datapath))   // if logfiles directory is not found
            {
                error = "Journal directory is not present!" + Environment.NewLine + "Specify location in settings tab";
                return null;
            }

            List<VisitedSystemsClass> vsSystemsList = VisitedSystemsClass.GetAll(EDDConfig.Instance.CurrentCmdrID);

            List<VisitedSystemsClass> visitedSystems = new List<VisitedSystemsClass>();
            Dictionary<string, TravelLogUnit> m_travelogUnits = TravelLogUnit.GetAll().Where(t => t.type == 3).GroupBy(t => t.Name).Select(g => g.First()).ToDictionary(t => t.Name);
            Dictionary<string, List<VisitedSystemsClass>> vsc_lookup = VisitedSystemsClass.GetAll().GroupBy(v => v.Unit).ToDictionary(g => g.Key, g => g.ToList());

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
            FileInfo[] allFiles = Directory.EnumerateFiles(datapath, "Journal.*.log", SearchOption.AllDirectories).Select(f => new FileInfo(f)).OrderBy(p => p.LastWriteTime).ToArray();

            List<EDJournalReader> readersToUpdate = new List<EDJournalReader>();

            for (int i = 0; i < allFiles.Length; i++)
            {
                FileInfo fi = allFiles[i];

                var reader = OpenFileReader(fi, m_travelogUnits, vsc_lookup);

                if (!m_travelogUnits.ContainsKey(reader.TravelLogUnit.Name))
                {
                    m_travelogUnits[reader.TravelLogUnit.Name] = reader.TravelLogUnit;
                    reader.TravelLogUnit.type = 3;
                    reader.TravelLogUnit.Add();
                }

                if (!netlogreaders.ContainsKey(reader.TravelLogUnit.Name))
                {
                    netlogreaders[reader.TravelLogUnit.Name] = lastnfi;
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
                            if (je is JournalEvents.JournalLocOrJump)
                            {
                                JournalEvents.JournalLocOrJump jl = (JournalEvents.JournalLocOrJump)je;
                                VisitedSystemsClass vsc = new VisitedSystemsClass();
                                vsc.EDSM_sync = false;
                                vsc.MapColour = defaultMapColour;
                                vsc.Commander = (reader.Commander ?? EDDConfig.Instance.CurrentCommander).Nr;
                                vsc.Name = jl.StarSystem;
                                vsc.X = jl.StarPos.X;
                                vsc.Y = jl.StarPos.Y;
                                vsc.Z = jl.StarPos.Z;
                                vsc.Unit = reader.TravelLogUnit.Name;
                                vsc.Source = reader.TravelLogUnit.id;
                                vsc.Time = je.EventTimeLocal;
                                vsc.Add(cn, tn);
                                visitedSystems.Add(vsc);
                            }

                            // TODO: Save journal entry to database
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

            return visitedSystems;
        }

        private EDJournalReader OpenFileReader(FileInfo fi, Dictionary<string, TravelLogUnit> tlu_lookup = null, Dictionary<string, List<VisitedSystemsClass>> vsc_lookup = null)
        {
            EDJournalReader reader;
            TravelLogUnit tlu;
            List<VisitedSystemsClass> vsclist = null;

            if (vsc_lookup != null && vsc_lookup.ContainsKey(fi.Name))
            {
                vsclist = vsc_lookup[fi.Name];
            }

            if (netlogreaders.ContainsKey(fi.Name))
            {
                reader = netlogreaders[fi.Name];
            }
            else if (tlu_lookup != null && tlu_lookup.ContainsKey(fi.Name))
            {
                tlu = tlu_lookup[fi.Name];
                tlu.Path = fi.DirectoryName;
                reader = new EDJournalReader(tlu, vsclist);
                netlogreaders[fi.Name] = reader;
            }
            else if (TravelLogUnit.TryGet(fi.Name, out tlu))
            {
                tlu.Path = fi.DirectoryName;
                reader = new EDJournalReader(tlu, vsclist);
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
            Debug.Assert(Application.MessageLoop);              // ensure.. paranoia

            if (m_Watcher == null)
            {
                try
                {
                    string logpath = GetJournalDir();

                    if (Directory.Exists(logpath))
                    {
                        m_netLogFileQueue = new ConcurrentQueue<string>();
                        m_Watcher = new System.IO.FileSystemWatcher();
                        m_Watcher.Path = logpath + Path.DirectorySeparatorChar;
                        m_Watcher.Filter = "Journal.*.log";
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
                List<VisitedSystemsClass> entries = (List<VisitedSystemsClass>)e.Result;

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
            var entries = new List<VisitedSystemsClass>();
            e.Result = entries;
            int netlogpos = 0;
            EDJournalReader nfi = null;

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
                else if (!File.Exists(lastnfi.FileName) || lastnfi.filePos >= new FileInfo(lastnfi.FileName).Length)
                {
                    HashSet<string> tlunames = new HashSet<string>(TravelLogUnit.GetAllNames());
                    string[] filenames = Directory.EnumerateFiles(GetJournalDir(), "Journal.*.log", SearchOption.AllDirectories)
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

                    foreach (JournalEntry je in nfi.ReadJournalLog())
                    {
                        if (je is JournalEvents.JournalLocOrJump)
                        {
                            JournalEvents.JournalLocOrJump jl = (JournalEvents.JournalLocOrJump)je;
                            VisitedSystemsClass vsc = new VisitedSystemsClass();
                            vsc.EDSM_sync = false;
                            vsc.MapColour = EDDConfig.Instance.DefaultMapColour;
                            vsc.Commander = (nfi.Commander ?? EDDConfig.Instance.CurrentCommander).Nr;
                            vsc.Name = jl.StarSystem;
                            vsc.X = jl.StarPos.X;
                            vsc.Y = jl.StarPos.Y;
                            vsc.Z = jl.StarPos.Z;
                            vsc.Unit = nfi.TravelLogUnit.Name;
                            vsc.Source = nfi.TravelLogUnit.id;
                            vsc.Time = jl.EventTimeLocal;
                            vsc.Add();

                            // here we need to make sure the cursystem is set up.. need to do it here because OnNewPosition expects all cursystems to be non null..

                            VisitedSystemsClass item2 = VisitedSystemsClass.GetLast(vsc.Commander, vsc.Time);
                            VisitedSystemsClass.UpdateVisitedSystemsEntries(vsc, item2, EDDiscoveryForm.EDDConfig.UseDistances);       // ensure they have system classes behind them..
                            entries.Add(vsc);

                            if (worker.CancellationPending)
                            {
                                break;
                            }
                        }

                        // TODO: Save journal entry to database
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
