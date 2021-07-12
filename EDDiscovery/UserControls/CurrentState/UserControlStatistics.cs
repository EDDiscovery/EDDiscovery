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
using System.Threading;
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
        private string dbShip = "Ship";

        private Thread StatsThread;
        private bool Exit = false;
        private bool Running = false;

        private bool endchecked, startchecked;

        private Stats currentstats;
        private Chart mostVisitedChart { get; set; }
        Queue<JournalEntry> entriesqueued = new Queue<JournalEntry>();     // we queue into here new entries, and dequeue in update.

        #region Init

        public UserControlStats()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "Stats";

            tabControlCustomStats.SelectedIndex = GetSetting(dbSelectedTabSave, 0);
            userControlStatsTimeScan.EnableDisplayStarsPlanetSelector();
            BaseUtils.Translator.Instance.Translate(this);

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

            discoveryform.OnRefreshCommanders += Discoveryform_OnRefreshCommanders;
            discoveryform.OnNewEntry += AddNewEntry;

            dateTimePickerStartDate.Value = GetSetting(dbStartDate, new DateTime(2014, 12, 14));
            startchecked = dateTimePickerStartDate.Checked = GetSetting(dbStartDateOn, false);
            dateTimePickerEndDate.Value = GetSetting(dbEndDate, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
            endchecked = dateTimePickerEndDate.Checked = GetSetting(dbEndDateOn, false);

            dateTimePickerStartDate.ValueChanged += DateTimePicker_ValueChangedStart;
            dateTimePickerEndDate.ValueChanged += DateTimePicker_ValueChangedEnd;

            labelStatus.Text = "";
        }

        public override void InitialDisplay()
        {
            if ( discoveryform.history.Count>0 )        // if we loaded a history, this is a new panel, so work
                StartThread();
        }

        public override void Closing()
        {
            StopThread();
            PutSetting(dbStatsTreeStateSave, GameStatTreeState());

            if (dataGridViewScan.Columns.Count > 0)     // anything to save..
                DGVSaveColumnLayout(dataGridViewScan, dataGridViewScan.Columns.Count <= 8 ? dbScanSummary : dbScanDWM);
            if (dataGridViewTravel.Columns.Count > 0)
                DGVSaveColumnLayout(dataGridViewTravel, dataGridViewTravel.Columns.Count <= 8 ? dbTravelSummary : dbTravelDWM);
            if ( dataGridViewByShip.Columns.Count>0 )
                DGVSaveColumnLayout(dataGridViewByShip, dbShip);

            discoveryform.OnRefreshCommanders -= Discoveryform_OnRefreshCommanders;
            discoveryform.OnNewEntry -= AddNewEntry;
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

        private void tabControlCustomStats_SelectedIndexChanged(object sender, EventArgs e)     // tab change.
        {
            PutSetting(dbSelectedTabSave, tabControlCustomStats.SelectedIndex);
            Display();
        }

        private void Discoveryform_OnRefreshCommanders()
        {
            StartThread();
        }

        private void DateTimePicker_ValueChangedStart(object sender, EventArgs e)
        {
            if (startchecked != dateTimePickerStartDate.Checked || dateTimePickerStartDate.Checked)     // if check changed, or date changed and its checked
                StartThread();
            startchecked = dateTimePickerStartDate.Checked;
            PutSetting(dbStartDate, dateTimePickerStartDate.Value);
            PutSetting(dbStartDateOn, dateTimePickerStartDate.Checked);
        }

        private void DateTimePicker_ValueChangedEnd(object sender, EventArgs e)
        {
            if (endchecked != dateTimePickerEndDate.Checked || dateTimePickerEndDate.Checked)
                StartThread();
            endchecked = dateTimePickerEndDate.Checked;
            PutSetting(dbEndDate, dateTimePickerEndDate.Value);
            PutSetting(dbEndDateOn, dateTimePickerEndDate.Checked);
        }

        #endregion

        #region Stats Computation

        private class Stats
        {
            public struct JumpInfo
            {
                public string bodyname;
                public DateTime utc;
                public int boostvalue;
                public double jumpdist;
                public string shipid;
            };

            public class ScanInfo
            {
                public DateTime utc;
                public ScanEstimatedValues ev;
                public EDStar? starid;
                public EDPlanet planetid;
                public string bodyname;
                public bool? wasdiscovered;
                public bool? wasmapped;
                public bool mapped;
                public bool efficientlymapped;
                public string shipid;
            };

            public class ScanCompleteInfo
            {
                public DateTime utc;
                public string bodyname;
                public bool efficientlymapped;
            }

            public class JetConeBoostInfo
            {
                public DateTime utc;
            }

            public class ScansAreForSameBody : EqualityComparer<ScanInfo>
            {
                public override bool Equals(ScanInfo x, ScanInfo y)
                {
                    return x.bodyname == y.bodyname;
                }

                public override int GetHashCode(ScanInfo obj)
                {
                    return obj.bodyname.GetHashCode();
                }
            }

            public class ShipInfo
            {
                public string name;
                public string ident;
                public int died;
            }

            public List<JumpInfo> FSDJumps;
            public List<ScanInfo> Scans;
            public List<ScanCompleteInfo> ScanComplete;
            public List<JetConeBoostInfo> JetConeBoost;
            public Dictionary<string, ShipInfo> Ships;
            public JournalLocOrJump MostNorth;
            public JournalLocOrJump MostSouth;
            public JournalLocOrJump MostEast;
            public JournalLocOrJump MostWest;
            public JournalLocOrJump MostUp;
            public JournalLocOrJump MostDown;

            public DateTime lastdocked;

            public string currentshipid;

            public JournalStatistics laststats;

            public int fsdcarrierjumps;

            public Stats()
            {
                FSDJumps = new List<JumpInfo>();
                Scans = new List<ScanInfo>();
                ScanComplete = new List<ScanCompleteInfo>();
                JetConeBoost = new List<JetConeBoostInfo>();
                Ships = new Dictionary<string, ShipInfo>();
                lastdocked = DateTime.UtcNow;
            }
        };

        private void StartThread()
        {
            StopThread();           // stop current computation..

            Running = true;
            Exit = false;
            entriesqueued.Clear();      // clear the queue, any new entries will just be stored into entriesqueued and not displayed until the end

            //System.Diagnostics.Debug.WriteLine("Kick off stats thread " + DBName(""));

            StatsThread = new System.Threading.Thread(new System.Threading.ThreadStart(StatisticsThread));
            StatsThread.Name = "Stats";
            StatsThread.IsBackground = true;
            StatsThread.Start();

            labelStatus.Text = "Working..";
        }

        private void StopThread()
        {
            if (StatsThread != null && StatsThread.IsAlive)
            {
                Exit = true;
                StatsThread.Join();
            }

            StatsThread = null;
            Exit = false;
            labelStatus.Text = "";
        }

        private void StatisticsThread()
        {
            var stats = new Stats();

            int cmdrid = EDCommander.CurrentCmdrID;
            if (cmdrid >= 0)
            {
                JournalTypeEnum[] events = new JournalTypeEnum[]     // 
                {
                    JournalTypeEnum.FSDJump, JournalTypeEnum.CarrierJump, JournalTypeEnum.Location, JournalTypeEnum.Docked, JournalTypeEnum.JetConeBoost,
                    JournalTypeEnum.Scan, JournalTypeEnum.SAAScanComplete, JournalTypeEnum.Docked,
                    JournalTypeEnum.ShipyardNew, JournalTypeEnum.ShipyardSwap, JournalTypeEnum.LoadGame,
                    JournalTypeEnum.Statistics, JournalTypeEnum.SetUserShipName, JournalTypeEnum.Loadout, JournalTypeEnum.Died,

                };

                DateTime? start = dateTimePickerStartDate.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromSelected(dateTimePickerStartDate.Value) : default(DateTime?);
                DateTime? end = dateTimePickerEndDate.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromSelected(dateTimePickerEndDate.Value.EndOfDay()) : default(DateTime?);

                // read journal for cmdr with these events and pass thru NewJE.
                JournalEntry.GetAll(cmdrid, ids: events, callback: NewJE, callbackobj: stats, startdateutc: start, enddateutc: end, chunksize: 100);
            }

            if (Exit == false)     // if not forced stop, call end compuation.. else just exit without completion
                BeginInvoke((MethodInvoker)(() => { EndComputation(stats); }));
        }

        private bool NewJE(JournalEntry ev, Object obj)
        {
            Stats st = obj as Stats;
          //  System.Diagnostics.Debug.WriteLine("{0} Stats JE {1} {2}", DBName("") , ev.EventTimeUTC, ev.EventTypeStr);

            switch (ev.EventTypeID)
            {
                case JournalTypeEnum.FSDJump:
                case JournalTypeEnum.Location:
                case JournalTypeEnum.CarrierJump:
                    {
                        JournalLocOrJump jl = ev as JournalLocOrJump;

                        //System.Diagnostics.Debug.WriteLine("NS System {0} {1}", jl.EventTimeUTC, jl.StarSystem);

                        if (jl.HasCoordinate)
                        {
                            if (st.MostNorth == null || st.MostNorth.StarPos.Z < jl.StarPos.Z)
                                st.MostNorth = jl;
                            if (st.MostSouth == null || st.MostSouth.StarPos.Z > jl.StarPos.Z)
                                st.MostSouth = jl;
                            if (st.MostEast == null || st.MostEast.StarPos.X < jl.StarPos.X)
                                st.MostEast = jl;
                            if (st.MostWest == null || st.MostWest.StarPos.X > jl.StarPos.X)
                                st.MostWest = jl;
                            if (st.MostUp == null || st.MostUp.StarPos.Y < jl.StarPos.Y)
                                st.MostUp = jl;
                            if (st.MostDown == null || st.MostDown.StarPos.Y > jl.StarPos.Y)
                                st.MostDown = jl;
                        }

                        JournalFSDJump fsd = ev as JournalFSDJump;
                        if (fsd != null)
                        {
                            st.fsdcarrierjumps++;
                            st.FSDJumps.Add(new Stats.JumpInfo() { utc = fsd.EventTimeUTC, jumpdist = fsd.JumpDist, boostvalue = fsd.BoostValue, shipid = st.currentshipid, bodyname = fsd.StarSystem });
                        }
                        else if ( ev.EventTypeID == JournalTypeEnum.CarrierJump)
                        {
                            st.fsdcarrierjumps++;
                        }

                        break;
                    }

                case JournalTypeEnum.Scan:
                    {
                        JournalScan sc = ev as JournalScan;
                        Stats.ScanCompleteInfo sci = st.ScanComplete.Find(x => x.bodyname == sc.BodyName);      // see if we have a Scancomplete already
                        bool mapped = sci != null;
                        bool effcientlymapped = sci?.efficientlymapped ?? false;
                        st.Scans.Add(new Stats.ScanInfo() { utc = sc.EventTimeUTC, ev = sc.GetEstimatedValues(), bodyname = sc.BodyName,
                            starid = sc.IsStar ? sc.StarTypeID : default(EDStar?),
                            planetid = sc.PlanetTypeID,
                            wasdiscovered = sc.WasDiscovered, wasmapped = sc.WasMapped,
                            mapped = mapped, efficientlymapped = effcientlymapped,
                            shipid = st.currentshipid });
                        break;
                    }

                case JournalTypeEnum.SAAScanComplete:
                    {
                        JournalSAAScanComplete sc = ev as JournalSAAScanComplete;
                        bool em = sc.ProbesUsed <= sc.EfficiencyTarget;
                        st.ScanComplete.Add(new Stats.ScanCompleteInfo() { utc = sc.EventTimeUTC, bodyname = sc.BodyName, efficientlymapped = em });
                        Stats.ScanInfo sci = st.Scans.Find(x => x.bodyname == sc.BodyName);
                        if (sci != null)
                        {
                            sci.mapped = true;
                            sci.efficientlymapped = em;
                        }
                        break;
                    }

                case JournalTypeEnum.JetConeBoost:
                    {
                        st.JetConeBoost.Add(new Stats.JetConeBoostInfo() { utc = ev.EventTimeUTC });
                        break;
                    }

                case JournalTypeEnum.Docked:
                    {
                        st.lastdocked = ev.EventTimeUTC;
                        break;
                    }

                case JournalTypeEnum.ShipyardSwap:
                    {
                        var j = ev as JournalShipyardSwap;
                        st.currentshipid = j.ShipType + ":" + j.ShipId.ToStringInvariant();
                        break;
                    }
                case JournalTypeEnum.ShipyardNew:
                    {
                        var j = ev as JournalShipyardNew;
                        st.currentshipid = j.ShipType + ":" + j.ShipId.ToStringInvariant();
                        break;
                    }
                case JournalTypeEnum.LoadGame:
                    {
                        var j = ev as JournalLoadGame;
                        st.currentshipid = j.Ship + ":" + j.ShipId.ToStringInvariant();
                        // System.Diagnostics.Debug.WriteLine("Stats Loadgame ship details {0} {1} {2} {3}", j.EventTimeUTC, j.ShipFD, j.ShipName, j.ShipIdent);

                        if (!st.Ships.TryGetValue(st.currentshipid, out var cls))
                            cls = new Stats.ShipInfo();
                        cls.ident = j.ShipIdent;
                        cls.name = j.ShipName;
                        System.Diagnostics.Debug.Assert(st.currentshipid != null);
                        st.Ships[st.currentshipid] = cls;
                        break;
                    }
                case JournalTypeEnum.Loadout:
                    {
                        var j = ev as JournalLoadout;
                        st.currentshipid = j.Ship + ":" + j.ShipId.ToStringInvariant();
                        //System.Diagnostics.Debug.WriteLine("Stats loadout ship details {0} {1} {2} {3} now {4}", j.EventTimeUTC, j.ShipFD, j.ShipName, j.ShipIdent, st.currentshipid);
                        if (!st.Ships.TryGetValue(st.currentshipid, out var cls))
                            cls = new Stats.ShipInfo();
                        cls.ident = j.ShipIdent;
                        cls.name = j.ShipName;
                        System.Diagnostics.Debug.Assert(st.currentshipid != null);
                        st.Ships[st.currentshipid] = cls;

                        break;
                    }
                case JournalTypeEnum.SetUserShipName:
                    {
                        var j = ev as JournalSetUserShipName;
                        st.currentshipid = j.Ship + ":" + j.ShipID.ToStringInvariant();
                        if (!st.Ships.TryGetValue(st.currentshipid, out var cls))
                            cls = new Stats.ShipInfo();
                        cls.ident = j.ShipIdent;
                        cls.name = j.ShipName;
                        System.Diagnostics.Debug.Assert(st.currentshipid != null);
                        st.Ships[st.currentshipid] = cls;
                        break;
                    }
                case JournalTypeEnum.Died:
                    {
                        if (st.currentshipid.HasChars())
                        {
                            var j = ev as JournalDied;
                            if (!st.Ships.TryGetValue(st.currentshipid, out var cls))
                                cls = new Stats.ShipInfo();
                            cls.died++;
                            //System.Diagnostics.Debug.WriteLine("Died {0} {1}", st.currentshipid, cls.died);
                            System.Diagnostics.Debug.Assert(st.currentshipid != null);
                            st.Ships[st.currentshipid] = cls;
                        }
                        break;
                    }
                case JournalTypeEnum.Statistics:
                    {
                        st.laststats = ev as JournalStatistics;
                        break;
                    }
                
            }

            return Exit == false;
        }

        private void EndComputation(Stats s)            // foreground thread, end of thread computation
        {
            System.Diagnostics.Debug.Assert(Application.MessageLoop);
            currentstats = s;
            Running = false;        // now go into normal non running mode where add new entry updates it
            labelStatus.Text = "";
            Display();
        }

        private void AddNewEntry(HistoryEntry he, HistoryList hl)
        {
            if (!dateTimePickerEndDate.Checked || he.journalEntry.EventTimeLocal <= dateTimePickerEndDate.Value.EndOfDay())            // ignore if past the end of of the current sel range
            {
                entriesqueued.Enqueue(he.journalEntry);
                if (!Running)                           // Running is true between foreground thread OnRefreshCommanders and foreground EndComputation, so no race condition
                    Display();
            }
        }

        #endregion

        #region Update

        private void Display()
        {
            if (currentstats == null)       // double check
                return;

            while (entriesqueued.Count > 0)
            {
                NewJE(entriesqueued.Dequeue(), currentstats);            // process any queued entries
            }

            DateTime endtimelocal = dateTimePickerEndDate.Checked ? dateTimePickerEndDate.Value.EndOfDay() : DateTime.Now.EndOfDay();

            if (tabControlCustomStats.SelectedIndex == 0)
                StatsGeneral(endtimelocal);
            else if (tabControlCustomStats.SelectedIndex == 1)
                StatsTravel(endtimelocal);
            else if (tabControlCustomStats.SelectedIndex == 2)
                StatsScan(endtimelocal);
            else if (tabControlCustomStats.SelectedIndex == 3)
                StatsGame();
            else if (tabControlCustomStats.SelectedIndex == 4)
                StatsByShip();
        }

        #endregion

        #region Stats General **************************************************************************************************************

        private void StatsGeneral(DateTime endtimelocal)
        {
            DGVGeneral("Total No of jumps".T(EDTx.UserControlStats_TotalNoofjumps), currentstats.fsdcarrierjumps.ToString());

            if (currentstats.FSDJumps.Count > 0)        // these will be null unless there are jumps
            {
                DGVGeneral("FSD jumps".T(EDTx.UserControlStats_FSDjumps), currentstats.FSDJumps.Count.ToString());

                DateTime endtimeutc = endtimelocal.ToUniversalTime();

                DGVGeneral("FSD Jump History".T(EDTx.UserControlStats_JumpHistory),
                                        "24 Hours: ".T(EDTx.UserControlStats_24hc) + currentstats.FSDJumps.Where(x => x.utc >= endtimeutc.AddHours(-24)).Count() +
                                        ", One Week: ".T(EDTx.UserControlStats_OneWeek) + currentstats.FSDJumps.Where(x => x.utc >= endtimeutc.AddDays(-7)).Count() +
                                        ", 30 Days: ".T(EDTx.UserControlStats_30Days) + currentstats.FSDJumps.Where(x => x.utc >= endtimeutc.AddDays(-30)).Count() +
                                        ", One Year: ".T(EDTx.UserControlStats_OneYear) + currentstats.FSDJumps.Where(x => x.utc >= endtimeutc.AddDays(-365)).Count()
                                        );

                DGVGeneral("Most North".T(EDTx.UserControlStats_MostNorth), GetSystemDataString(currentstats.MostNorth));
                DGVGeneral("Most South".T(EDTx.UserControlStats_MostSouth), GetSystemDataString(currentstats.MostSouth));
                DGVGeneral("Most East".T(EDTx.UserControlStats_MostEast), GetSystemDataString(currentstats.MostEast));
                DGVGeneral("Most West".T(EDTx.UserControlStats_MostWest), GetSystemDataString(currentstats.MostWest));
                DGVGeneral("Most Highest".T(EDTx.UserControlStats_MostHighest), GetSystemDataString(currentstats.MostUp));
                DGVGeneral("Most Lowest".T(EDTx.UserControlStats_MostLowest), GetSystemDataString(currentstats.MostDown));

                if (mostVisitedChart != null)        // chart exists
                {
                    mostVisitedChart.Visible = true;

                    Color GridC = discoveryform.theme.GridBorderLines;
                    Color TextC = discoveryform.theme.GridCellText;
                    mostVisitedChart.Titles.Clear();
                    mostVisitedChart.Titles.Add(new Title("Most Visited".T(EDTx.UserControlStats_MostVisited), Docking.Top, discoveryform.theme.GetFont, TextC));
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

                    var fsdbodies = currentstats.FSDJumps.GroupBy(x => x.bodyname).ToDictionary(x => x.Key, y => y.Count()).ToList();       // get KVP list of distinct bodies
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

        static JournalTypeEnum[] journalsForStatsTravel = new JournalTypeEnum[]
        {
            JournalTypeEnum.FSDJump,
            JournalTypeEnum.Docked,
            JournalTypeEnum.Undocked,
            JournalTypeEnum.JetConeBoost,
            JournalTypeEnum.Scan,
            JournalTypeEnum.SAAScanComplete,
        };


        void StatsTravel(DateTime endtimelocal)
        {
            int sortcol = dataGridViewTravel.SortedColumn?.Index ?? 99;
            SortOrder sortorder = dataGridViewTravel.SortOrder;

            if (userControlStatsTimeTravel.TimeMode == StatsTimeUserControl.TimeModeType.Summary )
            {
                int intervals = 6;
                
                var isTravelling = discoveryform.history.IsTravellingUTC(out var tripStartutc);

                // if travelling, and we have a end date set, make sure the trip is before end
                if (isTravelling && dateTimePickerEndDate.Checked && tripStartutc > endtimelocal.ToUniversalTime())            
                    isTravelling = false;

                DateTime[] starttimeutc = SetupSummary(endtimelocal, isTravelling ? tripStartutc.ToLocalTime() : DateTime.Now, dataGridViewTravel, dbTravelSummary);

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
                    var fsdStats = currentstats.FSDJumps.Where(x => x.utc >= starttimeutc[ii]).ToList();
                    var scanStats = currentstats.Scans.Where(x => x.utc >= starttimeutc[ii]).Distinct(new Stats.ScansAreForSameBody()).ToList();
                    var saascancomplete = currentstats.ScanComplete.Where(x => x.utc >= starttimeutc[ii]).ToList();
                    var jetconeboosts = currentstats.JetConeBoost.Where(x => x.utc >= starttimeutc[ii]).ToList();

                    jumps[ii] = fsdStats.Count.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    distances[ii] = fsdStats.Sum(j => j.jumpdist).ToString("N2", System.Globalization.CultureInfo.CurrentCulture);
                    basicBoosts[ii] = fsdStats.Where(j => j.boostvalue == 1).Count().ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    standardBoosts[ii] = fsdStats.Where(j => j.boostvalue == 2).Count().ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    premiumBoosts[ii] = fsdStats.Where(j => j.boostvalue == 3).Count().ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    jetBoosts[ii] = jetconeboosts.Count().ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    scanned[ii] = scanStats.Count.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    mapped[ii] = saascancomplete.Count().ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    ucValue[ii] = scanStats.Sum(x => (long)x.ev.EstimatedValue(x.wasdiscovered, x.wasmapped, x.mapped, x.efficientlymapped)).ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
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
                int intervals = 12;

                DateTime[] timeintervalsutc = SetUpDaysMonths(endtimelocal, dataGridViewTravel, userControlStatsTimeTravel.TimeMode, intervals, dbTravelDWM);

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
                    var fsdStats = currentstats.FSDJumps.Where(x => x.utc >= timeintervalsutc[ii + 1] && x.utc < timeintervalsutc[ii]).ToList();
                    var scanStats = currentstats.Scans.Where(x => x.utc >= timeintervalsutc[ii + 1] && x.utc < timeintervalsutc[ii]).Distinct(new Stats.ScansAreForSameBody()).ToList();
                    var saascancomplete = currentstats.ScanComplete.Where(x => x.utc >= timeintervalsutc[ii + 1] && x.utc < timeintervalsutc[ii]).ToList();
                    var jetconeboosts = currentstats.JetConeBoost.Where(x => x.utc >= timeintervalsutc[ii + 1] && x.utc < timeintervalsutc[ii]).ToList();

                    jumps[ii] = fsdStats.Count.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    distances[ii] = fsdStats.Sum(j => j.jumpdist).ToString("N2", System.Globalization.CultureInfo.CurrentCulture);
                    basicBoosts[ii] = fsdStats.Where(j => j.boostvalue == 1).Count().ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    standardBoosts[ii] = fsdStats.Where(j => j.boostvalue == 2).Count().ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    premiumBoosts[ii] = fsdStats.Where(j => j.boostvalue == 3).Count().ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    scanned[ii] = scanStats.Count.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    ucValue[ii] = scanStats.Sum(x=>(long)x.ev.EstimatedValue(x.wasdiscovered,x.wasmapped,x.mapped,x.efficientlymapped)).ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    mapped[ii] = saascancomplete.Count().ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
                    jetBoosts[ii] = jetconeboosts.Count().ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
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
            DGVSaveColumnLayout(dataGridViewTravel, dataGridViewTravel.Columns.Count <= 8 ? dbTravelSummary : dbTravelDWM);
            dataGridViewTravel.Columns.Clear();
            Display();
        }

        private DateTime[] SetupSummary(DateTime endtimelocal, DateTime triptime, DataGridView view, string dbname)
        {
            DateTime[] starttimelocal = new DateTime[6];
            starttimelocal[0] = endtimelocal.AddDays(-1).AddSeconds(1);
            starttimelocal[1] = endtimelocal.AddDays(-7).AddSeconds(1);
            starttimelocal[2] = endtimelocal.AddMonths(-1).AddSeconds(1);
            starttimelocal[3] = currentstats.lastdocked.ToLocalTime();
            starttimelocal[4] = triptime; //;
            starttimelocal[5] = new DateTime(2014, 12, 14);

            if (view.Columns.Count == 0)
            {
                view.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Type".T(EDTx.UserControlStats_Type), Tag = "AlphaSort" });
                for (int i = 0; i < starttimelocal.Length; i++)
                    view.Columns.Add(new DataGridViewTextBoxColumn());          // day

                DGVLoadColumnLayout(view, dbname);
            }

            view.Columns[1].HeaderText = starttimelocal[0].ToShortDateString();
            view.Columns[2].HeaderText = starttimelocal[1].ToShortDateString() + "-";
            view.Columns[3].HeaderText = starttimelocal[2].ToShortDateString() + "-";
            view.Columns[4].HeaderText = "Last dock".T(EDTx.UserControlStats_Lastdock);
            view.Columns[5].HeaderText = "Trip".T(EDTx.UserControlStats_Trip);
            view.Columns[6].HeaderText = "All".T(EDTx.UserControlStats_All);

            return (from x in starttimelocal select x.ToUniversalTime()).ToArray();  // need it in UTC for the functions
        }

        private DateTime[] SetUpDaysMonths(DateTime endtimelocal, DataGridView view, StatsTimeUserControl.TimeModeType timemode, int intervals, string dbname)
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

            DateTime[] timeintervalslocal = new DateTime[intervals + 1];

            if (timemode == StatsTimeUserControl.TimeModeType.Day)
            {
                timeintervalslocal[0] = endtimelocal.AddSeconds(1);               // 1st min next day
                DateTime startofday = endtimelocal.StartOfDay();
                for (int ii = 0; ii < intervals; ii++)
                {
                    timeintervalslocal[ii + 1] = startofday;
                    startofday = startofday.AddDays(-1);
                    view.Columns[ii + 1].HeaderText = timeintervalslocal[ii + 1].ToShortDateString();
                }

            }
            else if (timemode == StatsTimeUserControl.TimeModeType.Week)
            {
                DateTime startOfWeek = endtimelocal.AddDays(-1 * (int)(DateTime.Today.DayOfWeek - 1)).StartOfDay();
                timeintervalslocal[0] = startOfWeek.AddDays(7);
                for (int ii = 0; ii < intervals; ii++)
                {
                    timeintervalslocal[ii + 1] = timeintervalslocal[ii].AddDays(-7);
                    view.Columns[ii + 1].HeaderText = timeintervalslocal[ii + 1].ToShortDateString();
                }
            }
            else  // month
            {
                DateTime startOfMonth = new DateTime(endtimelocal.Year, endtimelocal.Month, 1);
                timeintervalslocal[0] = startOfMonth.AddMonths(1);
                for (int ii = 0; ii < intervals; ii++)
                {
                    timeintervalslocal[ii + 1] = timeintervalslocal[ii].AddMonths(-1);
                    view.Columns[ii + 1].HeaderText = timeintervalslocal[ii + 1].ToString("MM/yy");
                }
            }

            return (from x in timeintervalslocal select x.ToUniversalTime()).ToArray();  // need it in UTC for the functions
        }


        #endregion

        #region SCAN  ****************************************************************************************************************

        void StatsScan(DateTime endtimelocal )
        {
            int sortcol = dataGridViewScan.SortedColumn?.Index ?? 0;
            SortOrder sortorder = dataGridViewScan.SortOrder;

            dataGridViewScan.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            int intervals = 0;
            List<Stats.ScanInfo>[] scanlists = null;

            if (userControlStatsTimeScan.TimeMode == StatsTimeUserControl.TimeModeType.Summary )
            {
                intervals = 6;

                var isTravelling = discoveryform.history.IsTravellingUTC(out var tripStartutc);
                if (isTravelling && dateTimePickerEndDate.Checked && tripStartutc > endtimelocal.ToUniversalTime())            // if out of time, due to tripstart being after endtime
                    isTravelling = false;

                DateTime[] starttimeutc = SetupSummary(endtimelocal, isTravelling ? tripStartutc.ToLocalTime() : DateTime.Now, dataGridViewScan, dbScanSummary);

                scanlists = new List<Stats.ScanInfo>[intervals];

                scanlists[0] = currentstats.Scans.Where(x => x.utc >= starttimeutc[0]).Distinct(new Stats.ScansAreForSameBody()).ToList();
                scanlists[1] = currentstats.Scans.Where(x => x.utc >= starttimeutc[1]).Distinct(new Stats.ScansAreForSameBody()).ToList();
                scanlists[2] = currentstats.Scans.Where(x => x.utc >= starttimeutc[2]).Distinct(new Stats.ScansAreForSameBody()).ToList();
                scanlists[3] = currentstats.Scans.Where(x => x.utc >= starttimeutc[3]).Distinct(new Stats.ScansAreForSameBody()).ToList();
                scanlists[4] = currentstats.Scans.Where(x => x.utc >= starttimeutc[4]).Distinct(new Stats.ScansAreForSameBody()).ToList();
                scanlists[5] = currentstats.Scans.Distinct(new Stats.ScansAreForSameBody()).ToList();
            }
            else
            {
                intervals = 12;
                DateTime[] timeintervalsutc = SetUpDaysMonths(endtimelocal, dataGridViewScan, userControlStatsTimeScan.TimeMode, intervals, dbScanDWM);

                scanlists = new List<Stats.ScanInfo>[intervals];
                for (int ii = 0; ii < intervals; ii++)
                    scanlists[ii] = currentstats.Scans.Where(x => x.utc >= timeintervalsutc[ii + 1] && x.utc < timeintervalsutc[ii]).Distinct(new Stats.ScansAreForSameBody()).ToList();
            }

            for (int i = 1; i < dataGridViewScan.Columns.Count; i++)
                ColumnValueAlignment(dataGridViewScan.Columns[i] as DataGridViewTextBoxColumn);

            string[] strarr = new string[intervals];

            if (userControlStatsTimeScan.StarPlanetMode)
            {
                foreach (EDStar startype in Enum.GetValues(typeof(EDStar)))
                {
                    for (int ii = 0; ii < intervals; ii++)
                    {
                        int nr = 0;
                        for (int jj = 0; jj < scanlists[ii].Count; jj++)
                        {
                            if (scanlists[ii][jj].starid == startype )
                                nr++;
                        }

                        strarr[ii] = nr.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);

                    }

                    StatToDGV(dataGridViewScan, Bodies.StarName(startype), strarr);
                }
            }
            else
            {
                foreach (EDPlanet planettype in Enum.GetValues(typeof(EDPlanet)))
                {
                    for (int ii = 0; ii < intervals; ii++)
                    {
                        int nr = 0;
                        for (int jj = 0; jj < scanlists[ii].Count; jj++)
                        {
                            if (scanlists[ii][jj].planetid == planettype && scanlists[ii][jj].starid == null)
                                nr++;
                        }

                        strarr[ii] = nr.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);

                    }

                    StatToDGV(dataGridViewScan, planettype == EDPlanet.Unknown_Body_Type ? "Belt Cluster".T(EDTx.UserControlStats_Beltcluster) : Bodies.PlanetTypeName(planettype), strarr);
                }
            }

            if (sortcol < dataGridViewScan.Columns.Count)
            {
                dataGridViewScan.Sort(dataGridViewScan.Columns[sortcol], (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                dataGridViewScan.Columns[sortcol].HeaderCell.SortGlyphDirection = sortorder;
            }
        }

        private void userControlStatsTimeScan_DrawModeChanged(object sender, EventArgs e)
        {
            userControlStatsTimeScan_TimeModeChanged(sender, e);
        }
        private void dataGridViewScan_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Tag == null)     // tag null means numeric sort.
                e.SortDataGridViewColumnNumeric();
        }
        private void userControlStatsTimeScan_TimeModeChanged(object sender, EventArgs e)
        {
            dataGridViewScan.Rows.Clear();        // reset all
            DGVSaveColumnLayout(dataGridViewScan, dataGridViewScan.Columns.Count <= 8 ? dbScanSummary : dbScanDWM);
            dataGridViewScan.Columns.Clear();
            Display();
        }

        #endregion

 
        #region STATS IN GAME *************************************************************************************************************************
        void StatsGame()
        {
            string collapseExpand = GameStatTreeState();

            if (string.IsNullOrEmpty(collapseExpand))
                collapseExpand = GetSetting(dbStatsTreeStateSave, "YYYYYYYYYYYYYYYY");

            if (collapseExpand.Length < 16)
                collapseExpand += new string('Y', 16);

            JournalStatistics stats = currentstats.laststats;

            if (stats != null) // may not have one
            {
                AddTreeList("N1", "@", new string[] { currentstats.laststats.EventTimeLocal.ToString() }, collapseExpand[0]);

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

        static JournalTypeEnum[] journalsForShipStats = new JournalTypeEnum[]
        {
            JournalTypeEnum.FSDJump,
            JournalTypeEnum.Scan,
            JournalTypeEnum.MarketBuy,
            JournalTypeEnum.MarketSell,
            JournalTypeEnum.Died,
        };

        void StatsByShip()
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

            foreach (var kvp in currentstats.Ships)
            {
                var fsd = currentstats.FSDJumps.Where(x => x.shipid == kvp.Key);
                var scans = currentstats.Scans.Where(x => x.shipid == kvp.Key);
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


#endregion
    }

}
