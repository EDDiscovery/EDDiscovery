using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;

using static BaseUtils.UnitTests.CheckerHelpers;

namespace UnitTest
{
    public static class UnitTestWeapons
    {
        [BaseUtils.UnitTests.Test]
        public static void TestSuitsWeapons()
        {
            CheckSection("Suits");
            {
                string t1 = @"{ ""timestamp"":""2022-02-24T16:28:14Z"", ""event"":""BuySuit"", ""Name"":""TacticalSuit_Class3"", ""Name_Localised"":""$TacticalSuit_Class1_Name;"", ""Price"":11250000, ""SuitID"":1725662593809879, ""SuitMods"":[ ""suit_increasedammoreserves"", ""suit_xyz"" ] }";
                JournalBuySuit je = JournalEntry.CreateJournalEntry(t1) as JournalBuySuit;
                Check(je != null);
                Check(je.Name.Equals("TacticalSuit_Class3"));
                Check(je.SuitMods[0].Equals("suit_increasedammoreserves"));
                Check(je.SuitMods[1].Equals("suit_xyz"));
                CheckThat(je.SuitID).Is(new SuitID(1725662593809879));
            }
            {
                string t1 = @"{ ""timestamp"":""2022-03-19T18:53:16Z"", ""event"":""SellSuit"", ""SuitID"":1708610328553109, ""SuitMods"":[ ""suit_increasedammoreserves"", ""suit_xyz"" ], ""Name"":""tacticalsuit_class1"", ""Name_Localised"":""Dominator Suit"", ""Price"":90000 }";
                JournalSellSuit je = JournalEntry.CreateJournalEntry(t1) as JournalSellSuit;
                Check(je != null);
                Check(je.Name.Equals("TacticalSuit_Class1"));
                Check(je.SuitMods[0].Equals("suit_increasedammoreserves"));
                Check(je.SuitMods[1].Equals("suit_xyz"));
            }
            {
                string t1 = @"{ ""timestamp"":""2022-03-19T18:03:53Z"", ""event"":""CreateSuitLoadout"", ""SuitID"":1725662593809879, ""SuitName"":""tacticalsuit_class3"", ""SuitName_Localised"":""$TacticalSuit_Class1_Name;"", ""SuitMods"":[ ""suit_increasedammoreserves"" ], ""LoadoutID"":4293000011, ""LoadoutName"":""DOM-AMMO-L6-INT-P15"", "
                        + @"""Modules"":[ { ""SlotName"":""PrimaryWeapon1"", ""SuitModuleID"":1704554074259190, ""ModuleName"":""wpn_m_launcher_rocket_sauto"", ""ModuleName_Localised"":""Karma L-6"", ""Class"":3, ""WeaponMods"":[ ""weapon_handling"" ] },"
                                + @"{ ""SlotName"":""PrimaryWeapon2"", ""SuitModuleID"":1703487360143155, ""ModuleName"":""wpn_m_shotgun_plasma_doublebarrel"", ""ModuleName_Localised"":""Manticore Intimidator"", ""Class"":2, ""WeaponMods"":[  ] }, { ""SlotName"":""SecondaryWeapon"", ""SuitModuleID"":1704478266769994, ""ModuleName"":""wpn_s_pistol_kinetic_sauto"", ""ModuleName_Localised"":""Karma P-15"", ""Class"":3, ""WeaponMods"":[ ""x1""  ] } ] }";
                JournalCreateSuitLoadout je = JournalEntry.CreateJournalEntry(t1) as JournalCreateSuitLoadout;
                Check(je != null);
                Check(je.SuitName.Equals("TacticalSuit_Class3"));
                Check(je.SuitMods[0].Equals("suit_increasedammoreserves"));
                Check(je.Modules[0].SlotName == SuitLoadout.SuitSlot.PrimaryWeapon1);
                Check(je.Modules[0].ModuleName.Equals("Wpn_m_launcher_rocket_sauto"));
                Check(je.Modules[0].WeaponMods[0].Equals("weapon_handling"));
                Check(je.Modules[1].SlotName == SuitLoadout.SuitSlot.PrimaryWeapon2);
                Check(je.Modules[1].ModuleName.Equals("wpn_m_shotgun_plasma_doublebarrel"));
                CheckThat(je.Modules[0].SuitModuleID).Is(new WeaponID(1704554074259190));

                string t2 = @"{ ""timestamp"":""2021-03-29T16:45:21Z"", ""event"":""CreateSuitLoadout"", ""LoadoutID"":4293000001" +
@", ""LoadoutName"":""lets gooo"", ""Modules"":[ { ""SlotName"":""SecondaryWeapon""" +
@", ""ModuleName"":""wpn_s_pistol_kinetic_sauto"", ""ModuleName_Localised"":""Karma P-15"" } ] }";
                je = JournalEntry.CreateJournalEntry(t2) as JournalCreateSuitLoadout;
                Check(je != null);
                CheckThat(je.SuitName).IsNull();
            }

            {
                string t1 = @"{ ""timestamp"":""2021-03-30T06:26:11Z"", ""event"":""DeleteSuitLoadout"", ""LoadoutID"":4293000001" +
@", ""LoadoutName"":""lets gooo"" }";
                var je = JournalEntry.CreateJournalEntry(t1) as JournalDeleteSuitLoadout;
                Check(je != null);
                CheckThat(je.SuitName).IsNull();
                CheckThat(je.LoadoutName).Is("lets gooo");
                CheckThat(je.LoadoutID).Is(new LoadoutID(4293000001));

                string t2 = @"{ ""timestamp"":""2024-10-10T00:27:01Z"", ""event"":""DeleteSuitLoadout"", ""SuitID"":1802434327952478" +
@", ""SuitName"":""explorationsuit_class2"", ""SuitName_Localised"":""$ExplorationSuit_Class1_Name;""" +
@", ""LoadoutID"":4293000001, ""LoadoutName"":""Exploration"" }";
                je = JournalEntry.CreateJournalEntry(t2) as JournalDeleteSuitLoadout;
                Check(je != null);
                CheckThat(je.LoadoutName).Is("Exploration");
                CheckThat(je.SuitName).Is("explorationsuit_class2");
            }
            {
                // error situation, loadout iD missing, ensure jsonalwayscreate is making it
                string t1 = @"{ ""timestamp"":""2021-03-30T06:26:11Z"", ""event"":""DeleteSuitLoadout"", " + @"""LoadoutName"":""lets gooo"" }";
                var je = JournalEntry.CreateJournalEntry(t1) as JournalDeleteSuitLoadout;
                Check(je != null);
                CheckThat(je.SuitName).IsNull();
                CheckThat(je.LoadoutName).Is("lets gooo");
                CheckThat(je.LoadoutID).Is(new LoadoutID(0));
                CheckThat(je.LoadoutID.IsValid).Is(false);

            }

            CheckSection("Weapons");

            {
                string t1 = @"{ ""timestamp"":""2022-03-19T18:56:07Z"", ""event"":""SellWeapon"", ""Name"":""wpn_m_assaultrifle_laser_fauto"", ""Name_Localised"":""TK Aphelion"", ""Class"":1, ""WeaponMods"":[ ""xyz"" ], ""Price"":75000, ""SuitModuleID"":1700283549401507 }";
                JournalSellWeapon je = JournalEntry.CreateJournalEntry(t1) as JournalSellWeapon;
                Check(je != null);
                Check(je.Name.Equals("wpn_m_assaultrifle_laser_fauto"));
                Check(je.WeaponMods[0].Equals("xyz"));
            }

            {
                string t1 = @"{ ""timestamp"":""2021-07-01T13:15:18Z"", ""event"":""LoadoutEquipModule"", ""LoadoutName"":""DOM_C44c3-INT-ZENITH"", ""SuitID"":1702849450265015, ""SuitName"":""tacticalsuit_class3"", ""SuitName_Localised"":""$TacticalSuit_Class1_Name;"", ""LoadoutID"":4293000008, ""SlotName"":""SecondaryWeapon"", ""ModuleName"":""wpn_s_pistol_laser_sauto"", ""ModuleName_Localised"":""TK Zenith"", ""Class"":2, ""WeaponMods"":[ ""xyz"" ], ""SuitModuleID"":1700918996186162 }";
                JournalLoadoutEquipModule je = JournalEntry.CreateJournalEntry(t1) as JournalLoadoutEquipModule;
                Check(je != null);
                Check(je.SuitName.Equals("tacticalsuit_class3"));
                Check(je.WeaponMods[0].Equals("xyz"));
            }

        }
    }
}
