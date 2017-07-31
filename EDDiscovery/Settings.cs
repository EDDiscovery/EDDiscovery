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
using System.Windows.Forms;
using System.IO;
using EDDiscovery.Forms;
using EliteDangerousCore;
using EliteDangerousCore.DB;

namespace EDDiscovery
{
    public partial class Settings : UserControl
    {
        private EDDiscoveryForm _discoveryForm;
        private ISystem _homeSystem = new SystemClassDB("Sol", 0, 0, 0);
        private ExtendedControls.ThemeStandardEditor themeeditor = null;

        public ISystem HomeSystem
        {
            get
            {
                return _homeSystem;
            }
            private set
            {
                if (value != null && value.HasCoordinate)
                {
                    _homeSystem = value;
                    textBoxHomeSystem.Text = value.name;
                }
            }
        }
        public float MapZoom { get { return float.Parse(textBoxDefaultZoom.Text); } }
        public bool MapCentreOnSelection { get { return radioButtonHistorySelection.Checked; } }
        public bool OrderRowsInverted { get { return checkBoxOrderRowsInverted.Checked; } }

        public Settings()
        {
            InitializeComponent();
            this.textBoxHomeSystem.SetToolTip(toolTip, "Select home system for 3d Map");
            this.textBoxDefaultZoom.SetToolTip(toolTip, "Select default zoom of map. Use the map itself to determine this for you");
        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;

            ResetThemeList();
            SetEntryThemeComboBox();

            textBoxHomeSystem.SetAutoCompletor(SystemClassDB.ReturnSystemListForAutoComplete);
            comboBoxTheme.ItemHeight = 20;

            btnDeleteCommander.Enabled = EDCommander.NumberOfCommanders > 1;
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
            checkBoxEDSMLog.Checked = EDDiscoveryForm.EDDConfig.EDSMLog;
            checkBoxOrderRowsInverted.Checked = EDDiscoveryForm.EDDConfig.OrderRowsInverted;
            checkBoxMinimizeToNotifyIcon.Checked = EDDiscoveryForm.EDDConfig.MinimizeToNotifyIcon;
            checkBoxFocusNewSystem.Checked = EDDiscoveryForm.EDDConfig.FocusOnNewSystem;
            checkBoxKeepOnTop.Checked = EDDiscoveryForm.EDDConfig.KeepOnTop;
            checkBoxUseNotifyIcon.Checked = EDDiscoveryForm.EDDConfig.UseNotifyIcon;
            checkBoxUTC.Checked = EDDiscoveryForm.EDDConfig.DisplayUTC;
            checkBoxAutoLoad.Checked = EDDiscoveryForm.EDDConfig.AutoLoadPopOuts;
            checkBoxAutoSave.Checked = EDDiscoveryForm.EDDConfig.AutoSavePopOuts;

            checkBoxMinimizeToNotifyIcon.Enabled = EDDiscoveryForm.EDDConfig.UseNotifyIcon;

            HomeSystem = SystemClassDB.GetSystem(SQLiteDBClass.GetSettingString("DefaultMapCenter", "Sol"));

            textBoxDefaultZoom.Text = SQLiteDBClass.GetSettingDouble("DefaultMapZoom", 1.0).ToString();

            bool selectionCentre = SQLiteDBClass.GetSettingBool("CentreMapOnSelection", true);
            radioButtonHistorySelection.Checked = selectionCentre;
            radioButtonCentreHome.Checked = !selectionCentre;

            dataGridViewCommanders.AutoGenerateColumns = false;             // BEFORE assigned to list..
            dataGridViewCommanders.DataSource = EDCommander.GetList();

            panel_defaultmapcolor.BackColor = Color.FromArgb(EDDConfig.Instance.DefaultMapColour);

            this.comboBoxTheme.SelectedIndexChanged += this.comboBoxTheme_SelectedIndexChanged;    // now turn on the handler..
        }

