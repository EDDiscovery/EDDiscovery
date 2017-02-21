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
using EDDiscovery2;
using EDDiscovery2.EDSM;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace EDDiscovery
{
    public sealed class EDDConfig : INotifyPropertyChanged
    {
        #region Public interface

        /// <summary>
        /// The <see cref="INotifyPropertyChanged"/> event handler to allow for binding this class the <see cref="Settings"/> tab and other controls.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #region Configuration properties

        /*
         * When adding a new property, be sure to add it to the `static Load(bool, SQLConnection)` function to read it in once the database is up,
         * and set an appropriate default value to the backing field that is .
         * 
         * Since properties may exist in the database using an different name that no longer reflects the property name, the `Instance.SetT()`
         * methods allow for an optional `backendName` parameter capable of mapping to the older name used in the DB. When not specified, it
         * will default to the name of the property currently being set.
         */

        private bool _AutoLoadPopouts = false;
        /// <summary>
        /// Whether we should automatically load popout (S-Panel) windows at startup.
        /// </summary>
        public static bool AutoLoadPopOuts
        {
            get
            {
                return Instance._AutoLoadPopouts;
            }
            set
            {
                Instance.SetBool(ref Instance._AutoLoadPopouts, value);
            }
        }

        private bool _AutoSavePopouts = false;
        /// <summary>
        /// Whether we should automatically save popout (S-Panel) windows at exit.
        /// </summary>
        public static bool AutoSavePopOuts
        {
            get
            {
                return Instance._AutoSavePopouts;
            }
            set
            {
                Instance.SetBool(ref Instance._AutoSavePopouts, value);
            }
        }

        private bool _CanSkipSlowUpdates = false;
        /// <summary>
        /// (DEBUG ONLY) Whether to skip slow updates at startup. 
        /// </summary>
        public static bool CanSkipSlowUpdates
        {
            get
            {
                return (Instance._CanSkipSlowUpdates && EDDConfig.Options.Debug);
            }
            set
            {
                if (EDDConfig.Options.Debug)
                    Instance.SetBool(ref Instance._CanSkipSlowUpdates, value);
            }
        }

        private Color _CentredSystemColour = Color.Yellow;
        /// <summary>
        /// The color of the selected system in the maps.
        /// </summary>
        public static Color CentredSystemColour
        {
            get
            {
                return Instance._CentredSystemColour;
            }
            set
            {
                Instance.SetColor(ref Instance._CentredSystemColour, value, "MapColour_CentredSystem");
            }
        }
        
        private bool _ClearCommodities = false;
        /// <summary>
        /// Whether the commodities viewer should hide commodities that the commander does not have on-hand.
        /// </summary>
        public static bool ClearCommodities
        {
            get
            {
                return Instance._ClearCommodities;
            }
            set
            {
                Instance.SetBool(ref Instance._ClearCommodities, value);
            }
        }

        private bool _ClearMaterials = false;
        /// <summary>
        /// Whether the materials viewer should hide materials that the command does not have on-hand.
        /// </summary>
        public static bool ClearMaterials
        {
            get
            {
                return Instance._ClearMaterials;
            }
            set
            {
                Instance.SetBool(ref Instance._ClearMaterials, value);
            }
        }

        private Color _CoarseGridLinesColour = ColorTranslator.FromHtml("#296A6C");
        /// <summary>
        /// The coarse grid line colour used for the maps.
        /// </summary>
        public static Color CoarseGridLinesColour
        {
            get
            {
                return Instance._CoarseGridLinesColour;
            }
            set
            {
                Instance.SetColor(ref Instance._CoarseGridLinesColour, value, "MapColour_CoarseGridLines");
            }
        }

        private Color _DefaultMapColour = Color.Red;
        /// <summary>
        /// The default map colour.
        /// </summary>
        public static Color DefaultMapColour
        {
            get
            {
                return Instance._DefaultMapColour;
            }
            set
            {
                Instance.SetColor(ref Instance._DefaultMapColour, value, "DefaultMap");
            }
        }

        private string _DefaultVoiceDevice = "Default";
        /// <summary>
        /// The audio device that is selected to play text-to-speech events.
        /// </summary>
        public static string DefaultVoiceDevice
        {
            get
            {
                return Instance._DefaultVoiceDevice;
            }
            set
            {
                Instance.SetString(ref Instance._DefaultVoiceDevice, value, "VoiceAudioDevice");
            }
        }

        private string _DefaultWaveDevice = "Default";
        /// <summary>
        /// The audio device that is selected to play audio output events besides text-to-speech.
        /// </summary>
        public static string DefaultWaveDevice
        {
            get
            {
                return Instance._DefaultWaveDevice;
            }
            set
            {
                Instance.SetString(ref Instance._DefaultWaveDevice, value, "WaveAudioDevice");
            }
        }

        private bool _DisplayUTC = false;
        /// <summary>
        /// Whether to display event times in UTC (game time, when <c>true</c>) or in local time (when <c>false</c>).
        /// </summary>
        public static bool DisplayUTC
        {
            get
            {
                return Instance._DisplayUTC;
            }
            set
            {
                Instance.SetBool(ref Instance._DisplayUTC, value);
            }
        }

        private bool _EDSMLog = false;
        /// <summary>
        /// Whether to (verbosely) log all EDSM requests.
        /// </summary>
        public static bool EDSMLog
        {
            get
            {
                return Instance._EDSMLog;
            }
            set
            {
                Instance.SetBool(ref Instance._EDSMLog, value);
            }
        }

        private Color _FineGridLinesColour = ColorTranslator.FromHtml("#202020");
        /// <summary>
        /// The fine grid line color used for the maps.
        /// </summary>
        public static Color FineGridLinesColour
        {
            get
            {
                return Instance._FineGridLinesColour;
            }
            set
            {
                Instance.SetColor(ref Instance._FineGridLinesColour, value, "MapColour_FineGridLines");
            }
        }

        private bool _FocusOnNewSystem = false;
        /// <summary>
        /// Whether to automatically scroll (up/down) to new events in the journal and history log views.
        /// </summary>
        public static bool FocusOnNewSystem
        {
            get
            {
                return Instance._FocusOnNewSystem;
            }
            set
            {
                Instance.SetBool(ref Instance._FocusOnNewSystem, value);
            }
        }

        private string _HomeSystem = "Sol";
        /// <summary>
        /// This is what the user has marked as their home system, and is used for distance calculations as
        /// well as the default center point for the 3d map whenever it is displayed.
        /// </summary>
        public static string HomeSystem
        {
            get
            {
                return Instance._HomeSystem;
            }
            set
            {
                if (value != null)
                    Instance.SetString(ref Instance._HomeSystem, value, "DefaultMapCenter");
            }
        }

        private bool _KeepOnTop = false;
        /// <summary>
        /// Whether to keep the <see cref="EDDiscoveryForm"/> window on top.
        /// </summary>
        public static bool KeepOnTop
        {
            get
            {
                return Instance._KeepOnTop;
            }
            set
            {
                Instance.SetBool(ref Instance._KeepOnTop, value);
            }
        }

        private readonly string _LogIndex;
        /// <summary>
        /// Set during <see cref="EDDConfig"/> constructor to the current date. Used solely
        /// to determine the log file that this application instance wiill write to.
        /// </summary>
        public static string LogIndex
        {
            get
            {
                return Instance._LogIndex;
            }
        }

        private bool _MinimizeToNotifyIcon = false;
        /// <summary>
        /// Whether or not the main window will be hidden to the system notification area icon (systray)
        /// when minimized. Has no effect if <see cref="UseNotifyIcon"/> is not also enabled.
        /// </summary>
        public static bool MinimizeToNotifyIcon
        {
            get
            {
                return Instance._MinimizeToNotifyIcon;
            }
            set
            {
                Instance.SetBool(ref Instance._MinimizeToNotifyIcon, value);
            }
        }

        private bool _OrderRowsInverted = false;
        /// <summary>
        /// Whether newest rows are on top (<c>false</c>), or bottom (<c>true</c>).
        /// </summary>
        public static bool OrderRowsInverted
        {
            get
            {
                return Instance._OrderRowsInverted;
            }
            set
            {
                Instance.SetBool(ref Instance._OrderRowsInverted, value);
            }
        }

        private Color _PlannedRouteColour = Color.Green;
        /// <summary>
        /// The color of the currently planned route on the maps.
        /// </summary>
        public static Color PlannedRouteColour
        {
            get
            {
                return Instance._PlannedRouteColour;
            }
            set
            {
                Instance.SetColor(ref Instance._PlannedRouteColour, value, "MapColour_PlannedRoute");
            }
        }

        private bool _UseNotifyIcon = false;
        /// <summary>
        /// Whether or not a system notification area (systray) icon will be used.
        /// </summary>
        public static bool UseNotifyIcon
        {
            get
            {
                return Instance._UseNotifyIcon;
            }
            set
            {
                Instance.SetBool(ref Instance._UseNotifyIcon, value);
            }
        }

        #endregion // Configuration properties

        #region Less exciting stuff

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
        /// Command-line options, and other settings that cannot be changed during runtime.
        /// </summary>
        public static OptionsClass Options { get; } = new OptionsClass();
        /// <summary>
        /// Class representing command-line options, and other settings that cannot be changed while running.
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
            public string Version { get; private set; }
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

            /// <summary>
            /// Dump the configured options to a string for easy writing to the trace log.
            /// </summary>
            /// <returns>A string containing any configured options, ignoring ones that are visible in <see cref="VersionDisplayString"/>.</returns>
            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
#if DEBUG
                sb.Append("DEBUG defined, ");
#endif
#if TRACE
                sb.Append("TRACE defined, ");
#endif
                sb.Append($"AppDataDir: '{AppDataDirectory}', ");
                if (Debug)
                    sb.Append("Debug, ");
                if (LogExceptions)
                    sb.Append("LogExceptions, ");
                if (NoWindowReposition)
                    sb.Append("NoWindowReposition, ");
                if (!string.IsNullOrWhiteSpace(OldDatabasePath) && File.Exists(OldDatabasePath))
                    sb.Append($"OldDBPath: '{OldDatabasePath}', ");
                if (!string.IsNullOrWhiteSpace(OptionsFile) && File.Exists(OptionsFile))
                    sb.Append($"Using OptionsFile: '{OptionsFile}', ");
                if (!string.IsNullOrWhiteSpace(ReadJournal))
                    sb.Append($"ReadJournal: '{ReadJournal}', ");
                if (StoreDataInProgramDirectory)
                    sb.Append("Portable, ");
                if (TraceLog)
                    sb.Append("Explicit TraceLog, ");
                sb.Append($"UserDB: '{UserDatabasePath}', ");
                sb.Append($"SysDB: '{SystemDatabasePath}'");

                return sb.ToString();
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
                Version = Assembly.GetExecutingAssembly().FullName.Split(',')[1].Split('=')[1];

                StringBuilder sb = new StringBuilder();
                sb.Append($"Version {Version}");

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

        /// <summary>
        /// User-specified paths to directories and files on the computer
        /// </summary>
        public static UserPathsClass UserPaths { get; } = new UserPathsClass();
        /// <summary>
        /// Class representing paths to files on the current computer.
        /// </summary>
        /// <remarks>
        /// This exist as there are many people who will share the EDDUser.sqlite between different
        /// computers, and some of them do not have the same paths to images etc. on those computers.
        /// </remarks>
        public class UserPathsClass
        {
            #region Properties

            /// <summary>
            /// The folder containing the Elite Dangerous executable.
            /// </summary>
            public string EDDirectory { get; set; }

            /// <summary>
            /// The folder where EDD outputs processed screenshots.
            /// </summary>
            public string ImageHandlerOutputDir { get; set; }

            /// <summary>
            /// The folder where screenshots are output from Elite Dangerous, (potentially) prior to being processed by EDD.
            /// </summary>
            public string ImageHandlerScreenshotsDir { get; set; }

            #endregion

            #region Methods

            /// <summary>
            /// Loads the paths from the database and from UserPaths.json
            /// </summary>
            /// <param name="conn">Optional connection from which to load settings</param>
            public void Load(SQLiteConnectionUser conn = null)
            {
                EDDirectory = SQLiteConnectionUser.GetSettingString("EDDirectory", "", conn);
                ImageHandlerOutputDir = SQLiteConnectionUser.GetSettingString("ImageHandlerOutputDir", null, conn);
                ImageHandlerScreenshotsDir = SQLiteConnectionUser.GetSettingString("ImageHandlerScreenshotDir", null, conn);

                if (!string.IsNullOrWhiteSpace(Options.AppFolder) && File.Exists(Path.Combine(Options.AppFolder, "UserPaths.json")))
                {
                    JObject jo;

                    using (FileStream stream = File.OpenRead(Path.Combine(Options.AppFolder, "UserPaths.json")))
                    {
                        using (StreamReader rdr = new StreamReader(stream))
                        {
                            using (JsonTextReader jrdr = new JsonTextReader(rdr))
                            {
                                jo = JObject.Load(jrdr);
                            }
                        }
                    }

                    EDDirectory = JSONHelper.GetStringDef(jo["EDDirectory"], EDDirectory);
                    ImageHandlerOutputDir = JSONHelper.GetStringDef(jo["ImageHandlerOutputDir"], ImageHandlerOutputDir);
                    ImageHandlerScreenshotsDir = JSONHelper.GetStringDef(jo["ImageHandlerScreenshotsDir"], ImageHandlerScreenshotsDir);
                }
            }

            /// <summary>
            /// Saves the paths to the database and to UserPaths.json
            /// </summary>
            /// <param name="conn">Optional connection with which to save to the database</param>
            public void Save(SQLiteConnectionUser conn = null)
            {
                SQLiteConnectionUser.PutSettingString("EDDirectory", EDDirectory, conn);
                SQLiteConnectionUser.PutSettingString("ImageHandlerOutputDir", ImageHandlerOutputDir, conn);
                SQLiteConnectionUser.PutSettingString("ImageHandlerScreenshotsDir", ImageHandlerScreenshotsDir, conn);

                if (!string.IsNullOrWhiteSpace(Options.AppFolder))
                {
                    JObject jo = new JObject(
                        new JProperty("EDDirectory", EDDirectory),
                        new JProperty("ImageHandlerOutputDir", ImageHandlerOutputDir),
                        new JProperty("ImageHandlerScreenshotsDir", ImageHandlerScreenshotsDir)
                    );

                    using (FileStream stream = File.OpenWrite(Path.Combine(Options.AppFolder, "UserPaths.json.tmp")))
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            using (JsonTextWriter jwriter = new JsonTextWriter(writer))
                            {
                                jo.WriteTo(jwriter);
                            }
                        }
                    }

                    File.Delete(Path.Combine(Options.AppFolder, "UserPaths.json"));
                    File.Move(Path.Combine(Options.AppFolder, "UserPaths.json.tmp"), Path.Combine(Options.AppFolder, "UserPaths.json"));
                }
            }

            #endregion
        }

        private static readonly Lazy<EDDConfig> _Instance = new Lazy<EDDConfig>(() => new EDDConfig());
        /// <summary>
        /// Return the true instantiated EDDConfig, constructing it if necessary. Use this to access
        /// all settings that are not established beyond the scope of the application lifecycle.
        /// </summary>
        public static EDDConfig Instance
        {
            get
            {
                return _Instance.Value;
            }
        }

        /// <summary>
        /// Read config from storage. 
        /// </summary>
        /// <param name="write">Whether or not to write new commander information to storage.</param>
        /// <param name="conn">An existing <see cref="SQLiteConnectionUser"/> database connection, if available.</param>
        public static void Load(bool write = true, SQLiteConnectionUser conn = null)
        {
            bool createdconn = false;
            try
            {
                if (conn == null && write)
                {
                    createdconn = true;
                    conn = new SQLiteConnectionUser(Instance._DisplayUTC, EDDbAccessMode.Indeterminate);
                }
                Instance._AutoLoadPopouts       = SQLiteConnectionUser.GetSettingBool("AutoLoadPopouts", Instance._AutoLoadPopouts, conn);
                Instance._AutoSavePopouts       = SQLiteConnectionUser.GetSettingBool("AutoSavePopouts", Instance._AutoSavePopouts, conn);
                Instance._CanSkipSlowUpdates    = SQLiteConnectionUser.GetSettingBool("CanSkipSlowUpdates", Instance._CanSkipSlowUpdates, conn);
                Instance._ClearCommodities      = SQLiteConnectionUser.GetSettingBool("ClearCommodities", Instance._ClearCommodities, conn);
                Instance._ClearMaterials        = SQLiteConnectionUser.GetSettingBool("ClearMaterials", Instance._ClearMaterials, conn);
                Instance._DisplayUTC            = SQLiteConnectionUser.GetSettingBool("DisplayUTC", Instance._DisplayUTC, conn);
                Instance._EDSMLog               = SQLiteConnectionUser.GetSettingBool("EDSMLog", Instance._EDSMLog, conn);
                Instance._FocusOnNewSystem      = SQLiteConnectionUser.GetSettingBool("FocusOnNewSystem", Instance._FocusOnNewSystem, conn);
                Instance._KeepOnTop             = SQLiteConnectionUser.GetSettingBool("KeepOnTop", Instance._KeepOnTop, conn);
                Instance._MinimizeToNotifyIcon  = SQLiteConnectionUser.GetSettingBool("MinimizeToNotifyIcon", Instance._MinimizeToNotifyIcon, conn);
                Instance._OrderRowsInverted     = SQLiteConnectionUser.GetSettingBool("OrderRowsInverted", Instance._OrderRowsInverted, conn);
                Instance._UseNotifyIcon         = SQLiteConnectionUser.GetSettingBool("UseNotifyIcon", Instance._UseNotifyIcon, conn);

                Instance._DefaultVoiceDevice    = SQLiteConnectionUser.GetSettingString("VoiceAudioDevice", Instance._DefaultVoiceDevice, conn);
                Instance._DefaultWaveDevice     = SQLiteConnectionUser.GetSettingString("VoiceWaveDevice", Instance._DefaultWaveDevice, conn);
                Instance._HomeSystem            = SQLiteConnectionUser.GetSettingString("DefaultMapCenter", Instance._HomeSystem, conn);

                Instance._CentredSystemColour   = Color.FromArgb(SQLiteConnectionUser.GetSettingInt("MapColour_CentredSystem", Instance._CentredSystemColour.ToArgb(), conn));
                Instance._CoarseGridLinesColour = Color.FromArgb(SQLiteConnectionUser.GetSettingInt("MapColour_CoarseGridLines", Instance._CoarseGridLinesColour.ToArgb(), conn));
                Instance._DefaultMapColour      = Color.FromArgb(SQLiteConnectionUser.GetSettingInt("DefaultMap", Instance._DefaultMapColour.ToArgb(), conn));
                Instance._FineGridLinesColour   = Color.FromArgb(SQLiteConnectionUser.GetSettingInt("MapColour_FineGridLines", Instance._FineGridLinesColour.ToArgb(), conn));
                Instance._PlannedRouteColour    = Color.FromArgb(SQLiteConnectionUser.GetSettingInt("MapColour_PlannedRoute", Instance._PlannedRouteColour.ToArgb(), conn));

                EDCommander.Load(write, conn);
                EDDConfig.UserPaths.Load(conn);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("EDDConfig.Update()" + ":" + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }
            finally
            {
                if (createdconn)
                {
                    conn?.Dispose();
                    conn = null;
                }
            }
        }

        #endregion // Less exciting stuff

        #endregion // Public interface


        #region Private implementation

        /// <summary>
        /// The one true constructor (my precious!).
        /// </summary>
        private EDDConfig()
        {
            _LogIndex = DateTime.Now.ToString("yyyyMMdd");
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The property that has changed.</param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Update a <paramref name="field"/> of type <see cref="bool"/> to the provided <paramref name="value"/>, save it to
        /// the backend using the provided <paramref name="backendName"/>, and raise the <see cref="OnPropertyChanged"/> event.
        /// </summary>
        /// <param name="field">The field to save the <paramref name="value"/> to.</param>
        /// <param name="value">The new value to assign to the <paramref name="field"/>.</param>
        /// <param name="backendName">The name that this field will use in backend storage.</param>
        /// <param name="propertyName">The name of the property that has changed.</param>
        private bool SetBool(ref bool field, bool value, string backendName = null, [CallerMemberName] string propertyName = null)
        {
            if (field != value)
            {
                field = value;
                if (backendName == null)
                    SQLiteConnectionUser.PutSettingBool(propertyName, value);
                else
                    SQLiteConnectionUser.PutSettingBool(backendName, value);
                OnPropertyChanged(propertyName);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Update a <paramref name="field"/> of type <see cref="Color"/> to the provided <paramref name="value"/>, save it to
        /// the backend using the provided <paramref name="backendName"/>, and raise the <see cref="OnPropertyChanged"/> event.
        /// </summary>
        /// <param name="field">The field to save the <paramref name="value"/> to.</param>
        /// <param name="value">The new value to assign to the <paramref name="field"/>.</param>
        /// <param name="backendName">The name that this field will use in backend storage.</param>
        /// <param name="propertyName">The name of the property that has changed.</param>
        private bool SetColor(ref Color field, Color value, string backendName = null, [CallerMemberName] string propertyName = null)
        {
            if (field.ToArgb() != value.ToArgb())
            {
                field = value;
                if (backendName == null)
                    SQLiteConnectionUser.PutSettingInt(propertyName, value.ToArgb());
                else
                    SQLiteConnectionUser.PutSettingInt(backendName, value.ToArgb());
                OnPropertyChanged(propertyName);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Update a <paramref name="field"/> of type <see cref="int"/> to the provided <paramref name="value"/>, save it to
        /// the backend using the provided <paramref name="backendName"/>, and raise the <see cref="OnPropertyChanged"/> event.
        /// </summary>
        /// <param name="field">The field to save the <paramref name="value"/> to.</param>
        /// <param name="value">The new value to assign to the <paramref name="field"/>.</param>
        /// <param name="backendName">The name that this field will use in backend storage.</param>
        /// <param name="propertyName">The name of the property that has changed.</param>
        private bool SetInt(ref int field, int value, string backendName = null, [CallerMemberName] string propertyName = null)
        {
            if (field != value)
            {
                field = value;
                if (backendName == null)
                    SQLiteConnectionUser.PutSettingInt(propertyName, value);
                else
                    SQLiteConnectionUser.PutSettingInt(backendName, value);
                OnPropertyChanged(propertyName);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Update a <paramref name="field"/> of type <see cref="string"/> to the provided <paramref name="value"/>, save it to
        /// the backend using the provided <paramref name="propertyName"/>, and raise the <see cref="OnPropertyChanged"/> event.
        /// </summary>
        /// <param name="field">The field to save the <paramref name="value"/> to.</param>
        /// <param name="value">The new value to assign to the <paramref name="field"/>.</param>
        /// <param name="backendName">The name that this field will use in backend storage.</param>
        /// <param name="propertyName">The name of the property that has changed.</param>
        private bool SetString(ref string field, string value, string backendName = null, [CallerMemberName] string propertyName = null)
        {
            if (!field.Equals(value))
            {
                field = value;
                if (backendName == null)
                    SQLiteConnectionUser.PutSettingString(propertyName, value);
                else
                    SQLiteConnectionUser.PutSettingString(backendName, value);
                OnPropertyChanged(propertyName);
                return true;
            }
            else
                return false;
        }

        #endregion // Private implementation
    }
}
