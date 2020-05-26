/*
 * Copyright © 2018 EDDiscovery development team
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
using System.Threading;

namespace EliteDangerousCore.Inara
{
    public static class InaraSync
    {
        private class InaraQueueEntry
        {
            public JToken eventinfo;
            public Action<string> logger;
            public EDCommander cmdr;
            public string cmdrfid;
            public bool verbose;
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
                Submit(events,logger, cmdr, history.GetCommanderFID(), true);
            return true;
        }

        public static bool HistoricData(Action<string> logger, HistoryList history, EDCommander cmdr)
        {
            List<JToken> events = HistoricList(history);
            if (events.Count > 0)
                Submit(events, logger, cmdr, history.GetCommanderFID(), true);
            return true;
        }

        public static bool NewEvent(Action<string> logger, HistoryList history , HistoryEntry he)
        {
            List<JToken> events = NewEntryList(history,he);
            if (events.Count > 0)
                Submit(events,logger, he.Commander, history.GetCommanderFID(), false);
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
            if (last != null && last.EventTimeUTC > DateTime.UtcNow.AddDays(-30))
            {
                var si = last.ShipInformation;

                if (si != null)
                {
                    eventstosend.Add(InaraClass.setCommanderShip(si.ShipFD, si.ID, last.EventTimeUTC,
                                                                si.ShipUserName, si.ShipUserIdent, true, si.Hot,
                                                                si.HullValue, si.ModulesValue, si.Rebuy, last.System.Name,
                                                                last.IsDocked ? last.WhereAmI : null, last.IsDocked ? last.MarketID : null));
                }

                eventstosend.Add(InaraClass.setCommanderCredits(last.Credits, last.EventTimeUTC));

                eventstosend.Add(InaraClass.setCommanderTravelLocation(last.System.Name, last.IsDocked ? last.WhereAmI : null, last.MarketID.HasValue ? last.MarketID : null, last.EventTimeUTC));

                CmdrCredits = last.Credits;
            }

            return eventstosend;
        }

        public static List<JToken> HistoricList(HistoryList history)         // may create NULL entries if some material items not found
        {
            List<JToken> eventstosend = new List<JToken>();

            HistoryEntry last = history.GetLast;

            if (last != null && last.EventTimeUTC > DateTime.UtcNow.AddDays(-30))
            {
                foreach( var s in history.shipinformationlist.Ships )
                {
                    ShipInformation si = s.Value;
                    if ( si.State == ShipInformation.ShipState.Owned && !ShipModuleData.IsSRVOrFighter(si.ShipFD))
                    {
                        // loadout may be null if nothing in it.
                        eventstosend.Add(InaraClass.setCommanderShipLoadout(si.ShipFD, si.ID, si.Modules.Values, DateTime.UtcNow));

                        eventstosend.Add(InaraClass.setCommanderShip(si.ShipFD, si.ID, DateTime.UtcNow,
                                                                si.ShipUserName, si.ShipUserIdent, last.ShipInformation.ID == si.ID, null,
                                                                si.HullValue, si.ModulesValue, si.Rebuy, si.StoredAtSystem, si.StoredAtStation));
                    }
                }

                eventstosend.Add(InaraClass.setCommanderStorageModules(last.StoredModules.StoredModules, last.EventTimeUTC));
            }

            eventstosend = eventstosend.Where(x => x != null).ToList();     // remove any nulls

            return eventstosend;
        }


        public static List<JToken> NewEntryList(HistoryList history, HistoryEntry he)         // may create NULL entries if some material items not found
        {
            List<JToken> eventstosend = new List<JToken>();

            switch (he.journalEntry.EventTypeID)
            {
                case JournalTypeEnum.ShipyardBuy: // VERIFIED 18/5/2018
                    {
                        var je = he.journalEntry as JournalShipyardBuy;

                        if (je.StoreOldShipFD != null && je.StoreOldShipId.HasValue)
                            eventstosend.Add(InaraClass.setCommanderShip(je.StoreOldShipFD, je.StoreOldShipId.Value, he.EventTimeUTC, curship: false, starsystemName: he.System.Name, stationName: he.WhereAmI));
                        if (je.SellOldShipFD != null && je.SellOldShipId.HasValue)
                            eventstosend.Add(InaraClass.delCommanderShip(je.SellOldShipFD, je.SellOldShipId.Value, he.EventTimeUTC));

                        eventstosend.Add(InaraClass.setCommanderCredits(he.Credits, he.EventTimeUTC));
                        CmdrCredits = he.Credits;

                        break;
                    }

                case JournalTypeEnum.ShipyardNew:       // into a new ship // VERIFIED 18/5/2018
                    {
                        var je = he.journalEntry as JournalShipyardNew;
                        eventstosend.Add(InaraClass.setCommanderShip(je.ShipFD, je.ShipId, he.EventTimeUTC, curship: true, starsystemName: he.System.Name, stationName: he.WhereAmI));
                        break;
                    }

                case JournalTypeEnum.ShipyardSell: // VERIFIED 18/5/2018
                    {
                        var je = he.journalEntry as JournalShipyardSell;
                        eventstosend.Add(InaraClass.delCommanderShip(je.ShipTypeFD, je.SellShipId, he.EventTimeUTC));
                        eventstosend.Add(InaraClass.setCommanderCredits(he.Credits, he.EventTimeUTC));
                        CmdrCredits = he.Credits;
                        break;
                    }

                case JournalTypeEnum.ShipyardSwap: // VERIFIED 18/5/2018
                    {
                        var je = he.journalEntry as JournalShipyardSwap;
                        eventstosend.Add(InaraClass.setCommanderShip(je.StoreOldShipFD, je.StoreShipId.Value, he.EventTimeUTC, curship: false, starsystemName: he.System.Name, stationName: he.WhereAmI));
                        eventstosend.Add(InaraClass.setCommanderShip(je.ShipFD, je.ShipId, he.EventTimeUTC, curship: true, starsystemName: he.System.Name, stationName: he.WhereAmI));
                        break;
                    }

                case JournalTypeEnum.ShipyardTransfer: // VERIFIED 18/5/2018
                    {
                        var je = he.journalEntry as JournalShipyardTransfer;
                        eventstosend.Add(InaraClass.setCommanderShipTransfer(je.ShipTypeFD, je.ShipId, he.System.Name, he.WhereAmI, he.MarketID, je.nTransferTime ?? 0, he.EventTimeUTC));
                        break;
                    }

                case JournalTypeEnum.Loadout: // VERIFIED 18/5/2018
                    {
                        var je = he.journalEntry as JournalLoadout;
                        var si = he.ShipInformation;
                        if (si != null && je.ShipFD.HasChars() && !ShipModuleData.IsSRVOrFighter(je.ShipFD)) // if it has an FDname (defensive) and is not SRV/Fighter
                        {
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
                        }
                        break;
                    }

                case JournalTypeEnum.StoredModules: // VERIFIED 18/5/2018 from historic upload test
                    {
                        eventstosend.Add(InaraClass.setCommanderStorageModules(he.StoredModules.StoredModules, he.EventTimeUTC));
                        break;
                    }

                case JournalTypeEnum.SetUserShipName: // VERIFIED 18/5/2018
                    {
                        var je = he.journalEntry as JournalSetUserShipName;
                        eventstosend.Add(InaraClass.setCommanderShip(je.ShipFD, je.ShipID, he.EventTimeUTC, curship: true, username: je.ShipName, userid: je.ShipIdent, starsystemName: he.System.Name, stationName: he.WhereAmI));
                        break;
                    }

                case JournalTypeEnum.Docked:  // VERIFIED 18/5/2018 from historic upload test
                    {
                        if (he.ShipInformation != null)     // PR2754 error - a empty list can end up with he.shipinformation = null if all is hidden.
                        {
                            var je = he.journalEntry as JournalDocked;
                            eventstosend.Add(InaraClass.addCommanderTravelDock(he.ShipInformation.ShipFD, he.ShipInformation.ID, je.StarSystem, je.StationName, je.MarketID, he.EventTimeUTC));
                        }
                        break;
                    }

                case JournalTypeEnum.FSDJump: // VERIFIED 18/5/2018
                    {
                        if (he.ShipInformation != null)     // PR2754 error - a empty list can end up with he.shipinformation = null if all is hidden.
                        {
                            var je = he.journalEntry as JournalFSDJump;
                            eventstosend.Add(InaraClass.addCommanderTravelFSDJump(he.ShipInformation.ShipFD, he.ShipInformation.ID, je.StarSystem, je.JumpDist, he.EventTimeUTC));
                        }
                        break;
                    }

                case JournalTypeEnum.CarrierJump: // NEW! 26/5/2020
                    {
                        if (he.ShipInformation != null)     // PR2754 error - a empty list can end up with he.shipinformation = null if all is hidden.
                        {
                            var je = he.journalEntry as JournalCarrierJump;
                            eventstosend.Add(InaraClass.addCommanderTravelCarrierJump(he.ShipInformation.ShipFD, he.ShipInformation.ID, je.StarSystem, he.EventTimeUTC));
                        }
                        break;
                    }

                case JournalTypeEnum.Location: // VERIFIED 18/5/2018
                    {
                        var je = he.journalEntry as JournalLocation;
                        eventstosend.Add(InaraClass.setCommanderTravelLocation(je.StarSystem, je.Docked ? je.StationName : null, je.Docked ? je.MarketID : null, he.EventTimeUTC));
                        break;
                    }

                case JournalTypeEnum.MissionAccepted: // VERIFIED 18/5/2018
                    {
                        var je = he.journalEntry as JournalMissionAccepted;
                        eventstosend.Add(InaraClass.addCommanderMission(je, he.System.Name, he.WhereAmI));
                        break;
                    }
                case JournalTypeEnum.MissionAbandoned:// VERIFIED 18/5/2018
                    {
                        var je = he.journalEntry as JournalMissionAbandoned;
                        eventstosend.Add(InaraClass.setCommanderMissionAbandoned(je.MissionId, he.EventTimeUTC));
                        break;
                    }
                case JournalTypeEnum.MissionFailed:// VERIFIED 18/5/2018
                    {
                        var je = he.journalEntry as JournalMissionFailed;
                        eventstosend.Add(InaraClass.setCommanderMissionFailed(je.MissionId, he.EventTimeUTC));
                        break;
                    }

                case JournalTypeEnum.MissionCompleted: // VERIFIED 18/5/2018
                    { 
                        var je = he.journalEntry as JournalMissionCompleted;
                        eventstosend.Add(InaraClass.setCommanderMissionCompleted(je));
                        break;
                    }

                case JournalTypeEnum.Progress:      // progress comes after rank. No need for rank.     VERIFIED  16/5/18
                    {
                        JournalRank rank = history.GetLastJournalEntry<JournalRank>(x => x.EntryType == JournalTypeEnum.Rank);
                        JournalProgress progress = history.GetLastJournalEntry<JournalProgress>(x => x.EntryType == JournalTypeEnum.Progress);
                        if (rank != null)
                        {
                            eventstosend.Add(InaraClass.setCommanderRankPilot("combat", (int)rank.Combat, progress?.Combat ?? -1, rank.EventTimeUTC));
                            eventstosend.Add(InaraClass.setCommanderRankPilot("trade", (int)rank.Trade, progress?.Trade ?? -1, rank.EventTimeUTC));
                            eventstosend.Add(InaraClass.setCommanderRankPilot("explore", (int)rank.Explore, progress?.Explore ?? -1, rank.EventTimeUTC));
                            eventstosend.Add(InaraClass.setCommanderRankPilot("empire", (int)rank.Empire, progress?.Empire ?? -1, rank.EventTimeUTC));
                            eventstosend.Add(InaraClass.setCommanderRankPilot("federation", (int)rank.Federation, progress?.Federation ?? -1, rank.EventTimeUTC));
                            eventstosend.Add(InaraClass.setCommanderRankPilot("cqc", (int)rank.CQC, progress?.CQC ?? -1, rank.EventTimeUTC));
                        }

                        break;
                    }

                case JournalTypeEnum.Promotion:     // promotion
                    {
                        var promotion = he.journalEntry as JournalPromotion;
                        if (promotion.Combat != null)
                            eventstosend.Add(InaraClass.setCommanderRankPilot("combat", (int)promotion.Combat, 0, promotion.EventTimeUTC));     // by definition, since your promoted, progress = 0
                        if (promotion.Trade != null)
                            eventstosend.Add(InaraClass.setCommanderRankPilot("trade", (int)promotion.Trade, 0, promotion.EventTimeUTC));     // by definition, since your promoted, progress = 0
                        if (promotion.Explore != null)
                            eventstosend.Add(InaraClass.setCommanderRankPilot("explore", (int)promotion.Explore, 0, promotion.EventTimeUTC));     // by definition, since your promoted, progress = 0
                        if (promotion.Empire != null)
                            eventstosend.Add(InaraClass.setCommanderRankPilot("empire", (int)promotion.Empire, 0, promotion.EventTimeUTC));     // by definition, since your promoted, progress = 0
                        if (promotion.Federation != null)
                            eventstosend.Add(InaraClass.setCommanderRankPilot("federation", (int)promotion.Federation, 0, promotion.EventTimeUTC));     // by definition, since your promoted, progress = 0
                        if (promotion.CQC != null)
                            eventstosend.Add(InaraClass.setCommanderRankPilot("cqc", (int)promotion.CQC, 0, promotion.EventTimeUTC));     // by definition, since your promoted, progress = 0

                        break;
                    }

                case JournalTypeEnum.Reputation: // VERIFIED 16/5/18
                    {
                        var reputation = he.journalEntry as JournalReputation;
                        eventstosend.Add(InaraClass.setCommanderReputationMajorFaction("federation", reputation.Federation.HasValue ? reputation.Federation.Value : 0, reputation.EventTimeUTC));
                        eventstosend.Add(InaraClass.setCommanderReputationMajorFaction("empire", reputation.Empire.HasValue ? reputation.Empire.Value : 0, reputation.EventTimeUTC));
                        eventstosend.Add(InaraClass.setCommanderReputationMajorFaction("independent", reputation.Independent.HasValue ? reputation.Independent.Value : 0, reputation.EventTimeUTC));
                        eventstosend.Add(InaraClass.setCommanderReputationMajorFaction("alliance", reputation.Alliance.HasValue ? reputation.Alliance.Value : 0, reputation.EventTimeUTC));
                        break;
                    }

                case JournalTypeEnum.Powerplay: // VERIFIED 16/5/18
                    {
                        JournalPowerplay power = he.journalEntry as JournalPowerplay;
                        eventstosend.Add(InaraClass.setCommanderRankPower(power.Power, power.Rank, power.EventTimeUTC));
                        break;
                    }

                case JournalTypeEnum.EngineerProgress:      //VERIFIED 16/5/18
                    {
                        var je = he.journalEntry as JournalEngineerProgress;
                        foreach( var x in je.Engineers )
                            eventstosend.Add(InaraClass.setCommanderRankEngineer(x.Engineer, x.Progress, x.Rank, he.EventTimeUTC));
                        break;
                    }

                case JournalTypeEnum.Died: //VERIFIED 16/5/18
                    {
                        var je = he.journalEntry as JournalDied;
                        string[] killers = je.Killers != null ? je.Killers.Select(x => x.Name).ToArray() : null;
                        eventstosend.Add(InaraClass.addCommanderCombatDeath(he.System.Name, killers, he.EventTimeUTC));
                        break;
                    }

                case JournalTypeEnum.Interdicted: //VERIFIED 16/5/18
                    {
                        var je = he.journalEntry as JournalInterdicted;
                        eventstosend.Add(InaraClass.addCommanderCombatInterdicted(he.System.Name, je.Interdictor, je.IsPlayer, je.Submitted, he.EventTimeUTC));
                        break;
                    }
                case JournalTypeEnum.Interdiction: //VERIFIED 16/5/18
                    {
                        var je = he.journalEntry as JournalInterdiction;
                        eventstosend.Add(InaraClass.addCommanderCombatInterdiction(he.System.Name, je.Interdicted.HasChars() ? je.Interdicted : je.Faction, je.IsPlayer, je.Success, he.EventTimeUTC));
                        break;
                    }

                case JournalTypeEnum.EscapeInterdiction: //VERIFIED 16/5/18
                    {
                        var je = he.journalEntry as JournalEscapeInterdiction;
                        eventstosend.Add(InaraClass.addCommanderCombatInterdictionEscape(he.System.Name, je.Interdictor, je.IsPlayer, he.EventTimeUTC));
                        break;
                    }

                case JournalTypeEnum.PVPKill: //VERIFIED 16/5/18
                    {
                        var je = he.journalEntry as JournalPVPKill;
                        eventstosend.Add(InaraClass.addCommanderCombatKill(he.System.Name, je.Victim, he.EventTimeUTC));
                        break;
                    }

                case JournalTypeEnum.CargoDepot: //VERIFIED 16/5/18
                    {
                        var je = he.journalEntry as JournalCargoDepot;
                        if (je.CargoType.HasChars() && je.Count > 0)
                            eventstosend.Add(InaraClass.setCommanderInventoryItem(he.MaterialCommodity, je.CargoType, he.EventTimeUTC));
                        break;
                    }
                case JournalTypeEnum.CollectCargo: //VERIFIED 16/5/18
                    {
                        var je = he.journalEntry as JournalCollectCargo;
                        eventstosend.Add(InaraClass.setCommanderInventoryItem(he.MaterialCommodity, je.Type, he.EventTimeUTC));
                        break;
                    }
                case JournalTypeEnum.EjectCargo: //VERIFIED 16/5/18
                    { 
                        var je = he.journalEntry as JournalEjectCargo;
                        eventstosend.Add(InaraClass.setCommanderInventoryItem(he.MaterialCommodity, je.Type, he.EventTimeUTC));
                        break;
                    }
                case JournalTypeEnum.EngineerContribution: //VERIFIED 16/5/18
                    {
                        var je = he.journalEntry as JournalEngineerContribution;
                        if (je.Commodity.HasChars())
                            eventstosend.Add(InaraClass.setCommanderInventoryItem(he.MaterialCommodity, je.Commodity, he.EventTimeUTC));
                        if (je.Material.HasChars())
                            eventstosend.Add(InaraClass.setCommanderInventoryItem(he.MaterialCommodity, je.Material, he.EventTimeUTC));
                        break;
                    }
                case JournalTypeEnum.MarketBuy: //VERIFIED 16/5/18
                    {
                        var je = he.journalEntry as JournalMarketBuy;
                        eventstosend.Add(InaraClass.setCommanderInventoryItem(he.MaterialCommodity, je.Type, he.EventTimeUTC));
                        break;
                    }
                case JournalTypeEnum.MarketSell: //VERIFIED 16/5/18
                    {
                        var je = he.journalEntry as JournalMarketSell;
                        eventstosend.Add(InaraClass.setCommanderInventoryItem(he.MaterialCommodity, je.Type, he.EventTimeUTC));
                        break;
                    }
                case JournalTypeEnum.Cargo: //VERIFIED 16/5/18
                    {
                        List<MaterialCommodities> commod = he.MaterialCommodity.Sort(true);        // all commodities
                        var listc = commod.Where(x=>x.Count>0).Select(x => new Tuple<string, int>(x.Details.FDName, x.Count));
                        eventstosend.Add(InaraClass.setCommanderInventoryCargo(listc, he.EventTimeUTC));
                        break;
                    }

                case JournalTypeEnum.Materials: //VERIFIED 16/5/18
                    { 
                        List<MaterialCommodities> mat = he.MaterialCommodity.Sort(false);        // all materials
                        var listm = mat.Where(x => x.Count > 0).Select(x => new Tuple<string, int>(x.Details.FDName, x.Count));
                        eventstosend.Add(InaraClass.setCommanderInventoryMaterials(listm, he.EventTimeUTC));
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

                case JournalTypeEnum.MaterialTrade: // one out, one in.. //VERIFIED 16/5/18
                    {
                        var je = he.journalEntry as JournalMaterialTrade;
                        if (je.Paid != null)
                            eventstosend.Add(InaraClass.setCommanderInventoryItem(he.MaterialCommodity, je.Paid.Material, he.EventTimeUTC));
                        if (je.Received != null)
                            eventstosend.Add(InaraClass.setCommanderInventoryItem(he.MaterialCommodity, je.Received.Material, he.EventTimeUTC));

                        break;
                    }

                case JournalTypeEnum.EngineerCraft: //VERIFIED 16/5/18
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
                case JournalTypeEnum.Synthesis: //VERIFIED 16/5/18
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

                case JournalTypeEnum.Statistics://VERIFIED 16/5/18
                    {
                        JournalStatistics stats = he.journalEntry as JournalStatistics;
                        eventstosend.Add(InaraClass.setCommanderGameStatistics(stats.Json, stats.EventTimeUTC));
                        break;
                    }

                case JournalTypeEnum.CommunityGoal://VERIFIED 16/5/18
                    {
                        var je = he.journalEntry as JournalCommunityGoal;
                        foreach (var c in je.CommunityGoals)
                        {
                            eventstosend.Add(InaraClass.setCommunityGoal(c, he.EventTimeUTC));
                            eventstosend.Add(InaraClass.setCommandersCommunityGoalProgress(c, he.EventTimeUTC));
                        }

                        break;
                    }

                case JournalTypeEnum.Friends:
                    {
                        var je = he.journalEntry as JournalFriends;
                        if ( je.StatusList != null )
                        {
                            for( int i = 0; i < je.StatusList.Count; i++ )
                            {
                                string s = je.StatusList[i].ToLower();
                                if (s == "online" || s == "added")
                                    eventstosend.Add(InaraClass.addCommanderFriend(je.NameList[i], he.EventTimeUTC));
                                else if (s == "lost")
                                    eventstosend.Add(InaraClass.delCommanderFriend(je.NameList[i], he.EventTimeUTC));
                            }
                        }
                        else
                        {
                            string s = je.Status.ToLower();
                            if (s == "online" || s == "added")
                                eventstosend.Add(InaraClass.addCommanderFriend(je.Name, he.EventTimeUTC));
                            else if (s == "lost")
                                eventstosend.Add(InaraClass.delCommanderFriend(je.Name, he.EventTimeUTC));
                        }

                        break;
                    }

            }


            if ( Math.Abs(CmdrCredits-he.Credits) > 500000 )
            {
                eventstosend.Add(InaraClass.setCommanderCredits(he.Credits, he.EventTimeUTC));
                CmdrCredits = he.Credits;
            }

            eventstosend = eventstosend.Where(x => x != null).ToList();     // remove any nulls

            return eventstosend;
        }

#endregion

#region Thread

        public static void Submit(List<JToken> list, Action<string> logger, EDCommander cmdrn, string cmdrfidp, bool verbose)
        {
            foreach (var x in list)
                eventqueue.Enqueue(new InaraQueueEntry() { eventinfo = x, cmdr = cmdrn , cmdrfid = cmdrfidp, logger = logger, verbose = verbose});

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
                    exitevent.WaitOne(10000);       // wait in case others are being generated

                    if (Exit)
                        break;

                    if (eventqueue.TryDequeue(out InaraQueueEntry firstheq))
                    {
                        List<JToken> tosend = new List<JToken>() { firstheq.eventinfo };

                        int maxpergo = 50;
                        bool verbose = false;

                        // if not too many, and we have another, and the commander is the same 
                        while (tosend.Count < maxpergo && eventqueue.TryPeek(out InaraQueueEntry nextheq) && nextheq.cmdr.Nr == firstheq.cmdr.Nr)
                        {
                            eventqueue.TryDequeue(out nextheq);     // and remove it
                            tosend.Add(nextheq.eventinfo);
                            verbose |= nextheq.verbose;
                        }

                        InaraClass inara = new InaraClass(firstheq.cmdr,firstheq.cmdrfid);
                        string errs = inara.Send(tosend);
                        if ( errs != null)
                        {
                            System.Diagnostics.Debug.WriteLine("Inara reports error" + errs);
                            firstheq?.logger("INARA Reports: " + errs);
                        }
                        else if ( verbose )
                            firstheq?.logger("Sent " + tosend.Count + " events to INARA" );
                    }

                    exitevent.WaitOne(30000);       // space out events well

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

