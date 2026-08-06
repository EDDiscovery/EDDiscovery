using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;

using static BaseUtils.UnitTests.CheckerHelpers;

namespace UnitTest
{
    public static class UnitTestModules
    {
        [BaseUtils.UnitTests.Test]
        public static void TestModules()
        {
            CheckSection("Modules");

            {
                CheckThat(ItemData.TryGetShipModule(new ModFDName("paintjob_python_fullmetal_COPPER"), out ItemData.ShipModule m1, true)).IsTrue();
                CheckThat(ItemData.TryGetShipModule(new ModFDName("int_FSDinterdictor_size2_class3"), out ItemData.ShipModule m2, true)).IsTrue();
            }

            {
                string t1 = @"{ ""timestamp"":""2025-07-25T15:07:59Z"", ""event"":""Loadout"", ""Ship"":""panthermkii"", ""ShipID"":59, ""ShipName"":"""", ""ShipIdent"":""RO-25P"", ""HullHealth"":1.000000, ""UnladenMass"":1773.599976, ""CargoCapacity"":1040, ""MaxJumpRange"":20.758162, ""FuelCapacity"":{ ""Main"":128.000000, ""Reserve"":1.110000 }, ""Rebuy"":0, ""Modules"":"
                        + @"[ { ""Slot"":""LargeHardpoint1"", ""Item"":""hpt_pulselaser_turret_large"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""LargeHardpoint2"", ""Item"":""hpt_pulselaser_turret_large"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""MediumHardpoint1"", ""Item"":""hpt_multicannon_turret_medium"", ""On"":true, ""Priority"":2, ""AmmoInClip"":90, ""AmmoInHopper"":2100, ""Health"":1.000000 }, { ""Slot"":""MediumHardpoint2"", ""Item"":""hpt_multicannon_turret_medium"", ""On"":true, ""Priority"":2, ""AmmoInClip"":90, ""AmmoInHopper"":2100, ""Health"":1.000000 }, { ""Slot"":""MediumHardpoint3"", ""Item"":""hpt_multicannon_turret_medium"", ""On"":true, ""Priority"":2, ""AmmoInClip"":90, ""AmmoInHopper"":2100, ""Health"":1.000000 }, { ""Slot"":""MediumHardpoint4"", ""Item"":""hpt_multicannon_turret_medium"", ""On"":true, ""Priority"":2, ""AmmoInClip"":90, ""AmmoInHopper"":2100, ""Health"":1.000000 }, { ""Slot"":""SmallHardpoint1"", ""Item"":""hpt_pulselaser_turret_small"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""SmallHardpoint2"", ""Item"":""hpt_pulselaser_turret_small"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""SmallHardpoint3"", ""Item"":""hpt_pulselaser_turret_small"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""SmallHardpoint4"", ""Item"":""hpt_pulselaser_turret_small"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""TinyHardpoint1"", ""Item"":""hpt_shieldbooster_size0_class5"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""TinyHardpoint2"", ""Item"":""hpt_shieldbooster_size0_class5"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""TinyHardpoint3"", ""Item"":""hpt_shieldbooster_size0_class5"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""TinyHardpoint4"", ""Item"":""hpt_electroniccountermeasure_tiny"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""TinyHardpoint5"", ""Item"":""hpt_plasmapointdefence_turret_tiny"", ""On"":true, ""Priority"":2, ""AmmoInClip"":12, ""AmmoInHopper"":10000, ""Health"":1.000000 }, { ""Slot"":""TinyHardpoint6"", ""Item"":""hpt_heatsinklauncher_turret_tiny"", ""On"":true, ""Priority"":2, ""AmmoInClip"":1, ""AmmoInHopper"":2, ""Health"":1.000000 }, { ""Slot"":""Armour"", ""Item"":""panthermkii_armour_grade1"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""PaintJob"", ""Item"":""paintjob_panthermkii_01_02"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""PowerPlant"", ""Item"":""int_powerplant_size8_class5"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""MainEngines"", ""Item"":""int_engine_size8_class5"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""FrameShiftDrive"", ""Item"":""int_hyperdrive_overcharge_size7_class5"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""LifeSupport"", ""Item"":""int_lifesupport_size5_class5"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""PowerDistributor"", ""Item"":""int_powerdistributor_size7_class5"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""Radar"", ""Item"":""int_sensors_size5_class5"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""FuelTank"", ""Item"":""int_fueltank_size7_class3"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""Decal1"", ""Item"":""decal_pantherownersclub_01"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""Decal2"", ""Item"":""decal_pantherownersclub_01"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""Decal3"", ""Item"":""decal_pantherownersclub_01"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""Cargo01"", ""Item"":""int_largecargorack_size8_class1"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""Slot01_Size8"", ""Item"":""int_cargorack_size8_class1"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""Cargo02"", ""Item"":""int_largecargorack_size7_class1"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""Slot02_Size7"", ""Item"":""int_shieldgenerator_size7_class5"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""Slot03_Size6"", ""Item"":""int_cargorack_size6_class1"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""Slot04_Size6"", ""Item"":""int_cargorack_size6_class1"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""Slot05_Size6"", ""Item"":""int_fuelscoop_size6_class5"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""Slot06_Size5"", ""Item"":""int_cargorack_size5_class1"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""Slot07_Size5"", ""Item"":""int_cargorack_size5_class1"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""Slot08_Size4"", ""Item"":""int_cargorack_size4_class1"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""Slot09_Size2"", ""Item"":""int_dockingcomputer_advanced"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""Slot10_Size1"", ""Item"":""int_supercruiseassist"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""PlanetaryApproachSuite"", ""Item"":""int_planetapproachsuite_advanced"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""VesselVoice"", ""Item"":""voicepack_verity"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""ShipKitSpoiler"", ""Item"":""panthermkii_shipkita_spoiler2"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""ShipKitWings"", ""Item"":""panthermkii_shipkita_wings2"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""ShipKitBumper"", ""Item"":""panthermkii_shipkita_bumper1"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""ShipKitTail"", ""Item"":""panthermkii_shipkita_tail1"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""CargoHatch"", ""Item"":""modularcargobaydoor"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""ShipCockpit"", ""Item"":""panthermkii_cockpit"", ""On"":true, ""Priority"":1, ""Health"":1.000000 } ] }";
                JournalLoadout je = JournalEntry.CreateJournalEntry(t1) as JournalLoadout;
                Check(je != null);
                CheckThat(je.ShipFD).Is(new VehicleFDName("Panthermkii"));
                Check(je.ShipModules[0].ItemFD == new ModFDName("hpt_pulselaser_turret_large"));
                Check(je.ShipModules[0].SlotFD == ShipSlots.Slot.LargeHardpoint1);
                Check(je.ShipModules[je.ShipModules.Count - 1].ItemFD == new VehicleFDName("panthermkii_cockpit"));
                Check(je.ShipModules[je.ShipModules.Count - 1].SlotFD == ShipSlots.Slot.ShipCockpit);

                ShipList sl = new ShipList();
                je.ShipInformation(sl, null, null);
                var ship = sl.GetShip(new ShipID(59));
                Check(ship != null && ship.ShipFD.ID == "panthermkii");
            }

            {
                string t1 = @"{ ""timestamp"":""2026-07-01T08:41:05Z"", ""event"":""Outfitting"", ""MarketID"":128666762, ""StationName"":""Jameson Memorial"", ""StarSystem"":""Shinrarta Dezhra"", ""Horizons"":true, ""Items"":[
{ ""id"":129022085, ""Name"":""hpt_atmulticannon_turret_large_v2"", ""BuyPrice"":4026594 },
{ ""id"":129022080, ""Name"":""hpt_atmulticannon_fixed_medium_v2"", ""BuyPrice"":399331 },
{ ""id"":129022084, ""Name"":""hpt_atmulticannon_fixed_large_v2"", ""BuyPrice"":1193683 },
{ ""id"":129022082, ""Name"":""hpt_atdumbfiremissile_turret_large_v2"", ""BuyPrice"":4692462 },
{ ""id"":129022083, ""Name"":""hpt_atdumbfiremissile_turret_medium_v2"", ""BuyPrice"":2339667 },
{ ""id"":129022081, ""Name"":""hpt_atdumbfiremissile_fixed_medium_v2"", ""BuyPrice"":598047 },
{ ""id"":128049431, ""Name"":""hpt_beamlaser_fixed_huge"", ""BuyPrice"":2102631 },
{ ""id"":128049432, ""Name"":""hpt_beamlaser_gimbal_small"", ""BuyPrice"":65506 },
 ] }
";
                JournalOutfitting je = JournalEntry.CreateJournalEntry(t1) as JournalOutfitting;
                Check(je != null);
                Check(je.MarketID == 128666762);
                Check(je.YardInfo.StationName == "Jameson Memorial");
                Check(je.YardInfo.StarSystem == "Shinrarta Dezhra");
                Check(je.YardInfo.Items[0].FDName.Equals("hpt_atmulticannon_turret_large_v2"));
                Check(je.YardInfo.Items[0].BuyPrice == 4026594);
                Check(je.YardInfo.Items[7].FDName.Equals("hpt_beamlaser_gimbal_small"));
                Check(je.YardInfo.Items[7].BuyPrice == 65506);
            }

            {
                string t1 = @"{ ""timestamp"":""2026-07-01T08:41:05Z"", ""event"":""Outfitting"", ""MarketID"":128666762, ""StationName"":""Jameson Memorial"", ""StarSystem"":""Shinrarta Dezhra"", ""Horizons"":true, ""Items"":[
{ ""id"":129022085, ""Name"":""hpt_atmulticannon_turret_large_v2"", ""BuyPrice"":4026594 },
{ ""id"":129022080, ""Name"":""hpt_atmulticannon_fixed_medium_v2"", ""BuyPrice"":399331 },
{ ""id"":129022084, ""Name"":""hpt_atmulticannon_fixed_large_v2"", ""BuyPrice"":1193683 },
{ ""id"":129022082, ""Name"":""hpt_atdumbfiremissile_turret_large_v2"", ""BuyPrice"":4692462 },
{ ""id"":129022083, ""Name"":""hpt_atdumbfiremissile_turret_medium_v2"", ""BuyPrice"":2339667 },
{ ""id"":129022081, ""Name"":""hpt_atdumbfiremissile_fixed_medium_v2"", ""BuyPrice"":598047 },
{ ""id"":128049431, ""Name"":""hpt_beamlaser_fixed_huge"", ""BuyPrice"":2102631 },
{ ""id"":128049432, ""Name"":""hpt_beamlaser_gimbal_small"", ""BuyPrice"":65506 },
 ] }
";

                JournalOutfitting je = JournalEntry.CreateJournalEntry(t1) as JournalOutfitting;
                Check(je != null);
                Check(je.MarketID == 128666762);
                Check(je.YardInfo.StationName == "Jameson Memorial");
                Check(je.YardInfo.StarSystem == "Shinrarta Dezhra");
                Check(je.YardInfo.Items[0].FDName.Equals("hpt_atmulticannon_turret_large_v2"));
                Check(je.YardInfo.Items[0].BuyPrice == 4026594);
                Check(je.YardInfo.Items[7].FDName.Equals("hpt_beamlaser_gimbal_small"));
                Check(je.YardInfo.Items[7].BuyPrice == 65506);
            }


            {
                // buy

                string t0 = @"{ ""timestamp"":""2025-07-21T03:54:03Z"", ""event"":""ModuleBuy"", ""Slot"":""LargeHardpoint2""" +
                @", ""BuyItem"":""$hpt_pulselaserburst_fixed_large_name;"", ""BuyItem_Localised"":""Burst Laser""" +
                @", ""MarketID"":128666762, ""BuyPrice"":123201, ""Ship"":""python"", ""ShipID"":1 }";
                var je = JournalEntry.CreateJournalEntry(t0) as JournalModuleBuy;
                CheckThat(je).IsNotNull();
                CheckThat(je.SlotFD).Is(ShipSlots.Slot.LargeHardpoint2);
                CheckThat(je.BuyItemFD).Is("hpt_pulselaserburst_fixed_larGe");
                CheckThat(je.BuyItemLocalised).Is("Burst Laser");
                CheckThat(je.BuyPrice).Is(123201);
                CheckThat(je.StoredItemFD).IsNull();
                CheckThat(je.SellItemFD).IsNull();
                CheckThat(je.GetInfo()).Contains("Burst Laser");

                // buy store
                string t1 = @"{ ""timestamp"":""2025-07-21T03:49:34Z"", ""event"":""ModuleBuy"", ""Slot"":""PowerDistributor""" +
@", ""StoredItem"":""$int_powerdistributor_size6_class5_name;"", ""StoredItem_Localised"":""Power Distributor""" +
@", ""BuyItem"":""$int_powerdistributor_size7_class5_name;"", ""BuyItem_Localised"":""Power Distributor""" +
@", ""MarketID"":128666762, ""BuyPrice"":8539765, ""Ship"":""python"", ""ShipID"":1 }";

                je = JournalEntry.CreateJournalEntry(t1) as JournalModuleBuy;
                CheckThat(je).IsNotNull();
                CheckThat(je.SlotFD).Is(ShipSlots.Slot.PowerDistributor);
                CheckThat(je.BuyItemFD).Is("int_powerdistributor_size7_class5");
                CheckThat(je.StoredItemFD).Is("int_powerdistributor_size6_class5");
                CheckThat(je.StoredItemLocalised).Is("Power Distributor");
                CheckThat(je.SellItemFD).IsNull();

                // buy sell
                string t2 = @"{ ""timestamp"":""2025-07-21T03:54:51Z"", ""event"":""ModuleBuy"", ""Slot"":""LargeHardpoint2""" +
@", ""SellItem"":""$hpt_pulselaserburst_fixed_large_name;"", ""SellItem_Localised"":""Burst Laser""" +
@", ""SellPrice"":123201, ""BuyItem"":""$hpt_pulselaserburst_gimbal_large_name;""" +
@", ""BuyItem_Localised"":""Burst Laser"", ""MarketID"":128666762, ""BuyPrice"":247104" +
@", ""Ship"":""python"", ""ShipID"":1 }";

                je = JournalEntry.CreateJournalEntry(t2) as JournalModuleBuy;
                CheckThat(je).IsNotNull();
                CheckThat(je.SlotFD).Is(ShipSlots.Slot.LargeHardpoint2);
                CheckThat(je.SellItemFD).Is("hpt_pulselaserburst_fixed_large");
                CheckThat(je.SellItemLocalised).Is("Burst Laser");
                CheckThat(je.SellPrice).Is(123201);
                CheckThat(je.BuyItemFD).Is("hpt_pulselaserburst_gimbal_large");
                CheckThat(je.BuyItemLocalised).Is("Burst Laser");
                CheckThat(je.BuyPrice).Is(247104);
                CheckThat(je.StoredItemFD).IsNull();
                CheckThat(je.MarketID).Is(128666762UL);
            }
            {
                string t1 =@"{ ""timestamp"":""2025-12-15T10:12:40Z"", ""event"":""ModuleBuyAndStore"", ""BuyItem"":""$int_corrosionproofcargorack_size4_class1_name;""" +
@", ""BuyItem_Localised"":""Anti-Corrosion Cargo Rack"", ""MarketID"":128666762" +
@", ""BuyPrice"":82774, ""Ship"":""panthermkii"", ""ShipID"":64 }";
                var je = JournalEntry.CreateJournalEntry(t1) as JournalModuleBuyAndStore;
                Check(je != null);
                CheckThat(je.BuyItemFD).Is("int_corrosionproofcargorack_size4_class1");
                CheckThat(je.BuyItemLocalised).Is("Anti-Corrosion Cargo Rack");
                CheckThat(je.MarketID).Is(128666762UL);
                CheckThat(je.BuyPrice).Is(82774);
                CheckThat(je.ShipFD).Is("panthermkii");
                CheckThat(je.Ship).Is("Panther Clipper Mk II");
                CheckThat(je.ShipId).Is(new ShipID(64));
            }
            {
                string t1 = @"{ ""timestamp"":""2024-05-20T07:51:59Z"", ""event"":""ModuleSell"", ""MarketID"":128666762" +
@", ""Slot"":""MediumHardpoint1"", ""SellItem"":""$hpt_pulselaser_fixed_small_name;""" +
@", ""SellItem_Localised"":""Pulse Laser"", ""SellPrice"":1930, ""Ship"":""empire_trader""" +
@", ""ShipID"":69 }";
                var je = JournalEntry.CreateJournalEntry(t1) as JournalModuleSell;
                Check(je != null);
                CheckThat(je.SlotFD).Is(ShipSlots.Slot.MediumHardpoint1);
                CheckThat(je.SellItemFD).Is("hpt_pulselaser_fixed_small");
                CheckThat(je.SellItemLocalised).Is("Pulse Laser");
                CheckThat(je.SellPrice).Is(1930);
                CheckThat(je.ShipFD).Is("empire_trader");
                CheckThat(je.Ship).Is("Imperial Clipper");
                CheckThat(je.ShipId).Is(new ShipID(69));
            }
            {
                string t1 = @"{ ""timestamp"":""2024-05-25T19:56:16Z"", ""event"":""ModuleSellRemote"", ""StorageSlot"":196" +
@", ""SellItem"":""$int_dronecontrol_collection_size3_class4_name;"", ""SellItem_Localised"":""Collector""" +
@", ""ServerId"":128671237, ""SellPrice"":35802, ""Ship"":""empire_trader"", ""ShipID"":35 }";
                var je = JournalEntry.CreateJournalEntry(t1) as JournalModuleSellRemote;
                Check(je != null);
                CheckThat(je.SellItemFD).Is("int_dronecontrol_collection_size3_class4");
                CheckThat(je.SellItemLocalised).Is("Collector");
                CheckThat(je.SellPrice).Is(35802);
                CheckThat(je.ShipFD).Is("empire_trader");
                CheckThat(je.Ship).Is("Imperial Clipper");
                CheckThat(je.ShipId).Is(new ShipID(35));
                CheckThat(je.SlotNumber).Is(196);
            }
            {
                string t1 =@"{ ""timestamp"":""2024-11-04T20:54:41Z"", ""event"":""ModuleRetrieve"", ""MarketID"":3708879616" +
@", ""Slot"":""FrameShiftDrive"", ""RetrievedItem"":""$int_hyperdrive_size6_class5_name;""" +
@", ""RetrievedItem_Localised"":""FSD"", ""Ship"":""type9"", ""ShipID"":69, ""Hot"":false" +
@", ""EngineerModifications"":""FSD_LongRange"", ""Level"":5, ""Quality"":1.000000" +
@", ""SwapOutItem"":""$int_hyperdrive_size6_class5_name;"", ""SwapOutItem_Localised"":""FSD"" }";
                var je = JournalEntry.CreateJournalEntry(t1) as JournalModuleRetrieve;
                Check(je != null);
                CheckThat(je.SlotFD).Is(ShipSlots.Slot.FrameShiftDrive);
                CheckThat(je.RetrievedItemFD).Is("int_hyperdrive_size6_class5");
                CheckThat(je.RetrievedItemLocalised).Is("FSD");
                CheckThat(je.ShipFD).Is("type9");
                CheckThat(je.Ship).Is("Type-9 Heavy");
                CheckThat(je.ShipId).Is(new ShipID(69));
                CheckThat(je.Hot).Is(false);
                CheckThat(je.FDEngineerModifications).Is("FSD_LONGRANGE");
                CheckThat(je.EngineerModifications).Is("Increased FSD Range");
                CheckThat(je.Quality).Is(1.0);
                CheckThat(je.SwapOutItemFD).Is("int_hyperdrive_size6_class5");
                CheckThat(je.SwapOutItemLocalised).Is("FSD");

                string t2 = @"{ ""timestamp"":""2024-11-04T19:20:06Z"", ""event"":""ModuleRetrieve"", ""MarketID"":3708879616" +
@", ""Slot"":""Slot07_Size3"", ""RetrievedItem"":""$int_dronecontrol_collection_size3_class5_name;""" +
@", ""RetrievedItem_Localised"":""Collector"", ""Ship"":""krait_mkii"", ""ShipID"":63" +
@", ""Hot"":false, ""EngineerModifications"":""Misc_LightWeight"", ""Level"":4, ""Quality"":1.000000 }";

                je = JournalEntry.CreateJournalEntry(t2) as JournalModuleRetrieve;
                Check(je != null);
                CheckThat(je.SlotFD).Is(ShipSlots.Slot.Slot07_Size3);
                CheckThat(je.RetrievedItemFD).Is("int_dronecontrol_collection_size3_class5");
                CheckThat(je.RetrievedItemLocalised).Is("Collector");
                CheckThat(je.FDEngineerModifications).Is("Misc_LightWeight");
                CheckThat(je.EngineerModifications).Is("Lightweight");
                CheckThat(je.SwapOutItemFD).IsNull();
            }
            {
                string t1 =@"{ ""timestamp"":""2024-04-22T18:25:04Z"", ""event"":""ModuleStore"", ""MarketID"":3702018304" +
@", ""Slot"":""Slot03_Size3"", ""StoredItem"":""$int_dronecontrol_prospector_size3_class5_name;""" +
@", ""StoredItem_Localised"":""Prospector"", ""Ship"":""diamondback"", ""ShipID"":41" +
@", ""Hot"":false, ""EngineerModifications"":""Misc_LightWeight"", ""Level"":1, ""Quality"":1.000000 }";
                var je = JournalEntry.CreateJournalEntry(t1) as JournalModuleStore;
                Check(je != null);
                CheckThat(je.SlotFD).Is(ShipSlots.Slot.Slot03_Size3);
                CheckThat(je.StoredItemFD).Is("int_dronecontrol_prospector_size3_class5");
                CheckThat(je.StoredItemLocalised).Is("Prospector");
                CheckThat(je.FDEngineerModifications).Is("Misc_LightWeight");
                CheckThat(je.EngineerModifications).Is("Lightweight");
            }
            {
                string t1 =@"{ ""timestamp"":""2022-10-14T10:13:05Z"", ""event"":""ModuleSwap"", ""MarketID"":128666762" +
@", ""FromSlot"":""TinyHardpoint2"", ""ToSlot"":""TinyHardpoint1"", ""FromItem"":""$hpt_antiunknownshutdown_tiny_name;""" +
@", ""FromItem_Localised"":""Field Neutraliser"", ""ToItem"":""$hpt_xenoscanner_basic_tiny_name;""" +
@", ""ToItem_Localised"":""Xeno Scanner"", ""Ship"":""federation_corvette"", ""ShipID"":35 }";
                var je = JournalEntry.CreateJournalEntry(t1) as JournalModuleSwap;
                Check(je != null);
                CheckThat(je.FromSlotFD).Is(ShipSlots.Slot.TinyHardpoint2);
                CheckThat(je.ToSlotFD).Is(ShipSlots.Slot.TinyHardpoint1);
                CheckThat(je.FromItemFD).Is("hpt_antiunknownshutdown_tiny");
                CheckThat(je.ToItemFD).Is("hpt_xenoscanner_basic_tiny");
            }
            {
                string t1 = @"{ ""timestamp"":""2026 - 06 - 27T14: 57:55Z"",""event"":""ModuleInfo"",""Modules"":" + 
                    @"[{""Slot"":""MainEngines"",""Item"":""int_engine_size7_class1"",""Power"":6.08,""Priority"":0},{""Slot"":""Slot02_Size6"",""Item"":""int_shieldgenerator_size6_class1"",""Power"":1.86,""Priority"":2},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_overcharge_size8_class1"",""Power"":0.7,""Priority"":2},{ ""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""Power"":0.6,""Priority"":2},{ ""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size7_class1"",""Power"":0.59,""Priority"":2},{ ""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size5_class1"",""Power"":0.57,""Priority"":2},{ ""Slot"":""Radar"",""Item"":""int_sensors_size8_class1"",""Power"":0.55,""Priority"":2},{ ""Slot"":""Slot14_Size1"",""Item"":""int_dockingcomputer_advanced"",""Power"":0.45,""Priority"":2},{ ""Slot"":""MediumHardpoint1"",""Item"":""hpt_pulselaser_fixed_small"",""Power"":0.39,""Priority"":2},{ ""Slot"":""MediumHardpoint2"",""Item"":""hpt_pulselaser_fixed_small"",""Power"":0.39,""Priority"":2},{ ""Slot"":""Slot13_Size1"",""Item"":""int_supercruiseassist"",""Power"":0.3,""Priority"":2},{ ""Slot"":""Slot04_Size5"",""Item"":""int_fighterbaymk2_size5_class1_free"",""Power"":0.25,""Priority"":0},{ ""Slot"":""ShipCockpit"",""Item"":""explorer_nx_cockpit"",""Power"":0.0},{ ""Slot"":""PowerPlant"",""Item"":""int_powerplant_size8_class1"",""Power"":0.0},{ ""Slot"":""Slot01_Size7"",""Item"":""int_cargorack_size6_class1"",""Power"":0.0},{ ""Slot"":""Slot03_Size6"",""Item"":""int_cargorack_size5_class1"",""Power"":0.0},{ ""Slot"":""PlanetaryApproachSuite"",""Item"":""int_planetapproachsuite_advanced"",""Power"":0.0},{ ""Slot"":""DataLinkScanner"",""Item"":""hpt_shipdatalinkscanner"",""Power"":0.0,""Priority"":0},{ ""Slot"":""CodexScanner"",""Item"":""int_codexscanner"",""Power"":0.0},{ ""Slot"":""DiscoveryScanner"",""Item"":""int_stellarbodydiscoveryscanner_standard"",""Power"":0.0},{ ""Slot"":""ColonisationSuite"",""Item"":""int_colonisation"",""Power"":0.0}]}";

                var je = JournalEntry.CreateJournalEntry(t1) as JournalModuleInfo;
                Check(je != null);
                CheckThat(je.ShipModules[0].SlotFD).Is(ShipSlots.Slot.MainEngines);
                CheckThat(je.ShipModules[0].ItemFD).Is("int_engine_size7_class1");
                CheckThat(je.ShipModules[0].Power).Is(6.08);
                CheckThat(je.ShipModules[0].Priority).Is(0);
            }
            {
                string t1 = @"{ ""timestamp"":""2022-12-04T19:58:01Z"", ""event"":""StoredModules"", ""MarketID"":3224982272" +
@", ""StationName"":""Medupe City"", ""StarSystem"":""Cubeo"", ""Items"":[ { ""Name"":""$int_shieldgenerator_size8_class5_strong_name;""" +
@", ""Name_Localised"":""Prismatic Shield"", ""StorageSlot"":131, ""StarSystem"":""HR 8444""" +
@", ""MarketID"":3225377792, ""TransferCost"":1937529, ""TransferTime"":1864, ""BuyPrice"":202115326, ""InTransit"":true, ""EngineerModifications"":""ShieldGenerator_Reinforced"" " +
@", ""Hot"":false }, { ""Name"":""$int_shieldgenerator_size8_class5_strong_name;""" +
@", ""Name_Localised"":""Prismatic Shield"", ""StorageSlot"":127, ""StarSystem"":""HR 8444""" +
@", ""MarketID"":3225377792, ""TransferCost"":1937529, ""TransferTime"":1864, ""BuyPrice"":202115326" +
@", ""Hot"":false }, { ""Name"":""$int_shieldgenerator_size8_class5_strong_name;""" +
@", ""Name_Localised"":""Prismatic Shield"", ""StorageSlot"":126, ""StarSystem"":""HR 8444""" +
@", ""MarketID"":3225377792, ""TransferCost"":1937529, ""TransferTime"":1864, ""BuyPrice"":202115326" +
@", ""Hot"":false } ]}";

                var je = JournalEntry.CreateJournalEntry(t1) as JournalStoredModules;
                Check(je != null);
                CheckThat(je.StationName).Is("Medupe City");
                CheckThat(je.StarSystem).Is("Cubeo");
                CheckThat(je.ModuleItems[0].NameFD).Is("int_shieldgenerator_size8_class5_strong");
                CheckThat(je.ModuleItems[0].Name).Is("Prismatic Shield Generator Class 8 Rating A");
                CheckThat(je.ModuleItems[0].Name_Localised).Is("Prismatic Shield");
                CheckThat(je.ModuleItems[0].StarSystem).Is("HR 8444");
                CheckThat(je.ModuleItems[0].TransferCost).Is(1937529);
                CheckThat(je.ModuleItems[0].TransferTime).Is(1864);
                CheckThat(je.ModuleItems[0].BuyPrice).Is(202115326);
                CheckThat(je.ModuleItems[0].InTransit).Is(true);
                CheckThat(je.ModuleItems[0].EngineerModifications).Is("ShieldGenerator_Reinforced");
            }
            {
                string t1 = @"{ ""timestamp"":""2024-11-23T18:14:16Z"", ""event"":""MassModuleStore"", ""MarketID"":3710141696" +
@", ""Ship"":""diamondbackxl"", ""ShipID"":2, ""Items"":[ { ""Slot"":""TinyHardpoint1""" +
@", ""Name"":""$hpt_electroniccountermeasure_tiny_name;"", ""Name_Localised"":""ECM""" +
@", ""Hot"":false }, { ""Slot"":""TinyHardpoint2"", ""Name"":""$hpt_plasmapointdefence_turret_tiny_name;""" +
@", ""Name_Localised"":""Point Defence Turret"", ""Hot"":false, ""EngineerModifications"":""Misc_Shielded""" +
@", ""Level"":2, ""Quality"":1.000000 }, { ""Slot"":""TinyHardpoint3"", ""Name"":""$hpt_heatsinklauncher_turret_tiny_name;""" +
@", ""Name_Localised"":""Heatsink"", ""Hot"":false, ""EngineerModifications"":""Misc_HeatSinkCapacity""" +
@", ""Level"":1, ""Quality"":1.000000 }, { ""Slot"":""Slot01_Size4"", ""Name"":""$int_guardianfsdbooster_size4_name;""" +
@", ""Name_Localised"":""Guardian FSD Booster"", ""Hot"":false }, { ""Slot"":""Slot02_Size4""" +
@", ""Name"":""$int_buggybay_size4_class2_name;"", ""Name_Localised"":""Planetary Vehicle Hangar""" +
@", ""Hot"":false }, { ""Slot"":""Slot04_Size3"", ""Name"":""$int_fuelscoop_size3_class5_name;""" +
@", ""Name_Localised"":""Fuel Scoop"", ""Hot"":false }, { ""Slot"":""Slot07_Size1""" +
@", ""Name"":""$int_supercruiseassist_name;"", ""Name_Localised"":""Supercruise Assist""" +
@", ""Hot"":false } ] }";
                var je = JournalEntry.CreateJournalEntry(t1) as JournalMassModuleStore;
                Check(je != null);
                CheckThat(je.ShipFD).Is("diamondbackxl");
                CheckThat(je.Ship).Is("Diamondback Explorer");
                CheckThat(je.ShipId).Is(new ShipID(2));
                CheckThat(je.ModuleItems[0].SlotFD).Is(ShipSlots.Slot.TinyHardpoint1);
                CheckThat(je.ModuleItems[0].NameFD).Is("hpt_electroniccountermeasure_tiny");
                CheckThat(je.ModuleItems[0].Name).Is("Electronic Countermeasure Tiny");
                CheckThat(je.ModuleItems[0].Name_Localised).Is("ECM");
                CheckThat(je.ModuleItems[0].EngineerModifications).IsNull();
                CheckThat(je.ModuleItems[1].SlotFD).Is(ShipSlots.Slot.TinyHardpoint2);
                CheckThat(je.ModuleItems[1].NameFD).Is("hpt_plasmapointdefence_turret_tiny");
                CheckThat(je.ModuleItems[1].EngineerModifications).Is("Misc_Shielded");

            }
            {
                string t1 = @"{ ""timestamp"":""2023-07-06T18:08:05Z"", ""event"":""FetchRemoteModule"", ""StorageSlot"":175" +
@", ""StoredItem"":""$int_multidronecontrol_xeno_size3_class4_name;"", ""StoredItem_Localised"":""Xeno Multi-Limpet Controller""" +
@", ""ServerId"":129001928, ""TransferCost"":480, ""TransferTime"":1222, ""Ship"":""krait_mkii""" +
@", ""ShipID"":6 }";
                var je = JournalEntry.CreateJournalEntry(t1) as JournalFetchRemoteModule;
                Check(je != null);
                CheckThat(je.StoredItemFD).Is("int_multidronecontrol_xeno_size3_class4");
                CheckThat(je.StoredItemLocalised).Is("Xeno Multi-Limpet Controller");
            }
        }
    }
}
