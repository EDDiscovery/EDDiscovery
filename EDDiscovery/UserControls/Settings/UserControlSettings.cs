/*
 * Copyright © 2016 - 2019 EDDiscovery development team
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
        Timer tm = new Timer();

        public UserControlSettings()
        {
            InitializeComponent();
            var corner = dataGridViewCommanders.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            extComboBoxGameTime.Items.Add("Local".T(EDTx.UserControlSettings_Local));
            extComboBoxGameTime.Items.Add("UTC");
            extComboBoxGameTime.Items.Add("Game Time".T(EDTx.UserControlSettings_GameTime));

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);

            ResetThemeList();
            SetEntryThemeComboBox();

            btnDeleteCommander.Enabled = EDCommander.NumberOfCommanders > 1;

            comboBoxClickThruKey.Items = KeyObjectExtensions.KeyListString(inclshifts: true);
            comboBoxClickThruKey.SelectedItem = EDDConfig.Instance.ClickThruKey.VKeyToString();
            comboBoxClickThruKey.SelectedIndexChanged += comboBoxClickThruKey_SelectedIndexChanged;

            comboBoxCustomLanguage.Items.AddRange(BaseUtils.Translator.EnumerateLanguageNames(EDDOptions.Instance.TranslatorFolders()));

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
            checkBoxCustomResize.Checked = EDDiscoveryForm.EDDConfig.DrawDuringResize;

            extComboBoxGameTime.SelectedIndex = EDDiscoveryForm.EDDConfig.DisplayTimeIndex;

            checkBoxOrderRowsInverted.CheckedChanged += checkBoxOrderRowsInverted_CheckedChanged;
            checkBoxMinimizeToNotifyIcon.CheckedChanged += checkBoxMinimizeToNotifyIcon_CheckedChanged;
            checkBoxKeepOnTop.CheckedChanged += checkBoxKeepOnTop_CheckedChanged;
            checkBoxPanelSortOrder.CheckedChanged += checkBoxPanelSortOrder_CheckedChanged;
            checkBoxUseNotifyIcon.CheckedChanged += checkBoxUseNotifyIcon_CheckedChanged;
            extComboBoxGameTime.SelectedIndexChanged += ExtComboBoxGameTime_SelectedIndexChanged;
            checkBoxCustomResize.CheckedChanged += checkBoxCustomResize_CheckedChanged;

            checkBoxMinimizeToNotifyIcon.Enabled = EDDiscoveryForm.EDDConfig.UseNotifyIcon;

            dataGridViewCommanders.AutoGenerateColumns = false;             // BEFORE assigned to list..
            dataGridViewCommanders.DataSource = EDCommander.GetListCommanders();

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

            comboBoxCustomHistoryLoadTime.Items = new string[] { "Disabled-Load All".T(EDTx.UserControlSettings_DLA), ">7 days old".T(EDTx.UserControlSettings_7daysold),
                ">30 days old".T(EDTx.UserControlSettings_30daysold), ">60 days old".T(EDTx.UserControlSettings_60daysold), ">90 days old".T(EDTx.UserControlSettings_90daysold),
                ">180 days old".T(EDTx.UserControlSettings_180daysold), ">270 days old".T(EDTx.UserControlSettings_270daysold), "> 365 days old".T(EDTx.UserControlSettings_365daysold) };

            comboBoxCustomHistoryLoadTime.Tag = new int[] { 0, 7, 30, 60, 90, 180, 270, 365 };
            int ix = Array.FindIndex(comboBoxCustomHistoryLoadTime.Tag as int[], x => x == EDDConfig.Instance.FullHistoryLoadDayLimit);
            comboBoxCustomHistoryLoadTime.SelectedIndex = ix >= 0 ? ix : 0;
            comboBoxCustomHistoryLoadTime.SelectedIndexChanged += ComboBoxCustomHistoryLoadTime_SelectedIndexChanged;

            var eetn = new string[] { nameof(JournalEssentialEvents.EssentialEvents), nameof(JournalEssentialEvents.FullStatsEssentialEvents), nameof(JournalEssentialEvents.JumpScanEssentialEvents), nameof(JournalEssentialEvents.JumpEssentialEvents), nameof(JournalEssentialEvents.NoEssentialEvents) };
            comboBoxCustomEssentialEntries.Items = new string[] { "Scans,Cargo,Missions,State,Jumps etc".T(EDTx.UserControlSettings_ESM), "All entries for Statistics".T(EDTx.UserControlSettings_FS),
                                        "Jumps and Scans".T(EDTx.UserControlSettings_EJS),
                                        "Jumps".T(EDTx.UserControlSettings_EJ), "Nothing".T(EDTx.UserControlSettings_EN) };

            comboBoxCustomEssentialEntries.Tag = eetn;
            ix = Array.FindIndex(eetn, x => x == EDDConfig.Instance.EssentialEventTypes);
            comboBoxCustomEssentialEntries.SelectedIndex = ix >= 0 ? ix : 0;
            comboBoxCustomEssentialEntries.SelectedIndexChanged += ComboBoxCustomEssentialEntries_SelectedIndexChanged;

            extCheckBoxWebServerEnable.Checked = false;
            extButtonTestWeb.Enabled = numberBoxLongPortNo.Enabled = false;
            numberBoxLongPortNo.Value = EDDConfig.Instance.WebServerPort;
            tm.Tick += PeriodicCheck;
            tm.Interval = 1000;
            tm.Start();

            extCheckBoxWebServerEnable.CheckedChanged += ExtCheckBoxWebServerEnable_CheckedChanged;
        }

        public override void InitialDisplay()
        {
        }

        public override void Closing()
        {
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
            int selrow = dataGridViewCommanders.SelectedRows.Count > 0 ? dataGridViewCommanders.SelectedRows[0].Index : -1;
            dataGridViewCommanders.DataSource = null;
            List<EDCommander> cmdrs = EDCommander.GetListCommanders();
            dataGridViewCommanders.DataSource = cmdrs;
            if (selrow >= 0 && selrow < dataGridViewCommanders.RowCount)
                dataGridViewCommanders.Rows[selrow].Selected = true;
            dataGridViewCommanders.Update();
        }

        private void buttonAddCommander_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.Forms.CommanderForm cf = new EliteDangerousCore.Forms.CommanderForm();
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
                    ExtendedControls.MessageBoxTheme.Show(FindForm(), "Commander name is not valid or duplicate".T(EDTx.UserControlSettings_AddC) , "Cannot create Commander".T(EDTx.UserControlSettings_AddT), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void buttonEditCommander_Click(object sender, EventArgs e)
        {
            if (dataGridViewCommanders.SelectedRows.Count > 0)
            {
                int row = dataGridViewCommanders.SelectedRows[0].Index;
                EDCommander cmdr = dataGridViewCommanders.Rows[row].DataBoundItem as EDCommander;

                EliteDangerousCore.Forms.CommanderForm cf = new EliteDangerousCore.Forms.CommanderForm();
                cf.Init(cmdr,false);

                if (cf.ShowDialog(FindForm()) == DialogResult.OK)
                {
                    bool forceupdate = cf.Update(cmdr);
                    List<EDCommander> edcommanders = (List<EDCommander>)dataGridViewCommanders.DataSource;
                    discoveryform.LoadCommandersListBox();
                    EDCommander.Update(edcommanders, false);
                    dataGridViewCommanders.Refresh();

                    if ( forceupdate )                  // journal loc change forcing update
                        discoveryform.RefreshHistoryAsync();        // do a resync
                }
            }
        }

        private void btnDeleteCommander_Click(object sender, EventArgs e)
        {
            if (dataGridViewCommanders.SelectedRows.Count > 0)
            {
                int row = dataGridViewCommanders.SelectedRows[0].Index;
                EDCommander cmdr = dataGridViewCommanders.Rows[row].DataBoundItem as EDCommander;

                var result = ExtendedControls.MessageBoxTheme.Show(FindForm(), "Do you wish to delete commander ".T(EDTx.UserControlSettings_DelCmdr) + cmdr.Name + "?", "Warning".T(EDTx.Warning), MessageBoxButtons.YesNo, MessageBoxIcon.Information);

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

        private void checkBoxOrderRowsInverted_CheckedChanged(object sender, EventArgs e)
        {
            EDDConfig.Instance.OrderRowsInverted = checkBoxOrderRowsInverted.Checked;
        }

        private void ExtComboBoxGameTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            EDDiscoveryForm.EDDConfig.DisplayTimeIndex = extComboBoxGameTime.SelectedIndex;
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
                      "Would you like to load this theme using a replacement font?").T(EDTx.UserControlSettings_Font), fontwanted);

                DialogResult res = ExtendedControls.MessageBoxTheme.Show(FindForm(), warning, "Warning".T(EDTx.Warning), MessageBoxButtons.YesNo);

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
            discoveryform.ApplyTheme(true);
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

        public void button_edittheme_Click(object sender, EventArgs e)
        {
            if (themeeditor == null)                    // no theme editor, make one..
            {
                themeeditor = new ExtendedControls.ThemeStandardEditor() { TopMost = FindForm().TopMost };
                themeeditor.ApplyChanges = () => { discoveryform.ApplyTheme(true); };
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
                discoveryform.screenshotconverter.RemoveOriginal = frm.RemoveOriginal;
                discoveryform.screenshotconverter.FileNameFormat = frm.FileNameFormat;
                discoveryform.screenshotconverter.CropResize1 = frm.CropResize1;
                discoveryform.screenshotconverter.CropResize2 = frm.CropResize2;
                discoveryform.screenshotconverter.CropResizeArea1 = frm.CropResizeArea1;
                discoveryform.screenshotconverter.CropResizeArea2 = frm.CropResizeArea2;
                discoveryform.screenshotconverter.KeepMasterConvertedImage = frm.KeepMasterConvertedImage;
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
            ExtendedControls.ExtComboBox c = sender as ExtendedControls.ExtComboBox;
            EDDConfig.Instance.Language = c.Items[c.SelectedIndex];
            ExtendedControls.MessageBoxTheme.Show(this, "Applies at next restart of ED Discovery".T(EDTx.UserControlSettings_Language), "Information".T(EDTx.Information), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region window options

        private void comboBoxClickThruKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExtendedControls.ExtComboBox c = sender as ExtendedControls.ExtComboBox;
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


            if ( EDDConfig.Instance.EDSMEDDBDownload == true)   // if turned on
            {
                int gridsel = 0;
                bool[] grids = new bool[GridId.MaxGridID];
                foreach (int i in GridId.FromString(EDDConfig.Instance.EDSMGridIDs))
                    gridsel++;

                if (gridsel == 0)                               // but we have zero grids selected, force the user to select again
                    buttonExtEDSMConfigureArea_Click(sender, e);
            }
        }

        ExtendedControls.InfoForm info;
        System.Windows.Forms.Timer removetimer;

        private void buttonExtEDSMConfigureArea_Click(object sender, EventArgs e)
        {
            GalaxySectorSelect gss = new GalaxySectorSelect();

            if (!gss.Init(EDDConfig.Instance.EDSMGridIDs))
            {
                ExtendedControls.MessageBoxTheme.Show(this, "Warning".T(EDTx.Warning), "No map downloaded - please wait for it to download".T(EDTx.UserControlSettings_NoMap));
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
                    info.Info("Remove Sectors".T(EDTx.UserControlSettings_RemoveSectors),
                                EDDiscovery.Properties.Resources.edlogo_3mo_icon,
                                string.Format(("Removing {0} Sector(s)." + Environment.NewLine + Environment.NewLine +
                                "This will take a while (up to 30 mins dep on drive type and amount of sectors)." + Environment.NewLine +
                                "You may continue to use EDD while this operation takes place" + Environment.NewLine +
                                "but it may be slow to respond. Do not close down EDD until this window says" + Environment.NewLine +
                                "the process has finished" + Environment.NewLine + Environment.NewLine).T(EDTx.UserControlSettings_GalRemove), gss.Removed.Count));
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
            inform("Removing Grids" + Environment.NewLine);
            SystemsDB.RemoveGridSystems(sectors.ToArray(), inform);     // MUST do first as relies on system grid for info
            inform("Vacuum Database for size" + Environment.NewLine);
            SystemsDB.Vacuum();
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
                    "If you already have the 3dmap open, changes will not be reflected in that map until the next start of EDD" + Environment.NewLine).T(EDTx.UserControlSettings_GalFini));
            }
        }

        #endregion

        #region Safemode

        private void buttonExtSafeMode_Click(object sender, EventArgs e)
        {
            if (ExtendedControls.MessageBoxTheme.Show(this, "Confirm restart to safe mode".T(EDTx.UserControlSettings_CSM), "Safe Mode".T(EDTx.UserControlSettings_SM), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                bool force = EDDApplicationContext.RestartInSafeMode;
                EDDApplicationContext.RestartInSafeMode = true;

                if (!force)
                {
                    Application.Exit();
                }
                else
                {
                    System.Threading.Thread.CurrentThread.Abort();
                }
            }
        }

        #endregion

        #region Webserver

        private void PeriodicCheck(object sender, EventArgs e)      // webserver needs periodically checking to see if running.
        {
            bool running = discoveryform.WebServer.Running;
            if (extCheckBoxWebServerEnable.Checked != running)
            {
                extCheckBoxWebServerEnable.CheckedChanged -= ExtCheckBoxWebServerEnable_CheckedChanged;
                extCheckBoxWebServerEnable.Checked = running;
                extCheckBoxWebServerEnable.CheckedChanged += ExtCheckBoxWebServerEnable_CheckedChanged;
            }

            numberBoxLongPortNo.Enabled = !running;
            extButtonTestWeb.Enabled = running;
        }


        private void ExtCheckBoxWebServerEnable_CheckedChanged(object sender, EventArgs e)
        {
            bool runit = extCheckBoxWebServerEnable.Checked;

            if (runit)
            {
                var res = ExtendedControls.MessageBoxTheme.Show(this.FindForm(), ("You need to configure Windows to allow EDD to webserve" + Environment.NewLine +
                                                                           "Click Yes to do this. If you are not the adminstrator, a dialog will appear to ask you" + Environment.NewLine +
                                                                           "to sign in as an admin to allow this to happen" + Environment.NewLine +
                                                                           "If you have previously done this on this same port number you can click No and the enable will work").T(EDTx.UserControlSettings_WSQ),
                                                                           "Web Server",
                                                                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (res == DialogResult.Yes)
                {
                    BaseUtils.Processes process = new BaseUtils.Processes();

                    // exe method to set it to the eddiscovery ext does not seem to work during debugging.. not sure, can't find out.. abandon for now.
                    //string exe = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                    //string cmd = "-Command new-netfirewallrule -Name EDDiscovery -DisplayName EDDiscovery -Description Webserver -Program \"" + exe + "\" -Direction Inbound -Action Allow -LocalPort " + EDDConfig.Instance.WebServerPort.ToStringInvariant() + " -Protocol TCP" +

                    string cmd = "-Command " +
                                 "new-netfirewallrule " +
                                 "-Name EDDiscovery " +
                                 "-DisplayName EDDiscovery " +
                                 "-Description Webserver " +
                                 "-Direction Inbound " +
                                 "-Action Allow " +
                                $"-LocalPort {EDDConfig.Instance.WebServerPort.ToStringInvariant()} " +
                                 "-Protocol TCP;" +
                                 "netsh http add urlacl " +
                                $"url = http://*:{EDDConfig.Instance.WebServerPort.ToStringInvariant()}/ " +
                                $"user=\"{Environment.GetEnvironmentVariable("USERNAME")}\"";

                    int pid = process.StartProcess("Powershell.exe", cmd, "runas");

                    if (pid == 0)
                    {
                        ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Configuration did not run", "Web Server");
                    }
                    else
                    {
                        process.WaitForProcess(pid, 25000);
                    }
                }
            }

            if ( !discoveryform.WebServerControl(runit) )
            {
                ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Did not start - click OK to configure windows".T(EDTx.UserControlSettings_WSF), "Web Server");
            }
            else
                EDDConfig.Instance.WebServerEnable = runit;     // this is for next time at startup
        }

        #endregion

        private void extButtonTestWebClick(object sender, EventArgs e)
        {
            string ipv4 = BaseUtils.BrowserInfo.EstimateLocalHostPreferredIPV4();

            BaseUtils.BrowserInfo.LaunchBrowser("http://" + ipv4 + ":" + EDDConfig.Instance.WebServerPort.ToStringInvariant() + "/");
        }
    }
}

