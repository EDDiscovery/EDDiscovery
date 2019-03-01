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

        private Font displayfont;

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

            displayfont = discoveryform.theme.GetFont;
            EDDTheme.Instance.ApplyToControls(this);

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
            DrawSystem(uctg.GetCurrentHistoryEntry);
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

        /// <summary>
        /// Called at first start or hooked to change cursor
        /// </summary>
        /// <param name="he">HistoryEntry</param>
        /// <param name="hl">HistoryList</param>
        /// <param name="selectedEntry">todo: describe selectedEntry parameter on Display</param>
        private void Display(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            DrawSystem(he);
        }

        #endregion

        #region Main

        public class WantedBodies
        {
            public string Name { get; set; }
            public Image Img { get; set; }
            public string DistanceFromArrival { get; internal set; }

            public bool Ammonia, Earthlike, WaterWorld, Terraformable, Volcanism, Ringed, Mapped;

            public string VolcanismString { get; set; }
        }

        public static WantedBodies WantedBodiesList(string bdName, Image bdImg, string distance, bool bodyHasRings, bool bodyIsTerraformable, bool bodyHasVolcanism, string bodyVolcanismString, bool isAmmoniaWorld, bool isAnEarthLike, bool isWaterWorld, bool mapped)
        {
            return new WantedBodies()
            {
                Name = bdName,
                Img = bdImg,
                DistanceFromArrival = distance,
                Ringed = bodyHasRings,
                Terraformable = bodyIsTerraformable,
                Volcanism = bodyHasVolcanism,
                VolcanismString = bodyVolcanismString,
                Ammonia = isAmmoniaWorld,
                Earthlike = isAnEarthLike,
                WaterWorld = isWaterWorld,
                Mapped = mapped
            };
        }

        /// <summary>
        /// Retrieve the list of bodies which match the user needs
        /// </summary>
        /// <param name="he">HistoryEntry</param>        
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
                var wanted_nodes = new List<WantedBodies>();

                foreach (StarScan.ScanNode sn in all_nodes)
                {
                    if (sn.ScanData != null && sn.ScanData?.BodyName != null && !sn.ScanData.IsStar)
                    {
                        bool hasrings, terraformable, volcanism, ammonia, earthlike, waterworld, mapped;

                        if (sn.ScanData.HasRings || sn.ScanData.Terraformable || sn.ScanData.Volcanism != null || sn.ScanData.PlanetTypeID == EDPlanet.Earthlike_body || sn.ScanData.PlanetTypeID == EDPlanet.Ammonia_world || sn.ScanData.PlanetTypeID == EDPlanet.Water_world)
                        {
                            hasrings = sn.ScanData.PlanetTypeID == EDPlanet.Ammonia_world ? true : false;
                            terraformable = sn.ScanData.Terraformable ? true : false;
                            volcanism = sn.ScanData.Volcanism != null ? true : false;
                            ammonia = sn.ScanData.PlanetTypeID == EDPlanet.Ammonia_world ? true : false;
                            earthlike = sn.ScanData.PlanetTypeID == EDPlanet.Earthlike_body ? true : false;
                            waterworld = sn.ScanData.PlanetTypeID == EDPlanet.Water_world ? true : false;

                            mapped = sn.ScanData.Mapped;

                            var distanceString = new StringBuilder();

                            distanceString.AppendFormat("{0:0.00}AU ({1:0.0}ls)", sn.ScanData.DistanceFromArrivalLS / JournalScan.oneAU_LS, sn.ScanData.DistanceFromArrivalLS);

                            wanted_nodes.Add(WantedBodiesList(sn.ScanData.BodyName, sn.ScanData.GetPlanetClassImage(), distanceString.ToString(), hasrings, terraformable, volcanism, sn.ScanData.Volcanism, ammonia, earthlike, waterworld, mapped));
                        }
                    }
                }

                SelectBodiesToDisplay(wanted_nodes);
            }

            pictureBoxSurveyor.Render();
        }

        private void SelectBodiesToDisplay(List<WantedBodies> wanted_nodes)
        {
            if (wanted_nodes != null)
            {
                var bodiesCount = 0;

                using (var body = wanted_nodes.GetEnumerator())
                {
                    while (body.MoveNext())
                    {
                        if (body.Current.Ammonia && ammoniaWorldToolStripMenuItem.Checked)
                        {
                            bodiesCount++;
                            DrawToScreen(body.Current, bodiesCount);
                        }

                        if (body.Current.Earthlike && earthlikeWorldToolStripMenuItem.Checked)
                        {
                            bodiesCount++;
                            DrawToScreen(body.Current, bodiesCount);
                        }

                        if (body.Current.WaterWorld && waterWorldToolStripMenuItem.Checked)
                        {
                            bodiesCount++;
                            DrawToScreen(body.Current, bodiesCount);
                        }

                        if (body.Current.Ringed && !body.Current.Ammonia && !body.Current.Earthlike && !body.Current.WaterWorld && hasRingsToolStripMenuItem.Checked)
                        {
                            bodiesCount++;
                            DrawToScreen(body.Current, bodiesCount);
                        }

                        if (body.Current.Volcanism && !body.Current.Ammonia && !body.Current.Earthlike && !body.Current.WaterWorld && hasVolcanismToolStripMenuItem.Checked)
                        {
                            bodiesCount++;
                            DrawToScreen(body.Current, bodiesCount);
                        }

                        if (body.Current.Terraformable && !body.Current.Ammonia && !body.Current.Earthlike && !body.Current.WaterWorld && terraformableToolStripMenuItem.Checked)
                        {
                            bodiesCount++;
                            DrawToScreen(body.Current, bodiesCount);
                        }
                    }
                }
            }
        }

        private void DrawToScreen(WantedBodies body, int bodiesCount)
        {
            var information = new StringBuilder();

            // Is already surface mapped or not?
            if (body.Mapped)
                information.Append("o "); // let the cmdr see that this body is already mapped

            // Name
            information.Append(body.Name);

            // Additional information
            information.Append((body.Ammonia) ? @" is an ammonia world.".Tx(this) : null);
            information.Append((body.Earthlike) ? @" is an earth like world.".Tx(this) : null);
            information.Append((body.WaterWorld && !body.Terraformable) ? @" is a water world.".Tx(this) : null);
            information.Append((body.WaterWorld && body.Terraformable) ? @" is a terraformable water world.".Tx(this) : null);
            information.Append((body.Terraformable && !body.WaterWorld) ? @" is a terraformable planet.".Tx(this) : null);
            information.Append((body.Ringed) ? @" Has ring.".Tx(this) : null);
            information.Append((body.Volcanism) ? @" Has ".Tx(this) + body.VolcanismString : null);
            information.Append(@" " + body.DistanceFromArrival);

            //Debug.Print(information.ToString()); // for testing

            // Drawing Elements
            const int rowHeight = 24;
            var vPos = (bodiesCount * rowHeight) - rowHeight;

            var textcolour = IsTransparent ? discoveryform.theme.SPanelColor : discoveryform.theme.LabelColor;
            var backcolour = IsTransparent ? Color.Transparent : this.BackColor;

            using (var bitmap = new Bitmap(1, 1))
            {
                var grfx = Graphics.FromImage(bitmap);

                using (var font = new Font(displayfont, FontStyle.Regular))
                {
                    if (body != null)
                    {
                        if (!body.Mapped || (body.Mapped && !hideAlreadyMappedBodiesToolStripMenuItem.Checked))
                        {
                            var containerSize = new Size(pictureBoxSurveyor.Width, 24);
                            var label = information.ToString();

                            var bounds = BitMapHelpers.DrawTextIntoAutoSizedBitmap(label, containerSize, displayfont, textcolour, backcolour, 1.0F);

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
                                    new Point(labelOffset, vPos + 4),
                                    new Size((int)bounds.Width, 24),
                                    information.ToString(),
                                    displayfont,
                                    textcolour,
                                    backcolour,
                                    1.0F);
                        }

                        pictureBoxSurveyor.Refresh();
                    }
                }
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

        private void pictureBoxSurveyorAid_MouseClick(object sender, MouseEventArgs e)
        {
            contextMenuStrip.Visible |= e.Button == MouseButtons.Right;
            contextMenuStrip.Top = MousePosition.Y;
            contextMenuStrip.Left = MousePosition.X;
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