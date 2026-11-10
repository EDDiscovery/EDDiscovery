/*
 * Copyright © 2016 - 2023 EDDiscovery development team
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
        private int ExpiredDisplayed = 0;

        #region Init

        public UserControlMissions()
        {
            InitializeComponent();
            BaseUtils.TranslatorMkII.Instance.TranslateControls(missionListCurrent);
            BaseUtils.TranslatorMkII.Instance.TranslateControls(missionListPrevious);
            DBBaseName = "Missions";
        }

        protected override void Init()
        {
            
            missionListPrevious.SetDateTime(GetSetting("StartDate", DateTime.UtcNow),       // set up by picker
                                            GetSetting("StartDateChecked", false),
                                            GetSetting("EndDate", DateTime.UtcNow),
                                            GetSetting("EndDateChecked", false));
            
            DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange;

            missionListPrevious.SearchTextChanged += () => { Display(); };
        }

        protected override void LoadLayout()
        {
            missionListCurrent.SetMinimumHeight(Font.ScalePixels(26));
            missionListPrevious.SetMinimumHeight(Font.ScalePixels(26));

            DGVLoadColumnLayout(missionListCurrent.dataGridView, "Current");
            DGVLoadColumnLayout(missionListPrevious.dataGridView, "Previous");

            splitContainerMissions.SplitterDistance(GetSetting("Splitter", 0.4));

            missionListPrevious.DateTimeChanged = () => { Display(); };
        }

        protected override void Closing()
        {
            missionListCurrent.Closing();
            missionListPrevious.Closing();

            DGVSaveColumnLayout(missionListCurrent.dataGridView, "Current");
            DGVSaveColumnLayout(missionListPrevious.dataGridView, "Previous");

            DiscoveryForm.OnHistoryChange -= Discoveryform_OnHistoryChange;

            PutSetting("StartDate", missionListPrevious.customDateTimePickerStart.Value);
            PutSetting("StartDateChecked", missionListPrevious.customDateTimePickerStart.Checked);
            PutSetting("EndDate", missionListPrevious.customDateTimePickerEnd.Value);
            PutSetting("EndDateChecked", missionListPrevious.customDateTimePickerEnd.Checked);

            PutSetting("Splitter", splitContainerMissions.GetSplitterDistance());
        }

        #endregion

        #region Display

        protected override void InitialDisplay()
        {
            RequestPanelOperation(this, new UserControlCommonBase.RequestHistoryGridPos());     //request an update 
        }
        private void Discoveryform_OnHistoryChange()
        {
            missionListPrevious.VerifyDates();
        }

        public override void ReceiveHistoryEntry(HistoryEntry he)
        {
           // System.Diagnostics.Debug.WriteLine($"Mission List Clicked {he.EventTimeUTC} {he.MissionList} vs {last_he?.MissionList}, next expiry time {NextExpiryUTC}");

            var ml = DiscoveryForm.History.MissionListAccumulator.GetMissionListInProgress(he.MissionList);

            if (he.MissionList != last_he?.MissionList || he.EventTimeUTC > NextExpiryUTC || ml.Where(x=>!x.InProgressDateTime(he.EventTimeUTC)).Count() != ExpiredDisplayed)
            {
                last_he = he;
                Display();

                NextExpiryUTC = ml.OrderBy(e => e.MissionEndTime).FirstOrDefault()?.MissionEndTime ?? ObjectExtensionsDates.MaxValueUTC();
                //System.Diagnostics.Debug.WriteLine($"Mission List Redisplay {he.EventTimeUTC} {he.MissionList} vs {last_he?.MissionList}, next expiry time {NextExpiryUTC} Expired {ExpiredDisplayed}");
            }
        }

        HistoryEntry last_he = null;
    
        private void Display()
        {
            List<MissionState> ml = last_he != null ? DiscoveryForm.History.MissionListAccumulator.GetMissionListOrdered(last_he.MissionList) : null;

            missionListCurrent.Start();
            missionListPrevious.Start();

            ExpiredDisplayed = 0;

            if (ml != null)
            {
                DateTime hetime = last_he.EventTimeUTC;

                foreach (MissionState ms in ml.Where(x=>x.InProgress))
                {
                    bool notexpired = ms.InProgressDateTime(hetime);
                    ExpiredDisplayed += notexpired ? 0 : 1;
                    missionListCurrent.Add(ms, false, notexpired, null);
                }

                missionListCurrent.CompletedFill();

                foreach (MissionState ms in ml.Where(x=>!x.InProgress))
                {
                    missionListPrevious.Add(ms, true, false, missionListPrevious.SearchText);
                }

                missionListPrevious.CompletedFill();
            }

            missionListCurrent.Finish();
            missionListPrevious.Finish();
        }

        #endregion



    }
}

