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
                    EliteDangerousCore.MaterialCommodityDB mc = EliteDangerousCore.MaterialCommodityDB.GetCachedMaterialByName(name);

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
            EliteDangerousCore.MaterialCommodityDB.SetUpInitialTable();
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
                    fdname = fdname.Substring(0,fdname.LastIndexOf('_'));

                    //Console.WriteLine(name + " : " + descr + ":" + matid1.Value + "x" + matid1count.Value);

                    string ing = MatName(matid1, matid1count, filemats);
                    ing = ing.AppendPrePad(MatName(matid2, matid2count, filemats), ",");
                    ing = ing.AppendPrePad(MatName(matid3, matid3count, filemats), ",");

                    string cat = fdname.Word(new char[] { '_' }, 1).SplitCapsWordFull();
                    if (cat == "FS Dinterdictor")
                        cat = "FSD Interdictor";

                    string lookupname = fdname;
                    lookupname = lookupname.Replace(" ", "");
                    if (lookupname.StartsWith("BeamLaser_") || lookupname.StartsWith("BurstLaser_") || lookupname.StartsWith("Cannon_") ||
                                lookupname.StartsWith("FragCannon_") || lookupname.StartsWith("Mine_") || lookupname.StartsWith("Missile_") ||
                                lookupname.StartsWith("Multicannon_") || lookupname.StartsWith("PlasmaAccelerator_") ||
                                lookupname.StartsWith("PulseLaser_") || lookupname.StartsWith("RailGun") || lookupname.StartsWith("Torpedo")
                        )
                        lookupname = "Weapon" + lookupname.Substring(lookupname.IndexOf("_"));
                    else if (lookupname.StartsWith("CargoScanner_FastScan"))
                        lookupname = "Sensor_CargoScanner_FastScan";
                    else if (lookupname.StartsWith("CargoScanner_LongRange"))
                        lookupname = "Sensor_CargoScanner_LongRange";
                    else if (lookupname.StartsWith("CargoScanner_WideAngle"))
                        lookupname = "Sensor_CargoScanner_WideAngle";
                    else if (lookupname.StartsWith("SurfaceScanner_"))
                        lookupname = "Sensor_SurfaceScanner" + lookupname.Substring(lookupname.IndexOf("_"));
                    else if (lookupname.StartsWith("WakeScanner_") && !(lookupname.Contains("Light") || lookupname.Contains("Reinforced") || lookupname.Contains("Shielded")))
                        lookupname = "Sensor_WakeScanner" + lookupname.Substring(lookupname.IndexOf("_"));
                    else if (lookupname.StartsWith("KillWarrantScanner_FastScan") || lookupname.StartsWith("KillWarrantScanner_WideAngle"))
                        lookupname = "Sensor_KillWarrantScanner" + lookupname.Substring(lookupname.IndexOf("_"));
                    else if (lookupname.StartsWith("Sensor_"))
                        lookupname = "Sensor_Sensor" + lookupname.Substring(lookupname.IndexOf("_"));

                    CorolisEngineering.EngEntry eng = englist.Find(x => x.fdname == lookupname && x.grade == level.ToString());

                    if (ukname.StartsWith("Misc "))
                        ukname = ukname.Substring(5);

                    if (eng != null)
                    {
                        string engnames = eng.englist;
                        string classline = "        new EngineeringRecipe(\"" + ukname + "\", \"" + ing + "\", \"" + cat + "\", \"" + level.Value.ToString() + "\", \"" + engnames + "\" }";
                        ret += classline + Environment.NewLine;
                    }
                    else
                    {
                        Console.WriteLine("Corolis no data for " + fdname + "," + level);

                    }
                }
            }

            return ret;
        }
    }
}
