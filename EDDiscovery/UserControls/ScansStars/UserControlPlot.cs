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

using EliteDangerousCore;
using EliteDangerousCore.DB;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlPlot : UserControlCommonBase
    {
        private string DbSave { get { return DBName("PlotPanel" ); } }

        private StarDistanceComputer computer;

        public UserControlPlot()
        {
            InitializeComponent();
            dataGridList.Visible = false;
            reportView.Visible = false;

            this.plotViewTop.ActualController.UnbindMouseDown(OxyMouseButton.Left);
            this.plotViewTop.ActualController.BindMouseEnter(PlotCommands.HoverSnapTrack);            
            this.plotViewFront.ActualController.UnbindMouseDown(OxyMouseButton.Left);
            this.plotViewFront.ActualController.BindMouseEnter(PlotCommands.HoverSnapTrack);
            this.plotViewSide.ActualController.UnbindMouseDown(OxyMouseButton.Left);
            this.plotViewSide.ActualController.BindMouseEnter(PlotCommands.HoverSnapTrack);

            this.plotViewTop.ActualController.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);
            this.plotViewFront.ActualController.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);
            this.plotViewSide.ActualController.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);

            this.plotViewTop.ActualController.BindMouseDown(OxyMouseButton.Right, PlotCommands.ZoomRectangle);
            this.plotViewFront.ActualController.BindMouseDown(OxyMouseButton.Right, PlotCommands.ZoomRectangle);
            this.plotViewSide.ActualController.BindMouseDown(OxyMouseButton.Right, PlotCommands.ZoomRectangle);
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
            textMinRadius.ValueNoChange = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(DbSave + "PlotMin", 0);
            textMaxRadius.ValueNoChange = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(DbSave + "PlotMax", defaultmaximumradarradius);
            textMinRadius.SetComparitor(textMaxRadius, -2);     // need to do this after values are set
            textMaxRadius.SetComparitor(textMinRadius, 2);

            comboBoxView.Enabled = false;
            comboBoxView.Items.Add("Top".T(EDTx.UserControlPlot_Top));
            comboBoxView.Items.Add("Front".T(EDTx.UserControlPlot_Front));
            comboBoxView.Items.Add("Side".T(EDTx.UserControlPlot_Side));
            comboBoxView.Items.Add("Grid".T(EDTx.UserControlPlot_Grid));
            comboBoxView.Items.Add("Report".T(EDTx.UserControlPlot_Report));

            var sel = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbSave + "PlotOrientation", "!!");
            if (comboBoxView.Items.Contains(sel))
                comboBoxView.SelectedItem = sel;
            else
                comboBoxView.SelectedIndex = 0;

            comboBoxView.Enabled = true;

            computer = new StarDistanceComputer();

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(menuStrip, this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);

            SelectView();
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
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble(DbSave + "PlotMin", textMinRadius.Value);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble(DbSave + "PlotMax", textMaxRadius.Value);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(DbSave + "PlotOrientation", comboBoxView.SelectedItem.ToString());
        }

        public override void InitialDisplay()
        {
            KickComputation(uctg.GetCurrentHistoryEntry);
        }

        private void Uctg_OnTravelSelectionChanged(HistoryEntry he, HistoryList hl, bool selectedEntry)
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
                    (s, d) => this.ParentForm.BeginInvoke((MethodInvoker)delegate { NewStarListComputed(s, d); }),
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
            SetControlText(string.Format("2D Plot of systems in range from {0} ".T(EDTx.UserControlPlot_2d) , currentSystem.Name));

            const int pointSize = 4;

            // initializing the plot
            var modelTop = new PlotModel { };
            var modelFront = new PlotModel { };
            var modelSide = new PlotModel { };

            this.plotViewTop.Model = modelTop;
            this.plotViewFront.Model = modelFront;
            this.plotViewSide.Model = modelSide;

            modelTop.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, TicklineColor = OxyColors.BlueViolet });
            modelTop.Axes.Add(new LinearAxis { Position = AxisPosition.Left, TicklineColor = OxyColors.BlueViolet, StartPosition = 1, EndPosition = -1 });
            modelFront.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, TicklineColor = OxyColors.BlueViolet });
            modelFront.Axes.Add(new LinearAxis { Position = AxisPosition.Left, TicklineColor = OxyColors.BlueViolet });
            modelSide.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, TicklineColor = OxyColors.BlueViolet });
            modelSide.Axes.Add(new LinearAxis { Position = AxisPosition.Left, TicklineColor = OxyColors.BlueViolet });

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
            modelTop.Title = string.Format("Plot around {0}, viewed from the top".T(EDTx.UserControlPlot_PTOP), currentSystemName);
            modelFront.Title = string.Format("Plot around {0}, viewed from the front".T(EDTx.UserControlPlot_PFRONT), currentSystemName);
            modelSide.Title = string.Format("Plot around {0}, viewed from the side".T(EDTx.UserControlPlot_PSIDE), currentSystemName);

            // Title of the report           
            reportView.Text += string.Format(("Systems around {0}, from {1} to {2}, Ly: {3}").T(EDTx.UserControlPlot_SysAROUND) ,currentSystemName, 
                textMinRadius.Value.ToString(), textMaxRadius.Value.ToString() ,csl.Count.ToString() );

            // Fill with some information for the report                    
            //reportView.AppendText("\nText " + currentSystem.some_value_interesting_to_report);
            reportView.Text += string.Format((Environment.NewLine + "    Visits: {0}").T(EDTx.UserControlPlot_Vs) ,discoveryform.history.GetVisitsCount(currentSystem.Name));
            //removed - system does not have a note. reportView.Text += string.Format((Environment.NewLine + "    Notes: {0}" + Environment.NewLine).T(EDTx.UserControlPlot_Nt") ,currentSystem.SystemNote);

            // If the are any system inside the defined range...
            if (csl.Count() > 0)
            {
                // ...then iterate through each system in the list:
                foreach (KeyValuePair<double, ISystem> tvp in csl)
                {
                    // calculate the average distance;
                    var distFromCurrentSys = Math.Round(Math.Sqrt(tvp.Key), 2, MidpointRounding.AwayFromZero);

                    // count the total visits for each system;
                    int visits = discoveryform.history.GetVisitsCount(tvp.Value.Name);

                    // Then, populate the Grid with the systems in range
                    if (distFromCurrentSys >= textMinRadius.Value && distFromCurrentSys <= textMaxRadius.Value && tvp.Value.Name != currentSystemName)
                    {
                        // get the coordinates of each system in range;
                        var sysX = tvp.Value.X;
                        var sysY = tvp.Value.Y;
                        var sysZ = tvp.Value.Z;

                        // print information on each member of the list;
                        reportView.Text += string.Format((Environment.NewLine + "{0} distance {1:N2} Ly").T(EDTx.UserControlPlot_D1), tvp.Value.Name.ToString() , distFromCurrentSys);
                        reportView.Text += string.Format((Environment.NewLine + "    Visits: {0}").T(EDTx.UserControlPlot_D2) , visits.ToString());
                        reportView.Text += string.Format((Environment.NewLine + "    Coordinates: X:{0:N2}, Y:{1:N2}, Z:{2:N2}" + Environment.NewLine).T(EDTx.UserControlPlot_D3) ,sysX , sysY , sysZ);

                        // Create the list, with each system's name, distances by x, y and z coordinates and number of visits
                        object[] value = { tvp.Value.Name, $"{sysX:0.00}", $"{sysY:0.00}", $"{sysZ:0.00}", $"{visits:n0}" };
                        var i = dataGridList.Rows.Add(value);
                        dataGridList.Rows[i].Tag = tvp.Value;

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
                        seriesTop.Points.Add(new ScatterPoint(Convert.ToDouble(value[1]), Convert.ToDouble(value[2]), pointSize, Convert.ToDouble(value[3]), value[0]));
                        seriesFront.Points.Add(new ScatterPoint(Convert.ToDouble(value[1]), Convert.ToDouble(value[3]), pointSize, Convert.ToDouble(value[2]), value[0]));
                        seriesSide.Points.Add(new ScatterPoint(Convert.ToDouble(value[2]), Convert.ToDouble(value[3]), pointSize, Convert.ToDouble(value[1]), value[0]));

                        // Create a tracker which shows the name of the system and its coordinates
                        const string trackerTop = "{Tag}\n" + "X: {2:0.###}; Y: {4:0.###}; Z: {6:0.###}";
                        const string trackerFront = "{Tag}\n" + "X: {2:0.###}; Z: {4:0.###}; Y: {6:0.###}";
                        const string trackerSide = "{Tag}\n" + "Y: {2:0.###}; Z: {4:0.###}; X: {6:0.###}";

                        seriesTop.TrackerFormatString = trackerTop;
                        seriesFront.TrackerFormatString = trackerFront;
                        seriesSide.TrackerFormatString = trackerSide;
                    }

                    currentSeriesTop.Points.Add(new ScatterPoint(currentSystem.X, currentSystem.Y, pointSize, currentSystem.Z, currentSystemName));
                    currentSeriesFront.Points.Add(new ScatterPoint(currentSystem.X, currentSystem.Z, pointSize, currentSystem.Y, currentSystemName));
                    currentSeriesSide.Points.Add(new ScatterPoint(currentSystem.Y, currentSystem.Z, pointSize, currentSystem.X, currentSystemName));

                    const string currentTrackerTop = "{Tag}\n" + "X: {2:0.###}; Y: {4:0.###}; Z: {6:0.###}";
                    const string currentTrackerFront = "{Tag}\n" + "X: {2:0.###}; Z: {4:0.###}; Y: {6:0.###}";
                    const string currentTrackerSide = "{Tag}\n" + "Y: {2:0.###}; Z: {4:0.###}; X: {6:0.###}";

                    currentSeriesTop.TrackerFormatString = currentTrackerTop;
                    currentSeriesFront.TrackerFormatString = currentTrackerFront;
                    currentSeriesSide.TrackerFormatString = currentTrackerSide;
                }

                // End of the Report
                reportView.Text += Environment.NewLine + Environment.NewLine + " @ " + DateTime.Now.ToString();
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
            SelectView();
        }

        private void SelectView()
        { 
            int i = comboBoxView.SelectedIndex;
            if (i==0)
            {
                plotViewTop.Visible = true;
                plotViewFront.Visible = false;
                plotViewSide.Visible = false;
                dataGridList.Visible = false;
                reportView.Visible = false;
            }
            else if (i==1)
            {
                plotViewTop.Visible = false;
                plotViewFront.Visible = true;
                plotViewSide.Visible = false;
                dataGridList.Visible = false;
                reportView.Visible = false;
            }
            else if (i==2)
            {
                plotViewTop.Visible = false;
                plotViewFront.Visible = false;
                plotViewSide.Visible = true;
                dataGridList.Visible = false;
                reportView.Visible = false;
            }
            else if (i==3)
            {
                plotViewTop.Visible = false;
                plotViewFront.Visible = false;
                plotViewSide.Visible = false;
                dataGridList.Visible = true;
                reportView.Visible = false;
            }
            else if (i==4)
            {
                plotViewTop.Visible = false;
                plotViewFront.Visible = false;
                plotViewSide.Visible = false;
                dataGridList.Visible = false;
                reportView.Visible = true;
            }
        }

        private void dataGridList_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (sysName.Equals(e.Column) && sysX.Equals(e.Column) && sysY.Equals(e.Column) && sysZ.Equals(e.Column))
                e.SortDataGridViewColumnDate();
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

        private void pNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectFolder();

            try
            {
                // Export to PNG
                var pngExporter = new PngExporter { Width = 900, Height = 900, Background = OxyColors.White };

                string FileNameTop = Path.Combine(OutPath, DateTime.Now.ToString("yyyyMMddHHmmss_") + currentSystemName + "_Top".AddSuffixToFilename(".png"));
                string FileNameFront = Path.Combine(OutPath, DateTime.Now.ToString("yyyyMMddHHmmss_") + currentSystemName + "_Front".AddSuffixToFilename(".png"));
                string FileNameSide = Path.Combine(OutPath, DateTime.Now.ToString("yyyyMMddHHmmss_") + currentSystemName + "_Side".AddSuffixToFilename(".png"));
                                
                pngExporter.ExportToFile(plotViewTop.Model, FileNameTop);
                pngExporter.ExportToFile(plotViewFront.Model, FileNameFront);
                pngExporter.ExportToFile(plotViewSide.Model, FileNameSide);
            }
            catch
            {

            }
        }

        private void pDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectFolder();

            try
            {
                // Export to PDF
                string FileNameTop = Path.Combine(OutPath, DateTime.Now.ToString("yyyyMMddHHmmss_") + currentSystemName + "_Top".AddSuffixToFilename(".pdf"));
                string FileNameFront = Path.Combine(OutPath, DateTime.Now.ToString("yyyyMMddHHmmss_") + currentSystemName + "_Front".AddSuffixToFilename(".pdf"));
                string FileNameSide = Path.Combine(OutPath, DateTime.Now.ToString("yyyyMMddHHmmss_") + currentSystemName + "_Side".AddSuffixToFilename(".pdf"));
                             
                using (var stream = File.Create(FileNameTop))
                {
                    var pdfExporter = new PdfExporter { Width = 900, Height = 900 };
                    pdfExporter.Export(plotViewTop.Model, stream);
                }
                using (var stream = File.Create(FileNameFront))
                {
                    var pdfExporter = new PdfExporter { Width = 900, Height = 900 };
                    pdfExporter.Export(plotViewFront.Model, stream);
                }
                using (var stream = File.Create(FileNameSide))
                {
                    var pdfExporter = new PdfExporter { Width = 900, Height = 900 };
                    pdfExporter.Export(plotViewSide.Model, stream);
                }
            }
            catch
            {

            }
        }

        private void sVGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectFolder();

            try
            {
                // Export to SVG
                string FileNameTop = Path.Combine(OutPath, DateTime.Now.ToString("yyyyMMddHHmmss_") + currentSystemName + "_Top".AddSuffixToFilename(".svg"));
                string FileNameFront = Path.Combine(OutPath, DateTime.Now.ToString("yyyyMMddHHmmss_") + currentSystemName + "_Front".AddSuffixToFilename(".svg"));
                string FileNameSide = Path.Combine(OutPath, DateTime.Now.ToString("yyyyMMddHHmmss_") + currentSystemName + "_Side".AddSuffixToFilename(".svg"));

                using (var stream = File.Create(FileNameTop))
                {
                    var exporter = new OxyPlot.SvgExporter { Width = 600, Height = 400 };
                    exporter.Export(plotViewTop.Model, stream);
                }
                using (var stream = File.Create(FileNameFront))
                {
                    var exporter = new OxyPlot.SvgExporter { Width = 600, Height = 400 };
                    exporter.Export(plotViewFront.Model, stream);
                }
                using (var stream = File.Create(FileNameSide))
                {
                    var exporter = new OxyPlot.SvgExporter { Width = 600, Height = 400 };
                    exporter.Export(plotViewSide.Model, stream);
                }
            }
            catch
            {

            }
        }

        private void tXTToolStripMenuItem_Click(object sender, EventArgs e)
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
