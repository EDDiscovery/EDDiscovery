﻿/*
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
using EliteDangerousCore.DB;
using EliteDangerousCore;
using EliteDangerousCore.EDSM;
using EliteDangerousCore.EDDN;
using EDDiscovery.Export;

namespace EDDiscovery.UserControls
{
    public partial class UserControlTravelGrid : UserControlCommonBase
    {
        #region Public IF

        public DataGridViewRow GetCurrentRow { get { return dataGridViewTravel.CurrentCell != null ? dataGridViewTravel.Rows[dataGridViewTravel.CurrentCell.RowIndex] : null; } }
        public HistoryEntry GetCurrentHistoryEntry { get { return dataGridViewTravel.CurrentCell != null ? dataGridViewTravel.Rows[dataGridViewTravel.CurrentCell.RowIndex].Cells[TravelHistoryColumns.HistoryTag].Tag as HistoryEntry : null; } }

        public HistoryEntry GetHistoryEntry(int r) { return dataGridViewTravel.Rows[r].Cells[TravelHistoryColumns.HistoryTag].Tag as HistoryEntry; }

        public static HistoryEntry GetHistoryEntry(DataGridViewRow rw) { return rw.Cells[TravelHistoryColumns.HistoryTag].Tag as HistoryEntry; }

        public DataGridViewRow GetRow(int r) { return dataGridViewTravel.Rows[r]; }

        public TravelHistoryFilter GetHistoryFilter { get { return (TravelHistoryFilter)comboBoxHistoryWindow.SelectedItem ?? TravelHistoryFilter.NoFilter; } }

        #endregion

        #region Events

        public delegate void ChangedSelection(int rowno, int colno, bool doubleclick, bool note);
        public event ChangedSelection OnChangedSelection;   // After a change of selection

        public delegate void KeyDownInCell(int asciikeycode, int rowno, int colno, bool note);
        public event KeyDownInCell OnKeyDownInCell;   // After a change of selection

        public delegate void Resort();
        public event Resort OnResort;               // After a sort

        public delegate void AddedNewEntry(HistoryEntry he, HistoryList hl, bool accepted);
        public AddedNewEntry OnAddedNewEntry;       // FIRED after discoveryform.onNewEntry->this.AddNewEntry completes

        public delegate void Redisplay(HistoryList hl);
        public Redisplay OnRedisplay;               // FIRED after discoveryform.onHistoryChange->this.Display

        public delegate void PopOut();
        public PopOut OnPopOut;                     // pop out button pressed

        #endregion

        #region Init

        private class TravelHistoryColumns
        {
            public const int Time = 0;
            public const int Icon = 1;
            public const int Description = 2;
            public const int Information = 3;
            public const int Note = 4;

            public const int HistoryTag = Description;      // where the tags are used
        }

        private const int DefaultRowHeight = 26;

        private static EDDiscoveryForm discoveryform;
        private int displaynumber;

        private string DbFilterSave { get { return "TravelHistoryControlEventFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbColumnSave { get { return "TravelControl" + ((displaynumber > 0) ? displaynumber.ToString() : "") + "DGVCol"; } }
        private string DbHistorySave { get { return "EDUIHistory" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbFieldFilter { get { return "TravelHistoryControlFieldFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        private HistoryList current_historylist;        // the last one set, for internal refresh purposes on sort

        private Conditions.ConditionLists fieldfilter = new Conditions.ConditionLists();

        private Dictionary<long, DataGridViewRow> rowsbyjournalid = new Dictionary<long, DataGridViewRow>();

        EventFilterSelector cfs = new EventFilterSelector();

        public UserControlTravelGrid()
        {
            InitializeComponent();
            Name = "History";
            this.textBoxFilter.SetToolTip(toolTip1, "Display entries matching this string");
        }

        public override void Init(EDDiscoveryForm ed, int vn) //0=primary, 1 = first windowed version, etc
        {
            discoveryform = ed;
            displaynumber = vn;
            cfs.ConfigureThirdOption("Travel", "Docked;FSD Jump;Undocked;");
            cfs.Changed += EventFilterChanged;
            TravelHistoryFilter.InitaliseComboBox(comboBoxHistoryWindow, DbHistorySave);

            discoveryform.OnHistoryChange += Display;
            discoveryform.OnNewEntry += AddNewEntry;
            discoveryform.OnNoteChanged += OnNoteChanged;

            dataGridViewTravel.MakeDoubleBuffered();
            dataGridViewTravel.RowTemplate.Height = DefaultRowHeight;

            string filter = SQLiteDBClass.GetSettingString(DbFieldFilter, "");
            if (filter.Length > 0)
                fieldfilter.FromJSON(filter);        // load filter

#if !DEBUG
            writeEventInfoToLogDebugToolStripMenuItem.Visible = false;
#endif
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

        public override void Display(HistoryEntry current, HistoryList history)
        {
            Display(history);
        }

        public void Display(HistoryList hl)           // rowno current.. -1 if nothing
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

            result = FilterHelpers.FilterHistory(result, fieldfilter, discoveryform.Globals, out ftotal);
            toolTip1.SetToolTip(buttonField, (ftotal > 0) ? ("Total filtered out " + ftotal) : "Filter out entries matching the field selection");

            dataGridViewTravel.Rows.Clear();
            rowsbyjournalid.Clear();

            for (int ii = 0; ii < result.Count; ii++) //foreach (var item in result)
            {
                AddNewHistoryRow(false, result[ii]);      // for every one in filter, add a row.
            }

            StaticFilters.FilterGridView(dataGridViewTravel, textBoxFilter.Text);

            int rowno = FindGridPosByJID(pos.Item1, true);     // find row.. must be visible..  -1 if not found/not visible

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
            //string debugt = item.Journalid + "  " + item.System.id_edsm + " " + item.System.GetHashCode() + " "; // add on for debug purposes to a field below

            object[] rowobj = { EDDiscoveryForm.EDDConfig.DisplayUTC ? item.EventTimeUTC : item.EventTimeLocal, "", item.EventSummary, item.EventDescription, (item.snc != null) ? item.snc.Note : "" };

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

            rowsbyjournalid[item.Journalid] = dataGridViewTravel.Rows[rownr];
            dataGridViewTravel.Rows[rownr].Cells[TravelHistoryColumns.HistoryTag].Tag = item;

            dataGridViewTravel.Rows[rownr].DefaultCellStyle.ForeColor = (item.System.HasCoordinate || item.EntryType != JournalTypeEnum.FSDJump) ? discoveryform.theme.VisitedSystemColor : discoveryform.theme.NonVisitedSystemColor;

            string tip = item.EventSummary + Environment.NewLine + item.EventDescription + Environment.NewLine + item.EventDetailedInfo;

            dataGridViewTravel.Rows[rownr].Cells[0].ToolTipText = tip;
            dataGridViewTravel.Rows[rownr].Cells[1].ToolTipText = tip;
            dataGridViewTravel.Rows[rownr].Cells[2].ToolTipText = tip;
            dataGridViewTravel.Rows[rownr].Cells[3].ToolTipText = tip;
            dataGridViewTravel.Rows[rownr].Cells[4].ToolTipText = tip;

#if DEBUGVOICE
            List<Actions.ActionFileList.MatchingSets> ale = discoveryform.actionfiles.GetMatchingConditions(item.journalEntry.EventTypeStr);
            dataGridViewTravel.Rows[rownr].Cells[3].Value = ((ale.Count>0) ? "VOICE " :"NO VOICE") + dataGridViewTravel.Rows[rownr].Cells[3].Value;
#endif
        }

        public bool WouldAddEntry(HistoryEntry he)                  // do we filter? if its not in the journal event filter, or it is in the field filter
        {
            return he.IsJournalEventInEventFilter(SQLiteDBClass.GetSettingString(DbFilterSave, "All")) && FilterHelpers.FilterHistory(he, fieldfilter, discoveryform.Globals);
        }

        public void SelectTopRow()
        {
            dataGridViewTravel.ClearSelection();
            dataGridViewTravel.CurrentCell = dataGridViewTravel.Rows[0].Cells[1];       // its the current cell which needs to be set, moves the row marker as well
        }

        Tuple<long, int> CurrentGridPosByJID()          // Returns JID, column index.  JID = -1 if cell is not defined
        {
            long jid = (dataGridViewTravel.CurrentCell != null) ? ((HistoryEntry)(dataGridViewTravel.Rows[dataGridViewTravel.CurrentCell.RowIndex].Cells[TravelHistoryColumns.HistoryTag].Tag)).Journalid : -1;
            int cellno = (dataGridViewTravel.CurrentCell != null) ? dataGridViewTravel.CurrentCell.ColumnIndex : 0;
            return new Tuple<long, int>(jid, cellno);
        }

        int FindGridPosByJID(long jid, bool checkvisible)
        {
            if (rowsbyjournalid.ContainsKey(jid) && (!checkvisible || rowsbyjournalid[jid].Visible))
                return rowsbyjournalid[jid].Index;
            else
                return -1;
        }

        public void GotoPosByJID(long jid)
        {
            int rowno = FindGridPosByJID(jid, true);
            if (rowno >= 0)
            {
                dataGridViewTravel.CurrentCell = dataGridViewTravel.Rows[rowno].Cells[TravelHistoryColumns.Information];
                dataGridViewTravel.Rows[rowno].Selected = true;
            }
        }

        private void comboBoxHistoryWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingString(DbHistorySave, comboBoxHistoryWindow.Text);

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
            if (OnChangedSelection != null)
                OnChangedSelection(e.RowIndex, e.ColumnIndex, false, e.ColumnIndex == TravelHistoryColumns.Note);
        }

        int keyrepeatcount = 0;     // 1 is first down, 2 is second.  on 2+ we call the check selection to update the screen.  The final key up finished the job.

        private void dataGridViewTravel_KeyDown(object sender, KeyEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Key down " + e.KeyCode + " " + dataGridViewTravel.CurrentCell.RowIndex + ":" + dataGridViewTravel.CurrentCell.ColumnIndex);
            keyrepeatcount++;

            if (keyrepeatcount > 1)
                CheckForSelection(e.KeyCode);

            //System.Diagnostics.Debug.WriteLine("KC " + (int)e.KeyCode + " " + (int)e.KeyData + " " + e.KeyValue);
        }

        private void dataGridViewTravel_KeyPress(object sender, KeyPressEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("KP " + (int)e.KeyChar);

            if (OnKeyDownInCell != null && dataGridViewTravel.CurrentCell != null )
                OnKeyDownInCell(e.KeyChar, dataGridViewTravel.CurrentCell.RowIndex, dataGridViewTravel.CurrentCell.ColumnIndex, dataGridViewTravel.CurrentCell.ColumnIndex == TravelHistoryColumns.Note);
        }

        private void dataGridViewTravel_KeyUp(object sender, KeyEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Key up " + e.KeyCode + " " + dataGridViewTravel.CurrentCell.RowIndex + ":" + dataGridViewTravel.CurrentCell.ColumnIndex);
            CheckForSelection(e.KeyCode);
            keyrepeatcount = 0;
        }

        void CheckForSelection(Keys code)
        { 
            bool cursorkeydown = (code == Keys.Up || code == Keys.Down || code == Keys.PageDown || code == Keys.PageUp || code == Keys.Left || code == Keys.Right);

            if (cursorkeydown)
            {
                if (OnChangedSelection != null)
                    OnChangedSelection(dataGridViewTravel.CurrentCell.RowIndex, dataGridViewTravel.CurrentCell.ColumnIndex, false, dataGridViewTravel.CurrentCell.ColumnIndex == TravelHistoryColumns.Note);
            }
        }

        private void OnNoteChanged(Object sender,HistoryEntry he, bool committed)
        {
            if (rowsbyjournalid.ContainsKey(he.Journalid) ) // if we can find the grid entry
            {
                string s = (he.snc != null) ? he.snc.Note : "";     // snc may have gone null, so cope with it
                rowsbyjournalid[he.Journalid].Cells[TravelHistoryColumns.Note].Value = s;
            }
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            Tuple<long, int> pos = CurrentGridPosByJID();

            StaticFilters.FilterGridView(dataGridViewTravel, textBoxFilter.Text);

            int rowno = FindGridPosByJID(pos.Item1,true);
            if (rowno >= 0)
                dataGridViewTravel.CurrentCell = dataGridViewTravel.Rows[rowno].Cells[pos.Item2];
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
            System.Diagnostics.Debug.Assert(he != null);

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
                        h += 20;
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
            sendUnsyncedScanToEDDNToolStripMenuItem.Enabled = (rightclicksystem != null && rightclicksystem.EntryType == JournalTypeEnum.Scan && !rightclicksystem.EDDNSync);
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
                rowsbyjournalid.Remove(sp.Journalid);
            }

            // Remove rows
            if (selectedRows.Count<DataGridViewRow>() == dataGridViewTravel.Rows.Count)
            {
                dataGridViewTravel.Rows.Clear();
                rowsbyjournalid.Clear();
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
                rowsbyjournalid.Remove(sp.Journalid);
            }

            EDDiscovery.Forms.MoveToCommander movefrm = new EDDiscovery.Forms.MoveToCommander();

            movefrm.Init();

            DialogResult red = movefrm.ShowDialog(this);
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
            AddSystemToOthers(false, false, true);
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
            EliteDangerousCore.EDSM.EDSMClass edsm = new EDSMClass();
            long? id_edsm = rightclicksystem.System?.id_edsm;

            if (id_edsm <= 0)
            {
                id_edsm = null;
            }

            if (!edsm.ShowSystemInEDSM(rightclicksystem.System.name, id_edsm))
                ExtendedControls.MessageBoxTheme.Show("System could not be found - has not been synched or EDSM is unavailable");

            this.Cursor = Cursors.Default;
        }

        private void selectCorrectSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<JournalEntry> jents = JournalEntry.GetAll(EDCommander.CurrentCmdrID).OrderBy(j => j.EventTimeUTC).ThenBy(j => j.Id).ToList();
            int selindex = jents.FindIndex(j => j.Id == rightclicksystem.Journalid);
            int firstrow = selindex;
            int lastrow = selindex;

            if (selindex < 0)
            {
                // Selected entry is not in history for commander - abort.
                return;
            }

            EliteDangerousCore.JournalEvents.JournalLocOrJump journalent = null;

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

            journalent = jents.OfType<EliteDangerousCore.JournalEvents.JournalLocOrJump>().FirstOrDefault();

            if (journalent == null)
            {
                ExtendedControls.MessageBoxTheme.Show("Could not find Location or FSDJump entry associated with selected journal entry");
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
            if (ExtendedControls.MessageBoxTheme.Show("Confirm you wish to remove this entry" + Environment.NewLine + "It may reappear if the logs are rescanned", "WARNING", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                JournalEntry.Delete(rightclicksystem.Journalid);
                discoveryform.RefreshHistoryAsync();
            }
        }

        private void sendUnsyncedScanToEDDNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclicksystem != null && rightclicksystem.EntryType == JournalTypeEnum.Scan && !rightclicksystem.EDDNSync)
            {
                EDDNSync.SendEDDNEvent(discoveryform.LogLine, rightclicksystem);
            }
        }

        private void runActionsOnThisEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclicksystem != null)
                discoveryform.ActionRunOnEntry(rightclicksystem, "UserRightClick");
        }

        private void setNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclicksystem != null)
            {
                using (Forms.SetNoteForm noteform = new Forms.SetNoteForm(rightclicksystem, discoveryform))
                {
                    if (noteform.ShowDialog(this) == DialogResult.OK)
                    {
                        rightclicksystem.SetJournalSystemNoteText(noteform.NoteText, true);
                        discoveryform.NoteChanged(this,rightclicksystem, true);
                    }
                }
            }
        }

        private void writeEventInfoToLogDebugToolStripMenuItem_Click(object sender, EventArgs e)        //DEBUG ONLY
        {
            Conditions.ConditionVariables cv = new Conditions.ConditionVariables();
            cv.AddPropertiesFieldsOfClass(rightclicksystem.journalEntry, "", new Type[] { typeof(System.Drawing.Bitmap), typeof(Newtonsoft.Json.Linq.JObject) }, 5);
            discoveryform.LogLine(cv.ToString(separ: Environment.NewLine, quoteit: false));
            if (rightclicksystem.ShipInformation != null)
                discoveryform.LogLine(rightclicksystem.ShipInformation.ToString());
        }

        private void copyJournalEntryToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclicksystem != null && rightclicksystem.journalEntry != null)
            {
                Newtonsoft.Json.Linq.JObject jo = rightclicksystem.journalEntry.GetJson();
                string json = jo?.ToString();
                if (json != null)
                {
                    Clipboard.SetText(json);
                }
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
            Conditions.ConditionFilterForm frm = new Conditions.ConditionFilterForm();
            frm.InitFilter("History: Filter out fields",
                            System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location),
                            JournalEntry.GetListOfEventsWithOptMethod(false),
                            (s) => { return BaseUtils.FieldNames.GetPropertyFieldNames(JournalEntry.TypeOfJournalEntry(s)); },
                            discoveryform.Globals.NameList, fieldfilter);
            frm.TopMost = this.FindForm().TopMost;
            if (frm.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                fieldfilter = frm.result;
                SQLiteDBClass.PutSettingString(DbFieldFilter, fieldfilter.GetJSON());
                Display(current_historylist);
            }
        }

        #endregion

        #region Excel

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.Filter = "CSV export| *.csv";
            dlg.Title = "Export current History view to Excel (csv)";
                            // 0        1       2           3       4       5           6           7                   8       9               10              11              12
            string[] colh = { "Time", "Event", "System", "Body", "Ship" , "Summary", "Description", "Detailed Info", "Note" , "Travel Dist", "Travel Time" , "Travel Jumps" , "Travelled MisJumps" };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Export.ExportGrid grd = new ExportGrid();
                grd.onGetCell += delegate (int r, int c)
                {
                    if (c == -1)    // next line?
                        return r < dataGridViewTravel.Rows.Count;
                    else if (c < colh.Length && dataGridViewTravel.Rows[r].Visible)
                    {
                        HistoryEntry he = (HistoryEntry)dataGridViewTravel.Rows[r].Cells[TravelHistoryColumns.HistoryTag].Tag;
                        if (c == 0)
                            return dataGridViewTravel.Rows[r].Cells[0].Value;
                        else if (c == 1)
                            return he.journalEntry.EventTypeStr;
                        else if (c == 2)
                            return (he.System != null) ? he.System.name : "Unknown";    // paranoia
                        else if (c == 3)
                            return he.WhereAmI;
                        else if (c == 4)
                            return he.ShipInformation != null ? he.ShipInformation.Name : "Unknown";
                        else if (c == 5)
                            return he.EventSummary;
                        else if (c == 6)
                            return he.EventDescription;
                        else if (c == 7)
                            return he.EventDetailedInfo;
                        else if (c == 8)
                            return dataGridViewTravel.Rows[r].Cells[4].Value;
                        else if (c == 9)
                            return he.isTravelling ? he.TravelledDistance.ToString("0.0") : "";
                        else if (c == 10)
                            return he.isTravelling ? he.TravelledSeconds.ToString() : "";
                        else if (c == 11)
                            return he.isTravelling ? he.Travelledjumps.ToStringInvariant() : "";
                        else 
                            return he.isTravelling ? he.TravelledMissingjump.ToStringInvariant() : "";
                    }
                    else
                        return null;
                };

                grd.onGetHeader += delegate (int c)
                {
                    return (c < colh.Length) ? colh[c] : null;
                };

                grd.Csvformat = discoveryform.ExportControl.radioButtonCustomEU.Checked ? BaseUtils.CVSWrite.CSVFormat.EU : BaseUtils.CVSWrite.CSVFormat.USA_UK;
                if (grd.ToCSV(dlg.FileName))
                    System.Diagnostics.Process.Start(dlg.FileName);
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
