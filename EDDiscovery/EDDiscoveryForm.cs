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
using EliteDangerousCore.EDDN;
using EliteDangerousCore.EDSM;
using EDDiscovery.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.JournalEvents;
using EDDiscovery.Icons;

namespace EDDiscovery
{
    public partial class EDDiscoveryForm : Forms.DraggableFormPos
    { 
        #region Variables

        private EDDiscoveryController Controller;
        private Actions.ActionController actioncontroller;

        public EliteDangerousCore.DLL.EDDDLLManager DLLManager;
        public EliteDangerousCore.DLL.EDDDLLIF.EDDCallBacks DLLCallBacks;

        public WebServer.WebServer WebServer;

        public Actions.ActionController ActionController { get { return actioncontroller; } }

        static public EDDConfig EDDConfig { get { return EDDConfig.Instance; } }
        public EDDTheme theme { get { return EDDTheme.Instance; } }

        public UserControls.IHistoryCursor PrimaryCursor { get { return tabControlMain.PrimaryTab.GetTravelGrid; } }
        public UserControls.UserControlContainerSplitter PrimarySplitter { get { return tabControlMain.PrimaryTab; } }

        public ScreenShots.ScreenShotConverter screenshotconverter;

        public EDDiscovery._3DMap.MapManager Map { get; private set; }

        private bool themeok = true;
        private bool in_system_sync = false;        // between start/end sync of databases

        BaseUtils.GitHubRelease newRelease;

        public PopOutControl PopOuts;

        Timer datetimetimer;

        #endregion

        #region Callbacks from us
        public event Action<Object> OnNewTarget;
        public event Action<Object, HistoryEntry, bool> OnNoteChanged;  // UI.Note has been updated attached to this note
        public event Action<List<ISystem>> OnNewCalculatedRoute;        // route plotter has a new one
        public event Action OnAddOnsChanged;                            // add on changed
        public event Action<int,string> OnEDSMSyncComplete;             // EDSM Sync has completed with this list of stars are newly created
        public event Action<int> OnEDDNSyncComplete;                    // Sync has completed
        public event Action<int> OnIGAUSyncComplete;                    // Sync has completed
                                                                        // theme has changed by settings, hook if you have some UI which needs refreshing due to it. 
        public event Action OnThemeChanged;                             // Note you won't get it on startup because theme is applied to form before tabs/panels are setup
        #endregion


        #region Properties
        public HistoryList history { get { return Controller.history; } }
        public string LogText { get { return Controller.LogText; } }
        public bool PendingClose { get { return Controller.PendingClose; } }
        public GalacticMapping galacticMapping { get { return Controller.galacticMapping; } }
        #endregion

        #region Events - see the EDDiscoveryControl for meaning and context
        public event Action<HistoryList> OnHistoryChange { add { Controller.OnHistoryChange += value; } remove { Controller.OnHistoryChange -= value; } }
        public event Action<HistoryEntry, HistoryList> OnNewEntry { add { Controller.OnNewEntry += value; } remove { Controller.OnNewEntry -= value; } }
        public event Action<UIEvent> OnNewUIEvent { add { Controller.OnNewUIEvent += value; } remove { Controller.OnNewUIEvent -= value; } }
        public event Action<JournalEntry> OnNewJournalEntry { add { Controller.OnNewJournalEntry += value; } remove { Controller.OnNewJournalEntry -= value; } }
        public event Action<string, Color> OnNewLogEntry { add { Controller.OnNewLogEntry += value; } remove { Controller.OnNewLogEntry -= value; } }
        public event Action OnRefreshCommanders { add { Controller.OnRefreshCommanders += value; } remove { Controller.OnRefreshCommanders -= value; } }
        public event Action OnMapsDownloaded { add { Controller.OnMapsDownloaded += value; } remove { Controller.OnMapsDownloaded -= value; } }
        public event Action<bool> OnExpeditionsDownloaded { add { Controller.OnExpeditionsDownloaded += value; } remove { Controller.OnExpeditionsDownloaded -= value; } }

        #endregion

        #region Logging
        public void LogLine(string text) { Controller.LogLine(text); }
        public void LogLineHighlight(string text) { Controller.LogLineHighlight(text); }
        public void LogLineSuccess(string text) { Controller.LogLineSuccess(text); }
        public void LogLineColor(string text, Color color) { Controller.LogLineColor(text, color); }
        #endregion

        #region History
        public bool RefreshHistoryAsync()           // we only supply the basic refresh for the rest of the system..
        {
            return Controller.RefreshHistoryAsync();
        }
        public void RefreshDisplays() { Controller.RefreshDisplays(); }

        public void ChangeToCommander(int id)
        {
            EDCommander.CurrentCmdrID = id;
            Controller.RefreshHistoryAsync(currentcmdr: EDCommander.CurrentCmdrID);                                   // which will cause DIsplay to be called as some point
        }
        #endregion

        #region Initialisation

        // note we do not do the traditional Initialize component here.. we wait for splash form to call it
        // and we need to tell the drag form pos our save name
        public EDDiscoveryForm()
        {
            RestoreFormPositionRegKey = "Form";
            Controller = new EDDiscoveryController(() => theme.TextBlockColor, () => theme.TextBlockHighlightColor, 
                                                        () => theme.TextBlockSuccessColor, a => BeginInvoke(a));

            Controller.OnFinalClose += Controller_FinalClose;

            Controller.OnRefreshCommanders += Controller_RefreshCommanders;
            Controller.OnRefreshComplete += Controller_RefreshComplete;
            Controller.OnRefreshStarting += Controller_RefreshStarting;
            Controller.OnReportRefreshProgress += ReportRefreshProgress;

            Controller.OnSyncStarting += () => { edsmRefreshTimer.Enabled = false; in_system_sync = true; };
            Controller.OnSyncComplete += () => { edsmRefreshTimer.Enabled = true; in_system_sync = false; };
            Controller.OnReportSyncProgress += ReportSyncProgress;

            Controller.OnNewEntrySecond += Controller_NewEntrySecond;       // called after UI updates themselves with NewEntry
            Controller.OnNewUIEvent += Controller_NewUIEvent;       // called if its an UI event
        }

        public void Init(Action<string> msg)    // called from EDDApplicationContext .. continues on with the construction of the form
        {
            Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " ED init");        // STAGE 1

            msg.Invoke("Loading Translations");

            if (EDDOptions.Instance.ResetLanguage)
                EDDConfig.Instance.Language = "None";

            BaseUtils.Translator.Instance.LoadTranslation(EDDOptions.Instance.SelectLanguage ?? EDDConfig.Instance.Language, 
                    CultureInfo.CurrentUICulture, 
                    EDDOptions.Instance.TranslatorFolders(),
                    EDDOptions.Instance.TranslatorDirectoryIncludeSearchUpDepth, EDDOptions.Instance.AppDataDirectory);

            BaseUtils.Translator.Instance.AddExcludedControls(new Type[]
            {   typeof(ExtendedControls.ExtComboBox), typeof(ExtendedControls.NumberBoxDouble),typeof(ExtendedControls.NumberBoxFloat),typeof(ExtendedControls.NumberBoxLong),
                typeof(ExtendedControls.ExtScrollBar),typeof(ExtendedControls.ExtStatusStrip),typeof(ExtendedControls.ExtRichTextBox),typeof(ExtendedControls.ExtTextBox),
                typeof(ExtendedControls.ExtTextBoxAutoComplete),typeof(ExtendedControls.ExtDateTimePicker),typeof(ExtendedControls.ExtNumericUpDown) });

            Controller.Init();
            PanelInformation.Init();

            // Some components require the controller to be initialized
            // obsolete remove IconSet.SetPanelImageListGetter(PanelInformation.GetPanelImages);
            InitializeComponent();