        public void SaveSettings()
        {
            SQLiteDBClass.PutSettingString("DefaultMapCenter", textBoxHomeSystem.Text);
            double zoom = 1;
            SQLiteDBClass.PutSettingDouble("DefaultMapZoom", Double.TryParse(textBoxDefaultZoom.Text, out zoom) ? zoom : 1.0);
            SQLiteDBClass.PutSettingBool("CentreMapOnSelection", radioButtonHistorySelection.Checked);

            EDDiscoveryForm.EDDConfig.EDSMLog = checkBoxEDSMLog.Checked;
            _discoveryForm.SetUpLogging();

            EDDiscoveryForm.EDDConfig.UseNotifyIcon = checkBoxUseNotifyIcon.Checked;
            EDDiscoveryForm.EDDConfig.OrderRowsInverted = checkBoxOrderRowsInverted.Checked;
            EDDiscoveryForm.EDDConfig.MinimizeToNotifyIcon = checkBoxMinimizeToNotifyIcon.Checked;
            EDDiscoveryForm.EDDConfig.FocusOnNewSystem = checkBoxFocusNewSystem.Checked;
            EDDiscoveryForm.EDDConfig.KeepOnTop = checkBoxKeepOnTop.Checked;
            EDDiscoveryForm.EDDConfig.DisplayUTC = checkBoxUTC.Checked;
            EDDiscoveryForm.EDDConfig.AutoLoadPopOuts = checkBoxAutoLoad.Checked;
            EDDiscoveryForm.EDDConfig.AutoSavePopOuts = checkBoxAutoSave.Checked;
        }

        private void textBoxDefaultZoom_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var value = textBoxDefaultZoom.Text.Trim();
            double parseout = 0;
            if (!Double.TryParse(value, out parseout) || parseout < 0.01 || parseout > 50.0)
            {
                textBoxDefaultZoom.Text = "1";
            }
        }

        public void UpdateCommandersListBox()
        {
            dataGridViewCommanders.DataSource = null;
            dataGridViewCommanders.DataSource = EDCommander.GetList();
            dataGridViewCommanders.Update();
        }

