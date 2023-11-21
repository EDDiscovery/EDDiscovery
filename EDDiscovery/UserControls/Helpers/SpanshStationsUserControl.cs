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
        private List<EliteDangerousCore.JournalEvents.StationInfo> stationdata;
        
        private const string dbLS = "MaxLs";

        private const string dbTY = "TypeFilter";
        private string typefilter;      // current setting of typefilter, expanded out (no all/none). empty = none

        private const string dbCM = "CommdFilter";
        private string commdfilter;      // current setting of commdfilter, expanded out (no all/none). empty = none

        private const string dbOF = "OutfittingFilter";
        private string outfittingfilter;      // current setting of outfitting filter, expanded out (no all/none). empty = none

        private const string dbSH = "ShipsFilter";
        private string shipsfilter;      // current setting of filter, expanded out (no all/none). empty = none

        private const string dbEC = "EconomyFilter";
        private string economyfilter;      // current setting of filter, expanded out (no all/none). empty = none

        private const string dbSV = "ServicesFilter";
        private string servicesfilter;      // current setting of filter, expanded out (no all/none). empty = none

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
                GetStationData(); return true;
            };
            extTextBoxAutoCompleteSystem.AutoCompleteTimeout = 1000;

            var porttype = StationDefinitions.StarportTypes.Values.Distinct().Select(x => new ExtendedControls.CheckedIconListBoxFormGroup.StandardOption(x,x));
            extButtonType.InitAllNoneAllBack(porttype,
                typefilter = saver.GetSetting(dbTY, ExtendedControls.CheckedIconListBoxFormGroup.Disabled),
                (newsetting) => { if (typefilter != newsetting) { typefilter = newsetting; saver.PutSetting(dbTY, newsetting); Draw(); } });


            var comitems = MaterialCommodityMicroResourceType.GetCommodities(true)
                            .Select(x => new ExtendedControls.CheckedIconListBoxFormGroup.StandardOption(x.FDName, x.Name));

            extButtonCommodities.InitAllNoneAllBack(comitems,
                commdfilter = saver.GetSetting(dbCM, ExtendedControls.CheckedIconListBoxFormGroup.Disabled),
                (newsetting) => { if (commdfilter != newsetting) { commdfilter = newsetting; saver.PutSetting(dbCM, newsetting); Draw(); } });

            var moditems = ItemData.GetAllModules(true, false).Select(x=>x.ModType).Distinct().
                            Select(x2 => new ExtendedControls.CheckedIconListBoxFormGroup.StandardOption(x2, x2));

            extButtonOutfitting.InitAllNoneAllBack(moditems,
                outfittingfilter = saver.GetSetting(dbOF, ExtendedControls.CheckedIconListBoxFormGroup.Disabled),
                (newsetting) => { if (outfittingfilter != newsetting) { outfittingfilter = newsetting; saver.PutSetting(dbOF, newsetting); Draw(); } });

            var ships = ItemData.GetSpaceships().Select(x => 
                new ExtendedControls.CheckedIconListBoxFormGroup.StandardOption(((ItemData.ShipInfoString)x[ItemData.ShipPropID.FDID]).Value, 
                            ((ItemData.ShipInfoString)x[ItemData.ShipPropID.Name]).Value));

            extButtonShipyard.InitAllNoneAllBack(ships,
                shipsfilter = saver.GetSetting(dbSH, ExtendedControls.CheckedIconListBoxFormGroup.Disabled),
                (newsetting) => { if (shipsfilter != newsetting) { shipsfilter = newsetting; saver.PutSetting(dbSH, newsetting); Draw(); } });

            // tbd could use Identifers to localise later
            var economy = EconomyDefinitions.Types.Select(x => new ExtendedControls.CheckedIconListBoxFormGroup.StandardOption(x.Key, x.Value));

            extButtonEconomy.SettingsSplittingChar = '\u2345';     // because ; is used in identifiers
            extButtonEconomy.InitAllNoneAllBack(economy,
                economyfilter = saver.GetSetting(dbEC, ExtendedControls.CheckedIconListBoxFormGroup.Disabled),
                (newsetting) => { if (economyfilter != newsetting) { economyfilter = newsetting; saver.PutSetting(dbEC, newsetting); Draw(); } });

            var services = StationDefinitions.ServiceTypes.Select(x=>x.Value).Distinct().Select(x=> new ExtendedControls.CheckedIconListBoxFormGroup.StandardOption(x,x));

            extButtonServices.InitAllNoneAllBack(services,
                servicesfilter = saver.GetSetting(dbSV, ExtendedControls.CheckedIconListBoxFormGroup.Disabled),
                (newsetting) => { if (servicesfilter != newsetting) { servicesfilter = newsetting; saver.PutSetting(dbSV, newsetting); Draw(); } });

       ///     saver.DGVLoadColumnLayout(dataGridView);
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

        public void UpdateDefaultSystem(ISystem sys)
        {
            defaultsystem = sys;
            if (explicity_set_system == false)
                GetStationData();
        }
        
        public void Display(ISystem sys)
        {
            defaultsystem = sys;
            explicity_set_system = false;
            GetStationData();
        }

        private async void GetStationData()
        {
            // if explicity set, must have chars
            ISystem sys = explicity_set_system ? new SystemClass(extTextBoxAutoCompleteSystem.Text) : defaultsystem;

            System.Diagnostics.Debug.WriteLine($"Spansh station kick with min {valueBoxMaxLs.Value} at {sys.Name}");

            extTextBoxAutoCompleteSystem.TextNoChange = sys.Name;       // replace with system actually displayed, no text change
            extTextBoxAutoCompleteSystem.ClearOnFirstChar = true;       // reset so we can clear on next text input

            EliteDangerousCore.Spansh.SpanshClass sp = new EliteDangerousCore.Spansh.SpanshClass();
            stationdata = await sp.GetStationsByDumpAsync(sys);
            Draw();
        }

        public void Draw()
        {
            DataGridViewColumn sortcolprev = dataGridView.SortedColumn != null ? dataGridView.SortedColumn : dataGridView.Columns[0];
            SortOrder sortorderprev = dataGridView.SortedColumn != null ? dataGridView.SortOrder : SortOrder.Ascending;

            dataViewScrollerPanel.Suspend();
            dataGridView.Rows.Clear();

            if (stationdata != null)
            {
                foreach (var station in stationdata)
                {
                    string stationtype = station.StationType ?? "Unknown";

                    var tis = typefilter.SplitNoEmptyStrings(';');

                    bool filterin = station.DistanceToArrival <= valueBoxMaxLs.Value;
                    if (typefilter != ExtendedControls.CheckedIconListBoxFormGroup.Disabled)
                        filterin &= typefilter.HasChars() && typefilter.SplitNoEmptyStartFinish(extButtonType.SettingsSplittingChar).Contains(stationtype, StringComparison.InvariantCultureIgnoreCase) >= 0;
                    if ( commdfilter != ExtendedControls.CheckedIconListBoxFormGroup.Disabled)
                        filterin &= commdfilter.HasChars() && station.HasAnyItemToBuy(commdfilter.SplitNoEmptyStartFinish(extButtonCommodities.SettingsSplittingChar));
                    if (outfittingfilter != ExtendedControls.CheckedIconListBoxFormGroup.Disabled)
                        filterin &= outfittingfilter.HasChars() && station.HasAnyModuleTypes(outfittingfilter.SplitNoEmptyStartFinish(extButtonOutfitting.SettingsSplittingChar));
                    if (shipsfilter != ExtendedControls.CheckedIconListBoxFormGroup.Disabled)
                        filterin &= shipsfilter.HasChars() && station.HasAnyShipTypes(shipsfilter.SplitNoEmptyStartFinish(extButtonShipyard.SettingsSplittingChar));
                    if ( economyfilter != ExtendedControls.CheckedIconListBoxFormGroup.Disabled)
                        filterin &= economyfilter.HasChars() && station.HasAnyEconomyTypes(economyfilter.SplitNoEmptyStartFinish(extButtonEconomy.SettingsSplittingChar));
                    if ( servicesfilter != ExtendedControls.CheckedIconListBoxFormGroup.Disabled)
                        filterin &= servicesfilter.HasChars() && station.HasAnyServicesTypes(servicesfilter.SplitNoEmptyStartFinish(extButtonServices.SettingsSplittingChar));


                    if (filterin)
                    {
                        string ss = station.StationServices != null ? string.Join(", ", station.StationServices) : "";
                        object[] cells = new object[]
                        {
                            station.BodyName.ReplaceIfStartsWith(station.System.Name),
                            station.StationName,
                            station.DistanceToArrival > 0 ? station.DistanceToArrival.ToString("N1") : "",
                            stationtype,
                            station.Latitude.HasValue ? station.Latitude.Value.ToString("N4") : "",
                            station.Longitude.HasValue ? station.Longitude.Value.ToString("N4") : "",
                            station.HasMarket ? "\u2713" : "",
                            station.OutFitting != null ? "\u2713" : "",
                            station.Shipyard != null ? "\u2713" : "",
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
                    }
                }
            }

            dataGridView.Sort(sortcolprev, (sortorderprev == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
            dataGridView.Columns[sortcolprev.Index].HeaderCell.SortGlyphDirection = sortorderprev;

            dataViewScrollerPanel.Resume();

        }

        private void dataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            EliteDangerousCore.JournalEvents.StationInfo si = dataGridView.RightClickRowValid ? dataGridView.Rows[dataGridView.RightClickRow].Tag as EliteDangerousCore.JournalEvents.StationInfo : null;
            viewMarketToolStripMenuItem.Enabled = si?.Market?.Count > 0;
            viewOutfittingToolStripMenuItem.Enabled = si?.OutFitting?.Count > 0;
            viewShipyardToolStripMenuItem.Enabled = si?.Shipyard?.Count > 0;
            viewOnSpanshToolStripMenuItem.Enabled = si?.MarketID.HasValue ?? false;
        }

        private void viewOnSpanshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.JournalEvents.StationInfo si = dataGridView.Rows[dataGridView.RightClickRow].Tag as EliteDangerousCore.JournalEvents.StationInfo;
            EliteDangerousCore.Spansh.SpanshClass.LaunchBrowserForStationByMarketID(si.MarketID.Value);
        }

        private void viewMarketToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.JournalEvents.StationInfo si = dataGridView.Rows[dataGridView.RightClickRow].Tag as EliteDangerousCore.JournalEvents.StationInfo;

            var dgvpanel = new ExtendedControls.ExtPanelDataGridViewScrollWithDGV<BaseUtils.DataGridViewColumnControl>();
            dgvpanel.DataGrid.CreateTextColumns("Category", 100, 5,
                                                "Name", 150, 5,
                                                "Buy", 50, 5,
                                                "Stock", 50, 5,
                                                "Sell", 50, 5
                                                );

            dgvpanel.DataGrid.SortCompare += (s, ev) => { if (ev.Column.Index >= 2) ev.SortDataGridViewColumnNumeric(); };
            dgvpanel.DataGrid.RowHeadersVisible = false;

            saver.DGVLoadColumnLayout(dgvpanel.DataGrid, "ShowMarket");

            foreach (var commd in si.Market)
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

            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();
            f.Add(new ExtendedControls.ConfigurableForm.Entry(dgvpanel, "Grid", "", new System.Drawing.Point(3, 30), new System.Drawing.Size(800, 400), null)
            { Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom });
            f.AddOK(new Point(800 - 100, 460), "OK", anchor: AnchorStyles.Right | AnchorStyles.Bottom);
            f.InstallStandardTriggers();
            f.AllowResize = true;

            string title = "Materials/Commodities for ".T(EDTx.UserControlFactions_MaterialCommodsFor) + si.StationName + " " + si.MarketUpdateUTC.ToString();
            f.ShowDialogCentred(FindForm(), FindForm().Icon, title, closeicon: true);

            saver.DGVSaveColumnLayout(dgvpanel.DataGrid, "ShowMarket");

        }

        private void viewOutfittingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.JournalEvents.StationInfo si = dataGridView.Rows[dataGridView.RightClickRow].Tag as EliteDangerousCore.JournalEvents.StationInfo;

            var dgvpanel = new ExtendedControls.ExtPanelDataGridViewScrollWithDGV<BaseUtils.DataGridViewColumnControl>();
            dgvpanel.DataGrid.CreateTextColumns("Category", 100, 5,
                                                "Name", 150, 5);

            dgvpanel.DataGrid.RowHeadersVisible = false;

            saver.DGVLoadColumnLayout(dgvpanel.DataGrid, "ShowOutfitting");

            foreach (var oi in si.OutFitting)
            {
                object[] rowobj = { oi.ModType,
                                    oi.Name };
                var row = dgvpanel.DataGrid.RowTemplate.Clone() as DataGridViewRow;
                row.CreateCells(dgvpanel.DataGrid, rowobj);
                dgvpanel.DataGrid.Rows.Add(row);
            }

            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();
            f.Add(new ExtendedControls.ConfigurableForm.Entry(dgvpanel, "Grid", "", new System.Drawing.Point(3, 30), new System.Drawing.Size(800, 400), null)
            { Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom });
            f.AddOK(new Point(800 - 100, 460), "OK", anchor: AnchorStyles.Right | AnchorStyles.Bottom);
            f.InstallStandardTriggers();
            f.AllowResize = true;

            string title = "Outfitting for " + si.StationName + " " + si.OutfittingUpdateUTC.ToString();
            f.ShowDialogCentred(FindForm(), FindForm().Icon, title, closeicon: true);

            saver.DGVSaveColumnLayout(dgvpanel.DataGrid, "ShowOutfitting");

        }

        private void viewShipyardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.JournalEvents.StationInfo si = dataGridView.Rows[dataGridView.RightClickRow].Tag as EliteDangerousCore.JournalEvents.StationInfo;

            var dgvpanel = new ExtendedControls.ExtPanelDataGridViewScrollWithDGV<BaseUtils.DataGridViewColumnControl>();
            dgvpanel.DataGrid.CreateTextColumns("Name", 100, 5);

            dgvpanel.DataGrid.RowHeadersVisible = false;

            saver.DGVLoadColumnLayout(dgvpanel.DataGrid, "ShowShipyard");

            foreach (var oi in si.Shipyard)
            {
                object[] rowobj = { oi.ShipType_Localised,
                                    };
                var row = dgvpanel.DataGrid.RowTemplate.Clone() as DataGridViewRow;
                row.CreateCells(dgvpanel.DataGrid, rowobj);
                dgvpanel.DataGrid.Rows.Add(row);
            }

            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();
            f.Add(new ExtendedControls.ConfigurableForm.Entry(dgvpanel, "Grid", "", new System.Drawing.Point(3, 30), new System.Drawing.Size(800, 400), null)
            { Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom });
            f.AddOK(new Point(800 - 100, 460), "OK", anchor: AnchorStyles.Right | AnchorStyles.Bottom);
            f.InstallStandardTriggers();
            f.AllowResize = true;

            string title = "Shipyard for " + si.StationName + " " + si.ShipyardUpdateUTC.ToString();
            f.ShowDialogCentred(FindForm(), FindForm().Icon, title, closeicon: true);

            saver.DGVSaveColumnLayout(dgvpanel.DataGrid, "ShowShipyard");
        }

        private void dataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column == colBodyName)
                e.SortDataGridViewColumnAlphaInt();
            else if (e.Column == colDistance)
                e.SortDataGridViewColumnNumeric();
        }

        private void valueBoxMaxLs_ValueChanged(object sender, EventArgs e)
        {
            Draw();
        }

        private void extCheckBoxWordWrap_Click(object sender, EventArgs e)
        {
            saver.PutSetting(dbWordWrap, extCheckBoxWordWrap.Checked);
            UpdateWordWrap();
        }

        private void UpdateWordWrap()
        {
            dataGridView.SetWordWrap(extCheckBoxWordWrap.Checked);
            dataViewScrollerPanel.UpdateScroll();
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

    }
}
