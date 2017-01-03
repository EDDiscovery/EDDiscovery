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
    public enum UserControlStatsTimeModeEnum
    {
        Summary,
        Day,
        Week,
        Month
    }

    public enum UserControlStatsDrawModeEnum
    {
        Text,
        Graph
    }


    public partial class UserControlStatsTime : UserControl
    {
        public event EventHandler TimeModeChanged;
        public event EventHandler DrawModeChanged;


        public UserControlStatsTime()
        {
            InitializeComponent();

        }

        
        #region "properties"
        public UserControlStatsTimeModeEnum TimeMode
        {
            get
            {
                if (comboBoxTimeMode.SelectedIndex == 0)
                    return UserControlStatsTimeModeEnum.Summary;
                else if (comboBoxTimeMode.SelectedIndex == 1)
                    return UserControlStatsTimeModeEnum.Day;
                else if (comboBoxTimeMode.SelectedIndex == 2)
                    return UserControlStatsTimeModeEnum.Week;
                if (comboBoxTimeMode.SelectedIndex == 3)
                    return UserControlStatsTimeModeEnum.Month;
                else
                    return UserControlStatsTimeModeEnum.Summary;
            }

            set
            {
                if (comboBoxTimeMode.Items.Count > 0)
                {
                    switch (value)
                    {
                        case UserControlStatsTimeModeEnum.Summary:
                            comboBoxTimeMode.SelectedIndex = 0;
                            break;
                        case UserControlStatsTimeModeEnum.Day:
                            comboBoxTimeMode.SelectedIndex = 1;
                            break;
                        case UserControlStatsTimeModeEnum.Week:
                            comboBoxTimeMode.SelectedIndex = 2;
                            break;
                        case UserControlStatsTimeModeEnum.Month:
                            comboBoxTimeMode.SelectedIndex = 3;
                            break;

                        default:
                            comboBoxTimeMode.SelectedIndex = 0;
                            break;
                    }
                }
            }
        }

        public UserControlStatsDrawModeEnum DrawMode
        {
            get
            {
                if (checkBoxCustomText.Checked)
                    return UserControlStatsDrawModeEnum.Text;
                else
                    return UserControlStatsDrawModeEnum.Graph;
            }

            set
            {
                if (value == UserControlStatsDrawModeEnum.Text)
                    checkBoxCustomText.Checked = true;
                else
                    checkBoxCustomGraph.Checked = true;

            }
        }


        public bool Stars
        {
            get
            {
                if (checkBoxCustomStars.Checked)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value == true)
                    checkBoxCustomStars.Checked = true;
                else
                    checkBoxCustomPlanets.Checked = true;
            }

        }

        public bool ScanMode
        {
            set
            {
                if (value == true)
                {
                    checkBoxCustomStars.Visible = true;
                    checkBoxCustomPlanets.Visible = true;
                }
                else
                {
                    checkBoxCustomStars.Visible = false;
                    checkBoxCustomPlanets.Visible = false;

                }
            }
        }

        #endregion


        private void panelControls_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void UserControlStatsTime_Load(object sender, EventArgs e)
        {
            comboBoxTimeMode.Items.Add("Summary");
            comboBoxTimeMode.Items.Add("Day");
            comboBoxTimeMode.Items.Add("Week");
            comboBoxTimeMode.Items.Add("Month");
            comboBoxTimeMode.SelectedIndex = 0;
        }



        private void comboBoxTimeMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.TimeModeChanged != null)
                TimeModeChanged(this, e);
        }

        private void checkBoxCustomText_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCustomText.Checked == true)
            {
                checkBoxCustomGraph.Checked = false;
                if (this.DrawModeChanged != null)
                    DrawModeChanged(this, e);
            }

        }

        private void checkBoxCustomGraph_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCustomGraph.Checked == true)
            {
                checkBoxCustomText.Checked = false;
                if (this.DrawModeChanged != null)
                    DrawModeChanged(this, e);
            }

        }

        private void checkBoxCustomPlanets_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCustomPlanets.Checked == true)
            {
                checkBoxCustomStars.Checked = false;
                if (this.TimeModeChanged != null)
                    TimeModeChanged(this, e);
            }

        }

        private void checkBoxCustomStars_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCustomStars.Checked == true)
            {
                checkBoxCustomPlanets.Checked = false;
                if (this.TimeModeChanged != null)
                    TimeModeChanged(this, e);
            }
        }
    }
}
