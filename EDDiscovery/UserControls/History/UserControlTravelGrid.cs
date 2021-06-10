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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using EDDiscovery.Controls;
using EliteDangerousCore.DB;
using EliteDangerousCore;
using EliteDangerousCore.EDSM;
using EliteDangerousCore.EDDN;
using EDDiscovery.Forms;
using BaseUtils.JSON;

namespace EDDiscovery.UserControls
{
    public partial class UserControlTravelGrid : UserControlCommonBase, IHistoryCursorNewStarList
    {
        #region Public IF

        //ucct interface
        public HistoryEntry GetCurrentHistoryEntry { get { return dataGridViewTravel.CurrentCell != null ? dataGridViewTravel.Rows[dataGridViewTravel.CurrentCell.RowIndex].Tag as HistoryEntry : null; } }

        //horrible.. needs removing, used by export.
        public TravelHistoryFilter GetHistoryFilter { get { return (TravelHistoryFilter)comboBoxHistoryWindow.SelectedItem ?? TravelHistoryFilter.NoFilter; } }

        #endregion

        #region Events

        // implement IHistoryCursor fields
        public event ChangedSelectionHEHandler OnTravelSelectionChanged;   // as above, different format, for certain older controls

        public event OnNewStarsSubPanelsHandler OnNewStarList;

        // for primary travel grid for auto note jump
        public delegate void KeyDownInCell(int asciikeycode, int rowno, int colno, bool note);
        public event KeyDownInCell OnKeyDownInCell;   // After a change of selection

        #endregion

        #region Init

        private class Columns
        {
            public const int Time = 0;
            public const int Icon = 1;
            public const int Description = 2;
            public const int Information = 3;
            public const int Note = 4;
        }

        private int defaultRowHeight;

        private const string dbFilter = "EventFilter2";                 // DB names
        private const string dbHistorySave = "EDUIHistory";
        private const string dbFieldFilter = "FieldFilter";
        private const string dbOutlines = "Outlines";
        private const string dbWordWrap = "WordWrap";
        private const string dbVisitedColour = "VisitedColour";
        private const string dbDebugMode = "DebugMode";

        private HistoryList current_historylist;        // the last one set, for internal refresh purposes on sort

        private BaseUtils.ConditionLists fieldfilter = new BaseUtils.ConditionLists();

        private Dictionary<long, DataGridViewRow> rowsbyjournalid = new Dictionary<long, DataGridViewRow>();

        private FilterSelector cfs;

        private Timer searchtimer;

        private Timer todotimer;
        private Queue<Action> todo = new Queue<Action>();
        private Queue<HistoryEntry> queuedadds = new Queue<HistoryEntry>();

        public UserControlTravelGrid()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "TravelHistoryControl";

            //System.Diagnostics.Debug.WriteLine("Travel grid is " + this.GetHashCode());

            cfs = new FilterSelector();
            cfs.AddAllNone();
            cfs.AddJournalExtraOptions();
            cfs.AddJournalEntries();
            cfs.SaveSettings += EventFilterChanged;

            checkBoxCursorToTop.Checked = true;

            dataGridViewTravel.MakeDoubleBuffered();

            string filter = GetSetting(dbFieldFilter, "");
            if (filter.Length > 0)
                fieldfilter.FromJSON(filter);        // load filter

            writeEventInfoToLogDebugToolStripMenuItem.Visible =
            runActionsAcrossSelectionToolSpeechStripMenuItem.Visible =
            runSelectionThroughInaraSystemToolStripMenuItem.Visible =
            runEntryThroughProfileSystemToolStripMenuItem.Visible =
            runSelectionThroughIGAUDebugToolStripMenuItem.Visible =
            runSelectionThroughEDDNDebugNoSendToolStripMenuItem.Visible =
            runSelectionThroughEDAstroDebugToolStripMenuItem.Visible = EDDOptions.Instance.EnableTGRightDebugClicks;

            searchtimer = new Timer() { Interval = 500 };
            searchtimer.Tick += Searchtimer_Tick;

            todotimer = new Timer { Interval = 20 };
            todotimer.Tick += Todotimer_Tick;

            discoveryform.OnHistoryChange += HistoryChanged;
            discoveryform.OnNewEntry += AddNewEntry;
            discoveryform.OnNoteChanged += OnNoteChanged;

            this.showSystemVisitedForeColourToolStripMenuItem.Checked = GetSetting(dbVisitedColour, false);
            this.showSystemVisitedForeColourToolStripMenuItem.Click += new System.EventHandler(this.showSystemVisitedForeColourToolStripMenuItem_Click);

            contextMenuStripOutlines.SetToolStripState(GetSetting(dbOutlines, "rollUpOffToolStripMenuItem;"));
            this.rollUpOffToolStripMenuItem.Click += new System.EventHandler(this.rolluplimitToolStripMenuItem_Click);
            this.rollUpAfterFirstToolStripMenuItem.Click += new System.EventHandler(this.rolluplimitToolStripMenuItem_Click);
            this.rollUpAfter5ToolStripMenuItem.Click += new System.EventHandler(this.rolluplimitToolStripMenuItem_Click);
            this.outliningOnOffToolStripMenuItem.Click += new System.EventHandler(this.toolStripOutliningToggle);
            this.scanEventsOutliningOnOffToolStripMenuItem.Click += new System.EventHandler(this.toolStripOutliningToggle);
            extCheckBoxOutlines.Checked = outliningOnOffToolStripMenuItem.Checked;

            extCheckBoxWordWrap.Checked = GetSetting(dbWordWrap, false);
            UpdateWordWrap();
            extCheckBoxWordWrap.Click += extCheckBoxWordWrap_Click;

            travelGridInDebugModeToolStripMenuItem.Checked = GetSetting(dbDebugMode, false);
            travelGridInDebugModeToolStripMenuItem.CheckedChanged += new System.EventHandler(this.travelGridInDebugModeToolStripMenuItem_CheckedChanged);

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(historyContextMenu, this);
            BaseUtils.Translator.Instance.Translate(contextMenuStripOutlines, this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);

            TravelHistoryFilter.InitaliseComboBox(comboBoxHistoryWindow, GetSetting(dbHistorySave, ""));

            extButtonDrawnHelp.Text = "";
            extButtonDrawnHelp.Image = ExtendedControls.TabStrip.HelpIcon;
        }

