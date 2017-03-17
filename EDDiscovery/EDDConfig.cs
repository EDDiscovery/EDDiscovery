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
using EDDiscovery.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Reflection;
using EDDiscovery2.EDSM;

namespace EDDiscovery2
{
    public class EDDConfig
    {
        private static EDDConfig _instance;

        private EDDConfig()
        {
            LogIndex = DateTime.Now.ToString("yyyyMMdd");
        }

        public static EDDConfig Instance            // Singleton pattern
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EDDConfig();
                }
                return _instance;
            }
        }

        readonly public string LogIndex;            // fixed string

        #region Discrete Controls

        private bool _EDSMLog;
        private bool _canSkipSlowUpdates = false;
        private bool _useNotifyIcon = false;
        private bool _orderrowsinverted = false;
        private bool _minimizeToNotifyIcon = false;
        private bool _focusOnNewSystem = false; /**< Whether to automatically focus on a new system in the TravelHistory */
        private bool _keepOnTop = false; /**< Whether to keep the windows on top or not */
        private bool _displayUTC = false;
        private bool _clearMaterials = false;
        private bool _clearCommodities = false;
        private bool _autoLoadPopouts = false;
        private bool _autoSavePopouts = false;
        private string _defaultwavedevice = "Default";
        private string _defaultvoicedevice = "Default";

        public bool EDSMLog
        {
            get
            {
                return _EDSMLog;
            }

            set
            {
                _EDSMLog = value;
                SQLiteConnectionUser.PutSettingBool("EDSMLog", value);
            }
        }

        public bool CanSkipSlowUpdates
        {
            get
            {
                return EDDConfig.Options.Debug && _canSkipSlowUpdates;
            }
            set
            {
                _canSkipSlowUpdates = value;
                SQLiteConnectionUser.PutSettingBool("CanSkipSlowUpdates", value);
            }
        }

        /// <summary>
        /// Controls whether or not a system notification area (systray) icon will be shown.
        /// </summary>
        public bool UseNotifyIcon
        {
            get
            {
                return _useNotifyIcon;
            }
            set
            {
                _useNotifyIcon = value;
                SQLiteConnectionUser.PutSettingBool("UseNotifyIcon", value);
            }
        }

        public bool OrderRowsInverted
        {
            get
            {
                return _orderrowsinverted;
            }
            set
            {
                _orderrowsinverted = value;
                SQLiteConnectionUser.PutSettingBool("OrderRowsInverted", value);
            }
        }


        /// <summary>
        /// Controls whether or not the main window will be hidden to the
        /// system notification area icon (systray) when minimized.
        /// Has no effect if <see cref="UseNotifyIcon"/> is not enabled.
        /// </summary>
        public bool MinimizeToNotifyIcon
        {
            get
            {
                return _minimizeToNotifyIcon;
            }
            set
            {
                _minimizeToNotifyIcon = value;
                SQLiteConnectionUser.PutSettingBool("MinimizeToNotifyIcon", value);
            }
        }


        public bool FocusOnNewSystem
        {
            get
            {
                return _focusOnNewSystem;
            }
            set
            {
                _focusOnNewSystem = value;
                SQLiteConnectionUser.PutSettingBool("FocusOnNewSystem", value);
            }
        }

        public bool KeepOnTop
        {
            get
            {
                return _keepOnTop;
            }
            set
            {
                _keepOnTop = value;
                SQLiteConnectionUser.PutSettingBool("KeepOnTop", value);
            }
        }

        public bool DisplayUTC
        {
            get
            {
                return _displayUTC;
            }
            set
            {
                _displayUTC = value;
                SQLiteConnectionUser.PutSettingBool("DisplayUTC", value);
            }
        }

        public bool ClearMaterials
        {
            get
            {
                return _clearMaterials;
            }
            set
            {
                _clearMaterials = value;
                SQLiteConnectionUser.PutSettingBool("ClearMaterials", value);
            }
        }

        public bool ClearCommodities
        {
            get
            {
                return _clearCommodities;
            }
            set
            {
                _clearCommodities = value;
                SQLiteConnectionUser.PutSettingBool("ClearCommodities", value);
            }
        }

        public bool AutoLoadPopOuts
        {
            get
            {
                return _autoLoadPopouts;
            }
            set
            {
                _autoLoadPopouts = value;
                SQLiteConnectionUser.PutSettingBool("AutoLoadPopouts", value);
            }
        }

        public bool AutoSavePopOuts
        {
            get
            {
                return _autoSavePopouts;
            }
            set
            {
                _autoSavePopouts = value;
                SQLiteConnectionUser.PutSettingBool("AutoSavePopouts", value);
            }
        }

        public string DefaultWaveDevice
        {
            get
            {
                return _defaultwavedevice;
            }
            set
            {
                _defaultwavedevice = value;
                SQLiteConnectionUser.PutSettingString("WaveAudioDevice", value);
            }
        }

        public string DefaultVoiceDevice
        {
            get
            {
                return _defaultvoicedevice;
            }
            set
            {
                _defaultvoicedevice = value;
                SQLiteConnectionUser.PutSettingString("VoiceAudioDevice", value);
            }
        }

        #endregion

        #region Commanders

        private int currentCmdrID = 0;
        private List<EDCommander> _ListOfCommanders;

        public List<EDCommander> ListOfCommanders { get { if (_ListOfCommanders == null) Update(); return _ListOfCommanders; } }

        public EDCommander CurrentCommander
        {
            get
            {
                if (currentCmdrID >= ListOfCommanders.Count)
                    currentCmdrID = ListOfCommanders.Count - 1;

                return ListOfCommanders[currentCmdrID];
            }
        }

        public EDCommander Commander(int i)
        {
            return i < 0 ? null : ListOfCommanders.FirstOrDefault(c => c.Nr == i);
        }

        public int CurrentCmdrID
        {
            get
            {
                return CurrentCommander.Nr;
            }

            set
            {
                var cmdr = _ListOfCommanders.Select((c, i) => new { index = i, cmdr = c }).SingleOrDefault(a => a.cmdr.Nr == value);
                if (cmdr != null)
                {
                    currentCmdrID = cmdr.index;
                    SQLiteConnectionUser.PutSettingInt("ActiveCommander", value);
                }
            }
        }

        public bool CheckCommanderEDSMAPI
        {
            get
            {
                return CurrentCmdrID >= 0 && CurrentCommander.EdsmName.Length > 0 && CurrentCommander.APIKey.Length > 0;
            }
        }

        #endregion

        #region Update at start

        public void Update(bool write = true, SQLiteConnectionUser conn = null)     // call at start to populate above
        {
            try
            {
                _useNotifyIcon = SQLiteConnectionUser.GetSettingBool("UseNotifyIcon", false, conn);
                _EDSMLog = SQLiteConnectionUser.GetSettingBool("EDSMLog", false, conn);
                _canSkipSlowUpdates = SQLiteConnectionUser.GetSettingBool("CanSkipSlowUpdates", false, conn);
                _orderrowsinverted = SQLiteConnectionUser.GetSettingBool("OrderRowsInverted", false, conn);
                _minimizeToNotifyIcon = SQLiteConnectionUser.GetSettingBool("MinimizeToNotifyIcon", false, conn);
                _focusOnNewSystem = SQLiteConnectionUser.GetSettingBool("FocusOnNewSystem", false, conn);
                _keepOnTop = SQLiteConnectionUser.GetSettingBool("KeepOnTop", false, conn);
                _displayUTC = SQLiteConnectionUser.GetSettingBool("DisplayUTC", false, conn);
                _clearCommodities = SQLiteConnectionUser.GetSettingBool("ClearCommodities", false, conn);
                _clearMaterials = SQLiteConnectionUser.GetSettingBool("ClearMaterials", false, conn);
                _autoLoadPopouts = SQLiteConnectionUser.GetSettingBool("AutoLoadPopouts", false, conn);
                _autoSavePopouts = SQLiteConnectionUser.GetSettingBool("AutoSavePopouts", false, conn);
                _defaultvoicedevice = SQLiteConnectionUser.GetSettingString("VoiceAudioDevice", "Default", conn);
                _defaultwavedevice = SQLiteConnectionUser.GetSettingString("WaveAudioDevice", "Default", conn);

                LoadCommanders(write, conn);

                int activecommander = SQLiteConnectionUser.GetSettingInt("ActiveCommander", 0, conn);

                var cmdr = _ListOfCommanders.Select((c, i) => new { index = i, cmdr = c }).SingleOrDefault(a => a.cmdr.Nr == activecommander);

                if (cmdr != null)
                {
                    currentCmdrID = cmdr.index;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("EDDConfig.Update()" + ":" + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }

        }

        #endregion

        #region Mapping colours

        public int DefaultMapColour { get { return GetSettingInt("DefaultMap"); } set { PutSettingInt("DefaultMap", value); } }
        public MapColoursClass MapColours { get; private set; } = new EDDConfig.MapColoursClass();

        public class MapColoursClass
        {
            private System.Drawing.Color GetColour(string name)
            {
                return System.Drawing.Color.FromArgb(EDDConfig.Instance.GetSettingInt("MapColour_" + name));
            }

            private bool PutColour(string name, System.Drawing.Color colour)
            {
                return EDDConfig.Instance.PutSettingInt("MapColour_" + name, colour.ToArgb());
            }

            public System.Drawing.Color CoarseGridLines { get { return GetColour("CoarseGridLines"); } set { PutColour("CoarseGridLines", value); } }
            public System.Drawing.Color FineGridLines { get { return GetColour("FineGridLines"); } set { PutColour("FineGridLines", value); } }
            public System.Drawing.Color CentredSystem { get { return GetColour("CentredSystem"); } set { PutColour("CentredSystem", value); } }
            public System.Drawing.Color PlannedRoute { get { return GetColour("PlannedRoute"); } set { PutColour("PlannedRoute", value); } }
        }

        private Dictionary<string, object> settings = new Dictionary<string, object>();
        private Dictionary<string, Func<object>> defaults = new Dictionary<string, Func<object>>
        {
            { "MapColour_CoarseGridLines", () => System.Drawing.ColorTranslator.FromHtml("#296A6C").ToArgb() },
            { "MapColour_FineGridLines", () => System.Drawing.ColorTranslator.FromHtml("#202020").ToArgb() },
            { "MapColour_CentredSystem", () => System.Drawing.Color.Yellow.ToArgb() },
            { "MapColour_PlannedRoute", () => System.Drawing.Color.Green.ToArgb() },
            { "DefaultMap", () => System.Drawing.Color.Red.ToArgb() },
        };

        private int GetSettingInt(string key)
        {
            return GetSetting<int>(key, (k, d) => SQLiteConnectionUser.GetSettingInt(k, d));
        }

        private T GetSetting<T>(string key, Func<string, T, T> getter)
        {
            //System.Diagnostics.Debug.WriteLine("GetSetting " + key);
            if (!settings.ContainsKey(key))
            {
                T defval = default(T);
                if (defaults.ContainsKey(key))
                {
                    defval = (T)defaults[key]();
                }

                settings[key] = getter(key, defval);
            }

            return (T)settings[key];
        }

        private bool PutSettingInt(string key, int value)
        {
            return PutSetting<int>(key, value, (k, v) => SQLiteConnectionUser.PutSettingInt(k, v));
        }

        private bool PutSetting<T>(string key, T value, Func<string, T, bool> setter)
        {
            settings[key] = value;
            return setter(key, value);
        }


        #endregion

        #region Option control

        public enum EDSMServerType
        {
            Normal,
            Beta,
            Null
        }

        public class OptionsClass
        {
            public string VersionDisplayString { get; private set; }
            public string AppFolder { get; private set; }
            public string AppDataDirectory { get; private set; }
            public string UserDatabasePath { get; private set; }
            public string SystemDatabasePath { get; private set; }
            public string OldDatabasePath { get; private set; }
            public bool StoreDataInProgramDirectory { get; private set; }
            public bool NoWindowReposition { get; private set; }
            public bool Debug { get; private set; }
            public bool TraceLog { get; private set; }
            public bool LogExceptions { get; private set; }
            public EDSMServerType EDSMServerType { get; private set; } = EDSMServerType.Normal;
            public bool DisableBetaCheck { get; private set; }
            public string ReadJournal { get; private set; }
            public string OptionsFile { get; private set; }

            private void SetAppDataDirectory(string appfolder, bool portable)
            {
                if (appfolder == null)
                {
                    appfolder = (portable ? "Data" : "EDDiscovery");
                }

                if (Path.IsPathRooted(appfolder))
                {
                    AppDataDirectory = appfolder;
                }
                else if (portable)
                {
                    AppDataDirectory = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, appfolder);
                }
                else
                {
                    AppDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), appfolder);
                }

                if (!Directory.Exists(AppDataDirectory))
                    Directory.CreateDirectory(AppDataDirectory);
            }

            private void SetVersionDisplayString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Version ");
                sb.Append(Assembly.GetExecutingAssembly().FullName.Split(',')[1].Split('=')[1]);

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
                    EDDiscovery.EliteDangerous.EDJournalReader.disable_beta_commander_check = true;
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
                OptionsFile = "options.txt";

                ProcessCommandLineOptions((optname, optval) =>
                {
                    if (optname == "-optionsfile" && optval != null)
                    {
                        OptionsFile = optval;
                        return true;
                    }

                    return false;
                });

                if (!Path.IsPathRooted(OptionsFile))
                {
                    OptionsFile = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, OptionsFile);
                }

                if (File.Exists(OptionsFile))
                {
                    foreach (string line in File.ReadAllLines(OptionsFile))
                    {
                        string[] kvp = line.Split(new char[] { ' ' }, 2).Select(s => s.Trim()).ToArray();
                        ProcessCommandLineOption("-" + kvp[0], kvp.Length == 2 ? kvp[1] : null);
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
                else if (optname.StartsWith("-"))
                {
                    string opt = optname.Substring(1).ToLowerInvariant();
                    switch (opt)
                    {
                        case "norepositionwindow": NoWindowReposition = true; break;
                        case "portable": StoreDataInProgramDirectory = true; break;
                        case "nrw": NoWindowReposition = true; break;
                        case "debug": Debug = true; break;
                        case "tracelog": TraceLog = true; break;
                        case "logexceptions": LogExceptions = true; break;
                        case "edsmbeta": EDSMServerType = EDSMServerType.Beta; break;
                        case "edsmnull": EDSMServerType = EDSMServerType.Null; break;
                        case "disablebetacheck": DisableBetaCheck = true; break;
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

            public void Init(bool shift)
            {
                if (shift) NoWindowReposition = true;
                ProcessConfigVariables();
                ProcessOptionsFile();
                ProcessCommandLineOptions();
                SetAppDataDirectory(AppFolder, StoreDataInProgramDirectory);
                SetVersionDisplayString();
                if (UserDatabasePath == null) UserDatabasePath = Path.Combine(AppDataDirectory, "EDDUser.sqlite");
                if (SystemDatabasePath == null) SystemDatabasePath = Path.Combine(AppDataDirectory, "EDDSystem.sqlite");
                if (OldDatabasePath == null) OldDatabasePath = Path.Combine(AppDataDirectory, "EDDiscovery.sqlite");
            }
        }

        public static OptionsClass Options { get; } = new OptionsClass();

        #endregion

        #region Commander Functions

        private void LoadCommanders(bool write = true, SQLiteConnectionUser conn = null)
        {
            if ( _ListOfCommanders == null )
                _ListOfCommanders = new List<EDCommander>();

            lock (_ListOfCommanders)
            {
                _ListOfCommanders.Clear();

                bool migrate = false;

                var cmdrs = SQLiteConnectionUser.GetCommanders(conn);

                if (cmdrs.Count == 0)
                {
                    cmdrs = SQLiteConnectionUser.GetCommandersFromRegister(conn);
                    if (cmdrs.Count != 0)
                    {
                        migrate = true;
                    }
                }

                int maxnr = cmdrs.Count == 0 ? 0 : cmdrs.Max(c => c.Nr);

                _ListOfCommanders = cmdrs.Where(c => c.Deleted == false).ToList();

                if (_ListOfCommanders.Count == 0)
                {
                    if (write)
                    {
                        GetNewCommander("Jameson (Default)");
                    }
                    else
                    {
                        _ListOfCommanders = new List<EDCommander>
                        {
                            new EDCommander(maxnr + 1, "Jameson (Default)", "", false, false, false)
                        };
                    }
                }

                if (migrate && write)
                {
                    bool closeconn = false;
                    try
                    {
                        if (conn == null)
                        {
                            conn = new SQLiteConnectionUser();
                            closeconn = true;
                        }

                        using (DbCommand cmd = conn.CreateCommand("INSERT OR REPLACE INTO Commanders (Id, Name, EdsmName, EdsmApiKey, NetLogDir, Deleted, SyncToEdsm, SyncFromEdsm, SyncToEddn) VALUES (@Id, @Name, @EdsmName, @EdsmApiKey, @NetLogDir, @Deleted, @SyncToEdsm, @SyncFromEdsm, @SyncToEddn)"))
                        {
                            cmd.AddParameter("@Id", DbType.Int32);
                            cmd.AddParameter("@Name", DbType.String);
                            cmd.AddParameter("@EdsmName", DbType.String);
                            cmd.AddParameter("@EdsmApiKey", DbType.String);
                            cmd.AddParameter("@NetLogDir", DbType.String);
                            cmd.AddParameter("@Deleted", DbType.Boolean);
                            cmd.AddParameter("@SyncToEdsm", DbType.Boolean);
                            cmd.AddParameter("@SyncFromEdsm", DbType.Boolean);
                            cmd.AddParameter("@SyncToEddn", DbType.Boolean);

                            foreach (var cmdr in cmdrs)
                            {
                                cmd.Parameters["@Id"].Value = cmdr.Nr;
                                cmd.Parameters["@Name"].Value = cmdr.Name;
                                cmd.Parameters["@EdsmName"].Value = cmdr.EdsmName;
                                cmd.Parameters["@EdsmApiKey"].Value = cmdr.APIKey;
                                cmd.Parameters["@NetLogDir"].Value = cmdr.NetLogDir;
                                cmd.Parameters["@Deleted"].Value = cmdr.Deleted;
                                cmd.Parameters["@SyncToEdsm"].Value = cmdr.SyncToEdsm;
                                cmd.Parameters["@SyncFromEdsm"].Value = cmdr.SyncFromEdsm;
                                cmd.Parameters["@SyncToEddn"].Value = cmdr.SyncToEddn;

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    finally
                    {
                        if (closeconn && conn != null)
                        {
                            conn.Dispose();
                        }
                    }
                }
            }
        }

        public void UpdateCommanders(List<EDCommander> cmdrlist, bool reload)
        {
            using (SQLiteConnectionUser conn = new SQLiteConnectionUser())
            {
                using (DbCommand cmd = conn.CreateCommand("UPDATE Commanders SET Name=@Name, EdsmName=@EdsmName, EdsmApiKey=@EdsmApiKey, NetLogDir=@NetLogDir, JournalDir=@JournalDir, SyncToEdsm=@SyncToEdsm, SyncFromEdsm=@SyncFromEdsm, SyncToEddn=@SyncToEddn WHERE Id=@Id"))
                {
                    cmd.AddParameter("@Id", DbType.Int32);
                    cmd.AddParameter("@Name", DbType.String);
                    cmd.AddParameter("@EdsmName", DbType.String);
                    cmd.AddParameter("@EdsmApiKey", DbType.String);
                    cmd.AddParameter("@NetLogDir", DbType.String);
                    cmd.AddParameter("@JournalDir", DbType.String);
                    cmd.AddParameter("@SyncToEdsm", DbType.Boolean);
                    cmd.AddParameter("@SyncFromEdsm", DbType.Boolean);
                    cmd.AddParameter("@SyncToEddn", DbType.Boolean);

                    foreach (EDCommander edcmdr in cmdrlist)
                    {
                        cmd.Parameters["@Id"].Value = edcmdr.Nr;
                        cmd.Parameters["@Name"].Value = edcmdr.Name;
                        cmd.Parameters["@EdsmName"].Value = edcmdr.EdsmName;
                        cmd.Parameters["@EdsmApiKey"].Value = edcmdr.APIKey != null ? edcmdr.APIKey : "";
                        cmd.Parameters["@NetLogDir"].Value = edcmdr.NetLogDir != null ? edcmdr.NetLogDir : "";
                        cmd.Parameters["@JournalDir"].Value = edcmdr.JournalDir != null ? edcmdr.JournalDir : "";
                        cmd.Parameters["@SyncToEdsm"].Value = edcmdr.SyncToEdsm;
                        cmd.Parameters["@SyncFromEdsm"].Value = edcmdr.SyncFromEdsm;
                        cmd.Parameters["@SyncToEddn"].Value = edcmdr.SyncToEddn;
                        cmd.ExecuteNonQuery();
                    }

                    if (reload)
                        LoadCommanders();       // refresh in-memory copy
                }
            }
        }


        public EDCommander GetNewCommander(string name = null, string edsmName = null, string edsmApiKey = null, string journalpath = null)
        {
            EDCommander cmdr;

            using (SQLiteConnectionUser conn = new SQLiteConnectionUser())
            {
                using (DbCommand cmd = conn.CreateCommand("INSERT INTO Commanders (Name,EdsmName,EdsmApiKey,JournalDir,Deleted, SyncToEdsm, SyncFromEdsm, SyncToEddn) VALUES (@Name,@EdsmName,@EdsmApiKey,@JournalDir,@Deleted, @SyncToEdsm, @SyncFromEdsm, @SyncToEddn)"))
                {
                    cmd.AddParameterWithValue("@Name", name ?? "");
                    cmd.AddParameterWithValue("@EdsmName", edsmName ?? name ?? "");
                    cmd.AddParameterWithValue("@EdsmApiKey", edsmApiKey ?? "");
                    cmd.AddParameterWithValue("@JournalDir", journalpath ?? "");
                    cmd.AddParameterWithValue("@Deleted", false);
                    cmd.AddParameterWithValue("@SyncToEdsm", true);
                    cmd.AddParameterWithValue("@SyncFromEdsm", false);
                    cmd.AddParameterWithValue("@SyncToEddn", true);
                    cmd.ExecuteNonQuery();
                }

                using (DbCommand cmd = conn.CreateCommand("SELECT Id FROM Commanders WHERE rowid = last_insert_rowid()"))
                {
                    int nr = Convert.ToInt32(cmd.ExecuteScalar());
                }
                using (DbCommand cmd = conn.CreateCommand("SELECT * FROM Commanders WHERE rowid = last_insert_rowid()"))
                {
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        {
                            cmdr = new EDCommander(reader);
                        }
                    }
                }

                 if (name == null)
                {
                    using (DbCommand cmd = conn.CreateCommand("UPDATE Commanders SET Name = @Name WHERE rowid = last_insert_rowid()"))
                    {
                        cmd.AddParameterWithValue("@Name", cmdr.Name);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            LoadCommanders();       // refresh in-memory copy

            return cmdr;
        }

        public void DeleteCommander(EDCommander cmdr)
        {
            using (SQLiteConnectionUser conn = new SQLiteConnectionUser())
            {
                using (DbCommand cmd = conn.CreateCommand("UPDATE Commanders SET Deleted = 1 WHERE Id = @Id"))
                {
                    cmd.AddParameterWithValue("@Id", cmdr.Nr);
                    cmd.ExecuteNonQuery();
                }
            }

            LoadCommanders();       // refresh in-memory copy
        }

        #endregion
    }
}
