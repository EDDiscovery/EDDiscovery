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
using static EDDiscovery.UserControls.Recipes;

namespace EDDiscovery.UserControls
{
    public partial class UserControlEngineering : UserControlCommonBase
    {
        RecipeFilterSelector efs;
        RecipeFilterSelector mfs;
        RecipeFilterSelector ufs;
        RecipeFilterSelector lfs;
        RecipeFilterSelector matfs;

        private List<string> levels = new List<string> { "1", "2", "3", "4", "5" };
        private List<Tuple<string, string>> matLookUp;

        private string DbColumnSave { get { return ("EngineeringGrid") + ((displaynumber > 0) ? displaynumber.ToString() : "") + "DGVCol"; } }
        private string DbWSave { get { return "EngineeringWanted" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbOSave { get { return "EngineeringOrder" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbSelSave { get { return "EngineeringList" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbEngFilterSave { get { return "EngineeringGridControlEngineerFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbModFilterSave { get { return "EngineeringGridControlModuleFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbLevelFilterSave { get { return "EngineeringGridControlLevelFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbUpgradeFilterSave { get { return "EngineeringGridControlUpgradeFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbMaterialFilterSave { get { return "EngineeringGridControlMaterialFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        int[] Order;        // order
        int[] Wanted;       // wanted, in order terms

        internal bool isEmbedded = false;
        public Action<List<Tuple<MaterialCommoditiesList.Recipe, int>>> OnDisplayComplete;  // called when display complete, for use by other UCs using this

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

            Order = SQLiteDBClass.GetSettingString(DbOSave, "").RestoreArrayFromString(0, EngineeringRecipes.Count);
            if (Order.Distinct().Count() != Order.Length)       // if not distinct..
                for (int i = 0; i < Order.Length; i++)          // reset
                    Order[i] = i;

            Wanted = SQLiteDBClass.GetSettingString(DbWSave, "").RestoreArrayFromString(0, EngineeringRecipes.Count);

            List<string> engineers = EngineeringRecipes.SelectMany(r => r.engineers).Distinct().ToList();
            engineers.Sort();
            efs = new RecipeFilterSelector(engineers);
            efs.Changed += FilterChanged;

            lfs = new RecipeFilterSelector(levels);
            lfs.Changed += FilterChanged;

            List<string> modules = EngineeringRecipes.Select(r => r.module).Distinct().ToList();
            modules.Sort();
            mfs = new RecipeFilterSelector(modules);
            mfs.Changed += FilterChanged;

            var upgrades = EngineeringRecipes.Select(r => r.name).Distinct().ToList();
            upgrades.Sort();
            ufs = new RecipeFilterSelector(upgrades);
            ufs.Changed += FilterChanged;

            List<string> matShortNames = EngineeringRecipes.SelectMany(r => r.ingredients).Distinct().ToList();
            matLookUp = matShortNames.Select(sn => Tuple.Create<string,string>(sn, MaterialCommodityDB.GetCachedMaterialByShortName(sn).name)).ToList();
            List<string> matLongNames = matLookUp.Select(lu => lu.Item2).ToList();
            matLongNames.Sort();
            matfs = new RecipeFilterSelector(matLongNames);
            matfs.Changed += FilterChanged;

            for (int i = 0; i < EngineeringRecipes.Count; i++)         // pre-fill array.. preventing the crash on cell edit when you
            {
                int rno = Order[i];
                MaterialCommoditiesList.EngineeringRecipe r = EngineeringRecipes[rno];

                int rown = dataGridViewEngineering.Rows.Add();
                DataGridViewRow row = dataGridViewEngineering.Rows[rown];
                row.Cells[UpgradeCol.Index].Value = r.name; // debug rno + ":" + r.name;
                row.Cells[Module.Index].Value = r.module;
                row.Cells[Level.Index].Value = r.level;
                row.Cells[Recipe.Index].Value = r.ingredientsstring;
                row.Cells[Engineers.Index].Value = r.engineersstring;
                row.Tag = rno;
                row.Visible = false;
            }

            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            uctg.OnTravelSelectionChanged += Display;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;
        }

        #endregion

        #region Display

        public override void InitialDisplay()
        {
            last_he = uctg.GetCurrentHistoryEntry;
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
            last_he = he;
            Display();
        }

        private void Display()
        {
            //DONT turn on sorting in the future, thats not how it works.  You click and drag to sort manually since it gives you
            // the order of recipies.

            List<Tuple<MaterialCommoditiesList.Recipe, int>> wantedList = null;

            if (last_he != null)
            {
                List<MaterialCommodities> mcl = last_he.MaterialCommodity.Sort(false);

                int fdrow = dataGridViewEngineering.FirstDisplayedScrollingRowIndex;      // remember where we were displaying

                MaterialCommoditiesList.ResetUsed(mcl);

                wantedList = new List<Tuple<MaterialCommoditiesList.Recipe, int>>();

                string engineers = SQLiteDBClass.GetSettingString(DbEngFilterSave, "All");
                List<string> engList = engineers.Split(';').ToList<string>();
                string modules = SQLiteDBClass.GetSettingString(DbModFilterSave, "All");
                string[] modArray = modules.Split(';');
                string levels = SQLiteDBClass.GetSettingString(DbLevelFilterSave, "All");
                int[] lvlArray = (levels == "All"  || levels == "None") ? new int[0] : levels.Split(';').Where(x => !string.IsNullOrEmpty(x)).Select(x => int.Parse(x)).ToArray();
                string upgrades = SQLiteDBClass.GetSettingString(DbUpgradeFilterSave, "All");
                string[] upgArray = upgrades.Split(';');
                string materials = SQLiteDBClass.GetSettingString(DbMaterialFilterSave, "All");
                List<string> matList;
                if (materials == "All" || materials == "None") { matList = new List<string>(); }
                else { matList = materials.Split(';').Where(x => !string.IsNullOrEmpty(x)).Select(m => matLookUp.Where(u => u.Item2 == m).First().Item1).ToList(); }
                
                for (int i = 0; i < EngineeringRecipes.Count; i++)
                {
                    int rno = (int)dataGridViewEngineering.Rows[i].Tag;
                    dataGridViewEngineering[MaxCol.Index, i].Value = MaterialCommoditiesList.HowManyLeft(mcl, EngineeringRecipes[rno]).Item1.ToStringInvariant();
                    bool visible = true;
                    
                    if (engineers == "All" && modules == "All" && levels == "All" && upgrades == "All" && materials == "All")
                    { visible = true; }
                    else
                    {
                        visible = false;
                        if (engineers == "All") { visible = true; }
                        else
                        {
                            var included = engList.Intersect<string>(EngineeringRecipes[rno].engineers.ToList<string>());
                            visible = included.Count() > 0;
                        }
                        if (modules == "All") { visible = visible && true; }
                        else
                        {
                            visible = visible && modArray.Contains(EngineeringRecipes[rno].module);
                        }
                        if (levels == "All") { visible = visible && true; }
                        else
                        {
                            visible = visible && lvlArray.Contains(EngineeringRecipes[rno].level);
                        }
                        if (upgrades == "All") { visible = visible && true; }
                        else
                        {
                            visible = visible && upgArray.Contains(EngineeringRecipes[rno].name);
                        }
                        if (materials == "All") { visible = visible && true; }
                        else
                        {
                            var included = matList.Intersect<string>(EngineeringRecipes[rno].ingredients.ToList<string>());
                            visible = visible && included.Count() > 0;
                        }
                    }

                    dataGridViewEngineering.Rows[i].Visible = visible;

                    if (visible)
                    {
                        Tuple<int, int, string> res = MaterialCommoditiesList.HowManyLeft(mcl, EngineeringRecipes[rno], Wanted[rno]);
                        //System.Diagnostics.Debug.WriteLine("{0} Recipe {1} executed {2} {3} ", i, rno, Wanted[rno], res.Item2);

                        dataGridViewEngineering[WantedCol.Index, i].Value = Wanted[rno].ToStringInvariant();
                        dataGridViewEngineering[Available.Index, i].Value = res.Item2.ToStringInvariant();
                        dataGridViewEngineering[Notes.Index, i].Value = res.Item3;

                    }
                    if (Wanted[rno] > 0 && (visible || isEmbedded))      // embedded, need to 
                    {
                        wantedList.Add(new Tuple<MaterialCommoditiesList.Recipe, int>(EngineeringRecipes[rno], Wanted[rno]));
                    }
                }

                if (!isEmbedded)
                {
                    MaterialCommoditiesList.ResetUsed(mcl);
                    List<MaterialCommodities> shoppinglist = MaterialCommoditiesList.GetShoppingList(wantedList, mcl);
                    dataGridViewEngineering.RowCount = EngineeringRecipes.Count;         // truncate previous shopping list..
                    foreach (MaterialCommodities c in shoppinglist.OrderBy(mat => mat.name))      // and add new..
                    {

                        int rn = dataGridViewEngineering.Rows.Add();

                        foreach (var cell in dataGridViewEngineering.Rows[rn].Cells.OfType<DataGridViewCell>())
                        {
                            if (cell.OwningColumn == UpgradeCol)
                                cell.Value = c.name;
                            else if (cell.OwningColumn == WantedCol)
                                cell.Value = c.scratchpad.ToStringInvariant();
                            else if (cell.OwningColumn == Notes)
                                cell.Value = c.shortname;
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
        }

        #endregion

        #region Layout

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridViewEngineering, DbColumnSave);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewEngineering, DbColumnSave);

            uctg.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;

            SQLiteDBClass.PutSettingString(DbOSave, Order.ToString(","));
            SQLiteDBClass.PutSettingString(DbWSave, Wanted.ToString(","));
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

            if (rowFrom >= 0 && rowFrom < EngineeringRecipes.Count)        // only can drag EngineeringRecipes area..
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
            if (e.Effect == DragDropEffects.Move && droprow>=0 && droprow < EngineeringRecipes.Count )
            {
                DataGridViewRow rowTo = e.Data.GetData( typeof(DataGridViewRow)) as DataGridViewRow;
                dataGridViewEngineering.Rows.RemoveAt(rowFrom);
                dataGridViewEngineering.Rows.Insert(droprow, rowTo);

                for (int i = 0; i < EngineeringRecipes.Count; i++)
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
            for (int i = 0; i < EngineeringRecipes.Count; i++)
            {
                int rno = (int)dataGridViewEngineering.Rows[i].Tag;
                Wanted[rno] = 0;
            }
            Display();
        }
    }
}
