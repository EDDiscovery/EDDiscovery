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
using System.IO;

namespace EDDiscovery2
{
    public partial class Settings : UserControl
    {
        private EDDiscoveryForm _discoveryForm;
        private ThemeEditor themeeditor = null;

        public string MapHomeSystem { get { return textBoxHomeSystem.Text; } }
        public float MapZoom { get { return float.Parse(textBoxDefaultZoom.Text); } }
        public bool MapCentreOnSelection { get { return radioButtonHistorySelection.Checked; } }
        public bool OrderRowsInverted {  get { return checkBoxOrderRowsInverted.Checked; } }

        public Settings()
        {
            InitializeComponent();
        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;

            ResetThemeList();
            SetEntryThemeComboBox();
        }

        void SetEntryThemeComboBox()
        {
            int i = _discoveryForm.theme.GetIndexOfCurrentTheme();
            if (i == -1)
                comboBoxTheme.SelectedItem = "Custom";
            else
                comboBoxTheme.SelectedIndex = i;
        }

        private void ResetThemeList()
        {
            comboBoxTheme.Items = _discoveryForm.theme.GetThemeList();
            comboBoxTheme.Items.Add("Custom");
        }

        public void InitSettingsTab()
        {
            bool auto = EDDConfig.Instance.NetLogDirAutoMode;

            if (auto)
            {
                radioButton_Auto.Checked = auto;
            }
            else
            {
                radioButton_Manual.Checked = true;
            }

            textBoxNetLogDir.Text = EDDConfig.Instance.NetLogDir;

            EDDConfig.Instance.NetLogDirAutoModeChanged += EDDConfig_NetLogDirAutoModeChanged;
            EDDConfig.Instance.NetLogDirChanged += EDDConfig_NetLogDirChanged;

            checkBox_Distances.Checked = EDDiscoveryForm.EDDConfig.UseDistances;
            checkBoxEDSMLog.Checked = EDDiscoveryForm.EDDConfig.EDSMLog;
            checkboxSkipSlowUpdates.Checked = EDDiscoveryForm.EDDConfig.CanSkipSlowUpdates;
            checkBoxOrderRowsInverted.Checked = EDDiscoveryForm.EDDConfig.OrderRowsInverted;
            checkBoxFocusNewSystem.Checked = EDDiscoveryForm.EDDConfig.FocusOnNewSystem;
#if DEBUG
            checkboxSkipSlowUpdates.Visible = true;
#endif
            textBoxHomeSystem.Text = SQLiteDBClass.GetSettingString("DefaultMapCenter", "Sol");

            textBoxDefaultZoom.Text = SQLiteDBClass.GetSettingDouble("DefaultMapZoom", 1.0).ToString();

            bool selectionCentre = SQLiteDBClass.GetSettingBool("CentreMapOnSelection", true);
            if (selectionCentre)
            {
                radioButtonHistorySelection.Checked = true;
            }
            else
            {
                radioButtonCentreHome.Checked = true;
            }

            dataGridViewCommanders.DataSource = EDDiscoveryForm.EDDConfig.listCommanders;

            panel_defaultmapcolor.BackColor = Color.FromArgb(_discoveryForm.TravelControl.defaultMapColour);

            this.comboBoxTheme.SelectedIndexChanged += this.comboBoxTheme_SelectedIndexChanged;    // now turn on the handler..
        }

        public void SaveSettings()
        {
            EDDConfig.Instance.NetLogDirAutoMode = radioButton_Auto.Checked;
            EDDConfig.Instance.NetLogDir = textBoxNetLogDir.Text;
            SQLiteDBClass.PutSettingString("DefaultMapCenter", textBoxHomeSystem.Text);
            SQLiteDBClass.PutSettingDouble("DefaultMapZoom", Double.Parse(textBoxDefaultZoom.Text));
            SQLiteDBClass.PutSettingBool("CentreMapOnSelection", radioButtonHistorySelection.Checked);

            EDDiscoveryForm.EDDConfig.UseDistances = checkBox_Distances.Checked;
            EDDiscoveryForm.EDDConfig.EDSMLog = checkBoxEDSMLog.Checked;
            EDDiscoveryForm.EDDConfig.CanSkipSlowUpdates = checkboxSkipSlowUpdates.Checked;
            EDDiscoveryForm.EDDConfig.OrderRowsInverted = checkBoxOrderRowsInverted.Checked;
            EDDiscoveryForm.EDDConfig.FocusOnNewSystem = checkBoxFocusNewSystem.Checked;

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

        private void textBoxNetLogDir_Validating(object sender, CancelEventArgs e)
        {
            var path = textBoxNetLogDir.Text;
            if (!Directory.Exists(path))
            {
                e.Cancel = true;
            }
        }

        private void textBoxNetLogDir_Validated(object sender, EventArgs e)
        {
            EDDConfig.Instance.NetLogDir = textBoxNetLogDir.Text;
        }

        private void radioButton_Auto_CheckedChanged(object sender, EventArgs e)
        {
            EDDConfig.Instance.NetLogDirAutoMode = radioButton_Auto.Checked;
        }

        private void EDDConfig_NetLogDirChanged()
        {
            if (EDDConfig.Instance.NetLogDir != textBoxNetLogDir.Text)
            {
                textBoxNetLogDir.Text = EDDConfig.Instance.NetLogDir;
            }
        }

        private void EDDConfig_NetLogDirAutoModeChanged()
        {
            if (EDDConfig.Instance.NetLogDirAutoMode != radioButton_Auto.Checked)
            {
                radioButton_Auto.Checked = EDDConfig.Instance.NetLogDirAutoMode;
                radioButton_Manual.Checked = !EDDConfig.Instance.NetLogDirAutoMode;
            }
        }

        private void button_Browse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dirdlg = new FolderBrowserDialog();

            DialogResult dlgResult = dirdlg.ShowDialog();

            if (dlgResult == DialogResult.OK)
            {
                textBoxNetLogDir.Text = dirdlg.SelectedPath;
                EDDConfig.Instance.NetLogDir = textBoxNetLogDir.Text;
            }
        }

