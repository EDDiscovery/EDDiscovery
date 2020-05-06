/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EDDiscovery.Controls;
using EliteDangerousCore;
using EliteDangerousCore.DB;

namespace EDDiscovery.UserControls
{
    public partial class UserControlLedger : UserControlCommonBase
    {
        FilterSelector cfs; 

        private string DbFilterSave { get { return DBName("LedgerGridEventFilter2" ); } }
        private string DbColumnSave { get { return DBName("LedgerGrid" ,  "DGVCol"); } }
        private string DbHistorySave { get { return DBName("LedgerGridEDUIHistory" ); } }

        #region Init

        public UserControlLedger()
        {
            InitializeComponent();
            var corner = dataGridViewLedger.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            dataGridViewLedger.MakeDoubleBuffered();
            dataGridViewLedger.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            var jes = EliteDangerousCore.JournalEntry.GetNameImageOfEvents(new string[] { "Ledger" });
            string cashtype = string.Join(";", jes.Select(x=>x.Item1) ) + ";";

            cfs = new FilterSelector(DbFilterSave);
            cfs.AddAllNone();
            cfs.AddGroupOption(cashtype, "Cash Transactions".T(EDTx.UserControlLedger_CashTransactions),  JournalEntry.JournalTypeIcons[JournalTypeEnum.Bounty]);
            cfs.AddJournalEntries(new string[] { "Ledger", "LedgerNC" });
            cfs.SaveSettings += EventFilterChanged;

            discoveryform.OnHistoryChange += Redisplay;
            discoveryform.OnNewEntry += NewEntry;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(contextMenuStrip, this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);

            TravelHistoryFilter.InitaliseComboBox(comboBoxHistoryWindow, DbHistorySave, incldockstartend: false);
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg = thc;
        }

        public override void LoadLayout()
        {
            dataGridViewLedger.RowTemplate.MinimumHeight = Font.ScalePixels(26);
            DGVLoadColumnLayout(dataGridViewLedger, DbColumnSave);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewLedger, DbColumnSave);
            discoveryform.OnHistoryChange -= Redisplay;
            discoveryform.OnNewEntry -= NewEntry;
        }

        #endregion

        #region Display

        Ledger current_mc;

        private void Redisplay(HistoryList hl)
        {
            Display(hl.cashledger);
        }

        private void NewEntry(HistoryEntry l, HistoryList hl)
        {
            Display(hl.cashledger);
        }

        public override void InitialDisplay()
        {
            Display(discoveryform.history.cashledger);
        }

        private void Display(Ledger mc)
        {
            DataGridViewColumn sortcol = dataGridViewLedger.SortedColumn != null ? dataGridViewLedger.SortedColumn : dataGridViewLedger.Columns[0];
            SortOrder sortorder = dataGridViewLedger.SortOrder != SortOrder.None ? dataGridViewLedger.SortOrder : SortOrder.Descending;

            dataGridViewLedger.Rows.Clear();

            current_mc = mc;
            
            if (mc != null && mc.Transactions.Count > 0)
            {
                var filter = (TravelHistoryFilter)comboBoxHistoryWindow.SelectedItem ?? TravelHistoryFilter.NoFilter;
                List<Ledger.Transaction> filteredlist = filter.Filter(mc.Transactions);

                filteredlist = FilterByJournalEvent(filteredlist, EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbFilterSave, "All"));

                if (filteredlist.Count > 0)
                {
                    var rowsToAdd = new List<DataGridViewRow>(filteredlist.Count);
                    for (int i = filteredlist.Count - 1; i >= 0; i--)
                    {
                        Ledger.Transaction tx = filteredlist[i];

                        object[] rowobj = { EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(tx.utctime) ,
                                            tx.jtype.ToString().SplitCapsWord(),
                                            tx.notes,
                                            (tx.cashadjust>0) ? tx.cashadjust.ToString("N0") : "",
                                            (tx.cashadjust<0) ? (-tx.cashadjust).ToString("N0") : "",
                                            tx.cash.ToString("N0"),
                                            (tx.profitperunit!=0) ? tx.profitperunit.ToString("N0") : ""
                                        };

                        var row = dataGridViewLedger.RowTemplate.Clone() as DataGridViewRow;
                        row.CreateCells(dataGridViewLedger, rowobj);
                        row.Tag = tx.jid;
                        rowsToAdd.Add(row);
                    }

                    dataGridViewLedger.Rows.AddRange(rowsToAdd.ToArray());

                    dataGridViewLedger.FilterGridView(textBoxFilter.Text);
                }
            }

            dataGridViewLedger.Columns[0].HeaderText = EDDiscoveryForm.EDDConfig.GetTimeTitle();
            dataGridViewLedger.Sort(sortcol, (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
            dataGridViewLedger.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;
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
            cfs.Filter( b, this.FindForm());
        }

        private void comboBoxHistoryWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(DbHistorySave, comboBoxHistoryWindow.Text);

            if (current_mc != null)
            {
                Display(current_mc);
            }
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            dataGridViewLedger.FilterGridView(textBoxFilter.Text);
        }

        private void EventFilterChanged(object sender, bool same, Object e)
        {
            if (!same)
                Display(current_mc);
        }

        #region right clicks

        int rightclickrow = -1;

        private void dataGridViewLedger_MouseDown(object sender, MouseEventArgs e)
        {
            dataGridViewLedger.HandleClickOnDataGrid(e, out int unusedleftclickrow, out rightclickrow);
        }

        private void toolStripMenuItemGotoItem_Click(object sender, EventArgs e)
        {
            if (rightclickrow != -1)
            {
                long v = (long)dataGridViewLedger.Rows[rightclickrow].Tag;

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
            if (current_mc != null)
            {
                Forms.ExportForm frm = new Forms.ExportForm();
                frm.Init(new string[] { "Export Current View" }, disablestartendtime: true);

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
