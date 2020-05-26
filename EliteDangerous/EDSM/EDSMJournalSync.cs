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
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using EliteDangerousCore.DB;

namespace EliteDangerousCore.EDSM
{
    public static class EDSMJournalSync
    {
        private class HistoryQueueEntry
        {
            public bool ManualSync = false;
            public Action<string> Logger;
            public HistoryEntry HistoryEntry;
        }

        static public Action<int,string> SentEvents;       // called in thread when sync thread has finished and is terminating. first discovery list

        private static Thread ThreadEDSMSync;
        private static int running = 0;
        private static bool Exit = false;
        private static ConcurrentQueue<HistoryQueueEntry> historylist = new ConcurrentQueue<HistoryQueueEntry>();
        private static AutoResetEvent historyevent = new AutoResetEvent(false);
        private static ManualResetEvent exitevent = new ManualResetEvent(false);
        private static DateTime lastDiscardFetch;
        private static int maxEventsPerMessage = 200;
        private static HashSet<string> discardEvents = new HashSet<string>
        { // Default events to discard
            "ShutDown",
            "Fileheader",
            "NewCommander",
            "ClearSavedGame",
            "Music",
            "Continued",
            "Passengers",
            "DockingCancelled",
            "DockingDenied",
            "DockingGranted",
            "DockingRequested",
            "DockingTimeout",
            "StartJump",
            "Touchdown",
            "Liftoff",
            "ApproachSettlement",
            "NavBeaconScan",
            "Scan",
            "SupercruiseEntry",
            "SupercruiseExit",
            "Scanned",
            "DataScanned",
            "DatalinkScan",
            "EngineerApply",
            "FactionKillBond",
            "Bounty",
            "DatalinkVoucher",
            "SystemsShutdown",
            "EscapeInterdiction",
            "HeatDamage",
            "HeatWarning",
            "HullDamage",
            "ShieldState",
            "FuelScoop",
            "MaterialDiscovered",
            "Screenshot",
            "CrewAssign",
            "CrewFire",
            "ShipyardNew",
            "MassModuleStore",
            "ModuleStore",
            "ModuleSwap",
            "PowerplayVote",
            "PowerplayVoucher",
            "AfmuRepairs",
            "CockpitBreached",
            "ChangeCrewRole",
            "CrewLaunchFighter",
            "CrewMemberJoins",
            "CrewMemberQuits",
            "CrewMemberRoleChange",
            "KickCrewMember",
            "EndCrewSession",
            "LaunchFighter",
            "DockFighter",
            "VehicleSwitch",
            "LaunchSRV",
            "DockSRV",
            "JetConeBoost",
            "JetConeDamage",
            "RebootRepair",
            "RepairDrone",
            "WingAdd",
            "WingInvite",
            "WingJoin",
            "WingLeave",
            "ReceiveText",
            "SendText",
        };
        private static HashSet<string> alwaysDiscard = new HashSet<string>
        { // Discard spammy events
            "CommunityGoal",
            "ReceiveText",
            "SendText",
            "FuelScoop",
            "Friends",
            "UnderAttack",
            "FSDTarget"     // disabled 28/2/2019 due to it creating a system entry and preventing systemcreated from working
        };
        private static HashSet<JournalTypeEnum> holdEvents = new HashSet<JournalTypeEnum>
        {
            JournalTypeEnum.Cargo,
            JournalTypeEnum.Loadout,
            JournalTypeEnum.Materials,
            JournalTypeEnum.LoadGame,
            JournalTypeEnum.Rank,
            JournalTypeEnum.Progress,
            JournalTypeEnum.ShipyardBuy,
            JournalTypeEnum.ShipyardNew,
            JournalTypeEnum.ShipyardSwap
        };

        private static JObject RemoveCommonKeys(JObject obj)
        {
            foreach (JProperty prop in obj.Properties().ToList())
            {
                if (prop.Name.StartsWith("EDD"))
                {
                    obj.Remove(prop.Name);
                }
            }

            return obj;
        }

