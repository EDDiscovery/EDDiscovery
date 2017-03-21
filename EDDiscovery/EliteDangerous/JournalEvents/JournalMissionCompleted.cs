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
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when a mission is completed
    //Parameters:
    //•	Name: mission type
    //•	Faction: faction name
    //Optional parameters (depending on mission type)
    //•	Commodity: $Commodity_Name;
    //•	Commodity_Localised: commodity type
    //•	Count
    //•	Target
    //•	TargetType
    //•	TargetFaction
    //•	Reward: value of reward
    //•	Donation: donation offered (for altruism missions)
    //•	PermitsAwarded:[] (names of any permits awarded, as a JSON array)
    [JournalEntryType(JournalTypeEnum.MissionCompleted)]
    public class JournalMissionCompleted : JournalEntry, IMaterialCommodityJournalEntry, ILedgerJournalEntry
    {
        public JournalMissionCompleted(JObject evt ) : base(evt, JournalTypeEnum.MissionCompleted)
        {
            Name = JournalFieldNaming.GetBetterMissionName(evt["Name"].Str());
            Faction = evt["Faction"].Str();
            Commodity = evt["Commodity"].Str();
            Count = evt["Count"].IntNull();
            Target = evt["Target"].Str();
            TargetType = evt["TargetType"].Str();
            TargetFaction = evt["TargetFaction"].Str();
            Reward = evt["Reward"].LongNull();
            Donation = evt["Donation"].LongNull();
            MissionId = evt["MissionID"].Int();

            DestinationSystem = evt["DestinationSystem"].Str();
            DestinationStation = evt["DestinationStation"].Str();

            if (!evt["PermitsAwarded"].Empty())
                PermitsAwarded = evt.Value<JArray>("PermitsAwarded").Values<string>().ToArray();

            if (!evt["CommodityReward"].Empty())
            {
                JArray rewards = (JArray)evt["CommodityReward"];

                if (rewards.Count > 0)
                {
                    CommodityReward = new System.Tuple<string, int>[rewards.Count];
                    int i = 0;
                    foreach (JToken jc in rewards.Children())
                    {
                        if (!jc["Name"].Empty() && !jc["Count"].Empty())
                            CommodityReward[i++] = new System.Tuple<string, int>(jc["Name"].Value<string>(), jc["Count"].Value<int>());

                        //System.Diagnostics.Trace.WriteLine(string.Format(" >> Child {0} {1}", jc.Path, jc.Type.ToString()));
                    }
                }
            }

            FriendlyCommodity = JournalFieldNaming.RMat(Commodity);
        }

        public string Name { get; set; }
        public string Faction { get; set; }
        public string Commodity { get; set; }
        public string FriendlyCommodity { get; set; }
        public int? Count { get; set; }
        public string Target { get; set; }
        public string TargetType { get; set; }
        public string TargetFaction { get; set; }
        public string DestinationSystem { get; set; }
        public string DestinationStation { get; set; }
        public long? Reward { get; set; }
        public long? Donation { get; set; }
        public string[] PermitsAwarded { get; set; }
        public int MissionId { get; set; }
        public System.Tuple<string, int>[] CommodityReward { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.missioncompleted; } }

        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            if (CommodityReward != null)
            {
                // Forum indicates its commodities, and we get normal materialcollected events if its a material.
                for (int i = 0; i < CommodityReward.Length; i++)
                    mc.Change(MaterialCommodities.CommodityCategory, CommodityReward[i].Item1, CommodityReward[i].Item2, 0, conn);
            }
        }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Name, (Reward - Donation) , 0);
        }

        public override void FillInformation(out string summary, out string info, out string detailed)  //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = Tools.FieldBuilder("", Name, "<from ", Faction, "Reward:" , Reward , "Donation:" , Donation , "System:", DestinationSystem, "Station:", DestinationStation);
            detailed = Tools.FieldBuilder("Commodity:", FriendlyCommodity, "Target:", Target, "Type:", TargetType, "Target Faction:", TargetFaction);       

            if (PermitsAwarded != null)
            {
                foreach (string s in PermitsAwarded)
                    detailed += System.Environment.NewLine + "Permit: " + s;
            }

            if (CommodityReward != null)
            {
                foreach (System.Tuple<string,int> t in CommodityReward)
                    detailed += System.Environment.NewLine + "Commodity: " + JournalFieldNaming.RMat(t.Item1) + " " + t.Item2.ToString();
            }
        }
    }
}
