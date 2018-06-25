using BaseUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// take the frontier data excel they send us
// output the recipes tab to recipes.csv
// output the mats tab to mats.csv
// copy in corolis modules.json into the same folder
// run it.

namespace NetLogEntry
{
    static public class FrontierData
    {
        static string MatName(long? matid, long? matcount, CVSFile mat)
        {
            if (matid != null && matcount != null )
            {
                int matrow = mat.FindInColumn("A", matid.Value.ToString());
                if (matrow >= 0)
                {
                    string name = mat[matrow]["F"].Trim();
                    EliteDangerousCore.MaterialCommodityData mc = EliteDangerousCore.MaterialCommodityData.GetCachedMaterialByName(name);

                    if (mc != null)
                        return matcount.Value.ToString() + mc.shortname; //+ "(" + mc.name + ")";
                    else
                        return matcount.Value.ToString() + name + "( **** UNKNOWN *****)";

                }
            }
            return "";
        }

        static public string Process(string rootpath)            // overall index of items
        {
            EliteDangerousCore.MaterialCommodityData.SetUpInitialTable();
            string ret = "// DATA FROM Frontiers excel spreadsheet with corolis modules.json to give engineers names" + Environment.NewLine;
            ret += "// DO NOT UPDATE MANUALLY - use the netlogentry frontierdata scanner to do this" + Environment.NewLine;
            ret += "" + Environment.NewLine;

            string recipies = Path.Combine(rootpath, "recipes.csv");
            CVSFile filerecipes = new CVSFile();
            if (!filerecipes.Read(recipies))
            {
                return "ERROR no recipies";
            }

            string materials = Path.Combine(rootpath, "mats.csv");
            CVSFile filemats = new CVSFile();
            if (!filemats.Read(materials))
            {
                return "ERROR no mats";
            }

            List<CorolisEngineering.EngEntry> englist = CorolisEngineering.GetEng(rootpath);

            foreach ( CVSFile.Row line in filerecipes.Rows)
            {
                string fdname = line["A"];
                string ukname = line["C"];
                string descr = line["D"];
                int? level = line.GetInt("P");
                long? matid1 = line.GetInt("Q");
                long? matid1count = line.GetInt("R");
                long? matid2 = line.GetInt("S");
                long? matid2count = line.GetInt("T");
                long? matid3 = line.GetInt("U");
                long? matid3count = line.GetInt("V");

                if (level != null)
                {
                    fdname = fdname.Substring(0, fdname.LastIndexOf('_'));      //examples, AFM_Shielded, Armour_Heavy Duty
                    string fdfront = fdname.Substring(0, fdname.IndexOf('_'));
                    string fdback = fdname.Substring(fdname.IndexOf('_')+1).Replace(" ","");

                    string ing = MatName(matid1, matid1count, filemats);
                    ing = ing.AppendPrePad(MatName(matid2, matid2count, filemats), ",");
                    ing = ing.AppendPrePad(MatName(matid3, matid3count, filemats), ",");

                    string cat = fdname.Word(new char[] { '_' }, 1).SplitCapsWordFull();
                    if (cat == "FS Dinterdictor")
                        cat = "FSD Interdictor";

                    List<CorolisEngineering.EngEntry> modulelist = (from x in englist where x.corolisfrontierid == fdfront select x).ToList();

                    string engnames = "Not Known";

                    if ( modulelist.Count == 0 )
                    {
                        Console.WriteLine("No matching corolis module found " + fdname + ":" + fdfront);
                    }
                    else
                    {
                        CorolisEngineering.EngEntry anylevelentry = (from x in modulelist where x.fdname.IndexOf(fdback, StringComparison.InvariantCultureIgnoreCase) >= 0 select x).FirstOrDefault();
                        CorolisEngineering.EngEntry entry = (from x in modulelist where x.fdname.IndexOf(fdback, StringComparison.InvariantCultureIgnoreCase) >= 0 && x.grade == level.ToString() select x).FirstOrDefault();

                        if (entry == null)
                        {
                            if ( anylevelentry!=null )
                                Console.WriteLine("Possible mismatched grade for " + fdname + ":" + fdfront + ":" + fdback + " at " + level);
                            else
                                Console.WriteLine("No matching engineering found for " + fdname + ":" + fdfront + ":" + fdback);

                            modulelist = null;
                        }
                        else
                        {
                            //Console.WriteLine("engineering found for FDName: " + fdname + ": Entry " + entry.fdname);
                            engnames = entry.englist;
                            engnames = engnames.Replace("Chung", "Cheung");     // fix found misspelling 25 june 18
                        }
                    }

                    if (ukname.StartsWith("Misc "))
                        ukname = ukname.Substring(5);

                    if (modulelist != null)
                    {
                        string classline = "        new EngineeringRecipe(\"" + ukname + "\", \"" + ing + "\", \"" + cat + "\", \"" + level.Value.ToString() + "\", \"" + engnames + "\" ),";
                        ret += classline + Environment.NewLine;
                    }
                }
            }

            return ret;
        }
    }
}