        private void buttonAddCommander_Click(object sender, EventArgs e)
        {
            EDCommander cmdr = EDDiscoveryForm.EDDConfig.GetNewCommander();
            EDDiscoveryForm.EDDConfig.listCommanders.Add(cmdr);
            dataGridViewCommanders.DataSource = null;           // changing data source ends up, after this, screwing the column sizing..
            dataGridViewCommanders.DataSource = EDDiscoveryForm.EDDConfig.listCommanders;   // can't solve it, TBD
            dataGridViewCommanders.Update();
            _discoveryForm.TravelControl.LoadCommandersListBox();
        }

        private void dataGridViewCommanders_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            List<EDCommander> edcommanders = (List<EDCommander>)dataGridViewCommanders.DataSource;
            EDDiscoveryForm.EDDConfig.StoreCommanders(edcommanders);
            _discoveryForm.TravelControl.LoadCommandersListBox();
        }

        public void panel_defaultmapcolor_Click(object sender, EventArgs e)
        {
            ColorDialog mapColorDialog = new ColorDialog();
            mapColorDialog.AllowFullOpen = true;
            mapColorDialog.FullOpen = true;
            mapColorDialog.Color = Color.FromArgb(_discoveryForm.TravelControl.defaultMapColour);
            if (mapColorDialog.ShowDialog(this) == DialogResult.OK)
            {
                _discoveryForm.TravelControl.defaultMapColour = mapColorDialog.Color.ToArgb();
                EDDConfig.Instance.DefaultMapColour = _discoveryForm.TravelControl.defaultMapColour;
                panel_defaultmapcolor.BackColor = Color.FromArgb(_discoveryForm.TravelControl.defaultMapColour);
            }
        }

        private void comboBoxTheme_SelectedIndexChanged(object sender, EventArgs e) // theme selected..
        {
            string themename = comboBoxTheme.Items[comboBoxTheme.SelectedIndex].ToString();

            string fontwanted = null;                                               // don't check custom, only a stored theme..
            if (!themename.Equals("Custom") && !_discoveryForm.theme.IsFontAvailableInTheme(themename, out fontwanted))
            {
                DialogResult res = MessageBox.Show("The font used by this theme is not available on your system" + Environment.NewLine +
                      "The font needed is \"" + fontwanted + "\"" + Environment.NewLine +
                      "Install this font and you can use this scheme." + Environment.NewLine +
                      "EuroCaps font is available www.edassets.org.",
                      "Warning", MessageBoxButtons.OK);

                _discoveryForm.theme.SetCustom();                              // go to custom theme whatever
                SetEntryThemeComboBox();
                return;
            }

            if (!_discoveryForm.theme.SetThemeByName(themename))
                _discoveryForm.theme.SetCustom();                                   // go to custom theme..

            SetEntryThemeComboBox();
            _discoveryForm.ApplyTheme(true);
        }

        private void buttonSaveTheme_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.InitialDirectory = Path.Combine(Tools.GetAppDataDirectory(), "Theme");
            dlg.DefaultExt = "eddtheme";
            dlg.AddExtension = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _discoveryForm.theme.SaveSettings(dlg.FileName);        // should create a new theme files
                _discoveryForm.theme.LoadThemes();          // make sure up to data - we added a theme, reload them all
                _discoveryForm.theme.Name = Path.GetFileNameWithoutExtension(dlg.FileName); // go to the theme name

                ResetThemeList();

                int curindex = _discoveryForm.theme.GetIndexOfCurrentTheme();       // get theme index.. may be -1 if theme not loaded back

                if ( curindex == -1 )                                   // if not loaded, back to custom
                    _discoveryForm.theme.SetCustom();   // custom

                SetEntryThemeComboBox();
            }
        }

        public void button_edittheme_Click(object sender, EventArgs e)
        {
            if (themeeditor == null)                    // no theme editor, make one..
            {
                themeeditor = new ThemeEditor();
                themeeditor.InitForm(_discoveryForm);
                themeeditor.FormClosing += close_edit;  // lets see when it closes

                comboBoxTheme.Enabled = false;          // no doing this while theme editor is open
                buttonSaveTheme.Enabled = false;

                themeeditor.Show();                     // run form
            }
            else
                themeeditor.BringToFront();             // its up, make it at front to show it
        }

        public void close_edit(object sender, FormClosingEventArgs e)
        {
            themeeditor = null;                         // called when editor closes
            SetEntryThemeComboBox();
            comboBoxTheme.Enabled = true;          // no doing this while theme editor is open
            buttonSaveTheme.Enabled = true;
        }

        private void checkBoxFocusNewSystem_CheckedChanged(object sender, EventArgs e)
        {
            EDDConfig.Instance.FocusOnNewSystem = checkBoxFocusNewSystem.Checked;
        }

        private void checkBox_Distances_CheckedChanged(object sender, EventArgs e)
        {
            EDDiscoveryForm.EDDConfig.UseDistances = checkBox_Distances.Checked;
        }
    }
}
