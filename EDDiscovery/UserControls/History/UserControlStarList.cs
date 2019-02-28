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
using EliteDangerousCore.JournalEvents;

namespace EDDiscovery.UserControls
{
    public partial class UserControlStarList : UserControlCommonBase, IHistoryCursor
    {
        #region Public IF

        public HistoryEntry GetCurrentHistoryEntry { get { return dataGridViewStarList.CurrentCell != null ?
                    (dataGridViewStarList.Rows[dataGridViewStarList.CurrentCell.RowIndex].Tag as List<HistoryEntry>)[0] : null; } }

        #endregion

        #region Events

        // implement IHistoryCursor fields
        public event ChangedSelectionHEHandler OnTravelSelectionChanged;   // as above, different format, for certain older controls

        #endregion

        #region Init
        private class StarHistoryColumns
        {
            public const int LastVisit = 0;
            public const int StarName = 1;
            public const int NoVisits = 2;
            public const int OtherInformation = 3;
        }

        private const int DefaultRowHeight = 26;
        private string DbColumnSave { get { return DBName("StarListControl" ,  "DGVCol"); } }
        private string DbHistorySave { get { return DBName("StarListControlEDUIHistory" ); } }
        private string DbAutoTop { get { return DBName("StarListControlAutoTop" ); } }
        private string DbEDSM { get { return DBName("StarListControlEDSM" ); } }
        private string DbShowJumponium { get { return DBName("StarListControlJumponium" ); } }
        private string DbShowClasses { get { return DBName("StarListControlShowClasses" ); } }        

        private Dictionary<string, List<HistoryEntry>> systemsentered = new Dictionary<string, List<HistoryEntry>>();
        private Dictionary<long, DataGridViewRow> rowsbyjournalid = new Dictionary<long, DataGridViewRow>();
        private HistoryList current_historylist;

        Timer searchtimer;
        Timer autoupdateedsm;
        Timer todotimer;
        int autoupdaterowstart = 0;
        int autoupdaterowoffset = 0;
        Queue<Action> todo = new Queue<Action>();
        bool loadcomplete = false;

        public UserControlStarList()
        {
            InitializeComponent();
            var corner = dataGridViewStarList.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            checkBoxCursorToTop.Checked = SQLiteConnectionUser.GetSettingBool(DbAutoTop, true);

         
            dataGridViewStarList.MakeDoubleBuffered();
            dataGridViewStarList.RowTemplate.Height = DefaultRowHeight;
            dataGridViewStarList.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewStarList.RowTemplate.Height = 26;
            dataGridViewStarList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx

            dataGridViewStarList.Columns[2].ValueType = typeof(Int32);            

            checkBoxEDSM.Checked = SQLiteDBClass.GetSettingBool(DbEDSM, false);
            this.checkBoxEDSM.CheckedChanged += new System.EventHandler(this.checkBoxEDSM_CheckedChanged);

            checkBoxBodyClasses.Checked = SQLiteConnectionUser.GetSettingBool(DbShowClasses, true);
            this.checkBoxBodyClasses.CheckedChanged += new System.EventHandler(this.buttonBodyClasses_CheckedChanged);

            checkBoxJumponium.Checked = SQLiteConnectionUser.GetSettingBool(DbShowJumponium, true);
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

            TravelHistoryFilter.InitaliseComboBox(comboBoxHistoryWindow, DbHistorySave);
        }

        public override void LoadLayout()
        {
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
            SQLiteConnectionUser.PutSettingBool(DbAutoTop, checkBoxCursorToTop.Checked);
        }

        #endregion

        public override void InitialDisplay()
        {
            HistoryChanged(discoveryform.history);
        }

        public void HistoryChanged(HistoryList hl)           // on History change
        {
            HistoryChanged(hl, false);
        }

