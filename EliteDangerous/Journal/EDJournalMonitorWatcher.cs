/*
 * Copyright © 2016 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 *
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */

using EliteDangerousCore.DB;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Data.Common;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace EliteDangerousCore
{
    // watches a journal for changes, reads it, 

    public class JournalMonitorWatcher
    {
        public string WatcherFolder { get; set; }

        private Dictionary<string, EDJournalReader> netlogreaders = new Dictionary<string, EDJournalReader>();
        private EDJournalReader lastnfi = null;          // last one read..
        private FileSystemWatcher m_Watcher;
        private int ticksNoActivity = 0;
        private ConcurrentQueue<string> m_netLogFileQueue;
        private const string journalfilematch = "Journal*.log";       // this picks up beta and normal logs

        public JournalMonitorWatcher(string folder)
        {
            WatcherFolder = folder;
        }

        #region Scan start stop and monitor

        public void StartMonitor()
        {
            if (m_Watcher == null)
            {
                try
                {
                    m_netLogFileQueue = new ConcurrentQueue<string>();
                    m_Watcher = new System.IO.FileSystemWatcher();
                    m_Watcher.Path = WatcherFolder + Path.DirectorySeparatorChar;
                    m_Watcher.Filter = journalfilematch;
                    m_Watcher.IncludeSubdirectories = false;
                    m_Watcher.NotifyFilter = NotifyFilters.FileName;
                    m_Watcher.Changed += new FileSystemEventHandler(OnNewFile);
                    m_Watcher.Created += new FileSystemEventHandler(OnNewFile);
                    m_Watcher.EnableRaisingEvents = true;

                    System.Diagnostics.Trace.WriteLine("Start Monitor on " + WatcherFolder);
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
                m_Watcher.EnableRaisingEvents = false;
                m_Watcher.Dispose();
                m_Watcher = null;

                System.Diagnostics.Trace.WriteLine("Stop Monitor on " + WatcherFolder);
            }
        }

        // OS calls this when new file is available, add to list

        private void OnNewFile(object sender, FileSystemEventArgs e)        // only picks up new files
        {                                                                   // and it can kick in before any data has had time to be written to it...
            string filename = e.FullPath;
            m_netLogFileQueue.Enqueue(filename);
        }


        // Called by EDJournalClass periodically to scan for journal entries

        public Tuple<List<JournalEntry>,List<UIEvent>> ScanForNewEntries()
        {
            var entries = new List<JournalEntry>();
            var uientries = new List<UIEvent>();

            try
            {
                string filename = null;

                if (lastnfi != null)                            // always give old file another go, even if we are going to change
                {
                    if (!File.Exists(lastnfi.FileName))         // if its been removed, null
                    {
                        lastnfi = null;
                    }
                    else
                    {
                        ScanReader(lastnfi, entries, uientries);

                        if (entries.Count > 0 || uientries.Count > 0 )
                        {
                            ticksNoActivity = 0;
                            return new Tuple<List<JournalEntry>, List<UIEvent>>(entries,uientries);     // feed back now don't change file
                        }
                    }
                }

                if (m_netLogFileQueue.TryDequeue(out filename))      // if a new one queued, we swap to using it
                {
                    lastnfi = OpenFileReader(new FileInfo(filename));
                    System.Diagnostics.Debug.WriteLine(string.Format("Change to scan {0}", lastnfi.FileName));
                    if (lastnfi != null)
                        ScanReader(lastnfi, entries, uientries);   // scan new one
                }
                // every few goes, if its not there or filepos is greater equal to length (so only done when fully up to date)
                else if ( ticksNoActivity >= 30 && (lastnfi == null || lastnfi.filePos >= new FileInfo(lastnfi.FileName).Length))
                {
                    HashSet<string> tlunames = new HashSet<string>(TravelLogUnit.GetAllNames());
                    string[] filenames = Directory.EnumerateFiles(WatcherFolder, journalfilematch, SearchOption.AllDirectories)
                                                  .Select(s => new { name = Path.GetFileName(s), fullname = s })
                                                  .Where(s => !tlunames.Contains(s.name))           // find any new ones..
                                                  .OrderBy(s => s.name)
                                                  .Select(s => s.fullname)
                                                  .ToArray();

                    foreach (var name in filenames)         // for any new filenames..
                    {
                        System.Diagnostics.Debug.WriteLine("No Activity but found new file " + name);
                        lastnfi = OpenFileReader(new FileInfo(name));
                        break;      // stop on first found
                    }

                    if (lastnfi != null)
                        ScanReader(lastnfi, entries , uientries);   // scan new one

                    ticksNoActivity = 0;
                }

                ticksNoActivity++;

                return new Tuple<List<JournalEntry>, List<UIEvent>>(entries, uientries);     // feed back now don't change file
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Net tick exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
                return new Tuple<List<JournalEntry>, List<UIEvent>>(new List<JournalEntry>(), new List<UIEvent>());     // send out empty
            }
        }

        // Called by ScanForNewEntries (from EDJournalClass Scan Tick Worker) to scan a NFI for new entries

        private void ScanReader(EDJournalReader nfi, List<JournalEntry> entries, List<UIEvent> uientries )
        {
            int netlogpos = 0;

            try
            {
                if (nfi.TravelLogUnit.id == 0)
                {
                    nfi.TravelLogUnit.type = TravelLogUnit.JournalType;
                    nfi.TravelLogUnit.Add();
                }

                netlogpos = nfi.TravelLogUnit.Size;

                bool readanything = nfi.ReadJournal(out List<JournalReaderEntry> ents, out List<UIEvent> uie, historyrefreshparsing: false, resetOnError: false );

                uientries.AddRange(uie);

                if (readanything)           // if we read, we must update the travel log pos
                {
                    //System.Diagnostics.Debug.WriteLine("ScanReader " + Path.GetFileName(nfi.FileName) + " read " + ents.Count + " ui " +uientries.Count + " size " + netlogpos);

                    JournalEntry.ExecuteWithInserter(usetxn: true, action: inserter =>
                    {
                        ents = ents.Where(jre => JournalEntry.FindEntry(jre.JournalEntry, jre.Json).Count == 0).ToList();

                        foreach (JournalReaderEntry jre in ents)
                        {
                            entries.Add(jre.JournalEntry);
                            inserter.Add(jre.JournalEntry, jre.Json);
                        }
                    });

                    // System.Diagnostics.Debug.WriteLine("Wrote " + ents.Count() + " to db and updated TLU");

                    nfi.TravelLogUnit.Update();
                }
            }
            catch ( Exception ex )
            {
                System.Diagnostics.Debug.WriteLine("Exception " + ex.Message);
                // Revert and re-read the failed entries
                if (nfi != null && nfi.TravelLogUnit != null)
                {
                    nfi.TravelLogUnit.Size = netlogpos;
                }

                throw;
            }
        }

        #endregion

        #region Called during history refresh, by EDJournalClass, for a reparse.

        public void ParseJournalFiles(Func<bool> cancelRequested, Action<int, string> updateProgress, bool forceReload = false)
        {
//            System.Diagnostics.Trace.WriteLine(BaseUtils.AppTicks.TickCountLap("PJF", true), "Scanned " + WatcherFolder);

            Dictionary<string, TravelLogUnit> m_travelogUnits = TravelLogUnit.GetAll().Where(t => (t.type & TravelLogUnit.TypeMask) == TravelLogUnit.JournalType).GroupBy(t => t.Name).Select(g => g.First()).ToDictionary(t => t.Name);

            // order by file write time so we end up on the last one written
            FileInfo[] allFiles = Directory.EnumerateFiles(WatcherFolder, journalfilematch, SearchOption.AllDirectories).Select(f => new FileInfo(f)).OrderBy(p => p.LastWriteTime).ToArray();

            List<EDJournalReader> readersToUpdate = new List<EDJournalReader>();

            for (int i = 0; i < allFiles.Length; i++)
            {
                FileInfo fi = allFiles[i];

                var reader = OpenFileReader(fi, m_travelogUnits);       // open it

                if (!m_travelogUnits.ContainsKey(reader.TravelLogUnit.Name))
                {
                    m_travelogUnits[reader.TravelLogUnit.Name] = reader.TravelLogUnit;
                    reader.TravelLogUnit.type = TravelLogUnit.JournalType;
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

            //System.Diagnostics.Trace.WriteLine(BaseUtils.AppTicks.TickCountLap("PJF"), "Ready to update");

            for (int i = 0; i < readersToUpdate.Count; i++)
            {
                EDJournalReader reader = readersToUpdate[i];
                updateProgress(i * 100 / readersToUpdate.Count, reader.TravelLogUnit.Name);

                //System.Diagnostics.Trace.WriteLine(BaseUtils.AppTicks.TickCountLap("PJF"), i + " read ");

                reader.ReadJournal(out List<JournalReaderEntry> entries, out List<UIEvent> uievents, historyrefreshparsing: true, resetOnError:true);      // this may create new commanders, and may write to the TLU db

                if (entries.Count > 0)
                {
                    ILookup<DateTime, JournalEntry> existing = JournalEntry.GetAllByTLU(reader.TravelLogUnit.id).ToLookup(e => e.EventTimeUTC);

                    //System.Diagnostics.Trace.WriteLine(BaseUtils.AppTicks.TickCountLap("PJF"), i + " into db");

                    JournalEntry.ExecuteWithInserter(inserter =>
                    {
                        foreach (JournalReaderEntry jre in entries)
                        {
                            if (!existing[jre.JournalEntry.EventTimeUTC].Any(e => JournalEntry.AreSameEntry(jre.JournalEntry, e, ent1jo: jre.Json)))
                            {
                                inserter.Add(jre.JournalEntry, jre.Json);
                                //System.Diagnostics.Trace.WriteLine(string.Format("Write Journal to db {0} {1}", jre.JournalEntry.EventTimeUTC, jre.JournalEntry.EventTypeStr));
                            }
                        }
                    });
                }

                reader.TravelLogUnit.Update();

                updateProgress((i + 1) * 100 / readersToUpdate.Count, reader.TravelLogUnit.Name);

                lastnfi = reader;
            }

            updateProgress(-1, "");
        }

        #endregion

        #region Open

        // open a new file for watching, place it into the netlogreaders list

        private EDJournalReader OpenFileReader(FileInfo fi, Dictionary<string, TravelLogUnit> tlu_lookup = null)
        {
            EDJournalReader reader;
            TravelLogUnit tlu;

            //System.Diagnostics.Trace.WriteLine(string.Format("File Read {0}", fi.FullName));

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

                netlogreaders[fi.Name] = reader;
            }

            return reader;
        }

        #endregion

    }
}