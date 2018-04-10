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
 *
 * Data courtesy of Coriolis.IO https://github.com/EDCD/coriolis , data is intellectual property and copyright of Frontier Developments plc ('Frontier', 'Frontier Developments') and are subject to their terms and conditions.
 */

using System;
using System.Collections.Generic;

namespace EliteDangerousCore
{
    public class ShipModuleData
    {
        static ShipModuleData instance = null;

        private ShipModuleData()
        {
        }

        public static ShipModuleData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ShipModuleData();
                }
                return instance;
            }
        }

        public string NormaliseShipName(string fdshipname )        // fd seems to sometimes lower case it, convert it back to nominal
        {
            fdshipname = fdshipname.ToLower();
            foreach( string s in ships.Keys)
            {
                if (s.ToLower().Equals(fdshipname))
                    return s;
            }

            return fdshipname;
        }

        public Dictionary<string, ShipInfo> GetShipProperties(string fdshipname)        // get properties of a ship
        {
            return ships.ContainsKey(fdshipname) ? ships[fdshipname] : null;
        }

        public ShipInfo GetShipProperty(string fdshipname, string property)        // get property of a ship
        {
            return ships.ContainsKey(fdshipname) ? (ships[fdshipname].ContainsKey(property) ? ships[fdshipname][property] : null) : null;
        }

        public ShipModule GetModuleProperties(string fdname)        // get properties of a ship
        {
            fdname = fdname.ToLower();
            return modules.ContainsKey(fdname) ? modules[fdname] as ShipModule : null;
        }

        static public bool IsVanity(string ifd)
        {
            ifd = ifd.ToLower();
            foreach (var s in new[] { "bobble", "decal", "enginecustomisation", "nameplate", "paintjob",
                                    "shipkit", "weaponcustomisation", "voicepack" , "string_lights_coloured" })
            {
                if (ifd.Contains(s))
                    return true;       // no IDs
            }

            return false;
        }

        static public bool IsNotForExport(string ifd)
        {
            ifd = ifd.ToLower();
            foreach (var s in new[] { "cargobaydoor", "cockpit" })
            {
                if (ifd.Contains(s))
                    return true;       // no IDs
            }

            return false;
        }

        public int CalcID(string ifd , string fdshipname )            // -1 dont have one, 0 unknown, else code
        {
            //System.Diagnostics.Debug.WriteLine("Lookup " + inorm + ":" + ifd);

            ifd = ifd.ToLower();

            if ( ifd.Contains("armour_"))
            {
                if (ships.ContainsKey(fdshipname))
                {
                    Dictionary<string, ShipInfo> ship = ships[fdshipname];
                    int index = ifd.IndexOf("armour_");
                    string type = ifd.Substring(index + 7);

                    if (ship.ContainsKey(type))
                        return (ship[type] as ShipModule).moduleid;
                }

                return 0;
            }

            if (IsVanity(ifd) || IsNotForExport(ifd))
                return -1;

            if (modules.ContainsKey(ifd))
            {
                ShipModule m = modules[ifd] as ShipModule;
                return m.moduleid;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Not Found" + ifd);
            }

            return 0;
        }

        #region classes

        public class ShipInfo
        {
        };
        public class ShipModule : ShipInfo
        {
            public int moduleid;
            public double mass;
            public ShipModule(int id, double m) { moduleid = id; mass = m; }
        };
        public class ShipInfoString : ShipInfo
        {
            public string str;
            public ShipInfoString(string s) { str = s; }
        };
        public class ShipInfoInt : ShipInfo
        {
            public int value;
            public ShipInfoInt(int i) { value = i; }
        };
        public class ShipInfoDouble : ShipInfo
        {
            public double value;
            public ShipInfoDouble(double d) { value = d; }
        };

        #endregion

        #region Modules types to IDs

        //don't do this manually, its done using a corolis-data scanner in testnetlogentry!

        static Dictionary<string, ShipInfo> modules = new Dictionary<string, ShipInfo>
        {
            { "hpt_atdumbfiremissile_fixed_medium", new ShipModule(128788699,4)},
            { "hpt_atdumbfiremissile_turret_medium", new ShipModule(128788704,4)},
            { "hpt_atdumbfiremissile_fixed_large", new ShipModule(128788700,8)},
            { "hpt_atdumbfiremissile_turret_large", new ShipModule(128788705,8)},
            { "hpt_atmulticannon_fixed_medium", new ShipModule(128788701,4)},
            { "hpt_atmulticannon_turret_medium", new ShipModule(128793059,4)},
            { "hpt_atmulticannon_fixed_large", new ShipModule(128788702,8)},
            { "hpt_atmulticannon_turret_large", new ShipModule(128793060,8)},
            { "hpt_beamlaser_fixed_small", new ShipModule(128049428,2)},
            { "hpt_beamlaser_gimbal_small", new ShipModule(128049432,2)},
            { "hpt_beamlaser_turret_small", new ShipModule(128049435,2)},
            { "hpt_beamlaser_fixed_small_heat", new ShipModule(128671346,2)},
            { "hpt_beamlaser_fixed_medium", new ShipModule(128049429,4)},
            { "hpt_beamlaser_gimbal_medium", new ShipModule(128049433,4)},
            { "hpt_beamlaser_turret_medium", new ShipModule(128049436,4)},
            { "hpt_beamlaser_fixed_large", new ShipModule(128049430,8)},
            { "hpt_beamlaser_gimbal_large", new ShipModule(128049434,8)},
            { "hpt_beamlaser_turret_large", new ShipModule(128049437,8)},
            { "hpt_beamlaser_fixed_huge", new ShipModule(128049431,16)},
            { "hpt_beamlaser_gimbal_huge", new ShipModule(128681994,16)},
            { "hpt_pulselaserburst_fixed_small", new ShipModule(128049400,2)},
            { "hpt_pulselaserburst_gimbal_small", new ShipModule(128049404,2)},
            { "hpt_pulselaserburst_turret_small", new ShipModule(128049407,2)},
            { "hpt_pulselaserburst_fixed_small_scatter", new ShipModule(128671449,2)},
            { "hpt_pulselaserburst_fixed_medium", new ShipModule(128049401,4)},
            { "hpt_pulselaserburst_gimbal_medium", new ShipModule(128049405,4)},
            { "hpt_pulselaserburst_turret_medium", new ShipModule(128049408,4)},
            { "hpt_pulselaserburst_fixed_large", new ShipModule(128049402,8)},
            { "hpt_pulselaserburst_gimbal_large", new ShipModule(128049406,8)},
            { "hpt_pulselaserburst_turret_large", new ShipModule(128049409,8)},
            { "hpt_pulselaserburst_fixed_huge", new ShipModule(128049403,16)},
            { "hpt_pulselaserburst_gimbal_huge", new ShipModule(128727920,16)},
            { "hpt_cannon_fixed_small", new ShipModule(128049438,2)},
            { "hpt_cannon_gimbal_small", new ShipModule(128049442,2)},
            { "hpt_cannon_turret_small", new ShipModule(128049445,2)},
            { "hpt_cannon_fixed_medium", new ShipModule(128049439,4)},
            { "hpt_cannon_gimbal_medium", new ShipModule(128049443,4)},
            { "hpt_cannon_turret_medium", new ShipModule(128049446,4)},
            { "hpt_cannon_fixed_large", new ShipModule(128049440,8)},
            { "hpt_cannon_gimbal_large", new ShipModule(128671120,8)},
            { "hpt_cannon_turret_large", new ShipModule(128049447,8)},
            { "hpt_cannon_fixed_huge", new ShipModule(128049441,16)},
            { "hpt_cannon_gimbal_huge", new ShipModule(128049444,16)},
            { "hpt_cargoscanner_size0_class1", new ShipModule(128662520,1.3)},
            { "hpt_cargoscanner_size0_class2", new ShipModule(128662521,1.3)},
            { "hpt_cargoscanner_size0_class3", new ShipModule(128662522,1.3)},
            { "hpt_cargoscanner_size0_class4", new ShipModule(128662523,1.3)},
            { "hpt_cargoscanner_size0_class5", new ShipModule(128662524,1.3)},
            { "hpt_chafflauncher_tiny", new ShipModule(128049513,1.3)},
            { "hpt_electroniccountermeasure_tiny", new ShipModule(128049516,1.3)},
            { "hpt_slugshot_fixed_small", new ShipModule(128049448,2)},
            { "hpt_slugshot_gimbal_small", new ShipModule(128049451,2)},
            { "hpt_slugshot_turret_small", new ShipModule(128049453,2)},
            { "hpt_slugshot_fixed_medium", new ShipModule(128049449,4)},
            { "hpt_slugshot_gimbal_medium", new ShipModule(128049452,4)},
            { "hpt_slugshot_turret_medium", new ShipModule(128049454,4)},
            { "hpt_slugshot_fixed_large", new ShipModule(128049450,8)},
            { "hpt_slugshot_gimbal_large", new ShipModule(128671321,8)},
            { "hpt_slugshot_turret_large", new ShipModule(128671322,8)},
            { "hpt_slugshot_fixed_large_range", new ShipModule(128671343,8)},
            { "hpt_cloudscanner_size0_class1", new ShipModule(128662525,1.3)},
            { "hpt_cloudscanner_size0_class2", new ShipModule(128662526,1.3)},
            { "hpt_cloudscanner_size0_class3", new ShipModule(128662527,1.3)},
            { "hpt_cloudscanner_size0_class4", new ShipModule(128662528,1.3)},
            { "hpt_cloudscanner_size0_class5", new ShipModule(128662529,1.3)},
            { "hpt_heatsinklauncher_turret_tiny", new ShipModule(128049519,1.3)},
            { "hpt_crimescanner_size0_class1", new ShipModule(128662530,1.3)},
            { "hpt_crimescanner_size0_class2", new ShipModule(128662531,1.3)},
            { "hpt_crimescanner_size0_class3", new ShipModule(128662532,1.3)},
            { "hpt_crimescanner_size0_class4", new ShipModule(128662533,1.3)},
            { "hpt_crimescanner_size0_class5", new ShipModule(128662534,1.3)},
            { "hpt_minelauncher_fixed_small", new ShipModule(128049500,2)},
            { "hpt_minelauncher_fixed_small_impulse", new ShipModule(128671448,2)},
            { "hpt_minelauncher_fixed_medium", new ShipModule(128049501,4)},
            { "hpt_mininglaser_fixed_small", new ShipModule(128049525,2)},
            { "hpt_mininglaser_turret_small", new ShipModule(128740819,2)},
            { "hpt_mininglaser_fixed_small_advanced", new ShipModule(128671347,2)},
            { "hpt_mininglaser_fixed_medium", new ShipModule(128049526,2)},
            { "hpt_mininglaser_turret_medium", new ShipModule(128740820,2)},
            { "hpt_dumbfiremissilerack_fixed_small", new ShipModule(128666724,2)},
            { "hpt_basicmissilerack_fixed_small", new ShipModule(128049492,2)},
            { "hpt_dumbfiremissilerack_fixed_medium", new ShipModule(128666725,4)},
            { "hpt_basicmissilerack_fixed_medium", new ShipModule(128049493,4)},
            { "hpt_dumbfiremissilerack_fixed_medium_lasso", new ShipModule(128732552,4)},
            { "hpt_drunkmissilerack_fixed_medium", new ShipModule(128671344,4)},
            { "hpt_multicannon_fixed_small", new ShipModule(128049455,2)},
            { "hpt_multicannon_gimbal_small", new ShipModule(128049459,2)},
            { "hpt_multicannon_turret_small", new ShipModule(128049462,2)},
            { "hpt_multicannon_fixed_small_scatter", new ShipModule(128671345,2)},
            { "hpt_multicannon_fixed_medium", new ShipModule(128049456,4)},
            { "hpt_multicannon_gimbal_medium", new ShipModule(128049460,4)},
            { "hpt_multicannon_turret_medium", new ShipModule(128049463,4)},
            { "hpt_multicannon_fixed_large", new ShipModule(128049457,8)},
            { "hpt_multicannon_gimbal_large", new ShipModule(128049461,8)},
            { "hpt_multicannon_fixed_huge", new ShipModule(128049458,16)},
            { "hpt_multicannon_gimbal_huge", new ShipModule(128681996,16)},
            { "hpt_plasmaaccelerator_fixed_medium", new ShipModule(128049465,4)},
            { "hpt_plasmaaccelerator_fixed_large", new ShipModule(128049466,8)},
            { "hpt_plasmaaccelerator_fixed_large_advanced", new ShipModule(128671339,8)},
            { "hpt_plasmaaccelerator_fixed_huge", new ShipModule(128049467,16)},
            { "hpt_plasmapointdefence_turret_tiny", new ShipModule(128049522,0.5)},
            { "hpt_pulselaser_fixed_small", new ShipModule(128049381,2)},
            { "hpt_pulselaser_gimbal_small", new ShipModule(128049385,2)},
            { "hpt_pulselaser_turret_small", new ShipModule(128049388,2)},
            { "hpt_pulselaser_fixed_medium", new ShipModule(128049382,4)},
            { "hpt_pulselaser_gimbal_medium", new ShipModule(128049386,4)},
            { "hpt_pulselaser_turret_medium", new ShipModule(128049389,4)},
            { "hpt_pulselaser_fixed_medium_distruptor", new ShipModule(128671342,4)},
            { "hpt_pulselaser_fixed_large", new ShipModule(128049383,8)},
            { "hpt_pulselaser_gimbal_large", new ShipModule(128049387,8)},
            { "hpt_pulselaser_turret_large", new ShipModule(128049390,8)},
            { "hpt_pulselaser_fixed_huge", new ShipModule(128049384,16)},
            { "hpt_pulselaser_gimbal_huge", new ShipModule(128681995,16)},
            { "hpt_railgun_fixed_small", new ShipModule(128049488,2)},
            { "hpt_railgun_fixed_medium", new ShipModule(128049489,4)},
            { "hpt_railgun_fixed_medium_burst", new ShipModule(128671341,4)},
            { "hpt_flakmortar_fixed_medium", new ShipModule(128785626,4)},
            { "hpt_flakmortar_turret_medium", new ShipModule(128793058,4)},
            { "hpt_shieldbooster_size0_class1", new ShipModule(128668532,0.5)},
            { "hpt_shieldbooster_size0_class2", new ShipModule(128668533,1)},
            { "hpt_shieldbooster_size0_class3", new ShipModule(128668534,2)},
            { "hpt_shieldbooster_size0_class4", new ShipModule(128668535,3)},
            { "hpt_shieldbooster_size0_class5", new ShipModule(128668536,3.5)},
            { "hpt_antiunknownshutdown_tiny", new ShipModule(128771884,1.3)},
            { "hpt_advancedtorppylon_fixed_small", new ShipModule(128049509,2)},
            { "hpt_advancedtorppylon_fixed_medium", new ShipModule(128049510,4)},
            { "hpt_xenoscanner_basic_tiny", new ShipModule(128793115,1.3)},
            { "int_repairer_size1_class1", new ShipModule(128667598,0)},
            { "int_repairer_size1_class2", new ShipModule(128667606,0)},
            { "int_repairer_size1_class3", new ShipModule(128667614,0)},
            { "int_repairer_size1_class4", new ShipModule(128667622,0)},
            { "int_repairer_size1_class5", new ShipModule(128667630,0)},
            { "int_repairer_size2_class1", new ShipModule(128667599,0)},
            { "int_repairer_size2_class2", new ShipModule(128667607,0)},
            { "int_repairer_size2_class3", new ShipModule(128667615,0)},
            { "int_repairer_size2_class4", new ShipModule(128667623,0)},
            { "int_repairer_size2_class5", new ShipModule(128667631,0)},
            { "int_repairer_size3_class1", new ShipModule(128667600,0)},
            { "int_repairer_size3_class2", new ShipModule(128667608,0)},
            { "int_repairer_size3_class3", new ShipModule(128667616,0)},
            { "int_repairer_size3_class4", new ShipModule(128667624,0)},
            { "int_repairer_size3_class5", new ShipModule(128667632,0)},
            { "int_repairer_size4_class1", new ShipModule(128667601,0)},
            { "int_repairer_size4_class2", new ShipModule(128667609,0)},
            { "int_repairer_size4_class3", new ShipModule(128667617,0)},
            { "int_repairer_size4_class4", new ShipModule(128667625,0)},
            { "int_repairer_size4_class5", new ShipModule(128667633,0)},
            { "int_repairer_size5_class1", new ShipModule(128667602,0)},
            { "int_repairer_size5_class2", new ShipModule(128667610,0)},
            { "int_repairer_size5_class3", new ShipModule(128667618,0)},
            { "int_repairer_size5_class4", new ShipModule(128667626,0)},
            { "int_repairer_size5_class5", new ShipModule(128667634,0)},
            { "int_repairer_size6_class1", new ShipModule(128667603,0)},
            { "int_repairer_size6_class2", new ShipModule(128667611,0)},
            { "int_repairer_size6_class3", new ShipModule(128667619,0)},
            { "int_repairer_size6_class4", new ShipModule(128667627,0)},
            { "int_repairer_size6_class5", new ShipModule(128667635,0)},
            { "int_repairer_size7_class1", new ShipModule(128667604,0)},
            { "int_repairer_size7_class2", new ShipModule(128667612,0)},
            { "int_repairer_size7_class3", new ShipModule(128667620,0)},
            { "int_repairer_size7_class4", new ShipModule(128667628,0)},
            { "int_repairer_size7_class5", new ShipModule(128667636,0)},
            { "int_repairer_size8_class1", new ShipModule(128667605,0)},
            { "int_repairer_size8_class2", new ShipModule(128667613,0)},
            { "int_repairer_size8_class3", new ShipModule(128667621,0)},
            { "int_repairer_size8_class4", new ShipModule(128667629,0)},
            { "int_repairer_size8_class5", new ShipModule(128667637,0)},
            { "int_shieldgenerator_size1_class3_fast", new ShipModule(128671331,1.3)},
            { "int_shieldgenerator_size2_class3_fast", new ShipModule(128671332,2.5)},
            { "int_shieldgenerator_size3_class3_fast", new ShipModule(128671333,5)},
            { "int_shieldgenerator_size4_class3_fast", new ShipModule(128671334,10)},
            { "int_shieldgenerator_size5_class3_fast", new ShipModule(128671335,20)},
            { "int_shieldgenerator_size6_class3_fast", new ShipModule(128671336,40)},
            { "int_shieldgenerator_size7_class3_fast", new ShipModule(128671337,80)},
            { "int_shieldgenerator_size8_class3_fast", new ShipModule(128671338,160)},
            { "int_passengercabin_size3_class2", new ShipModule(128734692,5)},
            { "int_passengercabin_size4_class2", new ShipModule(128727923,10)},
            { "int_passengercabin_size5_class2", new ShipModule(128734694,20)},
            { "int_passengercabin_size6_class2", new ShipModule(128727927,40)},
            { "int_cargorack_size1_class1", new ShipModule(128064338,0)},
            { "int_cargorack_size2_class1", new ShipModule(128064339,0)},
            { "int_cargorack_size3_class1", new ShipModule(128064340,0)},
            { "int_cargorack_size4_class1", new ShipModule(128064341,0)},
            { "int_cargorack_size5_class1", new ShipModule(128064342,0)},
            { "int_cargorack_size6_class1", new ShipModule(128064343,0)},
            { "int_cargorack_size7_class1", new ShipModule(128064344,0)},
            { "int_cargorack_size8_class1", new ShipModule(128064345,0)},
            { "int_corrosionproofcargorack_size1_class1", new ShipModule(128681641,0)},
            { "int_corrosionproofcargorack_size1_class2", new ShipModule(128681992,0)},
            { "int_dronecontrol_collection_size1_class1", new ShipModule(128671229,0.5)},
            { "int_dronecontrol_collection_size1_class2", new ShipModule(128671230,0.5)},
            { "int_dronecontrol_collection_size1_class3", new ShipModule(128671231,1.3)},
            { "int_dronecontrol_collection_size1_class4", new ShipModule(128671232,2)},
            { "int_dronecontrol_collection_size1_class5", new ShipModule(128671233,2)},
            { "int_dronecontrol_collection_size3_class1", new ShipModule(128671234,2)},
            { "int_dronecontrol_collection_size3_class2", new ShipModule(128671235,2)},
            { "int_dronecontrol_collection_size3_class3", new ShipModule(128671236,5)},
            { "int_dronecontrol_collection_size3_class4", new ShipModule(128671237,8)},
            { "int_dronecontrol_collection_size3_class5", new ShipModule(128671238,8)},
            { "int_dronecontrol_collection_size5_class1", new ShipModule(128671239,8)},
            { "int_dronecontrol_collection_size5_class2", new ShipModule(128671240,8)},
            { "int_dronecontrol_collection_size5_class3", new ShipModule(128671241,20)},
            { "int_dronecontrol_collection_size5_class4", new ShipModule(128671242,32)},
            { "int_dronecontrol_collection_size5_class5", new ShipModule(128671243,32)},
            { "int_dronecontrol_collection_size7_class1", new ShipModule(128671244,32)},
            { "int_dronecontrol_collection_size7_class2", new ShipModule(128671245,32)},
            { "int_dronecontrol_collection_size7_class3", new ShipModule(128671246,80)},
            { "int_dronecontrol_collection_size7_class4", new ShipModule(128671247,128)},
            { "int_dronecontrol_collection_size7_class5", new ShipModule(128671248,128)},
            { "int_dockingcomputer_standard", new ShipModule(128049549,0)},
            { "int_passengercabin_size2_class1", new ShipModule(128734690,2.5)},
            { "int_passengercabin_size3_class1", new ShipModule(128734691,5)},
            { "int_passengercabin_size4_class1", new ShipModule(128727922,10)},
            { "int_passengercabin_size5_class1", new ShipModule(128734693,20)},
            { "int_passengercabin_size6_class1", new ShipModule(128727926,40)},
            { "int_fighterbay_size5_class1", new ShipModule(128727930,20)},
            { "int_fighterbay_size6_class1", new ShipModule(128727931,40)},
            { "int_fighterbay_size7_class1", new ShipModule(128727932,60)},
            { "int_passengercabin_size4_class3", new ShipModule(128727924,10)},
            { "int_passengercabin_size5_class3", new ShipModule(128734695,20)},
            { "int_passengercabin_size6_class3", new ShipModule(128727928,40)},
            { "int_fsdinterdictor_size1_class1", new ShipModule(128666704,1.3)},
            { "int_fsdinterdictor_size1_class2", new ShipModule(128666708,0.5)},
            { "int_fsdinterdictor_size1_class3", new ShipModule(128666712,1.3)},
            { "int_fsdinterdictor_size1_class4", new ShipModule(128666716,2)},
            { "int_fsdinterdictor_size1_class5", new ShipModule(128666720,1.3)},
            { "int_fsdinterdictor_size2_class1", new ShipModule(128666705,2.5)},
            { "int_fsdinterdictor_size2_class2", new ShipModule(128666709,1)},
            { "int_fsdinterdictor_size2_class3", new ShipModule(128666713,2.5)},
            { "int_fsdinterdictor_size2_class4", new ShipModule(128666717,4)},
            { "int_fsdinterdictor_size2_class5", new ShipModule(128666721,2.5)},
            { "int_fsdinterdictor_size3_class1", new ShipModule(128666706,5)},
            { "int_fsdinterdictor_size3_class2", new ShipModule(128666710,2)},
            { "int_fsdinterdictor_size3_class3", new ShipModule(128666714,5)},
            { "int_fsdinterdictor_size3_class4", new ShipModule(128666718,8)},
            { "int_fsdinterdictor_size3_class5", new ShipModule(128666722,5)},
            { "int_fsdinterdictor_size4_class1", new ShipModule(128666707,10)},
            { "int_fsdinterdictor_size4_class2", new ShipModule(128666711,4)},
            { "int_fsdinterdictor_size4_class3", new ShipModule(128666715,10)},
            { "int_fsdinterdictor_size4_class4", new ShipModule(128666719,16)},
            { "int_fsdinterdictor_size4_class5", new ShipModule(128666723,10)},
            { "int_fuelscoop_size1_class1", new ShipModule(128666644,0)},
            { "int_fuelscoop_size1_class2", new ShipModule(128666652,0)},
            { "int_fuelscoop_size1_class3", new ShipModule(128666660,0)},
            { "int_fuelscoop_size1_class4", new ShipModule(128666668,0)},
            { "int_fuelscoop_size1_class5", new ShipModule(128666676,0)},
            { "int_fuelscoop_size2_class1", new ShipModule(128666645,0)},
            { "int_fuelscoop_size2_class2", new ShipModule(128666653,0)},
            { "int_fuelscoop_size2_class3", new ShipModule(128666661,0)},
            { "int_fuelscoop_size2_class4", new ShipModule(128666669,0)},
            { "int_fuelscoop_size2_class5", new ShipModule(128666677,0)},
            { "int_fuelscoop_size3_class1", new ShipModule(128666646,0)},
            { "int_fuelscoop_size3_class2", new ShipModule(128666654,0)},
            { "int_fuelscoop_size3_class3", new ShipModule(128666662,0)},
            { "int_fuelscoop_size3_class4", new ShipModule(128666670,0)},
            { "int_fuelscoop_size3_class5", new ShipModule(128666678,0)},
            { "int_fuelscoop_size4_class1", new ShipModule(128666647,0)},
            { "int_fuelscoop_size4_class2", new ShipModule(128666655,0)},
            { "int_fuelscoop_size4_class3", new ShipModule(128666663,0)},
            { "int_fuelscoop_size4_class4", new ShipModule(128666671,0)},
            { "int_fuelscoop_size4_class5", new ShipModule(128666679,0)},
            { "int_fuelscoop_size5_class1", new ShipModule(128666648,0)},
            { "int_fuelscoop_size5_class2", new ShipModule(128666656,0)},
            { "int_fuelscoop_size5_class3", new ShipModule(128666664,0)},
            { "int_fuelscoop_size5_class4", new ShipModule(128666672,0)},
            { "int_fuelscoop_size5_class5", new ShipModule(128666680,0)},
            { "int_fuelscoop_size6_class1", new ShipModule(128666649,0)},
            { "int_fuelscoop_size6_class2", new ShipModule(128666657,0)},
            { "int_fuelscoop_size6_class3", new ShipModule(128666665,0)},
            { "int_fuelscoop_size6_class4", new ShipModule(128666673,0)},
            { "int_fuelscoop_size6_class5", new ShipModule(128666681,0)},
            { "int_fuelscoop_size7_class1", new ShipModule(128666650,0)},
            { "int_fuelscoop_size7_class2", new ShipModule(128666658,0)},
            { "int_fuelscoop_size7_class3", new ShipModule(128666666,0)},
            { "int_fuelscoop_size7_class4", new ShipModule(128666674,0)},
            { "int_fuelscoop_size7_class5", new ShipModule(128666682,0)},
            { "int_fuelscoop_size8_class1", new ShipModule(128666651,0)},
            { "int_fuelscoop_size8_class2", new ShipModule(128666659,0)},
            { "int_fuelscoop_size8_class3", new ShipModule(128666667,0)},
            { "int_fuelscoop_size8_class4", new ShipModule(128666675,0)},
            { "int_fuelscoop_size8_class5", new ShipModule(128666683,0)},
            { "int_dronecontrol_fueltransfer_size1_class1", new ShipModule(128671249,1.3)},
            { "int_dronecontrol_fueltransfer_size1_class2", new ShipModule(128671250,0.5)},
            { "int_dronecontrol_fueltransfer_size1_class3", new ShipModule(128671251,1.3)},
            { "int_dronecontrol_fueltransfer_size1_class4", new ShipModule(128671252,2)},
            { "int_dronecontrol_fueltransfer_size1_class5", new ShipModule(128671253,1.3)},
            { "int_dronecontrol_fueltransfer_size3_class1", new ShipModule(128671254,5)},
            { "int_dronecontrol_fueltransfer_size3_class2", new ShipModule(128671255,2)},
            { "int_dronecontrol_fueltransfer_size3_class3", new ShipModule(128671256,5)},
            { "int_dronecontrol_fueltransfer_size3_class4", new ShipModule(128671257,8)},
            { "int_dronecontrol_fueltransfer_size3_class5", new ShipModule(128671258,5)},
            { "int_dronecontrol_fueltransfer_size5_class1", new ShipModule(128671259,20)},
            { "int_dronecontrol_fueltransfer_size5_class2", new ShipModule(128671260,8)},
            { "int_dronecontrol_fueltransfer_size5_class3", new ShipModule(128671261,20)},
            { "int_dronecontrol_fueltransfer_size5_class4", new ShipModule(128671262,32)},
            { "int_dronecontrol_fueltransfer_size5_class5", new ShipModule(128671263,20)},
            { "int_dronecontrol_fueltransfer_size7_class1", new ShipModule(128671264,80)},
            { "int_dronecontrol_fueltransfer_size7_class2", new ShipModule(128671265,32)},
            { "int_dronecontrol_fueltransfer_size7_class3", new ShipModule(128671266,80)},
            { "int_dronecontrol_fueltransfer_size7_class4", new ShipModule(128671267,128)},
            { "int_dronecontrol_fueltransfer_size7_class5", new ShipModule(128671268,80)},
            { "int_dronecontrol_resourcesiphon_size1_class1", new ShipModule(128066532,1.3)},
            { "int_dronecontrol_resourcesiphon_size1_class2", new ShipModule(128066533,0.5)},
            { "int_dronecontrol_resourcesiphon_size1_class3", new ShipModule(128066534,1.3)},
            { "int_dronecontrol_resourcesiphon_size1_class4", new ShipModule(128066535,2)},
            { "int_dronecontrol_resourcesiphon_size1_class5", new ShipModule(128066536,1.3)},
            { "int_dronecontrol_resourcesiphon_size3_class1", new ShipModule(128066537,5)},
            { "int_dronecontrol_resourcesiphon_size3_class2", new ShipModule(128066538,2)},
            { "int_dronecontrol_resourcesiphon_size3_class3", new ShipModule(128066539,5)},
            { "int_dronecontrol_resourcesiphon_size3_class4", new ShipModule(128066540,8)},
            { "int_dronecontrol_resourcesiphon_size3_class5", new ShipModule(128066541,5)},
            { "int_dronecontrol_resourcesiphon_size5_class1", new ShipModule(128066542,20)},
            { "int_dronecontrol_resourcesiphon_size5_class2", new ShipModule(128066543,8)},
            { "int_dronecontrol_resourcesiphon_size5_class3", new ShipModule(128066544,20)},
            { "int_dronecontrol_resourcesiphon_size5_class4", new ShipModule(128066545,32)},
            { "int_dronecontrol_resourcesiphon_size5_class5", new ShipModule(128066546,20)},
            { "int_dronecontrol_resourcesiphon_size7_class1", new ShipModule(128066547,80)},
            { "int_dronecontrol_resourcesiphon_size7_class2", new ShipModule(128066548,32)},
            { "int_dronecontrol_resourcesiphon_size7_class3", new ShipModule(128066549,80)},
            { "int_dronecontrol_resourcesiphon_size7_class4", new ShipModule(128066550,128)},
            { "int_dronecontrol_resourcesiphon_size7_class5", new ShipModule(128066551,90)},
            { "int_hullreinforcement_size1_class1", new ShipModule(128668537,2)},
            { "int_hullreinforcement_size1_class2", new ShipModule(128668538,1)},
            { "int_hullreinforcement_size2_class1", new ShipModule(128668539,4)},
            { "int_hullreinforcement_size2_class2", new ShipModule(128668540,2)},
            { "int_hullreinforcement_size3_class1", new ShipModule(128668541,8)},
            { "int_hullreinforcement_size3_class2", new ShipModule(128668542,4)},
            { "int_hullreinforcement_size4_class1", new ShipModule(128668543,16)},
            { "int_hullreinforcement_size4_class2", new ShipModule(128668544,8)},
            { "int_hullreinforcement_size5_class1", new ShipModule(128668545,32)},
            { "int_hullreinforcement_size5_class2", new ShipModule(128668546,16)},
            { "int_fueltank_size1_class3", new ShipModule(128064346,0)},
            { "int_fueltank_size2_class3", new ShipModule(128064347,0)},
            { "int_fueltank_size3_class3", new ShipModule(128064348,0)},
            { "int_fueltank_size4_class3", new ShipModule(128064349,0)},
            { "int_fueltank_size5_class3", new ShipModule(128064350,0)},
            { "int_fueltank_size6_class3", new ShipModule(128064351,0)},
            { "int_fueltank_size7_class3", new ShipModule(128064352,0)},
            { "int_fueltank_size8_class3", new ShipModule(128064353,0)},
            { "int_passengercabin_size5_class4", new ShipModule(128727925,20)},
            { "int_passengercabin_size6_class4", new ShipModule(128727929,40)},
            { "int_modulereinforcement_size1_class1", new ShipModule(128737270,2)},
            { "int_modulereinforcement_size1_class2", new ShipModule(128737271,1)},
            { "int_modulereinforcement_size2_class1", new ShipModule(128737272,4)},
            { "int_modulereinforcement_size2_class2", new ShipModule(128737273,2)},
            { "int_modulereinforcement_size3_class1", new ShipModule(128737274,8)},
            { "int_modulereinforcement_size3_class2", new ShipModule(128737275,4)},
            { "int_modulereinforcement_size4_class1", new ShipModule(128737276,16)},
            { "int_modulereinforcement_size4_class2", new ShipModule(128737277,8)},
            { "int_modulereinforcement_size5_class1", new ShipModule(128737278,32)},
            { "int_modulereinforcement_size5_class2", new ShipModule(128737279,16)},
            { "int_buggybay_size2_class1", new ShipModule(128672288,12)},
            { "int_buggybay_size2_class2", new ShipModule(128672289,6)},
            { "int_buggybay_size4_class1", new ShipModule(128672290,20)},
            { "int_buggybay_size4_class2", new ShipModule(128672291,10)},
            { "int_buggybay_size6_class1", new ShipModule(128672292,34)},
            { "int_buggybay_size6_class2", new ShipModule(128672293,17)},
            { "int_shieldgenerator_size1_class5_strong", new ShipModule(128671323,2.5)},
            { "int_shieldgenerator_size2_class5_strong", new ShipModule(128671324,5)},
            { "int_shieldgenerator_size3_class5_strong", new ShipModule(128671325,10)},
            { "int_shieldgenerator_size4_class5_strong", new ShipModule(128671326,20)},
            { "int_shieldgenerator_size5_class5_strong", new ShipModule(128671327,40)},
            { "int_shieldgenerator_size6_class5_strong", new ShipModule(128671328,80)},
            { "int_shieldgenerator_size7_class5_strong", new ShipModule(128671329,160)},
            { "int_shieldgenerator_size8_class5_strong", new ShipModule(128671330,320)},
            { "int_dronecontrol_prospector_size1_class1", new ShipModule(128671269,1.3)},
            { "int_dronecontrol_prospector_size1_class2", new ShipModule(128671270,0.5)},
            { "int_dronecontrol_prospector_size1_class3", new ShipModule(128671271,1.3)},
            { "int_dronecontrol_prospector_size1_class4", new ShipModule(128671272,2)},
            { "int_dronecontrol_prospector_size1_class5", new ShipModule(128671273,1.3)},
            { "int_dronecontrol_prospector_size3_class1", new ShipModule(128671274,5)},
            { "int_dronecontrol_prospector_size3_class2", new ShipModule(128671275,2)},
            { "int_dronecontrol_prospector_size3_class3", new ShipModule(128671276,5)},
            { "int_dronecontrol_prospector_size3_class4", new ShipModule(128671277,8)},
            { "int_dronecontrol_prospector_size3_class5", new ShipModule(128671278,5)},
            { "int_dronecontrol_prospector_size5_class1", new ShipModule(128671279,20)},
            { "int_dronecontrol_prospector_size5_class2", new ShipModule(128671280,8)},
            { "int_dronecontrol_prospector_size5_class3", new ShipModule(128671281,20)},
            { "int_dronecontrol_prospector_size5_class4", new ShipModule(128671282,32)},
            { "int_dronecontrol_prospector_size5_class5", new ShipModule(128671283,20)},
            { "int_dronecontrol_prospector_size7_class1", new ShipModule(128671284,80)},
            { "int_dronecontrol_prospector_size7_class2", new ShipModule(128671285,32)},
            { "int_dronecontrol_prospector_size7_class3", new ShipModule(128671286,80)},
            { "int_dronecontrol_prospector_size7_class4", new ShipModule(128671287,128)},
            { "int_dronecontrol_prospector_size7_class5", new ShipModule(128671288,80)},
            { "int_refinery_size1_class1", new ShipModule(128666684,0)},
            { "int_refinery_size1_class2", new ShipModule(128666688,0)},
            { "int_refinery_size1_class3", new ShipModule(128666692,0)},
            { "int_refinery_size1_class4", new ShipModule(128666696,0)},
            { "int_refinery_size1_class5", new ShipModule(128666700,0)},
            { "int_refinery_size2_class1", new ShipModule(128666685,0)},
            { "int_refinery_size2_class2", new ShipModule(128666689,0)},
            { "int_refinery_size2_class3", new ShipModule(128666693,0)},
            { "int_refinery_size2_class4", new ShipModule(128666697,0)},
            { "int_refinery_size2_class5", new ShipModule(128666701,0)},
            { "int_refinery_size3_class1", new ShipModule(128666686,0)},
            { "int_refinery_size3_class2", new ShipModule(128666690,0)},
            { "int_refinery_size3_class3", new ShipModule(128666694,0)},
            { "int_refinery_size3_class4", new ShipModule(128666698,0)},
            { "int_refinery_size3_class5", new ShipModule(128666702,0)},
            { "int_refinery_size4_class1", new ShipModule(128666687,0)},
            { "int_refinery_size4_class2", new ShipModule(128666691,0)},
            { "int_refinery_size4_class3", new ShipModule(128666695,0)},
            { "int_refinery_size4_class4", new ShipModule(128666699,0)},
            { "int_refinery_size4_class5", new ShipModule(128666703,0)},
            { "int_dronecontrol_repair_size1_class1", new ShipModule(128777327,1.3)},
            { "int_dronecontrol_repair_size1_class2", new ShipModule(128777328,0.5)},
            { "int_dronecontrol_repair_size1_class3", new ShipModule(128777329,1.3)},
            { "int_dronecontrol_repair_size1_class4", new ShipModule(128777330,2)},
            { "int_dronecontrol_repair_size1_class5", new ShipModule(128777331,1.3)},
            { "int_dronecontrol_repair_size3_class1", new ShipModule(128777332,5)},
            { "int_dronecontrol_repair_size3_class2", new ShipModule(128777333,2)},
            { "int_dronecontrol_repair_size3_class3", new ShipModule(128777334,5)},
            { "int_dronecontrol_repair_size3_class4", new ShipModule(128777335,8)},
            { "int_dronecontrol_repair_size3_class5", new ShipModule(128777336,5)},
            { "int_dronecontrol_repair_size5_class1", new ShipModule(128777337,20)},
            { "int_dronecontrol_repair_size5_class2", new ShipModule(128777338,8)},
            { "int_dronecontrol_repair_size5_class3", new ShipModule(128777339,20)},
            { "int_dronecontrol_repair_size5_class4", new ShipModule(128777340,32)},
            { "int_dronecontrol_repair_size5_class5", new ShipModule(128777341,20)},
            { "int_dronecontrol_repair_size7_class1", new ShipModule(128777342,80)},
            { "int_dronecontrol_repair_size7_class2", new ShipModule(128777343,32)},
            { "int_dronecontrol_repair_size7_class3", new ShipModule(128777344,80)},
            { "int_dronecontrol_repair_size7_class4", new ShipModule(128777345,128)},
            { "int_dronecontrol_repair_size7_class5", new ShipModule(128777346,80)},
            { "int_stellarbodydiscoveryscanner_advanced", new ShipModule(128663561,2)},
            { "int_stellarbodydiscoveryscanner_intermediate", new ShipModule(128663560,2)},
            { "int_stellarbodydiscoveryscanner_standard", new ShipModule(128662535,2)},
            { "int_shieldcellbank_size1_class1", new ShipModule(128064298,1.3)},
            { "int_shieldcellbank_size1_class2", new ShipModule(128064299,0.5)},
            { "int_shieldcellbank_size1_class3", new ShipModule(128064300,1.3)},
            { "int_shieldcellbank_size1_class4", new ShipModule(128064301,2)},
            { "int_shieldcellbank_size1_class5", new ShipModule(128064302,1.3)},
            { "int_shieldcellbank_size2_class1", new ShipModule(128064303,2.5)},
            { "int_shieldcellbank_size2_class2", new ShipModule(128064304,1)},
            { "int_shieldcellbank_size2_class3", new ShipModule(128064305,2.5)},
            { "int_shieldcellbank_size2_class4", new ShipModule(128064306,4)},
            { "int_shieldcellbank_size2_class5", new ShipModule(128064307,2.5)},
            { "int_shieldcellbank_size3_class1", new ShipModule(128064308,5)},
            { "int_shieldcellbank_size3_class2", new ShipModule(128064309,2)},
            { "int_shieldcellbank_size3_class3", new ShipModule(128064310,5)},
            { "int_shieldcellbank_size3_class4", new ShipModule(128064311,8)},
            { "int_shieldcellbank_size3_class5", new ShipModule(128064312,5)},
            { "int_shieldcellbank_size4_class1", new ShipModule(128064313,10)},
            { "int_shieldcellbank_size4_class2", new ShipModule(128064314,4)},
            { "int_shieldcellbank_size4_class3", new ShipModule(128064315,10)},
            { "int_shieldcellbank_size4_class4", new ShipModule(128064316,16)},
            { "int_shieldcellbank_size4_class5", new ShipModule(128064317,10)},
            { "int_shieldcellbank_size5_class1", new ShipModule(128064318,20)},
            { "int_shieldcellbank_size5_class2", new ShipModule(128064319,8)},
            { "int_shieldcellbank_size5_class3", new ShipModule(128064320,20)},
            { "int_shieldcellbank_size5_class4", new ShipModule(128064321,32)},
            { "int_shieldcellbank_size5_class5", new ShipModule(128064322,20)},
            { "int_shieldcellbank_size6_class1", new ShipModule(128064323,40)},
            { "int_shieldcellbank_size6_class2", new ShipModule(128064324,16)},
            { "int_shieldcellbank_size6_class3", new ShipModule(128064325,40)},
            { "int_shieldcellbank_size6_class4", new ShipModule(128064326,64)},
            { "int_shieldcellbank_size6_class5", new ShipModule(128064327,40)},
            { "int_shieldcellbank_size7_class1", new ShipModule(128064328,80)},
            { "int_shieldcellbank_size7_class2", new ShipModule(128064329,32)},
            { "int_shieldcellbank_size7_class3", new ShipModule(128064330,80)},
            { "int_shieldcellbank_size7_class4", new ShipModule(128064331,128)},
            { "int_shieldcellbank_size7_class5", new ShipModule(128064332,80)},
            { "int_shieldcellbank_size8_class1", new ShipModule(128064333,160)},
            { "int_shieldcellbank_size8_class2", new ShipModule(128064334,64)},
            { "int_shieldcellbank_size8_class3", new ShipModule(128064335,160)},
            { "int_shieldcellbank_size8_class4", new ShipModule(128064336,256)},
            { "int_shieldcellbank_size8_class5", new ShipModule(128064337,160)},
            { "int_shieldgenerator_size1_class5", new ShipModule(128064262,1.3)},
            { "int_shieldgenerator_size2_class1", new ShipModule(128064263,2.5)},
            { "int_shieldgenerator_size2_class2", new ShipModule(128064264,1)},
            { "int_shieldgenerator_size2_class3", new ShipModule(128064265,2.5)},
            { "int_shieldgenerator_size2_class4", new ShipModule(128064266,4)},
            { "int_shieldgenerator_size2_class5", new ShipModule(128064267,2.5)},
            { "int_shieldgenerator_size3_class1", new ShipModule(128064268,5)},
            { "int_shieldgenerator_size3_class2", new ShipModule(128064269,2)},
            { "int_shieldgenerator_size3_class3", new ShipModule(128064270,5)},
            { "int_shieldgenerator_size3_class4", new ShipModule(128064271,8)},
            { "int_shieldgenerator_size3_class5", new ShipModule(128064272,5)},
            { "int_shieldgenerator_size4_class1", new ShipModule(128064273,10)},
            { "int_shieldgenerator_size4_class2", new ShipModule(128064274,4)},
            { "int_shieldgenerator_size4_class3", new ShipModule(128064275,10)},
            { "int_shieldgenerator_size4_class4", new ShipModule(128064276,16)},
            { "int_shieldgenerator_size4_class5", new ShipModule(128064277,10)},
            { "int_shieldgenerator_size5_class1", new ShipModule(128064278,20)},
            { "int_shieldgenerator_size5_class2", new ShipModule(128064279,8)},
            { "int_shieldgenerator_size5_class3", new ShipModule(128064280,20)},
            { "int_shieldgenerator_size5_class4", new ShipModule(128064281,32)},
            { "int_shieldgenerator_size5_class5", new ShipModule(128064282,20)},
            { "int_shieldgenerator_size6_class1", new ShipModule(128064283,40)},
            { "int_shieldgenerator_size6_class2", new ShipModule(128064284,16)},
            { "int_shieldgenerator_size6_class3", new ShipModule(128064285,40)},
            { "int_shieldgenerator_size6_class4", new ShipModule(128064286,64)},
            { "int_shieldgenerator_size6_class5", new ShipModule(128064287,40)},
            { "int_shieldgenerator_size7_class1", new ShipModule(128064288,80)},
            { "int_shieldgenerator_size7_class2", new ShipModule(128064289,32)},
            { "int_shieldgenerator_size7_class3", new ShipModule(128064290,80)},
            { "int_shieldgenerator_size7_class4", new ShipModule(128064291,128)},
            { "int_shieldgenerator_size7_class5", new ShipModule(128064292,80)},
            { "int_shieldgenerator_size8_class1", new ShipModule(128064293,160)},
            { "int_shieldgenerator_size8_class2", new ShipModule(128064294,64)},
            { "int_shieldgenerator_size8_class3", new ShipModule(128064295,160)},
            { "int_shieldgenerator_size8_class4", new ShipModule(128064296,256)},
            { "int_shieldgenerator_size8_class5", new ShipModule(128064297,160)},
            { "int_detailedsurfacescanner_tiny", new ShipModule(128666634,1.3)},
            { "int_hyperdrive_size8_class1", new ShipModule(128064133,160)},
            { "int_hyperdrive_size8_class2", new ShipModule(128064134,64)},
            { "int_hyperdrive_size8_class3", new ShipModule(128064135,160)},
            { "int_hyperdrive_size8_class4", new ShipModule(128064136,256)},
            { "int_hyperdrive_size8_class5", new ShipModule(128064137,160)},
            { "int_hyperdrive_size7_class1", new ShipModule(128064128,80)},
            { "int_hyperdrive_size7_class2", new ShipModule(128064129,32)},
            { "int_hyperdrive_size7_class3", new ShipModule(128064130,80)},
            { "int_hyperdrive_size7_class4", new ShipModule(128064131,128)},
            { "int_hyperdrive_size7_class5", new ShipModule(128064132,80)},
            { "int_hyperdrive_size6_class1", new ShipModule(128064123,40)},
            { "int_hyperdrive_size6_class2", new ShipModule(128064124,16)},
            { "int_hyperdrive_size6_class3", new ShipModule(128064125,40)},
            { "int_hyperdrive_size6_class4", new ShipModule(128064126,64)},
            { "int_hyperdrive_size6_class5", new ShipModule(128064127,40)},
            { "int_hyperdrive_size5_class1", new ShipModule(128064118,20)},
            { "int_hyperdrive_size5_class2", new ShipModule(128064119,8)},
            { "int_hyperdrive_size5_class3", new ShipModule(128064120,20)},
            { "int_hyperdrive_size5_class4", new ShipModule(128064121,32)},
            { "int_hyperdrive_size5_class5", new ShipModule(128064122,20)},
            { "int_hyperdrive_size4_class1", new ShipModule(128064113,10)},
            { "int_hyperdrive_size4_class2", new ShipModule(128064114,4)},
            { "int_hyperdrive_size4_class3", new ShipModule(128064115,10)},
            { "int_hyperdrive_size4_class4", new ShipModule(128064116,16)},
            { "int_hyperdrive_size4_class5", new ShipModule(128064117,10)},
            { "int_hyperdrive_size3_class1", new ShipModule(128064108,5)},
            { "int_hyperdrive_size3_class2", new ShipModule(128064109,2)},
            { "int_hyperdrive_size3_class3", new ShipModule(128064110,5)},
            { "int_hyperdrive_size3_class4", new ShipModule(128064111,8)},
            { "int_hyperdrive_size3_class5", new ShipModule(128064112,5)},
            { "int_hyperdrive_size2_class1", new ShipModule(128064103,2.5)},
            { "int_hyperdrive_size2_class2", new ShipModule(128064104,1)},
            { "int_hyperdrive_size2_class3", new ShipModule(128064105,2.5)},
            { "int_hyperdrive_size2_class4", new ShipModule(128064106,4)},
            { "int_hyperdrive_size2_class5", new ShipModule(128064107,2.5)},
            { "int_lifesupport_size8_class1", new ShipModule(128064173,160)},
            { "int_lifesupport_size8_class2", new ShipModule(128064174,64)},
            { "int_lifesupport_size8_class3", new ShipModule(128064175,160)},
            { "int_lifesupport_size8_class4", new ShipModule(128064176,256)},
            { "int_lifesupport_size8_class5", new ShipModule(128064177,160)},
            { "int_lifesupport_size7_class1", new ShipModule(128064168,80)},
            { "int_lifesupport_size7_class2", new ShipModule(128064169,32)},
            { "int_lifesupport_size7_class3", new ShipModule(128064170,80)},
            { "int_lifesupport_size7_class4", new ShipModule(128064171,128)},
            { "int_lifesupport_size7_class5", new ShipModule(128064172,80)},
            { "int_lifesupport_size6_class1", new ShipModule(128064163,40)},
            { "int_lifesupport_size6_class2", new ShipModule(128064164,16)},
            { "int_lifesupport_size6_class3", new ShipModule(128064165,40)},
            { "int_lifesupport_size6_class4", new ShipModule(128064166,64)},
            { "int_lifesupport_size6_class5", new ShipModule(128064167,40)},
            { "int_lifesupport_size5_class1", new ShipModule(128064158,20)},
            { "int_lifesupport_size5_class2", new ShipModule(128064159,8)},
            { "int_lifesupport_size5_class3", new ShipModule(128064160,20)},
            { "int_lifesupport_size5_class4", new ShipModule(128064161,32)},
            { "int_lifesupport_size5_class5", new ShipModule(128064162,20)},
            { "int_lifesupport_size4_class1", new ShipModule(128064153,10)},
            { "int_lifesupport_size4_class2", new ShipModule(128064154,4)},
            { "int_lifesupport_size4_class3", new ShipModule(128064155,10)},
            { "int_lifesupport_size4_class4", new ShipModule(128064156,16)},
            { "int_lifesupport_size4_class5", new ShipModule(128064157,10)},
            { "int_lifesupport_size3_class1", new ShipModule(128064148,5)},
            { "int_lifesupport_size3_class2", new ShipModule(128064149,2)},
            { "int_lifesupport_size3_class3", new ShipModule(128064150,5)},
            { "int_lifesupport_size3_class4", new ShipModule(128064151,8)},
            { "int_lifesupport_size3_class5", new ShipModule(128064152,5)},
            { "int_lifesupport_size2_class1", new ShipModule(128064143,2.5)},
            { "int_lifesupport_size2_class2", new ShipModule(128064144,1)},
            { "int_lifesupport_size2_class3", new ShipModule(128064145,2.5)},
            { "int_lifesupport_size2_class4", new ShipModule(128064146,4)},
            { "int_lifesupport_size2_class5", new ShipModule(128064147,2.5)},
            { "int_lifesupport_size1_class1", new ShipModule(128064138,1.3)},
            { "int_lifesupport_size1_class2", new ShipModule(128064139,0.5)},
            { "int_lifesupport_size1_class3", new ShipModule(128064140,1.3)},
            { "int_lifesupport_size1_class4", new ShipModule(128064141,2)},
            { "int_lifesupport_size1_class5", new ShipModule(128064142,1.3)},
            { "int_planetapproachsuite", new ShipModule(128672317,0)},
            { "int_powerdistributor_size8_class1", new ShipModule(128064213,160)},
            { "int_powerdistributor_size8_class2", new ShipModule(128064214,64)},
            { "int_powerdistributor_size8_class3", new ShipModule(128064215,160)},
            { "int_powerdistributor_size8_class4", new ShipModule(128064216,256)},
            { "int_powerdistributor_size8_class5", new ShipModule(128064217,160)},
            { "int_powerdistributor_size7_class1", new ShipModule(128064208,80)},
            { "int_powerdistributor_size7_class2", new ShipModule(128064209,32)},
            { "int_powerdistributor_size7_class3", new ShipModule(128064210,80)},
            { "int_powerdistributor_size7_class4", new ShipModule(128064211,128)},
            { "int_powerdistributor_size7_class5", new ShipModule(128064212,80)},
            { "int_powerdistributor_size6_class1", new ShipModule(128064203,40)},
            { "int_powerdistributor_size6_class2", new ShipModule(128064204,16)},
            { "int_powerdistributor_size6_class3", new ShipModule(128064205,40)},
            { "int_powerdistributor_size6_class4", new ShipModule(128064206,64)},
            { "int_powerdistributor_size6_class5", new ShipModule(128064207,40)},
            { "int_powerdistributor_size5_class1", new ShipModule(128064198,20)},
            { "int_powerdistributor_size5_class2", new ShipModule(128064199,8)},
            { "int_powerdistributor_size5_class3", new ShipModule(128064200,20)},
            { "int_powerdistributor_size5_class4", new ShipModule(128064201,32)},
            { "int_powerdistributor_size5_class5", new ShipModule(128064202,20)},
            { "int_powerdistributor_size4_class1", new ShipModule(128064193,10)},
            { "int_powerdistributor_size4_class2", new ShipModule(128064194,4)},
            { "int_powerdistributor_size4_class3", new ShipModule(128064195,10)},
            { "int_powerdistributor_size4_class4", new ShipModule(128064196,16)},
            { "int_powerdistributor_size4_class5", new ShipModule(128064197,10)},
            { "int_powerdistributor_size3_class1", new ShipModule(128064188,5)},
            { "int_powerdistributor_size3_class2", new ShipModule(128064189,2)},
            { "int_powerdistributor_size3_class3", new ShipModule(128064190,5)},
            { "int_powerdistributor_size3_class4", new ShipModule(128064191,8)},
            { "int_powerdistributor_size3_class5", new ShipModule(128064192,5)},
            { "int_powerdistributor_size2_class1", new ShipModule(128064183,2.5)},
            { "int_powerdistributor_size2_class2", new ShipModule(128064184,1)},
            { "int_powerdistributor_size2_class3", new ShipModule(128064185,2.5)},
            { "int_powerdistributor_size2_class4", new ShipModule(128064186,4)},
            { "int_powerdistributor_size2_class5", new ShipModule(128064187,2.5)},
            { "int_powerdistributor_size1_class1", new ShipModule(128064178,1.3)},
            { "int_powerdistributor_size1_class2", new ShipModule(128064179,0.5)},
            { "int_powerdistributor_size1_class3", new ShipModule(128064180,1.3)},
            { "int_powerdistributor_size1_class4", new ShipModule(128064181,2)},
            { "int_powerdistributor_size1_class5", new ShipModule(128064182,1.3)},
            { "int_powerplant_size8_class1", new ShipModule(128064063,160)},
            { "int_powerplant_size8_class2", new ShipModule(128064064,64)},
            { "int_powerplant_size8_class3", new ShipModule(128064065,80)},
            { "int_powerplant_size8_class4", new ShipModule(128064066,128)},
            { "int_powerplant_size8_class5", new ShipModule(128064067,80)},
            { "int_powerplant_size7_class1", new ShipModule(128064058,80)},
            { "int_powerplant_size7_class2", new ShipModule(128064059,32)},
            { "int_powerplant_size7_class3", new ShipModule(128064060,40)},
            { "int_powerplant_size7_class4", new ShipModule(128064061,64)},
            { "int_powerplant_size7_class5", new ShipModule(128064062,40)},
            { "int_powerplant_size6_class1", new ShipModule(128064053,40)},
            { "int_powerplant_size6_class2", new ShipModule(128064054,16)},
            { "int_powerplant_size6_class3", new ShipModule(128064055,20)},
            { "int_powerplant_size6_class4", new ShipModule(128064056,32)},
            { "int_powerplant_size6_class5", new ShipModule(128064057,20)},
            { "int_powerplant_size5_class1", new ShipModule(128064048,20)},
            { "int_powerplant_size5_class2", new ShipModule(128064049,8)},
            { "int_powerplant_size5_class3", new ShipModule(128064050,10)},
            { "int_powerplant_size5_class4", new ShipModule(128064051,16)},
            { "int_powerplant_size5_class5", new ShipModule(128064052,10)},
            { "int_powerplant_size4_class1", new ShipModule(128064043,10)},
            { "int_powerplant_size4_class2", new ShipModule(128064044,4)},
            { "int_powerplant_size4_class3", new ShipModule(128064045,5)},
            { "int_powerplant_size4_class4", new ShipModule(128064046,8)},
            { "int_powerplant_size4_class5", new ShipModule(128064047,5)},
            { "int_powerplant_size3_class1", new ShipModule(128064038,5)},
            { "int_powerplant_size3_class2", new ShipModule(128064039,2)},
            { "int_powerplant_size3_class3", new ShipModule(128064040,2.5)},
            { "int_powerplant_size3_class4", new ShipModule(128064041,4)},
            { "int_powerplant_size3_class5", new ShipModule(128064042,2.5)},
            { "int_powerplant_size2_class1", new ShipModule(128064033,2.5)},
            { "int_powerplant_size2_class2", new ShipModule(128064034,1)},
            { "int_powerplant_size2_class3", new ShipModule(128064035,1.3)},
            { "int_powerplant_size2_class4", new ShipModule(128064036,2)},
            { "int_powerplant_size2_class5", new ShipModule(128064037,1.3)},
            { "int_sensors_size8_class1", new ShipModule(128064253,160)},
            { "int_sensors_size8_class2", new ShipModule(128064254,64)},
            { "int_sensors_size8_class3", new ShipModule(128064255,160)},
            { "int_sensors_size8_class4", new ShipModule(128064256,256)},
            { "int_sensors_size8_class5", new ShipModule(128064257,160)},
            { "int_sensors_size7_class1", new ShipModule(128064248,80)},
            { "int_sensors_size7_class2", new ShipModule(128064249,32)},
            { "int_sensors_size7_class3", new ShipModule(128064250,80)},
            { "int_sensors_size7_class4", new ShipModule(128064251,128)},
            { "int_sensors_size7_class5", new ShipModule(128064252,80)},
            { "int_sensors_size6_class1", new ShipModule(128064243,40)},
            { "int_sensors_size6_class2", new ShipModule(128064244,16)},
            { "int_sensors_size6_class3", new ShipModule(128064245,40)},
            { "int_sensors_size6_class4", new ShipModule(128064246,64)},
            { "int_sensors_size6_class5", new ShipModule(128064247,40)},
            { "int_sensors_size5_class1", new ShipModule(128064238,20)},
            { "int_sensors_size5_class2", new ShipModule(128064239,8)},
            { "int_sensors_size5_class3", new ShipModule(128064240,20)},
            { "int_sensors_size5_class4", new ShipModule(128064241,32)},
            { "int_sensors_size5_class5", new ShipModule(128064242,20)},
            { "int_sensors_size4_class1", new ShipModule(128064233,10)},
            { "int_sensors_size4_class2", new ShipModule(128064234,4)},
            { "int_sensors_size4_class3", new ShipModule(128064235,10)},
            { "int_sensors_size4_class4", new ShipModule(128064236,16)},
            { "int_sensors_size4_class5", new ShipModule(128064237,10)},
            { "int_sensors_size3_class1", new ShipModule(128064228,5)},
            { "int_sensors_size3_class2", new ShipModule(128064229,2)},
            { "int_sensors_size3_class3", new ShipModule(128064230,5)},
            { "int_sensors_size3_class4", new ShipModule(128064231,8)},
            { "int_sensors_size3_class5", new ShipModule(128064232,5)},
            { "int_sensors_size2_class1", new ShipModule(128064223,2.5)},
            { "int_sensors_size2_class2", new ShipModule(128064224,1)},
            { "int_sensors_size2_class3", new ShipModule(128064225,2.5)},
            { "int_sensors_size2_class4", new ShipModule(128064226,4)},
            { "int_sensors_size2_class5", new ShipModule(128064227,2.5)},
            { "int_sensors_size1_class1", new ShipModule(128064218,1.3)},
            { "int_sensors_size1_class2", new ShipModule(128064219,0.5)},
            { "int_sensors_size1_class3", new ShipModule(128064220,1.3)},
            { "int_sensors_size1_class4", new ShipModule(128064221,2)},
            { "int_sensors_size1_class5", new ShipModule(128064222,1.3)},
            { "int_engine_size8_class1", new ShipModule(128064098,160)},
            { "int_engine_size8_class2", new ShipModule(128064099,64)},
            { "int_engine_size8_class3", new ShipModule(128064100,160)},
            { "int_engine_size8_class4", new ShipModule(128064101,256)},
            { "int_engine_size8_class5", new ShipModule(128064102,160)},
            { "int_engine_size7_class1", new ShipModule(128064093,80)},
            { "int_engine_size7_class2", new ShipModule(128064094,32)},
            { "int_engine_size7_class3", new ShipModule(128064095,80)},
            { "int_engine_size7_class4", new ShipModule(128064096,128)},
            { "int_engine_size7_class5", new ShipModule(128064097,80)},
            { "int_engine_size6_class1", new ShipModule(128064088,40)},
            { "int_engine_size6_class2", new ShipModule(128064089,16)},
            { "int_engine_size6_class3", new ShipModule(128064090,40)},
            { "int_engine_size6_class4", new ShipModule(128064091,64)},
            { "int_engine_size6_class5", new ShipModule(128064092,40)},
            { "int_engine_size5_class1", new ShipModule(128064083,20)},
            { "int_engine_size5_class2", new ShipModule(128064084,8)},
            { "int_engine_size5_class3", new ShipModule(128064085,20)},
            { "int_engine_size5_class4", new ShipModule(128064086,32)},
            { "int_engine_size5_class5", new ShipModule(128064087,20)},
            { "int_engine_size4_class1", new ShipModule(128064078,10)},
            { "int_engine_size4_class2", new ShipModule(128064079,4)},
            { "int_engine_size4_class3", new ShipModule(128064080,10)},
            { "int_engine_size4_class4", new ShipModule(128064081,16)},
            { "int_engine_size4_class5", new ShipModule(128064082,10)},
            { "int_engine_size3_class1", new ShipModule(128064073,5)},
            { "int_engine_size3_class2", new ShipModule(128064074,2)},
            { "int_engine_size3_class3", new ShipModule(128064075,5)},
            { "int_engine_size3_class4", new ShipModule(128064076,8)},
            { "int_engine_size3_class5", new ShipModule(128064077,5)},
            { "int_engine_size2_class1", new ShipModule(128064068,2.5)},
            { "int_engine_size2_class2", new ShipModule(128064069,1)},
            { "int_engine_size2_class3", new ShipModule(128064070,2.5)},
            { "int_engine_size2_class4", new ShipModule(128064071,4)},
            { "int_engine_size2_class5", new ShipModule(128064072,2.5)},
            { "int_engine_size3_class5_fast", new ShipModule(128682013,5)},
            { "int_engine_size2_class5_fast", new ShipModule(128682014,2.5)},
        };


        #endregion

        #region ship data

        static Dictionary<string, ShipInfo> adder = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128049268,0)},
            { "grade2", new ShipModule(128049269,3)},
            { "grade3", new ShipModule(128049270,5)},
            { "mirrored", new ShipModule(128049271,5)},
            { "reactive", new ShipModule(128049272,5)},
            { "HullMass", new ShipInfoInt(35)},
            { "Manu", new ShipInfoString("Zorgon Peterson")},
            { "Speed", new ShipInfoInt(220)},
            { "Boost", new ShipInfoInt(320)},
            { "HullCost", new ShipInfoInt(40000)},
            { "Class", new ShipInfoInt(1)},
        };
        static Dictionary<string, ShipInfo> anaconda = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128049364,0)},
            { "grade2", new ShipModule(128049365,30)},
            { "grade3", new ShipModule(128049366,60)},
            { "mirrored", new ShipModule(128049367,60)},
            { "reactive", new ShipModule(128049368,60)},
            { "HullMass", new ShipInfoInt(400)},
            { "Manu", new ShipInfoString("Faulcon DeLacy")},
            { "Speed", new ShipInfoInt(180)},
            { "Boost", new ShipInfoInt(240)},
            { "HullCost", new ShipInfoInt(141889930)},
            { "Class", new ShipInfoInt(3)},
        };
        static Dictionary<string, ShipInfo> asp = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128049304,0)},
            { "grade2", new ShipModule(128049305,21)},
            { "grade3", new ShipModule(128049306,42)},
            { "mirrored", new ShipModule(128049307,42)},
            { "reactive", new ShipModule(128049308,42)},
            { "HullMass", new ShipInfoInt(280)},
            { "Manu", new ShipInfoString("Lakon")},
            { "Speed", new ShipInfoInt(250)},
            { "Boost", new ShipInfoInt(340)},
            { "HullCost", new ShipInfoInt(6135660)},
            { "Class", new ShipInfoInt(2)},
        };
        static Dictionary<string, ShipInfo> asp_scout = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128672278,0)},
            { "grade2", new ShipModule(128672279,21)},
            { "grade3", new ShipModule(128672280,42)},
            { "mirrored", new ShipModule(128672281,42)},
            { "reactive", new ShipModule(128672282,42)},
            { "HullMass", new ShipInfoInt(150)},
            { "Manu", new ShipInfoString("Lakon")},
            { "Speed", new ShipInfoInt(220)},
            { "Boost", new ShipInfoInt(300)},
            { "HullCost", new ShipInfoInt(3818240)},
            { "Class", new ShipInfoInt(2)},
        };
        static Dictionary<string, ShipInfo> beluga = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128049346,0)},
            { "grade2", new ShipModule(128049347,83)},
            { "grade3", new ShipModule(128049348,165)},
            { "mirrored", new ShipModule(128049349,165)},
            { "reactive", new ShipModule(128049350,165)},
            { "HullMass", new ShipInfoInt(950)},
            { "Manu", new ShipInfoString("Saud Kruger")},
            { "Speed", new ShipInfoInt(200)},
            { "Boost", new ShipInfoInt(280)},
            { "HullCost", new ShipInfoInt(79654610)},
            { "Class", new ShipInfoInt(3)},
        };
        static Dictionary<string, ShipInfo> cobra_mk_iii = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128049280,0)},
            { "grade2", new ShipModule(128049281,14)},
            { "grade3", new ShipModule(128049282,27)},
            { "mirrored", new ShipModule(128049283,27)},
            { "reactive", new ShipModule(128049284,27)},
            { "HullMass", new ShipInfoInt(180)},
            { "Manu", new ShipInfoString("Faulcon DeLacy")},
            { "Speed", new ShipInfoInt(280)},
            { "Boost", new ShipInfoInt(400)},
            { "HullCost", new ShipInfoInt(205800)},
            { "Class", new ShipInfoInt(1)},
        };
        static Dictionary<string, ShipInfo> cobra_mk_iv = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128672264,0)},
            { "grade2", new ShipModule(128672265,14)},
            { "grade3", new ShipModule(128672266,27)},
            { "mirrored", new ShipModule(128672267,27)},
            { "reactive", new ShipModule(128672268,27)},
            { "HullMass", new ShipInfoInt(210)},
            { "Manu", new ShipInfoString("Faulcon DeLacy")},
            { "Speed", new ShipInfoInt(200)},
            { "Boost", new ShipInfoInt(300)},
            { "HullCost", new ShipInfoInt(603740)},
            { "Class", new ShipInfoInt(1)},
        };
        static Dictionary<string, ShipInfo> diamondback_explorer = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128671832,0)},
            { "grade2", new ShipModule(128671833,23)},
            { "grade3", new ShipModule(128671834,47)},
            { "mirrored", new ShipModule(128671835,26)},
            { "reactive", new ShipModule(128671836,47)},
            { "HullMass", new ShipInfoInt(260)},
            { "Manu", new ShipInfoString("Lakon")},
            { "Speed", new ShipInfoInt(260)},
            { "Boost", new ShipInfoInt(340)},
            { "HullCost", new ShipInfoInt(1635700)},
            { "Class", new ShipInfoInt(1)},
        };
        static Dictionary<string, ShipInfo> diamondback = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128671218,0)},
            { "grade2", new ShipModule(128671219,13)},
            { "grade3", new ShipModule(128671220,26)},
            { "mirrored", new ShipModule(128671221,26)},
            { "reactive", new ShipModule(128671222,26)},
            { "HullMass", new ShipInfoInt(170)},
            { "Manu", new ShipInfoString("Lakon")},
            { "Speed", new ShipInfoInt(280)},
            { "Boost", new ShipInfoInt(380)},
            { "HullCost", new ShipInfoInt(461340)},
            { "Class", new ShipInfoInt(1)},
        };
        static Dictionary<string, ShipInfo> dolphin = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128049292,0)},
            { "grade2", new ShipModule(128049293,32)},
            { "grade3", new ShipModule(128049294,63)},
            { "mirrored", new ShipModule(128049295,63)},
            { "reactive", new ShipModule(128049296,63)},
            { "HullMass", new ShipInfoInt(140)},
            { "Manu", new ShipInfoString("Saud Kruger")},
            { "Speed", new ShipInfoInt(250)},
            { "Boost", new ShipInfoInt(350)},
            { "HullCost", new ShipInfoInt(1115330)},
            { "Class", new ShipInfoInt(1)},
        };
        static Dictionary<string, ShipInfo> eagle = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128049256,0)},
            { "grade2", new ShipModule(128049257,4)},
            { "grade3", new ShipModule(128049258,8)},
            { "mirrored", new ShipModule(128049259,8)},
            { "reactive", new ShipModule(128049260,8)},
            { "HullMass", new ShipInfoInt(50)},
            { "Manu", new ShipInfoString("Core Dynamics")},
            { "Speed", new ShipInfoInt(240)},
            { "Boost", new ShipInfoInt(350)},
            { "HullCost", new ShipInfoInt(10440)},
            { "Class", new ShipInfoInt(1)},
        };
        static Dictionary<string, ShipInfo> federal_assault_ship = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128672147,0)},
            { "grade2", new ShipModule(128672148,44)},
            { "grade3", new ShipModule(128672149,87)},
            { "mirrored", new ShipModule(128672150,87)},
            { "reactive", new ShipModule(128672151,87)},
            { "HullMass", new ShipInfoInt(480)},
            { "Manu", new ShipInfoString("Core Dynamics")},
            { "Speed", new ShipInfoInt(210)},
            { "Boost", new ShipInfoInt(350)},
            { "HullCost", new ShipInfoInt(19072000)},
            { "Class", new ShipInfoInt(2)},
        };
        static Dictionary<string, ShipInfo> federal_corvette = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128049370,0)},
            { "grade2", new ShipModule(128049371,30)},
            { "grade3", new ShipModule(128049372,60)},
            { "mirrored", new ShipModule(128049373,60)},
            { "reactive", new ShipModule(128049374,60)},
            { "HullMass", new ShipInfoInt(900)},
            { "Manu", new ShipInfoString("Core Dynamics")},
            { "Speed", new ShipInfoInt(200)},
            { "Boost", new ShipInfoInt(260)},
            { "HullCost", new ShipInfoInt(182589570)},
            { "Class", new ShipInfoInt(3)},
        };
        static Dictionary<string, ShipInfo> federal_dropship = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128049322,0)},
            { "grade2", new ShipModule(128049323,44)},
            { "grade3", new ShipModule(128049324,87)},
            { "mirrored", new ShipModule(128049325,87)},
            { "reactive", new ShipModule(128049326,87)},
            { "HullMass", new ShipInfoInt(580)},
            { "Manu", new ShipInfoString("Core Dynamics")},
            { "Speed", new ShipInfoInt(180)},
            { "Boost", new ShipInfoInt(300)},
            { "HullCost", new ShipInfoInt(13469990)},
            { "Class", new ShipInfoInt(2)},
        };
        static Dictionary<string, ShipInfo> federal_gunship = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128672154,0)},
            { "grade2", new ShipModule(128672155,44)},
            { "grade3", new ShipModule(128672156,87)},
            { "mirrored", new ShipModule(128672157,87)},
            { "reactive", new ShipModule(128672158,87)},
            { "HullMass", new ShipInfoInt(580)},
            { "Manu", new ShipInfoString("Core Dynamics")},
            { "Speed", new ShipInfoInt(170)},
            { "Boost", new ShipInfoInt(280)},
            { "HullCost", new ShipInfoInt(34774790)},
            { "Class", new ShipInfoInt(2)},
        };
        static Dictionary<string, ShipInfo> fer_de_lance = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128049352,0)},
            { "grade2", new ShipModule(128049353,19)},
            { "grade3", new ShipModule(128049354,38)},
            { "mirrored", new ShipModule(128049355,38)},
            { "reactive", new ShipModule(128049356,38)},
            { "HullMass", new ShipInfoInt(250)},
            { "Manu", new ShipInfoString("Zorgon Peterson")},
            { "Speed", new ShipInfoInt(260)},
            { "Boost", new ShipInfoInt(350)},
            { "HullCost", new ShipInfoInt(51232230)},
            { "Class", new ShipInfoInt(2)},
        };
        static Dictionary<string, ShipInfo> hauler = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128049262,0)},
            { "grade2", new ShipModule(128049263,1)},
            { "grade3", new ShipModule(128049264,2)},
            { "mirrored", new ShipModule(128049265,2)},
            { "reactive", new ShipModule(128049266,2)},
            { "HullMass", new ShipInfoInt(14)},
            { "Manu", new ShipInfoString("Zorgon Peterson")},
            { "Speed", new ShipInfoInt(200)},
            { "Boost", new ShipInfoInt(300)},
            { "HullCost", new ShipInfoInt(29790)},
            { "Class", new ShipInfoInt(1)},
        };
        static Dictionary<string, ShipInfo> imperial_clipper = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128049316,0)},
            { "grade2", new ShipModule(128049317,30)},
            { "grade3", new ShipModule(128049318,60)},
            { "mirrored", new ShipModule(128049319,60)},
            { "reactive", new ShipModule(128049320,60)},
            { "HullMass", new ShipInfoInt(400)},
            { "Manu", new ShipInfoString("Gutamaya")},
            { "Speed", new ShipInfoInt(300)},
            { "Boost", new ShipInfoInt(380)},
            { "HullCost", new ShipInfoInt(21077780)},
            { "Class", new ShipInfoInt(3)},
        };
        static Dictionary<string, ShipInfo> imperial_courier = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128671224,0)},
            { "grade2", new ShipModule(128671225,4)},
            { "grade3", new ShipModule(128671226,8)},
            { "mirrored", new ShipModule(128671227,8)},
            { "reactive", new ShipModule(128671228,8)},
            { "HullMass", new ShipInfoInt(35)},
            { "Manu", new ShipInfoString("Gutamaya")},
            { "Speed", new ShipInfoInt(280)},
            { "Boost", new ShipInfoInt(380)},
            { "HullCost", new ShipInfoInt(2481550)},
            { "Class", new ShipInfoInt(1)},
        };
        static Dictionary<string, ShipInfo> imperial_cutter = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128049376,0)},
            { "grade2", new ShipModule(128049377,30)},
            { "grade3", new ShipModule(128049378,60)},
            { "mirrored", new ShipModule(128049379,60)},
            { "reactive", new ShipModule(128049380,60)},
            { "HullMass", new ShipInfoInt(1100)},
            { "Manu", new ShipInfoString("Gutamaya")},
            { "Speed", new ShipInfoInt(200)},
            { "Boost", new ShipInfoInt(320)},
            { "HullCost", new ShipInfoInt(199926890)},
            { "Class", new ShipInfoInt(3)},
        };
        static Dictionary<string, ShipInfo> imperial_eagle = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128672140,0)},
            { "grade2", new ShipModule(128672141,4)},
            { "grade3", new ShipModule(128672142,8)},
            { "mirrored", new ShipModule(128672143,8)},
            { "reactive", new ShipModule(128672144,8)},
            { "HullMass", new ShipInfoInt(50)},
            { "Manu", new ShipInfoString("Core Dynamics")},
            { "Speed", new ShipInfoInt(300)},
            { "Boost", new ShipInfoInt(400)},
            { "HullCost", new ShipInfoInt(72180)},
            { "Class", new ShipInfoInt(1)},
        };
        static Dictionary<string, ShipInfo> orca = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128049328,0)},
            { "grade2", new ShipModule(128049329,21)},
            { "grade3", new ShipModule(128049330,87)},
            { "mirrored", new ShipModule(128049331,87)},
            { "reactive", new ShipModule(128049332,87)},
            { "HullMass", new ShipInfoInt(290)},
            { "Manu", new ShipInfoString("Saud Kruger")},
            { "Speed", new ShipInfoInt(300)},
            { "Boost", new ShipInfoInt(380)},
            { "HullCost", new ShipInfoInt(47790590)},
            { "Class", new ShipInfoInt(3)},
        };
        static Dictionary<string, ShipInfo> python = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128049340,0)},
            { "grade2", new ShipModule(128049341,26)},
            { "grade3", new ShipModule(128049342,53)},
            { "mirrored", new ShipModule(128049343,53)},
            { "reactive", new ShipModule(128049344,53)},
            { "HullMass", new ShipInfoInt(350)},
            { "Manu", new ShipInfoString("Faulcon DeLacy")},
            { "Speed", new ShipInfoInt(230)},
            { "Boost", new ShipInfoInt(300)},
            { "HullCost", new ShipInfoInt(55171380)},
            { "Class", new ShipInfoInt(2)},
        };
        static Dictionary<string, ShipInfo> sidewinder = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128049250,0)},
            { "grade2", new ShipModule(128049251,2)},
            { "grade3", new ShipModule(128049252,4)},
            { "mirrored", new ShipModule(128049253,4)},
            { "reactive", new ShipModule(128049254,4)},
            { "HullMass", new ShipInfoInt(25)},
            { "Manu", new ShipInfoString("Faulcon DeLacy")},
            { "Speed", new ShipInfoInt(220)},
            { "Boost", new ShipInfoInt(320)},
            { "HullCost", new ShipInfoInt(4070)},
            { "Class", new ShipInfoInt(1)},
        };
        static Dictionary<string, ShipInfo> type_6_transporter = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128049286,0)},
            { "grade2", new ShipModule(128049287,12)},
            { "grade3", new ShipModule(128049288,23)},
            { "mirrored", new ShipModule(128049289,23)},
            { "reactive", new ShipModule(128049290,23)},
            { "HullMass", new ShipInfoInt(155)},
            { "Manu", new ShipInfoString("Lakon")},
            { "Speed", new ShipInfoInt(220)},
            { "Boost", new ShipInfoInt(350)},
            { "HullCost", new ShipInfoInt(865790)},
            { "Class", new ShipInfoInt(2)},
        };
        static Dictionary<string, ShipInfo> viper = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128049274,0)},
            { "grade2", new ShipModule(128049275,5)},
            { "grade3", new ShipModule(128049276,9)},
            { "mirrored", new ShipModule(128049277,9)},
            { "reactive", new ShipModule(128049278,9)},
            { "HullMass", new ShipInfoInt(50)},
            { "Manu", new ShipInfoString("Faulcon DeLacy")},
            { "Speed", new ShipInfoInt(320)},
            { "Boost", new ShipInfoInt(400)},
            { "HullCost", new ShipInfoInt(95900)},
            { "Class", new ShipInfoInt(1)},
        };
        static Dictionary<string, ShipInfo> viper_mk_iv = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128672257,0)},
            { "grade2", new ShipModule(128672258,5)},
            { "grade3", new ShipModule(128672259,9)},
            { "mirrored", new ShipModule(128672260,9)},
            { "reactive", new ShipModule(128672261,9)},
            { "HullMass", new ShipInfoInt(190)},
            { "Manu", new ShipInfoString("Faulcon DeLacy")},
            { "Speed", new ShipInfoInt(270)},
            { "Boost", new ShipInfoInt(340)},
            { "HullCost", new ShipInfoInt(310220)},
            { "Class", new ShipInfoInt(1)},
        };
        static Dictionary<string, ShipInfo> vulture = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128049310,0)},
            { "grade2", new ShipModule(128049311,17)},
            { "grade3", new ShipModule(128049312,35)},
            { "mirrored", new ShipModule(128049313,35)},
            { "reactive", new ShipModule(128049314,35)},
            { "HullMass", new ShipInfoInt(230)},
            { "Manu", new ShipInfoString("Core Dynamics")},
            { "Speed", new ShipInfoInt(210)},
            { "Boost", new ShipInfoInt(340)},
            { "HullCost", new ShipInfoInt(4689640)},
            { "Class", new ShipInfoInt(1)},
        };
        static Dictionary<string, ShipInfo> type_10_defender = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128049334,0)},
            { "grade2", new ShipModule(128049335,75)},
            { "grade3", new ShipModule(128049336,150)},
            { "mirrored", new ShipModule(128049337,150)},
            { "reactive", new ShipModule(128049338,150)},
            { "HullMass", new ShipInfoInt(1200)},
            { "Manu", new ShipInfoString("Lakon")},
            { "Speed", new ShipInfoInt(179)},
            { "Boost", new ShipInfoInt(219)},
            { "HullCost", new ShipInfoInt(121454173)},
            { "Class", new ShipInfoInt(3)},
        };
        static Dictionary<string, ShipInfo> type_9_heavy = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128049334,0)},
            { "grade2", new ShipModule(128049335,75)},
            { "grade3", new ShipModule(128049336,150)},
            { "mirrored", new ShipModule(128049337,150)},
            { "reactive", new ShipModule(128049338,150)},
            { "HullMass", new ShipInfoInt(850)},
            { "Manu", new ShipInfoString("Lakon")},
            { "Speed", new ShipInfoInt(130)},
            { "Boost", new ShipInfoInt(200)},
            { "HullCost", new ShipInfoInt(73255150)},
            { "Class", new ShipInfoInt(3)},
        };
        static Dictionary<string, ShipInfo> alliance_chieftain = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128816576,0)},
            { "grade2", new ShipModule(128816577,75)},
            { "grade3", new ShipModule(128816578,150)},
            { "mirrored", new ShipModule(128816579,150)},
            { "reactive", new ShipModule(128816580,150)},
            { "HullMass", new ShipInfoInt(400)},
            { "Manu", new ShipInfoString("Lakon")},
            { "Speed", new ShipInfoInt(230)},
            { "Boost", new ShipInfoInt(330)},
            { "HullCost", new ShipInfoInt(18182883)},
            { "Class", new ShipInfoInt(2)},
        };
        static Dictionary<string, ShipInfo> keelback = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128672271,0)},
            { "grade2", new ShipModule(128672272,12)},
            { "grade3", new ShipModule(128672273,23)},
            { "mirrored", new ShipModule(128672274,23)},
            { "reactive", new ShipModule(128672275,23)},
            { "HullMass", new ShipInfoInt(180)},
            { "Manu", new ShipInfoString("Lakon")},
            { "Speed", new ShipInfoInt(200)},
            { "Boost", new ShipInfoInt(300)},
            { "HullCost", new ShipInfoInt(2943870)},
            { "Class", new ShipInfoInt(2)},
        };
        static Dictionary<string, ShipInfo> type_7_transport = new Dictionary<string, ShipInfo>
        {
            { "grade1", new ShipModule(128049298,0)},
            { "grade2", new ShipModule(128049299,32)},
            { "grade3", new ShipModule(128049300,63)},
            { "mirrored", new ShipModule(128049301,63)},
            { "reactive", new ShipModule(128049302,63)},
            { "HullMass", new ShipInfoInt(350)},
            { "Manu", new ShipInfoString("Lakon")},
            { "Speed", new ShipInfoInt(180)},
            { "Boost", new ShipInfoInt(300)},
            { "HullCost", new ShipInfoInt(16780510)},
            { "Class", new ShipInfoInt(3)},
        };
        static Dictionary<string, Dictionary<string, ShipInfo>> ships = new Dictionary<string, Dictionary<string, ShipInfo>>
        {
            { "Adder",adder},
            { "Anaconda",anaconda},
            { "Asp",asp},
            { "Asp_Scout",asp_scout},
            { "BelugaLiner",beluga},
            { "CobraMkIII",cobra_mk_iii},
            { "CobraMkIV",cobra_mk_iv},
            { "DiamondBackXL",diamondback_explorer},
            { "DiamondBack",diamondback},
            { "Dolphin",dolphin},
            { "Eagle",eagle},
            { "Federation_Dropship_MkII",federal_assault_ship},
            { "Federation_Corvette",federal_corvette},
            { "Federation_Dropship",federal_dropship},
            { "Federation_Gunship",federal_gunship},
            { "FerDeLance",fer_de_lance},
            { "Hauler",hauler},
            { "Empire_Trader",imperial_clipper},
            { "Empire_Courier",imperial_courier},
            { "Cutter",imperial_cutter},
            { "Empire_Eagle",imperial_eagle},
            { "Orca",orca},
            { "Python",python},
            { "SideWinder",sidewinder},
            { "Type6",type_6_transporter},
            { "Viper",viper},
            { "Viper_MkIV",viper_mk_iv},
            { "Vulture",vulture},
            { "Type_9_military",type_10_defender},
            { "Type9",type_9_heavy},
            { "TypeX",alliance_chieftain},
            { "Independant_Trader",keelback},
            { "Type7",type_7_transport},
        };




        #endregion

    }
}
