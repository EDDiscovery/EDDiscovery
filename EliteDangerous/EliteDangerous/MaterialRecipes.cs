/*
 * Copyright © 2016 EDDiscovery development team
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

using EliteDangerousCore;
using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EliteDangerousCore
{
    public static class Recipes
    {
        public class Recipe
        {
            public string name;
            public string ingredientsstring;
            public string[] ingredients;
            public int[] count;

            public int Count { get { return ingredients.Length; } }

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
        static public Tuple<int, int, string> HowManyLeft(List<MaterialCommodities> list, Recipe r, int tomake = 0)
        {
            int max = int.MaxValue;
            System.Text.StringBuilder needed = new System.Text.StringBuilder(64);

            for (int i = 0; i < r.ingredients.Length; i++)
            {
                string ingredient = r.ingredients[i];

                int mi = list.FindIndex(x => x.shortname.Equals(ingredient));
                int got = (mi >= 0) ? list[mi].scratchpad : 0;
                int sets = got / r.count[i];

                max = Math.Min(max, sets);

                int need = r.count[i] * tomake;

                if (got < need)
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
                System.Text.StringBuilder usedstr = new System.Text.StringBuilder(64);

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

            return new Tuple<int, int, string>(max, made, needed.ToNullSafeString());
        }

        static public List<MaterialCommodities> GetShoppingList(List<Tuple<Recipe, int>> target, List<MaterialCommodities> list)
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

        public static string UsedInSynthesis(string name)
        {
            MaterialCommodityDB mc = MaterialCommodityDB.GetCachedMaterial(name);
            return Recipes.UsedInSynthesisAbv(mc?.shortname ?? "--");
        }

        public static string UsedInSynthesisAbv(string abv)
        {
            string usedin = "";
            foreach( var x in SynthesisRecipes )
            {
                if (x.ingredients.Contains(abv))
                    usedin = usedin.AppendPrePad(x.name + "-" + x.level, ",");
            }
            return usedin;
        }

        public static SynthesisRecipe FindSynthesis(string name, string level)
        {
            return SynthesisRecipes.Find(x => x.name.Equals(name, StringComparison.InvariantCultureIgnoreCase) && x.level.Equals(level, StringComparison.InvariantCultureIgnoreCase));
        }

        public static List<SynthesisRecipe> SynthesisRecipes = new List<SynthesisRecipe>()
        {
            new SynthesisRecipe( "FSD", "Premium","1C,1Ge,1Nb,1As,1Po,1Y" ),
            new SynthesisRecipe( "FSD", "Standard","1C,1V,1Ge,1Cd,1Nb" ),
            new SynthesisRecipe( "FSD", "Basic","1C,1V,1Ge" ),

            new SynthesisRecipe( "AFM Refill", "Premium","6V,4Cr,2Zn,2Zr,1Te,1Ru" ),
            new SynthesisRecipe( "AFM Refill", "Standard","6V,2Mn,1Mo,1Zr,1Sn" ),
            new SynthesisRecipe( "AFM Refill", "Basic","3V,2Ni,2Cr,2Zn" ),

            new SynthesisRecipe( "SRV Ammo", "Premium","2P,2Se,1Mo,1Tc" ),
            new SynthesisRecipe( "SRV Ammo", "Standard","1P,1Se,1Mn,1Mo" ),
            new SynthesisRecipe( "SRV Ammo", "Basic","1P,2S" ),

            new SynthesisRecipe( "SRV Repair", "Premium","2V,1Zn,2Cr,1W,1Te" ),
            new SynthesisRecipe( "SRV Repair", "Standard","3Ni,2V,1Mn,1Mo" ),
            new SynthesisRecipe( "SRV Repair", "Basic","2Fe,1Ni" ),

            new SynthesisRecipe( "SRV Refuel", "Premium","1S,1As,1Hg,1Tc" ),
            new SynthesisRecipe( "SRV Refuel", "Standard","1P,1S,1As,1Hg" ),
            new SynthesisRecipe( "SRV Refuel", "Basic","1P,1S" ),

            new SynthesisRecipe( "Plasma Munitions", "Premium", "5Se,4Mo,4Cd,2Tc" ),
            new SynthesisRecipe( "Plasma Munitions", "Standard","5P,1Se,3Mn,4Mo" ),
            new SynthesisRecipe( "Plasma Munitions", "Basic","4P,3S,1Mn" ),

            new SynthesisRecipe( "Explosive Munitions", "Premium","5P,4As,5Hg,5Nb,5Po" ),
            new SynthesisRecipe( "Explosive Munitions", "Standard","6P,6S,4As,2Hg" ),
            new SynthesisRecipe( "Explosive Munitions", "Basic","4S,3Fe,3Ni,4C" ),

            new SynthesisRecipe( "Small Calibre Munitions", "Premium","2P,2S,2Zr,2Hg,2W,1Sb" ),
            new SynthesisRecipe( "Small Calibre Munitions", "Standard","2P,2Fe,2Zr,2Zn,2Se" ),
            new SynthesisRecipe( "Small Calibre Munitions", "Basic","2S,2Fe,1Ni" ),

            new SynthesisRecipe( "High Velocity Munitions", "Premium","4V,2Zr,4W,2Y" ),
            new SynthesisRecipe( "High Velocity Munitions", "Standard","4Fe,3V,2Zr,2W" ),
            new SynthesisRecipe( "High Velocity Munitions", "Basic","2Fe,1V" ),

            new SynthesisRecipe( "Large Calibre Munitions", "Premium","8Zn,1As,1Hg,2W,2Sb" ),
            new SynthesisRecipe( "Large Calibre Munitions", "Standard","3P,2Zr,3Zn,1As,2Sn" ),
            new SynthesisRecipe( "Large Calibre Munitions", "Basic","2S,4Ni,3C" ),

            new SynthesisRecipe( "Limpets", "Basic", "10Fe,10Ni"),

            new SynthesisRecipe( "Chaff", "Premium", "1CC,2FiC,1ThA,1PRA"),
            new SynthesisRecipe( "Chaff", "Standard", "1CC,2FiC,1ThA"),
            new SynthesisRecipe( "Chaff", "Basic", "1CC,1FiC"),

            new SynthesisRecipe( "Heat Sinks", "Premium", "2BaC,2HCW,2HE,1PHR"),
            new SynthesisRecipe( "Heat Sinks", "Standard", "2BaC,2HCW,2HE"),
            new SynthesisRecipe( "Heat Sinks", "Basic", "1BaC,1HCW"),

            new SynthesisRecipe( "Life Support", "Basic", "2Fe,1Ni"),

            new SynthesisRecipe("AX Small Calibre Munitions", "Basic", "2Fe,1Ni,2S,2WP"),
            new SynthesisRecipe("AX Small Calibre Munitions", "Standard", "2Fe,2P,2Zr,3UES,4WP" ),
            new SynthesisRecipe("AX Small Calibre Munitions", "Premium", "3Fe,2P,2Zr,4UES,2UKCP,6WP" ),

            new SynthesisRecipe("Guardian Plasma Charger Munitions", "Basic", "3Cr,2HDP,3GPC,4GSWC"),
            new SynthesisRecipe("Guardian Plasma Charger Munitions", "Standard", "4Cr,2HE,2PA,2GPCe,2GTC"),
            new SynthesisRecipe("Guardian Plasma Charger Munitions", "Premium", "6Cr,2Zr,4HE,6PA,4GPCe,3GSWP"),

            new SynthesisRecipe("Guardian Gauss Cannon Munitions", "Basic", "3Mn,2FoC,2GPC,4GSWC"),
            new SynthesisRecipe("Guardian Gauss Cannon Munitions", "Standard", "5Mn,3HRC,5FoC,4GPC,3GSWP"),
            new SynthesisRecipe("Guardian Gauss Cannon Munitions", "Premium", "8Mn,4HRC,6FiC,10FoC"),

            new SynthesisRecipe("Enzyme Missile Launcher Munitions", "Basic", "3Fe,3S,4BMC,3PE,3WP,2Pb"),
            new SynthesisRecipe("Enzyme Missile Launcher Munitions", "Standard", "6S,4W,5BMC,6PE,4WP,4Pb"),
            new SynthesisRecipe("Enzyme Missile Launcher Munitions", "Premium", "5P,4W,6BMC,5PE,4WP,6Pb"),

            new SynthesisRecipe("AX Remote Flak Munitions", "Basic", "4Ni,3C,2S"),
            new SynthesisRecipe("AX Remote Flak Munitions", "Standard", "2Sn,3Zn,1As,3UKTC,2WC"),
            new SynthesisRecipe("AX Remote Flak Munitions", "Premium", "8Zn,2W,1As,3UES,4UKTC,1WP"),

            new SynthesisRecipe("Flechette Launcher Munitions", "Basic", "1W,3EA,2MC,2B"),
            new SynthesisRecipe("Flechette Launcher Munitions", "Standard", "4W,6EA,4MC,4B"),
            new SynthesisRecipe("Flechette Launcher Munitions", "Premium", "6W,5EA,9MC,6B"),

            new SynthesisRecipe("Guardian Shard Cannon Munitions", "Basic", "3C,2V,3CS,3GPCe,5GSWC"),
            new SynthesisRecipe("Guardian Shard Cannon Munitions", "Standard", "4CS,2GPCe,2GSWP"),
            new SynthesisRecipe("Guardian Shard Cannon Munitions", "Premium", "8C,3Se,4V,8CS"),

            new SynthesisRecipe("Guardian Shard Cannon Munitions", "Basic", "3GR,2HDP,2FoC,2PA,2Pb"),
            new SynthesisRecipe("Guardian Shard Cannon Munitions", "Standard", "5GR,3HDP,4FoC,5PA,3Pb"),
            new SynthesisRecipe("Guardian Shard Cannon Munitions", "Premium", "7GR,4HDP,6FoC,8PA,5Pb"),

            new SynthesisRecipe("AX Explosive Munitions", "Basic", "3Fe,3Ni,4C,3PE"),
            new SynthesisRecipe("AX Explosive Munitions", "Standard", "6S,6P,2Hg,4UKOC,4PE"),
            new SynthesisRecipe("AX Explosive Munitions", "Premium", "5W,4Hg,2Po,5BMC,5PE,6SFD"),
        };

        public static List<EngineeringRecipe> EngineeringRecipes = new List<EngineeringRecipe>()
        {
            #region Armor
            new EngineeringRecipe("Blast Resistant", "1Ni", "Armour", "1", "Liz Ryder,Selene Jean"),
            new EngineeringRecipe("Blast Resistant", "1C,1Zn", "Armour", "2", "Selene Jean"),
            new EngineeringRecipe("Blast Resistant", "1SAll,1V,1Zr", "Armour", "3", "Selene Jean"),
            new EngineeringRecipe("Blast Resistant", "1GA,1Hg,1W", "Armour", "4", "Selene Jean"),
            new EngineeringRecipe("Blast Resistant", "1PA,1Mo,1Ru", "Armour", "5", "Selene Jean"),
            new EngineeringRecipe("Heavy Duty", "1C", "Armour", "1", "Liz Ryder,Selene Jean"),
            new EngineeringRecipe("Heavy Duty", "1C,1SE", "Armour", "2", "Selene Jean"),
            new EngineeringRecipe("Heavy Duty", "1C,1HDC,1SE", "Armour", "3", "Selene Jean"),
            new EngineeringRecipe("Heavy Duty", "1FPC,1SS,1V", "Armour", "4", "Selene Jean"),
            new EngineeringRecipe("Heavy Duty", "1CoS,1FCC,1W", "Armour", "5", "Selene Jean"),
            new EngineeringRecipe("Kinetic Resistant", "1Ni", "Armour", "1", "Liz Ryder,Selene Jean"),
            new EngineeringRecipe("Kinetic Resistant", "1Ni,1V", "Armour", "2", "Selene Jean"),
            new EngineeringRecipe("Kinetic Resistant", "1HDC,1SAll,1V", "Armour", "3", "Selene Jean"),
            new EngineeringRecipe("Kinetic Resistant", "1GA,1FPC,1W", "Armour", "4", "Selene Jean"),
            new EngineeringRecipe("Kinetic Resistant", "1FCC,1Mo,1PA", "Armour", "5", "Selene Jean"),
            new EngineeringRecipe("Light Weight", "1Fe", "Armour", "1", "Liz Ryder,Selene Jean"),
            new EngineeringRecipe("Light Weight", "1CCo,1Fe", "Armour", "2", "Selene Jean"),
            new EngineeringRecipe("Light Weight", "1CCo,1HDC,1Fe", "Armour", "3", "Selene Jean"),
            new EngineeringRecipe("Light Weight", "1Ge,1CCe,1FPC", "Armour", "4", "Selene Jean"),
            new EngineeringRecipe("Light Weight", "1CCe,1Sn,1MGA", "Armour", "5", "Selene Jean"),
            new EngineeringRecipe("Thermal Resistant", "1HCW", "Armour", "1", "Liz Ryder,Selene Jean"),
            new EngineeringRecipe("Thermal Resistant", "1HDP,1Ni", "Armour", "2", "Selene Jean"),
            new EngineeringRecipe("Thermal Resistant", "1HE,1SAll,1V", "Armour", "3", "Selene Jean"),
            new EngineeringRecipe("Thermal Resistant", "1GA,1HV,1W", "Armour", "4", "Selene Jean"),
            new EngineeringRecipe("Thermal Resistant", "1Mo,1PA,1PHR", "Armour", "5", "Selene Jean"),
            #endregion
            #region Auto Field-Maintenance Unit
            new EngineeringRecipe("Shielded", "1WSE", "Auto Field-Maintenance Unit", "1", "Lori Jameson,Bill Turner"),
            new EngineeringRecipe("Shielded", "1C,1SE", "Auto Field-Maintenance Unit", "2", "Lori Jameson,Bill Turner"),
            new EngineeringRecipe("Shielded", "1C,1SE,1HDC", "Auto Field-Maintenance Unit", "3", "Lori Jameson,Bill Turner"),
            new EngineeringRecipe("Shielded", "1FPC,1SS,1V", "Auto Field-Maintenance Unit", "4", "Lori Jameson"),
            #endregion
            #region Beam Laser
            new EngineeringRecipe("Efficient Weapon", "1S", "Beam Laser", "1", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Efficient Weapon", "1HDP,1S", "Beam Laser", "2", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Efficient Weapon", "1Cr,1ESED,1HE", "Beam Laser", "3", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Efficient Weapon", "1HV,1IED,1Se", "Beam Laser", "4", "Broo Tarquin"),
            new EngineeringRecipe("Efficient Weapon", "1Cd,1PHR,1UED", "Beam Laser", "5", "Broo Tarquin"),
            new EngineeringRecipe("Light Weight", "1P", "Beam Laser", "1", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Light Weight", "1Mn,1SAll", "Beam Laser", "2", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Beam Laser", "3", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Beam Laser", "4", "Broo Tarquin"),
            new EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Beam Laser", "5", "Broo Tarquin"),
            new EngineeringRecipe("Long Range", "1S", "Beam Laser", "1", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Long Range", "1MCF,1S", "Beam Laser", "2", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Long Range", "1FoC,1MCF,1S", "Beam Laser", "3", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Long Range", "1CPo,1FoC,1MCF", "Beam Laser", "4", "Broo Tarquin"),
            new EngineeringRecipe("Long Range", "1BiC,1CIF,1ThA", "Beam Laser", "5", "Broo Tarquin"),
            new EngineeringRecipe("Overcharged", "1Ni", "Beam Laser", "1", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Overcharged", "1CCo,1Ni", "Beam Laser", "2", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Overcharged", "1CCo,1EA,1Ni", "Beam Laser", "3", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Overcharged", "1CCe,1PCa,1Zn", "Beam Laser", "4", "Broo Tarquin"),
            new EngineeringRecipe("Overcharged", "1CPo,1EFW,1Zr", "Beam Laser", "5", "Broo Tarquin"),
            new EngineeringRecipe("Short Range Blaster", "1Ni", "Beam Laser", "1", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Short Range Blaster", "1MCF,1Ni", "Beam Laser", "2", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Short Range Blaster", "1EA,1MCF,1Ni", "Beam Laser", "3", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Short Range Blaster", "1CPo,1EA,1MCF", "Beam Laser", "4", "Broo Tarquin"),
            new EngineeringRecipe("Short Range Blaster", "1BiC,1CCom,1CIF", "Beam Laser", "5", "Broo Tarquin"),
            new EngineeringRecipe("Sturdy Mount", "1Ni", "Beam Laser", "1", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Sturdy Mount", "1Ni,1SE", "Beam Laser", "2", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Sturdy Mount", "1Ni,1SE,1W", "Beam Laser", "3", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Sturdy Mount", "1Mo,1W,1Zn", "Beam Laser", "4", "Broo Tarquin"),
            new EngineeringRecipe("Sturdy Mount", "1HDC,1W,1Tc", "Beam Laser", "5", "Broo Tarquin"),
            #endregion
            #region Burst Laser
            new EngineeringRecipe("Efficient Weapon", "1S", "Burst Laser", "1", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Efficient Weapon", "1HDP,1S", "Burst Laser", "2", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Efficient Weapon", "1Cr,1ESED,1HE", "Burst Laser", "3", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Efficient Weapon", "1HV,1IED,1Se", "Burst Laser", "4", "Broo Tarquin"),
            new EngineeringRecipe("Efficient Weapon", "1Cd,1PHR,1UED", "Burst Laser", "5", "Broo Tarquin"),
            new EngineeringRecipe("Focused", "1Fe", "Burst Laser", "1", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Focused", "1CCo,1Fe", "Burst Laser", "2", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Focused", "1Cr,1CCe,1Fe", "Burst Laser", "3", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Focused", "1FoC,1Ge,1PCa", "Burst Laser", "4", "Broo Tarquin"),
            new EngineeringRecipe("Focused", "1MSC,1Nb,1RFC", "Burst Laser", "5", "Broo Tarquin"),
            new EngineeringRecipe("Light Weight", "1P", "Burst Laser", "1", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Light Weight", "1Mn,1SAll", "Burst Laser", "2", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Burst Laser", "3", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Burst Laser", "4", "Broo Tarquin"),
            new EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Burst Laser", "5", "Broo Tarquin"),
            new EngineeringRecipe("Long Range", "1S", "Burst Laser", "1", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Long Range", "1MCF,1S", "Burst Laser", "2", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Long Range", "1FoC,1MCF,1S", "Burst Laser", "3", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Long Range", "1CPo,1FoC,1MCF", "Burst Laser", "4", "Broo Tarquin"),
            new EngineeringRecipe("Long Range", "1BiC,1CIF,1ThA", "Burst Laser", "5", "Broo Tarquin"),
            new EngineeringRecipe("Overcharged", "1Ni", "Burst Laser", "1", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Overcharged", "1CCo,1Ni", "Burst Laser", "2", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Overcharged", "1CCo,1EA,1Ni", "Burst Laser", "3", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Overcharged", "1CCe,1PCa,1Zn", "Burst Laser", "4", "Broo Tarquin"),
            new EngineeringRecipe("Overcharged", "1CPo,1EFW,1Zr", "Burst Laser", "5", "Broo Tarquin"),
            new EngineeringRecipe("Rapid Fire", "1MS", "Burst Laser", "1", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Rapid Fire", "1HDP,1MS", "Burst Laser", "2", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Rapid Fire", "1ME,1PAll,1SLF", "Burst Laser", "3", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Rapid Fire", "1MC,1MCF,1ThA", "Burst Laser", "4", "Broo Tarquin"),
            new EngineeringRecipe("Rapid Fire", "1CCom,1PAll,1Tc", "Burst Laser", "5", "Broo Tarquin"),
            new EngineeringRecipe("Short Range Blaster", "1Ni", "Burst Laser", "1", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Short Range Blaster", "1MCF,1Ni", "Burst Laser", "2", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Short Range Blaster", "1EA,1MCF,1Ni", "Burst Laser", "3", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Short Range Blaster", "1CPo,1EA,1MCF", "Burst Laser", "4", "Broo Tarquin"),
            new EngineeringRecipe("Short Range Blaster", "1BiC,1CCom,1CIF", "Burst Laser", "5", "Broo Tarquin"),
            new EngineeringRecipe("Sturdy Mount", "1Ni", "Burst Laser", "1", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Sturdy Mount", "1Ni,1SE", "Burst Laser", "2", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Sturdy Mount", "1Ni,1SE,1W", "Burst Laser", "3", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Sturdy Mount", "1Mo,1W,1Zn", "Burst Laser", "4", "Broo Tarquin"),
            new EngineeringRecipe("Sturdy Mount", "1HDC,1Mo,1Tc", "Burst Laser", "5", "Broo Tarquin"),
            #endregion
            #region Cannon
            new EngineeringRecipe("Efficient Weapon", "1S", "Cannon", "1", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Efficient Weapon", "1HDP,1S", "Cannon", "2", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Efficient Weapon", "1Cr,1ESED,1HE", "Cannon", "3", "The Sarge"),
            new EngineeringRecipe("Efficient Weapon", "1HV,1IED,1Se", "Cannon", "4", "The Sarge"),
            new EngineeringRecipe("Efficient Weapon", "1Cd,1PHR,1UED", "Cannon", "5", "The Sarge"),
            new EngineeringRecipe("High Capacity Magazine", "1MS", "Cannon", "1", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("High Capacity Magazine", "1MS,1V", "Cannon", "2", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("High Capacity Magazine", "1MS,1Nb,1V", "Cannon", "3", "The Sarge"),
            new EngineeringRecipe("High Capacity Magazine", "1HDC,1ME,1Sn", "Cannon", "4", "The Sarge"),
            new EngineeringRecipe("High Capacity Magazine", "1MC,1MSC,1FPC", "Cannon", "5", "The Sarge"),
            new EngineeringRecipe("Light Weight", "1P", "Cannon", "1", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Light Weight", "1Mn,1SAll", "Cannon", "2", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Cannon", "3", "The Sarge"),
            new EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Cannon", "4", "The Sarge"),
            new EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Cannon", "5", "The Sarge"),
            new EngineeringRecipe("Long Range", "1S", "Cannon", "1", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Long Range", "1MCF,1S", "Cannon", "2", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Long Range", "1FoC,1MCF,1S", "Cannon", "3", "The Sarge"),
            new EngineeringRecipe("Long Range", "1CPo,1FoC,1MCF", "Cannon", "4", "The Sarge"),
            new EngineeringRecipe("Long Range", "1BiC,1CIF,1ThA", "Cannon", "5", "The Sarge"),
            new EngineeringRecipe("Overcharged", "1Ni", "Cannon", "1", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Overcharged", "1CCo,1Ni", "Cannon", "2", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Overcharged", "1CCo,1EA,1Ni", "Cannon", "3", "The Sarge"),
            new EngineeringRecipe("Overcharged", "1CCe,1PCa,1Zn", "Cannon", "4", "The Sarge"),
            new EngineeringRecipe("Overcharged", "1CPo,1EFW,1Zr", "Cannon", "5", "The Sarge"),
            new EngineeringRecipe("Rapid Fire", "1MS", "Cannon", "1", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Rapid Fire", "1HDP,1MS", "Cannon", "2", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Rapid Fire", "1ME,1PAll,1SLF", "Cannon", "3", "The Sarge"),
            new EngineeringRecipe("Rapid Fire", "1MC,1MCF,1ThA", "Cannon", "4", "The Sarge"),
            new EngineeringRecipe("Rapid Fire", "1CCom,1PAll,1Tc", "Cannon", "5", "The Sarge"),
            new EngineeringRecipe("Short Range Blaster", "1Ni", "Cannon", "1", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Short Range Blaster", "1MCF,1Ni", "Cannon", "2", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Short Range Blaster", "1EA,1MCF,1Ni", "Cannon", "3", "The Sarge"),
            new EngineeringRecipe("Short Range Blaster", "1CPo,1EA,1MCF", "Cannon", "4", "The Sarge"),
            new EngineeringRecipe("Short Range Blaster", "1BiC,1CCom,1CIF", "Cannon", "5", "The Sarge"),
            new EngineeringRecipe("Sturdy Mount", "1Ni", "Cannon", "1", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Sturdy Mount", "1Ni,1SE", "Cannon", "2", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Sturdy Mount", "1Ni,1SE,1W", "Cannon", "3", "The Sarge"),
            new EngineeringRecipe("Sturdy Mount", "1Mo,1W,1Zn", "Cannon", "4", "The Sarge"),
            new EngineeringRecipe("Sturdy Mount", "1HDC,1Mo,1Tc", "Cannon", "5", "The Sarge"),
            #endregion
            #region Chaff Launcher
            new EngineeringRecipe("Ammo Capacity", "1MS,1Nb,1V", "Chaff Launcher", "3", "Ram Tah"),
            new EngineeringRecipe("Light Weight", "1P", "Chaff Launcher", "1", "Ram Tah"),
            new EngineeringRecipe("Light Weight", "1Mn,1SAll", "Chaff Launcher", "2", "Ram Tah"),
            new EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Chaff Launcher", "3", "Ram Tah"),
            new EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Chaff Launcher", "4", "Ram Tah"),
            new EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Chaff Launcher", "5", "Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Ni", "Chaff Launcher", "1", "Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Ni,1SE", "Chaff Launcher", "2", "Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Ni,1SE,1W", "Chaff Launcher", "3", "Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Mo,1W,1Zn", "Chaff Launcher", "4", "Ram Tah"),
            new EngineeringRecipe("Reinforced", "1HDC,1Mo,1Tc", "Chaff Launcher", "5", "Ram Tah"),
            new EngineeringRecipe("Shielded", "1WSE", "Chaff Launcher", "1", "Ram Tah"),
            new EngineeringRecipe("Shielded", "1C,1SE", "Chaff Launcher", "2", "Ram Tah"),
            new EngineeringRecipe("Shielded", "1C,1HDC,1SE", "Chaff Launcher", "3", "Ram Tah"),
            new EngineeringRecipe("Shielded", "1FPC,1SS,1V", "Chaff Launcher", "4", "Ram Tah"),
            new EngineeringRecipe("Shielded", "1CoS,1FCC,1W", "Chaff Launcher", "5", "Ram Tah"),
            #endregion
            #region Collector Limpet Controller
            new EngineeringRecipe("Light Weight", "1P", "Collector Limpet Controller", "1", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Light Weight", "1Mn,1SAll", "Collector Limpet Controller", "2", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Collector Limpet Controller", "3", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Collector Limpet Controller", "4", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Collector Limpet Controller", "5", "Tiana Fortune,The Sarge"),
            new EngineeringRecipe("Reinforced", "1Ni", "Collector Limpet Controller", "1", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Ni,1SE", "Collector Limpet Controller", "2", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Ni,1SE,1W", "Collector Limpet Controller", "3", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Mo,1W,1Zn", "Collector Limpet Controller", "4", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Reinforced", "1HDC,1Mo,1Tc", "Collector Limpet Controller", "5", "Tiana Fortune,The Sarge"),
            new EngineeringRecipe("Shielded", "1WSE", "Collector Limpet Controller", "1", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Shielded", "1C,1SE", "Collector Limpet Controller", "2", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Shielded", "1C,1HDC,1SE", "Collector Limpet Controller", "3", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Shielded", "1FPC,1SS,1V", "Collector Limpet Controller", "4", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Shielded", "1CoS,1FCC,1W", "Collector Limpet Controller", "5", "Tiana Fortune,The Sarge"),
            #endregion
            #region Detailed Surface Scanner
            new EngineeringRecipe("Fast Scan", "1P", "Detailed Surface Scanner", "1", "Hera Tani,Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new EngineeringRecipe("Fast Scan", "1FFC,1P", "Detailed Surface Scanner", "2", "Hera Tani,Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new EngineeringRecipe("Fast Scan", "1FFC,1OSK,1P", "Detailed Surface Scanner", "3", "Hera Tani,Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new EngineeringRecipe("Fast Scan", "1AEA,1FoC,1Mn", "Detailed Surface Scanner", "4", "Hera Tani,Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Fast Scan", "1AEC,1As,1RFC", "Detailed Surface Scanner", "5", "Hera Tani,Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Long Range", "1Fe", "Detailed Surface Scanner", "1", "Hera Tani,Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new EngineeringRecipe("Long Range", "1HC,1Fe", "Detailed Surface Scanner", "2", "Hera Tani,Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new EngineeringRecipe("Long Range", "1HC,1Fe,1UED", "Detailed Surface Scanner", "3", "Hera Tani,Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new EngineeringRecipe("Long Range", "1DED,1EA,1Ge", "Detailed Surface Scanner", "4", "Hera Tani,Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Long Range", "1CED,1Nb,1PCa", "Detailed Surface Scanner", "5", "Hera Tani,Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Wide Angle", "1MS", "Detailed Surface Scanner", "1", "Hera Tani,Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new EngineeringRecipe("Wide Angle", "1Ge,1MS", "Detailed Surface Scanner", "2", "Hera Tani,Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new EngineeringRecipe("Wide Angle", "1CSD,1Ge,1MS", "Detailed Surface Scanner", "3", "Hera Tani,Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new EngineeringRecipe("Wide Angle", "1DSD,1ME,1Nb", "Detailed Surface Scanner", "4", "Hera Tani,Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Wide Angle", "1CSD,1MC,1Sn", "Detailed Surface Scanner", "5", "Hera Tani,Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner"),
            #endregion
            #region Electronic Countermeasure
            new EngineeringRecipe("Light Weight", "1P", "Electronic Countermeasure", "1", "Ram Tah"),
            new EngineeringRecipe("Light Weight", "1Mn,1SAll", "Electronic Countermeasure", "2", "Ram Tah"),
            new EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Electronic Countermeasure", "3", "Ram Tah"),
            new EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Electronic Countermeasure", "4", "Ram Tah"),
            new EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Electronic Countermeasure", "5", "Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Ni", "Electronic Countermeasure", "1", "Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Ni,1SE", "Electronic Countermeasure", "2", "Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Ni,1SE,1W", "Electronic Countermeasure", "3", "Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Mo,1W,1Zn", "Electronic Countermeasure", "4", "Ram Tah"),
            new EngineeringRecipe("Reinforced", "1HDC,1Mo,1Tc", "Electronic Countermeasure", "5", "Ram Tah"),
            new EngineeringRecipe("Shielded", "1WSE", "Electronic Countermeasure", "1", "Ram Tah"),
            new EngineeringRecipe("Shielded", "1C,1SE", "Electronic Countermeasure", "2", "Ram Tah"),
            new EngineeringRecipe("Shielded", "1C,1HDC", "Electronic Countermeasure", "3", "Ram Tah"),
            new EngineeringRecipe("Shielded", "1FPC,1SS,1V", "Electronic Countermeasure", "4", "Ram Tah"),
            new EngineeringRecipe("Shielded", "1CoS,1FCC,1W", "Electronic Countermeasure", "5", "Ram Tah"),
            #endregion
            #region Fragment Cannon
            new EngineeringRecipe("Double Shot", "1C", "Fragment Cannon", "1", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Double Shot", "1C,1ME", "Fragment Cannon", "2", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Double Shot", "1C,1CIF,1ME", "Fragment Cannon", "3", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Double Shot", "1MC,1SFP,1V", "Fragment Cannon", "4", "Zacariah Nemo"),
            new EngineeringRecipe("Double Shot", "1CCo,1HDC,1EFW", "Fragment Cannon", "5", "Zacariah Nemo"),
            new EngineeringRecipe("Efficient Weapon", "1S", "Fragment Cannon", "1", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Efficient Weapon", "1HDP,1S", "Fragment Cannon", "2", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Efficient Weapon", "1Cr,1ESED,1HE", "Fragment Cannon", "3", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Efficient Weapon", "1HV,1IED,1Se", "Fragment Cannon", "4", "Zacariah Nemo"),
            new EngineeringRecipe("Efficient Weapon", "1Cd,1PHR,1UED", "Fragment Cannon", "5", "Zacariah Nemo"),
            new EngineeringRecipe("High Capacity Magazine", "1MS", "Fragment Cannon", "1", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("High Capacity Magazine", "1MS,1V", "Fragment Cannon", "2", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("High Capacity Magazine", "1MS,1Nb,1V", "Fragment Cannon", "3", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("High Capacity Magazine", "1HDC,1ME,1Sn", "Fragment Cannon", "4", "Zacariah Nemo"),
            new EngineeringRecipe("High Capacity Magazine", "1MC,1MSC,1FPC", "Fragment Cannon", "5", "Zacariah Nemo"),
            new EngineeringRecipe("Light Weight", "1P", "Fragment Cannon", "1", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Light Weight", "1Mn,1SAll", "Fragment Cannon", "2", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Fragment Cannon", "3", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Fragment Cannon", "4", "Zacariah Nemo"),
            new EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Fragment Cannon", "5", "Zacariah Nemo"),
            new EngineeringRecipe("Overcharged", "1Ni", "Fragment Cannon", "1", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Overcharged", "1CCo,1Ni", "Fragment Cannon", "2", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Overcharged", "1CCo,1EA,1Ni", "Fragment Cannon", "3", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Overcharged", "1CCe,1PCa,1Zn", "Fragment Cannon", "4", "Zacariah Nemo"),
            new EngineeringRecipe("Overcharged", "1CPo,1EFW,1Zr", "Fragment Cannon", "5", "Zacariah Nemo"),
            new EngineeringRecipe("Rapid Fire", "1MS", "Fragment Cannon", "1", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Rapid Fire", "1HDP,1MS", "Fragment Cannon", "2", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Rapid Fire", "1ME,1PAll,1SLF", "Fragment Cannon", "3", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Rapid Fire", "1MC,1MCF,1ThA", "Fragment Cannon", "4", "Zacariah Nemo"),
            new EngineeringRecipe("Rapid Fire", "1CCom,1PAll,1Tc", "Fragment Cannon", "5", "Zacariah Nemo"),
            new EngineeringRecipe("Sturdy Mount", "1Ni", "Fragment Cannon", "1", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Sturdy Mount", "1Ni,1SE", "Fragment Cannon", "2", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Sturdy Mount", "1Ni,1SE,1W", "Fragment Cannon", "3", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Sturdy Mount", "1Mo,1W,1Zn", "Fragment Cannon", "4", "Zacariah Nemo"),
            new EngineeringRecipe("Sturdy Mount", "1HDC,1Mo,1Tc", "Fragment Cannon", "5", "Zacariah Nemo"),
            #endregion
            #region Frame Shift Drive
            new EngineeringRecipe("Faster Boot Sequence", "1GR", "Frame Shift Drive", "1", "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Faster Boot Sequence", "1Cr,1GR", "Frame Shift Drive", "2", "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Faster Boot Sequence", "1GR,1HDP,1Se", "Frame Shift Drive", "3", "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Faster Boot Sequence", "1Cd,1HE,1HC", "Frame Shift Drive", "4", "Elvira Martuuk,Felicity Farseer"),
            new EngineeringRecipe("Faster Boot Sequence", "1EA,1HV,1Te", "Frame Shift Drive", "5", "Elvira Martuuk,Felicity Farseer"),
            new EngineeringRecipe("Increased Range", "1ADWE", "Frame Shift Drive", "1", "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Increased Range", "1ADWE,1CP", "Frame Shift Drive", "2", "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Increased Range", "1CP,1P,1SWS", "Frame Shift Drive", "3", "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Increased Range", "1CD,1EHT,1Mn", "Frame Shift Drive", "4", "Elvira Martuuk,Felicity Farseer"),
            new EngineeringRecipe("Increased Range", "1As,1CM,1DWEx", "Frame Shift Drive", "5", "Elvira Martuuk,Felicity Farseer"),
            new EngineeringRecipe("Shielded", "1Ni", "Frame Shift Drive", "1", "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Shielded", "1C,1SE", "Frame Shift Drive", "2", "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Shielded", "1C,1SS,1Zn", "Frame Shift Drive", "3", "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Shielded", "1CoS,1HDC,1V", "Frame Shift Drive", "4", "Elvira Martuuk,Felicity Farseer"),
            new EngineeringRecipe("Shielded", "1IS,1FPC,1W", "Frame Shift Drive", "5", "Elvira Martuuk,Felicity Farseer"),
            #endregion
            #region Frame Shift Drive Interdictor
            new EngineeringRecipe("Expanded Capture Arc", "1MS", "Frame Shift Drive Interdictor", "1", "Colonel Bris Dekker,Felicity Farseer,Tiana Fortune"),
            new EngineeringRecipe("Expanded Capture Arc", "1ME,1UEF", "Frame Shift Drive Interdictor", "2", "Colonel Bris Dekker,Tiana Fortune"),
            new EngineeringRecipe("Expanded Capture Arc", "1GR,1MC,1TEC", "Frame Shift Drive Interdictor", "3", "Colonel Bris Dekker,Tiana Fortune"),
            new EngineeringRecipe("Expanded Capture Arc", "1DSD,1ME,1SWS", "Frame Shift Drive Interdictor", "4", "Colonel Bris Dekker,Tiana Fortune"),
            new EngineeringRecipe("Expanded Capture Arc", "1CSD,1EHT,1MC", "Frame Shift Drive Interdictor", "5", "Tiana Fortune"),
            new EngineeringRecipe("Longer Range", "1UEF", "Frame Shift Drive Interdictor", "1", "Colonel Bris Dekker,Felicity Farseer,Tiana Fortune"),
            new EngineeringRecipe("Longer Range", "1ADWE,1TEC", "Frame Shift Drive Interdictor", "2", "Colonel Bris Dekker,Tiana Fortune"),
            new EngineeringRecipe("Longer Range", "1ABSD,1AFT,1OSK", "Frame Shift Drive Interdictor", "3", "Colonel Bris Dekker,Tiana Fortune"),
            new EngineeringRecipe("Longer Range", "1USA,1SWS,1AEA", "Frams Shift Drive Interdictor", "4", "Colonel Bris Dekker"),
            #endregion
            #region Fuel Scoop
            new EngineeringRecipe("Shielded", "1WSE", "Fuel Scoop", "1", "Lori Jameson,Bill Turner"),
            new EngineeringRecipe("Shielded", "1C,1SE", "Fuel Scoop", "2", "Lori Jameson,Bill Turner"),
            new EngineeringRecipe("Shielded", "1C,1HDC", "Fuel Scoop", "3", "Lori Jameson,Bill Turner"),
            new EngineeringRecipe("Shielded", "1FPC,1SS,1V", "Fuel Scoop", "4", "Lori Jameson"),
            #endregion
            #region Fuel Transfer Limpet Controller
            new EngineeringRecipe("Light Weight", "1P", "Fuel Transfer Limpet Controller", "1", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Light Weight", "1Mn,1SAll", "Fuel Transfer Limpet Controller", "2", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Fuel Transfer Limpet Controller", "3", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Fuel Transfer Limpet Controller", "4", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Fuel Transfer Limpet Controller", "5", "Tiana Fortune,The Sarge"),
            new EngineeringRecipe("Reinforced", "1Ni", "Fuel Transfer Limpet Controller", "1", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Ni,1SE", "Fuel Transfer Limpet Controller", "2", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Ni,1SE,1W", "Fuel Transfer Limpet Controller", "3", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Mo,1W,1Zn", "Fuel Transfer Limpet Controller", "4", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Reinforced", "1HDC,1Mo,1Tc", "Fuel Transfer Limpet Controller", "5", "Tiana Fortune,The Sarge"),
            new EngineeringRecipe("Shielded", "1WSE", "Fuel Transfer Limpet Controller", "1", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Shielded", "1C,1SE", "Fuel Transfer Limpet Controller", "2", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Shielded", "1C,1HDC,1SE", "Fuel Transfer Limpet Controller", "3", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Shielded", "1FPC,1SS,1V", "Fuel Transfer Limpet Controller", "4", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Shielded", "1CoS,1FCC,1W", "Fuel Transfer Limpet Controller", "5", "Tiana Fortune,The Sarge"),
            #endregion
            #region Hatch Breaker Limpet Controller
            new EngineeringRecipe("Light Weight", "1P", "Hatch Breaker Limpet Controller", "1", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Light Weight", "1Mn,1SAll", "Hatch Breaker Limpet Controller", "2", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Hatch Breaker Limpet Controller", "3", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Hatch Breaker Limpet Controller", "4", "Tiana Fortune,The Sarge"),
            new EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Hatch Breaker Limpet Controller", "5", "Tiana Fortune,The Sarge"),
            new EngineeringRecipe("Reinforced", "1Ni", "Hatch Breaker Limpet Controller", "1", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Ni,1SE", "Hatch Breaker Limpet Controller", "2", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Ni,1SE,1W", "Hatch Breaker Limpet Controller", "3", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Mo,1W,1Zn", "Hatch Breaker Limpet Controller", "4", "Tiana Fortune,The Sarge"),
            new EngineeringRecipe("Reinforced", "1HDC,1Mo,1Tc", "Hatch Breaker Limpet Controller", "5", "Tiana Fortune,The Sarge"),
            new EngineeringRecipe("Shielded", "1WSE", "Hatch Breaker Limpet Controller", "1", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Shielded", "1C,1SE", "Hatch Breaker Limpet Controller", "2", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Shielded", "1C,1HDC,1SE", "Hatch Breaker Limpet Controller", "3", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Shielded", "1FPC,1SS,1V", "Hatch Breaker Limpet Controller", "4", "Tiana Fortune,The Sarge"),
            new EngineeringRecipe("Shielded", "1CoS,1FCC,1W", "Hatch Breaker Limpet Controller", "5", "Tiana Fortune,The Sarge"),
            #endregion
            #region Heat Sink Launcher
            new EngineeringRecipe("Ammo Capacity", "1MS,1Nb,1V", "Heat Sink Launcher", "3", "Ram Tah"),
            new EngineeringRecipe("Light Weight", "1P", "Heat Sink Launcher", "1", "Ram Tah"),
            new EngineeringRecipe("Light Weight", "1Mn,1SAll", "Heat Sink Launcher", "2", "Ram Tah"),
            new EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Heat Sink Launcher", "3", "Ram Tah"),
            new EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Heat Sink Launcher", "4", "Ram Tah"),
            new EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Heat Sink Launcher", "5", "Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Ni", "Heat Sink Launcher", "1", "Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Ni,1SE", "Heat Sink Launcher", "2", "Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Ni,1SE,1W", "Heat Sink Launcher", "3", "Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Mo,1W,1Zn", "Heat Sink Launcher", "4", "Ram Tah"),
            new EngineeringRecipe("Reinforced", "1HDC,1Mo,1Tc", "Heat Sink Launcher", "5", "Ram Tah"),
            new EngineeringRecipe("Shielded", "1WSE", "Heat Sink Launcher", "1", "Ram Tah"),
            new EngineeringRecipe("Shielded", "1C,1SE", "Heat Sink Launcher", "2", "Ram Tah"),
            new EngineeringRecipe("Shielded", "1C,1HDC,1SE", "Heat Sink Launcher", "3", "Ram Tah"),
            new EngineeringRecipe("Shielded", "1FPC,1SS,1V", "Heat Sink Launcher", "4", "Ram Tah"),
            new EngineeringRecipe("Shielded", "1CoS,1FCC,1W", "Heat Sink Launcher", "5", "Ram Tah"),
            #endregion
            #region Hull Reinforcement Package
            new EngineeringRecipe("Blast Resistant", "1Ni", "Hull Reinforcement Package", "1", "Liz Ryder,Selene Jean"),
            new EngineeringRecipe("Blast Resistant", "1C,1Zn", "Hull Reinforcement Package", "2", "Selene Jean"),
            new EngineeringRecipe("Blast Resistant", "1SAll,1V,1Zr", "Hull Reinforcement Package", "3", "Selene Jean"),
            new EngineeringRecipe("Blast Resistant", "1GA,1Hg,1W", "Hull Reinforcement Package", "4", "Selene Jean"),
            new EngineeringRecipe("Blast Resistant", "1Mo,1PA,1Ru", "Hull Reinforcement Package", "5", "Selene Jean"),
            new EngineeringRecipe("Heavy Duty", "1C", "Hull Reinforcement Package", "1", "Liz Ryder,Selene Jean"),
            new EngineeringRecipe("Heavy Duty", "1C,1SE", "Hull Reinforcement Package", "2", "Selene Jean"),
            new EngineeringRecipe("Heavy Duty", "1C,1HDC,1SE", "Hull Reinforcement Package", "3", "Selene Jean"),
            new EngineeringRecipe("Heavy Duty", "1FPC,1SS,1V", "Hull Reinforcement Package", "4", "Selene Jean"),
            new EngineeringRecipe("Heavy Duty", "1CoS,1FCC,1W", "Hull Reinforcement Package", "5", "Selene Jean"),
            new EngineeringRecipe("Kinetic Resistant", "1Ni", "Hull Reinforcement Package", "1", "Liz Ryder,Selene Jean"),
            new EngineeringRecipe("Kinetic Resistant", "1Ni,1V", "Hull Reinforcement Package", "2", "Selene Jean"),
            new EngineeringRecipe("Kinetic Resistant", "1HDC,1SAll,1V", "Hull Reinforcement Package", "3", "Selene Jean"),
            new EngineeringRecipe("Kinetic Resistant", "1GA,1FPC,1W", "Hull Reinforcement Package", "4", "Selene Jean"),
            new EngineeringRecipe("Kinetic Resistant", "1FCC,1Mo,1PA", "Hull Reinforcement Package", "5", "Selene Jean"),
            new EngineeringRecipe("Light Weight", "1Fe", "Hull Reinforcement Package", "1", "Liz Ryder,Selene Jean"),
            new EngineeringRecipe("Light Weight", "1CCo,1Fe", "Hull Reinforcement Package", "2", "Selene Jean"),
            new EngineeringRecipe("Light Weight", "1CCo,1HDC,1Fe", "Hull Reinforcement Package", "3", "Selene Jean"),
            new EngineeringRecipe("Light Weight", "1CCe,1Ge,1FPC", "Hull Reinforcement Package", "4", "Selene Jean"),
            new EngineeringRecipe("Light Weight", "1CCe,1MGA,1Sn", "Hull Reinforcement Package", "5", "Selene Jean"),
            new EngineeringRecipe("Thermal Resistant", "1HCW", "Hull Reinforcement Package", "1", "Liz Ryder,Selene Jean"),
            new EngineeringRecipe("Thermal Resistant", "1HDP,1Ni", "Hull Reinforcement Package", "2", "Selene Jean"),
            new EngineeringRecipe("Thermal Resistant", "1HE,1SAll,1V", "Hull Reinforcement Package", "3", "Selene Jean"),
            new EngineeringRecipe("Thermal Resistant", "1GA,1HV,1W", "Hull Reinforcement Package", "4", "Selene Jean"),
            new EngineeringRecipe("Thermal Resistant", "1Mo,1PA,1PHR", "Hull Reinforcement Package", "5", "Selene Jean"),
            #endregion
            #region Kill Warrant Scanner
            new EngineeringRecipe("Fast Scan", "1P", "Kill Warrant Scanner", "1", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Fast Scan", "1FFC,1P", "Kill Warrant Scanner", "2", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Fast Scan", "1FFC,1OSK,1P", "Kill Warrant Scanner", "3", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Fast Scan", "1AEA,1FoC,1Mn", "Kill Warrant Scanner", "4", "Tiana Fortune"),
            new EngineeringRecipe("Fast Scan", "1AEC,1As,1RFC", "Kill Warrant Scanner", "5", "Tiana Fortune"),
            new EngineeringRecipe("Light Weight", "1P", "Kill Warrant Scanner", "1", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Light Weight", "1Mn,1SAll", "Kill Warrant Scanner", "2", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Kill Warrant Scanner", "3", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Kill Warrant Scanner", "4", "Tiana Fortune"),
            new EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Kill Warrant Scanner", "5", "Tiana Fortune"),
            new EngineeringRecipe("Long Range", "1Fe", "Kill Warrant Scanner", "1", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Long Range", "1HC,1Fe", "Kill Warrant Scanner", "2", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Long Range", "1HC,1Fe,1UED", "Kill Warrant Scanner", "3", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Long Range", "1DED,1EA,1Ge", "Kill Warrant Scanner", "4", "Tiana Fortune"),
            new EngineeringRecipe("Long Range", "1CED,1Nb,1PCa", "Kill Warrant Scanner", "5", "Tiana Fortune"),
            new EngineeringRecipe("Reinforced", "1Ni", "Kill Warrant Scanner", "1", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Reinforced", "1Ni,1SE", "Kill Warrant Scanner", "2", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Reinforced", "1Ni,1SE,1W", "Kill Warrant Scanner", "3", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Reinforced", "1Mo,1W,1Zn", "Kill Warrant Scanner", "4", "Tiana Fortune"),
            new EngineeringRecipe("Reinforced", "1HDC,1Mo,1Tc", "Kill Warrant Scanner", "5", "Tiana Fortune"),
            new EngineeringRecipe("Shielded", "1WSE", "Kill Warrant Scanner", "1", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Shielded", "1C,1SE", "Kill Warrant Scanner", "2", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Shielded", "1C,1HDC,1SE", "Kill Warrant Scanner", "3", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Shielded", "1FPC,1SS,1V", "Kill Warrant Scanner", "4", "Tiana Fortune"),
            new EngineeringRecipe("Shielded", "1CoS,1FCC,1W", "Kill Warrant Scanner", "5", "Tiana Fortune"),
            new EngineeringRecipe("Wide Angle", "1MS", "Kill Warrant Scanner", "1", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Wide Angle", "1Ge,1MS", "Kill Warrant Scanner", "2", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Wide Angle", "1CSD,1Ge,1MS", "Kill Warrant Scanner", "3", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Wide Angle", "1DSD,1ME,1Nb", "Kill Warrant Scanner", "4", "Tiana Fortune"),
            new EngineeringRecipe("Wide Angle", "1CFSD,1MC,1Sn", "Kill Warrant Scanner", "5", "Tiana Fortune"),
            #endregion
            #region Life Support
            new EngineeringRecipe("Light Weight", "1P", "Life Support", "1", "Lori Jameson,Bill Turner"),
            new EngineeringRecipe("Light Weight", "1Mn,1SAll", "Life Support", "2", "Lori Jameson,Bill Turner"),
            new EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Life Support", "3", "Lori Jameson,Bill Turner"),
            new EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Life Support", "4", "Lori Jameson"),
            new EngineeringRecipe("Reinforced", "1Ni", "Life Support", "1", "Lori Jameson,Bill Turner"),
            new EngineeringRecipe("Reinforced", "1Ni,1SE", "Life Support", "2", "Lori Jameson,Bill Turner"),
            new EngineeringRecipe("Reinforced", "1Ni,1SE,1W", "Life Support", "3", "Lori Jameson,Bill Turner"),
            new EngineeringRecipe("Reinforced", "1Mo,1W,1Zn", "Life Support", "4", "Lori Jameson"),
            new EngineeringRecipe("Shielded", "1WSE", "Life Support", "1", "Lori Jameson,Bill Turner"),
            new EngineeringRecipe("Shielded", "1C,1SE", "Life Support", "2", "Lori Jameson,Bill Turner"),
            new EngineeringRecipe("Shielded", "1C,1HDC,1SE", "Life Support", "3", "Lori Jameson,Bill Turner"),
            new EngineeringRecipe("Shielded", "1FPC,1SS,1V", "Life Support", "4", "Lori Jameson"),
            #endregion
            #region Manifest Scanner
            new EngineeringRecipe("Fast Scan", "1P", "Manifest Scanner", "1", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Fast Scan", "1FFC,1P", "Manifest Scanner", "2", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Fast Scan", "1FFC,1OSK,1P", "Manifest Scanner", "3", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Fast Scan", "1AEA,1FoC,1Mn", "Manifest Scanner", "4", "Tiana Fortune"),
            new EngineeringRecipe("Fast Scan", "1AEC,1As,1RFC", "Manifest Scanner", "5", "Tiana Fortune"),
            new EngineeringRecipe("Light Weight", "1P", "Manifest Scanner", "1", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Light Weight", "1Mn,1SAll", "Manifest Scanner", "2", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Manifest Scanner", "3", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Manifest Scanner", "4", "Tiana Fortune"),
            new EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Manifest Scanner", "5", "Tiana Fortune"),
            new EngineeringRecipe("Long Range", "1Fe", "Manifest Scanner", "1", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Long Range", "1HC,1Fe", "Manifest Scanner", "2", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Long Range", "1HC,1Fe,1UED", "Manifest Scanner", "3", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Long Range", "1DED,1EA,1Ge", "Manifest Scanner", "4", "Tiana Fortune"),
            new EngineeringRecipe("Long Range", "1CED,1Nb,1PCa", "Manifest Scanner", "5", "Tiana Fortune"),
            new EngineeringRecipe("Reinforced", "1Ni", "Manifest Scanner", "1", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Reinforced", "1Ni,1SE", "Manifest Scanner", "2", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Reinforced", "1Ni,1SE,1W", "Manifest Scanner", "3", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Reinforced", "1Mo,1W,1Zn", "Manifest Scanner", "4", "Tiana Fortune"),
            new EngineeringRecipe("Reinforced", "1HDC,1Mo,1Tc", "Manifest Scanner", "5", "Tiana Fortune"),
            new EngineeringRecipe("Shielded", "1WSE", "Manifest Scanner", "1", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Shielded", "1C,1SE", "Manifest Scanner", "2", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Shielded", "1C,1HDC,1SE", "Manifest Scanner", "3", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Shielded", "1FPC,1SS,1V", "Manifest Scanner", "4", "Tiana Fortune"),
            new EngineeringRecipe("Shielded", "1CoS,1FCC,1W", "Manifest Scanner", "5", "Tiana Fortune"),
            new EngineeringRecipe("Wide Angle", "1MS", "Manifest Scanner", "1", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Wide Angle", "1Ge,1MS", "Manifest Scanner", "2", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Wide Angle", "1CSD,1Ge,1MS", "Manifest Scanner", "3", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Wide Angle", "1DSD,1ME,1Nb", "Manifest Scanner", "4", "Tiana Fortune"),
            new EngineeringRecipe("Wide Angle", "1CFSD,1MC,1Sn", "Manifest Scanner", "5", "Tiana Fortune"),
            #endregion
            #region Mine Launcher
            new EngineeringRecipe("High Capacity Magazine", "1MS", "Mine Launcher", "1", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("High Capacity Magazine", "1MS,1V", "Mine Launcher", "2", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("High Capacity Magazine", "1MS,1Nb,1V", "Mine Launcher", "3", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("High Capacity Magazine", "1HDC,1ME,1Sn", "Mine Launcher", "4", "Juri Ishmaak"),
            new EngineeringRecipe("High Capacity Magazine", "1MC,1MSC,1FPC", "Mine Launcher", "5", "Juri Ishmaak"),
            new EngineeringRecipe("Light Weight", "1P", "Mine Launcher", "1", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Light Weight", "1Mn,1SAll", "Mine Launcher", "2", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Mine Launcher", "3", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Mine Launcher", "4", "Juri Ishmaak"),
            new EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Mine Launcher", "5", "Juri Ishmaak"),
            new EngineeringRecipe("Rapid Fire", "1MS", "Mine Launcher", "1", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Rapid Fire", "1HDP,1MS", "Mine Launcher", "2", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Rapid Fire", "1ME,1PAll,1SLF", "Mine Launcher", "3", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Rapid Fire", "1MC,1MCF,1ThA", "Mine Launcher", "4", "Juri Ishmaak"),
            new EngineeringRecipe("Rapid Fire", "1CCom,1PAll,1Tc", "Mine Launcher", "5", "Juri Ishmaak"),
            new EngineeringRecipe("Sturdy Mount", "1Ni", "Mine Launcher", "1", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Sturdy Mount", "1Ni,1SE", "Mine Launcher", "2", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Sturdy Mount", "1Ni,1SE,1W", "Mine Launcher", "3", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Sturdy Mount", "1Mo,1W,1Zn", "Mine Launcher", "4", "Juri Ishmaak"),
            new EngineeringRecipe("Sturdy Mount", "1HDC,1Mo,1Tc", "Mine Launcher", "5", "Juri Ishmaak"),
            #endregion
            #region Missile Rack
            new EngineeringRecipe("High Capacity Magazine", "1MS", "Missile Rack", "1", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("High Capacity Magazine", "1MS,1V", "Missile Rack", "2", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("High Capacity Magazine", "1MS,1Nb,1V", "Missile Rack", "3", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("High Capacity Magazine", "1HDC,1ME,1Sn", "Missile Rack", "4", "Liz Ryder"),
            new EngineeringRecipe("High Capacity Magazine", "1MC,1MSC,1FPC", "Missile Rack", "5", "Liz Ryder"),
            new EngineeringRecipe("Light Weight", "1P", "Missile Rack", "1", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Light Weight", "1Mn,1SAll", "Missile Rack", "2", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Missile Rack", "3", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Missile Rack", "4", "Liz Ryder"),
            new EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Missile Rack", "5", "Liz Ryder"),
            new EngineeringRecipe("Rapid Fire", "1MS", "Missile Rack", "1", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Rapid Fire", "1HDP,1MS", "Missile Rack", "2", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Rapid Fire", "1ME,1PAll,1SLF", "Missile Rack", "3", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Rapid Fire", "1MC,1MCF,1ThA", "Missile Rack", "4", "Liz Ryder"),
            new EngineeringRecipe("Rapid Fire", "1CCom,1PAll,1Tc", "Missile Rack", "5", "Liz Ryder"),
            new EngineeringRecipe("Sturdy Mount", "1Ni", "Missile Rack", "1", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Sturdy Mount", "1Ni,1SE", "Missile Rack", "2", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Sturdy Mount", "1Ni,1SE,1W", "Missile Rack", "3", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Sturdy Mount", "1Mo,1W,1Zn", "Missile Rack", "4", "Liz Ryder"),
            new EngineeringRecipe("Sturdy Mount", "1HDC,1Mo,1Tc", "Missile Rack", "5", "Liz Ryder"),
            #endregion
            #region Multi-cannon
            new EngineeringRecipe("Efficient Weapon", "1S", "Multi-cannon", "1", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Efficient Weapon", "1HDP,1S", "Multi-cannon", "2", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Efficient Weapon", "1Cr,1ESED,1HE", "Multi-cannon", "3", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Efficient Weapon", "1HV,1IED,1Se", "Multi-cannon", "4", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("Efficient Weapon", "1Cd,1PHR,1UED", "Multi-cannon", "5", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("High Capacity Magazine", "1MS", "Multi-cannon", "1", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("High Capacity Magazine", "1MS,1V", "Multi-cannon", "2", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("High Capacity Magazine", "1MS,1Nb,1V", "Multi-cannon", "3", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("High Capacity Magazine", "1HDC,1ME,1Sn", "Multi-cannon", "4", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("High Capacity Magazine", "1MC,1MSC,1FPC", "Multi-cannon", "5", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("Light Weight", "1P", "Multi-cannon", "1", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Light Weight", "1Mn,1SAll", "Multi-cannon", "2", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Multi-cannon", "3", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Multi-cannon", "4", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Multi-cannon", "5", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("Long Range", "1S", "Multi-cannon", "1", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("Long Range", "1MCF,1S", "Multi-cannon", "2", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("Long Range", "1FoC,1MCF,1S", "Multi-cannon", "3", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("Long Range", "1CPo,1FoC,1MCF", "Multi-cannon", "4", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("Long Range", "1BiC,1CIF,1ThA", "Multi-cannon", "5", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("Overcharged", "1Ni", "Multi-cannon", "1", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Overcharged", "1CCo,1Ni", "Multi-cannon", "2", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Overcharged", "1CCo,1EA,1Ni", "Multi-cannon", "3", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Overcharged", "1CCe,1PCa,1Zn", "Multi-cannon", "4", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("Overcharged", "1CPo,1EFW,1Zr", "Multi-cannon", "5", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("Rapid Fire", "1MS", "Multi-cannon", "1", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("Rapid Fire", "1HDP,1MS", "Multi-cannon", "2", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("Rapid Fire", "1ME,1PAll,1SLF", "Multi-cannon", "3", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("Rapid Fire", "1MC,1MCF,1ThA", "Multi-cannon", "4", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("Rapid Fire", "1CCom,1PAll,1Tc", "Multi-cannon", "5", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("Short Range Blaster", "1Ni", "Multi-cannon", "1", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Short Range Blaster", "1MCF,1Ni", "Multi-cannon", "2", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Short Range Blaster", "1EA,1MCF,1Ni", "Multi-cannon", "3", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Short Range Blaster", "1CPo,1EA,1MCF", "Multi-cannon", "4", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("Short Range Blaster", "1BiC,1CCom,1CIF", "Multi-cannon", "5", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("Sturdy Mount", "1Ni", "Multi-cannon", "1", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Sturdy Mount", "1Ni,1SE", "Multi-cannon", "2", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Sturdy Mount", "1Ni,1SE,1W", "Multi-cannon", "3", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Sturdy Mount", "1Mo,1W,1Zn", "Multi-cannon", "4", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("Sturdy Mount", "1HDC,1Mo,1Tc", "Multi-cannon", "5", "Tod \"The Blaster\" McQuinn"),
            #endregion
            #region Plasma Accelerator
            new EngineeringRecipe("Efficient Weapon", "1S", "Plasma Accelerator", "1", "Zacariah Nemo,Bill Turner"),
            new EngineeringRecipe("Efficient Weapon", "1HDP,1S", "Plasma Accelerator", "2", "Zacariah Nemo,Bill Turner"),
            new EngineeringRecipe("Efficient Weapon", "1Cr,1ESED,1HE", "Plasma Accelerator", "3", "Bill Turner"),
            new EngineeringRecipe("Efficient Weapon", "1HV,1IED,1Se", "Plasma Accelerator", "4", "Bill Turner"),
            new EngineeringRecipe("Efficient Weapon", "1Cd,1PHR,1UED", "Plasma Accelerator", "5", "Bill Turner"),
            new EngineeringRecipe("Focused", "1Fe", "Plasma Accelerator", "1", "Zacariah Nemo,Bill Turner"),
            new EngineeringRecipe("Focused", "1CCo,1Fe", "Plasma Accelerator", "2", "Zacariah Nemo,Bill Turner"),
            new EngineeringRecipe("Focused", "1Cr,1CCe,1Fe", "Plasma Accelerator", "3", "Bill Turner"),
            new EngineeringRecipe("Focused", "1FoC,1Ge,1PCa", "Plasma Accelerator", "4", "Bill Turner"),
            new EngineeringRecipe("Focused", "1MSC,1Nb,1RFC", "Plasma Accelerator", "5", "Bill Turner"),
            new EngineeringRecipe("Light Weight", "1P", "Plasma Accelerator", "1", "Zacariah Nemo,Bill Turner"),
            new EngineeringRecipe("Light Weight", "1Mn,1SAll", "Plasma Accelerator", "2", "Zacariah Nemo,Bill Turner"),
            new EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Plasma Accelerator", "3", "Bill Turner"),
            new EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Plasma Accelerator", "4", "Bill Turner"),
            new EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Plasma Accelerator", "5", "Bill Turner"),
            new EngineeringRecipe("Long Range", "1S", "Plasma Accelerator", "1", "Zacariah Nemo,Bill Turner"),
            new EngineeringRecipe("Long Range", "1MCF,1S", "Plasma Accelerator", "2", "Zacariah Nemo,Bill Turner"),
            new EngineeringRecipe("Long Range", "1FoC,1MCF,1S", "Plasma Accelerator", "3", "Bill Turner"),
            new EngineeringRecipe("Long Range", "1CPo,1FoC,1MCF", "Plasma Accelerator", "4", "Bill Turner"),
            new EngineeringRecipe("Long Range", "1BiC,1CIF,1ThA", "Plasma Accelerator", "5", "Bill Turner"),
            new EngineeringRecipe("Overcharged", "1Ni", "Plasma Accelerator", "1", "Zacariah Nemo,Bill Turner"),
            new EngineeringRecipe("Overcharged", "1CCo,1Ni", "Plasma Accelerator", "2", "Zacariah Nemo,Bill Turner"),
            new EngineeringRecipe("Overcharged", "1CCo,1EA,1Ni", "Plasma Accelerator", "3", "Bill Turner"),
            new EngineeringRecipe("Overcharged", "1CCe,1PCa,1Zn", "Plasma Accelerator", "4", "Bill Turner"),
            new EngineeringRecipe("Overcharged", "1CPo,1EFW,1Zr", "Plasma Accelerator", "5", "Bill Turner"),
            new EngineeringRecipe("Rapid Fire", "1MS", "Plasma Accelerator", "1", "Zacariah Nemo,Bill Turner"),
            new EngineeringRecipe("Rapid Fire", "1HDP,1MS", "Plasma Accelerator", "2", "Zacariah Nemo,Bill Turner"),
            new EngineeringRecipe("Rapid Fire", "1ME,1PAll,1SLF", "Plasma Accelerator", "3", "Bill Turner"),
            new EngineeringRecipe("Rapid Fire", "1MC,1MCF,1ThA", "Plasma Accelerator", "4", "Bill Turner"),
            new EngineeringRecipe("Rapid Fire", "1CCom,1PAll,1Tc", "Plasma Accelerator", "5", "Bill Turner"),
            new EngineeringRecipe("Short Range Blaster", "1Ni", "Plasma Accelerator", "1", "Zacariah Nemo,Bill Turner"),
            new EngineeringRecipe("Short Range Blaster", "1MCF,1Ni", "Plasma Accelerator", "2", "Zacariah Nemo,Bill Turner"),
            new EngineeringRecipe("Short Range Blaster", "1EA,1MCF,1Ni", "Plasma Accelerator", "3", "Bill Turner"),
            new EngineeringRecipe("Short Range Blaster", "1CPo,1EA,1MCF", "Plasma Accelerator", "4", "Bill Turner"),
            new EngineeringRecipe("Short Range Blaster", "1BiC,1CCom,1CIF", "Plasma Accelerator", "5", "Bill Turner"),
            new EngineeringRecipe("Sturdy Mount", "1Ni", "Plasma Accelerator", "1", "Zacariah Nemo,Bill Turner"),
            new EngineeringRecipe("Sturdy Mount", "1Ni,1SE", "Plasma Accelerator", "2", "Zacariah Nemo,Bill Turner"),
            new EngineeringRecipe("Sturdy Mount", "1Ni,1SE,1W", "Plasma Accelerator", "3", "Bill Turner"),
            new EngineeringRecipe("Sturdy Mount", "1Mo,1W,1Zn", "Plasma Accelerator", "4", "Bill Turner"),
            new EngineeringRecipe("Sturdy Mount", "1HDC,1Mo,1Tc", "Plasma Accelerator", "5", "Bill Turner"),
            #endregion
            #region Point Defence
            new EngineeringRecipe("Ammo Capacity", "1MS,1Nb,1V", "Point Defence", "3", "Ram Tah"),
            new EngineeringRecipe("Light Weight", "1P", "Point Defence", "1", "Ram Tah"),
            new EngineeringRecipe("Light Weight", "1Mn,1SAll", "Point Defence", "2", "Ram Tah"),
            new EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Point Defence", "3", "Ram Tah"),
            new EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Point Defence", "4", "Ram Tah"),
            new EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Point Defence", "5", "Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Ni", "Point Defence", "1", "Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Ni,1SE", "Point Defence", "2", "Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Ni,1SE,1W", "Point Defence", "3", "Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Mo,1W,1Zn", "Point Defence", "4", "Ram Tah"),
            new EngineeringRecipe("Reinforced", "1HDC,1Mo,1Tc", "Point Defence", "5", "Ram Tah"),
            new EngineeringRecipe("Shielded", "1WSE", "Point Defence", "1", "Ram Tah"),
            new EngineeringRecipe("Shielded", "1C,1SE", "Point Defence", "2", "Ram Tah"),
            new EngineeringRecipe("Shielded", "1C,1HDC,1SE", "Point Defence", "3", "Ram Tah"),
            new EngineeringRecipe("Shielded", "1FPC,1SS,1V", "Point Defence", "4", "Ram Tah"),
            new EngineeringRecipe("Shielded", "1CoS,1FCC,1W", "Point Defence", "5", "Ram Tah"),
            #endregion
            #region Power Distributor
            new EngineeringRecipe("Charge Enhanced", "1SLF", "Power Distributor", "1", "Hera Tani,Marco Qwent,The Dweller"),
            new EngineeringRecipe("Charge Enhanced", "1CP,1SLF", "Power Distributor", "2", "Hera Tani,Marco Qwent,The Dweller"),
            new EngineeringRecipe("Charge Enhanced", "1CD,1GR,1MCF", "Power Distributor", "3", "Hera Tani,Marco Qwent,The Dweller"),
            new EngineeringRecipe("Charge Enhanced", "1CM,1CIF,1HC", "Power Distributor", "4", "The Dweller"),
            new EngineeringRecipe("Charge Enhanced", "1CM,1CIF,1EFC", "Power Distributor", "5", "The Dweller"),
            new EngineeringRecipe("Engine Focused", "1S", "Power Distributor", "1", "Hera Tani,Marco Qwent,The Dweller"),
            new EngineeringRecipe("Engine Focused", "1CCo,1S", "Power Distributor", "2", "Hera Tani,Marco Qwent,The Dweller"),
            new EngineeringRecipe("Engine Focused", "1ABSD,1Cr,1EA", "Power Distributor", "3", "Hera Tani,Marco Qwent,The Dweller"),
            new EngineeringRecipe("Engine Focused", "1USA,1Se,1PCa", "Power Distributor", "4", "The Dweller"),
            new EngineeringRecipe("Engine Focused", "1CSD,1Cd,1MSC", "Power Distributor", "5", "The Dweller"),
            new EngineeringRecipe("High Charge Capacity", "1S", "Power Distributor", "1", "Hera Tani,Marco Qwent,The Dweller"),
            new EngineeringRecipe("High Charge Capacity", "1Cr,1SLF", "Power Distributor", "2", "Hera Tani,Marco Qwent,The Dweller"),
            new EngineeringRecipe("High Charge Capacity", "1Cr,1HDC,1SLF", "Power Distributor", "3", "Hera Tani,Marco Qwent,The Dweller"),
            new EngineeringRecipe("High Charge Capacity", "1MCF,1FPC,1Se", "Power Distributor", "4", "The Dweller"),
            new EngineeringRecipe("High Charge Capacity", "1CIF,1MSC,1FPC", "Power Distributor", "5", "The Dweller"),
            new EngineeringRecipe("Shielded", "1WSE", "Power Distributor", "1", "Hera Tani,Marco Qwent,The Dweller"),
            new EngineeringRecipe("Shielded", "1C,1SE", "Power Distributor", "2", "Hera Tani,Marco Qwent,The Dweller"),
            new EngineeringRecipe("Shielded", "1C,1HDC,1SE", "Power Distributor", "3", "Hera Tani,Marco Qwent,The Dweller"),
            new EngineeringRecipe("Shielded", "1FPC,1SS,1V", "Power Distributor", "4", "The Dweller"),
            new EngineeringRecipe("Shielded", "1CoS,1FCC,1W", "Power Distributor", "5", "The Dweller"),
            new EngineeringRecipe("System Focused", "1S", "Power Distributor", "1", "Hera Tani,Marco Qwent,The Dweller"),
            new EngineeringRecipe("System Focused", "1CCo,1S", "Power Distributor", "2", "Hera Tani,Marco Qwent,The Dweller"),
            new EngineeringRecipe("System Focused", "1ABSD,1Cr,1EA", "Power Distributor", "3", "Hera Tani,Marco Qwent,The Dweller"),
            new EngineeringRecipe("System Focused", "1USA,1Se,1PCa", "Power Distributor", "4", "The Dweller"),
            new EngineeringRecipe("System Focused", "1CSD,1Cd,1MSC", "Power Distributor", "5", "The Dweller"),
            new EngineeringRecipe("Weapon Focused", "1S", "Power Distributor", "1", "Hera Tani,Marco Qwent,The Dweller"),
            new EngineeringRecipe("Weapon Focused", "1CCo,1S", "Power Distributor", "2", "Hera Tani,Marco Qwent,The Dweller"),
            new EngineeringRecipe("Weapon Focused", "1ABSD,1HC,1Se", "Power Distributor", "3", "Hera Tani,Marco Qwent,The Dweller"),
            new EngineeringRecipe("Weapon Focused", "1USA,1EA,1Cd", "Power Distributor", "4", "The Dweller"),
            new EngineeringRecipe("Weapon Focused", "1CSD,1PCa,1Te", "Power Distributor", "5", "The Dweller"),
            #endregion
            #region Power Plant
            new EngineeringRecipe("Armoured", "1WSE", "Power Plant", "1", "Felicity Farseer,Hera Tani,Marco Qwent"),
            new EngineeringRecipe("Armoured", "1C,1SE", "Power Plant", "2", "Hera Tani,Marco Qwent"),
            new EngineeringRecipe("Armoured", "1C,1HDC,1SE", "Power Plant", "3", "Hera Tani,Marco Qwent"),
            new EngineeringRecipe("Armoured", "1FPC,1SS,1V", "Power Plant", "4", "Hera Tani,Marco Qwent"),
            new EngineeringRecipe("Armoured", "1CoS,1FCC,1W", "Power Plant", "5", "Hera Tani"),
            new EngineeringRecipe("Low Emissions", "1Fe", "Power Plant", "1", "Felicity Farseer,Hera Tani,Marco Qwent"),
            new EngineeringRecipe("Low Emissions", "1Fe,1IED", "Power Plant", "2", "Hera Tani,Marco Qwent"),
            new EngineeringRecipe("Low Emissions", "1HE,1Fe,1IED", "Power Plant", "3", "Hera Tani,Marco Qwent"),
            new EngineeringRecipe("Low Emissions", "1Ge,1UED,1HV", "Power Plant", "4", "Hera Tani,Marco Qwent"),
            new EngineeringRecipe("Low Emissions", "1Nb,1DED,1PHR", "Power Plant", "5", "Hera Tani"),
            new EngineeringRecipe("Overcharged", "1S", "Power Plant", "1", "Felicity Farseer,Hera Tani,Marco Qwent"),
            new EngineeringRecipe("Overcharged", "1CCo,1HCW", "Power Plant", "2", "Hera Tani,Marco Qwent"),
            new EngineeringRecipe("Overcharged", "1CCo,1HCW,1Se", "Power Plant", "3", "Hera Tani,Marco Qwent"),
            new EngineeringRecipe("Overcharged", "1Cd,1CCe,1HDP", "Power Plant", "4", "Hera Tani,Marco Qwent"),
            new EngineeringRecipe("Overcharged", "1CM,1CCe,1Te", "Power Plant", "5", "Hera Tani"),
            #endregion
            #region Prospector Limpet Controller
            new EngineeringRecipe("Light Weight", "1P", "Prospector Limpet Controller", "1", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Light Weight", "1Mn,1SAll", "Prospector Limpet Controller", "2", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Prospector Limpet Controller", "3", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Prospector Limpet Controller", "4", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Prospector Limpet Controller", "5", "Tiana Fortune,The Sarge"),
            new EngineeringRecipe("Reinforced", "1Ni", "Prospector Limpet Controller", "1", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Ni,1SE", "Prospector Limpet Controller", "2", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Ni,1SE,1W", "Prospector Limpet Controller", "3", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Reinforced", "1Mo,1W,1Zn", "Prospector Limpet Controller", "4", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Reinforced", "1HDC,1Mo,1Tc", "Prospector Limpet Controller", "5", "Tiana Fortune,The Sarge"),
            new EngineeringRecipe("Shielded", "1WSE", "Prospector Limpet Controller", "1", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Shielded", "1C,1SE", "Prospector Limpet Controller", "2", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Shielded", "1C,1HDC", "Prospector Limpet Controller", "3", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Shielded", "1FPC,1SS,1V", "Prospector Limpet Controller", "4", "Tiana Fortune,The Sarge,Ram Tah"),
            new EngineeringRecipe("Shielded", "1CoS,1FCC,1W", "Prospector Limpet Controller", "5", "Tiana Fortune,The Sarge"),
            #endregion
            #region Pulse Laser
            new EngineeringRecipe("Efficient Weapon", "1S", "Pulse Laser", "1", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Efficient Weapon", "1HDP,1S", "Pulse Laser", "2", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Efficient Weapon", "1Cr,1ESED,1HE", "Pulse Laser", "3", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Efficient Weapon", "1HV,1IED,1Se", "Pulse Laser", "4", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Efficient Weapon", "1Cd,1PHR,1UED", "Pulse Laser", "5", "Broo Tarquin"),
            new EngineeringRecipe("Focused", "1Fe", "Pulse Laser", "1", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Focused", "1CCo,1Fe", "Pulse Laser", "2", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Focused", "1Cr,1CCe,1Fe", "Pulse Laser", "3", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Focused", "1FoC,1Ge,1PCa", "Pulse Laser", "4", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Focused", "1MSC,1Nb,1RFC", "Pulse Laser", "1", "Broo Tarquin"),
            new EngineeringRecipe("Light Weight", "1P", "Pulse Laser", "1", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Light Weight", "1Mn,1SAll", "Pulse Laser", "2", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Pulse Laser", "3", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Pulse Laser", "4", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Pulse Laser", "5", "Broo Tarquin"),
            new EngineeringRecipe("Long Range", "1S", "Pulse Laser", "1", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Long Range", "1MCF,1S", "Pulse Laser", "2", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Long Range", "1FoC,1MCF,1S", "Pulse Laser", "3", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Long Range", "1CPo,1FoC,1MCF", "Pulse Laser", "4", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Long Range", "1BiC,1CIF,1ThA", "Pulse Laser", "5", "Broo Tarquin"),
            new EngineeringRecipe("Overcharged", "1Ni", "Pulse Laser", "1", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Overcharged", "1CCo,1Ni", "Pulse Laser", "2", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Overcharged", "1CCo,1EA,1Ni", "Pulse Laser", "3", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Overcharged", "1CCe,1PCa,1Zn", "Pulse Laser", "4", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Overcharged", "1CPo,1EFW,1Zr", "Pulse Laser", "5", "Broo Tarquin"),
            new EngineeringRecipe("Rapid Fire", "1MS", "Pulse Laser", "1", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Rapid Fire", "1HDP,1MS", "Pulse Laser", "2", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Rapid Fire", "1ME,1PAll,1SLF", "Pulse Laser", "3", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Rapid Fire", "1MC,1MCF,1ThA", "Pulse Laser", "4", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Rapid Fire", "1CCom,1PAll,1Tc", "Pulse Laser", "5", "Broo Tarquin"),
            new EngineeringRecipe("Short Range Blaster","1Ni","Pulse Laser", "1", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Short Range Blaster","1MCF,1Ni","Pulse Laser", "2", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Short Range Blaster","1EA,1MCF,1Ni","Pulse Laser", "3", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Short Range Blaster","1CPo,1EA,1MCF","Pulse Laser", "4", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Short Range Blaster","1BiC,1CCom,1CIF","Pulse Laser", "5", "Broo Tarquin"),
            new EngineeringRecipe("Sturdy Mount", "1Ni", "Pulse Laser", "1", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Sturdy Mount", "1Ni,1SE", "Pulse Laser", "2", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Sturdy Mount", "1Ni,1SE,1W", "Pulse Laser", "3", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Sturdy Mount", "1Mo,1W,1Zn", "Pulse Laser", "4", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Sturdy Mount", "1HDC,1Mo,1Tc", "Pulse Laser", "5", "Broo Tarquin"),
            #endregion
            #region Rail Gun
            new EngineeringRecipe("High Capacity Magazine", "1MS", "Rail Gun", "1", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("High Capacity Magazine", "1MS,1V", "Rail Gun", "2", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("High Capacity Magazine", "1MS,1Nb,1V", "Rail Gun", "3", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("High Capacity Magazine", "1HDC,1ME,1Sn", "Rail Gun", "4", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("High Capacity Magazine", "1MC,1MSC,1FPC", "Rail Gun", "5", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("Light Weight", "1P", "Rail Gun", "1", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Light Weight", "1Mn,1SAll", "Rail Gun", "2", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Rail Gun", "3", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Rail Gun", "4", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Rail Gun", "5", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("Long Range", "1S", "Rail Gun", "1", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Long Range", "1MCF,1S", "Rail Gun", "2", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Long Range", "1FoC,1MCF,1S", "Rail Gun", "3", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Long Range", "1CPo,1FoC,1MCF", "Rail Gun", "4", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("Long Range", "1BiC,1CIF,1ThA", "Rail Gun", "5", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("Short Range Blaster", "1Ni", "Rail Gun", "1", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Short Range Blaster", "1MCF,1Ni", "Rail Gun", "2", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Short Range Blaster", "1EA,1MCF,1Ni", "Rail Gun", "3", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Short Range Blaster", "1CPo,1EA,1MCF", "Rail Gun", "4", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("Short Range Blaster", "1BiC,1CCom,1CIF", "Rail Gun", "5", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("Sturdy Mount", "1Ni", "Rail Gun", "1", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Sturdy Mount", "1SE,1Ni", "Rail Gun", "2", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Sturdy Mount", "1Ni,1SE,1W", "Rail Gun", "3", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Sturdy Mount", "1Mo,1W,1Zn", "Rail Gun", "4", "Tod \"The Blaster\" McQuinn"),
            new EngineeringRecipe("Sturdy Mount", "1HDC,1Mo,1Tc", "Rail Gun", "5", "Tod \"The Blaster\" McQuinn,The Sarge"),
            #endregion
            #region Refinery
            new EngineeringRecipe("Shielded", "1WSE", "Refinery", "1", "Lori Jameson,Bill Turner"),
            new EngineeringRecipe("Shielded", "1C,1SE", "Refinery", "2", "Lori Jameson,Bill Turner"),
            new EngineeringRecipe("Shielded", "1C,1SE,1HDC", "Refinery", "3", "Lori Jameson,Bill Turner"),
            new EngineeringRecipe("Shielded", "1FPC,1SS,1V", "Refinery", "4", "Lori Jameson"),
            #endregion
            #region Seeker Missile Rack
            new EngineeringRecipe("High Capacity Magazine", "1MS", "Seeker Missile Rack", "1", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("High Capacity Magazine", "1MS,1V", "Seeker Missile Rack", "2", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("High Capacity Magazine", "1MC,1V,1Nb", "Seeker Missile Rack", "3", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("High Capacity Magazine", "1ME,1HDC,1Sn", "Seeker Missile Rack", "4", "Liz Ryder"),
            new EngineeringRecipe("High Capacity Magazine", "1MC,1FPC,1MSC", "Seeker Missile Rack", "5", "Liz Ryder"),
            new EngineeringRecipe("Light Weight", "1P", "Seeker Missile Rack", "1", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Light Weight", "1SAll,1Mn", "Seeker Missile Rack", "2", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Light Weight", "1SAll,1Mn,1CCe", "Seeker Missile Rack", "3", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Seeker Missile Rack", "4", "Liz Ryder"),
            new EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Seeker Missile Rack", "5", "Liz Ryder"),
            new EngineeringRecipe("Rapid Fire", "1MS", "Seeker Missile Rack", "1", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Rapid Fire", "1MS,1HDP", "Seeker Missile Rack", "2", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Rapid Fire", "1SLF,1ME,1PAll", "Seeker Missile Rack", "3", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Rapid Fire", "1MCF,1MC,1ThA", "Seeker Missile Rack", "4", "Liz Ryder"),
            new EngineeringRecipe("Rapid Fire", "1PAll,1CCom,1Tc", "Seeker Missile Rack", "5", "Liz Ryder"),
            new EngineeringRecipe("Sturdy Mount", "1Ni", "Seeker Missile Rack", "1", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Sturdy Mount", "1Ni,1SE", "Seeker Missile Rack", "2", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Sturdy Mount", "1Ni,1SE,1W", "Seeker Missile Rack", "3", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Sturdy Mount", "1Zn,1W,1Mo", "Seeker Missile Rack", "4", "Liz Ryder"),
            new EngineeringRecipe("Sturdy Mount", "1HDC,1Mo,1Tc", "Seeker Missile Rack", "5", "Liz Ryder"),
            #endregion
            #region Sensors
            new EngineeringRecipe("Light Weight", "1P", "Sensors", "1", "Hera Tani,Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new EngineeringRecipe("Light Weight", "1Mn,1SAll", "Sensors", "2", "Hera Tani,Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Sensors", "3", "Hera Tani,Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Sensors", "4", "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Sensors", "5", "Juri Ishmaak,Lori Jameson,Lei Cheung,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Long Range", "1Fe", "Sensors", "1", "Hera Tani,Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new EngineeringRecipe("Long Range", "1HC,1Fe", "Sensors", "2", "Hera Tani,Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new EngineeringRecipe("Long Range", "1HC,1Fe,1UED", "Sensors", "3", "Hera Tani,Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new EngineeringRecipe("Long Range", "1DED,1EA,1Ge", "Sensors", "4", "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Long Range", "1CED,1Nb,1PCa", "Sensors", "5", "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Wide Angle", "1MS", "Sensors", "1", "Hera Tani,Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new EngineeringRecipe("Wide Angle", "1Ge,1MS", "Sensors", "2", "Hera Tani,Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new EngineeringRecipe("Wide Angle", "1CSD,1Ge,1MS", "Sensors", "3", "Hera Tani,Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new EngineeringRecipe("Wide Angle", "1DSD,1ME,1Nb", "Sensors", "4", "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Wide Angle", "1CFSD,1MC,1Sn", "Sensors", "5", "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner"),
            #endregion
            #region Shield Booster
            new EngineeringRecipe("Blast Resistant", "1Fe", "Shield Booster", "1", "Didi Vatermann,Felicity Farseer,Lei Cheung"),
            new EngineeringRecipe("Blast Resistant", "1CCo,1Fe", "Shield Booster", "2", "Didi Vatermann,Lei Cheung"),
            new EngineeringRecipe("Blast Resistant", "1CCo,1FoC,1Fe", "Shield Booster", "3", "Didi Vatermann,Lei Cheung"),
            new EngineeringRecipe("Blast Resistant", "1Ge,1RFC,1USS", "Shield Booster", "4", "Didi Vatermann"),
            new EngineeringRecipe("Blast Resistant", "1ASPA,1EFC,1Nb", "Shield Booster", "5", "Didi Vatermann"),
            new EngineeringRecipe("Heavy Duty", "1GR", "Shield Booster", "1", "Didi Vatermann,Felicity Farseer,Lei Cheung"),
            new EngineeringRecipe("Heavy Duty", "1DSCR,1HC", "Shield Booster", "2", "Didi Vatermann,Lei Cheung"),
            new EngineeringRecipe("Heavy Duty", "1DSCR,1HC,1Nb", "Shield Booster", "3", "Didi Vatermann,Lei Cheung"),
            new EngineeringRecipe("Heavy Duty", "1EA,1ISSA,1Sn", "Shield Booster", "4", "Didi Vatermann"),
            new EngineeringRecipe("Heavy Duty", "1Sb,1PCa,1USS", "Shield Booster", "5", "Didi Vatermann"),
            new EngineeringRecipe("Kinetic Resistant", "1Fe", "Shield Booster", "1", "Didi Vatermann,Felicity Farseer,Lei Cheung"),
            new EngineeringRecipe("Kinetic Resistant", "1Ge,1GR", "Shield Booster", "2", "Didi Vatermann,Lei Cheung"),
            new EngineeringRecipe("Kinetic Resistant", "1FoC,1HC,1SAll", "Shield Booster", "3", "Didi Vatermann,Lei Cheung"),
            new EngineeringRecipe("Kinetic Resistant", "1GA,1RFC,1USS", "Shield Booster", "4", "Didi Vatermann"),
            new EngineeringRecipe("Kinetic Resistant", "1ASPA,1EFC,1PA", "Shield Booster", "5", "Didi Vatermann"),
            new EngineeringRecipe("Resistance Augmented", "1P", "Shield Booster", "1", "Didi Vatermann,Felicity Farseer,Lei Cheung"),
            new EngineeringRecipe("Resistance Augmented", "1CCo,1P", "Shield Booster", "2", "Didi Vatermann,Lei Cheung"),
            new EngineeringRecipe("Resistance Augmented", "1CCo,1FoC,1P", "Shield Booster", "3", "Didi Vatermann,Lei Cheung"),
            new EngineeringRecipe("Resistance Augmented", "1CCe,1Mn,1RFC", "Shield Booster", "4", "Didi Vatermann"),
            new EngineeringRecipe("Resistance Augmented", "1CCe,1IS,1RFC", "Shield Booster", "5", "Didi Vatermann"),
            new EngineeringRecipe("Thermal Resistant", "1Fe", "Shield Booster", "1", "Didi Vatermann,Felicity Farseer,Lei Cheung"),
            new EngineeringRecipe("Thermal Resistant", "1Ge,1HCW", "Shield Booster", "2", "Didi Vatermann,Lei Cheung"),
            new EngineeringRecipe("Thermal Resistant", "1FoC,1HCW,1HDP", "Shield Booster", "3", "Didi Vatermann,Lei Cheung"),
            new EngineeringRecipe("Thermal Resistant", "1HDP,1RFC,1USS", "Shield Booster", "4", "Didi Vatermann"),
            new EngineeringRecipe("Thermal Resistant", "1ASPA,1EFC,1HE", "Shield Booster", "5", "Didi Vatermann"),
            #endregion
            #region Shield Cell Bank
            new EngineeringRecipe("Rapid Charge", "1S", "Shield Cell Bank", "1", "Elvira Martuuk,Lori Jameson"),
            new EngineeringRecipe("Rapid Charge", "1Cr,1GR", "Shield Cell Bank", "2", "Lori Jameson"),
            new EngineeringRecipe("Rapid Charge", "1HC,1PAll,1S", "Shield Cell Bank", "3", "Lori Jameson"),
            new EngineeringRecipe("Rapid Charge", "1Cr,1EA,1ThA", "Shield Cell Bank", "4", "Lori Jameson"),
            new EngineeringRecipe("Specialised", "1SLF", "Shield Cell Bank", "1", "Elvira Martuuk,Lori Jameson"),
            new EngineeringRecipe("Specialised", "1CCo,1SLF", "Shield Cell Bank", "2", "Lori Jameson"),
            new EngineeringRecipe("Specialised", "1CCo,1CIF,1IED", "Shield Cell Bank", "3", "Lori Jameson"),
            #endregion
            #region Shield Generator
            new EngineeringRecipe("Enhanced, Low Power", "1DSCR", "Shield Generator", "1", "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new EngineeringRecipe("Enhanced, Low Power", "1DSCR,1Ge", "Shield Generator", "2", "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new EngineeringRecipe("Enhanced, Low Power", "1DSCR,1Ge,1PAll", "Shield Generator", "3", "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new EngineeringRecipe("Enhanced, Low Power", "1ISSA,1Nb,1ThA", "Shield Generator", "4", "Lei Cheung"),
            new EngineeringRecipe("Enhanced, Low Power", "1MGA,1Sn,1USS", "Shield Generator", "5", "Lei Cheung"),
            new EngineeringRecipe("Kinetic Resistant", "1DSCR", "Shield Generator", "1", "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new EngineeringRecipe("Kinetic Resistant", "1DSCR,1MCF", "Shield Generator", "2", "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new EngineeringRecipe("Kinetic Resistant", "1DSCR,1MCF,1Se", "Shield Generator", "3", "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new EngineeringRecipe("Kinetic Resistant", "1FoC,1ISSA,1Hg", "Shield Generator", "4", "Lei Cheung"),
            new EngineeringRecipe("Kinetic Resistant", "1RFC,1Ru,1USS", "Shield Generator", "5", "Lei Cheung"),
            new EngineeringRecipe("Reinforced", "1P", "Shield Generator", "1", "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new EngineeringRecipe("Reinforced", "1CCo,1P", "Shield Generator", "2", "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new EngineeringRecipe("Reinforced", "1CCo,1MC,1P", "Shield Generator", "3", "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new EngineeringRecipe("Reinforced", "1CCe,1CCom,1Mn", "Shield Generator", "4", "Lei Cheung"),
            new EngineeringRecipe("Reinforced", "1As,1CPo,1IC", "Shield Generator", "5", "Lei Cheung"),
            new EngineeringRecipe("Thermal Resistant", "1DSCR", "Shield Generator", "1", "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new EngineeringRecipe("Thermal Resistant", "1DSCR,1Ge", "Shield Generator", "2", "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new EngineeringRecipe("Thermal Resistant", "1DSCR,1Ge,1Se", "Shield Generator", "3", "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new EngineeringRecipe("Thermal Resistant", "1FoC,1ISSA,1Hg", "Shield Generator", "4", "Lei Cheung"),
            new EngineeringRecipe("Thermal Resistant", "1RFC,1Ru,1USS", "Shield Generator", "5", "Lei Cheung"),
            #endregion
            #region Thrusters
            new EngineeringRecipe("Clean Drive Tuning", "1S","Thrusters", "1", "Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Clean Drive Tuning", "1CCo,1SLF","Thrusters", "2", "Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Clean Drive Tuning", "1CCo,1SLF,1UED","Thrusters", "3", "Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Clean Drive Tuning", "1CCe,1DED,1MCF","Thrusters", "4", "Professor Palin"),
            new EngineeringRecipe("Clean Drive Tuning", "1CED,1CCe,1Sn","Thrusters", "5", "Professor Palin"),
            new EngineeringRecipe("Dirty Drive Tuning", "1SLF","Thrusters", "1", "Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Dirty Drive Tuning", "1ME,1SLF","Thrusters", "2", "Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Dirty Drive Tuning", "1Cr,1MC,1SLF","Thrusters", "3", "Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Dirty Drive Tuning", "1CCom,1MCF,1Se","Thrusters", "4", "Professor Palin"),
            new EngineeringRecipe("Dirty Drive Tuning", "1Cd,1CIF,1PI","Thrusters", "5", "Professor Palin"),
            new EngineeringRecipe("Drive Strengthening", "1C","Thrusters", "1", "Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Drive Strengthening", "1HCW,1V","Thrusters", "2", "Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Drive Strengthening", "1HCW,1SS,1V","Thrusters", "3", "Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Drive Strengthening", "1CoS,1HDP,1HDC","Thrusters", "4", "Professor Palin"),
            new EngineeringRecipe("Drive Strengthening", "1HE,1IS,1FPC","Thrusters", "5", "Professor Palin"),
            #endregion
            #region Torpedo Pylon
            new EngineeringRecipe("Light Weight", "1P", "Torpedo Pylon", "1", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Light Weight", "1Mn,1SAll", "Torpedo Pylon", "2", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Torpedo Pylon", "3", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Torpedo Pylon", "4", "Liz Ryder"),
            new EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Torpedo Pylon", "5", "Liz Ryder"),
            new EngineeringRecipe("Sturdy Mount", "1Ni", "Torpedo Pylon", "1", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Sturdy Mount", "1Ni,1SE", "Torpedo Pylon", "2", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Sturdy Mount", "1Ni,1SE,1W", "Torpedo Pylon", "3", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Sturdy Mount", "1Mo,1W,1Zn", "Torpedo Pylon", "4", "Liz Ryder"),
            new EngineeringRecipe("Sturdy Mount", "1HDC,1Mo,1Tc", "Torpedo Pylon", "5", "Liz Ryder"),
            #endregion
            #region Wake Scanner
            new EngineeringRecipe("Fast Scan", "1P", "Wake Scanner", "1", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Fast Scan", "1FFC,1P", "Wake Scanner", "2", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Fast Scan", "1FFC,1OSK,1P", "Wake Scanner", "3", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Fast Scan", "1AEA,1FoC,1Mn", "Wake Scanner", "4", "Tiana Fortune"),
            new EngineeringRecipe("Fast Scan", "1AEC,1As,1RFC", "Wake Scanner", "5", "Tiana Fortune"),
            new EngineeringRecipe("Light Weight", "1P", "Wake Scanner", "1", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Light Weight", "1Mn,1SAll", "Wake Scanner", "2", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Wake Scanner", "3", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Wake Scanner", "4", "Tiana Fortune"),
            new EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Wake Scanner", "5", "Tiana Fortune"),
            new EngineeringRecipe("Long Range", "1Fe", "Wake Scanner", "1", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Long Range", "1HC,1Fe", "Wake Scanner", "2", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Long Range", "1HC,1Fe,1UED", "Wake Scanner", "3", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Long Range", "1DED,1EA,1Ge", "Wake Scanner", "4", "Tiana Fortune"),
            new EngineeringRecipe("Long Range", "1CED,1Nb,1PCa", "Wake Scanner", "5", "Tiana Fortune"),
            new EngineeringRecipe("Reinforced", "1Ni", "Wake Scanner", "1", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Reinforced", "1Ni,1SE", "Wake Scanner", "2", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Reinforced", "1Ni,1SE,1W", "Wake Scanner", "3", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Reinforced", "1Mo,1W,1Zn", "Wake Scanner", "4", "Tiana Fortune"),
            new EngineeringRecipe("Reinforced", "1HDC,1Mo,1Tc", "Wake Scanner", "5", "Tiana Fortune"),
            new EngineeringRecipe("Shielded", "1WSE", "Wake Scanner", "1", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Shielded", "1C,1SE", "Wake Scanner", "2", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Shielded", "1C,1SE,1HDC", "Wake Scanner", "3", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Shielded", "1FPC,1SS,1V", "Wake Scanner", "4", "Tiana Fortune"),
            new EngineeringRecipe("Shielded", "1CoS,1FCC,1W", "Wake Scanner", "5", "Tiana Fortune"),
            new EngineeringRecipe("Wide Angle", "1MS", "Wake Scanner", "1", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Wide Angle", "1Ge,1MS", "Wake Scanner", "2", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Wide Angle", "1CSD,1Ge,1MS", "Wake Scanner", "3", "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new EngineeringRecipe("Wide Angle", "1DSD,1ME,1Nb", "Wake Scanner", "4", "Tiana Fortune"),
            new EngineeringRecipe("Wide Angle", "1CFSD,1MC,1Sn", "Wake Scanner", "5", "Tiana Fortune"),
            #endregion
            #region Experimental Effects
            new EngineeringRecipe("Angled Plating", "5CC,3HDC,3Zr", "Armor", "Experimental", "Liz Ryder,Selene Jean"),
            new EngineeringRecipe("Angled Plating", "5TeA,3Zr,5C,3HDC", "Hull Reinforcement Package", "Experimental", "Liz Ryder,Selene Jean"),
            new EngineeringRecipe("Auto Loader", "4ME,3MC,3HDC", "Cannon,Multi-Cannon", "Experimental",  "Tod \"The Blaster\" McQuinn,The Sarge,Zacariah Nemo"),
            new EngineeringRecipe("Blast Block", "5ISSA,3HRC,3HDP,2Se", "Shield Booster", "Experimental", "Didi Vatermann,Felicity Farseer,Lei Cheung"),
            new EngineeringRecipe("Boss Cells", "5CSU,3Cr,1PCa", "Shield Cell Bank", "Experimental", "Elvira Martuuk,Lori Jameson"),
            new EngineeringRecipe("Cluster Capacitors", "5P,3HRC,1Cd", "Power Distributor", "Experimental", "Hera Tani,Marco Qwent,The Dweller"),
            new EngineeringRecipe("Concordant Sequence", "5FoC,3EFW,1Zr", "Beam Laser,Burst Laser,Pulse Laser", "Experimental", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Corrosive Shell", "5CSU,4PAll,3As", "Fragment Cannon,Multi-cannon", "Experimental", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Dazzle Shell", "5MS,4Mn,5HC","Fragment Cannon,Plasma Accelerator", "Experimental", "Tod \"The Blaster\" McQuinn,Zacariah Nemo,Bill Turner"),
            new EngineeringRecipe("Deep Charge", "5ADWE,3GA,1EHT", "Frame Shift Drive", "Experimental", "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Deep Plating", "5CC,3ME,2Mo", "Armor", "Experimental", "Liz Ryder,Selene Jean"),
            new EngineeringRecipe("Deep Plating", "5CC,3Mo,2Ru", "Hull Reinforcement Package", "Experimental", "Liz Ryder,Selene Jean"),
            new EngineeringRecipe("Dispersal Field", "5CCo,5HC,5IED,5WSE", "Plasma Accelerator,Cannon", "Experimental", "Tod \"The Blaster\" McQuinn,Zacariah Nemo,Bill Turner"),
            new EngineeringRecipe("Double Braced", "5WSE,3FFC,1CCom", "Shield Generator", "Experimental", "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new EngineeringRecipe("Double Braced", "5Fe,3HC,1FPC", "Thrusters", "Experimental", "Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Double Braced", "5ADWE,3GA,1CCom", "Frame Shift Drive", "Experimental", "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Double Braced", "5GR,3V,1FPC", "Power Plant", "Experimental", "Felicity Farseer,Hera Tani,Marco Qwent"),
            new EngineeringRecipe("Double Braced", "5P,3HRC,1FPC", "Power Distributor", "Experimental", "Hera Tani,Marco Qwent,The Dweller"),
            new EngineeringRecipe("Double Braced", "5CSU,3Cr,1Y", "Shield Cell Bank", "Experimental", "Elvira Martuuk,Lori Jameson"),
            new EngineeringRecipe("Double Braced", "5MS,5CC,3V", 
                "Beam Laser,Burst Laser,Pulse Laser,Mulit-cannon,Plasma Accelerator,Fragment Cannon,Rail Gun,Mine Launcher,Torpedo Pylon", "Experimental", 
                "Tod \"The Blaster\" McQuinn,The Sarge,Zacariah Nemo,Broo Tarquin,The Dweller,Bill Turner,Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Double Braced", "5DSCR,3GA,3SE", "Shield Booster", "Experimental", "Didi Vatermann,Felicity Farseer,Lei Cheung"),
            new EngineeringRecipe("Drag Drives", "5Fe,3HC,1SFP", "Thrusters", "Experimental", "Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Drag Munitions", "5C,5GR,2Mo", "Fragment Cannon,Seeker Missile Rack", "Experimental", "Juri Ishmaak,Liz Ryder,Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Drive Distributors", "5Fe,3HC,1SFP", "Thrusters", "Experimental", "Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Emissive Munitions", "4ME,3UED,3HE,3Mn", 
                "Pulse Laser,Multi-Cannon,Seeker Missile Rack,Missile Rack,Mine Launcher", "Experimental",
                "Broo Tarquin,The Dweller,Tod \"The Blaster\" McQuinn,Zacariah Nemo,Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Fast Charge", "5WSE,3FFC,1CoS", "Shield Generator", "Experimental", "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new EngineeringRecipe("Feedback Cascade", "5OSK,5SE,5FiC", "Rail Gun", "Experimental", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Flow Control", "5P,3HRC,1CPo", "Power Distributor", "Experimental", "Hera Tani,Marco Qwent,The Dweller"),
            new EngineeringRecipe("Flow Control", "5CSU,4Cr,1CPo", "Shield Cell Bank", "Experimental", "Elvira Martuuk,Lori Jameson"),
            new EngineeringRecipe("Flow Control", "5MS,3HC,1EFW",
                "Beam Laser,Burst Laser,Pulse Laser,Mulit-cannon,Plasma Accelerator,Fragment Cannon,Rail Gun,Mine Launcher,Torpedo Pylon", "Experimental",
                "Tod \"The Blaster\" McQuinn,The Sarge,Zacariah Nemo,Broo Tarquin,The Dweller,Bill Turner,Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Flow Control", "5ISSA,3SFP,3FoC,3Nb", "Shield Booster", "Experimental", "Didi Vatermann,Felicity Farseer,Lei Cheung"),
            new EngineeringRecipe("Force Block", "5WSE,3FFC,1DED", "Shield Generator", "Experimental", "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new EngineeringRecipe("Force Block", "5USA,3SS,2ASPA", "Shield Booster", "Experimental", "Didi Vatermann,Felicity Farseer,Lei Cheung"),
            new EngineeringRecipe("Force Shell", "5MS,5Zn,3PA,3HCW", "Cannon", "Experimental", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("FSD Interrupt", "3SWS,5AFT,5ME,3CCom", "Missile Rack", "Experimental", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Hi-Cap", "5WSE,3FFC,1CPo", "Shield Generator", "Experimental", "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new EngineeringRecipe("High Yield Shell", "5MS,3PLA,3CM,5Ni", "Cannon", "Experimental", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Incendiary Rounds", "5HCW,5P,5S,3PA", "Fragment Cannon,Multi-cannon", "Experimental", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Inertial Impact", "5FFC,5DSCR,5ADWE", "Burst Laser", "Experimental", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Ion Disruption", "5S,5P,3CD,3EA", "Mine Launcher", "Experimental", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Layered Plating", "5HCW,3HDC,1Nb", "Armor", "Experimental", "Liz Ryder,Selene Jean"),
            new EngineeringRecipe("Layered Plating", "5HCW,3SS,3W", "Hull Reinforcement Package", "Experimental", "Liz Ryder,Selene Jean"),
            new EngineeringRecipe("Lo-Draw", "5WSE,3FFC,1CPo", "Shield Generator", "Experimental", "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new EngineeringRecipe("Mass Lock Munition", "5ME,3HDC,3ASPA", "Torpedo Pylon", "Experimental", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Mass Manager", "5ADWE,3GA,1EHT", "Frame Shift Drive", "Experimental", "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Monstered", "5GR,3V,1PCa", "Power Plant", "Experimental", "Felicity Farseer,Hera Tani,Marco Qwent"),
            new EngineeringRecipe("Multi-servos", "5MS,4FoC,2CPo,2CCom", 
                "Burst Laser,Pulse Laser,Mulit-cannon,Plasma Accelerator,Fragment Cannon,Rail Gun,Cannon", "Experimental",
                "Tod \"The Blaster\" McQuinn,The Sarge,Zacariah Nemo,Broo Tarquin,The Dweller,Bill Turner,Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Multi-weave", "5WSE,3FFC,1ASPA", "Shield Generator", "Experimental", "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new EngineeringRecipe("Overload Munitions", "5FiC,4TEC,2ASPA,3Ge", "Seeker Missile Rack,Missile Rack,Mine Launcher", "Experimental", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Oversized", "5MS,3MC,1Ru",
                "Beam Laser,Burst Laser,Pulse Laser,Mulit-cannon,Plasma Accelerator,Fragment Cannon,Rail Gun,Mine Launcher,Torpedo Pylon", "Experimental",
                "Tod \"The Blaster\" McQuinn,The Sarge,Zacariah Nemo,Broo Tarquin,The Dweller,Bill Turner,Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Penetrator Munitions", "5GA,3EA,3Zr", "Missile Rack", "Experimental", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Penetrator Payload", "3MC,3W,5ABSD,3Se", "Torpedo Pylon", "Experimental", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Phasing Sequence", "5FoC,3ASPA,4Nb,3CCom", "Pulse Laser,Burst Laser,Plasma Accelerator", "Experimental", "Broo Tarquin,The Dweller,Zacariah Nemo,Bill Turner"),
            new EngineeringRecipe("Plasma Slug", "3HE,2EFW,2RFC,4Hg", "Plasma Accelerator,Rail Gun", "Experimental", "Zacariah Nemo,Bill Turner,Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Radiant Canister", "1Po,3PA,4HDP", "Mine Launcher", "Experimental", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Recycling Cell", "5CSU,3Cr,1CCom", "Shield Cell Bank", "Experimental", "Elvira Martuuk,Lori Jameson"),
            new EngineeringRecipe("Reflective Plating", "5CC,3HDP,2ThA", "Armor", "Experimental", "Liz Ryder,Selene Jean"),
            new EngineeringRecipe("Reflective Plating", "5HCW,3HDP,1PLA,4Zn", "Hull Reinforcement Package", "Experimental", "Liz Ryder,Selene Jean"),
            new EngineeringRecipe("Regeneration Sequence", "3RFC,4SS,1PSFD", "Beam Laser", "Experimental", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Reverberating Cascade", "2CCom,3CSD,4FiC,4Cr", "Mine Launcher,Torpedo Pylon", "Experimental", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Scramble Spectrum", "5CS,3USS,5ESED", "Burst Laser,Pulse Laser", "Experimental", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Screening Shell", "5MS,5DSCR,5MCF,3Nb", "Fragment Cannon", "Experimental", "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Shift-lock Canister", "5TeA,3SWS,5SAll", "Mine Launcher", "Experimental", "Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Smart Rounds", "5MS,3SFP,3DED,3CSD", "Cannon,Multi-Cannon", "Experimental", "Tod \"The Blaster\" McQuinn,Zacariah Nemo,The Sarge"),
            new EngineeringRecipe("Stripped Down", "5WSE,3FFC,1PLA", "Shield Generator", "Experimental", "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new EngineeringRecipe("Stripped Down", "5Fe,3HC,1PLA", "Thrusters", "Experimental", "Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Stripped Down", "5ADWE,3GA,1PLA", "Frame Shift Drive", "Experimental", "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Stripped Down", "5GR,3V,1PLA", "Power Plant", "Experimental", "Felicity Farseer,Hera Tani,Marco Qwent"),
            new EngineeringRecipe("Stripped Down", "5P,3HRC,1PLA", "Power Distributor", "Experimental", "Hera Tani,Marco Qwent,The Dweller"),
            new EngineeringRecipe("Stripped Down", "5CSU,3Cr,1PLA", "Shield Cell Bank", "Experimental", "Elvira Martuuk,Lori Jameson"),
            new EngineeringRecipe("Stripped Down", "5SAll,5C,1Sn",
                "Beam Laser,Burst Laser,Pulse Laser,Mulit-cannon,Plasma Accelerator,Fragment Cannon,Rail Gun,Mine Launcher,Torpedo Pylon", "Experimental",
                "Tod \"The Blaster\" McQuinn,The Sarge,Zacariah Nemo,Broo Tarquin,The Dweller,Bill Turner,Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Super Capacitors", "3USS,5CC,2Cd", "Shield Booster", "Experimental", "Didi Vatermann,Felicity Farseer,Lei Cheung"),
            new EngineeringRecipe("Super Conduits", "5P,3HRC,1SFP", "Power Distributor", "Experimental", "Hera Tani,Marco Qwent,The Dweller"),
            new EngineeringRecipe("Super Penetrator", "3PLA,3RFC,3Zr,5USS", "Rail Gun", "Experimental", "Tod \"The Blaster\" McQuinn,The Sarge"),
            new EngineeringRecipe("Target Lock Breaker", "5Se,3SFP,1AEC", "Plasma Accelerator", "Experimental", "Zacariah Nemo,Bill Turner"),
            new EngineeringRecipe("Thermal Cascade", "5HCW,4HC,3HDC,5P", "Cannon,Seeker Missile Rack,Missile Rack", "Experimental", "Tod \"The Blaster\" McQuinn,The Sarge,Juri Ishmaak,Liz Ryder"),
            new EngineeringRecipe("Thermal Conduit", "5HDP,5S,5TeA", "Beam Laser,Plasma Accelerator", "Experimental", "Zacariah Nemo,Bill Turner,Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Thermal Shock", "5FFC,3HRC,3CCo,3W", "Beam Laser,Burst Laser,Pulse Laser,Mulit-cannon", "Experimental", "Broo Tarquin,The Dweller,Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new EngineeringRecipe("Thermal Spread", "5Fe,3HC,1HV", "Thrusters", "Experimental", "Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Thermal Spread", "5ADWE,3GA,1HV,3GR", "Frame Shift Drive", "Experimental", "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new EngineeringRecipe("Thermal Spread", "5GR,3V,1HV", "Power Plant", "Experimental", "Felicity Farseer,Hera Tani,Marco Qwent"),
            new EngineeringRecipe("Thermal Vent", "5FFC,3CPo,3PAll", "Beam Laser", "Experimental", "Broo Tarquin,The Dweller"),
            new EngineeringRecipe("Thermo Block", "5WSE,3FFC,1HV", "Shield Generator", "Experimental", "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new EngineeringRecipe("Thermo Block", "5ABSD,3CCe,3HV", "Shield Booster", "Experimental", "Didi Vatermann,Felicity Farseer,Lei Cheung"),
            #endregion
        };

        public static List<TechBrokerUnlockRecipe> TechBrokerUnlocks = new List<TechBrokerUnlockRecipe>()
        {
            new TechBrokerUnlockRecipe("Corrosion Resistant Cargo Rack", "30Fe,18CM,30RB,12NFI,28MA"),
            new TechBrokerUnlockRecipe("Enzyme Missile Rack (Fixed)", "30UKEC,36UKOC,34Mo,35W,6RB"),
            new TechBrokerUnlockRecipe("Guardian Gauss Cannon (Fixed)", "4GWBS,36GPCe,42GTC,30Mn,10MEC"),
            new TechBrokerUnlockRecipe("Guardian Plasma Charger (Fixed)", "8GWBS,38GPC,34GSWP,28Cr,12MWCH"),
            new TechBrokerUnlockRecipe("Guardian Plasma Charger (Turreted)", "10GWBS,42GPC,39GSWP,33Cr,10AM"),
            new TechBrokerUnlockRecipe("Guardian Power Plant", "4GMBS,36GPC,42PEOD,30HRC,10EGA"),
            new TechBrokerUnlockRecipe("Meta Allow Hull Reinforcement", "30FoC,26ASPA,20CCom,12RMP,28MA"),
            new TechBrokerUnlockRecipe("Remote Release Flechette Launcher (Turreted)", "36Fe,32Mo,28Re,34Ge,10AM"),
            new TechBrokerUnlockRecipe("Remote Release Flechette Launcher (Fixed)", "40Fe,36Mo,28Re,32Ge,8CMMC"),
            new TechBrokerUnlockRecipe("Shock Cannon (Turreted)", "30V,32W,3Re,28Tc,8PTB"),
            new TechBrokerUnlockRecipe("Shock Cannon (Gimballed)", "36V,32W,30Re,28Tc,10PC"),
            new TechBrokerUnlockRecipe("Shock Cannon (Fixed)", "30V,30W,36Re,30Tc,6ID"),
        };
    }

    public class RecipeFilterSelector
    {
        ExtendedControls.CheckedListControlCustom cc;
        string dbstring;
        public event EventHandler Changed;

        private int reserved = 1;

        private List<string> options;

        public RecipeFilterSelector(List<string> opts)
        {
            options = opts;
        }

        public void FilterButton(string db, Control ctr, Color back, Color fore, Form parent)
        {
            FilterButton(db, ctr, back, fore, parent, options);
        }

        public void FilterButton(string db, Control ctr, Color back, Color fore, Form parent, List<string> list)
        {
            FilterButton(db, ctr.PointToScreen(new Point(0, ctr.Size.Height)), new Size(ctr.Width * 2, 400), back, fore, parent, list);
        }

        public void FilterButton(string db, Point p, Size s, Color back, Color fore, Form parent)
        {
            FilterButton(db, p, s, back, fore, parent, options);
        }

        public void FilterButton(string db, Point p, Size s, Color back, Color fore, Form parent, List<string> list)
        {
            if (cc == null)
            {
                dbstring = db;
                cc = new ExtendedControls.CheckedListControlCustom();
                cc.Items.Add("All");
                cc.Items.Add("None");

                cc.Items.AddRange(list.ToArray());

                cc.SetChecked(SQLiteDBClass.GetSettingString(dbstring, "All"));
                SetFilterSet();

                cc.FormClosed += FilterClosed;
                cc.CheckedChanged += FilterCheckChanged;
                cc.PositionSize(p, s);
                cc.SetColour(back, fore);
                cc.Show(parent);
            }
            else
                cc.Close();
        }

        private void SetFilterSet()
        {
            string list = cc.GetChecked(reserved);
            cc.SetChecked(list.Equals("All"), 0, 1);
            cc.SetChecked(list.Equals("None"), 1, 1);

        }

        private void FilterCheckChanged(Object sender, ItemCheckEventArgs e)
        {
            //Console.WriteLine("Changed " + e.Index);

            cc.SetChecked(e.NewValue == CheckState.Checked, e.Index, 1);        // force check now (its done after it) so our functions work..

            if (e.Index == 0 && e.NewValue == CheckState.Checked)
                cc.SetChecked(true, reserved);

            if (e.Index == 1 && e.NewValue == CheckState.Checked)
                cc.SetChecked(false, reserved);

            SetFilterSet();
        }

        private void FilterClosed(Object sender, FormClosedEventArgs e)
        {
            SQLiteDBClass.PutSettingString(dbstring, cc.GetChecked(2));
            cc = null;

            if (Changed != null)
                Changed(sender, e);
        }
    }
}