/*
 * Copyright © 2016 - 2020 EDDiscovery development team
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
        private string dbWSave = "Wanted";
        private string dbOSave = "Order";
        private string dbRecipeFilterSave = "RecipeFilter";
        private string dbLevelFilterSave = "LevelFilter";
        private string dbMaterialFilterSave = "MaterialFilter";
        private string dbHistoricMatsSave = "HistoricMaterials";
        private string dbWordWrap = "WordWrap";

        int[] RowToRecipe;        // order
        int[] WantedPerRecipe;       // wanted, in order terms
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
            DBBaseName = "Synthesis";
            InitializeComponent();
        }

        public override void Init()
        {
            dataGridViewSynthesis.MakeDoubleBuffered();
            extCheckBoxWordWrap.Checked = GetSetting(dbWordWrap, false);
            UpdateWordWrap();
            extCheckBoxWordWrap.Click += extCheckBoxWordWrap_Click;

            RowToRecipe = GetSetting(dbOSave, "").RestoreArrayFromString(0, Recipes.SynthesisRecipes.Count);
            if (RowToRecipe.Max() >= Recipes.SynthesisRecipes.Count || RowToRecipe.Min() < 0 || RowToRecipe.Distinct().Count() != Recipes.SynthesisRecipes.Count)       // if not distinct..
            {
                for (int i = 0; i < RowToRecipe.Length; i++)          // reset
                    RowToRecipe[i] = i;
            }

            WantedPerRecipe = GetSetting(dbWSave, "").RestoreArrayFromString(0, Recipes.SynthesisRecipes.Count);

            var rcpes = Recipes.SynthesisRecipes.Select(r => r.Name).Distinct().ToList();
            rcpes.Sort();
            rfs = new RecipeFilterSelector(rcpes);
            rfs.SaveSettings += (newvalue, e) => { PutSetting(dbRecipeFilterSave, newvalue); Display(); };

            var lvls = Recipes.SynthesisRecipes.Select(r => r.level).Distinct().ToList();
            lvls.Sort();
            lfs = new RecipeFilterSelector(lvls);
            lfs.SaveSettings += (newvalue, e) => { PutSetting(dbLevelFilterSave, newvalue); Display(); };

            List<string> matLongNames = Recipes.SynthesisRecipes.SelectMany(r => r.Ingredients).Select(x=>x.Name).Distinct().ToList();
            matLongNames.Sort();
            mfs = new RecipeFilterSelector(matLongNames);
            mfs.SaveSettings += (newvalue, e) => { PutSetting(dbMaterialFilterSave, newvalue); Display(); };

            for (int rowno = 0; rowno < Recipes.SynthesisRecipes.Count; rowno++)         // pre-fill array.. preventing the crash on cell edit when you
            {
                int recipeno = RowToRecipe[rowno];
                Recipes.SynthesisRecipe r = Recipes.SynthesisRecipes[recipeno];

                int rown = dataGridViewSynthesis.Rows.Add();

                using (DataGridViewRow row = dataGridViewSynthesis.Rows[rown])
                {
                    row.Cells[0].Value = r.Name; // debug rno + ":" + r.name;
                    row.Cells[1].Value = r.level;
                    row.Tag = recipeno;
                    row.Visible = false;
                }
            }

            isHistoric = GetSetting(dbHistoricMatsSave, false);

            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;

            var enumlist = new Enum[] { EDTx.UserControlSynthesis_UpgradeCol, EDTx.UserControlSynthesis_Level, EDTx.UserControlSynthesis_MaxCol, EDTx.UserControlSynthesis_WantedCol, EDTx.UserControlSynthesis_Available, EDTx.UserControlSynthesis_Notes, EDTx.UserControlSynthesis_Recipe };
            var enumlisttt = new Enum[] { EDTx.UserControlSynthesis_buttonRecipeFilter_ToolTip, EDTx.UserControlSynthesis_buttonFilterLevel_ToolTip, EDTx.UserControlSynthesis_buttonMaterialFilter_ToolTip, EDTx.UserControlSynthesis_buttonClear_ToolTip, EDTx.UserControlSynthesis_chkNotHistoric_ToolTip, EDTx.UserControlSynthesis_extCheckBoxWordWrap_ToolTip };

            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);
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
            DGVLoadColumnLayout(dataGridViewSynthesis);
            chkNotHistoric.Checked = !isHistoric;
            chkNotHistoric.Visible = !isEmbedded;
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewSynthesis);

            uctg.OnTravelSelectionChanged -= UCTGChanged;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;

            PutSetting(dbOSave, RowToRecipe.ToString(","));
            PutSetting(dbWSave, WantedPerRecipe.ToString(","));
            PutSetting(dbHistoricMatsSave, isHistoric);
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
                || he.IsLocOrJump || he.journalEntry.EventTypeID == JournalTypeEnum.Scan)       // only scan can change material list
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
                var totalmcl = discoveryform.history.MaterialCommoditiesMicroResources.Get(last_he.MaterialCommodity);
                var mclmats = discoveryform.history.MaterialCommoditiesMicroResources.GetMaterialsSorted(last_he.MaterialCommodity);      // mcl at this point

                int fdrow = dataGridViewSynthesis.SafeFirstDisplayedScrollingRowIndex();      // remember where we were displaying

                var totals = MaterialCommoditiesRecipe.TotalList(mclmats);                  // start with totals present

                wantedList = new List<Tuple<Recipes.Recipe, int>>();

                string recep = GetSetting(dbRecipeFilterSave, "All");
                string[] recipeArray = recep.Split(';');
                string levels = GetSetting(dbLevelFilterSave, "All");
                string[] lvlArray = (levels == "All" || levels == "None") ? new string[0] : levels.Split(';');
                string materials = GetSetting(dbMaterialFilterSave, "All");
                var matList = materials.Split(';');        // list of materials to show

                for (int i = 0; i < Recipes.SynthesisRecipes.Count; i++)
                {
                    int rno = (int)dataGridViewSynthesis.Rows[i].Tag;
                    dataGridViewSynthesis.Rows[i].Cells[2].Value = MaterialCommoditiesRecipe.HowManyLeft(mclmats, totals, Recipes.SynthesisRecipes[rno]).Item1.ToString();
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
                        var res = MaterialCommoditiesRecipe.HowManyLeft(mclmats, totals, r , WantedPerRecipe[rno]);
                        //System.Diagnostics.Debug.WriteLine("{0} Recipe {1} executed {2} {3} ", i, rno, Wanted[rno], res.Item2);

                        using (DataGridViewRow row = dataGridViewSynthesis.Rows[i])
                        {
                            row.Cells[3].Value = WantedPerRecipe[rno].ToString(); //wanted
                            row.Cells[4].Value = res.Item2.ToString();  // available
                            row.Cells[5].Value = res.Item5.ToString("N0"); // %
                            row.Cells[6].Value = res.Item3; // notes
                            row.Cells[6].ToolTipText = res.Item4;
                            row.Cells[7].Value = r.IngredientsStringvsCurrent(totalmcl);    // recipe
                            row.Cells[7].ToolTipText = r.IngredientsStringLong;
                            if (res.Item5 >= 100.0)
                                row.DefaultCellStyle.BackColor = ExtendedControls.Theme.Current.GridHighlightBack;
                        }
                    }
                    if (WantedPerRecipe[rno] > 0 && (dataGridViewSynthesis.Rows[i].Visible || isEmbedded))
                    {
                        wantedList.Add(new Tuple<Recipes.Recipe, int>(Recipes.SynthesisRecipes[rno], WantedPerRecipe[rno]));
                    }
                }

                dataGridViewSynthesis.RowCount = Recipes.SynthesisRecipes.Count;         // truncate previous shopping list..

                if (!isEmbedded)
                {
                    var shoppinglist = MaterialCommoditiesRecipe.GetShoppingList(wantedList, mclmats);

                    foreach (var c in shoppinglist)        // and add new..
                    {
                        var cur = totalmcl.Find((x) => x.Details == c.Item1.Details);    // may be null

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

        private void extCheckBoxWordWrap_Click(object sender, EventArgs e)
        {
            PutSetting(dbWordWrap, extCheckBoxWordWrap.Checked);
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
                    WantedPerRecipe[rno] = iv;
                    Display();
                }
            }
            else
                dataGridViewSynthesis.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = WantedPerRecipe[rno].ToString();
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
                    RowToRecipe[i] = (int)dataGridViewSynthesis.Rows[i].Tag;          // reset the order array

                for (int i = 0; i < dataGridViewSynthesis.RowCount; i++)
                    System.Diagnostics.Debug.WriteLine(i.ToString() + "=" + RowToRecipe[i] + " : "+ dataGridViewSynthesis.Rows[i].Tag);

                //for (int i = 0; i < 10; i++)   

                Display();
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Recipes.SynthesisRecipes.Count; i++)
            {
                int rno = (int)dataGridViewSynthesis.Rows[i].Tag;
                WantedPerRecipe[rno] = 0;
            }
            Display();
        }

        private void buttonRecipeFilter_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            rfs.Open(GetSetting(dbRecipeFilterSave, "All"), b, this.FindForm());
        }

        private void buttonFilterLevel_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            lfs.Open(GetSetting(dbLevelFilterSave, "All"), b, this.FindForm());
        }

        private void buttonMaterialFilter_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            mfs.Open(GetSetting(dbMaterialFilterSave, "All"), b, this.FindForm());
        }

        private void chkHistoric_CheckedChanged(object sender, EventArgs e)
        {
            SetHistoric(!chkNotHistoric.Checked);       // button sense changed
        }
    }
}
