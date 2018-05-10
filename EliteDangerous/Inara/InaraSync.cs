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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteDangerousCore.Inara
{
    public static class InaraSync
    {
        public static bool Refresh(Action<string> logger, HistoryList history)
        {
            HistoryEntry last = history.GetLast;
            if (last != null)
            {
                List<JToken> eventstosend = new List<JToken>();

                JournalStatistics stats = history.GetLastJournalEntry<JournalStatistics>(x => x.EntryType == JournalTypeEnum.Statistics);
                if (stats != null)
                    eventstosend.Add(InaraClass.setCommanderGameStatistics(stats.Json, stats.EventTimeUTC));

                JournalRank rank = history.GetLastJournalEntry<JournalRank>(x => x.EntryType == JournalTypeEnum.Rank);
                JournalProgress progress = history.GetLastJournalEntry<JournalProgress>(x => x.EntryType == JournalTypeEnum.Progress);
                if (rank != null )
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

                var si = last.ShipInformation;
                eventstosend.Add(InaraClass.setCommanderShip(si.ShipFD, si.ID, last.EventTimeUTC,  
                                                            si.ShipUserName, si.ShipUserIdent, true, si.Hot,
                                                            si.HullValue , si.ModulesValue, si.Rebuy, last.System.Name, 
                                                            last.IsDocked ? last.WhereAmI : null , last.IsDocked ? last.MarketID : null));

                eventstosend.Add(InaraClass.setCommanderShipLoadout(si.ShipFD, si.ID, si.Modules.Values, last.EventTimeUTC));
                InaraClass inara = new InaraClass();
                inara.Send(eventstosend);
            }

            return true;
        }


        public static bool NewEvent(Action<string> log, HistoryEntry he)
        {
            List<JToken> eventstosend = new List<JToken>();

            switch( he.journalEntry.EventTypeID)
            {
                case JournalTypeEnum.ShipyardBuy:
                    {
                        var je = he.journalEntry as JournalShipyardBuy;

                        if (je.StoreOldShipFD != null)
                            eventstosend.Add(InaraClass.setCommanderShip(je.StoreOldShipFD, je.StoreOldShipId.Value, he.EventTimeUTC, curship: false, starsystemName: he.System.Name, stationName: he.WhereAmI));
                        if (je.SellOldShipFD != null)
                            eventstosend.Add(InaraClass.delCommanderShip(je.SellOldShipFD, je.SellOldShipId.Value, he.EventTimeUTC));
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
                        eventstosend.Add(InaraClass.setCommanderShipTransfer(je.ShipTypeFD,je.ShipId, he.System.Name, he.WhereAmI, he.MarketID, je.nTransferTime??0, he.EventTimeUTC));
                        break;
                    }

                case JournalTypeEnum.Loadout:
                    {
                        var si = he.ShipInformation;
                        eventstosend.Add(InaraClass.setCommanderShipLoadout(si.ShipFD, si.ID, si.Modules.Values, he.EventTimeUTC));
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
                        eventstosend.Add(InaraClass.addCommanderTravelDock(he.ShipInformation.ShipFD, he.ShipInformation.ID, he.System.Name, he.WhereAmI, he.EventTimeUTC));
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

                case JournalTypeEnum.MissionCompleted:
                    {
                        var je = he.journalEntry as JournalMissionCompleted;
                        if (je.PermitsAwarded != null)
                        {
                            foreach (string s in je.PermitsAwarded)
                                eventstosend.Add(InaraClass.addCommanderPermit(s, he.EventTimeUTC));
                        }

                        // more stuff later.

                        if ( je.CommodityReward != null )
                        {
                            foreach (var x in je.CommodityReward)
                            {
                                JToken entry = InaraClass.setCommanderInventoryItem(he.MaterialCommodity, x.Name, he.EventTimeUTC);
                                if (entry != null)
                                    eventstosend.Add(entry);
                            }
                        }

                        if ( je.MaterialsReward != null )
                        {
                            foreach (var x in je.MaterialsReward)
                            {
                                JToken entry = InaraClass.setCommanderInventoryItem(he.MaterialCommodity, x.Name, he.EventTimeUTC);
                                if (entry != null)
                                    eventstosend.Add(entry);
                            }
                        }
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
                        {
                            JToken entry = InaraClass.setCommanderInventoryItem(he.MaterialCommodity, je.Paid.Material, he.EventTimeUTC);
                            if (entry != null)
                                eventstosend.Add(entry);
                        }

                        if (je.Received != null)
                        {
                            JToken entry = InaraClass.setCommanderInventoryItem(he.MaterialCommodity, je.Received.Material, he.EventTimeUTC);
                            if (entry != null)
                                eventstosend.Add(entry);
                        }

                        break;
                    }

                case JournalTypeEnum.EngineerCraft:
                    {
                        var je = he.journalEntry as JournalEngineerCraft;

                        if (je.Ingredients != null)
                        {
                            foreach (KeyValuePair<string, int> k in je.Ingredients)
                            {
                                JToken entry = InaraClass.setCommanderInventoryItem(he.MaterialCommodity, k.Key, he.EventTimeUTC);
                                if (entry != null)
                                    eventstosend.Add(entry);
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
                                JToken entry = InaraClass.setCommanderInventoryItem(he.MaterialCommodity, k.Key, he.EventTimeUTC);
                                if (entry != null)
                                    eventstosend.Add(entry);
                            }
                        }
                        break;
                    }
                case JournalTypeEnum.Died:
                    {
                        eventstosend.Add(InaraClass.setCommanderInventoryCargo(null, he.EventTimeUTC));
                        break;
                    }
                case JournalTypeEnum.CargoDepot:
                case JournalTypeEnum.CollectCargo:
                case JournalTypeEnum.EjectCargo:
                case JournalTypeEnum.EngineerContribution:
                case JournalTypeEnum.MarketBuy:
                case JournalTypeEnum.MarketSell:
                case JournalTypeEnum.MaterialCollected:
                case JournalTypeEnum.MaterialDiscarded:
                case JournalTypeEnum.MiningRefined:
                    {
                        var x = he.MaterialCommodity.LastChange;
                        if ( x != null)
                        {
                            if (x.category.Equals(MaterialCommodities.CommodityCategory))
                                eventstosend.Add(InaraClass.setCommanderInventoryCargoItem(x.fdname, x.count, null, he.EventTimeUTC));
                            else
                                eventstosend.Add(InaraClass.setCommanderInventoryMaterialsItem(x.fdname, x.count, he.EventTimeUTC));
                        }
                        break;
                    }

                // EDDItemSet/cargo/Materials - not doing, handled by initial sync
            }

            return true;
        }
    }
}