            screenshotconverter = new ScreenShots.ScreenShotConverter(this);
            PopOuts = new PopOutControl(this);
            Map = new EDDiscovery._3DMap.MapManager(this);

            Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Load popouts, themes, init controls");        // STAGE 2 themeing the main interface (not the tab pages)
            msg.Invoke("Applying Themes");

            comboBoxCommander.AutoSize = comboBoxCustomProfiles.AutoSize = true;
            panelToolBar.HiddenMarkerWidth = 200;
            panelToolBar.SecondHiddenMarkerWidth = 60;
            panelToolBar.PinState = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("ToolBarPanelPinState", true);

            labelGameDateTime.Text = "";
            labelInfoBoxTop.Text = "";
            label_version.Text = EDDOptions.Instance.VersionDisplayString;

            theme.LoadThemes();                                         // default themes and ones on disk loaded

            if (!EDDOptions.Instance.NoTheme)
                themeok = theme.RestoreSettings();                                    // theme, remember your saved settings

            if (EDDOptions.Instance.FontSize > 0)
                theme.FontSize = EDDOptions.Instance.FontSize;

            if (EDDOptions.Instance.Font.HasChars())
                theme.FontName = EDDOptions.Instance.Font;

            ApplyTheme();                       // we apply and scale (because its being applied to Form) before any tabs parts are setup.

            this.TopMost = EDDConfig.KeepOnTop;
            notifyIcon1.Visible = EDDConfig.UseNotifyIcon;

            // open all the major tabs except the built in ones
            Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Creating major tabs Now");        // STAGE 3 Tabs

            if (EDDOptions.Instance.TabsReset)
            {
                EliteDangerousCore.DB.UserDatabase.Instance.DeleteKey("GridControlWindows%");              // these hold the grid/splitter control values for all windows
                EliteDangerousCore.DB.UserDatabase.Instance.DeleteKey("SplitterControlWindows%");          // wack them so they start empty.
                EliteDangerousCore.DB.UserDatabase.Instance.DeleteKey("SavedPanelInformation.%");          // and delete the pop out history
                EliteDangerousCore.DB.UserDatabase.Instance.DeleteKey("ProfilePowerOnID");                 // back to base profile
            }

            //Make sure the primary splitter is set up.. and rational

            UserControls.UserControlContainerSplitter.CheckPrimarySplitterControlSettings(EDDOptions.Instance.TabsReset ? "?????" : "TravelControl"); // Double check, use TravelControlBottom etc as the old lookup name if its nonsence

            if (!EDDOptions.Instance.NoTabs)
            {
                tabControlMain.MinimumTabWidth = 32;
                tabControlMain.CreateTabs(this, EDDOptions.Instance.TabsReset, "0, -1,0, 26,0, 27,0, 29,0, 34,0");      // numbers from popouts, which are FIXED!
                if (tabControlMain.PrimaryTab == null || tabControlMain.PrimaryTab.GetTravelGrid == null)  // double check we have a primary tab and tg..
                {
                    MessageBox.Show(("Tab setup failure: Primary tab or TG failed to load." + Environment.NewLine +
                                    "This is a abnormal condition - please problem to EDD Team on discord or github." + Environment.NewLine +
                                    "To try and clear it, hold down shift and then launch the program." + Environment.NewLine +
                                    "Click on Reset tabs, then Run program, which may clear the problem.").T(EDTx.EDDiscoveryForm_TSF));
                    Application.Exit();
                }
            }

            PanelInformation.PanelIDs[] pids = PanelInformation.GetUserSelectablePanelIDs(EDDConfig.Instance.SortPanelsByName);      // only user panels

            BaseUtils.Translator.Instance.Translate(contextMenuStripTabs, this);        // need to translate BEFORE we add in extra items
            BaseUtils.Translator.Instance.Translate(notifyIconContextMenuStrip, this);        // need to translate BEFORE we add in extra items

            foreach (PanelInformation.PanelIDs pid in pids)
            {
                ToolStripMenuItem tsmi = PanelInformation.MakeToolStripMenuItem(pid,
                    (s, e) => tabControlMain.AddTab((PanelInformation.PanelIDs)((s as ToolStripMenuItem).Tag), tabControlMain.LastTabClicked));

                if (tsmi != null)
                    addTabToolStripMenuItem.DropDownItems.Add(tsmi);

                ToolStripMenuItem tsmi2 = PanelInformation.MakeToolStripMenuItem(pid,
                    (s, e) => PopOuts.PopOut((PanelInformation.PanelIDs)((s as ToolStripMenuItem).Tag)));

                if ( tsmi2 != null)
                    popOutPanelToolStripMenuItem.DropDownItems.Add(tsmi2);
            }

            removeTabToolStripMenuItem.Click += (s, e) => tabControlMain.RemoveTab(tabControlMain.LastTabClicked);
            renameTabToolStripMenuItem.Click += (s, e) => 
            {
                string newvalue = ExtendedControls.PromptSingleLine.ShowDialog(this, 
                                "Name:".T(EDTx.EDDiscoveryForm_RTABL), tabControlMain.TabPages[tabControlMain.LastTabClicked].Text, 
                                "Rename Tab".T(EDTx.EDDiscoveryForm_RTABT), this.Icon, false, "Enter a new name for the tab".T(EDTx.EDDiscoveryForm_RTABTT));
                if (newvalue != null)
                    tabControlMain.RenameTab(tabControlMain.LastTabClicked, newvalue.Replace(";", "_"));
            };


            msg.Invoke("Loading Action Packs");         // STAGE 4 Action packs

            actioncontroller = new Actions.ActionController(this, Controller, this.Icon, new Type[] { typeof(FormMap) });
            actioncontroller.ReLoad();          // load system up here

            // Stage 5 Misc

