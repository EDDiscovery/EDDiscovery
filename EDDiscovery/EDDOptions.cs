/*
 * Copyright © 2018 EDDiscovery development team
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            else if (optname.StartsWith("-"))
            {
                string opt = optname.Substring(1);

                switch (opt)
                {
                    case "safemode": SafeMode = true; break;
                    case "norepositionwindow": NoWindowReposition = true; break;
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
                    case "lowpriority": LowPriority = true; break;
                    case "nolowpriority": LowPriority = false; break;
                    case "backgroundpriority": BackgroundPriority = true; break;
                    case "nobackgroundpriority": BackgroundPriority = false; break;
                    case "forcetls12": ForceTLS12 = true; break;
                    case "disabletimedisplay": DisableTimeDisplay = true; break;
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
        public string WebServerFolder { get; set; }             // normally empty, so selections zip server
        public bool LowPriority { get; set; }
        public bool BackgroundPriority { get; set; }
        public bool ForceTLS12 { get; set; }
        public bool DisableTimeDisplay { get; set; }
        public string OutputEventHelp { get; set; }

        public string SubAppDirectory(string subfolder)     // ensures its there.. name without \ slashes
        {
            string path = Path.Combine(AppDataDirectory, subfolder);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        public string ExploreAppDirectory() { return SubAppDirectory("Exploration"); }
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
        public string TranslatorDirectory() { return translationfolder; }
        public int TranslatorDirectoryIncludeSearchUpDepth { get; private set; }
        static public string ExeDirectory() { return System.AppDomain.CurrentDomain.BaseDirectory;  }
        public string[] TranslatorFolders() { return new string[] { TranslatorDirectory(), ExeDirectory() }; }

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
                Directory.CreateDirectory(AppDataDirectory);

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


        private void Init()
        {
#if !DEBUG
            CheckGithubFiles = true;
            CheckRelease = true;
#endif

            ProcessConfigVariables();

            ProcessCommandLineForOptionsFile(ExeDirectory(), ProcessOption);     // go thru the command line looking for -optionfile, use relative base dir

            string optval = Path.Combine(ExeDirectory(), "options.txt");      // options in the exe folder.
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

            optval = Path.Combine(AppDataDirectory, "options.txt");      // options in the base folder.
            if (File.Exists(optval))   // try options.txt in the base folder..
                ProcessOptionFile(optval, ProcessOption);

            // db move system option file will contain user and system db overrides
            optval = Path.Combine(AppDataDirectory, "dboptions.txt");   // look for this file in the app folder
            if (File.Exists(optval))
                ProcessOptionFile(optval, ProcessOption);

            ProcessCommandLineOptions(ProcessOption);       // do all of the command line except optionsfile and appfolder..

            SetVersionDisplayString();  // then set the version display string up dependent on options selected

            if (UserDatabasePath == null) UserDatabasePath = Path.Combine(AppDataDirectory, "EDDUser.sqlite");
            if (SystemDatabasePath == null) SystemDatabasePath = Path.Combine(AppDataDirectory, "EDDSystem.sqlite");

            EliteDangerousCore.EliteConfigInstance.InstanceOptions = this;
        }

    }

    #endregion
}
