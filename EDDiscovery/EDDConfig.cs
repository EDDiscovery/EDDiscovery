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
using System.Drawing;
using System.IO;
using System.Reflection;
using EDDiscovery2.EDSM;

namespace EDDiscovery2
{
    public class EDDConfig
    {
        #region Public interface

        #region Events

        /// <summary>
        /// A configuration changed event.
        /// </summary>
        public class ConfigChangedEventArgs : EventArgs
        {
            /// <summary>
            /// The configuration property that was changed.
            /// </summary>
            public ConfigProperty Setting { get; protected set; }

            /// <summary>
            /// The new value for this property.
            /// </summary>
            public bool NewValue { get; protected set; }

            /// <summary>
            /// Constructs a new ConfigChangedEventArgs class in preparation to send it off in an event.
            /// </summary>
            /// <param name="setting">The configuration property that was changed.</param>
            /// <param name="newvalue">The new value of the configuration property.</param>
            public ConfigChangedEventArgs(ConfigProperty setting, bool newvalue)
            {
                Setting = setting;
                NewValue = newvalue;
            }
        }

        /// <summary>
        /// A map colour changed event.
        /// </summary>
        public class MapColourChangedEventArgs : EventArgs
        {
            /// <summary>
            /// The new colour for this property.
            /// </summary>
            public Color NewColour { get; protected set; }

            /// <summary>
            /// The map colour property that was changed.
            /// </summary>
            public MapColourProperty ColourProp { get; protected set; }

            /// <summary>
            /// A map colour changed event.
            /// </summary>
            /// <param name="prop">The <see cref="MapColourProperty"/> that was changed.</param>
            /// <param name="newColour">The new colour of this property.</param>
            public MapColourChangedEventArgs(MapColourProperty prop, Color newColour)
            {
                ColourProp = prop;
                NewColour = newColour;
            }
        }

        /// <summary>
        /// The configuration changed event handler.
        /// </summary>
        public event EventHandler<ConfigChangedEventArgs> ConfigChanged;

        /// <summary>
        /// The current commander changed event handler.
        /// </summary>
        public event EventHandler<CurrentCommanderChangedEventArgs> CurrentCommanderChanged;

        /// <summary>
        /// The map colour changed event handler.
        /// </summary>
        public event EventHandler<MapColourChangedEventArgs> MapColourChanged;

        #endregion

        #region Enums, subclasses

        /// <summary>
        /// The configuration property that has been changed.
        /// </summary>
        public enum ConfigProperty
        {
            AutoLoadPopOuts,
            AutoSavePopOuts,
            CanSkipSlowUpdates,
            CheckCommanderEDSMAPI,
            ClearCommodities,
            ClearMaterials,
            DisplayUTC,
            EDSMLog,
            FocusOnNewSystem,
            KeepOnTop,
            ListOfCommanders,
            MinimizeToNotifyIcon,
            OrderRowsInverted,
            UseNotifyIcon,
        };

        /// <summary>
        /// The map colour property that has been changed.
        /// </summary>
        public enum MapColourProperty
        {
            CentredSystem,
            CoarseGridLines,
            DefaultMapColour,
            FineGridLines,
            NamedStar,
            NamedStarUnpopulated,
            POISystem,
            SelectedSystem,
            TrilatCurrentReference,
            TrilatSuggestedReference,
            PlannedRoute,
            StationSystem,
            SystemDefault,
        };

        /// <summary>
        /// Whether EDSM connections shall be handled through the normal API server(s),
        /// or the beta server(s), or not allowed at all.
        /// </summary>
        public enum EDSMServerType
        {
            Normal,
            Beta,
            Null
        }

        /// <summary>
        /// Colours to be used for mapping.
        /// </summary>
        public class MapColoursClass
        {
            public Color GetColour(string name, Color defaultColour)
            {
                return Color.FromArgb(SQLiteConnectionUser.GetSettingInt("MapColour_" + name, defaultColour.ToArgb()));
            }

            public Color GetColour(string name, string defaultColour)
            {
                return GetColour(name, ColorTranslator.FromHtml(defaultColour));
            }

            public bool PutColour(string name, Color colour)
            {
                return SQLiteConnectionUser.PutSettingInt("MapColour_" + name, colour.ToArgb());
            }

