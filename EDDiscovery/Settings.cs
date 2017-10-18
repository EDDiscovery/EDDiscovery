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
        private ISystem _homeSystem = new SystemClass("Sol", 0, 0, 0);

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

        public Settings()
        {
            InitializeComponent();
        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;

            ResetThemeList();
            SetEntryThemeComboBox();

            textBoxHomeSystem.SetAutoCompletor(SystemClassDB.ReturnSystemListForAutoComplete);
            comboBoxTheme.ItemHeight = 20;

            btnDeleteCommander.Enabled = EDCommander.NumberOfCommanders > 1;

            comboBoxClickThruKey.Items = KeyObjectExtensions.KeyListString(inclshifts:true);
            comboBoxClickThruKey.SelectedItem = EDDConfig.Instance.ClickThruKey.VKeyToString();
            comboBoxClickThruKey.SelectedIndexChanged += comboBoxClickThruKey_SelectedIndexChanged;
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
            checkBoxKeepOnTop.Checked = EDDiscoveryForm.EDDConfig.KeepOnTop;
            checkBoxUseNotifyIcon.Checked = EDDiscoveryForm.EDDConfig.UseNotifyIcon;
            checkBoxUTC.Checked = EDDiscoveryForm.EDDConfig.DisplayUTC;
            checkBoxAutoLoad.Checked = EDDiscoveryForm.EDDConfig.AutoLoadPopOuts;
            checkBoxAutoSave.Checked = EDDiscoveryForm.EDDConfig.AutoSavePopOuts;
            checkBoxShowUIEvents.Checked = EDDiscoveryForm.EDDConfig.ShowUIEvents;

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

            checkBoxCustomRemoveOriginals.Checked = _discoveryForm.screenshotconverter.RemoveOriginal;
            checkBoxCustomMarkHiRes.Checked = _discoveryForm.screenshotconverter.MarkHiRes;
            checkBoxCustomEnableScreenshots.Checked = _discoveryForm.screenshotconverter.AutoConvert;
            checkBoxCustomCopyToClipboard.Checked = _discoveryForm.screenshotconverter.CopyToClipboard;

            this.checkBoxCustomRemoveOriginals.CheckedChanged += new System.EventHandler(this.checkBoxCustomRemoveOriginals_CheckedChanged);
            this.checkBoxCustomMarkHiRes.CheckedChanged += new System.EventHandler(this.checkBoxCustomMarkHiRes_CheckedChanged);
            this.checkBoxCustomEnableScreenshots.CheckedChanged += new System.EventHandler(this.checkBoxCustomEnableScreenshots_CheckedChanged);
            this.checkBoxCustomCopyToClipboard.CheckedChanged += new System.EventHandler(this.checkBoxCustomCopyToClipboard_CheckedChanged);
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
            EDDiscoveryForm.EDDConfig.KeepOnTop = checkBoxKeepOnTop.Checked;
            EDDiscoveryForm.EDDConfig.DisplayUTC = checkBoxUTC.Checked;
            EDDiscoveryForm.EDDConfig.AutoLoadPopOuts = checkBoxAutoLoad.Checked;
            EDDiscoveryForm.EDDConfig.AutoSavePopOuts = checkBoxAutoSave.Checked;
            EDDiscoveryForm.EDDConfig.ShowUIEvents = checkBoxShowUIEvents.Checked;
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

            if (cf.ShowDialog(FindForm()) == DialogResult.OK)
            {
                if (cf.Valid && !EDCommander.IsCommanderPresent(cf.CommanderName))
                {
                    EDCommander cmdr = new EDCommander();
                    cf.Update(cmdr);
                    EDCommander.Create(cmdr);
                    UpdateCommandersListBox();
                    _discoveryForm.LoadCommandersListBox();
                    _discoveryForm.RefreshHistoryAsync();           // will do a new parse on commander list adding/removing scanners
                    btnDeleteCommander.Enabled = EDCommander.NumberOfCommanders > 1;
                }
                else
                    ExtendedControls.MessageBoxTheme.Show(FindForm(), "Command name is not valid or duplicate" , "Cannot create Commander", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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

                if (cf.ShowDialog(FindForm()) == DialogResult.OK)
                {
                    cf.Update(cmdr);
                    List<EDCommander> edcommanders = (List<EDCommander>)dataGridViewCommanders.DataSource;
                    _discoveryForm.LoadCommandersListBox();
                    EDCommander.Update(edcommanders, false);
                }

                _discoveryForm.Capi.Logout();       // logout.. CAPI may have changed
                _discoveryForm.RefreshHistoryAsync();           // do a resync, CAPI may have changed, anything else, make it work again
            }
        }

        private void btnDeleteCommander_Click(object sender, EventArgs e)
        {
            if (dataGridViewCommanders.CurrentCell != null)
            {
                int row = dataGridViewCommanders.CurrentCell.RowIndex;
                EDCommander cmdr = dataGridViewCommanders.Rows[row].DataBoundItem as EDCommander;

                var result = ExtendedControls.MessageBoxTheme.Show(FindForm(), "Do you wish to delete commander " + cmdr.Name + "?", "Delete commander", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    EDCommander.Delete(cmdr);
                    _discoveryForm.LoadCommandersListBox();
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
            if (mapColorDialog.ShowDialog(FindForm()) == DialogResult.OK)
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
                DialogResult res = ExtendedControls.MessageBoxTheme.Show(FindForm(), "The font used by this theme is not available on your system" + Environment.NewLine +
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

            dlg.InitialDirectory = Path.Combine(EDDOptions.Instance.AppDataDirectory, "Theme");
            dlg.DefaultExt = "eddtheme";
            dlg.AddExtension = true;

            if (dlg.ShowDialog(FindForm()) == DialogResult.OK)
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

        public void button_edittheme_Click(object sender, EventArgs e)
        {
            var themeeditor = new ExtendedControls.ThemeStandardEditor();
            themeeditor.ApplyChanges += () => _discoveryForm.ApplyTheme();
            themeeditor.InitForm();
            themeeditor.ShowDialog(FindForm());                     // run form
            themeeditor.Dispose();

            SetEntryThemeComboBox();
            buttonSaveTheme.Enabled = true;
        }

        private void checkBoxKeepOnTop_CheckedChanged(object sender, EventArgs e)
        {
            EDDConfig.Instance.KeepOnTop = checkBoxKeepOnTop.Checked;
            this.FindForm().TopMost = checkBoxKeepOnTop.Checked;
            _discoveryForm.keepOnTopChanged(checkBoxKeepOnTop.Checked);
        }

        private void checkBoxShowUIEvents_CheckedChanged(object sender, EventArgs e)
        {
            EDDConfig.Instance.ShowUIEvents = checkBoxShowUIEvents.Checked;
        }

        private void checkBoxOrderRowsInverted_CheckedChanged(object sender, EventArgs e)
        {
            EDDConfig.Instance.OrderRowsInverted = checkBoxOrderRowsInverted.Checked;
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

        private void comboBoxClickThruKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExtendedControls.ComboBoxCustom c = sender as ExtendedControls.ComboBoxCustom;
            Keys k = c.Text.ToVkey();
            EDDConfig.Instance.ClickThruKey = k;
        }

        private void buttonExtScreenshot_Click(object sender, EventArgs e)
        {
            ScreenShots.ScreenShotConfigureForm frm = new ScreenShots.ScreenShotConfigureForm();
            frm.Init(_discoveryForm.screenshotconverter, _discoveryForm.screenshotconverter.MarkHiRes);

            if ( frm.ShowDialog(FindForm()) == DialogResult.OK )
            {
                _discoveryForm.screenshotconverter.Stop();
                _discoveryForm.screenshotconverter.ScreenshotsDir = frm.ScreenshotsDir;
                _discoveryForm.screenshotconverter.OutputDir = frm.OutputDir;
                _discoveryForm.screenshotconverter.InputFileExtension = frm.InputFileExtension;
                _discoveryForm.screenshotconverter.OutputFileExtension = frm.OutputFileExtension;
                _discoveryForm.screenshotconverter.FolderNameFormat = frm.FolderNameFormat;
                _discoveryForm.screenshotconverter.FileNameFormat = frm.FileNameFormat;
                _discoveryForm.screenshotconverter.CropImage = frm.CropImage;
                _discoveryForm.screenshotconverter.CropArea = frm.CropArea;
                _discoveryForm.screenshotconverter.Start();
            }
        }

        private void checkBoxCustomEnableScreenshots_CheckedChanged(object sender, EventArgs e)
        {
            _discoveryForm.screenshotconverter.AutoConvert = checkBoxCustomEnableScreenshots.Checked; 
        }

        private void checkBoxCustomRemoveOriginals_CheckedChanged(object sender, EventArgs e)
        {
            _discoveryForm.screenshotconverter.RemoveOriginal = checkBoxCustomRemoveOriginals.Checked; 
        }

        private void checkBoxCustomMarkHiRes_CheckedChanged(object sender, EventArgs e)
        {
            _discoveryForm.screenshotconverter.MarkHiRes = checkBoxCustomMarkHiRes.Checked;
        }

        private void checkBoxCustomCopyToClipboard_CheckedChanged(object sender, EventArgs e)
        {
            _discoveryForm.screenshotconverter.CopyToClipboard = checkBoxCustomCopyToClipboard.Checked;
        }
    }
}

