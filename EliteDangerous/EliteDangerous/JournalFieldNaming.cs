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


namespace EliteDangerousCore
{
    public static class JournalFieldNaming
    {
        public static string FixCommodityName(string fdname)      // instances in log on mining and mission entries of commodities in this form, back into fd form
        {
            if (fdname.Length >= 8 && fdname.StartsWith("$") && fdname.EndsWith("_name;", System.StringComparison.InvariantCultureIgnoreCase))
                fdname = fdname.Substring(1, fdname.Length - 7); // 1 for '$' plus 6 for '_name;'

            return fdname;
        }

        static public string FDNameTranslation(string old)
        {
            return MaterialCommodityDB.FDNameTranslation(old);
        }

        public static string RMat(string fdname)            // fix up fdname into a nicer name
        {
            MaterialCommodityDB mc = MaterialCommodityDB.GetCachedMaterial(fdname);

            if (mc != null)
                return mc.name;
            else
                return fdname.SplitCapsWordFull();
        }

        public static string RLat(double? lv)
        {
            if (lv.HasValue)
                return RLat(lv.Value);
            else
                return null;
        }


        public static string RLat(double lv)      
        {
            long arcsec = (long)(lv * 60 * 60);          // convert to arc seconds
            string marker = (arcsec < 0) ? "S" : "N";       // presume lat
            arcsec = Math.Abs(arcsec);
            return string.Format("{0}°{1} {2}'{3}\"", arcsec / 3600, marker, (arcsec / 60) % 60, arcsec % 60);
        }

