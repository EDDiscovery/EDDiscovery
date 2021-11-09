/*
 * Copyright © 2018-2021 EDDiscovery development team
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

using EliteDangerousCore.EDSM;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace EDDiscovery
{
    public class EDDOptions : EliteDangerousCore.IEliteOptions
    {
        #region Option processing

        private void ProcessOption(string optname, BaseUtils.CommandArgs ca, bool toeol)
        {
            optname = optname.ToLowerInvariant();
            //System.Diagnostics.Debug.WriteLine("     Option " + optname);

            if (optname == "-optionsfile" || optname == "-appfolder")
            {
                ca.Remove();   // waste it
            }
            else if (optname == "-translationfolder")
            {
                translationfolder = ca.NextEmpty();
                TranslatorDirectoryIncludeSearchUpDepth = ca.Int();
            }
            else if (optname == "-userdbpath")
            {
                UserDatabasePath = toeol ? ca.Rest() : ca.NextEmpty();
            }
            else if (optname == "-cmdr" || optname == "-commander")
            {
                Commander = toeol ? ca.Rest() : ca.NextEmpty();
            }
            else if (optname == "-profile")
            {
                Profile = toeol ? ca.Rest() : ca.NextEmpty();
            }
            else if (optname == "-systemsdbpath")
            {
                SystemDatabasePath = toeol ? ca.Rest() : ca.NextEmpty();
            }
            else if (optname == "-iconspath")
            {
                IconsPath = toeol ? ca.Rest() : ca.NextEmpty();
            }
            else if (optname == "-notificationfolder")
            {
                NotificationFolderOverride = toeol ? ca.Rest() : ca.NextEmpty();
            }
            else if (optname == "-tracelog")
            {
                TraceLog = toeol ? ca.Rest() : ca.NextEmpty();
            }
            else if (optname == "-fontsize")
            {
                FontSize = (toeol ? ca.Rest() : ca.NextEmpty()).InvariantParseFloat(0);
            }
            else if (optname == "-font")
            {
                Font = toeol ? ca.Rest() : ca.NextEmpty();
            }
            else if (optname == "-language")
            {
                SelectLanguage = toeol ? ca.Rest() : ca.NextEmpty();
            }
            else if (optname == "-webserverfolder" || optname == "-wsf")
            {
                WebServerFolder = toeol ? ca.Rest() : ca.NextEmpty();
            }
            else if (optname == "-outputeventhelp")
            {
                OutputEventHelp = toeol ? ca.Rest() : ca.NextEmpty();
            }
            else if (optname == "-defaultjournalfolder")
            {
                DefaultJournalFolder = toeol ? ca.Rest() : ca.NextEmpty();
            }
            else if (optname == "-journalfilematch")
            {
                DefaultJournalMatchFilename = toeol ? ca.Rest() : ca.NextEmpty();
            }
            else if (optname == "-minjournaldateutc")
            {
                string s= toeol ? ca.Rest() : ca.NextEmpty();
                MinJournalDateUTC = s.ParseDateTime(DateTime.MinValue, System.Globalization.CultureInfo.CurrentCulture);
            }
            else if (optname == "-historyloaddaylimit")
            {
                string s = (toeol ? ca.Rest() : ca.NextEmpty());
                if (DateTime.TryParse(s, out DateTime t))
                {
                    var delta = DateTime.UtcNow - t;
                    HistoryLoadDayLimit = (int)((delta.TotalHours + 23.999) / 24);
                }
                else
                    HistoryLoadDayLimit = s.InvariantParseInt(0);
            }
            else if (optname.StartsWith("-"))
            {
                string opt = optname.Substring(1);

                switch (opt)
                {
                    case "safemode": SafeMode = true; break;
                    case "norepositionwindow": NoWindowReposition = true; break;
                    case "minimize": case "minimise": MinimiseOnOpen = true; break;
                    case "maximise": case "maximize": MaximiseOnOpen = true; break;
                    case "portable": StoreDataInProgramDirectory = true; break;
                    case "nrw": NoWindowReposition = true; break;
                    case "showactionbutton": ActionButton = true; break;
                    case "noload": NoLoad = true; break;
                    case "nosystems": NoSystemsLoad = true; break;
                    case "logexceptions": LogExceptions = true; break;
                    case "nogithubpacks": DontAskGithubForPacks = true; break;
                    case "checkrelease": CheckRelease = true; break;
                    case "checkgithub": CheckGithubFiles = true; break;
                    case "nocheckrelease": CheckRelease = false; break;
                    case "nocheckgithub": CheckGithubFiles = false; break;
                    case "disablemerge": DisableMerge = true; break;
                    case "edsmbeta":
                        EDSMClass.ServerAddress = "http://beta.edsm.net:8080/";
                        break;
                    case "edsmnull":
                        EDSMClass.ServerAddress = "";
                        break;
                    case "disablebetacheck":
                        DisableBetaCommanderCheck = true;
                        break;
                    case "forcebeta":       // use to move logs to a beta commander for testing
                        ForceBetaOnCommander = true;
                        break;
                    case "notheme": NoTheme = true; break;
                    case "notabs": NoTabs = true; break;
                    case "tabsreset": TabsReset = true; break;
                    case "nosound": NoSound = true; break;
                    case "no3dmap": No3DMap = true; break;
                    case "notitleinfo": DisableShowDebugInfoInTitle = true; break;
                    case "resetlanguage": ResetLanguage = true; break;
                    case "tempdirindatadir": TempDirInDataDir = true; break;
                    case "notempdirindatadir": TempDirInDataDir = false; break;
                    case "lowpriority": ProcessPriorityClass = System.Diagnostics.ProcessPriorityClass.BelowNormal; break;
                    case "backgroundpriority": ProcessPriorityClass = System.Diagnostics.ProcessPriorityClass.Idle; break;
                    case "highpriority": ProcessPriorityClass = System.Diagnostics.ProcessPriorityClass.High; break;
                    case "abovenormalpriority": ProcessPriorityClass = System.Diagnostics.ProcessPriorityClass.AboveNormal; break;
                    case "realtimepriority": ProcessPriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime; break;
                    case "forcetls12": ForceTLS12 = true; break;
                    case "disabletimedisplay": DisableTimeDisplay = true; break;
                    case "disableversiondisplay": DisableVersionDisplay = true; break;
                    case "enabletgrightclicks": EnableTGRightDebugClicks = true; break;
                    default:
                        System.Diagnostics.Debug.WriteLine($"Unrecognized option -{opt}");
                        break;
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Unrecognized non option {optname}");
            }
        }

        #endregion

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

        static EDDOptions options = null;

        public string VersionDisplayString { get; private set; }
        public string AppDataDirectory { get; private set; }
        public string UserDatabasePath { get; private set; }
        public string SystemDatabasePath { get; private set; }
        public string IconsPath { get; private set; }
        public bool NoWindowReposition { get; set; }
        public bool MinimiseOnOpen { get; set; }
        public bool MaximiseOnOpen { get; set; }
        public bool ActionButton { get; private set; }
        public bool NoLoad { get; private set; }
        public bool NoTheme { get; set; }
        public bool NoTabs { get; set; }
        public bool TabsReset { get; set; }
        public bool NoSystemsLoad { get; private set; }
        public bool NoSound { get; private set; }
        public bool No3DMap { get; private set; }
        public string TraceLog { get; private set; }        // null = auto file, or fixed name
        public bool LogExceptions { get; private set; }
        public bool DisableShowDebugInfoInTitle { get; private set; }
        public string OptionsFile { get; private set; }
        public bool DontAskGithubForPacks { get; private set; }
        public bool DisableBetaCommanderCheck { get; private set; }
        public bool ForceBetaOnCommander { get; private set; }
        public bool CheckRelease { get; private set; }
        public bool CheckGithubFiles { get; private set; }
        public bool ResetLanguage { get; set; }
        public string SelectLanguage { get; set; }
        public bool SafeMode { get; set; }
        public bool DisableMerge { get; set; }
        public string NotificationFolderOverride { get; set; }      // normally null..
        public float FontSize { get; set; }                           // override font size, 0 if not
        public string Font { get; set; }                           // override font, null if not
        public string Commander { get; set; }                   // set commander, null if not
        public string Profile { get; set; }                   // set profile, null if not
        public bool TempDirInDataDir { get; set; }
        public string WebServerFolder { get; set; }             // normally empty, so selects zip server
        public bool LowPriority { get; set; }
        public System.Diagnostics.ProcessPriorityClass ProcessPriorityClass { get; set; } = System.Diagnostics.ProcessPriorityClass.Normal;
        public bool ForceTLS12 { get; set; }
        public bool DisableTimeDisplay { get; set; }
        public bool DisableVersionDisplay { get; set; }
        public string OutputEventHelp { get; set; }
        public string DefaultJournalFolder { get; set; }        // default is null, use computed value
        public string DefaultJournalMatchFilename { get; set; }        // default is set
        public DateTime MinJournalDateUTC { get; set; }        // default is MinDate
        public bool EnableTGRightDebugClicks { get; set; }
        public int HistoryLoadDayLimit { get; set; }    // default zero not set

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
        public string VideosAppDirectory() { return SubAppDirectory("Videos"); }
        public string SoundsAppDirectory() { return SubAppDirectory("Sounds"); }
        public string IconsAppDirectory() { return SubAppDirectory("Icons"); }
        public string MapsAppDirectory() { return SubAppDirectory("Maps"); }
        public string LogAppDirectory() { return SubAppDirectory("Log"); }
        public string FlightsAppDirectory() { return SubAppDirectory("Flights"); }
        public string ThemeAppDirectory() { return SubAppDirectory("Theme"); }
        public string DLLAppDirectory() { return SubAppDirectory("DLL"); }
        public string DLLExeDirectory() { return SubExeDirectory("DLL"); }
        public string HelpDirectory() { return SubAppDirectory("Help"); }
        public string CAPIDirectory() { return SubAppDirectory("CAPI"); }
        public string TempMoveDirectory() { return SubAppDirectory("MoveFolder"); }
        public string TranslatorDirectory() { return translationfolder; }
        public int TranslatorDirectoryIncludeSearchUpDepth { get; private set; }
        static public string ExeDirectory() { return System.AppDomain.CurrentDomain.BaseDirectory;  }
        public string[] TranslatorFolders() { return new string[] { TranslatorDirectory(), ExeDirectory() }; }
        public string ExeOptionsFile() { return Path.Combine(ExeDirectory(), "options.txt"); }
        public string AppOptionsFile() { return Path.Combine(AppDataDirectory, "options.txt"); }
        public string DbOptionsFile() { return Path.Combine(AppDataDirectory, "dboptions.txt"); }

        private string AppFolder;      // internal to use.. for -appfolder option
        private bool StoreDataInProgramDirectory;  // internal to us, to indicate portable
        private string translationfolder; // internal to us

        #endregion

        #region Implementation

        private EDDOptions()
        {
            Init();
        }

        public void ReRead()        // if you've changed the option file .opt then call reread
        {
            Init();
        }

        private void SetAppDataDirectory()
        {
            ProcessCommandLineOptions((optname, ca, toeol) =>              //FIRST pass thru command line options looking
            {                                                           //JUST for -appfolder
                if (optname == "-appfolder" && ca.More)
                {
                    AppFolder = ca.Next();
                    System.Diagnostics.Debug.WriteLine("App Folder to " + AppFolder);
                }
            });

            string appfolder = AppFolder;

            if (appfolder == null)  // if userdid not set it..
            {
                appfolder = (StoreDataInProgramDirectory ? "Data" : "EDDiscovery");
            }

            if (Path.IsPathRooted(appfolder))
            {
                AppDataDirectory = appfolder;
            }
            else if (StoreDataInProgramDirectory)
            {
                AppDataDirectory = Path.Combine(ExeDirectory(), appfolder);
            }
            else
            {
                AppDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), appfolder);
            }

            if (!Directory.Exists(AppDataDirectory))        // make sure its there..
                BaseUtils.FileHelpers.CreateDirectoryNoError(AppDataDirectory);

            if (TempDirInDataDir == true)
            {
                var tempdir = Path.Combine(AppDataDirectory, "Temp");
                if (!Directory.Exists(tempdir))
                    Directory.CreateDirectory(tempdir);

                Environment.SetEnvironmentVariable("TMP", tempdir);
                Environment.SetEnvironmentVariable("TEMP", tempdir);
            }
        }

        private void SetVersionDisplayString()
        {
            StringBuilder sb = new StringBuilder("Version " + EDDApplicationContext.AppVersion);

            if (!DisableShowDebugInfoInTitle)       // for when i'm doing help.. don't need this on
            {
                if (AppFolder != null)
                {
                    sb.Append($" (Using {AppFolder})");
                }

                if (EDSMClass.ServerAddress.Length ==0)
                    sb.Append(" (EDSM No server)");
                else if (EDSMClass.ServerAddress.IndexOf("Beta",StringComparison.InvariantCultureIgnoreCase)!=-1)
                    sb.Append(" (EDSM Beta server)");

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

        private void ProcessConfigVariables()
        {
            var appsettings = System.Configuration.ConfigurationManager.AppSettings;

            if (appsettings["StoreDataInProgramDirectory"] == "true")
                StoreDataInProgramDirectory = true;

            UserDatabasePath = appsettings["UserDatabasePath"];
        }

        private void ProcessCommandLineForOptionsFile(string basefolder, Action<string, BaseUtils.CommandArgs, bool> getopt)     // command line -optionsfile
        {
            //System.Diagnostics.Debug.WriteLine("OptionFile -optionsfile ");
            ProcessCommandLineOptions((optname, ca, toeol) =>              //FIRST pass thru command line options looking
            {                                                           //JUST for -optionsfile
                if (optname == "-optionsfile" && ca.More)
                {
                    string filepath = ca.Next();

                    if (!File.Exists(filepath) && !Path.IsPathRooted(filepath))  // if it does not exist on its own, may be relative to base folder ..
                        filepath = Path.Combine(basefolder, filepath);

                    if (File.Exists(filepath))
                        ProcessOptionFile(filepath, getopt);
                    else
                        System.Diagnostics.Debug.WriteLine("    No Option File " + filepath);
                }
            });
        }

        private void ProcessOptionFile(string filepath, Action<string, BaseUtils.CommandArgs, bool> getopt)       // read file and process options
        {
            //System.Diagnostics.Debug.WriteLine("Read File " + filepath);
            foreach (string line in File.ReadAllLines(filepath))
            {
                if (!line.IsEmpty())
                {
                    //string[] cmds = line.Split(new char[] { ' ' }, 2).Select(s => s.Trim()).ToArray();    // old version..
                    string[] cmds = BaseUtils.StringParser.ParseWordList(line, separ: ' ').ToArray();
                    BaseUtils.CommandArgs ca = new BaseUtils.CommandArgs(cmds);
                    getopt("-" + ca.Next().ToLowerInvariant(), ca, true);
                }
            }
        }

        private void ProcessCommandLineOptions(Action<string, BaseUtils.CommandArgs, bool> getopt)       // go thru command line..
        {
            string[] cmdlineopts = Environment.GetCommandLineArgs().ToArray();
            BaseUtils.CommandArgs ca = new BaseUtils.CommandArgs(cmdlineopts,1);
            //System.Diagnostics.Debug.WriteLine("Command Line:");
            while (ca.More)
            {
                getopt(ca.Next().ToLowerInvariant(), ca, false);
            }
        }

        public void ResetSystemDatabasePath()
        {
            SystemDatabasePath = Path.Combine(AppDataDirectory, "EDDSystem.sqlite");
        }

        public void ResetUserDatabasePath()
        {
            UserDatabasePath = Path.Combine(AppDataDirectory, "EDDUser.sqlite");
        }

        private void Init()
        {
#if !DEBUG
            CheckGithubFiles = true;
            CheckRelease = true;
#else
            EnableTGRightDebugClicks = true;
#endif
            DefaultJournalMatchFilename = "Journal*.log";

            ProcessConfigVariables();

            ProcessCommandLineForOptionsFile(ExeDirectory(), ProcessOption);     // go thru the command line looking for -optionfile, use relative base dir

            string optval = ExeOptionsFile();      // options in the exe folder.
            if (File.Exists(optval))   // try options.txt in the base folder..
            {
                ProcessOptionFile(optval, (optname, ca, toeol) =>              //FIRST pass thru options.txt options looking
                {                                                           //JUST for -appfolder
                    if (optname == "-appfolder" && ca.More)
                    {
                        AppFolder = ca.Rest();
                        System.Diagnostics.Debug.WriteLine("App Folder to " + AppFolder);
                    }
                });
                ProcessOptionFile(optval, ProcessOption);
            }

            SetAppDataDirectory();      // set the app directory, now we have given base dir options to override appfolder, and we have given any -optionfiles on the command line

            translationfolder = Path.Combine(AppDataDirectory, "Translator");

            ProcessCommandLineForOptionsFile(AppDataDirectory, ProcessOption);     // go thru the command line looking for -optionfile relative to app folder, then read them

            optval = AppOptionsFile();      
            if (File.Exists(optval))   // try options.txt in the app folder
                ProcessOptionFile(optval, ProcessOption);

            // db move system option file will contain user and system db overrides
            optval = DbOptionsFile();   // look for this file in the app folder
            if (File.Exists(optval))
                ProcessOptionFile(optval, ProcessOption);

            ProcessCommandLineOptions(ProcessOption);       // do all of the command line except optionsfile and appfolder..

            SetVersionDisplayString();  // then set the version display string up dependent on options selected

            if (UserDatabasePath == null)
                ResetUserDatabasePath();
            if (SystemDatabasePath == null)
                ResetSystemDatabasePath();

            EliteDangerousCore.EliteConfigInstance.InstanceOptions = this;
        }

    }

    #endregion
}
