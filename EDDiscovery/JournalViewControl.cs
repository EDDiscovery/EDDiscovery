using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EDDiscovery.EliteDangerous;
using EDDiscovery.DB;
using EDDiscovery.Controls;
using EDDiscovery2.EDSM;

namespace EDDiscovery
{
    public partial class JournalViewControl : UserControl
    {
        private EDDiscoveryForm _discoveryForm;
        bool ignorewidthchange = false;

        public class JournalHistoryColumns
        {
            public const int Time = 0;
            public const int Event = 1;
            public const int Type = 2;
            public const int Text = 3;
            public const int HistoryTag = 2;
        }

        EventFilterSelector cfs = new EventFilterSelector();

        public JournalViewControl()
        {
            InitializeComponent();
        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;
            dataGridViewJournal.MakeDoubleBuffered();
            dataGridViewJournal.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewJournal.RowTemplate.Height = 26;
            cfs.Changed += EventFilterChanged;

            TravelHistoryFilter.InitaliseComboBox(comboBoxJournalWindow, "JournalTimeHistory");
        }

        public void Display()
        {
            int rowno = (dataGridViewJournal.CurrentCell != null) ? dataGridViewJournal.CurrentCell.RowIndex : 0;
            int cellno = (dataGridViewJournal.CurrentCell != null) ? dataGridViewJournal.CurrentCell.ColumnIndex : 0;

            var filter = (TravelHistoryFilter)comboBoxJournalWindow.SelectedItem ?? TravelHistoryFilter.NoFilter;

            List<HistoryEntry> result = filter.Filter(_discoveryForm.history);

            result = HistoryList.FilterByJournalEvent(result, SQLiteDBClass.GetSettingString("JournalHistoryControlEventFilter", "All"));

            dataGridViewJournal.Rows.Clear();

            for (int ii = 0; ii < result.Count; ii++) //foreach (var item in result)
            {
                AddNewJournalRow(false, result[ii]);      // for every one in filter, add a row.
            }

            StaticFilters.FilterGridView(dataGridViewJournal, textBoxFilter.Text);

            dataGridViewJournal.AutoResizeRows();

            if (dataGridViewJournal.Rows.Count > 0)
            {
                rowno = Math.Min(rowno, dataGridViewJournal.Rows.Count - 1);
                dataGridViewJournal.CurrentCell = dataGridViewJournal.Rows[rowno].Cells[cellno];       // its the current cell which needs to be set, moves the row marker as well            currentGridRow = (rowno!=-1) ? 
                dataGridViewJournal.FirstDisplayedScrollingRowIndex = rowno;
                System.Diagnostics.Trace.WriteLine(string.Format("Cell {0} {1}", rowno, cellno));
            }

            dataGridViewJournal.Columns[0].HeaderText = EDDiscoveryForm.EDDConfig.DisplayUTC ? "Game Time" : "Time";
        }

        private void AddNewJournalRow(bool insert, HistoryEntry item)            // second part of add history row, adds item to view.
        {
            string detail = "";
            if (item.EventDescription.Length > 0)
                detail = item.EventDescription;
            if (item.EventDetailedInfo.Length > 0)
                detail += ((detail.Length>0) ? Environment.NewLine : "") + item.EventDetailedInfo;

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

        public void AddNewEntry(HistoryEntry he)
        {
            AddNewJournalRow(true, he);
        }

        #region Layout

        public void LoadLayoutSettings() // called by discovery form by us after its adjusted itself
        {
            ignorewidthchange = true;
            if (SQLiteConnectionUser.keyExists("JournalControlDGVCol1"))        // if stored values, set back to what they were..
            {
                for (int i = 0; i < dataGridViewJournal.Columns.Count; i++)
                {
                    int w = SQLiteDBClass.GetSettingInt("JournalControlDGVCol" + ((i + 1).ToString()), -1);
                    if (w > 10)        // in case something is up (min 10 pixels)
                        dataGridViewJournal.Columns[i].Width = w;
                }
            }

            FillDGVOut();
            ignorewidthchange = false;
        }

        public void SaveSettings()     // called by form when closing
        {
            for (int i = 0; i < dataGridViewJournal.Columns.Count; i++)
                SQLiteDBClass.PutSettingInt("JournalControlDGVCol" + ((i + 1).ToString()), dataGridViewJournal.Columns[i].Width);
        }

        private void dataGridViewJournal_Resize(object sender, EventArgs e)
        {
            ignorewidthchange = true;
            FillDGVOut();
            ignorewidthchange = false;
        }

        private void dataGridViewJournal_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (!ignorewidthchange)
            {
                ignorewidthchange = true;
                FillDGVOut();       // scale out so its filled..
                ignorewidthchange = false;
            }
        }

