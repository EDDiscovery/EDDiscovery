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
using System.Threading;
using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;

namespace EliteDangerousCore.EDDN
{
    public static class EDDNSync
    {
        private static Thread ThreadEDDNSync;
        private static int _running = 0;
        private static bool Exit = false;
        private static ConcurrentQueue<HistoryEntry> hlscanunsyncedlist = new ConcurrentQueue<HistoryEntry>();
        private static AutoResetEvent hlscanevent = new AutoResetEvent(false);
        private static Action<string> logger;

        public static bool SendEDDNEvent(Action<string> logger, HistoryEntry helist)
        {
            return SendEDDNEvents(logger, new[] { helist });
        }

        public static bool SendEDDNEvents(Action<string> logger, params HistoryEntry[] helist)
        {
            return SendEDDNEvents(logger, (IEnumerable<HistoryEntry>)helist);
        }

        public static bool SendEDDNEvents(Action<string> log, IEnumerable<HistoryEntry> helist)
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
                ThreadEDDNSync = new System.Threading.Thread(new System.Threading.ThreadStart(SyncThread));
                ThreadEDDNSync.Name = "EDDN Sync";
                ThreadEDDNSync.IsBackground = true;
                ThreadEDDNSync.Start();
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
            try
            {
                _running = 1;
                //mainForm.LogLine("Starting EDDN sync thread");

                while (hlscanunsyncedlist.Count != 0)
                {
                    List<HistoryEntry> hl = new List<HistoryEntry>();
                    HistoryEntry he = null;

                    while (hlscanunsyncedlist.TryDequeue(out he))
                    {
                        hlscanevent.Reset();

                        TimeSpan age = he.AgeOfEntry();

                        if (age.Days >= 1 && he.EntryType != EliteDangerousCore.JournalTypeEnum.Scan)
                        {
                            System.Diagnostics.Debug.WriteLine("EDDN: Ignoring entry due to age");
                        }
                        else if (EDDNSync.SendToEDDN(he))
                        {
                            logger?.Invoke($"Sent {he.EntryType.ToString()} event to EDDN ({he.EventSummary})");
                        }

                        if (Exit)
                        {
                            return;
                        }

                        Thread.Sleep(1000);   // Throttling to 1 per second to not kill EDDN network
                    }

                    // Wait up to 60 seconds for another EDDN event to come in
                    hlscanevent.WaitOne(60000);
                    if (Exit)
                    {
                        return;
                    }
                }

                //mainForm.LogLine("EDDN sync thread exiting");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception ex:" + ex.Message);
                logger?.Invoke("EDDN sync Exception " + ex.Message);
            }
            finally
            {
                _running = 0;
            }
        }

        static public bool SendToEDDN(HistoryEntry he)
        {
            EDDNClass eddn = new EDDNClass();

            if (he.Commander != null)
            {
                eddn.commanderName = he.Commander.EdsmName;
                if (string.IsNullOrEmpty(eddn.commanderName))
                    eddn.commanderName = he.Commander.Name;

                if (he.Commander.Name.StartsWith("[BETA]", StringComparison.InvariantCultureIgnoreCase))
                    eddn.isBeta = true;
            }

            if (he.IsBetaMessage)
                eddn.isBeta = true;

            JournalEntry je = he.journalEntry;

            if (je == null)
            {
                je = JournalEntry.Get(he.Journalid);
            }

            JObject msg = null;

            if (je.EventTypeID == JournalTypeEnum.FSDJump)
            {
                msg = eddn.CreateEDDNMessage(je as JournalFSDJump);

            }
            else if (je.EventTypeID == JournalTypeEnum.Docked)
            {
                msg = eddn.CreateEDDNMessage(je as JournalDocked, he.System.x, he.System.y, he.System.z);
            }
            else if (je.EventTypeID == JournalTypeEnum.Scan)
            {
                msg = eddn.CreateEDDNMessage(je as JournalScan, he.System.name, he.System.x, he.System.y, he.System.z);
            }

            if (msg != null)
            {
                if (eddn.PostMessage(msg))
                {
                    he.SetEddnSync();
                    return true;
                }
            }

            return false;
        }

    }
}
