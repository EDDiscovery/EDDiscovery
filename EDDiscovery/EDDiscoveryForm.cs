/*
 * Copyright © 2015 - 2017 EDDiscovery development team
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
using EDDiscovery.DB;
using EliteDangerousCore.EDDN;
using EliteDangerousCore.EDSM;
using EDDiscovery.Forms;
using EDDiscovery.Export;
using BaseUtils.Win32Constants;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.JournalEvents;

namespace EDDiscovery
{
    public partial class EDDiscoveryForm : Form, IDiscoveryController
    {
        #region Variables

        // Mono compatibility
        private bool _window_dragging = false;
        private Point _window_dragMousePos = Point.Empty;
        private Point _window_dragWindowPos = Point.Empty;

        private EDDiscoveryController Controller;
        private Actions.ActionController actioncontroller;

        static public EDDConfig EDDConfig { get { return EDDConfig.Instance; } }
        public EDDTheme theme { get { return EDDTheme.Instance; } }

        public TravelHistoryControl TravelControl { get { return travelHistoryControl; } }
        public RouteControl RouteControl { get { return routeControl1; } }
        public ExportControl ExportControl { get { return exportControl1; } }
        public EDDiscovery.ImageHandler.ImageHandler ImageHandler { get { return imageHandler1; } }

        public AudioExtensions.AudioQueue AudioQueueWave { get { return audioqueuewave; } }
        public AudioExtensions.AudioQueue AudioQueueSpeech { get { return audioqueuespeech; } }
        public AudioExtensions.SpeechSynthesizer SpeechSynthesizer { get { return speechsynth; } }

        public string EliteInputList() { return inputdevices.ListDevices(); }
        public string EliteInputCheck() { return inputdevicesactions.CheckBindings(); }

        public BindingsFile FrontierBindings { get { return frontierbindings; } }

        AudioExtensions.IAudioDriver audiodriverwave;
        AudioExtensions.AudioQueue audioqueuewave;
        AudioExtensions.IAudioDriver audiodriverspeech;
        AudioExtensions.AudioQueue audioqueuespeech;
        AudioExtensions.SpeechSynthesizer speechsynth;

        DirectInputDevices.InputDeviceList inputdevices;
        Actions.ActionsFromInputDevices inputdevicesactions;
        BindingsFile frontierbindings;

        public EliteDangerousCore.CompanionAPI.CompanionAPIClass Capi { get; private set; } = new EliteDangerousCore.CompanionAPI.CompanionAPIClass();

        public EDDiscovery._3DMap.MapManager Map { get; private set; }

        Task checkInstallerTask = null;
        private bool themeok = true;

        BaseUtils.GitHubRelease newRelease;

        public PopOutControl PopOuts;

        private bool _shownOnce = false;
        private bool _formMax;
        private int _formWidth;
        private int _formHeight;
        private int _formTop;
        private int _formLeft;

        #endregion

        #region Callbacks from us

        public event Action<Object> OnNewTarget;
        public event Action<Object, HistoryEntry, bool> OnNoteChanged;                    // UI.Note has been updated attached to this note

        #endregion

        #region IDiscoveryController interface
        #region Properties
        public HistoryList history { get { return Controller.history; } }
        public EDSMSync EdsmSync { get { return Controller.EdsmSync; } }
        public string LogText { get { return Controller.LogText; } }
        public bool PendingClose { get { return Controller.PendingClose; } }
        public GalacticMapping galacticMapping { get { return Controller.galacticMapping; } }
        #endregion

        #region Events - see the EDDiscoveryControl for meaning and context
        public event Action<HistoryList> OnHistoryChange { add { Controller.OnHistoryChange += value; } remove { Controller.OnHistoryChange -= value; } }
        public event Action<HistoryEntry, HistoryList> OnNewEntry { add { Controller.OnNewEntry += value; } remove { Controller.OnNewEntry -= value; } }
        public event Action<JournalEntry> OnNewJournalEntry { add { Controller.OnNewJournalEntry += value; } remove { Controller.OnNewJournalEntry -= value; } }
        public event Action<string, Color> OnNewLogEntry { add { Controller.OnNewLogEntry += value; } remove { Controller.OnNewLogEntry -= value; } }
        public event Action<EliteDangerousCore.CompanionAPI.CompanionAPIClass,HistoryEntry> OnNewCompanionAPIData;
        #endregion

        #region Logging
        public void LogLine(string text) { Controller.LogLine(text); }
        public void LogLineHighlight(string text) { Controller.LogLineHighlight(text); }
        public void LogLineSuccess(string text) { Controller.LogLineSuccess(text); }
        public void LogLineColor(string text, Color color) { Controller.LogLineColor(text, color); }
        public void ReportProgress(int percent, string message) { Controller.ReportProgress(percent, message); }
        #endregion

        #region History
        public bool RefreshHistoryAsync(string netlogpath = null, bool forcenetlogreload = false, bool forcejournalreload = false, bool checkedsm = false, int? currentcmdr = null)
        {
            return Controller.RefreshHistoryAsync(netlogpath, forcenetlogreload, forcejournalreload, checkedsm, currentcmdr);
        }
        public void RefreshDisplays() { Controller.RefreshDisplays(); }
        public void RecalculateHistoryDBs() { Controller.RecalculateHistoryDBs(); }
        #endregion

        #endregion

        #region Initialisation

        public EDDiscoveryForm()        // note we do not do the traditional Initialize component here.. we wait for splash form to call it
        {
            Controller = new EDDiscoveryController(() => theme.TextBlockColor, () => theme.TextBlockHighlightColor, () => theme.TextBlockSuccessColor, a => BeginInvoke(a));
            Controller.OnNewEntrySecond += Controller_NewEntrySecond;       // called after UI updates themselves with NewEntry
            Controller.OnBgSafeClose += Controller_BgSafeClose;
            Controller.OnFinalClose += Controller_FinalClose;
            Controller.OnInitialSyncComplete += Controller_InitialSyncComplete;
            Controller.OnRefreshCommanders += Controller_RefreshCommanders;
            Controller.OnRefreshComplete += Controller_RefreshComplete;
            Controller.OnRefreshStarting += Controller_RefreshStarting;
            Controller.OnReportProgress += Controller_ReportProgress;
            Controller.OnSyncComplete += Controller_SyncComplete;
            Controller.OnSyncStarting += Controller_SyncStarting;
        }

        public void Init(Action<string> msg)    // called from EDDApplicationContext .. continues on with the construction of the form
        {
            msg.Invoke("Modulating Shields");
            Controller.Init();

            // Some components require the controller to be initialized
            InitializeComponent();

            panelToolBar.HiddenMarkerWidth = 200;
            panelToolBar.PinState = SQLiteConnectionUser.GetSettingBool("ToolBarPanelPinState", true);

            comboBoxCustomPopOut.Enabled = false;
            comboBoxCustomPopOut.Items.AddRange(PopOutControl.GetPopOutNames());
            comboBoxCustomPopOut.SelectedIndex = 0;
            comboBoxCustomPopOut.Enabled = true;

            label_version.Text = EDDOptions.Instance.VersionDisplayString;

            PopOuts = new PopOutControl(this);

            ToolStripManager.Renderer = theme.toolstripRenderer;
            msg.Invoke("Repairing Canopy");
            theme.LoadThemes();                                         // default themes and ones on disk loaded

            if (!EDDOptions.Instance.NoTheme)
                themeok = theme.RestoreSettings();                                    // theme, remember your saved settings

            trilaterationControl.InitControl(this);
            travelHistoryControl.InitControl(this);
            imageHandler1.InitControl(this);
            settings.InitControl(this);
            journalViewControl1.InitControl(this, 0);
            gridControl.InitControl(this, 0);
            routeControl1.InitControl(this);
            savedRouteExpeditionControl1.InitControl(this);
            exportControl1.InitControl(this);

            Map = new EDDiscovery._3DMap.MapManager(EDDOptions.Instance.NoWindowReposition, this);

            this.TopMost = EDDConfig.KeepOnTop;

#if !NO_SYSTEM_SPEECH
            // Windows TTS (2000 and above). Speech *recognition* will be Version.Major >= 6 (Vista and above)
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 5)
            {
                msg.Invoke("Activating Sensors");
                audiodriverwave = new AudioExtensions.AudioDriverCSCore( EDDConfig.DefaultWaveDevice );
                audiodriverspeech = new AudioExtensions.AudioDriverCSCore( EDDConfig.DefaultVoiceDevice );
                speechsynth = new AudioExtensions.SpeechSynthesizer(new AudioExtensions.WindowsSpeechEngine());
            }
            else
            {
                audiodriverwave = new AudioExtensions.AudioDriverDummy();
                audiodriverspeech = new AudioExtensions.AudioDriverDummy();
                speechsynth = new AudioExtensions.SpeechSynthesizer(new AudioExtensions.DummySpeechEngine());
            }
#else
            audiodriverwave = new AudioExtensions.AudioDriverDummy();
            audiodriverspeech = new AudioExtensions.AudioDriverDummy();
            speechsynth = new AudioExtensions.SpeechSynthesizer(new AudioExtensions.DummySpeechEngine());
#endif
            audioqueuewave = new AudioExtensions.AudioQueue(audiodriverwave);
            audioqueuespeech = new AudioExtensions.AudioQueue(audiodriverspeech);

            actioncontroller = new Actions.ActionController(this, Controller, this.Icon);

            frontierbindings = new BindingsFile();
            inputdevices = new DirectInputDevices.InputDeviceList(a => BeginInvoke(a));
            inputdevicesactions = new Actions.ActionsFromInputDevices(inputdevices, frontierbindings, actioncontroller);

            ApplyTheme();

            notifyIcon1.Visible = EDDConfig.UseNotifyIcon;

            SetUpLogging();
        }

        // OnLoad is called the first time the form is shown, before OnShown or OnActivated are called
        private void EDDiscoveryForm_Load(object sender, EventArgs e)
        {
            try
            {
                MaterialCommodityDB.SetUpInitialTable();
                Controller.PostInit_Loaded();

                RepositionForm();
                ShowInfoPanel("Loading. Please wait!", true);
                routeControl1.travelhistorycontrol1 = travelHistoryControl;
                settings.InitSettingsTab();
                savedRouteExpeditionControl1.LoadControl();

                if (EDDOptions.Instance.ActionButton)
                {
                    buttonReloadActions.Visible = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("EDDiscoveryForm_Load exception: " + ex.Message + "\n" + "Trace: " + ex.StackTrace);
            }
        }

        // OnShown is called every time Show is called
        private void EDDiscoveryForm_Shown(object sender, EventArgs e)
        {
            Controller.PostInit_Shown();

            if (!themeok)
            {
                Controller.LogLineHighlight("The theme stored has missing colors or other missing information");
                Controller.LogLineHighlight("Correct the missing colors or other information manually using the Theme Editor in Settings");
            }

            actioncontroller.onStartup();

            _shownOnce = true;
        }

        #endregion

        #region New Installer

        private Task CheckForNewInstallerAsync()
        {
            return Task.Factory.StartNew(() =>
            {
#if !DEBUG
                CheckForNewinstaller();
#endif
            });
        }

        private bool CheckForNewinstaller()
        {
            try
            {

                BaseUtils.GitHubClass github = new BaseUtils.GitHubClass(EDDiscovery.Properties.Resources.URLGithubDownload, LogLine);

                BaseUtils.GitHubRelease rel = github.GetLatestRelease();

                if (rel != null)
                {
                    //string newInstaller = jo["Filename"].Value<string>();

                    var currentVersion = Application.ProductVersion;

                    Version v1, v2;
                    v1 = new Version(rel.ReleaseVersion);
                    v2 = new Version(currentVersion);

                    if (v1.CompareTo(v2) > 0) // Test if newer installer exists:
                    {
                        newRelease = rel;
                        this.BeginInvoke(new Action(() => Controller.LogLineHighlight("New EDDiscovery installer available: " + rel.ReleaseName)));
                        this.BeginInvoke(new Action(() => ShowInfoPanel("New Release Available!", true)));
                        return true;
                    }
                }
            }
            catch (Exception)
            {

            }

            return false;
        }

        #endregion

        #region Form positioning

        private void RepositionForm()
        {
            var top = SQLiteDBClass.GetSettingInt("FormTop", -1);
            if (top != -1 && EDDOptions.Instance.NoWindowReposition == false)
            {
                var left = SQLiteDBClass.GetSettingInt("FormLeft", 0);
                var height = SQLiteDBClass.GetSettingInt("FormHeight", 800);
                var width = SQLiteDBClass.GetSettingInt("FormWidth", 800);

                // Adjust so window fits on screen; just in case user unplugged a monitor or something

                var screen = SystemInformation.VirtualScreen;
                if (height > screen.Height) height = screen.Height;
                if (top + height > screen.Height + screen.Top) top = screen.Height + screen.Top - height;
                if (width > screen.Width) width = screen.Width;
                if (left + width > screen.Width + screen.Left) left = screen.Width + screen.Left - width;
                if (top < screen.Top) top = screen.Top;
                if (left < screen.Left) left = screen.Left;

                this.Top = top;
                this.Left = left;
                this.Height = height;
                this.Width = width;

                this.CreateParams.X = this.Left;
                this.CreateParams.Y = this.Top;
                this.StartPosition = FormStartPosition.Manual;

                _formMax = SQLiteDBClass.GetSettingBool("FormMax", false);
                if (_formMax) this.WindowState = FormWindowState.Maximized;
            }
            _formLeft = Left;
            _formTop = Top;
            _formHeight = Height;
            _formWidth = Width;

            travelHistoryControl.LoadLayoutSettings();
            journalViewControl1.LoadLayoutSettings();
            gridControl.LoadLayoutSettings();

            if (EDDConfig.AutoLoadPopOuts && EDDOptions.Instance.NoWindowReposition == false)
                PopOuts.LoadSavedPopouts();

            string tab = SQLiteConnectionUser.GetSettingString("MajorTab", "");
            SelectTabPage(tab);
        }

        #endregion

        #region Themeing

        public void ApplyTheme()
        {
            ToolStripManager.Renderer = theme.toolstripRenderer;
            panel_close.Visible = !theme.WindowsFrame;
            panel_minimize.Visible = !theme.WindowsFrame;
            label_version.Visible = !theme.WindowsFrame;

            this.Text = "EDDiscovery " + label_version.Text;            // note in no border mode, this is not visible on the title bar but it is in the taskbar..

            theme.ApplyToForm(this);

            labelInfoBoxTop.Location = new Point(label_version.Right + 16, labelInfoBoxTop.Top);

            Controller.RefreshDisplays();
        }

        #endregion

        #region EDSM and EDDB syncs code

        private void edsmRefreshTimer_Tick(object sender, EventArgs e)
        {
            Controller.AsyncPerformSync();
        }

        #endregion

        #region Controller event handlers 
        private void Controller_InitialSyncComplete()
        {
            imageHandler1.StartWatcher();
            routeControl1.EnableRouteTab(); // now we have systems, we can update this..

            routeControl1.travelhistorycontrol1 = travelHistoryControl;
            ShowInfoPanel("", false);

            checkInstallerTask = CheckForNewInstallerAsync();
        }

        private void Controller_SyncStarting()
        {
            edsmRefreshTimer.Enabled = false;
        }

        private void Controller_SyncComplete()
        {
            edsmRefreshTimer.Enabled = true;
        }

        private void Controller_RefreshStarting()
        {
            RefreshButton(false);
            actioncontroller.ActionRun("onRefreshStart", "ProgramEvent");
        }

        private void Controller_RefreshCommanders()
        {
            LoadCommandersListBox();             // in case a new commander has been detected
            exportControl1.PopulateCommanders();
            settings.UpdateCommandersListBox();
        }

        private void Controller_RefreshComplete()
        {
            RefreshButton(true);
            actioncontroller.ActionRunOnRefresh();

            if (!Capi.IsCommanderLoggedin(EDCommander.Current.Name))
            {
                Capi.Logout();

                if (EliteDangerousCore.CompanionAPI.CompanionCredentials.CredentialState(EDCommander.Current.Name) == EliteDangerousCore.CompanionAPI.CompanionCredentials.State.CONFIRMED)
                {
                    try
                    {
                        Capi.LoginAs(EDCommander.Current.Name);
                        LogLine("Logged into Companion API");
                    }
                    catch (Exception ex)
                    {
                        LogLineHighlight("Companion API log in failed: " + ex.Message);
                        if (!(ex is EliteDangerousCore.CompanionAPI.CompanionAppException))
                            LogLineHighlight(ex.StackTrace);
                    }
                }
            }

            if (Capi.LoggedIn)
            {
                if (Controller.history != null && Controller.history.Count > 0 && Controller.history.GetLast.ContainsRares())
                {
                    LogLine("Not performing Companion API get due to carrying rares");
                }
                else
                { 
                    try
                    {
                        Capi.GetProfile();
                        OnNewCompanionAPIData?.Invoke(Capi, null);
                    }
                    catch (Exception ex)
                    {
                        LogLineHighlight("Companion API get failed: " + ex.Message);
                        if (!(ex is EliteDangerousCore.CompanionAPI.CompanionAppException))
                            LogLineHighlight(ex.StackTrace);

                        // what do we do TBD
                    }
                }
            }
        }

        private void Controller_NewEntrySecond(HistoryEntry he, HistoryList hl)         // called after all UI's have had their chance
        {
            actioncontroller.ActionRunOnEntry(he, "NewEntry");

            // all notes committed
            SystemNoteClass.CommitDirtyNotes((snc) => { if (EDCommander.Current.SyncToEdsm && snc.FSDEntry) EDSMSync.SendComments(snc.SystemName, snc.Note, snc.EdsmId); });

            if ( he.EntryType == JournalTypeEnum.Docked )
            {
                if (Capi.IsCommanderLoggedin(EDCommander.Current.Name))
                {
                    if (he.ContainsRares())
                    {
                        LogLine("Not performing Companion API get due to carrying rares");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Commander " + EDCommander.Current.Name + " in CAPI");
                        try
                        {
                            Capi.GetProfile();

                            JournalDocked dockevt = he.journalEntry as JournalDocked;

                            if (!Capi.Profile.Cmdr.docked)
                            {
                                LogLineHighlight("CAPI not docked. Server API lagging!");
                                // Todo add a retry later...
                            }
                            else if (!dockevt.StarSystem.Equals(Capi.Profile.CurrentStarSystem.name))
                            {
                                LogLineHighlight("CAPI profileSystemRequired is " + dockevt.StarSystem + ", profile station is " + Capi.Profile.CurrentStarSystem.name);
                                // Todo add a retry later...
                            }
                            else if (!dockevt.StationName.Equals(Capi.Profile.StarPort.name))
                            {
                                LogLineHighlight("CAPI profileStationRequired is " + dockevt.StationName + ", profile station is " + Capi.Profile.StarPort.name);
                                // Todo add a retry later...
                            }
                            else
                            {
                                JournalEDDCommodityPrices entry = JournalEntry.AddEDDCommodityPrices(EDCommander.Current.Nr, he.journalEntry.EventTimeUTC.AddSeconds(1), Capi.Profile.StarPort.name, Capi.Profile.StarPort.faction, Capi.Profile.StarPort.jcommodities);
                                if (entry != null)
                                {
                                    Controller.NewEntry(entry);
                                    OnNewCompanionAPIData?.Invoke(Capi, he);

                                    if (EDCommander.Current.SyncToEddn)
                                        SendPricestoEDDN(he);

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogLineHighlight("Companion API get failed: " + ex.Message);
                        }
                    }
                }
            }
        }

        private void SendPricestoEDDN(HistoryEntry he)
        {
            try
            {
                EliteDangerousCore.EDDN.EDDNClass eddn = new EliteDangerousCore.EDDN.EDDNClass();

                eddn.commanderName = he.Commander.EdsmName;
                if (string.IsNullOrEmpty(eddn.commanderName))
                     eddn.commanderName = Capi.Credentials.Commander;


                JObject msg = eddn.CreateEDDNCommodityMessage(Capi.Profile.StarPort.commodities, Capi.Profile.CurrentStarSystem.name, Capi.Profile.StarPort.name, DateTime.UtcNow);

                if (msg != null)
                {
                    LogLine("EDDN: Send commodities prices");
                    if (eddn.PostMessage(msg))
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                LogLineHighlight("EDDN: Send commodities prices failed: " + ex.Message);
            }

        }

        private void Controller_ReportProgress(int percentComplete, string message)
        {
            if (!Controller.PendingClose)
            {
                if (percentComplete >= 0)
                {
                    this.toolStripProgressBar1.Visible = true;
                    this.toolStripProgressBar1.Value = percentComplete;
                }
                else
                {
                    this.toolStripProgressBar1.Visible = false;
                }

                this.toolStripStatusLabel1.Text = message;
            }
        }

        #endregion

        #region Closing

        private void EDDiscoveryForm_FormClosing(object sender, FormClosingEventArgs e)     // when user asks for a close
        {
            edsmRefreshTimer.Enabled = false;
            if (!Controller.ReadyForFinalClose)
            {
                e.Cancel = true;
                ShowInfoPanel("Closing, please wait!", true);
                actioncontroller.ActionRun("onShutdown", "ProgramEvent");
                Controller.Shutdown();
            }
        }

        private void Controller_BgSafeClose()       // run in thread..
        {
            actioncontroller.CloseDown();
        }

        private void Controller_FinalClose()        // run in UI, when controller finishes close
        {
            // send any dirty notes.  if they are, the call back gets called. If we have EDSM sync on, and its an FSD entry, send it
            SystemNoteClass.CommitDirtyNotes((snc) => { if (EDCommander.Current.SyncToEdsm && snc.FSDEntry) EDSMSync.SendComments(snc.SystemName, snc.Note, snc.EdsmId); });

            settings.SaveSettings();

            SQLiteDBClass.PutSettingBool("FormMax", _formMax);
            SQLiteDBClass.PutSettingInt("FormWidth", _formWidth);
            SQLiteDBClass.PutSettingInt("FormHeight", _formHeight);
            SQLiteDBClass.PutSettingInt("FormTop", _formTop);
            SQLiteDBClass.PutSettingInt("FormLeft", _formLeft);
            SQLiteDBClass.PutSettingBool("ToolBarPanelPinState", panelToolBar.PinState);

            routeControl1.SaveSettings();
            theme.SaveSettings(null);
            travelHistoryControl.SaveSettings();
            journalViewControl1.SaveSettings();
            gridControl.SaveSettings();
            if (EDDConfig.AutoSavePopOuts)
                PopOuts.SaveCurrentPopouts();

            SQLiteConnectionUser.PutSettingString("MajorTab", tabControlMain.SelectedTab.Text);
            notifyIcon1.Visible = false;

            audioqueuespeech.Dispose();     // in order..
            audiodriverspeech.Dispose();
            audioqueuewave.Dispose();
            audiodriverwave.Dispose();

            inputdevicesactions.Stop();
            inputdevices.Clear();

            Close();
            Application.Exit();
        }
     
        #endregion

        #region Buttons, Mouse, Menus, NotifyIcon

        public void ShowInfoPanel(string message, bool visible)
        {
            labelInfoBoxTop.Text = message;
            labelInfoBoxTop.Visible = visible;
        }

        private void buttonReloadActions_Click(object sender, EventArgs e)
        {
            actioncontroller.ReLoad();
            actioncontroller.onStartup();
        }

        private void addNewStarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://robert.astronet.se/Elite/ed-systems/entry.html");
        }

        private void frontierForumThreadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Properties.Resources.URLProjectEDForumPost);
        }

        private void eDDiscoveryFGESupportThreadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://firstgreatexpedition.org/mybb/showthread.php?tid=1406");
        }

        private void eDDiscoveryHomepageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Properties.Resources.URLProjectWiki);
        }

        private void showLogfilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                EDCommander cmdr = EDCommander.Current;

                if (cmdr != null)
                {
                    string cmdrfolder = cmdr.JournalDir;
                    if (cmdrfolder.Length < 1)
                        cmdrfolder = EDJournalClass.GetDefaultJournalDir();
                    Process.Start(cmdrfolder);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Show log files exception: " + ex.Message);
            }
        }

        private void show2DMapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open2DMap();
        }

        private void show3DMapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open3DMap(travelHistoryControl.GetTravelHistoryCurrent);
        }

        private void forceEDDBUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Controller.AsyncPerformSync(eddbsync: true))      // we want it to have run, to completion, to allow another go..
                ExtendedControls.MessageBoxTheme.Show("Synchronisation to databases is in operation or pending, please wait");
        }

        private void syncEDSMSystemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Controller.AsyncPerformSync(edsmsync: true))      // we want it to have run, to completion, to allow another go..
                ExtendedControls.MessageBoxTheme.Show("Synchronisation to databases is in operation or pending, please wait");
        }

        private void gitHubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Properties.Resources.URLProjectGithub);
        }

        private void reportIssueIdeasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Properties.Resources.URLProjectFeedback);
        }

        internal void keepOnTopChanged(bool keepOnTop)
        {
            this.TopMost = keepOnTop;
        }

        /// <summary>
        /// The settings panel check box for 'Use notification area icon' has changed.
        /// </summary>
        /// <param name="useNotifyIcon">Whether or not the setting is enabled.</param>
        internal void useNotifyIconChanged(bool useNotifyIcon)
        {
            notifyIcon1.Visible = useNotifyIcon;
            if (!useNotifyIcon && !Visible)
                Show();
        }

        private void panel_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void changeMapColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settings.panel_defaultmapcolor_Click(sender, e);
        }

        private void editThemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settings.button_edittheme_Click(this, null);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void AboutBox(Form parent = null)
        {
            AboutForm frm = new AboutForm();
            frm.TopMost = parent?.TopMost ?? this.TopMost;
            frm.ShowDialog(parent ?? this);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox(this);
        }

        private void eDDiscoveryChatDiscordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Properties.Resources.URLProjectDiscord);
        }

        private void howToRunInSafeModeToResetVariousParametersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExtendedControls.MessageBoxTheme.Show(this, "To start in safe mode, exit the program, hold down the shift key" + Environment.NewLine +
                            "and double click on the EDD program icon.  You will then be in the safe mode dialog." + Environment.NewLine +
                            "You can reset various parameters and move the data bases to other locations.",
                            "How to run safe mode", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void showAllInTaskBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PopOuts.ShowAllPopOutsInTaskBar();
        }

        private void turnOffAllTransparencyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PopOuts.MakeAllPopoutsOpaque();
        }

        private void clearEDSMIDAssignedToAllRecordsForCurrentCommanderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ExtendedControls.MessageBoxTheme.Show("Confirm you wish to reset the assigned EDSM IDs to all the current commander history entries," +
                                " and clear all the assigned EDSM IDs in all your notes for all commanders\r\n\r\n" +
                                "This will not change your history, but when you next refresh, it will try and reassign EDSM systems to " +
                                "your history and notes.  Use only if you think that the assignment of EDSM systems to entries is grossly wrong," +
                                "or notes are going missing\r\n" +
                                "\r\n" +
                                "You can manually change one EDSM assigned system by right clicking on the travel history and selecting the option"
                                , "WARNING", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                JournalEntry.ClearEDSMID(EDCommander.CurrentCmdrID);
                SystemNoteClass.ClearEDSMID();
            }

        }


        private void paneleddiscovery_Click(object sender, EventArgs e)
        {
            AboutBox(this);
        }

        private void panel_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void read21AndFormerLogFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Read21Folders(false);
        }

        private void read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Read21Folders(true);
        }

        private void Read21Folders(bool force)
        {
            if (Controller.history.CommanderId >= 0)
            {
                EDCommander cmdr = EDCommander.Current;
                if (cmdr != null)
                {
                    FolderBrowserDialog dirdlg = new FolderBrowserDialog();
                    DialogResult dlgResult = dirdlg.ShowDialog();

                    if (dlgResult == DialogResult.OK)
                    {
                        string logpath = dirdlg.SelectedPath;

                        Controller.RefreshHistoryAsync(netlogpath: logpath, forcenetlogreload: force, currentcmdr: cmdr.Nr);
                    }
                }
            }
        }

        private void dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ExtendedControls.MessageBoxTheme.Show("Confirm you wish to reset all history entries to the current commander", "WARNING", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                JournalEntry.ResetCommanderID(-1, EDCommander.CurrentCmdrID);
                Controller.RefreshHistoryAsync();
            }
        }


        private void rescanAllJournalFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controller.RefreshHistoryAsync(forcejournalreload: true, checkedsm: true);
        }

        private void checkForNewReleaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckForNewinstaller() )
            {
                if (newRelease != null)
                {
                    NewReleaseForm frm = new NewReleaseForm();
                    frm.release = newRelease;

                    frm.ShowDialog(this);
                }
            }
            else
            {
                ExtendedControls.MessageBoxTheme.Show(this,"No new release found", "EDDiscovery", MessageBoxButtons.OK);
            }
        }

        private void deleteDuplicateFSDJumpEntriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ExtendedControls.MessageBoxTheme.Show("Confirm you remove any duplicate FSD entries from the current commander", "WARNING", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                int n = JournalEntry.RemoveDuplicateFSDEntries(EDCommander.CurrentCmdrID);
                Controller.LogLine("Removed " + n + " FSD entries");
                Controller.RefreshHistoryAsync();
            }
        }

        private void panelInfo_Click(object sender, EventArgs e)
        {
            if (newRelease != null)
            {
                NewReleaseForm frm = new NewReleaseForm();
                frm.release = newRelease;

                frm.ShowDialog(this);
            }
        }

        private void labelPanelText_Click(object sender, EventArgs e)
        {
            if (newRelease != null)
            {
                NewReleaseForm frm = new NewReleaseForm();
                frm.release = newRelease;

                frm.ShowDialog(this);
            }
        }

        public ISystem GetHomeSystem()
        {
            return settings.HomeSystem;
        }

        public void Open3DMap(HistoryEntry he)
        {
            this.Cursor = Cursors.WaitCursor;

            ISystem HomeSystem = GetHomeSystem();

            Controller.history.FillInPositionsFSDJumps();

            Map.Prepare(he?.System, HomeSystem,
                        settings.MapCentreOnSelection ? he?.System : HomeSystem,
                        settings.MapZoom, Controller.history.FilterByTravel);
            Map.Show();
            this.Cursor = Cursors.Default;
        }

        public void Open2DMap()
        {
            this.Cursor = Cursors.WaitCursor;
            Form2DMap frm = new Form2DMap(Controller.history.FilterByFSDAndPosition);
            frm.Nowindowreposition = EDDOptions.Instance.NoWindowReposition;
            frm.Show();
            this.Cursor = Cursors.Default;
        }

        private void sendUnsuncedEDDNEventsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<HistoryEntry> hlsyncunsyncedlist = Controller.history.FilterByScanNotEDDNSynced;        // first entry is oldest

            EDDNSync.SendEDDNEvents(LogLine, hlsyncunsyncedlist);
        }

        private void materialSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindMaterialsForm frm = new FindMaterialsForm();

            frm.Show(this);
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            // Tray icon was double-clicked.
            if (FormWindowState.Minimized == WindowState)
            {
                if (EDDConfig.MinimizeToNotifyIcon)
                    Show();
                if (_formMax)
                    WindowState = FormWindowState.Maximized;
                else
                    WindowState = FormWindowState.Normal;
            }
            else
                WindowState = FormWindowState.Minimized;
        }

        private void notifyIconMenu_Hide_Click(object sender, EventArgs e)
        {
            // Tray icon 'Hide Tray Icon' menu item was clicked.
            settings.checkBoxUseNotifyIcon.Checked = false;
        }

        private void notifyIconMenu_Open_Click(object sender, EventArgs e)
        {
            // Tray icon 'Open EDDiscovery' menu item was clicked. Present the main window.
            if (FormWindowState.Minimized == WindowState)
            {
                if (EDDConfig.UseNotifyIcon && EDDConfig.MinimizeToNotifyIcon)
                    Show();
                if (_formMax)
                    WindowState = FormWindowState.Maximized;
                else
                    WindowState = FormWindowState.Normal;
            }
            else
                Activate();
        }

#endregion

        #region Window Control

        protected override void WndProc(ref Message m)
        {
            // Compatibility movement for Mono
            if (m.Msg == WM.LBUTTONDOWN && m.WParam == (IntPtr)1 && !theme.WindowsFrame)
            {
                int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                int y = unchecked((short)((uint)m.LParam >> 16));
                _window_dragMousePos = new Point(x, y);
                _window_dragWindowPos = this.Location;
                _window_dragging = true;
                m.Result = IntPtr.Zero;
                this.Capture = true;
            }
            else if (m.Msg == WM.MOUSEMOVE && m.WParam == (IntPtr)1 && _window_dragging)
            {
                int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                int y = unchecked((short)((uint)m.LParam >> 16));
                Point delta = new Point(x - _window_dragMousePos.X, y - _window_dragMousePos.Y);
                _window_dragWindowPos = new Point(_window_dragWindowPos.X + delta.X, _window_dragWindowPos.Y + delta.Y);
                this.Location = _window_dragWindowPos;
                this.Update();
                m.Result = IntPtr.Zero;
            }
            else if (m.Msg == WM.LBUTTONUP)
            {
                _window_dragging = false;
                _window_dragMousePos = Point.Empty;
                _window_dragWindowPos = Point.Empty;
                m.Result = IntPtr.Zero;
                this.Capture = false;
            }
            // Windows honours NCHITTEST; Mono does not
            else if (m.Msg == WM.NCHITTEST)
            {
                base.WndProc(ref m);
                //System.Diagnostics.Debug.WriteLine( Environment.TickCount + " Res " + ((int)m.Result));

                if (m.Result == (IntPtr)HT.CLIENT)
                {
                    int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                    int y = unchecked((short)((uint)m.LParam >> 16));
                    Point p = PointToClient(new Point(x, y));

                    if (p.X > this.ClientSize.Width - statusStrip1.Height && p.Y > this.ClientSize.Height - statusStrip1.Height)
                    {
                        m.Result = (IntPtr)HT.BOTTOMRIGHT;
                    }
                    else if (p.Y > this.ClientSize.Height - statusStrip1.Height)
                    {
                        m.Result = (IntPtr)HT.BOTTOM;
                    }
                    else if (p.X > this.ClientSize.Width - 5)       // 5 is generous.. really only a few pixels gets thru before the subwindows grabs them
                    {
                        m.Result = (IntPtr)HT.RIGHT;
                    }
                    else if (p.X < 5)
                    {
                        m.Result = (IntPtr)HT.LEFT;
                    }
                    else if (!theme.WindowsFrame)
                    {
                        m.Result = (IntPtr)HT.CAPTION;
                    }
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        private IntPtr SendMessage(int msg, IntPtr wparam, IntPtr lparam)
        {
            Message message = Message.Create(this.Handle, msg, wparam, lparam);
            this.WndProc(ref message);
            return message.Result;
        }

        private void MouseDownCAPTION( object sender, MouseEventArgs e)
        {
            ((Control)sender).Capture = false;
            SendMessage(WM.NCLBUTTONDOWN, (System.IntPtr)HT.CAPTION, (System.IntPtr)0);
        }

        private void RecordPosition()
        {
            if (FormWindowState.Minimized != WindowState)
            {
                _formLeft = this.Left;
                _formTop = this.Top;
                _formWidth = this.Width;
                _formHeight = this.Height;
                _formMax = FormWindowState.Maximized == WindowState;
            }
        }

        private void EDDiscoveryForm_Resize(object sender, EventArgs e)
        {
            // We may be getting called by this.ResumeLayout() from InitializeComponent().
            if (EDDConfig != null && _shownOnce)
            {
                if (EDDConfig.UseNotifyIcon && EDDConfig.MinimizeToNotifyIcon)
                {
                    if (FormWindowState.Minimized == WindowState)
                        Hide();
                    else if (!Visible)
                        Show();
                }
                RecordPosition();
                notifyIconMenu_Open.Enabled = FormWindowState.Minimized == WindowState;
            }
        }

        private void EDDiscoveryForm_ResizeEnd(object sender, EventArgs e)
        {
            RecordPosition();
        }

        #endregion

        #region Updators

        public void NewTargetSet(Object sender)
        {
            if (OnNewTarget != null)
                OnNewTarget(sender);
        }

        public void NoteChanged(Object sender, HistoryEntry snc, bool committed)
        {
            if (OnNoteChanged != null)
                OnNoteChanged(sender, snc,committed);
        }

        #endregion

        #region Add Ons

        private void manageAddOnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actioncontroller.ManageAddOns();
        }

        private void buttonExtManageAddOns_Click(object sender, EventArgs e)
        {
            actioncontroller.ManageAddOns();
        }

        private void configureAddOnActionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actioncontroller.EditAddOns();
        }

        private void buttonExtEditAddOns_Click(object sender, EventArgs e)
        {
            actioncontroller.EditAddOns();
        }

        private void editLastActionPackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actioncontroller.EditLastPack();
        }

        private void stopCurrentlyRunningActionProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actioncontroller.TerminateAll();
        }

        public bool AddNewMenuItemToAddOns(string menu, string menutext, string icon , string menuname, string packname)
        {
            ToolStripMenuItem parent;

            menu = menu.ToLower();
            if (menu.Equals("add-ons"))
                parent = addOnsToolStripMenuItem;
            else if (menu.Equals("help"))
                parent = helpToolStripMenuItem;
            else if (menu.Equals("tools"))
                parent = toolsToolStripMenuItem;
            else if (menu.Equals("admin"))
                parent = adminToolStripMenuItem;
            else
                return false;

            Object res = Properties.Resources.ResourceManager.GetObject(icon);
            if ( res == null )
                res = EliteDangerous.Properties.Resources.ResourceManager.GetObject(icon);

            var x = (from ToolStripItem p in parent.DropDownItems where p.Text.Equals(menutext) && p.Tag != null && p.Name.Equals(menuname) select p);

            if (x.Count() == 0)           // double entries screened out of same menu text from same pack
            {
                ToolStripMenuItem it = new ToolStripMenuItem();
                it.Text = menutext;
                it.Name = menuname;
                it.Tag = packname;
                if (res != null && res is Bitmap)
                    it.Image = (Bitmap)res;
                it.Size = new Size(313, 22);
                it.Click += MenuTrigger_Click;
                parent.DropDownItems.Add(it);
            }

            return true;
        }

        public bool IsMenuItemInstalled(string menuname)
        {
            foreach( ToolStripMenuItem tsi in menuStrip1.Items )
            {
                List<ToolStripItem> presentlist = (from ToolStripItem s in tsi.DropDownItems where s.Name.Equals(menuname) select s).ToList();
                if (presentlist.Count() > 0)
                    return true;
            }

            return false;
        }


        public void RemoveMenuItemsFromAddOns(ToolStripMenuItem menu, string packname)
        {
            List<ToolStripItem> removelist = (from ToolStripItem s in menu.DropDownItems where s.Tag != null && ((string)s.Tag).Equals(packname) select s).ToList();
            foreach (ToolStripItem it in removelist)
            {
                menu.DropDownItems.Remove(it);
                it.Dispose();
            }
        }

        public void RemoveMenuItemsFromAddOns(string packname)
        {
            RemoveMenuItemsFromAddOns(addOnsToolStripMenuItem, packname);
            RemoveMenuItemsFromAddOns(helpToolStripMenuItem, packname);
            RemoveMenuItemsFromAddOns(toolsToolStripMenuItem, packname);
            RemoveMenuItemsFromAddOns(adminToolStripMenuItem, packname);
        }

        private void MenuTrigger_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem it = sender as ToolStripMenuItem;
            Conditions.ConditionVariables vars = new Conditions.ConditionVariables(new string[]
            {   "MenuName", it.Name,
                "MenuText", it.Text,
                "TopLevelMenuName" , it.OwnerItem.Name,
            });

            actioncontroller.ActionRun("onMenuItem", "UserUIEvent", null, vars);
        }

        public bool SelectTabPage(string name)
        {
            foreach (TabPage p in tabControlMain.TabPages)
            {
                if (p.Text.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    tabControlMain.SelectTab(p);
                    return true;
                }
            }

            return false;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActionRun("onTabChange", "UserUIEvent", null, new Conditions.ConditionVariables("TabName", tabControlMain.TabPages[tabControlMain.SelectedIndex].Text));
        }

        public Conditions.ConditionVariables Globals { get { return actioncontroller.Globals; } }

        public int ActionRunOnEntry(HistoryEntry he, string triggertype)
        { return actioncontroller.ActionRunOnEntry(he, triggertype); }

        public int ActionRun(string name, string triggertype, HistoryEntry he = null, Conditions.ConditionVariables additionalvars = null, string flagstart = null, bool now = false)
        { return actioncontroller.ActionRun(name, triggertype,he,additionalvars,flagstart,now); }

        #endregion

        #region Toolbar

        public void LoadCommandersListBox()
        {
            comboBoxCommander.Enabled = false;
            comboBoxCommander.Items.Clear();            // comboBox is nicer with items
            comboBoxCommander.Items.Add("Hidden Log");
            comboBoxCommander.Items.AddRange((from EDCommander c in EDCommander.GetList() select c.Name).ToList());
            if (history.CommanderId == -1)
            {
                comboBoxCommander.SelectedIndex = 0;
                buttonExtEDSMSync.Enabled = false;
            }
            else
            {
                comboBoxCommander.SelectedItem = EDCommander.Current.Name;
                buttonExtEDSMSync.Enabled = EDCommander.Current.SyncToEdsm | EDCommander.Current.SyncFromEdsm;
            }

            comboBoxCommander.Enabled = true;
        }

        private void comboBoxCommander_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCommander.SelectedIndex >= 0 && comboBoxCommander.Enabled)     // DONT trigger during LoadCommandersListBox
            {
                if (comboBoxCommander.SelectedIndex == 0)
                    RefreshHistoryAsync(currentcmdr: -1);                                   // which will cause DIsplay to be called as some point
                else
                {
                    var itm = (from EDCommander c in EDCommander.GetList() where c.Name.Equals(comboBoxCommander.Text) select c).ToList();

                    EDCommander.CurrentCmdrID = itm[0].Nr;
                    RefreshHistoryAsync(currentcmdr: EDCommander.CurrentCmdrID);                                   // which will cause DIsplay to be called as some point
                }
            }

        }

        private void buttonExt3dmap_Click(object sender, EventArgs e)
        {
            Open3DMap(travelHistoryControl.GetTravelHistoryCurrent);
        }

        private void buttonExt2dmap_Click(object sender, EventArgs e)
        {
            Open2DMap();
        }

        public void RefreshButton(bool state)
        {
            buttonExtRefresh.Enabled = state;
        }

        private void buttonExtRefresh_Click(object sender, EventArgs e)
        {
            LogLine("Refresh History.");
            RefreshHistoryAsync(checkedsm: true);
        }

        private void buttonExtEDSMSync_Click(object sender, EventArgs e)
        {
            EDSMClass edsm = new EDSMClass();

            if (!edsm.IsApiKeySet)
            {
                ExtendedControls.MessageBoxTheme.Show("Please ensure a commander is selected and it has a EDSM API key set");
                return;
            }

            try
            {
                EdsmSync.StartSync(edsm, history, EDCommander.Current.SyncToEdsm, EDCommander.Current.SyncFromEdsm, EDDConfig.Instance.DefaultMapColour);
            }
            catch (Exception ex)
            {
                LogLine($"EDSM Sync failed: {ex.Message}");
            }

        }

        private void comboBoxCustomPopOut_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCustomPopOut.Enabled)
            {
                PopOuts.PopOut((PopOutControl.PopOuts)(comboBoxCustomPopOut.SelectedIndex));

                comboBoxCustomPopOut.Enabled = false;
                comboBoxCustomPopOut.SelectedIndex = 0;
                comboBoxCustomPopOut.Enabled = true;
            }
        }

        #endregion

        #region PopOuts

        internal void SaveCurrentPopOuts()
        {
            PopOuts.SaveCurrentPopouts();
        }

        internal void LoadSavedPopouts()
        {
            PopOuts.LoadSavedPopouts();
        }

        #endregion

        #region Elite Input

        public void EliteInput(bool on, bool axisevents)
        {
            inputdevicesactions.Stop();
            inputdevices.Clear();

#if !__MonoCS__
            if (on)
            {
                DirectInputDevices.InputDeviceJoystickWindows.CreateJoysticks(inputdevices, axisevents);
                DirectInputDevices.InputDeviceKeyboard.CreateKeyboard(inputdevices);              // Created.. not started..
                DirectInputDevices.InputDeviceMouse.CreateMouse(inputdevices);
                frontierbindings.LoadBindingsFile();
                inputdevicesactions.Start();
            }
#endif
        }

        #endregion

        #region Misc

        public void SetUpLogging()      // controls logging of HTTP stuff
        {
            BaseUtils.HttpCom.LogPath = EDDConfig.Instance.EDSMLog ? EDDOptions.Instance.AppDataDirectory : null;
        }

        #endregion

        private void sendUnsyncedEGOScansToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<HistoryEntry> hlsyncunsyncedlist = Controller.history.FilterByScanNotEGOSynced;        // first entry is oldest
            EDDiscoveryCore.EGO.EGOSync.SendEGOEvents(LogLine, hlsyncunsyncedlist);
        }

    }
}



