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
using EDDiscovery.EDDN;
using EDDiscovery.EDSM;
using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;
using EDDiscovery.Forms;
using EDDiscovery.Export;
using EDDiscovery.HTTP;
using EDDiscovery.Win32Constants;
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

        public TravelHistoryControl TravelControl { get { return travelHistoryControl1; } }
        public RouteControl RouteControl { get { return routeControl1; } }
        public ExportControl ExportControl { get { return exportControl1; } }
        public EDDiscovery.ImageHandler.ImageHandler ImageHandler { get { return imageHandler1; } }

        public Audio.AudioQueue AudioQueueWave { get { return audioqueuewave; } }
        public Audio.AudioQueue AudioQueueSpeech { get { return audioqueuespeech; } }
        public Audio.SpeechSynthesizer SpeechSynthesizer { get { return speechsynth; } }

        Audio.IAudioDriver audiodriverwave;
        Audio.AudioQueue audioqueuewave;
        Audio.IAudioDriver audiodriverspeech;
        Audio.AudioQueue audioqueuespeech;
        Audio.SpeechSynthesizer speechsynth;

        public CompanionAPI.CompanionAPIClass Capi { get; private set; } = new CompanionAPI.CompanionAPIClass();

        public EDDiscovery._3DMap.MapManager Map { get; private set; }

        public event Action OnNewTarget;

        Task checkInstallerTask = null;
        private bool themeok = true;

        GitHubRelease newRelease;

        public PopOutControl PopOuts;

        private bool _shownOnce = false;
        private bool _initialised = false;
        private bool _formMax;
        private int _formWidth;
        private int _formHeight;
        private int _formTop;
        private int _formLeft;

        private HistoryEntry _uncommittedNoteHistoryEntry;

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
        public event Action<CompanionAPI.CompanionAPIClass,HistoryEntry> OnNewCompanionAPIData;
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

        #region Star Distance Calculation
        public void CalculateClosestSystems(ISystem sys, Action<ISystem, SortedList<double, ISystem>> callback, bool ignoreDuplicates = true)
        {
            Controller.CalculateClosestSystems(sys, callback, ignoreDuplicates);
        }
        #endregion
        #endregion

        #region Initialisation

        public EDDiscoveryForm()
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

        public void Init()
        {
            _initialised = true;
            Controller.Init();

            // Some components require the controller to be initialized
            InitializeComponent();

            label_version.Text = EDDConfig.Options.VersionDisplayString;

            PopOuts = new PopOutControl(this);

            ToolStripManager.Renderer = theme.toolstripRenderer;
            theme.LoadThemes();                                         // default themes and ones on disk loaded
            themeok = theme.RestoreSettings();                                    // theme, remember your saved settings

            trilaterationControl.InitControl(this);
            travelHistoryControl1.InitControl(this);
            imageHandler1.InitControl(this);
            settings.InitControl(this);
            journalViewControl1.InitControl(this, 0);
            routeControl1.InitControl(this);
            savedRouteExpeditionControl1.InitControl(this);
            exportControl1.InitControl(this);

            Map = new EDDiscovery._3DMap.MapManager(EDDConfig.Options.NoWindowReposition, this);

            this.TopMost = EDDConfig.KeepOnTop;

#if !__MonoCS__
            // Windows TTS (2000 and above). Speech *recognition* will be Version.Major >= 6 (Vista and above)
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 5)
            {
                audiodriverwave = new Audio.AudioDriverCSCore( EDDConfig.DefaultWaveDevice );
                audiodriverspeech = new Audio.AudioDriverCSCore( EDDConfig.DefaultVoiceDevice );
                speechsynth = new Audio.SpeechSynthesizer(new Audio.WindowsSpeechEngine());
            }
            else
            {
                audiodriverwave = new Audio.AudioDriverDummy();
                audiodriverspeech = new Audio.AudioDriverDummy();
                speechsynth = new Audio.SpeechSynthesizer(new Audio.DummySpeechEngine());
            }
#else
            audiodriverwave = new Audio.AudioDriverDummy();
            audiodriverspeech = new Audio.AudioDriverDummy();
            speechsynth = new Audio.SpeechSynthesizer(new Audio.DummySpeechEngine());
