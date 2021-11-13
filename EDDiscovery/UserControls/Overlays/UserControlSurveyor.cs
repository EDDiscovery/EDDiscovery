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
using ExtendedControls;
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

            checkBoxEDSM.Checked = GetSetting("edsm", false);
            checkBoxEDSM.Click += new System.EventHandler(this.checkBoxEDSM_Clicked);

            PopulateCtrlList();

            extCheckBoxWordWrap.Checked = GetSetting("wordwrap", false);
            extCheckBoxWordWrap.Click += wordWrapToolStripMenuItem_Click;

            fsssignalsdisplayed = GetSetting("fsssignals", "");

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
            rollUpPanelTop.Visible = !on;
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
            if (sys != null && ( uistate == EliteDangerousCore.UIEvents.UIGUIFocus.Focus.NoFocus || !IsSet(CtrlList.autohide)
                                || ( uistate == EliteDangerousCore.UIEvents.UIGUIFocus.Focus.FSSMode && IsSet(CtrlList.donthidefssmode)) ) )
            {
                int vpos = 0;
                StringFormat frmt = new StringFormat(extCheckBoxWordWrap.Checked ? 0: StringFormatFlags.NoWrap);
                frmt.Alignment = alignment;
                var textcolour = IsTransparent ? discoveryform.theme.SPanelColor : discoveryform.theme.LabelColor;
                var backcolour = IsTransparent ? Color.Transparent : this.BackColor;

                if (!IsControlTextVisible() && IsSet(CtrlList.showsysinfo))
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

                    if ( value > 0 && IsSet(CtrlList.showValues))
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
                                    (hasbiosignals && geosignalschecked) ||
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

                        if (value > 0 && IsSet(CtrlList.showValues))
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

        private enum CtrlList
        {
            allplanets, showAmmonia, showEarthlike, showWaterWorld, showHMC, showMR,
            showTerraformable, showVolcanism, showRinged, showEccentricity, lowradius,
            signals, GeoSignals, BioSignals, isLandable, isLandableWithAtmosphere,
            largelandable, isLandableWithVolcanism,
            // 18
            allstars, beltclusters,
            // 20
            showValues, moreinfo, showGravity, atmos, volcanism, autohide, donthidefssmode,hideMapped,showsysinfo,
            // 29
            alignleft,aligncenter,alignright
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
                bool def = (e != CtrlList.alignright && e != CtrlList.aligncenter && e != CtrlList.autohide && e != CtrlList.lowradius
                    && e != CtrlList.allplanets && e != CtrlList.allstars && e != CtrlList.beltclusters);
                var v = GetSetting(e.ToString(), def);
                //System.Diagnostics.Debug.WriteLine($"Surveyor {e.ToString()} = {v}");
                ctrlset[(int)e] = v; 
            }

            alignment = ctrlset[(int)CtrlList.alignright] ? StringAlignment.Far : ctrlset[(int)CtrlList.aligncenter] ? StringAlignment.Center : StringAlignment.Near;
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
            displayfilter.AddStandardOption(CtrlList.allplanets.ToString(), "Show All Planets".TxID("UserControlSurveyor.showAllPlanetsToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.showAmmonia.ToString(), "Ammonia World".TxID("UserControlSurveyor.planetaryClassesToolStripMenuItem.ammoniaWorldToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.showEarthlike.ToString(), "Earthlike World".TxID("UserControlSurveyor.planetaryClassesToolStripMenuItem.earthlikeWorldToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.showWaterWorld.ToString(), "Water World".TxID("UserControlSurveyor.planetaryClassesToolStripMenuItem.waterWorldToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.showHMC.ToString(), "High metal content body".TxID("UserControlSurveyor.planetaryClassesToolStripMenuItem.highMetalContentBodyToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.showMR.ToString(), "Metal-rich body".TxID("UserControlSurveyor.planetaryClassesToolStripMenuItem.metalToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.showTerraformable.ToString(), "Terraformable".TxID("UserControlSurveyor.bodyFeaturesToolStripMenuItem.terraformableToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.showVolcanism.ToString(), "Has volcanism".TxID("UserControlSurveyor.bodyFeaturesToolStripMenuItem.hasVolcanismToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.showRinged.ToString(), "Has Rings".TxID("UserControlSurveyor.bodyFeaturesToolStripMenuItem.hasRingsToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.showEccentricity.ToString(), "High eccentricity".TxID("UserControlSurveyor.bodyFeaturesToolStripMenuItem.highEccentricityToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.lowradius.ToString(), "Tiny body".TxID("UserControlSurveyor.bodyFeaturesToolStripMenuItem.lowRadiusToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.signals.ToString(), "Has Signals".TxID("UserControlSurveyor.bodyFeaturesToolStripMenuItem.hasSignalsToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.GeoSignals.ToString(), "Has geological signals".TxID("UserControlSurveyor.bodyFeaturesToolStripMenuItem.hasGeologicalSignalsToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.BioSignals.ToString(), "Has biological signals".TxID("UserControlSurveyor.bodyFeaturesToolStripMenuItem.hasBiologicalSignalsToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.isLandable.ToString(), "Landable".TxID("UserControlSurveyor.bodyFeaturesToolStripMenuItem.landableToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.isLandableWithAtmosphere.ToString(), "Landable with atmosphere".TxID("UserControlSurveyor.bodyFeaturesToolStripMenuItem.landableWithAtmosphereToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.largelandable.ToString(), "Landable and large".TxID("UserControlSurveyor.bodyFeaturesToolStripMenuItem.landableAndLargeToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.isLandableWithVolcanism.ToString(), "Landable with volcanism".TxID("UserControlSurveyor.bodyFeaturesToolStripMenuItem.landableWithVolcanismToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);

            string[] planetctrllist = Enum.GetNames(typeof(CtrlList)).RangeSubset((int)CtrlList.allplanets, 18);
            CommonCtrl(displayfilter, planetctrllist,extButtonPlanets);

        }

        private void extButtonStars_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();

            displayfilter.AddAllNone();
            displayfilter.AddStandardOption(CtrlList.allstars.ToString(), "Show All Stars".TxID("UserControlSurveyor.showAllStarsToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.beltclusters.ToString(), "Show Belt Clusters".TxID("UserControlSurveyor.showBeltClustersToolStripMenuItem"), global::EDDiscovery.Icons.Controls.ScanGrid_Belt);

            string[] starctrllist = Enum.GetNames(typeof(CtrlList)).RangeSubset((int)CtrlList.allstars, 2);
            CommonCtrl(displayfilter, starctrllist, extButtonStars);
        }

        private void extButtonShowControl_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();

            displayfilter.AddAllNone();
            displayfilter.AddStandardOption(CtrlList.showValues.ToString(), "Show values".TxID("UserControlSurveyor.showValuesToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.moreinfo.ToString(), "Show more information".TxID("UserControlSurveyor.showMoreInformationToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.showGravity.ToString(), "Show gravity of landables".TxID("UserControlSurveyor.showGravityToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.atmos.ToString(), "Show atmosphere of landables".TxID("UserControlSurveyor.showAtmosToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.volcanism.ToString(), "Show volcanism of landables".TxID("UserControlSurveyor.showVolcanismToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.autohide.ToString(), "Auto Hide".TxID("UserControlSurveyor.autoHideToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.donthidefssmode.ToString(), "Don't hide in FSS Mode".TxID("UserControlSurveyor.dontHideInFSSModeToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.hideMapped.ToString(), "Hide already mapped bodies".TxID("UserControlSurveyor.hideAlreadyMappedBodiesToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.AddStandardOption(CtrlList.showsysinfo.ToString(), "Show System Info Always".TxID("UserControlSurveyor.showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem"), global::EDDiscovery.Icons.Controls.Scan_Star);

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
            CommonCtrl(displayfilter,showctrllist, extButtonAlignment);
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
    }
}
