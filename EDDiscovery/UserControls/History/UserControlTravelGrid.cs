/*
 * Copyright © 2016 - 2022 EDDiscovery development team
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
 */

using EDDiscovery.Controls;
using EliteDangerousCore;
using EliteDangerousCore.EDDN;
using EliteDangerousCore.EDSM;
using QuickJSON;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlTravelGrid : UserControlCommonBase, IHistoryCursorNewStarList
    {
        #region Public IF

        //ucct interface
        public HistoryEntry GetCurrentHistoryEntry { get { return dataGridViewTravel.CurrentCell != null ? dataGridViewTravel.Rows[dataGridViewTravel.CurrentCell.RowIndex].Tag as HistoryEntry : null; } }

        #endregion

        #region Events

        // implement IHistoryCursor fields
        public event ChangedSelectionHEHandler OnTravelSelectionChanged;   // as above, different format, for certain older controls

        public event OnNewStarsSubPanelsHandler OnNewStarList;

        #endregion

        #region Init

        private const string dbFilter = "EventFilter2";                 // DB names
        private const string dbHistorySave = "EDUIHistory";
        private const string dbFieldFilter = "FieldFilter";
        private const string dbOutlines = "Outlines";
        private const string dbWordWrap = "WordWrap";
        private const string dbVisitedColour = "VisitedColour";
        private const string dbDebugMode = "DebugMode";
        private const string dbBookmarks = "Bookmarks";
        private const string dbUserGroups = "UserGroups";

        private string searchterms = "system:body:station:stationfaction";

        private HistoryList current_historylist;        // the last one set, for internal refresh purposes on sort

        private BaseUtils.ConditionLists fieldfilter = new BaseUtils.ConditionLists();

        private Dictionary<long, DataGridViewRow> rowsbyjournalid = new Dictionary<long, DataGridViewRow>();

        private JournalFilterSelector cfs;

        private Timer searchtimer;

        private Timer todotimer;
        private Queue<Action> todo = new Queue<Action>();
        private Queue<HistoryEntry> queuedadds = new Queue<HistoryEntry>();

        private int fdropdown;     // filter total

        private HashSet<long> quickMarkJIDs = new HashSet<long>();

        public UserControlTravelGrid()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "TravelHistoryControl";

            //System.Diagnostics.Debug.WriteLine("Travel grid is " + this.GetHashCode());

            cfs = new JournalFilterSelector();
            cfs.AddAllNone();
            cfs.AddJournalExtraOptions();
            cfs.AddJournalEntries();
            cfs.AddUserGroups(GetSetting(dbUserGroups, ""));
            cfs.SaveSettings += EventFilterChanged;

            checkBoxCursorToTop.Checked = true;

            dataGridViewTravel.MakeDoubleBuffered();
            dataGridViewTravel.RowTemplate.MinimumHeight = 26;      // enough for the icon

            string filter = GetSetting(dbFieldFilter, "");
            if (filter.Length > 0)
                fieldfilter.FromJSON(filter);        // load filter

            System.Diagnostics.Trace.WriteLine($"TGR={EDDOptions.Instance.EnableTGRightDebugClicks}");

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

            writeEventInfoToLogDebugToolStripMenuItem.Visible =
            runActionsAcrossSelectionToolSpeechStripMenuItem.Visible =
            runSelectionThroughInaraSystemToolStripMenuItem.Visible =
            runEntryThroughProfileSystemToolStripMenuItem.Visible =
            runSelectionThroughIGAUDebugToolStripMenuItem.Visible =
            runSelectionThroughEDDNThruTestToolStripMenuItem.Visible =
            sendJournalEntriesToDLLsToolStripMenuItem.Visible =
            travelGridInDebugModeToolStripMenuItem.Visible = 
            runSelectionThroughEDAstroDebugToolStripMenuItem.Visible = EDDOptions.Instance.EnableTGRightDebugClicks;
            travelGridInDebugModeToolStripMenuItem.Checked = EDDOptions.Instance.EnableTGRightDebugClicks ? GetSetting(dbDebugMode, false) : false;
            travelGridInDebugModeToolStripMenuItem.CheckedChanged += new System.EventHandler(this.travelGridInDebugModeToolStripMenuItem_CheckedChanged);

            var enumlist = new Enum[] { EDTx.UserControlTravelGrid_ColumnTime, EDTx.UserControlTravelGrid_Icon, EDTx.UserControlTravelGrid_ColumnSystem, EDTx.UserControlTravelGrid_ColumnDistance, EDTx.UserControlTravelGrid_ColumnNote, EDTx.UserControlTravelGrid_labelTime, EDTx.UserControlTravelGrid_labelSearch };
            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);

            var enumlistcms = new Enum[] { EDTx.UserControlTravelGrid_quickMarkToolStripMenuItem, EDTx.UserControlTravelGrid_removeSortingOfColumnsToolStripMenuItem, EDTx.UserControlTravelGrid_gotoEntryNumberToolStripMenuItem, EDTx.UserControlTravelGrid_setNoteToolStripMenuItem, EDTx.UserControlTravelGrid_createEditBookmarkToolStripMenuItem, EDTx.UserControlTravelGrid_toolStripMenuItemStartStop, EDTx.UserControlTravelGrid_gotoNextStartStopMarkerToolStripMenuItem, EDTx.UserControlTravelGrid_mapGotoStartoolStripMenuItem, EDTx.UserControlTravelGrid_viewOnEDSMToolStripMenuItem, EDTx.UserControlTravelGrid_starMapColourToolStripMenuItem, EDTx.UserControlTravelGrid_addToTrilaterationToolStripMenuItem, EDTx.UserControlTravelGrid_addToTrilaterationToolStripMenuItem_trilaterationToolStripMenuItem, EDTx.UserControlTravelGrid_addToTrilaterationToolStripMenuItem_wantedSystemsToolStripMenuItem, 
                EDTx.UserControlTravelGrid_addToTrilaterationToolStripMenuItem_bothToolStripMenuItem, EDTx.UserControlTravelGrid_addToTrilaterationToolStripMenuItem_expeditionToolStripMenuItem, EDTx.UserControlTravelGrid_moveToAnotherCommanderToolStripMenuItem, EDTx.UserControlTravelGrid_hideSystemToolStripMenuItem, EDTx.UserControlTravelGrid_removeJournalEntryToolStripMenuItem, EDTx.UserControlTravelGrid_runActionsOnThisEntryToolStripMenuItem, EDTx.UserControlTravelGrid_copyJournalEntryToClipboardToolStripMenuItem, EDTx.UserControlTravelGrid_writeEventInfoToLogDebugToolStripMenuItem, EDTx.UserControlTravelGrid_runActionsAcrossSelectionToolSpeechStripMenuItem, EDTx.UserControlTravelGrid_runSelectionThroughInaraSystemToolStripMenuItem, EDTx.UserControlTravelGrid_runEntryThroughProfileSystemToolStripMenuItem, EDTx.UserControlTravelGrid_runSelectionThroughIGAUDebugToolStripMenuItem, EDTx.UserControlTravelGrid_runSelectionThroughEDDNThruTestToolStripMenuItem, EDTx.UserControlTravelGrid_runSelectionThroughEDAstroDebugToolStripMenuItem, EDTx.UserControlTravelGrid_sendJournalEntriesToDLLsToolStripMenuItem, EDTx.UserControlTravelGrid_showSystemVisitedForeColourToolStripMenuItem, EDTx.UserControlTravelGrid_travelGridInDebugModeToolStripMenuItem };
            BaseUtils.Translator.Instance.TranslateToolstrip(historyContextMenu, enumlistcms, this);

            var enumlistcms2 = new Enum[] { EDTx.UserControlTravelGrid_outliningOnOffToolStripMenuItem, EDTx.UserControlTravelGrid_scanEventsOutliningOnOffToolStripMenuItem, EDTx.UserControlTravelGrid_toolStripRollUpOlderOutlines, EDTx.UserControlTravelGrid_toolStripRollUpOlderOutlines_rollUpOffToolStripMenuItem, EDTx.UserControlTravelGrid_toolStripRollUpOlderOutlines_rollUpAfterFirstToolStripMenuItem, EDTx.UserControlTravelGrid_toolStripRollUpOlderOutlines_rollUpAfter5ToolStripMenuItem };
            BaseUtils.Translator.Instance.TranslateToolstrip(contextMenuStripOutlines, enumlistcms2, this);

            var enumlisttt = new Enum[] { EDTx.UserControlTravelGrid_comboBoxTime_ToolTip, EDTx.UserControlTravelGrid_textBoxSearch_ToolTip, EDTx.UserControlTravelGrid_buttonFilter_ToolTip, EDTx.UserControlTravelGrid_buttonField_ToolTip, EDTx.UserControlTravelGrid_buttonExtExcel_ToolTip, EDTx.UserControlTravelGrid_checkBoxCursorToTop_ToolTip, EDTx.UserControlTravelGrid_extCheckBoxWordWrap_ToolTip, EDTx.UserControlTravelGrid_extCheckBoxOutlines_ToolTip };
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);

            TravelHistoryFilter.InitaliseComboBox(comboBoxTime, GetSetting(dbHistorySave, ""));

            extButtonDrawnHelp.Text = "";
            extButtonDrawnHelp.Image = ExtendedControls.TabStrip.HelpIcon;

            if (TranslatorExtensions.TxDefined(EDTx.UserControlTravelGrid_SearchTerms))     // if translator has it defined, use it
                searchterms = searchterms.TxID(EDTx.UserControlTravelGrid_SearchTerms);
        }

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridViewTravel);
        }

        public override void Closing()
        {
            todo.Clear();
            todotimer.Stop();
            searchtimer.Stop();
            DGVSaveColumnLayout(dataGridViewTravel);
            PutSetting(dbUserGroups, cfs.GetUserGroupDefinition(1));
            discoveryform.OnHistoryChange -= HistoryChanged;
            discoveryform.OnNewEntry -= AddNewEntry;
            searchtimer.Dispose();
        }

