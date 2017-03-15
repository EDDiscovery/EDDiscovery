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
        public static JSONConverters StandardConverters()
        {
            JSONConverters jc = new JSONConverters();

            {           // unique field names across multiple entries.  First up so later ones can override if required

                jc.AddScale("MassEM", 1.0, "0.0000'em'", "Mass");
                jc.AddScale("MassMT", 1.0, "0.0'mt'", "Mass");
                jc.AddScale("SurfacePressure", 1.0, "0.0'p'");
                jc.AddScale("Radius", 1.0 / 1000, "0.0'km'");
                jc.AddScale("InnerRad", 1.0 / 1000, "0.0'km'", "Inner Radius");
                jc.AddScale("OuterRad", 1.0 / 1000, "0.0'km'", "Outer Radius");
                jc.AddScale("SemiMajorAxis", 1.0 / 1000, "0.0'km'", "Semi Major Axis");
                jc.AddScale("OrbitalPeriod;RotationPeriod", 1.0 / 86400, "0.0' days orbit'", "");
                jc.AddScale("SurfaceGravity", 1.0 / 9.8, "0.0'g'");
                jc.AddScale("SurfaceTemperature", 1.0, "0.0'K'");
                jc.AddScale("Scooped", 1.0, "'Scooped '0.0't'", "", "FuelScoop");
                jc.AddScale("Total", 1.0, "'Fuel Level '0.0't'", "", "FuelScoop");
                jc.AddScale("FuelUsed", 1.0, "'Fuel Used '0.0't'", "");
                jc.AddScale("FuelLevel", 1.0, "'Fuel Level Left '0.0't'", "");
                jc.AddScale("Amount", 1.0, "'Fuel Bought '0.0't'", "", "RefuelAll");
                jc.AddScale("BoostValue", 1.0, "0.0' boost'", "", "JetConeBoost");
                jc.AddScale("StarPos", 1.0, "0.0", "");          // any entry StarPos loses it name (inside arrays). StarPos as an array name gets printed sep.

                jc.AddBool("TidalLock", "Not Tidally Locked", "Tidally Locked", ""); // remove name
                jc.AddBool("Landable", "Not Landable", "Landable", ""); // remove name
                jc.AddBool("ShieldsUp", "Shields Down", "Shields Up Captain", ""); // remove name
                jc.AddState("TerraformState", "Not Terrraformable", "");    // remove name
                jc.AddState("Atmosphere", "No Atmosphere", "");
                jc.AddState("Volcanism", "No Volcanism", "");
                jc.AddPrePostfix("StationType", "; Type", "");
                jc.AddPrePostfix("StationName", "; Station", "");
                jc.AddPrePostfix("DestinationSystem", "; Destination Star System", "");
                jc.AddPrePostfix("DestinationStation", "; Destination Station", "");
                jc.AddPrePostfix("StarSystem;System", "; Star System", "");
                jc.AddPrePostfix("Allegiance", "; Allegiance", "");
                jc.AddPrePostfix("Security", "; Security", "");
                jc.AddPrePostfix("Faction", "; Faction", "");
                jc.AddPrePostfix("Government", "Government Type ", "");
                jc.AddPrePostfix("Economy", "Economy Type ", "");
                jc.AddBool("Docked", "Not Docked", "Docked", "");   // remove name
                jc.AddBool("PlayerControlled", "NPC Controlled", "Player Controlled", ""); // remove name

                jc.AddPrePostfix("Body", "At ", "");

                jc.AddPrePostfix("To", "To ", "", "VehicleSwitch");
                jc.AddPrePostfix("Name", "", "", "CrewAssign");

                jc.AddPrePostfix("Role", "; role", "", "CrewAssign");
                jc.AddPrePostfix("Cost;ShipPrice;BaseValue", "; credits", "");
                jc.AddPrePostfix("Bonus", "; credits bonus", "");
                jc.AddPrePostfix("Amount", "; credits", "", "PayLegacyFines");
                jc.AddPrePostfix("BuyPrice", "Bought for ; credits", "");
                jc.AddPrePostfix("SellPrice", "Sold for ; credits", "");
                jc.AddPrePostfix("TotalCost", "Total cost ; credits", "");

                jc.AddPrePostfix("LandingPad", "On pad ", "");

                jc.AddPrePostfix("BuyItem", "; bought", "");
                jc.AddPrePostfix("SellItem", "; sold", "");

                jc.AddPrePostfix("Credits", "; credits", "", "LoadGame");

                jc.AddPrePostfix("Ship;ShipType", "Ship ;", "", replacer: GetBetterShipName);
                jc.AddPrePostfix("StoreOldShip;SellOldShip", "; stored", "", replacer: GetBetterShipName);

                jc.AddScale("Health", 100.0, "'Health' 0.0'%'", "");

                jc.AddPrePostfix("Latitude", "", replacer: RLat);
                jc.AddPrePostfix("Longitude", "", replacer: RLong);

                jc.AddPrePostfix("Reward", "; credits", "");
            }

            {           //missions
                jc.AddPrePostfix("Name", "", "", "MissionAccepted;MissionAbandoned;MissionCompleted;MissionFailed", GetBetterMissionName);
            }

            {           // transfers
                string transfer = JL(new[] { JournalTypeEnum.ShipyardTransfer });
                jc.AddScale("Distance", 1.0 / 299792458.0 / 365 / 24 / 60 / 60, "'Distance' 0.0'ly'", "", transfer);
                jc.AddPrePostfix("TransferPrice", "; credits", "", transfer);
            }

            {           // misc
                jc.AddPrePostfix("Name", "; settlement", "", "ApproachSettlement");
                jc.AddPrePostfix("Item", ";", "", "Repair");
            }

            {           // scans
                string scan = JL(new[] { JournalTypeEnum.Scan });
                jc.AddPrePostfix("BodyName", "Scan ", "", scan);
                jc.AddScale("DistanceFromArrivalLS", 1.0, "0.0' ls from arrival point'", "", scan);
                jc.AddPrePostfix("StarType", "; type star", "", scan);
                jc.AddScale("StellarMass", 1.0, "0.0' stellar masses'", "", scan);
                jc.AddScale("Radius", 1.0 / 1000.0, "0.0' km radius'", "", scan);
                jc.AddScale("AbsoluteMagnitude", 1.0, "0.0' absolute magnitude'", "", scan);
                jc.AddScale("OrbitalPeriod", 1.0 / 86400, "0.0' days orbit'", "", scan);
                jc.AddScale("RotationPeriod", 1.0 / 86400, "0.0' days rotation'", "", scan);
                jc.AddPrePostfix("PlanetClass", "; planet class", "", scan);

            }

            {           // engineering
                string engineer = JL(new[] { JournalTypeEnum.EngineerProgress, JournalTypeEnum.EngineerApply, JournalTypeEnum.EngineerCraft });
                jc.AddPrePostfix("Engineer", "From ", "", engineer);
                jc.AddPrePostfix("Progress", "", "", engineer);
            }

            {           // bounties
            }

            {
                string rank = JL(new[] { JournalTypeEnum.Rank });
                jc.AddIndex("Combat", "; combat;0;Harmless;Mostly Harmless;Novice;Competent;Expert;Master;Dangerous;Deadly;Elite", "", rank);
                jc.AddIndex("Trade", "; trader;0;Penniless;Mostly Penniless;Peddler;Dealer;Merchant;Broker;Entrepreneur;Tycoon;Elite", "", rank);
                jc.AddIndex("Explore", "; explorer;0;Aimless;Mostly Aimless;Scout;Surveyor;Trailblazer;Pathfinder;Ranger;Pioneer;Elite", "", rank);
                jc.AddIndex("Empire", "; Empire;0;None;Outsider;Serf;Master;Squire;Knight;Lord;Baron;Viscount;Count;Earl;Marquis;Duke;Prince;King", "", rank);
                jc.AddIndex("Federation", "; Federation;0;None;Recruit;Cadet;Midshipman;Petty Officer;Chief Pretty Officer;Warren Officer;Ensign;Lieutenant;Lieutenant Commander;Post Commander;Post Captain;Rear Admiral;Vice Admiral;Admiral", "", rank);
            }


            {       // places where commodities occur
                string commodities = JL(new[] { JournalTypeEnum.MarketBuy, JournalTypeEnum.MarketSell, JournalTypeEnum.MiningRefined });
                jc.AddPrePostfix("Type", ";", "", commodities, RMat);
                jc.AddPrePostfix("Count", ";", "", commodities);
            }

            {
                string materials = JL(new[] { JournalTypeEnum.MaterialCollected, JournalTypeEnum.MaterialDiscarded, JournalTypeEnum.MaterialDiscovered });
                jc.AddPrePostfix("Name", ";", "", materials, RMat);
                jc.AddPrePostfix("Category", ";", "", materials);
                jc.AddPrePostfix("Count", "; items", "", materials);
            }

            {
                string slots = JL(new[] { JournalTypeEnum.MassModuleStore, JournalTypeEnum.ModuleBuy, JournalTypeEnum.ModuleRetrieve, JournalTypeEnum.ModuleSell, JournalTypeEnum.ModuleStore, JournalTypeEnum.Loadout });
                jc.AddPrePostfix("Slot", "Slot ;", "", slots, GetBetterSlotName);
                jc.AddPrePostfix("FromSlot", "From ;", "", "ModuleSwap", GetBetterSlotName);
                jc.AddPrePostfix("ToSlot", "To ;", "", "ModuleSwap", GetBetterSlotName);
            }

            {
                // localised.. so localised will print, don't need converters
                // FetchRemoveModule StoreItem
                // ModuleBuy: BuyItem, SellItem
                // Retrieve: RetrievedItem
                // Repair: Item
                // Sell: SellItem
                // SellRemote: SellItem
                // Store: StoredItem, Replacement item (I'm guessing)
                // Swap: FromItem ToItem

                // Not normally shown due to _Localised, but keep for debugging purposes, turn off the _localised bit in the journal entry to see them printed
                // jc.AddPrePostfix("BuyItem", "BI:;", "", JL(new[] { JournalTypeEnum.ModuleBuy }), GetBetterItemNameEvents);
                // jc.AddPrePostfix("SellItem", "SI:;", "", JL(new[] { JournalTypeEnum.ModuleBuy, JournalTypeEnum.ModuleSell }), GetBetterItemNameEvents);
                // jc.AddPrePostfix("RetrievedItem", "RI:;", "", JL(new[] { JournalTypeEnum.ModuleRetrieve }), GetBetterItemNameEvents);

                // needed 
                jc.AddPrePostfix("Item", "Item ;", "", JL(new[] { JournalTypeEnum.Loadout }), GetBetterItemNameLoadout);
                jc.AddPrePostfix("Name", "Item ;", "", JL(JournalTypeEnum.MassModuleStore), GetBetterItemNameEvents);
            }


            return jc;
        }

        static string RMat(string s)            // replacer for pretty print
        {
            EDDiscovery2.DB.MaterialCommodity mc = EDDiscovery2.DB.MaterialCommodity.GetCachedMaterial(s);
            if (mc != null)
                s = mc.name;
            return s;
        }

        static string RLat(string value)      // replacer for pretty print
        {
            double lv;
            if (double.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out lv))        // if it does parse, we can convert it
            {
                long arcsec = (long)(lv * 60 * 60);          // convert to arc seconds
                string marker = (arcsec < 0) ? "S" : "N";       // presume lat
                arcsec = Math.Abs(arcsec);
                value = string.Format("{0}°{1} {2}'{3}\"", arcsec / 3600, marker, (arcsec / 60) % 60, arcsec % 60);
            }

            return value;
        }

        static string RLong(string value)      // replacer for pretty print
        {
            double lv;
            if (double.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out lv))        // if it does parse, we can convert it
            {
                long arcsec = (long)(lv * 60 * 60);          // convert to arc seconds
                string marker = (arcsec < 0) ? "W" : "E";       // presume lat
                arcsec = Math.Abs(arcsec);
                value = string.Format("{0}°{1} {2}'{3}\"", arcsec / 3600, marker, (arcsec / 60) % 60, arcsec % 60);
            }

            return value;
        }

        static string JL(JournalTypeEnum ar)
        {
            return ar.ToString();
        }

        static string JL(JournalTypeEnum[] ar)
        {
            string s = "";
            foreach (JournalTypeEnum a in ar)
                s += ((s.Length > 0) ? ";" : "") + a.ToString();

            return s;
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
