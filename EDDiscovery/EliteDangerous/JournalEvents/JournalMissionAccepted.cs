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
using System.Globalization;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when starting a mission
    //Parameters:
    //•	Name: name of mission
    //•	Faction: faction offering mission
    //•	MissionID
    //•	Influence: effect on influence(None/Low/Med/High)
    //•	Reputation: effect on reputation(None/Low/Med/High)

    //Optional Parameters (depending on mission type)
    //•	Commodity: $Commodity_Name;
    //•	Commodity_Localised: commodity type
    //•	Count: number required / to deliver
    //•	Target: name of target
    //•	TargetType: type of target
    //•	TargetFaction: target’s faction
    //•	Expiry: mission expiry time, in ISO 8601
    //•	DestinationSystem
    //•	DestinationStation
    //•	PassengerCount
    //•	PassengerVIPs: bool
    //•	PassengerWanted: bool
    //•	PassengerType: eg Tourist, Soldier, Explorer,...


    [JournalEntryType(JournalTypeEnum.MissionAccepted)]
    public class JournalMissionAccepted : JournalEntry, IMissions
    {
        public JournalMissionAccepted(JObject evt ) : base(evt, JournalTypeEnum.MissionAccepted)
        {       
            Faction = evt["Faction"].Str();
            Name = JournalFieldNaming.GetBetterMissionName(evt["Name"].Str());

            TargetType = evt["TargetType"].Str();
            TargetTypeFriendly = JournalFieldNaming.GetBetterTargetTypeName(TargetType);    // remove $, underscore it
            TargetTypeLocalised = evt["TargetType_Localised"].Str();
            TargetFaction = evt["TargetFaction"].Str();
            Target = evt["Target"].Str();
            TargetFriendly = JournalFieldNaming.GetBetterTargetTypeName(Target);        // remove $, underscore it
            TargetLocalised = evt["Target_localised"].Str();        // not all

            DestinationSystem = evt["DestinationSystem"].Str();
            DestinationStation = evt["DestinationStation"].Str();

            Influence = evt["Influence"].Str();
            Reputation = evt["Reputation"].Str();

            MissionId = evt["MissionID"].Int();

            Commodity = JournalFieldNaming.FixCommodityName(evt["Commodity"].Str());        // instances of $_name, fix to fdname
            CommodityLocalised = evt["Commodity_Localised"].Str();
            FriendlyCommodity = JournalFieldNaming.RMat(Commodity);

            Count = evt["Count"].IntNull();

            if (!evt["Expiry"].Empty())
                Expiry = DateTime.Parse(evt.Value<string>("Expiry"), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

            PassengerCount = evt["PassengerCount"].IntNull();
            PassengerVIPs = evt["PassengerVIPs"].BoolNull();
            PassengerWanted = evt["PassengerWanted"].BoolNull();
            PassengerType = evt["PassengerType"].StrNull();

        }

        public string Faction { get; set; }                 // in MissionAccepted order
        public string Name { get; set; }

        public string TargetType { get; set; }
        public string TargetTypeFriendly { get; set; }
        public string TargetTypeLocalised { get; set; }
        public string TargetFaction { get; set; }
        public string DestinationSystem { get; set; }
        public string DestinationStation { get; set; }
        public string Target { get; set; }
        public string TargetFriendly { get; set; }
        public string TargetLocalised { get; set; }     // not all.. only for radars etc.
        public DateTime Expiry { get; set; }
        public string Influence { get; set; }
        public string Reputation { get; set; }
        public int MissionId { get; set; }

        public string Commodity { get; set; }               //fdname, this is for delivery missions, stuff being transported
        public string CommodityLocalised { get; set; }
        public string FriendlyCommodity { get; set; }       //db name
        public int? Count { get; set; }

        public int? PassengerCount { get; set; }            // for passenger missions
        public bool? PassengerVIPs { get; set; }
        public bool? PassengerWanted { get; set; }
        public string PassengerType { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.missionaccepted; } }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = Tools.FieldBuilder("", Name, 
                                      "< from ", Faction, 
                                      "System:", DestinationSystem, 
                                      "Station:", DestinationStation, 
                                      "Expiry:", Expiry,
                                      "Influence:", Influence,
                                      "Reputation:", Reputation);

            detailed = Tools.FieldBuilder("Deliver:", CommodityLocalised.Alt(FriendlyCommodity), 
                                           "Target:", TargetLocalised.Alt(TargetFriendly), 
                                           "Type:", TargetTypeFriendly,
                                           "Target Faction:", TargetFaction,
                                           "Target Type:", TargetTypeLocalised.Alt(TargetTypeFriendly),
                                           "Passengers:", PassengerCount);
        }

        public string MissionInformation()          // other stuff for the mission panel which it does not already cover
        {
            return Tools.FieldBuilder(  "Influence:", Influence,
                                        "Reputation:", Reputation,
                                        "Deliver:", CommodityLocalised.Alt(FriendlyCommodity),
                                        "Target:", TargetLocalised.Alt(TargetFriendly),
                                        "Type:", TargetTypeFriendly,
                                        "Target Faction:", TargetFaction,
                                        "Target Type:", TargetTypeLocalised.Alt(TargetTypeFriendly),
                                        "Passengers:", PassengerCount);

        }

        public void UpdateMissions(MissionListAccumulator mlist, EDDiscovery2.DB.ISystem sys, string body, DB.SQLiteConnectionUser conn)
        {
            mlist.Accepted(this,sys,body);
        }
    }
}

