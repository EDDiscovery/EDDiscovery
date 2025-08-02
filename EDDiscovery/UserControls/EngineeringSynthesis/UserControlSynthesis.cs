/*
 * Copyright © 2016 - 2023 EDDiscovery development team
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
 */

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
        public bool DontShowShoppingList { get; set; } = false;
        public bool isHistoric { get; set; } = false;

        public Action<List<Tuple<Recipes.Recipe, int>>> OnDisplayComplete;  // called when display complete, for use by other UCs using this
        public Dictionary<MaterialCommodityMicroResourceType, int> NeededResources;        // computed during display

        private string dbWSave = "Wanted";
        private string dbOSave = "Order";
        private string dbRecipeFilterSave = "RecipeFilter";
        private string dbLevelFilterSave = "LevelFilter";
        private string dbMaterialFilterSave = "MaterialFilter";
        private string dbHistoricMatsSave = "HistoricMaterials";
        private string dbWordWrap = "WordWrap";

        private int[] RowToRecipe;        // order
        private int[] WantedPerRecipe;       // wanted, in order terms

        private RecipeFilterSelector rfs;
        private RecipeFilterSelector lfs;
        private RecipeFilterSelector mfs;

        public HistoryEntry CurrentHistoryEntry { get {return last_he;} }     //one in use, may be null

        private HistoryEntry last_he = null;

        #region Init

        public UserControlSynthesis()
        {
            DBBaseName = "Synthesis";
            InitializeComponent();
        }

        protected override void Init()
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

            var lvls = Recipes.SynthesisRecipes.Select(r => r.Level).Distinct().ToList();
            lvls.Reverse();     // because the table starts with premium-std-basic, thats the order it gets picked up in. reverse it
            lfs = new RecipeFilterSelector(lvls);
            lfs.SaveSettings += (newvalue, e) => { PutSetting(dbLevelFilterSave, newvalue); Display(); };

            List<string> matLongNames = Recipes.SynthesisRecipes.SelectMany(r => r.Ingredients).Select(x=>x.TranslatedName).Distinct().ToList();
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
                    row.Cells[1].Value = r.Level;
                    row.Tag = recipeno;
                    row.Visible = false;
                }
            }
            System.Diagnostics.Debug.WriteLine($"Synth Rows are {dataGridViewSynthesis.RowCount}");

            isHistoric = GetSetting(dbHistoricMatsSave, false);

            DiscoveryForm.OnNewEntry += Discoveryform_OnNewEntry;
            DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange;

            BaseUtils.TranslatorMkII.Instance.TranslateControls(this);
            BaseUtils.TranslatorMkII.Instance.TranslateTooltip(toolTip,this);
        }

        protected override void LoadLayout()
        {
            dataGridViewSynthesis.RowTemplate.MinimumHeight = Font.ScalePixels(26);
            DGVLoadColumnLayout(dataGridViewSynthesis);
            chkNotHistoric.Checked = !isHistoric;
            chkNotHistoric.Visible = !DontShowShoppingList;
            this.chkNotHistoric.CheckedChanged += new System.EventHandler(this.chkHistoric_CheckedChanged);     // now trigger
        }

        protected override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewSynthesis);

            DiscoveryForm.OnNewEntry -= Discoveryform_OnNewEntry;
            DiscoveryForm.OnHistoryChange -= Discoveryform_OnHistoryChange;

            PutSetting(dbOSave, RowToRecipe.ToString(","));
            PutSetting(dbWSave, WantedPerRecipe.ToString(","));
            PutSetting(dbHistoricMatsSave, isHistoric);
        }


        #endregion

        #region Display
        protected override void InitialDisplay()
        {
            if (isHistoric)
            {
                RequestPanelOperation(this, new UserControlCommonBase.RequestTravelHistoryPos());     //request an update 
            }
            else
            {
                last_he = DiscoveryForm.History.GetLast;
                Display();
            }
        }

        private void Discoveryform_OnHistoryChange()
        {
            InitialDisplay();
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he)
        {
            if (!isHistoric) // only if current (not on history cursor)
            {
                last_he = he;
                //touchdown and liftoff ensure shopping list refresh in case displaying landed planet mats, scan for mat availability while flying in same
                if (he.journalEntry is IMaterialJournalEntry || he.journalEntry.EventTypeID == JournalTypeEnum.Touchdown || he.journalEntry.EventTypeID == JournalTypeEnum.Liftoff
                    || he.IsFSDLocationCarrierJump || he.journalEntry.EventTypeID == JournalTypeEnum.Scan)       // only scan can change material list
                {
                    Display();
                }
            }
        }

        public override void ReceiveHistoryEntry(HistoryEntry he)
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
                var totalmcl = DiscoveryForm.History.MaterialCommoditiesMicroResources.Get(last_he.MaterialCommodity);
                var mclmats = DiscoveryForm.History.MaterialCommoditiesMicroResources.GetMaterialsSorted(last_he.MaterialCommodity);      // mcl at this point

                int fdrow = dataGridViewSynthesis.SafeFirstDisplayedScrollingRowIndex();      // remember where we were displaying

                var totals = MaterialCommoditiesRecipe.TotalList(mclmats);                  // start with totals present

                wantedList = new List<Tuple<Recipes.Recipe, int>>();

                string recep = GetSetting(dbRecipeFilterSave, "All");
                string[] recipeArray = recep.Split(';');
                string levels = GetSetting(dbLevelFilterSave, "All");
                string[] lvlArray = (levels == "All" || levels == "None") ? new string[0] : levels.Split(';');
                string materials = GetSetting(dbMaterialFilterSave, "All");
                var matList = materials.Split(';');        // list of materials to show

                NeededResources = new Dictionary<MaterialCommodityMicroResourceType, int>();

                // filter by selections the rows and make the ones not required invisible

                for (int i = 0; i < Recipes.SynthesisRecipes.Count; i++)
                {
                    int rno = (int)dataGridViewSynthesis.Rows[i].Tag;

                    bool visible = true;
                
                    if (recep != "All" || levels != "All" || materials != "All")
                    { 
                        if (recep != "All")
                        {
                            visible &= recipeArray.Contains(Recipes.SynthesisRecipes[rno].Name);
                        }

                        if (levels != "All")
                        {
                            visible &= lvlArray.Contains(Recipes.SynthesisRecipes[rno].Level);
                        }

                        if (materials != "All")
                        {
                            var inglongname = Recipes.SynthesisRecipes[rno].Ingredients.Select(x => x.TranslatedName);
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
                        var res = MaterialCommoditiesRecipe.HowManyLeft(r , WantedPerRecipe[rno], mclmats, totals, NeededResources);

                        using (DataGridViewRow row = dataGridViewSynthesis.Rows[i])
                        {
                            row.Cells[2].Value = res.Item1.ToString();
                            row.Cells[3].Value = WantedPerRecipe[rno].ToString(); //wanted
                            row.Cells[4].Value = res.Item2.ToString();  // available
                            row.Cells[5].Value = res.Item5.ToString("N0"); // %
                            row.Cells[6].Value = res.Item3; // notes
                            row.Cells[6].ToolTipText = res.Item4;
                            row.Cells[7].Value = r.IngredientsStringvsCurrent(totalmcl);    // recipe
                            row.Cells[7].ToolTipText = r.IngredientsStringLong;
                            row.DefaultCellStyle.BackColor = res.Item5 >= 100.0 ? ExtendedControls.Theme.Current.GridHighlightBack : ExtendedControls.Theme.Current.GridCellBack;
                        }
                    }

                    // add to wanted list, either if visible or we don't show shopping list (therefore embedded)

                    if (WantedPerRecipe[rno] > 0 && (dataGridViewSynthesis.Rows[i].Visible || DontShowShoppingList))
                    {
                        wantedList.Add(new Tuple<Recipes.Recipe, int>(Recipes.SynthesisRecipes[rno], WantedPerRecipe[rno]));
                    }
                }

                if (!DontShowShoppingList)
                {
                    dataGridViewSynthesis.RowCount = Recipes.SynthesisRecipes.Count;         // truncate previous shopping list..

                    foreach (var kvp in NeededResources)        // and add new..
                    {
                        var cur = totalmcl.Find((x) => x.Details == kvp.Key);    // may be null

                        Object[] values = { kvp.Key.TranslatedName, kvp.Key.TranslatedCategory, (cur?.Count ?? 0).ToString(), kvp.Value.ToString(), "", "", kvp.Key.Shortname };
                        int rn = dataGridViewSynthesis.Rows.Add(values);
                        dataGridViewSynthesis.Rows[rn].ReadOnly = true;     // disable editing wanted..
                    }
                }

                // need to ensure within bounds (we add/remove rows from the grid for the shopping list results)
                if (fdrow >= 0 && fdrow < dataGridViewSynthesis.RowCount && dataGridViewSynthesis.Rows[fdrow].Visible)        // better check visible, may have changed..
                    dataGridViewSynthesis.SafeFirstDisplayedScrollingRowIndex(fdrow);

            }

            OnDisplayComplete?.Invoke(wantedList);
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
            dataGridViewSynthesis.SetWordWrap(extCheckBoxWordWrap.Checked);
            dataViewScrollerPanel.UpdateScroll();
        }

        private void dataGridViewModules_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == WantedCol.Index)
            {
                string v = (string)dataGridViewSynthesis.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                int rno = (int)dataGridViewSynthesis.Rows[e.RowIndex].Tag;

                // parse with current culture, as it was placed there with ToString()
                if (int.TryParse(v, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.CurrentCulture, out int iv))
                {
                    //System.Diagnostics.Debug.WriteLine("Set wanted {0} to {1}", rno, iv);
                    WantedPerRecipe[rno] = iv;
                    Display();
                }
                else
                    dataGridViewSynthesis.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = WantedPerRecipe[rno].ToString();
            }
        }
        private void dataGridViewSynthesis_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewSynthesis.Rows[e.RowIndex];

                if (e.ColumnIndex == Recipe.Index)
                {
                    int rno = (int)row.Tag;
                    Recipes.SynthesisRecipe r = Recipes.SynthesisRecipes[rno];
                    dataGridViewSynthesis.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = r.IngredientsStringLong;
                }

                int rcell = e.ColumnIndex;
                if (row.Cells[rcell].Style.WrapMode == DataGridViewTriState.True)
                    row.Cells[rcell].Style.WrapMode = DataGridViewTriState.NotSet;
                else
                    row.Cells[rcell].Style.WrapMode = DataGridViewTriState.True;
            }
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

          //      for (int i = 0; i < Recipes.SynthesisRecipes.Count; i++)
           //         System.Diagnostics.Debug.WriteLine(i.ToString() + "=" + RowToRecipe[i] + " : "+ dataGridViewSynthesis.Rows[i].Tag);

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

        private void extButtonPushResources_Click(object sender, EventArgs e)
        {
            if (NeededResources != null)
            {
                var req = new UserControlCommonBase.PushResourceWantedList() { Resources = NeededResources };
                RequestPanelOperationOpen(PanelInformation.PanelIDs.Resources, req);
            }
        }

        private void chkHistoric_CheckedChanged(object sender, EventArgs e)
        {
            SetHistoric(!chkNotHistoric.Checked);       // button sense changed
        }
        internal void SetHistoric(bool newVal)
        {
            isHistoric = newVal;
            InitialDisplay();       // same action as initial display
        }

    }
}
