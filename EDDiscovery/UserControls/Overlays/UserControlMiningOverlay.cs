/*
 * Copyright © 2017 EDDiscovery development team
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
using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace EDDiscovery.UserControls
{
    public partial class UserControlMiningOverlay :   UserControlCommonBase
    {
        private string DBChart { get { return DBName("MiningOverlay", "ChartSel"); } }

        class MaterialsFound
        {
            public string matnamefd2;
            public string friendlyname;

            public double amountrefined;            // amount refined
            public double amountcollected;          // amount collected
            public double amountdiscarded;
            public bool discovered;

            public int prospectednoasteroids;      // prospector entry updates this
            public List<double> prospectedamounts;

            public int motherloadasteroids;         // number where in motherload.

            public static MaterialsFound Find(string fdname, List<MaterialsFound> list)
            {
                MaterialsFound mat = list.Find(x => x.matnamefd2.Equals(fdname, StringComparison.InvariantCultureIgnoreCase));
                if (mat == null)
                {
                    mat = new MaterialsFound();
                    mat.matnamefd2 = fdname;
                    mat.friendlyname = MaterialCommodityData.GetByFDName(fdname)?.Name ?? fdname;
                    mat.prospectedamounts = new List<double>();
                    list.Add(mat);
                }

                return mat;
            }
        }

        #region Init

        public UserControlMiningOverlay()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            UpdateComboBox(null);
            BaseUtils.Translator.Instance.Translate(this);
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;
            Resize += UserControlMiningOverlay_Resize;
        }

        public override void InitialDisplay()
        {
            Display(uctg.GetCurrentHistoryEntry);
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;
        }

        public override bool SupportTransparency { get { return true; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            pictureBox.BackColor = this.BackColor = curcol;
            Display();
        }

        public override void Closing()
        {
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
        }

        private void Uctg_OnTravelSelectionChanged(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            Display(he);
        }

        private void UserControlMiningOverlay_Resize(object sender, EventArgs e)
        {
            Display();
        }

        #endregion

        #region Implementation

        List<HistoryEntry> curlist;
        HistoryEntry heabove, hebelow;
        object selectedchart = null;

        private void Display(HistoryEntry he)       // at he, see if changed
        {
            if ( he != null )
            {
                JournalTypeEnum[] boundevents = new JournalTypeEnum[] { JournalTypeEnum.Docked, JournalTypeEnum.Undocked, JournalTypeEnum.FSDJump, JournalTypeEnum.CarrierJump,
                                JournalTypeEnum.Touchdown, JournalTypeEnum.Liftoff };
                JournalTypeEnum[] miningevents = new JournalTypeEnum[] { JournalTypeEnum.AsteroidCracked, JournalTypeEnum.ProspectedAsteroid, JournalTypeEnum.LaunchDrone,
                                JournalTypeEnum.MiningRefined, JournalTypeEnum.MaterialCollected, JournalTypeEnum.MaterialDiscovered, JournalTypeEnum.MaterialDiscarded};

                var newlist = discoveryform.history.FilterBetween(he, x => boundevents.Contains(x.EntryType), y => miningevents.Contains(y.EntryType), out HistoryEntry newhebelow, out HistoryEntry newheabove);

                if (newlist != null)        // only if no history would we get null, unlikely since he has been tested, but still..
                {
                    // if no list, or diff no of items (due to new entry) or different start point, we reset and display, else we just quit as current is good
                    if (curlist == null || newlist.Count != curlist.Count || hebelow != newhebelow) 
                    {
                        curlist = newlist;
                        heabove = newheabove;
                        hebelow = newhebelow;
                        System.Diagnostics.Debug.WriteLine("Redisplay {0}", heabove.Indexno);
                        Display();

                    }

                    return;
                }
            }

            if (curlist != null)        // moved outside to no data..
            {
                curlist = null;         // fall thru means no data, clear and display
                heabove = hebelow = null;
                Display();
            }
        }

        private List<MaterialsFound> ReadHistory(out int limpetsleft, out int prospectorsused, out int collectorsused, out int asteroidscracked, out int prospected)
        {
            limpetsleft = heabove.MaterialCommodity.FindFDName("drones")?.Count ?? 0;
            prospectorsused = 0;
            collectorsused = 0;
            asteroidscracked = 0;
            prospected = 0;
            var found = new List<MaterialsFound>();

            foreach (var e in curlist)
            {
                System.Diagnostics.Debug.WriteLine("{0} {1} {2}", e.Indexno, e.EventTimeUTC, e.EventSummary);

                switch (e.EntryType)
                {
                    case JournalTypeEnum.AsteroidCracked:
                        asteroidscracked++;
                        break;
                    case JournalTypeEnum.ProspectedAsteroid:
                        prospected++;
                        var pa = e.journalEntry as JournalProspectedAsteroid;
                        if (pa.Materials != null)
                        {
                            foreach (var m in pa.Materials)
                            {
                                var matpa = MaterialsFound.Find(m.Name, found);
                                matpa.prospectednoasteroids++;
                                matpa.prospectedamounts.Add(m.Proportion);

                                System.Diagnostics.Debug.WriteLine("Prospected {0} {1}", m.Name, m.Proportion);
                            }

                            if (pa.MotherlodeMaterial.HasChars())
                            {
                                var matpa = MaterialsFound.Find(pa.MotherlodeMaterial, found);
                                matpa.motherloadasteroids++;
                            }
                        }
                        break;
                    case JournalTypeEnum.LaunchDrone:
                        var ld = e.journalEntry as JournalLaunchDrone;
                        if (ld.Type == JournalLaunchDrone.DroneType.Collection)
                            collectorsused++;
                        else
                            prospectorsused++;
                        break;
                    case JournalTypeEnum.MiningRefined:
                        var mr = e.journalEntry as JournalMiningRefined;
                        var matmr = MaterialsFound.Find(mr.Type, found);
                        matmr.amountrefined++;
                        break;
                    case JournalTypeEnum.MaterialCollected:
                        var mc = e.journalEntry as JournalMaterialCollected;
                        var matmc = MaterialsFound.Find(mc.Name, found);
                        matmc.amountcollected += mc.Count;
                        System.Diagnostics.Debug.WriteLine("Collected {0} {1}", mc.Count, matmc.amountcollected);
                        break;
                    case JournalTypeEnum.MaterialDiscarded:
                        var md = e.journalEntry as JournalMaterialDiscarded;
                        var matmd = MaterialsFound.Find(md.Name, found);
                        matmd.amountdiscarded += md.Count;
                        break;
                    case JournalTypeEnum.MaterialDiscovered:
                        var mdi = e.journalEntry as JournalMaterialDiscovered;
                        var matdi = MaterialsFound.Find(mdi.Name, found);
                        matdi.discovered = true;
                        break;
                }
            }

            found.Sort(delegate (MaterialsFound left, MaterialsFound right)
            {
                if (left.amountrefined > 0 || right.amountrefined > 0)
                    return right.amountrefined.CompareTo(left.amountrefined);
                else
                    return right.amountcollected.CompareTo(left.amountcollected);
            });

            return found;
        }

        private void Display()
        {
            pictureBox.ClearImageList();
            Controls.RemoveByKey("chart");

            if ( curlist != null)
            {
                var found = ReadHistory(out int limpetsleft, out int prospectorsused, out int collectorsused, out int asteroidscracked, out int prospected);

                Font displayfont = discoveryform.theme.GetFont;
                Color textcolour = IsTransparent ? discoveryform.theme.SPanelColor : discoveryform.theme.LabelColor;
                Color backcolour = IsTransparent ? Color.Transparent : this.BackColor;
                Color GridC = discoveryform.theme.GridBorderLines;
                Color TextC = discoveryform.theme.GridCellText;
                Color BackC = discoveryform.theme.GridCellBack;
                Color[] LineC = new Color[] { discoveryform.theme.VisitedSystemColor , discoveryform.theme.TextBlockHighlightColor,
                                                         Color.Blue, Color.Yellow, Color.Green, Color.Gray, Color.HotPink, Color.Teal };


                using (StringFormat frmt = new StringFormat())
                {
                    frmt.FormatFlags = StringFormatFlags.NoWrap;

                    int vpos = 5;
                    int colwidth = (int)BaseUtils.BitMapHelpers.MeasureStringInBitmap("0000", displayfont, frmt).Width;
                    int percentwidth = (int)BaseUtils.BitMapHelpers.MeasureStringInBitmap("00000.0", displayfont, frmt).Width;

                    int[] colsw = new int[] { colwidth * 4, colwidth, colwidth, colwidth, percentwidth, percentwidth, percentwidth, percentwidth, percentwidth, percentwidth};
                    int[] hpos = new int[colsw.Length];
                    hpos[0] = 4;
                    for (int i = 1; i < hpos.Length; i++)
                        hpos[i] = hpos[i - 1] + colsw[i - 1];

                    var ieprosp = pictureBox.AddTextAutoSize(new Point(hpos[0], vpos), new Size(this.Width - 20, this.Height), "Limpets left " + limpetsleft, displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                    vpos = ieprosp.Location.Bottom + displayfont.ScalePixels(2);

                    if (collectorsused > 0 || prospectorsused > 0)
                    {
                        string text = BaseUtils.FieldBuilder.Build("Prospectors Fired ", prospectorsused, "Collectors Deployed ", collectorsused);
                        pictureBox.AddTextAutoSize(new Point(ieprosp.Location.Right, ieprosp.Location.Top), new Size(this.Width - ieprosp.Location.Right - 20, this.Height), text, displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                    }

                    if ( prospected > 0 || found.Count>0 )
                    { 
                        var ie = pictureBox.AddTextAutoSize(new Point(hpos[1], vpos), new Size(colsw[1], this.Height), "Ref.", displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                        pictureBox.AddTextAutoSize(new Point(hpos[2], vpos), new Size(colsw[2], this.Height), "Coll.", displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                        pictureBox.AddTextAutoSize(new Point(hpos[3], vpos), new Size(colsw[3], this.Height), "Prosp.", displayfont, textcolour, backcolour, 1.0F, frmt: frmt);

                        pictureBox.AddTextAutoSize(new Point(hpos[4], vpos), new Size(colsw[4], this.Height), "Ratio%", displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                        pictureBox.AddTextAutoSize(new Point(hpos[5], vpos), new Size(colsw[5], this.Height), "Avg%", displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                        pictureBox.AddTextAutoSize(new Point(hpos[6], vpos), new Size(colsw[6], this.Height), "Min%", displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                        pictureBox.AddTextAutoSize(new Point(hpos[7], vpos), new Size(colsw[7], this.Height), "Max%", displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                        pictureBox.AddTextAutoSize(new Point(hpos[8], vpos), new Size(colsw[7], this.Height), "M.Lode", displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                        vpos = ie.Location.Bottom + displayfont.ScalePixels(2);
                    }

                    if ( prospected > 0 )
                    {
                        var ie = pictureBox.AddTextAutoSize(new Point(hpos[0], vpos), new Size(colsw[0], this.Height), "Asteroids Pros.", displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                        pictureBox.AddTextAutoSize(new Point(hpos[3], vpos), new Size(colsw[3], this.Height), prospected.ToString("N0"), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                        vpos = ie.Location.Bottom + displayfont.ScalePixels(2);
                    }

                    foreach (var m in found)
                    {
                        var ie1 = pictureBox.AddTextAutoSize(new Point(hpos[0], vpos), new Size(colsw[0], this.Height), m.friendlyname, displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                        pictureBox.AddTextAutoSize(new Point(hpos[1], vpos), new Size(colsw[1], this.Height), m.amountrefined.ToString("N0"), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                        pictureBox.AddTextAutoSize(new Point(hpos[2], vpos), new Size(colsw[2], this.Height), (m.amountcollected-m.amountdiscarded).ToString("N0"), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                        if ( m.prospectednoasteroids>0)
                        {
                            pictureBox.AddTextAutoSize(new Point(hpos[3], vpos), new Size(colsw[3], this.Height), m.prospectednoasteroids.ToString("N0"), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);

                            pictureBox.AddTextAutoSize(new Point(hpos[4], vpos), new Size(colsw[4], this.Height), (100.0*(double)m.prospectednoasteroids/prospected).ToString("N1"), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                            pictureBox.AddTextAutoSize(new Point(hpos[5], vpos), new Size(colsw[5], this.Height), m.prospectedamounts.Average().ToString("N1"), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                            pictureBox.AddTextAutoSize(new Point(hpos[6], vpos), new Size(colsw[6], this.Height), m.prospectedamounts.Min().ToString("N1"), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                            pictureBox.AddTextAutoSize(new Point(hpos[7], vpos), new Size(colsw[7], this.Height), m.prospectedamounts.Max().ToString("N1"), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                        }

                        if ( m.motherloadasteroids>0)
                        {
                            pictureBox.AddTextAutoSize(new Point(hpos[8], vpos), new Size(colsw[8], this.Height), m.motherloadasteroids.ToString("N0"), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                        }

                        vpos = ie1.Location.Bottom + displayfont.ScalePixels(2);
                    }

                    pictureBox.Render(true, new Size(0, vpos + displayfont.ScalePixels(8)));       // control is resized with a min height

                    var prospectedlist = found.Where(m => m.prospectednoasteroids > 0).ToList();

                    UpdateComboBox(prospectedlist.Select(m=>m.friendlyname).ToList());

                    int? seli = selectedchart as int?;
                    int? selm = selectedchart is string ? prospectedlist.FindIndex(x => x.friendlyname == selectedchart as string) : default(int?);

                    if (prospectedlist.Count > 0 && ((seli.HasValue && seli > 0) || selm.HasValue))
                    {
                        List<MaterialsFound> matdata = new List<MaterialsFound>();
                        if (seli.HasValue)
                        {
                            if (seli == chartfixeditems.Length - 1)
                                seli = LineC.Length;

                            for (int i =0; i < seli.Value && i < prospectedlist.Count; i++)
                                matdata.Add(prospectedlist[i]);
                        }
                        else
                            matdata.Add(prospectedlist[selm.Value]);

                        try
                        {
                            Chart chart = new Chart();
                            chart.Name = "chart";       // important for remove by key above
                            chart.Dock = DockStyle.Fill;
                            chart.BorderlineColor = GridC;          // around the whole thing.
                            chart.BackColor = backcolour;           // the whole chart background

                            chart.BeginInit();
                            chart.BorderlineDashStyle = ChartDashStyle.Solid;

                            ChartArea chartarea = new ChartArea();
                            chartarea.BorderColor = GridC;          // gives the top/right colours for this type of chart
                            chartarea.BorderDashStyle = ChartDashStyle.Solid;
                            chartarea.BorderWidth = 1;
                            chartarea.BackColor = BackC;            // chart area is coloured slightly

                            chartarea.AxisX.LabelStyle.ForeColor = TextC;  // label on axis colour
                            chartarea.AxisX.LabelStyle.Font = displayfont;
                            chartarea.AxisX.MajorGrid.LineColor = GridC;   // major grid colour
                            chartarea.AxisX.LineColor = GridC;      // axis (0 value) colour
                            chartarea.AxisX.Title = "Content %";
                            chartarea.AxisX.TitleFont = displayfont;
                            chartarea.AxisX.TitleForeColor = TextC;
                            chartarea.AxisX.Interval = 10;
                            chartarea.AxisX.Minimum = 0;

                            chartarea.AxisY.LabelStyle.ForeColor = TextC;
                            chartarea.AxisY.LabelStyle.Font = displayfont;
                            chartarea.AxisY.MajorGrid.LineColor = GridC;
                            chartarea.AxisY.LineColor = GridC;      // axis (0 value) colour
                            chartarea.AxisY.Title = "% Above";
                            chartarea.AxisY.TitleFont = displayfont;
                            chartarea.AxisY.TitleForeColor = TextC;
                            chartarea.AxisY.Interval = 10;
                            chartarea.AxisY.Maximum = 101;

                            chart.ChartAreas.Add(chartarea);

                            chart.Titles.Clear();
                            var title = new Title((matdata.Count == 1 ? (matdata[0].friendlyname+ " ") : "") + "Distribution", Docking.Top, displayfont, TextC);
                            chart.Titles.Add(title);

                            Legend legend = null;

                            if (matdata.Count > 1)      // one legend, series are attached to it via name field
                            {
                                legend = new Legend();
                                legend.Name = "Legend1";
                                legend.LegendStyle = LegendStyle.Column;
                                legend.Docking = Docking.Right;
                                legend.BackColor = backcolour;
                                legend.ForeColor = TextC;
                                chart.Legends.Add(legend);
                            }

                            int mi = 0;
                            foreach (var m in matdata)
                            {
                                Series series = new Series();
                                series.Name = m.friendlyname;
                                series.ChartArea = "ChartArea1";
                                series.ChartType = SeriesChartType.Line;
                                series.Color = LineC[mi];
                                series.Legend = "Legend1";

                                int i = 0;
                                for (i = 0; i < 50; i++)        // 0 - 50% fixed
                                {
                                    int numberabove = m.prospectedamounts.Count(x => x >= i);
                                    series.Points.Add(new DataPoint(i, (double)numberabove / m.prospectednoasteroids * 100.0));
                                    series.Points[i].AxisLabel = i.ToString();
                                }

                                chart.Series.Add(series);
                                mi++;
                            }

                            chart.EndInit();

                            Controls.Add(chart);
                            Controls.SetChildIndex(chart, 0);

                        }
                        catch (NotImplementedException)
                        {
                            // Charting not implemented in mono System.Windows.Forms
                        }
                    }

                }
            }
            else
                pictureBox.Render();

            Update();
        }

        #endregion

        #region UI

        static string[] chartfixeditems = new string[] { "No Chart", "Top Item", "Two Items", "Three items", "Four items", "Up to Eight" };

        void UpdateComboBox(List<string> otheritems)
        {
            extComboBoxChartOptions.SelectedIndexChanged -= ExtComboBoxChartOptions_SelectedIndexChanged;
            extComboBoxChartOptions.Items.Clear();
            extComboBoxChartOptions.Items.AddRange(chartfixeditems);
            if (otheritems != null)
                extComboBoxChartOptions.Items.AddRange(otheritems);

            if ( selectedchart == null )        // opening gambit, pick up saved pos
            {
                selectedchart = extComboBoxChartOptions.SelectedIndex = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(DBChart, 0);
            }
            else if (selectedchart is int)      // if on int, its one of the chartfixeditems
            {
                extComboBoxChartOptions.SelectedIndex = (int)selectedchart;
            }
            else
            {                                   // its a name, see if its there
                int i = extComboBoxChartOptions.Items.IndexOf(selectedchart as string);   // try and find the name of the mat
                if (i >= 0)
                {
                    extComboBoxChartOptions.SelectedIndex = i;
                }
                else
                {           // its not there, its been removed for some reason, go back to default state
                    selectedchart = extComboBoxChartOptions.SelectedIndex = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(DBChart, 0);      // not found, for some reason, remove and go back to defauklt
                }
            }
            
            extComboBoxChartOptions.SelectedIndexChanged += ExtComboBoxChartOptions_SelectedIndexChanged;
        }

        private void ExtComboBoxChartOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (extComboBoxChartOptions.SelectedIndex < chartfixeditems.Length)
            {
                selectedchart = extComboBoxChartOptions.SelectedIndex;
                EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(DBChart, (int)selectedchart);
            }
            else
                selectedchart = extComboBoxChartOptions.Text;
            Display();
        }


        #endregion
    }
}
