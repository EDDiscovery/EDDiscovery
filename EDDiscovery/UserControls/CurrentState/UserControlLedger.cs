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
            cfs.AddAllNone();
            cfs.AddGroupOption(cashtype, "Cash Transactions".T(EDTx.UserControlLedger_CashTransactions),  JournalEntry.JournalTypeIcons[JournalTypeEnum.Bounty]);
            cfs.AddJournalEntries(new string[] { "Ledger", "LedgerNC" });
            cfs.AddUserGroups(GetSetting(dbUserGroups, ""));
            cfs.SaveSettings += EventFilterChanged;

            extCheckBoxWordWrap.Checked = GetSetting(dbWordWrap, true);
            UpdateWordWrap();
            extCheckBoxWordWrap.Click += extCheckBoxWordWrap_Click;

            discoveryform.OnHistoryChange += Redisplay;
            discoveryform.OnNewEntry += NewEntry;

            var enumlist = new Enum[] { EDTx.UserControlLedger_TimeCol, EDTx.UserControlLedger_Type, EDTx.UserControlLedger_Notes, EDTx.UserControlLedger_Credits, EDTx.UserControlLedger_Debits, EDTx.UserControlLedger_Balance, EDTx.UserControlLedger_NormProfit, EDTx.UserControlLedger_TotalProfit, EDTx.UserControlLedger_labelTime, EDTx.UserControlLedger_labelSearch };
            var enumlistcms = new Enum[] { EDTx.UserControlLedger_toolStripMenuItemGotoItem };
            var enumlisttt = new Enum[] { EDTx.UserControlLedger_comboBoxTime_ToolTip, EDTx.UserControlLedger_textBoxFilter_ToolTip, EDTx.UserControlLedger_buttonFilter_ToolTip, EDTx.UserControlLedger_buttonExtExcel_ToolTip , EDTx.UserControlLedger_extCheckBoxWordWrap_ToolTip };

            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);
            BaseUtils.Translator.Instance.TranslateToolstrip(contextMenuStrip, enumlistcms, this);
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);

            TravelHistoryFilter.InitaliseComboBox(comboBoxTime, GetSetting(dbHistorySave,""), incldockstartend: false);

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

                extChartLedger.YAutoScale();
                extChartLedger.SetYAxisFormat("N0");

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

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg = thc;
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
            PutSetting(dbUserGroups, cfs.GetUserGroupDefinition(1));
            discoveryform.OnHistoryChange -= Redisplay;
            discoveryform.OnNewEntry -= NewEntry;
        }

        #endregion

        #region Display Grid

        private void Redisplay(HistoryList hl)
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

            var ledger = discoveryform.history.CashLedger;
            transactioncountatdisplay = 0;
            
            if (ledger != null && ledger.Transactions.Count > 0)
            {
                var filter = (TravelHistoryFilter)comboBoxTime.SelectedItem ?? TravelHistoryFilter.NoFilter;
                List<Ledger.Transaction> filteredlist = filter.Filter(ledger.Transactions);

                if (filteredlist.Count > 0)
                {
                    var eventfilter = GetSetting(dbFilter, "All").Split(';').ToHashSet();

                    var rowsToAdd = new List<DataGridViewRow>(filteredlist.Count);
                 
                    for (int i = filteredlist.Count - 1; i >= 0; i--)
                    {
                        var row = CreateRow(filteredlist[i], eventfilter, textBoxFilter.Text);      // create if not filtered out

                        if (row != null)
                        {
                            rowsToAdd.Add(row);
                            DateTime seltime = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(filteredlist[i].EventTimeUTC);
                            extChartLedger.AddXY(seltime, filteredlist[i].CashTotal, graphtooltip: $"{seltime.ToString()} {filteredlist[i].CashTotal:N0}cr" );
                        }
                    }

                    dataGridViewLedger.Rows.AddRange(rowsToAdd.ToArray());
                }

                transactioncountatdisplay = ledger.Transactions.Count;
            }

            dataGridViewLedger.Columns[0].HeaderText = EDDConfig.Instance.GetTimeTitle();
            dataGridViewLedger.Sort(sortcol, (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
            dataGridViewLedger.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;

            var tx1 = ledger.TransactionBefore(DateTime.UtcNow.AddHours(-24));
            label24h.Text = tx1 != null ? ("24h: " + (ledger.CashTotal - tx1.CashTotal).ToString("N0") + "cr") : "";
            var tx2 = ledger.TransactionBefore(DateTime.UtcNow.AddDays(-7));
            label7d.Text = tx2 != null ? ("7d: " + (ledger.CashTotal - tx2.CashTotal).ToString("N0") + "cr") : "";

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
                row.Cells[0].Tag = tx.EventTimeUTC;
                return row;
            }
            else
                return null;
        }

        private void NewEntry(HistoryEntry he, HistoryList hl)
        {
            while(transactioncountatdisplay < discoveryform.history.CashLedger.Transactions.Count)   // if new transaction
            {
                Ledger.Transaction tx = discoveryform.history.CashLedger.Transactions[transactioncountatdisplay];

                var eventfilter = GetSetting(dbFilter, "All").Split(';').ToHashSet();
                var row = CreateRow(tx, eventfilter, textBoxFilter.Text);

                if ( row!=null)
                {
                    dataGridViewLedger.Rows.Insert(0, row);     // insert at top
                    var seltime = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(tx.EventTimeUTC);
                    extChartLedger.AddXY(seltime, tx.CashTotal, graphtooltip: $"{seltime.ToString()} {tx.CashTotal:N0}cr");
                }

                transactioncountatdisplay++;
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
                uctg.GotoPosByJID(v);
            }
        }

        #endregion

        #region Click on grid

        private void dataGridViewLedger_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewLedger.RowCount)
            {
                var row = dataGridViewLedger.Rows[e.RowIndex];
                var datetime = (DateTime)row.Cells[0].Tag;
                System.Diagnostics.Debug.WriteLine($"Ledger Selected Graph cursor position {datetime}");
                extChartLedger.SetXCursorPosition(datetime);
            }
        }

        #endregion

        #region Chart
        private void LedgerCursorPositionChanged(ExtendedControls.ExtSafeChart chart, string chartarea, AxisName axis, double pos)
        {
            if (!double.IsNaN(pos))     // this means its off graph, ignore
            {
                DateTime dtgraph = DateTime.FromOADate(pos);                    // back to date/time
                int row = dataGridViewLedger.FindRowWithDateTagWithin((r) => (DateTime)r.Cells[0].Tag, dtgraph, long.MaxValue);  // we accept any nearest
                if (row >= 0)
                {
                    dataGridViewLedger.SetCurrentAndSelectAllCellsOnRow(row);
                    dataGridViewLedger.Rows[row].Selected = true;
                }
            }
        }

        #endregion


        #region Export

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            var current_mc = discoveryform.history.CashLedger;

            if ( current_mc != null )
            { 
                Forms.ExportForm frm = new Forms.ExportForm();

                frm.Init(false, new string[] { "Export Current View" }, showflags: new Forms.ExportForm.ShowFlags[] { Forms.ExportForm.ShowFlags.DisableDateTime });

                if (frm.ShowDialog(this.FindForm()) == DialogResult.OK)
                {
                    BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid();
                    grd.SetCSVDelimiter(frm.Comma);

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
