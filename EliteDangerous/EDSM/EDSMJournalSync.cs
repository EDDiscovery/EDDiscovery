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

namespace EliteDangerousCore.EDSM
{
    public static class EDSMJournalSync
    {
        private static Thread ThreadEDSMSync;
        private static int _running = 0;
        private static bool Exit = false;
        private static ConcurrentQueue<HistoryEntry> historylist = new ConcurrentQueue<HistoryEntry>();
        private static AutoResetEvent historyevent = new AutoResetEvent(false);
        private static ManualResetEvent exitevent = new ManualResetEvent(false);
        private static Action<string> logger;
        private static int maxEventsPerMessage = 32;
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

        public static bool SendEDSMEvents(Action<string> log, IEnumerable<HistoryEntry> helist)
        {
            logger = log;

            foreach (HistoryEntry he in helist)
            {
                if (ShouldSendEvent(he))
                {
                    historylist.Enqueue(he);
                }
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
                discardEvents = new HashSet<string>(edsm.GetJournalEventsToDiscard());

                while (historylist.Count != 0)
                {
                    HistoryEntry first = null;
                    List<HistoryEntry> hl = new List<HistoryEntry>();
                    HistoryEntry he = null;

                    if (historylist.TryDequeue(out first))
                    {
                        historyevent.Reset();

                        hl.Add(first);
                        logger?.Invoke($"Adding {first.EntryType.ToString()} event to EDSM journal sync ({first.EventSummary})");

                        if (holdEvents.Contains(first.EntryType) || (first.EntryType == JournalTypeEnum.Location && first.IsDocked))
                        {
                            historyevent.WaitOne(20000); // Wait up to 20 seconds for another entry to come through
                        }

                        while (hl.Count < maxEventsPerMessage && historylist.TryPeek(out he)) // Leave event in queue if commander changes
                        {
                            if (he.Commander != first.Commander)
                            {
                                break;
                            }

                            historylist.TryDequeue(out he);

                            historyevent.Reset();
                            hl.Add(he);
                            logger?.Invoke($"Adding {he.EntryType.ToString()} event to EDSM journal sync ({he.EventSummary})");

                            if (holdEvents.Contains(he.EntryType) || (he.EntryType == JournalTypeEnum.Location && he.IsDocked))
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
                            logger?.Invoke($"Unable to send events - giving up");
                        }
                        else
                        {
                            logger?.Invoke($"Sent {hl.Count} events to EDSM for commander {first.Commander.Name}");
                        }
                    }

                    // Wait at least 10 seconds between messages
                    exitevent.WaitOne(10000);
                    if (Exit)
                    {
                        return;
                    }

                    // Wait up to 120 seconds for another EDSM event to come in
                    historyevent.WaitOne(120000);
                    if (Exit)
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception ex:" + ex.Message);
                logger?.Invoke("EDSM journal sync Exception " + ex.Message);
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
                entries.Add(json);
            }

            int msgnr = edsm.SendJournalEvents(entries, out errmsg);

            if (msgnr < 100 || msgnr / 100 == 2)
            {
                return false;
            }
            else
            {
                foreach (HistoryEntry he in hl)
                {
                    he.SetEdsmSync();
                }

                return true;
            }
        }
    }
}
