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
using EDDiscovery2.DB;
using EDDiscovery.EliteDangerous;
using EDDiscovery2.EDSM;
using EDDiscovery2;

namespace EDDiscovery.UserControls
{
    public partial class UserControlTravelGrid : UserControlCommonBase
    {
        public int currentGridRow { get; set; } = -1;
        public DataGridViewRow GetCurrentRow { get { return currentGridRow >= 0 ? dataGridViewTravel.Rows[currentGridRow] : null; } }
        public HistoryEntry GetCurrentHistoryEntry { get { return currentGridRow >= 0 ? dataGridViewTravel.Rows[currentGridRow].Cells[TravelHistoryColumns.HistoryTag].Tag as HistoryEntry : null; } }
        public SystemNoteClass GetCurrentSystemNoteClass { get { return currentGridRow >= 0 ? dataGridViewTravel.Rows[currentGridRow].Cells[TravelHistoryColumns.NoteTag].Tag as SystemNoteClass : null; } }

        public HistoryEntry GetHistoryEntry(int r) { return dataGridViewTravel.Rows[r].Cells[TravelHistoryColumns.HistoryTag].Tag as HistoryEntry; }
        public SystemNoteClass GetSystemNoteClass(int r) { return dataGridViewTravel.Rows[r].Cells[TravelHistoryColumns.NoteTag].Tag as SystemNoteClass; }

        public static HistoryEntry GetHistoryEntry( DataGridViewRow rw) { return rw.Cells[TravelHistoryColumns.HistoryTag].Tag as HistoryEntry; }

        public DataGridViewRow GetRow(int r) { return dataGridViewTravel.Rows[r]; }

        public DataGridView TravelGrid { get { return dataGridViewTravel; } }

        public TravelHistoryFilter GetHistoryFilter { get { return (TravelHistoryFilter)comboBoxHistoryWindow.SelectedItem ?? TravelHistoryFilter.NoFilter; } }

        private JSONFilter fieldfilter = new JSONFilter();

        public delegate void ChangedSelection(int rowno, int colno, bool doubleclick , bool note);
        public event ChangedSelection OnChangedSelection;

        public delegate void Resort();
        public event Resort OnResort;

        public delegate void AddedNewEntry(HistoryEntry he, HistoryList hl, bool accepted);
        public AddedNewEntry OnAddedNewEntry;

        public delegate void Redisplay(HistoryList hl);
        public Redisplay OnRedisplay;

        public delegate void PopOut();
        public PopOut OnPopOut;

        #region Init

        private class TravelHistoryColumns
        {
            public const int Time = 0;
            public const int Icon = 1;
            public const int Description = 2;
            public const int Information = 3;
            public const int Note = 4;

            public const int HistoryTag = Description;      // where the tags are used
            public const int NoteTag = Note;
        }

        private const int DefaultRowHeight = 26;

        private static EDDiscoveryForm discoveryform;
        private int displaynumber;                          