        public static string RLong(double? lv)
        {
            if (lv.HasValue)
                return RLong(lv.Value);
            else
                return null;
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
                { "dolphin",                    "Dolphin" },
                { "eagle",                      "Eagle" },
                { "empire_courier",             "Imperial Courier" },
                { "empire_eagle",               "Imperial Eagle" },
                { "empire_fighter",             "Imperial Fighter" },
                { "empire_trader",              "Imperial Clipper" },
                { "federation_corvette",        "Federal Corvette" },
                { "federation_dropship_mkii",   "Federal Assault Ship" },
                { "federation_dropship",        "Federal Dropship" },
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
                { "type9_military",             "Type 10 Defender" },
                { "viper",                      "Viper Mk. III" },
                { "viper_mkiv",                 "Viper Mk. IV" },
                { "vulture",                    "Vulture" },
                { "testbuggy",                  "SRV" },
                { "typex",                      "Alliance Chieftain"},
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

        static public string GetBetterMissionName(string inname)
        {
            return inname.Replace("_name", "").SplitCapsWordFull();
        }

        static public string ShortenMissionName(string inname)
        {
            return inname.Replace("Mission ", "",StringComparison.InvariantCultureIgnoreCase).SplitCapsWordFull();
        }

        static Dictionary<string, string> replaceslots = new Dictionary<string, string>
        {
            {"Engines",     "Thrusters"},
        };

        static public string GetBetterSlotName(string s)
        {
            return s.SplitCapsWordFull(replaceslots);
        }

        static Dictionary<string, string> replaceitems = new Dictionary<string, string>
        {
            // see the program folder win64\items\shipmodule

            // general
            {"bobbleheads",     "Bobble Heads"},
            {"decal",     "Decal"},
            {"heatradiator",     "Heat Radiator"},

            // HPTs (also in Hpt_defencemodifier)
            {"advancedtorppylon",     "Advanced Torp Pylon"},
            {"basicmissilerack",     "Seeker Missile Rack"},
            {"beamlaser",     "Beam Laser"},                                     // V
            {"beampointdefence",     "Beam Point Defence"},                                     // V
            {"cannon",     "Cannon"},
            {"chafflauncher",     "Chaff Launcher"},
            {"drunkmissilerack",     "Pack Hound Missile Rack"},
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
            {"slugshot",     "Fragment Cannon"},
            {"fueltransfer",     "Fuel Transfer"},

            // Int_
            {"buggybay",     "Planetary Vehicle Hangar"},                                       // V
            {"cargorack",     "Cargo Rack"},                                     // V
            {"detailedsurfacescanner",     "Detailed Surface Scanner"},           // V
            {"dockingcomputer",     "Docking Computer"},
            {"dronecontrol",     "Drone Control"},
            {"engine",     "Thrusters"},
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
            {"powerplant",     "Power Plant"},                                   // V
            {"radar",     "Sensors"},                                             // V
            {"repairer",     "Auto Field Maintenance"},
            {"sensors",     "Sensors"},
            {"shieldcellbank",     "Shield Cell Bank"},
            {"shieldgenerator",     "Shield Generator"},
            {"resourcesiphon",     "Hatch Breaker"},

            // not in folder but found in logs

            {"cargoscanner",     "Cargo Scanner"},
            {"cloudscanner",     "Hyperspace Cloud Scanner"},
            {"corrosionproofcargorack",     "Corrosion Proof Cargo Rack"},
            {"crimescanner",     "Crime Scanner"},
            {"defencecrimescanner",     "Defence Crime Scanner"},
            {"refinery",     "Refinery"},
            {"stellarbodydiscoveryscanner",     "Stellar Body Discovery Scanner"}, // V
            {"shipdatalinkscanner", "Data Link Scanner" },
            {"modularcargobaydoorfdl", "Fer-De-Lance Cargo Bay Door" },
            {"modularcargobaydoor", "Cargo Bay Door" },

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

            {"Size1" , "Class 1" },     // need these because Key lookup is case sensitive
            {"Size2" , "Class 2" },
            {"Size3" , "Class 3" },
            {"Size4" , "Class 4" },
            {"Size5" , "Class 5" },
            {"Size6" , "Class 6" },
            {"Size7" , "Class 7" },
            {"Size8" , "Class 8" },

            {"Class1" , "Rating E" },
            {"Class2" , "Rating D" },
            {"Class3" , "Rating C" },
            {"Class4" , "Rating B" },
            {"Class5" , "Rating A" },

            {"Engine",     "Thrusters"},
            {"Basic",     "Seeker"},
            {"Drunk",     "Pack Hound"},
            {"Slugshot",     "Fragment Cannon"},
            {"Buggy",     "Planetary Vehicle"},              // V
            {"Bay",     "Hangar"},                        // V
            {"Resourcesiphon",     "Hatch Breaker"},        //TBD
            {"Repairer",     "Auto Field Maintenance"},     //TBD
            {"Cloudscanner",     "Hyperspace Cloud Scanner"}, //TBD
        };

        static public string GetBetterItemNameEvents(string s)            // for all except loadout.. has to deal with $int and $hpt
        {
            //string x = s;
            if (s.StartsWith("$int_", StringComparison.InvariantCultureIgnoreCase) || s.StartsWith("$hpt_", StringComparison.InvariantCultureIgnoreCase))     // events do that
                s = s.Substring(5);
            if (s.StartsWith("int_", StringComparison.InvariantCultureIgnoreCase) || s.StartsWith("hpt_", StringComparison.InvariantCultureIgnoreCase))       // outfitting.json
                s = s.Substring(4);
            else if (s.StartsWith("$"))
                s = s.Substring(1);
            if (s.EndsWith("_name;"))
                s = s.Substring(0, s.Length - 6);

            s = s.SplitCapsWordFull(replaceitems, shipnames);

            if (s.Contains("Planetary Vehicle Hangar"))                 // strange class naming, fix up after above.. don't want two tables
                s = s.Replace("Rating E", "Rating H").Replace("Rating D", "Rating G");

            //System.Diagnostics.Debug.WriteLine("PP Item " + x + " >> " + s);

            return s;
        }

        static public string NormaliseFDItemName(string s)      // has to deal with $int and $hpt.. This takes the FD name and keeps it, but turns it into the form
        {                                                       // used by Coriolis/Frontier API
            //string x = s;
            if (s.StartsWith("$int_"))
                s = s.Replace("$int_", "Int_");
            if (s.StartsWith("int_"))
                s = s.Replace("int_", "Int_");
            if (s.StartsWith("$hpt_"))
                s = s.Replace("$hpt_", "Hpt_");
            if (s.StartsWith("hpt_"))
                s = s.Replace("hpt_", "Hpt_");
            if (s.Contains("_armour_"))
                s = s.Replace("_Armour_", "_armour_");      // normalise Armour to now lower case.. as done in journals
            if (s.EndsWith("_name;"))
                s = s.Substring(0, s.Length - 6);

            return s;
        }

        static public string NormaliseFDSlotName(string s)            // FD slot name, anything to do.. leave in as there might be in the future
        {
            return s;
        }

        static public string NormaliseFDShipName(string s)            // FD ship names.. tend to change case.. Fix
        {
            s = ShipModuleData.Instance.NormaliseShipName(s);
            return s;
        }

        static public string GetBetterTargetTypeName(string s)      // has to deal with $ and underscored
        {
            //string x = s;
            if (s.StartsWith("$"))
                s = s.Substring(1);
            return s.SplitCapsWordFull();
        }

    }
}
