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
            public string fdname;
            public string grade;
            public string englist;
        }

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
                            list.Add(new EngEntry() { fdname = bpname, grade = grname, englist = englist });

                            //Console.WriteLine("Eng " + bpname + " : " + grname + " : " + englist);

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

            EliteDangerousCore.MaterialCommodityDB.SetUpInitialTable();

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
                            EliteDangerousCore.MaterialCommodityDB mat = EliteDangerousCore.MaterialCommodityDB.GetCachedMaterialByName(hp.Name);
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
                            string classline = "new EngineeringRecipe(\"" + name + "\", \"" + indlist + "\", \"" + cat + "\", \"" + gr.Name + "\", \"" + englist + "\" }";

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


