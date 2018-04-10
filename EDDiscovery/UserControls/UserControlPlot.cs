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
            //comboBoxView.Items.Add("Report");
            comboBoxView.Items.DefaultIfEmpty("Top");
            comboBoxView.SelectedItem = SQLiteConnectionUser.GetSettingString(DbSave + "PlotOrientation", "Top");
            comboBoxView.Enabled = true;

            // retrieve the path for export plots and reports
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
            plotViewTop.Refresh();
            plotViewFront.Refresh();
            plotViewSide.Refresh();
            dataGridList.Rows.Clear();
            reportView.Text = string.Empty;
            reportView.Refresh();

            var x = this.Handle; // workaround to avoid throw an exception if open the panel as pop up

            if (he?.System != null && he.System.HasCoordinate)
            {
                computer.CalculateClosestSystems(he.System,
                    (s, d) => BeginInvoke((MethodInvoker)delegate { NewStarListComputed(s, d); }),
                    maxitems, textMinRadius.Value, textMaxRadius.Value, true);
            }
        }

        private void NewStarListComputed(ISystem sys, BaseUtils.SortedListDoubleDuplicate<ISystem> list)      // In UI
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

            var pointSize = 4;

            // initializing the plot
            var modelTop = new PlotModel { Title = "Plot around " + currentSystem.Name };
            var modelFront = new PlotModel { Title = "Plot around " + currentSystem.Name };
            var modelSide = new PlotModel { Title = "Plot around " + currentSystem.Name };
            this.plotViewTop.Model = modelTop;
            this.plotViewFront.Model = modelFront;
            this.plotViewSide.Model = modelSide;

            // Define defaults properties of the series for the Top view
            var currentSeriesTop = new ScatterSeries { MarkerType = MarkerType.Circle, MarkerSize = pointSize, MarkerFill = OxyColors.Red };
            var lastoneSeriesTop = new ScatterSeries { MarkerType = MarkerType.Circle, MarkerSize = pointSize, MarkerFill = OxyColors.Purple };
            var inrangeSeriesTop = new ScatterSeries { MarkerType = MarkerType.Circle, MarkerSize = pointSize, MarkerFill = OxyColors.Yellow };
            var visitedSeriesTop = new ScatterSeries { MarkerType = MarkerType.Circle, MarkerSize = pointSize, MarkerFill = OxyColors.Cyan };

            // Define defaults properties of the series for the Front view
            var currentSeriesFront = new ScatterSeries { MarkerType = MarkerType.Circle, MarkerSize = pointSize, MarkerFill = OxyColors.Red };
            var lastoneSeriesFront = new ScatterSeries { MarkerType = MarkerType.Circle, MarkerSize = pointSize, MarkerFill = OxyColors.Purple };
            var inrangeSeriesFront = new ScatterSeries { MarkerType = MarkerType.Circle, MarkerSize = pointSize, MarkerFill = OxyColors.Yellow };
            var visitedSeriesFront = new ScatterSeries { MarkerType = MarkerType.Circle, MarkerSize = pointSize, MarkerFill = OxyColors.Cyan };

            // Define defaults properties of the series for the Side view
            var currentSeriesSide = new ScatterSeries { MarkerType = MarkerType.Circle, MarkerSize = pointSize, MarkerFill = OxyColors.Red };
            var lastoneSeriesSide = new ScatterSeries { MarkerType = MarkerType.Circle, MarkerSize = pointSize, MarkerFill = OxyColors.Purple };
            var inrangeSeriesSide = new ScatterSeries { MarkerType = MarkerType.Circle, MarkerSize = pointSize, MarkerFill = OxyColors.Yellow };
            var visitedSeriesSide = new ScatterSeries { MarkerType = MarkerType.Circle, MarkerSize = pointSize, MarkerFill = OxyColors.Cyan };

            // Add the series
            modelTop.Series.Add(currentSeriesTop);
            modelTop.Series.Add(lastoneSeriesTop);
            modelTop.Series.Add(visitedSeriesTop);
            modelTop.Series.Add(inrangeSeriesTop);

            // Add the series
            modelFront.Series.Add(currentSeriesFront);
            modelFront.Series.Add(lastoneSeriesFront);
            modelFront.Series.Add(visitedSeriesFront);
            modelFront.Series.Add(inrangeSeriesFront);

            // Add the series
            modelSide.Series.Add(currentSeriesSide);
            modelSide.Series.Add(lastoneSeriesSide);
            modelSide.Series.Add(visitedSeriesSide);
            modelSide.Series.Add(inrangeSeriesSide);

            // titles
            modelTop.Title = "Plot around " + currentSystemName + ", viewed from the top";
            modelFront.Title = "Plot around " + currentSystemName + ", viewed from the front";
            modelSide.Title = "Plot around " + currentSystemName + ", viewed from the side";

            // Title of the report           
            reportView.Text += ("\nSystems around " + currentSystemName + ", from " + textMinRadius.Value.ToString() + " to " + textMaxRadius.Value.ToString() + "Ly: " + csl.Count.ToString() + "\n");

            // Fill with some information for the report                    
            //reportView.AppendText("\nText " + currentSystem.some_value_interesting_to_report);
            reportView.Text += ("\nVisits: " + discoveryform.history.GetVisitsCount(currentSystem.Name, currentSystem.EDSMID));
            reportView.Text += ("\nNotes: " + currentSystem.SystemNote + "\n");

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
                        reportView.Text += ("\n" + tvp.Value.Name.ToString() + ", distant " + distFromCurrentSys + "Ly ");
                        reportView.Text += ("\n" + "Visits: " + visits.ToString());
                        reportView.Text += ("\nCoordinates: " + "X:" + sysX + ", Y:" + sysY + ", Z:" + sysZ);

                        // Create the list, with each system's name, distances by x, y and z coordinates and number of visits
                        object[] plotobj = { tvp.Value.Name, $"{sysX:0.00}", $"{sysY:0.00}", $"{sysZ:0.00}", $"{visits:n0}" };
                        int rowindex = dataGridList.Rows.Add(plotobj);
                        dataGridList.Rows[rowindex].Tag = tvp.Value;

                        var seriesTop = inrangeSeriesTop;
                        var seriesFront = inrangeSeriesFront;
                        var seriesSide = inrangeSeriesSide;

                        // Assign each system to the correct color and series, depending of its state: 
                        // visited; lastone; inrange (not visited).
                        if (visits > 0)
                        {
                            // is visited
                            if (tvp.Value.Name != previousSystemName)
                            {
                                seriesTop = visitedSeriesTop;
                                seriesFront = visitedSeriesFront;
                                seriesSide = visitedSeriesSide;
                            }
                            // is visited, and is the last visited system
                            else if (tvp.Value.Name == previousSystemName)
                            {
                                seriesTop = lastoneSeriesTop;
                                seriesFront = lastoneSeriesFront;
                                seriesSide = lastoneSeriesSide;
                            }
                        }
                        else
                        // is not visited yet
                        {
                            seriesTop = inrangeSeriesTop;
                            seriesFront = inrangeSeriesFront;
                            seriesSide = inrangeSeriesSide;
                        }
                        //

                        // Draw each point in the Plot                        
                        seriesTop.Points.Add(new ScatterPoint(Convert.ToDouble(plotobj[1]), Convert.ToDouble(plotobj[2]), pointSize, Convert.ToDouble(plotobj[3]), plotobj[0]));
                        seriesFront.Points.Add(new ScatterPoint(Convert.ToDouble(plotobj[1]), Convert.ToDouble(plotobj[3]), pointSize, Convert.ToDouble(plotobj[2]), plotobj[0]));
                        seriesSide.Points.Add(new ScatterPoint(Convert.ToDouble(plotobj[2]), Convert.ToDouble(plotobj[3]), pointSize, Convert.ToDouble(plotobj[1]), plotobj[0]));

                        // Create a tracker which shows the name of the system and its coordinates

                        string TrackerTop = "{Tag}\n" + "X: {2:0.###}; Y: {4:0.###}; Z: {6:0.###}";
                        string TrackerFront = "{Tag}\n" + "X: {2:0.###}; Z: {4:0.###}; Y: {6:0.###}";
                        string TrackerSide = "{Tag}\n" + "Y: {2:0.###}; Z: {4:0.###}; X: {6:0.###}";

                        seriesTop.TrackerFormatString = TrackerTop;
                        seriesFront.TrackerFormatString = TrackerFront;
                        seriesSide.TrackerFormatString = TrackerSide;
                    }

                    currentSeriesTop.Points.Add(new ScatterPoint(currentSystem.X, currentSystem.Y, pointSize, currentSystem.Z, currentSystemName));
                    currentSeriesFront.Points.Add(new ScatterPoint(currentSystem.X, currentSystem.Z, pointSize, currentSystem.Y, currentSystemName));
                    currentSeriesSide.Points.Add(new ScatterPoint(currentSystem.Y, currentSystem.Z, pointSize, currentSystem.X, currentSystemName));

                    string currentTracker = "{Tag}";

                    currentSeriesTop.TrackerFormatString = currentTracker;
                    currentSeriesFront.TrackerFormatString = currentTracker;
                    currentSeriesSide.TrackerFormatString = currentTracker;

                    // End of the systems list
                    reportView.Text += ("\n");
                }

                // End of the Report
                reportView.Text += ("\n\nReport created on " + DateTime.Now.ToString());
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
            if (s == "Top")
            {
                plotViewTop.Visible = true;
                plotViewFront.Visible = false;
                plotViewSide.Visible = false;
                dataGridList.Visible = false;
                reportView.Visible = false;
            }
            if (s == "Front")
            {
                plotViewTop.Visible = false;
                plotViewFront.Visible = true;
                plotViewSide.Visible = false;
                dataGridList.Visible = false;
                reportView.Visible = false;
            }
            if (s == "Side")
            {
                plotViewTop.Visible = false;
                plotViewFront.Visible = false;
                plotViewSide.Visible = true;
                dataGridList.Visible = false;
                reportView.Visible = false;
            }
            if (s == "Grid")
            {
                dataGridList.Visible = true;
                reportView.Visible = false;
            }
            if (s == "Report")
            {
                dataGridList.Visible = false;
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
                buttonExportPNG.Enabled = true;
                reportView.Visible = false;
            }
            if (s == "Front")
            {
                dataGridList.Visible = false;
                buttonExportPNG.Enabled = true;
                reportView.Visible = false;
            }
            if (s == "Side")
            {
                dataGridList.Visible = false;
                buttonExportPNG.Enabled = true;
                reportView.Visible = false;
            }
            if (s == "Grid")
            {
                dataGridList.Visible = true;
                buttonExportPNG.Enabled = true;
                reportView.Visible = false;
            }
            if (s == "Report")
            {
                dataGridList.Visible = false;
                buttonExportPNG.Enabled = true;
                reportView.Visible = true;
            }
        }

        string OutPath = "";

        private void SelectFolder()
        {            
            using (var folderDialog = new FolderBrowserDialog())
            {
                if (OutPath != null)
                {
                    folderDialog.SelectedPath = OutPath;
                }
                else
                {
                    folderDialog.RootFolder = Environment.SpecialFolder.MyPictures;
                }

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {                    
                    OutPath = folderDialog.SelectedPath.ToString();
                    Directory.CreateDirectory(OutPath);
                }
            }
        }

        private void buttonExportToImage_Click(object sender, EventArgs e)
        {
            SelectFolder();
                        
            try
            {
                var pngExporter = new PngExporter { Width = 900, Height = 900, Background = OxyColors.White };

                string FileNameTop = Path.Combine(OutPath, DateTime.Now.ToString("yyyyMMddHHmmss_") + currentSystemName + "_Top".AddSuffixToFilename(".png"));
                string FileNameFront = Path.Combine(OutPath, DateTime.Now.ToString("yyyyMMddHHmmss_") + currentSystemName + "_Front".AddSuffixToFilename(".png"));
                string FileNameSide = Path.Combine(OutPath, DateTime.Now.ToString("yyyyMMddHHmmss_") + currentSystemName + "_Side".AddSuffixToFilename(".png"));

                // Export Plots view as PNG
                pngExporter.ExportToFile(plotViewTop.Model, FileNameTop);
                pngExporter.ExportToFile(plotViewFront.Model, FileNameFront);
                pngExporter.ExportToFile(plotViewSide.Model, FileNameSide);
            }
            catch
            {

            }
        }

        private void buttonExtReport_Click(object sender, EventArgs e)
        {
            SelectFolder();

            try
            {   
                // Save a Report                                
                string FileNameReport = Path.Combine(OutPath, DateTime.Now.ToString("yyyyMMddHHmmss_") + currentSystemName + "_Report".AddSuffixToFilename(".txt"));
                File.WriteAllText(FileNameReport, reportView.Text);
            }
            catch
            {

            }
        }
    }
}