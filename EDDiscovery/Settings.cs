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
        private SQLiteDBClass _db;
        private ThemeEditor themeeditor = null;

        public string MapHomeSystem { get { return textBoxHomeSystem.Text; } }
        public double MapZoom { get { return Double.Parse(textBoxDefaultZoom.Text); } }
        public bool MapCentreOnSelection { get { return radioButtonHistorySelection.Checked; } }

        public Settings()
        {
            InitializeComponent();
        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;
            _db = new SQLiteDBClass();

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

            panel_defaultmapcolor.BackColor = Color.FromArgb(_discoveryForm.TravelControl.defaultMapColour);

            this.comboBoxTheme.SelectedIndexChanged += this.comboBoxTheme_SelectedIndexChanged;    // now turn on the handler..
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

        public void panel_defaultmapcolor_Click(object sender, EventArgs e)
        {
            ColorDialog mapColorDialog = new ColorDialog();
            mapColorDialog.AllowFullOpen = true;
            mapColorDialog.FullOpen = true;
            mapColorDialog.Color = Color.FromArgb(_discoveryForm.TravelControl.defaultMapColour);
            if (mapColorDialog.ShowDialog(this) == DialogResult.OK)
            {
                _discoveryForm.TravelControl.defaultMapColour = mapColorDialog.Color.ToArgb();
                var db = new SQLiteDBClass();
                db.PutSettingInt("DefaultMap", _discoveryForm.TravelControl.defaultMapColour);
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
    }
}
