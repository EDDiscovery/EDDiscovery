using BaseUtils;
using EliteDangerousCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// take the frontier data excel they send us
// output the mats tab to mats.csv
// output the commdods tab to commods.csv
// output the recipes tab to recipes.csv
// output the weapons tab to weapons.csv
// output the shipdata tab to ships.csv
// output the module tab to modules.csv
// output the tech broker tab to techbroker.csv
// run it.
// will verify materials in MaterialCommododities vs the sheet
// will verify any replacement lists of fdname 
// will verify commodities in MaterialCommododities vs the sheet
// will check weapon data vs the shipmoduledata file
// will check modules vs the shipmoduledata file
// will read tech broker info and write out a new tech broker lines - import this manually into MaterialRecipes.cs
// will read the recipes cvs and print out the recipe lines for MaterialRecipes.cs. This is cross checked vs the englist at the bottom of the file
//      You need to keep this englist up to date manually with Inara and other sources for engineer names...  If one is found missing, you need to 
//      update the list at the bottom.

// Keep rare list at the bottom up to date manually with Inara and other sources

// it will print out any missing/mismatched data - some if to be expected.


namespace NetLogEntry
{
    static public class FrontierData
    {
        static string MatName(long? matid, long? matcount, CVSFile mat)
        {
            if (matid != null && matcount != null)
            {
                int matrow = mat.FindInColumn("A", matid.Value.ToString());
                if (matrow >= 0)
                {
                    string fdname = mat[matrow]["B"].Trim();
                    EliteDangerousCore.MaterialCommodityData mc = EliteDangerousCore.MaterialCommodityData.GetByFDName(fdname);

                    if (mc != null)
                        return matcount.Value.ToString() + mc.Shortname; //+ "(" + mc.name + ")";
                    else
                        return matcount.Value.ToString() + fdname + "( **** UNKNOWN *****)";

                }
            }
            return "";
        }

        static public string Process(string rootpath)            // overall index of items
        {
            EliteDangerousCore.MaterialCommodityData.SetUpInitialTable();

            string recipies = Path.Combine(rootpath, "recipes.csv");
            CVSFile filerecipes = new CVSFile();
            if (!filerecipes.Read(recipies))
            {
                return "ERROR no recipies csv";
            }

            string materials = Path.Combine(rootpath, "mats.csv");
            CVSFile filemats = new CVSFile();
            if (!filemats.Read(materials))
            {
                return "ERROR no mats csv";
            }

            CVSFile filecommods = new CVSFile();
            if (!filecommods.Read(Path.Combine(rootpath, "commods.csv")))
            {
                return "ERROR no commodity csv";
            }

            CVSFile filemodules = new CVSFile();
            if (!filemodules.Read(Path.Combine(rootpath, "modules.csv")))
            {
                return "ERROR no modules csv";
            }

            CVSFile fileshipdata = new CVSFile();
            if (!fileshipdata.Read(Path.Combine(rootpath, "ships.csv")))
            {
                return "ERROR no ships csv";
            }

            CVSFile fileweapons = new CVSFile();
            if (!fileweapons.Read(Path.Combine(rootpath, "weapons.csv")))
            {
                return "ERROR no weapons csv";
            }

            CVSFile filetechbroker = new CVSFile();
            if (!filetechbroker.Read(Path.Combine(rootpath, "techbroker.csv")))
            {
                return "ERROR no techbroker csv";
            }

            // Check materials vs the excel sheet

            {
                MaterialCommodityData[] ourmats = MaterialCommodityData.GetMaterials();

                foreach (MaterialCommodityData m in ourmats)
                {
                    if (filemats.FindInColumn(5, m.Name, StringComparison.InvariantCultureIgnoreCase, true) == -1)
                        Console.WriteLine("Error " + m.Name + " not found in frontier data");
                    if (filemats.FindInColumn(1, m.FDName, StringComparison.InvariantCultureIgnoreCase) == -1)
                        Console.WriteLine("Error " + m.FDName + " not found in frontier data");
                }

                foreach (CVSFile.Row rw in filemats.RowsExcludingHeaderRow)
                {
                    string fdname = rw[1];
                    string rarity = rw[2];
                    string category = rw[3];
                    string ukname = rw[5];

                    MaterialCommodityData cached = MaterialCommodityData.GetByFDName(fdname);

                    if (cached == null)
                    {
                        Console.WriteLine("Error " + fdname + " not found in cache");
                    }
                    else if (cached.Category != category)
                    {
                        Console.WriteLine("Error " + fdname + " type " + category + " disagrees with " + cached.Type);
                    }

                }
            }

            // check replacement lists

            {
                List<MaterialCommodityData> ourcommods = MaterialCommodityData.GetAll().ToList();

                foreach (KeyValuePair<string, string> kvp in MaterialCommodityData.fdnamemangling)
                {
                    if (ourcommods.Find(x => x.FDName.Equals(kvp.Value)) == null)
                    {
                        Console.WriteLine("Error " + kvp.Value + " replacement is not in our cache");
                    }
                }

            }

            // check commodities

            {
                List<MaterialCommodityData> ourcommods = MaterialCommodityData.GetCommodities().ToList();

                foreach (MaterialCommodityData m in ourcommods)     // check our list vs the excel
                {
                    int n = filecommods.FindInColumn(3, m.Name, StringComparison.InvariantCultureIgnoreCase, true);
                    int f = filecommods.FindInColumn(2, m.FDName, StringComparison.InvariantCultureIgnoreCase);

                    bool isinararare = InaraRares.Contains(m.Name);

                    if (n == -1)    // no name in excel
                    {
                        if (f != -1)
                            Console.WriteLine("Error " + m.Name + " not found but ID is " + filecommods.Rows[f][2]);
                        else
                            Console.WriteLine("Error " + m.Name + " not found in frontier data");
                    }

                    if (f == -1)    // no id in excel
                    {
                        if (n != -1)
                        {
                            CVSFile.Row rw = filecommods[n];
                            Console.WriteLine("! AddCommodity(\"" + rw[3] + "\", \"" + rw[1] + "\", \"" + rw[2] + "\");");
                        }
                        else
                            Console.WriteLine("Error FDNAME " + m.FDName + " not found in frontier data");
                    }

                    if (m.Rarity != isinararare)
                        Console.WriteLine("Rarity flag incorrect for " + m.FDName);
                }

                foreach (CVSFile.Row rw in filecommods.RowsExcludingHeaderRow)
                {
                    string type = rw[1].Trim();
                    string fdname = rw[2].Trim();
                    string ukname = rw[3].Trim();

                    bool isinararare = InaraRares.Contains(ukname);

                    MaterialCommodityData cached = MaterialCommodityData.GetByFDName(fdname);

                    if (cached == null || cached.Type != type)
                    {
                        Console.WriteLine("AddCommodity" + (isinararare ? "Rare" : "") + "(\"" + ukname + "\", \"" + type + "\", \"" + fdname + "\");");
                    }

                }

            }

            // Check weapons as much as we can..

            {
                foreach (CVSFile.Row rw in fileweapons.RowsExcludingHeaderRow)
                {
                    string fdname = rw[0].Trim();
                    string ukname = rw[1].Trim();
                    string type = rw["N"].Trim();
                    string mount = rw["O"].Trim();
                    string size = rw["P"].Trim();
                    double powerdraw = rw["AA"].InvariantParseDouble(0);

                    if (ShipModuleData.modules.ContainsKey(fdname.ToLower()))
                    {
                        ShipModuleData.ShipModule minfo = ShipModuleData.modules[fdname.ToLower()];

                        if (Math.Abs(minfo.Power - powerdraw) > 0.05)
                            Console.WriteLine("Weapon " + fdname + " incorrect power draw " + minfo.Power + " vs " + powerdraw);
                    }
                    else
                    {
                        Console.WriteLine("Missing Weapon " + fdname);
                    }
                }
            }

            // Check modules
            {
                foreach (CVSFile.Row rw in filemodules.RowsExcludingHeaderRow)
                {
                    string fdname = rw[0].Trim();
                    string ukname = rw[1].Trim();
                    string ukdesc = rw[2].Trim();
                    string size = rw["N"].Trim();
                    double mass = rw["P"].InvariantParseDouble(0);
                    double powerdraw = rw["Q"].InvariantParseDouble(0);

                    if (ukdesc.IndexOf("(Information)", StringComparison.InvariantCultureIgnoreCase) == -1 && !fdname.Contains("_free"))
                    {
                        if (ShipModuleData.modules.ContainsKey(fdname.ToLower()))
                        {
                            ShipModuleData.ShipModule minfo = ShipModuleData.modules[fdname.ToLower()];

                            if (Math.Abs(minfo.Power - powerdraw) > 0.05)
                                Console.WriteLine("Module " + fdname + " incorrect power draw " + minfo.Power + " vs " + powerdraw);
                            if (Math.Abs(minfo.Mass - mass) > 0.05)
                                Console.WriteLine("Module " + fdname + " incorrect mass " + minfo.Mass + " vs " + mass);
                        }
                        else
                        {
                            Console.WriteLine("Missing Module " + fdname);
                        }
                    }
                    else
                    {
                        // Console.WriteLine("Rejected Module " + fdname + " "+ ukdesc);
                    }
                }
            }

            // tech broker
            {
                foreach (CVSFile.Row rw in filetechbroker.RowsExcludingHeaderRow)
                {
                    string fdname = rw[0].Trim();
                    string type = rw["C"].Trim();
                    string size = rw["D"].Trim();
                    string mount = rw["E"].Trim();

                    string nicename = fdname.Replace("hpt_", "").Replace("int_", "").SplitCapsWordFull();

                    string[] ing = new string[5];
                    int[] count = new int[5];
                    MaterialCommodityData[] mat = new MaterialCommodityData[5];

                    int i = 0;
                    for (; i < 5; i++)
                    {
                        ing[i] = rw["F", i * 2].Trim();
                        count[i] = rw["G", i * 2].InvariantParseInt(0);
                        mat[i] = MaterialCommodityData.GetByFDName(ing[i]);
                        if (mat[i] == null)
                        {
                            Console.WriteLine("Material DB is not up to date with materials for techbroker - update first " + fdname);
                            break;
                        }
                        else if (!mat[i].Shortname.HasChars())
                        {
                            Console.WriteLine("Material DB entry does not have a shortname - update first " + ing[i]);
                            break;
                        }
                    }

                    if (i == 5)
                    {
                        Console.WriteLine("new TechBrokerUnlockRecipe(\"" + nicename + "\",\"" +
                            count[0].ToStringInvariant() + mat[0].Shortname + "," +
                            count[1].ToStringInvariant() + mat[1].Shortname + "," +
                            count[2].ToStringInvariant() + mat[2].Shortname + "," +
                            count[3].ToStringInvariant() + mat[3].Shortname + "," +
                            count[4].ToStringInvariant() + mat[4].Shortname + "\"),");

                    }
                    else
                        Console.WriteLine("Material DB is not up to date with materials for techbroker - update first " + fdname);
                }
            }

            // Calculate recipes

            string ret = "// DATA FROM Frontiers excel spreadsheet" + Environment.NewLine;
            ret += "// DO NOT UPDATE MANUALLY - use the netlogentry frontierdata scanner to do this" + Environment.NewLine;
            ret += "" + Environment.NewLine;
            string ret2 = "";

            foreach (CVSFile.Row line in filerecipes.Rows)
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

                if (ukname.StartsWith("Misc "))
                    ukname = ukname.Substring(5);

                if (level != null)
                {
                    fdname = fdname.Substring(0, fdname.LastIndexOf('_'));      //examples, AFM_Shielded, Armour_Heavy Duty
                    string fdfront = fdname.Substring(0, fdname.IndexOf('_'));
                    string fdback = fdname.Substring(fdname.IndexOf('_') + 1).Replace(" ", "");

                    string ing = MatName(matid1, matid1count, filemats);
                    ing = ing.AppendPrePad(MatName(matid2, matid2count, filemats), ",");
                    ing = ing.AppendPrePad(MatName(matid3, matid3count, filemats), ",");

                    string cat = fdname.Word(new char[] { '_' }, 1).SplitCapsWordFull();
                    if (cat == "FS Dinterdictor")
                        cat = "FSD Interdictor";

                    string engnames = "Not Known";

                    EngineerList en = Array.Find(ourenglist, x => x.cat == cat && x.ukname == ukname && x.level == level);
                    if (en != null)
                        engnames = en.engnames;
                    else
                        Console.WriteLine("No matching internal data for " + fdname + ":" + fdfront);

                    string classline = "        new EngineeringRecipe(\"" + ukname + "\", \"" + ing + "\", \"" + cat + "\", \"" + level.Value.ToString() + "\", \"" + engnames + "\" ),";
                    ret += classline + Environment.NewLine;
                }
            }

