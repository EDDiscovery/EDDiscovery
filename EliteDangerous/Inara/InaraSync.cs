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

using EliteDangerousCore.JournalEvents;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EliteDangerousCore.Inara
{
    public static class InaraSync
    {
        private class InaraQueueEntry
        {
            public JToken eventinfo;
            public EDCommander cmdr;
        }

        private static ConcurrentQueue<InaraQueueEntry> eventqueue = new ConcurrentQueue<InaraQueueEntry>();
        private static Thread ThreadInaraSync;
        private static int running = 0;
        private static bool Exit = false;
        private static AutoResetEvent queuedevents = new AutoResetEvent(false);
        private static ManualResetEvent exitevent = new ManualResetEvent(false);
        private static long CmdrCredits = 0;

        #region Public IF

        public static bool Refresh(Action<string> logger, HistoryList history, EDCommander cmdr)
        {
            List<JToken> events = RefreshList(history);
            if (events.Count > 0)
                Submit(events,cmdr);
            return true;
        }

        public static bool NewEvent(Action<string> log, HistoryEntry he)
        {
            List<JToken> events = NewEntryList(he);
            if (events.Count > 0)
                Submit(events,he.Commander);
            return true;
        }

        public static void StopSync()
        {
            Exit = true;
            exitevent.Set();
            queuedevents.Set();     // also trigger in case we are in thread hold
        }

        #endregion

        #region Formatters

        public static List<JToken> RefreshList(HistoryList history)         // may create NULL entries if some material items not found
        {
            List<JToken> eventstosend = new List<JToken>();

            HistoryEntry last = history.GetLast;
            if (last != null)
            {
                JournalStatistics stats = history.GetLastJournalEntry<JournalStatistics>(x => x.EntryType == JournalTypeEnum.Statistics);
                if (stats != null)
                    eventstosend.Add(InaraClass.setCommanderGameStatistics(stats.Json, stats.EventTimeUTC));

                JournalRank rank = history.GetLastJournalEntry<JournalRank>(x => x.EntryType == JournalTypeEnum.Rank);
                JournalProgress progress = history.GetLastJournalEntry<JournalProgress>(x => x.EntryType == JournalTypeEnum.Progress);
                if (rank != null)
                {
                    eventstosend.Add(InaraClass.setCommanderRankPilot("combat", (int)rank.Combat, progress?.Combat ?? -1, rank.EventTimeUTC));
                    eventstosend.Add(InaraClass.setCommanderRankPilot("trade", (int)rank.Trade, progress?.Trade ?? -1, rank.EventTimeUTC));
                    eventstosend.Add(InaraClass.setCommanderRankPilot("explore", (int)rank.Explore, progress?.Explore ?? -1, rank.EventTimeUTC));
                    eventstosend.Add(InaraClass.setCommanderRankPilot("empire", (int)rank.Empire, progress?.Empire ?? -1, rank.EventTimeUTC));
                    eventstosend.Add(InaraClass.setCommanderRankPilot("federation", (int)rank.Federation, progress?.Federation ?? -1, rank.EventTimeUTC));
                    eventstosend.Add(InaraClass.setCommanderRankPilot("cqc", (int)rank.CQC, progress?.Combat ?? -1, rank.EventTimeUTC));
                }

                JournalPowerplay power = history.GetLastJournalEntry<JournalPowerplay>(x => x.EntryType == JournalTypeEnum.Powerplay);
                if (power != null)
                    eventstosend.Add(InaraClass.setCommanderRankPower(power.Power, power.Rank, power.EventTimeUTC));

                JournalReputation reputation = history.GetLastJournalEntry<JournalReputation>(x => x.EntryType == JournalTypeEnum.Reputation);
                if (reputation != null)
                {
                    eventstosend.Add(InaraClass.setCommanderReputationMajorFaction("federation", reputation.Federation.HasValue ? reputation.Federation.Value : 0, reputation.EventTimeUTC));
                    eventstosend.Add(InaraClass.setCommanderReputationMajorFaction("empire", reputation.Empire.HasValue ? reputation.Empire.Value : 0, reputation.EventTimeUTC));
                    eventstosend.Add(InaraClass.setCommanderReputationMajorFaction("independent", reputation.Independent.HasValue ? reputation.Independent.Value : 0, reputation.EventTimeUTC));
                    eventstosend.Add(InaraClass.setCommanderReputationMajorFaction("alliance", reputation.Alliance.HasValue ? reputation.Alliance.Value : 0, reputation.EventTimeUTC));
                }

                List<MaterialCommodities> commod = last.MaterialCommodity.Sort(true);        // all commodities
                var listc = commod.Select(x => new Tuple<string, int>(x.fdname, x.count));
                eventstosend.Add(InaraClass.setCommanderInventoryCargo(listc, last.EventTimeUTC));

                List<MaterialCommodities> mat = last.MaterialCommodity.Sort(false);        // all materials
                var listm = mat.Select(x => new Tuple<string, int>(x.fdname, x.count));
                eventstosend.Add(InaraClass.setCommanderInventoryMaterials(listm, last.EventTimeUTC));

                eventstosend.Add(InaraClass.setCommanderStorageModules(last.StoredModules.StoredModules, last.EventTimeUTC));

                eventstosend.Add(InaraClass.setCommanderCredits(last.Credits, last.EventTimeUTC));
                CmdrCredits = last.Credits;

                var si = last.ShipInformation;
                eventstosend.Add(InaraClass.setCommanderShip(si.ShipFD, si.ID, last.EventTimeUTC,
                                                            si.ShipUserName, si.ShipUserIdent, true, si.Hot,
                                                            si.HullValue, si.ModulesValue, si.Rebuy, last.System.Name,
                                                            last.IsDocked ? last.WhereAmI : null, last.IsDocked ? last.MarketID : null));

                eventstosend.Add(InaraClass.setCommanderShipLoadout(si.ShipFD, si.ID, si.Modules.Values, last.EventTimeUTC));

                InaraClass inara = new InaraClass();
                System.IO.File.WriteAllText(@"c:\code\inarastart.json", inara.ToJSONString(eventstosend));
            }

            return eventstosend;
        }


        public static List<JToken> NewEntryList(HistoryEntry he)         // may create NULL entries if some material items not found
        {
            List<JToken> eventstosend = new List<JToken>();

            switch (he.journalEntry.EventTypeID)
            {
                case JournalTypeEnum.ShipyardBuy:
                    {
                        var je = he.journalEntry as JournalShipyardBuy;

                        if (je.StoreOldShipFD != null)
                            eventstosend.Add(InaraClass.setCommanderShip(je.StoreOldShipFD, je.StoreOldShipId.Value, he.EventTimeUTC, curship: false, starsystemName: he.System.Name, stationName: he.WhereAmI));
                        if (je.SellOldShipFD != null)
                            eventstosend.Add(InaraClass.delCommanderShip(je.SellOldShipFD, je.SellOldShipId.Value, he.EventTimeUTC));

                        eventstosend.Add(InaraClass.setCommanderCredits(he.Credits, he.EventTimeUTC));
                        CmdrCredits = he.Credits;

                        break;
                    }

                case JournalTypeEnum.ShipyardNew:       // into a new ship
                    {
                        var je = he.journalEntry as JournalShipyardNew;
                        eventstosend.Add(InaraClass.setCommanderShip(je.ShipFD, je.ShipId, he.EventTimeUTC, curship: true, starsystemName: he.System.Name, stationName: he.WhereAmI));
                        break;
                    }

                case JournalTypeEnum.ShipyardSell:
                    {
                        var je = he.journalEntry as JournalShipyardSell;
                        eventstosend.Add(InaraClass.delCommanderShip(je.ShipTypeFD, je.SellShipId, he.EventTimeUTC));
                        eventstosend.Add(InaraClass.setCommanderCredits(he.Credits, he.EventTimeUTC));
                        CmdrCredits = he.Credits;
                        break;
                    }

                case JournalTypeEnum.ShipyardSwap:
                    {
                        var je = he.journalEntry as JournalShipyardSwap;
                        eventstosend.Add(InaraClass.setCommanderShip(je.StoreOldShipFD, je.StoreShipId.Value, he.EventTimeUTC, curship: false, starsystemName: he.System.Name, stationName: he.WhereAmI));
                        eventstosend.Add(InaraClass.setCommanderShip(je.ShipFD, je.ShipId, he.EventTimeUTC, curship: true, starsystemName: he.System.Name, stationName: he.WhereAmI));
                        break;
                    }

                case JournalTypeEnum.ShipyardTransfer:
                    {
                        var je = he.journalEntry as JournalShipyardTransfer;
                        eventstosend.Add(InaraClass.setCommanderShipTransfer(je.ShipTypeFD, je.ShipId, he.System.Name, he.WhereAmI, he.MarketID, je.nTransferTime ?? 0, he.EventTimeUTC));
                        break;
                    }

                case JournalTypeEnum.Loadout:
                    {
                        var je = he.journalEntry as JournalLoadout;
                        var si = he.ShipInformation;
                        if (je.ShipId == si.ID)
                        {
                            eventstosend.Add(InaraClass.setCommanderShipLoadout(je.ShipFD, je.ShipId, si.Modules.Values, he.EventTimeUTC));
                            eventstosend.Add(InaraClass.setCommanderShip(je.ShipFD, je.ShipId, he.EventTimeUTC,
                                                                    je.ShipName, je.ShipIdent, true, null,
                                                                    je.HullValue, je.ModulesValue, je.Rebuy));
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("ERROR IN EDD Inara system:Current ship does not match loadout");
                        }
                        break;
                    }

                case JournalTypeEnum.SetUserShipName:
                    {
                        var je = he.journalEntry as JournalSetUserShipName;
                        eventstosend.Add(InaraClass.setCommanderShip(je.ShipFD, je.ShipID, he.EventTimeUTC, curship: true, username: je.ShipName, userid: je.ShipIdent, starsystemName: he.System.Name, stationName: he.WhereAmI));
                        break;
                    }


                case JournalTypeEnum.Docked:
                    {
                        var je = he.journalEntry as JournalDocked;
                        eventstosend.Add(InaraClass.addCommanderTravelDock(he.ShipInformation.ShipFD, he.ShipInformation.ID, je.StarSystem, je.StationName, je.MarketID, he.EventTimeUTC));
                        break;
                    }

                case JournalTypeEnum.FSDJump:
                    {
                        var je = he.journalEntry as JournalFSDJump;
                        eventstosend.Add(InaraClass.addCommanderTravelFSDJump(he.ShipInformation.ShipFD, he.ShipInformation.ID, je.StarSystem, je.JumpDist, he.EventTimeUTC));
                        break;
                    }

                case JournalTypeEnum.Location:
                    {
                        var je = he.journalEntry as JournalLocation;
                        eventstosend.Add(InaraClass.addCommanderTravelLocation(je.StarSystem, je.Docked ? je.StationName : null, je.Docked ? je.MarketID : null, he.EventTimeUTC));
                        break;
                    }

                case JournalTypeEnum.MissionAccepted:
                    {
                        var je = he.journalEntry as JournalMissionAccepted;
                        eventstosend.Add(InaraClass.addCommanderMission(je, he.System.Name, he.WhereAmI));
                        break;
                    }
                case JournalTypeEnum.MissionAbandoned:
                    {
                        var je = he.journalEntry as JournalMissionAbandoned;
                        eventstosend.Add(InaraClass.setCommanderMissionAbandoned(je.MissionId, he.EventTimeUTC));
                        break;
                    }
                case JournalTypeEnum.MissionFailed:
                    {
                        var je = he.journalEntry as JournalMissionFailed;
                        eventstosend.Add(InaraClass.setCommanderMissionFailed(je.MissionId, he.EventTimeUTC));
                        break;
                    }

                case JournalTypeEnum.MissionCompleted:
                    {
                        var je = he.journalEntry as JournalMissionCompleted;
                        eventstosend.Add(InaraClass.setCommanderMissionCompleted(je));
                        break;
                    }

                case JournalTypeEnum.EngineerProgress:
                    {
                        var je = he.journalEntry as JournalEngineerProgress;
                        eventstosend.Add(InaraClass.setCommanderRankEngineer(je.Engineer, je.Progress, je.Rank.HasValue ? je.Rank.Value : 1, he.EventTimeUTC));
                        break;
                    }

                case JournalTypeEnum.MaterialTrade: // one out, one in..
                    {
                        var je = he.journalEntry as JournalMaterialTrade;
                        if (je.Paid != null)
                            eventstosend.Add(InaraClass.setCommanderInventoryItem(he.MaterialCommodity, je.Paid.Material, he.EventTimeUTC));
                        if (je.Received != null)
                            eventstosend.Add(InaraClass.setCommanderInventoryItem(he.MaterialCommodity, je.Received.Material, he.EventTimeUTC));

                        break;
                    }

                case JournalTypeEnum.EngineerCraft:
                    {
                        var je = he.journalEntry as JournalEngineerCraft;

                        if (je.Ingredients != null)
                        {
                            foreach (KeyValuePair<string, int> k in je.Ingredients)
                            {
                                eventstosend.Add(InaraClass.setCommanderInventoryItem(he.MaterialCommodity, k.Key, he.EventTimeUTC));
                            }
                        }
                        break;
                    }
                case JournalTypeEnum.Synthesis:
                    {
                        var je = he.journalEntry as JournalSynthesis;

                        if (je.Materials != null)
                        {
                            foreach (KeyValuePair<string, int> k in je.Materials)
                            {
                                eventstosend.Add(InaraClass.setCommanderInventoryItem(he.MaterialCommodity, k.Key, he.EventTimeUTC));
                            }
                        }
                        break;
                    }
                case JournalTypeEnum.Died:
                    {
                        var je = he.journalEntry as JournalDied;
                        string[] killers = je.Killers != null ? je.Killers.Select(x => x.Name).ToArray() : null;
                        eventstosend.Add(InaraClass.addCommanderCombatDeath(he.System.Name, killers, he.EventTimeUTC));
                        break;
                    }

                case JournalTypeEnum.Interdicted:
                    {
                        var je = he.journalEntry as JournalInterdicted;
                        eventstosend.Add(InaraClass.addCommanderCombatInterdicted(he.System.Name, je.Interdictor, je.IsPlayer, je.Submitted, he.EventTimeUTC));
                        break;
                    }
                case JournalTypeEnum.Interdiction:
                    {
                        var je = he.journalEntry as JournalInterdiction;
                        eventstosend.Add(InaraClass.addCommanderCombatInterdiction(he.System.Name, je.Interdicted.HasChars() ? je.Interdicted : je.Faction, je.IsPlayer, je.Success, he.EventTimeUTC));
                        break;
                    }

                case JournalTypeEnum.EscapeInterdiction:
                    {
                        var je = he.journalEntry as JournalEscapeInterdiction;
                        eventstosend.Add(InaraClass.addCommanderCombatInterdictionEscape(he.System.Name, je.Interdictor, je.IsPlayer, he.EventTimeUTC));
                        break;
                    }

                case JournalTypeEnum.PVPKill:
                    {
                        var je = he.journalEntry as JournalPVPKill;
                        eventstosend.Add(InaraClass.addCommanderCombatKill(he.System.Name, je.Victim, he.EventTimeUTC));
                        break;
                    }

                case JournalTypeEnum.CargoDepot:
                    {
                        var je = he.journalEntry as JournalCargoDepot;
                        if (je.CargoType.HasChars() && je.Count > 0)
                            eventstosend.Add(InaraClass.setCommanderInventoryItem(he.MaterialCommodity, je.CargoType, he.EventTimeUTC));
                        break;
                    }
                case JournalTypeEnum.CollectCargo:
                    {
                        var je = he.journalEntry as JournalCollectCargo;
                        eventstosend.Add(InaraClass.setCommanderInventoryItem(he.MaterialCommodity, je.Type, he.EventTimeUTC));
                        break;
                    }
                case JournalTypeEnum.EjectCargo:
                    {
                        var je = he.journalEntry as JournalEjectCargo;
                        eventstosend.Add(InaraClass.setCommanderInventoryItem(he.MaterialCommodity, je.Type, he.EventTimeUTC));
                        break;
                    }
                case JournalTypeEnum.EngineerContribution:
                    {
                        var je = he.journalEntry as JournalEngineerContribution;
                        if (je.Commodity.HasChars())
                            eventstosend.Add(InaraClass.setCommanderInventoryItem(he.MaterialCommodity, je.Commodity, he.EventTimeUTC));
                        if (je.Material.HasChars())
                            eventstosend.Add(InaraClass.setCommanderInventoryItem(he.MaterialCommodity, je.Material, he.EventTimeUTC));
                        break;
                    }
                case JournalTypeEnum.MarketBuy:
                    {
                        var je = he.journalEntry as JournalMarketBuy;
                        eventstosend.Add(InaraClass.setCommanderInventoryItem(he.MaterialCommodity, je.Type, he.EventTimeUTC));
                        break;
                    }
                case JournalTypeEnum.MarketSell:
                    {
                        var je = he.journalEntry as JournalMarketSell;
                        eventstosend.Add(InaraClass.setCommanderInventoryItem(he.MaterialCommodity, je.Type, he.EventTimeUTC));
                        break;
                    }
                case JournalTypeEnum.MaterialCollected:
                    {
                        var je = he.journalEntry as JournalMaterialCollected;
                        eventstosend.Add(InaraClass.setCommanderInventoryItem(he.MaterialCommodity, je.Name, he.EventTimeUTC));
                        break;
                    }
                case JournalTypeEnum.MaterialDiscarded:
                    {
                        var je = he.journalEntry as JournalMaterialDiscarded;
                        eventstosend.Add(InaraClass.setCommanderInventoryItem(he.MaterialCommodity, je.Name, he.EventTimeUTC));
                        break;
                    }
                case JournalTypeEnum.MiningRefined:
                    {
                        var je = he.journalEntry as JournalMiningRefined;
                        eventstosend.Add(InaraClass.setCommanderInventoryItem(he.MaterialCommodity, je.Type, he.EventTimeUTC));
                        break;
                    }

                    // EDDItemSet/cargo/Materials - not doing, handled by initial sync
            }

            //if ( Math.Abs(CmdrCredits-he.Credits) > 500000 )
            //{
            //    eventstosend.Add(InaraClass.setCommanderCredits(he.Credits, he.EventTimeUTC));
            //    CmdrCredits = he.Credits;
            //}

            eventstosend = eventstosend.Where(x => x != null).ToList();     // remove any nulls

            return eventstosend;
        }

        #endregion

        #region Thread

        public static void Submit(List<JToken> list, EDCommander cmdrn)
        {
            foreach (var x in list)
                eventqueue.Enqueue(new InaraQueueEntry() { eventinfo = x, cmdr = cmdrn });

            queuedevents.Set();

            // Start the sync thread if it's not already running
            if (Interlocked.CompareExchange(ref running, 1, 0) == 0)
            {
                Exit = false;
                exitevent.Reset();
                ThreadInaraSync = new System.Threading.Thread(new System.Threading.ThreadStart(SyncThread));
                ThreadInaraSync.Name = "Inara Journal Sync";
                ThreadInaraSync.IsBackground = true;
                ThreadInaraSync.Start();
            }
        }

        private static void SyncThread()
        {
            try
            {
                running = 1;

                while (eventqueue.Count != 0)      // while stuff to send
                {
                    
                    exitevent.WaitOne(10000);       // min time between sends..

                    if (Exit)
                        break;

                    if (eventqueue.IsEmpty)
                        queuedevents.WaitOne(120000);       // wait for another event keeping the thread open.. Note stop also sets this

                    if (Exit)
                        break;
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

        #endregion
    }
}

