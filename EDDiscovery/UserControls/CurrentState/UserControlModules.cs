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
        private Ship last_si = null;
        private int last_cargo = 0;

        private string dbDisplayFilters = "DisplayFiltersNew";
        private string[] displayfilters;

        private List<object> allmodulesref = new List<object>();

        #region Init

        public UserControlModules()
        {
            InitializeComponent();
            BaseUtils.TranslatorMkII.Instance.TranslateControls(this);
            BaseUtils.TranslatorMkII.Instance.TranslateTooltip(toolTip, this);
        }

        public override void Init()
        {
            DBBaseName = "ModulesGrid";

            ownedshipstext = "Owned Ships".Tx();
            allshipstext = "All Ships".Tx();
            storedmoduletext = "Stored Modules".Tx();
            travelhistorytext = "Travel History Entry".Tx();
            allmodulestext = "All Modules".Tx();
            allknownmodulestext = "All Known Modules".Tx();
            dataGridViewModules.MakeDoubleBuffered();

            displayfilters = GetSetting(dbDisplayFilters, "fullblueprint;engineeredvalues").Split(';');

            extCheckBoxWordWrap.Checked = GetSetting("WordWrap", true);
            UpdateWordWrap();
            extCheckBoxWordWrap.Click += extCheckBoxWordWrap_Click;

            DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange; ;
            DiscoveryForm.OnNewEntry += Discoveryform_OnNewEntry;
            DiscoveryForm.OnNewUIEvent += Discoveryform_OnNewUIEvent;


            multiPipControlEng.Add(multiPipControlSys);
            multiPipControlEng.Add(multiPipControlWep);
            multiPipControlSys.Add(multiPipControlEng);
            multiPipControlSys.Add(multiPipControlWep);
            multiPipControlWep.Add(multiPipControlSys);
            multiPipControlWep.Add(multiPipControlEng);
            multiPipControlEng.ValueChanged += (s) => { DisplayShipData(last_si); };
            multiPipControlSys.ValueChanged += (s) => { DisplayShipData(last_si); };
            multiPipControlWep.ValueChanged += (s) => { DisplayShipData(last_si); };
            extButtonDrawnResetPips.Text = "RST";   // done to bypass translation

            HideShipRelatedButtons();

            dataGridViewModules.EnableCellHoverOverCallback();
            dataGridViewModules.HoverOverCell += HoverOverCell;
        }

        public override void LoadLayout()
        {
            dataGridViewModules.RowTemplate.MinimumHeight = Font.ScalePixels(26);
            DGVLoadColumnLayout(dataGridViewModules);
        }

        public override void Closing()
        {
            if ( comboBoxShips.Text != allknownmodulestext)     // we fiddle with the columns in this view, so don't save
                DGVSaveColumnLayout(dataGridViewModules);

            DiscoveryForm.OnNewEntry -= Discoveryform_OnNewEntry;
            DiscoveryForm.OnHistoryChange -= Discoveryform_OnHistoryChange;
            DiscoveryForm.OnNewUIEvent -= Discoveryform_OnNewUIEvent;
        }

        #endregion

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

            if (uievent is EliteDangerousCore.UIEvents.UIFuel && last_si != null && DiscoveryForm.History.GetLast?.ShipInformation != null ) 
            {
                // if we are pointing at the same ship, use name since the last_si may be an old one if fuel keeps on updating it.

                if (last_si.ShipNameIdentType == DiscoveryForm.History.GetLast.ShipInformation.ShipNameIdentType ) 
                {
                    DisplayShipData(last_si);
                    System.Diagnostics.Debug.WriteLine("Modules Fuel update");
                }
            }
        }

        public override void InitialDisplay()
        {
            labelVehicle.Visible = buttonExtCoriolis.Visible = buttonExtEDShipyard.Visible = buttonExtConfigure.Visible = false;
            RequestPanelOperation(this, new UserControlCommonBase.RequestTravelHistoryPos());     //request an update 
            extButtonDrawnResetPips.Top = multiPipControlEng.Bottom - extButtonDrawnResetPips.Height;       // lets realign the control manually to make sure it lines up at the bottom
        }

        // new entry received from travel grid
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
                update = !Object.ReferenceEquals(he.ShipInformation, last_si);      
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
                update = !Object.ReferenceEquals(si, last_si);      // this vs ship
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

            dataGridViewModules.Rows.Clear();

            Refresh();

            dataViewScrollerPanel.SuspendLayout();

            last_si = null;     // no ship info
            allmodulesref.Clear();      // no ref to all modules info

            SetColHeaders(null,null,null,null, null,null,null,null);        //default
            sortmodecol = "AAAANANA";

            Value.Visible = SlotCol.Visible = PriorityEnable.Visible = BluePrint.Visible = true;

            if (comboBoxShips.Text == storedmoduletext)
            {
                HideShipRelatedButtons();

                if (last_he?.StoredModules != null)
                {
                    SetColHeaders(null, null, "System".Tx(), "Tx Time".Tx(), 
                                    null, null, "Cost".Tx(), "");

                    ShipModulesInStore mi = last_he.StoredModules;
                    labelVehicle.Text = "";

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

                HideShipRelatedButtons();

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
                HideShipRelatedButtons();
                Value.Visible = SlotCol.Visible = PriorityEnable.Visible = BluePrint.Visible = false;

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

                    dataGridViewModules.Rows.Add(rowobj);
                }
            }
            else if ( comboBoxShips.Text == allshipstext)
            {
                SetColHeaders("", "Type".Tx(), "Manufacturer".Tx(), "Speed".Tx(),
                                null, "Class".Tx(), null, "Info".Tx());
                sortmodecol = "AAANNANA";
                HideShipRelatedButtons(false);

                foreach( var ship in ItemData.GetSpaceships())
                {
                    var rw = dataGridViewModules.RowTemplate.Clone() as DataGridViewRow;           // need to add like this due to different types of cells
                    var pcb = new DataGridViewPictureBoxCell();
                    rw.Cells.Add(pcb);
                    rw.AddTextCells(7);
                    Image img = ItemData.GetShipImage(ship.FDID);
                    pcb.Tag = img;      // directing the hover over to the image
                    pcb.PictureBox.AddImage(new Rectangle(8, 8, 128, 128), img);
                    pcb.PictureBox.Render(minsize: new Size(128 + 8 + 8, 128 + 8 + 8));

                    rw.Cells[1].Value = ship.Name;
                    rw.Cells[2].Value = ship.Manufacturer;
                    rw.Cells[3].Value = ship.Speed.ToString("N0") + " / " + ship.Boost.ToString("N0");
                    rw.Cells[4].Value = ship.HullMass.ToString("N0");
                    rw.Cells[5].Value = ship.ClassString;
                    rw.Cells[6].Value = ship.HullCost.ToString("N0");
                    rw.Cells[7].Value = "S: " + ship.Shields.ToString("N0") + Environment.NewLine +
                                        "A: " + ship.Armour.ToString("N0") + Environment.NewLine +
                                        "Crew: " + ship.Crew.ToString("N0");
                    dataGridViewModules.Rows.Add(rw);
                }

            }
            else if (comboBoxShips.Text == ownedshipstext)
            {
                ShipList shm = DiscoveryForm.History.ShipInformationList;
                var ownedships = (from x1 in shm.Ships where x1.Value.State == Ship.ShipState.Owned && ItemData.IsShip(x1.Value.ShipFD) select x1.Value);

                SetColHeaders("", "Type".Tx(), "Manufacturer".Tx(), "Name".Tx(), "Ident".Tx(), "Mass".Tx(), "Location".Tx(), "Cost".Tx());
                sortmodecol = "AAAAANAN";
                HideShipRelatedButtons(false);

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
            if (firstline >= 0 && firstline < dataGridViewModules.RowCount)
                dataGridViewModules.SafeFirstDisplayedScrollingRowIndex(firstline);
        }

        // call to update the grid and the ship data panel
        private void DisplayShip(Ship si, bool displaydeleteloadoutbutton)
        {
            last_si = si;

            DisplayShipData(si);

            foreach (var key in si.Modules.Keys)
            {
                ShipModule sm = si.Modules[key];
                AddModuleLine(sm);
            }

            labelVehicle.Text = si.ShipFullInfo(cargo: false, fuel: false, manu: true);

            buttonExtConfigure.Visible = si.State == Ship.ShipState.Owned;
            buttonExtCoriolis.Visible = buttonExtEDShipyard.Visible = si.CheckMinimumModulesForCoriolisEDSY();          //ORDER is important due to flow control panel
            extButtonLoadLoadout.Visible = true;
            extPanelRollUpStats.Visible = !ItemData.IsSRVOrFighter(si.ShipFD);
            labelVehicle.Visible = true;
            extButtonSaveLoadout.Visible = true;
            extButtonDeleteLoadout.Visible = displaydeleteloadoutbutton;
            
        }

        // call to update the ship data panel
        private void DisplayShipData(Ship si)
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

            int row = dataGridViewModules.Rows.Add(rowobj);

            dataGridViewModules.Rows[row].Cells[3].ToolTipText = infoentry;

            if (engtooltip != null)
            {
                dataGridViewModules.Rows[row].Cells[5].ToolTipText = engtooltip;
            }
        }

        void AddInfoLine(string s, string v, string opt = "", string tooltip = "")
        {
            object[] rowobj = { s, v.AppendPrePad(opt), "", "", "", "", "", "" };
            dataGridViewModules.Rows.Add(rowobj);
            if (!string.IsNullOrEmpty(tooltip))
                dataGridViewModules.Rows[dataGridViewModules.Rows.Count - 1].Cells[0].ToolTipText = tooltip;
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

        private void HideShipRelatedButtons(bool showcontrol = true)
        {
            extButtonShowControl.Visible = showcontrol;
            extPanelRollUpStats.Visible = extButtonSaveLoadout.Visible = extButtonDeleteLoadout.Visible = extButtonLoadLoadout.Visible =
            labelVehicle.Visible = buttonExtCoriolis.Visible = buttonExtEDShipyard.Visible = buttonExtConfigure.Visible = false;
        }

        #endregion

        #region Word wrap

        private void extCheckBoxWordWrap_Click(object sender, EventArgs e)
        {
            PutSetting("WordWrap", extCheckBoxWordWrap.Checked);
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
                cursel = GetSetting("ShipSelect", "");

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
                PutSetting("ShipSelect", comboBoxShips.Text);
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
            if (last_si != null)
            {
                string errstr;
                string coriolis = last_si.ToJSONCoriolis(out errstr);

                if (errstr.Length > 0)
                    ExtendedControls.MessageBoxTheme.Show(FindForm(), errstr + Environment.NewLine + "This is probably a new or powerplay module" + Environment.NewLine + "Report to EDD Team by Github giving the full text above", "Unknown Module Type");

                System.Diagnostics.Debug.WriteLine("Coriolis Export " + last_si.JSONCoriolis(out string error).ToString(true));

                string uri = EDDConfig.Instance.CoriolisURL + "data=" + coriolis.URIGZipBase64Escape() + "&bn=" + Uri.EscapeDataString(last_si.Name);

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
            if (last_si != null)
            {
                string loadoutjournalline = last_si.ToJSONLoadout();

                // test code
                //loadoutjournalline = BaseUtils.FileHelpers.TryReadAllTextFromFile(@"c:\code\edsysidewinder.out");
                //QuickJSON.JToken tk = QuickJSON.JToken.Parse(loadoutjournalline, out string err);
                //QuickJSON.JArray tk1 = tk.Array();
                //QuickJSON.JObject tko = tk1[0]["data"].Object();
                //loadoutjournalline = tko.ToString(true);

                System.Diagnostics.Debug.WriteLine("EDSY Export " + last_si.JSONLoadout().ToString(true));
                //System.Diagnostics.Debug.WriteLine("EDSY Export " + loadoutjournalline);

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
            System.Diagnostics.Debug.Assert(last_si != null);           // must be set for this configure button to be visible

            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();

            int width = 430;
            int ctrlleft = 150;

            f.Add(new ExtendedControls.ConfigurableEntryList.Entry("L", typeof(Label), "Fuel Warning".Tx()+": ", new Point(10, 40), new Size(140, 24), ""));
            f.Add(new ExtendedControls.ConfigurableEntryList.Entry("FuelWarning", typeof(ExtendedControls.NumberBoxDouble),
                last_si.FuelWarningPercent.ToString(), new Point(ctrlleft, 40), new Size(width - ctrlleft - 20, 24), "Enter fuel warning level in % (0 = off, 1-100%)".Tx())
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
                    if ( ExtendedControls.MessageBoxTheme.Show(FindForm(), "Confirm sell of ship".Tx()+": "+ Environment.NewLine + last_si.ShipNameIdentType , "Warning".Tx(), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes ) 
                    {
                        var je = new EliteDangerousCore.JournalEvents.JournalShipyardSell(DateTime.UtcNow, last_si.ShipFD, last_si.ID, 0, EDCommander.CurrentCmdrID);
                        var jo = je.Json();
                        je.Add(jo);
                        DiscoveryForm.NewEntry(je);
                    }

                    f.ReturnResult(DialogResult.Cancel);
                }
            };

            DialogResult res = f.ShowDialogCentred(this.FindForm(), this.FindForm().Icon, "Ship Configure".Tx(), closeicon:true);

            if (res == DialogResult.OK)
            {
                last_si.FuelWarningPercent = f.GetDouble("FuelWarning").Value;
                Display();
            }
        }

        private void dataGridViewModules_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (sortmodecol.HasChars() && sortmodecol[e.Column.Index] == 'N')
                e.SortDataGridViewColumnNumeric(removetext:"t", striptonumeric:true);
        }

        private void dataGridViewModules_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (comboBoxShips.Text == ownedshipstext)
                {
                    Ship shp = dataGridViewModules.Rows[e.RowIndex].Tag as Ship;
                    if (shp != null)     // paranoia
                    {
                        string ident = shp.ShipNameIdentType;
                        comboBoxShips.SelectedItem = ident;
                    }
                }
                else
                {
                    string tt = dataGridViewModules.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText;
                    if (!string.IsNullOrEmpty(tt))
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


                frm.Export( last_si != null ? new string[] { "Export Current View", "Export SLEF Loadout" } : new string[] { "Export Current View" },
                                new Forms.ImportExportForm.ShowFlags[] { Forms.ImportExportForm.ShowFlags.ShowCSVOpenInclude, Forms.ImportExportForm.ShowFlags.None },
                                new string[] { "CSV export| *.csv", "SLEF Loadout|*.loadout" }
                                );
                if (last_si != null)
                {
                    frm.ShowOptionalButton("Loadout", () =>
                     {
                         frm.Close();
                         string s = last_si.JSONLoadout().ToString(false);
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
                        if (!BaseUtils.FileHelpers.TryWriteToFile(frm.Path, last_si.JSONLoadout().ToString(true)))
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

                        if (BaseUtils.FileHelpers.TryWriteToFile(path, si.JSONLoadout().ToString(true)))    // this must work, but check
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
            if (dataGridViewModules.RowCount > 0 && last_si != null)
            {
                string name = ExtendedControls.PromptSingleLine.ShowDialog(FindForm(), "Name:", "", "Enter loadout description to save ship with".Tx(), FindForm().Icon, requireinput: true);

                if ( name!=null )
                {
                    name += ".loadout";
                    string path = System.IO.Path.Combine(EDDOptions.Instance.ShipLoadoutsDirectory(), name);
                    if (BaseUtils.FileHelpers.TryWriteToFile(path, last_si.JSONLoadout().ToString(true)))
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