        public static void StopSync()
        {
            Exit = true;
            exitevent.Set();
            historyevent.Set();     // also trigger in case we are in thread hold
        }

        public static void UpdateDiscardList()
        {
            if (lastDiscardFetch < DateTime.UtcNow.AddMinutes(-120))     // check if we need a new discard list
            {
                try
                {
                    EDSMClass edsm = new EDSMClass();
                    var newdiscardEvents = new HashSet<string>(edsm.GetJournalEventsToDiscard());

                    lock (alwaysDiscard)        // use this as a perm proxy to lock discardEvents
                    {
                        discardEvents = newdiscardEvents;
                    }

                    lastDiscardFetch = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine($"Unable to retrieve events to be discarded: {ex.ToString()}");
                }
            }
        }

        // called by onNewEntry

        public static bool SendEDSMEvents(Action<string> logger, params HistoryEntry[] helist)
        {
            return SendEDSMEvents(logger, (IEnumerable<HistoryEntry>)helist);
        }

        // Called by Perform, by Sync, by above.

        public static bool SendEDSMEvents(Action<string> log, IEnumerable<HistoryEntry> helist, bool manual = false)
        {
            System.Diagnostics.Debug.WriteLine("Send " + helist.Count());

            int eventCount = 0;
            bool hasbeta = false;
            DateTime betatime = DateTime.MinValue;

            lock (alwaysDiscard)        // use this as a perm proxy to lock discardEvents
            {
                foreach (HistoryEntry he in helist)     // push list of events to historylist queue..
                {
                    if (!he.EdsmSync)     // if we have not sent it..
                    {
                        string eventtype = he.EntryType.ToString();

                        if (he.Commander.Name.StartsWith("[BETA]", StringComparison.InvariantCultureIgnoreCase) || he.IsBetaMessage)
                        {
                            hasbeta = true;
                            betatime = he.EventTimeUTC;
                            he.journalEntry.SetEdsmSync();       // crappy slow but unusual, but lets mark them as sent..
                        }
                        else if (!(he.MultiPlayer || discardEvents.Contains(eventtype) || alwaysDiscard.Contains(eventtype)))
                        {
                            historylist.Enqueue(new HistoryQueueEntry { HistoryEntry = he, Logger = log, ManualSync = manual });
                            eventCount++;
                        }
                    }
                }
            }

            if (hasbeta && eventCount == 0)
            {
                log?.Invoke($"Cannot send Beta logs to EDSM - most recent timestamp: {betatime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'")}");
            }

            if (manual )    // if in manual mode, we want to tell the user the end, so push an end marker
            {
                string logline = (eventCount > 0) ? $"Sending {eventCount} journal event(s) to EDSM" : "No new events to send to EDSM";
                log?.Invoke(logline);

                if ( eventCount>0)      // push end of sync event.
                    historylist.Enqueue(new HistoryQueueEntry { HistoryEntry = null, Logger = log, ManualSync = manual });      
            }

            if (eventCount > 0)
            {
                historyevent.Set();

                // Start the sync thread if it's not already running
                if (Interlocked.CompareExchange(ref running, 1, 0) == 0)
                {
                    Exit = false;
                    exitevent.Reset();
                    ThreadEDSMSync = new System.Threading.Thread(new System.Threading.ThreadStart(SyncThread));
                    ThreadEDSMSync.Name = "EDSM Journal Sync";
                    ThreadEDSMSync.IsBackground = true;
                    ThreadEDSMSync.Start();
                }
            }

            return true;
        }