            public Color CentredSystem { get { return GetColour("CentredSystem", Color.Yellow); } set { PutColour("CentredSystem", value); } }
            public Color CoarseGridLines { get { return GetColour("CoarseGridLines", "#296A6C"); } set { PutColour("CoarseGridLines", value); } }
            public Color FineGridLines { get { return GetColour("FineGridLines", "#202020"); } set { PutColour("FineGridLines", value); } }
            public Color NamedStar { get { return GetColour("NamedStar", Color.Yellow); } set { PutColour("NamedStar", value); } }
            public Color NamedStarUnpopulated { get { return GetColour("NamedStarUnpop", "#C0C000"); } set { PutColour("NamedStarUnpop", value); } }
            public Color POISystem { get { return GetColour("POISystem", Color.Purple); } set { PutColour("POISystem", value); } }
            public Color SelectedSystem { get { return GetColour("SelectedSystem", Color.Orange); } set { PutColour("SelectedSystem", value); } }
            public Color TrilatCurrentReference { get { return GetColour("TrilatCurrentReference", Color.Green); } set { PutColour("TrilatCurrentReference", value); } }
            public Color TrilatSuggestedReference { get { return GetColour("TrilatSuggestedReference", Color.DarkOrange); } set { PutColour("TrilatSuggestedReference", value); } }
            public Color PlannedRoute { get { return GetColour("PlannedRoute", Color.Green); } set { PutColour("PlannedRoute", value); } }
            public Color StationSystem { get { return GetColour("StationSystem", Color.RoyalBlue); } set { PutColour("StationSystem", value); } }
            public Color SystemDefault { get { return GetColour("SystemDefault", Color.White); } set { PutColour("SystemDefault", value); } }
        }

        /// <summary>
        /// Class representing command-line options, and other settings that cannot be changed during runtime.
        /// </summary>
        public class OptionsClass
        {
            #region Public properties

            public string AppDataDirectory { get; private set; }
            public string AppFolder { get; private set; }
            public bool Debug { get; private set; }
            public bool DisableBetaCheck { get; private set; }
            public EDSMServerType EDSMServerType { get; private set; } = EDSMServerType.Normal;
            public bool LogExceptions { get; private set; }
            public bool NoWindowReposition { get; private set; }
            public string OldDatabasePath { get; private set; }
            public string OptionsFile { get; private set; }
            public string ReadJournal { get; private set; }
            public bool StoreDataInProgramDirectory { get; private set; }
            public string SystemDatabasePath { get; private set; }
            public bool TraceLog { get; private set; }
            public string UserDatabasePath { get; private set; }
            public string VersionDisplayString { get; private set; }

            #endregion

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

            #region Private implementation

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

            #endregion
        }

        #endregion

        #region Fields

        /// <summary>
        /// Set during program startup to the current date. Used solely to determine
        /// the log file that this application instance wiill write to.
        /// </summary>
        readonly public static string LogIndex = DateTime.Now.ToString("yyyyMMdd");

        #endregion

        #region Static properties

        /// <summary>
        /// Command-line options, and other settings that cannot be changed during runtime.
        /// </summary>
        public static OptionsClass Options { get; } = new OptionsClass();

        /// <summary>
        /// Return the true instantiated EDDConfig, constructing it if necessary. Use this to access
        /// all settings that are not established beyond the scope of the application lifecycle.
        /// </summary>
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

        #endregion

        /* **** Use 'EDDConfig.Instance.' or (an instance of)EDDiscoveryForm'.EDDConfig.' to access these directly **** */
        #region Instantiated properties 

        /// <summary>
        /// Whether we should automatically load popout (S-Panel) windows at startup.
        /// </summary>
        public bool AutoLoadPopOuts
        {
            get
            {
                return _autoLoadPopouts;
            }
            set
            {
                if (_autoLoadPopouts != value)
                {
                    _autoLoadPopouts = value;
                    SQLiteConnectionUser.PutSettingBool("AutoLoadPopouts", value);
                    OnConfigChangedEvent(ConfigProperty.AutoLoadPopOuts, value);
                }
            }
        }

        /// <summary>
        /// Whether we should automatically save popout (S-Panel) windows at exit.
        /// </summary>
        public bool AutoSavePopOuts
        {
            get
            {
                return _autoSavePopouts;
            }
            set
            {
                if (_autoSavePopouts != value)
                {
                    _autoSavePopouts = value;
                    SQLiteConnectionUser.PutSettingBool("AutoSavePopouts", value);
                    OnConfigChangedEvent(ConfigProperty.AutoSavePopOuts, value);
                }
            }
        }

        /// <summary>
        /// (DEBUG ONLY) Whether to skip slow updates at startup. 
        /// </summary>
        public bool CanSkipSlowUpdates
        {
            get
            {
                return EDDConfig.Options.Debug && _canSkipSlowUpdates;
            }
            set
            {
                if (_canSkipSlowUpdates != value)
                {
                    _canSkipSlowUpdates = value;
                    SQLiteConnectionUser.PutSettingBool("CanSkipSlowUpdates", value);
                    OnConfigChangedEvent(ConfigProperty.CanSkipSlowUpdates, value);
                }
            }
        }

