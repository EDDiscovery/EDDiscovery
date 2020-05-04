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
using System.IO;
using EliteDangerousCore;
using EliteDangerousCore.DB;

namespace EDDiscovery.UserControls
{
    public partial class UserControlShipYards : UserControlCommonBase
    {
        private string DbColumnSave { get { return DBName("ShipYardGrid" ,  "DGVCol"); } }
        private string DbYardSave { get { return DBName("ShipYardGridSelect" ); } }

        #region Init

        public UserControlShipYards()
        {
            InitializeComponent();
            var corner = dataGridViewShips.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            dataGridViewShips.MakeDoubleBuffered();
            dataGridViewShips.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange; ;
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
            dataGridViewShips.RowTemplate.MinimumHeight = Font.ScalePixels(26);
            uctg.OnTravelSelectionChanged += Display;
            DGVLoadColumnLayout(dataGridViewShips, DbColumnSave);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewShips, DbColumnSave);
            uctg.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
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
            ShipYardList shm = hl.shipyards;
            string cursel = comboBoxYards.Text;

            string the = "Travel History Entry".T(EDTx.UserControlShipYards_TravelHistoryEntry);

            comboBoxYards.Items.Clear();
            comboBoxYards.Items.Add(the);

            comboBoxYards.Items.AddRange(shm.ShipList());

            var list = (from ShipYard x in shm.GetFilteredList() select x.Ident()).ToList();        // yard list repeats allowed within timescale
            comboBoxYards.Items.AddRange(list);

            if (cursel == "")
                cursel = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbYardSave, "");

            if (cursel == "" || !comboBoxYards.Items.Contains(cursel))
                cursel = the;

            comboBoxYards.Enabled = false;
            comboBoxYards.SelectedItem = cursel;
            comboBoxYards.Enabled = true;
        }

        public override void InitialDisplay()
        {
            Display(uctg.GetCurrentHistoryEntry, discoveryform.history);
        }

        HistoryEntry last_he = null;

        private void Display(HistoryEntry he, HistoryList hl) =>
            Display(he, hl, true);

        private void Display(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            if ( comboBoxYards.Items.Count == 0 )
                UpdateComboBox(hl);

            last_he = he;
            Display();
        }

        private void Display()
        {
            DataGridViewColumn sortcol = dataGridViewShips.SortedColumn != null ? dataGridViewShips.SortedColumn : dataGridViewShips.Columns[0];
            SortOrder sortorder = dataGridViewShips.SortOrder;

            dataGridViewShips.Rows.Clear();
            dataViewScrollerPanel1.SuspendLayout();

            labelYard.Visible = false;

            ShipYard yard = null;

            if (comboBoxYards.SelectedIndex == 0 || comboBoxYards.Text.Length == 0)  // second is due to the order History gets called vs this on start
            {
                HistoryEntry lastshipyard = discoveryform.history.GetLastHistoryEntry(x => x.EntryType == JournalTypeEnum.Shipyard, last_he);
                if (lastshipyard != null)
                    yard = (lastshipyard.journalEntry as EliteDangerousCore.JournalEvents.JournalShipyard).Yard;
            }
            else
            {
                yard = discoveryform.history.shipyards.GetFilteredList().Find(x => x.Ident().Equals(comboBoxYards.Text));
            }

            if (yard != null && yard.Ships != null)
            {
                DisplayYard(yard);
            }
            else
            {
                List<Tuple<ShipYard, ShipYard.ShipyardItem>> shiplist = discoveryform.history.shipyards.GetShipLocations(comboBoxYards.Text,nolocrepeats:true);
                if ( shiplist.Count > 0 )
                    DisplayShips(shiplist, comboBoxYards.Text);
            }

            dataViewScrollerPanel1.ResumeLayout();
            dataGridViewShips.Sort(sortcol, (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
            dataGridViewShips.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;
        }

        private void DisplayShips(List<Tuple<ShipYard, ShipYard.ShipyardItem>> shiplist,string ship)
        {
            ISystem cursys = discoveryform.history.CurrentSystem;

            foreach (Tuple<ShipYard, ShipYard.ShipyardItem> i in shiplist)
            {
                double distance = discoveryform.history.DistanceCurrentTo(i.Item1.StarSystem);
                string dte = EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(i.Item1.Datetime).ToString();
                object[] rowobj = { dte, i.Item1.Location, (distance > -1) ? (distance.ToString("N1") + "ly") : "Unknown".T(EDTx.Unknown), i.Item2.ShipPrice.ToString("N1") + "cr" };
                dataGridViewShips.Rows.Add(rowobj);
            }

            labelYard.Text = ship;
            labelYard.Visible = true;
            Col1.HeaderText = "Date".T(EDTx.UserControlShipYards_Date);
            Col1.Tag = "DT";
            Col2.HeaderText = "Yard".T(EDTx.UserControlShipYards_Yard);
            Col2.Tag = null;
            Col3.HeaderText = "Distance".T(EDTx.UserControlShipYards_Distance);
            Col3.Tag = "ly";
            ColPrice.HeaderText = "Price".T(EDTx.UserControlShipYards_Price);
            ColPrice.Tag = "cr";
        }

        private void DisplayYard(ShipYard yard)    
        {
            foreach (ShipYard.ShipyardItem i in yard.Ships)
            {
                string col2 = "Unknown".T(EDTx.Unknown), col3 = "";
                Dictionary<ShipModuleData.ShipPropID, ShipModuleData.ShipInfo> shipprops = ShipModuleData.Instance.GetShipProperties(i.FDShipType);
                if ( shipprops != null)
                {
                    col2 = (shipprops[ShipModuleData.ShipPropID.Manu] as ShipModuleData.ShipInfoString).Value;
                    col3 = (shipprops[ShipModuleData.ShipPropID.HullMass] as ShipModuleData.ShipInfoDouble).Value.ToString("0.#t") +
                        ", " + (shipprops[ShipModuleData.ShipPropID.Speed] as ShipModuleData.ShipInfoInt).Value.ToString() +
                        "/" + (shipprops[ShipModuleData.ShipPropID.Boost] as ShipModuleData.ShipInfoInt).Value.ToString();
                }

                object[] rowobj = { i.ShipType_Localised, col2, col3,i.ShipPrice.ToString("N1") + "cr" };

                dataGridViewShips.Rows.Add(rowobj);
            }

            double distance = discoveryform.history.DistanceCurrentTo(yard.StarSystem);

            labelYard.Text = yard.Ident() + (distance>-1 ? (" @ " + distance.ToString("N1") + "ly") : "");
            labelYard.Visible = true;
            Col1.HeaderText = "Ship".T(EDTx.UserControlShipYards_Ship);
            Col1.Tag = null;
            Col2.HeaderText = "Manufacturer".T(EDTx.UserControlShipYards_Manufacturer);
            Col2.Tag = null;
            Col3.HeaderText = "Mass/Speed".T(EDTx.UserControlShipYards_MS);
            Col3.Tag = null;
            ColPrice.HeaderText = "Price".T(EDTx.UserControlShipYards_Price);
            ColPrice.Tag = "cr";
        }

        #endregion


        private void comboBoxHistoryWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxYards.Enabled)
            {
                EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(DbYardSave, comboBoxYards.Text);
                Display();
            }
        }

        private void dataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Tag != null)
            {
                string t = (string)e.Column.Tag;

                if (t == "DT")
                    e.SortDataGridViewColumnDate();
                else
                    e.SortDataGridViewColumnNumeric(t);
            }
        }

    }
}
