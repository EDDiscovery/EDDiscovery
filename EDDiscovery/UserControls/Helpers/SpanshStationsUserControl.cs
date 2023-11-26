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
        private EliteDangerousCore.DB.IUserDatabaseSettingsSaver saver;
        private bool explicity_set_system = false;
        private ISystem defaultsystem;
        private List<StationInfo> stationdata;

        private const string dbLS = "MaxLs";

        enum FilterSettings { Type, Commodities, Outfitting, Shipyard, Economy, Services };
        ExtButtonWithCheckedIconListBoxGroup[] filters;

        private const string dbWordWrap = "WordWrap";

        public SpanshStationsUserControl()
        {
            InitializeComponent();
        }

        public void Init(EliteDangerousCore.DB.IUserDatabaseSettingsSaver saver)
        {
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

            filters = new ExtButtonWithCheckedIconListBoxGroup[] { extButtonType, extButtonCommodities, extButtonOutfitting, extButtonShipyard, extButtonEconomy, extButtonServices };

            var porttype = StationDefinitions.StarportTypes.Values.Distinct().Select(x => new CheckedIconListBoxFormGroup.StandardOption(x,x));
            extButtonType.InitAllNoneAllBack(porttype,
                GetFilter(FilterSettings.Type),
                (newsetting,ch) => { SetFilter(FilterSettings.Type, newsetting, ch); });


            var comitems = MaterialCommodityMicroResourceType.GetCommodities(true)
                            .Select(x => new CheckedIconListBoxFormGroup.StandardOption(x.FDName, x.Name));

            extButtonCommodities.InitAllNoneAllBack(comitems,
                GetFilter(FilterSettings.Commodities),
                (newsetting,ch) => { SetFilter(FilterSettings.Commodities, newsetting, ch); });

            var moditems = ItemData.GetModules().Select(x => x.ModTypeString).Distinct().      // only return buyable modules
                            Select(x2 => new CheckedIconListBoxFormGroup.StandardOption(x2, x2));

            extButtonOutfitting.InitAllNoneAllBack(moditems,
                GetFilter(FilterSettings.Outfitting),
                (newsetting, ch) => { SetFilter(FilterSettings.Outfitting, newsetting, ch); });

            var ships = ItemData.GetSpaceships().Select(x =>
                new CheckedIconListBoxFormGroup.StandardOption(((ItemData.ShipInfoString)x[ItemData.ShipPropID.FDID]).Value,
                            ((ItemData.ShipInfoString)x[ItemData.ShipPropID.Name]).Value));

            extButtonShipyard.InitAllNoneAllBack(ships,
                GetFilter(FilterSettings.Shipyard),
                (newsetting, ch) => { SetFilter(FilterSettings.Shipyard, newsetting, ch); });

            // could use Identifers to localise later
            var economy = EconomyDefinitions.Types.Select(x => new CheckedIconListBoxFormGroup.StandardOption(x.Key, x.Value));

            extButtonEconomy.SettingsSplittingChar = '\u2345';     // because ; is used in identifiers
            extButtonEconomy.InitAllNoneAllBack(economy,
                GetFilter(FilterSettings.Economy),
                (newsetting, ch) => { SetFilter(FilterSettings.Economy, newsetting, ch); });

            var services = StationDefinitions.ServiceTypes.Select(x => x.Value).Distinct().Select(x => new CheckedIconListBoxFormGroup.StandardOption(x, x));
            extButtonServices.InitAllNoneAllBack(services,
                GetFilter(FilterSettings.Services),
                (newsetting, ch) => { SetFilter(FilterSettings.Services, newsetting, ch); });

            ///  tbd   saver.DGVLoadColumnLayout(dataGridView);
            valueBoxMaxLs.ValueNoChange = saver.GetSetting(dbLS, 1000000.0);

            extCheckBoxWordWrap.Checked = saver.GetSetting(dbWordWrap, false);
            UpdateWordWrap();
            extCheckBoxWordWrap.Click += extCheckBoxWordWrap_Click;

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

            colDistanceRef.Visible = colSystem.Visible = false;
            extButtonTravelSystem.Enabled = explicity_set_system;
            Draw(true);
        }

        private void DrawSearch(List<StationInfo> si, bool clearotherfilters, FilterSettings alwaysclear)
        {
            foreach (FilterSettings e in Enum.GetValues(typeof(FilterSettings)))
            {
                if (clearotherfilters || e == alwaysclear)     // go thru filters and reset the filter
                {
                    SetFilter(e, CheckedIconListBoxFormGroup.Disabled, false);  // update the DB
                    filters[(int)e].Set(CheckedIconListBoxFormGroup.Disabled);  // we need to update the button with the same setting
                }
            }

            stationdata = si;
            explicity_set_system = true;
            colDistanceRef.Visible = colSystem.Visible = true;
            extButtonTravelSystem.Enabled = true;
            if (!extTextBoxAutoCompleteSystem.Text.Contains("("))           // name gets postfix added
                extTextBoxAutoCompleteSystem.TextNoChange += " (Search)";
            extTextBoxAutoCompleteSystem.ClearOnFirstChar = true;
            Draw(true);
        }


        public void Draw(bool removesort)
        {
            DataGridViewColumn sortcolprev = (dataGridView.SortedColumn?.Visible??false) ? dataGridView.SortedColumn : colSystem.Visible ? colSystem : colBodyName;
            SortOrder sortorderprev = dataGridView.SortedColumn != null ? dataGridView.SortOrder : SortOrder.Ascending;

            dataViewScrollerPanel.Suspend();
            dataGridView.Rows.Clear();

            if (stationdata != null)
            {
                foreach (var station in stationdata)
                {
                    string stationtype = station.StationType ?? "Unknown";

                    bool filterin = station.DistanceToArrival <= valueBoxMaxLs.Value;

                    if (!extButtonType.IsDisabled)
                        filterin &= extButtonType.Get().HasChars() && extButtonType.Get().SplitNoEmptyStartFinish(extButtonType.SettingsSplittingChar).Contains(stationtype, StringComparison.InvariantCultureIgnoreCase) >= 0;

                    if (!extButtonCommodities.IsDisabled)
                        filterin &= extButtonCommodities.Get().HasChars() && station.HasAnyItemToBuy(extButtonCommodities.Get().SplitNoEmptyStartFinish(extButtonCommodities.SettingsSplittingChar));

                    if (!extButtonOutfitting.IsDisabled)
                        filterin &= extButtonOutfitting.Get().HasChars() && station.HasAnyModuleTypes(extButtonOutfitting.Get().SplitNoEmptyStartFinish(extButtonOutfitting.SettingsSplittingChar));

                    if (!extButtonShipyard.IsDisabled)
                        filterin &= extButtonShipyard.Get().HasChars() && station.HasAnyShipTypes(extButtonShipyard.Get().SplitNoEmptyStartFinish(extButtonShipyard.SettingsSplittingChar));

                    if (!extButtonEconomy.IsDisabled)
                        filterin &= extButtonEconomy.Get().HasChars() && station.HasAnyEconomyTypes(extButtonEconomy.Get().SplitNoEmptyStartFinish(extButtonEconomy.SettingsSplittingChar));

                    if (!extButtonServices.IsDisabled)
                        filterin &= extButtonServices.Get().HasChars() && station.HasAnyServicesTypes(extButtonServices.Get().SplitNoEmptyStartFinish(extButtonServices.SettingsSplittingChar));

                    if (filterin)
                    {
                        string ss = station.StationServices != null ? string.Join(", ", station.StationServices) : "";
                        object[] cells = new object[]
                        {
                            station.System.Name,
                            station.DistanceRefSystem.ToString("N1"),
                            station.BodyName?.ReplaceIfStartsWith(station.System.Name) ?? "",
                            station.StationName,
                            station.DistanceToArrival > 0 ? station.DistanceToArrival.ToString("N1") : "",
                            stationtype,
                            station.Latitude.HasValue ? station.Latitude.Value.ToString("N4") : "",
                            station.Longitude.HasValue ? station.Longitude.Value.ToString("N4") : "",
                            station.MarketStateString,
                            station.OutfittingStateString,
                            station.ShipyardStateString,
                            station.Allegiance ?? "",
                            station.Economy_Localised ?? "",
                            station.Government_Localised ?? "",
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
                    }
                }
            }

            if (!removesort)
            {
                dataGridView.Sort(sortcolprev, (sortorderprev == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                dataGridView.Columns[sortcolprev.Index].HeaderCell.SortGlyphDirection = sortorderprev;
            }
            else
            {
                foreach (DataGridViewColumn c in dataGridView.Columns)
                    c.HeaderCell.SortGlyphDirection = SortOrder.None; 
            }

            dataViewScrollerPanel.Resume();

        }

        #region UI

        private void dataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column == colBodyName || e.Column == colSystem)
                e.SortDataGridViewColumnAlphaInt();
            else if (e.Column == colDistance || e.Column == colDistanceRef || e.Column == colLattitude || e.Column == colLongitude)
                e.SortDataGridViewColumnNumeric();
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

        #region Right click

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
            EliteDangerousCore.StationInfo si = dataGridView.Rows[dataGridView.RightClickRow].Tag as EliteDangerousCore.StationInfo;
            EliteDangerousCore.Spansh.SpanshClass.LaunchBrowserForStationByMarketID(si.MarketID.Value);
        }

        private void viewMarketToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.StationInfo si = dataGridView.Rows[dataGridView.RightClickRow].Tag as EliteDangerousCore.StationInfo;
            ViewMarket(si);
        }

        private void ViewMarket(StationInfo si)
        { 
            var dgvpanel = new ExtPanelDataGridViewScrollWithDGV<BaseUtils.DataGridViewColumnControl>();
            dgvpanel.DataGrid.CreateTextColumns("Category", 100, 5,
                                                "Name", 150, 5,
                                                "Buy", 50, 5,
                                                "Stock", 50, 5,
                                                "Sell", 50, 5
                                                );

            dgvpanel.DataGrid.SortCompare += (s, ev) => { if (ev.Column.Index >= 2) ev.SortDataGridViewColumnNumeric(); };
            dgvpanel.DataGrid.RowHeadersVisible = false;

            saver.DGVLoadColumnLayout(dgvpanel.DataGrid, "ShowMarket");

            foreach (var commd in si.Market.EmptyIfNull())
            {
                object[] rowobj = { commd.loccategory,
#if DEBUG
                    commd.locName + ":" + commd.fdname,
#else
                    commd.locName,
#endif
                    commd.buyPrice.ToString("N0"),
                                    commd.stock.ToString("N0"),
                                    commd.sellPrice.ToString("N0") };
                var row = dgvpanel.DataGrid.RowTemplate.Clone() as DataGridViewRow;
                row.CreateCells(dgvpanel.DataGrid, rowobj);
                dgvpanel.DataGrid.Rows.Add(row);
            }

            ConfigurableForm f = new ConfigurableForm();
            f.Add(new ConfigurableForm.Entry(dgvpanel, "Grid", "", new System.Drawing.Point(3, 30), new System.Drawing.Size(800, 400), null)
            { Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom });
            f.AddOK(new Point(800 - 100, 460), "OK", anchor: AnchorStyles.Right | AnchorStyles.Bottom);
            f.InstallStandardTriggers();
            f.AllowResize = true;

            string title = "Materials/Commodities for ".T(EDTx.UserControlFactions_MaterialCommodsFor) + si.StationName + " " + (si.MarketUpdateUTC.Year > 2000 ? EDDConfig.Instance.ConvertTimeToSelected(si.MarketUpdateUTC).ToString() : "No Data");
            f.ShowDialogCentred(FindForm(), FindForm().Icon, title, closeicon: true);

            saver.DGVSaveColumnLayout(dgvpanel.DataGrid, "ShowMarket");

        }

        private void viewOutfittingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.StationInfo si = dataGridView.Rows[dataGridView.RightClickRow].Tag as EliteDangerousCore.StationInfo;
            ViewOutfitting(si);
        }

        private void ViewOutfitting(StationInfo si)
        { 
            var dgvpanel = new ExtPanelDataGridViewScrollWithDGV<BaseUtils.DataGridViewColumnControl>();
            dgvpanel.DataGrid.CreateTextColumns("Category", 100, 5,
                                                "Name", 150, 5);

            dgvpanel.DataGrid.RowHeadersVisible = false;

            saver.DGVLoadColumnLayout(dgvpanel.DataGrid, "ShowOutfitting");

            foreach (var oi in si.Outfitting.EmptyIfNull())
            {
                object[] rowobj = { oi.ModType,
                                    oi.Name };
                var row = dgvpanel.DataGrid.RowTemplate.Clone() as DataGridViewRow;
                row.CreateCells(dgvpanel.DataGrid, rowobj);
                dgvpanel.DataGrid.Rows.Add(row);
            }

            ConfigurableForm f = new ConfigurableForm();
            f.Add(new ConfigurableForm.Entry(dgvpanel, "Grid", "", new System.Drawing.Point(3, 30), new System.Drawing.Size(800, 400), null)
            { Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom });
            f.AddOK(new Point(800 - 100, 460), "OK", anchor: AnchorStyles.Right | AnchorStyles.Bottom);
            f.InstallStandardTriggers();
            f.AllowResize = true;

            string title = "Outfitting for " + si.StationName + " " + (si.OutfittingUpdateUTC.Year>2000 ? EDDConfig.Instance.ConvertTimeToSelected(si.OutfittingUpdateUTC).ToString() : "No Data");
            f.ShowDialogCentred(FindForm(), FindForm().Icon, title, closeicon: true);

            saver.DGVSaveColumnLayout(dgvpanel.DataGrid, "ShowOutfitting");

        }

        private void viewShipyardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StationInfo si = dataGridView.Rows[dataGridView.RightClickRow].Tag as StationInfo;
            ViewShipyard(si);
        }
        private void ViewShipyard(StationInfo si)
        { 
            var dgvpanel = new ExtPanelDataGridViewScrollWithDGV<BaseUtils.DataGridViewColumnControl>();
            dgvpanel.DataGrid.CreateTextColumns("Name", 100, 5);

            dgvpanel.DataGrid.RowHeadersVisible = false;

            saver.DGVLoadColumnLayout(dgvpanel.DataGrid, "ShowShipyard");

            foreach (var oi in si.Shipyard.EmptyIfNull())
            {
                object[] rowobj = { oi.ShipType_Localised,
                                    };
                var row = dgvpanel.DataGrid.RowTemplate.Clone() as DataGridViewRow;
                row.CreateCells(dgvpanel.DataGrid, rowobj);
                dgvpanel.DataGrid.Rows.Add(row);
            }

            ConfigurableForm f = new ConfigurableForm();
            f.Add(new ConfigurableForm.Entry(dgvpanel, "Grid", "", new System.Drawing.Point(3, 30), new System.Drawing.Size(800, 400), null)
            { Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom });
            f.AddOK(new Point(800 - 100, 460), "OK", anchor: AnchorStyles.Right | AnchorStyles.Bottom);
            f.InstallStandardTriggers();
            f.AllowResize = true;

            string title = "Shipyard for " + si.StationName + " " + (si.ShipyardUpdateUTC.Year > 2000 ? EDDConfig.Instance.ConvertTimeToSelected(si.ShipyardUpdateUTC).ToString() : "No Data");
            f.ShowDialogCentred(FindForm(), FindForm().Icon, title, closeicon: true);

            saver.DGVSaveColumnLayout(dgvpanel.DataGrid, "ShowShipyard");
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ( e.RowIndex>=0)
            {
                EliteDangerousCore.StationInfo si = dataGridView.Rows[e.RowIndex].Tag as EliteDangerousCore.StationInfo;
                if (e.ColumnIndex == colOutfitting.Index && si.HasOutfitting)
                    ViewOutfitting(si);
                else if (e.ColumnIndex == colShipyard.Index && si.HasShipyard)
                    ViewShipyard(si);
                else if (e.ColumnIndex == colHasMarket.Index && si.HasMarket)
                    ViewMarket(si);
            }
        }

        #endregion

        #region Searches

        const int topmargin = 30;
        const int dataleft = 140;
        Size numberboxsize = new Size(64, 24);
        Size comboboxsize = new Size(200, 24);
        Size textboxsize = new Size(200, 28);
        Size checkboxsize = new Size(154, 24);
        Size labelsize = new Size(dataleft - 4, 24);

        int servicessearchdistance = 40;
        bool[] servicestate = new bool[128];
        bool servicesclearfilters = true;

        private void extButtonSearchServiceTypes_Click(object sender, EventArgs e)
        {
            var services = StationDefinitions.ServiceTypes.Select(x => x.Value).Distinct();
            Search(services, FilterSettings.Services, ref servicesclearfilters, ref servicestate, ref servicessearchdistance, 400);
        }

        int commoditiessearchdistance = 40;
        bool[] commoditiesstate = new bool[2048];
        bool commoditiesclearfilter = true;

        private void extButtonSearchCommodities_Click(object sender, EventArgs e)
        {
            var commodities = MaterialCommodityMicroResourceType.GetCommodities(true).Select(x=>x.EnglishName);
            Search(commodities, FilterSettings.Commodities, ref commoditiesclearfilter, ref commoditiesstate, ref commoditiessearchdistance, 1800);
        }

        int economysearchdistance = 40;
        bool[] economystate = new bool[64];
        bool economyclearfilter = true;
        private void extButtonSearchEconomy_Click(object sender, EventArgs e)
        {
            var economy = EconomyDefinitions.Types.Values.Select(x => x);
            Search(economy, FilterSettings.Economy, ref economyclearfilter, ref economystate, ref economysearchdistance, 200);

        }

        int shipssearchdistance = 40;
        bool[] shipsstate = new bool[128];
        bool shipsclearfilter = true;
        private void extButtonSearchShips_Click(object sender, EventArgs e)
        {
            var ships = ItemData.GetSpaceships().Select(x => x.ContainsKey(ItemData.ShipPropID.EDCDName) ? ((ItemData.ShipInfoString)x[ItemData.ShipPropID.EDCDName]).Value : ((ItemData.ShipInfoString)x[ItemData.ShipPropID.Name]).Value);
            Search(ships, FilterSettings.Shipyard, ref shipsclearfilter, ref shipsstate, ref shipssearchdistance, 400);
        }

        const int maxresults = 200;

        private void Search(IEnumerable<string> names, FilterSettings filter, ref bool clearfilters, ref bool[] state, ref int searchdistance, int dialogdepth)
        {
            string title = filter.ToString().SplitCapsWordFull();

            ConfigurableForm f = new ConfigurableForm();
            int vpos = 40;
            f.Add(new ConfigurableForm.Entry("CLRF", clearfilters, "Clear other filters", new Point(240, vpos), new Size(160, 22), title + " is cleared, clear the others as well"));
            f.AddLabelAndEntry("Maximum Distance", new Point(4, 4), ref vpos, 40, labelsize, new ConfigurableForm.Entry("radius", searchdistance, new Point(dataleft, 0), numberboxsize, "Maximum distance") { NumberBoxLongMinimum = 1 });
            int i = 0;
            int svpos = vpos;
            int colx = 4;
            foreach (var sv in names)
            {
                f.Add(ref vpos, 24, new ConfigurableForm.Entry("S_" + sv, state[i++], sv, new Point(colx, 0), new Size(200, 22), "Search for " + sv));
                if (vpos > dialogdepth)
                {
                    vpos = svpos;
                    colx += 200;
                }
            }

            f.Add(new ConfigurableForm.Entry("OK", typeof(ExtButton), "Search", new Point(400, 40), new Size(80, 24), null));
            f.InstallStandardTriggers();
            f.Trigger += (name, text, obj) => { f.GetControl("OK").Enabled = f.IsAllValid(); };

            if (extTextBoxAutoCompleteSystem.Text.HasChars())
            {
                string systemname = extTextBoxAutoCompleteSystem.Text.Substring(0, extTextBoxAutoCompleteSystem.Text.IndexOfOrLength("(")).Trim();

                if (f.ShowDialogCentred(FindForm(), FindForm().Icon, $"Find {title} from {systemname}", closeicon: true) == DialogResult.OK)
                {
                    var checkedlist = f.GetCheckedList("S_").Select(x => x.Substring(2)).ToArray();
                    state = f.GetCheckBoxBools("S_");
                    searchdistance = f.GetInt("radius").Value;
                    clearfilters = f.GetBool("CLRF").Value;

                    if (checkedlist.Length > 0)
                    {
                        EliteDangerousCore.Spansh.SpanshClass sp = new EliteDangerousCore.Spansh.SpanshClass();
                        List<StationInfo> ssd = null;

                        if (filter == FilterSettings.Services)
                            ssd = sp.SearchServices(systemname, checkedlist, searchdistance, maxresults);
                        else if (filter == FilterSettings.Commodities)
                            ssd = sp.SearchCommodities(systemname, checkedlist, searchdistance, maxresults);
                        else if (filter == FilterSettings.Economy)
                            ssd = sp.SearchEconomy(systemname, checkedlist, searchdistance, maxresults);
                        else if (filter == FilterSettings.Shipyard)
                            ssd = sp.SearchShips(systemname, checkedlist, searchdistance, maxresults);

                        if (ssd?.Count > 0)
                        {
                            DrawSearch(ssd, clearfilters, filter);
                        }
                        else
                        {
                            MessageBoxTheme.Show(this.FindForm(), $"No stations returned", "Warning".TxID(EDTx.Warning), MessageBoxButtons.OK);
                        }
                    }
                }
            }

        }


        int outfittingsearchdistance = 40;
        bool[] outfittingmodtypes = new bool[256];
        bool[] outfittingclasses = new bool[8] { true, false, false, false, false, false, false, false };   // 0 =all, 0..6
        bool[] outfittingratings = new bool[8] { true, false, false, false, false, false, false, false };   // 0 = all, A..G
        bool outfittingclearfilters = true;

        private void extButtonSearchOutfitting_Click(object sender, EventArgs e)
        {
            string title = "Outfitting";

            var moditems = ItemData.GetModules().Select(x => x.ModTypeString).Distinct();      // only return buyable modules

            ConfigurableForm f = new ConfigurableForm();
            int vpos = 40;
            f.Add(new ConfigurableForm.Entry("CLRF", outfittingclearfilters, "Clear other filters", new Point(240, vpos), new Size(160, 22), title + " is cleared, clear the others as well"));
            f.AddLabelAndEntry("Maximum Distance", new Point(4, 4), ref vpos, 40, labelsize, new ConfigurableForm.Entry("radius", outfittingsearchdistance, new Point(dataleft, 0), numberboxsize, "Maximum distance") { NumberBoxLongMinimum = 1 });
            int i = 0;
            int svpos = vpos;
            int colx = 4;
            foreach (var sv in moditems)
            {
                f.Add(ref vpos, 24, new ConfigurableForm.Entry("M_" + sv, outfittingmodtypes[i++], sv, new Point(colx, 0), new Size(200, 22), "Search for " + sv));
                if (vpos > 600)
                {
                    vpos = svpos;
                    colx += 200;
                }
            }

            vpos = 640;

            for (int cls = 0; cls <= 7; cls++)
            {
                f.Add(new ConfigurableForm.Entry("C_" + cls, outfittingclasses[cls], cls == 0 ? "All Classes" : "Class " + (cls-1), new Point(cls * 150, vpos), new Size(140, 22), null));
            }

            vpos += 30;

            for (int rating = 0; rating <= 7; rating++)
            {
                f.Add(new ConfigurableForm.Entry("R_" + rating, outfittingratings[rating], rating == 0 ? "All Ratings" : "Rating " + (char)('A'-1+rating), new Point(rating * 150, vpos), new Size(140, 22), null));
            }

            f.Add(new ConfigurableForm.Entry("OK", typeof(ExtButton), "Search", new Point(400, 40), new Size(80, 24), null));
            f.InstallStandardTriggers();
            f.Trigger += (name, ctrl, obj) => {
                System.Diagnostics.Debug.WriteLine($"Click on {name} {ctrl}");
                f.GetControl("OK").Enabled = f.IsAllValid();
                if (ctrl == "C_0")
                    f.SetCheckedList(new string[] { "C_1", "C_2", "C_3", "C_4", "C_5", "C_6", "C_7" }, false);
                else if (ctrl.StartsWith("C_"))
                    f.SetCheckedList(new string[] { "C_0" }, false);
                if (ctrl == "R_0")
                    f.SetCheckedList(new string[] { "R_1", "R_2", "R_3", "R_4", "R_5", "R_6" , "R_7" }, false);
                else if (ctrl.StartsWith("R_"))
                    f.SetCheckedList(new string[] { "R_0" }, false);
            };

            if (extTextBoxAutoCompleteSystem.Text.HasChars())
            {
                string systemname = extTextBoxAutoCompleteSystem.Text.Substring(0, extTextBoxAutoCompleteSystem.Text.IndexOfOrLength("(")).Trim();

                if (f.ShowDialogCentred(FindForm(), FindForm().Icon, $"Find {title} from {systemname}", closeicon: true) == DialogResult.OK)
                {
                    var modlist = f.GetCheckedList("M_").Select(x => x.Substring(2)).ToArray();

                    outfittingmodtypes = f.GetCheckBoxBools("M_");
                    outfittingclasses = f.GetCheckBoxBools("C_");
                    outfittingratings = f.GetCheckBoxBools("R_");
                    outfittingsearchdistance = f.GetInt("radius").Value;
                    outfittingclearfilters = f.GetBool("CLRF").Value;

                    if ( modlist.Length>0 && outfittingclasses.Contains(true) && outfittingratings.Contains(true))
                    {
                        EliteDangerousCore.Spansh.SpanshClass sp = new EliteDangerousCore.Spansh.SpanshClass();
                        List<StationInfo> ssd = sp.SearchOutfitting(systemname, modlist, outfittingclasses, outfittingratings, outfittingsearchdistance, maxresults);
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
            }
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
            return saver.GetSetting(f.ToString(), CheckedIconListBoxFormGroup.Disabled);
        }

        private void Excel()
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

    }
}
