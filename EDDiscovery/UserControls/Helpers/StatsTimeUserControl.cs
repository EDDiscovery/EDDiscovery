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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class StatsTimeUserControl : UserControl
    {
        public enum TimeModeType
        {
            Summary,
            Day,
            Week,
            Month,
            Custom
        }

        public enum DrawModeType
        {
            Text,
            Graph
        }

        public event EventHandler TimeModeChanged;      // time or planet/stars changed
        public event EventHandler DrawModeChanged;

        public DrawModeType DrawMode { get { return (checkBoxCustomText.Checked) ? DrawModeType.Text : DrawModeType.Graph; } }
        public bool StarPlanetMode { get { return checkBoxCustomStars.Checked; } }

        public TimeModeType TimeMode
        {
            get
            {
                if (comboBoxTimeMode.SelectedIndex == 0)
                    return TimeModeType.Summary;
                else if (comboBoxTimeMode.SelectedIndex == 1)
                    return TimeModeType.Day;
                else if (comboBoxTimeMode.SelectedIndex == 2)
                    return TimeModeType.Week;
                else if (comboBoxTimeMode.SelectedIndex == 3)
                    return TimeModeType.Month;
                else if (comboBoxTimeMode.SelectedIndex == 4)
                    return TimeModeType.Custom;
                else
                    return TimeModeType.Summary;
            }

            set
            {
                if (comboBoxTimeMode.Items.Count > 0)
                {
                    switch (value)
                    {
                        case TimeModeType.Summary:
                            comboBoxTimeMode.SelectedIndex = 0;
                            break;
                        case TimeModeType.Day:
                            comboBoxTimeMode.SelectedIndex = 1;
                            break;
                        case TimeModeType.Week:
                            comboBoxTimeMode.SelectedIndex = 2;
                            break;
                        case TimeModeType.Month:
                            comboBoxTimeMode.SelectedIndex = 3;
                            break;
                        case TimeModeType.Custom:
                            comboBoxTimeMode.SelectedIndex = 4;
                            break;

                        default:
                            comboBoxTimeMode.SelectedIndex = 0;
                            break;
                    }
                }
            }
        }

        public StatsTimeUserControl()
        {
            InitializeComponent();

            CustomDateTimePickerFrom.Value = DateTime.Today.AddMonths(-1);
            CustomDateTimePickerTo.Value = DateTime.Today;
            CustomDateTimePickerFrom.CustomFormat = "yyyy-MM-dd";
            CustomDateTimePickerTo.CustomFormat = "yyyy-MM-dd";
            PositionControls();
        }

        public void EnableDrawModeSelector()        // normally disabled.
        {
            checkBoxCustomText.Visible = checkBoxCustomGraph.Visible = true;
        }

        public void EnableDisplayStarsPlanetSelector()
        {
            checkBoxCustomStars.Visible = checkBoxCustomPlanets.Visible = true;
            PositionControls();
        }

        private void PositionControls()
        {
            bool pviz = CustomDateTimePickerFrom.Visible;
            bool pstars = checkBoxCustomStars.Visible;

            int widthbutton = checkBoxCustomStars.Width;

            int left = CustomDateTimePickerFrom.Left;
            int right = CustomDateTimePickerTo.Right+8;
            int starpos = pviz ? right : left;
            int textpos = starpos + (pstars ? widthbutton * 2 + 8 : 0);

            checkBoxCustomStars.Left = starpos;
            checkBoxCustomPlanets.Left = starpos + widthbutton + 4;
            checkBoxCustomText.Left = textpos;
            checkBoxCustomGraph.Left = textpos+ widthbutton + 4;
            //System.Diagnostics.Debug.WriteLine("Picker " + pviz + " pstars" + pstars);
            //System.Diagnostics.Debug.WriteLine("Starpos " + starpos + " " + textpos);
        }

        private void UserControlStatsTime_Load(object sender, EventArgs e)
        {
            comboBoxTimeMode.Items.Add("Summary".Tx(this));
            comboBoxTimeMode.Items.Add("Day".Tx(this));
            comboBoxTimeMode.Items.Add("Week".Tx(this));
            comboBoxTimeMode.Items.Add("Month".Tx(this));
            comboBoxTimeMode.Items.Add("Custom".Tx(this));
            comboBoxTimeMode.SelectedIndex = 0;
        }

        private void comboBoxTimeMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool showtime = comboBoxTimeMode.SelectedIndex == 4; // Custom'
            CustomDateTimePickerFrom.Visible = showtime;
            CustomDateTimePickerTo.Visible = showtime;
            PositionControls();
            TimeModeChanged?.Invoke(this, e);
        }

        bool preventrentry = false;

        private void checkBoxCustomText_CheckedChanged(object sender, EventArgs e)
        {
            SetTextDraw(checkBoxCustomText.Checked,e);
        }

        private void checkBoxCustomGraph_CheckedChanged(object sender, EventArgs e)
        {
            SetTextDraw(!checkBoxCustomGraph.Checked,e);
        }

        void SetTextDraw(bool text, EventArgs e)
        {
            if (!preventrentry)
            {
                preventrentry = true;
                checkBoxCustomGraph.Checked = !text;
                checkBoxCustomText.Checked = text;
                preventrentry = false;
                DrawModeChanged?.Invoke(this, e);
            }
        }

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

        private void customDateTimePickerFrom_ValueChanged(object sender, EventArgs e)
        {
            TimeModeChanged?.Invoke(this, e);
        }

        private void customDateTimePickerTo_ValueChanged(object sender, EventArgs e)
        {
            TimeModeChanged?.Invoke(this, e);
        }
    }
}
