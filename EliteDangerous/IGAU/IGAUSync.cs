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

namespace EliteDangerousCore.IGAU
{
    public static class IGAUSync
    {
        private static Thread ThreadIGAUSync;
        private static int running = 0;
        private static bool Exit = false;
        private static Action<string> logger;
        private static ConcurrentQueue<HistoryEntry> hlscanunsyncedlist = new ConcurrentQueue<HistoryEntry>();
        private static AutoResetEvent hlscanevent = new AutoResetEvent(false);
        static public Action<int> SentEvents;       // called in thread when sync thread has finished and is terminating

        public static bool NewEvent(Action<string> log, HistoryEntry he)
        {
            if (he.EntryType == JournalTypeEnum.CodexEntry)
            {
                System.Diagnostics.Debug.WriteLine("Send to IGAU Codex entry " + he.EventTimeUTC.ToStringZulu());
                logger = log;
                hlscanunsyncedlist.Enqueue(he);
                hlscanevent.Set();

                // Start the sync thread if it's not already running
                if (Interlocked.CompareExchange(ref running, 1, 0) == 0)
                {
                    Exit = false;
                    ThreadIGAUSync = new System.Threading.Thread(new System.Threading.ThreadStart(SyncThread));
                    ThreadIGAUSync.Name = "IGAU Sync";
                    ThreadIGAUSync.IsBackground = true;
                    ThreadIGAUSync.Start();
                }

                return true;
            }
            else
                return false;
        }

        public static void StopSync()
        {
            Exit = true;
            hlscanevent.Set();
        }

        private static void SyncThread()
        {
            running = 1;

            IGAUClass igau = new IGAUClass();

            while (hlscanunsyncedlist.Count != 0)
            {
                HistoryEntry he = null;

                int eventcount = 0;

                while (hlscanunsyncedlist.TryDequeue(out he))
                {
                    try
                    {
                        hlscanevent.Reset();
                        JournalCodexEntry c = he.journalEntry as JournalCodexEntry;
                        if (c.VoucherAmount != null && c.VoucherAmount > 0)
                        {
                            var msg = igau.CreateIGAUMessage(he.EventTimeUTC.ToStringZulu(),
                                                              c.EntryID.ToString(), c.Name, c.Name_Localised, c.System, c.SystemAddress?.ToString() ?? "0");

                            System.Diagnostics.Debug.WriteLine("IGAU Post " + msg.ToString(Newtonsoft.Json.Formatting.Indented));

                            // comment ball the ack in when your ready to try!

                            if (igau.PostMessage(msg))
                            {
                                logger?.Invoke("IGAU Message transmitted " + he.EventTimeUTC.ToStringZulu());
                            }
                        }

                        eventcount++;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine("Error sending event to IGAU:" + ex.Message + Environment.NewLine + ex.StackTrace);
                        logger?.Invoke("IGAU sync Exception " + ex.Message + Environment.NewLine + ex.StackTrace);
                    }

                    if (Exit)
                    {
                        running = 0;
                        return;
                    }

                    Thread.Sleep(1000);   // Throttling to 1 per second to not kill IGAU network
                }

                SentEvents?.Invoke(eventcount);     // tell the system..

                if (hlscanunsyncedlist.IsEmpty)     // if nothing there..
                    hlscanevent.WaitOne(60000);     // Wait up to 60 seconds for another IGAU event to come in

                if (Exit)
                {
                    break;
                }
            }

            running = 0;
        }
    }
}
