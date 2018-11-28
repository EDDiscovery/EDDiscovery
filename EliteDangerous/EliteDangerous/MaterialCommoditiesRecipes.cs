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

using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

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