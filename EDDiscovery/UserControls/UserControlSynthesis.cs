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
using EliteDangerousCore.DB;
using EliteDangerousCore;

namespace EDDiscovery.UserControls
{
    public partial class UserControlSynthesis : UserControlCommonBase
    {
        private int displaynumber = 0;
        private EDDiscoveryForm discoveryform;
        private UserControlCursorType uctg;

        private string DbColumnSave { get { return ("SynthesisGrid") + ((displaynumber > 0) ? displaynumber.ToString() : "") + "DGVCol"; } }
        private string DbWSave { get { return "SynthesisWanted" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbOSave { get { return "SynthesisOrder" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbRecipeFilterSave { get { return "SynthesisRecipeFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbLevelFilterSave { get { return "SynthesisLevelFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbMaterialFilterSave { get { return "SynthesisMaterialFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        int[] Order;        // order
        int[] Wanted;       // wanted, in order terms
        internal bool isEmbedded = false;

        private List<Tuple<string, string>> matLookUp;
        RecipeFilterSelector rfs;
        RecipeFilterSelector lfs;
        RecipeFilterSelector mfs;

        public Action<List<Tuple<MaterialCommoditiesList.Recipe, int>>> OnDisplayComplete;  // called when display complete, for use by other UCs using this

        public HistoryEntry CurrentHistoryEntry { get {return last_he;} }     //one in use, may be null

        #region Init

        public UserControlSynthesis()
        {
            InitializeComponent();
        }

        public override void Init(EDDiscoveryForm ed, UserControlCursorType thc, int vn) //0=primary, 1 = first windowed version, etc
        {
            discoveryform = ed;
            uctg = thc;
            displaynumber = vn;

            dataGridViewSynthesis.MakeDoubleBuffered();
            dataGridViewSynthesis.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dataGridViewSynthesis.RowTemplate.Height = 26;

            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            uctg.OnTravelSelectionChanged += Display;

            Order = SQLiteDBClass.GetSettingString(DbOSave, "").RestoreArrayFromString(0, Recipes.Count);
            if (Order.Distinct().Count() != Order.Length)       // if not distinct..
                for (int i = 0; i < Order.Length; i++)          // reset
                    Order[i] = i;

            Wanted = SQLiteDBClass.GetSettingString(DbWSave, "").RestoreArrayFromString(0, Recipes.Count);

            var rcpes = Recipes.Select(r => r.name).Distinct().ToList();
            rcpes.Sort();
            rfs = new RecipeFilterSelector(rcpes);
            rfs.Changed += FilterChanged;

            var lvls = Recipes.Select(r => r.level).Distinct().ToList();
            lvls.Sort();
            lfs = new RecipeFilterSelector(lvls);
            lfs.Changed += FilterChanged;

            List<string> matShortNames = Recipes.SelectMany(r => r.ingredients).Distinct().ToList();
            matLookUp = matShortNames.Select(sn => Tuple.Create<string, string>(sn, MaterialCommodityDB.GetCachedMaterialByShortName(sn).name)).ToList();
            List<string> matLongNames = matLookUp.Select(lu => lu.Item2).ToList();
            matLongNames.Sort();
            mfs = new RecipeFilterSelector(matLongNames);
            mfs.Changed += FilterChanged;

            for (int i = 0; i < Recipes.Count; i++)         // pre-fill array.. preventing the crash on cell edit when you
            {
                int rno = Order[i];
                MaterialCommoditiesList.SynthesisRecipe r = Recipes[rno];

                int rown = dataGridViewSynthesis.Rows.Add();

                using (DataGridViewRow row = dataGridViewSynthesis.Rows[rown])
                {
                    row.Cells[0].Value = r.name; // debug rno + ":" + r.name;
                    row.Cells[1].Value = r.level;
                    row.Cells[6].Value = r.ingredientsstring;
                    row.Tag = rno;
                    row.Visible = false;
                }
            }
        }

        public override void ChangeCursorType(UserControlCursorType thc)
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
                int fdrow = dataGridViewSynthesis.FirstDisplayedScrollingRowIndex;      // remember where we were displaying

                MaterialCommoditiesList.ResetUsed(mcl);

                wantedList = new List<Tuple<MaterialCommoditiesList.Recipe, int>>();

                string recep = SQLiteDBClass.GetSettingString(DbRecipeFilterSave, "All");
                string[] recipeArray = recep.Split(';');
                string levels = SQLiteDBClass.GetSettingString(DbLevelFilterSave, "All");
                string[] lvlArray = (levels == "All" || levels == "None") ? new string[0] : levels.Split(';');
                string materials = SQLiteDBClass.GetSettingString(DbMaterialFilterSave, "All");
                List<string> matList;
                if (materials == "All" || materials == "None") { matList = new List<string>(); }
                else { matList = materials.Split(';').Where(x => !string.IsNullOrEmpty(x)).Select(m => matLookUp.Where(u => u.Item2 == m).First().Item1).ToList(); }

                for (int i = 0; i < Recipes.Count; i++)
                {
                    int rno = (int)dataGridViewSynthesis.Rows[i].Tag;
                    dataGridViewSynthesis.Rows[i].Cells[2].Value = MaterialCommoditiesList.HowManyLeft(mcl, Recipes[rno]).Item1.ToStringInvariant();
                    bool visible = true;
                
                    if (recep == "All" && levels == "All" && materials == "All")
                    {
                        visible = true;
                    }
                    else
                    {
                        visible = false;
                        if (recep == "All") { visible = true; }
                        else
                        {
                            visible = recipeArray.Contains(Recipes[rno].name);
                        }
                        if (levels == "All") { visible = visible && true; }
                        else
                        {
                            visible = visible && lvlArray.Contains(Recipes[rno].level);
                        }
                        if (materials == "All") { visible = visible && true; }
                        else
                        {
                            var included = matList.Intersect<string>(Recipes[rno].ingredients.ToList<string>());
                            visible = visible && included.Count() > 0;
                        }
                    }

                    dataGridViewSynthesis.Rows[i].Visible = visible;
                }

                for (int i = 0; i < Recipes.Count; i++)
                {
                    int rno = (int)dataGridViewSynthesis.Rows[i].Tag;
                    if (dataGridViewSynthesis.Rows[i].Visible)
                    {
                        Tuple<int, int, string> res = MaterialCommoditiesList.HowManyLeft(mcl, Recipes[rno], Wanted[rno]);
                        //System.Diagnostics.Debug.WriteLine("{0} Recipe {1} executed {2} {3} ", i, rno, Wanted[rno], res.Item2);

                        using (DataGridViewRow row = dataGridViewSynthesis.Rows[i])
                        {
                            row.Cells[3].Value = Wanted[rno].ToStringInvariant();
                            row.Cells[4].Value = res.Item2.ToStringInvariant();
                            row.Cells[5].Value = res.Item3;
                        }
                    }
                    if (Wanted[rno] > 0 && (dataGridViewSynthesis.Rows[i].Visible || isEmbedded))
                    {
                        wantedList.Add(new Tuple<MaterialCommoditiesList.Recipe, int>(Recipes[rno], Wanted[rno]));
                    }
                }

                dataGridViewSynthesis.RowCount = Recipes.Count;         // truncate previous shopping list..

                if (!isEmbedded)
                {
                    MaterialCommoditiesList.ResetUsed(mcl);
                    List<MaterialCommodities> shoppinglist = MaterialCommoditiesList.GetShoppingList(wantedList, mcl);
                    shoppinglist.Sort(delegate (MaterialCommodities left, MaterialCommodities right) { return left.name.CompareTo(right.name); });

                    foreach (MaterialCommodities c in shoppinglist)        // and add new..
                    {
                        Object[] values = { c.name, "", c.scratchpad.ToStringInvariant(), "", c.shortname };
                        int rn = dataGridViewSynthesis.Rows.Add(values);
                        dataGridViewSynthesis.Rows[rn].ReadOnly = true;     // disable editing wanted..
                    }
                }

                if (fdrow >= 0 && dataGridViewSynthesis.Rows[fdrow].Visible)        // better check visible, may have changed..
                    dataGridViewSynthesis.FirstDisplayedScrollingRowIndex = fdrow;

            }

            if (OnDisplayComplete != null)
                OnDisplayComplete(wantedList);
        }

        #endregion

        #region Layout

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridViewSynthesis, DbColumnSave);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewSynthesis, DbColumnSave);

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
                dataGridViewSynthesis.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Wanted[rno].ToStringInvariant();
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

            if (rowFrom >= 0 && rowFrom < Recipes.Count)        // only can drag recipes area..
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
            if (e.Effect == DragDropEffects.Move && droprow>=0 && droprow < Recipes.Count )
            {
                DataGridViewRow rowTo = e.Data.GetData( typeof(DataGridViewRow)) as DataGridViewRow;
                dataGridViewSynthesis.Rows.RemoveAt(rowFrom);
                dataGridViewSynthesis.Rows.Insert(droprow, rowTo);

                for (int i = 0; i < Recipes.Count; i++)
                    Order[i] = (int)dataGridViewSynthesis.Rows[i].Tag;          // reset the order array

                //for (int i = 0; i < 10; i++)   System.Diagnostics.Debug.WriteLine(i.ToString() + "=" + Order[i]);

                Display();
            }
        }

        List<MaterialCommoditiesList.SynthesisRecipe> Recipes = new List<MaterialCommoditiesList.SynthesisRecipe>()
        {
            new MaterialCommoditiesList.SynthesisRecipe( "FSD", "Premium","3Nb,1As,1Po,1Y" ),
            new MaterialCommoditiesList.SynthesisRecipe( "FSD", "Standard","1V,1Ge,2Cd,1Nb" ),
            new MaterialCommoditiesList.SynthesisRecipe( "FSD", "Basic","2V,1Ge" ),

            new MaterialCommoditiesList.SynthesisRecipe( "AFM Refill", "Premium","6V,4Cr,2Zn,2Zr,1Te,1Ru" ),
            new MaterialCommoditiesList.SynthesisRecipe( "AFM Refill", "Standard","6V,2Mn,1Mo,1Zr,1Sn" ),
            new MaterialCommoditiesList.SynthesisRecipe( "AFM Refill", "Basic","3V,2Ni,2Cr,2Zn" ),

            new MaterialCommoditiesList.SynthesisRecipe( "SRV Ammo", "Premium","2P,2Se,1Mo,1Tc" ),
            new MaterialCommoditiesList.SynthesisRecipe( "SRV Ammo", "Standard","1P,1Se,1Mn,1Mo" ),
            new MaterialCommoditiesList.SynthesisRecipe( "SRV Ammo", "Basic","1P,2S" ),

            new MaterialCommoditiesList.SynthesisRecipe( "SRV Repair", "Premium","2V,1Zn,2Cr,1W,1Te" ),
            new MaterialCommoditiesList.SynthesisRecipe( "SRV Repair", "Standard","3Ni,2V,1Mn,1Mo" ),
            new MaterialCommoditiesList.SynthesisRecipe( "SRV Repair", "Basic","2Fe,1Ni" ),

            new MaterialCommoditiesList.SynthesisRecipe( "SRV Refuel", "Premium","1S,1As,1Hg,1Tc" ),
            new MaterialCommoditiesList.SynthesisRecipe( "SRV Refuel", "Standard","1P,1S,1As,1Hg" ),
            new MaterialCommoditiesList.SynthesisRecipe( "SRV Refuel", "Basic","1P,1S" ),

            new MaterialCommoditiesList.SynthesisRecipe( "Plasma Munitions", "Premium", "5Se,4Mo,4Cd,2Tc" ),
            new MaterialCommoditiesList.SynthesisRecipe( "Plasma Munitions", "Standard","5P,1Se,3Mn,4Mo" ),
            new MaterialCommoditiesList.SynthesisRecipe( "Plasma Munitions", "Basic","4P,3S,1Mn" ),

            new MaterialCommoditiesList.SynthesisRecipe( "Explosive Munitions", "Premium","5P,4As,5Hg,5Nb,5Po" ),
            new MaterialCommoditiesList.SynthesisRecipe( "Explosive Munitions", "Standard","6P,6S,4As,2Hg" ),
            new MaterialCommoditiesList.SynthesisRecipe( "Explosive Munitions", "Basic","4S,3Fe,3Ni,4C" ),

            new MaterialCommoditiesList.SynthesisRecipe( "Small Calibre Munitions", "Premium","2P,2S,2Zr,2Hg,2W,1Sb" ),
            new MaterialCommoditiesList.SynthesisRecipe( "Small Calibre Munitions", "Standard","2P,2Fe,2Zr,2Zn,2Se" ),
            new MaterialCommoditiesList.SynthesisRecipe( "Small Calibre Munitions", "Basic","2S,2Fe,1Ni" ),

            new MaterialCommoditiesList.SynthesisRecipe( "High Velocity Munitions", "Premium","4V,2Zr,4W,2Y" ),
            new MaterialCommoditiesList.SynthesisRecipe( "High Velocity Munitions", "Standard","4Fe,3V,2Zr,2W" ),
            new MaterialCommoditiesList.SynthesisRecipe( "High Velocity Munitions", "Basic","2Fe,1V" ),

            new MaterialCommoditiesList.SynthesisRecipe( "Large Calibre Munitions", "Premium","8Zn,1As,1Hg,2W,2Sb" ),
            new MaterialCommoditiesList.SynthesisRecipe( "Large Calibre Munitions", "Standard","3P,2Zr,3Zn,1As,2Sn" ),
            new MaterialCommoditiesList.SynthesisRecipe( "Large Calibre Munitions", "Basic","2S,4Ni,3C" ),

            new MaterialCommoditiesList.SynthesisRecipe( "Limpets", "Basic", "10Fe,10Ni"),

            new MaterialCommoditiesList.SynthesisRecipe( "Chaff", "Premium", "1CC,2FiC,1ThA,1PRA"),
            new MaterialCommoditiesList.SynthesisRecipe( "Chaff", "Standard", "1CC,2FiC,1ThA"),
            new MaterialCommoditiesList.SynthesisRecipe( "Chaff", "Basic", "1CC,1FiC"),

            new MaterialCommoditiesList.SynthesisRecipe( "Heat Sinks", "Premium", "2BaC,2HCW,2HE,1PHR"),
            new MaterialCommoditiesList.SynthesisRecipe( "Heat Sinks", "Standard", "2BaC,2HCW,2HE"),
            new MaterialCommoditiesList.SynthesisRecipe( "Heat Sinks", "Basic", "1BaC,1HCW"),

            new MaterialCommoditiesList.SynthesisRecipe( "Life Support", "Basic", "2Fe,1Ni")
        };

        private void buttonClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Recipes.Count; i++)
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
    }
}
