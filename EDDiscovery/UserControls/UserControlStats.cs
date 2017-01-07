using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using EDDiscovery.EliteDangerous.JournalEvents;
using EDDiscovery.EliteDangerous;

namespace EDDiscovery.UserControls
{
    public partial class UserControlStats : UserControlCommonBase
    {
        private int displaynumber = 0;
        private EDDiscoveryForm discoveryform;
        private TravelHistoryControl travelhistorycontrol;

        public UserControlStats()
        {
            InitializeComponent();
            Name = "Statistics";
        }

        public override void Init( EDDiscoveryForm ed, int vn) //0=primary, 1 = first windowed version, etc
        {
            discoveryform = ed;
            travelhistorycontrol = ed.TravelControl;
            displaynumber = vn;
            discoveryform.OnNewEntry += AddNewEntry;
            travelhistorycontrol.OnTravelSelectionChanged += SelectionChanged;

            userControlStatsTimeScan.ScanMode = true;

        }

        public override void Closing()
        {
            discoveryform.OnNewEntry -= AddNewEntry;
            travelhistorycontrol.OnTravelSelectionChanged -= SelectionChanged;
        }

        private void AddNewEntry(HistoryEntry he, HistoryList hl)
        {
            Stats(he,hl);
        }

        public override void Display(HistoryEntry current, HistoryList history)
        {
            SelectionChanged(current, history);
        }

        public void SelectionChanged(HistoryEntry he, HistoryList hl)
        {
            Stats(he, hl);
        }

        private HistoryEntry last_he=null;
        private HistoryList last_hl=null;
        private void Stats(HistoryEntry he, HistoryList hl)
        {
            // Cache old History entry and list to use for events inside control.
            if (he == null)
                he = last_he;
            if (hl == null)
                hl = last_hl;

            if (he == null || hl == null)
                return;

            last_hl = hl;
            last_he = he;

            if (tabControlCustomStats.SelectedIndex == 0)
            {
                StatsGeneral(he, hl);
            }
            if (tabControlCustomStats.SelectedIndex == 1)
            {
                StatsTravel(he, hl);
            }
            if (tabControlCustomStats.SelectedIndex == 2)
            {
                StatsScan(he, hl);
            }
        }

