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
using System.Windows.Forms.DataVisualization.Charting;
using EliteDangerousCore.JournalEvents;
using EliteDangerousCore;

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
                StatToDGV("Visits", hl.GetVisitsCount(he.System.name) + " to system " + he.System.name);
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

                StatToDGV("Most North", north.System.name + " @ " + north.System.x.ToString("0.0") + "; " + north.System.y.ToString("0.0") + "; " + north.System.z.ToString("0.0"));
                StatToDGV("Most South", south.System.name + " @ " + south.System.x.ToString("0.0") + "; " + south.System.y.ToString("0.0") + "; " + south.System.z.ToString("0.0"));
                StatToDGV("Most East", east.System.name + " @ " + east.System.x.ToString("0.0") + "; " + east.System.y.ToString("0.0") + "; " + east.System.z.ToString("0.0"));
                StatToDGV("Most West", west.System.name + " @ " + west.System.x.ToString("0.0") + "; " + west.System.y.ToString("0.0") + "; " + west.System.z.ToString("0.0"));
                StatToDGV("Most Highest", up.System.name + " @ " + up.System.x.ToString("0.0") + "; " + up.System.y.ToString("0.0") + "; " + up.System.z.ToString("0.0"));
                StatToDGV("Most Lowest", down.System.name + " @ " + down.System.x.ToString("0.0") + "; " + down.System.y.ToString("0.0") + "; " + down.System.z.ToString("0.0"));


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
            int intervals=0;
            DateTime[] timearr;
            DateTime endTime;


            if (userControlStatsTimeTravel.TimeMode == UserControlStatsTimeModeEnum.Summary || userControlStatsTimeTravel.TimeMode == UserControlStatsTimeModeEnum.Custom)
            {
                dataGridViewTravel.Rows.Clear();
                dataGridViewTravel.Columns.Clear();
                dataGridViewTravel.Dock = DockStyle.Fill;
                dataGridViewTravel.Visible = true;


                if (userControlStatsTimeTravel.TimeMode == UserControlStatsTimeModeEnum.Summary)
                {
                    intervals = 5;
                    var Col1 = new DataGridViewTextBoxColumn();
                    Col1.HeaderText = "Last";

                    var Col2 = new DataGridViewTextBoxColumn();
                    ColumnValueAlignment(Col2);
                    Col2.HeaderText = "24 hours";

                    var Col3 = new DataGridViewTextBoxColumn();
                    ColumnValueAlignment(Col3);
                    Col3.HeaderText = "week";

                    var Col4 = new DataGridViewTextBoxColumn();
                    ColumnValueAlignment(Col4);
                    Col4.HeaderText = "month";

                    var Col5 = new DataGridViewTextBoxColumn();
                    Col5.HeaderText = "Last dock";
                    ColumnValueAlignment(Col5);

                    var Col6 = new DataGridViewTextBoxColumn();
                    Col6.HeaderText = "all";
                    ColumnValueAlignment(Col6);

                    dataGridViewTravel.Columns.AddRange(new DataGridViewColumn[] { Col1, Col2, Col3, Col4, Col5, Col6 });


                    intar = new int[intervals];
                    strarr = new string[intervals];


                    timearr = new DateTime[intervals];


                    HistoryEntry lastdocked = hl.GetLastHistoryEntry(x => x.IsDocked);
                    DateTime lastdockTime = DateTime.Now;

                    if (lastdocked != null)
                        lastdockTime = lastdocked.EventTimeLocal;


                    timearr[0] = DateTime.Now.AddDays(-1);
                    timearr[1] = DateTime.Now.AddDays(-7);
                    timearr[2] = DateTime.Now.AddMonths(-1);
                    timearr[3] = lastdockTime;
                    timearr[4] = new DateTime(2012, 1, 1);

                    endTime = DateTime.Now;
                }
                else  // Custom
                {
                    intervals = 1;
                    var Col1 = new DataGridViewTextBoxColumn();
                    Col1.HeaderText = "";

                    var Col2 = new DataGridViewTextBoxColumn();
                    ColumnValueAlignment(Col2);
                    Col2.HeaderText = userControlStatsTimeTravel.CustomDateTimePickerFrom.Value.ToShortDateString() + " - " + userControlStatsTimeTravel.CustomDateTimePickerTo.Value.ToShortDateString();

                    dataGridViewTravel.Columns.AddRange(new DataGridViewColumn[] { Col1, Col2});


                    intar = new int[intervals];
                    strarr = new string[intervals];


                    timearr = new DateTime[intervals];


                    timearr[0] = userControlStatsTimeTravel.CustomDateTimePickerFrom.Value;
                    endTime = userControlStatsTimeTravel.CustomDateTimePickerTo.Value.AddDays(1);

                }



                for (int ii = 0; ii<intervals; ii++)
                    strarr[ii] = hl.GetFSDJumps(timearr[ii], endTime).ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                StatToDGV(dataGridViewTravel, "Jumps", strarr);

                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetTraveledLy(timearr[ii], endTime).ToString("N2", System.Globalization.CultureInfo.CurrentCulture);
                StatToDGV(dataGridViewTravel, "Traveled Ly", strarr);

                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetFSDBoostUsed(timearr[ii], endTime).ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                StatToDGV(dataGridViewTravel, "Boost used", strarr);

                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetJetConeBoost(timearr[ii], endTime).ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                StatToDGV(dataGridViewTravel, "Jet Cone Boost", strarr);

                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetTouchDown(timearr[ii], endTime).ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                StatToDGV(dataGridViewTravel, "Landed", strarr);


                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetHeatWarning(timearr[ii], endTime).ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                StatToDGV(dataGridViewTravel, "Heat Warning", strarr);

                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetHeatDamage(timearr[ii], endTime).ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                StatToDGV(dataGridViewTravel, "Heat damage", strarr);

                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetFuelScooped(timearr[ii], endTime).ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                StatToDGV(dataGridViewTravel, "Fuel Scooped", strarr);

                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetFuelScoopedTons(timearr[ii], endTime).ToString("N2", System.Globalization.CultureInfo.CurrentCulture);
                StatToDGV(dataGridViewTravel, "Scooped Tons", strarr);

                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetNrScans(timearr[ii], endTime).ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                StatToDGV(dataGridViewTravel, "Scans", strarr);

                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetScanValue(timearr[ii], endTime).ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                StatToDGV(dataGridViewTravel, "Scan value", strarr);


            }
            else
            {
                intervals = 10;
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
                    ColumnValueAlignment(Col2);
                    dataGridViewTravel.Columns.Add(Col2);
                }


                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetFSDJumps(timeintervals[ii + 1], timeintervals[ii]).ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                StatToDGV(dataGridViewTravel, "Jumps", strarr);


                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetTraveledLy(timeintervals[ii + 1], timeintervals[ii]).ToString("N2", System.Globalization.CultureInfo.CurrentCulture);
                StatToDGV(dataGridViewTravel, "Traveled Ly", strarr);


                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetFSDBoostUsed(timeintervals[ii + 1], timeintervals[ii]).ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                StatToDGV(dataGridViewTravel, "Boost used", strarr);


                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetJetConeBoost(timeintervals[ii + 1], timeintervals[ii]).ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                StatToDGV(dataGridViewTravel, "Jet Cone Boost", strarr);

                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetDocked(timeintervals[ii + 1], timeintervals[ii]).ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                StatToDGV(dataGridViewTravel, "Docked", strarr);

                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetTouchDown(timeintervals[ii + 1], timeintervals[ii]).ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                StatToDGV(dataGridViewTravel, "Landed", strarr);


                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetHeatWarning(timeintervals[ii + 1], timeintervals[ii]).ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                StatToDGV(dataGridViewTravel, "Heat Warning", strarr);

                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetHeatDamage(timeintervals[ii + 1], timeintervals[ii]).ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                StatToDGV(dataGridViewTravel, "Heat damage", strarr);

                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetFuelScooped(timeintervals[ii + 1], timeintervals[ii]).ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                StatToDGV(dataGridViewTravel, "Fuel Scooped", strarr);

                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetFuelScoopedTons(timeintervals[ii + 1], timeintervals[ii]).ToString("N2", System.Globalization.CultureInfo.CurrentCulture);
                StatToDGV(dataGridViewTravel, "Scooped Tons", strarr);

                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetNrScans(timeintervals[ii + 1], timeintervals[ii]).ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                StatToDGV(dataGridViewTravel, "Scans", strarr);


                for (int ii = 0; ii < intervals; ii++)
                    strarr[ii] = hl.GetScanValue(timeintervals[ii + 1], timeintervals[ii]).ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                StatToDGV(dataGridViewTravel, "Scan value", strarr);




            }
        }

        void StatsScan(HistoryEntry he, HistoryList hl)
        {
            int[] intar = null;
            string[] strarr = null;
            int intervals;
            List<JournalScan>[] scanlists = null;

            if (userControlStatsTimeScan.TimeMode == UserControlStatsTimeModeEnum.Summary || userControlStatsTimeScan.TimeMode == UserControlStatsTimeModeEnum.Custom)
            {
                dataGridViewScan.Rows.Clear();
                dataGridViewScan.Columns.Clear();
                dataGridViewScan.Dock = DockStyle.Fill;
                dataGridViewScan.Visible = true;

                if (userControlStatsTimeScan.TimeMode == UserControlStatsTimeModeEnum.Summary)
                {
                    var Col1 = new DataGridViewTextBoxColumn();
                    Col1.HeaderText = "Body Type";

                    var Col2 = new DataGridViewTextBoxColumn();
                    Col2.HeaderText = "24 hours";
                    ColumnValueAlignment(Col2);

                    var Col3 = new DataGridViewTextBoxColumn();
                    Col3.HeaderText = "week";
                    ColumnValueAlignment(Col3);

                    var Col4 = new DataGridViewTextBoxColumn();
                    Col4.HeaderText = "month";
                    ColumnValueAlignment(Col4);


                    var Col5 = new DataGridViewTextBoxColumn();
                    Col5.HeaderText = "Last dock";
                    ColumnValueAlignment(Col5);

                    var Col6 = new DataGridViewTextBoxColumn();
                    Col6.HeaderText = "all";
                    ColumnValueAlignment(Col6);

                    dataGridViewScan.Columns.AddRange(new DataGridViewColumn[] { Col1, Col2, Col3, Col4, Col5, Col6 });

                    intervals = 5;
                    intar = new int[intervals];
                    strarr = new string[intervals];


                    scanlists = new List<JournalScan>[intervals];

                    HistoryEntry lastdocked = hl.GetLastHistoryEntry(x => x.IsDocked);
                    DateTime lastdockTime = DateTime.Now;

                    if (lastdocked != null)
                        lastdockTime = lastdocked.EventTimeLocal;

                    scanlists[0] = hl.GetScanList(DateTime.Now.AddDays(-1), DateTime.Now);
                    scanlists[1] = hl.GetScanList(DateTime.Now.AddDays(-7), DateTime.Now);
                    scanlists[2] = hl.GetScanList(DateTime.Now.AddMonths(-1), DateTime.Now);
                    scanlists[3] = hl.GetScanList(lastdockTime, DateTime.Now);
                    scanlists[4] = hl.GetScanList(new DateTime(2012, 1, 1), DateTime.Now);
                }
                else
                {
                    var Col1 = new DataGridViewTextBoxColumn();
                    Col1.HeaderText = "";

                    var Col2 = new DataGridViewTextBoxColumn();
                    ColumnValueAlignment(Col2);
                    Col2.HeaderText = userControlStatsTimeScan.CustomDateTimePickerFrom.Value.ToShortDateString() + " - " + userControlStatsTimeScan.CustomDateTimePickerTo.Value.ToShortDateString();

                    dataGridViewScan.Columns.AddRange(new DataGridViewColumn[] { Col1, Col2 });

                    intervals = 1;
                    intar = new int[intervals];
                    strarr = new string[intervals];

                    scanlists = new List<JournalScan>[intervals];
                    scanlists[0] = hl.GetScanList(userControlStatsTimeScan.CustomDateTimePickerFrom.Value, userControlStatsTimeScan.CustomDateTimePickerTo.Value.AddDays(1));
                }


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
                Col1.HeaderText = "Body type";

                dataGridViewScan.Columns.Add(Col1);

                for (int ii = 0; ii < intervals; ii++)
                {
                    var Col2 = new DataGridViewTextBoxColumn();
                    Col2.HeaderText = timeintervals[ii + 1].ToShortDateString();
                    ColumnValueAlignment(Col2);
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

                        strarr[ii] = nr.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);

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

                        strarr[ii] = nr.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);

                    }
                    StatToDGV(dataGridViewScan, obj.ToString().Replace("_", " "), strarr, false);
                }
            }

        }

        private static void ColumnValueAlignment(DataGridViewTextBoxColumn Col2)
        {
            Col2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            Col2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            Col2.SortMode = DataGridViewColumnSortMode.NotSortable;
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

        private void dataGridViewStats_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
        }

        private void dataGridViewTravel_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewSorter2.DataGridSort2(dataGridViewTravel, e.ColumnIndex);
        }

        private void dataGridViewScan_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewSorter2.DataGridSort2(dataGridViewScan, e.ColumnIndex);
        }

        private void userControlStatsTimeTravel_Load(object sender, EventArgs e)
        {

        }
    }
}