        private string DbFilterSave { get { return "TravelHistoryControlEventFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbColumnSave { get { return "TravelControl" + ((displaynumber > 0) ? displaynumber.ToString() : "") + "DGVCol"; } }
        private string DbHistorySave { get { return "EDUIHistory" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbFieldFilter { get { return "TravelHistoryControlFieldFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        private HistoryList current_historylist;        // the last one set, for internal refresh purposes on sort

        private long preferred_jid = -1;                        // use Preferred to say, i'm about to refresh you, go here..

        EventFilterSelector cfs = new EventFilterSelector();

        public UserControlTravelGrid()
        {
            InitializeComponent();
        }

        public override void Init( EDDiscoveryForm ed, int vn) //0=primary, 1 = first windowed version, etc
        {
            discoveryform = ed;
            displaynumber = vn;
            cfs.ConfigureThirdOption("Travel", "Docked;FSD Jump;Undocked;");
            cfs.Changed += EventFilterChanged;
            TravelHistoryFilter.InitaliseComboBox(comboBoxHistoryWindow, DbHistorySave);

            discoveryform.OnHistoryChange += Display;
            discoveryform.OnNewEntry += AddNewEntry;

            dataGridViewTravel.MakeDoubleBuffered();
            dataGridViewTravel.RowTemplate.Height = DefaultRowHeight;

            string filter = SQLiteDBClass.GetSettingString(DbFieldFilter, "");
            if (filter.Length>0)
                fieldfilter.FromJSON(filter);        // load filter
        }

        public void NoHistoryIcon()
        {
            panelHistoryIcon.Visible = false;
            drawnPanelPopOut.Location = new Point(panelHistoryIcon.Location.X, drawnPanelPopOut.Location.Y);
        }

        public void NoPopOutIcon()
        {
            drawnPanelPopOut.Visible = false;
        }

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridViewTravel, DbColumnSave);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewTravel, DbColumnSave);
            discoveryform.OnHistoryChange -= Display;
            discoveryform.OnNewEntry -= AddNewEntry;
        }

        #endregion

        public void Display( HistoryList hl )           // rowno current.. -1 if nothing
        {
            if (hl == null)     // just for safety
                return;

            current_historylist = hl;
            Tuple<long, int> pos = CurrentGridPosByJID();

            var filter = (TravelHistoryFilter)comboBoxHistoryWindow.SelectedItem ?? TravelHistoryFilter.NoFilter;

            List<HistoryEntry> result = filter.Filter(hl);

            int ftotal;
            result = HistoryList.FilterByJournalEvent(result, SQLiteDBClass.GetSettingString(DbFilterSave, "All"), out ftotal);
            toolTip1.SetToolTip(buttonFilter, (ftotal > 0) ? ("Total filtered out " + ftotal) : "Filter out entries based on event type");

            result = fieldfilter.FilterHistory(result,out ftotal);
            toolTip1.SetToolTip(buttonField, (ftotal > 0) ? ("Total filtered out " + ftotal) : "Filter out entries matching the field selection");

            dataGridViewTravel.Rows.Clear();

            for (int ii = 0; ii < result.Count; ii++) //foreach (var item in result)
            {
                AddNewHistoryRow(false, result[ii]);      // for every one in filter, add a row.
            }

            StaticFilters.FilterGridView(dataGridViewTravel, textBoxFilter.Text);

            int rowno = FindGridPosByJID(preferred_jid>=0 ? preferred_jid : pos.Item1);     // either go back to preferred, or to remembered above
            preferred_jid = -1;                                                             // 1 shot at this

            if (rowno >= 0)
            {
                dataGridViewTravel.CurrentCell = dataGridViewTravel.Rows[rowno].Cells[pos.Item2];       // its the current cell which needs to be set, moves the row marker as well            currentGridRow = (rowno!=-1) ? 
            }
            else if (dataGridViewTravel.Rows.GetRowCount(DataGridViewElementStates.Visible) > 0)
            {
                rowno = dataGridViewTravel.Rows.GetFirstRow(DataGridViewElementStates.Visible);
                dataGridViewTravel.CurrentCell = dataGridViewTravel.Rows[rowno].Cells[TravelHistoryColumns.Description];
            }
            else
                rowno = -1;

            currentGridRow = rowno;

            dataGridViewTravel.Columns[0].HeaderText = EDDiscoveryForm.EDDConfig.DisplayUTC ? "Game Time" : "Time";

            if (OnRedisplay != null)
                OnRedisplay(hl);
        }

        private void AddNewEntry(HistoryEntry he, HistoryList hl)
        {
            bool add = WouldAddEntry(he);
            if (add)
                AddNewHistoryRow(true, he);

            if (OnAddedNewEntry != null)
                OnAddedNewEntry(he, hl, add);
        }

        private void AddNewHistoryRow(bool insert, HistoryEntry item)            // second part of add history row, adds item to view.
        {
            SystemNoteClass snc = SystemNoteClass.GetNoteOnJournalEntry(item.Journalid);
            if (snc == null && item.IsFSDJump)
                snc = SystemNoteClass.GetNoteOnSystem(item.System.name, item.System.id_edsm);

            // Try to fill EDSM ID where a system note is set but journal item EDSM ID is not set
            if (snc != null && snc.Name != null && snc.EdsmId > 0 && item.System.id_edsm <= 0)
            {
                item.System.id_edsm = 0;
                discoveryform.history.FillEDSM(item);

                if (snc.Journalid != item.Journalid && item.System.id_edsm > 0 && snc.EdsmId != item.System.id_edsm)
                    snc = null;
            }

            if (snc != null && snc.Name != null && snc.Journalid == item.Journalid && item.System.id_edsm > 0 && snc.EdsmId != item.System.id_edsm)
            {
                snc.EdsmId = item.System.id_edsm;
                snc.Update();
            }

            //string debugt = item.Journalid + "  " + item.System.id_edsm + " " + item.System.GetHashCode() + " "; // add on for debug purposes to a field below

            object[] rowobj = { EDDiscoveryForm.EDDConfig.DisplayUTC ? item.EventTimeUTC : item.EventTimeLocal, "", item.EventSummary, item.EventDescription, (snc != null) ? snc.Note : "" };

            int rownr;
            if (insert)
            {
                dataGridViewTravel.Rows.Insert(0, rowobj);
                rownr = 0;
            }
            else
            {
                dataGridViewTravel.Rows.Add(rowobj);
                rownr = dataGridViewTravel.Rows.Count - 1;
            }

            dataGridViewTravel.Rows[rownr].Cells[TravelHistoryColumns.HistoryTag].Tag = item;
            dataGridViewTravel.Rows[rownr].Cells[TravelHistoryColumns.NoteTag].Tag = snc;

            dataGridViewTravel.Rows[rownr].DefaultCellStyle.ForeColor = (item.System.HasCoordinate || item.EntryType != JournalTypeEnum.FSDJump) ? discoveryform.theme.VisitedSystemColor : discoveryform.theme.NonVisitedSystemColor;

            string tip = item.EventSummary + Environment.NewLine + item.EventDescription + Environment.NewLine + item.EventDetailedInfo;
            dataGridViewTravel.Rows[rownr].Cells[0].ToolTipText = tip;
            dataGridViewTravel.Rows[rownr].Cells[1].ToolTipText = tip;
            dataGridViewTravel.Rows[rownr].Cells[2].ToolTipText = tip;
            dataGridViewTravel.Rows[rownr].Cells[3].ToolTipText = tip;
            dataGridViewTravel.Rows[rownr].Cells[4].ToolTipText = tip;
        }

        public void SetPreferredJIDAfterRefresh(long jid)           // call if after the next Display refresh you would like to go to this jid
        {
            preferred_jid = jid;
        }

        public bool WouldAddEntry(HistoryEntry he)                  // do we filter? if its not in the journal event filter, or it is in the field filter
        {
            return he.IsJournalEventInEventFilter(SQLiteDBClass.GetSettingString(DbFilterSave, "All")) && fieldfilter.FilterHistory(he);
        }
        
        public void SelectTopRow()
        {
            dataGridViewTravel.ClearSelection();
            dataGridViewTravel.CurrentCell = dataGridViewTravel.Rows[0].Cells[1];       // its the current cell which needs to be set, moves the row marker as well
            currentGridRow = 0;
        }

        Tuple<long, int> CurrentGridPosByJID()
        {
            long jid = (dataGridViewTravel.CurrentCell != null) ? ((HistoryEntry)(dataGridViewTravel.Rows[dataGridViewTravel.CurrentCell.RowIndex].Cells[TravelHistoryColumns.HistoryTag].Tag)).Journalid : 0;
            int cellno = (dataGridViewTravel.CurrentCell != null) ? dataGridViewTravel.CurrentCell.ColumnIndex : 0;
            return new Tuple<long, int>(jid, cellno);
        }

        public void GotoPosByJID(long jid )
        {
            int rowno = FindGridPosByJID(jid);
            if (rowno >= 0)
            {
                dataGridViewTravel.CurrentCell = dataGridViewTravel.Rows[rowno].Cells[TravelHistoryColumns.Information];
                dataGridViewTravel.Rows[rowno].Selected = true;
            }
        }

        int FindGridPosByJID(long jid)
        {
            if (dataGridViewTravel.Rows.Count > 0 && jid != 0)
            {
                foreach (DataGridViewRow r in dataGridViewTravel.Rows)
                {
                    if (r.Visible && ((HistoryEntry)(r.Cells[TravelHistoryColumns.HistoryTag].Tag)).Journalid == jid)
                    {
                        return r.Index;
                    }
                }
            }

            return -1;
        }

        private void comboBoxHistoryWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingInt(DbHistorySave, comboBoxHistoryWindow.SelectedIndex);

            if (current_historylist != null)
            {
                Display(current_historylist);

                if (OnResort != null)
                    OnResort();
            }
        }

        private void dataGridViewTravel_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex != TravelHistoryColumns.Icon)
            {
                DataGridViewSorter.DataGridSort(dataGridViewTravel, e.ColumnIndex);
                if (OnResort != null)
                    OnResort();
            }
        }

