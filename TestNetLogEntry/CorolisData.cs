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
                            "          public int moduleid;" + Environment.NewLine +
                            "          public int mass;" + Environment.NewLine +
                            "          public ShipModule(int id, int m) {moduleid=id;mass=m;}" + Environment.NewLine +
                            "      };" + Environment.NewLine;

            returnstring += "      public class ShipInfoString : ShipInfo  {" + Environment.NewLine +
                            "          public string str;" + Environment.NewLine +
                            "          public ShipInfoString(string s) {str=s;}" + Environment.NewLine +
                            "      };" + Environment.NewLine;

            returnstring += "      public class ShipInfoInt : ShipInfo  {" + Environment.NewLine +
                            "          public int value;" + Environment.NewLine +
                            "          public ShipInfoInt(int i) {value=i;}" + Environment.NewLine +
                            "      };" + Environment.NewLine;

            returnstring += "      public class ShipInfoDouble : ShipInfo {" + Environment.NewLine +
                            "          public double value;" + Environment.NewLine +
                            "          public ShipInfoDouble(double d) {value=d;}" + Environment.NewLine +
                            "      };" + Environment.NewLine;

            foreach (FileInfo f in allFiles)
            {
                string json = File.ReadAllText(f.FullName, Encoding.UTF8);

                JObject jo = new JObject();
                jo = JObject.Parse(json);

                JToken first = jo.First;        // ship name
                string name = first.Path;
                string fdshipname = "??";

                foreach (Tuple<string, string> t in SHIP_FD_NAME_TO_CORIOLIS_NAME)
                {
                    if (name == t.Item2)
                    {
                        fdshipname = t.Item1;
                        break;
                    }
                }

                JObject second = (JObject)first.First;

                JToken bulk = second["bulkheads"];


                returnstring += "      static Dictionary<string, ShipInfo> " + name + " = new Dictionary<string, ShipInfo>" + Environment.NewLine +
                                     "        {" + Environment.NewLine;

                dictofdict += "            { \"" + fdshipname + "\"," + name + "}," + Environment.NewLine;

                int index = 0;
                string[] names = { "grade1", "grade2", "grade3", "mirrored", "reactive" };
                foreach (JObject j in bulk.Children())
                {
                    int edid = (int)j["edID"];
                    double mass = (double)j["mass"];

                    returnstring += "            { \"" + names[index++] + "\", new ShipModule(" + edid + "," + mass + ")}," + Environment.NewLine;

                    System.Diagnostics.Debug.WriteLine("P " + j.Path);
                }

                JToken prop = second["properties"];
                returnstring += "            { \"HullMass\", new ShipInfoDouble(" +((double)prop["hullMass"]) + ")}," + Environment.NewLine;
                returnstring += "            { \"Manu\", new ShipInfoString(\"" + ((string)prop["manufacturer"]) + "\")}," + Environment.NewLine;
                returnstring += "            { \"Speed\", new ShipInfoInt(" + ((string)prop["speed"]) + ")}," + Environment.NewLine;
                returnstring += "            { \"Boost\", new ShipInfoInt(" + ((string)prop["boost"]) + ")}," + Environment.NewLine;
                returnstring += "            { \"HullCost\", new ShipInfoInt(" + ((string)prop["hullCost"]) + ")}," + Environment.NewLine;
                returnstring += "            { \"Class\", new ShipInfoInt(" + ((string)prop["class"]) + ")}," + Environment.NewLine;

                returnstring += "        };" + Environment.NewLine;
            }

            returnstring += "      static Dictionary<string, Dictionary<string,ShipInfo>> ships = new Dictionary<string, Dictionary<string,ShipInfo>>" + Environment.NewLine +
                                     "        {" + Environment.NewLine;
            returnstring += dictofdict;
            returnstring += "        };" + Environment.NewLine;

            return returnstring;
        }

        static List<Tuple<string, string>> SHIP_FD_NAME_TO_CORIOLIS_NAME = new List<Tuple<string, string>>()
        {
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
              new Tuple<string,string>("Type_9_military","type_10_defender"),
              new Tuple<string,string>("TypeX","alliance_chieftain"),
              new Tuple<string,string>("Viper", "viper"),
              new Tuple<string,string>("Viper_MkIV", "viper_mk_iv"),
              new Tuple<string,string>("Vulture", "vulture")
        };


        // OUT OF DATE KEEP FOR REF

        static public string ProcessModulesOld(FileInfo[] allFiles)
        {
            string dictofdict = "";

            string returnstring = "";

            foreach (FileInfo f in allFiles)
            {
                string structname = f.Name.Substring(0, f.Name.IndexOf('.'));

                string mangledname = structname.Replace("_", "");
                if (mangledname == "burstlaser")
                    mangledname = "pulselaserburst";
                if (mangledname == "pointdefence")
                    mangledname = "plasmapointdefence";
                if (mangledname == "thrusters")
                    mangledname = "engine";
                if (mangledname == "frameshiftdrive")
                    mangledname = "hyperdrive";
                if (mangledname == "planetaryvehiclehanger")
                    mangledname = "buggybay";
                if (mangledname == "hullreinforcementpackage")
                    mangledname = "hullreinforcement";
                if (mangledname == "planetaryapproachsuite")
                    mangledname = "planetapproachsuite";
                if (mangledname == "fighterhanger")
                    mangledname = "fighterbay";
                if (mangledname == "fragmentcannon")
                    mangledname = "slugshot";

                dictofdict += "            { \"" + mangledname + "\", module_" + structname + "}," + Environment.NewLine;

                returnstring += "      static Dictionary<string, ShipInfo> module_" + structname + " = new Dictionary<string, ShipInfo>" + Environment.NewLine +
                                     "        {" + Environment.NewLine;

                returnstring += ProcessPartModuleOld(f.FullName);

                returnstring += "        };" + Environment.NewLine;
            }

            returnstring += "      static Dictionary<string, Dictionary<string,ShipInfo>> modules = new Dictionary<string, Dictionary<string,ShipInfo>>" + Environment.NewLine +
                                     "        {" + Environment.NewLine;
            returnstring += dictofdict;
            returnstring += "        };" + Environment.NewLine;

            return returnstring;
        }

        static string ProcessPartModuleOld(string file)
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
                int c = (int)j["class"];
                string rating = (string)j["rating"];

                if (j["edID"] != null)
                {
                    int edid = (int)j["edID"];

                    string other = "";
                    if (j["mount"] != null)
                    {
                        other += "-" + (string)j["mount"];
                        rating = "";
                    }

                    if (j["missile"] != null)
                        other += ">" + (string)j["missile"];

                    if (j["name"] != null)
                    {
                        string name = (string)j["name"];
                        if (name == "Retributor")
                            name = "-H";
                        else if ( name == "Cyoscrambler")
                            name = "-S";
                        else
                            name = "+" + name;
                        other += name;
                    }

                    int mass = j["mass"] != null ? (int)j["mass"] : 0;

                    string part = "            { \"" + c.ToString() + rating + other + "\", new ShipModule(" + edid + "," + mass + ")}," + Environment.NewLine;

                    returnstring += part;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(pname + c + rating + "MISSING EDID");
                }
            }

            return returnstring;
        }


        static public string ProcessModules(FileInfo[] allFiles)
        {
            string returnstring = "";
            returnstring += "      static Dictionary<string, ShipInfo> modules = new Dictionary<string, ShipInfo>" + Environment.NewLine +
                                     "        {" + Environment.NewLine;

            List<string> names = new List<string>();

            foreach (FileInfo f in allFiles)
            {
                returnstring += ProcessModulesInfo(f.FullName,names);
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
                string sym = (string)j["symbol"];
                int edid = (int)j["edID"];
                double mass = j["mass"] != null ? (double)j["mass"] : 0;

                if ( sym == null )      // missing from corolis data
                {
                    if (edid == 128671347)
                        sym = "Hpt_MiningLaser_Fixed_Small_Advanced";     // best guess
                    if (edid == 128671345)
                        sym = "Hpt_MultiCannon_Fixed_Small_Scatter";
                    if (edid == 128671342)
                        sym = "Hpt_PulseLaser_Fixed_Medium_Distruptor"; 
                }

                sym = sym.ToLower();
                if (names.Contains(sym))
                    returnstring += "**** is repeated";

                names.Add(sym);
                string part = "            { \"" + sym + "\", new ShipModule(" + edid + "," + mass + ")}," + Environment.NewLine;
                returnstring += part;
            }

            return returnstring;
        }


    }
}
