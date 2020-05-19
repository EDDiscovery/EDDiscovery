/*
 * Copyright © 2016 - 2020 EDDiscovery development team
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
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlModules : UserControlCommonBase
    {
        private string DbColumnSave { get { return DBName("ModulesGrid", "DGVCol"); } }
        private string DbShipSave { get { return DBName("ModulesGridShipSelect"); } }
        private string DbWordWrap { get { return DBName("ModulesGridWordWrap"); } }

        private string storedmoduletext;
        private string travelhistorytext;
        private string allmodulestext;
        private HistoryEntry last_he = null;
        private ShipInformation last_si = null;

        private string fuellevelname;
        private string fuelresname;

        #region Init

        public UserControlModules()
        {
            InitializeComponent();
            var corner = dataGridViewModules.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);
            storedmoduletext = "Stored Modules".T(EDTx.UserControlModules_StoredModules);
            travelhistorytext = "Travel History Entry".T(EDTx.UserControlModules_TravelHistoryEntry);
            allmodulestext = "All Modules".T(EDTx.UserControlModules_AllModules); //TBD
            dataGridViewModules.MakeDoubleBuffered();
            dataGridViewModules.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            buttonExtCoriolis.Visible = buttonExtEDShipyard.Visible = buttonExtConfigure.Visible = false;

            extCheckBoxWordWrap.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbWordWrap, false);
            UpdateWordWrap();
            extCheckBoxWordWrap.Click += extCheckBoxWordWrap_Click;

            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange; ;
            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            discoveryform.OnNewUIEvent += Discoveryform_OnNewUIEvent;

            fuelresname = "Fuel Reserve Capacity".T(EDTx.UserControlModules_FuelReserveCapacity);
            fuellevelname = "Fuel Level".T(EDTx.UserControlModules_FuelLevel);
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;
        }

        public override void LoadLayout()
        {
            dataGridViewModules.RowTemplate.MinimumHeight = Font.ScalePixels(26);
            uctg.OnTravelSelectionChanged += Display;
            DGVLoadColumnLayout(dataGridViewModules, DbColumnSave);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewModules, DbColumnSave);
            uctg.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
            discoveryform.OnNewUIEvent -= Discoveryform_OnNewUIEvent;
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

        private void Discoveryform_OnNewUIEvent(UIEvent obj)
        {
            if (obj is EliteDangerousCore.UIEvents.UIFuel && last_si != null && discoveryform.history.GetLast != null ) // fuel UI update the SI information globally, and we have a ship, and we have a last entry
            {
                if (last_si.ShipNameIdentType == discoveryform.history.GetLast.ShipInformation.ShipNameIdentType ) // and we are pointing at the same ship, use name since the last_si may be an old one if fuel keeps on updating it.
                {
                    // update grid if found. Doing it this way stops flicker.

                    int rowfuel = dataGridViewModules.FindRowWithValue(0, fuellevelname);
                    if (rowfuel != -1)
                        dataGridViewModules.Rows[rowfuel].Cells[1].Value = last_he.ShipInformation.FuelLevel.ToString("N1") + "t";
                    int rowres = dataGridViewModules.FindRowWithValue(0, fuelresname);
                        dataGridViewModules.Rows[rowres].Cells[1].Value = last_he.ShipInformation.ReserveFuelCapacity.ToString("N2") + "t";

                    System.Diagnostics.Debug.WriteLine("Modules Fuel update");
                }
            }
        }

        public override void InitialDisplay()
        {
            labelVehicle.Visible = buttonExtCoriolis.Visible = buttonExtEDShipyard.Visible = buttonExtConfigure.Visible = false;
            Display(uctg.GetCurrentHistoryEntry, discoveryform.history , true);
        }

        private void Display(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            if (comboBoxShips.Items.Count == 0)
                UpdateComboBox(hl);

            last_he = he;
            Display();
        }

        private void Display()      // allow redisplay of last data
        {
            DataGridViewColumn sortcolprev = dataGridViewModules.SortedColumn != null ? dataGridViewModules.SortedColumn : dataGridViewModules.Columns[0];
            SortOrder sortorderprev = dataGridViewModules.SortedColumn != null ? dataGridViewModules.SortOrder : SortOrder.Ascending;
            int firstline = dataGridViewModules.FirstDisplayedScrollingRowIndex;

            dataGridViewModules.Rows.Clear();

            dataViewScrollerPanel.SuspendLayout();

            last_si = null;     // no ship info

            dataGridViewModules.Columns[2].HeaderText = "Slot".T(EDTx.UserControlModules_SlotCol);
            dataGridViewModules.Columns[3].HeaderText = "Info".T(EDTx.UserControlModules_ItemInfo);
            dataGridViewModules.Columns[6].HeaderText = "Value".T(EDTx.UserControlModules_Value);

            if (comboBoxShips.Text == storedmoduletext)
            {
                labelVehicle.Visible = buttonExtCoriolis.Visible = buttonExtEDShipyard.Visible = buttonExtConfigure.Visible = false;

                if (last_he != null && last_he.StoredModules != null)
                {
                    ModulesInStore mi = last_he.StoredModules;
                    labelVehicle.Text = "";

                    foreach (ModulesInStore.StoredModule sm in mi.StoredModules)
                    {
                        object[] rowobj = { sm.Name_Localised.Alt(sm.Name), sm.Name,
                                sm.StarSystem.Alt("In Transit".T(EDTx.UserControlModules_InTransit)), sm.TransferTimeString ,
                                sm.Mass > 0 ? (sm.Mass.ToString()+"t") : "",
                                sm.EngineerModifications.Alt(""),
                                sm.TransferCost>0 ? sm.TransferCost.ToString("N0") : "",
                                "" };
                        dataGridViewModules.Rows.Add(rowobj);
                    }

                    dataGridViewModules.Columns[2].HeaderText = "System".T(EDTx.UserControlModules_System);
                    dataGridViewModules.Columns[3].HeaderText = "Tx Time".T(EDTx.UserControlModules_TxTime);
                    dataGridViewModules.Columns[6].HeaderText = "Cost".T(EDTx.UserControlModules_Cost);
                }
            }
            else if (comboBoxShips.Text == allmodulestext)
            {
                labelVehicle.Visible = buttonExtCoriolis.Visible = buttonExtEDShipyard.Visible = buttonExtConfigure.Visible = false;

                ShipInformationList shm = discoveryform.history.shipinformationlist;
                var ownedships = (from x1 in shm.Ships where x1.Value.State == ShipInformation.ShipState.Owned && !ShipModuleData.IsSRVOrFighter(x1.Value.ShipFD) select x1.Value);

                foreach( var si in ownedships )
                {
                    foreach (string key in si.Modules.Keys)
                    {
                        EliteDangerousCore.ShipModule sm = si.Modules[key];
                        AddModuleLine(sm,si);

                    }
                }

                if (last_he != null && last_he.StoredModules != null)
                {
                    ModulesInStore mi = last_he.StoredModules;
                    labelVehicle.Text = "";

                    foreach (ModulesInStore.StoredModule sm in mi.StoredModules)
                    {
                        string info = sm.StarSystem.Alt("In Transit".T(EDTx.UserControlModules_InTransit));
                        info = info.AppendPrePad(sm.TransferTimeString, ":");
                        object[] rowobj = { sm.Name_Localised.Alt(sm.Name), sm.Name, "Stored".Tx(EDTx.UserControlModules_Stored),
                                 info ,
                                sm.Mass > 0 ? (sm.Mass.ToString()+"t") : "",
                                sm.EngineerModifications.Alt(""),
                                sm.TransferCost>0 ? sm.TransferCost.ToString("N0") : "",
                                "" };
                        dataGridViewModules.Rows.Add(rowobj);
                    }
                }
            }
            else if (comboBoxShips.Text == travelhistorytext || comboBoxShips.Text.Length == 0)  // second is due to the order History gets called vs this on start
            {
                if (last_he != null && last_he.ShipInformation != null)
                {
                    DisplayShip(last_he.ShipInformation);
                }
            }
            else
            {
                ShipInformation si = discoveryform.history.shipinformationlist.GetShipByNameIdentType(comboBoxShips.Text);
                if (si != null)
                    DisplayShip(si);
            }

            dataViewScrollerPanel.ResumeLayout();

            dataGridViewModules.Sort(sortcolprev, (sortorderprev == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
            dataGridViewModules.Columns[sortcolprev.Index].HeaderCell.SortGlyphDirection = sortorderprev;
            if (firstline >= 0 && firstline < dataGridViewModules.RowCount)
                dataGridViewModules.SafeFirstDisplayedScrollingRowIndex(firstline);
        }

        private void DisplayShip(ShipInformation si)
        {
            //System.Diagnostics.Debug.WriteLine("HE " + last_he.Indexno);
            last_si = si;

            foreach (string key in si.Modules.Keys)
            {
                EliteDangerousCore.ShipModule sm = si.Modules[key];
                AddModuleLine(sm);
            }

            double hullmass = si.HullMass();
            double modulemass = si.ModuleMass();

            EliteDangerousCalculations.FSDSpec fsdspec = si.GetFSDSpec();
            if (fsdspec != null)
            {
                EliteDangerousCalculations.FSDSpec.JumpInfo ji = fsdspec.GetJumpInfo(0, modulemass + hullmass, si.FuelCapacity, si.FuelCapacity / 2);
                AddInfoLine("FSD Avg Jump".T(EDTx.UserControlModules_FSDAvgJump), ji.avgsinglejumpnocargo.ToString("N2") + "ly", "Half tank, no cargo".T(EDTx.UserControlModules_HT), fsdspec.ToString());
                DataGridViewRow rw = dataGridViewModules.Rows[dataGridViewModules.Rows.Count - 1];
                AddInfoLine("FSD Max Range".T(EDTx.UserControlModules_FSDMaxRange), ji.maxjumprange.ToString("N2") + "ly", "Full Tank, no cargo".T(EDTx.UserControlModules_FT), fsdspec.ToString());
                AddInfoLine("FSD Maximum Fuel per jump".T(EDTx.UserControlModules_FSDMaximumFuelperjump), fsdspec.MaxFuelPerJump.ToString() + "t", "", fsdspec.ToString());
            }

            if (si.HullValue > 0)
                AddValueLine("Hull Value".T(EDTx.UserControlModules_HullValue), si.HullValue);
            if (si.ModulesValue > 0)
                AddValueLine("Modules Value".T(EDTx.UserControlModules_ModulesValue), si.ModulesValue);
            if (si.HullValue > 0 && si.ModulesValue > 0)
                AddValueLine("Total Cost".T(EDTx.UserControlModules_TotalCost), si.HullValue + si.ModulesValue);
            if (si.Rebuy > 0)
                AddValueLine("Rebuy Cost".T(EDTx.UserControlModules_RebuyCost), si.Rebuy);

            AddMassLine("Mass Hull".T(EDTx.UserControlModules_MassHull), hullmass.ToString("N1") + "t");
            AddMassLine("Mass Unladen".T(EDTx.UserControlModules_MassUnladen), (hullmass + modulemass).ToString("N1") + "t");
            if (si.UnladenMass > 0)
                AddMassLine("Mass FD Unladen".T(EDTx.UserControlModules_MassFDUnladen), si.UnladenMass.ToString("N1") + "t");
            AddMassLine("Mass Modules".T(EDTx.UserControlModules_MassModules), modulemass.ToString("N1") + "t");

            AddInfoLine("Manufacturer".T(EDTx.UserControlModules_Manufacturer), si.Manufacturer);

            if (si.FuelCapacity > 0)
                AddInfoLine("Fuel Capacity".T(EDTx.UserControlModules_FuelCapacity), si.FuelCapacity.ToString("N1") + "t");
            if (si.FuelLevel > 0)
                AddInfoLine(fuellevelname, si.FuelLevel.ToString("N1") + "t");
            if (si.ReserveFuelCapacity > 0)
                AddInfoLine(fuelresname, si.ReserveFuelCapacity.ToString("N2") + "t");
            if (si.HullHealthAtLoadout > 0)
                AddInfoLine("Hull Health (Loadout)".T(EDTx.UserControlModules_HullHealth), si.HullHealthAtLoadout.ToString("N1") + "%");

            double fuelwarn = si.FuelWarningPercent;
            AddInfoLine("Fuel Warning %".T(EDTx.UserControlModules_FuelWarning), fuelwarn > 0 ? fuelwarn.ToString("N1") + "%" : "Off".T(EDTx.Off));

            AddInfoLine("Pad Size".T(EDTx.UserControlModules_PadSize), si.PadSize);
            AddInfoLine("Main Thruster Speed".T(EDTx.UserControlModules_MainThrusterSpeed), si.Speed.ToString("0.#"));
            AddInfoLine("Main Thruster Boost".T(EDTx.UserControlModules_MainThrusterBoost), si.Boost.ToString("0.#"));

            if (si.InTransit)
                AddInfoLine("In Transit to ".T(EDTx.UserControlModules_InTransitto), (si.StoredAtSystem ?? "Unknown".T(EDTx.Unknown)) + ":" + (si.StoredAtStation ?? "Unknown".T(EDTx.Unknown)));
            else if (si.StoredAtSystem != null)
                AddInfoLine("Stored at".T(EDTx.UserControlModules_Storedat), si.StoredAtSystem + ":" + (si.StoredAtStation ?? "Unknown".T(EDTx.Unknown)));

            int cc = si.CargoCapacity();
            if (cc > 0)
                AddInfoLine("Cargo Capacity".T(EDTx.UserControlModules_CargoCapacity), cc.ToString("N0") + "t");

            labelVehicle.Visible = buttonExtCoriolis.Visible = buttonExtEDShipyard.Visible = buttonExtConfigure.Visible = true;
            labelVehicle.Text = si.ShipFullInfo(cargo: false, fuel: false);
            buttonExtConfigure.Visible = si.State == ShipInformation.ShipState.Owned;
            buttonExtCoriolis.Visible = buttonExtEDShipyard.Visible = si.CheckMinimumJSONModules();
        }

        void AddModuleLine(ShipModule sm , ShipInformation si = null)
        {
            string infoentry = "";

            if (si != null)
                infoentry = si.ShipNameIdentType;
            else if (sm.AmmoHopper.HasValue)
            {
                infoentry = sm.AmmoHopper.Value.ToString();
                if (sm.AmmoClip.HasValue)
                    infoentry += "/" + sm.AmmoClip.ToString();
            }

            string value = (sm.Value.HasValue && sm.Value.Value > 0) ? sm.Value.Value.ToString("N0") : "";

            string typename = sm.LocalisedItem;
            if (typename.IsEmpty())
                typename = ShipModuleData.Instance.GetItemProperties(sm.ItemFD).ModType;

            string eng = "";
            if ( sm.Engineering != null )
                eng = sm.Engineering.FriendlyBlueprintName + ":" + sm.Engineering.Level.ToStringInvariant();

            object[] rowobj = { typename,
                                sm.Item, sm.Slot, infoentry,
                                sm.Mass > 0 ? (sm.Mass.ToString("0.#")+"t") : "",
                                eng,
                                value, sm.PE() };

            dataGridViewModules.Rows.Add(rowobj);

            if (sm.Engineering != null)
            {
                string text = sm.Engineering.ToString();
                EliteDangerousCalculations.FSDSpec spec = sm.GetFSDSpec();
                if (spec != null)
                    text += spec.ToString();

                dataGridViewModules.Rows[dataGridViewModules.Rows.Count - 1].Cells[5].ToolTipText = text;
            }
        }

        void AddValueLine(string s, long v, string opt = "")
        {
            object[] rowobj = { s, opt, "", "", "", "", v.ToString("N0"), "" };
            dataGridViewModules.Rows.Add(rowobj);
        }

        void AddInfoLine(string s, string v, string opt = "", string tooltip = "")
        {
            object[] rowobj = { s, v.AppendPrePad(opt), "", "", "", "", "", "" };
            dataGridViewModules.Rows.Add(rowobj);
            if (!string.IsNullOrEmpty(tooltip))
                dataGridViewModules.Rows[dataGridViewModules.Rows.Count - 1].Cells[0].ToolTipText = tooltip;
        }

        void AddMassLine(string m, string v, string opt = "")
        {
            object[] rowobj = { m, opt, "", "", v, "", "", "" };
            dataGridViewModules.Rows.Add(rowobj);
        }

        #endregion

        #region Word wrap

        private void extCheckBoxWordWrap_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbWordWrap, extCheckBoxWordWrap.Checked);
            UpdateWordWrap();
        }

        private void UpdateWordWrap()
        {
            dataGridViewModules.DefaultCellStyle.WrapMode = extCheckBoxWordWrap.Checked ? DataGridViewTriState.True : DataGridViewTriState.False;
            dataGridViewModules.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dataViewScrollerPanel.UpdateScroll();
        }

        #endregion


        private void UpdateComboBox(HistoryList hl)
        {
            ShipInformationList shm = hl.shipinformationlist;
            string cursel = comboBoxShips.Text;

            comboBoxShips.Items.Clear();
            comboBoxShips.Items.Add(travelhistorytext);
            comboBoxShips.Items.Add(storedmoduletext);
            comboBoxShips.Items.Add(allmodulestext);

            var ownedships = (from x1 in shm.Ships where x1.Value.State == ShipInformation.ShipState.Owned && !ShipModuleData.IsSRVOrFighter(x1.Value.ShipFD) select x1.Value);
            var notownedships = (from x1 in shm.Ships where x1.Value.State != ShipInformation.ShipState.Owned && !ShipModuleData.IsSRVOrFighter(x1.Value.ShipFD) select x1.Value);
            var fightersrvs = (from x1 in shm.Ships where ShipModuleData.IsSRVOrFighter(x1.Value.ShipFD) select x1.Value);

            var now = (from x1 in ownedships where x1.StoredAtSystem == null select x1.ShipNameIdentType).ToList();
            comboBoxShips.Items.AddRange(now);

            var stored = (from x1 in ownedships where x1.StoredAtSystem != null select x1.ShipNameIdentType).ToList();
            comboBoxShips.Items.AddRange(stored);

            comboBoxShips.Items.AddRange(notownedships.Select(x => x.ShipNameIdentType).ToList());
            comboBoxShips.Items.AddRange(fightersrvs.Select(x => x.ShipNameIdentType).ToList());

            if (cursel == "")
                cursel = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbShipSave, "");

            if (cursel == "" || !comboBoxShips.Items.Contains(cursel))
                cursel = travelhistorytext;

            comboBoxShips.Enabled = false;
            comboBoxShips.SelectedItem = cursel;
            comboBoxShips.Enabled = true;
        }

        private void comboBoxHistoryWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxShips.Enabled)
            {
                EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(DbShipSave, comboBoxShips.Text);
                Display();
            }
        }

        private void buttonExtCoriolis_Click(object sender, EventArgs e)
        {
            ShipInformation si = null;

            if (comboBoxShips.Text == travelhistorytext || comboBoxShips.Text.Length == 0)  // second is due to the order History gets called vs this on start
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

                string uri = EDDConfig.Instance.CoriolisURL + "data=" + BaseUtils.HttpUriEncode.URIGZipBase64Escape(s) + "&bn=" + Uri.EscapeDataString(si.Name);

                if (!BaseUtils.BrowserInfo.LaunchBrowser(uri))
                {
                    ExtendedControls.InfoForm info = new ExtendedControls.InfoForm();
                    info.Info("Cannot launch browser, use this JSON for manual Coriolis import", FindForm().Icon, s);
                    info.ShowDialog(FindForm());
                }
            }
        }

        private void buttonExtCoriolis_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string url = ExtendedControls.PromptSingleLine.ShowDialog(this.FindForm(), "URL:", EDDConfig.Instance.CoriolisURL,
                            "Enter Coriolis URL".T(EDTx.UserControlModules_CURL), this.FindForm().Icon);
                if (url != null)
                    EDDConfig.Instance.CoriolisURL = url;
            }
        }

        private void buttonExtEDShipyard_Click(object sender, EventArgs e)
        {
            ShipInformation si = null;

            if (comboBoxShips.Text == travelhistorytext || comboBoxShips.Text.Length == 0)  // second is due to the order History gets called vs this on start
            {
                if (last_he != null && last_he.ShipInformation != null)
                    si = last_he.ShipInformation;
            }
            else
                si = discoveryform.history.shipinformationlist.GetShipByNameIdentType(comboBoxShips.Text);

            if (si != null)
            {
                Newtonsoft.Json.Linq.JObject jo = si.ToJSONLoadout();

                string loadoutjournalline = jo.ToString(Newtonsoft.Json.Formatting.Indented);

                //     File.WriteAllText(@"c:\code\loadoutout.txt", loadoutjournalline);

                string uri = EDDConfig.Instance.EDDShipyardURL + "#/I=" + BaseUtils.HttpUriEncode.URIGZipBase64Escape(loadoutjournalline);

                if (!BaseUtils.BrowserInfo.LaunchBrowser(uri))
                {
                    ExtendedControls.InfoForm info = new ExtendedControls.InfoForm();
                    info.Info("Cannot launch browser, use this JSON for manual ED Shipyard import", FindForm().Icon, loadoutjournalline);
                    info.ShowDialog(FindForm());
                }
            }
        }

        private void buttonExtEDShipyard_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string url = ExtendedControls.PromptSingleLine.ShowDialog(this.FindForm(), "URL:", EDDConfig.Instance.EDDShipyardURL, "Enter ED Shipyard URL".T(EDTx.UserControlModules_EDSURL), this.FindForm().Icon);
                if (url != null)
                    EDDConfig.Instance.EDDShipyardURL = url;
            }
        }

        private void buttonExtConfigure_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Assert(last_si != null);           // must be set for this configure button to be visible

            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();

            int width = 430;
            int ctrlleft = 150;

            f.Add(new ExtendedControls.ConfigurableForm.Entry("L", typeof(Label), "Fuel Warning:".T(EDTx.UserControlModules_FW), new Point(10, 40), new Size(140, 24), ""));
            f.Add(new ExtendedControls.ConfigurableForm.Entry("FuelWarning", typeof(ExtendedControls.NumberBoxDouble),
                last_si.FuelWarningPercent.ToString(), new Point(ctrlleft, 40), new Size(width - ctrlleft - 20, 24), "Enter fuel warning level in % (0 = off, 1-100%)".T(EDTx.UserControlModules_TTF))
            { numberboxdoubleminimum = 0, numberboxdoublemaximum = 100, numberboxformat = "0.##" });

            f.Add(new ExtendedControls.ConfigurableForm.Entry("Sell", typeof(ExtendedControls.ExtButton), "Force Sell".T(EDTx.UserControlModules_ForceSell), new Point(10, 80), new Size(80, 24),null));

            f.AddOK(new Point(width - 100, 110), "Press to Accept".T(EDTx.UserControlModules_PresstoAccept));
            f.AddCancel(new Point(width - 200, 110), "Press to Cancel".T(EDTx.UserControlModules_PresstoCancel));

            f.Trigger += (dialogname, controlname, tag) =>
            {
                if (controlname == "OK")
                {
                    double? v3 = f.GetDouble("FuelWarning");
                    if (v3.HasValue)
                    {
                        f.ReturnResult(DialogResult.OK);
                    }
                    else
                        ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "A Value is not valid".T(EDTx.UserControlModules_NValid), "Warning".T(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (controlname == "Cancel" || controlname == "Close" )
                {
                    f.ReturnResult(DialogResult.Cancel);
                }
                else if (controlname == "Sell")
                {
                    if ( ExtendedControls.MessageBoxTheme.Show(FindForm(), "Confirm sell of ship:".Tx(EDTx.EDDiscoveryForm_ConfirmSyncToEDSM) + Environment.NewLine + last_si.ShipNameIdentType , "Warning".T(EDTx.Warning), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes ) 
                    {
                        var je = new EliteDangerousCore.JournalEvents.JournalShipyardSell(DateTime.UtcNow);
                        je.ShipTypeFD = last_si.ShipFD;
                        je.SellShipId = last_si.ID;
                        je.ShipPrice = 0;
                        je.SetCommander(EDCommander.CurrentCmdrID);
                        var jo = je.Json();
                        je.Add(jo);
                        discoveryform.NewEntry(je);
                    }

                    f.ReturnResult(DialogResult.Cancel);
                }
            };

            DialogResult res = f.ShowDialogCentred(this.FindForm(), this.FindForm().Icon, "Ship Configure".T(EDTx.UserControlModules_SC), closeicon:true);

            if (res == DialogResult.OK)
            {
                last_si.FuelWarningPercent = f.GetDouble("FuelWarning").Value;
                Display();
            }
        }

        private void dataGridViewModules_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 4 || e.Column.Index == 6)
                e.SortDataGridViewColumnNumeric("t");
        }

        private void dataGridViewModules_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewModules.RowCount)
            {
                DataGridViewRow row = dataGridViewModules.Rows[e.RowIndex];

                bool expanded = row.Cells[0].Tag != null && (bool)row.Cells[0].Tag == true;     // cell 0 tag holds expanded state

                for (int i = 0; i < dataGridViewModules.ColumnCount; i++)
                {
                    DataGridViewCell cell = row.Cells[i];
                    cell.Style.WrapMode = expanded ? DataGridViewTriState.NotSet : DataGridViewTriState.True;
                }

                row.Cells[0].Tag = !expanded;
            }
        }

        private void dataGridViewModules_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string tt = dataGridViewModules.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText;
                if (!string.IsNullOrEmpty(tt))
                {
                    Form mainform = FindForm();
                    ExtendedControls.InfoForm frm = new ExtendedControls.InfoForm();
                    frm.Info("Module Information".T(EDTx.UserControlModules_MI), mainform.Icon, tt);
                    frm.Size = new Size(600, 400);
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.Show(mainform);
                }
            }
        }

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            if (dataGridViewModules.RowCount>0)
            {
                Forms.ExportForm frm = new Forms.ExportForm();
                frm.Init(new string[] { "Export Current View" }, disablestartendtime: true);

                if (frm.ShowDialog(this.FindForm()) == DialogResult.OK)
                {
                    BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid();
                    grd.SetCSVDelimiter(frm.Comma);

                    grd.GetPreHeader += delegate (int r)
                    {
                        if (last_si != null)
                        {
                            if (r == 0)
                                return new Object[] { last_si.ShipUserName ?? "", last_si.ShipUserIdent ?? "", last_si.ShipType ?? "", last_si.ID };
                            else if (r == 1)
                                return new Object[] { };
                        }

                        return null;
                    };

                    grd.GetHeader += delegate (int c)
                    {
                        return (frm.IncludeHeader && c < dataGridViewModules.ColumnCount) ? dataGridViewModules.Columns[c].HeaderText : null;
                    };

                    grd.GetLine += delegate (int r)
                    {
                        if (r < dataGridViewModules.RowCount)
                        {
                            DataGridViewRow rw = dataGridViewModules.Rows[r];
                            return new Object[] { rw.Cells[0].Value, rw.Cells[1].Value, rw.Cells[2].Value, rw.Cells[3].Value, rw.Cells[4].Value, rw.Cells[5].Value, rw.Cells[6].Value, rw.Cells[7].Value };
                        }
                        else
                            return null;
                    };

                    var x = discoveryform.history.shipinformationlist.Ships.GetEnumerator();
                    x.MoveNext();

                    grd.GetPostHeader += delegate (int r)
                    {
                        if (r == 0)
                            return new Object[] { };
                        else if ( r == 1 )
                            return new Object[] { "Ships:" };

                        while (x.MoveNext())
                        {
                            if ( x.Current.Value.State == ShipInformation.ShipState.Owned )
                                return new Object[] { x.Current.Value.ShipFullInfo() };
                        }

                        return null;
                    };

                    grd.WriteGrid(frm.Path, frm.AutoOpen, FindForm());
                }
            }
            else
                ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "No Ship Information available".T(EDTx.UserControlModules_NOSI), "Warning".T(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
}