        private void dataGridViewTravel_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            currentGridRow = e.RowIndex;
            if (OnChangedSelection != null)
                OnChangedSelection(e.RowIndex, e.ColumnIndex, false, e.ColumnIndex == TravelHistoryColumns.Note);
        }


        private void dataGridViewTravel_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Cell Enter");

            if ( cursorkeydown )
            {
                cursorkeydown = false;
                currentGridRow = e.RowIndex;
                if (OnChangedSelection != null)
                    OnChangedSelection(e.RowIndex, e.ColumnIndex, false, e.ColumnIndex == TravelHistoryColumns.Note);
            }

        }

        bool cursorkeydown = false;
        private void dataGridViewTravel_KeyDown(object sender, KeyEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Key down " + e.KeyCode);
            cursorkeydown = (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode==Keys.PageDown || e.KeyCode == Keys.PageUp);
        }

        public void UpdateCurrentNote(string s)
        {
            if (currentGridRow >= 0)
                dataGridViewTravel.Rows[currentGridRow].Cells[TravelHistoryColumns.Note].Value = s;
        }

        public void UpdateNoteJID(long r, string s)
        {
            int row = FindGridPosByJID(r);
            if ( row >= 0 )
                dataGridViewTravel.Rows[row].Cells[TravelHistoryColumns.Note].Value = s;
        }
        
        public void UpdateCurrentNoteTag(Object o)
        {
            if (currentGridRow >= 0)
                dataGridViewTravel.Rows[currentGridRow].Cells[TravelHistoryColumns.Note].Tag = o;
        }

        private void textBoxFilter_KeyUp(object sender, KeyEventArgs e)
        {
            Tuple<long, int> pos = CurrentGridPosByJID();

            StaticFilters.FilterGridView(dataGridViewTravel, textBoxFilter.Text);

            int rowno = FindGridPosByJID(pos.Item1);
            if (rowno > 0)
                dataGridViewTravel.CurrentCell = dataGridViewTravel.Rows[rowno].Cells[pos.Item2];       // its the current cell which needs to be set, moves the row marker as well            currentGridRow = (rowno!=-1) ? 
        }

        private void dataGridViewTravel_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            PaintEventColumn(grid, e,
                            discoveryform.history.Count, (HistoryEntry)dataGridViewTravel.Rows[e.RowIndex].Cells[TravelHistoryColumns.HistoryTag].Tag,
                            grid.RowHeadersWidth + grid.Columns[0].Width, grid.Columns[1].Width, true);
        }

        public static void PaintEventColumn(DataGridView grid, DataGridViewRowPostPaintEventArgs e,
                                             int totalentries, HistoryEntry he,
                                             int hpos, int colwidth, bool showfsdmapcolour)
        {
            string rowIdx;

            if (discoveryform.settings.OrderRowsInverted)
                rowIdx = he.Indexno.ToString();            // oldest has the highest index
            else
                rowIdx = (totalentries - he.Indexno + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);

            using (Brush br = new SolidBrush(grid.RowHeadersDefaultCellStyle.ForeColor))
                e.Graphics.DrawString(rowIdx, grid.RowHeadersDefaultCellStyle.Font, br, headerBounds, centerFormat);

            int noicons = (he.IsFSDJump && showfsdmapcolour) ? 2 : 1;
            if (he.StartMarker || he.StopMarker)
                noicons++;

            int padding = 4;
            int size = 24;

            if (size * noicons > (colwidth - 2))
                size = (colwidth - 2) / noicons;

            int hstart = (hpos + colwidth / 2) - size / 2 * noicons - padding / 2 * (noicons - 1);

            int top = (e.RowBounds.Top + e.RowBounds.Bottom) / 2 - size / 2;

            e.Graphics.DrawImage(he.GetIcon, new Rectangle(hstart, top, size, size));
            hstart += size + padding;

            if (he.IsFSDJump && showfsdmapcolour)
            {
                using (Brush b = new SolidBrush(Color.FromArgb(he.MapColour)))
                {
                    e.Graphics.FillEllipse(b, new Rectangle(hstart + 2, top + 2, size - 6, size - 6));
                }

                hstart += size + padding;
            }

            if (he.StartMarker)
                e.Graphics.DrawImage(EDDiscovery.Properties.Resources.startflag, new Rectangle(hstart, top, size, size));
            else if (he.StopMarker)
                e.Graphics.DrawImage(EDDiscovery.Properties.Resources.stopflag, new Rectangle(hstart, top, size, size));

        }

        #region Clicks

        HistoryEntry rightclicksystem = null;
        int rightclickrow = -1;
        HistoryEntry leftclicksystem = null;
        int leftclickrow = -1;

        private void dataGridViewTravel_MouseDown(object sender, MouseEventArgs e)
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

            if (dataGridViewTravel.SelectedCells.Count < 2 || dataGridViewTravel.SelectedRows.Count == 1)      // if single row completely selected, or 1 cell or less..
            {
                DataGridView.HitTestInfo hti = dataGridViewTravel.HitTest(e.X, e.Y);
                if (hti.Type == DataGridViewHitTestType.Cell)
                {
                    dataGridViewTravel.ClearSelection();                // select row under cursor.
                    dataGridViewTravel.Rows[hti.RowIndex].Selected = true;

                    if (e.Button == MouseButtons.Right)         // right click on travel map, get in before the context menu
                    {
                        rightclickrow = hti.RowIndex;
                        rightclicksystem = (HistoryEntry)dataGridViewTravel.Rows[hti.RowIndex].Cells[TravelHistoryColumns.HistoryTag].Tag;
                    }
                    if (e.Button == MouseButtons.Left)         // right click on travel map, get in before the context menu
                    {
                        leftclickrow = hti.RowIndex;
                        leftclicksystem = (HistoryEntry)dataGridViewTravel.Rows[hti.RowIndex].Cells[TravelHistoryColumns.HistoryTag].Tag;
                    }
                }
            }
        }

        private void dataGridViewTravel_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (leftclickrow >= 0)                                                   // Click expands it..
            {
                int ch = dataGridViewTravel.Rows[leftclickrow].Height;
                bool toexpand = (ch <= DefaultRowHeight);

                string infotext = leftclicksystem.EventDescription + ((toexpand && leftclicksystem.EventDetailedInfo.Length > 0) ? (Environment.NewLine + leftclicksystem.EventDetailedInfo) : "");

                int h = DefaultRowHeight;

                if (toexpand)
                {
                    using (Graphics g = Parent.CreateGraphics())
                    {
                        int desch = (int)(g.MeasureString((string)dataGridViewTravel.Rows[leftclickrow].Cells[TravelHistoryColumns.Description].Value, dataGridViewTravel.Font, dataGridViewTravel.Columns[TravelHistoryColumns.Description].Width - 4).Height + 2);
                        int infoh = (int)(g.MeasureString(infotext, dataGridViewTravel.Font, dataGridViewTravel.Columns[TravelHistoryColumns.Information].Width - 4).Height + 2);
                        int noteh = (int)(g.MeasureString((string)dataGridViewTravel.Rows[leftclickrow].Cells[TravelHistoryColumns.Note].Value, dataGridViewTravel.Font, dataGridViewTravel.Columns[TravelHistoryColumns.Note].Width - 4).Height + 2);

                        h = Math.Max(desch, h);
                        h = Math.Max(infoh, h);
                        h = Math.Max(noteh, h);
                    }
                }

                toexpand = (h > DefaultRowHeight);      // now we have our h, is it bigger? If so, we need to go into wrap mode

                dataGridViewTravel.Rows[leftclickrow].Height = h;
                dataGridViewTravel.Rows[leftclickrow].Cells[TravelHistoryColumns.Information].Value = infotext;

                DataGridViewTriState ti = (toexpand) ? DataGridViewTriState.True : DataGridViewTriState.False;

                dataGridViewTravel.Rows[leftclickrow].Cells[TravelHistoryColumns.Information].Style.WrapMode = ti;
                dataGridViewTravel.Rows[leftclickrow].Cells[TravelHistoryColumns.Description].Style.WrapMode = ti;
                dataGridViewTravel.Rows[leftclickrow].Cells[TravelHistoryColumns.Note].Style.WrapMode = ti;
            }
        }

        #endregion

        #region TravelHistoryRightClick

        private void historyContextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (dataGridViewTravel.SelectedCells.Count == 0)      // need something selected  stops context menu opening on nothing..
                e.Cancel = true;

            HistoryEntry prev = discoveryform.history.PreviousFrom(rightclicksystem, true);    // null can be passed in safely

            mapGotoStartoolStripMenuItem.Enabled = (rightclicksystem != null && rightclicksystem.System.HasCoordinate);
            viewOnEDSMToolStripMenuItem.Enabled = (rightclicksystem != null);
            removeJournalEntryToolStripMenuItem.Enabled = (rightclicksystem != null);
        }

        private void mapGotoStartoolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!discoveryform.Map.Is3DMapsRunning)            // if not running, click the 3dmap button
                discoveryform.Open3DMap(GetCurrentHistoryEntry);

            if (discoveryform.Map.Is3DMapsRunning)             // double check here! for paranoia.
            {
                if (discoveryform.Map.MoveToSystem(rightclicksystem.System))
                    discoveryform.Map.Show();
            }
        }

        private void starMapColourToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEnumerable<DataGridViewRow> selectedRows = dataGridViewTravel.SelectedCells.Cast<DataGridViewCell>()
                                                           .Select(cell => cell.OwningRow)
                                                           .Distinct();
            ColorDialog mapColorDialog = new ColorDialog();
            mapColorDialog.AllowFullOpen = true;
            mapColorDialog.FullOpen = true;
            HistoryEntry sp2 = (HistoryEntry)selectedRows.First().Cells[TravelHistoryColumns.HistoryTag].Tag;
            mapColorDialog.Color = Color.FromArgb(sp2.MapColour);

            if (mapColorDialog.ShowDialog(this) == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;

                foreach (DataGridViewRow r in selectedRows)
                {
                    HistoryEntry sp = (HistoryEntry)r.Cells[TravelHistoryColumns.HistoryTag].Tag;
                    System.Diagnostics.Debug.Assert(sp != null);
                    sp.UpdateMapColour(mapColorDialog.Color.ToArgb());
                }

                this.Cursor = Cursors.Default;
                Display(current_historylist);
            }
        }

        private void hideSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEnumerable<DataGridViewRow> selectedRows = dataGridViewTravel.SelectedCells.Cast<DataGridViewCell>()
                .Select(cell => cell.OwningRow)
                .Distinct();

            this.Cursor = Cursors.WaitCursor;

            foreach (DataGridViewRow r in selectedRows)
            {
                HistoryEntry sp = (HistoryEntry)r.Cells[TravelHistoryColumns.HistoryTag].Tag;
                System.Diagnostics.Debug.Assert(sp != null);
                sp.UpdateCommanderID(-1);
            }

            // Remove rows
            if (selectedRows.Count<DataGridViewRow>() == dataGridViewTravel.Rows.Count)
            {
                dataGridViewTravel.Rows.Clear();
            }
            else
            {
                foreach (DataGridViewRow row in selectedRows.ToList<DataGridViewRow>())
                {
                    dataGridViewTravel.Rows.Remove(row);
                }
            }

            this.Cursor = Cursors.Default;
        }

        private void moveToAnotherCommanderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEnumerable<DataGridViewRow> selectedRows = dataGridViewTravel.SelectedCells.Cast<DataGridViewCell>()
                .Select(cell => cell.OwningRow)
                .Distinct();

            List<HistoryEntry> listsyspos = new List<HistoryEntry>();

            this.Cursor = Cursors.WaitCursor;
            foreach (DataGridViewRow r in selectedRows)
            {
                HistoryEntry sp = (HistoryEntry)r.Cells[TravelHistoryColumns.HistoryTag].Tag;
                System.Diagnostics.Debug.Assert(sp != null);
                listsyspos.Add(sp);
            }

            EDDiscovery2.MoveToCommander movefrm = new EDDiscovery2.MoveToCommander();

            movefrm.Init(listsyspos.Count > 1);

            DialogResult red = movefrm.ShowDialog();
            if (red == DialogResult.OK)
            {
                foreach (HistoryEntry sp in listsyspos)
                {
                    sp.UpdateCommanderID(movefrm.selectedCommander.Nr);
                }

                foreach (DataGridViewRow row in selectedRows)
                {
                    dataGridViewTravel.Rows.Remove(row);
                }
            }

            this.Cursor = Cursors.Default;
        }

        private void trilaterationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSystemToOthers(true, false, false);
        }

        private void wantedSystemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSystemToOthers(false, true, false);
        }

        private void bothToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSystemToOthers(true, true, false);
        }

        private void routeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSystemToOthers(false,false,true);
        }

        private void AddSystemToOthers(bool dist, bool wanted, bool route)
        {
            TrilaterationControl tctrl = discoveryform.trilaterationControl;

            IEnumerable<DataGridViewRow> selectedRows = dataGridViewTravel.SelectedCells.Cast<DataGridViewCell>()
                                                                        .Select(cell => cell.OwningRow)
                                                                        .Distinct()
                                                                        .OrderBy(cell => cell.Index);

            this.Cursor = Cursors.WaitCursor;

            List<string> systemnamelist = new List<string>();

            string lastname = "";
            foreach (DataGridViewRow r in selectedRows)
            {
                HistoryEntry sp = (HistoryEntry)r.Cells[TravelHistoryColumns.HistoryTag].Tag;

                if (!sp.System.name.Equals(lastname))
                {
                    lastname = sp.System.name;
                    systemnamelist.Add(lastname);
                }
            }

            if (dist)
            {
                foreach (string s in systemnamelist)
                    tctrl.AddSystemToDataGridViewDistances(s);
            }

            if (wanted)
            {
                foreach (string s in systemnamelist)
                    tctrl.AddWantedSystem(s);
            }

            if (route)
            {
                discoveryform.savedRouteExpeditionControl1.AppendRows(systemnamelist.ToArray());
            }

            this.Cursor = Cursors.Default;
        }

        private void viewOnEDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            EDDiscovery2.EDSM.EDSMClass edsm = new EDSMClass();
            long? id_edsm = rightclicksystem.System?.id_edsm;

            if (id_edsm <= 0)
            {
                id_edsm = null;
            }

            if (!edsm.ShowSystemInEDSM(rightclicksystem.System.name, id_edsm))
                MessageBox.Show("System could not be found - has not been synched or EDSM is unavailable");

            this.Cursor = Cursors.Default;
        }

        private void selectCorrectSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<JournalEntry> jents = JournalEntry.GetAll(EDDiscovery2.EDDConfig.Instance.CurrentCmdrID).OrderBy(j => j.EventTimeUTC).ThenBy(j => j.Id).ToList();
            int selindex = jents.FindIndex(j => j.Id == rightclicksystem.Journalid);
            int firstrow = selindex;
            int lastrow = selindex;

            if (selindex < 0)
            {
                // Selected entry is not in history for commander - abort.
                return;
            }

            EliteDangerous.JournalEvents.JournalLocOrJump journalent = null;

            if (jents[selindex].EventTypeID != JournalTypeEnum.FSDJump)
            {
                for (int i = selindex - 1; i >= 0; i--)
                {
                    var jent = jents[i];
                    if (jent.EdsmID != rightclicksystem.System.id_edsm || jent.EventTypeID == JournalTypeEnum.Died)
                        break;
                    firstrow = i;
                    if (jent.EventTypeID == JournalTypeEnum.FSDJump)
                        break;
                }
            }

            for (int i = rightclickrow + 1; i < dataGridViewTravel.RowCount; i++)
            {
                var jent = jents[i];
                if (jent.EdsmID != rightclicksystem.System.id_edsm || jent.EventTypeID == JournalTypeEnum.FSDJump)
                    break;
                lastrow = i;
                if (jent.EventTypeID == JournalTypeEnum.Died)
                    break;
            }

            var _jents = jents;
            jents = new List<JournalEntry>();

            for (int i = firstrow; i <= lastrow; i++)
            {
                jents.Add(_jents[i]);
            }

            journalent = jents.OfType<EliteDangerous.JournalEvents.JournalLocOrJump>().FirstOrDefault();

            if (journalent == null)
            {
                MessageBox.Show("Could not find Location or FSDJump entry associated with selected journal entry");
                return;
            }

            using (Forms.AssignTravelLogSystemForm form = new Forms.AssignTravelLogSystemForm(journalent))
            {
                DialogResult result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    foreach (var jent in jents)
                    {
                        jent.EdsmID = (int)form.AssignedEdsmId;
                        jent.Update();
                    }

                    discoveryform.RefreshHistoryAsync();
                }
            }
        }

        private void toolStripMenuItemStartStop_Click(object sender, EventArgs e)
        {
            if (rightclicksystem != null)
            {
                discoveryform.history.SetStartStop(rightclicksystem);
                discoveryform.RefreshHistoryAsync();
            }
        }

        private void removeJournalEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm you wish to remove this entry" + Environment.NewLine + "It may reappear if the logs are rescanned", "WARNING", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                JournalEntry.Delete(rightclicksystem.Journalid);
                discoveryform.RefreshHistoryAsync();
            }
        }

        #endregion

        #region Event Filter

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

        private void buttonField_Click(object sender, EventArgs e)
        {
            EDDiscovery2.JSONFiltersForm frm = new JSONFiltersForm();
            frm.Init("History: Filter out fields", "Filter Out", true, discoveryform.theme, fieldfilter);
            frm.TopMost = this.FindForm().TopMost;
            if (frm.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                fieldfilter = frm.result;
                SQLiteDBClass.PutSettingString(DbFieldFilter, fieldfilter.GetJSON());
                Display(current_historylist);
            }
        }

        #endregion

        private void drawnPanelPopOut_Click(object sender, EventArgs e)
        {
            if (OnPopOut != null)
                OnPopOut();
        }

    }

}
