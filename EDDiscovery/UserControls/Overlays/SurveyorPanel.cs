/*
 * Copyright 2016 - 2025 EDDiscovery development team
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
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class SurveyorPanel : UserControlCommonBase
    {
        #region Initialisation

        public SurveyorPanel()
        {
            InitializeComponent();
            BaseUtils.TranslatorMkII.Instance.TranslateTooltip(toolTip, this);

            DBBaseName = "Surveyor";
        }
        protected override void Init()
        {
            edsmSpanshButton.Init(this, "EDSMSpansh", "");
            edsmSpanshButton.ValueChanged += (s, ch) =>
            {
                DrawAll(cur_sys);
            };

            PopulateCtrlList();

            extCheckBoxWordWrap.Checked = GetSetting(dbWordWrap, false);
            extCheckBoxWordWrap.Click += wordWrapToolStripMenuItem_Click;

            fsssignalstodisplay = GetSetting(dbfsssignals, "");

            DiscoveryForm.OnNewUIEvent += Discoveryform_OnNewUIEvent;
            DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange;
            DiscoveryForm.OnNewEntry += Discoveryform_OnNewEntry;
            DiscoveryForm.OnNewTarget += Discoveryform_OnNewTarget;
            DiscoveryForm.OnThemeChanged += DiscoveryForm_OnThemeChanged;
            GlobalBookMarkList.Instance.OnBookmarkChange += GlobalBookMarkList_OnBookmarkChange;

            rollUpPanelTop.SetToolTip(toolTip);

            translatednavroutename = "Nav Route".Tx();

            displayfont = BaseUtils.FontHandler.GetFontFromSetting(GetSetting(dbFont, ""), null);        // null if not set

            LoadRoute(GetSetting("route", ""));
            routecontrolsettings = GetSetting(dbroutecontrol, "showJumps;showwaypoints;shownotetext");

            rollUpPanelTop.PinState = GetSetting(dbpinstate, true);

            drawsystemupdatetimer = new Timer() { Interval = 500 };
            drawsystemupdatetimer.Tick += DrawSystemUpdatetimer_Tick;
        }

        protected override void InitialDisplay()
        {
            RequestPanelOperation(this, new UserControlCommonBase.RequestHistoryGridPos());     //request an update 
            SetVisibility();
            doresize = true;                            // now allow resizing actions, before, resizes were due to setups, now due to user interactions
        }

        protected override void Closing()
        {
            drawsystemupdatetimer.Stop();

            PutSetting(dbpinstate, rollUpPanelTop.PinState);

            DiscoveryForm.OnNewUIEvent -= Discoveryform_OnNewUIEvent;
            DiscoveryForm.OnHistoryChange -= Discoveryform_OnHistoryChange;
            DiscoveryForm.OnNewEntry -= Discoveryform_OnNewEntry;
            DiscoveryForm.OnNewTarget -= Discoveryform_OnNewTarget;
            DiscoveryForm.OnThemeChanged -= DiscoveryForm_OnThemeChanged;
            GlobalBookMarkList.Instance.OnBookmarkChange -= GlobalBookMarkList_OnBookmarkChange;
        }

        private void Discoveryform_OnHistoryChange()
        {
            var hl = DiscoveryForm.History;
            cur_sys = hl.GetLast?.System;      // may be null
            shipfsdinfo = hl.GetLast?.GetJumpInfo(DiscoveryForm.History.MaterialCommoditiesMicroResources.CargoCount(hl.GetLast.MaterialCommodity));
            shipinfo = hl.GetLast?.ShipInformation;

            LoadRoute(GetSetting(dbRouteName, ""), GetSetting(dbRouteManualPos, -1));
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
                LoadRoute(NavRouteNameLabel, GetSetting(dbRouteManualPos, -1));        // into auto mode
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

        private void DiscoveryForm_OnThemeChanged()
        {
            DrawAll(cur_sys);
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
        protected override void SetTransparency(bool on, Color curcol)
        {
            //System.Diagnostics.Debug.WriteLine($"Surveyor set transparency {on} {curcol}");

            extPictureBoxScrollSystemDetails.ScrollBarEnabled = !on;     // turn off the scroll bar if its transparent

            this.BackColor = curcol;
            extPictureBoxScrollSystemDetails.BackColor =  curcol;
            rollUpPanelTop.Visible = !on;
            SetVisibility(!on);     // set our visible mode for text, but override to on if we are not transparent
        }

        protected override void TransparencyModeChanged(bool on)
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
                    CheckManualTarget();
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
                    playedtriggers = new Dictionary<string, HashSet<string>>();

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
                CheckManualTarget();
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
                text += " | " + bodies_found + " bodies detected.".Tx();
            if (all_found)
                text += " | " + "System scan complete".Tx()+ ": " + bodies_found + " bodies found.".Tx();

            SetControlText(text);
            ClearThenDrawText(extPictureBoxTitle, text);
        }

          
        // calculate then draw the scan summmary info. Await
        private async void CalculateThenDrawScanSummary(ISystem sys)
        {
            // System.Diagnostics.Debug.WriteLine($"Surveyor scan summary ${sys?.Name}");

            scansummarytext = "";

            if (sys != null)
            {
                var systemnode = await DiscoveryForm.History.StarScan2.FindSystemAsync(sys, edsmSpanshButton.WebLookup);        // get data with EDSM
                if (IsClosed)   // may close during await..
                    return;

                if (sys != cur_sys)
                {
                    System.Diagnostics.Debug.WriteLine($"{Environment.TickCount%10000} Surveyor Cancelled CalculateThenDrawScanSummary as cursys has changed during await");
                    return;
                }

                if (systemnode != null)
                {
                    int scanned = systemnode.StarPlanetsScanned(edsmSpanshButton.WebLookup);
                    int clusters = systemnode.BeltClusterBodies();

                    if (scanned > 0)
                    {
                        scansummarytext = scansummarytext.AppendPrePad("Scan".Tx()+ " " + scanned.ToString() + (systemnode.FSSTotalBodies != null ? (" / " + systemnode.FSSTotalBodies.Value.ToString()) : ""), Environment.NewLine);
                    }

                    long value = systemnode.ScanValue(false);   // don't include value of web bodies

                    if (value > 0 && IsSet(CtrlList.showValues))
                    {
                        scansummarytext.AppendPrePad("Scan".Tx()+ " " + scanned.ToString() + (systemnode.FSSTotalBodies != null ? (" / " + systemnode.FSSTotalBodies.Value.ToString()) : "", Environment.NewLine));
                        scansummarytext = scansummarytext.AppendPrePad("~ " + value.ToString("N0") + " cr" + ";");                        
                    }
                    if (systemnode.FSSTotalNonBodies != null && IsSet(CtrlList.showsignalmismatch))
                    {
                        scansummarytext = scansummarytext.AppendPrePad(systemnode.FSSTotalNonBodies.Value != clusters ? " Cluster and NonBody counts differ by ".Tx()+ Math.Abs(systemnode.FSSTotalNonBodies.Value - clusters).ToString("N0") : "");
                    }
                    
                }
            }

            lock (scansummarytext)
                DrawScanSummaryUnlocked();
        }

        private void DrawScanSummaryUnlocked()
        {
            ClearThenDrawText(extPictureBoxScanSummary, scansummarytext);
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
                Dictionary<string, HashSet<string>> cursystriggers = new Dictionary<string, HashSet<string>>();

                // perform searches, and store them in searchresults, keyed by body name matched

                var searchresults = new Dictionary<string, List<HistoryListQueries.ResultEntry>>();

                if (searchesactivetext.Length > 0 || searchesactivevoice.Length>0)       // if any searches
                {
                    DiscoveryForm.History.FillInScanNode();     // ensure all journal scan entries point to a scan node (expensive, done only when required in this panel)

                    // all entries related to sys.  Can't really limit the pick up as tried before using the afterlastevent option in this call
                    // due to being able to browse back in history. We may not be at the end of the list the system we are displaying. For now, just do a blind whole history search

                    //System.Diagnostics.Debug.WriteLine($"Find He's for system {sys}");

                    // for FSSsignal discovered (oct 25) we need to check the signals address is right, can't rely on the He.System as it may have been written in previous system

                    var helist = HistoryList.FilterByEventEntryOrder(DiscoveryForm.History.EntryOrder(), HistoryListQueries.AllSearchableJournalTypes,
                        (x) => x.EntryType == JournalTypeEnum.FSSSignalDiscovered ? (x.journalEntry as JournalFSSSignalDiscovered).IsSignalsOfSystem(sys.SystemAddress) : x.System.SystemAddress == sys.SystemAddress);

                    if (helist.Count > 0)        // no point executing if nothing in helist
                    {
                        //foreach (var h in helist) { if (h.journalEntry is JournalFSSSignalDiscovered sd) { System.Diagnostics.Debug.WriteLine($"FSS Signal {sd.Signals[0].SystemAddress} vs {sys.SystemAddress}, {sd.GetDetailed(null)}"); } }

                        var defaultvars = new BaseUtils.Variables();
                        defaultvars.AddPropertiesFieldsOfClass(new BodyPhysicalConstants(), "", null, 10, ensuredoublerep: true);

                        System.Diagnostics.Debug.WriteLine($"{Environment.TickCount%10000} ... Surveyor runs {searchesactivetext.Length} searches");

                        // check all voice and text actives

                        var allsearches = searchesactivetext.Union(searchesactivevoice).ToArray();

                        foreach (var searchname in allsearches)
                        {
                            // await is horrible, anything can happen, even closing
                            await HistoryListQueries.Instance.Find(helist, searchresults, searchname, defaultvars, DiscoveryForm.History.StarScan2, false); // execute the searches

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

                var systemnode = await DiscoveryForm.History.StarScan2.FindSystemAsync(sys, edsmSpanshButton.WebLookup);       
                if (IsClosed)   // may close during await..
                    return;

                // produce body event triggers only if we have seen events, so don't produce them on startup
                bool producebodytriggers = is_latest && eventsseen > 0;

                // produce search triggers, normally the same as body, but two of them exist for debugging
                bool producesearchtriggers = producebodytriggers;

                // if to play if we already have played a tigger
                bool playalwaystriggers = false;

//  producebodytriggers = true;
//producesearchtriggers = true;
//  playalwaystriggers = true;

                if (systemnode != null)     // if we have a node (should do of course since july 22 due to AddLocation in scan node)
                {
                    System.Diagnostics.Debug.WriteLine($"{Environment.TickCount % 10000} Surveyor Process node {sys.Name}");

                    foreach (var sn in systemnode.Bodies(x=>x.Scan!=null))        // only nodes with scan data can be treated here
                    {
                        var sd = sn.Scan;

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

                        // for this body, put into cursystriggers a fresh hashset
                        cursystriggers[sd.BodyName] = new HashSet<string>();

                        // this is an alias to cursystriggers
                        HashSet<string> cursystriggersalias = cursystriggers[sd.BodyName];

                        // we only send trigger actions when we are tracking the latest travel history, not when we are back in the history
                        // and we have had new events come in (showing we are not just started up cold)
                        if (producebodytriggers)
                        {
                            // test
                            //en.Add(TTThargoidSignals); en.Add(TTMiningSignals); en.Add(TTHighGravity + sd.nSurfaceGravityG.Value.ToStringInvariant("#.##")); en.Add(TTHuge);

                            if (hasminingsignals) cursystriggersalias.Add(TTMiningSignals);
                            if (hasgeosignals) cursystriggersalias.Add(TTGeoSignals);
                            if (hasbiosignals) cursystriggersalias.Add(TTBioSignals);
                            if (hasthargoidsignals) cursystriggersalias.Add(TTThargoidSignals);
                            if (hasguardiansignals) cursystriggersalias.Add(TTGuardianSignals);
                            if (hashumansignals) cursystriggersalias.Add(TTHumanSignals);
                            if (hasothersignals) cursystriggersalias.Add(TTOtherSignals);
                            if (sd.CanBeTerraformable) cursystriggersalias.Add(TTCanBeTerraformed);
                            if (sd.IsLandable) cursystriggersalias.Add(TTLandable);
                            if (sd.HasMeaningfulVolcanism) cursystriggersalias.Add(TTVolcanism);
                            if (sd.HasRings) cursystriggersalias.Add(TTRings);
                            if (sd.HasBelts) cursystriggersalias.Add(TTBelts);
                            if (sd.Earthlike) cursystriggersalias.Add(TTEarthlike);
                            if (sd.WaterWorld) cursystriggersalias.Add(TTWaterWorld);
                            if (sd.AmmoniaWorld) cursystriggersalias.Add(TTAmmoniaWorld);
                            if (sd.nEccentricity >= eccentricityLimit) cursystriggersalias.Add(TTEccentric);
                            if (sd.nRadius < lowRadiusLimit && sd.IsPlanet) cursystriggersalias.Add(TTTiny);
                            if (sd.nRadius > largeRadiusLimit && sd.IsPlanet && sd.IsLandable) cursystriggersalias.Add(TTHuge);
                            if (sd.HasAtmosphericComposition) cursystriggersalias.Add(TTAtmosphere + sd.AtmosphereTranslated);
                            if (sd.IsPlanet && sd.nSurfaceGravityG >= 2) cursystriggersalias.Add(TTHighGravity + sd.nSurfaceGravityG.Value.ToStringInvariant("#.##"));
                        }

                        // compute if we want search results displayed

                        searchresults.TryGetValue(sn.CanonicalNameOrOwnName, out List<HistoryListQueries.ResultEntry> searchresultfornode);     // will be null if not found

                        var surveyordisplay = searchresultfornode != null;      // if we have a search node, display

                        // work out if we want to display if we don't have searchresult, as long as not websourced or allow web sourced

                        if (surveyordisplay == false && (!sd.IsWebSourced || edsmSpanshButton.WebLookup)) 
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
                                (sd.IsBeltClusterBody && IsSet(CtrlList.beltclusters));

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
                                    if (searchesactivetext.Contains(hrentry.FilterPassed))   // if its a text entry search
                                        searchpasslist = searchpasslist.AppendPrePad(hrentry.FilterPassed, ", ");

                                    if (producesearchtriggers && searchesactivevoice.Contains(hrentry.FilterPassed))   // if its a voice entry
                                        cursystriggersalias.Add(TTDiscovery + hrentry.FilterPassed);       // add a trigger which is called discovery:filter name
                                }

                                if ( searchpasslist.HasChars())
                                    silstring += " : " + searchpasslist;
                            }

                            ldrawsystemtext[sd.BodyName] = silstring;

                            ldrawsystemvalue += sd.EstimatedValue;
                        }

                        // if we had a search result, remove it from the list as we have considered it above.  Even if we decided not to print it!
                        if ( searchresultfornode != null )
                            searchresults.Remove(sn.CanonicalNameOrOwnName);

                    }   // end for..
                }       // end of system node look thru

                // Any searches left print - they may have been triggered outside of a scan node
                // so we need to consider them

                foreach (var kvp in searchresults.EmptyIfNull())            // by bodyname
                {
                    string searchpasslist = "";

                    foreach (HistoryListQueries.ResultEntry hrentry in kvp.Value)
                    {
                        if (searchesactivetext.Contains(hrentry.FilterPassed))   // if its a text entry
                            searchpasslist = searchpasslist.AppendPrePad(hrentry.FilterPassed, ", ");

                        if (producesearchtriggers && searchesactivevoice.Contains(hrentry.FilterPassed))   // if its a voice entry
                        {
                            if (!cursystriggers.TryGetValue(kvp.Key, out HashSet<string> list))     // if we did not make the body above, make it..
                                cursystriggers.Add(kvp.Key, new HashSet<string>());

                            cursystriggers[kvp.Key].Add(TTDiscovery + hrentry.FilterPassed);       // add a trigger
                        }
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

                    var signallist = systemnode.FSSSignals ?? new List<FSSSignal>();         // This can be null, if so, make an empty array so the where does not except.

                    // mirrors scandisplaynodes

                    // was:
                    //var notexpired = signallist.Where(x => (!x.SignalName.Contains("-class") && (!x.TimeRemaining.HasValue || x.ExpiryUTC >= DateTime.UtcNow)) || 
                    //(x.SignalName.Contains("-class") && x.RecordedUTC > DateTime.UtcNow.AddDays(-14))).ToList();

                    // should be:
                    var notexpired = signallist.Where(x => (x.ClassOfSignal != SignalDefinitions.Classification.Megaship && (!x.TimeRemaining.HasValue || x.ExpiryUTC >= DateTime.UtcNow)) ||
                                                            (x.ClassOfSignal == SignalDefinitions.Classification.Megaship && x.RecordedUTC > DateTime.UtcNow.AddDays(-14))).ToList();

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
                                ldrawsystemsignallist = ldrawsystemsignallist.AppendPrePad("Expired".Tx()+": ", Environment.NewLine + Environment.NewLine);

                            ldrawsystemsignallist = ldrawsystemsignallist.AppendPrePad(fsssig.ToString(), Environment.NewLine);
                        }
                    }
                }

                // we have collected triggers for this system in cursystriggers
                // send them out, triaging for triggers already sent by body name

                foreach (var kvp in cursystriggers)
                {
                    BaseUtils.Variables v = new BaseUtils.Variables();

                    int index = 1;
                    foreach (string str in kvp.Value)          // this is all the triggers for this kvp.Key body
                    {
                        // if played triggers, for this body, does not have a hash set, make one
                        if (!playedtriggers.TryGetValue(kvp.Key, out HashSet<string> hs))      
                            hs = playedtriggers[kvp.Key] = new HashSet<string>();

                        if (playalwaystriggers || !hs.Contains(str))     // if play always, or its a new trigger to the body
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
            int setwidth = alignment != StringAlignment.Near ? extPictureBoxSystemDetails.Width - 6 : -1;   // so, if we are not using far, we now force the bitwidth to max in draw
            var textcolour = IsTransparentModeOn ? ExtendedControls.Theme.Current.SPanelColor : ExtendedControls.Theme.Current.LabelColor;
            var backcolour = IsTransparentModeOn ? Color.Transparent : this.BackColor;
            Font dfont = displayfont ?? this.Font;

            foreach (var kvp in drawsystemtext)        // and present any text results in sorted order
            {
                var i = new ExtendedControls.ImageElement.Element();
                i.TextAutoSize( new Point(3, vpos),
                        new Size(Math.Max(extPictureBoxSystemDetails.Width - 6, 24), 10000),
                        kvp.Value,
                        dfont,
                        textcolour,
                        backcolour,
                        1.0F,
                        frmt: frmt,
                        setwidth: setwidth);
                extPictureBoxSystemDetails.Add(i);
                vpos += i.Bounds.Height;
            }

            if (drawsystemvalue > 0 && IsSet(CtrlList.showValues))
            {
                var i = new ExtendedControls.ImageElement.Element();
                i.TextAutoSize( new Point(3, vpos),
                    new Size(Math.Max(extPictureBoxSystemDetails.Width - 6, 24), 10000),
                    "^^ ~ " + drawsystemvalue.ToString("N0") + " cr",
                    dfont,
                    textcolour,
                    backcolour,
                    1.0F,
                    frmt: frmt,
                    setwidth: setwidth);
                extPictureBoxSystemDetails.Add(i);
                vpos += i.Bounds.Height;
            }

            if (drawsystemsignallist.HasChars())
            {
                //System.Diagnostics.Debug.WriteLine("Display " + siglist);
                var i = new ExtendedControls.ImageElement.Element();
                i.TextAutoSize(new Point(3, vpos),
                                                new Size(Math.Max(extPictureBoxSystemDetails.Width - 6, 24), 10000),
                                                drawsystemsignallist,
                                                dfont,
                                                textcolour,
                                                backcolour,
                                                1.0F,
                                                frmt: frmt,
                                                setwidth:setwidth);
                vpos += i.Bounds.Height;
                extPictureBoxSystemDetails.Add(i);
            }

            frmt.Dispose();

            extPictureBoxScrollSystemDetails.Render();
            Refresh();
        }

        void ClearThenDrawText(ExtPictureBox pb, string text)
        {
            pb.ClearImageList();
            pb.AddPictureTextHorzDivider(new Rectangle(3, 0, Math.Max(extPictureBoxSystemDetails.Width - 6, 24), 10000),
                                         null, Size.Empty,
                                         text, displayfont ?? this.Font, extCheckBoxWordWrap.Checked, alignment,
                                         IsSet(CtrlList.showdividers) && text.HasChars(),
                                         IsTransparentModeOn ? ExtendedControls.Theme.Current.SPanelColor : ExtendedControls.Theme.Current.LabelColor,
                                         IsTransparentModeOn ? Color.Transparent : this.BackColor
                                         );
            pb.Render();
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

        Dictionary<string, HashSet<string>> playedtriggers = new Dictionary<string, HashSet<string>>();    // triggers already played

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
        public const string dbRouteName = "route";
        public const string dbRouteManualPos = "routepos";
        public const string dbFont = "font";
        public const string dbWordWrap = "wordwrap";
        public const string dbfsssignals = "fsssignals";
        public const string dbroutecontrol = "routecontrol";
        public const string dbpinstate = "PinState";
        #endregion

    }

    public class UserControlRouteTracker : SurveyorPanel
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
