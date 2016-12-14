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
using EDDiscovery.DB;
using EDDiscovery2.EDSM;
using EDDiscovery.EliteDangerous;

namespace EDDiscovery.UserControls
{
    public partial class UserControlJournalGrid : UserControlCommonBase
    {
        private EDDiscoveryForm discoveryform;
        private int displaynumber;                          // since this is plugged into something other than a TabControlForm, can't rely on its display number
        EventFilterSelector cfs = new EventFilterSelector();
        private JSONFilter fieldfilter = new JSONFilter();

        private string DbFilterSave { get { return "JournalGridControlEventFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbColumnSave { get { return "JournalGrid" + ((displaynumber > 0) ? displaynumber.ToString() : "") + "DGVCol"; } }
        private string DbHistorySave { get { return "JournalEDUIHistory" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbFieldFilter { get { return "JournalGridControlFieldFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        public delegate void PopOut();
        public PopOut OnPopOut;

        private HistoryList current_historylist;        // the last one set, for internal refresh purposes on sort

        #region Init

        private class JournalHistoryColumns
        {
            public const int Time = 0;
            public const int Event = 1;
            public const int Type = 2;
            public const int Text = 3;
            public const int HistoryTag = 2;
        }

        public UserControlJournalGrid()
        {
            InitializeComponent();
            Name = "Journal";
        }

        public override void Init( EDDiscoveryForm ed, int vn) //0=primary, 1 = first windowed version, etc
        {
            discoveryform = ed;
            displaynumber = vn;

            dataGridViewJournal.MakeDoubleBuffered();
            dataGridViewJournal.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewJournal.RowTemplate.Height = 26;
            dataGridViewJournal.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx
            cfs.ConfigureThirdOption("Travel", "Docked;FSD Jump;Undocked;");
            cfs.Changed += EventFilterChanged;
            TravelHistoryFilter.InitaliseComboBox(comboBoxJournalWindow, DbHistorySave);

            discoveryform.OnHistoryChange += Display;
            discoveryform.OnNewEntry += AddNewEntry;

            buttonRefresh.Visible = false;

            string filter = SQLiteDBClass.GetSettingString(DbFieldFilter, "");
            if (filter.Length > 0)
                fieldfilter.FromJSON(filter);        // load filter
        }

        public void NoHistoryIcon()
        {
            panelJournalIcon.Visible = false;
            drawnPanelPopOut.Location = new Point(panelJournalIcon.Location.X, drawnPanelPopOut.Location.Y);
        }

        public void NoPopOutIcon()
        {
            drawnPanelPopOut.Visible = false;
        }

        public void ShowRefresh()
        {
            buttonRefresh.Visible = true;
        }

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridViewJournal, DbColumnSave);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewJournal, DbColumnSave);
            discoveryform.OnHistoryChange -= Display;
            discoveryform.OnNewEntry -= AddNewEntry;
        }

        #endregion

        #region Display

        public void Display(HistoryList hl )
        {
            if (hl == null)     // just for safety
                return;

            current_historylist = hl;

            Tuple<long, int> pos = CurrentGridPosByJID();

            var filter = (TravelHistoryFilter)comboBoxJournalWindow.SelectedItem ?? TravelHistoryFilter.NoFilter;

            List<HistoryEntry> result = filter.Filter(hl);

            int ftotal;
            result = HistoryList.FilterByJournalEvent(result, SQLiteDBClass.GetSettingString(DbFilterSave, "All"), out ftotal);
            toolTip1.SetToolTip(buttonFilter, (ftotal > 0) ? ("Total filtered out " + ftotal) : "Filter out entries based on event type");

            result = fieldfilter.FilterHistory(result, out ftotal);
            toolTip1.SetToolTip(buttonField, (ftotal > 0) ? ("Total filtered out " + ftotal) : "Filter out entries matching the field selection");

            dataGridViewJournal.Rows.Clear();

            for (int ii = 0; ii < result.Count; ii++) //foreach (var item in result)
            {
                AddNewJournalRow(false, result[ii]);      // for every one in filter, add a row.
            }

            StaticFilters.FilterGridView(dataGridViewJournal, textBoxFilter.Text);

            int rowno = FindGridPosByJID(pos.Item1);

            if (rowno > 0)
            {
                dataGridViewJournal.CurrentCell = dataGridViewJournal.Rows[rowno].Cells[pos.Item2];       // its the current cell which needs to be set, moves the row marker as well            currentGridRow = (rowno!=-1) ? 
            }

            dataGridViewJournal.Columns[0].HeaderText = EDDiscoveryForm.EDDConfig.DisplayUTC ? "Game Time" : "Time";
        }

        private void AddNewJournalRow(bool insert, HistoryEntry item)            // second part of add history row, adds item to view.
        {
            string detail = "";
            if (item.EventDescription.Length > 0)
                detail = item.EventDescription;
            if (item.EventDetailedInfo.Length > 0)
                detail += ((detail.Length > 0) ? Environment.NewLine : "") + item.EventDetailedInfo;

            object[] rowobj = { EDDiscoveryForm.EDDConfig.DisplayUTC ? item.EventTimeUTC : item.EventTimeLocal, "", item.EventSummary, detail };

            int rownr;

            if (insert)
            {
                dataGridViewJournal.Rows.Insert(0, rowobj);
                rownr = 0;
            }
            else
            {
                dataGridViewJournal.Rows.Add(rowobj);
                rownr = dataGridViewJournal.Rows.Count - 1;
            }

            dataGridViewJournal.Rows[rownr].Cells[JournalHistoryColumns.HistoryTag].Tag = item;
        }

        private void AddNewEntry(HistoryEntry he, HistoryList hl)               // add if in event filter, and not in field filter..
        {
            if (he.IsJournalEventInEventFilter(SQLiteDBClass.GetSettingString(DbFilterSave, "All")) && fieldfilter.FilterHistory(he) )
            {
                AddNewJournalRow(true, he);
            }
        }

        #endregion

        #region Buttons

        public void RefreshButton(bool state)
        {
            buttonRefresh.Enabled = state;
        }

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            cfs.FilterButton(DbFilterSave, b,
                             discoveryform.theme.TextBackColor, discoveryform.theme.TextBlockColor, this.FindForm());
        }

        private void EventFilterChanged(object sender, EventArgs e)
        {
            Display(current_historylist);
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            discoveryform.LogLine("Refresh History.");
            discoveryform.RefreshHistoryAsync(checkedsm: true);
        }

        private void textBoxFilter_KeyUp(object sender, KeyEventArgs e)
        {
            Tuple<long, int> pos = CurrentGridPosByJID();

            StaticFilters.FilterGridView(dataGridViewJournal, textBoxFilter.Text);

            int rowno = FindGridPosByJID(pos.Item1);
            if (rowno > 0)
                dataGridViewJournal.CurrentCell = dataGridViewJournal.Rows[rowno].Cells[pos.Item2];       // its the current cell which needs to be set, moves the row marker as well            currentGridRow = (rowno!=-1) ? 
        }

        private void comboBoxJournalWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingInt(DbHistorySave, comboBoxJournalWindow.SelectedIndex);
            Display(current_historylist);
        }

        private void dataGridViewJournal_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex != JournalHistoryColumns.Event)
            {
                DataGridViewSorter.DataGridSort(dataGridViewJournal, e.ColumnIndex);
            }
        }

