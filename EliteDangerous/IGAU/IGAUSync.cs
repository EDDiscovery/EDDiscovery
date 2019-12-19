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

namespace EliteDangerousCore.IGAU
{
    public static class IGAUSync
    {
        private static Thread ThreadIGAUSync;
        private static int running = 0;
        private static bool Exit = false;
        private static ConcurrentQueue<HistoryEntry> hlscanunsyncedlist = new ConcurrentQueue<HistoryEntry>();
        private static AutoResetEvent hlscanevent = new AutoResetEvent(false);
        private static Action<string> logger;

        static public Action<int,string> SentEvents;       // called in thread when sync thread has finished and is terminating

        public static bool SendIGAUEvent(Action<string> log, HistoryEntry helist)
        {
            return SendIGAUEvents(log, new[] { helist });
        }

        public static bool SendIGAUEvents(Action<string> log, params HistoryEntry[] helist)
        {
            return SendIGAUEvents(log, (IEnumerable<HistoryEntry>)helist);
        }

        public static bool SendIGAUEvents(Action<string> log, IEnumerable<HistoryEntry> helist)
        {
            logger = log;

            foreach (HistoryEntry he in helist)
            {
                hlscanunsyncedlist.Enqueue(he);
            }

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

        public static void StopSync()
        {
            Exit = true;
            hlscanevent.Set();
        }

        private static void SyncThread()
        {
            try
            {
                running = 1;
                //mainForm.LogLine("Starting IGAU sync thread");

                while (hlscanunsyncedlist.Count != 0)
                {
                    List<HistoryEntry> hl = new List<HistoryEntry>();
                    HistoryEntry he = null;

                    string recordlist = "";
                    int eventcount = 0;

                    while (hlscanunsyncedlist.TryDequeue(out he))
                    {
                        hlscanevent.Reset();

                        if (IGAUSync.SendToIGAU(he, out bool recordflag))
                        {
                            logger?.Invoke($"Sent {he.EntryType.ToString()} event to IGAU ({he.EventSummary})");
                            if (recordflag)
                            {
                                logger?.Invoke("New IGAU record set");
                                recordlist = recordlist.AppendPrePad(he.System.Name, ";");
                            }

                            eventcount++;
                        }
                        else
                        {
                            logger?.Invoke($"Fail to send {he.EntryType.ToString()} event to IGAU ({he.EventSummary})");

                        }

                        if (hlscanunsyncedlist.Count>1 && hlscanunsyncedlist.Count%10==0)
                        {
                            logger?.Invoke($"{hlscanunsyncedlist.Count.ToString()} events in IGAU queue");
                        }

                        if (Exit)
                        {
                            return;
                        }

                        Thread.Sleep(1000);   // Throttling to 1 per second to not kill IGAU endpoint
                    }

                    SentEvents?.Invoke(eventcount, recordlist);    // tell the system..

                    if (hlscanunsyncedlist.IsEmpty)     // nothing queued
                        hlscanevent.WaitOne(60000);     // Wait up to 60 seconds for another EGO event to come in

                    if (Exit)
                    {
                        return;
                    }
                }

                //mainForm.LogLine("EGO sync thread exiting");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("IGAU Exception ex:" + ex.Message);
                logger?.Invoke("IGAU sync Exception " + ex.Message);
            }
            finally
            {
                running = 0;
            }
        }

        static public bool SendToIGAU(HistoryEntry he, out bool newRecord)
        {
            newRecord = false;

            IGAUClass ego = new IGAUClass();

            JournalEntry je = he.journalEntry;

            if (je == null)
            {
                je = JournalEntry.Get(he.Journalid);
            }

            JObject msg = null;

            if (je.EventTypeID == JournalTypeEnum.Scan)
            {
            //    msg = ego.CreateIGAUMessage(je as JournalScan, he.System.Name, he.System.X, he.System.Y, he.System.Z);
            }

            if (msg != null)
            {
              //  if (IGAU.PostMessage(msg, out newRecord))
              //  {
                    //he.journalEntry.SetIGAUSync();
              //      return true;
              //  }
            }

            return false;
        }

    }
}
