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
using CAPI;
using EDDiscovery.Forms;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlSettings : UserControlCommonBase
    {
        Timer tm = new Timer();

        public UserControlSettings()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            extComboBoxGameTime.Items.Add("Local".T(EDTx.UserControlSettings_Local));
            extComboBoxGameTime.Items.Add("UTC");
            extComboBoxGameTime.Items.Add("Game Time".T(EDTx.UserControlSettings_GameTime));

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);

            ResetThemeList();

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

            checkBoxOrderRowsInverted.Checked = EDDConfig.Instance.OrderRowsInverted;
            checkBoxMinimizeToNotifyIcon.Checked = EDDConfig.Instance.MinimizeToNotifyIcon;
            checkBoxKeepOnTop.Checked = EDDConfig.Instance.KeepOnTop;
            checkBoxPanelSortOrder.Checked = EDDConfig.Instance.SortPanelsByName;
            checkBoxUseNotifyIcon.Checked = EDDConfig.Instance.UseNotifyIcon;
            checkBoxCustomResize.Checked = EDDConfig.Instance.DrawDuringResize;

            extComboBoxGameTime.SelectedIndex = EDDConfig.Instance.DisplayTimeIndex;

            checkBoxOrderRowsInverted.CheckedChanged += checkBoxOrderRowsInverted_CheckedChanged;
            checkBoxMinimizeToNotifyIcon.CheckedChanged += checkBoxMinimizeToNotifyIcon_CheckedChanged;
            checkBoxKeepOnTop.CheckedChanged += checkBoxKeepOnTop_CheckedChanged;
            checkBoxPanelSortOrder.CheckedChanged += checkBoxPanelSortOrder_CheckedChanged;
            checkBoxUseNotifyIcon.CheckedChanged += checkBoxUseNotifyIcon_CheckedChanged;
            extComboBoxGameTime.SelectedIndexChanged += ExtComboBoxGameTime_SelectedIndexChanged;
            checkBoxCustomResize.CheckedChanged += checkBoxCustomResize_CheckedChanged;

            checkBoxMinimizeToNotifyIcon.Enabled = EDDConfig.Instance.UseNotifyIcon;

            dataGridViewCommanders.AutoGenerateColumns = false;             // BEFORE assigned to list..
            dataGridViewCommanders.DataSource = EDCommander.GetListCommanders();

            this.comboBoxTheme.SelectedIndexChanged += this.comboBoxTheme_SelectedIndexChanged;    // now turn on the handler..

            checkBoxCustomEnableScreenshots.Checked = discoveryform.ScreenshotConverter.AutoConvert;
            this.checkBoxCustomEnableScreenshots.CheckedChanged += new System.EventHandler(this.checkBoxCustomEnableScreenshots_CheckedChanged);

            checkBoxCustomEDSMDownload.Checked = EDDConfig.Instance.EDSMDownload;
            this.checkBoxCustomEDSMDownload.CheckedChanged += new System.EventHandler(this.checkBoxCustomEDSMDownload_CheckedChanged);

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

        // Install CAPI for EDD 

        bool capiloggedin;
        Label capiStateLabel;
        ExtendedControls.ExtButton capiButton;
        ExtendedControls.ExtButton capiclearloginButton;
        EliteDangerousCore.Forms.CommanderForm cf;

        private List<ExtendedControls.ExtGroupBox> AdditionalCmdrControls()
        {
            var gb = new List<ExtendedControls.ExtGroupBox>();

            ExtendedControls.ExtGroupBox g1 = new ExtendedControls.ExtGroupBox() { Name = "CAPIGB" , Height = 60, Text = "Frontier CAPI" };
            capiButton = new ExtendedControls.ExtButton() { Location = new System.Drawing.Point(240,23),
                                    ClientSize = new System.Drawing.Size(80,20), Name="CAPIButton"};
            capiButton.Click += CapiButton_Click;
            capiclearloginButton = new ExtendedControls.ExtButton() { Location = new System.Drawing.Point(340, 23),
                ClientSize = new System.Drawing.Size(80, 20), Name = "ClearCAPIButton", Text = "Clear" };
            capiclearloginButton.Click += clearLoginButton_Click;

            capiStateLabel = new Label() { Location = new System.Drawing.Point(4, 23), Name ="CAPIStatus"};
            g1.Controls.Add(capiButton);
            g1.Controls.Add(capiclearloginButton);
            g1.Controls.Add(capiStateLabel);
            gb.Add(g1);
            return gb;
        }

        private void clearLoginButton_Click(object sender, EventArgs e)
        {
            discoveryform.FrontierCAPI.LogOut(cf.CommanderName);        // force logout and deletion of .cred
            capiclearloginButton.Visible = false;
        }

        private void CapiButton_Click(object sender, EventArgs e)
        {
            if (cf.CommanderName.HasChars())    // good commander name..
            {
                // if we have a login, and it has credentials, delete them
                if (discoveryform.FrontierCAPI.GetUserState(cf.CommanderName) == CompanionAPI.UserState.HasLoggedInWithCredentials) 
                {
                    discoveryform.FrontierCAPI.LogOut(cf.CommanderName);
                    SetCAPILabelState();
                }
                else
                {
                    discoveryform.FrontierCAPI.LogIn(cf.CommanderName);         // perform login, which does the auth procedure.
                    capiloggedin = true;         // remember we did a logon
                    capiButton.Enabled = false;
                    capiStateLabel.Text = "Logging in".T(EDTx.CommanderForm_CAPILoggingin);
                }
            }
            else
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "No commander name", "Warning".T(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        void SetCAPILabelState()                                        
        {
            if (!discoveryform.FrontierCAPI.ClientIDAvailable)      // disable id no capiID
            {
                capiclearloginButton.Visible = false;
                capiButton.Enabled = false;
                capiStateLabel.Text = capiButton.Text = "Disabled".T(EDTx.CommanderForm_CAPIDisabled);
            }
            else if (cf.CommanderName.HasChars() && discoveryform.FrontierCAPI.GetUserState(cf.CommanderName) == CompanionAPI.UserState.HasLoggedInWithCredentials)   // if logged in..
            {
                capiclearloginButton.Visible = false;
                capiButton.Enabled = true;
                capiButton.Text = "Logout".T(EDTx.CommanderForm_CAPILogout);
                capiStateLabel.Text = "Logged In".T(EDTx.CommanderForm_CAPILoggedin);
            }
            else
            {                                                   // no cred, or logged out..
                capiclearloginButton.Visible = discoveryform.FrontierCAPI.GetUserState(cf.CommanderName) != CompanionAPI.UserState.NeverLoggedIn;
                capiButton.Enabled = true;
                capiButton.Text = "Login".T(EDTx.CommanderForm_CAPILogin);
                capiStateLabel.Text = "Await Log in".T(EDTx.CommanderForm_CAPIAwaitLogin);
            }
        }

        void CAPICallBack(CAPI.CompanionAPI.State s)            // call back from CAPI system saying state changed, update button/label
        {
            System.Diagnostics.Debug.WriteLine("Capi state " + s);
            SetCAPILabelState();
        }

        // add dde handler
        // callback to CAPICallback to authorized to change the state.

        private void buttonAddCommander_Click(object sender, EventArgs e)
        {
            cf = new EliteDangerousCore.Forms.CommanderForm(AdditionalCmdrControls());
            cf.Init(true);
            discoveryform.FrontierCAPI.StatusChange += CAPICallBack;
            SetCAPILabelState();

            if (cf.ShowDialog(FindForm()) == DialogResult.OK)
            {
                if (cf.Valid && !EDCommander.IsCommanderPresent(cf.CommanderName))
                {
                    EDCommander cmdr = new EDCommander();
                    cf.Update(cmdr);
                    EDCommander.Add(cmdr);
                    UpdateCommandersListBox();
                    discoveryform.LoadCommandersListBox();
                    discoveryform.RefreshHistoryAsync();           // will do a new parse on commander list adding/removing scanners
                    btnDeleteCommander.Enabled = EDCommander.NumberOfCommanders > 1;
                }
                else
                    ExtendedControls.MessageBoxTheme.Show(FindForm(), "Commander name is not valid or duplicate".T(EDTx.UserControlSettings_AddC) , "Cannot create Commander".T(EDTx.UserControlSettings_AddT), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

            discoveryform.FrontierCAPI.StatusChange -= CAPICallBack;
        }

        private void dataGridViewCommanders_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewCommanders.Rows.Count)
            {
                CmdrEdit(e.RowIndex);
            }
        }

        private void buttonEditCommander_Click(object sender, EventArgs e)
        {
            if (dataGridViewCommanders.SelectedRows.Count > 0)
            {
                CmdrEdit(dataGridViewCommanders.SelectedRows[0].Index);
            }
        }

        private void CmdrEdit(int row)
        { 
            EDCommander cmdr = dataGridViewCommanders.Rows[row].DataBoundItem as EDCommander;

            cf = new EliteDangerousCore.Forms.CommanderForm(AdditionalCmdrControls());
            cf.Init(cmdr,false);
            SetCAPILabelState();
            discoveryform.FrontierCAPI.StatusChange += CAPICallBack;
            capiloggedin = false;

            if (cf.ShowDialog(FindForm()) == DialogResult.OK)
            {
                bool forceupdate = cf.Update(cmdr);
                cmdr.Update();
                dataGridViewCommanders.Refresh();

                if ( forceupdate || capiloggedin )              // either a critial journal item changed, or a capi was logged
                    discoveryform.RefreshHistoryAsync();        // do a resync
            }

            discoveryform.FrontierCAPI.StatusChange -= CAPICallBack;
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
            EDDConfig.Instance.DisplayTimeIndex = extComboBoxGameTime.SelectedIndex;
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

        bool themeprogchange = false;
        private void ResetThemeList()
        {
            themeprogchange = true;
            comboBoxTheme.Items = discoveryform.ThemeList.GetThemeNames();
            int i = discoveryform.ThemeList.FindThemeIndex(ExtendedControls.Theme.Current.Name);
            if (i == -1)        // if not found
            {
                ExtendedControls.Theme.Current.SetCustom();     // not in list, must be custom, force name
                comboBoxTheme.Items.Add("Custom");
                comboBoxTheme.SelectedItem = "Custom";
            }
            else
                comboBoxTheme.SelectedIndex = i;

            themeprogchange = false;
        }
        private void comboBoxTheme_SelectedIndexChanged(object sender, EventArgs e) // theme selected..
        {
            if (!themeprogchange)
            {
                string themename = comboBoxTheme.Items[comboBoxTheme.SelectedIndex].ToString();

                string fontwanted = null;                                               // don't check custom, only a stored theme..
                if (!themename.Equals("Custom") && !discoveryform.ThemeList.IsFontAvailableInTheme(themename, out fontwanted))
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
                        ResetThemeList();
                        return;
                    }
                }

                discoveryform.ThemeList.SetThemeByName(themename);             // given the name, go to it, if possible. if not, its not there, it should be
                ResetThemeList();
                discoveryform.ApplyTheme(true);
            }
        }

        private void buttonSaveTheme_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.InitialDirectory = EDDOptions.Instance.ThemeAppDirectory();
            dlg.DefaultExt = "eddtheme";
            dlg.AddExtension = true;

            if (dlg.ShowDialog(FindForm()) == DialogResult.OK)
            {
                if ( ExtendedControls.Theme.Current.SaveFile(dlg.FileName))
                {
                    discoveryform.ThemeList.Load(EDDOptions.Instance.ThemeAppDirectory(),"*.eddtheme");          // make sure up to data - we added a theme, reload them all
                    ExtendedControls.Theme.Current.Name = Path.GetFileNameWithoutExtension(dlg.FileName);   // we set the name here, if its not in the theme list on reset, it will go to custom
                    ResetThemeList();
                }
            }
        }

        public void button_edittheme_Click(object sender, EventArgs e)
        {
            var themeeditor = new ExtendedControls.ThemeEditor() { TopMost = FindForm().TopMost };

            var curtheme = ExtendedControls.Theme.Current;

            themeeditor.ApplyChanges = (theme) => { ExtendedControls.Theme.Current = theme; discoveryform.ApplyTheme(true); };

            buttonSaveTheme.Enabled = comboBoxTheme.Enabled = button_edittheme.Enabled = false;

            themeeditor.InitForm(curtheme);     // makes a copy
            themeeditor.FormClosing += (sa, ea) =>
            {
                buttonSaveTheme.Enabled = comboBoxTheme.Enabled = button_edittheme.Enabled = true;

                if ( themeeditor.DialogResult == DialogResult.OK )
                {
                    ExtendedControls.Theme.Current = themeeditor.Theme;
                }
                else
                {
                    ExtendedControls.Theme.Current = curtheme;
                }

                ResetThemeList();
                discoveryform.ApplyTheme(true);
            };

            themeeditor.Show(FindForm());
        }

        #endregion

        #region Screenshots

        private void buttonExtScreenshot_Click(object sender, EventArgs e)
        {
            discoveryform.ScreenshotConverter.Configure(this.discoveryform);
        }

        private void checkBoxCustomEnableScreenshots_CheckedChanged(object sender, EventArgs e)
        {
            discoveryform.ScreenshotConverter.AutoConvert = checkBoxCustomEnableScreenshots.Checked;
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
            EDDConfig.Instance.MinimizeToNotifyIcon = checkBoxMinimizeToNotifyIcon.Checked;
        }

        private void checkBoxCustomResize_CheckedChanged(object sender, EventArgs e)
        {
            EDDConfig.Instance.DrawDuringResize = checkBoxCustomResize.Checked;
        }

        public void DisableNotifyIcon()
        {
            checkBoxUseNotifyIcon.Checked = false;
        }

        private void checkBoxUseNotifyIcon_CheckedChanged(object sender, EventArgs e)
        {
            bool chk = checkBoxUseNotifyIcon.Checked;
            EDDConfig.Instance.UseNotifyIcon = chk;
            checkBoxMinimizeToNotifyIcon.Enabled = chk;
            discoveryform.useNotifyIconChanged(chk);
        }

        private void checkBoxKeepOnTop_CheckedChanged(object sender, EventArgs e)
        {
            var frm = FindForm();
            EDDConfig.Instance.KeepOnTop = frm.TopMost = checkBoxKeepOnTop.Checked;
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

        #region EDSM

        private void checkBoxCustomEDSMDownload_CheckedChanged(object sender, EventArgs e)
        {
            EDDConfig.Instance.EDSMDownload = checkBoxCustomEDSMDownload.Checked;


            if ( EDDConfig.Instance.EDSMDownload == true)   // if turned on
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
                ExtendedControls.MessageBoxTheme.Show(this, "Warning".T(EDTx.Warning), "No map available!".T(EDTx.UserControlSettings_NoMap));
            }
            else if (gss.ShowDialog() == DialogResult.OK)
            {
                EDDConfig.Instance.EDSMGridIDs = gss.Selection;

                if (gss.Action == GalaxySectorSelect.ActionToDo.Add)
                {
                    discoveryform.ForceEDSMFullRefresh();
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
            SystemsDatabase.Instance.RemoveGridSystems(sectors.ToArray(), inform);   
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
                                $"user=\"{Environment.GetEnvironmentVariable("USERDOMAIN")}\\{Environment.GetEnvironmentVariable("USERNAME")}\"";

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

        private void extButtonDLLPerms_Click(object sender, EventArgs e)
        {
            string n = EliteDangerousCore.DLL.EDDDLLManager.DLLPermissionManager(this.FindForm(), this.FindForm().Icon, EDDConfig.Instance.DLLPermissions);
            if (n != null)
                EDDConfig.Instance.DLLPermissions = n;
        }

        private void extButtonDLLConfigure_Click(object sender, EventArgs e)
        {
            discoveryform.DLLManager.DLLConfigure(this.FindForm(), this.FindForm().Icon,
                (name) => UserDatabase.Instance.GetSettingString("DLLConfig_" + name, ""), (name, set) => UserDatabase.Instance.PutSettingString("DLLConfig_" + name, set));
        }
    }
}