        void FillDGVOut()
        {
            int twidth = dataGridViewJournal.RowHeadersWidth;        // get how many pixels we are using..
            for (int i = 0; i < dataGridViewJournal.Columns.Count; i++)
                twidth += dataGridViewJournal.Columns[i].Width;

            int delta = dataGridViewJournal.Width - twidth;

            if (delta < 0)        // not enough space
            {
                Collapse(ref delta, JournalHistoryColumns.Text);         // pick columns on preference list to shrink
                Collapse(ref delta, JournalHistoryColumns.Event);         // pick columns on preference list to shrink
                Collapse(ref delta, JournalHistoryColumns.Time);
                Collapse(ref delta, JournalHistoryColumns.Type);
            }
            else
                dataGridViewJournal.Columns[JournalHistoryColumns.Text].Width += delta;   // note is used to fill out columns
        }

        void Collapse(ref int delta, int col)
        {
            if (delta < 0)
            {
                int colsaving = dataGridViewJournal.Columns[col].Width - dataGridViewJournal.Columns[col].MinimumWidth;

                if (-delta <= colsaving)       // if can save 30 from col3, and delta is -20, 20<=30, do it.
                {
                    dataGridViewJournal.Columns[col].Width += delta;
                    delta = 0;
                }
                else
                {
                    delta += colsaving;
                    dataGridViewJournal.Columns[col].Width = dataGridViewJournal.Columns[col].MinimumWidth;
                }
            }
        }

        #endregion

        #region Buttons

        public void RefreshButton(bool state)
        {
            buttonRefresh.Enabled = state;
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            _discoveryForm.LogLine("Refresh History.");
            _discoveryForm.RefreshHistoryAsync();
        }

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            cfs.FilterButton("JournalHistoryControlEventFilter", b,
                             _discoveryForm.theme.TextBackColor, _discoveryForm.theme.TextBlockColor, _discoveryForm);
        }

        private void EventFilterChanged(object sender, EventArgs e)
        {
            Display();
        }

        private void textBoxFilter_KeyUp(object sender, KeyEventArgs e)
        {
            StaticFilters.FilterGridView(dataGridViewJournal, textBoxFilter.Text);
        }

        #endregion

        #region Paint
        private void dataGridViewJournal_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            TravelHistoryControl.PaintEventColumn(sender as DataGridView, e,
                _discoveryForm.history.Count, (HistoryEntry)dataGridViewJournal.Rows[e.RowIndex].Cells[JournalHistoryColumns.HistoryTag].Tag,
                grid.RowHeadersWidth + grid.Columns[0].Width, grid.Columns[1].Width, false);
        }

        #endregion


        private void comboBoxHistoryWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            Display();
            SQLiteDBClass.PutSettingInt("JournalTimeHistory", comboBoxJournalWindow.SelectedIndex);
        }

        private void dataGridViewJournal_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex != JournalHistoryColumns.Event)
            {
                DataGridViewSorter.DataGridSort(dataGridViewJournal, e.ColumnIndex);
            }

        }

        private void mapGotoStartoolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (!_discoveryForm.Map.Is3DMapsRunning)            // if not running, click the 3dmap button
                _discoveryForm.TravelControl.buttonMap_Click(sender, e);
            this.Cursor = Cursors.Default;

            if (_discoveryForm.Map.Is3DMapsRunning)             // double check here! for paranoia.
            {
                if (_discoveryForm.Map.MoveToSystem(rightclicksystem.System))
                    _discoveryForm.Map.Show();
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
                _discoveryForm.LogLineHighlight("System could not be found - has not been synched or EDSM is unavailable");

            this.Cursor = Cursors.Default;
        }

        private void toolStripMenuItemStartStop_Click(object sender, EventArgs e)
        {
            if (rightclicksystem != null)
            {
                _discoveryForm.history.SetStartStop(rightclicksystem);
                _discoveryForm.RefreshFrontEnd();                                   // which will cause DIsplay to be called as some point
            }
        }

        private void historyContextMenu_Opening(object sender, CancelEventArgs e)
        {
            mapGotoStartoolStripMenuItem.Enabled = (rightclicksystem != null && rightclicksystem.System.HasCoordinate);
            viewOnEDSMToolStripMenuItem.Enabled = (rightclicksystem != null);
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
    }
}
