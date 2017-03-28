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

            dataGridViewModules.MakeDoubleBuffered();
            dataGridViewModules.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dataGridViewModules.RowTemplate.Height = 26;

            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange; ;
            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            ed.TravelControl.OnTravelSelectionChanged += Display;

            Order = Restore(DB.SQLiteDBClass.GetSettingString(DbOSave, ""), 0, Recipes.Count);
            if (Order.Distinct().Count() != Order.Length)       // if not distinct..
                for (int i = 0; i < Order.Length; i++)          // reset
                    Order[i] = i;

            Wanted = Restore(DB.SQLiteDBClass.GetSettingString(DbWSave, ""), 0, Recipes.Count);
        }

        #endregion

        #region Display

        private void Discoveryform_OnNewEntry(HistoryEntry he, HistoryList hl)
        {
            Discoveryform_OnHistoryChange(hl);
        }

        private void Discoveryform_OnHistoryChange(HistoryList hl)
        {
            UpdateComboBox(hl);
        }

        private void UpdateComboBox(HistoryList hl)
        { 
            ShipInformationList shm = hl.shipinformationlist;
            string cursel = comboBoxSynthesis.Text;

            comboBoxSynthesis.Items.Clear();
            comboBoxSynthesis.Items.Add("All");
            comboBoxSynthesis.Items.Add("Travel");

            if (cursel == "" || !comboBoxSynthesis.Items.Contains(cursel))
                cursel = "All";

            comboBoxSynthesis.Enabled = false;
            comboBoxSynthesis.SelectedItem = cursel;
            comboBoxSynthesis.Enabled = true;
        }

        HistoryEntry last_he = null;
        public override void Display(HistoryEntry he, HistoryList hl)
        {
            if ( comboBoxSynthesis.Items.Count == 0 )
                UpdateComboBox(hl);

            last_he = he;
            Display();
        }

        public void Display()
        {
            dataGridViewModules.Rows.Clear();

            if (last_he != null)
            {
                List<MaterialCommodities> mcl = last_he.MaterialCommodity.Sort(false);
                MaterialCommoditiesList.ResetUsed(mcl);

                int[] max = new int[Recipes.Count];

                for (int i = 0; i < Recipes.Count; i++)
                    max[i] = MaterialCommoditiesList.HowManyLeft(mcl, Recipes[i]).Item1;

                for (int i = 0; i < Recipes.Count; i++)
                {
                    MaterialCommoditiesList.Recipe r = Recipes[Order[i]];

                    Tuple<int, int, string> res = MaterialCommoditiesList.HowManyLeft(mcl, r, Wanted[i]);

                    object[] rowobj = { r.name, max[i].ToStringInvariant(), Wanted[i].ToStringInvariant(), res.Item2.ToStringInvariant(), res.Item3, r.ingredientsstring };
                    dataGridViewModules.Rows.Add(rowobj);
                }
            }

        }


        #endregion

        #region Layout

        public override void LoadLayout()
        {
        //    DGVLoadColumnLayout(dataGridViewModules, DbColumnSave);
        }

        public override void Closing()
        {
          //  DGVSaveColumnLayout(dataGridViewModules, DbColumnSave);
            discoveryform.TravelControl.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;

            DB.SQLiteDBClass.PutSettingString(DbOSave, Order.ToString(","));
            DB.SQLiteDBClass.PutSettingString(DbWSave, Wanted.ToString(","));
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
            string v = (string)dataGridViewModules.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            int iv = 0;

            if (v.InvariantParse(out iv))
            {
                if (e.ColumnIndex == 2)
                {
                    Wanted[e.RowIndex] = iv;
                    Display();
                }
            }
        }
    }
}
