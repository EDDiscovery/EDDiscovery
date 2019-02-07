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
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    // CL UCs use the UCCB template BUT are not directly inserted into the normal panels.. they are inserted into the CL UCCB
    // Make sure DB saving has unique names.. they all share the same displayno.

    public partial class CaptainsLogDiary : UserControlCommonBase
    {
        public Action<DateTime,bool> ClickedonDate;

        private string DbDateSave { get { return DBName("CaptainsLogPanel", "DiaryMonth"); } }
        DateTime curmonth;
        ExtendedControls.ExtButton[] daybuttons = new ExtendedControls.ExtButton[31];
        Label[] daynameslabels = new Label[7];
        bool layoutdone = false;
        Font calfont;
        Font labfont;

        public CaptainsLogDiary()
        {
            InitializeComponent();
        }

        public override void Init()
        {
        }

        public override void LoadLayout()
        {
            DateTime firstofmonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            curmonth = SQLiteConnectionUser.GetSettingDate(DbDateSave, firstofmonth);

            string daynames = "Sun;Mon;Tue;Wed;Thu;Fri;Sat".Tx("Daysofweek");
            string[] daynamesplit = daynames.Split(';');

            for (int i = 0; i < 7; i++)
            {
                Label lb = new Label();
                lb.Text = daynamesplit[i];
                lb.TextAlign = ContentAlignment.MiddleCenter;
                daynameslabels[i] = lb;
                Controls.Add(lb);
            }

            for (int i = 1; i <= 31; i++)
            {
                ExtendedControls.ExtButton b = new ExtendedControls.ExtButton();
                b.Click += DayClick;
                Controls.Add(b);
                daybuttons[i - 1] = b;
            }

            layoutdone = true;
        }

        public override void InitialDisplay()
        {
            Display();
        }

        public override void Closing()
        {
            SQLiteConnectionUser.PutSettingDate(DbDateSave, curmonth);
        }

        private void CaptainsLogDiary_Resize(object sender, EventArgs e)
        {
            if (layoutdone)
                Display();

        }

        private void Display()
        {
            int buttop = Math.Max(16,this.Height/8);

            int hspacing = (this.Width)/8;
            int butwidth = hspacing * 3 / 4;
            int vspacing = (this.Height - buttop) / 5;
            int butheight = vspacing * 3 / 4;

            int butleft = hspacing / 2;
            int leftrighthoffset = hspacing/2-4;

            calfont = discoveryform.theme.GetFontAtSize(Math.Max(1,butwidth / 4));      // tried disposing of old font but that crashes since the button is still assigned to it..
            labfont = discoveryform.theme.GetFontAtSize(Math.Min(Math.Max(butwidth / 5,1),24));

            for (int i = 0; i < 7; i++)
            {
                daynameslabels[i].Size = new Size(butwidth, buttop-4);
                daynameslabels[i].Location = new Point(butleft + hspacing * i, 4);
                daynameslabels[i].Font = labfont;
            }

            buttonLeft.Size = buttonRight.Size = new Size(butwidth / 2, vspacing * 2 + butheight);
            buttonLeft.Location = new Point(butleft - leftrighthoffset, buttop + vspacing * 1);
            buttonRight.Location = new Point(butleft + 6 * hspacing + butwidth + leftrighthoffset-buttonRight.Width, buttonLeft.Top);

            int daysinmonth = CultureInfo.InvariantCulture.Calendar.GetDaysInMonth(curmonth.Year, curmonth.Month);
            int dayofweek = (int)curmonth.DayOfWeek;
            int vpos = buttop;

            for (int i = 0; i < 31; i++)
            {
                if (i < daysinmonth )
                {
                    daybuttons[i].Size = new Size(butwidth, butheight);
                    daybuttons[i].Location = new Point(butleft + hspacing * dayofweek, vpos);
                    daybuttons[i].Font = calfont;
                    dayofweek++;
                    if (dayofweek >= 7)
                    {
                        vpos += vspacing;
                        dayofweek = 0;
                    }

                    // display is in local or utc time, dependent on Config.  So we need to compare local or utc times
                    int noentries = GlobalCaptainsLogList.Instance.Find(new DateTime(curmonth.Year, curmonth.Month, i + 1), 
                                                                        new DateTime(curmonth.Year, curmonth.Month, i + 1, 23, 59, 59), 
                                                                        EDDConfig.Instance.DisplayUTC , EDCommander.CurrentCmdrID).Length;

                    daybuttons[i].Tag = noentries;

                    if (discoveryform.theme.ButtonStyle == EDDTheme.ButtonStyles[0])  // system buttons - can't colour them
                    {
                        daybuttons[i].Text = (i + 1).ToStringInvariant() + (noentries > 0 ? "*" : "");
                    }
                    else
                    {
                        daybuttons[i].Text = (i + 1).ToStringInvariant();
                        daybuttons[i].BackColor = noentries > 0 ? discoveryform.theme.SPanelColor : discoveryform.theme.ButtonBackColor;
                        daybuttons[i].FlatAppearance.MouseOverBackColor = discoveryform.theme.SPanelColor.Multiply(1.1f);
                    }

                    daybuttons[i].Visible = true;
                }
                else
                    daybuttons[i].Visible = false;
            }

            SetControlText(curmonth.ToString("yyyy - MM"));
        }

        private void buttonRight_Click(object sender, EventArgs e)
        {
            curmonth = curmonth.AddMonths(1);
            Display();
        }

        private void buttonLeft_Click(object sender, EventArgs e)
        {
            curmonth = curmonth.AddMonths(-1);
            Display();

        }

        private void DayClick(object sender, EventArgs e)       // send a button click up
        {
            ExtendedControls.ExtButton b = sender as ExtendedControls.ExtButton;
            int dayno = b.Text.InvariantParseInt(-1);
            ClickedonDate?.Invoke(new DateTime(curmonth.Year, curmonth.Month, dayno), (int)b.Tag == 0);
        }

    }
}
