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
using BaseUtils;
using QuickJSON;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.JournalEvents;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace EDDiscovery.UserControls
{
    public partial class UserControlExpedition : UserControlCommonBase
    {
        private SavedRouteClass loadedroute;           // this current route, null until created
        private List<ISystem> latestplottedroute;

        private string[] displayfilters;        // display filters
        private string dbDisplayFilters = "DisplayFilters";
        private string dbRolledUp = "RolledUp";
        private string dbWordWrap = "WordWrap";
        private string dbEDSM = "EDSM";

        const int lowRadiusLimit = 300 * 1000; // tiny body limit in km converted to m
        const int largeRadiusLimit = 20000 * 1000; // large body limit in km converted to m
        const double eccentricityLimit = 0.95; //orbital eccentricity limit

        private Timer autoupdateedsm;
        private bool updatingsystemrows = false;    // set during row update, to stop user interfering with the async processor without flashing icons if we just disabled them

        #region Standard UC Interfaces

        public UserControlExpedition()
        {
            InitializeComponent();
            SystemName.AutoCompleteGenerator = SystemCache.ReturnSystemAutoCompleteList;
        }

        public override void Init()
        {
            DBBaseName = "UserControlExpedition";

            dateTimePickerEndDate.Value = dateTimePickerEndTime.Value = dateTimePickerStartTime.Value = dateTimePickerStartDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow);

            discoveryform.OnNewCalculatedRoute += discoveryForm_OnNewCalculatedRoute;
            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;
            discoveryform.OnNoteChanged += Discoveryform_OnNoteChanged;
            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;

            dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

            extCheckBoxWordWrap.Checked = GetSetting(dbWordWrap, true);
            UpdateWordWrap();
            extCheckBoxWordWrap.Click += extCheckBoxWordWrap_Click;

            displayfilters = GetSetting(dbDisplayFilters, "stars;planets;signals;volcanism;values;shortinfo;gravity;atmos;rings;valueables;organics").Split(';');

            checkBoxEDSM.Checked = GetSetting(dbEDSM, false);
            this.checkBoxEDSM.CheckedChanged += new System.EventHandler(this.checkBoxEDSM_CheckedChanged);

            rollUpPanelTop.PinState = GetSetting(dbRolledUp, true);

            autoupdateedsm = new Timer() { Interval = 1000 };
            autoupdateedsm.Tick += Autoupdateedsm_Tick;
            autoupdateedsm.Start();     // something to display..

            var enumlist = new Enum[] { EDTx.UserControlExpedition_SystemName, EDTx.UserControlExpedition_Distance, EDTx.UserControlExpedition_Note, EDTx.UserControlExpedition_CurDist, 
                                        EDTx.UserControlExpedition_Visits, EDTx.UserControlExpedition_Scans, EDTx.UserControlExpedition_FSSBodies, EDTx.UserControlExpedition_KnownBodies, 
                                        EDTx.UserControlExpedition_Stars, EDTx.UserControlExpedition_Info, EDTx.UserControlExpedition_labelRouteName, EDTx.UserControlExpedition_labelDateStart, 
                                        EDTx.UserControlExpedition_labelEndDate, EDTx.UserControlExpedition_labelCml, EDTx.UserControlExpedition_labelP2P,
                                        EDTx.UserControlExpedition_ColumnDistStart, EDTx.UserControlExpedition_ColumnDistanceRemaining, EDTx.UserControlExpedition_ColumnHistoryNote};
            var enumlisttt = new Enum[] { EDTx.UserControlExpedition_extButtonLoadRoute_ToolTip, EDTx.UserControlExpedition_extButtonNew_ToolTip, EDTx.UserControlExpedition_extButtonSave_ToolTip, EDTx.UserControlExpedition_extButtonDelete_ToolTip, 
                                          EDTx.UserControlExpedition_extButtonImportFile_ToolTip, EDTx.UserControlExpedition_extButtonImportRoute_ToolTip, EDTx.UserControlExpedition_extButtonImportNavRoute_ToolTip, EDTx.UserControlExpedition_extButtonNavRouteLatest_ToolTip, 
                                          EDTx.UserControlExpedition_extButtonAddSystems_ToolTip, 
                                          EDTx.UserControlExpedition_buttonExtExport_ToolTip, EDTx.UserControlExpedition_extButtonShow3DMap_ToolTip, 
                                          EDTx.UserControlExpedition_extButtonDisplayFilters_ToolTip, EDTx.UserControlExpedition_checkBoxEDSM_ToolTip,
                                         EDTx.UserControlExpedition_buttonReverseRoute_ToolTip, EDTx.UserControlExpedition_extCheckBoxWordWrap_ToolTip };
            var enumlistcms = new Enum[] { EDTx.UserControlExpedition_copyToolStripMenuItem, EDTx.UserControlExpedition_cutToolStripMenuItem, EDTx.UserControlExpedition_pasteToolStripMenuItem,
                                            EDTx.UserControlExpedition_insertRowAboveToolStripMenuItem,
                                          EDTx.UserControlExpedition_setTargetToolStripMenuItem, EDTx.UserControlExpedition_editBookmarkToolStripMenuItem };

            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);
            BaseUtils.Translator.Instance.TranslateToolstrip(contextMenuCopyPaste, enumlistcms, this);
            rollUpPanelTop.SetToolTip(toolTip);
        }

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridView,"V2");

            if (uctg is IHistoryCursorNewStarList)
                (uctg as IHistoryCursorNewStarList).OnNewStarList += OnNewStars;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            if (uctg is IHistoryCursorNewStarList)
                (uctg as IHistoryCursorNewStarList).OnNewStarList -= OnNewStars;
            uctg = thc;
            if (uctg is IHistoryCursorNewStarList)
                (uctg as IHistoryCursorNewStarList).OnNewStarList += OnNewStars;
        }


        public override bool AllowClose()
        {
            return PromptAndSaveIfNeeded();
        }

        public override void Closing()
        {
            autoupdateedsm.Stop();

            DGVSaveColumnLayout(dataGridView,"V2");
            PutSetting(dbRolledUp, rollUpPanelTop.PinState);

            discoveryform.OnNewCalculatedRoute -= discoveryForm_OnNewCalculatedRoute;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
            discoveryform.OnNoteChanged -= Discoveryform_OnNoteChanged;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;

            if (uctg is IHistoryCursorNewStarList)
                (uctg as IHistoryCursorNewStarList).OnNewStarList -= OnNewStars;
        }

        private void Discoveryform_OnNoteChanged(object arg1, HistoryEntry arg2)
        {
            UpdateSystemRows();
        }

        private void Discoveryform_OnHistoryChange(HistoryList obj)
        {
            UpdateSystemRows();
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he, HistoryList hl)
        {
            if (he.journalEntry is IStarScan || he.IsFSDCarrierJump || he.journalEntry is IBodyNameAndID)
                UpdateSystemRows();
        }

        private void discoveryForm_OnNewCalculatedRoute(List<ISystem> obj) // called when a new route is calculated
        {
            latestplottedroute = obj;
        }

        private void OnNewStars(List<string> list, OnNewStarsPushType command)    // and if a user asked for stars to be added
        {
            if (command == OnNewStarsPushType.Expedition)
                AppendOrInsertSystems(-1,list);
        }

        #endregion


        #region Grid Display Route and update when required

        // insert or append (insertindex=-1) to the grid, either systementries, Isystems or strings
        public void AppendOrInsertSystems(int insertIndex, IEnumerable<object> sysnames, bool updatesystemrows = true)
        {
            int i = 0;
            foreach (var system in sysnames)
            {
                object[] data;

                if (system is string)
                {
                    data = new object[] { system, "" };
                }
                else if (system is SavedRouteClass.SystemEntry)
                {
                    var se = (SavedRouteClass.SystemEntry)system;
                    data = new object[] { se.SystemName, se.Note};
                }
                else
                {
                    var se = (ISystem)system;
                    data = new object[] { se.Name, ""};
                }

                if (((string)data[0]).HasChars())       // must have a name
                {
                    if (insertIndex < 0)
                        dataGridView.Rows.Add(data);
                    else
                        dataGridView.Rows.Insert(insertIndex++, data);
                }

                i++;
            }

            if (updatesystemrows)
                UpdateSystemRows();
        }

        private void ClearTable()
        {
            dataGridView.Rows.Clear();
            dateTimePickerEndDate.Value = dateTimePickerEndTime.Value = dateTimePickerStartTime.Value = dateTimePickerStartDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow);
            dateTimePickerEndTime.Checked = dateTimePickerEndDate.Checked = dateTimePickerStartTime.Checked = dateTimePickerStartDate.Checked = false;
            textBoxRouteName.Text = "";
            txtCmlDistance.Text = "";
            txtP2PDIstance.Text = "";
        }

        // this is an async function - which needs very special handling
        // scan rows indicated and fill in other columns
        // normal to do this with edsmcheck off, the auto edsm routing calls it with on
        private async void UpdateSystemRows(int rowstart = 0, int rowendinc = int.MaxValue, bool edsmcheck = false)
        {
            Cursor = Cursors.WaitCursor;
            updatingsystemrows = true;

            ISystem currentSystem = discoveryform.history.CurrentSystem(); // may be null

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
            bool disablegmoshow = displayfilters.Contains("gmoinfooff");

            ISystem startsystem = await LookupSystem(0, edsmcheck);
            ISystem endsystem = await LookupSystem(dataGridView.GetLastRowWithValue(), edsmcheck);
            if (IsClosed)
                return;
                
            for (int rowindex = rowstart; rowindex <= Math.Min(rowendinc, dataGridView.Rows.Count-1); rowindex++)
            {
                DataGridViewRow row = dataGridView.Rows[rowindex];

                string sysname = row.Cells[0].Value as string;       // value may be null, so protect (2999)

                if (!sysname.HasChars())
                    continue;

                string note = SystemNoteClass.GetTextNotesOnSystem(sysname);

                BookmarkClass bkmark = GlobalBookMarkList.Instance.FindBookmarkOnSystem(sysname);
                if (bkmark != null && !string.IsNullOrWhiteSpace(bkmark.Note))
                    note = note.AppendPrePad(bkmark.Note, "; ");

                if (!disablegmoshow)
                {
                    var gmo = discoveryform.galacticMapping.Find(sysname);
                    if (gmo != null && !string.IsNullOrWhiteSpace(gmo.Description))
                        note = note.AppendPrePad(gmo.Description, "; ");
                }

                row.Cells[ColumnHistoryNote.Index].Value = note;

                // find in history, and the DB, and EDSM, the system..

                if ( edsmcheck )    // mark if edsm checked, so we don't do it again
                    row.Cells[0].Tag = edsmcheck;       // if been looked up via EDSM, do first before async lookup as it will return immediately due to await

                //System.Diagnostics.Debug.WriteLine($"{Environment.TickCount % 10000} Looking up async for {sysname} EDSM {edsmcheck}");
                var lookup = edsmcheck;     // here so you can turn it off for speed
                var sys = await SystemCache.FindSystemAsync(sysname, discoveryform.galacticMapping, lookup);
                //System.Diagnostics.Debug.WriteLine($"{Environment.TickCount % 10000} Continuing for {sysname} EDSM {edsmcheck} found {sys?.Name}");
                if (IsClosed)        // because its async, the await returns with void, and then this is called back, and we may be closing.
                    return;

                row.Tag = sys;      // store tag

                row.Cells[0].Style.ForeColor = (sys?.HasCoordinate ?? false) ? Color.Empty : ExtendedControls.Theme.Current.UnknownSystemColor;

                row.Cells[Visits.Index].Value = discoveryform.history.Visits(sysname).ToString("0");

                if ( sys == null )      // no system found
                {
                    row.Cells[Distance.Index].Tag = null;
                    row.Cells[Distance.Index].Value =
                    row.Cells[ColumnX.Index].Value =
                    row.Cells[ColumnY.Index].Value =
                    row.Cells[ColumnZ.Index].Value =
                    row.Cells[CurDist.Index].Value =
                    row.Cells[Scans.Index].Value =
                    row.Cells[FSSBodies.Index].Value =
                    row.Cells[KnownBodies.Index].Value =
                    row.Cells[Stars.Index].Value = "";
                    row.Cells[Info.Index].Value = "System not known".T(EDTx.UserControlExpedition_EDSMUnk);
                }
                else 
                {
                    ISystem prevlinesys = rowindex > 0 ? dataGridView.Rows[rowindex - 1].Tag as ISystem : null;

                    double? dist = (prevlinesys?.HasCoordinate ?? false) ? sys.Distance(prevlinesys) : default(double?);
                    row.Cells[Distance.Index].Tag = dist;       // record in tag for computation laster
                    row.Cells[Distance.Index].Value = dist.HasValue ? dist.Value.ToString("0.#") : "";

                    row.Cells[ColumnX.Index].Value = sys.X.ToString("0.#");
                    row.Cells[ColumnY.Index].Value = sys.Y.ToString("0.#");
                    row.Cells[ColumnZ.Index].Value = sys.Z.ToString("0.#");

                    row.Cells[CurDist.Index].Value = (currentSystem?.HasCoordinate ?? false) ? sys.Distance(currentSystem).ToString("0.#") : "";

                    StarScan.SystemNode sysnode = await discoveryform.history.StarScan.FindSystemAsync(sys, lookup);

                    if (IsClosed)        // because its async, may be called during closedown. stop this
                        return;

                    if (sysnode != null)
                    {
                        row.Cells[Scans.Index].Value = sysnode.StarPlanetsScannednonEDSM().ToString("0");
                        row.Cells[FSSBodies.Index].Value = sysnode.FSSTotalBodies.HasValue ? sysnode.FSSTotalBodies.Value.ToString("0") : "";
                        row.Cells[KnownBodies.Index].Value = sysnode.StarPlanetsScanned().ToString("0");
                        row.Cells[Stars.Index].Value = sysnode.StarTypesFound(false);

                        string info = "";
                        foreach (var sn in sysnode.Bodies)
                        {
                            if (sn?.ScanData != null)  // must have scan data..
                            {
                                if (
                                   (sn.ScanData.IsBeltCluster && showbeltclusters && (!sn.ScanData.IsEDSMBody || checkBoxEDSM.Checked)) ||     // major selectors for line display
                                   (sn.ScanData.IsPlanet && showplanets && (!sn.ScanData.IsEDSMBody || checkBoxEDSM.Checked)) ||
                                   (sn.ScanData.IsStar && showstars && (!sn.ScanData.IsEDSMBody || checkBoxEDSM.Checked)) ||
                                   (showvalueables && (sn.ScanData.AmmoniaWorld || sn.ScanData.CanBeTerraformable || sn.ScanData.WaterWorld || sn.ScanData.Earthlike) && (!sn.ScanData.IsEDSMBody || checkBoxEDSM.Checked))
                                   )
                                {
                                    string bs = sn.SurveyorInfoLine(sys, showsignals, showorganics,
                                                                showvol, showv, showsi, showg,
                                                                showatmos && sn.ScanData.IsLandable, showrings,
                                                                lowRadiusLimit, largeRadiusLimit, eccentricityLimit);

                                    info = info.AppendPrePad(bs, Environment.NewLine);
                                }
                            }

                        }

                        row.Cells[Info.Index].Value = info.Trim();
                    }
                    else
                    {
                        row.Cells[Info.Index].Value = row.Cells[0].Tag != null ? "No Body information found on EDSM".T(EDTx.UserControlExpedition_EDSMUnk) : "No local scan info".T(EDTx.UserControlExpedition_NoScanInfo);
                    }
                }
            }

            {
                ISystem firstsys = null;
                ISystem lastsys = null;
                double totaldistance = 0;

                for (int rowindex = 0; rowindex < dataGridView.Rows.Count; rowindex++)  // scan all rows for distance total
                {
                    var row = dataGridView.Rows[rowindex];
                    var sys = row.Tag as ISystem;

                    if (sys?.HasCoordinate ?? false)
                    {
                        if (firstsys == null)
                            firstsys = sys;

                        lastsys = sys;

                        double? ds = row.Cells[Distance.Index].Tag as double?;
                        if (ds != null)
                        {
                            totaldistance += ds.Value;
                        }

                        row.Cells[ColumnDistStart.Index].Value = startsystem != null ? sys.Distance(startsystem).ToString("N1") : "";
                        row.Cells[ColumnDistanceRemaining.Index].Value = endsystem != null ? sys.Distance(endsystem).ToString("N1") : "";
                    }
                    else
                    {
                        row.Cells[ColumnDistStart.Index].Value = row.Cells[ColumnDistanceRemaining.Index].Value = "";
                    }
                }

                if (firstsys != null)      // therefore lastsys must too
                {
                    txtCmlDistance.Text = totaldistance.ToString("0.#") + " ly";
                    txtP2PDIstance.Text = firstsys.Distance(lastsys).ToString("0.#") + "ly";
                }
                else
                    txtCmlDistance.Text = txtP2PDIstance.Text = "";
            }

            Cursor = Cursors.Default;

            updatingsystemrows = false;
        }

        private Task<ISystem> LookupSystem(int row, bool checkedsm)
        {
            string name = (row >= 0 && row < dataGridView.RowCount && dataGridView[0, row].Value != null) ? (string)dataGridView[0, row].Value : "xxxRubbishxxx";
            return SystemCache.FindSystemAsync(name, null, checkedsm);
        }

        private void Autoupdateedsm_Tick(object sender, EventArgs e)            // tick tock to get edsm data very slowly!
        {
            if (checkBoxEDSM.Checked == false)
                return;

            for (int rowindex = 0; rowindex < dataGridView.Rows.Count; rowindex++)  // scan all rows for distance total
            {
                var row = dataGridView.Rows[rowindex];
                var name = row.Cells[0].Value as string;

                if (row.Cells[0].Tag== null && name.HasChars() )  // if cells[0] indicating EDSM lookup is null, and name, do edsm check
                {
                    var sys = row.Tag as ISystem;

                    System.Diagnostics.Debug.WriteLine($"{Environment.TickCount % 10000} Expedition - EDSM lookup on {rowindex} {dataGridView[0, rowindex].Value}");
                    UpdateSystemRows(rowindex, rowindex, true);
                    break;
                }
            }
        }

        // autopaint the row number..
        private void dataGridViewRouteSystems_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {           
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            if (!e.IsLastVisibleRow)
            {
                var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);

                // right alignment might actually make more sense for numbers
                using (var centerFormat = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    using (Brush br = new SolidBrush(grid.RowHeadersDefaultCellStyle.ForeColor))
                        e.Graphics.DrawString(rowIdx, grid.RowHeadersDefaultCellStyle.Font, br, headerBounds, centerFormat);
                }
            }
        }


        #endregion


        #region toolbar ui

        ExtendedControls.ExtListBoxForm dropdown;
        private void extButtonLoadRoute_Click(object sender, EventArgs e)
        {
            if (updatingsystemrows)
                return;

            ExtendedControls.ExtButton but = sender as ExtendedControls.ExtButton;

            dropdown = new ExtendedControls.ExtListBoxForm("", true);

            var savedroutes = SavedRouteClass.GetAllSavedRoutes();

            if (savedroutes.Count > 0)
            {
                dropdown.FitImagesToItemHeight = true;
                dropdown.Items = savedroutes.Select(x => x.Name).ToList();
                dropdown.FlatStyle = FlatStyle.Popup;
                dropdown.PositionBelow(sender as Control);
                dropdown.SelectedIndexChanged += (s, ea) =>
                {
                    if (PromptAndSaveIfNeeded())
                    {
                        string name = savedroutes[dropdown.SelectedIndex].Name;
                        savedroutes = SavedRouteClass.GetAllSavedRoutes();      // reload, in case reselecting saved route
                        loadedroute = savedroutes.Find(x => x.Name == name);        // if your picking the same route again for some strange reason

                        textBoxRouteName.Text = loadedroute.Name;
                        if (loadedroute.StartDateUTC == null)
                        {
                            dateTimePickerStartTime.Value = dateTimePickerStartDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow);
                            dateTimePickerStartTime.Checked = dateTimePickerStartDate.Checked = false;
                        }
                        else
                        {
                            dateTimePickerStartTime.Checked = dateTimePickerStartDate.Checked = true;
                            dateTimePickerStartTime.Value = dateTimePickerStartDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(loadedroute.StartDateUTC.Value);
                        }

                        if (loadedroute.EndDateUTC == null)
                        {
                            dateTimePickerEndTime.Value = dateTimePickerEndDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow);
                            dateTimePickerEndTime.Checked = dateTimePickerEndDate.Checked = false;
                        }
                        else
                        {
                            dateTimePickerEndTime.Checked = dateTimePickerEndDate.Checked = true;
                            dateTimePickerEndTime.Value = dateTimePickerEndDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(loadedroute.EndDateUTC.Value);
                        }

                        dataGridView.Rows.Clear();
                        AppendOrInsertSystems(-1, loadedroute.Systems);
                    }
                };

                ExtendedControls.Theme.Current.ApplyDialog(dropdown, true);
                dropdown.Show(this.FindForm());
            }
        }

        private void extButtonNew_Click(object sender, EventArgs e)
        {
            if (updatingsystemrows)
                return;

            if (PromptAndSaveIfNeeded())
            {
                ClearTable();
                loadedroute = null;
            }
        }

        private void extButtonSave_Click(object sender, EventArgs e)
        {
            if (updatingsystemrows)
                return;

            SaveGrid();
        }

        private void extButtonDelete_Click(object sender, EventArgs e)
        {
            if (updatingsystemrows)
                return;

            if ( loadedroute != null && loadedroute.EDSM == false && !IsDirty() )        // if loaded and unchanged, and not EDSM route
            {
                if (ExtendedControls.MessageBoxTheme.Show(FindForm(), "Are you sure you want to delete this route?".T(EDTx.UserControlExpedition_Delete), "Warning".T(EDTx.Warning), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    loadedroute.Delete();
                    loadedroute = null;
                    ClearTable();
                }
            }
        }

        private void extButtonImport_Click(object sender, EventArgs e)
        {
            if (updatingsystemrows)
                return;

            var frm = new Forms.ExportForm();
            frm.Init(true, new string[] { "CSV", "CSV No Header Line", "Text File", "JSON" },
                new string[] { "CSV|*.csv", "CSV|*.csv", "Text |*.txt|All|*.*", "JSON|*.json|All|*.*" },
                new Forms.ExportForm.ShowFlags[] { Forms.ExportForm.ShowFlags.DTOI, Forms.ExportForm.ShowFlags.DTOI, Forms.ExportForm.ShowFlags.DTCVSOI, Forms.ExportForm.ShowFlags.DTCVSOI });

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                if (frm.SelectedIndex == 3)     // JSON
                {
                    var list = ReadJSON(frm.Path);
                    if ( list != null )
                    {
                        if (list.Item1.HasChars() && textBoxRouteName.Text.IsEmpty())
                            textBoxRouteName.Text = list.Item1;
                        AppendOrInsertSystems(-1, list.Item2);
                    }
                }
                else
                {
                    CSVFile csv = new CSVFile();
                    if (csv.Read(frm.Path, FileShare.ReadWrite, frm.Comma))
                    {
                        var systems = new List<SavedRouteClass.SystemEntry>();

                        for (int r = frm.SelectedIndex == 0 ? 1 : 0; r < csv.Rows.Count; r++)  // if in mode 0, from line 1, else from line 0
                        {
                            var row = csv[r];                               // column 0 only
                            if (row.Cells.Count >= 1)
                                systems.Add(new SavedRouteClass.SystemEntry(row[0],row.Cells.Count>=2 ? row[1] : ""));
                        }

                        AppendOrInsertSystems(-1, systems);
                    }
                }
            }
        }

        private void extButtonImportRoute_Click(object sender, EventArgs e)
        {
            if (updatingsystemrows)
                return;

            if (latestplottedroute == null || latestplottedroute.Count == 0)
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "Please create a route on a route panel".T(EDTx.UserControlExpedition_Createroute), "Warning".T(EDTx.Warning));
                return;
            }

            AppendOrInsertSystems(-1, latestplottedroute);
        }

        private void extButtonImportNavRoute_Click(object sender, EventArgs e)
        {
            if (updatingsystemrows)
                return;

            var navroutes = discoveryform.history.LatestFirst().Where(x => x.EntryType == JournalTypeEnum.NavRoute && (x.journalEntry as JournalNavRoute).Route != null).Take(20).ToList();

            if (navroutes.Count > 0)
            {
                ExtendedControls.CheckedIconListBoxFormGroup navroutefilter = new CheckedIconListBoxFormGroup();

                for (int i = 0; i < navroutes.Count; i++)
                {
                    var jroute = navroutes[i].journalEntry as JournalNavRoute;
                    navroutefilter.AddStandardOption(i.ToStringInvariant(), EDDConfig.Instance.ConvertTimeToSelectedFromUTC(navroutes[i].EventTimeUTC).ToStringYearFirst() + " " + jroute.Route[0].StarSystem);
                }

                navroutefilter.SaveSettings = (s, o) =>
                {
                    int? i = s.Replace(";", "").InvariantParseIntNull();
                    if ( i != null )
                    {
                        var jroute = navroutes[i.Value].journalEntry as JournalNavRoute;

                        AppendOrInsertSystems(-1, jroute.Route.Select(x => x.StarSystem));
                    }
                };
                navroutefilter.CloseOnChange = true;
                navroutefilter.CloseBoundaryRegion = new Size(32, extButtonImportNavRoute.Height);
                navroutefilter.Show("", extButtonImportNavRoute, this.FindForm());
            }
        }

        private void extButtonNavLatest_Click(object sender, EventArgs e)
        {
            if (updatingsystemrows)
                return;

            var route = discoveryform.history.GetLastHistoryEntry(x => x.EntryType == JournalTypeEnum.NavRoute)?.journalEntry as EliteDangerousCore.JournalEvents.JournalNavRoute;
            if (route?.Route != null)
            {
                AppendOrInsertSystems(-1, route.Route.Select(s => s.StarSystem));
            }
        }
        private void buttonExtExport_Click(object sender, EventArgs e)
        {
            if (updatingsystemrows)
                return;

            var rt = CopyGridIntoRoute();
            if (rt == null)
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "There is no route to export ".T(EDTx.UserControlExpedition_NoRouteExport),
                    "Warning".T(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var frm = new Forms.ExportForm();
            frm.Init(false, new string[] { "Grid", "System Name Only", "JSON", "EDSM System Information" },
                            new string[] { "CSV export|*.csv", "Text File|*.txt|CSV export|*.csv", "JSON|*.json", "CSV export|*.csv" },
                            new Forms.ExportForm.ShowFlags[] { Forms.ExportForm.ShowFlags.DisableDateTime, Forms.ExportForm.ShowFlags.DisableDateTime, Forms.ExportForm.ShowFlags.DTCVSOI, Forms.ExportForm.ShowFlags.DisableDateTime },
                            new string[] { "ExpeditionGrid", "SystemList", "Systems", "EDSMData" }
                            );

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                if ( frm.SelectedIndex == 3 )           // EDSM System list
                {
                    List<JournalScan> scans = new List<JournalScan>();

                    foreach ( var syse in rt.Systems)
                    {
                        ISystem sys = SystemCache.FindSystem(syse.SystemName, discoveryform.galacticMapping, true);
                        if (sys != null)
                        {
                            var jl = EliteDangerousCore.EDSM.EDSMClass.GetBodiesList(sys);
                            List<JournalScan> sysscans = jl.Item1;
                            if (sysscans != null)
                                scans.AddRange(sysscans);
                        }
                    }

                    if ( CSVHelpers.OutputScanCSV(scans,frm.Path,frm.Comma,true,true,true,false,true))
                    {
                        try
                        {
                            if (frm.AutoOpen)
                                System.Diagnostics.Process.Start(frm.Path);
                        }
                        catch { }
                    }
                    else
                        CSVHelpers.WriteFailed(this.FindForm(), frm.Path);
                }
                else if (frm.SelectedIndex == 2)        // JSON
                {
                    List<string> systems = new List<string>();
                    foreach (DataGridViewRow row in dataGridView.Rows)
                    {
                        if (row.Cells[0] != null)
                        {
                            systems.Add(row.Cells[0].Value as string);
                        }
                    }

                    if ( !WriteJSON(frm.Path, textBoxRouteName.Text, systems))
                        CSVHelpers.WriteFailed(this.FindForm(), frm.Path);
                }
                else
                {
                    BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid();
                    grd.SetCSVDelimiter(frm.Comma);
                    grd.GetLineStatus += delegate (int r)
                    {
                        if (r < dataGridView.Rows.Count)
                        {
                            return BaseUtils.CSVWriteGrid.LineStatus.OK;
                        }
                        else
                            return BaseUtils.CSVWriteGrid.LineStatus.EOF;
                    };

                    grd.GetLine += delegate (int r)
                    {
                        DataGridViewRow rw = dataGridView.Rows[r];
                        if (frm.SelectedIndex == 1)
                            return new object[] { rw.Cells[0].Value };
                        else
                        {
                            object[] list = new object[dataGridView.ColumnCount];
                            for (int i = 0; i < dataGridView.ColumnCount; i++)
                                list[i] = rw.Cells[i].Value;
                            return list;
                        }
                    };

                    grd.GetHeader += delegate (int c)
                    {
                        return frm.IncludeHeader && (frm.SelectedIndex == 1 ? c == 0 : c < dataGridView.ColumnCount) ? dataGridView.Columns[c].HeaderText : null;
                    };

                    grd.WriteGrid(frm.Path, frm.AutoOpen, FindForm());
                }
            }
        }


        private void extButtonShow3DMap_Click(object sender, EventArgs e)
        {
            if (updatingsystemrows)
                return;

            var route = dataGridView.Rows.OfType<DataGridViewRow>()
                 .Where(r => r.Index < dataGridView.NewRowIndex && r.Tag != null)
                 .Select(s => s.Tag as ISystem)
                 .Where(s => s.HasCoordinate).ToList();

            if (route.Count >= 2)
            {
                discoveryform.Open3DMap(route[0], route);
            }
            else
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "No route set up. Please add at least two systems.".T(EDTx.UserControlExpedition_NoRoute), "Warning".T(EDTx.Warning), MessageBoxButtons.OK);
                return;
            }
        }

        private void extButtonAddSystems_Click(object sender, EventArgs e)
        {
            if (updatingsystemrows)
                return;

            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();

            FindSystemsUserControl usc = new FindSystemsUserControl();
            usc.ReturnSystems = (List<Tuple<ISystem, double>> syslist) =>
            {
                AppendOrInsertSystems(-1,syslist.Select(x=>x.Item1.Name.Trim()));
                f.ReturnResult(DialogResult.OK);
            };

            f.Add(new ExtendedControls.ConfigurableForm.Entry("UC", null, "", new Point(5, 30), usc.Size, null) { control = usc });
            f.AddCancel(new Point(4 + usc.Width - 80, usc.Height + 50));

            f.Trigger += (dialogname, controlname, tag) =>
            {
                if (controlname == "Cancel" || controlname == "Close")
                {
                    f.ReturnResult(DialogResult.Cancel);
                }
            };

            DBSettingsSaver db = new DBSettingsSaver(this, "Sys");

            f.ShowDialogCentred(this.FindForm(), this.FindForm().Icon, "Add Systems".T(EDTx.UserControlExpedition_AddSys),
                                callback: () =>
                                {
                                    usc.Init(db, false, discoveryform);
                                },
                                closeicon: true);
            usc.Save();

        }

        private void extButtonDisplayFilters_Click(object sender, EventArgs e)
        {
            if (updatingsystemrows)
                return;

            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();
            displayfilter.AllOrNoneBack = false;

            displayfilter.AddAllNone();
            displayfilter.AddStandardOption("stars", "Show All Stars".TxID(EDTx.UserControlSurveyor_showAllStarsToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption("planets", "Show All Planets".TxID(EDTx.UserControlSurveyor_showAllPlanetsToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_ShowMoons);
            displayfilter.AddStandardOption("beltcluster", "Show belt clusters".TxID(EDTx.UserControlSurveyor_showBeltClustersToolStripMenuItem), global::EDDiscovery.Icons.Controls.Belt);
            displayfilter.AddStandardOption("valueables", "Show valuable bodies".T(EDTx.UserControlStarList_valueables), global::EDDiscovery.Icons.Controls.Scan_Bodies_HighValue);
            displayfilter.AddStandardOption("signals", "Has Signals".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_hasSignalsToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_Signals);
            displayfilter.AddStandardOption("volcanism", "Has Volcanism".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_hasVolcanismToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_Volcanism);
            displayfilter.AddStandardOption("values", "Show values".TxID(EDTx.UserControlSurveyor_showValuesToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_HighValue);
            displayfilter.AddStandardOption("shortinfo", "Show More Information".TxID(EDTx.UserControlSurveyor_showMoreInformationToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_Landable);
            displayfilter.AddStandardOption("gravity", "Show gravity of landables".TxID(EDTx.UserControlSurveyor_showGravityToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_Landable);
            displayfilter.AddStandardOption("atmos", "Show atmospheres of landables".TxID(EDTx.UserControlSurveyor_showAtmosToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_Landable);
            displayfilter.AddStandardOption("rings", "Show rings".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_hasRingsToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_RingOnly);
            displayfilter.AddStandardOption("organics", "Show organic scans".T(EDTx.UserControlStarList_scanorganics), global::EDDiscovery.Icons.Controls.Scan_Bodies_NSP);
            displayfilter.AddStandardOption("gmoinfooff", "Disable showing GMO Info".T(EDTx.UserControlExpedition_GMOInfo), global::EDDiscovery.Icons.Controls.Globe);
            displayfilter.ImageSize = new Size(24, 24);
            displayfilter.SaveSettings = (s, o) =>
            {
                displayfilters = s.Split(';');
                PutSetting(dbDisplayFilters, string.Join(";", displayfilters));
                UpdateSystemRows();
            };

            displayfilter.CloseBoundaryRegion = new Size(32, extButtonDisplayFilters.Height);
            displayfilter.Show(string.Join(";", displayfilters), extButtonDisplayFilters, this.FindForm());
        }

        private void checkBoxEDSM_CheckedChanged(object sender, EventArgs e)
        {
            PutSetting(dbEDSM, checkBoxEDSM.Checked);       // update the store

            if (!updatingsystemrows)    // if we are not in the update system rows, redraw it
                UpdateSystemRows();
        }

        private void extCheckBoxWordWrap_Click(object sender, EventArgs e)
        {
            PutSetting(dbWordWrap, extCheckBoxWordWrap.Checked);
            UpdateWordWrap();
        }

        private void UpdateWordWrap()
        {
            dataGridView.SetWordWrap(extCheckBoxWordWrap.Checked);
            extPanelDataGridViewScroll.UpdateScroll();
        }

        private void buttonReverseRoute_Click(object sender, EventArgs e)
        {
            if (updatingsystemrows)
                return;

            var route = CopyGridIntoRoute();

            if (route != null)
            {
                dataGridView.Rows.Clear();
                route.ReverseSystemList();
                AppendOrInsertSystems(-1,route.Systems);
            }
        }

        #endregion

        #region Double click
        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (updatingsystemrows)
                return;

            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == ColumnHistoryNote.Index || e.ColumnIndex == Info.Index)
                {
                    var cell = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

                    string text = cell.Value as string;

                    if (text.HasChars())
                    {
                        InfoForm frm = new InfoForm();
                        frm.Info(dataGridView.Columns[e.ColumnIndex].HeaderText, FindForm().Icon, text);
                        frm.Show(FindForm());
                    }
                }
                else if ( e.ColumnIndex == Note.Index)
                {
                    if (!dataGridView.IsCurrentCellInEditMode)
                    {
                        Forms.SetNoteForm noteform = new Forms.SetNoteForm();
                        DataGridViewRow rw = dataGridView.Rows[e.RowIndex];
                        string x = rw.Cells[ColumnX.Index].Value as string;
                        string y = rw.Cells[ColumnY.Index].Value as string;
                        string z = rw.Cells[ColumnZ.Index].Value as string;
                        string distremain = rw.Cells[ColumnDistanceRemaining.Index].Value as string;
                        string summary = x.HasChars() ? $"{x}, {y}, {z} @ {distremain} ly" : "";

                        noteform.Init(rw.Cells[Note.Index].Value as string, DateTime.MinValue,
                                        rw.Cells[SystemName.Index].Value as string,
                                        summary,
                                        rw.Cells[ColumnHistoryNote.Index].Value as string
                                        );

                        if (noteform.ShowDialog(FindForm()) == DialogResult.OK)
                        {
                            dataGridView[Note.Index, e.RowIndex].Value = noteform.NoteText;
                        }
                    }
                }
            }

        }

        #endregion

        #region Right Click Context

        private void contextMenuCopyPaste_Opening(object sender, CancelEventArgs e)
        {
            if (updatingsystemrows)
            {
                e.Cancel = true;
                return;
            }

            bool hastext = false;

            try
            {
                hastext = Clipboard.ContainsText();
            }
            catch
            {
                System.Diagnostics.Trace.WriteLine("Unable to access clipboard");
            }

            copyToolStripMenuItem.Enabled = dataGridView.SelectedCells.Count > 0;
            pasteToolStripMenuItem.Enabled = hastext;
            cutToolStripMenuItem.Enabled = dataGridView.SelectedRows.Count > 0;     // only on if we marked rows

            var rows = dataGridView.SelectedRowAndCount(true, true, 0, false);      // same as below in insert, ascending, use cells if not row selection, leave on new row
            insertRowAboveToolStripMenuItem.Enabled = rows.Item1 != dataGridView.NewRowIndex;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataObject obj = dataGridView.GetClipboardContent();

            try
            {
                Clipboard.SetDataObject(obj);
            }
            catch
            {
                System.Diagnostics.Trace.WriteLine("Unable to access clipboard");
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copyToolStripMenuItem_Click(sender, e);       // copy first

            int[] selectedRows = dataGridView.SelectedRows(false,false);        // decending, must have row selection

            foreach ( var row in selectedRows)   // delete in this specific highest row first order
                dataGridView.Rows.RemoveAt(row);

            UpdateSystemRows();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string data = null;

            try
            {
                data = Clipboard.GetText();
            }
            catch
            {
                System.Diagnostics.Trace.WriteLine("Unable to access clipboard");
            }

            if (data != null )
            {
                bool tabs = data.Contains("\t");            // data copied from grid uses tab as cell dividers, else use comma
                string[][] textlines = data.Split(Environment.NewLine, tabs ? "\t" : ",", StringComparison.InvariantCulture);   // use nice splitter to split into newlines, then cells

                var rows = dataGridView.SelectedRowAndCount(true, true);   // ascending, use cells, default is 0, and remove new row
                // make up a systementry with the name, and possibly the note
                var se = textlines.Where(x => x.Length > 0 && x[0].HasChars()).Select(y => new SavedRouteClass.SystemEntry(y[0], y.Length >= 2 ? y[1] : ""));

                AppendOrInsertSystems(rows.Item1, se);
            }
        }

        private void insertRowAboveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rows = dataGridView.SelectedRowAndCount(true, true, 0, false);   // ascending, use cells if not row selection, don't change due to new row
            dataGridView.Rows.Insert(rows.Item1, rows.Item2);
            UpdateSystemRows();
        }

        private void setTargetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] selectedRows = dataGridView.SelectedCells.OfType<DataGridViewCell>().Where(c => c.RowIndex != dataGridView.NewRowIndex).Select(c => c.RowIndex).OrderBy(v => v).Distinct().ToArray();

            if (selectedRows.Length == 0)
                return;
            var obj = dataGridView[0, selectedRows[0]].Value;

            if (obj == null)
                return;

            ISystem sc = SystemCache.FindSystem((string)obj);       
            if (sc != null && sc.HasCoordinate)
            {
                if (TargetClass.SetTargetOnSystemConditional(sc.Name, sc.X, sc.Y, sc.Z))
                {
                    discoveryform.NewTargetSet(this);
                }
            }
        }

        private void editBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] selectedRows = dataGridView.SelectedCells.OfType<DataGridViewCell>()
                .Where(c => c.RowIndex != dataGridView.NewRowIndex)
                .Select(c => c.RowIndex).OrderBy(v => v).Distinct().ToArray();

            if (selectedRows.Length == 0)
                return;

            var obj = dataGridView[0, selectedRows[0]].Value;

            if (obj == null)
                return;

            ISystem sc = SystemCache.FindSystem((string)obj,discoveryform.galacticMapping, true);     // use EDSM directly if required

            if (sc == null)
                sc = new SystemClass((string)obj,0,0,0);

            BookmarkHelpers.ShowBookmarkForm(this.FindForm(), discoveryform, sc, null);
            UpdateSystemRows();
        }

        #endregion

        #region Validation

        private void dataGridViewRouteSystems_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                var row = dataGridView.Rows[e.RowIndex];
                var cellvalue = row.Cells[0].Value as string;
                if ( cellvalue.HasChars() && (row.Tag == null || !((ISystem)row.Tag).Name.Equals(cellvalue, StringComparison.InvariantCultureIgnoreCase)))
                {
                    System.Diagnostics.Debug.WriteLine("Expedition- Invalidate tag as text has changed");
                    row.Cells[0].Tag = row.Tag = null;        // clear system lookup
                    UpdateSystemRows(e.RowIndex, e.RowIndex);
                }
            }
        }

        private void dataGridViewRouteSystems_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 0)
                e.SortDataGridViewColumnAlphaInt();
            else if (e.Column.Index == Distance.Index || (e.Column.Index>=ColumnX.Index && e.Column.Index <= KnownBodies.Index))
                e.SortDataGridViewColumnNumeric();
        }

        private void dataGridView_Sorted(object sender, EventArgs e)
        {
            UpdateSystemRows();     // once sorted, recompute columns
        }

        #endregion

        #region JSON Output/Input of systems and name

        public static Tuple<string, List<string>> ReadJSON(string path)
        {
            string text = BaseUtils.FileHelpers.TryReadAllTextFromFile(path);
            if (text != null)
            {
                JObject jo = JObject.Parse(text, JToken.ParseOptions.AllowTrailingCommas | JToken.ParseOptions.CheckEOL);

                if (jo != null)
                {
                    var syslist = jo["Systems"].Array();

                    if (syslist != null)
                    {
                        List<string> sys = new List<string>();
                        foreach (var jk in syslist)
                            sys.Add(jk.Str());
                        return new Tuple<string, List<string>>(jo["Name"].Str(), sys);
                    }
                }
            }

            return null;
        }

        public static bool WriteJSON(string path, string name, List<string> systems)
        {
            JObject jn = new JObject();
            jn.Add("Name", name);
            JArray ja = new JArray();
            foreach (var s in systems)
                ja.Add(s);
            jn.Add("Systems", ja);
            try
            {
                File.WriteAllText(path, jn.ToString(true));
                return true;
            }
            catch
            {
                return false;
            }

        }

        #endregion

    }
}
