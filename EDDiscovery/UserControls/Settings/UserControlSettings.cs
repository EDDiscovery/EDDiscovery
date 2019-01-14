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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using EDDiscovery.Forms;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using System.Threading.Tasks;

namespace EDDiscovery.UserControls
{
    public partial class UserControlSettings : UserControlCommonBase
    {
        private ExtendedControls.ThemeStandardEditor themeeditor = null;

        public UserControlSettings()
        {
            InitializeComponent();
            var corner = dataGridViewCommanders.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            ResetThemeList();
            SetEntryThemeComboBox();

            textBoxHomeSystem.SetAutoCompletor(SystemClassDB.ReturnSystemListForAutoComplete);
            
            btnDeleteCommander.Enabled = EDCommander.NumberOfCommanders > 1;

            comboBoxClickThruKey.Items = KeyObjectExtensions.KeyListString(inclshifts:true);
            comboBoxClickThruKey.SelectedItem = EDDConfig.Instance.ClickThruKey.VKeyToString();
            comboBoxClickThruKey.SelectedIndexChanged += comboBoxClickThruKey_SelectedIndexChanged;

            comboBoxCustomLanguage.Items.AddRange(BaseUtils.Translator.EnumerateLanguageNames( EDDOptions.Instance.TranslatorFolders() ));

            comboBoxCustomLanguage.Items.Add("Auto");
            comboBoxCustomLanguage.Items.Add("Default (English)");
            if (comboBoxCustomLanguage.Items.Contains(EDDConfig.Instance.Language))
                comboBoxCustomLanguage.SelectedItem = EDDConfig.Instance.Language;
            else
                comboBoxCustomLanguage.SelectedIndex = comboBoxCustomLanguage.Items.Count - 1;
            comboBoxCustomLanguage.SelectedIndexChanged += ComboBoxCustomLanguage_SelectedIndexChanged;

            discoveryform.OnRefreshCommanders += DiscoveryForm_OnRefreshCommanders;

            checkBoxOrderRowsInverted.Checked = EDDiscoveryForm.EDDConfig.OrderRowsInverted;
            checkBoxMinimizeToNotifyIcon.Checked = EDDiscoveryForm.EDDConfig.MinimizeToNotifyIcon;
            checkBoxKeepOnTop.Checked = EDDiscoveryForm.EDDConfig.KeepOnTop;
            checkBoxPanelSortOrder.Checked = EDDConfig.Instance.SortPanelsByName;
            checkBoxUseNotifyIcon.Checked = EDDiscoveryForm.EDDConfig.UseNotifyIcon;
            checkBoxUTC.Checked = EDDiscoveryForm.EDDConfig.DisplayUTC;
            checkBoxShowUIEvents.Checked = EDDiscoveryForm.EDDConfig.ShowUIEvents;
            checkBoxCustomResize.Checked = EDDiscoveryForm.EDDConfig.DrawDuringResize;

            checkBoxMinimizeToNotifyIcon.Enabled = EDDiscoveryForm.EDDConfig.UseNotifyIcon;

            textBoxHomeSystem.Text = EDDConfig.Instance.HomeSystem.Name;

            textBoxDefaultZoom.ValueNoChange = EDDConfig.Instance.MapZoom;

            bool selectionCentre = EDDConfig.Instance.MapCentreOnSelection;
            radioButtonHistorySelection.Checked = selectionCentre;
            radioButtonCentreHome.Checked = !selectionCentre;

            dataGridViewCommanders.AutoGenerateColumns = false;             // BEFORE assigned to list..
            dataGridViewCommanders.DataSource = EDCommander.GetListCommanders();

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

            checkBoxCustomEDSMEDDBDownload.Checked = EDDConfig.Instance.EDSMEDDBDownload;
            this.checkBoxCustomEDSMEDDBDownload.CheckedChanged += new System.EventHandler(this.checkBoxCustomEDSMDownload_CheckedChanged);

            comboBoxCustomHistoryLoadTime.Items = new string[] { "Disabled-Load All".Tx(this,"DLA"), ">7 days old".Tx(this), ">30 days old".Tx(this), ">60 days old".Tx(this), ">90 days old".Tx(this), ">180 days old".Tx(this), ">270 days old".Tx(this), "> 365 days old".Tx(this) };
            comboBoxCustomHistoryLoadTime.Tag = new int[] { 0, 7, 30, 60, 90, 180, 270, 365 };
            int ix = Array.FindIndex(comboBoxCustomHistoryLoadTime.Tag as int[], x => x == EDDConfig.Instance.FullHistoryLoadDayLimit);
            comboBoxCustomHistoryLoadTime.SelectedIndex = ix >= 0 ? ix : 0;
            comboBoxCustomHistoryLoadTime.SelectedIndexChanged += ComboBoxCustomHistoryLoadTime_SelectedIndexChanged;

            var eetn = new string[] { nameof(JournalEntry.EssentialEvents), nameof(JournalEntry.FullStatsEssentialEvents) , nameof(JournalEntry.JumpScanEssentialEvents), nameof(JournalEntry.JumpEssentialEvents), nameof(JournalEntry.NoEssentialEvents)};
            comboBoxCustomEssentialEntries.Items = new string[] { "Scans,Cargo,Missions,State,Jumps etc".Tx(this, "ESM"), "All entries for Statistics".Tx(this, "FS"), "Jumps and Scans".Tx(this, "EJS"), "Jumps".Tx(this, "EJ"), "Nothing".Tx(this, "EN") };
            comboBoxCustomEssentialEntries.Tag = eetn;
            ix = Array.FindIndex(eetn, x => x == EDDConfig.Instance.EssentialEventTypes);
            comboBoxCustomEssentialEntries.SelectedIndex = ix >= 0 ? ix : 0;
            comboBoxCustomEssentialEntries.SelectedIndexChanged += ComboBoxCustomEssentialEntries_SelectedIndexChanged;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolTip,this);
        }