        public override void LoadLayout()
        {
            defaultRowHeight = dataGridViewTravel.RowTemplate.MinimumHeight = Math.Max(28, Font.ScalePixels(28));       // due to 24 bit icons
            DGVLoadColumnLayout(dataGridViewTravel);
        }

        public override void Closing()
        {
            todo.Clear();
            todotimer.Stop();
            searchtimer.Stop();
            DGVSaveColumnLayout(dataGridViewTravel);
            discoveryform.OnHistoryChange -= HistoryChanged;
            discoveryform.OnNewEntry -= AddNewEntry;
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

        public void HistoryChanged(HistoryList hl, bool disablesorting)
        {
            if (hl == null)     // just for safety
                return;
                                        
            current_historylist = hl;
            this.Cursor = Cursors.WaitCursor;

            extCheckBoxOutlines.Enabled = extCheckBoxWordWrap.Enabled = buttonExtExcel.Enabled = buttonFilter.Enabled = buttonField.Enabled = comboBoxHistoryWindow.Enabled = false;

            Tuple<long, int> pos = CurrentGridPosByJID();

            SortOrder sortorder = dataGridViewTravel.SortOrder;
            int sortcol = dataGridViewTravel.SortedColumn?.Index ?? -1;
            if (sortcol >= 0 && disablesorting)
            {
                dataGridViewTravel.Columns[sortcol].HeaderCell.SortGlyphDirection = SortOrder.None;
                sortcol = -1;
            }

            var filter = (TravelHistoryFilter)comboBoxHistoryWindow.SelectedItem ?? TravelHistoryFilter.NoFilter;

            List<HistoryEntry> result = filter.Filter(hl.EntryOrder());
            fdropdown = hl.Count - result.Count();

            //for (int i = 0; i < 10 && i < result.Count; i++)  System.Diagnostics.Debug.WriteLine("Hist {0} {1} {2}", result[i].EventTimeUTC, result[i].Indexno , result[i].EventSummary);

            result = HistoryList.FilterByJournalEvent(result, GetSetting(dbFilter, "All"), out ftotalevents);

            result = FilterHelpers.FilterHistory(result, fieldfilter, discoveryform.Globals, out ftotalfilters);

            panelOutlining.Clear();
            dataGridViewTravel.Rows.Clear();
            rowsbyjournalid.Clear();

            dataGridViewTravel.Columns[0].HeaderText = EDDiscoveryForm.EDDConfig.GetTimeTitle();

            List<HistoryEntry[]> chunks = new List<HistoryEntry[]>();

            int chunksize = 500;
            for (int i = 0; i < result.Count; i += chunksize, chunksize = 2000)
            {
                HistoryEntry[] chunk = new HistoryEntry[i + chunksize > result.Count ? result.Count - i : chunksize];

                result.CopyTo(i, chunk, 0, chunk.Length);
                chunks.Add(chunk);
            }

            todo.Clear();           // clear queue of things to do
            queuedadds.Clear();     // and any adds.
            todotimer.Stop();

            string filtertext = textBoxFilter.Text;
            List<DataGridViewRow> rows = new List<DataGridViewRow>();

            Outlining outlining = null;     // only outline if in normal time decend more with no filter text
            bool rollupscans = false;
            int rollupolder = 0;
            if (filtertext.IsEmpty() && (sortcol < 0 || (sortcol == 0 && sortorder == SortOrder.Descending)) && outliningOnOffToolStripMenuItem.Checked)
            {
                outlining = new Outlining();
                rollupscans = scanEventsOutliningOnOffToolStripMenuItem.Checked;
                rollupolder = rollUpOffToolStripMenuItem.Checked ? 0 : rollUpAfterFirstToolStripMenuItem.Checked ? 1 : 5;
            }

            extCheckBoxOutlines.Checked = outlining != null;

            System.Diagnostics.Stopwatch swtotal = new System.Diagnostics.Stopwatch();

            int lrowno = 0;

            if (chunks.Count != 0)
            {
                var chunk = chunks[0];

                swtotal.Start();
                bool debugmode = travelGridInDebugModeToolStripMenuItem.Checked;

                List<DataGridViewRow> rowstoadd = new List<DataGridViewRow>();
                foreach (var item in chunk)
                {
                    var row = CreateHistoryRow(item, filtertext,debugmode);

                    if (row != null)
                    {
                        //row.Cells[2].Value = (lrowno++).ToString() + " " + item.Journalid + " " + (string)row.Cells[2].Value;
                        row.Visible = outlining?.Process(item, lrowno, rollupscans, rollupolder) ?? true;
                        rowstoadd.Add(row);
                        lrowno++;
                    }
                }

                dataGridViewTravel.Rows.AddRange(rowstoadd.ToArray());

                if (dataGridViewTravel.MoveToSelection(rowsbyjournalid, ref pos, false))
                    FireChangeSelection();
            }

            foreach (var chunk in chunks.Skip(1))
            {
                todo.Enqueue(() =>
                {
                    List<DataGridViewRow> rowstoadd = new List<DataGridViewRow>();

                    //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch(); sw.Start();

                    bool debugmode = travelGridInDebugModeToolStripMenuItem.Checked;
                    foreach (var item in chunk)
                    {
                        var row = CreateHistoryRow(item, filtertext, debugmode);
                        if (row != null)
                        {
                            //row.Cells[2].Value = (lrowno++).ToString() + " " + item.Journalid + " " + (string)row.Cells[2].Value;
                            row.Visible = outlining?.Process(item, lrowno, rollupscans, rollupolder) ?? true;
                            rowstoadd.Add(row);
                            lrowno++;
                        }
                    }

                    dataGridViewTravel.Rows.AddRange(rowstoadd.ToArray());

                    if (dataGridViewTravel.MoveToSelection(rowsbyjournalid, ref pos, false))
                        FireChangeSelection();

                    //System.Diagnostics.Debug.WriteLine("T Chunk Load in " + sw.ElapsedMilliseconds);
                });
            }

            todo.Enqueue(() =>
            {
                if (chunks.Count != 0)
                {
                    if (outlining != null)
                    {
                        dataGridViewTravel.Rows[dataGridViewTravel.Rows.Count - 1].Visible = true;
                        outlining.Finished(dataGridViewTravel.Rows.Count - 1, rollupolder, panelOutlining);     // ensures we have a group at the end..
                    }
                }

                UpdateToolTipsForFilter();

                if (dataGridViewTravel.MoveToSelection(rowsbyjournalid, ref pos, true))
                {
                    FireChangeSelection();
                }

                if (sortcol >= 0)
                {
                    dataGridViewTravel.Sort(dataGridViewTravel.Columns[sortcol], (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                    dataGridViewTravel.Columns[sortcol].HeaderCell.SortGlyphDirection = sortorder;
                }

                this.Cursor = Cursors.Default;
                extCheckBoxOutlines.Enabled = extCheckBoxWordWrap.Enabled = buttonExtExcel.Enabled = buttonFilter.Enabled = buttonField.Enabled = comboBoxHistoryWindow.Enabled = true;

                System.Diagnostics.Trace.WriteLine(BaseUtils.AppTicks.TickCount + " TG TOTAL TIME " + swtotal.ElapsedMilliseconds);

                while( queuedadds.Count>0)              // finally, dequeue any adds added
                {
                    System.Diagnostics.Debug.WriteLine("TG Dequeue paused adds");
                    AddEntry(queuedadds.Dequeue());
                }
            });

            todotimer.Start();          // indicate the timer is going to start.. 
        }


        private void Todotimer_Tick(object sender, EventArgs e)
        {
            if (todo.Count != 0)
            {
                var act = todo.Dequeue();
                act();
            }
            else
            {
                todotimer.Stop();
            }
        }

        private void AddNewEntry(HistoryEntry he, HistoryList hl)           // on new entry from discovery system
        {
            if (todotimer.Enabled)      // if we have the todotimer running.. we add to the queue.  better than the old loadcomplete, no race conditions
            {
                queuedadds.Enqueue(he);
            }
            else
            {
                AddEntry(he);
            }
        }

        private void AddEntry(HistoryEntry he)      // from either a delayed queue or from addnewentry
        { 
            bool add = he.IsJournalEventInEventFilter(GetSetting(dbFilter, "All"));

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
            {
                var row = CreateHistoryRow(he, textBoxFilter.Text, travelGridInDebugModeToolStripMenuItem.Checked);     // may be dumped out by search
                if (row != null)
                    dataGridViewTravel.Rows.Insert(0, row);
                else
                    add = false;
            }

            if (add)            // its been added, we have at least 1 row visible, at row 0
            {
                var filter = (TravelHistoryFilter)comboBoxHistoryWindow.SelectedItem ?? TravelHistoryFilter.NoFilter;

                if (filter.MaximumNumberOfItems != null)            // this one won't remove the latest one
                {
                    for (int r = dataGridViewTravel.Rows.Count - 1; r >= filter.MaximumNumberOfItems; r--)
                    {
                        System.Diagnostics.Debug.WriteLine("TG Removed as too much " + r);
                        dataGridViewTravel.Rows.RemoveAt(r);
                    }
                }

                if (checkBoxCursorToTop.Checked)   // Move focus to first row
                {
                    System.Diagnostics.Trace.WriteLine("TG Auto selected top row on new entry");
                    dataGridViewTravel.ClearSelection();
                    dataGridViewTravel.SetCurrentAndSelectAllCellsOnRow(0);       // its the current cell which needs to be set, moves the row marker as well
                    FireChangeSelection();
                }
            }
        }

        private DataGridViewRow CreateHistoryRow(HistoryEntry item, string search, bool debugmode)
        {
            //string debugt = item.Journalid + "  " + item.System.id_edsm + " " + item.System.GetHashCode() + " "; // add on for debug purposes to a field below

            DateTime time = EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(item.EventTimeUTC);
            item.FillInformation(out string EventDescription, out string EventDetailedInfo);
            string note = (item.SNC != null) ? item.SNC.Note : "";

            if (search.HasChars())
            {
                string timestr = time.ToString();
                int rown = EDDConfig.Instance.OrderRowsInverted ? item.EntryNumber : (discoveryform.history.Count - item.EntryNumber + 1);
                string entryrow = rown.ToStringInvariant();
                bool matched = timestr.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                                item.EventSummary.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                                EventDescription.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                                note.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                                entryrow.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) >= 0;
                if (!matched)
                    return null;
            }

            var rw = dataGridViewTravel.RowTemplate.Clone() as DataGridViewRow;

            if (debugmode)
            {
                var js = item.journalEntry.GetJsonCloned();
                js.Remove("event", "timestamp");
                string j = js.ToString().Replace(",\"", ", \"");
                j = j.Left(1000);

                string state = string.Format("{0}\r\n[{1},{2}]\r\n[{3},{4}]\r\nmc{5}/w{6}/s{7}/l{8}\r\nb{9}/h{10}/o{11}", item.TravelState, item.Status.BodyName, item.Status.BodyType, item.Status.StationName, 
                                    item.Status.StationType,item.MaterialCommodity, item.Weapons,item.Suits, item.Loadouts , 
                                    item.journalEntry.IsBeta, item.journalEntry.IsHorizons,item.journalEntry.IsOdyssey);

                string eventname = item.journalEntry.EventTypeStr.SplitCapsWord() == item.EventSummary ? item.EventSummary : (item.journalEntry.EventTypeStr + Environment.NewLine + item.EventSummary);

                rw.CreateCells(dataGridViewTravel, time, state, eventname, EventDescription, j);
            }
            else
            {
                rw.CreateCells(dataGridViewTravel, time, "", item.EventSummary, EventDescription, note);
            }

            rw.Tag = item;  //tag on row

            if ( showSystemVisitedForeColourToolStripMenuItem.Checked )
                rw.DefaultCellStyle.ForeColor = (item.System.HasCoordinate) ? discoveryform.theme.KnownSystemColor : discoveryform.theme.UnknownSystemColor;
            else if ( item.EntryType == JournalTypeEnum.FSDJump || item.EntryType == JournalTypeEnum.CarrierJump)
                rw.Cells[2].Style.ForeColor = (item.System.HasCoordinate) ? Color.Empty : discoveryform.theme.UnknownSystemColor;

            string tip = item.EventSummary + Environment.NewLine + EventDescription + Environment.NewLine + EventDetailedInfo;
            if ( tip.Length>2000)
                tip = tip.Substring(0, 2000);

            rw.Cells[0].ToolTipText = tip;
            rw.Cells[1].ToolTipText = tip;
            rw.Cells[2].ToolTipText = tip;
            rw.Cells[3].ToolTipText = tip;
            rw.Cells[4].ToolTipText = tip;

            rowsbyjournalid[item.Journalid] = rw;
            return rw;

        }

        private void UpdateToolTipsForFilter()
        {
            string ms = string.Format(" showing {0} original {1}".T(EDTx.UserControlTravelGrid_TT1), dataGridViewTravel.Rows.Count, current_historylist?.Count ?? 0);
            comboBoxHistoryWindow.SetTipDynamically(toolTip, fdropdown > 0 ? string.Format("Filtered {0}".T(EDTx.UserControlTravelGrid_TTFilt1), fdropdown + ms) : "Select the entries by age, ".T(EDTx.UserControlTravelGrid_TTSelAge) + ms);
            toolTip.SetToolTip(buttonFilter, (ftotalevents > 0) ? string.Format("Filtered {0}".T(EDTx.UserControlTravelGrid_TTFilt2), ftotalevents + ms) : "Filter out entries based on event type, ".T(EDTx.UserControlTravelGrid_TTEvent) + ms);
            toolTip.SetToolTip(buttonField, (ftotalfilters > 0) ? string.Format("Total filtered out {0}".T(EDTx.UserControlTravelGrid_TTFilt3), ftotalfilters + ms) : "Filter out entries matching the field selection, ".T(EDTx.UserControlTravelGrid_TTTotal) + ms);
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
            long jid = (dataGridViewTravel.CurrentCell != null) ? ((HistoryEntry)(dataGridViewTravel.Rows[dataGridViewTravel.CurrentCell.RowIndex].Tag)).Journalid : -1;
            int cellno = (dataGridViewTravel.CurrentCell != null) ? dataGridViewTravel.CurrentCell.ColumnIndex : 0;
            return new Tuple<long, int>(jid, cellno);
        }

        public void GotoPosByJID(long jid)
        {
            int rowno = DataGridViewControlHelpersStaticFunc.FindGridPosByID(rowsbyjournalid,jid, true);
            if (rowno >= 0)
            {
                dataGridViewTravel.SetCurrentAndSelectAllCellsOnRow(rowno);
                dataGridViewTravel.Rows[rowno].Selected = true;
                FireChangeSelection();
            }
        }

        public void FireChangeSelection()
        {
            if (dataGridViewTravel.CurrentCell != null)
            {
                int row = dataGridViewTravel.CurrentCell.RowIndex;
                var he = dataGridViewTravel.Rows[row].Tag as HistoryEntry;
                //System.Diagnostics.Trace.WriteLine("************ TG Fire Change sel at " + row + " he " + he.EventTimeUTC + " " +  he.EventSummary + " " + he.System.Name + " " + dataGridViewTravel.CurrentCell.RowIndex + ":" + dataGridViewTravel.CurrentCell.ColumnIndex);

                if ( OnTravelSelectionChanged != null )     // we do this manually, so we can time each reaction if required.
                {
                    foreach (var e in OnTravelSelectionChanged.GetInvocationList())
                    {
                        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch(); sw.Start();
                        e.DynamicInvoke(he, current_historylist, true);
                        if ( sw.ElapsedMilliseconds>=50)
                            System.Diagnostics.Trace.WriteLine("TG FCS Method " + e.Method.DeclaringType + " took " + sw.ElapsedMilliseconds);
                    }
                }
            }
            else if (current_historylist != null && current_historylist.Count > 0)
            {
                OnTravelSelectionChanged?.Invoke(current_historylist.GetLast, current_historylist, false);
            }
        }

        private void comboBoxHistoryWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            PutSetting(dbHistorySave, comboBoxHistoryWindow.Text);

            if (current_historylist != null)
            {
                HistoryChanged(current_historylist);        // fires lots of events
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
                OnKeyDownInCell(e.KeyChar, dataGridViewTravel.CurrentCell.RowIndex, dataGridViewTravel.CurrentCell.ColumnIndex, dataGridViewTravel.CurrentCell.ColumnIndex == Columns.Note);
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
            if (todotimer.Enabled)
            {
                todo.Enqueue(() => OnNoteChanged(sender, he, committed));
                return;
            }

            if (rowsbyjournalid.ContainsKey(he.Journalid) ) // if we can find the grid entry
            {
                string s = (he.SNC != null) ? he.SNC.Note : "";     // snc may have gone null, so cope with it
                //System.Diagnostics.Debug.WriteLine("TG:Note changed " + s);
                rowsbyjournalid[he.Journalid].Cells[Columns.Note].Value = s;
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
            if (todotimer.Enabled)      // don't do it while we are loading
            {
                return;
            }

            searchtimer.Stop();
            this.Cursor = Cursors.WaitCursor;
            HistoryChanged(current_historylist);
            this.Cursor = Cursors.Default;
        }

        private void dataGridViewTravel_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView grid = sender as DataGridView;

            PaintHelpers.PaintEventColumn(grid, e,
                        discoveryform.history.Count, (HistoryEntry)dataGridViewTravel.Rows[e.RowIndex].Tag,
                        travelGridInDebugModeToolStripMenuItem.Checked ? -1 : Columns.Icon, 
                        true);
        }


#region Right/Left Clicks

        HistoryEntry rightclickhe = null;
        HistoryEntry leftclickhe = null;

        private void dataGridViewTravel_MouseDown(object sender, MouseEventArgs e)
        {
            rightclickhe = dataGridViewTravel.RightClickRowValid ? (HistoryEntry)dataGridViewTravel.Rows[dataGridViewTravel.RightClickRow].Tag : null;
            leftclickhe = dataGridViewTravel.LeftClickRowValid ? (HistoryEntry)dataGridViewTravel.Rows[dataGridViewTravel.LeftClickRow].Tag : null;
        }

        private void dataGridViewTravel_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewTravel.LeftClickRowValid)                                                   // Click expands it..
            {
                if (e.ColumnIndex == ColumnNote.Index)
                {
                    using (Forms.SetNoteForm noteform = new Forms.SetNoteForm(leftclickhe, discoveryform))
                    {
                        if (noteform.ShowDialog(FindForm()) == DialogResult.OK)
                        {
                            leftclickhe.SetJournalSystemNoteText(noteform.NoteText, true, EDCommander.Current.SyncToEdsm);
                            discoveryform.NoteChanged(this, leftclickhe, true);
                        }
                    }
                }
                else
                {
                    leftclickhe.FillInformation(out string EventDescription, out string EventDetailedInfo);
                    DataGridViewRow row = dataGridViewTravel.Rows[dataGridViewTravel.LeftClickRow];

                    bool expanded = row.Cells[Columns.Information].Tag != null;

                    if (expanded) // put it back to original text
                    {
                        row.Cells[Columns.Information].Value = EventDescription;
                        for (int i = 0; i < row.Cells.Count; i++)
                            row.Cells[i].Style.WrapMode = DataGridViewTriState.NotSet;
                        row.Cells[Columns.Information].Tag = null;
                    }
                    else
                    {
                        string infodetailed = EventDescription.AppendPrePad(EventDetailedInfo, Environment.NewLine);        // make up detailed line
                        if (leftclickhe.journalEntry is EliteDangerousCore.JournalEvents.JournalLocOrJump)
                        {
                            string travelinfo = leftclickhe.TravelInfo();
                            if (travelinfo != null)
                                infodetailed = travelinfo + Environment.NewLine + infodetailed;
                        }

                        using (Graphics g = Parent.CreateGraphics())
                        {
                            int maxh = 0;
                            for (int i = 0; i < row.Cells.Count; i++)
                            {
                                if (row.Cells[i].Value is string)
                                {
                                    string s = i == Columns.Information ? infodetailed : (string)row.Cells[i].Value;
                                    int h = (int)(g.MeasureString(s, dataGridViewTravel.Font, dataGridViewTravel.Columns[i].Width - 4).Height + 2);
                                    maxh = Math.Max(maxh, h);
                                }
                            }

                            if (maxh > dataGridViewTravel.Height * 3 / 4) // unreasonable amount of space to show it.
                            {
                                ExtendedControls.InfoForm info = new ExtendedControls.InfoForm();
                                info.Info(EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(leftclickhe.EventTimeUTC) + ": " + leftclickhe.EventSummary,
                                    FindForm().Icon, infodetailed);
                                info.Size = new Size(1200, 800);
                                info.Show(FindForm());
                            }
                            else
                            {
                                row.Cells[Columns.Information].Value = infodetailed;

                                if (!extCheckBoxWordWrap.Checked)
                                {
                                    for (int i = 0; i < row.Cells.Count; i++)
                                        row.Cells[i].Style.WrapMode = DataGridViewTriState.True;
                                }
                            }

                            row.Cells[Columns.Information].Tag = true;      // mark expanded
                        }
                    }

                    dataViewScrollerPanel.UpdateScroll();
                }
            }
        }

