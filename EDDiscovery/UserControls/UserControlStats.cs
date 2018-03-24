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
using EliteDangerousCore.DB;

namespace EDDiscovery.UserControls
{
    public partial class UserControlStats : UserControlCommonBase
    {
        private string DbSelectedTabSave { get { return "StatsSelectedTab" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbStatsTreeStateSave { get { return "StatsTreeExpanded" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        
        public UserControlStats()
        {
            InitializeComponent();
            var corner = dataGridViewStats.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            tabControlCustomStats.SelectedIndex = SQLiteDBClass.GetSettingInt(DbSelectedTabSave, 0);
            userControlStatsTimeScan.ScanMode = true;
            discoveryform.OnNewEntry += AddNewEntry;
            uctg.OnTravelSelectionChanged += SelectionChanged;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= SelectionChanged;
            uctg = thc;
            uctg.OnTravelSelectionChanged += SelectionChanged;
        }

        public override void Closing()
        {
            SQLiteDBClass.PutSettingInt(DbSelectedTabSave, tabControlCustomStats.SelectedIndex);
            SQLiteDBClass.PutSettingString(DbStatsTreeStateSave, GameStatTreeState());
            discoveryform.OnNewEntry -= AddNewEntry;
            uctg.OnTravelSelectionChanged -= SelectionChanged;
        }

        private void AddNewEntry(HistoryEntry he, HistoryList hl)
        {
            Stats(he,hl);
        }

        public override void InitialDisplay()
        {
            SelectionChanged(uctg.GetCurrentHistoryEntry,discoveryform.history);
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
            if (tabControlCustomStats.SelectedIndex == 3)
            {
                StatsGame(he, hl);
            }
        }

        private void StatsGeneral(HistoryEntry he, HistoryList hl)
        {
            dataGridViewStats.Rows.Clear();

            if (he != null)
            {
                StatToDGV("Visits", hl.GetVisitsCount(he.System.Name) + " to system " + he.System.Name);
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
                { bool v = s.IsFSDJump && s.System.HasCoordinate && s.System.Z > l; if (v) l = s.System.Z; return v; });

                HistoryEntry south = hl.GetConditionally(Double.MaxValue, (HistoryEntry s, ref double l) =>
                { bool v = s.IsFSDJump && s.System.HasCoordinate && s.System.Z < l; if (v) l = s.System.Z; return v; });

                HistoryEntry east = hl.GetConditionally(Double.MinValue, (HistoryEntry s, ref double l) =>
                { bool v = s.IsFSDJump && s.System.HasCoordinate && s.System.X > l; if (v) l = s.System.X; return v; });

                HistoryEntry west = hl.GetConditionally(Double.MaxValue, (HistoryEntry s, ref double l) =>
                { bool v = s.IsFSDJump && s.System.HasCoordinate && s.System.X < l; if (v) l = s.System.X; return v; });

                HistoryEntry up = hl.GetConditionally(Double.MinValue, (HistoryEntry s, ref double l) =>
                { bool v = s.IsFSDJump && s.System.HasCoordinate && s.System.Y > l; if (v) l = s.System.Y; return v; });

                HistoryEntry down = hl.GetConditionally(Double.MaxValue, (HistoryEntry s, ref double l) =>
                { bool v = s.IsFSDJump && s.System.HasCoordinate && s.System.Y < l; if (v) l = s.System.Y; return v; });

                StatToDGV("Most North", north.System.Name + " @ " + north.System.X.ToString("0.0") + "; " + north.System.Y.ToString("0.0") + "; " + north.System.Z.ToString("0.0"));
                StatToDGV("Most South", south.System.Name + " @ " + south.System.X.ToString("0.0") + "; " + south.System.Y.ToString("0.0") + "; " + south.System.Z.ToString("0.0"));
                StatToDGV("Most East", east.System.Name + " @ " + east.System.X.ToString("0.0") + "; " + east.System.Y.ToString("0.0") + "; " + east.System.Z.ToString("0.0"));
                StatToDGV("Most West", west.System.Name + " @ " + west.System.X.ToString("0.0") + "; " + west.System.Y.ToString("0.0") + "; " + west.System.Z.ToString("0.0"));
                StatToDGV("Most Highest", up.System.Name + " @ " + up.System.X.ToString("0.0") + "; " + up.System.Y.ToString("0.0") + "; " + up.System.Z.ToString("0.0"));
                StatToDGV("Most Lowest", down.System.Name + " @ " + down.System.X.ToString("0.0") + "; " + down.System.Y.ToString("0.0") + "; " + down.System.Z.ToString("0.0"));


                var groupeddata = from data in hl.OrderByDate
                                  where data.IsFSDJump
                                  group data by data.System.Name
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
                    strarr[ii] = hl.GetPlayerControlledTouchDown(timearr[ii], endTime).ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
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
                    strarr[ii] = hl.GetPlayerControlledTouchDown(timeintervals[ii + 1], timeintervals[ii]).ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
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

        void StatsGame(HistoryEntry he, HistoryList hl)
        {
            string collapseExpand = GameStatTreeState();
            if (string.IsNullOrEmpty(collapseExpand)) collapseExpand = SQLiteDBClass.GetSettingString(DbStatsTreeStateSave, "YYYYYYYYYYYYY");
            if (collapseExpand.Length < 13) collapseExpand += new string('Y', 13);
            JournalStatistics stats = (JournalStatistics)hl.Where(j => j.EntryType == JournalTypeEnum.Statistics).OrderByDescending(j => j.EventTimeUTC).FirstOrDefault().journalEntry;
            if (stats != null)
            {
                treeViewStats.Nodes.Clear();
                TreeNode bank = treeViewStats.Nodes.Add("Bank Account");
                AddChildNode(bank, "Current Assets", stats.BankAccount?.CurrentWealth.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddChildNode(bank, "Spent on Ships", stats.BankAccount?.SpentOnShips.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddChildNode(bank, "Spent on Outfitting", stats.BankAccount?.SpentOnOutfitting.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddChildNode(bank, "Spent on Repairs", stats.BankAccount?.SpentOnRepairs.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddChildNode(bank, "Spent on Fuel", stats.BankAccount?.SpentOnFuel.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddChildNode(bank, "Spent on Munitions", stats.BankAccount?.SpentOnAmmoConsumables.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddChildNode(bank, "Insurance Claims", stats.BankAccount?.InsuranceClaims.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddChildNode(bank, "Total Claim Costs", stats.BankAccount?.SpentOnInsurance.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                if (collapseExpand.Substring(0,1) == "Y") bank.Expand();
                TreeNode combat = treeViewStats.Nodes.Add("Combat");
                AddChildNode(combat, "Bounties Claimed", stats.Combat?.BountiesClaimed.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "");
                AddChildNode(combat, "Profit from Bounty Hunting", stats.Combat?.BountyHuntingProfit.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddChildNode(combat, "Combat Bonds", stats.Combat?.CombatBonds.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(combat, "Profit from Combat Bonds", stats.Combat?.CombatBondProfits.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddChildNode(combat, "Assassinations", stats.Combat?.Assassinations.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(combat, "Profit from Assassination", stats.Combat?.AssassinationProfits.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddChildNode(combat, "Highest Single Reward", stats.Combat?.HighestSingleReward.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddChildNode(combat, "Skimmers Killed", stats.Combat?.SkimmersKilled.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                if (collapseExpand.Substring(1,1) == "Y") combat.Expand();
                TreeNode crime = treeViewStats.Nodes.Add("Crime");
                AddChildNode(crime, "Notoriety", stats.Crime?.Notoriety.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(crime, "Number of Fines", stats.Crime?.Fines.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(crime, "Lifetime Fines Value", stats.Crime?.TotalFines.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddChildNode(crime, "Bounties Received", stats.Crime?.BountiesReceived.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(crime, "Lifetime Bounty Value", stats.Crime?.TotalBounties.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddChildNode(crime, "Highest Bounty Issued", stats.Crime?.HighestBounty.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                if (collapseExpand.Substring(2,1) == "Y") crime.Expand();
                TreeNode smuggling = treeViewStats.Nodes.Add("Smuggling");
                AddChildNode(smuggling, "Black Market Network", stats.Smuggling?.BlackMarketsTradedWith.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(smuggling, "Black Market Profits", stats.Smuggling?.BlackMarketsProfits.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddChildNode(smuggling, "Commodities Smuggled", stats.Smuggling?.ResourcesSmuggled.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(smuggling, "Average Profit", stats.Smuggling?.AverageProfit.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddChildNode(smuggling, "Highest Single Transaction", stats.Smuggling?.HighestSingleTransaction.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                if (collapseExpand.Substring(3,1) == "Y") smuggling.Expand();
                TreeNode trading = treeViewStats.Nodes.Add("Trading");
                AddChildNode(trading, "Market Network", stats.Trading?.MarketsTradedWith.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(trading, "Market Profits", stats.Trading?.MarketProfits.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddChildNode(trading, "Commodities Traded", stats.Trading?.ResourcesTraded.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(trading, "Average Profit", stats.Trading?.AverageProfit.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddChildNode(trading, "Highest Single Transaction", stats.Trading?.HighestSingleTransaction.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                if (collapseExpand.Substring(4,1) == "Y") trading.Expand();
                TreeNode mining = treeViewStats.Nodes.Add("Mining");
                AddChildNode(mining, "Mining Profits", stats.Mining?.MiningProfits.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddChildNode(mining, "Materials Refined", stats.Mining?.QuantityMined.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(mining, "Materials Collected", stats.Mining?.MaterialsCollected.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                if (collapseExpand.Substring(5,1) == "Y") mining.Expand();
                TreeNode exploration = treeViewStats.Nodes.Add("Exploration");
                AddChildNode(exploration, "Systems Visited", stats.Exploration?.SystemsVisited.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(exploration, "Exploration Profits", stats.Exploration?.ExplorationProfits.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(exploration, "Level 2 Scans", stats.Exploration?.PlanetsScannedToLevel2.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(exploration, "Level 3 Scans", stats.Exploration?.PlanetsScannedToLevel3.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(exploration, "Highest Payout", stats.Exploration?.HighestPayout.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddChildNode(exploration, "Total Hyperspace Distance", stats.Exploration?.TotalHyperspaceDistance.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "ly");
                AddChildNode(exploration, "Total Hyperspace Jumps", stats.Exploration?.TotalHyperspaceJumps.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(exploration, "Farthest From Start", stats.Exploration?.GreatestDistanceFromStart.ToString("N2", System.Globalization.CultureInfo.CurrentCulture), "ly");
                AddChildNode(exploration, "Time Played", stats.Exploration?.TimePlayedDisplay());
                if (collapseExpand.Substring(6,1) == "Y") exploration.Expand();
                TreeNode passengers = treeViewStats.Nodes.Add("Passengers");
                AddChildNode(passengers, "Total Bulk Passengers Delivered", stats.PassengerMissions?.Bulk.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(passengers, "Total VIPs Delivered", stats.PassengerMissions?.VIP.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(passengers, "Delivered", stats.PassengerMissions?.Delivered.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(passengers, "Ejected", stats.PassengerMissions?.Ejected.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                if (collapseExpand.Substring(7,1) == "Y") passengers.Expand();
                TreeNode search = treeViewStats.Nodes.Add("Search and Rescue");
                AddChildNode(search, "Total Items Rescued", stats.SearchAndRescue?.Traded.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(search, "Total Profit", stats.SearchAndRescue?.Profit.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddChildNode(search, "Total Rescue Transactions", stats.SearchAndRescue?.Count.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                if (collapseExpand.Substring(8,1) == "Y") search.Expand();
                TreeNode craft = treeViewStats.Nodes.Add("Crafting");
                AddChildNode(craft, "Engineers Used", stats.Crafting?.CountOfUsedEngineers.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(craft, "Total Recipes Generated", stats.Crafting?.RecipesGenerated.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(craft, "Grade 1 Recipes Generated", stats.Crafting?.RecipesGeneratedRank1.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(craft, "Grade 2 Recipes Generated", stats.Crafting?.RecipesGeneratedRank2.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(craft, "Grade 3 Recipes Generated", stats.Crafting?.RecipesGeneratedRank3.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(craft, "Grade 4 Recipes Generated", stats.Crafting?.RecipesGeneratedRank4.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(craft, "Grade 5 Recipes Generated", stats.Crafting?.RecipesGeneratedRank5.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                if (collapseExpand.Substring(9,1) == "Y") craft.Expand();
                TreeNode crew = treeViewStats.Nodes.Add("Crew"); 
                AddChildNode(crew, "Total Wages", stats.Crew?.NpcCrewTotalWages.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddChildNode(crew, "Total Hired", stats.Crew?.NpcCrewHired.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(crew, "Total Fired", stats.Crew?.NpcCrewHired.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(crew, "Died in Line of Duty", stats.Crew?.NpcCrewDied.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                if (collapseExpand.Substring(10,1) == "Y") crew.Expand();
                TreeNode multicrew = treeViewStats.Nodes.Add("Multi-crew");
                AddChildNode(multicrew, "Total Time", SecondsToDHMString(stats.Multicrew?.TimeTotal));
                AddChildNode(multicrew, "Fighter Time", SecondsToDHMString(stats.Multicrew?.FighterTimeTotal));
                AddChildNode(multicrew, "Gunner Time", SecondsToDHMString(stats.Multicrew?.GunnerTimeTotal));
                AddChildNode(multicrew, "Credits Made", stats.Multicrew?.CreditsTotal.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddChildNode(multicrew, "Fines Accrued", stats.Multicrew?.FinesTotal.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                if (collapseExpand.Substring(11,1) == "Y") multicrew.Expand();
                TreeNode mattrader = treeViewStats.Nodes.Add("Materials Trader");
                AddChildNode(mattrader, "Trades Completed", stats.MaterialTraderStats?.TradesCompleted.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddChildNode(mattrader, "Materials Traded", stats.MaterialTraderStats?.MaterialsTraded.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                if (collapseExpand.Substring(12,1) == "Y") mattrader.Expand();
            }
        }

        string SecondsToDHMString(int? seconds)
        {
            if (!seconds.HasValue) return "";

            TimeSpan time = TimeSpan.FromSeconds(seconds.Value);
            return $"{time.Days} days {time.Hours} hours {time.Minutes} minutes";
        }

        void AddChildNode(TreeNode parent, string name, string value, string units = "")
        {
            parent.Nodes.Add($"{name}:  {(String.IsNullOrEmpty(value) ? "0" : value)} {units}");
        }

        string GameStatTreeState()
        {
            string result = "";
            if (treeViewStats.Nodes.Count > 0)
            {
                foreach(TreeNode tn in treeViewStats.Nodes) result += tn.IsExpanded ? "Y" : "N";
            }
            else
                result = SQLiteDBClass.GetSettingString(DbStatsTreeStateSave, "YYYYYYYYYYYYY");
            return result;
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

        private void tabControlCustomStats_SelectedIndexChanged(object sender, EventArgs e)
        {
            Stats(null, null);
        }
    }
}
