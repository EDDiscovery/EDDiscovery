/*
 * Copyright 2017-2025 EDDiscovery development team
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
        private string dbChart = "ChartSel";
        private string dbZeroRefined = "ZeroRefined";
        private string dbRolledUp = "RolledUp";
        private string dbChartBase = "ChartBase";

        #region Init

        public UserControlMiningOverlay()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "MiningOverlay";

            UpdateComboBox(null);

            var enumlisttt = new Enum[] { EDTx.UserControlMiningOverlay_extCheckBoxZeroRefined_ToolTip, EDTx.UserControlMiningOverlay_buttonExtExcel_ToolTip, 
                        EDTx.UserControlMiningOverlay_extComboBoxChartOptions_ToolTip, EDTx.UserControlMiningOverlay_extCheckBoxChartBase_ToolTip, 
                        };
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);
            extPanelRollUp.SetToolTip(toolTip);

            buttonExtExcel.Enabled = false;
            extCheckBoxZeroRefined.Checked = GetSetting(dbZeroRefined, false);
            extCheckBoxZeroRefined.CheckedChanged += new System.EventHandler(this.extCheckBoxZeroRefined_CheckedChanged);
            extPanelRollUp.PinState = GetSetting(dbRolledUp, true);
            extCheckBoxChartBase.Checked = GetSetting(dbChartBase, false);
            extCheckBoxChartBase.CheckedChanged += new System.EventHandler(this.extCheckBoxChartBase_CheckedChanged);

            timetimer = new Timer();
            timetimer.Interval = 1000;
            timetimer.Tick += Timetimer_Tick;
        }

        public override void LoadLayout()
        {
        }

        public override void InitialDisplay()
        {
            RequestPanelOperation(this, new UserControlCommonBase.RequestTravelHistoryPos());     //request an update 
        }

        public override bool SupportTransparency { get { return true; } }
        public override bool DefaultTransparent { get { return true; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            extPanelRollUp.Visible = !on;
            pictureBox.BackColor = this.BackColor = curcol;
        }
        public override void TransparencyModeChanged(bool on)
        {
            Display();
        }

        public override void Closing()
        {
            timetimer.Stop();
            PutSetting(dbRolledUp, extPanelRollUp.PinState);
        }

        public override void ReceiveHistoryEntry(HistoryEntry he)
        {
            // for the next version , screen out and only trigger on types..
            // synced with ReadHistory
            //if ( he.EntryType == JournalTypeEnum.AsteroidCracked || he.EntryType == JournalTypeEnum.ProspectedAsteroid || he.EntryType == JournalTypeEnum.LaunchDrone ||
            //    he.EntryType == JournalTypeEnum.MiningRefined || he.EntryType == JournalTypeEnum.MaterialCollected || he.EntryType == JournalTypeEnum.MaterialDiscarded || 
             //   he.EntryType == JournalTypeEnum.MaterialDiscovered )

            Display(he);
        }

        #endregion

        #region Implementation

        List<HistoryEntry> eventlist;   // found events
        HistoryEntry heabove, hebelow;  // markers
        bool incurrentplay;             // true when heabove is at top of history AND its not a stop event - so we are in a play session
        object selectedchart = null;    // chart count (if int) or material (string)
        const int CFDbMax = 50;         // 0-N% on CFDB

        double lastrefined;             // used for timer display - refined count   
        ExtendedControls.ExtPictureBox.ImageElement timeie; // image element of time
        Timer timetimer;                // and its timer

        int limpetsleftdisplay = 0;     // used to track if we need to redisplay due to limpet change
        int cargoleftdisplay = 0;       // used to track what we wrote for cargo

        ExtendedControls.ExtSafeChart chart = null;

        private void Display(HistoryEntry he)       // at he, see if changed
        {
            if ( he != null )
            {
                JournalTypeEnum[] boundevents = new JournalTypeEnum[] { JournalTypeEnum.Docked, JournalTypeEnum.Undocked, JournalTypeEnum.FSDJump, JournalTypeEnum.CarrierJump,
                                JournalTypeEnum.Touchdown, JournalTypeEnum.Liftoff };
                JournalTypeEnum[] miningevents = new JournalTypeEnum[] { JournalTypeEnum.AsteroidCracked, JournalTypeEnum.ProspectedAsteroid, JournalTypeEnum.LaunchDrone,
                                JournalTypeEnum.MiningRefined, JournalTypeEnum.MaterialCollected, JournalTypeEnum.MaterialDiscovered, JournalTypeEnum.MaterialDiscarded};

                var newlist = HistoryList.FilterBetween(DiscoveryForm.History.EntryOrder(), he, x => boundevents.Contains(x.EntryType), y => miningevents.Contains(y.EntryType), out HistoryEntry newhebelow, out HistoryEntry newheabove);

                if (newlist != null)        // only if no history would we get null, unlikely since he has been tested, but still..
                {
                    int limpetsleft = DiscoveryForm.History.MaterialCommoditiesMicroResources.Get(newheabove.MaterialCommodity,"drones")?.Count ?? 0;
                    int cargo = DiscoveryForm.History.MaterialCommoditiesMicroResources.CargoCount(newheabove.MaterialCommodity);
                    int cargocap = newheabove.ShipInformation?.CalculateCargoCapacity() ?? 0;// so if we don't have a ShipInformation, use 0
                    int cargoleft = cargocap - cargo; 

                    // if no list, or diff no of items (due to new entry) or different start point, we reset and display, else we just quit as current is good
                    if (eventlist == null || newlist.Count != eventlist.Count || hebelow != newhebelow || limpetsleft != limpetsleftdisplay || cargoleft != cargoleftdisplay) 
                    {
                        eventlist = newlist;
                        heabove = newheabove;
                        hebelow = newhebelow;
                        limpetsleftdisplay = limpetsleft;
                        cargoleftdisplay = cargoleft;
                        incurrentplay = heabove == DiscoveryForm.History.GetLast && !boundevents.Contains(heabove.EntryType);
                        System.Diagnostics.Debug.WriteLine("Redisplay {0} current {1}", heabove.EntryNumber, incurrentplay);
                        Display();

                    }

                    return;
                }
            }

            if (eventlist != null)        // moved outside to no data..
            {
                eventlist = null;         // fall thru means no data, clear and display
                heabove = hebelow = null;
                Display();
            }
        }

        private void Display()
        {
            timetimer.Stop();       // redisplay, stop the time timer and lose the timeie.
            timeie = null;

            pictureBox.ClearImageList();

            Controls.RemoveByKey("chart");
            chart?.Dispose();

            if (eventlist != null)
            {
                var found = ReadHistory( out int prospectorsused, out int collectorsused, out int asteroidscracked, out int prospected, out int[] content);

                Font displayfont = ExtendedControls.Theme.Current.GetFont;
                Color textcolour = IsTransparentModeOn ? ExtendedControls.Theme.Current.SPanelColor : ExtendedControls.Theme.Current.LabelColor;
                Color backcolour = IsTransparentModeOn ? Color.Transparent : this.BackColor;
                
                using (StringFormat frmt = new StringFormat())
                {
                    frmt.FormatFlags = StringFormatFlags.NoWrap;

                    int vpos = 5;
                    int colwidth = (int)BaseUtils.BitMapHelpers.MeasureStringInBitmap("0000", displayfont, frmt).Width;
                    int percentwidth = (int)BaseUtils.BitMapHelpers.MeasureStringInBitmap("00000.0", displayfont, frmt).Width;
                    int hmlwidth = percentwidth * 2;

                    int[] colsw = new int[] { colwidth * 4, colwidth, colwidth, colwidth, percentwidth, percentwidth, percentwidth, percentwidth, percentwidth, percentwidth, hmlwidth, colwidth };
                    int[] hpos = new int[colsw.Length];
                    hpos[0] = 4;
                    for (int i = 1; i < hpos.Length; i++)
                        hpos[i] = hpos[i - 1] + colsw[i - 1];

                    int limpetscolpos = 0;
                    if (eventlist.Count() > 0)
                    {
                        lastrefined = found.Sum(x => x.amountrefined);      // for use by timer

                        string timetext = TimeText(true);
                        if (timetext.HasChars())
                        {
                            timeie = pictureBox.AddTextAutoSize(new Point(hpos[0], vpos), new Size(colsw[0], this.Height), timetext, displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                            limpetscolpos = 1;
                        }
                    }
                    else
                        lastrefined = 0;

                    {
                        string text = string.Format("Limpets left {0}, Cargo left {1}".T(EDTx.UserControlMiningOverlay_Limcargo), limpetsleftdisplay, cargoleftdisplay);
                        if (collectorsused > 0 || prospectorsused > 0 || asteroidscracked > 0)
                            text += string.Format(", Prospectors Fired {0}, Collectors Deployed {1}, Cracked {2}".T(EDTx.UserControlMiningOverlay_Proscoll), prospectorsused, collectorsused, asteroidscracked);

                        var ieprosp = pictureBox.AddTextAutoSize(new Point(hpos[limpetscolpos], vpos), new Size(2000, this.Height),
                                    text, displayfont, textcolour, backcolour, 1.0F, frmt: frmt);

                        vpos = ieprosp.Location.Bottom + displayfont.ScalePixels(2);
                    }

                    bool displaytable = found.Count > 0 && (extCheckBoxZeroRefined.Checked ? lastrefined > 0 : true);

                    if (displaytable)
                    {
                        var ieheader = pictureBox.AddTextAutoSize(new Point(hpos[1], vpos), new Size(colsw[1], this.Height), "Ref.".T(EDTx.UserControlMiningOverlay_ref), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                        pictureBox.AddTextAutoSize(new Point(hpos[2], vpos), new Size(colsw[2], this.Height), "Coll.".T(EDTx.UserControlMiningOverlay_coll), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                        pictureBox.AddTextAutoSize(new Point(hpos[3], vpos), new Size(colsw[3], this.Height), "Prosp.".T(EDTx.UserControlMiningOverlay_pros), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);

                        pictureBox.AddTextAutoSize(new Point(hpos[4], vpos), new Size(colsw[4], this.Height), "Ratio%".T(EDTx.UserControlMiningOverlay_ratio), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                        pictureBox.AddTextAutoSize(new Point(hpos[5], vpos), new Size(colsw[5], this.Height), "Avg%".T(EDTx.UserControlMiningOverlay_avg), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                        pictureBox.AddTextAutoSize(new Point(hpos[6], vpos), new Size(colsw[6], this.Height), "Min%".T(EDTx.UserControlMiningOverlay_min), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                        pictureBox.AddTextAutoSize(new Point(hpos[7], vpos), new Size(colsw[7], this.Height), "Max%".T(EDTx.UserControlMiningOverlay_max), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                        pictureBox.AddTextAutoSize(new Point(hpos[8], vpos), new Size(colsw[8], this.Height), "M.Lode".T(EDTx.UserControlMiningOverlay_mload), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                        pictureBox.AddTextAutoSize(new Point(hpos[9], vpos), new Size(colsw[9], this.Height), "HML Ct.".T(EDTx.UserControlMiningOverlay_hml), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                        pictureBox.AddTextAutoSize(new Point(hpos[10], vpos), new Size(colsw[10], this.Height), "Discv".T(EDTx.UserControlMiningOverlay_discv), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                        vpos = ieheader.Location.Bottom + displayfont.ScalePixels(2);

                        if (prospected > 0)
                        {
                            var ie = pictureBox.AddTextAutoSize(new Point(hpos[0], vpos), new Size(colsw[0], this.Height), "Asteroids Pros.".T(EDTx.UserControlMiningOverlay_astpros), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                            pictureBox.AddTextAutoSize(new Point(hpos[3], vpos), new Size(colsw[3], this.Height), prospected.ToString("N0"), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                            pictureBox.AddTextAutoSize(new Point(hpos[9], vpos), new Size(colsw[9], this.Height), content[0].ToString("N0") + "/" + content[1].ToString("N0") + "/" + content[2].ToString("N0"), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                            vpos = ie.Location.Bottom + displayfont.ScalePixels(2);
                        }

                        foreach (var m in found)
                        {
                            if (!extCheckBoxZeroRefined.Checked || m.amountrefined > 0)
                            {
                                var ie1 = pictureBox.AddTextAutoSize(new Point(hpos[0], vpos), new Size(colsw[0], this.Height), m.friendlyname, displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                                pictureBox.AddTextAutoSize(new Point(hpos[1], vpos), new Size(colsw[1], this.Height), m.amountrefined.ToString("N0"), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                                pictureBox.AddTextAutoSize(new Point(hpos[2], vpos), new Size(colsw[2], this.Height), (m.amountcollected - m.amountdiscarded).ToString("N0"), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                                if (m.prospectednoasteroids > 0)
                                {
                                    pictureBox.AddTextAutoSize(new Point(hpos[3], vpos), new Size(colsw[3], this.Height), m.prospectednoasteroids.ToString("N0"), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);

                                    pictureBox.AddTextAutoSize(new Point(hpos[4], vpos), new Size(colsw[4], this.Height), (100.0 * (double)m.prospectednoasteroids / prospected).ToString("N1"), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                                    pictureBox.AddTextAutoSize(new Point(hpos[5], vpos), new Size(colsw[5], this.Height), m.prospectedamounts.Average().ToString("N1"), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                                    pictureBox.AddTextAutoSize(new Point(hpos[6], vpos), new Size(colsw[6], this.Height), m.prospectedamounts.Min().ToString("N1"), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                                    pictureBox.AddTextAutoSize(new Point(hpos[7], vpos), new Size(colsw[7], this.Height), m.prospectedamounts.Max().ToString("N1"), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                                    pictureBox.AddTextAutoSize(new Point(hpos[9], vpos), new Size(colsw[9], this.Height), m.content[0].ToString("N0") + "/" + m.content[1].ToString("N0") + "/" + m.content[2].ToString("N0"), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                                }

                                if (m.motherloadasteroids > 0)
                                {
                                    pictureBox.AddTextAutoSize(new Point(hpos[8], vpos), new Size(colsw[8], this.Height), m.motherloadasteroids.ToString("N0"), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                                }

                                if (m.discovered)
                                    pictureBox.AddTextAutoSize(new Point(hpos[10], vpos), new Size(colsw[10], this.Height), " *", displayfont, textcolour, backcolour, 1.0F, frmt: frmt);

                                vpos = ie1.Location.Bottom + displayfont.ScalePixels(2);
                            }
                        }
                    }

                    pictureBox.Render(true, new Size(0, vpos + displayfont.ScalePixels(8)));       // control is resized with a min height

                    var prospectedlist = found.Where(m => m.prospectednoasteroids > 0).ToList();

                    UpdateComboBox(prospectedlist.Select(m => m.friendlyname).ToList());

                    int? seli = selectedchart as int?;          // indicates select N items
                    int? selm = selectedchart is string ? prospectedlist.FindIndex(x => x.friendlyname == selectedchart as string) : default(int?);     // indicates select particular item

                    if (prospectedlist.Count > 0 && ((seli.HasValue && seli > 0) || selm.HasValue)) // if we have a chart, and we selected something
                    {
                        List<MaterialsFound> matdata = new List<MaterialsFound>();

                        if (seli.HasValue)      // if selected number of items
                        {
                            if (seli == chartfixeditems.Length - 1)     // seli is index into chart options (None,1-4,8) so if selected 8, then select 8, else select 1,2,3,4 by using the index
                                seli = 8;

                            for (int i = 0; i < seli.Value && i < prospectedlist.Count; i++)    // add items to matdata
                                matdata.Add(prospectedlist[i]);
                        }
                        else
                            matdata.Add(prospectedlist[selm.Value]);    // else add just this to mat data


                        chart = new ExtendedControls.ExtSafeChart();
                        chart.Name = "chart";
                        chart.Dock = DockStyle.Fill;
                        chart.BeginInit();
                        chart.AddChartArea("mining");
                        chart.EnableZoomMouseWheelX();
                        chart.ZoomMouseWheelXMinimumInterval = 1;
                        chart.SetXAxisInterval(DateTimeIntervalType.Auto, 5);
                        chart.SetXAxisTitle("Content %".T(EDTx.UserControlMiningOverlay_content));
                        chart.SetYAxisMaxMin(0, 100);
                        chart.SetYAxisTitle("% Above".T(EDTx.UserControlMiningOverlay_above));

                        string title = (matdata.Count == 1 ? (matdata[0].friendlyname + " ") : "") + (extCheckBoxChartBase.Checked ? "Distribution vs All".T(EDTx.UserControlMiningOverlay_distall) : "Distribution if Contains".T(EDTx.UserControlMiningOverlay_dist));
                        chart.AddTitle("main", title);

                        if (matdata.Count > 1)
                        {
                            chart.AddLegend("legend");
                        }

                        int mi = 0;
                        foreach (var material in matdata)
                        {
                            chart.AddSeries(material.friendlyname, "mining", SeriesChartType.Line, legend: "legend");
                            double min = material.prospectedamounts.Min();
                            int asteroids = extCheckBoxChartBase.Checked ? prospected : material.prospectednoasteroids;

                            //foreach (var x in material.prospectedamounts) System.Diagnostics.Debug.WriteLine($"Material {material.friendlyname} % {x}");

                            for (int percent = 0; percent < CFDbMax; percent++)        // for each % chance up to CFdbMax (50)
                            {
                                // if percent is less than the prospected amount minimum value don't show

                                if (percent >= (int)min)
                                {
                                    // here, see how many entries are at or above the percentage amount
                                    int numberabove = material.prospectedamounts.Count(x => x >= percent);
                                    // and therefore the chance is the number above / no of asteroids
                                    double chance = ((double)numberabove) / asteroids * 100.0;

                                    //System.Diagnostics.Debug.WriteLine($"Mining {material.friendlyname} %{percent} no {numberabove} {material.prospectednoasteroids} = {chance}, min is {min}");

                                    chart.AddPoint(new DataPoint(percent, chance), axislabel: percent.ToString());

                                    //series.Points.Add(new DataPoint(percent, chance));
                                    //series.Points[series.Points.Count - 1].AxisLabel = percent.ToString();

                                    if (numberabove == 0 && percent % 10 == 0)      // if we have no items, terminate data at next 10% interval - chart will size
                                        break;
                                }
                            }

                            mi++;
                        }

                        chart.EndInit();
                        chart.Theme(ExtendedControls.Theme.Current);

                        try
                        {
                            Controls.Add(chart);
                            Controls.SetChildIndex(chart, 0);       // #3471 seen an exception here, but I can't make it do it. Hide error
                        }
                        catch ( Exception ex)
                        {
                            System.Diagnostics.Trace.WriteLine($"Mining overlay Exception during add/set child {ex}");
                        }
                    }
                }
    
                buttonExtExcel.Enabled = found.Count != 0;
            }
            else
            {
                pictureBox.Render();
                buttonExtExcel.Enabled = false;
            }

            Update();
        }

        private void Timetimer_Tick(object sender, EventArgs e)
        {
            Font displayfont = ExtendedControls.Theme.Current.GetFont;
            Color textcolour = IsTransparentModeOn ? ExtendedControls.Theme.Current.SPanelColor : ExtendedControls.Theme.Current.LabelColor;
            Color backcolour = IsTransparentModeOn ? Color.Transparent : this.BackColor;

            using (StringFormat frmt = new StringFormat())
            {
                var lab = new ExtendedControls.ExtPictureBox.ImageElement();
                lab.TextAutoSize(timeie.Position, timeie.Size, TimeText(false), displayfont, textcolour, backcolour, 1.0F, frmt: frmt);
                pictureBox.RemoveItem(timeie, backcolour);
                pictureBox.AddItem(lab);
                timeie = lab;
            }
        }

        private string TimeText(bool setstart)
        {
            if (eventlist != null && eventlist.Count > 0 )
            {
                DateTime lasteventtime = eventlist.Last().EventTimeUTC;        // last event time
                bool inprogress = incurrentplay && (DateTime.UtcNow - lasteventtime) < new TimeSpan(0, 15, 0);      // N min we are still in progress, and we are in current play
                TimeSpan timemining = inprogress ? (DateTime.UtcNow - eventlist.First().EventTimeUTC) : (lasteventtime - eventlist.First().EventTimeUTC);
                string text = timemining.ToString(@"hh\:mm\:ss");
                if (lastrefined > 0 && timemining.TotalHours>0)
                {
                    double rate = lastrefined / timemining.TotalHours;
                    text += " " + rate.ToString("N1") + "t/Hr";
                }
                if (setstart && inprogress)
                    timetimer.Start();
                return text;
            }
            else
                return "";
        }


        #endregion

        #region Material detection

        class MaterialsFound
        {
            public string matnamefd2;               // material name
            public string friendlyname;

            public double amountrefined;            // amount refined. Double because i've seen entries with floats
            public double amountcollected;          // amount collected
            public double amountdiscarded;
            public bool discovered;

            public int prospectednoasteroids;      // prospector entry updates this
            public List<double> prospectedamounts;  // list of % against each asteroid for this material

            public int motherloadasteroids;         // number where in motherload.

            public int[] content;                  // number in each cat, high/med/low

            public static MaterialsFound Find(string fdname, List<MaterialsFound> list)
            {
                MaterialsFound mat = list.Find(x => x.matnamefd2.Equals(fdname, StringComparison.InvariantCultureIgnoreCase));
                if (mat == null)
                {
                    mat = new MaterialsFound();
                    mat.matnamefd2 = fdname;
                    mat.friendlyname = MaterialCommodityMicroResourceType.GetByFDName(fdname)?.TranslatedName ?? fdname;
                    mat.prospectedamounts = new List<double>();
                    mat.content = new int[3];
                    list.Add(mat);
                }

                return mat;
            }
        }

        private List<MaterialsFound> ReadHistory(out int prospectorsused, out int collectorsused, out int asteroidscracked, out int prospected, out int[] content)
        {
            prospectorsused = 0;
            collectorsused = 0;
            asteroidscracked = 0;
            prospected = 0;
            content = new int[3];
            var found = new List<MaterialsFound>();

            foreach (var e in eventlist)
            {
                //System.Diagnostics.Debug.WriteLine("{0} {1} {2}", e.Indexno, e.EventTimeUTC, e.EventSummary);

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
                                matpa.content[0] += pa.Content == JournalProspectedAsteroid.AsteroidContent.High ? 1 : 0;
                                matpa.content[1] += pa.Content == JournalProspectedAsteroid.AsteroidContent.Medium ? 1 : 0;
                                matpa.content[2] += pa.Content == JournalProspectedAsteroid.AsteroidContent.Low ? 1 : 0;

                                //System.Diagnostics.Debug.WriteLine("Prospected {0} {1} {2}", m.Name, m.Proportion, pa.Content );
                            }

                            if (pa.MotherlodeMaterial.HasChars())
                            {
                                var matpa = MaterialsFound.Find(pa.MotherlodeMaterial, found);
                                matpa.motherloadasteroids++;
                            }
                        }

                        content[0] += pa.Content == JournalProspectedAsteroid.AsteroidContent.High ? 1 : 0;
                        content[1] += pa.Content == JournalProspectedAsteroid.AsteroidContent.Medium ? 1 : 0;
                        content[2] += pa.Content == JournalProspectedAsteroid.AsteroidContent.Low ? 1 : 0;

                        break;
                    case JournalTypeEnum.LaunchDrone:
                        var ld = e.journalEntry as JournalLaunchDrone;
                        if (ld.Type == JournalLaunchDrone.DroneType.Collection)
                            collectorsused++;
                        else if (ld.Type == JournalLaunchDrone.DroneType.Prospector)
                            prospectorsused++;
                        break;
                    case JournalTypeEnum.MiningRefined:
                        var mr = e.journalEntry as JournalMiningRefined;
                        var matmr = MaterialsFound.Find(mr.Type, found);
                        matmr.amountrefined++;
                       // System.Diagnostics.Debug.WriteLine($"Mining {e.EventTimeUTC} Refined {matmr.amountrefined}");
                        break;
                    case JournalTypeEnum.MaterialCollected:
                        var mc = e.journalEntry as JournalMaterialCollected;
                        var matmc = MaterialsFound.Find(mc.Name, found);
                        matmc.amountcollected += mc.Count;
                       // System.Diagnostics.Debug.WriteLine($"Mining {e.EventTimeUTC} Collected {mc.Count} {matmc.amountcollected}");
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
                selectedchart = extComboBoxChartOptions.SelectedIndex = GetSetting(dbChart, 0);
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
                    selectedchart = extComboBoxChartOptions.SelectedIndex = GetSetting(dbChart, 0);      // not found, for some reason, remove and go back to defauklt
                }
            }
            
            extComboBoxChartOptions.SelectedIndexChanged += ExtComboBoxChartOptions_SelectedIndexChanged;
        }


        private void ExtComboBoxChartOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (extComboBoxChartOptions.SelectedIndex < chartfixeditems.Length)
            {
                selectedchart = extComboBoxChartOptions.SelectedIndex;
                PutSetting(dbChart, (int)selectedchart);
            }
            else
                selectedchart = extComboBoxChartOptions.Text;
            Display();
        }


        private void extCheckBoxZeroRefined_CheckedChanged(object sender, EventArgs e)
        {
            PutSetting(dbZeroRefined, extCheckBoxZeroRefined.Checked);
            Display();
        }

        private void extCheckBoxChartBase_CheckedChanged(object sender, EventArgs e)
        {
            PutSetting(dbChartBase, extCheckBoxChartBase.Checked);
            Display();
        }

        #endregion

        #region excel

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            Forms.ImportExportForm frm = new Forms.ImportExportForm();
            frm.Export( new string[] { "All" }, new Forms.ImportExportForm.ShowFlags[] { Forms.ImportExportForm.ShowFlags.ShowCSVOpenInclude });

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid(frm.Delimiter);


                var found = ReadHistory(out int prospectorsused, out int collectorsused, out int asteroidscracked, out int prospected, out int[] content);

                grd.GetPreHeader += delegate (int r)
                {
                    if (r == 0)
                        return new Object[] { "Limpets left ", limpetsleftdisplay, "Prospectors Fired", prospectorsused, "Collectors Deployed", collectorsused };
                    else if (r == 1)
                        return new object[0];
                    else
                        return null;
                };

                grd.GetSetsPad = delegate (int s, int r)
                {
                    return r < 2 ? new object[0] : null;
                };

                {
                    grd.GetSetsHeader.Add(delegate (int s, int r)
                    {
                        if (r == 0)
                            return new object[] { "", "Ref.", "Coll.", "Pros", "Ratio%", "Avg%", "Min%", "Max%", "M.Load", "High Ct", "Med Ct", "Low Ct", "Discv" };
                        else
                            return null;
                    });

                    grd.GetSetsData.Add(delegate (int s, int r)
                    {
                        if (r == 0)
                        {
                            return new object[] { "","","", prospected.ToString("N0",grd.FormatCulture),"","","","","",
                                              content[0].ToString("N0",grd.FormatCulture),
                                              content[1].ToString("N0",grd.FormatCulture),
                                              content[2].ToString("N0",grd.FormatCulture) };
                        }
                        else if (r <= found.Count)
                        {
                            MaterialsFound f = found[r - 1];
                            return new object[] { f.friendlyname, f.amountrefined, f.amountcollected-f.amountdiscarded,
                                              f.prospectednoasteroids>0 ? f.prospectednoasteroids.ToString("N0",grd.FormatCulture) : "" ,
                                              f.prospectednoasteroids>0 ? (100.0 * (double)f.prospectednoasteroids / prospected).ToString("N1",grd.FormatCulture) : "" ,
                                              f.prospectednoasteroids>0 ? f.prospectedamounts.Average().ToString("N1",grd.FormatCulture) : "",
                                              f.prospectednoasteroids>0 ? f.prospectedamounts.Min().ToString("N1",grd.FormatCulture) : "",
                                              f.prospectednoasteroids>0 ? f.prospectedamounts.Max().ToString("N1",grd.FormatCulture) : "",
                                              f.motherloadasteroids>0 ? f.motherloadasteroids.ToString("N0",grd.FormatCulture) : "" ,
                                              f.prospectednoasteroids>0 ? f.content[0].ToString("N0",grd.FormatCulture) :"",
                                              f.prospectednoasteroids>0 ? f.content[1].ToString("N0",grd.FormatCulture) : "",
                                              f.prospectednoasteroids>0 ? f.content[2].ToString("N0",grd.FormatCulture) : "",
                                              f.discovered ? "*" : ""  };
                        }
                        else
                            return null;
                    });
                }

                var prosmat = found.Where(x => x.prospectednoasteroids > 0).ToList();

                {
                    grd.GetSetsHeader.Add(delegate (int s, int r)
                    {
                        if (r == 0)
                        {
                            Object[] ret = new object[prosmat.Count + 1];
                            ret[0] = "CDFb";

                            for (int i = 0; i < prosmat.Count; i++)
                                ret[i + 1] = prosmat[i].friendlyname;

                            return ret;
                        }
                        else
                            return null;
                    });

                    grd.GetSetsData.Add(delegate (int s, int r)
                    {
                        if (r < CFDbMax)
                        {
                            Object[] ret = new object[prosmat.Count + 1];
                            int percent = r;
                            ret[0] = percent.ToString("N0") + "%";
                            for (int m = 0; m < prosmat.Count; m++)
                            {
                                if (prosmat[m].prospectednoasteroids > 0)
                                    ret[m + 1] = ((double)prosmat[m].prospectedamounts.Count(x => x >= percent) / prosmat[m].prospectednoasteroids * 100.0).ToString("N1");
                                else
                                    ret[m + 1] = "";
                            }

                            return ret;
                        }
                        else
                            return null;
                    });
                }

                {
                    const int precol = 4;

                    grd.GetSetsHeader.Add(delegate (int s, int r)
                    {
                        if (r == 0)
                        {
                            Object[] ret = new object[found.Count + precol];
                            ret[0] = "Event";
                            ret[1] = "Time";
                            ret[2] = "Content";
                            ret[3] = "Motherload";

                            for (int i = 0; i < prosmat.Count; i++)
                                ret[i + precol] = prosmat[i].friendlyname;

                            return ret;
                        }
                        else
                            return null;
                    });

                    var proslist = eventlist.Where(x => x.EntryType == JournalTypeEnum.ProspectedAsteroid).ToList();

                    grd.GetSetsData.Add(delegate (int s, int r)
                    {
                        if (r < proslist.Count)
                        {
                            var jp = proslist[r].journalEntry as JournalProspectedAsteroid;

                            Object[] ret = new object[prosmat.Count + precol];
                            ret[0] = (r + 1).ToString("N0");
                            ret[1] = jp.EventTimeUTC.ToStringZulu();
                            ret[2] = jp.Content.ToString();
                            ret[3] = MaterialCommodityMicroResourceType.GetByFDName(jp.MotherlodeMaterial)?.TranslatedName ?? jp.MotherlodeMaterial;

                            for (int m = 0; m < prosmat.Count; m++)
                            {
                                int mi = Array.FindIndex(jp.Materials, x => x.Name == prosmat[m].matnamefd2);
                                if (mi >= 0)
                                    ret[m + precol] = jp.Materials[mi].Proportion.ToString("N2");
                                else
                                    ret[m + precol] = "";
                            }
                            return ret;
                        }
                        else
                            return null;
                    });

                }

                grd.WriteGrid(frm.Path, frm.AutoOpen, FindForm());
            }


        }

        #endregion
    }
}
