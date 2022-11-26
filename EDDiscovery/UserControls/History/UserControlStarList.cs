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
using EliteDangerousCore.EDSM;
using EliteDangerousCore.JournalEvents;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlStarList : UserControlCommonBase, IHistoryCursor
    {
        #region Public IF

        public HistoryEntry GetCurrentHistoryEntry { get { return dataGridViewStarList.CurrentCell != null ?
                    (dataGridViewStarList.Rows[dataGridViewStarList.CurrentCell.RowIndex].Tag as HistoryEntry) : null; } }

        #endregion

        #region Events

        // implement IHistoryCursor fields
        public event ChangedSelectionHEHandler OnTravelSelectionChanged;   // as above, different format, for certain older controls

        #endregion

        #region Init
        private class Columns
        {
            public const int LastVisit = 0;
            public const int StarName = 1;
            public const int NoVisits = 2;
            public const int OtherInformation = 3;
            public const int SystemValue = 4;
        }

        private string dbHistorySave = "EDUIHistory";
        private string dbEDSM = "EDSM";
        private string dbDisplayFilters = "DisplayFilters";

        private string[] displayfilters;        // display filters

        const int lowRadiusLimit = 300 * 1000; // tiny body limit in km converted to m
        const int largeRadiusLimit = 20000 * 1000; // large body limit in km converted to m
        const double eccentricityLimit = 0.95; //orbital eccentricity limit

        private Dictionary<long, DataGridViewRow> rowsbyjournalid = new Dictionary<long, DataGridViewRow>();

        private string searchterms = "system:body:station:stationfaction";

        private Timer searchtimer;

        private Timer autoupdateedsm;
        int autoupdaterowstart = 0;
        int autoupdaterowoffset = 0;

        private Timer todotimer;
        private Queue<Action> todo = new Queue<Action>();
        private Queue<HistoryEntry> queuedadds = new Queue<HistoryEntry>();

        public UserControlStarList()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "StarListControl";

            checkBoxCursorToTop.Checked = true;

            dataGridViewStarList.MakeDoubleBuffered();
            dataGridViewStarList.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewStarList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx

            dataGridViewStarList.Columns[2].ValueType = typeof(Int32);

            checkBoxEDSM.Checked = GetSetting(dbEDSM, false);
            this.checkBoxEDSM.CheckedChanged += new System.EventHandler(this.checkBoxEDSM_CheckedChanged);

            displayfilters = GetSetting(dbDisplayFilters, "stars;planets;signals;volcanism;values;shortinfo;gravity;atmos;rings;valueables;organics;codex").Split(';');

            discoveryform.OnHistoryChange += HistoryChanged;
            discoveryform.OnNewEntry += AddNewEntry;

            searchtimer = new Timer() { Interval = 500 };
            searchtimer.Tick += Searchtimer_Tick;
            autoupdateedsm = new Timer() { Interval = 2000 };
            autoupdateedsm.Tick += Autoupdateedsm_Tick;
            todotimer = new Timer() { Interval = 20 };
            todotimer.Tick += Todotimer_Tick;

            var enumlist = new Enum[] { EDTx.UserControlStarList_ColumnTime, EDTx.UserControlStarList_ColumnSystem, EDTx.UserControlStarList_ColumnVisits, EDTx.UserControlStarList_ColumnInformation, EDTx.UserControlStarList_Value, EDTx.UserControlStarList_labelTime, EDTx.UserControlStarList_labelSearch };
            var enumlistcms = new Enum[] { EDTx.UserControlStarList_removeSortingOfColumnsToolStripMenuItem, EDTx.UserControlStarList_mapGotoStartoolStripMenuItem, EDTx.UserControlStarList_viewOnEDSMToolStripMenuItem, EDTx.UserControlStarList_setNoteToolStripMenuItem, EDTx.UserControlStarList_viewScanDisplayToolStripMenuItem };
            var enumlisttt = new Enum[] { EDTx.UserControlStarList_comboBoxTime_ToolTip, EDTx.UserControlStarList_checkBoxEDSM_ToolTip, EDTx.UserControlStarList_textBoxSearch_ToolTip, EDTx.UserControlStarList_buttonExtExcel_ToolTip, EDTx.UserControlStarList_checkBoxCursorToTop_ToolTip };

            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);
            BaseUtils.Translator.Instance.TranslateToolstrip(contextMenuStrip, enumlistcms, this);
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);

            TravelHistoryFilter.InitaliseComboBox(comboBoxTime, GetSetting(dbHistorySave,""), false);

             if (TranslatorExtensions.TxDefined(EDTx.UserControlTravelGrid_SearchTerms))     // if translator has it defined, use it (share with travel grid)
                searchterms = searchterms.TxID(EDTx.UserControlTravelGrid_SearchTerms);
        }

        public override void LoadLayout()
        {
            dataGridViewStarList.RowTemplate.MinimumHeight = Math.Max(26, Font.ScalePixels(26));
            DGVLoadColumnLayout(dataGridViewStarList);
        }

        public override void Closing()
        {
            todo.Clear();
            todotimer.Stop();
            searchtimer.Stop();
            DGVSaveColumnLayout(dataGridViewStarList);
            discoveryform.OnHistoryChange -= HistoryChanged;
            discoveryform.OnNewEntry -= AddNewEntry;
        }

        public override void InitialDisplay()
        {
            HistoryChanged(discoveryform.history);
        }

        public void HistoryChanged(HistoryList hl)           // on History change
        {
            Display(false);
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
            if (todotimer.Enabled)      // if loading, add to queue..
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
            bool scan = he.journalEntry is IStarScan;                           // is this a scan node

            if (he.IsFSDCarrierJump || scan)                                      // jumped or scan
            {
                DataGridViewRow rowpresent = null;
                HistoryEntry rowhe = null;

                foreach (DataGridViewRow rowf in dataGridViewStarList.Rows)
                {
                    var her = rowf.Tag as HistoryEntry;
                    if (her != null && her.System.Name.Equals(he.System.Name))
                    {
                        rowpresent = rowf;
                        rowhe = rowpresent.Tag as HistoryEntry;
                        break;
                    }
                }

                if (he.IsFSDCarrierJump)                                      // jumped
                {
                    bool added = false;                                         // this means row 0 is visible and added

                    if (rowpresent != null)                                     // if its in the list, move to top and set visit count
                    {
                        dataGridViewStarList.Rows.Remove(rowpresent);
                        rowpresent.Cells[2].Value = discoveryform.history.Visits(he.System.Name);  // update count
                        dataGridViewStarList.Rows.Insert(0, rowpresent);        // move to top..
                        added = true;
                    }
                    else
                    {                                                           // not in the list, add it
                        var visitlist = discoveryform.history.Visited.Values.ToList();
                        visitlist.Sort(delegate (HistoryEntry left, HistoryEntry right) { return right.EventTimeUTC.CompareTo(left.EventTimeUTC); });
                        var filter = (TravelHistoryFilter)comboBoxTime.SelectedItem ?? TravelHistoryFilter.NoFilter;
                        visitlist = filter.FilterLatestFirst(visitlist);      // and filter

                        if (visitlist.Count > 0 && visitlist[0].System.Name == he.System.Name) // if the filtered result has our system in (which must be the first, since its newest), its inside the filter, add
                        {
                            var sst = new BaseUtils.StringSearchTerms(textBoxSearch.Text, searchterms);
                            var row = CreateHistoryRow(visitlist[0], sst);
                            if (row != null)    // text may have filtered it out
                            {
                                dataGridViewStarList.Rows.Insert(0, row);
                                added = true;
                            }
                        }
                    }

                    if (added && checkBoxCursorToTop.Checked)           // if we have a row 0
                    {
                        //System.Diagnostics.Debug.WriteLine("Auto Sel");
                        dataGridViewStarList.ClearSelection();
                        dataGridViewStarList.SetCurrentAndSelectAllCellsOnRow(0);       // its the current cell which needs to be set, moves the row marker as well
                        FireChangeSelection();
                    }
                }
                else if (scan)    // these can affect the display, so update
                {
                    if (rowpresent != null)       // only need to do something if its displayed
                    {
                        var node = discoveryform.history.StarScan?.FindSystemSynchronous(rowhe.System, false); // may be null
                        string info = Infoline(rowhe.System, node);  // lookup node, using star name, no EDSM lookup.
                        rowpresent.Cells[3].Value = info;   // update info
                        rowpresent.Cells[4].Value = node?.ScanValue(true).ToString("N0") ?? "0"; // update scan value
                    }
                }
            }
        }


        #endregion

        #region Display

        public void Display(bool disablesorting)        
        {
            todo.Clear();               // clear queues and stop timer
            queuedadds.Clear();
            todotimer.Stop();

            this.dataGridViewStarList.Cursor = Cursors.WaitCursor;

            var pos = CurrentGridPosByIndex();

            SortOrder sortorder = dataGridViewStarList.SortOrder;
            int sortcol = dataGridViewStarList.SortedColumn?.Index ?? -1;
            if (sortcol >= 0 && disablesorting)
            {
                dataGridViewStarList.Columns[sortcol].HeaderCell.SortGlyphDirection = SortOrder.None;
                sortcol = -1;
            }

            rowsbyjournalid.Clear();
            dataGridViewStarList.Rows.Clear();

            dataGridViewStarList.Columns[0].HeaderText = EDDConfig.Instance.GetTimeTitle();

            var visitlist = discoveryform.history.Visited.Values.ToList();

            // sort is latest first
            visitlist.Sort(delegate (HistoryEntry left, HistoryEntry right) { return right.EventTimeUTC.CompareTo(left.EventTimeUTC); });

            var filter = (TravelHistoryFilter)comboBoxTime.SelectedItem ?? TravelHistoryFilter.NoFilter;
            visitlist = filter.FilterLatestFirst(visitlist);      // and filter

            //foreach (var v in visitlist) System.Diagnostics.Debug.WriteLine("Visit {0} {1} {2}", v.EventTimeUTC, v.System.Name, v.Visits);

            List<HistoryEntry[]> syslistchunks = new List<HistoryEntry[]>();

            int chunksize = 500;
            for (int i = 0; i < visitlist.Count; i += chunksize, chunksize = 2000)
            {
                int totake = Math.Min(chunksize, visitlist.Count - i);
                HistoryEntry[] syslistchunk = new HistoryEntry[totake];
                visitlist.CopyTo(i, syslistchunk, 0, totake);
                syslistchunks.Add(syslistchunk);
            }

            var sst = new BaseUtils.StringSearchTerms(textBoxSearch.Text, searchterms);

            //System.Diagnostics.Stopwatch swtotal = new System.Diagnostics.Stopwatch(); swtotal.Start();

            foreach (var syslistchunk in syslistchunks)
            {
                todo.Enqueue(() =>
                {
                    List<DataGridViewRow> rowstoadd = new List<DataGridViewRow>();

                    foreach (var he in syslistchunk)
                    {
                        var row = CreateHistoryRow(he, sst);
                        if (row != null)
                            rowstoadd.Add(row);
                    }

                    dataGridViewStarList.Rows.AddRange(rowstoadd.ToArray());
                });

                if (dataGridViewStarList.MoveToSelection(rowsbyjournalid, ref pos, false))
                    FireChangeSelection();
            }

            todo.Enqueue(() =>
            {
                if (dataGridViewStarList.MoveToSelection(rowsbyjournalid, ref pos, true))
                    FireChangeSelection();

                if (sortcol >= 0)
                {
                    dataGridViewStarList.Sort(dataGridViewStarList.Columns[sortcol], (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                    dataGridViewStarList.Columns[sortcol].HeaderCell.SortGlyphDirection = sortorder;
                }

                autoupdaterowoffset = autoupdaterowstart = 0;
                autoupdateedsm.Start();

                this.dataGridViewStarList.Cursor = Cursors.Arrow;

                while (queuedadds.Count > 0)              // finally, dequeue any adds added
                {
                    System.Diagnostics.Debug.WriteLine("SL Dequeue paused adds");
                    AddEntry(queuedadds.Dequeue());
                }
            });

            todotimer.Start();
        }

        private DataGridViewRow CreateHistoryRow(HistoryEntry he, BaseUtils.StringSearchTerms search)
        {
            //string debugt = item.Journalid + "  " + item.System.id_edsm + " " + item.System.GetHashCode() + " "; // add on for debug purposes to a field below

            DateTime time = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(he.EventTimeUTC);
            var node = discoveryform.history.StarScan?.FindSystemSynchronous(he.System, false); // may be null

            string visits = discoveryform.history.Visits(he.System.Name).ToString();
            string info = Infoline(he.System, node);  // lookup node, using star name, no EDSM lookup.

            if (search.Enabled)
            {
                bool matched = false;

                if (search.Terms[0] != null)
                {
                    string timestr = time.ToString();
                    matched = timestr.IndexOf(search.Terms[0], StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                                he.System.Name.IndexOf(search.Terms[0], StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                                visits.IndexOf(search.Terms[0], StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                                info.IndexOf(search.Terms[0], StringComparison.InvariantCultureIgnoreCase) >= 0;
                }

                if (!matched && search.Terms[1] != null)       // system
                    matched = he.System.Name.WildCardMatch(search.Terms[1], true);
                if (!matched && search.Terms[2] != null)       // body
                    matched = he.Status.BodyName?.WildCardMatch(search.Terms[2], true) ?? false;
                if (!matched && search.Terms[3] != null)       // station
                    matched = he.Status.StationName?.WildCardMatch(search.Terms[3], true) ?? false;
                if (!matched && search.Terms[4] != null)       // stationfaction
                    matched = he.Status.StationFaction?.WildCardMatch(search.Terms[4], true) ?? false;

                if (!matched)
                    return null;
            }

            var rw = dataGridViewStarList.RowTemplate.Clone() as DataGridViewRow;
            rw.CreateCells(dataGridViewStarList, time, he.System.Name, visits, info, node?.ScanValue(true).ToString("N0") ?? "0");

            rowsbyjournalid[he.Journalid] = rw;      
            rw.Tag = he;

            rw.Cells[1].Style.ForeColor = (he.System.HasCoordinate) ? Color.Empty : ExtendedControls.Theme.Current.UnknownSystemColor;

            he.FillInformation(out string EventDescription, out string EventDetailedInfo);

            string tip = String.Join(Environment.NewLine, he.EventSummary, EventDescription, EventDetailedInfo);

            rw.Cells[1].ToolTipText = tip;
            rw.Cells[3].ToolTipText = tip;

            return rw;
        }

        string Infoline(ISystem system, StarScan.SystemNode sysnode )
        {
            string infostr = "";

            if (sysnode?.StarNodes != null)
            {
                string st = sysnode.StarTypesFound();
                int stars = sysnode.StarsScanned();
                int total = sysnode.StarPlanetsScanned();

                infostr = string.Format("{0} Star(s) {1}".T(EDTx.UserControlStarList_CS), stars, st);

                if (total > stars)
                {
                    infostr += " " + string.Format("{0} Other bodies".T(EDTx.UserControlStarList_OB), (total - stars).ToString());
                }

                if (sysnode.FSSTotalBodies.HasValue && total < sysnode.FSSTotalBodies.Value)        // only show if you've not got them all
                {
                    infostr += ", " + "Total".T(EDTx.UserControlStarList_Total) + " " + sysnode.FSSTotalBodies.Value.ToString();
                }

                bool showcodex = displayfilters.Contains("codex");

                if ( sysnode.CodexEntryList.Count>0 && showcodex)
                {
                    foreach( var c in sysnode.CodexEntryList)
                    {
                        infostr = infostr.AppendPrePad(c.Info(), Environment.NewLine);
                    }
                }


                string jumponium = "";

                // selectors for showing something
                bool showplanets = displayfilters.Contains("planets");
                bool showstars = displayfilters.Contains("stars");
                bool showvalueables = displayfilters.Contains("valueables");
                bool showbeltclusters = displayfilters.Contains("beltcluster");

                // selectors for what to print
                bool showsignals = displayfilters.Contains("signals");
                bool showvol = displayfilters.Contains("volcanism");
                bool showv = displayfilters.Contains("values");
                bool showsi = displayfilters.Contains("shortinfo");
                bool showg = displayfilters.Contains("gravity");
                bool showatmos = displayfilters.Contains("atmos");
                bool showrings = displayfilters.Contains("rings");
                bool showorganics = displayfilters.Contains("organics");

                bool showjumponium = displayfilters.Contains("jumponium");                

                foreach (StarScan.ScanNode sn in sysnode.Bodies)
                {
                    if (sn?.ScanData != null )  // must have scan data..
                    {
                         if (
                            (sn.ScanData.IsBeltCluster && showbeltclusters) ||     // major selectors for line display
                            (sn.ScanData.IsPlanet && showplanets) ||
                            (sn.ScanData.IsStar && showstars) ||
                            (showvalueables && (sn.ScanData.AmmoniaWorld || sn.ScanData.CanBeTerraformable || sn.ScanData.WaterWorld || sn.ScanData.Earthlike))
                            )
                        {
                            string info = sn.SurveyorInfoLine(system, showsignals, showorganics, 
                                                                showvol, showv, showsi, showg,
                                                                showatmos && sn.ScanData.IsLandable, showrings,
                                                                lowRadiusLimit, largeRadiusLimit, eccentricityLimit);
                            infostr = infostr.AppendPrePad(info, Environment.NewLine);
                        }

                        sn.ScanData.AccumulateJumponium(ref jumponium, sn.ScanData.BodyDesignationOrName);
                    }
                }

                if (jumponium.HasChars() )
                {
                    infostr = infostr.AppendPrePad("This system has materials for FSD boost".T(EDTx.UserControlStarList_FSD), Environment.NewLine);
                    if (showjumponium)
                        infostr = infostr.AppendPrePad(jumponium, Environment.NewLine);
                }
            }

            return infostr;
        }

        #endregion

        #region Row paint

        private void dataGridViewStarList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            int rown = EDDConfig.Instance.OrderRowsInverted ? (grid.RowCount - e.RowIndex) : (e.RowIndex + 1);
            string rowIdx = rown.ToString();

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);

            using (var centerFormat = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Brush br = new SolidBrush(grid.RowHeadersDefaultCellStyle.ForeColor))
                    e.Graphics.DrawString(rowIdx, grid.RowHeadersDefaultCellStyle.Font, br, headerBounds, centerFormat);
            }

        }

        #endregion

        #region UI

        Tuple<long, int> CurrentGridPosByJID()          // Returns JID, column index.  JID = -1 if cell is not defined
        {
            long jid = (dataGridViewStarList.CurrentCell != null) ? (dataGridViewStarList.Rows[dataGridViewStarList.CurrentCell.RowIndex].Tag as HistoryEntry).Journalid : -1;
            int cellno = (dataGridViewStarList.CurrentCell != null) ? dataGridViewStarList.CurrentCell.ColumnIndex : 0;
            return new Tuple<long, int>(jid, cellno);
        }

        public int GotoPosByJID(long jid)      // uccursor requirement. -1 if fails
        {
            int rowno = DataGridViewControlHelpersStaticFunc.FindGridPosByID(rowsbyjournalid, jid, true);
            if (rowno >= 0)
            {
                dataGridViewStarList.SetCurrentAndSelectAllCellsOnRow(rowno);
                dataGridViewStarList.Rows[rowno].Selected = true;
                FireChangeSelection();
            }
            return rowno;
        }

        public void FireChangeSelection() // uccursor requirement
        {
            if (dataGridViewStarList.CurrentCell != null)
            {
                int row = dataGridViewStarList.CurrentCell.RowIndex;
                //System.Diagnostics.Debug.WriteLine("Fire Change Sel row" + row);
                OnTravelSelectionChanged?.Invoke((dataGridViewStarList.Rows[row].Tag as HistoryEntry), discoveryform.history, true);
            }
        }

        Tuple<long, int> CurrentGridPosByIndex()          // Returns Index, column index.  Index = -1 if cell is not defined
        {
            long index = (dataGridViewStarList.CurrentCell != null) ? (dataGridViewStarList.Rows[dataGridViewStarList.CurrentCell.RowIndex].Tag as HistoryEntry).Journalid : -1;
            int cellno = (dataGridViewStarList.CurrentCell != null) ? dataGridViewStarList.CurrentCell.ColumnIndex : 0;
            return new Tuple<long, int>(index, cellno);
        }

        public void CheckEDSM()
        {
            if (dataGridViewStarList.CurrentCell != null)
                CheckEDSM(dataGridViewStarList.CurrentRow);
        }

        public async void CheckEDSM(DataGridViewRow row)
        {
            HistoryEntry he = row.Tag as HistoryEntry;
            var node = await discoveryform.history.StarScan?.FindSystemAsync(he.System, true);  // try an EDSM lookup, cache data, then redisplay.
            row.Cells[Columns.OtherInformation].Value = Infoline(he.System,node);
            row.Cells[Columns.SystemValue].Value = node?.ScanValue(true).ToString("N0") ?? "";
        }

        private void Autoupdateedsm_Tick(object sender, EventArgs e)            // tick tock to get edsm data very slowly!
        {
            if (dataGridViewStarList.FirstDisplayedCell != null && checkBoxEDSM.Checked)
            {
                int top = dataGridViewStarList.FirstDisplayedCell.RowIndex;
                if (top != autoupdaterowstart)
                {
                    autoupdaterowstart = top;
                    autoupdaterowoffset = 0;
                }

                while (true)
                {
                    int row = autoupdaterowstart + autoupdaterowoffset;
                    DataGridViewRow rw = (row >= 0 && row < dataGridViewStarList.Rows.Count) ? dataGridViewStarList.Rows[row] : null;

                    if (rw == null || rw.Displayed == false)        // don't go beyond end or beyond displayed area
                    {
                        //System.Diagnostics.Debug.WriteLine("Stop at " + row + " " + autoupdaterowoffset);
                        break;
                    }

                    autoupdaterowoffset++;

                    HistoryEntry he = rw.Tag as HistoryEntry;
                        
                    if (!EDSMClass.HasBodyLookupOccurred(he.System.Name))       // this tells us if a body lookup has occurred
                    {
                        System.Diagnostics.Debug.WriteLine("StarList EDSM Update row" + row);
                        CheckEDSM(rw);
                        break;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("StarList Skip row as already checked " + row);
                    }
                }
            }
        }

        private void comboBoxHistoryWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            PutSetting(dbHistorySave, comboBoxTime.Text);
            Display(false);
        }

        private void dataGridViewTravel_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CheckEDSM();
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
                CheckEDSM();
                FireChangeSelection();
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
            Display(false);
        }


        HistoryEntry rightclicksystem = null;

        private void dataGridViewTravel_MouseDown(object sender, MouseEventArgs e)
        {
            rightclicksystem = dataGridViewStarList.RightClickRowValid ? (HistoryEntry)(dataGridViewStarList.Rows[dataGridViewStarList.RightClickRow].Tag as HistoryEntry) : null;
        }

        #endregion

        #region TravelHistoryRightClick

        private void historyContextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (dataGridViewStarList.SelectedCells.Count == 0)      // need something selected  stops context menu opening on nothing..
                e.Cancel = true;

            mapGotoStartoolStripMenuItem.Enabled = (rightclicksystem != null && rightclicksystem.System.HasCoordinate);
            viewOnEDSMToolStripMenuItem.Enabled = (rightclicksystem != null);
            removeSortingOfColumnsToolStripMenuItem.Enabled = dataGridViewStarList.SortedColumn != null;
        }

        private void removeSortingOfColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Display(true);
        }

        private void mapGotoStartoolStripMenuItem_Click(object sender, EventArgs e)
        {
            discoveryform.Open3DMap(rightclicksystem?.System);
        }

        private void viewOnEDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.EDSM.EDSMClass edsm = new EDSMClass();

            if (!edsm.ShowSystemInEDSM(rightclicksystem.System.Name))
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "System could not be found - has not been synched or EDSM is unavailable".T(EDTx.UserControlStarList_NoEDSM));
        }

        private void setNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclicksystem != null)
            {
                using (Forms.SetNoteForm noteform = new Forms.SetNoteForm(rightclicksystem, discoveryform))
                {
                    if (noteform.ShowDialog(FindForm()) == DialogResult.OK)
                    {
                        rightclicksystem.journalEntry.UpdateSystemNote(noteform.NoteText, rightclicksystem.System.Name, EDCommander.Current.SyncToEdsm);
                        discoveryform.NoteChanged(this, rightclicksystem);
                    }
                }
            }
        }

        private void dataGridViewStarList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == ColumnInformation.Index)
                {
                    var cell = dataGridViewStarList.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    string text = cell.Value as string;
                    InfoForm frm = new InfoForm();
                    frm.Info(dataGridViewStarList.Columns[e.ColumnIndex].HeaderText, FindForm().Icon, text);
                    frm.Show(FindForm());

                }
                else
                {
                    var he = dataGridViewStarList.Rows[e.RowIndex].Tag as HistoryEntry;
                    ScanDisplayForm.ShowScanOrMarketForm(this.FindForm(), he, checkBoxEDSM.Checked, discoveryform.history);
                }
            }
        }

        private void viewScanDisplayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclicksystem != null)
            {
                ScanDisplayForm.ShowScanOrMarketForm(this.FindForm(), rightclicksystem, checkBoxEDSM.Checked, discoveryform.history);
            }
        }
        #endregion

        #region Events

        private void extButtonDisplayFilters_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();
            displayfilter.AllOrNoneBack = false;
            displayfilter.CloseBoundaryRegion = new Size(32, extButtonDisplayFilters.Height);
            displayfilter.AddAllNone();
            displayfilter.AddStandardOption("stars", "Show All Stars".TxID(EDTx.UserControlSurveyor_showAllStarsToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption("planets", "Show All Planets".TxID(EDTx.UserControlSurveyor_showAllPlanetsToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_ShowMoons);
            displayfilter.AddStandardOption("beltcluster", "Show Belt Clusters".TxID(EDTx.UserControlSurveyor_showBeltClustersToolStripMenuItem), global::EDDiscovery.Icons.Controls.Belt);
            displayfilter.AddStandardOption("valueables", "Show valuable bodies".T(EDTx.UserControlStarList_valueables), global::EDDiscovery.Icons.Controls.Scan_Bodies_HighValue);
            displayfilter.AddStandardOption("jumponium", "Show/Hide presence of Jumponium Materials".T(EDTx.UserControlStarList_JUMP), global::EDDiscovery.Icons.Controls.Scan_FSD);
            displayfilter.AddStandardOption("signals", "Has Signals".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_hasSignalsToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_Signals);
            displayfilter.AddStandardOption("volcanism", "Has volcanism".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_hasVolcanismToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_Volcanism);
            displayfilter.AddStandardOption("values", "Show values".TxID(EDTx.UserControlSurveyor_showValuesToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_HighValue);
            displayfilter.AddStandardOption("shortinfo", "Show more information".TxID(EDTx.UserControlSurveyor_showMoreInformationToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_Landable);
            displayfilter.AddStandardOption("gravity", "Show gravity of landables".TxID(EDTx.UserControlSurveyor_showGravityToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_Landable);
            displayfilter.AddStandardOption("atmos", "Show atmospheres".TxID(EDTx.UserControlSurveyor_showAtmosToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_Landable);
            displayfilter.AddStandardOption("rings", "Has Rings".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_hasRingsToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_RingOnly);
            displayfilter.AddStandardOption("organics", "Show organic scans".T(EDTx.UserControlStarList_scanorganics), global::EDDiscovery.Icons.Controls.Scan_Bodies_NSP);
            displayfilter.AddStandardOption("codex", "Show codex entries".T(EDTx.UserControlStarList_showcodex), global::EDDiscovery.Icons.Controls.Entries);
            displayfilter.ImageSize = new Size(24, 24);
            displayfilter.SaveSettings = (s, o) =>
            {
                displayfilters = s.Split(';');
                PutSetting(dbDisplayFilters, string.Join(";", displayfilters));
                Display(false);
            };

            displayfilter.Show(string.Join(";", displayfilters), extButtonDisplayFilters, this.FindForm());
        }

        private void checkBoxEDSM_CheckedChanged(object sender, EventArgs e)
        {
            PutSetting(dbEDSM, checkBoxEDSM.Checked);
        }

        // Override of visits column sorting, to properly ordering as integers and not as strings - do not work as expected, yet...
        private void dataGridViewStarList_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if ( e.Column.Index == 0 )
                e.SortDataGridViewColumnDate();
            else if (e.Column.Index == 2 || e.Column.Index == 4)
                e.SortDataGridViewColumnNumeric();
        }

        #endregion

        #region Excel

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            Forms.ExportForm frm = new Forms.ExportForm();
            frm.Init(false, new string[] { "Export Current View" });

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                if (frm.SelectedIndex == 0)
                {
                    // 0        1       2           3            4               5           6           7             8              9              10              11              12          
                    string[] colh = { "Time", "System", "Visits", "Other Info", "Scan Value", "Unused", "Body", "Ship", "Description", "Detailed Info", "Travel Dist", "Travel Time", "Travel Jumps", "Travelled MisJumps" };

                    BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid();
                    grd.SetCSVDelimiter(frm.Comma);
                    grd.GetLineStatus += delegate (int r)
                    {
                        if (r < dataGridViewStarList.Rows.Count)
                        {
                            HistoryEntry he = dataGridViewStarList.Rows[r].Tag as HistoryEntry;

                            return (dataGridViewStarList.Rows[r].Visible &&
                                    he.EventTimeUTC.CompareTo(frm.StartTimeUTC) >= 0 &&
                                    he.EventTimeUTC.CompareTo(frm.EndTimeUTC) <= 0) ? BaseUtils.CSVWriteGrid.LineStatus.OK : BaseUtils.CSVWriteGrid.LineStatus.Skip;
                        }
                        else
                            return BaseUtils.CSVWriteGrid.LineStatus.EOF;
                    };

                    grd.GetLine += delegate (int r)
                    {
                        HistoryEntry he = dataGridViewStarList.Rows[r].Tag as HistoryEntry;
                        DataGridViewRow rw = dataGridViewStarList.Rows[r];

                        he.FillInformation(out string EventDescription, out string EventDetailedInfo);

                        return new Object[] {
                            rw.Cells[0].Value,
                            rw.Cells[1].Value,
                            rw.Cells[2].Value ,
                            rw.Cells[3].Value ,
                            rw.Cells[4].Value ,
                            "",
                            he.WhereAmI ,
                            he.ShipInformation != null ? he.ShipInformation.Name : "Unknown",
                            EventDescription,
                            EventDetailedInfo,
                            he.isTravelling ? he.TravelledDistance.ToString("0.0") : "",
                            he.isTravelling ? he.TravelledSeconds.ToString() : "",
                            he.isTravelling ? he.Travelledjumps.ToString() : "",
                            he.isTravelling ? he.TravelledMissingjump.ToString() : "",
                            };
                    };

                    grd.GetHeader += delegate (int c)
                    {
                        return (c < colh.Length && frm.IncludeHeader) ? colh[c] : null;
                    };

                    grd.WriteGrid(frm.Path, frm.AutoOpen, FindForm());
                }
            }
        }

        #endregion

    }
}
