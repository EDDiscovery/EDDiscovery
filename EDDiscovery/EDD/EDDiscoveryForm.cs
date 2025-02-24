/*
 * Copyright © 2015 - 2024 EDDiscovery development team
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
 */

using BaseUtils;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        public UserControls.UserControlTravelGrid PrimaryTravelGrid { get { return tabControlMain.PrimarySplitterTab.GetTravelGrid; } }
        public UserControls.UserControlContainerSplitter PrimarySplitter { get { return tabControlMain.PrimarySplitterTab; } }

        public EliteDangerousCore.ScreenShots.ScreenShotConverter ScreenshotConverter { get; set; }
        public PopOutControl PopOuts { get; set; }

        public EliteDangerousCore.GMO.GalacticMapping GalacticMapping { get; private set; }
        public EliteDangerousCore.GMO.GalacticMapping EliteRegions { get; private set; }

        public HistoryList History { get { return Controller.History; } }

        public string LogText { get; private set; } = "";

        public EliteDangerousCore.UIEvents.UIOverallStatus UIOverallStatus { get { return Controller.UIOverallStatus; } }
        #endregion

        #region Callbacks from us
        public event Action<Object> OnNewTarget;
        public event Action<Object, HistoryEntry> OnNoteChanged;        // UI.Note has been updated attached to this note
        public event Action OnAddOnsChanged;                            // add on changed
        public event Action<PanelInformation.PanelIDs> OnPanelAdded;    // panel was added (DLL, action script panels)
        public event Action<PanelInformation.PanelIDs> OnPanelRemoved;  // panel was removed (ditto)
        public event Action<int,string> OnEDSMSyncComplete;             // EDSM journal sync has completed 
        public event Action<int> OnEDDNSyncComplete;                    // Sync has completed
                                                                        // theme is changing/ then has been changed by settings, hook if you have some UI which needs refreshing due to it. 
        public event Action OnThemeChanging;                            // Note you won't get it on startup because theme is applied to form before tabs/panels are setup. Before themeing
        public event Action OnThemeChanged;                             // Note you won't get it on startup because theme is applied to form before tabs/panels are setup
        public event Action<string, Size> ScreenShotCaptured;           // screen shot has been captured
        public event Action<string, Color> OnNewLogEntry;               // Mirrored. New log entry generated.
        #endregion

        #region Events due to EDDiscoveryControl 
        public event Action OnRefreshCommanders { add { Controller.OnRefreshCommanders += value; } remove { Controller.OnRefreshCommanders -= value; } }       
        public event Action OnHistoryChange { add { Controller.OnHistoryChange += value; } remove { Controller.OnHistoryChange -= value; } }
        public event Action<HistoryEntry> OnNewEntry { add { Controller.OnNewEntry += value; } remove { Controller.OnNewEntry -= value; } }
        public event Action<HistoryEntry> OnNewHistoryEntryUnfiltered { add { Controller.OnNewHistoryEntryUnfiltered += value; } remove { Controller.OnNewHistoryEntryUnfiltered -= value; } }
        public event Action<JournalEntry> OnNewJournalEntryUnfiltered { add { Controller.OnNewJournalEntryUnfiltered += value; } remove { Controller.OnNewJournalEntryUnfiltered -= value; } }
        public event Action<UIEvent> OnNewUIEvent { add { Controller.OnNewUIEvent += value; } remove { Controller.OnNewUIEvent -= value; } }
        public event Action<bool> OnExpeditionsDownloaded { add { Controller.OnExpeditionsDownloaded += value; } remove { Controller.OnExpeditionsDownloaded -= value; } }
        public event Action<long, long> OnSyncComplete { add { Controller.OnSyncComplete += value; } remove { Controller.OnSyncComplete -= value; } }

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

        private EDDiscoveryController Controller;
        private Actions.ActionController actioncontroller;
        private BaseUtils.GitHubRelease newRelease;
        private Timer periodicchecktimer;
        private bool in_system_sync = false;        // between start/end sync of databases

        private AudioExtensions.IAudioDriver audiodriverwave;
        private AudioExtensions.AudioQueue audioqueuewave;
        private AudioExtensions.IAudioDriver audiodriverspeech;
        private AudioExtensions.AudioQueue audioqueuespeech;
        private AudioExtensions.SpeechSynthesizer speechsynth;

        private BindingsFile frontierbindings;

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

            Controller.OnSyncStarting += () => {  in_system_sync = true; };
            Controller.OnSyncComplete += (c1,c2) => { in_system_sync = false; };

            Controller.OnNewHistoryEntryUnfiltered += Controller_NewHistoryEntryUnfiltered; // called before being added to the HE, unfiltered, unmerged stream
            Controller.OnNewEntrySecond += Controller_NewEntrySecond;       // called after UI updates themselves with NewEntry
            Controller.OnNewUIEvent += Controller_NewUIEvent;       // called if its an UI event
            Controller.OnNewCommanderDuringPlayDetected += Controller_NewCommanderDuringPlay;

            Controller.LogLine += (s) => { this.BeginInvoke((MethodInvoker)delegate { LogLine(s); }); };
            Controller.LogLineHighlight += (s) => { this.BeginInvoke((MethodInvoker)delegate { LogLineHighlight(s); }); };
            Controller.StatusLineUpdate += (c,p,s) => { this.BeginInvoke((MethodInvoker)delegate { StatusLineUpdate(c,p,s); }); };
        }

        // called from EDDApplicationContext .. continues on with the construction of the system
        public void Init(Action<string> msg)  
        {
            System.Diagnostics.Trace.WriteLine($"Elite Dangerous Discovery Initializing - {EDDOptions.Instance.VersionDisplayString}, Platform: {Environment.OSVersion.Platform.ToString()}");

            if (EDDOptions.Instance.Culture != null)
                CultureInfo.CurrentCulture = System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(EDDOptions.Instance.Culture);

            System.Diagnostics.Trace.WriteLine($"EDD UI Culture is {CultureInfo.CurrentCulture.Name} {System.Threading.Thread.CurrentThread.CurrentUICulture.Name}");

            // Clean up some log folders

            string logpath = EDDOptions.Instance.LogAppDirectory();

            FileHelpers.DeleteFiles(logpath, "*.hlog", new TimeSpan(2, 0, 0, 0), 256);        // Remove hlogs faster
            FileHelpers.DeleteFiles(logpath, "*.log", new TimeSpan(10, 0, 0, 0), 256);

            if (EDDOptions.Instance.ScanCacheEnabled)                                          // clean out old scan jsons
            {
                FileHelpers.CreateDirectoryNoError(EDDOptions.Instance.ScanCachePath);
                FileHelpers.DeleteFiles(EDDOptions.Instance.ScanCachePath, "*.json", new TimeSpan(7, 0, 0, 0), 256);
            }

            HttpCom.LogPath = logpath;

#if DEBUG
            QuickJSON.JToken.TraceOutput = true;
#endif

            PreInitDebug();        // call any debug we want at this point

            //--- listeners

#if DEBUG
            bool releasebuild = false;
#else
            bool releasebuild = true;
#endif
            // if no debugger or in release build or trace log set.

            if (!System.Diagnostics.Debugger.IsAttached || releasebuild || EDDOptions.Instance.TraceLog != null)
            {
                TraceLog.RedirectTrace(logpath, EDDOptions.Instance.TraceLog);

                TraceLog.LogFileWriterException += ex =>            // now we can attach the log writing highter into it
                {
                    LogLineColor($"Log Writer Exception: {ex}",Color.Red);
                };
            }

            // if no debugger, or log exceptions set
            if (!System.Diagnostics.Debugger.IsAttached || EDDOptions.Instance.LogExceptions)          
            {
                ExceptionCatcher.RedirectExceptions(Properties.Resources.URLProjectFeedback);
            }

            if (EDDOptions.Instance.LogExceptions)
            {
                FirstChanceExceptionCatcher.RegisterFirstChanceExceptionHandler();
            }

            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = EDDOptions.Instance.ProcessPriorityClass;

            if (EDDOptions.Instance.ForceTLS12)
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11;
            }

            GlobalBookMarkList.LoadBookmarks();
            GlobalCaptainsLogList.LoadLog();

            System.Diagnostics.Trace.WriteLine($"EDDInit {BaseUtils.AppTicks.TickCountLap()} EDF Load Icons");
            msg.Invoke("Loading Icons");

            EDDiscovery.Icons.ForceInclusion.Include();      // Force the assembly into the project by a empty call
            BaseUtils.Icons.IconSet.CreateSingleton();
            System.Reflection.Assembly iconasm = BaseUtils.ResourceHelpers.GetAssemblyByName("EDDiscovery.Icons");
            BaseUtils.Icons.IconSet.Instance.LoadIconsFromAssembly(iconasm);
            BaseUtils.Icons.IconSet.Instance.AddAlias("settings", "Controls.Settings");             // from use by action system..
            BaseUtils.Icons.IconSet.Instance.AddAlias("missioncompleted", "Journal.MissionCompleted");
            BaseUtils.Icons.IconSet.Instance.AddAlias("speaker", "Legacy.speaker");
            BaseUtils.Icons.IconSet.Instance.AddAlias("Default", "Legacy.star");        // MUST be present

            System.Diagnostics.Trace.WriteLine($"EDDInit {BaseUtils.AppTicks.TickCountLap()} EDF Load Configuration");
            msg.Invoke("Loading Configuration");

            EDDConfig.Instance.Update();
            EDDProfiles.Instance.LoadProfiles(EDDOptions.Instance.Profile);

            string path = EDDOptions.Instance.IconsPath ?? System.IO.Path.Combine(EDDOptions.Instance.IconsAppDirectory(), "*.zip");
            BaseUtils.Icons.IconSet.Instance.LoadIconPack(path, EDDOptions.Instance.AppDataDirectory, EDDOptions.ExeDirectory());

            System.Diagnostics.Trace.WriteLine($"EDDInit {BaseUtils.AppTicks.TickCountLap()} EDF Load Translations");
            msg.Invoke("Loading Translations");

            if (EDDOptions.Instance.ResetLanguage)
                EDDConfig.Instance.Language = "None";

            string lang = EDDOptions.Instance.SelectLanguage ?? EDDConfig.Instance.Language;

            bool debugtranslation = false;

