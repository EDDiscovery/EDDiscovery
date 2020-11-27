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
        }

        public override void Init()
        {
            missionListPrevious.SetDateTime(EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDate(DbStartDate, DateTime.UtcNow),
                                            EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbStartDateChecked, false),
                                            EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDate(DbEndDate, DateTime.UtcNow),
                                            EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbEndDateChecked, false));
            
            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;

            BaseUtils.Translator.Instance.Translate(missionListCurrent);
            BaseUtils.Translator.Instance.Translate(missionListPrevious);
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

            missionListCurrent.SetMinimumHeight(Font.ScalePixels(26));
            missionListPrevious.SetMinimumHeight(Font.ScalePixels(26));

            DGVLoadColumnLayout(missionListCurrent.dataGridView, DbColumnSaveCurrent);
            DGVLoadColumnLayout(missionListPrevious.dataGridView, DbColumnSavePrevious);

            splitContainerMissions.SplitterDistance(EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(DbSplitter, 0.4));

            missionListPrevious.DateTimeChanged = () => { Display(); };
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(missionListCurrent.dataGridView, DbColumnSaveCurrent);
            DGVSaveColumnLayout(missionListPrevious.dataGridView, DbColumnSavePrevious);

            uctg.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;

            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDate(DbStartDate, EDDiscoveryForm.EDDConfig.ConvertTimeToUTCFromSelected(missionListPrevious.customDateTimePickerStart.Value));
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDate(DbEndDate, EDDiscoveryForm.EDDConfig.ConvertTimeToUTCFromSelected(missionListPrevious.customDateTimePickerEnd.Value));

            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbStartDateChecked, missionListPrevious.customDateTimePickerStart.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbEndDateChecked, missionListPrevious.customDateTimePickerEnd.Checked);

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
            missionListPrevious.VerifyDates();
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

            missionListCurrent.Clear();
            missionListPrevious.Clear();

            if (ml != null)
            {
                DateTime hetime = last_he.EventTimeUTC;

                List<MissionState> mcurrent = ml.GetAllCurrentMissions(hetime);

                foreach (MissionState ms in mcurrent)
                {
                    missionListCurrent.Add(ms, false);
                }

                missionListCurrent.Finish();

                List<MissionState> mprev = ml.GetAllExpiredMissions(hetime);

                var previousRows = new List<DataGridViewRow>(mprev.Count);

                foreach (MissionState ms in mprev)
                {
                    missionListPrevious.Add(ms, true);
                }

                missionListPrevious.Finish();
            }
        }

        #endregion



    }
}