            EDSMJournalSync.SentEvents = (count,list) =>              // Sync thread finishing, transfers to this thread, then runs the callback and the action..
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    System.Diagnostics.Debug.Assert(Application.MessageLoop);
                    OnEDSMSyncComplete?.Invoke(count,list);
                    ActionRun(Actions.ActionEventEDList.onEDSMSync, null, new BaseUtils.Variables(new string[] { "EventStarList", list, "EventCount", count.ToStringInvariant() }));
                });
            };

            EDDNSync.SentEvents = (count) =>              // Sync thread finishing, transfers to this thread, then runs the callback and the action..
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    System.Diagnostics.Debug.Assert(Application.MessageLoop);
                    OnEDDNSyncComplete?.Invoke(count);
                    ActionRun(Actions.ActionEventEDList.onEDDNSync, null, new BaseUtils.Variables(new string[] { "EventCount", count.ToStringInvariant() }));
                });
            };

            EliteDangerousCore.IGAU.IGAUSync.SentEvents = (count) =>              // Sync thread finishing, transfers to this thread, then runs the callback and the action..
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    System.Diagnostics.Debug.Assert(Application.MessageLoop);
                    OnIGAUSyncComplete?.Invoke(count);
                    ActionRun(Actions.ActionEventEDList.onIGAUSync, null, new BaseUtils.Variables(new string[] { "EventCount", count.ToStringInvariant() }));
                });
            };

            Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Finish ED Init");

            DLLManager = new EliteDangerousCore.DLL.EDDDLLManager();
            DLLCallBacks = new EliteDangerousCore.DLL.EDDDLLIF.EDDCallBacks();

            WebServer = new WebServer.WebServer(this);

            UpdateProfileComboBox();
            comboBoxCustomProfiles.SelectedIndexChanged += ComboBoxCustomProfiles_SelectedIndexChanged;

            BaseUtils.Translator.Instance.Translate(mainMenu, this);
            BaseUtils.Translator.Instance.Translate(toolTip,this);

            panelToolBar.SetToolTip(toolTip);    // use the defaults

            if (EDDOptions.Instance.ActionButton)
                buttonReloadActions.Visible = true;

       //     Hookable.Hook();
        }

        // OnLoad is called the first time the form is shown, before OnShown or OnActivated are called

        private void EDDiscoveryForm_Load(object sender, EventArgs e)
        {
            if (!EDDOptions.Instance.NoTabs)        // load the tabs so when shown is done they are there..
                tabControlMain.LoadTabs();
        }

        // OnShown is called once
        private void EDDiscoveryForm_Shown(object sender, EventArgs e)
        {
            Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " EDF shown");

            Controller.PostInit_Shown();        // form is up, controller is released

            if (EDDConfig.Instance.EDSMGridIDs == "Not Set")        // initial state
            {
                EDDConfig.Instance.EDSMEDDBDownload = false;        // this stops the download working in the controller thread
                var ressel = GalaxySectorSelect.SelectGalaxyMenu(this);
                EDDConfig.Instance.EDSMEDDBDownload = ressel.Item1 != "None";
                EDDConfig.Instance.EDSMGridIDs = ressel.Item2;
                if (EDDConfig.Instance.EDSMEDDBDownload)
                    Controller.AsyncPerformSync(edsmfullsync: true, eddb_edsmalias_sync:true);      // order another go.
            }

            if (EDDOptions.Instance.NoWindowReposition == false)
                PopOuts.LoadSavedPopouts();  //moved from initial load so we don't open these before we can draw them properly

            actioncontroller.onStartup();

            actioncontroller.CheckWarn();

            screenshotconverter.Start();

            WebServerControl(EDDConfig.Instance.WebServerEnable);

            tabControlMain.SelectedIndexChanged += (snd, ea) =>
            {
                if (tabControlMain.SelectedIndex >= 0)   // may go to -1 on a clear all
                    ActionRun(Actions.ActionEventEDList.onTabChange, null, new BaseUtils.Variables("TabName", tabControlMain.TabPages[tabControlMain.SelectedIndex].Text));
            };

            // DLL loads

            string alloweddlls = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString("DLLAllowed", "");

            DLLCallBacks.RequestHistory = DLLRequestHistory;
            DLLCallBacks.RunAction = DLLRunAction;

            Tuple<string, string, string> res = DLLManager.Load(EDDOptions.Instance.DLLAppDirectory(), EDDApplicationContext.AppVersion, EDDOptions.Instance.DLLAppDirectory(), DLLCallBacks, alloweddlls);

            if (res.Item3.HasChars())
            {
                if (ExtendedControls.MessageBoxTheme.Show(this,
                                string.Format(("The following application extension DLLs have been found" + Environment.NewLine +
                                "Do you wish to allow these to be used?" + Environment.NewLine +
                                "{0} " + Environment.NewLine +
                                "If you do not, either remove the DLLs from the DLL folder in ED Appdata" + Environment.NewLine +
                                "or deinstall the action pack which introduced the DLL" + Environment.NewLine +
                                "or hold down shift when launching and use the Remove all Extensions DLL option").T(EDTx.EDDiscoveryForm_DLLW), res.Item3),
                                "Warning".T(EDTx.Warning),
                                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString("DLLAllowed", alloweddlls.AppendPrePad(res.Item3, ","));
                    DLLManager.UnLoad();
                    res = DLLManager.Load(EDDOptions.Instance.DLLAppDirectory(), EDDApplicationContext.AppVersion, EDDOptions.Instance.DLLAppDirectory(), DLLCallBacks, alloweddlls);
                }
            }

            if (res.Item1.HasChars())
                LogLine(string.Format("DLLs loaded: {0}".T(EDTx.EDDiscoveryForm_DLLL), res.Item1));
            if (res.Item2.HasChars())
                LogLineHighlight(string.Format("DLLs failed to load: {0}".T(EDTx.EDDiscoveryForm_DLLF), res.Item2));

            LogLine(string.Format("Profile {0} Loaded".T(EDTx.EDDiscoveryForm_PROFL), EDDProfiles.Instance.Current.Name));


            // Notifications

            if (!themeok)
            {
                Controller.LogLineHighlight(("The theme stored has missing colors or other missing information" + Environment.NewLine +
                "Correct the missing colors or other information manually using the Theme Editor in Settings").T(EDTx.EDDiscoveryForm_ThemeW));
            }

            Notifications.CheckForNewNotifications((notelist) =>
            {
                this.BeginInvoke(new Action(() =>
                {
                    string acklist = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString("NotificationLastAckTime", "");
                    Version curver = new Version(System.Reflection.Assembly.GetExecutingAssembly().GetVersionString());

                    foreach (Notifications.Notification n in notelist)
                    {
                        Notifications.NotificationParas p = n.Select(EDDConfig.Instance.Language);

                        Version vmax = n.VersionMax != null ? new Version(n.VersionMax) : null;
                        Version vmin = n.VersionMin != null ? new Version(n.VersionMin) : null;

                        if (p != null && DateTime.UtcNow >= n.StartUTC && DateTime.UtcNow <= n.EndUTC && 
                                ( vmax ==null || curver <= vmax) && ( vmin == null || curver >= vmin ) &&
                                (n.actionpackpresent == null || actioncontroller.Get(n.actionpackpresent).Length > 0) &&
                                (n.actionpackpresentenabled == null || actioncontroller.Get(n.actionpackpresentenabled, true).Length > 0) &&
                                (n.actionpackpresentdisabled == null || actioncontroller.Get(n.actionpackpresentdisabled, false).Length > 0) &&
                                (n.actionpacknotpresent == null || actioncontroller.Get(n.actionpacknotpresent).Length == 0)
                                )
                        {
                            if (n.EntryType == "Popup")
                            {
                                if ( !acklist.Contains(n.StartUTC.ToStringZulu()) )
                                    popupnotificationlist.Add(n);
                            }
                            else if (n.EntryType == "Log")
                            {
                                if (n.HighLight)
                                    LogLineHighlight(p.Text);
                                else
                                    LogLine(p.Text);
                            }
                        }

                    }

                    PopUpNotification();

                }));

            });

            Installer.CheckForNewInstallerAsync((rel) =>  // in thread
            {
                newRelease = rel;
                BeginInvoke(new Action(() => Controller.LogLineHighlight(string.Format("New EDDiscovery installer available: {0}".T(EDTx.EDDiscoveryForm_NI), newRelease.ReleaseName))));
                BeginInvoke(new Action(() => labelInfoBoxTop.Text = "New Release Available!".T(EDTx.EDDiscoveryForm_NRA)));
            });

            // this.DebugSizePosition(toolTip); // Debug - theme all the tooltips to show info on control - useful

            if (EDDOptions.Instance.OutputEventHelp != null)        // help for events, going to have to do this frequently, so keep
            {
                string s = "All Events" + Environment.NewLine;
                var infoe = BaseUtils.TypeHelpers.GetPropertyFieldNames(typeof(JournalEntry), "EventClass_", fields: true);
                foreach (var ix in infoe)
                {
                    s += "    " + ix.Name + " " + ix.Help + Environment.NewLine;
                }

                foreach (var x in Enum.GetValues(typeof(JournalTypeEnum)))
                {
                    JournalEntry je = JournalEntry.CreateJournalEntry(x.ToString(), DateTime.UtcNow);

                    if (!(je is JournalUnknown))
                    {
                        s += "Event " + x + Environment.NewLine;
                        var info = BaseUtils.TypeHelpers.GetPropertyFieldNames(je.GetType(), "EventClass_", excludedeclaretype: typeof(JournalEntry), fields: true);
                        foreach (var ix in info)
                        {
                            s += "    " + ix.Name + " : " + ix.Help + Environment.NewLine;
                        }
                    }
                }

                File.WriteAllText(EDDOptions.Instance.OutputEventHelp, s);
            }

            if (!EDDOptions.Instance.DisableTimeDisplay)
            {
                datetimetimer = new Timer();
                datetimetimer.Interval = 1000;
                datetimetimer.Tick += (sv, ev) => { DateTime gameutc = DateTime.UtcNow.AddYears(1286); labelGameDateTime.Text = gameutc.ToShortDateString() + " " + gameutc.ToShortTimeString(); };
                datetimetimer.Start();
            }
        }

        List<Notifications.Notification> popupnotificationlist = new List<Notifications.Notification>();
        void PopUpNotification()        // orgnanise pop ups one at a time..
        {
            if (popupnotificationlist.Count > 0)
            {
                Notifications.NotificationParas p = popupnotificationlist[0].Select(EDDConfig.Instance.Language);

                Action<object> act = new Action<object>((o) =>      // on ack, update list of ack entries
                {
                    DateTime ackdate = (DateTime)o;
                    System.Diagnostics.Debug.WriteLine("Ack to " + ackdate.ToStringZulu());
                    EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString("NotificationLastAckTime", EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString("NotificationLastAckTime","") + ackdate.ToStringZulu());
                });

                ExtendedControls.InfoForm infoform = new ExtendedControls.InfoForm();
                infoform.Info(p.Caption, this.Icon, p.Text, pointsize: popupnotificationlist[0].PointSize,
                        acknowledgeaction: act,
                        acknowledgedata: popupnotificationlist[0].StartUTC);

                infoform.FormClosed += (s, e1) => { PopUpNotification(); };     // chain to next, one at a time..

                popupnotificationlist.RemoveAt(0);
                infoform.Show();
            }
        }

        public bool DLLRunAction( string eventname, string paras )
        {
            System.Diagnostics.Debug.WriteLine("Run " + eventname + "(" + paras + ")");
            actioncontroller.ActionRun(Actions.ActionEventEDList.DLLEvent(eventname),new BaseUtils.Variables(paras,BaseUtils.Variables.FromMode.MultiEntryComma));
            return true;
        }

        public bool DLLRequestHistory(long index, bool isjid, out EliteDangerousCore.DLL.EDDDLLIF.JournalEntry f)
        {
            HistoryEntry he = isjid ? history.GetByJID(index) : history.GetByIndex((int)index);
            f = EliteDangerousCore.DLL.EDDDLLCallerHE.CreateFromHistoryEntry(he);
            return he != null;
        }

        private void EDDiscoveryForm_Resize(object sender, EventArgs e)
        {
            // We may be getting called by this.ResumeLayout() from InitializeComponent().
            if (EDDConfig != null && FormShownOnce)
            {
                if (EDDConfig.UseNotifyIcon && EDDConfig.MinimizeToNotifyIcon)
                {
                    if (FormWindowState.Minimized == WindowState)
                        Hide();
                    else if (!Visible)
                        Show();
                }

                notifyIconMenu_Open.Enabled = FormWindowState.Minimized == WindowState;
            }
        }

        private void EDDiscoveryForm_ResizeBegin(object sender, EventArgs e)
        {
            if (!EDDConfig.Instance.DrawDuringResize)
                this.SuspendLayout();
        }

        private void EDDiscoveryForm_ResizeEnd(object sender, EventArgs e)
        {
            if (!EDDConfig.Instance.DrawDuringResize)
                this.ResumeLayout();

        }



        #endregion

        #region Tabs - most code now in MajorTabControl.cs  (mostly) Only UI code left.

        public void AddTab(PanelInformation.PanelIDs id, int tabindex = 0) // negative means from the end.. -1 is one before end
        {
            tabControlMain.AddTab(id, tabindex);
        }

        public bool SelectTabPage(string name)
        {
            return tabControlMain.SelectTabPage(name);
        }

        private void panelTabControlBack_MouseDown(object sender, MouseEventArgs e)     // click on the empty space of the tabs.. backed up by the panel
        {
            if (e.Button == MouseButtons.Right)
            {
                tabControlMain.ClearLastTab();      // this sets LastTab to -1, which thankfully means insert at last but one position to the AddTab function
                contextMenuStripTabs.Show(tabControlMain.PointToScreen(e.Location));
            }
        }

        private void tabControlMain_MouseClick(object sender, MouseEventArgs e)     // click on one of the tab buttons
        {
            if (tabControlMain.LastTabClicked >= 0)
            {
                if (e.Button == MouseButtons.Right)
                {
                    Point p = tabControlMain.PointToScreen(e.Location);
                    p.Offset(0, -8);
                    contextMenuStripTabs.Show(p);
                }
                else if (e.Button == MouseButtons.Middle && !IsNonRemovableTab(tabControlMain.LastTabClicked))
                {
                    tabControlMain.RemoveTab(tabControlMain.LastTabClicked);
                }
            }
        }

        private void ContextMenuStripTabs_Opening(object sender, CancelEventArgs e)
        {
            int n = tabControlMain.LastTabClicked;
            bool validtab = n >= 0 && n < tabControlMain.TabPages.Count;   // sanity check

            removeTabToolStripMenuItem.Enabled = validtab && !IsNonRemovableTab(n);
            renameTabToolStripMenuItem.Enabled = validtab && !(tabControlMain.TabPages[n].Controls[0] is UserControls.UserControlPanelSelector);
        }

        private bool IsNonRemovableTab(int n)
        {
            bool uch = Object.ReferenceEquals(tabControlMain.TabPages[n].Controls[0], tabControlMain.PrimaryTab);
            bool sel = tabControlMain.TabPages[n].Controls[0] is UserControls.UserControlPanelSelector;
            return uch || sel;
        }

        private void EDDiscoveryForm_MouseDown(object sender, MouseEventArgs e)     // use the form to detect the click on the empty tab area.. it passes thru
        {
            if (e.Button == MouseButtons.Right && e.Y >= tabControlMain.Top)
            {
                tabControlMain.ClearLastTab();      // this sets LastTab to -1, which thankfully means insert at last but one position to the AddTab function
                Point p = this.PointToScreen(e.Location);
                p.Offset(0, -8);
                contextMenuStripTabs.Show(p);
            }
        }

        #endregion

        #region Themeing

        public void ApplyTheme(bool panelrefreshaswell = false)     // set true if your changing the theme
        {
            panel_close.Visible = !theme.WindowsFrame;
            panel_minimize.Visible = !theme.WindowsFrame;
            label_version.Visible = !theme.WindowsFrame;

            // debug label_version.Visible = true; labelInfoBoxTop.Text = "New! X.Y.Z.A";

            this.Text = "EDDiscovery " + label_version.Text;            // note in no border mode, this is not visible on the title bar but it is in the taskbar..

            theme.ApplyStd(this);

            statusStrip.Font = contextMenuStripTabs.Font = this.Font;

            //System.Diagnostics.Debug.WriteLine("Label version " + label_version.Location + " " + label_version.Size + " " + mainMenu.Size);

            OnThemeChanged?.Invoke();

            if ( panelrefreshaswell)
                Controller.RefreshDisplays(); // needed to cause them to cope with theme change
        }

