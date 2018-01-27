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
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace EliteDangerousCore
{
    public class EDJournalClass
    {
        public delegate void NewJournalEntryHandler(JournalEntry je);
        public event NewJournalEntryHandler OnNewJournalEntry;

        private Thread ScanThread;
        private ManualResetEvent StopRequested;
        private Action<Action> InvokeAsyncOnUiThread;
        private List<JournalMonitorWatcher> watchers = new List<JournalMonitorWatcher>();
        private string frontierfolder;

        public EDJournalClass(Action<Action> invokeAsyncOnUiThread)
        {
            InvokeAsyncOnUiThread = invokeAsyncOnUiThread;
            frontierfolder = GetDefaultJournalDir();
        }

        public static string GetDefaultJournalDir()
        {
            string path;

            // Windows Saved Games path (Vista and above)
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 6)
            {
                IntPtr pszPath;
                if (BaseUtils.Win32.UnsafeNativeMethods.SHGetKnownFolderPath(Win32FolderId_SavedGames, 0, IntPtr.Zero, out pszPath) == 0)
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

        public string GetWatchFolder(string d)
        {
            return (d == null || d.Length == 0) ? frontierfolder : d;
        }

        public void ParseJournalFiles(Func<bool> cancelRequested, Action<int, string> updateProgress, bool forceReload = false)
        {
            List<EDCommander> listCommanders = EDCommander.GetList();

            if (frontierfolder != null && frontierfolder.Length != 0 && Directory.Exists(frontierfolder))
            {
                if (watchers.FindIndex(x => x.m_watcherfolder.Equals(frontierfolder)) < 0)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("New watch on {0}", frontierfolder));
                    JournalMonitorWatcher mw = new JournalMonitorWatcher(frontierfolder);
                    watchers.Add(mw);
                }
            }

            for (int i = 0; i < listCommanders.Count; i++)             // see if new watchers are needed
            {
                string datapath = GetWatchFolder(listCommanders[i].JournalDir);

                if (datapath == null || datapath.Length == 0 || !Directory.Exists(datapath))
                    continue;

                if (watchers.FindIndex(x => x.m_watcherfolder.Equals(datapath)) >= 0)       // if we already have a watch on this folder..
                    continue;       // already done

                System.Diagnostics.Trace.WriteLine(string.Format("New watch on {0}", datapath));
                JournalMonitorWatcher mw = new JournalMonitorWatcher(datapath);
                watchers.Add(mw);
            }

            List<int> tobedeleted = new List<int>();
            for (int i = 0; i < watchers.Count; i++)
            {
                bool found = false;
                for (int j = 0; j < listCommanders.Count; j++)          // all commanders, see if this watch folder is present
                    found |= watchers[i].m_watcherfolder.Equals(GetWatchFolder(listCommanders[j].JournalDir));

                if (!found)
                    tobedeleted.Add(i);
            }

            foreach (int i in tobedeleted)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("Delete watch on {0}", watchers[i].m_watcherfolder));
                JournalMonitorWatcher mw = watchers[i];
                mw.StopMonitor();          // just in case
                watchers.Remove(mw);
            }

            for (int i = 0; i < watchers.Count; i++)             // parse files of all folders being watched
            {
                watchers[i].ParseJournalFiles(cancelRequested, updateProgress, forceReload);     // may create new commanders at the end, but won't need any new watchers, because they will obv be in the same folder
            }
        }

        public void StartMonitor()
        {
            StopRequested = new ManualResetEvent(false);
            ScanThread = new Thread(ScanThreadProc) { Name = "Journal Monitor Thread", IsBackground = true };
            ScanThread.Start();

            foreach (JournalMonitorWatcher mw in watchers)
            {
                mw.StartMonitor();
            }
        }

        public void StopMonitor()
        {
            foreach (JournalMonitorWatcher mw in watchers)
            {
                mw.StopMonitor();
            }

            if (StopRequested != null)
            {
                StopRequested.Set();
                StopRequested = null;
            }

            if (ScanThread != null)
            {
                ScanThread.Join();
                ScanThread = null;
            }
        }

        private void ScanThreadProc()
        {
            ManualResetEvent stopRequested = StopRequested;

            while (!stopRequested.WaitOne(250))
            {
                List<JournalEntry> jl = ScanTickWorker(() => stopRequested.WaitOne(0));

                if (jl != null && jl.Count != 0 && !stopRequested.WaitOne(0))
                {
                    InvokeAsyncOnUiThread(() => ScanTickDone(jl));
                }
            }
        }

        private List<JournalEntry> ScanTickWorker(Func<bool> stopRequested)
        {
            var entries = new List<JournalEntry>();

            foreach (JournalMonitorWatcher mw in watchers)
            {
                entries.AddRange(mw.ScanForNewEntries());

                if (stopRequested())
                {
                    return null;
                }
            }

            return entries;
        }

        private void ScanTickDone(List<JournalEntry> entries)
        {
            if (entries != null)
            {
                foreach (var ent in entries)                    // pass them to the handler
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("New entry {0} {1}", ent.EventTimeUTC, ent.EventTypeStr));
                    if (OnNewJournalEntry != null)
                        OnNewJournalEntry(ent);
                }
            }
        }

        private static Guid Win32FolderId_SavedGames = new Guid("4C5C32FF-BB9D-43b0-B5B4-2D72E54EAAA4");
    }
}