        public void HistoryChanged(HistoryList hl, bool disablesorting)           // on History change
        {
            if (hl == null)     // just for safety
                return;

            loadcomplete = false;

            autoupdateedsm.Stop();

            Tuple<long, int> pos = CurrentGridPosByJID();
            string filtertext = textBoxFilter.Text;

            current_historylist = hl;

            SortOrder sortorder = dataGridViewStarList.SortOrder;
            int sortcol = dataGridViewStarList.SortedColumn?.Index ?? -1;
            if (sortcol >= 0 && disablesorting)
            {
                dataGridViewStarList.Columns[sortcol].HeaderCell.SortGlyphDirection = SortOrder.None;
                sortcol = -1;
            }

            System.Diagnostics.Trace.WriteLine(BaseUtils.AppTicks.TickCountLap(this,true) + " SL " + displaynumber + " Load start");

            rowsbyjournalid.Clear();
            systemsentered.Clear();
            dataGridViewStarList.Rows.Clear();

            dataGridViewStarList.Columns[0].HeaderText = EDDiscoveryForm.EDDConfig.DisplayUTC ? "Game Time".Tx() : "Time".Tx();

            var filter = (TravelHistoryFilter)comboBoxHistoryWindow.SelectedItem ?? TravelHistoryFilter.NoFilter;

            List<HistoryEntry> result = filter.Filter(hl);      // last entry, first in list
            result = HistoryList.FilterHLByTravel(result);      // keep only travel entries (location after death, FSD jumps)

            foreach (HistoryEntry he in result)        // last first..
            {
                if (!systemsentered.ContainsKey(he.System.Name))
                    systemsentered[he.System.Name] = new List<HistoryEntry>();

                systemsentered[he.System.Name].Add(he);     // first entry is newest jump to, second is next last, etc
            }

            List<List<HistoryEntry>> syslists = systemsentered.Values.ToList();
            List<List<HistoryEntry>[]> syslistchunks = new List<List<HistoryEntry>[]>();

            for (int i = 0; i < syslists.Count; i += 1000)
            {
                int totake = Math.Min(1000, syslists.Count - i);
                List<HistoryEntry>[] syslistchunk = new List<HistoryEntry>[totake];
                syslists.CopyTo(i, syslistchunk, 0, totake);
                syslistchunks.Add(syslistchunk);
            }

            todo.Clear();

            foreach (var syslistchunk in syslistchunks)
            {
                todo.Enqueue(() =>
                {
                    dataViewScrollerPanel.Suspend();
                    foreach (var syslist in syslistchunk)
                    {
                        var row = CreateHistoryRow(syslist, filtertext);
                        if (row != null)
                            dataGridViewStarList.Rows.Add(row);
                    }

                    dataViewScrollerPanel.Resume();
                });
            }

            todo.Enqueue(() =>
            {
                dataGridViewStarList.FilterGridView(filtertext);

                int rowno = FindGridPosByJID(pos.Item1, true);     // find row.. must be visible..  -1 if not found/not visible

                if (rowno >= 0)
                {
                    dataGridViewStarList.CurrentCell = dataGridViewStarList.Rows[rowno].Cells[pos.Item2];       // its the current cell which needs to be set, moves the row marker as well currentGridRow = (rowno!=-1) ? 
                }
                else if (dataGridViewStarList.Rows.GetRowCount(DataGridViewElementStates.Visible) > 0)
                {
                    rowno = dataGridViewStarList.Rows.GetFirstRow(DataGridViewElementStates.Visible);
                    dataGridViewStarList.CurrentCell = dataGridViewStarList.Rows[rowno].Cells[StarHistoryColumns.StarName];
                }
                else
                    rowno = -1;

                System.Diagnostics.Trace.WriteLine(BaseUtils.AppTicks.TickCountLap(this) + " SL " + displaynumber + " Load Finish");

                if (sortcol >= 0)
                {
                    dataGridViewStarList.Sort(dataGridViewStarList.Columns[sortcol], (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                    dataGridViewStarList.Columns[sortcol].HeaderCell.SortGlyphDirection = sortorder;
                }

                //System.Diagnostics.Debug.WriteLine("Fire HC");

                autoupdaterowoffset = autoupdaterowstart = 0;
                autoupdateedsm.Start();

                FireChangeSelection();      // and since we repainted, we should fire selection, as we in effect may have selected a new one

                loadcomplete = true;
            });

            todotimer.Start();
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

        private void AddNewEntry(HistoryEntry he, HistoryList hl)           // on new entry from discovery system
        {
            if (!loadcomplete)
            {
                todo.Enqueue(new Action(() => AddNewEntry(he, hl)));
                return;
            }

            if ( he.IsFSDJump )     // FSD jumps mean move system.. so
                HistoryChanged(hl); // just recalc all..

            if (checkBoxCursorToTop.Checked && dataGridViewStarList.DisplayedRowCount(false) > 0)   // Move focus to new row
            {
                //System.Diagnostics.Debug.WriteLine("Auto Sel");
                dataGridViewStarList.ClearSelection();
                int rowno = dataGridViewStarList.Rows.GetFirstRow(DataGridViewElementStates.Visible);
                if ( rowno != -1 )
                    dataGridViewStarList.CurrentCell = dataGridViewStarList.Rows[rowno].Cells[1];       // its the current cell which needs to be set, moves the row marker as well

                FireChangeSelection();
            }
        }

        private DataGridViewRow CreateHistoryRow(List<HistoryEntry> syslist, string search)
        {
            //string debugt = item.Journalid + "  " + item.System.id_edsm + " " + item.System.GetHashCode() + " "; // add on for debug purposes to a field below

            HistoryEntry he = syslist[0];

            DateTime time = EDDiscoveryForm.EDDConfig.DisplayUTC ? he.EventTimeUTC : he.EventTimeLocal;
            string visits = $"{syslist.Count:N0}";
            string info = Infoline(syslist);

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
            rw.CreateCells(dataGridViewStarList, time, he.System.Name, visits, info);

            foreach ( HistoryEntry hel in syslist )
                rowsbyjournalid[hel.Journalid] = rw;      // all JIDs in this array, to this row

            rw.Tag = syslist;

            rw.DefaultCellStyle.ForeColor = (he.System.HasCoordinate || he.EntryType != JournalTypeEnum.FSDJump) ? discoveryform.theme.VisitedSystemColor : discoveryform.theme.NonVisitedSystemColor;

            he.journalEntry.FillInformation(out string EventDescription, out string EventDetailedInfo);

            string tip = String.Join(Environment.NewLine, he.EventSummary, EventDescription, EventDetailedInfo);

            rw.Cells[0].ToolTipText = tip;
            rw.Cells[1].ToolTipText = tip;
            rw.Cells[2].ToolTipText = tip;
            rw.Cells[3].ToolTipText = tip;

            return rw;
        }

        string Infoline(List<HistoryEntry> syslist)
        {
            string infostr = "";
            string jumponium = "";
            bool hasMaterials = false;

            if (syslist.Count > 1)
                infostr = string.Format("First visit {0}".Tx(this,"FV"),syslist.Last().EventTimeLocal.ToShortDateString());

            HistoryEntry he = syslist[0];
            StarScan.SystemNode node = discoveryform.history.starscan?.FindSystem(he.System,false);

            #region information

            if (node != null)
            {
                if (node.starnodes != null)
                {
                    infostr = infostr.AppendPrePad(string.Format("{0} Star(s)".Tx(this, "CS"), node.starnodes.Count) , Environment.NewLine);
                    string extrainfo = "";
                    string prefix = Environment.NewLine;
                    int total = 0;

                    foreach (StarScan.ScanNode sn in node.Bodies)
                    {
                        total++;
                        if (sn.ScanData != null && checkBoxBodyClasses.Checked)
                        {
                            JournalScan sc = sn.ScanData;

                            if (sc.IsStar) // brief notification for special or uncommon celestial bodies, useful to traverse the history and search for that special body you discovered.
                            {
                                // Sagittarius A* is a special body: is the centre of the Milky Way, and the only one which is classified as a Super Massive Black Hole. As far as we know...                                
                                if (sc.StarTypeID == EDStar.SuperMassiveBlackHole)
                                    extrainfo = extrainfo.AppendPrePad(string.Format("{0} is a super massive black hole".Tx(this,"SMBH"), sc.BodyName) , prefix);

                                // black holes
                                if (sc.StarTypeID == EDStar.H)
                                    extrainfo = extrainfo.AppendPrePad(string.Format("{0} is a black hole".Tx(this, "BH"), sc.BodyName), prefix);

                                // neutron stars
                                if (sc.StarTypeID == EDStar.N)
                                    extrainfo = extrainfo.AppendPrePad(string.Format("{0} is a neutron star".Tx(this, "NS"), sc.BodyName), prefix);

                                // white dwarf (D, DA, DAB, DAO, DAZ, DAV, DB, DBZ, DBV, DO, DOV, DQ, DC, DCV, DX)
                                string WhiteDwarf = "White Dwarf";
                                if (sc.StarTypeText.Contains(WhiteDwarf))
                                    extrainfo = extrainfo.AppendPrePad(string.Format("{0} is a {1} white dwarf star".Tx(this, "WD"), sc.BodyName, sc.StarTypeID), prefix);

                                // wolf rayet (W, WN, WNC, WC, WO)
                                string WolfRayet = "Wolf-Rayet";
                                if (sc.StarTypeText.Contains(WolfRayet))
                                    extrainfo = extrainfo.AppendPrePad(string.Format("{0} is a {1} wolf-rayet star".Tx(this, "WR"), sc.BodyName, sc.StarTypeID), prefix);

                                // giants. It should recognize all classes of giants.
                                if (sc.StarTypeText.Contains("Giant"))
                                    extrainfo = extrainfo.AppendPrePad(string.Format("{0} is a {1}".Tx(this, "OTHER"), sc.BodyName, sc.StarTypeText), prefix);

                                // rogue planets - not sure if they really exists, but they are in the journal, so...
                                if (sc.StarTypeID == EDStar.RoguePlanet)
                                    extrainfo = extrainfo.AppendPrePad(string.Format("{0} is a rogue planet".Tx(this, "RP"), sc.BodyName), prefix);
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
                                        extrainfo = extrainfo.AppendPrePad(string.Format("{0} is an earth like moon".Tx(this, "ELM"), sc.BodyName), prefix);

                                    // Terraformable water moon
                                    if (sc.Terraformable == true && sc.PlanetTypeID == EDPlanet.Water_world)
                                        extrainfo = extrainfo.AppendPrePad(string.Format("{0} is a terraformable water moon".Tx(this, "TWM"), sc.BodyName), prefix);
                                    // Water moon
                                    if (sc.Terraformable == false && sc.PlanetTypeID == EDPlanet.Water_world)
                                        extrainfo = extrainfo.AppendPrePad(string.Format("{0} is a water moon".Tx(this, "WM"), sc.BodyName), prefix);

                                    // Terraformable moon
                                    if (sc.Terraformable == true && sc.PlanetTypeID != EDPlanet.Water_world)
                                        extrainfo = extrainfo.AppendPrePad(string.Format("{0} is a terraformable moon".Tx(this, "TM"), sc.BodyName), prefix);

                                    // Ammonia moon
                                    if (sc.PlanetTypeID == EDPlanet.Ammonia_world)
                                        extrainfo = extrainfo.AppendPrePad(string.Format("{0} is an ammonia moon".Tx(this, "AM"), sc.BodyName), prefix);
                                }

                                else

                                // Do the same, for all planets
                                {
                                    // Earth Like planet
                                    if (sc.PlanetTypeID == EDPlanet.Earthlike_body)
                                        extrainfo = extrainfo.AppendPrePad(string.Format("{0} is an earth like planet".Tx(this, "ELP"), sc.BodyName), prefix);

                                    // Terraformable water world
                                    if (sc.PlanetTypeID == EDPlanet.Water_world && sc.Terraformable == true)
                                        extrainfo = extrainfo.AppendPrePad(string.Format("{0} is a terraformable water world".Tx(this, "TWW"), sc.BodyName), prefix);
                                    // Water world
                                    if (sc.PlanetTypeID == EDPlanet.Water_world && sc.Terraformable == false)
                                        extrainfo = extrainfo.AppendPrePad(string.Format("{0} is a water world".Tx(this, "WW"), sc.BodyName), prefix);

                                    // Terraformable planet
                                    if (sc.Terraformable == true && sc.PlanetTypeID != EDPlanet.Water_world)
                                        extrainfo = extrainfo.AppendPrePad(string.Format("{0} is a terraformable planet".Tx(this, "TP"), sc.BodyName), prefix);

                                    // Ammonia world
                                    if (sc.PlanetTypeID == EDPlanet.Ammonia_world)
                                        extrainfo = extrainfo.AppendPrePad(string.Format("{0} is an ammonia world".Tx(this, "AW"), sc.BodyName), prefix);
                                }
                            }
                        }

                        // Landable bodies with valuable materials
                        if (sn.ScanData != null && sn.ScanData.IsLandable == true && sn.ScanData.HasMaterials && checkBoxJumponium.Checked == true)
                        {
                            hasMaterials = true;

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
                                    jumpLevel.AppendPrePad(basic + "/" + Recipes.FindSynthesis("FSD", "Basic").Count + " Basic".Tx(this,"BFSD"), ", ");
                                if (standard != 0)
                                    jumpLevel.AppendPrePad(standard + "/" + Recipes.FindSynthesis("FSD", "Standard").Count + " Standard".Tx(this,"SFSD"), ", ");
                                if (premium != 0)
                                    jumpLevel.AppendPrePad(premium + "/" + Recipes.FindSynthesis("FSD", "Premium").Count + " Premium".Tx(this,"PFSD"), ", ");

                                jumponium = jumponium.AppendPrePad(Environment.NewLine + string.Format("{0} has {1} level elements.".Tx(this, "LE"), sn.ScanData.BodyName ,jumpLevel));
                            }
                        }
                    }

                    total -= node.starnodes.Count;
                    if (total > 0)
                    {   // tell us that a system has other bodies, and how much, beside stars
                        infostr = infostr.AppendPrePad(string.Format("{0} Other bodies".Tx(this,"OB"), total.ToStringInvariant()), ", ");
                        infostr = infostr.AppendPrePad(extrainfo, prefix);                                                
                    }
                    else
                    {   // we need this to allow the panel to scan also through systems which has only stars
                        infostr = infostr.AppendPrePad(extrainfo, prefix);
                    }
                    if (hasMaterials == true && checkBoxJumponium.Checked == true)
                    {
                        infostr = infostr.AppendPrePad(Environment.NewLine + Environment.NewLine + "This system has materials for FSD boost: ".Tx(this,"FSD"));
                        infostr = infostr.AppendPrePad(jumponium);
                    }
                }
            }

            return infostr;
        }

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
            long jid = (dataGridViewStarList.CurrentCell != null) ? (dataGridViewStarList.Rows[dataGridViewStarList.CurrentCell.RowIndex].Tag as List<HistoryEntry>)[0].Journalid : -1;
            int cellno = (dataGridViewStarList.CurrentCell != null) ? dataGridViewStarList.CurrentCell.ColumnIndex : 0;
            return new Tuple<long, int>(jid, cellno);
        }

