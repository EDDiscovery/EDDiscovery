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
    class ModuleEDID
    {
        static ModuleEDID instance = null;

        private ModuleEDID()
        {
        }

        public static ModuleEDID Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ModuleEDID();
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

        // fdname is the name given in the journals, which match the ones in the folder elite.../win64/items/shipmodule
        // in normal form of prefix_itemname_extensions
        // As of 31/3/2017, Robby has gone thru as many modules as he can ;-)
        // Powerplay modules are hard.. don't have any, so guessed from that folder naming scheme which matches the events normally.
        // shipname is in frontier speak.

        public int CalcID(string ifd , string shipname )            // -1 dont have one, 0 unknown, else code
        {
            //System.Diagnostics.Debug.WriteLine("Lookup " + inorm + ":" + ifd);

            ifd = ifd.ToLower();

            if ( ifd.Contains("armour_"))
            {
                if (ships.ContainsKey(shipname))
                {
                    Dictionary<string, int> ship = ships[shipname];
                    int index = ifd.IndexOf("armour_");
                    string type = ifd.Substring(index + 7);

                    if (ship.ContainsKey(type))
                        return ship[type];
                }

                return 0;
            }

            foreach (var s in new[] { "bobble", "cargobaydoor", "cockpit", "decal", "enginecustomisation", "nameplate", "paintjob",
                                    "shipkit", "weaponcustomisation", "voicepack" , "string_lights_coloured" })
            {
                if (ifd.Contains(s))
                    return -1;       // no IDs
            }

            int firstunderscore = ifd.IndexOf('_');

            if (firstunderscore >= 0 )      // isolate the hpt_<itemfdname>_<extension> parts.
            {
                int nextunderscore = ifd.IndexOf('_',firstunderscore+1);
                string itemfdname, extension="";
                if (nextunderscore >= 0)
                {
                    itemfdname = ifd.Substring(firstunderscore + 1, nextunderscore - firstunderscore - 1);

                    if (itemfdname.Equals("dronecontrol"))      // this comes in two parts, so needs two underscores
                    {
                        int tryNext = ifd.IndexOf('_', nextunderscore + 1);
                        if (tryNext > 0)
                        {
                            nextunderscore = tryNext;
                            itemfdname = ifd.Substring(firstunderscore + 1, nextunderscore - firstunderscore - 1);
                        }
                        else  // research limpet controllers only come in one size so that's it.  Not in Coriolis as I write this so no EDID anyway but prevent errors while we wait for one
                        {
                            itemfdname = ifd.Substring(firstunderscore + 1);
                            nextunderscore = ifd.Length;
                        }
                    }

                    extension = ifd.Substring(nextunderscore);      // include the underscore so we have a name delimiter
                }
                else
                    itemfdname = ifd.Substring(firstunderscore + 1);    // whole string

                //System.Diagnostics.Debug.WriteLine("item " + itemfdname + " ext " + extension);

                if (modules.ContainsKey(itemfdname))
                {
                    int classvalue = -1;
                    if (extension.Contains("_tiny"))                // these are Hpt_ extensions to indicate size
                        classvalue = 0;
                    else if (extension.Contains("_small"))
                        classvalue = 1;
                    else if (extension.Contains("_medium"))
                        classvalue = 2;
                    else if (extension.Contains("_large"))
                        classvalue = 3;
                    else if (extension.Contains("_huge"))
                        classvalue = 4;

                    string matchtype = "";

                    if (classvalue != -1)      // weapon
                    {
                        string postfix = "";

                        if (extension.Contains("_turret"))          // basic type
                            postfix = "-T";
                        else if (extension.Contains("_gimbal"))
                            postfix = "-G";
                        else if (extension.Contains("_fixed"))
                            postfix = "-F";

                        if (extension.Contains("_impulse"))        // shock mine launcher, tested
                            postfix += "-I";
                        else if (extension.Contains("_advanced"))      // mining laster, FD folders.  Plasma Accelerator, fixed large advanced, FD folders
                            postfix += "-A";
                        else if (extension.Contains("_heat"))        // 1-F-Retributor beam laser, FD folders
                            postfix += "-H";
                        else if (extension.Contains("_scatter"))        // 1-F-Cytoscrambler burst laser, FD folders
                            postfix += "-S";
                        else if (extension.Contains("_strong"))        // Multicannon fixed small strong, FD folders
                            postfix += "-S";
                        else if (extension.Contains("_disruptor"))        // Pulselaser, fixed, medium, distruptor, FD folders
                            postfix += "-D";
                        else if (extension.Contains("_burst"))        // railgun, fixed, medium, burst.  FD folders.  _burst should not interfer with PulseLaserBurst
                            postfix += "-B";

                        matchtype = classvalue.ToString() + postfix;

                        Dictionary<string, int> m = modules[itemfdname];

                        if (modules[itemfdname].ContainsKey(matchtype))     // primary match
                            return modules[itemfdname][matchtype];
                        else if (modules[itemfdname].ContainsKey(ifd))      // worse case, try the whole id
                            return modules[itemfdname][ifd];
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("KEY NOT FOUND  " + itemfdname + " " + ifd + " = " + matchtype);
                        }
                    }
                    else
                    {                                                   // normal module naming, itemname_size4_class2 (size is class, class is really rating)
                        int si = extension.IndexOf("size");
                        int ci = extension.IndexOf("class");
                        if (si != -1 && ci != -1)
                        {
                            if (extension.Contains("_fast"))            // _fast at end means its a modification to the standard, move to other tables if applicable
                            {
                                if (itemfdname.Equals("shieldgenerator"))      // log and ED folder evidence
                                    itemfdname = "biweaveshieldgenerator";
                                else if (itemfdname.Equals("engine"))           // ED Folder evidence
                                    itemfdname = "enginefast";
                            }
                            else if (extension.Contains("_strong") && itemfdname.Equals("shieldgenerator"))     // ED folder evidence, _strong is prismatic
                                itemfdname = "pristmaticshieldgenerator";

                            classvalue = extension[si + 4] - '0';

                            if ( itemfdname == "buggybay")          //GH types..
                                matchtype = classvalue.ToString() + Convert.ToChar('H' - (extension[ci + 5] - '1'));          //GH comes out as class 1+2, used on some types.
                            else
                                matchtype = classvalue.ToString() + Convert.ToChar('E' - (extension[ci + 5] - '1'));

                            if (modules[itemfdname].ContainsKey(matchtype))
                                return modules[itemfdname][matchtype];
                            else if (modules[itemfdname].ContainsKey(ifd))      // worse case, use the whole string
                                return modules[itemfdname][ifd];
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("KEY NOT FOUND ST " + itemfdname + " " + ifd + " = " + matchtype);
                            }
                        }
                        else
                        {
                            if (modules[itemfdname].ContainsKey(ifd))       // no class, no size, see if whole matches.. some modules do this
                                return modules[itemfdname][ifd];
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("UNRECON " + itemfdname + " " + ifd);
                            }
                        }
                    }
                }
                else
                    System.Diagnostics.Debug.WriteLine("NOT FOUND  " + itemfdname + ":" + ifd);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Not _X" + ifd);
            }

            return 0;
        }

        #region Modules types to IDs

        static Dictionary<string, int> discoveryscanner = new Dictionary<string, int>
        {
            { "int_stellarbodydiscoveryscanner_advanced",128663561},
            { "int_stellarbodydiscoveryscanner_intermediate",128663560},
            { "int_stellarbodydiscoveryscanner_standard",128662535},
        };

        static Dictionary<string, int> detailedscanner = new Dictionary<string, int>
        {
            { "int_detailedsurfacescanner_tiny",128666634},
        };

        static Dictionary<string, int> ax_missile_rack = new Dictionary<string, int>
        {
            { "2-F>D",128788699},
            { "2-T>D",128788704},
            { "3-F>D",128788700},
            { "3-T>D",128788705},
        };
        static Dictionary<string, int> ax_multi_cannon = new Dictionary<string, int>
        {
            { "2-F",128788701},
            { "2-T",128793059},
            { "3-F",128788702},
            { "3-T",128793060},
        };

        static Dictionary<string, int> beam_laser = new Dictionary<string, int>
        {
            { "1-F",128049428},
            { "1-G",128049432},
            { "1-T",128049435},
            { "1-F-H",128671346}, // evidence, ED ShipModule folder.
            { "2-F",128049429},
            { "2-G",128049433},
            { "2-T",128049436},
            { "3-F",128049430},
            { "3-G",128049434},
            { "3-T",128049437},
            { "4-F",128049431},
            { "4-G",128681994},
        };
        static Dictionary<string, int> burst_laser = new Dictionary<string, int>
        {
            { "1-F",128049400},
            { "1-G",128049404},
            { "1-T",128049407},
            { "1-F-S",128671449}, //Evidence, FD folders
            { "2-F",128049401},
            { "2-G",128049405},
            { "2-T",128049408},
            { "3-F",128049402},
            { "3-G",128049406},
            { "3-T",128049409},
            { "4-F",128049403},
            { "4-G",128727920},
        };
        static Dictionary<string, int> cannon = new Dictionary<string, int>
        {
            { "1-F",128049438},
            { "1-G",128049442},
            { "1-T",128049445},
            { "2-F",128049439},
            { "2-G",128049443},
            { "2-T",128049446},
            { "3-F",128049440},
            { "3-G",128671120},
            { "3-T",128049447},
            { "4-F",128049441},
            { "4-G",128049444},
        };
        static Dictionary<string, int> cargo_scanner = new Dictionary<string, int>
        {
            { "0E",128662520},
            { "0D",128662521},
            { "0C",128662522},
            { "0B",128662523},
            { "0A",128662524},
        };

        static Dictionary<string, int> kill_warrant_scanner = new Dictionary<string, int>
        {
            { "0E",128662530},
            { "0D",128662531},
            { "0C",128662532},
            { "0B",128662533},
            { "0A",128662534},
        };

        static Dictionary<string, int> chaff_launcher = new Dictionary<string, int>
        {
            { "0",128049513},
        };
        static Dictionary<string, int> electronic_countermeasure = new Dictionary<string, int>
        {
            { "0",128049516},
        };
        static Dictionary<string, int> fragment_cannon = new Dictionary<string, int>
        {
            { "1-F",128049448},
            { "1-G",128049451},
            { "1-T",128049453},
            { "2-F",128049449},
            { "2-G",128049452},
            { "2-T",128049454},
            { "3-F",128049450},
            { "3-G",128671321},
            { "3-T",128671322},
            { "3-F+Pacifier",128671343}, //TBD
        };

        static Dictionary<string, int> frame_shift_wake_scanner = new Dictionary<string, int>
        {
            { "0E",128662525},
            { "0D",128662526},
            { "0C",128662527},
            { "0B",128662528},
            { "0A",128662529},
        };

        static Dictionary<string, int> heat_sink_launcher = new Dictionary<string, int>
        {
            { "0-T",128049519}, //ok
        };
        static Dictionary<string, int> mine_launcher = new Dictionary<string, int>
        {
            { "1-F",128049500},
            { "1-F-I",128671448},       // shock, called minelauncher_fixed_small_impulse
            { "2-F",128049501},
        };
        static Dictionary<string, int> mining_laser = new Dictionary<string, int>
        {
            { "1-F",128049525},
            { "1-F-A",128671347},   // Mining lance, called Mininglaser_fixed_small_advanced in fd folders
            { "2-F",128049526},
            { "2-T", 128740820 },
        };
        static Dictionary<string, int> dumbfiremissile_rack = new Dictionary<string, int>
        {
            { "1-F",128666724},
            { "2-F",128666725},
        };
        static Dictionary<string, int> seekermissile_rack = new Dictionary<string, int>
        {
            { "1-F",128049492},
            { "2-F",128049493},
        };
        static Dictionary<string, int> drunkmissile_rack = new Dictionary<string, int>
        {
            { "2-F",128671344},    // TBD, Guess, from FD folders that this code is a drunk missile rack.  
            // FSD disruptor does not have an ID code in coriolis
        };
        static Dictionary<string, int> multi_cannon = new Dictionary<string, int>
        {
            { "1-F",128049455},
            { "1-G",128049459},
            { "1-T",128049462},
            { "1-F-S",128671345},    //Evidence FD folders, called _strong
            { "2-F",128049456},
            { "2-G",128049460},
            { "2-T",128049463},
            { "3-F",128049457},
            { "3-G",128049461},
            { "4-F",128049458},
            { "4-G",128681996},
        };
        static Dictionary<string, int> plasma_accelerator = new Dictionary<string, int>
        {
            { "2-F",128049465},
            { "3-F",128049466},
            { "3-F-A",128671339}, // Evidence, FD folders, called _advanced
            { "4-F",128049467},
        };
        static Dictionary<string, int> point_defence = new Dictionary<string, int>
        {
            { "0-T",128049522},
        };
        static Dictionary<string, int> pulse_laser = new Dictionary<string, int>
        {
            { "1-F",128049381},
            { "1-G",128049385},
            { "1-T",128049388},
            { "2-F",128049382},
            { "2-G",128049386},
            { "2-T",128049389},
            { "2-F-D",128671342},  // disruptor, FD folders
            { "3-F",128049383},
            { "3-G",128049387},
            { "3-T",128049390},
            { "4-F",128049384},
            { "4-G",128681995},
        };
        static Dictionary<string, int> rail_gun = new Dictionary<string, int>
        {
            { "1-F",128049488},
            { "2-F",128049489},
            { "2-F-B",128671341},     // fixed medium burst in FD folders
        };

        static Dictionary<string, int> remote_release_flak_launcher = new Dictionary<string, int>
        {
            { "2-F",128785626},
            { "2-T",128793058},
        };

        static Dictionary<string, int> shield_booster = new Dictionary<string, int>
        {
            { "0E",128668532},
            { "0D",128668533},
            { "0C",128668534},
            { "0B",128668535},
            { "0A",128668536},
        };
        static Dictionary<string, int> torpedo_pylon = new Dictionary<string, int>
        {
            { "1-F",128049509},           //Verified
            { "2-F",128049510},
        };


        static Dictionary<string, int> shutdown_field_neutraliser = new Dictionary<string, int>
        {
            { "0F+Shutdown Field Neutraliser",128771884},
        };

        static Dictionary<string, int> xeno_scanner = new Dictionary<string, int>
        {
            { "0E+Xeno Scanner",128793115},
        };

        static Dictionary<string, int> auto_field_maintenance_unit = new Dictionary<string, int>
        {
            { "1E",128667598},
            { "1D",128667606},
            { "1C",128667614},
            { "1B",128667622},
            { "1A",128667630},
            { "2E",128667599},
            { "2D",128667607},
            { "2C",128667615},
            { "2B",128667623},
            { "2A",128667631},
            { "3E",128667600},
            { "3D",128667608},
            { "3C",128667616},
            { "3B",128667624},
            { "3A",128667632},
            { "4E",128667601},
            { "4D",128667609},
            { "4C",128667617},
            { "4B",128667625},
            { "4A",128667633},
            { "5E",128667602},
            { "5D",128667610},
            { "5C",128667618},
            { "5B",128667626},
            { "5A",128667634},
            { "6E",128667603},
            { "6D",128667611},
            { "6C",128667619},
            { "6B",128667627},
            { "6A",128667635},
            { "7E",128667604},
            { "7D",128667612},
            { "7C",128667620},
            { "7B",128667628},
            { "7A",128667636},
            { "8E",128667605},
            { "8D",128667613},
            { "8C",128667621},
            { "8B",128667629},
            { "8A",128667637},
        };
        static Dictionary<string, int> bi_weave_shield_generator = new Dictionary<string, int>
        {
            { "1C",128671331},
            { "2C",128671332},
            { "3C",128671333},
            { "4C",128671334},
            { "5C",128671335},
            { "6C",128671336},
            { "7C",128671337},
            { "8C",128671338},
        };
        static Dictionary<string, int> cargo_rack = new Dictionary<string, int>
        {
            { "1E",128064338},
            { "2E",128064339},
            { "3E",128064340},
            { "4E",128064341},
            { "5E",128064342},
            { "6E",128064343},
            { "7E",128064344},
            { "8E",128064345},
            { "1E+Corrosion Resistant",128681641},
            { "1F+Corrosion Resistant",128681992},
        };

        static Dictionary<string, int> corrosion_cargo_rack = new Dictionary<string, int>   
        {
            { "1E",128681641},          // TBD called E (small) F (larger) in coriolis.. surely wrong.  cargo size is 1 for E, 2 for F, which does not match.
            { "1D",128681992},
        };

        static Dictionary<string, int> collector_limpet_controllers = new Dictionary<string, int>
        {
            { "1E",128671229},
            { "1D",128671230},
            { "1C",128671231},
            { "1B",128671232},
            { "1A",128671233},
            { "3E",128671234},
            { "3D",128671235},
            { "3C",128671236},
            { "3B",128671237},
            { "3A",128671238},
            { "5E",128671239},
            { "5D",128671240},
            { "5C",128671241},
            { "5B",128671242},
            { "5A",128671243},
            { "7E",128671244},
            { "7D",128671245},
            { "7C",128671246},
            { "7B",128671247},
            { "7A",128671248},
        };
        static Dictionary<string, int> docking_computer = new Dictionary<string, int>
        {
            { "int_dockingcomputer_standard",128049549},
        };
        static Dictionary<string, int> passenger_cabin = new Dictionary<string, int>
        {
            { "2E",128734690},      //TBD
            { "3E",128734691},
            { "4E",128727922},
            { "5E",128734693},
            { "6E",128727926},

            { "4C",128727924},
            { "5C",128734695},
            { "6C",128727928},

            { "3D",128734692},
            { "4D",128727923},
            { "5D",128734694},
            { "6D",128727927},

            { "5B",128727925},
            { "6B",128727929},
        };

        static Dictionary<string, int> fighter_hangar = new Dictionary<string, int>
        {
            { "int_fighterbay_size5_class1",128727930},
            { "int_fighterbay_size6_class1",128727931},
            { "int_fighterbay_size7_class1",128727932},
        };
        static Dictionary<string, int> frame_shift_drive_interdictor = new Dictionary<string, int>
        {
            { "1E",128666704},
            { "1D",128666708},
            { "1C",128666712},
            { "1B",128666716},
            { "1A",128666720},
            { "2E",128666705},
            { "2D",128666709},
            { "2C",128666713},
            { "2B",128666717},
            { "2A",128666721},
            { "3E",128666706},
            { "3D",128666710},
            { "3C",128666714},
            { "3B",128666718},
            { "3A",128666722},
            { "4E",128666707},
            { "4D",128666711},
            { "4C",128666715},
            { "4B",128666719},
            { "4A",128666723},
        };
        static Dictionary<string, int> fuel_scoop = new Dictionary<string, int>
        {
            { "1E",128666644},
            { "1D",128666652},
            { "1C",128666660},
            { "1B",128666668},
            { "1A",128666676},
            { "2E",128666645},
            { "2D",128666653},
            { "2C",128666661},
            { "2B",128666669},
            { "2A",128666677},
            { "3E",128666646},
            { "3D",128666654},
            { "3C",128666662},
            { "3B",128666670},
            { "3A",128666678},
            { "4E",128666647},
            { "4D",128666655},
            { "4C",128666663},
            { "4B",128666671},
            { "4A",128666679},
            { "5E",128666648},
            { "5D",128666656},
            { "5C",128666664},
            { "5B",128666672},
            { "5A",128666680},
            { "6E",128666649},
            { "6D",128666657},
            { "6C",128666665},
            { "6B",128666673},
            { "6A",128666681},
            { "7E",128666650},
            { "7D",128666658},
            { "7C",128666666},
            { "7B",128666674},
            { "7A",128666682},
            { "8E",128666651},
            { "8D",128666659},
            { "8C",128666667},
            { "8B",128666675},
            { "8A",128666683},
        };
        static Dictionary<string, int> fuel_transfer_limpet_controllers = new Dictionary<string, int>
        {
            { "1E",128671249},
            { "1D",128671250},
            { "1C",128671251},
            { "1B",128671252},
            { "1A",128671253},
            { "3E",128671254},
            { "3D",128671255},
            { "3C",128671256},
            { "3B",128671257},
            { "3A",128671258},
            { "5E",128671259},
            { "5D",128671260},
            { "5C",128671261},
            { "5B",128671262},
            { "5A",128671263},
            { "7E",128671264},
            { "7D",128671265},
            { "7C",128671266},
            { "7B",128671267},
            { "7A",128671268},
        };
        static Dictionary<string, int> hatch_breaker_limpet_controller = new Dictionary<string, int>
        {
            { "1E",128066532},
            { "1D",128066533},
            { "1C",128066534},
            { "1B",128066535},
            { "1A",128066536},
            { "3E",128066537},
            { "3D",128066538},
            { "3C",128066539},
            { "3B",128066540},
            { "3A",128066541},
            { "5E",128066542},
            { "5D",128066543},
            { "5C",128066544},
            { "5B",128066545},
            { "5A",128066546},
            { "7E",128066547},
            { "7D",128066548},
            { "7C",128066549},
            { "7B",128066550},
            { "7A",128066551},
        };
        static Dictionary<string, int> hull_reinforcement_package = new Dictionary<string, int>
        {
            { "1E",128668537},
            { "1D",128668538},
            { "2E",128668539},
            { "2D",128668540},
            { "3E",128668541},
            { "3D",128668542},
            { "4E",128668543},
            { "4D",128668544},
            { "5E",128668545},
            { "5D",128668546},
        };
        static Dictionary<string, int> module_reinforcement_package = new Dictionary<string, int>
        {
            { "1E",128737270},
            { "1D",128737271},
            { "2E",128737272},
            { "2D",128737273},
            { "3E",128737274},
            { "3D",128737275},
            { "4E",128737276},
            { "4D",128737277},
            { "5E",128737278},
            { "5D",128737279},
        };
        static Dictionary<string, int> planetary_vehicle_hanger = new Dictionary<string, int>
        {
            { "2H",128672288},
            { "2G",128672289},
            { "4H",128672290},
            { "4G",128672291},
            { "6H",128672292},
            { "6G",128672293},
        };
        static Dictionary<string, int> pristmatic_shield_generator = new Dictionary<string, int>
        {
            { "1A",128671323},
            { "2A",128671324},
            { "3A",128671325},
            { "4A",128671326},
            { "5A",128671327},
            { "6A",128671328},
            { "7A",128671329},
            { "8A",128671330},
        };
        static Dictionary<string, int> prospector_limpet_controllers = new Dictionary<string, int>
        {
            { "1E",128671269},
            { "1D",128671270},
            { "1C",128671271},
            { "1B",128671272},
            { "1A",128671273},
            { "3E",128671274},
            { "3D",128671275},
            { "3C",128671276},
            { "3B",128671277},
            { "3A",128671278},
            { "5E",128671279},
            { "5D",128671280},
            { "5C",128671281},
            { "5B",128671282},
            { "5A",128671283},
            { "7E",128671284},
            { "7D",128671285},
            { "7C",128671286},
            { "7B",128671287},
            { "7A",128671288},
        };
        static Dictionary<string, int> refinery = new Dictionary<string, int>
        {
            { "1E",128666684},
            { "1D",128666688},
            { "1C",128666692},
            { "1B",128666696},
            { "1A",128666700},
            { "2E",128666685},
            { "2D",128666689},
            { "2C",128666693},
            { "2B",128666697},
            { "2A",128666701},
            { "3E",128666686},
            { "3D",128666690},
            { "3C",128666694},
            { "3B",128666698},
            { "3A",128666702},
            { "4E",128666687},
            { "4D",128666691},
            { "4C",128666695},
            { "4B",128666699},
            { "4A",128666703},
        };
        static Dictionary<string, int> shield_cell_bank = new Dictionary<string, int>
        {
            { "1E",128064298},
            { "1D",128064299},
            { "1C",128064300},
            { "1B",128064301},
            { "1A",128064302},
            { "2E",128064303},
            { "2D",128064304},
            { "2C",128064305},
            { "2B",128064306},
            { "2A",128064307},
            { "3E",128064308},
            { "3D",128064309},
            { "3C",128064310},
            { "3B",128064311},
            { "3A",128064312},
            { "4E",128064313},
            { "4D",128064314},
            { "4C",128064315},
            { "4B",128064316},
            { "4A",128064317},
            { "5E",128064318},
            { "5D",128064319},
            { "5C",128064320},
            { "5B",128064321},
            { "5A",128064322},
            { "6E",128064323},
            { "6D",128064324},
            { "6C",128064325},
            { "6B",128064326},
            { "6A",128064327},
            { "7E",128064328},
            { "7D",128064329},
            { "7C",128064330},
            { "7B",128064331},
            { "7A",128064332},
            { "8E",128064333},
            { "8D",128064334},
            { "8C",128064335},
            { "8B",128064336},
            { "8A",128064337},
        };
        static Dictionary<string, int> shield_generator = new Dictionary<string, int>
        {
            { "1A",128064262},
            { "2E",128064263},
            { "2D",128064264},
            { "2C",128064265},
            { "2B",128064266},
            { "2A",128064267},
            { "3E",128064268},
            { "3D",128064269},
            { "3C",128064270},
            { "3B",128064271},
            { "3A",128064272},
            { "4E",128064273},
            { "4D",128064274},
            { "4C",128064275},
            { "4B",128064276},
            { "4A",128064277},
            { "5E",128064278},
            { "5D",128064279},
            { "5C",128064280},
            { "5B",128064281},
            { "5A",128064282},
            { "6E",128064283},
            { "6D",128064284},
            { "6C",128064285},
            { "6B",128064286},
            { "6A",128064287},
            { "7E",128064288},
            { "7D",128064289},
            { "7C",128064290},
            { "7B",128064291},
            { "7A",128064292},
            { "8E",128064293},
            { "8D",128064294},
            { "8C",128064295},
            { "8B",128064296},
            { "8A",128064297},
        };
        static Dictionary<string, int> frame_shift_drive = new Dictionary<string, int>
        {
            { "8E",128064133},
            { "8D",128064134},
            { "8C",128064135},
            { "8B",128064136},
            { "8A",128064137},
            { "7E",128064128},
            { "7D",128064129},
            { "7C",128064130},
            { "7B",128064131},
            { "7A",128064132},
            { "6E",128064123},
            { "6D",128064124},
            { "6C",128064125},
            { "6B",128064126},
            { "6A",128064127},
            { "5E",128064118},
            { "5D",128064119},
            { "5C",128064120},
            { "5B",128064121},
            { "5A",128064122},
            { "4E",128064113},
            { "4D",128064114},
            { "4C",128064115},
            { "4B",128064116},
            { "4A",128064117},
            { "3E",128064108},
            { "3D",128064109},
            { "3C",128064110},
            { "3B",128064111},
            { "3A",128064112},
            { "2E",128064103},
            { "2D",128064104},
            { "2C",128064105},
            { "2B",128064106},
            { "2A",128064107},
        };
        static Dictionary<string, int> fuel_tank = new Dictionary<string, int>
        {
            { "1C",128064346},
            { "2C",128064347},
            { "3C",128064348},
            { "4C",128064349},
            { "5C",128064350},
            { "6C",128064351},
            { "7C",128064352},
            { "8C",128064353},
        };
        static Dictionary<string, int> life_support = new Dictionary<string, int>
        {
            { "8E",128064173},
            { "8D",128064174},
            { "8C",128064175},
            { "8B",128064176},
            { "8A",128064177},
            { "7E",128064168},
            { "7D",128064169},
            { "7C",128064170},
            { "7B",128064171},
            { "7A",128064172},
            { "6E",128064163},
            { "6D",128064164},
            { "6C",128064165},
            { "6B",128064166},
            { "6A",128064167},
            { "5E",128064158},
            { "5D",128064159},
            { "5C",128064160},
            { "5B",128064161},
            { "5A",128064162},
            { "4E",128064153},
            { "4D",128064154},
            { "4C",128064155},
            { "4B",128064156},
            { "4A",128064157},
            { "3E",128064148},
            { "3D",128064149},
            { "3C",128064150},
            { "3B",128064151},
            { "3A",128064152},
            { "2E",128064143},
            { "2D",128064144},
            { "2C",128064145},
            { "2B",128064146},
            { "2A",128064147},
            { "1E",128064138},
            { "1D",128064139},
            { "1C",128064140},
            { "1B",128064141},
            { "1A",128064142},
        };
        static Dictionary<string, int> planetary_approach_suite = new Dictionary<string, int>
        {
            { "int_planetapproachsuite",128672317},
        };

        static Dictionary<string, int> power_distributor = new Dictionary<string, int>
        {
            { "8E",128064213},
            { "8D",128064214},
            { "8C",128064215},
            { "8B",128064216},
            { "8A",128064217},
            { "7E",128064208},
            { "7D",128064209},
            { "7C",128064210},
            { "7B",128064211},
            { "7A",128064212},
            { "6E",128064203},
            { "6D",128064204},
            { "6C",128064205},
            { "6B",128064206},
            { "6A",128064207},
            { "5E",128064198},
            { "5D",128064199},
            { "5C",128064200},
            { "5B",128064201},
            { "5A",128064202},
            { "4E",128064193},
            { "4D",128064194},
            { "4C",128064195},
            { "4B",128064196},
            { "4A",128064197},
            { "3E",128064188},
            { "3D",128064189},
            { "3C",128064190},
            { "3B",128064191},
            { "3A",128064192},
            { "2E",128064183},
            { "2D",128064184},
            { "2C",128064185},
            { "2B",128064186},
            { "2A",128064187},
            { "1E",128064178},
            { "1D",128064179},
            { "1C",128064180},
            { "1B",128064181},
            { "1A",128064182},
        };
        static Dictionary<string, int> power_plant = new Dictionary<string, int>
        {
            { "8E",128064063},
            { "8D",128064064},
            { "8C",128064065},
            { "8B",128064066},
            { "8A",128064067},
            { "7E",128064058},
            { "7D",128064059},
            { "7C",128064060},
            { "7B",128064061},
            { "7A",128064062},
            { "6E",128064053},
            { "6D",128064054},
            { "6C",128064055},
            { "6B",128064056},
            { "6A",128064057},
            { "5E",128064048},
            { "5D",128064049},
            { "5C",128064050},
            { "5B",128064051},
            { "5A",128064052},
            { "4E",128064043},
            { "4D",128064044},
            { "4C",128064045},
            { "4B",128064046},
            { "4A",128064047},
            { "3E",128064038},
            { "3D",128064039},
            { "3C",128064040},
            { "3B",128064041},
            { "3A",128064042},
            { "2E",128064033},
            { "2D",128064034},
            { "2C",128064035},
            { "2B",128064036},
            { "2A",128064037},
        };
        static Dictionary<string, int> sensors = new Dictionary<string, int>
        {
            { "8E",128064253},
            { "8D",128064254},
            { "8C",128064255},
            { "8B",128064256},
            { "8A",128064257},
            { "7E",128064248},
            { "7D",128064249},
            { "7C",128064250},
            { "7B",128064251},
            { "7A",128064252},
            { "6E",128064243},
            { "6D",128064244},
            { "6C",128064245},
            { "6B",128064246},
            { "6A",128064247},
            { "5E",128064238},
            { "5D",128064239},
            { "5C",128064240},
            { "5B",128064241},
            { "5A",128064242},
            { "4E",128064233},
            { "4D",128064234},
            { "4C",128064235},
            { "4B",128064236},
            { "4A",128064237},
            { "3E",128064228},
            { "3D",128064229},
            { "3C",128064230},
            { "3B",128064231},
            { "3A",128064232},
            { "2E",128064223},
            { "2D",128064224},
            { "2C",128064225},
            { "2B",128064226},
            { "2A",128064227},
            { "1E",128064218},
            { "1D",128064219},
            { "1C",128064220},
            { "1B",128064221},
            { "1A",128064222},
        };
        static Dictionary<string, int> thrusters = new Dictionary<string, int>
        {
            { "8E",128064098},
            { "8D",128064099},
            { "8C",128064100},
            { "8B",128064101},
            { "8A",128064102},
            { "7E",128064093},
            { "7D",128064094},
            { "7C",128064095},
            { "7B",128064096},
            { "7A",128064097},
            { "6E",128064088},
            { "6D",128064089},
            { "6C",128064090},
            { "6B",128064091},
            { "6A",128064092},
            { "5E",128064083},
            { "5D",128064084},
            { "5C",128064085},
            { "5B",128064086},
            { "5A",128064087},
            { "4E",128064078},
            { "4D",128064079},
            { "4C",128064080},
            { "4B",128064081},
            { "4A",128064082},
            { "3E",128064073},
            { "3D",128064074},
            { "3C",128064075},
            { "3B",128064076},
            { "3A",128064077},
            { "2E",128064068},
            { "2D",128064069},
            { "2C",128064070},
            { "2B",128064071},
            { "2A",128064072},
        };

        static Dictionary<string, int> thrusters_fast = new Dictionary<string, int>     // fast thrusters, evidence in FD folders of size3_class5_fast Ids
        {
            { "3A",128682013}, 
            { "2A",128682014}, 
        };

        static Dictionary<string, int> repair_limpet_controller = new Dictionary<string, int>
        {
            { "1E", 128777327 },
            { "1D", 128777328 },
            { "1C", 128777329 },
            { "1B", 128777330 },
            { "1A", 128777331 },
            { "3E", 128777332 },
            { "3D", 128777333 },
            { "3C", 128777334 },
            { "3B", 128777335 },
            { "3A", 128777336 },
            { "5E", 128777337 },
            { "5D", 128777338 },
            { "5C", 128777339 },
            { "5B", 128777340 },
            { "5A", 128777341 },
            { "7E", 128777342 },
            { "7D", 128777343 },
            { "7C", 128777344 },
            { "7B", 128777345 },
            { "7A", 128777346 },
        };

        #endregion

        #region Item name to Module List

        static Dictionary<string, Dictionary<string, int>> modules = new Dictionary<string, Dictionary<string, int>>
        {
            { "chafflauncher",chaff_launcher},      //V
            { "electroniccountermeasure",electronic_countermeasure},        //U - Elite has the same folder name
            { "heatsinklauncher",heat_sink_launcher},       // V
            { "plasmapointdefence",point_defence},      //V
            { "shieldbooster",shield_booster},      //V
            { "cloudscanner",frame_shift_wake_scanner}, //V
            { "crimescanner",kill_warrant_scanner}, //V
            { "cargoscanner",cargo_scanner},    //V

            { "repairer",auto_field_maintenance_unit},                      // V
            { "cargorack",cargo_rack},                                      //V
            { "corrosionproofcargorack",corrosion_cargo_rack },             // U TBD fd folders calls it this.  classes appear screwed up
            { "dronecontrol_collection",collector_limpet_controllers},      //V
            { "dockingcomputer",docking_computer},                          // V
            { "fsdinterdictor",frame_shift_drive_interdictor},              //V
            { "fuelscoop",fuel_scoop},                                      //V
            { "dronecontrol_fueltransfer",fuel_transfer_limpet_controllers},    //V
            { "dronecontrol_resourcesiphon",hatch_breaker_limpet_controller},   //V
            { "hullreinforcement",hull_reinforcement_package},  //V
            { "modulereinforcement",module_reinforcement_package},  //V
            { "passengercabin",passenger_cabin},    // economy first class checked..
            { "dronecontrol_prospector",prospector_limpet_controllers}, //V
            { "stellarbodydiscoveryscanner",discoveryscanner },             // V
            { "detailedsurfacescanner",detailedscanner},        //V
            { "fighterbay",fighter_hangar},     //PV
            { "buggybay",planetary_vehicle_hanger},     //V
            { "refinery",refinery}, //V
            { "shieldcellbank",shield_cell_bank}, //V
            { "biweaveshieldgenerator",bi_weave_shield_generator},          // V. NOT occuring in ED, specially redirected by code in ID. Have _Fast at end of id, checked, and  Evidence ED folders, have _fast at end
            { "pristmaticshieldgenerator",pristmatic_shield_generator},     //V. NOT occuring in ED, specially redirected by code in ID.  Evidence ED folders, have _strong at end
            { "shieldgenerator",shield_generator},  //V

            { "axmissilerack",ax_missile_rack},
            { "axmulticannon",ax_multi_cannon},
            { "advancedtorppylon",torpedo_pylon},       //V
            { "beamlaser",beam_laser},                  //V
            { "pulselaserburst",burst_laser},           //V
            { "cannon",cannon},                         //V
            { "slugshot",fragment_cannon},              //V
            { "minelauncher",mine_launcher},            //V, shock
            { "mininglaser",mining_laser},              //V
            { "dumbfiremissilerack",dumbfiremissile_rack},  //V
            { "drunkmissilerack",drunkmissile_rack},  // TBD
            { "basicmissilerack",seekermissile_rack},   //V
            { "multicannon",multi_cannon},              //V
            { "plasmaaccelerator",plasma_accelerator},      //V
            { "pulselaser",pulse_laser},            //V
            { "railgun",rail_gun},                  //V
            { "remotereleaseflaklauncher",remote_release_flak_launcher},
            { "shutdownfieldneutraliser",shutdown_field_neutraliser},
            { "xenoscanner",xeno_scanner},

            { "hyperdrive",frame_shift_drive},  //V
            { "fueltank",fuel_tank},    //V
            { "lifesupport",life_support},  //V
            { "planetapproachsuite",planetary_approach_suite},  //V
            { "powerdistributor",power_distributor},    //V
            { "powerplant",power_plant},    //V
            { "sensors",sensors},   //V
            { "engine",thrusters},  //V
            { "enginefast",thrusters_fast},  // U NOT occuring in ED, specially redirected by code in ID. Evidence ED folders, engines having fast at end
            { "dronecontrol_repair", repair_limpet_controller },
        };

        #endregion

        #region ship to Armour type

        static Dictionary<string, int> adder = new Dictionary<string, int>
        {
            { "grade1",128049268},
            { "grade2",128049269},
            { "grade3",128049270},
            { "mirrored",128049271},
            { "reactive",128049272},
        };
        static Dictionary<string, int> anaconda = new Dictionary<string, int>
        {
            { "grade1",128049364},
            { "grade2",128049365},
            { "grade3",128049366},
            { "mirrored",128049367},
            { "reactive",128049368},
        };
        static Dictionary<string, int> asp = new Dictionary<string, int>
        {
            { "grade1",128049304},
            { "grade2",128049305},
            { "grade3",128049306},
            { "mirrored",128049307},
            { "reactive",128049308},
        };
        static Dictionary<string, int> asp_scout = new Dictionary<string, int>
        {
            { "grade1",128672278},
            { "grade2",128672279},
            { "grade3",128672280},
            { "mirrored",128672281},
            { "reactive",128672282},
        };
        static Dictionary<string, int> beluga = new Dictionary<string, int>
        {
            { "grade1",128049346},
            { "grade2",128049347},
            { "grade3",128049348},
            { "mirrored",128049349},
            { "reactive",128049350},
        };
        static Dictionary<string, int> cobra_mk_iii = new Dictionary<string, int>
        {
            { "grade1",128049280},
            { "grade2",128049281},
            { "grade3",128049282},
            { "mirrored",128049283},
            { "reactive",128049284},
        };
        static Dictionary<string, int> cobra_mk_iv = new Dictionary<string, int>
        {
            { "grade1",128672264},
            { "grade2",128672265},
            { "grade3",128672266},
            { "mirrored",128672267},
            { "reactive",128672268},
        };
        static Dictionary<string, int> diamondback_explorer = new Dictionary<string, int>
        {
            { "grade1",128671832},
            { "grade2",128671833},
            { "grade3",128671834},
            { "mirrored",128671835},
            { "reactive",128671836},
        };
        static Dictionary<string, int> diamondback = new Dictionary<string, int>
        {
            { "grade1",128671218},
            { "grade2",128671219},
            { "grade3",128671220},
            { "mirrored",128671221},
            { "reactive",128671222},
        };
        static Dictionary<string, int> eagle = new Dictionary<string, int>
        {
            { "grade1",128049256},
            { "grade2",128049257},
            { "grade3",128049258},
            { "mirrored",128049259},
            { "reactive",128049260},
        };
        static Dictionary<string, int> federal_assault_ship = new Dictionary<string, int>
        {
            { "grade1",128672147},
            { "grade2",128672148},
            { "grade3",128672149},
            { "mirrored",128672150},
            { "reactive",128672151},
        };
        static Dictionary<string, int> federal_corvette = new Dictionary<string, int>
        {
            { "grade1",128049370},
            { "grade2",128049371},
            { "grade3",128049372},
            { "mirrored",128049373},
            { "reactive",128049374},
        };
        static Dictionary<string, int> federal_dropship = new Dictionary<string, int>
        {
            { "grade1",128049322},
            { "grade2",128049323},
            { "grade3",128049324},
            { "mirrored",128049325},
            { "reactive",128049326},
        };
        static Dictionary<string, int> federal_gunship = new Dictionary<string, int>
        {
            { "grade1",128672154},
            { "grade2",128672155},
            { "grade3",128672156},
            { "mirrored",128672157},
            { "reactive",128672158},
        };
        static Dictionary<string, int> fer_de_lance = new Dictionary<string, int>
        {
            { "grade1",128049352},
            { "grade2",128049353},
            { "grade3",128049354},
            { "mirrored",128049355},
            { "reactive",128049356},
        };
        static Dictionary<string, int> hauler = new Dictionary<string, int>
        {
            { "grade1",128049262},
            { "grade2",128049263},
            { "grade3",128049264},
            { "mirrored",128049265},
            { "reactive",128049266},
        };
        static Dictionary<string, int> imperial_clipper = new Dictionary<string, int>
        {
            { "grade1",128049316},
            { "grade2",128049317},
            { "grade3",128049318},
            { "mirrored",128049319},
            { "reactive",128049320},
        };
        static Dictionary<string, int> imperial_courier = new Dictionary<string, int>
        {
            { "grade1",128671224},
            { "grade2",128671225},
            { "grade3",128671226},
            { "mirrored",128671227},
            { "reactive",128671228},
        };
        static Dictionary<string, int> imperial_cutter = new Dictionary<string, int>
        {
            { "grade1",128049376},
            { "grade2",128049377},
            { "grade3",128049378},
            { "mirrored",128049379},
            { "reactive",128049380},
        };
        static Dictionary<string, int> imperial_eagle = new Dictionary<string, int>
        {
            { "grade1",128672140},
            { "grade2",128672141},
            { "grade3",128672142},
            { "mirrored",128672143},
            { "reactive",128672144},
        };
        static Dictionary<string, int> keelback = new Dictionary<string, int>
        {
            { "grade1",128672271},
            { "grade2",128672272},
            { "grade3",128672273},
            { "mirrored",128672274},
            { "reactive",128672275},
        };
        static Dictionary<string, int> orca = new Dictionary<string, int>
        {
            { "grade1",128049328},
            { "grade2",128049329},
            { "grade3",128049330},
            { "mirrored",128049331},
            { "reactive",128049332},
        };
        static Dictionary<string, int> python = new Dictionary<string, int>
        {
            { "grade1",128049340},
            { "grade2",128049341},
            { "grade3",128049342},
            { "mirrored",128049343},
            { "reactive",128049344},
        };
        static Dictionary<string, int> sidewinder = new Dictionary<string, int>
        {
            { "grade1",128049250},
            { "grade2",128049251},
            { "grade3",128049252},
            { "mirrored",128049253},
            { "reactive",128049254},
        };
        static Dictionary<string, int> type_6_transporter = new Dictionary<string, int>
        {
            { "grade1",128049286},
            { "grade2",128049287},
            { "grade3",128049288},
            { "mirrored",128049289},
            { "reactive",128049290},
        };
        static Dictionary<string, int> type_7_transport = new Dictionary<string, int>
        {
            { "grade1",128049298},
            { "grade2",128049299},
            { "grade3",128049300},
            { "mirrored",128049301},
            { "reactive",128049302},
        };
        static Dictionary<string, int> type_9_heavy = new Dictionary<string, int>
        {
            { "grade1",128049334},
            { "grade2",128049335},
            { "grade3",128049336},
            { "mirrored",128049337},
            { "reactive",128049338},
        };
        static Dictionary<string, int> type_10_defender = new Dictionary<string, int>
        {
            { "grade1",128049334},
            { "grade2",128049335},
            { "grade3",128049336},
            { "mirrored",128049337},
            { "reactive",128049338},
        };
        static Dictionary<string, int> viper = new Dictionary<string, int>
        {
            { "grade1",128049274},
            { "grade2",128049275},
            { "grade3",128049276},
            { "mirrored",128049277},
            { "reactive",128049278},
        };
        static Dictionary<string, int> viper_mk_iv = new Dictionary<string, int>
        {
            { "grade1",128672257},
            { "grade2",128672258},
            { "grade3",128672259},
            { "mirrored",128672260},
            { "reactive",128672261},
        };
        static Dictionary<string, int> vulture = new Dictionary<string, int>
        {
            { "grade1",128049310},
            { "grade2",128049311},
            { "grade3",128049312},
            { "mirrored",128049313},
            { "reactive",128049314},
        };

        #endregion

        #region Ship to armour list

        static Dictionary<string, Dictionary<string, int>> ships = new Dictionary<string, Dictionary<string, int>>
        {
            { "Adder",adder},                                       // ship name is frontier offical used in logs
            { "Anaconda",anaconda},
            { "Asp",asp},
            { "Asp_Scout",asp_scout},
            { "BelugaLiner",beluga},
            { "CobraMkIII",cobra_mk_iii},
            { "CobraMkIV",cobra_mk_iv},
            { "DiamondBackXL",diamondback_explorer},
            { "DiamondBack",diamondback},
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
            { "Independant_Trader",keelback},
            { "Orca",orca},
            { "Python",python},
            { "SideWinder",sidewinder},
            { "Type6",type_6_transporter},
            { "Type7",type_7_transport},
            { "Type9",type_9_heavy},
            { "Type9_Military",type_10_defender},
            { "Viper",viper},
            { "Viper_MkIV",viper_mk_iv},
            { "Vulture",vulture},
        };

        #endregion

    }
}
