/*
 * Copyright 2016 - 2025 EDDiscovery development team
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
using EliteDangerousCore.StarScan2;
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
    public partial class UserControlModules : UserControlCommonBase
    {
        private string ownedshipstext;
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

        #region Init

        public UserControlModules()
        {
            InitializeComponent();
            BaseUtils.TranslatorMkII.Instance.TranslateControls(this);
            BaseUtils.TranslatorMkII.Instance.TranslateTooltip(toolTip, this);
            DBBaseName = "ModulesGrid";
        }

        protected override void Init()
        {

            ownedshipstext = "Owned Ships".Tx();
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

            HideShipRelatedButtonsAndPanels();

            dataGridViewModules.EnableCellHoverOverCallback();
            dataGridViewModules.HoverOverCell += HoverOverCell;

            DiscoveryForm_OnThemeChanged();

            extPictureBoxModules.ClickElement += ModuleDisplayClickElement;

            splitContainer.SplitterDistance(GetSetting(dbModSplitter, 0.4));
        }

        protected override void LoadLayout()
        {
            dataGridViewModules.RowTemplate.MinimumHeight = Font.ScalePixels(26);
            DGVLoadColumnLayout(dataGridViewModules);
        }

        protected override void Closing()
        {
            if ( comboBoxShips.Text != allknownmodulestext)     // we fiddle with the columns in this view, so don't save
                DGVSaveColumnLayout(dataGridViewModules);

            PutSetting(dbModSplitter, splitContainer.GetSplitterDistance());

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

            bool update = false;

            if (comboBoxShips.Text == storedmoduletext)         // stored at he
            {
                update = !Object.ReferenceEquals(he.StoredModules, last_he?.StoredModules);
            }
            else if (comboBoxShips.Text == allmodulestext)      // this displays the stored modules, as well as all other ship modules, at top of history
            {
                ShipList shm = DiscoveryForm.History.ShipInformationList;
                var ownedships = (from x1 in shm.Ships where x1.Value.State == Ship.ShipState.Owned && ItemData.IsShip(x1.Value.ShipFD) select x1.Value);

                List<object> curref = new List<object>();       // repeat the objects used in the display
                foreach (var si in ownedships)  
                    curref.Add(si);
                foreach (ShipModulesInStore.StoredModule sm in shm.StoredModules.StoredModules)
                    curref.Add(sm);

                update = !allmodulesref.ReferenceEquals(curref);        // if not identical, something has changed, execute update
            }
            else if (comboBoxShips.Text == ownedshipstext)
            {
                update = true;      // tbd optimise we need a way of knowing ship info has changed
            }
            else if (comboBoxShips.Text == allshipstext)
            {
                last_he = he;
                if (dataGridViewModules.Rows.Count == 0)                // if nothing displayed, display, else ignore subsequence updates
                    Display();
            }
            else if (comboBoxShips.Text == travelhistorytext)           // travel history, it should the the current si vs the last displayed si
            {
                update = !Object.ReferenceEquals(he.ShipInformation, last_displayship);      
            }
            else if (comboBoxShips.Text == allknownmodulestext ||
                     comboBoxShips.Text.ContainsIIC(".loadout")
                     )
            {
                last_he = he;
                if (dataGridViewModules.Rows.Count == 0)                // if nothing displayed, display, else ignore subsequence updates
                    Display();
            }
            else
            {
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

            dataGridViewModules.Rows.Clear();

            Refresh();

            dataViewScrollerPanel.SuspendLayout();

            last_displayship = null;     // no ship info
            last_moduleshipproperties = null; // no module ship props

            allmodulesref.Clear();      // no ref to all modules info

            SetColHeaders(null,null,null,null, null,null,null,null);        //default
            sortmodecol = null; 

            Value.Visible = SlotCol.Visible = PriorityEnable.Visible = BluePrint.Visible = true;

            if (comboBoxShips.Text == storedmoduletext)
            {
                HideShipRelatedButtonsAndPanels();

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
                                JournalFieldNaming.GetForeignModuleType(sm.NameFD),
                                JournalFieldNaming.GetForeignModuleName(sm.NameFD,sm.Name_Localised),
                                sm.StarSystem.Alt("In Transit".Tx()), 
                                sm.TransferTimeString ,
                                sm.Mass > 0 ? (sm.Mass.ToString()+"t") : "",
                                sm.EngineerModifications.Alt(""),
                                sm.TransferCost>0 ? sm.TransferCost.ToString("N0") : "",
                                "" };
                        dataGridViewModules.Rows.Add(rowobj);
                    }
                }
            }
            else if (comboBoxShips.Text == allmodulestext)
            {
                HideShipRelatedButtonsAndPanels();

                sortmodecol = "AASANANA";           // default is alpha, alpha, slot (via TAG), Alpha Num Alpha Num, Alpha

                ShipList shm = DiscoveryForm.History.ShipInformationList;
                var ownedships = (from x1 in shm.Ships where x1.Value.State == Ship.ShipState.Owned && ItemData.IsShip(x1.Value.ShipFD) select x1.Value);

                foreach (var si in ownedships)
                {
                    foreach (var key in si.Modules.Keys)
                    {
                        ShipModule sm = si.Modules[key];
                        AddModuleLine(sm, si);
                    }
                    allmodulesref.Add(si);      // we add ref in effect to the list of modules we extracted info from - this is used to see if they changed during the update abovevi
                }

                foreach (ShipModulesInStore.StoredModule sm in shm.StoredModules.StoredModules)
                {
                    string info = sm.StarSystem.Alt("In Transit".Tx());
                    info = info.AppendPrePad(sm.TransferTimeString, ":");
                    object[] rowobj = {
                                JournalFieldNaming.GetForeignModuleType(sm.NameFD),
                                JournalFieldNaming.GetForeignModuleName(sm.NameFD,sm.Name_Localised),
                                "Stored".Tx(),
                                 info ,
                                sm.Mass > 0 ? (sm.Mass.ToString()+"t") : "",
                                sm.EngineerModifications.Alt(""),
                                sm.TransferCost>0 ? sm.TransferCost.ToString("N0") : "",
                                "" };
                    dataGridViewModules.Rows.Add(rowobj);
                    allmodulesref.Add(sm);
                }
            }
            else if (comboBoxShips.Text == allknownmodulestext)
            {
                HideShipRelatedButtonsAndPanels();

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
                HideShipRelatedButtonsAndPanels(false);
                splitContainer.Panel1Collapsed = false;

                SetColHeaders("", "Type".Tx(), "Manufacturer".Tx(), "Speed".Tx(),
                                null, "Class".Tx(), null, "Info".Tx());
                sortmodecol = "PAANNANA";           // P = sort on column 1 fixed

                foreach(ItemData.ShipProperties shipproperties in ItemData.GetSpaceships())
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
            else if (comboBoxShips.Text == ownedshipstext)
            {
                HideShipRelatedButtonsAndPanels(false);
                splitContainer.Panel1Collapsed = false;

                ShipList shm = DiscoveryForm.History.ShipInformationList;
                var ownedships = (from x1 in shm.Ships where x1.Value.State == Ship.ShipState.Owned && ItemData.IsShip(x1.Value.ShipFD) select x1.Value);

                SetColHeaders("", "Type".Tx(), "Manufacturer".Tx(), "Name".Tx(), "Ident".Tx(), "Mass".Tx(), "Location".Tx(), "Cost".Tx());
                sortmodecol = "PAAAANAN";      // P = sort on column 1 fixed

                foreach (var ship in ownedships)
                {
                    var rw = dataGridViewModules.RowTemplate.Clone() as DataGridViewRow;           // need to add like this due to different types of cells
                    var pcb = new DataGridViewPictureBoxCell();
                    rw.Cells.Add(pcb);
                    rw.AddTextCells(7);
                    Image img = ItemData.GetShipImage(ship.ShipFD);
                    pcb.Tag = img;      // directing the hover over to the image
                    pcb.PictureBox.AddImage(new Rectangle(8, 8, 128, 128), img);
                    pcb.PictureBox.Render(minsize: new Size(128 + 8 + 8, 128 + 8 + 8));

                   
                    rw.Cells[1].Value = ship.ShipType;
                    rw.Cells[2].Value = ship.GetShipProperties()?.Manufacturer ?? "Unknown ship";       // ship may be unknown to us
                    rw.Cells[3].Value = ship.ShipUserName;
                    rw.Cells[4].Value = ship.ShipUserIdent;
                    rw.Cells[5].Value = "T: " + (ship.HullMass()+ship.ModuleMass()).ToString("N0") + Environment.NewLine +
                                        "H: " + ship.HullMass().ToString("N0") + Environment.NewLine + 
                                        "M: " + ship.ModuleMass().ToString("N0");
                    rw.Cells[6].Value = ship.InTransit ? "Transit" : ship.StoredAtSystem != null ? (ship.StoredAtSystem + (ship.StoredAtStation != null ? (" " + ship.StoredAtStation) : "")) : "";
                    rw.Cells[7].Value = ship.HullValue > 0 || ship.ModulesValue > 0 ?
                                            "T: " + (ship.HullValue + ship.ModulesValue).ToString("N0") + Environment.NewLine +
                                            "H: " + ship.HullValue.ToString("N0") + Environment.NewLine + 
                                            "M: " + ship.ModulesValue.ToString("N0") 
                                            : "";

                    rw.Tag = ship;                      // record for double click
                    dataGridViewModules.Rows.Add(rw);
                }
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
            extPanelRollUpStats.Visible = !ItemData.IsSRVOrFighterOrLander(shipinstance.ShipFD);
            labelVehicle.Visible = true;
            extButtonSaveLoadout.Visible = true;
            extButtonDeleteLoadout.Visible = displaydeleteloadoutbutton;
            splitContainer.Panel1Collapsed = false;
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
            double? warningpercent = si.FuelWarningPercent > 0 ? si.FuelWarningPercent : default(double?);
            warningpercent = 20;

            labelDataMass.Data = new object[] { hullmass + modulemass + si.FuelLevel + last_cargo + si.ReserveFuelLevel,
                            hullmass, modulemass, hullmass + modulemass, last_cargo, si.CalculateCargoCapacity(), warningpercent};

            labelDataCost.Data = new object[] { si.HullValue, si.ModulesValue, si.HullValue + si.ModulesValue, si.Rebuy };

            if (stats != null)
            {
                labelDataPower.Data = new object[] { stats.PowerDrawCore,
                    stats.PowerPlant.HasValue ? 100.0 *stats.PowerDrawCore / stats.PowerPlant : null,
                    stats.PowerDrawWeapons ,
                    stats.PowerPlant.HasValue ? 100.0 *stats.PowerDrawWeapons / stats.PowerPlant : null,
                    stats.PowerDrawTotal,
                    stats.PowerPlant.HasValue ? 100.0 *stats.PowerDrawTotal / stats.PowerPlant : null,
                };

                extProgressBar1.Value = stats.PowerPlant != null ? (int)(100.0 * stats.PowerDrawTotal / stats.PowerPlant) : 0;
                extProgressBar1.Marker1 = stats.PowerPlant != null ? (int)(100.0 * stats.PowerDrawCore / stats.PowerPlant) : -1;
            }
            else
            {
                labelDataPower.Data = null;
                extProgressBar1.Value = 0;
                extProgressBar1.Marker1 = -1;
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

            object[] rowobj = { JournalFieldNaming.GetForeignModuleType(sm.ItemFD),
                                JournalFieldNaming.GetForeignModuleName(sm.ItemFD,sm.LocalisedItem),
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

            System.Diagnostics.Debug.WriteLine($"Add Module {sm.ItemFD} {sm.SlotFD} {sm.LocalisedItem}");
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
        }

        private void HideShipRelatedButtonsAndPanels(bool showcontrol = true)
        {
            splitContainer.Panel1Collapsed = true;
            extPanelRollUpStats.Visible = false;
            extButtonShowControl.Visible = showcontrol;
            extButtonSaveLoadout.Visible = extButtonDeleteLoadout.Visible = extButtonLoadLoadout.Visible =
            labelVehicle.Visible = buttonExtCoriolis.Visible = buttonExtEDShipyard.Visible = buttonExtConfigure.Visible = false;
            extPictureBoxModules.ClearImageList();
            pbsModuleDisplay.Render();
        }

#endregion

        #region Word wrap

        private void extCheckBoxWordWrap_Click(object sender, EventArgs e)
        {
            PutSetting(dbWordWrap, extCheckBoxWordWrap.Checked);
            UpdateWordWrap();
        }

        private void UpdateWordWrap()
        {
            dataGridViewModules.SetWordWrap(extCheckBoxWordWrap.Checked);
            dataViewScrollerPanel.UpdateScroll();
        }

        #endregion


        #region UI

        private void UpdateComboBox()
        {
            ShipList shm = DiscoveryForm.History.ShipInformationList;
            string cursel = comboBoxShips.Text;

            comboBoxShips.Items.Clear();
            comboBoxShips.Items.Add(travelhistorytext);
            comboBoxShips.Items.Add(ownedshipstext);
            comboBoxShips.Items.Add(allshipstext);
            comboBoxShips.Items.Add(storedmoduletext);
            comboBoxShips.Items.Add(allmodulestext);
            comboBoxShips.Items.Add(allknownmodulestext);

            IEnumerable<Ship> ownedships = (from x1 in shm.Ships where x1.Value.State == Ship.ShipState.Owned && ItemData.IsShip(x1.Value.ShipFD) select x1.Value);
            IEnumerable<Ship> soldships = (from x1 in shm.Ships where x1.Value.State != Ship.ShipState.Owned && ItemData.IsShip(x1.Value.ShipFD) select x1.Value);
            // withdrawn, appears loadouts no longer written for these. var fightersrvs = (from x1 in shm.Ships where ItemData.IsSRVOrFighter(x1.Value.ShipFD) select x1.Value);

            var now = (from x1 in ownedships where x1.StoredAtSystem == null select x1.ShipNameIdentType);
            comboBoxShips.Items.AddRange(now);

            var stored = (from x1 in ownedships where x1.StoredAtSystem != null select x1.ShipNameIdentType);
            comboBoxShips.Items.AddRange(stored);


            var loadoutfiles = System.IO.Directory.EnumerateFiles(EDDOptions.Instance.ShipLoadoutsDirectory(), "*.loadout", System.IO.SearchOption.TopDirectoryOnly).
                        Select(f => new System.IO.FileInfo(f)).OrderByDescending(p => p.LastWriteTimeUtc).ToArray();
            foreach (var x in loadoutfiles)
                comboBoxShips.Items.Add(x.Name);

            comboBoxShips.Items.AddRange(soldships.Select(x => x.ShipNameIdentType).ToList());

            //comboBoxShips.Items.AddRange(fightersrvs.Select(x => x.ShipNameIdentType).ToList());

            if (cursel == "")
                cursel = GetSetting(dbShipSelect, "");

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
                PutSetting(dbShipSelect, comboBoxShips.Text);
                Display();
            }
        }

        private void extButtonShowControl_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconNewListBoxForm displayfilter = new ExtendedControls.CheckedIconNewListBoxForm();
            displayfilter.AllOrNoneBack = false;

            // not yet as only one item. displayfilter.UC.AddAllNone();
            displayfilter.UC.Add("fullblueprint", "Show full blueprint information".Tx());
            displayfilter.UC.Add("engineeredvalues", "Show Engineered Values".Tx());

            displayfilter.UC.ImageSize = new Size(24, 24);
            displayfilter.SaveSettings = (s, o) =>
            {
                displayfilters = s.Split(';');
                PutSetting(dbDisplayFilters, string.Join(";", displayfilters));
                Display();
            };
            displayfilter.CloseBoundaryRegion = new Size(32, extButtonShowControl.Height);
            displayfilter.Show(string.Join(";", displayfilters), extButtonShowControl, this.FindForm());
        }

        private void extButtonDrawnResetPips_Click(object sender, EventArgs e)
        {
            multiPipControlEng.Value = multiPipControlSys.Value = multiPipControlWep.Value = 4;
        }

        private void buttonExtCoriolis_Click(object sender, EventArgs e)
        {
            if (last_displayship != null)
            {
                string coriolis = last_displayship.JSONCoriolis(out string errstr).ToString();

                if (errstr.Length > 0)
                    ExtendedControls.MessageBoxTheme.Show(FindForm(), errstr + Environment.NewLine + "This is probably a new or powerplay module" + Environment.NewLine + "Report to EDD Team by Github giving the full text above", "Unknown Module Type");

                System.Diagnostics.Debug.WriteLine("Coriolis Export " + last_displayship.JSONCoriolis(out string error).ToString(true));

                string uri = EDDConfig.Instance.CoriolisURL + "data=" + coriolis.URIGZipBase64Escape() + "&bn=" + Uri.EscapeDataString(last_displayship.Name);

                if (!BaseUtils.BrowserInfo.LaunchBrowser(uri))
                {
                    ExtendedControls.InfoForm info = new ExtendedControls.InfoForm();
                    info.Info("Cannot launch browser, use this JSON for manual Coriolis import", FindForm().Icon, coriolis);
                    info.ShowDialog(FindForm());
                }
            }
        }

        private void buttonExtCoriolis_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string url = ExtendedControls.PromptSingleLine.ShowDialog(this.FindForm(), "URL:", EDDConfig.Instance.CoriolisURL,
                            "Enter Coriolis URL".Tx(), this.FindForm().Icon, requireinput:true);
                if (url != null)
                    EDDConfig.Instance.CoriolisURL = url;
            }
        }

        private void buttonExtEDShipyard_Click(object sender, EventArgs e)
        {
            if (last_displayship != null)
            {
                string loadoutjournalline = last_displayship.JSONLoadout(false).ToString();

                string uri = EDDConfig.Instance.EDDShipyardURL + "#/I=" +loadoutjournalline.URIGZipBase64Escape();

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
                string url = ExtendedControls.PromptSingleLine.ShowDialog(this.FindForm(), "URL:", EDDConfig.Instance.EDDShipyardURL, "Enter ED Shipyard URL".Tx(), this.FindForm().Icon, requireinput:true);
                if (url != null)
                    EDDConfig.Instance.EDDShipyardURL = url;
            }
        }

        private void buttonExtConfigure_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Assert(last_displayship != null);           // must be set for this configure button to be visible

            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();

            int width = 430;
            int ctrlleft = 150;

            f.Add(new ExtendedControls.ConfigurableEntryList.Entry("L", typeof(Label), "Fuel Warning".Tx()+": ", new Point(10, 40), new Size(140, 24), ""));
            f.Add(new ExtendedControls.ConfigurableEntryList.Entry("FuelWarning", typeof(ExtendedControls.NumberBoxDouble),
                last_displayship.FuelWarningPercent.ToString(), new Point(ctrlleft, 40), new Size(width - ctrlleft - 20, 24), "Enter fuel warning level in % (0 = off, 1-100%)".Tx())
            { NumberBoxDoubleMinimum = 0, NumberBoxDoubleMaximum = 100, NumberBoxFormat = "0.##" });

            f.Add(new ExtendedControls.ConfigurableEntryList.Entry("Sell", typeof(ExtendedControls.ExtButton), "Force Sell".Tx(), new Point(10, 80), new Size(80, 24),null));

            f.AddOK(new Point(width - 100, 110), "Press to Accept".Tx());
            f.AddCancel(new Point(width - 200, 110), "Press to Cancel".Tx());

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
                        ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "A Value is not valid".Tx(), "Warning".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (controlname == "Cancel" || controlname == "Close" )
                {
                    f.ReturnResult(DialogResult.Cancel);
                }
                else if (controlname == "Sell")
                {
                    if ( ExtendedControls.MessageBoxTheme.Show(FindForm(), "Confirm sell of ship".Tx()+": "+ Environment.NewLine + last_displayship.ShipNameIdentType , "Warning".Tx(), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes ) 
                    {
                        var je = new EliteDangerousCore.JournalEvents.JournalShipyardSell(DateTime.UtcNow, last_displayship.ShipFD, last_displayship.ID, 0, EDCommander.CurrentCmdrID);
                        var jo = je.CreateJSON();
                        je.Add(jo);
                        DiscoveryForm.NewEntry(je);
                    }

                    f.ReturnResult(DialogResult.Cancel);
                }
            };

            DialogResult res = f.ShowDialogCentred(this.FindForm(), this.FindForm().Icon, "Ship Configure".Tx(), closeicon:true);

            if (res == DialogResult.OK)
            {
                last_displayship.FuelWarningPercent = f.GetDouble("FuelWarning").Value;
                Display();
            }
        }

        private void ModuleDisplayClickElement(object sender, MouseEventArgs e, ExtendedControls.ImageElement.Element i, object tag)
        {
            if (i != null)
            {
                if (tag is ShipSlots.Slot ss)
                {
                    foreach (DataGridViewRow rw in dataGridViewModules.Rows)
                    {
                        if (rw.Cells[2].Tag != null && (ShipSlots.Slot)rw.Cells[2].Tag == ss)      // Cells[2] has the tag
                        {
                            dataGridViewModules.SafeFirstDisplayedScrollingRowIndex(rw.Index);
                            dataGridViewModules.ClearSelection();
                            break;
                        }
                    }
                }
                else if ( tag is ShipModule sm)
                {
                    sm.SetEnabled(sm.Enabled != true);
                    Display();
                }
            }

        }

        private void dataGridViewModules_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                Ship shipinstance = dataGridViewModules.Rows[e.RowIndex].Tag as Ship;        // if row tag is ship (ownedshiptext)
                ItemData.ShipProperties ship = dataGridViewModules.Rows[e.RowIndex].Tag as ItemData.ShipProperties;        // if row tag is ship prop (all ship text)
                if (shipinstance != null)
                {
                    DisplayModuleDiagram(shipinstance.GetShipProperties(), shipinstance, false);
                }
                else if (ship != null)
                {
                    DisplayModuleDiagram(ship, null, false);
                }
            }
        }
        private void dataGridViewModules_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                Ship shp = dataGridViewModules.Rows[e.RowIndex].Tag as Ship;        // if row tag is ship (ownedshiptext)
                if (shp != null)     
                {
                    string ident = shp.ShipNameIdentType;
                    comboBoxShips.SelectedItem = ident;
                }
                else
                { 
                    string tt = dataGridViewModules.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText;      
                    if (!string.IsNullOrEmpty(tt))      // if we have a tool tip
                    {
                        Form mainform = FindForm();
                        ExtendedControls.InfoForm frm = new ExtendedControls.InfoForm();
                        frm.Info("Module Information".Tx(), mainform.Icon, tt);
                        frm.Size = new Size(600, 400);
                        frm.StartPosition = FormStartPosition.CenterParent;
                        frm.Show(mainform);
                    }
                }
            }
        }

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            if (dataGridViewModules.RowCount > 0)
            {
                Forms.ImportExportForm frm = new Forms.ImportExportForm();


                frm.Export( last_displayship != null ? new string[] { "Export Current View", "Export SLEF Loadout" } : new string[] { "Export Current View" },
                                new Forms.ImportExportForm.ShowFlags[] { Forms.ImportExportForm.ShowFlags.ShowCSVOpenInclude, Forms.ImportExportForm.ShowFlags.None },
                                new string[] { "CSV export| *.csv", "SLEF Loadout|*.loadout" }
                                );
                if (last_displayship != null)
                {
                    frm.ShowOptionalButton("Loadout", () =>
                     {
                         frm.Close();
                         string s = last_displayship.JSONLoadout(true).ToString(false);
                         ExtendedControls.InfoForm info = new ExtendedControls.InfoForm();
                         info.Info("Loadout", this.FindForm().Icon, s);
                         info.Show(this);
                     });
                }

                if (frm.ShowDialog(this.FindForm()) == DialogResult.OK)
                {
                    if (frm.SelectedIndex == 0)
                    {
                        BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid(frm.Delimiter);

                        grd.GetPreHeader += delegate (int r)
                        {
                            if (last_displayship != null)
                            {
                                if (r == 0)
                                    return new Object[] { last_displayship.ShipUserName ?? "", last_displayship.ShipUserIdent ?? "", last_displayship.ShipType ?? "", last_displayship.ID };
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

                        var x = DiscoveryForm.History.ShipInformationList.Ships.GetEnumerator();
                        x.MoveNext();

                        grd.GetPostHeader += delegate (int r)
                        {
                            if (r == 0)
                                return new Object[] { };
                            else if (r == 1)
                                return new Object[] { "Ships:" };

                            while (x.MoveNext())
                            {
                                if (x.Current.Value.State == Ship.ShipState.Owned)
                                    return new Object[] { x.Current.Value.ShipFullInfo() };
                            }

                            return null;
                        };

                        grd.WriteGrid(frm.Path, frm.AutoOpen, FindForm());
                    }
                    else
                    {
                        if (!BaseUtils.FileHelpers.TryWriteToFile(frm.Path, last_displayship.JSONLoadout(true).ToString(true)))
                        {
                            CSVHelpers.WriteFailed(FindForm(), frm.Path);
                        }
                    }
                }
            }
            else
                ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "No Ship Information available".Tx(), "Warning".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void extButtonLoadLoadout_Click(object sender, EventArgs e)
        {
            var frm = new Forms.ImportExportForm();

            frm.Import(new string[] { "SLEF - Loadout from EDSY or journal loadout event" },
                 new Forms.ImportExportForm.ShowFlags[] { Forms.ImportExportForm.ShowFlags.ShowPaste  | ExtendedForms.ImportExportForm.ShowFlags.ShowImportSaveas },
                 new string[] { "LoadOut|*.loadout|JSON|*.json" }
            );

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                string loadout = frm.ReadSource();
                if (loadout?.Length>0)
                {
                    Ship si = Ship.CreateFromLoadout(loadout);

                    if ( si !=null )
                    {
                        string name = frm.SaveImportAs.HasChars() ? frm.SaveImportAs : "Last imported ship";
                        name += ".loadout";
                        string path = System.IO.Path.Combine(EDDOptions.Instance.ShipLoadoutsDirectory(), name);

                        if (BaseUtils.FileHelpers.TryWriteToFile(path, si.JSONLoadout(true).ToString(true)))    // this must work, but check
                        {
                            UpdateComboBox();
                            if ( comboBoxShips.Text == name)
                            {
                                Display();
                            }
                            else
                                comboBoxShips.SelectedItem = name;
                        }
                    }
                }
            }
        }

        private void extButtonSaveLoadout_Click(object sender, EventArgs e)
        {
            if (dataGridViewModules.RowCount > 0 && last_displayship != null)
            {
                string name = ExtendedControls.PromptSingleLine.ShowDialog(FindForm(), "Name:", "", "Enter loadout description to save ship with".Tx(), FindForm().Icon, requireinput: true);

                if ( name!=null )
                {
                    name += ".loadout";
                    string path = System.IO.Path.Combine(EDDOptions.Instance.ShipLoadoutsDirectory(), name);
                    if (BaseUtils.FileHelpers.TryWriteToFile(path, last_displayship.JSONLoadout(true).ToString(true)))
                    {
                        UpdateComboBox();
                        comboBoxShips.SelectedItem = name;
                    }
                }
            }
            else
                ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "No Ship Information available".Tx(), "Warning".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void extButtonDeleteLoadout_Click(object sender, EventArgs e)
        {
            string name = comboBoxShips.Text;
            if (ExtendedControls.MessageBoxTheme.Show($"Confirm removal of".Tx()+ " " + name, "Warning".Tx(), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                string path = System.IO.Path.Combine(EDDOptions.Instance.ShipLoadoutsDirectory(), name);
                BaseUtils.FileHelpers.DeleteFileNoError(path);
                UpdateComboBox();       // this will remove the entry from the combo box and go back to travel history
                Display();
            }
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

    }
}
