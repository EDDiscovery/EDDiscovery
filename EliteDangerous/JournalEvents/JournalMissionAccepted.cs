/*
 * Copyright © 2016-2018 EDDiscovery development team
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
using EliteDangerousCore.DB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.MissionAccepted)]
    public class JournalMissionAccepted : JournalEntry, IMissions, IMaterialCommodityJournalEntry
    {
        public JournalMissionAccepted(JObject evt ) : base(evt, JournalTypeEnum.MissionAccepted)
        {       
            Faction = evt["Faction"].Str();
            FDName = evt["Name"].Str();
            Name = JournalFieldNaming.GetBetterMissionName(FDName);

            TargetType = evt["TargetType"].Str();
            TargetTypeFriendly = JournalFieldNaming.GetBetterTargetTypeName(TargetType);    // remove $, underscore it
            TargetTypeLocalised = JournalFieldNaming.CheckLocalisation(evt["TargetType_Localised"].Str(),TargetTypeFriendly);

            TargetFaction = evt["TargetFaction"].Str();

            Target = evt["Target"].Str();
            TargetFriendly = JournalFieldNaming.GetBetterTargetTypeName(Target);        // remove $, underscore it
            TargetLocalised = JournalFieldNaming.CheckLocalisation(evt["Target_localised"].Str(),TargetFriendly);        // not all

            KillCount = evt["KillCount"].IntNull();

            DestinationSystem = evt["DestinationSystem"].Str().Replace("$MISSIONUTIL_MULTIPLE_INNER_SEPARATOR;", ",")
                                                              .Replace("$MISSIONUTIL_MULTIPLE_FINAL_SEPARATOR;", ",");       // multi missions get this strange list;
            DestinationStation = evt["DestinationStation"].Str();

            Influence = evt["Influence"].Str();
            Reputation = evt["Reputation"].Str();

            MissionId = evt["MissionID"].Int();

            Commodity = JournalFieldNaming.FixCommodityName(evt["Commodity"].Str());        // instances of $_name, fix to fdname
            FriendlyCommodity = JournalFieldNaming.RMat(Commodity);
            CommodityLocalised = JournalFieldNaming.CheckLocalisation(evt["Commodity_Localised"].Str(),FriendlyCommodity);

            Count = evt["Count"].IntNull();
            Expiry = evt["Expiry"].DateTimeUTC();

            PassengerCount = evt["PassengerCount"].IntNull();
            PassengerVIPs = evt["PassengerVIPs"].BoolNull();
            PassengerWanted = evt["PassengerWanted"].BoolNull();
            PassengerType = evt["PassengerType"].StrNull();

            Reward = evt["Reward"].IntNull();   // not in DOC V13, but present in latest journal entries

            Wing = evt["Wing"].BoolNull();      // new 3.02

        }

        public int MissionId { get; private set; }

        public string Faction { get; private set; }                 // in MissionAccepted order
        public string Name { get; private set; }
        public string FDName { get; private set; }

        public string DestinationSystem { get; private set; }
        public string DestinationStation { get; private set; }

        public string TargetType { get; private set; }
        public string TargetTypeFriendly { get; private set; }
        public string TargetTypeLocalised { get; private set; }
        public string TargetFaction { get; private set; }
        public string Target { get; private set; }
        public string TargetFriendly { get; private set; }
        public string TargetLocalised { get; private set; }     // not all.. only for radars etc.
        public int? KillCount { get; private set; }

        public DateTime Expiry { get; private set; }            // MARKED as 2000 if not there..
        public bool ExpiryValid { get { return Expiry.Year >= 2014; } }

        public string Influence { get; private set; }
        public string Reputation { get; private set; }

        public string Commodity { get; private set; }               //fdname, this is for delivery missions, stuff being transported
        public string CommodityLocalised { get; private set; }
        public string FriendlyCommodity { get; private set; }       //db name
        public int? Count { get; private set; }

        public int? PassengerCount { get; private set; }            // for passenger missions
        public bool? PassengerVIPs { get; private set; }
        public bool? PassengerWanted { get; private set; }
        public string PassengerType { get; private set; }

        public int? Reward { get; private set; }

        public bool? Wing { get; private set; }     // 3.02

        public override void FillInformation(out string info, out string detailed) 
        {
            info = MissionBasicInfo();
            detailed = MissionDetailedInfo();
        }

        private static HashSet<string> DeliveryMissions = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
        {
            "Mission_Delivery",
            "Mission_Delivery_Boom",
            "Chain_HelpFinishTheOrder",
            "Mission_Delivery_War"
        };

        public string MissionBasicInfo()          // other stuff for the mission panel which it does not already cover
        {
            DateTime exp = Expiry;
            if (exp != null && !EliteConfigInstance.InstanceConfig.DisplayUTC)
                exp = exp.ToLocalTime();

            return BaseUtils.FieldBuilder.Build("", Name,
                                      "< from ", Faction,
                                      "System:", DestinationSystem,
                                      "Station:", DestinationStation,
                                      "Expiry:", exp,
                                      "Influence:", Influence,
                                      "Reputation:", Reputation,
                                      "Reward:; cr;N0", Reward,
                                      "; (Wing)", Wing);
        }

        public string MissionDetailedInfo()          // other stuff for the mission panel which it does not already cover
        {
            return BaseUtils.FieldBuilder.Build("Deliver:", CommodityLocalised,
                                           "Target:", TargetLocalised,
                                           "Type:", TargetTypeFriendly,
                                           "Target Faction:", TargetFaction,
                                           "Target Type:", TargetTypeLocalised,
                                           "Kill Count:", KillCount,
                                           "Passengers:", PassengerCount);
        }

        public string MissionAuxInfo()          // other stuff for the mission panel which it does not already cover
        {
            return BaseUtils.FieldBuilder.Build("Influence:", Influence,
                                        "Reputation:", Reputation,
                                        "Deliver:", CommodityLocalised,
                                        "Target:", TargetLocalised,
                                        "Type:", TargetTypeFriendly,
                                        "Target Faction:", TargetFaction,
                                        "Target Type:", TargetTypeLocalised,
                                        "Passengers:", PassengerCount);

        }

        public void UpdateMissions(MissionListAccumulator mlist, EliteDangerousCore.ISystem sys, string body, DB.SQLiteConnectionUser conn)
        {
            mlist.Accepted(this,sys,body);
        }

        public void MaterialList(MaterialCommoditiesList mc, SQLiteConnectionUser conn)
        {
            if (Commodity != null && Count != null && DeliveryMissions.Contains(FDName))
            {
                mc.Change(MaterialCommodityData.CommodityCategory, Commodity, (int)Count, 0, conn);
            }
        }
    }
}