#if DEBUG
            if (lang == "example-ex")       // if we are loading english, turn on code vs english comparision to see if we can find any out of date english.ex
            {
                Translator.Instance.CompareTranslatedToCode = true;
                debugtranslation = true;
            }
#endif

            bool found = BaseUtils.Translator.Instance.LoadTranslation(lang, 
                    CultureInfo.CurrentUICulture, 
                    EDDOptions.Instance.TranslatorFolders(),
                    EDDOptions.Instance.TranslatorDirectoryIncludeSearchUpDepth, EDDOptions.Instance.AppDataDirectory, 
                    loadorgenglish:debugtranslation,
                    loadfile:debugtranslation,
                    debugout:debugtranslation);

            if (!found && !lang.Contains("Default", StringComparison.InvariantCultureIgnoreCase) && !lang.Contains("Auto", StringComparison.InvariantCultureIgnoreCase))
                ExtendedControls.MessageBoxTheme.Show("Translation file disappeared - check your debugger -translationfolder settings!","Translation file");

            BaseUtils.Translator.Instance.AddExcludedControls(new Type[]
            {   typeof(ExtendedControls.ExtComboBox), typeof(ExtendedControls.NumberBoxDouble),typeof(ExtendedControls.NumberBoxFloat),typeof(ExtendedControls.NumberBoxLong),
                typeof(ExtendedControls.ExtScrollBar),typeof(ExtendedControls.ExtStatusStrip),typeof(ExtendedControls.ExtRichTextBox),typeof(ExtendedControls.ExtTextBox),
                typeof(ExtendedControls.ExtTextBoxAutoComplete),typeof(ExtendedControls.ExtDateTimePicker),typeof(ExtendedControls.ExtNumericUpDown),
                typeof(ExtendedControls.MultiPipControl)});

            System.Diagnostics.Trace.WriteLine($"EDDInit {BaseUtils.AppTicks.TickCountLap()} EDF Initialise Item Data and components");

            MaterialCommodityMicroResourceType.Initialise();     // lets statically fill the table way before anyone wants to access it
            ItemData.Initialise();                              // let the item data initialise
            Stars.Prepopulate();                                // we do it this way instead of statically because we don't want them autofilled
            Planets.Prepopulate();

            System.Diagnostics.Trace.WriteLine($"EDDInit {BaseUtils.AppTicks.TickCountLap()} EDF Initialise Controller");

            Controller.Init();
            PanelInformation.Init();

            // Some components require the controller to be initialized
            InitializeComponent();

            ScreenshotConverter = new EliteDangerousCore.ScreenShots.ScreenShotConverter();
            PopOuts = new PopOutControl(this);

            PopOuts.RequestPanelOperation += tabControlMain.RequestPanelOperationPopOut;          // HOOK requests from the forms into the main tab..

            // load saved commanders

            System.Diagnostics.Debug.WriteLine($"EDDInit {BaseUtils.AppTicks.TickCountLap()} EDF Load Commanders");
            EDCommander.LoadCommanders();

            // STAGE 2 themeing the main interface (not the tab pages)

            System.Diagnostics.Trace.WriteLine($"EDDInit {BaseUtils.AppTicks.TickCountLap()} EDF Load popouts, themes, init controls");
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
                bool fontav = FontLoader.IsFontAvailable("Verdana", 8F, FontStyle.Regular);
                string deftheme = Environment.OSVersion.Platform == PlatformID.Win32NT && fontav ? "Elite Verdana Small" : "Windows Default";
                ThemeList.SetThemeByName(deftheme);                // this is the default theme we use normally
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

            System.Diagnostics.Trace.WriteLine($"EDDInit {BaseUtils.AppTicks.TickCountLap()} EDF Apply theme");
            ApplyTheme();                       // we apply and scale (because its being applied to Form) before any tabs parts are setup.

            this.TopMost = EDDConfig.Instance.KeepOnTop;
            notifyIconEDD.Visible = EDDConfig.Instance.UseNotifyIcon;

            // create audio system, now done in here not in action controller

            System.Diagnostics.Trace.WriteLine($"EDDInit {BaseUtils.AppTicks.TickCountLap()} EDF Load audio");

