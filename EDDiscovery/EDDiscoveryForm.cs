/*
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

using BaseUtils;
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
        #region Major systems

        public EliteDangerousCore.DLL.EDDDLLManager DLLManager { get; set; }

        public CAPI.CompanionAPI FrontierCAPI { get { return Controller.FrontierCAPI; } }

        public WebServer.EDDWebServer WebServer;

        public Actions.ActionController ActionController { get { return actioncontroller; } }
        public BaseUtils.Variables Globals { get { return actioncontroller.Globals; } }

        public ExtendedControls.ThemeList ThemeList { get; private set; }

        public UserControls.IHistoryCursor PrimaryCursor { get { return tabControlMain.PrimaryTab.GetTravelGrid; } }
        public UserControls.UserControlContainerSplitter PrimarySplitter { get { return tabControlMain.PrimaryTab; } }

        public EliteDangerousCore.ScreenShots.ScreenShotConverter ScreenshotConverter { get; set; }
        public PopOutControl PopOuts { get; set; }

        public GalacticMapping galacticMapping { get; private set; }
        public GalacticMapping eliteRegions { get; private set; }

        public HistoryList history { get { return Controller.history; } }

        public string LogText { get { return Controller.LogText; } }
        #endregion

        #region Callbacks from us
        public event Action<Object> OnNewTarget;
        public event Action<Object, HistoryEntry> OnNoteChanged;        // UI.Note has been updated attached to this note
        public event Action<List<ISystem>> OnNewCalculatedRoute;        // route plotter has a new one
        public event Action OnAddOnsChanged;                            // add on changed
        public event Action OnPanelAdded;                               // add on changed
        public event Action<int,string> OnEDSMSyncComplete;             // EDSM Sync has completed with this list of stars are newly created
        public event Action<int> OnEDDNSyncComplete;                    // Sync has completed
        public event Action<int> OnIGAUSyncComplete;                    // Sync has completed
                                                                        // theme is changing/ then has been changed by settings, hook if you have some UI which needs refreshing due to it. 
        public event Action OnThemeChanging;                            // Note you won't get it on startup because theme is applied to form before tabs/panels are setup. Before themeing
        public event Action OnThemeChanged;                             // Note you won't get it on startup because theme is applied to form before tabs/panels are setup
        public event Action<string, Size> ScreenShotCaptured;           // screen shot has been captured
        #endregion

        #region Events due to EDDiscoveryControl 
        public event Action OnRefreshCommanders { add { Controller.OnRefreshCommanders += value; } remove { Controller.OnRefreshCommanders -= value; } }       
        public event Action<HistoryList> OnHistoryChange { add { Controller.OnHistoryChange += value; } remove { Controller.OnHistoryChange -= value; } }
        public event Action<HistoryEntry, HistoryList> OnNewEntry { add { Controller.OnNewEntry += value; } remove { Controller.OnNewEntry -= value; } }
        public event Action<UIEvent> OnNewUIEvent { add { Controller.OnNewUIEvent += value; } remove { Controller.OnNewUIEvent -= value; } }
        public event Action<string, Color> OnNewLogEntry { add { Controller.OnNewLogEntry += value; } remove { Controller.OnNewLogEntry -= value; } }
        public event Action<bool> OnExpeditionsDownloaded { add { Controller.OnExpeditionsDownloaded += value; } remove { Controller.OnExpeditionsDownloaded -= value; } }
        public event Action<long, long> OnSyncComplete { add { Controller.OnSyncComplete += value; } remove { Controller.OnSyncComplete -= value; } }

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
            Controller.RefreshHistoryAsync(currentcmdr: EDCommander.CurrentCmdrID);                                   // which will cause Display to be called as some point
        }
        #endregion

        #region Privates

        private EDDDLLInterfaces.EDDDLLIF.EDDCallBacks DLLCallBacks;
        private EDDiscoveryController Controller;
        private Actions.ActionController actioncontroller;
        private BaseUtils.GitHubRelease newRelease;
        private Timer periodicchecktimer;
        private bool in_system_sync = false;        // between start/end sync of databases
        private Tuple<string, string, string, string> dllresults;   // hold results between load and shown
        private string dllsalloweddisallowed; // holds DLL allowed between load and shown

        #endregion

        #region Initialisation

        // note we do not do the traditional Initialize component here.. we wait for splash form to call it
        // and we need to tell the drag form pos our save name
        public EDDiscoveryForm()
        {
            RestoreFormPositionRegKey = "Form";
            Controller = new EDDiscoveryController(a => BeginInvoke(a));

            Controller.OnFinalClose += Controller_FinalClose;

            Controller.OnRefreshCommanders += Controller_RefreshCommanders;
            Controller.OnRefreshComplete += Controller_RefreshComplete;
            Controller.OnRefreshStarting += Controller_RefreshStarting;
            Controller.OnReportRefreshProgress += ReportRefreshProgress;

            Controller.OnSyncStarting += () => { edsmRefreshTimer.Enabled = false; in_system_sync = true; };
            Controller.OnSyncComplete += (c1,c2) => { edsmRefreshTimer.Enabled = true; in_system_sync = false; };
            Controller.OnReportSyncProgress += ReportSyncProgress;

            Controller.OnNewHistoryEntryUnfiltered += Controller_NewHistoryEntryUnfiltered; // called before being added to the HE, unfiltered, unmerged stream
            Controller.OnNewEntrySecond += Controller_NewEntrySecond;       // called after UI updates themselves with NewEntry
            Controller.OnNewUIEvent += Controller_NewUIEvent;       // called if its an UI event
        }

        // called from EDDApplicationContext .. continues on with the construction of the system
        public void Init(Action<string> msg)  
        {
            if (EDDOptions.Instance.Culture != null)
                CultureInfo.CurrentCulture = System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(EDDOptions.Instance.Culture);

            // previously in controller

            string logpath = EDDOptions.Instance.LogAppDirectory();

            BaseUtils.LogClean.DeleteOldLogFiles(logpath, "*.hlog", 2, 256);        // Remove hlogs faster
            BaseUtils.LogClean.DeleteOldLogFiles(logpath, "*.log", 10, 256);

            if (!Debugger.IsAttached || EDDOptions.Instance.TraceLog != null)       // no debugger, or tracelog option set
            {
                TraceLog.RedirectTrace(logpath, true, EDDOptions.Instance.TraceLog);
            }

            if (!Debugger.IsAttached || EDDOptions.Instance.LogExceptions)          // no debugger, or log exceptions set
            {
                ExceptionCatcher.RedirectExceptions(Properties.Resources.URLProjectFeedback);
            }

            if (EDDOptions.Instance.LogExceptions)
            {
                FirstChanceExceptionCatcher.RegisterFirstChanceExceptionHandler();
            }

            Process.GetCurrentProcess().PriorityClass = EDDOptions.Instance.ProcessPriorityClass;

            if (EDDOptions.Instance.ForceTLS12)
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11;
            }

            HttpCom.LogPath = logpath;

            Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDF Init config finished");

            Trace.WriteLine($"*** Elite Dangerous Discovery Initializing - {EDDOptions.Instance.VersionDisplayString}, Platform: {Environment.OSVersion.Platform.ToString()}");

            GlobalBookMarkList.LoadBookmarks();
            GlobalCaptainsLogList.LoadLog();

            msg.Invoke("Loading Icons");
            EDDiscovery.Icons.ForceInclusion.Include();      // Force the assembly into the project by a empty call
            BaseUtils.Icons.IconSet.CreateSingleton();
            System.Reflection.Assembly iconasm = BaseUtils.ResourceHelpers.GetAssemblyByName("EDDiscovery.Icons");
            BaseUtils.Icons.IconSet.Instance.LoadIconsFromAssembly(iconasm);
            BaseUtils.Icons.IconSet.Instance.AddAlias("settings", "Controls.Settings");             // from use by action system..
            BaseUtils.Icons.IconSet.Instance.AddAlias("missioncompleted", "Journal.MissionCompleted");
            BaseUtils.Icons.IconSet.Instance.AddAlias("speaker", "Legacy.speaker");
            BaseUtils.Icons.IconSet.Instance.AddAlias("Default", "Legacy.star");        // MUST be present

            msg.Invoke("Loading Configuration");
            EDDConfig.Instance.Update();
            EDDProfiles.Instance.LoadProfiles(EDDOptions.Instance.Profile);

            string path = EDDOptions.Instance.IconsPath ?? System.IO.Path.Combine(EDDOptions.Instance.IconsAppDirectory(), "*.zip");
            BaseUtils.Icons.IconSet.Instance.LoadIconPack(path, EDDOptions.Instance.AppDataDirectory, EDDOptions.ExeDirectory());

            Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDF init");        // STAGE 1

            msg.Invoke("Loading Translations");

            if (EDDOptions.Instance.ResetLanguage)
                EDDConfig.Instance.Language = "None";

            string lang = EDDOptions.Instance.SelectLanguage ?? EDDConfig.Instance.Language;

            bool loadorgenglish = false;

#if DEBUG
            if (lang == "example-ex")       // if we are loading english, turn on code vs english comparision to see if we can find any out of date english.ex
            {
                Translator.Instance.CompareTranslatedToCode = true;
                loadorgenglish = true;
            }
#endif
            bool found = BaseUtils.Translator.Instance.LoadTranslation(lang, 
                    CultureInfo.CurrentUICulture, 
                    EDDOptions.Instance.TranslatorFolders(),
                    EDDOptions.Instance.TranslatorDirectoryIncludeSearchUpDepth, EDDOptions.Instance.AppDataDirectory, 
                    loadorgenglish:loadorgenglish,
                    debugout:false);


            if (!found && !lang.Contains("Default", StringComparison.InvariantCultureIgnoreCase) && !lang.Contains("Auto", StringComparison.InvariantCultureIgnoreCase))
                ExtendedControls.MessageBoxTheme.Show("Translation file disappeared - check your debugger -translationfolder settings!","Translation file");

            BaseUtils.Translator.Instance.AddExcludedControls(new Type[]
            {   typeof(ExtendedControls.ExtComboBox), typeof(ExtendedControls.NumberBoxDouble),typeof(ExtendedControls.NumberBoxFloat),typeof(ExtendedControls.NumberBoxLong),
                typeof(ExtendedControls.ExtScrollBar),typeof(ExtendedControls.ExtStatusStrip),typeof(ExtendedControls.ExtRichTextBox),typeof(ExtendedControls.ExtTextBox),
                typeof(ExtendedControls.ExtTextBoxAutoComplete),typeof(ExtendedControls.ExtDateTimePicker),typeof(ExtendedControls.ExtNumericUpDown) });

            MaterialCommodityMicroResourceType.FillTable();     // lets statically fill the table way before anyone wants to access it

            Controller.Init();
            PanelInformation.Init();

            // Some components require the controller to be initialized
            InitializeComponent();

            ScreenshotConverter = new EliteDangerousCore.ScreenShots.ScreenShotConverter();
            PopOuts = new PopOutControl(this);

            Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDF Load popouts, themes, init controls");        // STAGE 2 themeing the main interface (not the tab pages)
            msg.Invoke("Applying Themes");

            comboBoxCommander.AutoSize = comboBoxCustomProfiles.AutoSize = true;
            panelToolBar.HiddenMarkerWidth = 200;
            panelToolBar.SecondHiddenMarkerWidth = 60;
            panelToolBar.PinState = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("ToolBarPanelPinState", true);

            labelGameDateTime.Text = "";
            labelInfoBoxTop.Text = "";
            label_version.Text = EDDOptions.Instance.VersionDisplayString;

            ThemeList = new ExtendedControls.ThemeList();
            ThemeList.LoadBaseThemes();                                         // default themes and ones on disk loaded
            ThemeList.Load(EDDOptions.Instance.ThemeAppDirectory(), "*.eddtheme"); // load any file stored themes

            if (!EDDOptions.Instance.NoTheme)
            {
                ThemeList.SetThemeByName("Elite Verdana Small");                // this is the default theme we use normally
                var theme = GetThemeFromDB();
                if (theme != null)
                    ExtendedControls.Theme.Current = theme;
            }
            else
                ThemeList.SetThemeByName("Windows Default");                    // this is the default theme we use for notheme

            if (EDDOptions.Instance.FontSize > 0)
                ExtendedControls.Theme.Current.FontSize = EDDOptions.Instance.FontSize;

            if (EDDOptions.Instance.Font.HasChars())
                ExtendedControls.Theme.Current.FontName = EDDOptions.Instance.Font;

            ApplyTheme();                       // we apply and scale (because its being applied to Form) before any tabs parts are setup.

            this.TopMost = EDDConfig.Instance.KeepOnTop;
            notifyIconEDD.Visible = EDDConfig.Instance.UseNotifyIcon;

            // create the action controller and install commands before we executre tabs, since some tabs need these set up

            actioncontroller = new Actions.ActionController(this, Controller, this.Icon, new Type[] { });

            msg.Invoke("Loading Action Packs");         // STAGE 4 Action packs

            // ---------------------------------------------------------------- Finish up any installing/deleting which failed during the upgrade process because the files were in use

            {
                List<string> alloweddllslist = EDDConfig.Instance.DLLPermissions.Split(",").ToList();

                FileInfo[] allFiles = Directory.EnumerateFiles(EDDOptions.Instance.TempMoveDirectory(), "*.txt", SearchOption.TopDirectoryOnly).Select(f => new FileInfo(f)).OrderBy(p => p.Name).ToArray();

                foreach (FileInfo f in allFiles)
                {
                    string cmd = BaseUtils.FileHelpers.TryReadAllTextFromFile(f.FullName);
                    if ( cmd != null )
                    {
                        if (cmd.StartsWith("Delete:"))
                        {
                            string d = cmd.Substring(7);
                            int i = alloweddllslist.ContainsIn(d, StringComparison.InvariantCultureIgnoreCase);
                            if (i >= 0)
                                alloweddllslist.RemoveAt(i);
                            EDDConfig.Instance.DLLPermissions = String.Join(",", alloweddllslist);
                            BaseUtils.FileHelpers.DeleteFileNoError(d);
                        }
                        else if (cmd.StartsWith("Copy:"))
                        {
                            string s = cmd.Substring(5, cmd.IndexOf(":To:",5) - 5);
                            string d = cmd.Substring(cmd.IndexOf(":To:") + 4);
                            if (!BaseUtils.FileHelpers.TryCopy(s, d, true))
                                System.Diagnostics.Debug.WriteLine("Can't copy over on startup" + d);
                            BaseUtils.FileHelpers.DeleteFileNoError(s);
                        }
                        BaseUtils.FileHelpers.DeleteFileNoError(f.FullName);
                    }
                }
            }

            // ---------------------------------------------------------------- Event hook

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

            //----------------------------------------------------------------- Do translations before any thing else gets added to these toolbars

            PanelInformation.PanelIDs[] pids = PanelInformation.GetUserSelectablePanelIDs(EDDConfig.Instance.SortPanelsByName);      // only user panels

            var enumlistcms = new Enum[] { EDTx.EDDiscoveryForm_addTabToolStripMenuItem, EDTx.EDDiscoveryForm_removeTabToolStripMenuItem, EDTx.EDDiscoveryForm_renameTabToolStripMenuItem, EDTx.EDDiscoveryForm_popOutPanelToolStripMenuItem, EDTx.EDDiscoveryForm_helpTabToolStripMenuItem };
            BaseUtils.Translator.Instance.TranslateToolstrip(contextMenuStripTabs, enumlistcms, this);        // need to translate BEFORE we add in extra items

            var enumlistcms2 = new Enum[] { EDTx.EDDiscoveryForm_notifyIconMenu_Open, EDTx.EDDiscoveryForm_notifyIconMenu_Hide, EDTx.EDDiscoveryForm_notifyIconMenu_Exit };
            BaseUtils.Translator.Instance.TranslateToolstrip(notifyIconContextMenuStrip, enumlistcms2, this);        // need to translate BEFORE we add in extra items

            // ---------------------------------------------------------------- DLL Load

            msg.Invoke("Loading Extension DLLs");
            Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDF DLL setup");

            EliteDangerousCore.DLL.EDDDLLAssemblyFinder.AssemblyFindPaths.Add(EDDOptions.Instance.DLLAppDirectory());      // any needed assemblies from here
            var dllexe = EDDOptions.Instance.DLLExeDirectory();     // and possibly from here, may not be present
            if ( dllexe != null )
                EliteDangerousCore.DLL.EDDDLLAssemblyFinder.AssemblyFindPaths.Add(dllexe);       
            AppDomain.CurrentDomain.AssemblyResolve += EliteDangerousCore.DLL.EDDDLLAssemblyFinder.AssemblyResolve;

            DLLManager = new EliteDangerousCore.DLL.EDDDLLManager();
            DLLCallBacks = new EDDDLLInterfaces.EDDDLLIF.EDDCallBacks();
            DLLCallBacks.ver = 2;
            DLLCallBacks.RequestHistory = DLLRequestHistory;
            DLLCallBacks.RunAction = DLLRunAction;
            DLLCallBacks.GetShipLoadout = (s) => { return null; };
            dllsalloweddisallowed = EDDConfig.Instance.DLLPermissions;
            dllresults = DLLStart(ref dllsalloweddisallowed);       // we run it, and keep the results for processing in Shown

           // temp example - will be removed later.
           //AddPanel(20100, typeof(UserControls.UserControlExtPanel), typeof(UserControls.NewPanel1), "New Panel 1", "newpanel1", "New panel 1 auto installed", BaseUtils.Icons.IconSet.Instance.Get("fred"));
           //AddPanel(20200, typeof(UserControls.UserControlExtPanel), typeof(UserControls.NewPanel2), "New Panel 2", "newpanel2", "New panel 2 auto installed", BaseUtils.Icons.IconSet.Instance.Get("fred"));

            // ---------------------------------------------------------------- Web server

            WebServer = new WebServer.EDDWebServer(this);

            //----------------------------------------------------------------- GMO etc load

            string gmofile = Path.Combine(EDDOptions.Instance.AppDataDirectory, "galacticmapping.json");

            if (!EDDOptions.Instance.NoSystemsLoad && !File.Exists(gmofile))        // if allowed to load, and no gmo file, fetch immediately
            {
                LogLine("Get galactic mapping from EDSM.".T(EDTx.EDDiscoveryController_EDSM));
                if (EDSMClass.DownloadGMOFileFromEDSM(gmofile))
                    SystemsDatabase.Instance.SetEDSMGalMapLast(DateTime.UtcNow);
            }

            {
                galacticMapping = new GalacticMapping();
                if (File.Exists(gmofile))
                    galacticMapping.ParseEDSMFile(gmofile);                            // at this point, gal map data has been uploaded - get it into memory
                galacticMapping.LoadMarxObjects();
            }

            {
                eliteRegions = new GalacticMapping();
                var text = System.Text.Encoding.UTF8.GetString(Properties.Resources.EliteGalacticRegions);
                eliteRegions.ParseEDSMJson(text);                            // at this point, gal map data has been uploaded - get it into memory
            }

            SystemCache.AddToAutoCompleteList(galacticMapping.GetGMONames());

            Bodies.Prepopulate();           

            UpdateProfileComboBox();
            comboBoxCustomProfiles.SelectedIndexChanged += ComboBoxCustomProfiles_SelectedIndexChanged;

            //---------------------------------------------------------------------- Tool tips

            var enumlistcms3 = new Enum[] { EDTx.EDDiscoveryForm_toolsToolStripMenuItem, EDTx.EDDiscoveryForm_toolsToolStripMenuItem_settingsToolStripMenuItem, 
                EDTx.EDDiscoveryForm_toolsToolStripMenuItem_showAllPopoutsInTaskBarToolStripMenuItem, EDTx.EDDiscoveryForm_toolsToolStripMenuItem_showAllPopoutsInTaskBarToolStripMenuItem_showAllInTaskBarToolStripMenuItem, 
                EDTx.EDDiscoveryForm_toolsToolStripMenuItem_showAllPopoutsInTaskBarToolStripMenuItem_turnOffAllTransparencyToolStripMenuItem, EDTx.EDDiscoveryForm_toolsToolStripMenuItem_exitToolStripMenuItem, EDTx.EDDiscoveryForm_adminToolStripMenuItem, 
                EDTx.EDDiscoveryForm_adminToolStripMenuItem_syncEDSMSystemsToolStripMenuItem, EDTx.EDDiscoveryForm_adminToolStripMenuItem_syncEDSMSystemsToolStripMenuItem_sendUnsyncedEDSMJournalsToolStripMenuItem, 
                EDTx.EDDiscoveryForm_adminToolStripMenuItem_syncEDSMSystemsToolStripMenuItem_fetchLogsAgainToolStripMenuItem, EDTx.EDDiscoveryForm_adminToolStripMenuItem_syncEDSMSystemsToolStripMenuItem_fetchStarDataAgainToolStripMenuItem, 
                EDTx.EDDiscoveryForm_adminToolStripMenuItem_rescanAllJournalFilesToolStripMenuItem, EDTx.EDDiscoveryForm_adminToolStripMenuItem_sendHistoricDataToInaraToolStripMenuItem, 
                EDTx.EDDiscoveryForm_adminToolStripMenuItem_rebuildUserDBIndexesToolStripMenuItem, EDTx.EDDiscoveryForm_adminToolStripMenuItem_rebuildSystemDBIndexesToolStripMenuItem,
                EDTx.EDDiscoveryForm_adminToolStripMenuItem_updateUnknownSystemCoordsWithDataFromSystemDBToolStripMenuItem, EDTx.EDDiscoveryForm_adminToolStripMenuItem_showLogfilesToolStripMenuItem,
                EDTx.EDDiscoveryForm_adminToolStripMenuItem_dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem, EDTx.EDDiscoveryForm_adminToolStripMenuItem_deleteDuplicateFSDJumpEntriesToolStripMenuItem, 
                EDTx.EDDiscoveryForm_adminToolStripMenuItem_read21AndFormerLogFilesToolStripMenuItem, EDTx.EDDiscoveryForm_adminToolStripMenuItem_read21AndFormerLogFilesToolStripMenuItem_load21ToolStripMenuItem, 
                EDTx.EDDiscoveryForm_adminToolStripMenuItem_read21AndFormerLogFilesToolStripMenuItem_read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem, EDTx.EDDiscoveryForm_addOnsToolStripMenuItem, 
                EDTx.EDDiscoveryForm_addOnsToolStripMenuItem_manageAddOnsToolStripMenuItem, EDTx.EDDiscoveryForm_addOnsToolStripMenuItem_configureAddOnActionsToolStripMenuItem,
                EDTx.EDDiscoveryForm_addOnsToolStripMenuItem_editLastActionPackToolStripMenuItem, EDTx.EDDiscoveryForm_addOnsToolStripMenuItem_stopCurrentlyRunningActionProgramToolStripMenuItem, EDTx.EDDiscoveryForm_helpToolStripMenuItem, 
                EDTx.EDDiscoveryForm_helpToolStripMenuItem_aboutToolStripMenuItem, EDTx.EDDiscoveryForm_helpToolStripMenuItem_wikiHelpToolStripMenuItem, EDTx.EDDiscoveryForm_helpToolStripMenuItem_viewHelpVideosToolStripMenuItem, 
                EDTx.EDDiscoveryForm_helpToolStripMenuItem_eDDiscoveryChatDiscordToolStripMenuItem, 
                EDTx.EDDiscoveryForm_helpToolStripMenuItem_frontierForumThreadToolStripMenuItem, EDTx.EDDiscoveryForm_helpToolStripMenuItem_gitHubToolStripMenuItem, EDTx.EDDiscoveryForm_helpToolStripMenuItem_reportIssueIdeasToolStripMenuItem,
                EDTx.EDDiscoveryForm_helpToolStripMenuItem_toolStripMenuItemListBindings,
                EDTx.EDDiscoveryForm_helpToolStripMenuItem_checkForNewReleaseToolStripMenuItem };

            BaseUtils.Translator.Instance.TranslateToolstrip(mainMenu, enumlistcms3, this);

            var enumlisttt = new Enum[] { EDTx.EDDiscoveryForm_tabControlMain_ToolTip, EDTx.EDDiscoveryForm_comboBoxCommander_ToolTip, EDTx.EDDiscoveryForm_buttonExtRefresh_ToolTip, EDTx.EDDiscoveryForm_comboBoxCustomProfiles_ToolTip,
                EDTx.EDDiscoveryForm_buttonExtManageAddOns_ToolTip, EDTx.EDDiscoveryForm_buttonExtEditAddOns_ToolTip, EDTx.EDDiscoveryForm_buttonExtPopOut_ToolTip, EDTx.EDDiscoveryForm_buttonReloadActions_ToolTip , EDTx.EDDiscoveryForm_extButtonCAPI_ToolTip };

            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);

            panelToolBar.SetToolTip(toolTip);    // use the defaults

            if (EDDOptions.Instance.ActionButton)
            {
                buttonReloadActions.Visible = true;
                extButtonCAPI.Visible = true;
            }

            extButtonDrawnHelp.Text = "";
            extButtonDrawnHelp.Image = ExtendedControls.TabStrip.HelpIcon;

            // ---------------------------------------------------------------- open all the major tabs except the built in ones

            msg.Invoke("Loading Tabs");
            Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDF Creating major tabs Now");        // STAGE 3 Tabs

            if (EDDOptions.Instance.TabsReset)
            {
                EliteDangerousCore.DB.UserDatabase.Instance.DeleteKey("GridControlWindows%");              // these hold the grid/splitter control values for all windows
                EliteDangerousCore.DB.UserDatabase.Instance.DeleteKey("SplitterControlWindows%");          // wack them so they start empty.
                EliteDangerousCore.DB.UserDatabase.Instance.DeleteKey("SavedPanelInformation.%");          // and delete the pop out history
                EliteDangerousCore.DB.UserDatabase.Instance.DeleteKey("ProfilePowerOnID");                 // back to base profile
            }

            // Make sure the primary splitter is set up.. and rational

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

            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                tabControlMain.AllowDragReorder = false;

            UpdatePanelListInContextMenuStrip();

            removeTabToolStripMenuItem.Click += (s, e) => tabControlMain.RemoveTab(tabControlMain.LastTabClicked);
            renameTabToolStripMenuItem.Click += (s, e) =>
            {
                string newvalue = ExtendedControls.PromptSingleLine.ShowDialog(this,
                                "Name:".T(EDTx.EDDiscoveryForm_RTABL), tabControlMain.TabPages[tabControlMain.LastTabClicked].Text,
                                "Rename Tab".T(EDTx.EDDiscoveryForm_RTABT), this.Icon, false, "Enter a new name for the tab".T(EDTx.EDDiscoveryForm_RTABTT));
                if (newvalue != null)
                    tabControlMain.RenameTab(tabControlMain.LastTabClicked, newvalue.Replace(";", "_"));
            };

            helpTabToolStripMenuItem.Click += (s, e) => { tabControlMain.HelpOn(this, contextMenuStripTabs.PointToScreen(new Point(0, 0)), tabControlMain.LastTabClicked); };

            Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDF Finish ED Init");
        }

        // OnLoad is called the first time the form is shown, before OnShown or OnActivated are called

        private void EDDiscoveryForm_Load(object sender, EventArgs e)
        {
            Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDF Load");

            if (!EDDOptions.Instance.NoTabs)        // load the tabs so when shown is done they are there..
                tabControlMain.LoadTabs();

            Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDF Load Complete");
        }


        // OnShown is called once
        private void EDDiscoveryForm_Shown(object sender, EventArgs e)
        {
            Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDF shown");

            if (EDDConfig.Instance.EDSMGridIDs == "Not Set")        // initial state
            {
                EDDConfig.Instance.EDSMDownload = false;        // this stops the download working in the controller thread
                var ressel = GalaxySectorSelect.SelectGalaxyMenu(this);
                EDDConfig.Instance.EDSMDownload = ressel.Item2 != "None";
                EDDConfig.Instance.EDSMGridIDs = ressel.Item2;
                if (EDDConfig.Instance.EDSMDownload)
                    Controller.AsyncPerformSync(edsmfullsync: true, edsm_alias_sync: true);      // order another go.
            }

            actioncontroller.ReLoad();          // load the action system up here, with the UI running

            Controller.PostInit_Shown();        // form is up, controller is released, create controller background thread

            if (EDDOptions.Instance.NoWindowReposition == false)
                PopOuts.LoadSavedPopouts();  //moved from initial load so we don't open these before we can draw them properly

            actioncontroller.onStartup();

            actioncontroller.CheckWarn();

            ScreenshotConverter.Start((a) => Invoke(a),
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

            ScreenshotConverter.OnScreenshot += (infile, outfile, imagesize, ss) => // screenshot seen
            {
                if (ss != null)
                {
                    ss.SetConvertedFilename(infile ?? "Deleted", outfile, imagesize.Width, imagesize.Height);       // record in SS the in/out files
                }

                ScreenShotCaptured?.Invoke(outfile, imagesize);         // tell others screen shot is captured
            };

            Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDF Web");

            WebServerControl(EDDConfig.Instance.WebServerEnable, EDDConfig.Instance.WebServerPort);

            tabControlMain.SelectedIndexChanged += (snd, ea) =>
            {
                if (tabControlMain.SelectedIndex >= 0)   // may go to -1 on a clear all
                    ActionRun(Actions.ActionEventEDList.onTabChange, new BaseUtils.Variables("TabName", tabControlMain.TabPages[tabControlMain.SelectedIndex].Text));
            };

            // Check on DLL load result and see if new DLLs
 
            if (dllresults.Item3.HasChars())       // new DLLs
            {
                string[] list = dllresults.Item3.Split(',');
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
                        dllsalloweddisallowed = dllsalloweddisallowed.AppendPrePad("+" + dll, ",");
                        changed = true;
                    }
                    else
                    {
                        dllsalloweddisallowed = dllsalloweddisallowed.AppendPrePad("-" + dll, ",");
                    }
                }

                if (changed)
                {
                    DLLManager.UnLoad();
                    Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDF Reload DLL");
                    dllresults = DLLStart(ref dllsalloweddisallowed);
                }
            }

            EDDConfig.Instance.DLLPermissions = dllsalloweddisallowed;        // write back the permission string

            if (dllresults.Item1.HasChars())   // ok
                LogLine(string.Format("DLLs loaded: {0}".T(EDTx.EDDiscoveryForm_DLLL), dllresults.Item1));
            if (dllresults.Item2.HasChars())   // failed
                LogLineHighlight(string.Format("DLLs failed to load: {0}".T(EDTx.EDDiscoveryForm_DLLF), dllresults.Item2));
            if (dllresults.Item4.HasChars())   // failed
                LogLine(string.Format("DLLs disabled: {0}".T(EDTx.EDDiscoveryForm_DLLDIS), dllresults.Item4));

            LogLine(string.Format("Profile {0} Loaded".T(EDTx.EDDiscoveryForm_PROFL), EDDProfiles.Instance.Current.Name));

            // Bindings
            Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDF Bindings");

            if (actioncontroller.FrontierBindings.FileLoaded != null)
            {
                if (actioncontroller.FrontierBindings.FileLoaded.HasChars() )
                    LogLine("Loaded Bindings " + actioncontroller.FrontierBindings.FileLoaded);
                else
                    LogLine("No Bindings File Found");
                if (actioncontroller.FrontierBindings.ErrorList.HasChars())
                    LogLineHighlight("Bindings Errors " + Environment.NewLine + actioncontroller.FrontierBindings.ErrorList);
            }
            else
                LogLine("Frontier bindings did not load");

            Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDF Notifications");

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

            Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDF Installer");
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

                var excllist = new Type[] { typeof(System.Drawing.Icon), typeof(System.Drawing.Image), typeof(System.Drawing.Bitmap), typeof(QuickJSON.JObject) };

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

            DLLManager.Shown();     // tell the DLLs form has shown

            Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDF End shown");

            // test code - open all types of panel
            //foreach (PanelInformation.PanelIDs pc in Enum.GetValues(typeof(PanelInformation.PanelIDs))) { if (pc != PanelInformation.PanelIDs.GroupMarker) tabControlMain.EnsureMajorTabIsPresent(pc, false); }
            // test code - close down all panels except tab 0
            //foreach (PanelInformation.PanelIDs pc in Enum.GetValues(typeof(PanelInformation.PanelIDs))) { if (pc != PanelInformation.PanelIDs.GroupMarker) { TabPage p = tabControlMain.GetMajorTab(pc); if (p != null && p.TabIndex>0) tabControlMain.RemoveTab(p); } }
        }

        private void EDDiscoveryForm_Resize(object sender, EventArgs e)
        {
            // We may be getting called by this.ResumeLayout() from InitializeComponent().
            if (EDDConfig.Instance != null && FormShownOnce)
            {
                if (EDDConfig.Instance.UseNotifyIcon && EDDConfig.Instance.MinimizeToNotifyIcon)
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

        public bool SelectTabPage(string name, bool openit, bool closeit)
        {
            PanelInformation.PanelIDs? id = PanelInformation.GetPanelIDByWindowsRefName(name);

            if (id == null)     // if a name, perform a name select
            {
                TabPage p = tabControlMain.FindTabPageByName(name);
                if (p != null)
                {
                    if (closeit)
                        tabControlMain.RemoveTab(p);
                    else
                        tabControlMain.SelectTab(p);
                    return true;
                }
            }
            else
            {
                TabPage p = tabControlMain.GetMajorTab(id.Value);
                if (p == null)
                {
                    if (openit)
                    {
                        tabControlMain.EnsureMajorTabIsPresent(id.Value, true);
                        return true;
                    }
                }
                else
                {
                    if (closeit)
                        tabControlMain.RemoveTab(p);
                    else
                        tabControlMain.SelectTab(p);
                    return true;
                }
            }
            return false;
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

        private void UpdatePanelListInContextMenuStrip()
        {
            addTabToolStripMenuItem.DropDownItems.Clear();

            foreach (PanelInformation.PanelIDs pid in PanelInformation.GetUserSelectablePanelIDs(EDDConfig.Instance.SortPanelsByName))
            {
                ToolStripMenuItem tsmi = PanelInformation.MakeToolStripMenuItem(pid,
                    (s, e) => tabControlMain.AddTab((PanelInformation.PanelIDs)((s as ToolStripMenuItem).Tag), tabControlMain.LastTabClicked));

                if (tsmi != null)
                    addTabToolStripMenuItem.DropDownItems.Add(tsmi);

                ToolStripMenuItem tsmi2 = PanelInformation.MakeToolStripMenuItem(pid,
                    (s, e) => PopOuts.PopOut((PanelInformation.PanelIDs)((s as ToolStripMenuItem).Tag)));

                if (tsmi2 != null)
                    popOutPanelToolStripMenuItem.DropDownItems.Add(tsmi2);
            }
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

        public bool disallowclose = true;
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = disallowclose;
            System.Diagnostics.Debug.WriteLine($"EDF form closing called {Controller.PendingClose} {disallowclose}");

            if (!Controller.PendingClose)       // if not shutting down..
            {
                bool goforit = !in_system_sync || ExtendedControls.MessageBoxTheme.Show("EDDiscovery is updating the EDSM databases\r\nPress OK to close now, Cancel to wait until update is complete".T(EDTx.EDDiscoveryForm_CloseWarning), "Warning".T(EDTx.Warning), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK;

                if (goforit)
                {
                    if (tabControlMain.AllowClose() == false)
                        goforit = false;
                    else if (PopOuts.AllowClose() == false)
                        goforit = false;
                }

                if (goforit)
                {
                    edsmRefreshTimer.Enabled = false;
                    ReportRefreshProgress(-1, "Closing, please wait!".T(EDTx.EDDiscoveryForm_Closing));
                    actioncontroller.ActionRun(Actions.ActionEventEDList.onShutdown);
                    Controller.Shutdown();
                }
            }

            base.OnFormClosing(e);
        }

        private void Controller_FinalClose()        // run in UI, when controller finishes close
        {
            actioncontroller.HoldTillProgStops();

            if (WebServer.Running)
                WebServer.Stop();

            ScreenshotConverter.SaveSettings();
            ScreenshotConverter.Stop();

            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("ToolBarPanelPinState", panelToolBar.PinState);

            SaveThemeToDB(ExtendedControls.Theme.Current);

            tabControlMain.CloseTabList();      // close and save tab list

            PopOuts.SaveCurrentPopouts();

            notifyIconEDD.Visible = false;
            notifyIconEDD.Dispose();

            actioncontroller.CloseDown();

            DLLManager.UnLoad();

            disallowclose = false;
            Close();
            Application.Exit();
        }
     
        #endregion
    
        #region Tools Menu

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControlMain.EnsureMajorTabIsPresent(PanelInformation.PanelIDs.Settings, true);
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
            if (Controller.history.IsRealCommanderId)
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

        private void paneleddiscovery_Click(object sender, EventArgs e)
        {
            AboutBox(this);
        }

        private void toolStripMenuItemListBindings_Click(object sender, EventArgs e)
        {
            ExtendedControls.InfoForm ifrm = new ExtendedControls.InfoForm();
            string t = actioncontroller.FrontierBindings.ListBindings();
            if (actioncontroller.FrontierBindings.ErrorList.HasChars())
            {
                t = actioncontroller.FrontierBindings.ErrorList + Environment.NewLine + t;
            }
            ifrm.Info("Bindings", this.Icon, t);
            ifrm.Show(this);
        }


        #endregion

        #region Toolbar

        public void LoadCommandersListBox()
        {
            comboBoxCommander.Enabled = false;
            comboBoxCommander.Items.Clear();            // comboBox is nicer with items

            if (EDDOptions.Instance.DisableCommanderSelect)
            {
                comboBoxCommander.Items.Add("Jameson");
                comboBoxCommander.SelectedIndex = 0;
            }
            else
            {
                comboBoxCommander.Items.AddRange((from EDCommander c in EDCommander.GetListInclHidden() select c.Name).ToList());
                if (history.CommanderId == -1)  // is hidden log
                {
                    comboBoxCommander.SelectedIndex = 0;
                }
                else
                {
                    comboBoxCommander.SelectedItem = EDCommander.Current.Name;
                }

                comboBoxCommander.Enabled = true;
            }
        }

        private void comboBoxCommander_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCommander.SelectedIndex >= 0 && comboBoxCommander.Enabled)     // DONT trigger during LoadCommandersListBox
            {
                var itm = (from EDCommander c in EDCommander.GetListInclHidden() where c.Name.Equals(comboBoxCommander.Text) select c).ToList();
                ChangeToCommander(itm[0].Id);
            }
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
            popoutdropdown.FitImagesToItemHeight = true;
            popoutdropdown.SelectedIndexChanged += (s, ea) =>
            {
                PopOuts.PopOut(pids[popoutdropdown.SelectedIndex]);
            };

            ExtendedControls.Theme.Current.ApplyStd(popoutdropdown,true);
            popoutdropdown.SelectionBackColor = ExtendedControls.Theme.Current.ButtonBackColor;
            popoutdropdown.Show(this);
        }

        private void extButtonDrawnHelp_Click(object sender, EventArgs e)
        {
            tabControlMain.HelpOn(this,extButtonDrawnHelp.PointToScreen(new Point(0, extButtonDrawnHelp.Bottom)), tabControlMain.SelectedIndex);
        }

        private void buttonReloadActions_Click(object sender, EventArgs e)
        {
            actioncontroller.ReLoad();
            actioncontroller.CheckWarn();
            actioncontroller.onStartup();
            Controller.ResetUIStatus();

            // keep for debug:

            //var tx = BaseUtils.Translator.Instance.NotUsed();  foreach (var s in tx) System.Diagnostics.Debug.WriteLine(s); // turn on usetracker at top to use

            //if (FrontierCAPI.Active && !EDCommander.Current.ConsoleCommander)
            //  Controller.DoCAPI(history.GetLast.Status.StationName, history.GetLast.System.Name, false, history.Shipyards.AllowCobraMkIV);
        }

        private void extButtonCAPI_Click(object sender, EventArgs e)
        {
            var he = history.GetLast;
            if (he != null && he.IsDocked)
                Controller.DoCAPI(he.WhereAmI, he.System.Name, history.Shipyards.AllowCobraMkIV);
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
                if (EDDConfig.Instance.MinimizeToNotifyIcon)
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
                if (EDDConfig.Instance.UseNotifyIcon && EDDConfig.Instance.MinimizeToNotifyIcon)
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



