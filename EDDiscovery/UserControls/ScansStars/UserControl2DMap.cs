/*
 * Copyright © 2015 - 2021 EDDiscovery development team
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

using BaseUtils;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using EMK.LightGeometry;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControl2DMap : UserControlCommonBase
    {
        public List<Map2d> images;
        private Map2d currentimage;
        
        private DateTime startDate, endDate;
        public bool DisplayTestGrid = false;

        private ExtendedControls.ExtDateTimePicker pickerStart, pickerStop;
        ToolStripControlHost host1, host2;
        List<Point3D> starpositions = null;

        List<HistoryEntry> systemlist;
        List<HistoryEntry> displayedlist;

        ToolTip controltooltip = new ToolTip();

        public UserControl2DMap() 
        {
            InitializeComponent();
            controltooltip.InitialDelay = 250;
            controltooltip.AutoPopDelay = 30000;
            controltooltip.ReshowDelay = 500;
            controltooltip.IsBalloon = true;
            imageViewer.MouseMove += ImageViewer_MouseMove;
            imageViewer.MaxZoom = 800;
            DBBaseName = "2DMap";
        }

        protected override void Init()
        {
            systemlist = HistoryList.FilterByFSDCarrierJumpAndPosition(DiscoveryForm.History.EntryOrder());

            pickerStart = new ExtendedControls.ExtDateTimePicker();
            pickerStop = new ExtendedControls.ExtDateTimePicker();
            pickerStart.Size = new Size(200, 20);
            pickerStop.Size = new Size(200, 20);
            host1 = new ToolStripControlHost(pickerStart);
            toolStrip.Items.Add(host1);
            host2 = new ToolStripControlHost(pickerStop);
            toolStrip.Items.Add(host2);
            pickerStart.Value = DateTime.Today.AddMonths(-1);

            this.pickerStart.ValueChanged += new System.EventHandler(this.dateTimePickerStart_ValueChanged);
            this.pickerStop.ValueChanged += new System.EventHandler(this.dateTimePickerStop_ValueChanged);

            startDate = new DateTime(2010, 1, 1);

            images = EDDiscovery.Icons.IconMaps.StandardMaps();
            images.AddRange(Map2d.LoadFromFolder(EDDOptions.Instance.MapsAppDirectory()));

            toolStripComboExpo.Items.Clear();

            foreach (Map2d img in images)
            {
                toolStripComboExpo.Items.Add(img.FileName);
            }

            toolStripComboBoxTime.Items.AddRange(new string[] {
            "Last Week".Tx(),
            "Last Month".Tx(),
            "Last Year".Tx(),
            "All".Tx(),
            "Custom".Tx()});

            toolStripComboExpo.SelectedIndex = 0;       // causes a display
            toolStripComboBoxTime.SelectedIndex = 3;
            
            imageViewer.BackColor = Color.FromArgb(5, 5, 5);

            DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange;
            DiscoveryForm.OnNewEntry += Discoveryform_OnNewEntry;
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he)
        {
            if (he.IsFSDCarrierJump)
            {
                systemlist = HistoryList.FilterByFSDCarrierJumpAndPosition(DiscoveryForm.History.EntryOrder());
                if (imageViewer.Image != null)      // make sure we have a image up, if so, just augment the travel history
                    DrawTravelHistory();
                else
                    Display();                      // else do the whole thing
            }
        }

        private void Discoveryform_OnHistoryChange()
        {
            systemlist = HistoryList.FilterByFSDCarrierJumpAndPosition(DiscoveryForm.History.EntryOrder());
            Display();
        }

        protected override void Closing()
        {
            DiscoveryForm.OnHistoryChange -= Discoveryform_OnHistoryChange;
            DiscoveryForm.OnNewEntry -= Discoveryform_OnNewEntry;
            imageViewer.Image?.Dispose();
            imageViewer.Image = null;
            foreach (var i in images)
                i.Dispose();
            images = null;
            starpositions = null;
        }

        private void Display()
        {
            string str = toolStripComboExpo.SelectedItem.ToString();

            Map2d map = images.FirstOrDefault(i => i.FileName == str);

            if (map != null)
            {
                try
                {
                    imageViewer.Image?.Dispose();
                    imageViewer.Image = (Bitmap)map.Image.Clone();      // clone as we are going to draw on it.
                    imageViewer.ZoomToFit();
                    currentimage = map;
                    if (toolStripButtonStars.Checked)
                        DrawStars();

                    DrawTravelHistory();
                }
                catch ( Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Loading 2dmap " + ex);
                    imageViewer.Image = null;
                }


                imageViewer.Invalidate();
            }
        }

        private void DrawTravelHistory()
        {
            DateTime start = startDate;

            int currentcmdr = EDCommander.CurrentCmdrID;

            displayedlist = (from systems in systemlist where systems.EventTimeUTC.ToLocalTime() > start && systems.EventTimeUTC.ToLocalTime() < endDate && systems.IsFSDLocationCarrierJump orderby systems.EventTimeUTC select systems).ToList();

            //foreach (var j in displayedlist)  System.Diagnostics.Debug.WriteLine("Jump {0} {1},{2},{3}", j.System.Name, j.System.X, j.System.Y, j.System.Z);

            Color drawcolour = Color.Green;

            using (Graphics gfx = Graphics.FromImage(imageViewer.Image))
            {
                for (int ii = 0; ii < displayedlist.Count-1; ii++)
                {
                    if (displayedlist[ii].journalEntry is IJournalJumpColor)
                    {
                        drawcolour = Color.FromArgb(((IJournalJumpColor)displayedlist[ii].journalEntry).MapColor);
                        if (drawcolour.GetBrightness() < 0.05)
                            drawcolour = Color.Red;
                    }

                    using (Pen pen = new Pen(drawcolour, 2))
                        DrawLine(gfx, pen, displayedlist[ii].System, displayedlist[ii+1].System);
                }

                if (DisplayTestGrid)
                    TestGrid(gfx);
            }
        }

        private void DrawStars()
        {
            Cursor.Current = Cursors.WaitCursor;

            if ( starpositions == null )
                starpositions = SystemsDB.GetStarPositions(10,(x,y,z)=> { return new Point3D(x, y, z); });     // limit to 10%

            using (Pen pen = new Pen(Color.White, 2))
            using (Graphics gfx = Graphics.FromImage(imageViewer.Image))
            {
                foreach (Point3D si in starpositions)
                {
                    DrawPoint(gfx, pen, si.X,si.Z );
                }
            }

            Cursor.Current = Cursors.Default;
        }

        private void DrawLine(Graphics gfx, Pen pen, ISystem sys1, ISystem sys2)
        {
            var p1 = currentimage.TransformCoordinate(new PointF((float)sys1.X, (float)sys1.Z));
            var p2 = currentimage.TransformCoordinate(new PointF((float)sys2.X, (float)sys2.Z));
            gfx.DrawLine(pen, p1 , p2);
        }

        private void DrawPoint(Graphics gfx, Pen pen, double x, double z)
        {
            PointF point = currentimage.TransformCoordinate(new Point((int)x, (int)z));
            gfx.FillRectangle(pen.Brush, point.X, point.Y, 1, 1);
        }

        private void TestGrid(Graphics gfx)
        {
            using (Pen pointPen = new Pen(Color.LawnGreen, 3))
                for (int x = currentimage.BottomLeft.X; x <= currentimage.BottomRight.X; x += 1000)
                    for (int z = currentimage.BottomLeft.Y; z <= currentimage.TopLeft.Y; z += 1000)
                        gfx.DrawLine(pointPen, currentimage.TransformCoordinate(new Point(x, z)), currentimage.TransformCoordinate(new Point(x + 10, z)));
        }

        private void ImageViewer_MouseMove(object sender, MouseEventArgs e)
        {
            if (displayedlist != null && imageViewer.PositionFromMouse(e.Location, out Point pos))       // 0,0 is top left
            {
                PointF lypos = currentimage.TransformCoordinate(pos, false);
               // System.Diagnostics.Debug.WriteLine("Point " + pos + " " + lypos);

                const int lydistance = 50;      // from 90000ly/4096 pixes, 22 ly per pixel, allow a pixel error

                HashSet<string> displayedit = new HashSet<string>();
                string list = "";
                foreach( var he in displayedlist)
                {

                    if (!displayedit.Contains(he.System.Name) && ObjectExtensionsNumbersBool.Length(lypos.X, lypos.Y,  he.System.X,he.System.Z) < lydistance)
                    {
                        list = list.AppendPrePad($"{he.System.Name} @ {he.System.X.ToString("N2")},{he.System.Y.ToString("N2")},{he.System.Z.ToString("N2")}", Environment.NewLine);
                        displayedit.Add(he.System.Name);
                       
                        if ( displayedit.Count>10)      // too many, truncate, happens around sol
                        {
                            list += Environment.NewLine + "...";
                            break;
                        }
                    }
                }

                if (list.HasChars())
                    controltooltip.SetToolTip(imageViewer, list);
                else
                    controltooltip.Hide(imageViewer);

            }
        }

        private void toolStripComboBoxExpo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Display();
        }


        private void toolStripComboBoxTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nr = toolStripComboBoxTime.SelectedIndex;

            endDate = DateTime.Today.AddDays(1);
            if (nr == 0)
                startDate = DateTime.Now.AddDays(-7);
            else if (nr == 1)
                startDate = DateTime.Now.AddMonths(-1);
            else if (nr == 2)
                startDate = DateTime.Now.AddYears(-1);
            else if (nr == 3)
                startDate = new DateTime(2010, 8, 1);
            else if (nr == 4)  // Custom
                startDate = new DateTime(2010, 8, 1);

            if (nr == 4)
            {
                host1.Visible = true;
                host2.Visible = true;
                endDate = pickerStop.Value;
                startDate = pickerStart.Value;
            }
            else
            {
                host1.Visible = false;
                host2.Visible = false;
                endDate = DateTime.Today.AddDays(1);
            }
            
            Display();
        }

        private void toolStripButtonZoomIn_Click(object sender, EventArgs e)
        {
            imageViewer.ZoomIn();
        }

        private void dateTimePickerStart_ValueChanged(object sender, EventArgs e)
        {
            startDate = pickerStart.Value;
            Display();
        }

        private void dateTimePickerStop_ValueChanged(object sender, EventArgs e)
        {
            endDate = pickerStop.Value;
            Display();
        }

        private void toolStripButtonStars_Click(object sender, EventArgs e)
        {
            Display();
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                switch (saveFileDialog.FilterIndex)
                {
                    case 1:
                        imageViewer.Image.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case 2:
                        imageViewer.Image.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case 3:
                        imageViewer.Image.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                }
            }
        }

        private void toolStripButtonZoomOut_Click(object sender, EventArgs e)
        {
            imageViewer.ZoomOut();
        }

        private void toolStripButtonZoomtoFit_Click(object sender, EventArgs e)
        {
            imageViewer.ZoomToFit();
        }
    }
}
