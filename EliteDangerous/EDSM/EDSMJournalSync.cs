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

        private static Thread ThreadEDSMSync;
        private static int _running = 0;
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

        public static bool SendEDSMEvent(Action<string> logger, HistoryEntry helist)
        {
            return SendEDSMEvents(logger, new[] { helist });
        }

        public static bool SendEDSMEvents(Action<string> logger, params HistoryEntry[] helist)
        {
            return SendEDSMEvents(logger, (IEnumerable<HistoryEntry>)helist);
        }

        public static bool SendEDSMEvents(Action<string> log, IEnumerable<HistoryEntry> helist, bool verbose = false, bool manual = false)
        {
            int eventCount = 0;
            foreach (HistoryEntry he in helist)
            {
                if (ShouldSendEvent(he))
                {
                    historylist.Enqueue(new HistoryQueueEntry { HistoryEntry = he, Logger = verbose ? log : null, ManualSync = manual });
                    eventCount++;
                }
            }

            if (manual && eventCount != 0)
            {
                string logline = $"Sending {eventCount} journal event(s) to EDSM";
                log?.Invoke(logline);
                historylist.Enqueue(new HistoryQueueEntry { HistoryEntry = null, Logger = log, ManualSync = manual });
            }

            historyevent.Set();

            // Start the sync thread if it's not already running
            if (Interlocked.CompareExchange(ref _running, 1, 0) == 0)
            {
                Exit = false;
                exitevent.Reset();
                ThreadEDSMSync = new System.Threading.Thread(new System.Threading.ThreadStart(SyncThread));
                ThreadEDSMSync.Name = "EDSM Journal Sync";
                ThreadEDSMSync.IsBackground = true;
                ThreadEDSMSync.Start();
            }

            return true;
        }

        private static bool ShouldSendEvent(HistoryEntry he)
        {
            if (lastDiscardFetch < DateTime.UtcNow.AddMinutes(-30))
            {
                EDSMClass edsm = new EDSMClass();
                discardEvents = new HashSet<string>(edsm.GetJournalEventsToDiscard());
                lastDiscardFetch = DateTime.UtcNow;
            }

            string eventtype = he.EntryType.ToString();
            if (he.EdsmSync || 
                he.IsStarPosFromEDSM ||
                he.MultiPlayer ||
                discardEvents.Contains(eventtype) ||
                alwaysDiscard.Contains(eventtype))
            {
                return false;
            }

            JournalFSDJump fsd = he.journalEntry as JournalFSDJump;

            if (fsd != null && fsd.FuelLevel <= 0) // Exclude pre-2.2 events
            {
                return false;
            }

            return true;
        }

        public static void StopSync()
        {
            Exit = true;
            exitevent.Set();
            historyevent.Set();
        }

        private static void SyncThread()
        {
            try
            {
                _running = 1;

                EDSMClass edsm = new EDSMClass();
                bool manual = false;
                int manualcount = 0;
                Action<string> logger;

                while (historylist.Count != 0)
                {
                    HistoryEntry first = null;
                    List<HistoryEntry> hl = new List<HistoryEntry>();
                    HistoryQueueEntry hqe = null;
                    HistoryEntry he = null;

                    if (historylist.TryDequeue(out hqe))
                    {
                        first = hqe.HistoryEntry;
                        historyevent.Reset();
                        manual = hqe.ManualSync;
                        logger = hqe.Logger;

                        if (!manual)
                        {
                            manualcount = 0;
                        }
                        else if (hqe.HistoryEntry == null)
                        {
                            hqe.Logger?.Invoke("Manual EDSM journal sync complete");
                            continue; // Discard end-of-sync event
                        }
                        else
                        {
                            manualcount++;
                        }

                        hl.Add(first);
                        string logline = $"Adding {first.EntryType.ToString()} event to EDSM journal sync ({first.EventSummary})";
                        hqe.Logger?.Invoke(logline);
                        System.Diagnostics.Trace.WriteLine(logline);

                        if (holdEvents.Contains(first.EntryType) || (first.EntryType == JournalTypeEnum.Location && first.IsDocked))
                        {
                            if (historylist.IsEmpty)
                            {
                                historyevent.WaitOne(20000); // Wait up to 20 seconds for another entry to come through
                            }
                        }

                        while (hl.Count < maxEventsPerMessage && historylist.TryPeek(out hqe)) // Leave event in queue if commander changes
                        {
                            he = hqe.HistoryEntry;
                            if (he == null || he.Commander != first.Commander || hqe.ManualSync != manual)
                            {
                                break;
                            }

                            historylist.TryDequeue(out hqe);
                            historyevent.Reset();

                            if (manual)
                            {
                                manualcount++;
                            }

                            hl.Add(he);
                            logline = $"Adding {he.EntryType.ToString()} event to EDSM journal sync ({he.EventSummary})";
                            hqe.Logger?.Invoke(logline);
                            System.Diagnostics.Trace.WriteLine(logline);

                            if ((holdEvents.Contains(he.EntryType) || (he.EntryType == JournalTypeEnum.Location && he.IsDocked)) && historylist.IsEmpty)
                            {
                                historyevent.WaitOne(20000); // Wait up to 20 seconds for another entry to come through
                            }

                            if (Exit)
                            {
                                return;
                            }
                        }

                        string errmsg;
                        int sendretries = 5;
                        int waittime = 30000;
                        while (sendretries > 0 && !EDSMJournalSync.SendToEDSM(hl, first.Commander, first.IsBetaMessage, out errmsg))
                        {
                            logger?.Invoke($"Error sending events: {errmsg}");
                            System.Diagnostics.Trace.WriteLine($"Error sending events: {errmsg}");
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
                        }
                    }

                    // Wait at least 50ms between messages
                    exitevent.WaitOne(50);
                    if (Exit)
                    {
                        return;
                    }

                    // Wait up to 120 seconds for another EDSM event to come in
                    if (historylist.IsEmpty)
                    {
                        historyevent.WaitOne(120000);
                    }

                    if (Exit)
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception ex:" + ex.Message);
            }
            finally
            {
                _running = 0;
            }
        }

        static public bool SendToEDSM(List<HistoryEntry> hl, EDCommander cmdr, bool isBeta, out string errmsg)
        {
            EDSMClass edsm = new EDSMClass();
            errmsg = null;

            if (cmdr != null)
            {
                edsm.commanderName = cmdr.EdsmName;
                if (string.IsNullOrEmpty(edsm.commanderName))
                    edsm.commanderName = cmdr.Name;

                if (cmdr.Name.StartsWith("[BETA]", StringComparison.InvariantCultureIgnoreCase))
                    return false;
            }

            if (isBeta)
                return false;

            List<JObject> entries = new List<JObject>();

            foreach (HistoryEntry he in hl)
            {
                JournalEntry je = he.journalEntry;

                if (je == null)
                {
                    je = JournalEntry.Get(he.Journalid);
                }

                JObject json = je.GetJson();
                RemoveCommonKeys(json);
                json.Remove("StarPosFromEDSM");
                json["_systemName"] = he.System.name;
                json["_systemCoordinates"] = new JArray(he.System.x, he.System.y, he.System.z);
                if (he.System.SystemAddress != null)
                    json["_systemAddress"] = he.System.SystemAddress;
                if (he.IsDocked)
                {
                    json["_stationName"] = he.StationName;
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
                using (var cn = new SQLiteConnectionUser(utc: true))
                {
                    using (var txn = cn.BeginTransaction())
                    {
                        for (int i = 0; i < hl.Count && i < results.Count; i++)
                        {
                            HistoryEntry he = hl[i];
                            JObject result = results[i];
                            int msgnr = result["msgnum"].Int();
                            int systemId = result["systemId"].Int();

                            if ((msgnr >= 100 && msgnr < 200) || msgnr == 500)
                            {
                                if (he.EntryType == JournalTypeEnum.FSDJump || he.EntryType == JournalTypeEnum.Location)
                                {
                                    if (systemId != 0)
                                    {
                                        he.System.id_edsm = systemId;
                                        JournalEntry.UpdateEDSMIDPosJump(he.Journalid, he.System, false, 0, cn, txn);
                                    }
                                }

                                he.SetEdsmSync(cn, txn);

                                if (msgnr == 500)
                                {
                                    System.Diagnostics.Trace.WriteLine($"Warning submitting event {he.Journalid} \"{he.EventSummary}\": {msgnr} {result["msg"].Str()}");
                                }
                            }
                            else
                            {
                                System.Diagnostics.Trace.WriteLine($"Error submitting event {he.Journalid} \"{he.EventSummary}\": {msgnr} {result["msg"].Str()}");
                            }

                            txn.Commit();
                        }
                    }
                }

                return true;
            }
        }
    }
}
