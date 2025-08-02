/*
 * Copyright © 2016 - 2023 EDDiscovery development team
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
        private string dbEDSMSpansh = "EDSMSpansh";

        const int lowRadiusLimit = 300 * 1000; // tiny body limit in km converted to m
        const int largeRadiusLimit = 20000 * 1000; // large body limit in km converted to m
        const double eccentricityLimit = 0.95; //orbital eccentricity limit

        private Timer autoupdate;

        private bool forcetotalsupdate = false;     // force an update of the totals
        private int outstandingprocessing = 0;      // processing is outstanding

        #region Standard UC Interfaces

        public UserControlExpedition()
        {
            InitializeComponent();
            SystemName.AutoCompleteGenerator = SystemCache.ReturnSystemAutoCompleteList;
        }

        protected override void Init()
        {
            DBBaseName = "UserControlExpedition";

            dateTimePickerEndDate.Value = dateTimePickerEndTime.Value = dateTimePickerStartTime.Value = dateTimePickerStartDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow);

            DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange;
            DiscoveryForm.OnNoteChanged += Discoveryform_OnNoteChanged;
            DiscoveryForm.OnNewEntry += Discoveryform_OnNewEntry;

            dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

            extCheckBoxWordWrap.Checked = GetSetting(dbWordWrap, true);
            UpdateWordWrap();
            extCheckBoxWordWrap.Click += extCheckBoxWordWrap_Click;

            displayfilters = GetSetting(dbDisplayFilters, "stars;planets;signals;volcanism;values;shortinfo;gravity;atmos;rings;valueables;organics").Split(';');

            edsmSpanshButton.Init(this, dbEDSMSpansh, "");
            edsmSpanshButton.ValueChanged += (s, ch) =>
            {
                System.Diagnostics.Debug.WriteLine($"EDSM/Spansh changed {ch} spansh {edsmSpanshButton.WebLookup}");
            };

            rollUpPanelTop.PinState = GetSetting(dbRolledUp, true);

            autoupdate = new Timer() { Interval = 100 };
            autoupdate.Tick += Autoupdate_Tick;

            BaseUtils.TranslatorMkII.Instance.TranslateControls(this);
            BaseUtils.TranslatorMkII.Instance.TranslateTooltip(toolTip,this);
            BaseUtils.TranslatorMkII.Instance.TranslateToolstrip(contextMenuCopyPaste);
            rollUpPanelTop.SetToolTip(toolTip);

            labelBusy.Text = "BUSY";    // in english, suck it up
        }

        protected override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridView,"V2");
            autoupdate.Start();     // start check tick
        }

        public override bool AllowClose()
        {
            return PromptAndSaveIfNeeded();
        }

        protected override void Closing()
        {
            autoupdate.Stop();

            DGVSaveColumnLayout(dataGridView,"V2");
            PutSetting(dbRolledUp, rollUpPanelTop.PinState);

            DiscoveryForm.OnHistoryChange -= Discoveryform_OnHistoryChange;
            DiscoveryForm.OnNoteChanged -= Discoveryform_OnNoteChanged;
            DiscoveryForm.OnNewEntry -= Discoveryform_OnNewEntry;
        }

        private void Discoveryform_OnNoteChanged(object arg1, HistoryEntry arg2)
        {
            UpdateAllRows();
        }

        private void Discoveryform_OnHistoryChange()
        {
            UpdateAllRows();
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he)
        {
            if (he.journalEntry is IStarScan || he.IsFSDCarrierJump || he.journalEntry is IBodyNameAndID)
                UpdateAllRows();
        }

        public override PanelActionState PerformPanelOperation(UserControlCommonBase sender, object actionobj)
        {
            var push = actionobj as UserControlCommonBase.PushStars;
            var action = actionobj as UserControlCommonBase.PanelAction;
            var pushlist = actionobj as UserControlCommonBase.PushRouteList;
            if (push != null)
            {
                if (push.PushTo == PushStars.PushType.Expedition)
                {
                    if (push.SystemList != null)
                    {
                        AppendOrInsertSystems(-1, push.SystemList);     // systems list 
                    }
                    else
                        AppendOrInsertSystems(-1, push.SystemNames);   // push names

                    if ( push.MakeVisible )
                        MakeVisible();
                    if (push.RouteTitle != null)
                        textBoxRouteName.Text = push.RouteTitle;
                    return PanelActionState.Success;
                }
            }
            else if ( action != null )
            {
                if ( action.Action == PanelAction.ImportCSV)
                {
                    if (PromptAndSaveIfNeeded())
                    {
                        MakeVisible();      // we may not be on this screen if called (shutdown, import) make visible
                        ClearTable();
                        string file = action.Data as string;
                        System.Diagnostics.Debug.WriteLine($"Expedition import CSV {file}");
                        string str = FileHelpers.TryReadAllTextFromFile(file);
                        Import(0, Path.GetFileNameWithoutExtension(file), str, ",", true); // will cope with str = null
                    }
                    return PanelActionState.Success;
                }
            }
            else if ( pushlist != null)
            { 
                latestplottedroute = pushlist.Systems;
                return PanelActionState.HandledContinue;
            }

            return PanelActionState.NotHandled;
        }

        #endregion


        // tags: row.tag holds the computed system class
        const int systemnametagcell = 0;    //cells[0].Tag is the system name tag, set to detect differences in name
        const int processedweblookup = 1;   //cells[processedweblookup].Tag is null (not processed), or indicates if a web lookup has occurred (bool)
        const int id64imported = 2;         //cells[id64imported].Tag is the system address, set if we imported it from external, or set if we did a db lookup

        #region auto update

        private void Autoupdate_Tick(object sender, EventArgs e)            // tick tock to get edsm data very slowly!
        {
            // if we are not EDSM checking, or nothing is outstanding, we can check to see if we can process

            if (edsmSpanshButton.IsNoneSet || outstandingprocessing == 0)      // if not doing web, or no outstandings
            {
                int maxlaunch = edsmSpanshButton.WebLookup == WebExternalDataLookup.EDSM ? 1 : edsmSpanshButton.WebLookup == WebExternalDataLookup.None ? 20 : 2;

              // System.Diagnostics.Debug.WriteLine($"{AppTicks.MSd} Expedition Checking rows {maxlaunch}");

                for (int rowindex = 0; rowindex < dataGridView.Rows.Count; rowindex++)  // scan all rows
                {
                    var row = dataGridView.Rows[rowindex];
                    var name = row.Cells[0].Value as string;            // name in grid now
                    string nametag = row.Cells[systemnametagcell].Tag as string;        // null or system name, set in here, used to detect name change
                    SystemClass systemtag = row.Tag as SystemClass;     // null or set after processing. May or may not have co-ords
                    bool? weblookup = row.Cells[processedweblookup].Tag != null ? (bool)row.Cells[processedweblookup].Tag : default(bool?); // null or edsm flag is in tag 1 - meaning we processed

                    // if name, so row can be valid
                    if (name.HasChars())
                    {
                        // if name tag is null, or name tag is not name, or edsm tag is set but different to current orders, process again
                        if ((nametag == null || nametag != name) || (weblookup.HasValue && weblookup.Value != edsmSpanshButton.IsAnySet))
                        {
                            if (nametag != null && nametag != name)        // if changed name.. we clear the xyz, so it will update
                            {
                                row.Cells[ColumnX.Index].Value = "";
                                row.Cells[ColumnY.Index].Value = "";
                                row.Cells[ColumnZ.Index].Value = "";
                            }

                            row.Cells[systemnametagcell].Tag = name;        // set to mark we processed with this name
                            row.Cells[processedweblookup].Tag = null;       // cancel the processed/edsm flag, it will be set on completion by process
                            row.Cells[id64imported].Tag = null;             // cancel any imported id64 tag
                            forcetotalsupdate = true;       // update when finished
                            labelBusy.Visible = true;       // make busy

                            UpdateRowAsync(rowindex, edsmSpanshButton.WebLookup );

                            if (--maxlaunch == 0)           // if launch max, stop
                                return;
                        }
                        else if ( weblookup.HasValue )        // if we have processed this line
                        {
                            // lets see if the user has changed the co-ords manually

                            SystemClass gridsys = GetSystemClass(rowindex);     // get the system class from the grid. Will never be null, since here grid name is not null

                            //System.Diagnostics.Debug.WriteLine($"Row {rowindex} processed, gridsys [{gridsys}] vs [{systemtag}], tag {row.cells[processedweblookup].Tag}");

                            // if we have a gridpos, lets see if either system tag was not set, or its different xyz using the ints to compare

                            if ( gridsys.HasCoordinate && (!systemtag.HasCoordinate || 
                                        systemtag.Xi != gridsys.Xi || systemtag.Yi != gridsys.Yi || systemtag.Zi != gridsys.Zi ))
                            {
                                System.Diagnostics.Debug.WriteLine($"Expedition Grid row {rowindex} position now set or changed");
                                row.Tag = gridsys;
                                row.Cells[0].Style.ForeColor = Color.Empty;
                                forcetotalsupdate = true;
                            }
                            else if (systemtag.HasCoordinate && !gridsys.HasCoordinate ) // user fucked up the position ;-)
                            {
                                System.Diagnostics.Debug.WriteLine($"Expedition Grid row {rowindex} position lost {systemtag} -> {gridsys}");
                                row.Tag = gridsys;
                                forcetotalsupdate = true;
                                row.Cells[0].Style.ForeColor = ExtendedControls.Theme.Current.UnknownSystemColor;
                            }
                        }
                    }
                }
            }

            bool alldone = true;

            // scan all rows and see if any are outstanding via the edsmtag set in UpdateRowAsync

            for (int rowindex = 0; rowindex < dataGridView.Rows.Count; rowindex++)
            {
                var row = dataGridView.Rows[rowindex];
                var name = row.Cells[0].Value as string;
                var edsmtag = row.Cells[processedweblookup].Tag;
                if ( name.HasChars() && edsmtag == null) // if its got a name, and we have not processed it, stop
                {
                    alldone = false;
                    break;
                }
            }

            // check both edsm tag and outstanding for safety, then process if required

            if ( alldone && outstandingprocessing == 0 && forcetotalsupdate)        
            {
                System.Diagnostics.Debug.WriteLine($"{AppTicks.MSd} Update totals");

                SystemClass firstsys = dataGridView.Rows[0].Tag as SystemClass;     // may be null, may not have distance.  Find first system
                if (firstsys != null && !firstsys.HasCoordinate)        // no co-ord means firstsys is useless
                    firstsys = null;

                SystemClass lastsys = null;
                for (int i = dataGridView.RowCount - 1; i >= 0; i--)        // find last system
                {
                    if (dataGridView.Rows[i].Tag != null)
                    {
                        lastsys = dataGridView.Rows[i].Tag as SystemClass;      // find last filled one
                        if (!lastsys.HasCoordinate)                             // no coord, useless
                            lastsys = null;
                        break;
                    }
                }

                double[] distances = new double[dataGridView.Rows.Count];       // will all be 0
                SystemClass prevrowsystem = null;

                for (int rowindex = 0; rowindex < dataGridView.Rows.Count; rowindex++)  // for all rows, compute distances
                {
                    SystemClass sys = dataGridView.Rows[rowindex].Tag as SystemClass;
                    if (sys != null && sys.HasCoordinate)       // if its a valid co-ordinate system
                    {
                        if (prevrowsystem != null)              // we can measure distance..
                            distances[rowindex] = prevrowsystem.Distance(sys);

                        //System.Diagnostics.Debug.WriteLine($"{rowindex} {prevrowsystem?.Name ?? "-"} -> {sys.Name} : {distances[rowindex]}");

                        prevrowsystem = sys;                    // and we set this to system. We skip entries without valid co-ords
                    }
                }

                double totaldistance = distances.Sum();
                double distancedone = 0;

                for (int rowindex = 0; rowindex < dataGridView.Rows.Count; rowindex++)  // for all rows, compute distances
                {
                    distancedone += distances[rowindex];

                    var row = dataGridView.Rows[rowindex];
                    row.Cells[Distance.Index].Value = distances[rowindex]>0 ? distances[rowindex].ToString("N2") : "";

                    SystemClass sys = row.Tag as SystemClass;
                    row.Cells[ColumnDistStart.Index].Value = firstsys != null && sys != null && sys.HasCoordinate ? sys.Distance(firstsys).ToString("N2") : "";
                    row.Cells[ColumnDistanceRemaining.Index].Value = distances[rowindex] > 0 || rowindex == 0 ? (totaldistance - distancedone).ToString("N2") : "";

                }

                txtCmlDistance.Text = totaldistance.ToString("N1") + " ly";
                txtP2PDIstance.Text = firstsys != null && lastsys != null ? firstsys.Distance(lastsys).ToString("N1") + "ly" : "?";

                forcetotalsupdate = false;
                labelBusy.Visible = false;
            }
        }


        #endregion

        #region Grid Display Route and update when required

        private void ClearTable()
        {
            dataGridView.Rows.Clear();
            dateTimePickerEndDate.Value = dateTimePickerEndTime.Value = dateTimePickerStartTime.Value = dateTimePickerStartDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow);
            dateTimePickerEndTime.Checked = dateTimePickerEndDate.Checked = dateTimePickerStartTime.Checked = dateTimePickerStartDate.Checked = false;
            textBoxRouteName.Text = "";
            txtCmlDistance.Text = "";
            txtP2PDIstance.Text = "";
            loadedroute = null;
        }

        void UpdateAllRows()
        {
            foreach (DataGridViewRow row in dataGridView.Rows)      // clear all tags
                row.Cells[systemnametagcell].Tag = null;
        }

        // this is an async function - which needs very special handling
        // update a row with edsm check if required

        private async void UpdateRowAsync(int rowindex, EliteDangerousCore.WebExternalDataLookup lookup)
        {
            System.Threading.Interlocked.Increment(ref outstandingprocessing);      // increase processing count to indicate we are going. Doubt we need the interlock

            ISystem historySystem = DiscoveryForm.History.CurrentSystem(); // may be null

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
            bool showTemp = displayfilters.Contains("temp");
            bool showrings = displayfilters.Contains("rings");
            bool showorganics = displayfilters.Contains("organics");
            bool disablegmoshow = displayfilters.Contains("gmoinfooff");

            DataGridViewRow row = dataGridView.Rows[rowindex];

            // get the system, with name, and include xyz from grid
            SystemClass sys = GetSystemClass(rowindex);

            System.Diagnostics.Debug.WriteLine($"{AppTicks.MSd} Update row {rowindex} {sys.Name} {sys.SystemAddress} coord {sys.X} {sys.Y} {sys.Z}");

            if (sys != null)
            {
                string note = SystemNoteClass.GetTextNotesOnSystem(sys.Name);

                BookmarkClass bkmark = GlobalBookMarkList.Instance.FindBookmarkOnSystem(sys.Name);
                if (bkmark != null && !string.IsNullOrWhiteSpace(bkmark.Note))
                    note = note.AppendPrePad(bkmark.Note, "; ");

                if (!disablegmoshow)
                {
                    var gmo = DiscoveryForm.GalacticMapping.FindSystem(sys.Name);
                    if (gmo != null && !string.IsNullOrWhiteSpace(gmo.Description))
                        note = note.AppendPrePad(gmo.Description, "; ");
                }

                row.Cells[ColumnHistoryNote.Index].Value = note;

                row.Cells[Visits.Index].Value = DiscoveryForm.History.Visits(sys.Name).ToString("0");

                //  if we don't have a co-ord, or we don't have a system address, see if we can find it in db
                if (!sys.HasCoordinate || !sys.SystemAddress.HasValue)
                {
                    System.Diagnostics.Debug.WriteLine($"..{AppTicks.MSd} Looking up async for {sys.Name} {lookup}");
                    var syslookup = await SystemCache.FindSystemAsync(sys.Name, DiscoveryForm.GalacticMapping, lookup);
                    System.Diagnostics.Debug.WriteLine($"..{AppTicks.MSd} Continuing for {sys.Name} found {syslookup}");
                    if (IsClosed)        // because its async, the await returns with void, and then this is called back, and we may be closing.
                        return;

                    if (syslookup != null)
                    {
                        //System.Diagnostics.Debug.WriteLine($"Lookup for {sys.Name} Found co-ords");
                        row.Cells[ColumnX.Index].Value = syslookup.X.ToString("N5");              // write culture specific location.
                        row.Cells[ColumnY.Index].Value = syslookup.Y.ToString("N5");
                        row.Cells[ColumnZ.Index].Value = syslookup.Z.ToString("N5");

                        if ( sys.SystemAddress != null )                                // this is the id64 cache location used by GetSystemClass, update it
                            row.Cells[id64imported].Tag = sys.SystemAddress;

                        sys = new SystemClass(syslookup);
                    }
                }

                row.Cells[0].Style.ForeColor = sys.HasCoordinate ? Color.Empty : ExtendedControls.Theme.Current.UnknownSystemColor;

                double? disttocur = sys.HasCoordinate && historySystem != null ? sys.Distance(historySystem) : default(double?);
                row.Cells[CurDist.Index].Value = disttocur.HasValue ? disttocur.Value.ToString("N2") : "";

                StarScan.SystemNode sysnode = await DiscoveryForm.History.StarScan.FindSystemAsync(sys, lookup);

                if (IsClosed)        // because its async, may be called during closedown. stop this
                    return;

                if (sysnode != null)
                {
                    row.Cells[Scans.Index].Value = sysnode.StarPlanetsWithData(edsmSpanshButton.IsAnySet).ToString("0");
                    row.Cells[FSSBodies.Index].Value = sysnode.FSSTotalBodies.HasValue ? sysnode.FSSTotalBodies.Value.ToString("0") : "";
                    row.Cells[KnownBodies.Index].Value = sysnode.StarPlanetsWithData(edsmSpanshButton.IsAnySet).ToString("0");
                    row.Cells[Stars.Index].Value = sysnode.StarTypesFound(false);

                    string info = "";
                    foreach (var sn in sysnode.Bodies())
                    {
                        if (sn?.ScanData != null)  // must have scan data..
                        {
                            if (
                                (sn.ScanData.IsBeltCluster && showbeltclusters && (!sn.ScanData.IsWebSourced || edsmSpanshButton.IsAnySet)) ||     // major selectors for line display
                                (sn.ScanData.IsPlanet && showplanets && (!sn.ScanData.IsWebSourced || edsmSpanshButton.IsAnySet)) ||
                                (sn.ScanData.IsStar && showstars && (!sn.ScanData.IsWebSourced || edsmSpanshButton.IsAnySet)) ||
                                (showvalueables && (sn.ScanData.AmmoniaWorld || sn.ScanData.CanBeTerraformable || sn.ScanData.WaterWorld || sn.ScanData.Earthlike) && 
                                            (!sn.ScanData.IsWebSourced || edsmSpanshButton.IsAnySet))
                                )
                            {
                                string bs = sn.SurveyorInfoLine(sys, showsignals, showorganics,
                                                            showvol, showv, showsi, showg,
                                                            showatmos && sn.ScanData.IsLandable, showTemp, showrings,
                                                            lowRadiusLimit, largeRadiusLimit, eccentricityLimit);

                                info = info.AppendPrePad(bs, Environment.NewLine);
                            }
                        }

                    }

                    row.Cells[Info.Index].Value = info.Trim();
                }
                else
                {
                    row.Cells[Scans.Index].Value =
                    row.Cells[FSSBodies.Index].Value =
                    row.Cells[KnownBodies.Index].Value =
                    row.Cells[Stars.Index].Value = "";
                    row.Cells[Info.Index].Value = lookup != EliteDangerousCore.WebExternalDataLookup.None ? "No Body information found on web".Tx(): "No local scan info".Tx();
                }
            }

            System.Diagnostics.Debug.Assert(sys != null);

            System.Diagnostics.Debug.WriteLine($"{AppTicks.MSd} Set Expedition Row {rowindex} tag is {sys} set cells[1] to {lookup}");
            row.Tag = sys;      // keep system tag
            row.Cells[processedweblookup].Tag = lookup != EliteDangerousCore.WebExternalDataLookup.None;       // and record the web state. this indicates row has been processed

            System.Threading.Interlocked.Decrement(ref outstandingprocessing);      // and decrease processing count..
        }

        public static System.Threading.Tasks.Task KillTimeAsync(int time)       // debug feature
        {
            return System.Threading.Tasks.Task.Run(() =>
            {
                System.Threading.Thread.Sleep(time);
            });
        }


        // return a system class from grid information, with optional xyz. Null if no name
        private SystemClass GetSystemClass(int rown)
        {
            if (rown >= 0 && rown < dataGridView.Rows.Count)
            {
                DataGridViewRow row = dataGridView.Rows[rown];
                string name = (string)row.Cells[SystemName.Index].Value;

                if (name.HasChars())
                {
                    // it was written in current culture above in UpdateRow, so need to read it in current culture with style Number

                    double? xpos = ((string)row.Cells[ColumnX.Index].Value).ParseDoubleNull(System.Globalization.CultureInfo.CurrentCulture, System.Globalization.NumberStyles.Number);
                    double? ypos = ((string)row.Cells[ColumnY.Index].Value).ParseDoubleNull(System.Globalization.CultureInfo.CurrentCulture, System.Globalization.NumberStyles.Number);
                    double? zpos = ((string)row.Cells[ColumnZ.Index].Value).ParseDoubleNull(System.Globalization.CultureInfo.CurrentCulture, System.Globalization.NumberStyles.Number);

                    bool knownpos = xpos != null && ypos != null && zpos != null;
                    long? id64 = row.Cells[id64imported].Tag as long?;      // we may have an id 64 tag if it was imported from somewhere

                    return knownpos ? new SystemClass(name, id64, xpos.Value,ypos.Value,zpos.Value) : new SystemClass(name, id64);
                }
            }
            return null;
        }

        #endregion

        #region Grid Paint

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

        #region Toolbar ui

        private void extButtonLoadRoute_Click(object sender, EventArgs e)
        {
            if (outstandingprocessing != 0)
            {
                Console.Beep(512, 500);
                return;
            }

            ExtButton but = sender as ExtendedControls.ExtButton;

            CheckedIconNewListBoxForm selection = new CheckedIconNewListBoxForm();

            var savedroutes = SavedRouteClass.GetAllSavedRoutes();

            if (savedroutes.Count > 0)      // may not in debug situation of bad internet have any
            {
                foreach (var x in savedroutes)
                {
                    selection.UC.AddButton("tag", x.Name, EDDiscovery.Icons.Controls.expedition, usertag: x);
                }

                //displayfilter.UC.ImageSize = new Size(4, 4);
                selection.CloseBoundaryRegion = new Size(32, 32);
                selection.UC.MultiColumnSlide = true;
                selection.PositionBelow(but);
                selection.UC.ButtonPressed += (index, tag, text, usertag, barg) =>
                {
                    selection.Close();
                    if (PromptAndSaveIfNeeded())
                    {
                        string name = savedroutes[index].Name;
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

                selection.Show(this.FindForm());
            }
        }

        private void extButtonNew_Click(object sender, EventArgs e)
        {
            if (outstandingprocessing != 0)
            {
                Console.Beep(512, 500);
                return;
            }

            if (PromptAndSaveIfNeeded())
            {
                ClearTable();
            }
        }

        private void extButtonSave_Click(object sender, EventArgs e)
        {
            if (outstandingprocessing != 0)
            {
                Console.Beep(512, 500);
                return;
            }

            SaveGrid();
        }

        private void extButtonDelete_Click(object sender, EventArgs e)
        {
            if (outstandingprocessing != 0)
            {
                Console.Beep(512, 500);
                return;
            }

            if ( loadedroute != null && loadedroute.EDSM == false && !IsDirty() )        // if loaded and unchanged, and not EDSM route
            {
                if (ExtendedControls.MessageBoxTheme.Show(FindForm(), "Are you sure you want to delete this route?".Tx(), "Warning".Tx(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    loadedroute.Delete();
                    ClearTable();
                }
            }
        }

        private void extButtonImport_Click(object sender, EventArgs e)
        {
            if (outstandingprocessing != 0)
            {
                Console.Beep(512, 500);
                return;
            }

            var frm = new Forms.ImportExportForm();
            frm.Import( new string[] { "CSV", "Text File", "JSON" },
                new Forms.ImportExportForm.ShowFlags[] { Forms.ImportExportForm.ShowFlags.ShowImportOptions, Forms.ImportExportForm.ShowFlags.ShowImportOptions, Forms.ImportExportForm.ShowFlags.ShowPaste },
                new string[] { "CSV|*.csv", "Text |*.txt|All|*.*", "JSON|*.json|All|*.*" }
                );

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                var src = frm.ReadSource();

                if (src == null || !Import(frm.SelectedIndex, Path.GetFileNameWithoutExtension(frm.Path), src, frm.Delimiter, frm.ExcludeHeader))
                {
                    MessageBoxTheme.Show(FindForm(), "Failed to read " + frm.Path, "Import Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        // form = 0 CSV with ignored header line. Cells 0 = system, optional 1 = note, 2.. are merged into note
        // form = 1 text file
        // duplicate systems on same lines removed and notes are merges
        // any columns past 2 are merged into notes
        // form = 2 json import
        // text can be null to complain about a bad load

        private bool Import(int importtype, string name, string text, string delimiter, bool excludeheaderrow)
        {
            if (text != null)
            {
                if (importtype == 2)
                {
                    var list = ReadJSON(text);
                    if (list != null)
                    {
                        if (list.Item1.HasChars() && textBoxRouteName.Text.IsEmpty())
                            textBoxRouteName.Text = list.Item1;
                        AppendOrInsertSystems(-1, list.Item2);
                        textBoxRouteName.Text = name;
                        return true;
                    }
                }
                else 
                {
                    CSVFile csv = new CSVFile(delimiter);
                    if (csv.ReadString(text))
                    {
                        var systems = new List<SavedRouteClass.SystemEntry>();
                        string lastsystem = null;

                        CSVFile.Row rowheader = csv.Rows.Count > 1 && excludeheaderrow ? csv[0] : null;

                        for (int r = excludeheaderrow ? 1 : 0; r < csv.Rows.Count; r++)
                        {
                            var row = csv[r];                               // column 0 only
                            if (row.Cells.Count >= 1)
                            {
                                string note = "";
                                for (int i = 1; i < row.Cells.Count; i++)
                                {
                                    string header = rowheader != null && rowheader.Cells.Count > i ? rowheader[i] + ":" : "";
                                    string data = row[i];
                                    if (data.InvariantParseDoubleNull() != null && data.Contains(".")) // if its a number, and its a dotted number
                                        data = data.InvariantParseDouble(0).ToString("0.#####");

                                    note = note.AppendPrePad(header + data, Environment.NewLine);
                                }

                                string systemname = row[0];
                                if (systemname == lastsystem)
                                {
                                    systems.Last().Note = systems.Last().Note.AppendPrePad(note, Environment.NewLine);
                                }
                                else
                                    systems.Add(new SavedRouteClass.SystemEntry(systemname, note));

                                lastsystem = systemname;
                            }

                        }

                        textBoxRouteName.Text = name;
                        AppendOrInsertSystems(-1, systems);
                        return true;
                    }
                }
            }

            ExtendedControls.MessageBoxTheme.Show(FindForm(), "Failed to read " + name, "Import Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
        }

        private void extButtonImportRoute_Click(object sender, EventArgs e)
        {
            if (outstandingprocessing != 0)
            {
                Console.Beep(512, 500);
                return;
            }

            if (latestplottedroute == null || latestplottedroute.Count == 0)
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "Please create a route on a route panel".Tx(), "Warning".Tx());
                return;
            }

            AppendOrInsertSystems(-1, latestplottedroute);
        }

        private void extButtonImportNavRoute_Click(object sender, EventArgs e)
        {
            if (outstandingprocessing != 0)
            {
                Console.Beep(512, 500);
                return;
            }

            var navroutes = DiscoveryForm.History.LatestFirst().Where(x => x.EntryType == JournalTypeEnum.NavRoute && (x.journalEntry as JournalNavRoute).Route != null).Take(20).ToList();

            if (navroutes.Count > 0)
            {
                ExtendedControls.CheckedIconNewListBoxForm navroutefilter = new CheckedIconNewListBoxForm();

                for (int i = 0; i < navroutes.Count; i++)
                {
                    var jroute = navroutes[i].journalEntry as JournalNavRoute;
                    navroutefilter.UC.Add(i.ToStringInvariant(), EDDConfig.Instance.ConvertTimeToSelectedFromUTC(navroutes[i].EventTimeUTC).ToStringYearFirst() + " " + jroute.Route[0].StarSystem);
                }

                navroutefilter.SaveSettings = (s, o) =>
                {
                    int? i = s.Replace(";", "").InvariantParseIntNull();
                    if ( i != null )
                    {
                        var jroute = navroutes[i.Value].journalEntry as JournalNavRoute;

                        AppendOrInsertSystems(-1, jroute.Route.Select(r => new SavedRouteClass.SystemEntry(r.StarSystem, "", r.StarPos.X, r.StarPos.Y, r.StarPos.Z)));
                    }
                };
                navroutefilter.CloseOnChange = true;
                navroutefilter.CloseBoundaryRegion = new Size(32, extButtonImportNavRoute.Height);
                navroutefilter.Show("", extButtonImportNavRoute, this.FindForm());
            }
        }

        private void extButtonNavLatest_Click(object sender, EventArgs e)
        {
            if (outstandingprocessing != 0)
            {
                Console.Beep(512, 500);
                return;
            }

            var route = DiscoveryForm.History.GetLastHistoryEntry(x => x.EntryType == JournalTypeEnum.NavRoute)?.journalEntry as EliteDangerousCore.JournalEvents.JournalNavRoute;
            if (route?.Route != null)
            {
                AppendOrInsertSystems(-1, route.Route.Select(r => new SavedRouteClass.SystemEntry(r.StarSystem, "", r.StarPos.X, r.StarPos.Y, r.StarPos.Z)));
            }
        }
        private void buttonExtExport_Click(object sender, EventArgs e)
        {
            if (outstandingprocessing != 0)
            {
                Console.Beep(512, 500);
                return;
            }

            var rt = CopyGridIntoRoute();
            if (rt == null)
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "There is no route to export ".Tx(),
                    "Warning".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var frm = new Forms.ImportExportForm();
            frm.Export( new string[] { "Grid", "System Name Only", "JSON", "EDSM System Information" },
                            new Forms.ImportExportForm.ShowFlags[] { Forms.ImportExportForm.ShowFlags.ShowCSVOpenInclude, Forms.ImportExportForm.ShowFlags.ShowCSVOpenInclude, Forms.ImportExportForm.ShowFlags.None, Forms.ImportExportForm.ShowFlags.ShowCSVOpenInclude },
                            new string[] { "CSV export|*.csv", "Text File|*.txt|CSV export|*.csv", "JSON|*.json", "CSV export|*.csv" },
                            new string[] { "ExpeditionGrid", "SystemList", "Systems", "EDSMData" }
                            );

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                if ( frm.SelectedIndex == 3 )           // EDSM System list
                {
                    List<JournalScan> scans = new List<JournalScan>();

                    foreach ( var syse in rt.Systems)
                    {
                        ISystem sys = SystemCache.FindSystem(syse.Name, DiscoveryForm.GalacticMapping, EliteDangerousCore.WebExternalDataLookup.All);
                        if (sys != null)
                        {
                            var jl = EliteDangerousCore.EDSM.EDSMClass.GetBodiesList(sys);
                            List<JournalScan> sysscans = jl?.Bodies;
                            if (sysscans != null)
                                scans.AddRange(sysscans);
                        }
                    }

                    if ( CSVHelpers.OutputScanCSV(scans,frm.Path,frm.Delimiter,true,true,true,false,true))
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
                    BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid(frm.Delimiter);

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
            //var savedroutes = SavedRouteClass.GetAllSavedRoutes();            // debug only
            //SavedRouteClass tr = savedroutes.Find(x => x.Name == "Test2");
            //if (tr != null)
            //{
            //    tr.TestHarness2();
            //}

            if (outstandingprocessing != 0)
            {
                Console.Beep(512, 500);
                return;
            }

            var route = dataGridView.Rows.OfType<DataGridViewRow>()
                 .Where(r => r.Index < dataGridView.NewRowIndex && r.Tag != null)
                 .Select(s => s.Tag as ISystem)
                 .Where(s => s.HasCoordinate).ToList();

            if (route.Count >= 2)
            {
                DiscoveryForm.Open3DMap(route[0], route);
            }
            else
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "No route set up. Please add at least two systems.".Tx(), "Warning".Tx(), MessageBoxButtons.OK);
                return;
            }
        }

        private void extButtonAddSystems_Click(object sender, EventArgs e)
        {
            if (outstandingprocessing != 0)
            {
                Console.Beep(512, 500);
                return;
            }

            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();

            FindSystemsUserControl usc = new FindSystemsUserControl();
            BaseUtils.TranslatorMkII.Instance.TranslateControls(usc);

            usc.ReturnSystems = (List<Tuple<ISystem, double>> syslist) =>
            {
                AppendOrInsertSystems(-1, syslist.Select(r=> new SavedRouteClass.SystemEntry(r.Item1.Name.Trim(),"",r.Item1.X,r.Item1.Y,r.Item1.Z)));
                f.ReturnResult(DialogResult.OK);
            };

            f.Add(new ExtendedControls.ConfigurableEntryList.Entry(usc, "UC", "", new Point(5, 30), usc.Size, null));

            f.Trigger += (dialogname, controlname, tag) =>
            {
                if (controlname == "Cancel" || controlname == "Close")
                {
                    f.ReturnResult(DialogResult.Cancel);
                }
            };

            UserDatabaseSettingsSaver db = new UserDatabaseSettingsSaver(this, "Sys");

            f.ShowDialogCentred(this.FindForm(), this.FindForm().Icon, "Add Systems".Tx(),
                                callback: () =>
                                {
                                    usc.Init(db, false, DiscoveryForm);
                                },
                                closeicon: true);
            usc.Save();

        }

        private void extButtonDisplayFilters_Click(object sender, EventArgs e)
        {
            if (outstandingprocessing != 0)
            {
                Console.Beep(512, 500);
                return;
            }

            ExtendedControls.CheckedIconNewListBoxForm displayfilter = new CheckedIconNewListBoxForm();
            displayfilter.AllOrNoneBack = false;

            displayfilter.UC.AddAllNone();
            displayfilter.UC.Add("stars", "Show All Stars".Tx(), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.UC.Add("planets", "Show All Planets".Tx(), global::EDDiscovery.Icons.Controls.Scan_ShowMoons);
            displayfilter.UC.Add("beltcluster", "Show Belt Clusters".Tx(), global::EDDiscovery.Icons.Controls.Belt);
            displayfilter.UC.Add("valueables", "Show valuable bodies".Tx(), global::EDDiscovery.Icons.Controls.Scan_Bodies_HighValue);
            displayfilter.UC.Add("signals", "Has any other signals".Tx(), global::EDDiscovery.Icons.Controls.Scan_Bodies_Signals);
            displayfilter.UC.Add("volcanism", "Has volcanism".Tx(), global::EDDiscovery.Icons.Controls.Scan_Bodies_Volcanism);
            displayfilter.UC.Add("values", "Show values".Tx(), global::EDDiscovery.Icons.Controls.Scan_Bodies_HighValue);
            displayfilter.UC.Add("shortinfo", "Show more information".Tx(), global::EDDiscovery.Icons.Controls.Scan_Bodies_Landable);
            displayfilter.UC.Add("gravity", "Show gravity of landables".Tx(), global::EDDiscovery.Icons.Controls.Scan_Bodies_Landable);
            displayfilter.UC.Add("atmos", "Show atmospheres".Tx(), global::EDDiscovery.Icons.Controls.Scan_Bodies_Landable);
            displayfilter.UC.Add("temp", "Show surface temperature".Tx(), global::EDDiscovery.Icons.Controls.Scan_Bodies_Signals);
            displayfilter.UC.Add("rings", "Show rings".Tx(), global::EDDiscovery.Icons.Controls.Scan_Bodies_RingOnly);
            displayfilter.UC.Add("organics", "Show organic scans".Tx(), global::EDDiscovery.Icons.Controls.Scan_Bodies_NSP);
            displayfilter.UC.Add("gmoinfooff", "Disable showing GMO Info".Tx(), global::EDDiscovery.Icons.Controls.Globe);
            displayfilter.UC.ImageSize = new Size(24, 24);
            displayfilter.SaveSettings = (s, o) =>
            {
                displayfilters = s.Split(';');
                PutSetting(dbDisplayFilters, string.Join(";", displayfilters));
                UpdateAllRows();
            };

            displayfilter.CloseBoundaryRegion = new Size(32, extButtonDisplayFilters.Height);
            displayfilter.Show(string.Join(";", displayfilters), extButtonDisplayFilters, this.FindForm());
        }

        private void extCheckBoxWordWrap_Click(object sender, EventArgs e)
        {
            PutSetting(dbWordWrap, extCheckBoxWordWrap.Checked);
            UpdateWordWrap();
        }

        private void UpdateWordWrap()
        {
            dataGridView.SetWordWrap(extCheckBoxWordWrap.Checked);
            Note.DefaultCellStyle.WrapMode = DataGridViewTriState.True;     // notes are by default word wrapped.
            extPanelDataGridViewScroll.UpdateScroll();
        }

        private void buttonReverseRoute_Click(object sender, EventArgs e)
        {
            if (outstandingprocessing != 0)
            {
                Console.Beep(512, 500);
                return;
            }

            List<DataGridViewRow> rows = new List<DataGridViewRow>();
            rows.AddRange(dataGridView.Rows.Cast<DataGridViewRow>().Where(x=>x.Cells[0].Value != null && x.Cells[0].Value.ToString().HasChars()));
            rows.Reverse();
            dataGridView.Rows.Clear();
            dataGridView.Rows.AddRange(rows.ToArray());

            forcetotalsupdate = true;
        }

        #endregion

        #region Double click
        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (outstandingprocessing != 0)
            {
                Console.Beep(512, 500);
                return;
            }

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
            if (outstandingprocessing != 0)     // protect whole of context menu copy pasta
            {
                Console.Beep(512, 500);
                e.Cancel = true;
                return;
            }

            copyToolStripMenuItem.Enabled = dataGridView.SelectedCells.Count > 0;
            pasteToolStripMenuItem.Enabled = ClipboardHasText();
            cutToolStripMenuItem.Enabled = dataGridView.SelectedRows.Count > 0;     // only on if we marked rows

            var rows = dataGridView.SelectedRowAndCount(true, true, 0, false);      // same as below in insert, ascending, use cells if not row selection, leave on new row
            insertRowAboveToolStripMenuItem.Enabled = rows.Item1 != dataGridView.NewRowIndex;

            rows = dataGridView.SelectedRowAndCount(true, true, -1, false);
            viewOnEDSMToolStripMenuItem.Enabled = viewSystemToolStripMenuItem.Enabled = rows.Item1 >= 0 && rows.Item2 == 1 && dataGridView.Rows[rows.Item1].Tag != null && rows.Item1 != dataGridView.NewRowIndex;
            viewOnSpanshToolStripMenuItem.Enabled = viewSystemToolStripMenuItem.Enabled && (dataGridView.Rows[rows.Item1].Tag as ISystem).SystemAddress.HasValue;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetClipboard(dataGridView.GetClipboardContent());
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copyToolStripMenuItem_Click(sender, e);       // copy first

            int[] selectedRows = dataGridView.SelectedRows(false,false);        // decending, must have row selection

            foreach ( var row in selectedRows)   // delete in this specific highest row first order
                dataGridView.Rows.RemoveAt(row);

            forcetotalsupdate = true;           // update the totals
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string data = GetClipboardText();

            if (data != null )
            {
                bool tabs = data.Contains("\t");            // data copied from grid uses tab as cell dividers, else use comma
                string[][] textlines = data.Split(Environment.NewLine, tabs ? "\t" : ",", StringComparison.InvariantCulture);   // use nice splitter to split into newlines, then cells

                var rows = dataGridView.SelectedRowAndCount(true, true, nonewrow:false);   // ascending, use cells, default is 0, and keep end row
                // make up a systementry with the name, and possibly the note
                var se = textlines.Where(x => x.Length > 0 && x[0].HasChars()).Select(r => new SavedRouteClass.SystemEntry(r[0], r.Length >= 2 ? r[1] : "",
                                        r.Length >= 3 ? r[2].InvariantParseDouble(SavedRouteClass.SystemEntry.NotKnown) : SavedRouteClass.SystemEntry.NotKnown,
                                        r.Length >= 4 ? r[3].InvariantParseDouble(SavedRouteClass.SystemEntry.NotKnown) : SavedRouteClass.SystemEntry.NotKnown,
                                        r.Length >= 5 ? r[4].InvariantParseDouble(SavedRouteClass.SystemEntry.NotKnown) : SavedRouteClass.SystemEntry.NotKnown
                                        ));

                AppendOrInsertSystems(rows.Item1, se);
            }
        }

        private void insertRowAboveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rows = dataGridView.SelectedRowAndCount(true, true, 0, false);   // ascending, use cells if not row selection, don't change due to new row
            dataGridView.Rows.Insert(rows.Item1, rows.Item2);
            forcetotalsupdate = true;           // update the totals
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
                    DiscoveryForm.NewTargetSet(this);
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

            ISystem sc = SystemCache.FindSystem((string)obj,DiscoveryForm.GalacticMapping, EliteDangerousCore.WebExternalDataLookup.All);     // use EDSM directly if required

            if (sc == null)
                sc = new SystemClass((string)obj,0,0,0);

            BookmarkHelpers.ShowBookmarkForm(this.FindForm(), DiscoveryForm, sc, null);
            UpdateAllRows();
        }


        private void viewSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rows = dataGridView.SelectedRowAndCount(true, true, -1, false);
            ScanDisplayForm.ShowScanOrMarketForm(this.FindForm(), dataGridView.Rows[rows.Item1].Tag as ISystem, DiscoveryForm.History, forcedlookup: edsmSpanshButton.WebLookup);
        }

        private void viewOnSpanshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rows = dataGridView.SelectedRowAndCount(true, true, -1, false);
            EliteDangerousCore.Spansh.SpanshClass.LaunchBrowserForSystem((dataGridView.Rows[rows.Item1].Tag as ISystem).SystemAddress.Value);
        }

        private void viewOnEDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rows = dataGridView.SelectedRowAndCount(true, true, -1, false);
            EliteDangerousCore.EDSM.EDSMClass edsm = new EliteDangerousCore.EDSM.EDSMClass();
            edsm.ShowSystemInEDSM((dataGridView.Rows[rows.Item1].Tag as ISystem).Name);
        }

        #endregion

        #region Drag drop

        private void dataGridView_DragDrop(object sender, DragEventArgs e)
        {
            if (outstandingprocessing != 0)     // protect against update
            {
                Console.Beep(512, 500);
                return;
            }

            // still check if the associated data from the file(s) can be used for this purpose
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Fetch the file(s) names with full path here to be processed
                string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (outstandingprocessing != 0 == false)
                {
                    if (PromptAndSaveIfNeeded())
                    {
                        ClearTable();
                        System.Diagnostics.Debug.WriteLine($"Expedition import CSV {fileList[0]}");
                        string str = FileHelpers.TryReadAllTextFromFile(fileList[0]);
                        Import(0, Path.GetFileNameWithoutExtension(fileList[0]), str, ",", true);   // will cope with str = null
                    }
                }
                else
                    Console.Beep(512, 500);
            }
        }

        private void dataGridView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (Path.GetExtension(fileList[0]).EqualsIIC(".csv"))
                    e.Effect = DragDropEffects.Move;
            }
        }

        #endregion

        #region Validation

        private void dataGridViewRouteSystems_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 0)
                e.SortDataGridViewColumnAlphaInt();
            else if (e.Column.Index == Distance.Index || (e.Column.Index>=ColumnX.Index && e.Column.Index <= KnownBodies.Index))
                e.SortDataGridViewColumnNumeric();
        }

        private void dataGridView_Sorted(object sender, EventArgs e)
        {
            forcetotalsupdate = true;       // recompute the totals
        }

        #endregion

        #region JSON Output/Input of systems and name

        public static Tuple<string, List<string>> ReadJSON(string text)
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
