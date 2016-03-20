using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EDDiscovery;
using EDDiscovery2.DB;
using EDDiscovery.DB;

namespace EDDiscovery2
{
    public partial class Settings : UserControl
    {
        private EDDiscoveryForm _discoveryForm;
        private EDDTheme theme;
        private SQLiteDBClass _db; 


        public Settings()
        {
            InitializeComponent();
        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;
            _db = new SQLiteDBClass();
            theme = _discoveryForm.theme;                               // ref value to same one c# rules

            theme.FillComboBoxWithThemes(comboBoxTheme);                // set up combo box with default themes
            comboBoxTheme.Items.Add("Custom");                          // and add an extra custom one which enables the individual controls
            theme.SetComboBoxIndex(comboBoxTheme);                      // given the theme selected, set the combo box

//            button_theme_forecolor.Visible = _discoveryForm.theme.IsCustomTheme();
            //button_theme_forecolor.ForeColor = theme.ForeColor;
            //button_theme_backcolor.Visible = theme.IsCustomTheme();
            //button_theme_backcolor.ForeColor = Color.FromArgb(1 - theme.BackColor.ToArgb());          // ensure its visible
            //b/utton_theme_textcolor.Visible = theme.IsCustomTheme();
            //button_theme_textcolor.ForeColor = theme.TextColor;
            //button_theme_highlightcolor.Visible = theme.IsCustomTheme();
            //button_theme_highlightcolor.ForeColor = theme.TextHighlightColor;
            //button_theme_visitedcolor.Visible = theme.IsCustomTheme();
            //button_theme_visitedcolor.ForeColor = theme.VisitedSystemColor;
            //button_theme_mapblockcolor.Visible = theme.IsCustomTheme();
            //button_theme_mapblockcolor.ForeColor = theme.MapBlockColor;
            checkBox_theme_windowframe.Visible = theme.IsCustomTheme();
            checkBox_theme_windowframe.Checked = theme.WindowsFrame;
            trackBar_theme_opacity.Visible = theme.IsCustomTheme();
            trackBar_theme_opacity.Value = (int)theme.Opacity;
            label_opacity.Visible = theme.IsCustomTheme();
        }

        public void InitSettingsTab()
        {
            bool auto = _db.GetSettingBool("NetlogDirAutoMode", true);
            
            if (auto)
            {
                radioButton_Auto.Checked = auto;
            }
            else
            {
                radioButton_Manual.Checked = true;
            }

            string datapath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Frontier_Developments\\Products"); // \\FORC-FDEV-D-1001\\Logs\\";
            textBoxNetLogDir.Text = _db.GetSettingString("Netlogdir", datapath);

            checkBox_Distances.Checked = EDDiscoveryForm.EDDConfig.UseDistances;
            checkBoxEDSMLog.Checked = EDDiscoveryForm.EDDConfig.EDSMLog;
            checkboxSkipSlowUpdates.Checked = EDDiscoveryForm.EDDConfig.CanSkipSlowUpdates;
#if DEBUG
            checkboxSkipSlowUpdates.Visible = true;
#endif
            textBoxHomeSystem.AutoCompleteCustomSource = _discoveryForm.SystemNames;
            textBoxHomeSystem.Text = _db.GetSettingString("DefaultMapCenter", "Sol");

            textBoxDefaultZoom.Text = _db.GetSettingDouble("DefaultMapZoom", 1.0).ToString();

            bool selectionCentre = _db.GetSettingBool("CentreMapOnSelection", true);
            if (selectionCentre)
            {
                radioButtonHistorySelection.Checked = true;
            }
            else
            {
                radioButtonCentreHome.Checked = true;
            }

            dataGridViewCommanders.DataSource = EDDiscoveryForm.EDDConfig.listCommanders;
        }

