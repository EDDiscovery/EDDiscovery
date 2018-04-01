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
using ExtendedControls;
using EliteDangerousCore.JournalEvents;

namespace EDDiscovery.UserControls
{
    public partial class UserControlPlot : UserControlCommonBase
    {
        private string DbSave { get { return "PlotPanel" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        private StarDistanceComputer computer;

        public UserControlPlot()
        {
            InitializeComponent();
            this.chartBubble.MouseWheel += Zoom_MouseWheel;
                        
            SetMarkerSize();
        }

        const double defaultmaximumradarradius = 50;
        const int maxitems = 500;

        public string currentSystemName = "";
        public string previousSystemName = "";
        public double prevX = 0.0;
        public double prevY = 0.0;
        public double prevZ = 0.0;

        public override void Init()
        {
            computer = new StarDistanceComputer();

            textMinRadius.ValueNoChange = SQLiteConnectionUser.GetSettingDouble(DbSave + "PlotMin", 0);
            textMaxRadius.ValueNoChange = SQLiteConnectionUser.GetSettingDouble(DbSave + "PlotMax", defaultmaximumradarradius);
            textMinRadius.SetComparitor(textMaxRadius, -2);     // need to do this after values are set
            textMaxRadius.SetComparitor(textMinRadius, 2);

            comboBoxView.Enabled = false;
            comboBoxView.Items.Add("Top");
            comboBoxView.Items.Add("Front");
            comboBoxView.Items.Add("Side");
            comboBoxView.Items.DefaultIfEmpty("Top");
            comboBoxView.SelectedItem = SQLiteConnectionUser.GetSettingString(DbSave + "PlotOrientation", "Top");
            comboBoxView.Enabled = true;
            
            checkBoxDotSize.Checked = SQLiteConnectionUser.GetSettingBool(DbSave + "PlotDepth", true);

            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {            
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;

            //refreshRadar();
        }

        public override void Closing()
        {
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            computer.ShutDown();
            SQLiteConnectionUser.PutSettingDouble(DbSave + "PlotMin", textMinRadius.Value);
            SQLiteConnectionUser.PutSettingDouble(DbSave + "PlotMax", textMaxRadius.Value);
            SQLiteConnectionUser.PutSettingString(DbSave + "PlotOrientation", comboBoxView.SelectedItem.ToString());
            SQLiteConnectionUser.PutSettingBool(DbSave + "PlotDepth", checkBoxDotSize.Checked);
        }

        public override void InitialDisplay()
        {
            KickComputation(uctg.GetCurrentHistoryEntry);           
        }              

        private void Uctg_OnTravelSelectionChanged(HistoryEntry he, HistoryList hl)
        {
            KickComputation(he);

            // Previous system
            currentSystemName = he.System.Name;            
            GetPreviousSystemInHistory(hl);

            refreshRadar();
            SetChartSize(chartBubble, 1);
        }

        private void GetPreviousSystemInHistory(HistoryList hl)
        {
            HistoryEntry prev_he = hl.GetLastHistoryEntry(he => he.System.Name != currentSystemName);
            previousSystemName = prev_he.System.Name;
            prevX = prev_he.System.X;
            prevY = prev_he.System.Y;
            prevZ = prev_he.System.Z;
        }

        private void KickComputation(HistoryEntry he)
        {
            var x = this.Handle; // workaround to avoid throw an exception if open the panel as popup

            if (he?.System != null && he.System.HasCoordinate)
            {
                computer.CalculateClosestSystems(he.System,
                    (s, d) => BeginInvoke((MethodInvoker)delegate { NewStarListComputed(s, d); }),
                    maxitems, textMinRadius.Value, textMaxRadius.Value, true);
            }
        }
        private void NewStarListComputed(ISystem sys, BaseUtils.SortedListDoubleDuplicate<ISystem>  list)      // In UI
        {
            System.Diagnostics.Debug.Assert(Application.MessageLoop);       // check!
            discoveryform.history.CalculateSqDistances(list, sys.X, sys.Y, sys.Z, maxitems, textMinRadius.Value, textMaxRadius.Value, true);
            FillPlot(list, sys);
        }
        
        private void FillPlot(BaseUtils.SortedListDoubleDuplicate<ISystem> csl, ISystem currentSystem)
        {
            SetControlText("");

            if (csl.Count() > 0)
            {
                SetControlText("2D Plot of systems in range from " + currentSystem.Name);                

                // position the current system in the center of the references coordinates
                chartBubble.Series[0].Points.AddXY(0, 0, 4);
                chartBubble.Series[0].ToolTip = currentSystem.Name;
                chartBubble.Series[3].Points.AddXY(0, 0, 4);
                chartBubble.Series[3].ToolTip = currentSystem.Name;
                chartBubble.Series[6].Points.AddXY(0, 0, 4);
                chartBubble.Series[6].ToolTip = currentSystem.Name;
                
                // create a point for each system in range
                foreach (KeyValuePair<double, ISystem> tvp in csl)
                {
                    var inRangeSystem = tvp.Value;

                    if (tvp.Value.Name != currentSystem.Name && tvp.Value.Name != previousSystemName)
                    { 
                        // get the coordinates of each system in range
                        var sysX = inRangeSystem.X;
                        var sysY = inRangeSystem.Y;
                        var sysZ = inRangeSystem.Z;
                        
                        // get the coordinates of the current system, to properly calculate the distance
                        var curX = currentSystem.X;
                        var curY = currentSystem.Y;
                        var curZ = currentSystem.Z;

                        // calculate the distance
                        var distFromCurrentSys = Math.Round(Math.Sqrt(tvp.Key), 2, MidpointRounding.AwayFromZero);

                        // reset charts axis
                        chartBubble.ChartAreas[0].AxisY.IsStartedFromZero = false;
                        chartBubble.ChartAreas[1].AxisY.IsStartedFromZero = false;
                        chartBubble.ChartAreas[2].AxisY.IsStartedFromZero = false;
                        chartBubble.ChartAreas[0].AxisX.IsStartedFromZero = false;
                        chartBubble.ChartAreas[1].AxisX.IsStartedFromZero = false;
                        chartBubble.ChartAreas[2].AxisX.IsStartedFromZero = false;

                        // for a spherical distribution, do not count distances bigger than the selected radius
                        if (distFromCurrentSys > textMinRadius.Value)
                        {
                            // count the total visits for each system
                            int visits = discoveryform.history.GetVisitsCount(tvp.Value.Name, tvp.Value.EDSMID);

                            // create the label for the tooltip
                            StringBuilder label = new StringBuilder();
                            label.Append(inRangeSystem.Name + " / " + visits + " visits" + "\n" + distFromCurrentSys);

                            // calculate the reference for each coordinate
                            double dx = curX - sysX;
                            double dy = curY - sysY;
                            double dz = curZ - sysZ;

                            // prepare the value to be displayed by the plot and fix the orientation
                            int px = Convert.ToInt32(dx) * -1;
                            int py = Convert.ToInt32(dy) * -1;
                            int pz = Convert.ToInt32(dz) * -1;

                            // visited systems go to series #1, #4 and #7; unvisited to series #2, #5 and #8. 
                            // series #0, #3 and #6 is for the current system...

                            if (visits > 0)
                            {
                                // Top view
                                chartBubble.Series[1].Points.AddXY(px, py, pz);
                                chartBubble.Series[1].ToolTip = label.ToString();

                                // Front view
                                chartBubble.Series[4].Points.AddXY(px, pz, py);
                                chartBubble.Series[4].ToolTip = label.ToString();

                                // Side view
                                chartBubble.Series[7].Points.AddXY(py, pz, px);
                                chartBubble.Series[7].ToolTip = label.ToString();                                
                            }
                            else
                            {
                                // Top view
                                chartBubble.Series[2].Points.AddXY(px, py, pz);
                                chartBubble.Series[2].ToolTip = label.ToString();

                                // Front view
                                chartBubble.Series[5].Points.AddXY(px, pz, py);
                                chartBubble.Series[5].ToolTip = label.ToString();
                                 
                                // Side view
                                chartBubble.Series[8].Points.AddXY(py, pz, px);
                                chartBubble.Series[8].ToolTip = label.ToString();                                                                
                            }
                        }
                    }

                    if (tvp.Value.Name != currentSystem.Name && tvp.Value.Name == previousSystemName)
                    {
                        // Previous system coordinates, distances and label
                        int prevx = Convert.ToInt32(currentSystem.X - prevX) * -1;
                        int prevy = Convert.ToInt32(currentSystem.Y - prevY) * -1;
                        int prevz = Convert.ToInt32(currentSystem.Z - prevZ) * -1;

                        int visits = discoveryform.history.GetVisitsCount(tvp.Value.Name, tvp.Value.EDSMID);
                        var distFromCurrentSys = Math.Round(Math.Sqrt(tvp.Key), 2, MidpointRounding.AwayFromZero);

                        StringBuilder label = new StringBuilder();
                        label.Append(previousSystemName + " / " + visits + " visits" + "\n" + distFromCurrentSys);

                        // Top view
                        chartBubble.Series[9].Points.AddXY(prevx, prevy, prevz);
                        chartBubble.Series[9].ToolTip = label.ToString();

                        // Front view
                        chartBubble.Series[10].Points.AddXY(prevx, prevz, prevy);
                        chartBubble.Series[10].ToolTip = label.ToString();

                        // Side view
                        chartBubble.Series[11].Points.AddXY(prevy, prevz, prevx);
                        chartBubble.Series[11].ToolTip = label.ToString();
                    }                    
                }
            }
        }

        // Zoom
        
        // zoom with the mouse scroll wheel
        private double[] zoomFactor = { 1.0, 1.25, 1.5, 1.75, 2.0, 2.5, 3.0, 3.5, 4.0 };
        private double[] markerReduction = { 1.0, 0.9, 0.8, 0.7, 0.6, 0.5, 0.4, 0.3, 0.2 }; // markes reduce in size when zoom in, for a clearer view
        private int zoomIndex = 0; // default zoom at 1:1

        private int[] seriesIsCurrent = { 0, 3, 6 };
        private int[] seriesIsVisited = { 1, 4, 7 };
        private int[] seriesUnVisited = { 2, 5, 8 };
        private int[] seriesIsPrevious = { 9, 10, 11 };

        private void SetMarkerSize()
        {
            int maxMarker = 2;
            int defMarker = 2;
            int minMarker = 2;

            if (checkBoxDotSize.Checked == true)
            {
                maxMarker = Convert.ToInt32(6 * (markerReduction[zoomIndex]));
                defMarker = Convert.ToInt32(4 * (markerReduction[zoomIndex]));

                int minMarkAbsolute = Convert.ToInt32(2 * (markerReduction[zoomIndex]));
                minMarker = minMarkAbsolute < 1 ? minMarker = 1 : minMarker = 2; // avoid zero values or less than 1 pixel marker when zooming
            }
            else
            {
                maxMarker = Convert.ToInt32(2 * (markerReduction[zoomIndex]));
                defMarker = Convert.ToInt32(2 * (markerReduction[zoomIndex]));

                int minMarkAbsolute = Convert.ToInt32(2 * (markerReduction[zoomIndex]));
                minMarker = minMarkAbsolute < 2 ? minMarker = 2 : minMarker = 2; // try to maintain all markers at the same size
            }

            // Min and Max size for Current system
            foreach (int serie in seriesIsCurrent)
            {
                chartBubble.Series[serie]["BubbleMaxSize"] = maxMarker.ToString();
                chartBubble.Series[serie]["MarkerSize"] = defMarker.ToString();
                chartBubble.Series[serie]["BubbleMinSize"] = minMarker.ToString();
            }
            // Min and Max size for Visited systems
            foreach (int serie in seriesIsVisited)
            {
                chartBubble.Series[serie]["BubbleMaxSize"] = maxMarker.ToString();
                chartBubble.Series[serie]["MarkerSize"] = defMarker.ToString();
                chartBubble.Series[serie]["BubbleMinSize"] = minMarker.ToString();
            }
            // Min and Max size for Unvisited systems
            foreach (int serie in seriesUnVisited)
            {
                chartBubble.Series[serie]["BubbleMaxSize"] = maxMarker.ToString();
                chartBubble.Series[serie]["MarkerSize"] = defMarker.ToString();
                chartBubble.Series[serie]["BubbleMinSize"] = minMarker.ToString();
            }
            // Min and Max size for Previous systems
            foreach (int serie in seriesIsPrevious)
            {
                chartBubble.Series[serie]["BubbleMaxSize"] = maxMarker.ToString();
                chartBubble.Series[serie]["MarkerSize"] = defMarker.ToString();
                chartBubble.Series[serie]["BubbleMinSize"] = minMarker.ToString();
            }
        }

        private void SetChartSize(Control ctrlToZoom, double zoomratio)
        {
            ctrlToZoom.Height = ctrlToZoom.Parent.Height;
            ctrlToZoom.Width = ctrlToZoom.Parent.Width;

            // multiply the chart's size to the zoom factor 
            if (zoomratio == 1.0) 
            { // reduce a bit the size of the chart for a better overall visualization
                ctrlToZoom.Width = Convert.ToInt32(ctrlToZoom.Parent.Width * zoomratio);
                ctrlToZoom.Height = (Convert.ToInt32(ctrlToZoom.Parent.Height * zoomratio) / 100) * 90;
            }
            else
            {
                ctrlToZoom.Width = Convert.ToInt32(ctrlToZoom.Parent.Width * zoomratio);
                ctrlToZoom.Height = Convert.ToInt32(ctrlToZoom.Parent.Height * zoomratio);
            }

            ctrlToZoom.Left = ctrlToZoom.Parent.Left;
            ctrlToZoom.Top = ctrlToZoom.Parent.Top;

            double offsetX = (ctrlToZoom.Width - ctrlToZoom.Parent.Width) / 2;
            double offsetY = (ctrlToZoom.Height - ctrlToZoom.Parent.Height) / 2;

            ctrlToZoom.Left = ctrlToZoom.Left - (int)offsetX;
            ctrlToZoom.Top = ctrlToZoom.Top - (int)offsetY;
        }

        private void Zoom_MouseWheel(object sender, MouseEventArgs e)
        {            
            // Zoom In
            if (e.Delta > 0)
            {
                if (zoomIndex < 8)
                    zoomIndex++;

                ZoomChart();                
            }

            // Zoom Out
            else if (e.Delta < 0)
            {
                if (zoomIndex > 0)
                    zoomIndex--;

                ZoomChart();
            }
        }

        private void ZoomChart()
        {
            if (zoomIndex > 0)
            {
                SetMarkerSize();
                SetChartSize(chartBubble, zoomFactor[zoomIndex]);
            }
            if (zoomIndex == 0)
            {
                SetChartSize(chartBubble, 1.0);
                SetMarkerSize();
            }
        }

        private Point mousePosPan;

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

        private void textMinRadius_ValueChanged(object sender, EventArgs e)
        {
           KickComputation(uctg.GetCurrentHistoryEntry);
            refreshRadar();
        }

        private void textMaxRadius_ValueChanged(object sender, EventArgs e)
        {
            KickComputation(uctg.GetCurrentHistoryEntry);
            refreshRadar();
        }
                
        private void refreshRadar()
        {
            for (int i = 0; i <= 11; i++)
            {
                chartBubble.Series[i].Points.Clear();
            }            
            chartBubble.Update();            
        }
        
        private bool panSwitch = false;

        private void chartBubble_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle && panSwitch == true)
            {                
                mousePosPan = e.Location;
            }

            if (e.Button == MouseButtons.Right)
            {

                //Point cursor = PointToScreen(new Point(e.Location.X, e.Location.Y));
                int cursorX = Cursor.Position.X;
                int cursorY = Cursor.Position.Y;
                contextMenuStrip.Show(cursorX, cursorY);
            }
        }

        private void chartBubble_MouseMove(object sender, MouseEventArgs e)
        {
            // pan the chart with the middle mouse buttom
            if (e.Button == MouseButtons.Middle)
            {
                PanControl(chartBubble, e);
            }
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetChartSize(chartBubble, 1);
            SetMarkerSize();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            zoomIndex = 0;
            SetChartSize(chartBubble, zoomIndex);
            SetMarkerSize();
        }

        private void toolStripMenuItem125_Click(object sender, EventArgs e)
        {
            zoomIndex = 1;
            SetChartSize(chartBubble, zoomIndex);
            SetMarkerSize();
        }                

        private void toolStripMenuItem15_Click(object sender, EventArgs e)
        {
            zoomIndex = 2;
            SetChartSize(chartBubble, zoomIndex);
            SetMarkerSize();
        }

        private void toolStripMenuItem175_Click(object sender, EventArgs e)
        {
            zoomIndex = 3;
            SetChartSize(chartBubble, zoomIndex);
            SetMarkerSize();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            zoomIndex = 4;
            SetChartSize(chartBubble, zoomIndex);
            SetMarkerSize();
        }

        private void toolStripMenuItem25_Click(object sender, EventArgs e)
        {
            zoomIndex = 5;
            SetChartSize(chartBubble, zoomIndex);
            SetMarkerSize();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            zoomIndex = 6;
            SetChartSize(chartBubble, zoomIndex);
            SetMarkerSize();
        }

        private void toolStripMenuItem35_Click(object sender, EventArgs e)
        {
            zoomIndex = 7;
            SetChartSize(chartBubble, zoomIndex);
            SetMarkerSize();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            zoomIndex = 8;
            SetChartSize(chartBubble, zoomIndex);
            SetMarkerSize();
        }

        private void chartBubble_MouseEnter(object sender, EventArgs e)
        {
            panSwitch = true;
        }

        private void chartBubble_MouseLeave(object sender, EventArgs e)
        {
            panSwitch = false;
        }

        private void comboBoxView_SelectedIndexChanged(object sender, EventArgs e)
        {
            string s = comboBoxView.SelectedItem.ToString();
            if ( s == "Top")
            {
                chartBubble.ChartAreas[0].Visible = true;
                chartBubble.ChartAreas[1].Visible = false;
                chartBubble.ChartAreas[2].Visible = false;
            }
            if (s == "Front")
            {
                chartBubble.ChartAreas[0].Visible = false;
                chartBubble.ChartAreas[1].Visible = true;
                chartBubble.ChartAreas[2].Visible = false;
            }
            if (s == "Side")
            {
                chartBubble.ChartAreas[0].Visible = false;
                chartBubble.ChartAreas[1].Visible = false;
                chartBubble.ChartAreas[2].Visible = true;
            }
        }

        private void background_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                //Point cursor = PointToScreen(new Point(e.Location.X, e.Location.Y));
                int cursorX = Cursor.Position.X;
                int cursorY = Cursor.Position.Y;
                contextMenuStrip.Show(cursorX, cursorY);
            }
        }

        private void UserControlPlot_Resize(object sender, EventArgs e)
        {
            SetChartSize(chartBubble, 1);
            SetMarkerSize();           
        }

        private void checkBoxDotSize_CheckedChanged(object sender, EventArgs e)
        {
            SetMarkerSize();
        }
    }    
}
