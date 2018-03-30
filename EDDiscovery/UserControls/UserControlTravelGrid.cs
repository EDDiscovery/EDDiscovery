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
using EliteDangerousCore.DB;
using EliteDangerousCore;
using EliteDangerousCore.EDSM;
using EliteDangerousCore.EDDN;
using EDDiscovery.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlTravelGrid : UserControlCommonBase, IHistoryCursor
    {
        #region Public IF

        //ucct interface
        public HistoryEntry GetCurrentHistoryEntry { get { return dataGridViewTravel.CurrentCell != null ? dataGridViewTravel.Rows[dataGridViewTravel.CurrentCell.RowIndex].Cells[TravelHistoryColumns.HistoryTag].Tag as HistoryEntry : null; } }

        //horrible.. needs removing, used by export.
        public TravelHistoryFilter GetHistoryFilter { get { return (TravelHistoryFilter)comboBoxHistoryWindow.SelectedItem ?? TravelHistoryFilter.NoFilter; } }

        #endregion

        #region Events

        // implement IHistoryCursor fields
        public event ChangedSelectionHandler OnChangedSelection;   // After a change of selection by the user, or after a OnHistoryChanged, or after a sort.
        public event ChangedSelectionHEHandler OnTravelSelectionChanged;   // as above, different format, for certain older controls

        // for primary travel grid for auto note jump
        public delegate void KeyDownInCell(int asciikeycode, int rowno, int colno, bool note);
        public event KeyDownInCell OnKeyDownInCell;   // After a change of selection

        // for primary travel grid to chain stuff after display has been updated
        public delegate void AddedNewEntry(HistoryEntry he, HistoryList hl, bool accepted);
        public AddedNewEntry OnNewEntry;       // FIRED after discoveryform.onNewEntry->this.AddNewEntry completes

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

        private string DbFilterSave { get { return "TravelHistoryControlEventFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbColumnSave { get { return "TravelControl" + ((displaynumber > 0) ? displaynumber.ToString() : "") + "DGVCol"; } }
        private string DbHistorySave { get { return "EDUIHistory" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbFieldFilter { get { return "TravelHistoryControlFieldFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbAutoTop { get { return "TravelHistoryControlAutoTop" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        private HistoryList current_historylist;        // the last one set, for internal refresh purposes on sort

        private Conditions.ConditionLists fieldfilter = new Conditions.ConditionLists();

        private Dictionary<long, DataGridViewRow> rowsbyjournalid = new Dictionary<long, DataGridViewRow>();

        EventFilterSelector cfs = new EventFilterSelector();

        Timer searchtimer;

        public UserControlTravelGrid()
        {
            InitializeComponent();
            var corner = dataGridViewTravel.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            cfs.ConfigureThirdOption("Travel", "Docked;FSD Jump;Undocked;");
            cfs.Changed += EventFilterChanged;
            TravelHistoryFilter.InitaliseComboBox(comboBoxHistoryWindow, DbHistorySave);

            checkBoxMoveToTop.Checked = SQLiteConnectionUser.GetSettingBool(DbAutoTop, true);

            dataGridViewTravel.MakeDoubleBuffered();
            dataGridViewTravel.RowTemplate.Height = DefaultRowHeight;

            string filter = SQLiteDBClass.GetSettingString(DbFieldFilter, "");
            if (filter.Length > 0)
                fieldfilter.FromJSON(filter);        // load filter

#if !DEBUG
            writeEventInfoToLogDebugToolStripMenuItem.Visible = false;
            writeJournalToLogtoolStripMenuItem.Visible = false;
            runActionsAcrossSelectionToolSpeechStripMenuItem.Visible = false;
#endif

            searchtimer = new Timer() { Interval = 500 };
            searchtimer.Tick += Searchtimer_Tick;

            discoveryform.OnHistoryChange += HistoryChanged;
            discoveryform.OnNewEntry += AddNewEntry;
            discoveryform.OnNoteChanged += OnNoteChanged;
        }

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridViewTravel, DbColumnSave);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewTravel, DbColumnSave);
            discoveryform.OnHistoryChange -= HistoryChanged;
            discoveryform.OnNewEntry -= AddNewEntry;
            SQLiteConnectionUser.PutSettingBool(DbAutoTop, checkBoxMoveToTop.Checked);
            searchtimer.Dispose();
        }

        #endregion

        public override void InitialDisplay()
        {
            HistoryChanged(discoveryform.history);
        }

        int fdropdown, ftotalevents, ftotalfilters;     // filter totals
        public void HistoryChanged(HistoryList hl)           // on History change
        {
            HistoryChanged(hl, false);
        }

        public void HistoryChanged(HistoryList hl, bool disablesorting)           // on History change
        {
            if (hl == null)     // just for safety
                return;

            current_historylist = hl;

            Tuple<long, int> pos = CurrentGridPosByJID();

            SortOrder sortorder = dataGridViewTravel.SortOrder;
            int sortcol = dataGridViewTravel.SortedColumn?.Index ?? -1;
            if (sortcol >= 0 && disablesorting)
            {
                dataGridViewTravel.Columns[sortcol].HeaderCell.SortGlyphDirection = SortOrder.None;
                sortcol = -1;
            }

            var filter = (TravelHistoryFilter)comboBoxHistoryWindow.SelectedItem ?? TravelHistoryFilter.NoFilter;

            List<HistoryEntry> result = filter.Filter(hl);
            fdropdown = hl.Count() - result.Count();

            result = HistoryList.FilterByJournalEvent(result, SQLiteDBClass.GetSettingString(DbFilterSave, "All"), out ftotalevents);
            result = FilterHelpers.FilterHistory(result, fieldfilter, discoveryform.Globals, out ftotalfilters);

            
            dataGridViewTravel.Rows.Clear();
            rowsbyjournalid.Clear();

            for (int ii = 0; ii < result.Count; ii++) //foreach (var item in result)
            {
                AddNewHistoryRow(false, result[ii]);      // for every one in filter, add a row.
            }

            StaticFilters.FilterGridView(dataGridViewTravel, textBoxFilter.Text);

            UpdateToolTipsForFilter();

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

            if (sortcol >= 0)
            {
                dataGridViewTravel.Sort(dataGridViewTravel.Columns[sortcol], (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                dataGridViewTravel.Columns[sortcol].HeaderCell.SortGlyphDirection = sortorder;
            }

            FireChangeSelection();      // and since we repainted, we should fire selection, as we in effect may have selected a new one
        }

        private void AddNewEntry(HistoryEntry he, HistoryList hl)           // on new entry from discovery system
        {
            bool add = he.IsJournalEventInEventFilter(SQLiteDBClass.GetSettingString(DbFilterSave, "All"));

            if (!add)                   // filtered out, update filter total and display
            {
                ftotalevents++;
                UpdateToolTipsForFilter();
            }

            if (add && !FilterHelpers.FilterHistory(he, fieldfilter, discoveryform.Globals))
            {
                add = false;
                ftotalfilters++;
                UpdateToolTipsForFilter();
            }
            
            if (add)
                AddNewHistoryRow(true, he);

            if (OnNewEntry != null)
                OnNewEntry(he, hl, add);

            if (add)
            {
                var filter = (TravelHistoryFilter)comboBoxHistoryWindow.SelectedItem ?? TravelHistoryFilter.NoFilter;

                if (filter.MaximumNumberOfItems != null)
                {
                    for (int r = dataGridViewTravel.Rows.Count - 1; r >= dataGridViewTravel.Rows.Count; r--)
                    {
                        dataGridViewTravel.Rows.RemoveAt(r);
                    }
                }

                if (filter.MaximumDataAge != null)
                {
                    for (int r = dataGridViewTravel.Rows.Count - 1; r > 0; r--)
                    {
                        var rhe = dataGridViewTravel.Rows[r].Tag as HistoryEntry;
                        if (rhe != null && rhe.AgeOfEntry() > filter.MaximumDataAge)
                        {
                            dataGridViewTravel.Rows.RemoveAt(r);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (checkBoxMoveToTop.Checked && dataGridViewTravel.DisplayedRowCount(false) > 0)   // Move focus to new row
                {
                    //System.Diagnostics.Debug.WriteLine("Auto Sel");
                    dataGridViewTravel.ClearSelection();
                    int rowno = dataGridViewTravel.Rows.GetFirstRow(DataGridViewElementStates.Visible);
                    if (rowno != -1)
                        dataGridViewTravel.CurrentCell = dataGridViewTravel.Rows[rowno].Cells[1];       // its the current cell which needs to be set, moves the row marker as well

                    FireChangeSelection();
                }
            }
        }

        private void AddNewHistoryRow(bool insert, HistoryEntry item)            // second part of add history row, adds item to view.
        {
            //string debugt = item.Journalid + "  " + item.System.id_edsm + " " + item.System.GetHashCode() + " "; // add on for debug purposes to a field below

            var rw = dataGridViewTravel.RowTemplate.Clone() as DataGridViewRow;
            rw.CreateCells(dataGridViewTravel, EDDiscoveryForm.EDDConfig.DisplayUTC ? item.EventTimeUTC : item.EventTimeLocal, "", item.EventSummary, item.EventDescription, (item.snc != null) ? item.snc.Note : "");
            rw.Cells[TravelHistoryColumns.HistoryTag].Tag = item;

            int rownr = 0;
            if (insert)
            {
                dataGridViewTravel.Rows.Insert(rownr, rw);
            }
            else
            {
                rownr = dataGridViewTravel.Rows.Add(rw);
            }

            rowsbyjournalid[item.Journalid] = dataGridViewTravel.Rows[rownr];

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

        private void UpdateToolTipsForFilter()
        {
            string ms = " showing " + dataGridViewTravel.Rows.Count + " original " + (current_historylist?.Count()??0);
            comboBoxHistoryWindow.SetTipDynamically(toolTip, fdropdown > 0 ? ("Filtered " + fdropdown + ms) : "Select the entries by age, " + ms);
            toolTip.SetToolTip(buttonFilter, (ftotalevents > 0) ? ("Filtered " + ftotalevents + ms) : "Filter out entries based on event type, " + ms);
            toolTip.SetToolTip(buttonField, (ftotalfilters > 0) ? ("Total filtered out " + ftotalfilters + ms) : "Filter out entries matching the field selection, " + ms);
        }

        private void dataGridViewTravel_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if ( e.Column.Index == 0 )
            {
                e.SortDataGridViewColumnDate();
            }
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
                FireChangeSelection();
            }
        }

        private void comboBoxHistoryWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingString(DbHistorySave, comboBoxHistoryWindow.Text);

            if (current_historylist != null)
            {
                HistoryChanged(current_historylist);        // fires lots of events
            }
        }

        public void FireChangeSelection()
        {
            if (dataGridViewTravel.CurrentCell != null)
            {
                int row = dataGridViewTravel.CurrentCell.RowIndex;
                //System.Diagnostics.Debug.WriteLine("Fire Change Sel row" + row);
                if (OnChangedSelection != null)
                    OnChangedSelection(row, dataGridViewTravel.CurrentCell.ColumnIndex, false, dataGridViewTravel.CurrentCell.ColumnIndex == TravelHistoryColumns.Note);
                if (OnTravelSelectionChanged != null)
                    OnTravelSelectionChanged(dataGridViewTravel.Rows[row].Cells[TravelHistoryColumns.HistoryTag].Tag as HistoryEntry, current_historylist);
            }
        }

        private void dataGridViewTravel_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            FireChangeSelection();
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
                FireChangeSelection();
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
            searchtimer.Stop();
            searchtimer.Start();
            //System.Diagnostics.Debug.WriteLine(Environment.TickCount % 10000 + "Char");
        }

        private void Searchtimer_Tick(object sender, EventArgs e)
        {
            searchtimer.Stop();
            this.Cursor = Cursors.WaitCursor;

            //System.Diagnostics.Debug.WriteLine(Environment.TickCount % 10000 + "Searching");
            Tuple<long, int> pos = CurrentGridPosByJID();

            StaticFilters.FilterGridView(dataGridViewTravel, textBoxFilter.Text);

            int rowno = FindGridPosByJID(pos.Item1, true);
            if (rowno >= 0)
                dataGridViewTravel.CurrentCell = dataGridViewTravel.Rows[rowno].Cells[pos.Item2];

            this.Cursor = Cursors.Default;
            //System.Diagnostics.Debug.WriteLine(Environment.TickCount % 10000 + "Complete");
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
            System.Diagnostics.Debug.Assert(he != null);    // Trip for debug builds if something is wrong,
            if (he == null)                                 // otherwise, ignore it and return.
                return;

            string rowIdx;

            if (EDDConfig.Instance.OrderRowsInverted)
                rowIdx = he.Indexno.ToString();            // oldest has the highest index
            else
                rowIdx = (totalentries - he.Indexno + 1).ToString();

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);

            // right alignment might actually make more sense for numbers
            using (var centerFormat = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Brush br = new SolidBrush(grid.RowHeadersDefaultCellStyle.ForeColor))
                    e.Graphics.DrawString(rowIdx, grid.RowHeadersDefaultCellStyle.Font, br, headerBounds, centerFormat);
            }

            int noicons = (he.IsFSDJump && showfsdmapcolour) ? 2 : 1;
            if (he.StartMarker || he.StopMarker)
                noicons++;

            BookmarkClass bk = null;
            if (he.IsLocOrJump)
            {
                bk = GlobalBookMarkList.Instance.FindBookmarkOnSystem(he.System.Name);
                if (bk != null)
                    noicons++;
            }

            int padding = 4;
            int size = 24;

            if (size * noicons > (colwidth - 2))
                size = (colwidth - 2) / noicons;

            int hstart = (hpos + colwidth / 2) - size / 2 * noicons - padding / 2 * (noicons - 1);

            int top = (e.RowBounds.Top + e.RowBounds.Bottom) / 2 - size / 2;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

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
            {
                e.Graphics.DrawImage(Icons.Controls.TravelGrid_FlagStart, new Rectangle(hstart, top, size, size));
                hstart += size + padding;
            }
            else if (he.StopMarker)
            {
                e.Graphics.DrawImage(Icons.Controls.TravelGrid_FlagStop, new Rectangle(hstart, top, size, size));
                hstart += size + padding;
            }
            if (bk != null)
            {
                e.Graphics.DrawImage(Icons.Controls.Map3D_Bookmarks_Star, new Rectangle(hstart, top, size, size));
            }
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
            removeSortingOfColumnsToolStripMenuItem.Enabled = dataGridViewTravel.SortedColumn != null;
        }

        private void removeSortingOfColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HistoryChanged(current_historylist, true);
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

            if (mapColorDialog.ShowDialog(FindForm()) == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;

                foreach (DataGridViewRow r in selectedRows)
                {
                    HistoryEntry sp = (HistoryEntry)r.Cells[TravelHistoryColumns.HistoryTag].Tag;
                    System.Diagnostics.Debug.Assert(sp != null);
                    sp.UpdateMapColour(mapColorDialog.Color.ToArgb());
                }

                this.Cursor = Cursors.Default;
                HistoryChanged(current_historylist);
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

            DialogResult red = movefrm.ShowDialog(FindForm());
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

                if (!sp.System.Name.Equals(lastname))
                {
                    lastname = sp.System.Name;
                    systemnamelist.Add(lastname);
                }
            }

            if (dist)
            {
                discoveryform.NewTriLatStars(systemnamelist, false);
            }

            if (wanted)
            {
                discoveryform.NewTriLatStars(systemnamelist, true);
            }

            if (route)
            {
                discoveryform.NewExpeditionStars(systemnamelist);
            }

            this.Cursor = Cursors.Default;
        }

        private void viewOnEDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            EliteDangerousCore.EDSM.EDSMClass edsm = new EDSMClass();
            long? id_edsm = rightclicksystem.System?.EDSMID;

            if (id_edsm <= 0)
            {
                id_edsm = null;
            }

            if (!edsm.ShowSystemInEDSM(rightclicksystem.System.Name, id_edsm))
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "System could not be found - has not been synched or EDSM is unavailable");

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
                    if (jent.EdsmID != rightclicksystem.System.EDSMID || jent.EventTypeID == JournalTypeEnum.Died)
                        break;
                    firstrow = i;
                    if (jent.EventTypeID == JournalTypeEnum.FSDJump)
                        break;
                }
            }

            for (int i = rightclickrow + 1; i < dataGridViewTravel.RowCount; i++)
            {
                var jent = jents[i];
                if (jent.EdsmID != rightclicksystem.System.EDSMID || jent.EventTypeID == JournalTypeEnum.FSDJump)
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
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "Could not find Location or FSDJump entry associated with selected journal entry");
                return;
            }

            using (Forms.AssignTravelLogSystemForm form = new Forms.AssignTravelLogSystemForm(journalent))
            {
                DialogResult result = form.ShowDialog(FindForm());
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
            if (ExtendedControls.MessageBoxTheme.Show(FindForm(), "Confirm you wish to remove this entry" + Environment.NewLine + "It may reappear if the logs are rescanned", "WARNING", MessageBoxButtons.OKCancel) == DialogResult.OK)
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
                discoveryform.ActionRunOnEntry(rightclicksystem, Actions.ActionEventEDList.UserRightClick(rightclicksystem));
        }

        private void setNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclicksystem != null)
            {
                using (Forms.SetNoteForm noteform = new Forms.SetNoteForm(rightclicksystem, discoveryform))
                {
                    if (noteform.ShowDialog(FindForm()) == DialogResult.OK)
                    {
                        rightclicksystem.SetJournalSystemNoteText(noteform.NoteText, true , EDCommander.Current.SyncToEdsm);

                        discoveryform.NoteChanged(this,rightclicksystem, true);
                    }
                }
            }
        }

        private void writeEventInfoToLogDebugToolStripMenuItem_Click(object sender, EventArgs e)        //DEBUG ONLY
        {
            Conditions.ConditionVariables cv = new Conditions.ConditionVariables();
            cv.AddPropertiesFieldsOfClass(rightclicksystem.journalEntry, "", new Type[] { typeof(System.Drawing.Image), typeof(System.Drawing.Icon), typeof(System.Drawing.Bitmap), typeof(Newtonsoft.Json.Linq.JObject) }, 5);
            discoveryform.LogLine(cv.ToString(separ: Environment.NewLine));
            //if (rightclicksystem.ShipInformation != null)
            //    discoveryform.LogLine(rightclicksystem.ShipInformation.ToString());
        }

        private void writeJournalToLogtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclicksystem != null && rightclicksystem.journalEntry != null)
            {
                Newtonsoft.Json.Linq.JObject jo = rightclicksystem.journalEntry.GetJson();
                string json = jo?.ToString();
                if (json != null)
                {
                    discoveryform.LogLine(json);
                }
            }
        }

        private void copyJournalEntryToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclicksystem != null && rightclicksystem.journalEntry != null)
            {
                Newtonsoft.Json.Linq.JObject jo = rightclicksystem.journalEntry.GetJson();
                string json = jo?.ToString(Newtonsoft.Json.Formatting.None);
                if (json != null)
                {
                    Clipboard.SetText(json);
                }
            }
        }

        private void runActionsAcrossSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string laststring = "";
            string lasttype = "";
            int lasttypecount = 0;

            discoveryform.DEBUGGETAC.AsyncMode = false;     // to force it to do all the action code before returning..

            if (dataGridViewTravel.SelectedRows.Count > 0)
            {
                List<DataGridViewRow> rows = (from DataGridViewRow x in dataGridViewTravel.SelectedRows where x.Visible orderby x.Index select x).ToList();
                foreach (DataGridViewRow rw in rows)
                {
                    HistoryEntry he = rw.Cells[TravelHistoryColumns.HistoryTag].Tag as HistoryEntry;
                    // System.Diagnostics.Debug.WriteLine("Row " + rw.Index + " " + he.EventSummary + " " + he.EventDescription);


                    bool same = he.journalEntry.EventTypeStr.Equals(lasttype);
                    if (!same || lasttypecount < 10)
                    {
                        lasttype = he.journalEntry.EventTypeStr;
                        lasttypecount = (same) ? ++lasttypecount : 0;

                        discoveryform.DEBUGGETAC.SetPeristentGlobal("GlobalSaySaid", "");
                        Conditions.ConditionFunctionHandlers.SetRandom(new Random(rw.Index + 1));
                        discoveryform.ActionRunOnEntry(he, Actions.ActionEventEDList.UserRightClick(he));

                        Newtonsoft.Json.Linq.JObject jo = he.journalEntry.GetJson();
                        string json = jo?.ToString(Newtonsoft.Json.Formatting.None);

                        string s = discoveryform.DEBUGGETAC.Globals["GlobalSaySaid"];

                        if (s.Length > 0 && !s.Equals(laststring))
                        {
                            System.Diagnostics.Debug.WriteLine("Call ts(j='" + json.Replace("'", "\\'") + "',s='" + s.Replace("'", "\\'") + "',r=" + (rw.Index + 1).ToStringInvariant() + ")");
                            laststring = s;
                        }
                    }
                }
            }
        }

        private void createEditBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclicksystem != null)
            {
                BookmarkForm bookmarkForm = new BookmarkForm();
                BookmarkClass existing = GlobalBookMarkList.Instance.FindBookmarkOnSystem(rightclicksystem.System.Name);
                DateTime tme;
                if (existing != null)
                {
                    tme = existing.Time;
                    bookmarkForm.Update(existing);
                }
                else
                {
                    tme = DateTime.Now;
                    bookmarkForm.NewSystemBookmark(rightclicksystem.System, "", tme);
                }
                DialogResult dr = bookmarkForm.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    GlobalBookMarkList.Instance.AddOrUpdateBookmark(existing, true, rightclicksystem.System.Name, rightclicksystem.System.X, rightclicksystem.System.Y, rightclicksystem.System.Z,
                        tme, bookmarkForm.Notes, bookmarkForm.SurfaceLocations);
                }
                if (dr == DialogResult.Abort && existing != null)
                {
                    GlobalBookMarkList.Instance.Delete(existing);
                }

                dataGridViewTravel.Refresh();
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
            HistoryChanged(current_historylist,true);
        }

        private void buttonField_Click(object sender, EventArgs e)
        {
            Conditions.ConditionFilterForm frm = new Conditions.ConditionFilterForm();
            List<string> namelist = new List<string>() { "Note" };
            namelist.AddRange(discoveryform.Globals.NameList);
            frm.InitFilter("History: Filter out fields",
                            System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location),
                            JournalEntry.GetListOfEventsWithOptMethod(false),
                            (s) => { return BaseUtils.FieldNames.GetPropertyFieldNames(JournalEntry.TypeOfJournalEntry(s)); },
                            namelist, fieldfilter);
            if (frm.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                fieldfilter = frm.result;
                SQLiteDBClass.PutSettingString(DbFieldFilter, fieldfilter.GetJSON());
                HistoryChanged(current_historylist);
            }
        }

        #endregion

        #region Excel

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            Forms.ExportForm frm = new Forms.ExportForm();
            frm.Init(new string[] { "View", "FSD Jumps only", "With Notes only" , "With Notes, no repeat"  });

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid();
                grd.SetCSVDelimiter(frm.Comma);

                List<SystemNoteClass> sysnotecache = new List<SystemNoteClass>();
                string[] colh = null;

                grd.GetLineStatus += delegate (int r)
                {
                    if (r < dataGridViewTravel.Rows.Count)
                    {
                        HistoryEntry he = (HistoryEntry)dataGridViewTravel.Rows[r].Cells[TravelHistoryColumns.HistoryTag].Tag;
                        return (dataGridViewTravel.Rows[r].Visible &&
                                he.EventTimeLocal.CompareTo(frm.StartTime) >= 0 &&
                                he.EventTimeLocal.CompareTo(frm.EndTime) <= 0) ? BaseUtils.CSVWriteGrid.LineStatus.OK : BaseUtils.CSVWriteGrid.LineStatus.Skip;
                    }
                    else
                        return BaseUtils.CSVWriteGrid.LineStatus.EOF;
                };

                if (frm.SelectedIndex == 1)     // export fsd jumps
                {
                    colh = new string[] { "Time", "Name", "X", "Y", "Z", "Distance", "Fuel Used", "Fuel Left", "Boost", "Note" };

                    grd.VerifyLine += delegate (int r)      // addition qualifier for FSD jump
                    {
                        HistoryEntry he = (HistoryEntry)dataGridViewTravel.Rows[r].Cells[TravelHistoryColumns.HistoryTag].Tag;
                        return he.EntryType == JournalTypeEnum.FSDJump;
                    };

                    grd.GetLine += delegate (int r)
                    {
                        HistoryEntry he = (HistoryEntry)dataGridViewTravel.Rows[r].Cells[TravelHistoryColumns.HistoryTag].Tag;
                        EliteDangerousCore.JournalEvents.JournalFSDJump fsd = he.journalEntry as EliteDangerousCore.JournalEvents.JournalFSDJump;

                        return new Object[] {
                            fsd.EventTimeLocal,
                            fsd.StarSystem,
                            fsd.StarPos.X,
                            fsd.StarPos.Y,
                            fsd.StarPos.Z,
                            fsd.JumpDist,
                            fsd.FuelUsed,
                            fsd.FuelLevel,
                            fsd.BoostUsed,
                            he.snc != null ? he.snc.Note : "",
                        };
                        
                    };
                }
                else
                {
                    colh = new string[] { "Time", "Event", "System", "Body",            //0
                                          "Ship", "Summary", "Description", "Detailed Info",        //4
                                          "Note", "Travel Dist", "Travel Time", "Travel Jumps",     //8
                                          "Travelled MisJumps" , "X", "Y","Z" ,     //12
                                          "JID", "EDSMID" , "EDDBID" };             //16

                    grd.GetLine += delegate (int r)
                    {
                        HistoryEntry he = (HistoryEntry)dataGridViewTravel.Rows[r].Cells[TravelHistoryColumns.HistoryTag].Tag;
                        return new Object[] {
                            dataGridViewTravel.Rows[r].Cells[0].Value,
                            he.journalEntry.EventTypeStr,
                            (he.System != null) ? he.System.Name : "Unknown",    // paranoia
                            he.WhereAmI,
                            he.ShipInformation != null ? he.ShipInformation.Name : "Unknown",
                            he.EventSummary,
                            he.EventDescription,
                            he.EventDetailedInfo,
                            dataGridViewTravel.Rows[r].Cells[4].Value,
                            he.isTravelling ? he.TravelledDistance.ToString("0.0") : "",
                            he.isTravelling ? he.TravelledSeconds.ToString() : "",
                            he.isTravelling ? he.Travelledjumps.ToStringInvariant() : "",
                            he.isTravelling ? he.TravelledMissingjump.ToStringInvariant() : "",
                            he.System.X,
                            he.System.Y,
                            he.System.Z,
                            he.Journalid,
                            he.System.EDSMID,
                            he.System.EDDBID,
                        };
                    };

                    if (frm.SelectedIndex == 2 || frm.SelectedIndex == 3)     // export notes
                    {
                        grd.VerifyLine += delegate (int r)      // second hook to reject line
                        {
                            HistoryEntry he = (HistoryEntry)dataGridViewTravel.Rows[r].Cells[TravelHistoryColumns.HistoryTag].Tag;
                            if (he.snc != null)
                            {
                                if (sysnotecache.Contains(he.snc))
                                    return false;
                                else
                                {
                                    if (frm.SelectedIndex == 3)
                                        sysnotecache.Add(he.snc);
                                    return true;
                                }
                            }
                            else
                                return false;
                        };
                    }
                }

                grd.GetHeader += delegate (int c)
                {
                    return (c < colh.Length && frm.IncludeHeader) ? colh[c] : null;
                };

                if (grd.WriteCSV(frm.Path))
                {
                    if (frm.AutoOpen)
                        System.Diagnostics.Process.Start(frm.Path);
                }
                else
                    ExtendedControls.MessageBoxTheme.Show(FindForm(), "Failed to write to " + frm.Path, "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        #endregion

    }
}