#endif
            audioqueuewave = new Audio.AudioQueue(audiodriverwave);
            audioqueuespeech = new Audio.AudioQueue(audiodriverspeech);

            actioncontroller = new Actions.ActionController(this, Controller);

            ApplyTheme();

            notifyIcon1.Visible = EDDConfig.UseNotifyIcon;
        }

        private void EDDiscoveryForm_Layout(object sender, LayoutEventArgs e)       // Manually position, could not get gripper under tab control with it sizing for the life of me
        {
        }

        // OnLoad is called the first time the form is shown, before OnShown or OnActivated are called
        private void EDDiscoveryForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (!_initialised)
                {
                    Init();
                }

                Controller.PostInit_Loaded();

                RepositionForm();
                InitFormControls();
                settings.InitSettingsTab();
                savedRouteExpeditionControl1.LoadControl();
                travelHistoryControl1.LoadControl();

                if (EDDConfig.Options.Debug)
                {
                    buttonReloadActions.Visible = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("EDDiscoveryForm_Load exception: " + ex.Message + "\n" + "Trace: " + ex.StackTrace);
            }
        }

        internal void SaveCurrentPopOuts()
        {
            PopOuts.SaveCurrentPopouts();
        }

        internal void LoadSavedPopouts()
        {
            PopOuts.LoadSavedPopouts();
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

            actioncontroller.ActionRun("onStartup", "ProgramEvent");
            _shownOnce = true;
        }

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

                GitHubClass github = new GitHubClass(LogLine);

                GitHubRelease rel = github.GetLatestRelease();

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
                        this.BeginInvoke(new Action(() => PanelInfoNewRelease()));
                        return true;
                    }
                }
            }
            catch (Exception)
            {

            }

            return false;
        }

        private void PanelInfoNewRelease()
        {
            ShowInfoPanel("Download new release!", true, Color.Green);
        }


        private void InitFormControls()
        {
            ShowInfoPanel("Loading. Please wait!", true, Color.Gold);

            routeControl1.travelhistorycontrol1 = travelHistoryControl1;
        }

        private void RepositionForm()
        {
            var top = SQLiteDBClass.GetSettingInt("FormTop", -1);
            if (top != -1 && EDDConfig.Options.NoWindowReposition == false)
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

            travelHistoryControl1.LoadLayoutSettings();
            journalViewControl1.LoadLayoutSettings();
            if (EDDConfig.AutoLoadPopOuts && EDDConfig.Options.NoWindowReposition == false)
                PopOuts.LoadSavedPopouts();
        }

        private void EDDiscoveryForm_Activated(object sender, EventArgs e)
        {
        }

        public void ApplyTheme()
        {
            ToolStripManager.Renderer = theme.toolstripRenderer;
            panel_close.Visible = !theme.WindowsFrame;
            panel_minimize.Visible = !theme.WindowsFrame;
            label_version.Visible = !theme.WindowsFrame;

            this.Text = "EDDiscovery " + label_version.Text;            // note in no border mode, this is not visible on the title bar but it is in the taskbar..

            theme.ApplyToForm(this);

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

            routeControl1.travelhistorycontrol1 = travelHistoryControl1;
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
            travelHistoryControl1.RefreshButton(false);
            journalViewControl1.RefreshButton(false);
            actioncontroller.ActionRun("onRefreshStart", "ProgramEvent");
        }

        private void Controller_RefreshCommanders()
        {
            travelHistoryControl1.LoadCommandersListBox();             // in case a new commander has been detected
            exportControl1.PopulateCommanders();
            settings.UpdateCommandersListBox();
        }

        private void Controller_RefreshComplete()
        {
            travelHistoryControl1.RefreshButton(true);
            journalViewControl1.RefreshButton(true);
            actioncontroller.ActionRunOnRefresh();

            if (!Capi.IsCommanderLoggedin(EDCommander.Current.Name))
            {
                Capi.Logout();

                if (CompanionAPI.CompanionCredentials.CredentialState(EDCommander.Current.Name) == CompanionAPI.CompanionCredentials.State.CONFIRMED)
                {
                    try
                    {
                        Capi.LoginAs(EDCommander.Current.Name);
                        LogLine("Logged into Companion API");
                    }
                    catch (Exception ex)
                    {
                        LogLineHighlight("Companion API log in failed: " + ex.Message);
                        if (!(ex is CompanionAPI.CompanionAppException))
                            LogLineHighlight(ex.StackTrace);
                    }
                }
            }

            if ( Capi.LoggedIn )
            {
                try
                {
                    Capi.GetProfile();
                    OnNewCompanionAPIData?.Invoke(Capi, null);
                }
                catch (Exception ex)
                {
                    LogLineHighlight("Companion API get failed: " + ex.Message);
                    if (!(ex is CompanionAPI.CompanionAppException))
                        LogLineHighlight(ex.StackTrace);

                    // what do we do TBD
                }
            }
        }

        private void Controller_NewEntrySecond(HistoryEntry he, HistoryList hl)         // called after all UI's have had their chance
        {
            actioncontroller.ActionRunOnEntry(he, "NewEntry");

            if ( he.EntryType == JournalTypeEnum.Docked )
            {
                if (Capi.IsCommanderLoggedin(EDCommander.Current.Name))
                {
                    System.Diagnostics.Debug.WriteLine("Commander " + EDCommander.Current.Name + " in CAPI");
                    try
                    {
                        Capi.GetProfile();
                        JournalEDDCommodityPrices entry = JournalEntry.AddEDDCommodityPrices(EDCommander.Current.Nr, he.journalEntry.EventTimeUTC.AddSeconds(1), Capi.Profile.StarPort.name, Capi.Profile.StarPort.faction, Capi.Profile.StarPort.jcommodities);
                        if (entry != null)
                        {
                            Controller.NewEntry(entry);
                            OnNewCompanionAPIData?.Invoke(Capi, he);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogLineHighlight("Companion API get failed: " + ex.Message);
                    }
                }
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

        private void Controller_BgSafeClose()       // run in thread..
        {
            actioncontroller.CloseDown();
        }

        private void Controller_FinalClose()        // run in UI
        {
            StoreUncommittedNote();
            SaveSettings();         // do close now
            notifyIcon1.Visible = false;

            audioqueuespeech.Dispose();     // in order..
            audiodriverspeech.Dispose();
            audioqueuewave.Dispose();
            audiodriverwave.Dispose();

            Close();
            Application.Exit();
        }


#endregion

#region Closing
        private void SaveSettings()
        {
            settings.SaveSettings();

            SQLiteDBClass.PutSettingBool("FormMax", _formMax);
            SQLiteDBClass.PutSettingInt("FormWidth", _formWidth);
            SQLiteDBClass.PutSettingInt("FormHeight", _formHeight);
            SQLiteDBClass.PutSettingInt("FormTop", _formTop);
            SQLiteDBClass.PutSettingInt("FormLeft", _formLeft);
            routeControl1.SaveSettings();
            theme.SaveSettings(null);
            travelHistoryControl1.SaveSettings();
            journalViewControl1.SaveSettings();
            if (EDDConfig.AutoSavePopOuts)
                PopOuts.SaveCurrentPopouts();
        }

        public void ShowInfoPanel(string message, bool visible, Color? backColour = null)
        {
            labelPanelText.Text = message;
            panelInfo.Visible = visible;
            if (backColour.HasValue) panelInfo.BackColor = backColour.Value;
        }

        private void EDDiscoveryForm_FormClosing(object sender, FormClosingEventArgs e)
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

#endregion

#region Buttons, Mouse, Menus, NotifyIcon

        private void buttonReloadActions_Click(object sender, EventArgs e)
        {
            actioncontroller.ReLoad();
            actioncontroller.ActionRun("onStartup", "ProgramEvent");
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

        private void openEliteDangerousDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (EliteDangerousClass.EDDirectory != null && !EliteDangerousClass.EDDirectory.Equals(""))
                    Process.Start(EliteDangerousClass.EDDirectory);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Open EliteDangerous directory exception: " + ex.Message);
            }

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
                        cmdrfolder = EliteDangerous.EDJournalClass.GetDefaultJournalDir();
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
            TravelControl.buttonMap_Click(sender, e);
        }

        private void forceEDDBUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Controller.AsyncPerformSync(eddbsync: true))      // we want it to have run, to completion, to allow another go..
                EDDiscovery.Forms.MessageBoxTheme.Show("Synchronisation to databases is in operation or pending, please wait");
        }

        private void syncEDSMSystemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Controller.AsyncPerformSync(edsmsync: true))      // we want it to have run, to completion, to allow another go..
                EDDiscovery.Forms.MessageBoxTheme.Show("Synchronisation to databases is in operation or pending, please wait");
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

        private void AboutBox()
        {
            AboutForm frm = new AboutForm();
            frm.labelVersion.Text = this.Text;
            frm.TopMost = EDDiscoveryForm.EDDConfig.KeepOnTop;
            frm.ShowDialog(this);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox();
        }

        private void eDDiscoveryChatDiscordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Properties.Resources.URLProjectDiscord);
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
            if (EDDiscovery.Forms.MessageBoxTheme.Show("Confirm you wish to reset the assigned EDSM IDs to all the current commander history entries," +
                                " and clear all the assigned EDSM IDs in all your notes for all commanders\r\n\r\n" +
                                "This will not change your history, but when you next refresh, it will try and reassign EDSM systems to " +
                                "your history and notes.  Use only if you think that the assignment of EDSM systems to entries is grossly wrong," +
                                "or notes are going missing\r\n" +
                                "\r\n" +
                                "You can manually change one EDSM assigned system by right clicking on the travel history and selecting the option"
                                , "WARNING", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                EliteDangerous.JournalEntry.ClearEDSMID(EDCommander.CurrentCmdrID);
                SystemNoteClass.ClearEDSMID();
            }

        }


        private void paneleddiscovery_Click(object sender, EventArgs e)
        {
            AboutBox();
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
                    string netlogpath = cmdr.NetLogDir;
                    FolderBrowserDialog dirdlg = new FolderBrowserDialog();
                    if (netlogpath != null && Directory.Exists(netlogpath))
                    {
                        dirdlg.SelectedPath = netlogpath;
                    }

                    DialogResult dlgResult = dirdlg.ShowDialog();

                    if (dlgResult == DialogResult.OK)
                    {
                        string logpath = dirdlg.SelectedPath;

                        if (logpath != netlogpath)
                        {
                            cmdr.NetLogDir = logpath;
                            EDCommander.Update(new List<EDCommander> { cmdr }, true);
                        }

                        //string logpath = "c:\\games\\edlaunch\\products\\elite-dangerous-64\\logs";
                        Controller.RefreshHistoryAsync(netlogpath: logpath, forcenetlogreload: force, currentcmdr: cmdr.Nr);
                    }
                }
            }
        }

        private void dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EDDiscovery.Forms.MessageBoxTheme.Show("Confirm you wish to reset all history entries to the current commander", "WARNING", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                EliteDangerous.JournalEntry.ResetCommanderID(-1, EDCommander.CurrentCmdrID);
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
                EDDiscovery.Forms.MessageBoxTheme.Show(this,"No new release found", "EDDiscovery", MessageBoxButtons.OK);
            }
        }

        private void deleteDuplicateFSDJumpEntriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EDDiscovery.Forms.MessageBoxTheme.Show("Confirm you remove any duplicate FSD entries from the current commander", "WARNING", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                int n = EliteDangerous.JournalEntry.RemoveDuplicateFSDEntries(EDCommander.CurrentCmdrID);
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
            string homesysname = settings.MapHomeSystem;

            ISystem homesys = ((homesysname != null) ? SystemClass.GetSystem(homesysname) : null);

            if (homesys == null || !homesys.HasCoordinate)
            {
                homesys = SystemClass.GetSystem("Sol");

                if (homesys == null)
                {
                    homesys = new SystemClass("Sol", 0, 0, 0);
                }
            }

            return homesys;
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
            frm.Nowindowreposition = EDDConfig.Options.NoWindowReposition;
            frm.Show();
            this.Cursor = Cursors.Default;
        }

        public void StoreSystemNote(HistoryEntry he, string txt, bool send = false)
        {
            if (he != null && txt != null)
            {
                // Update the System Note; and if the text was changed, save our Note for sending and storing later
                // Also notify various "quick" listeners
                if (he.UpdateSystemNote(txt, send))
                {
                    if (_uncommittedNoteHistoryEntry == null)
                    {
                        _uncommittedNoteHistoryEntry = he;
                    }
                    travelHistoryControl1.UpdateNoteJID(he.Journalid, txt);
                    PopOuts.UpdateNoteJID(he.Journalid, txt);
                    // MKW TODO: Update the Note editor SPanel.
                }

                // If we have an uncommitted system note from another entry, or the send flag is set, commit to DB and send to EDSM
                // Also notify any "slow" listeners
                if (_uncommittedNoteHistoryEntry != null && (send || _uncommittedNoteHistoryEntry != he))
                {
                    StoreUncommittedNote();
                    Map.UpdateNote();
                }
            }
        }

        private void StoreUncommittedNote()
        {
            if (_uncommittedNoteHistoryEntry != null)
            {
                _uncommittedNoteHistoryEntry.CommitSystemNote();
                    if (EDCommander.Current.SyncToEdsm && _uncommittedNoteHistoryEntry.IsFSDJump)       // only send on FSD jumps
                        EDSMSync.SendComments(_uncommittedNoteHistoryEntry.snc.Name, _uncommittedNoteHistoryEntry.snc.Note, _uncommittedNoteHistoryEntry.snc.EdsmId);
                _uncommittedNoteHistoryEntry = null;
            }
        }

        private void sendUnsuncedEDDNEventsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<HistoryEntry> hlsyncunsyncedlist = Controller.history.FilterByScanNotEDDNSynced;        // first entry is oldest
            EDDNSync.SendEDDNEvents(this, hlsyncunsyncedlist);
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
            if (m.Msg == WM.LBUTTONDOWN && (int)m.WParam == 1 && !theme.WindowsFrame)
            {
                int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                int y = unchecked((short)((uint)m.LParam >> 16));
                _window_dragMousePos = new Point(x, y);
                _window_dragWindowPos = this.Location;
                _window_dragging = true;
                m.Result = IntPtr.Zero;
                this.Capture = true;
            }
            else if (m.Msg == WM.MOUSEMOVE && (int)m.WParam == 1 && _window_dragging)
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

                if ((int)m.Result == HT.CLIENT)
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

