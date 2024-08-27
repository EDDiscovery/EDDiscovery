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
 * 
 */
using EDDiscovery.Controls;
using EliteDangerousCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace EDDiscovery.UserControls
{
    public partial class UserControlLedger : UserControlCommonBase
    {
        private JournalFilterSelector cfs;
        private const string dbFilter = "EventFilter2";
        private const string dbHistorySave = "EDUIHistory";
        private const string dbSCLedger = "SCLedger";
        private const string dbWordWrap = "WordWrap";
        private const string dbUserGroups = "UserGroups";
        private int transactioncountatdisplay = 0;

        #region Init

        public UserControlLedger()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "LedgerGrid";

            dataGridViewLedger.MakeDoubleBuffered();

            var jes = EliteDangerousCore.JournalEntry.GetNameImageOfEvents(new string[] { "Ledger" });
            string cashtype = string.Join(";", jes.Select(x=>x.Item1) ) + ";";

            cfs = new JournalFilterSelector();
            cfs.UC.AddAllNone();
            cfs.UC.AddGroupItem(cashtype, "Cash Transactions".T(EDTx.UserControlLedger_CashTransactions),  JournalEntry.JournalTypeIcons[JournalTypeEnum.Bounty]);
            cfs.AddJournalEntries(new string[] { "Ledger", "LedgerNC" });
            cfs.AddUserGroups(GetSetting(dbUserGroups, ""));
            cfs.SaveSettings += EventFilterChanged;

            extCheckBoxWordWrap.Checked = GetSetting(dbWordWrap, true);
            UpdateWordWrap();
            extCheckBoxWordWrap.Click += extCheckBoxWordWrap_Click;

            checkBoxCursorToTop.Checked = true;

            DiscoveryForm.OnHistoryChange += Redisplay;
            DiscoveryForm.OnNewEntry += OnNewEntry;

            var enumlist = new Enum[] { EDTx.UserControlLedger_TimeCol, EDTx.UserControlLedger_Type, EDTx.UserControlLedger_Notes, 
                                    EDTx.UserControlLedger_Credits, EDTx.UserControlLedger_Debits, EDTx.UserControlLedger_Balance, 
                                    EDTx.UserControlLedger_NormProfit, EDTx.UserControlLedger_TotalProfit, EDTx.UserControlLedger_labelTime, 
                                    EDTx.UserControlLedger_labelSearch };
            var enumlistcms = new Enum[] { EDTx.UserControlLedger_toolStripMenuItemGotoItem };
            var enumlisttt = new Enum[] { EDTx.UserControlLedger_comboBoxTime_ToolTip, EDTx.UserControlLedger_textBoxFilter_ToolTip, 
                        EDTx.UserControlLedger_buttonFilter_ToolTip, EDTx.UserControlLedger_buttonExtExcel_ToolTip , EDTx.UserControlLedger_extCheckBoxWordWrap_ToolTip,
                        EDTx.UserControlLedger_checkBoxCursorToTop_ToolTip};

            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);
            BaseUtils.Translator.Instance.TranslateToolstrip(contextMenuStrip, enumlistcms, this);
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);

            TravelHistoryFilter.InitialiseComboBox(comboBoxTime, GetSetting(dbHistorySave,""), false,true,false);

            splitContainerLedger.SplitterDistance(GetSetting(dbSCLedger, 0.5));

            {                                                                               // same code as in carrier/stats really
                extChartLedger.AddChartArea("LedgerCA1");
                extChartLedger.AddSeries("LedgerS1", "LedgerCA1", SeriesChartType.Line);
                extChartLedger.EnableZoomMouseWheelX();
                extChartLedger.ZoomMouseWheelXMinimumInterval = 5.0 / 60.0 / 24.0;

                extChartLedger.SetXAxisInterval(DateTimeIntervalType.Days, 0, IntervalAutoMode.VariableCount);
                extChartLedger.SetXAxisFormat("g");

                extChartLedger.XCursorShown();
                extChartLedger.XCursorSelection();
                extChartLedger.SetXCursorInterval(1, DateTimeIntervalType.Seconds);

                extChartLedger.YAutoScale();        // turn on Y autoscale
                extChartLedger.SetYAxisFormat("N0");
                extChartLedger.IsStartedFromZeroY = false;

                extChartLedger.ShowSeriesMarkers(MarkerStyle.Diamond);

                extChartLedger.AddContextMenu(new string[] { "Zoom out by 1", "Reset Zoom" },
                                    new Action<ToolStripMenuItem>[]
                                        { new Action<ToolStripMenuItem>((s)=> {extChartLedger.ZoomOutX(); } ),
                                              new Action<ToolStripMenuItem>((s)=> {extChartLedger.ZoomResetX(); } ),
                                        },
                                    new Action<ToolStripMenuItem[]>((list) =>
                                    {
                                        list[0].Enabled = list[1].Enabled = extChartLedger.IsZoomedX;
                                    })
                                    );

                extChartLedger.CursorPositionChanged = LedgerCursorPositionChanged;
            }

        }

        public override void LoadLayout()
        {
            dataGridViewLedger.RowTemplate.MinimumHeight = Font.ScalePixels(26);
            DGVLoadColumnLayout(dataGridViewLedger);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewLedger);
            PutSetting(dbSCLedger, splitContainerLedger.GetSplitterDistance());
            PutSetting(dbUserGroups, cfs.GetUserGroups());
            DiscoveryForm.OnHistoryChange -= Redisplay;
            DiscoveryForm.OnNewEntry -= OnNewEntry;
        }

        #endregion

        #region Display Grid

        private void Redisplay()
        {
            Display();
        }

        public override void InitialDisplay()
        {
            Display();
        }

        private void Display()
        {
            DataGridViewColumn sortcol = dataGridViewLedger.SortedColumn != null ? dataGridViewLedger.SortedColumn : dataGridViewLedger.Columns[0];
            SortOrder sortorder = dataGridViewLedger.SortOrder != SortOrder.None ? dataGridViewLedger.SortOrder : SortOrder.Descending;

            dataGridViewLedger.Rows.Clear();
            extChartLedger.ClearSeriesPoints();

            var ledger = DiscoveryForm.History.CashLedger;
            transactioncountatdisplay = 0;

            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCountLap("LD", true)} Ledger");
            if (ledger != null && ledger.Transactions.Count > 0)
            {
                var filter = (TravelHistoryFilter)comboBoxTime.SelectedItem ?? TravelHistoryFilter.NoFilter;
                
                // transactions are in date ascending, and filtered list is too in date ascending
                List<Ledger.Transaction> filteredlist = filter.Filter(ledger.Transactions);

                if (filteredlist.Count > 0)
                {
                    var eventfilter = GetSetting(dbFilter, "All").Split(';').ToHashSet();

                    var rowsToAdd = new List<DataGridViewRow>(filteredlist.Count);
                    bool[] filteredout = new bool[filteredlist.Count];

                    for (int i = filteredlist.Count - 1; i >= 0; i--)       // fill the rows backwards, oldest first
                    {
                        var row = CreateRow(filteredlist[i], eventfilter, textBoxFilter.Text);      // create if not filtered out
                        if (row != null)
                            rowsToAdd.Add(row);         // add..
                        else
                            filteredout[i] = true;      // remember
                    }

                    dataGridViewLedger.Rows.AddRange(rowsToAdd.ToArray());

                    extChartLedger.BeginInit();

                    for (int i = 0; i < filteredlist.Count;  i++)       // chart is filled in date ascending order
                    {
                        if (!filteredout[i])        // if not filtered out, add
                        {
                            DateTime seltime = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(filteredlist[i].EventTimeUTC);
                            //System.Diagnostics.Debug.WriteLine($"Ledger Chart add {seltime} {seltime.ToOADate()} {filteredlist[i].CashTotal}");
                            extChartLedger.AddXY(seltime, filteredlist[i].CashTotal);   // purposely no chart tips - uses too much space, no need with grid reflection
                        }
                    }

                    extChartLedger.EndInit();
                }

                transactioncountatdisplay = ledger.Transactions.Count;
            }

            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCountLap("LD")} Ledger end");

            dataGridViewLedger.Columns[0].HeaderText = EDDConfig.Instance.GetTimeTitle();
            dataGridViewLedger.Sort(sortcol, (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
            dataGridViewLedger.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;

            Calculate24h7Day();
        }

        private DataGridViewRow CreateRow(Ledger.Transaction tx, HashSet<string> eventfilter, string textfilter)
        {
            if (!eventfilter.Contains(tx.EventType.ToString()) && !eventfilter.Contains("All"))        // All or event name must be in list
                return null;

            string[] rowobj = { EDDConfig.Instance.ConvertTimeToSelectedFromUTC(tx.EventTimeUTC).ToString() ,
                                            tx.EventType.ToString().SplitCapsWord(),
                                            tx.Notes,
                                            (tx.CashAdjust>0) ? tx.CashAdjust.ToString("N0") : "",
                                            (tx.CashAdjust<0) ? (-tx.CashAdjust).ToString("N0") : "",
                                            tx.CashTotal.ToString("N0"),
                                            (tx.ProfitPerUnit!=0) ? tx.ProfitPerUnit.ToString("N0") : "",
                                            (tx.Profit!=0) ? tx.Profit.ToString("N0") : ""
                                        };

            if (!textfilter.HasChars() || rowobj.ContainsIn(textfilter, StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                var row = dataGridViewLedger.RowTemplate.Clone() as DataGridViewRow;
                row.CreateCells(dataGridViewLedger, rowobj);
                row.Tag = tx.JID;
                row.Cells[0].Tag = tx;
                return row;
            }
            else
                return null;
        }

        private void OnNewEntry(HistoryEntry he)
        {
            while(transactioncountatdisplay < DiscoveryForm.History.CashLedger.Transactions.Count)   // if new transaction
            {
                Ledger.Transaction tx = DiscoveryForm.History.CashLedger.Transactions[transactioncountatdisplay];

                var eventfilter = GetSetting(dbFilter, "All").Split(';').ToHashSet();
                var row = CreateRow(tx, eventfilter, textBoxFilter.Text);

                if (row != null)
                {
                    dataGridViewLedger.Rows.Insert(0, row);     // insert at top

                    var seltime = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(tx.EventTimeUTC);
                    //System.Diagnostics.Debug.WriteLine($"Ledger Chart add {seltime} {seltime.ToOADate()} {tx.CashTotal}");
                    extChartLedger.AddXY(seltime, tx.CashTotal);

                    if (checkBoxCursorToTop.Checked) // Move focus to first row
                    {
                        dataGridViewLedger.ClearSelection();
                        dataGridViewLedger.SetCurrentAndSelectAllCellsOnRow(0);       // its the current cell which needs to be set, moves the row marker as well
                    }
                }

                transactioncountatdisplay++;
            }
        }

        private void Calculate24h7Day()
        {
            int row = dataGridViewLedger.SelectedRowAndCount(false, true, -1).Item1;
            if (row >= 0)
            {
                Ledger ledger = DiscoveryForm.History.CashLedger;
                Ledger.Transaction tx = dataGridViewLedger.Rows[row].Cells[0].Tag as Ledger.Transaction;

                //System.Diagnostics.Debug.WriteLine($"Ledger calc amount at row {row} time {tx.EventTimeUTC}");
                var tx1 = ledger.TransactionBefore(tx, new TimeSpan(24, 0, 0));
                //System.Diagnostics.Debug.WriteLine($"Ledger calc amount tx1 {tx1?.EventTimeUTC}");
                var tx2 = ledger.TransactionBefore(tx, new TimeSpan(7, 0, 0, 0));
                //System.Diagnostics.Debug.WriteLine($"Ledger calc amount tx2 {tx2?.EventTimeUTC}");

                label24h.Text = tx1 != null ? ("24h: " + (tx.CashTotal - tx1.CashTotal).ToString("N0") + "cr") : "";
                label7d.Text = tx2 != null ? ("7d: " + (tx.CashTotal - tx2.CashTotal).ToString("N0") + "cr") : "";
                //label24h.Text = tx1 != null ? ("24h: " + tx1.EventTimeUTC + " " + (ledger.CashTotal - tx1.CashTotal).ToString("N0") + "cr") : "";
                //label7d.Text = tx2 != null ? ("7d: " + tx2.EventTimeUTC + " " + (ledger.CashTotal - tx2.CashTotal).ToString("N0") + "cr") : "";
            }
        }


        private void dataGridViewLedger_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 0)
                e.SortDataGridViewColumnDate(true);
            else if (e.Column.Index >= 3)
                e.SortDataGridViewColumnNumeric();
        }

        #endregion

        #region Top UI

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            cfs.Open( GetSetting(dbFilter,"All"), b, this.FindForm());
        }

        private void EventFilterChanged(string newset, Object e)
        {
            string filters = GetSetting(dbFilter, "All");
            if (filters != newset)
            {
                PutSetting(dbFilter, newset);
                Display();
            }
        }
        private void extCheckBoxWordWrap_Click(object sender, EventArgs e)
        {
            PutSetting(dbWordWrap, extCheckBoxWordWrap.Checked);
            UpdateWordWrap();
        }

        private void UpdateWordWrap()
        {
            dataGridViewLedger.SetWordWrap(extCheckBoxWordWrap.Checked);
            dataViewScrollerPanel.UpdateScroll();
        }

        private void comboBoxHistoryWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            PutSetting(dbHistorySave, comboBoxTime.Text);
            Display();
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            Display();
        }

        #endregion

        #region right clicks on grid

        private void toolStripMenuItemGotoItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewLedger.RightClickRow != -1)
            {
                long v = (long)dataGridViewLedger.Rows[dataGridViewLedger.RightClickRow].Tag;

                if (RequestPanelOperation(this, new UserControlCommonBase.RequestTravelToJID() { JID = v, MakeVisible = true }) == PanelActionState.Failed)
                    ExtendedControls.MessageBoxTheme.Show(DiscoveryForm, "Entry is filtered out of grid".TxID(EDTx.UserControlTravelGrid_entryfilteredout), "Warning".TxID(EDTx.Warning));
            }
        }

        #endregion

        #region Grid interaction

        bool movingpos = false;     // prevent changing the ledger row causing a call back to chart cursor pos change, and visa versa

        private void dataGridViewLedger_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!movingpos && e.RowIndex >= 0 && e.RowIndex < dataGridViewLedger.RowCount)        // triage actually clicking on row
            {
                Ledger.Transaction tx = dataGridViewLedger.Rows[e.RowIndex].Cells[0].Tag as Ledger.Transaction;
                // bug here, found by Ealhstan. Graph is in user time base, need to convert utc->user time
                var usertime = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(tx.EventTimeUTC);

                //System.Diagnostics.Debug.WriteLine($"Ledger Selected Graph cursor position utc {tx.EventTimeUTC} user {usertime}");
                movingpos = true;
                extChartLedger.SetXCursorPosition(usertime);
                movingpos = false;
                //System.Diagnostics.Debug.WriteLine($".. end Selected Graph cursor position utc {tx.EventTimeUTC} user {usertime}");

                Calculate24h7Day();
            }

        }

        #endregion

        #region Chart
        private void LedgerCursorPositionChanged(ExtendedControls.ExtSafeChart chart, string chartarea, AxisName axis, double pos)
        {
            if (!movingpos && !double.IsNaN(pos))     // this means its off graph, ignore. time is a double based on OLE automation Date!
            {
                var oadate = DateTime.FromOADate(pos);                                  // from pos-> Date
                var seldate = EDDConfig.Instance.ConvertTimeToSelected(oadate);         // Make sure its in selected kind (Local or UTC)
                var utc = EDDConfig.Instance.ConvertTimeToUTCFromSelected(seldate);     // convert to UTC

                int row = dataGridViewLedger.FindRowWithDateTagWithin((r) => ((Ledger.Transaction)r.Cells[0].Tag).EventTimeUTC, utc, long.MaxValue);  // we accept any nearest
               // System.Diagnostics.Debug.WriteLine($"Ledger Chart Selected {pos} correspondion to {seldate} utc {utc} = row {row}");
                if (row >= 0)
                {
                    movingpos = true;
                    dataGridViewLedger.SetCurrentAndSelectAllCellsOnRow(row);
                    dataGridViewLedger.Rows[row].Selected = true;
                    movingpos = false;
                    Calculate24h7Day();
                }
               // System.Diagnostics.Debug.WriteLine($".. end Chart Selected {pos} correspondion to {seldate} utc {utc} = row {row}");
            }
        }

        #endregion


        #region Export

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            var current_mc = DiscoveryForm.History.CashLedger;

            if ( current_mc != null )
            { 
                Forms.ImportExportForm frm = new Forms.ImportExportForm();

                frm.Export( new string[] { "Export Current View" }, new Forms.ImportExportForm.ShowFlags[] { Forms.ImportExportForm.ShowFlags.ShowCSVOpenInclude });

                if (frm.ShowDialog(this.FindForm()) == DialogResult.OK)
                {
                    BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid(frm.Delimiter);

                    grd.GetHeader += delegate (int c)
                    {
                        return (frm.IncludeHeader && c < dataGridViewLedger.ColumnCount) ? dataGridViewLedger.Columns[c].HeaderText : null;
                    };

                    grd.GetLine += delegate (int r)
                    {
                        if (r < dataGridViewLedger.RowCount)
                        {
                            DataGridViewRow rw = dataGridViewLedger.Rows[r];
                            return new Object[] { rw.Cells[0].Value, rw.Cells[1].Value, rw.Cells[2].Value, rw.Cells[3].Value, rw.Cells[4].Value, rw.Cells[5].Value, rw.Cells[6].Value};
                        }
                        else
                            return null;
                    };

                    grd.WriteGrid(frm.Path, frm.AutoOpen, FindForm());
                }
            }
            else
                ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "No Ledger available".T(EDTx.UserControlLedger_NOLG), "Warning".T(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        #endregion

    }
}
