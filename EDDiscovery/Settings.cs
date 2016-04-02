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


        public Settings()
        {
            InitializeComponent();

            SetPanel(panel_theme1, "Form Back Colour", EDDTheme.Settings.CI.form);                  // using tag, and tool tips, hook up patches to enum
            SetPanel(panel_theme2, "Text box Back Colour", EDDTheme.Settings.CI.textbox_back);
            SetPanel(panel_theme3, "Text box Text Colour", EDDTheme.Settings.CI.textbox_fore);
            SetPanel(panel_theme4, "Text box Highlight Colour", EDDTheme.Settings.CI.textbox_highlight);
            SetPanel(panel_theme15,"Text box Success Colour", EDDTheme.Settings.CI.textbox_success);
            SetPanel(panel_theme5, "Button Back Colour", EDDTheme.Settings.CI.button_back);
            SetPanel(panel_theme6, "Button Text Colour", EDDTheme.Settings.CI.button_text);
            SetPanel(panel_theme7, "Grid Border Back Colour", EDDTheme.Settings.CI.grid_border);
            SetPanel(panel_theme8, "Grid Border Text Colour", EDDTheme.Settings.CI.grid_bordertext);
            SetPanel(panel_theme9, "Grid Data Back Colour", EDDTheme.Settings.CI.grid_background);
            SetPanel(panel_theme10, "Grid Data Text Colour", EDDTheme.Settings.CI.grid_text);
            SetPanel(panel_theme11, "Menu Back Colour", EDDTheme.Settings.CI.menu_back);
            SetPanel(panel_theme12, "Menu Text Colour", EDDTheme.Settings.CI.menu_fore);
            SetPanel(panel_theme13, "Visited system without known position", EDDTheme.Settings.CI.travelgrid_nonvisted);
            SetPanel(panel_theme14, "Visited system with coordinates", EDDTheme.Settings.CI.travelgrid_visited);
            SetPanel(panel_theme16, "Check Box Text Colour", EDDTheme.Settings.CI.checkbox);
            SetPanel(panel_theme17, "Label Text Colour", EDDTheme.Settings.CI.label);
            SetPanel(panel_theme18, "Group box Back Colour", EDDTheme.Settings.CI.group_back);
            SetPanel(panel_theme19, "Group box Text Colour", EDDTheme.Settings.CI.group_text);
        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;
            _db = new SQLiteDBClass();

            _discoveryForm.theme.FillComboBoxWithThemes(comboBoxTheme);                // set up combo box with default themes
            _discoveryForm.theme.SetComboBoxIndex(comboBoxTheme);                      // given the theme selected, set the combo box

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

            UpdatePatchesEtc();

            panel_defaultmapcolor.BackColor = Color.FromArgb(_discoveryForm.TravelControl.defaultMapColour);

            trackBar_theme_opacity.Value = (int)_discoveryForm.theme.Opacity;

            this.comboBoxTheme.SelectedIndexChanged += new System.EventHandler(this.comboBoxTheme_SelectedIndexChanged);    // now turn on the handler.. 
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

        public void UpdatePatchesEtc()                                         // update patch colours..
        {
            _discoveryForm.theme.UpdatePatch(panel_theme1);
            _discoveryForm.theme.UpdatePatch(panel_theme2);
            _discoveryForm.theme.UpdatePatch(panel_theme3);
            _discoveryForm.theme.UpdatePatch(panel_theme4);
            _discoveryForm.theme.UpdatePatch(panel_theme5);
            _discoveryForm.theme.UpdatePatch(panel_theme6);
            _discoveryForm.theme.UpdatePatch(panel_theme7);
            _discoveryForm.theme.UpdatePatch(panel_theme8);
            _discoveryForm.theme.UpdatePatch(panel_theme9);
            _discoveryForm.theme.UpdatePatch(panel_theme10);
            _discoveryForm.theme.UpdatePatch(panel_theme11);
            _discoveryForm.theme.UpdatePatch(panel_theme12);
            _discoveryForm.theme.UpdatePatch(panel_theme13);
            _discoveryForm.theme.UpdatePatch(panel_theme14);
            _discoveryForm.theme.UpdatePatch(panel_theme15);
            _discoveryForm.theme.UpdatePatch(panel_theme16);
            _discoveryForm.theme.UpdatePatch(panel_theme17);
            _discoveryForm.theme.UpdatePatch(panel_theme18);
            _discoveryForm.theme.UpdatePatch(panel_theme19);
            textBox_Font.Text = _discoveryForm.theme.FontName;
            checkBox_theme_windowframe.Checked = _discoveryForm.theme.WindowsFrame;
        }

        private void SetPanel(Panel pn, string name, EDDTheme.Settings.CI ex)
        {
            toolTip.SetToolTip(pn, name);        // assign tool tips and indicate which color to edit
            pn.Tag = ex;
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
            string themename = comboBoxTheme.Items[comboBoxTheme.SelectedIndex].ToString();

            string fontwanted = null;                                               // don't check custom, only a stored theme..
            if (!themename.Equals("Custom") && !_discoveryForm.theme.IsFontAvailableInTheme(themename, out fontwanted))
            {
                DialogResult res = MessageBox.Show("The font used by this theme is not available on your system" + Environment.NewLine +
                      "The font needed is \"" + fontwanted + "\"" + Environment.NewLine +
                      "Install this font and you can use this scheme.",
                      "Warning", MessageBoxButtons.OK);

                _discoveryForm.theme.SetCustom();                              // go to custom theme whatever
                _discoveryForm.theme.SetComboBoxIndex(comboBoxTheme);           // reselect to custom, refires this..
                return;
            }

            if (!_discoveryForm.theme.SetThemeByName(themename))    
                _discoveryForm.theme.SetCustom();                                   // go to custom theme..

            _discoveryForm.ApplyTheme(true);
            UpdatePatchesEtc();
        }

        private void trackBar_theme_opacity_MouseCaptureChanged(object sender, EventArgs e)
        {
            _discoveryForm.theme.Opacity = (double)trackBar_theme_opacity.Value;
            _discoveryForm.ApplyTheme(true);
            _discoveryForm.theme.SetComboBoxIndex(comboBoxTheme);                      // given the theme selected, set the combo box
        }

        private void checkBox_theme_windowframe_MouseClick(object sender, MouseEventArgs e)
        {
            _discoveryForm.theme.WindowsFrame = checkBox_theme_windowframe.Checked;
            _discoveryForm.ApplyTheme(true);
            _discoveryForm.theme.SetComboBoxIndex(comboBoxTheme);                      // given the theme selected, set the combo box
        }

        private void panel_theme_Click(object sender, EventArgs e)  
        {
            EDDTheme.Settings.CI ci = (EDDTheme.Settings.CI)(((Control)sender).Tag);        // tag carries the colour we want to edit

            if (_discoveryForm.theme.EditColor(ci))
            {
                _discoveryForm.ApplyTheme(true);
                _discoveryForm.theme.SetComboBoxIndex(comboBoxTheme);                      // given the theme selected, set the combo box
                UpdatePatchesEtc();
            }
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
                _discoveryForm.theme.Name = Path.GetFileNameWithoutExtension(dlg.FileName); // go to the name

                _discoveryForm.theme.FillComboBoxWithThemes(comboBoxTheme);   // set up combo box with default themes

                if (!_discoveryForm.theme.SetComboBoxIndex(comboBoxTheme))    // if can't select it, probably saved it somewhere else..
                {
                    _discoveryForm.theme.SetCustom();   // custom
                }
            }
        }

        private void textBoxFont_MouseClick(object sender, MouseEventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.Font = new Font(_discoveryForm.theme.FontName, _discoveryForm.theme.FontSize);
            fd.MinSize = 4;
            fd.MaxSize = 12;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                if (fd.Font.Style == FontStyle.Regular)
                {
                    _discoveryForm.theme.FontName = fd.Font.Name;
                    _discoveryForm.theme.FontSize = fd.Font.Size;
                    _discoveryForm.theme.SetComboBoxIndex(comboBoxTheme);                      // given the theme selected, set the combo box
                    UpdatePatchesEtc();
                    _discoveryForm.ApplyTheme(true);
                }
                else
                    MessageBox.Show("Font does not have regular style");
            }
        }

        public void panel_defaultmapcolor_Click(object sender, EventArgs e)
        {
            ColorDialog mapColorDialog = new ColorDialog();

            mapColorDialog.Color = Color.FromArgb(_discoveryForm.TravelControl.defaultMapColour);
            if (mapColorDialog.ShowDialog(this) == DialogResult.OK)
            {
                _discoveryForm.TravelControl.defaultMapColour = mapColorDialog.Color.ToArgb();
                var db = new SQLiteDBClass();
                db.PutSettingInt("DefaultMap", _discoveryForm.TravelControl.defaultMapColour);
                panel_defaultmapcolor.BackColor = Color.FromArgb(_discoveryForm.TravelControl.defaultMapColour);
            }
        }


    }
}
