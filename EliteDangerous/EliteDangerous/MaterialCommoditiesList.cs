/*
 * Copyright © 2015 - 2016 EDDiscovery development team
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
using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace EliteDangerousCore
{
    [System.Diagnostics.DebuggerDisplay("Mat {name} count {count} left {scratchpad}")]
    public class MaterialCommodities               // in memory version of it
    {
        public int count { get; set; }
        public double price { get; set; }
        public MaterialCommodityDB Details { get; set; }

        public MaterialCommodities(MaterialCommodityDB c)
        {
            count = scratchpad = 0;
            price = 0;
            this.Details = c;
        }

        public MaterialCommodities(MaterialCommodities c)
        {
            count = c.count;        // clone these
            price = c.price;
            this.Details = c.Details;       // can copy this, its fixed
        }

        public string category { get { return Details.category; } }
        public string name { get { return Details.name; } }
        public string fdname { get { return Details.fdname; } }
        public string type { get { return Details.type; } }
        public string shortname { get { return Details.shortname; } }
        public Color colour { get { return Details.colour; } }

        #region Static properties and methods linking to MaterialCommodity
        public static string CommodityCategory { get { return MaterialCommodityDB.CommodityCategory; } }
        public static string MaterialRawCategory { get { return MaterialCommodityDB.MaterialRawCategory; } }
        public static string MaterialEncodedCategory { get { return MaterialCommodityDB.MaterialEncodedCategory; } }
        public static string MaterialManufacturedCategory { get { return MaterialCommodityDB.MaterialManufacturedCategory; } }
        #endregion

        public int scratchpad { get; set; }        // for synthesis dialog..
    }


    public class MaterialCommoditiesList
    {
        private List<MaterialCommodities> list;

        public MaterialCommoditiesList()
        {
            list = new List<MaterialCommodities>();
        }

        public bool ContainsRares() // function on purpose
        {
            return list.FindIndex(x => x.type.Equals(MaterialCommodityDB.CommodityTypeRareGoods) && x.count > 0) != -1;
        }

        public MaterialCommoditiesList Clone(bool clearzeromaterials, bool clearzerocommodities)       // returns a new copy of this class.. all items a copy
        {
            MaterialCommoditiesList mcl = new MaterialCommoditiesList();

            mcl.list = new List<MaterialCommodities>(list.Count);
            list.ForEach(item =>
            {
                bool commodity = item.category.Equals(MaterialCommodities.CommodityCategory);
                // if items, or commodity and not clear zero, or material and not clear zero, add
                if (item.count > 0 || (commodity && !clearzerocommodities) || (!commodity && !clearzeromaterials))
                    mcl.list.Add(item);
            });

            return mcl;
        }

        public List<MaterialCommodities> Sort(bool commodity)
        {
            List<MaterialCommodities> ret = new List<MaterialCommodities>();

            if (commodity)
                ret = list.Where(x => x.category.Equals(MaterialCommodities.CommodityCategory)).OrderBy(x => x.type)
                           .ThenBy(x => x.name).ToList();
            else
                ret = list.Where(x => !x.category.Equals(MaterialCommodities.CommodityCategory)).OrderBy(x => x.name).ToList();

            return ret;
        }

        public int Count(string [] cats)    // for all types of cat, if item matches or does not, count
        {
            int total = 0;
            foreach (MaterialCommodities c in list)
            {
                if ( Array.IndexOf<string>(cats, c.category) != -1 )
                    total += c.count;
            }

            return total;
        }

        public int DataCount { get { return Count(new string[] { MaterialCommodities.MaterialEncodedCategory }); } }
        public int MaterialsCount { get { return Count(new string[] { MaterialCommodities.MaterialRawCategory, MaterialCommodities.MaterialManufacturedCategory }); } }
        public int CargoCount { get { return Count(new string[] { MaterialCommodities.CommodityCategory }); } }

        public int DataHash() { return list.GetHashCode(); }

        void Dump()
        {
            System.Diagnostics.Debug.Write(list.GetHashCode() + " ");
            foreach ( MaterialCommodities m in list )
            {
                System.Diagnostics.Debug.Write( "{" + m.GetHashCode() + " " + m.category + " " + m.fdname + " " + m.count + "}");
            }
            System.Diagnostics.Debug.WriteLine("");
        }

        // ifnorecatonsearch is used if you don't know if its a material or commodity.. for future use.

        private MaterialCommodities GetNewCopyOf(string cat, string fdname, SQLiteConnectionUser conn, bool ignorecatonsearch = false)
        {
            int index = list.FindIndex(x => x.fdname.Equals(fdname, StringComparison.InvariantCultureIgnoreCase) && (ignorecatonsearch || x.category.Equals(cat, StringComparison.InvariantCultureIgnoreCase)));

            if (index >= 0)
            {
                list[index] = new MaterialCommodities(list[index]);    // fresh copy..
                return list[index];
            }
            else
            {
                MaterialCommodityDB mcdb = MaterialCommodityDB.EnsurePresent(cat,fdname, conn);    // get a MCDB of this
                MaterialCommodities mc = new MaterialCommodities(mcdb);        // make a new entry
                list.Add(mc);
                return mc;
            }
        }

        // ignore cat is only used if you don't know what it is 
        public void Change(string cat, string fdname, int num, long price, SQLiteConnectionUser conn, bool ignorecatonsearch = false)
        {
            MaterialCommodities mc = GetNewCopyOf(cat, fdname, conn, ignorecatonsearch);

            double costprev = mc.count * mc.price;
            double costnew = num * price;
            mc.count = Math.Max(mc.count + num, 0);

            if (mc.count > 0 && num > 0)      // if bought (defensive with mc.count)
                mc.price = (costprev + costnew) / mc.count;       // price is now a combination of the current cost and the new cost. in case we buy in tranches

            //System.Diagnostics.Debug.WriteLine("Mat:" + cat + " " + fdname + " " + num + " " + mc.count);
        }

        public void Craft(string fdname, int num)
        {
            int index = list.FindIndex(x => x.fdname.Equals(fdname, StringComparison.InvariantCultureIgnoreCase));

            if (index >= 0)
            {
                MaterialCommodities mc = new MaterialCommodities(list[index]);      // new clone of
                list[index] = mc;       // replace ours with new one
                mc.count = Math.Max(mc.count - num, 0);
                //System.Diagnostics.Debug.WriteLine("craft:" + fdname + " " + num + " " + mc.count);
            }
        }

        public void Died()
        {
            list.RemoveAll(x => x.category.Equals(MaterialCommodities.CommodityCategory));      // empty the list of all commodities
        }

        public void Set(string cat, string fdname, int num, double price, SQLiteConnectionUser conn, bool ignorecatonsearch = false)
        {
            MaterialCommodities mc = GetNewCopyOf(cat, fdname, conn, ignorecatonsearch);

            mc.count = num;
            if (price > 0)
                mc.price = price;

            //System.Diagnostics.Debug.WriteLine("Set:" + cat + " " + fdname + " " + num + " " + mc.count);
        }

        public void Clear(bool commodity)
        {
            for (int i = 0; i < list.Count; i++)
            {
                MaterialCommodities mc = list[i];
                if (commodity == (mc.category == MaterialCommodities.CommodityCategory))
                {
                    list[i] = new MaterialCommodities(list[i]);     // new clone of it we can change..
                    list[i].count = 0;  // and clear it
                    //System.Diagnostics.Debug.WriteLine("Clear:" + mc.fdname);
                }
            }
        }

        static public MaterialCommoditiesList Process(JournalEntry je, MaterialCommoditiesList oldml, SQLiteConnectionUser conn,
                                                        bool clearzeromaterials, bool clearzerocommodities)
        {
            MaterialCommoditiesList newmc = (oldml == null) ? new MaterialCommoditiesList() : oldml;

            if (je is IMaterialCommodityJournalEntry)
            {
                IMaterialCommodityJournalEntry e = je as IMaterialCommodityJournalEntry;
                newmc = newmc.Clone(clearzeromaterials, clearzerocommodities);          // so we need a new one, makes a new list, but copies the items..
                e.MaterialList(newmc, conn);
                // newmc.Dump();    // debug
            }

            return newmc;
        }


        #region Synthesis

        public class Recipe
        {
            public string name;
            public string ingredientsstring;
            public string[] ingredients;
            public int[] count;

            public Recipe(string n, string indg)
            {
                name = n;
                ingredientsstring = indg;
                string[] ilist = indg.Split(',');
                ingredients = new string[ilist.Length];
                count = new int[ilist.Length];
                for (int i = 0; i < ilist.Length; i++)
                {
                    //Thanks to 10Fe and 10 Ni to synthesise a limpet we can no longer assume the first character is a number and the rest is the material
                    //Maybe worth changing the class to pass ingredients and numbers as separate lists but not today...
                    string s = new string(ilist[i].TakeWhile(c => !Char.IsLetter(c)).ToArray());
                    ingredients[i] = ilist[i].Substring(s.Length);
                    count[i] = int.Parse(s);
                }
            }
        }

        public class SynthesisRecipe : Recipe
        {
            public string level;

            public SynthesisRecipe(string n, string l, string indg)
                : base(n, indg)
            {
                level = l;
            }
        }

        public class EngineeringRecipe : Recipe
        {
            public string level;
            public string modulesstring;
            public string[] modules;
            public string engineersstring;
            public string[] engineers;

            public EngineeringRecipe(string n, string indg, string mod, string lvl, string engnrs)
                : base(n, indg)
            {
                level = lvl;
                modulesstring = mod;
                modules = mod.Split(',');
                engineersstring = engnrs;
                engineers = engnrs.Split(',');
            }
        }

        public class TechBrokerUnlockRecipe : Recipe
        {
            public TechBrokerUnlockRecipe(string n, string indg)
                : base(n, indg)
            { }
        }

        static public void ResetUsed(List<MaterialCommodities> mcl)
        {
            for (int i = 0; i < mcl.Count; i++)
                mcl[i].scratchpad = mcl[i].count;
        }
        
        //return maximum can make, how many made, needed string.
        static public Tuple<int, int, string> HowManyLeft(List<MaterialCommodities> list, Recipe r, int tomake = 0 )
        {
            int max = int.MaxValue;
            StringBuilder needed = new StringBuilder(64);

            for (int i = 0; i < r.ingredients.Length; i++)
            {
                string ingredient = r.ingredients[i];

                int mi = list.FindIndex(x => x.shortname.Equals(ingredient));
                int got = (mi >= 0) ? list[mi].scratchpad : 0;
                int sets = got / r.count[i];

                max = Math.Min(max, sets);

                int need = r.count[i] * tomake;

                if (got < need )
                {
                    string dispName;
                    if (mi > 0)
                    { dispName = (list[mi].category == MaterialCommodityDB.MaterialEncodedCategory || list[mi].category == MaterialCommodityDB.MaterialManufacturedCategory) ? " " + list[mi].name : list[mi].shortname; }
                    else
                    {
                        MaterialCommodityDB db = MaterialCommodityDB.GetCachedMaterialByShortName(ingredient);
                        dispName = (db.category == MaterialCommodityDB.MaterialEncodedCategory || db.category == MaterialCommodityDB.MaterialManufacturedCategory) ? " " + db.name : db.shortname;
                    }
                    string s = (need - got).ToStringInvariant() + dispName;
                    if (needed.Length == 0)
                        needed.Append("Need:" + s);
                    else
                        needed.Append("," + s);
                }
            }

            int made = 0;

            if (max > 0 && tomake > 0)             // if we have a set, and use it up
            {
                made = Math.Min(max, tomake);                // can only make this much
                StringBuilder usedstr = new StringBuilder(64);

                for (int i = 0; i < r.ingredients.Length; i++)
                {
                    int mi = list.FindIndex(x => x.shortname.Equals(r.ingredients[i]));
                    System.Diagnostics.Debug.Assert(mi != -1);
                    int used = r.count[i] * made;
                    list[mi].scratchpad -= used;
                    string dispName = (list[mi].category == MaterialCommodityDB.MaterialEncodedCategory || list[mi].category == MaterialCommodityDB.MaterialManufacturedCategory) ? " " + list[mi].name : list[mi].shortname;
                    usedstr.AppendPrePad(used.ToStringInvariant() + dispName, ",");
                }

                needed.AppendPrePad("Used: " + usedstr.ToString(), ", ");
            }

            return new Tuple<int,int,string>(max, made, needed.ToNullSafeString());
        }

        static public List<MaterialCommodities> GetShoppingList(List<Tuple<Recipe,int>> target, List<MaterialCommodities> list)
        {
            List<MaterialCommodities> shoppingList = new List<MaterialCommodities>();

            foreach (Tuple<Recipe, int> want in target)
            {
                Recipe r = want.Item1;
                int wanted = want.Item2;
                for (int i = 0; i < r.ingredients.Length; i++)
                {
                    string ingredient = r.ingredients[i];
                    int mi = list.FindIndex(x => x.shortname.Equals(ingredient));
                    int got = (mi >= 0) ? list[mi].scratchpad : 0;
                    int need = r.count[i] * wanted;

                    if (got < need)
                    {
                        int shopentry = shoppingList.FindIndex(x => x.shortname.Equals(ingredient));
                        if (shopentry >= 0)
                            shoppingList[shopentry].scratchpad += (need - got);
                        else
                        {
                            MaterialCommodityDB db = MaterialCommodityDB.GetCachedMaterialByShortName(ingredient);
                            if (db != null)       // MUST be there, its well know, but will check..
                            {
                                MaterialCommodities mc = new MaterialCommodities(db);        // make a new entry
                                mc.scratchpad = (need - got);
                                shoppingList.Add(mc);
                            }
                        }
                        if (mi >= 0) list[mi].scratchpad = 0;
                    }
                    else
                    {
                        if (mi >= 0) list[mi].scratchpad -= need;
                    }
                }
            }
            return shoppingList;
        }

        #endregion
    }

}