        private void StatsGeneral(HistoryEntry he, HistoryList hl)
        {
            dataGridViewStats.Rows.Clear();

            if (he != null)
            {
                StatToDGV("Visits", hl.GetVisitsCount(he.System.name, he.System.id_edsm) + " to system " + he.System.name);
                StatToDGV("Jumps Before System", hl.GetFSDJumpsBeforeUTC(he.EventTimeUTC));
            }

            int totaljumps = hl.GetFSDJumps(new TimeSpan(10000, 0, 0, 0));
            StatToDGV("Total No of jumps: ", totaljumps);
            if (totaljumps > 0)
            {
                StatToDGV("Jump History", "24 Hours: " + hl.GetFSDJumps(new TimeSpan(1, 0, 0, 0)) +
                                      ", One Week: " + hl.GetFSDJumps(new TimeSpan(7, 0, 0, 0)) +
                                      ", 30 Days: " + hl.GetFSDJumps(new TimeSpan(30, 0, 0, 0)) +
                                      ", One Year: " + hl.GetFSDJumps(new TimeSpan(365, 0, 0, 0))
                                      );

                HistoryEntry north = hl.GetConditionally(Double.MinValue, (HistoryEntry s, ref double l) =>
                { bool v = s.IsFSDJump && s.System.HasCoordinate && s.System.z > l; if (v) l = s.System.z; return v; });

                HistoryEntry south = hl.GetConditionally(Double.MaxValue, (HistoryEntry s, ref double l) =>
                { bool v = s.IsFSDJump && s.System.HasCoordinate && s.System.z < l; if (v) l = s.System.z; return v; });

                HistoryEntry east = hl.GetConditionally(Double.MinValue, (HistoryEntry s, ref double l) =>
                { bool v = s.IsFSDJump && s.System.HasCoordinate && s.System.x > l; if (v) l = s.System.x; return v; });

                HistoryEntry west = hl.GetConditionally(Double.MaxValue, (HistoryEntry s, ref double l) =>
                { bool v = s.IsFSDJump && s.System.HasCoordinate && s.System.x < l; if (v) l = s.System.x; return v; });

                HistoryEntry up = hl.GetConditionally(Double.MinValue, (HistoryEntry s, ref double l) =>
                { bool v = s.IsFSDJump && s.System.HasCoordinate && s.System.y > l; if (v) l = s.System.y; return v; });

                HistoryEntry down = hl.GetConditionally(Double.MaxValue, (HistoryEntry s, ref double l) =>
                { bool v = s.IsFSDJump && s.System.HasCoordinate && s.System.y < l; if (v) l = s.System.y; return v; });

                StatToDGV("Most North", north.System.name + " @ " + north.System.x.ToString("0.0") + "," + north.System.y.ToString("0.0") + "," + north.System.z.ToString("0.0"));
                StatToDGV("Most South", south.System.name + " @ " + south.System.x.ToString("0.0") + "," + south.System.y.ToString("0.0") + "," + south.System.z.ToString("0.0"));
                StatToDGV("Most East", east.System.name + " @ " + east.System.x.ToString("0.0") + "," + east.System.y.ToString("0.0") + "," + east.System.z.ToString("0.0"));
                StatToDGV("Most West", west.System.name + " @ " + west.System.x.ToString("0.0") + "," + west.System.y.ToString("0.0") + "," + west.System.z.ToString("0.0"));
                StatToDGV("Most Highest", up.System.name + " @ " + up.System.x.ToString("0.0") + "," + up.System.y.ToString("0.0") + "," + up.System.z.ToString("0.0"));
                StatToDGV("Most Lowest", down.System.name + " @ " + down.System.x.ToString("0.0") + "," + down.System.y.ToString("0.0") + "," + down.System.z.ToString("0.0"));


                var groupeddata = from data in hl.OrderByDate
                                  where data.IsFSDJump
                                  group data by data.System.name
                                      into grouped
                                  select new
                                  {
                                      Title = grouped.Key,
                                      Count = grouped.Count()
                                  };

                mostVisited.Visible = true;

                Color GridC = discoveryform.theme.GridCellText;
                Color TextC = discoveryform.theme.VisitedSystemColor;
                mostVisited.Titles.Clear();
                mostVisited.Titles.Add(new Title("Most Visited", Docking.Top, discoveryform.theme.GetFont, TextC));
                mostVisited.Series[0].Points.Clear();

                mostVisited.ChartAreas[0].AxisX.LabelStyle.ForeColor = TextC;
                mostVisited.ChartAreas[0].AxisY.LabelStyle.ForeColor = TextC;
                mostVisited.ChartAreas[0].AxisX.MajorGrid.LineColor = GridC;
                mostVisited.ChartAreas[0].AxisX.MinorGrid.LineColor = GridC;
                mostVisited.ChartAreas[0].AxisY.MajorGrid.LineColor = GridC;
                mostVisited.ChartAreas[0].AxisY.MinorGrid.LineColor = GridC;
                mostVisited.ChartAreas[0].BorderColor = GridC;
                mostVisited.ChartAreas[0].BorderDashStyle = ChartDashStyle.Solid;
                mostVisited.ChartAreas[0].BorderWidth = 2;

                mostVisited.ChartAreas[0].BackColor = Color.Transparent;
                mostVisited.Series[0].Color = GridC;
                mostVisited.BorderlineColor = Color.Transparent;

                int i = 0;
                foreach (var data in from a in groupeddata orderby a.Count descending select a)
                {
                    if (data.Count <= 1 || i == 10)
                        break;

                    mostVisited.Series[0].Points.Add(new DataPoint(i, data.Count));
                    mostVisited.Series[0].Points[i].AxisLabel = data.Title;
                    mostVisited.Series[0].Points[i].LabelForeColor = TextC;
                    i++;
                }
            }
            else
                mostVisited.Visible = false;

            SizeControls();
        }

