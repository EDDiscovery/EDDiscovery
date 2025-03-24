/*
 * Copyright © 2016 - 2024 EDDiscovery development team
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

using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.JournalEvents;
using EliteDangerousCore.UIEvents;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlSurveyor : UserControlCommonBase
    {
        #region Initialisation

        public UserControlSurveyor()
        {
            InitializeComponent();
            DBBaseName = "Surveyor";
        }
        public override void Init()
        {
            edsmSpanshButton.Init(this, "EDSMSpansh", "");
            edsmSpanshButton.ValueChanged += (s, ch) =>
            {
                DrawAll(cur_sys);
            };

            PopulateCtrlList();

            extCheckBoxWordWrap.Checked = GetSetting("wordwrap", false);
            extCheckBoxWordWrap.Click += wordWrapToolStripMenuItem_Click;

            fsssignalstodisplay = GetSetting("fsssignals", "");

            DiscoveryForm.OnNewUIEvent += Discoveryform_OnNewUIEvent;
            DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange;
            DiscoveryForm.OnNewEntry += Discoveryform_OnNewEntry;
            DiscoveryForm.OnNewTarget += Discoveryform_OnNewTarget;
            GlobalBookMarkList.Instance.OnBookmarkChange += GlobalBookMarkList_OnBookmarkChange;

            string thisname = typeof(UserControlSurveyor).Name;
            var enumlisttt = new Enum[] { EDTx.UserControlSurveyor_extButtonPlanets_ToolTip, EDTx.UserControlSurveyor_extButtonStars_ToolTip, EDTx.UserControlSurveyor_extButtonShowControl_ToolTip, EDTx.UserControlSurveyor_extButtonAlignment_ToolTip, 
                            EDTx.UserControlSurveyor_extButtonFSS_ToolTip, EDTx.UserControlSurveyor_extButtonSetRoute_ToolTip, 
                            EDTx.UserControlSurveyor_extButtonControlRoute_ToolTip, EDTx.UserControlSurveyor_extButtonFont_ToolTip, EDTx.UserControlSurveyor_extCheckBoxWordWrap_ToolTip, 
                            EDTx.UserControlSurveyor_extButtonSearches_ToolTip };
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this, new string[] { thisname });
            rollUpPanelTop.SetToolTip(toolTip);

            translatednavroutename = "Nav Route".T(EDTx.UserControlSurveyor_navroute);

            displayfont = FontHelpers.GetFont(GetSetting("font", ""), null);        // null if not set

            LoadRoute(GetSetting("route", ""));
            routecontrolsettings = GetSetting("routecontrol", "showJumps;showwaypoints;shownotetext");

            rollUpPanelTop.PinState = GetSetting("PinState", true);

            drawsystemupdatetimer = new Timer() { Interval = 500 };
            drawsystemupdatetimer.Tick += DrawSystemUpdatetimer_Tick;
        }


        public override void InitialDisplay()
        {
            RequestPanelOperation(this, new UserControlCommonBase.RequestTravelHistoryPos());     //request an update 
            SetVisibility();
            doresize = true;                            // now allow resizing actions, before, resizes were due to setups, now due to user interactions
        }

        public override void Closing()
        {
            drawsystemupdatetimer.Stop();

            PutSetting("PinState", rollUpPanelTop.PinState);

            DiscoveryForm.OnNewUIEvent -= Discoveryform_OnNewUIEvent;
            DiscoveryForm.OnHistoryChange -= Discoveryform_OnHistoryChange;
            DiscoveryForm.OnNewEntry -= Discoveryform_OnNewEntry;
            DiscoveryForm.OnNewTarget -= Discoveryform_OnNewTarget;
            GlobalBookMarkList.Instance.OnBookmarkChange -= GlobalBookMarkList_OnBookmarkChange;
        }

        private void Discoveryform_OnHistoryChange()
        {
            var hl = DiscoveryForm.History;
            cur_sys = hl.GetLast?.System;      // may be null
            shipfsdinfo = hl.GetLast?.GetJumpInfo(DiscoveryForm.History.MaterialCommoditiesMicroResources.CargoCount(hl.GetLast.MaterialCommodity));
            shipinfo = hl.GetLast?.ShipInformation;

            LoadRoute(GetSetting("route", ""));     // reload the route, may have locations now
            //System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber}  History Load '{currentRoute?.Name}' {currentRoute?.Id} systems {currentRoute?.Systems.Count} lastsys '{last_sys?.Name}'");

            DrawAll(cur_sys);
            SetVisibility();

            //t.Interval = 200; t.Tick += (s, e) => { var sys = currentRoute.PosAlongRoute(percent, 0); DrawSystem(sys, sys.Name); percent += 0.5; }; t.Start();  // debug to make it play thru.. leave
            //  t.Interval = 2000; t.Tick += (s, e) => { if (sysno < currentRoute.Systems.Count ){ var sys = currentRoute.KnownSystemList()[sysno++]; DrawSystem(sys.Item1, sys.Item1.Name); } }; t.Start();  // debug to make it play thru.. leave
        }
        //int sysno = 0; double percent = -10; Timer t = new Timer();// play thru harness

        private void Discoveryform_OnNewEntry(HistoryEntry he)
        {
            // received a new navroute, and we have navroute selected (-1), reload
            if (he.EntryType == JournalTypeEnum.NavRoute && currentRoute != null && currentRoute.Id == -1)
            {
                //System.Diagnostics.Debug.WriteLine("Surveyor {displaynumber} new entry, load nav route");
                LoadRoute(NavRouteNameLabel);
                cur_sys = DiscoveryForm.History.GetLast?.System;      // may be null, make sure its set.
                CalculateThenDrawSystemSignals(cur_sys);
            }

            eventsseen++;
        }

        private void Discoveryform_OnNewTarget(object obj)
        {
            DrawTarget();
        }
        private void GlobalBookMarkList_OnBookmarkChange(BookmarkClass bk, bool deleted)
        {
            CalculateThenDrawSystemSignals(cur_sys);
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
            //System.Diagnostics.Debug.WriteLine($"Surveyor set transparency {on} {curcol}");

            extPictureBoxScrollSystemDetails.ScrollBarEnabled = !on;     // turn off the scroll bar if its transparent

            this.BackColor = curcol;
            extPictureBoxScrollSystemDetails.BackColor =  curcol;
            rollUpPanelTop.Visible = !on;
            SetVisibility(!on);     // set our visible mode for text, but override to on if we are not transparent
        }

        public override void TransparencyModeChanged(bool on)
        {
            DrawAll(cur_sys);
        }

        // normally we use uistate/uimode to determine if the output is visible. We can override this, for use when transparent mode is on but its not transparent
        private void SetVisibility(bool overrideon = false)
        {
            bool showit = true;

            // if in autohide, and a transparent option is turned on
            if (!overrideon && IsSet(CtrlList.autohide) && IsTransparentModeOn )
            { 
                // if no focus, or fssmode and override
                showit = uistate == EliteDangerousCore.UIEvents.UIGUIFocus.Focus.NoFocus || (uistate == EliteDangerousCore.UIEvents.UIGUIFocus.Focus.FSSMode && IsSet(CtrlList.donthidefssmode));
                // and we should be either in None or MainShip..
                showit = showit && (uimode == EliteDangerousCore.UIEvents.UIMode.ModeType.None || uimode == EliteDangerousCore.UIEvents.UIMode.ModeType.MainShipSupercruise);
            }

            //System.Diagnostics.Debug.WriteLine($"Surveyor Visibility uimode {uimode} uistate {uistate} Visibility {showit} route {currentRoute!=null}");
            extPictureBoxScrollSystemDetails.Visible = showit;
            extPictureBoxScanSummary.Visible = IsSet(CtrlList.showscansum) && showit;
            extPictureBoxTitle.Visible = IsSet(CtrlList.showsysinfo) && showit;
            extPictureBoxRoute.Visible = currentRoute != null && showit;
            extPictureBoxTarget.Visible = showit && IsSet(RouteControl.showtarget) && TargetClass.IsTargetSet();
            extPictureBoxFuel.Visible = IsSet(RouteControl.showfuel) && showit;
        }

        private void UserControlSurveyor_Resize(object sender, EventArgs e)
        {
            if ( doresize)
                DrawAll(cur_sys,true);     // don't recalc
        }

        bool instartjump = false;


        // travelgrid sends this when cursor position changes, either up or down
        public override void ReceiveHistoryEntry(HistoryEntry he)
        {
            // something has changed and just blindly for now recalc the fsd info
            shipfsdinfo = he.GetJumpInfo(DiscoveryForm.History.MaterialCommoditiesMicroResources.CargoCount(he.MaterialCommodity));
            shipinfo = he.ShipInformation;

            // calculate if tracking history.  is_latest is used to gate action triggers - they only occur if we are tracking the top of history
            is_latest = Object.ReferenceEquals(DiscoveryForm.History.GetLast, he);

            // set if we have completed a jump
            bool jumpover = he.IsFSDLocationCarrierJump;

            System.Diagnostics.Debug.WriteLine($"Surveyor HE {he.EventTimeUTC} {he.EventSummary} state latest {is_latest} jumpover {jumpover}");

            if (is_latest)       // if at top of history.. if not, we don't do startjump sequencing
            {
                if (instartjump && !jumpover)       // if in a start jump, and jump not over.. throw away all events between start jump and fsd jump
                {
                    if (he.EntryType == JournalTypeEnum.FSSSignalDiscovered &&
                            ((JournalFSSSignalDiscovered)he.journalEntry).Signals[0].SystemAddress == cur_sys.SystemAddress)      // we may, unlikely, get signals from the next system
                    {
                        CalculateThenDrawSystemSignals(cur_sys);        // present..
                    }

                    System.Diagnostics.Debug.WriteLine($"{Environment.TickCount % 10000} Surveyor in jump sequence reject entry {he.EntryType}");
                    return;
                }

                if (jumpover)       // when jump is over, we release instartjump, ensure cur_sys has the full info, clear all recorded triggers, and then kick for a full display
                {
                    instartjump = false;
                    cur_sys = he.System;        // ensure fully populated
                    System.Diagnostics.Debug.WriteLine($"{Environment.TickCount % 10000} Surveyor arrived in system {cur_sys.Name}");
                    DrawAll(cur_sys);      // we are in the system now, draw all, in case we had signals between start jump and now, and to update the route/fuel thingies
                    return;
                }
            }

            if (he.EntryType == JournalTypeEnum.StartJump)                  // start jump stops rest of below working
            {
                JournalStartJump jsj = he.journalEntry as JournalStartJump;

                // needs to be a hyperspace one, not supercruise, and needs to be at top of list, so we are processing real time
                if (jsj.IsHyperspace && is_latest)
                {
                    // important need system address as scan uses it for quick lookup
                    // set our system to the next system
                    cur_sys = new SystemClass(jsj.StarSystem, jsj.SystemAddress);
                    System.Diagnostics.Debug.WriteLine($"{Environment.TickCount % 10000} Surveyor start jump changed to {cur_sys.Name} Draw title, scan summary, scan system");

                    // clear the system triggers memory here, in case we get something during start jump sequence
                    cursystriggered = new Dictionary<string, HashSet<string>>();

                    starclass = jsj.FriendlyStarClass;  // reset vars
                    bodies_found = 0;
                    all_found = false;

                    instartjump = true;     // begin start jump sequence

                    DrawTitle();
                    // no route,target or fuel needed
                    CalculateThenDrawScanSummary(cur_sys);     // do this on the next system
                    CalculateThenDrawSystemSignals(cur_sys);
                }

                return;     // ignore otherwise and don't let the stuff below happen since its an IStarScan and it will cause a recomputation
            }

            instartjump = false;            // if we got here, we are not in a start jump sequence, we are clicking around

            // If not got a system, or different name
            if (cur_sys == null || cur_sys.Name != he.System.Name)
            {
                starclass = null;           // we cancel out the text info fields
                bodies_found = 0;
                all_found = false;
                cur_sys = he.System;       // and set, then
                System.Diagnostics.Debug.WriteLine($"{Environment.TickCount % 10000} Surveyor noted system change to {cur_sys.Name} DrawAll");
                DrawAll(cur_sys);
                return;         // finish, do nothing else
            }
            else
                cur_sys = he.System;       // its the same system, but since we may have synthesised a cur_sys in startjump, and we have a real one from the journal, make sure its updated

            // if FSDJump/CarrierJump, when clicking around (in a start jump sequence we won't get this far down)
            // these affect fuel use and route, and cancel start jump sequence
            if (he.EntryType == JournalTypeEnum.FSDJump || he.EntryType == JournalTypeEnum.CarrierJump) 
            {
                System.Diagnostics.Debug.WriteLine($"{Environment.TickCount%10000} Surveyor FSDJump draw fuel/route");
                DrawFuel();
                DrawRoute(cur_sys);
            }
            else if (he.EntryType == JournalTypeEnum.FSSAllBodiesFound)     // title uses body count and all found, so update it
            {
                JournalFSSAllBodiesFound fs = he.journalEntry as JournalFSSAllBodiesFound;
                all_found = true;
                bodies_found = fs.Count;
                System.Diagnostics.Debug.WriteLine($"{Environment.TickCount%10000} Surveyor FSSAllbodies found Draw title");
                DrawTitle();      
            }
            else if (he.EntryType == JournalTypeEnum.FSSDiscoveryScan) // discovery scan carriers body counts
            {
                var je = he.journalEntry as JournalFSSDiscoveryScan;
                bodies_found = je.BodyCount;
                System.Diagnostics.Debug.WriteLine($"{Environment.TickCount % 10000} Surveyor FSSDiscoveryscan found on {cur_sys.Name} Draw Title Scan Summary");
                DrawTitle();
                CalculateThenDrawScanSummary(cur_sys);
            }
            else if (he.EntryType == JournalTypeEnum.FuelScoop || he.EntryType == JournalTypeEnum.ReservoirReplenished)
            {
                instartjump = false;
                DrawFuel();
            }

            // these are in IStarScan, but we don't need to do anything here, as they don't affect the display
            else if (he.EntryType == JournalTypeEnum.SAAScanComplete || he.EntryType == JournalTypeEnum.Location)
            {
                // System.Diagnostics.Debug.WriteLine($"Surveyor Ignore {he.EventTimeUTC} {he.journalEntry.EventTypeStr}");
            }

            // IStarScan : FSSSignalDiscovery, SAASignalsFound, FSSBodySignals, Scan all involved in Searches
            // Scan is involved in searches
            // ScanBaryCentre is also indirectly involved in Searches

            else if (he.journalEntry is IStarScan )
            {
                System.Diagnostics.Debug.WriteLine($"{Environment.TickCount%10000} Surveyor Update due to {he.EventTimeUTC} {he.journalEntry.EventTypeStr}");

                // kick the scan aggregator timer 
                drawsystemupdatetimer.Stop();      
                drawsystemupdatetimer.Start();
            }
        }

        // when we get a starscan update, we set a timer in the hope of aggregating them and preventing repeated recalcs. timer ticked out
        private void DrawSystemUpdatetimer_Tick(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"{Environment.TickCount % 10000} Surveyor timer expired on {cur_sys.Name}");
            drawsystemupdatetimer.Stop();
            CalculateThenDrawScanSummary(cur_sys);
            CalculateThenDrawSystemSignals(cur_sys);
        }

        #endregion

        #region Display

        // Display all. If resizeonly is on, the await parts are not recalculated, but are drawn from previous calculated data
        private void DrawAll(ISystem sys, bool resizeonly = false)
        {
            //    System.Diagnostics.Debug.WriteLine($"\nSurveyor draw All {presentsystemonly}");

            // these calculate and draw quick
            DrawTitle();        
            DrawRoute(sys);
            DrawTarget();
            DrawFuel();

            if (resizeonly)  // used during resize
            {
                lock (scansummarytext)
                {
                    DrawScanSummaryUnlocked();
                }
                lock (extPictureBoxScrollSystemDetails)      // because of the async call, we lock the local structures drawsystem* over the draw, so a calculate can't reset them during the draw
                {
                    DrawSystemUnlocked();
                }
            }
            else
            {
                CalculateThenDrawScanSummary(sys);
                CalculateThenDrawSystemSignals(sys);
            }
        }

        // stack consists of:
        // Title - drawn direct
        // Route - drawn direct
        // Target - drawn direct
        // Fuel - drawn direct
        // Scan summary of values - async await, then draw
        // Body and search data - async await, then draw

        // title, immediate, no async
        private void DrawTitle()
        {
           // System.Diagnostics.Debug.WriteLine($"Surveyor draw title with {IsTransparentModeOn}");

            string text = cur_sys?.Name ?? "";
            if (starclass.HasChars() && IsSet(CtrlList.showstarclass))
                text += " | " + starclass;
            if (bodies_found > 0 && !all_found)
                text += " | " + bodies_found + " bodies detected.".T(EDTx.UserControlSurveyor_bodiesdetected);
            if (all_found)
                text += " | " + "System scan complete".T(EDTx.UserControlSurveyor_Systemscancomplete) + ": " + bodies_found + " bodies found.".T(EDTx.UserControlSurveyor_bodiesfound);

            SetControlText(text);
            DrawTextIntoBox(extPictureBoxTitle, text);
        }

        // draw route, direct, no async. Sys is current system
        private void DrawRoute(ISystem sys)
        {
            //System.Diagnostics.Debug.WriteLine($"Surveyor draw route {sys?.Name}");

            string lastroutetext = "No System Info";

            //System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} Current route set, sys has coord, route is {currentRoute.Systems.Count} {currentRoute.Id}");
            if (sys != null && currentRoute != null && sys.HasCoordinate)      // must have a system, a current route, and coord
            {
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

                        if (IsSet(RouteControl.shownotetext) && closest.nextsystemwaypointnote.HasChars())
                        {
                            if ( sys.Name == closest.lastsystem.Name)
                            {
                                lastroutetext = lastroutetext.AppendPrePad(closest.lastsystem.Name + ": " + closest.lastsystemwaypointnote, Environment.NewLine);
                                lastroutetext = lastroutetext.AppendPrePad(closest.nextsystem.Name + ": " + closest.nextsystemwaypointnote, Environment.NewLine);
                            }
                            else
                                lastroutetext = lastroutetext.AppendPrePad(closest.nextsystemwaypointnote, Environment.NewLine);
                        }

                        string closestsystem = closest.nextsystem.Name;

                        if (lastsystemonroute == null || closestsystem.CompareTo(lastsystemonroute) != 0)
                        {
                            if (IsSet(RouteControl.autocopy))
                                SetClipboardText(closestsystem);

                            if (IsSet(RouteControl.settarget))
                            {
                                if (TargetClass.SetTargetOnSystemConditional(closestsystem, closest.nextsystem.X, closest.nextsystem.Y, closest.nextsystem.Z))
                                {
                                    DiscoveryForm.NewTargetSet(this);
                                }
                            }

                            lastsystemonroute = closestsystem;
                        }
                    }
                }
            }

            DrawTextIntoBox(extPictureBoxRoute, lastroutetext);
        }

        // Target, direct, no async
        private void DrawTarget()
        {
            //System.Diagnostics.Debug.WriteLine($"Surveyor draw target");

            string lasttargettext = "No Target";

            if (TargetClass.GetTargetPosition(out string name, out double x, out double y, out double z))
            {
                if (cur_sys != null)     // double checky
                {
                    double dist = cur_sys.Distance(x, y, z);

                    string jumpstr = "";
                    if (shipfsdinfo != null)
                    {
                        int jumps = (int)Math.Ceiling(dist / shipfsdinfo.avgsinglejump);
                        if (jumps > 0)
                            jumpstr = jumps.ToString() + " " + ((jumps == 1) ? "jump".T(EDTx.UserControlRouteTracker_J1) : "jumps".T(EDTx.UserControlRouteTracker_JS));
                    }

                    lasttargettext = $"T-> {name} {dist:N1}ly {jumpstr}";
                }
                else
                    lasttargettext = "No known system";
            }

            DrawTextIntoBox(extPictureBoxTarget, lasttargettext);
        }

        // Fuel, direct, no async
        private void DrawFuel()
        {
            //System.Diagnostics.Debug.WriteLine($"Surveyor draw fuel");

            string fueltext = "";

            if (shipinfo != null)
            {
                shipinfo.UpdateFuelWarningPercent();      // ensure its fresh from the DB
                
                double fuel = shipinfo.FuelLevel;
                double tanksize = shipinfo.FuelCapacity;
                double warninglevelpercent = shipinfo.FuelWarningPercent;

                fueltext = $"{fuel:N1}/{tanksize:N1}t";

                if (warninglevelpercent > 0 && fuel < tanksize * warninglevelpercent / 100.0)
                {
                    fueltext += $" < {warninglevelpercent:N1}%";
                }

                if (shipfsdinfo != null)
                {
                    fueltext += string.Format(" " + "Avg {0:N1}ly Fume {1:N1}ly Range {2:N1}ly".T(EDTx.UserControlSurveyor_fuel), shipfsdinfo.avgsinglejump, shipfsdinfo.curfumessinglejump, shipfsdinfo.maxjumprange);
                }
            }

            DrawTextIntoBox(extPictureBoxFuel, fueltext);
        }

        // calculate then draw the scan summmary info. Await
        private async void CalculateThenDrawScanSummary(ISystem sys)
        {
            // System.Diagnostics.Debug.WriteLine($"Surveyor scan summary ${sys?.Name}");

            scansummarytext = "";

            if (sys != null)
            {
                StarScan.SystemNode systemnode = await DiscoveryForm.History.StarScan.FindSystemAsync(sys, edsmSpanshButton.WebLookup);        // get data with EDSM
                if (IsClosed)   // may close during await..
                    return;

                if (sys != cur_sys)
                {
                    System.Diagnostics.Debug.WriteLine($"{Environment.TickCount%10000} Surveyor Cancelled CalculateThenDrawScanSummary as cursys has changed during await");
                    return;
                }

                if (systemnode != null)
                {
                    int scanned = systemnode.StarPlanetsWithData(edsmSpanshButton.IsAnySet);
                    int clusters = systemnode.BeltClusters();

                    if (scanned > 0)
                    {
                        scansummarytext = scansummarytext.AppendPrePad("Scan".T(EDTx.UserControlSurveyor_Scan) + " " + scanned.ToString() + (systemnode.FSSTotalBodies != null ? (" / " + systemnode.FSSTotalBodies.Value.ToString()) : ""), Environment.NewLine);
                    }

                    long value = systemnode.ScanValue(false);   // don't include value of web bodies

                    if (value > 0 && IsSet(CtrlList.showValues))
                    {
                        scansummarytext.AppendPrePad("Scan".T(EDTx.UserControlSurveyor_Scan) + " " + scanned.ToString() + (systemnode.FSSTotalBodies != null ? (" / " + systemnode.FSSTotalBodies.Value.ToString()) : "", Environment.NewLine));
                        scansummarytext = scansummarytext.AppendPrePad("~ " + value.ToString("N0") + " cr" + ";");                        
                    }
                    if (systemnode.FSSTotalNonBodies != null && IsSet(CtrlList.showsignalmismatch))
                    {
                        scansummarytext = scansummarytext.AppendPrePad(systemnode.FSSTotalNonBodies.Value != clusters ? " Cluster and NonBody counts differ by ".T(EDTx.UserControlSurveyor_ShowSignalMismatch) + Math.Abs(systemnode.FSSTotalNonBodies.Value - clusters).ToString("N0") : "");
                    }
                    
                }
            }

            lock (scansummarytext)
                DrawScanSummaryUnlocked();
        }

        private void DrawScanSummaryUnlocked()
        {
            DrawTextIntoBox(extPictureBoxScanSummary, scansummarytext);
        }

        // recalc the system drawSystem* values, then lock, set the locals above, then lock draw
        async private void CalculateThenDrawSystemSignals(ISystem sys)
        {
            System.Diagnostics.Debug.WriteLine($"{Environment.TickCount % 10000} Surveyor {DisplayNumber} calc system {sys?.Name}");

            SortedList<string, string> ldrawsystemtext = new SortedList<string, string>(new CollectionStaticHelpers.AlphaIntCompare<string>());
            string ldrawsystemsignallist = "";
            long ldrawsystemvalue = 0;

            if (sys != null)      // if we have a system
            {
                // record triggers for this system
                Dictionary<string, HashSet<string>> triggers = new Dictionary<string, HashSet<string>>();

                // perform searches, and store them in searchresults, keyed by body name matched

                var searchresults = new Dictionary<string, List<HistoryListQueries.ResultEntry>>();

                if (searchesactivetext.Length > 0 || searchesactivevoice.Length>0)       // if any searches
                {
                    DiscoveryForm.History.FillInScanNode();     // ensure all journal scan entries point to a scan node (expensive, done only when required in this panel)

                    // all entries related to sys.  Can't really limit the pick up as tried before using the afterlastevent option in this call
                    // due to being able to browse back in history. We may not be at the end of the list the system we are displaying. For now, just do a blind whole history search

                    var helist = HistoryList.FilterByEventEntryOrder(DiscoveryForm.History.EntryOrder(), HistoryListQueries.AllSearchableJournalTypes, sys);

                    if (helist.Count > 0)        // no point executing if nothing in helist
                    {
                        var defaultvars = new BaseUtils.Variables();
                        defaultvars.AddPropertiesFieldsOfClass(new BodyPhysicalConstants(), "", null, 10, ensuredoublerep: true);

                        System.Diagnostics.Debug.WriteLine($"{Environment.TickCount%10000} ... Surveyor runs {searchesactivetext.Length} searches");

                        var allsearches = searchesactivetext.Union(searchesactivevoice).ToArray();

                        foreach (var searchname in allsearches)
                        {
                            // await is horrible, anything can happen, even closing
                            await HistoryListQueries.Instance.Find(helist, searchresults, searchname, defaultvars, DiscoveryForm.History.StarScan, false); // execute the searches

                            if (IsClosed)       // if we was ordered to close, abore
                                return;

                        }

                        //System.Diagnostics.Debug.WriteLine($"  Surveyor reports {searchresults.Count} results");
                    }
                }

                // we store strings in textresults, sorted by its body name, and accumulate them during nodes and finally render them search results
                // this is because we have two goes at filling in text, nodes and remaining search results, but we want them alpha sorted

                // find if we have system nodes

                System.Diagnostics.Debug.WriteLine($"{Environment.TickCount % 10000} Surveyor Find System Async {sys.Name}");

                StarScan.SystemNode systemnode = await DiscoveryForm.History.StarScan.FindSystemAsync(sys, edsmSpanshButton.WebLookup);       
                if (IsClosed)   // may close during await..
                    return;

                bool producebodytriggers = is_latest && eventsseen > 0;     // produce triggers only if we have seen events, so don't produce them on startup
                bool producesearchtriggers = producebodytriggers;       // two of them exist for debugging

// tbd
  producebodytriggers = true;
producesearchtriggers = false;

                if (systemnode != null)     // if we have a node (should do of course since july 22 due to AddLocation in scan node)
                {
                    System.Diagnostics.Debug.WriteLine($"{Environment.TickCount % 10000} Surveyor Process node {sys.Name}");

                    foreach (StarScan.ScanNode sn in systemnode.Bodies().EmptyIfNull().Where(x => x.ScanData != null))        // only nodes with scan data can be treated here
                    {
                        var sd = sn.ScanData;

                        // compute some properties of the object

                        bool hasgeosignals = sn.CountGeoSignals > 0;
                        bool hasbiosignals = sn.CountBioSignals > 0;
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
                        bool matchedlandablevolcanism = sd.IsLandable && sd.HasMeaningfulVolcanism && IsSet(CtrlList.isLandableWithVolcanism);
                        bool matchedvolcanism = sd.HasMeaningfulVolcanism && IsSet(CtrlList.showVolcanism);

                        // make up triggers for the action programs

                        triggers[sd.BodyName] = new HashSet<string>();
                        var en = triggers[sd.BodyName];

                        // we only send trigger actions when we are tracking the latest travel history, not when we are back in the history
                        // and we have had new events come in (showing we are not just started up cold)

                        if (producebodytriggers)
                        {
                            // test
                            //en.Add(TTThargoidSignals); en.Add(TTMiningSignals); en.Add(TTHighGravity + sd.nSurfaceGravityG.Value.ToStringInvariant("#.##")); en.Add(TTHuge);

                            if (hasminingsignals) en.Add(TTMiningSignals);
                            if (hasgeosignals) en.Add(TTGeoSignals);
                            if (hasbiosignals) en.Add(TTBioSignals);
                            if (hasthargoidsignals) en.Add(TTThargoidSignals);
                            if (hasguardiansignals) en.Add(TTGuardianSignals);
                            if (hashumansignals) en.Add(TTHumanSignals);
                            if (hasothersignals) en.Add(TTOtherSignals);
                            if (sd.CanBeTerraformable) en.Add(TTCanBeTerraformed);
                            if (sd.IsLandable) en.Add(TTLandable);
                            if (sd.HasMeaningfulVolcanism) en.Add(TTVolcanism);
                            if (sd.HasRings) en.Add(TTRings);
                            if (sd.HasBelts) en.Add(TTBelts);
                            if (sd.Earthlike) en.Add(TTEarthlike);
                            if (sd.WaterWorld) en.Add(TTWaterWorld);
                            if (sd.AmmoniaWorld) en.Add(TTAmmoniaWorld);
                            if (sd.nEccentricity >= eccentricityLimit) en.Add(TTEccentric);
                            if (sd.nRadius < lowRadiusLimit && sd.IsPlanet) en.Add(TTTiny);
                            if (sd.nRadius > largeRadiusLimit && sd.IsPlanet && sd.IsLandable) en.Add(TTHuge);
                            if (sd.HasAtmosphericComposition) en.Add(TTAtmosphere + sd.AtmosphereTranslated);
                            if (sd.IsPlanet && sd.nSurfaceGravityG >= 2) en.Add(TTHighGravity + sd.nSurfaceGravityG.Value.ToStringInvariant("#.##"));
                        }

                        // compute if we want search results displayed

                        searchresults.TryGetValue(sn.BodyDesignator, out List<HistoryListQueries.ResultEntry> searchresultfornode);     // will be null if not found

                        var surveyordisplay = searchresultfornode != null;      // if we have a search node, display

                        // work out if we want to display if we don't have searchresult, as long as not websourced or allow web sourced

                        if (surveyordisplay == false && (!sd.IsWebSourced || edsmSpanshButton.IsAnySet)) 
                        {
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

                        }

                        // qualify choice by mapped - now applies to both discovery searches and surveyor displays
                        surveyordisplay &= !sd.Mapped || IsSet(CtrlList.hideMapped) == false;

                        // if we do display..
                        if (surveyordisplay)
                        {
                            // surveyor line, some of these use the triggers above
                            var silstring = sd.SurveyorInfoLine(sys,
                                    matchedminingsignals || (hasminingsignals && IsSet(CtrlList.showsignals)),
                                    matchedgeosignals || (hasgeosignals && IsSet(CtrlList.showsignals)),
                                    matchedbiosignals || (hasbiosignals && IsSet(CtrlList.showsignals)),
                                    matchedthargoidsignals || (hasthargoidsignals && IsSet(CtrlList.showsignals)),
                                    matchedguardiansignals || (hasguardiansignals && IsSet(CtrlList.showsignals)),
                                    matchedhumansignals || (hashumansignals && IsSet(CtrlList.showsignals)),
                                    matchedothersignals || (hasothersignals && IsSet(CtrlList.showsignals)),
                                    false,      // so this is the surveyor, we don't want to bompsamther with showing if its got organics, since you have to scan them
                                    IsSet(CtrlList.volcanism) || matchedlandablevolcanism || matchedvolcanism, // any of these causes a show
                                    IsSet(CtrlList.showValues),        // show values
                                    IsSet(CtrlList.moreinfo),   // show extra info such as mass/radius
                                    IsSet(CtrlList.showGravity),       // show gravity select
                                    IsSet(CtrlList.atmos) || matchedlandablewithatmosphere, // show atmosphere if landable (surveyor shows this if landable)
                                    IsSet(CtrlList.temp),
                                    IsSet(CtrlList.showRinged),          // show rings
                                    lowRadiusLimit, largeRadiusLimit, eccentricityLimit);

                            if (searchresultfornode != null) // if search results are set for this body, add text and produce triggers
                            {
                                string searchpasslist = "";

                                foreach ( HistoryListQueries.ResultEntry hrentry in searchresultfornode)
                                {
                                    if (searchesactivetext.Contains(hrentry.FilterPassed))   // if its a text entry
                                        searchpasslist = searchpasslist.AppendPrePad(hrentry.FilterPassed, ", ");

                                    if (producesearchtriggers && searchesactivevoice.Contains(hrentry.FilterPassed))   // if its a text entry
                                        en.Add(TTDiscovery + hrentry.FilterPassed);       // add a trigger
                                }

                                if ( searchpasslist.HasChars())
                                    silstring += " : " + searchpasslist;
                            }

                            ldrawsystemtext[sd.BodyName] = silstring;

                            ldrawsystemvalue += sd.EstimatedValue;
                        }

                        // if we had a search result, remove it from the list as we have considered it above.  Even if we decided not to print it!
                        if ( searchresultfornode != null )
                            searchresults.Remove(sn.BodyDesignator);

                    }   // end for..
                }       // end of system node look thru

                // Any searches left print - they may have been triggered outside of a scan node
                // so we need to consider them

                foreach (var kvp in searchresults.EmptyIfNull())            // by bodyname
                {
                    string searchpasslist = "";

                    foreach(HistoryListQueries.ResultEntry hrentry in kvp.Value)
                    {
                        if (searchesactivetext.Contains(hrentry.FilterPassed))   // if its a text entry
                            searchpasslist = searchpasslist.AppendPrePad(hrentry.FilterPassed, ", ");

                        if (producesearchtriggers && searchesactivevoice.Contains(hrentry.FilterPassed))   // if its a text entry
                            triggers[kvp.Key].Add(TTDiscovery + hrentry.FilterPassed);       // add a trigger
                    }

                    if ( searchpasslist.HasChars())
                    {
                        ldrawsystemtext[kvp.Key] = $"{kvp.Key.ReplaceIfStartsWith(sys.Name)}: {searchpasslist}";
                    }
                }

                // any FSS items, if we have system node

                if (systemnode != null && fsssignalstodisplay.HasChars())
                {
                    string[] filter = fsssignalstodisplay.Split(';');

                    var signallist = JournalFSSSignalDiscovered.SignalList(systemnode.FSSSignalList);

                    // mirrors scandisplaynodes

                    var notexpired = signallist.Where(x => (!x.SignalName.Contains("-class") && (!x.TimeRemaining.HasValue || x.ExpiryUTC >= DateTime.UtcNow)) || (x.SignalName.Contains("-class") && x.RecordedUTC > DateTime.UtcNow.AddDays(-14))).ToList();
                    notexpired.Sort(delegate (FSSSignal l, FSSSignal r) { return l.ClassOfSignal.CompareTo(r.ClassOfSignal); });

                    var expired = signallist.Where(x => x.TimeRemaining.HasValue && x.ExpiryUTC < DateTime.UtcNow).ToList();
                    expired.Sort(delegate (FSSSignal l, FSSSignal r) { return r.ExpiryUTC.CompareTo(l.ExpiryUTC); });

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
                            fsssignalstodisplay.Equals("*"))
                        {
                            if (pos++ == expiredpos)
                                ldrawsystemsignallist = ldrawsystemsignallist.AppendPrePad("Expired:".T(EDTx.UserControlScan_Expired), Environment.NewLine + Environment.NewLine);

                            ldrawsystemsignallist = ldrawsystemsignallist.AppendPrePad(fsssig.ToString(true), Environment.NewLine);
                        }
                    }
                }

                foreach (var kvp in triggers)
                {
                    BaseUtils.Variables v = new BaseUtils.Variables();

                    int index = 1;
                    foreach (var str in kvp.Value)
                    {
                        if (!cursystriggered.TryGetValue(kvp.Key, out HashSet<string> hs))      // if cursys triggered does not have this body, add
                            hs = cursystriggered[kvp.Key] = new HashSet<string>();
// tbd
                        if (true || !hs.Contains(str))     // if a new trigger
                        {
                            hs.Add(str);
                            string name = str.Substring(0, str.IndexOfOrLength(":"));
                            string data = str.Contains(":") ? str.Substring(str.IndexOf(':') + 1) : "";
                            v["EventName" + index.ToStringInvariant()] = name;
                            v["Value" + index.ToStringInvariant()] = data;
                            //System.Diagnostics.Debug.WriteLine($"Surveyor Trigger {name} : {data}");
                            index++;
                        }
                    }

                    if ( v.Count>0 )
                    {
                        v["System"] = sys.Name;
                        v["Body"] = kvp.Key;
                        v["BodyShortName"] = kvp.Key.ReplaceIfStartsWith(sys.Name).Trim();
                        System.Diagnostics.Debug.WriteLine($"Surveryor Run Action on Body {kvp.Key} Trigger {v.ToString()}");
                        DiscoveryForm.ActionRun(Actions.ActionEventEDList.onSurveyor, v);
                    }
                }

            }       // end sys

            if (sys != cur_sys)
            {
                System.Diagnostics.Debug.WriteLine($"{Environment.TickCount%10000} Surveyor Cancelled CalculateThenDrawSystemSignals as cursys has changed during await");
                return;
            }

            lock (extPictureBoxScrollSystemDetails)      // because of the async call above, we may be running two of these at the same time. So, we lock, set vars, draw, unlock
            {
                drawsystemtext = ldrawsystemtext;                   // we keep these in variables so we can just draw again. Nasty
                drawsystemsignallist = ldrawsystemsignallist;
                drawsystemvalue = ldrawsystemvalue;
                DrawSystemUnlocked();
            }
        }

        // Draw system data, stored in drawsystem* values, to screen, taking into account width
        // this is not locked. MUST be extenally locked

        private void DrawSystemUnlocked()
        {
            System.Diagnostics.Debug.WriteLine($"{Environment.TickCount % 10000} Surveyor present system results");

            extPictureBoxSystemDetails.ClearImageList();

            int vpos = 0;

            StringFormat frmt = new StringFormat(extCheckBoxWordWrap.Checked ? 0 : StringFormatFlags.NoWrap);
            frmt.Alignment = alignment;
            var textcolour = IsTransparentModeOn ? ExtendedControls.Theme.Current.SPanelColor : ExtendedControls.Theme.Current.LabelColor;
            var backcolour = IsTransparentModeOn ? Color.Transparent : this.BackColor;
            Font dfont = displayfont ?? this.Font;

            foreach (var kvp in drawsystemtext)        // and present any text results in sorted order
            {
                var i = new ExtPictureBox.ImageElement();
                i.TextAutoSize(
                        new Point(3, vpos),
                        new Size(Math.Max(extPictureBoxSystemDetails.Width - 6, 24), 10000),
                        kvp.Value,
                        dfont,
                        textcolour,
                        backcolour,
                        1.0F,
                        frmt: frmt);
                extPictureBoxSystemDetails.Add(i);
                vpos += i.Location.Height;
            }

            if (drawsystemvalue > 0 && IsSet(CtrlList.showValues))
            {
                var i = new ExtPictureBox.ImageElement();
                i.TextAutoSize(
                    new Point(3, vpos),
                    new Size(Math.Max(extPictureBoxSystemDetails.Width - 6, 24), 10000),
                    "^^ ~ " + drawsystemvalue.ToString("N0") + " cr",
                    dfont,
                    textcolour,
                    backcolour,
                    1.0F,
                    frmt: frmt);
                extPictureBoxSystemDetails.Add(i);
                vpos += i.Location.Height;
            }

            if (drawsystemsignallist.HasChars())
            {
                //System.Diagnostics.Debug.WriteLine("Display " + siglist);
                var i = new ExtPictureBox.ImageElement();
                i.TextAutoSize(new Point(3, vpos),
                                                new Size(Math.Max(extPictureBoxSystemDetails.Width - 6, 24), 10000),
                                                drawsystemsignallist,
                                                dfont,
                                                textcolour,
                                                backcolour,
                                                1.0F,
                                                frmt: frmt);
                vpos += i.Location.Height;
                extPictureBoxSystemDetails.Add(i);
            }

            frmt.Dispose();

            extPictureBoxScrollSystemDetails.Render();
            Refresh();
        }


        void DrawTextIntoBox(ExtPictureBox pb, string text)
        {
            StringFormat frmt = new StringFormat(extCheckBoxWordWrap.Checked ? 0 : StringFormatFlags.NoWrap);
            frmt.Alignment = alignment;
            var textcolour = IsTransparentModeOn ? ExtendedControls.Theme.Current.SPanelColor : ExtendedControls.Theme.Current.LabelColor;
            var backcolour = IsTransparentModeOn ? Color.Transparent : this.BackColor;
            Font dfont = displayfont ?? this.Font;

            var i = new ExtPictureBox.ImageElement();
            i.TextAutoSize(
                    new Point(3, 0),
                    new Size(Math.Max(extPictureBoxSystemDetails.Width - 6, 24), 10000),
                    text,
                    dfont,
                    textcolour,
                    backcolour,
                    1.0F,
                    frmt: frmt);
            pb.ClearImageList();
            pb.Add(i);
            if ( text.HasChars() && IsSet(CtrlList.showdividers))
                pb.AddHorizontalDivider(textcolour.Multiply(0.4f), new Rectangle(0,i.Location.Height, i.Location.Width, 8), 1, 4);
            pb.Render();

            frmt.Dispose();
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
            showValues, moreinfo, showGravity, atmos, temp, volcanism, showsignals, autohide, donthidefssmode, hideMapped, showsysinfo, showscansum, showstarclass, showdividers,
            // 31
            alignleft, aligncenter, alignright,
            //34
            showsignalmismatch
        };

        private bool[] ctrlset; // holds current state of each control above

        private bool IsSet(CtrlList v)
        {
            return ctrlset[(int)v];
        }

        private string[] searchesactivetext;
        private string[] searchesactivevoice;

        // from DB, set up ctrlset, and set the defaults
        private void PopulateCtrlList()
        {
            ctrlset = GetSettingAsCtrlSet<CtrlList>(DefaultSetting);
            alignment = ctrlset[(int)CtrlList.alignright] ? StringAlignment.Far : ctrlset[(int)CtrlList.aligncenter] ? StringAlignment.Center : StringAlignment.Near;
            string sat = GetSetting(dbSearchesText, HistoryListQueries.Instance.DefaultSearches(SettingsSplittingChar),true);
            searchesactivetext = sat.SplitNoEmptyStartFinish('\u2188');
            searchesactivevoice = GetSetting(dbSearchesVoice, sat, true).SplitNoEmptyStartFinish('\u2188');
        }

        protected virtual bool DefaultSetting(CtrlList e)
        {
            bool def = (e != CtrlList.alignright && e != CtrlList.aligncenter && e != CtrlList.autohide && e != CtrlList.lowradius
                && e != CtrlList.allplanets && e != CtrlList.allstars && e != CtrlList.beltclusters);
            return def;
        }

        private void extButtonPlanets_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconNewListBoxForm displayfilter = new CheckedIconNewListBoxForm();

            displayfilter.UC.AddAllNone();
            displayfilter.UC.Add(CtrlList.allplanets.ToString(), "Show All Planets".TxID(EDTx.UserControlSurveyor_showAllPlanetsToolStripMenuItem), BaseUtils.Icons.IconSet.GetIcon("Bodies.Planets.Terrestrial.HMCv10"));
            displayfilter.UC.Add(CtrlList.showAmmonia.ToString(), "Ammonia World".TxID(EDTx.UserControlSurveyor_planetaryClassesToolStripMenuItem_ammoniaWorldToolStripMenuItem), BaseUtils.Icons.IconSet.GetIcon("Bodies.Planets.Terrestrial.AMWv1"));
            displayfilter.UC.Add(CtrlList.showEarthlike.ToString(), "Earthlike World".TxID(EDTx.UserControlSurveyor_planetaryClassesToolStripMenuItem_earthlikeWorldToolStripMenuItem), BaseUtils.Icons.IconSet.GetIcon("Bodies.Planets.Terrestrial.ELWv5"));
            displayfilter.UC.Add(CtrlList.showWaterWorld.ToString(), "Water World".TxID(EDTx.UserControlSurveyor_planetaryClassesToolStripMenuItem_waterWorldToolStripMenuItem), BaseUtils.Icons.IconSet.GetIcon("Bodies.Planets.Terrestrial.WTRv7"));
            displayfilter.UC.Add(CtrlList.showHMC.ToString(), "High metal content body".TxID(EDTx.UserControlSurveyor_planetaryClassesToolStripMenuItem_highMetalContentBodyToolStripMenuItem), BaseUtils.Icons.IconSet.GetIcon("Bodies.Planets.Terrestrial.HMCv3"));
            displayfilter.UC.Add(CtrlList.showMR.ToString(), "Metal-rich body".TxID(EDTx.UserControlSurveyor_planetaryClassesToolStripMenuItem_metalToolStripMenuItem), BaseUtils.Icons.IconSet.GetIcon("Bodies.Planets.Terrestrial.MRBv5"));
            displayfilter.UC.Add(CtrlList.showTerraformable.ToString(), "Terraformable".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_terraformableToolStripMenuItem), BaseUtils.Icons.IconSet.GetIcon("Bodies.Planets.Terrestrial.ELWv5"));
            displayfilter.UC.Add(CtrlList.showVolcanism.ToString(), "Has volcanism".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_hasVolcanismToolStripMenuItem), BaseUtils.Icons.IconSet.GetIcon("Bodies.Planets.Terrestrial.HMCv37"));
            displayfilter.UC.Add(CtrlList.showRinged.ToString(), "Has Rings".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_hasRingsToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_RingOnly);
            displayfilter.UC.Add(CtrlList.showEccentricity.ToString(), "High eccentricity".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_highEccentricityToolStripMenuItem), global::EDDiscovery.Icons.Controls.Eccentric);
            displayfilter.UC.Add(CtrlList.lowradius.ToString(), "Tiny body".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_lowRadiusToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_SizeSmall);
            displayfilter.UC.Add(CtrlList.GeoSignals.ToString(), "Has geological signals".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_hasGeologicalSignalsToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_Signals);
            displayfilter.UC.Add(CtrlList.BioSignals.ToString(), "Has biological signals".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_hasBiologicalSignalsToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_Signals);
            displayfilter.UC.Add(CtrlList.signals.ToString(), "Has any other signals".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_hasSignalsToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_Signals);
            displayfilter.UC.Add(CtrlList.isLandable.ToString(), "Landable".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_landableToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_LandablePlanet);
            displayfilter.UC.Add(CtrlList.isLandableWithAtmosphere.ToString(), "Landable with atmosphere".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_landableWithAtmosphereToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_LandablePlanet);
            displayfilter.UC.Add(CtrlList.largelandable.ToString(), "Landable and large".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_landableAndLargeToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_LandablePlanet);
            displayfilter.UC.Add(CtrlList.isLandableWithVolcanism.ToString(), "Landable with volcanism".TxID(EDTx.UserControlSurveyor_bodyFeaturesToolStripMenuItem_landableWithVolcanismToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Bodies_LandablePlanet);

            CommonCtrl(displayfilter, extButtonPlanets);

        }


        private const char SettingsSplittingChar = '\u2188';     // pick a crazy one soe

        private void extButtonSearches_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconNewListBoxForm displayfilter = new CheckedIconNewListBoxForm();
            displayfilter.UC.AddAllNone(3);
            displayfilter.UC.AddGroupItem(HistoryListQueries.Instance.DefaultSearches(SettingsSplittingChar), "Default".T(EDTx.ProfileEditor_Default),checkmap:3);
            displayfilter.UC.SettingsSplittingChar = '\u2188';     // pick a crazy one soe

            var searches = HistoryListQueries.Instance.Searches.Where(x => x.UserOrBuiltIn).ToList();
            foreach (var s in searches)
                displayfilter.UC.Add(s.Name, s.Name,checkmap:3, checkbuttontooltiptext:new string[] { "Text Output","Voice Output"});

            var under = extButtonSearches;

            displayfilter.CloseBoundaryRegion = new Size(32, under.Height);
            displayfilter.AllOrNoneBack = false;
            displayfilter.UC.ImageSize = new Size(24, 24);
            displayfilter.UC.ScreenMargin = new Size(0, 0);
            displayfilter.UC.MultiColumnSlide = true;

            displayfilter.SaveSettings = (s, o) =>
            {
                PutSetting(dbSearchesText, displayfilter.GetChecked(0));
                PutSetting(dbSearchesVoice, displayfilter.GetChecked(1));
                PopulateCtrlList();
                DrawAll(cur_sys);
            };

            string primarysetting = GetSetting(dbSearchesText, "");
            displayfilter.Show(new string[] { primarysetting, GetSetting(dbSearchesVoice, primarysetting) }, under, this.FindForm());
        }

        private void extButtonStars_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconNewListBoxForm displayfilter = new CheckedIconNewListBoxForm();

            displayfilter.UC.AddAllNone();
            displayfilter.UC.Add(CtrlList.allstars.ToString(), "Show All Stars".TxID(EDTx.UserControlSurveyor_showAllStarsToolStripMenuItem), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.UC.Add(CtrlList.beltclusters.ToString(), "Show Belt Clusters".TxID(EDTx.UserControlSurveyor_showBeltClustersToolStripMenuItem), global::EDDiscovery.Icons.Controls.Belt);

            CommonCtrl(displayfilter, extButtonStars);
        }

        private void extButtonShowControl_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconNewListBoxForm displayfilter = new CheckedIconNewListBoxForm();

            displayfilter.UC.AddAllNone();
            displayfilter.UC.Add(CtrlList.showValues.ToString(), "Show values".TxID(EDTx.UserControlSurveyor_showValuesToolStripMenuItem));
            displayfilter.UC.Add(CtrlList.moreinfo.ToString(), "Show more information".TxID(EDTx.UserControlSurveyor_showMoreInformationToolStripMenuItem));
            displayfilter.UC.Add(CtrlList.showGravity.ToString(), "Show gravity of landables".TxID(EDTx.UserControlSurveyor_showGravityToolStripMenuItem));
            displayfilter.UC.Add(CtrlList.atmos.ToString(), "Show atmospheres".TxID(EDTx.UserControlSurveyor_showAtmosToolStripMenuItem));
            displayfilter.UC.Add(CtrlList.temp.ToString(), "Show surface temperature".TxID(EDTx.UserControlSurveyor_showTempToolStripMenuItem));
            displayfilter.UC.Add(CtrlList.volcanism.ToString(), "Show volcanism".TxID(EDTx.UserControlSurveyor_showVolcanismToolStripMenuItem));
            displayfilter.UC.Add(CtrlList.showsignals.ToString(), "Show signals".TxID(EDTx.UserControlSurveyor_showSignalsToolStripMenuItem));
            displayfilter.UC.Add(CtrlList.showsignalmismatch.ToString(), "Show signal mismatch".TxID(EDTx.UserControlSurveyor_showSignalMismatchToolStripMenuItem));
            displayfilter.UC.Add(CtrlList.autohide.ToString(), "Auto Hide".TxID(EDTx.UserControlSurveyor_autoHideToolStripMenuItem));
            displayfilter.UC.Add(CtrlList.donthidefssmode.ToString(), "Don't hide in FSS Mode".TxID(EDTx.UserControlSurveyor_dontHideInFSSModeToolStripMenuItem));
            displayfilter.UC.Add(CtrlList.hideMapped.ToString(), "Hide already mapped bodies".TxID(EDTx.UserControlSurveyor_hideAlreadyMappedBodiesToolStripMenuItem));
            displayfilter.UC.Add(CtrlList.showsysinfo.ToString(), "Show system info always".TxID(EDTx.UserControlSurveyor_showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem));
            displayfilter.UC.Add(CtrlList.showscansum.ToString(), "Show scan summary always".TxID(EDTx.UserControlSurveyor_showScanSummaryOnScreenWhenInTransparentModeToolStripMenuItem));
            displayfilter.UC.Add(CtrlList.showstarclass.ToString(), "Show star class in system info".TxID(EDTx.UserControlSurveyor_showstarclassToolStripMenuItem));
            displayfilter.UC.Add(CtrlList.showdividers.ToString(), "Show dividers".TxID(EDTx.UserControlSurveyor_showDividersToolStripMenuItem));

            CommonCtrl(displayfilter, extButtonShowControl);
        }

        private void extButtonAlignment_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconNewListBoxForm displayfilter = new CheckedIconNewListBoxForm();

            string lt = CtrlList.alignleft.ToString();
            string ct = CtrlList.aligncenter.ToString();
            string rt = CtrlList.alignright.ToString();

            displayfilter.UC.Add(lt, "Alignment Left".TxID(EDTx.UserControlSurveyor_textAlignToolStripMenuItem_leftToolStripMenuItem), global::EDDiscovery.Icons.Controls.AlignLeft, exclusivetags: ct + ";" + rt, disableuncheck: true);
            displayfilter.UC.Add(ct, "Alignment Center".TxID(EDTx.UserControlSurveyor_textAlignToolStripMenuItem_centerToolStripMenuItem), global::EDDiscovery.Icons.Controls.AlignCentre, exclusivetags: lt + ";" + rt, disableuncheck: true);
            displayfilter.UC.Add(rt, "Alignment Right".TxID(EDTx.UserControlSurveyor_textAlignToolStripMenuItem_rightToolStripMenuItem), global::EDDiscovery.Icons.Controls.AlignRight, exclusivetags: lt + ";" + ct, disableuncheck: true);
            displayfilter.CloseOnChange = true;
            CommonCtrl(displayfilter, extButtonAlignment);
        }

        private void CommonCtrl(ExtendedControls.CheckedIconNewListBoxForm displayfilter, Control under, string saveasstring = null)
        {
            displayfilter.CloseBoundaryRegion = new Size(32, under.Height);
            displayfilter.AllOrNoneBack = false;
            displayfilter.UC.ImageSize = new Size(24, 24);
            displayfilter.UC.ScreenMargin = new Size(0, 0);
            displayfilter.UC.MultiColumnSlide = true;

            displayfilter.SaveSettings = (s, o) =>
            {
                if (saveasstring == null)
                    PutBoolSettingsFromString(s, displayfilter.UC.TagList());
                else
                    PutSetting(saveasstring, s);

                PopulateCtrlList();
                SetVisibility();
                DrawAll(cur_sys);
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
            DrawAll(cur_sys,true);
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("wordwrap", extCheckBoxWordWrap.Checked);
            DrawAll(cur_sys,true);
        }

        private void extButtonFSS_Click(object sender, EventArgs e)
        {
            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();

            int width = 430;

            f.Add(new ExtendedControls.ConfigurableEntryList.Entry("Text", typeof(ExtendedControls.ExtTextBox), fsssignalstodisplay, new Point(10, 40), new Size(width - 10 - 20, 110), "List Names to show") { TextBoxMultiline = true });

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
                fsssignalstodisplay = f.Get("Text");
                PutSetting("fsssignals", fsssignalstodisplay);
                CalculateThenDrawSystemSignals(cur_sys);
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
            dropdown.SelectedIndexChanged += (s, ea, key) =>
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

                SetVisibility();
                DrawRoute(cur_sys);
            };

            ExtendedControls.Theme.Current.ApplyDialog(dropdown, true);
            dropdown.Show(this.FindForm());
        }

        private void LoadRoute(string name)
        {
            //System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} Order load of route '{name}'");
            PutSetting("route", name);      // store back the current name 

            //System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} In DB its now '{GetSetting("route","???")}'");

            currentRoute = null;        // clear route and held text

            if (name.HasChars())
            {
                if (name.Equals(NavRouteNameLabel))     // if selecting navroutes
                {
                    // find last from history
                    var route = DiscoveryForm.History.GetLastHistoryEntry(x => x.EntryType == JournalTypeEnum.NavRoute)?.journalEntry as EliteDangerousCore.JournalEvents.JournalNavRoute;

                    if (route?.Route != null)   // if found
                    {
                        // pick out x/y/z to fill in route so it does not need any system lookup
                        var systems = route.Route.Where(x => x.StarSystem.HasChars()).
                                Select(rt => new SavedRouteClass.SystemEntry(rt.StarSystem,"",rt.StarPos.X,rt.StarPos.Y,rt.StarPos.Z)).ToList();

                        currentRoute = new SavedRouteClass(translatednavroutename, systems);      // with an ID of -1 note, used to detect navroutes
                        //System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} Loaded Nav route with {systems.Length}");
                    }
                    else
                    {
                        currentRoute = new SavedRouteClass();
                        currentRoute.Name = translatednavroutename;     // no known systems yet, but make a navroute so we have it selected
                        //System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} No route available, loaded empty Nav route");
                    }
                }
                else
                {
                    var savedroutes = SavedRouteClass.GetAllSavedRoutes();      // load routes
                    currentRoute = savedroutes.Find(x => x.Name == name);       // pick, if not found, will be null
                    currentRoute?.FillInCoordinates();                           // fill in any co-ords into DB - it may be in the DB without known co-ords
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
            shownotetext,
        };

        private bool IsSet(RouteControl c)
        {
            return routecontrolsettings.Contains(c.ToString());
        }
        private void extButtonControlRoute_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconNewListBoxForm displayfilter = new CheckedIconNewListBoxForm();

            displayfilter.UC.AddAllNone();
            displayfilter.UC.Add(RouteControl.showJumps.ToString(), "Show Jumps To Go".TxID(EDTx.UserControlRouteTracker_showJumpsToolStripMenuItem));
            displayfilter.UC.Add(RouteControl.showwaypoints.ToString(), "Show Waypoint Coordinates".TxID(EDTx.UserControlRouteTracker_showWaypointCoordinatesToolStripMenuItem));
            displayfilter.UC.Add(RouteControl.showdeviation.ToString(), "Show Deviation from route".TxID(EDTx.UserControlRouteTracker_showDeviationFromRouteToolStripMenuItem));
            displayfilter.UC.Add(RouteControl.showbookmarks.ToString(), "Show Bookmark Notes".TxID(EDTx.UserControlRouteTracker_showBookmarkNotesToolStripMenuItem));
            displayfilter.UC.Add(RouteControl.shownotetext.ToString(), "Show Route Note on waypoint".TxID(EDTx.UserControlRouteTracker_showSystemRouteNoteToolStripMenuItem));
            displayfilter.UC.Add(RouteControl.autocopy.ToString(), "Auto copy waypoint".TxID(EDTx.UserControlRouteTracker_autoCopyWPToolStripMenuItem));
            displayfilter.UC.Add(RouteControl.settarget.ToString(), "Auto set target".TxID(EDTx.UserControlRouteTracker_autoSetTargetToolStripMenuItem));
            displayfilter.UC.Add(RouteControl.showtarget.ToString(), "Show Target Information".TxID(EDTx.UserControlRouteTracker_showtargetinfo));
            displayfilter.UC.Add(RouteControl.showfuel.ToString(), "Show Fuel Information".TxID(EDTx.UserControlRouteTracker_showfuelinfo));

            displayfilter.AllOrNoneBack = false;
            displayfilter.UC.ImageSize = new Size(24, 24);
            displayfilter.UC.ScreenMargin = new Size(0,0);
            displayfilter.CloseBoundaryRegion = new Size(32, ((Control)sender).Height);

            displayfilter.SaveSettings = (s, o) =>
            {
                routecontrolsettings = s;
                PutSetting("routecontrol", s);
                DrawRoute(cur_sys);
                DrawFuel();
                SetVisibility();
            };

            displayfilter.Show(routecontrolsettings, (Control)sender, this.FindForm());
        }

        #endregion

        #region vars

        // we keep the results of the last CalculatethenDrawSystem so we can just DrawSystem later
        // we also set it to null in case DrawSystem gets called first before a calculate
        private SortedList<string, string> drawsystemtext = new SortedList<string, string>(new CollectionStaticHelpers.AlphaIntCompare<string>());
        private string drawsystemsignallist = "";
        private long drawsystemvalue = 0;
        
        // we keep the results of the last scan summary
        private string scansummarytext = "";

        // picked up from startjump and displayed
        private string starclass = "";

        // bodies found from FSSAllBodies found or DiscoveryScan
        private int bodies_found;
        private bool all_found = false;

        // the UI state is kept for display visibility
        private EliteDangerousCore.UIEvents.UIGUIFocus.Focus uistate = EliteDangerousCore.UIEvents.UIGUIFocus.Focus.NoFocus;
        private EliteDangerousCore.UIEvents.UIMode.ModeType uimode = EliteDangerousCore.UIEvents.UIMode.ModeType.None;

        // Route selection
        private const string NavRouteNameLabel = "!*NavRoute";      // special label to identify a save of using the nav route - not user presented
        private string translatednavroutename = "";     // presented to the user, found by the translator
        private SavedRouteClass currentRoute = null;    // the current route selected by the user. ID=-1 means a navroute
        private string lastsystemonroute;               // last system on route, used to check for updates

        // fsd
        private Ship shipinfo;   // and last ship info
        private EliteDangerousCalculations.FSDSpec.JumpInfo shipfsdinfo;        // last values of fsd info

        // user selections
        private Font displayfont;                       // font selected
        private StringAlignment alignment = StringAlignment.Near;   // text alignment
        private string fsssignalstodisplay = "";        // fss signals to show selector

        // tracking state
        private int eventsseen = 0;             // counts OnNewEvent calls, so we can actually see we are playing
        private bool is_latest = false;         // set to indicate latest travel entry is at top of history
        private ISystem cur_sys = null;         // set to indicate system on display
        private Timer drawsystemupdatetimer;    // delay before displaying system, to limit processing in case multiple scans are received at once
        private bool doresize = false;          // enable resize processing and display, limited until we know the display is set up after initial display

        private const int lowRadiusLimit = 300 * 1000; // tiny body limit in km converted to m
        private const int largeRadiusLimit = 20000 * 1000; // large body limit in km converted to m
        private const double eccentricityLimit = 0.95; //orbital eccentricity limit

        Dictionary<string, HashSet<string>> cursystriggered = new Dictionary<string, HashSet<string>>();    // triggers already played

        public const string TTMiningSignals = "MiningSignals";      // triggering names
        public const string TTGeoSignals = "GeoSignals";
        public const string TTBioSignals = "BioSignals";
        public const string TTThargoidSignals = "ThargoidSignals";
        public const string TTGuardianSignals = "GuardianSignals";
        public const string TTHumanSignals = "HumanSignals";
        public const string TTOtherSignals = "OtherSignals";
        public const string TTCanBeTerraformed = "Terraformable";
        public const string TTLandable = "Landable";
        public const string TTVolcanism = "Volcanism";
        public const string TTRings = "Rings";
        public const string TTBelts = "Belts";
        public const string TTEccentric = "Eccentric";
        public const string TTTiny = "TinyPlanetRadius";
        public const string TTHuge = "HugePlanetRadius";
        public const string TTAtmosphere = "Atmosphere:";
        public const string TTDiscovery = "Discovery:";
        public const string TTHighGravity = "HighGravity:";
        public const string TTEarthlike = "Earthlike";
        public const string TTWaterWorld = "WaterWorld";
        public const string TTAmmoniaWorld = "AmmoniaWorld";


        public const string dbSearchesText = "Searches";
        public const string dbSearchesVoice = "SearchesVoice";
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
