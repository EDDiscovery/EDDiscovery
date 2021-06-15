﻿/*
 * Copyright © 2015 - 2021 EDDiscovery development team
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

using EDDiscovery.Forms;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.EDDN;
using EliteDangerousCore.EDSM;
using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery
{
    public partial class EDDiscoveryForm : Forms.DraggableFormPos
    { 
        #region Variables

        private EDDiscoveryController Controller;
        private Actions.ActionController actioncontroller;

        public EliteDangerousCore.DLL.EDDDLLManager DLLManager;
        public EDDDLLInterfaces.EDDDLLIF.EDDCallBacks DLLCallBacks;

        public CAPI.CompanionAPI FrontierCAPI { get { return Controller.FrontierCAPI; } }

        public WebServer.EDDWebServer WebServer;

        public Actions.ActionController ActionController { get { return actioncontroller; } }
        public BaseUtils.Variables Globals { get { return actioncontroller.Globals; } }

        static public EDDConfig EDDConfig { get { return EDDConfig.Instance; } }
        public EDDTheme theme { get { return EDDTheme.Instance; } }

        public UserControls.IHistoryCursor PrimaryCursor { get { return tabControlMain.PrimaryTab.GetTravelGrid; } }
        public UserControls.UserControlContainerSplitter PrimarySplitter { get { return tabControlMain.PrimaryTab; } }

        public EliteDangerousCore.ScreenShots.ScreenShotConverter screenshotconverter;

        public EDDiscovery._3DMap.MapManager Map { get; private set; }

        private bool in_system_sync = false;        // between start/end sync of databases

        BaseUtils.GitHubRelease newRelease;

        public PopOutControl PopOuts;

        Timer periodicchecktimer;

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
        public event Action<string, Size> ScreenShotCaptured;           // screen shot has been captured

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

            MaterialCommodityMicroResourceType.FillTable();     // lets statically fill the table way before anyone wants to access it

            Controller.Init();
            PanelInformation.Init();

            // Some components require the controller to be initialized
            // obsolete remove IconSet.SetPanelImageListGetter(PanelInformation.GetPanelImages);
            InitializeComponent();

            screenshotconverter = new EliteDangerousCore.ScreenShots.ScreenShotConverter();
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
                theme.RestoreSettings();                                // theme, remember your saved settings

            if (EDDOptions.Instance.FontSize > 0)
                theme.FontSize = EDDOptions.Instance.FontSize;

            if (EDDOptions.Instance.Font.HasChars())
                theme.FontName = EDDOptions.Instance.Font;

            ApplyTheme();                       // we apply and scale (because its being applied to Form) before any tabs parts are setup.

            this.TopMost = EDDConfig.KeepOnTop;
            notifyIconEDD.Visible = EDDConfig.UseNotifyIcon;

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

            UserControls.UserControlContainerSplitter.CheckPrimarySplitterControlSettings(EDDOptions.Instance.TabsReset); // Double check, use TravelControlBottom etc as the old lookup name if its nonsence

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

            helpTabToolStripMenuItem.Click += (s, e) => { tabControlMain.HelpOn(this,contextMenuStripTabs.PointToScreen(new Point(0,0)), tabControlMain.LastTabClicked); };

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
                    ActionRun(Actions.ActionEventEDList.onEDSMSync, new BaseUtils.Variables(new string[] { "EventStarList", list, "EventCount", count.ToStringInvariant() }));
                });
            };

            EDDNSync.SentEvents = (count) =>              // Sync thread finishing, transfers to this thread, then runs the callback and the action..
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    System.Diagnostics.Debug.Assert(Application.MessageLoop);
                    OnEDDNSyncComplete?.Invoke(count);
                    ActionRun(Actions.ActionEventEDList.onEDDNSync, new BaseUtils.Variables(new string[] { "EventCount", count.ToStringInvariant() }));
                });
            };

            EliteDangerousCore.IGAU.IGAUSync.SentEvents = (count) =>              // Sync thread finishing, transfers to this thread, then runs the callback and the action..
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    System.Diagnostics.Debug.Assert(Application.MessageLoop);
                    OnIGAUSyncComplete?.Invoke(count);
                    ActionRun(Actions.ActionEventEDList.onIGAUSync, new BaseUtils.Variables(new string[] { "EventCount", count.ToStringInvariant() }));
                });
            };

            Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Finish ED Init");

            EliteDangerousCore.DLL.EDDDLLAssemblyFinder.AssemblyFindPath = EDDOptions.Instance.DLLAppDirectory();      // any needed assemblies from here
            AppDomain.CurrentDomain.AssemblyResolve += EliteDangerousCore.DLL.EDDDLLAssemblyFinder.AssemblyResolve;

            DLLManager = new EliteDangerousCore.DLL.EDDDLLManager();
            DLLCallBacks = new EDDDLLInterfaces.EDDDLLIF.EDDCallBacks();

            WebServer = new WebServer.EDDWebServer(this);

            UpdateProfileComboBox();
            comboBoxCustomProfiles.SelectedIndexChanged += ComboBoxCustomProfiles_SelectedIndexChanged;

            BaseUtils.Translator.Instance.Translate(mainMenu, this);
            BaseUtils.Translator.Instance.Translate(toolTip,this);

            panelToolBar.SetToolTip(toolTip);    // use the defaults

            if (EDDOptions.Instance.ActionButton)
                buttonReloadActions.Visible = true;

#if !DEBUG
            sendUnsuncedEDDNEventsToolStripMenuItem.Visible = false;        // for testing only now
#endif

            extButtonDrawnHelp.Text = "";
            extButtonDrawnHelp.Image = ExtendedControls.TabStrip.HelpIcon;
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
                EDDConfig.Instance.EDSMDownload = false;        // this stops the download working in the controller thread
                var ressel = GalaxySectorSelect.SelectGalaxyMenu(this);
                EDDConfig.Instance.EDSMDownload = ressel.Item1 != "None";
                EDDConfig.Instance.EDSMGridIDs = ressel.Item2;
                if (EDDConfig.Instance.EDSMDownload)
                    Controller.AsyncPerformSync(edsmfullsync: true, edsm_alias_sync: true);      // order another go.
            }

            if (EDDOptions.Instance.NoWindowReposition == false)
                PopOuts.LoadSavedPopouts();  //moved from initial load so we don't open these before we can draw them properly

            actioncontroller.onStartup();

            actioncontroller.CheckWarn();

            screenshotconverter.Start((a) => Invoke(a),
                             (b) => LogLine(b),
                             () =>
                             {
                                 if (history.GetLast != null)        // lasthe should have name and whereami, and an indication of commander
                                 {
                                     return new Tuple<string, string, string>(history.GetLast.System.Name, history.GetLast.WhereAmI, history.GetLast.Commander?.Name ?? "Unknown");
                                 }
                                 else
                                 {
                                     return new Tuple<string, string, string>("Unknown", "Unknown", "Unknown");
                                 }
                             },
                             8000       // ms to wait after file detected before assuming journal will not be updated
                             );

            screenshotconverter.OnScreenshot += (infile, outfile, imagesize, ss) => // screenshot seen
            {
                if (ss != null)
                {
                    ss.SetConvertedFilename(infile ?? "Deleted", outfile, imagesize.Width, imagesize.Height);       // record in SS the in/out files
                }

                ScreenShotCaptured?.Invoke(outfile, imagesize);         // tell others screen shot is captured
            };

            WebServerControl(EDDConfig.Instance.WebServerEnable);

            tabControlMain.SelectedIndexChanged += (snd, ea) =>
            {
                if (tabControlMain.SelectedIndex >= 0)   // may go to -1 on a clear all
                    ActionRun(Actions.ActionEventEDList.onTabChange, new BaseUtils.Variables("TabName", tabControlMain.TabPages[tabControlMain.SelectedIndex].Text));
            };

            // DLL loads

            DLLCallBacks.RequestHistory = DLLRequestHistory;
            DLLCallBacks.RunAction = DLLRunAction;
            DLLCallBacks.GetShipLoadout = (s) => { return null; };

            string verstring = EDDApplicationContext.AppVersion;
            string[] options = new string[] { EDDDLLInterfaces.EDDDLLIF.FLAG_HOSTNAME + "EDDiscovery",
                                              EDDDLLInterfaces.EDDDLLIF.FLAG_JOURNALVERSION + "2",
                                              EDDDLLInterfaces.EDDDLLIF.FLAG_CALLBACKVERSION + "2",
                                            };

            string alloweddlls = EDDConfig.Instance.DLLPermissions;

            Tuple<string, string, string> res = DLLManager.Load(EDDOptions.Instance.DLLAppDirectory(), verstring, options, DLLCallBacks, alloweddlls);

            if (res.Item3.HasChars())       // new DLLs
            {
                string[] list = res.Item3.Split(',');
                bool changed = false;
                foreach (var dll in list)
                {
                    if (ExtendedControls.MessageBoxTheme.Show(this,
                                    string.Format(("The following application extension DLL have been found" + Environment.NewLine +
                                    "Do you wish to allow it to be used?" + Environment.NewLine + Environment.NewLine +
                                    "{0} " + Environment.NewLine
                                    ).T(EDTx.EDDiscoveryForm_DLLW), dll),
                                    "Warning".T(EDTx.Warning),
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        alloweddlls = alloweddlls.AppendPrePad("+" + dll, ",");
                        changed = true;
                    }
                    else
                    {
                        alloweddlls = alloweddlls.AppendPrePad("-" + dll, ",");
                    }
                }

                EDDConfig.Instance.DLLPermissions = alloweddlls;

                if (changed)
                {
                    DLLManager.UnLoad();
                    res = DLLManager.Load(EDDOptions.Instance.DLLAppDirectory(), verstring, options, DLLCallBacks, alloweddlls);
                }
            }

            if (res.Item1.HasChars())
                LogLine(string.Format("DLLs loaded: {0}".T(EDTx.EDDiscoveryForm_DLLL), res.Item1));
            if (res.Item2.HasChars())
                LogLineHighlight(string.Format("DLLs failed to load: {0}".T(EDTx.EDDiscoveryForm_DLLF), res.Item2));

            LogLine(string.Format("Profile {0} Loaded".T(EDTx.EDDiscoveryForm_PROFL), EDDProfiles.Instance.Current.Name));

            // Bindings

            if (actioncontroller.FrontierBindings.FileLoaded != null)
                LogLine(string.Format("Loaded Bindings {0}", actioncontroller.FrontierBindings.FileLoaded));
            else
                LogLine("Frontier bindings did not load");


            // Notifications

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
                                (vmax == null || curver <= vmax) && (vmin == null || curver >= vmin) &&
                                (n.actionpackpresent == null || actioncontroller.Get(n.actionpackpresent).Length > 0) &&
                                (n.actionpackpresentenabled == null || actioncontroller.Get(n.actionpackpresentenabled, true).Length > 0) &&
                                (n.actionpackpresentdisabled == null || actioncontroller.Get(n.actionpackpresentdisabled, false).Length > 0) &&
                                (n.actionpacknotpresent == null || actioncontroller.Get(n.actionpacknotpresent).Length == 0)
                                )
                        {
                            if (n.EntryType == "Popup")
                            {
                                if (!acklist.Contains(n.StartUTC.ToStringZulu()))
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

                    ShowNotification();

                }));

            });

            // Now the installer

            Installer.CheckForNewInstallerAsync((rel) =>  // in thread
            {
                newRelease = rel;
                BeginInvoke(new Action(() => Controller.LogLineHighlight(string.Format("New EDDiscovery installer available: {0}".T(EDTx.EDDiscoveryForm_NI), newRelease.ReleaseName))));
                BeginInvoke(new Action(() => labelInfoBoxTop.Text = "New Release Available!".T(EDTx.EDDiscoveryForm_NRA)));
            });

            // DEBUG STUFF

            //this.DebugSizePosition(toolTip); // Debug - theme all the tooltips to show info on control - useful

            if (EDDOptions.Instance.OutputEventHelp != null)        // help for events, going to have to do this frequently, so keep
            {
                string fn = EDDOptions.Instance.OutputEventHelp;
                string colon = " : ";
                string prefix = "    ";
                int ll = 80;

                if (EDDOptions.Instance.OutputEventHelp.StartsWith("G:"))
                {
                    fn = fn.Substring(2);
                    colon = " | ";
                    ll = int.MaxValue;
                    prefix = "";
                }

                string s = "All Journal Events" + Environment.NewLine;

                var excllist = new Type[] { typeof(System.Drawing.Icon), typeof(System.Drawing.Image), typeof(System.Drawing.Bitmap), typeof(BaseUtils.JSON.JObject) };

                var infoe = BaseUtils.TypeHelpers.GetPropertyFieldNames(typeof(JournalEntry), "EventClass_", fields: true, linelen: ll, propexcluded:excllist);
                foreach (var ix in infoe)
                {
                    s += prefix + ix.Name + colon + ix.Help + Environment.NewLine;
                }

                s += Environment.NewLine;

                foreach (var x in Enum.GetValues(typeof(JournalTypeEnum)))
                {
                    //var x = "CreateSuitLoadout";
                    JournalEntry je = JournalEntry.CreateJournalEntry(x.ToString(), DateTime.UtcNow);

                    if (!(je is JournalUnknown))
                    {
                        s += Environment.NewLine + "Event: " + x + Environment.NewLine;
                        var info = BaseUtils.TypeHelpers.GetPropertyFieldNames(je.GetType(), "EventClass_", excludedeclaretype: typeof(JournalEntry), fields: true, linelen:ll, propexcluded: excllist);
                        foreach (var ix in info)
                        {
                            s += prefix + ix.Name + colon + ix.Help + Environment.NewLine;
                        }
                    }
                }

                s += Environment.NewLine;
                s += Environment.NewLine + "All UI Events" + Environment.NewLine;
                var infoui = BaseUtils.TypeHelpers.GetPropertyFieldNames(typeof(UIEvent), "EventClass_", fields: true, linelen:ll, propexcluded: excllist);
                foreach (var ix in infoui)
                {
                    s += prefix + ix.Name + colon + ix.Help + Environment.NewLine;
                }

                s += Environment.NewLine;

                foreach (var x in Enum.GetValues(typeof(UITypeEnum)))
                {
                    UIEvent ui = UIEvent.CreateEvent(x.ToString(), DateTime.UtcNow, false);
                    s += Environment.NewLine + "UIEvent: UI" + x + Environment.NewLine;
                    var info = BaseUtils.TypeHelpers.GetPropertyFieldNames(ui.GetType(), "EventClass_", excludedeclaretype: typeof(UIEvent), fields: true, linelen:ll, propexcluded: excllist);
                    foreach (var ix in info)
                    {
                        s += prefix+ ix.Name + colon + ix.Help + Environment.NewLine;
                    }
                }

                File.WriteAllText(fn, s);
            }

            // Time Display

            periodicchecktimer = new Timer();                   // timer for periodic actions
            periodicchecktimer.Interval = 1000;
            periodicchecktimer.Tick += (sv, ev) =>
            {
                if (!EDDOptions.Instance.DisableTimeDisplay)
                {
                    DateTime gameutc = DateTime.UtcNow.AddYears(1286);
                    labelGameDateTime.Text = gameutc.ToShortDateString() + " " + gameutc.ToShortTimeString();
                }

                if (buttonReloadActions.Visible)
                {
                    if (actioncontroller.CheckForActionFilesChange()) // autoreload edited action files..
                        buttonReloadActions_Click(null, null);
                }
            };

            periodicchecktimer.Start();

            // Options for automatic stuff

            if (EDDOptions.Instance.AutoOpen3DMap)
                Open3DMap(PrimaryCursor.GetCurrentHistoryEntry);
            if (EDDOptions.Instance.MinimiseOnOpen)
                WindowState = FormWindowState.Minimized;
            else if (EDDOptions.Instance.MaximiseOnOpen)
                WindowState = FormWindowState.Maximized;

            // About box automatic open to prompt them to acknowledge program

            {
                var lastaboutversion = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString("AboutBoxLastVersionPresented", "0.0.0.0").VersionFromString();
                var eddversion = EDDApplicationContext.AppVersion.VersionFromString();
                lastaboutversion[3] = eddversion[3] = 0;        // ignore the last dot
                lastaboutversion[2] = eddversion[2] = 0;        // ignore the second one
                if (lastaboutversion.CompareVersion(eddversion) < 0)
                {
                    EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString("AboutBoxLastVersionPresented", EDDApplicationContext.AppVersion);
                    AboutBox();
                }
            }
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


        #region Closing

        private void EDDiscoveryForm_FormClosing(object sender, FormClosingEventArgs e)     // when user asks for a close
        {
            edsmRefreshTimer.Enabled = false;
            if (!Controller.PendingClose)       // only allow 1 close attempt..
            {
                e.Cancel = true;

                bool goforit = !in_system_sync || ExtendedControls.MessageBoxTheme.Show("EDDiscovery is updating the EDSM databases\r\nPress OK to close now, Cancel to wait until update is complete".T(EDTx.EDDiscoveryForm_CloseWarning), "Warning".T(EDTx.Warning), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK;

                if ( goforit )
                {
                    if (tabControlMain.AllowClose() == false)
                        goforit = false;
                    else if (PopOuts.AllowClose() == false)
                        goforit = false;
                }

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
            SystemNoteClass.CommitDirtyNotes((snc) => { if (EDCommander.Current.SyncToEdsm && snc.FSDEntry) EDSMClass.SendComments(snc.SystemName, snc.Note); });

            screenshotconverter.SaveSettings();
            screenshotconverter.Stop();

            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("ToolBarPanelPinState", panelToolBar.PinState);

            theme.SaveSettings(null);

            tabControlMain.CloseTabList();      // close and save tab list

            PopOuts.SaveCurrentPopouts();

            notifyIconEDD.Visible = false;

            actioncontroller.CloseDown();

            DLLManager.UnLoad();

            Close();
            Application.Exit();
        }
     
        #endregion
    
        #region Tools Menu

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControlMain.EnsureMajorTabIsPresent(PanelInformation.PanelIDs.Settings, true);
        }

        private void show3DMapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open3DMap(PrimaryCursor.GetCurrentHistoryEntry);
        }

        private void showAllInTaskBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PopOuts.ShowAllPopOutsInTaskBar();
        }

        private void turnOffAllTransparencyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PopOuts.MakeAllPopoutsOpaque();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region Admin Menu

        private void showLogfilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EDCommander cmdr = EDCommander.Current;

            if (cmdr != null)
            {
                string cmdrfolder = cmdr.JournalDir.HasChars() ? cmdr.JournalDir : (EliteDangerousCore.FrontierFolder.FolderName() ?? ".");
                if (Directory.Exists(cmdrfolder))
                {
                    Process.Start(cmdrfolder);
                }
            }
        }

        private void syncEDSMSystemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EDDConfig.Instance.EDSMDownload)
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
                    dirdlg.SelectedPath = UserDatabase.Instance.GetSettingString("Folder21Import", @"c:\");
                    DialogResult dlgResult = dirdlg.ShowDialog(this);

                    if (dlgResult == DialogResult.OK)
                    {
                        UserDatabase.Instance.PutSettingString("Folder21Import", dirdlg.SelectedPath);
                        string logpath = dirdlg.SelectedPath;

                        Controller.RefreshHistoryAsync(netlogpath: logpath, forcenetlogreload: force, currentcmdr: cmdr.Id);
                    }
                }
            }
        }

        private void debugResetAllHistoryToFirstCommanderToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void sendUnsyncedEDDNEventsToolStripMenuItem_Click(object sender, EventArgs e)      //DEBUG ONLY
        {
            List<HistoryEntry> hlsyncunsyncedlist = HistoryList.FilterByScanNotEDDNSynced(Controller.history.EntryOrder());        // first entry is oldest

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

        private void rebuildUserDBIndexesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ExtendedControls.MessageBoxTheme.Show(this, "Are you sure to Rebuild Indexes? It may take a long time.".T(EDTx.EDDiscoveryForm_IndexW), "Warning".T(EDTx.Warning), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                UserDatabase.Instance.RebuildIndexes(LogLine);
            }
        }

        private void rebuildSystemDBIndexesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ExtendedControls.MessageBoxTheme.Show(this, "Are you sure to Rebuild Indexes? It may take a long time.".T(EDTx.EDDiscoveryForm_IndexW), "Warning".T(EDTx.Warning), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                SystemsDatabase.Instance.RebuildIndexes(LogLine);
            }
        }

        private void updateUnknownSystemCoordsWithDataFromSystemDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ExtendedControls.MessageBoxTheme.Show(this, 
                    "Scan your history, and for systems without co-ordinates,\r\ntry and fill them in from your system database\r\nConfirm?".T(EDTx.EDDiscoveryForm_FillPos), 
                    "Warning".T(EDTx.Warning), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                history.FillInPositionsFSDJumps(LogLine);
                RefreshDisplays();
            }
        }

        private void DLLPermissions_Click(object sender, EventArgs e)
        {
            string n = EliteDangerousCore.DLL.EDDDLLManager.DLLPermissionManager(this, this.Icon, EDDConfig.Instance.DLLPermissions);
            if (n != null)
                EDDConfig.Instance.DLLPermissions = n;
        }

        #endregion

        #region Add Ons Menu

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

        #endregion

        #region Help Menu

        private void frontierForumThreadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BaseUtils.BrowserInfo.LaunchBrowser(Properties.Resources.URLProjectEDForumPost);
        }

        private void wikiHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BaseUtils.BrowserInfo.LaunchBrowser(Properties.Resources.URLProjectWiki);
        }

        private void viewHelpVideosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BaseUtils.BrowserInfo.LaunchBrowser(Properties.Resources.URLProjectVideos);
        }

        private void gitHubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BaseUtils.BrowserInfo.LaunchBrowser(Properties.Resources.URLProjectGithub);
        }

        private void reportIssueIdeasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BaseUtils.BrowserInfo.LaunchBrowser(Properties.Resources.URLProjectFeedback);
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

        private void eddiscoveryChatDiscordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BaseUtils.BrowserInfo.LaunchBrowser(Properties.Resources.URLProjectDiscord);
        }

        private void howToRunInSafeModeToResetVariousParametersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExtendedControls.MessageBoxTheme.Show(this,
                            ("To start in safe mode, exit the program, hold down the shift key" + Environment.NewLine +
                            "and double click on the EDD program icon.  You will then be in the safe mode dialog." + Environment.NewLine +
                            "You can reset various parameters and move the data bases to other locations.").T(EDTx.EDDiscoveryForm_SafeMode),
                            "Information".T(EDTx.Information), MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void paneleddiscovery_Click(object sender, EventArgs e)
        {
            AboutBox(this);
        }

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
            }
            else
            {
                comboBoxCommander.SelectedItem = EDCommander.Current.Name;
            }

            comboBoxCommander.Enabled = true;
        }

        private void comboBoxCommander_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCommander.SelectedIndex >= 0 && comboBoxCommander.Enabled)     // DONT trigger during LoadCommandersListBox
            {
                var itm = (from EDCommander c in EDCommander.GetListInclHidden() where c.Name.Equals(comboBoxCommander.Text) select c).ToList();
                ChangeToCommander(itm[0].Id);
            }
        }

        private void buttonExt3dmap_Click(object sender, EventArgs e)
        {
            Open3DMap(PrimaryCursor.GetCurrentHistoryEntry);
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

        private void sendUnsyncedEDSMJournalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EDSMClass edsm = new EDSMClass();

            if (!edsm.ValidCredentials)
            {
                ExtendedControls.MessageBoxTheme.Show(this, "No EDSM API key set".T(EDTx.EDDiscoveryForm_NoEDSMAPI));
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

            EDSMSend();
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

        private void extButtonDrawnHelp_Click(object sender, EventArgs e)
        {
            tabControlMain.HelpOn(this,extButtonDrawnHelp.PointToScreen(new Point(0, extButtonDrawnHelp.Bottom)), tabControlMain.SelectedIndex);
        }

        private void buttonReloadActions_Click(object sender, EventArgs e)
        {
            BaseUtils.Translator.Instance.LoadTranslation(EDDConfig.Instance.Language, CultureInfo.CurrentUICulture, 
                    EDDOptions.Instance.TranslatorFolders(),
                    EDDOptions.Instance.TranslatorDirectoryIncludeSearchUpDepth, EDDOptions.Instance.AppDataDirectory);
            actioncontroller.ReLoad();
            actioncontroller.CheckWarn();
            actioncontroller.onStartup();
         }


        #endregion
        
        #region Other clicks - Captions etc

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

        #region Notify Icons

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
            if (t != null)
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

        internal void useNotifyIconChanged(bool useNotifyIcon)
        {
            notifyIconEDD.Visible = useNotifyIcon;
            if (!useNotifyIcon && !Visible)
                Show();
        }

        #endregion


    }
}



