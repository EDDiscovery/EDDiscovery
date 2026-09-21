using EliteDangerousCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnitTest
{
    public partial class UnitTestModuleDisplay : UserControl
    {
        public UnitTestModuleDisplay()
        {
            InitializeComponent();
        }


        public void Init()
        {
            string loadout =
@"{""timestamp"":""2026-09-20T15:16:00Z"",""event"":""Loadout"",""Ship"":""ferdelance"",""ShipID"":15,""ShipName"":""Intrepid"",""ShipIdent"":""RXP-2"",""HullValue"":51232230,
""ModulesValue"":75786204,""HullHealth"":1.0,""UnladenMass"":510.200012,""CargoCapacity"":16,""MaxJumpRange"":12.48404,""FuelCapacity"":{""Main"":8.0,
""Reserve"":0.67},""Rebuy"":4763194,""Modules"":[{""Slot"":""HugeHardpoint1"",""Item"":""hpt_multicannon_gimbal_huge"",""On"":true,""AmmoInClip"":77,
""AmmoInHopper"":2100,""Health"":1.0,""Value"":6377600,""Engineering"":{""Engineer"":""Tod 'The Blaster' McQuinn"",""EngineerID"":300260,""BlueprintID"":128673504,
""BlueprintName"":""Weapon_Overcharged"",""Level"":5,""Quality"":0.362,""Modifiers"":[{""Label"":""DamagePerSecond"",""Value"":38.12291,""OriginalValue"":23.299664,
""LessIsGood"":0},{""Label"":""Damage"",""Value"":5.661252,""OriginalValue"":3.46,""LessIsGood"":0},{""Label"":""DistributorDraw"",""Value"":0.4995,""OriginalValue"":0.37,
""LessIsGood"":1},{""Label"":""ThermalLoad"",""Value"":0.5865,""OriginalValue"":0.51,""LessIsGood"":1},{""Label"":""AmmoClipSize"",""Value"":77.0,""OriginalValue"":90.0,
""LessIsGood"":0}]}},{""Slot"":""MediumHardpoint1"",""Item"":""hpt_beamlaser_gimbal_medium"",""On"":true,""Priority"":4,""Health"":1.0,""Value"":500600,
""Engineering"":{""Engineer"":""Broo Tarquin"",""EngineerID"":300030,""BlueprintID"":128739086,""BlueprintName"":""Weapon_Overcharged"",""Level"":5,""Quality"":0.291,
""Modifiers"":[{""Label"":""DamagePerSecond"",""Value"":20.396334,""OriginalValue"":12.52,""LessIsGood"":0},{""Label"":""DistributorDraw"",""Value"":4.644,
""OriginalValue"":3.44,""LessIsGood"":1},{""Label"":""ThermalLoad"",""Value"":6.118,""OriginalValue"":5.32,""LessIsGood"":1}]}},{""Slot"":""MediumHardpoint2"",
""Item"":""hpt_beamlaser_gimbal_medium"",""On"":true,""Priority"":0,""Health"":1.0,""Value"":500600,""Engineering"":{""Engineer"":""Broo Tarquin"",""EngineerID"":300030,
""BlueprintID"":128739086,""BlueprintName"":""Weapon_Overcharged"",""Level"":5,""Quality"":0.284,""Modifiers"":[{""Label"":""DamagePerSecond"",""Value"":20.387569,
""OriginalValue"":12.52,""LessIsGood"":0},{""Label"":""DistributorDraw"",""Value"":4.644,""OriginalValue"":3.44,""LessIsGood"":1},{""Label"":""ThermalLoad"",
""Value"":6.118,""OriginalValue"":5.32,""LessIsGood"":1}]}},{""Slot"":""MediumHardpoint3"",""Item"":""hpt_multicannon_gimbal_medium"",""On"":true,""Priority"":0,
""AmmoInClip"":77,""AmmoInHopper"":2100,""Health"":1.0,""Value"":57000,""Engineering"":{""Engineer"":""Tod 'The Blaster' McQuinn"",""EngineerID"":300260,
""BlueprintID"":128673504,""BlueprintName"":""Weapon_Overcharged"",""Level"":5,""Quality"":0.226,""Modifiers"":[{""Label"":""DamagePerSecond"",""Value"":20.469725,
""OriginalValue"":12.615385,""LessIsGood"":0},{""Label"":""Damage"",""Value"":2.661064,""OriginalValue"":1.64,""LessIsGood"":0},{""Label"":""DistributorDraw"",
""Value"":0.189,""OriginalValue"":0.14,""LessIsGood"":1},{""Label"":""ThermalLoad"",""Value"":0.23,""OriginalValue"":0.2,""LessIsGood"":1},{""Label"":""AmmoClipSize"",
""Value"":77.0,""OriginalValue"":90.0,""LessIsGood"":0}]}},{""Slot"":""MediumHardpoint4"",""Item"":""hpt_dumbfiremissilerack_fixed_medium"",""On"":true,
""Priority"":0,""AmmoInClip"":12,""AmmoInHopper"":48,""Health"":1.0,""Value"":234390},{""Slot"":""TinyHardpoint1"",""Item"":""hpt_plasmapointdefence_turret_tiny"",
""On"":true,""Priority"":0,""AmmoInClip"":12,""AmmoInHopper"":10000,""Health"":1.0,""Value"":18546},{""Slot"":""TinyHardpoint2"",""Item"":""hpt_shieldbooster_size0_class4"",
""On"":true,""Priority"":0,""Health"":1.0,""Value"":118950,""Engineering"":{""Engineer"":""Lei Cheung"",""EngineerID"":300120,""BlueprintID"":128673797,
""BlueprintName"":""ShieldBooster_Thermic"",""Level"":3,""Quality"":0.974,""Modifiers"":[{""Label"":""KineticResistance"",""Value"":-2.499998,""OriginalValue"":0.0,
""LessIsGood"":0},{""Label"":""ThermicResistance"",""Value"":16.869999,""OriginalValue"":0.0,""LessIsGood"":0},{""Label"":""ExplosiveResistance"",""Value"":-2.499998,
""OriginalValue"":0.0,""LessIsGood"":0}]}},{""Slot"":""TinyHardpoint3"",""Item"":""hpt_shieldbooster_size0_class5"",""On"":true,""Priority"":0,""Health"":1.0,
""Value"":281000,""Engineering"":{""Engineer"":""Lei Cheung"",""EngineerID"":300120,""BlueprintID"":128673782,""BlueprintName"":""ShieldBooster_HeavyDuty"",
""Level"":3,""Quality"":0.8829,""Modifiers"":[{""Label"":""Mass"",""Value"":10.5,""OriginalValue"":3.5,""LessIsGood"":1},{""Label"":""Integrity"",""Value"":52.1712,
""OriginalValue"":48.0,""LessIsGood"":0},{""Label"":""PowerDraw"",""Value"":1.38,""OriginalValue"":1.2,""LessIsGood"":1},{""Label"":""DefenceModifierShieldMultiplier"",
""Value"":47.816002,""OriginalValue"":20.000004,""LessIsGood"":0}]}},{""Slot"":""TinyHardpoint4"",""Item"":""hpt_shieldbooster_size0_class4"",""On"":true,
""Priority"":0,""Health"":1.0,""Value"":122000,""Engineering"":{""Engineer"":""Lei Cheung"",""EngineerID"":300120,""BlueprintID"":128673782,""BlueprintName"":""ShieldBooster_HeavyDuty"",
""Level"":3,""Quality"":0.91,""Modifiers"":[{""Label"":""Mass"",""Value"":9.0,""OriginalValue"":3.0,""LessIsGood"":1},{""Label"":""Integrity"",""Value"":48.928501,
""OriginalValue"":45.0,""LessIsGood"":0},{""Label"":""PowerDraw"",""Value"":1.15,""OriginalValue"":1.0,""LessIsGood"":1},{""Label"":""DefenceModifierShieldMultiplier"",
""Value"":43.236794,""OriginalValue"":15.999996,""LessIsGood"":0}]}},{""Slot"":""TinyHardpoint5"",""Item"":""hpt_plasmapointdefence_turret_tiny"",""On"":true,
""Priority"":0,""AmmoInClip"":12,""AmmoInHopper"":10000,""Health"":1.0,""Value"":18546},{""Slot"":""TinyHardpoint6"",""Item"":""hpt_chafflauncher_tiny"",
""On"":true,""Priority"":0,""AmmoInClip"":1,""AmmoInHopper"":10,""Health"":1.0,""Value"":8500},{""Slot"":""Armour"",""Item"":""ferdelance_armour_grade3"",
""On"":true,""Priority"":1,""Health"":1.0,""Value"":39448786,""Engineering"":{""Engineer"":""Selene Jean"",""EngineerID"":300210,""BlueprintID"":128673644,
""BlueprintName"":""Armour_HeavyDuty"",""Level"":5,""Quality"":0.74,""Modifiers"":[{""Label"":""Mass"",""Value"":49.399998,""OriginalValue"":38.0,""LessIsGood"":1}
,{""Label"":""DefenceModifierHealthMultiplier"",""Value"":358.5,""OriginalValue"":250.0,""LessIsGood"":0},{""Label"":""KineticResistance"",""Value"":-14.312005,
""OriginalValue"":-20.000004,""LessIsGood"":0},{""Label"":""ThermicResistance"",""Value"":4.74,""OriginalValue"":0.0,""LessIsGood"":0},{""Label"":""ExplosiveResistance"",
""Value"":-33.363998,""OriginalValue"":-39.999996,""LessIsGood"":0}]}},{""Slot"":""PaintJob"",""Item"":""paintjob_ferdelance_blackfriday_01"",""On"":true,
""Priority"":1,""Health"":1.0},{""Slot"":""Decal1"",""Item"":""decal_combat_dangerous"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""Decal2"",""Item"":""decal_explorer_elite"",
""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""Decal3"",""Item"":""decal_combat_dangerous"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""ShipName0"",
""Item"":""nameplate_explorer01_white"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""ShipName1"",""Item"":""nameplate_explorer01_white"",""On"":true,
""Priority"":1,""Health"":1.0},{""Slot"":""ShipID0"",""Item"":""nameplate_shipid_doubleline_white"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""ShipID1"",
""Item"":""nameplate_shipid_doubleline_white"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size6_class5"",
""On"":true,""Priority"":1,""Health"":1.0,""Value"":13752602},{""Slot"":""MainEngines"",""Item"":""int_engine_size5_class5"",""On"":true,""Priority"":0,""Health"":1.0,
""Value"":4338361,""Engineering"":{""Engineer"":""Professor Palin"",""EngineerID"":300220,""BlueprintID"":128673659,""BlueprintName"":""Engine_Dirty"",
""Level"":5,""Quality"":0.9757,""Modifiers"":[{""Label"":""Integrity"",""Value"":90.100006,""OriginalValue"":106.0,""LessIsGood"":0},{""Label"":""PowerDraw"",
""Value"":6.8544,""OriginalValue"":6.12,""LessIsGood"":1},{""Label"":""EngineOptimalMass"",""Value"":735.0,""OriginalValue"":840.0,""LessIsGood"":0},
{""Label"":""EngineOptPerformance"",""Value"":139.829987,""OriginalValue"":100.0,""LessIsGood"":0},{""Label"":""EngineHeatRate"",""Value"":2.08,""OriginalValue"":1.3,
""LessIsGood"":1}]}},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_overcharge_size4_class5"",""On"":true,""Priority"":0,""Health"":1.0,""Value"":1883794}
,{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size4_class2"",""On"":true,""Priority"":0,""Health"":1.0,""Value"":28373},{""Slot"":""PowerDistributor"",
""Item"":""int_powerdistributor_size6_class5"",""On"":true,""Priority"":0,""Health"":1.0,""Value"":3475688,""Engineering"":{""Engineer"":""The Dweller"",
""EngineerID"":300180,""BlueprintID"":128673739,""BlueprintName"":""PowerDistributor_HighFrequency"",""Level"":5,""Quality"":0.2822,""Modifiers"":[{""Label"":""WeaponsCapacity"",
""Value"":47.5,""OriginalValue"":50.0,""LessIsGood"":0},{""Label"":""WeaponsRecharge"",""Value"":7.20408,""OriginalValue"":5.2,""LessIsGood"":0},{""Label"":""EnginesCapacity"",
""Value"":33.25,""OriginalValue"":35.0,""LessIsGood"":0},{""Label"":""EnginesRecharge"",""Value"":4.43328,""OriginalValue"":3.2,""LessIsGood"":0},{""Label"":""SystemsCapacity"",
""Value"":33.25,""OriginalValue"":35.0,""LessIsGood"":0},{""Label"":""SystemsRecharge"",""Value"":4.43328,""OriginalValue"":3.2,""LessIsGood"":0}]}},
{""Slot"":""Radar"",""Item"":""int_sensors_size4_class2"",""On"":true,""Priority"":0,""Health"":1.0,""Value"":28373,""Engineering"":{""Engineer"":""Lei Cheung"",
""EngineerID"":300120,""BlueprintID"":128740136,""BlueprintName"":""Sensor_LongRange"",""Level"":5,""Quality"":0.3273,""Modifiers"":[{""Label"":""Mass"",
""Value"":8.0,""OriginalValue"":4.0,""LessIsGood"":1},{""Label"":""SensorTargetScanAngle"",""Value"":21.0,""OriginalValue"":30.0,""LessIsGood"":0},{""Label"":""Range"",
""Value"":8311.463867,""OriginalValue"":5040.0,""LessIsGood"":0}]}},{""Slot"":""FuelTank"",""Item"":""int_fueltank_size3_class3"",""On"":true,""Priority"":1,
""Health"":1.0,""Value"":7063},{""Slot"":""Slot01_Size5"",""Item"":""int_shieldgenerator_size5_class5"",""On"":true,""Priority"":0,""Health"":1.0,""Value"":4338361,
""Engineering"":{""Engineer"":""Lei Cheung"",""EngineerID"":300120,""BlueprintID"":128673838,""BlueprintName"":""ShieldGenerator_Reinforced"",""Level"":4,
""Quality"":0.9617,""Modifiers"":[{""Label"":""ShieldGenStrength"",""Value"":158.124008,""OriginalValue"":120.000008,""LessIsGood"":0},{""Label"":""BrokenRegenRate"",
""Value"":3.375,""OriginalValue"":3.75,""LessIsGood"":0},{""Label"":""EnergyPerRegen"",""Value"":0.66,""OriginalValue"":0.6,""LessIsGood"":1},{""Label"":""KineticResistance"",
""Value"":48.051994,""OriginalValue"":39.999996,""LessIsGood"":0},{""Label"":""ThermicResistance"",""Value"":-3.89601,""OriginalValue"":-20.000004,
""LessIsGood"":0},{""Label"":""ExplosiveResistance"",""Value"":56.709999,""OriginalValue"":50.0,""LessIsGood"":0}]}},{""Slot"":""Slot02_Size4"",""Item"":""int_shieldcellbank_size4_class4"",
""On"":true,""Priority"":3,""AmmoInClip"":1,""AmmoInHopper"":4,""Health"":1.0,""Value"":177331},{""Slot"":""Slot03_Size4"",""Item"":""int_cargorack_size4_class1"",
""On"":true,""Priority"":1,""Health"":1.0,""Value"":33470},{""Slot"":""Slot04_Size2"",""Item"":""int_buggybay_size2_class1"",""On"":true,""Priority"":0,""Health"":1.0,
""Value"":17550},{""Slot"":""Slot05_Size1"",""Item"":""int_dronecontrol_collection_size1_class5"",""On"":true,""Priority"":0,""Health"":1.0,""Value"":9360}
,{""Slot"":""Slot06_Size1"",""Item"":""int_dronecontrol_collection_size1_class5"",""On"":true,""Priority"":0,""Health"":1.0,""Value"":9360},{""Slot"":""PlanetaryApproachSuite"",
""Item"":""int_planetapproachsuite_advanced"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""WeaponColour"",""Item"":""weaponcustomisation_red"",
""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""VesselVoice"",""Item"":""voicepack_verity"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""ShipCockpit"",
""Item"":""ferdelance_cockpit"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoorfdl"",""On"":true,""Priority"":4,
""Health"":1.0}]}";

string viper =
            @"{""timestamp"":""2026-09-20T17:22:18Z"",""event"":""Loadout"",""Ship"":""viper_mkiv"",""ShipID"":52,""ShipName"":""VENOM"",""ShipIdent"":""RXP-11"",""HullValue"":282922,
""ModulesValue"":6442385,""HullHealth"":1.0,""UnladenMass"":274.5,""CargoCapacity"":0,""MaxJumpRange"":20.86791,""FuelCapacity"":{""Main"":16.0,""Reserve"":0.46}
,""Rebuy"":252200,""Modules"":[{""Slot"":""MediumHardpoint1"",""Item"":""hpt_beamlaser_gimbal_medium"",""On"":true,""Priority"":0,""Health"":1.0,""Value"":439277}
,{""Slot"":""MediumHardpoint2"",""Item"":""hpt_beamlaser_fixed_medium"",""On"":true,""Priority"":0,""Health"":1.0,""Value"":262829},{""Slot"":""SmallHardpoint1"",
""Item"":""hpt_cannon_fixed_small"",""On"":true,""Priority"":0,""AmmoInClip"":5,""AmmoInHopper"":120,""Health"":1.0,""Value"":18516},{""Slot"":""SmallHardpoint2"",
""Item"":""hpt_cannon_fixed_small"",""On"":true,""Priority"":0,""AmmoInClip"":5,""AmmoInHopper"":120,""Health"":1.0,""Value"":18516},{""Slot"":""TinyHardpoint1"",
""Item"":""hpt_shieldbooster_size0_class5"",""On"":true,""Priority"":0,""Health"":1.0,""Value"":246578},{""Slot"":""TinyHardpoint2"",""Item"":""hpt_shieldbooster_size0_class4"",
""On"":true,""Priority"":0,""Health"":1.0,""Value"":107055},{""Slot"":""Armour"",""Item"":""viper_mkiv_armour_grade1"",""On"":true,""Priority"":1,""Health"":1.0}
,{""Slot"":""PowerPlant"",""Item"":""int_powerplant_size4_class5"",""On"":true,""Priority"":1,""Health"":1.0,""Value"":1264683},{""Slot"":""MainEngines"",
""Item"":""int_engine_size4_class5"",""On"":true,""Priority"":0,""Health"":1.0,""Value"":1412846},{""Slot"":""FrameShiftDrive"",""Item"":""int_hyperdrive_overcharge_size4_class3"",
""On"":true,""Priority"":2,""Health"":1.0,""Value"":565139},{""Slot"":""LifeSupport"",""Item"":""int_lifesupport_size2_class1"",""On"":true,""Priority"":0,
""Health"":1.0,""Value"":1411},{""Slot"":""PowerDistributor"",""Item"":""int_powerdistributor_size3_class5"",""On"":true,""Priority"":0,""Health"":1.0,
""Value"":138936},{""Slot"":""Radar"",""Item"":""int_sensors_size3_class5"",""On"":true,""Priority"":0,""Health"":1.0,""Value"":138936},{""Slot"":""FuelTank"",
""Item"":""int_fueltank_size4_class3"",""On"":true,""Priority"":1,""Health"":1.0,""Value"":24115},{""Slot"":""Slot01_Size4"",""Item"":""int_shieldgenerator_size4_class5"",
""On"":true,""Priority"":0,""Health"":1.0,""Value"":1412846},{""Slot"":""Slot02_Size4"",""Item"":""int_modulereinforcement_size4_class2"",""On"":true,""Priority"":1,
""Health"":1.0,""Value"":171113},{""Slot"":""Slot03_Size3"",""Item"":""int_hullreinforcement_size3_class2"",""On"":true,""Priority"":1,""Health"":1.0,""Value"":73710}
,{""Slot"":""Slot04_Size2"",""Item"":""int_shieldcellbank_size2_class5"",""On"":true,""Priority"":1,""AmmoInClip"":1,""AmmoInHopper"":3,""Health"":1.0,
""Value"":49621},{""Slot"":""Slot07_Size1"",""Item"":""int_supercruiseassist"",""On"":true,""Priority"":2,""Health"":1.0,""Value"":8892},{""Slot"":""Slot08_Size1"",
""Item"":""int_dockingcomputer_advanced"",""On"":false,""Priority"":2,""Health"":1.0,""Value"":13169},{""Slot"":""Military01"",""Item"":""int_hullreinforcement_size3_class2"",
""On"":true,""Priority"":1,""Health"":1.0,""Value"":73710},{""Slot"":""PlanetaryApproachSuite"",""Item"":""int_planetapproachsuite_advanced"",""On"":true,
""Priority"":1,""Health"":1.0,""Value"":487},{""Slot"":""VesselVoice"",""Item"":""voicepack_verity"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""ShipCockpit"",
""Item"":""viper_mkiv_cockpit"",""On"":true,""Priority"":1,""Health"":1.0},{""Slot"":""CargoHatch"",""Item"":""modularcargobaydoor"",""On"":true,""Priority"":2,
""Health"":1.0}]}";


            smd.Font = new Font("Arial", 8.25f);
            smd.FontLarge = new Font("Arial", 10);
            smd.BoxBackColor1 = Color.FromArgb(255, 40, 40, 40);
            smd.BoxBackColor2 = Color.FromArgb(255, 80, 80, 80);
            
            ship = Ship.CreateFromLoadout(viper);
            Display();
            extPictureBox.ClickElement += ModuleDisplayClickElement;
            extCheckBoxPriority.CheckedChanged += (s, e) => { Display(); };
            extCheckBoxDPS.CheckedChanged += (s, e) => { Display(); };
            extCheckBoxMW.CheckedChanged += (s, e) => { Display(); };
            extCheckBoxHealth.CheckedChanged += (s, e) => { Display(); };
            extCheckBoxAmmo.CheckedChanged += (s, e) => { Display(); };
        }
        EliteDangerousCore.ShipModuleDisplay smd = new EliteDangerousCore.ShipModuleDisplay();
        Ship ship;

        void Display()
        {
            smd.DisplayPriority = extCheckBoxPriority.Checked;
            smd.DisplayDPS = extCheckBoxDPS.Checked;
            smd.DisplayMW = extCheckBoxMW.Checked;
            smd.DisplayHealth = extCheckBoxHealth.Checked;
            smd.DisplayAmmo = extCheckBoxAmmo.Checked;
            var images = smd.CreateImages(ship.GetShipProperties(), ship, new Point(0, 0), 1200, "Ship Module Test", true, true);
            extPictureBox.AddRange(images);
            extPictureBox.Render();
        }

        private void ModuleDisplayClickElement(object sender, MouseEventArgs e, ExtendedControls.ImageElement.Element i, object tag)
        {
            if (i != null)
            {
                if (tag is ShipModule sm)
                {
                    if (i.Name == "Enable")
                    {
                        sm.SetEnabled(sm.Enabled != true);
                        Display();
                    }
                    else if ( i.Name == "Priority")
                    {
                        sm.CyclePriority();
                        Display();
                    }
                }
            }

        }
    }
}
