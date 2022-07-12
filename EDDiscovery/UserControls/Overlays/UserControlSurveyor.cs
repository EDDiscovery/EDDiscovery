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
using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.JournalEvents;
using EMK.LightGeometry;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlSurveyor : UserControlCommonBase
    {
        private StringAlignment alignment = StringAlignment.Near;
        private string fsssignalsdisplayed = "";

        const int lowRadiusLimit = 300 * 1000; // tiny body limit in km converted to m
        const int largeRadiusLimit = 20000 * 1000; // large body limit in km converted to m
        const double eccentricityLimit = 0.95; //orbital eccentricity limit

        private EliteDangerousCore.UIEvents.UIGUIFocus.Focus uistate = EliteDangerousCore.UIEvents.UIGUIFocus.Focus.NoFocus;
        private EliteDangerousCore.UIEvents.UIMode.ModeType uimode = EliteDangerousCore.UIEvents.UIMode.ModeType.None;

        const string NavRouteNameLabel = "!*NavRoute";      // special label to identify a save of using the nav route - not user presented
        private string lastroutetext = "";      // cached so we can represent it even if we pass a sys into drawsystem with no coord
        private string translatednavroutename = "";
        private SavedRouteClass currentRoute = null;
        private string lastsystemroute;

        private ShipInformation shipinfo;   // and last ship info
        private EliteDangerousCalculations.FSDSpec.JumpInfo shipfsdinfo;        // last values of fsd info

        private Font displayfont;

        private ISystem last_sys = null;
        private string starclass = "";
        private Timer updatetimer;
        private int bodies_found;
        private bool all_found = false;

        #region Initialisation

        public UserControlSurveyor()
        {
            InitializeComponent();
            DBBaseName = "Surveyor";
        }

        public override void Init()
        {
            checkBoxEDSM.Checked = GetSetting("edsm", false);
            checkBoxEDSM.Click += new System.EventHandler(this.checkBoxEDSM_Clicked);

            PopulateCtrlList();

            extCheckBoxWordWrap.Checked = GetSetting("wordwrap", false);
            extCheckBoxWordWrap.Click += wordWrapToolStripMenuItem_Click;

            fsssignalsdisplayed = GetSetting("fsssignals", "");

            discoveryform.OnNewUIEvent += Discoveryform_OnNewUIEvent;
            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;
            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            discoveryform.OnNewTarget += Discoveryform_OnNewTarget;
            GlobalBookMarkList.Instance.OnBookmarkChange += GlobalBookMarkList_OnBookmarkChange;

            string thisname = typeof(UserControlSurveyor).Name;
            var enumlisttt = new Enum[] { EDTx.UserControlSurveyor_extButtonPlanets_ToolTip, EDTx.UserControlSurveyor_extButtonStars_ToolTip, EDTx.UserControlSurveyor_extButtonShowControl_ToolTip, EDTx.UserControlSurveyor_extButtonAlignment_ToolTip, EDTx.UserControlSurveyor_extButtonFSS_ToolTip, EDTx.UserControlSurveyor_checkBoxEDSM_ToolTip, EDTx.UserControlSurveyor_extButtonSetRoute_ToolTip, EDTx.UserControlSurveyor_extButtonControlRoute_ToolTip, EDTx.UserControlSurveyor_extButtonFont_ToolTip, EDTx.UserControlSurveyor_extCheckBoxWordWrap_ToolTip, EDTx.UserControlSurveyor_extButtonSearches_ToolTip };
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this, thisname);
            rollUpPanelTop.SetToolTip(toolTip);

            translatednavroutename = "Nav Route".T(EDTx.UserControlSurveyor_navroute);

            displayfont = FontHelpers.GetFont(GetSetting("font", ""), null);        // null if not set

            LoadRoute(GetSetting("route", ""));
            routecontrolsettings = GetSetting("routecontrol", "showJumps;showwaypoints");

            rollUpPanelTop.PinState = GetSetting("PinState", true);

            updatetimer = new Timer() { Interval = 250 };
            updatetimer.Tick += Updatetimer_Tick;
        }


        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;
        }

        public override void InitialDisplay()
        {
            last_sys = uctg.GetCurrentHistoryEntry?.System;
            SetTitle();   // may be null
            DrawSystem(last_sys);    // may be null
            SetVisibility();
        }

        public override void Closing()
        {
            updatetimer.Stop();

            PutSetting("PinState", rollUpPanelTop.PinState);

            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            discoveryform.OnNewUIEvent -= Discoveryform_OnNewUIEvent;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.OnNewTarget -= Discoveryform_OnNewTarget;
            GlobalBookMarkList.Instance.OnBookmarkChange -= GlobalBookMarkList_OnBookmarkChange;
        }

        private void Discoveryform_OnHistoryChange(HistoryList hl)
        {
            last_sys = hl.GetLast?.System;      // may be null
            shipfsdinfo = hl.GetLast?.GetJumpInfo(discoveryform.history.MaterialCommoditiesMicroResources.CargoCount(hl.GetLast.MaterialCommodity));
            shipinfo = hl.GetLast?.ShipInformation;

            LoadRoute(GetSetting("route", ""));     // reload the route, may have locations now
            //System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber}  History Load '{currentRoute?.Name}' {currentRoute?.Id} systems {currentRoute?.Systems.Count} lastsys '{last_sys?.Name}'");

            SetTitle();   // may be null
            DrawSystem(last_sys);    // may be null
            SetVisibility();

            //t.Interval = 200; t.Tick += (s, e) => { var sys = currentRoute.PosAlongRoute(percent, 0); DrawSystem(sys, sys.Name); percent += 0.5; }; t.Start();  // debug to make it play thru.. leave
            //  t.Interval = 2000; t.Tick += (s, e) => { if (sysno < currentRoute.Systems.Count ){ var sys = currentRoute.KnownSystemList()[sysno++]; DrawSystem(sys.Item1, sys.Item1.Name); } }; t.Start();  // debug to make it play thru.. leave
        }
        //int sysno = 0; double percent = -10; Timer t = new Timer();// play thru harness

        private void Discoveryform_OnNewEntry(HistoryEntry he, HistoryList hl)
        {
            // received a new navroute, and we have navroute selected, reload
            if (he.EntryType == JournalTypeEnum.NavRoute && currentRoute != null && currentRoute.Id == -1)
            {
                //System.Diagnostics.Debug.WriteLine("Surveyor {displaynumber} new entry, load nav route");
                LoadRoute(NavRouteNameLabel);
                last_sys = hl.GetLast?.System;      // may be null, make sure its set.
                DrawSystem(last_sys);
            }
        }

        private void Discoveryform_OnNewTarget(object obj)
        {
            DrawSystem(last_sys);
        }
        private void GlobalBookMarkList_OnBookmarkChange(BookmarkClass bk, bool deleted)
        {
            DrawSystem(last_sys);
        }

        private void Uctg_OnTravelSelectionChanged(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            if (he != null)
            {
                bool islatest = Object.ReferenceEquals(hl.GetLast, he);        // is this the latest

                if (islatest && he.EntryType == JournalTypeEnum.Friends)        // if latest, and its friends, we ignore, so we don't let Friends interrupt the start jump sequence..
                    return;                                                     // friends can occur between startjump/fsdjump

                // something has changed and just blindly for now recalc the fsd info
                shipfsdinfo = he.GetJumpInfo(discoveryform.history.MaterialCommoditiesMicroResources.CargoCount(he.MaterialCommodity));
                shipinfo = he.ShipInformation;

                bool kicktimer = false;

                if (last_sys == null || last_sys.Name != he.System.Name) // If not got a system, or different name
                {
                    starclass = null;           // we cancel out the text info fields
                    bodies_found = 0;
                    all_found = false;
                    last_sys = he.System;       // and set, then
                    kicktimer = true;           // kick for redisplay
                }
                else
                    last_sys = he.System;       // its the same system, but since we may have synthesised a last_sys in startjump, and we have a real one from the journal, make sure its updated
                
                if (he.EntryType == JournalTypeEnum.StartJump)         // so we can pre-present
                {
                    JournalStartJump jsj = he.journalEntry as JournalStartJump;
                    if (jsj.IsHyperspace)       // needs to be a hyperspace one, not supercruise
                    {
                        last_sys = new SystemClass(jsj.SystemAddress, jsj.StarSystem);       // important need system address as scan uses it for quick lookup
                        starclass = jsj.FriendlyStarClass;
                        bodies_found = 0;
                        all_found = false;
                        kicktimer = true;
                    }
                }
                else if (he.EntryType == JournalTypeEnum.FSSAllBodiesFound)     // since we present body counts in title, we update. Not used in search 
                {
                    JournalFSSAllBodiesFound fs = he.journalEntry as JournalFSSAllBodiesFound;
                    all_found = true;
                    bodies_found = fs.Count;     
                    SetTitle();
                }
                else if (he.EntryType == JournalTypeEnum.FSSDiscoveryScan)      // since we present body counts in title, we update. Discovery scans are not used in search so no need to kick
                {
                    var je = he.journalEntry as JournalFSSDiscoveryScan;
                    bodies_found = je.BodyCount;
                    SetTitle();
                }

                // these are in IStarScan, but we don't need to do anything here, as they don't affect the display
                else if ( he.EntryType == JournalTypeEnum.FSDJump || he.EntryType == JournalTypeEnum.SAAScanComplete || he.EntryType == JournalTypeEnum.Location || he.EntryType == JournalTypeEnum.CarrierJump )         
                {
                   // System.Diagnostics.Debug.WriteLine($"Surveyor Ignore {he.EventTimeUTC} {he.journalEntry.EventTypeStr}");
                }

                // IStarScan : FSSSignalDiscovery, SAASignalsFound, FSSBodySignals, Scan all involved in Searches
                // Scan is involved in inbuilt presentations
                // ScanBaryCentre is also indirectly involved in Searches

                else if (he.journalEntry is IStarScan || he.EntryType == JournalTypeEnum.FuelScoop)      
                {
                    System.Diagnostics.Debug.WriteLine($"Surveyor Update due to {he.EventTimeUTC} {he.journalEntry.EventTypeStr}");
                    //System.Diagnostics.Debug.WriteLine("Scan got, sys " + he.System.Name + " " + last_sys.Name);
                    kicktimer = true;
                }

                if ( kicktimer )
                {
                    updatetimer.Stop();
                    updatetimer.Start();
                    System.Diagnostics.Debug.WriteLine($"Surveyor {Environment.TickCount % 10000} start timer on {last_sys.Name}");
                }
            }
        }

        private void Updatetimer_Tick(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Surveyor {Environment.TickCount % 10000} timer expired on {last_sys.Name}");
            updatetimer.Stop();
            SetTitle();
            DrawSystem(last_sys);
        }

        private void Discoveryform_OnNewUIEvent(UIEvent uievent)
        {
            EliteDangerousCore.UIEvents.UIGUIFocus gui = uievent as EliteDangerousCore.UIEvents.UIGUIFocus;
            EliteDangerousCore.UIEvents.UIMode mode = uievent as EliteDangerousCore.UIEvents.UIMode;
            bool refresh = false;

            if (gui != null)
            {
                refresh = gui.GUIFocus != uistate;
                uistate = gui.GUIFocus;

            }
            else if ( mode != null)
            {
                refresh = mode.Mode != uimode;
                uimode = mode.Mode;
            }

            if (refresh)
            {
                SetVisibility();
            }
        }

        public override void onControlTextVisibilityChanged(bool newvalue)       // user changed control text, but I don't think we need to know from testing. Leave as a reminder
        {
        }

        public override bool SupportTransparency { get { return true; } }       // turn it on
        public override void SetTransparency(bool on, Color curcol)
        {
            extPictureBoxScroll.ScrollBarEnabled = !on;     // turn off the scroll bar if its transparent
            extPictureBoxScroll.BackColor = pictureBoxSurveyor.BackColor = this.BackColor = curcol;
            rollUpPanelTop.Visible = !on;
            SetVisibility(!on);     // set our visible mode for text, but override to on if we are not transparent
        }

        // normally we use uistate/uimode to determine if the output is visible. We can override this, for use when transparent mode is on but its not transparent
        private void SetVisibility(bool overrideon = false)
        {
            bool showit = true;

            // if in autohide, and a transparent option is turned on
            if (!overrideon && IsSet(CtrlList.autohide) && IsTransparent )     
            {
                // if no focus, or fssmode and override
                showit = uistate == EliteDangerousCore.UIEvents.UIGUIFocus.Focus.NoFocus || (uistate == EliteDangerousCore.UIEvents.UIGUIFocus.Focus.FSSMode && IsSet(CtrlList.donthidefssmode));
                // and we should be either in None or MainShip..
                showit = showit && (uimode == EliteDangerousCore.UIEvents.UIMode.ModeType.None || uimode == EliteDangerousCore.UIEvents.UIMode.ModeType.MainShipSupercruise);
            }

            System.Diagnostics.Debug.WriteLine($"Surveyor uimode {uimode} uistate {uistate} Visibility {showit}");
            extPictureBoxScroll.Visible = showit;
            extPictureBoxTitle.Visible = IsSet(CtrlList.showsysinfo) && showit;
        }

        private void UserControlSurveyor_Resize(object sender, EventArgs e)
        {
            DrawSystem(last_sys);
        }

        #endregion

        #region Display

        // default title
        private void SetTitle()
        {
            string text = last_sys?.Name ?? "";
            if (starclass.HasChars() && IsSet(CtrlList.showstarclass))
                text += " | " + starclass;
            if (bodies_found > 0 && !all_found)
                text += " | " + bodies_found + " bodies found.".T(EDTx.UserControlSurveyor_bodiesfound);
            if (all_found )
                text += " | " + "System scan complete".T(EDTx.UserControlSurveyor_Systemscancomplete) + " " + bodies_found + " bodies found.".T(EDTx.UserControlSurveyor_bodiesfound);

            SetControlText(text);

            extPictureBoxTitle.ClearImageList();
            var i = new ExtPictureBox.ImageElement();

            StringFormat frmt = new StringFormat(extCheckBoxWordWrap.Checked ? 0 : StringFormatFlags.NoWrap);
            frmt.Alignment = alignment;
            var textcolour = IsTransparent ? ExtendedControls.Theme.Current.SPanelColor : ExtendedControls.Theme.Current.LabelColor;
            var backcolour = IsTransparent ? Color.Transparent : this.BackColor;
            Font dfont = displayfont ?? this.Font;

            i.TextAutoSize(
                    new Point(3, 0),
                    new Size(2000, 10000),
                    text,
                    dfont,
                    textcolour,
                    backcolour,
                    1.0F,
                    frmt: frmt);
            extPictureBoxTitle.Add(i);
            extPictureBoxTitle.Render();
        }

        async private void DrawSystem(ISystem sys)
        {
            var picelements = new List<ExtPictureBox.ImageElement>();       // accumulate picture elements in here and render under lock due to async below.

            if ( sys != null )      // if we have a system
            {
                int vpos = 0;

                StringFormat frmt = new StringFormat(extCheckBoxWordWrap.Checked ? 0 : StringFormatFlags.NoWrap);
                frmt.Alignment = alignment;
                var textcolour = IsTransparent ? ExtendedControls.Theme.Current.SPanelColor : ExtendedControls.Theme.Current.LabelColor;
                var backcolour = IsTransparent ? Color.Transparent : this.BackColor;
                Font dfont = displayfont ?? this.Font;

                if (currentRoute != null && sys.HasCoordinate)      // if we have a route and a coord, we can work out the next route text
                {
                    //System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} Current route set, sys has coord, route is {currentRoute.Systems.Count} {currentRoute.Id}");
                    if (currentRoute.Systems.Count == 0)
                    {
                        lastroutetext = "Route contains no waypoints".T(EDTx.UserControlRouteTracker_NoWay);
                    }
                    else
                    {
                        SavedRouteClass.ClosestInfo closest = currentRoute.ClosestTo(sys);
                        if (closest == null)  // if null, no systems found.. uh oh
                        {
                            lastroutetext = "No systems in route have known co-ords".T(EDTx.UserControlRouteTracker_NoCo);
                        }
                        else
                        {
                            double routedistance = currentRoute.CumulativeDistance();       // total distance

                            double distleft = closest.disttowaypoint + (closest.deviation < 0 ? 0 : closest.cumulativewpdist);

                            //System.Diagnostics.Debug.WriteLine($"Surveyor: {closest.lastsystem?.Name}->{closest.nextsystem?.Name} {closest.waypoint} distance to {closest.disttowaypoint} dev {closest.deviation} cuml after wp {closest.cumulativewpdist} inc wp {distleft} route {routedistance}");

                            lastroutetext = $"{currentRoute.Name} {currentRoute.Systems.Count} WPs, {routedistance:N1}ly -> {closest.finalsystem.Name}";

                            string jumpmsg = "";
                            if (IsSet(RouteControl.showJumps))
                            {
                                if (shipfsdinfo != null)
                                {
                                    // navroutes are precomputed, so the total jump count is from it. Else do it by distance
                                    int jumps = currentRoute.Id != -1 ? (int)Math.Ceiling(routedistance / shipfsdinfo.avgsinglejump) : currentRoute.Systems.Count - 1;
                                    if (jumps > 0)
                                        lastroutetext += ", " + jumps.ToString() + " " + ((jumps == 1) ? "jump".T(EDTx.UserControlRouteTracker_J1) : "jumps".T(EDTx.UserControlRouteTracker_JS));

                                    jumps = (int)Math.Ceiling(closest.disttowaypoint / shipfsdinfo.avgsinglejump);

                                    if (jumps > 1 && currentRoute.Id != -1) // if more than 1 jump, and not nav route
                                        jumpmsg = ", " + jumps.ToString() + " " + ((jumps == 1) ? "jump".T(EDTx.UserControlRouteTracker_J1) : "jumps".T(EDTx.UserControlRouteTracker_JS));
                                }
                                else
                                    jumpmsg = " No Ship FSD Information".T(EDTx.UserControlRouteTracker_NoFSD);
                            }

                            string wpposmsg = "";
                            if (IsSet(RouteControl.showwaypoints))
                                wpposmsg = String.Format(" @{0:N1},{1:N1},{2:N1}", closest.nextsystem.X, closest.nextsystem.Y, closest.nextsystem.Z);

                            if (closest.deviation < 0)        // if not on path
                            {
                                lastroutetext += Environment.NewLine;
                                lastroutetext += closest.cumulativewpdist == 0 ? "From Last WP ".T(EDTx.UserControlRouteTracker_FL) : "To First WP ".T(EDTx.UserControlRouteTracker_TF);
                                lastroutetext += $" >> {closest.disttowaypoint:N1}ly >> " + closest.nextsystem.Name + wpposmsg + jumpmsg;
                            }
                            else
                            {
                                lastroutetext += String.Format(", Left {0:N1}ly".T(EDTx.UserControlRouteTracker_LF), distleft) + Environment.NewLine;
                                if (distleft == 0)
                                    lastroutetext += $"{closest.nextsystem.Name}";
                                else
                                    lastroutetext += $"{closest.lastsystem?.Name} >> {closest.disttowaypoint:N1}ly >> {closest.nextsystem.Name}";
                                lastroutetext += " " + String.Format("(WP {0})".T(EDTx.UserControlRouteTracker_ToWP), closest.waypoint + 1);
                                lastroutetext += wpposmsg + jumpmsg;

                                if (IsSet(RouteControl.showdeviation))
                                    lastroutetext += String.Format(", Dev {0:N1}ly".T(EDTx.UserControlRouteTracker_Dev), closest.deviation);
                            }

                            //System.Diagnostics.Debug.WriteLine(lastroutetext);
                            //System.Diagnostics.Debug.WriteLine("---");

                            if (IsSet(RouteControl.showbookmarks))
                            {
                                BookmarkClass bookmark = GlobalBookMarkList.Instance.FindBookmarkOnSystem(sys.Name);
                                if (bookmark != null)
                                    lastroutetext += Environment.NewLine + String.Format("Note: {0}".T(EDTx.UserControlRouteTracker_Note), bookmark.Note);
                            }

                            string name = closest.nextsystem.Name;

                            if (lastsystemroute == null || name.CompareTo(lastsystemroute) != 0)
                            {
                                if (IsSet(RouteControl.autocopy))
                                    SetClipboardText(name);

                                if (IsSet(RouteControl.settarget))
                                {
                                    string targetName;
                                    double x, y, z;
                                    TargetClass.GetTargetPosition(out targetName, out x, out y, out z);
                                    if (name.CompareTo(targetName) != 0)
                                        TargetHelpers.SetTargetSystem(this, discoveryform, name, false);
                                }

                                lastsystemroute = name;
                            }
                        }
                    }

                    if (IsSet(RouteControl.showtarget))
                    {
                        if (TargetClass.GetTargetPosition(out string name, out Point3D tpos))
                        {
                            double dist = last_sys.Distance(tpos.X, tpos.Y, tpos.Z);

                            string jumpstr = "";
                            if (shipfsdinfo != null)
                            {
                                int jumps = (int)Math.Ceiling(dist / shipfsdinfo.avgsinglejump);
                                if (jumps > 0)
                                    jumpstr = jumps.ToString() + " " + ((jumps == 1) ? "jump".T(EDTx.UserControlRouteTracker_J1) : "jumps".T(EDTx.UserControlRouteTracker_JS));
                            }

                            lastroutetext = lastroutetext.AppendPrePad($"T-> {name} {dist:N1}ly {jumpstr}", Environment.NewLine);
                        }
                    }
                }
                else
                {
                    //System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} can't do route '{currentRoute?.Name}' {sys.HasCoordinate}");
                }

                string routelinefueltext = lastroutetext;       // lastroutetext may not have changed, we need to take a copy as we may go thru fuel many times before it gets updated again

                if (IsSet(RouteControl.showfuel) && shipinfo != null)
                {
                    double fuel = shipinfo.FuelLevel;
                    double tanksize = shipinfo.FuelCapacity;
                    double warninglevelpercent = shipinfo.FuelWarningPercent;

                    string addtext = $"{fuel:N1}/{tanksize:N1}t";

                    if (warninglevelpercent > 0 && fuel < tanksize * warninglevelpercent / 100.0)
                    {
                        addtext += $" < {warninglevelpercent:N1}%";
                    }

                    if (shipfsdinfo != null)
                    {
                        addtext += string.Format(" " + "Avg {0:N1}ly Fume {1:N1}ly Range {2:N1}ly".T(EDTx.UserControlSurveyor_fuel), shipfsdinfo.avgsinglejump, shipfsdinfo.curfumessinglejump, shipfsdinfo.maxjumprange);
                    }

                    routelinefueltext = routelinefueltext.AppendPrePad(addtext, Environment.NewLine);
                }


                //System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} last route text '{routelinefueltext}'");

                if (routelinefueltext.HasChars())     // if we have any characters, display it
                {
                    var i = new ExtPictureBox.ImageElement();
                    i.TextAutoSize(
                            new Point(3, vpos),
                            new Size(Math.Max(pictureBoxSurveyor.Width - 6, 24), 10000),
                            routelinefueltext,
                            dfont,
                            textcolour,
                            backcolour,
                            1.0F,
                            frmt: frmt);
                    picelements.Add(i);

                    vpos += i.Location.Height;
                }

                // now perform searches, and store them in searchresults, keyed by body name matched
               
                Dictionary<string, HistoryListQueries.Results> searchresults = new Dictionary<string, HistoryListQueries.Results>();

                if (searchesactive.Length > 0)       // if any searches
                {
                    discoveryform.history.FillInScanNode();     // ensure all journal scan entries point to a scan node (expensive, done only when required in this panel)

                    // all entries related to sys.  Can't really limit the pick up as tried before using the afterlastevent option in this call
                    // due to being able to browse back in history. We may not be at the end of the list the system we are displaying. For now, just do a blind whole history search

                    var helist = HistoryList.FilterByEventEntryOrder(discoveryform.history.EntryOrder(), HistoryListQueries.AllSearchableJournalTypes, sys);

                    if (helist.Count > 0)        // no point executing if nothing in helist
                    {
                        var defaultvars = new BaseUtils.Variables();
                        defaultvars.AddPropertiesFieldsOfClass(new BodyPhysicalConstants(), "", null, 10, ensuredoublerep: true);

                        System.Diagnostics.Debug.WriteLine($"{Environment.TickCount} Surveyor runs {searchesactive.Length} searches");
                        foreach (var searchname in searchesactive)
                        {
                            // await is horrible, anything can happen, even closing
                            await HistoryListQueries.Instance.Find(helist, searchresults, searchname, defaultvars, discoveryform.history.StarScan, false); // execute the searches

                            if (IsClosed)       // if we was ordered to close, abore
                                return;
                        }

                        System.Diagnostics.Debug.WriteLine($"{Environment.TickCount} Surveyor reports {searchresults.Count} results");
                    }
                }

                // we store strings in textresults, sorted by its body name, and accumulate them during nodes and finally render them search results
                // this is because we have two goes at filling in text, nodes and remaining search results, but we want them alpha sorted

                SortedList<string, string> textresults = new SortedList<string, string>(new CollectionStaticHelpers.AlphaIntCompare<string>());      // we dump all test into this sorted dictionary

                // find if we have system nodes

                StarScan.SystemNode systemnode = await discoveryform.history.StarScan.FindSystemAsync(sys, checkBoxEDSM.Checked);        // get data with EDSM
                if (IsClosed)   // may close during await..
                    return;

                long value = 0;     // accumulate value if required of shown bodies

                if (systemnode != null)     // if we have a node (should do of course since july 22 due to AddLocation in scan node)
                {
                    string infoline = "";

                    int scanned = checkBoxEDSM.Checked ? systemnode.StarPlanetsScanned() : systemnode.StarPlanetsScannednonEDSM();

                    if (scanned > 0)
                    {
                        infoline = "Scan".T(EDTx.UserControlSurveyor_Scan) + " " + scanned.ToString() + (systemnode.FSSTotalBodies != null ? (" / " + systemnode.FSSTotalBodies.Value.ToString()) : "");
                    }

                    value = systemnode.ScanValue(false);

                    if (value > 0 && IsSet(CtrlList.showValues))
                    {
                        infoline = infoline.AppendPrePad("~ " + value.ToString("N0") + " cr", "; ");
                    }

                    if (infoline.HasChars())
                    {
                        //System.Diagnostics.Debug.WriteLine("Draw " + infoline);
                        var i = new ExtPictureBox.ImageElement();
                        i.TextAutoSize(
                            new Point(3, vpos),
                            new Size(Math.Max(pictureBoxSurveyor.Width - 6, 24), 10000),
                            infoline,
                            dfont,
                            textcolour,
                            backcolour,
                            1.0F,
                            frmt: frmt);
                        picelements.Add(i);
                        vpos += i.Location.Height;
                    }

                    value = 0;

                    foreach (StarScan.ScanNode sn in systemnode.Bodies.EmptyIfNull().Where(x=>x.ScanData!=null))        // only nodes with scan data can be treated here
                    {
                        var sd = sn.ScanData;

                        searchresults.TryGetValue(sn.FullName, out HistoryListQueries.Results searchresultfornode);     // will be null if not found

                        var surveyordisplay = searchresultfornode != null;      // if we have a search node, display

                        bool hasgeosignals = sn.CountGeoSignals>0;
                        bool hasbiosignals = sn.CountBioSignals>0;
                        bool hasthargoidsignals = sn.CountThargoidSignals > 0;
                        bool hasguardiansignals = sn.CountGuardianSignals > 0;
                        bool hashumansignals = sn.CountHumanSignals > 0;
                        bool hasothersignals = sn.CountOtherSignals > 0;
                        bool hasminingsignals = sn.CountUncategorisedSignals > 0;

                        bool matchedgeosignals = hasgeosignals && IsSet(CtrlList.GeoSignals);           // these are filters, to determine if to display
                        bool matchedbiosignals = hasbiosignals && IsSet(CtrlList.BioSignals);
                        bool matchedthargoidsignals = hasthargoidsignals && IsSet(CtrlList.signals);
                        bool matchedguardiansignals = hasguardiansignals && IsSet(CtrlList.signals);
                        bool matchedhumansignals = hashumansignals && IsSet(CtrlList.signals);
                        bool matchedothersignals = hasothersignals && IsSet(CtrlList.signals);
                        bool matchedminingsignals = hasminingsignals && IsSet(CtrlList.signals);

                        bool matchedlandablewithatmosphere = sd.IsLandable && sd.HasAtmosphericComposition && IsSet(CtrlList.isLandableWithAtmosphere);
                        bool matchedlandablevolcanism =  sd.IsLandable && sd.HasMeaningfulVolcanism && IsSet(CtrlList.isLandableWithVolcanism);
                        bool matchedvolcanism = sd.HasMeaningfulVolcanism && IsSet(CtrlList.showVolcanism);

                        if ( surveyordisplay == false && (!sd.IsEDSMBody || checkBoxEDSM.Checked)) // if to perform inbuilt checks - must have scan data to do this
                        {
                            // work out if we want to display
                            surveyordisplay = (sd.IsLandable && IsSet(CtrlList.isLandable)) ||
                                matchedlandablewithatmosphere ||
                                matchedlandablevolcanism ||
                                (sd.IsLandable && sd.nRadius.HasValue && sd.nRadius >= largeRadiusLimit && IsSet(CtrlList.largelandable)) ||
                                (sd.AmmoniaWorld && IsSet(CtrlList.showAmmonia)) ||
                                (sd.Earthlike && IsSet(CtrlList.showEarthlike)) ||
                                (sd.WaterWorld && IsSet(CtrlList.showWaterWorld)) ||
                                (sd.PlanetTypeID == EDPlanet.High_metal_content_body && IsSet(CtrlList.showHMC)) ||
                                (sd.PlanetTypeID == EDPlanet.Metal_rich_body && IsSet(CtrlList.showMR)) ||
                                (sd.HasRingsOrBelts && IsSet(CtrlList.showRinged)) ||
                                matchedvolcanism ||
                                (sd.nEccentricity.HasValue && sd.nEccentricity >= eccentricityLimit && IsSet(CtrlList.showEccentricity)) ||
                                (sd.CanBeTerraformable && IsSet(CtrlList.showTerraformable)) ||
                                (sd.IsPlanet && IsSet(CtrlList.lowradius) && sd.nRadius.HasValue && sd.nRadius < lowRadiusLimit) ||
                                matchedgeosignals || matchedbiosignals || matchedthargoidsignals || matchedguardiansignals || matchedhumansignals || matchedothersignals || matchedminingsignals ||
                                (sd.IsStar && IsSet(CtrlList.allstars)) ||
                                (sd.IsPlanet && IsSet(CtrlList.allplanets)) ||
                                (sd.IsBeltCluster && IsSet(CtrlList.beltclusters));

                            // qualify choice by mapped
                            surveyordisplay &= !sd.Mapped || IsSet(CtrlList.hideMapped) == false;
                        }

                        if ( surveyordisplay )
                        {
                            var silstring = sd.SurveyorInfoLine(sys,
                                    matchedminingsignals || (hasminingsignals && IsSet(CtrlList.showsignals)), 
                                    matchedgeosignals || (hasgeosignals && IsSet(CtrlList.showsignals)), 
                                    matchedbiosignals || (hasbiosignals && IsSet(CtrlList.showsignals)), 
                                    matchedthargoidsignals || (hasthargoidsignals && IsSet(CtrlList.showsignals)),
                                    matchedguardiansignals || (hasguardiansignals && IsSet(CtrlList.showsignals)),
                                    matchedhumansignals || (hashumansignals && IsSet(CtrlList.showsignals)),
                                    matchedothersignals || (hasothersignals && IsSet(CtrlList.showsignals)),
                                    false,      // so this is the surveyor, we don't want to bother with showing if its got organics, since you have to scan them
                                    IsSet(CtrlList.volcanism) || matchedlandablevolcanism || matchedvolcanism, // any of these causes a show
                                    IsSet(CtrlList.showValues),        // show values
                                    IsSet(CtrlList.moreinfo),   // show extra info such as mass/radius
                                    IsSet(CtrlList.showGravity),       // show gravity select
                                    IsSet(CtrlList.atmos) || matchedlandablewithatmosphere, // show atmosphere if landable (surveyor shows this if landable)
                                    IsSet(CtrlList.showRinged),          // show rings
                                    lowRadiusLimit, largeRadiusLimit, eccentricityLimit);

                                if (searchresultfornode!= null) // if search results are set for this body, add text
                                {
                                    searchresults.Remove(sn.FullName);  // we have processed it, finish
                                    string info = string.Join(", ", searchresultfornode.FiltersPassed);
                                    silstring += " : " + info;
                                }

                                textresults[sd.BodyName] = silstring;

                                value += sd.EstimatedValue;
                        }

                    }   // end for..
                }       // end of system node look thru

                // we may have searches without scan nodes, so present
                        
                foreach (var kvp in searchresults.EmptyIfNull())            // by bodyname
                {
                    string info = string.Join(", ", kvp.Value.FiltersPassed);
                    string bodyname = kvp.Key;
                    textresults[bodyname] = $"{bodyname.ReplaceIfStartsWith(sys.Name)}: {info}";
                }

                foreach (var kvp in textresults)        // and present any text results in sorted order
                {
                    var i = new ExtPictureBox.ImageElement();
                    i.TextAutoSize(
                            new Point(3, vpos),
                            new Size(Math.Max(pictureBoxSurveyor.Width - 6, 24), 10000),
                            kvp.Value,
                            dfont,
                            textcolour,
                            backcolour,
                            1.0F,
                            frmt: frmt);
                    picelements.Add(i);
                    vpos += i.Location.Height;
                }

                if (value > 0 && IsSet(CtrlList.showValues))
                {
                    var i = new ExtPictureBox.ImageElement();
                    i.TextAutoSize(
                        new Point(3, vpos),
                        new Size(Math.Max(pictureBoxSurveyor.Width - 6, 24), 10000),
                        "^^ ~ " + value.ToString("N0") + " cr",
                        dfont,
                        textcolour,
                        backcolour,
                        1.0F,
                        frmt: frmt);
                    picelements.Add(i);
                    vpos += i.Location.Height;
                }

                if (systemnode != null && fsssignalsdisplayed.HasChars())
                {
                    string siglist = "";
                    string[] filter = fsssignalsdisplayed.Split(';');

                    var signallist = JournalFSSSignalDiscovered.SignalList(systemnode.FSSSignalList);

                    // mirrors scandisplaynodes

                    var notexpired = signallist.Where(x => !x.TimeRemaining.HasValue || x.ExpiryUTC >= DateTime.UtcNow).ToList();
                    notexpired.Sort(delegate (JournalFSSSignalDiscovered.FSSSignal l, JournalFSSSignalDiscovered.FSSSignal r) { return l.ClassOfSignal.CompareTo(r.ClassOfSignal); });

                    var expired = signallist.Where(x => x.TimeRemaining.HasValue && x.ExpiryUTC < DateTime.UtcNow).ToList();
                    expired.Sort(delegate (JournalFSSSignalDiscovered.FSSSignal l, JournalFSSSignalDiscovered.FSSSignal r) { return r.ExpiryUTC.CompareTo(l.ExpiryUTC); });

                    int expiredpos = notexpired.Count;
                    notexpired.AddRange(expired);

                    int pos = 0;
                    foreach (var fsssig in notexpired)
                    {
                        if (filter.ComparisionContains(fsssig.SignalName.Alt("!~~"), StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                            filter.ComparisionContains(fsssig.SignalName_Localised.Alt("!~~"), StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                            filter.ComparisionContains(fsssig.SpawningState_Localised.Alt("!~~"), StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                            filter.ComparisionContains(fsssig.SpawningFaction_Localised.Alt("!~~"), StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                            filter.ComparisionContains(fsssig.USSTypeLocalised.Alt("!~~"), StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                            filter.ComparisionContains(fsssig.ClassOfSignal.ToString(), StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                            fsssignalsdisplayed.Equals("*"))
                        {
                            if (pos++ == expiredpos)
                                siglist = siglist.AppendPrePad("Expired:".T(EDTx.UserControlScan_Expired), Environment.NewLine + Environment.NewLine);

                            siglist = siglist.AppendPrePad(fsssig.ToString(true), Environment.NewLine);
                        }
                    }

                    if (siglist.HasChars())
                    {
                        //System.Diagnostics.Debug.WriteLine("Display " + siglist);
                        var i = new ExtPictureBox.ImageElement();
                        i.TextAutoSize(new Point(3, vpos),
                                                        new Size(Math.Max(pictureBoxSurveyor.Width - 6, 24), 10000),
                                                        siglist,
                                                        dfont,
                                                        textcolour,
                                                        backcolour,
                                                        1.0F,
                                                        frmt: frmt);
                        vpos += i.Location.Height;
                        picelements.Add(i);
                    }

                }

                frmt.Dispose();
            }       // end sys

            lock ( extPictureBoxScroll)      // because of the async call above, we may be running two of these at the same time. So, we lock and then add/update/render
            {
                pictureBoxSurveyor.ClearImageList();
                System.Diagnostics.Debug.WriteLine($"Surveyor draw system {displaynumber} no {picelements.Count}");
                pictureBoxSurveyor.AddRange(picelements); 
                extPictureBoxScroll.Render();
                Refresh();
            }
        }

        #endregion

        #region UI
        protected enum CtrlList
        {
            allplanets, showAmmonia, showEarthlike, showWaterWorld, showHMC, showMR,
            showTerraformable, showVolcanism, showRinged, showEccentricity, lowradius,
            signals, GeoSignals, BioSignals, isLandable, isLandableWithAtmosphere,
            largelandable, isLandableWithVolcanism,
            // 18
            allstars, beltclusters,
            // 20
            showValues, moreinfo, showGravity, atmos, volcanism, showsignals, autohide, donthidefssmode, hideMapped, showsysinfo, showstarclass,
            // 30
            alignleft, aligncenter, alignright
        };

        private bool[] ctrlset; // holds current state of each control above

        private bool IsSet(CtrlList v)
        {
            return ctrlset[(int)v];
        }

        private string[] searchesactive;

        // from DB, set up ctrlset, and set the defaults
        private void PopulateCtrlList()
        {
            ctrlset = GetSettingAsCtrlSet<CtrlList>(DefaultSetting);
            alignment = ctrlset[(int)CtrlList.alignright] ? StringAlignment.Far : ctrlset[(int)CtrlList.aligncenter] ? StringAlignment.Center : StringAlignment.Near;
            searchesactive = GetSetting("Searches", "").SplitNoEmptyStartFinish('\u2188');
        }

        protected virtual bool DefaultSetting(CtrlList e)
        {
            bool def = (e != CtrlList.alignright && e != CtrlList.aligncenter && e != CtrlList.autohide && e != CtrlList.lowradius
                && e != CtrlList.allplanets && e != CtrlList.allstars && e != CtrlList.beltclusters);
            return def;
        }

        private void extButtonPlanets_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();

            displayfilter.AddAllNone();
            displayfilter.AddStandardOption(CtrlList.allplanets.ToString(), "Show All Planets".TxID(EDTx.UserControlSurveyor_showAllPlanetsToolStripMenuItem), BaseUtils.Icons.IconSet.GetIcon("Bodies.Planets.Terrestrial.HMCv10"));
            displayfilter.AddStandardOption(CtrlList.showAmmonia.ToString(), "Ammonia World".TxID(EDTx.UserControlSurveyor_planetaryClassesToolStripMenuItem_ammoniaWorldToolStripMenuItem), BaseUtils.Icons.IconSet.GetIcon("Bodies.Planets.Terrestrial.AMWv1"));
            displayfilter.AddStandardOption(CtrlList.showEarthlike.ToString(), "Earthlike World".TxID(EDTx.UserControlSurveyor_planetaryClassesToolStripMenuItem_earthlikeWorldToolStripMenuItem), BaseUtils.Icons.IconSet.GetIcon("Bodies.Planets.Terrestrial.ELWv5"));
            displayfilter.AddStandardOption(CtrlList.showWaterWorld.ToString(), "Water World".TxID(EDTx.UserControlSurveyor_planetaryClassesToolStripMenuItem_waterWorldToolStripMenuItem), BaseUtils.Icons.IconSet.GetIcon("Bodies.Planets.Terrestrial.WTRv7"));
            displayfilter.AddStandardOption(CtrlList.showHMC.ToString(), "High metal content body".TxID(EDTx.UserControlSurveyor_planetaryClassesToolStripMenuItem_highMetalContentBodyToolStripMenuItem), BaseUtils.Icons.IconSet.GetIcon("Bodies.Planets.Terrestrial.HMCv3"));
            displayfilter.AddStandardOption(CtrlList.showMR.ToString(), "Metal-rich body".TxID(EDTx.UserControlSurveyor_planetaryClassesToolStripMenuItem_metalToolStripMenuItem), BaseUtils.Icons.IconSet.GetIcon("Bodies.Planets.Terrestrial.MRBv"));
            displayfilter.AddStandardOption(CtrlList.showTerraformable.ToString(), "Terraformable".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_terraformableToolStripMenuItem), BaseUtils.Icons.IconSet.GetIcon("Bodies.Planets.Terrestrial.ELWv5"));
            displayfilter.AddStandardOption(CtrlList.showVolcanism.ToString(), "Has volcanism".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_hasVolcanismToolStripMenuItem), BaseUtils.Icons.IconSet.GetIcon("Bodies.Planets.Terrestrial.HMCv37"));
            displayfilter.AddStandardOption(CtrlList.showRinged.ToString(), "Has Rings".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_hasRingsToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_RingOnly);
            displayfilter.AddStandardOption(CtrlList.showEccentricity.ToString(), "High eccentricity".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_highEccentricityToolStripMenuItem), global::EDDiscovery.Icons.Controls.Eccentric);
            displayfilter.AddStandardOption(CtrlList.lowradius.ToString(), "Tiny body".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_lowRadiusToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_SizeSmall);
            displayfilter.AddStandardOption(CtrlList.signals.ToString(), "Has Signals".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_hasSignalsToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_Signals);
            displayfilter.AddStandardOption(CtrlList.GeoSignals.ToString(), "Has geological signals".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_hasGeologicalSignalsToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_Signals);
            displayfilter.AddStandardOption(CtrlList.BioSignals.ToString(), "Has biological signals".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_hasBiologicalSignalsToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_Signals);
            displayfilter.AddStandardOption(CtrlList.isLandable.ToString(), "Landable".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_landableToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_LandablePlanet);
            displayfilter.AddStandardOption(CtrlList.isLandableWithAtmosphere.ToString(), "Landable with atmosphere".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_landableWithAtmosphereToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_LandablePlanet);
            displayfilter.AddStandardOption(CtrlList.largelandable.ToString(), "Landable and large".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_landableAndLargeToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_LandablePlanet);
            displayfilter.AddStandardOption(CtrlList.isLandableWithVolcanism.ToString(), "Landable with volcanism".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_landableWithVolcanismToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_LandablePlanet);

            CommonCtrl(displayfilter, extButtonPlanets);

        }

        private void extButtonSearches_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();
            displayfilter.AddAllNone();
            displayfilter.AddGroupOption(HistoryListQueries.DefaultSearches, "Default".T(EDTx.ProfileEditor_Default));
            displayfilter.SettingsSplittingChar = '\u2188';     // pick a crazy one soe

            var searches = HistoryListQueries.Instance.Searches.Where(x => x.UserOrBuiltIn).ToList();
            foreach (var s in searches)
                displayfilter.AddStandardOption(s.Name, s.Name);

            CommonCtrl(displayfilter, extButtonSearches, "Searches");
        }

        private void extButtonStars_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();

            displayfilter.AddAllNone();
            displayfilter.AddStandardOption(CtrlList.allstars.ToString(), "Show All Stars".TxID(EDTx.UserControlSurveyor_showAllStarsToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.beltclusters.ToString(), "Show Belt Clusters".TxID(EDTx.UserControlSurveyor_showBeltClustersToolStripMenuItem), global::EDDiscovery.Icons.Controls.Belt);

            CommonCtrl(displayfilter, extButtonStars);
        }

        private void extButtonShowControl_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();

            displayfilter.AddAllNone();
            displayfilter.AddStandardOption(CtrlList.showstarclass.ToString(), "Show star class".TxID(EDTx.UserControlSurveyor_showstarclassToolStripMenuItem));
            displayfilter.AddStandardOption(CtrlList.showValues.ToString(), "Show values".TxID(EDTx.UserControlSurveyor_showValuesToolStripMenuItem));
            displayfilter.AddStandardOption(CtrlList.moreinfo.ToString(), "Show more information".TxID(EDTx.UserControlSurveyor_showMoreInformationToolStripMenuItem));
            displayfilter.AddStandardOption(CtrlList.showGravity.ToString(), "Show gravity of landables".TxID(EDTx.UserControlSurveyor_showGravityToolStripMenuItem));
            displayfilter.AddStandardOption(CtrlList.atmos.ToString(), "Show atmospheres".TxID(EDTx.UserControlSurveyor_showAtmosToolStripMenuItem));
            displayfilter.AddStandardOption(CtrlList.volcanism.ToString(), "Show volcanism".TxID(EDTx.UserControlSurveyor_showVolcanismToolStripMenuItem));
            displayfilter.AddStandardOption(CtrlList.showsignals.ToString(), "Show signals".TxID(EDTx.UserControlSurveyor_showSignalsToolStripMenuItem));
            displayfilter.AddStandardOption(CtrlList.autohide.ToString(), "Auto Hide".TxID(EDTx.UserControlSurveyor_autoHideToolStripMenuItem));
            displayfilter.AddStandardOption(CtrlList.donthidefssmode.ToString(), "Don't hide in FSS Mode".TxID(EDTx.UserControlSurveyor_dontHideInFSSModeToolStripMenuItem));
            displayfilter.AddStandardOption(CtrlList.hideMapped.ToString(), "Hide already mapped bodies".TxID(EDTx.UserControlSurveyor_hideAlreadyMappedBodiesToolStripMenuItem));
            displayfilter.AddStandardOption(CtrlList.showsysinfo.ToString(), "Show System Info Always".TxID(EDTx.UserControlSurveyor_showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem));

            CommonCtrl(displayfilter, extButtonShowControl);
        }

        private void extButtonAlignment_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();

            string lt = CtrlList.alignleft.ToString();
            string ct = CtrlList.aligncenter.ToString();
            string rt = CtrlList.alignright.ToString();

            displayfilter.AddStandardOption(lt, "Alignment Left".TxID(EDTx.UserControlSurveyor_textAlignToolStripMenuItem_leftToolStripMenuItem), global::EDDiscovery.Icons.Controls.AlignLeft, exclusivetags: ct + ";" + rt, disableuncheck: true);
            displayfilter.AddStandardOption(ct, "Alignment Center".TxID(EDTx.UserControlSurveyor_textAlignToolStripMenuItem_centerToolStripMenuItem), global::EDDiscovery.Icons.Controls.AlignCentre, exclusivetags: lt + ";" + rt, disableuncheck: true);
            displayfilter.AddStandardOption(rt, "Alignment Right".TxID(EDTx.UserControlSurveyor_textAlignToolStripMenuItem_rightToolStripMenuItem), global::EDDiscovery.Icons.Controls.AlignRight, exclusivetags: lt + ";" + ct, disableuncheck: true);
            displayfilter.CloseOnChange = true;
            CommonCtrl(displayfilter, extButtonAlignment);
        }

        private void CommonCtrl(ExtendedControls.CheckedIconListBoxFormGroup displayfilter, Control under, string saveasstring = null)
        {
            displayfilter.CloseBoundaryRegion = new Size(32, under.Height);
            displayfilter.AllOrNoneBack = false;
            displayfilter.ImageSize = new Size(24, 24);
            displayfilter.ScreenMargin = new Size(0, 0);

            displayfilter.SaveSettings = (s, o) =>
            {
                if (saveasstring == null)
                    PutBoolSettingsFromString(s, displayfilter.SettingsTagList());
                else
                    PutSetting(saveasstring, s);

                PopulateCtrlList();
                SetVisibility();
                DrawSystem(last_sys);
                SetTitle();
            };

            if ( saveasstring == null)
                displayfilter.Show(typeof(CtrlList), ctrlset, under, this.FindForm());
            else
                displayfilter.Show(GetSetting(saveasstring,""), under, this.FindForm());
        }

        private void extButtonFont_Click(object sender, EventArgs e)
        {
            Font f = FontHelpers.FontSelection(this.FindForm(), displayfont ?? this.Font);     // will be null on cancel
            string setting = FontHelpers.GetFontSettingString(f);
            //System.Diagnostics.Debug.WriteLine($"Surveyor Font selected {setting}");
            PutSetting("font", setting);
            displayfont = f;
            DrawSystem(last_sys);
        }

        private void checkBoxEDSM_Clicked(object sender, EventArgs e)
        {
            PutSetting("edsm", checkBoxEDSM.Checked);
            DrawSystem(last_sys);
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("wordwrap", extCheckBoxWordWrap.Checked);
            DrawSystem(last_sys);
        }

        private void extButtonFSS_Click(object sender, EventArgs e)
        {
            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();

            int width = 430;

            f.Add(new ExtendedControls.ConfigurableForm.Entry("Text", typeof(ExtendedControls.ExtTextBox), fsssignalsdisplayed, new Point(10, 40), new Size(width - 10 - 20, 110), "List Names to show") { textboxmultiline = true });

            f.AddOK(new Point(width - 100, 180));
            f.AddCancel(new Point(width - 200, 180));

            f.Trigger += (dialogname, controlname, tag) =>
            {
                if (controlname == "OK")
                {
                    f.ReturnResult(DialogResult.OK);
                }
                else if (controlname == "Cancel" || controlname == "Close")
                {
                    f.ReturnResult(DialogResult.Cancel);
                }
            };

            DialogResult res = f.ShowDialogCentred(this.FindForm(), this.FindForm().Icon, "List signals to display, semicolon seperated".T(EDTx.UserControlSurveyor_fsssignals), closeicon: true);
            if (res == DialogResult.OK)
            {
                fsssignalsdisplayed = f.Get("Text");
                PutSetting("fsssignals", fsssignalsdisplayed);
                DrawSystem(last_sys);
            }
        }

        #endregion

        #region Route control

        ExtendedControls.ExtListBoxForm dropdown;
        private void extButtonSetRoute_Click(object sender, EventArgs e)
        {
            ExtendedControls.ExtButton but = sender as ExtendedControls.ExtButton;

            dropdown = new ExtendedControls.ExtListBoxForm("", true);

            var savedroutes = SavedRouteClass.GetAllSavedRoutes();

            dropdown.FitImagesToItemHeight = true;
            var list = savedroutes.Select(x => x.Name).ToList();
            list.Insert(0, "Off".T(EDTx.Off));
            list.Insert(1, translatednavroutename);
            dropdown.Items = list;
            dropdown.FlatStyle = FlatStyle.Popup;
            dropdown.PositionBelow(sender as Control);
            dropdown.SelectedIndexChanged += (s, ea) =>
            {
                if (dropdown.SelectedIndex == 0)    // off
                {
                    LoadRoute("");
                }
                else if (dropdown.SelectedIndex == 1)      // navroute
                {
                    LoadRoute(NavRouteNameLabel);
                }
                else
                {
                    string name = savedroutes[dropdown.SelectedIndex - 2].Name;
                    LoadRoute(name);
                }

                DrawSystem(last_sys);
            };

            ExtendedControls.Theme.Current.ApplyDialog(dropdown, true);
            dropdown.Show(this.FindForm());
        }

        private void LoadRoute(string name)
        {
            //System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} Order load of route '{name}'");
            PutSetting("route", name);      // store back the current name 

            //System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} In DB its now '{GetSetting("route","???")}'");

            currentRoute = null;
            lastroutetext = "";       // reset the route text stored due to start jump not having coords

            if (name.HasChars())
            {
                if (name.Equals(NavRouteNameLabel))
                {
                    var route = discoveryform.history.GetLastHistoryEntry(x => x.EntryType == JournalTypeEnum.NavRoute)?.journalEntry as EliteDangerousCore.JournalEvents.JournalNavRoute;
                    if (route?.Route != null)
                    {
                        var systems = route.Route.Where(x => x.StarSystem.HasChars()).Select(y => y.StarSystem).ToArray();
                        currentRoute = new SavedRouteClass(translatednavroutename, systems);      // with an ID of -1 note
                        //System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} Loaded Nav route with {systems.Length}");
                    }
                    else
                    {
                        currentRoute = new SavedRouteClass(translatednavroutename, new string[] { });     // no known systems yet, but make a navroute so we have it selected
                        //System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} No route available, loaded empty Nav route");

                    }
                }
                else
                {
                    var savedroutes = SavedRouteClass.GetAllSavedRoutes();      // load routes
                    currentRoute = savedroutes.Find(x => x.Name == name);       // pick, if not found, will be null
                    //System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} Loaded route with {currentRoute?.Systems.Count}");
                }
            }
            else
            {
                //System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} No route wanted '{name}'");
            }
        }

        private string routecontrolsettings;

        enum RouteControl
        {
            showJumps,
            showwaypoints,
            showdeviation,
            showbookmarks,
            autocopy,
            settarget,
            showtarget,
            showfuel,
        };

        private bool IsSet(RouteControl c)
        {
            return routecontrolsettings.Contains(c.ToString());
        }
        private void extButtonControlRoute_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();

            displayfilter.AddAllNone();
            displayfilter.AddStandardOption(RouteControl.showJumps.ToString(), "Show Jumps To Go".TxID(EDTx.UserControlRouteTracker_showJumpsToolStripMenuItem));
            displayfilter.AddStandardOption(RouteControl.showwaypoints.ToString(), "Show Waypoint Coordinates".TxID(EDTx.UserControlRouteTracker_showWaypointCoordinatesToolStripMenuItem));
            displayfilter.AddStandardOption(RouteControl.showdeviation.ToString(), "Show Deviation from route".TxID(EDTx.UserControlRouteTracker_showDeviationFromRouteToolStripMenuItem));
            displayfilter.AddStandardOption(RouteControl.showbookmarks.ToString(), "Show Bookmark Notes".TxID(EDTx.UserControlRouteTracker_showBookmarkNotesToolStripMenuItem));
            displayfilter.AddStandardOption(RouteControl.autocopy.ToString(), "Auto copy waypoint".TxID(EDTx.UserControlRouteTracker_autoCopyWPToolStripMenuItem));
            displayfilter.AddStandardOption(RouteControl.settarget.ToString(), "Auto set target".TxID(EDTx.UserControlRouteTracker_autoSetTargetToolStripMenuItem));
            displayfilter.AddStandardOption(RouteControl.showtarget.ToString(), "Show Target Information".TxID(EDTx.UserControlRouteTracker_showtargetinfo));
            displayfilter.AddStandardOption(RouteControl.showfuel.ToString(), "Show Fuel Information".TxID(EDTx.UserControlRouteTracker_showfuelinfo));

            displayfilter.AllOrNoneBack = false;
            displayfilter.ImageSize = new Size(24, 24);
            displayfilter.ScreenMargin = new Size(0, 0);

            displayfilter.SaveSettings = (s, o) =>
            {
                routecontrolsettings = s;
                PutSetting("routecontrol", s);
                DrawSystem(last_sys);
            };

            displayfilter.Show(routecontrolsettings, (Control)sender, this.FindForm());
        }

        #endregion


    }

    public class UserControlRouteTracker : UserControlSurveyor
    {
        public UserControlRouteTracker() : base()
        {
            DBBaseName = "RouteTracker";
        }

        protected override bool DefaultSetting(CtrlList e)
        {
            bool def = (e == CtrlList.alignleft);
            return def;
        }
    }
}
