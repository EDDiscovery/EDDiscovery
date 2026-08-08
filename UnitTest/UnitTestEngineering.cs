/*
 * Copyright 2026-2026 EDDiscovery development team
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
 */

using QuickJSON;
using BaseUtils;
using System;
using System.Linq;
using EliteDangerousCore;
using static BaseUtils.UnitTests.CheckerHelpers;

namespace UnitTest
{
    public static class UnitTestEngineeringRecipes
    {
        [BaseUtils.UnitTests.Test(50)]
        public static void TestEngineeringRecipes()
        {
            CheckSection("Engineering Recipes");

            //{
            //    // this one has GuardianModuleResistance - check its ignored nicely
            //    string t = @"{""timestamp"":""2024-06-23T20:05:48Z"",""event"":""Loadout"",""Ship"":""typex"",""ShipID"":278,""ShipName"":"""",""ShipIdent"":""SK-09T"",""HullHealth"":1.0,""UnladenMass"":724.900024,""CargoCapacity"":16,""MaxJumpRange"":16.874365,""FuelCapacity"":{""Main"":16.0,""Reserve"":0.77},""Rebuy"":0,""Modules"":[{""Slot"":""LargeHardpoint1"",""Item"":""hpt_atmulticannon_gimbal_large"",""On"":true,""Priority"":2,""AmmoInClip"":100,""AmmoInHopper"":2100,""Health"":1.0},{""Slot"":""LargeHardpoint2"",""Item"":""hpt_atmulticannon_gimbal_large"",""On"":true,""Priority"":2,""AmmoInClip"":100,""AmmoInHopper"":2100,""Health"":1.0},{""Slot"":""MediumHardpoint1"",""Item"":""hpt_atventdisruptorpylon_fixed_medium"",""On"":true,""Priority"":2,""AmmoInClip"":1,""AmmoInHopper"":64,""Health"":1.0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_beamlaser_gimbal_small"",""On"":true,""Priority"":2,""Health"":1.0},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_guardian_gausscannon_fixed_small"",""On"":true,""Priority"":2,""AmmoInClip"":1,""AmmoInHopper"":80,""Health"":1.0,""Engineering"":{""Engineer"":""Ram Tah"",""EngineerID"":300110,""BlueprintID"":129030458,""BlueprintName"":""GuardianWeapon_Sturdy"",""Level"":1,""Quality"":0.0,""Modifiers"":[{""Label"":""DamagePerSecond"",""Value"":21.204821,""OriginalValue"":26.506025,""LessIsGood"":0},{""Label"":""Damage"",""Value"":17.6,""OriginalValue"":22.0,""LessIsGood"":0},{""Label"":""GuardianModuleResistance"",""ValueStr"":""$INT_PANEL_module_active;"",""ValueStr_Localised"":""Active""}]}},{""Slot"":""SmallHardpoint3"",""Item"":""hpt_guardian_gausscannon_fixed_small"",""On"":true,""Priority"":2,""AmmoInClip"":1,""AmmoInHopper"":80,""Health"":1.0,""Engineering"":{""Engineer"":""Ram Tah"",""EngineerID"":300110,""BlueprintID"":129030458,""BlueprintName"":""GuardianWeapon_Sturdy"",""Level"":1,""Quality"":0.0,""Modifiers"":[{""Label"":""DamagePerSecond"",""Value"":21.204821,""OriginalValue"":26.506025,""LessIsGood"":0},{""Label"":""Damage"",""Value"":17.6,""OriginalValue"":22.0,""LessIsGood"":0},{""Label"":""GuardianModuleResistance"",""ValueStr"":""$INT_PANEL_module_active;"",""ValueStr_Localised"":""Active""}]}},{""Slot"":""TinyHardpoint1"",""Item"":""hpt_heatsinklauncher_turret_tiny"",""On"":true,""Priority"":2,""AmmoInClip"":1,""AmmoInHopper"":2,""Health"":1.0},{""Slot"":""TinyHardpoint2"",""Item"":""hpt_causticsinklauncher_turret_tiny"",""On"":true,""Priority"":2,""AmmoInClip"":1,""AmmoInHopper"":5,""Health"":1.0},{""Slot"":""TinyHardpoint3"",""Item"":""hpt_xenoscannermk2_basic_tiny"",""On"":true,""Priority"":2,""Health"":1.0},{""Slot"":""TinyHardpoint4"",""Item"":""hpt_antiunknownshutdown_tiny_v2"",""On"":true,""Priority"":2,""Health"":1.0},{""Slot"":""PaintJob"",""Item"":""paintjob_typex_iridescentblack_04"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""Armour"",""Item"":""typex_armour_grade3"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size6_class5"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""MainEngines"",""Item"":""int_engine_size6_class5"",""On"":true,""Priority"":2,""Health"":1.0,""Engineering"":{""Engineer"":""Professor Palin"",""EngineerID"":300220,""BlueprintID"":128673659,""BlueprintName"":""Engine_Dirty"",""Level"":5,""Quality"":1.0,""ExperimentalEffect"":""special_engine_overloaded"",""ExperimentalEffect_Localised"":""Drag Drives"",""Modifiers"":[{""Label"":""Integrity"",""Value"":105.400002,""OriginalValue"":124.0,""LessIsGood"":0},{""Label"":""PowerDraw"",""Value"":8.4672,""OriginalValue"":7.56,""LessIsGood"":1},{""Label"":""EngineOptimalMass"",""Value"":1260.0,""OriginalValue"":1440.0,""LessIsGood"":0},{""Label"":""EngineOptPerformance"",""Value"":145.599991,""OriginalValue"":100.0,""LessIsGood"":0},{""Label"":""EngineHeatRate"",""Value"":2.288,""OriginalValue"":1.3,""LessIsGood"":1}]}},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size5_class5"",""On"":true,""Priority"":2,""Health"":1.0},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size5_class2"",""On"":true,""Priority"":2,""Health"":1.0},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size6_class5"",""On"":true,""Priority"":2,""Health"":1.0},{""Slot"":""Radar"",""Item"":""int_sensors_size4_class2"",""On"":true,""Priority"":2,""Health"":1.0},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size4_class3"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""Slot01_Size6"",""Item"":""int_shieldgenerator_size6_class3_fast"",""On"":true,""Priority"":2,""Health"":1.0},{""Slot"":""Slot02_Size5"",""Item"":""int_hullreinforcement_size5_class2"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""Slot03_Size4"",""Item"":""int_cargorack_size4_class1"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""Slot04_Size2"",""Item"":""int_repairer_size2_class5"",""On"":true,""Priority"":2,""Health"":1.0},{""Slot"":""Slot05_Size2"",""Item"":""int_dronecontrol_repair_size1_class5"",""On"":true,""Priority"":2,""Health"":1.0},{""Slot"":""Slot06_Size1"",""Item"":""int_dronecontrol_unkvesselresearch"",""On"":true,""Priority"":2,""Health"":1.0},{""Slot"":""Military01"",""Item"":""int_modulereinforcement_size4_class2"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""Military02"",""Item"":""int_hullreinforcement_size4_class2"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""Military03"",""Item"":""int_hullreinforcement_size4_class2"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""PlanetaryApproachSuite"",""Item"":""int_planetapproachsuite_advanced"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""ShipKitSpoiler"",""Item"":""typex_shipkit1_spoiler4"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""ShipKitWings"",""Item"":""typex_shipkit1_wings1"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""ShipKitBumper"",""Item"":""typex_shipkit1_bumper3"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""VesselVoice"",""Item"":""voicepack_verity"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""ShipCockpit"",""Item"":""typex_cockpit"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":2,""Health"":1.0}]}";
            //    GetModule(t, ShipSlots.Slot.SmallHardpoint2, true);
            //}
            {
                // this one has GuardianModuleResistance - check its ignored nicely
                string t = @"{""timestamp"":""2024-06-23T20:05:48Z"",""event"":""Loadout"",""Ship"":""typex"",""ShipID"":278,""ShipName"":"""",""ShipIdent"":""SK-09T"",""HullHealth"":1.0,""UnladenMass"":724.900024,""CargoCapacity"":16,""MaxJumpRange"":16.874365,""FuelCapacity"":{""Main"":16.0,""Reserve"":0.77},""Rebuy"":0,""Modules"":[{""Slot"":""LargeHardpoint1"",""Item"":""hpt_atmulticannon_gimbal_large"",""On"":true,""Priority"":2,""AmmoInClip"":100,""AmmoInHopper"":2100,""Health"":1.0},{""Slot"":""LargeHardpoint2"",""Item"":""hpt_atmulticannon_gimbal_large"",""On"":true,""Priority"":2,""AmmoInClip"":100,""AmmoInHopper"":2100,""Health"":1.0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_beamlaser_gimbal_small"",""On"":true,""Priority"":2,""Health"":1.0},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_atventdisruptorpylon_fixed_medium"",""On"":true,""Priority"":2,""AmmoInClip"":1,""AmmoInHopper"":80,""Health"":1.0,""Engineering"":{""Engineer"":""Ram Tah"",""EngineerID"":300110,""BlueprintID"":129030458,""BlueprintName"":""GuardianWeapon_Sturdy"",""Level"":1,""Quality"":0.0,""Modifiers"":[{""Label"":""DamagePerSecond"",""Value"":21.204821,""OriginalValue"":26.506025,""LessIsGood"":0},{""Label"":""Damage"",""Value"":17.6,""OriginalValue"":22.0,""LessIsGood"":0},{""Label"":""GuardianModuleResistance"",""ValueStr"":""$INT_PANEL_module_active;"",""ValueStr_Localised"":""Active""}]}},{""Slot"":""SmallHardpoint3"",""Item"":""hpt_guardian_gausscannon_fixed_small"",""On"":true,""Priority"":2,""AmmoInClip"":1,""AmmoInHopper"":80,""Health"":1.0,""Engineering"":{""Engineer"":""Ram Tah"",""EngineerID"":300110,""BlueprintID"":129030458,""BlueprintName"":""GuardianWeapon_Sturdy"",""Level"":1,""Quality"":0.0,""Modifiers"":[{""Label"":""DamagePerSecond"",""Value"":21.204821,""OriginalValue"":26.506025,""LessIsGood"":0},{""Label"":""Damage"",""Value"":17.6,""OriginalValue"":22.0,""LessIsGood"":0},{""Label"":""GuardianModuleResistance"",""ValueStr"":""$INT_PANEL_module_active;"",""ValueStr_Localised"":""Active""}]}},{""Slot"":""TinyHardpoint1"",""Item"":""hpt_heatsinklauncher_turret_tiny"",""On"":true,""Priority"":2,""AmmoInClip"":1,""AmmoInHopper"":2,""Health"":1.0},{""Slot"":""TinyHardpoint2"",""Item"":""hpt_causticsinklauncher_turret_tiny"",""On"":true,""Priority"":2,""AmmoInClip"":1,""AmmoInHopper"":5,""Health"":1.0},{""Slot"":""TinyHardpoint3"",""Item"":""hpt_xenoscannermk2_basic_tiny"",""On"":true,""Priority"":2,""Health"":1.0},{""Slot"":""TinyHardpoint4"",""Item"":""hpt_antiunknownshutdown_tiny_v2"",""On"":true,""Priority"":2,""Health"":1.0},{""Slot"":""PaintJob"",""Item"":""paintjob_typex_iridescentblack_04"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""Armour"",""Item"":""typex_armour_grade3"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size6_class5"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""MainEngines"",""Item"":""int_engine_size6_class5"",""On"":true,""Priority"":2,""Health"":1.0,""Engineering"":{""Engineer"":""Professor Palin"",""EngineerID"":300220,""BlueprintID"":128673659,""BlueprintName"":""Engine_Dirty"",""Level"":5,""Quality"":1.0,""ExperimentalEffect"":""special_engine_overloaded"",""ExperimentalEffect_Localised"":""Drag Drives"",""Modifiers"":[{""Label"":""Integrity"",""Value"":105.400002,""OriginalValue"":124.0,""LessIsGood"":0},{""Label"":""PowerDraw"",""Value"":8.4672,""OriginalValue"":7.56,""LessIsGood"":1},{""Label"":""EngineOptimalMass"",""Value"":1260.0,""OriginalValue"":1440.0,""LessIsGood"":0},{""Label"":""EngineOptPerformance"",""Value"":145.599991,""OriginalValue"":100.0,""LessIsGood"":0},{""Label"":""EngineHeatRate"",""Value"":2.288,""OriginalValue"":1.3,""LessIsGood"":1}]}},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size5_class5"",""On"":true,""Priority"":2,""Health"":1.0},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size5_class2"",""On"":true,""Priority"":2,""Health"":1.0},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size6_class5"",""On"":true,""Priority"":2,""Health"":1.0},{""Slot"":""Radar"",""Item"":""int_sensors_size4_class2"",""On"":true,""Priority"":2,""Health"":1.0},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size4_class3"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""Slot01_Size6"",""Item"":""int_shieldgenerator_size6_class3_fast"",""On"":true,""Priority"":2,""Health"":1.0},{""Slot"":""Slot02_Size5"",""Item"":""int_hullreinforcement_size5_class2"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""Slot03_Size4"",""Item"":""int_cargorack_size4_class1"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""Slot04_Size2"",""Item"":""int_repairer_size2_class5"",""On"":true,""Priority"":2,""Health"":1.0},{""Slot"":""Slot05_Size2"",""Item"":""int_dronecontrol_repair_size1_class5"",""On"":true,""Priority"":2,""Health"":1.0},{""Slot"":""Slot06_Size1"",""Item"":""int_dronecontrol_unkvesselresearch"",""On"":true,""Priority"":2,""Health"":1.0},{""Slot"":""Military01"",""Item"":""int_modulereinforcement_size4_class2"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""Military02"",""Item"":""int_hullreinforcement_size4_class2"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""Military03"",""Item"":""int_hullreinforcement_size4_class2"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""PlanetaryApproachSuite"",""Item"":""int_planetapproachsuite_advanced"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""ShipKitSpoiler"",""Item"":""typex_shipkit1_spoiler4"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""ShipKitWings"",""Item"":""typex_shipkit1_wings1"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""ShipKitBumper"",""Item"":""typex_shipkit1_bumper3"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""VesselVoice"",""Item"":""voicepack_verity"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""ShipCockpit"",""Item"":""typex_cockpit"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":2,""Health"":1.0}]}";
                System.Diagnostics.Debug.WriteLine(t);
                EngineerModule(t, ShipSlots.Slot.SmallHardpoint2, true);
            }

            {
                // slugshot error 19/7/24 due to EDSY not listing bstrof or bstsize
                string t = @"{""timestamp"":""2024-07-18T20:54:27Z"",""event"":""Loadout"",""Ship"":""python"",""ShipID"":153,""ShipName"":""Kajblood"",""ShipIdent"":""KJBLD"",""HullValue"":45842780,
""ModulesValue"":163348357,""HullHealth"":1.0,""UnladenMass"":671.450012,""CargoCapacity"":286,""MaxJumpRange"":24.341875,""FuelCapacity"":{""Main"":32.0,
""Reserve"":0.83},""Rebuy"":10459558,""Modules"":[{""Slot"":""LargeHardpoint1"",""Item"":""hpt_slugshot_fixed_large_range"",""On"":true,""Priority"":2,
""AmmoInClip"":4,""AmmoInHopper"":180,""Health"":1.0,""Value"":1365812,""Engineering"":{""Engineer"":""Zacariah Nemo"",""EngineerID"":300050,""BlueprintID"":128673437,
""BlueprintName"":""Weapon_DoubleShot"",""Level"":3,""Quality"":1.0,""Modifiers"":[{""Label"":""DamagePerSecond"",""Value"":297.0,""OriginalValue"":216.0,
""LessIsGood"":0},{""Label"":""MaximumRange"",""Value"":2820.0,""OriginalValue"":3000.0,""LessIsGood"":0},{""Label"":""RateOfFire"",""Value"":6.25,""OriginalValue"":4.545455,
""LessIsGood"":0},{""Label"":""BurstRateOfFire"",""Value"":10.0,""OriginalValue"":-1.0,""LessIsGood"":0},{""Label"":""BurstSize"",""Value"":2.0,""OriginalValue"":1.0,
""LessIsGood"":0},{""Label"":""AmmoClipSize"",""Value"":4.0,""OriginalValue"":3.0,""LessIsGood"":0}]}},{""Slot"":""LargeHardpoint2"",""Item"":""hpt_slugshot_fixed_large_range"",
""On"":true,""Priority"":2,""AmmoInClip"":4,""AmmoInHopper"":180,""Health"":1.0,""Value"":1365812,""Engineering"":{""Engineer"":""Zacariah Nemo"",""EngineerID"":300050,
""BlueprintID"":128673437,""BlueprintName"":""Weapon_DoubleShot"",""Level"":3,""Quality"":1.0,""Modifiers"":[{""Label"":""DamagePerSecond"",""Value"":297.0,
""OriginalValue"":216.0,""LessIsGood"":0},{""Label"":""MaximumRange"",""Value"":2820.0,""OriginalValue"":3000.0,""LessIsGood"":0},{""Label"":""RateOfFire"",
""Value"":6.25,""OriginalValue"":4.545455,""LessIsGood"":0},{""Label"":""BurstRateOfFire"",""Value"":10.0,""OriginalValue"":-1.0,""LessIsGood"":0},{""Label"":""BurstSize"",
""Value"":2.0,""OriginalValue"":1.0,""LessIsGood"":0},{""Label"":""AmmoClipSize"",""Value"":4.0,""OriginalValue"":3.0,""LessIsGood"":0}]}},{""Slot"":""LargeHardpoint3"",
""Item"":""hpt_slugshot_fixed_large_range"",""On"":true,""Priority"":2,""AmmoInClip"":4,""AmmoInHopper"":180,""Health"":1.0,""Value"":1365812,""Engineering"":{""Engineer"":""Zacariah Nemo"",
""EngineerID"":300050,""BlueprintID"":128673437,""BlueprintName"":""Weapon_DoubleShot"",""Level"":3,""Quality"":1.0,""Modifiers"":[{""Label"":""DamagePerSecond"",
""Value"":297.0,""OriginalValue"":216.0,""LessIsGood"":0},{""Label"":""MaximumRange"",""Value"":2820.0,""OriginalValue"":3000.0,""LessIsGood"":0},{""Label"":""RateOfFire"",
""Value"":6.25,""OriginalValue"":4.545455,""LessIsGood"":0},{""Label"":""BurstRateOfFire"",""Value"":10.0,""OriginalValue"":-1.0,""LessIsGood"":0},{""Label"":""BurstSize"",
""Value"":2.0,""OriginalValue"":1.0,""LessIsGood"":0},{""Label"":""AmmoClipSize"",""Value"":4.0,""OriginalValue"":3.0,""LessIsGood"":0}]}},{""Slot"":""MediumHardpoint1"",
""Item"":""hpt_drunkmissilerack_fixed_medium"",""On"":true,""Priority"":2,""AmmoInClip"":19,""AmmoInHopper"":182,""Health"":1.0,""Value"":749385,""Engineering"":{""Engineer"":""Liz Ryder"",
""EngineerID"":300080,""BlueprintID"":128673476,""BlueprintName"":""Weapon_HighCapacity"",""Level"":2,""Quality"":1.0,""Modifiers"":[{""Label"":""Mass"",
""Value"":5.2,""OriginalValue"":4.0,""LessIsGood"":1},{""Label"":""PowerDraw"",""Value"":1.296,""OriginalValue"":1.2,""LessIsGood"":1},{""Label"":""DamagePerSecond"",
""Value"":62.500004,""OriginalValue"":60.0,""LessIsGood"":0},{""Label"":""RateOfFire"",""Value"":2.083333,""OriginalValue"":2.0,""LessIsGood"":0},{""Label"":""AmmoClipSize"",
""Value"":19.0,""OriginalValue"":12.0,""LessIsGood"":0},{""Label"":""AmmoMaximum"",""Value"":182.0,""OriginalValue"":120.0,""LessIsGood"":0}]}},{""Slot"":""MediumHardpoint2"",
""Item"":""hpt_drunkmissilerack_fixed_medium"",""On"":true,""Priority"":2,""AmmoInClip"":23,""AmmoInHopper"":220,""Health"":1.0,""Value"":749385,""Engineering"":{""Engineer"":""Liz Ryder"",
""EngineerID"":300080,""BlueprintID"":128673478,""BlueprintName"":""Weapon_HighCapacity"",""Level"":4,""Quality"":0.9706,""Modifiers"":[{""Label"":""Mass"",
""Value"":6.0,""OriginalValue"":4.0,""LessIsGood"":1},{""Label"":""PowerDraw"",""Value"":1.392,""OriginalValue"":1.2,""LessIsGood"":1},{""Label"":""DamagePerSecond"",
""Value"":65.217392,""OriginalValue"":60.0,""LessIsGood"":0},{""Label"":""RateOfFire"",""Value"":2.173913,""OriginalValue"":2.0,""LessIsGood"":0},{""Label"":""AmmoClipSize"",
""Value"":23.0,""OriginalValue"":12.0,""LessIsGood"":0},{""Label"":""AmmoMaximum"",""Value"":220.0,""OriginalValue"":120.0,""LessIsGood"":0}]}},{""Slot"":""TinyHardpoint1"",
""Item"":""hpt_chafflauncher_tiny"",""On"":true,""Priority"":1,""AmmoInClip"":1,""AmmoInHopper"":10,""Health"":1.0,""Value"":7045},{""Slot"":""TinyHardpoint2"",
""Item"":""hpt_plasmapointdefence_turret_tiny"",""On"":true,""Priority"":3,""AmmoInClip"":12,""AmmoInHopper"":10000,""Health"":1.0,""Value"":15371},{""Slot"":""TinyHardpoint3"",
""Item"":""hpt_heatsinklauncher_turret_tiny"",""On"":true,""Priority"":3,""AmmoInClip"":1,""AmmoInHopper"":2,""Health"":1.0,""Value"":2901},{""Slot"":""TinyHardpoint4"",
""Item"":""hpt_heatsinklauncher_turret_tiny"",""On"":true,""Priority"":3,""AmmoInClip"":1,""AmmoInHopper"":2,""Health"":1.0,""Value"":2901},{""Slot"":""PaintJob"",
""Item"":""paintjob_python_egypt_01"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""Decal1"",""Item"":""decal_triple_elite"",""On"":true,""Priority"":1,
""Health"":1.0},{""Slot"":""Decal2"",""Item"":""decal_triple_elite"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""Decal3"",""Item"":""decal_triple_elite"",
""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""ShipName0"",""Item"":""nameplate_empire02_white"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""ShipName1"",
""Item"":""nameplate_empire02_white"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""ShipID0"",""Item"":""nameplate_shipid_grey"",""On"":true,""Priority"":1,
""Health"":1.0},{""Slot"":""ShipID1"",""Item"":""nameplate_shipid_grey"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""Armour"",""Item"":""python_armour_reactive"",
""On"":true,""Priority"":1,""Health"":1.0,""Value"":130940128,""Engineering"":{""Engineer"":""Petra Olmanova"",""EngineerID"":300130,""BlueprintID"":128673643,
""BlueprintName"":""Armour_HeavyDuty"",""Level"":4,""Quality"":0.91,""Modifiers"":[{""Label"":""Mass"",""Value"":66.25,""OriginalValue"":53.0,""LessIsGood"":1}
,{""Label"":""DefenceModifierHealthMultiplier"",""Value"":343.835022,""OriginalValue"":250.0,""LessIsGood"":0},{""Label"":""KineticResistance"",""Value"":27.932501,
""OriginalValue"":25.0,""LessIsGood"":0},{""Label"":""ThermicResistance"",""Value"":-34.526001,""OriginalValue"":-39.999996,""LessIsGood"":0},{""Label"":""ExplosiveResistance"",
""Value"":23.127996,""OriginalValue"":19.999998,""LessIsGood"":0}]}},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size7_class2"",""On"":true,""Priority"":1,
""Health"":1.0,""Value"":1194423},{""Slot"":""MainEngines"",""Item"":""int_engine_size6_class5"",""On"":true,""Priority"":0,""Health"":1.0,""Value"":13408787,
""Engineering"":{""Engineer"":""Professor Palin"",""EngineerID"":300220,""BlueprintID"":128673656,""BlueprintName"":""Engine_Dirty"",""Level"":2,""Quality"":0.9129,
""Modifiers"":[{""Label"":""Integrity"",""Value"":116.559998,""OriginalValue"":124.0,""LessIsGood"":0},{""Label"":""PowerDraw"",""Value"":8.013599,""OriginalValue"":7.56,
""LessIsGood"":1},{""Label"":""EngineOptimalMass"",""Value"":1368.0,""OriginalValue"":1440.0,""LessIsGood"":0},{""Label"":""EngineOptPerformance"",""Value"":118.389999,
""OriginalValue"":100.0,""LessIsGood"":0},{""Label"":""EngineHeatRate"",""Value"":1.69,""OriginalValue"":1.3,""LessIsGood"":1}]}},{""Slot"":""FrameShiftDrive"",
""Item"":""int_hyperdrive_overcharge_size5_class2"",""On"":true,""Priority"":0,""Health"":1.0,""Value"":1990542,""Engineering"":{""Engineer"":""Elvira Martuuk"",
""EngineerID"":300160,""BlueprintID"":128673692,""BlueprintName"":""FSD_LongRange"",""Level"":3,""Quality"":0.869,""Modifiers"":[{""Label"":""Mass"",""Value"":9.6,
""OriginalValue"":8.0,""LessIsGood"":1},{""Label"":""Integrity"",""Value"":100.099998,""OriginalValue"":110.0,""LessIsGood"":0},{""Label"":""PowerDraw"",
""Value"":0.545,""OriginalValue"":0.5,""LessIsGood"":1},{""Label"":""FSDOptimalMass"",""Value"":1403.744995,""OriginalValue"":1050.0,""LessIsGood"":0}
]}},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size4_class2"",""On"":true,""Priority"":3,""Health"":1.0,""Value"":23516},{""Slot"":""PowerDistributor"",
""Item"":""int_powerdistributor_size7_class5"",""On"":true,""Priority"":1,""Health"":1.0,""Value"":8065334,""Engineering"":{""Engineer"":""The Dweller"",
""EngineerID"":300180,""BlueprintID"":128673739,""BlueprintName"":""PowerDistributor_HighFrequency"",""Level"":5,""Quality"":1.0,""Modifiers"":[{""Label"":""WeaponsCapacity"",
""Value"":57.950001,""OriginalValue"":61.0,""LessIsGood"":0},{""Label"":""WeaponsRecharge"",""Value"":8.845,""OriginalValue"":6.1,""LessIsGood"":0},{""Label"":""EnginesCapacity"",
""Value"":38.950001,""OriginalValue"":41.0,""LessIsGood"":0},{""Label"":""EnginesRecharge"",""Value"":5.8,""OriginalValue"":4.0,""LessIsGood"":0},{""Label"":""SystemsCapacity"",
""Value"":38.950001,""OriginalValue"":41.0,""LessIsGood"":0},{""Label"":""SystemsRecharge"",""Value"":5.8,""OriginalValue"":4.0,""LessIsGood"":0}]}},
{""Slot"":""Radar"",""Item"":""int_sensors_size6_class1"",""On"":true,""Priority"":2,""Health"":1.0,""Value"":73740},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size5_class3"",
""On"":true,""Priority"":1,""Health"":1.0,""Value"":81013},{""Slot"":""Slot01_Size6"",""Item"":""int_cargorack_size6_class1"",""On"":true,""Priority"":1,
""Health"":1.0,""Value"":300498},{""Slot"":""Slot02_Size6"",""Item"":""int_cargorack_size6_class1"",""On"":true,""Priority"":1,""Health"":1.0,""Value"":300498}
,{""Slot"":""Slot03_Size6"",""Item"":""int_cargorack_size6_class1"",""On"":true,""Priority"":1,""Health"":1.0,""Value"":300498},{""Slot"":""Slot04_Size5"",
""Item"":""int_cargorack_size5_class1"",""On"":true,""Priority"":1,""Health"":1.0,""Value"":92460},{""Slot"":""Slot05_Size5"",""Item"":""int_cargorack_size5_class1"",
""On"":true,""Priority"":1,""Health"":1.0,""Value"":92462},{""Slot"":""Slot06_Size4"",""Item"":""int_corrosionproofcargorack_size4_class1"",""On"":true,
""Priority"":1,""Health"":1.0,""Value"":91970},{""Slot"":""Slot07_Size3"",""Item"":""int_shieldgenerator_size3_class5_strong"",""On"":true,""Priority"":1,
""Health"":1.0,""Value"":742822,""Engineering"":{""Engineer"":""Lei Cheung"",""EngineerID"":300120,""BlueprintID"":128673838,""BlueprintName"":""ShieldGenerator_Reinforced"",
""Level"":4,""Quality"":0.9733,""Modifiers"":[{""Label"":""ShieldGenStrength"",""Value"":197.865005,""OriginalValue"":150.0,""LessIsGood"":0},{""Label"":""BrokenRegenRate"",
""Value"":1.17,""OriginalValue"":1.3,""LessIsGood"":0},{""Label"":""EnergyPerRegen"",""Value"":0.66,""OriginalValue"":0.6,""LessIsGood"":1},{""Label"":""KineticResistance"",
""Value"":48.051994,""OriginalValue"":39.999996,""LessIsGood"":0},{""Label"":""ThermicResistance"",""Value"":-3.89601,""OriginalValue"":-20.000004,
""LessIsGood"":0},{""Label"":""ExplosiveResistance"",""Value"":56.709999,""OriginalValue"":50.0,""LessIsGood"":0}]}},{""Slot"":""Slot08_Size3"",""Item"":""int_cargorack_size3_class1"",
""On"":true,""Priority"":1,""Health"":1.0,""Value"":10299},{""Slot"":""Slot09_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":1,
""Health"":1.0,""Value"":2694},{""Slot"":""Slot10_Size1"",""Item"":""int_corrosionproofcargorack_size1_class2"",""On"":true,""Priority"":1,""Health"":1.0,
""Value"":12249},{""Slot"":""PlanetaryApproachSuite"",""Item"":""int_planetapproachsuite_advanced"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""Bobble01"",
""Item"":""bobble_plant_rosequartz"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""Bobble08"",""Item"":""bobble_plant_anemone"",""On"":true,""Priority"":1,
""Health"":1.0},{""Slot"":""Bobble09"",""Item"":""bobble_plant_succulent"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""Bobble10"",""Item"":""bobble_plant_aloe"",
""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""ShipKitSpoiler"",""Item"":""python_shipkit1_spoiler3"",""On"":true,""Priority"":1,""Health"":1.0},
{""Slot"":""ShipKitWings"",""Item"":""python_shipkit1_wings3"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""ShipKitTail"",""Item"":""python_shipkit1_tail2"",
""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""ShipKitBumper"",""Item"":""python_shipkit1_bumper1"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""WeaponColour"",
""Item"":""weaponcustomisation_green"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""EngineColour"",""Item"":""enginecustomisation_green"",""On"":true,
""Priority"":1,""Health"":1.0},{""Slot"":""VesselVoice"",""Item"":""voicepack_carina"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""ShipCockpit"",
""Item"":""python_cockpit"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":4,
""Health"":1.0}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.LargeHardpoint1, true);
                CheckThat(mod.Mass.Value).IsApprox(8);
                CheckThat(mod.Integrity.Value).IsApprox(64);
                CheckThat(mod.PowerDraw.Value).IsApprox(1.02);
                CheckThat(mod.BootTime.Value).IsApprox(0);
                CheckThat(mod.DPS.Value).IsApprox(297);
                CheckThat(mod.Damage.Value).IsApprox(3.96);
                CheckThat(mod.DistributorDraw.Value).IsApprox(0.57);
                CheckThat(mod.ThermalLoad.Value).IsApprox(1.13);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(45);
                CheckThat(mod.Range.Value).IsApprox(2820);
                CheckThat(mod.Falloff.Value).IsApprox(2800);
                CheckThat(mod.Speed.Value).IsApprox(1000);
                CheckThat(mod.RateOfFire.Value).IsApprox(6.25);
                CheckThat(mod.BurstInterval.Value).IsApprox(0.22);
                CheckThat(mod.BurstRateOfFire.Value).IsApprox(10);
                CheckThat(mod.BurstSize.Value).IsApprox(2);
                CheckThat(mod.Clip).Is( 4);
                CheckThat(mod.Ammo).Is( 180);
                CheckThat(mod.Rounds).Is( 12);
                CheckThat(mod.ReloadTime.Value).IsApprox(5);
                CheckThat(mod.BreachDamage.Value).IsApprox(3.564);
                CheckThat(mod.BreachMin.Value).IsApprox(40);
                CheckThat(mod.BreachMax.Value).IsApprox(80);
                CheckThat(mod.Jitter.Value).IsApprox(1.7);
                CheckThat(mod.KineticProportionDamage.Value).IsApprox(100);
                //CheckThat(mod.ThermalProportionDamage.Value).IsApprox((0));

            }

            {
                // gauss cannon no eng
                string t = @"{""event"":""Loadout"",""Ship"":""python"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":55316050,""ModulesValue"":1357640,""UnladenMass"":622,""CargoCapacity"":0,""MaxJumpRange"":9.186911,""FuelCapacity"":{""Main"":32,""Reserve"":0.83},""Rebuy"":2833684,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""LargeHardpoint1"",""Item"":""hpt_guardian_gausscannon_fixed_small"",""On"":true,""Priority"":0,""Value"":167250},{""Slot"":""Armour"",""Item"":""python_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size7_class1"",""On"":true,""Priority"":0,""Value"":480410},{""Slot"":""MainEngines"",""Item"":""int_engine_size6_class1"",""On"":true,""Priority"":0,""Value"":199750},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size5_class1"",""On"":true,""Priority"":0,""Value"":63010},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size4_class1"",""On"":true,""Priority"":0,""Value"":11350},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size7_class1"",""On"":true,""Priority"":0,""Value"":249140},{""Slot"":""Radar"",""Item"":""int_sensors_size6_class1"",""On"":true,""Priority"":0,""Value"":88980},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size5_class3"",""On"":true,""Priority"":0,""Value"":97750}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.LargeHardpoint1, true);
                CheckThat(mod.Mass.Value).IsApprox(2);
                CheckThat(mod.Integrity.Value).IsApprox(40);
                CheckThat(mod.PowerDraw.Value).IsApprox(1.91);
                CheckThat(mod.BootTime.Value).IsApprox(0);
                CheckThat(mod.DPS.Value).IsApprox(19.7);
                CheckThat(mod.Damage.Value).IsApprox(40);
                CheckThat(mod.Time.Value).IsApprox(1.2);
                CheckThat(mod.DistributorDraw.Value).IsApprox(3.8);
                CheckThat(mod.ThermalLoad.Value).IsApprox(15);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(140);
                CheckThat(mod.Range.Value).IsApprox(3000);
                CheckThat(mod.Falloff.Value).IsApprox(1500);
                CheckThat(mod.RateOfFire.Value).IsApprox(0.4926);
                CheckThat(mod.BurstInterval.Value).IsApprox(0.83);
                CheckThat(mod.BurstRateOfFire.Value).IsApprox(1);
                CheckThat(mod.BurstSize.Value).IsApprox(1);
                CheckThat(mod.Clip).Is( 1);
                CheckThat(mod.Ammo).Is( 80);
                CheckThat(mod.ReloadTime.Value).IsApprox(1);
                CheckThat(mod.BreachDamage.Value).IsApprox(20);
                CheckThat(mod.BreachMin.Value).IsApprox(20);
                CheckThat(mod.BreachMax.Value).IsApprox(40);
                CheckThat(mod.ThermalProportionDamage.Value).IsApprox(50);
                CheckThat(mod.AXPorportionDamage.Value).IsApprox(50);
            }

            {
                // gauss cannon rapid fire
                string t = @"{""event"":""Loadout"",""Ship"":""python"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":55316050,""ModulesValue"":1357640,""UnladenMass"":622,""CargoCapacity"":0,""MaxJumpRange"":9.186911,""FuelCapacity"":{""Main"":32,""Reserve"":0.83},""Rebuy"":2833684,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""LargeHardpoint1"",""Item"":""hpt_guardian_gausscannon_fixed_small"",""On"":true,""Priority"":0,""Value"":167250,""Engineering"":{""BlueprintName"":""Weapon_RapidFire"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""DamagePerSecond"",""Value"":22.825565,""OriginalValue"":19.704433},{""Label"":""Damage"",""Value"":38,""OriginalValue"":40},{""Label"":""DistributorDraw"",""Value"":2.47,""OriginalValue"":3.8},{""Label"":""RateOfFire"",""Value"":0.600673,""OriginalValue"":0.492611}]}},{""Slot"":""Armour"",""Item"":""python_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size7_class1"",""On"":true,""Priority"":0,""Value"":480410},{""Slot"":""MainEngines"",""Item"":""int_engine_size6_class1"",""On"":true,""Priority"":0,""Value"":199750},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size5_class1"",""On"":true,""Priority"":0,""Value"":63010},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size4_class1"",""On"":true,""Priority"":0,""Value"":11350},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size7_class1"",""On"":true,""Priority"":0,""Value"":249140},{""Slot"":""Radar"",""Item"":""int_sensors_size6_class1"",""On"":true,""Priority"":0,""Value"":88980},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size5_class3"",""On"":true,""Priority"":0,""Value"":97750}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.LargeHardpoint1, true);
                CheckThat(mod.Mass.Value).IsApprox(2);
                CheckThat(mod.Integrity.Value).IsApprox(40);
                CheckThat(mod.PowerDraw.Value).IsApprox(1.91);
                CheckThat(mod.BootTime.Value).IsApprox(0);
                CheckThat(mod.DPS.Value).IsApprox(22.83);
                CheckThat(mod.Damage.Value).IsApprox(38);
                CheckThat(mod.Time.Value).IsApprox(1.2);
                CheckThat(mod.DistributorDraw.Value).IsApprox(2.47);
                CheckThat(mod.ThermalLoad.Value).IsApprox(15);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(140);
                CheckThat(mod.Range.Value).IsApprox(3000);
                CheckThat(mod.Falloff.Value).IsApprox(1500);
                CheckThat(mod.RateOfFire.Value).IsApprox(0.6007);
                CheckThat(mod.BurstInterval.Value).IsApprox(0.4658);
                CheckThat(mod.BurstRateOfFire.Value).IsApprox(1);
                CheckThat(mod.BurstSize.Value).IsApprox(1);
                CheckThat(mod.Clip).Is( 1);
                CheckThat(mod.Ammo).Is( 80);
                CheckThat(mod.ReloadTime.Value).IsApprox(1);
                CheckThat(mod.BreachDamage.Value).IsApprox(19);
                CheckThat(mod.BreachMin.Value).IsApprox(20);
                CheckThat(mod.BreachMax.Value).IsApprox(40);
                CheckThat(mod.ThermalProportionDamage.Value).IsApprox(50);
                CheckThat(mod.AXPorportionDamage.Value).IsApprox(50);
            }



            {
                // larger gauss cannon rapid fire
                string t = @"{""event"":""Loadout"",""Ship"":""python"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":55316050,""ModulesValue"":1734190,""UnladenMass"":624,""CargoCapacity"":0,""MaxJumpRange"":9.15762,""FuelCapacity"":{""Main"":32,""Reserve"":0.83},""Rebuy"":2852512,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""LargeHardpoint2"",""Item"":""hpt_guardian_gausscannon_fixed_medium"",""On"":true,""Priority"":0,""Value"":543800,""Engineering"":{""BlueprintName"":""Weapon_RapidFire"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""DamagePerSecond"",""Value"":39.944738,""OriginalValue"":34.482759},{""Label"":""Damage"",""Value"":66.5,""OriginalValue"":70},{""Label"":""DistributorDraw"",""Value"":4.68,""OriginalValue"":7.2},{""Label"":""RateOfFire"",""Value"":0.600673,""OriginalValue"":0.492611}]}},{""Slot"":""Armour"",""Item"":""python_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size7_class1"",""On"":true,""Priority"":0,""Value"":480410},{""Slot"":""MainEngines"",""Item"":""int_engine_size6_class1"",""On"":true,""Priority"":0,""Value"":199750},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size5_class1"",""On"":true,""Priority"":0,""Value"":63010},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size4_class1"",""On"":true,""Priority"":0,""Value"":11350},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size7_class1"",""On"":true,""Priority"":0,""Value"":249140},{""Slot"":""Radar"",""Item"":""int_sensors_size6_class1"",""On"":true,""Priority"":0,""Value"":88980},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size5_class3"",""On"":true,""Priority"":0,""Value"":97750}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.LargeHardpoint2, true);
                CheckThat(mod.Mass.Value).IsApprox(4);
                CheckThat(mod.Integrity.Value).IsApprox(42);
                CheckThat(mod.PowerDraw.Value).IsApprox(2.61);
                CheckThat(mod.BootTime.Value).IsApprox(0);
                CheckThat(mod.DPS.Value).IsApprox(39.94);
                CheckThat(mod.Damage.Value).IsApprox(66.5);
                CheckThat(mod.Time.Value).IsApprox(1.2);
                CheckThat(mod.DistributorDraw.Value).IsApprox(4.68);
                CheckThat(mod.ThermalLoad.Value).IsApprox(25);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(140);
                CheckThat(mod.Range.Value).IsApprox(3000);
                CheckThat(mod.Falloff.Value).IsApprox(1500);
                CheckThat(mod.RateOfFire.Value).IsApprox(0.6007);
                CheckThat(mod.BurstInterval.Value).IsApprox(0.4658);
                CheckThat(mod.BurstRateOfFire.Value).IsApprox(1);
                CheckThat(mod.BurstSize.Value).IsApprox(1);
                CheckThat(mod.Clip).Is( 1);
                CheckThat(mod.Ammo).Is( 80);
                CheckThat(mod.ReloadTime.Value).IsApprox(1);
                CheckThat(mod.BreachDamage.Value).IsApprox(33.25);
                CheckThat(mod.BreachMin.Value).IsApprox(20);
                CheckThat(mod.BreachMax.Value).IsApprox(40);
                CheckThat(mod.ThermalProportionDamage.Value).IsApprox(50);
                CheckThat(mod.AXPorportionDamage.Value).IsApprox(50);


            }

            {
                // robbies FDL
                string t = @"{ ""timestamp"":""2024 - 06 - 28T09: 20:18.255Z"",""event"":""Loadout"",""Ship"":""FerDeLance"",""ShipID"":15,""ShipName"":""Intrepid"",""ShipIdent"":""RXP-2"",""HullValue"":51232230,""ModulesValue"":75512490,""HullHealth"":1.0,""UnladenMass"":512.199951,""CargoCapacity"":16,""FuelCapacity"":{ ""Main"":8.0,""Reserve"":0.67},""Rebuy"":4752930,""Modules"":[{""Slot"":""Armour"",""Item"":""ferdelance_Armour_grade3"",""On"":true,""Priority"":1,""Value"":39448786,""Engineering"":{""Engineer"":""Selene Jean"",""EngineerID"":300210,""BlueprintID"":128673644,""BlueprintName"":""Armour_HeavyDuty"",""Level"":5,""Quality"":0.74,""Modifiers"":[{""Label"":""Mass"",""Value"":49.399998,""OriginalValue"":38.0,""LessIsGood"":1},{""Label"":""DefenceModifierHealthMultiplier"",""Value"":358.5,""OriginalValue"":250.0,""LessIsGood"":0},{""Label"":""KineticResistance"",""Value"":-14.312005,""OriginalValue"":-20.000004,""LessIsGood"":0},{ ""Label"":""ThermicResistance"",""Value"":4.74,""OriginalValue"":0.0,""LessIsGood"":0},{ ""Label"":""ExplosiveResistance"",""Value"":-33.363998,""OriginalValue"":-39.999996,""LessIsGood"":0}]}},{ ""Slot"":""CargoHatch"",""Item"":""modularcargobaydoorfdl"",""On"":true,""Priority"":4},{ ""Slot"":""Decal1"",""Item"":""Decal_Combat_Dangerous"",""On"":true,""Priority"":1},{ ""Slot"":""Decal3"",""Item"":""Decal_Combat_Dangerous"",""On"":true,""Priority"":1},{ ""Slot"":""Decal2"",""Item"":""decal_explorer_elite"",""On"":true,""Priority"":1},{ ""Slot"":""FrameShiftDrive"",""Item"":""Int_hyperdrive_size4_class5"",""On"":true,""Priority"":0,""Value"":1610080,""Engineering"":{ ""Engineer"":""Professor Palin"",""EngineerID"":300220,""BlueprintID"":128673692,""BlueprintName"":""FSD_LongRange"",""Level"":3,""Quality"":0.888,""Modifiers"":[{ ""Label"":""Mass"",""Value"":12.0,""OriginalValue"":10.0,""LessIsGood"":1},{ ""Label"":""Integrity"",""Value"":91.0,""OriginalValue"":100.0,""LessIsGood"":0},{ ""Label"":""PowerDraw"",""Value"":0.4905,""OriginalValue"":0.45,""LessIsGood"":1},{ ""Label"":""FSDOptimalMass"",""Value"":702.869995,""OriginalValue"":525.0,""LessIsGood"":0}]} },{ ""Slot"":""FuelTank"",""Item"":""Int_fueltank_size3_class3"",""On"":true,""Priority"":1,""Value"":7063},{ ""Slot"":""HugeHardpoint1"",""Item"":""Hpt_multicannon_gimbal_huge"",""On"":true,""Priority"":0,""Value"":6377600,""Engineering"":{ ""Engineer"":""Tod 'The Blaster' McQuinn"",""EngineerID"":300260,""BlueprintID"":128673504,""BlueprintName"":""Weapon_Overcharged"",""Level"":5,""Quality"":0.362,""Modifiers"":[{ ""Label"":""DamagePerSecond"",""Value"":38.12291,""OriginalValue"":23.299664,""LessIsGood"":0},{ ""Label"":""Damage"",""Value"":5.661252,""OriginalValue"":3.46,""LessIsGood"":0},{ ""Label"":""DistributorDraw"",""Value"":0.4995,""OriginalValue"":0.37,""LessIsGood"":1},{ ""Label"":""ThermalLoad"",""Value"":0.5865,""OriginalValue"":0.51,""LessIsGood"":1},{ ""Label"":""AmmoClipSize"",""Value"":77.0,""OriginalValue"":90.0,""LessIsGood"":0}]} },{ ""Slot"":""LifeSupport"",""Item"":""Int_lifesupport_size4_class2"",""On"":true,""Priority"":0,""Value"":28373},{ ""Slot"":""MediumHardpoint1"",""Item"":""Hpt_beamlaser_gimbal_medium"",""On"":true,""Priority"":4,""Value"":500600,""Engineering"":{ ""Engineer"":""Broo Tarquin"",""EngineerID"":300030,""BlueprintID"":128739086,""BlueprintName"":""Weapon_Overcharged"",""Level"":5,""Quality"":0.291,""Modifiers"":[{ ""Label"":""DamagePerSecond"",""Value"":20.396334,""OriginalValue"":12.52,""LessIsGood"":0},{ ""Label"":""DistributorDraw"",""Value"":4.644,""OriginalValue"":3.44,""LessIsGood"":1},{ ""Label"":""ThermalLoad"",""Value"":6.118,""OriginalValue"":5.32,""LessIsGood"":1}]} },{ ""Slot"":""MediumHardpoint2"",""Item"":""Hpt_beamlaser_gimbal_medium"",""On"":true,""Priority"":0,""Value"":500600,""Engineering"":{ ""Engineer"":""Broo Tarquin"",""EngineerID"":300030,""BlueprintID"":128739086,""BlueprintName"":""Weapon_Overcharged"",""Level"":5,""Quality"":0.284,""Modifiers"":[{ ""Label"":""DamagePerSecond"",""Value"":20.387569,""OriginalValue"":12.52,""LessIsGood"":0},{ ""Label"":""DistributorDraw"",""Value"":4.644,""OriginalValue"":3.44,""LessIsGood"":1},{ ""Label"":""ThermalLoad"",""Value"":6.118,""OriginalValue"":5.32,""LessIsGood"":1}]} },{ ""Slot"":""Slot01_Size5"",""Item"":""Int_shieldgenerator_size5_class5"",""On"":true,""Priority"":0,""Value"":4338361,""Engineering"":{ ""Engineer"":""Lei Cheung"",""EngineerID"":300120,""BlueprintID"":128673838,""BlueprintName"":""ShieldGenerator_Reinforced"",""Level"":4,""Quality"":0.9617,""Modifiers"":[{ ""Label"":""ShieldGenStrength"",""Value"":158.124008,""OriginalValue"":120.000008,""LessIsGood"":0},{ ""Label"":""BrokenRegenRate"",""Value"":3.375,""OriginalValue"":3.75,""LessIsGood"":0},{ ""Label"":""EnergyPerRegen"",""Value"":0.66,""OriginalValue"":0.6,""LessIsGood"":1},{ ""Label"":""KineticResistance"",""Value"":48.051994,""OriginalValue"":39.999996,""LessIsGood"":0},{ ""Label"":""ThermicResistance"",""Value"":-3.89601,""OriginalValue"":-20.000004,""LessIsGood"":0},{ ""Label"":""ExplosiveResistance"",""Value"":56.709999,""OriginalValue"":50.0,""LessIsGood"":0}]} },{ ""Slot"":""Slot02_Size4"",""Item"":""Int_shieldcellbank_size4_class4"",""On"":true,""Priority"":3,""Value"":177331},{ ""Slot"":""PaintJob"",""Item"":""PaintJob_FerDeLance_BlackFriday_01"",""On"":true,""Priority"":1},{ ""Slot"":""PlanetaryApproachSuite"",""Item"":""Int_planetapproachsuite_advanced"",""On"":true,""Priority"":1},{ ""Slot"":""PowerDistributor"",""Item"":""Int_powerdistributor_size6_class5"",""On"":true,""Priority"":0,""Value"":3475688,""Engineering"":{ ""Engineer"":""The Dweller"",""EngineerID"":300180,""BlueprintID"":128673739,""BlueprintName"":""PowerDistributor_HighFrequency"",""Level"":5,""Quality"":0.2822,""Modifiers"":[{ ""Label"":""WeaponsCapacity"",""Value"":47.5,""OriginalValue"":50.0,""LessIsGood"":0},{ ""Label"":""WeaponsRecharge"",""Value"":7.20408,""OriginalValue"":5.2,""LessIsGood"":0},{ ""Label"":""EnginesCapacity"",""Value"":33.25,""OriginalValue"":35.0,""LessIsGood"":0},{ ""Label"":""EnginesRecharge"",""Value"":4.43328,""OriginalValue"":3.2,""LessIsGood"":0},{ ""Label"":""SystemsCapacity"",""Value"":33.25,""OriginalValue"":35.0,""LessIsGood"":0},{ ""Label"":""SystemsRecharge"",""Value"":4.43328,""OriginalValue"":3.2,""LessIsGood"":0}]} },{ ""Slot"":""PowerPlant"",""Item"":""Int_powerplant_size6_class5"",""On"":true,""Priority"":1,""Value"":13752602},{ ""Slot"":""Radar"",""Item"":""Int_sensors_size4_class2"",""On"":true,""Priority"":0,""Value"":28373,""Engineering"":{ ""Engineer"":""Lei Cheung"",""EngineerID"":300120,""BlueprintID"":128740136,""BlueprintName"":""Sensor_LongRange"",""Level"":5,""Quality"":0.3273,""Modifiers"":[{ ""Label"":""Mass"",""Value"":8.0,""OriginalValue"":4.0,""LessIsGood"":1},{ ""Label"":""SensorTargetScanAngle"",""Value"":21.0,""OriginalValue"":30.0,""LessIsGood"":0},{ ""Label"":""Range"",""Value"":8311.463867,""OriginalValue"":5040.0,""LessIsGood"":0}]} },{ ""Slot"":""ShipCockpit"",""Item"":""ferdelance_cockpit"",""On"":true,""Priority"":1},{ ""Slot"":""MainEngines"",""Item"":""Int_engine_size5_class5"",""On"":true,""Priority"":0,""Value"":4338361,""Engineering"":{ ""Engineer"":""Professor Palin"",""EngineerID"":300220,""BlueprintID"":128673659,""BlueprintName"":""Engine_Dirty"",""Level"":5,""Quality"":0.9757,""Modifiers"":[{ ""Label"":""Integrity"",""Value"":90.100006,""OriginalValue"":106.0,""LessIsGood"":0},{ ""Label"":""PowerDraw"",""Value"":6.8544,""OriginalValue"":6.12,""LessIsGood"":1},{ ""Label"":""EngineOptimalMass"",""Value"":735.0,""OriginalValue"":840.0,""LessIsGood"":0},{ ""Label"":""EngineOptPerformance"",""Value"":139.829987,""OriginalValue"":100.0,""LessIsGood"":0},{ ""Label"":""EngineHeatRate"",""Value"":2.08,""OriginalValue"":1.3,""LessIsGood"":1}]} },{ ""Slot"":""TinyHardpoint1"",""Item"":""Hpt_plasmapointdefence_turret_tiny"",""On"":true,""Priority"":0,""Value"":18546},{ ""Slot"":""TinyHardpoint2"",""Item"":""Hpt_shieldbooster_size0_class4"",""On"":true,""Priority"":0,""Value"":118950,""Engineering"":{ ""Engineer"":""Lei Cheung"",""EngineerID"":300120,""BlueprintID"":128673797,""BlueprintName"":""ShieldBooster_Thermic"",""Level"":3,""Quality"":0.974,""Modifiers"":[{ ""Label"":""KineticResistance"",""Value"":-2.499998,""OriginalValue"":0.0,""LessIsGood"":0},{ ""Label"":""ThermicResistance"",""Value"":16.869999,""OriginalValue"":0.0,""LessIsGood"":0},{ ""Label"":""ExplosiveResistance"",""Value"":-2.499998,""OriginalValue"":0.0,""LessIsGood"":0}]} },{ ""Slot"":""TinyHardpoint3"",""Item"":""Hpt_shieldbooster_size0_class5"",""On"":true,""Priority"":0,""Value"":281000,""Engineering"":{ ""Engineer"":""Lei Cheung"",""EngineerID"":300120,""BlueprintID"":128673782,""BlueprintName"":""ShieldBooster_HeavyDuty"",""Level"":3,""Quality"":0.8829,""Modifiers"":[{ ""Label"":""Mass"",""Value"":10.5,""OriginalValue"":3.5,""LessIsGood"":1},{ ""Label"":""Integrity"",""Value"":52.1712,""OriginalValue"":48.0,""LessIsGood"":0},{ ""Label"":""PowerDraw"",""Value"":1.38,""OriginalValue"":1.2,""LessIsGood"":1},{ ""Label"":""DefenceModifierShieldMultiplier"",""Value"":47.816002,""OriginalValue"":20.000004,""LessIsGood"":0}]} },{ ""Slot"":""TinyHardpoint4"",""Item"":""Hpt_shieldbooster_size0_class4"",""On"":true,""Priority"":0,""Value"":122000,""Engineering"":{ ""Engineer"":""Lei Cheung"",""EngineerID"":300120,""BlueprintID"":128673782,""BlueprintName"":""ShieldBooster_HeavyDuty"",""Level"":3,""Quality"":0.91,""Modifiers"":[{ ""Label"":""Mass"",""Value"":9.0,""OriginalValue"":3.0,""LessIsGood"":1},{ ""Label"":""Integrity"",""Value"":48.928501,""OriginalValue"":45.0,""LessIsGood"":0},{ ""Label"":""PowerDraw"",""Value"":1.15,""OriginalValue"":1.0,""LessIsGood"":1},{ ""Label"":""DefenceModifierShieldMultiplier"",""Value"":43.236794,""OriginalValue"":15.999996,""LessIsGood"":0}]} },{ ""Slot"":""TinyHardpoint5"",""Item"":""Hpt_plasmapointdefence_turret_tiny"",""On"":true,""Priority"":0,""Value"":18546},{ ""Slot"":""TinyHardpoint6"",""Item"":""Hpt_chafflauncher_tiny"",""On"":true,""Priority"":0,""Value"":8500},{ ""Slot"":""ShipName1"",""Item"":""Nameplate_Explorer01_White"",""On"":true,""Priority"":1},{ ""Slot"":""ShipName0"",""Item"":""Nameplate_Explorer01_White"",""On"":true,""Priority"":1},{ ""Slot"":""ShipID1"",""Item"":""nameplate_shipid_doubleline_white"",""On"":true,""Priority"":1},{ ""Slot"":""ShipID0"",""Item"":""nameplate_shipid_doubleline_white"",""On"":true,""Priority"":1},{ ""Slot"":""WeaponColour"",""Item"":""weaponcustomisation_red"",""On"":true,""Priority"":1},{ ""Slot"":""VesselVoice"",""Item"":""VoicePack_Verity"",""On"":true,""Priority"":1},{ ""Slot"":""Slot04_Size2"",""Item"":""Int_buggybay_size2_class1"",""On"":true,""Priority"":0,""Value"":17550},{ ""Slot"":""Slot06_Size1"",""Item"":""Int_dronecontrol_collection_size1_class5"",""On"":true,""Priority"":0,""Value"":9360},{ ""Slot"":""Slot03_Size4"",""Item"":""Int_cargorack_size4_class1"",""On"":true,""Priority"":1,""Value"":33470},{ ""Slot"":""Slot05_Size1"",""Item"":""Int_dronecontrol_collection_size1_class5"",""On"":true,""Priority"":0,""Value"":9360},{ ""Slot"":""MediumHardpoint3"",""Item"":""Hpt_multicannon_gimbal_medium"",""On"":true,""Priority"":0,""Value"":57000,""Engineering"":{ ""Engineer"":""Tod 'The Blaster' McQuinn"",""EngineerID"":300260,""BlueprintID"":128673504,""BlueprintName"":""Weapon_Overcharged"",""Level"":5,""Quality"":0.226,""Modifiers"":[{ ""Label"":""DamagePerSecond"",""Value"":20.469725,""OriginalValue"":12.615385,""LessIsGood"":0},{ ""Label"":""Damage"",""Value"":2.661064,""OriginalValue"":1.64,""LessIsGood"":0},{ ""Label"":""DistributorDraw"",""Value"":0.189,""OriginalValue"":0.14,""LessIsGood"":1},{ ""Label"":""ThermalLoad"",""Value"":0.23,""OriginalValue"":0.2,""LessIsGood"":1},{ ""Label"":""AmmoClipSize"",""Value"":77.0,""OriginalValue"":90.0,""LessIsGood"":0}]} },{ ""Slot"":""MediumHardpoint4"",""Item"":""Hpt_dumbfiremissilerack_fixed_medium"",""On"":true,""Priority"":0,""Value"":234390}]}";

                var mod = EngineerModule(t, ShipSlots.Slot.HugeHardpoint1, true);
                CheckThat(mod.Mass.Value).IsApprox(16);
                CheckThat(mod.Integrity.Value).IsApprox(80);
                CheckThat(mod.PowerDraw.Value).IsApprox(1.22);
                CheckThat(mod.BootTime.Value).IsApprox(0);
                CheckThat(mod.DPS.Value).IsApprox(38.12);
                CheckThat(mod.Damage.Value).IsApprox(5.661);
                CheckThat(mod.DistributorDraw.Value).IsApprox(0.4995);
                CheckThat(mod.ThermalLoad.Value).IsApprox(0.5865);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(68);
                CheckThat(mod.Range.Value).IsApprox(4000);
                CheckThat(mod.Falloff.Value).IsApprox(2000);
                CheckThat(mod.Speed.Value).IsApprox(1600);
                CheckThat(mod.RateOfFire.Value).IsApprox(3.367);
                CheckThat(mod.BurstInterval.Value).IsApprox(0.297);
                CheckThat(mod.Clip).Is( 77);
                CheckThat(mod.Ammo).Is( 2100);
                CheckThat(mod.Rounds).Is( 2);
                CheckThat(mod.ReloadTime.Value).IsApprox(5);
                CheckThat(mod.BreachDamage.Value).IsApprox(5.095);
                CheckThat(mod.BreachMin.Value).IsApprox(40);
                CheckThat(mod.BreachMax.Value).IsApprox(80);
                CheckThat(mod.Jitter.Value).IsApprox(0);
                CheckThat(mod.KineticProportionDamage.Value).IsApprox(100);
                //CheckThat(mod.ThermalProportionDamage.Value).IsApprox((0));

                mod = EngineerModule(t, ShipSlots.Slot.MediumHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(4);
                CheckThat(mod.Integrity.Value).IsApprox(51);
                CheckThat(mod.PowerDraw.Value).IsApprox(1);
                CheckThat(mod.BootTime.Value).IsApprox(0);
                CheckThat(mod.DPS.Value).IsApprox(20.396);
                CheckThat(mod.Damage.Value).IsApprox(20.396);
                CheckThat(mod.DistributorDraw.Value).IsApprox(4.644);
                CheckThat(mod.ThermalLoad.Value).IsApprox(6.118);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(35);
                CheckThat(mod.Range.Value).IsApprox(3000);
                CheckThat(mod.Falloff.Value).IsApprox(600);
                CheckThat(mod.BreachDamage.Value).IsApprox(16.317);
                CheckThat(mod.BreachMin.Value).IsApprox(40);
                CheckThat(mod.BreachMax.Value).IsApprox(80);
                CheckThat(mod.Jitter.Value).IsApprox(0);
                CheckThat(mod.ThermalProportionDamage.Value).IsApprox(100);

                mod = EngineerModule(t, ShipSlots.Slot.MediumHardpoint3);
                CheckThat(mod.Mass.Value).IsApprox(4);
                CheckThat(mod.Integrity.Value).IsApprox(51);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.64);
                CheckThat(mod.BootTime.Value).IsApprox(0);
                CheckThat(mod.DPS.Value).IsApprox(20.47);
                CheckThat(mod.Damage.Value).IsApprox(2.661);
                CheckThat(mod.DistributorDraw.Value).IsApprox(0.189);
                CheckThat(mod.ThermalLoad.Value).IsApprox(0.23);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(37);
                CheckThat(mod.Range.Value).IsApprox(4000);
                CheckThat(mod.Falloff.Value).IsApprox(2000);
                CheckThat(mod.Speed.Value).IsApprox(1600);
                CheckThat(mod.RateOfFire.Value).IsApprox(7.692);
                CheckThat(mod.BurstInterval.Value).IsApprox(0.13);
                CheckThat(mod.Clip.Value).Is( 77);
                CheckThat(mod.Ammo.Value).Is( 2100);
                CheckThat(mod.ReloadTime.Value).IsApprox(5);
                CheckThat(mod.BreachDamage.Value).IsApprox(2.394);
                CheckThat(mod.BreachMin.Value).IsApprox(40);
                CheckThat(mod.BreachMax.Value).IsApprox(80);
                CheckThat(mod.Jitter.Value).IsApprox(0);
                CheckThat(mod.KineticProportionDamage.Value).IsApprox(100);
                //CheckThat(mod.ThermalProportionDamage.Value).IsApprox((0));

                mod = EngineerModule(t, ShipSlots.Slot.TinyHardpoint2);
                CheckThat(mod.Mass.Value).IsApprox(3);
                CheckThat(mod.Integrity.Value).IsApprox(45);
                CheckThat(mod.PowerDraw.Value).IsApprox(1);
                CheckThat(mod.BootTime.Value).IsApprox(0);
                CheckThat(mod.ShieldReinforcement.Value).IsApprox(16);
                CheckThat(mod.KineticResistance.Value).IsApprox(-2.5);
                CheckThat(mod.ThermalResistance.Value).IsApprox(16.87);
                CheckThat(mod.KineticResistance.Value).IsApprox(-2.5);

                mod = EngineerModule(t, ShipSlots.Slot.TinyHardpoint3);
                CheckThat(mod.Mass.Value).IsApprox(10.5);
                CheckThat(mod.Integrity.Value).IsApprox(52.17);
                CheckThat(mod.PowerDraw.Value).IsApprox(1.38);
                CheckThat(mod.BootTime.Value).IsApprox(0);
                CheckThat(mod.ShieldReinforcement.Value).IsApprox(47.82);
                CheckThat(mod.KineticResistance.Value).IsApprox(0);
                CheckThat(mod.ThermalResistance.Value).IsApprox(0);
                CheckThat(mod.KineticResistance.Value).IsApprox(0);

                mod = EngineerModule(t, ShipSlots.Slot.TinyHardpoint4);
                CheckThat(mod.Mass.Value).IsApprox(9);
                CheckThat(mod.Integrity.Value).IsApprox(48.93);
                CheckThat(mod.PowerDraw.Value).IsApprox(1.15);
                CheckThat(mod.BootTime.Value).IsApprox(0);
                CheckThat(mod.ShieldReinforcement.Value).IsApprox(43.24);
                CheckThat(mod.KineticResistance.Value).IsApprox(0);
                CheckThat(mod.ThermalResistance.Value).IsApprox(0);
                CheckThat(mod.KineticResistance.Value).IsApprox(0);

                mod = EngineerModule(t, ShipSlots.Slot.Slot01_Size5);

                CheckThat(mod.Mass.Value).IsApprox(20);
                CheckThat(mod.Integrity.Value).IsApprox(115);
                CheckThat(mod.PowerDraw.Value).IsApprox(3.64);
                CheckThat(mod.BootTime.Value).IsApprox(1);
                CheckThat(mod.MinMass.Value).IsApprox(203);
                CheckThat(mod.OptMass.Value).IsApprox(405);
                CheckThat(mod.MaxMass.Value).IsApprox(1013);
                CheckThat(mod.MinStrength.Value).IsApprox(92.24);
                CheckThat(mod.OptStrength.Value).IsApprox(158.12);
                CheckThat(mod.MaxStrength.Value).IsApprox(224);
                CheckThat(mod.RegenRate.Value).IsApprox(1);
                CheckThat(mod.BrokenRegenRate.Value).IsApprox(3.375);
                CheckThat(mod.MWPerUnit.Value).IsApprox(0.66);
                CheckThat(mod.KineticResistance.Value).IsApprox(48.05);
                CheckThat(mod.ThermalResistance.Value).IsApprox(-3.89601);
                CheckThat(mod.ExplosiveResistance.Value).IsApprox(56.71);
                CheckThat(mod.AXResistance.Value).IsApprox(95);

                Ship si = Ship.CreateFromLoadout(t);
                Debugger.BreakAssert(si != null, "Bad ship");

                var stats = si.GetShipStats(4, 4, 4, 0, 8, 0);

                CheckThat(Math.Round(stats.CurrentSpeed.Value)).Is( 361);
                CheckThat(Math.Round(stats.LadenSpeed)).Is( 389);
                CheckThat(Math.Round(stats.UnladenSpeed)).Is( 391);
                CheckThat(Math.Round(stats.MaxSpeed)).Is( 393);
                CheckThat(Math.Round(stats.CurrentBoost)).Is( 527);
                CheckThat(Math.Round(stats.LadenBoost)).Is( 523);
                CheckThat(Math.Round(stats.UnladenBoost)).Is( 527);
                CheckThat(Math.Round(stats.MaxBoost)).Is( 529);
                CheckThat(stats.CurrentBoostFrequency).IsApprox(9.1867);
                CheckThat(stats.MaxBoostFrequency).IsApprox(4.2857);

                CheckThat(stats.ShieldsSystemPercentage).IsApprox(33.3);
                CheckThat(stats.ShieldsKineticPercentage).IsApprox(46.8);
                CheckThat(stats.ShieldsThermalPercentage).IsApprox(13.6312);
                CheckThat(stats.ShieldsExplosivePercentage).IsApprox(55.6277);
                CheckThat(stats.ShieldsSystemValue).IsApprox(1920.7);
                CheckThat(stats.ShieldsKineticValue).IsApprox(3607.2);
                CheckThat(stats.ShieldsThermalValue).IsApprox(2223.9);
                CheckThat(stats.ShieldsExplosiveValue).IsApprox(4328.7);
                CheckThat(stats.ShieldBuildTime.Value).IsApprox(3 * 60 + 26);
                CheckThat(stats.ShieldRegenTime).IsApprox(10 * 60 + 41);

                CheckThat(stats.ArmourRaw.Value).IsApprox(1031.625);
                CheckThat(stats.ArmourKineticPercentage).IsApprox(-14.312);
                CheckThat(stats.ArmourThermalPercentage).IsApprox(4.74);
                CheckThat(stats.ArmourExplosivePercentage).IsApprox(-33.363);
                CheckThat(stats.ArmourCausticPercentage).IsApprox(0);
                CheckThat(stats.ArmourKineticValue).IsApprox(902.464);
                CheckThat(stats.ArmourThermalValue).IsApprox(1082.95);
                CheckThat(stats.ArmourExplosiveValue).IsApprox(773.54);
                CheckThat(stats.ArmourCausticValue).IsApprox(1031.625);

                CheckThat(stats.FSDCurrentRange.Value).IsApprox(14.903);
                CheckThat(stats.FSDCurrentMaxRange).IsApprox(42.533);
                CheckThat(stats.FSDLadenRange).IsApprox(14.4587);
                CheckThat(stats.FSDUnladenRange).IsApprox(14.9034);
                CheckThat(stats.FSDMaxRange).IsApprox(15.048);
                CheckThat(stats.FSDMaxFuelPerJump).IsApprox(3);

                CheckThat(stats.WeaponRaw.Value).IsApprox(108.396);
                CheckThat(stats.WeaponAbsolutePercentage).IsApprox(0);
                CheckThat(stats.WeaponKineticPercentage).IsApprox(41.874);
                CheckThat(stats.WeaponThermalPercentage).IsApprox(37.624);
                CheckThat(stats.WeaponExplosivePercentage).IsApprox(20.5008);
                CheckThat(stats.WeaponAXPercentage).IsApprox(0);
                CheckThat(stats.WeaponDuration).IsApprox(5.172);
                CheckThat(stats.WeaponDurationMax).IsApprox(8.895);
                CheckThat(stats.WeaponAmmoDuration).IsApprox(3 * 60 + 35);
                CheckThat(stats.WeaponCurSus).IsApprox(28.559);
                CheckThat(stats.WeaponMaxSus).IsApprox(61.219);

            }

            {
                // armour, refinforcement (noting that reinforcement is setting a zero value for armour)
                string t = @"{""timestamp"":""2024-07-11T14:36:56.902Z"",""event"":""Loadout"",""Ship"":""Cutter"",""ShipID"":8,""ShipName"":""Phönix"",""ShipIdent"":""MIS-08"",""HullValue"":175924977,""ModulesValue"":342785372,""HullHealth"":1.0,""UnladenMass"":1799.76001,""CargoCapacity"":704,""FuelCapacity"":{""Main"":64.0,""Reserve"":1.16},""Rebuy"":25935519,""Modules"":[{""Slot"":""Armour"",""Item"":""cutter_Armour_grade1"",""On"":true,""Priority"":1,""Engineering"":{""Engineer"":""Selene Jean"",""EngineerID"":300210,""BlueprintID"":128673634,""BlueprintName"":""Armour_Advanced"",""Level"":5,""Quality"":0.986,""Modifiers"":[{""Label"":""DefenceModifierHealthMultiplier"",""Value"":70.999992,""OriginalValue"":79.999992,""LessIsGood"":0},{""Label"":""KineticResistance"",""Value"":-2.00001,""OriginalValue"":-20.000004,""LessIsGood"":0},{""Label"":""ThermicResistance"",""Value"":14.999998,""OriginalValue"":0.0,""LessIsGood"":0},{""Label"":""ExplosiveResistance"",""Value"":-19.000006,""OriginalValue"":-39.999996,""LessIsGood"":0}]}},{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":3},{""Slot"":""Decal1"",""Item"":""decal_trade_elite"",""On"":true,""Priority"":1},{""Slot"":""Decal3"",""Item"":""decal_trade_elite"",""On"":true,""Priority"":1},{""Slot"":""Decal2"",""Item"":""decal_trade_elite"",""On"":true,""Priority"":1},{""Slot"":""FrameShiftDrive"",""Item"":""Int_hyperdrive_size7_class5"",""On"":true,""Priority"":0,""Engineering"":{""Engineer"":""Felicity Farseer"",""EngineerID"":300100,""BlueprintID"":128673694,""BlueprintName"":""FSD_LongRange"",""Level"":5,""Quality"":1.0,""ExperimentalEffect"":""special_fsd_fuelcapacity"",""ExperimentalEffect_Localised"":""Deep Charge"",""Modifiers"":[{""Label"":""Mass"",""Value"":104.0,""OriginalValue"":80.0,""LessIsGood"":1},{""Label"":""Integrity"",""Value"":139.400009,""OriginalValue"":164.0,""LessIsGood"":0},{""Label"":""PowerDraw"",""Value"":1.08675,""OriginalValue"":0.9,""LessIsGood"":1},{""Label"":""FSDOptimalMass"",""Value"":4185.0,""OriginalValue"":2700.0,""LessIsGood"":0},{""Label"":""MaxFuelPerJump"",""Value"":14.080001,""OriginalValue"":12.8,""LessIsGood"":0}]}},{""Slot"":""FuelTank"",""Item"":""Int_fueltank_size6_class3"",""On"":true,""Priority"":1},{""Slot"":""HugeHardpoint1"",""Item"":""Hpt_multicannon_gimbal_huge"",""On"":true,""Priority"":0,""Engineering"":{""Engineer"":""Tod 'The Blaster' McQuinn"",""EngineerID"":300260,""BlueprintID"":128673504,""BlueprintName"":""Weapon_Overcharged"",""Level"":5,""Quality"":1.0,""ExperimentalEffect"":""special_incendiary_rounds"",""ExperimentalEffect_Localised"":""Incendiary Rounds"",""Modifiers"":[{""Label"":""DamagePerSecond"",""Value"":37.62896,""OriginalValue"":23.299664,""LessIsGood"":0},{""Label"":""Damage"",""Value"":5.882,""OriginalValue"":3.46,""LessIsGood"":0},{""Label"":""DistributorDraw"",""Value"":0.4995,""OriginalValue"":0.37,""LessIsGood"":1},{""Label"":""ThermalLoad"",""Value"":1.7595,""OriginalValue"":0.51,""LessIsGood"":1},{""Label"":""RateOfFire"",""Value"":3.198653,""OriginalValue"":3.367003,""LessIsGood"":0},{""Label"":""AmmoClipSize"",""Value"":77.0,""OriginalValue"":90.0,""LessIsGood"":0},{""Label"":""DamageType"",""ValueStr"":""$Thermic;""}]}},{""Slot"":""LargeHardpoint1"",""Item"":""Hpt_multicannon_gimbal_large"",""On"":true,""Priority"":0,""Engineering"":{""Engineer"":""Tod 'The Blaster' McQuinn"",""EngineerID"":300260,""BlueprintID"":128673504,""BlueprintName"":""Weapon_Overcharged"",""Level"":5,""Quality"":1.0,""ExperimentalEffect"":""special_corrosive_shell"",""ExperimentalEffect_Localised"":""Corrosive Shell"",""Modifiers"":[{""Label"":""DamagePerSecond"",""Value"":32.186665,""OriginalValue"":18.933332,""LessIsGood"":0},{""Label"":""Damage"",""Value"":4.828,""OriginalValue"":2.84,""LessIsGood"":0},{""Label"":""DistributorDraw"",""Value"":0.3375,""OriginalValue"":0.25,""LessIsGood"":1},{""Label"":""ThermalLoad"",""Value"":0.391,""OriginalValue"":0.34,""LessIsGood"":1},{""Label"":""AmmoClipSize"",""Value"":77.0,""OriginalValue"":90.0,""LessIsGood"":0},{""Label"":""AmmoMaximum"",""Value"":1680.0,""OriginalValue"":2100.0,""LessIsGood"":0}]}},{""Slot"":""LargeHardpoint2"",""Item"":""Hpt_multicannon_gimbal_large"",""On"":true,""Priority"":0,""Engineering"":{""Engineer"":""Tod 'The Blaster' McQuinn"",""EngineerID"":300260,""BlueprintID"":128673504,""BlueprintName"":""Weapon_Overcharged"",""Level"":5,""Quality"":1.0,""ExperimentalEffect"":""special_incendiary_rounds"",""ExperimentalEffect_Localised"":""Incendiary Rounds"",""Modifiers"":[{""Label"":""DamagePerSecond"",""Value"":30.577332,""OriginalValue"":18.933332,""LessIsGood"":0},{""Label"":""Damage"",""Value"":4.828,""OriginalValue"":2.84,""LessIsGood"":0},{""Label"":""DistributorDraw"",""Value"":0.3375,""OriginalValue"":0.25,""LessIsGood"":1},{""Label"":""ThermalLoad"",""Value"":1.173,""OriginalValue"":0.34,""LessIsGood"":1},{""Label"":""RateOfFire"",""Value"":6.333333,""OriginalValue"":6.666667,""LessIsGood"":0},{""Label"":""AmmoClipSize"",""Value"":77.0,""OriginalValue"":90.0,""LessIsGood"":0},{""Label"":""DamageType"",""ValueStr"":""$Thermic;""}]}},{""Slot"":""LifeSupport"",""Item"":""Int_lifesupport_size7_class2"",""On"":true,""Priority"":0},{""Slot"":""MediumHardpoint1"",""Item"":""Hpt_pulselaserburst_turret_medium"",""On"":true,""Priority"":1},{""Slot"":""MediumHardpoint2"",""Item"":""Hpt_pulselaser_turret_medium"",""On"":true,""Priority"":1,""Engineering"":{""Engineer"":""The Dweller"",""EngineerID"":300180,""BlueprintID"":128673577,""BlueprintName"":""Weapon_LongRange"",""Level"":3,""Quality"":1.0,""ExperimentalEffect"":""special_phasing_sequence"",""ExperimentalEffect_Localised"":""Phasing Sequence"",""Modifiers"":[{""Label"":""Mass"",""Value"":4.8,""OriginalValue"":4.0,""LessIsGood"":1},{""Label"":""PowerDraw"",""Value"":0.6322,""OriginalValue"":0.58,""LessIsGood"":1},{""Label"":""DamagePerSecond"",""Value"":5.590909,""OriginalValue"":6.212121,""LessIsGood"":0},{""Label"":""Damage"",""Value"":1.845,""OriginalValue"":2.05,""LessIsGood"":0},{""Label"":""MaximumRange"",""Value"":4800.0,""OriginalValue"":3000.0,""LessIsGood"":0},{""Label"":""DamageFalloffRange"",""Value"":4800.0,""OriginalValue"":500.0,""LessIsGood"":0}]}},{""Slot"":""MediumHardpoint3"",""Item"":""Hpt_beamlaser_turret_medium"",""On"":true,""Priority"":1},{""Slot"":""MediumHardpoint4"",""Item"":""Hpt_beamlaser_turret_medium"",""On"":true,""Priority"":0,""Engineering"":{""Engineer"":""Broo Tarquin"",""EngineerID"":300030,""BlueprintID"":128739091,""BlueprintName"":""Weapon_Efficient"",""Level"":5,""Quality"":1.0,""ExperimentalEffect"":""special_thermal_vent"",""ExperimentalEffect_Localised"":""Thermal Vent"",""Modifiers"":[{""Label"":""PowerDraw"",""Value"":0.4836,""OriginalValue"":0.93,""LessIsGood"":1},{""Label"":""DamagePerSecond"",""Value"":10.9492,""OriginalValue"":8.83,""LessIsGood"":0},{""Label"":""DistributorDraw"",""Value"":1.188,""OriginalValue"":2.16,""LessIsGood"":1},{""Label"":""ThermalLoad"",""Value"":1.412,""OriginalValue"":3.53,""LessIsGood"":1}]}},{""Slot"":""Military01"",""Item"":""Int_shieldcellbank_size5_class5"",""On"":true,""Priority"":2},{""Slot"":""Military02"",""Item"":""Int_hullreinforcement_size5_class2"",""On"":true,""Priority"":1,""Engineering"":{""Engineer"":""Selene Jean"",""EngineerID"":300210,""BlueprintID"":128673709,""BlueprintName"":""HullReinforcement_Advanced"",""Level"":5,""Quality"":1.0,""ExperimentalEffect"":""special_hullreinforcement_chunky"",""ExperimentalEffect_Localised"":""Deep Plating"",""Modifiers"":[{""Label"":""Mass"",""Value"":12.16,""OriginalValue"":16.0,""LessIsGood"":1},{""Label"":""DefenceModifierHealthMultiplier"",""Value"":24.0,""OriginalValue"":0.0,""LessIsGood"":0},{""Label"":""DefenceModifierHealthAddition"",""Value"":343.200012,""OriginalValue"":390.0,""LessIsGood"":0},{""Label"":""KineticResistance"",""Value"":0.550002,""OriginalValue"":2.499998,""LessIsGood"":0},{""Label"":""ThermicResistance"",""Value"":0.550002,""OriginalValue"":2.499998,""LessIsGood"":0},{""Label"":""ExplosiveResistance"",""Value"":0.550002,""OriginalValue"":2.499998,""LessIsGood"":0}]}},{""Slot"":""Slot01_Size8"",""Item"":""Int_cargorack_size8_class1"",""On"":true,""Priority"":1},{""Slot"":""Slot10_Size1"",""Item"":""Int_supercruiseassist"",""On"":true,""Priority"":4},{""Slot"":""Slot02_Size8"",""Item"":""Int_cargorack_size8_class1"",""On"":true,""Priority"":1},{""Slot"":""Slot03_Size6"",""Item"":""Int_cargorack_size6_class1"",""On"":true,""Priority"":1},{""Slot"":""Slot04_Size6"",""Item"":""Int_cargorack_size6_class1"",""On"":true,""Priority"":1},{""Slot"":""Slot05_Size6"",""Item"":""Int_shieldgenerator_size6_class5_strong"",""On"":true,""Priority"":0,""Engineering"":{""Engineer"":""Lei Cheung"",""EngineerID"":300120,""BlueprintID"":128673839,""BlueprintName"":""ShieldGenerator_Reinforced"",""Level"":5,""Quality"":1.0,""ExperimentalEffect"":""special_shield_health"",""ExperimentalEffect_Localised"":""Hi-Cap"",""Modifiers"":[{""Label"":""PowerDraw"",""Value"":7.161,""OriginalValue"":6.51,""LessIsGood"":1},{""Label"":""ShieldGenStrength"",""Value"":219.419983,""OriginalValue"":150.0,""LessIsGood"":0},{""Label"":""BrokenRegenRate"",""Value"":2.88,""OriginalValue"":3.2,""LessIsGood"":0},{""Label"":""EnergyPerRegen"",""Value"":0.84,""OriginalValue"":0.6,""LessIsGood"":1},{""Label"":""KineticResistance"",""Value"":49.900002,""OriginalValue"":39.999996,""LessIsGood"":0},{""Label"":""ThermicResistance"",""Value"":-0.199997,""OriginalValue"":-20.000004,""LessIsGood"":0},{""Label"":""ExplosiveResistance"",""Value"":58.25,""OriginalValue"":50.0,""LessIsGood"":0}]}},{""Slot"":""Slot06_Size5"",""Item"":""Int_cargorack_size5_class1"",""On"":true,""Priority"":1},{""Slot"":""Slot07_Size5"",""Item"":""Int_cargorack_size5_class1"",""On"":true,""Priority"":1},{""Slot"":""Slot08_Size4"",""Item"":""Int_dronecontrol_collection_size3_class5"",""On"":true,""Priority"":0},{""Slot"":""Slot09_Size3"",""Item"":""Int_dockingcomputer_advanced"",""On"":true,""Priority"":4},{""Slot"":""PaintJob"",""Item"":""paintjob_cutter_militaire_forest_green"",""On"":true,""Priority"":1},{""Slot"":""PlanetaryApproachSuite"",""Item"":""Int_planetapproachsuite"",""On"":true,""Priority"":1},{""Slot"":""PowerDistributor"",""Item"":""Int_powerdistributor_size7_class5"",""On"":true,""Priority"":0,""Engineering"":{""Engineer"":""The Dweller"",""EngineerID"":300180,""BlueprintID"":128673739,""BlueprintName"":""PowerDistributor_HighFrequency"",""Level"":5,""Quality"":1.0,""ExperimentalEffect"":""special_powerdistributor_fast"",""ExperimentalEffect_Localised"":""Super Conduits"",""Modifiers"":[{""Label"":""WeaponsCapacity"",""Value"":55.632,""OriginalValue"":61.0,""LessIsGood"":0},{""Label"":""WeaponsRecharge"",""Value"":9.1988,""OriginalValue"":6.1,""LessIsGood"":0},{""Label"":""EnginesCapacity"",""Value"":37.391998,""OriginalValue"":41.0,""LessIsGood"":0},{""Label"":""EnginesRecharge"",""Value"":6.032,""OriginalValue"":4.0,""LessIsGood"":0},{""Label"":""SystemsCapacity"",""Value"":37.391998,""OriginalValue"":41.0,""LessIsGood"":0},{""Label"":""SystemsRecharge"",""Value"":6.032,""OriginalValue"":4.0,""LessIsGood"":0}]}},{""Slot"":""PowerPlant"",""Item"":""Int_powerplant_size8_class5"",""On"":true,""Priority"":1,""Engineering"":{""Engineer"":""Hera Tani"",""EngineerID"":300090,""BlueprintID"":128673764,""BlueprintName"":""PowerPlant_Armoured"",""Level"":5,""Quality"":1.0,""ExperimentalEffect"":""special_powerplant_cooled"",""ExperimentalEffect_Localised"":""Thermal Spread"",""Modifiers"":[{""Label"":""Mass"",""Value"":96.0,""OriginalValue"":80.0,""LessIsGood"":1},{""Label"":""Integrity"",""Value"":363.0,""OriginalValue"":165.0,""LessIsGood"":0},{""Label"":""PowerCapacity"",""Value"":40.32,""OriginalValue"":36.0,""LessIsGood"":0},{""Label"":""HeatEfficiency"",""Value"":0.3168,""OriginalValue"":0.4,""LessIsGood"":1}]}},{""Slot"":""Radar"",""Item"":""Int_sensors_size7_class2"",""On"":true,""Priority"":0},{""Slot"":""ShipCockpit"",""Item"":""cutter_cockpit"",""On"":true,""Priority"":1},{""Slot"":""MainEngines"",""Item"":""Int_engine_size8_class5"",""On"":true,""Priority"":0,""Engineering"":{""Engineer"":""Professor Palin"",""EngineerID"":300220,""BlueprintID"":128673659,""BlueprintName"":""Engine_Dirty"",""Level"":5,""Quality"":1.0,""ExperimentalEffect"":""special_engine_overloaded"",""ExperimentalEffect_Localised"":""Drag Drives"",""Modifiers"":[{""Label"":""Integrity"",""Value"":140.25,""OriginalValue"":165.0,""LessIsGood"":0},{""Label"":""PowerDraw"",""Value"":12.096001,""OriginalValue"":10.8,""LessIsGood"":1},{""Label"":""EngineOptimalMass"",""Value"":2940.0,""OriginalValue"":3360.0,""LessIsGood"":0},{""Label"":""EngineOptPerformance"",""Value"":145.600006,""OriginalValue"":100.0,""LessIsGood"":0},{""Label"":""EngineHeatRate"",""Value"":2.288,""OriginalValue"":1.3,""LessIsGood"":1}]}},{""Slot"":""TinyHardpoint1"",""Item"":""Hpt_plasmapointdefence_turret_tiny"",""On"":true,""Priority"":0},{""Slot"":""TinyHardpoint2"",""Item"":""Hpt_shieldbooster_size0_class5"",""On"":true,""Priority"":2,""Engineering"":{""Engineer"":""Felicity Farseer"",""EngineerID"":300100,""BlueprintID"":128673780,""BlueprintName"":""ShieldBooster_HeavyDuty"",""Level"":1,""Quality"":1.0,""Modifiers"":[{""Label"":""Mass"",""Value"":7.0,""OriginalValue"":3.5,""LessIsGood"":1},{""Label"":""Integrity"",""Value"":49.439999,""OriginalValue"":48.0,""LessIsGood"":0},{""Label"":""PowerDraw"",""Value"":1.26,""OriginalValue"":1.2,""LessIsGood"":1},{""Label"":""DefenceModifierShieldMultiplier"",""Value"":32.000004,""OriginalValue"":20.000004,""LessIsGood"":0}]}},{""Slot"":""TinyHardpoint3"",""Item"":""Hpt_heatsinklauncher_turret_tiny"",""On"":true,""Priority"":1},{""Slot"":""TinyHardpoint4"",""Item"":""Hpt_shieldbooster_size0_class5"",""On"":true,""Priority"":2,""Engineering"":{""Engineer"":""Felicity Farseer"",""EngineerID"":300100,""BlueprintID"":128673780,""BlueprintName"":""ShieldBooster_HeavyDuty"",""Level"":1,""Quality"":1.0,""Modifiers"":[{""Label"":""Mass"",""Value"":7.0,""OriginalValue"":3.5,""LessIsGood"":1},{""Label"":""Integrity"",""Value"":49.439999,""OriginalValue"":48.0,""LessIsGood"":0},{""Label"":""PowerDraw"",""Value"":1.26,""OriginalValue"":1.2,""LessIsGood"":1},{""Label"":""DefenceModifierShieldMultiplier"",""Value"":32.000004,""OriginalValue"":20.000004,""LessIsGood"":0}]}},{""Slot"":""TinyHardpoint5"",""Item"":""Hpt_shieldbooster_size0_class5"",""On"":true,""Priority"":2},{""Slot"":""TinyHardpoint6"",""Item"":""Hpt_shieldbooster_size0_class5"",""On"":true,""Priority"":2},{""Slot"":""TinyHardpoint7"",""Item"":""Hpt_plasmapointdefence_turret_tiny"",""On"":true,""Priority"":1},{""Slot"":""TinyHardpoint8"",""Item"":""Hpt_shieldbooster_size0_class5"",""On"":true,""Priority"":2,""Engineering"":{""Engineer"":""Felicity Farseer"",""EngineerID"":300100,""BlueprintID"":128673790,""BlueprintName"":""ShieldBooster_Resistive"",""Level"":1,""Quality"":1.0,""Modifiers"":[{""Label"":""Integrity"",""Value"":46.079998,""OriginalValue"":48.0,""LessIsGood"":0},{""Label"":""PowerDraw"",""Value"":1.26,""OriginalValue"":1.2,""LessIsGood"":1},{""Label"":""KineticResistance"",""Value"":5.000001,""OriginalValue"":0.0,""LessIsGood"":0},{""Label"":""ThermicResistance"",""Value"":5.000001,""OriginalValue"":0.0,""LessIsGood"":0},{""Label"":""ExplosiveResistance"",""Value"":5.000001,""OriginalValue"":0.0,""LessIsGood"":0}]}},{""Slot"":""VesselVoice"",""Item"":""voicepack_verity"",""On"":true,""Priority"":1}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.Armour);

                CheckThat(mod.Mass.Value).IsApprox(0);
                CheckThat(mod.HullStrengthBonus.Value).IsApprox(71);
                CheckThat(mod.KineticResistance.Value).IsApprox(-2);
                CheckThat(mod.ThermalResistance.Value).IsApprox(15);
                CheckThat(mod.ExplosiveResistance.Value).IsApprox(-19);


                mod = EngineerModule(t, ShipSlots.Slot.Military02);
                CheckThat(mod.Mass.Value).IsApprox(12.16);
                CheckThat(mod.HullStrengthBonus.Value).IsApprox(24);
                CheckThat(mod.HullReinforcement.Value).IsApprox(343.2);
                CheckThat(mod.KineticResistance.Value).IsApprox(0.55);
                CheckThat(mod.ThermalResistance.Value).IsApprox(0.55);
                CheckThat(mod.ExplosiveResistance.Value).IsApprox(0.55);

                Ship si = Ship.CreateFromLoadout(t);
                Debugger.BreakAssert(si != null, "Bad ship");
                var stats = si.GetShipStats(4, 4, 4, 0, 8, 0);
                CheckThat(stats.ArmourRaw.Value).IsApprox(1123.2);
            }

            {
                // edsy mixed with ealhstans loadout description direct from game
                string t = @"{ ""event"":""Loadout"",""Ship"":""krait_mkii"",""ShipName"":"""",""ShipIdent"":""ST-13K"",""HullValue"":38743029,""ModulesValue"":115884722,""UnladenMass"":559,""CargoCapacity"":0,""MaxJumpRange"":21.837943,""FuelCapacity"":{""Main"":32,""Reserve"":0.63},""Rebuy"":7731387,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""MediumHardpoint1"",""Item"":""Hpt_pulselaserburst_gimbal_medium"",""On"":true,""Priority"":0,""Value"":42559,""Engineering"":{""Engineer"":""The Dweller"",""EngineerID"":300180,""BlueprintID"":128673362,""BlueprintName"":""Weapon_Focused"",""Level"":3,""Quality"":0.3,""ExperimentalEffect"":""special_distortion_field"",""ExperimentalEffect_Localised"":""Inertial Impact"",""Modifiers"":[{""Label"":""ThermalLoad"",""Value"":0.6901,""OriginalValue"":0.67,""LessIsGood"":1},{""Label"":""ArmourPenetration"",""Value"":58.100002,""OriginalValue"":35.0,""LessIsGood"":0},{""Label"":""MaximumRange"",""Value"":4714.5,""OriginalValue"":3000.0,""LessIsGood"":0},{""Label"":""Jitter"",""Value"":3.0,""OriginalValue"":0.0,""LessIsGood"":1},{""Label"":""DamageType"",""ValueStr"":""$Kinetic;""},{""Label"":""DamageFalloffRange"",""Value"":785.750061,""OriginalValue"":500.0,""LessIsGood"":0}]}},{""Slot"":""Armour"",""Item"":""krait_mkii_armour_reactive"",""On"":true,""Priority"":1,""Value"":94756030},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size7_class2"",""On"":true,""Priority"":1,""Value"":1264679},{""Slot"":""MainEngines"",""Item"":""int_engine_size6_class5"",""On"":true,""Priority"":0,""Value"":14197538},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size5_class5"",""On"":true,""Priority"":0,""Value"":4478716},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size4_class2"",""On"":true,""Priority"":0,""Value"":24895},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size7_class2"",""On"":true,""Priority"":0,""Value"":546542},{""Slot"":""Radar"",""Item"":""int_sensors_size6_class3"",""On"":true,""Priority"":0,""Value"":487987},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size5_class3"",""On"":true,""Priority"":1,""Value"":85776}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.MediumHardpoint1, true);

                CheckThat(mod.Mass.Value).IsApprox(4);
                CheckThat(mod.Integrity.Value).IsApprox(40);
                CheckThat(mod.PowerDraw.Value).IsApprox(1.04);
                CheckThat(mod.BootTime.Value).IsApprox(0);
                CheckThat(mod.DPS.Value).IsApprox(15.445);
                CheckThat(mod.Damage.Value).IsApprox(3.675);
                CheckThat(mod.DistributorDraw.Value).IsApprox(0.49);
                CheckThat(mod.ThermalLoad.Value).IsApprox(0.6901);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(58.1);
                CheckThat(mod.Range.Value).IsApprox(4715);
                CheckThat(mod.Falloff.Value).IsApprox(785.8);
                CheckThat(mod.RateOfFire.Value).IsApprox(4.203);
                CheckThat(mod.BurstInterval.Value).IsApprox(0.56);
                CheckThat(mod.BurstRateOfFire.Value).IsApprox(13);
                CheckThat(mod.BurstSize.Value).IsApprox(3);
                CheckThat(mod.BreachDamage.Value).IsApprox(3.124);
                CheckThat(mod.BreachMin.Value).IsApprox(40);
                CheckThat(mod.BreachMax.Value).IsApprox(80);
                CheckThat(mod.Jitter.Value).IsApprox(3);
                CheckThat(mod.KineticProportionDamage.Value).IsApprox(50);
                CheckThat(mod.ThermalProportionDamage.Value).IsApprox(50);


            }

            {

                //TEST Module Hpt_pulselaserburst_gimbal_medium in MediumHardpoint2 Blueprint: Efficient Weapon
                //Level: 5
                //Quality: 1
                //Power Draw: 0.541, Original: 1.04, Mult: -48.0 % (Worse)
                //Damage Per Second: 12.767, Original: 10.296, Mult: 24.0 % (Better)
                //Damage: 3.038, Original: 2.45, Mult: 24.0 % (Better)
                //Distributor Draw: 0.27, Original: 0.49, Mult: -45.0 % (Worse)
                //Thermal Load: 0.268, Original: 0.67, Mult: -60.0 % (Worse)

                //Engineer Burst Laser Gimbal Medium PowerDraw PowerDraw 1.04-> 0.5408 ratio 0.52
                //Engineer Burst Laser Gimbal Medium DamagePerSecond DPS 10.296-> 12.767457 ratio 1.24000003496389
                //   Engineer Burst Laser Gimbal Medium DamagePerSecond Damage NOT changing due to primary modifier being present
                //   Engineer Burst Laser Gimbal Medium DamagePerSecond BreachDamage NOT changing due to condition - Damage
                //Engineer Burst Laser Gimbal Medium Damage Damage 2.45-> 3.038 ratio 1.24
                //   Engineer Burst Laser Gimbal Medium Damage BreachDamage 2.1-> 2.604 ratio 1.24
                //   Engineer Burst Laser Gimbal Medium Damage BurstInterval NOT changing due to condition +hpt_railgun *
                //Engineer Burst Laser Gimbal Medium DistributorDraw DistributorDraw 0.49-> 0.2695 ratio 0.55
                //Engineer Burst Laser Gimbal Medium ThermalLoad ThermalLoad 0.67-> 0.268 ratio 0.4

                string t = @"{""event"":""Loadout"",""Ship"":""krait_mkii"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":44152080,""ModulesValue"":1708430,""UnladenMass"":636,""CargoCapacity"":82,""MaxJumpRange"":8.985727,""FuelCapacity"":{""Main"":32,""Reserve"":0.63},""Rebuy"":2293025,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""MediumHardpoint1"",""Item"":""hpt_pulselaser_fixed_small"",""On"":true,""Priority"":0,""Value"":2200},{""Slot"":""MediumHardpoint2"",""Item"":""hpt_pulselaserburst_gimbal_medium"",""On"":true,""Priority"":0,""Value"":48500,""Engineering"":{""BlueprintName"":""Weapon_Efficient"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""PowerDraw"",""Value"":0.5408,""OriginalValue"":1.04},{""Label"":""DamagePerSecond"",""Value"":12.767457,""OriginalValue"":10.296336},{""Label"":""Damage"",""Value"":3.038,""OriginalValue"":2.45},{""Label"":""DistributorDraw"",""Value"":0.2695,""OriginalValue"":0.49},{""Label"":""ThermalLoad"",""Value"":0.268,""OriginalValue"":0.67}]}},{""Slot"":""Armour"",""Item"":""krait_mkii_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size7_class1"",""On"":true,""Priority"":0,""Value"":480410},{""Slot"":""MainEngines"",""Item"":""int_engine_size6_class1"",""On"":true,""Priority"":0,""Value"":199750},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size5_class1"",""On"":true,""Priority"":0,""Value"":63010},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size4_class1"",""On"":true,""Priority"":0,""Value"":11350},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size7_class1"",""On"":true,""Priority"":0,""Value"":249140},{""Slot"":""Radar"",""Item"":""int_sensors_size6_class1"",""On"":true,""Priority"":0,""Value"":88980},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size5_class3"",""On"":true,""Priority"":0,""Value"":97750},{""Slot"":""Slot01_Size6"",""Item"":""int_shieldgenerator_size6_class1"",""On"":true,""Priority"":0,""Value"":199750},{""Slot"":""Slot02_Size6"",""Item"":""int_cargorack_size5_class1"",""On"":true,""Priority"":0,""Value"":111570},{""Slot"":""Slot03_Size5"",""Item"":""int_cargorack_size5_class1"",""On"":true,""Priority"":0,""Value"":111570},{""Slot"":""Slot04_Size5"",""Item"":""int_cargorack_size4_class1"",""On"":true,""Priority"":0,""Value"":34330},{""Slot"":""Slot08_Size2"",""Item"":""int_cargorack_size1_class1"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot09_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":9120}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.MediumHardpoint2, true);

                CheckThat(mod.Mass.Value).IsApprox(4);
                CheckThat(mod.Integrity.Value).IsApprox(40);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.5408);
                CheckThat(mod.BootTime.Value).IsApprox(0);
                CheckThat(mod.DPS.Value).IsApprox(12.767);
                CheckThat(mod.Damage.Value).IsApprox(3.038);
                CheckThat(mod.DistributorDraw.Value).IsApprox(0.2695);
                CheckThat(mod.ThermalLoad.Value).IsApprox(0.268);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(35);
                CheckThat(mod.Range.Value).IsApprox(3000);
                CheckThat(mod.Falloff.Value).IsApprox(500);
                CheckThat(mod.RateOfFire.Value).IsApprox(4.203);
                CheckThat(mod.BurstInterval.Value).IsApprox(0.56);
                CheckThat(mod.BurstRateOfFire.Value).IsApprox(13);
                CheckThat(mod.BurstSize.Value).IsApprox(3);
                CheckThat(mod.BreachDamage.Value).IsApprox(2.582);
                CheckThat(mod.BreachMin.Value).IsApprox(40);
                CheckThat(mod.BreachMax.Value).IsApprox(80);
                CheckThat(mod.Jitter.Value).IsApprox(0);
              //  CheckThat(mod.KineticProportionDamage.Value).IsApprox((0));
                CheckThat(mod.ThermalProportionDamage.Value).IsApprox(100);
            }


            {
                //TEST Module Hpt_pulselaserburst_turret_small in SmallHardpoint1 Blueprint: Focused Weapon
                //Level: 5
                //Quality: 1
                //Experimental Effect: Inertial Impact
                //   Damage: 50
                //   Jitter: 3
                //   KineticProportionDamage: 50
                //   ThermalProportionDamage: 50

                //Damage Per Second: 6.261, Original: 4.174, Mult: 50.0 % (Better)
                //Damage: 1.305, Original: 0.87, Mult: 50.0 % (Better)
                //Thermal Load: 0.2, Original: 0.19, Mult: 5.0 % (Better)
                //Armour Penetration: 44, Original: 20, Mult: 120.0 % (Better)
                //Maximum Range: 6000, Original: 3000, Mult: 100.0 % (Better)
                //Falloff Range: 1000, Original: 500, Mult: 100.0 % (Better)
                //Jitter: 3, Original: 0, Mult: ∞% (Better)

                string t = @"{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":61300,""UnladenMass"":38.4,""CargoCapacity"":0,""MaxJumpRange"":9.089833,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":3318,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaserburst_turret_small"",""On"":true,""Priority"":0,""Value"":52800,""Engineering"":{""BlueprintName"":""Weapon_Focused"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_distortion_field"",""Modifiers"":[{""Label"":""DamagePerSecond"",""Value"":6.261364,""OriginalValue"":4.174242},{""Label"":""Damage"",""Value"":1.305,""OriginalValue"":0.87},{""Label"":""ThermalLoad"",""Value"":0.1995,""OriginalValue"":0.19},{""Label"":""ArmourPenetration"",""Value"":44,""OriginalValue"":20},{""Label"":""MaximumRange"",""Value"":6000,""OriginalValue"":3000},{""Label"":""FalloffRange"",""Value"":1000,""OriginalValue"":500},{""Label"":""Jitter"",""Value"":3,""OriginalValue"":0}]}},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.SmallHardpoint1, true);

                CheckThat(mod.Mass.Value).IsApprox(2);
                CheckThat(mod.Integrity.Value).IsApprox(40);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.6);
                CheckThat(mod.BootTime.Value).IsApprox(0);
                CheckThat(mod.DPS.Value).IsApprox(6.261);
                CheckThat(mod.Damage.Value).IsApprox(1.305);
                CheckThat(mod.DistributorDraw.Value).IsApprox(0.139);
                CheckThat(mod.ThermalLoad.Value).IsApprox(0.1995);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(44);
                CheckThat(mod.Range.Value).IsApprox(6000);
                CheckThat(mod.Falloff.Value).IsApprox(1000);
                CheckThat(mod.RateOfFire.Value).IsApprox(4.798);
                CheckThat(mod.BurstInterval.Value).IsApprox(0.52);
                CheckThat(mod.BurstRateOfFire.Value).IsApprox(19);
                CheckThat(mod.BurstSize.Value).IsApprox(3);
                CheckThat(mod.BreachDamage.Value).IsApprox(0.652);
                CheckThat(mod.BreachMin.Value).IsApprox(60);
                CheckThat(mod.BreachMax.Value).IsApprox(80);
                CheckThat(mod.Jitter.Value).IsApprox(3);
                CheckThat(mod.KineticProportionDamage.Value).IsApprox(50);
                CheckThat(mod.ThermalProportionDamage.Value).IsApprox(50);
            }

            {
                //TEST Module Hpt_pulselaserburst_gimbal_medium in MediumHardpoint2 Blueprint: Efficient Weapon
                //Level: 3
                //Quality: 1
                //Power Draw: 0.79, Original: 1.04, Mult: -24.0 % (Worse)
                //Damage Per Second: 11.944, Original: 10.296, Mult: 16.0 % (Better)
                //Damage: 2.842, Original: 2.45, Mult: 16.0 % (Better)
                //Distributor Draw: 0.368, Original: 0.49, Mult: -25.0 % (Worse)
                //Thermal Load: 0.352, Original: 0.67, Mult: -47.5 % (Worse)

                //Engineer Burst Laser Gimbal Medium PowerDraw PowerDraw 1.04-> 0.7904 ratio 0.76
                //Engineer Burst Laser Gimbal Medium DamagePerSecond DPS 10.296-> 11.94375 ratio 1.16000002330926
                //   Engineer Burst Laser Gimbal Medium DamagePerSecond Damage NOT changing due to primary modifier being present
                //   Engineer Burst Laser Gimbal Medium DamagePerSecond BreachDamage NOT changing due to condition - Damage
                //Engineer Burst Laser Gimbal Medium Damage Damage 2.45-> 2.842 ratio 1.16
                //   Engineer Burst Laser Gimbal Medium Damage BreachDamage 2.1-> 2.436 ratio 1.16
                //   Engineer Burst Laser Gimbal Medium Damage BurstInterval NOT changing due to condition +hpt_railgun *
                //Engineer Burst Laser Gimbal Medium DistributorDraw DistributorDraw 0.49-> 0.3675 ratio 0.75
                //Engineer Burst Laser Gimbal Medium ThermalLoad ThermalLoad 0.67-> 0.35175 ratio 0.525

                string t = @"{""event"":""Loadout"",""Ship"":""krait_mkii"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":44152080,""ModulesValue"":1238890,""UnladenMass"":594,""CargoCapacity"":0,""MaxJumpRange"":9.617571,""FuelCapacity"":{""Main"":32,""Reserve"":0.63},""Rebuy"":2269548,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""MediumHardpoint2"",""Item"":""hpt_pulselaserburst_gimbal_medium"",""On"":true,""Priority"":0,""Value"":48500,""Engineering"":{""BlueprintName"":""Weapon_Efficient"",""Level"":3,""Quality"":1,""Modifiers"":[{""Label"":""PowerDraw"",""Value"":0.7904,""OriginalValue"":1.04},{""Label"":""DamagePerSecond"",""Value"":11.94375,""OriginalValue"":10.296336},{""Label"":""Damage"",""Value"":2.842,""OriginalValue"":2.45},{""Label"":""DistributorDraw"",""Value"":0.3675,""OriginalValue"":0.49},{""Label"":""ThermalLoad"",""Value"":0.35175,""OriginalValue"":0.67}]}},{""Slot"":""Armour"",""Item"":""krait_mkii_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size7_class1"",""On"":true,""Priority"":0,""Value"":480410},{""Slot"":""MainEngines"",""Item"":""int_engine_size6_class1"",""On"":true,""Priority"":0,""Value"":199750},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size5_class1"",""On"":true,""Priority"":0,""Value"":63010},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size4_class1"",""On"":true,""Priority"":0,""Value"":11350},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size7_class1"",""On"":true,""Priority"":0,""Value"":249140},{""Slot"":""Radar"",""Item"":""int_sensors_size6_class1"",""On"":true,""Priority"":0,""Value"":88980},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size5_class3"",""On"":true,""Priority"":0,""Value"":97750}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.MediumHardpoint2, true);

                CheckThat(mod.Mass.Value).IsApprox(4);
                CheckThat(mod.Integrity.Value).IsApprox(40);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.7904);
                CheckThat(mod.BootTime.Value).IsApprox(0);
                CheckThat(mod.DPS.Value).IsApprox(11.944);
                CheckThat(mod.Damage.Value).IsApprox(2.842);
                CheckThat(mod.DistributorDraw.Value).IsApprox(0.3675);
                CheckThat(mod.ThermalLoad.Value).IsApprox(0.3518);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(35);
                CheckThat(mod.Range.Value).IsApprox(3000);
                CheckThat(mod.Falloff.Value).IsApprox(500);
                CheckThat(mod.RateOfFire.Value).IsApprox(4.203);
                CheckThat(mod.BurstInterval.Value).IsApprox(0.56);
                CheckThat(mod.BurstRateOfFire.Value).IsApprox(13);
                CheckThat(mod.BurstSize.Value).IsApprox(3);
                CheckThat(mod.BreachDamage.Value).IsApprox(2.416);
                CheckThat(mod.BreachMin.Value).IsApprox(40);
                CheckThat(mod.BreachMax.Value).IsApprox(80);
                CheckThat(mod.Jitter.Value).IsApprox(0);
            //    CheckThat(mod.KineticProportionDamage.Value).IsApprox((0));
                CheckThat(mod.ThermalProportionDamage.Value).IsApprox(100);
            }

            {
                //TEST Module Hpt_pulselaserburst_gimbal_medium in MediumHardpoint2 Blueprint: Focused Weapon
                //Level: 3
                //Quality: 1
                //Thermal Load: 0.69, Original: 0.67, Mult: 3.0 % (Better)
                //Armour Penetration: 63, Original: 35, Mult: 80.0 % (Better)
                //Maximum Range: 5040, Original: 3000, Mult: 68.0 % (Better)
                //Falloff Range: 840, Original: 500, Mult: 68.0 % (Better)

                //* **Engineer module Hpt_pulselaserburst_gimbal_medium
                //Engineer Burst Laser Gimbal Medium ThermalLoad ThermalLoad 0.67-> 0.6901 ratio 1.03
                //Engineer Burst Laser Gimbal Medium ArmourPenetration ArmourPiercing 35-> 63 ratio 1.8
                //Engineer Burst Laser Gimbal Medium MaximumRange Range 3000-> 5040 ratio 1.68
                //Engineer Burst Laser Gimbal Medium FalloffRange Falloff 500-> 840 ratio 1.68


                string t = @"{""event"":""Loadout"",""Ship"":""krait_mkii"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":44152080,""ModulesValue"":1238890,""UnladenMass"":594,""CargoCapacity"":0,""MaxJumpRange"":9.617571,""FuelCapacity"":{""Main"":32,""Reserve"":0.63},""Rebuy"":2269548,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""MediumHardpoint2"",""Item"":""hpt_pulselaserburst_gimbal_medium"",""On"":true,""Priority"":0,""Value"":48500,""Engineering"":{""BlueprintName"":""Weapon_Focused"",""Level"":3,""Quality"":1,""Modifiers"":[{""Label"":""ThermalLoad"",""Value"":0.6901,""OriginalValue"":0.67},{""Label"":""ArmourPenetration"",""Value"":63,""OriginalValue"":35},{""Label"":""MaximumRange"",""Value"":5040,""OriginalValue"":3000},{""Label"":""FalloffRange"",""Value"":840,""OriginalValue"":500}]}},{""Slot"":""Armour"",""Item"":""krait_mkii_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size7_class1"",""On"":true,""Priority"":0,""Value"":480410},{""Slot"":""MainEngines"",""Item"":""int_engine_size6_class1"",""On"":true,""Priority"":0,""Value"":199750},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size5_class1"",""On"":true,""Priority"":0,""Value"":63010},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size4_class1"",""On"":true,""Priority"":0,""Value"":11350},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size7_class1"",""On"":true,""Priority"":0,""Value"":249140},{""Slot"":""Radar"",""Item"":""int_sensors_size6_class1"",""On"":true,""Priority"":0,""Value"":88980},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size5_class3"",""On"":true,""Priority"":0,""Value"":97750}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.MediumHardpoint2, true);

                CheckThat(mod.Mass.Value).IsApprox(4);
                CheckThat(mod.Integrity.Value).IsApprox(40);
                CheckThat(mod.PowerDraw.Value).IsApprox(1.04);
                CheckThat(mod.BootTime.Value).IsApprox(0);
                CheckThat(mod.DPS.Value).IsApprox(10.296);
                CheckThat(mod.Damage.Value).IsApprox(2.45);
                CheckThat(mod.DistributorDraw.Value).IsApprox(0.49);
                CheckThat(mod.ThermalLoad.Value).IsApprox(0.6901);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(63);
                CheckThat(mod.Range.Value).IsApprox(5040);
                CheckThat(mod.Falloff.Value).IsApprox(840);
                CheckThat(mod.RateOfFire.Value).IsApprox(4.203);
                CheckThat(mod.BurstInterval.Value).IsApprox(0.56);
                CheckThat(mod.BurstRateOfFire.Value).IsApprox(13);
                CheckThat(mod.BurstSize.Value).IsApprox(3);
                CheckThat(mod.BreachDamage.Value).IsApprox(2.083);
                CheckThat(mod.BreachMin.Value).IsApprox(40);
                CheckThat(mod.BreachMax.Value).IsApprox(80);
                CheckThat(mod.Jitter.Value).IsApprox(0);
              // CheckThat(mod.KineticProportionDamage.Value).IsApprox((0));
                CheckThat(mod.ThermalProportionDamage.Value).IsApprox(100);
            }

            {
                // From edsy
                //TEST Module Hpt_pulselaserburst_gimbal_medium in MediumHardpoint2 Blueprint: Focused Weapon
                //Level: 3
                //Quality: 1
                //Experimental Effect: Inertial Impact
                //   Damage: 50
                //   Jitter: 3
                //   KineticProportionDamage: 50
                //   ThermalProportionDamage: 50
                //Damage Per Second: 15.445, Original: 10.296, Mult: 50.0 % (Better)
                //Damage: 3.675, Original: 2.45, Mult: 50.0 % (Better)
                //Thermal Load: 0.69, Original: 0.67, Mult: 3.0 % (Better)
                //Armour Penetration: 63, Original: 35, Mult: 80.0 % (Better)
                //Maximum Range: 5040, Original: 3000, Mult: 68.0 % (Better)
                //Falloff Range: 840, Original: 500, Mult: 68.0 % (Better)
                //Jitter: 3, Original: 0, Mult: ∞% (Better)

                string t = @"{""event"":""Loadout"",""Ship"":""krait_mkii"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":44152080,""ModulesValue"":1238890,""UnladenMass"":594,""CargoCapacity"":0,""MaxJumpRange"":9.617571,""FuelCapacity"":{""Main"":32,""Reserve"":0.63},""Rebuy"":2269548,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""MediumHardpoint2"",""Item"":""hpt_pulselaserburst_gimbal_medium"",""On"":true,""Priority"":0,""Value"":48500,""Engineering"":{""BlueprintName"":""Weapon_Focused"",""Level"":3,""Quality"":1,""ExperimentalEffect"":""special_distortion_field"",""Modifiers"":[{""Label"":""DamagePerSecond"",""Value"":15.444504,""OriginalValue"":10.296336},{""Label"":""Damage"",""Value"":3.675,""OriginalValue"":2.45},{""Label"":""ThermalLoad"",""Value"":0.6901,""OriginalValue"":0.67},{""Label"":""ArmourPenetration"",""Value"":63,""OriginalValue"":35},{""Label"":""MaximumRange"",""Value"":5040,""OriginalValue"":3000},{""Label"":""FalloffRange"",""Value"":840,""OriginalValue"":500},{""Label"":""Jitter"",""Value"":3,""OriginalValue"":0}]}},{""Slot"":""Armour"",""Item"":""krait_mkii_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size7_class1"",""On"":true,""Priority"":0,""Value"":480410},{""Slot"":""MainEngines"",""Item"":""int_engine_size6_class1"",""On"":true,""Priority"":0,""Value"":199750},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size5_class1"",""On"":true,""Priority"":0,""Value"":63010},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size4_class1"",""On"":true,""Priority"":0,""Value"":11350},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size7_class1"",""On"":true,""Priority"":0,""Value"":249140},{""Slot"":""Radar"",""Item"":""int_sensors_size6_class1"",""On"":true,""Priority"":0,""Value"":88980},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size5_class3"",""On"":true,""Priority"":0,""Value"":97750}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.MediumHardpoint2, true);

                CheckThat(mod.Mass.Value).IsApprox(4);
                CheckThat(mod.Integrity.Value).IsApprox(40);
                CheckThat(mod.PowerDraw.Value).IsApprox(1.04);
                CheckThat(mod.BootTime.Value).IsApprox(0);
                CheckThat(mod.DPS.Value).IsApprox(15.445);
                CheckThat(mod.Damage.Value).IsApprox(3.675);
                CheckThat(mod.DistributorDraw.Value).IsApprox(0.49);
                CheckThat(mod.ThermalLoad.Value).IsApprox(0.6901);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(63);
                CheckThat(mod.Range.Value).IsApprox(5040);
                CheckThat(mod.Falloff.Value).IsApprox(840);
                CheckThat(mod.RateOfFire.Value).IsApprox(4.203);
                CheckThat(mod.BurstInterval.Value).IsApprox(0.56);
                CheckThat(mod.BurstRateOfFire.Value).IsApprox(13);
                CheckThat(mod.BurstSize.Value).IsApprox(3);
                CheckThat(mod.BreachDamage.Value).IsApprox(3.124);
                CheckThat(mod.BreachMin.Value).IsApprox(40);
                CheckThat(mod.BreachMax.Value).IsApprox(80);
                CheckThat(mod.Jitter.Value).IsApprox(3);
                CheckThat(mod.KineticProportionDamage.Value).IsApprox(50);
                CheckThat(mod.ThermalProportionDamage.Value).IsApprox(50);
            }


            {
                // from ealhstan ship import, to edsy, to export

                // TEST Module Hpt_pulselaserburst_gimbal_medium in MediumHardpoint2 Blueprint: Focused Weapon
                //Level: 3
                //Quality: 0
                //Experimental Effect: Inertial Impact
                //   Damage: 50
                //   Jitter: 3
                //   KineticProportionDamage: 50
                //   ThermalProportionDamage: 50
                //Damage Per Second: 15.445, Original: 10.296, Mult: 50.0 % (Better)
                //Damage: 3.675, Original: 2.45, Mult: 50.0 % (Better)
                //Thermal Load: 0.69, Original: 0.67, Mult: 3.0 % (Better)
                //Armour Penetration: 58.324, Original: 35, Mult: 66.6 % (Better)
                //Maximum Range: 4690.2, Original: 3000, Mult: 56.3 % (Better)
                //Falloff Range: 781.7, Original: 500, Mult: 56.3 % (Better)
                //Jitter: 3, Original: 0, Mult: ∞% (Better)

                string t = @"{""event"":""Loadout"",""Ship"":""krait_mkii"",""ShipName"":"""",""ShipIdent"":""ST-13K"",""HullValue"":38743029,""ModulesValue"":115884722,""UnladenMass"":559,""CargoCapacity"":0,""MaxJumpRange"":21.837943,""FuelCapacity"":{""Main"":32,""Reserve"":0.63},""Rebuy"":7731387,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""MediumHardpoint2"",""Item"":""hpt_pulselaserburst_gimbal_medium"",""On"":true,""Priority"":0,""Value"":42559,""Engineering"":{""BlueprintName"":""Weapon_Focused"",""Level"":3,""Quality"":0.2713,""ExperimentalEffect"":""special_distortion_field"",""Modifiers"":[{""Label"":""DamagePerSecond"",""Value"":15.444504,""OriginalValue"":10.296336},{""Label"":""Damage"",""Value"":3.675,""OriginalValue"":2.45},{""Label"":""ThermalLoad"",""Value"":0.6901,""OriginalValue"":0.67},{""Label"":""ArmourPenetration"",""Value"":58.323997,""OriginalValue"":35},{""Label"":""MaximumRange"",""Value"":4690.200195,""OriginalValue"":3000},{""Label"":""FalloffRange"",""Value"":781.700012,""OriginalValue"":500},{""Label"":""Jitter"",""Value"":3,""OriginalValue"":0}]}},{""Slot"":""Armour"",""Item"":""krait_mkii_armour_reactive"",""On"":true,""Priority"":1,""Value"":94756030},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size7_class2"",""On"":true,""Priority"":1,""Value"":1264679},{""Slot"":""MainEngines"",""Item"":""int_engine_size6_class5"",""On"":true,""Priority"":0,""Value"":14197538},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size5_class5"",""On"":true,""Priority"":0,""Value"":4478716},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size4_class2"",""On"":true,""Priority"":0,""Value"":24895},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size7_class2"",""On"":true,""Priority"":0,""Value"":546542},{""Slot"":""Radar"",""Item"":""int_sensors_size6_class3"",""On"":true,""Priority"":0,""Value"":487987},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size5_class3"",""On"":true,""Priority"":1,""Value"":85776}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.MediumHardpoint2, true);
                CheckThat(mod.Mass.Value).IsApprox(4);
                CheckThat(mod.Integrity.Value).IsApprox(40);
                CheckThat(mod.PowerDraw.Value).IsApprox(1.04);
                CheckThat(mod.BootTime.Value).IsApprox(0);
                CheckThat(mod.DPS.Value).IsApprox(15.445);
                CheckThat(mod.Damage.Value).IsApprox(3.675);
                CheckThat(mod.DistributorDraw.Value).IsApprox(0.49);
                CheckThat(mod.ThermalLoad.Value).IsApprox(0.6901);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(58.32);
                CheckThat(mod.Range.Value).IsApprox(4690);
                CheckThat(mod.Falloff.Value).IsApprox(781.7);
                CheckThat(mod.RateOfFire.Value).IsApprox(4.203);
                CheckThat(mod.BurstInterval.Value).IsApprox(0.56);
                CheckThat(mod.BurstRateOfFire.Value).IsApprox(13);
                CheckThat(mod.BurstSize.Value).IsApprox(3);
                CheckThat(mod.BreachDamage.Value).IsApprox(3.124);
                CheckThat(mod.BreachMin.Value).IsApprox(40);
                CheckThat(mod.BreachMax.Value).IsApprox(80);
                CheckThat(mod.Jitter.Value).IsApprox(3);
                CheckThat(mod.KineticProportionDamage.Value).IsApprox(50);
                CheckThat(mod.ThermalProportionDamage.Value).IsApprox(50);
            }

            {
                // burst laser focused  Level 3
                string t = @"{""event"":""Loadout"",""Ship"":""krait_mkii"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":44152080,""ModulesValue"":1708430,""UnladenMass"":636,""CargoCapacity"":82,""MaxJumpRange"":8.985727,""FuelCapacity"":{""Main"":32,""Reserve"":0.63},""Rebuy"":2293025,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""MediumHardpoint1"",""Item"":""hpt_pulselaser_fixed_small"",""On"":true,""Priority"":0,""Value"":2200},{""Slot"":""MediumHardpoint2"",""Item"":""hpt_pulselaserburst_gimbal_medium"",""On"":true,""Priority"":0,""Value"":48500,""Engineering"":{""BlueprintName"":""Weapon_Focused"",""Level"":3,""Quality"":1,""Modifiers"":[{""Label"":""ThermalLoad"",""Value"":0.6901,""OriginalValue"":0.67},{""Label"":""ArmourPenetration"",""Value"":63,""OriginalValue"":35},{""Label"":""MaximumRange"",""Value"":5040,""OriginalValue"":3000},{""Label"":""FalloffRange"",""Value"":840,""OriginalValue"":500}]}},{""Slot"":""Armour"",""Item"":""krait_mkii_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size7_class1"",""On"":true,""Priority"":0,""Value"":480410},{""Slot"":""MainEngines"",""Item"":""int_engine_size6_class1"",""On"":true,""Priority"":0,""Value"":199750},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size5_class1"",""On"":true,""Priority"":0,""Value"":63010},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size4_class1"",""On"":true,""Priority"":0,""Value"":11350},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size7_class1"",""On"":true,""Priority"":0,""Value"":249140},{""Slot"":""Radar"",""Item"":""int_sensors_size6_class1"",""On"":true,""Priority"":0,""Value"":88980},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size5_class3"",""On"":true,""Priority"":0,""Value"":97750},{""Slot"":""Slot01_Size6"",""Item"":""int_shieldgenerator_size6_class1"",""On"":true,""Priority"":0,""Value"":199750},{""Slot"":""Slot02_Size6"",""Item"":""int_cargorack_size5_class1"",""On"":true,""Priority"":0,""Value"":111570},{""Slot"":""Slot03_Size5"",""Item"":""int_cargorack_size5_class1"",""On"":true,""Priority"":0,""Value"":111570},{""Slot"":""Slot04_Size5"",""Item"":""int_cargorack_size4_class1"",""On"":true,""Priority"":0,""Value"":34330},{""Slot"":""Slot08_Size2"",""Item"":""int_cargorack_size1_class1"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot09_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":9120}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.MediumHardpoint2, true);

                CheckThat(mod.Mass.Value).IsApprox(4);
                CheckThat(mod.Integrity.Value).IsApprox(40);
                CheckThat(mod.PowerDraw.Value).IsApprox(1.04);
                CheckThat(mod.BootTime.Value).IsApprox(0);
                CheckThat(mod.DPS.Value).IsApprox(10.296);
                CheckThat(mod.Damage.Value).IsApprox(2.45);
                CheckThat(mod.DistributorDraw.Value).IsApprox(0.49);
                CheckThat(mod.ThermalLoad.Value).IsApprox(0.6901);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(63);
                CheckThat(mod.Range.Value).IsApprox(5040);
                CheckThat(mod.Falloff.Value).IsApprox(840);
                CheckThat(mod.RateOfFire.Value).IsApprox(4.203);
                CheckThat(mod.BurstInterval.Value).IsApprox(0.56);
                CheckThat(mod.BurstRateOfFire.Value).IsApprox(13);
                CheckThat(mod.BurstSize.Value).IsApprox(3);
                CheckThat(mod.BreachDamage.Value).IsApprox(2.083);
                CheckThat(mod.BreachMin.Value).IsApprox(40);
                CheckThat(mod.BreachMax.Value).IsApprox(80);
                CheckThat(mod.Jitter.Value).IsApprox(0);
              //  CheckThat(mod.KineticProportionDamage.Value).IsApprox((0));
                CheckThat(mod.ThermalProportionDamage.Value).IsApprox(100);
            }



            {
                // thrusters clean tuning thermal spread
                string t = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,Eht00FBR00,,9p300A3w00AJYG03L_W0AZo00Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":411380,""UnladenMass"":41.825,""CargoCapacity"":4,""MaxJumpRange"":8.356005,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":20822,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_beamlaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":74650},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class5"",""On"":true,""Priority"":0,""Value"":160140},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class5"",""On"":true,""Priority"":0,""Value"":160220,""Engineering"":{""BlueprintName"":""Engine_Tuned"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_engine_cooled"",""Modifiers"":[{""Label"":""Mass"",""Value"":2.625,""OriginalValue"":2.5},{""Label"":""Integrity"",""Value"":47.04,""OriginalValue"":56},{""Label"":""PowerDraw"",""Value"":3.48,""OriginalValue"":3},{""Label"":""EngineOptimalMass"",""Value"":64.8,""OriginalValue"":72},{""Label"":""EngineOptPerformance"",""Value"":128,""OriginalValue"":100},{""Label"":""EngineHeatRate"",""Value"":0.468,""OriginalValue"":1.3}]}},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(t, ShipSlots.Slot.MainEngines);

                CheckThat(mod.Mass.Value).IsApprox(2.625);
                CheckThat(mod.Integrity.Value).IsApprox(47.04);
                CheckThat(mod.PowerDraw.Value).IsApprox(3.48);
                CheckThat(mod.MinMass.Value).IsApprox(32.4);
                CheckThat(mod.OptMass.Value).IsApprox(64.8);
                CheckThat(mod.MaxMass.Value).IsApprox(97.2);
                CheckThat(mod.EngineMinMultiplier.Value).IsApprox(122.8);
                CheckThat(mod.EngineOptMultiplier.Value).IsApprox(128);
                CheckThat(mod.EngineMaxMultiplier.Value).IsApprox(148.48);
                CheckThat(mod.ThermalLoad.Value).IsApprox(0.468);
            }
            {
                // thrusters strengthing drive distrubutors
                string t = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,FBR00FBR00,,9p300A4Y00AKAG07J_W0AZo00Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":26930,""UnladenMass"":43.525,""CargoCapacity"":4,""MaxJumpRange"":8.034074,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":1600,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980,""Engineering"":{""BlueprintName"":""Engine_Reinforced"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_engine_haulage"",""Modifiers"":[{""Label"":""Mass"",""Value"":3.125,""OriginalValue"":2.5},{""Label"":""Integrity"",""Value"":96.6,""OriginalValue"":46},{""Label"":""EngineOptimalMass"",""Value"":52.8,""OriginalValue"":48},{""Label"":""EngineHeatRate"",""Value"":0.65,""OriginalValue"":1.3}]}},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(t, ShipSlots.Slot.MainEngines);

                CheckThat(mod.Mass.Value).IsApprox(3.125);
                CheckThat(mod.Integrity.Value).IsApprox(96.6);
                CheckThat(mod.PowerDraw.Value).IsApprox(2);
                CheckThat(mod.MinMass.Value).IsApprox(26.4);
                CheckThat(mod.OptMass.Value).IsApprox(52.8);
                CheckThat(mod.MaxMass.Value).IsApprox(79.2);
                CheckThat(mod.EngineMinMultiplier.Value).IsApprox(83);
                CheckThat(mod.EngineOptMultiplier.Value).IsApprox(100);
                CheckThat(mod.EngineMaxMultiplier.Value).IsApprox(103);
                CheckThat(mod.ThermalLoad.Value).IsApprox(0.65);
            }

            {
                // enhanced performance thrusters dirty tuning drive distributors
                string t = @"{""event"":""Loadout"",""Ship"":""cobramkiii"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":186260,""ModulesValue"":5178500,""UnladenMass"":220,""CargoCapacity"":0,""MaxJumpRange"":12.113121,""FuelCapacity"":{""Main"":16,""Reserve"":0.49},""Rebuy"":268238,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""Armour"",""Item"":""cobramkiii_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size4_class1"",""On"":true,""Priority"":0,""Value"":17790},{""Slot"":""MainEngines"",""Item"":""int_engine_size3_class5_fast"",""On"":true,""Priority"":0,""Value"":5103950,""Engineering"":{""BlueprintName"":""Engine_Dirty"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_engine_haulage"",""Modifiers"":[{""Label"":""Integrity"",""Value"":46.75,""OriginalValue"":55},{""Label"":""PowerDraw"",""Value"":5.6,""OriginalValue"":5},{""Label"":""EngineOptimalMass"",""Value"":86.625,""OriginalValue"":90},{""Label"":""EngineOptPerformance"",""Value"":161,""OriginalValue"":115},{""Label"":""EngineHeatRate"",""Value"":2.08,""OriginalValue"":1.3}]}},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size4_class1"",""On"":true,""Priority"":0,""Value"":19880},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size3_class1"",""On"":true,""Priority"":0,""Value"":4050},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size3_class1"",""On"":true,""Priority"":0,""Value"":4050},{""Slot"":""Radar"",""Item"":""int_sensors_size3_class1"",""On"":true,""Priority"":0,""Value"":4050},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size4_class3"",""On"":true,""Priority"":0,""Value"":24730}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.MainEngines);

                CheckThat(mod.Mass.Value).IsApprox(5);
                CheckThat(mod.Integrity.Value).IsApprox(46.75);
                CheckThat(mod.PowerDraw.Value).IsApprox(5.6);
                CheckThat(mod.BootTime.Value).IsApprox(0);
                CheckThat(mod.MinMass.Value).IsApprox(67.38);
                CheckThat(mod.OptMass.Value).IsApprox(86.63);
                CheckThat(mod.MaxMass.Value).IsApprox(192.5);
                CheckThat(mod.EngineMinMultiplier.Value).IsApprox(126);
                CheckThat(mod.EngineOptMultiplier.Value).IsApprox(161);
                CheckThat(mod.EngineMaxMultiplier.Value).IsApprox(191.7);
                CheckThat(mod.MinimumSpeedModifier.Value).IsApprox(126);
                CheckThat(mod.OptimalSpeedModifier.Value).IsApprox(175);
                CheckThat(mod.MaximumSpeedModifier.Value).IsApprox(224);
                CheckThat(mod.MinimumAccelerationModifier.Value).IsApprox(126);
                CheckThat(mod.OptimalAccelerationModifier.Value).IsApprox(154);
                CheckThat(mod.MaximumAccelerationModifier.Value).IsApprox(168);
                CheckThat(mod.MinimumRotationModifier.Value).IsApprox(126);
                CheckThat(mod.OptimalRotationModifier.Value).IsApprox(154);
                CheckThat(mod.MaximumRotationModifier.Value).IsApprox(182);
                CheckThat(mod.ThermalLoad.Value).IsApprox(2.08);
            }


            {
                // power plant armoured monstered
                string t = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,Eht00FBR00,,9p300A3wG03I_W0AKA00AZo00Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":253140,""UnladenMass"":42.116,""CargoCapacity"":4,""MaxJumpRange"":8.29908,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":12910,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_beamlaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":74650},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class5"",""On"":true,""Priority"":0,""Value"":160140,""Engineering"":{""BlueprintName"":""PowerPlant_Armoured"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_powerplant_highcharge"",""Modifiers"":[{""Label"":""Mass"",""Value"":1.716,""OriginalValue"":1.3},{""Label"":""Integrity"",""Value"":123.2,""OriginalValue"":56},{""Label"":""PowerCapacity"",""Value"":11.2896,""OriginalValue"":9.6},{""Label"":""HeatEfficiency"",""Value"":0.352,""OriginalValue"":0.4}]}},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(t, ShipSlots.Slot.PowerPlant);

                CheckThat(mod.Mass.Value).IsApprox(1.7165);
                CheckThat(mod.Integrity.Value).IsApprox(123.2);
                CheckThat(mod.PowerGen.Value).IsApprox(11.29);
                CheckThat(mod.HeatEfficiency.Value).IsApprox(0.352);
            }

            {
                // power plant overcharge thermal spread
                string t = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,Eht00FBR00,,9p300A3wG07K_W0AKA00AZo00Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":253140,""UnladenMass"":41.7,""CargoCapacity"":4,""MaxJumpRange"":8.380697,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":12910,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_beamlaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":74650},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class5"",""On"":true,""Priority"":0,""Value"":160140,""Engineering"":{""BlueprintName"":""PowerPlant_Boosted"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_powerplant_cooled"",""Modifiers"":[{""Label"":""Integrity"",""Value"":42,""OriginalValue"":56},{""Label"":""PowerCapacity"",""Value"":13.44,""OriginalValue"":9.6},{""Label"":""HeatEfficiency"",""Value"":0.45,""OriginalValue"":0.4}]}},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(t, ShipSlots.Slot.PowerPlant);

                CheckThat(mod.Mass.Value).IsApprox(1.3);
                CheckThat(mod.Integrity.Value).IsApprox(42);
                CheckThat(mod.PowerGen.Value).IsApprox(13.44);
                CheckThat(mod.HeatEfficiency.Value).IsApprox(0.45);
            }



            {
                // chaff
                string t = @"{""event"":""Loadout"",""Ship"":""anaconda"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":142447820,""ModulesValue"":8668390,""UnladenMass"":1067.25,""CargoCapacity"":50,""MaxJumpRange"":9.632341,""FuelCapacity"":{""Main"":32,""Reserve"":1.07},""Rebuy"":7555810,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_fixed_small"",""On"":true,""Priority"":0,""Value"":2200},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_fixed_small"",""On"":true,""Priority"":0,""Value"":2200},{""Slot"":""TinyHardpoint1"",""Item"":""hpt_chafflauncher_tiny"",""On"":true,""Priority"":0,""Value"":8500,""Engineering"":{""BlueprintName"":""Misc_Reinforced"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""Mass"",""Value"":3.25,""OriginalValue"":1.3},{""Label"":""Integrity"",""Value"":80,""OriginalValue"":20}]}},{""Slot"":""Armour"",""Item"":""anaconda_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size8_class1"",""On"":true,""Priority"":0,""Value"":1441230},{""Slot"":""MainEngines"",""Item"":""int_engine_size7_class1"",""On"":true,""Priority"":0,""Value"":633200},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size6_class1"",""On"":true,""Priority"":0,""Value"":199750},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size5_class1"",""On"":true,""Priority"":0,""Value"":31780},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size8_class1"",""On"":true,""Priority"":0,""Value"":697580},{""Slot"":""Radar"",""Item"":""int_sensors_size8_class1"",""On"":true,""Priority"":0,""Value"":697580},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size5_class3"",""On"":true,""Priority"":0,""Value"":97750},{""Slot"":""Slot01_Size7"",""Item"":""int_refinery_size4_class5"",""On"":true,""Priority"":0,""Value"":4500850},{""Slot"":""Slot02_Size6"",""Item"":""int_cargorack_size5_class1"",""On"":true,""Priority"":0,""Value"":111570},{""Slot"":""Slot03_Size6"",""Item"":""int_shieldgenerator_size6_class1"",""On"":true,""Priority"":0,""Value"":199750},{""Slot"":""Slot05_Size5"",""Item"":""int_cargorack_size4_class1"",""On"":true,""Priority"":0,""Value"":34330},{""Slot"":""Slot13_Size2"",""Item"":""int_cargorack_size1_class1"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot14_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":9120}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.TinyHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(3.25);
                CheckThat(mod.Integrity.Value).IsApprox(80);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.2);
                CheckThat(mod.BootTime.Value).IsApprox(0);
                CheckThat(mod.DistributorDraw.Value).IsApprox(4);
                CheckThat(mod.ThermalLoad.Value).IsApprox(4);
                CheckThat(mod.RateOfFire.Value).IsApprox(1);
                CheckThat(mod.BurstInterval.Value).IsApprox(1);
                CheckThat(mod.Clip).Is( 1);
                CheckThat(mod.Ammo).Is( 10);
                CheckThat(mod.ReloadTime).Is( 10);
                CheckThat(mod.Time).Is( 20);
            }
            {
                // chaff ammo cap
                string t = @"{""event"":""Loadout"",""Ship"":""anaconda"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":142447820,""ModulesValue"":8668390,""UnladenMass"":1066.6,""CargoCapacity"":50,""MaxJumpRange"":9.638182,""FuelCapacity"":{""Main"":32,""Reserve"":1.07},""Rebuy"":7555810,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_fixed_small"",""On"":true,""Priority"":0,""Value"":2200},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_fixed_small"",""On"":true,""Priority"":0,""Value"":2200},{""Slot"":""TinyHardpoint1"",""Item"":""hpt_chafflauncher_tiny"",""On"":true,""Priority"":0,""Value"":8500,""Engineering"":{""BlueprintName"":""Misc_ChaffCapacity"",""Level"":1,""Quality"":1,""Modifiers"":[{""Label"":""Mass"",""Value"":2.6,""OriginalValue"":1.3},{""Label"":""AmmoMaximum"",""Value"":15,""OriginalValue"":10},{""Label"":""ReloadTime"",""Value"":11,""OriginalValue"":10}]}},{""Slot"":""Armour"",""Item"":""anaconda_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size8_class1"",""On"":true,""Priority"":0,""Value"":1441230},{""Slot"":""MainEngines"",""Item"":""int_engine_size7_class1"",""On"":true,""Priority"":0,""Value"":633200},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size6_class1"",""On"":true,""Priority"":0,""Value"":199750},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size5_class1"",""On"":true,""Priority"":0,""Value"":31780},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size8_class1"",""On"":true,""Priority"":0,""Value"":697580},{""Slot"":""Radar"",""Item"":""int_sensors_size8_class1"",""On"":true,""Priority"":0,""Value"":697580},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size5_class3"",""On"":true,""Priority"":0,""Value"":97750},{""Slot"":""Slot01_Size7"",""Item"":""int_refinery_size4_class5"",""On"":true,""Priority"":0,""Value"":4500850},{""Slot"":""Slot02_Size6"",""Item"":""int_cargorack_size5_class1"",""On"":true,""Priority"":0,""Value"":111570},{""Slot"":""Slot03_Size6"",""Item"":""int_shieldgenerator_size6_class1"",""On"":true,""Priority"":0,""Value"":199750},{""Slot"":""Slot05_Size5"",""Item"":""int_cargorack_size4_class1"",""On"":true,""Priority"":0,""Value"":34330},{""Slot"":""Slot13_Size2"",""Item"":""int_cargorack_size1_class1"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot14_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":9120}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.TinyHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(2.6);
                CheckThat(mod.Integrity.Value).IsApprox(20);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.2);
                CheckThat(mod.BootTime.Value).IsApprox(0);
                CheckThat(mod.DistributorDraw.Value).IsApprox(4);
                CheckThat(mod.ThermalLoad.Value).IsApprox(4);
                CheckThat(mod.RateOfFire.Value).IsApprox(1);
                CheckThat(mod.BurstInterval.Value).IsApprox(1);
                CheckThat(mod.Clip).Is( 1);
                CheckThat(mod.Ammo).Is( 15);
                CheckThat(mod.ReloadTime).Is( 11);
                CheckThat(mod.Time).Is( 20);
            }

            {
                // ecm reinforced
                string t = @"{""event"":""Loadout"",""Ship"":""anaconda"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":142447820,""ModulesValue"":8672390,""UnladenMass"":1067.25,""CargoCapacity"":50,""MaxJumpRange"":9.632341,""FuelCapacity"":{""Main"":32,""Reserve"":1.07},""Rebuy"":7556010,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_fixed_small"",""On"":true,""Priority"":0,""Value"":2200},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_fixed_small"",""On"":true,""Priority"":0,""Value"":2200},{""Slot"":""TinyHardpoint1"",""Item"":""hpt_electroniccountermeasure_tiny"",""On"":true,""Priority"":0,""Value"":12500,""Engineering"":{""BlueprintName"":""Misc_Reinforced"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""Mass"",""Value"":3.25,""OriginalValue"":1.3},{""Label"":""Integrity"",""Value"":80,""OriginalValue"":20}]}},{""Slot"":""Armour"",""Item"":""anaconda_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size8_class1"",""On"":true,""Priority"":0,""Value"":1441230},{""Slot"":""MainEngines"",""Item"":""int_engine_size7_class1"",""On"":true,""Priority"":0,""Value"":633200},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size6_class1"",""On"":true,""Priority"":0,""Value"":199750},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size5_class1"",""On"":true,""Priority"":0,""Value"":31780},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size8_class1"",""On"":true,""Priority"":0,""Value"":697580},{""Slot"":""Radar"",""Item"":""int_sensors_size8_class1"",""On"":true,""Priority"":0,""Value"":697580},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size5_class3"",""On"":true,""Priority"":0,""Value"":97750},{""Slot"":""Slot01_Size7"",""Item"":""int_refinery_size4_class5"",""On"":true,""Priority"":0,""Value"":4500850},{""Slot"":""Slot02_Size6"",""Item"":""int_cargorack_size5_class1"",""On"":true,""Priority"":0,""Value"":111570},{""Slot"":""Slot03_Size6"",""Item"":""int_shieldgenerator_size6_class1"",""On"":true,""Priority"":0,""Value"":199750},{""Slot"":""Slot05_Size5"",""Item"":""int_cargorack_size4_class1"",""On"":true,""Priority"":0,""Value"":34330},{""Slot"":""Slot13_Size2"",""Item"":""int_cargorack_size1_class1"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot14_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":9120}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.TinyHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(3.25);
                CheckThat(mod.Integrity.Value).IsApprox(80);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.2);
                CheckThat(mod.BootTime.Value).IsApprox(0);
                CheckThat(mod.Range.Value).IsApprox(3000);
                CheckThat(mod.Time).Is( 3);
                CheckThat(mod.ActivePower.Value).IsApprox(4);
                CheckThat(mod.ThermalLoad.Value).IsApprox(4);
                CheckThat(mod.ReloadTime.Value).IsApprox(10);
            }

            {
                //  heat sink reinforced
                string t = @"{""event"":""Loadout"",""Ship"":""anaconda"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":142447820,""ModulesValue"":8663390,""UnladenMass"":1067.25,""CargoCapacity"":50,""MaxJumpRange"":9.632341,""FuelCapacity"":{""Main"":32,""Reserve"":1.07},""Rebuy"":7555560,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_fixed_small"",""On"":true,""Priority"":0,""Value"":2200},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_fixed_small"",""On"":true,""Priority"":0,""Value"":2200},{""Slot"":""TinyHardpoint1"",""Item"":""hpt_heatsinklauncher_turret_tiny"",""On"":true,""Priority"":0,""Value"":3500,""Engineering"":{""BlueprintName"":""Misc_Reinforced"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""Mass"",""Value"":3.25,""OriginalValue"":1.3},{""Label"":""Integrity"",""Value"":180,""OriginalValue"":45}]}},{""Slot"":""Armour"",""Item"":""anaconda_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size8_class1"",""On"":true,""Priority"":0,""Value"":1441230},{""Slot"":""MainEngines"",""Item"":""int_engine_size7_class1"",""On"":true,""Priority"":0,""Value"":633200},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size6_class1"",""On"":true,""Priority"":0,""Value"":199750},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size5_class1"",""On"":true,""Priority"":0,""Value"":31780},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size8_class1"",""On"":true,""Priority"":0,""Value"":697580},{""Slot"":""Radar"",""Item"":""int_sensors_size8_class1"",""On"":true,""Priority"":0,""Value"":697580},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size5_class3"",""On"":true,""Priority"":0,""Value"":97750},{""Slot"":""Slot01_Size7"",""Item"":""int_refinery_size4_class5"",""On"":true,""Priority"":0,""Value"":4500850},{""Slot"":""Slot02_Size6"",""Item"":""int_cargorack_size5_class1"",""On"":true,""Priority"":0,""Value"":111570},{""Slot"":""Slot03_Size6"",""Item"":""int_shieldgenerator_size6_class1"",""On"":true,""Priority"":0,""Value"":199750},{""Slot"":""Slot05_Size5"",""Item"":""int_cargorack_size4_class1"",""On"":true,""Priority"":0,""Value"":34330},{""Slot"":""Slot13_Size2"",""Item"":""int_cargorack_size1_class1"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot14_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":9120}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.TinyHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(3.25);
                CheckThat(mod.Integrity.Value).IsApprox(180);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.2);
                CheckThat(mod.BootTime.Value).IsApprox(0);
                CheckThat(mod.DistributorDraw.Value).IsApprox(2);
                CheckThat(mod.RateOfFire.Value).IsApprox(0.2);
                CheckThat(mod.Clip).Is( 1);
                CheckThat(mod.Ammo).Is( 2);
                CheckThat(mod.ReloadTime.Value).IsApprox(10);
                CheckThat(mod.Time.Value).IsApprox(10);
                CheckThat(mod.ThermalDrain.Value).IsApprox(100);
            }
            {
                //  heat sink ammo cap
                string t = @"{""event"":""Loadout"",""Ship"":""anaconda"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":142447820,""ModulesValue"":8663390,""UnladenMass"":1066.6,""CargoCapacity"":50,""MaxJumpRange"":9.638182,""FuelCapacity"":{""Main"":32,""Reserve"":1.07},""Rebuy"":7555560,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_fixed_small"",""On"":true,""Priority"":0,""Value"":2200},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_fixed_small"",""On"":true,""Priority"":0,""Value"":2200},{""Slot"":""TinyHardpoint1"",""Item"":""hpt_heatsinklauncher_turret_tiny"",""On"":true,""Priority"":0,""Value"":3500,""Engineering"":{""BlueprintName"":""Misc_HeatSinkCapacity"",""Level"":1,""Quality"":1,""Modifiers"":[{""Label"":""Mass"",""Value"":2.6,""OriginalValue"":1.3},{""Label"":""AmmoMaximum"",""Value"":3,""OriginalValue"":2},{""Label"":""ReloadTime"",""Value"":15,""OriginalValue"":10}]}},{""Slot"":""Armour"",""Item"":""anaconda_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size8_class1"",""On"":true,""Priority"":0,""Value"":1441230},{""Slot"":""MainEngines"",""Item"":""int_engine_size7_class1"",""On"":true,""Priority"":0,""Value"":633200},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size6_class1"",""On"":true,""Priority"":0,""Value"":199750},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size5_class1"",""On"":true,""Priority"":0,""Value"":31780},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size8_class1"",""On"":true,""Priority"":0,""Value"":697580},{""Slot"":""Radar"",""Item"":""int_sensors_size8_class1"",""On"":true,""Priority"":0,""Value"":697580},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size5_class3"",""On"":true,""Priority"":0,""Value"":97750},{""Slot"":""Slot01_Size7"",""Item"":""int_refinery_size4_class5"",""On"":true,""Priority"":0,""Value"":4500850},{""Slot"":""Slot02_Size6"",""Item"":""int_cargorack_size5_class1"",""On"":true,""Priority"":0,""Value"":111570},{""Slot"":""Slot03_Size6"",""Item"":""int_shieldgenerator_size6_class1"",""On"":true,""Priority"":0,""Value"":199750},{""Slot"":""Slot05_Size5"",""Item"":""int_cargorack_size4_class1"",""On"":true,""Priority"":0,""Value"":34330},{""Slot"":""Slot13_Size2"",""Item"":""int_cargorack_size1_class1"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot14_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":9120}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.TinyHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(2.6);
                CheckThat(mod.Integrity.Value).IsApprox(45);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.2);
                CheckThat(mod.BootTime.Value).IsApprox(0);
                CheckThat(mod.DistributorDraw.Value).IsApprox(2);
                CheckThat(mod.RateOfFire.Value).IsApprox(0.2);
                CheckThat(mod.BurstInterval.Value).IsApprox(5);
                CheckThat(mod.Clip).Is( 1);
                CheckThat(mod.Ammo).Is( 3);
                CheckThat(mod.ReloadTime.Value).IsApprox(15);
                CheckThat(mod.Time.Value).IsApprox(10);
                CheckThat(mod.ThermalDrain.Value).IsApprox(100);
            }
            {
                // kill warrant reinforced
                string t = @"{""event"":""Loadout"",""Ship"":""anaconda"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":142447820,""ModulesValue"":4895970,""UnladenMass"":1023.25,""CargoCapacity"":0,""MaxJumpRange"":10.044399,""FuelCapacity"":{""Main"":32,""Reserve"":1.07},""Rebuy"":7367189,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""TinyHardpoint1"",""Item"":""hpt_crimescanner_size0_class5"",""On"":true,""Priority"":0,""Value"":1097100,""Engineering"":{""BlueprintName"":""Misc_Reinforced"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""Mass"",""Value"":3.25,""OriginalValue"":1.3},{""Label"":""Integrity"",""Value"":192,""OriginalValue"":48}]}},{""Slot"":""Armour"",""Item"":""anaconda_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size8_class1"",""On"":true,""Priority"":0,""Value"":1441230},{""Slot"":""MainEngines"",""Item"":""int_engine_size7_class1"",""On"":true,""Priority"":0,""Value"":633200},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size6_class1"",""On"":true,""Priority"":0,""Value"":199750},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size5_class1"",""On"":true,""Priority"":0,""Value"":31780},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size8_class1"",""On"":true,""Priority"":0,""Value"":697580},{""Slot"":""Radar"",""Item"":""int_sensors_size8_class1"",""On"":true,""Priority"":0,""Value"":697580},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size5_class3"",""On"":true,""Priority"":0,""Value"":97750}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.TinyHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(3.25);
                CheckThat(mod.Integrity.Value).IsApprox(192);
                CheckThat(mod.PowerDraw.Value).IsApprox(3.2);
                CheckThat(mod.BootTime.Value).IsApprox(2);
                CheckThat(mod.Range.Value).IsApprox(4000);
                CheckThat(mod.Angle.Value).IsApprox(15);
                CheckThat(mod.Time.Value).IsApprox(10);
            }
            {
                // manifest scanner shielded
                string t = @"{""event"":""Loadout"",""Ship"":""anaconda"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":142447820,""ModulesValue"":4895970,""UnladenMass"":1021.3,""CargoCapacity"":0,""MaxJumpRange"":10.063478,""FuelCapacity"":{""Main"":32,""Reserve"":1.07},""Rebuy"":7367189,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""TinyHardpoint1"",""Item"":""hpt_cargoscanner_size0_class5"",""On"":true,""Priority"":0,""Value"":1097100,""Engineering"":{""BlueprintName"":""Misc_Shielded"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""Integrity"",""Value"":192,""OriginalValue"":48},{""Label"":""PowerDraw"",""Value"":6.4,""OriginalValue"":3.2}]}},{""Slot"":""Armour"",""Item"":""anaconda_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size8_class1"",""On"":true,""Priority"":0,""Value"":1441230},{""Slot"":""MainEngines"",""Item"":""int_engine_size7_class1"",""On"":true,""Priority"":0,""Value"":633200},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size6_class1"",""On"":true,""Priority"":0,""Value"":199750},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size5_class1"",""On"":true,""Priority"":0,""Value"":31780},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size8_class1"",""On"":true,""Priority"":0,""Value"":697580},{""Slot"":""Radar"",""Item"":""int_sensors_size8_class1"",""On"":true,""Priority"":0,""Value"":697580},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size5_class3"",""On"":true,""Priority"":0,""Value"":97750}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.TinyHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(1.3);
                CheckThat(mod.Integrity.Value).IsApprox(192);
                CheckThat(mod.PowerDraw.Value).IsApprox(6.4);
                CheckThat(mod.BootTime.Value).IsApprox(3);
                CheckThat(mod.Range.Value).IsApprox(4000);
                CheckThat(mod.Angle.Value).IsApprox(15);
                CheckThat(mod.Time.Value).IsApprox(10);
            }
            {
                // point defence
                string t = @"{""event"":""Loadout"",""Ship"":""anaconda"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":142447820,""ModulesValue"":3817420,""UnladenMass"":1021,""CargoCapacity"":0,""MaxJumpRange"":10.06642,""FuelCapacity"":{""Main"":32,""Reserve"":1.07},""Rebuy"":7313262,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""TinyHardpoint1"",""Item"":""hpt_plasmapointdefence_turret_tiny"",""On"":true,""Priority"":0,""Value"":18550,""Engineering"":{""BlueprintName"":""Misc_PointDefenseCapacity"",""Level"":1,""Quality"":1,""Modifiers"":[{""Label"":""Mass"",""Value"":1,""OriginalValue"":0.5},{""Label"":""AmmoMaximum"",""Value"":15000,""OriginalValue"":10000},{""Label"":""ReloadTime"",""Value"":0.44,""OriginalValue"":0.4}]}},{""Slot"":""Armour"",""Item"":""anaconda_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size8_class1"",""On"":true,""Priority"":0,""Value"":1441230},{""Slot"":""MainEngines"",""Item"":""int_engine_size7_class1"",""On"":true,""Priority"":0,""Value"":633200},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size6_class1"",""On"":true,""Priority"":0,""Value"":199750},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size5_class1"",""On"":true,""Priority"":0,""Value"":31780},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size8_class1"",""On"":true,""Priority"":0,""Value"":697580},{""Slot"":""Radar"",""Item"":""int_sensors_size8_class1"",""On"":true,""Priority"":0,""Value"":697580},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size5_class3"",""On"":true,""Priority"":0,""Value"":97750}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.TinyHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(1);
                CheckThat(mod.Integrity.Value).IsApprox(30);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.2);
                CheckThat(mod.BootTime.Value).IsApprox(0);
                CheckThat(mod.DPS.Value).IsApprox(2);
                CheckThat(mod.Damage.Value).IsApprox(0.2);
                CheckThat(mod.ThermalLoad.Value).IsApprox(0.07);
                CheckThat(mod.Range.Value).IsApprox(2500);
                CheckThat(mod.Speed.Value).IsApprox(1000);
                CheckThat(mod.RateOfFire.Value).IsApprox(10);
                CheckThat(mod.BurstInterval.Value).IsApprox(0.2);
                CheckThat(mod.BurstRateOfFire.Value).IsApprox(15);
                CheckThat(mod.BurstSize.Value).IsApprox(4);
                CheckThat(mod.Clip.Value).Is( 12);
                CheckThat(mod.Ammo.Value).Is( 15000);
                CheckThat(mod.ReloadTime.Value).IsApprox(0.44);
                CheckThat(mod.Jitter.Value).IsApprox(0.75);
                CheckThat(mod.KineticProportionDamage.Value).IsApprox(100);
            }
            {
                // frame shift wake scanner
                string t = @"{""event"":""Loadout"",""Ship"":""anaconda"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":142447820,""ModulesValue"":4895970,""UnladenMass"":1022.6,""CargoCapacity"":0,""MaxJumpRange"":10.050751,""FuelCapacity"":{""Main"":32,""Reserve"":1.07},""Rebuy"":7367189,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""TinyHardpoint1"",""Item"":""hpt_cloudscanner_size0_class5"",""On"":true,""Priority"":0,""Value"":1097100,""Engineering"":{""BlueprintName"":""Sensor_WideAngle"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""Mass"",""Value"":2.6,""OriginalValue"":1.3},{""Label"":""MaxAngle"",""Value"":45,""OriginalValue"":15},{""Label"":""ScannerTimeToScan"",""Value"":15,""OriginalValue"":10}]}},{""Slot"":""Armour"",""Item"":""anaconda_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size8_class1"",""On"":true,""Priority"":0,""Value"":1441230},{""Slot"":""MainEngines"",""Item"":""int_engine_size7_class1"",""On"":true,""Priority"":0,""Value"":633200},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size6_class1"",""On"":true,""Priority"":0,""Value"":199750},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size5_class1"",""On"":true,""Priority"":0,""Value"":31780},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size8_class1"",""On"":true,""Priority"":0,""Value"":697580},{""Slot"":""Radar"",""Item"":""int_sensors_size8_class1"",""On"":true,""Priority"":0,""Value"":697580},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size5_class3"",""On"":true,""Priority"":0,""Value"":97750}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.TinyHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(2.6);
                CheckThat(mod.Integrity.Value).IsApprox(48);
                CheckThat(mod.PowerDraw.Value).IsApprox(3.2);
                CheckThat(mod.BootTime.Value).IsApprox(1);
                CheckThat(mod.Range.Value).IsApprox(4000);
                CheckThat(mod.Angle.Value).IsApprox(45);
                CheckThat(mod.Time.Value).IsApprox(15);
            }
            {
                // shield defence
                string t = @"{""event"":""Loadout"",""Ship"":""anaconda"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":142447820,""ModulesValue"":4079870,""UnladenMass"":1034,""CargoCapacity"":0,""MaxJumpRange"":9.940505,""FuelCapacity"":{""Main"":32,""Reserve"":1.07},""Rebuy"":7326384,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""TinyHardpoint1"",""Item"":""hpt_shieldbooster_size0_class5"",""On"":true,""Priority"":0,""Value"":281000,""Engineering"":{""BlueprintName"":""ShieldBooster_HeavyDuty"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_shieldbooster_chunky"",""Modifiers"":[{""Label"":""Mass"",""Value"":14,""OriginalValue"":3.5},{""Label"":""Integrity"",""Value"":55.2,""OriginalValue"":48},{""Label"":""PowerDraw"",""Value"":1.5,""OriginalValue"":1.2},{""Label"":""DefenceModifierShieldMultiplier"",""Value"":73.88,""OriginalValue"":20},{""Label"":""KineticResistance"",""Value"":-2,""OriginalValue"":0},{""Label"":""ThermicResistance"",""Value"":-2,""OriginalValue"":0},{""Label"":""ExplosiveResistance"",""Value"":-2,""OriginalValue"":0}]}},{""Slot"":""Armour"",""Item"":""anaconda_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size8_class1"",""On"":true,""Priority"":0,""Value"":1441230},{""Slot"":""MainEngines"",""Item"":""int_engine_size7_class1"",""On"":true,""Priority"":0,""Value"":633200},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size6_class1"",""On"":true,""Priority"":0,""Value"":199750},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size5_class1"",""On"":true,""Priority"":0,""Value"":31780},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size8_class1"",""On"":true,""Priority"":0,""Value"":697580},{""Slot"":""Radar"",""Item"":""int_sensors_size8_class1"",""On"":true,""Priority"":0,""Value"":697580},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size5_class3"",""On"":true,""Priority"":0,""Value"":97750}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.TinyHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(14);
                CheckThat(mod.Integrity.Value).IsApprox(55.2);
                CheckThat(mod.PowerDraw.Value).IsApprox(1.5);
                CheckThat(mod.BootTime.Value).IsApprox(0);
                CheckThat(mod.ShieldReinforcement.Value).IsApprox(73.88);
                CheckThat(mod.KineticResistance.Value).IsApprox(-2);
                CheckThat(mod.ThermalResistance.Value).IsApprox(-2);
                CheckThat(mod.KineticResistance.Value).IsApprox(-2);
            }

            {
                // SCB rapid charge flow control
                string t = @"{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":81500,""UnladenMass"":42.9,""CargoCapacity"":4,""MaxJumpRange"":8.149506,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":4328,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldcellbank_size2_class5"",""On"":true,""Priority"":0,""Value"":56550,""Engineering"":{""BlueprintName"":""ShieldCellBank_Rapid"",""Level"":4,""Quality"":1,""ExperimentalEffect"":""special_shieldcell_efficient"",""Modifiers"":[{""Label"":""PowerDraw"",""Value"":1.062,""OriginalValue"":1.18},{""Label"":""BootTime"",""Value"":31.25,""OriginalValue"":25},{""Label"":""ShieldBankSpinUp"",""Value"":3,""OriginalValue"":5},{""Label"":""ShieldBankDuration"",""Value"":1.14,""OriginalValue"":1.5},{""Label"":""ShieldBankReinforcement"",""Value"":38.4,""OriginalValue"":32}]}},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.Slot01_Size2);

                CheckThat(mod.Mass.Value).IsApprox(2.5);
                CheckThat(mod.Integrity.Value).IsApprox(61);
                CheckThat(mod.PowerDraw.Value).IsApprox(1.062);
                CheckThat(mod.BootTime.Value).IsApprox(31.25);
                CheckThat(mod.SCBSpinUp.Value).IsApprox(3);
                CheckThat(mod.SCBDuration.Value).IsApprox(1.14);
                CheckThat(mod.ShieldReinforcement.Value).IsApprox(38.4);
                CheckThat(mod.ThermalLoad.Value).IsApprox(240);
                CheckThat(mod.Clip).Is( 1);
                CheckThat(mod.Ammo).Is( 3);
            }
            {
                // SCB specialised boss cells
                string t = @"{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":81500,""UnladenMass"":42.9,""CargoCapacity"":4,""MaxJumpRange"":8.149506,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":4328,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldcellbank_size2_class5"",""On"":true,""Priority"":0,""Value"":56550,""Engineering"":{""BlueprintName"":""ShieldCellBank_Specialised"",""Level"":4,""Quality"":1,""ExperimentalEffect"":""special_shieldcell_oversized"",""Modifiers"":[{""Label"":""Integrity"",""Value"":48.8,""OriginalValue"":61},{""Label"":""PowerDraw"",""Value"":1.475,""OriginalValue"":1.18},{""Label"":""BootTime"",""Value"":17,""OriginalValue"":25},{""Label"":""ShieldBankSpinUp"",""Value"":6,""OriginalValue"":5},{""Label"":""ShieldBankReinforcement"",""Value"":36.96,""OriginalValue"":32},{""Label"":""ShieldBankHeat"",""Value"":182.4,""OriginalValue"":240}]}},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.Slot01_Size2);

                CheckThat(mod.Mass.Value).IsApprox(2.5);
                CheckThat(mod.Integrity.Value).IsApprox(48.8);
                CheckThat(mod.PowerDraw.Value).IsApprox(1.475);
                CheckThat(mod.BootTime.Value).IsApprox(17);
                CheckThat(mod.SCBSpinUp.Value).IsApprox(6);
                CheckThat(mod.SCBDuration.Value).IsApprox(1.5);
                CheckThat(mod.ShieldReinforcement.Value).IsApprox(36.96);
                CheckThat(mod.ThermalLoad.Value).IsApprox(182.4);
                CheckThat(mod.Clip).Is( 1);
                CheckThat(mod.Ammo).Is( 3);
            }

            {
                // detailed surface scanner expanded
                string t = @"{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":274950,""UnladenMass"":40.4,""CargoCapacity"":4,""MaxJumpRange"":8.646427,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":14001,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_detailedsurfacescanner_tiny"",""On"":true,""Priority"":0,""Value"":250000,""Engineering"":{""BlueprintName"":""Sensor_Expanded"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""DSS_PatchRadius"",""Value"":30,""OriginalValue"":20}]}},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.Slot01_Size2);

                CheckThat(mod.Integrity.Value).IsApprox(20);
                CheckThat(mod.Clip).Is( 3);
                CheckThat(mod.ProbeRadius.Value).IsApprox(30);
            }

            {
                // frame shift drive interdictos expanded arc capture
                string t = @"{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":2746550,""UnladenMass"":42.9,""CargoCapacity"":4,""MaxJumpRange"":8.149506,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":137581,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_fsdinterdictor_size2_class5"",""On"":true,""Priority"":0,""Value"":2721600,""Engineering"":{""BlueprintName"":""FSDinterdictor_Expanded"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""PowerDraw"",""Value"":0.585,""OriginalValue"":0.39},{""Label"":""FSDInterdictorRange"",""Value"":7,""OriginalValue"":10},{""Label"":""FSDInterdictorFacingLimit"",""Value"":110,""OriginalValue"":50}]}},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.Slot01_Size2);

                CheckThat(mod.Mass.Value).IsApprox(2.5);
                CheckThat(mod.Integrity.Value).IsApprox(61);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.585);
                CheckThat(mod.BootTime.Value).IsApprox(15);
                CheckThat(mod.TargetMaxTime).Is( 7);
                CheckThat(mod.Angle.Value).Is( 110);
            }

            {
                // frame shift drive interdictos expanded arc capture
                string t = @"{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":2746550,""UnladenMass"":43.65,""CargoCapacity"":4,""MaxJumpRange"":8.011378,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":137581,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_fsdinterdictor_size2_class5"",""On"":true,""Priority"":0,""Value"":2721600,""Engineering"":{""BlueprintName"":""FSDinterdictor_LongRange"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""Mass"",""Value"":3.25,""OriginalValue"":2.5},{""Label"":""PowerDraw"",""Value"":0.585,""OriginalValue"":0.39},{""Label"":""FSDInterdictorRange"",""Value"":16,""OriginalValue"":10},{""Label"":""FSDInterdictorFacingLimit"",""Value"":35,""OriginalValue"":50}]}},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.Slot01_Size2);

                CheckThat(mod.Mass.Value).IsApprox(3.25);
                CheckThat(mod.Integrity.Value).IsApprox(61);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.585);
                CheckThat(mod.BootTime.Value).IsApprox(15);
                CheckThat(mod.TargetMaxTime).Is( 16);
                CheckThat(mod.Angle.Value).Is( 35);
            }

            {
                // fuel scoop shielded
                string t = @"{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":309790,""UnladenMass"":40.4,""CargoCapacity"":4,""MaxJumpRange"":8.646427,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":15743,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_fuelscoop_size2_class5"",""On"":true,""Priority"":0,""Value"":284840,""Engineering"":{""BlueprintName"":""Misc_Shielded"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""Integrity"",""Value"":244,""OriginalValue"":61},{""Label"":""PowerDraw"",""Value"":0.78,""OriginalValue"":0.39}]}},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.Slot01_Size2);

                CheckThat(mod.Integrity.Value).IsApprox(244);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.78);
                CheckThat(mod.BootTime.Value).IsApprox(4);
                CheckThat(mod.RefillRate.Value.ApproxEquals(0.075));
            }

            {
                // fuel transfer limpet lightweight
                string t = @"{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":34550,""UnladenMass"":40.595,""CargoCapacity"":4,""MaxJumpRange"":8.605498,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":1981,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_dronecontrol_fueltransfer_size1_class5"",""On"":true,""Priority"":0,""Value"":9600,""Engineering"":{""BlueprintName"":""Misc_LightWeight"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""Mass"",""Value"":0.195,""OriginalValue"":1.3},{""Label"":""Integrity"",""Value"":28,""OriginalValue"":56}]}},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.Slot01_Size2);

                CheckThat(mod.Mass.Value).IsApprox(0.195);
                CheckThat(mod.Integrity.Value).IsApprox(28);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.28);
                CheckThat(mod.BootTime.Value).IsApprox(10);
                CheckThat(mod.Limpets).Is( 1);
                CheckThat(mod.Range).Is( 1400);
                CheckThat(mod.Time.Value).Is( 60);
                CheckThat(mod.Speed.Value).Is( 200);
                CheckThat(mod.FuelTransfer).Is( 1);
            }

            {
                // fuel transfer limpet reinforced
                string t = @"{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":34550,""UnladenMass"":43.65,""CargoCapacity"":4,""MaxJumpRange"":8.011378,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":1981,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_dronecontrol_fueltransfer_size1_class5"",""On"":true,""Priority"":0,""Value"":9600,""Engineering"":{""BlueprintName"":""Misc_Reinforced"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""Mass"",""Value"":3.25,""OriginalValue"":1.3},{""Label"":""Integrity"",""Value"":224,""OriginalValue"":56}]}},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.Slot01_Size2);

                CheckThat(mod.Mass.Value).IsApprox(3.25);
                CheckThat(mod.Integrity.Value).IsApprox(224);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.28);
                CheckThat(mod.BootTime.Value).IsApprox(10);
                CheckThat(mod.Limpets).Is( 1);
                CheckThat(mod.Range).Is( 1400);
                CheckThat(mod.Time.Value).Is( 60);
                CheckThat(mod.Speed.Value).Is( 200);
                CheckThat(mod.FuelTransfer).Is( 1);
            }
            {
                // prospector limpet reinforced
                string t = @"{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":34550,""UnladenMass"":43.65,""CargoCapacity"":4,""MaxJumpRange"":8.011378,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":1981,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_dronecontrol_prospector_size1_class5"",""On"":true,""Priority"":0,""Value"":9600,""Engineering"":{""BlueprintName"":""Misc_Reinforced"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""Mass"",""Value"":3.25,""OriginalValue"":1.3},{""Label"":""Integrity"",""Value"":224,""OriginalValue"":56}]}},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.Slot01_Size2);

                CheckThat(mod.Mass.Value).IsApprox(3.25);
                CheckThat(mod.Integrity.Value).IsApprox(224);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.28);
                CheckThat(mod.BootTime.Value).IsApprox(4);
                CheckThat(mod.Limpets).Is( 1);
                CheckThat(mod.Range).Is( 7000);
                CheckThat(mod.Speed.Value).Is( 200);
            }
            {
                // refinery shielded
                string t = @"{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":1045550,""UnladenMass"":40.4,""CargoCapacity"":4,""MaxJumpRange"":8.646427,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":52531,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_refinery_size2_class5"",""On"":true,""Priority"":0,""Value"":1020600,""Engineering"":{""BlueprintName"":""Misc_Shielded"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""Integrity"",""Value"":244,""OriginalValue"":61},{""Label"":""PowerDraw"",""Value"":0.78,""OriginalValue"":0.39}]}},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.Slot01_Size2);

                CheckThat(mod.Integrity.Value).IsApprox(244);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.78);
                CheckThat(mod.BootTime.Value).IsApprox(10);
                CheckThat(mod.Capacity).Is( 6);
            }

            {
                // hatch breaker shielded
                string t = @"{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":34550,""UnladenMass"":41.7,""CargoCapacity"":4,""MaxJumpRange"":8.380697,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":1981,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_dronecontrol_resourcesiphon_size1_class5"",""On"":true,""Priority"":0,""Value"":9600,""Engineering"":{""BlueprintName"":""Misc_Shielded"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""Integrity"",""Value"":192,""OriginalValue"":48},{""Label"":""PowerDraw"",""Value"":0.56,""OriginalValue"":0.28}]}},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.Slot01_Size2);

                CheckThat(mod.Mass.Value).IsApprox(1.3);
                CheckThat(mod.Integrity.Value).IsApprox(192);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.56);
                CheckThat(mod.BootTime.Value).IsApprox(3);
                CheckThat(mod.Limpets).Is( 1);
                CheckThat(mod.Range).Is( 3600);
                CheckThat(mod.TargetRange).Is( 3500);
                CheckThat(mod.Time.Value).Is( 120);
                CheckThat(mod.Speed.Value).Is( 500);
                CheckThat(mod.HackTime.Value).Is( 10);
                CheckThat(mod.MinCargo).Is( 5);
                CheckThat(mod.MaxCargo).Is( 10);
            }

            {
                // shields enhanced low power multi weave
                string t = @"{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":185170,""UnladenMass"":41.845,""CargoCapacity"":4,""MaxJumpRange"":18.234246,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":9512,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class5"",""On"":true,""Priority"":0,""Value"":160220},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520,""Engineering"":{""BlueprintName"":""PowerDistributor_Shielded"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_powerdistributor_toughened"",""Modifiers"":[{""Label"":""Mass"",""Value"":1.495,""OriginalValue"":1.3},{""Label"":""Integrity"",""Value"":124.2,""OriginalValue"":36},{""Label"":""PowerDraw"",""Value"":0.224,""OriginalValue"":0.32}]}},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520,""Engineering"":{""BlueprintName"":""Sensor_WideAngle"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""PowerDraw"",""Value"":0.24,""OriginalValue"":0.16},{""Label"":""SensorTargetScanAngle"",""Value"":90,""OriginalValue"":30},{""Label"":""Range"",""Value"":3200,""OriginalValue"":4000}]}},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980,""Engineering"":{""BlueprintName"":""ShieldGenerator_Optimised"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_shield_resistive"",""Modifiers"":[{""Label"":""Mass"",""Value"":1.25,""OriginalValue"":2.5},{""Label"":""Integrity"",""Value"":30.75,""OriginalValue"":41},{""Label"":""PowerDraw"",""Value"":0.594,""OriginalValue"":0.9},{""Label"":""ShieldGenOptimalMass"",""Value"":51.7,""OriginalValue"":55},{""Label"":""ShieldGenStrength"",""Value"":92,""OriginalValue"":80},{""Label"":""EnergyPerRegen"",""Value"":0.75,""OriginalValue"":0.6},{""Label"":""KineticResistance"",""Value"":41.8,""OriginalValue"":40},{""Label"":""ThermicResistance"",""Value"":-16.4,""OriginalValue"":-20},{""Label"":""ExplosiveResistance"",""Value"":51.5,""OriginalValue"":50}]}},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.Slot01_Size2);

                CheckThat(mod.Mass.Value).IsApprox(1.25);
                CheckThat(mod.Integrity.Value).IsApprox(30.75);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.594);
                CheckThat(mod.BootTime.Value).IsApprox(1);
                CheckThat(mod.MinMass.Value).IsApprox(26.32);
                CheckThat(mod.OptMass.Value).IsApprox(51.7);
                CheckThat(mod.MaxMass.Value).IsApprox(138);
                CheckThat(mod.MinStrength.Value).IsApprox(34.5);
                CheckThat(mod.OptStrength.Value).IsApprox(92);
                CheckThat(mod.MaxStrength.Value).IsApprox(149.5);
                CheckThat(mod.RegenRate.Value).IsApprox(1);
                CheckThat(mod.BrokenRegenRate.Value).IsApprox(1.6);
                CheckThat(mod.MWPerUnit.Value).IsApprox(0.75);
                CheckThat(mod.KineticResistance.Value).IsApprox(41.8);
                CheckThat(mod.ThermalResistance.Value).IsApprox(-16.4);
                CheckThat(mod.ExplosiveResistance.Value).IsApprox(51.5);
                CheckThat(mod.AXResistance.Value).IsApprox(95);
            }

            {
                // shields enhanced low power force block
                string t = @"{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":185170,""UnladenMass"":41.845,""CargoCapacity"":4,""MaxJumpRange"":18.234246,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":9512,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class5"",""On"":true,""Priority"":0,""Value"":160220},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520,""Engineering"":{""BlueprintName"":""PowerDistributor_Shielded"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_powerdistributor_toughened"",""Modifiers"":[{""Label"":""Mass"",""Value"":1.495,""OriginalValue"":1.3},{""Label"":""Integrity"",""Value"":124.2,""OriginalValue"":36},{""Label"":""PowerDraw"",""Value"":0.224,""OriginalValue"":0.32}]}},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520,""Engineering"":{""BlueprintName"":""Sensor_WideAngle"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""PowerDraw"",""Value"":0.24,""OriginalValue"":0.16},{""Label"":""SensorTargetScanAngle"",""Value"":90,""OriginalValue"":30},{""Label"":""Range"",""Value"":3200,""OriginalValue"":4000}]}},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980,""Engineering"":{""BlueprintName"":""ShieldGenerator_Optimised"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_shield_kinetic"",""Modifiers"":[{""Label"":""Mass"",""Value"":1.25,""OriginalValue"":2.5},{""Label"":""Integrity"",""Value"":30.75,""OriginalValue"":41},{""Label"":""PowerDraw"",""Value"":0.54,""OriginalValue"":0.9},{""Label"":""ShieldGenOptimalMass"",""Value"":51.7,""OriginalValue"":55},{""Label"":""ShieldGenStrength"",""Value"":89.24,""OriginalValue"":80},{""Label"":""KineticResistance"",""Value"":44.8,""OriginalValue"":40}]}},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.Slot01_Size2);

                CheckThat(mod.Mass.Value).IsApprox(1.25);
                CheckThat(mod.Integrity.Value).IsApprox(30.75);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.54);
                CheckThat(mod.BootTime.Value).IsApprox(1);
                CheckThat(mod.MinMass.Value).IsApprox(26.32);
                CheckThat(mod.OptMass.Value).IsApprox(51.7);
                CheckThat(mod.MaxMass.Value).IsApprox(138);
                CheckThat(mod.MinStrength.Value).IsApprox(33.46);
                CheckThat(mod.OptStrength.Value).IsApprox(89.24);
                CheckThat(mod.MaxStrength.Value).IsApprox(145.02);
                CheckThat(mod.RegenRate.Value).IsApprox(1);
                CheckThat(mod.BrokenRegenRate.Value).IsApprox(1.6);
                CheckThat(mod.MWPerUnit.Value).IsApprox(0.6);
                CheckThat(mod.KineticResistance.Value).IsApprox(44.8);
                CheckThat(mod.ThermalResistance.Value).IsApprox(-20);
                CheckThat(mod.ExplosiveResistance.Value).IsApprox(50);
                CheckThat(mod.AXResistance.Value).IsApprox(95);
            }

            {
                // shields  enhanced low power thermo block
                string t = @"{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":185170,""UnladenMass"":41.845,""CargoCapacity"":4,""MaxJumpRange"":18.234246,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":9512,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class5"",""On"":true,""Priority"":0,""Value"":160220},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520,""Engineering"":{""BlueprintName"":""PowerDistributor_Shielded"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_powerdistributor_toughened"",""Modifiers"":[{""Label"":""Mass"",""Value"":1.495,""OriginalValue"":1.3},{""Label"":""Integrity"",""Value"":124.2,""OriginalValue"":36},{""Label"":""PowerDraw"",""Value"":0.224,""OriginalValue"":0.32}]}},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520,""Engineering"":{""BlueprintName"":""Sensor_WideAngle"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""PowerDraw"",""Value"":0.24,""OriginalValue"":0.16},{""Label"":""SensorTargetScanAngle"",""Value"":90,""OriginalValue"":30},{""Label"":""Range"",""Value"":3200,""OriginalValue"":4000}]}},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980,""Engineering"":{""BlueprintName"":""ShieldGenerator_Optimised"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_shield_thermic"",""Modifiers"":[{""Label"":""Mass"",""Value"":1.25,""OriginalValue"":2.5},{""Label"":""Integrity"",""Value"":30.75,""OriginalValue"":41},{""Label"":""PowerDraw"",""Value"":0.54,""OriginalValue"":0.9},{""Label"":""ShieldGenOptimalMass"",""Value"":51.7,""OriginalValue"":55},{""Label"":""ShieldGenStrength"",""Value"":89.24,""OriginalValue"":80},{""Label"":""ThermicResistance"",""Value"":-10.4,""OriginalValue"":-20}]}},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.Slot01_Size2);

                CheckThat(mod.Mass.Value).IsApprox(1.25);
                CheckThat(mod.Integrity.Value).IsApprox(30.75);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.54);
                CheckThat(mod.BootTime.Value).IsApprox(1);
                CheckThat(mod.MinMass.Value).IsApprox(26.32);
                CheckThat(mod.OptMass.Value).IsApprox(51.7);
                CheckThat(mod.MaxMass.Value).IsApprox(138);
                CheckThat(mod.MinStrength.Value).IsApprox(33.46);
                CheckThat(mod.OptStrength.Value).IsApprox(89.24);
                CheckThat(mod.MaxStrength.Value).IsApprox(145.02);
                CheckThat(mod.RegenRate.Value).IsApprox(1);
                CheckThat(mod.BrokenRegenRate.Value).IsApprox(1.6);
                CheckThat(mod.MWPerUnit.Value).IsApprox(0.6);
                CheckThat(mod.KineticResistance.Value).IsApprox(40);
                CheckThat(mod.ThermalResistance.Value).IsApprox(-10.4);
                CheckThat(mod.ExplosiveResistance.Value).IsApprox(50);
                CheckThat(mod.AXResistance.Value).IsApprox(95);
            }


            {
                // shields kinetic multi weave
                string t = @"{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":185170,""UnladenMass"":43.095,""CargoCapacity"":4,""MaxJumpRange"":17.716169,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":9512,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class5"",""On"":true,""Priority"":0,""Value"":160220},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520,""Engineering"":{""BlueprintName"":""PowerDistributor_Shielded"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_powerdistributor_toughened"",""Modifiers"":[{""Label"":""Mass"",""Value"":1.495,""OriginalValue"":1.3},{""Label"":""Integrity"",""Value"":124.2,""OriginalValue"":36},{""Label"":""PowerDraw"",""Value"":0.224,""OriginalValue"":0.32}]}},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520,""Engineering"":{""BlueprintName"":""Sensor_WideAngle"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""PowerDraw"",""Value"":0.24,""OriginalValue"":0.16},{""Label"":""SensorTargetScanAngle"",""Value"":90,""OriginalValue"":30},{""Label"":""Range"",""Value"":3200,""OriginalValue"":4000}]}},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980,""Engineering"":{""BlueprintName"":""ShieldGenerator_Kinetic"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_shield_resistive"",""Modifiers"":[{""Label"":""Integrity"",""Value"":57.4,""OriginalValue"":41},{""Label"":""PowerDraw"",""Value"":0.99,""OriginalValue"":0.9},{""Label"":""EnergyPerRegen"",""Value"":0.75,""OriginalValue"":0.6},{""Label"":""KineticResistance"",""Value"":70.9,""OriginalValue"":40},{""Label"":""ThermicResistance"",""Value"":-33.86,""OriginalValue"":-20},{""Label"":""ExplosiveResistance"",""Value"":51.5,""OriginalValue"":50}]}},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.Slot01_Size2);

                CheckThat(mod.Mass.Value).IsApprox(2.5);
                CheckThat(mod.Integrity.Value).IsApprox(57.4);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.99);
                CheckThat(mod.BootTime.Value).IsApprox(1);
                CheckThat(mod.MinMass.Value).IsApprox(28);
                CheckThat(mod.OptMass.Value).IsApprox(55);
                CheckThat(mod.MaxMass.Value).IsApprox(138);
                CheckThat(mod.MinStrength.Value).IsApprox(30);
                CheckThat(mod.OptStrength.Value).IsApprox(80);
                CheckThat(mod.MaxStrength.Value).IsApprox(130);
                CheckThat(mod.RegenRate.Value).IsApprox(1);
                CheckThat(mod.BrokenRegenRate.Value).IsApprox(1.6);
                CheckThat(mod.MWPerUnit.Value).IsApprox(0.75);
                CheckThat(mod.KineticResistance.Value).IsApprox(70.9);
                CheckThat(mod.ThermalResistance.Value).IsApprox(-33.86);
                CheckThat(mod.ExplosiveResistance.Value).IsApprox(51.5);
                CheckThat(mod.AXResistance.Value).IsApprox(95);
            }
            {
                // shields reinforced lo draw
                string t = @"{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":185170,""UnladenMass"":43.095,""CargoCapacity"":4,""MaxJumpRange"":17.716169,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":9512,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class5"",""On"":true,""Priority"":0,""Value"":160220},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520,""Engineering"":{""BlueprintName"":""PowerDistributor_Shielded"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_powerdistributor_toughened"",""Modifiers"":[{""Label"":""Mass"",""Value"":1.495,""OriginalValue"":1.3},{""Label"":""Integrity"",""Value"":124.2,""OriginalValue"":36},{""Label"":""PowerDraw"",""Value"":0.224,""OriginalValue"":0.32}]}},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520,""Engineering"":{""BlueprintName"":""Sensor_WideAngle"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""PowerDraw"",""Value"":0.24,""OriginalValue"":0.16},{""Label"":""SensorTargetScanAngle"",""Value"":90,""OriginalValue"":30},{""Label"":""Range"",""Value"":3200,""OriginalValue"":4000}]}},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980,""Engineering"":{""BlueprintName"":""ShieldGenerator_Reinforced"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_shield_efficient"",""Modifiers"":[{""Label"":""PowerDraw"",""Value"":0.72,""OriginalValue"":0.9},{""Label"":""ShieldGenStrength"",""Value"":108.192,""OriginalValue"":80},{""Label"":""BrokenRegenRate"",""Value"":1.44,""OriginalValue"":1.6},{""Label"":""EnergyPerRegen"",""Value"":0.5376,""OriginalValue"":0.6},{""Label"":""KineticResistance"",""Value"":49.399,""OriginalValue"":40},{""Label"":""ThermicResistance"",""Value"":-1.202,""OriginalValue"":-20},{""Label"":""ExplosiveResistance"",""Value"":57.8325,""OriginalValue"":50}]}},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.Slot01_Size2);

                CheckThat(mod.Mass.Value).IsApprox(2.5);
                CheckThat(mod.Integrity.Value).IsApprox(41);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.72);
                CheckThat(mod.BootTime.Value).IsApprox(1);
                CheckThat(mod.MinMass.Value).IsApprox(28);
                CheckThat(mod.OptMass.Value).IsApprox(55);
                CheckThat(mod.MaxMass.Value).IsApprox(138);
                CheckThat(mod.MinStrength.Value).IsApprox(40.57);
                CheckThat(mod.OptStrength.Value).IsApprox(108.19);
                CheckThat(mod.MaxStrength.Value).IsApprox(175.81);
                CheckThat(mod.RegenRate.Value).IsApprox(1);
                CheckThat(mod.BrokenRegenRate.Value).IsApprox(1.44);
                CheckThat(mod.MWPerUnit.Value).IsApprox(0.5376);
                CheckThat(mod.KineticResistance.Value).IsApprox(49.4);
                CheckThat(mod.ThermalResistance.Value).IsApprox(-1.202);
                CheckThat(mod.ExplosiveResistance.Value).IsApprox(57.83);
                CheckThat(mod.AXResistance.Value).IsApprox(95);
            }
            {
                // shields thermal fast charge
                string t = @"{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":185170,""UnladenMass"":43.095,""CargoCapacity"":4,""MaxJumpRange"":17.716169,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":9512,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class5"",""On"":true,""Priority"":0,""Value"":160220},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520,""Engineering"":{""BlueprintName"":""PowerDistributor_Shielded"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_powerdistributor_toughened"",""Modifiers"":[{""Label"":""Mass"",""Value"":1.495,""OriginalValue"":1.3},{""Label"":""Integrity"",""Value"":124.2,""OriginalValue"":36},{""Label"":""PowerDraw"",""Value"":0.224,""OriginalValue"":0.32}]}},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520,""Engineering"":{""BlueprintName"":""Sensor_WideAngle"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""PowerDraw"",""Value"":0.24,""OriginalValue"":0.16},{""Label"":""SensorTargetScanAngle"",""Value"":90,""OriginalValue"":30},{""Label"":""Range"",""Value"":3200,""OriginalValue"":4000}]}},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980,""Engineering"":{""BlueprintName"":""ShieldGenerator_Thermic"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_shield_regenerative"",""Modifiers"":[{""Label"":""Integrity"",""Value"":57.4,""OriginalValue"":41},{""Label"":""RegenRate"",""Value"":1.15,""OriginalValue"":1},{""Label"":""BrokenRegenRate"",""Value"":1.84,""OriginalValue"":1.6},{""Label"":""KineticResistance"",""Value"":26.92,""OriginalValue"":40},{""Label"":""ThermicResistance"",""Value"":39.1,""OriginalValue"":-20},{""Label"":""ExplosiveResistance"",""Value"":49.25,""OriginalValue"":50}]}},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.Slot01_Size2);

                CheckThat(mod.Mass.Value).IsApprox(2.5);
                CheckThat(mod.Integrity.Value).IsApprox(57.4);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.9);
                CheckThat(mod.BootTime.Value).IsApprox(1);
                CheckThat(mod.MinMass.Value).IsApprox(28);
                CheckThat(mod.OptMass.Value).IsApprox(55);
                CheckThat(mod.MaxMass.Value).IsApprox(138);
                CheckThat(mod.MinStrength.Value).IsApprox(30);
                CheckThat(mod.OptStrength.Value).IsApprox(80);
                CheckThat(mod.MaxStrength.Value).IsApprox(130);
                CheckThat(mod.RegenRate.Value).IsApprox(1.15);
                CheckThat(mod.BrokenRegenRate.Value).IsApprox(1.84);
                CheckThat(mod.MWPerUnit.Value).IsApprox(0.6);
                CheckThat(mod.KineticResistance.Value).IsApprox(26.92);
                CheckThat(mod.ThermalResistance.Value).IsApprox(39.1);
                CheckThat(mod.ExplosiveResistance.Value).IsApprox(49.25);
                CheckThat(mod.AXResistance.Value).IsApprox(95);
            }

            {
                // afm shielded
                string t = @"{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":1641190,""UnladenMass"":40.595,""CargoCapacity"":4,""MaxJumpRange"":18.783537,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":82313,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class5"",""On"":true,""Priority"":0,""Value"":160220},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520,""Engineering"":{""BlueprintName"":""PowerDistributor_Shielded"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_powerdistributor_toughened"",""Modifiers"":[{""Label"":""Mass"",""Value"":1.495,""OriginalValue"":1.3},{""Label"":""Integrity"",""Value"":124.2,""OriginalValue"":36},{""Label"":""PowerDraw"",""Value"":0.224,""OriginalValue"":0.32}]}},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520,""Engineering"":{""BlueprintName"":""Sensor_WideAngle"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""PowerDraw"",""Value"":0.24,""OriginalValue"":0.16},{""Label"":""SensorTargetScanAngle"",""Value"":90,""OriginalValue"":30},{""Label"":""Range"",""Value"":3200,""OriginalValue"":4000}]}},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_repairer_size2_class5"",""On"":true,""Priority"":0,""Value"":1458000,""Engineering"":{""BlueprintName"":""Misc_Shielded"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""Integrity"",""Value"":236,""OriginalValue"":59},{""Label"":""PowerDraw"",""Value"":3.16,""OriginalValue"":1.58}]}},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.Slot01_Size2);

                CheckThat(mod.Integrity.Value).IsApprox(236);
                CheckThat(mod.PowerDraw.Value).IsApprox(3.16);
                CheckThat(mod.BootTime.Value).IsApprox(9);
                CheckThat(mod.Ammo.Value).Is( 2500);
                CheckThat(mod.RateOfRepairConsumption.Value).IsApprox(10);
                CheckThat(mod.RepairCostPerMat.Value).IsApprox(0.028);
            }
            {
                // collection climpet lightweight
                string t = @"{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":187990,""UnladenMass"":40.7,""CargoCapacity"":4,""MaxJumpRange"":18.736127,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":9653,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class5"",""On"":true,""Priority"":0,""Value"":160220},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_dronecontrol_collection_size1_class4"",""On"":true,""Priority"":0,""Value"":4800,""Engineering"":{""BlueprintName"":""Misc_LightWeight"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""Mass"",""Value"":0.3,""OriginalValue"":2},{""Label"":""Integrity"",""Value"":24,""OriginalValue"":48}]}},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.Slot01_Size2);

                CheckThat(mod.Mass.Value).IsApprox(0.3);
                CheckThat(mod.Integrity.Value).IsApprox(24);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.28);
                CheckThat(mod.BootTime.Value).IsApprox(6);
                CheckThat(mod.Limpets).Is( 1);
                CheckThat(mod.Range).Is( 1400);
                CheckThat(mod.Time.Value).Is( 420);
                CheckThat(mod.Speed.Value).Is( 200);
                CheckThat(mod.MultiTargetSpeed).Is( 60);
            }
            {
                // collection climpet reinforced
                string t = @"{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":187990,""UnladenMass"":45.4,""CargoCapacity"":4,""MaxJumpRange"":16.834187,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":9653,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class5"",""On"":true,""Priority"":0,""Value"":160220},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_dronecontrol_collection_size1_class4"",""On"":true,""Priority"":0,""Value"":4800,""Engineering"":{""BlueprintName"":""Misc_Reinforced"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""Mass"",""Value"":5,""OriginalValue"":2},{""Label"":""Integrity"",""Value"":192,""OriginalValue"":48}]}},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.Slot01_Size2);

                CheckThat(mod.Mass.Value).IsApprox(5);
                CheckThat(mod.Integrity.Value).IsApprox(192);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.28);
                CheckThat(mod.BootTime.Value).IsApprox(6);
                CheckThat(mod.Limpets).Is( 1);
                CheckThat(mod.Range).Is( 1400);
                CheckThat(mod.Time.Value).Is( 420);
                CheckThat(mod.Speed.Value).Is( 200);
                CheckThat(mod.MultiTargetSpeed).Is( 60);
            }


            {
                // rail gun nothing
                string t = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,KYi00FBR00,,9p300A4Y00AKA00AZo00Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":71930,""UnladenMass"":42.9,""CargoCapacity"":4,""MaxJumpRange"":8.149506,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":3850,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_railgun_fixed_small"",""On"":true,""Priority"":0,""Value"":51600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(t, ShipSlots.Slot.SmallHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(2);
                CheckThat(mod.Integrity.Value).IsApprox(40);
                CheckThat(mod.PowerDraw.Value).IsApprox(1.15);
                CheckThat(mod.DPS.Value).IsApprox(14.319);
                CheckThat(mod.Damage.Value).IsApprox(23.34);
                CheckThat(mod.DistributorDraw.Value).IsApprox(2.69);
                CheckThat(mod.ThermalLoad.Value).IsApprox(12);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(100);
                CheckThat(mod.Range.Value).IsApprox(3000);
                CheckThat(mod.Falloff.Value).IsApprox(1000);
                CheckThat(mod.RateOfFire.Value).IsApprox(0.6135);
                CheckThat(mod.BurstInterval.Value).IsApprox(0.63);
                CheckThat(mod.Clip.Value).Is( 1);
                CheckThat(mod.Ammo.Value).Is( 80);
                CheckThat(mod.ReloadTime.Value).IsApprox(1);
                CheckThat(mod.BreachDamage.Value).IsApprox(22.173);
            }

            {
                // railgun high cap plasma slug
                string t = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,KYiG03M_W0FBR00,,9p300A4Y00AKA00AZo00Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":71930,""UnladenMass"":44.1,""CargoCapacity"":4,""MaxJumpRange"":7.930727,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":3850,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_railgun_fixed_small"",""On"":true,""Priority"":0,""Value"":51600,""Engineering"":{""BlueprintName"":""Weapon_HighCapacity"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_plasma_slug_cooled"",""Modifiers"":[{""Label"":""Mass"",""Value"":3.2,""OriginalValue"":2},{""Label"":""PowerDraw"",""Value"":1.38,""OriginalValue"":1.15},{""Label"":""DamagePerSecond"",""Value"":13.405233,""OriginalValue"":14.319018},{""Label"":""Damage"",""Value"":21.006,""OriginalValue"":23.34},{""Label"":""ThermalLoad"",""Value"":7.2,""OriginalValue"":12},{""Label"":""RateOfFire"",""Value"":0.638162,""OriginalValue"":0.613497},{""Label"":""AmmoClipSize"",""Value"":2,""OriginalValue"":1},{""Label"":""AmmoMaximum"",""Value"":0,""OriginalValue"":80}]}},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(t, ShipSlots.Slot.SmallHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(3.2);
                CheckThat(mod.Integrity.Value).IsApprox(40);
                CheckThat(mod.PowerDraw.Value).IsApprox(1.38);
                CheckThat(mod.DPS.Value).IsApprox(13.405);
                CheckThat(mod.Damage.Value).IsApprox(21.01);
                CheckThat(mod.DistributorDraw.Value).IsApprox(2.69);
                CheckThat(mod.ThermalLoad.Value).IsApprox(7.2);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(100);
                CheckThat(mod.Range.Value).IsApprox(3000);
                CheckThat(mod.Falloff.Value).IsApprox(1000);
                CheckThat(mod.RateOfFire.Value).IsApprox(0.6382);
                CheckThat(mod.BurstInterval.Value).IsApprox(0.567);
                CheckThat(mod.Clip.Value).Is( 2);
                CheckThat(mod.ReloadTime.Value).IsApprox(1);
                CheckThat(mod.BreachDamage.Value).IsApprox(19.955);
            }


            {
                // rail gun long range feedback cascade
                string t = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,KYiG07I_W0FBR00,,9p300A4Y00AKA00AZo00Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":71930,""UnladenMass"":43.5,""CargoCapacity"":4,""MaxJumpRange"":8.038628,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":3850,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_railgun_fixed_small"",""On"":true,""Priority"":0,""Value"":51600,""Engineering"":{""BlueprintName"":""Weapon_LongRange"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_feedback_cascade_cooled"",""Modifiers"":[{""Label"":""Mass"",""Value"":2.6,""OriginalValue"":2},{""Label"":""PowerDraw"",""Value"":1.3225,""OriginalValue"":1.15},{""Label"":""DamagePerSecond"",""Value"":11.455215,""OriginalValue"":14.319018},{""Label"":""Damage"",""Value"":18.672,""OriginalValue"":23.34},{""Label"":""ThermalLoad"",""Value"":7.2,""OriginalValue"":12},{""Label"":""MaximumRange"",""Value"":6000,""OriginalValue"":3000},{""Label"":""FalloffRange"",""Value"":6000,""OriginalValue"":1000}]}},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(t, ShipSlots.Slot.SmallHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(2.6);
                CheckThat(mod.Integrity.Value).IsApprox(40);
                CheckThat(mod.PowerDraw.Value).IsApprox(1.3225);
                CheckThat(mod.DPS.Value).IsApprox(11.455);
                CheckThat(mod.Damage.Value).IsApprox(18.67);
                CheckThat(mod.DistributorDraw.Value).IsApprox(2.69);
                CheckThat(mod.ThermalLoad.Value).IsApprox(7.2);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(100);
                CheckThat(mod.Range.Value).IsApprox(6000);
                CheckThat(mod.Falloff.Value).IsApprox(6000);
                CheckThat(mod.RateOfFire.Value).IsApprox(0.6135);
                CheckThat(mod.BurstInterval.Value).IsApprox(0.63);
                CheckThat(mod.Clip.Value).Is( 1);
                CheckThat(mod.Ammo.Value).Is( 80);
                CheckThat(mod.ReloadTime.Value).IsApprox(1);
                CheckThat(mod.BreachDamage.Value).IsApprox(17.738);
            }


            {
                // rail gun light weight feedback cascade
                string t = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,KYiG05I_W0FBR00,,9p300A4Y00AKA00AZo00Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":71930,""UnladenMass"":41.1,""CargoCapacity"":4,""MaxJumpRange"":8.501283,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":3850,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_railgun_fixed_small"",""On"":true,""Priority"":0,""Value"":51600,""Engineering"":{""BlueprintName"":""Weapon_LightWeight"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_feedback_cascade_cooled"",""Modifiers"":[{""Label"":""Mass"",""Value"":0.2,""OriginalValue"":2},{""Label"":""Integrity"",""Value"":16,""OriginalValue"":40},{""Label"":""PowerDraw"",""Value"":0.69,""OriginalValue"":1.15},{""Label"":""DamagePerSecond"",""Value"":11.455215,""OriginalValue"":14.319018},{""Label"":""Damage"",""Value"":18.672,""OriginalValue"":23.34},{""Label"":""DistributorDraw"",""Value"":1.7485,""OriginalValue"":2.69},{""Label"":""ThermalLoad"",""Value"":7.2,""OriginalValue"":12}]}},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(t, ShipSlots.Slot.SmallHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(0.2);
                CheckThat(mod.Integrity.Value).IsApprox(16);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.69);
                CheckThat(mod.DPS.Value).IsApprox(11.455);
                CheckThat(mod.Damage.Value).IsApprox(18.67);
                CheckThat(mod.DistributorDraw.Value).IsApprox(1.749);
                CheckThat(mod.ThermalLoad.Value).IsApprox(7.2);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(100);
                CheckThat(mod.Range.Value).IsApprox(3000);
                CheckThat(mod.Falloff.Value).IsApprox(1000);
                CheckThat(mod.RateOfFire.Value).IsApprox(0.6135);
                CheckThat(mod.BurstInterval.Value).IsApprox(0.63);
                CheckThat(mod.Clip.Value).Is( 1);
                CheckThat(mod.Ammo.Value).Is( 80);
                CheckThat(mod.ReloadTime.Value).IsApprox(1);
                CheckThat(mod.BreachDamage.Value).IsApprox(17.73);
            }




            {
                //  missile high cap penetrator munitions
                string t = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,K38G03O_W0FBR00,,9p300A4Y00AKA00AZo00Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":52510,""UnladenMass"":44.1,""CargoCapacity"":4,""MaxJumpRange"":7.930727,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":2879,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_dumbfiremissilerack_fixed_small"",""On"":true,""Priority"":0,""Value"":32180,""Engineering"":{""BlueprintName"":""Weapon_HighCapacity"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_penetrator_munitions"",""Modifiers"":[{""Label"":""Mass"",""Value"":3.2,""OriginalValue"":2},{""Label"":""PowerDraw"",""Value"":0.48,""OriginalValue"":0.4},{""Label"":""DamagePerSecond"",""Value"":27.777778,""OriginalValue"":25},{""Label"":""RateOfFire"",""Value"":0.555556,""OriginalValue"":0.5},{""Label"":""AmmoClipSize"",""Value"":16,""OriginalValue"":8},{""Label"":""AmmoMaximum"",""Value"":32,""OriginalValue"":16}]}},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(t, ShipSlots.Slot.SmallHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(3.2);
                CheckThat(mod.Integrity.Value).IsApprox(40);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.48);
                CheckThat(mod.DPS.Value).IsApprox(27.78);
                CheckThat(mod.Damage.Value).IsApprox(50);
                CheckThat(mod.DistributorDraw.Value).IsApprox(0.24);
                CheckThat(mod.ThermalLoad.Value).IsApprox(3.6);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(60);
                CheckThat(mod.Speed.Value).IsApprox(750);
                CheckThat(mod.RateOfFire.Value).IsApprox(0.5556);
                CheckThat(mod.BurstInterval.Value).IsApprox(1.7777);
                CheckThat(mod.Clip.Value).Is( 16);
                CheckThat(mod.Ammo.Value).Is( 32);
                CheckThat(mod.ReloadTime.Value).IsApprox(5);
                CheckThat(mod.BreachDamage.Value).IsApprox(20);
            }
            {
                // missile sturdy emissive munitions
                string t = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,K38G09I_W0FBR00,,9p300A4Y00AKA00AZo00Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":52510,""UnladenMass"":44.9,""CargoCapacity"":4,""MaxJumpRange"":7.791286,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":2879,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_dumbfiremissilerack_fixed_small"",""On"":true,""Priority"":0,""Value"":32180,""Engineering"":{""BlueprintName"":""Weapon_Sturdy"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_emissive_munitions"",""Modifiers"":[{""Label"":""Mass"",""Value"":4,""OriginalValue"":2},{""Label"":""Integrity"",""Value"":160,""OriginalValue"":40},{""Label"":""ThermalLoad"",""Value"":5.04,""OriginalValue"":3.6},{""Label"":""ArmourPenetration"",""Value"":96,""OriginalValue"":60}]}},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(t, ShipSlots.Slot.SmallHardpoint1);


                CheckThat(mod.Mass.Value).IsApprox(4);
                CheckThat(mod.Integrity.Value).IsApprox(160);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.4);
                CheckThat(mod.DPS.Value).IsApprox(25);
                CheckThat(mod.Damage.Value).IsApprox(50);
                CheckThat(mod.DistributorDraw.Value).IsApprox(0.24);
                CheckThat(mod.ThermalLoad.Value).IsApprox(5.04);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(96);
                CheckThat(mod.Speed.Value).IsApprox(750);
                CheckThat(mod.RateOfFire.Value).IsApprox(0.5);
                CheckThat(mod.BurstInterval.Value).IsApprox(2);
                CheckThat(mod.Clip.Value).Is( 8);
                CheckThat(mod.Ammo.Value).Is( 16);
                CheckThat(mod.ReloadTime.Value).IsApprox(5);
                CheckThat(mod.BreachDamage.Value).IsApprox(20);
            }
            {
                string pulselaserefficientscramble = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,FBRG03O_W0FBR00,,9p300A4Y00AKA00AZo00Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":26930,""UnladenMass"":42.9,""CargoCapacity"":4,""MaxJumpRange"":8.149506,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":1600,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600,""Engineering"":{""BlueprintName"":""Weapon_Efficient"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_scramble_spectrum"",""Modifiers"":[{""Label"":""PowerDraw"",""Value"":0.2028,""OriginalValue"":0.39},{""Label"":""DamagePerSecond"",""Value"":6.96384,""OriginalValue"":6.24},{""Label"":""Damage"",""Value"":1.9344,""OriginalValue"":1.56},{""Label"":""DistributorDraw"",""Value"":0.1705,""OriginalValue"":0.31},{""Label"":""ThermalLoad"",""Value"":0.124,""OriginalValue"":0.31},{""Label"":""RateOfFire"",""Value"":3.6,""OriginalValue"":4}]}},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";

                var mod = EngineerModule(pulselaserefficientscramble, ShipSlots.Slot.SmallHardpoint1);
                CheckThat(mod.Mass.Value).IsApprox(2);
                CheckThat(mod.Integrity.Value).IsApprox(40);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.2028);
                CheckThat(mod.DPS.Value).IsApprox(6.964);
                CheckThat(mod.Damage.Value).IsApprox(1.9344);
                CheckThat(mod.DistributorDraw.Value).IsApprox(0.1705);
                CheckThat(mod.ThermalLoad.Value).IsApprox(0.124);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(20);
                CheckThat(mod.RateOfFire.Value).IsApprox(3.6);
                CheckThat(mod.BurstInterval.Value).IsApprox(0.2778);
            }

            {
                string pulseefficientemmisive = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,FBRG03J_W0FBR00,,9p300A4Y00AKA00AZo00Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":26930,""UnladenMass"":42.9,""CargoCapacity"":4,""MaxJumpRange"":8.149506,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":1600,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600,""Engineering"":{""BlueprintName"":""Weapon_Efficient"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_emissive_munitions"",""Modifiers"":[{""Label"":""PowerDraw"",""Value"":0.2028,""OriginalValue"":0.39},{""Label"":""DamagePerSecond"",""Value"":7.7376,""OriginalValue"":6.24},{""Label"":""Damage"",""Value"":1.9344,""OriginalValue"":1.56},{""Label"":""DistributorDraw"",""Value"":0.1705,""OriginalValue"":0.31},{""Label"":""ThermalLoad"",""Value"":0.248,""OriginalValue"":0.31}]}},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(pulseefficientemmisive, ShipSlots.Slot.SmallHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(2);
                CheckThat(mod.Integrity.Value).IsApprox(40);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.2028);
                CheckThat(mod.DPS.Value).IsApprox(7.738);
                CheckThat(mod.Damage.Value).IsApprox(1.9344);
                CheckThat(mod.DistributorDraw.Value).IsApprox(0.1705);
                CheckThat(mod.ThermalLoad.Value).IsApprox(0.248);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(20);
                CheckThat(mod.RateOfFire.Value).IsApprox(4);
                CheckThat(mod.BurstInterval.Value).IsApprox(0.25);
            }
            {
                string burstefficientemmisive = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,FBRG03J_W0FBR00,,9p300A4Y00AKA00AZo00Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":26930,""UnladenMass"":42.9,""CargoCapacity"":4,""MaxJumpRange"":8.149506,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":1600,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600,""Engineering"":{""BlueprintName"":""Weapon_Efficient"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_emissive_munitions"",""Modifiers"":[{""Label"":""PowerDraw"",""Value"":0.2028,""OriginalValue"":0.39},{""Label"":""DamagePerSecond"",""Value"":7.7376,""OriginalValue"":6.24},{""Label"":""Damage"",""Value"":1.9344,""OriginalValue"":1.56},{""Label"":""DistributorDraw"",""Value"":0.1705,""OriginalValue"":0.31},{""Label"":""ThermalLoad"",""Value"":0.248,""OriginalValue"":0.31}]}},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(burstefficientemmisive, ShipSlots.Slot.SmallHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(2);
                CheckThat(mod.Integrity.Value).IsApprox(40);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.2028);
                CheckThat(mod.DPS.Value).IsApprox(7.738);
                CheckThat(mod.Damage.Value).IsApprox(1.9344);
                CheckThat(mod.DistributorDraw.Value).IsApprox(0.1705);
                CheckThat(mod.ThermalLoad.Value).IsApprox(0.248);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(20);
                CheckThat(mod.RateOfFire.Value).IsApprox(4);
                CheckThat(mod.BurstInterval.Value).IsApprox(0.25);
            }
            {
                // cannon efficient smart
                string t = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,H87G03P_W0FBR00,,9p300A4Y00AKA00AZo00Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":62530,""UnladenMass"":42.9,""CargoCapacity"":4,""MaxJumpRange"":8.149506,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":3380,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_cannon_gimbal_small"",""On"":true,""Priority"":0,""Value"":42200,""Engineering"":{""BlueprintName"":""Weapon_Efficient"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_smart_rounds"",""Modifiers"":[{""Label"":""PowerDraw"",""Value"":0.1976,""OriginalValue"":0.38},{""Label"":""DamagePerSecond"",""Value"":10.281667,""OriginalValue"":8.291667},{""Label"":""Damage"",""Value"":19.7408,""OriginalValue"":15.92},{""Label"":""DistributorDraw"",""Value"":0.264,""OriginalValue"":0.48},{""Label"":""ThermalLoad"",""Value"":0.5,""OriginalValue"":1.25}]}},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(t, ShipSlots.Slot.SmallHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(2);
                CheckThat(mod.Integrity.Value).IsApprox(40);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.1976);
                CheckThat(mod.DPS.Value).IsApprox(10.282);
                CheckThat(mod.Damage.Value).IsApprox(19.741);
                CheckThat(mod.DistributorDraw.Value).IsApprox(0.264);
                CheckThat(mod.ThermalLoad.Value).IsApprox(0.5);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(35);
                CheckThat(mod.Range.Value).IsApprox(3000);
                CheckThat(mod.Falloff.Value).IsApprox(3000);
                CheckThat(mod.Speed.Value).IsApprox(1000);
                CheckThat(mod.RateOfFire.Value).IsApprox(0.5208);
                CheckThat(mod.BurstInterval.Value).IsApprox(1.92);
                CheckThat(mod.Clip.Value).Is( 5);
                CheckThat(mod.Ammo.Value).Is( 100);
                CheckThat(mod.ReloadTime.Value).IsApprox(4);
                CheckThat(mod.BreachDamage.Value).IsApprox(18.753);
            }
            {
                // cannon efficient force shell
                string t = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,H87G03L_W0FBR00,,9p300A4Y00AKA00AZo00Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":62530,""UnladenMass"":42.9,""CargoCapacity"":4,""MaxJumpRange"":8.149506,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":3380,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_cannon_gimbal_small"",""On"":true,""Priority"":0,""Value"":42200,""Engineering"":{""BlueprintName"":""Weapon_Efficient"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_force_shell"",""Modifiers"":[{""Label"":""PowerDraw"",""Value"":0.1976,""OriginalValue"":0.38},{""Label"":""DamagePerSecond"",""Value"":10.281667,""OriginalValue"":8.291667},{""Label"":""Damage"",""Value"":19.7408,""OriginalValue"":15.92},{""Label"":""DistributorDraw"",""Value"":0.264,""OriginalValue"":0.48},{""Label"":""ThermalLoad"",""Value"":0.5,""OriginalValue"":1.25},{""Label"":""ShotSpeed"",""Value"":833.333333,""OriginalValue"":1000}]}},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(t, ShipSlots.Slot.SmallHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(2);
                CheckThat(mod.Integrity.Value).IsApprox(40);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.1976);
                CheckThat(mod.DPS.Value).IsApprox(10.282);
                CheckThat(mod.Damage.Value).IsApprox(19.741);
                CheckThat(mod.DistributorDraw.Value).IsApprox(0.264);
                CheckThat(mod.ThermalLoad.Value).IsApprox(0.5);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(35);
                CheckThat(mod.Range.Value).IsApprox(3000);
                CheckThat(mod.Falloff.Value).IsApprox(3000);
                CheckThat(mod.Speed.Value).IsApprox(833.3);
                CheckThat(mod.RateOfFire.Value).IsApprox(0.5208);
                CheckThat(mod.BurstInterval.Value).IsApprox(1.92);
                CheckThat(mod.Clip.Value).Is( 5);
                CheckThat(mod.Ammo.Value).Is( 100);
                CheckThat(mod.ReloadTime.Value).IsApprox(4);
                CheckThat(mod.BreachDamage.Value).IsApprox(18.753);
            }

            {
                // fragment efficient special ince
                string t = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,HNlG05M_W0FBR00,,9p300A4Y00AKA00AZo00Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":75050,""UnladenMass"":42.9,""CargoCapacity"":4,""MaxJumpRange"":8.149506,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":4006,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_slugshot_gimbal_small"",""On"":true,""Priority"":0,""Value"":54720,""Engineering"":{""BlueprintName"":""Weapon_Efficient"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_incendiary_rounds"",""Modifiers"":[{""Label"":""PowerDraw"",""Value"":0.3068,""OriginalValue"":0.59},{""Label"":""DamagePerSecond"",""Value"":83.984471,""OriginalValue"":71.294118},{""Label"":""Damage"",""Value"":1.2524,""OriginalValue"":1.01},{""Label"":""DistributorDraw"",""Value"":0.143,""OriginalValue"":0.26},{""Label"":""ThermalLoad"",""Value"":0.528,""OriginalValue"":0.44},{""Label"":""RateOfFire"",""Value"":5.588235,""OriginalValue"":5.882353}]}},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(t, ShipSlots.Slot.SmallHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(2);
                CheckThat(mod.Integrity.Value).IsApprox(40);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.3068);
                CheckThat(mod.DPS.Value).IsApprox(83.98);
                CheckThat(mod.Damage.Value).IsApprox(1.2524);
                CheckThat(mod.DistributorDraw.Value).IsApprox(0.143);
                CheckThat(mod.ThermalLoad.Value).IsApprox(0.528);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(20);
                CheckThat(mod.Range.Value).IsApprox(2000);
                CheckThat(mod.Falloff.Value).IsApprox(1800);
                CheckThat(mod.Speed.Value).IsApprox(667);
                CheckThat(mod.RateOfFire.Value).IsApprox(5.588);
                CheckThat(mod.BurstInterval.Value).IsApprox(0.1789);
                CheckThat(mod.Clip.Value).Is( 3);
                CheckThat(mod.Ammo.Value).Is( 180);
                CheckThat(mod.ReloadTime.Value).IsApprox(5);
                CheckThat(mod.BreachDamage.Value).IsApprox(1.127);
            }

            {
                // fragment overchanged incendiary
                string t = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,HNlG0BM_W0FBR00,,9p300A4Y00AKA00AZo00Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":75050,""UnladenMass"":42.9,""CargoCapacity"":4,""MaxJumpRange"":8.149506,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":4006,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_slugshot_gimbal_small"",""On"":true,""Priority"":0,""Value"":54720,""Engineering"":{""BlueprintName"":""Weapon_Overcharged"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_incendiary_rounds"",""Modifiers"":[{""Label"":""DamagePerSecond"",""Value"":115.14,""OriginalValue"":71.294118},{""Label"":""Damage"",""Value"":1.717,""OriginalValue"":1.01},{""Label"":""DistributorDraw"",""Value"":0.351,""OriginalValue"":0.26},{""Label"":""ThermalLoad"",""Value"":1.518,""OriginalValue"":0.44},{""Label"":""RateOfFire"",""Value"":5.588235,""OriginalValue"":5.882353}]}},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(t, ShipSlots.Slot.SmallHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(2);
                CheckThat(mod.Integrity.Value).IsApprox(40);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.59);
                CheckThat(mod.DPS.Value).IsApprox(115.14);
                CheckThat(mod.Damage.Value).IsApprox(1.717);
                CheckThat(mod.DistributorDraw.Value).IsApprox(0.351);
                CheckThat(mod.ThermalLoad.Value).IsApprox(1.518);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(20);
                CheckThat(mod.Range.Value).IsApprox(2000);
                CheckThat(mod.Falloff.Value).IsApprox(1800);
                CheckThat(mod.Speed.Value).IsApprox(667);
                CheckThat(mod.RateOfFire.Value).IsApprox(5.588);
                CheckThat(mod.BurstInterval.Value).IsApprox(0.1789);
                CheckThat(mod.Clip.Value).Is( 3);
                CheckThat(mod.Ammo.Value).Is( 180);
                CheckThat(mod.ReloadTime.Value).IsApprox(5);
                CheckThat(mod.BreachDamage.Value).IsApprox(1.545);
            }

            {
                // beam efficient thermal shock
                string t = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,EhtG03O_W0FBR00,,9p300A4Y00AKA00AZo00Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":94980,""UnladenMass"":42.9,""CargoCapacity"":4,""MaxJumpRange"":8.149506,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":5002,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_beamlaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":74650,""Engineering"":{""BlueprintName"":""Weapon_Efficient"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_thermalshock"",""Modifiers"":[{""Label"":""PowerDraw"",""Value"":0.312,""OriginalValue"":0.6},{""Label"":""DamagePerSecond"",""Value"":8.57088,""OriginalValue"":7.68},{""Label"":""DistributorDraw"",""Value"":1.1605,""OriginalValue"":2.11},{""Label"":""ThermalLoad"",""Value"":1.46,""OriginalValue"":3.65}]}},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(t, ShipSlots.Slot.SmallHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(2);
                CheckThat(mod.Integrity.Value).IsApprox(40);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.312);
                CheckThat(mod.DPS.Value).IsApprox(8.571);
                CheckThat(mod.Damage.Value).IsApprox(8.571);
                CheckThat(mod.DistributorDraw.Value).IsApprox(1.161);
                CheckThat(mod.ThermalLoad.Value).IsApprox(1.46);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(18);
                CheckThat(mod.Range.Value).IsApprox(3000);
                CheckThat(mod.Falloff.Value).IsApprox(600);
                CheckThat(mod.BreachDamage.Value).IsApprox(6.856);
            }

            {
                // beam efficient concordant
                string t = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,EhtG03H_W0FBR00,,9p300A4Y00AKA00AZo00Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":94980,""UnladenMass"":42.9,""CargoCapacity"":4,""MaxJumpRange"":8.149506,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":5002,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_beamlaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":74650,""Engineering"":{""BlueprintName"":""Weapon_Efficient"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_concordant_sequence"",""Modifiers"":[{""Label"":""PowerDraw"",""Value"":0.312,""OriginalValue"":0.6},{""Label"":""DamagePerSecond"",""Value"":9.5232,""OriginalValue"":7.68},{""Label"":""DistributorDraw"",""Value"":1.1605,""OriginalValue"":2.11},{""Label"":""ThermalLoad"",""Value"":2.19,""OriginalValue"":3.65}]}},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(t, ShipSlots.Slot.SmallHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(2);
                CheckThat(mod.Integrity.Value).IsApprox(40);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.312);
                CheckThat(mod.DPS.Value).IsApprox(9.523);
                CheckThat(mod.Damage.Value).IsApprox(9.523);
                CheckThat(mod.DistributorDraw.Value).IsApprox(1.161);
                CheckThat(mod.ThermalLoad.Value).IsApprox(2.19);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(18);
                CheckThat(mod.Range.Value).IsApprox(3000);
                CheckThat(mod.Falloff.Value).IsApprox(600);
                CheckThat(mod.BreachDamage.Value).IsApprox(7.618);
            }
            {
                // beam overchanged thermal conduit
                string t = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,EhtG09N_W0FBR00,,9p300A4Y00AKA00AZo00Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":94980,""UnladenMass"":42.9,""CargoCapacity"":4,""MaxJumpRange"":8.149506,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":5002,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_beamlaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":74650,""Engineering"":{""BlueprintName"":""Weapon_Overcharged"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_thermal_conduit"",""Modifiers"":[{""Label"":""DamagePerSecond"",""Value"":13.056,""OriginalValue"":7.68},{""Label"":""DistributorDraw"",""Value"":2.8485,""OriginalValue"":2.11},{""Label"":""ThermalLoad"",""Value"":4.1975,""OriginalValue"":3.65}]}},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(t, ShipSlots.Slot.SmallHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(2);
                CheckThat(mod.Integrity.Value).IsApprox(40);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.6);
                CheckThat(mod.DPS.Value).IsApprox(13.056);
                CheckThat(mod.Damage.Value).IsApprox(13.056);
                CheckThat(mod.DistributorDraw.Value).IsApprox(2.849);
                CheckThat(mod.ThermalLoad.Value).IsApprox(4.198);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(18);
                CheckThat(mod.Range.Value).IsApprox(3000);
                CheckThat(mod.Falloff.Value).IsApprox(600);
                CheckThat(mod.BreachDamage.Value).IsApprox(10.444);
            }
            {
                // multicannon efficient incendiary rounds
                string t = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,HdhG03M_W0FBR00,,9p300A4Y00AKA00AZo00Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":34580,""UnladenMass"":42.9,""CargoCapacity"":4,""MaxJumpRange"":8.149506,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":1982,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_multicannon_gimbal_small"",""On"":true,""Priority"":0,""Value"":14250,""Engineering"":{""BlueprintName"":""Weapon_Efficient"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_incendiary_rounds"",""Modifiers"":[{""Label"":""PowerDraw"",""Value"":0.1924,""OriginalValue"":0.37},{""Label"":""DamagePerSecond"",""Value"":8.049667,""OriginalValue"":6.833333},{""Label"":""Damage"",""Value"":1.0168,""OriginalValue"":0.82},{""Label"":""DistributorDraw"",""Value"":0.0385,""OriginalValue"":0.07},{""Label"":""ThermalLoad"",""Value"":0.12,""OriginalValue"":0.1},{""Label"":""RateOfFire"",""Value"":7.916667,""OriginalValue"":8.333333}]}},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(t, ShipSlots.Slot.SmallHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(2);
                CheckThat(mod.Integrity.Value).IsApprox(40);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.1924);
                CheckThat(mod.DPS.Value).IsApprox(8.05);
                CheckThat(mod.Damage.Value).IsApprox(1.0168);
                CheckThat(mod.DistributorDraw.Value).IsApprox(0.0385);
                CheckThat(mod.ThermalLoad.Value).IsApprox(0.12);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(22);
                CheckThat(mod.Range.Value).IsApprox(4000);
                CheckThat(mod.Falloff.Value).IsApprox(2000);
                CheckThat(mod.Speed.Value).IsApprox(1600);
                CheckThat(mod.RateOfFire.Value).IsApprox(7.917);
                CheckThat(mod.BurstInterval.Value).IsApprox(0.12632);
                CheckThat(mod.Clip.Value).Is( 90);
                CheckThat(mod.Ammo.Value).Is( 2100);
                CheckThat(mod.ReloadTime.Value).IsApprox(5);
                CheckThat(mod.BreachDamage.Value).IsApprox(0.915);
            }

            {
                // multicannon overchanged corrosive
                string t = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,HdhG0BI_W0FBR00,,9p300A4Y00AKA00AZo00Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":34580,""UnladenMass"":42.9,""CargoCapacity"":4,""MaxJumpRange"":8.149506,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":1982,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_multicannon_gimbal_small"",""On"":true,""Priority"":0,""Value"":14250,""Engineering"":{""BlueprintName"":""Weapon_Overcharged"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_corrosive_shell"",""Modifiers"":[{""Label"":""DamagePerSecond"",""Value"":11.616667,""OriginalValue"":6.833333},{""Label"":""Damage"",""Value"":1.394,""OriginalValue"":0.82},{""Label"":""DistributorDraw"",""Value"":0.0945,""OriginalValue"":0.07},{""Label"":""ThermalLoad"",""Value"":0.115,""OriginalValue"":0.1},{""Label"":""AmmoClipSize"",""Value"":77,""OriginalValue"":90},{""Label"":""AmmoMaximum"",""Value"":1680,""OriginalValue"":2100}]}},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(t, ShipSlots.Slot.SmallHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(2);
                CheckThat(mod.Integrity.Value).IsApprox(40);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.37);
                CheckThat(mod.DPS.Value).IsApprox(11.617);
                CheckThat(mod.Damage.Value).IsApprox(1.394);
                CheckThat(mod.DistributorDraw.Value).IsApprox(0.0945);
                CheckThat(mod.ThermalLoad.Value).IsApprox(0.115);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(22);
                CheckThat(mod.Range.Value).IsApprox(4000);
                CheckThat(mod.Falloff.Value).IsApprox(2000);
                CheckThat(mod.Speed.Value).IsApprox(1600);
                CheckThat(mod.RateOfFire.Value).IsApprox(8.333);
                CheckThat(mod.Clip.Value).Is( 77);
                CheckThat(mod.Ammo.Value).Is( 1680);
                CheckThat(mod.ReloadTime.Value).IsApprox(5);
                CheckThat(mod.BreachDamage.Value).IsApprox(1.254);
                CheckThat(mod.BurstInterval.Value).IsApprox(0.12);
            }




            {
                // missile, light weight, overload
                string t = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,K3BG05M_W0FBR00,,9p300A4Y00AKA00AZo00Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":92930,""UnladenMass"":41.1,""CargoCapacity"":4,""MaxJumpRange"":8.501283,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":4900,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_basicmissilerack_fixed_small"",""On"":true,""Priority"":0,""Value"":72600,""Engineering"":{""BlueprintName"":""Weapon_LightWeight"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_overload_munitions"",""Modifiers"":[{""Label"":""Mass"",""Value"":0.2,""OriginalValue"":2},{""Label"":""Integrity"",""Value"":16,""OriginalValue"":40},{""Label"":""PowerDraw"",""Value"":0.36,""OriginalValue"":0.6},{""Label"":""DistributorDraw"",""Value"":0.156,""OriginalValue"":0.24}]}},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(t, ShipSlots.Slot.SmallHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(0.2);
                CheckThat(mod.Integrity.Value).IsApprox(16);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.36);
                CheckThat(mod.DPS.Value).IsApprox(13.333);
                CheckThat(mod.Damage.Value).IsApprox(40);
                CheckThat(mod.DistributorDraw.Value).IsApprox(0.156);
                CheckThat(mod.ThermalLoad.Value).IsApprox(3.6);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(60);
                CheckThat(mod.Speed.Value).IsApprox(625);
                CheckThat(mod.RateOfFire.Value).IsApprox(0.3333);
                CheckThat(mod.BurstInterval.Value).IsApprox(3);
                CheckThat(mod.Clip.Value).Is( 6);
                CheckThat(mod.Ammo.Value).Is( 6);
                CheckThat(mod.ReloadTime.Value).IsApprox(12);
                CheckThat(mod.BreachDamage.Value).IsApprox(16);
            }

            {
                // missile, light weight, stripped down, 50%
                string t = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,K3BG05PVG0FBR00,,9p300A4Y00AKA00AZo00Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":92930,""UnladenMass"":41.215,""CargoCapacity"":4,""MaxJumpRange"":8.477903,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":4900,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_basicmissilerack_fixed_small"",""On"":true,""Priority"":0,""Value"":72600,""Engineering"":{""BlueprintName"":""Weapon_LightWeight"",""Level"":5,""Quality"":0.5,""ExperimentalEffect"":""special_weapon_lightweight"",""Modifiers"":[{""Label"":""Mass"",""Value"":0.315,""OriginalValue"":2},{""Label"":""Integrity"",""Value"":16,""OriginalValue"":40},{""Label"":""PowerDraw"",""Value"":0.39,""OriginalValue"":0.6},{""Label"":""DistributorDraw"",""Value"":0.162,""OriginalValue"":0.24}]}},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(t, ShipSlots.Slot.SmallHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(0.315);
                CheckThat(mod.Integrity.Value).IsApprox(16);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.39);
                CheckThat(mod.DPS.Value).IsApprox(13.333);
                CheckThat(mod.Damage.Value).IsApprox(40);
                CheckThat(mod.DistributorDraw.Value).IsApprox(0.162);
                CheckThat(mod.ThermalLoad.Value).IsApprox(3.6);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(60);
                CheckThat(mod.Speed.Value).IsApprox(625);
                CheckThat(mod.RateOfFire.Value).IsApprox(0.3333);
                CheckThat(mod.BurstInterval.Value).IsApprox(3);
                CheckThat(mod.Clip.Value).Is( 6);
                CheckThat(mod.Ammo.Value).Is( 6);
                CheckThat(mod.ReloadTime.Value).IsApprox(12);
                CheckThat(mod.BreachDamage.Value).IsApprox(16);
            }

            {
                // torpedo light weight penetrator
                string t = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,Kp9G03L_W0FBR00,,9p300A4Y00AKA00AZo00Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":31530,""UnladenMass"":41.1,""CargoCapacity"":4,""MaxJumpRange"":8.501283,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":1830,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_advancedtorppylon_fixed_small"",""On"":true,""Priority"":0,""Value"":11200,""Engineering"":{""BlueprintName"":""Weapon_LightWeight"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_deep_cut_payload"",""Modifiers"":[{""Label"":""Mass"",""Value"":0.2,""OriginalValue"":2},{""Label"":""Integrity"",""Value"":16,""OriginalValue"":40},{""Label"":""PowerDraw"",""Value"":0.24,""OriginalValue"":0.4}]}},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(t, ShipSlots.Slot.SmallHardpoint1);

                CheckThat(mod.Mass.Value).IsApprox(0.2);
                CheckThat(mod.Integrity.Value).IsApprox(16);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.24);
                CheckThat(mod.DPS.Value).IsApprox(120);
                CheckThat(mod.Damage.Value).IsApprox(120);
                CheckThat(mod.ThermalLoad.Value).IsApprox(45);
                CheckThat(mod.ArmourPiercing.Value).IsApprox(10000);
                CheckThat(mod.Speed.Value).IsApprox(250);
                CheckThat(mod.RateOfFire.Value).IsApprox(1);
                CheckThat(mod.BurstInterval.Value).IsApprox(1);
                CheckThat(mod.Clip.Value).Is( 1);
                CheckThat(mod.ReloadTime.Value).IsApprox(5);
                CheckThat(mod.BreachDamage.Value).IsApprox(60);
            }
            {
                // lightweight alloy heavy duty deep plating
                string t = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,Eht00FBR00,,9p3G05I_W0A4Y00AKA00AZo00Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":94980,""UnladenMass"":42.9,""CargoCapacity"":4,""MaxJumpRange"":8.149506,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":5002,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_beamlaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":74650},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0,""Engineering"":{""BlueprintName"":""Armour_HeavyDuty"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_armour_chunky"",""Modifiers"":[{""Label"":""DefenceModifierHealthMultiplier"",""Value"":156.608,""OriginalValue"":80},{""Label"":""KineticResistance"",""Value"":-17.42,""OriginalValue"":-20},{""Label"":""ThermicResistance"",""Value"":2.15,""OriginalValue"":0},{""Label"":""ExplosiveResistance"",""Value"":-36.99,""OriginalValue"":-40}]}},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(t, ShipSlots.Slot.Armour);

                CheckThat(mod.Mass.Value).IsApprox(0);
                CheckThat(mod.HullStrengthBonus.Value).IsApprox(156.61);
                CheckThat(mod.KineticResistance.Value).IsApprox(-17.42);
                CheckThat(mod.ThermalResistance.Value).IsApprox(2.15);
                CheckThat(mod.ExplosiveResistance.Value).IsApprox(-36.99);
                CheckThat(mod.AXResistance.Value).IsApprox(90);
            }

            {
                // reactive alloy kinetic resistance deep plating
                string t = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,Eht00FBR00,,9opG07I_W0A4Y00AKA00AZo00Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":234400,""UnladenMass"":46.9,""CargoCapacity"":4,""MaxJumpRange"":7.463231,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":11973,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_beamlaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":74650},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_reactive"",""On"":true,""Priority"":0,""Value"":139420,""Engineering"":{""BlueprintName"":""Armour_Kinetic"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_armour_chunky"",""Modifiers"":[{""Label"":""DefenceModifierHealthMultiplier"",""Value"":278,""OriginalValue"":250},{""Label"":""KineticResistance"",""Value"":53.65,""OriginalValue"":25},{""Label"":""ThermicResistance"",""Value"":-61.504,""OriginalValue"":-40},{""Label"":""ExplosiveResistance"",""Value"":7.712,""OriginalValue"":20}]}},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(t, ShipSlots.Slot.Armour);

                CheckThat(mod.Mass.Value).IsApprox(4);
                CheckThat(mod.HullStrengthBonus.Value).IsApprox(278);
                CheckThat(mod.KineticResistance.Value).IsApprox(53.65);
                CheckThat(mod.ThermalResistance.Value).IsApprox(-61.5);
                CheckThat(mod.ExplosiveResistance.Value).IsApprox(7.71);
                CheckThat(mod.AXResistance.Value).IsApprox(90);
            }

            {
                // frame shift drive
                string t = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,FBR00FBR00,,9p300A4Y00AKAG07J_W0AZAG05J_W0Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":185170,""UnladenMass"":44.275,""CargoCapacity"":4,""MaxJumpRange"":27.812499,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":9512,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980,""Engineering"":{""BlueprintName"":""Engine_Reinforced"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_engine_haulage"",""Modifiers"":[{""Label"":""Mass"",""Value"":3.125,""OriginalValue"":2.5},{""Label"":""Integrity"",""Value"":96.6,""OriginalValue"":46},{""Label"":""EngineOptimalMass"",""Value"":52.8,""OriginalValue"":48},{""Label"":""EngineHeatRate"",""Value"":0.65,""OriginalValue"":1.3}]}},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class5"",""On"":true,""Priority"":0,""Value"":160220,""Engineering"":{""BlueprintName"":""FSD_LongRange"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_fsd_heavy"",""Modifiers"":[{""Label"":""Mass"",""Value"":3.25,""OriginalValue"":2.5},{""Label"":""Integrity"",""Value"":50.048,""OriginalValue"":64},{""Label"":""PowerDraw"",""Value"":0.345,""OriginalValue"":0.3},{""Label"":""FSDOptimalMass"",""Value"":145.08,""OriginalValue"":90}]}},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(t, ShipSlots.Slot.FrameShiftDrive);

                CheckThat(mod.Mass.Value).IsApprox(3.25);
                CheckThat(mod.Integrity.Value).IsApprox(50.05);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.345);
                CheckThat(mod.BootTime.Value).IsApprox(10);
                CheckThat(mod.OptMass.Value).IsApprox(145.08);
                CheckThat(mod.ThermalLoad.Value).IsApprox(10);
                CheckThat(mod.MaxFuelPerJump.Value).IsApprox(0.9);
            }
            {
                // frame shift drive
                string t = @"[{""header"":{""appName"":""EDSY"",""appVersion"":308189904,""appURL"":""https://edsy.org/#/L=H100000H4C0S00,FBR00FBR00,,9p300A4Y00AKAG07J_W0AZAG03L_W0Ans00B1U00BH600BWQ00,,7Og0003w00mpU0nG0-0nF0-""},""data"":{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":185170,""UnladenMass"":43.525,""CargoCapacity"":4,""MaxJumpRange"":20.176394,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":9512,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980,""Engineering"":{""BlueprintName"":""Engine_Reinforced"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_engine_haulage"",""Modifiers"":[{""Label"":""Mass"",""Value"":3.125,""OriginalValue"":2.5},{""Label"":""Integrity"",""Value"":96.6,""OriginalValue"":46},{""Label"":""EngineOptimalMass"",""Value"":52.8,""OriginalValue"":48},{""Label"":""EngineHeatRate"",""Value"":0.65,""OriginalValue"":1.3}]}},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class5"",""On"":true,""Priority"":0,""Value"":160220,""Engineering"":{""BlueprintName"":""FSD_FastBoot"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_fsd_cooled"",""Modifiers"":[{""Label"":""Integrity"",""Value"":54.4,""OriginalValue"":64},{""Label"":""BootTime"",""Value"":2,""OriginalValue"":10},{""Label"":""FSDOptimalMass"",""Value"":103.5,""OriginalValue"":90},{""Label"":""FSDHeatRate"",""Value"":10.8,""OriginalValue"":10}]}},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}}]";
                var mod = EngineerModule(t, ShipSlots.Slot.FrameShiftDrive);

                CheckThat(mod.Mass.Value).IsApprox(2.5);
                CheckThat(mod.Integrity.Value).IsApprox(54.4);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.3);
                CheckThat(mod.BootTime.Value).IsApprox(2);
                CheckThat(mod.OptMass.Value).IsApprox(103.5);
                CheckThat(mod.ThermalLoad.Value).IsApprox(10.8);
                CheckThat(mod.MaxFuelPerJump.Value).IsApprox(0.9);
            }
            {
                // life support
                string t = @"{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":185170,""UnladenMass"":44.85,""CargoCapacity"":4,""MaxJumpRange"":17.036565,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":9512,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class5"",""On"":true,""Priority"":0,""Value"":160220},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520,""Engineering"":{""BlueprintName"":""Misc_Reinforced"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""Mass"",""Value"":3.25,""OriginalValue"":1.3},{""Label"":""Integrity"",""Value"":128,""OriginalValue"":32}]}},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.LifeSupport);

                CheckThat(mod.Mass.Value).IsApprox(3.25);
                CheckThat(mod.Integrity.Value).IsApprox(128);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.32);
                CheckThat(mod.BootTime.Value).IsApprox(1);
                CheckThat(mod.Time.Value).IsApprox(300);
            }
            {
                // power dist engine focused stripped down
                string t = @"{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":185170,""UnladenMass"":42.77,""CargoCapacity"":4,""MaxJumpRange"":17.848016,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":9512,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class5"",""On"":true,""Priority"":0,""Value"":160220},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520,""Engineering"":{""BlueprintName"":""PowerDistributor_PriorityEngines"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_powerdistributor_lightweight"",""Modifiers"":[{""Label"":""Mass"",""Value"":1.17,""OriginalValue"":1.3},{""Label"":""WeaponsCapacity"",""Value"":8.5,""OriginalValue"":10},{""Label"":""WeaponsRecharge"",""Value"":1.14,""OriginalValue"":1.2},{""Label"":""EnginesCapacity"",""Value"":12.8,""OriginalValue"":8},{""Label"":""EnginesRecharge"",""Value"":0.576,""OriginalValue"":0.4},{""Label"":""SystemsCapacity"",""Value"":6.8,""OriginalValue"":8},{""Label"":""SystemsRecharge"",""Value"":0.34,""OriginalValue"":0.4}]}},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.PowerDistributor);

                CheckThat(mod.Mass.Value).IsApprox(1.17);
                CheckThat(mod.Integrity.Value).IsApprox(36);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.32);
                CheckThat(mod.BootTime.Value).IsApprox(5);
                CheckThat(mod.WeaponsCapacity.Value).IsApprox(8.5);
                CheckThat(mod.WeaponsRechargeRate.Value).IsApprox(1.14);
                CheckThat(mod.EngineCapacity.Value).IsApprox(12.8);
                CheckThat(mod.EngineRechargeRate.Value).IsApprox(0.576);
                CheckThat(mod.SystemsCapacity.Value).IsApprox(6.8);
                CheckThat(mod.SystemsRechargeRate.Value).IsApprox(0.34);
            }

            {
                // power dist shielded double braced
                string t = @"{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":185170,""UnladenMass"":43.095,""CargoCapacity"":4,""MaxJumpRange"":17.716169,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":9512,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class5"",""On"":true,""Priority"":0,""Value"":160220},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520,""Engineering"":{""BlueprintName"":""PowerDistributor_Shielded"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_powerdistributor_toughened"",""Modifiers"":[{""Label"":""Mass"",""Value"":1.495,""OriginalValue"":1.3},{""Label"":""Integrity"",""Value"":124.2,""OriginalValue"":36},{""Label"":""PowerDraw"",""Value"":0.224,""OriginalValue"":0.32}]}},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.PowerDistributor);

                CheckThat(mod.Mass.Value).IsApprox(1.495);
                CheckThat(mod.Integrity.Value).IsApprox(124.2);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.224);
                CheckThat(mod.BootTime.Value).IsApprox(5);
                CheckThat(mod.WeaponsCapacity.Value).IsApprox(10);
                CheckThat(mod.WeaponsRechargeRate.Value).IsApprox(1.2);
                CheckThat(mod.EngineCapacity.Value).IsApprox(8);
                CheckThat(mod.EngineRechargeRate.Value).IsApprox(0.4);
                CheckThat(mod.SystemsCapacity.Value).IsApprox(8);
                CheckThat(mod.SystemsRechargeRate.Value).IsApprox(0.4);
            }

            {
                // long range
                string t = @"{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":185170,""UnladenMass"":44.395,""CargoCapacity"":4,""MaxJumpRange"":17.207702,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":9512,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class5"",""On"":true,""Priority"":0,""Value"":160220},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520,""Engineering"":{""BlueprintName"":""PowerDistributor_Shielded"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_powerdistributor_toughened"",""Modifiers"":[{""Label"":""Mass"",""Value"":1.495,""OriginalValue"":1.3},{""Label"":""Integrity"",""Value"":124.2,""OriginalValue"":36},{""Label"":""PowerDraw"",""Value"":0.224,""OriginalValue"":0.32}]}},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520,""Engineering"":{""BlueprintName"":""Sensor_LongRange"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""Mass"",""Value"":2.6,""OriginalValue"":1.3},{""Label"":""SensorTargetScanAngle"",""Value"":21,""OriginalValue"":30},{""Label"":""Range"",""Value"":7000,""OriginalValue"":4000}]}},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.Radar);

                CheckThat(mod.Mass.Value).IsApprox(2.6);
                CheckThat(mod.Integrity.Value).IsApprox(36);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.16);
                CheckThat(mod.BootTime.Value).IsApprox(5);
                CheckThat(mod.Range.Value).IsApprox(14000);
                CheckThat(mod.Angle.Value).IsApprox(21);
                CheckThat(mod.TypicalEmission.Value).IsApprox(7000);
            }

            {
                // wide angle
                string t = @"{""event"":""Loadout"",""Ship"":""sidewinder"",""ShipName"":"""",""ShipIdent"":"""",""HullValue"":5070,""ModulesValue"":185170,""UnladenMass"":43.095,""CargoCapacity"":4,""MaxJumpRange"":17.716169,""FuelCapacity"":{""Main"":2,""Reserve"":0.3},""Rebuy"":9512,""Modules"":[{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":0},{""Slot"":""SmallHardpoint1"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""SmallHardpoint2"",""Item"":""hpt_pulselaser_gimbal_small"",""On"":true,""Priority"":0,""Value"":6600},{""Slot"":""Armour"",""Item"":""sidewinder_armour_grade1"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""MainEngines"",""Item"":""int_engine_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_size2_class5"",""On"":true,""Priority"":0,""Value"":160220},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size1_class1"",""On"":true,""Priority"":0,""Value"":520},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size1_class1"",""On"":true,""Priority"":0,""Value"":520,""Engineering"":{""BlueprintName"":""PowerDistributor_Shielded"",""Level"":5,""Quality"":1,""ExperimentalEffect"":""special_powerdistributor_toughened"",""Modifiers"":[{""Label"":""Mass"",""Value"":1.495,""OriginalValue"":1.3},{""Label"":""Integrity"",""Value"":124.2,""OriginalValue"":36},{""Label"":""PowerDraw"",""Value"":0.224,""OriginalValue"":0.32}]}},{""Slot"":""Radar"",""Item"":""int_sensors_size1_class1"",""On"":true,""Priority"":0,""Value"":520,""Engineering"":{""BlueprintName"":""Sensor_WideAngle"",""Level"":5,""Quality"":1,""Modifiers"":[{""Label"":""PowerDraw"",""Value"":0.24,""OriginalValue"":0.16},{""Label"":""SensorTargetScanAngle"",""Value"":90,""OriginalValue"":30},{""Label"":""Range"",""Value"":3200,""OriginalValue"":4000}]}},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size1_class3"",""On"":true,""Priority"":0,""Value"":1000},{""Slot"":""Slot01_Size2"",""Item"":""int_shieldgenerator_size2_class1"",""On"":true,""Priority"":0,""Value"":1980},{""Slot"":""Slot02_Size2"",""Item"":""int_cargorack_size2_class1"",""On"":true,""Priority"":0,""Value"":3250},{""Slot"":""Slot05_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":0,""Value"":0},{""Slot"":""Slot06_Size1"",""Item"":""int_dockingcomputer_advanced"",""On"":true,""Priority"":0,""Value"":0}]}";
                var mod = EngineerModule(t, ShipSlots.Slot.Radar);

                CheckThat(mod.Mass.Value).IsApprox(1.3);
                CheckThat(mod.Integrity.Value).IsApprox(36);
                CheckThat(mod.PowerDraw.Value).IsApprox(0.24);
                CheckThat(mod.BootTime.Value).IsApprox(5);
                CheckThat(mod.Range.Value).IsApprox(6400);
                CheckThat(mod.Angle.Value).IsApprox(90);
                CheckThat(mod.TypicalEmission.Value).IsApprox(3200);
            }
        }

        [System.Diagnostics.DebuggerHidden]
        public static ItemData.ShipModule EngineerModule(string loadout, ShipSlots.Slot slot, bool debugit = false)
        {
            Ship si = Ship.CreateFromLoadout(loadout);
            Debugger.BreakAssert(si != null, "Bad ship");

         //  System.Diagnostics.Debug.WriteLine($"\r\nTEST Module {si.Modules[slot].ItemFD} in {slot} {si.Modules[slot]?.Engineering?.ToString()}");
            //System.Diagnostics.Debug.WriteLine($"\r\nTEST Module {si.Modules[slot].ItemFD} in {lastslot} ");

            var module = si.GetShipModulePropertiesEngineered(slot, debugit);
            Debugger.BreakAssert(module != null, "Bad module");
            return module;
        }

        // this one scans logs for Loadouts and engineers them..

        [BaseUtils.UnitTests.Test(-100)]
        public static void TestLoadouts()
        {
            MaterialCommodityMicroResourceType.Initialise();     // lets statically fill the table way before anyone wants to access it
            ItemData.Initialise();

            var filelist = System.IO.Directory.EnumerateFiles(@"c:\code\logs", "Journal*.log", System.IO.SearchOption.AllDirectories).Select(f => new System.IO.FileInfo(f)).OrderByDescending(p => p.LastWriteTime).ToArray();

            foreach (var x in filelist)
            {
                string[] filelines = BaseUtils.FileHelpers.TryReadAllLinesFromFile(x.FullName);
                if (filelines != null)
                {
                    foreach (var line in filelines)
                    {
                        JObject jo = JObject.Parse(line, JToken.ParseOptions.CheckEOL);     // may be bad json, ignore
                        if (jo != null)
                        {
                            if (jo.Contains("timestamp") && jo.Contains("event") && jo["event"].Str() =="Loadout")
                            {
                                Ship si = Ship.CreateFromLoadout(line);
                                if (si != null)
                                { 
                                    foreach( ShipSlots.Slot slot in Enum.GetValues(typeof(ShipSlots.Slot)))
                                    {
                                        ShipModule sm = si.GetModuleInSlot(slot);       // do we have it?
                                        if (sm != null)
                                        {
                                            if ( sm.Engineering != null )   // is it engineered?
                                            {
                                                //  System.Diagnostics.Debug.WriteLine($"Engineer {sm.ItemFD} with {sm.Engineering.BlueprintName}");

                                                ItemData.ShipModule sme = si.GetShipModulePropertiesEngineered(slot, out string errorlist, false);
                                                if ( errorlist.HasChars() )
                                                {
                                                    System.Diagnostics.Debug.WriteLine($"Engineer {sm.ItemFD} with {sm.Engineering.BlueprintName} produced errors {errorlist}");
                                                }

                                            }
                                        }
                                    }
                                }
                                else
                                    System.Diagnostics.Debug.WriteLine($"Cannot make ship {line}");
                            }
                                
                        }
                    }
                }
                else
                    System.Diagnostics.Debug.WriteLine($"Cannot read file {x.FullName}");
            }

        }
    }
}
    