        int FindGridPosByJID(long jid, bool checkvisible)
        {
            if (rowsbyjournalid.ContainsKey(jid) && (!checkvisible || rowsbyjournalid[jid].Visible))
                return rowsbyjournalid[jid].Index;
            else
                return -1;
        }

        public void GotoPosByJID(long jid)      // uccursor requirement
        {
            int rowno = FindGridPosByJID(jid, true);
            if (rowno >= 0)
            {
                dataGridViewStarList.CurrentCell = dataGridViewStarList.Rows[rowno].Cells[StarHistoryColumns.StarName];
                dataGridViewStarList.Rows[rowno].Selected = true;
                FireChangeSelection();
            }
        }

        public void CheckEDSM()
        {
            if (dataGridViewStarList.CurrentCell != null)
                CheckEDSM(dataGridViewStarList.CurrentRow);
        }

        public void CheckEDSM(DataGridViewRow row)
        {
            List<HistoryEntry> syslist = row.Tag as List<HistoryEntry>;

            discoveryform.history.starscan?.FindSystem(syslist[0].System, true);  // try an EDSM lookup
            row.Cells[StarHistoryColumns.OtherInformation].Value = Infoline(syslist);
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

                    List<HistoryEntry> syslist = rw.Tag as List<HistoryEntry>;

                    if (!discoveryform.history.starscan.HasWebLookupOccurred(syslist[0].System))        // if we have done a lookup, we can skip this one quickly
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
            SQLiteDBClass.PutSettingString(DbHistorySave, comboBoxHistoryWindow.Text);

            if (current_historylist != null)
                HistoryChanged(current_historylist);        // fires lots of events
        }

