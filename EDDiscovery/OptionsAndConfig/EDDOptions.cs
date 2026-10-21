/*
 * Copyright 2018-2025 EDDiscovery development team
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
using System;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;

namespace EDDiscovery
{
    public class EDDOptions : EliteDangerousCore.IEliteOptions
    {
        #region Variables

        public static bool Instanced { get { return options != null; } }

        public static EDDOptions Instance
        {
            get
            {
                if (options == null)
                    options = new EDDOptions();

                return options;
            }
        }

        public string VersionDisplayString { get; private set; }
        public string AppDataDirectory { get; private set; }    
        public string UserDatabasePath { get; private set; }       
        public string UserDatabaseFilename { get; private set; } = "EDDUser.sqlite";
        public string SystemDatabasePath { get; private set; }
        public string ScanCachePath { get; private set; }
        public bool ScanCacheEnabled => !ScanCachePath.EqualsIIC("DISABLED");

        public string SystemDatabaseFilename { get; private set; } = "EDDSystem.sqlite";
        public string IconsPath { get; private set; }
        public bool NoWindowReposition { get; set; }
        public bool MinimiseOnOpen { get; set; }
        public bool MaximiseOnOpen { get; set; }
        public bool ActionButton { get; private set; }
        public bool NoLoad { get; private set; }
        public bool NoTheme { get; set; }
        public bool NoTabs { get; private set; }
        public bool OpenAllTabTypes { get; private set; }
        public bool TabsReset { get; set; }
        public bool NoSystemsLoad { get; private set; }
        public bool NoSound { get; private set; }
        public string TraceLog { get; private set; }        // null = auto file, or fixed name
        public bool LogExceptions { get; private set; }
        public bool DisableShowDebugInfoInTitle { get; private set; }
        public string OptionsFile { get; private set; }
        public bool DontAskGithubForPacks { get; private set; }
        public bool DisableBetaCommanderCheck { get; private set; }
        public bool ForceBetaOnCommander { get; private set; }
        public bool CheckGithubRelease { get; private set; }
        public bool CheckGithubNotifications { get; private set; }
        public bool CheckGithubAddOn { get; private set; }
        public bool ResetLanguage { get; set; }
        public string SelectLanguage { get; private set; }
        public bool SafeMode { get; private set; }
        public bool DisableJournalMerge { get; private set; }
        public bool DisableJournalRemoval { get; private set; }
        public bool MultithreadedJournalRead { get; private set; } = true;
        public string NotificationFolderOverride { get; private set; }      // normally null..
        public float FontSize { get; private set; }                           // override font size, 0 if not
        public string Font { get; private set; }                           // override font, null if not
        public string Commander { get; private set; }                   // set commander, null if not
        public string Profile { get; private set; }                   // set profile, null if not
        public bool TempDirInDataDir { get; private set; }
        public string WebServerFolder { get; private set; }             // normally empty, so selects zip server
        public bool LowPriority { get; private set; }
        public System.Diagnostics.ProcessPriorityClass ProcessPriorityClass { get; private set; } = System.Diagnostics.ProcessPriorityClass.Normal;
        public bool ForceTLS12 { get; private set; }
        public bool DisableTimeDisplay { get; private set; }
        public bool DisableCommanderSelect { get; private set; }
        public bool DisableVersionDisplay { get; private set; }
        public string OutputEventHelp { get; private set; }
        public string DefaultJournalFolder { get; private set; }        // default is null, use computed value
        public string DefaultJournalMatchFilename { get; private set; } = "Journal*.log";      
        public bool EnableTGRightDebugClicks { get; private set; }
        public bool AutoLoadNextCommander { get; private set; }
        public int HistoryLoadDayLimit { get; private set; }    // default zero not set. Overrides the FullHistoryLoadDayLimit in EDDConfig
        public DateTime MinJournalDateUTC { get; private set; }    // default is MinDate, UTC, set by -minjournaldateutc, used by scanner to set a min date limit
        public DateTime? MaxJournalDateUTC { get; private set; }  // if set by -readto, UTC, read only up to this date from DB.  DB may contain further entries
        public bool DeleteSystemDB { get; private set; }        
        public bool DeleteUserDB { get; private set; }
        public bool DeleteUserJournals { get; private set; }

        public bool KeepSystemDataDownloadedFiles { get; private set; }
        public string Culture { get; private set; }             // default null use system culture, use de-DE etc

        public int ZMQPort { get; set; } = 12300;       // < 10000 does not launch the program, you should be running the script via the debugger

        public bool AllowDeviceInput { get { return Environment.OSVersion.Platform == PlatformID.Win32NT; } }
        public bool AllowAudio { get { return Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 5 && !NoSound; } }
        public bool AllowFontLoad { get { return Environment.OSVersion.Platform == PlatformID.Win32NT; } }
        public bool AllowDragReorderTabs { get { return Environment.OSVersion.Platform == PlatformID.Win32NT; } }
        public bool AllowAllPanels { get { return Environment.OSVersion.Platform == PlatformID.Win32NT; } }

        public string SubAppDirectory(string subfolder)     // ensures its there.. name without \ slashes
        {
            string path = Path.Combine(AppDataDirectory, subfolder);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        public string SubExeDirectory(string subfolder)     // may return null if not there
        {
            string path = Path.Combine(ExeDirectory(), subfolder);
            return Directory.Exists(path) ? path : null;
        }

        public string NotificationsAppDirectory() { return NotificationFolderOverride ?? SubAppDirectory("Notifications"); }
        public string ExpeditionsAppDirectory() { return SubAppDirectory("Expeditions"); }
        public string ActionsAppDirectory() { return SubAppDirectory("Actions"); }
        public string OtherInstallFilesDirectory() { return SubAppDirectory("AddonFiles"); }
        public string VideosAppDirectory() { return SubAppDirectory("Videos"); }
        public string SoundsAppDirectory() { return SubAppDirectory("Sounds"); }
        public string IconsAppDirectory() { return SubAppDirectory("Icons"); }
        public string MapsAppDirectory() { return SubAppDirectory("Maps"); }
        public string LogAppDirectory() { return SubAppDirectory("Log"); }
        public string ThemeAppDirectory() { return SubAppDirectory("Theme"); }
        public string DLLAppDirectory() { return SubAppDirectory("DLL"); }
        public string DLLExeDirectory() { return SubExeDirectory("DLL"); }
        public string DownloadedImages() { return SubAppDirectory("Images"); }
        public string CAPIDirectory() { return SubAppDirectory("CAPI"); }
        public string TempDirectory() { return SubAppDirectory("Temp"); }
        public string WebView2ProfileDirectory() { return SubAppDirectory("WebView2"); }
        public string TranslatorDirectory() { return translationfolder; }
        public int TranslatorDirectoryIncludeSearchUpDepth { get; private set; }
        public bool SetEDDNforNewCommanders { get; private set; } = true;
        public string ShipLoadoutsDirectory() { return SubAppDirectory("Loadouts"); }
        static public string ExeDirectory() { return System.AppDomain.CurrentDomain.BaseDirectory;  }
        public string[] TranslatorFolders() { return new string[] { TranslatorDirectory(), ExeDirectory() }; }
        public string FrontierBindingsFolder { get; set; }
        
        #endregion

        #region Public interface
        public void ReRead()        // if you've changed the option file .opt then call reread
        {
            Init();
        }

        #endregion

        #region Implementation
        private EDDOptions()
        {
            Init();
        }

        private void Init()
        {
#if !DEBUG
            CheckGithubNotifications = CheckGithubAddOn = CheckGithubRelease = true;
#else
            EnableTGRightDebugClicks = true;
#endif

            // FIND all appfolder changes

            // 1. go thru the command line looking for -optionfile only and process them only for -appdata changes
            // allow rooted and relative to exe folder
            System.Diagnostics.Debug.WriteLine($"Options process commandline for appdata thru -optionsfile");
            ProcessCommandLineForOptionsFile(ExeDirectory(), true, false, ProcessAppFolderOptions);

            // 2. read the options*.txt files in the exe folder and allow app changes
            foreach (var exefile in Directory.EnumerateFiles(ExeDirectory(), "options*.txt", SearchOption.TopDirectoryOnly))
            {
                System.Diagnostics.Debug.WriteLine($"Options process file exe for apppath {exefile}");
                CommandArgs.ProcessFile(exefile, ProcessAppFolderOptions);
            }

            appfolderwarningmessage = "";        // Cancel any app folder message override from above

            // 3. process command line options for -appfolder
            System.Diagnostics.Debug.WriteLine($"Options process commandline for apppath");
            CommandArgs.ProcessCommandLine(ProcessAppFolderOptions);

            // 4. Set up AppDataDirectory

            if (AppFolder == null)                      // if user did not set it..
            {
                AppFolder = "EDDiscovery";
            }

            if (Path.IsPathRooted(AppFolder))           // if rooted, the dir is -appfolder
            {
                AppDataDirectory = AppFolder;
            }
            else if (PortableInstall)                   // if store in program folder, its exe/appfolder
            {
                AppDataDirectory = Path.Combine(ExeDirectory(), AppFolder);
            }
            else
            {
                AppDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppFolder);
            }

            System.Diagnostics.Debug.WriteLine($"Options ****** Set AppFolder to `{AppDataDirectory}` message `{appfolderwarningmessage}`");

            if (!Directory.Exists(AppDataDirectory))        // make sure its there, don't fail
                BaseUtils.FileHelpers.CreateDirectoryNoError(AppDataDirectory);

            // here because 1 user did not have a big enough C: drive to holds the SQL temp files!  This may be an over-engineer!
            if (TempDirInDataDir == true)
            {
                var tempdir = TempDirectory();
                Environment.SetEnvironmentVariable("TMP", tempdir);
                Environment.SetEnvironmentVariable("TEMP", tempdir);
            }

            translationfolder = Path.Combine(AppDataDirectory, "Translator");

            // 5. read the options*.txt files in the exe folder again and allow options
            foreach (var exefile in Directory.EnumerateFiles(ExeDirectory(), "options*.txt", SearchOption.TopDirectoryOnly))
            {
                System.Diagnostics.Debug.WriteLine($"Options process file for options {exefile}");
                CommandArgs.ProcessFile(exefile, ProcessOption);
            }

            // 6. with appdata set, go thru the command line looking for -optionfile rooted or relative to app folder and execute options

            System.Diagnostics.Debug.WriteLine($"Options process commandline for -optionsfile and execute options");
            ProcessCommandLineForOptionsFile(AppDataDirectory, false, true, ProcessOption);

            // 7. go thru appfolder looking for options*.txt and dboptions*.txt

            try  // protect.. check incase we could not create appdata, because the user gave us a bad location where we can't make directories in
            {
                // process appdata options files
                foreach (var appfile in Directory.EnumerateFiles(AppDataDirectory, "options*.txt", SearchOption.TopDirectoryOnly))
                {
                    System.Diagnostics.Debug.WriteLine($"Options process APP DATA file {appfile}");
                    CommandArgs.ProcessFile(appfile, ProcessOption);
                }

                // process appdata dboptions files
                foreach (var dbfile in Directory.EnumerateFiles(AppDataDirectory, "dboptions*.txt", SearchOption.TopDirectoryOnly))
                {
                    System.Diagnostics.Debug.WriteLine($"Options process DB file {dbfile}");
                    CommandArgs.ProcessFile(dbfile, ProcessOption);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"EDDOptions enumerate exception {ex}");
            }

            // 8. do all of the command line except optionsfile and appfolder..
            System.Diagnostics.Debug.WriteLine($"Options process commandline for options");
            CommandArgs.ProcessCommandLine(ProcessOption);

            // Finished

            // Set version display string
            {
                StringBuilder sb = new StringBuilder("Version " + EDDApplicationContext.AppVersion);

                if (!DisableShowDebugInfoInTitle)       // for when i'm doing help.. don't need this on
                {
                    sb.Append(appfolderwarningmessage);

                    if (DisableBetaCommanderCheck)
                    {
                        sb.Append(" (no BETA detect)");
                    }
                    if (ForceBetaOnCommander)
                    {
                        sb.Append(" (Force BETA)");
                    }
                }

                VersionDisplayString = sb.ToString();
            }

            // finally ensure we have a path for the dbs

            if (UserDatabasePath == null)
            {
                UserDatabasePath = Path.Combine(AppDataDirectory, UserDatabaseFilename);
            }
            if (SystemDatabasePath == null)
            {
                SystemDatabasePath = Path.Combine(AppDataDirectory, SystemDatabaseFilename);
            }
            if (ScanCachePath == null)
            {
                ScanCachePath = Path.Combine(AppDataDirectory, "WebScans");
            }

            FrontierBindingsFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Frontier Developments", "Elite Dangerous", "Options", "Bindings");

            EliteDangerousCore.EliteConfigInstance.InstanceOptions = this;
        }


        // go thru command line and find -optionsfile, and execute the options in it.
        private void ProcessCommandLineForOptionsFile(string basefolder, bool allowappfolderchange, bool allowoptions,
                                                            Action<string, BaseUtils.CommandArgs, bool> processoption)   
        {
            CommandArgs.ProcessCommandLine((optname, ca, _) =>
            {
                if (optname == "-optionsfile" && ca.More)
                {
                    string cmdlineoptfile = ca.Next();

                    bool ispathrooted = Path.IsPathRooted(cmdlineoptfile);

                    if (!File.Exists(cmdlineoptfile) && !ispathrooted)                      // if it does not exist on its own, may be relative to base folder ..
                        cmdlineoptfile = Path.Combine(basefolder, cmdlineoptfile);

                    if (File.Exists(cmdlineoptfile))
                    { 
                        if (allowappfolderchange)
                        {
                            System.Diagnostics.Debug.WriteLine($" Options command line options file process for appfolder {cmdlineoptfile}");
                            CommandArgs.ProcessFile(cmdlineoptfile, ProcessAppFolderOptions);  // give it a chance to set appfolder
                        }

                        if (allowoptions)
                        {
                            System.Diagnostics.Debug.WriteLine($" Options command line options file proccess for options {cmdlineoptfile}");
                            CommandArgs.ProcessFile(cmdlineoptfile, processoption);                       // execute all options
                        }
                    }
                    else
                        System.Diagnostics.Debug.WriteLine(" No Option File " + cmdlineoptfile);
                }
            });
        }

        // Process the app folder options..
        private void ProcessAppFolderOptions(string optname, CommandArgs ca, bool fileread)
        {
            if (optname == "-appfolder" && ca.More)
            {
                AppFolder = fileread ? ca.Rest() : ca.NextEmpty();
                appfolderwarningmessage = " (Using " + AppFolder + ")";
                System.Diagnostics.Debug.WriteLine($"  Option accepted:        {optname} to {AppFolder}");
            }
            else if (optname == "-portable")
            {
                PortableInstall = true;
                System.Diagnostics.Debug.WriteLine($"  Option accepted:        {optname}");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"  Option ignored:         {optname}");
            }
        }

        // standard process option (excepting -optionsfile, -appfolder, -portable)
        private void ProcessOption(string optname, BaseUtils.CommandArgs ca, bool fileread)
        {
            optname = optname.ToLowerInvariant();

            // not processed thru this function

            if (optname == "-optionsfile" || optname == "-appfolder" || optname == "-portable")
            {
                ca.Remove();   // waste it
                System.Diagnostics.Debug.WriteLine($"  Option ignored:        {optname}");
            }
            else
            {
                bool handled = true;
                string readdata = null;

                if (optname == "-translationfolder")
                {
                    readdata = translationfolder = ca.NextEmpty();
                    TranslatorDirectoryIncludeSearchUpDepth = ca.Int();
                }
                else if (optname == "-userdbpath")
                {
                    readdata = UserDatabasePath = fileread ? ca.Rest() : ca.NextEmpty();
                }
                else if (optname == "-userdbfilename")
                {
                    readdata = UserDatabaseFilename = fileread ? ca.Rest() : ca.NextEmpty();
                }
                else if (optname == "-cmdr" || optname == "-commander")
                {
                    readdata = Commander = fileread ? ca.Rest() : ca.NextEmpty();
                }
                else if (optname == "-profile")
                {
                    readdata = Profile = fileread ? ca.Rest() : ca.NextEmpty();
                }
                else if (optname == "-zmqport")
                {
                    readdata = fileread ? ca.Rest() : ca.NextEmpty();
                    ZMQPort = readdata.InvariantParseInt(12300);
                }
                else if (optname == "-systemsdbpath")
                {
                    readdata = SystemDatabasePath = fileread ? ca.Rest() : ca.NextEmpty();
                }
                else if (optname == "-scancachepath")
                {
                    readdata = ScanCachePath = fileread ? ca.Rest() : ca.NextEmpty();     // use DISABLED c/i
                }
                else if (optname == "-systemsdbfilename")
                {
                    readdata = SystemDatabaseFilename = fileread ? ca.Rest() : ca.NextEmpty();
                }
                else if (optname == "-iconspath")
                {
                    readdata = IconsPath = fileread ? ca.Rest() : ca.NextEmpty();
                }
                else if (optname == "-notificationfolder")
                {
                    readdata = NotificationFolderOverride = fileread ? ca.Rest() : ca.NextEmpty();
                }
                else if (optname == "-tracelog")
                {
                    readdata = TraceLog = fileread ? ca.Rest() : ca.NextEmpty();
                }
                else if (optname == "-fontsize")
                {
                    readdata = fileread ? ca.Rest() : ca.NextEmpty();
                    FontSize = readdata.InvariantParseFloat(0);
                }
                else if (optname == "-font")
                {
                    readdata = Font = fileread ? ca.Rest() : ca.NextEmpty();
                }
                else if (optname == "-language")
                {
                    readdata = SelectLanguage = fileread ? ca.Rest() : ca.NextEmpty();
                }
                else if (optname == "-webserverfolder" || optname == "-wsf")
                {
                    readdata = WebServerFolder = fileread ? ca.Rest() : ca.NextEmpty();
                }
                else if (optname == "-outputeventhelp")
                {
                    readdata = OutputEventHelp = fileread ? ca.Rest() : ca.NextEmpty();
                }
                else if (optname == "-defaultjournalfolder")
                {
                    readdata = DefaultJournalFolder = fileread ? ca.Rest() : ca.NextEmpty();
                }
                else if (optname == "-journalfilematch")
                {
                    readdata = DefaultJournalMatchFilename = fileread ? ca.Rest() : ca.NextEmpty();
                }
                else if (optname == "-culture")
                {
                    readdata = Culture = fileread ? ca.Rest() : ca.NextEmpty();
                }
                else if (optname == "-minjournaldateutc")
                {
                    readdata = fileread ? ca.Rest() : ca.NextEmpty();
                    MinJournalDateUTC = readdata.ParseDateTime(ObjectExtensionsDates.MinValueUTC(), System.Globalization.CultureInfo.CurrentCulture);
                }
                else if (optname == "-historyloaddaylimit")
                {
                    readdata = (fileread ? ca.Rest() : ca.NextEmpty());
                    if (DateTime.TryParse(readdata, out DateTime t))
                    {
                        var delta = DateTime.UtcNow - t;
                        HistoryLoadDayLimit = (int)((delta.TotalHours + 23.999) / 24);
                    }
                    else
                        HistoryLoadDayLimit = readdata.InvariantParseInt(0);
                }
                else if (optname == "-readto")
                {
                    readdata = fileread ? ca.Rest() : ca.NextEmpty();
                    MaxJournalDateUTC = readdata.ParseDateTime(ObjectExtensionsDates.MaxValueUTC(), System.Globalization.CultureInfo.CurrentCulture);
                }
                else if (optname.StartsWith("-"))
                {
                    switch (optname)
                    {
                        case "-journalsinglethread": MultithreadedJournalRead = false; break;
                        case "-safemode": SafeMode = true; break;
                        case "-norepositionwindow": NoWindowReposition = true; break;
                        case "-minimize": case "-minimise": MinimiseOnOpen = true; break;
                        case "-maximise": case "-maximize": MaximiseOnOpen = true; break;
                        case "-nrw": NoWindowReposition = true; break;
                        case "-showactionbutton": ActionButton = true; break;
                        case "-noload": NoLoad = true; break;
                        case "-nosystemsload": NoSystemsLoad = true; break;
                        case "-logexceptions": LogExceptions = true; break;
                        case "-nogithubpacks": DontAskGithubForPacks = true; break;
                        case "-checkrelease": CheckGithubRelease = true; break;
                        case "-checknotifications": CheckGithubNotifications = true; break;
                        case "-checkaddons": CheckGithubAddOn = true; break;
                        case "-nocheckrelease": CheckGithubRelease = false; break;
                        case "-nochecknotifications": CheckGithubNotifications = false; break;
                        case "-nocheckaddons": CheckGithubAddOn = false; break;
                        case "-disablemerge": DisableJournalMerge = true; break;
                        case "-disableremoval": DisableJournalRemoval = true; break;
                        case "-disablebetacheck":
                            DisableBetaCommanderCheck = true;
                            break;
                        case "-forcebeta":       // use to move logs to a beta commander for testing
                            ForceBetaOnCommander = true;
                            break;
                        case "-notheme": NoTheme = true; break;
                        case "-notabs": NoTabs = true; break;
                        case "-openalltabtypes": OpenAllTabTypes = true; break;
                        case "-tabsreset": TabsReset = true; break;
                        case "-nosound": NoSound = true; break;
                        case "-notitleinfo": DisableShowDebugInfoInTitle = true; break;
                        case "-resetlanguage": ResetLanguage = true; break;
                        case "-tempdirindatadir": TempDirInDataDir = true; break;
                        case "-notempdirindatadir": TempDirInDataDir = false; break;
                        case "-lowpriority": ProcessPriorityClass = System.Diagnostics.ProcessPriorityClass.BelowNormal; break;
                        case "-backgroundpriority": ProcessPriorityClass = System.Diagnostics.ProcessPriorityClass.Idle; break;
                        case "-highpriority": ProcessPriorityClass = System.Diagnostics.ProcessPriorityClass.High; break;
                        case "-abovenormalpriority": ProcessPriorityClass = System.Diagnostics.ProcessPriorityClass.AboveNormal; break;
                        case "-realtimepriority": ProcessPriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime; break;
                        case "-forcetls12": ForceTLS12 = true; break;
                        case "-disabletimedisplay": DisableTimeDisplay = true; break;
                        case "-disablecommanderselect": DisableCommanderSelect = true; break;
                        case "-disableversiondisplay": DisableVersionDisplay = true; break;
                        case "-enabletgrightclicks": EnableTGRightDebugClicks = true; break;
                        case "-autoloadnextcommander": AutoLoadNextCommander = true; break;
                        case "-null": break;     // null option - used by installer when it writes a app options file if it does not want to do anything
                        case "-deletesystemdb": DeleteSystemDB = true; break;
                        case "-deleteuserdb": DeleteUserDB = true; break;
                        case "-deleteuserjournals": DeleteUserJournals = true; break;
                        case "-keepsystemdownloadedfiles": KeepSystemDataDownloadedFiles = true; break;
                        case "-noeddnfornewcommanders": SetEDDNforNewCommanders = false; break;
                        default: handled = false; break;
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Unrecognized non option {optname}");
                    return;
                }

                if (handled)
                    System.Diagnostics.Debug.WriteLine($"  Option accepted:       {optname} {readdata}");
                else
                    System.Diagnostics.Debug.WriteLine($"  Option not recognised: {optname}");
            }
        }



        private static EDDOptions options = null;
        private string AppFolder;      // internal to use.. for -appfolder option
        private bool PortableInstall;  // internal to us, to indicate app folder is relative to exe not %localappdata%
        private string translationfolder; // internal to us

        private string appfolderwarningmessage = "";


#endregion

    }

}