        /// <summary>
        /// Whether the commodities viewer should hide commodities that the commander does not have on-hand.
        /// </summary>
        public bool ClearCommodities
        {
            get
            {
                return _clearCommodities;
            }
            set
            {
                if (_clearCommodities != value)
                {
                    _clearCommodities = value;
                    SQLiteConnectionUser.PutSettingBool("ClearCommodities", value);
                    OnConfigChangedEvent(ConfigProperty.ClearCommodities, value);
                }
            }
        }

        /// <summary>
        /// Whether the materials viewer should hide materials that the command does not have on-hand.
        /// </summary>
        public bool ClearMaterials
        {
            get
            {
                return _clearMaterials;
            }
            set
            {
                if (_clearMaterials != value)
                {
                    _clearMaterials = value;
                    SQLiteConnectionUser.PutSettingBool("ClearMaterials", value);
                    OnConfigChangedEvent(ConfigProperty.ClearMaterials, value);
                }
            }
        }

        /// <summary>
        /// The default map colour.
        /// </summary>
        public Color DefaultMapColour
        {
            get
            {
                return _defaultMapColour;
            }
            set
            {
                if (_defaultMapColour.ToArgb().CompareTo(value) != 0)
                {
                    _defaultMapColour = value;
                    SQLiteConnectionUser.PutSettingInt("DefaultMap", value.ToArgb());
                    OnMapColourChangedEvent(MapColourProperty.DefaultMapColour, value);
                }
            }
        }

        /// <summary>
        /// Whether to display event times in UTC (game time) or in local time.
        /// </summary>
        public bool DisplayUTC
        {
            get
            {
                return _displayUTC;
            }
            set
            {
                if (_displayUTC != value)
                {
                    _displayUTC = value;
                    SQLiteConnectionUser.PutSettingBool("DisplayUTC", value);
                    OnConfigChangedEvent(ConfigProperty.DisplayUTC, value);
                }
            }
        }

        /// <summary>
        /// Whether to (verbosely) log all EDSM requests.
        /// </summary>
        public bool EDSMLog
        {
            get
            {
                return _EDSMLog;
            }
            set
            {
                if (_EDSMLog != value)
                {
                    _EDSMLog = value;
                    SQLiteConnectionUser.PutSettingBool("EDSMLog", value);
                    OnConfigChangedEvent(ConfigProperty.EDSMLog, value);
                }
            }
        }

        /// <summary>
        /// Whether to automatically scroll (up/down) to new events in the journal and history log views.
        /// </summary>
        public bool FocusOnNewSystem
        {
            get
            {
                return _focusOnNewSystem;
            }
            set
            {
                if (_focusOnNewSystem != value)
                {
                    _focusOnNewSystem = value;
                    SQLiteConnectionUser.PutSettingBool("FocusOnNewSystem", value);
                    OnConfigChangedEvent(ConfigProperty.FocusOnNewSystem, value);
                }
            }
        }

        /// <summary>
        /// Whether to keep the <see cref="EDDiscovery"/> window on top.
        /// </summary>
        public bool KeepOnTop
        {
            get
            {
                return _keepOnTop;
            }
            set
            {
                if (_keepOnTop != value)
                {
                    _keepOnTop = value;
                    SQLiteConnectionUser.PutSettingBool("KeepOnTop", value);
                    OnConfigChangedEvent(ConfigProperty.KeepOnTop, value);
                }
            }
        }

        /// <summary>
        /// The currently selected colour map.
        /// </summary>
        public MapColoursClass MapColours { get; private set; } = new EDDConfig.MapColoursClass();

        /// <summary>
        /// Whether or not the main window will be hidden to the system notification
        /// area icon (systray) when minimized. Has no effect if
        /// <see cref="UseNotifyIcon"/> is not also enabled.
        /// </summary>
        public bool MinimizeToNotifyIcon
        {
            get
            {
                return _minimizeToNotifyIcon;
            }
            set
            {
                if (_minimizeToNotifyIcon != value)
                {
                    _minimizeToNotifyIcon = value;
                    SQLiteConnectionUser.PutSettingBool("MinimizeToNotifyIcon", value);
                    OnConfigChangedEvent(ConfigProperty.MinimizeToNotifyIcon, value);
                }
            }
        }

