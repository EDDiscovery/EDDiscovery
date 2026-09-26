/*
 * Copyright 2016 - 2026 EDDiscovery development team
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
 */

using EliteDangerousCore;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class ShipsAndModules : UserControlCommonBase
    {
        #region Init

        public ShipsAndModules()
        {
            InitializeComponent();
            BaseUtils.TranslatorMkII.Instance.TranslateControls(this);
            BaseUtils.TranslatorMkII.Instance.TranslateTooltip(toolTip, this);
            DBBaseName = "ModulesGrid";
        }

        protected override void Init()
        {
            currentownedshipstext = "Owned Ships".Tx();
            allownedshipstext = "Owned Ships Current and Sold/Destroyed".Tx();
            allshipstext = "All Ships".Tx();
            storedmoduletext = "Stored Modules".Tx();
            travelhistorytext = "Travel History Entry".Tx();
            allmodulestext = "All Modules".Tx();
            allknownmodulestext = "All Known Modules".Tx();
            dataGridViewModules.MakeDoubleBuffered();

            displayfilters = GetSetting(dbDisplayFilters, "fullblueprint;engineeredvalues").Split(';');

            extCheckBoxWordWrap.Checked = GetSetting(dbWordWrap, true);
            UpdateWordWrap();
            extCheckBoxWordWrap.Click += extCheckBoxWordWrap_Click;

            DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange; ;
            DiscoveryForm.OnNewEntry += Discoveryform_OnNewEntry;
            DiscoveryForm.OnNewUIEvent += Discoveryform_OnNewUIEvent;
            DiscoveryForm.OnThemeChanged += DiscoveryForm_OnThemeChanged;


            multiPipControlEng.Add(multiPipControlSys);
            multiPipControlEng.Add(multiPipControlWep);
            multiPipControlSys.Add(multiPipControlEng);
            multiPipControlSys.Add(multiPipControlWep);
            multiPipControlWep.Add(multiPipControlSys);
            multiPipControlWep.Add(multiPipControlEng);
            multiPipControlEng.ValueChanged += (s) => { DisplayShipStats(last_displayship); };
            multiPipControlSys.ValueChanged += (s) => { DisplayShipStats(last_displayship); };
            multiPipControlWep.ValueChanged += (s) => { DisplayShipStats(last_displayship); };
            extButtonDrawnResetPips.Text = "RST";   // done to bypass translation

            HideShipRelatedButtonsAndPanelsClearModuleDiagram();

            dataGridViewModules.EnableCellHoverOverCallback();
            dataGridViewModules.HoverOverCell += HoverOverCell;

            DiscoveryForm_OnThemeChanged();

            extPictureBoxModules.ClickElement += ModuleDisplayClickElement;

            splitContainerModulesGrid.SplitterDistance(GetSetting(dbModSplitter, 0.4));

            extProgressBarMSPriorities.SegmentColors = new Color[] { Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Magenta };
        }

        protected override void LoadLayout()
        {
            dataGridViewModules.RowTemplate.MinimumHeight = Font.ScalePixels(26);
            DGVLoadColumnLayout(dataGridViewModules);
            ColO1.Visible = ColO2.Visible = false;
        }

        protected override void Closing()
        {
            if ( comboBoxShips.Text != allknownmodulestext)     // we fiddle with the columns in this view, so don't save
                DGVSaveColumnLayout(dataGridViewModules);

            PutSetting(dbModSplitter, splitContainerModulesGrid.GetSplitterDistance());

            DiscoveryForm.OnThemeChanged -= DiscoveryForm_OnThemeChanged;
            DiscoveryForm.OnNewEntry -= Discoveryform_OnNewEntry;
            DiscoveryForm.OnHistoryChange -= Discoveryform_OnHistoryChange;
            DiscoveryForm.OnNewUIEvent -= Discoveryform_OnNewUIEvent;
        }

        #endregion
        private void DiscoveryForm_OnThemeChanged()
        {
            smd.Font = Theme.Current.GetFont;
            smd.TextForeColor = Theme.Current.ListBoxTextColor;
            smd.BoxBorderColor = Theme.Current.ListBoxBorderColor;
            smd.BoxBackColor1 = Theme.Current.ListBoxBackColor;
            smd.BoxBackColor2 = Theme.Current.ListBoxBackColor2;
            smd.BoxSize = new Size(smd.Font.Height *24, smd.Font.Height * 4);
            PbsModuleDisplay_Resize(null, null);
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he)
        {
            if (he.journalEntry is IShipInformation)        // anything that ShipInformationList processes could cause a change in history.StoredModules or history.Shipinformation
            {
                UpdateComboBox();
            }
        }

        private void Discoveryform_OnHistoryChange()
        {
            UpdateComboBox();
        }

        private void Discoveryform_OnNewUIEvent(UIEvent uievent)
        {
            // fuel UI update the SI information globally, and we have a ship, and we have a last entry, and we have ship information
            // protect against ship information or he being null

            if (uievent is EliteDangerousCore.UIEvents.UIFuel && last_displayship != null && DiscoveryForm.History.GetLast?.ShipInformation != null ) 
            {
                // if we are pointing at the same ship, use name since the last_si may be an old one if fuel keeps on updating it.

                if (last_displayship.ShipNameIdentType == DiscoveryForm.History.GetLast.ShipInformation.ShipNameIdentType ) 
                {
                    DisplayShipStats(last_displayship);
                    System.Diagnostics.Debug.WriteLine("Modules Fuel update");
                }
            }
        }

        protected override void InitialDisplay()
        {
            labelVehicle.Visible = buttonExtCoriolis.Visible = buttonExtEDShipyard.Visible = buttonExtConfigure.Visible = false;
            RequestPanelOperation(this, new UserControlCommonBase.RequestHistoryGridPos());     //request an update 
            extButtonDrawnResetPips.Top = multiPipControlEng.Bottom - extButtonDrawnResetPips.Height;       // lets realign the control manually to make sure it lines up at the bottom
        }

        // new entry received from history
        public override void ReceiveHistoryEntry(HistoryEntry he)
        {
            if (comboBoxShips.Items.Count == 0)
                UpdateComboBox();

            bool attopofhistory = he == DiscoveryForm.History.GetLast;

            bool update = false;

            // in declaration order at bottom of file

            if (comboBoxShips.Text == currentownedshipstext || comboBoxShips.Text == allownedshipstext) // these display at top of history, not at history cursor
            {
                if (last_he == null || (attopofhistory && he.journalEntry is IShipInformation))
                    update = true;
                else
                    last_he = he;
            }
            else if (comboBoxShips.Text == storedmoduletext)         // stored at he. Displayed at last_he.
            {
                update = !Object.ReferenceEquals(he.StoredModules, last_he?.StoredModules); // update if stored modules different
            }
            else if (comboBoxShips.Text == travelhistorytext)       // travel history, it should the the current si vs the last displayed si
            {
                update = !Object.ReferenceEquals(he.ShipInformation, last_displayship);
            }
            else if (comboBoxShips.Text == allmodulestext)          // this displays the stored modules, as well as all other ship modules, at top of history
            {
                ShipList shm = DiscoveryForm.History.ShipInformationList;

                List<object> curref = new List<object>();       // repeat the objects used in the display
                foreach (var si in shm.OwnedSpaceShips())  
                    curref.Add(si);
                foreach (ShipModulesInStore.StoredModule sm in shm.StoredModules.StoredModules)
                    curref.Add(sm);

                update = !allmodulesref.ReferenceEquals(curref);        // if not identical, something has changed, execute update
            }
            else if (comboBoxShips.Text == allshipstext)
            {
                last_he = he;
                if (dataGridViewModules.Rows.Count == 0)                // if nothing displayed, display, else ignore subsequence updates
                    Display();
            }
            else if (comboBoxShips.Text == allknownmodulestext || comboBoxShips.Text.ContainsIIC(".loadout") )  // these are not history dependent
            {
                last_he = he;
                if (dataGridViewModules.Rows.Count == 0)                // if nothing displayed, display, else ignore subsequence updates
                    Display();
            }
            else
            {                                                           // discrete ship
                Ship si = DiscoveryForm.History.ShipInformationList.GetShipByNameIdentType(comboBoxShips.Text);      // grab SI of specific ship (may be null)
                update = !Object.ReferenceEquals(si, last_displayship);      // this vs ship
            }

            if (update)      // if stored modules is different..
            {
               // System.Diagnostics.Debug.WriteLine($"Modules recalc {he.EventTimeUTC} vs {last_he?.EventTimeUTC} {comboBoxShips.Text}");
                last_he = he;
                Display();
            }
        }

        #region Display

        private void Display()      // allow redisplay of last data
        {
            DataGridViewColumn sortcolprev = dataGridViewModules.SortedColumn != null ? dataGridViewModules.SortedColumn : dataGridViewModules.Columns[0];
            SortOrder sortorderprev = dataGridViewModules.SortedColumn != null ? dataGridViewModules.SortOrder : SortOrder.Ascending;
            int firstline = dataGridViewModules.SafeFirstDisplayedScrollingRowIndex();

            pbsModuleDisplay.Resize -= PbsModuleDisplay_Resize;

            var sortstate = dataGridViewModules.GetSort();

            dataGridViewModules.Rows.Clear();

            Refresh();

            dataViewScrollerPanel.SuspendLayout();

            dataGridViewModules.ContextMenuStrip = null;

            last_displayship = null;     // no ship info
            last_moduleshipproperties = null; // no module ship props

            allmodulesref.Clear();      // no ref to all modules info

            SetColHeaders(null, null, null, null, null, null, null, null);        //default
            sortmodecol = null; 

            Value.Visible = SlotCol.Visible = PriorityEnable.Visible = BluePrint.Visible = true;

            if (comboBoxShips.Text == storedmoduletext)
            {
                HideShipRelatedButtonsAndPanelsClearModuleDiagram();

                if (last_he?.StoredModules != null)
                {
                    SetColHeaders(null, null, "System".Tx(), "Tx Time".Tx(), 
                                    null, null, "Cost".Tx(), "");

                    ShipModulesInStore mi = last_he.StoredModules;
                    labelVehicle.Text = "";
                    sortmodecol = "AAATNANA";

                    foreach (ShipModulesInStore.StoredModule sm in mi.StoredModules)
                    {
                        object[] rowobj = {
                                sm.NameFD.GetForeignModuleType(),
                                sm.NameFD.GetForeignModuleName(sm.Name_Localised),
                                sm.StarSystem.Alt("In Transit".Tx()), 
                                sm.TransferTimeString ,
                                sm.Mass > 0 ? (sm.Mass.ToString()+"t") : "",
                                sm.EngineerModifications?.NameAndLevel() ?? "",
                                sm.TransferCost>0 ? sm.TransferCost.ToString("N0") : "",
                                "" };
                        dataGridViewModules.Rows.Add(rowobj);
                    }
                }
            }
            else if (comboBoxShips.Text == allmodulestext)
            {
                HideShipRelatedButtonsAndPanelsClearModuleDiagram();

                sortmodecol = "AASANANA";           // default is alpha, alpha, slot (via TAG), Alpha Num Alpha Num, Alpha

                ShipList shm = DiscoveryForm.History.ShipInformationList;

                foreach (var kvp in shm.OwnedSpaceShips())
                {
                    foreach (var key in kvp.Value.Modules.Keys)
                    {
                        ShipModule sm = kvp.Value.Modules[key];
                        AddModuleLine(sm, kvp.Value);
                    }
                    allmodulesref.Add(kvp.Value);      // we add ref in effect to the list of modules we extracted info from - this is used to see if they changed during the update abovevi
                }

                foreach (ShipModulesInStore.StoredModule sm in shm.StoredModules.StoredModules)
                {
                    string info = sm.StarSystem.Alt("In Transit".Tx());
                    info = info.AppendPrePad(sm.TransferTimeString, ":");
                    object[] rowobj = {
                                sm.NameFD.GetForeignModuleType(),
                                sm.NameFD.GetForeignModuleName(sm.Name_Localised),
                                "Stored".Tx(),
                                 info ,
                                sm.Mass > 0 ? (sm.Mass.ToString()+"t") : "",
                                sm.EngineerModifications?.NameAndLevel() ?? "",
                                sm.TransferCost>0 ? sm.TransferCost.ToString("N0") : "",
                                "" };
                    dataGridViewModules.Rows.Add(rowobj);
                    allmodulesref.Add(sm);
                }
            }
            else if (comboBoxShips.Text == allknownmodulestext)
            {
                HideShipRelatedButtonsAndPanelsClearModuleDiagram();

                Value.Visible = SlotCol.Visible = PriorityEnable.Visible = BluePrint.Visible = false;
                sortmodecol = "AAAANAAA";

                var modules = ItemData.GetShipModules(true, true, true, true, true, compressarmourtosidewinderonly: false);
                foreach (var kvp in modules)
                {
                    ItemData.ShipModule sm = kvp.Value;
                    object[] rowobj = {
                                    sm.TranslatedModTypeString(),
                                    sm.TranslatedModName,
                                    "",
                                    sm.ToString(Environment.NewLine),
                                    sm.Mass > 0 ? (sm.Mass.ToString()+"t") : "",
                                    "",
                                    "",
                                    "",
                                };

                    var rw = dataGridViewModules.Rows.Add(rowobj);
                }
            }
            else if ( comboBoxShips.Text == allshipstext)
            {
                HideShipRelatedButtonsAndPanelsClearModuleDiagram(false, false, "Click on a ship to display its modules");

                SetColHeaders("", "Type".Tx(), "Manufacturer".Tx(), "Speed".Tx(),
                                null, "Class".Tx(), null, "Info".Tx());
                sortmodecol = "PAANNANA";           // P = sort on column 1 fixed

                foreach (ItemData.ShipProperties shipproperties in ItemData.GetSpaceships())
                {
                    var rw = dataGridViewModules.RowTemplate.Clone() as DataGridViewRow;           // need to add like this due to different types of cells
                    var pcb = new DataGridViewPictureBoxCell();
                    rw.Cells.Add(pcb);
                    rw.AddTextCells(7);
                    Image img = ItemData.GetShipImage(shipproperties.FDID);
                    pcb.Tag = img;      // directing the hover over to the image
                    pcb.PictureBox.AddImage(new Rectangle(8, 8, 128, 128), img);
                    pcb.PictureBox.Render(minsize: new Size(128 + 8 + 8, 128 + 8 + 8));

                    rw.Cells[1].Value = shipproperties.Name;
                    rw.Cells[2].Value = shipproperties.Manufacturer;
                    rw.Cells[3].Value = shipproperties.Speed.ToString("N0") + " / " + shipproperties.Boost.ToString("N0");
                    rw.Cells[4].Value = shipproperties.HullMass.ToString("N0");
                    rw.Cells[5].Value = shipproperties.ClassString;
                    rw.Cells[6].Value = shipproperties.HullCost.ToString("N0");
                    rw.Cells[7].Value = "S: " + shipproperties.Shields.ToString("N0") + Environment.NewLine +
                                        "A: " + shipproperties.Armour.ToString("N0") + Environment.NewLine +
                                        "Crew: " + shipproperties.Crew.ToString("N0");

                    rw.Tag = shipproperties;
                    dataGridViewModules.Rows.Add(rw);
                }

            }
            else if (comboBoxShips.Text == allownedshipstext || comboBoxShips.Text == currentownedshipstext)
            {
                bool currentships = comboBoxShips.Text == currentownedshipstext;

                HideShipRelatedButtonsAndPanelsClearModuleDiagram(false, false, "Click on a ship to display its modules");

                splitContainerModulesGrid.Panel1Collapsed = false;

                SetColHeaders("", "Type".Tx(), "Manufacturer".Tx(), "Name".Tx(), "Ident".Tx(), "Mass".Tx(), "Location".Tx(), "Info".Tx(), "Date Bought", currentships ? null : "Date Sold/Destroyed");
                sortmodecol = "PAAAANANDD";      // P = sort on column 1 fixed

                dataGridViewModules.ContextMenuStrip = contextMenuStripShipList;

                foreach (var kvp in currentships ? DiscoveryForm.History.ShipInformationList.OwnedSpaceShips() : DiscoveryForm.History.ShipInformationList.SpaceShips() )
                {
                    var ship = kvp.Value;
                    var rw = dataGridViewModules.RowTemplate.Clone() as DataGridViewRow;           // need to add like this due to different types of cells
                    var pcb = new DataGridViewPictureBoxCell();
                    rw.Cells.Add(pcb);
                    rw.AddTextCells(9);
                    Image img = ItemData.GetShipImage(ship.ShipFD);
                    pcb.Tag = img;      // directing the hover over to the image
                    pcb.PictureBox.AddImage(new Rectangle(8, 8, 128, 128), img);
                    pcb.PictureBox.Render(minsize: new Size(128 + 8 + 8, 128 + 8 + 8));

                    rw.Cells[1].Value = ship.ShipType;
                    rw.Cells[2].Value = ship.GetShipProperties()?.Manufacturer ?? "Unknown ship";       // ship may be unknown to us
                    rw.Cells[3].Value = ship.ShipUserName;
                    string id = ship.ID.ToString() + ShipList.ReuseMarkerIndex(kvp.Key, " / ");

                    rw.Cells[4].Value = ship.ShipUserIdent.HasChars() ? ship.ShipUserIdent + $" ({id})" : $"ID: {id}";
                    rw.Cells[5].Value = "T: " + (ship.HullMass()+ship.ModuleMass()).ToString("N0") + Environment.NewLine +
                                        "H: " + ship.HullMass().ToString("N0") + Environment.NewLine + 
                                        "M: " + ship.ModuleMass().ToString("N0");
                    rw.Cells[6].Value = ship.InTransit ? "Transit" : ship.StoredAtSystem != null ? (ship.StoredAtSystem + (ship.StoredAtStation != null ? (" " + ship.StoredAtStation) : "")) : "";

                    string info = ship.HullValue > 0 || ship.ModulesValue > 0 ?
                                            "T: " + (ship.HullValue + ship.ModulesValue).ToString("N0") + Environment.NewLine +
                                            "H: " + ship.HullValue.ToString("N0") + Environment.NewLine +
                                            "M: " + ship.ModulesValue.ToString("N0") 
                                            : "";

                    double? lyrange = ship.GetJumpRange(0);
                    if (lyrange.HasValue)
                        info = info.AppendPrePad($"LY: {lyrange:N2}", Environment.NewLine);
                    rw.Cells[7].Value = info;
                                            
                    rw.Cells[8].Value = ship.CreateEvent != null ? EDDConfig.Instance.ConvertTimeToSelectedFromUTC(ship.CreateEvent.EventTimeUTC).ToString("dd/MM/yyyy HH:mm:ss") : "-";
                    if ( !currentships )
                        rw.Cells[9].Value = ship.SoldDestroyedEvent != null ? EDDConfig.Instance.ConvertTimeToSelectedFromUTC(ship.SoldDestroyedEvent.EventTimeUTC).ToString("dd/MM/yyyy HH:mm:ss") : "-";

                    rw.Tag = ship;                      // record for double click
                    dataGridViewModules.Rows.Add(rw);
                }

                dataGridViewModules.Sort(sortstate);
            }
            else if (comboBoxShips.Text.ContainsIIC(".loadout"))
            {
                string loadoutfile = BaseUtils.FileHelpers.TryReadAllTextFromFile(System.IO.Path.Combine(EDDOptions.Instance.ShipLoadoutsDirectory(), comboBoxShips.Text));
                if (loadoutfile.HasChars())
                {
                    Ship si = Ship.CreateFromLoadout(loadoutfile);
                    if (si != null)
                    {
                        DisplayShip(si, true);
                    }
                }
            }
            else if (comboBoxShips.Text == travelhistorytext || comboBoxShips.Text.Length == 0)  // second is due to the order History gets called vs this on start.  Current ship at travel history
            {
                if (last_he?.ShipInformation != null)
                {
                    last_cargo = DiscoveryForm.History.MaterialCommoditiesMicroResources.CargoCount(last_he.MaterialCommodity);
                    last_he.ShipInformation.UpdateFuelWarningPercent();      // ensure its fresh from the DB
                    DisplayShip(last_he.ShipInformation, false);
                }
            }
            else
            {
                Ship si = DiscoveryForm.History.ShipInformationList.GetShipByNameIdentType(comboBoxShips.Text);
                if (si != null)
                {
                    last_cargo = 0;                     // presume empty cargo
                    si.UpdateFuelWarningPercent();      // ensure its fresh from the DB
                    DisplayShip(si, false);
                }
            }

            dataViewScrollerPanel.ResumeLayout();

            dataGridViewModules.Sort(sortcolprev, (sortorderprev == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
            dataGridViewModules.Columns[sortcolprev.Index].HeaderCell.SortGlyphDirection = sortorderprev;
            dataGridViewModules.ClearSelection();
            if (firstline >= 0 && firstline < dataGridViewModules.RowCount)
                dataGridViewModules.SafeFirstDisplayedScrollingRowIndex(firstline);
        }

        // call to update the grid and the ship data panel
        private void DisplayShip(Ship shipinstance, bool displaydeleteloadoutbutton)
        {
            sortmodecol = "AASANANA";           // default is alpha, alpha, slot (via TAG), Alpha Num Alpha Num, Alpha
            last_displayship = shipinstance;

            DisplayShipStats(shipinstance);
            DisplayModuleDiagram(shipinstance.GetShipProperties(),shipinstance, true);
            foreach (var key in shipinstance.Modules.Keys)
            {
                ShipModule sm = shipinstance.Modules[key];
                AddModuleLine(sm);
            }

            labelVehicle.Text = shipinstance.ShipFullInfo(cargo: false, fuel: false, manu: true);

            buttonExtConfigure.Visible = shipinstance.State == Ship.ShipState.Owned;
            buttonExtCoriolis.Visible = buttonExtEDShipyard.Visible = shipinstance.CheckMinimumModulesForCoriolisEDSY();          //ORDER is important due to flow control panel
            extButtonLoadLoadout.Visible = true;
            extPanelRollUpStats.Visible = shipinstance.ShipFD.Type == VehicleFDName.VehicleType.Ship;
            labelVehicle.Visible = true;
            extButtonSaveLoadout.Visible = true;
            extButtonDeleteLoadout.Visible = displaydeleteloadoutbutton;
            splitContainerModulesGrid.Panel1Collapsed = false;
        }


        private void DisplayModuleDiagram(ItemData.ShipProperties shipproperties, Ship shipinstance, bool clickbacks)
        {
            pbsModuleDisplay.Resize -= PbsModuleDisplay_Resize;
            extPictureBoxModules.ClearImageList();
            if (shipproperties != null)       // we may not know the ship
            {
                var images = smd.CreateImages(shipproperties, shipinstance, new Point(0, 0), extPictureBoxModules.Width, null, clickbacks, clickbacks);
                extPictureBoxModules.AddRange(images);
                last_moduleshipproperties = shipproperties; // keep a record of this for resize
                last_moduleclickbacks = clickbacks;
            }
            pbsModuleDisplay.Render();
            pbsModuleDisplay.Resize += PbsModuleDisplay_Resize;
        }

        private void PbsModuleDisplay_Resize(object sender, EventArgs e)
        {
            if ( last_moduleshipproperties!=null)
            {
//                System.Diagnostics.Debug.WriteLine($"PBS Module redisplay {pbsModuleDisplay.Size}");
                DisplayModuleDiagram(last_moduleshipproperties, last_displayship, last_moduleclickbacks);
            }
        }


        // call to update the ship data panel
        private void DisplayShipStats(Ship si)
        {
            var stats = si?.GetShipStats(multiPipControlSys.Value, multiPipControlEng.Value, multiPipControlWep.Value, last_cargo, si.FuelLevel, si.ReserveFuelLevel);                  // may be null

            labelDataArmour.Data = stats?.ArmourRaw.HasValue ?? false ? new object[] {
                                stats.ArmourRaw,
                                stats.ArmourKineticPercentage, stats.ArmourKineticValue,
                                stats.ArmourThermalPercentage, stats.ArmourThermalValue,
                                stats.ArmourExplosivePercentage, stats.ArmourExplosiveValue,
                                stats.ArmourCausticPercentage, stats.ArmourCausticValue
            } : null;

            labelDataShields.Data = stats?.ShieldsRaw.HasValue ?? false ? new object[] {
                            stats.ShieldsRaw,
                            stats.ShieldsSystemPercentage, stats.ShieldsSystemValue,
                            stats.ShieldsKineticPercentage, stats.ShieldsKineticValue,
                            stats.ShieldsThermalPercentage, stats.ShieldsThermalValue,
                            stats.ShieldsExplosivePercentage, stats.ShieldsExplosiveValue
            } : null;

            string transit = si.InTransit ? (si.StoredAtSystem ?? "Unknown".Tx()) + ":" + (si.StoredAtStation ?? "Unknown".Tx()) : null;
            string storedat = si.StoredAtSystem != null ? (si.StoredAtSystem + ":" + (si.StoredAtStation ?? "Unknown".Tx())) : null;

            labelDataFSD.Data = stats?.FSDCurrentRange.HasValue ?? false ? new object[] {
                    stats.FSDCurrentRange, stats.FSDCurrentMaxRange, stats.FSDLadenRange, stats.FSDUnladenRange, stats.FSDMaxRange, stats.FSDMaxFuelPerJump,
                                    si.FuelLevel, si.FuelCapacity, si.ReserveFuelLevel, si.ReserveFuelCapacity, transit, storedat
            } : null;

            //Raw {0.#} Abs {0.#|%} Kin {0.#|%} Thm {0.#|%} Exp {0.#|%} AX {0.#|%} Dur {0.#|s} DurMax {0.#|s} Ammo {0.#|s} Cur {0.#|%} Max {0.#|%}

            labelDataWep.Data = stats?.ValidWeaponData ?? false ? new object[] { 
                        stats.WeaponRaw.Value,
                        stats.WeaponAbsolutePercentage, stats.WeaponKineticPercentage, stats.WeaponThermalPercentage,stats.WeaponExplosivePercentage, stats.WeaponAXPercentage,
                        stats.WeaponDuration, stats.WeaponDurationMax, stats.WeaponAmmoDuration, stats.WeaponCurSus, stats.WeaponMaxSus,
            } : null;

            labelDataThrust.Data = stats?.CurrentSpeed.HasValue ?? false ? new object[]  {
                                        stats.CurrentSpeed, stats.CurrentBoost,
                        stats.LadenSpeed, stats.LadenBoost,
                        stats.UnladenSpeed, stats.UnladenBoost,
                        stats.MaxSpeed, stats.MaxBoost,
                        stats.CurrentBoostFrequency, stats.MaxBoostFrequency
            } : null;

            double hullmass = si.HullMass();
            double modulemass = si.ModuleMass();
            //System.Diagnostics.Debug.WriteLine($"Stats Hull Mass {hullmass} Module {modulemass}");
            double? warningpercent = si.FuelWarningPercent > 0 ? si.FuelWarningPercent : default(double?);
            warningpercent = 20;

            labelDataMass.Data = new object[] { hullmass + modulemass + si.FuelLevel + last_cargo + si.ReserveFuelLevel,
                            hullmass, modulemass, hullmass + modulemass, last_cargo, si.CalculateCargoCapacity(), warningpercent};

            labelDataCost.Data = new object[] { si.HullValue, si.ModulesValue, si.HullValue + si.ModulesValue, si.Rebuy };

            if (stats != null && stats.PowerPlant != null)
            {
                double u0 = stats.PowerDrawCorePrio[0] + stats.PowerDrawWeaponsPrio[0];
                double u1= stats.PowerDrawCorePrio[1] + stats.PowerDrawWeaponsPrio[1];
                double u2 = stats.PowerDrawCorePrio[2] + stats.PowerDrawWeaponsPrio[2];
                double u3 = stats.PowerDrawCorePrio[3] + stats.PowerDrawWeaponsPrio[3];
                double u4 = stats.PowerDrawCorePrio[4] + stats.PowerDrawWeaponsPrio[4];
                double p0 = 100 * u0 / stats.PowerPlant.Value;
                double p1 = 100 * u1 / stats.PowerPlant.Value;
                double p2 = 100 * u2 / stats.PowerPlant.Value;
                double p3 = 100 * u3 / stats.PowerPlant.Value;
                double p4 = 100 * u4 / stats.PowerPlant.Value;

                labelDataPower.Data = new object[] { 
                    stats.PowerPlant,
                    stats.PowerDrawCore,  100.0 *stats.PowerDrawCore / stats.PowerPlant,
                    stats.PowerDrawWeapons , 100.0 *stats.PowerDrawWeapons / stats.PowerPlant,
                    stats.PowerDrawTotal,  100.0 *stats.PowerDrawTotal / stats.PowerPlant,
                    u0,  100.0 *u0 / stats.PowerPlant,
                    u0+u1,  100.0 *(u0+u1) / stats.PowerPlant,
                    u0+u1+u2,  100.0 *(u0+u1+u2) / stats.PowerPlant,
                };

                extProgressBarCoreWeapons.Value = (int)(100.0 * stats.PowerDrawTotal / stats.PowerPlant);
                extProgressBarCoreWeapons.Marker1 = (int)(100.0 * stats.PowerDrawCore / stats.PowerPlant);
                extProgressBarMSPriorities.SegmentValues = new double[] { p0, p1, p2, p3, p4 };
            }
            else
            {
                labelDataPower.Data = null;
                extProgressBarCoreWeapons.Value = 0;
                extProgressBarCoreWeapons.Marker1 = -1;
                extProgressBarMSPriorities.SegmentValues = null;
            }

        }

        void AddModuleLine(ShipModule sm , Ship onship = null)
        {
            string infoentry = "";

            if (onship != null)
                infoentry = onship.ShipNameIdentType;

            if (sm.AmmoHopper.HasValue)
            {
                infoentry = infoentry.AppendPrePad($"Current Hopper: {sm.AmmoHopper.Value.ToString()}", ", ");
                if (sm.AmmoClip.HasValue)
                    infoentry += "/" + sm.AmmoClip.ToString();
            }

            if (displayfilters.Contains("engineeredvalues"))
            {
                var engmod = sm.GetModuleEngineered(out string _);
                if (engmod != null) // may not have enough details to find module
                {
                    infoentry = infoentry.AppendPrePad(engmod.ToString(" " + Environment.NewLine), Environment.NewLine);
                }
            }

            string value = (sm.Value.HasValue && sm.Value.Value > 0) ? sm.Value.Value.ToString("N0") : "";

            string blueprintcol = "";
            string engtooltip = null;

            if (sm.Engineering != null)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder(1024);
                sb.Append(sm.Engineering.FriendlyBlueprintName);
                sb.AppendColonS();
                sb.Append(sm.Engineering.Level.ToStringInvariant());
                if (sm.Engineering.ExperimentalEffect_Localised.HasChars())
                {
                    sb.AppendColonS();
                    sb.Append(sm.Engineering.ExperimentalEffect_Localised);
                }

                blueprintcol = sb.ToString();

                System.Text.StringBuilder sbtt = new System.Text.StringBuilder(1024);
                sm.Engineering.Build(sbtt);
                engtooltip = sbtt.ToString();

                if (displayfilters.Contains("fullblueprint"))
                {
                    blueprintcol = engtooltip;
                }
            }

            object[] rowobj = {
                                sm.ItemFD.GetForeignModuleType(),
                                sm.ItemFD.GetForeignModuleName(sm.LocalisedItem),
                                ShipSlots.ToLocalisedLanguage(sm.SlotFD),
                                infoentry,
                                sm.Mass() > 0 ? (sm.Mass().ToString("0.#")+"t") : "",                                
                                blueprintcol,
                                value, 
                                sm.PE() };

            var row = dataGridViewModules.Rows.Add(rowobj);
            var rw = dataGridViewModules.Rows[row];

            rw.Cells[3].ToolTipText = infoentry;
            rw.Cells[2].Tag = sm.SlotFD;                // Used by sort S, and used by module diagram element click to find slot

            if (engtooltip != null)
            {
                dataGridViewModules.Rows[row].Cells[5].ToolTipText = engtooltip;
            }

         //   System.Diagnostics.Debug.WriteLine($"Add Module {sm.ItemFD.Str()} {sm.SlotFD} {sm.LocalisedItem}");
        }

        void SetColHeaders(params string[] list)
        {
            ItemLocalised.HeaderText = list[0] ?? "Type".Tx();
            ItemCol.HeaderText = list[1] ?? "Item".Tx();
            SlotCol.HeaderText = list[2] ?? "Slot".Tx();
            ItemInfo.HeaderText = list[3] ?? "Info".Tx();
            Mass.HeaderText = list[4] ?? "Mass".Tx();
            BluePrint.HeaderText = list[5] ?? "BluePrint".Tx();
            Value.HeaderText = list[6] ?? "Value".Tx();
            PriorityEnable.HeaderText = list[7] ?? "P/E".Tx();
            ColO1.Visible = list.Length > 8;
            ColO1.HeaderText = list.Length>8 ? list[8] : "";
            ColO2.Visible = list.Length > 9 && list[9]!=null;
            ColO2.HeaderText = list.Length>9 ? list[9] : "";
        }

        private void HideShipRelatedButtonsAndPanelsClearModuleDiagram(bool showcontrol = true, bool collapsemodule = true, string moduletexthelper = null)
        {
            splitContainerModulesGrid.Panel1Collapsed = collapsemodule;
            extPanelRollUpStats.Visible = false;
            extButtonShowControl.Visible = showcontrol;
            extButtonSaveLoadout.Visible = extButtonDeleteLoadout.Visible = extButtonLoadLoadout.Visible =
            labelVehicle.Visible = buttonExtCoriolis.Visible = buttonExtEDShipyard.Visible = buttonExtConfigure.Visible = false;
            extPictureBoxModules.ClearImageList();
            if ( moduletexthelper!=null)
            {
                extPictureBoxModules.AddTextAutoSize(new Point(4, 10), new Size(10000, 10000), moduletexthelper , this.Font, Theme.Current.TextBlockForeColor, Theme.Current.Form, 1.0f);
                pbsModuleDisplay.Render();
            }
            pbsModuleDisplay.Render();
        }

        #endregion

        #region Sort

        private void dataGridViewModules_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (sortmodecol.HasChars())
            {
                var sort = sortmodecol[e.Column.Index];
                if (sort == 'P')     // sort on column 1, not this column
                {
                    var left = dataGridViewModules[1, e.RowIndex1].Value.ToString();
                    var right = dataGridViewModules[1, e.RowIndex2].Value.ToString();
                    e.SortResult = left.CompareTo(right);
                    e.Handled = true;
                }
                else if (sort == 'N')
                {
                    e.SortDataGridViewColumnNumeric(removetext: "t", striptonumeric: true);
                }
                else if (sort == 'S')
                {
                    var tag1 = dataGridViewModules[e.Column.Index, e.RowIndex1].Tag;
                    var tag2 = dataGridViewModules[e.Column.Index, e.RowIndex2].Tag;
                    if (tag1 != null)
                    {
                        if (tag2 != null)
                            e.SortResult = ((ShipSlots.Slot)tag1).CompareTo((ShipSlots.Slot)tag2);
                        else
                            e.SortResult = -1;
                    }
                    else
                        e.SortResult = 1;
                    e.Handled = true;
                }
                else if (sort == 'T')
                {
                    if (e.CellValue1 != null && TimeSpan.TryParse(e.CellValue1 as string, out TimeSpan l))
                    {
                        if (e.CellValue2 != null && TimeSpan.TryParse(e.CellValue2 as string, out TimeSpan r))
                        {
                            e.SortResult = l.CompareTo(r);
                        }
                        else
                            e.SortResult = -1;
                    }
                    else
                        e.SortResult = 1;
                    e.Handled = true;
                }
                else if (sort == 'D')
                {
                    e.SortDataGridViewColumnDate();
                }
                else if (sort == 'A')
                {       // default
                }
                else
                    System.Diagnostics.Debug.Assert(false, "Bad sort mode");
            }
        }

        #endregion


        #region Hover over
        void HoverOverCell(DataGridViewCell cell, Rectangle area, Point screenpos)
        {
            ulong curtime = (ulong)Environment.TickCount;

            if (cell.Tag is Image && cell.ColumnIndex == 0 )
            {
                if (popupform != null)
                    popupform.Close();
                screenpos.Offset(8, 8);
                popupform = new PopUpForm(screenpos, new Size(400, 400), 750);
                var imgctrl = new ImageControl();
                imgctrl.Dock = DockStyle.Fill;
                imgctrl.SetDrawImage(cell.Tag as Image, new Rectangle(0, 0, 400, 400));
                popupform.ContentPanel.Controls.Add(imgctrl);

                popupform.Show(this);
            }
        }

        PopUpForm popupform = null;

        #endregion


        #region Vars

        private string currentownedshipstext;
        private string allownedshipstext;
        private string storedmoduletext;
        private string travelhistorytext;
        private string allmodulestext;
        private string allshipstext;
        private string allknownmodulestext;

        private string sortmodecol = "";

        private HistoryEntry last_he = null;
        private Ship last_displayship = null;
        private int last_cargo = 0;
        private ItemData.ShipProperties last_moduleshipproperties;
        private bool last_moduleclickbacks;
        private string dbDisplayFilters = "DisplayFiltersNew";
        private string dbWordWrap = "WordWrap";
        private string dbShipSelect = "ShipSelect";
        private string dbModSplitter = "ModSplitter";
        private string[] displayfilters;
        private List<object> allmodulesref = new List<object>();
        ShipModuleDisplay smd = new ShipModuleDisplay();

        #endregion

    }
}
