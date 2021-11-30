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
using System.Drawing;

namespace EDDiscovery.UserControls
{
    public partial class UserControlCaptainsLog : UserControlCommonBase
    {
        private string dbSelectedSave = "Tab";

        public UserControlCaptainsLog()
        {
            InitializeComponent();
        }

        DateTime? gotodatestart = null;
        DateTime? gotodateend = null;
        bool createnew = false;

        public override void Init()
        {
            DBBaseName = "CaptainsLog";

            tabStrip.ImageList = new Image[] { EDDiscovery.Icons.Controls.Diary, EDDiscovery.Icons.Controls.Entries};
            tabStrip.TextList = new string[] { "Diary".T(EDTx.UserControlCaptainsLog_Diary), "Entries".T(EDTx.UserControlCaptainsLog_Entries) };
            tabStrip.TagList = new Type[] { typeof(CaptainsLogDiary), typeof(CaptainsLogEntries) };

            tabStrip.OnCreateTab += (tab, si) =>
            {
                UserControlCommonBase uccb = (UserControlCommonBase)Activator.CreateInstance((Type)tab.TagList[si], null);
                uccb.Name = tab.TextList[si];
                return uccb;
            };

            tabStrip.OnPostCreateTab += (tab, ctrl, si) =>
            {
                UserControlCommonBase uccb = ctrl as UserControlCommonBase;
                uccb.Init(discoveryform, displaynumber);
                discoveryform.theme.ApplyStd(uccb);       // contract, in UCCB, states theming is between init and load
                uccb.LoadLayout();
                uccb.InitialDisplay();

                if (uccb is CaptainsLogDiary)
                {
                    (uccb as CaptainsLogDiary).ClickedonDate = (d,b) =>
                    {
                        gotodatestart = gotodateend = d;
                        createnew = b;
                        tabStrip.SelectedIndex = 1;
                    };
                }
                else if ( uccb is CaptainsLogEntries )
                {
                    if ( gotodatestart.HasValue )
                    {
                        var clentries = uccb as CaptainsLogEntries;
                        clentries.SelectDate(gotodatestart.Value,gotodateend.Value,createnew);
                        gotodatestart = gotodateend = null;
                    }
                }
            };

            tabStrip.OnRemoving += (tab, ctrl) =>
            {
                UserControlCommonBase uccb = ctrl as UserControlCommonBase;
                uccb.CloseDown();
            };

            tabStrip.OnControlTextClick += (tab) =>
            {
                CaptainsLogDiary cld = tabStrip.CurrentControl as CaptainsLogDiary;
                if ( cld != null )
                {
                    gotodatestart = cld.CurrentMonth;
                    gotodateend = new DateTime(cld.CurrentMonth.Year, cld.CurrentMonth.Month, DateTime.DaysInMonth(cld.CurrentMonth.Year, cld.CurrentMonth.Month));
                    createnew = false;
                    tabStrip.SelectedIndex = 1;
                }
            };
        }

        public override void InitialDisplay()
        {
            int seltab = GetSetting(dbSelectedSave, 0);
            seltab = seltab.Range(0, tabStrip.ImageList.Length - 1);
            tabStrip.SelectedIndex = seltab;
        }

        public override void Closing()
        {
            PutSetting(dbSelectedSave, tabStrip.SelectedIndex);
            tabStrip.Close();
        }
    }
}