        public override void InitialDisplay()
        {
        }

        public override void Closing()
        {
            if (textBoxHomeSystem.Text != EDDiscoveryForm.EDDConfig.HomeSystem.Name)
                ValidateAndSaveHomeSystem();     // make sure any change is persisted

            discoveryform.OnRefreshCommanders -= DiscoveryForm_OnRefreshCommanders;

            themeeditor?.Dispose();
            var frm = FindForm();
            if (typeof(ExtendedControls.SmartSysMenuForm).IsAssignableFrom(frm?.GetType()))
                (frm as ExtendedControls.SmartSysMenuForm).TopMostChanged -= ParentForm_TopMostChanged;
        }

        private void DiscoveryForm_OnRefreshCommanders()
        {
            UpdateCommandersListBox();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var frm = FindForm();
            if (typeof(ExtendedControls.SmartSysMenuForm).IsAssignableFrom(frm?.GetType()))
                (frm as ExtendedControls.SmartSysMenuForm).TopMostChanged += ParentForm_TopMostChanged;
        }

        #region Commanders

        public void UpdateCommandersListBox()
        {
            dataGridViewCommanders.DataSource = null;
            dataGridViewCommanders.DataSource = EDCommander.GetListCommanders();
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
                    discoveryform.LoadCommandersListBox();
                    discoveryform.RefreshHistoryAsync();           // will do a new parse on commander list adding/removing scanners
                    btnDeleteCommander.Enabled = EDCommander.NumberOfCommanders > 1;
                }
                else
                    ExtendedControls.MessageBoxTheme.Show(FindForm(), "Commander name is not valid or duplicate".Tx(this,"AddC") , "Cannot create Commander".Tx(this,"AddT"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
                    discoveryform.LoadCommandersListBox();
                    EDCommander.Update(edcommanders, false);
                }

                // CAPI Descoped discoveryform.Capi.Logout();       // logout.. CAPI may have changed
                discoveryform.RefreshHistoryAsync();           // do a resync, CAPI may have changed, anything else, make it work again
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
                    discoveryform.LoadCommandersListBox();
                    UpdateCommandersListBox();
                    discoveryform.RefreshHistoryAsync();           // will do a new parse on commander list adding/removing scanners

                    btnDeleteCommander.Enabled = EDCommander.NumberOfCommanders > 1;
                }
            }
        }


        #endregion

        #region History

        private void checkBoxShowUIEvents_CheckedChanged(object sender, EventArgs e)
        {
            EDDConfig.Instance.ShowUIEvents = checkBoxShowUIEvents.Checked;
        }

        private void checkBoxOrderRowsInverted_CheckedChanged(object sender, EventArgs e)
        {
            EDDConfig.Instance.OrderRowsInverted = checkBoxOrderRowsInverted.Checked;
        }

        private void checkBoxUTC_CheckedChanged(object sender, EventArgs e)
        {
            EDDiscoveryForm.EDDConfig.DisplayUTC = checkBoxUTC.Checked;
            discoveryform.RefreshDisplays();
        }

        private void ComboBoxCustomHistoryLoadTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCustomHistoryLoadTime.SelectedIndex >= 0)       // paranoia
            {
                EDDConfig.Instance.FullHistoryLoadDayLimit = (comboBoxCustomHistoryLoadTime.Tag as int[])[comboBoxCustomHistoryLoadTime.SelectedIndex];
                discoveryform.RefreshHistoryAsync();
            }
        }

        private void ComboBoxCustomEssentialEntries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCustomEssentialEntries.SelectedIndex >= 0)       // paranoia
            {
                string[] tn = comboBoxCustomEssentialEntries.Tag as string[];
                EDDConfig.Instance.EssentialEventTypes = tn[comboBoxCustomEssentialEntries.SelectedIndex];
                discoveryform.RefreshHistoryAsync();
            }
        }


        #endregion

        #region Theme

        private void comboBoxTheme_SelectedIndexChanged(object sender, EventArgs e) // theme selected..
        {
            string themename = comboBoxTheme.Items[comboBoxTheme.SelectedIndex].ToString();

            string fontwanted = null;                                               // don't check custom, only a stored theme..
            if (!themename.Equals("Custom") && !discoveryform.theme.IsFontAvailableInTheme(themename, out fontwanted))
            {
                string warning = string.Format(
                      ("The font used by this theme is not available on your system." + Environment.NewLine +
                      "The font needed is \"{0}\"." + Environment.NewLine +
                      "Install this font to fully use this theme." + Environment.NewLine +
                      "Euro Caps font is freely available from www.edassets.org." + Environment.NewLine +
                      "and is in your install folder " + Path.GetDirectoryName(Application.ExecutablePath) + " - install it manually" + Environment.NewLine +
                      Environment.NewLine +
                      "Would you like to load this theme using a replacement font?").Tx(this, "Font"), fontwanted);

                DialogResult res = ExtendedControls.MessageBoxTheme.Show(FindForm(), warning, "Warning".Tx(), MessageBoxButtons.YesNo);

                if (res != DialogResult.Yes)
                {
                    // Reset the combo box to the previous theme name and don't change anything else.
                    SetEntryThemeComboBox();
                    return;
                }
            }

            if (!discoveryform.theme.SetThemeByName(themename))
                discoveryform.theme.SetCustom();                                   // go to custom theme..

            SetEntryThemeComboBox();
            discoveryform.ApplyTheme();
        }

        private void buttonSaveTheme_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.InitialDirectory = EDDOptions.Instance.ThemeAppDirectory();
            dlg.DefaultExt = "eddtheme";
            dlg.AddExtension = true;

            if (dlg.ShowDialog(FindForm()) == DialogResult.OK)
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
                themeeditor = new ExtendedControls.ThemeStandardEditor() { TopMost = FindForm().TopMost };
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
            comboBoxTheme.Enabled = true;               // no doing this while theme editor is open
            buttonSaveTheme.Enabled = true;
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

        #endregion

        #region Screenshots

        private void buttonExtScreenshot_Click(object sender, EventArgs e)
        {
            ScreenShots.ScreenShotConfigureForm frm = new ScreenShots.ScreenShotConfigureForm();
            frm.Init(discoveryform.screenshotconverter, discoveryform.screenshotconverter.MarkHiRes);

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
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

        #endregion


        #region Language

        private void ComboBoxCustomLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExtendedControls.ComboBoxCustom c = sender as ExtendedControls.ComboBoxCustom;
            EDDConfig.Instance.Language = c.Items[c.SelectedIndex];
            ExtendedControls.MessageBoxTheme.Show(this, "Applies at next restart of ED Discovery".Tx(this, "Language"), "Information".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion


        #region 3dmap

        private void textBoxDefaultZoom_ValueChanged(object sender, EventArgs e)
        {
            EDDConfig.Instance.MapZoom = (float)textBoxDefaultZoom.Value;
        }

        private void radioButtonCentreHome_CheckedChanged(object sender, EventArgs e)
        {
            EDDConfig.Instance.MapCentreOnSelection = radioButtonHistorySelection.Checked;
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

        private void textBoxHomeSystem_Leave(object sender, EventArgs e)
        {
            ValidateAndSaveHomeSystem();
        }

        private void ValidateAndSaveHomeSystem()
        {
            string t = textBoxHomeSystem.Text.Trim();
            ISystem s = SystemClassDB.GetSystem(t);

            if (s != null)
            {
                textBoxHomeSystem.Text = s.Name;
                EDDConfig.Instance.HomeSystem = s;
            }
            else
                textBoxHomeSystem.Text = EDDConfig.Instance.HomeSystem.Name;
        }

        #endregion

        #region window options

        private void comboBoxClickThruKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExtendedControls.ComboBoxCustom c = sender as ExtendedControls.ComboBoxCustom;
            Keys k = c.Text.ToVkey();
            EDDConfig.Instance.ClickThruKey = k;
        }

        private void checkBoxMinimizeToNotifyIcon_CheckedChanged(object sender, EventArgs e)
        {
            EDDiscoveryForm.EDDConfig.MinimizeToNotifyIcon = checkBoxMinimizeToNotifyIcon.Checked;
        }

        private void checkBoxCustomResize_CheckedChanged(object sender, EventArgs e)
        {
            EDDiscoveryForm.EDDConfig.DrawDuringResize = checkBoxCustomResize.Checked;
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

        private void checkBoxKeepOnTop_CheckedChanged(object sender, EventArgs e)
        {
            var frm = FindForm();

            EDDConfig.Instance.KeepOnTop = frm.TopMost = checkBoxKeepOnTop.Checked;
            if (themeeditor != null)
                themeeditor.TopMost = checkBoxKeepOnTop.Checked;
        }

        private void ParentForm_TopMostChanged(object sender, EventArgs e)
        {
            checkBoxKeepOnTop.Checked = (sender as Form).TopMost;
        }

        private void checkBoxPanelSortOrder_CheckedChanged(object sender, EventArgs e)
        {
            EDDConfig.Instance.SortPanelsByName = checkBoxPanelSortOrder.Checked;
        }

        #endregion

        #region EDDB EDSM

        private void checkBoxCustomEDSMDownload_CheckedChanged(object sender, EventArgs e)
        {
            EDDConfig.Instance.EDSMEDDBDownload = checkBoxCustomEDSMEDDBDownload.Checked;
        }

        ExtendedControls.InfoForm info;
        System.Windows.Forms.Timer removetimer;

        private void buttonExtEDSMConfigureArea_Click(object sender, EventArgs e)
        {
            GalaxySectorSelect gss = new GalaxySectorSelect();

            if (!gss.Init(EDDConfig.Instance.EDSMGridIDs))
            {
                ExtendedControls.MessageBoxTheme.Show(this, "Failed", "No map downloaded - please wait for it to download");
            }
            else if (gss.ShowDialog() == DialogResult.OK)
            {
                EDDConfig.Instance.EDSMGridIDs = gss.Selection;

                if (gss.Action == GalaxySectorSelect.ActionToDo.Add)
                {
                    discoveryform.ForceEDSMEDDBFullRefresh();
                }
                else if (gss.Action == GalaxySectorSelect.ActionToDo.Remove)
                {
                    System.Diagnostics.Debug.WriteLine("Remove ");

                    info = new ExtendedControls.InfoForm();
                    info.Info("Remove Sectors".Tx(this), EDDiscovery.Properties.Resources.edlogo_3mo_icon,
                                string.Format( ("Removing {0} Sector(s)." + Environment.NewLine + Environment.NewLine +
                                "This will take a while (up to 30 mins dep on drive type and amount of sectors)." + Environment.NewLine +
                                "You may continue to use EDD while this operation takes place" + Environment.NewLine +
                                "but it may be slow to respond. Do not close down EDD until this window says" + Environment.NewLine +
                                "the process has finished" + Environment.NewLine + Environment.NewLine).Tx(this,"GalRemove"), gss.Removed.Count ));
                    info.EnableClose = false;
                    info.Show(discoveryform);

                    taskremovesectors = Task.Factory.StartNew(() => RemoveSectors(gss.AllRemoveSectors, (s) => discoveryform.Invoke(new Action(() => { info.AddText(s); }))));

                    removetimer = new Timer() { Interval = 200 };
                    removetimer.Tick += Removetimer_Tick;
                    removetimer.Start();
                }
            }
        }

        public static void RemoveSectors(List<int> sectors, Action<string> inform)
        {
            inform("Removing Names" + Environment.NewLine);
            SystemClassDB.RemoveGridNames(sectors, inform);     // MUST do first as relies on system grid for info
            inform("Removing System Information" + Environment.NewLine);
            SystemClassDB.RemoveGridSystems(sectors, inform);
            inform("Vacuum Database for size" + Environment.NewLine);
            SystemClassDB.Vacuum();
        }

        private Task taskremovesectors = null;

        private void Removetimer_Tick(object sender, EventArgs e)
        {
            if (taskremovesectors.Status == TaskStatus.RanToCompletion)
            {
                removetimer.Stop();
                taskremovesectors.Dispose();
                info.EnableClose = true;
                info.AddText(("Finished, Please close the window." + Environment.NewLine +
                    "If you already have the 3dmap open, changes will not be reflected in that map until the next start of EDD" + Environment.NewLine).Tx(this,"GalFini"));
            }
        }

        #endregion

        #region Safemode

        private void buttonExtSafeMode_Click(object sender, EventArgs e)
        {
            if (ExtendedControls.MessageBoxTheme.Show(this, "Safe Mode".Tx(this,"SM"), "Confirm restart to safe mode".Tx(this, "CSM"), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                Application.Exit();
                System.Diagnostics.Process.Start(Application.ExecutablePath, "-safemode");
            }
        }

        #endregion
    }
}

