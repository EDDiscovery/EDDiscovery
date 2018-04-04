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
using System.IO;
using EliteDangerousCore;
using EliteDangerousCore.EDSM;
using EliteDangerousCore.DB;
using ExtendedControls;
using EliteDangerousCore.JournalEvents;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.Annotations;
using OxyPlot.WindowsForms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlPlot : UserControlCommonBase
    {
        private string DbSave { get { return "PlotPanel" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        private StarDistanceComputer computer;

        public UserControlPlot()
        {
            InitializeComponent();
            dataGridList.Visible = false;
            reportView.Visible = false;
        }

        const double defaultmaximumradarradius = 50;
        int maxitems = 500;

        public string currentSystemName = "";
        public string previousSystemName = "";
        public double prevX = 0.0;
        public double prevY = 0.0;
        public double prevZ = 0.0;

        public override void Init()
        {
            textMinRadius.ValueNoChange = SQLiteConnectionUser.GetSettingDouble(DbSave + "PlotMin", 0);
            textMaxRadius.ValueNoChange = SQLiteConnectionUser.GetSettingDouble(DbSave + "PlotMax", defaultmaximumradarradius);
            textMinRadius.SetComparitor(textMaxRadius, -2);     // need to do this after values are set
            textMaxRadius.SetComparitor(textMinRadius, 2);

            comboBoxView.Enabled = false;
            comboBoxView.Items.Add("Top");
            comboBoxView.Items.Add("Front");
            comboBoxView.Items.Add("Side");
            comboBoxView.Items.Add("Grid");
            comboBoxView.Items.Add("Report");
            comboBoxView.Items.DefaultIfEmpty("Top");
            comboBoxView.SelectedItem = SQLiteConnectionUser.GetSettingString(DbSave + "PlotOrientation", "Top");
            comboBoxView.Enabled = true;

            checkBoxDotSize.Checked = SQLiteConnectionUser.GetSettingBool(DbSave + "PlotDepth", true);
            checkBoxLegend.Checked = SQLiteConnectionUser.GetSettingBool(DbSave + "PlotLegend", true);

            

            computer = new StarDistanceComputer();
            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {            
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;
        }

        public override void Closing()
        {
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            computer.ShutDown();
            SQLiteConnectionUser.PutSettingDouble(DbSave + "PlotMin", textMinRadius.Value);
            SQLiteConnectionUser.PutSettingDouble(DbSave + "PlotMax", textMaxRadius.Value);
            SQLiteConnectionUser.PutSettingString(DbSave + "PlotOrientation", comboBoxView.SelectedItem.ToString());
            SQLiteConnectionUser.PutSettingBool(DbSave + "PlotDepth", checkBoxDotSize.Checked);
            SQLiteConnectionUser.PutSettingBool(DbSave + "PlotLegend", checkBoxLegend.Checked);
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
            dataGridList.Rows.Clear();
            reportView.Clear();

            var x = this.Handle; // workaround to avoid throw an exception if open the panel as pop up

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
            // debug
            //debugView.AppendText("Plot started.");

            SetControlText("2D Plot of systems in range from " + currentSystem.Name);

            // initializing the plot
            var model = new PlotModel { Title = "Plot around " + currentSystem.Name };
            this.plotView.Model = model;

            // series
            var currentSeries = new ScatterSeries { MarkerType = MarkerType.Circle, MarkerFill = OxyColors.Red, TrackerFormatString = currentSystemName.ToString() };
            var lastoneSeries = new ScatterSeries { MarkerType = MarkerType.Circle, MarkerFill = OxyColors.Purple };
            var visitedSeries = new ScatterSeries { MarkerType = MarkerType.Circle, MarkerFill = OxyColors.RoyalBlue };
            var inrangeSeries = new ScatterSeries { MarkerType = MarkerType.Circle, MarkerFill = OxyColors.Yellow };

            // adding the series to the plot
            model.Series.Add(inrangeSeries);
            model.Series.Add(visitedSeries);
            model.Series.Add(currentSeries);
            model.Series.Add(lastoneSeries);

            // axes
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
                        
            // position the current system in the center of the references coordinates
            currentSeries.Points.Add(new ScatterPoint(0, 0, 5));

            // get the coordinates of the current system, to properly calculate the distances
            var curX = currentSystem.X;
            var curY = currentSystem.Y;
            var curZ = currentSystem.Z;

            // Title of the report           
            reportView.AppendText("\nSystems around " + currentSystemName + ", between " + textMinRadius.Value.ToString()  + " and " + textMaxRadius.Value.ToString() + ": " + csl.Count.ToString() + "\n");
            
            if (csl.Count() > 0)
            {       
                // debug
                //debugView.AppendText("\n\nStart to iterate though the list.");
                
                // iterate through each system in the list
                foreach (KeyValuePair<double, ISystem> tvp in csl)
                {
                    // Created on date
                    //debugView.AppendText("\nDate: \nCreated on " + tvp.Value.CreateDate.ToString());
                    // Updated on date
                    //debugView.AppendText("\nUpdated on " + tvp.Value.UpdateDate.ToString());

                    // name it
                    var sysName = tvp.Value.Name;

                    // for each system that is not the current center system, do:
                    if (tvp.Value.Name != currentSystem.Name)
                    {                        
                        // get the coordinates of each system in range
                        var sysX = tvp.Value.X;
                        var sysY = tvp.Value.Y;
                        var sysZ = tvp.Value.Z;

                        // Information on each member of the list
                        reportView.AppendText("\n" + tvp.Value.Name.ToString());
                        reportView.AppendText("\nCoordinates: " + sysX + ", " + sysY + ", " + sysZ);

                        // Count the total visits for each system
                        int visits = discoveryform.history.GetVisitsCount(tvp.Value.Name, tvp.Value.EDSMID);
                                                
                        // calculate the distance
                        var distFromCurrentSys = Math.Round(Math.Sqrt(tvp.Key), 2, MidpointRounding.AwayFromZero);
                                                
                        // calculate the distance of each coordinate
                        double dx = curX - sysX;
                        double dy = curY - sysY;
                        double dz = curZ - sysZ;                        

                        // Populate the List with the systems in range
                        if (distFromCurrentSys >= textMinRadius.Value && distFromCurrentSys <= textMaxRadius.Value)
                        {                            
                            // Create the list, with each system's name, distances by x, y and z coordinates and number of visits
                            object[] rowobj = { tvp.Value.Name, $"{dx:0.00}", $"{dy:0.00}", $"{dz:0.00}", $"{visits:n0}" };
                            int rowindex = dataGridList.Rows.Add(rowobj);
                            dataGridList.Rows[rowindex].Tag = tvp.Value;

                            // Iterate through the list, and plot each system
                            foreach (object element in rowobj)
                            {
                                if (element != null)
                                {   
                                    if (tvp.Value.Name == previousSystemName)

                                    // Last visited system
                                    {
                                        // Previous system coordinates, distances and label
                                        dx = Convert.ToInt32(currentSystem.X - prevX) * -1;
                                        dy = Convert.ToInt32(currentSystem.Y - prevY) * -1;
                                        dz = Convert.ToInt32(currentSystem.Z - prevZ) * -1;
                                    }
                                    else
                                    {
                                        dx = Convert.ToInt32(dx) * -1;
                                        dy = Convert.ToInt32(dy) * -1;
                                        dz = Convert.ToInt32(dz) * -1;
                                    }

                                    var coord1 = 0.0;
                                    var coord2 = 0.0;
                                    var coord3 = 0.0;

                                    string s = comboBoxView.SelectedItem.ToString();
                                    if (s == "Top")
                                    {
                                        coord1 = dx;
                                        coord2 = dy;
                                        coord3 = dz;
                                    }
                                    else if (s == "Front")
                                    {
                                        coord1 = dx;
                                        coord2 = dz;
                                        coord3 = dy;
                                    }
                                    else if (s == "Side")
                                    {
                                        coord1 = dz;
                                        coord2 = dy;
                                        coord3 = dx;
                                    }

                                    // assign the correct series for each state: visited; last visited; non visited.
                                    if (visits > 0)
                                    {
                                        // is visited
                                        if (tvp.Value.Name != previousSystemName)
                                        {
                                            visitedSeries.Points.Add(new ScatterPoint(coord1, coord2));
                                            visitedSeries.TrackerFormatString = sysName;
                                        }
                                        // is visited, and is the last visited system
                                        else if (tvp.Value.Name == previousSystemName)
                                        {
                                            lastoneSeries.Points.Add(new ScatterPoint(coord1, coord2));
                                            lastoneSeries.TrackerFormatString = sysName;
                                        }
                                    }
                                    else
                                    // is not visited yet
                                    {
                                        inrangeSeries.Points.Add(new ScatterPoint(coord1, coord2));
                                        inrangeSeries.TrackerFormatString = sysName;
                                    }                                    
                                }                                
                            }                                                        
                        }
                    }

                    // debug
                    reportView.AppendText("\n");
                }

                // debug
                //debugView.AppendText("\n\nPlot completed.");
            }
        }
             
        private void textMinRadius_ValueChanged(object sender, EventArgs e)
        {
            KickComputation(uctg.GetCurrentHistoryEntry);            
        }

        private void textMaxRadius_ValueChanged(object sender, EventArgs e)
        {
            KickComputation(uctg.GetCurrentHistoryEntry);            
        }

        private void comboBoxView_SelectedIndexChanged(object sender, EventArgs e)
        {
            string s = comboBoxView.SelectedItem.ToString();
            if ( s == "Top")
            {                
                dataGridList.Visible = false;
                buttonExportToImage.Enabled = true;
                reportView.Visible = false;
                KickComputation(uctg.GetCurrentHistoryEntry);                
            }
            if (s == "Front")
            {
                dataGridList.Visible = false;
                buttonExportToImage.Enabled = true;
                reportView.Visible = false;
                KickComputation(uctg.GetCurrentHistoryEntry);                
            }
            if (s == "Side")
            {
                dataGridList.Visible = false;
                buttonExportToImage.Enabled = true;
                reportView.Visible = false;
                KickComputation(uctg.GetCurrentHistoryEntry);                
            }
            if (s == "Grid")
            {
                dataGridList.Visible = true;
                buttonExportToImage.Enabled = false;
                reportView.Visible = false;
            }
            if (s == "Report")
            {
                dataGridList.Visible = false;
                buttonExportToImage.Enabled = false;
                reportView.Visible = true;
            }
        }
        
        private void checkBoxDotSize_CheckedChanged(object sender, EventArgs e)
        {
 
        }

        private void dataGridList_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (sysName.Equals(e.Column) && sysX.Equals(e.Column) && sysY.Equals(e.Column) && sysZ.Equals(e.Column))
                e.SortDataGridViewColumnDate();
        }

        
        private void buttonExportToImage_Click(object sender, EventArgs e)
        {            
            string FileName = "Plot around " + currentSystemName + " " + " view - in range " + textMinRadius.Value.ToString() + "Ly to " + textMaxRadius.Value.ToString() + "Ly.png";
            string AddPAth = currentSystemName;
            string screenshotsDirdefault = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Frontier Developments", "Plots");

            string OutputPath = Path.Combine(screenshotsDirdefault, AddPAth);
            System.IO.Directory.CreateDirectory(OutputPath);

            string FilePath = Path.Combine(OutputPath, FileName);            

            //chartBubblePlot.SaveImage(FilePath, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
           
        }

        private void comboBoxView_EnabledChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBoxView_TextChanged(object sender, EventArgs e)
        {            
            string s = comboBoxView.SelectedItem.ToString();
            if (s == "Top")
            {
                dataGridList.Visible = false;
                buttonExportToImage.Enabled = true;
                reportView.Visible = false;
            }
            if (s == "Front")
            {
                dataGridList.Visible = false;
                buttonExportToImage.Enabled = true;
                reportView.Visible = false;
            }
            if (s == "Side")
            {
                dataGridList.Visible = false;
                buttonExportToImage.Enabled = true;
                reportView.Visible = false;
            }
            if (s == "Grid")
            {
                dataGridList.Visible = true;
                buttonExportToImage.Enabled = false;
                reportView.Visible = false;
            }
            if (s == "Report")
            {
                dataGridList.Visible = false;
                buttonExportToImage.Enabled = false;
                reportView.Visible = true;
            }
        }

        private void buttonExportReport_Click(object sender, EventArgs e)
        {
            
        }
    }    
}