        public void SaveSettings()
        {
            _db.PutSettingBool("NetlogDirAutoMode", radioButton_Auto.Checked);
            _db.PutSettingString("Netlogdir", textBoxNetLogDir.Text);
            _db.PutSettingString("DefaultMapCenter", textBoxHomeSystem.Text);
            _db.PutSettingDouble("DefaultMapZoom", Double.Parse(textBoxDefaultZoom.Text));
            _db.PutSettingBool("CentreMapOnSelection", radioButtonHistorySelection.Checked);

            EDDiscoveryForm.EDDConfig.UseDistances = checkBox_Distances.Checked;
            EDDiscoveryForm.EDDConfig.EDSMLog = checkBoxEDSMLog.Checked;
            EDDiscoveryForm.EDDConfig.CanSkipSlowUpdates = checkboxSkipSlowUpdates.Checked;

            List<EDCommander> edcommanders = (List<EDCommander>)dataGridViewCommanders.DataSource;
            EDDiscoveryForm.EDDConfig.StoreCommanders(edcommanders);
            dataGridViewCommanders.DataSource = null;
            dataGridViewCommanders.DataSource = EDDiscoveryForm.EDDConfig.listCommanders;
            dataGridViewCommanders.Update();
        }

        private void textBoxDefaultZoom_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var value = textBoxDefaultZoom.Text.Trim();
            double parseout;
            if (Double.TryParse(value, out parseout))
            {
                e.Cancel = (parseout < 0.01 || parseout > 50.0);
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void button_Browse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dirdlg = new FolderBrowserDialog();

            DialogResult dlgResult = dirdlg.ShowDialog();

            if (dlgResult == DialogResult.OK)
            {
                textBoxNetLogDir.Text = dirdlg.SelectedPath;
            }
        }

        private void buttonAddCommander_Click(object sender, EventArgs e)
        {
            EDCommander cmdr = EDDiscoveryForm.EDDConfig.GetNewCommander();
            EDDiscoveryForm.EDDConfig.listCommanders.Add(cmdr);
            dataGridViewCommanders.DataSource = null;
            dataGridViewCommanders.DataSource = EDDiscoveryForm.EDDConfig.listCommanders;
            dataGridViewCommanders.Update();
        }

        private void comboBoxTheme_SelectedIndexChanged(object sender, EventArgs e) // theme selected..
        {
            if (!_discoveryForm.theme.SetThemeByName(comboBoxTheme.Items[comboBoxTheme.SelectedIndex].ToString()))    // only Custom will fail..
                _discoveryForm.theme.SetCustom();                              // go to custom theme..

            _discoveryForm.ApplyTheme(true);
        }

        private void trackBar_theme_opacity_MouseCaptureChanged(object sender, EventArgs e)
        {
            _discoveryForm.theme.Opacity = (double)trackBar_theme_opacity.Value;
            _discoveryForm.ApplyTheme(true);
        }

        private void button_theme_forecolor_Click(object sender, EventArgs e)
        {
            _discoveryForm.theme.EditColor(EDDTheme.EditIndex.Fore);
            _discoveryForm.ApplyTheme(true);                               // no harm even if nothing changed..
        }

        private void button_theme_backcolor_Click(object sender, EventArgs e)
        {
            _discoveryForm.theme.EditColor(EDDTheme.EditIndex.Back);
            _discoveryForm.ApplyTheme(true);
        }

        private void button_theme_textcolor_Click(object sender, EventArgs e)
        {
            _discoveryForm.theme.EditColor(EDDTheme.EditIndex.Text);
            _discoveryForm.ApplyTheme(true);
        }

        private void button_theme_visited_Click(object sender, EventArgs e)
        {
            _discoveryForm.theme.EditColor(EDDTheme.EditIndex.Visited);
            _discoveryForm.ApplyTheme(true);
        }

        private void button_theme_texthighlightcolor_Click(object sender, EventArgs e)
        {
            _discoveryForm.theme.EditColor(EDDTheme.EditIndex.HL);
            _discoveryForm.ApplyTheme(true);
        }

        private void button_theme_mapblockcolor_Click(object sender, EventArgs e)
        {
            _discoveryForm.theme.EditColor(EDDTheme.EditIndex.MapBlock);
            _discoveryForm.ApplyTheme(true);
        }

        private void checkBox_theme_windowframe_MouseClick(object sender, MouseEventArgs e)
        {
            _discoveryForm.theme.WindowsFrame = checkBox_theme_windowframe.Checked;
            _discoveryForm.ApplyTheme(true);
        }

    }
}
