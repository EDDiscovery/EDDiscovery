using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;

using static BaseUtils.UnitTests.CheckerHelpers;

namespace UnitTest
{
    public static class UnitTestEngineer
    {
        [BaseUtils.UnitTests.Test]
        public static void TestEngineer()
        {
            CheckSection("Engineer");
            {
                string t1 = @"{ ""timestamp"":""2018-01-27T13:59:17Z"", ""event"":""EngineerApply"", ""Engineer"":""Hera Tani""" +
@", ""Blueprint"":""PowerPlant_Boosted"", ""Level"":5 }";
                var je = JournalEntry.CreateJournalEntry(t1) as JournalEngineerApply;
                Check(je != null);
                CheckThat(je.Engineer).Is("Hera Tani");
                CheckThat(je.FDBlueprint).Is("PowerPlant_Boosted");
                CheckThat(je.Blueprint).Is("Overcharged Power Plant");
                CheckThat(je.GetInfo()).Contains("Hera Tani");

                string t2 =
@"{ ""timestamp"":""2018-02-11T22:50:18Z"", ""event"":""EngineerApply"", ""Engineer"":""Liz Ryder""" +
@", ""Blueprint"":""Weapon_HighCapacity"", ""Level"":5, ""Override"":""special_drag_munitions"" }";
                je = JournalEntry.CreateJournalEntry(t2) as JournalEngineerApply;
                Check(je != null);
                CheckThat(je.Engineer).Is("Liz Ryder");
                CheckThat(je.FDBlueprint).Is("Weapon_HighCapacity");
                CheckThat(je.Blueprint).Is("High Capacity Magazine");
                CheckThat(je.Level).Is(5);
                CheckThat(je.FDOverride).Is("special_drag_munitions");
                CheckThat(je.Override).Is("Drag Munitions");
            }
            {
                string t1 =@"{ ""timestamp"":""2021-05-24T12:52:09Z"", ""event"":""EngineerContribution"", ""Engineer"":""Bill Turner""" +
@", ""EngineerID"":300010, ""Type"":""Commodity"", ""Commodity"":""bromellite""" +
@", ""Quantity"":50, ""TotalQuantity"":50 }";
                var je = JournalEntry.CreateJournalEntry(t1) as JournalEngineerContribution;
                Check(je != null);
                CheckThat(je.Engineer).Is("Bill Turner");
                CheckThat(je.EngineerID).Is(300010);
                CheckThat(je.Type).Is(JournalEngineerContribution.ContributionType.Commodity);
                CheckThat(je.Commodity).Is("bromellite");
                CheckThat(je.Commodity_Localised).Is("Bromellite");
                CheckThat(je.Quantity).Is(50);
                CheckThat(je.TotalQuantity).Is(50);

                string t2 = @"{ ""timestamp"":""2021-07-12T20:18:29Z"", ""event"":""EngineerContribution"", ""Engineer"":""Chloe Sedesi""" +
@", ""EngineerID"":300300, ""Type"":""Materials"", ""Material"":""unknownenergysource""" +
@", ""Material_Localised"":""Sensor Fragment"", ""Quantity"":25, ""TotalQuantity"":25 }";
                je = JournalEntry.CreateJournalEntry(t2) as JournalEngineerContribution;
                Check(je != null);
                CheckThat(je.Engineer).Is("Chloe Sedesi");
                CheckThat(je.EngineerID).Is(300300);
                CheckThat(je.Type).Is(JournalEngineerContribution.ContributionType.Materials);
                CheckThat(je.Material).Is("unknownenergysource");
                CheckThat(je.Material_Localised).Is("Sensor Fragment");
                CheckThat(je.Quantity).Is(25);
                CheckThat(je.TotalQuantity).Is(25);

                string t3 = @"{ ""timestamp"":""2021-07-20T16:56:28Z"", ""event"":""EngineerContribution"", ""Engineer"":""Colonel Bris Dekker""" +
@", ""EngineerID"":300140, ""Type"":""Bond"", ""Quantity"":651239, ""TotalQuantity"":1000000 }";
                je = JournalEntry.CreateJournalEntry(t3) as JournalEngineerContribution;
                Check(je != null);
                CheckThat(je.Engineer).Is("Colonel Bris Dekker");
                CheckThat(je.EngineerID).Is(300140);
                CheckThat(je.Type).Is(JournalEngineerContribution.ContributionType.Bond);
                CheckThat(je.Quantity).Is(651239);
                CheckThat(je.TotalQuantity).Is(1000000);
                CheckThat(je.GetInfo()).Contains("Colonel Bris Dekker");

            }
            {
                // very early one..

                string t1 = @"{ ""timestamp"":""2016-09-21T02:38:56Z"", ""event"":""EngineerCraft"", ""Engineer"":""Felicity Farseer""" +
@", ""Blueprint"":""FSD_LongRange"", ""Level"":5, ""Ingredients"":{""magneticemittercoil"":1" +
@", ""arsenic"":1, ""chemicalmanipulators"":1, ""dataminedwake"":1 } }";
                var je = JournalEntry.CreateJournalEntry(t1) as JournalEngineerCraft;
                Check(je != null);
                CheckThat(je.SlotFD).Is(ShipSlots.Slot.Unknown);
                CheckThat(je.ModuleFD).IsNull();
                CheckThat(je.Engineering.Engineer).Is("Felicity Farseer");
                CheckThat(je.Engineering.Level).Is(5);
                CheckThat(je.Engineering.BlueprintName).Is("FSD_LongRange");
                CheckThat(je.Engineering.FriendlyBlueprintName).Is("Increased FSD Range");

                string t2 = @"{ ""timestamp"":""2025-01-09T16:37:28Z"", ""event"":""EngineerCraft"", ""Slot"":""TinyHardpoint4""" +
@", ""Module"":""hpt_shieldbooster_size0_class5"", ""Ingredients"":[ { ""Name"":""shielddensityreports""" +
@", ""Name_Localised"":""Untypical Shield Scans"", ""Count"":1 }, { ""Name"":""polymercapacitors""" +
@", ""Name_Localised"":""Polymer Capacitors"", ""Count"":1 }, { ""Name"":""antimony""" +
@", ""Count"":1 } ], ""Engineer"":""Didi Vatermann"", ""EngineerID"":300000, ""BlueprintID"":128673784" +
@", ""BlueprintName"":""ShieldBooster_HeavyDuty"", ""Level"":5, ""Quality"":0.600000" +
@", ""ExperimentalEffect"":""special_shieldbooster_efficient"", ""ExperimentalEffect_Localised"":""Flow Control""" +
@", ""Modifiers"":[ { ""Label"":""Mass"", ""Value"":14.000000, ""OriginalValue"":3.500000" +
@", ""LessIsGood"":1 }, { ""Label"":""Integrity"", ""Value"":54.624001, ""OriginalValue"":48.000000" +
@", ""LessIsGood"":0 }, { ""Label"":""PowerDraw"", ""Value"":1.350000, ""OriginalValue"":1.200000" +
@", ""LessIsGood"":1 }, { ""Label"":""DefenceModifierShieldMultiplier"", ""Value"":62.240005" +
@", ""OriginalValue"":20.000004, ""LessIsGood"":0 } ] }";
                je = JournalEntry.CreateJournalEntry(t2) as JournalEngineerCraft;
                Check(je != null);
                CheckThat(je.SlotFD).Is(ShipSlots.Slot.TinyHardpoint4);
                CheckThat(je.ModuleFD).Is("hpt_shieldbooster_size0_class5");
                CheckThat(je.Ingredients[0].NameFD).Is("shielddensityreports");
                CheckThat(je.Ingredients[0].Name).Is("Untypical Shield Scans");
                CheckThat(je.Ingredients[0].Name_Localised).Is("Untypical Shield Scans");
                CheckThat(je.Engineering.Engineer).Is("Didi Vatermann");
                CheckThat(je.Engineering.BlueprintID).Is(128673784UL);
                CheckThat(je.Engineering.Level).Is(5);
                CheckThat(je.Engineering.BlueprintName).Is("ShieldBooster_HeavyDuty");
                CheckThat(je.Engineering.FriendlyBlueprintName).Is("Heavy Duty Shield Booster");
                CheckThat(je.Engineering.Quality).Is(0.6);
                CheckThat(je.Engineering.ExperimentalEffect).Is("special_shieldbooster_efficient");
                CheckThat(je.Engineering.FriendlyExperimentalEffect).Is("Flow Control");
                CheckThat(je.Engineering.ExperimentalEffect_Localised).Is("Flow Control");
                CheckThat(je.Engineering.Modifiers[0].Label).Is("Mass");
                CheckThat(je.Engineering.Modifiers[0].FriendlyLabel).Is("Mass");
                CheckThat(je.Engineering.Modifiers[0].Value).Is(14.0);
                CheckThat(je.Engineering.Modifiers[0].OriginalValue).Is(3.5);
                CheckThat(je.Engineering.Modifiers[0].LessIsGood).Is(true);
                CheckThat(je.Engineering.Modifiers[3].Label).Is("DefenceModifierShieldMultiplier");
                CheckThat(je.Engineering.Modifiers[3].FriendlyLabel).Is("Defence Modifier Shield Multiplier");
                CheckThat(je.GetInfo()).Contains("Didi");
                CheckThat(je.GetDetailed()).Contains("Untypical Shield Scans");
            }
            {
                string t1 = @"{ ""timestamp"":""2025-11-05T13:21:41Z"", ""event"":""EngineerProgress"", ""Engineers"":[ { ""Engineer"":""Hera Tani""" +
@", ""EngineerID"":300090, ""Progress"":""Unlocked"", ""RankProgress"":0, ""Rank"":5 }" +
@", { ""Engineer"":""The Sarge"", ""EngineerID"":300040, ""Progress"":""Invited"" }" +
@", { ""Engineer"":""Professor Palin"", ""EngineerID"":300220, ""Progress"":""Unlocked""" +
@", ""RankProgress"":0, ""Rank"":5 }, { ""Engineer"":""Felicity Farseer"", ""EngineerID"":300100" +
@", ""Progress"":""Unlocked"", ""RankProgress"":0, ""Rank"":5 }, { ""Engineer"":""Eleanor Bresa""" +
@", ""EngineerID"":400011, ""Progress"":""Known"" }, { ""Engineer"":""Hero Ferrari""" +
@", ""EngineerID"":400003, ""Progress"":""Unlocked"", ""RankProgress"":0, ""Rank"":0 }" +
@", { ""Engineer"":""Tiana Fortune"", ""EngineerID"":300270, ""Progress"":""Unlocked""" +
@", ""RankProgress"":0, ""Rank"":1 }, { ""Engineer"":""Jude Navarro"", ""EngineerID"":400001" +
@", ""Progress"":""Known"" }, { ""Engineer"":""Broo Tarquin"", ""EngineerID"":300030" +
@", ""Progress"":""Unlocked"", ""RankProgress"":0, ""Rank"":5 }, { ""Engineer"":""Etienne Dorn""" +
@", ""EngineerID"":300290, ""Progress"":""Unlocked"", ""RankProgress"":0, ""Rank"":5 }" +
@", { ""Engineer"":""Lori Jameson"", ""EngineerID"":300230, ""Progress"":""Invited"" }" +
@", { ""Engineer"":""Yarden Bond"", ""EngineerID"":400009, ""Progress"":""Invited"" }" +
@", { ""Engineer"":""Uma Laszlo"", ""EngineerID"":400007, ""Progress"":""Invited"" }" +
@", { ""Engineer"":""Bill Turner"", ""EngineerID"":300010, ""Progress"":""Unlocked""" +
@", ""RankProgress"":0, ""Rank"":5 }, { ""Engineer"":""Liz Ryder"", ""EngineerID"":300080" +
@", ""Progress"":""Unlocked"", ""RankProgress"":0, ""Rank"":5 }, { ""Engineer"":""Rosa Dayette""" +
@", ""EngineerID"":400012, ""Progress"":""Known"" }, { ""Engineer"":""Juri Ishmaak""" +
@", ""EngineerID"":300250, ""Progress"":""Unlocked"", ""RankProgress"":0, ""Rank"":5 }" +
@", { ""Engineer"":""Zacariah Nemo"", ""EngineerID"":300050, ""Progress"":""Unlocked""" +
@", ""RankProgress"":0, ""Rank"":5 }, { ""Engineer"":""Mel Brandon"", ""EngineerID"":300280" +
@", ""Progress"":""Unlocked"", ""RankProgress"":0, ""Rank"":5 }, { ""Engineer"":""Selene Jean""" +
@", ""EngineerID"":300210, ""Progress"":""Unlocked"", ""RankProgress"":0, ""Rank"":5 }" +
@", { ""Engineer"":""Marco Qwent"", ""EngineerID"":300200, ""Progress"":""Unlocked""" +
@", ""RankProgress"":0, ""Rank"":5 }, { ""Engineer"":""Chloe Sedesi"", ""EngineerID"":300300" +
@", ""Progress"":""Invited"" }, { ""Engineer"":""Baltanos"", ""EngineerID"":400010" +
@", ""Progress"":""Invited"" }, { ""Engineer"":""Petra Olmanova"", ""EngineerID"":300130" +
@", ""Progress"":""Invited"" }, { ""Engineer"":""Ram Tah"", ""EngineerID"":300110" +
@", ""Progress"":""Unlocked"", ""RankProgress"":0, ""Rank"":5 }, { ""Engineer"":""The Dweller""" +
@", ""EngineerID"":300180, ""Progress"":""Unlocked"", ""RankProgress"":0, ""Rank"":5 }" +
@", { ""Engineer"":""Elvira Martuuk"", ""EngineerID"":300160, ""Progress"":""Unlocked""" +
@", ""RankProgress"":0, ""Rank"":5 }, { ""Engineer"":""Lei Cheung"", ""EngineerID"":300120" +
@", ""Progress"":""Unlocked"", ""RankProgress"":0, ""Rank"":5 }, { ""Engineer"":""Kit Fowler""" +
@", ""EngineerID"":400004, ""Progress"":""Unlocked"", ""RankProgress"":0, ""Rank"":0 }" +
@", { ""Engineer"":""Colonel Bris Dekker"", ""EngineerID"":300140, ""Progress"":""Invited"" }" +
@", { ""Engineer"":""Didi Vatermann"", ""EngineerID"":300000, ""Progress"":""Unlocked""" +
@", ""RankProgress"":0, ""Rank"":5 }, { ""Engineer"":""Tod 'The Blaster' McQuinn""" +
@", ""EngineerID"":300260, ""Progress"":""Unlocked"", ""RankProgress"":0, ""Rank"":5 }" +
@", { ""Engineer"":""Domino Green"", ""EngineerID"":400002, ""Progress"":""Unlocked""" +
@", ""RankProgress"":0, ""Rank"":0 }, { ""Engineer"":""Wellington Beck"", ""EngineerID"":400005" +
@", ""Progress"":""Unlocked"", ""RankProgress"":0, ""Rank"":0 }, { ""Engineer"":""Marsha Hicks""" +
@", ""EngineerID"":300150, ""Progress"":""Invited"" } ] }";
                var je = JournalEntry.CreateJournalEntry(t1) as JournalEngineerProgress;
                Check(je != null);
                CheckThat(je.Engineers[0].Engineer).Is("Baltanos");
                CheckThat(je.Engineers[0].EngineerID).Is(400010);
                CheckThat(je.Engineers[0].Progress).Is(JournalEngineerProgress.ProgressType.Invited);
                CheckThat(je.GetDetailed()).Contains("Rank");
                CheckThat(je.GetInfo()).Contains("Progress");

                string t2 = @"{ ""timestamp"":""2016-09-20T10:20:19Z"", ""event"":""EngineerProgress"", ""Engineer"":""Ram Tah""" +
@", ""Progress"":""Invited"" }";
                je = JournalEntry.CreateJournalEntry(t2) as JournalEngineerProgress;
                Check(je != null);
            }

            { 
                string t1 = @"{ ""timestamp"":""2022-10-10T14:58:43Z"", ""event"":""Synthesis"", ""Name"":""Heat Sink Basic"", ""Materials"":[ { ""Name"":""basicconductors""" +
@", ""Name_Localised"":""Basic Conductors"", ""Count"":2 }, { ""Name"":""heatconductionwiring"", ""Name_Localised"":""Heat Conduction Wiring""" +
@", ""Count"":2 } ] }";
                var je = JournalEntry.CreateJournalEntry(t1) as JournalSynthesis;
                Check(je != null);
                CheckThat(je.FDName).Is(new SynthesisRecipeFDName("Heat Sink Basic"));
                CheckThat(je.Materials[new MCFDName("basicconductors")]).Is(2);
                CheckThat(je.Materials[new MCFDName("heatconductionwiring")]).Is(2);

            }
        }
    }
}
