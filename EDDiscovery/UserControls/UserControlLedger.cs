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
using EDDiscovery2.DB;

namespace EDDiscovery.UserControls
{
    public partial class UserControlLedger : UserControlCommonBase
    {
        private int displaynumber = 0;
        private EDDiscoveryForm discoveryform;
        
        EventFilterSelector cfs = new EventFilterSelector();

        private string DbFilterSave { get { return "LedgerGridEventFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbColumnSave { get { return ("LedgerGrid") + ((displaynumber > 0) ? displaynumber.ToString() : "") + "DGVCol"; } }
        private string DbHistorySave { get { return "LedgerGridEDUIHistory" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        public delegate void GotoJID( long jid);
        public event GotoJID OnGotoJID;

        #region Init

        public UserControlLedger()
        {
            InitializeComponent();
        }

        public override void Init( EDDiscoveryForm ed, int vn) //0=primary, 1 = first windowed version, etc
        {
            discoveryform = ed;
            displaynumber = vn;

            dataGridViewLedger.MakeDoubleBuffered();
            dataGridViewLedger.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dataGridViewLedger.RowTemplate.Height = 26;

            cfs.ConfigureThirdOption("Cash Transactions", string.Join(";", EliteDangerous.JournalEntry.GetListOfEventsWithOptMethod(true, "Ledger")));

            cfs.Changed += EventFilterChanged;
            TravelHistoryFilter.InitaliseComboBox(comboBoxHistoryWindow, DbHistorySave);

            discoveryform.OnHistoryChange += Redisplay;
            discoveryform.OnNewEntry += NewEntry;
        }

        private void Discoveryform_OnHistoryChange(HistoryList l)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Display

        MaterialCommoditiesLedger current_mc;

        public void Redisplay(HistoryList hl)
        {
            Display(hl.materialcommodititiesledger);
        }

        private void NewEntry(HistoryEntry l, HistoryList hl)
        {
            Display(hl.materialcommodititiesledger);
        }

        public void Display(MaterialCommoditiesLedger mc)
        {
            dataGridViewLedger.Rows.Clear();
            bool utctime = EDDiscoveryForm.EDDConfig.DisplayUTC;

            current_mc = mc;
            
            labelNoItems.Visible = true;

            if (mc != null && mc.Transactions.Count > 0)
            {
                var filter = (TravelHistoryFilter)comboBoxHistoryWindow.SelectedItem ?? TravelHistoryFilter.NoFilter;
                List<MaterialCommoditiesLedger.Transaction> filteredlist = filter.Filter(mc.Transactions);

                filteredlist = FilterByJournalEvent(filteredlist, DB.SQLiteDBClass.GetSettingString(DbFilterSave, "All"));

                if (filteredlist.Count > 0)
                {
                    for (int i = filteredlist.Count - 1; i >= 0; i--)
                    {
                        MaterialCommoditiesLedger.Transaction tx = filteredlist[i];

                        object[] rowobj = { utctime ? tx.utctime : tx.utctime.ToLocalTime() ,
                                            Tools.SplitCapsWord(tx.jtype.ToString()),
                                            tx.notes,
                                            (tx.cashadjust>0) ? tx.cashadjust.ToString("N0") : "",
                                            (tx.cashadjust<0) ? (-tx.cashadjust).ToString("N0") : "",
                                            tx.cash.ToString("N0"),
                                            (tx.profitperunit!=0) ? tx.profitperunit.ToString("N0") : ""
                                        };

                        dataGridViewLedger.Rows.Add(rowobj);
                        dataGridViewLedger.Rows[dataGridViewLedger.Rows.Count - 1].Tag = tx.jid;

                    }

                    StaticFilters.FilterGridView(dataGridViewLedger, textBoxFilter.Text);

                    labelNoItems.Visible = false;
                }
            }

            dataGridViewLedger.Columns[0].HeaderText = utctime ? "Game Time" : "Time";
        }

        public List<MaterialCommoditiesLedger.Transaction> FilterByJournalEvent(List<MaterialCommoditiesLedger.Transaction> txlist, string eventstring)
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

        #region Layout

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

        private void dataGridViewMC_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
        }

        private void dataGridViewMC_Resize(object sender, EventArgs e)
        {
        }

        #endregion

        private void checkBoxCustomCashOnly_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            cfs.FilterButton(DbFilterSave, b,
                             discoveryform.theme.TextBackColor, discoveryform.theme.TextBlockColor, this.FindForm() ,
                             EliteDangerous.JournalEntry.GetListOfEventsWithOptMethod(true, "Ledger", "LedgerNC")
                             );
        }

        private void comboBoxHistoryWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            DB.SQLiteDBClass.PutSettingInt(DbHistorySave, comboBoxHistoryWindow.SelectedIndex);

            if (current_mc != null)
            {
                Display(current_mc);
            }
        }

        private void textBoxFilter_KeyUp(object sender, KeyEventArgs e)
        {
            StaticFilters.FilterGridView(dataGridViewLedger, textBoxFilter.Text);
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

                if (OnGotoJID != null)
                    OnGotoJID(v);
            }
        }

        #endregion
    }
}
