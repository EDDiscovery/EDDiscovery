/*
 * Copyright © 2016 - 2022 EDDiscovery development team
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
using System.Collections.Concurrent;
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
        private string dbSelectedTabSave = "SelectedTab";
        private string dbStartDate = "StartDate";
        private string dbStartDateOn = "StartDateChecked";
        private string dbEndDate = "EndDate";
        private string dbEndDateOn = "EndDateChecked";

        private string dbStatsTreeStateSave = "TreeExpanded";

        private string dbScanSummary = "ScanSummary";
        private string dbScanDWM = "ScanDWM";
        private string dbTravelSummary = "TravelSummary";
        private string dbTravelDWM = "TravelDWM";
        private string dbCombatSummary = "CombatSummary";
        private string dbCombatDWM = "CombatDWM";

        private string dbShip = "Ship";

        private bool endchecked, startchecked;

        private JournalStatisticsComputer statscomputer = new JournalStatisticsComputer();
        private JournalStats currentstats;
        private JournalStats pendingstats;
        private bool redisplay = false;

        private Timer timerupdate;

        // because of the async awaits, we are in effect multithreaded, best to have a concurrent queue
        private ConcurrentQueue<JournalEntry> entriesqueued = new ConcurrentQueue<JournalEntry>();    

        private Chart mostVisitedChart { get; set; }

        #region Init

        public UserControlStats()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "Stats";

            tabControlCustomStats.SelectedIndex = GetSetting(dbSelectedTabSave, 0);
            userControlStatsTimeScan.DisplayStarsPlanetSelector(true);

            var enumlist = new Enum[] { EDTx.UserControlStats_tabControlCustomStats_tabPageGeneral, EDTx.UserControlStats_tabControlCustomStats_tabPageGeneral_ItemName, 
                                        EDTx.UserControlStats_tabControlCustomStats_tabPageGeneral_Information, EDTx.UserControlStats_tabControlCustomStats_tabPageTravel, 
                                        EDTx.UserControlStats_tabControlCustomStats_tabPageScan, 
                                        EDTx.UserControlStats_tabControlCustomStats_tabPageGameStats, 
                                        EDTx.UserControlStats_tabControlCustomStats_tabPageByShip, EDTx.UserControlStats_labelStart, EDTx.UserControlStats_labelEndDate,
                                        EDTx.UserControlStats_tabControlCustomStats_tabPageCombat
                                        };
            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);

            try
            {
                Chart chart = new Chart();                                                  // create a chart, to see if it works, may not on all platforms
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
                chart.Visible = false;
                this.panelGeneral.Controls.Add(chart);
                this.mostVisitedChart = chart;
                chart.EndInit();
            }
            catch (NotImplementedException)
            {
                // Charting not implemented in mono System.Windows.Forms
            }

            discoveryform.OnNewEntry += AddNewEntry;
            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;

            // datetime picker kind is not used
            dateTimePickerStartDate.Value = GetSetting(dbStartDate, EDDConfig.Instance.ConvertTimeToSelectedFromUTC(EDDConfig.GameLaunchTimeUTC())).StartOfDay();
            dateTimePickerEndDate.Value = GetSetting(dbEndDate, EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow)).EndOfDay();
            startchecked = dateTimePickerStartDate.Checked = GetSetting(dbStartDateOn, false);
            endchecked = dateTimePickerEndDate.Checked = GetSetting(dbEndDateOn, false);
            VerifyDates();
            dateTimePickerStartDate.ValueChanged += DateTimePicker_ValueChangedStart;
            dateTimePickerEndDate.ValueChanged += DateTimePicker_ValueChangedEnd;

            labelStatus.Text = "";

            timerupdate = new System.Windows.Forms.Timer();
            timerupdate.Interval = 2000;
            timerupdate.Tick += Timerupdate_Tick;
        }

        public override void InitialDisplay()
        {
            if ( discoveryform.history.Count>0 )        // if we loaded a history, this is a new panel, so work
                KickComputer();

            timerupdate.Start();
        }

        private void Discoveryform_OnHistoryChange(HistoryList hl)
        {
            VerifyDates();      // date range may have changed
            KickComputer();
        }

        public override void Closing()
        {
            statscomputer.Stop();

            PutSetting(dbStatsTreeStateSave, GameStatTreeState());

            if (dataGridViewScan.Columns.Count > 0)     // anything to save..
                DGVSaveColumnLayout(dataGridViewScan, dataGridViewScan.Columns.Count <= 8 ? dbScanSummary : dbScanDWM);
            if (dataGridViewTravel.Columns.Count > 0)
                DGVSaveColumnLayout(dataGridViewTravel, dataGridViewTravel.Columns.Count <= 8 ? dbTravelSummary : dbTravelDWM);
            if (dataGridViewCombat.Columns.Count > 0)
                DGVSaveColumnLayout(dataGridViewByShip, dataGridViewCombat.Columns.Count <= 8 ? dbCombatSummary : dbCombatDWM);
            if (dataGridViewByShip.Columns.Count > 0)
                DGVSaveColumnLayout(dataGridViewByShip, dbShip);

            discoveryform.OnNewEntry -= AddNewEntry;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            dataGridViewTravel.RowTemplate.MinimumHeight =
            dataGridViewScan.RowTemplate.MinimumHeight =
            dataGridViewByShip.RowTemplate.MinimumHeight =
            dataGridViewGeneral.RowTemplate.MinimumHeight = Font.ScalePixels(24);

            int height = 0;
            foreach (DataGridViewRow row in dataGridViewGeneral.Rows)
            {
                height += row.Height + 1;
            }
            height += dataGridViewGeneral.ColumnHeadersHeight + 2;

            dataGridViewGeneral.Height = height;
            var width = panelGeneral.Width - panelGeneral.ScrollBarWidth - dataGridViewGeneral.Left;
            dataGridViewGeneral.Width = width;

            if (mostVisitedChart != null)
            {
                mostVisitedChart.Width = width;
                mostVisitedChart.Top = dataGridViewGeneral.Bottom + 8;
            }
            base.OnLayout(e);
        }

        private void tabControlCustomStats_SelectedIndexChanged(object sender, EventArgs e)     // tab change, UI will see tab has changed
        {
            PutSetting(dbSelectedTabSave, tabControlCustomStats.SelectedIndex);
            redisplay = true;
        }

        bool updateprogramatically = false;
        private void VerifyDates()
        {
            updateprogramatically = true;
            if (!EDDConfig.Instance.DateTimeInRangeForGame(dateTimePickerStartDate.Value) || !EDDConfig.Instance.DateTimeInRangeForGame(dateTimePickerEndDate.Value))
            {
                dateTimePickerStartDate.Checked = dateTimePickerEndDate.Checked = false;
                dateTimePickerStartDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(EDDConfig.GameLaunchTimeUTC());
                dateTimePickerEndDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow.EndOfDay());
            }
            updateprogramatically = false;
        }

        private void DateTimePicker_ValueChangedStart(object sender, EventArgs e)
        {
            if (!updateprogramatically)
            {
                if (startchecked != dateTimePickerStartDate.Checked || dateTimePickerStartDate.Checked)     // if check changed, or date changed and its checked
                    KickComputer();
                startchecked = dateTimePickerStartDate.Checked;
                PutSetting(dbStartDate, dateTimePickerStartDate.Value);
                PutSetting(dbStartDateOn, dateTimePickerStartDate.Checked);
            }
        }

        private void DateTimePicker_ValueChangedEnd(object sender, EventArgs e)
        {
            if (!updateprogramatically)
            {
                if (endchecked != dateTimePickerEndDate.Checked || dateTimePickerEndDate.Checked)
                    KickComputer();
                endchecked = dateTimePickerEndDate.Checked;
                PutSetting(dbEndDate, dateTimePickerEndDate.Value);
                PutSetting(dbEndDateOn, dateTimePickerEndDate.Checked);
            }
        }

        #endregion

        #region Stats Computation


        // Computation of stats may have changed
        private void KickComputer()
        {
            statscomputer.Stop();

            if (EDCommander.CurrentCmdrID >= 0)  // real commander
            {
                while (entriesqueued.TryDequeue(out JournalEntry unused))       // empty queue
                    ;

                statscomputer.Start(EDCommander.CurrentCmdrID,
                                                dateTimePickerStartDate.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(dateTimePickerStartDate.Value.StartOfDay()) : default(DateTime?),
                                                dateTimePickerEndDate.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(dateTimePickerEndDate.Value.EndOfDay()) : default(DateTime?),
                                                (rs) => 
                                                {
                                                    BeginInvoke((MethodInvoker)(() => { EndComputation(rs); })); 
                                                });

                labelStatus.Text = "Working..";
            }
        }

        private void EndComputation(JournalStats rs)
        {
            System.Diagnostics.Debug.Assert(Application.MessageLoop);
            pendingstats = rs;              // the UI tick will see we have new pending stats and apply them..
            labelStatus.Text = "";
        }

        private void AddNewEntry(HistoryEntry he, HistoryList hl)
        {
            if (JournalStatisticsComputer.IsJournalEntryForStats(he.journalEntry))     // only if its got something to do with stats
            {
                bool withintime = !dateTimePickerEndDate.Checked || he.journalEntry.EventTimeUTC <= EDDConfig.Instance.ConvertTimeToUTCFromPicker(dateTimePickerEndDate.Value.EndOfDay());

                if (withintime)
                {
                    entriesqueued.Enqueue(he.journalEntry);     // queue, the UI tick will see we have new events and add
                }
            }
        }

        #endregion

        #region Update

        private void Timerupdate_Tick(object sender, EventArgs e)
        {
            bool newstats = pendingstats != null;                        // this means kick computation happened..
            bool enqueued = entriesqueued.Count > 0;                    // queued entries

         //   System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCount} Tick {newstats} {enqueued} {redisplay}");

            if ( newstats || enqueued || redisplay)                      // redisplay is due to tab change or time mode change
            {
                timerupdate.Stop();     // in case we take a long time, stop the timer, so we don't reenter

                bool turnontimer = true; //default is back on

                System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCountLap("Display", true)} Stats display starts {tabControlCustomStats.SelectedIndex}");

                if ( newstats )                                         // new kick computation
                {
                    currentstats = pendingstats;                        // swap stats structure
                    pendingstats = null;
                }

                if (currentstats != null)                               // if we have stats..
                {
                    while (entriesqueued.TryDequeue(out JournalEntry result))       // if any entries are queued, process them off
                    {
                        currentstats.Process(result);
                    }

                    if ( newstats || enqueued )     // if we did newstate, or enqueued, all of the below can now be out of date
                    {                               // we may not be currently displaying them, but we will need to refresh if we select them again
                        System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCount} Stats clear flags due to ns{newstats} eq{enqueued} ");
                        lasttraveltimemode = lastscantimemode = lastcombattimemode = StatsTimeUserControl.TimeModeType.NotSet;
                        laststatsgeneraldisplayed = laststatsbyshipdisplayed = false;
                    }

                    DateTime endtimeutc = dateTimePickerEndDate.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(dateTimePickerEndDate.Value.EndOfDay()) :
                                                                            EDDConfig.Instance.SelectedEndOfTodayUTC();


                    if (tabControlCustomStats.SelectedTab == tabPageGeneral)
                    {
                        if ( laststatsgeneraldisplayed == false)
                            StatsGeneral(currentstats, endtimeutc);
                    }
                    else if (tabControlCustomStats.SelectedTab == tabPageTravel)
                    {
                        if ( lasttraveltimemode != userControlStatsTimeTravel.TimeMode)
                        {
                            turnontimer = false;
                            StatsTravel(currentstats, endtimeutc);
                        }
                    }
                    else if (tabControlCustomStats.SelectedTab == tabPageScan)
                    {
                        if (lastscantimemode != userControlStatsTimeScan.TimeMode)
                        {
                            turnontimer = false;
                            StatsScan(currentstats, endtimeutc);
                        }
                    }
                    else if (tabControlCustomStats.SelectedTab == tabPageCombat)
                    {
                        if (lastcombattimemode != statsTimeUserControlCombat.TimeMode)
                        {
                            turnontimer = false;
                            StatsCombat(currentstats, endtimeutc);
                        }
                    }
                    else if (tabControlCustomStats.SelectedTab == tabPageGameStats)
                    {
                        if (lastgamestatsdisplayed != currentstats.laststats)       // if different game stats structure (will apply on newstats or enqueued)
                            StatsGame(currentstats);
                    }
                    else if (tabControlCustomStats.SelectedTab == tabPageByShip)
                    {
                        if ( laststatsbyshipdisplayed == false)
                            StatsByShip(currentstats);
                    }
                }

                redisplay = false;

                System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCountLap("Display")} Stats display ends");

                if ( turnontimer )
                    timerupdate.Start();                           
            }

        }

        #endregion

        #region Stats General **************************************************************************************************************

        private bool laststatsgeneraldisplayed = false;
        private void StatsGeneral(JournalStats currentstat, DateTime endtimeutc)
        {
            DGVGeneral("Total No of jumps".T(EDTx.UserControlStats_TotalNoofjumps), currentstat.fsdcarrierjumps.ToString());

            if (currentstat.FSDJumps.Count > 0)        // these will be null unless there are jumps
            {
                DGVGeneral("FSD jumps".T(EDTx.UserControlStats_FSDjumps), currentstat.FSDJumps.Count.ToString());

                DGVGeneral("FSD Jump History".T(EDTx.UserControlStats_JumpHistory),
                                        "24 Hours: ".T(EDTx.UserControlStats_24hc) + currentstat.FSDJumps.Where(x => x.utc >= endtimeutc.AddHours(-24)).Count() +
                                        ", One Week: ".T(EDTx.UserControlStats_OneWeek) + currentstat.FSDJumps.Where(x => x.utc >= endtimeutc.AddDays(-7)).Count() +
                                        ", 30 Days: ".T(EDTx.UserControlStats_30Days) + currentstat.FSDJumps.Where(x => x.utc >= endtimeutc.AddDays(-30)).Count() +
                                        ", One Year: ".T(EDTx.UserControlStats_OneYear) + currentstat.FSDJumps.Where(x => x.utc >= endtimeutc.AddDays(-365)).Count()
                                        );

                DGVGeneral("Most North".T(EDTx.UserControlStats_MostNorth), GetSystemDataString(currentstat.MostNorth));
                DGVGeneral("Most South".T(EDTx.UserControlStats_MostSouth), GetSystemDataString(currentstat.MostSouth));
                DGVGeneral("Most East".T(EDTx.UserControlStats_MostEast), GetSystemDataString(currentstat.MostEast));
                DGVGeneral("Most West".T(EDTx.UserControlStats_MostWest), GetSystemDataString(currentstat.MostWest));
                DGVGeneral("Most Highest".T(EDTx.UserControlStats_MostHighest), GetSystemDataString(currentstat.MostUp));
                DGVGeneral("Most Lowest".T(EDTx.UserControlStats_MostLowest), GetSystemDataString(currentstat.MostDown));

                if (mostVisitedChart != null)        // chart exists
                {
                    mostVisitedChart.Visible = true;

                    Color GridC = ExtendedControls.Theme.Current.GridBorderLines;
                    Color TextC = ExtendedControls.Theme.Current.GridCellText;
                    mostVisitedChart.Titles.Clear();
                    mostVisitedChart.Titles.Add(new Title("Most Visited".T(EDTx.UserControlStats_MostVisited), Docking.Top, ExtendedControls.Theme.Current.GetFont, TextC));
                    mostVisitedChart.Series[0].Points.Clear();

                    mostVisitedChart.ChartAreas[0].AxisX.LabelStyle.ForeColor = TextC;
                    mostVisitedChart.ChartAreas[0].AxisY.LabelStyle.ForeColor = TextC;
                    mostVisitedChart.ChartAreas[0].AxisX.MajorGrid.LineColor = GridC;
                    mostVisitedChart.ChartAreas[0].AxisX.MinorGrid.LineColor = GridC;
                    mostVisitedChart.ChartAreas[0].AxisY.MajorGrid.LineColor = GridC;
                    mostVisitedChart.ChartAreas[0].AxisY.MinorGrid.LineColor = GridC;
                    mostVisitedChart.ChartAreas[0].BorderColor = GridC;
                    mostVisitedChart.ChartAreas[0].BorderDashStyle = ChartDashStyle.Solid;
                    mostVisitedChart.ChartAreas[0].BorderWidth = 2;

                    mostVisitedChart.ChartAreas[0].BackColor = Color.Transparent;
                    mostVisitedChart.Series[0].Color = GridC;
                    mostVisitedChart.BorderlineColor = Color.Transparent;

                    var fsdbodies = currentstat.FSDJumps.GroupBy(x => x.bodyname).ToDictionary(x => x.Key, y => y.Count()).ToList();       // get KVP list of distinct bodies
                    fsdbodies.Sort(delegate (KeyValuePair<string, int> left, KeyValuePair<string, int> right) { return right.Value.CompareTo(left.Value); }); // and sort highest

                    int i = 0;
                    foreach (var data in fsdbodies.Take(10))        // display 10
                    {
                        mostVisitedChart.Series[0].Points.Add(new DataPoint(i, data.Value));
                        mostVisitedChart.Series[0].Points[i].AxisLabel = data.Key;
                        mostVisitedChart.Series[0].Points[i].LabelForeColor = TextC;
                        i++;
                    }
                }
            }
            else
            {
                if (mostVisitedChart != null)
                    mostVisitedChart.Visible = false;
            }

            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCount} General tab displayed");

            laststatsgeneraldisplayed = true;
            PerformLayout();
        }

        private string GetSystemDataString(JournalLocOrJump he)
        {
            return he == null ? "N/A" : he.StarSystem + " @ " + he.StarPos.X.ToString("0.0") + "; " + he.StarPos.Y.ToString("0.0") + "; " + he.StarPos.Z.ToString("0.0");
        }

        void DGVGeneral(string title, string data)
        {
            int rowpresent = dataGridViewGeneral.FindRowWithValue(0, title);
            if (rowpresent != -1)
                dataGridViewGeneral.Rows[rowpresent].Cells[1].Value = data;
            else
                dataGridViewGeneral.Rows.Add(new object[] { title, data });
        }

        #endregion


        #region Travel Panel *********************************************************************************************************************

        private StatsTimeUserControl.TimeModeType lasttraveltimemode = StatsTimeUserControl.TimeModeType.NotSet;

        async void StatsTravel(JournalStats currentstat, DateTime endtimeutc)
        {
            var timemode = userControlStatsTimeTravel.TimeMode;

            userControlStatsTimeTravel.Enabled = false;
            int sortcol = dataGridViewTravel.SortedColumn?.Index ?? 99;
            SortOrder sortorder = dataGridViewTravel.SortOrder;

            int intervals = timemode == StatsTimeUserControl.TimeModeType.Summary ? 6 : timemode == StatsTimeUserControl.TimeModeType.Year ? Math.Min(12, DateTime.Now.Year - EDDConfig.GameLaunchTimeUTC().Year + 1) : 12;

            // plainly wrong with dates...

            var isTravelling = discoveryform.history.IsTravellingUTC(out var tripStartutc);
            if (isTravelling && tripStartutc > endtimeutc)
                isTravelling = false;

            var tupletimes = timemode == StatsTimeUserControl.TimeModeType.Summary ?
                                    SetupSummary(endtimeutc, isTravelling ? tripStartutc : DateTime.UtcNow, currentstat.lastdockedutc, dataGridViewTravel, dbTravelSummary) :
                                    SetUpDaysMonthsYear(endtimeutc, dataGridViewTravel, timemode, intervals, dbTravelDWM);

            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCountLap("STATS", true)} Travel stats interval begin {timemode}");
            var res = await ComputeTravel(currentstat, tupletimes);
            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCountLap("STATS")} Travel stats interval end {timemode}");
            if (IsClosed)
                return;

            int row = 0;
            StatToDGV(dataGridViewTravel, "Jumps".T(EDTx.UserControlStats_Jumps), res[row++]);
            StatToDGV(dataGridViewTravel, "Travelled Ly".T(EDTx.UserControlStats_TravelledLy), res[row++]);
            StatToDGV(dataGridViewTravel, "Basic Boost".T(EDTx.UserControlStats_BasicBoost), res[row++]);
            StatToDGV(dataGridViewTravel, "Standard Boost".T(EDTx.UserControlStats_StandardBoost), res[row++]);
            StatToDGV(dataGridViewTravel, "Premium Boost".T(EDTx.UserControlStats_PremiumBoost), res[row++]);
            StatToDGV(dataGridViewTravel, "Jet Cone Boost".T(EDTx.UserControlStats_JetConeBoost), res[row++]);
            StatToDGV(dataGridViewTravel, "Scans".T(EDTx.UserControlStats_Scans), res[row++]);
            StatToDGV(dataGridViewTravel, "Mapped".T(EDTx.UserControlStats_Mapped), res[row++]);
            StatToDGV(dataGridViewTravel, "Scan value".T(EDTx.UserControlStats_Scanvalue), res[row++]);
            StatToDGV(dataGridViewTravel, "Organic Scans Value".T(EDTx.UserControlStats_OrganicScans), res[row++]);

            for (int i = 1; i < dataGridViewTravel.Columns.Count; i++)
                ColumnValueAlignment(dataGridViewTravel.Columns[i] as DataGridViewTextBoxColumn);

            if (sortcol < dataGridViewTravel.Columns.Count)
            {
                dataGridViewTravel.Sort(dataGridViewTravel.Columns[sortcol], (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                dataGridViewTravel.Columns[sortcol].HeaderCell.SortGlyphDirection = sortorder;
            }

            userControlStatsTimeTravel.Enabled = true;

            lasttraveltimemode = timemode;

            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCount} Turn back on timer after travel");
            timerupdate.Start();                                // we did an await above, we now turn the timer back on after the update
        }

        private static System.Threading.Tasks.Task<string[][]> ComputeTravel(JournalStats currentstat, Tuple<DateTime[], DateTime[]> tupletimes)
        {
            return System.Threading.Tasks.Task.Run(() =>
            {
                int results = 10;

                string[][] res = new string[results][];
                for (var i = 0; i < results; i++)
                    res[i] = new string[tupletimes.Item1.Length];

                for (var ii = 0; ii < tupletimes.Item1.Length; ii++)
                {
                    var startutc = tupletimes.Item1[ii];
                    var endutc = tupletimes.Item2[ii];

                    var fsdStats = currentstat.FSDJumps.Where(x => x.utc >= startutc && x.utc < endutc).ToList();
                    var jetconeboosts = currentstat.JetConeBoost.Where(x => x.utc >= startutc && x.utc < endutc).ToList();
                    var scanStats = currentstat.Scans.Values.Where(x => x.EventTimeUTC >= startutc && x.EventTimeUTC < endutc).ToList();
                    var saascancomplete = currentstat.SAAScanComplete.Values.Where(x => x.EventTimeUTC >= startutc && x.EventTimeUTC < endutc).ToList();
                    var organicscans = currentstat.OrganicScans.Where(x => x.EventTimeUTC >= startutc && x.EventTimeUTC < endutc).ToList();

                    int row = 0;
                    res[row++][ii] = fsdStats.Count.ToString("N0");
                    res[row++][ii] = fsdStats.Sum(j => j.jumpdist).ToString("N2");
                    res[row++][ii] = fsdStats.Where(j => j.boostvalue == 1).Count().ToString("N0");
                    res[row++][ii] = fsdStats.Where(j => j.boostvalue == 2).Count().ToString("N0");

                    res[row++][ii] = fsdStats.Where(j => j.boostvalue == 3).Count().ToString("N0");
                    res[row++][ii] = jetconeboosts.Count().ToString("N0");
                    res[row++][ii] = scanStats.Count.ToString("N0");       // scan count
                    res[row++][ii] = saascancomplete.Count().ToString("N0");   // mapped

                    res[row++][ii] = scanStats.Sum(x => (long)x.EstimatedValue).ToString("N0");
                    res[row++][ii] = organicscans.Sum(x => (long)(x.EstimatedValue ?? 0)).ToString("N0");
                    System.Diagnostics.Debug.Assert(row == results);
                }

                return res;
            });
        }
        private void userControlStatsTimeTravel_TimeModeChanged(object sender, EventArgs e)
        {
            dataGridViewTravel.Rows.Clear();        // reset all
            DGVSaveColumnLayout(dataGridViewTravel, dataGridViewTravel.Columns.Count <= 8 ? dbTravelSummary : dbTravelDWM);
            dataGridViewTravel.Columns.Clear();
            redisplay = true;
        }

        #endregion

        #region SCAN  ****************************************************************************************************************

        private StatsTimeUserControl.TimeModeType lastscantimemode = StatsTimeUserControl.TimeModeType.NotSet;

        async void StatsScan(JournalStats currentstat, DateTime endtimeutc )
        {
            userControlStatsTimeScan.Enabled = false;
            int sortcol = dataGridViewScan.SortedColumn?.Index ?? 0;
            SortOrder sortorder = dataGridViewScan.SortOrder;

            dataGridViewScan.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            var timemode = userControlStatsTimeScan.TimeMode;

            int intervals = timemode == StatsTimeUserControl.TimeModeType.Summary ? 6 : timemode == StatsTimeUserControl.TimeModeType.Year ? Math.Min(12, DateTime.Now.Year - EDDConfig.GameLaunchTimeUTC().Year + 1) : 12;

            var isTravelling = discoveryform.history.IsTravellingUTC(out var tripStartutc);
            if (isTravelling && tripStartutc > endtimeutc)
                isTravelling = false;

            var tupletimes = timemode == StatsTimeUserControl.TimeModeType.Summary ?
                                    SetupSummary(endtimeutc, isTravelling ? tripStartutc : DateTime.UtcNow, currentstat.lastdockedutc, dataGridViewScan, dbScanSummary) :
                             SetUpDaysMonthsYear(endtimeutc, dataGridViewScan, timemode, intervals, dbScanDWM);

            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCountLap("STATS", true)} Scan stats interval begin {timemode}");
            var res = await ComputeScans(currentstat, tupletimes, userControlStatsTimeScan.StarMode);
            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCountLap("STATS")} Scan stats interval end {timemode}");
            if (IsClosed)
                return;

            if (userControlStatsTimeScan.StarMode)
            {
                int row = 0;
                foreach (EDStar startype in Enum.GetValues(typeof(EDStar)))
                {
                    StatToDGV(dataGridViewScan, Bodies.StarName(startype), res[row++]);
                }
            }
            else
            {
                int row = 0;
                foreach (EDPlanet planettype in Enum.GetValues(typeof(EDPlanet)))
                {
                    StatToDGV(dataGridViewScan, planettype == EDPlanet.Unknown_Body_Type ? "Belt Cluster".T(EDTx.UserControlStats_Beltcluster) : Bodies.PlanetTypeName(planettype), res[row++]);

                }
            }

            for (int i = 1; i < dataGridViewScan.Columns.Count; i++)
                ColumnValueAlignment(dataGridViewScan.Columns[i] as DataGridViewTextBoxColumn);

            if (sortcol < dataGridViewScan.Columns.Count)
            {
                dataGridViewScan.Sort(dataGridViewScan.Columns[sortcol], (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                dataGridViewScan.Columns[sortcol].HeaderCell.SortGlyphDirection = sortorder;
            }

            userControlStatsTimeScan.Enabled = true;
            lastscantimemode = timemode;

            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCount} Stats Turn back on timer after scan");
            timerupdate.Start();                                // we did an await above, we now turn the timer back on after the update
        }

        private static System.Threading.Tasks.Task<string[][]> ComputeScans(JournalStats currentstat, Tuple<DateTime[], DateTime[]> tupletimes, bool starmode)
        {
            return System.Threading.Tasks.Task.Run(() =>
            {
                int results = starmode ? Enum.GetValues(typeof(EDStar)).Length : Enum.GetValues(typeof(EDPlanet)).Length;
                int intervals = tupletimes.Item1.Length;

                var scanlists = new List<JournalScan>[intervals];
                for (int ii = 0; ii < intervals; ii++)
                    scanlists[ii] = currentstat.Scans.Values.Where(x => x.EventTimeUTC >= tupletimes.Item1[ii] && x.EventTimeUTC < tupletimes.Item2[ii]).ToList();

                string[][] res = new string[results][];
                for (var i = 0; i < results; i++)
                    res[i] = new string[intervals];

                if (starmode)
                {
                    int row = 0;
                    foreach (EDStar startype in Enum.GetValues(typeof(EDStar)))
                    {
                        for (int ii = 0; ii < intervals; ii++)
                        {
                            int num = 0;
                            for (int jj = 0; jj < scanlists[ii].Count; jj++)
                            {
                                if (scanlists[ii][jj].StarTypeID == startype && scanlists[ii][jj].IsStar)
                                    num++;
                            }

                            res[row][ii] = num.ToString("N0");
                        }

                        row++;
                    }
                    System.Diagnostics.Debug.Assert(row == results);
                }
                else
                {
                    int row = 0;
                    foreach (EDPlanet planettype in Enum.GetValues(typeof(EDPlanet)))
                    {
                        for (int ii = 0; ii < intervals; ii++)
                        {
                            int num = 0;
                            for (int jj = 0; jj < scanlists[ii].Count; jj++)
                            {
                                // System.Diagnostics.Debug.WriteLine($"Planet for {planettype} {scanlists[ii][jj].PlanetTypeID} {scanlists[ii][jj].EventTimeUTC}");
                                if (scanlists[ii][jj].PlanetTypeID == planettype && !scanlists[ii][jj].IsStar)
                                    num++;
                            }

                            res[row][ii] = num.ToString("N0");
                        }
                        row++;
                    }
                    System.Diagnostics.Debug.Assert(row == results);
                }

                return res;
            });
        }

        private void userControlStatsTimeScan_TimeModeChanged(object sender, EventArgs e)
        {
            dataGridViewScan.Rows.Clear();        // reset all
            DGVSaveColumnLayout(dataGridViewScan, dataGridViewScan.Columns.Count <= 8 ? dbScanSummary : dbScanDWM);
            dataGridViewScan.Columns.Clear();
            redisplay = true;
        }

        #endregion

        #region Combat  ****************************************************************************************************************************

        private StatsTimeUserControl.TimeModeType lastcombattimemode = StatsTimeUserControl.TimeModeType.NotSet;
        async void StatsCombat(JournalStats currentstat, DateTime endtimeutc)
        {
            statsTimeUserControlCombat.Enabled = false;

            int sortcol = dataGridViewCombat.SortedColumn?.Index ?? 99;
            SortOrder sortorder = dataGridViewCombat.SortOrder;

            var timemode = statsTimeUserControlCombat.TimeMode;

            int intervals = timemode == StatsTimeUserControl.TimeModeType.Summary ? 6 : timemode == StatsTimeUserControl.TimeModeType.Year ? Math.Min(12, DateTime.Now.Year - EDDConfig.GameLaunchTimeUTC().Year + 1) : 12;

            var isTravelling = discoveryform.history.IsTravellingUTC(out var tripStartutc);
            if (isTravelling && tripStartutc > endtimeutc)
                isTravelling = false;

            var tupletimes = timemode == StatsTimeUserControl.TimeModeType.Summary ?
                                    SetupSummary(endtimeutc, isTravelling ? tripStartutc : DateTime.UtcNow, currentstat.lastdockedutc, dataGridViewCombat, dbCombatSummary) :
                             SetUpDaysMonthsYear(endtimeutc, dataGridViewCombat, timemode, intervals, dbCombatDWM);

            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCountLap("STATS", true)} Combat stats interval begin {timemode}");
            var res = await ComputeCombat(currentstat, tupletimes);
            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCountLap("STATS")} Combat stats interval end {timemode}");
            if (IsClosed)
                return;

            int row = 0;
            StatToDGV(dataGridViewCombat, "PVP Kills".T(EDTx.UserControlStats_Jumps), res[row++]);
            StatToDGV(dataGridViewCombat, "PVP Elite".T(EDTx.UserControlStats_Jumps), res[row++]);
            StatToDGV(dataGridViewCombat, "PVP Deadly".T(EDTx.UserControlStats_Jumps), res[row++]);
            StatToDGV(dataGridViewCombat, "PVP Dangerous".T(EDTx.UserControlStats_Jumps), res[row++]);
            StatToDGV(dataGridViewCombat, "PVP <= Master".T(EDTx.UserControlStats_Jumps), res[row++]);
            StatToDGV(dataGridViewCombat, "Bounties".T(EDTx.UserControlStats_Jumps), res[row++]);
            StatToDGV(dataGridViewCombat, "Bounty Value".T(EDTx.UserControlStats_Jumps), res[row++]);
            StatToDGV(dataGridViewCombat, "Thargoids".T(EDTx.UserControlStats_Jumps), res[row++]);
            StatToDGV(dataGridViewCombat, "Crimes".T(EDTx.UserControlStats_Jumps), res[row++]);
            StatToDGV(dataGridViewCombat, "Crime Cost".T(EDTx.UserControlStats_Jumps), res[row++]);
            StatToDGV(dataGridViewCombat, "Faction Kill Bonds".T(EDTx.UserControlStats_Jumps), res[row++]);
            StatToDGV(dataGridViewCombat, "FKB Value".T(EDTx.UserControlStats_Jumps), res[row++]);
            StatToDGV(dataGridViewCombat, "Interdictions Player Succeeded".T(EDTx.UserControlStats_Jumps), res[row++]);
            StatToDGV(dataGridViewCombat, "Interdictions Player Failed".T(EDTx.UserControlStats_Jumps), res[row++]);
            StatToDGV(dataGridViewCombat, "Interdictions NPC Succeeded".T(EDTx.UserControlStats_Jumps), res[row++]);
            StatToDGV(dataGridViewCombat, "Interdictions NPC Failed".T(EDTx.UserControlStats_Jumps), res[row++]);
            StatToDGV(dataGridViewCombat, "Interdicted Player Succeeded".T(EDTx.UserControlStats_Jumps), res[row++]);
            StatToDGV(dataGridViewCombat, "Interdicted Player Failed".T(EDTx.UserControlStats_Jumps), res[row++]);
            StatToDGV(dataGridViewCombat, "Interdicted NPC Succeeded".T(EDTx.UserControlStats_Jumps), res[row++]);
            StatToDGV(dataGridViewCombat, "Interdicted NPC Failed".T(EDTx.UserControlStats_Jumps), res[row++]);

            if (sortcol < dataGridViewCombat.Columns.Count)
            {
                dataGridViewCombat.Sort(dataGridViewCombat.Columns[sortcol], (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                dataGridViewCombat.Columns[sortcol].HeaderCell.SortGlyphDirection = sortorder;
            }

            statsTimeUserControlCombat.Enabled = true;
            lastcombattimemode = timemode;

            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCount} Stats Turn back on timer after combat");
            timerupdate.Start();                                // we did an await above, we now turn the timer back on after the update
        }

        private static System.Threading.Tasks.Task<string[][]> ComputeCombat(JournalStats currentstat, Tuple<DateTime[], DateTime[]> tupletimes)
        {
            return System.Threading.Tasks.Task.Run(() =>
            {
                int results = 20;
                int intervals = tupletimes.Item1.Length;

                string[][] res = new string[results][];
                for (var i = 0; i < results; i++)
                    res[i] = new string[intervals];

                for (var ii = 0; ii < intervals; ii++)
                {
                    var startutc = tupletimes.Item1[ii];
                    var endutc = tupletimes.Item2[ii];

                    var pvpStats = currentstat.PVPKills.Where(x => x.EventTimeUTC >= startutc && x.EventTimeUTC < endutc).ToList();
                    var bountyStats = currentstat.Bounties.Where(x => x.EventTimeUTC >= startutc && x.EventTimeUTC < endutc).ToList();
                    var crimesStats = currentstat.Crimes.Where(x => x.EventTimeUTC >= startutc && x.EventTimeUTC < endutc).ToList();
                    var sfactionkillbonds = currentstat.FactionKillBonds.Where(x => x.EventTimeUTC >= startutc && x.EventTimeUTC < endutc).ToList();
                    var interdictions = currentstat.Interdiction.Where(x => x.EventTimeUTC >= startutc && x.EventTimeUTC < endutc).ToList();
                    var interdicted = currentstat.Interdicted.Where(x => x.EventTimeUTC >= startutc && x.EventTimeUTC < endutc).ToList();

                    int row = 0;
                    res[row++][ii] = pvpStats.Count.ToString("N0");
                    res[row++][ii] = pvpStats.Where(x => x.CombatRank >= CombatRank.Elite).Count().ToString("N0");
                    res[row++][ii] = pvpStats.Where(x => x.CombatRank == CombatRank.Deadly).Count().ToString("N0");
                    res[row++][ii] = pvpStats.Where(x => x.CombatRank == CombatRank.Dangerous).Count().ToString("N0");
                    res[row++][ii] = pvpStats.Where(x => x.CombatRank <= CombatRank.Master).Count().ToString("N0");

                    res[row++][ii] = bountyStats.Count.ToString("N0");                                        
                    res[row++][ii] = bountyStats.Select(x => x.TotalReward).Sum().ToString("N0");
                    res[row++][ii] = bountyStats.Where(x => x.IsThargoid).Count().ToString("N0");

                    res[row++][ii] = crimesStats.Count.ToString("N0");
                    res[row++][ii] = crimesStats.Select(x => x.Cost).Sum().ToString("N0");

                    res[row++][ii] = sfactionkillbonds.Count.ToString("N0");
                    res[row++][ii] = sfactionkillbonds.Select(x => x.Reward).Sum().ToString("N0");

                    res[row++][ii] = interdictions.Where(x => x.Success && x.IsPlayer).Count().ToString("N0");
                    res[row++][ii] = interdictions.Where(x => !x.Success && x.IsPlayer).Count().ToString("N0");
                    res[row++][ii] = interdictions.Where(x => x.Success && !x.IsPlayer).Count().ToString("N0");
                    res[row++][ii] = interdictions.Where(x => !x.Success && !x.IsPlayer).Count().ToString("N0");

                    res[row++][ii] = interdicted.Where(x => x.Submitted && x.IsPlayer).Count().ToString("N0");
                    res[row++][ii] = interdicted.Where(x => !x.Submitted && x.IsPlayer).Count().ToString("N0");
                    res[row++][ii] = interdicted.Where(x => x.Submitted && !x.IsPlayer).Count().ToString("N0");
                    res[row++][ii] = interdicted.Where(x => !x.Submitted && !x.IsPlayer).Count().ToString("N0");

                    System.Diagnostics.Debug.Assert(row == results);
                }

                return res;
            });
        }

        private void statsTimeUserControlCombat_TimeModeChanged(object sender, EventArgs e)
        {
            dataGridViewCombat.Rows.Clear();        // reset all
            DGVSaveColumnLayout(dataGridViewCombat, dataGridViewTravel.Columns.Count <= 8 ? dbCombatSummary : dbCombatDWM);
            dataGridViewCombat.Columns.Clear();
            redisplay = true;
        }

        #endregion


        #region STATS IN GAME *************************************************************************************************************************

        private JournalStatistics lastgamestatsdisplayed = null;

        void StatsGame(JournalStats currentstat)
        {
            string collapseExpand = GameStatTreeState();

            if (string.IsNullOrEmpty(collapseExpand))
                collapseExpand = GetSetting(dbStatsTreeStateSave, "YYYYYYYYYYYYYYYY");

            if (collapseExpand.Length < 16)
                collapseExpand += new string('Y', 16);

            JournalStatistics stats = currentstat.laststats;

            if (stats != null) // may not have one
            {
                AddTreeList("N1", "@", new string[] { EDDConfig.Instance.ConvertTimeToSelectedFromUTC(stats.EventTimeUTC).ToString() }, collapseExpand[0]);

                AddTreeList("N2", "Bank Account".T(EDTx.UserControlStats_BankAccount), stats.BankAccount.Format("").Split(Environment.NewLine),collapseExpand[1]);
                AddTreeList("N3", "Combat".T(EDTx.UserControlStats_Combat), stats.Combat.Format("").Split(Environment.NewLine), collapseExpand[2]);
                AddTreeList("N4", "Crime".T(EDTx.UserControlStats_Crime), stats.Crime.Format("").Split(Environment.NewLine), collapseExpand[3]);
                AddTreeList("N5", "Smuggling".T(EDTx.UserControlStats_Smuggling), stats.Smuggling.Format("").Split(Environment.NewLine),collapseExpand[4]);

                AddTreeList("N6", "Trading".T(EDTx.UserControlStats_Trading), stats.Trading.Format("").Split(Environment.NewLine), collapseExpand[5]);
                AddTreeList("N7", "Mining".T(EDTx.UserControlStats_Mining), stats.Mining.Format("").Split(Environment.NewLine),collapseExpand[6]);
                AddTreeList("N8", "Exploration".T(EDTx.UserControlStats_Exploration), stats.Exploration.Format("").Split(Environment.NewLine),collapseExpand[7]);
                AddTreeList("N9", "Passengers".T(EDTx.UserControlStats_Passengers), stats.PassengerMissions.Format("").Split(Environment.NewLine), collapseExpand[8]);

                AddTreeList("N10", "Search and Rescue".T(EDTx.UserControlStats_SearchandRescue), stats.SearchAndRescue.Format("").Split(Environment.NewLine), collapseExpand[9]);
                AddTreeList("N11", "Crafting".T(EDTx.UserControlStats_Crafting), stats.Crafting.Format("").Split(Environment.NewLine), collapseExpand[10]);
                AddTreeList("N12", "Crew".T(EDTx.UserControlStats_Crew), stats.Crew.Format("").Split(Environment.NewLine), collapseExpand[11]);
                AddTreeList("N13", "Multi-crew".T(EDTx.UserControlStats_Multi), stats.Multicrew.Format("").Split(Environment.NewLine), collapseExpand[12]);

                AddTreeList("N14", "Materials Trader".T(EDTx.UserControlStats_MaterialsTrader), stats.MaterialTraderStats.Format("").Split(Environment.NewLine), collapseExpand[13]);
                AddTreeList("N15", "CQC".T(EDTx.UserControlStats_CQC), stats.CQC.Format("").Split(Environment.NewLine), collapseExpand[14]);
                AddTreeList("N16", "Fleetcarrier".T(EDTx.UserControlStats_FLEETCARRIER), stats.FLEETCARRIER.Format("").Split(Environment.NewLine), collapseExpand[15]);
                AddTreeList("N17", "Exobiology".T(EDTx.UserControlStats_Exobiology), stats.Exobiology.Format("").Split(Environment.NewLine), collapseExpand[16]);
            }
            else
                treeViewStats.Nodes.Clear();

            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCount} Game Stats displayed");
            lastgamestatsdisplayed = stats;
        }

        // idea is to populate the tree, then next time, just replace the text of the children.
        // so the tree does not get wiped and refilled, keeping the current view pos, etc.

        TreeNode AddTreeList(string parentid, string parenttext, string[] children, char ce)
        {
            TreeNode[] parents = treeViewStats.Nodes.Find(parentid, false);                     // find parent id in tree
            TreeNode pnode = (parents.Length == 0) ? (treeViewStats.Nodes.Add(parentid,parenttext)) : parents[0];  // if not found, add it, else get it

            int eno = 0;
            foreach( var childtext in children)
            {
                string childid = parentid + "-" + (eno++).ToString();       // make up a child id
                TreeNode[] childs = pnode.Nodes.Find(childid, false);       // find it..
                if (childs.Length > 0)                                      // found
                    childs[0].Text = childtext;                             // reset text
                else
                    pnode.Nodes.Add(childid, childtext);                    // else set the text
            }

            if (ce == 'Y')
                pnode.Expand();

            return pnode;
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
                result = GetSetting(dbStatsTreeStateSave, "YYYYYYYYYYYYY");
            return result;
        }
        #endregion

        #region STATS_BY_SHIP ****************************************************************************************************************************

        private bool laststatsbyshipdisplayed = false;
        void StatsByShip(JournalStats currentstat)
        {
            int sortcol = dataGridViewByShip.SortedColumn?.Index ?? 0;
            SortOrder sortorder = dataGridViewByShip.SortOrder;

            if (dataGridViewByShip.Columns.Count == 0)
            {
                dataGridViewByShip.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Type".T(EDTx.UserControlStats_Type), Tag = "AlphaSort" });
                dataGridViewByShip.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Name".T(EDTx.UserControlStats_Name), Tag = "AlphaSort" });
                dataGridViewByShip.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Ident".T(EDTx.UserControlStats_Ident), Tag = "AlphaSort" });
                dataGridViewByShip.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Jumps".T(EDTx.UserControlStats_Jumps) });
                dataGridViewByShip.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Travelled Ly".T(EDTx.UserControlStats_TravelledLy) });
                dataGridViewByShip.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Bodies Scanned".T(EDTx.UserControlStats_BodiesScanned) });
                dataGridViewByShip.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Destroyed".T(EDTx.UserControlStats_Destroyed) });
                DGVLoadColumnLayout(dataGridViewByShip, dbShip);
            }

            string[] strarr = new string[6];

            dataGridViewByShip.Rows.Clear();

            foreach (var kvp in currentstat.Ships)
            {
                var fsd = currentstat.FSDJumps.Where(x => x.shipid == kvp.Key);
                var scans = currentstat.Scans.Values.Where(x => x.ShipIDForStatsOnly == kvp.Key);
                strarr[0] = kvp.Value.name?? "-";
                strarr[1] = kvp.Value.ident ?? "-";

                strarr[2] = fsd.Count().ToString();
                strarr[3] = fsd.Sum(x => x.jumpdist).ToString("N0");
                strarr[4] = scans.Count().ToString("N0");
                strarr[5] = kvp.Value.died.ToString();
                StatToDGV(dataGridViewByShip, kvp.Key, strarr, true);
            }

            if (sortcol < dataGridViewByShip.Columns.Count)
            {
                dataGridViewByShip.Sort(dataGridViewByShip.Columns[sortcol], (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                dataGridViewByShip.Columns[sortcol].HeaderCell.SortGlyphDirection = sortorder;
            }

            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCount} Stats by ship displayed");
            laststatsbyshipdisplayed = true;
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

        void StatToDGV(DataGridView datagrid, string title, string[] data, bool addatend = false)
        {
            object[] rowobj = new object[data.Length + 1];

            rowobj[0] = title;

            for (int ii = 0; ii < data.Length; ii++)
            {
                rowobj[ii + 1] = data[ii];
            }

            int rowpresent = datagrid.FindRowWithValue(0, title);

            if (rowpresent != -1 && !addatend)
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

        // set up a time date array with the limit times in utc, and set up the grid view columns
        private Tuple<DateTime[],DateTime[]> SetupSummary(DateTime endtimenowutc, DateTime triptimeutc, DateTime lastdockedutc, DataGridView view, string dbnameforlayout)
        {
            DateTime[] starttimeutc = new DateTime[6];
            DateTime[] endtimeutc = new DateTime[6];
            starttimeutc[0] = endtimenowutc.AddDays(-1).AddSeconds(1);
            starttimeutc[1] = endtimenowutc.AddDays(-7).AddSeconds(1);
            starttimeutc[2] = endtimenowutc.AddMonths(-1).AddSeconds(1);
            starttimeutc[3] = lastdockedutc;
            starttimeutc[4] = triptimeutc;
            starttimeutc[5] = EDDConfig.GameLaunchTimeUTC();
            endtimeutc[0] = endtimeutc[1] = endtimeutc[2] = endtimeutc[3] = endtimeutc[4] = endtimeutc[5] = endtimenowutc;

            if (view.Columns.Count == 0)
            {
                view.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Type".T(EDTx.UserControlStats_Type), Tag = "AlphaSort" });
                for (int i = 0; i < starttimeutc.Length; i++)
                    view.Columns.Add(new DataGridViewTextBoxColumn());          // day

                DGVLoadColumnLayout(view, dbnameforlayout);     // reload layout
            }

            view.Columns[1].HeaderText = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(starttimeutc[0]).ToShortDateString();
            view.Columns[2].HeaderText = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(starttimeutc[1]).ToShortDateString();
            view.Columns[3].HeaderText = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(starttimeutc[2]).ToShortDateString();
            view.Columns[4].HeaderText = "Last dock".T(EDTx.UserControlStats_Lastdock);
            view.Columns[5].HeaderText = "Trip".T(EDTx.UserControlStats_Trip);
            view.Columns[6].HeaderText = "All".T(EDTx.UserControlStats_All);

            //for (int i = 0; i < starttimeutc.Length; i++)  System.Diagnostics.Debug.WriteLine($"Time {starttimeutc[i].ToString()} - {endtimeutc[i].ToString()} {starttimeutc[i].Kind}");

            return new Tuple<DateTime[], DateTime[]>(starttimeutc, endtimeutc);
        }

        // set up a time date array with the limit times in utc over months, and set up the grid view columns
        private Tuple<DateTime[], DateTime[]> SetUpDaysMonthsYear(DateTime endtimenowutc, DataGridView view, StatsTimeUserControl.TimeModeType timemode, 
                                                                int intervals, string dbname)
        {
            if (view.Columns.Count == 0)
            {
                var Col1 = new DataGridViewTextBoxColumn();
                Col1.HeaderText = "Type".T(EDTx.UserControlStats_Type);
                Col1.Tag = "AlphaSort";
                view.Columns.Add(Col1);
                for (int i = 0; i < intervals; i++)
                    view.Columns.Add(new DataGridViewTextBoxColumn());          // day

                DGVLoadColumnLayout(view, dbname);
            }

            DateTime[] starttimeutc = new DateTime[intervals];
            DateTime[] endtimeutc = new DateTime[intervals];

            for (int ii = 0; ii < intervals; ii++)
            {
                if (ii == 0)
                {
                    if (timemode == StatsTimeUserControl.TimeModeType.Month)
                        endtimeutc[0] = endtimenowutc.EndOfMonth().AddSeconds(1);
                    else if (timemode == StatsTimeUserControl.TimeModeType.Week)
                        endtimeutc[0] = endtimenowutc.EndOfWeek().AddSeconds(1);
                    else if (timemode == StatsTimeUserControl.TimeModeType.Year)
                        endtimeutc[0] = endtimenowutc.EndOfYear().AddSeconds(1);
                    else
                        endtimeutc[0] = endtimenowutc.AddSeconds(1);
                }
                else
                    endtimeutc[ii] = starttimeutc[ii - 1];

                starttimeutc[ii] = timemode == StatsTimeUserControl.TimeModeType.Day ? endtimeutc[ii].AddDays(-1) :
                                timemode == StatsTimeUserControl.TimeModeType.Week ? endtimeutc[ii].AddDays(-7) :
                                timemode == StatsTimeUserControl.TimeModeType.Year ? endtimeutc[ii].AddYears(-1) :
                                endtimeutc[ii].AddMonths(-1);

                var stime = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(starttimeutc[ii]);
                view.Columns[ii + 1].HeaderText = timemode == StatsTimeUserControl.TimeModeType.Month ? stime.ToString("MM/yyyy") : 
                        timemode == StatsTimeUserControl.TimeModeType.Year ? stime.ToString("yyyy") : 
                        stime.ToShortDateString();
            }

            //for (int i = 0; i < starttimeutc.Length; i++)  System.Diagnostics.Debug.WriteLine($"Time {starttimeutc[i].ToString()} - {endtimeutc[i].ToString()} {starttimeutc[i].Kind}");

            return new Tuple<DateTime[], DateTime[]>(starttimeutc, endtimeutc);
        }


        private void dataGridView_SortCompareNull(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Tag == null)     // tag null means numeric sort.
                e.SortDataGridViewColumnNumeric();
        }

        #endregion
    }

}
