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
 * EDDiscovery is not affiliated with Fronter Developments plc.
 */
using EDDiscovery.DB;
using EDDiscovery2;
using EDDiscovery2.DB;
using EDDiscovery2.EDSM;
using EDDiscovery2.Forms;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Configuration;
using EDDiscovery.EDSM;
using System.Threading.Tasks;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;
using EDDiscovery.HTTP;
using EDDiscovery.Forms;
using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;
using EDDiscovery.EDDN;

namespace EDDiscovery
{
    public partial class EDDiscoveryForm : Form, IDiscoveryController
    {
        #region Variables

        public const int WM_MOVE = 3;
        public const int WM_SIZE = 5;
        public const int WM_MOUSEMOVE = 0x200;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int WM_NCLBUTTONUP = 0xA2;
        public const int WM_NCMOUSEMOVE = 0xA0;
        public const int HT_CLIENT = 0x1;
        public const int HT_CAPTION = 0x2;
        public const int HT_LEFT = 0xA;
        public const int HT_RIGHT = 0xB;
        public const int HT_BOTTOM = 0xF;
        public const int HT_BOTTOMRIGHT = 0x11;
        public const int WM_NCL_RESIZE = 0x112;
        public const int HT_RESIZE = 61448;
        public const int WM_NCHITTEST = 0x84;

        // Mono compatibility
        private bool _window_dragging = false;
        private Point _window_dragMousePos = Point.Empty;
        private Point _window_dragWindowPos = Point.Empty;
        public EDDTheme theme;

        private EDDiscoveryController Controller;

        static public EDDConfig EDDConfig { get { return EDDConfig.Instance; } }

        public TravelHistoryControl TravelControl { get { return travelHistoryControl1; } }
        public RouteControl RouteControl { get { return routeControl1; } }
        public ExportControl ExportControl { get { return exportControl1; } }
        public EDDiscovery2.ImageHandler.ImageHandler ImageHandler { get { return imageHandler1; } }

        public EDDiscovery2._3DMap.MapManager Map { get; private set; }

        public event Action OnNewTarget;

        public Actions.ActionFileList actionfiles;
        public string actionfileskeyevents;
        ActionMessageFilter actionfilesmessagefilter;
        public Actions.ActionRun actionrunasync;
        private ConditionVariables internalglobalvariables;         // internally set variables, either program or user program ones
        private ConditionVariables usercontrolledglobalvariables;     // user variables, set by user only
        public ConditionVariables globalvariables;               // combo of above.

        Task checkInstallerTask = null;
        private bool themeok = true;
        private Forms.SplashForm splashform = null;

        GitHubRelease newRelease;

        public PopOutControl PopOuts;

        private bool _formMax;
        private int _formWidth;
        private int _formHeight;
        private int _formTop;
        private int _formLeft;

        #endregion

        #region IDiscoveryController interface
        #region Properties
        public HistoryList history { get { return Controller.history; } }
        public EDSMSync EdsmSync { get { return Controller.EdsmSync; } }
        public string LogText { get { return Controller.LogText; } }
        public bool PendingClose { get { return Controller.PendingClose; } }
        public GalacticMapping galacticMapping { get { return Controller.galacticMapping; } }
        #endregion

        #region Events
        public event Action<HistoryList> OnHistoryChange { add { Controller.OnHistoryChange += value; } remove { Controller.OnHistoryChange -= value; } }
        public event Action<HistoryEntry, HistoryList> OnNewEntry { add { Controller.OnNewEntry += value; } remove { Controller.OnNewEntry -= value; } }
        public event Action<JournalEntry> OnNewJournalEntry { add { Controller.OnNewJournalEntry += value; } remove { Controller.OnNewJournalEntry -= value; } }
        public event Action<string, Color> OnNewLogEntry { add { Controller.OnNewLogEntry += value; } remove { Controller.OnNewLogEntry -= value; } }
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
            Controller = new EDDiscoveryController(() => theme.TextBlockColor, () => theme.TextBlockHighlightColor, () => theme.TextBlockSuccessColor, a => Invoke(a), a => BeginInvoke(a));
            Controller.OnNewEntry += (he, hl) => ActionRunOnEntry(he, "NewEntry");
            Controller.OnDbInitComplete += Controller_DbInitComplete;
            Controller.OnBgSafeClose += Controller_BgSafeClose;
            Controller.OnFinalClose += Controller_FinalClose;
            Controller.OnInitialSyncComplete += Controller_InitialSyncComplete;
            Controller.OnRefreshCommanders += Controller_RefreshCommanders;
            Controller.OnRefreshComplete += Controller_RefreshComplete;
            Controller.OnRefreshStarting += Controller_RefreshStarting;
            Controller.OnReportProgress += Controller_ReportProgress;
            Controller.OnSyncComplete += Controller_SyncComplete;
            Controller.OnSyncStarting += Controller_SyncStarting;
            Controller.Init(Control.ModifierKeys.HasFlag(Keys.Shift));

