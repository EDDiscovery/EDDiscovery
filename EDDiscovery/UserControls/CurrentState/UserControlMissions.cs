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
        private string DbColumnSaveCurrent { get { return DBName("MissionsGridCurrent" ,  "DGVCol"); } }
        private string DbColumnSavePrevious { get { return DBName("MissionsGridPrevious" ,  "DGVCol"); } }
        private string DbStartDate { get { return DBName("MissionsStartDate" ); } }
        private string DbEndDate { get { return DBName("MissionsEndDate" ); } }
        private string DbStartDateChecked { get { return DBName("MissionsStartDateCheck" ); } }
        private string DbEndDateChecked { get { return DBName("MissionsEndDateCheck" ); } }
        private string DbSplitter { get { return DBName("MissionsSplitter") ; } }

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

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);
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

        private void Display(HistoryEntry he, HistoryList hl) =>
            Display(he, hl, true);

        private void Display(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            last_he = he;
            Display();
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

                int count = mcurrent.Count();

                cColName.HeaderText = (count > 0) ? (count.ToStringInvariant() + (count > 1 ? " Missions".Tx(this,"MPlural") : " Mission".Tx(this,"MSingular"))) : "Name".Tx(this,"Name");
                cColValue.HeaderText = (totalReward != 0) ? string.Format("Value (cr):\n{0:N0}".Tx(this,"Value") ,totalReward) : "Value (cr)".Tx(this,"ValueN");

                //                cColValue.HeaderText = (count>0) ? (count.ToStringInvariant() + (count > 1 ? " Missions" : " Mission") + (totalReward>0 ? $", {totalReward:N0}" : "")) : "Value";

                List<MissionState> mprev = ml.GetAllExpiredMissions(hetime);

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
                labelValue.Text = "Value: ".Tx(this,"ValueC") + value.ToStringInvariant() + " C:" + completed.ToStringInvariant() + " A:" + abandonded.ToStringInvariant() + " F:" + failed.ToStringInvariant();

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