        public void FireChangeSelection() // uccursor requirement
        {
            if (dataGridViewStarList.CurrentCell != null)
            {
                int row = dataGridViewStarList.CurrentCell.RowIndex;
                //System.Diagnostics.Debug.WriteLine("Fire Change Sel row" + row);
                OnTravelSelectionChanged?.Invoke((dataGridViewStarList.Rows[row].Tag as List<HistoryEntry>)[0], current_historylist, true);
            }
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
            HistoryChanged(current_historylist, false);
            this.Cursor = Cursors.Default;
        }


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

            if (dataGridViewStarList.SelectedCells.Count < 2 || dataGridViewStarList.SelectedRows.Count == 1)      // if single row completely selected, or 1 cell or less..
            {
                DataGridView.HitTestInfo hti = dataGridViewStarList.HitTest(e.X, e.Y);
                if (hti.Type == DataGridViewHitTestType.Cell)
                {
                    dataGridViewStarList.ClearSelection();                // select row under cursor.
                    dataGridViewStarList.Rows[hti.RowIndex].Selected = true;

                    if (e.Button == MouseButtons.Right)         // right click on travel map, get in before the context menu
                    {
                        rightclickrow = hti.RowIndex;
                        rightclicksystem = (dataGridViewStarList.Rows[hti.RowIndex].Tag as List<HistoryEntry>)[0];
                    }
                    if (e.Button == MouseButtons.Left)         // right click on travel map, get in before the context menu
                    {
                        leftclickrow = hti.RowIndex;
                        leftclicksystem = (dataGridViewStarList.Rows[hti.RowIndex].Tag as List<HistoryEntry>)[0];
                    }
                }
            }
        }

