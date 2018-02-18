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
            CommodityLocalised = evt["Commodity_Localised"].Str();
            FriendlyCommodity = JournalFieldNaming.RMat(Commodity);

            Count = evt["Count"].IntNull();

            TargetType = evt["TargetType"].Str();
            TargetTypeFriendly = JournalFieldNaming.GetBetterTargetTypeName(TargetType);        // remove $, underscores etc
            TargetTypeLocalised = evt["TargetTypeLocalised"].Str();     // may be empty..
            TargetFaction = evt["TargetFaction"].Str();
            Target = evt["Target"].Str();
            TargetFriendly = JournalFieldNaming.GetBetterTargetTypeName(Target);        // remove $, underscores etc
            TargetLocalised = evt["Target_Localised"].Str();        // copied from Accepted.. no evidence

            Reward = evt["Reward"].LongNull();
            Donation = evt["Donation"].LongNull();
            MissionId = evt["MissionID"].Int();

            DestinationSystem = evt["DestinationSystem"].Str().Replace("$MISSIONUTIL_MULTIPLE_INNER_SEPARATOR;", ",");       // multi missions get this strange list
            DestinationStation = evt["DestinationStation"].Str();

            if (!evt["PermitsAwarded"].Empty())
                PermitsAwarded = evt.Value<JArray>("PermitsAwarded").Values<string>().ToArray();

            if (!evt["CommodityReward"].Empty())
            {
                JArray rewards = (JArray)evt["CommodityReward"];        // does not have the $_name problem, straight FDNAME

                if (rewards.Count > 0)
                {
                    System.Tuple<string, int>[] cr = new System.Tuple<string, int>[rewards.Count];
                    int i = 0;
                    foreach (JToken jc in rewards.Children())
                    {
                        if (!jc["Name"].Empty() && !jc["Count"].Empty())        // evidence of empty values
                            cr[i++] = new System.Tuple<string, int>(JournalFieldNaming.FDNameTranslation(jc["Name"].Value<string>()), jc["Count"].Value<int>());

                        //System.Diagnostics.Trace.WriteLine(string.Format(" >> Child {0} {1}", jc.Path, jc.Type.ToString()));
                    }
                    CommodityReward = new System.Tuple<string, int>[i];
                    System.Array.Copy(cr, CommodityReward, i);
                }
            }

            // 16/2/2018 NO EVIDENCE in manual of format, turn on when have answer

            if (!evt["MaterialsReward"].Empty())
            {
                JArray rewards = (JArray)evt["MaterialsReward"];        // does not have the $_name problem, straight FDNAME

                if (rewards.Count > 0)
                {
                    System.Tuple<string, int>[] cr = new System.Tuple<string, int>[rewards.Count];
                    int i = 0;
                    foreach (JToken jc in rewards.Children())
                    {
                        if (!jc["Name"].Empty() && !jc["Count"].Empty())        // evidence of empty values
                            cr[i++] = new System.Tuple<string, int>(JournalFieldNaming.FDNameTranslation(jc["Name"].Str()), jc["Count"].Int());

                        //System.Diagnostics.Trace.WriteLine(string.Format(" >> Child {0} {1}", jc.Path, jc.Type.ToString()));
                    }
                    MaterialsReward = new System.Tuple<string, int>[i];
                    System.Array.Copy(cr, MaterialsReward, i);
                }
            }

            FactionEffects = evt["FactionEffects"]?.ToObject<FactionEffectsEntry[]>();      // NEEDS TEST
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

        public System.Tuple<string, int>[] CommodityReward { get; set; }            // Verified in fdname, not in $_name. Must be in fdname format
        public System.Tuple<string, int>[] MaterialsReward { get; set; }

        public FactionEffectsEntry[] FactionEffects;

        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            if (CommodityReward != null)
            {
                // Forum indicates its commodities, and we get normal materialcollected events if its a material.
                for (int i = 0; i < CommodityReward.Length; i++)
                    mc.Change(MaterialCommodities.CommodityCategory, CommodityReward[i].Item1, CommodityReward[i].Item2, 0, conn);
            }

            if (MaterialsReward != null)
            {
                for (int i = 0; i < MaterialsReward.Length; i++)        // NOTE as of 3.0 journal 15, we are not getting a CAT, so set true don't use CAT to match
                    mc.Change(MaterialCommodities.MaterialRawCategory, MaterialsReward[i].Item1, MaterialsReward[i].Item2, 0, conn, true);
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

            detailed = BaseUtils.FieldBuilder.Build("Commodity:", CommodityLocalised.Alt(FriendlyCommodity),
                                            "Target:", TargetLocalised.Alt(TargetFriendly),
                                            "Type:", TargetTypeFriendly,
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
                    detailed += ((i > 0) ? "," : "") + JournalFieldNaming.RMat(CommodityReward[i].Item1) + " " + CommodityReward[i].Item2.ToStringInvariant();

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
                    detailed += ((i > 0) ? "," : "") + JournalFieldNaming.RMat(MaterialsReward[i].Item1) + " " + MaterialsReward[i].Item2.ToStringInvariant();

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
