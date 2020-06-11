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

        private List<string> levels = new List<string> { "1", "2", "3", "4", "5", "Experimental" };

        public string PrefixName = "Engineering";

        private string DbColumnSave { get { return (PrefixName + "Grid") + ((displaynumber > 0) ? displaynumber.ToString() : "") + "DGVCol"; } }
        private string DbWSave { get { return PrefixName + "Wanted" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbOSave { get { return PrefixName + "Order" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbSelSave { get { return PrefixName + "List" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbEngFilterSave { get { return PrefixName + "GridControlEngineerFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbModFilterSave { get { return PrefixName + "GridControlModuleFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbLevelFilterSave { get { return PrefixName + "GridControlLevelFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbUpgradeFilterSave { get { return PrefixName + "GridControlUpgradeFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbMaterialFilterSave { get { return PrefixName + "GridControlMaterialFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbHistoricMatsSave { get { return PrefixName + "GridHistoricMaterials" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbWordWrap { get { return PrefixName + "WordWrap" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        int[] Order;        // order
        int[] Wanted;       // wanted, in order terms

        internal bool isEmbedded = false;
        internal bool isHistoric = false;
        public Action<List<Tuple<Recipes.Recipe, int>>> OnDisplayComplete;  // called when display complete, for use by other UCs using this

        HistoryEntry last_he = null;

        #region Init

        public UserControlEngineering()
        {
            InitializeComponent();
            var corner = dataGridViewEngineering.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            dataGridViewEngineering.MakeDoubleBuffered();
            extCheckBoxWordWrap.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbWordWrap, false);
            UpdateWordWrap();
            extCheckBoxWordWrap.Click += extCheckBoxWordWrap_Click;

            Order = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbOSave, "").RestoreArrayFromString(0, Recipes.EngineeringRecipes.Count);
            if (Order.Max() >= Recipes.EngineeringRecipes.Count || Order.Min() < 0 || Order.Distinct().Count() != Recipes.EngineeringRecipes.Count)       // if not distinct..
            {
                for (int i = 0; i < Order.Length; i++)          // reset
                    Order[i] = i;
            }

            Wanted = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbWSave, "").RestoreArrayFromString(0, Recipes.EngineeringRecipes.Count);

            List<string> engineers = Recipes.EngineeringRecipes.SelectMany(r => r.engineers).Distinct().ToList();
            engineers.Sort();
            efs = new RecipeFilterSelector(engineers);
            efs.Changed += FilterChanged;

            lfs = new RecipeFilterSelector(levels);
            lfs.Changed += FilterChanged;

            List<string> modules = Recipes.EngineeringRecipes.SelectMany(r => r.modules).Distinct().ToList();
            modules.Sort();
            mfs = new RecipeFilterSelector(modules);
            mfs.Changed += FilterChanged;

            var upgrades = Recipes.EngineeringRecipes.Select(r => r.Name).Distinct().ToList();
            upgrades.Sort();
            ufs = new RecipeFilterSelector(upgrades);
            ufs.Changed += FilterChanged;

            List<string> matLongNames = Recipes.EngineeringRecipes.SelectMany(r => r.Ingredients).Select(x=>x.Name).Distinct().ToList();
            matfs = new RecipeFilterSelector(matLongNames);
            matfs.Changed += FilterChanged;

            for (int i = 0; i < Recipes.EngineeringRecipes.Count; i++)         // pre-fill array.. preventing the crash on cell edit when you
            {
                int rno = Order[i];
                Recipes.EngineeringRecipe r = Recipes.EngineeringRecipes[rno];

                int rown = dataGridViewEngineering.Rows.Add();
                DataGridViewRow row = dataGridViewEngineering.Rows[rown];
                row.Cells[UpgradeCol.Index].Value = r.Name; // debug rno + ":" + r.name;
                row.Cells[ModuleCol.Index].Value = r.modulesstring;
                row.Cells[LevelCol.Index].Value = r.level;
                row.Cells[EngineersCol.Index].Value = r.engineersstring;
                row.Tag = rno;
                row.Visible = false;
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
            dataGridViewEngineering.RowTemplate.MinimumHeight = Font.ScalePixels(26);
            uctg.OnTravelSelectionChanged += UCTGChanged;
            DGVLoadColumnLayout(dataGridViewEngineering, DbColumnSave);
            chkNotHistoric.Checked = !isHistoric;       // upside down now
            chkNotHistoric.Visible = !isEmbedded;
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewEngineering, DbColumnSave);

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
                List<MaterialCommodities> mcl = last_he.MaterialCommodity.Sort(false);

                int fdrow = dataGridViewEngineering.FirstDisplayedScrollingRowIndex;      // remember where we were displaying

                var totals = MaterialCommoditiesRecipe.TotalList(mcl);                  // start with totals present

                wantedList = new List<Tuple<Recipes.Recipe, int>>();

                string engineers = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbEngFilterSave, "All");
                List<string> engList = engineers.Split(';').ToList<string>();
                string modules = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbModFilterSave, "All");
                List<string> modList = modules.Split(';').ToList<string>();
                string levels = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbLevelFilterSave, "All");
                string[] lvlArray = (levels == "All" || levels == "None") ? new string[0] : levels.Split(';');
                string upgrades = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbUpgradeFilterSave, "All");
                string[] upgArray = upgrades.Split(';');
                string materials = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbMaterialFilterSave, "All");
                var matList = materials.Split(';');        // list of materials to show
                
                for (int i = 0; i < Recipes.EngineeringRecipes.Count; i++)
                {
                    int rno = (int)dataGridViewEngineering.Rows[i].Tag;
                    dataGridViewEngineering[MaxCol.Index, i].Value = MaterialCommoditiesRecipe.HowManyLeft(mcl, totals, Recipes.EngineeringRecipes[rno]).Item1.ToString();
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

                        Tuple<int, int, string,string> res = MaterialCommoditiesRecipe.HowManyLeft(mcl, totals, Recipes.EngineeringRecipes[rno], Wanted[rno]);
                        //System.Diagnostics.Debug.WriteLine("{0} Recipe {1} executed {2} {3} ", i, rno, Wanted[rno], res.Item2);

                        dataGridViewEngineering[WantedCol.Index, i].Value = Wanted[rno].ToString();
                        dataGridViewEngineering[AvailableCol.Index, i].Value = res.Item2.ToString();
                        dataGridViewEngineering[NotesCol.Index, i].Value = res.Item3;
                        dataGridViewEngineering[NotesCol.Index, i].ToolTipText = res.Item4;
                        dataGridViewEngineering[RecipeCol.Index, i].Value = r.IngredientsStringvsCurrent(last_he.MaterialCommodity);
                        dataGridViewEngineering[RecipeCol.Index, i].ToolTipText = r.IngredientsStringLong;
                    }
                    if (Wanted[rno] > 0 && (visible || isEmbedded))      // embedded, need to 
                    {
                        wantedList.Add(new Tuple<Recipes.Recipe, int>(Recipes.EngineeringRecipes[rno], Wanted[rno]));
                    }
                }

                if (!isEmbedded)
                {
                    var shoppinglist = MaterialCommoditiesRecipe.GetShoppingList(wantedList, mcl);

                    dataGridViewEngineering.RowCount = Recipes.EngineeringRecipes.Count;         // truncate previous shopping list..

                    foreach (var c in shoppinglist)      // and add new..
                    {
                        var cur = last_he.MaterialCommodity.Find(c.Item1.Details);    // may be null

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
                    Wanted[rno] = iv;
                    Display();
                }
                else
                    dataGridViewEngineering[WantedCol.Index, e.RowIndex].Value = Wanted[rno].ToString();
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
                    Order[i] = (int)dataGridViewEngineering.Rows[i].Tag;          // reset the order array

                //for (int i = 0; i < 10; i++)   System.Diagnostics.Debug.WriteLine(i.ToString() + "=" + Order[i]);

                Display();
            }
        }
        
        private void buttonFilterModule_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            mfs.FilterButton(DbModFilterSave, b,
                             discoveryform.theme.TextBackColor, discoveryform.theme.TextBlockColor, this.FindForm());
        }

        private void buttonFilterEngineer_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            efs.FilterButton(DbEngFilterSave, b,
                             discoveryform.theme.TextBackColor, discoveryform.theme.TextBlockColor, this.FindForm());
        }

        private void buttonFilterLevel_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            lfs.FilterButton(DbLevelFilterSave, b,
                             discoveryform.theme.TextBackColor, discoveryform.theme.TextBlockColor, this.FindForm());
        }

        private void buttonFilterUpgrade_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            ufs.FilterButton(DbUpgradeFilterSave, b,
                             discoveryform.theme.TextBackColor, discoveryform.theme.TextBlockColor, this.FindForm());
        }

        private void buttonFilterMaterial_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            matfs.FilterButton(DbMaterialFilterSave, b,
                             discoveryform.theme.TextBackColor, discoveryform.theme.TextBlockColor, this.FindForm());
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Recipes.EngineeringRecipes.Count; i++)
            {
                int rno = (int)dataGridViewEngineering.Rows[i].Tag;
                Wanted[rno] = 0;
            }
            Display();
        }

        private void chkHistoric_CheckedChanged(object sender, EventArgs e)
        {
            SetHistoric(!chkNotHistoric.Checked);      // upside down when changed appearance
        }
    }
}