            InitializeComponent();

            label_version.Text = EDDConfig.Options.VersionDisplayString;

            theme = new EDDTheme();

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

            Map = new EDDiscovery2._3DMap.MapManager(EDDConfig.Options.NoWindowReposition, this);

            this.TopMost = EDDConfig.KeepOnTop;

            ApplyTheme();

            notifyIcon1.Visible = EDDConfig.UseNotifyIcon;
        }

        private void EDDiscoveryForm_Layout(object sender, LayoutEventArgs e)       // Manually position, could not get gripper under tab control with it sizing for the life of me
        {
        }

        private void EDDiscoveryForm_Load(object sender, EventArgs e)
        {
            try
            {
                usercontrolledglobalvariables = new ConditionVariables();
                usercontrolledglobalvariables.FromString(SQLiteConnectionUser.GetSettingString("UserGlobalActionVars", ""), ConditionVariables.FromMode.MultiEntryComma);

                globalvariables = new ConditionVariables(usercontrolledglobalvariables);        // copy existing user ones into to shared buffer..

                internalglobalvariables = new ConditionVariables();

                SetInternalGlobal("CurrentCulture", System.Threading.Thread.CurrentThread.CurrentCulture.Name);
                SetInternalGlobal("CurrentCultureInEnglish", System.Threading.Thread.CurrentThread.CurrentCulture.EnglishName);
                SetInternalGlobal("CurrentCultureISO", System.Threading.Thread.CurrentThread.CurrentCulture.ThreeLetterISOLanguageName);

                Controller.PostInit_Loading();

                if (!(SQLiteConnectionUser.IsInitialized && SQLiteConnectionSystem.IsInitialized))
                {
                    splashform = new SplashForm();
                    splashform.ShowDialog(this);
                }

                Controller.PostInit_Loaded();

                RepositionForm();
                InitFormControls();
                settings.InitSettingsTab();
                savedRouteExpeditionControl1.LoadControl();
                travelHistoryControl1.LoadControl();

                if (EDDConfig.Options.Debug)
                {
                    button_test.Visible = true;
                }

                StartUpActions();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("EDDiscoveryForm_Load exception: " + ex.Message + "\n" + "Trace: " + ex.StackTrace);
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

        private void EDDiscoveryForm_Shown(object sender, EventArgs e)
        {
            Controller.PostInit_Shown();

            if (!themeok)
            {
                Controller.LogLineHighlight("The theme stored has missing colors or other missing information");
                Controller.LogLineHighlight("Correct the missing colors or other information manually using the Theme Editor in Settings");
            }

            ActionRunOnEvent("onStartup", "ProgramEvent");
        }

        private Task CheckForNewInstallerAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                CheckForNewinstaller();
            });
        }

        private bool CheckForNewinstaller()
        {
            try
            {

                GitHubClass github = new GitHubClass(this);

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
        private void Controller_DbInitComplete()
        {
            if (splashform != null)
            {
                splashform.Close();
            }
        }

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
            ActionRunOnEvent("onRefreshStart", "ProgramEvent");
        }

        private void Controller_RefreshCommanders()
        {
            string prevcommander = globalvariables.ContainsKey("Commander") ? globalvariables["Commander"] : "None";
            string commander = (Controller.history.CommanderId < 0) ? "Hidden" : EDDConfig.Instance.CurrentCommander.Name;

            string refreshcount = prevcommander.Equals(commander) ? internalglobalvariables.AddToVar("RefreshCount", 1, 1) : "1";
            SetInternalGlobal("RefreshCount", refreshcount);
            SetInternalGlobal("Commander", commander);

            travelHistoryControl1.LoadCommandersListBox();             // in case a new commander has been detected
            exportControl1.PopulateCommanders();
            settings.UpdateCommandersListBox();
        }

        private void Controller_RefreshComplete()
        {
            travelHistoryControl1.RefreshButton(true);
            journalViewControl1.RefreshButton(true);

            ActionRunOnEvent("onRefreshEnd", "ProgramEvent");
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

        private void Controller_BgSafeClose()
        {
        }

        private void Controller_FinalClose()
        {
            SaveSettings();         // do close now
            notifyIcon1.Visible = false;
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
                Actions.ActionSay.KillSpeech();
                Controller.Shutdown();
            }
            else
            {
                Console.WriteLine("go for close");
            }
        }

        #endregion

        #region Buttons, Mouse, Menus, NotifyIcon

        private void button_test_Click(object sender, EventArgs e)
        {
            ActionRunOnEvent("onStartup", "ProgramEvent");
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
                EDCommander cmdr = EDDConfig.Instance.ListOfCommanders.Find(x => x.Nr == EDDConfig.Instance.CurrentCmdrID);

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
                MessageBox.Show("Synchronisation to databases is in operation or pending, please wait");
        }

        private void syncEDSMSystemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Controller.AsyncPerformSync(edsmsync: true))      // we want it to have run, to completion, to allow another go..
                MessageBox.Show("Synchronisation to databases is in operation or pending, please wait");
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
            if (MessageBox.Show("Confirm you wish to reset the assigned EDSM IDs to all the current commander history entries," +
                                " and clear all the assigned EDSM IDs in all your notes for all commanders\r\n\r\n" +
                                "This will not change your history, but when you next refresh, it will try and reassign EDSM systems to " +
                                "your history and notes.  Use only if you think that the assignment of EDSM systems to entries is grossly wrong," +
                                "or notes are going missing\r\n" +
                                "\r\n" +
                                "You can manually change one EDSM assigned system by right clicking on the travel history and selecting the option"
                                , "WARNING", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                EliteDangerous.JournalEntry.ClearEDSMID(EDDConfig.CurrentCommander.Nr);
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
                EDCommander cmdr = EDDConfig.ListOfCommanders.Find(c => c.Nr == Controller.history.CommanderId);
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
                            EDDConfig.UpdateCommanders(new List<EDCommander> { cmdr }, true);
                        }

                        //string logpath = "c:\\games\\edlaunch\\products\\elite-dangerous-64\\logs";
                        Controller.RefreshHistoryAsync(netlogpath: logpath, forcenetlogreload: force, currentcmdr: cmdr.Nr);
                    }
                }
            }
        }

        private void dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm you wish to reset all history entries to the current commander", "WARNING", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                EliteDangerous.JournalEntry.ResetCommanderID(-1, EDDConfig.CurrentCommander.Nr);
                Controller.RefreshHistoryAsync();
            }
        }


        private void rescanAllJournalFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controller.RefreshHistoryAsync(forcejournalreload: true, checkedsm: true);
        }

        private void checkForNewReleaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckForNewinstaller())
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
                MessageBox.Show("No new release found", "EDDiscovery", MessageBoxButtons.OK);
            }
        }

        private void deleteDuplicateFSDJumpEntriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm you remove any duplicate FSD entries from the current commander", "WARNING", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                int n = EliteDangerous.JournalEntry.RemoveDuplicateFSDEntries(EDDConfig.CurrentCommander.Nr);
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

        public void Open3DMap(HistoryEntry he)
        {
            this.Cursor = Cursors.WaitCursor;

            string HomeSystem = settings.MapHomeSystem;

            Controller.history.FillInPositionsFSDJumps();

            Map.Prepare(he?.System, HomeSystem,
                        settings.MapCentreOnSelection ? he?.System : SystemClass.GetSystem(String.IsNullOrEmpty(HomeSystem) ? "Sol" : HomeSystem),
                        settings.MapZoom, Controller.history.FilterByTravel);
            Map.Show();
            this.Cursor = Cursors.Default;
        }

        public void Open2DMap()
        {
            this.Cursor = Cursors.WaitCursor;
            FormSagCarinaMission frm = new FormSagCarinaMission(Controller.history.FilterByFSDAndPosition);
            frm.Nowindowreposition = EDDConfig.Options.NoWindowReposition;
            frm.Show();
            this.Cursor = Cursors.Default;
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
            if (m.Msg == WM_LBUTTONDOWN && (int)m.WParam == 1 && !theme.WindowsFrame)
            {
                int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                int y = unchecked((short)((uint)m.LParam >> 16));
                _window_dragMousePos = new Point(x, y);
                _window_dragWindowPos = this.Location;
                _window_dragging = true;
                m.Result = IntPtr.Zero;
                this.Capture = true;
            }
            else if (m.Msg == WM_MOUSEMOVE && (int)m.WParam == 1 && _window_dragging)
            {
                int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                int y = unchecked((short)((uint)m.LParam >> 16));
                Point delta = new Point(x - _window_dragMousePos.X, y - _window_dragMousePos.Y);
                _window_dragWindowPos = new Point(_window_dragWindowPos.X + delta.X, _window_dragWindowPos.Y + delta.Y);
                this.Location = _window_dragWindowPos;
                this.Update();
                m.Result = IntPtr.Zero;
            }
            else if (m.Msg == WM_LBUTTONUP)
            {
                _window_dragging = false;
                _window_dragMousePos = Point.Empty;
                _window_dragWindowPos = Point.Empty;
                m.Result = IntPtr.Zero;
                this.Capture = false;
            }
            // Windows honours NCHITTEST; Mono does not
            else if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m);
                //System.Diagnostics.Debug.WriteLine( Environment.TickCount + " Res " + ((int)m.Result));

                if ((int)m.Result == HT_CLIENT)
                {
                    int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                    int y = unchecked((short)((uint)m.LParam >> 16));
                    Point p = PointToClient(new Point(x, y));

                    if (p.X > this.ClientSize.Width - statusStrip1.Height && p.Y > this.ClientSize.Height - statusStrip1.Height)
                    {
                        m.Result = (IntPtr)HT_BOTTOMRIGHT;
                    }
                    else if (p.Y > this.ClientSize.Height - statusStrip1.Height)
                    {
                        m.Result = (IntPtr)HT_BOTTOM;
                    }
                    else if (p.X > this.ClientSize.Width - 5)       // 5 is generous.. really only a few pixels gets thru before the subwindows grabs them
                    {
                        m.Result = (IntPtr)HT_RIGHT;
                    }
                    else if (p.X < 5)
                    {
                        m.Result = (IntPtr)HT_LEFT;
                    }
                    else if (!theme.WindowsFrame)
                    {
                        m.Result = (IntPtr)HT_CAPTION;
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
            if (EDDConfig != null)
            {
                if (EDDConfig.UseNotifyIcon && EDDConfig.MinimizeToNotifyIcon)
                {
                    if (FormWindowState.Minimized == WindowState)
                        Hide();
                    else
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

        #region Actions



        public void StartUpActions()
        {
            actionfiles = new Actions.ActionFileList();
            actionfiles.LoadAllActionFiles();
            actionrunasync = new Actions.ActionRun(this, actionfiles, true);        // this is the guy who runs programs asynchronously

            ActionConfigureKeys();
        }

        public void EditAddOnActionFile()
        {
            EDDiscovery2.ConditionFilterForm frm = new ConditionFilterForm();

            List<string> events = EDDiscovery.EliteDangerous.JournalEntry.GetListOfEventsWithOptMethod(false);
            events.Add("All");
            events.Add("onRefreshStart");
            events.Add("onRefreshEnd");
            events.Add("onStartup");
            events.Add("onShutdown");
            events.Add("onKeyPress");
            //events.Add("onClosedown");

            frm.InitAction("Actions: Define actions", events, globalvariables.KeyList, usercontrolledglobalvariables, actionfiles, theme);
            frm.TopMost = this.FindForm().TopMost;

            frm.ShowDialog(this.FindForm()); // don't care about the result, the form does all the saving

            usercontrolledglobalvariables = frm.userglobalvariables;
            SQLiteConnectionUser.PutSettingString("UserGlobalActionVars", usercontrolledglobalvariables.ToString());

            globalvariables = new ConditionVariables(internalglobalvariables, usercontrolledglobalvariables);    // remake

            ActionConfigureKeys();
        }

        public void ManageAddOns()
        {

        }

        public void ConfigureVoice()
        {

        }

        public int ActionRunOnEntry(HistoryEntry he, string triggertype, string flagstart = null)       //set flagstart to be the first flag of the actiondata..
        {
            List<Actions.ActionFileList.MatchingSets> ale = actionfiles.GetMatchingConditions(he.journalEntry.EventTypeStr, flagstart);

            if (ale.Count > 0)
            {
                ConditionVariables testvars = new ConditionVariables(globalvariables);
                Actions.ActionVars.TriggerVars(testvars, he.journalEntry.EventTypeStr, triggertype);
                Actions.ActionVars.HistoryEventVars(testvars, he, "Event");

                ConditionFunctions functions = new ConditionFunctions(this, Controller.history, he);                   // function handler

                if (actionfiles.CheckActions(ale, he.journalEntry.EventDataString, testvars, functions.ExpandString) > 0)
                {
                    ConditionVariables eventvars = new ConditionVariables();        // we don't pass globals in - added when they are run
                    Actions.ActionVars.TriggerVars(eventvars, he.journalEntry.EventTypeStr, triggertype);
                    Actions.ActionVars.HistoryEventVars(eventvars, he, "Event");
                    eventvars.GetJSONFieldNamesAndValues(he.journalEntry.EventDataString, "EventJS_");        // for all events, add to field list

                    actionfiles.RunActions(ale, actionrunasync, eventvars, Controller.history, he);  // add programs to action run

                    actionrunasync.Execute();       // will execute
                }
            }

            return ale.Count;
        }

        public int ActionRunOnEvent(string name, string triggertype)
        {
            List<Actions.ActionFileList.MatchingSets> ale = actionfiles.GetMatchingConditions(name);

            if (ale.Count > 0)
            {
                ConditionVariables testvars = new ConditionVariables(globalvariables);
                Actions.ActionVars.TriggerVars(testvars, name, triggertype);

                ConditionFunctions functions = new ConditionFunctions(this, Controller.history, null);                   // function handler

                if (actionfiles.CheckActions(ale, null, testvars, functions.ExpandString) > 0)
                {
                    ConditionVariables eventvars = new ConditionVariables();
                    Actions.ActionVars.TriggerVars(eventvars, name, triggertype);

                    actionfiles.RunActions(ale, actionrunasync, eventvars, Controller.history, null);  // add programs to action run

                    actionrunasync.Execute();       // will execute
                }
            }

            return ale.Count;
        }

        private void SetInternalGlobal(string name, string value)
        {
            internalglobalvariables[name] = globalvariables[name] = value;
        }

        public void SetProgramGlobal(string name, string value)     // different name for identification purposes
        {
            internalglobalvariables[name] = globalvariables[name] = value;
        }

        void ActionConfigureKeys()
        {
            List<Tuple<string, ConditionLists.MatchType>> ret = actionfiles.ReturnValuesOfSpecificConditions("KeyPress", new List<ConditionLists.MatchType>() { ConditionLists.MatchType.Equals, ConditionLists.MatchType.IsOneOf });        // need these to decide

            if (ret.Count > 0)
            {
                actionfileskeyevents = "";
                foreach (Tuple<string, ConditionLists.MatchType> t in ret)                  // go thru the list, making up a comparision string with Name, on it..
                {
                    if (t.Item2 == ConditionLists.MatchType.Equals)
                        actionfileskeyevents += "<" + t.Item1 + ">";
                    else
                    {
                        StringParser p = new StringParser(t.Item1);
                        List<string> klist = p.NextQuotedWordList();
                        if (klist != null)
                        {
                            foreach (string s in klist)
                                actionfileskeyevents += "<" + s + ">";
                        }
                    }
                }

                if (actionfilesmessagefilter == null)
                {
                    actionfilesmessagefilter = new ActionMessageFilter(this);
                    Application.AddMessageFilter(actionfilesmessagefilter);
                    System.Diagnostics.Debug.WriteLine("Installed message filter for keys");
                }
            }
            else if (actionfilesmessagefilter != null)
            {
                Application.RemoveMessageFilter(actionfilesmessagefilter);
                actionfilesmessagefilter = null;
                System.Diagnostics.Debug.WriteLine("Removed message filter for keys");
            }
        }

        public bool CheckKeys(string keyname)
        {
            if (actionfileskeyevents.Contains("<" + keyname + ">"))  // fast string comparision to determine if key is overridden..
            {
                globalvariables["KeyPress"] = keyname;          // only add it to global variables, its not kept in internals.
                ActionRunOnEvent("onKeyPress", "KeyPress");
                return true;
            }
            else
                return false;
        }

        const int WM_KEYDOWN = 0x100;
        const int WM_KEYCHAR = 0x102;
        const int WM_SYSKEYDOWN = 0x104;

        public class ActionMessageFilter : IMessageFilter
        {
            EDDiscoveryForm discoveryForm;
            public ActionMessageFilter(EDDiscoveryForm ed)
            {
                discoveryForm = ed;
            }

            public bool PreFilterMessage(ref Message m)
            {
                if (m.Msg == WM_KEYDOWN || m.Msg == WM_SYSKEYDOWN)
                {
                    Keys k = (Keys)m.WParam;
                    if (k != Keys.ControlKey && k != Keys.ShiftKey && k != Keys.Menu)
                    {
                        //System.Diagnostics.Debug.WriteLine("Keydown " + m.LParam + " " + k.ToString(Control.ModifierKeys) + " " + m.WParam + " " + Control.ModifierKeys);
                        if (discoveryForm.CheckKeys(k.ToString(Control.ModifierKeys)))
                            return true;    // swallow, we did it
                    }
                }

                return false;
            }
        }

        private void manageAddOnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageAddOns();
        }

        private void configureAddOnActionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditAddOnActionFile();
        }

        private void speechSynthesisSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigureVoice();
        }

        private void stopCurrentlyRunningActionProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionrunasync.TerminateAll();
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

         #endregion
    }
}