#endregion

#region EDSM and EDDB syncs code

        private void edsmRefreshTimer_Tick(object sender, EventArgs e)
        {
            Controller.AsyncPerformSync();
        }

        public void ForceEDSMEDDBFullRefresh()
        {
            SystemsDatabase.Instance.ForceEDSMFullUpdate();
            SystemsDatabase.Instance.ForceEDDBFullUpdate();
            Controller.AsyncPerformSync(true, true);
        }

#endregion

#region Controller event handlers 

        private void Controller_RefreshCommanders()
        {
            LoadCommandersListBox();             // in case a new commander has been detected
        }

        private void Controller_RefreshStarting()
        {
            RefreshButton(false);
            actioncontroller.ActionRun(Actions.ActionEventEDList.onRefreshStart);
        }

        private void Controller_RefreshComplete()
        {
            Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Refresh complete");

            RefreshButton(true);
            actioncontroller.ActionRunOnRefresh();

            if (EDCommander.Current.SyncToInara)
            {
                EliteDangerousCore.Inara.InaraSync.Refresh(LogLine, history, EDCommander.Current);
            }

            DLLManager.Refresh(EDCommander.Current.Name, EliteDangerousCore.DLL.EDDDLLCallerHE.CreateFromHistoryEntry(history.GetLast));

            Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Refresh complete finished");
        }

        public void NewEntry( JournalEntry e)       // programatically do a new entry
        {
            Controller.NewEntry(e);                 // push it thru as if the monitor watcher saw it
        }


        private void Controller_NewEntrySecond(HistoryEntry he, HistoryList hl)         // called after all UI's have had their chance
        {
            actioncontroller.ActionRunOnEntry(he, Actions.ActionEventEDList.NewEntry(he));

            // all notes committed
            SystemNoteClass.CommitDirtyNotes((snc) => { if (EDCommander.Current.SyncToEdsm && snc.FSDEntry) EDSMClass.SendComments(snc.SystemName, snc.Note, snc.EdsmId, he.Commander); });

            // HERE PERFORM CAPI.. DOCKED

            var lastent = history.GetLast;
            if (!object.ReferenceEquals(he, lastent))
            {
                LogLineHighlight(string.Format("Current history entry is not last in history - possible re-entrancy.\nAlert the EDDiscovery developers using either discord or Github (see help) and attach log file {0}", BaseUtils.TraceLog.LogFileName));
                Trace.WriteLine($"Current history entry is not last in history");
                Trace.WriteLine($"Current entry: {he.journalEntry?.GetJson()?.ToString()}");
                Trace.WriteLine($"Last entry: {lastent.journalEntry?.GetJson()?.ToString()}");
                Trace.WriteLine($"Stack Trace:");
                Trace.WriteLine(new StackTrace(true).ToString());
            }

            if (he.IsFSDCarrierJump)
            {
                int count = history.GetVisitsCount(he.System.Name);
                LogLine(string.Format("Arrived at system {0} Visit No. {1}".T(EDTx.EDDiscoveryForm_Arrived), he.System.Name, count));
                System.Diagnostics.Trace.WriteLine("Arrived at system: " + he.System.Name + " " + count + ":th visit.");
            }

            if (EDCommander.Current.SyncToEdsm)
            {
                EDSMJournalSync.SendEDSMEvents(LogLine, he);
            }

            if (EDCommander.Current.SyncToInara)
            {
                EliteDangerousCore.Inara.InaraSync.NewEvent(LogLine, he);
            }

            if (EDCommander.Current.SyncToIGAU )
            {
                EliteDangerousCore.IGAU.IGAUSync.NewEvent(LogLine, he);
            }

            if (EDDNClass.IsEDDNMessage(he.EntryType,he.EventTimeUTC) && he.AgeOfEntry() < TimeSpan.FromDays(1.0) && EDCommander.Current.SyncToEddn == true)
            {
                EDDNSync.SendEDDNEvents(LogLine, he);
            }

            DLLManager.NewJournalEntry(EliteDangerousCore.DLL.EDDDLLCallerHE.CreateFromHistoryEntry(he));

            CheckActionProfile(he);
        }

        private void Controller_NewUIEvent(UIEvent uievent)
        {
            BaseUtils.Variables cv = new BaseUtils.Variables();

            string prefix = "EventClass_";
            cv.AddPropertiesFieldsOfClass(uievent, prefix, new Type[] { typeof(System.Drawing.Icon), typeof(System.Drawing.Image), typeof(System.Drawing.Bitmap), typeof(Newtonsoft.Json.Linq.JObject) }, 5);
            cv[prefix + "UIDisplayed"] = "0";
            actioncontroller.ActionRun(Actions.ActionEventEDList.onUIEvent, cv);
            actioncontroller.ActionRun(Actions.ActionEventEDList.EliteUIEvent(uievent), cv);

            if (!uievent.EventRefresh)      // don't send the refresh events thru the system..  see if profiles need changing
            {
                Actions.ActionVars.TriggerVars(cv, "UI" + uievent.EventTypeStr, "UIEvent");

                int i = EDDProfiles.Instance.ActionOn(cv, out string errlist);
                if (i >= 0)
                    ChangeToProfileId(i, true);

                if (errlist.HasChars())
                    LogLine("Profile reports errors in triggers:".T(EDTx.EDDiscoveryForm_PE1) + errlist);
            }
        }

        string syncprogressstring="",refreshprogressstring="";

        private void ReportSyncProgress(int percentComplete, string message)
        {
            if (!Controller.PendingClose)
            {
                if (percentComplete >= 0)
                {
                    toolStripProgressBar1.Visible = true;
                    toolStripProgressBar1.Value = percentComplete;
                }
                else
                {
                    toolStripProgressBar1.Visible = false;
                }

                syncprogressstring = message;
                toolStripStatusLabel1.Text = ObjectExtensionsStrings.AppendPrePad(syncprogressstring, refreshprogressstring, " | ");
            }
        }

        private void ReportRefreshProgress(int percentComplete, string message)      // percent not implemented for this
        {
            if (!Controller.PendingClose)
            {
                refreshprogressstring = message;
                toolStripStatusLabel1.Text = ObjectExtensionsStrings.AppendPrePad(syncprogressstring, refreshprogressstring, " | ");
                Update();       // nasty but it works - needed since we are doing UI work here and the UI thread will be blocked
            }
        }

        #endregion

        #region Closing

        private void EDDiscoveryForm_FormClosing(object sender, FormClosingEventArgs e)     // when user asks for a close
        {
            edsmRefreshTimer.Enabled = false;
            if (!Controller.PendingClose)       // only allow 1 close attempt..
            {
                e.Cancel = true;

                bool goforit = !in_system_sync || ExtendedControls.MessageBoxTheme.Show("EDDiscovery is updating the EDSM and EDDB databases\r\nPress OK to close now, Cancel to wait until update is complete".T(EDTx.EDDiscoveryForm_CloseWarning), "Warning".T(EDTx.Warning), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK;

                if (goforit)
                {
                    ReportRefreshProgress(-1, "Closing, please wait!".T(EDTx.EDDiscoveryForm_Closing));
                    actioncontroller.ActionRun(Actions.ActionEventEDList.onShutdown);
                    Controller.Shutdown();
                }
            }
        }

        private void Controller_FinalClose()        // run in UI, when controller finishes close
        {
            actioncontroller.HoldTillProgStops();

            if (WebServer.Running)
                WebServer.Stop();

            // send any dirty notes.  if they are, the call back gets called. If we have EDSM sync on, and its an FSD entry, send it
            SystemNoteClass.CommitDirtyNotes((snc) => { if (EDCommander.Current.SyncToEdsm && snc.FSDEntry) EDSMClass.SendComments(snc.SystemName, snc.Note, snc.EdsmId); });

            screenshotconverter.SaveSettings();
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("ToolBarPanelPinState", panelToolBar.PinState);

            theme.SaveSettings(null);

            tabControlMain.CloseTabList();      // close and save tab list

            PopOuts.SaveCurrentPopouts();

            notifyIcon1.Visible = false;

            actioncontroller.CloseDown();

            DLLManager.UnLoad();

            Close();
            Application.Exit();
        }
     
#endregion

#region Buttons, Mouse, Menus, NotifyIcon

        private void buttonReloadActions_Click(object sender, EventArgs e)
        {
            BaseUtils.Translator.Instance.LoadTranslation(EDDConfig.Instance.Language, CultureInfo.CurrentUICulture, 
                    EDDOptions.Instance.TranslatorFolders(),
                    EDDOptions.Instance.TranslatorDirectoryIncludeSearchUpDepth, EDDOptions.Instance.AppDataDirectory);
            actioncontroller.ReLoad();
            actioncontroller.CheckWarn();
            actioncontroller.onStartup();
         }


        private void frontierForumThreadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Properties.Resources.URLProjectEDForumPost);
        }

        private void eDDiscoveryHomepageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Properties.Resources.URLProjectWiki);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControlMain.EnsureMajorTabIsPresent(PanelInformation.PanelIDs.Settings, true);
        }


        private void showLogfilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EDCommander cmdr = EDCommander.Current;

            if (cmdr != null)
            {
                string cmdrfolder = cmdr.JournalDir;
                if (cmdrfolder == null || cmdrfolder.Length < 1)
                    cmdrfolder = EDJournalClass.GetDefaultJournalDir();

                if (Directory.Exists(cmdrfolder))
                {
                    Process.Start(cmdrfolder);
                }
            }
        }

        private void show2DMapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open2DMap();
        }

        private void show3DMapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open3DMap(PrimaryCursor.GetCurrentHistoryEntry);
        }

        private void forceEDDBUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EDDConfig.Instance.EDSMEDDBDownload)
                ExtendedControls.MessageBoxTheme.Show(this, "Star Data download is disabled. Use Settings to reenable it".T(EDTx.EDDiscoveryForm_SDDis));
            else if (!Controller.AsyncPerformSync(eddb_edsmalias_sync: true))      // we want it to have run, to completion, to allow another go..
                ExtendedControls.MessageBoxTheme.Show(this, "Synchronisation to databases is in operation or pending, please wait".T(EDTx.EDDiscoveryForm_SDSyncErr));
        }

        private void syncEDSMSystemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EDDConfig.Instance.EDSMEDDBDownload)
                ExtendedControls.MessageBoxTheme.Show(this, "Star Data download is disabled. Use Settings to reenable it".T(EDTx.EDDiscoveryForm_SDDis));
            else if (ExtendedControls.MessageBoxTheme.Show(this, ("This can take a considerable amount of time and bandwidth" + Environment.NewLine + "Confirm you want to do this?").T(EDTx.EDDiscoveryForm_EDSMQ), "Warning".T(EDTx.Warning), MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk)  == DialogResult.OK )
            {
                if (!Controller.AsyncPerformSync(edsmfullsync: true))      // we want it to have run, to completion, to allow another go..
                    ExtendedControls.MessageBoxTheme.Show(this, "Synchronisation to databases is in operation or pending, please wait".T(EDTx.EDDiscoveryForm_SDSyncErr));
            }
        }

        private void fetchLogsAgainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controller.EdsmLogFetcher.ResetFetch();
        }

        private void gitHubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Properties.Resources.URLProjectGithub);
        }

        private void reportIssueIdeasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Properties.Resources.URLProjectFeedback);
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void AboutBox(Form parent = null)
        {
            AboutForm frm = new AboutForm();
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
            ExtendedControls.MessageBoxTheme.Show(this, 
                            ("To start in safe mode, exit the program, hold down the shift key" + Environment.NewLine +
                            "and double click on the EDD program icon.  You will then be in the safe mode dialog." + Environment.NewLine +
                            "You can reset various parameters and move the data bases to other locations.").T(EDTx.EDDiscoveryForm_SafeMode),
                            "Information".T(EDTx.Information), MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            if (ExtendedControls.MessageBoxTheme.Show(this, 
                                ("Confirm you wish to reset the assigned EDSM ID\r\n" +
                                "to all the current commander history entries,\r\n" +
                                "and clear all the assigned EDSM IDs in all your notes for all commanders\r\n\r\n" +
                                "This will not change your history, but when you next refresh,\r\n" +
                                "it will try and reassign EDSM systems to your history and notes.\r\n" +
                                "Use only if you think that the assignment of EDSM systems to entries is grossly wrong," +
                                "or notes are going missing\r\n" +
                                "\r\n" +
                                "You can manually change one EDSM assigned system by right clicking\r\n" +
                                "on the travel history and selecting the option").T(EDTx.EDDiscoveryForm_ResetEDSMID)
                                , "Warning".T(EDTx.Warning), MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                JournalEntry.ClearEDSMID(EDCommander.CurrentCmdrID);
                SystemNoteClass.ClearEDSMID();
                Controller.RefreshHistoryAsync();
            }

        }


        private void paneleddiscovery_Click(object sender, EventArgs e)
        {
            AboutBox(this);
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
                    DialogResult dlgResult = dirdlg.ShowDialog(this);

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
            if (ExtendedControls.MessageBoxTheme.Show(this, "Confirm you wish to reset all history entries to the current commander".T(EDTx.EDDiscoveryForm_ResetCMDR), "Warning".T(EDTx.Warning), MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                JournalEntry.ResetCommanderID(-1, EDCommander.CurrentCmdrID);
                Controller.RefreshHistoryAsync();
            }
        }


        private void rescanAllJournalFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controller.RefreshHistoryAsync(forcejournalreload: true);
        }

        private void checkForNewReleaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newRelease = Installer.CheckForNewinstaller();
            if ( newRelease != null )
            {
                using (NewReleaseForm frm = new NewReleaseForm(newRelease))
                    frm.ShowDialog(this);
            }
            else
            {
                ExtendedControls.MessageBoxTheme.Show(this,"No new release found".T(EDTx.EDDiscoveryForm_NoRel), "Warning".T(EDTx.Warning), MessageBoxButtons.OK);
            }
        }

        private void deleteDuplicateFSDJumpEntriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ExtendedControls.MessageBoxTheme.Show(this, "Confirm you remove any duplicate FSD entries from the current commander".T(EDTx.EDDiscoveryForm_RevFSD), "Warning".T(EDTx.Warning), MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Controller.RefreshHistoryAsync(removedupfsdentries: true);
            }
        }

        private void exportVistedStarsListToEliteDangerousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string exportfilename = null;
            bool found = false;
            string folder = EliteDangerousCore.VisitingStarsCacheFolder.GetVisitedStarsCacheDirectory();

            if (folder != null)
            {
                exportfilename = Path.Combine(folder, "ImportStars.txt");
                found = true;
            }
            else
            {
                SaveFileDialog dlg = new SaveFileDialog();

                dlg.Filter = "ImportedStars export| *.txt";
                dlg.Title = "Could not find VisitedStarsCache.dat file, choose file".T(EDTx.EDDiscoveryForm_VSLNF);
                dlg.FileName = "ImportStars.txt";

                if (dlg.ShowDialog(this) != DialogResult.OK)
                    return;
                exportfilename = dlg.FileName;
            }

            List<JournalEntry> scans = JournalEntry.GetByEventType(JournalTypeEnum.FSDJump, EDCommander.CurrentCmdrID, new DateTime(2014, 1, 1), DateTime.UtcNow);

            var tscans = scans.ConvertAll<JournalFSDJump>(x => (JournalFSDJump)x);

            try
            {
                using (StreamWriter writer = new StreamWriter(exportfilename, false))
                {
                    foreach (var system in tscans.Select(o => o.StarSystem).Distinct())
                    {
                        writer.WriteLine(system);
                    }
                }

                ExtendedControls.MessageBoxTheme.Show(this, string.Format(("File {0} created." + Environment.NewLine + "{1}").T(EDTx.EDDiscoveryForm_VSLEXP),
                    exportfilename, (found ? "Restart Elite Dangerous to have this file read into the galaxy map".T(EDTx.EDDiscoveryForm_VSLRestart) : "" )), "Warning".T(EDTx.Warning));
            }
            catch (IOException)
            {
                ExtendedControls.MessageBoxTheme.Show(this, string.Format("Error writing {0} export visited stars".T(EDTx.EDDiscoveryForm_VSLFileErr), exportfilename), "Warning".T(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Open3DMap(HistoryEntry he)
        {
            this.Cursor = Cursors.WaitCursor;

            history.FillInPositionsFSDJumps();

            Map.Prepare(he?.System, EDCommander.Current.HomeSystemTextOrSol,
                        EDCommander.Current.MapCentreOnSelection ? he?.System : EDCommander.Current.HomeSystemIOrSol,
                        EDCommander.Current.MapZoom, Controller.history.FilterByTravel);
            Map.Show();
            this.Cursor = Cursors.Default;
        }

        public void Open3DMapOnSystem(ISystem centerSystem )
        {
            this.Cursor = Cursors.WaitCursor;

            if (centerSystem == null || !centerSystem.HasCoordinate)
                centerSystem = history.GetLastWithPosition.System;

            Map.Prepare(centerSystem, EDCommander.Current.HomeSystemTextOrSol, centerSystem,
                             EDCommander.Current.MapZoom, history.FilterByTravel);

            Map.Show();
            this.Cursor = Cursors.Default;
        }

        public void Open2DMap()
        {
            this.Cursor = Cursors.WaitCursor;
            Form2DMap frm = new Form2DMap(Controller.history.FilterByFSDCarrierJumpAndPosition);
            frm.Show();
            this.Cursor = Cursors.Default;
        }

        private void sendUnsuncedEDDNEventsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<HistoryEntry> hlsyncunsyncedlist = Controller.history.FilterByScanNotEDDNSynced;        // first entry is oldest

            EDDNSync.SendEDDNEvents(LogLine, hlsyncunsyncedlist);
        }

        private void sendHistoricDataToInaraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime lasttime = UserDatabase.Instance.GetSettingDate("InaraLastHistoricUpload", DateTime.MinValue);

            if (DateTime.UtcNow.Subtract(lasttime).TotalHours >= 1)  // every hours, allowed to do this..
            {
                EliteDangerousCore.Inara.InaraSync.HistoricData(LogLine, history, EDCommander.Current);
                UserDatabase.Instance.PutSettingDate("InaraLastHistoricUpload", DateTime.UtcNow);
            }
            else
                ExtendedControls.MessageBoxTheme.Show(this, "Inara historic upload is disabled until 1 hour has elapsed from the last try to prevent server flooding".T(EDTx.EDDiscoveryForm_InaraW), "Warning".T(EDTx.Warning));
        }

        private void rebuildSystemDBIndexesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ExtendedControls.MessageBoxTheme.Show(this, "Are you sure to Rebuild Indexes? It may take a long time.".T(EDTx.EDDiscoveryForm_IndexW), "Warning".T(EDTx.Warning),MessageBoxButtons.OKCancel,MessageBoxIcon.Warning) == DialogResult.OK )
            {
                SystemsDatabase.Instance.RebuildIndexes(LogLine);
            }
        }


        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            // Tray icon was double-clicked.
            if (FormWindowState.Minimized == WindowState)
            {
                if (EDDConfig.MinimizeToNotifyIcon)
                    Show();

                if (FormIsMaximised)
                    WindowState = FormWindowState.Maximized;
                else
                    WindowState = FormWindowState.Normal;
            }
            else
                WindowState = FormWindowState.Minimized;
        }

        private void notifyIconMenu_Hide_Click(object sender, EventArgs e)
        {       // horrible circular ref to this sub func then back up.. can't think of a fix for now.
            TabPage t = tabControlMain.GetMajorTab(PanelInformation.PanelIDs.Settings);
            if ( t != null )
                (t.Controls[0] as UserControls.UserControlSettings).DisableNotifyIcon();
        }

        private void notifyIconMenu_Open_Click(object sender, EventArgs e)
        {
            // Tray icon 'Open EDDiscovery' menu item was clicked. Present the main window.
            if (FormWindowState.Minimized == WindowState)
            {
                if (EDDConfig.UseNotifyIcon && EDDConfig.MinimizeToNotifyIcon)
                    Show();

                if (FormIsMaximised)
                    WindowState = FormWindowState.Maximized;
                else
                    WindowState = FormWindowState.Normal;
            }
            else
                Activate();
        }

