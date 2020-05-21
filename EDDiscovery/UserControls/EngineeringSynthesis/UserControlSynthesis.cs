/*
 * Copyright © 2016 - 2019 EDDiscovery development team
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
using EDDiscovery.Controls;
using EliteDangerousCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlSynthesis : UserControlCommonBase
    {
        public string PrefixName = "Synthesis";

        private string DbColumnSave { get { return (PrefixName + "Grid") + ((displaynumber > 0) ? displaynumber.ToString() : "") + "DGVCol"; } }
        private string DbWSave { get { return PrefixName + "Wanted" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbOSave { get { return PrefixName + "Order" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbRecipeFilterSave { get { return PrefixName + "RecipeFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbLevelFilterSave { get { return PrefixName + "LevelFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbMaterialFilterSave { get { return PrefixName + "MaterialFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbHistoricMatsSave { get { return PrefixName + "HistoricMaterials" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbWordWrap { get { return PrefixName + "WordWrap" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        int[] Order;        // order
        int[] Wanted;       // wanted, in order terms
        internal bool isEmbedded = false;
        internal bool isHistoric = false;

        RecipeFilterSelector rfs;
        RecipeFilterSelector lfs;
        RecipeFilterSelector mfs;

        public Action<List<Tuple<Recipes.Recipe, int>>> OnDisplayComplete;  // called when display complete, for use by other UCs using this

        public HistoryEntry CurrentHistoryEntry { get {return last_he;} }     //one in use, may be null

        HistoryEntry last_he = null;

        #region Init

        public UserControlSynthesis()
        {
            InitializeComponent();
            var corner = dataGridViewSynthesis.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            dataGridViewSynthesis.MakeDoubleBuffered();
            extCheckBoxWordWrap.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbWordWrap, false);
            UpdateWordWrap();
            extCheckBoxWordWrap.Click += extCheckBoxWordWrap_Click;

            Order = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbOSave, "").RestoreArrayFromString(0, Recipes.SynthesisRecipes.Count);
            if (Order.Max() >= Recipes.SynthesisRecipes.Count || Order.Min() < 0 || Order.Distinct().Count() != Recipes.SynthesisRecipes.Count)       // if not distinct..
            {
                for (int i = 0; i < Order.Length; i++)          // reset
                    Order[i] = i;
            }

            Wanted = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbWSave, "").RestoreArrayFromString(0, Recipes.SynthesisRecipes.Count);

            var rcpes = Recipes.SynthesisRecipes.Select(r => r.Name).Distinct().ToList();
            rcpes.Sort();
            rfs = new RecipeFilterSelector(rcpes);
            rfs.Changed += FilterChanged;

            var lvls = Recipes.SynthesisRecipes.Select(r => r.level).Distinct().ToList();
            lvls.Sort();
            lfs = new RecipeFilterSelector(lvls);
            lfs.Changed += FilterChanged;

            List<string> matLongNames = Recipes.SynthesisRecipes.SelectMany(r => r.Ingredients).Select(x=>x.Name).Distinct().ToList();
            matLongNames.Sort();
            mfs = new RecipeFilterSelector(matLongNames);
            mfs.Changed += FilterChanged;

            for (int i = 0; i < Recipes.SynthesisRecipes.Count; i++)         // pre-fill array.. preventing the crash on cell edit when you
            {
                int rno = Order[i];
                Recipes.SynthesisRecipe r = Recipes.SynthesisRecipes[rno];

                int rown = dataGridViewSynthesis.Rows.Add();

                using (DataGridViewRow row = dataGridViewSynthesis.Rows[rown])
                {
                    row.Cells[0].Value = r.Name; // debug rno + ":" + r.name;
                    row.Cells[1].Value = r.level;
                    row.Tag = rno;
                    row.Visible = false;
                }
            }

            isHistoric = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbHistoricMatsSave, false);

            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= UCTGChanged;
            uctg = thc;
            uctg.OnTravelSelectionChanged += UCTGChanged;
        }

        public override void LoadLayout()
        {
            dataGridViewSynthesis.RowTemplate.MinimumHeight = Font.ScalePixels(26);
            uctg.OnTravelSelectionChanged += UCTGChanged;
            DGVLoadColumnLayout(dataGridViewSynthesis, DbColumnSave);
            chkNotHistoric.Checked = !isHistoric;
            chkNotHistoric.Visible = !isEmbedded;
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewSynthesis, DbColumnSave);

            uctg.OnTravelSelectionChanged -= UCTGChanged;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;

            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(DbOSave, Order.ToString(","));
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(DbWSave, Wanted.ToString(","));
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbHistoricMatsSave, isHistoric);
        }


        #endregion

        #region Display
        internal void SetHistoric(bool newVal)
        {
            isHistoric = newVal;
            if (isHistoric)
            {
                last_he = uctg.GetCurrentHistoryEntry;
            }
            else
            {
                last_he = discoveryform.history.GetLast;
            }
            Display();
        }

        public override void InitialDisplay()
        {
            last_he = isHistoric ? uctg.GetCurrentHistoryEntry : discoveryform.history.GetLast;
            Display();
        }

        private void Discoveryform_OnHistoryChange(HistoryList obj)
        {
            InitialDisplay();
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he, HistoryList hl)
        {
            last_he = he;
            //touchdown and liftoff ensure shopping list refresh in case displaying landed planet mats, scan for mat availability while flying in same
            if (he.journalEntry is IMaterialJournalEntry || he.journalEntry.EventTypeID == JournalTypeEnum.Touchdown || he.journalEntry.EventTypeID == JournalTypeEnum.Liftoff
                || he.IsLocOrJump || he.journalEntry.EventTypeID == JournalTypeEnum.Scan)
            {
                Display();
            }
        }

        private void UCTGChanged(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            if (isHistoric || last_he == null)
            {
                last_he = he;
                Display();
            }
        }

        private void Display()
        {
            //DONT turn on sorting in the future, thats not how it works.  You click and drag to sort manually since it gives you
            // the order of recipies.
            List<Tuple<Recipes.Recipe, int>> wantedList = null;

            if (last_he != null)
            {
                List<MaterialCommodities> mcl = last_he.MaterialCommodity.Sort(false);
                int fdrow = dataGridViewSynthesis.FirstDisplayedScrollingRowIndex;      // remember where we were displaying

                var totals = MaterialCommoditiesRecipe.TotalList(mcl);                  // start with totals present

                wantedList = new List<Tuple<Recipes.Recipe, int>>();

                string recep = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbRecipeFilterSave, "All");
                string[] recipeArray = recep.Split(';');
                string levels = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbLevelFilterSave, "All");
                string[] lvlArray = (levels == "All" || levels == "None") ? new string[0] : levels.Split(';');
                string materials = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbMaterialFilterSave, "All");
                var matList = materials.Split(';');        // list of materials to show

                for (int i = 0; i < Recipes.SynthesisRecipes.Count; i++)
                {
                    int rno = (int)dataGridViewSynthesis.Rows[i].Tag;
                    dataGridViewSynthesis.Rows[i].Cells[2].Value = MaterialCommoditiesRecipe.HowManyLeft(mcl, totals, Recipes.SynthesisRecipes[rno]).Item1.ToString();
                    bool visible = true;
                
                    if (recep != "All" || levels != "All" || materials != "All")
                    { 
                        if (recep != "All")
                        {
                            visible &= recipeArray.Contains(Recipes.SynthesisRecipes[rno].Name);
                        }

                        if (levels != "All")
                        {
                            visible &= lvlArray.Contains(Recipes.SynthesisRecipes[rno].level);
                        }

                        if (materials != "All")
                        {
                            var inglongname = Recipes.SynthesisRecipes[rno].Ingredients.Select(x => x.Name);
                            var included = matList.Intersect<string>(inglongname);
                            visible &= included.Count() > 0;
                        }
                    }

                    dataGridViewSynthesis.Rows[i].Visible = visible;
                }

                for (int i = 0; i < Recipes.SynthesisRecipes.Count; i++)
                {
                    int rno = (int)dataGridViewSynthesis.Rows[i].Tag;

                    if (dataGridViewSynthesis.Rows[i].Visible)
                    {
                        Recipes.Recipe r = Recipes.SynthesisRecipes[rno];
                        Tuple<int, int, string, string> res = MaterialCommoditiesRecipe.HowManyLeft(mcl, totals, r , Wanted[rno]);
                        //System.Diagnostics.Debug.WriteLine("{0} Recipe {1} executed {2} {3} ", i, rno, Wanted[rno], res.Item2);

                        using (DataGridViewRow row = dataGridViewSynthesis.Rows[i])
                        {
                            row.Cells[3].Value = Wanted[rno].ToString();
                            row.Cells[4].Value = res.Item2.ToString();
                            row.Cells[5].Value = res.Item3;
                            row.Cells[5].ToolTipText = res.Item4;
                            row.Cells[6].Value = r.IngredientsStringvsCurrent(last_he.MaterialCommodity);
                            row.Cells[6].ToolTipText = r.IngredientsStringLong;
                        }
                    }
                    if (Wanted[rno] > 0 && (dataGridViewSynthesis.Rows[i].Visible || isEmbedded))
                    {
                        wantedList.Add(new Tuple<Recipes.Recipe, int>(Recipes.SynthesisRecipes[rno], Wanted[rno]));
                    }
                }

                dataGridViewSynthesis.RowCount = Recipes.SynthesisRecipes.Count;         // truncate previous shopping list..

                if (!isEmbedded)
                {
                    var shoppinglist = MaterialCommoditiesRecipe.GetShoppingList(wantedList, mcl);

                    foreach (var c in shoppinglist)        // and add new..
                    {
                        var cur = last_he.MaterialCommodity.Find(c.Item1.Details);    // may be null
                        Object[] values = { c.Item1.Details.Name, c.Item1.Details.TranslatedCategory, (cur?.Count ?? 0).ToString(), c.Item2.ToString(), "", "", c.Item1.Details.Shortname };
                        int rn = dataGridViewSynthesis.Rows.Add(values);
                        dataGridViewSynthesis.Rows[rn].ReadOnly = true;     // disable editing wanted..
                    }
                }

                if (fdrow >= 0 && dataGridViewSynthesis.Rows[fdrow].Visible)        // better check visible, may have changed..
                    dataGridViewSynthesis.SafeFirstDisplayedScrollingRowIndex(fdrow);

            }

            if (OnDisplayComplete != null)
                OnDisplayComplete(wantedList);

        }

        #endregion


        private void comboBoxHistoryWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            Display();
        }

        private void FilterChanged()
        {
            Display();
        }

        private void extCheckBoxWordWrap_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbWordWrap, extCheckBoxWordWrap.Checked);
            UpdateWordWrap();
        }

        private void UpdateWordWrap()
        {
            dataGridViewSynthesis.DefaultCellStyle.WrapMode = extCheckBoxWordWrap.Checked ? DataGridViewTriState.True : DataGridViewTriState.False;
            dataGridViewSynthesis.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dataViewScrollerPanel.UpdateScroll();
        }

        private void dataGridViewModules_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string v = (string)dataGridViewSynthesis.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            int iv = 0;
            int rno = (int)dataGridViewSynthesis.Rows[e.RowIndex].Tag;

            if (v.InvariantParse(out iv))
            {
                if (e.ColumnIndex == 3)
                {
                    //System.Diagnostics.Debug.WriteLine("Set wanted {0} to {1}", rno, iv);
                    Wanted[rno] = iv;
                    Display();
                }
            }
            else
                dataGridViewSynthesis.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Wanted[rno].ToString();
        }

        private Rectangle moveMoveDragBox;
        private int rowFrom;

        private void dataGridViewSynthesis_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (moveMoveDragBox != Rectangle.Empty && !moveMoveDragBox.Contains(e.X, e.Y))
                {
//                    System.Diagnostics.Debug.WriteLine("move: Drag Start");
                    dataGridViewSynthesis.DoDragDrop( dataGridViewSynthesis.Rows[rowFrom], DragDropEffects.Move);
                }
            }
        }

        private void dataGridViewSynthesis_MouseDown(object sender, MouseEventArgs e)
        {
            rowFrom = dataGridViewSynthesis.HitTest(e.X, e.Y).RowIndex;

            if (rowFrom >= 0 && rowFrom < Recipes.SynthesisRecipes.Count)        // only can drag Recipes.SynthesisRecipes area..
            {
                Size dragSize = SystemInformation.DragSize;
                moveMoveDragBox = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                               e.Y - (dragSize.Height / 2)), dragSize);
            }
            else
                moveMoveDragBox = Rectangle.Empty;
        }

        private void dataGridViewSynthesis_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void dataGridViewSynthesis_DragDrop(object sender, DragEventArgs e)
        {
            Point clientPoint = dataGridViewSynthesis.PointToClient(new Point(e.X, e.Y));
            int droprow = dataGridViewSynthesis.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
            //System.Diagnostics.Debug.WriteLine(Environment.TickCount + " drop at " + droprow);

            // If the drag operation was a move then remove and insert the row.
            if (e.Effect == DragDropEffects.Move && droprow>=0 && droprow < Recipes.SynthesisRecipes.Count )
            {
                DataGridViewRow rowTo = e.Data.GetData( typeof(DataGridViewRow)) as DataGridViewRow;
                dataGridViewSynthesis.Rows.RemoveAt(rowFrom);
                dataGridViewSynthesis.Rows.Insert(droprow, rowTo);

                for (int i = 0; i < Recipes.SynthesisRecipes.Count; i++)
                    Order[i] = (int)dataGridViewSynthesis.Rows[i].Tag;          // reset the order array

                //for (int i = 0; i < 10; i++)   System.Diagnostics.Debug.WriteLine(i.ToString() + "=" + Order[i]);

                Display();
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Recipes.SynthesisRecipes.Count; i++)
            {
                int rno = (int)dataGridViewSynthesis.Rows[i].Tag;
                Wanted[rno] = 0;
            }
            Display();
        }

        private void buttonRecipeFilter_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            rfs.FilterButton(DbRecipeFilterSave, b,
                             discoveryform.theme.TextBackColor, discoveryform.theme.TextBlockColor, this.FindForm());
        }

        private void buttonFilterLevel_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            lfs.FilterButton(DbLevelFilterSave, b,
                             discoveryform.theme.TextBackColor, discoveryform.theme.TextBlockColor, this.FindForm());
        }

        private void buttonMaterialFilter_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            mfs.FilterButton(DbMaterialFilterSave, b,
                             discoveryform.theme.TextBackColor, discoveryform.theme.TextBlockColor, this.FindForm());
        }

        private void chkHistoric_CheckedChanged(object sender, EventArgs e)
        {
            SetHistoric(!chkNotHistoric.Checked);       // button sense changed
        }
    }
}
