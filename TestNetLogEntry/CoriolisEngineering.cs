using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// look into modules.json and blueprints.json for data

namespace NetLogEntry
{
    static public class CorolisEngineering
    {
        public class EngEntry
        {
            public string corolisclass;
            public string corolisclasstext;
            public string corolisfrontierid;
            public string fdname;
            public string grade;
            public string englist;
        }

        public class NameConvert
        {
            public string c; public string cnv; public string frontier;
            public NameConvert(string a, string b, string f = "") { c = a; cnv = b; frontier = f.HasChars() ? f : b.Replace(" ", ""); }
        }

        public static NameConvert[] conversions = new NameConvert[]
        {
            new NameConvert("am", "Auto Field-Maintenance Unit","AFM"),       // from corolis.  Third column is to match it against frontier data fdname first part
            new NameConvert("bh", "Bulkheads", "Armour"),
            new NameConvert("bl", "Beam Laser"),
            new NameConvert("bsg", "Bi-Weave Shield Generator"),
            new NameConvert("c", "Cannon"),
            new NameConvert("cc", "Collector Limpet Controller", "CollectionLimpet"),
            new NameConvert("ch", "Chaff Launcher"),
            new NameConvert("cr", "Cargo Rack"),
            new NameConvert("cs", "Manifest Scanner", "CargoScanner"),
            new NameConvert("dc", "Docking Computer"),
            new NameConvert("ec", "Electronic Countermeasure","ECM"),
            new NameConvert("fc", "Fragment Cannon","FragCannon"),
            new NameConvert("fh", "Fighter Hangar"),
            new NameConvert("fi", "FSD Interdictor","FSDinterdictor"),
            new NameConvert("fs", "Fuel Scoop"),
            new NameConvert("fsd", "Frame Shift Drive","FSD"),
            new NameConvert("ft", "Fuel Tank"),
            new NameConvert("fx", "Fuel Transfer Limpet Controller","FuelTransferLimpet"),
            new NameConvert("hb", "Hatch Breaker Limpet Controller","HatchBreakerLimpet"),
            new NameConvert("hr", "Hull Reinforcement Package", "HullReinforcement"),
            new NameConvert("hs", "Heat Sink Launcher"),
            new NameConvert("kw", "Kill Warrant Scanner"),
            new NameConvert("ls", "Life Support"),
            new NameConvert("mc", "Multi cannon"),
            new NameConvert("axmc", "AX Multi-cannon"),
            new NameConvert("ml", "Mining Laser"),
            new NameConvert("mr", "Missile Rack","Missile"),
            new NameConvert("axmr", "AX Missile Rack"),
            new NameConvert("mrp", "Module Reinforcement Package"),
            new NameConvert("nl", "Mine Launcher","Mine"),
            new NameConvert("pa", "Plasma Accelerator"),
            new NameConvert("pas", "Planetary Approach Suite"),
            new NameConvert("pc", "Prospector Limpet Controller","ProspectingLimpet"),
            new NameConvert("pce", "Economy Class Passenger Cabin"),
            new NameConvert("pci", "Business Class Passenger Cabin"),
            new NameConvert("pcm", "First Class Passenger Cabin"),
            new NameConvert("pcq", "Luxury Passenger Cabin"),
            new NameConvert("pd", "Power Distributor"),
            new NameConvert("pl", "Pulse Laser"),
            new NameConvert("po", "Point Defence"),
            new NameConvert("pp", "Power Plant"),
            new NameConvert("psg", "Prismatic Shield Generator"),
            new NameConvert("pv", "Planetary Vehicle Hangar"),
            new NameConvert("rf", "Refinery", "Refineries"),
            new NameConvert("rfl", "Remote Release Flak Launcher"),
            new NameConvert("rg", "Rail Gun"),
            new NameConvert("s", "Sensor"),
            new NameConvert("sb", "Shield Booster"),
            new NameConvert("sc", "Stellar Scanners"),
            new NameConvert("scb", "Shield Cell Bank"),
            new NameConvert("sfn", "Shutdown Field Neutraliser"),
            new NameConvert("sg", "Shield Generator"),
            new NameConvert("ss", "Surface Scanner"),
            new NameConvert("t", "thrusters","Engine"),
            new NameConvert("tp", "Torpedo Pylon", "Torpedo"),
            new NameConvert("ul", "Burst Laser"),
            new NameConvert("ws", "Frame Shift Wake Scanner", "WakeScanner"),
            new NameConvert("rpl", "Repair Limpet Controller"),
            new NameConvert("xs", "Xeno Scanner"),

        };

