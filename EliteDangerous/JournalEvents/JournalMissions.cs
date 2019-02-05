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
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.Missions)]
    public class JournalMissions : JournalEntry, IMissions
    {
        public JournalMissions(JObject evt) : base(evt, JournalTypeEnum.Missions)
        {
            ActiveMissions = evt["Active"]?.ToObjectProtected<MissionItem[]>();
            Normalise(ActiveMissions);
            FailedMissions = evt["Failed"]?.ToObjectProtected<MissionItem[]>();
            Normalise(FailedMissions);
            CompletedMissions = evt["Complete"]?.ToObjectProtected<MissionItem[]>();
            Normalise(CompletedMissions);
        }

        public MissionItem[] ActiveMissions { get; set; }
        public MissionItem[] FailedMissions { get; set; }
        public MissionItem[] CompletedMissions { get; set; }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = BaseUtils.FieldBuilder.Build("Active:".Txb(this), ActiveMissions?.Length, "Failed:".Txb(this), FailedMissions?.Length, "Completed:".Txb(this), CompletedMissions?.Length);
            detailed = "";
            if (ActiveMissions != null && ActiveMissions.Length>0)
            {
                detailed = detailed.AppendPrePad("Active:".Txb(this), Environment.NewLine);
                foreach (var x in ActiveMissions)
                    detailed = detailed.AppendPrePad("    " + x.Format(), Environment.NewLine);
            }
            if (FailedMissions != null && FailedMissions.Length>0)
            {
                detailed = detailed.AppendPrePad("Failed:".Txb(this), Environment.NewLine);
                foreach (var x in FailedMissions)
                    detailed = detailed.AppendPrePad("    " + x.Format(), Environment.NewLine);
            }
            if (CompletedMissions != null && CompletedMissions.Length > 0)
            {
                detailed = detailed.AppendPrePad("Completed:".Txb(this), Environment.NewLine);
                foreach (var x in CompletedMissions)
                    detailed = detailed.AppendPrePad("    " + x.Format(), Environment.NewLine);
            }

        }

        public void UpdateMissions(MissionListAccumulator mlist, EliteDangerousCore.ISystem sys, string body, DB.SQLiteConnectionUser conn)
        {
            mlist.Missions(this);       // check vs our mission list
        }


        public void Normalise(MissionItem[] array)
        {
            if (array != null)
                foreach (var x in array)
                    x.Normalise(EventTimeUTC);
        }

        public class MissionItem
        {
            public int MissionID;
            public string Name;
            public bool PassengerMission;
            public int Expires;

            DateTime ExpiryTimeUTC;

            public void Normalise(DateTime utcnow)
            {
                ExpiryTimeUTC = utcnow.AddSeconds(Expires);
                Name = JournalFieldNaming.GetBetterMissionName(Name);       // Names are normalised, per MissionAccepted
            }

            public string Format()
            {
                return BaseUtils.FieldBuilder.Build("", Name, "<;(Passenger)".Tx(this), PassengerMission, " " + "Expires:".Tx(this), ExpiryTimeUTC.ToLocalTime());
            }
        }
    }


    [JournalEntryType(JournalTypeEnum.MissionAccepted)]
    public class JournalMissionAccepted : JournalEntry, IMissions, ICommodityJournalEntry
    {
        public JournalMissionAccepted(JObject evt) : base(evt, JournalTypeEnum.MissionAccepted)
        {
            Faction = evt["Faction"].Str();
            FDName = evt["Name"].Str();
            Name = JournalFieldNaming.GetBetterMissionName(FDName);
            LocalisedName = evt["LocalisedName"].Str(); 

            TargetType = evt["TargetType"].Str();
            TargetTypeFriendly = JournalFieldNaming.GetBetterTargetTypeName(TargetType);    // remove $, underscore it
            TargetTypeLocalised = JournalFieldNaming.CheckLocalisation(evt["TargetType_Localised"].Str(), TargetTypeFriendly);

            TargetFaction = evt["TargetFaction"].Str();

            Target = evt["Target"].Str();
            TargetFriendly = JournalFieldNaming.GetBetterTargetTypeName(Target);        // remove $, underscore it
            TargetLocalised = JournalFieldNaming.CheckLocalisation(evt["Target_localised"].Str(), TargetFriendly);        // not all

            KillCount = evt["KillCount"].IntNull();

            DestinationSystem = evt["DestinationSystem"].Str().Replace("$MISSIONUTIL_MULTIPLE_INNER_SEPARATOR;", ",")
                                                              .Replace("$MISSIONUTIL_MULTIPLE_FINAL_SEPARATOR;", ",");       // multi missions get this strange list;
            DestinationStation = evt["DestinationStation"].Str();

            Influence = evt["Influence"].Str();
            Reputation = evt["Reputation"].Str();

            MissionId = evt["MissionID"].Int();

            Commodity = JournalFieldNaming.FixCommodityName(evt["Commodity"].Str());        // instances of $_name, fix to fdname
            FriendlyCommodity = MaterialCommodityData.GetNameByFDName(Commodity);
            CommodityLocalised = JournalFieldNaming.CheckLocalisationTranslation(evt["Commodity_Localised"].Str(), FriendlyCommodity);

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
        public string LocalisedName { get; private set; }
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

        private static DateTime ED32Date = new DateTime(2018, 8, 28, 10, 0, 0);

        public string MissionBasicInfo()          // MissionList::FullInfo uses this. Journal Entry info brief uses this
        {
            DateTime exp = Expiry;
            if (exp != null && !EliteConfigInstance.InstanceConfig.DisplayUTC)
                exp = exp.ToLocalTime();

            return BaseUtils.FieldBuilder.Build("", Name,
                                      "< from ".Tx(this), Faction,
                                      "System:".Txb(this), DestinationSystem,
                                      "Station:".Txb(this), DestinationStation,
                                      "Expiry:".Tx(this), exp,
                                      "Influence:".Tx(this), Influence,
                                      "Reputation:".Tx(this), Reputation,
                                      "Reward:; cr;N0".Tx(this), Reward,
                                      "; (Wing)".Tx(this), Wing);
        }

        public string MissionDetailedInfo()          // MissionList::FullInfo (DLL uses this), Journal Entry detailed info
        {
            return BaseUtils.FieldBuilder.Build("Deliver:".Tx(this), CommodityLocalised,
                                           "Target:".Txb(this), TargetLocalised,
                                           "Type:".Txb(this), TargetTypeFriendly,
                                           "Target Faction:".Txb(this), TargetFaction,
                                           "Target Type:".Tx(this), TargetTypeLocalised,
                                           "Kill Count:".Tx(this), KillCount,
                                           "Passengers:".Tx(this), PassengerCount);
        }

        public string MissionAuxInfo()          //  MissionList:info, used for MissionList:Info, used in mission panels.
        {
            return BaseUtils.FieldBuilder.Build("", LocalisedName,
                                        "Influence:".Tx(this), Influence,
                                        "Reputation:".Tx(this), Reputation,
                                        "Deliver:".Tx(this), CommodityLocalised,
                                        "Target:".Txb(this), TargetLocalised,
                                        "Type:".Txb(this), TargetTypeFriendly,
                                        "Target Faction:".Txb(this), TargetFaction,
                                        "Target Type:".Tx(this), TargetTypeLocalised,
                                        "Passengers:".Tx(this), PassengerCount,
                                        "Count:".Tx(this), Count);

        }

        public void UpdateMissions(MissionListAccumulator mlist, EliteDangerousCore.ISystem sys, string body, DB.SQLiteConnectionUser conn)
        {
            mlist.Accepted(this, sys, body);
        }

        public void UpdateCommodities(MaterialCommoditiesList mc, SQLiteConnectionUser conn)
        {
            if (Commodity != null && Count != null && DeliveryMissions.Contains(FDName) && EventTimeUTC < ED32Date)
            {
                mc.Change(MaterialCommodityData.CommodityCategory, Commodity, (int)Count, 0, conn);
            }
        }
    }



    [JournalEntryType(JournalTypeEnum.MissionCompleted)]
    public class JournalMissionCompleted : JournalEntry, ICommodityJournalEntry, IMaterialJournalEntry, ILedgerJournalEntry, IMissions
    {
        public JournalMissionCompleted(JObject evt) : base(evt, JournalTypeEnum.MissionCompleted)
        {
            FDName = evt["Name"].Str();
            Name = JournalFieldNaming.GetBetterMissionName(FDName);
            Faction = evt["Faction"].Str();

            Commodity = JournalFieldNaming.FixCommodityName(evt["Commodity"].Str());             // evidence of $_name problem, fix to fdname
            Commodity = JournalFieldNaming.FDNameTranslation(Commodity);     // pre-mangle to latest names, in case we are reading old journal records
            FriendlyCommodity = MaterialCommodityData.GetNameByFDName(Commodity);
            CommodityLocalised = JournalFieldNaming.CheckLocalisationTranslation(evt["Commodity_Localised"].Str(), FriendlyCommodity);

            Count = evt["Count"].IntNull();

            TargetType = evt["TargetType"].Str();
            TargetTypeFriendly = JournalFieldNaming.GetBetterTargetTypeName(TargetType);        // remove $, underscores etc
            TargetTypeLocalised = JournalFieldNaming.CheckLocalisation(evt["TargetTypeLocalised"].Str(), TargetTypeFriendly);     // may be empty..

            TargetFaction = evt["TargetFaction"].Str();

            Target = evt["Target"].Str();
            TargetFriendly = JournalFieldNaming.GetBetterTargetTypeName(Target);        // remove $, underscores etc
            TargetLocalised = JournalFieldNaming.CheckLocalisation(evt["Target_Localised"].Str(), TargetFriendly);        // copied from Accepted.. no evidence

            Reward = evt["Reward"].LongNull();
            Donation = evt["Donation"].LongNull();
            MissionId = evt["MissionID"].Int();

            DestinationSystem = evt["DestinationSystem"].Str().Replace("$MISSIONUTIL_MULTIPLE_INNER_SEPARATOR;", ",")
                                                              .Replace("$MISSIONUTIL_MULTIPLE_FINAL_SEPARATOR;", ",");       // multi missions get this strange list;
            DestinationStation = evt["DestinationStation"].Str();

            PermitsAwarded = evt["PermitsAwarded"]?.ToObjectProtected<string[]>();

            // 7/3/2018 journal 16 3.02

            CommodityReward = evt["CommodityReward"]?.ToObjectProtected<CommodityRewards[]>();

            if (CommodityReward != null)
            {
                foreach (CommodityRewards c in CommodityReward)
                    c.Normalise();
            }

            MaterialsReward = evt["MaterialsReward"]?.ToObjectProtected<MaterialRewards[]>();

            if (MaterialsReward != null)
            {
                foreach (MaterialRewards m in MaterialsReward)
                    m.Normalise();
            }

            FactionEffects = evt["FactionEffects"]?.ToObjectProtected<FactionEffectsEntry[]>();      // NEEDS TEST
        }

        public string Name { get; set; }
        public string FDName { get; set; }
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

        public void UpdateCommodities(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            if (Commodity != null && Count != null)
            {
                mc.Change(MaterialCommodityData.CommodityCategory, Commodity, -(int)Count, 0, conn);
            }

            if (CommodityReward != null)
            {
                foreach (CommodityRewards c in CommodityReward)
                    mc.Change(MaterialCommodityData.CommodityCategory, c.Name, c.Count, 0, conn);
            }
        }

        public void UpdateMaterials(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            if (MaterialsReward != null)
            {
                foreach (MaterialRewards m in MaterialsReward)                 // 7/3/2018 not yet fully proven.. changed in 3.02
                {
                    string c = m.Category.Alt(MaterialCommodityData.MaterialRawCategory);     // older ones did not have this tag..
                    mc.Change(c, m.Name, m.Count, 0, conn);
                }
            }
        }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            long rv = Reward.HasValue ? Reward.Value : 0;
            long dv = Donation.HasValue ? Donation.Value : 0;

            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Name, (rv - dv), 0);
        }

        public void UpdateMissions(MissionListAccumulator mlist, EliteDangerousCore.ISystem sys, string body, DB.SQLiteConnectionUser conn)
        {
            mlist.Completed(this);
        }

        public override void FillInformation(out string info, out string detailed)
        {

            info = BaseUtils.FieldBuilder.Build("", Name,
                                        "< from ".Txb(this), Faction,
                                        "Reward:; cr;N0".Txb(this), Reward,
                                        "Donation:".Txb(this), Donation,
                                        "System:".Txb(this), DestinationSystem,
                                        "Station:".Txb(this), DestinationStation);

            detailed = BaseUtils.FieldBuilder.Build("Commodity:".Txb(this), CommodityLocalised,
                                            "Target:".Txb(this), TargetLocalised,
                                            "Type:".Txb(this), TargetTypeLocalised,
                                            "Target Faction:".Txb(this), TargetFaction);

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
                    detailed += "Permits:".Txb(this);

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
                    detailed += "Rewards:".Txb(this);
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
                    detailed += "Rewards:".Txb(this);

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
            public string Name_Localised;       // may be null on reading
            public string Category; // may be null
            public string Category_Localised; // may be null
            public int Count;

            public void Normalise()
            {
                Name = JournalFieldNaming.FDNameTranslation(Name);
                FriendlyName = MaterialCommodityData.GetNameByFDName(Name);
                Name_Localised = JournalFieldNaming.CheckLocalisationTranslation(Name_Localised ?? "", FriendlyName);

                if (Category != null)
                {
                    Category = JournalFieldNaming.NormaliseMaterialCategory(Category);
                    Category_Localised = JournalFieldNaming.CheckLocalisation(Category_Localised ?? "", Category);
                }
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
                FriendlyName = MaterialCommodityData.GetNameByFDName(Name);
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


    [JournalEntryType(JournalTypeEnum.MissionFailed)]
    public class JournalMissionFailed : JournalEntry, IMissions
    {
        public JournalMissionFailed(JObject evt) : base(evt, JournalTypeEnum.MissionFailed)
        {
            FDName = evt["Name"].Str();
            Name = JournalFieldNaming.GetBetterMissionName(FDName);
            MissionId = evt["MissionID"].Int();
            Fine = evt["Fine"].LongNull();
        }

        public string Name { get; set; }
        public string FDName { get; set; }
        public int MissionId { get; set; }
        public long? Fine { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {

            info = BaseUtils.FieldBuilder.Build("", Name, "Fine:".Txb(this), Fine);
            detailed = "";
        }

        public void UpdateMissions(MissionListAccumulator mlist, EliteDangerousCore.ISystem sys, string body, DB.SQLiteConnectionUser conn)
        {
            mlist.Failed(this);
        }

    }


    [JournalEntryType(JournalTypeEnum.MissionRedirected)]
    public class JournalMissionRedirected : JournalEntry, IMissions
    {
        public JournalMissionRedirected(JObject evt) : base(evt, JournalTypeEnum.MissionRedirected)
        {
            FDName = evt["Name"].Str();
            Name = JournalFieldNaming.GetBetterMissionName(FDName);
            MissionId = evt["MissionID"].Int();
            NewDestinationStation = evt["NewDestinationStation"].Str();
            OldDestinationStation = evt["OldDestinationStation"].Str();
            NewDestinationSystem = evt["NewDestinationSystem"].Str();
            OldDestinationSystem = evt["OldDestinationSystem"].Str();
        }

        public string NewDestinationStation { get; set; }
        public string OldDestinationStation { get; set; }
        public string NewDestinationSystem { get; set; }
        public string OldDestinationSystem { get; set; }

        public int MissionId { get; set; }
        public string Name { get; set; }
        public string FDName { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = info = BaseUtils.FieldBuilder.Build("Mission name:".Txb(this), Name,
                                      "From:".Tx(this), OldDestinationSystem,
                                      "", OldDestinationStation,
                                      "To:".Tx(this), NewDestinationSystem,
                                      "", NewDestinationStation);

            detailed = "";
        }

        public void UpdateMissions(MissionListAccumulator mlist, EliteDangerousCore.ISystem sys, string body, DB.SQLiteConnectionUser conn)
        {
            mlist.Redirected(this);
        }

    }



    [JournalEntryType(JournalTypeEnum.MissionAbandoned)]
    public class JournalMissionAbandoned : JournalEntry, IMissions
    {
        public JournalMissionAbandoned(JObject evt) : base(evt, JournalTypeEnum.MissionAbandoned)
        {
            FDName = evt["Name"].Str();
            Name = JournalFieldNaming.GetBetterMissionName(FDName);
            MissionId = evt["MissionID"].Int();
            Fine = evt["Fine"].LongNull();
        }

        public string Name { get; set; }
        public string FDName { get; set; }
        public int MissionId { get; set; }
        public long? Fine { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", Name, "Fine:".Txb(this), Fine);
            detailed = "";
        }

        public void UpdateMissions(MissionListAccumulator mlist, EliteDangerousCore.ISystem sys, string body, DB.SQLiteConnectionUser conn)
        {
            mlist.Abandoned(this);
        }

    }


}
