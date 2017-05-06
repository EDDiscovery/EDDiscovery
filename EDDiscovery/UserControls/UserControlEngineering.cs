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
using EDDiscovery.DB;
using EDDiscovery.EliteDangerous;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlEngineering : UserControlCommonBase
    {
        private int displaynumber = 0;
        private EDDiscoveryForm discoveryform;

        EngineeringFilterSelector efs;
        EngineeringFilterSelector mfs;
        EngineeringFilterSelector ufs;
        EngineeringFilterSelector lfs;

        private List<string> levels = new List<string> { "1", "2", "3", "4", "5" };

        private string DbColumnSave { get { return ("EngineeringGrid") + ((displaynumber > 0) ? displaynumber.ToString() : "") + "DGVCol"; } }
        private string DbWSave { get { return "EngineeringWanted" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbOSave { get { return "EngineeringOrder" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbSelSave { get { return "EngineeringList" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbEngFilterSave { get { return "EngineeringGridControlEngineerFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbModFilterSave { get { return "EngineeringGridControlModuleFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbLevelFilterSave { get { return "EngineeringGridControlLevelFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbUpgradeFilterSave { get { return "EngineeringGridControlUpgradeFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

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

            List<string> engineers = Recipes.SelectMany(r => r.engineers.ToList()).Distinct().ToList();
            engineers.Sort();
            efs = new EngineeringFilterSelector(engineers);
            efs.Changed += FilterChanged;

            lfs = new EngineeringFilterSelector(levels);
            lfs.Changed += FilterChanged;

            List<string> modules = Recipes.Select(r => r.module).Distinct().ToList();
            modules.Sort();
            mfs = new EngineeringFilterSelector(modules);
            mfs.Changed += FilterChanged;

            var upgrades = Recipes.Select(r => r.name).Distinct().ToList();
            upgrades.Sort();
            ufs = new EngineeringFilterSelector(upgrades);
            ufs.Changed += FilterChanged;

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

                string engineers = SQLiteDBClass.GetSettingString(DbEngFilterSave, "All");
                List<string> engList = engineers.Split(';').ToList<string>();
                string modules = SQLiteDBClass.GetSettingString(DbModFilterSave, "All");
                string[] modArray = modules.Split(';');
                string levels = SQLiteDBClass.GetSettingString(DbLevelFilterSave, "All");
                int[] lvlArray = (levels == "All" || levels == "None") ? new int[0] : levels.Split(';').Where(x => !string.IsNullOrEmpty(x)).Select(x => int.Parse(x)).ToArray();
                string upgrades = SQLiteDBClass.GetSettingString(DbUpgradeFilterSave, "All");
                string[] upgArray = upgrades.Split(';');

                for (int i = 0; i < Recipes.Count; i++)
                {
                    int rno = (int)dataGridViewEngineering.Rows[i].Tag;
                    dataGridViewEngineering.Rows[i].Cells[3].Value = MaterialCommoditiesList.HowManyLeft(mcl, Recipes[rno]).Item1.ToStringInvariant();
                    bool visible = true;
                    
                    if (engineers == "All" && modules == "All" && levels == "All" && upgrades == "All")
                    { visible = true; }
                    else
                    {
                        visible = false;
                        if (engineers == "All") { visible = true; }
                        else
                        {
                            var included = engList.Intersect<string>(Recipes[rno].engineers.ToList<string>());
                            visible = included.Count() > 0;
                        }
                        if (modules == "All") { visible = visible && true; }
                        else
                        {
                            visible = visible && modArray.Contains(Recipes[rno].module);
                        }
                        if (levels == "All") { visible = visible && true; }
                        else
                        {
                            visible = visible && lvlArray.Contains(Recipes[rno].level);
                        }
                        if (upgrades == "All") { visible = visible && true; }
                        else
                        {
                            visible = visible && upgArray.Contains(Recipes[rno].name);
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
                    Object[] values = { c.name, "", "", "", c.scratchpad.ToStringInvariant(), "", c.shortname };
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

        #region Recipes

        List<MaterialCommoditiesList.EngineeringRecipe> Recipes = new List<MaterialCommoditiesList.EngineeringRecipe>()
        {
            new MaterialCommoditiesList.EngineeringRecipe("Blast Resistant", "1Ni", "Armour", 1, "Liz Ryder,Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Blast Resistant", "1C,1Zn", "Armour", 2, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Blast Resistant", "1SAll,1V,1Zr", "Armour", 3, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Heavy Duty", "1C", "Armour", 1, "Liz Ryder,Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Heavy Duty", "1C,1SE", "Armour", 2, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Heavy Duty", "1C,1HDC,1SE", "Armour", 3, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Heavy Duty", "1PCo,1SS,1V", "Armour", 4, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Heavy Duty", "1CoS,1CDC,1W", "Armour", 5, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Kinetic Resistant", "1Ni", "Armour", 1, "Liz Ryder,Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Kinetic Resistant", "1Ni,1V", "Armour", 2, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Kinetic Resistant", "1HDC,1SAll,1V", "Armour", 3, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Kinetic Resistant", "1GA,1PCo,1W", "Armour", 4, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Kinetic Resistant", "1CDC,1Mo,1PA", "Armour", 5, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Lightweight", "1Fe", "Armour", 1, "Liz Ryder,Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Lightweight", "1CCo,1Fe", "Armour", 2, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Lightweight", "1CCo,1HDC,1Fe", "Armour", 3, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Thermal Resistant", "1HCW", "Armour", 1, "Liz Ryder,Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Thermal Resistant", "1HDP,1Ni", "Armour", 2, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Thermal Resistant", "1HE,1SAll,1V", "Armour", 3, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Thermal Resistant", "1GA,1HV,1W", "Armour", 4, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Thermal Resistant", "1Mo,1PA,1PHR", "Armour", 5, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1WSE", "Auto Field-Maintenance Unit", 1, "Lori Jameson,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1SE", "Auto Field-Maintenance Unit", 2, "Lori Jameson,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1HDC", "Auto Field-Maintenance Unit", 3, "Lori Jameson,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1PCo,1SS,1V", "Auto Field-Maintenance Unit", 4, "Lori Jameson"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1S", "Beam Laser", 1, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1HDP,1S", "Beam Laser", 2, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1Cr,1ESED,1HE", "Beam Laser", 3, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1HV,1IED,1Se", "Beam Laser", 4, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1Cd,1PHR,1UED", "Beam Laser", 5, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1P", "Beam Laser", 1, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1Mn,1SAll", "Beam Laser", 2, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Beam Laser", 3, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Beam Laser", 4, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Beam Laser", 5, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1S", "Beam Laser", 1, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1MCF,1S", "Beam Laser", 2, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1FoC,1MCF,1S", "Beam Laser", 3, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1CPo,1FoC,1MCF", "Beam Laser", 4, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1BiC,1CIF,1ThA", "Beam Laser", 5, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1Ni", "Beam Laser", 1, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CCo,1Ni", "Beam Laser", 2, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CCo,1EA,1Ni", "Beam Laser", 3, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CCe,1PCa,1Zn", "Beam Laser", 4, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CPo,1MEF,1Zr", "Beam Laser", 5, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1Ni", "Beam Laser", 1, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1MCF,1Ni", "Beam Laser", 2, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1EA,1MCF,1Ni", "Beam Laser", 3, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1CPo,1EA,1MCF", "Beam Laser", 4, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1BiC,1CCom,1CIF", "Beam Laser", 5, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni", "Beam Laser", 1, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni,1SE", "Beam Laser", 2, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Nb,1SE,1W", "Beam Laser", 3, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Mo,1W,1Zn", "Beam Laser", 4, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1HDC,1W,1Tc", "Beam Laser", 5, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1S", "Burst Laser", 1, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1HDP,1S", "Burst Laser", 2, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1Cr,1ESED,1HE", "Burst Laser", 3, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1HV,1IED,1Se", "Burst Laser", 4, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1Cd,1PHR,1UED", "Burst Laser", 5, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Focused", "1Fe", "Burst Laser", 1, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Focused", "1CCo,1Fe", "Burst Laser", 2, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Focused", "1Cr,1CCe,1Fe", "Burst Laser", 3, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Focused", "1FoC,1Ge,1PCa", "Burst Laser", 4, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Focused", "1MSC,1Nb,1RFC", "Burst Laser", 5, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1P", "Burst Laser", 1, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1Mn,1SAll", "Burst Laser", 2, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Burst Laser", 3, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Burst Laser", 4, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Burst Laser", 5, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1S", "Burst Laser", 1, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1MCF,1S", "Burst Laser", 2, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1FoC,1MCF,1S", "Burst Laser", 3, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1CPo,1FoC,1MCF", "Burst Laser", 4, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1BiC,1CIF,1ThA", "Burst Laser", 5, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1Ni", "Burst Laser", 1, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CCo,1Ni", "Burst Laser", 2, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CCo,1EA,1Ni", "Burst Laser", 3, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CCe,1PCa,1Zn", "Burst Laser", 4, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CPo,1MCF,1Zr", "Burst Laser", 5, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1MS", "Burst Laser", 1, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1HDP,1MS", "Burst Laser", 2, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1ME,1PAll,1SLF", "Burst Laser", 3, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1MC,1MCF,1ThA", "Burst Laser", 4, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1CCom,1PAll,1Tc", "Burst Laser", 5, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1Ni", "Burst Laser", 1, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1MCF,1Ni", "Burst Laser", 2, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1EA,1MCF,1Ni", "Burst Laser", 3, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1CPo,1EA,1MCF", "Burst Laser", 4, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1BiC,1CCom,1CIF", "Burst Laser", 5, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni", "Burst Laser", 1, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni,1SE", "Burst Laser", 2, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni,1SE,1W", "Burst Laser", 3, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Mo,1W,1Zn", "Burst Laser", 4, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1HDC,1Mo,1Tc", "Burst Laser", 5, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1S", "Cannon", 1, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1HDP,1S", "Cannon", 2, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1Cr,1ESED,1HE", "Cannon", 3, "The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1HV,1IED,1Se", "Cannon", 4, "The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1Cd,1PHR,1UED", "Cannon", 5, "The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1MS", "Cannon", 1, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1MS,1V", "Cannon", 2, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1MS,1Nb,1V", "Cannon", 3, "The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1HDC,1ME,1Sn", "Cannon", 4, "The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1MC,1MSC,1PCo", "Cannon", 5, "The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1P", "Cannon", 1, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1Mn,1SAll", "Cannon", 2, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Cannon", 3, "The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Cannon", 4, "The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Cannon", 5, "The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1S", "Cannon", 1, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1MCF,1S", "Cannon", 2, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1FoC,1MCF,1S", "Cannon", 3, "The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1CPo,1FoC,1MCF", "Cannon", 4, "The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1BiC,1CIF,1ThA", "Cannon", 5, "The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1Ni", "Cannon", 1, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CCo,1Ni", "Cannon", 2, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CCo,1EA,1Ni", "Cannon", 3, "The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CCe,1PCa,1Zn", "Cannon", 4, "The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CPo,1MEF,1Zr", "Cannon", 5, "The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1MS", "Cannon", 1, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1HDP,1MS", "Cannon", 2, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1ME,1PAll,1SLF", "Cannon", 3, "The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1MC,1MCF,1ThA", "Cannon", 4, "The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1CCom,1PAll,1Tc", "Cannon", 5, "The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1Ni", "Cannon", 1, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1MCF,1Ni", "Cannon", 2, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1EA,1MCF,1Ni", "Cannon", 3, "The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1CPo,1EA,1MCF", "Cannon", 4, "The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1BiC,1CCom,1CIF", "Cannon", 5, "The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni", "Cannon", 1, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni,1SE", "Cannon", 2, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni,1SE,1W", "Cannon", 3, "The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Mo,1W,1Zn", "Cannon", 4, "The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1HDC,1Mo,1Tc", "Cannon", 5, "The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Ammo Capacity", "1MS,1Nb,1V", "Chaff Launcher", 3, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1P", "Chaff Launcher", 1, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1Mn,1SAll", "Chaff Launcher", 2, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Chaff Launcher", 3, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Chaff Launcher", 4, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Chaff Launcher", 5, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni", "Chaff Launcher", 1, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni,1SE", "Chaff Launcher", 2, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni,1SE,1W", "Chaff Launcher", 3, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Mo,1W,1Zn", "Chaff Launcher", 4, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1HDC,1Mo,1Tc", "Chaff Launcher", 5, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1WSE", "Chaff Launcher", 1, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1SE", "Chaff Launcher", 2, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1HDC,1SE", "Chaff Launcher", 3, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1PCo,1SS,1V", "Chaff Launcher", 4, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1CoS,1CDC,1W", "Chaff Launcher", 5, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1P", "Collector Limpet Controller", 1, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1Mn,1SAll", "Collector Limpet Controller", 2, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Collector Limpet Controller", 3, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Collector Limpet Controller", 4, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Collector Limpet Controller", 5, "Tiana Fortune,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni", "Collector Limpet Controller", 1, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni,1SE", "Collector Limpet Controller", 2, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni,1SE,1W", "Collector Limpet Controller", 3, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Mo,1W,1Zn", "Collector Limpet Controller", 4, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1HDC,1Mo,1Tc", "Collector Limpet Controller", 5, "Tiana Fortune,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1WSE", "Collector Limpet Controller", 1, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1SE", "Collector Limpet Controller", 2, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1HDC,1SE", "Collector Limpet Controller", 3, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1PCo,1SS,1V", "Collector Limpet Controller", 4, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1CoS,1CDC,1W", "Collector Limpet Controller", 5, "Tiana Fortune,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Fast Scan", "1P", "Detailed Surface Scanner", 1, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new MaterialCommoditiesList.EngineeringRecipe("Fast Scan", "1FFC,1P", "Detailed Surface Scanner", 2, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new MaterialCommoditiesList.EngineeringRecipe("Fast Scan", "1FFC,1OSK,1P", "Detailed Surface Scanner", 3, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Felicity Farseer"),
            new MaterialCommoditiesList.EngineeringRecipe("Fast Scan", "1AEA,1FoC,1Mn", "Detailed Surface Scanner", 4, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Fast Scan", "1AEC,1As,1RFC", "Detailed Surface Scanner", 5, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1Fe", "Detailed Surface Scanner", 1, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1HC,1Fe", "Detailed Surface Scanner", 2, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1HC,1Fe,1UED", "Detailed Surface Scanner", 3, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Felicity Farseer"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1DED,1EA,1Ge", "Detailed Surface Scanner", 4, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1ACED,1Nb,1PCa", "Detailed Surface Scanner", 5, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Wide Angle", "1MS", "Detailed Surface Scanner", 1, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new MaterialCommoditiesList.EngineeringRecipe("Wide Angle", "1Ge,1MS", "Detailed Surface Scanner", 2, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new MaterialCommoditiesList.EngineeringRecipe("Wide Angle", "1CSD,1Ge,1MS", "Detailed Surface Scanner", 3, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Felicity Farseer"),
            new MaterialCommoditiesList.EngineeringRecipe("Wide Angle", "1DSD,1ME,1Nb", "Detailed Surface Scanner", 4, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Wide Angle", "1CSD,1MC,1Sn", "Detailed Surface Scanner", 5, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1P", "Electronic Countermeasure", 1, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1Mn,1SAll", "Electronic Countermeasure", 2, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Electronic Countermeasure", 3, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Electronic Countermeasure", 4, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Electronic Countermeasure", 5, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni", "Electronic Countermeasure", 1, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni,1SE", "Electronic Countermeasure", 2, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni,1SE,1W", "Electronic Countermeasure", 3, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Mo,1W,1Zn", "Electronic Countermeasure", 4, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1HDC,1Mo,1Tc", "Electronic Countermeasure", 5, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1WSE", "Electronic Countermeasure", 1, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1SE", "Electronic Countermeasure", 2, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1HDC", "Electronic Countermeasure", 3, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1PCo,1SS,1V", "Electronic Countermeasure", 4, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1CoS,1CDC,1W", "Electronic Countermeasure", 5, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Double Shot", "1C", "Fragment Cannon", 1, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Double Shot", "1C,1ME", "Fragment Cannon", 2, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Double Shot", "1C,1CIF,1ME", "Fragment Cannon", 3, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Double Shot", "1MC,1SFP,1V", "Fragment Cannon", 4, "Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Double Shot", "1CCo,1HDC,1MEF", "Fragment Cannon", 5, "Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1S", "Fragment Cannon", 1, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1HDP,1S", "Fragment Cannon", 2, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1Cr,1ESED,1HE", "Fragment Cannon", 3, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1HV,1IED,1Se", "Fragment Cannon", 4, "Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1Cd,1PHR,1UED", "Fragment Cannon", 5, "Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1MS", "Fragment Cannon", 1, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1MS,1V", "Fragment Cannon", 2, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1MS,1Nb,1V", "Fragment Cannon", 3, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1HDC,1ME,1Sn", "Fragment Cannon", 4, "Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1MC,1MSC,1PCo", "Fragment Cannon", 5, "Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1P", "Fragment Cannon", 1, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1Mn,1SAll", "Fragment Cannon", 2, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Fragment Cannon", 3, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Fragment Cannon", 4, "Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Fragment Cannon", 5, "Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1Ni", "Fragment Cannon", 1, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CCo,1Ni", "Fragment Cannon", 2, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CCo,1EA,1Ni", "Fragment Cannon", 3, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CCe,1PCa,1Zn", "Fragment Cannon", 4, "Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CPo,1MEF,1Zr", "Fragment Cannon", 5, "Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1MS", "Fragment Cannon", 1, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1HDP,1MS", "Fragment Cannon", 2, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1ME,1PAll,1SLF", "Fragment Cannon", 3, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1MC,1MCF,1ThA", "Fragment Cannon", 4, "Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1CCom,1PAll,1Tc", "Fragment Cannon", 5, "Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni", "Fragment Cannon", 1, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni,1SE", "Fragment Cannon", 2, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni,1SE,1W", "Fragment Cannon", 3, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Mo,1W,1Zn", "Fragment Cannon", 4, "Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1HDC,1Mo,1Tc", "Fragment Cannon", 5, "Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Faster Boot Sequence", "1GR", "Frame Shift Drive", 1, "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new MaterialCommoditiesList.EngineeringRecipe("Faster Boot Sequence", "1Cr,1GR", "Frame Shift Drive", 2, "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new MaterialCommoditiesList.EngineeringRecipe("Faster Boot Sequence", "1GR,1HDP,1Se", "Frame Shift Drive", 3, "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new MaterialCommoditiesList.EngineeringRecipe("Faster Boot Sequence", "1Cd,1HE,1HC", "Frame Shift Drive", 4, "Elvira Martuuk,Felicity Farseer"),
            new MaterialCommoditiesList.EngineeringRecipe("Faster Boot Sequence", "1EA,1HV,1Te", "Frame Shift Drive", 5, "Elvira Martuuk,Felicity Farseer"),
            new MaterialCommoditiesList.EngineeringRecipe("Increased Range", "1ADWE", "Frame Shift Drive", 1, "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new MaterialCommoditiesList.EngineeringRecipe("Increased Range", "1ADWE,1CP", "Frame Shift Drive", 2, "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new MaterialCommoditiesList.EngineeringRecipe("Increased Range", "1CP,1P,1SWS", "Frame Shift Drive", 3, "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new MaterialCommoditiesList.EngineeringRecipe("Increased Range", "1CD,1EHT,1Mn", "Frame Shift Drive", 4, "Elvira Martuuk,Felicity Farseer"),
            new MaterialCommoditiesList.EngineeringRecipe("Increased Range", "1As,1CM,1DWEx", "Frame Shift Drive", 5, "Elvira Martuuk,Felicity Farseer"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1Ni", "Frame Shift Drive", 1, "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1SE", "Frame Shift Drive", 2, "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1SS,1Zn", "Frame Shift Drive", 3, "Colonel Bris Dekker,Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1CoS,1HDC,1V", "Frame Shift Drive", 4, "Elvira Martuuk,Felicity Farseer"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1IS,1PCo,1W", "Frame Shift Drive", 5, "Elvira Martuuk,Felicity Farseer"),
            new MaterialCommoditiesList.EngineeringRecipe("Expanded Capture Arc", "1MS", "Frame Shift Drive Interdictor", 1, "Colonel Bris Dekker,Felicity Farseer,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Expanded Capture Arc", "1ME,1UEF", "Frame Shift Drive Interdictor", 2, "Colonel Bris Dekker,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Expanded Capture Arc", "1GR,1MC,1TEC", "Frame Shift Drive Interdictor", 3, "Colonel Bris Dekker,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Expanded Capture Arc", "1DSD,1ME,1SWS", "Frame Shift Drive Interdictor", 4, "Colonel Bris Dekker,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Expanded Capture Arc", "1CSD,1EHT,1MC", "Frame Shift Drive Interdictor", 5, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Longer Range", "1UEF", "Frame Shift Drive Interdictor", 1, "Colonel Bris Dekker,Felicity Farseer,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Longer Range", "1ADWE,1TEC", "Frame Shift Drive Interdictor", 2, "Colonel Bris Dekker,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Longer Range", "1ABSD,1AFT,1OSK", "Frame Shift Drive Interdictor", 3, "Colonel Bris Dekker,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1WSE", "Fuel Scoop", 1, "Lori Jameson,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1SE", "Fuel Scoop", 2, "Lori Jameson,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1HDC", "Fuel Scoop", 3, "Lori Jameson,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1PCo,1SS,1V", "Fuel Scoop", 4, "Lori Jameson,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1P", "Fuel Transfer Limpet Controller", 1, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1Mn,1SAll", "Fuel Transfer Limpet Controller", 2, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Fuel Transfer Limpet Controller", 3, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Fuel Transfer Limpet Controller", 4, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Fuel Transfer Limpet Controller", 5, "Tiana Fortune,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni", "Fuel Transfer Limpet Controller", 1, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni,1SE", "Fuel Transfer Limpet Controller", 2, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni,1SE,1W", "Fuel Transfer Limpet Controller", 3, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Mo,1W,1Zn", "Fuel Transfer Limpet Controller", 4, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1HDC,1Mo,1Tc", "Fuel Transfer Limpet Controller", 5, "Tiana Fortune,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1WSE", "Fuel Transfer Limpet Controller", 1, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1SE", "Fuel Transfer Limpet Controller", 2, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1HDC,1SE", "Fuel Transfer Limpet Controller", 3, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1PCo,1SS,1V", "Fuel Transfer Limpet Controller", 4, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1CoS,1CDC,1W", "Fuel Transfer Limpet Controller", 5, "Tiana Fortune,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1P", "Hatch Breaker Limpet Controller", 1, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1Mn,1SAll", "Hatch Breaker Limpet Controller", 2, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Hatch Breaker Limpet Controller", 3, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Hatch Breaker Limpet Controller", 4, "Tiana Fortune,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Hatch Breaker Limpet Controller", 5, "Tiana Fortune,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni", "Hatch Breaker Limpet Controller", 1, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni,1SE", "Hatch Breaker Limpet Controller", 2, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni,1SE,1W", "Hatch Breaker Limpet Controller", 3, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Mo,1W,1Zn", "Hatch Breaker Limpet Controller", 4, "Tiana Fortune,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1HDC,1Mo,1Tc", "Hatch Breaker Limpet Controller", 5, "Tiana Fortune,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1WSE", "Hatch Breaker Limpet Controller", 1, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1SE", "Hatch Breaker Limpet Controller", 2, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1HDC,1SE", "Hatch Breaker Limpet Controller", 3, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1PCo,1SS,1V", "Hatch Breaker Limpet Controller", 4, "Tiana Fortune,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1CoS,1CDC,1W", "Hatch Breaker Limpet Controller", 5, "Tiana Fortune,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Ammo Capacity", "1MS,1Nb,1V", "Heat Sink Launcher", 3, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1P", "Heat Sink Launcher", 1, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1Mn,1SAll", "Heat Sink Launcher", 2, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Heat Sink Launcher", 3, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Heat Sink Launcher", 4, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Heat Sink Launcher", 5, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni", "Heat Sink Launcher", 1, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni,1SE", "Heat Sink Launcher", 2, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni,1SE,1W", "Heat Sink Launcher", 3, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Mo,1W,1Zn", "Heat Sink Launcher", 4, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1HDC,1Mo,1Tc", "Heat Sink Launcher", 5, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1WSE", "Heat Sink Launcher", 1, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1SE", "Heat Sink Launcher", 2, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1HDC,1SE", "Heat Sink Launcher", 3, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1PCo,1SS,1V", "Heat Sink Launcher", 4, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1CoS,1CDC,1W", "Heat Sink Launcher", 5, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Blast Resistant", "1Ni", "Hull Reinforcement Package", 1, "Liz Ryder,Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Blast Resistant", "1C,1Zn", "Hull Reinforcement Package", 2, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Blast Resistant", "1SAll,1V,1Zr", "Hull Reinforcement Package", 3, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Blast Resistant", "1GA,1Hg,1W", "Hull Reinforcement Package", 4, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Blast Resistant", "1Mo,1PA,1Ru", "Hull Reinforcement Package", 5, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Heavy Duty", "1C", "Hull Reinforcement Package", 1, "Liz Ryder,Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Heavy Duty", "1C,1SE", "Hull Reinforcement Package", 2, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Heavy Duty", "1C,1HDC,1SE", "Hull Reinforcement Package", 3, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Heavy Duty", "1PCo,1SS,1V", "Hull Reinforcement Package", 4, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Heavy Duty", "1CoS,1CDC,1W", "Hull Reinforcement Package", 5, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Kinetic Resistant", "1Ni", "Hull Reinforcement Package", 1, "Liz Ryder,Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Kinetic Resistant", "1Ni,1V", "Hull Reinforcement Package", 2, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Kinetic Resistant", "1HDC,1SAll,1V", "Hull Reinforcement Package", 3, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Kinetic Resistant", "1GA,1PCo,1W", "Hull Reinforcement Package", 4, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Kinetic Resistant", "1CDC,1Mo,1PA", "Hull Reinforcement Package", 5, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1Fe", "Hull Reinforcement Package", 1, "Liz Ryder,Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCo,1Fe", "Hull Reinforcement Package", 2, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCo,1HDC,1Fe", "Hull Reinforcement Package", 3, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1Ge,1PCo", "Hull Reinforcement Package", 4, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1MGA,1Sn", "Hull Reinforcement Package", 5, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Thermal Resistant", "1HCW", "Hull Reinforcement Package", 1, "Liz Ryder,Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Thermal Resistant", "1HDP,1Ni", "Hull Reinforcement Package", 2, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Thermal Resistant", "1HE,1SAll,1V", "Hull Reinforcement Package", 3, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Thermal Resistant", "1GA,1HV,1W", "Hull Reinforcement Package", 4, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Thermal Resistant", "1Mo,1PA,1PHR", "Hull Reinforcement Package", 5, "Selene Jean"),
            new MaterialCommoditiesList.EngineeringRecipe("Fast Scan", "1P", "Kill Warrant Scanner", 1, "Juri Ishmaak,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Fast Scan", "1FFC,1P", "Kill Warrant Scanner", 2, "Juri Ishmaak,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Fast Scan", "1FFC,1OSK,1P", "Kill Warrant Scanner", 3, "Juri Ishmaak,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Fast Scan", "1AEA,1FoC,1Mn", "Kill Warrant Scanner", 4, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Fast Scan", "1AEC,1As,1RFC", "Kill Warrant Scanner", 5, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1P", "Kill Warrant Scanner", 1, "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1Mn,1SAll", "Kill Warrant Scanner", 2, "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Kill Warrant Scanner", 3, "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Kill Warrant Scanner", 4, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Kill Warrant Scanner", 5, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1Fe", "Kill Warrant Scanner", 1, "Juri Ishmaak,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1HC,1Fe", "Kill Warrant Scanner", 2, "Juri Ishmaak,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1HC,1Fe,1UED", "Kill Warrant Scanner", 3, "Juri Ishmaak,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1DED,1EA,1Ge", "Kill Warrant Scanner", 4, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1ACED,1Nb,1PCa", "Kill Warrant Scanner", 5, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni", "Kill Warrant Scanner", 1, "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni,1SE", "Kill Warrant Scanner", 2, "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni,1SE,1W", "Kill Warrant Scanner", 3, "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Mo,1W,1Zn", "Kill Warrant Scanner", 4, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1HDC,1Mo,1Tc", "Kill Warrant Scanner", 5, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1WSE", "Kill Warrant Scanner", 1, "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1SE", "Kill Warrant Scanner", 2, "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1HDC,1SE", "Kill Warrant Scanner", 3, "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1PCo,1SS,1V", "Kill Warrant Scanner", 4, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1CoS,1CDC,1W", "Kill Warrant Scanner", 5, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Wide Angle", "1MS", "Kill Warrant Scanner", 1, "Juri Ishmaak,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Wide Angle", "1Ge,1MS", "Kill Warrant Scanner", 2, "Juri Ishmaak,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Wide Angle", "1CSD,1Ge,1MS", "Kill Warrant Scanner", 3, "Juri Ishmaak,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Wide Angle", "1DSD,1ME,1Nb", "Kill Warrant Scanner", 4, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Wide Angle", "1CSF,1MC,1Sn", "Kill Warrant Scanner", 5, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1P", "Life Support", 1, "Lori Jameson,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1Mn,1SAll", "Life Support", 2, "Lori Jameson,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Life Support", 3, "Lori Jameson,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Life Support", 4, "Lori Jameson"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni", "Life Support", 1, "Lori Jameson,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni,1SE", "Life Support", 2, "Lori Jameson,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni,1SE,1W", "Life Support", 3, "Lori Jameson,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Mo,1W,1Zn", "Life Support", 4, "Lori Jameson"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1WSE", "Life Support", 1, "Lori Jameson,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1SE", "Life Support", 2, "Lori Jameson,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1HDC,1SE", "Life Support", 3, "Lori Jameson,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1PCo,1SS,1V", "Life Support", 4, "Lori Jameson"),
            new MaterialCommoditiesList.EngineeringRecipe("Fast Scan", "1P", "Manifest Scanner", 1, "Juri Ishmaak,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Fast Scan", "1FFC,1P", "Manifest Scanner", 2, "Juri Ishmaak,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Fast Scan", "1FFC,1OSK,1P", "Manifest Scanner", 3, "Juri Ishmaak,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Fast Scan", "1AEA,1FoC,1Mn", "Manifest Scanner", 4, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Fast Scan", "1AEC,1As,1RFC", "Manifest Scanner", 5, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1P", "Manifest Scanner", 1, "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1Mn,1SAll", "Manifest Scanner", 2, "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Manifest Scanner", 3, "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Manifest Scanner", 4, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Manifest Scanner", 5, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1Fe", "Manifest Scanner", 1, "Juri Ishmaak,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1HC,1Fe", "Manifest Scanner", 2, "Juri Ishmaak,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1HC,1Fe,1UED", "Manifest Scanner", 3, "Juri Ishmaak,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1DED,1EA,1Ge", "Manifest Scanner", 4, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1ACED,1Nb,1PCa", "Manifest Scanner", 5, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni", "Manifest Scanner", 1, "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni,1SE", "Manifest Scanner", 2, "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni,1SE,1W", "Manifest Scanner", 3, "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Mo,1W,1Zn", "Manifest Scanner", 4, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1HDC,1Mo,1Tc", "Manifest Scanner", 5, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1WSE", "Manifest Scanner", 1, "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1SE", "Manifest Scanner", 2, "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1HDC,1SE", "Manifest Scanner", 3, "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1PCo,1SS,1V", "Manifest Scanner", 4, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1CoS,1CDC,1W", "Manifest Scanner", 5, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Wide Angle", "1MS", "Manifest Scanner", 1, "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Wide Angle", "1Ge,1MS", "Manifest Scanner", 2, "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Wide Angle", "1CSD,1Ge,1MS", "Manifest Scanner", 3, "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Wide Angle", "1DSD,1ME,1Nb", "Manifest Scanner", 4, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Wide Angle", "1CSF,1MC,1Sn", "Manifest Scanner", 5, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1MS", "Mine Launcher", 1, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1MS,1V", "Mine Launcher", 2, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1MS,1Nb,1V", "Mine Launcher", 3, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1HDC,1ME,1Sn", "Mine Launcher", 4, "Juri Ishmaak"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1MC,1MSC,1PCo", "Mine Launcher", 5, "Juri Ishmaak"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1P", "Mine Launcher", 1, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1Mn,1SAll", "Mine Launcher", 2, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Mine Launcher", 3, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Mine Launcher", 4, "Juri Ishmaak"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Mine Launcher", 5, "Juri Ishmaak"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1MS", "Mine Launcher", 1, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1HDP,1MS", "Mine Launcher", 2, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1ME,1PAll,1SLF", "Mine Launcher", 3, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1MC,1MCF,1ThA", "Mine Launcher", 4, "Juri Ishmaak"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1CCom,1PAll,1Tc", "Mine Launcher", 5, "Juri Ishmaak"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni", "Mine Launcher", 1, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni,1SE", "Mine Launcher", 2, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni,1SE,1W", "Mine Launcher", 3, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Mo,1W,1Zn", "Mine Launcher", 4, "Juri Ishmaak"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1HDC,1Mo,1Tc", "Mine Launcher", 5, "Juri Ishmaak"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1MS", "Missile Rack", 1, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1MS,1V", "Missile Rack", 2, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1MS,1Nb,1V", "Missile Rack", 3, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1HDC,1ME,1Sn", "Missile Rack", 4, "Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1MC,1MSC,1PCo", "Missile Rack", 5, "Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1P", "Missile Rack", 1, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1Mn,1SAll", "Missile Rack", 2, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Missile Rack", 3, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Missile Rack", 4, "Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Missile Rack", 5, "Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1MS", "Missile Rack", 1, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1HDP,1MS", "Missile Rack", 2, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1ME,1PAll,1SLF", "Missile Rack", 3, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1MC,1MCF,1ThA", "Missile Rack", 4, "Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1CCom,1PAll,1Tc", "Missile Rack", 5, "Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni", "Missile Rack", 1, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni,1SE", "Missile Rack", 2, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni,1SE,1W", "Missile Rack", 3, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Mo,1W,1Zn", "Missile Rack", 4, "Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1HDC,1Mo,1Tc", "Missile Rack", 5, "Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1S", "Multi-cannon", 1, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1HDP,1S", "Multi-cannon", 2, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1Cr,1ESED,1HE", "Multi-cannon", 3, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1HV,1IED,1Se", "Multi-cannon", 4, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1Cd,1PHR,1UED", "Multi-cannon", 5, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1MS", "Multi-cannon", 1, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1MS,1V", "Multi-cannon", 2, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1MS,1Nb,1V", "Multi-cannon", 3, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1HDC,1ME,1Sn", "Multi-cannon", 4, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1MC,1MSC,1PCo", "Multi-cannon", 5, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1P", "Multi-cannon", 1, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1Mn,1SAll", "Multi-cannon", 2, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Multi-cannon", 3, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Multi-cannon", 4, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Multi-cannon", 5, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1S", "Multi-cannon", 1, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1MCF,1S", "Multi-cannon", 2, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1FoC,1MCF,1S", "Multi-cannon", 3, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1CPo,1FoC,1MCF", "Multi-cannon", 4, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1BiC,1CIF,1ThA", "Multi-cannon", 5, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1Ni", "Multi-cannon", 1, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CCo,1Ni", "Multi-cannon", 2, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CCo,1EA,1Ni", "Multi-cannon", 3, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CCe,1PCa,1Zn", "Multi-cannon", 4, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CPo,1MEF,1Zr", "Multi-cannon", 5, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1MS", "Multi-cannon", 1, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1HDP,1MS", "Multi-cannon", 2, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1ME,1PAll,1SLF", "Multi-cannon", 3, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1MC,1MCF,1ThA", "Multi-cannon", 4, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1CCom,1PAll,1Tc", "Multi-cannon", 5, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1Ni", "Multi-cannon", 1, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1MCF,1Ni", "Multi-cannon", 2, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1EA,1MCF,1Ni", "Multi-cannon", 3, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1CPo,1EA,1MCF", "Multi-cannon", 4, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1BiC,1CCom,1CIF", "Multi-cannon", 5, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni", "Multi-cannon", 1, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni,1SE", "Multi-cannon", 2, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni,1SE,1W", "Multi-cannon", 3, "Tod \"The Blaster\" McQuinn,Zacariah Nemo"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Mo,1W,1Zn", "Multi-cannon", 4, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1HDC,1Mo,1Tc", "Multi-cannon", 5, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1S", "Plasma Accelerator", 1, "Zacariah Nemo,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1HDP,1S", "Plasma Accelerator", 2, "Zacariah Nemo,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1Cr,1ESED,1HE", "Plasma Accelerator", 3, "Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1HV,1IED,1Se", "Plasma Accelerator", 4, "Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1Cd,1PHR,1UED", "Plasma Accelerator", 5, "Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Focused", "1Fe", "Plasma Accelerator", 1, "Zacariah Nemo,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Focused", "1CCo,1Fe", "Plasma Accelerator", 2, "Zacariah Nemo,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Focused", "1Cr,1CCe,1Fe", "Plasma Accelerator", 3, "Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Focused", "1FoC,1Ge,1PCa", "Plasma Accelerator", 4, "Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Focused", "1MSC,1Nb,1RFC", "Plasma Accelerator", 5, "Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1P", "Plasma Accelerator", 1, "Zacariah Nemo,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1Mn,1SAll", "Plasma Accelerator", 2, "Zacariah Nemo,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Plasma Accelerator", 3, "Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Plasma Accelerator", 4, "Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Plasma Accelerator", 5, "Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1S", "Plasma Accelerator", 1, "Zacariah Nemo,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1MCF,1S", "Plasma Accelerator", 2, "Zacariah Nemo,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1FoC,1MCF,1S", "Plasma Accelerator", 3, "Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1CPo,1FoC,1MCF", "Plasma Accelerator", 4, "Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1BiC,1CIF,1ThA", "Plasma Accelerator", 5, "Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1Ni", "Plasma Accelerator", 1, "Zacariah Nemo,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CCo,1Ni", "Plasma Accelerator", 2, "Zacariah Nemo,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CCo,1EA,1Ni", "Plasma Accelerator", 3, "Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CCe,1PCa,1Zn", "Plasma Accelerator", 4, "Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CPo,1MEF,1Zr", "Plasma Accelerator", 5, "Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1MS", "Plasma Accelerator", 1, "Zacariah Nemo,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1HDP,1MS", "Plasma Accelerator", 2, "Zacariah Nemo,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1ME,1PAll,1SLF", "Plasma Accelerator", 3, "Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1MC,1MCF,1ThA", "Plasma Accelerator", 4, "Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1CCom,1PAll,1Tc", "Plasma Accelerator", 5, "Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1Ni", "Plasma Accelerator", 1, "Zacariah Nemo,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1MCF,1Ni", "Plasma Accelerator", 2, "Zacariah Nemo,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1EA,1MCF,1Ni", "Plasma Accelerator", 3, "Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1CPo,1EA,1MCF", "Plasma Accelerator", 4, "Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1BiC,1CCom,1CIF", "Plasma Accelerator", 5, "Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni", "Plasma Accelerator", 1, "Zacariah Nemo,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni,1SE", "Plasma Accelerator", 2, "Zacariah Nemo,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni,1SE,1W", "Plasma Accelerator", 3, "Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Mo,1W,1Zn", "Plasma Accelerator", 4, "Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1HDC,1Mo,1Tc", "Plasma Accelerator", 5, "Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Ammo Capacity", "1MS,1Nb,1V", "Point Defence", 3, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1P", "Point Defence", 1, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1Mn,1SAll", "Point Defence", 2, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Point Defence", 3, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Point Defence", 4, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Point Defence", 5, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni", "Point Defence", 1, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni,1SE", "Point Defence", 2, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni,1SE,1W", "Point Defence", 3, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Mo,1W,1Zn", "Point Defence", 4, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1HDC,1Mo,1Tc", "Point Defence", 5, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1WSE", "Point Defence", 1, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1SE", "Point Defence", 2, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1HDC,1SE", "Point Defence", 3, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1PCo,1SS,1V", "Point Defence", 4, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1CoS,1CDC,1W", "Point Defence", 5, "Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Charge Enhanced", "1SLF", "Power Distributor", 1, "Hera Tani,Marco Qwent,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Charge Enhanced", "1CP,1SLF", "Power Distributor", 2, "Hera Tani,Marco Qwent,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Charge Enhanced", "1CD,1GR,1MCF", "Power Distributor", 3, "Hera Tani,Marco Qwent,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Charge Enhanced", "1CM,1CIF,1HC", "Power Distributor", 4, "The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Charge Enhanced", "1CM,1CIF,1EFC", "Power Distributor", 5, "The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Engine Focused", "1S", "Power Distributor", 1, "Hera Tani,Marco Qwent,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Engine Focused", "1CCo,1S", "Power Distributor", 2, "Hera Tani,Marco Qwent,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Engine Focused", "1ABSD,1Cr,1EA", "Power Distributor", 3, "Hera Tani,Marco Qwent,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("High Charge Capacity", "1S", "Power Distributor", 1, "Hera Tani,Marco Qwent,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("High Charge Capacity", "1Cr,1SLF", "Power Distributor", 2, "Hera Tani,Marco Qwent,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("High Charge Capacity", "1Cr,1HDC,1SLF", "Power Distributor", 3, "Hera Tani,Marco Qwent,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("High Charge Capacity", "1MCF,1PCo,1Se", "Power Distributor", 4, "The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("High Charge Capacity", "1CIF,1MSC,1PCo", "Power Distributor", 5, "The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1WSE", "Power Distributor", 1, "Hera Tani,Marco Qwent,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1SE", "Power Distributor", 2, "Hera Tani,Marco Qwent,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1HDC,1SE", "Power Distributor", 3, "Hera Tani,Marco Qwent,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1PCo,1SS,1V", "Power Distributor", 4, "The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1CoS,1CDC,1W", "Power Distributor", 5, "The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("System Focused", "1S", "Power Distributor", 1, "Hera Tani,Marco Qwent,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("System Focused", "1CCo,1S", "Power Distributor", 2, "Hera Tani,Marco Qwent,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("System Focused", "1ABSD,1Cr,1EA", "Power Distributor", 3, "Hera Tani,Marco Qwent,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Weapon Focused", "1S", "Power Distributor", 1, "Hera Tani,Marco Qwent,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Weapon Focused", "1CCo,1S", "Power Distributor", 2, "Hera Tani,Marco Qwent,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Weapon Focused", "1ABSD,1HC,1Se", "Power Distributor", 3, "Hera Tani,Marco Qwent,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Armoured", "1WSE", "Power Plant", 1, "Felicity Farseer,Hera Tani,Marco Qwent"),
            new MaterialCommoditiesList.EngineeringRecipe("Armoured", "1C,1SE", "Power Plant", 2, "Hera Tani,Marco Qwent"),
            new MaterialCommoditiesList.EngineeringRecipe("Armoured", "1C,1HDC,1SE", "Power Plant", 3, "Hera Tani,Marco Qwent"),
            new MaterialCommoditiesList.EngineeringRecipe("Armoured", "1PCo,1SS,1V", "Power Plant", 4, "Hera Tani,Marco Qwent"),
            new MaterialCommoditiesList.EngineeringRecipe("Armoured", "1CoS,1CDC,1W", "Power Plant", 5, "Hera Tani"),
            new MaterialCommoditiesList.EngineeringRecipe("Low Emissions", "1Fe", "Power Plant", 1, "Felicity Farseer,Hera Tani,Marco Qwent"),
            new MaterialCommoditiesList.EngineeringRecipe("Low Emissions", "1Fe,1IED", "Power Plant", 2, "Hera Tani,Marco Qwent"),
            new MaterialCommoditiesList.EngineeringRecipe("Low Emissions", "1HE,1Fe,1IED", "Power Plant", 3, "Hera Tani,Marco Qwent"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1S", "Power Plant", 1, "Felicity Farseer,Hera Tani,Marco Qwent"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CCo,1HCW", "Power Plant", 2, "Hera Tani,Marco Qwent"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CCo,1HCW,1Se", "Power Plant", 3, "Hera Tani,Marco Qwent"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1Cd,1CCe,1HDP", "Power Plant", 4, "Hera Tani,Marco Qwent"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CM,1CCe,1Te", "Power Plant", 5, "Hera Tani"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1P", "Prospector Limpet Controller", 1, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1Mn,1SAll", "Prospector Limpet Controller", 2, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Prospector Limpet Controller", 3, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Prospector Limpet Controller", 4, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Prospector Limpet Controller", 5, "Tiana Fortune,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni", "Prospector Limpet Controller", 1, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni,1SE", "Prospector Limpet Controller", 2, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni,1SE,1W", "Prospector Limpet Controller", 3, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Mo,1W,1Zn", "Prospector Limpet Controller", 4, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1HDC,1Mo,1Tc", "Prospector Limpet Controller", 5, "Tiana Fortune,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1WSE", "Prospector Limpet Controller", 1, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1SE", "Prospector Limpet Controller", 2, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1HDC", "Prospector Limpet Controller", 3, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1PCo,1SS,1V", "Prospector Limpet Controller", 4, "Tiana Fortune,The Sarge,Ram Tah"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1CoS,1CDC,1W", "Prospector Limpet Controller", 5, "Tiana Fortune,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1S", "Pulse Laser", 1, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1HDP,1S", "Pulse Laser", 2, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1Cr,1ESED,1HE", "Pulse Laser", 3, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1HV,1IED,1Se", "Pulse Laser", 4, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Efficient Weapon", "1Cd,1PHR,1UED", "Pulse Laser", 5, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Focused", "1Fe", "Pulse Laser", 1, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Focused", "1CCo,1Fe", "Pulse Laser", 2, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Focused", "1Cr,1CCe,1Fe", "Pulse Laser", 3, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Focused", "1FoC,1Ge,1PCa", "Pulse Laser", 4, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Focused", "1MSC,1Nb,1RFC", "Pulse Laser", 1, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1P", "Pulse Laser", 1, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1Mn,1SAll", "Pulse Laser", 2, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Pulse Laser", 3, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Pulse Laser", 4, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Pulse Laser", 5, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1S", "Pulse Laser", 1, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1MCF,1S", "Pulse Laser", 2, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1FoC,1MCF,1S", "Pulse Laser", 3, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1CPo,1FoC,1MCF", "Pulse Laser", 4, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1BiC,1CIF,1ThA", "Pulse Laser", 5, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1Ni", "Pulse Laser", 1, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CCo,1Ni", "Pulse Laser", 2, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CCo,1EA,1Ni", "Pulse Laser", 3, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CCe,1PCa,1Zn", "Pulse Laser", 4, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Overcharged", "1CPo,1MEF,1Zr", "Pulse Laser", 5, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1MS", "Pulse Laser", 1, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1HDP,1MS", "Pulse Laser", 2, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1ME,1PAll,1SLF", "Pulse Laser", 3, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1MC,1MCF,1ThA", "Pulse Laser", 4, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Fire", "1CCom,1PAll,1Tc", "Pulse Laser", 5, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster","1Ni","Pulse Laser", 1, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster","1MCF,1Ni","Pulse Laser", 2, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster","1EA,1MCF,1Ni","Pulse Laser", 3, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster","1CPo,1EA,1MCF","Pulse Laser", 4, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster","1BiC,1CCom,1CIF","Pulse Laser", 5, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni", "Pulse Laser", 1, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni,1SE", "Pulse Laser", 2, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni,1SE,1W", "Pulse Laser", 3, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Mo,1W,1Zn", "Pulse Laser", 4, "Broo Tarquin,The Dweller"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1HDC,1Mo,1Tc", "Pulse Laser", 5, "Broo Tarquin"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1MS", "Rail Gun", 1, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1MS,1V", "Rail Gun", 2, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1MS,1Nb,1V", "Rail Gun", 3, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1HDC,1ME,1Sn", "Rail Gun", 4, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("High Capacity Magazine", "1MC,1MSC,1PCo", "Rail Gun", 5, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1P", "Rail Gun", 1, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1Mn,1SAll", "Rail Gun", 2, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Rail Gun", 3, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Rail Gun", 4, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Rail Gun", 5, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1S", "Rail Gun", 1, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1MCF,1S", "Rail Gun", 2, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1FoC,1MCF,1S", "Rail Gun", 3, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1CPo,1FoC,1MCF", "Rail Gun", 4, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1BiC,1CIF,1ThA", "Rail Gun", 5, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1Ni", "Rail Gun", 1, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1MCF,1Ni", "Rail Gun", 2, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1EA,1MCF,1Ni", "Rail Gun", 3, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1CPo,1EA,1MCF", "Rail Gun", 4, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Short Range Blaster", "1BiC,1CCom,1CIF", "Rail Gun", 5, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni", "Rail Gun", 1, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1SE,1Ni", "Rail Gun", 2, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni,1SE,1W", "Rail Gun", 3, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Mo,1W,1Zn", "Rail Gun", 4, "Tod \"The Blaster\" McQuinn"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1HDC,1Mo,1Tc", "Rail Gun", 5, "Tod \"The Blaster\" McQuinn,The Sarge"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1WSE", "Refinery", 1, "Lori Jameson,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1SE", "Refinery", 2, "Lori Jameson,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1HDC", "Refinery", 3, "Lori Jameson,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1PCo,1SS,1V", "Refinery", 4, "Lori Jameson,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1P", "Sensors", 1, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1Mn,1SAll", "Sensors", 2, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Sensors", 3, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Sensors", 4, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Sensors", 5, "Juri Ishmaak,Lori Jameson,Lei Cheung,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1Fe", "Sensors", 1, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1HC,1Fe", "Sensors", 2, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1HC,1Fe,1UED", "Sensors", 3, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1DED,1EA,1Ge", "Sensors", 4, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1ACED,1Nb,1PCa", "Sensors", 5, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Wide Angle", "1MS", "Sensors", 1, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new MaterialCommoditiesList.EngineeringRecipe("Wide Angle", "1Ge,1MS", "Sensors", 2, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new MaterialCommoditiesList.EngineeringRecipe("Wide Angle", "1CSD,1Ge,1MS", "Sensors", 3, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner,Felicity Farseer"),
            new MaterialCommoditiesList.EngineeringRecipe("Wide Angle", "1DSD,1ME,1Nb", "Sensors", 4, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Wide Angle", "1CSF,1MC,1Sn", "Sensors", 5, "Juri Ishmaak,Lei Cheung,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Blast Resistant", "1Fe", "Shield Booster", 1, "Didi Vatermann,Felicity Farseer,Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Blast Resistant", "1CCo,1Fe", "Shield Booster", 2, "Didi Vatermann,Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Blast Resistant", "1CCo,1FoC,1Fe", "Shield Booster", 3, "Didi Vatermann,Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Blast Resistant", "1Ge,1RFC,1USS", "Shield Booster", 4, "Didi Vatermann"),
            new MaterialCommoditiesList.EngineeringRecipe("Blast Resistant", "1ASPA,1EFC,1Nb", "Shield Booster", 5, "Didi Vatermann"),
            new MaterialCommoditiesList.EngineeringRecipe("Heavy Duty", "1GR", "Shield Booster", 1, "Didi Vatermann,Felicity Farseer,Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Heavy Duty", "1DSCR,1HC", "Shield Booster", 2, "Didi Vatermann,Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Heavy Duty", "1DSCR,1HC,1Nb", "Shield Booster", 3, "Didi Vatermann,Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Heavy Duty", "1EA,1ISSA,1Sn", "Shield Booster", 4, "Didi Vatermann"),
            new MaterialCommoditiesList.EngineeringRecipe("Heavy Duty", "1Sb,1PCa,1USS", "Shield Booster", 5, "Didi Vatermann"),
            new MaterialCommoditiesList.EngineeringRecipe("Kinetic Resistant", "1Fe", "Shield Booster", 1, "Didi Vatermann,Felicity Farseer,Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Kinetic Resistant", "1Ge,1GR", "Shield Booster", 2, "Didi Vatermann,Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Kinetic Resistant", "1FoC,1HC,1SAll", "Shield Booster", 3, "Didi Vatermann,Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Kinetic Resistant", "1GA,1RFC,1USS", "Shield Booster", 4, "Didi Vatermann"),
            new MaterialCommoditiesList.EngineeringRecipe("Kinetic Resistant", "1ASPA,1EFC,1PA", "Shield Booster", 5, "Didi Vatermann"),
            new MaterialCommoditiesList.EngineeringRecipe("Resistance Augmented", "1P", "Shield Booster", 1, "Didi Vatermann,Felicity Farseer,Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Resistance Augmented", "1CCo,1P", "Shield Booster", 2, "Didi Vatermann,Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Resistance Augmented", "1CCo,1FoC,1P", "Shield Booster", 3, "Didi Vatermann,Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Resistance Augmented", "1CCe,1Mn,1RFC", "Shield Booster", 4, "Didi Vatermann"),
            new MaterialCommoditiesList.EngineeringRecipe("Resistance Augmented", "1CCe,1IS,1RFC", "Shield Booster", 5, "Didi Vatermann"),
            new MaterialCommoditiesList.EngineeringRecipe("Thermal Resistant", "1Fe", "Shield Booster", 1, "Didi Vatermann,Felicity Farseer,Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Thermal Resistant", "1Ge,1HCW", "Shield Booster", 2, "Didi Vatermann,Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Thermal Resistant", "1FoC,1HCW,1HDP", "Shield Booster", 3, "Didi Vatermann,Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Thermal Resistant", "1HDP,1RFC,1USS", "Shield Booster", 4, "Didi Vatermann"),
            new MaterialCommoditiesList.EngineeringRecipe("Thermal Resistant", "1ASPA,1EFC,1HE", "Shield Booster", 5, "Didi Vatermann"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Charge", "1S", "Shield Cell Bank", 1, "Elvira Martuuk,Lori Jameson"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Charge", "1Cr,1GR", "Shield Cell Bank", 2, "Lori Jameson"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Charge", "1HC,1PAll,1S", "Shield Cell Bank", 3, "Lori Jameson"),
            new MaterialCommoditiesList.EngineeringRecipe("Rapid Charge", "1Cr,1EA,1ThA", "Shield Cell Bank", 4, "Lori Jameson"),
            new MaterialCommoditiesList.EngineeringRecipe("Specialised", "1SLF", "Shield Cell Bank", 1, "Elvira Martuuk,Lori Jameson"),
            new MaterialCommoditiesList.EngineeringRecipe("Specialised", "1CCo,1SLF", "Shield Cell Bank", 2, "Lori Jameson"),
            new MaterialCommoditiesList.EngineeringRecipe("Specialised", "1CCo,1CIF,1IED", "Shield Cell Bank", 3, "Lori Jameson"),
            new MaterialCommoditiesList.EngineeringRecipe("Enhanced, Low Power", "1DSCR", "Shield Generator", 1, "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Enhanced, Low Power", "1DSCR,1Ge", "Shield Generator", 2, "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Enhanced, Low Power", "1DSCR,1Ge,1PAll", "Shield Generator", 3, "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Enhanced, Low Power", "1ISSA,1Nb,1ThA", "Shield Generator", 4, "Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Enhanced, Low Power", "1MGA,1Sn,1USS", "Shield Generator", 5, "Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Kinetic Resistant", "1DSCR", "Shield Generator", 1, "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Kinetic Resistant", "1DSCR,1MCF", "Shield Generator", 2, "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Kinetic Resistant", "1DSCR,1MCF,1Se", "Shield Generator", 3, "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Kinetic Resistant", "1FoC,1ISSA,1Hg", "Shield Generator", 4, "Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Kinetic Resistant", "1RFC,1Ru,1USS", "Shield Generator", 5, "Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1P", "Shield Generator", 1, "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1CCo,1P", "Shield Generator", 2, "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1CCo,1MC,1P", "Shield Generator", 3, "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1CCe,1CCom,1Mn", "Shield Generator", 4, "Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1As,1CPo,1IC", "Shield Generator", 5, "Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Thermal Resistant", "1DSCR", "Shield Generator", 1, "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Thermal Resistant", "1DSCR,1Ge", "Shield Generator", 2, "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Thermal Resistant", "1DSCR,1Ge,1Se", "Shield Generator", 3, "Didi Vatermann,Elvira Martuuk,Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Thermal Resistant", "1FoC,1ISSA,1Hg", "Shield Generator", 4, "Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Thermal Resistant", "1RFC,1Ru,1USS", "Shield Generator", 5, "Lei Cheung"),
            new MaterialCommoditiesList.EngineeringRecipe("Clean Drive Tuning", "1S","Thrusters", 1, "Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new MaterialCommoditiesList.EngineeringRecipe("Clean Drive Tuning", "1CCo,1SLF","Thrusters", 2, "Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new MaterialCommoditiesList.EngineeringRecipe("Clean Drive Tuning", "1CCo,1SLF,1UED","Thrusters", 3, "Felicity Farseer,Professor Palin"),
            new MaterialCommoditiesList.EngineeringRecipe("Clean Drive Tuning", "1CCe,1DED,1MCF","Thrusters", 4, "Professor Palin"),
            new MaterialCommoditiesList.EngineeringRecipe("Clean Drive Tuning", "1ACED,1CCe,1Sn","Thrusters", 5, "Professor Palin"),
            new MaterialCommoditiesList.EngineeringRecipe("Dirty Drive Tuning", "1SLF","Thrusters", 1, "Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new MaterialCommoditiesList.EngineeringRecipe("Dirty Drive Tuning", "1ME,1SLF","Thrusters", 2, "Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new MaterialCommoditiesList.EngineeringRecipe("Dirty Drive Tuning", "1Cr,1MC,1SLF","Thrusters", 3, "Felicity Farseer,Professor Palin"),
            new MaterialCommoditiesList.EngineeringRecipe("Dirty Drive Tuning", "1CCom,1MCF,1Se","Thrusters", 4, "Professor Palin"),
            new MaterialCommoditiesList.EngineeringRecipe("Dirty Drive Tuning", "1Cd,1CIF,1PI","Thrusters", 5, "Professor Palin"),
            new MaterialCommoditiesList.EngineeringRecipe("Drive Strengthening", "1C","Thrusters", 1, "Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new MaterialCommoditiesList.EngineeringRecipe("Drive Strengthening", "1HCW,1V","Thrusters", 2, "Elvira Martuuk,Felicity Farseer,Professor Palin"),
            new MaterialCommoditiesList.EngineeringRecipe("Drive Strengthening", "1HCW,1SS,1V","Thrusters", 3, "Felicity Farseer,Professor Palin"),
            new MaterialCommoditiesList.EngineeringRecipe("Drive Strengthening", "1CoS,1HDP,1HDC","Thrusters", 4, "Professor Palin"),
            new MaterialCommoditiesList.EngineeringRecipe("Drive Strengthening", "1HE,1IS,1PCo","Thrusters", 5, "Professor Palin"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1P", "Torpedo Pylon", 1, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1Mn,1SAll", "Torpedo Pylon", 2, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Torpedo Pylon", 3, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Torpedo Pylon", 4, "Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Torpedo Pylon", 5, "Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni", "Torpedo Pylon", 1, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni,1SE", "Torpedo Pylon", 2, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Ni,1SE,1W", "Torpedo Pylon", 3, "Juri Ishmaak,Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1Mo,1W,1Zn", "Torpedo Pylon", 4, "Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Sturdy Mount", "1HDC,1Mo,1Tc", "Torpedo Pylon", 5, "Liz Ryder"),
            new MaterialCommoditiesList.EngineeringRecipe("Fast Scan", "1P", "Wake Scanner", 1, "Juri Ishmaak,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Fast Scan", "1FFC,1P", "Wake Scanner", 2, "Juri Ishmaak,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Fast Scan", "1FFC,1OSK,1P", "Wake Scanner", 3, "Juri Ishmaak,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Fast Scan", "1AEA,1FoC,1Mn", "Wake Scanner", 4, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Fast Scan", "1AEC,1As,1RFC", "Wake Scanner", 5, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1P", "Wake Scanner", 1, "Juri Ishmaak,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1Mn,1SAll", "Wake Scanner", 2, "Juri Ishmaak,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1Mn,1SAll", "Wake Scanner", 3, "Juri Ishmaak,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCo,1PA,1PLA", "Wake Scanner", 4, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Light Weight", "1CCe,1PLA,1PRA", "Wake Scanner", 5, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1Fe", "Wake Scanner", 1, "Juri Ishmaak,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1HC,1Fe", "Wake Scanner", 2, "Juri Ishmaak,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1HC,1Fe,1UED", "Wake Scanner", 3, "Juri Ishmaak,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1DED,1EA,1Ge", "Wake Scanner", 4, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Long Range", "1ACED,1Nb,1PCa", "Wake Scanner", 5, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni", "Wake Scanner", 1, "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni,1SE", "Wake Scanner", 2, "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Ni,1SE,1W", "Wake Scanner", 3, "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1Mo,1W,1Zn", "Wake Scanner", 4, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Reinforced", "1HDC,1Mo,1Tc", "Wake Scanner", 5, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1WSE", "Wake Scanner", 1, "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1SE", "Wake Scanner", 2, "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1C,1HDC", "Wake Scanner", 3, "Juri Ishmaak,Lori Jameson,Tiana Fortune,Bill Turner"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1PCo,1SS,1V", "Wake Scanner", 4, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Shielded", "1CoS,1CDC,1W", "Wake Scanner", 5, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Wide Angle", "1MS", "Wake Scanner", 1, "Juri Ishmaak,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Wide Angle", "1Ge,1MS", "Wake Scanner", 2, "Juri Ishmaak,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Wide Angle", "1CSD,1Ge,1MS", "Wake Scanner", 3, "Juri Ishmaak,Lori Jameson,Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Wide Angle", "1DSD,1ME,1Nb", "Wake Scanner", 4, "Tiana Fortune"),
            new MaterialCommoditiesList.EngineeringRecipe("Wide Angle", "1CSF,1MC,1Sn", "Wake Scanner", 5, "Tiana Fortune"),
        };

        //"Wake Scanner"
        #endregion

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
    }
}
