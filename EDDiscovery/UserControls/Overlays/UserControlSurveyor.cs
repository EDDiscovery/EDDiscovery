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
            ammoniaWorldToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showAmmonia", true);
            earthlikeWorldToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showEarthlike", true);
            waterWorldToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showWaterWorld", true);
            terraformableToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showTerraformable", true);
            hasVolcanismToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showVolcanism", true);
            hasRingsToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showRinged", true);
            hideAlreadyMappedBodiesToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "hideMapped", true);
            autoHideToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "autohide", false);
            lowRadiusToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "lowradius", false);
            checkEDSMForInformationToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "edsm", false);
            showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showsysinfo", true);
            dontHideInFSSModeToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "donthidefssmode", true);
            SetAlign((StringAlignment)SQLiteDBClass.GetSettingInt(DbSave + "align", 0));

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
            this.dontHideInFSSModeToolStripMenuItem.Click += new System.EventHandler(this.dontHideInFSSModeToolStripMenutItem_Click);

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
                    DrawSystem(last_sys, last_sys.Name + " " + "System scan complete.".Tx(this));
                }
                else if (he.EntryType == JournalTypeEnum.FSSDiscoveryScan)
                {
                    var je = he.journalEntry as JournalFSSDiscoveryScan;
                    var bodies_found = je.BodyCount;
                    DrawSystem( last_sys, last_sys.Name + " " + bodies_found + " bodies found.".Tx(this));
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

        private void DrawSystem(ISystem sys, string tt = null)
        {
            if ( tt != null )
            {
                titletext = tt;
                SetControlText(tt);
            }

            pictureBoxSurveyor.ClearImageList();

            // if system, and we are in no focus or don't care
            if (sys != null && ( uistate == EliteDangerousCore.UIEvents.UIGUIFocus.Focus.NoFocus || !autoHideToolStripMenuItem.Checked )
                 || ( uistate == EliteDangerousCore.UIEvents.UIGUIFocus.Focus.FSSMode && dontHideInFSSModeToolStripMenuItem.Checked) )
            {
                int vpos = 0;
                StringFormat frmt = new StringFormat(StringFormatFlags.NoWrap);
                frmt.Alignment = alignment;
                var textcolour = IsTransparent ? discoveryform.theme.SPanelColor : discoveryform.theme.LabelColor;
                var backcolour = IsTransparent ? Color.Transparent : this.BackColor;

                if (!IsControlTextVisible() && showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem.Checked)
                {
                    pictureBoxSurveyor.AddTextFixedSizeC(
                            new Point(3, vpos),
                            new Size(Math.Max(pictureBoxSurveyor.Width - 6, 24), Font.Height),
                            titletext,
                            Font,
                            textcolour,
                            backcolour,
                            1.0F,
                            false,
                            frmt: frmt);

                    vpos += (int)Font.Height;
                }

                StarScan.SystemNode scannode = discoveryform.history.starscan.FindSystem(sys, checkEDSMForInformationToolStripMenuItem.Checked);        // get data with EDSM

                if (scannode != null)     // no data, clear display, clear any last_he so samesys is false next time
                {
                    var all_nodes = scannode.Bodies.ToList();

                    if (all_nodes != null)
                    {
                        foreach (StarScan.ScanNode sn in all_nodes)
                        {
                            if (sn.ScanData != null && sn.ScanData?.BodyName != null && !sn.ScanData.IsStar)
                            {
                                var sd = sn.ScanData;

                                if ((sd.AmmoniaWorld && ammoniaWorldToolStripMenuItem.Checked) ||
                                                        (sd.Earthlike && earthlikeWorldToolStripMenuItem.Checked) ||
                                                        (sd.WaterWorld && waterWorldToolStripMenuItem.Checked) ||
                                                        (sd.HasRings && !sd.AmmoniaWorld && !sd.Earthlike && !sd.WaterWorld && hasRingsToolStripMenuItem.Checked) ||
                                                        (sd.HasMeaningfulVolcanism && hasVolcanismToolStripMenuItem.Checked) ||
                                                        (sd.Terraformable && terraformableToolStripMenuItem.Checked) ||
                                                        (lowRadiusToolStripMenuItem.Checked && sd.nRadius < lowRadiusLimit)
                                                        )
                                {
                                    if (!sd.Mapped || hideAlreadyMappedBodiesToolStripMenuItem.Checked == false)      // if not mapped, or show mapped
                                    {
                                        pictureBoxSurveyor.AddTextFixedSizeC(
                                                new Point(3, vpos),
                                                new Size(Math.Max(pictureBoxSurveyor.Width - 6, 24), Font.Height),
                                                InfoLine(last_sys,sd),
                                                Font,
                                                textcolour,
                                                backcolour,
                                                1.0F,
                                                false,
                                                frmt: frmt);

                                        vpos += (int)Font.Height;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            pictureBoxSurveyor.Render();
        }

        private string InfoLine(ISystem sys, JournalScan sd)
        {
            var information = new StringBuilder();

            if (sd.Mapped)
                information.Append("\u2713"); // let the cmdr see that this body is already mapped - this is a check

            string bodyname = sd.BodyName;

            // if [0] starts with [1], and there is more in [0] then [1], remove
            if (bodyname.StartsWith(sys.Name, StringComparison.InvariantCultureIgnoreCase) && bodyname.Length > sys.Name.Length)
                bodyname = bodyname.Substring(sys.Name.Length).Trim();

            // Name
            information.Append(bodyname);

            // Additional information
            information.Append((sd.AmmoniaWorld) ? @" is an ammonia world.".Tx(this) : null);
            information.Append((sd.Earthlike) ? @" is an earth like world.".Tx(this) : null);
            information.Append((sd.WaterWorld && !sd.Terraformable) ? @" is a water world.".Tx(this) : null);
            information.Append((sd.WaterWorld && sd.Terraformable) ? @" is a terraformable water world.".Tx(this) : null);
            information.Append((sd.Terraformable && !sd.WaterWorld) ? @" is a terraformable planet.".Tx(this) : null);
            information.Append((sd.HasRings) ? @" Has ring.".Tx(this) : null);
            information.Append((sd.HasMeaningfulVolcanism) ? @" Has ".Tx(this) + sd.Volcanism : null);
            information.Append((sd.nRadius < lowRadiusLimit) ? @" Low Radius.".Tx(this) : null);
            information.Append(@" " + sd.DistanceFromArrivalText);
            if (sd.WasMapped == true && sd.WasDiscovered == true)
                information.Append(" (Mapped & Discovered)".Tx(this));
            else if (sd.WasMapped == true)
                information.Append(" (Mapped)".Tx(this));
            else if (sd.WasDiscovered == true)
                information.Append(" (Discovered)".Tx(this));

            return information.ToString();
        }

        
        #endregion

        private void ammoniaWorldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "showAmmonia", ammoniaWorldToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void earthlikeWorldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "showEarthlike", earthlikeWorldToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void waterWorldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "showWaterWorld", waterWorldToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void terraformableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "showTerraformable", terraformableToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void hasVolcanismToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "showVolcanism", hasVolcanismToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void hasRingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "showRinged", hasRingsToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void hideAlreadyMappedBodiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "hideMapped", hideAlreadyMappedBodiesToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void autoHideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "autohide", autoHideToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void lowRadiusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "lowradius", lowRadiusToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void checkEDSMForInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "edsm", checkEDSMForInformationToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "showsysinfo", showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem.Checked);
            DrawSystem(last_sys);
        }

        private void dontHideInFSSModeToolStripMenutItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "donthidefssmode", dontHideInFSSModeToolStripMenuItem.Checked);
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
            SQLiteDBClass.PutSettingInt(DbSave + "align", (int)alignment);
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