#if !NO_SYSTEM_SPEECH
            // Windows TTS (2000 and above). Speech *recognition* will be Version.Major >= 6 (Vista and above)
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 5 && !EDDOptions.Instance.NoSound)
            {
                audiodriverwave = AudioHelper.GetAudioDriver(LogLineHighlight, EDDConfig.Instance.DefaultWaveDevice);
                audiodriverspeech = AudioHelper.GetAudioDriver(LogLineHighlight, EDDConfig.Instance.DefaultVoiceDevice);
                AudioExtensions.ISpeechEngine speechengine;

                speechengine = AudioHelper.GetSpeechEngine(LogLineHighlight);
                speechsynth = new AudioExtensions.SpeechSynthesizer(speechengine);
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

            // Frontier bindings

            frontierbindings = new BindingsFile();

            frontierbindings.LoadBindingsFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Frontier Developments", "Elite Dangerous", "Options", "Bindings"), true);

            //System.Diagnostics.Debug.WriteLine("Bindings" + frontierbindings.ListBindings());
            //System.Diagnostics.Debug.WriteLine("Key Names" + frontierbindings.ListKeyNames("{","}"));


            System.Diagnostics.Trace.WriteLine($"EDDInit {BaseUtils.AppTicks.TickCountLap()} EDF Load action controller");

            // install extra functions

            Functions.GetCFH = ConditionEDDFunctions.DefaultGetCFH;

            // create the action controller and install commands before we execute tabs, since some tabs need these set up

            string eddiscoveryglobalvars = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString("UserGlobalActionVars", "");
            actioncontroller = MakeAC(this,
                        EDDOptions.Instance.ActionsAppDirectory(), EDDOptions.Instance.AppDataDirectory, EDDOptions.Instance.OtherInstallFilesDirectory(), 
                        eddiscoveryglobalvars,
                        LogLine);

            msg.Invoke("Loading Action Packs");         // STAGE 4 Action packs

            // ---------------------------------------------------------------- Finish up any installing/deleting which failed during the upgrade process because the files were in use

            System.Diagnostics.Trace.WriteLine($"EDDInit {BaseUtils.AppTicks.TickCountLap()} EDF Install/remove previous files");

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

            EliteDangerousCore.EDSM.EDSMJournalSync.SentEvents = (count,list) =>              // Sync thread finishing, transfers to this thread, then runs the callback and the action..
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    System.Diagnostics.Debug.Assert(Application.MessageLoop);
                    OnEDSMSyncComplete?.Invoke(count,list);
                    ActionRun(Actions.ActionEventEDList.onEDSMSync, new BaseUtils.Variables(new string[] { "EventStarList", list, "EventCount", count.ToStringInvariant() }));
                });
            };

            EliteDangerousCore.EDDN.EDDNSync.SentEvents = (count) =>              // Sync thread finishing, transfers to this thread, then runs the callback and the action..
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    System.Diagnostics.Debug.Assert(Application.MessageLoop);
                    OnEDDNSyncComplete?.Invoke(count);
                    ActionRun(Actions.ActionEventEDList.onEDDNSync, new BaseUtils.Variables(new string[] { "EventCount", count.ToStringInvariant() }));
                });
            };

            //----------------------------------------------------------------- Do translations before any thing else gets added to these toolbars

            var enumlistcms = new Enum[] { EDTx.EDDiscoveryForm_addTabToolStripMenuItem, EDTx.EDDiscoveryForm_removeTabToolStripMenuItem, EDTx.EDDiscoveryForm_renameTabToolStripMenuItem, EDTx.EDDiscoveryForm_popOutPanelToolStripMenuItem, EDTx.EDDiscoveryForm_helpTabToolStripMenuItem };
            BaseUtils.Translator.Instance.TranslateToolstrip(contextMenuStripTabs, enumlistcms, this);        // need to translate BEFORE we add in extra items

            var enumlistcms2 = new Enum[] { EDTx.EDDiscoveryForm_notifyIconMenu_Open, EDTx.EDDiscoveryForm_notifyIconMenu_Hide, EDTx.EDDiscoveryForm_notifyIconMenu_Exit };
            BaseUtils.Translator.Instance.TranslateToolstrip(notifyIconContextMenuStrip, enumlistcms2, this);        // need to translate BEFORE we add in extra items

            // ---------------------------------------------------------------- DLL Load

            msg.Invoke("Loading Extension DLLs");
            System.Diagnostics.Trace.WriteLine($"EDDInit {BaseUtils.AppTicks.TickCountLap()} EDF DLL setup");

            DLLStart();

            //----------------------------------------------------------------- Action controller (moved here oct 24 to have panels created before load)

            // this takes a long time too
            actioncontroller.ReLoad();          // load the action system up here, with the UI running
            actioncontroller.CreatePanelsFromActionFiles();    // create any panels related to plugins

            // ---------------------------------------------------------------- Web server

            WebServer = new WebServer.EDDWebServer(this);

            //----------------------------------------------------------------- GMO etc load

            System.Diagnostics.Trace.WriteLine($"EDDInit {BaseUtils.AppTicks.TickCountLap()} EDF Load GMO");

            GalacticMapping = new EliteDangerousCore.GMO.GalacticMapping();         // empty in case someone needs them
            EliteRegions = new EliteDangerousCore.GMO.GalacticMapping();

            Task.Factory.StartNew(() =>
            {
                // normally updated by DownloadEDSMGEC in ControllerHelpers, but first time run, we want them now, before we continue

                string edsmgmofile = Path.Combine(EDDOptions.Instance.AppDataDirectory, "galacticmapping.json");

                if (!EDDOptions.Instance.NoSystemsLoad && !File.Exists(edsmgmofile))        // if allowed to load, and no gmo file, fetch immediately
                {
                    LogLine("Get galactic mapping from EDSM.".T(EDTx.EDDiscoveryController_EDSM));
                    if (EliteDangerousCore.EDSM.EDSMClass.DownloadGMOFileFromEDSM(edsmgmofile, new System.Threading.CancellationToken()))
                        SystemsDatabase.Instance.SetEDSMGalMapLast(DateTime.UtcNow);
                }

                string gecfile = Path.Combine(EDDOptions.Instance.AppDataDirectory, "gecmapping.json");

                if (!EDDOptions.Instance.NoSystemsLoad && !File.Exists(gecfile))        // if allowed to load, and no gec file, fetch immediately
                {
                    LogLine("Get galactic mapping from GEC.".T(EDTx.EDDiscoveryController_GEC));
                    if (EliteDangerousCore.GEC.GECClass.DownloadGECFile(gecfile, new System.Threading.CancellationToken()))
                        SystemsDatabase.Instance.SetGECGalMapLast(DateTime.UtcNow);
                }


                // in priority order..

                if (File.Exists(gecfile))
                    GalacticMapping.ParseGMPFile(gecfile, int.MaxValue / 2);                    // at this point, gal map data has been uploaded - get it into memory

                if (File.Exists(edsmgmofile))
                    GalacticMapping.ParseGMPFile(edsmgmofile, 0);                            // at this point, gal map data has been uploaded - get it into memory

                GalacticMapping.LoadCSV(EDDiscovery.Properties.Resources.TouristBeacons, "GECTB", "", "Community Sourced Tourist Beacon");

                GalacticMapping.LoadCSV(EDDiscovery.Properties.Resources.Marx_Nebula_List_26_10_21, "MarxNebula", " Nebula", "Marx sourced nebula");

                // GalacticMapping.Dump();

                {
                    var text = System.Text.Encoding.UTF8.GetString(Properties.Resources.EliteGalacticRegions);
                    EliteRegions.ParseGMPJson(text, int.MaxValue / 2 + 100000);                            // at this point, gal map data has been uploaded - get it into memory
                }

                SystemCache.AddToAutoCompleteList(GalacticMapping.GetGMPNames());
            });

            //-------------------------------------------------------------------- Profile

            System.Diagnostics.Trace.WriteLine($"EDDInit {BaseUtils.AppTicks.TickCountLap()} EDF Load Profile");

            UpdateProfileComboBox();
            comboBoxCustomProfiles.SelectedIndexChanged += ComboBoxCustomProfiles_SelectedIndexChanged;

            //---------------------------------------------------------------------- Tool tips

            System.Diagnostics.Trace.WriteLine($"EDDInit {BaseUtils.AppTicks.TickCountLap()} EDF Translate UI");

            var enumlistcms3 = new Enum[] { EDTx.EDDiscoveryForm_toolsToolStripMenuItem, EDTx.EDDiscoveryForm_toolsToolStripMenuItem_settingsToolStripMenuItem, 
                EDTx.EDDiscoveryForm_toolsToolStripMenuItem_showAllPopoutsInTaskBarToolStripMenuItem, EDTx.EDDiscoveryForm_toolsToolStripMenuItem_showAllPopoutsInTaskBarToolStripMenuItem_showAllInTaskBarToolStripMenuItem, 
                EDTx.EDDiscoveryForm_toolsToolStripMenuItem_showAllPopoutsInTaskBarToolStripMenuItem_turnOffAllTransparencyToolStripMenuItem, EDTx.EDDiscoveryForm_toolsToolStripMenuItem_exitToolStripMenuItem, EDTx.EDDiscoveryForm_adminToolStripMenuItem, 
                EDTx.EDDiscoveryForm_adminToolStripMenuItem_syncEDSMSystemsToolStripMenuItem, EDTx.EDDiscoveryForm_adminToolStripMenuItem_syncEDSMSystemsToolStripMenuItem_sendUnsyncedEDSMJournalsToolStripMenuItem, 
                EDTx.EDDiscoveryForm_adminToolStripMenuItem_syncEDSMSystemsToolStripMenuItem_fetchLogsAgainToolStripMenuItem,
                EDTx.EDDiscoveryForm_adminToolStripMenuItem_fetchStarDataAgainToolStripMenuItem,
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
                EDTx.EDDiscoveryForm_buttonExtManageAddOns_ToolTip, EDTx.EDDiscoveryForm_buttonExtEditAddOns_ToolTip, EDTx.EDDiscoveryForm_buttonExtPopOut_ToolTip };

            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);

            panelToolBar.SetToolTip(toolTip);    // use the defaults

            if (EDDOptions.Instance.ActionButton)
            {
                buttonReloadActions.Visible = true;
                extButtonCAPI.Visible = true;
            }

            extButtonDrawnHelp.Text = "";
            extButtonDrawnHelp.Image = ExtendedControls.TabStrip.HelpIcon;

            panelToolBar.SetVisibility(extButtonNewFeature, false);         // use the panel tool bar interface to set it invisible, as the TB controls visibility itself

            // ---------------------------------------------------------------- open all the major tabs except the built in ones

            msg.Invoke("Loading Tabs");
            System.Diagnostics.Trace.WriteLine($"EDDInit {BaseUtils.AppTicks.TickCountLap()} EDF Creating major tabs Now");        // STAGE 3 Tabs

            if (EDDOptions.Instance.TabsReset)
            {
               UserDatabase.Instance.DeleteKey("GridControlWindows%");              // these hold the grid/splitter control values for all windows
               UserDatabase.Instance.DeleteKey("SplitterControlWindows%");          // wack them so they start empty.
               UserDatabase.Instance.DeleteKey("SavedPanelInformation.%");          // and delete the pop out history
               UserDatabase.Instance.DeleteKey("ProfilePowerOnID");                 // back to base profile
            }

            // Make sure the primary splitter is set up.. and rational

            UserControls.UserControlContainerSplitter.CheckPrimarySplitterControlSettings(EDDOptions.Instance.TabsReset); // Double check, use TravelControlBottom etc as the old lookup name if its nonsence

            if (!EDDOptions.Instance.NoTabs)
            {
                tabControlMain.MinimumTabWidth = 32;
                tabControlMain.CreateTabs(this, EDDOptions.Instance.TabsReset, "0, -1,0, 26,0, 27,0, 29,0, 34,0");      // numbers from popouts, which are FIXED!
                if (tabControlMain.PrimarySplitterTab == null || tabControlMain.PrimarySplitterTab.GetTravelGrid == null)  // double check we have a primary tab and tg..
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
                string newvalue = ExtendedControls.PromptSingleLine.ShowDialog(this,"Name:".T(EDTx.EDDiscoveryForm_RTABL), tabControlMain.TabPages[tabControlMain.LastTabClicked].Text,
                                "Rename Tab".T(EDTx.EDDiscoveryForm_RTABT), this.Icon, false, "Enter a new name for the tab".T(EDTx.EDDiscoveryForm_RTABTT), requireinput:true);
                if (newvalue != null)
                    tabControlMain.RenameTab(tabControlMain.LastTabClicked, newvalue.Replace(";", "_"));
            };

            helpTabToolStripMenuItem.Click += (s, e) => { tabControlMain.HelpOn(this, contextMenuStripTabs.PointToScreen(new Point(0, 0)), tabControlMain.LastTabClicked); };

            System.Diagnostics.Trace.WriteLine($"EDDInit {BaseUtils.AppTicks.TickCountLap()} EDF Finish ED Init");

            PostInitDebug();        // call any debug we want at this point

        }

        // OnLoad is called the first time the form is shown, before OnShown or OnActivated are called

        private void EDDiscoveryForm_Load(object sender, EventArgs e)
        {
            System.Diagnostics.Trace.WriteLine($"EDDInit {BaseUtils.AppTicks.TickCountLap()} EDF Load");

            // here we install the new may'24 important message hook, now the window has been created. Any *** messages get pumped out
            TraceLog.ImportantMessage += ex => { LogLineColor(ex, Color.FromArgb(255,255,40,40)); };

            // load the tabs so when shown is done they are there..
            if (!EDDOptions.Instance.NoTabs)        
                tabControlMain.LoadTabs();

            if ( EDDOptions.Instance.OpenAllTabTypes)
            {
                foreach( PanelInformation.PanelIDs pid in Enum.GetValues(typeof(PanelInformation.PanelIDs)))
                {
                    if ( pid>=0)
                        tabControlMain.EnsureMajorTabIsPresent(pid,false);
                }
            }

            System.Diagnostics.Trace.WriteLine($"EDDInit {BaseUtils.AppTicks.TickCountLap()} EDF Load Complete");
        }


        // OnShown is called once
        private void EDDiscoveryForm_Shown(object sender, EventArgs e)
        {
            System.Diagnostics.Trace.WriteLine($"EDDInit {BaseUtils.AppTicks.TickCountLap()} EDF shown");

            if (SystemsDatabase.Instance.GetGridIDs() == "Not Set")        // initial state.. this holds up the shown and postinit_shown stopping any background worker action
            {
                var ressel = Forms.GalaxySectorSelect.SelectGalaxyMenu(this, EDDOptions.Instance.SystemDatabasePath);
                SystemsDatabase.Instance.SetDBSource(ressel.Item1);
                SystemsDatabase.Instance.SetGridIDs(ressel.Item3);
                EDDConfig.Instance.SystemDBDownload = ressel.Item3 != "None";
            }

            Controller.PostInit_Shown();        // form is up, controller is released, create controller background thread

            if (EDDOptions.Instance.NoWindowReposition == false)
                PopOuts.LoadSavedPopouts();  //moved from initial load so we don't open these before we can draw them properly

            actioncontroller.onStartup();

            actioncontroller.CheckWarn();

            ScreenshotConverter.Start((a) => Invoke(a),
                             (b) => LogLine(b),
                             () =>
                             {
                                 if (History.GetLast != null)        // lasthe should have name and whereami, and an indication of commander
                                 {
                                     return new Tuple<string, string, string>(History.GetLast.System.Name, History.GetLast.WhereAmI, History.GetLast.Commander?.Name ?? "Unknown");
                                 }
                                 else
                                 {
                                     return new Tuple<string, string, string>("Unknown", "Unknown", "Unknown");
                                 }
                             },
                             3000       // ms to wait after file detected before assuming journal will not be updated
                             );

            ScreenshotConverter.OnScreenshot += (infile, outfile, imagesize, ss) => // screenshot seen
            {
                if (ss != null)
                {
                    ss.SetConvertedFilename(infile ?? "Deleted", outfile, imagesize.Width, imagesize.Height);       // record in SS the in/out files
                }

                ScreenShotCaptured?.Invoke(outfile, imagesize);         // tell others screen shot is captured
            };

            System.Diagnostics.Trace.WriteLine($"EDDInit {BaseUtils.AppTicks.TickCountLap()} EDF Web");

            WebServerControl(EDDConfig.Instance.WebServerEnable, EDDConfig.Instance.WebServerPort);

            tabControlMain.SelectedIndexChanged += (snd, ea) =>
            {
                if (tabControlMain.SelectedIndex >= 0)   // may go to -1 on a clear all
                    ActionRun(Actions.ActionEventEDList.onTabChange, new BaseUtils.Variables("TabName", tabControlMain.TabPages[tabControlMain.SelectedIndex].Text));
            };

            // Check on DLL load result and see if new DLLs

            DLLVerify();

            // Continue..
 

            LogLine(string.Format("Profile {0} Loaded".T(EDTx.EDDiscoveryForm_PROFL), EDDProfiles.Instance.Current.Name));

            // Bindings
            System.Diagnostics.Trace.WriteLine($"EDDInit {BaseUtils.AppTicks.TickCountLap()} EDF Bindings");

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

            System.Diagnostics.Trace.WriteLine($"EDDInit {BaseUtils.AppTicks.TickCountLap()} EDF Notifications");

            // Notifications, only check github when directed and we are not debugging it using a folder override

            Notifications.CheckForNewNotifications(EDDOptions.Instance.CheckGithubFiles && EDDOptions.Instance.NotificationFolderOverride == null, 
                                                   "Notifications", // github folder name
                                                   EDDOptions.Instance.NotificationsAppDirectory(),
                                                   EDDiscovery.Properties.Resources.URLGithubDataDownload,  // github url
                                                   "NotificationsV2",       // xml notification section. Previous version had a flaw where it always showed stuff unless a recognised condition was present. Did not allow for future expansion. 
            (notelist) =>
            {
                this.BeginInvoke(new Action(() =>
                {
                    string acklist = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString("NotificationLastAckTime", "");
                    Version curver = new Version(System.Reflection.Assembly.GetExecutingAssembly().GetAssemblyVersionString());

                    List<BaseUtils.Notifications.Notification> popupnotificationlist = new List<BaseUtils.Notifications.Notification>();

                    // sorted by oldest first..
                    foreach (Notifications.Notification n in notelist)
                    {
                        Notifications.NotificationParas p = n.Select(EDDConfig.Instance.Language);

                        Version vmax = n.VersionMax != null ? new Version(n.VersionMax) : null;
                        Version vmin = n.VersionMin != null ? new Version(n.VersionMin) : null;

                        // if basic checked for time/date and version pass
                        if (p != null && DateTime.UtcNow >= n.StartUTC && DateTime.UtcNow <= n.EndUTC &&
                                (vmax == null || curver <= vmax) && (vmin == null || curver >= vmin))
                        {
                            // one of these must pass
                            if ( n.AlwaysShow == true ||
                                 (n.Conditions.TryGetValue("ConditionActionPackPresent", out string[] pp) && actioncontroller.Get(pp, null).Length > 0) ||
                                 (n.Conditions.TryGetValue("ConditionActionPackNotPresent", out string[] np) && actioncontroller.Get(np, null).Length == 0) ||
                                 (n.Conditions.TryGetValue("ConditionActionPackPresentEnabled", out string[] ep) && actioncontroller.Get(ep, true).Length > 0) ||
                                 (n.Conditions.TryGetValue("ConditionActionPackPresentDisabled", out string[] dp) && actioncontroller.Get(dp, false).Length > 0) ||
                                 (n.Conditions.TryGetValue("ConditionActionPackPresentEnabledOldVersion", out string[] ov) && ov.Length == 2 && actioncontroller.IsOlderEnabled(ov[0], ov[1]))
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
                                else if (n.EntryType == "New")
                                {
                                    extButtonNewFeature.Tag = n;
                                    bool read = UserDatabase.Instance.GetSettingString("NotificationLastNewFeature", "") == n.StartUTC.ToStringZulu();
                                    extButtonNewFeature.Image = read ? EDDiscovery.Icons.Controls.NewFeatureGreen : EDDiscovery.Icons.Controls.NewFeature;
                                    panelToolBar.SetVisibility(extButtonNewFeature, true);         // use the panel tool bar interface to set it visible, as the TB controls visibility itself
                                }
                            }
                        }
                    }

                    ShowNotification(popupnotificationlist);

                }));

            });

            // Now the installer

            System.Diagnostics.Trace.WriteLine($"EDDInit {BaseUtils.AppTicks.TickCountLap()} EDF Installer");

            if (EDDOptions.Instance.CheckRelease )
            {
                GitHubRelease.CheckForNewInstallerAsync(EDDiscovery.Properties.Resources.URLGithubDownload,
                            System.Reflection.Assembly.GetExecutingAssembly().GetAssemblyVersionString(), (rel) =>  // in thread
                {
                    newRelease = rel;
                    BeginInvoke(new Action(() => LogLineHighlight(string.Format("New EDDiscovery installer available: {0}".T(EDTx.EDDiscoveryForm_NI), newRelease.ReleaseName))));
                    BeginInvoke(new Action(() => labelInfoBoxTop.Text = "New Release Available!".T(EDTx.EDDiscoveryForm_NRA)));
                });
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

            System.Diagnostics.Trace.WriteLine($"EDDInit {BaseUtils.AppTicks.TickCountLap()} EDF End shown");

            PostShownDebug();
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

#region Closing

        public bool disallowclose = true;
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = disallowclose;
            System.Diagnostics.Debug.WriteLine($"EDF form closing called {Controller.PendingClose} {disallowclose}");

            if (!Controller.PendingClose.IsCancellationRequested)       // if not shutting down..
            {
                bool goforit = !in_system_sync || ExtendedControls.MessageBoxTheme.Show("EDDiscovery is updating the system database\r\nPress OK to close now, Cancel to wait until update is complete".T(EDTx.EDDiscoveryForm_CloseWarning), "Warning".T(EDTx.Warning), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK;

                if (goforit)
                {
                    if (tabControlMain.AllowClose() == false)
                        goforit = false;
                    else if (PopOuts.AllowClose() == false)
                        goforit = false;
                }

                if (goforit)
                {
                    StatusLineUpdate(EDDiscoveryForm.StatusLineUpdateType.CloseDown, -1,"Closing, please wait!".T(EDTx.EDDiscoveryForm_Closing));
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

            tabControlMain.CloseSaveTabs();      // close and save tab list

            PopOuts.SaveCurrentPopouts();

            notifyIconEDD.Visible = false;
            notifyIconEDD.Dispose();

            string persistentvars = actioncontroller.CloseDown();
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString("UserGlobalActionVars", persistentvars);

            DLLManager.UnLoad();

            audioqueuespeech.StopAll();
            audioqueuewave.StopAll();
            audioqueuespeech.Dispose();     // in order..
            audiodriverspeech.Dispose();
            audioqueuewave.Dispose();
            audiodriverwave.Dispose();

            disallowclose = false;
            Close();

            TraceLog.TerminateLogger();
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
                    System.Diagnostics.Process.Start(cmdrfolder);
                }
            }
        }

        private void syncStarDataSystemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EDDConfig.Instance.SystemDBDownload)
                ExtendedControls.MessageBoxTheme.Show(this, "Star Data download is disabled. Use Settings to reenable it".T(EDTx.EDDiscoveryForm_SDDis));
            else if (ExtendedControls.MessageBoxTheme.Show(this, ("This can take a considerable amount of time and bandwidth" + Environment.NewLine + "Confirm you want to do this?").T(EDTx.EDDiscoveryForm_EDSMQ), "Warning".T(EDTx.Warning), MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk)  == DialogResult.OK )
            {
                if (!Controller.AsyncPerformSync(true))      // we want it to have run, to completion, to allow another go..
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
            if (Controller.History.IsRealCommanderId)
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
            newRelease = GitHubRelease.CheckForNewInstaller(EDDiscovery.Properties.Resources.URLGithubDownload, System.Reflection.Assembly.GetExecutingAssembly().GetAssemblyVersionString());
            if ( newRelease != null )
            {
                using (Forms.NewReleaseForm frm = new Forms.NewReleaseForm(newRelease))
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
                EliteDangerousCore.Inara.InaraSync.HistoricData(LogLine, History, EDCommander.Current);
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
                History.FillInPositionsFSDJumps(LogLine);
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
            Forms.AboutForm frm = new Forms.AboutForm();
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

        private void extButtonNewFeature_Click(object sender, EventArgs e)
        {
            Notifications.Notification n = extButtonNewFeature.Tag as Notifications.Notification;
            Notifications.NotificationParas p = n.Select(EDDConfig.Instance.Language);

            ExtendedControls.InfoForm infoform = new ExtendedControls.InfoForm();
            infoform.Info(p.Caption, this.Icon, p.Text, pointsize: n.PointSize, enableurls: true);
            infoform.LinkClicked += (ef) => { BaseUtils.BrowserInfo.LaunchBrowser(ef.LinkText); };
            infoform.StartPosition = FormStartPosition.CenterParent;
            infoform.Show(this);
            UserDatabase.Instance.PutSettingString("NotificationLastNewFeature", n.StartUTC.ToStringZulu());
            extButtonNewFeature.Image = EDDiscovery.Icons.Controls.NewFeatureGreen;
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
                using (Forms.NewReleaseForm frm = new Forms.NewReleaseForm(newRelease))
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



