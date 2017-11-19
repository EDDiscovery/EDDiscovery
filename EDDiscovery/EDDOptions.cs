using EliteDangerousCore.EDSM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery
{
    public class EDDOptions : EliteDangerousCore.EliteOptions
    {
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
        public bool NoWindowReposition { get;  set; }
        public bool ActionButton { get; private set; }
        public bool NoLoad { get; private set; }
        public bool NoTheme { get; set; }
        public bool NoSystemsLoad { get; private set; }
        public bool NoSound { get; private set; }
        public bool No3DMap { get; private set; }
        public bool TraceLog { get; private set; }
        public bool LogExceptions { get; private set; }
        public bool DisableShowDebugInfoInTitle { get; private set; }
        public string ReadJournal { get; private set; }
        public string OptionsFile { get; private set; }
        public bool DontAskGithubForPacks { get; private set; }

        private string AppFolder { get; set; }      // internal to use.. for -appfolder option
        private bool StoreDataInProgramDirectory { get; set; }  // internal to us, to indicate portable

        private void SetAppDataDirectory()
        {
            ProcessCommandLineOptions((optname, optval) =>              //FIRST pass thru command line options looking
            {                                                           //JUST for -appfolder
                if (optname == "-appfolder" && optval != null)
                {
                    AppFolder = optval;

                    return true;    // used one
                }

                return false;
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
                AppDataDirectory = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, appfolder);
            }
            else
            {
                AppDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), appfolder);
            }

            if (!Directory.Exists(AppDataDirectory))        // make sure its there..
                Directory.CreateDirectory(AppDataDirectory);
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

                if (EliteDangerousCore.EDJournalReader.disable_beta_commander_check)
                {
                    sb.Append(" (no BETA detect)");
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

        private void ProcessOptionsFileOption(string basefolder)     // command line -optionsfile
        {
            //System.Diagnostics.Debug.WriteLine("OptionFile -optionsfile ");
            ProcessCommandLineOptions((optname, optval) =>              //FIRST pass thru command line options looking
            {                                                           //JUST for -optionsfile
                if (optname == "-optionsfile" && optval != null)
                {
                    if (!File.Exists(optval) && !Path.IsPathRooted(optval))  // if it does not exist on its own, may be relative to base folder ..
                        optval = Path.Combine(basefolder, optval);

                    if (File.Exists(optval))
                        ProcessOptionFile(optval);

                    return true;    // used one
                }

                return false;
            });
        }

        private void ProcessOptionFile(string optval)       // read file and process options
        {
            //System.Diagnostics.Debug.WriteLine("OptionFile " + optval);
            foreach (string line in File.ReadAllLines(optval))
            {
                string[] kvp = line.Split(new char[] { ' ' }, 2).Select(s => s.Trim()).ToArray();
                ProcessOption("-" + kvp[0], kvp.Length == 2 ? kvp[1] : null);
            }
        }

        private void ProcessCommandLineOptions(Func<string, string, bool> getopt)       // go thru command line..
        {
            string[] cmdlineopts = Environment.GetCommandLineArgs().ToArray();

            int i = 0;

            while (i < cmdlineopts.Length)
            {
                string optname = cmdlineopts[i].ToLowerInvariant();
                string optval = null;
                if (i < cmdlineopts.Length - 1)
                {
                    optval = cmdlineopts[i + 1];
                }

                if (getopt(optname, optval))
                {
                    i++;
                }
                i++;
            }
        }

        private bool ProcessOption(string optname, string optval)
        {
            optname = optname.ToLowerInvariant();
            //System.Diagnostics.Debug.WriteLine("     Option " + optname);

            if (optname == "-optionsfile")
            {
                // Skip option as it was handled in ProcessOptionsFile
                return true;
            }
            else if (optname == "-appfolder")
            {
                if (AppDataDirectory == null) // Only override AppFolder if AppDataDirectory has not been set
                {
                    AppFolder = optval;
                }
                return true;
            }
            else if (optname == "-readjournal")
            {
                ReadJournal = optval;
                return true;
            }
            else if (optname == "-userdbpath")
            {
                UserDatabasePath = optval;
                return true;
            }
            else if (optname == "-systemsdbpath")
            {
                SystemDatabasePath = optval;
                return true;
            }
            else if (optname.StartsWith("-"))
            {
                string opt = optname.Substring(1);

                switch (opt)
                {
                    case "norepositionwindow": NoWindowReposition = true; break;
                    case "portable": StoreDataInProgramDirectory = true; break;
                    case "nrw": NoWindowReposition = true; break;
                    case "showactionbutton": ActionButton = true; break;
                    case "noload": NoLoad = true; break;
                    case "nosystems": NoSystemsLoad = true; break;
                    case "tracelog": TraceLog = true; break;
                    case "logexceptions": LogExceptions = true; break;
                    case "nogithubpacks": DontAskGithubForPacks = true; break;
                    case "edsmbeta":
                        EDSMClass.ServerAddress = "http://beta.edsm.net:8080/";
                        break;
                    case "edsmnull":
                        EDSMClass.ServerAddress = "";
                        break;
                    case "disablebetacheck":
                        EliteDangerousCore.EDJournalReader.disable_beta_commander_check = true;
                        break;
                    case "notheme": NoTheme = true; break;
                    case "nosound": NoSound = true; break;
                    case "no3dmap": No3DMap = true; break;
                    case "notitleinfo": DisableShowDebugInfoInTitle = true; break;
                    default:
                        Console.WriteLine($"Unrecognized option -{opt}");
                        break;
                }
            }
            else
            {
                Console.WriteLine($"Unexpected non-option {optname}");
            }
            return false;
        }

        public void Init()
        {
            ProcessConfigVariables();

            ProcessOptionsFileOption(System.AppDomain.CurrentDomain.BaseDirectory);     // go thru the command line looking for -optionfile, use relative base dir

            string optval = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "options.txt");      // options in the base folder.
            if (File.Exists(optval))   // try options.txt in the base folder..
                ProcessOptionFile(optval);

            SetAppDataDirectory();      // set the app directory, now we have given base dir options to override appfolder, and we have given any -optionfiles on the command line

            ProcessOptionsFileOption(AppDataDirectory);     // go thru the command line looking for -optionfile relative to app folder, then read them

            optval = Path.Combine(AppDataDirectory, "options.txt");      // options in the base folder.
            if (File.Exists(optval))   // try options.txt in the base folder..
                ProcessOptionFile(optval);

            // db move system option file will contain user and system db overrides
            optval = Path.Combine(AppDataDirectory, "dboptions.txt");   // look for this file in the app folder
            if (File.Exists(optval))
                ProcessOptionFile(optval);

            ProcessCommandLineOptions(ProcessOption);       // do all of the command line except optionsfile and appfolder..

            SetVersionDisplayString();  // then set the version display string up dependent on options selected

            if (UserDatabasePath == null) UserDatabasePath = Path.Combine(AppDataDirectory, "EDDUser.sqlite");
            if (SystemDatabasePath == null) SystemDatabasePath = Path.Combine(AppDataDirectory, "EDDSystem.sqlite");

            EliteDangerousCore.EliteConfigInstance.InstanceOptions = this;
        }

    }
}
