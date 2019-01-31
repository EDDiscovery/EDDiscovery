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

using EDDiscovery.Forms;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.EDSM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlCaptainsLog : UserControlCommonBase
    {
        private string DbSelectedSave { get { return DBName("UCCaptainsLogTop", "Tab"); } }

        public UserControlCaptainsLog()
        {
            InitializeComponent();
        }

        DateTime? gotodate = null;
        bool createnew = false;

        public override void Init()
        {
            tabStrip.ImageList = new Image[] { EDDiscovery.Icons.Controls.CaptainsLog_Diary, EDDiscovery.Icons.Controls.CaptainsLog_Entries};
            tabStrip.TextList = new string[] { "Diary".Tx(this), "Entries".Tx(this) };
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
                uccb.LoadLayout();
                discoveryform.theme.ApplyToControls(tab);       // theme BEFORE initial display on purpose
                uccb.InitialDisplay();

                if (uccb is CaptainsLogDiary)
                {
                    (uccb as CaptainsLogDiary).ClickedonDate = (d,b) =>
                    {
                        gotodate = d;
                        createnew = b;
                        tabStrip.SelectedIndex = 1;
                    };
                }
                else if ( uccb is CaptainsLogEntries )
                {
                    if ( gotodate.HasValue )
                    {
                        var clentries = uccb as CaptainsLogEntries;
                        System.Diagnostics.Debug.WriteLine("Goto " + gotodate);
                        clentries.SelectDate(gotodate.Value,createnew);
                        gotodate = null;
                    }
                }
            };

            tabStrip.OnRemoving += (tab, ctrl) =>
            {
                UserControlCommonBase uccb = ctrl as UserControlCommonBase;
                uccb.CloseDown();
            };
        }

        public override void InitialDisplay()
        {
            int seltab = SQLiteConnectionUser.GetSettingInt(DbSelectedSave, 0);
            seltab = seltab.Range(0, tabStrip.ImageList.Length - 1);
            tabStrip.SelectedIndex = seltab;
        }

        public override void Closing()
        {
            SQLiteConnectionUser.PutSettingInt(DbSelectedSave, tabStrip.SelectedIndex);
            tabStrip.Close();
        }
    }
}
