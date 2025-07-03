/*
 * Copyright © 2023 - 2023 EDDiscovery development team
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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls.Helpers
{
    public partial class SpanshStationsUserControl : UserControl
    {
        public SpanshStationsUserControl()
        {
            InitializeComponent();
        }

        public void Init(EliteDangerousCore.DB.IUserDatabaseSettingsSaver saver, Func<bool> isClosed)
        {
            this.IsClosed = isClosed;
            this.saver = saver;
            colOutfitting.DefaultCellStyle.Alignment =
              colShipyard.DefaultCellStyle.Alignment =
              colHasMarket.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            extTextBoxAutoCompleteSystem.SetAutoCompletor(SystemCache.ReturnSystemAutoCompleteList, true);
            extTextBoxAutoCompleteSystem.ReturnPressed += (s) => {
                System.Diagnostics.Debug.WriteLine($"SpanshStation return pressed {extTextBoxAutoCompleteSystem.Text}");
                extTextBoxAutoCompleteSystem.CancelAutoComplete();
                explicity_set_system = extTextBoxAutoCompleteSystem.Text.HasChars(); 
                DrawSystem(); return true;
            };
            extTextBoxAutoCompleteSystem.AutoCompleteTimeout = 1000;

            filters = new ExtButtonWithNewCheckedListBox[] { extButtonType, extButtonCommoditiesBuy, extButtonCommoditiesSell, extButtonOutfitting, extButtonShipyard, extButtonEconomy, extButtonServices };

            // we place the fdname of the station type and the translated text
            var stationtype = StationDefinitions.ValidTypes(true).Select(x => new CheckedIconUserControl.Item(x.ToString(),StationDefinitions.ToLocalisedLanguage(x)));
            extButtonType.InitAllNoneAllBack(stationtype,
                GetFilter(FilterSettings.Type),
                (newsetting,ch) => { SetFilter(FilterSettings.Type, newsetting, ch); });

            // we place fdname of commodity with translated name
            var comitems = MaterialCommodityMicroResourceType.GetCommodities(MaterialCommodityMicroResourceType.SortMethod.AlphabeticalRaresLast)
                            .Select(x => new CheckedIconUserControl.Item(x.FDName, x.TranslatedName));

            extButtonCommoditiesBuy.InitAllNoneAllBack(comitems,
                GetFilter(FilterSettings.CommoditiesBuy),
                (newsetting, ch) => { SetFilter(FilterSettings.CommoditiesBuy, newsetting, ch); });

            extButtonCommoditiesSell.InitAllNoneAllBack(comitems,
                GetFilter(FilterSettings.CommoditiesSell),
                (newsetting, ch) => { SetFilter(FilterSettings.CommoditiesSell, newsetting, ch); });

            // we place fdname of module and the translated text
            // note the bodges - only get the lightweight armour, and then mangle the text
            var moditems = ItemData.GetShipModules(compressarmourtosidewinderonly:true).
                                Select(x => new CheckedIconUserControl.Item(x.Key, x.Value.TranslatedModName.Replace("Sidewinder ","")));

            extButtonOutfitting.InitAllNoneAllBack(moditems,
                GetFilter(FilterSettings.Outfitting),
                (newsetting, ch) => { SetFilter(FilterSettings.Outfitting, newsetting, ch); }, sortitems:true);

            // we place the spaceship fdname with its string name
            var ships = ItemData.GetSpaceships().Select(x =>  new CheckedIconUserControl.Item(x.FDID,x.Name));

            extButtonShipyard.InitAllNoneAllBack(ships,
                GetFilter(FilterSettings.Shipyard),
                (newsetting, ch) => { SetFilter(FilterSettings.Shipyard, newsetting, ch); });

            // we place the fdname of the economy (as per EconomyDefinitions) with the localised name
            extButtonEconomy.SettingsSplittingChar = '\u2345';     // because ; is used in identifiers. Key is fdname
            extButtonEconomy.InitAllNoneAllBack(EconomyDefinitions.ValidStates().Select(x => new CheckedIconUserControl.Item(x.ToString(), EconomyDefinitions.ToLocalisedLanguage(x))),
                GetFilter(FilterSettings.Economy),
                (newsetting, ch) => { SetFilter(FilterSettings.Economy, newsetting, ch); });

            // we place the fdname of the service (as per StationDefinitions) with the localised name
            var services = StationDefinitions.ValidServices().Select(x => new CheckedIconUserControl.Item(x.ToString(),StationDefinitions.ToLocalisedLanguage(x) ));
            extButtonServices.InitAllNoneAllBack(services,
                GetFilter(FilterSettings.Services),
                (newsetting, ch) => { SetFilter(FilterSettings.Services, newsetting, ch); });

            saver.DGVLoadColumnLayout(dataGridView);
            valueBoxMaxLs.ValueNoChange = saver.GetSetting(dbLS, 1000000.0);

            extCheckBoxWordWrap.Checked = saver.GetSetting(dbWordWrap, false);
            UpdateWordWrap();
            extCheckBoxWordWrap.Click += extCheckBoxWordWrap_Click;

            ColStockDemand1.Visible = ColStockDemand2.Visible = ColStockDemand3.Visible = 
            ColPrice1.Visible = ColPrice2.Visible = ColPrice3.Visible = 
            colDistanceRef.Visible = colSystem.Visible = false;
        }

        public void Close()
        {
            saver.DGVSaveColumnLayout(dataGridView);
            saver.PutSetting(dbLS, valueBoxMaxLs.Value);

        }

        // update the default system, and if we have not got an explicity set system, update data on screen

        public void UpdateDefaultSystem(ISystem sys)
        {
            defaultsystem = sys;
            if (explicity_set_system == false)
                DrawSystem();
        }
        
        // explicity display system
        public void DisplaySystemStations(ISystem sys)
        {
            defaultsystem = sys;
            explicity_set_system = false;
            DrawSystem();
        }

        // get data for system, either defaultsystem (explicity set system = false) or text system
        private async void DrawSystem()
        {
            // if explicity set, must have chars
            ISystem sys = explicity_set_system ? new SystemClass(extTextBoxAutoCompleteSystem.Text) : defaultsystem;

            System.Diagnostics.Debug.WriteLine($"Spansh station kick with min {valueBoxMaxLs.Value} at {sys.Name}");

            extTextBoxAutoCompleteSystem.TextNoChange = sys.Name;       // replace with system actually displayed, no text change
            extTextBoxAutoCompleteSystem.ClearOnFirstChar = true;       // reset so we can clear on next text input

            EliteDangerousCore.Spansh.SpanshClass sp = new EliteDangerousCore.Spansh.SpanshClass();
            stationdata = await sp.GetStationsByDumpAsync(sys);
            if (IsClosed())      // async protect!
                return;

            colDistanceRef.Visible = colSystem.Visible = false;
            extButtonTravelSystem.Enabled = explicity_set_system;

            Draw(true);
        }

        // clearallfilters means all filters are cleared
        // alwaysclear means filter will always clear this
        private bool DrawSearch(List<StationInfo> si, bool clearallfilters, FilterSettings alwaysclear)
        {
            if (si == null)
            {
                MessageBoxTheme.Show(this.FindForm(), $"No stations returned", "Warning".TxID(EDTx.Warning), MessageBoxButtons.OK);
                return false;
            }
            else
            {
                foreach (FilterSettings e in Enum.GetValues(typeof(FilterSettings)))
                {
                    // go thru filters and reset the filter. alwaysclear == commoditiesbuy clears sell as well
                    if (clearallfilters || e == alwaysclear || (e == FilterSettings.CommoditiesSell && alwaysclear == FilterSettings.CommoditiesBuy))     
                    {
                        SetFilter(e, CheckedIconGroupUserControl.Disabled, false);  // update the DB
                        filters[(int)e].Set(CheckedIconGroupUserControl.Disabled);  // we need to update the button with the same setting
                    }
                }

                stationdata = si.ToList();
                explicity_set_system = true;

                colDistanceRef.Visible = colSystem.Visible = true;
                extButtonTravelSystem.Enabled = true;

                if (!extTextBoxAutoCompleteSystem.Text.Contains("("))           // name gets postfix added
                    extTextBoxAutoCompleteSystem.TextNoChange += " (Search)";
                extTextBoxAutoCompleteSystem.ClearOnFirstChar = true;
                Draw(true);

                return true;
            }
        }

        private void Draw(bool removesort)
        {
            bool wassorted = dataGridView.SortedColumn != null;
            DataGridViewColumn sortcolprev = (dataGridView.SortedColumn?.Visible??false) ? dataGridView.SortedColumn : colSystem.Visible ? colSystem : colBodyName;
            SortOrder sortorderprev = dataGridView.SortedColumn != null ? dataGridView.SortOrder : SortOrder.Ascending;

            dataViewScrollerPanel.Suspend();
            dataGridView.Rows.Clear();

            if (stationdata != null)
            {
                // we precompute this, as we need to expand all armour_grade1 back out into all their different armour types before checking
                string[] outfittinglist = ItemData.ExpandArmours(extButtonOutfitting.Get().SplitNoEmptyStartFinish(extButtonOutfitting.SettingsSplittingChar));

                foreach (var station in stationdata)
                {
                    bool filterin = station.DistanceToArrival <= valueBoxMaxLs.Value;

                    if (!extButtonType.IsDisabled)
                        filterin &= extButtonType.Get().HasChars() && extButtonType.Get().SplitNoEmptyStartFinish(extButtonType.SettingsSplittingChar).Contains(station.FDStationType.ToString(), StringComparison.InvariantCultureIgnoreCase) >= 0;

                    if (!extButtonCommoditiesBuy.IsDisabled)
                        filterin &= extButtonCommoditiesBuy.Get().HasChars() && station.HasAnyItemInStock(extButtonCommoditiesBuy.Get().SplitNoEmptyStartFinish(extButtonCommoditiesBuy.SettingsSplittingChar));

                    if (!extButtonCommoditiesSell.IsDisabled)
                        filterin &= extButtonCommoditiesSell.Get().HasChars() && station.HasAnyItemWithDemandAndPrice(extButtonCommoditiesSell.Get().SplitNoEmptyStartFinish(extButtonCommoditiesSell.SettingsSplittingChar));

                    if (!extButtonOutfitting.IsDisabled)
                        filterin &= extButtonOutfitting.Get().HasChars() && station.HasAnyModuleTypes(outfittinglist);

                    if (!extButtonShipyard.IsDisabled)
                        filterin &= extButtonShipyard.Get().HasChars() && station.HasAnyShipTypes(extButtonShipyard.Get().SplitNoEmptyStartFinish(extButtonShipyard.SettingsSplittingChar));

                    if (!extButtonEconomy.IsDisabled)
                        filterin &= extButtonEconomy.Get().HasChars() && station.HasAnyEconomyTypes(extButtonEconomy.Get().SplitNoEmptyStartFinish(extButtonEconomy.SettingsSplittingChar));

                    if (!extButtonServices.IsDisabled)
                        filterin &= extButtonServices.Get().HasChars() && station.HasAnyServicesTypes(extButtonServices.Get().SplitNoEmptyStartFinish(extButtonServices.SettingsSplittingChar));

                    if (filterin)
                    {
                        string ss = station.StationServices != null ? string.Join(", ", station.StationServices.Select(x => StationDefinitions.ToLocalisedLanguage(x))) : "";

                        object[] cells = new object[]
                        {
                            station.System.Name,
                            station.DistanceRefSystem.ToString("N1"),
                            station.BodyName?.ReplaceIfStartsWith(station.System.Name) ?? "",
                            station.StationName,
                            station.DistanceToArrival > 0 ? station.DistanceToArrival.ToString("N1") : "",
                            StationDefinitions.ToLocalisedLanguage(station.FDStationType),
                            station.Latitude.HasValue ? station.Latitude.Value.ToString("N4") : "",
                            station.Longitude.HasValue ? station.Longitude.Value.ToString("N4") : "",
                            station.MarketStateString,
                            ColPrice1.Tag != null ? station.GetItemPriceString((string)ColPrice1.Tag,showcommoditiesselltostation) ?? "" : "",
                            ColPrice1.Tag != null ? station.GetItemStockDemandString((string)ColPrice1.Tag,showcommoditiesselltostation) ?? "" : "",
                            ColPrice2.Tag != null ? station.GetItemPriceString((string)ColPrice2.Tag,showcommoditiesselltostation) ?? "" : "",
                            ColPrice2.Tag != null ? station.GetItemStockDemandString((string)ColPrice2.Tag,showcommoditiesselltostation) ?? "" : "",
                            ColPrice3.Tag != null ? station.GetItemPriceString((string)ColPrice3.Tag,showcommoditiesselltostation) ?? "" : "",
                            ColPrice3.Tag != null ? station.GetItemStockDemandString((string)ColPrice3.Tag,showcommoditiesselltostation) ?? "" : "",
                            station.OutfittingStateString,
                            station.ShipyardStateString,
                            AllegianceDefinitions.ToLocalisedLanguage(station.Allegiance),
                            station.Faction ?? "",
                            EconomyDefinitions.ToLocalisedLanguage(station.Economy),
                            GovernmentDefinitions.ToLocalisedLanguage(station.Government),
                            ss,
                            station.LandingPads?.Small.ToString() ?? "",
                            station.LandingPads?.Medium.ToString() ?? "",
                            station.LandingPads?.Large.ToString() ?? "",
                        };

                        var rw = dataGridView.RowTemplate.Clone() as DataGridViewRow;
                        rw.CreateCells(dataGridView, cells);
                        rw.Tag = station;
                        dataGridView.Rows.Add(rw);
                        if (ss.HasChars())
                            rw.Cells[colServices.Index].ToolTipText = ss.Replace(", ", Environment.NewLine);
                        if ( station.EconomyList!=null)
                            rw.Cells[colEconomy.Index].ToolTipText = string.Join(Environment.NewLine, station.EconomyList.Select(x=>$"{x.Name_Localised} : {(x.Proportion*100.0):N1}%"));
                        if (ColPrice1.Tag != null)
                            rw.Cells[ColPrice1.Index].ToolTipText = rw.Cells[ColStockDemand1.Index].ToolTipText = station.GetItemString((string)ColPrice1.Tag);
                        if (ColPrice2.Tag != null)
                            rw.Cells[ColPrice2.Index].ToolTipText = rw.Cells[ColStockDemand2.Index].ToolTipText = station.GetItemString((string)ColPrice2.Tag);
                        if (ColPrice3.Tag != null)
                            rw.Cells[ColPrice3.Index].ToolTipText = rw.Cells[ColStockDemand3.Index].ToolTipText = station.GetItemString((string)ColPrice3.Tag);
                    }
                }
            }

            if (!removesort)
            {
                if (wassorted)
                {
                    dataGridView.Sort(sortcolprev, (sortorderprev == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                    dataGridView.Columns[sortcolprev.Index].HeaderCell.SortGlyphDirection = sortorderprev;
                }
            }
            else
            {
                foreach (DataGridViewColumn c in dataGridView.Columns)
                    c.HeaderCell.SortGlyphDirection = SortOrder.None; 
            }

            colLattitude.Visible = colLongitude.Visible = ColPrice1.Tag == null && ColPrice2.Tag == null && ColPrice3.Tag == null;
            ColStockDemand1.Visible = ColPrice1.Visible = ColPrice1.Tag != null;
            ColStockDemand2.Visible = ColPrice2.Visible = ColPrice2.Tag != null;
            ColStockDemand3.Visible = ColPrice3.Visible = ColPrice3.Tag != null;
            ColStockDemand1.HeaderText = ColStockDemand2.HeaderText = ColStockDemand3.HeaderText = showcommoditiesselltostation ? "Demand" : "Stock";

            dataViewScrollerPanel.Resume();

        }

        #region UI

        private void dataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column == colBodyName || e.Column == colSystem)
                e.SortDataGridViewColumnAlphaInt();
            else if (e.Column == colDistance || e.Column == colDistanceRef || e.Column == colLattitude || e.Column == colLongitude ||
                     e.Column == ColPrice1 || e.Column == ColPrice2 || e.Column == ColPrice3)
                e.SortDataGridViewColumnNumeric();
            else if ( e.Column == colHasMarket || e.Column == colOutfitting || e.Column == colShipyard )
                e.SortDataGridViewColumnNumeric("\u2713 ");
        }

        private void valueBoxMaxLs_ValueChanged(object sender, EventArgs e)
        {
            Draw(false);
        }
        private void extButtonTravelSystem_Click(object sender, EventArgs e)
        {
            if (explicity_set_system)
            {
                explicity_set_system = false;
                DrawSystem();
            }
        }

        #endregion

        #region Right click and UIs

        private void extCheckBoxWordWrap_Click(object sender, EventArgs e)
        {
            saver.PutSetting(dbWordWrap, extCheckBoxWordWrap.Checked);
            UpdateWordWrap();
        }


        private void dataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            EliteDangerousCore.StationInfo si = dataGridView.RightClickRowValid ? dataGridView.Rows[dataGridView.RightClickRow].Tag as EliteDangerousCore.StationInfo : null;
            viewMarketToolStripMenuItem.Enabled = si?.HasMarket ?? false;
            viewOutfittingToolStripMenuItem.Enabled = si?.HasOutfitting ?? false;
            viewShipyardToolStripMenuItem.Enabled = si?.HasShipyard ?? false;
            viewOnSpanshToolStripMenuItem.Enabled = si?.MarketID.HasValue ?? false;
        }

        private void viewOnSpanshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StationInfo si = dataGridView.Rows[dataGridView.RightClickRow].Tag as EliteDangerousCore.StationInfo;
            EliteDangerousCore.Spansh.SpanshClass.LaunchBrowserForStationByMarketID(si.MarketID.Value);
        }

        private void viewMarketToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StationInfo si = dataGridView.Rows[dataGridView.RightClickRow].Tag as EliteDangerousCore.StationInfo;
            si.ViewMarket(FindForm(), saver);
        }

  
        private void viewOutfittingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StationInfo si = dataGridView.Rows[dataGridView.RightClickRow].Tag as EliteDangerousCore.StationInfo;
            si.ViewOutfitting(FindForm(), saver);
        }

  
        private void viewShipyardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StationInfo si = dataGridView.Rows[dataGridView.RightClickRow].Tag as StationInfo;
            si.ViewShipyard(FindForm(), saver);
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ( e.RowIndex>=0)
            {
                EliteDangerousCore.StationInfo si = dataGridView.Rows[e.RowIndex].Tag as EliteDangerousCore.StationInfo;
                if (e.ColumnIndex == colOutfitting.Index && si.HasOutfitting)
                    si.ViewOutfitting(FindForm(), saver);
                else if (e.ColumnIndex == colShipyard.Index && si.HasShipyard)
                    si.ViewShipyard(FindForm(), saver);
                else if (e.ColumnIndex == colHasMarket.Index && si.HasMarket)
                    si.ViewMarket(FindForm(), saver);
            }
        }

   
        #endregion

        #region Searches

        Size numberboxsize = new Size(60, 24);
        Size labelsize = new Size(120, 24);
        const int maxresults = 200;

        int commoditiessearchdistance = 40;
        HashSet<string> commoditiesstate = new HashSet<string>();
        bool commoditiesclearfilters = true , commoditieslargepad = false, commoditiescarriers = true;

        private void extButtonSearchCommodities_Click(object sender, EventArgs e)
        {
            char separ = '\u2345';

            string systemname = extTextBoxAutoCompleteSystem.Text.Substring(0, extTextBoxAutoCompleteSystem.Text.IndexOfOrLength("(")).Trim();
            ConfigurableForm.ShowDialogCentred(
            (f) => {
                var commodities = MaterialCommodityMicroResourceType.GetNormalCommodities(MaterialCommodityMicroResourceType.SortMethod.Alphabetical);
                var rarecommodities = MaterialCommodityMicroResourceType.GetRareCommodities(MaterialCommodityMicroResourceType.SortMethod.Alphabetical);

                // tag consists of english name <separ> fdname
                int max = f.AddBools(commodities.Select(x => x.EnglishName + separ + x.FDName).ToArray(), commodities.Select(x => x.TranslatedName).ToArray(), commoditiesstate, 4, 24, 1000, 4, 200, "S_");
                f.AddBools(rarecommodities.Select(x => x.EnglishName + separ + x.FDName).ToArray(), rarecommodities.Select(x => x.TranslatedName).ToArray(), commoditiesstate, max + 16, 24, 500, 4, 200, "S_");

                f.Add(new ConfigurableEntryList.Entry("B_Buy", showcommoditiesselltostation, "Sell to Station", new Point(600, 4), new Size(100, 22), "Set = Search for stations with a station buy price and has demand, Clear = Search for stations with stock to sell") { PlacedInPanel = ConfigurableEntryList.Entry.PanelType.Top });

                AddSearchEntries(f, commoditiessearchdistance, commoditiesclearfilters, commoditieslargepad, commoditiescarriers, lpadx:700,carrierx:800,clearfilterx:900);

            }, 
            async (f)=>
            {
                SetValues(f, ref commoditiessearchdistance, ref commoditiesclearfilters, ref commoditieslargepad, ref commoditiescarriers);

                commoditiesstate = f.GetCheckedListNames("S_").ToHashSet();
                showcommoditiesselltostation = f.GetBool("B_Buy").Value;

                var entries = f.GetCheckedListEntries("S_");

                if (entries.Length > 0)
                {
                    EliteDangerousCore.Spansh.SpanshClass sp = new EliteDangerousCore.Spansh.SpanshClass();

                    var search = entries.Select(x => new EliteDangerousCore.Spansh.SpanshClass.SearchCommoditity
                    {
                        EnglishName = x.Name.Substring(2, x.Name.IndexOf(separ) - 2),                               // extract the english name, remove the S_ prefix

                        supply = !showcommoditiesselltostation ? new Tuple<int, int>(1, int.MaxValue) : null,      // if we want to buy, we need the supply to be >=1
                     //   sellprice = !showcommoditiesstationtobuyprice ? new Tuple<int, int>(1, int.MaxValue) : null,   // if we want to buy, we need a sell price to be >=1 - we have removed this for now

                        demand = showcommoditiesselltostation ? new Tuple<int, int>(1, int.MaxValue) : null,       // if we want to sell, we need demand
                        buyprice = showcommoditiesselltostation ? new Tuple<int, int>(1, int.MaxValue) : null      // if we want to sell, we need a price
                    }).ToArray();

                    var res = await sp.SearchCommoditiesAsync(systemname, search, commoditiessearchdistance, commoditieslargepad ? true : default(bool?), commoditiescarriers, maxresults);

                    if ( res != null && !IsClosed() )      // must have a result and async protect
                    {
                        ColPrice1.Tag = entries.Length >= 1 ? entries[0].Name.Substring(entries[0].Name.IndexOf(separ) + 1) : null;      // need to get the fdname for the columns
                        ColPrice1.HeaderText = entries.Length >= 1 ? entries[0].TextValue : "?";

                        ColPrice2.Tag = entries.Length >= 2 ? entries[1].Name.Substring(entries[1].Name.IndexOf(separ) + 1) : null;
                        ColPrice2.HeaderText = entries.Length >= 2 ? entries[1].TextValue : "?";

                        ColPrice3.Tag = entries.Length >= 3 ? entries[2].Name.Substring(entries[2].Name.IndexOf(separ) + 1) : null;
                        ColPrice3.HeaderText = entries.Length >= 3 ? entries[2].TextValue : "?";

                        showcommoditiesstate = commoditiesstate.Take(3).Select(x=>x.Substring(x.IndexOf(separ) +1)).ToHashSet();     // take the list, limit to 3, get the fdname only, fill in the c-state

                        DrawSearch(res, commoditiesclearfilters, FilterSettings.CommoditiesBuy);        // use Buy, will clear sell as well
                    }
                }
            },
            this, $"Commodities from {systemname}", 32);
        }

        HashSet<string> showcommoditiesstate = new HashSet<string>();
        private void extButtonEditCommodities_Click(object sender, EventArgs e)
        {
            ConfigurableForm.ShowDialogCentred((f) => {
                var commodities = MaterialCommodityMicroResourceType.GetNormalCommodities(MaterialCommodityMicroResourceType.SortMethod.Alphabetical);
                var rarecommodities = MaterialCommodityMicroResourceType.GetRareCommodities(MaterialCommodityMicroResourceType.SortMethod.Alphabetical);
                f.Add(new ConfigurableEntryList.Entry("B_Buy", showcommoditiesselltostation, "Sell to Station", new Point(600, 4), new Size(160, 22), 
                                    $"Set = Show price station buys the commodity at {Environment.NewLine} Clear = Show station sell price") { PlacedInPanel = ConfigurableEntryList.Entry.PanelType.Top });

                int max = f.AddBools(commodities.Select(x => x.FDName).ToArray(), commodities.Select(x => x.TranslatedName).ToArray(), showcommoditiesstate, 4, 24, 1000, 4, 200, "S_");
                f.AddBools(rarecommodities.Select(x => x.FDName).ToArray(), rarecommodities.Select(x => x.TranslatedName).ToArray(), showcommoditiesstate, max + 16, 24, 500, 4, 200, "S_");
                f.Add(new ConfigurableEntryList.Entry("OK", typeof(ExtButton), "Show", new Point(300, 4), new Size(80, 24), null) { PlacedInPanel = ConfigurableEntryList.Entry.PanelType.Top });

                f.Trigger += (d, ctrlname, text) => { f.RadioButton("S_", ctrlname, 3); };
            },
            (f) =>
            {
                showcommoditiesstate = f.GetCheckedListNames("S_").ToHashSet();
                showcommoditiesselltostation = f.GetBool("B_Buy").Value;

                var entries = f.GetCheckedListEntries("S_");        // 0 to 3.. 

                ColPrice1.Tag = entries.Length >= 1 ? entries[0].Name.Substring(2) : null;      // set to the fdname
                ColPrice1.HeaderText = entries.Length >= 1 ? entries[0].TextValue : "?";

                ColPrice2.Tag = entries.Length >= 2 ? entries[1].Name.Substring(2) : null;
                ColPrice2.HeaderText = entries.Length >= 2 ? entries[1].TextValue : "?";

                ColPrice3.Tag = entries.Length >= 3 ? entries[2].Name.Substring(2) : null;
                ColPrice3.HeaderText = entries.Length >= 3 ? entries[2].TextValue : "?";

                Draw(false);
            },
            this, $"Select Commodities to show",32);
        }

        int servicessearchdistance = 40;
        HashSet<string> servicestate = new HashSet<string>();
        bool servicesclearfilters = true, serviceslargepad = false, servicescarriers = true;

        private void extButtonSearchServiceTypes_Click(object sender, EventArgs e)
        {
            string systemname = extTextBoxAutoCompleteSystem.Text.Substring(0, extTextBoxAutoCompleteSystem.Text.IndexOfOrLength("(")).Trim();

            ConfigurableForm.ShowDialogCentred((f) => {
                var services = StationDefinitions.ValidServices();
                f.AddBools(services.Select(x=>x.ToString()).ToArray(), services.Select(x=>StationDefinitions.ToLocalisedLanguage(x)).ToArray(), servicestate, 4, 24, 200, 4, 200, "S_");
                AddSearchEntries(f, servicessearchdistance, servicesclearfilters, serviceslargepad, servicescarriers);
            },
            async (f) =>
            {
                SetValues(f, ref servicessearchdistance, ref servicesclearfilters, ref serviceslargepad, ref servicescarriers);

                servicestate = f.GetCheckedListNames("S_").ToHashSet();

                var checkedlist = f.GetCheckedListEntries("S_").Select(x => x.Name.Substring(2)).ToArray();
                if (checkedlist.Length > 0)
                {
                    EliteDangerousCore.Spansh.SpanshClass sp = new EliteDangerousCore.Spansh.SpanshClass();
                    var res = await sp.SearchServicesAsync(systemname, checkedlist, servicessearchdistance, serviceslargepad ? true : default(bool?), servicescarriers, maxresults);
                    if (!IsClosed())      // async protect!
                    {
                        DrawSearch(res, servicesclearfilters, FilterSettings.Services);
                    }
                }
            },
            this, $"Services from {systemname}", 32);
        }

        int economysearchdistance = 40;
        HashSet<string> economystate = new HashSet<string>();
        bool economyclearfilters = true, economylargepad = false;
        private void extButtonSearchEconomy_Click(object sender, EventArgs e)
        {
            string systemname = extTextBoxAutoCompleteSystem.Text.Substring(0, extTextBoxAutoCompleteSystem.Text.IndexOfOrLength("(")).Trim();

            ConfigurableForm.ShowDialogCentred((f) =>
            {
                var economy = EconomyDefinitions.ValidStates();
                f.AddBools(economy.Select(x => x.ToString()).ToArray(), economy.Select(x => EconomyDefinitions.ToLocalisedLanguage(x)).ToArray(), economystate, 4, 24, 120, 4, 120, "S_");
                AddSearchEntries(f, economysearchdistance, economyclearfilters, economylargepad, null, clearfilterx: 700);
            },
            async (f) =>
            {
                bool _ = false;
                SetValues(f, ref economysearchdistance, ref economyclearfilters, ref economylargepad, ref _);

                economystate = f.GetCheckedListNames("S_").ToHashSet();

                var checkedlist = f.GetCheckedListEntries("S_").Select(x => x.Name.Substring(2)).ToArray();
                if (checkedlist.Length > 0)
                {
                    EliteDangerousCore.Spansh.SpanshClass sp = new EliteDangerousCore.Spansh.SpanshClass();
                    var res = await sp.SearchEconomyAsync(systemname, checkedlist, economysearchdistance, economylargepad ? true : default(bool?), maxresults);
                    if (!IsClosed())      // async protect!
                    {
                        DrawSearch(res, economyclearfilters, FilterSettings.Economy); ;
                    }
                }
            },
            this, $"Economies from {systemname}", 32);
        }


        int shipssearchdistance = 40;
        HashSet<string> shipsstate = new HashSet<string>();
        bool shipsclearfilters = true, shipslargepad = false, shipscarriers = true;
        private void extButtonSearchShips_Click(object sender, EventArgs e)
        {
            string systemname = extTextBoxAutoCompleteSystem.Text.Substring(0, extTextBoxAutoCompleteSystem.Text.IndexOfOrLength("(")).Trim();

            ConfigurableForm.ShowDialogCentred((f) => {
                var ships = ItemData.GetSpaceships().Select(x => x.EDCDName).OrderBy(y=>y).ToArray();
                f.AddBools(ships, ships, shipsstate, 4, 24, 200, 4, 200, "S_");
                AddSearchEntries(f, shipssearchdistance, shipsclearfilters, shipslargepad, shipscarriers);
            },
            async (f) => {
                SetValues(f, ref shipssearchdistance, ref shipsclearfilters, ref shipslargepad, ref shipscarriers);

                shipsstate = f.GetCheckedListNames("S_").ToHashSet();
                var checkedlist = f.GetCheckedListEntries("S_").Select(x => x.Name.Substring(2)).ToArray();
                if (checkedlist.Length > 0)
                {
                    EliteDangerousCore.Spansh.SpanshClass sp = new EliteDangerousCore.Spansh.SpanshClass();
                    var res = await sp.SearchShipsAsync(systemname, checkedlist, shipssearchdistance, shipslargepad ? true : default(bool?), shipscarriers, maxresults);
                    if (!IsClosed())      // async protect!
                    {
                        DrawSearch(res, shipsclearfilters, FilterSettings.Shipyard);
                    }
                }
            },
            this, $"Ships from {systemname}",32);
        }


        int outfittingsearchdistance = 40;
        bool[] outfittingmodtypes = new bool[256];
        bool[] outfittingclasses = new bool[10] { true, false, false, false, false,    false, false, false, false,false };   // 0 =all, 0..8
        bool[] outfittingratings = new bool[8] { true, false, false, false, false, false, false, false };   // 0 = all, A..G
        bool outfittingclearfilters = true, outfittinglargepad = false, outfittingcarriers = true;

        private void extButtonSearchOutfitting_Click(object sender, EventArgs e)
        {
            string systemname = extTextBoxAutoCompleteSystem.Text.Substring(0, extTextBoxAutoCompleteSystem.Text.IndexOfOrLength("(")).Trim();

            var moditems = ItemData.GetModuleTypeNamesTranslations(ItemData.GetShipModules());      // kvp of english modtype names vs translated names

            ConfigurableForm.ShowDialogCentred((f) =>
            {
                // key is in english of course, value is translated.
                f.AddBools(moditems.Select(x=>x.Key).ToArray(), moditems.Select(x=>x.Value).ToArray(), outfittingmodtypes, 4, 24, 550, 4, 200, "M_");

                int vpos = 580;
                for (int cls = 0; cls < outfittingclasses.Length; cls++)
                {
                    f.Add(new ConfigurableEntryList.Entry("C_" + cls, outfittingclasses[cls], cls == 0 ? "All Classes" : "Class " + (cls - 1), new Point(4 + cls * 100, vpos), new Size(100, 22), null));
                }

                vpos += 30;
                for (int rating = 0; rating < outfittingratings.Length; rating++)
                {
                    f.Add(new ConfigurableEntryList.Entry("R_" + rating, outfittingratings[rating], rating == 0 ? "All Ratings" : "Rating " + (char)('A' - 1 + rating), new Point(4 + rating * 100, vpos), new Size(100, 22), null));
                }

                AddSearchEntries(f, outfittingsearchdistance, outfittingclearfilters, outfittinglargepad, outfittingcarriers);

                f.Trigger += (name, ctrl, obj) =>
                {
                    System.Diagnostics.Debug.WriteLine($"Click on {name} {ctrl}");
                    f.GetControl("OK").Enabled = f.IsAllValid();
                    if (ctrl == "C_0")
                        f.SetCheckedList(new string[] { "C_1", "C_2", "C_3", "C_4", "C_5", "C_6", "C_7", "C_8", "C_9" }, false);
                    else if (ctrl.StartsWith("C_"))
                        f.SetCheckedList(new string[] { "C_0" }, false);
                    if (ctrl == "R_0")
                        f.SetCheckedList(new string[] { "R_1", "R_2", "R_3", "R_4", "R_5", "R_6", "R_7" }, false);
                    else if (ctrl.StartsWith("R_"))
                        f.SetCheckedList(new string[] { "R_0" }, false);
                };
            },
            async (f) =>
            {
                var modlist = f.GetCheckedListNames("M_").ToArray();

                outfittingmodtypes = f.GetCheckBoxBools("M_");
                outfittingclasses = f.GetCheckBoxBools("C_");
                outfittingratings = f.GetCheckBoxBools("R_");
                outfittingsearchdistance = f.GetInt("radius").Value;
                outfittingclearfilters = f.GetBool("CLRF").Value;

                if (modlist.Length > 0 && outfittingclasses.Contains(true) && outfittingratings.Contains(true))
                {
                    EliteDangerousCore.Spansh.SpanshClass sp = new EliteDangerousCore.Spansh.SpanshClass();

                    List<StationInfo> ssd = await sp.SearchOutfittingAsync(systemname, modlist, outfittingclasses, outfittingratings, outfittingsearchdistance, outfittinglargepad ? true : default(bool?), outfittingcarriers, maxresults);

                    if (!IsClosed())      // async protect!
                    {
                        if (ssd?.Count > 0)
                        {
                            DrawSearch(ssd, outfittingclearfilters, FilterSettings.Outfitting);
                        }
                        else
                        {
                            MessageBoxTheme.Show(this.FindForm(), $"No stations returned", "Warning".TxID(EDTx.Warning), MessageBoxButtons.OK);
                        }
                    }

                }
            }, this, $"Outfitting from {systemname}", 32);
        }

        private void AddSearchEntries(ConfigurableForm f, int searchdistance, bool clearfilters, bool lpad, bool? carrier, int maxdx = 400, int lpadx = 600, int carrierx = 700, int clearfilterx = 800)
        {
            f.Add(new ConfigurableEntryList.Entry("OK", typeof(ExtButton), "Show", new Point(300, 4), new Size(80, 24), null) { PlacedInPanel = ConfigurableEntryList.Entry.PanelType.Top });
            f.AddLabelAndEntry("Maximum Distance", new Point(maxdx, 8), labelsize, new ConfigurableEntryList.Entry("radius", searchdistance, new Point(maxdx + labelsize.Width, 4), numberboxsize, "Maximum distance") { NumberBoxLongMinimum = 1, PlacedInPanel = ConfigurableEntryList.Entry.PanelType.Top });
            f.Add(new ConfigurableEntryList.Entry("LPAD", lpad, "Large Pad", new Point(lpadx, 4), new Size(100, 22), "Large pad is required") { PlacedInPanel = ConfigurableEntryList.Entry.PanelType.Top });
            if (carrier.HasValue)
                f.Add(new ConfigurableEntryList.Entry("CARRIER", carrier.Value, "Incl Carriers", new Point(carrierx, 4), new Size(100, 22), "Drake Class Carriers are to be included") { PlacedInPanel = ConfigurableEntryList.Entry.PanelType.Top });
            f.Add(new ConfigurableEntryList.Entry("CLRF", clearfilters, "Clear other filters", new Point(clearfilterx, 4), new Size(160, 22), "Type filter is cleared, clear the others as well") { PlacedInPanel = ConfigurableEntryList.Entry.PanelType.Top });
        }
        private void SetValues(ConfigurableForm f, ref int commoditiessearchdistance, ref bool commoditiesclearfilters, ref bool commoditieslargepad, ref bool commoditiescarriers)
        {
            commoditiessearchdistance = f.GetInt("radius").Value;
            commoditiesclearfilters = f.GetBool("CLRF").Value;
            commoditieslargepad = f.GetBool("LPAD").Value;
            var v = f.GetBool("CARRIER");
            commoditiescarriers = v.HasValue ? v.Value : false;
        }


        #endregion

        #region Misc

        private void UpdateWordWrap()
        {
            dataGridView.SetWordWrap(extCheckBoxWordWrap.Checked);
            dataViewScrollerPanel.UpdateScroll();
        }

        private void SetFilter(FilterSettings f, string newsetting, bool redraw)
        {
            saver.PutSetting(f.ToString(), newsetting);
            if (redraw)
                Draw(false);
        }

        private string GetFilter(FilterSettings f)
        {
            return saver.GetSetting(f.ToString(), CheckedIconGroupUserControl.Disabled);
        }

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            Forms.ImportExportForm frm = new Forms.ImportExportForm();
            frm.Export(new string[] { "View" },
                            new Forms.ImportExportForm.ShowFlags[] { Forms.ImportExportForm.ShowFlags.ShowCSVOpenInclude, Forms.ImportExportForm.ShowFlags.None },
                            new string[] { "CSV|*.csv" },
                            new string[] { "stations" }
                );

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                if (frm.SelectedIndex == 0)
                {
                    BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid(frm.Delimiter);

                    grd.GetLine += delegate (int r)
                    {
                        if (r < dataGridView.RowCount)
                        {
                            DataGridViewRow rw = dataGridView.Rows[r];
                            object[] ret = new object[dataGridView.ColumnCount];
                            for (int i = 0; i < dataGridView.ColumnCount; i++)
                                ret[i] = rw.Cells[i].Value;

                            return ret;
                        }
                        else
                            return null;
                    };

                    grd.GetHeader += delegate (int c)
                    {
                        if (frm.IncludeHeader)
                        {
                            if (c < dataGridView.ColumnCount)
                                return dataGridView.Columns[c].HeaderText;
                        }
                        return null;
                    };

                    grd.WriteGrid(frm.Path, frm.AutoOpen, FindForm());
                }
            }
        }

        #endregion

        #region vars

        private EliteDangerousCore.DB.IUserDatabaseSettingsSaver saver;
        private bool explicity_set_system = false;
        private ISystem defaultsystem;
        private List<StationInfo> stationdata;

        private bool showcommoditiesselltostation = false;      // on columns, show buy price, else sell price (if has stock)

        private const string dbLS = "MaxLs";

        private enum FilterSettings { Type, CommoditiesBuy, CommoditiesSell, Outfitting, Shipyard, Economy, Services };
        private ExtButtonWithNewCheckedListBox[] filters;

        private const string dbWordWrap = "WordWrap";

        private Func<bool> IsClosed;

        #endregion
    }
}
