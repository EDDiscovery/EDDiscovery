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

                returnstring += "      static Dictionary<string, int> " + name + " = new Dictionary<string, int>" + Environment.NewLine +
                                     "        {" + Environment.NewLine;

                dictofdict += "            { \"" + fdshipname.ToLower() + "\"," + name + "}," + Environment.NewLine;

                int index = 0;
                string[] names = { "grade1", "grade2", "grade3", "mirrored", "reactive" };
                foreach (JObject j in bulk.Children())
                {
                    int edid = (int)j["edID"];

                    returnstring += "            { \"" + names[index++] + "\"," + edid + "}," + Environment.NewLine;

                    System.Diagnostics.Debug.WriteLine("P " + j.Path);
                }

                returnstring += "        };" + Environment.NewLine;
            }

            returnstring += "      static Dictionary<string, Dictionary<string,int>> ships = new Dictionary<string, Dictionary<string,int>>" + Environment.NewLine +
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
              new Tuple<string,string>("Viper", "viper"),
              new Tuple<string,string>("Viper_MkIV", "viper_mk_iv"),
              new Tuple<string,string>("Vulture", "vulture")
        };

        static public string ProcessModules(FileInfo[] allFiles)
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

                dictofdict += "            { \"" + mangledname + "\"," + structname + "}," + Environment.NewLine;

                returnstring += "      static Dictionary<string, int> " + structname + " = new Dictionary<string, int>" + Environment.NewLine +
                                     "        {" + Environment.NewLine;

                returnstring += ProcessPartModule(f.FullName);

                returnstring += "        };" + Environment.NewLine;
            }

            returnstring += "      static Dictionary<string, Dictionary<string,int>> modules = new Dictionary<string, Dictionary<string,int>>" + Environment.NewLine +
                                     "        {" + Environment.NewLine;
            returnstring += dictofdict;
            returnstring += "        };" + Environment.NewLine;

            return returnstring;
        }

        static string ProcessPartModule(string file)
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
                        other += "+" + (string)j["name"];

                    returnstring += "            { \"" + c.ToString() + rating + other + "\"," + edid + "}," + Environment.NewLine;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(pname + c + rating + "MISSING EDID");
                }
            }

            return returnstring;
        }


        // unused code

        static private string expand(JToken jt, int level, string title)
        {
            string returnstring = "";

            if (jt.HasValues)
            {
                string name = jt.Path;

                int totalchildren = jt is JContainer ? ((JContainer)jt).Count : 0;

                bool isarray = jt is JArray;
                bool isobject = jt is JObject;

                JTokenType[] decodeable = { JTokenType.Boolean, JTokenType.Date, JTokenType.Integer, JTokenType.String, JTokenType.Float, JTokenType.TimeSpan };

                foreach (JToken jc in jt.Children())
                {
                    System.Diagnostics.Trace.WriteLine(string.Format(" >> Child {0} : {1} {2}", 1, jc.Path, jc.Type.ToString()));
                    if (jc.HasValues)
                    {
                        returnstring += expand(jc, level + 1, title);
                    }
                    else if (Array.FindIndex(decodeable, x => x == jc.Type) != -1)
                    {
                        string path = jc.Path;
                        string value = jc.Value<string>();

                        if (path.Contains("class"))
                            returnstring += title + "," + value;
                        else if (path.Contains("edID"))
                            returnstring += "," + value;
                        else if (path.Contains("rating"))
                            returnstring += ",\"" + value + "\"" + Environment.NewLine;
                    }
                }
            }

            return returnstring;
        }






    }
}