#endregion

#region "Caption" controls

        private void MouseDownCAPTION(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void MouseUpCAPTION(object sender, MouseEventArgs e)
        {
            OnCaptionMouseUp((Control)sender, e);
        }


        private void labelversion_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && newRelease != null)
            {
                using (NewReleaseForm frm = new NewReleaseForm(newRelease))
                    frm.ShowDialog(this);
            }
            else
            {
                MouseDownCAPTION(sender, e);
            }
        }

        private void panel_close_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                Close();
        }

        private void panel_minimize_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.WindowState = FormWindowState.Minimized;
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

        public void NewCalculatedRoute(List<ISystem> list)
        {
            if (OnNewCalculatedRoute != null)
                OnNewCalculatedRoute(list);
        }

        #endregion

        #region Add Ons
        public void manageAddOnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonExtManageAddOns_Click(sender,e);
        }

        private void buttonExtManageAddOns_Click(object sender, EventArgs e)
        {
            if (actioncontroller.ManageAddOns())
                OnAddOnsChanged?.Invoke();
        }

        private void configureAddOnActionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonExtEditAddOns_Click(sender, e);
        }

        private void buttonExtEditAddOns_Click(object sender, EventArgs e)
        {
            if ( actioncontroller.EditAddOns() )
                OnAddOnsChanged?.Invoke();
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

            menu = menu.ToLowerInvariant();
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

            Image img = BaseUtils.Icons.IconSet.GetIcon(icon);

            var x = (from ToolStripItem p in parent.DropDownItems where p.Text.Equals(menutext) && p.Tag != null && p.Name.Equals(menuname) select p);

            if (x.Count() == 0)           // double entries screened out of same menu text from same pack
            {
                ToolStripMenuItem it = new ToolStripMenuItem();
                it.Text = menutext;
                it.Name = menuname;
                it.Tag = packname;
                if (img != null)
                    it.Image = img;
                it.Size = new Size(313, 22);
                it.Click += MenuTrigger_Click;
                parent.DropDownItems.Add(it);
            }

            return true;
        }

        public bool IsMenuItemInstalled(string menuname)
        {
            foreach( ToolStripMenuItem tsi in mainMenu.Items )
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
            BaseUtils.Variables vars = new BaseUtils.Variables(new string[]
            {   "MenuName", it.Name,
                "MenuText", it.Text,
                "TopLevelMenuName" , it.OwnerItem.Name,
            });

            actioncontroller.ActionRun(Actions.ActionEventEDList.onMenuItem, null, vars);
        }

        public BaseUtils.Variables Globals { get { return actioncontroller.Globals; } }

        public int ActionRunOnEntry(HistoryEntry he, ActionLanguage.ActionEvent av)
        { return actioncontroller.ActionRunOnEntry(he, av); }

        public int ActionRun(ActionLanguage.ActionEvent ev, HistoryEntry he = null, BaseUtils.Variables additionalvars = null, string flagstart = null, bool now = false)
        { return actioncontroller.ActionRun(ev,he,additionalvars,flagstart,now); }