#endregion

        public override void InitialDisplay()
        {
            HistoryChanged(discoveryform.history);
        }

        public void HistoryChanged(HistoryList hl)           // on History change
        {
            HistoryChanged(hl, false);

            // quick marks are commander dependent
            var str = GetSetting(dbBookmarks + ":" + hl.CommanderId, "").Split(';');
            quickMarkJIDs = str.Select(x => x.InvariantParseLong(-1)).ToList().ToHashSet();
        }

        public void HistoryChanged(HistoryList hl, bool disablesorting)
        {
            todo.Clear();           // clear queue of things to do
            queuedadds.Clear();     // and any adds.
            todotimer.Stop();       // ensure timer is off

            if (hl == null)     // just for safety
                return;
                                        
            current_historylist = hl;
            this.dataGridViewTravel.Cursor = Cursors.WaitCursor;

            extComboBoxQuickMarks.Enabled = extCheckBoxOutlines.Enabled = extCheckBoxWordWrap.Enabled = buttonExtExcel.Enabled = buttonFilter.Enabled = buttonField.Enabled = false;

            Tuple<long, int> pos = CurrentGridPosByJID();

            SortOrder sortorder = dataGridViewTravel.SortOrder;
            int sortcol = dataGridViewTravel.SortedColumn?.Index ?? -1;
            if (sortcol >= 0 && disablesorting)
            {
                dataGridViewTravel.Columns[sortcol].HeaderCell.SortGlyphDirection = SortOrder.None;
                sortcol = -1;
            }

            var filter = (TravelHistoryFilter)comboBoxTime.SelectedItem ?? TravelHistoryFilter.NoFilter;

            List<HistoryEntry> result = filter.Filter(hl.EntryOrder());
            fdropdown = hl.Count - result.Count();

            //for (int i = 0; i < 10 && i < result.Count; i++)  System.Diagnostics.Debug.WriteLine("Hist {0} {1} {2}", result[i].EventTimeUTC, result[i].Indexno , result[i].EventSummary);

            panelOutlining.Clear();
            dataGridViewTravel.Rows.Clear();
            rowsbyjournalid.Clear();

            dataGridViewTravel.Columns[0].HeaderText = EDDConfig.Instance.GetTimeTitle();

            List<HistoryEntry[]> chunks = new List<HistoryEntry[]>();

            int chunksize = 500;
            for (int i = 0; i < result.Count; i += chunksize, chunksize = 2000)
            {
                HistoryEntry[] chunk = new HistoryEntry[i + chunksize > result.Count ? result.Count - i : chunksize];

                result.CopyTo(i, chunk, 0, chunk.Length);
                chunks.Add(chunk);
            }

            var sst = new BaseUtils.StringSearchTerms(textBoxSearch.Text, searchterms);

            List<DataGridViewRow> rows = new List<DataGridViewRow>();

            Outlining outlining = null;     // only outline if in normal time decend more with no filter text
            bool rollupscans = false;
            int rollupolder = 0;
            if (!sst.Enabled && (sortcol < 0 || (sortcol == 0 && sortorder == SortOrder.Descending)) && outliningOnOffToolStripMenuItem.Checked)
            {
                outlining = new Outlining();
                rollupscans = scanEventsOutliningOnOffToolStripMenuItem.Checked;
                rollupolder = rollUpOffToolStripMenuItem.Checked ? 0 : rollUpAfterFirstToolStripMenuItem.Checked ? 1 : 5;
            }

            extCheckBoxOutlines.Checked = outlining != null;

            System.Diagnostics.Stopwatch swtotal = new System.Diagnostics.Stopwatch();

            int lrowno = 0;

            HistoryEventFilter hef = new HistoryEventFilter(GetSetting(dbFilter, "All"), fieldfilter, discoveryform.Globals);

            if (chunks.Count != 0)
            {
                var chunk = chunks[0];

                swtotal.Start();
                bool debugmode = travelGridInDebugModeToolStripMenuItem.Checked;

                List<DataGridViewRow> rowstoadd = new List<DataGridViewRow>();
                foreach (var item in chunk)
                {
                    var row = CreateHistoryRow(item, sst, hef, debugmode);

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
                        var row = CreateHistoryRow(item, sst, hef, debugmode);
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
                    if (outlining != null && dataGridViewTravel.Rows.Count > 0)     // if outlining, and #3317 we have rows!
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

                UpdateQuickMarkComboBox();

                this.dataGridViewTravel.Cursor = Cursors.Arrow;
                extComboBoxQuickMarks.Enabled = extCheckBoxOutlines.Enabled = extCheckBoxWordWrap.Enabled = buttonExtExcel.Enabled = buttonFilter.Enabled = buttonField.Enabled = true;

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
            var sst = new BaseUtils.StringSearchTerms(textBoxSearch.Text, searchterms);

            HistoryEventFilter hef = new HistoryEventFilter(GetSetting(dbFilter, "All"), fieldfilter, discoveryform.Globals);

            var row = CreateHistoryRow(he, sst, hef, travelGridInDebugModeToolStripMenuItem.Checked);     // may be dumped out by search

            if (row != null)
            { 
                dataGridViewTravel.Rows.Insert(0, row);

                var filter = (TravelHistoryFilter)comboBoxTime.SelectedItem ?? TravelHistoryFilter.NoFilter;

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

        private DataGridViewRow CreateHistoryRow(HistoryEntry he, BaseUtils.StringSearchTerms search, HistoryEventFilter hef, bool debugmode)
        {
            if (!hef.IsIncluded(he))
                return null;

            //string debugt = item.Journalid + "  " + item.System.id_edsm + " " + item.System.GetHashCode() + " "; // add on for debug purposes to a field below

            string colTime = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(he.EventTimeUTC).ToString();
            //colTime += $" {he.journalEntry.Id}";

            string colIcon = "";
            string colDescription = he.EventSummary;
            he.FillInformation(out string colInformation, out string eventDetailedInfo);

            string colNote = he.GetNoteText;

            if (debugmode)
            {
                colTime += Environment.NewLine + $"{he.TravelState} @ {he.System.Name}\r\n"
                               + $"b{he.Status.BodyName},{he.Status.BodyType},{he.Status.BodyID},ba {he.Status.BodyApproached}\r\n"
                               + $"s{he.Status.StationName},{he.Status.StationType}\r\n"
                               + $"mc{he.MaterialCommodity}/w{he.Weapons}/s{he.Suits}/l{he.Loadouts}/e{he.Engineering}\r\n"
                               + $"b{he.journalEntry.IsBeta}/h{ he.journalEntry.IsHorizons}/o{ he.journalEntry.IsOdyssey}\r\n"
                               + $"bkt{he.Status.BookedTaxi} d {he.Status.BookedDropship}\r\n"
                               + $"jcb{he.Status.CurrentBoost} fsds{he.FSDJumpSequence}\r\n"
                               + $"jid{he.journalEntry.Id} tlu{he.journalEntry.TLUId}"
                               ;

                colDescription = he.journalEntry.EventTypeStr.SplitCapsWord() == he.EventSummary ? he.EventSummary : (he.journalEntry.EventTypeStr + Environment.NewLine + he.EventSummary);

                var js = he.journalEntry.GetJsonCloned();
                js.Remove("event", "timestamp");
                string j = js.ToString().Replace(",\"", ", \"");
                colNote = j.Left(1000);
            }

            if (search.Enabled)
            {
                bool matched = false;

                if (search.Terms[0] != null)
                {
                    int rown = EDDConfig.Instance.OrderRowsInverted ? he.EntryNumber : (discoveryform.history.Count - he.EntryNumber + 1);
                    string entryrow = rown.ToStringInvariant();
                    matched = entryrow.IndexOf(search.Terms[0], StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                                colTime.IndexOf(search.Terms[0], StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                                colIcon.IndexOf(search.Terms[0], StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                                colDescription.IndexOf(search.Terms[0], StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                                colInformation.IndexOf(search.Terms[0], StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                                colNote.IndexOf(search.Terms[0], StringComparison.InvariantCultureIgnoreCase) >= 0;
                }

                if (!matched && search.Terms[1] != null)       // system
                    matched = he.System.Name.WildCardMatch(search.Terms[1],true);
                if (!matched && search.Terms[2] != null)       // body
                    matched = he.Status.BodyName?.WildCardMatch(search.Terms[2],true) ?? false;
                if (!matched && search.Terms[3] != null)       // station
                    matched = he.Status.StationName?.WildCardMatch(search.Terms[3], true) ?? false;
                if (!matched && search.Terms[4] != null)       // stationfaction
                    matched = he.Status.StationFaction?.WildCardMatch(search.Terms[4], true) ?? false;

                if (!matched)
                    return null;
            }

            var rw = dataGridViewTravel.RowTemplate.Clone() as DataGridViewRow;
            rw.CreateCells(dataGridViewTravel, colTime, colIcon, colDescription, colInformation, colNote);

            rw.Tag = he;  //tag on row

            if ( showSystemVisitedForeColourToolStripMenuItem.Checked )
                rw.DefaultCellStyle.ForeColor = (he.System.HasCoordinate) ? ExtendedControls.Theme.Current.KnownSystemColor : ExtendedControls.Theme.Current.UnknownSystemColor;
            else if ( he.EntryType == JournalTypeEnum.FSDJump || he.EntryType == JournalTypeEnum.CarrierJump)
                rw.Cells[2].Style.ForeColor = (he.System.HasCoordinate) ? Color.Empty : ExtendedControls.Theme.Current.UnknownSystemColor;

            string tip = he.EventSummary + Environment.NewLine + colInformation + Environment.NewLine + eventDetailedInfo;
            if ( tip.Length>2000)
                tip = tip.Substring(0, 2000);

            rw.Cells[3].ToolTipText = tip;

            rowsbyjournalid[he.Journalid] = rw;
            return rw;

        }

        private void UpdateToolTipsForFilter()
        {
            string ms = string.Format(" showing {0} original {1}".T(EDTx.UserControlTravelGrid_TT1), dataGridViewTravel.Rows.Count, current_historylist?.Count ?? 0);
            comboBoxTime.SetTipDynamically(toolTip, fdropdown > 0 ? string.Format("Filtered {0}".T(EDTx.UserControlTravelGrid_TTFilt1), fdropdown + ms) : "Select the entries by age, ".T(EDTx.UserControlTravelGrid_TTSelAge) + ms);
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

        public int GotoPosByJID(long jid)       // -1 if fails
        {
            int rowno = DataGridViewControlHelpersStaticFunc.FindGridPosByID(rowsbyjournalid,jid, true);
            if (rowno >= 0)
            {
                dataGridViewTravel.SetCurrentAndSelectAllCellsOnRow(rowno);
                dataGridViewTravel.Rows[rowno].Selected = true;
                FireChangeSelection();
            }
            return rowno;
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
            PutSetting(dbHistorySave, comboBoxTime.Text);
            HistoryChanged(current_historylist);       
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
        private void dataGridViewTravel_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var c = dataGridViewTravel.CurrentCell;
            if (c != null && c.ColumnIndex == ColumnNote.Index)
            {
                var he = dataGridViewTravel.Rows[c.RowIndex].Tag as HistoryEntry;
                var str = dataGridViewTravel[ColumnNote.Index, c.RowIndex].Value as string;
                he.journalEntry.UpdateSystemNote(str, he.System.Name, EDCommander.Current.SyncToEdsm);
                discoveryform.NoteChanged(this, he);
            }
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

        private void OnNoteChanged(Object sender,HistoryEntry he)
        {
            if (todotimer.Enabled)
            {
                todo.Enqueue(() => OnNoteChanged(sender, he));
                return;
            }

            if ( rowsbyjournalid.TryGetValue(he.Journalid,out DataGridViewRow row))
            {
                System.Diagnostics.Debug.WriteLine($"TravelGrid update note due to external {row.Index} {he.GetNoteText} {he.EventSummary}");
                string s = he.GetNoteText;  
                row.Cells[ColumnNote.Index].Value = s;
            }
        }


        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            searchtimer.Stop();
            searchtimer.Start();
            //System.Diagnostics.Debug.WriteLine(Environment.TickCount % 10000 + "Char");
        }

        private void Searchtimer_Tick(object sender, EventArgs e)
        {
            searchtimer.Stop();
            HistoryChanged(current_historylist);
        }

        private void dataGridViewTravel_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            HistoryEntry he = (HistoryEntry)dataGridViewTravel.Rows[e.RowIndex].Tag;
            bool debugmode = travelGridInDebugModeToolStripMenuItem.Checked;
            int rowno = debugmode ? (int)he.Journalid : (EDDConfig.Instance.OrderRowsInverted ? he.EntryNumber : (discoveryform.history.Count - he.EntryNumber + 1));
            PaintHelpers.PaintEventColumn(dataGridViewTravel, e, rowno, he, Icon.Index, true);
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
            if (dataGridViewTravel.LeftClickRowValid)                                                
            {
                if (e.ColumnIndex == ColumnNote.Index)
                {
                    if (!dataGridViewTravel.IsCurrentCellInEditMode)
                    {
                        using (Forms.SetNoteForm noteform = new Forms.SetNoteForm(leftclickhe, discoveryform))
                        {
                            if (noteform.ShowDialog(FindForm()) == DialogResult.OK)
                            {
                                leftclickhe.journalEntry.UpdateSystemNote(noteform.NoteText, leftclickhe.System.Name, EDCommander.Current.SyncToEdsm);
                                discoveryform.NoteChanged(this, leftclickhe);
                            }
                        }
                    }
                }
                else
                {
                    leftclickhe.FillInformation(out string EventDescription, out string EventDetailedInfo);
                    DataGridViewRow row = dataGridViewTravel.Rows[dataGridViewTravel.LeftClickRow];

                    bool expanded = row.Cells[ColumnDistance.Index].Tag != null;

                    if (expanded) // put it back to original text
                    {
                        row.Cells[ColumnDistance.Index].Value = EventDescription;
                        for (int i = 0; i < row.Cells.Count; i++)
                            row.Cells[i].Style.WrapMode = DataGridViewTriState.NotSet;
                        row.Cells[ColumnDistance.Index].Tag = null;
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
                                    string s = i == ColumnDistance.Index ? infodetailed : (string)row.Cells[i].Value;
                                    int h = (int)(g.MeasureString(s, dataGridViewTravel.Font, dataGridViewTravel.Columns[i].Width - 4).Height + 2);
                                    maxh = Math.Max(maxh, h);
                                }
                            }

                            if (maxh > dataGridViewTravel.Height * 3 / 4) // unreasonable amount of space to show it.
                            {
                                ExtendedControls.InfoForm info = new ExtendedControls.InfoForm();
                                info.Info(EDDConfig.Instance.ConvertTimeToSelectedFromUTC(leftclickhe.EventTimeUTC) + ": " + leftclickhe.EventSummary,
                                    FindForm().Icon, infodetailed);
                                info.Size = new Size(1200, 800);
                                info.Show(FindForm());
                            }
                            else
                            {
                                row.Cells[ColumnDistance.Index].Value = infodetailed;

                                if (!extCheckBoxWordWrap.Checked)
                                {
                                    for (int i = 0; i < row.Cells.Count; i++)
                                        row.Cells[i].Style.WrapMode = DataGridViewTriState.True;
                                }
                            }

                            row.Cells[ColumnDistance.Index].Tag = true;      // mark expanded
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

            quickMarkToolStripMenuItem.Checked = quickMarkJIDs.Contains(rightclickhe.journalEntry.Id);              // set the check 
            mapGotoStartoolStripMenuItem.Enabled = (rightclickhe != null && rightclickhe.System.HasCoordinate);
            viewOnEDSMToolStripMenuItem.Enabled = (rightclickhe != null);
            toolStripMenuItemStartStop.Enabled = (rightclickhe != null);
            removeJournalEntryToolStripMenuItem.Enabled = (rightclickhe != null);
            runActionsOnThisEntryToolStripMenuItem.Enabled = (rightclickhe != null);
            setNoteToolStripMenuItem.Enabled = (rightclickhe != null);
            writeEventInfoToLogDebugToolStripMenuItem.Enabled = (rightclickhe != null);
            copyJournalEntryToClipboardToolStripMenuItem.Enabled = (rightclickhe != null);
            createEditBookmarkToolStripMenuItem.Enabled = (rightclickhe != null);
            gotoEntryNumberToolStripMenuItem.Enabled = dataGridViewTravel.Rows.Count > 0;
            removeSortingOfColumnsToolStripMenuItem.Enabled = dataGridViewTravel.SortedColumn != null;
            gotoNextStartStopMarkerToolStripMenuItem.Enabled = (rightclickhe != null);

            var invokelist = OnNewStarList?.GetInvocationList();
            bothToolStripMenuItem.Enabled = wantedSystemsToolStripMenuItem.Enabled = trilaterationToolStripMenuItem.Enabled = invokelist != null && Array.Find(invokelist, x => x.Method.DeclaringType == typeof(UserControlTrilateration)) != null;
            expeditionToolStripMenuItem.Enabled = invokelist != null && Array.Find(invokelist, x => x.Method.DeclaringType == typeof(UserControlExpedition)) != null;
        }

        private void removeSortingOfColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HistoryChanged(current_historylist, true);
        }

        private void mapGotoStartoolStripMenuItem_Click(object sender, EventArgs e)
        {
            discoveryform.Open3DMap(rightclickhe?.System);
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
                foreach (DataGridViewRow r in selectedRows)
                {
                    HistoryEntry sp = (HistoryEntry)r.Tag;
                    System.Diagnostics.Debug.Assert(sp != null);
                    sp.journalEntry.UpdateMapColour(mapColorDialog.Color.ToArgb());
                }

                HistoryChanged(current_historylist);
            }
        }

        private void hideSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEnumerable<DataGridViewRow> selectedRows = dataGridViewTravel.SelectedCells.Cast<DataGridViewCell>()
                .Select(cell => cell.OwningRow)
                .Distinct();

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
        }

        private void moveToAnotherCommanderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEnumerable<DataGridViewRow> selectedRows = dataGridViewTravel.SelectedCells.Cast<DataGridViewCell>()
                .Select(cell => cell.OwningRow)
                .Distinct();

            List<HistoryEntry> listsyspos = new List<HistoryEntry>();

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

        private void expeditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSystemToOthers(expedition:true);
        }

        private void AddSystemToOthers(bool dist = false, bool wanted = false, bool expedition = false, bool exploration = false)
        {
            IEnumerable<DataGridViewRow> selectedRows = dataGridViewTravel.SelectedCells.Cast<DataGridViewCell>()
                                                                        .Select(cell => cell.OwningRow)
                                                                        .Distinct()
                                                                        .OrderBy(cell => cell.Index);

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

        }

        public void FireNewStarList(List<string> system, OnNewStarsPushType pushtype)
        {
            OnNewStarList?.Invoke(system, pushtype);
        }

        private void viewOnEDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.EDSM.EDSMClass edsm = new EDSMClass();
            if (!edsm.ShowSystemInEDSM(rightclickhe.System.Name))
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "System could not be found - has not been synched or EDSM is unavailable".T(EDTx.UserControlTravelGrid_NotSynced));
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
                    rightclickhe.journalEntry.UpdateSystemNote(noteform.NoteText, rightclickhe.System.Name, EDCommander.Current.SyncToEdsm);
                    discoveryform.NoteChanged(this, rightclickhe);
                }
            }
        }

        private void writeEventInfoToLogDebugToolStripMenuItem_Click(object sender, EventArgs e)        
        {
            // copies the work of action run for he's
            BaseUtils.Variables eventvars = new BaseUtils.Variables();
            var ev = Actions.ActionEventEDList.NewEntry(rightclickhe);
            Actions.ActionVars.TriggerVars(eventvars, ev.TriggerName, ev.TriggerType);
            Actions.ActionVars.HistoryEventVars(eventvars, rightclickhe, "Event");     // if HE is null, ignored
            Actions.ActionVars.ShipBasicInformation(eventvars, rightclickhe?.ShipInformation, "Event");     // if He null, or si null, ignore
            Actions.ActionVars.SystemVars(eventvars, rightclickhe?.System, "Event");
            discoveryform.LogLine(eventvars.ToString(separ: Environment.NewLine));
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
            BookmarkHelpers.ShowBookmarkForm(discoveryform, discoveryform, rightclickhe.System, null);
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

        // quickmarks.
        private void quickMarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (quickMarkToolStripMenuItem.Checked)
                quickMarkJIDs.Add(rightclickhe.journalEntry.Id);
            else
                quickMarkJIDs.Remove(rightclickhe.journalEntry.Id);

            var str = quickMarkJIDs.Select(x => x.ToStringInvariant()).ToArray().Join(';');
            PutSetting(dbBookmarks + ":" + discoveryform.history.CommanderId.ToStringInvariant(), str);
            UpdateQuickMarkComboBox();
        }

        private void UpdateQuickMarkComboBox()
        {
            extComboBoxQuickMarks.Items.Clear();
            List<long> jids = new List<long>();
            foreach (var j in quickMarkJIDs)
            {
                if (rowsbyjournalid.TryGetValue(j, out DataGridViewRow row)) // if it parses and its in view, add it to combo box.
                {
                    extComboBoxQuickMarks.Items.Add((string)row.Cells[0].Value + ":" + (string)row.Cells[2].Value);
                    jids.Add(j);
                }
            }
            extComboBoxQuickMarks.Tag = jids;
            extComboBoxQuickMarks.Text = "Marked".TxID(EDTx.UserControlTravelGrid_quickMarkToolStripMenuItem);      // only works for custom
        }
        private void extComboBoxBookmark_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<long> jids = extComboBoxQuickMarks.Tag as List<long>;
            long jid = jids[extComboBoxQuickMarks.SelectedIndex];
            GotoPosByJID(jid);
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
            BaseUtils.ConditionLists res = HistoryFilterHelpers.ShowDialog(FindForm(), fieldfilter, discoveryform, "History: Filter out fields".T(EDTx.UserControlTravelGrid_THF));
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
            dataGridViewTravel.SetWordWrap(extCheckBoxWordWrap.Checked);
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
                var mcmr = discoveryform.history.MaterialCommoditiesMicroResources.GetDict(rightclickhe.MaterialCommodity);
                List<QuickJSON.JToken> list = EliteDangerousCore.Inara.InaraSync.NewEntryList(rightclickhe, mcmr);

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
                //System.IO.File.WriteAllText(@"c:\code\inaraentry.json", str);

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
            EDDHelp.Help(this.FindForm(), extButtonDrawnHelp.PointToScreen(new Point(0,extButtonDrawnHelp.Bottom)),this.HelpKeyOrAddress());
        }

        private void runSelectionThroughEDAstroDebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclickhe != null)
            {
                EliteDangerousCore.EDAstro.EDAstroSync.SendEDAstroEvents(new List<HistoryEntry>() { rightclickhe });
            }
        }

        private void sendJournalEntriesToDLLsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclickhe != null)
            {
                discoveryform.DLLManager.NewJournalEntry(EliteDangerousCore.DLL.EDDDLLCallerHE.CreateFromHistoryEntry(discoveryform.history, rightclickhe), true);
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

    }
}


