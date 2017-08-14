using EliteDangerousCore.EDSM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery
{
    public enum EDSMServerType
    {
        Normal,
        Beta,
        Null
    }

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
        public string OldDatabasePath { get; private set; }
        public bool NoWindowReposition { get;  set; }
        public bool ActionButton { get; private set; }
        public bool NoLoad { get; private set; }
        public bool NoTheme { get; set; }
        public bool NoSystemsLoad { get; private set; }
        public bool TraceLog { get; private set; }
        public bool LogExceptions { get; private set; }
        public EDSMServerType EDSMServerType { get; private set; } = EDSMServerType.Normal;
        public bool DisableBetaCheck { get; private set; }
        public string ReadJournal { get; private set; }
        public string OptionsFile { get; private set; }

        private string AppFolder { get; set; }      // internal to use.. for -appfolder option
        private bool StoreDataInProgramDirectory { get; set; }  // internal to us, to indicate portable

        private void SetAppDataDirectory()
        {
            if (AppFolder == null)  // if userdid not set it..
            {
                AppFolder = (StoreDataInProgramDirectory ? "Data" : "EDDiscovery");
            }

            if (Path.IsPathRooted(AppFolder))
            {
                AppDataDirectory = AppFolder;
            }
            else if (StoreDataInProgramDirectory)
            {
                AppDataDirectory = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, AppFolder);
            }
            else
            {
                AppDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppFolder);
            }

            if (!Directory.Exists(AppDataDirectory))        // make sure its there..
                Directory.CreateDirectory(AppDataDirectory);
        }

        private void SetVersionDisplayString()
        {
            StringBuilder sb = new StringBuilder("Version " + EDDApplicationContext.AppVersion);

            if (AppFolder != null)
            {
                sb.Append($" (Using {AppFolder})");
            }

            switch (EDSMServerType)
            {
                case EDSMServerType.Beta:
                    EDSMClass.ServerAddress = "http://beta.edsm.net:8080/";
                    sb.Append(" (EDSMBeta)");
                    break;
                case EDSMServerType.Null:
                    EDSMClass.ServerAddress = "";
                    sb.Append(" (EDSM No server)");
                    break;
            }

            if (DisableBetaCheck)
            {
                EliteDangerousCore.EDJournalReader.disable_beta_commander_check = true;
                sb.Append(" (no BETA detect)");
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

        private void ProcessOptionsFileOption()     // command line -optionsfile
        {
            ProcessCommandLineOptions((optname, optval) =>              //FIRST pass thru command line options looking
            {                                                           //JUST for -optionsfile
                if (optname == "-optionsfile" && optval != null)
                {
                    if (!File.Exists(optval))   // if it does not exist on its own, may be relative to base folder ..
                        optval = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, optval);

                    if (File.Exists(optval))
                        ProcessOptionFile(optval);

                    return true;    // used one
                }

                return false;
            });
        }

        private void ProcessOptionFile(string optval)       // read file and process options
        {
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

            if (optname == "-optionsfile")
            {
                // Skip option as it was handled in ProcessOptionsFile
                return true;
            }
            else if (optname == "-appfolder")
            {
                AppFolder = optval;
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
            else if (optname == "-olddbpath")
            {
                OldDatabasePath = optval;
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
                    case "edsmbeta": EDSMServerType = EDSMServerType.Beta; break;
                    case "edsmnull": EDSMServerType = EDSMServerType.Null; break;
                    case "disablebetacheck": DisableBetaCheck = true; break;
                    case "notheme": NoTheme = true; break;
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

            ProcessOptionsFileOption();     // go thru the command line looking for -optionfile, then read them

            ProcessCommandLineOptions(ProcessOption);       // do all of the command line

            string optval = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "options.txt");      // options in the base folder.
            if (File.Exists(optval))   // try options.txt in the base folder..
                ProcessOptionFile(optval);

            SetAppDataDirectory();      // set the app directory

            optval = Path.Combine(AppDataDirectory, "options.txt");      // options in the base folder.
            if (File.Exists(optval))   // try options.txt in the base folder..
                ProcessOptionFile(optval);

            // must be last, to override any previous options.. will contain user and system db overrides
            optval = Path.Combine(AppDataDirectory, "dboptions.txt");   // look for this file in the app folder
            if (File.Exists(optval))
                ProcessOptionFile(optval);

            SetVersionDisplayString();  // then set the version display string up dependent on options selected

            if (UserDatabasePath == null) UserDatabasePath = Path.Combine(AppDataDirectory, "EDDUser.sqlite");
            if (SystemDatabasePath == null) SystemDatabasePath = Path.Combine(AppDataDirectory, "EDDSystem.sqlite");
            if (OldDatabasePath == null) OldDatabasePath = Path.Combine(AppDataDirectory, "EDDiscovery.sqlite");

            EliteDangerousCore.EliteConfigInstance.InstanceOptions = this;
        }

    }
}
