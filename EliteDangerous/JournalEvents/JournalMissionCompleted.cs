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

namespace EliteDangerousCore.JournalEvents
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
    public class JournalMissionCompleted : JournalEntry, IMaterialCommodityJournalEntry, ILedgerJournalEntry, IMissions
    {
        public JournalMissionCompleted(JObject evt) : base(evt, JournalTypeEnum.MissionCompleted)
        {
            Name = JournalFieldNaming.GetBetterMissionName(evt["Name"].Str());
            Faction = evt["Faction"].Str();

            Commodity = JournalFieldNaming.FixCommodityName(evt["Commodity"].Str());             // evidence of $_name problem, fix to fdname
            Commodity = JournalFieldNaming.FDNameTranslation(Commodity);     // pre-mangle to latest names, in case we are reading old journal records
            FriendlyCommodity = JournalFieldNaming.RMat(Commodity);
            CommodityLocalised = evt["Commodity_Localised"].Str().Alt(FriendlyCommodity);

            Count = evt["Count"].IntNull();

            TargetType = evt["TargetType"].Str();
            TargetTypeFriendly = JournalFieldNaming.GetBetterTargetTypeName(TargetType);        // remove $, underscores etc
            TargetTypeLocalised = evt["TargetTypeLocalised"].Str().Alt(TargetTypeFriendly);     // may be empty..

            TargetFaction = evt["TargetFaction"].Str();

            Target = evt["Target"].Str();
            TargetFriendly = JournalFieldNaming.GetBetterTargetTypeName(Target);        // remove $, underscores etc
            TargetLocalised = evt["Target_Localised"].Str().Alt(TargetFriendly);        // copied from Accepted.. no evidence

            Reward = evt["Reward"].LongNull();
            Donation = evt["Donation"].LongNull();
            MissionId = evt["MissionID"].Int();

            DestinationSystem = evt["DestinationSystem"].Str().Replace("$MISSIONUTIL_MULTIPLE_INNER_SEPARATOR;", ",");       // multi missions get this strange list
            DestinationStation = evt["DestinationStation"].Str();

            PermitsAwarded = evt["PermitsAwarded"]?.ToObjectProtected<string[]>();

            // 7/3/2018 journal 16 3.02

            CommodityReward = evt["CommodityReward"]?.ToObjectProtected<CommodityRewards[]>();

            if ( CommodityReward != null )
            {
                foreach (CommodityRewards c in CommodityReward)
                    c.Normalise();
            }

            MaterialsReward = evt["MaterialsReward"]?.ToObjectProtected<MaterialRewards[]>();

            if ( MaterialsReward != null)
            {
                foreach (MaterialRewards m in MaterialsReward)
                    m.Normalise();
            }

            FactionEffects = evt["FactionEffects"]?.ToObjectProtected<FactionEffectsEntry[]>();      // NEEDS TEST
        }

        public string Name { get; set; }
        public string Faction { get; set; }

        public string Commodity { get; set; }               // FDNAME, leave, evidence of the $_name problem
        public string CommodityLocalised { get; set; }
        public string FriendlyCommodity { get; set; }
        public int? Count { get; set; }

        public string Target { get; set; }
        public string TargetLocalised { get; set; }
        public string TargetFriendly { get; set; }
        public string TargetType { get; set; }
        public string TargetTypeLocalised { get; set; }
        public string TargetTypeFriendly { get; set; }
        public string TargetFaction { get; set; }

        public string DestinationSystem { get; set; }
        public string DestinationStation { get; set; }

        public long? Reward { get; set; }
        public long? Donation { get; set; }
        public string[] PermitsAwarded { get; set; }
        public int MissionId { get; set; }

        public CommodityRewards[] CommodityReward { get; set; }            
        public MaterialRewards[] MaterialsReward { get; set; }

        public FactionEffectsEntry[] FactionEffects;

        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            if (CommodityReward != null)
            {
                foreach (CommodityRewards c in CommodityReward)
                    mc.Change(MaterialCommodities.CommodityCategory, c.Name, c.Count, 0, conn);
            }

            if (MaterialsReward != null)
            {
                foreach( MaterialRewards m in MaterialsReward )                 // 7/3/2018 not yet fully proven.. changed in 3.02
                {
                    string c = m.Category.Alt(MaterialCommodities.MaterialRawCategory);     // older ones did not have this tag..
                    mc.Change(c, m.Name, m.Count, 0, conn);
                }
            }
        }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            long rv = Reward.HasValue ? Reward.Value : 0;
            long dv = Donation.HasValue ? Donation.Value : 0;

            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Name, (rv-dv), 0);
        }

        public void UpdateMissions(MissionListAccumulator mlist, EliteDangerousCore.ISystem sys, string body, DB.SQLiteConnectionUser conn)
        {
            mlist.Completed(this);
        }

        public override void FillInformation(out string summary, out string info, out string detailed)  //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("", Name,
                                        "< from ", Faction,
                                        "Reward:; cr;N0", Reward,
                                        "Donation:", Donation,
                                        "System:", DestinationSystem,
                                        "Station:", DestinationStation);

            detailed = BaseUtils.FieldBuilder.Build("Commodity:", CommodityLocalised,
                                            "Target:", TargetLocalised,
                                            "Type:", TargetTypeLocalised,
                                            "Target Faction:", TargetFaction);

            detailed += PermitsList();
            detailed += CommoditiesList();
            detailed += MaterialList();
        }

        public string PermitsList(bool pretty = true)
        {
            string detailed = "";
            if (PermitsAwarded != null && PermitsAwarded.Length > 0)
            {
                if (pretty)
                    detailed += "Permits:";

                for (int i = 0; i < PermitsAwarded.Length; i++)
                    detailed += ((i > 0) ? "," : "") + PermitsAwarded[i];

                if (pretty)
                    detailed += System.Environment.NewLine;
            }
            return detailed;
        }

        public string CommoditiesList(bool pretty = true)
        {
            string detailed = "";
            if (CommodityReward != null && CommodityReward.Length > 0)
            {
                if (pretty)
                    detailed += "Rewards:";
                for (int i = 0; i < CommodityReward.Length; i++)
                {
                    CommodityRewards c = CommodityReward[i];
                    detailed += ((i > 0) ? "," : "") + c.Name_Localised + " " + CommodityReward[i].Count.ToStringInvariant();
                }

                if (pretty)
                    detailed += System.Environment.NewLine;
            }
            return detailed;
        }

        public string MaterialList(bool pretty = true)
        {
            string detailed = "";
            if (MaterialsReward != null && MaterialsReward.Length > 0)
            {
                if (pretty)
                    detailed += "Rewards:";
                for (int i = 0; i < MaterialsReward.Length; i++)
                {
                    MaterialRewards m = MaterialsReward[i];
                    detailed += ((i > 0) ? "," : "") + m.Name_Localised + " " + MaterialsReward[i].Count.ToStringInvariant();
                }

                if (pretty)
                    detailed += System.Environment.NewLine;
            }
            return detailed;
        }

        public string RewardOrDonation { get { return Reward.HasValue ? Reward.Value.ToString("N0") : (Donation.HasValue ? (-Donation.Value).ToString("N0") : ""); } }
        public long Value { get { return Reward.HasValue ? Reward.Value : (Donation.HasValue ? (-Donation.Value) : 0); } }

        public string MissionInformation()          // other stuff for the mission panel which it does not already cover or accepted has
        {
            return PermitsList() + CommoditiesList() + MaterialList();
        }

        public class MaterialRewards
        {
            public string Name; // fdname
            public string FriendlyName; // our conversion
            public string Name_Localised;       // may be null
            public string Category; // may be null
            public string Category_Localised; // may be null
            public int Count;

            public void Normalise()
            {
                Name = JournalFieldNaming.FDNameTranslation(Name);
                FriendlyName = JournalFieldNaming.RMat(Name);
                Name_Localised = Name_Localised.Alt(FriendlyName);
            }
        }

        public class CommodityRewards
        {
            public string Name; // fdname
            public string FriendlyName; // our conversion
            public string Name_Localised;   // may be null
            public int Count;

            public void Normalise()
            {
                Name = JournalFieldNaming.FDNameTranslation(Name);
                FriendlyName = JournalFieldNaming.RMat(Name);
                Name_Localised = Name_Localised.Alt(FriendlyName);
            }
        }

        public class EffectTrend
        {
            public string Effect;
            public string Effect_Localised;
            public string Trend;
        }

        public class InfuluenceTrend
        {
            public long SystemAddress;
            public string Trend;
        }

        public class FactionEffectsEntry
        {
            public string Faction;
            public EffectTrend[] Effects;
            public InfuluenceTrend[] Influence;
            public string Reputation;
        }
    }
}
