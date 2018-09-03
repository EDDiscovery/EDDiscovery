using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLogEntry
{
    static public class CorolisShips
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

                dictofdict += "            { \"" + fdshipname.ToLowerInvariant() + "\"," + name + "}," + Environment.NewLine;

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

                    modulestring += "            { \"" + fdname.ToLowerInvariant() + "\", new ShipModule(" + edid + "," + mass + ",0,\"" + infostr + "\",\""+ nicename + "\",\"Armour\")}," + Environment.NewLine;
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
              new Tuple<string,string>("Krait_MkII", "krait_mkii"),
              new Tuple<string,string>("Independant_Trader", "keelback"),
              new Tuple<string,string>("Orca", "orca"),
              new Tuple<string,string>("Python", "python"),
              new Tuple<string,string>("SideWinder", "sidewinder"),
              new Tuple<string,string>("TypeX_2", "alliance_crusader"),     // FDNAME -> name in json
              new Tuple<string,string>("TypeX_3", "alliance_challenger"),
              new Tuple<string,string>("Type6", "type_6_transporter"),
              new Tuple<string,string>("Type7", "type_7_transport"),
              new Tuple<string,string>("Type9", "type_9_heavy"),
              new Tuple<string,string>("Type9_Military","type_10_defender"),
              new Tuple<string,string>("TypeX","alliance_chieftain"),
              new Tuple<string,string>("Viper", "viper"),
              new Tuple<string,string>("Viper_MkIV", "viper_mk_iv"),
              new Tuple<string,string>("Vulture", "vulture")
        };

    }
}