        void StatsTravel(HistoryEntry he, HistoryList hl)
        {
            int[] intar = null;
            string[] strarr = null;

            if (userControlStatsTimeTravel.TimeMode == UserControlStatsTimeModeEnum.Summary)
            {
                dataGridViewTravel.Rows.Clear();
                dataGridViewTravel.Columns.Clear();
                dataGridViewTravel.Dock = DockStyle.Fill;
                dataGridViewTravel.Visible = true;


                var Col1 = new DataGridViewTextBoxColumn();
                Col1.HeaderText = "Last";

                var Col2 = new DataGridViewTextBoxColumn();
                Col2.HeaderText = "24 hours";

                var Col3 = new DataGridViewTextBoxColumn();
                Col3.HeaderText = "week";

                var Col4 = new DataGridViewTextBoxColumn();
                Col4.HeaderText = "month";

                var Col5 = new DataGridViewTextBoxColumn();
                Col5.HeaderText = "all";

                dataGridViewTravel.Columns.AddRange(new DataGridViewColumn[] { Col1, Col2, Col3, Col4, Col5 });


                intar = new int[4];
                strarr = new string[4];


                intar[0] = hl.GetFSDJumps(DateTime.Now.AddDays(-1), DateTime.Now);
                intar[1] = hl.GetFSDJumps(DateTime.Now.AddDays(-7), DateTime.Now);
                intar[2] = hl.GetFSDJumps(DateTime.Now.AddMonths(-1), DateTime.Now);
                intar[3] = hl.GetFSDJumps(new DateTime(2012, 1, 1), DateTime.Now);
                StatToDGV(dataGridViewTravel, "Jumps", intar);

                strarr[0] = hl.GetTraveledLy(DateTime.Now.AddDays(-1), DateTime.Now).ToString("0.00");
                strarr[1] = hl.GetTraveledLy(DateTime.Now.AddDays(-7), DateTime.Now).ToString("0.00");
                strarr[2] = hl.GetTraveledLy(DateTime.Now.AddDays(-30), DateTime.Now).ToString("0.00");
                strarr[3] = hl.GetTraveledLy(new DateTime(2012, 1, 1), DateTime.Now).ToString("0.00");
                StatToDGV(dataGridViewTravel, "Traveled Ly", strarr);

                intar[0] = hl.GetFSDBoostUsed(DateTime.Now.AddDays(-1), DateTime.Now);
                intar[1] = hl.GetFSDBoostUsed(DateTime.Now.AddDays(-7), DateTime.Now);
                intar[2] = hl.GetFSDBoostUsed(DateTime.Now.AddDays(-30), DateTime.Now);
                intar[3] = hl.GetFSDBoostUsed(new DateTime(2012, 1, 1), DateTime.Now);
                StatToDGV(dataGridViewTravel, "Boost used", intar);



                intar[0] = hl.GetJetConeBoost(DateTime.Now.AddDays(-1), DateTime.Now);
                intar[1] = hl.GetJetConeBoost(DateTime.Now.AddDays(-7), DateTime.Now);
                intar[2] = hl.GetJetConeBoost(DateTime.Now.AddDays(-30), DateTime.Now);
                intar[3] = hl.GetJetConeBoost(new DateTime(2012, 1, 1), DateTime.Now);
                StatToDGV(dataGridViewTravel, "Jet Cone Boost", intar);

                intar[0] = hl.GetTouchDown(DateTime.Now.AddDays(-1), DateTime.Now);
                intar[1] = hl.GetTouchDown(DateTime.Now.AddDays(-7), DateTime.Now);
                intar[2] = hl.GetTouchDown(DateTime.Now.AddDays(-30), DateTime.Now);
                intar[3] = hl.GetTouchDown(new DateTime(2012, 1, 1), DateTime.Now);
                StatToDGV(dataGridViewTravel, "Landed", intar);


                intar[0] = hl.GetHeatWarning(DateTime.Now.AddDays(-1), DateTime.Now);
                intar[1] = hl.GetHeatWarning(DateTime.Now.AddDays(-7), DateTime.Now);
                intar[2] = hl.GetHeatWarning(DateTime.Now.AddDays(-30), DateTime.Now);
                intar[3] = hl.GetHeatWarning(new DateTime(2012, 1, 1), DateTime.Now);
                StatToDGV(dataGridViewTravel, "Heat Warning", intar);

                intar[0] = hl.GetHeatDamage(DateTime.Now.AddDays(-1), DateTime.Now);
                intar[1] = hl.GetHeatDamage(DateTime.Now.AddDays(-7), DateTime.Now);
                intar[2] = hl.GetHeatDamage(DateTime.Now.AddDays(-30), DateTime.Now);
                intar[3] = hl.GetHeatDamage(new DateTime(2012, 1, 1), DateTime.Now);
                StatToDGV(dataGridViewTravel, "Heat damage", intar);

                intar[0] = hl.GetFuelScooped(DateTime.Now.AddDays(-1), DateTime.Now);
                intar[1] = hl.GetFuelScooped(DateTime.Now.AddDays(-7), DateTime.Now);
                intar[2] = hl.GetFuelScooped(DateTime.Now.AddDays(-30), DateTime.Now);
                intar[3] = hl.GetFuelScooped(new DateTime(2012, 1, 1), DateTime.Now);
                StatToDGV(dataGridViewTravel, "Fuel Scooped", intar);

                strarr[0] = hl.GetFuelScoopedTons(DateTime.Now.AddDays(-1), DateTime.Now).ToString("0.00");
                strarr[1] = hl.GetFuelScoopedTons(DateTime.Now.AddDays(-7), DateTime.Now).ToString("0.00");
                strarr[2] = hl.GetFuelScoopedTons(DateTime.Now.AddDays(-30), DateTime.Now).ToString("0.00");
                strarr[3] = hl.GetFuelScoopedTons(new DateTime(2012, 1, 1), DateTime.Now).ToString("0.00");
                StatToDGV(dataGridViewTravel, "Scooped Tons", strarr);
            }
            else
            {
                int intervals = 10;
                DateTime[] timeintervals = new DateTime[intervals + 1];
                DateTime currentday = DateTime.Today;


                if (userControlStatsTimeTravel.TimeMode == UserControlStatsTimeModeEnum.Day)
                {
                    timeintervals[0] = currentday.AddDays(1);
                    for (int ii = 0; ii < intervals; ii++)
                        timeintervals[ii + 1] = timeintervals[ii].AddDays(-1);

                }
                else if (userControlStatsTimeTravel.TimeMode == UserControlStatsTimeModeEnum.Week)
                {
                    DateTime startOfWeek = currentday.AddDays(-1 * (int)(DateTime.Today.DayOfWeek -1));
                    timeintervals[0] = startOfWeek.AddDays(7);
                    for (int ii = 0; ii < intervals; ii++)
                        timeintervals[ii + 1] = timeintervals[ii].AddDays(-7);

                }
                else  // month
                {
                    DateTime startOfMonth = new DateTime(currentday.Year, currentday.Month, 1);
                    timeintervals[0] = startOfMonth.AddMonths(1);
                    for (int ii = 0; ii < intervals; ii++)
                        timeintervals[ii + 1] = timeintervals[ii].AddMonths(-1);
                }

                strarr = new string[intervals];

                dataGridViewTravel.Rows.Clear();
                dataGridViewTravel.Columns.Clear();
                dataGridViewTravel.Dock = DockStyle.Fill;
                dataGridViewTravel.Visible = true;


                var Col1 = new DataGridViewTextBoxColumn();
                Col1.HeaderText = "";

                dataGridViewTravel.Columns.Add(Col1);

                for (int ii = 0; ii < intervals; ii++)
                {
                    var Col2 = new DataGridViewTextBoxColumn();
                    Col2.HeaderText = timeintervals[ii+1].ToShortDateString();
                    dataGridViewTravel.Columns.Add(Col2);
                }


                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetFSDJumps(timeintervals[ii + 1], timeintervals[ii]).ToString();
                StatToDGV(dataGridViewTravel, "Jumps", strarr);


                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetTraveledLy(timeintervals[ii + 1], timeintervals[ii]).ToString("0.00");
                StatToDGV(dataGridViewTravel, "Traveled Ly", strarr);


                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetFSDBoostUsed(timeintervals[ii + 1], timeintervals[ii]).ToString();
                StatToDGV(dataGridViewTravel, "Boost used", strarr);


                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetJetConeBoost(timeintervals[ii + 1], timeintervals[ii]).ToString();
                StatToDGV(dataGridViewTravel, "Jet Cone Boost", strarr);

                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetDocked(timeintervals[ii + 1], timeintervals[ii]).ToString();
                    StatToDGV(dataGridViewTravel, "Docked", strarr);

                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetTouchDown(timeintervals[ii + 1], timeintervals[ii]).ToString();
                StatToDGV(dataGridViewTravel, "Landed", strarr);


                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetHeatWarning(timeintervals[ii + 1], timeintervals[ii]).ToString(); 
                StatToDGV(dataGridViewTravel, "Heat Warning", strarr);

                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetHeatDamage(timeintervals[ii + 1], timeintervals[ii]).ToString(); 
                StatToDGV(dataGridViewTravel, "Heat damage", strarr);

                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetFuelScooped(timeintervals[ii + 1], timeintervals[ii]).ToString();
                StatToDGV(dataGridViewTravel, "Fuel Scooped", strarr);

                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetFuelScoopedTons(timeintervals[ii + 1], timeintervals[ii]).ToString("0.00"); 
                StatToDGV(dataGridViewTravel, "Scooped Tons", strarr);




            }
        }

