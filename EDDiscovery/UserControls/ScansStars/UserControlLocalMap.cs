/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Concurrent;
using System.Threading;
using EliteDangerousCore;
using EliteDangerousCore.EDSM;
using EliteDangerousCore.DB;
using System.Diagnostics;


namespace EDDiscovery.UserControls
{
    public partial class UserControlLocalMap : UserControlCommonBase
    {
        #region init

        private string DbSave { get { return DBName("MapPanel" ); } }

        private StarDistanceComputer computer;

        public UserControlLocalMap()
        {
            InitializeComponent();
            this.chartMap.MouseWheel += Zoom_MouseWheel;
        }

        private HistoryEntry last_he = null;
        private const double defaultMapMaxRadius = 1000;
        private const double defaultMapMinRadius = 0;
        private int maxitems = 500;
        private System.Windows.Forms.Timer slidetimer;

        public override void Init()
        {
            computer = new StarDistanceComputer();

            slideMaxItems.Value = maxitems = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(DbSave + "MapMaxItems", maxitems);

            textMaxRadius.ValueNoChange = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(DbSave + "MapMax", defaultMapMaxRadius);
            textMinRadius.ValueNoChange = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(DbSave + "MapMin", defaultMapMinRadius);
            textMinRadius.SetComparitor(textMaxRadius, -2);     // need to do this after values are set
            textMaxRadius.SetComparitor(textMinRadius, 2);

            slidetimer = new System.Windows.Forms.Timer();
            slidetimer.Interval = 500;
            slidetimer.Tick += Slidetimer_Tick;

            var style = chartMap.ChartAreas[0].Area3DStyle;
            style.Rotation = Math.Min(180, Math.Max(-180, style.Rotation - (Convert.ToInt32(EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(DbSave + "MapRotationX", xr)))));
            style.Inclination = Math.Min(90, Math.Max(-90, style.Inclination + (Convert.ToInt32(EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(DbSave + "MapRotationY", yr)))));

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(contextMenuStrip, this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;
        }

        public override void Closing()
        {
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            computer.ShutDown();
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble(DbSave + "MapMin", textMinRadius.Value);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble(DbSave + "MapMax", textMaxRadius.Value);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(DbSave + "MapMaxItems", maxitems);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble(DbSave + "MapRotationX", prevxr);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble(DbSave + "MapRotationY", prevyr);
        }

        public override void InitialDisplay()
        {
            KickComputation(uctg.GetCurrentHistoryEntry);
        }

        private void Uctg_OnTravelSelectionChanged(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            KickComputation(he);
            RefreshMap();
            ControlReset(chartMap);
        }

        #endregion

        #region computer

        private void KickComputation(HistoryEntry he)
        {
            if (he?.System != null && he.System.HasCoordinate)
            {
                computer.CalculateClosestSystems(he.System,
                    (s, d) => this.ParentForm.BeginInvoke((MethodInvoker)delegate { NewStarListComputed(s, d); }),
                    maxitems, textMinRadius.Value, textMaxRadius.Value, true);
            }
        }

        private void NewStarListComputed(ISystem sys, BaseUtils.SortedListDoubleDuplicate<ISystem> list)      // In UI
        {
            System.Diagnostics.Debug.Assert(Application.MessageLoop);       // check!
            discoveryform.history.CalculateSqDistances(list, sys.X, sys.Y, sys.Z, maxitems, textMinRadius.Value, textMaxRadius.Value, true);
            FillMap(list, sys);
        }

        private void FillMap(SortedList<double, ISystem> csl, ISystem centerSystem)
        {
            SetControlText(string.Format("3D Map of closest systems from {0}".T(EDTx.UserControlLocalMap_3dmap), centerSystem.Name));

            for (int s = 0; s <= 100; s++)
            {
               chartMap.Series[s].MarkerSize = 6;
            }
            chartMap.Series[50].MarkerSize = 8;

            // Add the current system
            chartMap.Series[50].Points.AddXY(0, 0);
            chartMap.Series[50].ToolTip = centerSystem.Name; // tooltip

            System.Diagnostics.Debug.WriteLine("Max " + textMaxRadius.Value + " Min " + textMinRadius.Value + " Count " + csl.Count()); ;
            if (csl.Count() > 0)
            {
                foreach (KeyValuePair<double, ISystem> tvp in csl)
                {
                    if (tvp.Value != centerSystem)
                    {
                        var theISystemInQuestion = tvp.Value;
                        var sysX = theISystemInQuestion.X;
                        var sysY = theISystemInQuestion.Y;
                        var sysZ = theISystemInQuestion.Z;
                        var distFromCurrentSys = Math.Round(Math.Sqrt(tvp.Key), 2, MidpointRounding.AwayFromZero);
                        var curX = centerSystem.X;
                        var curY = centerSystem.Y;
                        var curZ = centerSystem.Z;

                        // reset charts axis
                        chartMap.ChartAreas[0].AxisY.IsStartedFromZero = false;
                        chartMap.ChartAreas[0].AxisX.IsStartedFromZero = false;
                        chartMap.ChartAreas[0].AxisX.Maximum = textMaxRadius.Value;
                        chartMap.ChartAreas[0].AxisX.Minimum = textMaxRadius.Value * -1;
                        chartMap.ChartAreas[0].AxisY.Maximum = textMaxRadius.Value;
                        chartMap.ChartAreas[0].AxisY.Minimum = textMaxRadius.Value * -1;

                        // depth of the series layers need to be adjusted, so to follow the X and Y axis

                        int sdepth;

                        if (textMaxRadius.Value < 25)
                        {
                            sdepth = (Convert.ToInt32((textMaxRadius.Value) * 3));
                        }
                        else if (textMaxRadius.Value == 26 || textMaxRadius.Value <= 50)
                        {
                            sdepth = (Convert.ToInt32((textMaxRadius.Value) * 2.66));
                        }
                        else if (textMaxRadius.Value == 51 || textMaxRadius.Value <= 100)
                        {
                            sdepth = (Convert.ToInt32((textMaxRadius.Value) * 2.33));
                        }
                        else
                        {
                            sdepth = (Convert.ToInt32((textMaxRadius.Value) * 2));
                        }

                        // maximum allowed depth in chart is 1000%
                        if (sdepth > 1000)
                        {
                            sdepth = 1000;
                        }

                        chartMap.ChartAreas[0].Area3DStyle.PointDepth = sdepth;

                        if (distFromCurrentSys > textMinRadius.Value) // we want to be able to define a shell 
                        {
                            int visits = discoveryform.history.GetVisitsCount(tvp.Value.Name);

                            // calculate distance for each axis; this provide a cubical distribution
                            double dx = curX - sysX;
                            double dy = curY - sysY;
                            double dz = curZ - sysZ;

                            int px = Convert.ToInt32(dx) * -1;
                            int py = Convert.ToInt32(dy);
                            int pz = Convert.ToInt32(dz) * -1;

                            int nseries = 101; // # of series in the 3d plot (0 to 49, before the current system; 50, our current system; 51 to 101, after the current system)
                            double ratio = ((textMaxRadius.Value * 2) / nseries);
                            double spy = ((py + Convert.ToInt32(textMaxRadius.Value)) / ratio); // steps (series) to Y coordinate

                            int ispy = Convert.ToInt32(spy);

                            // we don't want a very close system to be showed as current one, so if it share the same serie of the current system we put in right before or after.                            
                            if (ispy == 50)
                            {
                                ispy = 49;
                            }
                            if (ispy < 0)
                            {
                                ispy = 0;
                            }
                            if (ispy > 100)
                            {
                                ispy = 100;
                            }

                            // strings to add in tooltip
                            StringBuilder label = new StringBuilder();
                            label.Append(theISystemInQuestion.Name + " / " + visits + " visits" + "\n" + distFromCurrentSys + "\n" +
                                          "X: " + sysX + ", Y: " + sysY + ", Z:" + sysZ);

                            chartMap.Series[ispy].Points.AddXY(px, pz);
                            chartMap.Series[ispy].ToolTip = label.ToString(); // tooltips
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < chartMap.Series.Count; i++)
                    chartMap.Series[i].Points.Clear();

            }
        }

        #endregion

        #region refresh

        private void textMinRadius_ValueChanged(object sender, EventArgs e)
        {
            KickComputation(last_he ?? uctg.GetCurrentHistoryEntry);
        }

        private void textMaxRadius_ValueChanged(object sender, EventArgs e)
        {
            KickComputation(last_he ?? uctg.GetCurrentHistoryEntry);
        }

        private void RefreshMap()
        {
            // clean up all the series
            foreach (int s in Enumerable.Range(0, 100))
            {
                chartMap.Series[s].Points.Clear();
            }

            // update the chart
            chartMap.Update();
        }

        #endregion

        #region slide

        private void SlideMaxItems_Scroll(object sender, EventArgs e)
        {
            slidetimer.Start();
        }
        private void Slidetimer_Tick(object sender, EventArgs e)            // DONT use a Wait loop - this kills the rest of the system..
        {
            slidetimer.Stop();
            toolTip.SetToolTip(slideMaxItems, slideMaxItems.Value.ToString());
            maxitems = slideMaxItems.Value;
            KickComputation(uctg.GetCurrentHistoryEntry);
            RefreshMap();
        }

        private void SlideMaxItems_MouseHover(object sender, EventArgs e)
        {
            toolTip.SetToolTip(slideMaxItems, slideMaxItems.Value.ToString() + " max number of systems.");
        }

        #endregion

        #region rotation, pan and zoom

        private Point mousePosRotate;
        private Point mousePosPan;

        // zoom with the mouse scroll wheel
        private double[] zoomFactor = { 1.0, 1.25, 1.5, 1.75, 2.0, 2.5, 3.0, 3.5, 4.0, 5.0, 6.0, 8.0, 10.0 };
        private int zoomIndex = 0; // default zoom at 1:1

        private void Zoom_MouseWheel(object sender, MouseEventArgs e)
        {
            chartMap.Left = chartMap.Parent.Left;
            chartMap.Top = chartMap.Parent.Top;

            // Zoom In
            if (e.Delta > 0)
            {
                if (zoomIndex < 12)
                {
                    zoomIndex++;
                    ZoomControl(chartMap, zoomIndex, e);
                }
            }

            // Zoom Out
            else if (e.Delta < 0)
            {
                if (zoomIndex > 0)
                {
                    zoomIndex--;
                    ZoomControl(chartMap, zoomIndex, e);
                }
                else if (zoomIndex == 0) // necessary to avoid zoom try to reduce the map when 1:1
                {
                    zoomIndex = 0; // do nothing...
                }
            }
        }

        private void ZoomControl(Control ctrlToZoom, int zoomIndex, MouseEventArgs e)
        {
            // More than 1:1
            if (zoomFactor[zoomIndex] != 0)
            {
                // get the current position of the chart
                var ctrlPrevCoords = PointToScreen(new Point(ctrlToZoom.Left, ctrlToZoom.Top));

                // get the mouse position
                var mouseCoords = PointToScreen(new Point(e.X, e.Y));

                // multiply the control's size to the zoom factor 
                ctrlToZoom.Width = Convert.ToInt32(ctrlToZoom.Parent.Width * zoomFactor[zoomIndex]);
                ctrlToZoom.Height = Convert.ToInt32(ctrlToZoom.Parent.Height * zoomFactor[zoomIndex]);

                // calculate the new center
                var Center = new Point(Convert.ToInt32(ctrlToZoom.Parent.Width / 2), Convert.ToInt32(ctrlToZoom.Parent.Height / 2));

                // calculate the offset
                var Offset = new Point(Convert.ToInt32(mouseCoords.X - Center.X), Convert.ToInt32((mouseCoords.Y - Center.Y)));

                // calculate the new position of the control, offsetting it's center to the mouse coordinates
                var NewPosition = new Point(ctrlPrevCoords.X - Offset.X, ctrlPrevCoords.Y - Offset.Y);

                ctrlToZoom.Left = NewPosition.X;
                ctrlToZoom.Top = NewPosition.Y;
            }

            // 1:1
            if (zoomFactor[zoomIndex] == 0)
            {
                // resize to the minimum width and height
                ctrlToZoom.Width = ctrlToZoom.Parent.Width;
                ctrlToZoom.Height = ctrlToZoom.Parent.Height;

                // position the control in the center of the panel
                var chartNoZoom = new Point(ctrlToZoom.Parent.Left, ctrlToZoom.Parent.Top);
                ctrlToZoom.Left = chartNoZoom.X;
                ctrlToZoom.Top = chartNoZoom.Y;
            }
        }

        private void ZoomToFactor(Control ctrlToZoom, double zoomratio)
        {
            ctrlToZoom.Height = ctrlToZoom.Parent.Height;
            ctrlToZoom.Width = ctrlToZoom.Parent.Width;

            // multiply the chart's size to the zoom factor 
            ctrlToZoom.Width = Convert.ToInt32(ctrlToZoom.Parent.Width * zoomratio);
            ctrlToZoom.Height = Convert.ToInt32(ctrlToZoom.Parent.Height * zoomratio);
        }

        // coordinates for rotation mouse reposition computation
        private double xr = 0;
        private double yr = 0;
        private double prevxr;
        private double prevyr;

        private void ChartMap_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Smooth out the rotation of the chart
                Cursor.Hide(); // hide the cursor
                xr = Cursor.Position.X; // record the current mouse position
                yr = Cursor.Position.Y; // 

                if (prevxr.ToString() != null || prevyr.ToString() != null)
                {
                    prevxr = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(DbSave + "MapRotationX", xr);
                    prevyr = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(DbSave + "MapRotationY", yr);

                    Cursor.Position = new Point(Convert.ToInt32(prevxr), Convert.ToInt32(prevyr));
                }
            }

            if (e.Button == MouseButtons.Middle)
            {
                // change to the pan cursor
                Cursor.Current = Cursors.NoMove2D;
                mousePosPan = e.Location;
            }

            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }

        private void ChartMap_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                prevxr = Cursor.Position.X; // record the last mouse position
                prevyr = Cursor.Position.Y; //       

                EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble(DbSave + "MapRotationX", prevxr);
                EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble(DbSave + "MapRotationY", prevyr);

                CenterMouseOverControl(chartMap);
                Cursor.Show(); // show the cursor                
            }
            if (e.Button == MouseButtons.Middle)
            {
                // return to the default cursor
                Cursor.Current = Cursors.Default;
            }
        }