#endregion

#region Toolbar

        public void LoadCommandersListBox()
        {
            comboBoxCommander.Enabled = false;
            comboBoxCommander.Items.Clear();            // comboBox is nicer with items
            comboBoxCommander.Items.AddRange((from EDCommander c in EDCommander.GetListInclHidden() select c.Name).ToList());
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
                var itm = (from EDCommander c in EDCommander.GetListInclHidden() where c.Name.Equals(comboBoxCommander.Text) select c).ToList();
                ChangeToCommander(itm[0].Nr);
            }
        }


        private void buttonExt3dmap_Click(object sender, EventArgs e)
        {
            Open3DMap(PrimaryCursor.GetCurrentHistoryEntry);
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
            LogLine("Refresh History.".T(EDTx.EDDiscoveryForm_RH));
            RefreshHistoryAsync();
        }

        private void SendUnsyncedJournalsToEDSM()
        {
            EDSMClass edsm = new EDSMClass();

            if (!edsm.ValidCredentials)
            {
                ExtendedControls.MessageBoxTheme.Show(this, "Please ensure a commander is selected and it has a EDSM API key set".T(EDTx.EDDiscoveryForm_NoEDSMAPI));
                return;
            }

            if (!EDCommander.Current.SyncToEdsm)
            {
                string dlgtext = "You have disabled sync to EDSM for this commander.  Are you sure you want to send unsynced events to EDSM?".T(EDTx.EDDiscoveryForm_ConfirmSyncToEDSM);
                string dlgcapt = "Confirm EDSM sync".T(EDTx.EDDiscoveryForm_ConfirmSyncToEDSMCaption);

                if (ExtendedControls.MessageBoxTheme.Show(this, dlgtext, dlgcapt, MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }

            try
            {
                EDSMJournalSync.SendEDSMEvents(l => LogLine(l), history, manual: true);
            }
            catch (Exception ex)
            {
                LogLine(string.Format("EDSM Sync failed: {0}".T(EDTx.EDDiscoveryForm_EDSMSyncE), ex.Message));
            }
        }

        private void buttonExtEDSMSync_Click(object sender, EventArgs e)
        {
            SendUnsyncedJournalsToEDSM();
        }

        private void sendUnsyncedEDSMJournalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendUnsyncedJournalsToEDSM();
        }

        #endregion

        #region Profiles

        private void UpdateProfileComboBox()
        {
            comboBoxCustomProfiles.Enabled = false;                         // and update this box, making sure we don't renter
            comboBoxCustomProfiles.Items.Clear();
            comboBoxCustomProfiles.Items.AddRange(EDDProfiles.Instance.Names());
            comboBoxCustomProfiles.Items.Add("Edit Profiles".T(EDTx.EDDiscoveryForm_EP));
            comboBoxCustomProfiles.SelectedIndex = EDDProfiles.Instance.IndexOf(EDDProfiles.Instance.Current.Id);
            comboBoxCustomProfiles.Enabled = true;
        }

        private void ComboBoxCustomProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCustomProfiles.SelectedIndex >= 0 && comboBoxCustomProfiles.Enabled)
            {
                if (comboBoxCustomProfiles.SelectedIndex == comboBoxCustomProfiles.Items.Count-1)    // last one if edit profiles
                {
                    Forms.ProfileEditor pe = new ProfileEditor();
                    pe.Init(EDDProfiles.Instance, this.Icon);
                    if (pe.ShowDialog() == DialogResult.OK)
                    {
                        bool removedcurprofile = EDDProfiles.Instance.UpdateProfiles(pe.Result, pe.PowerOnIndex);       // see if the current one has changed...

                        if (removedcurprofile)
                            ChangeToProfileId(EDDProfiles.DefaultId, false );
                    }

                    UpdateProfileComboBox();
                }
                else
                {
                    ChangeToProfileId(EDDProfiles.Instance.IdOfIndex(comboBoxCustomProfiles.SelectedIndex), true);
                }
            }
        }

        private void ChangeToProfileId(int id, bool checksavecur)
        {
            if (!checksavecur || EDDProfiles.Instance.Current.Id != id)
            {
                System.Diagnostics.Debug.WriteLine(BaseUtils.AppTicks.TickCountLap("ProfT") + " *************************** CHANGE To profile " + id);
                if (checksavecur)
                {
                    tabControlMain.CloseTabList();
                    PopOuts.SaveCurrentPopouts();
                }

                PopOuts.CloseAllPopouts();


                comboBoxCustomProfiles.Enabled = false;                         // and update the selection box, making sure we don't trigger a change
                comboBoxCustomProfiles.SelectedIndex = EDDProfiles.Instance.IndexOf(id);
                comboBoxCustomProfiles.Enabled = true;

                EDDProfiles.Instance.ChangeToId(id);

                UserControls.UserControlContainerSplitter.CheckPrimarySplitterControlSettings("??????????"); // use a nonsense name to make sure we just get the default set if we don't have a valid save

                tabControlMain.TabPages.Clear();
                tabControlMain.CreateTabs(this, EDDOptions.Instance.TabsReset, "0, -1,0, 26,0, 27,0, 29,0, 34,0");      // numbers from popouts, which are FIXED!
                tabControlMain.LoadTabs();
                ApplyTheme();

                PopOuts.LoadSavedPopouts();

                System.Diagnostics.Debug.WriteLine(BaseUtils.AppTicks.TickCountLap("ProfT") + " *************************** Finished Profile " + id);
                LogLine(string.Format("Profile {0} Loaded".T(EDTx.EDDiscoveryForm_PL), EDDProfiles.Instance.Current.Name ));
            }
        }

        public void CheckActionProfile(HistoryEntry he)
        {
            BaseUtils.Variables eventvars = new BaseUtils.Variables();
            Actions.ActionVars.TriggerVars(eventvars, he.journalEntry.EventTypeStr, "JournalEvent");
            Actions.ActionVars.HistoryEventVars(eventvars, he, "Event");     // if HE is null, ignored
            Actions.ActionVars.ShipBasicInformation(eventvars, he?.ShipInformation, "Event");     // if He null, or si null, ignore
            Actions.ActionVars.SystemVars(eventvars, he?.System, "Event");

            int i = EDDProfiles.Instance.ActionOn(eventvars, out string errlist);
            if (i >= 0)
                ChangeToProfileId(i, true);

            if (errlist.HasChars())
                LogLine("Profile reports errors in triggers:".T(EDTx.EDDiscoveryForm_PE1) + errlist); 
        }

        #endregion

        #region PopOuts

        ExtendedControls.ExtListBoxForm popoutdropdown;

        private void buttonExtPopOut_Click(object sender, EventArgs e)
        {
            popoutdropdown = new ExtendedControls.ExtListBoxForm("", true);
            popoutdropdown.StartPosition = FormStartPosition.Manual;
            popoutdropdown.Items = PanelInformation.GetUserSelectablePanelDescriptions(EDDConfig.Instance.SortPanelsByName).ToList();
            popoutdropdown.ImageItems = PanelInformation.GetUserSelectablePanelImages(EDDConfig.Instance.SortPanelsByName).ToList();
            popoutdropdown.ItemSeperators = PanelInformation.GetUserSelectableSeperatorIndex(EDDConfig.Instance.SortPanelsByName);
            PanelInformation.PanelIDs[] pids = PanelInformation.GetUserSelectablePanelIDs(EDDConfig.Instance.SortPanelsByName);
            popoutdropdown.FlatStyle = FlatStyle.Popup;
            popoutdropdown.PositionBelow(buttonExtPopOut);
            popoutdropdown.SelectedIndexChanged += (s, ea) =>
            {
                PopOuts.PopOut(pids[popoutdropdown.SelectedIndex]);
            };

            theme.ApplyStd(popoutdropdown,true);
            popoutdropdown.SelectionBackColor = theme.ButtonBackColor;
            popoutdropdown.Show(this);
        }



        #endregion

        #region webserver

        public bool WebServerControl(bool start)        // false if an error occurs
        {
            if (WebServer.Running)
                WebServer.Stop();

            if (start)
            {
                WebServer.Port = EDDConfig.Instance.WebServerPort;

                string servefrom;
                bool valid;

                if (EDDOptions.Instance.WebServerFolder != null)
                {
                    servefrom = EDDOptions.Instance.WebServerFolder;
                    valid = Directory.Exists(servefrom);
                }
                else
                {
                    servefrom = Path.Combine(EDDOptions.ExeDirectory(), "eddwebsite.zip");
                    valid = File.Exists(servefrom);
                }

                if (valid)
                {
                    System.Diagnostics.Debug.WriteLine("Serve from " + servefrom + " on port " + EDDConfig.Instance.WebServerPort);

                    if (WebServer.Start(servefrom))   // may fail due to some security reasons
                        Controller.LogLine("Web server enabled".T(EDTx.EDDiscoveryForm_WSE));
                    else
                    {
                        Controller.LogLineHighlight("Web server failed to start".T(EDTx.EDDiscoveryForm_WSF));
                        return false;
                    }
                }
                else
                    Controller.LogLineHighlight("Web server disabled due to incorrect folder or missing zip file".T(EDTx.EDDiscoveryForm_WSERR));
            }
            else
            {
                Controller.LogLine("*** Web server is disabled ***".T(EDTx.EDDiscoveryForm_WSD));
            }

            return true;
        }

        #endregion


    }
}



