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
 * EDDiscovery is not affiliated with Fronter Developments plc.
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
        public class MapColoursClass
        {
            public System.Drawing.Color GetColour(string name)
            {
                return System.Drawing.Color.FromArgb(EDDConfig.Instance.GetSettingInt("MapColour_" + name));
            }

            public bool PutColour(string name, System.Drawing.Color colour)
            {
                return EDDConfig.Instance.PutSettingInt("MapColour_" + name, colour.ToArgb());
            }

            public System.Drawing.Color CoarseGridLines { get { return GetColour("CoarseGridLines"); } set { PutColour("CoarseGridLines", value); } }
            public System.Drawing.Color FineGridLines { get { return GetColour("FineGridLines"); } set { PutColour("FineGridLines", value); } }
            public System.Drawing.Color SystemDefault { get { return GetColour("SystemDefault"); } set { PutColour("SystemDefault", value); } }
            public System.Drawing.Color StationSystem { get { return GetColour("StationSystem"); } set { PutColour("StationSystem", value); } }
            public System.Drawing.Color CentredSystem { get { return GetColour("CentredSystem"); } set { PutColour("CentredSystem", value); } }
            public System.Drawing.Color SelectedSystem { get { return GetColour("SelectedSystem"); } set { PutColour("SelectedSystem", value); } }
            public System.Drawing.Color POISystem { get { return GetColour("POISystem"); } set { PutColour("POISystem", value); } }
            public System.Drawing.Color TrilatCurrentReference { get { return GetColour("TrilatCurrentReference"); } set { PutColour("TrilatCurrentReference", value); } }
            public System.Drawing.Color TrilatSuggestedReference { get { return GetColour("TrilatSuggestedReference"); } set { PutColour("TrilatSuggestedReference", value); } }
            public System.Drawing.Color PlannedRoute { get { return GetColour("PlannedRoute"); } set { PutColour("PlannedRoute", value); } }
            public System.Drawing.Color NamedStar { get { return GetColour("NamedStar"); } set { PutColour("NamedStar", value); } }
            public System.Drawing.Color NamedStarUnpopulated { get { return GetColour("NamedStarUnpop"); } set { PutColour("NamedStarUnpop", value); } }
        }

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

            private void ProcessCommandLineOptions()
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

                    if (optname == "-appfolder")
                    {
                        AppFolder = optval;
                        i++;
                    }
                    else if (optname == "-readjournal")
                    {
                        ReadJournal = optval;
                        i++;
                    }
                    else if (optname == "-userdbpath")
                    {
                        UserDatabasePath = optval;
                        i++;
                    }
                    else if (optname == "-systemsdbpath")
                    {
                        SystemDatabasePath = optval;
                        i++;
                    }
                    else if (optname == "-olddbpath")
                    {
                        OldDatabasePath = optval;
                        i++;
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
                }
            }

            public void Init()
            {
                ProcessConfigVariables();
                ProcessCommandLineOptions();
                SetAppDataDirectory(AppFolder, StoreDataInProgramDirectory);
                SetVersionDisplayString();
                if (UserDatabasePath != null) UserDatabasePath = Path.Combine(AppDataDirectory, "EDDUser.sqlite");
                if (SystemDatabasePath != null) SystemDatabasePath = Path.Combine(AppDataDirectory, "EDDSystem.sqlite");
                if (OldDatabasePath != null) OldDatabasePath = Path.Combine(AppDataDirectory, "EDDiscovery.sqlite");
            }
        }

        public static OptionsClass Options { get; } = new OptionsClass();

        private static EDDConfig _instance;
        public static EDDConfig Instance
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

        private bool _useDistances;
        private bool _EDSMLog;
        readonly public string LogIndex;
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

        private List<EDCommander> _ListOfCommanders;
        public List<EDCommander> ListOfCommanders { get { if (_ListOfCommanders == null) Update();  return _ListOfCommanders;  } }
        
        private int currentCmdrID=0;
        private Dictionary<string, object> settings = new Dictionary<string, object>();
        private Dictionary<string, Func<object>> defaults = new Dictionary<string, Func<object>>
        {
            { "JournalDir", () => EDDiscovery.EliteDangerous.EDJournalClass.GetDefaultJournalDir() },
            { "JournalDirAutoMode", () => true },
            { "DefaultMap", () => System.Drawing.Color.Red.ToArgb() },
            { "MapColour_CoarseGridLines", () => System.Drawing.ColorTranslator.FromHtml("#296A6C").ToArgb() },
            { "MapColour_FineGridLines", () => System.Drawing.ColorTranslator.FromHtml("#202020").ToArgb() },
            { "MapColour_SystemDefault", () => System.Drawing.Color.White.ToArgb() },
            { "MapColour_StationSystem", () => System.Drawing.Color.RoyalBlue.ToArgb() },
            { "MapColour_CentredSystem", () => System.Drawing.Color.Yellow.ToArgb() },
            { "MapColour_SelectedSystem", () => System.Drawing.Color.Orange.ToArgb() },
            { "MapColour_POISystem", () => System.Drawing.Color.Purple.ToArgb() },
            { "MapColour_TrilatCurrentReference", () => System.Drawing.Color.Green.ToArgb() },
            { "MapColour_TrilatSuggestedReference", () => System.Drawing.Color.DarkOrange.ToArgb() },
            { "MapColour_PlannedRoute", () => System.Drawing.Color.Green.ToArgb() },
            { "MapColour_NamedStar", () => System.Drawing.Color.Yellow.ToArgb() },
            { "MapColour_NamedStarUnpop", () => System.Drawing.Color.FromArgb(255,192,192,0).ToArgb() }
        };

        private EDDConfig()
        {
            LogIndex = DateTime.Now.ToString("yyyyMMdd");
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

        public bool UseDistances
        {
            get
            {
                return _useDistances;
            }

            set
            {
                _useDistances = value;
                SQLiteConnectionUser.PutSettingBool("EDSMDistances", value);
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

        public EDCommander CurrentCommander
        {
            get
            {
                if (currentCmdrID >= ListOfCommanders.Count)
                    currentCmdrID = ListOfCommanders.Count - 1;

                return ListOfCommanders[currentCmdrID];
            }
        }

        public EDCommander Commander( int i )
        {
            return i < 0 ? null : ListOfCommanders.FirstOrDefault(c => c.Nr == i);
        }

        public bool CheckCommanderEDSMAPI
        {
            get
            {
                return CurrentCmdrID >= 0 && CurrentCommander.EdsmName.Length > 0 && CurrentCommander.APIKey.Length > 0;
            }
        }

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
                return _canSkipSlowUpdates;
            }
            set
            {
                _canSkipSlowUpdates = value;
                SQLiteConnectionUser.PutSettingBool("CanSkipSlowUpdates", value);
            }
        }

        public bool OrderRowsInverted {
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

        public bool FocusOnNewSystem {
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

        public int DefaultMapColour { get { return GetSettingInt("DefaultMap"); } set { PutSettingInt("DefaultMap", value); } }
        public MapColoursClass MapColours { get; private set; } = new EDDConfig.MapColoursClass();

        private bool GetSettingBool(string key)
        {
            return GetSetting<bool>(key, (k, d) => SQLiteConnectionUser.GetSettingBool(k, d));
        }

        private int GetSettingInt(string key)
        {
            return GetSetting<int>(key, (k, d) => SQLiteConnectionUser.GetSettingInt(k, d));
        }

        private double GetSettingDouble(string key)
        {
            return GetSetting<double>(key, (k, d) => SQLiteConnectionUser.GetSettingDouble(k, d));
        }

        private string GetSettingString(string key)
        {
            return GetSetting<string>(key, (k, d) => SQLiteConnectionUser.GetSettingString(k, d));
        }

        private T GetSetting<T>(string key, Func<string,T,T> getter)
        {
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

        private bool PutSettingBool(string key, bool value)
        {
            return PutSetting<bool>(key, value, (k, v) => SQLiteConnectionUser.PutSettingBool(k, v));
        }

        private bool PutSettingInt(string key, int value)
        {
            return PutSetting<int>(key, value, (k, v) => SQLiteConnectionUser.PutSettingInt(k, v));
        }

        private bool PutSettingDouble(string key, double value)
        {
            return PutSetting<double>(key, value, (k, v) => SQLiteConnectionUser.PutSettingDouble(k, v));
        }

        private bool PutSettingString(string key, string value)
        {
            return PutSetting<string>(key, value, (k, v) => SQLiteConnectionUser.PutSettingString(k, v));
        }

        private bool PutSetting<T>(string key, T value, Func<string,T,bool> setter)
        {
            settings[key] = value;

            if (setter(key, value))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Update(bool write = true, SQLiteConnectionUser conn = null)
        {
            try
            {
                _useNotifyIcon = SQLiteConnectionUser.GetSettingBool("UseNotifyIcon", false, conn);
                _useDistances = SQLiteConnectionUser.GetSettingBool("EDSMDistances", false, conn);
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
    }
}