        private void buttonAddCommander_Click(object sender, EventArgs e)
        {
            CommanderForm cf = new CommanderForm();
            cf.Init(true);

            if (cf.ShowDialog(this) == DialogResult.OK)
            {
                if (cf.Valid && !EDCommander.IsCommanderPresent(cf.CommanderName))
                {
                    EDCommander cmdr = new EDCommander();
                    cf.Update(cmdr);
                    EDCommander.Create(cmdr);
                    UpdateCommandersListBox();
                    _discoveryForm.LoadCommandersListBox();
                    _discoveryForm.ExportControl.PopulateCommanders();
                    _discoveryForm.RefreshHistoryAsync();           // will do a new parse on commander list adding/removing scanners
                    btnDeleteCommander.Enabled = EDCommander.NumberOfCommanders > 1;
                }
                else
                    ExtendedControls.MessageBoxTheme.Show(this, "Command name is not valid or duplicate" , "Cannot create Commander", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void buttonEditCommander_Click(object sender, EventArgs e)
        {
            if (dataGridViewCommanders.CurrentCell != null)
            {
                int row = dataGridViewCommanders.CurrentCell.RowIndex;
                EDCommander cmdr = dataGridViewCommanders.Rows[row].DataBoundItem as EDCommander;

                CommanderForm cf = new CommanderForm();
                cf.Init(cmdr,false);

                if (cf.ShowDialog(this) == DialogResult.OK)
                {
                    string currentjloc = cmdr.JournalDir;

                    cf.Update(cmdr);
                    List<EDCommander> edcommanders = (List<EDCommander>)dataGridViewCommanders.DataSource;
                    EDCommander.Update(edcommanders, false);

                    if ( currentjloc != cmdr.JournalDir )
                        _discoveryForm.RefreshHistoryAsync();           // will do a new parse on commander list adding/removing scanners
                }
            }
        }

        private void btnDeleteCommander_Click(object sender, EventArgs e)
        {
            if (dataGridViewCommanders.CurrentCell != null)
            {
                int row = dataGridViewCommanders.CurrentCell.RowIndex;
                EDCommander cmdr = dataGridViewCommanders.Rows[row].DataBoundItem as EDCommander;

                var result = ExtendedControls.MessageBoxTheme.Show("Do you wish to delete commander " + cmdr.Name + "?", "Delete commander", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    EDCommander.Delete(cmdr);
                    _discoveryForm.LoadCommandersListBox();
                    _discoveryForm.ExportControl.PopulateCommanders();
                    UpdateCommandersListBox();
                    _discoveryForm.RefreshHistoryAsync();           // will do a new parse on commander list adding/removing scanners

                    btnDeleteCommander.Enabled = EDCommander.NumberOfCommanders > 1;
                }
            }
        }

        public void panel_defaultmapcolor_Click(object sender, EventArgs e)
        {
            ColorDialog mapColorDialog = new ColorDialog();
            mapColorDialog.AllowFullOpen = true;
            mapColorDialog.FullOpen = true;
            mapColorDialog.Color = Color.FromArgb(EDDConfig.Instance.DefaultMapColour);
            if (mapColorDialog.ShowDialog(this) == DialogResult.OK)
            {
                EDDConfig.Instance.DefaultMapColour = mapColorDialog.Color.ToArgb();
                EDDConfig.Instance.DefaultMapColour = EDDConfig.Instance.DefaultMapColour;
                panel_defaultmapcolor.BackColor = Color.FromArgb(EDDConfig.Instance.DefaultMapColour);
            }
        }

        private void comboBoxTheme_SelectedIndexChanged(object sender, EventArgs e) // theme selected..
        {
            string themename = comboBoxTheme.Items[comboBoxTheme.SelectedIndex].ToString();

            string fontwanted = null;                                               // don't check custom, only a stored theme..
            if (!themename.Equals("Custom") && !_discoveryForm.theme.IsFontAvailableInTheme(themename, out fontwanted))
            {
                DialogResult res = ExtendedControls.MessageBoxTheme.Show("The font used by this theme is not available on your system" + Environment.NewLine +
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
            _discoveryForm.ApplyTheme();
        }

        private void buttonSaveTheme_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.InitialDirectory = Path.Combine(EDDConfig.Options.AppDataDirectory, "Theme");
            dlg.DefaultExt = "eddtheme";
            dlg.AddExtension = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _discoveryForm.theme.SaveSettings(dlg.FileName);        // should create a new theme files
                _discoveryForm.theme.LoadThemes();          // make sure up to data - we added a theme, reload them all
                _discoveryForm.theme.Name = Path.GetFileNameWithoutExtension(dlg.FileName); // go to the theme name

                ResetThemeList();

                int curindex = _discoveryForm.theme.GetIndexOfCurrentTheme();       // get theme index.. may be -1 if theme not loaded back

                if (curindex == -1)                                   // if not loaded, back to custom
                    _discoveryForm.theme.SetCustom();   // custom

                SetEntryThemeComboBox();
            }
        }

        public void UpdateThemeChanges()
        {
            _discoveryForm.ApplyTheme();
        }

        public void button_edittheme_Click(object sender, EventArgs e)
        {
            if (themeeditor == null)                    // no theme editor, make one..
            {
                themeeditor = new ExtendedControls.ThemeStandardEditor();
                themeeditor.ApplyChanges = UpdateThemeChanges;
                themeeditor.InitForm();
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

        private void checkBoxKeepOnTop_CheckedChanged(object sender, EventArgs e)
        {
            EDDConfig.Instance.KeepOnTop = checkBoxKeepOnTop.Checked;
            this.FindForm().TopMost = checkBoxKeepOnTop.Checked;
            _discoveryForm.keepOnTopChanged(checkBoxKeepOnTop.Checked);
        }

        private void checkBoxMinimizeToNotifyIcon_CheckedChanged(object sender, EventArgs e)
        {
            EDDiscoveryForm.EDDConfig.MinimizeToNotifyIcon = checkBoxMinimizeToNotifyIcon.Checked;
        }

        private void checkBoxUseNotifyIcon_CheckedChanged(object sender, EventArgs e)
        {
            bool chk = checkBoxUseNotifyIcon.Checked;
            EDDiscoveryForm.EDDConfig.UseNotifyIcon = chk;
            checkBoxMinimizeToNotifyIcon.Enabled = chk;
            _discoveryForm.useNotifyIconChanged(chk);
        }

        private void checkBoxUTC_CheckedChanged(object sender, EventArgs e)
        {
            EDDiscoveryForm.EDDConfig.DisplayUTC = checkBoxUTC.Checked;
            _discoveryForm.RefreshDisplays();
        }

        private void buttonSaveSetup_Click(object sender, EventArgs e)
        {
            _discoveryForm.SaveCurrentPopOuts();
        }

        private void buttonReloadSaved_Click(object sender, EventArgs e)
        {
            _discoveryForm.LoadSavedPopouts();
        }

        private void textBoxHomeSystem_Validated(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxHomeSystem.Text))
            {
                HomeSystem = SystemClassDB.GetSystem(textBoxHomeSystem.Text);
            }
        }
    }
}

