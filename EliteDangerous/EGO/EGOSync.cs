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

 using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;

namespace EDDiscoveryCore.EGO
{
    public static class EGOSync
    {
        private static Thread ThreadEGOSync;
        private static int _running = 0;
        private static bool Exit = false;
        private static ConcurrentQueue<HistoryEntry> hlscanunsyncedlist = new ConcurrentQueue<HistoryEntry>();
        private static AutoResetEvent hlscanevent = new AutoResetEvent(false);
        private static Action<string> logger;

        public static bool SendEGOEvent(Action<string> log, HistoryEntry helist)
        {
            return SendEGOEvents(log, new[] { helist });
        }

        public static bool SendEGOEvents(Action<string> log, params HistoryEntry[] helist)
        {
            return SendEGOEvents(log, (IEnumerable<HistoryEntry>)helist);
        }

        public static bool SendEGOEvents(Action<string> log, IEnumerable<HistoryEntry> helist)
        {
            logger = log;

            foreach (HistoryEntry he in helist)
            {
                hlscanunsyncedlist.Enqueue(he);
            }

            hlscanevent.Set();

            // Start the sync thread if it's not already running
            if (Interlocked.CompareExchange(ref _running, 1, 0) == 0)
            {
                Exit = false;
                ThreadEGOSync = new System.Threading.Thread(new System.Threading.ThreadStart(SyncThread));
                ThreadEGOSync.Name = "EGO Sync";
                ThreadEGOSync.IsBackground = true;
                ThreadEGOSync.Start();
            }

            return true;
        }

        public static void StopSync()
        {
            Exit = true;
            hlscanevent.Set();
        }

        private static void SyncThread()
        {
            bool newRecord = false;
            try
            {
                _running = 1;
                //mainForm.LogLine("Starting EGO sync thread");

                while (hlscanunsyncedlist.Count != 0)
                {
                    List<HistoryEntry> hl = new List<HistoryEntry>();
                    HistoryEntry he = null;

                    while (hlscanunsyncedlist.TryDequeue(out he))
                    {
                        hlscanevent.Reset();
                        newRecord = false;

                        if (EGOSync.SendToEGO(he, ref newRecord))
                        {
                            logger?.Invoke($"Sent {he.EntryType.ToString()} event to EGO ({he.EventSummary})");
                            if (newRecord) { logger?.Invoke("New EGO record set"); }
                        }
                        else
                        {
                            logger?.Invoke($"Fail to send {he.EntryType.ToString()} event to EGO ({he.EventSummary})");

                        }

                        if (hlscanunsyncedlist.Count>1 && hlscanunsyncedlist.Count%10==0)
                        {
                            logger?.Invoke($"{hlscanunsyncedlist.Count.ToString()} events in EGO queue");
                        }

                        if (Exit)
                        {
                            return;
                        }

                        if (DateTime.UtcNow < new DateTime(2017,8,18))  // Temporary slow down EGO sync more...
                            Thread.Sleep(5000);   // Throttling to 1 per second to not kill EGO network
                        else
                            Thread.Sleep(1000);   // Throttling to 1 per second to not kill EGO network

                    }

                    // Wait up to 60 seconds for another EGO event to come in
                    hlscanevent.WaitOne(60000);
                    if (Exit)
                    {
                        return;
                    }
                }

                //mainForm.LogLine("EGO sync thread exiting");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception ex:" + ex.Message);
                logger?.Invoke("EGO sync Exception " + ex.Message);
            }
            finally
            {
                _running = 0;
            }
        }

        static public bool SendToEGO(HistoryEntry he, ref bool newRecord)
        {
            EGOClass ego = new EGOClass();
            

            if (he.Commander != null)
            {
                ego.commanderName = he.Commander.EdsmName;
                if (string.IsNullOrEmpty(ego.commanderName))
                    ego.commanderName = he.Commander.Name;
            }

            JournalEntry je = he.journalEntry;

            if (je == null)
            {
                je = JournalEntry.Get(he.Journalid);
            }

            JObject msg = null;

            if (je.EventTypeID == JournalTypeEnum.Scan)
            {
                msg = ego.CreateEGOMessage(je as JournalScan, he.System.name, he.System.x, he.System.y, he.System.z);
            }

            if (msg != null)
            {
                if (ego.PostMessage(msg, ref newRecord))
                {
                    he.SetEGOSync();
                    return true;
                }
            }

            return false;
        }

    }
}
