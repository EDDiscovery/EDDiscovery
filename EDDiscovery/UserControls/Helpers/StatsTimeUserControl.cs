/*
 * Copyright © 2016-2022 EDDiscovery development team
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
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class StatsTimeUserControl : UserControl
    {
        public enum TimeModeType
        {
            Summary = 0,
            Day = 1,
            Week = 2,
            Month = 3,
            Year = 4,
            NotSet = 999,
        }

        public event EventHandler TimeModeChanged;      // time or planet/stars changed

        public bool StarMode { get { return checkBoxCustomStars.Checked; } }

        public TimeModeType TimeMode
        {
            get
            {
                return (TimeModeType)comboBoxTimeMode.SelectedIndex;
            }
            set
            {
                if ((int)value < comboBoxTimeMode.Items.Count)
                    comboBoxTimeMode.SelectedIndex = (int)value;
            }
        }

        public StatsTimeUserControl()
        {
            InitializeComponent();
        }

        public void DisplayStarsPlanetSelector(bool on)
        {
            checkBoxCustomStars.Visible = checkBoxCustomPlanets.Visible = on;
        }

        private void UserControlStatsTime_Load(object sender, EventArgs e)
        {
            comboBoxTimeMode.Items.Add("Summary".T(EDTx.StatsTimeUserControl_Summary));
            comboBoxTimeMode.Items.Add("Day".T(EDTx.StatsTimeUserControl_Day));
            comboBoxTimeMode.Items.Add("Week".T(EDTx.StatsTimeUserControl_Week));
            comboBoxTimeMode.Items.Add("Month".T(EDTx.StatsTimeUserControl_Month));
            comboBoxTimeMode.Items.Add("Year".T(EDTx.TravelHistoryFilter_Year));
            comboBoxTimeMode.SelectedIndex = 0;
        }

        private void comboBoxTimeMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            TimeModeChanged?.Invoke(this, e);
        }

        bool preventrentry = false;

        private void checkBoxCustomPlanets_CheckedChanged(object sender, EventArgs e)
        {
            SetStarDraw(!checkBoxCustomPlanets.Checked,e);
        }

        private void checkBoxCustomStars_CheckedChanged(object sender, EventArgs e)
        {
            SetStarDraw(checkBoxCustomStars.Checked,e);
        }

        void SetStarDraw(bool text, EventArgs e)
        {
            if (!preventrentry)
            {
                preventrentry = true;
                checkBoxCustomStars.Checked = text;
                checkBoxCustomPlanets.Checked = !text;
                preventrentry = false;
                TimeModeChanged?.Invoke(this, e);
            }
        }
    }
}
