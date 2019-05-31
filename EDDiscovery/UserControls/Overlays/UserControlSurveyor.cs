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
        HistoryEntry last_he = null;

        private string DbSave => DBName("Surveyor");

        private enum Alignment
        {
            left = 0,
            center = 1,
            right = 2,
        }

        private Alignment align;

        public UserControlSurveyor()
        {
            InitializeComponent();
        }

        #region Overrides

        public override void Init()
        {
            discoveryform.OnHistoryChange += Display;
            discoveryform.OnNewEntry += NewEntry;

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
            align = (Alignment)SQLiteDBClass.GetSettingInt(DbSave + nameof(align), 0);

            switch (align)
            {
                case Alignment.right:
                    rightToolStripMenuItem.Checked = true;
                    break;
                case Alignment.center:
                    centerToolStripMenuItem.Checked = true;
                    break;
                default:
                    leftToolStripMenuItem.Checked = true;
                    break;
            }
        }

        private void Display(HistoryList hl)
        {
            DrawSystem(uctg.GetCurrentHistoryEntry);
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += Display;
        }

        /// <summary>
        /// Called when the cursor move to another system
        /// </summary>
        /// <param name="thc"></param>
        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;
        }

        public override void Closing()
        {
            uctg.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewEntry -= NewEntry;
            discoveryform.OnHistoryChange -= Display;
        }

        public override void InitialDisplay()
        {
            DrawSystem(uctg?.GetCurrentHistoryEntry);
        }

        #endregion

        #region Events

        /// <summary>
        /// called when a new entry is made.. check to see if its a scan update
        /// </summary>
        /// <param name="he">HistoryEntry</param>
        /// <param name="hl">HistoryList</param>
        private void NewEntry(HistoryEntry he, HistoryList hl)
        {
            if (he.EntryType == JournalTypeEnum.Scan)
            {
                DrawSystem(he);
            }

            if (he.EntryType == JournalTypeEnum.FSSAllBodiesFound)
            {
                SetControlText("System scan complete.".Tx(this));
            }

            if (he.EntryType == JournalTypeEnum.FSDJump)
            {
                SetControlText(string.Empty);
                pictureBoxSurveyor.ClearImageList();
                Refresh();
            }

            if (he.EntryType == JournalTypeEnum.FSSDiscoveryScan)
            {
                var je = he.journalEntry as JournalFSSDiscoveryScan;
                if (je != null)
                {
                    var bodies_found = je.BodyCount;
                    SetControlText(bodies_found + " bodies found.".Tx(this));
                }
            }
        }

        public override Color ColorTransparency => Color.Green;

        public int labelOffset { get; private set; }

        public override void SetTransparency(bool on, Color curcol)
        {
            this.BackColor = curcol;
        }

        private void Display(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            DrawSystem(he);
        }

        #endregion

        #region Main

        private void DrawSystem(HistoryEntry he)
        {
            pictureBoxSurveyor.ClearImageList();

            StarScan.SystemNode scannode = null;

            var samesys = last_he?.System != null && he?.System != null && he.System.Name == last_he.System.Name;

            if (he == null)     //  no he, no display
            {
                last_he = he;
                SetControlText("No scan reported.".Tx(this));
                return;
            }
            else
            {
                scannode = discoveryform.history.starscan.FindSystem(he.System, true);        // get data with EDSM

                if (scannode == null)     // no data, clear display, clear any last_he so samesys is false next time
                {
                    last_he = null;
                    SetControlText("No scan reported.".Tx(this));
                    return;
                }
            }

            last_he = he;

            var all_nodes = scannode.Bodies.ToList();

            if (all_nodes != null)
            {
                int vpos = 0;

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
                                                (sd.Terraformable && terraformableToolStripMenuItem.Checked)
                                                )
                        {
                            if (!sd.Mapped || hideAlreadyMappedBodiesToolStripMenuItem.Checked == false)      // if not mapped, or show mapped
                            {
                                DrawToScreen(sd, vpos);
                                vpos += (int)Font.Height;
                            }
                        }
//                        wanted_nodes.Add(new BodyInfo(sn.ScanData.BodyName, sn.ScanData.GetPlanetClassImage(), distanceString.ToString(), hasrings, terraformable, volcanism, sn.ScanData.Volcanism, ammonia, earthlike, waterworld, mapped));
                    }
                }
            }

            pictureBoxSurveyor.Render();
        }

        private void DrawToScreen(JournalScan sd, int vpos)
        {
            var information = new StringBuilder();

            if (sd.Mapped)
                information.Append("\u2713"); // let the cmdr see that this body is already mapped - this is a check

            // Name
            information.Append(sd.BodyName);

            // Additional information
            information.Append((sd.AmmoniaWorld) ? @" is an ammonia world.".Tx(this) : null);
            information.Append((sd.Earthlike) ? @" is an earth like world.".Tx(this) : null);
            information.Append((sd.WaterWorld && !sd.Terraformable) ? @" is a water world.".Tx(this) : null);
            information.Append((sd.WaterWorld && sd.Terraformable) ? @" is a terraformable water world.".Tx(this) : null);
            information.Append((sd.Terraformable && !sd.WaterWorld) ? @" is a terraformable planet.".Tx(this) : null);
            information.Append((sd.HasRings) ? @" Has ring.".Tx(this) : null);
            information.Append((sd.HasMeaningfulVolcanism) ? @" Has ".Tx(this) + sd.Volcanism : null);
            information.Append(@" " + sd.DistanceFromArrivalText);
            if ( sd.WasMapped == true && sd.WasDiscovered == true )
                information.Append(" (Mapped & Discovered)".Tx(this));
            else if (sd.WasMapped == true)
                information.Append(" (Mapped)".Tx(this));
            else if (sd.WasDiscovered == true)
                information.Append(" (Discovered)".Tx(this));

            var textcolour = IsTransparent ? discoveryform.theme.SPanelColor : discoveryform.theme.LabelColor;
            var backcolour = IsTransparent ? Color.Transparent : this.BackColor;

            using (var bitmap = new Bitmap(1, 1))
            {
                var grfx = Graphics.FromImage(bitmap);

                var containerSize = new Size(Math.Max(pictureBoxSurveyor.Width,24), 24);        // note when minimized, we could have a tiny width, so need to protect
                var label = information.ToString();

                var bounds = BitMapHelpers.DrawTextIntoAutoSizedBitmap(label, containerSize, Font, textcolour, backcolour, 1.0F);

                if (align == Alignment.center)
                {
                    labelOffset = (int)((containerSize.Width - bounds.Width) / 2);
                }
                else if (align == Alignment.right)
                {
                    labelOffset = (int)(containerSize.Width - bounds.Width);
                }
                else
                {
                    labelOffset = 0;
                }

                pictureBoxSurveyor?.AddTextAutoSize(
                        new Point(labelOffset, vpos),
                        new Size((int)bounds.Width, 24),
                        information.ToString(),
                        Font,
                        textcolour,
                        backcolour,
                        1.0F);
            }
        }

        #endregion

        private void ammoniaWorldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "showAmmonia", ammoniaWorldToolStripMenuItem.Checked);
            DrawSystem(last_he);
        }

        private void earthlikeWorldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "showEarthlike", earthlikeWorldToolStripMenuItem.Checked);
            DrawSystem(last_he);
        }

        private void waterWorldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "showWaterWorld", waterWorldToolStripMenuItem.Checked);
            DrawSystem(last_he);
        }

        private void terraformableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "showTerraformable", terraformableToolStripMenuItem.Checked);
            DrawSystem(last_he);
        }

        private void hasVolcanismToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "showVolcanism", hasVolcanismToolStripMenuItem.Checked);
            DrawSystem(last_he);
        }

        private void hasRingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "showRinged", hasRingsToolStripMenuItem.Checked);
            DrawSystem(last_he);
        }

        private void hideAlreadyMappedBodiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "hideMapped", hideAlreadyMappedBodiesToolStripMenuItem.Checked);
            DrawSystem(last_he);
        }

        private void leftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingInt(DbSave + nameof(align), (int)Alignment.left);

            align = Alignment.left;

            if (leftToolStripMenuItem.Checked)
            {
                centerToolStripMenuItem.Checked = false;
                rightToolStripMenuItem.Checked = false;
            }

            DrawSystem(last_he);
        }

        private void centerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingInt(DbSave + nameof(align), (int)Alignment.center);

            align = Alignment.center;

            if (centerToolStripMenuItem.Checked)
            {
                leftToolStripMenuItem.Checked = false;
                rightToolStripMenuItem.Checked = false;
            }

            DrawSystem(last_he);
        }

        private void rightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingInt(DbSave + nameof(align), (int)Alignment.right);

            align = Alignment.right;

            if (rightToolStripMenuItem.Checked)
            {
                centerToolStripMenuItem.Checked = false;
                leftToolStripMenuItem.Checked = false;
            }

            DrawSystem(last_he);
        }

        private void UserControlSurveyor_Resize(object sender, EventArgs e)
        {
            DrawSystem(last_he);
        }
    }
}