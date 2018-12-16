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
        EventFilterSelector cfs = new EventFilterSelector();

        private string DbFilterSave { get { return DBName("LedgerGridEventFilter" ); } }
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
            dataGridViewLedger.RowTemplate.Height = 26;

            cfs.ConfigureThirdOption("Cash Transactions".Tx(this), string.Join(";", EliteDangerousCore.JournalEntry.GetListOfEventsWithOptMethod(true, "Ledger")));

            cfs.Changed += EventFilterChanged;
            TravelHistoryFilter.InitaliseComboBox(comboBoxHistoryWindow, DbHistorySave , incldockstartend:false);

            discoveryform.OnHistoryChange += Redisplay;
            discoveryform.OnNewEntry += NewEntry;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(contextMenuStrip, this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);
        }

        public override void LoadLayout()
        {
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
            SortOrder sortorder = dataGridViewLedger.SortOrder;

            dataGridViewLedger.Rows.Clear();
            bool utctime = EDDiscoveryForm.EDDConfig.DisplayUTC;

            current_mc = mc;
            
            labelNoItems.Visible = true;

            if (mc != null && mc.Transactions.Count > 0)
            {
                var filter = (TravelHistoryFilter)comboBoxHistoryWindow.SelectedItem ?? TravelHistoryFilter.NoFilter;
                List<Ledger.Transaction> filteredlist = filter.Filter(mc.Transactions);

                filteredlist = FilterByJournalEvent(filteredlist, SQLiteDBClass.GetSettingString(DbFilterSave, "All"));

                if (filteredlist.Count > 0)
                {
                    for (int i = filteredlist.Count - 1; i >= 0; i--)
                    {
                        Ledger.Transaction tx = filteredlist[i];

                        object[] rowobj = { utctime ? tx.utctime : tx.utctime.ToLocalTime() ,
                                            tx.jtype.ToString().SplitCapsWord(),
                                            tx.notes,
                                            (tx.cashadjust>0) ? tx.cashadjust.ToString("N0") : "",
                                            (tx.cashadjust<0) ? (-tx.cashadjust).ToString("N0") : "",
                                            tx.cash.ToString("N0"),
                                            (tx.profitperunit!=0) ? tx.profitperunit.ToString("N0") : ""
                                        };

                        dataGridViewLedger.Rows.Add(rowobj);
                        dataGridViewLedger.Rows[dataGridViewLedger.Rows.Count - 1].Tag = tx.jid;

                    }

                    dataGridViewLedger.FilterGridView(textBoxFilter.Text);

                    labelNoItems.Visible = false;
                }
            }

            dataGridViewLedger.Columns[0].HeaderText = utctime ? "Game Time".Tx() : "Time".Tx();
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
            cfs.FilterButton(DbFilterSave, b,
                             discoveryform.theme.TextBackColor, discoveryform.theme.TextBlockColor, discoveryform.theme.GetFontStandardFontSize(), this.FindForm() ,
                             EliteDangerousCore.JournalEntry.GetListOfEventsWithOptMethod(true, "Ledger", "LedgerNC")
                             );
        }

        private void comboBoxHistoryWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingString(DbHistorySave, comboBoxHistoryWindow.Text);

            if (current_mc != null)
            {
                Display(current_mc);
            }
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            dataGridViewLedger.FilterGridView(textBoxFilter.Text);
        }

        private void EventFilterChanged(object sender, EventArgs e)
        {
            Display(current_mc);
        }

        #region right clicks

        int rightclickrow = -1;
        int leftclickrow = -1;

        private void dataGridViewLedger_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)         // right click on travel map, get in before the context menu
            {
                rightclickrow = -1;
            }
            if (e.Button == MouseButtons.Left)         // right click on travel map, get in before the context menu
            {
                leftclickrow = -1;
            }

            if (dataGridViewLedger.SelectedCells.Count < 2 || dataGridViewLedger.SelectedRows.Count == 1)      // if single row completely selected, or 1 cell or less..
            {
                DataGridView.HitTestInfo hti = dataGridViewLedger.HitTest(e.X, e.Y);
                if (hti.Type == DataGridViewHitTestType.Cell)
                {
                    dataGridViewLedger.ClearSelection();                // select row under cursor.
                    dataGridViewLedger.Rows[hti.RowIndex].Selected = true;

                    if (e.Button == MouseButtons.Right)         // right click on travel map, get in before the context menu
                    {
                        rightclickrow = hti.RowIndex;
                    }
                    if (e.Button == MouseButtons.Left)         // right click on travel map, get in before the context menu
                    {
                        leftclickrow = hti.RowIndex;
                    }
                }
            }
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
                e.SortDataGridViewColumnDate();
            else if (e.Column.Index >= 3)
                e.SortDataGridViewColumnNumeric();
        }
    }
}
