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
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlSpanshStations : UserControlCommonBase
    {
        private HistoryEntry last_he = null;

        public UserControlSpanshStations()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "SpanshStations";

            //var enumlist = new Enum[] { EDTx.UserControlStarDistance_colName, EDTx.UserControlStarDistance_colDistance, EDTx.UserControlStarDistance_colVisited, 
            //    EDTx.UserControlStarDistance_labelExtMin, EDTx.UserControlStarDistance_labelExtMax, EDTx.UserControlStarDistance_checkBoxCube };
            //var enumlistcms = new Enum[] { EDTx.UserControlStarDistance_viewSystemToolStripMenuItem, EDTx.UserControlStarDistance_viewOnEDSMToolStripMenuItem1,
            //    EDTx.UserControlStarDistance_viewOnSpanshToolStripMenuItem,
            //    EDTx.UserControlStarDistance_addToTrilaterationToolStripMenuItem1, EDTx.UserControlStarDistance_addToExpeditionToolStripMenuItem };
            //var enumlisttt = new Enum[] { EDTx.UserControlStarDistance_textMinRadius_ToolTip, EDTx.UserControlStarDistance_textMaxRadius_ToolTip, 
            //    EDTx.UserControlStarDistance_checkBoxCube_ToolTip };
            //BaseUtils.Translator.Instance.TranslateControls(this, enumlist);
            //BaseUtils.Translator.Instance.TranslateToolstrip(contextMenuStrip, enumlistcms, this);
            //BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);

            colOutfitting.DefaultCellStyle.Alignment =
            colShipyard.DefaultCellStyle.Alignment = 
            colHasMarket.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        public override void LoadLayout()
        {
           // DGVLoadColumnLayout(dataGridView);
            DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange;
            //valueBoxMaxLs.Value = GetSetting("MaxLs", 1000000);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridView);
            DiscoveryForm.OnHistoryChange -= Discoveryform_OnHistoryChange;
            PutSetting("Maxls", valueBoxMaxLs.Value);
        }

        public override void InitialDisplay()
        {
            RequestPanelOperation(this, new UserControlCommonBase.RequestTravelHistoryPos());     //request an update 
        }

        private void Discoveryform_OnHistoryChange()
        {
            KickComputation(DiscoveryForm.History.GetLast);   // copes with getlast = null
        }

        public override void ReceiveHistoryEntry(HistoryEntry he)
        {
            KickComputation(he);
        }

        private async void KickComputation(HistoryEntry he, bool force = false)
        {
            if (he?.System != null && (force || !he.System.Equals(last_he?.System)))
            {
                last_he = he;

                System.Diagnostics.Debug.WriteLine($"Spansh station kick with min {valueBoxMaxLs.Value} at {he.System.Name}");

                EliteDangerousCore.Spansh.SpanshClass sp = new EliteDangerousCore.Spansh.SpanshClass();



                SystemClass sol = new SystemClass("Sol", 10477373803);

                //var res = await sp.GetStationsByDumpAsync(he.System, valueBoxMaxLs.Value, false);
                var res = await sp.GetStationsByDumpAsync(sol, valueBoxMaxLs.Value, false);

                FillGrid(res);
            }
        }

        private void FillGrid(List<EliteDangerousCore.JournalEvents.StationInfo> res)
        {
            DataGridViewColumn sortcolprev = dataGridView.SortedColumn != null ? dataGridView.SortedColumn : dataGridView.Columns[0];
            SortOrder sortorderprev = dataGridView.SortedColumn != null ? dataGridView.SortOrder : SortOrder.Ascending;

            dataViewScrollerPanel.Suspend();
            dataGridView.Rows.Clear();

            if ( res != null )
            {
                foreach( var station in res)
                {
                    object[] cells = new object[]
                    {
                        station.BodyName.ReplaceIfStartsWith(station.System.Name),
                        station.StationName,
                        station.DistanceToArrival > 0 ? station.DistanceToArrival.ToString() : "",
                        station.StationType ?? "",
                        station.Latitude.HasValue ? station.Latitude.Value.ToString("N4") : "",
                        station.Longitude.HasValue ? station.Longitude.Value.ToString("N4") : "",
                        station.HasMarket ? "\u2713" : "",
                        station.OutFitting != null ? "\u2713" : "",
                        station.Shipyard != null ? "\u2713" : "",
                        station.Allegiance ?? "",
                        station.Economy ?? "",
                        station.Government ?? "",
                        station.StationServices != null ? string.Join(",",station.StationServices) : "",
                        station.LandingPads?.Small.ToString() ?? "",
                        station.LandingPads?.Medium.ToString() ?? "",
                        station.LandingPads?.Large.ToString() ?? "",
                    };


                    var rw = dataGridView.RowTemplate.Clone() as DataGridViewRow;
                    rw.CreateCells(dataGridView, cells);
                    rw.Tag = station;
                    dataGridView.Rows.Add(rw);
                }
            }

            dataGridView.Sort(sortcolprev, (sortorderprev == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
            dataGridView.Columns[sortcolprev.Index].HeaderCell.SortGlyphDirection = sortorderprev;

            dataViewScrollerPanel.Resume();
        }

        private void dataGridViewNearest_MouseDown(object sender, MouseEventArgs e)
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
            
            DGVLoadColumnLayout(dgvpanel.DataGrid, "ShowMarket");

            foreach( var commd in si.Market)
            {
                object[] rowobj = { commd.loccategory,
                                    commd.locName,
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

            string title = "Materials/Commodities for ".T(EDTx.UserControlFactions_MaterialCommodsFor) + si.StationName;
            f.ShowDialogCentred(FindForm(), FindForm().Icon, title, closeicon: true);

            DGVSaveColumnLayout(dgvpanel.DataGrid, "ShowMarket");

        }

        private void viewOutfittingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.JournalEvents.StationInfo si = dataGridView.Rows[dataGridView.RightClickRow].Tag as EliteDangerousCore.JournalEvents.StationInfo;

            var dgvpanel = new ExtendedControls.ExtPanelDataGridViewScrollWithDGV<BaseUtils.DataGridViewColumnControl>();
            dgvpanel.DataGrid.CreateTextColumns("Category", 100, 5,
                                                "Name", 150, 5 );

            dgvpanel.DataGrid.RowHeadersVisible = false;

            DGVLoadColumnLayout(dgvpanel.DataGrid, "ShowOutfitting");

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

            string title = "Outfitting for " + si.StationName;
            f.ShowDialogCentred(FindForm(), FindForm().Icon, title, closeicon: true);

            DGVSaveColumnLayout(dgvpanel.DataGrid, "ShowOutfitting");

        }

        private void viewShipyardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.JournalEvents.StationInfo si = dataGridView.Rows[dataGridView.RightClickRow].Tag as EliteDangerousCore.JournalEvents.StationInfo;

            var dgvpanel = new ExtendedControls.ExtPanelDataGridViewScrollWithDGV<BaseUtils.DataGridViewColumnControl>();
            dgvpanel.DataGrid.CreateTextColumns( "Name", 100, 5);

            dgvpanel.DataGrid.RowHeadersVisible = false;

            DGVLoadColumnLayout(dgvpanel.DataGrid, "ShoShipyard");

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

            string title = "Shipyard for " + si.StationName;
            f.ShowDialogCentred(FindForm(), FindForm().Icon, title, closeicon: true);

            DGVSaveColumnLayout(dgvpanel.DataGrid, "ShowShipyard");

        }
        private void dataGridViewNearest_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column == colBodyName)
                e.SortDataGridViewColumnAlphaInt();
            else if (e.Column == colDistance)
                e.SortDataGridViewColumnNumeric();
        }

        private void textMinRadius_ValueChanged(object sender, EventArgs e)
        {
            KickComputation(last_he, true);
        }

        private void dataGridViewNearest_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.ColumnIndex == 0 && e.RowIndex >= 0 && e.RowIndex < dataGridView.Rows.Count)
            //{
            //    SetClipboardText(dataGridView.Rows[e.RowIndex].Cells[0].Value as string);
            //}
        }

        private void dataGridViewNearest_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView.Rows.Count)
            {
                // market display
            }
        }

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            if (last_he == null)
                return;

            Forms.ImportExportForm frm = new Forms.ImportExportForm();
            frm.Export( new string[] { "View" },
                            new Forms.ImportExportForm.ShowFlags[] { Forms.ImportExportForm.ShowFlags.ShowCSVOpenInclude, Forms.ImportExportForm.ShowFlags.None },
                            new string[] { "CSV|*.csv"},
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
