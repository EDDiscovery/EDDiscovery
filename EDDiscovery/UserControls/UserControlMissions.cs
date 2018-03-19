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
using EDDiscovery.Controls;
using EliteDangerousCore.DB;
using EliteDangerousCore;

namespace EDDiscovery.UserControls
{
    public partial class UserControlMissions : UserControlCommonBase
    {
        private string DbColumnSaveCurrent { get { return ("MissionsGridCurrent") + ((displaynumber > 0) ? displaynumber.ToString() : "") + "DGVCol"; } }
        private string DbColumnSavePrevious { get { return ("MissionsGridPrevious") + ((displaynumber > 0) ? displaynumber.ToString() : "") + "DGVCol"; } }
        private string DbStartDate { get { return ("MissionsStartDate") + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbEndDate { get { return ("MissionsEndDate") + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbStartDateChecked { get { return ("MissionsStartDateCheck") + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbEndDateChecked { get { return ("MissionsEndDateCheck") + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbSplitter { get { return ("MissionsSplitter") + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        #region Init

        public UserControlMissions()
        {
            InitializeComponent();
            var corner = dataGridViewCurrent.TopLeftHeaderCell; // work around #1487
            var corner2 = dataGridViewPrevious.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            dataGridViewCurrent.MakeDoubleBuffered();
            dataGridViewCurrent.RowTemplate.Height = 26;
            dataGridViewCurrent.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewCurrent.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx

            dataGridViewPrevious.MakeDoubleBuffered();
            dataGridViewPrevious.RowTemplate.Height = 26;
            dataGridViewPrevious.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewPrevious.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx

            string start = SQLiteDBClass.GetSettingString(DbStartDate, "");
            DateTime dt;
            if (start != "" && DateTime.TryParse(start, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                customDateTimePickerStart.Value = dt;

            customDateTimePickerStart.Checked = SQLiteDBClass.GetSettingBool(DbStartDateChecked, false);

            string end = SQLiteDBClass.GetSettingString(DbEndDate, "");
            if (end != "" && DateTime.TryParse(end, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                customDateTimePickerEnd.Value = dt;

            customDateTimePickerEnd.Checked = SQLiteDBClass.GetSettingBool(DbEndDateChecked, false);

            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            uctg.OnTravelSelectionChanged += Display;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;
        }

        #endregion

        #region Display

        public override void InitialDisplay()
        {
            Display(uctg.GetCurrentHistoryEntry, discoveryform.history);
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he, HistoryList hl)
        {
            last_he = he;
            Display();
        }

        HistoryEntry last_he = null;
        private void Display(HistoryEntry he, HistoryList hl)
        {
            last_he = he;
            Display();
        }

        private void Display()
        {
            MissionList ml = last_he?.MissionList;

            dataGridViewCurrent.Rows.Clear();
            dataGridViewPrevious.Rows.Clear();

            if (ml != null)
            {
                DateTime hetime = last_he.EventTimeUTC;

                List<MissionState> mcurrent = (from MissionState ms in ml.Missions.Values where ms.InProgressDateTime(hetime) orderby ms.Mission.EventTimeUTC descending select ms).ToList();

                var totalReward = 0;
                foreach (MissionState ms in mcurrent)
                {
                    object[] rowobj = { JournalFieldNaming.ShortenMissionName(ms.Mission.Name) ,
                                        EDDiscoveryForm.EDDConfig.DisplayUTC ? ms.Mission.EventTimeUTC : ms.Mission.EventTimeLocal,
                                        EDDiscoveryForm.EDDConfig.DisplayUTC ? ms.Mission.Expiry : ms.Mission.Expiry.ToLocalTime(),
                                        ms.OriginatingSystem + ":" + ms.OriginatingStation,
                                        ms.Mission.Faction,
                                        ms.DestinationSystemStation(),
                                        ms.Mission.TargetFaction,
                                        ms.Mission.Reward.GetValueOrDefault().ToString("N0"),
                                        ms.Info()
                    };

                    if (ms.Mission.Reward.HasValue)
                    {
                        totalReward += ms.Mission.Reward.Value;
                    }

                    int rowno = dataGridViewCurrent.Rows.Add(rowobj);
                    dataGridViewCurrent.Rows[rowno].Tag = ms;
                }


                cColValue.HeaderText = (totalReward!=0) ? $"Reward (cr)\nTotal: {totalReward:N0}" : "Reward (cr)";

                List<MissionState> mprev = (from MissionState ms in ml.Missions.Values where !ms.InProgressDateTime(hetime) orderby ms.Mission.EventTimeUTC descending select ms).ToList();

                DateTime startdate = customDateTimePickerStart.Checked ? customDateTimePickerStart.Value : new DateTime(1980, 1, 1);
                DateTime enddate = customDateTimePickerEnd.Checked ? customDateTimePickerEnd.Value : new DateTime(2999, 1, 1);

                long value = 0;
                int completed = 0, abandonded = 0, failed = 0;

                foreach (MissionState ms in mprev)
                {
                    int cmps = EDDiscoveryForm.EDDConfig.DisplayUTC ? DateTime.Compare(ms.Mission.EventTimeUTC, startdate) : DateTime.Compare(ms.Mission.EventTimeLocal, startdate);

                    //System.Diagnostics.Debug.WriteLine(ms.Mission.EventTimeUTC.ToString() + " " + startdate.ToString() + " " + cmps);
                    if (cmps >= 0)
                    {
                        int cmpe = EDDiscoveryForm.EDDConfig.DisplayUTC ? DateTime.Compare(ms.Mission.EventTimeUTC, enddate) : DateTime.Compare(ms.Mission.EventTimeLocal, enddate);

                        if (cmpe <= 0)
                        {
                            if (ms.State == MissionState.StateTypes.Abandoned)
                                abandonded++;
                            else if (ms.State == MissionState.StateTypes.Completed)
                                completed++;
                            else if (ms.State == MissionState.StateTypes.Failed)
                                failed++;

                            object[] rowobj = { JournalFieldNaming.ShortenMissionName(ms.Mission.Name) ,
                                        EDDiscoveryForm.EDDConfig.DisplayUTC ? ms.Mission.EventTimeUTC : ms.Mission.EventTimeLocal,
                                        EDDiscoveryForm.EDDConfig.DisplayUTC ? ms.MissionEndTime : ms.MissionEndTime.ToLocalTime(),
                                        ms.OriginatingSystem + ":" + ms.OriginatingStation,
                                        ms.Mission.Faction,
                                        ms.DestinationSystemStation(),
                                        ms.Mission.TargetFaction,
                                        ms.StateText,
                                        ms.Info()
                                        };

                            int rowno = dataGridViewPrevious.Rows.Add(rowobj);
                            dataGridViewPrevious.Rows[rowno].Tag = ms;

                            value += ms.Value;
                        }
                    }
                }

                labelValue.Visible = (value != 0);
                labelValue.Text = "Value: " + value.ToStringInvariant() + " C:" + completed.ToStringInvariant() + " A:" + abandonded.ToStringInvariant() + " F:" + failed.ToStringInvariant();
            }
        }

        #endregion

        #region Layout

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridViewCurrent, DbColumnSaveCurrent);
            DGVLoadColumnLayout(dataGridViewPrevious, DbColumnSavePrevious);

            splitContainerMissions.SplitterDistance(SQLiteDBClass.GetSettingDouble(DbSplitter, 0.4));
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewCurrent, DbColumnSaveCurrent);
            DGVSaveColumnLayout(dataGridViewPrevious, DbColumnSavePrevious);

            uctg.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;

            SQLiteDBClass.PutSettingString(DbStartDate, customDateTimePickerStart.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            SQLiteDBClass.PutSettingString(DbEndDate, customDateTimePickerEnd.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));

            SQLiteDBClass.PutSettingBool(DbStartDateChecked, customDateTimePickerStart.Checked);
            SQLiteDBClass.PutSettingBool(DbEndDateChecked, customDateTimePickerEnd.Checked);

            SQLiteDBClass.PutSettingDouble(DbSplitter, splitContainerMissions.GetSplitterDistance());
        }

        #endregion

        private void customDateTimePickerStart_ValueChanged(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Start changed");
            Display();
        }

        private void customDateTimePickerEnd_ValueChanged(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("End changed");
            Display();
        }

        private void dataGridViewPrevious_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 7)
            {
                double v1;
                double v2;
                bool v1hasval = Double.TryParse(e.CellValue1?.ToString().Replace("cr", ""), out v1);
                bool v2hasval = Double.TryParse(e.CellValue2?.ToString().Replace("cr", ""), out v2);

                if (v1hasval)
                {
                    if (v2hasval)
                        e.SortResult = v1.CompareTo(v2);
                    else
                        e.SortResult = 1;
                }
                else if (v2hasval)
                    e.SortResult = -1;
                else
                    return;

                e.Handled = true;
            }

        }
    }
}