        static public List<EngEntry> GetEng(string rootpath)
        {
            List<EngEntry> list = new List<EngEntry>();

            try
            {
                string modulespath = Path.Combine(rootpath, "modules.json");
                string moduletext = File.ReadAllText(modulespath);

                JObject modulesjo = JObject.Parse(moduletext);

                foreach (JProperty m in modulesjo.Children())
                {
                    string clsname = m.Name;

                    JObject jm = (JObject)m.First;

                    JObject blueprints = (JObject)jm["blueprints"];

                    foreach (JProperty bc in blueprints.Children())
                    {
                        string bpname = bc.Name;

                        JObject jp = (JObject)bc.First;
                        JObject grades = (JObject)jp["grades"];

                        foreach (JProperty gr in grades.Children())
                        {
                            string grname = gr.Name;

                            JObject gradeinfo = (JObject)gr.Value;

                            JArray engs = (JArray)gradeinfo["engineers"];

                            string englist = "";
                            foreach (string s in engs)
                            {
                                englist = englist.AppendPrePad(s, ",");
                            }

                            NameConvert namec = Array.Find(conversions, x => x.c == clsname);
                            list.Add(new EngEntry() { corolisclass = clsname, corolisclasstext = namec.cnv, corolisfrontierid = namec.frontier,
                                fdname = bpname, grade = grname, englist = englist });

                          //  Console.WriteLine("Eng " + clsname + " : " + namec.frontier + ":" + bpname + " : " + grname + " : " + englist);

                        }

                    }
                }
            }
            catch
            {

            }

            return list;
        }

        static public string ProcessEngineering(string rootpath)            // overall index of items
        {
            string ret = "";

            EliteDangerousCore.MaterialCommodityData.SetUpInitialTable();

            List<EngEntry> list = GetEng(rootpath);
            
            try
            {
                string blueprintspath = Path.Combine(rootpath, "blueprints.json");
                string blueprinttext = File.ReadAllText(blueprintspath);

                JObject blueprintsjo = JObject.Parse(blueprinttext);

                foreach (JProperty c in blueprintsjo.Children())
                {
                    JObject jo = (JObject)c.First;
                    string fdname = jo["fdname"].Str();

                    //Console.WriteLine("Item " + c.Path + " " + fdname);

                    JObject grades = (JObject)jo["grades"];

                    foreach (JProperty gr in grades.Children())
                    {
                        string grname = gr.Name;
                        //                        Console.WriteLine("  Grade " + gr.Name);

                        JObject gradeinfo = (JObject)gr.Value;

                        JObject ingredients = (JObject)gradeinfo["components"];

                        string indlist = "";

                        foreach (JProperty hp in ingredients.Children())
                        {
                            EliteDangerousCore.MaterialCommodityData mat = EliteDangerousCore.MaterialCommodityData.GetCachedMaterialByName(hp.Name);
                            if (mat == null)
                                Console.WriteLine(hp.Name + " **** Not found in " + fdname + " " + grname);
                            else
                            {
                                string postfix = "(" + mat.name + ")";
                                indlist = indlist.AppendPrePad(hp.Value.Int() + mat.shortname + postfix, ",");
                            }
                        }

                        EngEntry eng = list.Find(x => x.fdname == fdname && x.grade == gr.Name);

                        if (eng == null)
                        {
                            Console.WriteLine(" **** No engineer for " + fdname + "," + gr.Name);
                        }
                        else
                        {
                            string englist = eng.englist;

                            string name = fdname.SplitCapsWordFull();
                            string cat = name.Word(new char[] { ' ' }, 1);
                            string classline = "new EngineeringRecipe(\"" + name + "\", \"" + indlist + "\", \"" + cat + "\", \"" + gr.Name + "\", \"" + englist + "\" ),";

                            ret += classline + Environment.NewLine;
                        }
                    }
                }



            }
            catch (Exception e)
            {
                Console.WriteLine("Exception " + e.ToString());
            }

            return ret;
        }

    }
}


