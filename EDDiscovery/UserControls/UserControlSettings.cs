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

namespace EDDiscovery.UserControls
{
    public partial class UserControlSettings : UserControlCommonBase
    {
        private EDDiscoveryForm discoveryform;
        private ExtendedControls.ThemeStandardEditor themeeditor = null;

        public UserControlSettings()
        {
            InitializeComponent();
        }

        public override void Init(EDDiscoveryForm discoveryForm, UserControlCursorType uctgnotused, int displaynumbernotused)
        {
            discoveryform = discoveryForm;

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
            int i = discoveryform.theme.GetIndexOfCurrentTheme();
            if (i == -1)
                comboBoxTheme.SelectedItem = "Custom";
            else
                comboBoxTheme.SelectedIndex = i;
        }

        private void ResetThemeList()
        {
            comboBoxTheme.Items = discoveryform.theme.GetThemeList();
            comboBoxTheme.Items.Add("Custom");
        }

        public override void InitialDisplay()
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

            textBoxHomeSystem.Text = EDDConfig.Instance.HomeSystem.name;

            textBoxDefaultZoom.Text = EDDConfig.Instance.MapZoom.ToString();

            bool selectionCentre = EDDConfig.Instance.MapCentreOnSelection;
            radioButtonHistorySelection.Checked = selectionCentre;
            radioButtonCentreHome.Checked = !selectionCentre;

            dataGridViewCommanders.AutoGenerateColumns = false;             // BEFORE assigned to list..
            dataGridViewCommanders.DataSource = EDCommander.GetList();

            panel_defaultmapcolor.BackColor = Color.FromArgb(EDDConfig.Instance.DefaultMapColour);

            this.comboBoxTheme.SelectedIndexChanged += this.comboBoxTheme_SelectedIndexChanged;    // now turn on the handler..

            checkBoxCustomRemoveOriginals.Checked = discoveryform.screenshotconverter.RemoveOriginal;
            checkBoxCustomMarkHiRes.Checked = discoveryform.screenshotconverter.MarkHiRes;
            checkBoxCustomEnableScreenshots.Checked = discoveryform.screenshotconverter.AutoConvert;
            checkBoxCustomCopyToClipboard.Checked = discoveryform.screenshotconverter.CopyToClipboard;

            this.checkBoxCustomRemoveOriginals.CheckedChanged += new System.EventHandler(this.checkBoxCustomRemoveOriginals_CheckedChanged);
            this.checkBoxCustomMarkHiRes.CheckedChanged += new System.EventHandler(this.checkBoxCustomMarkHiRes_CheckedChanged);
            this.checkBoxCustomEnableScreenshots.CheckedChanged += new System.EventHandler(this.checkBoxCustomEnableScreenshots_CheckedChanged);
            this.checkBoxCustomCopyToClipboard.CheckedChanged += new System.EventHandler(this.checkBoxCustomCopyToClipboard_CheckedChanged);
        }

        public override void Closing()
        {
            EDDiscoveryForm.EDDConfig.AutoLoadPopOuts = checkBoxAutoLoad.Checked;   // ok to do here..
            EDDiscoveryForm.EDDConfig.AutoSavePopOuts = checkBoxAutoSave.Checked;
        }

        private void textBoxHomeSystem_Validated(object sender, EventArgs e)
        {
            string t = textBoxHomeSystem.Text.Trim();
            ISystem s = SystemClassDB.GetSystem(t);

            if (s != null)
            {
                textBoxHomeSystem.Text = s.name;
                EDDConfig.Instance.HomeSystem = s;
            }
            else
                textBoxHomeSystem.Text = EDDConfig.Instance.HomeSystem.name;
        }

