/*
 * Copyright © 2018-2022 EDDiscovery development team
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
using EliteDangerousCore.DB;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    // CL UCs use the UCCB template BUT are not directly inserted into the normal panels.. they are inserted into the CL UCCB
    // Make sure DB saving has unique names.. they all share the same displayno.

    public partial class CaptainsLogDiary : UserControlCommonBase
    {
        public Action<DateTime, bool> ClickedOnDate { get; set; }       // DateTime is in UTC
        public DateTime CurrentMonthFirstDayUTC { get; private set; }      // this is day 1 of the month, in utc

        private string dbDateSave = "DiaryMonth";
        private ExtendedControls.ExtButton[] daybuttons = new ExtendedControls.ExtButton[31];
        private Label[] daynameslabels = new Label[7];
        private bool layoutdone = false;
        private Font calfont;
        private Font labfont;

        public CaptainsLogDiary()
        {
            InitializeComponent();
        }

        protected override void Init()
        {
            DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange;
        }

        protected override void LoadLayout()
        {
            DBBaseName = "CaptainsLogPanel";

            // get setting in utc
            CurrentMonthFirstDayUTC = GetSetting(dbDateSave, DateTime.UtcNow.StartOfMonth());     

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

        protected override void InitialDisplay()
        {
            Display();
        }

        private void Discoveryform_OnHistoryChange()
        {
            Display();
        }


        protected override void Closing()
        {
            PutSetting(dbDateSave, CurrentMonthFirstDayUTC);
            DiscoveryForm.OnHistoryChange -= Discoveryform_OnHistoryChange;
        }

        private void CaptainsLogDiary_Resize(object sender, EventArgs e)
        {
            if (layoutdone)
                Display();

        }

        private void Display()
        {
            int buttop = Math.Max(16,this.Height/8);

            int daysinmonth = CultureInfo.InvariantCulture.Calendar.GetDaysInMonth(CurrentMonthFirstDayUTC.Year, CurrentMonthFirstDayUTC.Month);
            int firstdayofmonth = (int)CurrentMonthFirstDayUTC.DayOfWeek;            // CurrentMonth is at 1/1 at 0:0, so first start day in month. Sunday is zero
            int lastdayindex = firstdayofmonth + daysinmonth - 1;
            int numberrows = lastdayindex / 7 + 1;

            int hspacing = (this.Width)/8;
            int butwidth = hspacing * 3 / 4;
            int vspacing = (this.Height - buttop) / numberrows;
            int butheight = vspacing * 3 / 4;

            int butleft = hspacing / 2;
            int leftrighthoffset = hspacing/2-4;

            float pixelh = ExtendedControls.Theme.Current.GetFont.GetHeight();     // get some idea of the size of the font

            // tried disposing of old font but that crashes since the button is still assigned to it..
            //System.Diagnostics.Debug.WriteLine("Button width " + butwidth + " vs" + pixelh);
            calfont = ExtendedControls.Theme.Current.GetScaledFont((float)Math.Min(butwidth,butheight) / pixelh * 0.5f);
            labfont = ExtendedControls.Theme.Current.GetScaledFont((float)Math.Min(butwidth, butheight) / pixelh * 0.4f);

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
                    daybuttons[i].Location = new Point(butleft + hspacing * firstdayofmonth, vpos);
                    daybuttons[i].Font = calfont;
                    firstdayofmonth++;
                    if (firstdayofmonth >= 7)
                    {
                        vpos += vspacing;
                        firstdayofmonth = 0;
                    }

                    // so nasty. Make up start/end markers with the correct kind (UTC or local) then go to universal
                    DateTime start = CurrentMonthFirstDayUTC.AddDays(i);
                    DateTime end = CurrentMonthFirstDayUTC.AddDays(i).EndOfDay();
                    int noentries = GlobalCaptainsLogList.Instance.FindUTC(start,end, EDCommander.CurrentCmdrID).Length;    // UTC comparision..  CL class is UTC
                    //System.Diagnostics.Debug.WriteLine($"CL Check {start}-{end} = {noentries}");

                    daybuttons[i].Tag = noentries;

                    if (ExtendedControls.Theme.Current.ButtonStyle == ExtendedControls.Theme.ButtonStyles[0])  // system buttons - can't colour them
                    {
                        daybuttons[i].Text = (i + 1).ToStringInvariant() + (noentries > 0 ? "*" : "");
                    }
                    else
                    {
                        daybuttons[i].Text = (i + 1).ToStringInvariant();
                        daybuttons[i].BackColor = noentries > 0 ? ExtendedControls.Theme.Current.SPanelColor : ExtendedControls.Theme.Current.ButtonBackColor;
                        daybuttons[i].ForeColor = noentries > 0 ? ExtendedControls.Theme.Current.TextBlockBackColor : ExtendedControls.Theme.Current.ButtonTextColor;

                        daybuttons[i].FlatAppearance.MouseOverBackColor = ExtendedControls.Theme.Current.SPanelColor.Multiply(1.1f);
                    }

                    daybuttons[i].Visible = true;
                }
                else
                    daybuttons[i].Visible = false;
            }

            SetControlText(EDDConfig.Instance.ConvertTimeToSelectedFromUTC(CurrentMonthFirstDayUTC).ToString("yyyy - MM"));
            //System.Diagnostics.Debug.WriteLine("Editing month " + curmonthnokind);
        }

        private void buttonRight_Click(object sender, EventArgs e)
        {
            CurrentMonthFirstDayUTC = CurrentMonthFirstDayUTC.AddMonths(1);
            Display();
        }

        private void buttonLeft_Click(object sender, EventArgs e)
        {
            CurrentMonthFirstDayUTC = CurrentMonthFirstDayUTC.AddMonths(-1);
            Display();

        }

        private void DayClick(object sender, EventArgs e)       // send a button click up
        {
            ExtendedControls.ExtButton b = sender as ExtendedControls.ExtButton;
            int dayno = b.Text.Replace("*","").InvariantParseInt(-1);
            DateTime clickdate = CurrentMonthFirstDayUTC.AddDays(dayno-1);
            ClickedOnDate?.Invoke(clickdate, (int)b.Tag == 0);
        }

    }
}
