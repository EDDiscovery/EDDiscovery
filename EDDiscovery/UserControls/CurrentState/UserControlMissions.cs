/*
 * Copyright © 2016 - 2021 EDDiscovery development team
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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlMissions : UserControlCommonBase
    {
        private DateTime NextExpiryUTC;

        #region Init

        public UserControlMissions()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "Missions";

            missionListPrevious.SetDateTime(GetSetting("StartDate", DateTime.UtcNow),       // set up by picker
                                            GetSetting("StartDateChecked", false),
                                            GetSetting("EndDate", DateTime.UtcNow),
                                            GetSetting("EndDateChecked", false));
            
            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;

            var enumlist = new Enum[] { EDTx.MissionListUserControl_PcolName, EDTx.MissionListUserControl_pColStart, EDTx.MissionListUserControl_pColEnd, EDTx.MissionListUserControl_pColOrigin, EDTx.MissionListUserControl_pColFromFaction, EDTx.MissionListUserControl_pColDestSys, EDTx.MissionListUserControl_pColTargetFaction, EDTx.MissionListUserControl_pColResult, EDTx.MissionListUserControl_pColInfo, EDTx.MissionListUserControl_labelTo, EDTx.MissionListUserControl_labelSearch };
            
            BaseUtils.Translator.Instance.TranslateControls(missionListCurrent, enumlist);
            BaseUtils.Translator.Instance.TranslateControls(missionListPrevious, enumlist);

            missionListPrevious.SearchTextChanged += () => { Display(); };
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

            DGVLoadColumnLayout(missionListCurrent.dataGridView, "Current");
            DGVLoadColumnLayout(missionListPrevious.dataGridView, "Previous");

            splitContainerMissions.SplitterDistance(GetSetting("Splitter", 0.4));

            missionListPrevious.DateTimeChanged = () => { Display(); };
        }

        public override void Closing()
        {
            missionListCurrent.Closing();
            missionListPrevious.Closing();

            DGVSaveColumnLayout(missionListCurrent.dataGridView, "Current");
            DGVSaveColumnLayout(missionListPrevious.dataGridView, "Previous");

            uctg.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;

            PutSetting("StartDate", missionListPrevious.customDateTimePickerStart.Value);
            PutSetting("StartDateChecked", missionListPrevious.customDateTimePickerStart.Checked);
            PutSetting("EndDate", missionListPrevious.customDateTimePickerEnd.Value);
            PutSetting("EndDateChecked", missionListPrevious.customDateTimePickerEnd.Checked);

            PutSetting("Splitter", splitContainerMissions.GetSplitterDistance());
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
            if (!object.ReferenceEquals(he.MissionList, last_he?.MissionList) || he.EventTimeUTC > NextExpiryUTC)
            {
                last_he = he;
                Display();

                // he can be null
                var ml = hl.MissionListAccumulator.GetAllCurrentMissions(he?.MissionList ?? uint.MaxValue, he?.EventTimeUTC ?? ObjectExtensionsDates.MaxValueUTC());    // will always return an array
                NextExpiryUTC = ml.OrderBy(e => e.MissionEndTime).FirstOrDefault()?.MissionEndTime ?? ObjectExtensionsDates.MaxValueUTC();
            }
        }

        HistoryEntry last_he = null;

        private void Display(HistoryEntry he, HistoryList hl) =>
            Display(he, hl, true);

        private void Display(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            last_he = he;
            Display();

            // he can be null
            var ml = hl.MissionListAccumulator.GetAllCurrentMissions(he?.MissionList ?? uint.MaxValue, he?.EventTimeUTC ?? ObjectExtensionsDates.MaxValueUTC());    // will always return an array
            NextExpiryUTC = ml.OrderBy(e => e.MissionEndTime).FirstOrDefault()?.MissionEndTime ?? ObjectExtensionsDates.MaxValueUTC();
        }

        private void Display()
        {
            List<MissionState> ml = last_he != null ? discoveryform.history.MissionListAccumulator.GetMissionList(last_he.MissionList) : null;

            missionListCurrent.Clear();
            missionListPrevious.Clear();

            if (ml != null)
            {
                DateTime hetime = last_he.EventTimeUTC;

                List<MissionState> mcurrent = MissionListAccumulator.GetAllCurrentMissions(ml,hetime);

                foreach (MissionState ms in mcurrent)
                {
                    missionListCurrent.Add(ms, false, null);
                }

                missionListCurrent.Finish();

                List<MissionState> mprev = MissionListAccumulator.GetAllExpiredMissions(ml,hetime);

                var previousRows = new List<DataGridViewRow>(mprev.Count);

                foreach (MissionState ms in mprev)
                {
                    missionListPrevious.Add(ms, true, missionListPrevious.SearchText);
                }

                missionListPrevious.Finish();
            }
        }

        #endregion



    }
}