        #endregion

        #region TravelHistoryRightClick

        private void historyContextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (dataGridViewStarList.SelectedCells.Count == 0)      // need something selected  stops context menu opening on nothing..
                e.Cancel = true;

            HistoryEntry prev = discoveryform.history.PreviousFrom(rightclicksystem, true);    // null can be passed in safely

            mapGotoStartoolStripMenuItem.Enabled = (rightclicksystem != null && rightclicksystem.System.HasCoordinate);
            viewOnEDSMToolStripMenuItem.Enabled = (rightclicksystem != null);
            removeSortingOfColumnsToolStripMenuItem.Enabled = dataGridViewStarList.SortedColumn != null;

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
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "System could not be found - has not been synched or EDSM is unavailable".Tx(this,"NoEDSM"));

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

        #endregion

        #region Events

        private void checkBoxEDSM_CheckedChanged(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbEDSM, checkBoxEDSM.Checked);
        }

        private void buttonBodyClasses_CheckedChanged(object sender, EventArgs e)
        {
            SQLiteConnectionUser.PutSettingBool(DbShowClasses, checkBoxBodyClasses.Checked);
            HistoryChanged(current_historylist);
        }

        private void buttonJumponium_CheckedChanged(object sender, EventArgs e)
        {
            SQLiteConnectionUser.PutSettingBool(DbShowJumponium, checkBoxJumponium.Checked);
            HistoryChanged(current_historylist);
        }

        // Override of visits column sorting, to properly ordering as integers and not as strings - do not work as expected, yet...
        private void dataGridViewStarList_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if ( e.Column.Index == 0 )
                e.SortDataGridViewColumnDate();
            else if (e.Column.Index == 2)
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
                    string[] colh = { "Time", "System", "Visits", "Other Info", "Visit List", "Body", "Ship", "Description", "Detailed Info", "Travel Dist", "Travel Time", "Travel Jumps", "Travelled MisJumps" };

                    BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid();
                    grd.SetCSVDelimiter(frm.Comma);
                    grd.GetLineStatus += delegate (int r)
                    {
                        if (r < dataGridViewStarList.Rows.Count)
                        {
                            List<HistoryEntry> syslist = dataGridViewStarList.Rows[r].Tag as List<HistoryEntry>;
                            HistoryEntry he = syslist[0];

                            return (dataGridViewStarList.Rows[r].Visible &&
                                    he.EventTimeLocal.CompareTo(frm.StartTime) >= 0 &&
                                    he.EventTimeLocal.CompareTo(frm.EndTime) <= 0) ? BaseUtils.CSVWriteGrid.LineStatus.OK : BaseUtils.CSVWriteGrid.LineStatus.Skip;
                        }
                        else
                            return BaseUtils.CSVWriteGrid.LineStatus.EOF;
                    };
                    grd.GetLine += delegate (int r)
                    {
                        List<HistoryEntry> syslist = dataGridViewStarList.Rows[r].Tag as List<HistoryEntry>;
                        HistoryEntry he = syslist[0];
                        DataGridViewRow rw = dataGridViewStarList.Rows[r];

                        string tlist = "";
                        if (syslist.Count > 1)
                        {
                            for (int i = 1; i < syslist.Count; i++)
                                tlist = tlist.AppendPrePad(syslist[i].EventTimeLocal.ToShortDateString() + " " + syslist[i].EventTimeLocal.ToShortTimeString(), ", ");
                        }

                        he.journalEntry.FillInformation(out string EventDescription, out string EventDetailedInfo);

                        return new Object[] {
                            rw.Cells[0].Value,
                            rw.Cells[1].Value,
                            rw.Cells[2].Value ,
                            rw.Cells[3].Value ,
                            tlist,
                            he.WhereAmI ,
                            he.ShipInformation != null ? he.ShipInformation.Name : "Unknown",
                            EventDescription,
                            EventDetailedInfo,
                            he.isTravelling ? he.TravelledDistance.ToString("0.0") : "",
                            he.isTravelling ? he.TravelledSeconds.ToString() : "",
                            he.isTravelling ? he.Travelledjumps.ToStringInvariant() : "",
                            he.isTravelling ? he.TravelledMissingjump.ToStringInvariant() : "",
                            };
                    };

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
        }

        #endregion

    }
}