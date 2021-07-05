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
                            System.Diagnostics.Debug.WriteLine("Died {0} {1}", st.currentshipid, cls.died);
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
                AddTreeNode("@", "T", currentstats.laststats.EventTimeLocal.ToString());


                string bank = "Bank Account".T(EDTx.UserControlStats_BankAccount);
                var node = AddTreeNode(bank, "Current Assets".T(EDTx.UserControlStats_CurrentAssets), stats.BankAccount?.CurrentWealth.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(bank, "Spent on Ships".T(EDTx.UserControlStats_SpentonShips), stats.BankAccount?.SpentOnShips.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(bank, "Spent on Outfitting".T(EDTx.UserControlStats_SpentonOutfitting), stats.BankAccount?.SpentOnOutfitting.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(bank, "Spent on Repairs".T(EDTx.UserControlStats_SpentonRepairs), stats.BankAccount?.SpentOnRepairs.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(bank, "Spent on Fuel".T(EDTx.UserControlStats_SpentonFuel), stats.BankAccount?.SpentOnFuel.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(bank, "Spent on Munitions".T(EDTx.UserControlStats_SpentonMunitions), stats.BankAccount?.SpentOnAmmoConsumables.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(bank, "Insurance Claims".T(EDTx.UserControlStats_InsuranceClaims), stats.BankAccount?.InsuranceClaims.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(bank, "Total Claim Costs".T(EDTx.UserControlStats_TotalClaimCosts), stats.BankAccount?.SpentOnInsurance.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(bank, "Owned Ships".T(EDTx.UserControlStats_OwnedShipCount), stats.BankAccount?.OwnedShipCount.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(bank, "Spent on Suits".T(EDTx.UserControlStats_SpentOnSuits), stats.BankAccount?.SpentOnSuits.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(bank, "Spent on Weapons".T(EDTx.UserControlStats_SpentOnWeapons), stats.BankAccount?.SpentOnWeapons.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(bank, "Spent on Suit Consumables".T(EDTx.UserControlStats_SpentOnSuitConsumables), stats.BankAccount?.SpentOnSuitConsumables.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(bank, "Suits Owned".T(EDTx.UserControlStats_SuitsOwned), stats.BankAccount?.SuitsOwned.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(bank, "Weapons Owned".T(EDTx.UserControlStats_WeaponsOwned), stats.BankAccount?.WeaponsOwned.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(bank, "Spent on Premium Stock".T(EDTx.UserControlStats_SpentOnPremiumStock), stats.BankAccount?.SpentOnPremiumStock.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(bank, "Premium Stock Bought".T(EDTx.UserControlStats_PremiumStockBought), stats.BankAccount?.PremiumStockBought.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
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
                AddTreeNode(combat, "Dropships Taken".T(EDTx.UserControlStats_DropshipsTaken), stats.Combat?.DropshipsTaken.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(combat, "Dropships Booked".T(EDTx.UserControlStats_DropshipsBooked), stats.Combat?.DropShipsBooked.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(combat, "Dropships Cancelled".T(EDTx.UserControlStats_DropshipsCancelled), stats.Combat?.DropshipsCancelled.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(combat, "High Intensity Conflict Zones fought".T(EDTx.UserControlStats_ConflictZoneHigh), stats.Combat?.ConflictZoneHigh.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(combat, "Medium Intensity Conflict Zones fought".T(EDTx.UserControlStats_ConflictZoneMedium), stats.Combat?.ConflictZoneMedium.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(combat, "Low Intensity Conflict Zones fought".T(EDTx.UserControlStats_ConflictZoneLow), stats.Combat?.ConflictZoneLow.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(combat, "Total Conflict Zones fought".T(EDTx.UserControlStats_ConflictZoneTotal), stats.Combat?.ConflictZoneTotal.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(combat, "High Intensity Conflict Zones won".T(EDTx.UserControlStats_ConflictZoneHighWins), stats.Combat?.ConflictZoneHighWins.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(combat, "Medium Intensity Conflict Zones won".T(EDTx.UserControlStats_ConflictZoneMediumWins), stats.Combat?.ConflictZoneMediumWins.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(combat, "Low Intensity Conflict Zones won".T(EDTx.UserControlStats_ConflictZoneLowWins), stats.Combat?.ConflictZoneLowWins.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(combat, "Total Conflict Zones won".T(EDTx.UserControlStats_ConflictZoneTotalWins), stats.Combat?.ConflictZoneTotalWins.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(combat, "Settlements Defended".T(EDTx.UserControlStats_SettlementDefended), stats.Combat?.SettlementDefended.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(combat, "Settlements Conquered".T(EDTx.UserControlStats_SettlementConquered), stats.Combat?.SettlementConquered.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(combat, "Skimmers Killed on Foot".T(EDTx.UserControlStats_OnFootSkimmersKilled), stats.Combat?.OnFootSkimmersKilled.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(combat, "Scavengers Killed on Foot".T(EDTx.UserControlStats_OnFootScavsKilled), stats.Combat?.OnFootScavsKilled.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                if (collapseExpand[1] == 'Y')
                    node.Item1.Expand();

                string crime = "Crime".T(EDTx.UserControlStats_Crime);
                node = AddTreeNode(crime, "Notoriety".T(EDTx.UserControlStats_Notoriety), stats.Crime?.Notoriety.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(crime, "Number of Fines".T(EDTx.UserControlStats_NumberofFines), stats.Crime?.Fines.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(crime, "Lifetime Fines Value".T(EDTx.UserControlStats_LifetimeFinesValue), stats.Crime?.TotalFines.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(crime, "Bounties Received".T(EDTx.UserControlStats_BountiesReceived), stats.Crime?.BountiesReceived.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(crime, "Lifetime Bounty Value".T(EDTx.UserControlStats_LifetimeBountyValue), stats.Crime?.TotalBounties.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(crime, "Highest Bounty Issued".T(EDTx.UserControlStats_HighestBountyIssued), stats.Crime?.HighestBounty.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(crime, "Malware Uploaded".T(EDTx.UserControlStats_MalwareUploaded), stats.Crime?.MalwareUploaded.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(crime, "Settlements shut down".T(EDTx.UserControlStats_SettlementsStateShutdown), stats.Crime?.SettlementsStateShutdown.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(crime, "Production Sabotaged".T(EDTx.UserControlStats_ProductionSabotage), stats.Crime?.ProductionSabotage.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(crime, "Production Thefts".T(EDTx.UserControlStats_ProductionTheft), stats.Crime?.ProductionTheft.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(crime, "Total Murders".T(EDTx.UserControlStats_TotalMurders), stats.Crime?.TotalMurders.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(crime, "Citizens Murdered".T(EDTx.UserControlStats_CitizensMurdered), stats.Crime?.CitizensMurdered.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(crime, "Omnipol Murdered".T(EDTx.UserControlStats_OmnipolMurdered), stats.Crime?.OmnipolMurdered.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(crime, "Guards Murdered".T(EDTx.UserControlStats_GuardsMurdered), stats.Crime?.GuardsMurdered.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(crime, "Data Stolen".T(EDTx.UserControlStats_DataStolen), stats.Crime?.DataStolen.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(crime, "Goods Stolen".T(EDTx.UserControlStats_GoodsStolen), stats.Crime?.GoodsStolen.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(crime, "Production Samples Stolen".T(EDTx.UserControlStats_SampleStolen), stats.Crime?.SampleStolen.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(crime, "Total Inventory Items Stolen".T(EDTx.UserControlStats_TotalStolen), stats.Crime?.TotalStolen.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(crime, "Turrets Destroyed".T(EDTx.UserControlStats_TurretsDestroyed), stats.Crime?.TurretsDestroyed.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(crime, "Turrets Overloaded".T(EDTx.UserControlStats_TurretsOverloaded), stats.Crime?.TurretsOverloaded.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(crime, "Total Turrets shut down".T(EDTx.UserControlStats_TurretsTotal), stats.Crime?.TurretsTotal.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(crime, "Stolen Items Value".T(EDTx.UserControlStats_ValueStolenStateChange), stats.Crime?.ValueStolenStateChange.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(crime, "ProfilesCloned".T(EDTx.UserControlStats_ProfilesCloned), stats.Crime?.ProfilesCloned.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
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
                AddTreeNode(trading, "Data Sold".T(EDTx.UserControlStats_DataSold), stats.Trading?.DataSold.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(trading, "Goods Sold".T(EDTx.UserControlStats_TradingGoodsSold), stats.Trading?.GoodsSold.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(trading, "Assets Sold".T(EDTx.UserControlStats_AssetsSold), stats.Trading?.AssetsSold.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
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
                AddTreeNode(exploration, "Efficient Scans".T(EDTx.UserControlStats_EfficientScans), stats.Exploration?.EfficientScans.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(exploration, "Highest Payout".T(EDTx.UserControlStats_HighestPayout), stats.Exploration?.HighestPayout.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(exploration, "Total Hyperspace Distance".T(EDTx.UserControlStats_TotalHyperspaceDistance), stats.Exploration?.TotalHyperspaceDistance.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "ly");
                AddTreeNode(exploration, "Total Hyperspace Jumps".T(EDTx.UserControlStats_TotalHyperspaceJumps), stats.Exploration?.TotalHyperspaceJumps.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(exploration, "Farthest From Start".T(EDTx.UserControlStats_FarthestFromStart), stats.Exploration?.GreatestDistanceFromStart.ToString("N2", System.Globalization.CultureInfo.CurrentCulture), "ly");
                AddTreeNode(exploration, "Time Played".T(EDTx.UserControlStats_TimePlayed), stats.Exploration?.TimePlayed.SecondsToWeeksDaysHoursMinutesSeconds());
                AddTreeNode(exploration, "Shuttle Journeys".T(EDTx.UserControlStats_ShuttleJourneys), stats.Exploration?.ShuttleJourneys.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(exploration, "Shuttle Distance Travelled".T(EDTx.UserControlStats_ShuttleDistanceTravelled), stats.Exploration?.ShuttleDistanceTravelled.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "ly");
                AddTreeNode(exploration, "Credits Spent on Shuttles".T(EDTx.UserControlStats_SpentOnShuttles), stats.Exploration?.SpentOnShuttles.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "cr");
                AddTreeNode(exploration, "First Footfalls".T(EDTx.UserControlStats_FirstFootfalls), stats.Exploration?.FirstFootfalls.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(exploration, "Planets walked on".T(EDTx.UserControlStats_PlanetFootfalls), stats.Exploration?.PlanetFootfalls.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(exploration, "Settlements docked at".T(EDTx.UserControlStats_SettlementsVisited), stats.Exploration?.SettlementsVisited.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                if (collapseExpand[6] == 'Y')
                    node.Item1.Expand();

                string passengers = "Passengers".T(EDTx.UserControlStats_Passengers);
                node = AddTreeNode(passengers, "Total Bulk Passengers Delivered".T(EDTx.UserControlStats_TotalBulkPassengersDelivered), stats.PassengerMissions?.Bulk.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(passengers, "Total VIPs Delivered".T(EDTx.UserControlStats_TotalVIPsDelivered), stats.PassengerMissions?.VIP.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(passengers, "Accepted".T(EDTx.UserControlStats_Accepted), stats.PassengerMissions?.Accepted.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(passengers, "Delivered".T(EDTx.UserControlStats_Delivered), stats.PassengerMissions?.Delivered.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(passengers, "Disgruntled".T(EDTx.UserControlStats_Disgruntled), stats.PassengerMissions?.Disgruntled.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(passengers, "Ejected".T(EDTx.UserControlStats_Ejected), stats.PassengerMissions?.Ejected.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                if (collapseExpand[7] == 'Y')
                    node.Item1.Expand();

                string search = "Search and Rescue".T(EDTx.UserControlStats_SearchandRescue);
                node = AddTreeNode(search, "Total Items Rescued".T(EDTx.UserControlStats_TotalItemsRescued), stats.SearchAndRescue?.Traded.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(search, "Total Profit".T(EDTx.UserControlStats_TotalProfit), stats.SearchAndRescue?.Profit.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(search, "Total Rescue Transactions".T(EDTx.UserControlStats_TotalRescueTransactions), stats.SearchAndRescue?.Count.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(search, "Legal Salvage Value - Surface".T(EDTx.UserControlStats_SalvageLegalPOI), stats.SearchAndRescue?.SalvageLegalPOI.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(search, "Legal Salvage Value - Settlements".T(EDTx.UserControlStats_SalvageLegalSettlements), stats.SearchAndRescue?.SalvageLegalSettlements.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(search, "Illegal Salvage Value - Surface".T(EDTx.UserControlStats_SalvageIllegalPOI), stats.SearchAndRescue?.SalvageIllegalPOI.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(search, "Illegal Salvage Value - Settlements".T(EDTx.UserControlStats_SalvageIllegalSettlements), stats.SearchAndRescue?.SalvageIllegalSettlements.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(search, "Maglocks cut".T(EDTx.UserControlStats_MaglocksOpened), stats.SearchAndRescue?.MaglocksOpened.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(search, "Panels cut".T(EDTx.UserControlStats_PanelsOpened), stats.SearchAndRescue?.PanelsOpened.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(search, "Settlement Fires extinguished".T(EDTx.UserControlStats_SettlementsStateFireOut), stats.SearchAndRescue?.SettlementsStateFireOut.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(search, "Settlements rebooted".T(EDTx.UserControlStats_SettlementsStateReboot), stats.SearchAndRescue?.SettlementsStateReboot.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
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
                AddTreeNode(craft, "Suit Modifications Applied".T(EDTx.UserControlStats_SuitModsApplied), stats.Crafting?.SuitModsApplied.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(craft, "Weapon Modifications Applied".T(EDTx.UserControlStats_WeaponModsApplied), stats.Crafting?.WeaponModsApplied.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(craft, "Suit Upgrades Applied".T(EDTx.UserControlStats_SuitsUpgraded), stats.Crafting?.SuitsUpgraded.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(craft, "Weapon Upgrades Applied".T(EDTx.UserControlStats_WeaponsUpgraded), stats.Crafting?.WeaponsUpgraded.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(craft, "Suits fully Upgraded".T(EDTx.UserControlStats_SuitsUpgradedFull), stats.Crafting?.SuitsUpgradedFull.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(craft, "Weapons fully Upgraded".T(EDTx.UserControlStats_WeaponsUpgradedFull), stats.Crafting?.WeaponsUpgradedFull.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(craft, "Suits fully Modified".T(EDTx.UserControlStats_SuitModsAppliedFull), stats.Crafting?.SuitModsAppliedFull.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(craft, "Weapons fully Modified".T(EDTx.UserControlStats_WeaponModsAppliedFull), stats.Crafting?.WeaponModsAppliedFull.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
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
                node = AddTreeNode(mattrader, "Material Trades Completed".T(EDTx.UserControlStats_TradesCompleted), stats.MaterialTraderStats?.TradesCompleted.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(mattrader, "Materials Traded".T(EDTx.UserControlStats_MaterialsTraded), stats.MaterialTraderStats?.MaterialsTraded.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(mattrader, "Encoded Materials Traded".T(EDTx.UserControlStats_EncodedMaterialsTraded), stats.MaterialTraderStats?.EncodedMaterialsTraded.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(mattrader, "Raw Materials Traded".T(EDTx.UserControlStats_RawMaterialsTraded), stats.MaterialTraderStats?.RawMaterialsTraded.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(mattrader, "Grade 1 Materials Traded".T(EDTx.UserControlStats_G1MaterialsTraded), stats.MaterialTraderStats?.Grade1MaterialsTraded.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(mattrader, "Grade 2 Materials Traded".T(EDTx.UserControlStats_G2MaterialsTraded), stats.MaterialTraderStats?.Grade2MaterialsTraded.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(mattrader, "Grade 3 Materials Traded".T(EDTx.UserControlStats_G3MaterialsTraded), stats.MaterialTraderStats?.Grade3MaterialsTraded.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(mattrader, "Grade 4 Materials Traded".T(EDTx.UserControlStats_G4MaterialsTraded), stats.MaterialTraderStats?.Grade4MaterialsTraded.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(mattrader, "Grade 5 Materials Traded".T(EDTx.UserControlStats_G5MaterialsTraded), stats.MaterialTraderStats?.Grade5MaterialsTraded.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                if (collapseExpand[12] == 'Y')
                    node.Item1.Expand();

                string CQC = "CQC".T(EDTx.UserControlStats_CQC);
                node = AddTreeNode(CQC, "Profits from CQC".T(EDTx.UserControlStats_CreditsEarned), stats.CQC?.CreditsEarned.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(CQC, "Time Played".T(EDTx.UserControlStats_CQCTimePlayed), stats.CQC?.TimePlayed.SecondsToWeeksDaysHoursMinutesSeconds());
                AddTreeNode(CQC, "K/D Ratio".T(EDTx.UserControlStats_KDRatio), stats.CQC?.KD.ToString("N2", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(CQC, "Kills".T(EDTx.UserControlStats_Kills), stats.CQC?.Kills.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(CQC, "Win/Loss".T(EDTx.UserControlStats_Win), stats.CQC?.WL.ToString("N2", System.Globalization.CultureInfo.CurrentCulture));
                if (collapseExpand[13] == 'Y')
                    node.Item1.Expand();

                string FLEETCARRIER = "Fleetcarrier".T(EDTx.UserControlStats_FLEETCARRIER);
                node = AddTreeNode(FLEETCARRIER, "Total Commodities Exported".T(EDTx.UserControlStats_EXPORTTOTAL), stats.FLEETCARRIER?.EXPORTTOTAL.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(FLEETCARRIER, "Total Commodities Imported".T(EDTx.UserControlStats_IMPORTTOTAL), stats.FLEETCARRIER?.IMPORTTOTAL.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(FLEETCARRIER, "Credits earned from Commodities".T(EDTx.UserControlStats_TRADEPROFITTOTAL), stats.FLEETCARRIER?.TRADEPROFITTOTAL.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(FLEETCARRIER, "Credits spent on Commodities".T(EDTx.UserControlStats_TRADESPENDTOTAL), stats.FLEETCARRIER?.TRADESPENDTOTAL.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(FLEETCARRIER, "Credits earned from Stolen Goods".T(EDTx.UserControlStats_STOLENPROFITTOTAL), stats.FLEETCARRIER?.STOLENPROFITTOTAL.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(FLEETCARRIER, "Credits spent on Stolen Goods".T(EDTx.UserControlStats_STOLENSPENDTOTAL), stats.FLEETCARRIER?.STOLENSPENDTOTAL.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(FLEETCARRIER, "Total Travel Distance".T(EDTx.UserControlStats_DISTANCETRAVELLED), stats.FLEETCARRIER?.DISTANCETRAVELLED.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(FLEETCARRIER, "Number of Carrier Jumps".T(EDTx.UserControlStats_TOTALJUMPS), stats.FLEETCARRIER?.TOTALJUMPS.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(FLEETCARRIER, "Total Ships Sold".T(EDTx.UserControlStats_SHIPYARDSOLD), stats.FLEETCARRIER?.SHIPYARDSOLD.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(FLEETCARRIER, "Credits earned from Shipyard".T(EDTx.UserControlStats_SHIPYARDPROFIT), stats.FLEETCARRIER?.SHIPYARDPROFIT.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(FLEETCARRIER, "Total Modules Sold".T(EDTx.UserControlStats_OUTFITTINGSOLD), stats.FLEETCARRIER?.OUTFITTINGSOLD.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(FLEETCARRIER, "Credits earned from Outfitting".T(EDTx.UserControlStats_OUTFITTINGPROFIT), stats.FLEETCARRIER?.OUTFITTINGPROFIT.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(FLEETCARRIER, "Total Ships Restocked".T(EDTx.UserControlStats_REARMTOTAL), stats.FLEETCARRIER?.REARMTOTAL.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(FLEETCARRIER, "Total Ships Refuelled".T(EDTx.UserControlStats_REFUELTOTAL), stats.FLEETCARRIER?.REFUELTOTAL.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(FLEETCARRIER, "Credits earned from Refuelling".T(EDTx.UserControlStats_REFUELPROFIT), stats.FLEETCARRIER?.REFUELPROFIT.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(FLEETCARRIER, "Total Ships Repaired".T(EDTx.UserControlStats_REPAIRSTOTAL), stats.FLEETCARRIER?.REPAIRSTOTAL.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(FLEETCARRIER, " Redemption Office Exchanges".T(EDTx.UserControlStats_VOUCHERSREDEEMED), stats.FLEETCARRIER?.VOUCHERSREDEEMED.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(FLEETCARRIER, " Redemption Office Payouts".T(EDTx.UserControlStats_VOUCHERSPROFIT), stats.FLEETCARRIER?.VOUCHERSPROFIT.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                if (collapseExpand[14] == 'Y')
                    node.Item1.Expand();

                string Exobiology = "Exobiology".T(EDTx.UserControlStats_Exobiology);
                node = AddTreeNode(Exobiology, "Unique Genus Encountered".T(EDTx.UserControlStats_OrganicGenusEncountered), stats.Exobiology?.OrganicGenusEncountered.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(Exobiology, "Unique Species Encountered".T(EDTx.UserControlStats_OrganicSpeciesEncountered), stats.Exobiology?.OrganicSpeciesEncountered.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(Exobiology, "Unique Variants Encountered".T(EDTx.UserControlStats_OrganicVariantEncountered), stats.Exobiology?.OrganicVariantEncountered.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(Exobiology, "Profit from Organic Data".T(EDTx.UserControlStats_OrganicDataProfits), stats.Exobiology?.OrganicDataProfits.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(Exobiology, "Organic Data Registered".T(EDTx.UserControlStats_OrganicData), stats.Exobiology?.OrganicData.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(Exobiology, "Profit from First Logged".T(EDTx.UserControlStats_FirstLoggedProfits), stats.Exobiology?.FirstLoggedProfits.ToString("N0", System.Globalization.CultureInfo.CurrentCulture), "Cr");
                AddTreeNode(Exobiology, "First Logged".T(EDTx.UserControlStats_FirstLogged), stats.Exobiology?.FirstLogged.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(Exobiology, "Systems with Organic Life".T(EDTx.UserControlStats_OrganicSystems), stats.Exobiology?.OrganicSystems.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(Exobiology, "Planets with Organic Life".T(EDTx.UserControlStats_OrganicPlanets), stats.Exobiology?.OrganicPlanets.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(Exobiology, "Unique Genus Data Logged".T(EDTx.UserControlStats_OrganicGenus), stats.Exobiology?.OrganicGenus.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));
                AddTreeNode(Exobiology, "Unique Species Data Logged".T(EDTx.UserControlStats_OrganicSpecies), stats.Exobiology?.OrganicSpecies.ToString("N0", System.Globalization.CultureInfo.CurrentCulture));                
                if (collapseExpand[15] == 'Y')
                    node.Item1.Expand();
            }
            else
                treeViewStats.Nodes.Clear();
        }

        string SecondsToDHMString(int? seconds)
        {
            if (!seconds.HasValue)
                return "";

            TimeSpan time = TimeSpan.FromSeconds(seconds.Value);
            return string.Format("{0} days {1} hours {2} minutes".T(EDTx.UserControlStats_TME), time.Days, time.Hours, time.Minutes);
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
