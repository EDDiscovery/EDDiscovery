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
        private string dbScan = "Scan";     // these have with Summary or DWM on end
        private string dbTravel = "Travel";
        private string dbCombat = "Combat";     
        private string dbShip = "Ship";
        private string dbSCCombat = "SCCombat";
        private string dbSCGeneral = "SCGeneral";
        private string dbSCLedger = "SCLedger";
        private string dbSCScan = "SCScan";
        private string dbSCShip = "SCShip";

        private bool endchecked, startchecked;

        private JournalStatisticsComputer statscomputer = new JournalStatisticsComputer();
        private JournalStats currentstats;
        private JournalStats pendingstats;
        private bool redisplay = false;

        private Timer timerupdate;

        // because of the async awaits, we are in effect multithreaded, best to have a concurrent queue
        private ConcurrentQueue<JournalEntry> entriesqueued = new ConcurrentQueue<JournalEntry>();

        private class DataReturn        // used to pass data from thread back to user control
        {
            public string[][] griddata;
            public int[][] chart1data;
            public string[] chart2labels;
            public int[][] chart2data;
            public string[] chart1labels;
        }

        static Color[] piechartcolours = new Color[] { Color.Red, Color.Green, Color.Blue, Color.DarkCyan, Color.Magenta, Color.Brown, Color.Orange, Color.Yellow, Color.Fuchsia };


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
                                        EDTx.UserControlStats_tabControlCustomStats_tabPageRanks, EDTx.UserControlStats_tabControlCustomStats_tabPageRanks_dataGridViewTextBoxColumnRank, EDTx.UserControlStats_tabControlCustomStats_tabPageRanks_dataGridViewTextBoxColumnAtStart, EDTx.UserControlStats_tabControlCustomStats_tabPageRanks_dataGridViewTextBoxAtEnd, 
                                        EDTx.UserControlStats_tabControlCustomStats_tabPageRanks_dataGridViewTextBoxColumnLastPromotionDate,EDTx.UserControlStats_tabControlCustomStats_tabPageRanks_dataGridViewTextColumnRankProgressNumeric,
                                        EDTx.UserControlStats_tabControlCustomStats_tabPageGameStats,
                                        EDTx.UserControlStats_tabControlCustomStats_tabPageByShip, EDTx.UserControlStats_labelStart, EDTx.UserControlStats_labelEndDate,
                                        EDTx.UserControlStats_tabControlCustomStats_tabPageCombat,
                                        EDTx.UserControlStats_tabControlCustomStats_tabPageLedger, EDTx.UserControlStats_tabControlCustomStats_tabPageLedger_dataGridViewTextBoxColumnLedgerDate,
                                        EDTx.UserControlStats_tabControlCustomStats_tabPageLedger_dataGridViewTextBoxColumnNumericCredits
            };

            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);

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

            splitContainerCombat.SplitterDistance(GetSetting(dbSCCombat, 0.5));
            splitContainerGeneral.SplitterDistance(GetSetting(dbSCGeneral, 0.5));
            splitContainerLedger.SplitterDistance(GetSetting(dbSCLedger, 0.5));
            splitContainerScan.SplitterDistance(GetSetting(dbSCScan, 0.5));
            splitContainerShips.SplitterDistance(GetSetting(dbSCShip, 0.5));


            userControlStatsTimeTravel.TimeModeChanged += userControlStatsTimeTravel_TimeModeChanged;
            userControlStatsTimeScan.TimeModeChanged += userControlStatsTimeScan_TimeModeChanged;
            userControlStatsTimeScan.StarPlanetModeChanged += userControlStatsTimeScan_StarPlanetModeChanged;
            statsTimeUserControlCombat.TimeModeChanged += userControlStatsTimeCombat_TimeModeChanged;

            // set the charts up before themeing so the themer helps us out

            {
                extChartTravelDest.AddChartArea("TravelCA1");
                extChartTravelDest.AddSeries("TravelS1", "TravelCA1", SeriesChartType.Column);
                extChartTravelDest.AddTitle("MV1", "Most Visited".T(EDTx.UserControlStats_MostVisited), dockingpos: Docking.Top);
            }

            {
                extChartLedger.AddChartArea("LedgerCA1");
                extChartLedger.AddSeries("LedgerS1", "LedgerCA1", SeriesChartType.Line);
                extChartLedger.EnableZoomMouseWheelX();
                extChartLedger.ZoomMouseWheelXMinimumInterval = 5.0 / 60.0 / 24.0;

                extChartLedger.SetXAxisInterval(DateTimeIntervalType.Days, 0, IntervalAutoMode.VariableCount);
                extChartLedger.SetXAxisFormat("g");

                extChartLedger.XCursorShown();
                extChartLedger.XCursorSelection();
                extChartLedger.SetXCursorInterval(1, DateTimeIntervalType.Seconds);

                extChartLedger.YAutoScale();
                extChartLedger.SetYAxisFormat("N0");

                extChartLedger.ShowSeriesMarkers(MarkerStyle.Diamond);

                extChartLedger.AddContextMenu(new string[] { "Zoom out by 1", "Reset Zoom" },
                                    new Action<ToolStripMenuItem>[]
                                        { new Action<ToolStripMenuItem>((s)=> {extChartLedger.ZoomOutX(); } ),
                                              new Action<ToolStripMenuItem>((s)=> {extChartLedger.ZoomResetX(); } ),
                                        },
                                    new Action<ToolStripMenuItem[]>((list) =>
                                    {
                                        list[0].Enabled = list[1].Enabled = extChartLedger.IsZoomedX;
                                    })
                                    );

                extChartLedger.CursorPositionChanged = LedgerCursorPositionChanged;
            }

            {
                const int cw = 48;
                const int ch = 96;
                const int ct = 2;
                const int c1l = 1;
                const int c2l = 51;
                const int lw = 15;
                const float titley = 3;
                const float titleh = 7;
                const float butw = 1.5f;
                const float buth = 7;
                const float ctc = 50;
                const float ctw = 15;

                extChartCombat.AddChartArea("CA-CA1", new ElementPosition(c1l, ct, cw, ch));
                extChartCombat.SetChartArea3DStyle(new ChartArea3DStyle() { Inclination = 15, Enable3D = true, Rotation = -90, LightStyle = LightStyle.Simplistic });
                extChartCombat.SetChartAreaPlotArea(new ElementPosition(lw * 2, 1, 100 - lw * 2, 98));       // its *2 because lw is specified in whole chart terms, and this is in chart area terms
                extChartCombat.AddLegend("CA-L1", position: new ElementPosition(c1l + 0.2f, ct + 0.5f, lw, ch - 1.5f));
                extChartCombat.AddSeries("CA-S1", "CA-CA1", SeriesChartType.Pie, legend: "CA-L1");

                extChartCombat.AddChartArea("CA-CA2", new ElementPosition(c2l, ct, cw, ch));
                extChartCombat.SetChartAreaPlotArea(new ElementPosition(0, 1, 100 - lw * 2, 98));
                extChartCombat.SetChartArea3DStyle(new ChartArea3DStyle() { Inclination = 15, Enable3D = true, Rotation = -90, LightStyle = LightStyle.Simplistic });
                extChartCombat.AddLegend("CA-L2", position: new ElementPosition(c2l + cw - lw - 0.2f, ct + 0.5f, lw, ch - 1.5f));
                extChartCombat.AddSeries("CA-S2", "CA-CA2", SeriesChartType.Pie, legend: "CA-L2");

                extChartCombat.AddTitle("CA-T1", "", alignment: ContentAlignment.MiddleCenter, position: new ElementPosition(ctc - ctw / 2, titley, ctw, titleh));
                extChartCombat.LeftArrowPosition = new ElementPosition(ctc - ctw / 2 - butw - 0.1f, titley, butw, buth);
                extChartCombat.RightArrowPosition = new ElementPosition(ctc + ctw / 2 + 0.1f, titley, butw, buth);
                extChartCombat.ArrowButtonPressed += CombatChartArrowPressed;
                extChartCombat.LeftArrowEnable = extChartCombat.RightArrowEnable = false;     // disable

                extChartScan.AddChartArea("SC-CA1", new ElementPosition(c1l, ct, cw * 2, ch));
                extChartScan.SetChartAreaPlotArea(new ElementPosition(lw, 1, 100 - lw * 2, 98));        // 1 chart, so lw does not need scaling
                extChartScan.SetChartArea3DStyle(new ChartArea3DStyle() { Inclination = 15, Enable3D = true, Rotation = -90, LightStyle = LightStyle.Simplistic });
                extChartScan.AddLegend("SC-L1", position: new ElementPosition(c1l + 0.2f, ct + 0.5f, lw, ch - 1.5f));
                extChartScan.AddSeries("SC-S1", "SC-CA1", SeriesChartType.Pie, legend: "SC-L1");

                const float stw = 10;
                float stc = (c1l + 0.2f) + lw + stw / 2 + butw + 1f;
                extChartScan.AddTitle("SC-T1", "", alignment: ContentAlignment.MiddleCenter, position: new ElementPosition(stc - stw / 2, titley, stw, titleh));
                extChartScan.LeftArrowPosition = new ElementPosition(stc - stw / 2 - butw - 0.1f, titley, butw, buth);
                extChartScan.RightArrowPosition = new ElementPosition(stc + stw / 2 + 0.1f, titley, butw, buth);
                extChartScan.ArrowButtonPressed += ScanChartArrowPressed;
                extChartScan.LeftArrowEnable = extChartScan.RightArrowEnable = false;     // disable
            }

            {
                const int c1l = 21;
                const int ct = 2;
                const int cw = 25;
                const int ch = 96;
                extChartShips.AddChartArea("SH-CA1", new ElementPosition(c1l, ct, cw, ch));
                extChartShips.SetChartArea3DStyle(new ChartArea3DStyle() { Inclination = 15, Enable3D = true, Rotation = -90, LightStyle = LightStyle.Simplistic });
                extChartShips.AddLegend("SH-L1", position: new ElementPosition(0, ct, c1l-1, ch));
                extChartShips.AddSeries("SH-S1", "SH-CA1", SeriesChartType.Pie, legend: "SH-L1");
                extChartShips.AddTitle("SH-T1", "", alignment: ContentAlignment.MiddleLeft, position: new ElementPosition(c1l+1f, 3f, 8, 5), bordercolor: ExtendedControls.ExtSafeChart.Disable);

                extChartShips.AddChartArea("SH-CA2", new ElementPosition(c1l+cw+0.5f, ct, cw, ch));
                extChartShips.SetChartArea3DStyle(new ChartArea3DStyle() { Inclination = 15, Enable3D = true, Rotation = -90, LightStyle = LightStyle.Simplistic });
                extChartShips.AddSeries("SH-S2", "SH-CA2", SeriesChartType.Pie);      // setting legend blank means don't show
                extChartShips.AddTitle("SH-T2", "", alignment: ContentAlignment.MiddleLeft, position: new ElementPosition(c1l + cw + 1f, 3f, 10, 5), bordercolor: ExtendedControls.ExtSafeChart.Disable);

                extChartShips.AddChartArea("SH-CA3", new ElementPosition(c1l+cw+cw+1f, ct, cw, ch));
                extChartShips.SetChartArea3DStyle(new ChartArea3DStyle() { Inclination = 15, Enable3D = true, Rotation = -90, LightStyle = LightStyle.Simplistic });
                extChartShips.AddSeries("SH-S3", "SH-CA3", SeriesChartType.Pie);
                extChartShips.AddTitle("SH-T3", "", alignment: ContentAlignment.MiddleLeft, position: new ElementPosition(c1l + cw + cw + 1.5f, 3f, 10, 5), bordercolor: ExtendedControls.ExtSafeChart.Disable);

            }

        }

        // themeing has been performed
        public override void InitialDisplay()
        {
            tabControlCustomStats.TabStyle = new ExtendedControls.TabStyleSquare();        // after themeing, set to differentiate

            dataGridViewTravel.RowTemplate.MinimumHeight =
                        dataGridViewScan.RowTemplate.MinimumHeight =
                        dataGridViewCombat.RowTemplate.MinimumHeight =
                        dataGridViewByShip.RowTemplate.MinimumHeight =
                        dataGridViewGeneral.RowTemplate.MinimumHeight = Font.ScalePixels(24);

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
            timerupdate.Stop();
            statscomputer.Stop();

            PutSetting(dbStatsTreeStateSave, GameStatTreeState());

            if (dataGridViewTravel.Columns.Count > 0)
                DGVSaveColumnLayout(dataGridViewTravel, dbTravel + userControlStatsTimeTravel.TimeMode.ToString());

            if (dataGridViewScan.Columns.Count > 0)    
                DGVSaveColumnLayout(dataGridViewScan, dbScan + userControlStatsTimeScan.TimeMode.ToString());

            if (dataGridViewCombat.Columns.Count > 0)
                DGVSaveColumnLayout(dataGridViewCombat, dbCombat + statsTimeUserControlCombat.TimeMode.ToString());

            if (dataGridViewByShip.Columns.Count > 0)
                DGVSaveColumnLayout(dataGridViewByShip, dbShip);

            PutSetting(dbSCCombat, splitContainerCombat.GetSplitterDistance());
            PutSetting(dbSCGeneral, splitContainerGeneral.GetSplitterDistance());
            PutSetting(dbSCLedger, splitContainerLedger.GetSplitterDistance());
            PutSetting(dbSCScan, splitContainerScan.GetSplitterDistance());
            PutSetting(dbSCShip, splitContainerShips.GetSplitterDistance());

            discoveryform.OnNewEntry -= AddNewEntry;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
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

          //  System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCount} Tick {newstats} {enqueued} {redisplay}");

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
                        laststatsgeneraldisplayed = laststatsbyshipdisplayed = laststatsledgerdisplayed = laststatsrankdisplayed= false;
                    }

                    DateTime starttimeutc = dateTimePickerStartDate.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(dateTimePickerStartDate.Value.StartOfDay()) :
                                                                            EDDConfig.GameLaunchTimeUTC();
                    DateTime endtimeutc = dateTimePickerEndDate.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(dateTimePickerEndDate.Value.EndOfDay()) :
                                                                            EDDConfig.Instance.SelectedEndOfTodayUTC();


                    if (tabControlCustomStats.SelectedTab == tabPageGeneral)
                    {
                        if (laststatsgeneraldisplayed == false)
                            StatsGeneral(currentstats, endtimeutc);
                    }
                    if (tabControlCustomStats.SelectedTab == tabPageRanks)
                    {
                        if (laststatsrankdisplayed == false)
                            StatsRanks(currentstats, endtimeutc);
                    }
                    if (tabControlCustomStats.SelectedTab == tabPageLedger)
                    {
                        if (laststatsledgerdisplayed == false)
                            StatsLedger(currentstats);
                    }
                    else if (tabControlCustomStats.SelectedTab == tabPageTravel)
                    {
                        if ( lasttraveltimemode != userControlStatsTimeTravel.TimeMode)
                        {
                            turnontimer = false;
                            StatsTravel(currentstats, starttimeutc, endtimeutc);
                        }
                    }
                    else if (tabControlCustomStats.SelectedTab == tabPageScan)
                    {
                        if (lastscantimemode != userControlStatsTimeScan.TimeMode || lastscanstarmode != userControlStatsTimeScan.StarMode)
                        {
                            turnontimer = false;
                            StatsScan(currentstats, starttimeutc, endtimeutc);
                        }
                    }
                    else if (tabControlCustomStats.SelectedTab == tabPageCombat)
                    {
                        if (lastcombattimemode != statsTimeUserControlCombat.TimeMode)
                        {
                            turnontimer = false;
                            StatsCombat(currentstats, starttimeutc, endtimeutc);
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

                if (turnontimer)
                {
                    timerupdate.Start();
                    labelStatus.Text = "";  // not working now
                }
            }

        }

        #endregion

        #region Stats General **************************************************************************************************************

        private bool laststatsgeneraldisplayed = false;
        private void StatsGeneral(JournalStats currentstat, DateTime endtimeutc)
        {
            TravelDGV("Total No of jumps".T(EDTx.UserControlStats_TotalNoofjumps), currentstat.fsdcarrierjumps.ToString());

            extChartTravelDest.ClearSeriesPoints();

            if (currentstat.FSDJumps.Count > 0)        // these will be null unless there are jumps
            {
                TravelDGV("FSD jumps".T(EDTx.UserControlStats_FSDjumps), currentstat.FSDJumps.Count.ToString());

                TravelDGV("FSD Jump History".T(EDTx.UserControlStats_JumpHistory),
                                        "24 Hours: ".T(EDTx.UserControlStats_24hc) + currentstat.FSDJumps.Where(x => x.utc >= endtimeutc.AddHours(-24)).Count() +
                                        ", One Week: ".T(EDTx.UserControlStats_OneWeek) + currentstat.FSDJumps.Where(x => x.utc >= endtimeutc.AddDays(-7)).Count() +
                                        ", 30 Days: ".T(EDTx.UserControlStats_30Days) + currentstat.FSDJumps.Where(x => x.utc >= endtimeutc.AddDays(-30)).Count() +
                                        ", One Year: ".T(EDTx.UserControlStats_OneYear) + currentstat.FSDJumps.Where(x => x.utc >= endtimeutc.AddDays(-365)).Count()
                                        );

                TravelDGV("Most North".T(EDTx.UserControlStats_MostNorth), GetSystemDataString(currentstat.MostNorth));
                TravelDGV("Most South".T(EDTx.UserControlStats_MostSouth), GetSystemDataString(currentstat.MostSouth));
                TravelDGV("Most East".T(EDTx.UserControlStats_MostEast), GetSystemDataString(currentstat.MostEast));
                TravelDGV("Most West".T(EDTx.UserControlStats_MostWest), GetSystemDataString(currentstat.MostWest));
                TravelDGV("Most Highest".T(EDTx.UserControlStats_MostHighest), GetSystemDataString(currentstat.MostUp));
                TravelDGV("Most Lowest".T(EDTx.UserControlStats_MostLowest), GetSystemDataString(currentstat.MostDown));

                if (extChartTravelDest.Active)
                {
                    var fsdbodies = currentstat.FSDJumps.GroupBy(x => x.bodyname).ToDictionary(x => x.Key, y => y.Count()).ToList();       // get KVP list of distinct bodies
                    fsdbodies.Sort(delegate (KeyValuePair<string, int> left, KeyValuePair<string, int> right) { return right.Value.CompareTo(left.Value); }); // and sort highest

                    // charts themed by themer has X and Y series label color set so they will show

                    int i = 0;
                    foreach (var data in fsdbodies.Take(12))      
                    {
                        extChartTravelDest.AddXY(i, data.Value, data.Key);
                        i++;
                    }
                }
            }

            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCount} General tab displayed");

            laststatsgeneraldisplayed = true;
        }

        private string GetSystemDataString(JournalLocOrJump he)
        {
            return he == null ? "N/A" : he.StarSystem + " @ " + he.StarPos.X.ToString("0.0") + "; " + he.StarPos.Y.ToString("0.0") + "; " + he.StarPos.Z.ToString("0.0");
        }

        void TravelDGV(string title, string data)
        {
            int rowpresent = dataGridViewGeneral.FindRowWithValue(0, title);
            if (rowpresent != -1)
                dataGridViewGeneral.Rows[rowpresent].Cells[1].Value = data;
            else
                dataGridViewGeneral.Rows.Add(new object[] { title, data });
        }

        #endregion

        #region Ranks

        private bool laststatsrankdisplayed = false;

        private void StatsRanks(JournalStats currentstat, DateTime endtimeutc)
        {
            JournalRank firstrank = currentstat.Ranks.FirstOrDefault();        // may be null;
            JournalRank lastrank = currentstat.Ranks.LastOrDefault();        // may be null;
            JournalPromotion pcombat = currentstat.Promotions.FindLast(x => x.Combat.HasValue);
            JournalPromotion ptrade = currentstat.Promotions.FindLast(x => x.Trade.HasValue);
            JournalPromotion pexplore = currentstat.Promotions.FindLast(x => x.Explore.HasValue);
            JournalPromotion psoldier= currentstat.Promotions.FindLast(x => x.Soldier.HasValue);
            JournalPromotion pexobiologist = currentstat.Promotions.FindLast(x => x.ExoBiologist.HasValue);
            JournalPromotion pempire = currentstat.Promotions.FindLast(x => x.Empire.HasValue);
            JournalPromotion pfederation = currentstat.Promotions.FindLast(x => x.Federation.HasValue);
            JournalPromotion pcqc = currentstat.Promotions.FindLast(x => x.CQC.HasValue);

            dataGridViewRanks.Rows.Clear();

            var ranknames = JournalRank.TranslatedRankNames();

            dataGridViewRanks.Rows.Add(new string[]
            {
                ranknames[0], firstrank?.Combat.ToString().Replace("_", " ") ?? "?",lastrank?.Combat.ToString().Replace("_", " ") ?? "?",
                currentstat.LastProgress?.Combat.ToString() ?? "-",
                pcombat!=null ? EDDConfig.Instance.ConvertTimeToSelectedFromUTC(pcombat.EventTimeUTC).ToString() : "-",
            });
            dataGridViewRanks.Rows.Add(new string[]
            {
                ranknames[1], firstrank?.Trade.ToString().Replace("_", " ") ?? "?",lastrank?.Trade.ToString().Replace("_", " ") ?? "?",
                currentstat.LastProgress?.Trade.ToString() ?? "-",
                ptrade!=null ? EDDConfig.Instance.ConvertTimeToSelectedFromUTC(ptrade.EventTimeUTC).ToString() : "-",
            });
            dataGridViewRanks.Rows.Add(new string[]
            {
                ranknames[2], firstrank?.Explore.ToString().Replace("_", " ") ?? "?",lastrank?.Explore.ToString().Replace("_", " ") ?? "?",
                currentstat.LastProgress?.Explore.ToString() ?? "-",
                pexplore!=null ? EDDConfig.Instance.ConvertTimeToSelectedFromUTC(pexplore.EventTimeUTC).ToString() : "-",
            });
            dataGridViewRanks.Rows.Add(new string[]
            {
                ranknames[3], firstrank?.Soldier.ToString().Replace("_", " ") ?? "?",lastrank?.Soldier.ToString().Replace("_", " ") ?? "?",
                currentstat.LastProgress?.Soldier.ToString() ?? "-",
                psoldier!=null ? EDDConfig.Instance.ConvertTimeToSelectedFromUTC(psoldier.EventTimeUTC).ToString() : "-",
            });
            dataGridViewRanks.Rows.Add(new string[]
            {
                ranknames[4], firstrank?.ExoBiologist.ToString().Replace("_", " ") ?? "?",lastrank?.ExoBiologist.ToString().Replace("_", " ") ?? "?",
                currentstat.LastProgress?.ExoBiologist.ToString() ?? "-",
                pexobiologist!=null ? EDDConfig.Instance.ConvertTimeToSelectedFromUTC(pexobiologist.EventTimeUTC).ToString() : "-",
            });
            dataGridViewRanks.Rows.Add(new string[]
            {
                ranknames[5], firstrank?.Empire.ToString().Replace("_", " ") ?? "?",lastrank?.Empire.ToString().Replace("_", " ") ?? "?",
                currentstat.LastProgress?.Empire.ToString() ?? "-",
                pempire!=null ? EDDConfig.Instance.ConvertTimeToSelectedFromUTC(pempire.EventTimeUTC).ToString() : "-",
            });
            dataGridViewRanks.Rows.Add(new string[]
            {
                ranknames[6], firstrank?.Federation.ToString().Replace("_", " ") ?? "?",lastrank?.Federation.ToString().Replace("_", " ") ?? "?",
                currentstat.LastProgress?.Federation.ToString() ?? "-",
                pfederation!=null ? EDDConfig.Instance.ConvertTimeToSelectedFromUTC(pfederation.EventTimeUTC).ToString() : "-",
            });
            dataGridViewRanks.Rows.Add(new string[]
            {
                ranknames[7], firstrank?.CQC.ToString().Replace("_", " ") ?? "?",lastrank?.CQC.ToString().Replace("_", " ") ?? "?",
                currentstat.LastProgress?.CQC.ToString() ?? "-",
                pcqc!=null ? EDDConfig.Instance.ConvertTimeToSelectedFromUTC(pcqc.EventTimeUTC).ToString() : "-",
            });

            string pp = currentstat.LastPowerplay != null ? $"{currentstat.LastPowerplay.Power} - {currentstat.LastPowerplay.Rank} - {currentstat.LastPowerplay.Merits}" : "-";
            string pptime = currentstat.LastPowerplay != null ? EDDConfig.Instance.ConvertTimeToSelectedFromUTC(currentstat.LastPowerplay.TimeJoinedUTC).ToString() : "";
            dataGridViewRanks.Rows.Add(new string[]
            {
                "Powerplay".TxID(EDTx.UserControlStats_Powerplay), "", pp,"",pptime,
            });

            string ss = currentstat.LastSquadronStartup != null ? $"{currentstat.LastSquadronStartup.Name} - {currentstat.LastSquadronStartup.CurrentRank.ToString()}" : "-";
            string sstime = currentstat.LastSquadronPromotion != null ? EDDConfig.Instance.ConvertTimeToSelectedFromUTC(currentstat.LastSquadronPromotion.EventTimeUTC).ToString() : "";
            dataGridViewRanks.Rows.Add(new string[]
            {
                "Squadron".TxID(EDTx.UserControlStats_Squadron), "", ss,"",sstime,
            });


            laststatsrankdisplayed = true;
        }

        #endregion

        #region Ledger *********************************************************************************************************************

        private bool laststatsledgerdisplayed = false;
        private void StatsLedger(JournalStats currentstat)
        {
            DataGridViewColumn sortcol = dataGridViewLedger.SortedColumn != null ? dataGridViewLedger.SortedColumn : dataGridViewLedger.Columns[0];
            SortOrder sortorder = dataGridViewLedger.SortOrder != SortOrder.None ? dataGridViewLedger.SortOrder : SortOrder.Descending;

            extChartLedger.ClearSeriesPoints();
            dataGridViewLedger.Rows.Clear();

            foreach( var kvp in currentstat.Credits)
            {
                DateTime seltime = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(kvp.Key);
                object[] coldata = new object[] { seltime.ToString(), kvp.Value.ToString("N0") };
                int row =dataGridViewLedger.Rows.Add(coldata);
                dataGridViewLedger.Rows[row].Tag = seltime;
                extChartLedger.AddXY(seltime, kvp.Value, graphtooltip: $"{seltime.ToString()} {kvp.Value:N0}cr");
            }

            dataGridViewLedger.Sort(sortcol, (sortorder == SortOrder.Descending) ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending);
            dataGridViewLedger.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;

            laststatsledgerdisplayed = true;
        }

        private void dataGridViewLedger_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewLedger.RowCount)
            {
                var row = dataGridViewLedger.Rows[e.RowIndex];
                var datetime = (DateTime)row.Tag;
              //  System.Diagnostics.Debug.WriteLine($"Stats Selected Graph cursor position {datetime}");
                extChartLedger.SetXCursorPosition(datetime);
            }
        }
        private void LedgerCursorPositionChanged(ExtendedControls.ExtSafeChart chart, string chartarea, AxisName axis, double pos)
        {
            if (!double.IsNaN(pos))     // this means its off graph, ignore
            {
                DateTime dtgraph = DateTime.FromOADate(pos);                    // back to date/time
                int row = dataGridViewLedger.FindRowWithDateTagWithin((r) => (DateTime)r.Tag, dtgraph, long.MaxValue);  // we accept any nearest
                if (row >= 0)
                {
                    dataGridViewLedger.SetCurrentAndSelectAllCellsOnRow(row);
                    dataGridViewLedger.Rows[row].Selected = true;
                }
            }
        }


        #endregion

        #region Travel Panel *********************************************************************************************************************

        private StatsTimeUserControl.TimeModeType lasttraveltimemode = StatsTimeUserControl.TimeModeType.NotSet;

        async void StatsTravel(JournalStats currentstat, DateTime starttimeutc, DateTime endtimeutc)
        {
            userControlStatsTimeTravel.Enabled = false;

            var timemode = userControlStatsTimeTravel.TimeMode;

            int sortcol = dataGridViewTravel.SortedColumn?.Index ?? 99;
            SortOrder sortorder = dataGridViewTravel.SortOrder;

            var tupletimes = timemode == StatsTimeUserControl.TimeModeType.Summary ?
                                    SetupSummary(starttimeutc, endtimeutc, currentstat.lastdockedutc, dataGridViewTravel, dbTravel) :
                                    SetUpDaysMonthsYear(endtimeutc, dataGridViewTravel, timemode, dbTravel);

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

            if (sortcol < dataGridViewTravel.Columns.Count)
            {
                dataGridViewTravel.Sort(dataGridViewTravel.Columns[sortcol], (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                dataGridViewTravel.Columns[sortcol].HeaderCell.SortGlyphDirection = sortorder;
            }

            userControlStatsTimeTravel.Enabled = true;

            lasttraveltimemode = timemode;

            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCount} Turn back on timer after travel");
            timerupdate.Start();                                // we did an await above, we now turn the timer back on after the update
            labelStatus.Text = "";  // not working now
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
        private void userControlStatsTimeTravel_TimeModeChanged(StatsTimeUserControl.TimeModeType previous, StatsTimeUserControl.TimeModeType next)
        {
            if (previous != next)
            {
                DGVSaveColumnLayout(dataGridViewTravel, dbTravel + previous.ToString());
                dataGridViewTravel.Columns.Clear();
                dataGridViewTravel.Rows.Clear();
                redisplay = true;
            }
        }

        #endregion

        #region SCAN  ****************************************************************************************************************

        private StatsTimeUserControl.TimeModeType lastscantimemode = StatsTimeUserControl.TimeModeType.NotSet;
        private bool lastscanstarmode = false;

        async void StatsScan(JournalStats currentstat, DateTime starttimeutc, DateTime endtimeutc )
        {
            userControlStatsTimeScan.Enabled = false;
            int sortcol = dataGridViewScan.SortedColumn?.Index ?? 0;
            SortOrder sortorder = dataGridViewScan.SortOrder;

            dataGridViewScan.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            var timemode = userControlStatsTimeScan.TimeMode;

            var tupletimes = timemode == StatsTimeUserControl.TimeModeType.Summary ?
                                    SetupSummary(starttimeutc, endtimeutc, currentstat.lastdockedutc, dataGridViewScan, dbScan) :
                             SetUpDaysMonthsYear(endtimeutc, dataGridViewScan, timemode, dbScan);

            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCountLap("STATS", true)} Scan stats interval begin {timemode}");
            var cres = await ComputeScans(currentstat, tupletimes, userControlStatsTimeScan.StarMode);
            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCountLap("STATS")} Scan stats interval end {timemode}");
            if (IsClosed)
                return;

            for( int i = 0; i < cres.chart1labels.Length; i++)
            {
                StatToDGV(dataGridViewScan, cres.chart1labels[i], cres.griddata[i]);
            }

            StatToDGV(dataGridViewScan, " ** Total Scans **", cres.griddata[cres.chart1labels.Length]);

            if (sortcol < dataGridViewScan.Columns.Count)
            {
                dataGridViewScan.Sort(dataGridViewScan.Columns[sortcol], (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                dataGridViewScan.Columns[sortcol].HeaderCell.SortGlyphDirection = sortorder;
            }


            extChartScan.Tag = cres;
            scanchartcolumn = MoveChart(0, dataGridViewScan.ColumnCount - 1, cres.chart1data, cres.chart1data, scanchartcolumn);

            FillScanChart();

            userControlStatsTimeScan.Enabled = true;
            lastscantimemode = timemode;
            lastscanstarmode = userControlStatsTimeScan.StarMode;

            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCount} Stats Turn back on timer after scan");
            timerupdate.Start();                                // we did an await above, we now turn the timer back on after the update
            labelStatus.Text = "";  // not working now
        }

        int scanchartcolumn = 0;
        private void FillScanChart()
        {
            DataReturn res = (DataReturn)extChartScan.Tag;

            int[] scandata = res.chart1data[scanchartcolumn];

            if (!extChartScan.CompareYPoints(scandata.Select(x => (double)x).ToArray()))       // if not the same values
            {
                extChartScan.ClearSeriesPoints();

                double scantotal = scandata.Sum();

                int c = 0;
                for (int i = 0; i < scandata.Length; i++)
                {
                    if (scandata[i] != 0)
                    {
                        string t = res.chart1labels[i] + ": " + scandata[i].ToString();
                        string tt = $"{t} {(scandata[i] / scantotal) * 100:N2}%";
                        extChartScan.AddPoint(scandata[i], null, t, piechartcolours[c++ % piechartcolours.Length], false, tt);  // null means no labels on actual graph, we do it via legend
                    }
                }

                extChartScan.SetChartAreaVisible(extChartScan.GetNumberPoints() > 0);
            }

            bool showing = scandata.Sum() > 0;      // see if anything is displayed. We should have selected a filled area

            extChartScan.SetTitleText(showing ? dataGridViewScan.Columns[scanchartcolumn + 1].HeaderText : "");
            extChartScan.LeftArrowEnable = extChartScan.RightArrowEnable = showing;     // if we are not showing anything, nothing is present, disable arrows
        }

        private void ScanChartArrowPressed(bool right)
        {
            DataReturn res = (DataReturn)extChartScan.Tag;
            scanchartcolumn = MoveChart(right ? 1 : -1, dataGridViewScan.ColumnCount - 1, res.chart1data, res.chart1data, scanchartcolumn);
            FillScanChart();
        }
        private static System.Threading.Tasks.Task<DataReturn> ComputeScans(JournalStats currentstat, Tuple<DateTime[], DateTime[]> tupletimes, bool starmode)
        {
            return System.Threading.Tasks.Task.Run(() =>
            {
                DataReturn crs = new DataReturn();

                int results = starmode ? Enum.GetValues(typeof(EDStar)).Length : Enum.GetValues(typeof(EDPlanet)).Length;

                crs.chart1labels = new string[results];     // fill up chart labels
                if (starmode)
                {
                    int i = 0;
                    foreach (EDStar startype in Enum.GetValues(typeof(EDStar)))
                        crs.chart1labels[i++] = Bodies.StarName(startype);
                }
                else
                {
                    int i = 0;
                    foreach (EDPlanet planettype in Enum.GetValues(typeof(EDPlanet)))
                        crs.chart1labels[i++] = planettype == EDPlanet.Unknown_Body_Type ? "Belt Cluster".T(EDTx.UserControlStats_Beltcluster) : Bodies.PlanetTypeName(planettype);
                }

                int intervals = tupletimes.Item1.Length;

                crs.chart1data = new int[intervals][];        // outer [] is intervals      CHART 1 is PVP
                for (var i = 0; i < intervals; i++)
                    crs.chart1data[i] = new int[crs.chart1labels.Length];

                var scanlists = new List<JournalScan>[intervals];
                for (int ii = 0; ii < intervals; ii++)
                    scanlists[ii] = currentstat.Scans.Values.Where(x => x.EventTimeUTC >= tupletimes.Item1[ii] && x.EventTimeUTC < tupletimes.Item2[ii]).ToList();

                results++;      // 1 more for totals at end

                crs.griddata = new string[results][];
                for (var i = 0; i < results; i++)
                    crs.griddata[i] = new string[intervals];

                long[] totals = new long[intervals];

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

                            crs.chart1data[ii][row] = num;
                            crs.griddata[row][ii] = num.ToString("N0");
                            totals[ii] += num;
                        }

                        row++;
                    }
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

                            crs.chart1data[ii][row] = planettype == EDPlanet.Unknown_Body_Type ? 0 : num;       // we knock out of the chart belt clusters
                            crs.griddata[row][ii] = num.ToString("N0");
                            totals[ii] += num;
                        }
                        row++;
                    }
                }

                for (int i = 0; i < intervals; i++)
                    crs.griddata[results-1][i] = totals[i].ToString("N0");

                return crs;
            });
        }

        private void userControlStatsTimeScan_TimeModeChanged(StatsTimeUserControl.TimeModeType previous, StatsTimeUserControl.TimeModeType next)
        {
            if (previous != next)
            {
                DGVSaveColumnLayout(dataGridViewScan, dbScan + previous.ToString());
                ClearScanParts();
            }
        }

        private void userControlStatsTimeScan_StarPlanetModeChanged()
        {
            DGVSaveColumnLayout(dataGridViewScan, dbScan + userControlStatsTimeScan.TimeMode.ToString());        // we get a complete redisplay, with DGV load, so need to save it back even though don't really need to
            ClearScanParts();
            dataGridViewScan.Columns.Clear();
            dataGridViewScan.Rows.Clear();
        }

        private void ClearScanParts()
        {
            dataGridViewScan.Columns.Clear();
            dataGridViewScan.Rows.Clear();
            extChartScan.ClearAllSeriesPoints();
            scanchartcolumn = 0;
            redisplay = true;
        }

        private void ExtComboBoxScanChart_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillScanChart();
        }

        #endregion

        #region Combat  ****************************************************************************************************************************

        private StatsTimeUserControl.TimeModeType lastcombattimemode = StatsTimeUserControl.TimeModeType.NotSet;
        async void StatsCombat(JournalStats currentstat, DateTime starttimeutc, DateTime endtimeutc)
        {
            statsTimeUserControlCombat.Enabled = false;

            int sortcol = dataGridViewCombat.SortedColumn?.Index ?? 99;
            SortOrder sortorder = dataGridViewCombat.SortOrder;

            var timemode = statsTimeUserControlCombat.TimeMode;

            var tupletimes = timemode == StatsTimeUserControl.TimeModeType.Summary ?
                                    SetupSummary(starttimeutc, endtimeutc, currentstat.lastdockedutc, dataGridViewCombat, dbCombat) :
                             SetUpDaysMonthsYear(endtimeutc, dataGridViewCombat, timemode, dbCombat);

            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCountLap("STATS", true)} Combat stats interval begin {timemode}");
            var cres = await ComputeCombat(currentstat, tupletimes);
            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCountLap("STATS")} Combat stats interval end {timemode}");
            if (IsClosed)
                return;

            int row = 0;
            StatToDGV(dataGridViewCombat, "Bounties".T(EDTx.UserControlStats_Bounties), cres.griddata[row++]);
            StatToDGV(dataGridViewCombat, "Bounty Value".T(EDTx.UserControlStats_Bountyvalue), cres.griddata[row++]);
            StatToDGV(dataGridViewCombat, "Bounties on Ships".T(EDTx.UserControlStats_Bountiesonships), cres.griddata[row++]);

            foreach (var lab in cres.chart1labels)
                StatToDGV(dataGridViewCombat, lab, cres.griddata[row++]);

            StatToDGV(dataGridViewCombat, "Crimes".T(EDTx.UserControlStats_Crimes), cres.griddata[row++]);
            StatToDGV(dataGridViewCombat, "Crime Cost".T(EDTx.UserControlStats_CrimeCost), cres.griddata[row++]);
            StatToDGV(dataGridViewCombat, "Faction Kill Bonds".T(EDTx.UserControlStats_FactionKillBonds), cres.griddata[row++]);
            StatToDGV(dataGridViewCombat, "FKB Value".T(EDTx.UserControlStats_FKBValue), cres.griddata[row++]);

            StatToDGV(dataGridViewCombat, "Interdictions Player Succeeded".T(EDTx.UserControlStats_InterdictionPlayerSucceeded), cres.griddata[row++]);
            StatToDGV(dataGridViewCombat, "Interdictions Player Failed".T(EDTx.UserControlStats_InterdictionPlayerFailed), cres.griddata[row++]);
            StatToDGV(dataGridViewCombat, "Interdictions NPC Succeeded".T(EDTx.UserControlStats_InterdictionNPCSucceeded), cres.griddata[row++]);
            StatToDGV(dataGridViewCombat, "Interdictions NPC Failed".T(EDTx.UserControlStats_InterdictionNPCFailed), cres.griddata[row++]);

            StatToDGV(dataGridViewCombat, "Interdicted Player Succeeded".T(EDTx.UserControlStats_InterdictedPlayerSucceeded), cres.griddata[row++]);
            StatToDGV(dataGridViewCombat, "Interdicted Player Failed".T(EDTx.UserControlStats_InterdictedPlayerFailed), cres.griddata[row++]);
            StatToDGV(dataGridViewCombat, "Interdicted NPC Succeeded".T(EDTx.UserControlStats_InterdictedNPCSucceeded), cres.griddata[row++]);
            StatToDGV(dataGridViewCombat, "Interdicted NPC Failed".T(EDTx.UserControlStats_InterdictedNPCFailed), cres.griddata[row++]);

            StatToDGV(dataGridViewCombat, "PVP Kills".T(EDTx.UserControlStats_PVPKills), cres.griddata[row++]);
            foreach (var lab in cres.chart2labels)
                StatToDGV(dataGridViewCombat, lab, cres.griddata[row++]);

            if (sortcol < dataGridViewCombat.Columns.Count)
            {
                dataGridViewCombat.Sort(dataGridViewCombat.Columns[sortcol], (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                dataGridViewCombat.Columns[sortcol].HeaderCell.SortGlyphDirection = sortorder;
            }

            extChartCombat.Tag = cres;                  // tag remembers data to fill pie with

            // try and find a filled column, then display.  If nothing is found, we will be on an empty position
            combatchartcolumn = MoveChart(0, dataGridViewCombat.ColumnCount - 1, cres.chart1data, cres.chart2data, combatchartcolumn);
            FillCombatChart();

            statsTimeUserControlCombat.Enabled = true;
            lastcombattimemode = timemode;

            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCount} Stats Turn back on timer after combat");
            timerupdate.Start();                                // we did an await above, we now turn the timer back on after the update
            labelStatus.Text = "";  // not working now
        }

        int combatchartcolumn = 0;
        private void FillCombatChart()
        {
            DataReturn res = (DataReturn)extChartCombat.Tag;

            extChartCombat.SetCurrentSeries(0);     // 0 is NPC PIE

            int[] npcdata = res.chart1data[combatchartcolumn];

            if (!extChartCombat.CompareYPoints(npcdata.Select(x => (double)x).ToArray()))       // if not the same values
            {
                extChartCombat.ClearSeriesPoints();
                double total = npcdata.Sum();
                int c = 0;
                for (int i = 0; i < npcdata.Length; i++)
                {
                    if (npcdata[i] != 0)
                    {
                        string t = res.chart1labels[i] + ": " + npcdata[i].ToString();
                        string tt = $"{t} {(npcdata[i] / total) * 100:N2}%";
                        extChartCombat.AddPoint(npcdata[i], null, t, piechartcolours[c++ % piechartcolours.Length], false, tt);  // null means no labels on actual graph, we do it via legend
                    }
                }
            }

            extChartCombat.SetChartAreaVisible(0,npcdata.Sum() > 0);

            extChartCombat.SetCurrentSeries(1); // PVP

            int[] pvpdata = res.chart2data[combatchartcolumn];

            if (!extChartCombat.CompareYPoints(npcdata.Select(x => (double)x).ToArray()))       // if not the same values
            {
                extChartCombat.ClearSeriesPoints();
                double total = pvpdata.Sum();
                int c = 0;
                for (int i = 0; i < pvpdata.Length; i++)
                {
                    if (pvpdata[i] != 0)
                    {
                        string t = res.chart2labels[i] + ": " + pvpdata[i].ToString();
                        string tt = $"{t} {(pvpdata[i] / total) * 100:N2}%";
                        extChartCombat.AddPoint(pvpdata[i], null, t, piechartcolours[c++ % piechartcolours.Length], false, tt);
                    }
                }
            }

            extChartCombat.SetChartAreaVisible(1,pvpdata.Sum() > 0);

            bool showing = npcdata.Sum() > 0 || pvpdata.Sum() > 0;      // see if anything is displayed. We should have selected a filled area

            extChartCombat.SetTitleText(showing ? dataGridViewCombat.Columns[combatchartcolumn + 1].HeaderText : "");
            extChartCombat.LeftArrowEnable = extChartCombat.RightArrowEnable = showing;     // if we are not showing anything, nothing is present, disable arrows
        }

        private void CombatChartArrowPressed(bool right)
        {
            DataReturn res = (DataReturn)extChartCombat.Tag;
            combatchartcolumn = MoveChart(right ? 1 : -1, dataGridViewCombat.ColumnCount - 1, res.chart1data, res.chart2data, combatchartcolumn);
            FillCombatChart();
        }

        private static System.Threading.Tasks.Task<DataReturn> ComputeCombat(JournalStats currentstat, Tuple<DateTime[], DateTime[]> tupletimes)
        {
            return System.Threading.Tasks.Task.Run(() =>
            {
                DataReturn crs = new DataReturn();

                int intervals = tupletimes.Item1.Length;

                int results = 40;                               // does not need to be accurate
                crs.griddata = new string[results][];         // outer [] is results
                for (var i = 0; i < results; i++)
                    crs.griddata[i] = new string[intervals];

                crs.chart1labels = new string[]
                {
                    "Bounties on Thargoids".T(EDTx.UserControlStats_BountiesThargoid),
                    "Bounties on On Foot NPC".T(EDTx.UserControlStats_BountiesOnFootNPC), 
                    "Bounties on Skimmers".T(EDTx.UserControlStats_BountiesSkimmers), 
                    "Ships Unknown Rank".T(EDTx.UserControlStats_ShipsUnknown),
                    "Ships Elite Rank".T(EDTx.UserControlStats_ShipsElite),
                    "Ships Deadly Rank".T(EDTx.UserControlStats_ShipsDeadly),
                    "Ships Dangerous Rank".T(EDTx.UserControlStats_ShipsDangerous),
                    "Ships Master Rank".T(EDTx.UserControlStats_ShipsMaster),
                    "Ships Expert Rank".T(EDTx.UserControlStats_ShipsExpert),
                    "Ships Competent Rank".T(EDTx.UserControlStats_ShipsCompetent),
                    "Ships Novice Rank".T(EDTx.UserControlStats_ShipsNovice),
                    "Ships Harmless Rank".T(EDTx.UserControlStats_ShipsHarmless),
                };

                crs.chart1data = new int[intervals][];        // outer [] is intervals      CHART 1 is PVP
                for (var i = 0; i < intervals; i++)
                    crs.chart1data[i] = new int[crs.chart1labels.Length];

                crs.chart2labels = new string[]
                {
                     "PVP Elite Rank".T(EDTx.UserControlStats_PVPElite),
                     "PVP Deadly Rank".T(EDTx.UserControlStats_PVPDeadly),
                     "PVP Dangerous Rank".T(EDTx.UserControlStats_PVPDangerous),
                     "PVP Master Rank".T(EDTx.UserControlStats_PVPMaster),
                     "PVP Expert Rank".T(EDTx.UserControlStats_PVPExpert),
                     "PVP Competent Rank".T(EDTx.UserControlStats_PVPCompetent),
                     "PVP Novice Rank".T(EDTx.UserControlStats_PVPNovice),
                     "PVP Harmless Rank".T(EDTx.UserControlStats_PVPHarmless),
                };

                crs.chart2data = new int[intervals][];        // outer [] is intervals
                for (var i = 0; i < intervals; i++)
                    crs.chart2data[i] = new int[crs.chart2labels.Length];

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

                    //foreach(var x in bountyStats) System.Diagnostics.Debug.WriteLine($"{ii} = {x.EventTimeUTC} {x.Target} {x.IsShip} {x.IsThargoid} {x.IsOnFootNPC} {x.VictimFaction}");

                    int row = 0;

                    crs.griddata[row++][ii] = bountyStats.Count.ToString("N0");
                    crs.griddata[row++][ii] = bountyStats.Select(x => x.TotalReward).Sum().ToString("N0");
                    crs.griddata[row++][ii] = bountyStats.Where(x => x.IsShip).Count().ToString("N0");

                    int p = 0;
                    crs.chart1data[ii][p++] = bountyStats.Where(x => x.IsThargoid).Count();
                    crs.chart1data[ii][p++] = bountyStats.Where(x => x.IsOnFootNPC).Count();
                    crs.chart1data[ii][p++] = bountyStats.Where(x => x.IsSkimmer).Count();
                    crs.chart1data[ii][p++] = bountyStats.Where(x => x.StatsUnknownShip).Count();
                    crs.chart1data[ii][p++] = bountyStats.Where(x => x.StatsEliteAboveShip).Count();
                    crs.chart1data[ii][p++] = bountyStats.Where(x => x.StatsRankShip(CombatRank.Deadly)).Count();
                    crs.chart1data[ii][p++] = bountyStats.Where(x => x.StatsRankShip(CombatRank.Dangerous)).Count();
                    crs.chart1data[ii][p++] = bountyStats.Where(x => x.StatsRankShip(CombatRank.Master)).Count();
                    crs.chart1data[ii][p++] = bountyStats.Where(x => x.StatsRankShip(CombatRank.Expert)).Count();
                    crs.chart1data[ii][p++] = bountyStats.Where(x => x.StatsRankShip(CombatRank.Competent)).Count();
                    crs.chart1data[ii][p++] = bountyStats.Where(x => x.StatsRankShip(CombatRank.Novice)).Count();
                    crs.chart1data[ii][p++] = bountyStats.Where(x => x.StatsHarmlessShip).Count();

                    for (int pp = 0; pp < p; pp++)
                    {
                        //if ( ii==0)
                        //crs.chart1data[ii][pp] = (pp + 1);
                        //crs.chart1data[ii][pp] = 0;
                        crs.griddata[row++][ii] = crs.chart1data[ii][pp].ToString("N0");
                    }

                    crs.griddata[row++][ii] = crimesStats.Count.ToString("N0");
                    crs.griddata[row++][ii] = crimesStats.Select(x => x.Cost).Sum().ToString("N0");

                    crs.griddata[row++][ii] = sfactionkillbonds.Count.ToString("N0");
                    crs.griddata[row++][ii] = sfactionkillbonds.Select(x => x.Reward).Sum().ToString("N0");

                    crs.griddata[row++][ii] = interdictions.Where(x => x.Success && x.IsPlayer).Count().ToString("N0");
                    crs.griddata[row++][ii] = interdictions.Where(x => !x.Success && x.IsPlayer).Count().ToString("N0");
                    crs.griddata[row++][ii] = interdictions.Where(x => x.Success && !x.IsPlayer).Count().ToString("N0");
                    crs.griddata[row++][ii] = interdictions.Where(x => !x.Success && !x.IsPlayer).Count().ToString("N0");

                    crs.griddata[row++][ii] = interdicted.Where(x => x.Submitted && x.IsPlayer).Count().ToString("N0");
                    crs.griddata[row++][ii] = interdicted.Where(x => !x.Submitted && x.IsPlayer).Count().ToString("N0");
                    crs.griddata[row++][ii] = interdicted.Where(x => x.Submitted && !x.IsPlayer).Count().ToString("N0");
                    crs.griddata[row++][ii] = interdicted.Where(x => !x.Submitted && !x.IsPlayer).Count().ToString("N0");

                    crs.griddata[row++][ii] = pvpStats.Count.ToString("N0");

                    p = 0;
                    crs.chart2data[ii][p++] = pvpStats.Where(x => x.CombatRank >= CombatRank.Elite).Count();
                    crs.chart2data[ii][p++] = pvpStats.Where(x => x.CombatRank == CombatRank.Deadly).Count();
                    crs.chart2data[ii][p++] = pvpStats.Where(x => x.CombatRank == CombatRank.Dangerous).Count();
                    crs.chart2data[ii][p++] = pvpStats.Where(x => x.CombatRank == CombatRank.Master).Count();
                    crs.chart2data[ii][p++] = pvpStats.Where(x => x.CombatRank == CombatRank.Expert).Count();
                    crs.chart2data[ii][p++] = pvpStats.Where(x => x.CombatRank == CombatRank.Competent).Count();
                    crs.chart2data[ii][p++] = pvpStats.Where(x => x.CombatRank == CombatRank.Novice).Count();
                    crs.chart2data[ii][p++] = pvpStats.Where(x => x.CombatRank <= CombatRank.Mostly_Harmless).Count();

                    for (int pp = 0; pp < p; pp++)
                    {
                        //if ( ii == 2)
                        //crs.chart2data[ii][pp] = (pp + 1);
                        //crs.chart2data[ii][pp] = 0;
                        crs.griddata[row++][ii] = crs.chart2data[ii][pp].ToString("N0");
                    }
                }

                return crs;
            });
        }

        private void userControlStatsTimeCombat_TimeModeChanged(StatsTimeUserControl.TimeModeType previous, StatsTimeUserControl.TimeModeType next)
        {
            if (previous != next)
            {
                DGVSaveColumnLayout(dataGridViewCombat, dbCombat + previous.ToString());
                dataGridViewCombat.Columns.Clear();
                dataGridViewCombat.Rows.Clear();
                extChartCombat.ClearAllSeriesPoints();          // clear the charts, remove items from selector box
                combatchartcolumn = 0;
                redisplay = true;
            }
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
                string jumpscolh = "Jumps".T(EDTx.UserControlStats_Jumps);
                string travelledcolh = "Travelled Ly".T(EDTx.UserControlStats_TravelledLy);
                string bodiesscannedcolh = "Bodies Scanned".T(EDTx.UserControlStats_BodiesScanned);

                dataGridViewByShip.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Alpha1", HeaderText = "Type".T(EDTx.UserControlStats_Type), ReadOnly = true });
                dataGridViewByShip.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Alpha2", HeaderText = "Name".T(EDTx.UserControlStats_Name), ReadOnly = true });
                dataGridViewByShip.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Alpha3", HeaderText = "Ident".T(EDTx.UserControlStats_Ident), ReadOnly = true });
                dataGridViewByShip.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Numeric1", HeaderText = jumpscolh, ReadOnly = true });
                dataGridViewByShip.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Numeric2", HeaderText = travelledcolh, ReadOnly = true });
                dataGridViewByShip.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Numeric3", HeaderText = bodiesscannedcolh, ReadOnly = true });
                dataGridViewByShip.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Numeric4", HeaderText = "Destroyed".T(EDTx.UserControlStats_Destroyed), ReadOnly = true });
                DGVLoadColumnLayout(dataGridViewByShip, dbShip);

                extChartShips.SetCurrentTitle(0);
                extChartShips.SetTitleText(jumpscolh);
                extChartShips.SetCurrentTitle(1);
                extChartShips.SetTitleText(travelledcolh);
                extChartShips.SetCurrentTitle(2);
                extChartShips.SetTitleText(bodiesscannedcolh);
            }

            string[] strarr = new string[6];

            dataGridViewByShip.Rows.Clear();

            double[] piejumpdist = new double[currentstat.Ships.Count];
            int[] piescans = new int[currentstat.Ships.Count];
            int[] piefsdjumps = new int[currentstat.Ships.Count];

            int row = 0;
            foreach (var kvp in currentstat.Ships)
            {
                var fsd = currentstat.FSDJumps.Where(x => x.shipid == kvp.Key);
                var scans = currentstat.Scans.Values.Where(x => x.ShipIDForStatsOnly == kvp.Key);
                strarr[0] = kvp.Value.Name?? "-";
                strarr[1] = kvp.Value.Ident ?? "-";

                piefsdjumps[row] = fsd.Count();
                strarr[2] = piefsdjumps[row].ToString();
                piejumpdist[row] = fsd.Sum(x => x.jumpdist);
                strarr[3] = piejumpdist[row].ToString("N0");
                piescans[row] = scans.Count();
                strarr[4] = piescans[row].ToString("N0");
                strarr[5] = kvp.Value.Died.ToString();
                StatToDGV(dataGridViewByShip, kvp.Key, strarr, true);
                row++;
            }

            extChartShips.ClearAllSeriesPoints();

            row = 0;
            int c = 0;
            foreach (var kvp in currentstat.Ships)
            {
                extChartShips.SetCurrentSeries(0);
                extChartShips.AddPoint(piefsdjumps[row], null, kvp.Key, piechartcolours[c % piechartcolours.Length], false, $"{kvp.Key}: {piefsdjumps[row]}");
                extChartShips.SetCurrentSeries(1);
                extChartShips.AddPoint(piejumpdist[row], null, null, piechartcolours[c % piechartcolours.Length], false, $"{kvp.Key}: {piejumpdist[row]}ly");
                extChartShips.SetCurrentSeries(2);
                extChartShips.AddPoint(piescans[row], null, null, piechartcolours[c % piechartcolours.Length], false, $"{kvp.Key}: {piescans[row]}");
                c++; row++;
            }

            if (sortcol < dataGridViewByShip.Columns.Count)
            {
                dataGridViewByShip.Sort(dataGridViewByShip.Columns[sortcol], (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                dataGridViewByShip.Columns[sortcol].HeaderCell.SortGlyphDirection = sortorder;
            }

            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCount} Stats by ship displayed");
            laststatsbyshipdisplayed = true;
        }

        #endregion


        #region Helpers

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
        private Tuple<DateTime[],DateTime[]> SetupSummary(DateTime starttimeutc, DateTime endtimeutc, DateTime lastdockedutc, DataGridView gridview, string dbname)
        {
            DateTime[] starttimesutc = new DateTime[5];
            DateTime[] endtimesutc = new DateTime[5];
            starttimesutc[0] = endtimeutc.AddDays(-1).AddSeconds(1);
            starttimesutc[1] = endtimeutc.AddDays(-7).AddSeconds(1);
            starttimesutc[2] = endtimeutc.AddMonths(-1).AddSeconds(1);
            starttimesutc[3] = lastdockedutc;
            starttimesutc[4] = starttimeutc;
            endtimesutc[0] = endtimesutc[1] = endtimesutc[2] = endtimesutc[3] = endtimesutc[4] = endtimeutc;

            if (gridview.Columns.Count == 0)
            {
                gridview.Columns.Add(new DataGridViewTextBoxColumn() { Name="AlphaCol", HeaderText = "Type".T(EDTx.UserControlStats_Type), ReadOnly = true});
               
                for (int i = 0; i < starttimesutc.Length; i++)
                    gridview.Columns.Add(new DataGridViewTextBoxColumn() { Name = "NumericCol"+i , ReadOnly = true});          // Name is important

                DGVLoadColumnLayout(gridview, dbname + "Summary");           // changed mode, therefore load layout
            }

            gridview.Columns[1].HeaderText = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(starttimesutc[0]).ToShortDateString() + "..";
            gridview.Columns[2].HeaderText = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(starttimesutc[1]).ToShortDateString() + "..";
            gridview.Columns[3].HeaderText = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(starttimesutc[2]).ToShortDateString() + "..";
            gridview.Columns[4].HeaderText = "Last dock".T(EDTx.UserControlStats_Lastdock);
            gridview.Columns[5].HeaderText = "All".T(EDTx.UserControlStats_All);

            //for (int i = 0; i < starttimeutc.Length; i++)  System.Diagnostics.Debug.WriteLine($"Time {starttimeutc[i].ToString()} - {endtimeutc[i].ToString()} {starttimeutc[i].Kind}");

            return new Tuple<DateTime[], DateTime[]>(starttimesutc, endtimesutc);
        }

        // set up a time date array with the limit times in utc over months, and set up the grid view columns
        private Tuple<DateTime[], DateTime[]> SetUpDaysMonthsYear(DateTime endtimenowutc, DataGridView gridview, StatsTimeUserControl.TimeModeType timemode, string dbname)
        {
            int intervals = timemode == StatsTimeUserControl.TimeModeType.Year ? Math.Min(12, endtimenowutc.Year - 2013) : 12;

            if (gridview.Columns.Count == 0)
            {
                var Col1 = new DataGridViewTextBoxColumn() { Name="AlphaCol", HeaderText = "Type".T(EDTx.UserControlStats_Type), ReadOnly = true };
                gridview.Columns.Add(Col1);
                for (int i = 0; i < intervals; i++)
                    gridview.Columns.Add(new DataGridViewTextBoxColumn() { Name = "NumericCol" + i, ReadOnly = true });          //Name is important for autosorting

                DGVLoadColumnLayout(gridview, dbname + timemode.ToString());           // changed mode, therefore load layout
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
                gridview.Columns[ii + 1].HeaderText = timemode == StatsTimeUserControl.TimeModeType.Month ? stime.ToString("MM/yyyy") : 
                        timemode == StatsTimeUserControl.TimeModeType.Year ? stime.ToString("yyyy") : 
                        stime.ToShortDateString();
            }

            //for (int i = 0; i < starttimeutc.Length; i++)  System.Diagnostics.Debug.WriteLine($"Time {starttimeutc[i].ToString()} - {endtimeutc[i].ToString()} {starttimeutc[i].Kind}");

            return new Tuple<DateTime[], DateTime[]>(starttimeutc, endtimeutc);
        }

        // moves curpos ignoring empty areas. dir  = +1 right, -1 left, 0 test and move right if empty
        private int MoveChart(int dir, int cols, int[][] data, int[][] data2, int current)
        {
            current = Math.Min(Math.Max(0, current), cols - 1);     // limit to 0-cols

            int curpos = current;

            if (cols > 0)
            {

                if (dir == 0)
                {
                    if (data[curpos].Sum() != 0 || data2[curpos].Sum() != 0)          // if testing, and good, stay
                        return curpos;
                    dir = 1;
                }

                do
                {
                    System.Diagnostics.Debug.WriteLine($"Trying column {curpos} in {dir}");
                    curpos += dir;
                    if (curpos < 0)
                        curpos = cols - 1;
                    else if (curpos >= cols)
                        curpos = 0;
                } while (curpos != current && (data[curpos].Sum() == 0 && data2[curpos].Sum() == 0));     // if not wrapped, and sum = 0, around

            }
            return curpos;
        }


        #endregion
    }

}
