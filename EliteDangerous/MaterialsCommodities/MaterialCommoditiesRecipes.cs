/*
 * Copyright © 2016-2018 EDDiscovery development team
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

using System;
using System.Collections.Generic;

namespace EliteDangerousCore
{
    // helper to handle recipe data with material commodities lists

    public static class MaterialCommoditiesRecipe
    {
        static public void ResetUsed(List<MaterialCommodities> mcl)
        {
            for (int i = 0; i < mcl.Count; i++)
                mcl[i].scratchpad = mcl[i].Count;
        }

        //return maximum can make, how many made, needed string.
        static public Tuple<int, int, string, string> HowManyLeft(List<MaterialCommodities> list, Recipes.Recipe r, int tomake = 0)
        {
            int max = int.MaxValue;
            System.Text.StringBuilder needed = new System.Text.StringBuilder(64);
            System.Text.StringBuilder neededlong = new System.Text.StringBuilder(64);

            for (int i = 0; i < r.ingredients.Length; i++)
            {
                string ingredient = r.ingredients[i];

                int mi = list.FindIndex(x => x.Details.Shortname.Equals(ingredient));
                int got = (mi >= 0) ? list[mi].scratchpad : 0;
                int sets = got / r.count[i];

                max = Math.Min(max, sets);

                int need = r.count[i] * tomake;

                if (got < need)
                {
                    string dispshort;
                    string displong;
                    if (mi > 0)     // if got one..
                    {
                        dispshort = (list[mi].Details.IsEncodedOrManufactured) ? " " + list[mi].Details.Name : list[mi].Details.Shortname;
                        displong = " " + list[mi].Details.Name;
                    }
                    else
                    {
                        MaterialCommodityData db = MaterialCommodityData.GetByShortName(ingredient);
                        dispshort = (db.Category == MaterialCommodityData.MaterialEncodedCategory || db.Category == MaterialCommodityData.MaterialManufacturedCategory) ? " " + db.Name : db.Shortname;
                        displong = " " + db.Name;
                    }

                    string sshort = (need - got).ToStringInvariant() + dispshort;
                    string slong = (need - got).ToStringInvariant() + " x " + displong + Environment.NewLine;

                    if (needed.Length == 0)
                    {
                        needed.Append("Need:" + sshort);
                        neededlong.Append("Need:" + Environment.NewLine + slong);
                    }
                    else
                    {
                        needed.Append("," + sshort);
                        neededlong.Append(slong);
                    }
                }
            }

            int made = 0;

            if (max > 0 && tomake > 0)             // if we have a set, and use it up
            {
                made = Math.Min(max, tomake);                // can only make this much
                System.Text.StringBuilder usedstrshort = new System.Text.StringBuilder(64);
                System.Text.StringBuilder usedstrlong = new System.Text.StringBuilder(64);

                for (int i = 0; i < r.ingredients.Length; i++)
                {
                    int mi = list.FindIndex(x => x.Details.Shortname.Equals(r.ingredients[i]));
                    System.Diagnostics.Debug.Assert(mi != -1);
                    int used = r.count[i] * made;
                    list[mi].scratchpad -= used;

                    string dispshort = (list[mi].Details.IsEncodedOrManufactured) ? " " + list[mi].Details.Name : list[mi].Details.Shortname;
                    string displong = " " + list[mi].Details.Name;

                    usedstrshort.AppendPrePad(used.ToStringInvariant() + dispshort, ",");
                    usedstrlong.AppendPrePad(used.ToStringInvariant() + " x " + displong, Environment.NewLine);
                }

                needed.AppendPrePad("Used: " + usedstrshort.ToString(), ", ");
                neededlong.Append("Used: " + Environment.NewLine + usedstrlong.ToString());
            }

            return new Tuple<int, int, string, string>(max, made, needed.ToNullSafeString(), neededlong.ToNullSafeString());
        }

        static public List<MaterialCommodities> GetShoppingList(List<Tuple<Recipes.Recipe, int>> target, List<MaterialCommodities> list)
        {
            List<MaterialCommodities> shoppingList = new List<MaterialCommodities>();

            foreach (Tuple<Recipes.Recipe, int> want in target)
            {
                Recipes.Recipe r = want.Item1;
                int wanted = want.Item2;
                for (int i = 0; i < r.ingredients.Length; i++)
                {
                    string ingredient = r.ingredients[i];
                    int mi = list.FindIndex(x => x.Details.Shortname.Equals(ingredient));
                    int got = (mi >= 0) ? list[mi].scratchpad : 0;
                    int need = r.count[i] * wanted;

                    if (got < need)
                    {
                        int shopentry = shoppingList.FindIndex(x => x.Details.Shortname.Equals(ingredient));
                        if (shopentry >= 0)
                            shoppingList[shopentry].scratchpad += (need - got);
                        else
                        {
                            MaterialCommodityData db = MaterialCommodityData.GetByShortName(ingredient);
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

    }
}