#region Targets

        public void NewTargetSet()
        {
            System.Diagnostics.Debug.WriteLine("New target set");
            if (OnNewTarget != null)
                OnNewTarget();
        }

#endregion

#region Add Ons

        private void manageAddOnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actioncontroller.ManageAddOns();
        }

        private void configureAddOnActionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actioncontroller.EditAddOnActionFile();
        }

        private void stopCurrentlyRunningActionProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actioncontroller.TerminateAll();
        }

        public bool AddNewMenuItemToAddOns(string menuname, string menuitemtext, string icon , string menutrigger, string packname)
        {
            ToolStripMenuItem parent;

            menuname = menuname.ToLower();
            if (menuname.Equals("add-ons"))
                parent = addOnsToolStripMenuItem;
            else if (menuname.Equals("help"))
                parent = helpToolStripMenuItem;
            else if (menuname.Equals("tools"))
                parent = toolsToolStripMenuItem;
            else if (menuname.Equals("admin"))
                parent = adminToolStripMenuItem;
            else
                return false;

            Object res = Properties.Resources.ResourceManager.GetObject(icon);

            var x = (from ToolStripItem p in parent.DropDownItems where p.Text.Equals(menuitemtext) && p.Tag != null && p.Name.Equals(menutrigger) select p);

            if (x.Count() == 0)           // double entries screened out
            {
                ToolStripMenuItem it = new ToolStripMenuItem();
                it.Text = menuitemtext;
                it.Name = menutrigger;
                it.Tag = packname;
                if (res != null && res is Bitmap)
                    it.Image = (Bitmap)res;
                it.Size = new Size(313, 22);
                it.Click += MenuTrigger_Click;
                parent.DropDownItems.Add(it);
            }

            return true;
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
            ConditionVariables vars = new ConditionVariables(new string[]
            {   "MenuName", it.Name,
                "MenuText", it.Text,
                "TopLevelMenuName" , it.OwnerItem.Name,
            });

            actioncontroller.ActionRun("onMenuItem", "UserUIEvent", null, vars);
        }

        public bool SelectTabPage(string name)
        {
            foreach (TabPage p in tabControl1.TabPages)
            {
                if (p.Text.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    tabControl1.SelectTab(p);
                    return true;
                }
            }

            return false;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActionRun("onTabChange", "UserUIEvent", null,new ConditionVariables("TabName", tabControl1.TabPages[tabControl1.SelectedIndex].Text));
        }

        public ConditionVariables Globals { get { return actioncontroller.Globals; } }

        public int ActionRunOnEntry(HistoryEntry he, string triggertype)
        { return actioncontroller.ActionRunOnEntry(he, triggertype); }

        public int ActionRun(string name, string triggertype, HistoryEntry he = null, ConditionVariables additionalvars = null, string flagstart = null, bool now = false)
        { return actioncontroller.ActionRun(name, triggertype,he,additionalvars,flagstart,now); }

#endregion

        private void companionAPILoginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCAPI frm = new FormCAPI();

            frm.Show();
        }
    }
}


