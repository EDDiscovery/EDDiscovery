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
        private string dataOutputDir;

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

            dataOutputDir = SQLiteDBClass.GetSettingString("ImageHandlerOutputDir", dataOutputDir);
            
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

            var pointSize = 3;            

            // initializing the plot
            var model = new PlotModel { Title = "Plot around " + currentSystem.Name };
            this.plotView.Model = model;

            // Define defaults properties of the series
            var currentSeries = new ScatterSeries { MarkerType = MarkerType.Circle, MarkerSize = pointSize, MarkerFill = OxyColors.Red };
            var lastoneSeries = new ScatterSeries { MarkerType = MarkerType.Circle, MarkerSize = pointSize, MarkerFill = OxyColors.Purple };
            var inrangeSeries = new ScatterSeries { MarkerType = MarkerType.Circle, MarkerSize = pointSize, MarkerFill = OxyColors.Yellow };
            var visitedSeries = new ScatterSeries { MarkerType = MarkerType.Circle, MarkerSize = pointSize, MarkerFill = OxyColors.Cyan };

            // Add the series
            model.Series.Add(currentSeries);
            model.Series.Add(lastoneSeries);
            model.Series.Add(visitedSeries);
            model.Series.Add(inrangeSeries);
                        
            // Draw the current system
            currentSeries.Points.Add(new ScatterPoint(currentSystem.X, currentSystem.Y, pointSize, currentSystem.Z, currentSystemName));
            currentSeries.TrackerFormatString = "{Tag}";
            
            // Title of the report           
            reportView.AppendText("\nSystems around " + currentSystemName + ", from " + textMinRadius.Value.ToString()  + " to " + textMaxRadius.Value.ToString() + "Ly: " + csl.Count.ToString() + "\n");

            // Fill with some information for the report                    
            //reportView.AppendText("\nText " + currentSystem.some_value_interesting_to_report);
            reportView.AppendText("\nCreated on:" + currentSystem.CreateDate);
            reportView.AppendText("\nNotes:" + currentSystem.SystemNote+ "\n");

            // If the are any system inside the defined range...
            if (csl.Count() > 0)
            {   
                // ...then iterate through each system in the list:
                foreach (KeyValuePair<double, ISystem> tvp in csl)
                {
                    // calculate the average distance;
                    var distFromCurrentSys = Math.Round(Math.Sqrt(tvp.Key), 2, MidpointRounding.AwayFromZero);

                    // count the total visits for each system;
                    int visits = discoveryform.history.GetVisitsCount(tvp.Value.Name, tvp.Value.EDSMID);
                    
                    // Then, populate the Grid with the systems in range
                    if (distFromCurrentSys >= textMinRadius.Value && distFromCurrentSys <= textMaxRadius.Value && tvp.Value.Name != currentSystemName)
                    {
                        // get the coordinates of each system in range;
                        var sysX = tvp.Value.X;
                        var sysY = tvp.Value.Y;
                        var sysZ = tvp.Value.Z;
                        
                        // print information on each member of the list;
                        reportView.AppendText("\n" + tvp.Value.Name.ToString() + ", distant " + distFromCurrentSys + "Ly ");
                        reportView.AppendText("\nCoordinates: " + sysX + ", " + sysY + ", " + sysZ);

                        // Create the list, with each system's name, distances by x, y and z coordinates and number of visits
                        object[] plotobj = { tvp.Value.Name, $"{sysX:0.00}", $"{sysY:0.00}", $"{sysZ:0.00}", $"{visits:n0}" };
                        int rowindex = dataGridList.Rows.Add(plotobj);
                        dataGridList.Rows[rowindex].Tag = tvp.Value;

                        var series = inrangeSeries;

                        // Assign each system to the correct color, depending of its state: 
                        // visited; lastone; inrange (not visited).
                        if (visits > 0)
                        {
                            // is visited
                            if (tvp.Value.Name != previousSystemName)
                            {
                                series = visitedSeries;
                            }
                            // is visited, and is the last visited system
                            else if (tvp.Value.Name == previousSystemName)
                            {
                                series = lastoneSeries;
                            }
                        }
                        else
                        // is not visited yet
                        {
                            series = inrangeSeries;
                        }
                        
                        // Draw each point in the Plot                        
                        series.Points.Add(new ScatterPoint(Convert.ToDouble(plotobj[1]), Convert.ToDouble(plotobj[2]), pointSize, Convert.ToDouble(plotobj[3]), plotobj[0]));

                        // Create a tracker which shows the name of the system and its coordinates
                        series.TrackerFormatString =
                            "{Tag}\n" +
                            "Z: {2:0.###}; Y: {4:0.###}; Z: {6:0.###}\n";                            
                    }

                    // debug
                    reportView.AppendText("\n");
                }

                // debug
                reportView.AppendText("\n\nReport created on " + DateTime.Now.ToString());
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

        private void buttonExportToImage_Click(object sender, EventArgs e)
        {
            try
            {
                //Plots
                string plotsDir = Path.Combine(dataOutputDir, "Plots");
                string systemPath = currentSystemName;
                string FilePath = Path.Combine(plotsDir, systemPath);
                File.Create(FilePath);

                string FileName = Path.Combine(FilePath, "Plot".AddSuffixToFilename(".png"));

                var pngExporter = new PngExporter { Width = 600, Height = 600, Background = OxyColors.White };
                pngExporter.ExportToFile(plotView.Model, FileName);
            }
            catch
            {
            	
            }
            
        }

        private void buttonExportReport_Click(object sender, EventArgs e)
        {
            if (reportView.Text.ToString() != null)
            {
                try
                {

                    string reportsDir = Path.Combine(dataOutputDir, "Reports");
                    string systemPath = currentSystemName;
                    string FileName = "Report";
                    string FilePath = Path.Combine(reportsDir, systemPath, FileName.AddSuffixToFilename(".txt"));
                    File.Create(FilePath);
                                        
                    // Create a string array with the lines of text
                    string[] lines = { "First line", "Second line", "Third line" };
                                        
                    // Write the string array to the output file
                    using (StreamWriter outputFile = new StreamWriter(FilePath))
                    {
                        foreach (string line in lines)
                            outputFile.WriteLine(line);
                    }                    
                }
                catch { }                
            }            
        }
    }    
}