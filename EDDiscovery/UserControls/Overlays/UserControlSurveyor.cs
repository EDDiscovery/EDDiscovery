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
        ISystem last_sys = null;

        private StringAlignment alignment = StringAlignment.Near;
        private string titletext = "";
        private string fsssignalsdisplayed = "";

        const int lowRadiusLimit = 300 * 1000; // tiny body limit in km converted to m
        const int largeRadiusLimit = 20000 * 1000; // large body limit in km converted to m
        const double eccentricityLimit = 0.95; //orbital eccentricity limit

        EliteDangerousCore.UIEvents.UIGUIFocus.Focus uistate = EliteDangerousCore.UIEvents.UIGUIFocus.Focus.NoFocus;

        private SavedRouteClass currentRoute = null;
        private string lastsystem;
        private EliteDangerousCalculations.FSDSpec.JumpInfo shipfsdinfo;        // last values of fsd info
        private ShipInformation shipinfo;   // and last ship info
        const string NavRouteNameLabel = "!*NavRoute";      // special label to identify a save of using the nav route - not user presented
        private string lastroutetext = "";      // cached so we can represent it even if we pass a sys into drawsystem with no coord
        private string translatednavroutename = "";

        private Font displayfont;

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

            BaseUtils.Translator.Instance.Translate(toolTip, this);

            translatednavroutename = "Nav Route".T(EDTx.UserControlSurveyor_navroute);

            displayfont = FontHelpers.GetFont(GetSetting("font", ""), discoveryform.theme.GetFont);
            System.Diagnostics.Debug.WriteLine($"Surveyor font {FontHelpers.GetFontSettingString(displayfont)}");

            LoadRoute(GetSetting("route", ""));
            routecontrolsettings = GetSetting("routecontrol", "showJumps;showwaypoints");
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
            DrawSystem(last_sys, last_sys?.Name);    // may be null
        }

        public override void Closing()
        {
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            discoveryform.OnNewUIEvent -= Discoveryform_OnNewUIEvent;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.OnNewTarget -= Discoveryform_OnNewTarget;
            GlobalBookMarkList.Instance.OnBookmarkChange -= GlobalBookMarkList_OnBookmarkChange;
        }

        public override bool SupportTransparency { get { return true; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            extPictureBoxScroll.ScrollBarEnabled = !on;     // turn off the scroll bar if its transparent
            extPictureBoxScroll.BackColor = pictureBoxSurveyor.BackColor = this.BackColor = curcol;
            rollUpPanelTop.Visible = !on;
        }

        private void Discoveryform_OnHistoryChange(HistoryList hl)
        {
            last_sys = hl.GetLast?.System;      // may be null
            shipfsdinfo = hl.GetLast?.GetJumpInfo(discoveryform.history.MaterialCommoditiesMicroResources.CargoCount(hl.GetLast.MaterialCommodity));
            shipinfo = hl.GetLast?.ShipInformation;

            LoadRoute(GetSetting("route", ""));     // reload the route, may have locations now
            System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber}  History Load '{currentRoute?.Name}' {currentRoute?.Id} systems {currentRoute?.Systems.Count} lastsys '{last_sys?.Name}'");

            DrawSystem(last_sys, last_sys?.Name);    // may be null

            //t.Interval = 200; t.Tick += (s, e) => { var sys = currentRoute.PosAlongRoute(percent, 0); DrawSystem(sys, sys.Name); percent += 0.5; }; t.Start();  // debug to make it play thru.. leave
          //  t.Interval = 2000; t.Tick += (s, e) => { if (sysno < currentRoute.Systems.Count ){ var sys = currentRoute.KnownSystemList()[sysno++]; DrawSystem(sys.Item1, sys.Item1.Name); } }; t.Start();  // debug to make it play thru.. leave
        }
        //int sysno = 0; double percent = -10; Timer t = new Timer();// play thru harness

        private void Discoveryform_OnNewEntry(HistoryEntry he, HistoryList hl)
        {
            // received a new navroute, and we have navroute selected, reload
            if (he.EntryType == JournalTypeEnum.NavRoute && currentRoute != null && currentRoute.Id == -1)
            {
                System.Diagnostics.Debug.WriteLine("Surveyor {displaynumber} new entry, load nav route");
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
                // something has changed and just blindly for now recalc the fsd info
                shipfsdinfo = he.GetJumpInfo(discoveryform.history.MaterialCommoditiesMicroResources.CargoCount(he.MaterialCommodity));
                shipinfo = he.ShipInformation;

                if (last_sys == null || last_sys.Name != he.System.Name || !last_sys.HasCoordinate) // If not got a system, or different name, or last does not have co-ord (StartJump)
                {
                    last_sys = he.System;
                    DrawSystem(last_sys, last_sys.Name);
                }
                else if (he.EntryType == JournalTypeEnum.StartJump)         // so we can pre-present
                {
                    JournalStartJump jsj = he.journalEntry as JournalStartJump;
                    if (jsj.IsHyperspace)       // needs to be a hyperspace one, not supercruise
                    {
                        last_sys = new SystemClass(jsj.SystemAddress, jsj.StarSystem);       // important need system address as scan uses it for quick lookup
                        DrawSystem(last_sys, last_sys.Name);
                    }
                }
                else if (he.EntryType == JournalTypeEnum.FSSAllBodiesFound)     // since we present body counts
                {
                    DrawSystem(last_sys, last_sys.Name + " " + "System scan complete.".T(EDTx.UserControlSurveyor_Systemscancomplete));
                }
                else if (he.EntryType == JournalTypeEnum.FSSDiscoveryScan)      // since we present body counts
                {
                    var je = he.journalEntry as JournalFSSDiscoveryScan;
                    var bodies_found = je.BodyCount;
                    DrawSystem(last_sys, last_sys.Name + " " + bodies_found + " bodies found.".T(EDTx.UserControlSurveyor_bodiesfound));
                }
                else if (he.journalEntry is IStarScan || he.EntryType == JournalTypeEnum.FuelScoop)      // an entry to a scan node
                {
                    //System.Diagnostics.Debug.WriteLine("Scan got, sys " + he.System.Name + " " + last_sys.Name);
                    DrawSystem(last_sys);
                }
            }
        }

        private void Discoveryform_OnNewUIEvent(UIEvent uievent)
        {
            EliteDangerousCore.UIEvents.UIGUIFocus gui = uievent as EliteDangerousCore.UIEvents.UIGUIFocus;

            if (gui != null)
            {
                bool refresh = gui.GUIFocus != uistate;
                uistate = gui.GUIFocus;

                if (refresh)
                    DrawSystem(last_sys);
            }
        }

        public override void onControlTextVisibilityChanged(bool newvalue)       // user changed vis, update
        {
            DrawSystem(last_sys);
        }


        #endregion

        #region Main
        async private void DrawSystem(ISystem sys, string tt = null)
        {
            if (tt != null)
            {
                titletext = tt;
                SetControlText(tt);
            }

            var picelements = new List<ExtPictureBox.ImageElement>();       // accumulate picture elements in here and render under lock due to async below.

            System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} Draw {sys?.Name} {sys?.HasCoordinate}");

            // if system, and we are in no focus or don't care
            if (sys != null && (uistate == EliteDangerousCore.UIEvents.UIGUIFocus.Focus.NoFocus || !IsSet(CtrlList.autohide)
                                || (uistate == EliteDangerousCore.UIEvents.UIGUIFocus.Focus.FSSMode && IsSet(CtrlList.donthidefssmode))))
            {
                System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} Go for draw");

                int vpos = 0;
                StringFormat frmt = new StringFormat(extCheckBoxWordWrap.Checked ? 0 : StringFormatFlags.NoWrap);
                frmt.Alignment = alignment;
                var textcolour = IsTransparent ? discoveryform.theme.SPanelColor : discoveryform.theme.LabelColor;
                var backcolour = IsTransparent ? Color.Transparent : this.BackColor;

                if (IsSet(CtrlList.showsysinfo))
                {
                    var i = new ExtPictureBox.ImageElement();
                    i.TextAutoSize(
                            new Point(3, vpos),
                            new Size(Math.Max(pictureBoxSurveyor.Width - 6, 24), 10000),
                            titletext,
                            displayfont,
                            textcolour,
                            backcolour,
                            1.0F,
                            frmt: frmt);
                    picelements.Add(i);

                    vpos += i.Location.Height;
                }

                if (currentRoute != null && sys.HasCoordinate)      // if we have a route and a coord, we can work out the next route text
                {
                    System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} Current route set, sys has coord, route is {currentRoute.Systems.Count} {currentRoute.Id}");
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

                            System.Diagnostics.Debug.WriteLine($"Surveyor: {closest.lastsystem?.Name}->{closest.nextsystem?.Name} {closest.waypoint} distance to {closest.disttowaypoint} dev {closest.deviation} cuml after wp {closest.cumulativewpdist} inc wp {distleft} route {routedistance}");

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

                            System.Diagnostics.Debug.WriteLine(lastroutetext);
                            System.Diagnostics.Debug.WriteLine("---");

                            if (IsSet(RouteControl.showbookmarks))
                            {
                                BookmarkClass bookmark = GlobalBookMarkList.Instance.FindBookmarkOnSystem(sys.Name);
                                if (bookmark != null)
                                    lastroutetext += Environment.NewLine + String.Format("Note: {0}".T(EDTx.UserControlRouteTracker_Note), bookmark.Note);
                            }

                            string name = closest.nextsystem.Name;

                            if (lastsystem == null || name.CompareTo(lastsystem) != 0)
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

                                lastsystem = name;
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
                    System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} can't do route '{currentRoute?.Name}' {sys.HasCoordinate}");
                }

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

                    lastroutetext = lastroutetext.AppendPrePad(addtext, Environment.NewLine);

                }

                System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} last route text '{lastroutetext}'");

                if (lastroutetext.HasChars())     // if we have last route text, display it
                {
                    var i = new ExtPictureBox.ImageElement();
                    i.TextAutoSize(
                            new Point(3, vpos),
                            new Size(Math.Max(pictureBoxSurveyor.Width - 6, 24), 10000),
                            lastroutetext,
                            displayfont,
                            textcolour,
                            backcolour,
                            1.0F,
                            frmt: frmt);
                    picelements.Add(i);

                    vpos += i.Location.Height;
                }

                StarScan.SystemNode systemnode = await discoveryform.history.StarScan.FindSystemAsync(sys, checkBoxEDSM.Checked);        // get data with EDSM

                if (systemnode != null)     // no data, clear display, clear any last_he so samesys is false next time
                {
                    string infoline = "";

                    int scanned = checkBoxEDSM.Checked ? systemnode.StarPlanetsScanned() : systemnode.StarPlanetsScannednonEDSM();

                    if (scanned > 0)
                    {
                        infoline = "Scan".T(EDTx.UserControlSurveyor_Scan) + " " + scanned.ToString() + (systemnode.FSSTotalBodies != null ? (" / " + systemnode.FSSTotalBodies.Value.ToString()) : "");
                    }

                    long value = systemnode.ScanValue(false);

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
                            displayfont,
                            textcolour,
                            backcolour,
                            1.0F,
                            frmt: frmt);
                        picelements.Add(i);
                        vpos += i.Location.Height;
                    }

                    var all_nodes = systemnode.Bodies.ToList();

                    if (all_nodes != null)
                    {
                        bool sigchecked = IsSet(CtrlList.signals);
                        bool biosignalschecked = IsSet(CtrlList.BioSignals);
                        bool geosignalschecked = IsSet(CtrlList.GeoSignals);

                        value = 0;

                        foreach (StarScan.ScanNode sn in all_nodes)
                        {
                            if (sn.ScanData != null && sn.ScanData?.BodyName != null && (!sn.ScanData.IsEDSMBody || checkBoxEDSM.Checked))

                            {
                                var sd = sn.ScanData;

                                bool hasgeosignals = sn.Signals?.Find(x => x.IsGeo) != null;
                                bool hasbiosignals = sn.Signals?.Find(x => x.IsBio) != null;


                                if (
                                    (sd.IsLandable && IsSet(CtrlList.isLandable)) ||
                                    (sd.IsLandable && sd.HasAtmosphericComposition && IsSet(CtrlList.isLandableWithAtmosphere)) ||
                                    (sd.IsLandable && sd.HasMeaningfulVolcanism && IsSet(CtrlList.isLandableWithVolcanism)) ||
                                    (sd.IsLandable && sd.nRadius.HasValue && sd.nRadius >= largeRadiusLimit && IsSet(CtrlList.largelandable)) ||
                                    (sd.AmmoniaWorld && IsSet(CtrlList.showAmmonia)) ||
                                    (sd.Earthlike && IsSet(CtrlList.showEarthlike)) ||
                                    (sd.WaterWorld && IsSet(CtrlList.showWaterWorld)) ||
                                    (sd.PlanetTypeID == EDPlanet.High_metal_content_body && IsSet(CtrlList.showHMC)) ||
                                    (sd.PlanetTypeID == EDPlanet.Metal_rich_body && IsSet(CtrlList.showMR)) ||
                                    (sd.HasRings && IsSet(CtrlList.showRinged)) ||
                                    (sd.HasMeaningfulVolcanism && IsSet(CtrlList.showVolcanism)) ||
                                    (sd.nEccentricity.HasValue && sd.nEccentricity >= eccentricityLimit && IsSet(CtrlList.showEccentricity)) ||
                                    (sd.CanBeTerraformable && IsSet(CtrlList.showTerraformable)) ||
                                    (sd.IsPlanet && IsSet(CtrlList.lowradius) && sd.nRadius.HasValue && sd.nRadius < lowRadiusLimit) ||
                                    (sn.Signals != null && sigchecked) ||
                                    (hasgeosignals && geosignalschecked) ||
                                    (hasbiosignals && biosignalschecked) ||
                                    (sd.IsStar && IsSet(CtrlList.allstars)) ||
                                    (sd.IsPlanet && IsSet(CtrlList.allplanets)) ||
                                    (sd.IsBeltCluster && IsSet(CtrlList.beltclusters)))
                                {
                                    if (!sd.Mapped || IsSet(CtrlList.hideMapped) == false)      // if not mapped, or show mapped
                                    {
                                        bool hasthargoidsignals = sn.Signals?.Find(x => x.IsThargoid) != null;
                                        bool hasguardiansignals = sn.Signals?.Find(x => x.IsGuardian) != null;
                                        bool hashumansignals = sn.Signals?.Find(x => x.IsHuman) != null;
                                        bool hasothersignals = sn.Signals?.Find(x => x.IsOther) != null;
                                        bool hasminingsignals = sn.Signals?.Find(x => x.IsUncategorised) != null;


                                        var il = sd.SurveyorInfoLine(sys,
                                            hasminingsignals && sigchecked,  // show signals if we have some andthe all signals filter is checked
                                            hasgeosignals && (sigchecked || geosignalschecked || biosignalschecked), // show geological signals if there are any and any signal filter is checked (as there are bios that need geos to appear)
                                            hasbiosignals && (sigchecked || biosignalschecked), // show biological signals if there are any and the all signal filter or the bio signal filter is checked
                                            hasthargoidsignals && sigchecked, // show thargoid signals if there are any and the all signals filter is checked
                                            hasguardiansignals && sigchecked, // show guardian signals if there are any and the all signals filter is checked
                                            hashumansignals && sigchecked, // show human signals if there are any and the all signals filter is checked
                                            hasothersignals && sigchecked, // show other signals if there are any and the all signals filter is checked
                                            false,      // so this is the surveyor, we don't want to bother with showing if its got organics, since you have to scan them
                                            IsSet(CtrlList.showVolcanism) || (sd.IsLandable && IsSet(CtrlList.isLandableWithVolcanism))
                                                || (sd.IsLandable && IsSet(CtrlList.volcanism)), // any of these makes us need to show volcanic state
                                            IsSet(CtrlList.showValues),        // show values
                                            IsSet(CtrlList.moreinfo),   // show extra info such as mass/radius
                                            IsSet(CtrlList.showGravity),       // show gravity select
                                            sd.IsLandable && IsSet(CtrlList.atmos), // show atmosphere if landable (surveyor shows this if landable)
                                            IsSet(CtrlList.showRinged),          // show rings
                                            lowRadiusLimit, largeRadiusLimit, eccentricityLimit);

                                        //System.Diagnostics.Debug.WriteLine("Display " + il);
                                        var i = new ExtPictureBox.ImageElement();
                                        i.TextAutoSize(
                                                new Point(3, vpos),
                                                new Size(Math.Max(pictureBoxSurveyor.Width - 6, 24), 10000),
                                                il,
                                                displayfont,
                                                textcolour,
                                                backcolour,
                                                1.0F,
                                                frmt: frmt);
                                        picelements.Add(i);
                                        vpos += i.Location.Height;
                                        value += sd.EstimatedValue;
                                    }
                                }
                            }
                        }

                        if (value > 0 && IsSet(CtrlList.showValues))
                        {
                            var i = new ExtPictureBox.ImageElement();
                            i.TextAutoSize(
                                new Point(3, vpos),
                                new Size(Math.Max(pictureBoxSurveyor.Width - 6, 24), 10000),
                                "^^ ~ " + value.ToString("N0") + " cr",
                                displayfont,
                                textcolour,
                                backcolour,
                                1.0F,
                                frmt: frmt);
                            picelements.Add(i);
                            vpos += i.Location.Height;
                        }
                    }

                    if (fsssignalsdisplayed.HasChars())
                    {
                        string siglist = "";
                        string[] filter = fsssignalsdisplayed.Split(';');

                        // mirrors scandisplaynodes

                        var notexpired = systemnode.FSSSignalList.Where(x => !x.TimeRemaining.HasValue || x.ExpiryUTC >= DateTime.UtcNow).ToList();
                        notexpired.Sort(delegate (JournalFSSSignalDiscovered.FSSSignal l, JournalFSSSignalDiscovered.FSSSignal r) { return l.ClassOfSignal.CompareTo(r.ClassOfSignal); });

                        var expired = systemnode.FSSSignalList.Where(x => x.TimeRemaining.HasValue && x.ExpiryUTC < DateTime.UtcNow).ToList();
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
                                                            displayfont,
                                                            textcolour,
                                                            backcolour,
                                                            1.0F,
                                                            frmt: frmt);
                            picelements.Add(i);
                        }

                    }
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Surveyor ${displaynumber} display disabled");
            }

            lock ( extPictureBoxScroll)      // because of the async call above, we may be running two of these at the same time. So, we lock and then add/update/render
            {
                pictureBoxSurveyor.ClearImageList();
                pictureBoxSurveyor.AddRange(picelements);
                extPictureBoxScroll.Render();
                Refresh();
            }
        }

        private void UserControlSurveyor_Resize(object sender, EventArgs e)
        {
            DrawSystem(last_sys);
        }

        #endregion

        #region UI

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

            DialogResult res = f.ShowDialogCentred(this.FindForm(), this.FindForm().Icon, "List signals to display, semicolon seperated", closeicon: true);
            if (res == DialogResult.OK)
            {
                fsssignalsdisplayed = f.Get("Text");
                PutSetting("fsssignals", fsssignalsdisplayed);
                DrawSystem(last_sys);
            }
        }

        #endregion

        #region UI for Ctrl items

        protected enum CtrlList
        {
            allplanets, showAmmonia, showEarthlike, showWaterWorld, showHMC, showMR,
            showTerraformable, showVolcanism, showRinged, showEccentricity, lowradius,
            signals, GeoSignals, BioSignals, isLandable, isLandableWithAtmosphere,
            largelandable, isLandableWithVolcanism,
            // 18
            allstars, beltclusters,
            // 20
            showValues, moreinfo, showGravity, atmos, volcanism, autohide, donthidefssmode, hideMapped, showsysinfo,
            // 29
            alignleft, aligncenter, alignright
        };

        private bool[] ctrlset; // holds current state of each control above

        private bool IsSet(CtrlList v)
        {
            return ctrlset[(int)v];
        }

        // from DB, set up ctrlset, and set the defaults
        private void PopulateCtrlList()
        {
            ctrlset = new bool[Enum.GetNames(typeof(CtrlList)).Length];
            foreach (CtrlList e in Enum.GetValues(typeof(CtrlList)))
            {
                bool def = DefaultSetting(e);
                var v = GetSetting(e.ToString(), def);
                //System.Diagnostics.Debug.WriteLine($"Surveyor {e.ToString()} = {v}");
                ctrlset[(int)e] = v;
            }

            alignment = ctrlset[(int)CtrlList.alignright] ? StringAlignment.Far : ctrlset[(int)CtrlList.aligncenter] ? StringAlignment.Center : StringAlignment.Near;
        }

        protected virtual bool DefaultSetting(CtrlList e)
        {
            bool def = (e != CtrlList.alignright && e != CtrlList.aligncenter && e != CtrlList.autohide && e != CtrlList.lowradius
                && e != CtrlList.allplanets && e != CtrlList.allstars && e != CtrlList.beltclusters);
            return def;
        }

        private string CtrlStateAsString()      // returns all controls in one string, note Show below does not care about the extras
        {
            string s = "";
            foreach (CtrlList v in Enum.GetValues(typeof(CtrlList)))
            {
                if (ctrlset[(int)v])
                    s += v.ToString() + ";";
            }

            return s;
        }

        private void extButtonPlanets_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();

            displayfilter.AddAllNone();
            displayfilter.AddStandardOption(CtrlList.allplanets.ToString(), "Show All Planets".TxID("UserControlSurveyor.showAllPlanetsToolStripMenuItem"), BaseUtils.Icons.IconSet.GetIcon("Bodies.Planets.Terrestrial.HMCv10"));
            displayfilter.AddStandardOption(CtrlList.showAmmonia.ToString(), "Ammonia World".TxID("UserControlSurveyor.planetaryClassesToolStripMenuItem.ammoniaWorldToolStripMenuItem"), BaseUtils.Icons.IconSet.GetIcon("Bodies.Planets.Terrestrial.AMWv1"));
            displayfilter.AddStandardOption(CtrlList.showEarthlike.ToString(), "Earthlike World".TxID("UserControlSurveyor.planetaryClassesToolStripMenuItem.earthlikeWorldToolStripMenuItem"), BaseUtils.Icons.IconSet.GetIcon("Bodies.Planets.Terrestrial.ELWv5"));
            displayfilter.AddStandardOption(CtrlList.showWaterWorld.ToString(), "Water World".TxID("UserControlSurveyor.planetaryClassesToolStripMenuItem.waterWorldToolStripMenuItem"), BaseUtils.Icons.IconSet.GetIcon("Bodies.Planets.Terrestrial.WTRv7"));
            displayfilter.AddStandardOption(CtrlList.showHMC.ToString(), "High metal content body".TxID("UserControlSurveyor.planetaryClassesToolStripMenuItem.highMetalContentBodyToolStripMenuItem"), BaseUtils.Icons.IconSet.GetIcon("Bodies.Planets.Terrestrial.HMCv3"));
            displayfilter.AddStandardOption(CtrlList.showMR.ToString(), "Metal-rich body".TxID("UserControlSurveyor.planetaryClassesToolStripMenuItem.metalToolStripMenuItem"), BaseUtils.Icons.IconSet.GetIcon("Bodies.Planets.Terrestrial.MRBv5"));
            displayfilter.AddStandardOption(CtrlList.showTerraformable.ToString(), "Terraformable".TxID("UserControlSurveyor.bodyFeaturesToolStripMenuItem.terraformableToolStripMenuItem"), BaseUtils.Icons.IconSet.GetIcon("Bodies.Planets.Terrestrial.ELWv5"));
            displayfilter.AddStandardOption(CtrlList.showVolcanism.ToString(), "Has volcanism".TxID("UserControlSurveyor.bodyFeaturesToolStripMenuItem.hasVolcanismToolStripMenuItem"), BaseUtils.Icons.IconSet.GetIcon("Bodies.Planets.Terrestrial.HMCv37"));
            displayfilter.AddStandardOption(CtrlList.showRinged.ToString(), "Has Rings".TxID("UserControlSurveyor.bodyFeaturesToolStripMenuItem.hasRingsToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Bodies_RingOnly);
            displayfilter.AddStandardOption(CtrlList.showEccentricity.ToString(), "High eccentricity".TxID("UserControlSurveyor.bodyFeaturesToolStripMenuItem.highEccentricityToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Eccentric);
            displayfilter.AddStandardOption(CtrlList.lowradius.ToString(), "Tiny body".TxID("UserControlSurveyor.bodyFeaturesToolStripMenuItem.lowRadiusToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_SizeSmall);
            displayfilter.AddStandardOption(CtrlList.signals.ToString(), "Has Signals".TxID("UserControlSurveyor.bodyFeaturesToolStripMenuItem.hasSignalsToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Bodies_Signals);
            displayfilter.AddStandardOption(CtrlList.GeoSignals.ToString(), "Has geological signals".TxID("UserControlSurveyor.bodyFeaturesToolStripMenuItem.hasGeologicalSignalsToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Bodies_Signals);
            displayfilter.AddStandardOption(CtrlList.BioSignals.ToString(), "Has biological signals".TxID("UserControlSurveyor.bodyFeaturesToolStripMenuItem.hasBiologicalSignalsToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Bodies_Signals);
            displayfilter.AddStandardOption(CtrlList.isLandable.ToString(), "Landable".TxID("UserControlSurveyor.bodyFeaturesToolStripMenuItem.landableToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Bodies_LandablePlanet);
            displayfilter.AddStandardOption(CtrlList.isLandableWithAtmosphere.ToString(), "Landable with atmosphere".TxID("UserControlSurveyor.bodyFeaturesToolStripMenuItem.landableWithAtmosphereToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Bodies_LandablePlanet);
            displayfilter.AddStandardOption(CtrlList.largelandable.ToString(), "Landable and large".TxID("UserControlSurveyor.bodyFeaturesToolStripMenuItem.landableAndLargeToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Bodies_LandablePlanet);
            displayfilter.AddStandardOption(CtrlList.isLandableWithVolcanism.ToString(), "Landable with volcanism".TxID("UserControlSurveyor.bodyFeaturesToolStripMenuItem.landableWithVolcanismToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Bodies_LandablePlanet);

            string[] planetctrllist = Enum.GetNames(typeof(CtrlList)).RangeSubset((int)CtrlList.allplanets, 18);
            CommonCtrl(displayfilter, planetctrllist, extButtonPlanets);

        }

        private void extButtonStars_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();

            displayfilter.AddAllNone();
            displayfilter.AddStandardOption(CtrlList.allstars.ToString(), "Show All Stars".TxID("UserControlSurveyor.showAllStarsToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.beltclusters.ToString(), "Show Belt Clusters".TxID("UserControlSurveyor.showBeltClustersToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Belt);

            string[] starctrllist = Enum.GetNames(typeof(CtrlList)).RangeSubset((int)CtrlList.allstars, 2);
            CommonCtrl(displayfilter, starctrllist, extButtonStars);
        }

        private void extButtonShowControl_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();

            displayfilter.AddAllNone();
            displayfilter.AddStandardOption(CtrlList.showValues.ToString(), "Show values".TxID("UserControlSurveyor.showValuesToolStripMenuItem"));
            displayfilter.AddStandardOption(CtrlList.moreinfo.ToString(), "Show more information".TxID("UserControlSurveyor.showMoreInformationToolStripMenuItem"));
            displayfilter.AddStandardOption(CtrlList.showGravity.ToString(), "Show gravity of landables".TxID("UserControlSurveyor.showGravityToolStripMenuItem"));
            displayfilter.AddStandardOption(CtrlList.atmos.ToString(), "Show atmosphere of landables".TxID("UserControlSurveyor.showAtmosToolStripMenuItem"));
            displayfilter.AddStandardOption(CtrlList.volcanism.ToString(), "Show volcanism of landables".TxID("UserControlSurveyor.showVolcanismToolStripMenuItem"));
            displayfilter.AddStandardOption(CtrlList.autohide.ToString(), "Auto Hide".TxID("UserControlSurveyor.autoHideToolStripMenuItem"));
            displayfilter.AddStandardOption(CtrlList.donthidefssmode.ToString(), "Don't hide in FSS Mode".TxID("UserControlSurveyor.dontHideInFSSModeToolStripMenuItem"));
            displayfilter.AddStandardOption(CtrlList.hideMapped.ToString(), "Hide already mapped bodies".TxID("UserControlSurveyor.hideAlreadyMappedBodiesToolStripMenuItem"));
            displayfilter.AddStandardOption(CtrlList.showsysinfo.ToString(), "Show System Info Always".TxID("UserControlSurveyor.showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem"));

            string[] showctrllist = Enum.GetNames(typeof(CtrlList)).RangeSubset((int)CtrlList.showValues, 9);
            CommonCtrl(displayfilter, showctrllist, extButtonShowControl);
        }

        #endregion

        private void extButtonAlignment_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();

            string lt = CtrlList.alignleft.ToString();
            string ct = CtrlList.aligncenter.ToString();
            string rt = CtrlList.alignright.ToString();

            displayfilter.AddStandardOption(lt, "Alignment Left".TxID("UserControlSurveyor.textAlignToolStripMenuItem.leftToolStripMenuItem"), global::EDDiscovery.Icons.Controls.AlignLeft, exclusivetags: ct + ";" + rt, disableuncheck: true);
            displayfilter.AddStandardOption(ct, "Alignment Center".TxID("UserControlSurveyor.textAlignToolStripMenuItem.centerToolStripMenuItem"), global::EDDiscovery.Icons.Controls.AlignCentre, exclusivetags: lt + ";" + rt, disableuncheck: true);
            displayfilter.AddStandardOption(rt, "Alignment Right".TxID("UserControlSurveyor.textAlignToolStripMenuItem.rightToolStripMenuItem"), global::EDDiscovery.Icons.Controls.AlignRight, exclusivetags: lt + ";" + ct, disableuncheck: true);

            string[] showctrllist = Enum.GetNames(typeof(CtrlList)).RangeSubset((int)CtrlList.alignleft, 3);
            CommonCtrl(displayfilter, showctrllist, extButtonAlignment);
        }

        private void CommonCtrl(ExtendedControls.CheckedIconListBoxFormGroup displayfilter, string[] list, Control under)
        {
            displayfilter.AllOrNoneBack = false;
            displayfilter.ImageSize = new Size(24, 24);
            displayfilter.ScreenMargin = new Size(0, 0);

            displayfilter.SaveSettings = (s, o) =>
            {
                PutBoolSettingsFromString(s, list);
                PopulateCtrlList();
                DrawSystem(last_sys);
            };

            displayfilter.Show(CtrlStateAsString(), under, this.FindForm());
        }

        private void extButtonFont_Click(object sender, EventArgs e)
        {
            Font f = FontHelpers.FontSelection(this.FindForm(), displayfont);
            string setting = FontHelpers.GetFontSettingString(f);
            System.Diagnostics.Debug.WriteLine($"Surveyor Font selected {setting}");
            PutSetting("font", setting);
            displayfont = f != null ? f : discoveryform.theme.GetFont;
            DrawSystem(last_sys);
        }


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

            EDDTheme.Instance.ApplyDialog(dropdown, true);
            dropdown.Show(this.FindForm());
        }

        private void LoadRoute(string name)
        {
            System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} Order load of route '{name}'");
            PutSetting("route", name);      // store back the current name 

            System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} In DB its now '{GetSetting("route","???")}'");

            currentRoute = null;
            lastroutetext = null;       // reset the route text stored due to start jump not having coords

            if (name.HasChars())
            {
                if (name.Equals(NavRouteNameLabel))
                {
                    var route = discoveryform.history.GetLastHistoryEntry(x => x.EntryType == JournalTypeEnum.NavRoute)?.journalEntry as EliteDangerousCore.JournalEvents.JournalNavRoute;
                    if (route?.Route != null)
                    {
                        var systems = route.Route.Where(x => x.StarSystem.HasChars()).Select(y => y.StarSystem).ToArray();
                        currentRoute = new SavedRouteClass(translatednavroutename, systems);      // with an ID of -1 note
                        System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} Loaded Nav route with {systems.Length}");
                    }
                    else
                    {
                        currentRoute = new SavedRouteClass(translatednavroutename, new string[] { });     // no known systems yet, but make a navroute so we have it selected
                        System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} No route available, loaded empty Nav route");

                    }
                }
                else
                {
                    var savedroutes = SavedRouteClass.GetAllSavedRoutes();      // load routes
                    currentRoute = savedroutes.Find(x => x.Name == name);       // pick, if not found, will be null
                    System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} Loaded route with {currentRoute?.Systems.Count}");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} No route wanted '{name}'");
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
            displayfilter.AddStandardOption(RouteControl.showJumps.ToString(), "Show Jumps To Go".TxID("UserControlRouteTracker.showJumpsToolStripMenuItem"));
            displayfilter.AddStandardOption(RouteControl.showwaypoints.ToString(), "Show Waypoint Coordinates".TxID("UserControlRouteTracker.showWaypointCoordinatesToolStripMenuItem"));
            displayfilter.AddStandardOption(RouteControl.showdeviation.ToString(), "Show Deviation from route".TxID("UserControlRouteTracker.showDeviationFromRouteToolStripMenuItem"));
            displayfilter.AddStandardOption(RouteControl.showbookmarks.ToString(), "Show Bookmark Notes".TxID("UserControlRouteTracker.showBookmarkNotesToolStripMenuItem"));
            displayfilter.AddStandardOption(RouteControl.autocopy.ToString(), "Auto copy waypoint".TxID("UserControlRouteTracker.autoCopyWPToolStripMenuItem"));
            displayfilter.AddStandardOption(RouteControl.settarget.ToString(), "Auto set target".TxID("UserControlRouteTracker.autoSetTargetToolStripMenuItem"));
            displayfilter.AddStandardOption(RouteControl.showtarget.ToString(), "Show Target Information".TxID("UserControlRouteTracker.showtargetinfo"));
            displayfilter.AddStandardOption(RouteControl.showfuel.ToString(), "Show Fuel Information".TxID("UserControlRouteTracker.showfuelinfo"));

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
