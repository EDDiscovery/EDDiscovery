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
        DateTime curmonthnokind;
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
            curmonthnokind = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDate(DbDateSave, firstofmonth);

            string daynames = "Sun;Mon;Tue;Wed;Thu;Fri;Sat".T(EDTx.CaptainsLogDiary_Daysofweek);
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
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDate(DbDateSave, curmonthnokind);
        }

        private void CaptainsLogDiary_Resize(object sender, EventArgs e)
        {
            if (layoutdone)
                Display();

        }

        private void Display()
        {
            int buttop = Math.Max(16,this.Height/8);

            int daysinmonth = CultureInfo.InvariantCulture.Calendar.GetDaysInMonth(curmonthnokind.Year, curmonthnokind.Month);
            int dayofweek = (int)curmonthnokind.DayOfWeek;
            int lastdayindex = dayofweek + daysinmonth - 1;
            int numberrows = lastdayindex / 7 + 1;

            int hspacing = (this.Width)/8;
            int butwidth = hspacing * 3 / 4;
            int vspacing = (this.Height - buttop) / numberrows;
            int butheight = vspacing * 3 / 4;

            int butleft = hspacing / 2;
            int leftrighthoffset = hspacing/2-4;

            float pixelh = discoveryform.theme.GetFont.GetHeight();     // get some idea of the size of the font

            // tried disposing of old font but that crashes since the button is still assigned to it..
            //System.Diagnostics.Debug.WriteLine("Button width " + butwidth + " vs" + pixelh);
            calfont = discoveryform.theme.GetScaledFont((float)Math.Min(butwidth,butheight) / pixelh * 0.5f);
            labfont = discoveryform.theme.GetScaledFont((float)Math.Min(butwidth, butheight) / pixelh * 0.4f);

            for (int i = 0; i < 7; i++)
            {
                daynameslabels[i].Size = new Size(butwidth, buttop-4);
                daynameslabels[i].Location = new Point(butleft + hspacing * i, 4);
                daynameslabels[i].Font = labfont;
            }

            buttonLeft.Size = buttonRight.Size = new Size(butwidth / 2, vspacing * 2 + butheight);
            buttonLeft.Location = new Point(butleft - leftrighthoffset, buttop + vspacing * 1);
            buttonRight.Location = new Point(butleft + 6 * hspacing + butwidth + leftrighthoffset-buttonRight.Width, buttonLeft.Top);

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

                    // so nasty. Make up start/end markers with the correct kind (UTC or local) then go to universal
                    DateTime start = new DateTime(curmonthnokind.Year, curmonthnokind.Month, i + 1, 0, 0, 0, EDDConfig.Instance.DisplayTimeLocal ? DateTimeKind.Local : DateTimeKind.Utc);
                    DateTime end = new DateTime(curmonthnokind.Year, curmonthnokind.Month, i + 1, 23, 59, 59, EDDConfig.Instance.DisplayTimeLocal ? DateTimeKind.Local : DateTimeKind.Utc);
                    start = start.ToUniversalTime();
                    end = end.ToUniversalTime();

                    int noentries = GlobalCaptainsLogList.Instance.FindUTC(start,end, EDCommander.CurrentCmdrID).Length;    // UTC comparision..  CL class is UTC

                    daybuttons[i].Tag = noentries;

                    if (discoveryform.theme.ButtonStyle == EDDTheme.ButtonStyles[0])  // system buttons - can't colour them
                    {
                        daybuttons[i].Text = (i + 1).ToStringInvariant() + (noentries > 0 ? "*" : "");
                    }
                    else
                    {
                        daybuttons[i].Text = (i + 1).ToStringInvariant();
                        daybuttons[i].BackColor = noentries > 0 ? discoveryform.theme.SPanelColor : discoveryform.theme.ButtonBackColor;
                        daybuttons[i].ForeColor = noentries > 0 ? discoveryform.theme.TextBackColor : discoveryform.theme.ButtonTextColor;

                        daybuttons[i].FlatAppearance.MouseOverBackColor = discoveryform.theme.SPanelColor.Multiply(1.1f);
                    }

                    daybuttons[i].Visible = true;
                }
                else
                    daybuttons[i].Visible = false;
            }

            SetControlText(EDDConfig.Instance.ConvertTimeToSelectedNoKind(curmonthnokind).ToString("yyyy - MM"));
            //System.Diagnostics.Debug.WriteLine("Editing month " + curmonthnokind);
        }

        private void buttonRight_Click(object sender, EventArgs e)
        {
            curmonthnokind = curmonthnokind.AddMonths(1);
            Display();
        }

        private void buttonLeft_Click(object sender, EventArgs e)
        {
            curmonthnokind = curmonthnokind.AddMonths(-1);
            Display();

        }

        private void DayClick(object sender, EventArgs e)       // send a button click up
        {
            ExtendedControls.ExtButton b = sender as ExtendedControls.ExtButton;
            int dayno = b.Text.Replace("*","").InvariantParseInt(-1);
            ClickedonDate?.Invoke(new DateTime(curmonthnokind.Year, curmonthnokind.Month, dayno), (int)b.Tag == 0);
        }

    }
}