        private void buttonField_Click(object sender, EventArgs e)
        {
            EDDiscovery2.JSONFiltersForm frm = new EDDiscovery2.JSONFiltersForm();
            frm.Init("Journal: Filter out fields", "Filter Out", true, discoveryform.theme, fieldfilter);
            frm.TopMost = this.FindForm().TopMost;
            if (frm.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                fieldfilter = frm.result;
                SQLiteDBClass.PutSettingString(DbFieldFilter, fieldfilter.GetJSON());
                Display(current_historylist);
            }
        }

        #endregion

        private void dataGridViewJournal_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            UserControls.UserControlTravelGrid.PaintEventColumn(sender as DataGridView, e,
                discoveryform.history.Count, (HistoryEntry)dataGridViewJournal.Rows[e.RowIndex].Cells[JournalHistoryColumns.HistoryTag].Tag,
                grid.RowHeadersWidth + grid.Columns[0].Width, grid.Columns[1].Width, false);
        }

        #region Mouse Clicks

        private void historyContextMenu_Opening(object sender, CancelEventArgs e)
        {
            mapGotoStartoolStripMenuItem.Enabled = (rightclicksystem != null && rightclicksystem.System.HasCoordinate);
            viewOnEDSMToolStripMenuItem.Enabled = (rightclicksystem != null);
            sendUnsyncedScanToEDDNToolStripMenuItem.Enabled = (rightclicksystem != null && rightclicksystem.EntryType == JournalTypeEnum.Scan && !rightclicksystem.EDDNSync);
        }

        HistoryEntry rightclicksystem = null;
        int rightclickrow = -1;
        HistoryEntry leftclicksystem = null;
        int leftclickrow = -1;

        private void dataGridViewJournal_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)         // right click on travel map, get in before the context menu
            {
                rightclicksystem = null;
                rightclickrow = -1;
            }
            if (e.Button == MouseButtons.Left)         // right click on travel map, get in before the context menu
            {
                leftclicksystem = null;
                leftclickrow = -1;
            }

            if (dataGridViewJournal.SelectedCells.Count < 2 || dataGridViewJournal.SelectedRows.Count == 1)      // if single row completely selected, or 1 cell or less..
            {
                DataGridView.HitTestInfo hti = dataGridViewJournal.HitTest(e.X, e.Y);
                if (hti.Type == DataGridViewHitTestType.Cell)
                {
                    dataGridViewJournal.ClearSelection();                // select row under cursor.
                    dataGridViewJournal.Rows[hti.RowIndex].Selected = true;

                    if (e.Button == MouseButtons.Right)         // right click on travel map, get in before the context menu
                    {
                        rightclickrow = hti.RowIndex;
                        rightclicksystem = (HistoryEntry)dataGridViewJournal.Rows[hti.RowIndex].Cells[JournalHistoryColumns.HistoryTag].Tag;
                    }
                    if (e.Button == MouseButtons.Left)         // right click on travel map, get in before the context menu
                    {
                        leftclickrow = hti.RowIndex;
                        leftclicksystem = (HistoryEntry)dataGridViewJournal.Rows[hti.RowIndex].Cells[JournalHistoryColumns.HistoryTag].Tag;
                    }
                }
            }
        }

        private void mapGotoStartoolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            if (!discoveryform.Map.Is3DMapsRunning)            // if not running, click the 3dmap button
                discoveryform.Open3DMap(null);

            this.Cursor = Cursors.Default;

            if (discoveryform.Map.Is3DMapsRunning)             // double check here! for paranoia.
            {
                if (discoveryform.Map.MoveToSystem(rightclicksystem.System))
                    discoveryform.Map.Show();
            }

        }

        private void viewOnEDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            EDSMClass edsm = new EDSMClass();
            long? id_edsm = rightclicksystem.System?.id_edsm;

            if (id_edsm == 0)
            {
                id_edsm = null;
            }

            if (!edsm.ShowSystemInEDSM(rightclicksystem.System.name, id_edsm))
                MessageBox.Show("System could not be found - has not been synched or EDSM is unavailable");

            this.Cursor = Cursors.Default;
        }

        private void toolStripMenuItemStartStop_Click(object sender, EventArgs e)
        {
            if (rightclicksystem != null)
            {
                discoveryform.history.SetStartStop(rightclicksystem);
                discoveryform.RefreshHistoryAsync();
            }
        }

        private void sendUnsyncedScanToEDDNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclicksystem != null && rightclicksystem.EntryType == JournalTypeEnum.Scan && !rightclicksystem.EDDNSync)
            {
                EDDNSync.SendEDDNEvent(discoveryform, rightclicksystem);
            }
        }

        #endregion

        Tuple<long, int> CurrentGridPosByJID()
        {
            long jid = (dataGridViewJournal.CurrentCell != null) ? ((HistoryEntry)(dataGridViewJournal.Rows[dataGridViewJournal.CurrentCell.RowIndex].Cells[JournalHistoryColumns.HistoryTag].Tag)).Journalid : 0;
            int cellno = (dataGridViewJournal.CurrentCell != null) ? dataGridViewJournal.CurrentCell.ColumnIndex : 0;
            return new Tuple<long, int>(jid, cellno);
        }

        int FindGridPosByJID(long jid)
        {
            if (dataGridViewJournal.Rows.Count > 0 && jid != 0)
            {
                foreach (DataGridViewRow r in dataGridViewJournal.Rows)
                {
                    if (r.Visible && ((HistoryEntry)(r.Cells[JournalHistoryColumns.HistoryTag].Tag)).Journalid == jid)
                    {
                        return r.Index;
                    }
                }
            }

            return -1;
        }

        private void drawnPanelPopOut_Click(object sender, EventArgs e)
        {
            if (OnPopOut != null)
                OnPopOut();
        }
    }
}
