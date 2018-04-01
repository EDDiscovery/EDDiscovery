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
using static EDDiscovery.UserControls.Recipes;

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
        public event ChangedSelectionHandler OnChangedSelection;   // After a change of selection by the user, or after a OnHistoryChanged, or after a sort.
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
        private string DbColumnSave { get { return "StarListControl" + ((displaynumber > 0) ? displaynumber.ToString() : "") + "DGVCol"; } }
        private string DbHistorySave { get { return "StarListControlEDUIHistory" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbAutoTop { get { return "StarListControlAutoTop" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbEDSM { get { return "StarListControlEDSM" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbShowJumponium { get { return "StarListControlJumponium" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbShowClasses { get { return "StarListControlShowClasses" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }        

        private Dictionary<string, List<HistoryEntry>> systemsentered = new Dictionary<string, List<HistoryEntry>>();
        private Dictionary<long, DataGridViewRow> rowsbyjournalid = new Dictionary<long, DataGridViewRow>();
        private HistoryList current_historylist;
        
        public UserControlStarList()
        {
            InitializeComponent();
            var corner = dataGridViewStarList.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            TravelHistoryFilter.InitaliseComboBox(comboBoxHistoryWindow, DbHistorySave);

            checkBoxMoveToTop.Checked = SQLiteConnectionUser.GetSettingBool(DbAutoTop, true);

         
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
        }
        
                
        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridViewStarList, DbColumnSave);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewStarList, DbColumnSave);
            discoveryform.OnHistoryChange -= HistoryChanged;
            discoveryform.OnNewEntry -= AddNewEntry;
            SQLiteConnectionUser.PutSettingBool(DbAutoTop, checkBoxMoveToTop.Checked);
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

            Tuple<long, int> pos = CurrentGridPosByJID();

            current_historylist = hl;

            SortOrder sortorder = dataGridViewStarList.SortOrder;
            int sortcol = dataGridViewStarList.SortedColumn?.Index ?? -1;
            if (sortcol >= 0 && disablesorting)
            {
                dataGridViewStarList.Columns[sortcol].HeaderCell.SortGlyphDirection = SortOrder.None;
                sortcol = -1;
            }

            rowsbyjournalid.Clear();
            systemsentered.Clear();
            dataGridViewStarList.Rows.Clear();

            var filter = (TravelHistoryFilter)comboBoxHistoryWindow.SelectedItem ?? TravelHistoryFilter.NoFilter;

            List<HistoryEntry> result = filter.Filter(hl);      // last entry, first in list
            result = HistoryList.FilterHLByTravel(result);      // keep only travel entries (location after death, FSD jumps)


            foreach (HistoryEntry he in result)        // last first..
            {
                if (!systemsentered.ContainsKey(he.System.Name))
                    systemsentered[he.System.Name] = new List<HistoryEntry>();

                systemsentered[he.System.Name].Add(he);     // first entry is newest jump to, second is next last, etc
            }

            foreach( List<HistoryEntry> syslist in systemsentered.Values ) // will be in order of entry..
            {
                AddNewHistoryRow(false, syslist);      // add, with the properties of the first (latest) entry, giving the number of entries..
            }

            StaticFilters.FilterGridView(dataGridViewStarList, textBoxFilter.Text);

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

            dataGridViewStarList.Columns[0].HeaderText = EDDiscoveryForm.EDDConfig.DisplayUTC ? "Game Time" : "Time";

            if (sortcol >= 0)
            {
                dataGridViewStarList.Sort(dataGridViewStarList.Columns[sortcol], (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                dataGridViewStarList.Columns[sortcol].HeaderCell.SortGlyphDirection = sortorder;
            }

            //System.Diagnostics.Debug.WriteLine("Fire HC");

            FireChangeSelection();      // and since we repainted, we should fire selection, as we in effect may have selected a new one
        }

        private void AddNewEntry(HistoryEntry he, HistoryList hl)           // on new entry from discovery system
        {
            if ( he.IsFSDJump )     // FSD jumps mean move system.. so
                HistoryChanged(hl); // just recalc all..

            if (checkBoxMoveToTop.Checked && dataGridViewStarList.DisplayedRowCount(false) > 0)   // Move focus to new row
            {
                //System.Diagnostics.Debug.WriteLine("Auto Sel");
                dataGridViewStarList.ClearSelection();
                int rowno = dataGridViewStarList.Rows.GetFirstRow(DataGridViewElementStates.Visible);
                if ( rowno != -1 )
                    dataGridViewStarList.CurrentCell = dataGridViewStarList.Rows[rowno].Cells[1];       // its the current cell which needs to be set, moves the row marker as well

                FireChangeSelection();
            }
        }

        private void AddNewHistoryRow(bool insert, List<HistoryEntry> syslist)            // second part of add history row, adds item to view.
        {
            //string debugt = item.Journalid + "  " + item.System.id_edsm + " " + item.System.GetHashCode() + " "; // add on for debug purposes to a field below

            HistoryEntry he = syslist[0];
            
            int visits = syslist.Count;                        
            object[] rowobj = { EDDiscoveryForm.EDDConfig.DisplayUTC ? he.EventTimeUTC : he.EventTimeLocal, he.System.Name, $"{visits:N0}", Infoline(syslist) };

            int rownr;
            if (insert)
            {
                dataGridViewStarList.Rows.Insert(0, rowobj);
                rownr = 0;
            }
            else
            {
                dataGridViewStarList.Rows.Add(rowobj);
                rownr = dataGridViewStarList.Rows.Count - 1;
            }

            foreach( HistoryEntry hel in syslist )
                rowsbyjournalid[hel.Journalid] = dataGridViewStarList.Rows[rownr];      // all JIDs in this array, to this row

            dataGridViewStarList.Rows[rownr].Tag = syslist;

            dataGridViewStarList.Rows[rownr].DefaultCellStyle.ForeColor = (he.System.HasCoordinate || he.EntryType != JournalTypeEnum.FSDJump) ? discoveryform.theme.VisitedSystemColor : discoveryform.theme.NonVisitedSystemColor;

            string tip = he.EventSummary + Environment.NewLine + he.EventDescription + Environment.NewLine + he.EventDetailedInfo;

            dataGridViewStarList.Rows[rownr].Cells[0].Tag = false;  //[0] records if checked EDSm

            dataGridViewStarList.Rows[rownr].Cells[0].ToolTipText = tip;
            dataGridViewStarList.Rows[rownr].Cells[1].ToolTipText = tip;
            dataGridViewStarList.Rows[rownr].Cells[2].ToolTipText = tip;
            dataGridViewStarList.Rows[rownr].Cells[3].ToolTipText = tip;                       
        }

        string Infoline(List<HistoryEntry> syslist)
        {
            string infostr = "";
            string jumponium = "";
            bool hasMaterials = false;

            if (syslist.Count > 1)
                infostr = "First visit " + syslist.Last().EventTimeLocal.ToShortDateString();

            HistoryEntry he = syslist[0];
            StarScan.SystemNode node = discoveryform.history.starscan?.FindSystem(he.System,false);

            #region information

            if (node != null)
            {
                if (node.starnodes != null)
                {
                    infostr = infostr.AppendPrePad(node.starnodes.Count.ToStringInvariant() + " Star" + ((node.starnodes.Count > 1) ? "s" : ""), Environment.NewLine);
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
                                    extrainfo = extrainfo.AppendPrePad(sc.BodyName + " is a super massive black hole", prefix);

                                // black holes
                                if (sc.StarTypeID == EDStar.H)
                                    extrainfo = extrainfo.AppendPrePad(sc.BodyName + " is a black hole", prefix);

                                // neutron stars
                                if (sc.StarTypeID == EDStar.N)
                                    extrainfo = extrainfo.AppendPrePad(sc.BodyName + " is a neutron star", prefix);

                                // white dwarf (D, DA, DAB, DAO, DAZ, DAV, DB, DBZ, DBV, DO, DOV, DQ, DC, DCV, DX)
                                string WhiteDwarf = "White Dwarf";
                                if (sc.StarTypeText.Contains(WhiteDwarf))
                                    extrainfo = extrainfo.AppendPrePad(sc.BodyName + " is a " + sc.StarTypeID + " white dwarf star", prefix);

                                // wolf rayet (W, WN, WNC, WC, WO)
                                string WolfRayet = "Wolf-Rayet";
                                if (sc.StarTypeText.Contains(WolfRayet))
                                    extrainfo = extrainfo.AppendPrePad(sc.BodyName + " is a " + sc.StarTypeID + " wolf-rayet star", prefix);

                                // giants. It should recognize all classes of giants.
                                string Giant = "Giant";
                                if (sc.StarTypeText.Contains(Giant))
                                    extrainfo = extrainfo.AppendPrePad(sc.BodyName + " is a " + sc.StarTypeText, prefix);

                                // rogue planets - not sure if they really exists, but they are in the journal, so...
                                if (sc.StarTypeID == EDStar.RoguePlanet)
                                    extrainfo = extrainfo.AppendPrePad(sc.BodyName + " is a rogue planet", prefix);
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
                                        extrainfo = extrainfo.AppendPrePad(sc.BodyName + " is an earth like moon", prefix);

                                    // Terraformable water moon
                                    if (sc.Terraformable == true && sc.PlanetTypeID == EDPlanet.Water_world)
                                        extrainfo = extrainfo.AppendPrePad(sc.BodyName + " is a terraformable water moon", prefix);
                                    // Water moon
                                    if (sc.Terraformable == false && sc.PlanetTypeID == EDPlanet.Water_world)
                                        extrainfo = extrainfo.AppendPrePad(sc.BodyName + " is a water moon", prefix);

                                    // Terraformable moon
                                    if (sc.Terraformable == true && sc.PlanetTypeID != EDPlanet.Water_world)
                                        extrainfo = extrainfo.AppendPrePad(sc.BodyName + " is a terraformable moon", prefix);

                                    // Ammonia moon
                                    if (sc.PlanetTypeID == EDPlanet.Ammonia_world)
                                        extrainfo = extrainfo.AppendPrePad(sc.BodyName + " is an ammonia moon", prefix);
                                }

                                else

                                // Do the same, for all planets
                                {
                                    // Earth Like planet
                                    if (sc.PlanetTypeID == EDPlanet.Earthlike_body)
                                        extrainfo = extrainfo.AppendPrePad(sc.BodyName + " is an earth like planet", prefix);

                                    // Terraformable water world
                                    if (sc.PlanetTypeID == EDPlanet.Water_world && sc.Terraformable == true)
                                        extrainfo = extrainfo.AppendPrePad(sc.BodyName + " is a terraformable water world", prefix);
                                    // Water world
                                    if (sc.PlanetTypeID == EDPlanet.Water_world && sc.Terraformable == false)
                                        extrainfo = extrainfo.AppendPrePad(sc.BodyName + " is a water world", prefix);

                                    // Terraformable planet
                                    if (sc.Terraformable == true && sc.PlanetTypeID != EDPlanet.Water_world)
                                        extrainfo = extrainfo.AppendPrePad(sc.BodyName + " is a terraformable planet", prefix);

                                    // Ammonia world
                                    if (sc.PlanetTypeID == EDPlanet.Ammonia_world)
                                        extrainfo = extrainfo.AppendPrePad(sc.BodyName + " is an ammonia world", prefix);
                                }
                            }
                        }

                        // Landable bodies with valuable materials
                        if (sn.ScanData != null && sn.ScanData.IsLandable == true && sn.ScanData.HasMaterials && checkBoxJumponium.Checked == true)
                        {
                            hasMaterials = true;

                            string MaterialsBrief = sn.ScanData.DisplayMaterials(4).ToString();
                            // jumponium materials: Arsenic (As), Cadmium (Cd), Carbon (C), Germanium (Ge), Niobium (Nb), Polonium (Po), Vanadium (V), Yttrium (Y)
                            // "Premium","1C,1Ge,1Nb,1As,1Po,1Y" ),
                            // "Standard", "1C,1V,1Ge,1Cd,1Nb"),
                            // "Basic", "1C,1V,1Ge"),

                            int basic = 0;
                            int standard = 0;
                            int premium = 0;
                            int totalMterials = 0;

                            if (MaterialsBrief.Contains("Arsenic"))
                            {
                                premium += 1;
                            }
                            if (MaterialsBrief.Contains("Cadmium"))
                            {
                                standard += 1;
                            }
                            if (MaterialsBrief.Contains("Carbon"))
                            {
                                basic += 1;
                                standard += 1;
                                premium += 1;
                            }
                            if (MaterialsBrief.Contains("Germanium"))
                            {
                                basic += 1;
                                standard += 1;
                                premium += 1;
                            }
                            if (MaterialsBrief.Contains("Niobium"))
                            {
                                standard += 1;
                                premium += 1;
                            }
                            if (MaterialsBrief.Contains("Polonium"))
                            {
                                premium += 1;
                            }
                            if (MaterialsBrief.Contains("Vanadium"))
                            {
                                basic += 1;
                            }
                            if (MaterialsBrief.Contains("Yttrium"))
                            {
                                premium += 1;
                            }

                            if (basic > 0 || standard > 0 || premium > 0)
                            {
                                totalMterials = basic + standard + premium;

                                StringBuilder jumpLevel = new StringBuilder();

                                // level I
                                if (basic != 0 && standard == 0 && premium == 0)
                                {
                                    jumpLevel.Append(basic + " Basic");
                                }
                                // level I and II
                                if (basic != 0 && standard != 0 && premium == 0)
                                {
                                    jumpLevel.Append(basic + " Basic " + standard + " Standard");
                                }
                                // level I
                                if (basic == 0 && standard != 0 && premium == 0)
                                {
                                    jumpLevel.Append(standard + " Standard");
                                }
                                // level II and III
                                if (basic == 0 && standard != 0 && premium != 0)
                                {
                                    jumpLevel.Append(standard + " Standard and " + premium + " Premium");
                                }
                                // level III
                                if (basic == 0 && standard == 0 && premium != 0)
                                {
                                    jumpLevel.Append(premium + " Premium");
                                }
                                // level I and III
                                if (basic != 0 && standard == 0 && premium != 0)
                                {
                                    jumpLevel.Append(basic + " Basic and " + premium + " Premium");
                                }
                                // all levels
                                if (basic != 0 && standard != 0 && premium != 0)
                                {
                                    jumpLevel.Append(basic + " Basic, " + standard + " Standard and " + premium + " Premium");
                                }

                                jumponium = jumponium.AppendPrePad("\n" + sn.ScanData.BodyName + " has " + jumpLevel + " level elements.");
                            }
                        }
                    }

                    total -= node.starnodes.Count;
                    if (total > 0)
                    {   // tell us that a system has other bodies, and how much, beside stars
                        infostr = infostr.AppendPrePad(total.ToStringInvariant() + " Other bod" + ((total > 1) ? "ies" : "y"), ", ");
                        infostr = infostr.AppendPrePad(extrainfo, prefix);                                                
                    }
                    else
                    {   // we need this to allow the panel to scan also through systems which has only stars
                        infostr = infostr.AppendPrePad(extrainfo, prefix);
                    }
                    if (hasMaterials == true && checkBoxJumponium.Checked == true)
                    {
                        infostr = infostr.AppendPrePad("\n\nThis system has materials for FSD boost: ");
                        infostr = infostr.AppendPrePad(jumponium);
                    }
                }
            }

            return infostr;
        }

        #endregion

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
            {
                DataGridViewRow row = dataGridViewStarList.CurrentRow;
                List<HistoryEntry> syslist = row.Tag as List<HistoryEntry>;

                if ((bool)row.Cells[0].Tag == false && checkBoxEDSM.Checked)
                {
                    discoveryform.history.starscan?.FindSystem(syslist[0].System, true);  // try an EDSM lookup
                    row.Cells[StarHistoryColumns.OtherInformation].Value = Infoline(syslist);
                    row.Cells[0].Tag = true;
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
                if (OnChangedSelection != null)
                    OnChangedSelection(row, dataGridViewStarList.CurrentCell.ColumnIndex, false, false);
                if (OnTravelSelectionChanged != null)
                    OnTravelSelectionChanged((dataGridViewStarList.Rows[row].Tag as List<HistoryEntry>)[0], current_historylist);
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
            Tuple<long, int> pos = CurrentGridPosByJID();

            StaticFilters.FilterGridView(dataGridViewStarList, textBoxFilter.Text);

            int rowno = FindGridPosByJID(pos.Item1, true);
            if (rowno >= 0)
                dataGridViewStarList.CurrentCell = dataGridViewStarList.Rows[rowno].Cells[pos.Item2];
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
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "System could not be found - has not been synched or EDSM is unavailable");

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

                        return new Object[] {
                            rw.Cells[0].Value,
                            rw.Cells[1].Value,
                            rw.Cells[2].Value ,
                            rw.Cells[3].Value ,
                            tlist,
                            he.WhereAmI ,
                            he.ShipInformation != null ? he.ShipInformation.Name : "Unknown",
                            he.EventDescription,
                            he.EventDetailedInfo,
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