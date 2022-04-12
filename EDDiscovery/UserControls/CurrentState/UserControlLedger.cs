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

namespace EDDiscovery.UserControls
{
    public partial class UserControlLedger : UserControlCommonBase
    {
        private JournalFilterSelector cfs;
        private string dbFilter = "EventFilter2";
        private string dbHistorySave = "EDUIHistory";
        private const string dbWordWrap = "WordWrap";
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
            dataGridViewLedger.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            var jes = EliteDangerousCore.JournalEntry.GetNameImageOfEvents(new string[] { "Ledger" });
            string cashtype = string.Join(";", jes.Select(x=>x.Item1) ) + ";";

            cfs = new JournalFilterSelector();
            cfs.AddAllNone();
            cfs.AddGroupOption(cashtype, "Cash Transactions".T(EDTx.UserControlLedger_CashTransactions),  JournalEntry.JournalTypeIcons[JournalTypeEnum.Bounty]);
            cfs.AddJournalEntries(new string[] { "Ledger", "LedgerNC" });
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
            discoveryform.OnHistoryChange -= Redisplay;
            discoveryform.OnNewEntry -= NewEntry;
        }

        #endregion

        #region Display

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

            var mc = discoveryform.history.CashLedger;
            transactioncountatdisplay = 0;
            
            if (mc != null && mc.Transactions.Count > 0)
            {
                var filter = (TravelHistoryFilter)comboBoxTime.SelectedItem ?? TravelHistoryFilter.NoFilter;
                List<Ledger.Transaction> filteredlist = filter.Filter(mc.Transactions);

                filteredlist = FilterByJournalEvent(filteredlist, GetSetting(dbFilter, "All"));

                if (filteredlist.Count > 0)
                {
                    var rowsToAdd = new List<DataGridViewRow>(filteredlist.Count);
                    for (int i = filteredlist.Count - 1; i >= 0; i--)
                    {
                        Ledger.Transaction tx = filteredlist[i];
                        rowsToAdd.Add(CreateRow(tx));
                    }

                    dataGridViewLedger.Rows.AddRange(rowsToAdd.ToArray());

                    dataGridViewLedger.FilterGridView(textBoxFilter.Text);
                }

                transactioncountatdisplay = mc.Transactions.Count;
            }

            dataGridViewLedger.Columns[0].HeaderText = EDDConfig.Instance.GetTimeTitle();
            dataGridViewLedger.Sort(sortcol, (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
            dataGridViewLedger.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;
        }

        private DataGridViewRow CreateRow(Ledger.Transaction tx)
        {
            object[] rowobj = { EDDConfig.Instance.ConvertTimeToSelectedFromUTC(tx.utctime) ,
                                            tx.jtype.ToString().SplitCapsWord(),
                                            tx.notes,
                                            (tx.cashadjust>0) ? tx.cashadjust.ToString("N0") : "",
                                            (tx.cashadjust<0) ? (-tx.cashadjust).ToString("N0") : "",
                                            tx.cash.ToString("N0"),
                                            (tx.profitperunit!=0) ? tx.profitperunit.ToString("N0") : "",
                                            (tx.profit!=0) ? tx.profit.ToString("N0") : ""
                                        };

            var row = dataGridViewLedger.RowTemplate.Clone() as DataGridViewRow;
            row.CreateCells(dataGridViewLedger, rowobj);
            row.Tag = tx.jid;
            return row;
        }

        private void NewEntry(HistoryEntry he, HistoryList hl)
        {
            while(transactioncountatdisplay < discoveryform.history.CashLedger.Transactions.Count)   // if new transaction
            {
                Ledger.Transaction tx = discoveryform.history.CashLedger.Transactions[transactioncountatdisplay];

                string evstring = GetSetting(dbFilter, "All");

                if ( evstring.Equals("All") || tx.IsJournalEventInEventFilter(evstring.Split(';')))     // if in filter..
                {
                    var row = CreateRow(tx);
                    bool visible = false;

                    if (textBoxFilter.Text.HasChars())      // if we are text filtering..
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            if (cell.Value != null)
                            {
                                if (cell.Value.ToString().IndexOf(textBoxFilter.Text, 0, StringComparison.CurrentCultureIgnoreCase) >= 0)
                                {
                                    visible = true;
                                    break;
                                }
                            }
                        }
                    }
                    else
                        visible = true;

                    row.Visible = visible;                      // searching on this panel sets the visibility flag
                    dataGridViewLedger.Rows.Insert(0, row);     // insert at top
                }

                transactioncountatdisplay++;
            }
        }

        public List<Ledger.Transaction> FilterByJournalEvent(List<Ledger.Transaction> txlist, string eventstring)
        {
            if (eventstring.Equals("All"))
                return txlist;
            else
            {
                string[] events = eventstring.Split(';');
                return (from tx in txlist where tx.IsJournalEventInEventFilter(events) select tx).ToList();
            }
        }

        #endregion

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
            dataGridViewLedger.DefaultCellStyle.WrapMode = extCheckBoxWordWrap.Checked ? DataGridViewTriState.True : DataGridViewTriState.False;
            dataGridViewLedger.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dataViewScrollerPanel.UpdateScroll();
        }


        private void comboBoxHistoryWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            PutSetting(dbHistorySave, comboBoxTime.Text);
            Display();
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            dataGridViewLedger.FilterGridView(textBoxFilter.Text);
        }

        #region right clicks

        private void toolStripMenuItemGotoItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewLedger.RightClickRow != -1)
            {
                long v = (long)dataGridViewLedger.Rows[dataGridViewLedger.RightClickRow].Tag;

                uctg.GotoPosByJID(v);
            }
        }

        #endregion

        private void dataGridViewLedger_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 0)
                e.SortDataGridViewColumnDate(true);
            else if (e.Column.Index >= 3)
                e.SortDataGridViewColumnNumeric();
        }

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
    }
}
