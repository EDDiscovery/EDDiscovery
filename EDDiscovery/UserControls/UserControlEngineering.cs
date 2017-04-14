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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EDDiscovery.Controls;
using EDDiscovery.DB;
using EDDiscovery.EliteDangerous;

namespace EDDiscovery.UserControls
{
    public partial class UserControlEngineering : UserControlCommonBase
    {
        private int displaynumber = 0;
        private EDDiscoveryForm discoveryform;

        EngineerFilterSelector efs = new EngineerFilterSelector();
        ModuleFilterSelector mfs = new ModuleFilterSelector();
        LevelFilterSelector lfs = new LevelFilterSelector();

        private string DbColumnSave { get { return ("EngineeringGrid") + ((displaynumber > 0) ? displaynumber.ToString() : "") + "DGVCol"; } }
        private string DbWSave { get { return "EngineeringWanted" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbOSave { get { return "EngineeringOrder" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbSelSave { get { return "EngineeringList" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbEngFilterSave { get { return "EngineeringGridControlEngineerFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbModFilterSave { get { return "EngineeringGridControlModuleFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbLevelFilterSave { get { return "EngineeringGridControlLevelFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        int[] Order;        // order
        int[] Wanted;       // wanted, in order terms

        #region Init

        public UserControlEngineering()
        {
            InitializeComponent();
            Name = "Engineering";
        }

        public override void Init(EDDiscoveryForm ed, int vn) //0=primary, 1 = first windowed version, etc
        {
            discoveryform = ed;
            displaynumber = vn;

            dataGridViewEngineering.MakeDoubleBuffered();
            dataGridViewEngineering.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dataGridViewEngineering.RowTemplate.Height = 26;

            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            ed.TravelControl.OnTravelSelectionChanged += Display;

            Order = Restore(DB.SQLiteDBClass.GetSettingString(DbOSave, ""), 0, Recipes.Count);
            if (Order.Distinct().Count() != Order.Length)       // if not distinct..
                for (int i = 0; i < Order.Length; i++)          // reset
                    Order[i] = i;

            Wanted = Restore(DB.SQLiteDBClass.GetSettingString(DbWSave, ""), 0, Recipes.Count);

            efs.Changed += FilterChanged;
            mfs.Changed += FilterChanged;
            lfs.Changed += FilterChanged;

            for (int i = 0; i < Recipes.Count; i++)         // pre-fill array.. preventing the crash on cell edit when you
            {
                int rno = Order[i];
                MaterialCommoditiesList.EngineeringRecipe r = Recipes[rno];

                int rown = dataGridViewEngineering.Rows.Add();

                using (DataGridViewRow row = dataGridViewEngineering.Rows[rown])
                {
                    row.Cells[0].Value = r.name; // debug rno + ":" + r.name;
                    row.Cells[1].Value = r.module;
                    row.Cells[2].Value = r.level;
                    row.Cells[7].Value = r.ingredientsstring;
                    row.Cells[8].Value = r.engineersstring;
                    row.Tag = rno;
                    row.Visible = false;
                }
            }
        }

        #endregion

        #region Display

        private void Discoveryform_OnNewEntry(HistoryEntry he, HistoryList hl)
        {
            last_he = he;
            Display();
        }

        HistoryEntry last_he = null;
        public override void Display(HistoryEntry he, HistoryList hl)
        {
            last_he = he;
            Display();
        }

        public void Display()
        {
            //DONT turn on sorting in the future, thats not how it works.  You click and drag to sort manually since it gives you
            // the order of recipies.

            if (last_he != null)
            {
                List<MaterialCommodities> mcl = last_he.MaterialCommodity.Sort(false);

                int fdrow = dataGridViewEngineering.FirstDisplayedScrollingRowIndex;      // remember where we were displaying

                MaterialCommoditiesList.ResetUsed(mcl);

                for (int i = 0; i < Recipes.Count; i++)
                {
                    int rno = (int)dataGridViewEngineering.Rows[i].Tag;
                    dataGridViewEngineering.Rows[i].Cells[3].Value = MaterialCommoditiesList.HowManyLeft(mcl, Recipes[rno]).Item1.ToStringInvariant();
                    bool visible = true;
                    string engineers = SQLiteDBClass.GetSettingString(DbEngFilterSave, "All");
                    string modules = SQLiteDBClass.GetSettingString(DbModFilterSave, "All");
                    string levels = SQLiteDBClass.GetSettingString(DbLevelFilterSave, "All");

                    if (engineers == "All" && modules == "All" && levels == "All")
                    { visible = true; }
                    else
                    {
                        visible = false;
                        if (engineers == "All") { visible = true; }
                        else
                        {
                            var engineer = engineers.Split(';').ToList<string>();
                            var included = engineer.Intersect<string>(Recipes[rno].engineers.ToList<string>());
                            visible = included.Count() > 0;
                        }
                        if (modules == "All") { visible = visible && true; }
                        else
                        {
                            visible = visible && modules.Split(';').Contains(Recipes[rno].module);
                        }
                        if (levels == "All") { visible = visible && true; }
                        else
                        {
                            visible = visible && levels.Split(';').Contains(Recipes[rno].level.ToString());
                        }
                    }

                    dataGridViewEngineering.Rows[i].Visible = visible;
                }

                List<MaterialCommodities> shoppinglist = new List<MaterialCommodities>();

                for (int i = 0; i < Recipes.Count; i++)
                {
                    if (dataGridViewEngineering.Rows[i].Visible)
                    {
                        int rno = (int)dataGridViewEngineering.Rows[i].Tag;
                        Tuple<int, int, string> res = MaterialCommoditiesList.HowManyLeft(mcl, Recipes[rno], shoppinglist, Wanted[rno]);
                        //System.Diagnostics.Debug.WriteLine("{0} Recipe {1} executed {2} {3} ", i, rno, Wanted[rno], res.Item2);

                        using (DataGridViewRow row = dataGridViewEngineering.Rows[i])
                        {
                            row.Cells[4].Value = Wanted[rno].ToStringInvariant();
                            row.Cells[5].Value = res.Item2.ToStringInvariant();
                            row.Cells[6].Value = res.Item3;
                        }
                    }
                }

                dataGridViewEngineering.RowCount = Recipes.Count;         // truncate previous shopping list..

                shoppinglist.Sort(delegate (MaterialCommodities left, MaterialCommodities right) { return left.name.CompareTo(right.name); });

                foreach( MaterialCommodities c in shoppinglist )        // and add new..
                {
                    Object[] values = { c.name, "", c.scratchpad.ToStringInvariant(), "", c.shortname };
                    int rn = dataGridViewEngineering.Rows.Add(values);
                    dataGridViewEngineering.Rows[rn].ReadOnly = true;     // disable editing wanted..
                }

                if ( fdrow>=0 && dataGridViewEngineering.Rows[fdrow].Visible )        // better check visible, may have changed..
                    dataGridViewEngineering.FirstDisplayedScrollingRowIndex = fdrow;
            }
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

            discoveryform.TravelControl.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;

            DB.SQLiteDBClass.PutSettingString(DbOSave, Order.ToString(","));
            DB.SQLiteDBClass.PutSettingString(DbWSave, Wanted.ToString(","));
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

        int[] Restore(string plist, int def , int length)
        {
            int i = 0;
            string[] parray = plist.Split(',');
            int[] newarray = new int[length];
            for (; i < length; i++)
            {
                if (i >= parray.Length || !parray[i].InvariantParse(out newarray[i]))
                    newarray[i] = def;
            }

            return newarray;
        }

        private void dataGridViewModules_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string v = (string)dataGridViewEngineering.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            int iv = 0;
            int rno = (int)dataGridViewEngineering.Rows[e.RowIndex].Tag;

            if (v.InvariantParse(out iv))
            {
                if (e.ColumnIndex == 4)
                {
                    //System.Diagnostics.Debug.WriteLine("Set wanted {0} to {1}", rno, iv);
                    Wanted[rno] = iv;
                    Display();
                }
            }
            else
                dataGridViewEngineering.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Wanted[rno].ToStringInvariant();
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

            if (rowFrom >= 0 && rowFrom < Recipes.Count)        // only can drag recipes area..
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
            if (e.Effect == DragDropEffects.Move && droprow>=0 && droprow < Recipes.Count )
            {
                DataGridViewRow rowTo = e.Data.GetData( typeof(DataGridViewRow)) as DataGridViewRow;
                dataGridViewEngineering.Rows.RemoveAt(rowFrom);
                dataGridViewEngineering.Rows.Insert(droprow, rowTo);

                for (int i = 0; i < Recipes.Count; i++)
                    Order[i] = (int)dataGridViewEngineering.Rows[i].Tag;          // reset the order array

                //for (int i = 0; i < 10; i++)   System.Diagnostics.Debug.WriteLine(i.ToString() + "=" + Order[i]);

                Display();
            }
        }

        List<MaterialCommoditiesList.EngineeringRecipe> Recipes = new List<MaterialCommoditiesList.EngineeringRecipe>()
        {
            new MaterialCommoditiesList.EngineeringRecipe("Blast Resistant Armor", "1Ni", "Armour", 1, "Liz Ryder,Selene Jean" ),
            new MaterialCommoditiesList.EngineeringRecipe("Blast Resistant Armor", "1C,1Zn", "Armour", 2, "Selene Jean" ),
            new MaterialCommoditiesList.EngineeringRecipe("Blast Resistant Armor", "1SAll,1V,1Zr", "Armour", 3, "Selene Jean" )

        };

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
    }
}
