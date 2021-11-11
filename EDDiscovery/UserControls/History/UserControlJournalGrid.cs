/*
 * Copyright © 2016 - 2021 EDDiscovery development team
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
using EDDiscovery.Controls;
using EliteDangerousCore;
using EliteDangerousCore.EDDN;
using EliteDangerousCore.EDSM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlJournalGrid : UserControlCommonBase, IHistoryCursor
    {
        private JournalFilterSelector cfs;
        private BaseUtils.ConditionLists fieldfilter = new BaseUtils.ConditionLists();
        private Dictionary<long, DataGridViewRow> rowsbyjournalid = new Dictionary<long, DataGridViewRow>();

        private string dbFilter = "EventFilter2";
        private string dbHistorySave = "EDUIHistory";
        private string dbFieldFilter = "ControlFieldFilter";

        public delegate void PopOut();
        public PopOut OnPopOut;

        private HistoryList current_historylist;        // the last one set, for internal refresh purposes on sort

        public event ChangedSelectionHEHandler OnTravelSelectionChanged;   // as above, different format, for certain older controls

        public HistoryEntry GetCurrentHistoryEntry { get { return dataGridViewJournal.CurrentCell != null ? dataGridViewJournal.Rows[dataGridViewJournal.CurrentCell.RowIndex].Tag as HistoryEntry : null; } }

        #region Init

        private class Columns
        {
            public const int Time = 0;
            public const int Event = 1;
            public const int Type = 2;
            public const int Text = 3;
        }

        private Timer searchtimer;

        private Timer todotimer;
        private Queue<Action> todo = new Queue<Action>();
        private Queue<HistoryEntry> queuedadds = new Queue<HistoryEntry>();

        public UserControlJournalGrid()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "JournalGrid";

            dataGridViewJournal.MakeDoubleBuffered();
            dataGridViewJournal.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewJournal.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx

            cfs = new JournalFilterSelector();
            cfs.AddAllNone();
            cfs.AddJournalExtraOptions();
            cfs.AddJournalEntries();
            cfs.SaveSettings += EventFilterChanged;

            checkBoxCursorToTop.Checked = true;

            string filter = GetSetting(dbFieldFilter, "");
            if (filter.Length > 0)
                fieldfilter.FromJSON(filter);        // load filter

            searchtimer = new Timer() { Interval = 500 };
            searchtimer.Tick += Searchtimer_Tick;

            todotimer = new Timer() { Interval = 10 };
            todotimer.Tick += Todotimer_Tick;

            discoveryform.OnHistoryChange += HistoryChanged;
            discoveryform.OnNewEntry += AddNewEntry;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(historyContextMenu, this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);

            TravelHistoryFilter.InitaliseComboBox(comboBoxTime, GetSetting(dbHistorySave, ""));
        }

        public override void LoadLayout()
        {
            dataGridViewJournal.RowTemplate.MinimumHeight = Math.Max(28, Font.ScalePixels(28));
            DGVLoadColumnLayout(dataGridViewJournal);
        }

        public override void Closing()
        {
            todo.Clear();
            todotimer.Stop();
            searchtimer.Stop();
            DGVSaveColumnLayout(dataGridViewJournal);
            discoveryform.OnHistoryChange -= HistoryChanged;
            discoveryform.OnNewEntry -= AddNewEntry;
            searchtimer.Dispose();
        }

        #endregion

        #region Display

        public override void InitialDisplay()
        {
            HistoryChanged(discoveryform.history);
        }

        int fdropdown, ftotalevents, ftotalfilters;     // filter totals

        private void HistoryChanged(HistoryList hl)
        {
            Display(hl, false);
        }

        private void Display(HistoryList hl, bool disablesorting )
        {
            todo.Clear();           // ensure in a quiet state
            queuedadds.Clear();
            todotimer.Stop();

            if (hl == null)     // just for safety
                return;

            this.dataGridViewJournal.Cursor = Cursors.WaitCursor;
            buttonExtExcel.Enabled = buttonFilter.Enabled = buttonField.Enabled = false;

            current_historylist = hl;

            Tuple<long, int> pos = CurrentGridPosByJID();

            SortOrder sortorder = dataGridViewJournal.SortOrder;
            int sortcol = dataGridViewJournal.SortedColumn?.Index ?? -1;
            if (sortcol >= 0 && disablesorting)
            {
                dataGridViewJournal.Columns[sortcol].HeaderCell.SortGlyphDirection = SortOrder.None;
                sortcol = -1;
            }

            var filter = (TravelHistoryFilter)comboBoxTime.SelectedItem ?? TravelHistoryFilter.NoFilter;

            List<HistoryEntry> result = filter.Filter(hl.EntryOrder());
            fdropdown = hl.Count - result.Count();

            result = HistoryList.FilterByJournalEvent(result, GetSetting(dbFilter, "All"), out ftotalevents);
            result = HistoryFilterHelpers.FilterHistory(result, fieldfilter, discoveryform.Globals, out ftotalfilters);

            dataGridViewJournal.Rows.Clear();
            rowsbyjournalid.Clear();

            dataGridViewJournal.Columns[0].HeaderText = EDDiscoveryForm.EDDConfig.GetTimeTitle();

            List<HistoryEntry[]> chunks = new List<HistoryEntry[]>();

            int chunksize = 500;
            for (int i = 0; i < result.Count; i += chunksize, chunksize = 2000)
            {
                HistoryEntry[] chunk = new HistoryEntry[i + chunksize > result.Count ? result.Count - i : chunksize];

                result.CopyTo(i, chunk, 0, chunk.Length);
                chunks.Add(chunk);
            }

            string filtertext = textBoxSearch.Text;

            System.Diagnostics.Stopwatch swtotal = new System.Diagnostics.Stopwatch(); swtotal.Start();

            //int lrowno = 0;

            foreach (var chunk in chunks)
            {
                todo.Enqueue(() =>
                {
                    //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch(); sw.Start();

                    List<DataGridViewRow> rowstoadd = new List<DataGridViewRow>();

                    foreach (var item in chunk)
                    {
                        var row = CreateHistoryRow(item, filtertext);
                        if (row != null)
                        {
                            //row.Cells[2].Value = (lrowno++).ToString() + " " + item.Journalid + " " + (string)row.Cells[2].Value;
                            rowstoadd.Add(row);
                        }
                    }

                    dataGridViewJournal.Rows.AddRange(rowstoadd.ToArray()); // much faster to send in one chunk

                    // System.Diagnostics.Debug.WriteLine("J Chunk Load in " + sw.ElapsedMilliseconds);

                    if (dataGridViewJournal.MoveToSelection(rowsbyjournalid, ref pos, false))
                        FireChangeSelection();

                });
            }

            todo.Enqueue(() =>
            {
                System.Diagnostics.Debug.WriteLine(BaseUtils.AppTicks.TickCount + " JG TOTAL TIME " + swtotal.ElapsedMilliseconds);

                UpdateToolTipsForFilter();

                if (dataGridViewJournal.MoveToSelection(rowsbyjournalid, ref pos, true))
                    FireChangeSelection();

                if (sortcol >= 0)
                {
                    dataGridViewJournal.Sort(dataGridViewJournal.Columns[sortcol], (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                    dataGridViewJournal.Columns[sortcol].HeaderCell.SortGlyphDirection = sortorder;
                }

                this.dataGridViewJournal.Cursor = Cursors.Default;
                buttonExtExcel.Enabled = buttonFilter.Enabled = buttonField.Enabled = true;

                while (queuedadds.Count > 0)              // finally, dequeue any adds added
                {
                    System.Diagnostics.Debug.WriteLine("JG Dequeue paused adds");
                    AddEntry(queuedadds.Dequeue());
                }
            });

            todotimer.Start();
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

        private void AddNewEntry(HistoryEntry he, HistoryList hl)               // on new entry from discovery system
        {
            if (todotimer.Enabled)       // if we have the todotimer running.. we add to the queue.  better than the old loadcomplete, no race conditions
            {
                queuedadds.Enqueue(he);
            }
            else
            {
                AddEntry(he);
            }
        }

        private void AddEntry(HistoryEntry he)
        { 
            bool add = he.IsJournalEventInEventFilter(GetSetting(dbFilter, "All"));

            if (!add)
            {
                ftotalevents++;
                UpdateToolTipsForFilter();
            }

            if (add && !HistoryFilterHelpers.FilterHistory(he, fieldfilter, discoveryform.Globals))
            {
                add = false;
                ftotalfilters++;
                UpdateToolTipsForFilter();
            }

            if (add)
            {
                var row = CreateHistoryRow(he, textBoxSearch.Text);     // we might be filtered out by search
                if (row != null)
                    dataGridViewJournal.Rows.Insert(0, row);
                else
                    add = false;
            }

            if (add)                                // its been added, we have at least 1 row visible, at row 0
            { 
                var filter = (TravelHistoryFilter)comboBoxTime.SelectedItem ?? TravelHistoryFilter.NoFilter;

                if (filter.MaximumNumberOfItems != null)        // this one won't remove the latest one
                {
                    for (int r = dataGridViewJournal.Rows.Count - 1; r >= filter.MaximumNumberOfItems; r--)
                    {
                        dataGridViewJournal.Rows.RemoveAt(r);
                    }
                }

                if (checkBoxCursorToTop.Checked) // Move focus to first row
                {
                    dataGridViewJournal.ClearSelection();
                    dataGridViewJournal.SetCurrentAndSelectAllCellsOnRow(0);       // its the current cell which needs to be set, moves the row marker as well
                    FireChangeSelection();
                }
            }
        }

        private DataGridViewRow CreateHistoryRow(HistoryEntry he, string search)
        {
            DateTime time = EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(he.EventTimeUTC);
            he.FillInformation(out string EventDescription, out string EventDetailedInfo);
            string detail = EventDescription;
            detail = detail.AppendPrePad(EventDetailedInfo.LineLimit(15,Environment.NewLine + "..."), Environment.NewLine);

            if (search.HasChars())
            {
                string timestr = time.ToString();
                int rown = EDDConfig.Instance.OrderRowsInverted ? he.EntryNumber : (discoveryform.history.Count - he.EntryNumber + 1);
                string entryrow = rown.ToStringInvariant();
                bool matched = timestr.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                                he.EventSummary.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                                detail.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                                entryrow.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) >= 0;

                if (!matched)
                    return null;
            }

            var rw = dataGridViewJournal.RowTemplate.Clone() as DataGridViewRow;
            rw.CreateCells(dataGridViewJournal, time, "", he.EventSummary, detail);

            rw.Tag = he;

            rowsbyjournalid[he.Journalid] = rw;
            return rw;
        }

        private void UpdateToolTipsForFilter()
        {
            string ms = string.Format(" showing {0} original {1}".T(EDTx.UserControlJournalGrid_TT1), dataGridViewJournal.Rows.Count, current_historylist?.Count ?? 0);
            comboBoxTime.SetTipDynamically(toolTip, fdropdown > 0 ? string.Format("Filtered {0}".T(EDTx.UserControlJournalGrid_TTFilt1), fdropdown + ms) : "Select the entries by age, ".T(EDTx.UserControlJournalGrid_TTSelAge) + ms);
            toolTip.SetToolTip(buttonFilter, (ftotalevents > 0) ? string.Format("Filtered {0}".T(EDTx.UserControlJournalGrid_TTFilt2), ftotalevents + ms) : "Filter out entries based on event type, ".T(EDTx.UserControlJournalGrid_TTEvent) + ms);
            toolTip.SetToolTip(buttonField, (ftotalfilters > 0) ? string.Format("Total filtered out {0}".T(EDTx.UserControlJournalGrid_TTFilt3), ftotalfilters + ms) : "Filter out entries matching the field selection, ".T(EDTx.UserControlJournalGrid_TTTotal) + ms);
        }

	    #endregion

        #region Buttons

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            cfs.Open(GetSetting(dbFilter,"All"), b, this.FindForm());
        }

        private void EventFilterChanged(string newset, Object e)
        {
            string filters = GetSetting(dbFilter, "All");
            if (filters != newset)
            {
                PutSetting(dbFilter, newset);
                HistoryChanged(current_historylist);
            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            searchtimer.Stop();
            searchtimer.Start();
        }

        private void Searchtimer_Tick(object sender, EventArgs e)
        {
            searchtimer.Stop();
            Display(current_historylist, false);
        }

        private void comboBoxJournalWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            PutSetting(dbHistorySave, comboBoxTime.Text);
            HistoryChanged(current_historylist);
        }

        private void buttonField_Click(object sender, EventArgs e)
        {
            BaseUtils.ConditionLists res = HistoryFilterHelpers.ShowDialog(FindForm(), fieldfilter, discoveryform, "Journal: Filter out fields".T(EDTx.UserControlJournalGrid_JHF));
            if ( res != null )
            {
                fieldfilter = res;
                PutSetting(dbFieldFilter, fieldfilter.GetJSON());
                HistoryChanged(current_historylist);
            }
        }

        #endregion

        private void dataGridViewJournal_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            PaintHelpers.PaintEventColumn(sender as DataGridView, e,
                discoveryform.history.Count, (HistoryEntry)dataGridViewJournal.Rows[e.RowIndex].Tag,
                Columns.Event, false);
        }

        #region Mouse Clicks

        private void historyContextMenu_Opening(object sender, CancelEventArgs e)
        {
            mapGotoStartoolStripMenuItem.Enabled = (rightclicksystem != null && rightclicksystem.System.HasCoordinate);
            viewOnEDSMToolStripMenuItem.Enabled = (rightclicksystem != null);
            sendUnsyncedScanToEDDNToolStripMenuItem.Enabled = (rightclicksystem != null && EDDNClass.IsDelayableEDDNMessage(rightclicksystem.EntryType, rightclicksystem.EventTimeUTC) && !rightclicksystem.EDDNSync);
            removeSortingOfColumnsToolStripMenuItem.Enabled = dataGridViewJournal.SortedColumn != null;
            jumpToEntryToolStripMenuItem.Enabled = dataGridViewJournal.Rows.Count > 0;
        }

        HistoryEntry rightclicksystem = null;
        HistoryEntry leftclicksystem = null;

        private void dataGridViewJournal_MouseDown(object sender, MouseEventArgs e)
        {
            rightclicksystem = dataGridViewJournal.RightClickRowValid ? (HistoryEntry)dataGridViewJournal.Rows[dataGridViewJournal.RightClickRow].Tag : null;
            leftclicksystem = dataGridViewJournal.LeftClickRowValid ? (HistoryEntry)dataGridViewJournal.Rows[dataGridViewJournal.LeftClickRow].Tag : null;
        }

        private void dataGridViewJournal_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewJournal.LeftClickRowValid)                                                   // Click expands it..
            {
                ExtendedControls.InfoForm info = new ExtendedControls.InfoForm();
                leftclicksystem.FillInformation(out string EventDescription, out string EventDetailedInfo);
                string infodetailed = EventDescription.AppendPrePad(EventDetailedInfo, Environment.NewLine);
                info.Info( (EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(leftclicksystem.EventTimeUTC)) + ": " + leftclicksystem.EventSummary,
                    FindForm().Icon, infodetailed);
                info.Size = new Size(1200, 800);
                info.Show(FindForm());
            }
        }

        private void dataGridViewJournal_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            FireChangeSelection();
        }

        private void mapGotoStartoolStripMenuItem_Click(object sender, EventArgs e)
        {
            discoveryform.Open3DMap(rightclicksystem?.System);
        }

        private void viewOnEDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EDSMClass edsm = new EDSMClass();
            if (!edsm.ShowSystemInEDSM(rightclicksystem.System.Name))
                ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "System could not be found - has not been synched or EDSM is unavailable".T(EDTx.UserControlJournalGrid_NotSynced));
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
            if (rightclicksystem != null && EDDNClass.IsDelayableEDDNMessage(rightclicksystem.EntryType, rightclicksystem.EventTimeUTC) && !rightclicksystem.EDDNSync)
            {
                EDDNSync.SendEDDNEvent(discoveryform.LogLine, rightclicksystem);
            }
        }

        private void runActionsOnThisEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclicksystem != null)
                discoveryform.ActionRunOnEntry(rightclicksystem, Actions.ActionEventEDList.UserRightClick(rightclicksystem));
        }

        private void copyJournalEntryToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclicksystem != null && rightclicksystem.journalEntry != null)
            {
                string json = rightclicksystem.journalEntry.GetJsonString();
                if (json != null)
                {
                    SetClipboardText(json);
                }
            }
        }

        private void removeSortingOfColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Display(current_historylist, true);
        }

        #endregion

        Tuple<long, int> CurrentGridPosByJID()          // Returns JID, column index.  JID = -1 if cell is not defined
        {
            long jid = (dataGridViewJournal.CurrentCell != null) ? ((HistoryEntry)(dataGridViewJournal.Rows[dataGridViewJournal.CurrentCell.RowIndex].Tag)).Journalid : -1;
            int cellno = (dataGridViewJournal.CurrentCell != null) ? dataGridViewJournal.CurrentCell.ColumnIndex : 0;
            return new Tuple<long, int>(jid, cellno);
        }

        public void GotoPosByJID(long jid)
        {
            int rowno = DataGridViewControlHelpersStaticFunc.FindGridPosByID(rowsbyjournalid, jid, true);
            if (rowno >= 0)
            {
                dataGridViewJournal.SetCurrentAndSelectAllCellsOnRow(rowno);
                dataGridViewJournal.Rows[rowno].Selected = true;
                FireChangeSelection();
            }
        }

        public void FireChangeSelection()
        {
            System.Diagnostics.Debug.WriteLine("JG Fire Change Sel");
            if (dataGridViewJournal.CurrentCell != null)
            {
                int row = dataGridViewJournal.CurrentCell.RowIndex;
                OnTravelSelectionChanged?.Invoke(dataGridViewJournal.Rows[row].Tag as HistoryEntry, current_historylist, true);
            }
        }

        private void jumpToEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int curi = rightclicksystem != null ? (EDDConfig.Instance.OrderRowsInverted ? rightclicksystem.EntryNumber : (discoveryform.history.Count - rightclicksystem.EntryNumber + 1)) : 0;
            int selrow = dataGridViewJournal.JumpToDialog(this.FindForm(), curi, r =>
            {
                HistoryEntry he = r.Tag as HistoryEntry;
                return EDDConfig.Instance.OrderRowsInverted ? he.EntryNumber : (discoveryform.history.Count - he.EntryNumber + 1);
            });

            if (selrow >= 0)
            {
                dataGridViewJournal.ClearSelection();
                dataGridViewJournal.SetCurrentAndSelectAllCellsOnRow(selrow);
                FireChangeSelection();
            }
        }

        private void dataGridViewJournal_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 0)
            {
                e.SortDataGridViewColumnDate();
            }
        }


        #region Excel

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            Forms.ExportForm frm = new Forms.ExportForm();
            frm.Init(new string[] { "Export Current View", "Export as Journals" }, outputext: new string[] { "CSV export| *.csv", "Journal Export|*.log" });

            if (frm.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                if (frm.SelectedIndex == 1)
                {
                    try
                    { 
                        using (StreamWriter writer = new StreamWriter(frm.Path))
                        {
                            foreach(DataGridViewRow dgvr in dataGridViewJournal.Rows)
                            {
                                HistoryEntry he = dgvr.Tag as HistoryEntry;
                                if (dgvr.Visible && he.EventTimeUTC.CompareTo(frm.StartTimeUTC) >= 0 && he.EventTimeUTC.CompareTo(frm.EndTimeUTC) <= 0)
                                {
                                    string forExport = he.journalEntry.GetJsonString().Replace("\r\n", "");
                                    if (forExport != null)
                                    {
                                        forExport = System.Text.RegularExpressions.Regex.Replace(forExport, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
                                        writer.Write(forExport);
                                        writer.WriteLine();
                                    }
                                }
                            }
                        }

                        if (frm.AutoOpen)
                        {
                            try
                            {
                                System.Diagnostics.Process.Start(frm.Path);
                            }
                            catch
                            {
                                ExtendedControls.MessageBoxTheme.Show(FindForm(), "Failed to open " + frm.Path, "Warning".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                    }
                    catch
                    {
                        ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Failed to write to " + frm.Path, "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid();
                    grd.SetCSVDelimiter(frm.Comma);
                    grd.GetLineStatus += delegate (int r)
                    {
                        if (r < dataGridViewJournal.Rows.Count)
                        {
                            HistoryEntry he = dataGridViewJournal.Rows[r].Tag as HistoryEntry;
                            return (dataGridViewJournal.Rows[r].Visible &&
                                he.EventTimeUTC.CompareTo(frm.StartTimeUTC) >= 0 &&
                                he.EventTimeUTC.CompareTo(frm.EndTimeUTC) <= 0) ? BaseUtils.CSVWriteGrid.LineStatus.OK : BaseUtils.CSVWriteGrid.LineStatus.Skip;
                        }
                        else
                            return BaseUtils.CSVWriteGrid.LineStatus.EOF;
                    };

                    grd.GetLine += delegate (int r)
                    {
                        DataGridViewRow rw = dataGridViewJournal.Rows[r];
                        return new Object[] { rw.Cells[0].Value, rw.Cells[2].Value, rw.Cells[3].Value };
                    };

                    grd.GetHeader += delegate (int c)
                    {
                        return (c < 3 && frm.IncludeHeader) ? dataGridViewJournal.Columns[c + ((c > 0) ? 1 : 0)].HeaderText : null;
                    };

                    grd.WriteGrid(frm.Path, frm.AutoOpen, FindForm());
                }
            }
        }

        #endregion

    }
}
