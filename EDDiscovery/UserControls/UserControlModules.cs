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
using EDDiscovery.ExtendedControls;
using EDDiscovery.DB;
using EDDiscovery.EliteDangerous;
using System.IO;

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

            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange; ;
            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            ed.TravelControl.OnTravelSelectionChanged += Display;

            buttonExtCoriolis.Visible = false;
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
            buttonExtCoriolis.Visible = false;

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
            else if (comboBoxShips.Text.Contains("Travel") || comboBoxShips.Text.Length == 0)  // second is due to the order History gets called vs this on start
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
            dataGridViewModules.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;
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


                object[] rowobj = { sm.Slot, sm.Item, sm.LocalisedItem.ToNullSafeString() , ammo, blueprint , value, sm.PE() };
                // debug object[] rowobj = { sm.Slot+":" + sm.SlotFD, sm.Item + ":" + sm.ItemFD, sm.LocalisedItem.ToNullSafeString() , ammo, blueprint , value, sm.PE() };
                dataGridViewModules.Rows.Add(rowobj);

            }

            LabelVehicleText.Visible = labelVehicle.Visible = true;
            labelVehicle.Text = si.ShipFullInfo;
            buttonExtCoriolis.Visible = si.CheckMinimumJSONModules();
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

        private void buttonExtCoriolis_Click(object sender, EventArgs e)
        {
            ShipInformation si = null;

            if (comboBoxShips.Text.Contains("Travel") || comboBoxShips.Text.Length == 0)  // second is due to the order History gets called vs this on start
            {
                if (last_he != null && last_he.ShipInformation != null)
                    si = last_he.ShipInformation;
            }
            else 
                si = discoveryform.history.shipinformationlist.GetShipByShortName(comboBoxShips.Text);
                     
            if (si != null)
            {
                string errstr;
                string s = si.ToJSON(out errstr);

                if (errstr.Length > 0)
                    Forms.MessageBoxTheme.Show(this, errstr + Environment.NewLine + "This is probably a new or powerplay module" + Environment.NewLine + "Report to EDD Team by Github giving the full text above", "Unknown Module Type");

                string uri = null;

                var bytes = Encoding.UTF8.GetBytes(s);
                using (MemoryStream indata = new MemoryStream(bytes))
                {
                    using (MemoryStream outdata = new MemoryStream())
                    {
                        using (System.IO.Compression.GZipStream gzipStream = new System.IO.Compression.GZipStream(outdata, System.IO.Compression.CompressionLevel.Optimal, true))
                            indata.CopyTo(gzipStream);      // important to clean up gzip otherwise all the data is not written.. using

                        uri += Properties.Resources.URLCoriolis + "data=" + Uri.EscapeDataString(Convert.ToBase64String(outdata.ToArray()));
                        uri += "&bn=" + Uri.EscapeDataString(si.Name);

                        string browser = Tools.GetDefaultBrowser();

                        if ( browser != null )
                        {
                            string path = Tools.GetBrowserPath(browser);

                            if (path != null)
                            {
                                try
                                {
                                    System.Diagnostics.ProcessStartInfo p = new System.Diagnostics.ProcessStartInfo(path, uri);
                                    p.UseShellExecute = false;
                                    System.Diagnostics.Process.Start(p);
                                    return;
                                }
                                catch (Exception ex)
                                {
                                    Forms.MessageBoxTheme.Show(this, "Unable to launch browser" + ex.Message, "Browser Launch Error");
                                }
                            }
                        }
                         
                    }
                }

                Forms.InfoForm info = new Forms.InfoForm();
                info.Info("Cannot launch browser, use this JSON for manual Coriolis import", s, new Font("MS Sans Serif", 10), new int[] { 0, 100 });
                info.ShowDialog(this);
            }
        }

        private void dataGridViewModules_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 5)
            {
                double v1;
                double v2;
                bool v1hasval = Double.TryParse(e.CellValue1?.ToString().Replace("cr", ""), out v1);
                bool v2hasval = Double.TryParse(e.CellValue2?.ToString().Replace("cr", ""), out v2);

                if (v1hasval)
                {
                    if (v2hasval)
                        e.SortResult = v1.CompareTo(v2);
                    else
                        e.SortResult = 1;
                }
                else if (v2hasval)
                    e.SortResult = -1;
                else
                    return;

                e.Handled = true;
            }
        }
    }
}
