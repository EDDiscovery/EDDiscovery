﻿/*
 * Copyright © 2015 - 2017 EDDiscovery development team
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
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery
{
    public partial class Form2DMap : Forms.DraggableFormPos
    {
        public List<Map2d> fgeimages;
        private Map2d currentFGEImage;
        
        private DateTime startDate, endDate;
        public bool DisplayTestGrid = false;

        private DateTimePicker pickerStart, pickerStop;
        ToolStripControlHost host1, host2;
        List<Point3D> starpositions = null;

        List<HistoryEntry> syslist;

        public Form2DMap(List<HistoryEntry> sl) 
        {
            RestoreFormPositionRegKey = "Map2DForm";
            syslist = sl;
            InitializeComponent();
        }

        bool initdone = false;
        private void Form2dLoad(object sender, EventArgs e)
        {
            initdone = false;
            pickerStart = new DateTimePicker();
            pickerStop = new DateTimePicker();
            host1 = new ToolStripControlHost(pickerStart);
            toolStrip.Items.Add(host1);
            host2 = new ToolStripControlHost(pickerStop);
            toolStrip.Items.Add(host2);
            pickerStart.Value = DateTime.Today.AddMonths(-1);

            this.pickerStart.ValueChanged += new System.EventHandler(this.dateTimePickerStart_ValueChanged);
            this.pickerStop.ValueChanged += new System.EventHandler(this.dateTimePickerStop_ValueChanged);

            startDate = new DateTime(2010, 1, 1);
            if ( !AddImages() )
            {
                ExtendedControls.MessageBoxTheme.Show(this, "2DMaps", "No maps available", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                Close();
                return;
            }

            toolStripComboExpo.Items.Clear();

            foreach (Map2d img in fgeimages)
            {
                toolStripComboExpo.Items.Add(img.FileName);
            }

            toolStripComboBoxTime.Items.AddRange(new string[] {
            "Distant Worlds Expedition",
            "FGE Expedition start",
            "Last Week",
            "Last Month",
            "Last Year",
            "All",
            "Custom"});

            toolStripComboExpo.SelectedIndex = 0;
            toolStripComboBoxTime.SelectedIndex = 0;
            initdone = true;
            ShowSelectedImage();

            imageViewer.BackColor = Color.FromArgb(5, 5, 5);

            EDDiscovery.EDDTheme theme = EDDiscovery.EDDTheme.Instance;
            bool winborder = theme.ApplyToForm(this);
            statusStripCustom.Visible = panel_close.Visible = panel_minimize.Visible = !winborder;
            
        }

        private void Form2dClosing(object sender, FormClosingEventArgs e)
        {
            if (imageViewer.Image != null)
            {
                imageViewer.Image.Dispose();
            }
            imageViewer.Image = null;
            fgeimages = null;
            starpositions = null;
        }

        private bool AddImages()
        {
            fgeimages = new List<Map2d>();
            string datapath = Path.Combine(EDDOptions.Instance.AppDataDirectory, "Maps");
            if (Directory.Exists(datapath))
            {
                fgeimages = Map2d.LoadImages(datapath);
                return fgeimages.Count > 0;
            }
            else
                return false;
        }

        private void DrawTravelHistory()
        {
            DateTime start = startDate;

            int currentcmdr = EDCommander.CurrentCmdrID;

            var history = from systems in syslist where systems.EventTimeLocal > start && systems.EventTimeLocal<endDate && systems.System.HasCoordinate == true  orderby systems.EventTimeUTC  select systems;
            List<HistoryEntry> listHistory = history.ToList();

            using (Graphics gfx = Graphics.FromImage(imageViewer.Image))
            {
                if (listHistory.Count > 1)
                {
                    for (int ii = 1; ii < listHistory.Count; ii++)
                    {
                        Color a = Color.FromArgb(listHistory[ii].MapColour);
                        Color b = (a.IsFullyTransparent()) ? Color.FromArgb(255, a) : a;

                        using (Pen pen = new Pen(b, 2))
                            DrawLine(gfx, pen, listHistory[ii - 1].System, listHistory[ii].System);
                    }
                }

                if (DisplayTestGrid)
                    TestGrid(gfx);
            }
        }

        private void DrawStars()
        {
            Cursor.Current = Cursors.WaitCursor;

            if ( starpositions == null )
                starpositions = SystemClassDB.GetStarPositions(25);     // limit to 25%

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
            gfx.DrawLine(pen, Transform2Screen(currentFGEImage.TransformCoordinate(new Point((int)sys1.X, (int)sys1.Z))), Transform2Screen(currentFGEImage.TransformCoordinate(new Point((int)sys2.X, (int)sys2.Z))));
        }

        private void DrawPoint(Graphics gfx, Pen pen, ISystem sys1, ISystem sys2)
        {
            Point point = Transform2Screen(currentFGEImage.TransformCoordinate(new Point((int)sys1.X, (int)sys1.Z)));
            gfx.FillRectangle(pen.Brush, point.X, point.Y, 1, 1);
        }

        private void DrawPoint(Graphics gfx, Pen pen, double x, double z)
        {
            Point point = Transform2Screen(currentFGEImage.TransformCoordinate(new Point((int)x, (int)z)));
            gfx.FillRectangle(pen.Brush, point.X, point.Y, 1, 1);
        }

        private void TestGrid(Graphics gfx)
        {
            using (Pen pointPen = new Pen(Color.LawnGreen, 3))
                for (int x = currentFGEImage.BottomLeft.X; x <= currentFGEImage.BottomRight.X; x += 1000)
                    for (int z = currentFGEImage.BottomLeft.Y; z <= currentFGEImage.TopLeft.Y; z += 1000)
                        gfx.DrawLine(pointPen, currentFGEImage.TransformCoordinate(new Point(x, z)), currentFGEImage.TransformCoordinate(new Point(x + 10, z)));
        }


        private Point Transform2Screen(Point point)
        {
            return point;
        }

        private void toolStripComboBoxExpo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowSelectedImage();
        }

        private void ShowSelectedImage()
        {
            string str = toolStripComboExpo.SelectedItem.ToString();

            Map2d fgeimg = fgeimages.FirstOrDefault(i => i.FileName == str);

            if (fgeimg != null && initdone)
            {
                imageViewer.Image?.Dispose();
                //panel1.BackgroundImage = new Bitmap(fgeimg.FilePath);
                imageViewer.Image = new Bitmap(fgeimg.FilePath);
                imageViewer.ZoomToFit();
                currentFGEImage = fgeimg;

                if (toolStripButtonStars.Checked)
                    DrawStars();

                DrawTravelHistory();
                imageViewer.Invalidate();
            }
        }

        private void toolStripComboBoxTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nr = toolStripComboBoxTime.SelectedIndex;
            /*
            Distant Worlds Expedition
            FGE Expedition start
            Last Week
            Last Month
            Last Year
            All
            */

            endDate = DateTime.Today.AddDays(1);
            if (nr == 0)
                startDate = new DateTime(2016, 1, 14);
            else if (nr == 1)
                startDate = new DateTime(2015, 8, 1);
            else if (nr == 2)
                startDate = DateTime.Now.AddDays(-7);
            else if (nr == 3)
                startDate = DateTime.Now.AddMonths(-1);
            else if (nr == 4)
                startDate = DateTime.Now.AddYears(-1);
            else if (nr == 5)
                startDate = new DateTime(2010, 8, 1);
            else if (nr == 6)  // Custom
                startDate = new DateTime(2010, 8, 1);

            if (nr == 6)
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
            
            ShowSelectedImage();
        }

        private void toolStripButtonZoomIn_Click(object sender, EventArgs e)
        {
            imageViewer.ZoomIn();
        }

        private void dateTimePickerStart_ValueChanged(object sender, EventArgs e)
        {
            startDate = pickerStart.Value;
            ShowSelectedImage();
        }

        private void dateTimePickerStop_ValueChanged(object sender, EventArgs e)
        {
            endDate = pickerStop.Value;
            ShowSelectedImage();
        }

        private void toolStripButtonStars_Click(object sender, EventArgs e)
        {
            ShowSelectedImage();
        }

        private void panel_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panel_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panelTop_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void panelTop_MouseUp(object sender, MouseEventArgs e)
        {
            OnCaptionMouseUp((Control)sender, e);
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        imageViewer.Image.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case 2:
                        imageViewer.Image.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case 3:
                        imageViewer.Image.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
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
