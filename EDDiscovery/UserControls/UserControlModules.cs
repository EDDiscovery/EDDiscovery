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
    public partial class UserControlModules : UserControlCommonBase
    {
        private string DbColumnSave { get { return ("ModulesGrid") + ((displaynumber > 0) ? displaynumber.ToString() : "") + "DGVCol"; } }
        private string DbShipSave { get { return "ModulesGridShipSelect" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        #region Init

        public UserControlModules()
        {
            InitializeComponent();
            var corner = dataGridViewModules.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            dataGridViewModules.MakeDoubleBuffered();
            dataGridViewModules.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dataGridViewModules.RowTemplate.Height = 26;

            buttonExtCoriolis.Visible = buttonExtEDShipyard.Visible = false;

            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange; ;
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
            foreach ( string id in shm.Ships.Keys)
            {
                ShipInformation sm = shm.Ships[id];
                comboBoxShips.Items.Add(sm.ShipNameIdentType);
            }

            if (cursel == "")
                cursel = SQLiteDBClass.GetSettingString(DbShipSave, "");

            if (cursel == "" || !comboBoxShips.Items.Contains(cursel))
                cursel = "Travel History Entry";

            comboBoxShips.Enabled = false;
            comboBoxShips.SelectedItem = cursel;
            comboBoxShips.Enabled = true;
        }

        public override void InitialDisplay()
        {
            Display(uctg.GetCurrentHistoryEntry, discoveryform.history);
        }

        HistoryEntry last_he = null;
        private void Display(HistoryEntry he, HistoryList hl)
        {
            if ( comboBoxShips.Items.Count == 0 )
                UpdateComboBox(hl);

            last_he = he;
            Display();
        }

        private void Display()
        {
            DataGridViewColumn sortcol = dataGridViewModules.SortedColumn != null ? dataGridViewModules.SortedColumn : dataGridViewModules.Columns[0];
            SortOrder sortorder = dataGridViewModules.SortOrder;

            dataGridViewModules.Rows.Clear();

            LabelVehicleText.Visible = false;
            labelVehicle.Visible = false;
            buttonExtCoriolis.Visible = buttonExtEDShipyard.Visible = false;

            if (comboBoxShips.Text.Contains("Stored"))
            {
                if (last_he != null && last_he.StoredModules != null)
                {
                    ModulesInStore mi = last_he.StoredModules;
                    labelVehicle.Text = "";
                    int i = 1;
                    foreach(EliteDangerousCore.JournalEvents.JournalLoadout.ShipModule sm in mi.StoredModules )
                    {
                        object[] rowobj = { i.ToString(), sm.Item, sm.LocalisedItem.ToNullSafeString() };
                        dataGridViewModules.Rows.Add(rowobj);
                        i++;
                    }
                }
            }
            else if (comboBoxShips.Text.Contains("Travel") || comboBoxShips.Text.Length == 0)  // second is due to the order History gets called vs this on start
            {
                if (last_he != null && last_he.ShipInformation != null)
                {
                    Display(last_he.ShipInformation);
                }
            }
            else
            {
                ShipInformation si = discoveryform.history.shipinformationlist.GetShipByNameIdentType(comboBoxShips.Text);
                if (si != null)
                    Display(si);
            }

            dataGridViewModules.Sort(sortcol, (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
            dataGridViewModules.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;
        }

        private void Display(ShipInformation si)
        {
            foreach (string key in si.Modules.Keys)
            {
                EliteDangerousCore.JournalEvents.JournalLoadout.ShipModule sm = si.Modules[key];

                string ammo = "";
                if (sm.AmmoHopper.HasValue)
                {
                    ammo = sm.AmmoHopper.Value.ToString();
                    if (sm.AmmoClip.HasValue)
                        ammo += "/" + sm.AmmoClip.ToString();
                }

                string value = (sm.Value.HasValue && sm.Value.Value > 0) ? sm.Value.Value.ToString(System.Globalization.CultureInfo.InvariantCulture) : "";

                string eng = "";
                if (sm.Engineering != null)
                    eng = sm.Engineering.FriendlyBlueprintName;

                object[] rowobj = { sm.Slot, sm.Item, sm.LocalisedItem.ToNullSafeString(), ammo, eng, value, sm.PE() };
                // debug object[] rowobj = { sm.Slot+":" + sm.SlotFD, sm.Item + ":" + sm.ItemFD, sm.LocalisedItem.ToNullSafeString() , ammo, blueprint , value, sm.PE() };
                dataGridViewModules.Rows.Add(rowobj);

            }

            if (si.HullValue > 0)
                AddValueLine("Hull Value", si.HullValue);
            if (si.ModulesValue > 0)
                AddValueLine("Modules Value", si.ModulesValue);
            if (si.HullValue > 0 && si.ModulesValue > 0)
                AddValueLine("Total Cost", si.HullValue + si.ModulesValue);
            if (si.Rebuy > 0)
                AddValueLine("Rebuy Cost", si.Rebuy);

            LabelVehicleText.Visible = labelVehicle.Visible = true;
            labelVehicle.Text = si.ShipFullInfo();
            buttonExtCoriolis.Visible = buttonExtEDShipyard.Visible = si.CheckMinimumJSONModules();
        }

        void AddValueLine(string s, long v )
        {
            object[] rowobj = { s, "", "", "", "", v.ToString("N0"), "" };
            dataGridViewModules.Rows.Add(rowobj);
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
            uctg.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
        }

        #endregion

        private void comboBoxHistoryWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxShips.Enabled)
            {
                SQLiteDBClass.PutSettingString(DbShipSave, comboBoxShips.Text);
                Display();
            }
        }

        private void buttonExtCoriolis_Click(object sender, EventArgs e)
        {
            ShipInformation si = null;

            if (comboBoxShips.Text.Contains("Travel") || comboBoxShips.Text.Length == 0)  // second is due to the order History gets called vs this on start
            {
                if (last_he != null && last_he.ShipInformation != null)
                    si = last_he.ShipInformation;
            }
            else
                si = discoveryform.history.shipinformationlist.GetShipByNameIdentType(comboBoxShips.Text);

            if (si != null)
            {
                string errstr;
                string s = si.ToJSONCoriolis(out errstr);

                if (errstr.Length > 0)
                    ExtendedControls.MessageBoxTheme.Show(FindForm(), errstr + Environment.NewLine + "This is probably a new or powerplay module" + Environment.NewLine + "Report to EDD Team by Github giving the full text above", "Unknown Module Type");

                string uri = Properties.Resources.URLCoriolis + "data=" + BaseUtils.HttpUriEncode.URIGZipBase64Escape(s) + "&bn=" + Uri.EscapeDataString(si.Name);

                if (!BaseUtils.BrowserInfo.LaunchBrowser(uri))
                {
                    ExtendedControls.InfoForm info = new ExtendedControls.InfoForm();
                    info.Info("Cannot launch browser, use this JSON for manual Coriolis import", Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location),
                                    s, new int[] { 0, 100 });
                    info.ShowDialog(FindForm());
                }
            }
        }

        private void buttonExtEDShipyard_Click(object sender, EventArgs e)
        {
            ShipInformation si = null;

            if (comboBoxShips.Text.Contains("Travel") || comboBoxShips.Text.Length == 0)  // second is due to the order History gets called vs this on start
            {
                if (last_he != null && last_he.ShipInformation != null)
                    si = last_he.ShipInformation;
            }
            else
                si = discoveryform.history.shipinformationlist.GetShipByNameIdentType(comboBoxShips.Text);

            if (si != null)
            {
                string s = si.ToJSONLoadout();

                string uri = Properties.Resources.URLEDShipyard + "#/I=" + BaseUtils.HttpUriEncode.URIGZipBase64Escape(s);

                //File.WriteAllText(@"c:\code\out.txt", uri);

                if (!BaseUtils.BrowserInfo.LaunchBrowser(uri))
                {
                    ExtendedControls.InfoForm info = new ExtendedControls.InfoForm();
                    info.Info("Cannot launch browser, use this JSON for manual ED Shipyard import", Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location),
                                    s, new int[] { 0, 100 });
                    info.ShowDialog(FindForm());
                }
            }
        }

        private void dataGridViewModules_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 5)
                e.SortDataGridViewColumnDate();
        }

    }
}
