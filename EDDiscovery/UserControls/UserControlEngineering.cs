/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
using EliteDangerousCore.DB;
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
        private Dictionary<string, string> matLookUp;

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

        int[] Order;        // order
        int[] Wanted;       // wanted, in order terms

        internal bool isEmbedded = false;
        internal bool isHistoric = false;
        public Action<List<Tuple<Recipes.Recipe, int>>> OnDisplayComplete;  // called when display complete, for use by other UCs using this

        #region Init

        public UserControlEngineering()
        {
            InitializeComponent();
            var corner = dataGridViewEngineering.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            dataGridViewEngineering.MakeDoubleBuffered();
            dataGridViewEngineering.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dataGridViewEngineering.RowTemplate.Height = 26;

            Order = SQLiteDBClass.GetSettingString(DbOSave, "").RestoreArrayFromString(0, Recipes.EngineeringRecipes.Count);
            if (Order.Max() >= Recipes.EngineeringRecipes.Count || Order.Min() < 0 || Order.Distinct().Count() != Recipes.EngineeringRecipes.Count)       // if not distinct..
            {
                for (int i = 0; i < Order.Length; i++)          // reset
                    Order[i] = i;
            }

            Wanted = SQLiteDBClass.GetSettingString(DbWSave, "").RestoreArrayFromString(0, Recipes.EngineeringRecipes.Count);

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

            var upgrades = Recipes.EngineeringRecipes.Select(r => r.name).Distinct().ToList();
            upgrades.Sort();
            ufs = new RecipeFilterSelector(upgrades);
            ufs.Changed += FilterChanged;

            List<string> matShortNames = Recipes.EngineeringRecipes.SelectMany(r => r.ingredients).Distinct().ToList();
            matLookUp = matShortNames.ToDictionary(sn => MaterialCommodityData.GetByShortName(sn).Name, sn => sn);
            List<string> matLongNames = matLookUp.Keys.ToList();
            matLongNames.Sort();
            matfs = new RecipeFilterSelector(matLongNames);
            matfs.Changed += FilterChanged;

            for (int i = 0; i < Recipes.EngineeringRecipes.Count; i++)         // pre-fill array.. preventing the crash on cell edit when you
            {
                int rno = Order[i];
                Recipes.EngineeringRecipe r = Recipes.EngineeringRecipes[rno];

                int rown = dataGridViewEngineering.Rows.Add();
                DataGridViewRow row = dataGridViewEngineering.Rows[rown];
                row.Cells[UpgradeCol.Index].Value = r.name; // debug rno + ":" + r.name;
                row.Cells[ModuleCol.Index].Value = r.modulesstring;
                row.Cells[LevelCol.Index].Value = r.level;
                row.Cells[RecipeCol.Index].Value = r.ingredientsstring;
                row.Cells[RecipeCol.Index].ToolTipText = r.ingredientsstringlong;
                row.Cells[EngineersCol.Index].Value = r.engineersstring;
                row.Tag = rno;
                row.Visible = false;
            }

            isHistoric = SQLiteDBClass.GetSettingBool(DbHistoricMatsSave, false);
            
            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += Display;
            DGVLoadColumnLayout(dataGridViewEngineering, DbColumnSave);
            chkHistoric.Checked = isHistoric;
            chkHistoric.Visible = !isEmbedded;
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewEngineering, DbColumnSave);

            uctg.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;

            SQLiteDBClass.PutSettingString(DbOSave, Order.ToString(","));
            SQLiteDBClass.PutSettingString(DbWSave, Wanted.ToString(","));
            SQLiteDBClass.PutSettingBool(DbHistoricMatsSave, isHistoric);
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

        private void Discoveryform_OnNewEntry(HistoryEntry he, HistoryList hl)
        {
            last_he = he;
            if (he.journalEntry is IMaterialCommodityJournalEntry)
                Display();
        }

        HistoryEntry last_he = null;
        private void Display(HistoryEntry he, HistoryList hl)
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

            System.Diagnostics.Trace.WriteLine(BaseUtils.AppTicks.TickCountLap(this,true) + " EN " + displaynumber + " Begin Display");

            if (last_he != null)
            {
                List<MaterialCommodities> mcl = last_he.MaterialCommodity.Sort(false);

                int fdrow = dataGridViewEngineering.FirstDisplayedScrollingRowIndex;      // remember where we were displaying

                MaterialCommoditiesRecipe.ResetUsed(mcl);

                wantedList = new List<Tuple<Recipes.Recipe, int>>();

                string engineers = SQLiteDBClass.GetSettingString(DbEngFilterSave, "All");
                List<string> engList = engineers.Split(';').ToList<string>();
                string modules = SQLiteDBClass.GetSettingString(DbModFilterSave, "All");
                List<string> modList = modules.Split(';').ToList<string>();
                string levels = SQLiteDBClass.GetSettingString(DbLevelFilterSave, "All");
                string[] lvlArray = (levels == "All" || levels == "None") ? new string[0] : levels.Split(';');
                string upgrades = SQLiteDBClass.GetSettingString(DbUpgradeFilterSave, "All");
                string[] upgArray = upgrades.Split(';');
                string materials = SQLiteDBClass.GetSettingString(DbMaterialFilterSave, "All");
                List<string> matList;
                if (materials == "All" || materials == "None")
                {
                    matList = new List<string>();
                }
                else
                {
                    matList = materials.Split(';').Where(x => !string.IsNullOrEmpty(x) && matLookUp.ContainsKey(x)).Select(m => matLookUp[m]).ToList();
                }
                
                for (int i = 0; i < Recipes.EngineeringRecipes.Count; i++)
                {
                    int rno = (int)dataGridViewEngineering.Rows[i].Tag;
                    dataGridViewEngineering[MaxCol.Index, i].Value = MaterialCommoditiesRecipe.HowManyLeft(mcl, Recipes.EngineeringRecipes[rno]).Item1.ToStringInvariant();
                    bool visible = true;
                    
                    if (engineers == "All" && modules == "All" && levels == "All" && upgrades == "All" && materials == "All")
                    { visible = true; }
                    else
                    {
                        visible = false;
                        if (engineers == "All") { visible = true; }
                        else
                        {
                            var included = engList.Intersect<string>(Recipes.EngineeringRecipes[rno].engineers.ToList<string>());
                            visible = included.Count() > 0;
                        }
                        if (modules == "All") { visible = visible && true; }
                        else
                        {
                            var included = modList.Intersect<string>(Recipes.EngineeringRecipes[rno].modules.ToList<string>());
                            visible = visible && included.Count() > 0;
                        }
                        if (levels == "All") { visible = visible && true; }
                        else
                        {
                            visible = visible && lvlArray.Contains(Recipes.EngineeringRecipes[rno].level);
                        }
                        if (upgrades == "All") { visible = visible && true; }
                        else
                        {
                            visible = visible && upgArray.Contains(Recipes.EngineeringRecipes[rno].name);
                        }
                        if (materials == "All") { visible = visible && true; }
                        else
                        {
                            var included = matList.Intersect<string>(Recipes.EngineeringRecipes[rno].ingredients.ToList<string>());
                            visible = visible && included.Count() > 0;
                        }
                    }

                    dataGridViewEngineering.Rows[i].Visible = visible;

                    if (visible)
                    {
                        Tuple<int, int, string,string> res = MaterialCommoditiesRecipe.HowManyLeft(mcl, Recipes.EngineeringRecipes[rno], Wanted[rno]);
                        //System.Diagnostics.Debug.WriteLine("{0} Recipe {1} executed {2} {3} ", i, rno, Wanted[rno], res.Item2);

                        dataGridViewEngineering[WantedCol.Index, i].Value = Wanted[rno].ToStringInvariant();
                        dataGridViewEngineering[AvailableCol.Index, i].Value = res.Item2.ToStringInvariant();
                        dataGridViewEngineering[NotesCol.Index, i].Value = res.Item3;
                        dataGridViewEngineering[NotesCol.Index, i].ToolTipText = res.Item4;

                    }
                    if (Wanted[rno] > 0 && (visible || isEmbedded))      // embedded, need to 
                    {
                        wantedList.Add(new Tuple<Recipes.Recipe, int>(Recipes.EngineeringRecipes[rno], Wanted[rno]));
                    }
                }

                if (!isEmbedded)
                {
                    MaterialCommoditiesRecipe.ResetUsed(mcl);
                    List<MaterialCommodities> shoppinglist = MaterialCommoditiesRecipe.GetShoppingList(wantedList, mcl);
                    dataGridViewEngineering.RowCount = Recipes.EngineeringRecipes.Count;         // truncate previous shopping list..
                    foreach (MaterialCommodities c in shoppinglist.OrderBy(mat => mat.Details.Name))      // and add new..
                    {

                        int rn = dataGridViewEngineering.Rows.Add();

                        foreach (var cell in dataGridViewEngineering.Rows[rn].Cells.OfType<DataGridViewCell>())
                        {
                            if (cell.OwningColumn == UpgradeCol)
                                cell.Value = c.Details.Name;
                            else if (cell.OwningColumn == WantedCol)
                                cell.Value = c.scratchpad.ToStringInvariant();
                            else if (cell.OwningColumn == NotesCol)
                                cell.Value = c.Details.Shortname;
                            else if (cell.ValueType == null || cell.ValueType.IsAssignableFrom(typeof(string)))
                                cell.Value = string.Empty;
                        }
                        dataGridViewEngineering.Rows[rn].ReadOnly = true;   // disable editing wanted..
                    }
                }

                if ( fdrow>=0 && dataGridViewEngineering.Rows[fdrow].Visible )        // better check visible, may have changed..
                    dataGridViewEngineering.FirstDisplayedScrollingRowIndex = fdrow;
            }

            if (OnDisplayComplete != null)
                OnDisplayComplete(wantedList);

            System.Diagnostics.Trace.WriteLine(BaseUtils.AppTicks.TickCountLap(this) + " EN " + displaynumber + " Load Finished");
        }

        #endregion


        private void comboBoxHistoryWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            Display();
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            Display();
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
                    dataGridViewEngineering[WantedCol.Index, e.RowIndex].Value = Wanted[rno].ToStringInvariant();
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
            SetHistoric(chkHistoric.Checked);
        }
    }
}
