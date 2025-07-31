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



        static Color[] piechartcolours = new Color[] { Color.Red, Color.Green, Color.Blue, Color.DarkCyan, Color.Magenta, Color.Brown, Color.Orange, Color.Yellow, Color.Fuchsia };


        #region Init

        public UserControlStats()
        {
            InitializeComponent();
            BaseUtils.TranslatorMkII.Instance.TranslateControls(this);
        }

        public override void Init()
        {
            DBBaseName = "Stats";

            tabControlCustomStats.SelectedIndex = GetSetting(dbSelectedTabSave, 0);
            userControlStatsTimeScan.DisplayStarsPlanetSelector(true);

            DiscoveryForm.OnNewEntry += AddNewEntry;
            DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange;

            // datetime picker kind is not used
            dateTimePickerStartDate.Value = GetSetting(dbStartDate, EDDConfig.Instance.ConvertTimeToSelectedFromUTC(EliteDangerousCore.EliteReleaseDates.GameRelease));
            dateTimePickerEndDate.Value = GetSetting(dbEndDate, EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow));
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
                extChartTravelDest.AddTitle("MV1", "Most Visited".Tx(), dockingpos: Docking.Top);
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
                extChartLedger.IsStartedFromZeroY = false;

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

            if ( DiscoveryForm.History.Count>0 )        // if we loaded a history, this is a new panel, so work
                KickComputer();

            timerupdate.Start();
        }

        private void Discoveryform_OnHistoryChange()
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

            DiscoveryForm.OnNewEntry -= AddNewEntry;
            DiscoveryForm.OnHistoryChange -= Discoveryform_OnHistoryChange;
        }

        #endregion

        #region UI

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
                dateTimePickerStartDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(EliteDangerousCore.EliteReleaseDates.GameRelease);
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

        private void extButtonStartStop_Click(object sender, EventArgs e)
        {
            var startstops = JournalEntry.GetStartStopDates(EDCommander.CurrentCmdrID);

            if (startstops.Count > 0)
            {
                var reformedlist = startstops.Select(x => new Tuple<DateTime, DateTime>(x.Item2, x.Item4)).ToList();
                reformedlist.Reverse();
                DateTimeRangeDialog(extButtonStartStop, reformedlist);
            }
        }
        private void extButtonUndocked_Click(object sender, EventArgs e)
        {
            var docked = JournalEntry.GetByEventType(JournalTypeEnum.Undocked, EDCommander.CurrentCmdrID, DateTime.MinValue, DateTime.MaxValue);

            if (docked.Count > 0)
            {
                var times = new List<Tuple<DateTime, DateTime>>();
                //                times.Add(new Tuple<DateTime, DateTime>(docked[docked.Count - 1].EventTimeUTC, new DateTime(2099,12,31, 0,0,0,DateTimeKind.Utc)));
                times.Add(new Tuple<DateTime, DateTime>(docked[docked.Count - 1].EventTimeUTC, DateTime.MaxValue.ToUniversalKind()));

                for (int i = docked.Count - 1; i >= docked.Count - 1000 && i >= 1; i -= 1)        // limit to 1000 last entries
                    times.Add(new Tuple<DateTime, DateTime>(docked[i - 1].EventTimeUTC, docked[i].EventTimeUTC));

                DateTimeRangeDialog(extButtonUndocked, times);
            }
        }

        private void DateTimeRangeDialog(Control button, List<Tuple<DateTime,DateTime>> times)
        {
            ExtendedControls.CheckedIconNewListBoxForm startstopsel = new ExtendedControls.CheckedIconNewListBoxForm();

            for (int i = 0; i < times.Count; i++)
            {
                string s = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(times[i].Item1).ToStringYearFirst() + " - ";
                if ( times[i].Item2 != DateTime.MaxValue)
                {
                    s += EDDConfig.Instance.ConvertTimeToSelectedFromUTC(times[i].Item2).ToStringYearFirst() + " \u0394 " + (times[i].Item2 - times[i].Item1).ToString(@"d\:hh\:mm\:ss");
                }
                startstopsel.UC.Add(i.ToStringInvariant(), s);
            }

            startstopsel.SaveSettings = (s, o) =>
            {
                int index = s.Replace(";", "").InvariantParseInt(-1);

                if (index >= 0)
                {
                    updateprogramatically = true;

                    dateTimePickerStartDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(times[index].Item1);
                    startchecked = dateTimePickerStartDate.Checked = true;

                    if (times[index].Item2 != DateTime.MaxValue)
                    {
                        dateTimePickerEndDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(times[index].Item2);
                        dateTimePickerEndDate.Checked = true;
                    }
                    else
                        dateTimePickerEndDate.Checked = false;

                    endchecked = dateTimePickerEndDate.Checked;

                    PutSetting(dbStartDate, dateTimePickerStartDate.Value);
                    PutSetting(dbStartDateOn, dateTimePickerStartDate.Checked);
                    PutSetting(dbEndDate, dateTimePickerEndDate.Value);
                    PutSetting(dbEndDateOn, dateTimePickerEndDate.Checked);

                    updateprogramatically = false;
                    KickComputer();
                }
            };

            startstopsel.CloseOnChange = true;
            startstopsel.CloseBoundaryRegion = new Size(32, extButtonStartStop.Height);
            startstopsel.Show("", button, this.FindForm());

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
                                                dateTimePickerStartDate.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(dateTimePickerStartDate.Value) : default(DateTime?),
                                                dateTimePickerEndDate.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(dateTimePickerEndDate.Value) : default(DateTime?),
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

        private void AddNewEntry(HistoryEntry he)
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
                        lasttraveltimemode = lastscantimemode = lastcombattimemode = JournalStatsInfo.TimeModeType.NotSet;
                        laststatsgeneraldisplayed = laststatsbyshipdisplayed = laststatsledgerdisplayed = laststatsrankdisplayed= false;
                    }

                    DateTime starttimeutc = dateTimePickerStartDate.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(dateTimePickerStartDate.Value) :
                                                                            EliteDangerousCore.EliteReleaseDates.GameRelease;
                    DateTime endtimeutc = dateTimePickerEndDate.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(dateTimePickerEndDate.Value) :
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
            TravelDGV("Total No of jumps".Tx(), currentstat.fsdcarrierjumps.ToString());

            extChartTravelDest.ClearSeriesPoints();

            if (currentstat.FSDJumps.Count > 0)        // these will be null unless there are jumps
            {
                TravelDGV("FSD jumps".Tx(), currentstat.FSDJumps.Count.ToString());

                TravelDGV("FSD Jump History".Tx(),
                                        "24 Hours".Tx()+": "+ currentstat.FSDJumps.Where(x => x.utc >= endtimeutc.AddHours(-24)).Count() +
                                        ", One Week".Tx()+": "+ currentstat.FSDJumps.Where(x => x.utc >= endtimeutc.AddDays(-7)).Count() +
                                        ", 30 Days".Tx()+": "+ currentstat.FSDJumps.Where(x => x.utc >= endtimeutc.AddDays(-30)).Count() +
                                        ", One Year".Tx()+": "+ currentstat.FSDJumps.Where(x => x.utc >= endtimeutc.AddDays(-365)).Count()
                                        );

                TravelDGV("Most North".Tx(), GetSystemDataString(currentstat.MostNorth));
                TravelDGV("Most South".Tx(), GetSystemDataString(currentstat.MostSouth));
                TravelDGV("Most East".Tx(), GetSystemDataString(currentstat.MostEast));
                TravelDGV("Most West".Tx(), GetSystemDataString(currentstat.MostWest));
                TravelDGV("Most Highest".Tx(), GetSystemDataString(currentstat.MostUp));
                TravelDGV("Most Lowest".Tx(), GetSystemDataString(currentstat.MostDown));

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

            // so we may have been promoted after lastrank, so horribly, fix it up
            if ( lastrank != null )
            {
                if (pcombat?.EventTimeUTC > lastrank.EventTimeUTC)
                    lastrank = new JournalRank(pcombat.EventTimeUTC, pcombat.Combat.Value, lastrank.Trade, lastrank.Explore, lastrank.Soldier, lastrank.ExoBiologist, lastrank.Empire, lastrank.Federation, lastrank.CQC);
                if (ptrade?.EventTimeUTC > lastrank.EventTimeUTC)
                    lastrank = new JournalRank(ptrade.EventTimeUTC, lastrank.Combat, ptrade.Trade.Value, lastrank.Explore, lastrank.Soldier, lastrank.ExoBiologist, lastrank.Empire, lastrank.Federation, lastrank.CQC);
                if (pexplore?.EventTimeUTC > lastrank.EventTimeUTC)
                    lastrank = new JournalRank(pexplore.EventTimeUTC, lastrank.Combat, lastrank.Trade, pexplore.Explore.Value, lastrank.Soldier, lastrank.ExoBiologist, lastrank.Empire, lastrank.Federation, lastrank.CQC);
                if (psoldier?.EventTimeUTC > lastrank.EventTimeUTC)
                    lastrank = new JournalRank(psoldier.EventTimeUTC, lastrank.Combat, lastrank.Trade, lastrank.Explore, psoldier.Soldier.Value, lastrank.ExoBiologist, lastrank.Empire, lastrank.Federation, lastrank.CQC);
                if (pexobiologist?.EventTimeUTC > lastrank.EventTimeUTC)
                    lastrank = new JournalRank(pexobiologist.EventTimeUTC, lastrank.Combat, lastrank.Trade, lastrank.Explore, lastrank.Soldier, pexobiologist.ExoBiologist.Value, lastrank.Empire, lastrank.Federation, lastrank.CQC);
                if (pempire?.EventTimeUTC > lastrank.EventTimeUTC)
                    lastrank = new JournalRank(pempire.EventTimeUTC, lastrank.Combat, lastrank.Trade, lastrank.Explore, lastrank.Soldier, lastrank.ExoBiologist, pempire.Empire.Value, lastrank.Federation, lastrank.CQC);
                if (pfederation?.EventTimeUTC > lastrank.EventTimeUTC)
                    lastrank = new JournalRank(pfederation.EventTimeUTC, lastrank.Combat, lastrank.Trade, lastrank.Explore, lastrank.Soldier, lastrank.ExoBiologist, lastrank.Empire, pfederation.Federation.Value, lastrank.CQC);
                if (pcqc?.EventTimeUTC > lastrank.EventTimeUTC)
                    lastrank = new JournalRank(pcqc.EventTimeUTC, lastrank.Combat, lastrank.Trade, lastrank.Explore, lastrank.Soldier, lastrank.ExoBiologist, lastrank.Empire, lastrank.Federation, pcqc.CQC.Value);
            }

            dataGridViewRanks.Rows.Clear();

            var ranknames = JournalRank.TranslatedRankNames();

            dataGridViewRanks.Rows.Add(new string[]
            {
                ranknames[0], RankDefinitions.FriendlyName(firstrank?.Combat),RankDefinitions.FriendlyName(lastrank?.Combat),
                currentstat.LastProgress?.Combat.ToString() ?? "-",
                pcombat!=null ? EDDConfig.Instance.ConvertTimeToSelectedFromUTC(pcombat.EventTimeUTC).ToString() : "-",
            });
            dataGridViewRanks.Rows.Add(new string[]
            {
                ranknames[1], RankDefinitions.FriendlyName(firstrank?.Trade),RankDefinitions.FriendlyName(lastrank?.Trade),
                currentstat.LastProgress?.Trade.ToString() ?? "-",
                ptrade!=null ? EDDConfig.Instance.ConvertTimeToSelectedFromUTC(ptrade.EventTimeUTC).ToString() : "-",
            });
            dataGridViewRanks.Rows.Add(new string[]
            {
                ranknames[2], RankDefinitions.FriendlyName(firstrank?.Explore),RankDefinitions.FriendlyName(lastrank?.Explore),
                currentstat.LastProgress?.Explore.ToString() ?? "-",
                pexplore!=null ? EDDConfig.Instance.ConvertTimeToSelectedFromUTC(pexplore.EventTimeUTC).ToString() : "-",
            });
            dataGridViewRanks.Rows.Add(new string[]
            {
                ranknames[3], RankDefinitions.FriendlyName(firstrank?.Soldier),RankDefinitions.FriendlyName(lastrank?.Soldier),
                currentstat.LastProgress?.Soldier.ToString() ?? "-",
                psoldier!=null ? EDDConfig.Instance.ConvertTimeToSelectedFromUTC(psoldier.EventTimeUTC).ToString() : "-",
            });
            dataGridViewRanks.Rows.Add(new string[]
            {
                ranknames[4], RankDefinitions.FriendlyName(firstrank?.ExoBiologist),RankDefinitions.FriendlyName(lastrank?.ExoBiologist),
                currentstat.LastProgress?.ExoBiologist.ToString() ?? "-",
                pexobiologist!=null ? EDDConfig.Instance.ConvertTimeToSelectedFromUTC(pexobiologist.EventTimeUTC).ToString() : "-",
            });
            dataGridViewRanks.Rows.Add(new string[]
            {
                ranknames[5], RankDefinitions.FriendlyName(firstrank?.Empire),RankDefinitions.FriendlyName(lastrank?.Empire),
                currentstat.LastProgress?.Empire.ToString() ?? "-",
                pempire!=null ? EDDConfig.Instance.ConvertTimeToSelectedFromUTC(pempire.EventTimeUTC).ToString() : "-",
            });
            dataGridViewRanks.Rows.Add(new string[]
            {
                ranknames[6], RankDefinitions.FriendlyName(firstrank?.Federation) ,RankDefinitions.FriendlyName(lastrank?.Federation),
                currentstat.LastProgress?.Federation.ToString() ?? "-",
                pfederation!=null ? EDDConfig.Instance.ConvertTimeToSelectedFromUTC(pfederation.EventTimeUTC).ToString() : "-",
            });
            dataGridViewRanks.Rows.Add(new string[]
            {
                ranknames[7], RankDefinitions.FriendlyName(firstrank?.CQC),RankDefinitions.FriendlyName(lastrank?.CQC),
                currentstat.LastProgress?.CQC.ToString() ?? "-",
                pcqc!=null ? EDDConfig.Instance.ConvertTimeToSelectedFromUTC(pcqc.EventTimeUTC).ToString() : "-",
            });

            string pp = currentstat.LastPowerplay != null ? $"{currentstat.LastPowerplay.Power} - {currentstat.LastPowerplay.Rank} - {currentstat.LastPowerplay.Merits}" : "-";
            string pptime = currentstat.LastPowerplay != null ? EDDConfig.Instance.ConvertTimeToSelectedFromUTC(currentstat.LastPowerplay.TimeJoinedUTC).ToString() : "";
            dataGridViewRanks.Rows.Add(new string[]
            {
                "Powerplay".Tx(), "", pp,"",pptime,
            });

            string ss = currentstat.LastSquadronStartup != null ? $"{currentstat.LastSquadronStartup.Name} - {currentstat.LastSquadronStartup.CurrentRank.ToString()}" : "-";
            string sstime = currentstat.LastSquadronPromotion != null ? EDDConfig.Instance.ConvertTimeToSelectedFromUTC(currentstat.LastSquadronPromotion.EventTimeUTC).ToString() : "";
            dataGridViewRanks.Rows.Add(new string[]
            {
                "Squadron".Tx(), "", ss,"",sstime,
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
            extChartLedger.BeginInit();
            dataGridViewLedger.Rows.Clear();

            foreach( var kvp in currentstat.Credits)
            {
                DateTime seltime = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(kvp.Key);
                object[] coldata = new object[] { seltime.ToString(), kvp.Value.ToString("N0") };
                int row =dataGridViewLedger.Rows.Add(coldata);
                dataGridViewLedger.Rows[row].Tag = seltime;
                extChartLedger.AddXY(seltime, kvp.Value);   // no tip, since the grid responds to a click on a point 
            }

            extChartLedger.EndInit();
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

        private JournalStatsInfo.TimeModeType lasttraveltimemode = JournalStatsInfo.TimeModeType.NotSet;

        async void StatsTravel(JournalStats currentstat, DateTime starttimeutc, DateTime endtimeutc)
        {
            userControlStatsTimeTravel.Enabled = false;

            var timemode = userControlStatsTimeTravel.TimeMode;

            int sortcol = dataGridViewTravel.SortedColumn?.Index ?? 99;
            SortOrder sortorder = dataGridViewTravel.SortOrder;

            var tupletimes = timemode == JournalStatsInfo.TimeModeType.Summary ?
                                    SetupSummary(starttimeutc, endtimeutc, currentstat.lastdockedutc, dataGridViewTravel, dbTravel) :
                                    SetUpDaysMonthsYear(endtimeutc, dataGridViewTravel, timemode, dbTravel);

            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCountLap("STATS", true)} Travel stats interval begin {timemode}");
            var res = await JournalStatsInfo.ComputeTravel(currentstat, tupletimes);
            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCountLap("STATS")} Travel stats interval end {timemode}");
            if (IsClosed)
                return;

            int row = 0;
            StatToDGV(dataGridViewTravel, "Jumps".Tx(), res[row++]);
            StatToDGV(dataGridViewTravel, "Travelled Ly".Tx(), res[row++]);
            StatToDGV(dataGridViewTravel, "Basic Boost".Tx(), res[row++]);
            StatToDGV(dataGridViewTravel, "Standard Boost".Tx(), res[row++]);
            StatToDGV(dataGridViewTravel, "Premium Boost".Tx(), res[row++]);
            StatToDGV(dataGridViewTravel, "Jet Cone Boost".Tx(), res[row++]);
            StatToDGV(dataGridViewTravel, "Scans".Tx(), res[row++]);
            StatToDGV(dataGridViewTravel, "Mapped".Tx(), res[row++]);
            StatToDGV(dataGridViewTravel, "Scan value".Tx(), res[row++]);
            StatToDGV(dataGridViewTravel, "Organic Scans Value".Tx(), res[row++]);

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

        private void userControlStatsTimeTravel_TimeModeChanged(JournalStatsInfo.TimeModeType previous, JournalStatsInfo.TimeModeType next)
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

        private JournalStatsInfo.TimeModeType lastscantimemode = JournalStatsInfo.TimeModeType.NotSet;
        private bool lastscanstarmode = false;

        async void StatsScan(JournalStats currentstat, DateTime starttimeutc, DateTime endtimeutc )
        {
            userControlStatsTimeScan.Enabled = false;
            int sortcol = dataGridViewScan.SortedColumn?.Index ?? 0;
            SortOrder sortorder = dataGridViewScan.SortOrder;

            dataGridViewScan.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            var timemode = userControlStatsTimeScan.TimeMode;

            var tupletimes = timemode == JournalStatsInfo.TimeModeType.Summary ?
                                    SetupSummary(starttimeutc, endtimeutc, currentstat.lastdockedutc, dataGridViewScan, dbScan) :
                             SetUpDaysMonthsYear(endtimeutc, dataGridViewScan, timemode, dbScan);

            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCountLap("STATS", true)} Scan stats interval begin {timemode}");
            var cres = await JournalStatsInfo.ComputeScans(currentstat, tupletimes, userControlStatsTimeScan.StarMode);
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
            var res = (JournalStatsInfo.DataReturn)extChartScan.Tag;

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
            var res = (JournalStatsInfo.DataReturn)extChartScan.Tag;
            scanchartcolumn = MoveChart(right ? 1 : -1, dataGridViewScan.ColumnCount - 1, res.chart1data, res.chart1data, scanchartcolumn);
            FillScanChart();
        }

        private void userControlStatsTimeScan_TimeModeChanged(JournalStatsInfo.TimeModeType previous, JournalStatsInfo.TimeModeType next)
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

        private JournalStatsInfo.TimeModeType lastcombattimemode = JournalStatsInfo.TimeModeType.NotSet;
        async void StatsCombat(JournalStats currentstat, DateTime starttimeutc, DateTime endtimeutc)
        {
            statsTimeUserControlCombat.Enabled = false;

            int sortcol = dataGridViewCombat.SortedColumn?.Index ?? 99;
            SortOrder sortorder = dataGridViewCombat.SortOrder;

            var timemode = statsTimeUserControlCombat.TimeMode;

            var tupletimes = timemode == JournalStatsInfo.TimeModeType.Summary ?
                                    SetupSummary(starttimeutc, endtimeutc, currentstat.lastdockedutc, dataGridViewCombat, dbCombat) :
                             SetUpDaysMonthsYear(endtimeutc, dataGridViewCombat, timemode, dbCombat);

            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCountLap("STATS", true)} Combat stats interval begin {timemode}");
            var cres = await JournalStatsInfo.ComputeCombat(currentstat, tupletimes);
            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCountLap("STATS")} Combat stats interval end {timemode}");
            if (IsClosed)
                return;

            int row = 0;
            StatToDGV(dataGridViewCombat, "Bounties".Tx(), cres.griddata[row++]);
            StatToDGV(dataGridViewCombat, "Bounty Value".Tx(), cres.griddata[row++]);
            StatToDGV(dataGridViewCombat, "Bounties on Ships".Tx(), cres.griddata[row++]);

            foreach (var lab in cres.chart1labels)
                StatToDGV(dataGridViewCombat, lab, cres.griddata[row++]);

            StatToDGV(dataGridViewCombat, "Crimes".Tx(), cres.griddata[row++]);
            StatToDGV(dataGridViewCombat, "Crime Cost".Tx(), cres.griddata[row++]);
            StatToDGV(dataGridViewCombat, "Faction Kill Bonds".Tx(), cres.griddata[row++]);
            StatToDGV(dataGridViewCombat, "FKB Value".Tx(), cres.griddata[row++]);

            StatToDGV(dataGridViewCombat, "Interdictions Player Succeeded".Tx(), cres.griddata[row++]);
            StatToDGV(dataGridViewCombat, "Interdictions Player Failed".Tx(), cres.griddata[row++]);
            StatToDGV(dataGridViewCombat, "Interdictions NPC Succeeded".Tx(), cres.griddata[row++]);
            StatToDGV(dataGridViewCombat, "Interdictions NPC Failed".Tx(), cres.griddata[row++]);

            StatToDGV(dataGridViewCombat, "Interdicted Player Succeeded".Tx(), cres.griddata[row++]);
            StatToDGV(dataGridViewCombat, "Interdicted Player Failed".Tx(), cres.griddata[row++]);
            StatToDGV(dataGridViewCombat, "Interdicted NPC Succeeded".Tx(), cres.griddata[row++]);
            StatToDGV(dataGridViewCombat, "Interdicted NPC Failed".Tx(), cres.griddata[row++]);

            StatToDGV(dataGridViewCombat, "PVP Kills".Tx(), cres.griddata[row++]);
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
            var res = (JournalStatsInfo.DataReturn)extChartCombat.Tag;

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
            var res = (JournalStatsInfo.DataReturn)extChartCombat.Tag;
            combatchartcolumn = MoveChart(right ? 1 : -1, dataGridViewCombat.ColumnCount - 1, res.chart1data, res.chart2data, combatchartcolumn);
            FillCombatChart();
        }


        private void userControlStatsTimeCombat_TimeModeChanged(JournalStatsInfo.TimeModeType previous, JournalStatsInfo.TimeModeType next)
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

            JournalStatistics stats = currentstat.laststats;

            if (stats != null) // may not have one
            {
                string collapseExpand = GameStatTreeState();

                AddTreeList("N1", "@", new string[] { EDDConfig.Instance.ConvertTimeToSelectedFromUTC(stats.EventTimeUTC).ToString() }, collapseExpand[0]);

                AddTreeList("N2", "Bank Account".Tx(), stats.BankAccount.Format("").Split(Environment.NewLine),collapseExpand[1]);
                AddTreeList("N3", "Combat".Tx(), stats.Combat.Format("").Split(Environment.NewLine), collapseExpand[2]);
                AddTreeList("N4", "Crime".Tx(), stats.Crime.Format("").Split(Environment.NewLine), collapseExpand[3]);
                AddTreeList("N5", "Smuggling".Tx(), stats.Smuggling.Format("").Split(Environment.NewLine),collapseExpand[4]);

                AddTreeList("N6", "Trading".Tx(), stats.Trading.Format("").Split(Environment.NewLine), collapseExpand[5]);
                AddTreeList("N7", "Mining".Tx(), stats.Mining.Format("").Split(Environment.NewLine),collapseExpand[6]);
                AddTreeList("N8", "Exploration".Tx(), stats.Exploration.Format("").Split(Environment.NewLine),collapseExpand[7]);
                AddTreeList("N9", "Passengers".Tx(), stats.PassengerMissions.Format("").Split(Environment.NewLine), collapseExpand[8]);

                AddTreeList("N10", "Search and Rescue".Tx(), stats.SearchAndRescue.Format("").Split(Environment.NewLine), collapseExpand[9]);
                AddTreeList("N11", "Crafting".Tx(), stats.Crafting.Format("").Split(Environment.NewLine), collapseExpand[10]);
                AddTreeList("N12", "Crew".Tx(), stats.Crew.Format("").Split(Environment.NewLine), collapseExpand[11]);
                AddTreeList("N13", "Multi-crew".Tx(), stats.Multicrew.Format("").Split(Environment.NewLine), collapseExpand[12]);

                AddTreeList("N14", "Materials Trader".Tx(), stats.MaterialTraderStats.Format("").Split(Environment.NewLine), collapseExpand[13]);
                AddTreeList("N15", "CQC".Tx(), stats.CQC.Format("").Split(Environment.NewLine), collapseExpand[14]);
                AddTreeList("N16", "Fleetcarrier".Tx(), stats.FLEETCARRIER.Format("").Split(Environment.NewLine), collapseExpand[15]);
                AddTreeList("N17", "Exobiology".Tx(), stats.Exobiology.Format("").Split(Environment.NewLine), collapseExpand[16]);
                AddTreeList("N18", "Thargoids".Tx(), stats.Thargoids.Format("",true).Split(Environment.NewLine), collapseExpand[17]);
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

            if (pnode.Nodes.Count > 0)          // defend against nodes coming and going so we don't show bad data from previous searches
            {
                if (pnode.Nodes.Count != children.Length)   // different length
                {
                    pnode.Nodes.Clear();
                }
                else
                {
                    int exno = 0;
                    foreach( TreeNode cn in pnode.Nodes)        // check IDs are in the same order
                    {
                        string childid = parentid + "-" + (exno++).ToString();       // make up a child id
                        if ( cn.Name != childid)
                        {
                            pnode.Nodes.Clear();
                            break;
                        }
                    }
                }
            }

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
            if (treeViewStats.Nodes.Count > 0)          // if nodes there, report state
            {
                foreach (TreeNode tn in treeViewStats.Nodes)
                    result += tn.IsExpanded ? "Y" : "N";
            }
            else
            {
                result = GetSetting(dbStatsTreeStateSave, "");

                if (result.Length < 64)                     // So we set the save to a stupid size so if people add new entries above and forget it won't crash
                    result += new string('N', 64 - result.Length);
            }

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
                string jumpscolh = "Jumps".Tx();
                string travelledcolh = "Travelled Ly".Tx();
                string bodiesscannedcolh = "Bodies Scanned".Tx();

                dataGridViewByShip.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Alpha1", HeaderText = "Type".Tx(), ReadOnly = true });
                dataGridViewByShip.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Alpha2", HeaderText = "Name".Tx(), ReadOnly = true });
                dataGridViewByShip.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Alpha3", HeaderText = "Ident".Tx(), ReadOnly = true });
                dataGridViewByShip.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Numeric1", HeaderText = jumpscolh, ReadOnly = true });
                dataGridViewByShip.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Numeric2", HeaderText = travelledcolh, ReadOnly = true });
                dataGridViewByShip.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Numeric3", HeaderText = bodiesscannedcolh, ReadOnly = true });
                dataGridViewByShip.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Numeric4", HeaderText = "Destroyed".Tx(), ReadOnly = true });
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
                strarr[2] = piefsdjumps[row].ToString("N0");
                piejumpdist[row] = fsd.Sum(x => x.jumpdist);
                strarr[3] = piejumpdist[row].ToString("N0");
                piescans[row] = scans.Count();
                strarr[4] = piescans[row].ToString("N0");
                strarr[5] = kvp.Value.Died.ToString("N0");
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
            var ret = JournalStatsInfo.SetupSummary(starttimeutc, endtimeutc, lastdockedutc);

            if (gridview.Columns.Count == 0)
            {
                gridview.Columns.Add(new DataGridViewTextBoxColumn() { Name="AlphaCol", HeaderText = "Type".Tx(), ReadOnly = true});
               
                for (int i = 0; i < ret.Item1.Length; i++)
                    gridview.Columns.Add(new DataGridViewTextBoxColumn() { Name = "NumericCol"+i , ReadOnly = true});          // Name is important

                DGVLoadColumnLayout(gridview, dbname + "Summary");           // changed mode, therefore load layout
            }

            gridview.Columns[1].HeaderText = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(ret.Item1[0]).ToShortDateString() + "..";
            gridview.Columns[2].HeaderText = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(ret.Item1[1]).ToShortDateString() + "..";
            gridview.Columns[3].HeaderText = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(ret.Item1[2]).ToShortDateString() + "..";
            gridview.Columns[4].HeaderText = "Last dock".Tx();
            gridview.Columns[5].HeaderText = "All".Tx();

            //for (int i = 0; i < starttimeutc.Length; i++)  System.Diagnostics.Debug.WriteLine($"Time {starttimeutc[i].ToString()} - {endtimeutc[i].ToString()} {starttimeutc[i].Kind}");

            return ret;
        }

        // set up a time date array with the limit times in utc over months, and set up the grid view columns
        private Tuple<DateTime[], DateTime[]> SetUpDaysMonthsYear(DateTime endtimenowutc, DataGridView gridview, JournalStatsInfo.TimeModeType timemode, string dbname)
        {
            var ret = JournalStatsInfo.SetUpDaysMonthsYear(endtimenowutc, timemode);

            int intervals = ret.Item1.Length;

            if (gridview.Columns.Count == 0)
            {
                var Col1 = new DataGridViewTextBoxColumn() { Name="AlphaCol", HeaderText = "Type".Tx(), ReadOnly = true };
                gridview.Columns.Add(Col1);
                for (int i = 0; i < intervals; i++)
                    gridview.Columns.Add(new DataGridViewTextBoxColumn() { Name = "NumericCol" + i, ReadOnly = true });          //Name is important for autosorting

                DGVLoadColumnLayout(gridview, dbname + timemode.ToString());           // changed mode, therefore load layout
            }

            for (int ii = 0; ii < intervals; ii++)
            {
                var stime = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(ret.Item1[ii]);

                gridview.Columns[ii + 1].HeaderText = timemode == JournalStatsInfo.TimeModeType.Month ? stime.ToString("MM/yyyy") :
                        timemode == JournalStatsInfo.TimeModeType.Year ? stime.ToString("yyyy") :
                        stime.ToShortDateString();
            }

            //for (int i = 0; i < starttimeutc.Length; i++)  System.Diagnostics.Debug.WriteLine($"Time {starttimeutc[i].ToString()} - {endtimeutc[i].ToString()} {starttimeutc[i].Kind}");

            return ret;
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
