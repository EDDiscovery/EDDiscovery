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
using EDDiscovery2.DB;
using EDDiscovery.EliteDangerous;

namespace EDDiscovery.UserControls
{
    public partial class UserControlSynthesis : UserControlCommonBase
    {
        private int displaynumber = 0;
        private EDDiscoveryForm discoveryform;
        
        private string DbColumnSave { get { return ("SynthesisGrid") + ((displaynumber > 0) ? displaynumber.ToString() : "") + "DGVCol"; } }
        private string DbWSave { get { return "SynthesisWanted" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbOSave { get { return "SynthesisOrder" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbSelSave { get { return "SynthesisList" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        int[] Order;        // order
        int[] Wanted;       // wanted, in order terms

        #region Init

        public UserControlSynthesis()
        {
            InitializeComponent();
            Name = "Synthesis";
        }

        public override void Init(EDDiscoveryForm ed, int vn) //0=primary, 1 = first windowed version, etc
        {
            discoveryform = ed;
            displaynumber = vn;

            dataGridViewSynthesis.MakeDoubleBuffered();
            dataGridViewSynthesis.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dataGridViewSynthesis.RowTemplate.Height = 26;

            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            ed.TravelControl.OnTravelSelectionChanged += Display;

            Order = Restore(DB.SQLiteDBClass.GetSettingString(DbOSave, ""), 0, Recipes.Count);
            if (Order.Distinct().Count() != Order.Length)       // if not distinct..
                for (int i = 0; i < Order.Length; i++)          // reset
                    Order[i] = i;

            Wanted = Restore(DB.SQLiteDBClass.GetSettingString(DbWSave, ""), 0, Recipes.Count);

            comboBoxSynthesis.Items.Add("All");
            comboBoxSynthesis.Items.Add("Travel");
            comboBoxSynthesis.Items.Add("Ammo");
            comboBoxSynthesis.Items.Add("SRV");

            comboBoxSynthesis.Enabled = false;
            string s = DB.SQLiteDBClass.GetSettingString(DbSelSave, "All");
            comboBoxSynthesis.SelectedItem = comboBoxSynthesis.Items.Contains(s) ? s : "All";
            comboBoxSynthesis.Enabled = true;

            for (int i = 0; i < Recipes.Count; i++)         // pre-fill array
            {
                int rno = Order[i];
                MaterialCommoditiesList.Recipe r = Recipes[rno];

                int rown = dataGridViewSynthesis.Rows.Add();

                using (DataGridViewRow row = dataGridViewSynthesis.Rows[rown])
                {
                    row.Cells[0].Value = r.name; // debug rno + ":" + r.name;
                    row.Cells[5].Value = r.ingredientsstring;
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

                MaterialCommoditiesList.ResetUsed(mcl);

                string sel = (string)comboBoxSynthesis.SelectedItem;

                for (int i = 0; i < dataGridViewSynthesis.Rows.Count; i++)
                {
                    int rno = (int)dataGridViewSynthesis.Rows[i].Tag;
                    dataGridViewSynthesis.Rows[i].Cells[1].Value = MaterialCommoditiesList.HowManyLeft(mcl, Recipes[rno]).Item1.ToStringInvariant();
                    bool visible = true;
                    if (sel == "Travel")
                        visible = (rno < FirstAmmoRow);
                    if (sel == "Ammo")
                        visible = (rno >= FirstAmmoRow);
                    if (sel == "SRV")
                        visible = (rno>= FirstSRVRow && rno< FirstAmmoRow);
                    dataGridViewSynthesis.Rows[i].Visible = visible;
                }

                for (int i = 0; i < dataGridViewSynthesis.Rows.Count; i++)
                {
                    if (dataGridViewSynthesis.Rows[i].Visible)
                    {
                        int rno = (int)dataGridViewSynthesis.Rows[i].Tag;
                        Tuple<int, int, string> res = MaterialCommoditiesList.HowManyLeft(mcl, Recipes[rno], Wanted[rno]);
                        //System.Diagnostics.Debug.WriteLine("{0} Recipe {1} executed {2} {3} ", i, rno, Wanted[rno], res.Item2);

                        using (DataGridViewRow row = dataGridViewSynthesis.Rows[i])
                        {
                            row.Cells[2].Value = Wanted[rno].ToStringInvariant();
                            row.Cells[3].Value = res.Item2.ToStringInvariant();
                            row.Cells[4].Value = res.Item3;
                        }
                    }
                }
            }
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

            discoveryform.TravelControl.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;

            DB.SQLiteDBClass.PutSettingString(DbOSave, Order.ToString(","));
            DB.SQLiteDBClass.PutSettingString(DbWSave, Wanted.ToString(","));
            DB.SQLiteDBClass.PutSettingString(DbSelSave, (string)comboBoxSynthesis.SelectedItem);
        }

        #endregion

        private void comboBoxHistoryWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSynthesis.Enabled)
            {
                Display();
            }
        }

        #region receipies


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
        #endregion

        private void dataGridViewModules_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string v = (string)dataGridViewSynthesis.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            int iv = 0;
            int rno = (int)dataGridViewSynthesis.Rows[e.RowIndex].Tag;

            if (v.InvariantParse(out iv))
            {
                if (e.ColumnIndex == 2)
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
                if (moveMoveDragBox != Rectangle.Empty &&!moveMoveDragBox.Contains(e.X, e.Y))
                {
                    //System.Diagnostics.Debug.WriteLine("move: Drag Start");
                    dataGridViewSynthesis.DoDragDrop( dataGridViewSynthesis.Rows[rowFrom], DragDropEffects.Move);
                }
            }
        }

        private void dataGridViewSynthesis_MouseDown(object sender, MouseEventArgs e)
        {
            rowFrom = dataGridViewSynthesis.HitTest(e.X, e.Y).RowIndex;

            if (rowFrom >= 0)
            {
                Size dragSize = SystemInformation.DragSize;
                //System.Diagnostics.Debug.WriteLine("move: Mouse down");
                moveMoveDragBox = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                               e.Y - (dragSize.Height / 2)),
                                                        dragSize);
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

            // If the drag operation was a move then remove and insert the row.
            if (e.Effect == DragDropEffects.Move && droprow>=0 )
            {
                DataGridViewRow rowTo = e.Data.GetData( typeof(DataGridViewRow)) as DataGridViewRow;
                dataGridViewSynthesis.Rows.RemoveAt(rowFrom);
                dataGridViewSynthesis.Rows.Insert(droprow, rowTo);

                System.Diagnostics.Debug.Assert(dataGridViewSynthesis.Rows.Count == Order.Length);

                for (int i = 0; i < dataGridViewSynthesis.Rows.Count; i++)
                    Order[i] = (int)dataGridViewSynthesis.Rows[i].Tag;          // reset the order array

                //for (int i = 0; i < 10; i++)   System.Diagnostics.Debug.WriteLine(i.ToString() + "=" + Order[i]);

                Display();
            }
        }

        const int FirstSRVRow = 6;          // SYNC to table
        const int FirstAmmoRow = 15;
        List<MaterialCommoditiesList.Recipe> Recipes = new List<MaterialCommoditiesList.Recipe>()
        {
            new MaterialCommoditiesList.Recipe( "FSD Premium","3Nb,1As,1Po,1Y" ),
            new MaterialCommoditiesList.Recipe( "FSD Standard","1V,1Ge,2Cd,1Nb" ),
            new MaterialCommoditiesList.Recipe( "FSD Basic","2V,1Ge" ),

            new MaterialCommoditiesList.Recipe( "AFM Refill Premium","6V,4Cr,2Zn,2Zr,1Te,1Ru" ),
            new MaterialCommoditiesList.Recipe( "AFM Refill Standard","6V,2Mn,1Mo,1Zr,1Sn" ),
            new MaterialCommoditiesList.Recipe( "AFM Refill Basic","3V,2Ni,2Cr,2Zn" ),

            new MaterialCommoditiesList.Recipe( "SRV Ammo Premium","2P,2Se,1Mo,1Tc" ),
            new MaterialCommoditiesList.Recipe( "SRV Ammo Standard","1P,1Se,1Mn,1Mo" ),
            new MaterialCommoditiesList.Recipe( "SRV Ammo Basic","1P,2S" ),

            new MaterialCommoditiesList.Recipe( "SRV Repair Premium","2V,1Z,2Cr,1W,1Te" ),
            new MaterialCommoditiesList.Recipe( "SRV Repair Standard","3Ni,2V,1Mn,1Mo" ),
            new MaterialCommoditiesList.Recipe( "SRV Repair Basic","2Fe,1Ni" ),

            new MaterialCommoditiesList.Recipe( "SRV Refuel Premium","1S,1As,1Hg,1Tc" ),
            new MaterialCommoditiesList.Recipe( "SRV Refuel Standard","1P,1S,1As,1Hg" ),
            new MaterialCommoditiesList.Recipe( "SRV Refuel Basic","1P,1S" ),

            new MaterialCommoditiesList.Recipe( "Plasma Munitions Premium", "5Se,4Mo,4Cd,2Tc" ),
            new MaterialCommoditiesList.Recipe( "Plasma Munitions Standard","5P,1Se,3Mn,4Mo" ),
            new MaterialCommoditiesList.Recipe( "Plasma Munitions Basic","4P,3S,1Mn" ),

            new MaterialCommoditiesList.Recipe( "Explosive Munitions Premium","5P,4As,5Hg,5Nb,5Po" ),
            new MaterialCommoditiesList.Recipe( "Explosive Munitions Standard","6P,6S,4As,2Hg" ),
            new MaterialCommoditiesList.Recipe( "Explosive Munitions Basic","4S,3Fe,3Ni,4C" ),

            new MaterialCommoditiesList.Recipe( "Small Calibre Munitions Premium","2P,2S,2Zr,2Hg,2W,1Sb" ),
            new MaterialCommoditiesList.Recipe( "Small Calibre Munitions Standard","2P,2Fe,2Zr,2Zn,2Se" ),
            new MaterialCommoditiesList.Recipe( "Small Calibre Munitions Basic","2S,2Fe,1Ni" ),

            new MaterialCommoditiesList.Recipe( "High Velocity Munitions Premium","4V,2Zr,4W,2Y" ),
            new MaterialCommoditiesList.Recipe( "High Velocity Munitions Standard","4Fe,3V,2Zr,2W" ),
            new MaterialCommoditiesList.Recipe( "High Velocity Munitions Basic","2Fe,1V" ),

            new MaterialCommoditiesList.Recipe( "Large Calibre Munitions Premium","8Zn,1As,1Hg,2W,2Sb" ),
            new MaterialCommoditiesList.Recipe( "Large Calibre Munitions Standard","3P,2Zr,3Zn,1As,2Sn" ),
            new MaterialCommoditiesList.Recipe( "Large Calibre Munitions Basic","2S,4Ni,3C" ),

        };

    }
}
