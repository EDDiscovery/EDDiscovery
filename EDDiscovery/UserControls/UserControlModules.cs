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
    public partial class UserControlModules : UserControlCommonBase
    {
        private int displaynumber = 0;
        private EDDiscoveryForm discoveryform;
        
        private string DbColumnSave { get { return ("ModulesGrid") + ((displaynumber > 0) ? displaynumber.ToString() : "") + "DGVCol"; } }
        private string DbShipSave { get { return "ModulesGridShipSelect" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        #region Init

        public UserControlModules()
        {
            InitializeComponent();
            Name = "Loadout";
        }

        public override void Init( EDDiscoveryForm ed, int vn) //0=primary, 1 = first windowed version, etc
        {
            discoveryform = ed;
            displaynumber = vn;

            dataGridViewModules.MakeDoubleBuffered();
            dataGridViewModules.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dataGridViewModules.RowTemplate.Height = 26;

            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange; ;
            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            ed.TravelControl.OnTravelSelectionChanged += Display;
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
            string cursel = comboBoxShips.Text;

            comboBoxShips.Items.Clear();
            comboBoxShips.Items.Add("Travel History Entry");
            comboBoxShips.Items.Add("Stored Modules");
            foreach ( int id in shm.Ships.Keys)
            {
                ShipInformation sm = shm.Ships[id];
                comboBoxShips.Items.Add(sm.ShipShortName);
            }

            if (cursel == "")
                cursel = DB.SQLiteDBClass.GetSettingString(DbShipSave, "");

            if (cursel == "" || !comboBoxShips.Items.Contains(cursel))
                cursel = "Travel History Entry";

            comboBoxShips.Enabled = false;
            comboBoxShips.SelectedItem = cursel;
            comboBoxShips.Enabled = true;
        }

        HistoryEntry last_he = null;
        public override void Display(HistoryEntry he, HistoryList hl)
        {
            if ( comboBoxShips.Items.Count == 0 )
                UpdateComboBox(hl);

            last_he = he;
            Display();
        }

        public void Display()
        {
            DataGridViewColumn sortcol = dataGridViewModules.SortedColumn != null ? dataGridViewModules.SortedColumn : dataGridViewModules.Columns[0];
            SortOrder sortorder = dataGridViewModules.SortOrder;

            dataGridViewModules.Rows.Clear();

            LabelVehicleText.Visible = false;
            labelVehicle.Visible = false;

            if (comboBoxShips.Text.Contains("Stored"))
            {
                if (last_he != null && last_he.StoredModules != null)
                {
                    ModulesInStore mi = last_he.StoredModules;
                    labelVehicle.Text = "";
                    int i = 1;
                    foreach(EliteDangerous.JournalEvents.JournalLoadout.ShipModule sm in mi.StoredModules )
                    {
                        object[] rowobj = { i.ToString(), sm.Item, sm.LocalisedItem.ToNullSafeString() };
                        dataGridViewModules.Rows.Add(rowobj);
                        i++;
                    }
                }
            }
            else if (comboBoxShips.Text.Contains("Travel") || comboBoxShips.Text.Length == 0 )  // second is due to the order History gets called vs this on start
            {
                if (last_he != null && last_he.ShipInformation != null)
                {
                    Display(last_he.ShipInformation);
                }
            }
            else
            {
                ShipInformation si = discoveryform.history.shipinformationlist.GetShipByShortName(comboBoxShips.Text);
                if (si != null)
                    Display(si);
            }

            dataGridViewModules.Sort(sortcol, (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
            dataGridViewModules.Columns[0].HeaderCell.SortGlyphDirection = sortorder;
        }

        public void Display(ShipInformation si)
        {
            foreach (string key in si.Modules.Keys)
            {
                EliteDangerous.JournalEvents.JournalLoadout.ShipModule sm = si.Modules[key];

                string ammo = "";
                if (sm.AmmoHopper.HasValue)
                {
                    ammo = sm.AmmoHopper.Value.ToString();
                    if (sm.AmmoClip.HasValue)
                        ammo += "/" + sm.AmmoClip.ToString();
                }

                string blueprint = "";
                if (sm.Blueprint!=null)
                {
                    blueprint = sm.Blueprint;
                    if (sm.BlueprintLevel.HasValue)
                        blueprint += ":" + sm.BlueprintLevel.ToString();
                }

                string value = (sm.Value.HasValue && sm.Value.Value > 0) ? sm.Value.Value.ToString(System.Globalization.CultureInfo.InvariantCulture) : "";

                string pe = "";
                if (sm.Priority.HasValue)
                    pe = "P" + sm.Priority.Value.ToString();
                if (sm.Enabled.HasValue)
                    pe += "E";

                object[] rowobj = { sm.Slot, sm.Item, sm.LocalisedItem.ToNullSafeString() , ammo, blueprint , value, pe };
                dataGridViewModules.Rows.Add(rowobj);

            }

            LabelVehicleText.Visible = labelVehicle.Visible = true;
            labelVehicle.Text = si.ShipFullInfo;
        }

        #endregion

        #region Layout

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridViewModules, DbColumnSave);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewModules, DbColumnSave);
            discoveryform.TravelControl.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
        }

        #endregion

        private void comboBoxHistoryWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxShips.Enabled)
            {
                DB.SQLiteDBClass.PutSettingString(DbShipSave, comboBoxShips.Text);
                Display();
            }
        }
    }
}
