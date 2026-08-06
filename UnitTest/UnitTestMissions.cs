using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;

using static BaseUtils.UnitTests.CheckerHelpers;

namespace UnitTest
{
    public partial class UnitTests
    {
        public static class UnitTestMissions
        {
            [BaseUtils.UnitTests.Test]
            public static void Missions()
            {
                CheckSection("Missions");
                {
                    string t1 = @"{""timestamp"":""2020-03-31T11:19:41Z"",""event"":""Missions"",""Active"":[{""MissionID"":559130037,""Name"":""Mission_DeliveryWing_name"",""PassengerMission"":false,""Expires"":28698}],""Failed"":[],""Complete"":[]}";
                    var je = JournalEntry.CreateJournalEntry(t1) as JournalMissions;
                    Check( je != null);
                    Check( je.ActiveMissions[0].Name.Equals("Mission_DeliveryWing_name"));
                    Check( je.ActiveMissions[0].Name_Localised.Equals("Mission Delivery Wing"));
                }
                {
                    string t1 = @"{""timestamp"":""2022-11-28T10:53:13Z"",""event"":""MissionAbandoned"",""Name"":""Mission_OnFoot_SalvageIllegal_BS_MB_name"",""MissionID"":902580000}";
                    var je = JournalEntry.CreateJournalEntry(t1) as JournalMissionAbandoned;
                    Check( je != null);
                    Check(je.FDName.Equals("Mission_OnFoot_SalvageIllegal_BS_MB_name"));

                    CheckThat(je).IsNotNull();
                    CheckThat(je.Name).Equals("Mission On Foot Salvage Illegal BS MB");
                }
                {
                    string t1 = @"{""timestamp"":""2021-08-17T16:38:09Z"",""event"":""MissionRedirected"",""MissionID"":800065361,""Name"":""Mission_OnFoot_Collect_MB"",""NewDestinationStation"":""Oliver Market"",""NewDestinationSystem"":""Adriatikuru"",""OldDestinationStation"":""Giudice Command Outpost"",""OldDestinationSystem"":""Adriatikuru""}";
                    var je = JournalEntry.CreateJournalEntry(t1) as JournalMissionRedirected;
                    CheckThat(je).IsNotNull();
                    CheckThat(je.FDName).Is(new MissionFDName("Mission_OnFoot_Collect_MB"));
                    CheckThat(je.Name).Is("Mission On Foot Collect MB");
                    CheckThat(je.LocalisedName).Is("Mission On Foot Collect MB");
                }
                {
                    string t1 = @"{""timestamp"":""2023-04-16T11:33:57Z"",""event"":""MissionFailed"",""Name"":""Mission_OnFoot_Defence_MacGuffin_MB_StandardCanister_name"",""MissionID"":924234541}";
                    var je = JournalEntry.CreateJournalEntry(t1) as JournalMissionFailed;
                    CheckThat(je).IsNotNull();
                    CheckThat(je.FDName).Equals(new MissionFDName("Mission_OnFoot_Defence_MacGuffin_MB_StandardCanister_name"));
                }
                {
                    string t1 = @"{ ""timestamp"":""2021-08-04T12:47:55Z"", ""event"":""MissionCompleted"", ""Faction"":""Lotian Crimson Brotherhood"", ""Name"":""Mission_OnFoot_Salvage_MB_name"", ""MissionID"":797165185, ""Commodity"":""$SurveillanceEquipment_Name;"", ""Commodity_Localised"":""Surveillance Equipment"", ""Count"":1, ""Reward"":10000, ""FactionEffects"":[ { ""Faction"":""Lotian Crimson Brotherhood"", ""Effects"":[  ], ""Influence"":[ { ""SystemAddress"":5067658962369, ""Trend"":""UpGood"", ""Influence"":""+"" } ], ""ReputationTrend"":""UpGood"", ""Reputation"":""+++"" } ] }";
                    var je = JournalEntry.CreateJournalEntry(t1) as JournalMissionCompleted;
                    CheckThat(je).IsNotNull();
                    CheckThat(je.FDName).Equals(new MissionFDName("Mission_OnFoot_Salvage_MB_name"));
                    CheckThat(je.Commodity).Equals(new MCFDName("SurveillanceEquipment"));
                    CheckThat(je.CommodityLocalised).Equals("Surveillance Equipment");
                }
                {
                    string t1 = @"{ ""timestamp"":""2023-04-01T15:16:10Z"", ""event"":""MissionCompleted"", ""Faction"":""Liberals of NLTT 8653""" +
                                @", ""Name"":""Mission_OnFoot_SalvageIllegal_MB_name"", ""MissionID"":922088450" +
                                @", ""Commodity"":""$MemoryChip_Name;"", ""Commodity_Localised"":""Memory Chip""" +
                                @", ""Count"":1, ""Reward"":10000, ""FactionEffects"":[ { ""Faction"":""Liberals of NLTT 8653""" +
                                @", ""Effects"":[  ], ""Influence"":[ { ""SystemAddress"":2282942829266, ""Trend"":""UpGood""" +
                                @", ""Influence"":""++"" } ], ""ReputationTrend"":""UpGood"", ""Reputation"":""+++"" } ] }";

                    var je = JournalEntry.CreateJournalEntry(t1) as JournalMissionCompleted;
                    CheckThat(je).IsNotNull();
                    CheckThat(je.FDName).Equals(new MissionFDName("Mission_OnFoot_SalvageIllegal_MB_name"));
                    CheckThat(je.Commodity).Equals(new MCFDName("MemoryChip"));
                    CheckThat(je.Reward).Equals(10000);
                    CheckThat(je.FactionEffects[0].Faction).Equals("Liberals of NLTT 8653");
                    CheckThat(je.FactionEffects[0].Influence[0].SystemAddress.Equals(2282942829266));
                }
                {
                    string t1 = @"{ ""timestamp"":""2023-04-01T15:16:10Z"", ""event"":""MissionCompleted"", ""Faction"":""Liberals of NLTT 8653""" +
                                @", ""Name"":""Mission_OnFoot_SalvageIllegal_MB_name"", ""MissionID"":922088450" +
                                @", ""Count"":1, ""Reward"":10000, ""FactionEffects"":[ { ""Faction"":""Liberals of NLTT 8653""" +
                                @", ""Effects"":[  ], ""Influence"":[ { ""SystemAddress"":2282942829266, ""Trend"":""UpGood""" +
                                @", ""Influence"":""++"" } ], ""ReputationTrend"":""UpGood"", ""Reputation"":""+++"" } ] }";

                    var je = JournalEntry.CreateJournalEntry(t1) as JournalMissionCompleted;
                    CheckThat(je).IsNotNull();
                    CheckThat(je.FDName).Equals(new MissionFDName("Mission_OnFoot_SalvageIllegal_MB_name"));
                    CheckThat(je.Commodity).IsNull();
                    CheckThat(je.Reward).Equals(10000);
                    CheckThat(je.FactionEffects[0].Faction).Equals("Liberals of NLTT 8653");
                    CheckThat(je.FactionEffects[0].Influence[0].SystemAddress.Equals(2282942829266));
                }
                {
                    string t1 = @"{ ""timestamp"":""2023-04-16T11:20:08Z"", ""event"":""MissionAccepted"", ""Faction"":""Eurybia Blue Mafia""" +
                                @", ""Name"":""Mission_OnFoot_Delivery_Contact_MB"", ""LocalisedName"":""Take a package to Vern Raymond""" +
                                @", ""Commodity"":""$SuitSchematic_Name;"", ""Commodity_Localised"":""Suit Schematic""" +
                                @", ""Count"":1, ""DestinationSystem"":""LTT 1598"", ""DestinationSettlement"":""Kabbah Cultivation Biosphere""" +
                                @", ""Target"":""Vern Raymond"", ""Expiry"":""2023-04-16T17:27:13Z"", ""Wing"":false" +
                                @", ""Influence"":""+"", ""Reputation"":""+"", ""Reward"":72583, ""MissionID"":924234566 }";

                    var je = JournalEntry.CreateJournalEntry(t1) as JournalMissionAccepted;
                    CheckThat(je).IsNotNull();
                    CheckThat(je.FDName).Equals(new MissionFDName("Mission_OnFoot_Delivery_Contact_MB"));
                    CheckThat(je.LocalisedName).Is("Take a package to Vern Raymond");
                    CheckThat(je.Commodity).Equals(new MCFDName("SuitSchematic"));
                    CheckThat(je.DestinationSystem).Equals("LTT 1598");
                    CheckThat(je.DestinationSettlement).Equals("Kabbah Cultivation Biosphere");
                    CheckThat(je.Target).Is("Vern Raymond");
                }
            }
        }
    }
}

