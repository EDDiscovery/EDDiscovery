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
using EDDiscovery.Controls;
using EliteDangerousCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlMissions : UserControlCommonBase
    {
        private string DbColumnSaveCurrent { get { return DBName("MissionsGridCurrent" ,  "DGVCol"); } }
        private string DbColumnSavePrevious { get { return DBName("MissionsGridPrevious" ,  "DGVCol"); } }
        private string DbStartDate { get { return DBName("MissionsStartDate" ); } }
        private string DbEndDate { get { return DBName("MissionsEndDate" ); } }
        private string DbStartDateChecked { get { return DBName("MissionsStartDateCheck" ); } }
        private string DbEndDateChecked { get { return DBName("MissionsEndDateCheck" ); } }
        private string DbSplitter { get { return DBName("MissionsSplitter") ; } }
        private DateTime NextExpiry;

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
            dataGridViewCurrent.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewCurrent.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx

            dataGridViewPrevious.MakeDoubleBuffered();
            dataGridViewPrevious.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewPrevious.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx

            customDateTimePickerStart.Value = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDate(DbStartDate, DateTime.UtcNow);
            customDateTimePickerStart.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbStartDateChecked, false);
            customDateTimePickerEnd.Value = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDate(DbEndDate, DateTime.UtcNow);
            customDateTimePickerEnd.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbEndDateChecked, false);
            VerifyDates();

            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);

            dataViewScrollerPanelPrev.LimitLargeChange = dataViewScrollerPanelCurrent.LimitLargeChange = 4;

            labelValue.Visible = false;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += Display;

            dataGridViewPrevious.RowTemplate.MinimumHeight = dataGridViewCurrent.RowTemplate.MinimumHeight = Font.ScalePixels(26);

            DGVLoadColumnLayout(dataGridViewCurrent, DbColumnSaveCurrent);
            DGVLoadColumnLayout(dataGridViewPrevious, DbColumnSavePrevious);

            splitContainerMissions.SplitterDistance(EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(DbSplitter, 0.4));
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewCurrent, DbColumnSaveCurrent);
            DGVSaveColumnLayout(dataGridViewPrevious, DbColumnSavePrevious);

            uctg.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;

            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDate(DbStartDate, EDDiscoveryForm.EDDConfig.ConvertTimeToUTCFromSelected(customDateTimePickerStart.Value));
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDate(DbEndDate, EDDiscoveryForm.EDDConfig.ConvertTimeToUTCFromSelected(customDateTimePickerEnd.Value));

            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbStartDateChecked, customDateTimePickerStart.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbEndDateChecked, customDateTimePickerEnd.Checked);

            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble(DbSplitter, splitContainerMissions.GetSplitterDistance());
        }

        #endregion

        #region Display

        public override void InitialDisplay()
        {
            Display(uctg.GetCurrentHistoryEntry, discoveryform.history);
        }

        private void Discoveryform_OnHistoryChange(HistoryList obj)
        {
            VerifyDates();
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he, HistoryList hl)
        {
            if (!object.ReferenceEquals(he.MissionList, last_he?.MissionList) || he.EventTimeUTC > NextExpiry)
            {
                last_he = he;
                Display();
                NextExpiry = he?.MissionList?.GetAllCurrentMissions(he.EventTimeUTC).OrderBy(e => e.MissionEndTime).FirstOrDefault()?.MissionEndTime ?? DateTime.MaxValue;
            }
        }

        HistoryEntry last_he = null;

        private void Display(HistoryEntry he, HistoryList hl) =>
            Display(he, hl, true);

        private void Display(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            last_he = he;
            Display();
            NextExpiry = he?.MissionList?.GetAllCurrentMissions(he.EventTimeUTC).OrderBy(e => e.MissionEndTime).FirstOrDefault()?.MissionEndTime ?? DateTime.MaxValue;
        }

        private void Display()
        {
            MissionList ml = last_he?.MissionList;

            DataGridViewColumn sortcolprev = dataGridViewPrevious.SortedColumn != null ? dataGridViewPrevious.SortedColumn : dataGridViewPrevious.Columns[1];
            SortOrder sortorderprev = dataGridViewPrevious.SortedColumn != null ? dataGridViewPrevious.SortOrder : SortOrder.Descending;

            DataGridViewColumn sortcolcur = dataGridViewCurrent.SortedColumn != null ? dataGridViewCurrent.SortedColumn : dataGridViewCurrent.Columns[1];
            SortOrder sortordercur = dataGridViewCurrent.SortedColumn != null ? dataGridViewCurrent.SortOrder : SortOrder.Descending;

            dataGridViewCurrent.Rows.Clear();
            dataGridViewPrevious.Rows.Clear();

            if (ml != null)
            {
                DateTime hetime = last_he.EventTimeUTC;

                List<MissionState> mcurrent = ml.GetAllCurrentMissions(hetime);

                var totalReward = 0;
                var currentRows = new List<DataGridViewRow>(mcurrent.Count);
                foreach (MissionState ms in mcurrent)
                {
                    object[] rowobj = { JournalFieldNaming.ShortenMissionName(ms.Mission.LocalisedName) ,
                                        EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(ms.Mission.EventTimeUTC),
                                        EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(ms.Mission.Expiry),
                                        ms.OriginatingSystem + ": " + ms.OriginatingStation,
                                        ms.Mission.Faction,
                                        ms.DestinationSystemStation(),
                                        ms.Mission.TargetFaction,
                                        ms.Mission.Reward.GetValueOrDefault().ToString("N0"),
                                        ms.MissionInfoColumn()
                    };

                    if (ms.Mission.Reward.HasValue)
                    {
                        totalReward += ms.Mission.Reward.Value;
                    }

                    var row = dataGridViewCurrent.RowTemplate.Clone() as DataGridViewRow;
                    row.CreateCells(dataGridViewCurrent, rowobj);
                    row.Tag = ms;
                    currentRows.Add(row);
                }

                dataGridViewCurrent.Rows.AddRange(currentRows.ToArray());

                int count = mcurrent.Count();

                cColName.HeaderText = (count > 0) ? (count.ToString() + (count > 1 ? " Missions".T(EDTx.UserControlMissions_MPlural) : " Mission".T(EDTx.UserControlMissions_MSingular))) : "Name".T(EDTx.UserControlMissions_Name);
                cColValue.HeaderText = (totalReward != 0) ? string.Format("Value (cr):\n{0:N0}".T(EDTx.UserControlMissions_Value) ,totalReward) : "Value (cr)".T(EDTx.UserControlMissions_ValueN);

                //                cColValue.HeaderText = (count>0) ? (count.ToStringInvariant() + (count > 1 ? " Missions" : " Mission") + (totalReward>0 ? $", {totalReward:N0}" : "")) : "Value";

                List<MissionState> mprev = ml.GetAllExpiredMissions(hetime);

                DateTime startdateutc = customDateTimePickerStart.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromSelected(customDateTimePickerStart.Value) : new DateTime(1980, 1, 1);
                DateTime enddateutc = customDateTimePickerEnd.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromSelected(customDateTimePickerEnd.Value) : new DateTime(8999, 1, 1);

                long value = 0;
                int completed = 0, abandonded = 0, failed = 0;
                var previousRows = new List<DataGridViewRow>(mprev.Count);

                foreach (MissionState ms in mprev)
                {
                    int cmps = DateTime.Compare(ms.Mission.EventTimeUTC, startdateutc);

                    //System.Diagnostics.Debug.WriteLine(ms.Mission.EventTimeUTC.ToString() + " " + startdate.ToString() + " " + cmps);
                    if (cmps >= 0)
                    {
                        int cmpe = DateTime.Compare(ms.Mission.EventTimeUTC, enddateutc);

                        if (cmpe <= 0)
                        {
                            if (ms.State == MissionState.StateTypes.Abandoned)
                                abandonded++;
                            else if (ms.State == MissionState.StateTypes.Completed)
                                completed++;
                            else if (ms.State == MissionState.StateTypes.Failed)
                                failed++;

                            object[] rowobj = { JournalFieldNaming.ShortenMissionName(ms.Mission.LocalisedName) ,
                                         EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(ms.Mission.EventTimeUTC),
                                         EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(ms.MissionEndTime),
                                        ms.OriginatingSystem + ": " + ms.OriginatingStation,
                                        ms.Mission.Faction,
                                        ms.DestinationSystemStation(),
                                        ms.Mission.TargetFaction,
                                        ms.StateText,
                                        ms.MissionInfoColumn()
                                        };

                            var row = dataGridViewPrevious.RowTemplate.Clone() as DataGridViewRow;
                            row.CreateCells(dataGridViewPrevious, rowobj);
                            row.Tag = ms;
                            previousRows.Add(row);

                            value += ms.Value;
                        }
                    }
                }

                dataGridViewPrevious.Rows.AddRange(previousRows.ToArray());

                labelValue.Visible = (value != 0);
                labelValue.Text = "Value: ".T(EDTx.UserControlMissions_ValueC) + value.ToString("N0") + " C:" + completed.ToString("N0") + " A:" + abandonded.ToString("N0") + " F:" + failed.ToString("N0");

                //System.Diagnostics.Debug.WriteLine("Prev " + sortorderprev + " " + sortcolprev.Index);
                dataGridViewPrevious.Sort(sortcolprev, (sortorderprev == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                dataGridViewPrevious.Columns[sortcolprev.Index].HeaderCell.SortGlyphDirection = sortorderprev;

                dataGridViewCurrent.Sort(sortcolcur, (sortordercur == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                dataGridViewCurrent.Columns[sortcolcur.Index].HeaderCell.SortGlyphDirection = sortordercur;
            }
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

        private void VerifyDates()
        {
            if (!EDDConfig.Instance.DateTimeInRangeForGame(customDateTimePickerStart.Value) || !EDDConfig.Instance.DateTimeInRangeForGame(customDateTimePickerEnd.Value))
            {
                customDateTimePickerStart.Checked = customDateTimePickerEnd.Checked = false;
                customDateTimePickerEnd.Value = customDateTimePickerStart.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow);
            }
        }

        private void dataGridViewPrevious_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 1 || e.Column.Index == 2)
                e.SortDataGridViewColumnDate();
            else if (e.Column.Index == 7)
                e.SortDataGridViewColumnNumeric();
        }

        private void dataGridViewCurrent_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 1 || e.Column.Index == 2)
                e.SortDataGridViewColumnDate();
            else if (e.Column.Index == 7)
                e.SortDataGridViewColumnNumeric();

        }
    }
}

