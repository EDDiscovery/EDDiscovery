using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;

using static BaseUtils.UnitTests.CheckerHelpers;

namespace UnitTest
{
    public static class UnitTestShips
    {
        [BaseUtils.UnitTests.Test]
        public static void TestShips()
        {
            CheckSection("Ships");

            {
                CheckThat(ItemData.GetShipFDID(new VehicleFDName("SIDEWinder"))).IsNotNull();
                CheckThat(ItemData.GetShipFDID(new VehicleFDName("2SIDEWinder"))).IsNull();
                CheckThat(ItemData.GetShipFDID(new VehicleFDName("2SIDEWinder"))).IsNull();
                var Fighter1 = new VehicleFDName("\"gdn_hybrid_fighter_v3\"");
                CheckThat(Fighter1.Type).Is(VehicleFDName.VehicleType.Fighter);
                var srv1 = new VehicleFDName("testBuggy");
                CheckThat(srv1.Type).Is(VehicleFDName.VehicleType.SRV);
                var srv2 = new VehicleFDName("combat_multicrew_srv_01");
                CheckThat(srv2.Type).Is(VehicleFDName.VehicleType.SRV);
                var lander1 = new VehicleFDName("lander01");
                CheckThat(lander1.Type).Is(VehicleFDName.VehicleType.Lander);
            }
            {
                string t1 = @"{ ""timestamp"":""2025-07-25T15:07:59Z"", ""event"":""Loadout"", ""Ship"":""panthermkii"", ""ShipID"":59, ""ShipName"":"""", ""ShipIdent"":""RO-25P"", ""HullHealth"":1.000000, ""UnladenMass"":1773.599976, ""CargoCapacity"":1040, ""MaxJumpRange"":20.758162, ""FuelCapacity"":{ ""Main"":128.000000, ""Reserve"":1.110000 }, ""Rebuy"":0, ""Modules"":"
                        + @"[ { ""Slot"":""LargeHardpoint1"", ""Item"":""hpt_pulselaser_turret_large"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""LargeHardpoint2"", ""Item"":""hpt_pulselaser_turret_large"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""MediumHardpoint1"", ""Item"":""hpt_multicannon_turret_medium"", ""On"":true, ""Priority"":2, ""AmmoInClip"":90, ""AmmoInHopper"":2100, ""Health"":1.000000 }, { ""Slot"":""MediumHardpoint2"", ""Item"":""hpt_multicannon_turret_medium"", ""On"":true, ""Priority"":2, ""AmmoInClip"":90, ""AmmoInHopper"":2100, ""Health"":1.000000 }, { ""Slot"":""MediumHardpoint3"", ""Item"":""hpt_multicannon_turret_medium"", ""On"":true, ""Priority"":2, ""AmmoInClip"":90, ""AmmoInHopper"":2100, ""Health"":1.000000 }, { ""Slot"":""MediumHardpoint4"", ""Item"":""hpt_multicannon_turret_medium"", ""On"":true, ""Priority"":2, ""AmmoInClip"":90, ""AmmoInHopper"":2100, ""Health"":1.000000 }, { ""Slot"":""SmallHardpoint1"", ""Item"":""hpt_pulselaser_turret_small"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""SmallHardpoint2"", ""Item"":""hpt_pulselaser_turret_small"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""SmallHardpoint3"", ""Item"":""hpt_pulselaser_turret_small"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""SmallHardpoint4"", ""Item"":""hpt_pulselaser_turret_small"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""TinyHardpoint1"", ""Item"":""hpt_shieldbooster_size0_class5"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""TinyHardpoint2"", ""Item"":""hpt_shieldbooster_size0_class5"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""TinyHardpoint3"", ""Item"":""hpt_shieldbooster_size0_class5"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""TinyHardpoint4"", ""Item"":""hpt_electroniccountermeasure_tiny"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""TinyHardpoint5"", ""Item"":""hpt_plasmapointdefence_turret_tiny"", ""On"":true, ""Priority"":2, ""AmmoInClip"":12, ""AmmoInHopper"":10000, ""Health"":1.000000 }, { ""Slot"":""TinyHardpoint6"", ""Item"":""hpt_heatsinklauncher_turret_tiny"", ""On"":true, ""Priority"":2, ""AmmoInClip"":1, ""AmmoInHopper"":2, ""Health"":1.000000 }, { ""Slot"":""Armour"", ""Item"":""panthermkii_armour_grade1"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""PaintJob"", ""Item"":""paintjob_panthermkii_01_02"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""PowerPlant"", ""Item"":""int_powerplant_size8_class5"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""MainEngines"", ""Item"":""int_engine_size8_class5"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""FrameShiftDrive"", ""Item"":""int_hyperdrive_overcharge_size7_class5"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""LifeSupport"", ""Item"":""int_lifesupport_size5_class5"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""PowerDistributor"", ""Item"":""int_powerdistributor_size7_class5"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""Radar"", ""Item"":""int_sensors_size5_class5"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""FuelTank"", ""Item"":""int_fueltank_size7_class3"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""Decal1"", ""Item"":""decal_pantherownersclub_01"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""Decal2"", ""Item"":""decal_pantherownersclub_01"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""Decal3"", ""Item"":""decal_pantherownersclub_01"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""Cargo01"", ""Item"":""int_largecargorack_size8_class1"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""Slot01_Size8"", ""Item"":""int_cargorack_size8_class1"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""Cargo02"", ""Item"":""int_largecargorack_size7_class1"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""Slot02_Size7"", ""Item"":""int_shieldgenerator_size7_class5"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""Slot03_Size6"", ""Item"":""int_cargorack_size6_class1"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""Slot04_Size6"", ""Item"":""int_cargorack_size6_class1"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""Slot05_Size6"", ""Item"":""int_fuelscoop_size6_class5"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""Slot06_Size5"", ""Item"":""int_cargorack_size5_class1"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""Slot07_Size5"", ""Item"":""int_cargorack_size5_class1"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""Slot08_Size4"", ""Item"":""int_cargorack_size4_class1"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""Slot09_Size2"", ""Item"":""int_dockingcomputer_advanced"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""Slot10_Size1"", ""Item"":""int_supercruiseassist"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""PlanetaryApproachSuite"", ""Item"":""int_planetapproachsuite_advanced"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""VesselVoice"", ""Item"":""voicepack_verity"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""ShipKitSpoiler"", ""Item"":""panthermkii_shipkita_spoiler2"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""ShipKitWings"", ""Item"":""panthermkii_shipkita_wings2"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""ShipKitBumper"", ""Item"":""panthermkii_shipkita_bumper1"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""ShipKitTail"", ""Item"":""panthermkii_shipkita_tail1"", ""On"":true, ""Priority"":1, ""Health"":1.000000 }, { ""Slot"":""CargoHatch"", ""Item"":""modularcargobaydoor"", ""On"":true, ""Priority"":2, ""Health"":1.000000 }, { ""Slot"":""ShipCockpit"", ""Item"":""panthermkii_cockpit"", ""On"":true, ""Priority"":1, ""Health"":1.000000 } ] }";
                JournalLoadout je = JournalEntry.CreateJournalEntry(t1) as JournalLoadout;
                Check(je != null);
                Check(je.ShipFD == new VehicleFDName("Panthermkii"));
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
                string t1 =@"{ ""timestamp"":""2025-09-07T14:46:02Z"", ""event"":""StoredShips"", ""StationName"":""Thome Installation"", ""MarketID"":3224860416" +
@", ""StarSystem"":""Djenni"", ""ShipsHere"":[  ], ""ShipsRemote"":[ { ""ShipID"":3, ""ShipType"":""Empire_Courier"", ""ShipType_Localised"":""Imperial Courier""" +
@", ""Name"":""SIMAKAZE"", ""StarSystem"":""Dounthiassi"", ""ShipMarketID"":3712825856, ""TransferPrice"":86526, ""TransferTime"":1525" +
@", ""Value"":10192447, ""Hot"":false }, { ""ShipID"":23, ""ShipType"":""Python_NX"", ""ShipType_Localised"":""Python Mk II"", ""Name"":""""" +
@", ""StarSystem"":""Cubeo"", ""ShipMarketID"":3930424065, ""TransferPrice"":491516, ""TransferTime"":1547, ""Value"":57459910, ""Hot"":false }" +
@", { ""ShipID"":1, ""ShipType"":""Python"", ""Name"":""IRAM IMPERSKA"", ""StarSystem"":""Dounthiassi"", ""ShipMarketID"":3712825856" +
@", ""TransferPrice"":596149, ""TransferTime"":1525, ""Value"":70925814, ""Hot"":false }, { ""ShipID"":16, ""ShipType"":""Corsair""" +
@", ""Name"":""TAKAO"", ""StarSystem"":""Siris"", ""ShipMarketID"":18446744073709551615, ""TransferPrice"":106171, ""TransferTime"":416" +
@", ""Value"":104973453, ""Hot"":false }, { ""ShipID"":26, ""ShipType"":""Anaconda"", ""Name"":""VIRGE GARO"", ""StarSystem"":""Dounthiassi""" +
@", ""ShipMarketID"":3712825856, ""TransferPrice"":1523244, ""TransferTime"":1525, ""Value"":181410644, ""Hot"":false }, { ""ShipID"":4" +
@", ""ShipType"":""Cutter"", ""ShipType_Localised"":""Imperial Cutter"", ""Name"":""ADMIRAL NAKHIMOV"", ""StarSystem"":""Dounthiassi""" +
@", ""ShipMarketID"":3712825856, ""TransferPrice"":2839204, ""TransferTime"":1525, ""Value"":338237721, ""Hot"":false } ] }";
                var je = JournalEntry.CreateJournalEntry(t1) as JournalStoredShips;
                Check(je != null);
            }
#if false
            {
                string t1 =
                var je = JournalEntry.CreateJournalEntry(t1) as JournalCarrierCrewServices;
                Check(je != null);
                Check(je.CrewRole == CarrierDefinitions.ServiceType.PioneerSupplies);
            }
            {
                string t1 =
                var je = JournalEntry.CreateJournalEntry(t1) as JournalCarrierCrewServices;
                Check(je != null);
                Check(je.CrewRole == CarrierDefinitions.ServiceType.PioneerSupplies);
            }
#endif
        }
    }
}