            {
                string de = "", fr = "", es = "", ru = "", pr = "";
                foreach (CVSFile.Row rw in filemats.RowsExcludingHeaderRow)
                {
                    string fdname = rw["B"].Trim();
                    string ukname = rw["F"].Trim();
                    string dename = rw["H"].Trim();
                    string frname = rw["J"].Trim();
                    string esname = rw["L"].Trim();
                    string runame = rw["N"].Trim();
                    string prname = rw["P"].Trim();

                    de += "MaterialCommodityData." + fdname.ToLower() + ": " + ukname.AlwaysQuoteString() + " => " + dename.AlwaysQuoteString() + Environment.NewLine;
                    fr += "MaterialCommodityData." + fdname.ToLower() + ": " + ukname.AlwaysQuoteString() + " => " + frname.AlwaysQuoteString() + Environment.NewLine;
                    es += "MaterialCommodityData." + fdname.ToLower() + ": " + ukname.AlwaysQuoteString() + " => " + esname.AlwaysQuoteString() + Environment.NewLine;
                    ru += "MaterialCommodityData." + fdname.ToLower() + ": " + ukname.AlwaysQuoteString() + " => " + runame.AlwaysQuoteString() + Environment.NewLine;
                    pr += "MaterialCommodityData." + fdname.ToLower() + ": " + ukname.AlwaysQuoteString() + " => " + prname.AlwaysQuoteString() + Environment.NewLine;
                }
                foreach (CVSFile.Row rw in filecommods.RowsExcludingHeaderRow)
                {
                    string fdname = rw["C"].Trim();
                    string ukname = rw["D"].Trim();
                    string dename = rw["F"].Trim();
                    string frname = rw["H"].Trim();
                    string esname = rw["J"].Trim();
                    string runame = rw["L"].Trim();
                    string prname = rw["N"].Trim();

                    de += "MaterialCommodityData." + fdname.ToLower() + ": " + ukname.AlwaysQuoteString() + " => " + dename.AlwaysQuoteString() + Environment.NewLine;
                    fr += "MaterialCommodityData." + fdname.ToLower() + ": " + ukname.AlwaysQuoteString() + " => " + frname.AlwaysQuoteString() + Environment.NewLine;
                    es += "MaterialCommodityData." + fdname.ToLower() + ": " + ukname.AlwaysQuoteString() + " => " + esname.AlwaysQuoteString() + Environment.NewLine;
                    ru += "MaterialCommodityData." + fdname.ToLower() + ": " + ukname.AlwaysQuoteString() + " => " + runame.AlwaysQuoteString() + Environment.NewLine;
                    pr += "MaterialCommodityData." + fdname.ToLower() + ": " + ukname.AlwaysQuoteString() + " => " + prname.AlwaysQuoteString() + Environment.NewLine;
                }

                File.WriteAllText(Path.Combine(rootpath, "mat-de.part.txt"), de, Encoding.UTF8);
                File.WriteAllText(Path.Combine(rootpath, "mat-fr.part.txt"), fr, Encoding.UTF8);
                File.WriteAllText(Path.Combine(rootpath, "mat-es.part.txt"), es, Encoding.UTF8);
                File.WriteAllText(Path.Combine(rootpath, "mat-ru.part.txt"), ru, Encoding.UTF8);
                File.WriteAllText(Path.Combine(rootpath, "mat-pr.part.txt"), pr, Encoding.UTF8);
            }

            return ret + ret2;
        }

        class EngineerList
        {
            public string cat;
            public string ukname;
            public int level;
            public string engnames;
            public EngineerList(string c, string u, int l, string e) { cat = c; ukname = u; level = l; engnames = e; }
        }

        //FROM coriolis, corrected manually for 3.1 on 2/july/18 from inara