        void StatsScan(HistoryEntry he, HistoryList hl)
        {
            int[] intar = null;
            string[] strarr = null;
            int intervals;
            List<JournalScan>[] scanlists = null;

            if (userControlStatsTimeScan.TimeMode == UserControlStatsTimeModeEnum.Summary)
            {
                dataGridViewScan.Rows.Clear();
                dataGridViewScan.Columns.Clear();
                dataGridViewScan.Dock = DockStyle.Fill;
                dataGridViewScan.Visible = true;


                var Col1 = new DataGridViewTextBoxColumn();
                Col1.HeaderText = "Last";

                var Col2 = new DataGridViewTextBoxColumn();
                Col2.HeaderText = "24 hours";

                var Col3 = new DataGridViewTextBoxColumn();
                Col3.HeaderText = "week";

                var Col4 = new DataGridViewTextBoxColumn();
                Col4.HeaderText = "month";

                var Col5 = new DataGridViewTextBoxColumn();
                Col5.HeaderText = "all";

                dataGridViewScan.Columns.AddRange(new DataGridViewColumn[] { Col1, Col2, Col3, Col4, Col5 });


                intar = new int[4];
                strarr = new string[4];

                intervals = 4;
                scanlists = new List<JournalScan>[intervals];


                scanlists[0] = hl.GetScanList(DateTime.Now.AddDays(-1), DateTime.Now);
                scanlists[1] = hl.GetScanList(DateTime.Now.AddDays(-7), DateTime.Now);
                scanlists[2] = hl.GetScanList(DateTime.Now.AddMonths(-1), DateTime.Now);
                scanlists[3] = hl.GetScanList(new DateTime(2012, 1, 1), DateTime.Now);



            }
            else
            {
                intervals = 10;
                DateTime[] timeintervals = new DateTime[intervals + 1];
                DateTime currentday = DateTime.Today;


                if (userControlStatsTimeScan.TimeMode == UserControlStatsTimeModeEnum.Day)
                {
                    timeintervals[0] = currentday.AddDays(1);
                    for (int ii = 0; ii < intervals; ii++)
                        timeintervals[ii + 1] = timeintervals[ii].AddDays(-1);

                }
                else if (userControlStatsTimeScan.TimeMode == UserControlStatsTimeModeEnum.Week)
                {
                    DateTime startOfWeek = currentday.AddDays(-1 * (int)(DateTime.Today.DayOfWeek - 1));
                    timeintervals[0] = startOfWeek.AddDays(7);
                    for (int ii = 0; ii < intervals; ii++)
                        timeintervals[ii + 1] = timeintervals[ii].AddDays(-7);

                }
                else  // month
                {
                    DateTime startOfMonth = new DateTime(currentday.Year, currentday.Month, 1);
                    timeintervals[0] = startOfMonth.AddMonths(1);
                    for (int ii = 0; ii < intervals; ii++)
                        timeintervals[ii + 1] = timeintervals[ii].AddMonths(-1);
                }

                strarr = new string[intervals];

                dataGridViewScan.Rows.Clear();
                dataGridViewScan.Columns.Clear();
                dataGridViewScan.Dock = DockStyle.Fill;
                dataGridViewScan.Visible = true;


                var Col1 = new DataGridViewTextBoxColumn();
                Col1.HeaderText = "";

                dataGridViewScan.Columns.Add(Col1);

                for (int ii = 0; ii < intervals; ii++)
                {
                    var Col2 = new DataGridViewTextBoxColumn();
                    Col2.HeaderText = timeintervals[ii + 1].ToShortDateString();
                    dataGridViewScan.Columns.Add(Col2);
                }


                scanlists = new List<JournalScan>[intervals];
                for (int ii = 0; ii < intervals; ii++)
                    scanlists[ii] = hl.GetScanList(timeintervals[ii + 1], timeintervals[ii]);
            }


            if (userControlStatsTimeScan.Stars)
            {
                foreach (EDStar obj in Enum.GetValues(typeof(EDStar)))
                {
                    for (int ii = 0; ii < intervals; ii++)
                    {
                        int nr = 0;
                        for (int jj = 0; jj < scanlists[ii].Count; jj++)
                        {
                            if (scanlists[ii][jj].StarTypeID == obj && scanlists[ii][jj].IsStar)
                                nr++;
                        }

                        strarr[ii] = nr.ToString();

                    }
                    StatToDGV(dataGridViewScan, obj.ToString().Replace("_", " "), strarr, false);
                }
            }
            else
            {
                foreach (EDPlanet obj in Enum.GetValues(typeof(EDPlanet)))
                {
                    for (int ii = 0; ii < intervals; ii++)
                    {
                        int nr = 0;
                        for (int jj = 0; jj < scanlists[ii].Count; jj++)
                        {
                            if (scanlists[ii][jj].PlanetTypeID == obj && scanlists[ii][jj].IsStar == false)
                                nr++;
                        }

                        strarr[ii] = nr.ToString();

                    }
                    StatToDGV(dataGridViewScan, obj.ToString().Replace("_", " "), strarr, false);
                }
            }

        }


