/*
 * Copyright © 2015 - 2017 EDDiscovery development team
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.EliteDangerous
{
    class JournalFieldNaming
    {
        public static string RMat(string s)            // replacer for pretty print
        {
            EDDiscovery2.DB.MaterialCommodity mc = EDDiscovery2.DB.MaterialCommodity.GetCachedMaterial(s);

            if (mc != null)
                return mc.name;
            else
            {
                StringBuilder ret = new StringBuilder();        //Phroggsters method

                if (s.Length >= 8 && s.StartsWith("$") && s.EndsWith("_name;", StringComparison.InvariantCultureIgnoreCase))
                {
                    ret.Append(s.Substring(1, s.Length - 7)); // 1 for '$' plus 6 for '_name;'
                    return ret.ToString().ToLowerInvariant();
                }
                else
                    return s;
            }
        }

        public static string RLat(double lv)      
        {
            long arcsec = (long)(lv * 60 * 60);          // convert to arc seconds
            string marker = (arcsec < 0) ? "S" : "N";       // presume lat
            arcsec = Math.Abs(arcsec);
            return string.Format("{0}°{1} {2}'{3}\"", arcsec / 3600, marker, (arcsec / 60) % 60, arcsec % 60);
        }

        public static string RLong(double lv)      
        {
            long arcsec = (long)(lv * 60 * 60);          // convert to arc seconds
            string marker = (arcsec < 0) ? "W" : "E";       // presume lat
            arcsec = Math.Abs(arcsec);
            return string.Format("{0}°{1} {2}'{3}\"", arcsec / 3600, marker, (arcsec / 60) % 60, arcsec % 60);
        }

        private static Dictionary<string, string> shipnames = new Dictionary<string, string>()
        {
                { "adder" ,                     "Adder"},
                { "anaconda",                   "Anaconda" },
                { "asp",                        "Asp Explorer" },
                { "asp_scout",                  "Asp Scout" },
                { "belugaliner",                "Beluga Liner" },
                { "cobramkiii",                 "Cobra Mk. III" },
                { "cobramkiv",                  "Cobra Mk. IV" },
                { "cutter",                     "Imperial Cutter" },
                { "diamondback",                "Diamondback Scout" },
                { "diamondbackxl",              "Diamondback Explorer" },
                { "eagle",                      "Eagle" },
                { "empire_courier",             "Imperial Courier" },
                { "empire_eagle",               "Imperial Eagle" },
                { "empire_fighter",             "Imperial Fighter" },
                { "empire_trader",              "Imperial Clipper" },
                { "federation_corvette",        "Federal Corvette" },
                { "federation_dropship",        "Federal Dropship" },
                { "federation_dropship_mkii",   "Federal Assault Ship" },
                { "federation_gunship",         "Federal Gunship" },
                { "federation_fighter",         "F63 Condor" },
                { "ferdelance",                 "Fer-de-Lance" },
                { "hauler",                     "Hauler" },
                { "independant_trader",         "Keelback" },
                { "orca",                       "Orca" },
                { "python",                     "Python" },
                { "sidewinder",                 "Sidewinder" },
                { "type6",                      "Type 6 Transporter" },
                { "type7",                      "Type 7 Transporter" },
                { "type9",                      "Type 9 Heavy" },
                { "viper",                      "Viper Mk. III" },
                { "viper_mkiv",                 "Viper Mk. IV" },
                { "vulture",                    "Vulture" },
                { "testbuggy",                  "SRV" },
        };

        static public string GetBetterShipName(string inname)
        {
            return shipnames.ContainsKey(inname.ToLower()) ? shipnames[inname.ToLower()] : inname;
        }

        static public bool IsSRV(string inname) // better name
        {
            return inname.Contains("SRV");
        }

        static public bool IsFighter(string inname) // better name
        {
            return inname.Contains("F63") || inname.Contains("Fighter");
        }

        static public bool IsSRVOrFighter(string inname )
        {
            return IsSRV(inname) || IsFighter(inname);
        }

        static public string PhoneticShipName(string inname)
        {
            return inname.Replace("Mk. IV", "Mark 4").Replace("Mk. III", "Mark 3");
        }

        static public string GetBetterMissionName(string inname)
        {
            return inname.Replace("_name", "").SplitCapsWordFull();
        }

        static public string GetBetterSlotName(string s)
        {
            return s.SplitCapsWordFull();
        }

        static Dictionary<string, string> replaceevents = new Dictionary<string, string>
        {
            // see the program folder win64\items\shipmodule

            // general
            {"bobbleheads",     "Bobble Heads"},
            {"decal",     "Decal"},
            {"heatradiator",     "Heat Radiator"},                                     

            // HPTs (also in Hpt_defencemodifier)
            {"advancedtorppylon",     "Advanced Torp Pylon"},
            {"basicmissilerack",     "Basic Missile Rack"},
            {"beamlaser",     "Beam Laser"},                                     // V
            {"beampointdefence",     "Beam Point Defence"},                                     // V
            {"cannon",     "Cannon"},
            {"chafflauncher",     "Chaff Launcher"},
            {"drunkmissilerack",     "Drunk Missile Rack"},
            {"dumbfiremissilerack",     "Dumbfire Missile Rack"},
            {"electroniccountermeasure",     "Electronic Counter Measure"},
            {"enforcementlight",     "Enforcement Light"},
            {"heatsinklauncher",     "Heat Sink Launcher"},                       
            {"minelauncher",     "Mine Launcher"},
            {"mininglaser",     "Mining Laser"},
            {"multicannon",     "Multi Cannon"},                                 // V
            {"plasmaaccelerator",     "Plasma Accelerator"},
            {"plasmapointdefence",     "Plasma Point Defence"},                   // V
            {"pulselaser",     "Pulse Laser"},
            {"pulselaserburst",     "Pulse Laser Burst"},
            {"railgun",     "Rail Gun"},
            {"scanners",     "Scanners"},
            {"shieldbooster",     "Shield Booster"},
            {"slugshot",     "Slugshot"},

            // Int_
            {"buggybay",     "Buggy Bay"},                                       // V
            {"cargorack",     "Cargo Rack"},                                     // V
            {"detailedsurfacescanner",     "Detailed Surface Scanner"},           // V
            {"dockingcomputer",     "Docking Computer"},
            {"dronecontrol",     "Drone Control"},
            {"engine",     "Engine"},
            {"fighterbay",     "Fighter Bay"},
            {"fsdinterdictor",     "FSD Interdictor"},
            {"fuelscoop",     "Fuel Scoop"},                                     // V
            {"fueltank",     "Fuel Tank"},                                       // V
            {"hullreinforcement",     "Hull Reinforcement"},
            {"modulereinforcement",     "Module Reinforcement"},
            {"hyperdrive",     "Hyperdrive"},                                   // V
            {"lifesupport",     "Life Support"},                                 // V
            {"passengercabin",     "Passenger Cabin"},
            {"planetapproachsuite",     "Planet Approach Suite"},                 // V
            {"powerdistributor",     "Power Distributor"},                       // V INT_ and $int
            {"powerplant",     "Powerplant"},                                   // V
            {"radar",     "Radar"},                                             // V
            {"repairer",     "Repairer"},
            {"sensors",     "Sensors"},
            {"shieldcellbank",     "Shield Cell Bank"},
            {"shieldgenerator",     "Shield Generator"},

            // not in folder but found in logs

            {"cargoscanner",     "Cargo Scanner"},
            {"cloudscanner",     "Cloud Scanner"},
            {"corrosionproofcargorack",     "Corrosion Proof Cargo Rack"},
            {"crimescanner",     "Crime Scanner"},
            {"defencecrimescanner",     "Defence Crime Scanner"},
            {"refinery",     "Refinery"},
            {"stellarbodydiscoveryscanner",     "Stellar Body Discovery Scanner"}, // V

            {"class1" , "Rating E" },
            {"class2" , "Rating D" },
            {"class3" , "Rating C" },
            {"class4" , "Rating B" },
            {"class5" , "Rating A" },

            {"size1" , "Class 1" },
            {"size2" , "Class 2" },
            {"size3" , "Class 3" },
            {"size4" , "Class 4" },
            {"size5" , "Class 5" },
            {"size6" , "Class 6" },
            {"size7" , "Class 7" },
            {"size8" , "Class 8" },
        };

        static public string GetBetterItemNameEvents(string s)            // for all except loadout.. has to deal with $int and $hpt
        {
            //string x = s;
            if (s.StartsWith("$int_") || s.StartsWith("$hpt_"))
                s = s.Substring(5);
            if (s.EndsWith("_name;"))
                s = s.Substring(0, s.Length - 6);

            s = s.SplitCapsWordFull(replaceevents);
            //System.Diagnostics.Debug.WriteLine("PP Item " + x + " >> " + s);
            return s;
        }

        static Dictionary<string, string> replaceloadouts = new Dictionary<string, string>
        {
            {"Class1" , "Rating E" },
            {"Class2" , "Rating D" },
            {"Class3" , "Rating C" },
            {"Class4" , "Rating B" },
            {"Class5" , "Rating A" },

            {"Size1" , "Class 1" },
            {"Size2" , "Class 2" },
            {"Size3" , "Class 3" },
            {"Size4" , "Class 4" },
            {"Size5" , "Class 5" },
            {"Size6" , "Class 6" },
            {"Size7" , "Class 7" },
            {"Size8" , "Class 8" },
        };

        static public string GetBetterItemNameLoadout(string s)
        {
            //string x = s;
            if (s.StartsWith("Int_") || s.StartsWith("Hpt_"))
                s = s.Substring(4);
            s = s.SplitCapsWordFull(replaceloadouts);
            //System.Diagnostics.Debug.WriteLine("LO Item " + x + " >> " + s);
            return s;
        }

    }
}
