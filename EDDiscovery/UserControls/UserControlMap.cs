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
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EDDiscovery.Forms;
using System.Collections.Concurrent;
using System.Threading;
using EliteDangerousCore;
using EliteDangerousCore.EDSM;
using EliteDangerousCore.DB;
using System.Diagnostics;


namespace EDDiscovery.UserControls
{
    public partial class UserControlMap : UserControlCommonBase
    {
        #region init

        private string DbSave { get { return "StarDistancePanel" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        private UserControlStarDistance.StarDistanceComputer computer;

        public UserControlMap()
        {
            InitializeComponent();
            this.chartMap.MouseWheel += Zoom_MouseWheel;            
        }

        private HistoryEntry last_he = null;
        private string pendingText = null;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double _MaxRadius = defaultMapMaxRadius;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double _MinRadius = defaultMapMinRadius;

        private const double defaultMapMaxRadius = 1000;
        private const double defaultMapMinRadius = 0;
        private int maxitems = 500;


        [DefaultValue(defaultMapMaxRadius)]
        private double MaxRadius
        {
            get { return _MaxRadius; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value) || value <= 0)
                    value = defaultMapMaxRadius;
                if (_MaxRadius != value)
                {
                    _MaxRadius = value;
                    if (last_he != null || uctg != null)
                        KickComputation(last_he ?? uctg.GetCurrentHistoryEntry);
                }
                // Don't adjust text in a focused textbox while a user is typing.
                if (textMaxRadius.ContainsFocus)
                    pendingText = $"{value:0.00}";
                else
                    textMaxRadius.Text = $"{value:0.00}";
            }
        }
        [DefaultValue(defaultMapMinRadius)]
        private double MinRadius
        {
            get { return _MinRadius; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value) || value < 0)
                    value = defaultMapMinRadius;
                if (_MinRadius != value)
                {
                    _MinRadius = value;
                    if (last_he != null || uctg != null)
                        KickComputation(last_he ?? uctg.GetCurrentHistoryEntry);
                }
                // Don't adjust text in a focused textbox while a user is typing.
                if (textMinRadius.ContainsFocus)
                    pendingText = $"{value:0.00}";
                else
                    textMinRadius.Text = $"{value:0.00}";
            }
        }
        
        public override void Init()
        {
            computer = new UserControlStarDistance.StarDistanceComputer();

            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;

            textMinRadius.Text = SQLiteConnectionUser.GetSettingDouble(DbSave + "MapMin", 0).ToStringInvariant();
            textMaxRadius.Text = SQLiteConnectionUser.GetSettingDouble(DbSave + "MapMax", defaultMapMaxRadius).ToStringInvariant();
            maxitems = SQLiteConnectionUser.GetSettingInt(DbSave + "MapMaxItems", maxitems);
            slideMaxItems.Value = SQLiteConnectionUser.GetSettingInt(DbSave + "MapMaxItems", maxitems);
            MaxRadius = float.Parse(textMaxRadius.Text);
            MinRadius = float.Parse(textMinRadius.Text);
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {            
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;

            RefreshMap();
            ControlReset(chartMap);
        }

        public override void Closing()
        {
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            computer.ShutDown();
            SQLiteConnectionUser.PutSettingDouble(DbSave + "MapMin", textMinRadius.Text.InvariantParseDouble(0));
            SQLiteConnectionUser.PutSettingDouble(DbSave + "MapMax", textMaxRadius.Text.InvariantParseDouble(defaultMapMaxRadius));
            SQLiteConnectionUser.PutSettingInt(DbSave + "MapMaxItems", maxitems);
        }

        public override void InitialDisplay()
        {
            KickComputation(uctg.GetCurrentHistoryEntry);            
        }

        private void Uctg_OnTravelSelectionChanged(HistoryEntry he, HistoryList hl)
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
                    (s, d) => BeginInvoke((MethodInvoker)delegate { NewStarListComputed(s, d); }),
                    maxitems, MinRadius, MaxRadius , true);
            }
        }
        private void NewStarListComputed(ISystem sys, BaseUtils.SortedListDoubleDuplicate<ISystem> list)      // In UI
        {
            double? max = textMaxRadius.Text.InvariantParseDoubleNull();
            if (max != null)
                MaxRadius = float.Parse(textMaxRadius.Text);

            double? min = textMinRadius.Text.InvariantParseDoubleNull();
            if (min != null)
                MinRadius = float.Parse(textMinRadius.Text);

            System.Diagnostics.Debug.Assert(Application.MessageLoop);       // check!
            discoveryform.history.CalculateSqDistances(list, sys.x, sys.y, sys.z, maxitems, MinRadius, MaxRadius , true);
            FillMap(list, sys);
        }             

        private void FillMap(SortedList<double, ISystem> csl, ISystem centerSystem)
        {   
            SetControlText("3D Map of closest systems from " + centerSystem.name);

            // Add the current system
            chartMap.Series[50].Points.AddXY(0, 0); 
            chartMap.Series[50].ToolTip = centerSystem.name; // tooltip

            if (csl.Count() > 0)
            {
                foreach (KeyValuePair<double, ISystem> tvp in csl)
                {
                    if (tvp.Value != centerSystem)
                    { 
                    var theISystemInQuestion = tvp.Value;
                    var sysX = theISystemInQuestion.x;
                    var sysY = theISystemInQuestion.y;
                    var sysZ = theISystemInQuestion.z;
                    var distFromCurrentSys = Math.Round(Math.Sqrt(tvp.Key), 2, MidpointRounding.AwayFromZero);
                    var curX = centerSystem.x;
                    var curY = centerSystem.y;
                    var curZ = centerSystem.z;

                        // reset charts axis
                        chartMap.ChartAreas[0].AxisY.IsStartedFromZero = false;
                        chartMap.ChartAreas[0].AxisX.IsStartedFromZero = false;                        
                        chartMap.ChartAreas[0].AxisX.Maximum = MaxRadius;
                        chartMap.ChartAreas[0].AxisX.Minimum = MaxRadius * -1;
                        chartMap.ChartAreas[0].AxisY.Maximum = MaxRadius;
                        chartMap.ChartAreas[0].AxisY.Minimum = MaxRadius * -1;

                        // depth of the series layers need to be adjusted, so to follow the X and Y axis
                        int sdepth = (Convert.ToInt32((MaxRadius) * 2));

                        // maximum allowed depth in chart is 1000%
                        if (sdepth > 1000)
                        {
                            sdepth = 1000;
                        }

                        chartMap.ChartAreas[0].Area3DStyle.PointDepth = sdepth;
                        
                        if (distFromCurrentSys > MinRadius) // we want to be able to define a shell 
                        {
                            int visits = discoveryform.history.GetVisitsCount(tvp.Value.name, tvp.Value.id_edsm);
                                                        
                            // calculate distance for each axis; this provide a cubical distribution
                            double dx = curX - sysX;
                            double dy = curY - sysY;
                            double dz = curZ - sysZ;

                            int px = Convert.ToInt32(dx);
                            int py = Convert.ToInt32(dy) * -1;
                            int pz = Convert.ToInt32(dz);

                            int nseries = 101; // # of series in the 3d plot (0 to 49, before the current system; 50, our current system; 51 to 101, after the current system)
                            double ratio = ((MaxRadius * 2) / nseries);
                            double spy = ((py + Convert.ToInt32(MaxRadius)) / ratio); // steps (series) to Y coordinate

                            int ispy = Convert.ToInt32(spy);

                            // we don't want a very close system to be showed as current one, so if it share the same serie of the current system we put in right before or after.
                            if (ispy > 50 && ispy < 51)
                            {
                                ispy = 51;
                            }
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
                            label.Append(theISystemInQuestion.name + " / " + visits + " visits" + "\n" + distFromCurrentSys);

                            chartMap.Series[ispy].Points.AddXY(px, pz);
                            chartMap.Series[ispy].ToolTip = label.ToString(); // tooltips
                        }
                    }
                }
            }
        }

        #endregion

        #region refresh

        private void TextMinRadius_TextChanged(object sender, EventArgs e)
        {
            // Don't let others directly assigning to textMinRadius.Text result in parsing.
            if (textMinRadius.ContainsFocus)
            {
                double? min = textMinRadius.Text.InvariantParseDoubleNull();
                MinRadius = min ?? defaultMapMinRadius;                
            }
            RefreshMap();
        }

        private void textMinRadius_Leave(object sender, EventArgs e)
        {
            if (pendingText != null)
                textMinRadius.Text = pendingText;
            pendingText = null;            
        }

        private void TextMaxRadius_TextChanged(object sender, EventArgs e)
        {
            // Don't let others directly assigning to textMaxRadius.Text result in parsing.
            if (textMaxRadius.ContainsFocus)
            {
                double? max = textMaxRadius.Text.InvariantParseDoubleNull();
                MaxRadius = max ?? defaultMapMaxRadius;                
            }
            RefreshMap();
        }

        private void textMaxRadius_Leave(object sender, EventArgs e)
        {
            if (pendingText != null)
                textMaxRadius.Text = pendingText;
            pendingText = null;            
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

        // slide 
        //
        // wait while move the slide, than refresh the chart...
        private void Wait(int ms)
        {
            DateTime start = DateTime.Now;
            while ((DateTime.Now - start).TotalMilliseconds < ms)
                Application.DoEvents();
        }

        private void SlideMaxItems_Scroll(object sender, EventArgs e)
        {
            toolTip.SetToolTip(slideMaxItems, slideMaxItems.Value.ToString());

            maxitems = slideMaxItems.Value;

            Wait(500);
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
                    zoomIndex++;

                ZoomControl(chartMap, zoomIndex, e);
            }

            // Zoom Out
            else if (e.Delta < 0)
            {
                if (zoomIndex > 0)
                    zoomIndex--;

                ZoomControl(chartMap, zoomIndex, e);
            }
        }

        private void ZoomControl(Control ctrlToZoom, int zoomIndex, MouseEventArgs e)
        {
            // More than 1:1
            if (zoomFactor[zoomIndex] != 0)
            {
                // get the current position of the chart
                var chartZoomIn = PointToScreen(new Point(chartMap.Left, chartMap.Top));

                // get the mouse position
                var Mouse = PointToScreen(new Point(e.X, e.Y));

                // multiply the chart's size to the zoom factor 
                ctrlToZoom.Width = Convert.ToInt32(ctrlToZoom.Parent.Width * zoomFactor[zoomIndex]);
                ctrlToZoom.Height = Convert.ToInt32(ctrlToZoom.Parent.Height * zoomFactor[zoomIndex]);

                // calculate the new center
                var Center = new Point(Convert.ToInt32(ctrlToZoom.Parent.Width / 2), Convert.ToInt32(ctrlToZoom.Parent.Height / 2));

                // calculate the offset
                var Offset = new Point(Convert.ToInt32(Mouse.X - Center.X), Convert.ToInt32((Mouse.Y - Center.Y)));

                // calculate the new position of the chart, offsetting it's center to the mouse coordinates
                var NewPosition = new Point(chartZoomIn.X - Offset.X, chartZoomIn.Y - Offset.Y);

                ctrlToZoom.Left = NewPosition.X;
                ctrlToZoom.Top = NewPosition.Y;
            }

            // 1:1
            if (zoomFactor[zoomIndex] == 0)
            {
                // resize to the minimum width and height
                chartMap.Width = chartMap.Parent.Width;
                chartMap.Height = chartMap.Parent.Height;

                // position the chart in the center of the panel
                var chartNoZoom = new Point(chartMap.Parent.Left, chartMap.Parent.Top);
                chartMap.Left = chartNoZoom.X;
                chartMap.Top = chartNoZoom.Y;
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
        private Int32 xr = 0;
        private Int32 yr = 0;
        private Int32 prevxr;
        private Int32 prevyr;

        private void ChartMap_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Smooth out the rotation of the chart
                Cursor.Hide(); // hide the cursor
                xr = Cursor.Position.X; // record the initial mouse position
                yr = Cursor.Position.Y; // 

                if (prevxr.ToString() != null || prevyr.ToString() != null)
                {
                    Cursor.Position = new Point(prevxr, prevyr);
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

        private void UserControlMap_MouseClick(object sender, MouseEventArgs e)
        {
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
                CenterMouseOverControl(chartMap);
                Cursor.Show(); // show the cursor                
            }
            if (e.Button == MouseButtons.Middle)
            {
                // return to the default cursor
                Cursor.Current = Cursors.Default;
            }
        }

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
            if (e.Button == MouseButtons.Middle)
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

        #endregion
        
        public void AboutMapPanel(Form parent = null)
        {
            AboutMapPanel frm = new AboutMapPanel();
            frm.Show();
        }
                

        private void aboutToolStripAbout_Click(object sender, EventArgs e)
        {            
            AboutMapPanel();        
        }
    }
}