        private static void SyncThread()
        {
            try
            {
                UpdateDiscardList();

                running = 1;

                int manualcount = 0;

                while (historylist.Count != 0)      // while stuff to send
                {
                    HistoryQueueEntry hqe = null;

                    if (historylist.TryDequeue(out hqe))        // next history event...
                    {
                        HistoryEntry first = hqe.HistoryEntry;
                        bool manual = hqe.ManualSync;

                        if (!manual)
                        {
                            manualcount = 0;
                        }
                        else if (first == null)
                        {
                            hqe.Logger?.Invoke("Manual EDSM journal sync complete");
                            continue; // Discard end-of-sync event
                        }
                        else if (discardEvents.Contains(first.EntryType.ToString()))
                        {
                            continue;
                        }
                        else
                        {
                            manualcount++;
                        }

                        historyevent.Reset();
                        Action<string> logger = hqe.Logger;

                        List<HistoryEntry> hl = new List<HistoryEntry>() { first };

                        string logline = $"Adding {first.EntryType.ToString()} event to EDSM journal sync ({first.EventSummary})";
                        System.Diagnostics.Trace.WriteLine(logline);
                        //removed too verbose if (!manual)  hqe.Logger?.Invoke(logline);

                        if (holdEvents.Contains(first.EntryType) || (first.EntryType == JournalTypeEnum.Location && first.IsDocked))
                        {
                            System.Diagnostics.Debug.WriteLine("Holding for another event");

                            if (historylist.IsEmpty)
                            {
                                historyevent.WaitOne(20000); // Wait up to 20 seconds for another entry to come through
                            }
                        }

                        while (hl.Count < maxEventsPerMessage && historylist.TryPeek(out hqe)) // Leave event in queue if commander changes
                        {
                            HistoryEntry he = hqe.HistoryEntry;
                            if (he == null || he.Commander != first.Commander || hqe.ManualSync != manual)
                            {
                                break;
                            }

                            historylist.TryDequeue(out hqe);
                            historyevent.Reset();

                            if (hqe.HistoryEntry != null && discardEvents.Contains(hqe.HistoryEntry.EntryType.ToString()))
                            {
                                continue;
                            }

                            logline = $"Adding {he.EntryType.ToString()} event to EDSM journal sync ({he.EventSummary})";
                            System.Diagnostics.Trace.WriteLine(logline);

                            if (manual)
                                manualcount++;
                            else
                                hqe.Logger?.Invoke(logline);

                            hl.Add(he);

                            if ((holdEvents.Contains(he.EntryType) || (he.EntryType == JournalTypeEnum.Location && he.IsDocked)) && historylist.IsEmpty)
                            {
                                historyevent.WaitOne(20000); // Wait up to 20 seconds for another entry to come through
                            }

                            if (Exit)
                            {
                                return;
                            }
                        }

                        int sendretries = 5;
                        int waittime = 30000;
                        string firstdiscovery = "";

                        while (sendretries > 0 && !SendToEDSM(hl, first.Commander, out string errmsg, out firstdiscovery))
                        {
                            logger?.Invoke($"Error sending EDSM events {errmsg}");
                            System.Diagnostics.Trace.WriteLine($"Error sending EDSM events {errmsg}");
                            exitevent.WaitOne(waittime); // Wait and retry
                            if (Exit)
                            {
                                return;
                            }
                            sendretries--;
                            waittime *= 2; // Double back-off time, up to 8 minutes between tries or 15.5 minutes total
                        }

                        if (sendretries == 0)
                        {
                            logger?.Invoke("Unable to send events - giving up");
                            System.Diagnostics.Trace.WriteLine("Unable to send events - giving up");
                        }
                        else
                        {
                            if (manual)
                            {
                                logger?.Invoke($"Sent {manualcount} events to EDSM so far for commander {first.Commander.Name}");
                            }

                            SentEvents?.Invoke(hl.Count,firstdiscovery);       // finished sending everything, tell..
                        }
                    }

                    // Wait at least N between messages
                    exitevent.WaitOne(100);

                    if (Exit)
                        return;

                    if (historylist.IsEmpty)
                        historyevent.WaitOne(120000);       // wait for another event keeping the thread open.. Note stop also sets this

                    if (Exit)
                        return;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception ex:" + ex.Message);
            }
            finally
            {
                running = 0;
            }
        }

        static private bool SendToEDSM(List<HistoryEntry> hl, EDCommander cmdr, out string errmsg , out string firstdiscovers )
        {
            EDSMClass edsm = new EDSMClass(cmdr);       // Ensure we use the commanders EDSM credentials.
            errmsg = null;
            firstdiscovers = "";

            List<JObject> entries = new List<JObject>();

            foreach (HistoryEntry he in hl)
            {
                JournalEntry je = he.journalEntry;

                if (je == null)
                {
                    je = JournalEntry.Get(he.Journalid);
                }

                JObject json = je.GetJson();

                if (json == null)
                {
                    continue;
                }

                RemoveCommonKeys(json);
                if (je.EventTypeID == JournalTypeEnum.FSDJump && json["FuelUsed"].Empty())
                    json["_convertedNetlog"] = true;
                if (json["StarPosFromEDSM"].Bool(false)) // Remove star pos from EDSM
                    json.Remove("StarPos");
                json.Remove("StarPosFromEDSM");
                json["_systemName"] = he.System.Name;
                json["_systemCoordinates"] = new JArray(he.System.X, he.System.Y, he.System.Z);
                if (he.System.SystemAddress != null)
                    json["_systemAddress"] = he.System.SystemAddress;
                if (he.IsDocked)
                {
                    json["_stationName"] = he.WhereAmI;
                    if (he.MarketID != null)
                        json["_stationMarketId"] = he.MarketID;
                }
                json["_shipId"] = he.ShipId;
                entries.Add(json);
            }

            List<JObject> results = edsm.SendJournalEvents(entries, out errmsg);

            if (results == null)
            {
                return false;
            }
            else
            {
                firstdiscovers = UserDatabase.Instance.ExecuteWithDatabase<string>(cn =>
                {
                    string firsts = "";
                    using (var txn = cn.Connection.BeginTransaction())
                    {
                        for (int i = 0; i < hl.Count && i < results.Count; i++)
                        {
                            HistoryEntry he = hl[i];
                            JObject result = results[i];
                            int msgnr = result["msgnum"].Int();
                            int systemId = result["systemId"].Int();

                            if ((msgnr >= 100 && msgnr < 200) || msgnr == 500)
                            {
                                if (he.IsLocOrJump)
                                {
                                    if (systemId != 0)
                                    {
                                        he.System.EDSMID = systemId;
                                        JournalEntry.UpdateEDSMIDPosJump(he.Journalid, he.System, false, 0, cn.Connection, txn);
                                    }
                                }

                                if (he.EntryType == JournalTypeEnum.FSDJump)       // only on FSD, confirmed with Anthor.  25/4/2018
                                {
                                    bool systemCreated = result["systemCreated"].Bool();

                                    if (systemCreated)
                                    {
                                        System.Diagnostics.Debug.WriteLine("** EDSM indicates first entry for " + he.System.Name);
                                        (he.journalEntry as JournalFSDJump).UpdateFirstDiscover(true, cn.Connection, txn);
                                        firsts = firsts.AppendPrePad(he.System.Name, ";");
                                    }
                                }

                                he.journalEntry.SetEdsmSync(cn.Connection, txn);

                                if (msgnr == 500)
                                {
                                    System.Diagnostics.Trace.WriteLine($"Warning submitting event {he.Journalid} \"{he.EventSummary}\": {msgnr} {result["msg"].Str()}");
                                }
                            }
                            else
                            {
                                System.Diagnostics.Trace.WriteLine($"Error submitting event {he.Journalid} \"{he.EventSummary}\": {msgnr} {result["msg"].Str()}");
                            }

                        }

                        txn.Commit();
                        return firsts;
                    }
                });

                return true;
            }
        }
    }
}
