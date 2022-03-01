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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlEngineering : UserControlCommonBase
    {
        RecipeFilterSelector efs;
        RecipeFilterSelector mfs;
        RecipeFilterSelector ufs;
        RecipeFilterSelector lfs;
        RecipeFilterSelector matfs;

        private List<string> levels = new List<string> { "1", "2", "3", "4", "5", "NA"};

        private string dbWSave = "Wanted";
        private string dbOSave = "Order";
        private string dbEngFilterSave = "GridControlEngineerFilter";
        private string dbModFilterSave = "GridControlModuleFilter";
        private string dbLevelFilterSave = "GridControlLevelFilter";
        private string dbUpgradeFilterSave = "GridControlUpgradeFilter";
        private string dbMaterialFilterSave = "GridControlMaterialFilter";
        private string dbHistoricMatsSave = "GridHistoricMaterials";
        private string dbWordWrap = "WordWrap";

        int[] RowToRecipe;        // order
        int[] WantedPerRecipe;       // wanted, in order terms

        internal bool isEmbedded = false;
        internal bool isHistoric = false;
        public Action<List<Tuple<Recipes.Recipe, int>>> OnDisplayComplete;  // called when display complete, for use by other UCs using this

        HistoryEntry last_he = null;

        #region Init

        public UserControlEngineering()
        {
            DBBaseName = "Engineering";
            InitializeComponent();
        }

        public override void Init()
        {
            dataGridViewEngineering.MakeDoubleBuffered();
            extCheckBoxWordWrap.Checked = GetSetting(dbWordWrap, false);
            UpdateWordWrap();
            extCheckBoxWordWrap.Click += extCheckBoxWordWrap_Click;

            RowToRecipe = GetSetting(dbOSave, "").RestoreArrayFromString(0, Recipes.EngineeringRecipes.Count);
            if (RowToRecipe.Max() >= Recipes.EngineeringRecipes.Count || RowToRecipe.Min() < 0 || RowToRecipe.Distinct().Count() != Recipes.EngineeringRecipes.Count)       // if not distinct..
            {
                for (int i = 0; i < RowToRecipe.Length; i++)          // reset
                    RowToRecipe[i] = i;
            }

            WantedPerRecipe = GetSetting(dbWSave, "").RestoreArrayFromString(0, Recipes.EngineeringRecipes.Count);

            List<string> engineers = Recipes.EngineeringRecipes.SelectMany(r => r.engineers).Distinct().ToList();
            engineers.Sort();
            efs = new RecipeFilterSelector(engineers);
            efs.SaveSettings += (newvalue, e) => { PutSetting(dbEngFilterSave, newvalue); Display(); };

            lfs = new RecipeFilterSelector(levels);
            lfs.SaveSettings += (newvalue, e) => { PutSetting(dbLevelFilterSave, newvalue); Display(); };

            List<string> modules = Recipes.EngineeringRecipes.SelectMany(r => r.modules).Distinct().ToList();
            modules.Sort();
            mfs = new RecipeFilterSelector(modules);
            mfs.SaveSettings += (newvalue, e) => { PutSetting(dbModFilterSave, newvalue); Display(); };

            var upgrades = Recipes.EngineeringRecipes.Select(r => r.Name).Distinct().ToList();
            upgrades.Sort();
            ufs = new RecipeFilterSelector(upgrades);
            ufs.SaveSettings += (newvalue, e) => { PutSetting(dbUpgradeFilterSave, newvalue); Display(); };

            List<string> matLongNames = Recipes.EngineeringRecipes.SelectMany(r => r.Ingredients).Select(x=>x.Name).Distinct().ToList();
            matfs = new RecipeFilterSelector(matLongNames);
            matfs.SaveSettings += (newvalue, e) => { PutSetting(dbMaterialFilterSave, newvalue); Display(); };

            for (int rowno = 0; rowno < Recipes.EngineeringRecipes.Count; rowno++)         // pre-fill array.. preventing the crash on cell edit when you
            {
                int recipeno = RowToRecipe[rowno];
                Recipes.EngineeringRecipe r = Recipes.EngineeringRecipes[recipeno];

                int rown = dataGridViewEngineering.Rows.Add();
                DataGridViewRow row = dataGridViewEngineering.Rows[rown];
                row.Cells[UpgradeCol.Index].Value = r.Name; // debug rno + ":" + r.name;
                row.Cells[ModuleCol.Index].Value = r.modulesstring;
                row.Cells[LevelCol.Index].Value = r.level;
                row.Cells[EngineersCol.Index].Value = r.engineersstring;
                row.Tag = recipeno;
                row.Visible = false;
            }

            isHistoric = GetSetting(dbHistoricMatsSave, false);
            
            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;

            var enumlist = new Enum[] { EDTx.UserControlEngineering_UpgradeCol, EDTx.UserControlEngineering_ModuleCol, EDTx.UserControlEngineering_LevelCol, EDTx.UserControlEngineering_MaxCol, EDTx.UserControlEngineering_WantedCol, EDTx.UserControlEngineering_AvailableCol, EDTx.UserControlEngineering_NotesCol, EDTx.UserControlEngineering_RecipeCol, EDTx.UserControlEngineering_EngineersCol };
            BaseUtils.Translator.Instance.TranslateControls(this,enumlist);
            var enumlisttt = new Enum[] { EDTx.UserControlEngineering_ToolTip, EDTx.UserControlEngineering_buttonFilterUpgrade_ToolTip, EDTx.UserControlEngineering_buttonFilterModule_ToolTip, EDTx.UserControlEngineering_buttonFilterLevel_ToolTip, EDTx.UserControlEngineering_buttonFilterEngineer_ToolTip, EDTx.UserControlEngineering_buttonFilterMaterial_ToolTip, EDTx.UserControlEngineering_buttonClear_ToolTip, EDTx.UserControlEngineering_chkNotHistoric_ToolTip, EDTx.UserControlEngineering_extCheckBoxWordWrap_ToolTip };
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
            dataGridViewEngineering.RowTemplate.MinimumHeight = Font.ScalePixels(26);
            uctg.OnTravelSelectionChanged += UCTGChanged;
      //      DGVLoadColumnLayout(dataGridViewEngineering);
            chkNotHistoric.Checked = !isHistoric;       // upside down now
            chkNotHistoric.Visible = !isEmbedded;
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewEngineering);

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
            if (he.journalEntry is ICommodityJournalEntry || he.journalEntry is IMaterialJournalEntry)
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

                int fdrow = dataGridViewEngineering.SafeFirstDisplayedScrollingRowIndex();      // remember where we were displaying

                var totals = MaterialCommoditiesRecipe.TotalList(mclmats);                  // start with totals present

                wantedList = new List<Tuple<Recipes.Recipe, int>>();

                string engineers = GetSetting(dbEngFilterSave, "All");
                List<string> engList = engineers.Split(';').ToList<string>();
                string modules = GetSetting(dbModFilterSave, "All");
                List<string> modList = modules.Split(';').ToList<string>();
                string levels = GetSetting(dbLevelFilterSave, "All");
                string[] lvlArray = (levels == "All" || levels == "None") ? new string[0] : levels.Split(';');
                string upgrades = GetSetting(dbUpgradeFilterSave, "All");
                string[] upgArray = upgrades.Split(';');
                string materials = GetSetting(dbMaterialFilterSave, "All");
                var matList = materials.Split(';');        // list of materials to show
                
                for (int i = 0; i < Recipes.EngineeringRecipes.Count; i++)
                {
                    int rno = (int)dataGridViewEngineering.Rows[i].Tag;
                    dataGridViewEngineering[MaxCol.Index, i].Value = MaterialCommoditiesRecipe.HowManyLeft(mclmats, totals, Recipes.EngineeringRecipes[rno]).Item1.ToString();
                    bool visible = true;
                    
                    if (!(engineers == "All" && modules == "All" && levels == "All" && upgrades == "All" && materials == "All"))
                    {
                        if (engineers != "All")
                        {
                            var included = engList.Intersect<string>(Recipes.EngineeringRecipes[rno].engineers.ToList<string>());
                            visible &= included.Count() > 0;
                        }

                        if (modules != "All")
                        { 
                            var included = modList.Intersect<string>(Recipes.EngineeringRecipes[rno].modules.ToList<string>());
                            visible &= included.Count() > 0;
                        }

                        if (levels != "All")
                        { 
                            visible &= lvlArray.Contains(Recipes.EngineeringRecipes[rno].level);
                        }

                        if (upgrades != "All")
                        { 
                            visible &= upgArray.Contains(Recipes.EngineeringRecipes[rno].Name);
                        }

                        if (materials != "All")
                        {
                            var inglongname = Recipes.EngineeringRecipes[rno].Ingredients.Select(x => x.Name);
                            var included = matList.Intersect<string>(inglongname);
                            visible &= included.Count() > 0;
                        }
                    }

                    dataGridViewEngineering.Rows[i].Visible = visible;

                    if (visible)
                    {
                        Recipes.Recipe r = Recipes.EngineeringRecipes[i];

                        var res = MaterialCommoditiesRecipe.HowManyLeft(mclmats, totals, Recipes.EngineeringRecipes[rno], WantedPerRecipe[rno]);
                        //System.Diagnostics.Debug.WriteLine("{0} Recipe {1} executed {2} {3} ", i, rno, Wanted[rno], res.Item2);

                        dataGridViewEngineering[WantedCol.Index, i].Value = WantedPerRecipe[rno].ToString();
                        dataGridViewEngineering[AvailableCol.Index, i].Value = res.Item2.ToString();
                        dataGridViewEngineering[PercentageCol.Index, i].Value = res.Item5.ToString("N0");
                        dataGridViewEngineering[NotesCol.Index, i].Value = res.Item3;
                        dataGridViewEngineering[NotesCol.Index, i].ToolTipText = res.Item4;
                        dataGridViewEngineering[RecipeCol.Index, i].Value = r.IngredientsStringvsCurrent(totalmcl);
                        dataGridViewEngineering[RecipeCol.Index, i].ToolTipText = r.IngredientsStringLong;
                        if (res.Item5 >= 100.0)
                            dataGridViewEngineering.Rows[i].DefaultCellStyle.BackColor = ExtendedControls.Theme.Current.GridHighlightBack;
                    }
                    if (WantedPerRecipe[rno] > 0 && (visible || isEmbedded))      // embedded, need to 
                    {
                        wantedList.Add(new Tuple<Recipes.Recipe, int>(Recipes.EngineeringRecipes[rno], WantedPerRecipe[rno]));
                    }
                }

                if (!isEmbedded)
                {
                    var shoppinglist = MaterialCommoditiesRecipe.GetShoppingList(wantedList, mclmats);

                    dataGridViewEngineering.RowCount = Recipes.EngineeringRecipes.Count;         // truncate previous shopping list..

                    foreach (var c in shoppinglist)      // and add new..
                    {
                        var cur = totalmcl.Find((x) => x.Details == c.Item1.Details);    // may be null

                        DataGridViewRow r = dataGridViewEngineering.RowTemplate.Clone() as DataGridViewRow;
                        r.CreateCells(dataGridViewEngineering, c.Item1.Details.Name, "", "", "", c.Item2.ToString(), (cur?.Count ?? 0).ToString(), "", c.Item1.Details.Shortname, "");
                        r.ReadOnly = true;
                        dataGridViewEngineering.Rows.Add(r);
                    }
                }

                if ( fdrow>=0 && dataGridViewEngineering.Rows[fdrow].Visible )        // better check visible, may have changed..
                    dataGridViewEngineering.SafeFirstDisplayedScrollingRowIndex(fdrow);
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
            dataGridViewEngineering.DefaultCellStyle.WrapMode = extCheckBoxWordWrap.Checked ? DataGridViewTriState.True : DataGridViewTriState.False;
            dataGridViewEngineering.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dataViewScrollerPanel.UpdateScroll();
        }

        private void dataGridViewModules_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == WantedCol.Index)
            {
                string v = (string)dataGridViewEngineering[WantedCol.Index, e.RowIndex].Value;
                int iv = 0;
                int rno = (int)dataGridViewEngineering.Rows[e.RowIndex].Tag;

                if (v.InvariantParse(out iv))
                {
                    //System.Diagnostics.Debug.WriteLine("Set wanted {0} to {1}", rno, iv);
                    WantedPerRecipe[rno] = iv;
                    Display();
                }
                else
                    dataGridViewEngineering[WantedCol.Index, e.RowIndex].Value = WantedPerRecipe[rno].ToString();
            }
        }

        private Rectangle moveMoveDragBox;
        private int rowFrom;

        private void dataGridViewEngineering_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (moveMoveDragBox != Rectangle.Empty && !moveMoveDragBox.Contains(e.X, e.Y))
                {
//                    System.Diagnostics.Debug.WriteLine("move: Drag Start");
                    dataGridViewEngineering.DoDragDrop( dataGridViewEngineering.Rows[rowFrom], DragDropEffects.Move);
                }
            }
        }

        private void dataGridViewEngineering_MouseDown(object sender, MouseEventArgs e)
        {
            rowFrom = dataGridViewEngineering.HitTest(e.X, e.Y).RowIndex;

            if (rowFrom >= 0 && rowFrom < Recipes.EngineeringRecipes.Count)        // only can drag Recipes.EngineeringRecipes area..
            {
                Size dragSize = SystemInformation.DragSize;
                moveMoveDragBox = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                               e.Y - (dragSize.Height / 2)), dragSize);
            }
            else
                moveMoveDragBox = Rectangle.Empty;
        }

        private void dataGridViewEngineering_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void dataGridViewEngineering_DragDrop(object sender, DragEventArgs e)
        {
            Point clientPoint = dataGridViewEngineering.PointToClient(new Point(e.X, e.Y));
            int droprow = dataGridViewEngineering.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
            //System.Diagnostics.Debug.WriteLine(Environment.TickCount + " drop at " + droprow);

            // If the drag operation was a move then remove and insert the row.
            if (e.Effect == DragDropEffects.Move && droprow>=0 && droprow < Recipes.EngineeringRecipes.Count )
            {
                DataGridViewRow rowTo = e.Data.GetData( typeof(DataGridViewRow)) as DataGridViewRow;
                dataGridViewEngineering.Rows.RemoveAt(rowFrom);
                dataGridViewEngineering.Rows.Insert(droprow, rowTo);

                for (int i = 0; i < Recipes.EngineeringRecipes.Count; i++)
                    RowToRecipe[i] = (int)dataGridViewEngineering.Rows[i].Tag;          // reset the order array

                //for (int i = 0; i < 10; i++)   System.Diagnostics.Debug.WriteLine(i.ToString() + "=" + Order[i]);

                Display();
            }
        }
        
        private void buttonFilterModule_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            mfs.Open(GetSetting(dbModFilterSave,"All"), b, this.FindForm());
        }

        private void buttonFilterEngineer_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            efs.Open(GetSetting(dbEngFilterSave,"All"), b, this.FindForm());
        }

        private void buttonFilterLevel_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            lfs.Open(GetSetting(dbLevelFilterSave,"All"), b, this.FindForm());
        }

        private void buttonFilterUpgrade_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            ufs.Open(GetSetting(dbUpgradeFilterSave,"All"), b, this.FindForm());
        }

        private void buttonFilterMaterial_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            matfs.Open(GetSetting(dbMaterialFilterSave,"All"), b, this.FindForm());
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Recipes.EngineeringRecipes.Count; i++)
            {
                int rno = (int)dataGridViewEngineering.Rows[i].Tag;
                WantedPerRecipe[rno] = 0;
            }
            Display();
        }

        private void chkHistoric_CheckedChanged(object sender, EventArgs e)
        {
            SetHistoric(!chkNotHistoric.Checked);      // upside down when changed appearance
        }
    }
}
