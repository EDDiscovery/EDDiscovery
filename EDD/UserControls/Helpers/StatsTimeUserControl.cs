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
using EliteDangerousCore;
using System;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class StatsTimeUserControl : UserControl
    {
        public Action<JournalStatsInfo.TimeModeType, JournalStatsInfo.TimeModeType> TimeModeChanged;          // time mode changed
        public Action StarPlanetModeChanged;                                // Star planet changed

        public bool StarMode { get; private set; } = false;

        public JournalStatsInfo.TimeModeType TimeMode
        {
            get
            {
                return (JournalStatsInfo.TimeModeType)comboBoxTimeMode.SelectedIndex;
            }
            set
            {
                if ((int)value < comboBoxTimeMode.Items.Count)
                    comboBoxTimeMode.SelectedIndex = (int)value;
            }
        }

        public JournalStatsInfo.TimeModeType PreviousTimeMode { get; private set; }

        public StatsTimeUserControl()
        {
            InitializeComponent();
            comboBoxTimeMode.Items.Add("Summary".Tx());
            comboBoxTimeMode.Items.Add("Day".Tx());
            comboBoxTimeMode.Items.Add("Week".Tx());
            comboBoxTimeMode.Items.Add("Month".Tx());
            comboBoxTimeMode.Items.Add("Year".Tx());
            PreviousTimeMode = JournalStatsInfo.TimeModeType.Summary;
        }

        public void DisplayStarsPlanetSelector(bool on)
        {
            extButtonPlanet.Visible = extButtonStar.Visible = true;
            SetEnables();
        }

        private void SetEnables()
        {
            extButtonPlanet.Enabled = StarMode;
            extButtonStar.Enabled = !StarMode;
        }

        private void comboBoxTimeMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            TimeModeChanged?.Invoke(PreviousTimeMode, TimeMode);
            PreviousTimeMode = TimeMode;
        }

        private void extButtonPlanet_Click(object sender, EventArgs e)
        {
            StarMode = false;
            SetEnables();
            StarPlanetModeChanged?.Invoke();
        }

        private void extButtonStar_Click(object sender, EventArgs e)
        {
            StarMode = true;
            SetEnables();
            StarPlanetModeChanged?.Invoke();
        }
    }
}
