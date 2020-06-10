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
using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace EDDiscovery.UserControls
{
    public partial class UserControlStats : UserControlCommonBase
    {
        private string DbSelectedTabSave { get { return DBName("StatsSelectedTab"); } }
        private string DbStatsTreeStateSave { get { return DBName("StatsTreeExpanded"); } }
        private Chart mostVisited { get; set; }
        bool wasTravelling;

        #region Init

        public UserControlStats()
        {
            InitializeComponent();
            userControlStatsTimeScan.AutoScaleMode = AutoScaleMode.Inherit;
            userControlStatsTimeTravel.AutoScaleMode = AutoScaleMode.Inherit;

            var corner = dataGridViewStats.TopLeftHeaderCell; // work around #1487

            try
            {
                Chart chart = new Chart();
                ChartArea chartArea1 = new ChartArea();
                Series series1 = new Series();
                chart.BeginInit();
                chart.BorderlineDashStyle = ChartDashStyle.Solid;
                chartArea1.Name = "ChartArea1";
                chart.ChartAreas.Add(chartArea1);
                chart.Location = new Point(3, 250);
                chart.Name = "mostVisited";
                series1.ChartArea = "ChartArea1";
                series1.Name = "Series1";
                chart.Series.Add(series1);
                chart.Size = new Size(482, 177);
                chart.TabIndex = 5;
                chart.Text = "Most Visited";
                this.panelGeneral.Controls.Add(chart);
                this.mostVisited = chart;
                chart.EndInit();
            }
            catch (NotImplementedException)
            {
                // Charting not implemented in mono System.Windows.Forms
            }
        }

        public override void Init()
        {
            tabControlCustomStats.SelectedIndex = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(DbSelectedTabSave, 0);
            userControlStatsTimeScan.EnableDisplayStarsPlanetSelector();
            discoveryform.OnNewEntry += AddNewEntry;
            BaseUtils.Translator.Instance.Translate(this);
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += TravelGridChanged;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= TravelGridChanged;
            uctg = thc;
            uctg.OnTravelSelectionChanged += TravelGridChanged;
        }

        public override void Closing()
        {
            UserDatabase.Instance.PutSettingInt(DbSelectedTabSave, tabControlCustomStats.SelectedIndex);
            UserDatabase.Instance.PutSettingString(DbStatsTreeStateSave, GameStatTreeState());
            uctg.OnTravelSelectionChanged -= TravelGridChanged;
            discoveryform.OnNewEntry -= AddNewEntry;
        }

        public override void InitialDisplay() =>
            Stats(uctg.GetCurrentHistoryEntry, discoveryform.history, true);

        public void TravelGridChanged(HistoryEntry he, HistoryList hl, bool selectedEntry) =>
            Stats(he, hl, false);

        private void AddNewEntry(HistoryEntry he, HistoryList hl) =>
            Stats(he, hl, false);

        private void tabControlCustomStats_SelectedIndexChanged(object sender, EventArgs e) =>
            Stats(last_he, last_hl, true);

        private HistoryEntry last_he = null;
        private HistoryList last_hl = null;

        private void Stats(HistoryEntry he, HistoryList hl, bool forceupdate)     // paint new stats..
        {
            if (hl == null)
                return;

            if (tabControlCustomStats.SelectedIndex == 0)
            {
                if (forceupdate || he == null || last_he == null || hl.IsBetween(last_he, he, x => x.IsLocOrJump))
                    StatsGeneral(he, hl);
            }
            else if (tabControlCustomStats.SelectedIndex == 1)
            {
                if (forceupdate || he == null || last_he == null || hl.AnyBetween(last_he, he, journalsForStatsTravel))
                    StatsTravel(hl);
            }
            else if (tabControlCustomStats.SelectedIndex == 2)
            {
                if (forceupdate || he == null || last_he == null || hl.AnyBetween(last_he, he, new[] { JournalTypeEnum.Scan }))
                    StatsScan(hl);
            }
            else if (tabControlCustomStats.SelectedIndex == 3)
            {
                if (forceupdate || he == null || last_he == null || hl.AnyBetween(last_he, he, new[] { JournalTypeEnum.Statistics }))
                    StatsGame(he, hl);
            }
            else if (tabControlCustomStats.SelectedIndex == 4)
            {
                if (forceupdate || he == null || last_he == null || hl.AnyBetween(last_he, he, journalsForShipStats))
                    StatsByShip(hl);
            }

            last_hl = hl;
            last_he = he;
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Stats General

        private void StatsGeneral(HistoryEntry he, HistoryList hl)
        {
            if (he != null)
            {
                StatToDGVStats("Visits".T(EDTx.UserControlStats_Visits), hl.GetVisitsCount(he.System.Name) + " to system ".T(EDTx.UserControlStats_tosystem) + he.System.Name);
                StatToDGVStats("Jumps Before System".T(EDTx.UserControlStats_JumpsBeforeSystem), hl.GetFSDCarrierJumpsBeforeUTC(he.EventTimeUTC) + " to system ".T(EDTx.UserControlStats_tosystem) + he.System.Name);
            }

            int totaljumps = hl.GetFSDCarrierJumps(new TimeSpan(10000, 0, 0, 0));
            StatToDGVStats("Total No of jumps: ".T(EDTx.UserControlStats_TotalNoofjumps), totaljumps);
            if (totaljumps > 0)
            {
                StatToDGVStats("Jump History".T(EDTx.UserControlStats_JumpHistory), "24 Hours: ".T(EDTx.UserControlStats_24hc) + hl.GetFSDCarrierJumps(new TimeSpan(1, 0, 0, 0)) +
                                      ", One Week: ".T(EDTx.UserControlStats_OneWeek) + hl.GetFSDCarrierJumps(new TimeSpan(7, 0, 0, 0)) +
                                      ", 30 Days: ".T(EDTx.UserControlStats_30Days) + hl.GetFSDCarrierJumps(new TimeSpan(30, 0, 0, 0)) +
                                      ", One Year: ".T(EDTx.UserControlStats_OneYear) + hl.GetFSDCarrierJumps(new TimeSpan(365, 0, 0, 0))
                                      );

                HistoryEntry north = hl.GetConditionally(double.MinValue, (HistoryEntry s, ref double l) =>
                { bool v = s.IsFSDCarrierJump && s.System.HasCoordinate && s.System.Z > l; if (v) l = s.System.Z; return v; });

                HistoryEntry south = hl.GetConditionally(double.MaxValue, (HistoryEntry s, ref double l) =>
                { bool v = s.IsFSDCarrierJump && s.System.HasCoordinate && s.System.Z < l; if (v) l = s.System.Z; return v; });

                HistoryEntry east = hl.GetConditionally(double.MinValue, (HistoryEntry s, ref double l) =>
                { bool v = s.IsFSDCarrierJump && s.System.HasCoordinate && s.System.X > l; if (v) l = s.System.X; return v; });

                HistoryEntry west = hl.GetConditionally(double.MaxValue, (HistoryEntry s, ref double l) =>
                { bool v = s.IsFSDCarrierJump && s.System.HasCoordinate && s.System.X < l; if (v) l = s.System.X; return v; });

                HistoryEntry up = hl.GetConditionally(double.MinValue, (HistoryEntry s, ref double l) =>
                { bool v = s.IsFSDCarrierJump && s.System.HasCoordinate && s.System.Y > l; if (v) l = s.System.Y; return v; });

                HistoryEntry down = hl.GetConditionally(double.MaxValue, (HistoryEntry s, ref double l) =>
                { bool v = s.IsFSDCarrierJump && s.System.HasCoordinate && s.System.Y < l; if (v) l = s.System.Y; return v; });

                StatToDGVStats("Most North".T(EDTx.UserControlStats_MostNorth), GetSystemDataString(north));
                StatToDGVStats("Most South".T(EDTx.UserControlStats_MostSouth), GetSystemDataString(south));
                StatToDGVStats("Most East".T(EDTx.UserControlStats_MostEast), GetSystemDataString(east));
                StatToDGVStats("Most West".T(EDTx.UserControlStats_MostWest), GetSystemDataString(west));
                StatToDGVStats("Most Highest".T(EDTx.UserControlStats_MostHighest), GetSystemDataString(up));
                StatToDGVStats("Most Lowest".T(EDTx.UserControlStats_MostLowest), GetSystemDataString(down));

                if (mostVisited != null)
                {
                    var groupeddata = from data in hl.OrderByDate
                                      where data.IsFSDCarrierJump
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
                    mostVisited.Titles.Add(new Title("Most Visited".T(EDTx.UserControlStats_MostVisited), Docking.Top, discoveryform.theme.GetFont, TextC));
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
                    foreach (var data in groupeddata.OrderByDescending(g => g.Count).Take(10).Where(g => g.Count > 1))
                    {
                        mostVisited.Series[0].Points.Add(new DataPoint(i, data.Count));
                        mostVisited.Series[0].Points[i].AxisLabel = data.Title;
                        mostVisited.Series[0].Points[i].LabelForeColor = TextC;
                        i++;
                    }
                }
            }
            else if (mostVisited != null)
            {
                mostVisited.Visible = false;
            }

            PerformLayout();
        }

        private string GetSystemDataString(HistoryEntry he)
        {
            if (he == null)
            {
                return "N/A";
            }
            else
            {
                return he.System.Name + " @ " + he.System.X.ToString("0.0") + "; " + he.System.Y.ToString("0.0") + "; " + he.System.Z.ToString("0.0");
            }
        }

        void StatToDGVStats(string title, string data)
        {
            int rowpresent = dataGridViewStats.FindRowWithValue(0, title);
            if (rowpresent != -1)
                dataGridViewStats.Rows[rowpresent].Cells[1].Value = data;
            else
                dataGridViewStats.Rows.Add(new object[] { title, data });
        }

        void StatToDGVStats(string title, int data)
        {
            StatToDGVStats(title, data.ToString());
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Travel Panel

        static JournalTypeEnum[] journalsForStatsTravel = new JournalTypeEnum[]
        {
            JournalTypeEnum.FSDJump,
            JournalTypeEnum.Docked,
            JournalTypeEnum.Undocked,
            JournalTypeEnum.JetConeBoost,
            JournalTypeEnum.Scan,
            JournalTypeEnum.SAAScanComplete,
        };

        void StatsTravel(HistoryList hl)
        {
            int sortcol = dataGridViewTravel.SortedColumn?.Index ?? 99;
            SortOrder sortorder = dataGridViewTravel.SortOrder;

            if (userControlStatsTimeTravel.TimeMode == StatsTimeUserControl.TimeModeType.Summary || userControlStatsTimeTravel.TimeMode == StatsTimeUserControl.TimeModeType.Custom)
            {
                int intervals;
                DateTime[] timearrutc;
                DateTime endTimeutc;

                if (userControlStatsTimeTravel.TimeMode == StatsTimeUserControl.TimeModeType.Summary)   // Summary
                {
                    var isTravelling = hl.IsTravellingUTC(out var tripStartutc);
                    intervals = isTravelling ? 6 : 5;
                    if (isTravelling != wasTravelling)
                        dataGridViewTravel.Columns.Clear();

                    wasTravelling = isTravelling;
                    if (dataGridViewTravel.Columns.Count == 0)  // if DGV is clear..
                    {
                        var cols = new List<DataGridViewColumn>(intervals + 1);
                        cols.Add(new DataGridViewTextBoxColumn()
                        {
                            HeaderText = "Type".T(EDTx.UserControlStats_Type),
                            Tag = "AlphaSort"
                        });


                        cols.Add(new DataGridViewTextBoxColumn
                        {
                            HeaderText = "24 Hours".T(EDTx.UserControlStats_24Hours)
                        });

                        cols.Add(new DataGridViewTextBoxColumn
                        {
                            HeaderText = "Week".T(EDTx.UserControlStats_Week)
                        });

                        cols.Add(new DataGridViewTextBoxColumn
                        {
                            HeaderText = "Month".T(EDTx.UserControlStats_Month)
                        });

                        cols.Add(new DataGridViewTextBoxColumn
                        {
                            HeaderText = "Last dock".T(EDTx.UserControlStats_Lastdock)
                        });

                        if (isTravelling)
                            cols.Add(new DataGridViewTextBoxColumn
                            {
                                HeaderText = "Trip".T(EDTx.UserControlStats_Trip)
                            });

                        cols.Add(new DataGridViewTextBoxColumn
                        {
                            HeaderText = "All".T(EDTx.UserControlStats_All)
                        });

                        dataGridViewTravel.Columns.AddRange(cols.ToArray());
                    }

                    timearrutc = new DateTime[intervals];

                    timearrutc[0] = DateTime.UtcNow.AddDays(-1);
                    timearrutc[1] = DateTime.UtcNow.AddDays(-7);
                    timearrutc[2] = DateTime.UtcNow.AddMonths(-1);

                    HistoryEntry lastdocked = hl.GetLastHistoryEntry(x => x.IsDocked);
                    DateTime lastdockTimeutc = (lastdocked != null) ? lastdocked.EventTimeUTC : DateTime.UtcNow;
                    timearrutc[3] = lastdockTimeutc;

                    if (isTravelling)
                    {
                        timearrutc[4] = tripStartutc;
                        timearrutc[5] = new DateTime(2012, 1, 1);
                    }
                    else
                    {
                        timearrutc[4] = new DateTime(2012, 1, 1);
                    }

                    endTimeutc = DateTime.UtcNow;
                }
                else  // Custom
                {
                    intervals = 1;

                    if (dataGridViewTravel.Columns.Count == 0)  // if DGV is clear..
                    {
                        var Col1 = new DataGridViewTextBoxColumn();
                        Col1.HeaderText = "Type".T(EDTx.UserControlStats_Type);
                        Col1.Tag = "AlphaSort";

                        var Col2 = new DataGridViewTextBoxColumn();
                        Col2.HeaderText = userControlStatsTimeTravel.CustomDateTimePickerFrom.Value.ToShortDateString() + " - " + userControlStatsTimeTravel.CustomDateTimePickerTo.Value.ToShortDateString();

                        dataGridViewTravel.Columns.AddRange(new DataGridViewColumn[] { Col1, Col2 });
                    }

                    timearrutc = new DateTime[intervals];
                    timearrutc[0] = EDDConfig.Instance.ConvertTimeToUTCFromSelected(userControlStatsTimeTravel.CustomDateTimePickerFrom.Value);
                    endTimeutc = EDDConfig.Instance.ConvertTimeToUTCFromSelected(userControlStatsTimeTravel.CustomDateTimePickerTo.Value.AddDays(1));
                    System.Diagnostics.Debug.WriteLine("Date " + timearrutc[0].ToString() + "-" + endTimeutc.ToString());
                }

                var jumps = new string[intervals];
                var distances = new string[intervals];
                var basicBoosts = new string[intervals];
                var standardBoosts = new string[intervals];
                var premiumBoosts = new string[intervals];
                var jetBoosts = new string[intervals];
                var scanned = new string[intervals];
                var mapped = new string[intervals];
                var ucValue = new string[intervals];

                for (var ii = 0; ii < intervals; ii++)
                {
                    var fsdStats = hl.GetFsdJumpStatistics(timearrutc[ii], endTimeutc);
                    var scanStats = hl.GetScanCountAndValueUTC(timearrutc[ii], endTimeutc);

                    jumps[ii] = fsdStats.Count.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    distances[ii] = fsdStats.Distance.ToString("N2", System.Globalization.CultureInfo.CurrentCulture);
                    basicBoosts[ii] = fsdStats.BasicBoosts.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    standardBoosts[ii] = fsdStats.StandardBoosts.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    premiumBoosts[ii] = fsdStats.PremiumBoosts.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    scanned[ii] = scanStats.Item1.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    ucValue[ii] = scanStats.Item2.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    mapped[ii] = hl.GetNrMappedUTC(timearrutc[ii], endTimeutc).ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    jetBoosts[ii] = hl.GetJetConeBoostUTC(timearrutc[ii], endTimeutc).ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                }

                StatToDGV(dataGridViewTravel, "Jumps".T(EDTx.UserControlStats_Jumps), jumps);
                StatToDGV(dataGridViewTravel, "Travelled Ly".T(EDTx.UserControlStats_TravelledLy), distances);
                StatToDGV(dataGridViewTravel, "Premium Boost".T(EDTx.UserControlStats_PremiumBoost), premiumBoosts);
                StatToDGV(dataGridViewTravel, "Standard Boost".T(EDTx.UserControlStats_StandardBoost), standardBoosts);
                StatToDGV(dataGridViewTravel, "Basic Boost".T(EDTx.UserControlStats_BasicBoost), basicBoosts);
                StatToDGV(dataGridViewTravel, "Jet Cone Boost".T(EDTx.UserControlStats_JetConeBoost), jetBoosts);
                StatToDGV(dataGridViewTravel, "Scans".T(EDTx.UserControlStats_Scans), scanned);
                StatToDGV(dataGridViewTravel, "Mapped".T(EDTx.UserControlStats_Mapped), mapped);
                StatToDGV(dataGridViewTravel, "Scan value".T(EDTx.UserControlStats_Scanvalue), ucValue);
            }
            else // MAJOR
            {
                int intervals = 10;

                DateTime[] timeintervalslocal = new DateTime[intervals + 1];
                DateTime currentdaylocal = DateTime.Today;

                if (userControlStatsTimeTravel.TimeMode == StatsTimeUserControl.TimeModeType.Day)
                {
                    timeintervalslocal[0] = currentdaylocal.AddDays(1);
                    for (int ii = 0; ii < intervals; ii++)
                        timeintervalslocal[ii + 1] = timeintervalslocal[ii].AddDays(-1);

                }
                else if (userControlStatsTimeTravel.TimeMode == StatsTimeUserControl.TimeModeType.Week)
                {
                    DateTime startOfWeek = currentdaylocal.AddDays(-1 * (int)(DateTime.Today.DayOfWeek - 1));
                    timeintervalslocal[0] = startOfWeek.AddDays(7);
                    for (int ii = 0; ii < intervals; ii++)
                        timeintervalslocal[ii + 1] = timeintervalslocal[ii].AddDays(-7);

                }
                else  // month
                {
                    DateTime startOfMonth = new DateTime(currentdaylocal.Year, currentdaylocal.Month, 1);
                    timeintervalslocal[0] = startOfMonth.AddMonths(1);
                    for (int ii = 0; ii < intervals; ii++)
                        timeintervalslocal[ii + 1] = timeintervalslocal[ii].AddMonths(-1);
                }

                if (dataGridViewTravel.Columns.Count == 0)  // if DGV is clear..
                {
                    var Col1 = new DataGridViewTextBoxColumn();
                    Col1.HeaderText = "Type".T(EDTx.UserControlStats_Type);
                    Col1.Tag = "AlphaSort";
                    dataGridViewTravel.Columns.Add(Col1);

                    for (int ii = 0; ii < intervals; ii++)
                    {
                        var ColN = new DataGridViewTextBoxColumn();
                        ColN.HeaderText = timeintervalslocal[ii + 1].ToShortDateString();
                        dataGridViewTravel.Columns.Add(ColN);
                    }
                }

                DateTime[] timeintervalsutc = (from x in timeintervalslocal select x.ToUniversalTime()).ToArray();  // need it in UTC for the functions

                var jumps = new string[intervals];
                var distances = new string[intervals];
                var basicBoosts = new string[intervals];
                var standardBoosts = new string[intervals];
                var premiumBoosts = new string[intervals];
                var jetBoosts = new string[intervals];
                var scanned = new string[intervals];
                var mapped = new string[intervals];
                var ucValue = new string[intervals];

                for (var ii = 0; ii < intervals; ii++)
                {
                    var fsdStats = hl.GetFsdJumpStatistics(timeintervalsutc[ii + 1], timeintervalsutc[ii]);
                    var scanStats = hl.GetScanCountAndValueUTC(timeintervalsutc[ii + 1], timeintervalsutc[ii]);

                    jumps[ii] = fsdStats.Count.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    distances[ii] = fsdStats.Distance.ToString("N2", System.Globalization.CultureInfo.CurrentCulture);
                    basicBoosts[ii] = fsdStats.BasicBoosts.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    standardBoosts[ii] = fsdStats.StandardBoosts.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    premiumBoosts[ii] = fsdStats.PremiumBoosts.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    scanned[ii] = scanStats.Item1.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    ucValue[ii] = scanStats.Item2.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    mapped[ii] = hl.GetNrMappedUTC(timeintervalsutc[ii + 1], timeintervalsutc[ii]).ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    jetBoosts[ii] = hl.GetJetConeBoostUTC(timeintervalsutc[ii + 1], timeintervalsutc[ii]).ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                }

                StatToDGV(dataGridViewTravel, "Jumps".T(EDTx.UserControlStats_Jumps), jumps);
                StatToDGV(dataGridViewTravel, "Travelled Ly".T(EDTx.UserControlStats_TravelledLy), distances);
                StatToDGV(dataGridViewTravel, "Premium Boost".T(EDTx.UserControlStats_PremiumBoost), premiumBoosts);
                StatToDGV(dataGridViewTravel, "Standard Boost".T(EDTx.UserControlStats_StandardBoost), standardBoosts);
                StatToDGV(dataGridViewTravel, "Basic Boost".T(EDTx.UserControlStats_BasicBoost), basicBoosts);
                StatToDGV(dataGridViewTravel, "Jet Cone Boost".T(EDTx.UserControlStats_JetConeBoost), jetBoosts);
                StatToDGV(dataGridViewTravel, "Scans".T(EDTx.UserControlStats_Scans), scanned);
                StatToDGV(dataGridViewTravel, "Mapped".T(EDTx.UserControlStats_Mapped), mapped);
                StatToDGV(dataGridViewTravel, "Scan value".T(EDTx.UserControlStats_Scanvalue), ucValue);
            }

            for (int i = 1; i < dataGridViewTravel.Columns.Count; i++)
                ColumnValueAlignment(dataGridViewTravel.Columns[i] as DataGridViewTextBoxColumn);

            if (sortcol < dataGridViewTravel.Columns.Count)
            {
                dataGridViewTravel.Sort(dataGridViewTravel.Columns[sortcol], (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                dataGridViewTravel.Columns[sortcol].HeaderCell.SortGlyphDirection = sortorder;
            }

        }

        private void dataGridViewTravel_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Tag == null)     // tag null means date sort
                e.SortDataGridViewColumnDate();
        }

        private void userControlStatsTimeTravel_TimeModeChanged(object sender, EventArgs e)
        {
            dataGridViewTravel.Rows.Clear();        // reset all
            dataGridViewTravel.Columns.Clear();
            Stats(last_he, last_hl, true);
        }

        #endregion

        /////////////////////////////////////////////////////////////        
        #region SCAN 

        void StatsScan(HistoryList hl)
        {
            int sortcol = dataGridViewScan.SortedColumn?.Index ?? 0;
            SortOrder sortorder = dataGridViewScan.SortOrder;

            dataGridViewScan.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            int intervals = 0;
            List<JournalScan>[] scanlists = new List<JournalScan>[intervals];

            if (userControlStatsTimeScan.TimeMode == StatsTimeUserControl.TimeModeType.Summary || userControlStatsTimeScan.TimeMode == StatsTimeUserControl.TimeModeType.Custom)
            {
                if (userControlStatsTimeScan.TimeMode == StatsTimeUserControl.TimeModeType.Summary)
                {
                    intervals = 6;

                    if (dataGridViewScan.Columns.Count == 0)  // if DGV is clear..
                    {
                        var Col1 = new DataGridViewTextBoxColumn();
                        Col1.HeaderText = "Body Type".T(EDTx.UserControlStats_BodyType);
                        Col1.Tag = "AlphaSort";

                        var Col2 = new DataGridViewTextBoxColumn();
                        Col2.HeaderText = "24 Hours".T(EDTx.UserControlStats_24Hours);

                        var Col3 = new DataGridViewTextBoxColumn();
                        Col3.HeaderText = "Week".T(EDTx.UserControlStats_Week);

                        var Col4 = new DataGridViewTextBoxColumn();
                        Col4.HeaderText = "Month".T(EDTx.UserControlStats_Month);

                        var Col5 = new DataGridViewTextBoxColumn();
                        Col5.HeaderText = "Last dock".T(EDTx.UserControlStats_Lastdock);

                        var Col6 = new DataGridViewTextBoxColumn();
                        Col6.HeaderText = "Trip".T(EDTx.UserControlStats_Trip);

                        var Col7 = new DataGridViewTextBoxColumn();
                        Col7.HeaderText = "All".T(EDTx.UserControlStats_All);

                        dataGridViewScan.Columns.AddRange(new DataGridViewColumn[] { Col1, Col2, Col3, Col4, Col5, Col6, Col7 });
                    }

                    scanlists = new List<JournalScan>[intervals];

                    scanlists[0] = hl.GetScanListUTC(DateTime.UtcNow.AddDays(-1), DateTime.UtcNow);
                    scanlists[1] = hl.GetScanListUTC(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);
                    scanlists[2] = hl.GetScanListUTC(DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow);

                    HistoryEntry lastdocked = hl.GetLastHistoryEntry(x => x.IsDocked);
                    DateTime lastdockTimeutc = (lastdocked != null) ? lastdocked.EventTimeUTC : DateTime.UtcNow;

                    scanlists[3] = hl.GetScanListUTC(lastdockTimeutc, DateTime.UtcNow);

                    bool inTrip = hl.IsTravellingUTC(out DateTime tripStart);
                    scanlists[4] = hl.GetScanListUTC(inTrip ? tripStart : DateTime.UtcNow, DateTime.UtcNow);

                    scanlists[5] = hl.GetScanListUTC(new DateTime(2012, 1, 1), DateTime.UtcNow);
                }
                else
                {
                    intervals = 1;

                    if (dataGridViewScan.Columns.Count == 0)  // if DGV is clear..
                    {
                        var Col1 = new DataGridViewTextBoxColumn();
                        Col1.HeaderText = "";
                        Col1.Tag = "AlphaSort";

                        var Col2 = new DataGridViewTextBoxColumn();
                        Col2.HeaderText = userControlStatsTimeScan.CustomDateTimePickerFrom.Value.ToShortDateString() + " - " + userControlStatsTimeScan.CustomDateTimePickerTo.Value.ToShortDateString();

                        dataGridViewScan.Columns.AddRange(new DataGridViewColumn[] { Col1, Col2 });
                    }

                    DateTime fromTimeutc = EDDConfig.Instance.ConvertTimeToUTCFromSelected(userControlStatsTimeScan.CustomDateTimePickerFrom.Value);
                    DateTime endTimeutc = EDDConfig.Instance.ConvertTimeToUTCFromSelected(userControlStatsTimeScan.CustomDateTimePickerTo.Value.AddDays(1));
                    System.Diagnostics.Debug.WriteLine("Date " + fromTimeutc.ToString() + "-" + endTimeutc.ToString());

                    scanlists = new List<JournalScan>[intervals];
                    scanlists[0] = hl.GetScanListUTC(fromTimeutc,endTimeutc);
                }
            }
            else
            {
                intervals = 10;

                DateTime[] timeintervalslocal = new DateTime[intervals + 1];
                DateTime currentdaylocal = DateTime.Today;

                if (userControlStatsTimeScan.TimeMode == StatsTimeUserControl.TimeModeType.Day)
                {
                    timeintervalslocal[0] = currentdaylocal.AddDays(1);
                    for (int ii = 0; ii < intervals; ii++)
                        timeintervalslocal[ii + 1] = timeintervalslocal[ii].AddDays(-1);

                }
                else if (userControlStatsTimeScan.TimeMode == StatsTimeUserControl.TimeModeType.Week)
                {
                    DateTime startOfWeek = currentdaylocal.AddDays(-1 * (int)(DateTime.Today.DayOfWeek - 1));
                    timeintervalslocal[0] = startOfWeek.AddDays(7);
                    for (int ii = 0; ii < intervals; ii++)
                        timeintervalslocal[ii + 1] = timeintervalslocal[ii].AddDays(-7);

                }
                else  // month
                {
                    DateTime startOfMonth = new DateTime(currentdaylocal.Year, currentdaylocal.Month, 1);
                    timeintervalslocal[0] = startOfMonth.AddMonths(1);
                    for (int ii = 0; ii < intervals; ii++)
                        timeintervalslocal[ii + 1] = timeintervalslocal[ii].AddMonths(-1);
                }

                if (dataGridViewScan.Columns.Count == 0)  // if DGV is clear..
                {
                    var Col1 = new DataGridViewTextBoxColumn();
                    Col1.HeaderText = "Body Type".T(EDTx.UserControlStats_BodyType);
                    Col1.Tag = "AlphaSort";

                    dataGridViewScan.Columns.Add(Col1);

                    for (int ii = 0; ii < intervals; ii++)
                    {
                        var Col2 = new DataGridViewTextBoxColumn();
                        Col2.HeaderText = timeintervalslocal[ii + 1].ToShortDateString();
                        dataGridViewScan.Columns.Add(Col2);
                    }
                }

                scanlists = new List<JournalScan>[intervals];
                for (int ii = 0; ii < intervals; ii++)
                    scanlists[ii] = hl.GetScanListUTC(timeintervalslocal[ii + 1].ToUniversalTime(), timeintervalslocal[ii].ToUniversalTime());
            }

            for (int i = 1; i < dataGridViewScan.Columns.Count; i++)
                ColumnValueAlignment(dataGridViewScan.Columns[i] as DataGridViewTextBoxColumn);

            string[] strarr = new string[intervals];

            if (userControlStatsTimeScan.StarPlanetMode)
            {
                foreach (EDStar obj in Bodies.StarTypes)
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

                    StatToDGV(dataGridViewScan, Bodies.StarTypeNameShorter(obj), strarr);
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

                    StatToDGV(dataGridViewScan, obj == EDPlanet.Unknown_Body_Type ? "Belt Cluster" : Bodies.PlanetTypeName(obj), strarr);
                }
            }

            if (sortcol < dataGridViewScan.Columns.Count)
            {
                dataGridViewScan.Sort(dataGridViewScan.Columns[sortcol], (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                dataGridViewScan.Columns[sortcol].HeaderCell.SortGlyphDirection = sortorder;
            }
        }

        private void dataGridViewScan_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Tag == null)     // tag null means numeric sort.
                e.SortDataGridViewColumnDate();
        }

        private void userControlStatsTimeScan_DrawModeChanged(object sender, EventArgs e)
        {
            userControlStatsTimeScan_TimeModeChanged(sender, e);
        }

        private void userControlStatsTimeScan_TimeModeChanged(object sender, EventArgs e)
        {
            dataGridViewScan.Rows.Clear();        // reset all
            dataGridViewScan.Columns.Clear();
            Stats(last_he, last_hl, true);
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////////

        #region STATS IN GAME

        void StatsGame(HistoryEntry he, HistoryList hl)
        {
            string collapseExpand = GameStatTreeState();

            if (string.IsNullOrEmpty(collapseExpand))
                collapseExpand = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbStatsTreeStateSave, "YYYYYYYYYYYYY");

            if (collapseExpand.Length < 13)
                collapseExpand += new string('Y', 13);

            HistoryEntry hestats = hl.GetLastHistoryEntry(x => x.EntryType == JournalTypeEnum.Statistics, he);

            JournalStatistics stats = hestats?.journalEntry as JournalStatistics;

            if (stats != null)
            {
                AddTreeNode("@", "@", hestats.System.Name);
                AddTreeNode("@", "T", EDDConfig.Instance.ConvertTimeToSelectedFromUTC(hestats.EventTimeUTC).ToString());
                AddTreeNode("@", "UTC", hestats.EventTimeUTC.ToString());
                if (he != null)
                {
                    int rown = EDDConfig.Instance.OrderRowsInverted ? hestats.Indexno : (hl.Count - he.Indexno + 1);
                    AddTreeNode("@", "N", rown.ToString());
                }

                string bank = "Bank Account".T(EDTx.UserControlStats_BankAccount);
                var node = AddTreeNode(bank, "Current Assets".T(EDTx.UserControlStats_CurrentAssets), stats.BankAccount?.CurrentWealth.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(bank, "Spent on Ships".T(EDTx.UserControlStats_SpentonShips), stats.BankAccount?.SpentOnShips.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(bank, "Spent on Outfitting".T(EDTx.UserControlStats_SpentonOutfitting), stats.BankAccount?.SpentOnOutfitting.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(bank, "Spent on Repairs".T(EDTx.UserControlStats_SpentonRepairs), stats.BankAccount?.SpentOnRepairs.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(bank, "Spent on Fuel".T(EDTx.UserControlStats_SpentonFuel), stats.BankAccount?.SpentOnFuel.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(bank, "Spent on Munitions".T(EDTx.UserControlStats_SpentonMunitions), stats.BankAccount?.SpentOnAmmoConsumables.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(bank, "Insurance Claims".T(EDTx.UserControlStats_InsuranceClaims), stats.BankAccount?.InsuranceClaims.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(bank, "Total Claim Costs".T(EDTx.UserControlStats_TotalClaimCosts), stats.BankAccount?.SpentOnInsurance.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                if (collapseExpand[0] == 'Y')
                    node.Item1.Expand();

                string combat = "Combat".T(EDTx.UserControlStats_Combat);
                node = AddTreeNode(combat, "Bounties Claimed".T(EDTx.UserControlStats_BountiesClaimed), stats.Combat?.BountiesClaimed.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "");
                AddTreeNode(combat, "Profit from Bounty Hunting".T(EDTx.UserControlStats_ProfitfromBountyHunting), stats.Combat?.BountyHuntingProfit.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(combat, "Combat Bonds".T(EDTx.UserControlStats_CombatBonds), stats.Combat?.CombatBonds.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(combat, "Profit from Combat Bonds".T(EDTx.UserControlStats_ProfitfromCombatBonds), stats.Combat?.CombatBondProfits.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(combat, "Assassinations".T(EDTx.UserControlStats_Assassinations), stats.Combat?.Assassinations.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(combat, "Profit from Assassination".T(EDTx.UserControlStats_ProfitfromAssassination), stats.Combat?.AssassinationProfits.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(combat, "Highest Single Reward".T(EDTx.UserControlStats_HighestSingleReward), stats.Combat?.HighestSingleReward.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(combat, "Skimmers Killed".T(EDTx.UserControlStats_SkimmersKilled), stats.Combat?.SkimmersKilled.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                if (collapseExpand[1] == 'Y')
                    node.Item1.Expand();

                string crime = "Crime".T(EDTx.UserControlStats_Crime);
                node = AddTreeNode(crime, "Notoriety".T(EDTx.UserControlStats_Notoriety), stats.Crime?.Notoriety.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(crime, "Number of Fines".T(EDTx.UserControlStats_NumberofFines), stats.Crime?.Fines.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(crime, "Lifetime Fines Value".T(EDTx.UserControlStats_LifetimeFinesValue), stats.Crime?.TotalFines.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(crime, "Bounties Received".T(EDTx.UserControlStats_BountiesReceived), stats.Crime?.BountiesReceived.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(crime, "Lifetime Bounty Value".T(EDTx.UserControlStats_LifetimeBountyValue), stats.Crime?.TotalBounties.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(crime, "Highest Bounty Issued".T(EDTx.UserControlStats_HighestBountyIssued), stats.Crime?.HighestBounty.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                if (collapseExpand[2] == 'Y')
                    node.Item1.Expand();

                string smuggling = "Smuggling".T(EDTx.UserControlStats_Smuggling);
                node = AddTreeNode(smuggling, "Black Market Network".T(EDTx.UserControlStats_BlackMarketNetwork), stats.Smuggling?.BlackMarketsTradedWith.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(smuggling, "Black Market Profits".T(EDTx.UserControlStats_BlackMarketProfits), stats.Smuggling?.BlackMarketsProfits.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(smuggling, "Commodities Smuggled".T(EDTx.UserControlStats_CommoditiesSmuggled), stats.Smuggling?.ResourcesSmuggled.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(smuggling, "Average Profit".T(EDTx.UserControlStats_AverageProfit), stats.Smuggling?.AverageProfit.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(smuggling, "Highest Single Transaction".T(EDTx.UserControlStats_HighestSingleTransaction), stats.Smuggling?.HighestSingleTransaction.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                if (collapseExpand[3] == 'Y')
                    node.Item1.Expand();

                string trading = "Trading".T(EDTx.UserControlStats_Trading);
                node = AddTreeNode(trading, "Market Network".T(EDTx.UserControlStats_MarketNetwork), stats.Trading?.MarketsTradedWith.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(trading, "Market Profits".T(EDTx.UserControlStats_MarketProfits), stats.Trading?.MarketProfits.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(trading, "Commodities Traded".T(EDTx.UserControlStats_CommoditiesTraded), stats.Trading?.ResourcesTraded.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(trading, "Average Profit".T(EDTx.UserControlStats_AverageProfit), stats.Trading?.AverageProfit.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(trading, "Highest Single Transaction".T(EDTx.UserControlStats_HighestSingleTransaction), stats.Trading?.HighestSingleTransaction.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                if (collapseExpand[4] == 'Y')
                    node.Item1.Expand();

                string mining = "Mining".T(EDTx.UserControlStats_Mining);
                node = AddTreeNode(mining, "Mining Profits".T(EDTx.UserControlStats_MiningProfits), stats.Mining?.MiningProfits.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(mining, "Materials Refined".T(EDTx.UserControlStats_MaterialsRefined), stats.Mining?.QuantityMined.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(mining, "Materials Collected".T(EDTx.UserControlStats_MaterialsCollected), stats.Mining?.MaterialsCollected.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                if (collapseExpand[5] == 'Y')
                    node.Item1.Expand();

                string exploration = "Exploration".T(EDTx.UserControlStats_Exploration);
                node = AddTreeNode(exploration, "Systems Visited".T(EDTx.UserControlStats_SystemsVisited), stats.Exploration?.SystemsVisited.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(exploration, "Exploration Profits".T(EDTx.UserControlStats_ExplorationProfits), stats.Exploration?.ExplorationProfits.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(exploration, "Level 2 Scans".T(EDTx.UserControlStats_Level2Scans), stats.Exploration?.PlanetsScannedToLevel2.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(exploration, "Level 3 Scans".T(EDTx.UserControlStats_Level3Scans), stats.Exploration?.PlanetsScannedToLevel3.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(exploration, "Highest Payout".T(EDTx.UserControlStats_HighestPayout), stats.Exploration?.HighestPayout.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(exploration, "Total Hyperspace Distance".T(EDTx.UserControlStats_TotalHyperspaceDistance), stats.Exploration?.TotalHyperspaceDistance.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "ly");
                AddTreeNode(exploration, "Total Hyperspace Jumps".T(EDTx.UserControlStats_TotalHyperspaceJumps), stats.Exploration?.TotalHyperspaceJumps.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(exploration, "Farthest From Start".T(EDTx.UserControlStats_FarthestFromStart), stats.Exploration?.GreatestDistanceFromStart.ToString("N2", System.Globalization.CultureInfo.CurrentCulture), "ly");
                AddTreeNode(exploration, "Time Played".T(EDTx.UserControlStats_TimePlayed), stats.Exploration?.TimePlayed.SecondsToWeeksDaysHoursMinutesSeconds());
                if (collapseExpand[6] == 'Y')
                    node.Item1.Expand();

                string passengers = "Passengers".T(EDTx.UserControlStats_Passengers);
                node = AddTreeNode(passengers, "Total Bulk Passengers Delivered".T(EDTx.UserControlStats_TotalBulkPassengersDelivered), stats.PassengerMissions?.Bulk.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(passengers, "Total VIPs Delivered".T(EDTx.UserControlStats_TotalVIPsDelivered), stats.PassengerMissions?.VIP.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(passengers, "Delivered".T(EDTx.UserControlStats_Delivered), stats.PassengerMissions?.Delivered.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(passengers, "Ejected".T(EDTx.UserControlStats_Ejected), stats.PassengerMissions?.Ejected.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                if (collapseExpand[7] == 'Y')
                    node.Item1.Expand();

                string search = "Search and Rescue".T(EDTx.UserControlStats_SearchandRescue);
                node = AddTreeNode(search, "Total Items Rescued".T(EDTx.UserControlStats_TotalItemsRescued), stats.SearchAndRescue?.Traded.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(search, "Total Profit".T(EDTx.UserControlStats_TotalProfit), stats.SearchAndRescue?.Profit.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(search, "Total Rescue Transactions".T(EDTx.UserControlStats_TotalRescueTransactions), stats.SearchAndRescue?.Count.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                if (collapseExpand[8] == 'Y')
                    node.Item1.Expand();

                string craft = "Crafting".T(EDTx.UserControlStats_Crafting);
                node = AddTreeNode(craft, "Engineers Used".T(EDTx.UserControlStats_EngineersUsed), stats.Crafting?.CountOfUsedEngineers.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(craft, "Total Recipes Generated".T(EDTx.UserControlStats_TotalRecipesGenerated), stats.Crafting?.RecipesGenerated.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(craft, "Grade 1 Recipes Generated".T(EDTx.UserControlStats_Grade1RecipesGenerated), stats.Crafting?.RecipesGeneratedRank1.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(craft, "Grade 2 Recipes Generated".T(EDTx.UserControlStats_Grade2RecipesGenerated), stats.Crafting?.RecipesGeneratedRank2.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(craft, "Grade 3 Recipes Generated".T(EDTx.UserControlStats_Grade3RecipesGenerated), stats.Crafting?.RecipesGeneratedRank3.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(craft, "Grade 4 Recipes Generated".T(EDTx.UserControlStats_Grade4RecipesGenerated), stats.Crafting?.RecipesGeneratedRank4.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(craft, "Grade 5 Recipes Generated".T(EDTx.UserControlStats_Grade5RecipesGenerated), stats.Crafting?.RecipesGeneratedRank5.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                if (collapseExpand[9] == 'Y')
                    node.Item1.Expand();

                string crew = "Crew".T(EDTx.UserControlStats_Crew);
                node = AddTreeNode(crew, "Total Wages".T(EDTx.UserControlStats_TotalWages), stats.Crew?.TotalWages.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(crew, "Total Hired".T(EDTx.UserControlStats_TotalHired), stats.Crew?.Hired.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(crew, "Total Fired".T(EDTx.UserControlStats_TotalFired), stats.Crew?.Fired.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(crew, "Died in Line of Duty".T(EDTx.UserControlStats_DiedinLineofDuty), stats.Crew?.Died.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                if (collapseExpand[10] == 'Y')
                    node.Item1.Expand();

                string multicrew = "Multi-crew".T(EDTx.UserControlStats_Multi);
                node = AddTreeNode(multicrew, "Total Time".T(EDTx.UserControlStats_TotalTime), SecondsToDHMString(stats.Multicrew?.TimeTotal));
                AddTreeNode(multicrew, "Fighter Time".T(EDTx.UserControlStats_FighterTime), SecondsToDHMString(stats.Multicrew?.FighterTimeTotal));
                AddTreeNode(multicrew, "Gunner Time".T(EDTx.UserControlStats_GunnerTime), SecondsToDHMString(stats.Multicrew?.GunnerTimeTotal));
                AddTreeNode(multicrew, "Credits Made".T(EDTx.UserControlStats_CreditsMade), stats.Multicrew?.CreditsTotal.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(multicrew, "Fines Accrued".T(EDTx.UserControlStats_FinesAccrued), stats.Multicrew?.FinesTotal.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                if (collapseExpand[11] == 'Y')
                    node.Item1.Expand();

                string mattrader = "Materials Trader".T(EDTx.UserControlStats_MaterialsTrader);
                node = AddTreeNode(mattrader, "Trades Completed".T(EDTx.UserControlStats_TradesCompleted), stats.MaterialTraderStats?.TradesCompleted.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(mattrader, "Materials Traded".T(EDTx.UserControlStats_MaterialsTraded), stats.MaterialTraderStats?.MaterialsTraded.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                if (collapseExpand[12] == 'Y')
                    node.Item1.Expand();
            }
        }

        string SecondsToDHMString(int? seconds)
        {
            if (!seconds.HasValue)
                return "";

            TimeSpan time = TimeSpan.FromSeconds(seconds.Value);
            return string.Format("{0} days {1} hours {1} minutes".T(EDTx.UserControlStats_TME), time.Days, time.Hours, time.Minutes);
        }

        Tuple<TreeNode, TreeNode> AddTreeNode(string parentname, string name, string value, string units = "")
        {
            TreeNode[] parents = treeViewStats.Nodes.Find(parentname, false);
            TreeNode parent = (parents.Length == 0) ? (treeViewStats.Nodes.Add(parentname, parentname)) : parents[0];

            TreeNode[] childs = parent.Nodes.Find(name, false);
            TreeNode child = (childs.Length == 0) ? (parent.Nodes.Add(name, "")) : childs[0];
            child.Text = $"{name}:  {(String.IsNullOrEmpty(value) ? "0" : value)} {units}";
            return new Tuple<TreeNode, TreeNode>(parent, child);
        }

        string GameStatTreeState()
        {
            string result = "";
            if (treeViewStats.Nodes.Count > 0)
            {
                foreach (TreeNode tn in treeViewStats.Nodes)
                    result += tn.IsExpanded ? "Y" : "N";
            }
            else
                result = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbStatsTreeStateSave, "YYYYYYYYYYYYY");
            return result;
        }

        #endregion

        #region STATS_BY_SHIP

        static JournalTypeEnum[] journalsForShipStats = new JournalTypeEnum[]
        {
            JournalTypeEnum.FSDJump,
            JournalTypeEnum.Scan,
            JournalTypeEnum.MarketBuy,
            JournalTypeEnum.MarketSell,
            JournalTypeEnum.Died,
        };

        void StatsByShip(HistoryList hl)
        {
            int sortcol = dataGridViewByShip.SortedColumn?.Index ?? 0;
            SortOrder sortorder = dataGridViewByShip.SortOrder;

            if (dataGridViewByShip.Columns.Count == 0)
            {
                var Col1 = new DataGridViewTextBoxColumn();
                Col1.HeaderText = "Type".T(EDTx.UserControlStats_Type);
                Col1.Tag = "AlphaSort";

                var Col2 = new DataGridViewTextBoxColumn();
                Col2.HeaderText = "Name".T(EDTx.UserControlStats_Name);
                Col2.Tag = "AlphaSort";

                var Col3 = new DataGridViewTextBoxColumn();
                Col3.HeaderText = "Ident".T(EDTx.UserControlStats_Ident);
                Col3.Tag = "AlphaSort";

                var Col4 = new DataGridViewTextBoxColumn();
                Col4.HeaderText = "Jumps".T(EDTx.UserControlStats_Jumps);

                var Col5 = new DataGridViewTextBoxColumn();
                Col5.HeaderText = "Travelled Ly".T(EDTx.UserControlStats_TravelledLy);

                var Col6 = new DataGridViewTextBoxColumn();
                Col6.HeaderText = "Bodies Scanned".T(EDTx.UserControlStats_BodiesScanned);

                var Col7 = new DataGridViewTextBoxColumn();
                Col7.HeaderText = "Goods Bought".T(EDTx.UserControlStats_GoodsBought);

                var Col8 = new DataGridViewTextBoxColumn();
                Col8.HeaderText = "Goods Sold".T(EDTx.UserControlStats_GoodsSold);

                var Col9 = new DataGridViewTextBoxColumn();
                Col9.HeaderText = "Destroyed".T(EDTx.UserControlStats_Destroyed);

                dataGridViewByShip.Columns.AddRange(new DataGridViewColumn[] { Col1, Col2, Col3, Col4, Col5, Col6, Col7, Col8, Col9 });
            }

            string[] strarr = new string[8];

            foreach (var si in hl.shipinformationlist.Ships.Where(val => !ShipModuleData.IsSRVOrFighter(val.Value.ShipFD)))
            {
                strarr[0] = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(si.Value.ShipUserName ?? "-");
                strarr[1] = (si.Value.ShipUserIdent ?? "-").ToUpper();
                strarr[2] = hl.GetFSDCarrierJumps(si.Key).ToString();
                strarr[3] = hl.GetTraveledLy(si.Key).ToString("N0");
                strarr[4] = hl.GetBodiesScanned(si.Key).ToString();
                strarr[5] = hl.GetTonnesBought(si.Key).ToString("N0");
                strarr[6] = hl.GetTonnesSold(si.Key).ToString("N0");
                strarr[7] = hl.GetDeathCount(si.Key).ToString();
                StatToDGV(dataGridViewByShip, si.Value.ShipType ?? "Unknown".T(EDTx.Unknown), strarr);
            }

            if (sortcol < dataGridViewByShip.Columns.Count)
            {
                dataGridViewByShip.Sort(dataGridViewByShip.Columns[sortcol], (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                dataGridViewByShip.Columns[sortcol].HeaderCell.SortGlyphDirection = sortorder;
            }
        }

        private void dataGridViewByShip_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Tag == null)     // tag null means numeric sort.
                e.SortDataGridViewColumnNumeric();
        }

        #endregion

        #region Helpers

        private static void ColumnValueAlignment(DataGridViewTextBoxColumn Col2)
        {
            Col2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            Col2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            Col2.SortMode = DataGridViewColumnSortMode.Automatic;
        }

        void StatToDGV(DataGridView datagrid, string title, string[] data)
        {
            object[] rowobj = new object[data.Length + 1];

            rowobj[0] = title;

            for (int ii = 0; ii < data.Length; ii++)
            {
                rowobj[ii + 1] = data[ii];
            }

            int rowpresent = datagrid.FindRowWithValue(0, title);

            if (rowpresent != -1)
            {
                for (int ii = 1; ii < rowobj.Length; ii++)
                {
                    var row = datagrid.Rows[rowpresent];
                    var cell = row.Cells[ii];
                    cell.Value = rowobj[ii];
                }
            }
            else
                datagrid.Rows.Add(rowobj);
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            dataGridViewTravel.RowTemplate.MinimumHeight =
            dataGridViewScan.RowTemplate.MinimumHeight =
            dataGridViewByShip.RowTemplate.MinimumHeight =
            dataGridViewStats.RowTemplate.MinimumHeight = Font.ScalePixels(24);

            int height = 0;
            foreach (DataGridViewRow row in dataGridViewStats.Rows)
            {
                height += row.Height + 1;
            }
            height += dataGridViewStats.ColumnHeadersHeight + 2;

            dataGridViewStats.Height = height;
            var width = panelGeneral.Width - panelGeneral.ScrollBarWidth - dataGridViewStats.Left;
            dataGridViewStats.Width = width;

            if (mostVisited != null)
            {
                mostVisited.Width = width;
                mostVisited.Top = dataGridViewStats.Bottom + 8;
            }
            base.OnLayout(e);
        }

        #endregion


    }
}
