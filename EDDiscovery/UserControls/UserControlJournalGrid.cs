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
using EliteDangerousCore.EDSM;
using EliteDangerousCore.EDDN;
using EliteDangerousCore.DB;
using EliteDangerousCore;
using System.IO;

namespace EDDiscovery.UserControls
{
    public partial class UserControlJournalGrid : UserControlCommonBase, IHistoryCursor
    {
        EventFilterSelector cfs = new EventFilterSelector();
        private Conditions.ConditionLists fieldfilter = new Conditions.ConditionLists();
        private Dictionary<long, DataGridViewRow> rowsbyjournalid = new Dictionary<long, DataGridViewRow>();

        private string DbFilterSave { get { return "JournalGridControlEventFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbColumnSave { get { return "JournalGrid" + ((displaynumber > 0) ? displaynumber.ToString() : "") + "DGVCol"; } }
        private string DbHistorySave { get { return "JournalEDUIHistory" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbFieldFilter { get { return "JournalGridControlFieldFilter" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbAutoTop { get { return "JournalGridControlAutoTop" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        public delegate void PopOut();
        public PopOut OnPopOut;

        private HistoryList current_historylist;        // the last one set, for internal refresh purposes on sort

        public event ChangedSelectionHandler OnChangedSelection;   // After a change of selection by the user, or after a OnHistoryChanged, or after a sort.
        public event ChangedSelectionHEHandler OnTravelSelectionChanged;   // as above, different format, for certain older controls
        public HistoryEntry GetCurrentHistoryEntry { get { return dataGridViewJournal.CurrentCell != null ? dataGridViewJournal.Rows[dataGridViewJournal.CurrentCell.RowIndex].Cells[JournalHistoryColumns.HistoryTag].Tag as HistoryEntry : null; } }


        #region Init

        private class JournalHistoryColumns
        {
            public const int Time = 0;
            public const int Event = 1;
            public const int Type = 2;
            public const int Text = 3;
            public const int HistoryTag = 2;
        }

        Timer searchtimer;

        public UserControlJournalGrid()
        {
            InitializeComponent();
            var corner = dataGridViewJournal.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            dataGridViewJournal.MakeDoubleBuffered();
            dataGridViewJournal.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewJournal.RowTemplate.Height = 26;
            dataGridViewJournal.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx
            cfs.ConfigureThirdOption("Travel", "Docked;FSD Jump;Undocked;");
            cfs.Changed += EventFilterChanged;
            TravelHistoryFilter.InitaliseComboBox(comboBoxJournalWindow, DbHistorySave);

            checkBoxMoveToTop.Checked = SQLiteConnectionUser.GetSettingBool(DbAutoTop, true);

            string filter = SQLiteDBClass.GetSettingString(DbFieldFilter, "");
            if (filter.Length > 0)
                fieldfilter.FromJSON(filter);        // load filter

            searchtimer = new Timer() { Interval = 500 };
            searchtimer.Tick += Searchtimer_Tick;

            discoveryform.OnHistoryChange += Display;
            discoveryform.OnNewEntry += AddNewEntry;
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
            SQLiteConnectionUser.PutSettingBool(DbAutoTop, checkBoxMoveToTop.Checked);
            searchtimer.Dispose();
        }

        #endregion

        #region Display

        public override void InitialDisplay()
        {
            Display(discoveryform.history);
        }

        int fdropdown, ftotalevents, ftotalfilters;     // filter totals

        private void Display(HistoryList hl)
        {
            Display(hl, false);
        }

        private void Display(HistoryList hl, bool disablesorting )
        {
            if (hl == null)     // just for safety
                return;

            current_historylist = hl;

            Tuple<long, int> pos = CurrentGridPosByJID();

            SortOrder sortorder = dataGridViewJournal.SortOrder;
            int sortcol = dataGridViewJournal.SortedColumn?.Index ?? -1;
            if (sortcol >= 0 && disablesorting)
            {
                dataGridViewJournal.Columns[sortcol].HeaderCell.SortGlyphDirection = SortOrder.None;
                sortcol = -1;
            }

            var filter = (TravelHistoryFilter)comboBoxJournalWindow.SelectedItem ?? TravelHistoryFilter.NoFilter;

            List<HistoryEntry> result = filter.Filter(hl);
            fdropdown = hl.Count() - result.Count();

            result = HistoryList.FilterByJournalEvent(result, SQLiteDBClass.GetSettingString(DbFilterSave, "All"), out ftotalevents);
            result = FilterHelpers.FilterHistory(result, fieldfilter, discoveryform.Globals, out ftotalfilters);


            dataGridViewJournal.Rows.Clear();
            rowsbyjournalid.Clear();

            for (int ii = 0; ii < result.Count; ii++) //foreach (var item in result)
            {
                AddNewJournalRow(false, result[ii]);      // for every one in filter, add a row.
            }

            StaticFilters.FilterGridView(dataGridViewJournal, textBoxFilter.Text);

            UpdateToolTipsForFilter();

            int rowno = FindGridPosByJID(pos.Item1,true);

            if (rowno >= 0)
            {
                dataGridViewJournal.CurrentCell = dataGridViewJournal.Rows[rowno].Cells[pos.Item2];       // its the current cell which needs to be set, moves the row marker as well            currentGridRow = (rowno!=-1) ? 
            }

            dataGridViewJournal.Columns[0].HeaderText = EDDiscoveryForm.EDDConfig.DisplayUTC ? "Game Time" : "Time";

            if (sortcol >= 0)
            {
                dataGridViewJournal.Sort(dataGridViewJournal.Columns[sortcol], (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                dataGridViewJournal.Columns[sortcol].HeaderCell.SortGlyphDirection = sortorder;
            }

            FireChangeSelection();
        }


        private void AddNewJournalRow(bool insert, HistoryEntry item)            // second part of add history row, adds item to view.
        {
            string detail = "";
            if (item.EventDescription.Length > 0)
                detail = item.EventDescription;
            if (item.EventDetailedInfo.Length > 0)
                detail += ((detail.Length > 0) ? Environment.NewLine : "") + item.EventDetailedInfo;

            var rw = dataGridViewJournal.RowTemplate.Clone() as DataGridViewRow;
            rw.CreateCells(dataGridViewJournal, EDDiscoveryForm.EDDConfig.DisplayUTC ? item.EventTimeUTC : item.EventTimeLocal, "", item.EventSummary, detail);
            rw.Cells[JournalHistoryColumns.HistoryTag].Tag = item;

            int rownr = 0;

            if (insert)
            {
                dataGridViewJournal.Rows.Insert(rownr, rw);
            }
            else
            {
                rownr = dataGridViewJournal.Rows.Add(rw);
            }   

            rowsbyjournalid[item.Journalid] = dataGridViewJournal.Rows[rownr];
        }

        private void AddNewEntry(HistoryEntry he, HistoryList hl)               // add if in event filter, and not in field filter..
        {
            bool add = he.IsJournalEventInEventFilter(SQLiteDBClass.GetSettingString(DbFilterSave, "All"));

            if (!add)
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

            if ( add )
            {
                AddNewJournalRow(true, he);

                var filter = (TravelHistoryFilter)comboBoxJournalWindow.SelectedItem ?? TravelHistoryFilter.NoFilter;

                if (filter.MaximumNumberOfItems != null)
                {
                    for (int r = dataGridViewJournal.Rows.Count - 1; r >= dataGridViewJournal.Rows.Count; r--)
                    {
                        dataGridViewJournal.Rows.RemoveAt(r);
                    }
                }

                if (filter.MaximumDataAge != null)
                {
                    for (int r = dataGridViewJournal.Rows.Count - 1; r > 0; r--)
                    {
                        var rhe = dataGridViewJournal.Rows[r].Tag as HistoryEntry;
                        if (rhe != null && rhe.AgeOfEntry() > filter.MaximumDataAge)
                        {
                            dataGridViewJournal.Rows.RemoveAt(r);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (checkBoxMoveToTop.Checked && dataGridViewJournal.DisplayedRowCount(false) > 0)   // Move focus to new row
                {
                    dataGridViewJournal.ClearSelection();
                    int rowno = dataGridViewJournal.Rows.GetFirstRow(DataGridViewElementStates.Visible);
                    if (rowno != -1)
                        dataGridViewJournal.CurrentCell = dataGridViewJournal.Rows[rowno].Cells[1];       // its the current cell which needs to be set, moves the row marker as well

                    FireChangeSelection();
                }
            }
        }

        private void UpdateToolTipsForFilter()
        {
            string ms = " showing " + dataGridViewJournal.Rows.Count + " original " + (current_historylist?.Count() ?? 0);
            comboBoxJournalWindow.SetTipDynamically(toolTip, fdropdown > 0 ? ("Filtered " + fdropdown + ms) : "Select the entries by age, " + ms);
            toolTip.SetToolTip(buttonFilter, (ftotalevents > 0) ? ("Filtered " + ftotalevents + ms) : "Filter out entries based on event type, " + ms);
            toolTip.SetToolTip(buttonField, (ftotalfilters > 0) ? ("Total filtered out " + ftotalfilters + ms) : "Filter out entries matching the field selection, " + ms);
        }

	    #endregion

        #region Buttons

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

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            searchtimer.Stop();
            searchtimer.Start();
        }

        private void Searchtimer_Tick(object sender, EventArgs e)
        {
            searchtimer.Stop();
            this.Cursor = Cursors.WaitCursor;

            Tuple<long, int> pos = CurrentGridPosByJID();

            StaticFilters.FilterGridView(dataGridViewJournal, textBoxFilter.Text);

            int rowno = FindGridPosByJID(pos.Item1,true);
            if (rowno >= 0)
                dataGridViewJournal.CurrentCell = dataGridViewJournal.Rows[rowno].Cells[pos.Item2];       // its the current cell which needs to be set, moves the row marker as well            currentGridRow = (rowno!=-1) ? 

            this.Cursor = Cursors.Default;
        }

        private void comboBoxJournalWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingString(DbHistorySave, comboBoxJournalWindow.Text);
            Display(current_historylist);
        }

        private void buttonField_Click(object sender, EventArgs e)
        {
            Conditions.ConditionFilterForm frm = new Conditions.ConditionFilterForm();
            List<string> namelist = new List<string>() { "Note" };
            namelist.AddRange(discoveryform.Globals.NameList);
            frm.InitFilter("Journal: Filter out fields",
                            Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location),
                            JournalEntry.GetListOfEventsWithOptMethod(false) ,
                            (s) => { return BaseUtils.FieldNames.GetPropertyFieldNames(JournalEntry.TypeOfJournalEntry(s)); },
                            namelist, fieldfilter);
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
            removeSortingOfColumnsToolStripMenuItem.Enabled = dataGridViewJournal.SortedColumn != null;
        }

        HistoryEntry rightclicksystem = null;
        int rightclickrow = -1;

        private void dataGridViewJournal_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)         // right click on travel map, get in before the context menu
            {
                rightclicksystem = null;
                rightclickrow = -1;
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
            long? id_edsm = rightclicksystem.System?.EDSMID;

            if (id_edsm == 0)
            {
                id_edsm = null;
            }

            if (!edsm.ShowSystemInEDSM(rightclicksystem.System.Name, id_edsm))
                ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "System could not be found - has not been synched or EDSM is unavailable");

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
                Newtonsoft.Json.Linq.JObject jo = rightclicksystem.journalEntry.GetJson();
                string json = jo?.ToString();
                if (json != null)
                {
                    Clipboard.SetText(json);
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
            long jid = (dataGridViewJournal.CurrentCell != null) ? ((HistoryEntry)(dataGridViewJournal.Rows[dataGridViewJournal.CurrentCell.RowIndex].Cells[JournalHistoryColumns.HistoryTag].Tag)).Journalid : -1;
            int cellno = (dataGridViewJournal.CurrentCell != null) ? dataGridViewJournal.CurrentCell.ColumnIndex : 0;
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
                dataGridViewJournal.CurrentCell = dataGridViewJournal.Rows[rowno].Cells[JournalHistoryColumns.Event];
                dataGridViewJournal.Rows[rowno].Selected = true;
                FireChangeSelection();
            }
        }

        #region Excel

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            Forms.ExportForm frm = new Forms.ExportForm();
            frm.Init(new string[] { "Export Current View" }, allowRawJournalExport: true);

            if (frm.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                if (frm.ExportAsJournals)
                {
                    try
                    { 
                        using (StreamWriter writer = new StreamWriter(frm.Path))
                        {
                            foreach(DataGridViewRow dgvr in dataGridViewJournal.Rows)
                            {
                                HistoryEntry he = dgvr.Cells[JournalHistoryColumns.HistoryTag].Tag as HistoryEntry;
                                if (dgvr.Visible && he.EventTimeLocal.CompareTo(frm.StartTime) >= 0 && he.EventTimeLocal.CompareTo(frm.EndTime) <= 0)
                                {
                                    string forExport = he.journalEntry.GetJson()?.ToString().Replace("\r\n", "");
                                    forExport = System.Text.RegularExpressions.Regex.Replace(forExport, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
                                    writer.Write(forExport);
                                    writer.WriteLine();
                                }
                            }
                        }
                        if (frm.AutoOpen)
                            System.Diagnostics.Process.Start(frm.Path);
                    }
                    catch
                    {
                        ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Failed to write to " + frm.Path, "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    if (frm.SelectedIndex == 0)
                    {
                        BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid();
                        grd.SetCSVDelimiter(frm.Comma);
                        grd.GetLineStatus += delegate (int r)
                        {
                            if (r < dataGridViewJournal.Rows.Count)
                            {
                                HistoryEntry he = dataGridViewJournal.Rows[r].Cells[JournalHistoryColumns.HistoryTag].Tag as HistoryEntry;
                                return (dataGridViewJournal.Rows[r].Visible &&
                                    he.EventTimeLocal.CompareTo(frm.StartTime) >= 0 &&
                                    he.EventTimeLocal.CompareTo(frm.EndTime) <= 0) ? BaseUtils.CSVWriteGrid.LineStatus.OK : BaseUtils.CSVWriteGrid.LineStatus.Skip;
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

                        if (grd.WriteCSV(frm.Path))
                        {
                            if (frm.AutoOpen)
                                System.Diagnostics.Process.Start(frm.Path);
                        }
                        else
                            ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Failed to write to " + frm.Path, "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  
                    }
                }
            }
        }

        #endregion

        private void dataGridViewJournal_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 0)
            {
                e.SortDataGridViewColumnDate();
            }
        }

        private void dataGridViewJournal_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            FireChangeSelection();
        }

        public void FireChangeSelection()
        {
            if (dataGridViewJournal.CurrentCell != null)
            {
                int row = dataGridViewJournal.CurrentCell.RowIndex;
                //System.Diagnostics.Debug.WriteLine("Fire Change Sel row" + row);
                if (OnChangedSelection != null)
                    OnChangedSelection(row, dataGridViewJournal.CurrentCell.ColumnIndex, false, false);
                if (OnTravelSelectionChanged != null)
                    OnTravelSelectionChanged(dataGridViewJournal.Rows[row].Cells[JournalHistoryColumns.HistoryTag].Tag as HistoryEntry, current_historylist);
            }
        }
    }
}
