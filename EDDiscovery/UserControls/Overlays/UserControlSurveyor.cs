/*
 * Copyright © 2016 - 2019 EDDiscovery development team
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
using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlSurveyor : UserControlCommonBase
    {
        ISystem last_sys = null;

        private System.Drawing.StringAlignment alignment = System.Drawing.StringAlignment.Near;
        private string titletext = "";
        private string fsssignalsdisplayed = "";

        const int lowRadiusLimit = 300 * 1000; // tiny body limit in km converted to m
        const int largeRadiusLimit = 20000 * 1000; // large body limit in km converted to m
        const double eccentricityLimit = 0.95; //orbital eccentricity limit

        EliteDangerousCore.UIEvents.UIGUIFocus.Focus uistate = EliteDangerousCore.UIEvents.UIGUIFocus.Focus.NoFocus;

        public UserControlSurveyor()
        {
            InitializeComponent();
        }

        #region Overrides

        public override void Init()
        {
            DBBaseName = "Surveyor";

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);
            BaseUtils.Translator.Instance.Translate(contextMenuStrip, this);

            // set context menu checkboxes
            showValuesToolStripMenuItem.Checked = GetSetting("showValues", true);
            ammoniaWorldToolStripMenuItem.Checked = GetSetting("showAmmonia", true);
            earthlikeWorldToolStripMenuItem.Checked = GetSetting("showEarthlike", true);
            waterWorldToolStripMenuItem.Checked = GetSetting("showWaterWorld", true);
            highMetalContentBodyToolStripMenuItem.Checked = GetSetting("showHMC", true);
            metalToolStripMenuItem.Checked = GetSetting("showMR", true);
            terraformableToolStripMenuItem.Checked = GetSetting("showTerraformable", true);
            hasVolcanismToolStripMenuItem.Checked = GetSetting("showVolcanism", true);
            highEccentricityToolStripMenuItem.Checked = GetSetting("showEccentricity", true);
            landableToolStripMenuItem.Checked = GetSetting("isLandable", true);
            landableWithAtmosphereToolStripMenuItem.Checked = GetSetting("isLandableWithAtmosphere", true);
            landableWithVolcanismToolStripMenuItem.Checked = GetSetting("isLandableWithVolcanism", true);
            landableAndLargeToolStripMenuItem.Checked = GetSetting("largelandable", true);
            showAtmosToolStripMenuItem.Checked = GetSetting("atmos", true);
            hasRingsToolStripMenuItem.Checked = GetSetting("showRinged", true);
            showGravityToolStripMenuItem.Checked = GetSetting("showGravity", true);
            showVolcanismToolStripMenuItem.Checked = GetSetting("volcanism", true);
            hideAlreadyMappedBodiesToolStripMenuItem.Checked = GetSetting("hideMapped", true);
            autoHideToolStripMenuItem.Checked = GetSetting("autohide", false);
            lowRadiusToolStripMenuItem.Checked = GetSetting("lowradius", false);
            checkEDSMForInformationToolStripMenuItem.Checked = GetSetting("edsm", false);
            showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem.Checked = GetSetting("showsysinfo", true);
            dontHideInFSSModeToolStripMenuItem.Checked = GetSetting("donthidefssmode", true);
            hasSignalsToolStripMenuItem.Checked = GetSetting("signals", true);
            hasGeologicalSignalsToolStripMenuItem.Checked = GetSetting("GeoSignals", true);
            hasBiologicalSignalsToolStripMenuItem.Checked = GetSetting("BioSignals", true);
            showAllPlanetsToolStripMenuItem.Checked = GetSetting("allplanets", false);
            showAllStarsToolStripMenuItem.Checked = GetSetting("allstars", false);
            showBeltClustersToolStripMenuItem.Checked = GetSetting("beltclusters", false);
            showMoreInformationToolStripMenuItem.Checked = GetSetting("moreinfo", true);
            wordWrapToolStripMenuItem.Checked = GetSetting("wordwrap", false);

            fsssignalsdisplayed = GetSetting("fsssignals", "");

            SetAlign((StringAlignment)GetSetting("align", 0));

            // install the handlers AFTER setup otherwise you get lots of events

            // planetary classes
            this.ammoniaWorldToolStripMenuItem.Click += new System.EventHandler(this.ammoniaWorldToolStripMenuItem_Click);
            this.earthlikeWorldToolStripMenuItem.Click += new System.EventHandler(this.earthlikeWorldToolStripMenuItem_Click);
            this.waterWorldToolStripMenuItem.Click += new System.EventHandler(this.waterWorldToolStripMenuItem_Click);
            this.highMetalContentBodyToolStripMenuItem.Click += new System.EventHandler(this.highMetalContentBodyToolStripMenuItem_Click);
            this.metalToolStripMenuItem.Click += new System.EventHandler(this.metalToolStripMenuItem_Click);

            // body features
            this.terraformableToolStripMenuItem.Click += new System.EventHandler(this.terraformableToolStripMenuItem_Click);
            this.hasVolcanismToolStripMenuItem.Click += new System.EventHandler(this.hasVolcanismToolStripMenuItem_Click);
            this.hasRingsToolStripMenuItem.Click += new System.EventHandler(this.hasRingsToolStripMenuItem_Click);
            this.highEccentricityToolStripMenuItem.Click += new System.EventHandler(this.highEccentricityToolStripMenuItem_Click);
            this.lowRadiusToolStripMenuItem.Click += new System.EventHandler(this.lowRadiusToolStripMenuItem_Click);
            this.hasSignalsToolStripMenuItem.Click += new System.EventHandler(this.hasSignalsToolStripMenuItem_Click);
            this.hasGeologicalSignalsToolStripMenuItem.Click += new System.EventHandler(this.hasSignalsToolStripMenuItem_Click);
            this.hasBiologicalSignalsToolStripMenuItem.Click += new System.EventHandler(this.hasSignalsToolStripMenuItem_Click);
            this.landableToolStripMenuItem.Click += new System.EventHandler(this.landableToolStripMenuItem_Click);
            this.landableWithAtmosphereToolStripMenuItem.Click += new System.EventHandler(this.landableWithAtmosphereToolStripMenuItem_Click);
            this.landableAndLargeToolStripMenuItem.Click += new System.EventHandler(this.landableAndLargeToolStripMenuItem_Click);
            this.landableWithVolcanismToolStripMenuItem.Click += new System.EventHandler(this.landableWithVolcanismToolStripMenuItem_Click);

            // main menu
            this.showAllPlanetsToolStripMenuItem.Click += new System.EventHandler(this.showAllPlanetsToolStripMenuItem_Click);
            this.showAllStarsToolStripMenuItem.Click += new System.EventHandler(this.showAllStarsToolStripMenuItem_Click);
            this.showBeltClustersToolStripMenuItem.Click += new System.EventHandler(this.showBeltClustersToolStripMenuItem_Click);
            this.showGravityToolStripMenuItem.Click += new System.EventHandler(this.showGravityToolStripMenuItem_Click);
            this.showAtmosToolStripMenuItem.Click += new System.EventHandler(this.showAtmosToolStripMenuItem_Click);
            this.showVolcanismToolStripMenuItem.Click += new System.EventHandler(this.showVolcanismToolStripMenuItem_Click);
            this.showValuesToolStripMenuItem.Click += new System.EventHandler(this.showValuesToolStripMenuItem_Click);
            this.showMoreInformationToolStripMenuItem.Click += new System.EventHandler(this.showMoreInformationToolStripMenuItem_Click);
            this.selectFSSSignalsShownToolStripMenuItem.Click += new System.EventHandler(this.selectFSSSignalsShownToolStripMenuItem_Click);

            // bottom part
            this.hideAlreadyMappedBodiesToolStripMenuItem.Click += new System.EventHandler(this.hideAlreadyMappedBodiesToolStripMenuItem_Click);
            this.checkEDSMForInformationToolStripMenuItem.Click += new System.EventHandler(this.checkEDSMForInformationToolStripMenuItem_Click);
            this.autoHideToolStripMenuItem.Click += new System.EventHandler(this.autoHideToolStripMenuItem_Click);
            this.dontHideInFSSModeToolStripMenuItem.Click += new System.EventHandler(this.dontHideInFSSModeToolStripMenuItem_Click);
            this.showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem.Click += new System.EventHandler(this.showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem_Click);

            // word wrap align
            this.wordWrapToolStripMenuItem.Click += new System.EventHandler(this.wordWrapToolStripMenuItem_Click);
            this.leftToolStripMenuItem.Click += new System.EventHandler(this.leftToolStripMenuItem_Click);
            this.centerToolStripMenuItem.Click += new System.EventHandler(this.centerToolStripMenuItem_Click);
            this.rightToolStripMenuItem.Click += new System.EventHandler(this.rightToolStripMenuItem_Click);

            discoveryform.OnNewUIEvent += Discoveryform_OnNewUIEvent;
            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;
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

        private void Discoveryform_OnHistoryChange(HistoryList hl)
        {
            last_sys = hl.GetLast?.System;      // may be null
            DrawSystem(last_sys, last_sys?.Name);    // may be null
        }

        public override void Closing()
        {
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            discoveryform.OnNewUIEvent -= Discoveryform_OnNewUIEvent;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
        }

        public override bool SupportTransparency { get { return true; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            extPictureBoxScroll.ScrollBarEnabled = !on;     // turn off the scroll bar if its transparent
            extPictureBoxScroll.BackColor =  pictureBoxSurveyor.BackColor = this.BackColor = curcol;
            DrawSystem(last_sys);   // need to redraw as we use backcolour
        }

        private void Uctg_OnTravelSelectionChanged(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            if (he != null)
            {
                if (last_sys == null || last_sys.Name != he.System.Name) // if new entry is scan, may be new data.. or not presenting or diff sys
                {
                    last_sys = he.System;
                    DrawSystem(last_sys, last_sys.Name);
                }
                else if (he.EntryType == JournalTypeEnum.StartJump)         // so we can pre-present
                {
                    JournalStartJump jsj = he.journalEntry as JournalStartJump;
                    last_sys = new SystemClass(jsj.SystemAddress, jsj.StarSystem);       // important need system address as scan uses it for quick lookup
                    DrawSystem(last_sys, last_sys.Name);
                }
                else if (he.EntryType == JournalTypeEnum.FSSAllBodiesFound)     // since we present body counts
                {
                    DrawSystem(last_sys, last_sys.Name + " " + "System scan complete.".T(EDTx.UserControlSurveyor_Systemscancomplete));
                }
                else if (he.EntryType == JournalTypeEnum.FSSDiscoveryScan)      // since we present body counts
                {
                    var je = he.journalEntry as JournalFSSDiscoveryScan;
                    var bodies_found = je.BodyCount;
                    DrawSystem( last_sys, last_sys.Name + " " + bodies_found + " bodies found.".T(EDTx.UserControlSurveyor_bodiesfound));
                }
                else if (he.journalEntry is IStarScan )      // an entry to a scan node
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
            if ( tt != null )
            {
                titletext = tt;
                SetControlText(tt);
            }

            pictureBoxSurveyor.ClearImageList();

            // if system, and we are in no focus or don't care
            if (sys != null && ( uistate == EliteDangerousCore.UIEvents.UIGUIFocus.Focus.NoFocus || !autoHideToolStripMenuItem.Checked 
                                || ( uistate == EliteDangerousCore.UIEvents.UIGUIFocus.Focus.FSSMode && dontHideInFSSModeToolStripMenuItem.Checked) ) )
            {
                int vpos = 0;
                StringFormat frmt = new StringFormat(wordWrapToolStripMenuItem.Checked ? 0: StringFormatFlags.NoWrap);
                frmt.Alignment = alignment;
                var textcolour = IsTransparent ? discoveryform.theme.SPanelColor : discoveryform.theme.LabelColor;
                var backcolour = IsTransparent ? Color.Transparent : this.BackColor;

                if (!IsControlTextVisible() && showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem.Checked)
                {
                    var i = pictureBoxSurveyor.AddTextAutoSize(
                            new Point(3, vpos),
                            new Size(Math.Max(pictureBoxSurveyor.Width - 6, 24), 10000),
                            titletext,
                            Font,
                            textcolour,
                            backcolour,
                            1.0F,
                            frmt: frmt);

                    vpos += i.Location.Height;
                }

                StarScan.SystemNode systemnode = await discoveryform.history.StarScan.FindSystemAsync(sys, checkEDSMForInformationToolStripMenuItem.Checked);        // get data with EDSM

                if (systemnode != null)     // no data, clear display, clear any last_he so samesys is false next time
                {
                    string infoline = "";

                    int scanned = systemnode.StarPlanetsScanned();

                    if (scanned > 0)
                    {
                        infoline = "Scan".T(EDTx.UserControlSurveyor_Scan) + " " + scanned.ToString() + (systemnode.FSSTotalBodies != null ? (" / " + systemnode.FSSTotalBodies.Value.ToString()) : "");
                    }

                    long value = systemnode.ScanValue(false);

                    if ( value > 0  && showValuesToolStripMenuItem.Checked )
                    {
                        infoline = infoline.AppendPrePad("~ " + value.ToString("N0") + " cr", "; ");
                    }

                    if (infoline.HasChars())
                    {
                        //System.Diagnostics.Debug.WriteLine("Draw " + infoline);
                        var i = pictureBoxSurveyor.AddTextAutoSize(
                            new Point(3, vpos),
                            new Size(Math.Max(pictureBoxSurveyor.Width - 6, 24), 10000),
                            infoline,
                            Font,
                            textcolour,
                            backcolour,
                            1.0F,
                            frmt: frmt);
                        vpos += i.Location.Height;
                    }

                    var all_nodes = systemnode.Bodies.ToList();

                    if (all_nodes != null)
                    {
                        value = 0;
                        
                        foreach (StarScan.ScanNode sn in all_nodes)
                        {
                            if (sn.ScanData != null && sn.ScanData?.BodyName != null && (!sn.ScanData.IsEDSMBody || checkEDSMForInformationToolStripMenuItem.Checked))
                                
                            {
                                var sd = sn.ScanData;
                                
                                bool GeoSignal = false;
                                bool BioSignal = false;
                                if (sn.Signals != null)
                                {
                                    foreach (var y in sn.Signals)
                                    {
                                        if (y.Type.Contains("$SAA_SignalType_Geological;"))
                                        {
                                            GeoSignal = true;
                                        }
                                        else if (y.Type.Contains("$SAA_SignalType_Biological;"))
                                        {
                                            BioSignal = true;
                                        }
                                        else { }
                                    }
                                }

                                if  (                                   
                                    (sd.IsLandable && landableToolStripMenuItem.Checked) ||
                                    (sd.IsLandable && sd.HasAtmosphericComposition && landableWithAtmosphereToolStripMenuItem.Checked) ||
                                    (sd.IsLandable && sd.HasMeaningfulVolcanism && landableWithVolcanismToolStripMenuItem.Checked) ||
                                    (sd.IsLandable && sd.nRadius.HasValue && sd.nRadius >= largeRadiusLimit && landableAndLargeToolStripMenuItem.Checked) ||
                                    (sd.AmmoniaWorld && ammoniaWorldToolStripMenuItem.Checked) ||
                                    (sd.Earthlike && earthlikeWorldToolStripMenuItem.Checked) ||
                                    (sd.WaterWorld && waterWorldToolStripMenuItem.Checked) ||
                                    (sd.PlanetTypeID == EDPlanet.High_metal_content_body && highMetalContentBodyToolStripMenuItem.Checked) ||
                                    (sd.PlanetTypeID == EDPlanet.Metal_rich_body && metalToolStripMenuItem.Checked) ||
                                    (sd.HasRings && hasRingsToolStripMenuItem.Checked) ||
                                    (sd.HasMeaningfulVolcanism && hasVolcanismToolStripMenuItem.Checked) ||
                                    (sd.nEccentricity.HasValue && sd.nEccentricity >= eccentricityLimit && highEccentricityToolStripMenuItem.Checked) ||
                                    (sd.CanBeTerraformable && terraformableToolStripMenuItem.Checked) ||
                                    (sd.IsPlanet && lowRadiusToolStripMenuItem.Checked && sd.nRadius.HasValue && sd.nRadius < lowRadiusLimit) ||
                                    (sn.Signals != null && hasSignalsToolStripMenuItem.Checked) ||
                                    (GeoSignal && hasGeologicalSignalsToolStripMenuItem.Checked) ||
                                    (BioSignal && hasBiologicalSignalsToolStripMenuItem.Checked) ||
                                    (sd.IsStar && showAllStarsToolStripMenuItem.Checked) ||
                                    (sd.IsPlanet && showAllPlanetsToolStripMenuItem.Checked) ||
                                    (sd.IsBeltCluster && showBeltClustersToolStripMenuItem.Checked))
                                {
                                    if (!sd.Mapped || hideAlreadyMappedBodiesToolStripMenuItem.Checked == false)      // if not mapped, or show mapped
                                    {
                                        var il = sd.SurveyorInfoLine(sys,
                                                                        sn.Signals != null && hasSignalsToolStripMenuItem.Checked && !GeoSignal && !BioSignal,  // show signals if we have some and filter for all signals is checked
                                                                        GeoSignal && (hasSignalsToolStripMenuItem.Checked || hasGeologicalSignalsToolStripMenuItem.Checked || hasBiologicalSignalsToolStripMenuItem.Checked), // show biological signals if there are any and any signal filter is checked (as there are bios that need geos to appear)
                                                                        BioSignal && (hasSignalsToolStripMenuItem.Checked || hasBiologicalSignalsToolStripMenuItem.Checked), // show biological signals if there are any and the all signal filter or the bio signal filter is checked
                                                                        hasVolcanismToolStripMenuItem.Checked || (sd.IsLandable && landableWithVolcanismToolStripMenuItem.Checked)
                                                                            || (sd.IsLandable && showVolcanismToolStripMenuItem.Checked), // any of these makes us need to show volcanic state
                                                                        showValuesToolStripMenuItem.Checked,        // show values
                                                                        showMoreInformationToolStripMenuItem.Checked,   // show extra info such as mass/radius
                                                                        showGravityToolStripMenuItem.Checked,       // show gravity select
                                                                        sd.IsLandable && showAtmosToolStripMenuItem.Checked, // show atmosphere if landable (surveyor shows this if landable)
                                                                        hasRingsToolStripMenuItem.Checked,          // show rings
                                                                        lowRadiusLimit, largeRadiusLimit, eccentricityLimit);

                                        //System.Diagnostics.Debug.WriteLine("Display " + il);
                                        var i = pictureBoxSurveyor.AddTextAutoSize(
                                                new Point(3, vpos),
                                                new Size(Math.Max(pictureBoxSurveyor.Width - 6, 24), 10000),
                                                il,
                                                Font,
                                                textcolour,
                                                backcolour,
                                                1.0F,
                                                frmt: frmt);
                                        vpos += i.Location.Height;
                                        value += sd.EstimatedValue;
                                    }
                                }
                            }
                        }

                        if (value > 0 && showValuesToolStripMenuItem.Checked )
                        {
                            var i = pictureBoxSurveyor.AddTextAutoSize(
                                new Point(3, vpos),
                                new Size(Math.Max(pictureBoxSurveyor.Width - 6, 24), 10000),
                                "^^ ~ " + value.ToString("N0") + " cr",
                                Font,
                                textcolour,
                                backcolour,
                                1.0F,
                                frmt: frmt);
                            vpos += i.Location.Height;
                        }
                    }

                    if ( fsssignalsdisplayed.HasChars())
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
                                if ( pos++ == expiredpos )
                                    siglist = siglist.AppendPrePad("Expired:".T(EDTx.UserControlScan_Expired), Environment.NewLine + Environment.NewLine);

                                siglist = siglist.AppendPrePad(fsssig.ToString(true), Environment.NewLine);
                            }
                        }

                        if (siglist.HasChars())
                        {
                            //System.Diagnostics.Debug.WriteLine("Display " + siglist);
                            pictureBoxSurveyor.AddTextAutoSize(new Point(3, vpos),
                                                            new Size(Math.Max(pictureBoxSurveyor.Width - 6, 24), 10000),
                                                            siglist,
                                                            Font,
                                                            textcolour,
                                                            backcolour,
                                                            1.0F,
                                                            frmt: frmt);
                        }

                    }
                }
            }

            extPictureBoxScroll.Render();
        }

 
        #endregion

        #region UI

        private void ammoniaWorldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("showAmmonia", ammoniaWorldToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void earthlikeWorldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("showEarthlike", earthlikeWorldToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void waterWorldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("showWaterWorld", waterWorldToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void highMetalContentBodyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("showHMC", highMetalContentBodyToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void metalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("showMR", metalToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void terraformableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("showTerraformable", terraformableToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void highEccentricityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("showEccentricity", terraformableToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void landableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("isLandable", landableToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }
        private void landableWithAtmosphereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("isLandableWithAtmosphere", landableToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }
        private void landableWithVolcanismToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("isLandableWithVolcanism", landableToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void landableAndLargeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("largelandable", landableToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }
        private void hasVolcanismToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("showVolcanism", hasVolcanismToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void hasRingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("showRinged", hasRingsToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void hideAlreadyMappedBodiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("hideMapped", hideAlreadyMappedBodiesToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void autoHideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("autohide", autoHideToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void showVolcanismToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("volcanism", showVolcanismToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void lowRadiusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("lowradius", lowRadiusToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void showGravityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("showGravity", showGravityToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void checkEDSMForInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("edsm", checkEDSMForInformationToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("showsysinfo", showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void dontHideInFSSModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("donthidefssmode", dontHideInFSSModeToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void hasSignalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("signals", hasSignalsToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void hasBiologicalSignalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("BioSignals", hasSignalsToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }
        private void hasGeologicalSignalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("GeoSignals", hasSignalsToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }
        private void showValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("showValues", showValuesToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void showAllPlanetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("allplanets", showAllPlanetsToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void showAllStarsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("allstars", showAllStarsToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void showBeltClustersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("beltclusters", showBeltClustersToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void showMoreInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("moreinfo", showMoreInformationToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("wordwrap", wordWrapToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void showAtmosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("atmos", showAtmosToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }


        private void leftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetAlign(StringAlignment.Near);
            DrawSystem(last_sys);
        }

        private void SetAlign(StringAlignment al)
        {
            alignment = al;
            PutSetting("align", (int)alignment);
            leftToolStripMenuItem.Checked = alignment == StringAlignment.Near;
            centerToolStripMenuItem.Checked = alignment == StringAlignment.Center;
            rightToolStripMenuItem.Checked = alignment == StringAlignment.Far;
        }

        private void centerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetAlign(StringAlignment.Center);
            DrawSystem(last_sys);
        }

        private void rightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetAlign(StringAlignment.Far);
            DrawSystem(last_sys);
        }

        private void UserControlSurveyor_Resize(object sender, EventArgs e)
        {
            DrawSystem(last_sys);
        }

        private void selectFSSSignalsShownToolStripMenuItem_Click(object sender, EventArgs e)
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

        
    }
}