        private void dataGridViewTravel_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            panelOutlining.Clear();
            extCheckBoxOutlines.Checked = false;
        }

        #endregion

        #region Outlining

        private void extButtonOutlines_Click(object sender, EventArgs e)
        {
            if (extCheckBoxOutlines.Checked == true && outliningOnOffToolStripMenuItem.Checked)     // if going checked.. means it was unchecked
            {
                HistoryChanged(current_historylist, true);      // Reapply, disabled by sorting etc
            }
            else
            {
                var p = extCheckBoxOutlines.PointToScreen(new Point(0, extCheckBoxOutlines.Height));
                contextMenuStripOutlines.Font = this.Font;
                contextMenuStripOutlines.Show(p);
            }
        }

        private void toolStripOutliningToggle(object sender, EventArgs e)
        {
            PutSetting(dbOutlines, contextMenuStripOutlines.GetToolStripState());
            extCheckBoxOutlines.Checked = outliningOnOffToolStripMenuItem.Checked;
            if (outliningOnOffToolStripMenuItem.Checked || sender == outliningOnOffToolStripMenuItem)
                HistoryChanged(current_historylist, true);
        }

        private void rolluplimitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tmi = sender as ToolStripMenuItem;
            rollUpOffToolStripMenuItem.Checked = tmi == rollUpOffToolStripMenuItem;         // makes them work as radio buttons
            rollUpAfterFirstToolStripMenuItem.Checked = tmi == rollUpAfterFirstToolStripMenuItem;
            rollUpAfter5ToolStripMenuItem.Checked = tmi == rollUpAfter5ToolStripMenuItem;
            PutSetting(dbOutlines, contextMenuStripOutlines.GetToolStripState());
            if (outliningOnOffToolStripMenuItem.Checked )
                HistoryChanged(current_historylist, true);
        }

        #endregion

        #region TravelHistoryRightClick

        private void historyContextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (dataGridViewTravel.SelectedCells.Count == 0)      // need something selected  stops context menu opening on nothing..
                e.Cancel = true;

            if (rightclickhe != null)
            {
                if (rightclickhe.StartMarker)
                {
                    toolStripMenuItemStartStop.Text = "Clear Start marker".T(EDTx.UserControlTravelGrid_CSTART);
                }
                else if (rightclickhe.StopMarker)
                {
                    toolStripMenuItemStartStop.Text = "Clear Stop marker".T(EDTx.UserControlTravelGrid_CSTOP);
                }
                else if (rightclickhe.isTravelling)
                {
                    toolStripMenuItemStartStop.Text = "Set Stop marker for travel calculations".T(EDTx.UserControlTravelGrid_SETSTOPTC);
                }
                else
                {
                    toolStripMenuItemStartStop.Text = "Set Start marker for travel calculations".T(EDTx.UserControlTravelGrid_SETSTARTTC);
                }
            }
            else
            {
                toolStripMenuItemStartStop.Text = "Set Start/Stop point for travel calculations".T(EDTx.UserControlTravelGrid_SETSTSTOP); ;
            }

            mapGotoStartoolStripMenuItem.Enabled = (rightclickhe != null && rightclickhe.System.HasCoordinate);
            viewOnEDSMToolStripMenuItem.Enabled = (rightclickhe != null);
            toolStripMenuItemStartStop.Enabled = (rightclickhe != null);
            removeJournalEntryToolStripMenuItem.Enabled = (rightclickhe != null);
            sendUnsyncedScanToEDDNToolStripMenuItem.Enabled = (rightclickhe != null && EDDNClass.IsDelayableEDDNMessage(rightclickhe.EntryType, rightclickhe.EventTimeUTC) && !rightclickhe.EDDNSync);
            runActionsOnThisEntryToolStripMenuItem.Enabled = (rightclickhe != null);
            setNoteToolStripMenuItem.Enabled = (rightclickhe != null);
            writeEventInfoToLogDebugToolStripMenuItem.Enabled = (rightclickhe != null);
            copyJournalEntryToClipboardToolStripMenuItem.Enabled = (rightclickhe != null);
            createEditBookmarkToolStripMenuItem.Enabled = (rightclickhe != null);
            gotoEntryNumberToolStripMenuItem.Enabled = dataGridViewTravel.Rows.Count > 0;
            removeSortingOfColumnsToolStripMenuItem.Enabled = dataGridViewTravel.SortedColumn != null;
            gotoNextStartStopMarkerToolStripMenuItem.Enabled = (rightclickhe != null);
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
                if (discoveryform.Map.MoveToSystem(rightclickhe.System))
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
            HistoryEntry sp2 = (HistoryEntry)selectedRows.First().Tag;
            mapColorDialog.Color = Color.Red;

            if (mapColorDialog.ShowDialog(FindForm()) == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;

                foreach (DataGridViewRow r in selectedRows)
                {
                    HistoryEntry sp = (HistoryEntry)r.Tag;
                    System.Diagnostics.Debug.Assert(sp != null);
                    sp.journalEntry.UpdateMapColour(mapColorDialog.Color.ToArgb());
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
                HistoryEntry sp = (HistoryEntry)r.Tag;
                System.Diagnostics.Debug.Assert(sp != null);
                sp.journalEntry.UpdateCommanderID(-1);
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
                HistoryEntry sp = (HistoryEntry)r.Tag;
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
                    sp.journalEntry.UpdateCommanderID(movefrm.selectedCommander.Id);
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
            AddSystemToOthers(dist:true);
        }

        private void wantedSystemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSystemToOthers(wanted:true);
        }

        private void bothToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSystemToOthers(dist:true, wanted:true);
        }

        private void routeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSystemToOthers(expedition:true);
        }

        private void explorationPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSystemToOthers(exploration:true);
        }

        private void AddSystemToOthers(bool dist = false, bool wanted = false, bool expedition = false, bool exploration = false)
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
                HistoryEntry sp = (HistoryEntry)r.Tag;

                if (!sp.System.Name.Equals(lastname))
                {
                    lastname = sp.System.Name;
                    systemnamelist.Add(lastname);
                }
            }

            if (dist)
                FireNewStarList(systemnamelist, OnNewStarsPushType.TriSystems);

            if (wanted)
                FireNewStarList(systemnamelist, OnNewStarsPushType.TriWanted);

            if (expedition)
                FireNewStarList(systemnamelist, OnNewStarsPushType.Expedition);

            if (exploration)
                FireNewStarList(systemnamelist, OnNewStarsPushType.Exploration);

            this.Cursor = Cursors.Default;
        }

        public void FireNewStarList(List<string> system, OnNewStarsPushType pushtype)
        {
            OnNewStarList?.Invoke(system, pushtype);
        }

        private void viewOnEDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            EliteDangerousCore.EDSM.EDSMClass edsm = new EDSMClass();
            if (!edsm.ShowSystemInEDSM(rightclickhe.System.Name))
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "System could not be found - has not been synched or EDSM is unavailable".T(EDTx.UserControlTravelGrid_NotSynced));

            this.Cursor = Cursors.Default;
        }

        private void toolStripMenuItemStartStop_Click(object sender, EventArgs e)
        {
            discoveryform.history.SetStartStop(rightclickhe);
            discoveryform.RefreshHistoryAsync();
        }

        private void removeJournalEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string warning = ("Confirm you wish to remove this entry" + Environment.NewLine + "It may reappear if the logs are rescanned").T(EDTx.UserControlTravelGrid_Remove);
            if (ExtendedControls.MessageBoxTheme.Show(FindForm(), warning, "Warning".T(EDTx.Warning), MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                JournalEntry.Delete(rightclickhe.Journalid);
                discoveryform.RefreshHistoryAsync();
            }
        }

        private void sendUnsyncedScanToEDDNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EDDNClass.IsDelayableEDDNMessage(rightclickhe.EntryType, rightclickhe.EventTimeUTC) && !rightclickhe.EDDNSync)
            {
                EDDNSync.SendEDDNEvent(discoveryform.LogLine, rightclickhe);
            }
        }

        private void runActionsOnThisEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            discoveryform.ActionRunOnEntry(rightclickhe, Actions.ActionEventEDList.UserRightClick(rightclickhe));
        }

        private void setNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Forms.SetNoteForm noteform = new Forms.SetNoteForm(rightclickhe, discoveryform))
            {
                if (noteform.ShowDialog(FindForm()) == DialogResult.OK)
                {
                    rightclickhe.SetJournalSystemNoteText(noteform.NoteText, true, EDCommander.Current.SyncToEdsm);

                    discoveryform.NoteChanged(this, rightclickhe, true);
                }
            }
        }

        private void writeEventInfoToLogDebugToolStripMenuItem_Click(object sender, EventArgs e)        //DEBUG ONLY
        {
            BaseUtils.Variables cv = new BaseUtils.Variables();
            cv.AddPropertiesFieldsOfClass(rightclickhe.journalEntry, "EventClass_", new Type[] { typeof(System.Drawing.Image), typeof(System.Drawing.Icon), typeof(System.Drawing.Bitmap), typeof(BaseUtils.JSON.JObject) }, 5);
            discoveryform.LogLine(cv.ToString(separ: Environment.NewLine));
        }

        private void copyJournalEntryToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string json = rightclickhe.journalEntry.GetJsonString();
            if (json != null)
            {
                SetClipboardText(json);
                discoveryform.LogLine(json);
            }
        }

        private void createEditBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BookmarkForm bookmarkForm = new BookmarkForm(discoveryform.history);
            BookmarkClass existing = GlobalBookMarkList.Instance.FindBookmarkOnSystem(rightclickhe.System.Name);
            DateTime timeutc;
            if (existing != null)
            {
                timeutc = existing.TimeUTC;
                bookmarkForm.Bookmark(existing);
            }
            else
            {
                timeutc = DateTime.UtcNow;
                bookmarkForm.NewSystemBookmark(rightclickhe.System, "", timeutc);
            }
            DialogResult dr = bookmarkForm.ShowDialog();
            if (dr == DialogResult.OK)
            {
                GlobalBookMarkList.Instance.AddOrUpdateBookmark(existing, true, rightclickhe.System.Name, rightclickhe.System.X, rightclickhe.System.Y, rightclickhe.System.Z,
                    timeutc, bookmarkForm.Notes, bookmarkForm.SurfaceLocations);
            }
            if (dr == DialogResult.Abort && existing != null)
            {
                GlobalBookMarkList.Instance.Delete(existing);
            }

            dataGridViewTravel.Refresh();
        }

        private void gotoEntryNumberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int curi = rightclickhe != null ? (EDDConfig.Instance.OrderRowsInverted ? rightclickhe.EntryNumber : (discoveryform.history.Count - rightclickhe.EntryNumber + 1)) : 0;
            int selrow = dataGridViewTravel.JumpToDialog(this.FindForm(), curi, r =>
            {
                HistoryEntry he = r.Tag as HistoryEntry;
                return EDDConfig.Instance.OrderRowsInverted ? he.EntryNumber : (discoveryform.history.Count - he.EntryNumber + 1);
            });

            if (selrow >= 0)
            {
                dataGridViewTravel.ClearSelection();
                dataGridViewTravel.SetCurrentAndSelectAllCellsOnRow(selrow);
                FireChangeSelection();
            }
        }

        private void gotoNextStartStopMarkerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for( int rown = dataGridViewTravel.RightClickRow + 1; rown < dataGridViewTravel.Rows.Count; rown++ )
            {
                DataGridViewRow r = dataGridViewTravel.Rows[rown];
                if (r.Visible)
                {
                    HistoryEntry h = r.Tag as HistoryEntry;
                    if (h.StartMarker || h.StopMarker)
                    {
                        dataGridViewTravel.DisplayRow(r.Index, true);
                        dataGridViewTravel.ClearSelection();
                        dataGridViewTravel.Rows[r.Index].Selected = true;
                        break;
                    }
                }
            }
        }

        #endregion

        #region Event Filter

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            cfs.Open(GetSetting(dbFilter, "All"), b, this.FindForm());
        }

        private void EventFilterChanged(string newset, Object e)
        {
            string filters = GetSetting(dbFilter, "All");
            if (filters != newset)
            {
                PutSetting(dbFilter, newset);
                HistoryChanged(current_historylist, true);
            }
        }


        private void buttonField_Click(object sender, EventArgs e)
        {
            BaseUtils.ConditionLists res = FilterHelpers.ShowDialog(FindForm(), fieldfilter, discoveryform, "History: Filter out fields".T(EDTx.UserControlTravelGrid_THF));
            if (res != null)
            {
                fieldfilter = res;
                PutSetting(dbFieldFilter, fieldfilter.GetJSON());
                HistoryChanged(current_historylist);
            }
        }

        #endregion

        #region Word wrap/visited

        private void extCheckBoxWordWrap_Click(object sender, EventArgs e)
        {
            PutSetting(dbWordWrap, extCheckBoxWordWrap.Checked);
            UpdateWordWrap();
        }

        private void UpdateWordWrap()
        {
            dataGridViewTravel.DefaultCellStyle.WrapMode = extCheckBoxWordWrap.Checked ? DataGridViewTriState.True : DataGridViewTriState.False;
            dataGridViewTravel.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dataViewScrollerPanel.UpdateScroll();
        }

        private void showSystemVisitedForeColourToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting(dbVisitedColour, showSystemVisitedForeColourToolStripMenuItem.Checked);
            HistoryChanged(discoveryform.history);
        }

        #endregion

        #region DEBUG clicks - only for special people who build the debug version!

        private void runSelectionThroughInaraSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclickhe != null)
            {
                List<BaseUtils.JSON.JToken> list = EliteDangerousCore.Inara.InaraSync.NewEntryList(discoveryform.history,rightclickhe);

                foreach (var j in list)
                {
                    j["eventTimestamp"] = DateTime.UtcNow.ToStringZulu();       // mangle time to now to allow it to send.
                    if (j["eventName"].Str() == "addCommanderMission")
                    {
                        j["eventData"]["missionExpiry"] = DateTime.UtcNow.AddDays(1).ToStringZulu();       // mangle mission time to now to allow it to send.
                    }
                    if (j["eventName"].Str() == "setCommunityGoal")
                    {
                        j["eventData"]["goalExpiry"] = DateTime.UtcNow.AddDays(5).ToStringZulu();       // mangle expiry time
                    }
                }

                string json = rightclickhe.journalEntry.GetJsonString();
                discoveryform.LogLine(json);

                EliteDangerousCore.Inara.InaraClass inara = new EliteDangerousCore.Inara.InaraClass(EDCommander.Current);
                string str = inara.ToJSONString(list);
                discoveryform.LogLine(str);
                System.IO.File.WriteAllText(@"c:\code\inaraentry.json", str);

                if (list.Count > 0)
                {
                    string strres = inara.Send(list);
                    discoveryform.LogLine(strres);
                }
                else
                    discoveryform.LogLine("No Events");
            }
        }

        private void runEntryThroughProfileSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            discoveryform.CheckActionProfile(rightclickhe);
        }

        private void runSelectionThroughIGAUDebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclickhe != null)
            {
                EliteDangerousCore.IGAU.IGAUSync.NewEvent(discoveryform.LogLine, rightclickhe);
            }
        }

        private void runSelectionThroughEDDNDebugNoSendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ( rightclickhe != null )
            {
                EDDNSync.SendToEDDN(rightclickhe, true);
            }

        }

        private void extButtonDrawnHelp_Click(object sender, EventArgs e)
        {
            EDDHelp.Help(this.FindForm(), extButtonDrawnHelp.PointToScreen(new Point(0,extButtonDrawnHelp.Bottom)),this);
        }

        private void runSelectionThroughEDAstroDebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclickhe != null)
            {
                EliteDangerousCore.EDAstro.EDAstroSync.SendEDAstroEvents(new List<HistoryEntry>() { rightclickhe });
            }
        }

        private void travelGridInDebugModeToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            PutSetting(dbDebugMode, travelGridInDebugModeToolStripMenuItem.Checked);
            HistoryChanged(discoveryform.history);
        }

        private void runActionsAcrossSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string laststring = "";
            string lasttype = "";
            int lasttypecount = 0;

            discoveryform.ActionController.AsyncMode = false;     // to force it to do all the action code before returning..

            if (dataGridViewTravel.SelectedRows.Count > 0)
            {
                List<DataGridViewRow> rows = (from DataGridViewRow x in dataGridViewTravel.SelectedRows where x.Visible orderby x.Index select x).ToList();
                foreach (DataGridViewRow rw in rows)
                {
                    HistoryEntry he = rw.Tag as HistoryEntry;
                    // System.Diagnostics.Debug.WriteLine("Row " + rw.Index + " " + he.EventSummary + " " + he.EventDescription);


                    bool same = he.journalEntry.EventTypeStr.Equals(lasttype);
                    if (!same || lasttypecount < 10)
                    {
                        lasttype = he.journalEntry.EventTypeStr;
                        lasttypecount = (same) ? ++lasttypecount : 0;

                        discoveryform.ActionController.SetPeristentGlobal("GlobalSaySaid", "");
                        BaseUtils.FunctionHandlers.SetRandom(new Random(rw.Index + 1));
                        discoveryform.ActionRunOnEntry(he, Actions.ActionEventEDList.UserRightClick(he));

                        string json = he.journalEntry.GetJsonString();

                        string s = discoveryform.ActionController.Globals["GlobalSaySaid"];

                        if (s.Length > 0 && !s.Equals(laststring))
                        {
                            System.Diagnostics.Debug.WriteLine("Call ts(j='" + json?.Replace("'", "\\'") + "',s='" + s.Replace("'", "\\'") + "',r=" + (rw.Index + 1).ToStringInvariant() + ")");
                            laststring = s;
                        }
                    }
                }
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
                        HistoryEntry he = (HistoryEntry)dataGridViewTravel.Rows[r].Tag;
                        return (dataGridViewTravel.Rows[r].Visible &&
                                he.EventTimeUTC.CompareTo(frm.StartTimeUTC) >= 0 &&
                                he.EventTimeUTC.CompareTo(frm.EndTimeUTC) <= 0) ? BaseUtils.CSVWriteGrid.LineStatus.OK : BaseUtils.CSVWriteGrid.LineStatus.Skip;
                    }
                    else
                        return BaseUtils.CSVWriteGrid.LineStatus.EOF;
                };

                if (frm.SelectedIndex == 1)     // export fsd jumps
                {
                    colh = new string[] { "Time", "Name", "X", "Y", "Z", "Distance", "Fuel Used", "Fuel Left", "Boost", "Note" };

                    grd.VerifyLine += delegate (int r)      // addition qualifier for FSD jump
                    {
                        HistoryEntry he = (HistoryEntry)dataGridViewTravel.Rows[r].Tag;
                        return he.EntryType == JournalTypeEnum.FSDJump;
                    };

                    grd.GetLine += delegate (int r)
                    {
                        HistoryEntry he = (HistoryEntry)dataGridViewTravel.Rows[r].Tag;
                        EliteDangerousCore.JournalEvents.JournalFSDJump fsd = he.journalEntry as EliteDangerousCore.JournalEvents.JournalFSDJump;

                        return new Object[] {
                            EDDConfig.Instance.ConvertTimeToSelectedFromUTC(fsd.EventTimeUTC),
                            fsd.StarSystem,
                            fsd.StarPos.X,
                            fsd.StarPos.Y,
                            fsd.StarPos.Z,
                            fsd.JumpDist,
                            fsd.FuelUsed,
                            fsd.FuelLevel,
                            fsd.BoostUsed,
                            he.SNC != null ? he.SNC.Note : "",
                        };

                    };
                }
                else
                {
                    colh = new string[] { "Time", "Event", "System", "Body",            //0
                                          "Ship", "Summary", "Description", "Detailed Info",        //4
                                          "Note", "Travel Dist", "Travel Time", "Travel Jumps",     //8
                                          "Travelled MisJumps" , "X", "Y","Z" ,     //12
                                          "JID", "EDSMID"};             //16

                    grd.GetLine += delegate (int r)
                    {
                        HistoryEntry he = (HistoryEntry)dataGridViewTravel.Rows[r].Tag;
                        he.FillInformation(out string EventDescription, out string EventDetailedInfo);
                        return new Object[] {
                            EDDConfig.Instance.ConvertTimeToSelectedFromUTC(he.EventTimeUTC),
                            he.EventSummary,
                            (he.System != null) ? he.System.Name : "Unknown",    // paranoia
                            he.WhereAmI,
                            he.ShipInformation != null ? he.ShipInformation.Name : "Unknown",
                            he.EventSummary,
                            EventDescription,
                            EventDetailedInfo,
                            dataGridViewTravel.Rows[r].Cells[4].Value,
                            he.isTravelling ? he.TravelledDistance.ToString("0.0") : "",
                            he.isTravelling ? he.TravelledSeconds.ToString() : "",
                            he.isTravelling ? he.Travelledjumps.ToString() : "",
                            he.isTravelling ? he.TravelledMissingjump.ToString() : "",
                            he.System.X,
                            he.System.Y,
                            he.System.Z,
                            he.Journalid,
                            he.System.EDSMID,
                        };
                    };

                    if (frm.SelectedIndex == 2 || frm.SelectedIndex == 3)     // export notes
                    {
                        grd.VerifyLine += delegate (int r)      // second hook to reject line
                        {
                            HistoryEntry he = (HistoryEntry)dataGridViewTravel.Rows[r].Tag;
                            if (he.SNC != null)
                            {
                                if (sysnotecache.Contains(he.SNC))
                                    return false;
                                else
                                {
                                    if (frm.SelectedIndex == 3)
                                        sysnotecache.Add(he.SNC);
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

                grd.WriteGrid(frm.Path, frm.AutoOpen, FindForm());
            }

        }

        #endregion
    }
}


