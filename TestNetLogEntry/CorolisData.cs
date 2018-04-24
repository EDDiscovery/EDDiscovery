using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLogEntry
{
    static public class CorolisData
    {
        static public string ProcessShips(FileInfo[] allFiles)            // overall index of items
        {
            string dictofdict = "";

            string returnstring = "";

            returnstring += "      public interface ShipInfo {" + Environment.NewLine +
                            "      };" + Environment.NewLine;

            returnstring += "      public class ShipModule : ShipInfo {" + Environment.NewLine +
                            "          public int ModuleID;" + Environment.NewLine +
                            "          public double Mass;" + Environment.NewLine +
                            "          public string ModName;" + Environment.NewLine +
                            "          public string ModType;" + Environment.NewLine +
                            "          public double Power;" + Environment.NewLine +
                            "          public string Info;" + Environment.NewLine +
                            "          public ShipModule(int id, double m, string n , string t) {ModuleID=id;Mass=m;ModName=n;ModType=t;}" + Environment.NewLine +
                            "          public ShipModule(int id, double m, double p, string i, string n , string t) {ModuleID=id;Mass=m;Power=p;Info=i,ModName=n;ModType=t;}" + Environment.NewLine +
                            "      };" + Environment.NewLine;

            returnstring += "      public class ShipInfoString : ShipInfo  {" + Environment.NewLine +
                            "          public string Value;" + Environment.NewLine +
                            "          public ShipInfoString(string s) {Value=s;}" + Environment.NewLine +
                            "      };" + Environment.NewLine;

            returnstring += "      public class ShipInfoInt : ShipInfo  {" + Environment.NewLine +
                            "          public int Value;" + Environment.NewLine +
                            "          public ShipInfoInt(int i) {Value=i;}" + Environment.NewLine +
                            "      };" + Environment.NewLine;

            returnstring += "      public class ShipInfoDouble : ShipInfo {" + Environment.NewLine +
                            "          public double Value;" + Environment.NewLine +
                            "          public ShipInfoDouble(double d) {Value=d;}" + Environment.NewLine +
                            "      };" + Environment.NewLine;

            returnstring += "      enum ShipPropID { FDID, HullMass, Name, Manu, Speed, Boost, HullCost, Class };" + Environment.NewLine;

            returnstring += Environment.NewLine;

            string modulestring = "";

            foreach (FileInfo f in allFiles)
            {
                string json = File.ReadAllText(f.FullName, Encoding.UTF8);

                JObject jo = new JObject();
                jo = JObject.Parse(json);

                JToken first = jo.First;        // ship name
                string name = first.Path;
                string fdshipname = null;

                foreach (Tuple<string, string> t in SHIP_FD_NAME_TO_CORIOLIS_NAME)
                {
                    if (name == t.Item2)
                    {
                        fdshipname = t.Item1;
                        break;
                    }
                }

                if ( fdshipname == null)
                {
                    Console.WriteLine("Unknown ship " + name);
                    return "";
                }

                JObject second = (JObject)first.First;

                JToken bulk = second["bulkheads"];


                returnstring += "      static Dictionary<ShipPropID, ShipInfo> " + name + " = new Dictionary<ShipPropID, ShipInfo>" + Environment.NewLine +
                                     "        {" + Environment.NewLine;

                dictofdict += "            { \"" + fdshipname.ToLower() + "\"," + name + "}," + Environment.NewLine;

                JToken prop = second["properties"];

                int index = 0;
                string[] names = { "Grade1", "Grade2", "Grade3", "Mirrored", "Reactive" };
                string[] fullnames = { "Lightweight", "Reinforced", "Military", "Mirrored Surface Composite", "Reactive Surface Composite" };
                foreach (JObject j in bulk.Children())
                {
                    int edid = (int)j["edID"];
                    double mass = (double)j["mass"];

                    string infostr = "";
                    infostr = infostr.AppendPrePad("Explosive:" + ((double)j["explres"]).ToString("0.#%"), ", ");
                    infostr = infostr.AppendPrePad("Kinetic:" + ((double)j["kinres"]).ToString("0.#%"), ", ");
                    infostr = infostr.AppendPrePad("Thermal:" + ((double)j["thermres"]).ToString("0.#%"), ", ");

                    string fdname = fdshipname + "_Armour_" + names[index];
                    string nicename = (string)prop["name"] + " " + fullnames[index] + " Armour";

                    modulestring += "            { \"" + fdname.ToLower() + "\", new ShipModule(" + edid + "," + mass + ",0,\"" + infostr + "\",\""+ nicename + "\",\"Armour\")}," + Environment.NewLine;
                    index++;
                }

                returnstring += "            { ShipPropID.FDID, new ShipInfoString(\"" + fdshipname + "\")}," + Environment.NewLine;
                returnstring += "            { ShipPropID.HullMass, new ShipInfoDouble(" + ((double)prop["hullMass"]) + "F)}," + Environment.NewLine;
                returnstring += "            { ShipPropID.Name, new ShipInfoString(\"" + ((string)prop["name"]) + "\")}," + Environment.NewLine;
                returnstring += "            { ShipPropID.Manu, new ShipInfoString(\"" + ((string)prop["manufacturer"]) + "\")}," + Environment.NewLine;
                returnstring += "            { ShipPropID.Speed, new ShipInfoInt(" + ((string)prop["speed"]) + ")}," + Environment.NewLine;
                returnstring += "            { ShipPropID.Boost, new ShipInfoInt(" + ((string)prop["boost"]) + ")}," + Environment.NewLine;
                returnstring += "            { ShipPropID.HullCost, new ShipInfoInt(" + ((string)prop["hullCost"]) + ")}," + Environment.NewLine;
                returnstring += "            { ShipPropID.Class, new ShipInfoInt(" + ((string)prop["class"]) + ")}," + Environment.NewLine;

                returnstring += "        };" + Environment.NewLine;
            }

            returnstring += "      static Dictionary<string, Dictionary<ShipPropID,ShipInfo>> ships = new Dictionary<string, Dictionary<ShipPropID,ShipInfo>>" + Environment.NewLine +
                                     "        {" + Environment.NewLine;
            returnstring += dictofdict;
            returnstring += "        };" + Environment.NewLine;
            returnstring += Environment.NewLine;
            returnstring += modulestring;

            return returnstring;
        }

        static List<Tuple<string, string>> SHIP_FD_NAME_TO_CORIOLIS_NAME = new List<Tuple<string, string>>()
        {
            // column 2 is the root name node in corolis
            // column 1 is the upper case name of the FD id

              new Tuple<string,string>("Adder", "adder"),
              new Tuple<string,string>("Anaconda", "anaconda"),
              new Tuple<string,string>("Asp", "asp"),
              new Tuple<string,string>("Asp_Scout", "asp_scout"),
              new Tuple<string,string>("BelugaLiner", "beluga"),
              new Tuple<string,string>("CobraMkIII", "cobra_mk_iii"),
              new Tuple<string,string>("CobraMkIV", "cobra_mk_iv"),
              new Tuple<string,string>("Cutter", "imperial_cutter"),
              new Tuple<string,string>("DiamondBackXL", "diamondback_explorer"),
              new Tuple<string,string>("DiamondBack", "diamondback"),
              new Tuple<string,string>("Dolphin", "dolphin"),
              new Tuple<string,string>("Eagle", "eagle"),
              new Tuple<string,string>("Empire_Courier", "imperial_courier"),
              new Tuple<string,string>("Empire_Eagle", "imperial_eagle"),
              new Tuple<string,string>("Empire_Trader", "imperial_clipper"),
              new Tuple<string,string>("Federation_Corvette", "federal_corvette"),
              new Tuple<string,string>("Federation_Dropship", "federal_dropship"),
              new Tuple<string,string>("Federation_Dropship_MkII", "federal_assault_ship"),
              new Tuple<string,string>("Federation_Gunship", "federal_gunship"),
              new Tuple<string,string>("FerDeLance", "fer_de_lance"),
              new Tuple<string,string>("Hauler", "hauler"),
              new Tuple<string,string>("Independant_Trader", "keelback"),
              new Tuple<string,string>("Orca", "orca"),
              new Tuple<string,string>("Python", "python"),
              new Tuple<string,string>("SideWinder", "sidewinder"),
              new Tuple<string,string>("Type6", "type_6_transporter"),
              new Tuple<string,string>("Type7", "type_7_transport"),
              new Tuple<string,string>("Type9", "type_9_heavy"),
              new Tuple<string,string>("Type9_Military","type_10_defender"),
              new Tuple<string,string>("TypeX","alliance_chieftain"),
              new Tuple<string,string>("Viper", "viper"),
              new Tuple<string,string>("Viper_MkIV", "viper_mk_iv"),
              new Tuple<string,string>("Vulture", "vulture")
        };


        // New stuff April 2018

        static public string ProcessModules(FileInfo[] allFiles)
        {
            string returnstring = "";
            returnstring += "      static Dictionary<string, ShipModule> modules = new Dictionary<string, ShipModule>" + Environment.NewLine +
                                     "        {" + Environment.NewLine;

            List<string> names = new List<string>();

            foreach (FileInfo f in allFiles)
            {
                returnstring += ProcessModulesInfo(f.FullName, names);
            }

            returnstring += "        };" + Environment.NewLine;

            return returnstring;
        }

        static string ProcessModulesInfo(string file, List<string> names)
        {
            string json = File.ReadAllText(file, Encoding.UTF8);

            JObject jo = new JObject();
            jo = JObject.Parse(json);

            JToken first = jo.First;
            JToken first1 = first.First;
            JArray ja = (JArray)first1;
            string pname = ja.Path;

            string returnstring = "";

            foreach (JObject j in ja.Children())
            {
                string betternameprefix = "";
                string sym = (string)j["symbol"];
                int edid = (int)j["edID"];
                double mass = j["mass"] != null ? (double)j["mass"] : 0;
                string massstr = mass + (mass != Math.Floor(mass) ? "F" : "");

                double power = j["power"] != null ? (double)j["power"] : 0;
                string powerstr = power + (power != Math.Floor(power) ? "F" : "");

                string infostr = "";
                if (j["ammo"] != null)
                {
                    infostr = infostr.AppendPrePad( "Ammo:" + ((int)j["ammo"]).ToString(), ", ");
                    if (j["clip"] != null)
                        infostr += "/" + ((int)j["clip"]).ToString();
                }
                else if (j["rof"] != null)
                {
                    infostr = infostr.AppendPrePad("ROF:" + ((double)j["rof"]).ToString("0.#")+"/s", ", ");
                }

                if (j["damage"] != null)
                    infostr = infostr.AppendPrePad("Damage:" + ((double)j["damage"]).ToString("0.#"), ", ");
                if (j["fuel"] != null)
                    infostr = infostr.AppendPrePad("Size:" + ((int)j["fuel"]).ToString() + "t", ", ");
                if (j["optmass"] != null)
                    infostr = infostr.AppendPrePad("OptMass:" + ((int)j["optmass"]).ToString() + "t", ", ");
                if (j["maxmass"] != null)
                    infostr = infostr.AppendPrePad("MaxMass:" + ((int)j["maxmass"]).ToString() + "t", ", ");
                if (j["minmass"] != null)
                    infostr = infostr.AppendPrePad("MinMass:" + ((int)j["minmass"]).ToString() + "t", ", ");
                if (j["time"] != null)
                    infostr = infostr.AppendPrePad("Time:" + ((int)j["time"]).ToString() + "s", ", ");
                if (j["sysrate"] != null)
                    infostr = infostr.AppendPrePad("Sys:" + ((double)j["sysrate"]).ToString("0.#MW") + ", Eng:" + ((double)j["engrate"]).ToString("0.#MW") + ", Wep:" + ((double)j["weprate"]).ToString("0.#MW"), ", ");
                if (j["pgen"] != null)
                    infostr = infostr.AppendPrePad("Power:" + ((double)j["pgen"]).ToString("0.#MW"), ", ");
                if (j["range"] != null)
                {
                    double rng = ((double)j["range"]);
                    if ( rng > 100 )
                        infostr = infostr.AppendPrePad("Range:" + rng.ToString("0.#m"), ", ");
                    else
                        infostr = infostr.AppendPrePad("Range:" + rng.ToString("0.#km"), ", ");
                }
                if (j["shotspeed"] != null)
                    infostr = infostr.AppendPrePad("Speed:" + ((int)j["shotspeed"]).ToString() + "m/s", ", ");
                if (j["cargo"] != null)
                    infostr = infostr.AppendPrePad("Size:" + ((int)j["cargo"]).ToString() + "t", ", ");
                if (j["rebuildsperbay"] != null)
                    infostr = infostr.AppendPrePad("Rebuilds:" + ((int)j["rebuildsperbay"]).ToString() + "t", ", ");
                if (j["reload"] != null)
                    infostr = infostr.AppendPrePad("Reload:" + ((double)j["reload"]).ToString("0.#s"), ", ");
                if (j["shieldboost"] != null)
                    infostr = infostr.AppendPrePad("Boost:" + (((double)j["shieldboost"])).ToString("0.0%"), ", ");
                if (j["thermload"] != null)
                    infostr = infostr.AppendPrePad("ThermL:" + ((double)j["thermload"]).ToString("0.#"), ", ");
                if (j["repair"] != null)
                    infostr = infostr.AppendPrePad("Repair:" + ((double)j["repair"]).ToString("0.#"), ", ");
                if (j["rate"] != null)
                    infostr = infostr.AppendPrePad("Rate:" + ((double)j["rate"]).ToString("0.#"), ", ");

                if (j["explres"] != null)
                    infostr = infostr.AppendPrePad("Explosive:" + ((double)j["explres"]).ToString("0.##%"), ", ");
                if (j["kinres"] != null)
                    infostr = infostr.AppendPrePad("Kinetic:" + ((double)j["kinres"]).ToString("0.##%"), ", ");
                if (j["thermres"] != null)
                    infostr = infostr.AppendPrePad("Thermal:" + ((double)j["thermres"]).ToString("0.##%"), ", ");
                if (j["protection"] != null)
                    infostr = infostr.AppendPrePad("Protection:" + ((double)j["protection"]).ToString("0.##"), ", ");
                if (j["rangeLS"] != null && j["rangeLS"].ToString().Length>0)
                {

                     infostr = infostr.AppendPrePad("Range:" + ((double)j["rangeLS"]).ToString("0.#ls"), ", ");

                }


                if (j["passengers"] != null)
                {
                    infostr = infostr.AppendPrePad("Passengers:" + ((int)j["passengers"]).ToString(), ", ");
                    if (j["grp"] != null)
                    {
                        string grp = (string)j["grp"];
                        if (grp == "pcq")
                            grp = "Luxury";
                        if (grp == "pci")
                            grp = "Business Class";
                        if (grp == "pce")
                            grp = "Economy";
                        if (grp == "pcm")
                            grp = "First Class";

                        betternameprefix = grp + " ";
                    }
                }

                infostr = infostr.Length > 0 ? "\"" + infostr + "\"" : "null";

                if (sym == null)      // missing from corolis data
                {
                    if (edid == 128671347)
                        sym = "Hpt_MiningLaser_Fixed_Small_Advanced";     // best guess
                    if (edid == 128671345)
                        sym = "Hpt_MultiCannon_Fixed_Small_Scatter";
                    if (edid == 128671342)
                        sym = "Hpt_PulseLaser_Fixed_Medium_Distruptor";
                }

                string bettername = GetBetterItemNameEvents(sym);
                bettername = betternameprefix + bettername.Replace("  ", " ").Trim();


                int first_ = sym.IndexOf("_");
                int second_ = first_ >= 0 ? sym.IndexOf("_", first_ + 1) : -1;

                string modtypename = second_ >= 0 ? sym.Substring(0, second_) : sym;

                if (modtypename == "Int_DroneControl")
                {
                    int third_ = sym.IndexOf("_", second_ + 1);
                    modtypename = sym.Substring(0, third_);
                }

                modtypename = modtypename.SplitCapsWord().Replace("Hpt_", "").Replace("Int_", "").Replace("_", " ");

               // Console.WriteLine("Compare " + modtypename);
                Tuple<string, string> modtype = MOD_PREFIX_TO_TYPE.Find(x => modtypename.IndexOf(x.Item1, StringComparison.InvariantCultureIgnoreCase) >= 0);
                modtypename = modtype?.Item2 ?? modtypename;

                sym = sym.ToLower();
                if (names.Contains(sym))
                    returnstring += "**** is repeated";

                bool massisnonint = Math.Floor(mass) != mass;
                bool powerisnonint = Math.Floor(power) != power;

                names.Add(sym);
                string part = "            { \"" + sym + "\", new ShipModule(" + edid + ", " + massstr + ", " + powerstr + ", " +
                                                                         infostr + "," +
                                                                        "\"" + bettername + "\", \"" + modtypename + "\")}," + Environment.NewLine;
                returnstring += part;
            }

            return returnstring;
        }


        static Dictionary<string, string> replaceitems = new Dictionary<string, string>
        {
            // see the program folder win64\items\shipmodule

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

            {"Size0" , "" },
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
            {"Anti", "Shutdown" },
            {"Unknown", "Field" },
            {"Shutdown", "Neutraliser" },
            {"Tiny", "" },
            {"AT", "AX" },
            {"Control", "Controller" },
            {"Resource", "Hatch" },
            {"Siphon", "Breaker" },
        };

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

            s = s.Replace("Pulse Laser Burst", "Burst Laser");
            if (s.Contains("Planetary Vehicle Hangar"))                 // strange class naming, fix up after above.. don't want two tables
                s = s.Replace("Rating E", "Rating H").Replace("Rating D", "Rating G");
            if ( s.Contains("Drone Controller"))
            {
                int endnextword = s.IndexOf(' ', 17);
                s = s.Substring(17,endnextword-17) + " " + s.Substring(0, 16) + s.Substring(endnextword);
                s = s.Replace("Hatch Drone Controller Breaker", "Hatch Breaker Drone Controller ");
                s = s.Replace("Fuel Drone Controller Transfer", "Fuel Transfer Drone Controller ");
            }

            //System.Diagnostics.Debug.WriteLine("PP Item " + x + " >> " + s);

            return s;
        }


        static List<Tuple<string, string>> MOD_PREFIX_TO_TYPE = new List<Tuple<string, string>>()
            {
                  new Tuple<string,string>("Pulse Laser Burst", "Burst Laser"),
                  new Tuple<string,string>("Plasma Point Defence", "Point Defence"),
                  new Tuple<string,string>("AT Dumbfire Missile", "Missile Rack"),
                  new Tuple<string,string>("AT Multi Cannon", "Multi Cannon"),
                  new Tuple<string,string>("Repairer", "Auto Field Maintenance"),
                  new Tuple<string,string>("Drone Control Repair", "Limpet Controller"),
                  new Tuple<string,string>("Drone Control Prospector", "Limpet Controller"),
                  new Tuple<string,string>("Drone Control Resource Siphon", "Limpet Controller"),
                  new Tuple<string,string>("Drone Control Fuel Transfer", "Limpet Controller"),
                  new Tuple<string,string>("Drone Control Collection", "Limpet Controller"),
                  new Tuple<string,string>("Drone Control Repair", "Repair Limpet Controller"),
                  new Tuple<string,string>("Buggy Bay", "Planetary Vehicle Hangar"),
                  new Tuple<string,string>("Engine", "Thrusters"),
                  new Tuple<string,string>("Anti Unknown Shutdown", "Shutdown Field Neutraliser"),
                  new Tuple<string,string>("Cloud Scanner", "Hyperspace Cloud Scanner"),
                  new Tuple<string,string>("Drunk Missile Rack", "Missile Rack"),
                  new Tuple<string,string>("Dumbfire Missile Rack", "Missile Rack"),
                  new Tuple<string,string>("Slugshot", "Fragment Cannon"),
                  new Tuple<string,string>("Basic Missile Rack", "Missile Rack"),
                  new Tuple<string,string>("Pack Hound Missile Rack", "Missile Rack"),
                  new Tuple<string,string>("Corrosion Proof Cargo Rack", "Cargo Rack"),
                  new Tuple<string,string>("Advanced Torp Pylon" ,"Missile Rack"),
            };
    }
}
