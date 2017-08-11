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
        public string AppFolder { get; private set; }
        public string AppDataDirectory { get; private set; }
        public string UserDatabasePath { get; private set; }
        public string SystemDatabasePath { get; private set; }
        public string OldDatabasePath { get; private set; }
        public string NewUserDatabasePath { get; private set; }
        public string NewSystemDatabasePath { get; private set; }
        public bool StoreDataInProgramDirectory { get; private set; }
        public bool NoWindowReposition { get; private set; }
        public bool ActionButton { get; private set; }
        public bool NoLoad { get; private set; }
        public bool NoTheme { get; private set; }
        public bool NoSystemsLoad { get; private set; }
        public bool TraceLog { get; private set; }
        public bool LogExceptions { get; private set; }
        public EDSMServerType EDSMServerType { get; private set; } = EDSMServerType.Normal;
        public bool DisableBetaCheck { get; private set; }
        public string ReadJournal { get; private set; }
        public string OptionsFile { get; private set; }

        private string GetAppDataDirectory(string appfolder, bool portable)
        {
            if (appfolder == null)
            {
                appfolder = (portable ? "Data" : "EDDiscovery");
            }

            if (Path.IsPathRooted(appfolder))
            {
                return appfolder;
            }
            else if (portable)
            {
                return Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, appfolder);
            }
            else
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), appfolder);
            }
        }

        private void SetAppDataDirectory(string appfolder, bool portable)
        {
            AppDataDirectory = GetAppDataDirectory(appfolder, portable);

            if (!Directory.Exists(AppDataDirectory))
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

            if (appsettings["StoreDataInProgramDirectory"] == "true") StoreDataInProgramDirectory = true;
            UserDatabasePath = appsettings["UserDatabasePath"];
        }

        private void ProcessOptionsFile()
        {
            string optionsFileName = "options.txt";
            bool useAppDataOptionsFile = false;

            ProcessCommandLineOptions((optname, optval) =>
            {
                if (optname == "-optionsfile" && optval != null)
                {
                    optionsFileName = optval;
                    return true;
                }

                return false;
            });

            OptionsFile = optionsFileName;

            if (!Path.IsPathRooted(OptionsFile))
            {
                OptionsFile = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, OptionsFile);
                useAppDataOptionsFile = true;
            }

            if (File.Exists(OptionsFile))
            {
                foreach (string line in File.ReadAllLines(OptionsFile))
                {
                    string[] kvp = line.Split(new char[] { ' ' }, 2).Select(s => s.Trim()).ToArray();
                    ProcessCommandLineOption("-" + kvp[0], kvp.Length == 2 ? kvp[1] : null);
                }
            }

            string appdatadir = GetAppDataDirectory(AppFolder, StoreDataInProgramDirectory);

            if (useAppDataOptionsFile && File.Exists(Path.Combine(appdatadir, optionsFileName)))
            {
                OptionsFile = Path.Combine(appdatadir, optionsFileName);
                foreach (string line in File.ReadAllLines(OptionsFile))
                {
                    string[] kvp = line.Split(new char[] { ' ' }, 2).Select(s => s.Trim()).ToArray();
                    ProcessCommandLineOption("-" + kvp[0], kvp.Length == 2 ? kvp[1] : null);
                }
            }
        }

        private bool TryReadLine(string filename, out string line)
        {
            line = null;

            if (File.Exists(filename))
            {
                try
                {
                    line = File.ReadLines(filename).FirstOrDefault();
                    return true;
                }
                catch
                {
                }
            }

            return false;
        }

        private void ProcessDbRedirectFiles()
        {
            string userdbpath = null;
            string userdbmove = null;
            string sysdbpath = null;
            string sysdbmove = null;

            if (UserDatabasePath == null)
            {
                if (TryReadLine(Path.Combine(AppDataDirectory, "EDDUser.sqlite.redirect.new"), out userdbmove))
                {
                    NewUserDatabasePath = userdbmove;
                }

                if (TryReadLine(Path.Combine(AppDataDirectory, "EDDUser.sqlite.redirect"), out userdbpath) && File.Exists(userdbpath))
                {
                    UserDatabasePath = userdbpath;
                }
            }

            if (SystemDatabasePath == null)
            {
                if (TryReadLine(Path.Combine(AppDataDirectory, "EDDSystem.sqlite.redirect.new"), out sysdbmove))
                {
                    NewSystemDatabasePath = sysdbmove;
                }

                if (TryReadLine(Path.Combine(AppDataDirectory, "EDDSystem.sqlite.redirect"), out sysdbpath) && File.Exists(sysdbpath))
                {
                    SystemDatabasePath = sysdbpath;
                }
            }
        }

        private void ProcessCommandLineOptions(Func<string, string, bool> getopt)
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

        private bool ProcessCommandLineOption(string optname, string optval)
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
            else if (optname == "-movesystemsdb")
            {
                NewSystemDatabasePath = optval;
                return true;
            }
            else if (optname == "-moveuserdb")
            {
                NewUserDatabasePath = optval;
                return true;
            }
            else if (optname.StartsWith("-"))
            {
                string opt = optname.Substring(1).ToLowerInvariant();
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

        private void ProcessCommandLineOptions()
        {
            ProcessCommandLineOptions(ProcessCommandLineOption);
        }

        public void Init(bool shift , bool ctrl)
        {
            if (shift)
                NoWindowReposition = true;

            if ( ctrl)
                NoTheme = true;
            
            ProcessConfigVariables();
            ProcessOptionsFile();
            ProcessCommandLineOptions();
            SetAppDataDirectory(AppFolder, StoreDataInProgramDirectory);
            SetVersionDisplayString();
            ProcessDbRedirectFiles();
            if (UserDatabasePath == null) UserDatabasePath = Path.Combine(AppDataDirectory, "EDDUser.sqlite");
            if (SystemDatabasePath == null) SystemDatabasePath = Path.Combine(AppDataDirectory, "EDDSystem.sqlite");
            if (OldDatabasePath == null) OldDatabasePath = Path.Combine(AppDataDirectory, "EDDiscovery.sqlite");

            EliteDangerousCore.EliteConfigInstance.InstanceOptions = this;
        }

        public bool MoveDatabases(Action<string> msg)
        {
            FileStream userdbstream = null;
            FileStream newuserdbstream = null;
            FileStream sysdbstream = null;
            FileStream newsysdbstream = null;

            try
            {
                if (NewUserDatabasePath != null)
                {
                    if (File.Exists(UserDatabasePath) && NewUserDatabasePath != UserDatabasePath)
                    {
                        FileStream instream = userdbstream = File.Open(UserDatabasePath, FileMode.Open, FileAccess.Read, FileShare.Delete);
                        FileStream outstream = newuserdbstream = File.Open(NewUserDatabasePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                        long size = instream.Length;
                        int blk = 0;
                        int len = 0;
                        byte[] data = new byte[65536];

                        while ((len = instream.Read(data, 0, data.Length)) != 0)
                        {
                            outstream.Write(data, 0, len);
                            blk++;
                            if ((blk % 16) == 0)
                            {
                                msg.Invoke($"Moving User DB ({outstream.Position / 1048576} / {size / 1048576}MiB)");
                            }
                        }
                    }
                }

                if (NewSystemDatabasePath != null)
                {
                    if (File.Exists(UserDatabasePath) && NewUserDatabasePath != UserDatabasePath)
                    {
                        FileStream instream = sysdbstream = File.Open(SystemDatabasePath, FileMode.Open, FileAccess.Read, FileShare.Delete);
                        FileStream outstream = newsysdbstream = File.Open(NewSystemDatabasePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                        long size = instream.Length;
                        int blk = 0;
                        int len = 0;
                        byte[] data = new byte[65536];

                        while ((len = instream.Read(data, 0, data.Length)) != 0)
                        {
                            outstream.Write(data, 0, len);
                            blk++;
                            if ((blk % 16) == 0)
                            {
                                msg.Invoke($"Moving System DB ({outstream.Position / 1048576} / {size / 1048576}MiB)");
                            }
                        }
                    }
                }

                if (userdbstream != null && newuserdbstream != null && userdbstream.Length == newuserdbstream.Length)
                {
                    File.WriteAllText(Path.Combine(AppDataDirectory, "EDDUser.sqlite.redirect"), NewUserDatabasePath);
                    File.Delete(Path.Combine(AppDataDirectory, "EDDUser.sqlite.redirect.new"));
                    File.Delete(UserDatabasePath);
                    UserDatabasePath = NewUserDatabasePath;
                    NewUserDatabasePath = null;
                }

                if (sysdbstream != null && newsysdbstream != null && sysdbstream.Length == newsysdbstream.Length)
                {
                    File.WriteAllText(Path.Combine(AppDataDirectory, "EDDSystem.sqlite.redirect"), NewSystemDatabasePath);
                    File.Delete(Path.Combine(AppDataDirectory, "EDDSystem.sqlite.redirect.new"));
                    File.Delete(SystemDatabasePath);
                    SystemDatabasePath = NewSystemDatabasePath;
                    NewSystemDatabasePath = null;
                }

                return true;
            }
            finally
            {
                if (newuserdbstream != null) newuserdbstream.Close();
                if (newsysdbstream != null) newsysdbstream.Close();
                if (userdbstream != null) userdbstream.Close();
                if (sysdbstream != null) sysdbstream.Close();
            }
        }
    }
}