        void SizeControls()
        {
            try
            {
                int height = 0;
                foreach (DataGridViewRow row in dataGridViewStats.Rows)
                {
                    height += row.Height + 1;
                }
                height += dataGridViewStats.ColumnHeadersHeight + 2;
                dataGridViewStats.Size = new Size(Math.Max(10, panelData.DisplayRectangle.Width - panelData.ScrollBarWidth), height);             // all controls should be placed each time.
                                                                                                                                    //System.Diagnostics.Debug.WriteLine("DGV {0} {1}", dataGridViewStats.Size, dataGridViewStats.Location);
                mostVisited.Location = new Point(0, height);
                mostVisited.Size = new Size(Math.Max(10, panelData.DisplayRectangle.Width - panelData.ScrollBarWidth), mostVisited.Height);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"UserControlStats::SizeControls Exception: {ex.Message}");
                return;
            }

        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);       
            SizeControls();         // need to size controls here as well.. goes tabstrip.. create user control.. calls updatestats with incorrect size.. added to UC panel.. relayout
        }

        void StatToDGV(string title, string data)
        {
            object[] rowobj = { title, data };
            dataGridViewStats.Rows.Add(rowobj);
        }

        void StatToDGV(string title, int data)
        {
            object[] rowobj = { title, data.ToString() };
            dataGridViewStats.Rows.Add(rowobj);
        }

        void StatToDGV(DataGridView datagrid,  string title, string[] data, bool showEmptyLines = true)
        {
            object[] rowobj = new object[data.Length + 1];
            bool empty = true;

            rowobj[0] = title;
            for (int ii = 0; ii < data.Length; ii++)
            {
                rowobj[ii + 1] = data[ii];
                if (!data[ii].Equals("0") && !data[ii].Equals("0.00"))
                    empty = false;
            }

            if (showEmptyLines ||empty ==false)
                datagrid.Rows.Add(rowobj);
        }

        void StatToDGV(DataGridView datagrid, string title, int[] data)
        {
            object[] rowobj = new object[data.Length + 1];

            rowobj[0] = title;
            for (int ii = 0; ii < data.Length; ii++)
                rowobj[ii + 1] = data[ii].ToString();

            datagrid.Rows.Add(rowobj);
        }


        private void panelData_Paint(object sender, PaintEventArgs e)
        {

        }

        private void userControlStatsTimeTravel_TimeModeChanged(object sender, EventArgs e)
        {
            Stats(null, null);
        }

        private void userControlStatsTimeTravel_DrawModeChanged(object sender, EventArgs e)
        {
            Stats(null, null);
        }

        private void userControlStatsTimeScan_TimeModeChanged(object sender, EventArgs e)
        {
            Stats(null, null);
        }
    }
}