        private void UserControlMap_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }

        private bool panSwitch = false;

        private void ChartMap_MouseMove(object sender, MouseEventArgs e)
        {
            // rotate the map with the firt mouse button

            if (e.Button == MouseButtons.Left)
            {
                // rotate the chart            
                if (!mousePosRotate.IsEmpty)
                {
                    var style = chartMap.ChartAreas[0].Area3DStyle;
                    style.Rotation = Math.Min(180, Math.Max(-180, style.Rotation - (e.Location.X - mousePosRotate.X)));
                    style.Inclination = Math.Min(90, Math.Max(-90, style.Inclination + (e.Location.Y - mousePosRotate.Y)));
                }

                mousePosRotate = e.Location;
            }

            // pan the chart with the middle mouse buttom
            if (e.Button == MouseButtons.Middle & panSwitch == true)
            {
                PanControl(chartMap, e);
            }
        }

        // pan the map
        private void PanControl(Control ctrlToPan, MouseEventArgs e)
        {
            // Pan functions
            if (zoomFactor[zoomIndex] != 0)
            {
                Point mousePosNow = e.Location;

                int deltaX = mousePosNow.X - mousePosPan.X;
                int deltaY = mousePosNow.Y - mousePosPan.Y;

                int newX = ctrlToPan.Location.X + deltaX;
                int newY = ctrlToPan.Location.Y + deltaY;

                ctrlToPan.Location = new Point(newX, newY);
            }
        }

        // reset the size of the map to the default
        private void ControlReset(Control ctrlToReset)
        {
            ctrlToReset.Height = chartMap.Parent.Height;
            ctrlToReset.Width = chartMap.Parent.Width;
            var Origin = new Point(chartMap.Parent.Location.X, chartMap.Parent.Location.Y);
            ctrlToReset.Location = Origin;
        }

        // Center the mouse cursor over a control.
        private void CenterMouseOverControl(Control centerToCtrl)
        {
            // calculate the center of the control
            Point target = new Point(
                (centerToCtrl.Left + centerToCtrl.Right) / 2,
                (centerToCtrl.Top + centerToCtrl.Bottom) / 2);

            // convert to screen coordinates
            Point screen_coords = centerToCtrl.Parent.PointToScreen(target);

            // move the cursor to the center of the control
            Cursor.Position = screen_coords;
        }

        // Center control to parent (it does not resize it, just reposition)
        private void CenterControlToParent(Control ctrlToParentCenter)
        {
            ctrlToParentCenter.Left = ctrlToParentCenter.Parent.Left;
            ctrlToParentCenter.Top = ctrlToParentCenter.Parent.Top;

            double offsetX = (ctrlToParentCenter.Width - ctrlToParentCenter.Parent.Width) / 2;
            double offsetY = (ctrlToParentCenter.Height - ctrlToParentCenter.Parent.Height) / 2;

            ctrlToParentCenter.Left = ctrlToParentCenter.Left - (int)offsetX;
            ctrlToParentCenter.Top = ctrlToParentCenter.Top - (int)offsetY;
        }

        // on load
        private void UserControlMap_Load(object sender, EventArgs e)
        {
            ControlReset(chartMap);
        }

        // on resize the panel, reset the map to the default size
        private void UserControlMap_Resize(object sender, EventArgs e)
        {
            ControlReset(chartMap);
        }

        #endregion

        #region contextmenu

        // reset
        private void toolStripMenuReset_Click(object sender, EventArgs e)
        {
            ControlReset(chartMap);
            Refresh();
        }

        private void toolStripMenuZoomCenterMap_Click(object sender, EventArgs e)
        {
            CenterControlToParent(chartMap);
        }

        // zoom factor
        // 1:1
        private void toolStripMenuZoom1_Click(object sender, EventArgs e)
        {
            CenterControlToParent(chartMap);
            zoomFactor[zoomIndex] = 0;
            // resize to the minimum width and height
            chartMap.Width = chartMap.Parent.Width;
            chartMap.Height = chartMap.Parent.Height;

            // position the chart in the center of the panel
            var chartNoZoom = new Point(chartMap.Parent.Left, chartMap.Parent.Top);
            chartMap.Left = chartNoZoom.X;
            chartMap.Top = chartNoZoom.Y;
        }

        // 1.25:1
        private void toolStripMenuZoom125_Click(object sender, EventArgs e)
        {
            ControlReset(chartMap);
            ZoomToFactor(chartMap, 1.25);
            CenterControlToParent(chartMap);
        }

        // 1.5:1
        private void toolStripMenuZoom15_Click(object sender, EventArgs e)
        {
            ControlReset(chartMap);
            ZoomToFactor(chartMap, 1.5);
            CenterControlToParent(chartMap);
        }

        // 1.75:1
        private void toolStripMenuZoom175_Click(object sender, EventArgs e)
        {
            ControlReset(chartMap);
            ZoomToFactor(chartMap, 1.75);
            CenterControlToParent(chartMap);
        }

        // 2:1
        private void toolStripMenuZoom2_Click(object sender, EventArgs e)
        {
            ControlReset(chartMap);
            ZoomToFactor(chartMap, 2);
            CenterControlToParent(chartMap);
        }

        // 2.5:1
        private void toolStripMenuZoom25_Click(object sender, EventArgs e)
        {
            ControlReset(chartMap);
            ZoomToFactor(chartMap, 2.5);
            CenterControlToParent(chartMap);
        }

        // 3:1
        private void toolStripMenuZoom3_Click(object sender, EventArgs e)
        {
            ControlReset(chartMap);
            ZoomToFactor(chartMap, 3);
            CenterControlToParent(chartMap);
        }

        // 3.5:1
        private void toolStripMenuZoom35_Click(object sender, EventArgs e)
        {
            ControlReset(chartMap);
            ZoomToFactor(chartMap, 3.5);
            CenterControlToParent(chartMap);
        }

        // 4:1
        private void toolStripMenuZoom4_Click(object sender, EventArgs e)
        {
            ZoomToFactor(chartMap, 4);
            CenterControlToParent(chartMap);
        }

        // 5:1
        private void toolStripMenuZoom5_Click(object sender, EventArgs e)
        {
            ControlReset(chartMap);
            ZoomToFactor(chartMap, 5);
            CenterControlToParent(chartMap);
        }

        // 6:1
        private void toolStripMenuZoom6_Click(object sender, EventArgs e)
        {
            ControlReset(chartMap);
            ZoomToFactor(chartMap, 6);
            CenterControlToParent(chartMap);
        }

        // 8:1
        private void toolStripMenuZoom8_Click(object sender, EventArgs e)
        {
            ZoomToFactor(chartMap, 8);
            CenterControlToParent(chartMap);
        }

        // 10:1
        private void toolStripMenuZoom10_Click(object sender, EventArgs e)
        {
            ControlReset(chartMap);
            ZoomToFactor(chartMap, 10);
            CenterControlToParent(chartMap);
        }

        // About Map

        private void aboutToolStripAbout_Click(object sender, EventArgs e)
        {
            ExtendedControls.InfoForm frm = new ExtendedControls.InfoForm();
            frm.Info("Map Help", FindForm().Icon, EDDiscovery.Properties.Resources.mapuc, new int[1] { 300 });
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.Show(FindForm());
        }

        #endregion

        private void chartMap_MouseEnter(object sender, EventArgs e)
        {
            panSwitch = true;
        }

        private void chartMap_MouseLeave(object sender, EventArgs e)
        {
            panSwitch = false;
        }

        private void background_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }
    }
}

