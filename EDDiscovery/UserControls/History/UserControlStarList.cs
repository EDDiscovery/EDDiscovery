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

using EDDiscovery.Controls;
using EliteDangerousCore;
using EliteDangerousCore.EDSM;
using EliteDangerousCore.JournalEvents;
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

        private string DbColumnSave { get { return DBName("StarListControl", "DGVCol"); } }
        private string DbHistorySave { get { return DBName("StarListControlEDUIHistory"); } }
        private string DbEDSM { get { return DBName("StarListControlEDSM"); } }
        private string DbShowJumponium { get { return DBName("StarListControlJumponium"); } }
        private string DbShowClasses { get { return DBName("StarListControlShowClasses"); } }

        private Dictionary<long, DataGridViewRow> rowsbyjournalid = new Dictionary<long, DataGridViewRow>();
        bool loadcomplete;

        Timer searchtimer;
        Timer autoupdateedsm;
        Timer todotimer;
        int autoupdaterowstart = 0;
        int autoupdaterowoffset = 0;
        Queue<Action> todo = new Queue<Action>();

        public UserControlStarList()
        {
            InitializeComponent();
            var corner = dataGridViewStarList.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            checkBoxCursorToTop.Checked = true;


            dataGridViewStarList.MakeDoubleBuffered();
            dataGridViewStarList.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewStarList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx

            dataGridViewStarList.Columns[2].ValueType = typeof(Int32);

            checkBoxEDSM.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbEDSM, false);
            this.checkBoxEDSM.CheckedChanged += new System.EventHandler(this.checkBoxEDSM_CheckedChanged);

            checkBoxBodyClasses.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbShowClasses, true);
            this.checkBoxBodyClasses.CheckedChanged += new System.EventHandler(this.buttonBodyClasses_CheckedChanged);

            checkBoxJumponium.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbShowJumponium, true);
            this.checkBoxJumponium.CheckedChanged += new System.EventHandler(this.buttonJumponium_CheckedChanged);

            discoveryform.OnHistoryChange += HistoryChanged;
            discoveryform.OnNewEntry += AddNewEntry;

            searchtimer = new Timer() { Interval = 500 };
            searchtimer.Tick += Searchtimer_Tick;
            autoupdateedsm = new Timer() { Interval = 2000 };
            autoupdateedsm.Tick += Autoupdateedsm_Tick;
            todotimer = new Timer() { Interval = 20 };
            todotimer.Tick += Todotimer_Tick;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(contextMenuStrip, this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);

            TravelHistoryFilter.InitaliseComboBox(comboBoxHistoryWindow, DbHistorySave, false);
        }

        public override void LoadLayout()
        {
            dataGridViewStarList.RowTemplate.MinimumHeight = Math.Max(26, Font.ScalePixels(26));
            DGVLoadColumnLayout(dataGridViewStarList, DbColumnSave);
        }

        public override void Closing()
        {
            todo.Clear();
            todotimer.Stop();
            searchtimer.Stop();
            DGVSaveColumnLayout(dataGridViewStarList, DbColumnSave);
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

        private void AddNewEntry(HistoryEntry he, HistoryList hl)           // on new entry from discovery system
        {
            if (!loadcomplete)      // if loading, add to queue..
            {
                todo.Enqueue(new Action(() => AddNewEntry(he, hl)));
                return;
            }

            bool scan = he.journalEntry is JournalScan || he.journalEntry is JournalFSSDiscoveryScan;

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
                    if (rowpresent != null)                                   // if its in the list, move to top and set visit count
                    {
                        dataGridViewStarList.Rows.Remove(rowpresent);
                        rowpresent.Cells[2].Value = hl.Visits(he.System.Name);  // update count
                        dataGridViewStarList.Rows.Insert(0, rowpresent);        // move to top..
                    }
                    else
                    {
                        var visitlist = discoveryform.history.Visited.Values.ToList();
                        visitlist.Sort(delegate (HistoryEntry left, HistoryEntry right) { return right.EventTimeUTC.CompareTo(left.EventTimeUTC); });
                        var filter = (TravelHistoryFilter)comboBoxHistoryWindow.SelectedItem ?? TravelHistoryFilter.NoFilter;
                        visitlist = filter.FilterLatestFirst(visitlist);      // and filter

                        if (visitlist.Count > 0 && visitlist[0].System.Name == he.System.Name) // if the filtered result has our system in (which must be the first, since its newest), its inside the filter, add
                        {
                            string filtertext = textBoxFilter.Text;
                            var row = CreateHistoryRow(visitlist[0], filtertext);
                            if (row != null)    // text may have filtered it out
                                dataGridViewStarList.Rows.Insert(0, row);
                        }
                    }

                    if (checkBoxCursorToTop.Checked && dataGridViewStarList.DisplayedRowCount(false) > 0)   // Move focus to new row
                    {
                        //System.Diagnostics.Debug.WriteLine("Auto Sel");
                        dataGridViewStarList.ClearSelection();
                        int rowno = dataGridViewStarList.Rows.GetFirstRow(DataGridViewElementStates.Visible);
                        if (rowno != -1)
                            dataGridViewStarList.SetCurrentAndSelectAllCellsOnRow(rowno);       // its the current cell which needs to be set, moves the row marker as well

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

        private void Todotimer_Tick(object sender, EventArgs e)
        {
            ProcessTodo();
        }

        private void ProcessTodo()
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


        #endregion

        #region Display

        public void Display(bool disablesorting)           // on History change
        {
            autoupdateedsm.Stop();

            loadcomplete = false;
            this.Cursor = Cursors.WaitCursor;

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

            checkBoxJumponium.Enabled = checkBoxBodyClasses.Enabled = buttonExtExcel.Enabled = comboBoxHistoryWindow.Enabled = false;

            dataGridViewStarList.Columns[0].HeaderText = EDDiscoveryForm.EDDConfig.GetTimeTitle();


            var visitlist = discoveryform.history.Visited.Values.ToList();

            // sort is latest first
            visitlist.Sort(delegate (HistoryEntry left, HistoryEntry right) { return right.EventTimeUTC.CompareTo(left.EventTimeUTC); });

            var filter = (TravelHistoryFilter)comboBoxHistoryWindow.SelectedItem ?? TravelHistoryFilter.NoFilter;
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

            todo.Clear();

            string filtertext = textBoxFilter.Text;

            //System.Diagnostics.Stopwatch swtotal = new System.Diagnostics.Stopwatch(); swtotal.Start();

            foreach (var syslistchunk in syslistchunks)
            {
                todo.Enqueue(() =>
                {
                    List<DataGridViewRow> rowstoadd = new List<DataGridViewRow>();

                    foreach (var he in syslistchunk)
                    {
                        var row = CreateHistoryRow(he, filtertext);
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

                loadcomplete = true;
                checkBoxJumponium.Enabled = checkBoxBodyClasses.Enabled = buttonExtExcel.Enabled = comboBoxHistoryWindow.Enabled = true;
                this.Cursor = Cursors.Default;
            });

            todotimer.Start();
        }

        private DataGridViewRow CreateHistoryRow(HistoryEntry he, string search)
        {
            //string debugt = item.Journalid + "  " + item.System.id_edsm + " " + item.System.GetHashCode() + " "; // add on for debug purposes to a field below

            DateTime time = EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(he.EventTimeUTC);
            var node = discoveryform.history.StarScan?.FindSystemSynchronous(he.System, false); // may be null

            string visits = discoveryform.history.Visits(he.System.Name).ToString();
            string info = Infoline(he.System, node);  // lookup node, using star name, no EDSM lookup.

            if (search.HasChars())
            {
                string timestr = time.ToString();
                bool matched = timestr.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                                he.System.Name.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                                visits.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                                info.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) >= 0;
                if (!matched)
                    return null;
            }

            var rw = dataGridViewStarList.RowTemplate.Clone() as DataGridViewRow;
            rw.CreateCells(dataGridViewStarList, time, he.System.Name, visits, info, node?.ScanValue(true).ToString("N0") ?? "0");

            rowsbyjournalid[he.Journalid] = rw;      
            rw.Tag = he;

            rw.Cells[1].Style.ForeColor = (he.System.HasCoordinate) ? Color.Empty : discoveryform.theme.UnknownSystemColor;

            he.journalEntry.FillInformation(out string EventDescription, out string EventDetailedInfo);

            string tip = String.Join(Environment.NewLine, he.EventSummary, EventDescription, EventDetailedInfo);

            rw.Cells[0].ToolTipText = tip;
            rw.Cells[1].ToolTipText = tip;
            rw.Cells[2].ToolTipText = tip;
            rw.Cells[3].ToolTipText = tip;

            return rw;
        }

        string Infoline(ISystem system, StarScan.SystemNode sysnode )
        {
            string infostr = "";

            if (sysnode != null)
            {
                if (sysnode.starnodes != null)
                {
                    string st = sysnode.StarTypesFound();
                    if (st.HasChars())
                        st = " " + st;
                    int stars = sysnode.StarsScanned();
                    infostr = infostr.AppendPrePad(string.Format("{0} Star(s){1}".T(EDTx.UserControlStarList_CS), stars, st) , Environment.NewLine);

                    string extrainfo = "";
                    string prefix = Environment.NewLine;
                    string noprefix = "";
                    string jumponium = "";

                    foreach (StarScan.ScanNode sn in sysnode.Bodies)
                    {
                        string bodyinfo = "";

                        if (sn.ScanData != null && checkBoxBodyClasses.Checked)
                        {
                            JournalScan sc = sn.ScanData;
                            string bodynameshort = sc.BodyName.ReplaceIfStartsWith(system.Name);

                            if (sc.IsStar) // brief notification for special or uncommon celestial bodies, useful to traverse the history and search for that special body you discovered.
                            {
                                // Sagittarius A* is a special body: is the centre of the Milky Way, and the only one which is classified as a Super Massive Black Hole. As far as we know...                                
                                if (sc.StarTypeID == EDStar.SuperMassiveBlackHole)
                                    bodyinfo = bodyinfo.AppendPrePad(string.Format("{0} is a super massive black hole".T(EDTx.UserControlStarList_SMBH), sc.BodyName), prefix);
                                    
                                // black holes
                                if (sc.StarTypeID == EDStar.H)
                                    bodyinfo = bodyinfo.AppendPrePad(string.Format("{0} is a black hole".T(EDTx.UserControlStarList_BH), sc.BodyName), prefix);

                                // neutron stars
                                if (sc.StarTypeID == EDStar.N)
                                    bodyinfo = bodyinfo.AppendPrePad(string.Format("{0} is a neutron star".T(EDTx.UserControlStarList_NS), sc.BodyName), prefix);

                                // white dwarf (D, DA, DAB, DAO, DAZ, DAV, DB, DBZ, DBV, DO, DOV, DQ, DC, DCV, DX)
                                string WhiteDwarf = "White Dwarf";
                                if (sc.StarTypeText.Contains(WhiteDwarf))
                                    bodyinfo = bodyinfo.AppendPrePad(string.Format("{0} is a {1} white dwarf star".T(EDTx.UserControlStarList_WD), sc.BodyName, sc.StarTypeID), prefix);

                                // wolf rayet (W, WN, WNC, WC, WO)
                                string WolfRayet = "Wolf-Rayet";
                                if (sc.StarTypeText.Contains(WolfRayet))
                                    bodyinfo = bodyinfo.AppendPrePad(string.Format("{0} is a {1} wolf-rayet star".T(EDTx.UserControlStarList_WR), sc.BodyName, sc.StarTypeID), prefix);

                                // giants. It should recognize all classes of giants.
                                if (sc.StarTypeText.Contains("Giant"))
                                    bodyinfo = bodyinfo.AppendPrePad(string.Format("{0} is a {1}".T(EDTx.UserControlStarList_OTHER), sc.BodyName, sc.StarTypeText), prefix);

                                // rogue planets - not sure if they really exists, but they are in the journal, so...
                                if (sc.StarTypeID == EDStar.RoguePlanet)
                                    bodyinfo = bodyinfo.AppendPrePad(string.Format("{0} is a rogue planet".T(EDTx.UserControlStarList_RP), sc.BodyName), prefix);
                            }

                            else

                            {
                                // Check if a non-star body is a moon or not. We want it to further refine our brief summary in the visited star list.
                                // To avoid duplicates, we need to apply our filters before on the bodies recognized as a moon, than do the same for the other bodies that do not fulfill that criteria.

                                if (sn.level >= 2 && sn.type == StarScan.ScanNodeType.body)

                                // Tell us that that special body is a moon. After all, it can be quite an outstanding discovery...
                                {
                                    // Earth-like moon
                                    if (sc.PlanetTypeID == EDPlanet.Earthlike_body)
                                        bodyinfo = bodyinfo.AppendPrePad(string.Format("{0} is an earth like moon".T(EDTx.UserControlStarList_ELM), bodynameshort), prefix);

                                    // Terraformable water moon
                                    if (sc.Terraformable == true && sc.PlanetTypeID == EDPlanet.Water_world)
                                        bodyinfo = bodyinfo.AppendPrePad(string.Format("{0} is a terraformable water moon".T(EDTx.UserControlStarList_TWM), bodynameshort), prefix);
                                    // Water moon
                                    if (sc.Terraformable == false && sc.PlanetTypeID == EDPlanet.Water_world)
                                        bodyinfo = bodyinfo.AppendPrePad(string.Format("{0} is a water moon".T(EDTx.UserControlStarList_WM), bodynameshort), prefix);

                                    // Terraformable moon
                                    if (sc.Terraformable == true && sc.PlanetTypeID != EDPlanet.Water_world)
                                        bodyinfo = bodyinfo.AppendPrePad(string.Format("{0} is a terraformable moon".T(EDTx.UserControlStarList_TM), bodynameshort), prefix);

                                    // Ammonia moon
                                    if (sc.PlanetTypeID == EDPlanet.Ammonia_world)
                                        bodyinfo = bodyinfo.AppendPrePad(string.Format("{0} is an ammonia moon".T(EDTx.UserControlStarList_AM), bodynameshort), prefix);
                                }

                                else

                                // Do the same, for all planets
                                {
                                    // Earth Like planet
                                    if (sc.PlanetTypeID == EDPlanet.Earthlike_body)
                                        bodyinfo = bodyinfo.AppendPrePad(string.Format("{0} is an earth like planet".T(EDTx.UserControlStarList_ELP), bodynameshort), prefix);

                                    // Terraformable water world
                                    if (sc.PlanetTypeID == EDPlanet.Water_world && sc.Terraformable == true)
                                        bodyinfo = bodyinfo.AppendPrePad(string.Format("{0} is a terraformable water world".T(EDTx.UserControlStarList_TWW), bodynameshort), prefix);
                                    // Water world
                                    if (sc.PlanetTypeID == EDPlanet.Water_world && sc.Terraformable == false)
                                        bodyinfo = bodyinfo.AppendPrePad(string.Format("{0} is a water world".T(EDTx.UserControlStarList_WW), bodynameshort), prefix);

                                    // Terraformable planet
                                    if (sc.Terraformable == true && sc.PlanetTypeID != EDPlanet.Water_world)
                                        bodyinfo = bodyinfo.AppendPrePad(string.Format("{0} is a terraformable planet".T(EDTx.UserControlStarList_TP), bodynameshort), prefix);

                                    // Ammonia world
                                    if (sc.PlanetTypeID == EDPlanet.Ammonia_world)
                                        bodyinfo = bodyinfo.AppendPrePad(string.Format("{0} is an ammonia world".T(EDTx.UserControlStarList_AW), bodynameshort), prefix);
                                }

                                if (sn.Signals != null)
                                {
                                    bodyinfo = bodyinfo.AppendPrePad(string.Format("{0} has signals".T(EDTx.UserControlStarList_Signals), bodynameshort), prefix);
                                }

                                //Add Distance - Remember no newline
                                if (bodyinfo != "")
                                {
                                    double distance = sc.DistanceFromArrivalLS;
                                    bodyinfo = bodyinfo.AppendPrePad(string.Format(" ({0} ls)".T(EDTx.UserControlStarList_Distance), distance.ToString("n0")), noprefix);
                                    extrainfo = extrainfo.AppendPrePad(bodyinfo, prefix);
                                }
                            }
                        }

                        // Landable bodies with valuable materials, collect into jumponimum
                        if (sn.ScanData != null && sn.ScanData.IsLandable == true && sn.ScanData.HasMaterials && checkBoxJumponium.Checked == true)
                        {
                            int basic = 0;
                            int standard = 0;
                            int premium = 0;

                            foreach (KeyValuePair<string, double> mat in sn.ScanData.Materials)
                            {
                                string usedin = Recipes.UsedInSythesisByFDName(mat.Key);
                                if (usedin.Contains("FSD-Basic"))
                                    basic++;
                                if (usedin.Contains("FSD-Standard"))
                                    standard++;
                                if (usedin.Contains("FSD-Premium"))
                                    premium++;
                            }

                            // string MaterialsBrief = sn.ScanData.DisplayMaterials(4).ToString();
                            //System.Diagnostics.Debug.WriteLine("{0} {1} {2} {3} {4}", sn.fullname , basic, standard, premium, MaterialsBrief);

                            if (basic > 0 || standard > 0 || premium > 0)
                            {
                                int mats = basic + standard + premium;

                                StringBuilder jumpLevel = new StringBuilder();

                                if (basic != 0)
                                    jumpLevel.AppendPrePad(basic + "/" + Recipes.FindSynthesis("FSD", "Basic").Count + " Basic".T(EDTx.UserControlStarList_BFSD), ", ");
                                if (standard != 0)
                                    jumpLevel.AppendPrePad(standard + "/" + Recipes.FindSynthesis("FSD", "Standard").Count + " Standard".T(EDTx.UserControlStarList_SFSD), ", ");
                                if (premium != 0)
                                    jumpLevel.AppendPrePad(premium + "/" + Recipes.FindSynthesis("FSD", "Premium").Count + " Premium".T(EDTx.UserControlStarList_PFSD), ", ");

                                jumponium = jumponium.AppendPrePad(Environment.NewLine + string.Format("{0} has {1} level elements.".T(EDTx.UserControlStarList_LE), sn.ScanData.BodyName ,jumpLevel));
                            }
                        }
                    }

                    int total = sysnode.StarPlanetsScanned();

                    if (total > 0)
                    {
                        int totalwithoutstars = total - stars;

                        if (totalwithoutstars > 0)
                        {
                            infostr = infostr.AppendPrePad(string.Format("{0} Other bodies".T(EDTx.UserControlStarList_OB), totalwithoutstars.ToString()), ", ");
                        }

                        if ( sysnode.FSSTotalBodies.HasValue && total < sysnode.FSSTotalBodies.Value )        // only show if you've not got them all
                        {
                            infostr += ", " + "Total".T(EDTx.UserControlStarList_Total) + " "+ sysnode.FSSTotalBodies.Value.ToString();
                        }

                        infostr = infostr.AppendPrePad(extrainfo, prefix);                                                
                    }
                    else
                    {   // we need this to allow the panel to scan also through systems which has only stars
                        infostr = infostr.AppendPrePad(extrainfo, prefix);
                    }

                    if (jumponium.HasChars() )
                    {
                        infostr = infostr.AppendPrePad(Environment.NewLine + Environment.NewLine + "This system has materials for FSD boost: ".T(EDTx.UserControlStarList_FSD));
                        infostr = infostr.AppendPrePad(jumponium);
                    }
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

        public void GotoPosByJID(long jid)      // uccursor requirement
        {
            int rowno = DataGridViewControlHelpersStaticFunc.FindGridPosByID(rowsbyjournalid, jid, true);
            if (rowno >= 0)
            {
                dataGridViewStarList.SetCurrentAndSelectAllCellsOnRow(rowno);
                dataGridViewStarList.Rows[rowno].Selected = true;
                FireChangeSelection();
            }
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
                    
                    if (!discoveryform.history.StarScan.HasWebLookupOccurred(he.System))        // if we have done a lookup, we can skip this one quickly
                    {
                        //System.Diagnostics.Debug.WriteLine("EDSM Update row" + row);
                        CheckEDSM(rw);
                        break;
                    }
                    else
                    {
                        //System.Diagnostics.Debug.WriteLine("Skip row as already checked " + row);
                    }
                }
            }
        }


        private void comboBoxHistoryWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(DbHistorySave, comboBoxHistoryWindow.Text);
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

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            searchtimer.Stop();
            searchtimer.Start();
        }

        private void Searchtimer_Tick(object sender, EventArgs e)
        {
            searchtimer.Stop();
            this.Cursor = Cursors.WaitCursor;
            Display(false);
            this.Cursor = Cursors.Default;
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
            if (!discoveryform.Map.Is3DMapsRunning)            // if not running, click the 3dmap button
                discoveryform.Open3DMap(GetCurrentHistoryEntry);

            if (discoveryform.Map.Is3DMapsRunning)             // double check here! for paranoia.
            {
                if (discoveryform.Map.MoveToSystem(rightclicksystem.System))
                    discoveryform.Map.Show();
            }
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
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "System could not be found - has not been synched or EDSM is unavailable".T(EDTx.UserControlStarList_NoEDSM));

            this.Cursor = Cursors.Default;
        }

        private void setNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclicksystem != null)
            {
                using (Forms.SetNoteForm noteform = new Forms.SetNoteForm(rightclicksystem, discoveryform))
                {
                    if (noteform.ShowDialog(FindForm()) == DialogResult.OK)
                    {
                        rightclicksystem.SetJournalSystemNoteText(noteform.NoteText, true, EDCommander.Current.SyncToEdsm);

                        discoveryform.NoteChanged(this, rightclicksystem, true);
                    }
                }
            }
        }

        private void dataGridViewStarList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var he = dataGridViewStarList.Rows[e.RowIndex].Tag as HistoryEntry;
                ScanDisplayForm.ShowScanOrMarketForm(this.FindForm(), he, checkBoxEDSM.Checked, discoveryform.history);
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

            private void checkBoxEDSM_CheckedChanged(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbEDSM, checkBoxEDSM.Checked);
        }

        private void buttonBodyClasses_CheckedChanged(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbShowClasses, checkBoxBodyClasses.Checked);
            Display(false);
        }

        private void buttonJumponium_CheckedChanged(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbShowJumponium, checkBoxJumponium.Checked);
            Display(false);
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
            frm.Init(new string[] { "Export Current View" });

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

                        he.journalEntry.FillInformation(out string EventDescription, out string EventDetailedInfo);

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