/*
 * Copyright © 2017-2022 EDDiscovery development team
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

        #region Init

        public UserControlMiningOverlay()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "MiningOverlay";

            UpdateComboBox(null);

            var enumlisttt = new Enum[] { EDTx.UserControlMiningOverlay_extCheckBoxZeroRefined_ToolTip, EDTx.UserControlMiningOverlay_buttonExtExcel_ToolTip, EDTx.UserControlMiningOverlay_extComboBoxChartOptions_ToolTip };
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);
            extPanelRollUp.SetToolTip(toolTip);

            buttonExtExcel.Enabled = false;
            extCheckBoxZeroRefined.Checked = GetSetting(dbZeroRefined, false);
            extCheckBoxZeroRefined.CheckedChanged += new System.EventHandler(this.extCheckBoxZeroRefined_CheckedChanged);
            extPanelRollUp.PinState = GetSetting(dbRolledUp, true);

            timetimer = new Timer();
            timetimer.Interval = 1000;
            timetimer.Tick += Timetimer_Tick;
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
        public override bool DefaultTransparent { get { return true; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            extPanelRollUp.Visible = !on;
            pictureBox.BackColor = this.BackColor = curcol;
            Display();
        }

        public override void Closing()
        {
            timetimer.Stop();
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            PutSetting(dbRolledUp, extPanelRollUp.PinState);
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

        List<HistoryEntry> curlist;     // found events
        HistoryEntry heabove, hebelow;  // markers
        bool incurrentplay;             // true when heabove is at top of history AND its not a stop event - so we are in a play session
        object selectedchart = null;    // chart count (if int) or material (string)
        const int CFDbMax = 50;         // 0-N% on CFDB

        double lastrefined;             // used for timer display - refined count   
        ExtendedControls.ExtPictureBox.ImageElement timeie; // image element of time
        Timer timetimer;                // and its timer

        int limpetsleftdisplay = 0;     // used to track if we need to redisplay due to limpet change
        int cargoleftdisplay = 0;       // used to track what we wrote for cargo

        private void Display(HistoryEntry he)       // at he, see if changed
        {
            if ( he != null )
            {
                JournalTypeEnum[] boundevents = new JournalTypeEnum[] { JournalTypeEnum.Docked, JournalTypeEnum.Undocked, JournalTypeEnum.FSDJump, JournalTypeEnum.CarrierJump,
                                JournalTypeEnum.Touchdown, JournalTypeEnum.Liftoff };
                JournalTypeEnum[] miningevents = new JournalTypeEnum[] { JournalTypeEnum.AsteroidCracked, JournalTypeEnum.ProspectedAsteroid, JournalTypeEnum.LaunchDrone,
                                JournalTypeEnum.MiningRefined, JournalTypeEnum.MaterialCollected, JournalTypeEnum.MaterialDiscovered, JournalTypeEnum.MaterialDiscarded};

                var newlist = HistoryList.FilterBetween(discoveryform.history.EntryOrder(), he, x => boundevents.Contains(x.EntryType), y => miningevents.Contains(y.EntryType), out HistoryEntry newhebelow, out HistoryEntry newheabove);

                if (newlist != null)        // only if no history would we get null, unlikely since he has been tested, but still..
                {
                    int limpetsleft = discoveryform.history.MaterialCommoditiesMicroResources.Get(newheabove.MaterialCommodity,"drones")?.Count ?? 0;
                    int cargo = discoveryform.history.MaterialCommoditiesMicroResources.CargoCount(newheabove.MaterialCommodity);
                    int cargocap = newheabove.ShipInformation?.CargoCapacity() ?? 0;// so if we don't have a ShipInformation, use 0
                    int cargoleft = cargocap - cargo; 

                    // if no list, or diff no of items (due to new entry) or different start point, we reset and display, else we just quit as current is good
                    if (curlist == null || newlist.Count != curlist.Count || hebelow != newhebelow || limpetsleft != limpetsleftdisplay || cargoleft != cargoleftdisplay) 
                    {
                        curlist = newlist;
                        heabove = newheabove;
                        hebelow = newhebelow;
                        limpetsleftdisplay = limpetsleft;
                        cargoleftdisplay = cargoleft;
                        incurrentplay = heabove == discoveryform.history.GetLast && !boundevents.Contains(heabove.EntryType);
                        System.Diagnostics.Debug.WriteLine("Redisplay {0} current {1}", heabove.EntryNumber, incurrentplay);
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

        class MaterialsFound
        {
            public string matnamefd2;
            public string friendlyname;

            public double amountrefined;            // amount refined. Double because i've seen entries with floats
            public double amountcollected;          // amount collected
            public double amountdiscarded;
            public bool discovered;

            public int prospectednoasteroids;      // prospector entry updates this
            public List<double> prospectedamounts;

            public int motherloadasteroids;         // number where in motherload.

            public int[] content;                  // number in each cat, high/med/low

            public static MaterialsFound Find(string fdname, List<MaterialsFound> list)
            {
                MaterialsFound mat = list.Find(x => x.matnamefd2.Equals(fdname, StringComparison.InvariantCultureIgnoreCase));
                if (mat == null)
                {
                    mat = new MaterialsFound();
                    mat.matnamefd2 = fdname;
                    mat.friendlyname = MaterialCommodityMicroResourceType.GetByFDName(fdname)?.Name ?? fdname;
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

            foreach (var e in curlist)
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
                        break;
                    case JournalTypeEnum.MaterialCollected:
                        var mc = e.journalEntry as JournalMaterialCollected;
                        var matmc = MaterialsFound.Find(mc.Name, found);
                        matmc.amountcollected += mc.Count;
                        //System.Diagnostics.Debug.WriteLine("Collected {0} {1}", mc.Count, matmc.amountcollected);
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
            timetimer.Stop();       // redisplay, stop the time timer and lose the timeie.
            timeie = null;

            pictureBox.ClearImageList();
            Controls.RemoveByKey("chart");

            if (curlist != null)
            {
                var found = ReadHistory( out int prospectorsused, out int collectorsused, out int asteroidscracked, out int prospected, out int[] content);

                Font displayfont = ExtendedControls.Theme.Current.GetFont;
                Color textcolour = IsTransparentModeOn ? ExtendedControls.Theme.Current.SPanelColor : ExtendedControls.Theme.Current.LabelColor;
                Color backcolour = IsTransparentModeOn ? Color.Transparent : this.BackColor;
                Color GridC = ExtendedControls.Theme.Current.GridBorderLines;
                Color TextC = ExtendedControls.Theme.Current.GridCellText;
                Color BackC = ExtendedControls.Theme.Current.GridCellBack;
                Color[] LineC = new Color[] { ExtendedControls.Theme.Current.KnownSystemColor , ExtendedControls.Theme.Current.TextBlockHighlightColor,
                                                         Color.Blue, Color.Yellow, Color.Green, Color.Gray, Color.HotPink, Color.Teal };

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
                    if (curlist.Count() > 0)
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

                        var ieprosp = pictureBox.AddTextAutoSize(new Point(hpos[limpetscolpos], vpos), new Size(this.Width - hpos[limpetscolpos] - 20, this.Height),
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

                    int? seli = selectedchart as int?;
                    int? selm = selectedchart is string ? prospectedlist.FindIndex(x => x.friendlyname == selectedchart as string) : default(int?);

                    if (prospectedlist.Count > 0 && ((seli.HasValue && seli > 0) || selm.HasValue))
                    {
                        List<MaterialsFound> matdata = new List<MaterialsFound>();
                        if (seli.HasValue)
                        {
                            if (seli == chartfixeditems.Length - 1)
                                seli = LineC.Length;

                            for (int i = 0; i < seli.Value && i < prospectedlist.Count; i++)
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
                            chartarea.AxisX.Title = "Content %".T(EDTx.UserControlMiningOverlay_content);
                            chartarea.AxisX.TitleFont = displayfont;
                            chartarea.AxisX.TitleForeColor = TextC;
                            chartarea.AxisX.Interval = 10;
                            chartarea.AxisX.Minimum = 0;

                            chartarea.AxisY.LabelStyle.ForeColor = TextC;
                            chartarea.AxisY.LabelStyle.Font = displayfont;
                            chartarea.AxisY.MajorGrid.LineColor = GridC;
                            chartarea.AxisY.LineColor = GridC;      // axis (0 value) colour
                            chartarea.AxisY.Title = "% Above".T(EDTx.UserControlMiningOverlay_above);
                            chartarea.AxisY.TitleFont = displayfont;
                            chartarea.AxisY.TitleForeColor = TextC;
                            chartarea.AxisY.Interval = 10;
                            chartarea.AxisY.Maximum = 101;

                            chart.ChartAreas.Add(chartarea);

                            chart.Titles.Clear();
                            var title = new Title((matdata.Count == 1 ? (matdata[0].friendlyname + " ") : "") + "Distribution".T(EDTx.UserControlMiningOverlay_dist), Docking.Top, displayfont, TextC);
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
                                for (i = 0; i < CFDbMax; i++)        // 0 - fixed
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
            if (curlist != null && curlist.Count > 0 )
            {
                DateTime lasteventtime = curlist.Last().EventTimeUTC;        // last event time
                bool inprogress = incurrentplay && (DateTime.UtcNow - lasteventtime) < new TimeSpan(0, 15, 0);      // N min we are still in progress, and we are in current play
                TimeSpan timemining = inprogress ? (DateTime.UtcNow - curlist.First().EventTimeUTC) : (lasteventtime - curlist.First().EventTimeUTC);
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

        #endregion

        #region excel

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            Forms.ExportForm frm = new Forms.ExportForm();
            frm.Init(false, new string[] { "All" }, showflags: new Forms.ExportForm.ShowFlags[] { Forms.ExportForm.ShowFlags.DisableDateTime });

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid();
                grd.SetCSVDelimiter(frm.Comma);

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
                            return new object[] { "","","", prospected.ToString("N0"),"","","","","",
                                              content[0].ToString("N0"),
                                              content[1].ToString("N0"),
                                              content[2].ToString("N0") };
                        }
                        else if (r <= found.Count)
                        {
                            MaterialsFound f = found[r - 1];
                            return new object[] { f.friendlyname, f.amountrefined, f.amountcollected-f.amountdiscarded,
                                              f.prospectednoasteroids>0 ? f.prospectednoasteroids.ToString("N0") : "" ,
                                              f.prospectednoasteroids>0 ? (100.0 * (double)f.prospectednoasteroids / prospected).ToString("N1") : "" ,
                                              f.prospectednoasteroids>0 ? f.prospectedamounts.Average().ToString("N1") : "",
                                              f.prospectednoasteroids>0 ? f.prospectedamounts.Min().ToString("N1") : "",
                                              f.prospectednoasteroids>0 ? f.prospectedamounts.Max().ToString("N1") : "",
                                              f.motherloadasteroids>0 ? f.motherloadasteroids.ToString("N0") : "" ,
                                              f.prospectednoasteroids>0 ? f.content[0].ToString("N0") :"",
                                              f.prospectednoasteroids>0 ? f.content[1].ToString("N0") : "",
                                              f.prospectednoasteroids>0 ? f.content[2].ToString("N0") : "",
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

                    var proslist = curlist.Where(x => x.EntryType == JournalTypeEnum.ProspectedAsteroid).ToList();

                    grd.GetSetsData.Add(delegate (int s, int r)
                    {
                        if (r < proslist.Count)
                        {
                            var jp = proslist[r].journalEntry as JournalProspectedAsteroid;

                            Object[] ret = new object[prosmat.Count + precol];
                            ret[0] = (r + 1).ToString("N0");
                            ret[1] = jp.EventTimeUTC.ToStringZulu();
                            ret[2] = jp.Content.ToString();
                            ret[3] = MaterialCommodityMicroResourceType.GetByFDName(jp.MotherlodeMaterial)?.Name ?? jp.MotherlodeMaterial;

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