        /// <summary>
        /// Whether newest rows are on top (<c>false</c>), or bottom (<c>true</c>).
        /// </summary>
        public bool OrderRowsInverted
        {
            get
            {
                return _orderrowsinverted;
            }
            set
            {
                if (_orderrowsinverted != value)
                {
                    _orderrowsinverted = value;
                    SQLiteConnectionUser.PutSettingBool("OrderRowsInverted", value);
                    OnConfigChangedEvent(ConfigProperty.OrderRowsInverted, value);
                }
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
                if (_useNotifyIcon != value)
                {
                    _useNotifyIcon = value;
                    SQLiteConnectionUser.PutSettingBool("UseNotifyIcon", value);
                    OnConfigChangedEvent(ConfigProperty.UseNotifyIcon, value);
                }
            }
        }

        #endregion // Instantiated properties

        #region Methods

        /// <summary>
        /// Read config from storage. 
        /// </summary>
        /// <param name="write">Whether or not to write new commander information to storage.</param>
        /// <param name="conn">An existing UserDB connection, if available.</param>
        public void Update(bool write = true, SQLiteConnectionUser conn = null)
        {
            try
            {
                _autoLoadPopouts        = SQLiteConnectionUser.GetSettingBool("AutoLoadPopouts", _autoLoadPopouts, conn);
                _autoSavePopouts        = SQLiteConnectionUser.GetSettingBool("AutoSavePopouts", _autoSavePopouts, conn);
                _canSkipSlowUpdates     = SQLiteConnectionUser.GetSettingBool("CanSkipSlowUpdates", _canSkipSlowUpdates, conn);
                _clearCommodities       = SQLiteConnectionUser.GetSettingBool("ClearCommodities", _clearCommodities, conn);
                _clearMaterials         = SQLiteConnectionUser.GetSettingBool("ClearMaterials", _clearMaterials, conn);
                _defaultMapColour       = Color.FromArgb(SQLiteConnectionUser.GetSettingInt("DefaultMap", _defaultMapColour.ToArgb(), conn));
                _displayUTC             = SQLiteConnectionUser.GetSettingBool("DisplayUTC", _displayUTC, conn);
                _EDSMLog                = SQLiteConnectionUser.GetSettingBool("EDSMLog", _EDSMLog, conn);
                _focusOnNewSystem       = SQLiteConnectionUser.GetSettingBool("FocusOnNewSystem", _focusOnNewSystem, conn);
                _keepOnTop              = SQLiteConnectionUser.GetSettingBool("KeepOnTop", _keepOnTop, conn);
                _minimizeToNotifyIcon   = SQLiteConnectionUser.GetSettingBool("MinimizeToNotifyIcon", _minimizeToNotifyIcon, conn);
                _orderrowsinverted      = SQLiteConnectionUser.GetSettingBool("OrderRowsInverted", _orderrowsinverted, conn);
                _useNotifyIcon          = SQLiteConnectionUser.GetSettingBool("UseNotifyIcon", _useNotifyIcon, conn);

                EDCommander.Load(write, conn);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("EDDConfig.Update()" + ":" + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }

        }

        #endregion // Public methods

        #endregion // Public interface


        #region Protected event dispatchers

        protected virtual void OnConfigChangedEvent(ConfigProperty prop, bool newValue)
        {
            ConfigChanged?.Invoke(this, new ConfigChangedEventArgs(prop, newValue));
        }

        protected virtual void OnCurrentCommanderChangedEvent(int commanderIndex)
        {
            CurrentCommanderChanged?.Invoke(this, new CurrentCommanderChangedEventArgs(commanderIndex));
        }

        protected virtual void OnMapColourChangedEvent(MapColourProperty prop, Color colour)
        {
            MapColourChanged?.Invoke(this, new MapColourChangedEventArgs(prop, colour));
        }

        #endregion // Protected event dispatchers


        #region Private implementation

        #region Fields, both static and instantiated.

        private static EDDConfig _instance;

        // The values assigned here will be treated as program defaults upon initial program execution.
        private bool _autoLoadPopouts = false;
        private bool _autoSavePopouts = false;
        private bool _canSkipSlowUpdates = false;
        private bool _clearCommodities = false;
        private bool _clearMaterials = false;
        private Color _defaultMapColour = Color.Red;
        private bool _displayUTC = false;
        private bool _EDSMLog = false;
        private bool _focusOnNewSystem = false; /**< Whether to automatically focus on a new system in the TravelHistory */
        private bool _keepOnTop = false;        /**< Whether to keep the windows on top or not */
        private bool _minimizeToNotifyIcon = false;
        private bool _orderrowsinverted = false;
        private bool _useNotifyIcon = false;

        #endregion // Fields, both static and instantiated.

        #region Methods

        /// <summary>
        /// The one true constructor (my precious!).
        /// </summary>
        private EDDConfig()
        {
        }

        #endregion // Private methods

        #endregion // Private implementation
    }
}
