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
using EliteDangerousCore.DB;
using EliteDangerousCore.JournalEvents;
using BaseUtils;
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

        private string DbSave => DBName("Surveyor");

        System.Drawing.StringAlignment alignment = System.Drawing.StringAlignment.Near;
        string titletext = "";

        const int lowRadiusLimit = 600 * 1000;

        EliteDangerousCore.UIEvents.UIGUIFocus.Focus uistate = EliteDangerousCore.UIEvents.UIGUIFocus.Focus.NoFocus;

        public UserControlSurveyor()
        {
            InitializeComponent();
        }

        #region Overrides

        public override void Init()
        {
            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);
            BaseUtils.Translator.Instance.Translate(contextMenuStrip, this);

            // set context menu checkboxes
            showValuesToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "showValues", true);
            ammoniaWorldToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "showAmmonia", true);
            earthlikeWorldToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "showEarthlike", true);
            waterWorldToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "showWaterWorld", true);
            terraformableToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "showTerraformable", true);
            hasVolcanismToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "showVolcanism", true);
            hasRingsToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "showRinged", true);
            hideAlreadyMappedBodiesToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "hideMapped", true);
            autoHideToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "autohide", false);
            lowRadiusToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "lowradius", false);
            checkEDSMForInformationToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "edsm", false);
            showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "showsysinfo", true);
            dontHideInFSSModeToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "donthidefssmode", true);
            hasSignalsToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "signals", true);
            showAllPlanetsToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "allplanets", false);
            showAllStarsToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "allstars", false);
            showBeltClustersToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "beltclusters", false);
            showMoreInformationToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "moreinfo", true);
            wordWrapToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "wordwrap", false);

            SetAlign((StringAlignment)EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(DbSave + "align", 0));

            // install the handlers AFTER setup otherwise you get lots of events
            this.ammoniaWorldToolStripMenuItem.Click += new System.EventHandler(this.ammoniaWorldToolStripMenuItem_Click);
            this.earthlikeWorldToolStripMenuItem.Click += new System.EventHandler(this.earthlikeWorldToolStripMenuItem_Click);
            this.waterWorldToolStripMenuItem.Click += new System.EventHandler(this.waterWorldToolStripMenuItem_Click);
            this.terraformableToolStripMenuItem.Click += new System.EventHandler(this.terraformableToolStripMenuItem_Click);
            this.hasVolcanismToolStripMenuItem.Click += new System.EventHandler(this.hasVolcanismToolStripMenuItem_Click);
            this.hasRingsToolStripMenuItem.Click += new System.EventHandler(this.hasRingsToolStripMenuItem_Click);
            this.lowRadiusToolStripMenuItem.Click += new System.EventHandler(this.lowRadiusToolStripMenuItem_Click);
            this.hideAlreadyMappedBodiesToolStripMenuItem.Click += new System.EventHandler(this.hideAlreadyMappedBodiesToolStripMenuItem_Click);
            this.leftToolStripMenuItem.Click += new System.EventHandler(this.leftToolStripMenuItem_Click);
            this.centerToolStripMenuItem.Click += new System.EventHandler(this.centerToolStripMenuItem_Click);
            this.rightToolStripMenuItem.Click += new System.EventHandler(this.rightToolStripMenuItem_Click);
            this.autoHideToolStripMenuItem.Click += new System.EventHandler(this.autoHideToolStripMenuItem_Click);
            this.checkEDSMForInformationToolStripMenuItem.Click += new System.EventHandler(this.checkEDSMForInformationToolStripMenuItem_Click);
            this.showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem.Click += new System.EventHandler(this.showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem_Click);
            this.dontHideInFSSModeToolStripMenuItem.Click += new System.EventHandler(this.dontHideInFSSModeToolStripMenuItem_Click);
            this.showValuesToolStripMenuItem.Click += new System.EventHandler(this.showValuesToolStripMenuItem_Click);
            this.showAllPlanetsToolStripMenuItem.Click += new System.EventHandler(this.showAllPlanetsToolStripMenuItem_Click);
            this.showAllStarsToolStripMenuItem.Click += new System.EventHandler(this.showAllStarsToolStripMenuItem_Click);
            this.showBeltClustersToolStripMenuItem.Click += new System.EventHandler(this.showBeltClustersToolStripMenuItem_Click);
            this.showMoreInformationToolStripMenuItem.Click += new System.EventHandler(this.showMoreInformationToolStripMenuItem_Click);
            this.wordWrapToolStripMenuItem.Click += new System.EventHandler(this.wordWrapToolStripMenuItem_Click);

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

        public override Color ColorTransparency => Color.Green;
        public override void SetTransparency(bool on, Color curcol)
        {
            System.Diagnostics.Debug.WriteLine("Set colour to " + curcol);
            pictureBoxSurveyor.BackColor = this.BackColor = curcol;
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
                else if (he.EntryType == JournalTypeEnum.StartJump)  // we ignore start jump if overriden      
                {
                    JournalStartJump jsj = he.journalEntry as JournalStartJump;
                    last_sys = new SystemClass(jsj.StarSystem);
                    DrawSystem(last_sys, last_sys.Name);
                }
                else if (he.EntryType == JournalTypeEnum.FSSAllBodiesFound)
                {
                    DrawSystem(last_sys, last_sys.Name + " " + "System scan complete.".T(EDTx.UserControlSurveyor_Systemscancomplete));
                }
                else if (he.EntryType == JournalTypeEnum.FSSDiscoveryScan)
                {
                    var je = he.journalEntry as JournalFSSDiscoveryScan;
                    var bodies_found = je.BodyCount;
                    DrawSystem( last_sys, last_sys.Name + " " + bodies_found + " bodies found.".T(EDTx.UserControlSurveyor_bodiesfound));
                }
                else if (he.EntryType == JournalTypeEnum.Scan)
                {
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

                System.Diagnostics.Debug.WriteLine("Surveyor UI event " + uistate);
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
                            new Size(Math.Max(pictureBoxSurveyor.Width - 6, 24), 1000),
                            titletext,
                            Font,
                            textcolour,
                            backcolour,
                            1.0F,
                            frmt: frmt);

                    vpos += i.Location.Height;
                }

                StarScan.SystemNode systemnode = await discoveryform.history.starscan.FindSystemAsync(sys, checkEDSMForInformationToolStripMenuItem.Checked);        // get data with EDSM

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
                        var  i = pictureBoxSurveyor.AddTextAutoSize(
                            new Point(3, vpos),
                            new Size(Math.Max(pictureBoxSurveyor.Width - 6, 24), 1000),
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
                            if (sn.ScanData != null && sn.ScanData?.BodyName != null )
                            {
                                var sd = sn.ScanData;

                                if (    (sd.AmmoniaWorld && ammoniaWorldToolStripMenuItem.Checked) ||
                                        (sd.Earthlike && earthlikeWorldToolStripMenuItem.Checked) ||
                                        (sd.WaterWorld && waterWorldToolStripMenuItem.Checked) ||
                                        (sd.HasRings && !sd.AmmoniaWorld && !sd.Earthlike && !sd.WaterWorld && hasRingsToolStripMenuItem.Checked) ||
                                        (sd.HasMeaningfulVolcanism && hasVolcanismToolStripMenuItem.Checked) ||
                                        (sd.Terraformable && terraformableToolStripMenuItem.Checked) ||
                                        (lowRadiusToolStripMenuItem.Checked && sd.nRadius.HasValue && sd.nRadius < lowRadiusLimit) ||
                                        (sn.Signals != null && hasSignalsToolStripMenuItem.Checked ) ||
                                        (sd.IsStar && showAllStarsToolStripMenuItem.Checked) ||
                                        (sd.IsPlanet && showAllPlanetsToolStripMenuItem.Checked) ||
                                        (sd.IsBeltCluster && showBeltClustersToolStripMenuItem.Checked)
                                    )
                                {
                                    if (!sd.Mapped || hideAlreadyMappedBodiesToolStripMenuItem.Checked == false)      // if not mapped, or show mapped
                                    {
                                        var i = pictureBoxSurveyor.AddTextAutoSize(
                                                new Point(3, vpos),
                                                new Size(Math.Max(pictureBoxSurveyor.Width - 6, 24), 1000),
                                                InfoLine(last_sys, sn, sd),
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
                                new Size(Math.Max(pictureBoxSurveyor.Width - 6, 24), 1000),
                                "^^ ~ " + value.ToString("N0") + " cr",
                                Font,
                                textcolour,
                                backcolour,
                                1.0F,
                                frmt: frmt);
                            vpos += i.Location.Height;
                        }

                    }
                }
            }

            pictureBoxSurveyor.Render();
        }

        private string InfoLine(ISystem sys, StarScan.ScanNode sn, JournalScan js)
        {
            var information = new StringBuilder();

            if (js.Mapped)
                information.Append("\u2713"); // let the cmdr see that this body is already mapped - this is a check

            string bodyname = js.BodyName.ReplaceIfStartsWith(sys.Name);

            // Name
            information.Append(bodyname);

            // Additional information
            information.Append((js.AmmoniaWorld) ? @" is an ammonia world.".T(EDTx.UserControlSurveyor_isanammoniaworld) : null);
            information.Append((js.Earthlike) ? @" is an earth like world.".T(EDTx.UserControlSurveyor_isanearthlikeworld) : null);
            information.Append((js.WaterWorld && !js.Terraformable) ? @" is a water world.".T(EDTx.UserControlSurveyor_isawaterworld) : null);
            information.Append((js.WaterWorld && js.Terraformable) ? @" is a terraformable water world.".T(EDTx.UserControlSurveyor_isaterraformablewaterworld) : null);
            information.Append((js.Terraformable && !js.WaterWorld) ? @" is a terraformable planet.".T(EDTx.UserControlSurveyor_isaterraformableplanet) : null);
            information.Append((js.HasRings) ? @" Has ring.".T(EDTx.UserControlSurveyor_Hasring) : null);
            information.Append((js.HasMeaningfulVolcanism) ? @" Has ".T(EDTx.UserControlSurveyor_Has) + js.Volcanism + "." : null);
            information.Append((js.nRadius < lowRadiusLimit) ? @" Low Radius.".T(EDTx.UserControlSurveyor_LowRadius) : null);
            information.Append((sn.Signals != null) ? " Has Signals.".T(EDTx.UserControlSurveyor_Signals) : null);

            if (js.WasMapped == true && js.WasDiscovered == true)
            {
                information.Append(" (Mapped & Discovered)".T(EDTx.UserControlSurveyor_MandD));
                if (showValuesToolStripMenuItem.Checked)
                {
                    information.Append(" " + js.EstimatedValueBase.ToString("N0") + " cr");
                }
            }
            else if (js.WasMapped == true && js.WasDiscovered == false)
            {
                information.Append(" (Mapped)".T(EDTx.UserControlSurveyor_MP));
                if (showValuesToolStripMenuItem.Checked)
                {
                    information.Append(" " + js.EstimatedValueBase.ToString("N0") + " cr");
                }
            }
            else if (js.WasDiscovered == true && js.WasMapped == false)
            {
                information.Append(" (Discovered)".T(EDTx.UserControlSurveyor_DIS));
                if (showValuesToolStripMenuItem.Checked)
                {
                    information.Append(" " + js.EstimatedValueFirstMappedEfficiently.ToString("N0") + " cr");
                }
            }
            else
            {      
                if (showValuesToolStripMenuItem.Checked)
                {
                    information.Append(" " + (js.EstimatedValueFirstDiscoveredFirstMappedEfficiently > 0 ? js.EstimatedValueFirstDiscoveredFirstMappedEfficiently : js.EstimatedValueBase).ToString("N0") + " cr");
                }
            }

            if (showMoreInformationToolStripMenuItem.Checked)
            {
                information.Append(" " + js.ShortInformation());
            }
            else
                information.Append(@" " + js.DistanceFromArrivalText);

            return information.ToString();
        }

        
        #endregion

        private void ammoniaWorldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "showAmmonia", ammoniaWorldToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void earthlikeWorldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "showEarthlike", earthlikeWorldToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void waterWorldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "showWaterWorld", waterWorldToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void terraformableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "showTerraformable", terraformableToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void hasVolcanismToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "showVolcanism", hasVolcanismToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void hasRingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "showRinged", hasRingsToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void hideAlreadyMappedBodiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "hideMapped", hideAlreadyMappedBodiesToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void autoHideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "autohide", autoHideToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void lowRadiusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "lowradius", lowRadiusToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void checkEDSMForInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "edsm", checkEDSMForInformationToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "showsysinfo", showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void dontHideInFSSModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "donthidefssmode", dontHideInFSSModeToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void hasSignalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "signals", hasSignalsToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void showValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "showValues", showValuesToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void showAllPlanetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "allplanets", showAllPlanetsToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void showAllStarsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "allstars", showAllStarsToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void showBeltClustersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "beltclusters", showBeltClustersToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void showMoreInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "moreinfo", showMoreInformationToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "wordwrap", wordWrapToolStripMenuItem.Checked);
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
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(DbSave + "align", (int)alignment);
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
    }
}