        static EngineerList[] ourenglist = new EngineerList[]  {
                    new EngineerList( "AFM", "Shielded", 1, "Bill Turner,Lori Jameson"),
                    new EngineerList( "AFM", "Shielded", 2, "Bill Turner,Lori Jameson"),
                    new EngineerList( "AFM", "Shielded", 3, "Bill Turner,Lori Jameson"),
                    new EngineerList( "AFM", "Shielded", 4, "Lori Jameson"),
                    new EngineerList( "AFM", "Shielded", 5, "Unknown"),        //OK

                    new EngineerList( "Armour", "Lightweight Armour", 1, "Liz Ryder,Selene Jean"),
                    new EngineerList( "Armour", "Lightweight Armour", 2, "Selene Jean"),
                    new EngineerList( "Armour", "Lightweight Armour", 3, "Selene Jean"),
                    new EngineerList( "Armour", "Lightweight Armour", 4, "Selene Jean"),
                    new EngineerList( "Armour", "Lightweight Armour", 5, "Selene Jean"),    //OK

                    new EngineerList( "Armour", "Blast Resistant Armour", 1, "Liz Ryder,Selene Jean"),
                    new EngineerList( "Armour", "Blast Resistant Armour", 2, "Selene Jean"),
                    new EngineerList( "Armour", "Blast Resistant Armour", 3, "Selene Jean"),
                    new EngineerList( "Armour", "Blast Resistant Armour", 4, "Selene Jean"),
                    new EngineerList( "Armour", "Blast Resistant Armour", 5, "Selene Jean"),    //OK

                                new EngineerList( "Armour", "Heavy Duty Armour", 1, "Liz Ryder,Selene Jean"),
                    new EngineerList( "Armour", "Heavy Duty Armour", 2, "Selene Jean"),
                    new EngineerList( "Armour", "Heavy Duty Armour", 3, "Selene Jean"),
                    new EngineerList( "Armour", "Heavy Duty Armour", 4, "Selene Jean"),
                    new EngineerList( "Armour", "Heavy Duty Armour", 5, "Selene Jean"),     //OK

                    new EngineerList( "Armour", "Kinetic Resistant Armour", 1, "Liz Ryder,Selene Jean"),
                    new EngineerList( "Armour", "Kinetic Resistant Armour", 2, "Selene Jean"),
                    new EngineerList( "Armour", "Kinetic Resistant Armour", 3, "Selene Jean"),
                    new EngineerList( "Armour", "Kinetic Resistant Armour", 4, "Selene Jean"),
                    new EngineerList( "Armour", "Kinetic Resistant Armour", 5, "Selene Jean"),  //OK

                    new EngineerList( "Armour", "Thermal Resistant Armour", 1, "Liz Ryder,Selene Jean"),
                    new EngineerList( "Armour", "Thermal Resistant Armour", 2, "Selene Jean"),
                    new EngineerList( "Armour", "Thermal Resistant Armour", 3, "Selene Jean"),
                    new EngineerList( "Armour", "Thermal Resistant Armour", 4, "Selene Jean"),
                    new EngineerList( "Armour", "Thermal Resistant Armour", 5, "Selene Jean"),      //OK


                    new EngineerList( "Beam Laser", "Efficient Weapon", 1, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Beam Laser", "Efficient Weapon", 2, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Beam Laser", "Efficient Weapon", 3, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Beam Laser", "Efficient Weapon", 4, "Broo Tarquin"),
                    new EngineerList( "Beam Laser", "Efficient Weapon", 5, "Broo Tarquin"), //ok

                    new EngineerList( "Beam Laser", "Light Weight Mount", 1, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Beam Laser", "Light Weight Mount", 2, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Beam Laser", "Light Weight Mount", 3, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Beam Laser", "Light Weight Mount", 4, "Broo Tarquin"),
                    new EngineerList( "Beam Laser", "Light Weight Mount", 5, "Broo Tarquin"),   //OK
    
                    new EngineerList( "Beam Laser", "Long-Range Weapon", 1, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Beam Laser", "Long-Range Weapon", 2, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Beam Laser", "Long-Range Weapon", 3, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Beam Laser", "Long-Range Weapon", 4, "Broo Tarquin"),
                    new EngineerList( "Beam Laser", "Long-Range Weapon", 5, "Broo Tarquin"),    //OK
                    new EngineerList( "Beam Laser", "Overcharged Weapon", 1, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Beam Laser", "Overcharged Weapon", 2, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Beam Laser", "Overcharged Weapon", 3, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Beam Laser", "Overcharged Weapon", 4, "Broo Tarquin"),
                    new EngineerList( "Beam Laser", "Overcharged Weapon", 5, "Broo Tarquin"),   //OK
                    new EngineerList( "Beam Laser", "Short-Range Blaster", 1, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Beam Laser", "Short-Range Blaster", 2, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Beam Laser", "Short-Range Blaster", 3, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Beam Laser", "Short-Range Blaster", 4, "Broo Tarquin"),      //OK
                    new EngineerList( "Beam Laser", "Short-Range Blaster", 5, "Broo Tarquin"),
                    new EngineerList( "Beam Laser", "Sturdy Mount", 1, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Beam Laser", "Sturdy Mount", 2, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Beam Laser", "Sturdy Mount", 3, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Beam Laser", "Sturdy Mount", 4, "Broo Tarquin"),
                    new EngineerList( "Beam Laser", "Sturdy Mount", 5, "Broo Tarquin"),     //OK


                    new EngineerList( "Burst Laser", "Efficient Weapon", 1, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Burst Laser", "Efficient Weapon", 2, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Burst Laser", "Efficient Weapon", 3, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Burst Laser", "Efficient Weapon", 4, "Broo Tarquin"),
                    new EngineerList( "Burst Laser", "Efficient Weapon", 5, "Broo Tarquin"),        //OK
                    new EngineerList( "Burst Laser", "Focused Weapon", 1, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Burst Laser", "Focused Weapon", 2, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Burst Laser", "Focused Weapon", 3, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Burst Laser", "Focused Weapon", 4, "Broo Tarquin"),
                    new EngineerList( "Burst Laser", "Focused Weapon", 5, "Broo Tarquin"),
                    new EngineerList( "Burst Laser", "Light Weight Mount", 1, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Burst Laser", "Light Weight Mount", 2, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Burst Laser", "Light Weight Mount", 3, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Burst Laser", "Light Weight Mount", 4, "Broo Tarquin"),
                    new EngineerList( "Burst Laser", "Light Weight Mount", 5, "Broo Tarquin"),
                    new EngineerList( "Burst Laser", "Long-Range Weapon", 1, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Burst Laser", "Long-Range Weapon", 2, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Burst Laser", "Long-Range Weapon", 3, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Burst Laser", "Long-Range Weapon", 4, "Broo Tarquin"),
                    new EngineerList( "Burst Laser", "Long-Range Weapon", 5, "Broo Tarquin"),
                    new EngineerList( "Burst Laser", "Overcharged Weapon", 1, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Burst Laser", "Overcharged Weapon", 2, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Burst Laser", "Overcharged Weapon", 3, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Burst Laser", "Overcharged Weapon", 4, "Broo Tarquin"),
                    new EngineerList( "Burst Laser", "Overcharged Weapon", 5, "Broo Tarquin"),
                    new EngineerList( "Burst Laser", "Rapid Fire Modification", 1, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Burst Laser", "Rapid Fire Modification", 2, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Burst Laser", "Rapid Fire Modification", 3, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Burst Laser", "Rapid Fire Modification", 4, "Broo Tarquin"),
                    new EngineerList( "Burst Laser", "Rapid Fire Modification", 5, "Broo Tarquin"),
                    new EngineerList( "Burst Laser", "Short-Range Blaster", 1, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Burst Laser", "Short-Range Blaster", 2, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Burst Laser", "Short-Range Blaster", 3, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Burst Laser", "Short-Range Blaster", 4, "Broo Tarquin"),
                    new EngineerList( "Burst Laser", "Short-Range Blaster", 5, "Broo Tarquin"),
                    new EngineerList( "Burst Laser", "Sturdy Mount", 1, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Burst Laser", "Sturdy Mount", 2, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Burst Laser", "Sturdy Mount", 3, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Burst Laser", "Sturdy Mount", 4, "Broo Tarquin"),
                    new EngineerList( "Burst Laser", "Sturdy Mount", 5, "Broo Tarquin"),        //OK


                    new EngineerList( "Cannon", "Efficient Weapon", 1, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Cannon", "Efficient Weapon", 2, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Cannon", "Efficient Weapon", 3, "The Sarge"),
                    new EngineerList( "Cannon", "Efficient Weapon", 4, "The Sarge"),
                    new EngineerList( "Cannon", "Efficient Weapon", 5, "The Sarge"),
                    new EngineerList( "Cannon", "High Capacity Magazine", 1, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Cannon", "High Capacity Magazine", 2, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Cannon", "High Capacity Magazine", 3, "The Sarge"),
                    new EngineerList( "Cannon", "High Capacity Magazine", 4, "The Sarge"),
                    new EngineerList( "Cannon", "High Capacity Magazine", 5, "The Sarge"),
                    new EngineerList( "Cannon", "Light Weight Mount", 1, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Cannon", "Light Weight Mount", 2, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Cannon", "Light Weight Mount", 3, "The Sarge"),
                    new EngineerList( "Cannon", "Light Weight Mount", 4, "The Sarge"),
                    new EngineerList( "Cannon", "Light Weight Mount", 5, "The Sarge"),
                    new EngineerList( "Cannon", "Long-Range Weapon", 1, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Cannon", "Long-Range Weapon", 2, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Cannon", "Long-Range Weapon", 3, "The Sarge"),
                    new EngineerList( "Cannon", "Long-Range Weapon", 4, "The Sarge"),
                    new EngineerList( "Cannon", "Long-Range Weapon", 5, "The Sarge"),
                    new EngineerList( "Cannon", "Overcharged Weapon", 1, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Cannon", "Overcharged Weapon", 2, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Cannon", "Overcharged Weapon", 3, "The Sarge"),
                    new EngineerList( "Cannon", "Overcharged Weapon", 4, "The Sarge"),
                    new EngineerList( "Cannon", "Overcharged Weapon", 5, "The Sarge"),
                    new EngineerList( "Cannon", "Rapid Fire Modification", 1, "Unknown"),
                    new EngineerList( "Cannon", "Rapid Fire Modification", 2, "Unknown"),
                    new EngineerList( "Cannon", "Rapid Fire Modification", 3, "Unknown"),
                    new EngineerList( "Cannon", "Rapid Fire Modification", 4, "Unknown"),
                    new EngineerList( "Cannon", "Rapid Fire Modification", 5, "Unknown"),
                    new EngineerList( "Cannon", "Short-Range Blaster", 1, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Cannon", "Short-Range Blaster", 2, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Cannon", "Short-Range Blaster", 3, "The Sarge"),
                    new EngineerList( "Cannon", "Short-Range Blaster", 4, "The Sarge"),
                    new EngineerList( "Cannon", "Short-Range Blaster", 5, "The Sarge"),
                    new EngineerList( "Cannon", "Sturdy Mount", 1, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Cannon", "Sturdy Mount", 2, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Cannon", "Sturdy Mount", 3, "The Sarge"),
                    new EngineerList( "Cannon", "Sturdy Mount", 4, "The Sarge"),
                    new EngineerList( "Cannon", "Sturdy Mount", 5, "The Sarge"),    //ok

                    new EngineerList( "Cargo Scanner", "Fast Scanner", 1, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Fast Scanner", 2, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Fast Scanner", 3, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Fast Scanner", 4, "Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Fast Scanner", 5, "Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Lightweight", 1, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Lightweight", 2, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Lightweight", 3, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Lightweight", 4, "Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Lightweight", 5, "Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Long-Range Scanner", 1, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Long-Range Scanner", 2, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Long-Range Scanner", 3, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Long-Range Scanner", 4, "Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Long-Range Scanner", 5, "Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Reinforced", 1, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Reinforced", 2, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Reinforced", 3, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Reinforced", 4, "Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Reinforced", 5, "Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Shielded", 1, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Shielded", 2, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Shielded", 3, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Shielded", 4, "Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Shielded", 5, "Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Wide Angle Scanner", 1, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Wide Angle Scanner", 2, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Wide Angle Scanner", 3, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Wide Angle Scanner", 4, "Tiana Fortune"),
                    new EngineerList( "Cargo Scanner", "Wide Angle Scanner", 5, "Tiana Fortune"),       //OK

                    new EngineerList( "Chaff Launcher", "Chaff Ammo Capacity", 1, "Ram Tah"),
                    new EngineerList( "Chaff Launcher", "Lightweight", 1, "Ram Tah"),
                    new EngineerList( "Chaff Launcher", "Lightweight", 2, "Ram Tah"),
                    new EngineerList( "Chaff Launcher", "Lightweight", 3, "Ram Tah"),
                    new EngineerList( "Chaff Launcher", "Lightweight", 4, "Ram Tah"),
                    new EngineerList( "Chaff Launcher", "Lightweight", 5, "Ram Tah"),
                    new EngineerList( "Chaff Launcher", "Reinforced", 1, "Ram Tah"),
                    new EngineerList( "Chaff Launcher", "Reinforced", 2, "Ram Tah"),
                    new EngineerList( "Chaff Launcher", "Reinforced", 3, "Ram Tah"),
                    new EngineerList( "Chaff Launcher", "Reinforced", 4, "Ram Tah"),
                    new EngineerList( "Chaff Launcher", "Reinforced", 5, "Ram Tah"),
                    new EngineerList( "Chaff Launcher", "Shielded", 1, "Ram Tah"),
                    new EngineerList( "Chaff Launcher", "Shielded", 2, "Ram Tah"),
                    new EngineerList( "Chaff Launcher", "Shielded", 3, "Ram Tah"),
                    new EngineerList( "Chaff Launcher", "Shielded", 4, "Ram Tah"),
                    new EngineerList( "Chaff Launcher", "Shielded", 5, "Ram Tah"), //OK

                    new EngineerList( "Collection Limpet", "Lightweight", 1, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Collection Limpet", "Lightweight", 2, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Collection Limpet", "Lightweight", 3, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Collection Limpet", "Lightweight", 4, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Collection Limpet", "Lightweight", 5, "The Sarge,Tiana Fortune"),
                    new EngineerList( "Collection Limpet", "Reinforced", 1, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Collection Limpet", "Reinforced", 2, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Collection Limpet", "Reinforced", 3, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Collection Limpet", "Reinforced", 4, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Collection Limpet", "Reinforced", 5, "The Sarge,Tiana Fortune"),
                    new EngineerList( "Collection Limpet", "Shielded", 1, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Collection Limpet", "Shielded", 2, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Collection Limpet", "Shielded", 3, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Collection Limpet", "Shielded", 4, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Collection Limpet", "Shielded", 5, "The Sarge,Tiana Fortune"),  //OK

                    new EngineerList( "ECM", "Lightweight", 1, "Ram Tah"),
                    new EngineerList( "ECM", "Lightweight", 2, "Ram Tah"),
                    new EngineerList( "ECM", "Lightweight", 3, "Ram Tah"),
                    new EngineerList( "ECM", "Lightweight", 4, "Ram Tah"),
                    new EngineerList( "ECM", "Lightweight", 5, "Ram Tah"),
                    new EngineerList( "ECM", "Reinforced", 1, "Ram Tah"),
                    new EngineerList( "ECM", "Reinforced", 2, "Ram Tah"),
                    new EngineerList( "ECM", "Reinforced", 3, "Ram Tah"),
                    new EngineerList( "ECM", "Reinforced", 4, "Ram Tah"),
                    new EngineerList( "ECM", "Reinforced", 5, "Ram Tah"),
                    new EngineerList( "ECM", "Shielded", 1, "Ram Tah"),
                    new EngineerList( "ECM", "Shielded", 2, "Ram Tah"),
                    new EngineerList( "ECM", "Shielded", 3, "Ram Tah"),
                    new EngineerList( "ECM", "Shielded", 4, "Ram Tah"),
                    new EngineerList( "ECM", "Shielded", 5, "Ram Tah"),    //OK

                    new EngineerList( "Engine", "Dirty Drive Tuning", 1, "Elvira Martuuk,Felicty Farseer,Professor Palin"),
                    new EngineerList( "Engine", "Dirty Drive Tuning", 2, "Elvira Martuuk,Felicty Farseer,Professor Palin"),
                    new EngineerList( "Engine", "Dirty Drive Tuning", 3, "Felicty Farseer,Professor Palin"),
                    new EngineerList( "Engine", "Dirty Drive Tuning", 4, "Professor Palin"),
                    new EngineerList( "Engine", "Dirty Drive Tuning", 5, "Professor Palin"),
                    new EngineerList( "Engine", "Drive Strengthening", 1, "Elvira Martuuk,Felicty Farseer,Professor Palin"),
                    new EngineerList( "Engine", "Drive Strengthening", 2, "Elvira Martuuk,Felicty Farseer,Professor Palin"),
                    new EngineerList( "Engine", "Drive Strengthening", 3, "Felicty Farseer,Professor Palin"),
                    new EngineerList( "Engine", "Drive Strengthening", 4, "Professor Palin"),
                    new EngineerList( "Engine", "Drive Strengthening", 5, "Professor Palin"),
                    new EngineerList( "Engine", "Clean Drive Tuning", 1, "Elvira Martuuk,Felicty Farseer,Professor Palin"),
                    new EngineerList( "Engine", "Clean Drive Tuning", 2, "Elvira Martuuk,Felicty Farseer,Professor Palin"),
                    new EngineerList( "Engine", "Clean Drive Tuning", 3, "Felicty Farseer,Professor Palin"),
                    new EngineerList( "Engine", "Clean Drive Tuning", 4, "Professor Palin"),
                    new EngineerList( "Engine", "Clean Drive Tuning", 5, "Professor Palin"),        //OK

                    new EngineerList( "Frag Cannon", "Double Shot", 1, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Double Shot", 2, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Double Shot", 3, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Double Shot", 4, "Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Double Shot", 5, "Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Efficient Weapon", 1, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Efficient Weapon", 2, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Efficient Weapon", 3, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Efficient Weapon", 4, "Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Efficient Weapon", 5, "Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "High Capacity Magazine", 1, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "High Capacity Magazine", 2, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "High Capacity Magazine", 3, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "High Capacity Magazine", 4, "Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "High Capacity Magazine", 5, "Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Light Weight Mount", 1, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Light Weight Mount", 2, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Light Weight Mount", 3, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Light Weight Mount", 4, "Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Light Weight Mount", 5, "Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Overcharged Weapon", 1, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Overcharged Weapon", 2, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Overcharged Weapon", 3, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Overcharged Weapon", 4, "Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Overcharged Weapon", 5, "Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Rapid Fire Modification", 1, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Rapid Fire Modification", 2, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Rapid Fire Modification", 3, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Rapid Fire Modification", 4, "Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Rapid Fire Modification", 5, "Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Sturdy Mount", 1, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Sturdy Mount", 2, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Sturdy Mount", 3, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Sturdy Mount", 4, "Zacariah Nemo"),
                    new EngineerList( "Frag Cannon", "Sturdy Mount", 5, "Zacariah Nemo"),  //OK


                    new EngineerList( "FSD", "Faster FSD Boot Sequence", 1, "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
                    new EngineerList( "FSD", "Faster FSD Boot Sequence", 2, "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
                    new EngineerList( "FSD", "Faster FSD Boot Sequence", 3, "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
                    new EngineerList( "FSD", "Faster FSD Boot Sequence", 4, "Elvira Martuuk,Felicity Farseer"),
                    new EngineerList( "FSD", "Faster FSD Boot Sequence", 5, "Elvira Martuuk,Felicity Farseer"),
                    new EngineerList( "FSD", "Increased FSD Range", 1, "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
                    new EngineerList( "FSD", "Increased FSD Range", 2, "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
                    new EngineerList( "FSD", "Increased FSD Range", 3, "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
                    new EngineerList( "FSD", "Increased FSD Range", 4, "Elvira Martuuk,Felicity Farseer"),
                    new EngineerList( "FSD", "Increased FSD Range", 5, "Elvira Martuuk,Felicity Farseer"),
                    new EngineerList( "FSD", "Shielded FSD", 1, "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
                    new EngineerList( "FSD", "Shielded FSD", 2, "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
                    new EngineerList( "FSD", "Shielded FSD", 3, "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
                    new EngineerList( "FSD", "Shielded FSD", 4, "Elvira Martuuk,Felicity Farseer"),
                    new EngineerList( "FSD", "Shielded FSD", 5, "Elvira Martuuk,Felicity Farseer"),     //OK


                    new EngineerList( "FSD Interdictor", "Expanded FSD Interdictor Capture Arc", 1, "Colonel Bris Dekker,Felicity Farseer,Tiana Fortune"),
                    new EngineerList( "FSD Interdictor", "Expanded FSD Interdictor Capture Arc", 2, "Colonel Bris Dekker,Tiana Fortune"),
                    new EngineerList( "FSD Interdictor", "Expanded FSD Interdictor Capture Arc", 3, "Colonel Bris Dekker,Tiana Fortune"),
                    new EngineerList( "FSD Interdictor", "Expanded FSD Interdictor Capture Arc", 4, "Colonel Bris Dekker"),
                    new EngineerList( "FSD Interdictor", "Expanded FSD Interdictor Capture Arc", 5, "Unknown"),
                    new EngineerList( "FSD Interdictor", "Longer Range FSD Interdictor", 1, "Colonel Bris Dekker,Felicity Farseer,Tiana Fortune"),
                    new EngineerList( "FSD Interdictor", "Longer Range FSD Interdictor", 2, "Colonel Bris Dekker,Tiana Fortune"),
                    new EngineerList( "FSD Interdictor", "Longer Range FSD Interdictor", 3, "Colonel Bris Dekker,Tiana Fortune"),
                    new EngineerList( "FSD Interdictor", "Longer Range FSD Interdictor", 4, "Colonel Bris Dekker"),
                    new EngineerList( "FSD Interdictor", "Longer Range FSD Interdictor", 5, "Unknown"), //OK

                    new EngineerList( "Fuel Scoop", "Shielded", 1, "Bill Turner,Lori Jameson"),
                    new EngineerList( "Fuel Scoop", "Shielded", 2, "Bill Turner,Lori Jameson"),
                    new EngineerList( "Fuel Scoop", "Shielded", 3, "Bill Turner,Lori Jameson"),
                    new EngineerList( "Fuel Scoop", "Shielded", 4, "Lori Jameson"),
                    new EngineerList( "Fuel Scoop", "Shielded", 5, "Unknown"), //OK

                    new EngineerList( "Fuel Transfer Limpet", "Lightweight", 1, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Fuel Transfer Limpet", "Lightweight", 2, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Fuel Transfer Limpet", "Lightweight", 3, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Fuel Transfer Limpet", "Lightweight", 4, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Fuel Transfer Limpet", "Lightweight", 5, "The Sarge,Tiana Fortune"),
                    new EngineerList( "Fuel Transfer Limpet", "Reinforced", 1, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Fuel Transfer Limpet", "Reinforced", 2, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Fuel Transfer Limpet", "Reinforced", 3, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Fuel Transfer Limpet", "Reinforced", 4, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Fuel Transfer Limpet", "Reinforced", 5, "The Sarge,Tiana Fortune"),
                    new EngineerList( "Fuel Transfer Limpet", "Shielded", 1, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Fuel Transfer Limpet", "Shielded", 2, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Fuel Transfer Limpet", "Shielded", 3, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Fuel Transfer Limpet", "Shielded", 4, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Fuel Transfer Limpet", "Shielded", 5, "The Sarge,Tiana Fortune"), //OK

                    new EngineerList( "Hatch Breaker Limpet", "Lightweight", 1, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Hatch Breaker Limpet", "Lightweight", 2, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Hatch Breaker Limpet", "Lightweight", 3, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Hatch Breaker Limpet", "Lightweight", 4, "The Sarge,Tiana Fortune"),
                    new EngineerList( "Hatch Breaker Limpet", "Lightweight", 5, "The Sarge,Tiana Fortune"),
                    new EngineerList( "Hatch Breaker Limpet", "Reinforced", 1, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Hatch Breaker Limpet", "Reinforced", 2, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Hatch Breaker Limpet", "Reinforced", 3, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Hatch Breaker Limpet", "Reinforced", 4, "The Sarge,Tiana Fortune"),
                    new EngineerList( "Hatch Breaker Limpet", "Reinforced", 5, "The Sarge,Tiana Fortune"),
                    new EngineerList( "Hatch Breaker Limpet", "Shielded", 1, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Hatch Breaker Limpet", "Shielded", 2, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Hatch Breaker Limpet", "Shielded", 3, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Hatch Breaker Limpet", "Shielded", 4, "The Sarge,Tiana Fortune"),
                    new EngineerList( "Hatch Breaker Limpet", "Shielded", 5, "The Sarge,Tiana Fortune"),

                    new EngineerList( "Heat Sink Launcher", "Heatsink Ammo Capacity", 1, "Ram Tah"),
                    new EngineerList( "Heat Sink Launcher", "Lightweight", 1, "Ram Tah"),
                    new EngineerList( "Heat Sink Launcher", "Lightweight", 2, "Ram Tah"),
                    new EngineerList( "Heat Sink Launcher", "Lightweight", 3, "Ram Tah"),
                    new EngineerList( "Heat Sink Launcher", "Lightweight", 4, "Ram Tah"),
                    new EngineerList( "Heat Sink Launcher", "Lightweight", 5, "Ram Tah"),
                    new EngineerList( "Heat Sink Launcher", "Reinforced", 1, "Ram Tah"),
                    new EngineerList( "Heat Sink Launcher", "Reinforced", 2, "Ram Tah"),
                    new EngineerList( "Heat Sink Launcher", "Reinforced", 3, "Ram Tah"),
                    new EngineerList( "Heat Sink Launcher", "Reinforced", 4, "Ram Tah"),
                    new EngineerList( "Heat Sink Launcher", "Reinforced", 5, "Ram Tah"),
                    new EngineerList( "Heat Sink Launcher", "Shielded", 1, "Ram Tah"),
                    new EngineerList( "Heat Sink Launcher", "Shielded", 2, "Ram Tah"),
                    new EngineerList( "Heat Sink Launcher", "Shielded", 3, "Ram Tah"),
                    new EngineerList( "Heat Sink Launcher", "Shielded", 4, "Ram Tah"),
                    new EngineerList( "Heat Sink Launcher", "Shielded", 5, "Ram Tah"), //OK

                    new EngineerList( "Hull Reinforcement", "Lightweight Hull Reinforcement", 1, "Liz Ryder,Selene Jean"),
                    new EngineerList( "Hull Reinforcement", "Lightweight Hull Reinforcement", 2, "Selene Jean"),
                    new EngineerList( "Hull Reinforcement", "Lightweight Hull Reinforcement", 3, "Selene Jean"),
                    new EngineerList( "Hull Reinforcement", "Lightweight Hull Reinforcement", 4, "Selene Jean"),
                    new EngineerList( "Hull Reinforcement", "Lightweight Hull Reinforcement", 5, "Selene Jean"),
                    new EngineerList( "Hull Reinforcement", "Blast Resistant Hull Reinforcement", 1, "Liz Ryder,Selene Jean"),
                    new EngineerList( "Hull Reinforcement", "Blast Resistant Hull Reinforcement", 2, "Selene Jean"),
                    new EngineerList( "Hull Reinforcement", "Blast Resistant Hull Reinforcement", 3, "Selene Jean"),
                    new EngineerList( "Hull Reinforcement", "Blast Resistant Hull Reinforcement", 4, "Selene Jean"),
                    new EngineerList( "Hull Reinforcement", "Blast Resistant Hull Reinforcement", 5, "Selene Jean"),
                    new EngineerList( "Hull Reinforcement", "Heavy Duty Hull Reinforcement", 1, "Liz Ryder,Selene Jean"),
                    new EngineerList( "Hull Reinforcement", "Heavy Duty Hull Reinforcement", 2, "Selene Jean"),
                    new EngineerList( "Hull Reinforcement", "Heavy Duty Hull Reinforcement", 3, "Selene Jean"),
                    new EngineerList( "Hull Reinforcement", "Heavy Duty Hull Reinforcement", 4, "Selene Jean"),
                    new EngineerList( "Hull Reinforcement", "Heavy Duty Hull Reinforcement", 5, "Selene Jean"),
                    new EngineerList( "Hull Reinforcement", "Kinetic Resistant Hull Reinforcement", 1, "Liz Ryder,Selene Jean"),
                    new EngineerList( "Hull Reinforcement", "Kinetic Resistant Hull Reinforcement", 2, "Selene Jean"),
                    new EngineerList( "Hull Reinforcement", "Kinetic Resistant Hull Reinforcement", 3, "Selene Jean"),
                    new EngineerList( "Hull Reinforcement", "Kinetic Resistant Hull Reinforcement", 4, "Selene Jean"),
                    new EngineerList( "Hull Reinforcement", "Kinetic Resistant Hull Reinforcement", 5, "Selene Jean"),
                    new EngineerList( "Hull Reinforcement", "Thermal Resistant Hull Reinforcement", 1, "Liz Ryder,Selene Jean"),
                    new EngineerList( "Hull Reinforcement", "Thermal Resistant Hull Reinforcement", 2, "Selene Jean"),
                    new EngineerList( "Hull Reinforcement", "Thermal Resistant Hull Reinforcement", 3, "Selene Jean"),
                    new EngineerList( "Hull Reinforcement", "Thermal Resistant Hull Reinforcement", 4, "Selene Jean"),
                    new EngineerList( "Hull Reinforcement", "Thermal Resistant Hull Reinforcement", 5, "Selene Jean"),  //OK


                    new EngineerList( "Kill Warrant Scanner", "Fast Scanner", 1, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Fast Scanner", 2, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Fast Scanner", 3, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Fast Scanner", 4, "Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Fast Scanner", 5, "Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Lightweight", 1, "Bill Turner,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Lightweight", 2, "Bill Turner,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Lightweight", 3, "Bill Turner,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Lightweight", 4, "Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Lightweight", 5, "Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Long-Range Scanner", 1, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Long-Range Scanner", 2, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Long-Range Scanner", 3, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Long-Range Scanner", 4, "Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Long-Range Scanner", 5, "Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Reinforced", 1, "Bill Turner,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Reinforced", 2, "Bill Turner,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Reinforced", 3, "Bill Turner,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Reinforced", 4, "Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Reinforced", 5, "Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Shielded", 1, "Bill Turner,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Shielded", 2, "Bill Turner,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Shielded", 3, "Bill Turner,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Shielded", 4, "Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Shielded", 5, "Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Wide Angle Scanner", 1, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Wide Angle Scanner", 2, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Wide Angle Scanner", 3, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Wide Angle Scanner", 4, "Tiana Fortune"),
                    new EngineerList( "Kill Warrant Scanner", "Wide Angle Scanner", 5, "Tiana Fortune"),        //OK

                    new EngineerList( "Life Support", "Lightweight", 1, "Bill Turner,Lori Jameson"),
                    new EngineerList( "Life Support", "Lightweight", 2, "Bill Turner,Lori Jameson"),
                    new EngineerList( "Life Support", "Lightweight", 3, "Bill Turner,Lori Jameson"),
                    new EngineerList( "Life Support", "Lightweight", 4, "Lori Jameson"),
                    new EngineerList( "Life Support", "Lightweight", 5, "Unknown"),
                    new EngineerList( "Life Support", "Reinforced", 1, "Bill Turner,Lori Jameson"),
                    new EngineerList( "Life Support", "Reinforced", 2, "Bill Turner,Lori Jameson"),
                    new EngineerList( "Life Support", "Reinforced", 3, "Bill Turner,Lori Jameson"),
                    new EngineerList( "Life Support", "Reinforced", 4, "Lori Jameson"),
                    new EngineerList( "Life Support", "Reinforced", 5, "Unknown"),
                    new EngineerList( "Life Support", "Shielded", 1, "Bill Turner,Lori Jameson"),
                    new EngineerList( "Life Support", "Shielded", 2, "Bill Turner,Lori Jameson"),
                    new EngineerList( "Life Support", "Shielded", 3, "Bill Turner,Lori Jameson"),
                    new EngineerList( "Life Support", "Shielded", 4, "Lori Jameson"),
                    new EngineerList( "Life Support", "Shielded", 5, "Unknown"),   //OK

                    new EngineerList( "Mine", "High Capacity Magazine", 1, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Mine", "High Capacity Magazine", 2, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Mine", "High Capacity Magazine", 3, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Mine", "High Capacity Magazine", 4, "Juri Ishmaak"),
                    new EngineerList( "Mine", "High Capacity Magazine", 5, "Juri Ishmaak"),
                    new EngineerList( "Mine", "Light Weight Mount", 1, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Mine", "Light Weight Mount", 2, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Mine", "Light Weight Mount", 3, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Mine", "Light Weight Mount", 4, "Juri Ishmaak"),
                    new EngineerList( "Mine", "Light Weight Mount", 5, "Juri Ishmaak"),
                    new EngineerList( "Mine", "Rapid Fire Modification", 1, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Mine", "Rapid Fire Modification", 2, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Mine", "Rapid Fire Modification", 3, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Mine", "Rapid Fire Modification", 4, "Juri Ishmaak"),
                    new EngineerList( "Mine", "Rapid Fire Modification", 5, "Juri Ishmaak"),
                    new EngineerList( "Mine", "Sturdy Mount", 1, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Mine", "Sturdy Mount", 2, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Mine", "Sturdy Mount", 3, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Mine", "Sturdy Mount", 4, "Juri Ishmaak"),
                    new EngineerList( "Mine", "Sturdy Mount", 5, "Juri Ishmaak"),       //OK

                    new EngineerList( "Missile", "High Capacity Magazine", 1, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Missile", "High Capacity Magazine", 2, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Missile", "High Capacity Magazine", 3, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Missile", "High Capacity Magazine", 4, "Liz Ryder"),
                    new EngineerList( "Missile", "High Capacity Magazine", 5, "Liz Ryder"),
                    new EngineerList( "Missile", "Light Weight Mount", 1, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Missile", "Light Weight Mount", 2, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Missile", "Light Weight Mount", 3, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Missile", "Light Weight Mount", 4, "Liz Ryder"),
                    new EngineerList( "Missile", "Light Weight Mount", 5, "Liz Ryder"),
                    new EngineerList( "Missile", "Rapid Fire Modification", 1, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Missile", "Rapid Fire Modification", 2, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Missile", "Rapid Fire Modification", 3, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Missile", "Rapid Fire Modification", 4, "Liz Ryder"),
                    new EngineerList( "Missile", "Rapid Fire Modification", 5, "Liz Ryder"),
                    new EngineerList( "Missile", "Sturdy Mount", 1, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Missile", "Sturdy Mount", 2, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Missile", "Sturdy Mount", 3, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Missile", "Sturdy Mount", 4, "Liz Ryder"),
                    new EngineerList( "Missile", "Sturdy Mount", 5, "Liz Ryder"),   //OK

                    new EngineerList( "Multicannon", "Efficient Weapon", 1, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Multicannon", "Efficient Weapon", 2, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Multicannon", "Efficient Weapon", 3, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Multicannon", "Efficient Weapon", 4, "Tod McQuinn"),
                    new EngineerList( "Multicannon", "Efficient Weapon", 5, "Tod McQuinn"),
                    new EngineerList( "Multicannon", "High Capacity Magazine", 1, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Multicannon", "High Capacity Magazine", 2, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Multicannon", "High Capacity Magazine", 3, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Multicannon", "High Capacity Magazine", 4, "Tod McQuinn"),
                    new EngineerList( "Multicannon", "High Capacity Magazine", 5, "Tod McQuinn"),
                    new EngineerList( "Multicannon", "Light Weight Mount", 1, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Multicannon", "Light Weight Mount", 2, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Multicannon", "Light Weight Mount", 3, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Multicannon", "Light Weight Mount", 4, "Tod McQuinn"),
                    new EngineerList( "Multicannon", "Light Weight Mount", 5, "Tod McQuinn"),
                    new EngineerList( "Multicannon", "Long-Range Weapon", 1, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Multicannon", "Long-Range Weapon", 2, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Multicannon", "Long-Range Weapon", 3, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Multicannon", "Long-Range Weapon", 4, "Tod McQuinn"),
                    new EngineerList( "Multicannon", "Long-Range Weapon", 5, "Tod McQuinn"),
                    new EngineerList( "Multicannon", "Overcharged Weapon", 1, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Multicannon", "Overcharged Weapon", 2, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Multicannon", "Overcharged Weapon", 3, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Multicannon", "Overcharged Weapon", 4, "Tod McQuinn"),
                    new EngineerList( "Multicannon", "Overcharged Weapon", 5, "Tod McQuinn"),
                    new EngineerList( "Multicannon", "Rapid Fire Modification", 1, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Multicannon", "Rapid Fire Modification", 2, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Multicannon", "Rapid Fire Modification", 3, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Multicannon", "Rapid Fire Modification", 4, "Tod McQuinn"),
                    new EngineerList( "Multicannon", "Rapid Fire Modification", 5, "Tod McQuinn"),
                    new EngineerList( "Multicannon", "Short-Range Blaster", 1, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Multicannon", "Short-Range Blaster", 2, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Multicannon", "Short-Range Blaster", 3, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Multicannon", "Short-Range Blaster", 4, "Tod McQuinn"),
                    new EngineerList( "Multicannon", "Short-Range Blaster", 5, "Tod McQuinn"),
                    new EngineerList( "Multicannon", "Sturdy Mount", 1, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Multicannon", "Sturdy Mount", 2, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Multicannon", "Sturdy Mount", 3, "Tod McQuinn,Zacariah Nemo"),
                    new EngineerList( "Multicannon", "Sturdy Mount", 4, "Tod McQuinn"),
                    new EngineerList( "Multicannon", "Sturdy Mount", 5, "Tod McQuinn"),     //OK

                    new EngineerList( "Plasma Accelerator", "Efficient Weapon", 1, "Bill Turner,Zacariah Nemo"),
                    new EngineerList( "Plasma Accelerator", "Efficient Weapon", 2, "Bill Turner,Zacariah Nemo"),
                    new EngineerList( "Plasma Accelerator", "Efficient Weapon", 3, "Bill Turner"),
                    new EngineerList( "Plasma Accelerator", "Efficient Weapon", 4, "Bill Turner"),
                    new EngineerList( "Plasma Accelerator", "Efficient Weapon", 5, "Bill Turner"),
                    new EngineerList( "Plasma Accelerator", "Focused Weapon", 1, "Bill Turner,Zacariah Nemo"),
                    new EngineerList( "Plasma Accelerator", "Focused Weapon", 2, "Bill Turner,Zacariah Nemo"),
                    new EngineerList( "Plasma Accelerator", "Focused Weapon", 3, "Bill Turner"),
                    new EngineerList( "Plasma Accelerator", "Focused Weapon", 4, "Bill Turner"),
                    new EngineerList( "Plasma Accelerator", "Focused Weapon", 5, "Bill Turner"),
                    new EngineerList( "Plasma Accelerator", "Light Weight Mount", 1, "Bill Turner,Zacariah Nemo"),
                    new EngineerList( "Plasma Accelerator", "Light Weight Mount", 2, "Bill Turner,Zacariah Nemo"),
                    new EngineerList( "Plasma Accelerator", "Light Weight Mount", 3, "Bill Turner"),
                    new EngineerList( "Plasma Accelerator", "Light Weight Mount", 4, "Bill Turner"),
                    new EngineerList( "Plasma Accelerator", "Light Weight Mount", 5, "Bill Turner"),
                    new EngineerList( "Plasma Accelerator", "Long-Range Weapon", 1, "Bill Turner,Zacariah Nemo"),
                    new EngineerList( "Plasma Accelerator", "Long-Range Weapon", 2, "Bill Turner,Zacariah Nemo"),
                    new EngineerList( "Plasma Accelerator", "Long-Range Weapon", 3, "Bill Turner"),
                    new EngineerList( "Plasma Accelerator", "Long-Range Weapon", 4, "Bill Turner"),
                    new EngineerList( "Plasma Accelerator", "Long-Range Weapon", 5, "Bill Turner"),
                    new EngineerList( "Plasma Accelerator", "Overcharged Weapon", 1, "Bill Turner,Zacariah Nemo"),
                    new EngineerList( "Plasma Accelerator", "Overcharged Weapon", 2, "Bill Turner,Zacariah Nemo"),
                    new EngineerList( "Plasma Accelerator", "Overcharged Weapon", 3, "Bill Turner"),
                    new EngineerList( "Plasma Accelerator", "Overcharged Weapon", 4, "Bill Turner"),
                    new EngineerList( "Plasma Accelerator", "Overcharged Weapon", 5, "Bill Turner"),
                    new EngineerList( "Plasma Accelerator", "Rapid Fire Modification", 1, "Bill Turner,Zacariah Nemo"),
                    new EngineerList( "Plasma Accelerator", "Rapid Fire Modification", 2, "Bill Turner,Zacariah Nemo"),
                    new EngineerList( "Plasma Accelerator", "Rapid Fire Modification", 3, "Bill Turner"),
                    new EngineerList( "Plasma Accelerator", "Rapid Fire Modification", 4, "Bill Turner"),
                    new EngineerList( "Plasma Accelerator", "Rapid Fire Modification", 5, "Bill Turner"),
                    new EngineerList( "Plasma Accelerator", "Short-Range Blaster", 1, "Bill Turner,Zacariah Nemo"),
                    new EngineerList( "Plasma Accelerator", "Short-Range Blaster", 2, "Bill Turner,Zacariah Nemo"),
                    new EngineerList( "Plasma Accelerator", "Short-Range Blaster", 3, "Bill Turner"),
                    new EngineerList( "Plasma Accelerator", "Short-Range Blaster", 4, "Bill Turner"),
                    new EngineerList( "Plasma Accelerator", "Short-Range Blaster", 5, "Bill Turner"),
                    new EngineerList( "Plasma Accelerator", "Sturdy Mount", 1, "Bill Turner,Zacariah Nemo"),
                    new EngineerList( "Plasma Accelerator", "Sturdy Mount", 2, "Bill Turner,Zacariah Nemo"),
                    new EngineerList( "Plasma Accelerator", "Sturdy Mount", 3, "Bill Turner"),
                    new EngineerList( "Plasma Accelerator", "Sturdy Mount", 4, "Bill Turner"),
                    new EngineerList( "Plasma Accelerator", "Sturdy Mount", 5, "Bill Turner"),      //OK


                    new EngineerList( "Point Defence", "Lightweight", 1, "Ram Tah"),
                    new EngineerList( "Point Defence", "Lightweight", 2, "Ram Tah"),
                    new EngineerList( "Point Defence", "Lightweight", 3, "Ram Tah"),
                    new EngineerList( "Point Defence", "Lightweight", 4, "Ram Tah"),
                    new EngineerList( "Point Defence", "Lightweight", 5, "Ram Tah"),
                    new EngineerList( "Point Defence", "Point Defence Ammo Capacity", 1, "Ram Tah"),
                    new EngineerList( "Point Defence", "Reinforced", 1, "Ram Tah"),
                    new EngineerList( "Point Defence", "Reinforced", 2, "Ram Tah"),
                    new EngineerList( "Point Defence", "Reinforced", 3, "Ram Tah"),
                    new EngineerList( "Point Defence", "Reinforced", 4, "Ram Tah"),
                    new EngineerList( "Point Defence", "Reinforced", 5, "Ram Tah"),
                    new EngineerList( "Point Defence", "Shielded", 1, "Ram Tah"),
                    new EngineerList( "Point Defence", "Shielded", 2, "Ram Tah"),
                    new EngineerList( "Point Defence", "Shielded", 3, "Ram Tah"),
                    new EngineerList( "Point Defence", "Shielded", 4, "Ram Tah"),
                    new EngineerList( "Point Defence", "Shielded", 5, "Ram Tah"),

                    new EngineerList( "Power Distributor", "High Charge Capacity Power Distributor", 1, "Hera Tani,Marco Qwent,The Dweller"),
                    new EngineerList( "Power Distributor", "High Charge Capacity Power Distributor", 2, "Hera Tani,Marco Qwent,The Dweller"),
                    new EngineerList( "Power Distributor", "High Charge Capacity Power Distributor", 3, "Hera Tani,Marco Qwent,The Dweller"),
                    new EngineerList( "Power Distributor", "High Charge Capacity Power Distributor", 4, "The Dweller"),
                    new EngineerList( "Power Distributor", "High Charge Capacity Power Distributor", 5, "The Dweller"),
                    new EngineerList( "Power Distributor", "Charge Enhanced Power Distributor", 1, "Hera Tani,Marco Qwent,The Dweller"),
                    new EngineerList( "Power Distributor", "Charge Enhanced Power Distributor", 2, "Hera Tani,Marco Qwent,The Dweller"),
                    new EngineerList( "Power Distributor", "Charge Enhanced Power Distributor", 3, "Hera Tani,Marco Qwent,The Dweller"),
                    new EngineerList( "Power Distributor", "Charge Enhanced Power Distributor", 4, "The Dweller"),
                    new EngineerList( "Power Distributor", "Charge Enhanced Power Distributor", 5, "The Dweller"),
                    new EngineerList( "Power Distributor", "Engine Focused Power Distributor", 1, "Hera Tani,Marco Qwent,The Dweller"),
                    new EngineerList( "Power Distributor", "Engine Focused Power Distributor", 2, "Hera Tani,Marco Qwent,The Dweller"),
                    new EngineerList( "Power Distributor", "Engine Focused Power Distributor", 3, "Hera Tani,Marco Qwent,The Dweller"),
                    new EngineerList( "Power Distributor", "Engine Focused Power Distributor", 4, "The Dweller"),
                    new EngineerList( "Power Distributor", "Engine Focused Power Distributor", 5, "The Dweller"),
                    new EngineerList( "Power Distributor", "System Focused Power Distributor", 1, "Hera Tani,Marco Qwent,The Dweller"),
                    new EngineerList( "Power Distributor", "System Focused Power Distributor", 2, "Hera Tani,Marco Qwent,The Dweller"),
                    new EngineerList( "Power Distributor", "System Focused Power Distributor", 3, "Hera Tani,Marco Qwent,The Dweller"),
                    new EngineerList( "Power Distributor", "System Focused Power Distributor", 4, "The Dweller"),
                    new EngineerList( "Power Distributor", "System Focused Power Distributor", 5, "The Dweller"),
                    new EngineerList( "Power Distributor", "Weapon Focused Power Distributor", 1, "Hera Tani,Marco Qwent,The Dweller"),
                    new EngineerList( "Power Distributor", "Weapon Focused Power Distributor", 2, "Hera Tani,Marco Qwent,The Dweller"),
                    new EngineerList( "Power Distributor", "Weapon Focused Power Distributor", 3, "Hera Tani,Marco Qwent,The Dweller"),
                    new EngineerList( "Power Distributor", "Weapon Focused Power Distributor", 4, "The Dweller"),
                    new EngineerList( "Power Distributor", "Weapon Focused Power Distributor", 5, "The Dweller"),
                    new EngineerList( "Power Distributor", "Shielded Power Distributor", 1, "Hera Tani,Marco Qwent,The Dweller"),
                    new EngineerList( "Power Distributor", "Shielded Power Distributor", 2, "Hera Tani,Marco Qwent,The Dweller"),
                    new EngineerList( "Power Distributor", "Shielded Power Distributor", 3, "Hera Tani,Marco Qwent,The Dweller"),
                    new EngineerList( "Power Distributor", "Shielded Power Distributor", 4, "The Dweller"),
                    new EngineerList( "Power Distributor", "Shielded Power Distributor", 5, "The Dweller"), //OK

                    new EngineerList( "Power Plant", "Armoured Power Plant", 1, "Felicity Farseer,Hera Tani,Marco Qwent"),
                    new EngineerList( "Power Plant", "Armoured Power Plant", 2, "Hera Tani,Marco Qwent"),
                    new EngineerList( "Power Plant", "Armoured Power Plant", 3, "Hera Tani,Marco Qwent"),
                    new EngineerList( "Power Plant", "Armoured Power Plant", 4, "Hera Tani,Marco Qwent"),
                    new EngineerList( "Power Plant", "Armoured Power Plant", 5, "Hera Tani"),
                    new EngineerList( "Power Plant", "Overcharged Power Plant", 1, "Felicity Farseer,Hera Tani,Marco Qwent"),
                    new EngineerList( "Power Plant", "Overcharged Power Plant", 2, "Hera Tani,Marco Qwent"),
                    new EngineerList( "Power Plant", "Overcharged Power Plant", 3, "Hera Tani,Marco Qwent"),
                    new EngineerList( "Power Plant", "Overcharged Power Plant", 4, "Hera Tani,Marco Qwent"),
                    new EngineerList( "Power Plant", "Overcharged Power Plant", 5, "Hera Tani"),
                    new EngineerList( "Power Plant", "Low Emissions Power Plant", 1, "Felicity Farseer,Hera Tani,Marco Qwent"),
                    new EngineerList( "Power Plant", "Low Emissions Power Plant", 2, "Hera Tani,Marco Qwent"),
                    new EngineerList( "Power Plant", "Low Emissions Power Plant", 3, "Hera Tani,Marco Qwent"),
                    new EngineerList( "Power Plant", "Low Emissions Power Plant", 4, "Hera Tani,Marco Qwent"),
                    new EngineerList( "Power Plant", "Low Emissions Power Plant", 5, "Hera Tani"),  //OK

                    new EngineerList( "Prospecting Limpet", "Lightweight", 1, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Prospecting Limpet", "Lightweight", 2, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Prospecting Limpet", "Lightweight", 3, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Prospecting Limpet", "Lightweight", 4, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Prospecting Limpet", "Lightweight", 5, "The Sarge,Tiana Fortune"),
                    new EngineerList( "Prospecting Limpet", "Reinforced", 1, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Prospecting Limpet", "Reinforced", 2, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Prospecting Limpet", "Reinforced", 3, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Prospecting Limpet", "Reinforced", 4, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Prospecting Limpet", "Reinforced", 5, "The Sarge,Tiana Fortune"),
                    new EngineerList( "Prospecting Limpet", "Shielded", 1, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Prospecting Limpet", "Shielded", 2, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Prospecting Limpet", "Shielded", 3, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Prospecting Limpet", "Shielded", 4, "Ram Tah,The Sarge,Tiana Fortune"),
                    new EngineerList( "Prospecting Limpet", "Shielded", 5, "The Sarge,Tiana Fortune"), //OK

                    new EngineerList( "Pulse Laser", "Efficient Weapon", 1, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Efficient Weapon", 2, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Efficient Weapon", 3, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Efficient Weapon", 4, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Efficient Weapon", 5, "Broo Tarquin"),
                    new EngineerList( "Pulse Laser", "Focused Weapon", 1, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Focused Weapon", 2, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Focused Weapon", 3, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Focused Weapon", 4, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Focused Weapon", 5, "Broo Tarquin"),
                    new EngineerList( "Pulse Laser", "Light Weight Mount", 1, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Light Weight Mount", 2, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Light Weight Mount", 3, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Light Weight Mount", 4, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Light Weight Mount", 5, "Broo Tarquin"),
                    new EngineerList( "Pulse Laser", "Long-Range Weapon", 1, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Long-Range Weapon", 2, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Long-Range Weapon", 3, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Long-Range Weapon", 4, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Long-Range Weapon", 5, "Broo Tarquin"),
                    new EngineerList( "Pulse Laser", "Overcharged Weapon", 1, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Overcharged Weapon", 2, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Overcharged Weapon", 3, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Overcharged Weapon", 4, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Overcharged Weapon", 5, "Broo Tarquin"),
                    new EngineerList( "Pulse Laser", "Rapid Fire Modification", 1, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Rapid Fire Modification", 2, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Rapid Fire Modification", 3, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Rapid Fire Modification", 4, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Rapid Fire Modification", 5, "Broo Tarquin"),
                    new EngineerList( "Pulse Laser", "Short-Range Blaster", 1, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Short-Range Blaster", 2, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Short-Range Blaster", 3, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Short-Range Blaster", 4, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Short-Range Blaster", 5, "Broo Tarquin"),
                    new EngineerList( "Pulse Laser", "Sturdy Mount", 1, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Sturdy Mount", 2, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Sturdy Mount", 3, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Sturdy Mount", 4, "Broo Tarquin,The Dweller"),
                    new EngineerList( "Pulse Laser", "Sturdy Mount", 5, "Broo Tarquin"),    //OK

                    new EngineerList( "Rail Gun", "High Capacity Magazine", 1, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Rail Gun", "High Capacity Magazine", 2, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Rail Gun", "High Capacity Magazine", 3, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Rail Gun", "High Capacity Magazine", 4, "Tod McQuinn"),
                    new EngineerList( "Rail Gun", "High Capacity Magazine", 5, "Tod McQuinn"),
                    new EngineerList( "Rail Gun", "Light Weight Mount", 1, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Rail Gun", "Light Weight Mount", 2, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Rail Gun", "Light Weight Mount", 3, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Rail Gun", "Light Weight Mount", 4, "Tod McQuinn"),
                    new EngineerList( "Rail Gun", "Light Weight Mount", 5, "Tod McQuinn"),
                    new EngineerList( "Rail Gun", "Long-Range Weapon", 1, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Rail Gun", "Long-Range Weapon", 2, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Rail Gun", "Long-Range Weapon", 3, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Rail Gun", "Long-Range Weapon", 4, "Tod McQuinn"),
                    new EngineerList( "Rail Gun", "Long-Range Weapon", 5, "Tod McQuinn"),
                    new EngineerList( "Rail Gun", "Short-Range Blaster", 1, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Rail Gun", "Short-Range Blaster", 2, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Rail Gun", "Short-Range Blaster", 3, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Rail Gun", "Short-Range Blaster", 4, "Tod McQuinn"),
                    new EngineerList( "Rail Gun", "Short-Range Blaster", 5, "Tod McQuinn"),
                    new EngineerList( "Rail Gun", "Sturdy Mount", 1, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Rail Gun", "Sturdy Mount", 2, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Rail Gun", "Sturdy Mount", 3, "The Sarge,Tod McQuinn"),
                    new EngineerList( "Rail Gun", "Sturdy Mount", 4, "Tod McQuinn"),
                    new EngineerList( "Rail Gun", "Sturdy Mount", 5, "Tod McQuinn"),  //OK


                    new EngineerList( "Refineries", "Shielded", 1, "Bill Turner,Lori Jameson"),
                    new EngineerList( "Refineries", "Shielded", 2, "Bill Turner,Lori Jameson"),
                    new EngineerList( "Refineries", "Shielded", 3, "Bill Turner,Lori Jameson"),
                    new EngineerList( "Refineries", "Shielded", 4, "Lori Jameson"),
                    new EngineerList( "Refineries", "Shielded", 5, "Unknown"), //ok





                    new EngineerList( "Sensor", "Light Weight Scanner", 1, "Felicity Farseer,Lei Cheung,Hera Tani,Juri Ishmaak,Tiana Fortune,Bill Turner,Lori Jameson"),
                    new EngineerList( "Sensor", "Light Weight Scanner", 2, "Felicity Farseer,Lei Cheung,Hera Tani,Juri Ishmaak,Tiana Fortune,Bill Turner,Lori Jameson"),
                    new EngineerList( "Sensor", "Light Weight Scanner", 3, "Felicity Farseer,Lei Cheung,Hera Tani,Juri Ishmaak,Tiana Fortune,Bill Turner,Lori Jameson"),
                    new EngineerList( "Sensor", "Light Weight Scanner", 4, "Lei Cheung,Juri Ishmaak,Tiana Fortune,Bill Turner,Lori Jameson"),
                    new EngineerList( "Sensor", "Light Weight Scanner", 5, "Lei Cheung,Juri Ishmaak,Tiana Fortune,Bill Turner,Lori Jameson"),   //ok

                    new EngineerList( "Sensor", "Long-Range Scanner", 1, "Felicity Farseer,Lei Cheung,Hera Tani,Juri Ishmaak,Tiana Fortune,Bill Turner,Lori Jameson"),
                    new EngineerList( "Sensor", "Long-Range Scanner", 2, "Felicity Farseer,Lei Cheung,Hera Tani,Juri Ishmaak,Tiana Fortune,Bill Turner,Lori Jameson"),
                    new EngineerList( "Sensor", "Long-Range Scanner", 3, "Felicity Farseer,Lei Cheung,Hera Tani,Juri Ishmaak,Tiana Fortune,Bill Turner,Lori Jameson"),
                    new EngineerList( "Sensor", "Long-Range Scanner", 4, "Lei Cheung,Juri Ishmaak,Tiana Fortune,Bill Turner,Lori Jameson"),   //ok
                    new EngineerList( "Sensor", "Long-Range Scanner", 5, "Lei Cheung,Juri Ishmaak,Tiana Fortune,Bill Turner,Lori Jameson"),   //ok
                    new EngineerList( "Sensor", "Wide Angle Scanner", 1, "Felicity Farseer,Lei Cheung,Hera Tani,Juri Ishmaak,Tiana Fortune,Bill Turner,Lori Jameson"),
                    new EngineerList( "Sensor", "Wide Angle Scanner", 2, "Felicity Farseer,Lei Cheung,Hera Tani,Juri Ishmaak,Tiana Fortune,Bill Turner,Lori Jameson"),
                    new EngineerList( "Sensor", "Wide Angle Scanner", 3, "Felicity Farseer,Lei Cheung,Hera Tani,Juri Ishmaak,Tiana Fortune,Bill Turner,Lori Jameson"),
                    new EngineerList( "Sensor", "Wide Angle Scanner", 4, "Lei Cheung,Juri Ishmaak,Tiana Fortune,Bill Turner,Lori Jameson"),   //ok
                    new EngineerList( "Sensor", "Wide Angle Scanner", 5, "Lei Cheung,Juri Ishmaak,Tiana Fortune,Bill Turner,Lori Jameson"),   //ok


                    new EngineerList( "Shield Booster", "Blast Resistant Shield Booster", 1, "Didi Vatermann,Felicity Farseer,Lei Cheung"),
                    new EngineerList( "Shield Booster", "Blast Resistant Shield Booster", 2, "Didi Vatermann,Lei Cheung"),
                    new EngineerList( "Shield Booster", "Blast Resistant Shield Booster", 3, "Didi Vatermann,Lei Cheung"),
                    new EngineerList( "Shield Booster", "Blast Resistant Shield Booster", 4, "Didi Vatermann"),
                    new EngineerList( "Shield Booster", "Blast Resistant Shield Booster", 5, "Didi Vatermann"),
                    new EngineerList( "Shield Booster", "Heavy Duty Shield Booster", 1, "Didi Vatermann,Felicity Farseer,Lei Cheung"),
                    new EngineerList( "Shield Booster", "Heavy Duty Shield Booster", 2, "Didi Vatermann,Lei Cheung"),
                    new EngineerList( "Shield Booster", "Heavy Duty Shield Booster", 3, "Didi Vatermann,Lei Cheung"),
                    new EngineerList( "Shield Booster", "Heavy Duty Shield Booster", 4, "Didi Vatermann"),
                    new EngineerList( "Shield Booster", "Heavy Duty Shield Booster", 5, "Didi Vatermann"),
                    new EngineerList( "Shield Booster", "Kinetic Resistant Shield Booster", 1, "Didi Vatermann,Felicity Farseer,Lei Cheung"),
                    new EngineerList( "Shield Booster", "Kinetic Resistant Shield Booster", 2, "Didi Vatermann,Lei Cheung"),
                    new EngineerList( "Shield Booster", "Kinetic Resistant Shield Booster", 3, "Didi Vatermann,Lei Cheung"),
                    new EngineerList( "Shield Booster", "Kinetic Resistant Shield Booster", 4, "Didi Vatermann"),
                    new EngineerList( "Shield Booster", "Kinetic Resistant Shield Booster", 5, "Didi Vatermann"),
                    new EngineerList( "Shield Booster", "Resistance Augmented Shield Booster", 1, "Didi Vatermann,Felicity Farseer,Lei Cheung"),
                    new EngineerList( "Shield Booster", "Resistance Augmented Shield Booster", 2, "Didi Vatermann,Lei Cheung"),
                    new EngineerList( "Shield Booster", "Resistance Augmented Shield Booster", 3, "Didi Vatermann,Lei Cheung"),
                    new EngineerList( "Shield Booster", "Resistance Augmented Shield Booster", 4, "Didi Vatermann"),
                    new EngineerList( "Shield Booster", "Resistance Augmented Shield Booster", 5, "Didi Vatermann"),
                    new EngineerList( "Shield Booster", "Thermal Resistant Shield Booster", 1, "Didi Vatermann,Felicity Farseer,Lei Cheung"),
                    new EngineerList( "Shield Booster", "Thermal Resistant Shield Booster", 2, "Didi Vatermann,Lei Cheung"),
                    new EngineerList( "Shield Booster", "Thermal Resistant Shield Booster", 3, "Didi Vatermann,Lei Cheung"),
                    new EngineerList( "Shield Booster", "Thermal Resistant Shield Booster", 4, "Didi Vatermann"),
                    new EngineerList( "Shield Booster", "Thermal Resistant Shield Booster", 5, "Didi Vatermann"),       //ok

                    new EngineerList( "Shield Cell Bank", "Rapid Charge Shield Cell Bank", 1, "Elvira Martuuk,Lori Jameson"),
                    new EngineerList( "Shield Cell Bank", "Rapid Charge Shield Cell Bank", 2, "Lori Jameson"),
                    new EngineerList( "Shield Cell Bank", "Rapid Charge Shield Cell Bank", 3, "Lori Jameson"),
                    new EngineerList( "Shield Cell Bank", "Rapid Charge Shield Cell Bank", 4, "Unknown"),
                    new EngineerList( "Shield Cell Bank", "Specialised Shield Cell Bank", 1, "Elvira Martuuk,Lori Jameson"),
                    new EngineerList( "Shield Cell Bank", "Specialised Shield Cell Bank", 2, "Lori Jameson"),
                    new EngineerList( "Shield Cell Bank", "Specialised Shield Cell Bank", 3, "Lori Jameson"),
                    new EngineerList( "Shield Cell Bank", "Specialised Shield Cell Bank", 4, "Unknown"),    //ok


                    new EngineerList( "Shield Generator", "Kinetic Resistant Shields", 1, "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
                    new EngineerList( "Shield Generator", "Kinetic Resistant Shields", 2, "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
                    new EngineerList( "Shield Generator", "Kinetic Resistant Shields", 3, "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
                    new EngineerList( "Shield Generator", "Kinetic Resistant Shields", 4, "Lei Cheung"),
                    new EngineerList( "Shield Generator", "Kinetic Resistant Shields", 5, "Lei Cheung"),
                    new EngineerList( "Shield Generator", "Enhanced, Low Power Shields", 1, "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
                    new EngineerList( "Shield Generator", "Enhanced, Low Power Shields", 2, "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
                    new EngineerList( "Shield Generator", "Enhanced, Low Power Shields", 3, "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
                    new EngineerList( "Shield Generator", "Enhanced, Low Power Shields", 4, "Lei Cheung"),
                    new EngineerList( "Shield Generator", "Enhanced, Low Power Shields", 5, "Lei Cheung"),
                    new EngineerList( "Shield Generator", "Reinforced Shields", 1, "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
                    new EngineerList( "Shield Generator", "Reinforced Shields", 2, "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
                    new EngineerList( "Shield Generator", "Reinforced Shields", 3, "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
                    new EngineerList( "Shield Generator", "Reinforced Shields", 4, "Lei Cheung"),
                    new EngineerList( "Shield Generator", "Reinforced Shields", 5, "Lei Cheung"),
                    new EngineerList( "Shield Generator", "Thermal Resistant Shields", 1, "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
                    new EngineerList( "Shield Generator", "Thermal Resistant Shields", 2, "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
                    new EngineerList( "Shield Generator", "Thermal Resistant Shields", 3, "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
                    new EngineerList( "Shield Generator", "Thermal Resistant Shields", 4, "Lei Cheung"),
                    new EngineerList( "Shield Generator", "Thermal Resistant Shields", 5, "Lei Cheung"),    //ok

                    new EngineerList( "Surface Scanner", "Fast Scanner", 1, "Felicity Farseer,Lei Cheung,Hera Tani,Juri Ishmaak,Tiana Fortune,Bill Turner,Lori Jameson"),
                    new EngineerList( "Surface Scanner", "Fast Scanner", 2, "Felicity Farseer,Lei Cheung,Hera Tani,Juri Ishmaak,Tiana Fortune,Bill Turner,Lori Jameson"),
                    new EngineerList( "Surface Scanner", "Fast Scanner", 3, "Felicity Farseer,Lei Cheung,Hera Tani,Juri Ishmaak,Tiana Fortune,Bill Turner,Lori Jameson"),
                    new EngineerList( "Surface Scanner", "Fast Scanner", 4, "Lei Cheung,Hera Tani,Juri Ishmaak,Bill Turner,Lori Jameson"),
                    new EngineerList( "Surface Scanner", "Fast Scanner", 5,"Lei Cheung,Hera Tani,Juri Ishmaak,Bill Turner,Lori Jameson"),
                    new EngineerList( "Surface Scanner", "Long-Range Scanner", 1, "Felicity Farseer,Lei Cheung,Hera Tani,Juri Ishmaak,Tiana Fortune,Bill Turner,Lori Jameson"),
                    new EngineerList( "Surface Scanner", "Long-Range Scanner", 2, "Felicity Farseer,Lei Cheung,Hera Tani,Juri Ishmaak,Tiana Fortune,Bill Turner,Lori Jameson"),
                    new EngineerList( "Surface Scanner", "Long-Range Scanner", 3, "Felicity Farseer,Lei Cheung,Hera Tani,Juri Ishmaak,Tiana Fortune,Bill Turner,Lori Jameson"),
                    new EngineerList( "Surface Scanner", "Long-Range Scanner", 4, "Lei Cheung,Hera Tani,Juri Ishmaak,Bill Turner,Lori Jameson"),
                    new EngineerList( "Surface Scanner", "Long-Range Scanner", 5, "Lei Cheung,Hera Tani,Juri Ishmaak,Bill Turner,Lori Jameson"),
                    new EngineerList( "Surface Scanner", "Wide Angle Scanner", 1, "Felicity Farseer,Lei Cheung,Hera Tani,Juri Ishmaak,Tiana Fortune,Bill Turner,Lori Jameson"),
                    new EngineerList( "Surface Scanner", "Wide Angle Scanner", 2, "Felicity Farseer,Lei Cheung,Hera Tani,Juri Ishmaak,Tiana Fortune,Bill Turner,Lori Jameson"),
                    new EngineerList( "Surface Scanner", "Wide Angle Scanner", 3, "Felicity Farseer,Lei Cheung,Hera Tani,Juri Ishmaak,Tiana Fortune,Bill Turner,Lori Jameson"),
                    new EngineerList( "Surface Scanner", "Wide Angle Scanner", 4, "Lei Cheung,Hera Tani,Juri Ishmaak,Bill Turner,Lori Jameson"),
                    new EngineerList( "Surface Scanner", "Wide Angle Scanner", 5, "Lei Cheung,Hera Tani,Juri Ishmaak,Bill Turner,Lori Jameson"),        //OK

                    new EngineerList( "Torpedo", "Light Weight Mount", 1, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Torpedo", "Light Weight Mount", 2, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Torpedo", "Light Weight Mount", 3, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Torpedo", "Light Weight Mount", 4, "Liz Ryder"),
                    new EngineerList( "Torpedo", "Light Weight Mount", 5, "Liz Ryder"),
                    new EngineerList( "Torpedo", "Sturdy Mount", 1, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Torpedo", "Sturdy Mount", 2, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Torpedo", "Sturdy Mount", 3, "Juri Ishmaak,Liz Ryder"),
                    new EngineerList( "Torpedo", "Sturdy Mount", 4, "Liz Ryder"),
                    new EngineerList( "Torpedo", "Sturdy Mount", 5, "Liz Ryder"),   //OK

                    new EngineerList( "Wake Scanner", "Fast Scanner", 1, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Fast Scanner", 2, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Fast Scanner", 3, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Fast Scanner", 4, "Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Fast Scanner", 5, "Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Lightweight", 1, "Bill Turner,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Lightweight", 2, "Bill Turner,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Lightweight", 3, "Bill Turner,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Lightweight", 4, "Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Lightweight", 5, "Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Long-Range Scanner", 1, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Long-Range Scanner", 2, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Long-Range Scanner", 3, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Long-Range Scanner", 4, "Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Long-Range Scanner", 5, "Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Reinforced", 1, "Bill Turner,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Reinforced", 2, "Bill Turner,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Reinforced", 3, "Bill Turner,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Reinforced", 4, "Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Reinforced", 5, "Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Shielded", 1, "Bill Turner,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Shielded", 2, "Bill Turner,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Shielded", 3, "Bill Turner,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Shielded", 4, "Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Shielded", 5, "Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Wide Angle Scanner", 1, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Wide Angle Scanner", 2, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Wide Angle Scanner", 3, "Bill Turner,Juri Ishmaak,Lori Jameson,Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Wide Angle Scanner", 4, "Tiana Fortune"),
                    new EngineerList( "Wake Scanner", "Wide Angle Scanner", 5, "Tiana Fortune"),    //OK

        };

        static string[] InaraRares = new string[]
        {
            // inara 2/7/2018

            "Aepyornis Egg",
            "Aganippe Rush",
            "Alacarakmo Skin Art",
            "Albino Quechua Mammoth Meat",
            "Altairian Skin",
            "Alya Body Soap",
            "Anduliga Fire Works",
            "Any Na Coffee",
            "Arouca Conventual Sweets",
            "AZ Cancri Formula 42",
            "Azure Milk",
            "Baltah'sine Vacuum Krill",
            "Banki Amphibious Leather",
            "Bast Snake Gin",
            "Belalans Ray Leather",
            "Borasetani Pathogenetics",
            "Buckyball Beer Mats",
            "Burnham Bile Distillate",
            "CD-75 Kitten Brand Coffee",
            "Centauri Mega Gin",
            "Ceremonial Heike Tea",
            "Ceti Rabbits",
            "Chameleon Cloth",
            "Chateau de Aegaeon",
            "Cherbones Blood Crystals",
            "Chi Eridani Marine Paste",
            "Coquim Spongiform Victuals",
            "Crom Silver Fesh",
            "Crystalline Spheres",
            "Damna Carapaces",
            "Delta Phoenicis Palms",
            "Deuringas Truffles",
            "Diso Ma Corn",
            "Eden Apples Of Aerial",
            "Eleu Thermals",
            "Eranin Pearl Whisky",
            "Eshu Umbrellas",
            "Esuseku Caviar",
            "Ethgreze Tea Buds",
            "Fujin Tea",
            "Galactic Travel Guide",
            "Geawen Dance Dust",
            "Gerasian Gueuze Beer",
            "Giant Irukama Snails",
            "Giant Verrix",
            "Gilya Signature Weapons",
            "Goman Yaupon Coffee",
            "Haiden Black Brew",
            "Havasupai Dream Catcher",
            "Helvetitj Pearls",
            "HIP 10175 Bush Meat",
            "HIP 118311 Swarm",
            "HIP Organophosphates",
            "HIP Proto-Squid",
            "Holva Duelling Blades",
            "Honesty Pills",
            "HR 7221 Wheat",
            "Indi Bourbon",
            "Jaques Quinentian Still",
            "Jaradharre Puzzle Box",
            "Jaroua Rice",
            "Jotun Mookah",
            "Kachirigin Filter Leeches",
            "Kamitra Cigars",
            "Kamorin Historic Weapons",
            "Karetii Couture",
            "Karsuki Locusts",
            "Kinago Violins",
            "Kongga Ale",
            "Koro Kung Pellets",
            "Lavian Brandy",
            "Leathery Eggs",
            "Leestian Evil Juice",
            "Live Hecate Sea Worms",
            "LTT Hyper Sweet",
            "Lucan Onionhead",
            "Master Chefs",
            "Mechucos High Tea",
            "Medb Starlube",
            "Mokojing Beast Feast",
            "Momus Bog Spaniel",
            "Motrona Experience Jelly",
            "Mukusubii Chitin-os",
            "Mulachi Giant Fungus",
            "Neritus Berries",
            "Ngadandari Fire Opals",
            "Nguna Modern Antiques",
            "Njangari Saddles",
            "Non Euclidian Exotanks",
            "Ochoeng Chillies",
            "Onionhead",
            "Onionhead Alpha Strain",
            "Onionhead Beta Strain",
            "Ophiuch Exino Artefacts",
            "Orrerian Vicious Brew",
            "Pantaa Prayer Sticks",
            "Pavonis Ear Grubs",
            "Personal Gifts",
            "Rajukru Multi-Stoves",
            "Rapa Bao Snake Skins",
            "Rusani Old Smokey",
            "Sanuma Decorative Meat",
            "Saxon Wine",
            "Shan's Charis Orchid",
            "Soontill Relics",
            "Sothis Crystalline Gold",
            "Tanmark Tranquil Tea",
            "Tarach Spice",
            "Tauri Chimes",
            "Terra Mater Blood Bores",
            "The Hutton Mug",
            "The Waters of Shintara",
            "Thrutis Cream",
            "Tiegfries Synth Silk",
            "Tiolce Waste2Paste Units",
            "Toxandji Virocide",
            "Ultra-Compact Processor Prototypes",
            "Uszaian Tree Grub",
            "Utgaroar Millennial Eggs",
            "Uzumoku Low-G Wings",
            "V Herculis Body Rub",
            "Vanayequi Ceratomorpha Fur",
            "Vega Slimweed",
            "Vidavantian Lace",
            "Void Extract Coffee",
            "Volkhab Bee Drones",
            "Wheemete Wheat Cakes",
            "Witchhaul Kobe Beef",
            "Wolf Fesh",
            "Wulpa Hyperbore Systems",
            "Wuthielo Ku Froth",
            "Xihe Biomorphic Companions",
            "Yaso Kondi Leaf",
            "Zeessze Ant Grub Glue",

            // added from frontier data

            "Lyrae Weed",
            "Chateau De Aegaeon",
            "The Waters Of Shintara",
            "Baked Greebles",
            "Hip Organophosphates",
            "Harma Silver Sea Rum",
            "Earth Relics",


        };

    }
}

