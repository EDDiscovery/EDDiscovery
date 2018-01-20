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


namespace EDDiscovery.UserControls
{
    public partial class UserControlMap : UserControlCommonBase
    {
        private string DbSave { get { return "StarDistancePanel" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        private UserControlStarDistance.StarDistanceComputer computer;

        public UserControlMap()
        {
            InitializeComponent();
            this.chartMap.MouseWheel += Zoom_MouseWheel;
        }

        const double defaultMaximumMapRadius = 100;

        int maxitems = 500;

        double MaxRadius = 100;
        double MinRadius = 0;
        

        public override void Init()
        {
            computer = new UserControlStarDistance.StarDistanceComputer();

            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;

            textMinRadius.Text = SQLiteConnectionUser.GetSettingDouble(DbSave + "MapMin", 0).ToStringInvariant();
            textMaxRadius.Text = SQLiteConnectionUser.GetSettingDouble(DbSave + "MapMax", defaultMaximumMapRadius).ToStringInvariant();
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
            ControlResize(chartMap);
        }

        public override void Closing()
        {
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            computer.ShutDown();
            SQLiteConnectionUser.PutSettingDouble(DbSave + "MapMin", textMinRadius.Text.InvariantParseDouble(0));
            SQLiteConnectionUser.PutSettingDouble(DbSave + "MapMax", textMaxRadius.Text.InvariantParseDouble(defaultMaximumMapRadius));
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
            ControlResize(chartMap);
        }

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
        private void NewStarListComputed(ISystem sys, SortedList<double, ISystem> list)      // In UI
        {
            double? max = textMaxRadius.Text.InvariantParseDoubleNull();
            if (max != null)
                MaxRadius = float.Parse(textMaxRadius.Text);

            double? min = textMinRadius.Text.InvariantParseDoubleNull();
            if (min != null)
                MinRadius = float.Parse(textMinRadius.Text);

            System.Diagnostics.Debug.Assert(Application.MessageLoop);       // check!
            discoveryform.history.CalculateSqDistances(list, sys.x, sys.y, sys.z, maxitems, MinRadius, MaxRadius , true);
            FillRadar(list, sys);
        }
        
        private void FillRadar(SortedList<double, ISystem> csl, ISystem centerSystem)
        {   
            SetControlText("3D Map of closest systems from " + centerSystem.name);

            // Add the current system
            chartMap.Series[50].Points.AddXY(0, 0); 
            chartMap.Series[50].ToolTip = centerSystem.name;

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

                            StringBuilder label = new StringBuilder();
                            label.Append(theISystemInQuestion.name + " / " + visits + " visits" + "\n" + distFromCurrentSys);

                            // calculate distance for each axis; this provide a cubical distribution
                            double dx = curX - sysX;
                            double dy = curY - sysY;
                            double dz = curZ - sysZ;

                            int px = Convert.ToInt32(dx);
                            int py = Convert.ToInt32(dy) * -1;
                            int pz = Convert.ToInt32(dz);

                            int nseries = 101; // # of series in the 3d plot
                            double ratio = ((MaxRadius * 2) / nseries);
                            double spy = ((py + Convert.ToInt32(MaxRadius)) / ratio); // step of the point y coordinate

                            int ispy = Convert.ToInt32(spy);

                            // we don't want a close system to be marked as the current one, so we put in right before or after.
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
                                                        
                            chartMap.Series[ispy].Points.AddXY(px, pz);
                            chartMap.Series[ispy].ToolTip = label.ToString();
                        }
                    }
                }
            }
        }

        #endregion

        #region refresh

        private void TextMinRadius_TextChanged(object sender, EventArgs e)
        {
            double? min = textMinRadius.Text.InvariantParseDoubleNull();
            if (min != null)
            MinRadius = float.Parse(textMinRadius.Text);

            KickComputation(uctg.GetCurrentHistoryEntry);

            RefreshMap();
        }

        private void TextMaxRadius_TextChanged(object sender, EventArgs e)
        {
            double? max = textMaxRadius.Text.InvariantParseDoubleNull();
            if (max != null)
            MaxRadius = float.Parse(textMaxRadius.Text);

            KickComputation(uctg.GetCurrentHistoryEntry);

            RefreshMap();            
        }
                
        private void RefreshMap()
        {
            foreach (int s in Enumerable.Range(0, 100))
            {
                chartMap.Series[s].Points.Clear();
            }

            chartMap.Update();            
        }

        #endregion
                
        private Point mousePos;
                
        private void ChartMap_MouseMove(object sender, MouseEventArgs e)
        {
            // rotate the map with the firt mouse button

            if (e.Button == MouseButtons.Left)
            {
                // rotate the chart            
                if (!mousePos.IsEmpty)
                {
                    var style = chartMap.ChartAreas[0].Area3DStyle;
                    style.Rotation = Math.Min(180, Math.Max(-180,
                        style.Rotation - (e.Location.X - mousePos.X)));
                    style.Inclination = Math.Min(90, Math.Max(-90,
                        style.Inclination + (e.Location.Y - mousePos.Y)));
                }
                mousePos = e.Location;
            }

            // pan the chart with the middle mouse buttom
            if (e.Button == MouseButtons.Middle)
            {
                if (!mousePos.IsEmpty)
                {
                    /*var lastPoint = new Point();
                    var point = new Point(e.Location.X, e.Location.Y);
                    point.X = (point.X - (chartMap.Width / 2) - lastPoint.X);
                    point.Y = (point.Y - (chartMap.Height / 2) - lastPoint.Y);
                    chartMap.Location = point;
                    lastPoint.X = e.Location.X;
                    lastPoint.Y = e.Location.Y;*/
                }

                mousePos = e.Location;
            }
        }

        // zoom the chart
        private void Zoom_MouseWheel(object sender, MouseEventArgs e)
        {
            int minWidth = chartMap.Parent.Width;
            int minHeight = chartMap.Parent.Height;

            if (e.Delta > 0)
            {
                // oversize the chart while mantain it in the center
                ZoomInControl(chartMap);
            }
            else if (e.Delta < 0)
            {
                // shrink the chart size
                ZoomOutControl(chartMap);
            }
        }

        private void ZoomInControl(Control ctrlToZoom)
        {
            ctrlToZoom.Height = ctrlToZoom.Height + 100;
            ctrlToZoom.Width = ctrlToZoom.Width + 100;
            var point = new Point(ctrlToZoom.Location.X - 50, ctrlToZoom.Location.Y - 50);
            ctrlToZoom.Location = point;
        }

        private void ZoomOutControl(Control ctrlToZoom)
        {
            if (ctrlToZoom.Height >= ctrlToZoom.Parent.Height)
            {
                ctrlToZoom.Height = ctrlToZoom.Height - 100;
                ctrlToZoom.Width = ctrlToZoom.Width - 100;
                var point = new Point(ctrlToZoom.Location.X + 50, ctrlToZoom.Location.Y + 50);
                ctrlToZoom.Location = point;
            }
        }

        private void ControlResize(Control ctrlResize)
        {
            ctrlResize.Height = chartMap.Parent.Height;
            ctrlResize.Width = chartMap.Parent.Width;
            var point = new Point(chartMap.Parent.Location.X, chartMap.Parent.Location.Y);
            ctrlResize.Location = point;
        }
                
        // coordinates for rotation mouse reposition computation
        public Int32 xr = 0;
        public Int32 yr = 0;
        public Int32 prevxr;
        public Int32 prevyr;

        // coordinates for zooming center to current mouse position
        public Int32 xz = 0;
        public Int32 yz = 0;
        public Int32 lastxz;
        public Int32 lastyz;

        // coordinates to smooth the movement of the chart pan
        public Int32 xp = 0;
        public Int32 yp = 0;
        public Int32 lastxp;
        public Int32 lastyp;

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
            }
        }

        // Center the mouse over a control.
        private void CenterMouseOverControl(Control centerToCtrl)
        {
            // calculate the center of the control
            Point target = new Point(
                (centerToCtrl.Left + centerToCtrl.Right) / 2,
                (centerToCtrl.Top + centerToCtrl.Bottom) / 2);

            // convert to screen coordinates
            Point screen_coords = centerToCtrl.Parent.PointToScreen(target);

            // move the cursor to the center of teh control
            Cursor.Position = screen_coords;
        }        

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

        private void UserControlMap_Load(object sender, EventArgs e)
        {
            ControlResize(chartMap);
        }

        private void UserControlMap_Resize(object sender, EventArgs e)
        {
            ControlResize(chartMap);
        }
    }
}