        private void textBoxDefaultZoom_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            float? v = textBoxDefaultZoom.Text.InvariantParseFloatNull();
            if (v != null)
            {
                textBoxDefaultZoom.Text = v.Value.ToStringInvariant();
                EDDConfig.Instance.MapZoom = v.Value;
            }
            else
                textBoxDefaultZoom.Text = EDDConfig.Instance.MapZoom.ToStringInvariant();
        }

        private void radioButtonCentreHome_CheckedChanged(object sender, EventArgs e)
        {
            EDDConfig.Instance.MapCentreOnSelection = radioButtonHistorySelection.Checked;
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
                    discoveryform.LoadCommandersListBox();
                    discoveryform.RefreshHistoryAsync();           // will do a new parse on commander list adding/removing scanners
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
                    cf.Update(cmdr);
                    List<EDCommander> edcommanders = (List<EDCommander>)dataGridViewCommanders.DataSource;
                    discoveryform.LoadCommandersListBox();
                    EDCommander.Update(edcommanders, false);
                }

                discoveryform.Capi.Logout();       // logout.. CAPI may have changed
                discoveryform.RefreshHistoryAsync();           // do a resync, CAPI may have changed, anything else, make it work again
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
                    discoveryform.LoadCommandersListBox();
                    UpdateCommandersListBox();
                    discoveryform.RefreshHistoryAsync();           // will do a new parse on commander list adding/removing scanners

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
            if (!themename.Equals("Custom") && !discoveryform.theme.IsFontAvailableInTheme(themename, out fontwanted))
            {
                DialogResult res = ExtendedControls.MessageBoxTheme.Show("The font used by this theme is not available on your system" + Environment.NewLine +
                      "The font needed is \"" + fontwanted + "\"" + Environment.NewLine +
                      "Install this font and you can use this scheme." + Environment.NewLine +
                      "EuroCaps font is available www.edassets.org.",
                      "Warning", MessageBoxButtons.OK);

                discoveryform.theme.SetCustom();                              // go to custom theme whatever
                SetEntryThemeComboBox();
                return;
            }

            if (!discoveryform.theme.SetThemeByName(themename))
                discoveryform.theme.SetCustom();                                   // go to custom theme..

            SetEntryThemeComboBox();
            discoveryform.ApplyTheme();
        }

        private void buttonSaveTheme_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.InitialDirectory = Path.Combine(EDDOptions.Instance.AppDataDirectory, "Theme");
            dlg.DefaultExt = "eddtheme";
            dlg.AddExtension = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                discoveryform.theme.SaveSettings(dlg.FileName);        // should create a new theme files
                discoveryform.theme.LoadThemes();          // make sure up to data - we added a theme, reload them all
                discoveryform.theme.Name = Path.GetFileNameWithoutExtension(dlg.FileName); // go to the theme name

                ResetThemeList();

                int curindex = discoveryform.theme.GetIndexOfCurrentTheme();       // get theme index.. may be -1 if theme not loaded back

                if (curindex == -1)                                   // if not loaded, back to custom
                    discoveryform.theme.SetCustom();   // custom

                SetEntryThemeComboBox();
            }
        }

        public void UpdateThemeChanges()
        {
            discoveryform.ApplyTheme();
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

        private void checkBoxKeepOnTop_CheckedChanged(object sender, EventArgs e)
        {
            EDDConfig.Instance.KeepOnTop = checkBoxKeepOnTop.Checked;
            this.FindForm().TopMost = checkBoxKeepOnTop.Checked;
            discoveryform.keepOnTopChanged(checkBoxKeepOnTop.Checked);
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

        public void DisableNotifyIcon()
        {
            checkBoxUseNotifyIcon.Checked = false;
        }

        private void checkBoxUseNotifyIcon_CheckedChanged(object sender, EventArgs e)
        {
            bool chk = checkBoxUseNotifyIcon.Checked;
            EDDiscoveryForm.EDDConfig.UseNotifyIcon = chk;
            checkBoxMinimizeToNotifyIcon.Enabled = chk;
            discoveryform.useNotifyIconChanged(chk);
        }

        private void checkBoxUTC_CheckedChanged(object sender, EventArgs e)
        {
            EDDiscoveryForm.EDDConfig.DisplayUTC = checkBoxUTC.Checked;
            discoveryform.RefreshDisplays();
        }

        private void buttonSaveSetup_Click(object sender, EventArgs e)
        {
            discoveryform.SaveCurrentPopOuts();
        }

        private void buttonReloadSaved_Click(object sender, EventArgs e)
        {
            discoveryform.LoadSavedPopouts();
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
            frm.Init(discoveryform.screenshotconverter, discoveryform.screenshotconverter.MarkHiRes);

            if ( frm.ShowDialog() == DialogResult.OK )
            {
                discoveryform.screenshotconverter.Stop();
                discoveryform.screenshotconverter.ScreenshotsDir = frm.ScreenshotsDir;
                discoveryform.screenshotconverter.OutputDir = frm.OutputDir;
                discoveryform.screenshotconverter.InputFileExtension = frm.InputFileExtension;
                discoveryform.screenshotconverter.OutputFileExtension = frm.OutputFileExtension;
                discoveryform.screenshotconverter.FolderNameFormat = frm.FolderNameFormat;
                discoveryform.screenshotconverter.FileNameFormat = frm.FileNameFormat;
                discoveryform.screenshotconverter.CropImage = frm.CropImage;
                discoveryform.screenshotconverter.CropArea = frm.CropArea;
                discoveryform.screenshotconverter.Start();
            }
        }

        private void checkBoxCustomEnableScreenshots_CheckedChanged(object sender, EventArgs e)
        {
            discoveryform.screenshotconverter.AutoConvert = checkBoxCustomEnableScreenshots.Checked; 
        }

        private void checkBoxCustomRemoveOriginals_CheckedChanged(object sender, EventArgs e)
        {
            discoveryform.screenshotconverter.RemoveOriginal = checkBoxCustomRemoveOriginals.Checked; 
        }

        private void checkBoxCustomMarkHiRes_CheckedChanged(object sender, EventArgs e)
        {
            discoveryform.screenshotconverter.MarkHiRes = checkBoxCustomMarkHiRes.Checked;
        }

        private void checkBoxCustomCopyToClipboard_CheckedChanged(object sender, EventArgs e)
        {
            discoveryform.screenshotconverter.CopyToClipboard = checkBoxCustomCopyToClipboard.Checked;
        }

        private void checkBoxEDSMLog_CheckStateChanged(object sender, EventArgs e)
        {
            EDDiscoveryForm.EDDConfig.EDSMLog = checkBoxEDSMLog.Checked;
            discoveryform.SetUpLogging();
        }
    }
}

