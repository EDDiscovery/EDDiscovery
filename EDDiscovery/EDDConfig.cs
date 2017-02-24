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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace EDDiscovery
{
    public sealed class EDDConfig : INotifyPropertyChanged
    {
        /* ***** General Notes *****
         * When adding a new property, be sure to add it to the `void Load(bool, SQL)` function to read the value in
         * from the database, and set an appropriate default value to the backing field to ensure a reasonable default.
         * 
         * Properties may exist in the database using an different name that no longer reflects the property name, so
         * the SetT() methods allow for an optional `backendName` parameter capable of mapping to whatever name is used
         * in the DB. When not specified, it will default to the name of the property currently being set.
         * 
         * `PropNameChanged` events will be dispatched using the thread that the subscriber used to add the event handler,
         * meaning that as long as you subscribe to these events on your UI thread, you can update your UI in said handler
         * without having to Invoke your own Form/Controls.
         * 
         * If you'd like to be like the Settings tab and allow direct Control<->Property updating, use the included
         * ThreadedBindingList<EDDConfig> and be sure to construct that on your UI thread only.
         */


        #region Public interface

        #region Named Properties

        private bool _AutoLoadPopouts = false;
        /// <summary>
        /// Whether we should automatically load popout (S-Panel) windows at startup.
        /// </summary>
        public bool AutoLoadPopouts
        {
            get
            {
                return _AutoLoadPopouts;
            }
            set
            {
                SetBool(ref _AutoLoadPopouts, value, null);
            }
        }

        private bool _AutoSavePopouts = false;
        /// <summary>
        /// Whether we should automatically save popout (S-Panel) windows at exit.
        /// </summary>
        public bool AutoSavePopouts
        {
            get
            {
                return _AutoSavePopouts;
            }
            set
            {
                SetBool(ref _AutoSavePopouts, value, null);
            }
        }

        private bool _CanSkipSlowUpdates = false;
        /// <summary>
        /// The <see cref="CanSkipSlowUpdates"/> property changed event.
        /// </summary>
        public event EventHandler<bool> CanSkipSlowUpdatesChanged;
        /// <summary>
        /// (DEBUG ONLY) Whether to skip slow updates at startup. 
        /// </summary>
        public bool CanSkipSlowUpdates
        {
            get
            {
                return (_CanSkipSlowUpdates);
            }
            set
            {
                SetBool(ref _CanSkipSlowUpdates, value, CanSkipSlowUpdatesChanged);
            }
        }

        private Color _CentredSystemColour = Color.Yellow;
        /// <summary>
        /// The <see cref="CentredSystemColour"/> property changed event.
        /// </summary>
        public event EventHandler<Color> CentredSystemColourChanged;
        /// <summary>
        /// The color of the selected system in the maps.
        /// </summary>
        public Color CentredSystemColour
        {
            get
            {
                return _CentredSystemColour;
            }
            set
            {
                SetColor(ref _CentredSystemColour, value, CentredSystemColourChanged, "MapColour_CentredSystem");
            }
        }
        
        private bool _ClearCommodities = false;
        /// <summary>
        /// The <see cref="ClearCommodities"/> property changed event.
        /// </summary>
        public event EventHandler<bool> ClearCommoditiesChanged;
        /// <summary>
        /// Whether the commodities viewer should hide commodities that the commander does not have on-hand.
        /// </summary>
        public bool ClearCommodities
        {
            get
            {
                return _ClearCommodities;
            }
            set
            {
                SetBool(ref _ClearCommodities, value, ClearCommoditiesChanged);
            }
        }

        private bool _ClearMaterials = false;
        /// <summary>
        /// The <see cref="ClearMaterials"/> property changed event.
        /// </summary>
        public event EventHandler<bool> ClearMaterialsChanged;
        /// <summary>
        /// Whether the materials viewer should hide materials that the command does not have on-hand.
        /// </summary>
        public bool ClearMaterials
        {
            get
            {
                return _ClearMaterials;
            }
            set
            {
                SetBool(ref _ClearMaterials, value, ClearMaterialsChanged);
            }
        }

        private Color _CoarseGridLinesColour = ColorTranslator.FromHtml("#296A6C");
        /// <summary>
        /// The <see cref="CoarseGridLinesColour"/> property changed event.
        /// </summary>
        public event EventHandler<Color> CoarseGridLinesColourChanged;
        /// <summary>
        /// The coarse grid line colour used for the maps.
        /// </summary>
        public Color CoarseGridLinesColour
        {
            get
            {
                return _CoarseGridLinesColour;
            }
            set
            {
                SetColor(ref _CoarseGridLinesColour, value, CoarseGridLinesColourChanged, "MapColour_CoarseGridLines");
            }
        }

        private Color _DefaultMapColour = Color.Red;
        /// <summary>
        /// The <see cref="DefaultMapColour"/> property changed event.
        /// </summary>
        public event EventHandler<Color> DefaultMapColourChanged;
        /// <summary>
        /// The default map colour.
        /// </summary>
        public Color DefaultMapColour
        {
            get
            {
                return _DefaultMapColour;
            }
            set
            {
                SetColor(ref _DefaultMapColour, value, DefaultMapColourChanged, "DefaultMap");
            }
        }

        private string _DefaultVoiceDevice = "Default";
        /// <summary>
        /// The <see cref="DefaultVoiceDevice"/> property changed event.
        /// </summary>
        public event EventHandler<string> DefaultVoiceDeviceChanged;
        /// <summary>
        /// The audio device that is selected to play text-to-speech events.
        /// </summary>
        public string DefaultVoiceDevice
        {
            get
            {
                return _DefaultVoiceDevice;
            }
            set
            {
                SetString(ref _DefaultVoiceDevice, value, DefaultVoiceDeviceChanged, "VoiceAudioDevice");
            }
        }

        private string _DefaultWaveDevice = "Default";
        /// <summary>
        /// The <see cref="DefaultWaveDevice"/> property changed event.
        /// </summary>
        public event EventHandler<string> DefaultWaveDeviceChanged;
        /// <summary>
        /// The audio device that is selected to play audio output events besides text-to-speech.
        /// </summary>
        public string DefaultWaveDevice
        {
            get
            {
                return _DefaultWaveDevice;
            }
            set
            {
                SetString(ref _DefaultWaveDevice, value, DefaultWaveDeviceChanged, "WaveAudioDevice");
            }
        }

        private bool _DisplayUTC = false;
        /// <summary>
        /// The <see cref="DisplayUTC"/> property changed event.
        /// </summary>
        public event EventHandler<bool> DisplayUTCChanged;
        /// <summary>
        /// Whether to display event times in UTC (game time, when <c>true</c>) or in local time (when <c>false</c>).
        /// </summary>
        public bool DisplayUTC
        {
            get
            {
                return _DisplayUTC;
            }
            set
            {
                SetBool(ref _DisplayUTC, value, DisplayUTCChanged);
            }
        }

        private bool _EDSMLog = false;
        /// <summary>
        /// The <see cref="EDSMLog"/> property changed event.
        /// </summary>
        public event EventHandler<bool> EDSMLogChanged;
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
                SetBool(ref _EDSMLog, value, EDSMLogChanged);
            }
        }

        private Color _FineGridLinesColour = ColorTranslator.FromHtml("#202020");
        /// <summary>
        /// The <see cref="FineGridLinesColour"/> property changed event.
        /// </summary>
        public event EventHandler<Color> FineGridLinesColourChanged;
        /// <summary>
        /// The fine grid line color used for the maps.
        /// </summary>
        public Color FineGridLinesColour
        {
            get
            {
                return _FineGridLinesColour;
            }
            set
            {
                SetColor(ref _FineGridLinesColour, value, FineGridLinesColourChanged, "MapColour_FineGridLines");
            }
        }

        private bool _FocusOnNewSystem = false;
        /// <summary>
        /// Whether to automatically scroll (up/down) to new events in the journal and history log views.
        /// </summary>
        public bool FocusOnNewSystem
        {
            get
            {
                return _FocusOnNewSystem;
            }
            set
            {
                SetBool(ref _FocusOnNewSystem, value, null);
            }
        }

        private string _HomeSystem = "Sol";
        /// <summary>
        /// The <see cref="HomeSystem"/> property changed event.
        /// </summary>
        public event EventHandler<string> HomeSystemChanged;
        /// <summary>
        /// This is what the user has marked as their home system, and is used for distance calculations as
        /// well as the default center point for the 3d map whenever it is displayed.
        /// </summary>
        public string HomeSystem
        {
            get
            {
                return _HomeSystem;
            }
            set
            {
                SetString(ref _HomeSystem, value, HomeSystemChanged, "DefaultMapCenter");
            }
        }

        private bool _KeepOnTop = false;
        /// <summary>
        /// The <see cref="KeepOnTop"/> property changed event.
        /// </summary>
        public event EventHandler<bool> KeepOnTopChanged;
        /// <summary>
        /// Whether to keep the <see cref="EDDiscoveryForm"/> window on top.
        /// </summary>
        public bool KeepOnTop
        {
            get
            {
                return _KeepOnTop;
            }
            set
            {
                SetBool(ref _KeepOnTop, value, KeepOnTopChanged);
            }
        }

        private bool _MinimizeToNotifyIcon = false;
        /// <summary>
        /// The <see cref="MinimizeToNotifyIcon"/> property changed event.
        /// </summary>
        public event EventHandler<bool> MinimizeToNotifyIconChanged;
        /// <summary>
        /// Whether or not the main window will be hidden to the system notification area icon (systray)
        /// when minimized. Has no effect if <see cref="UseNotifyIcon"/> is not also enabled.
        /// </summary>
        public bool MinimizeToNotifyIcon
        {
            get
            {
                return _MinimizeToNotifyIcon;
            }
            set
            {
                SetBool(ref _MinimizeToNotifyIcon, value, MinimizeToNotifyIconChanged);
            }
        }

        private bool _OrderRowsInverted = false;
        /// <summary>
        /// The <see cref="OrderRowsInverted"/> property changed event.
        /// </summary>
        public event EventHandler<bool> OrderRowsInvertedChanged;
        /// <summary>
        /// Whether newest rows are on top (<c>false</c>), or bottom (<c>true</c>).
        /// </summary>
        public bool OrderRowsInverted
        {
            get
            {
                return _OrderRowsInverted;
            }
            set
            {
                SetBool(ref _OrderRowsInverted, value, OrderRowsInvertedChanged);
            }
        }

        private Color _PlannedRouteColour = Color.Green;
        /// <summary>
        /// The <see cref="PlannedRouteColour"/> property changed event.
        /// </summary>
        public event EventHandler<Color> PlannedRouteColourChanged;
        /// <summary>
        /// The color of the currently planned route on the maps.
        /// </summary>
        public Color PlannedRouteColour
        {
            get
            {
                return _PlannedRouteColour;
            }
            set
            {
                SetColor(ref _PlannedRouteColour, value, PlannedRouteColourChanged, "MapColour_PlannedRoute");
            }
        }

        private bool _UseNotifyIcon = false;
        /// <summary>
        /// The <see cref="UseNotifyIcon"/> property changed event.
        /// </summary>
        public event EventHandler<bool> UseNotifyIconChanged;
        /// <summary>
        /// Whether or not a system notification area (systray) icon will be used.
        /// </summary>
        public bool UseNotifyIcon
        {
            get
            {
                return _UseNotifyIcon;
            }
            set
            {
                SetBool(ref _UseNotifyIcon, value, UseNotifyIconChanged);
            }
        }

        #endregion // Named Properties

        #region Less exciting stuff

        #region Boilerplate Properties

        /// <summary>
        /// You should probably not use this event directly, instead see <see cref="ThreadedBindingList"/>.
        /// An event handler to allow for binding properties of this class to controls.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private static readonly Lazy<EDDConfig> _Instance = new Lazy<EDDConfig>(() => new EDDConfig());
        /// <summary>
        /// The single instantiated <see cref="EDDConfig"/>.
        /// </summary>
        public static EDDConfig Instance
        {
            get
            {
                return _Instance.Value;
            }
        }

        private readonly string _LogIndex;
        /// <summary>
        /// The date (in yyyyMMdd format) of when this app instance was started. Used solely
        /// to determine which log file this application instance wiill write to.
        /// </summary>
        public string LogIndex
        {
            get
            {
                return _LogIndex;
            }
        }

        /// <summary>
        /// Command-line options, and other settings that cannot be changed during runtime.
        /// </summary>
        public static OptionsClass Options { get; } = new OptionsClass();

        /// <summary>
        /// User-specified paths to directories and files on the computer
        /// </summary>
        public static UserPathsClass UserPaths { get; } = new UserPathsClass();

        #endregion // Boilerplate Properties

        #region SubType definitions

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
        /// A thread-safe BindingList{EDDConfig} to bind control properties to <see cref="EDDConfig"/> properties.
        /// </summary>
        public class ThreadedBindingList : BindingList<EDDConfig>, IDisposable
        {
            private BindingSource _bs;
            private readonly SynchronizationContext _ctx;

            public ThreadedBindingList()
            {
                _ctx = SynchronizationContext.Current;
                _bs = new BindingSource();
                _bs.Add(this);
                Add(Instance);
            }

            /// <summary>
            /// Convenience method to bind a <see cref="Control"/> property to an <see cref="EDDConfig"/> property
            /// with immediate updates for controls such as a checkbox or radio buttons.
            /// </summary>
            /// <param name="control">The control that shall be bound to the <paramref name="configProp"/>.</param>
            /// <param name="controlProp">The control's property that shall be bound to, such as Text, Checked, etc.</param>
            /// <param name="configProp">A named <see cref="EDDConfig"/> property to bind to the control.</param>
            public void Bind(IBindableComponent control, string controlProp, string configProp)
            {
                control.DataBindings.Add(controlProp, this, configProp, false, DataSourceUpdateMode.OnPropertyChanged);
            }

            /// <summary>
            /// Convenience method to bind a <see cref="Control"/> property to an <see cref="EDDConfig"/> property
            /// with updates occuring after the control has validated any input.
            /// </summary>
            /// <param name="control">The control that shall be bound to the <paramref name="configProp"/>.</param>
            /// <param name="controlProp">The control's property that shall be bound to, such as Text, Checked, etc.</param>
            /// <param name="configProp">A named <see cref="EDDConfig"/> property to bind to the control.</param>
            public void BindValidated(IBindableComponent control, string controlProp, string configProp)
            {
                control.DataBindings.Add(controlProp, this, configProp, false, DataSourceUpdateMode.OnValidation);
            }

            public void Dispose()
            {
                Clear();
                _bs.Clear();
            }

            protected override void OnListChanged(ListChangedEventArgs e)
            {
                if (_ctx == null)
                    base.OnListChanged(e);
                else
                    _ctx.Send(delegate
                    {
                        base.OnListChanged(e);
                    }, null);
            }
        }

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

        #endregion // SubType definitions


        /// <summary>
        /// Read config from storage. 
        /// </summary>
        /// <param name="write">Whether or not to write new commander information to storage.</param>
        /// <param name="conn">An existing <see cref="SQLiteConnectionUser"/> database connection, if available.</param>
        public void Load(bool write = true, SQLiteConnectionUser conn = null)
        {
            bool createdconn = false;
            try
            {
                if (conn == null && write)
                {
                    createdconn = true;
                    conn = new SQLiteConnectionUser(_DisplayUTC, EDDbAccessMode.Indeterminate);
                }
                _AutoLoadPopouts       = SQLiteConnectionUser.GetSettingBool("AutoLoadPopouts", _AutoLoadPopouts, conn);
                _AutoSavePopouts       = SQLiteConnectionUser.GetSettingBool("AutoSavePopouts", _AutoSavePopouts, conn);
                _CanSkipSlowUpdates    = SQLiteConnectionUser.GetSettingBool("CanSkipSlowUpdates", _CanSkipSlowUpdates, conn);
                _ClearCommodities      = SQLiteConnectionUser.GetSettingBool("ClearCommodities", _ClearCommodities, conn);
                _ClearMaterials        = SQLiteConnectionUser.GetSettingBool("ClearMaterials", _ClearMaterials, conn);
                _DisplayUTC            = SQLiteConnectionUser.GetSettingBool("DisplayUTC", _DisplayUTC, conn);
                _EDSMLog               = SQLiteConnectionUser.GetSettingBool("EDSMLog", _EDSMLog, conn);
                _FocusOnNewSystem      = SQLiteConnectionUser.GetSettingBool("FocusOnNewSystem", _FocusOnNewSystem, conn);
                _KeepOnTop             = SQLiteConnectionUser.GetSettingBool("KeepOnTop", _KeepOnTop, conn);
                _MinimizeToNotifyIcon  = SQLiteConnectionUser.GetSettingBool("MinimizeToNotifyIcon", _MinimizeToNotifyIcon, conn);
                _OrderRowsInverted     = SQLiteConnectionUser.GetSettingBool("OrderRowsInverted", _OrderRowsInverted, conn);
                _UseNotifyIcon         = SQLiteConnectionUser.GetSettingBool("UseNotifyIcon", _UseNotifyIcon, conn);

                _DefaultVoiceDevice    = SQLiteConnectionUser.GetSettingString("VoiceAudioDevice", _DefaultVoiceDevice, conn);
                _DefaultWaveDevice     = SQLiteConnectionUser.GetSettingString("VoiceWaveDevice", _DefaultWaveDevice, conn);
                _HomeSystem            = SQLiteConnectionUser.GetSettingString("DefaultMapCenter", _HomeSystem, conn);

                _CentredSystemColour   = Color.FromArgb(SQLiteConnectionUser.GetSettingInt("MapColour_CentredSystem", _CentredSystemColour.ToArgb(), conn));
                _CoarseGridLinesColour = Color.FromArgb(SQLiteConnectionUser.GetSettingInt("MapColour_CoarseGridLines", _CoarseGridLinesColour.ToArgb(), conn));
                _DefaultMapColour      = Color.FromArgb(SQLiteConnectionUser.GetSettingInt("DefaultMap", _DefaultMapColour.ToArgb(), conn));
                _FineGridLinesColour   = Color.FromArgb(SQLiteConnectionUser.GetSettingInt("MapColour_FineGridLines", _FineGridLinesColour.ToArgb(), conn));
                _PlannedRouteColour    = Color.FromArgb(SQLiteConnectionUser.GetSettingInt("MapColour_PlannedRoute", _PlannedRouteColour.ToArgb(), conn));

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
        /// Raise the <see cref="PropertyChanged"/> event, and the individual 'NameOfProperty'Changed event.
        /// </summary>
        /// <param name="propertyName">The property that has changed.</param>
        /// <param name="eventHandler">The 'NameOfProperty'Changed event handler corresponding to the <paramref name="propertyName"/>.</param>
        /// <param name="newValue">The new value of the property.</param>
        private void OnPropertyChanged<T>(string propertyName, EventHandler<T> eventHandler, T newValue)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(propertyName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (eventHandler != null)
            {
                foreach (EventHandler<T> hdlr in eventHandler.GetInvocationList())
                {
                    var sync = hdlr.Target as ISynchronizeInvoke;
                    if (sync != null && sync.InvokeRequired)
                        sync.Invoke(hdlr, new object[] { this, newValue });
                    else
                        hdlr.Invoke(this, newValue);
                }
            }
        }

        #region SetT() overrides

        /// <summary>
        /// Update a <paramref name="field"/> of type <see cref="bool"/> to the provided <paramref name="value"/>, save it to
        /// the backend using the provided <paramref name="backendName"/>, and raise the neccessary events.
        /// </summary>
        /// <param name="field">The field to save the <paramref name="value"/> to.</param>
        /// <param name="value">The new value to assign to the <paramref name="field"/>.</param>
        /// <param name="handler">The <see cref="EventHandler"/> cooresponding to this property.</param>
        /// <param name="backendName">The name that this field will use in backend storage.</param>
        /// <param name="propertyName">The name of the property that has changed.</param>
        private bool SetBool(ref bool field, bool value, EventHandler<bool> handler,
            string backendName = null, [CallerMemberName] string propertyName = null)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(propertyName));
            if (field != value)
            {
                field = value;
                if (backendName == null)
                    SQLiteConnectionUser.PutSettingBool(propertyName, value);
                else
                    SQLiteConnectionUser.PutSettingBool(backendName, value);
                OnPropertyChanged(propertyName, handler, value);
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
        /// <param name="handler">The <see cref="EventHandler"/> cooresponding to this property.</param>
        /// <param name="backendName">The name that this field will use in backend storage.</param>
        /// <param name="propertyName">The name of the property that has changed.</param>
        private bool SetColor(ref Color field, Color value, EventHandler<Color> handler,
            string backendName = null, [CallerMemberName] string propertyName = null)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(propertyName));
            if (field.ToArgb() != value.ToArgb())
            {
                field = value;
                if (backendName == null)
                    SQLiteConnectionUser.PutSettingInt(propertyName, value.ToArgb());
                else
                    SQLiteConnectionUser.PutSettingInt(backendName, value.ToArgb());
                OnPropertyChanged(propertyName, handler, value);
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
        /// <param name="handler">The <see cref="EventHandler"/> cooresponding to this property.</param>
        /// <param name="backendName">The name that this field will use in backend storage.</param>
        /// <param name="propertyName">The name of the property that has changed.</param>
        private bool SetInt(ref int field, int value, EventHandler<int> handler,
            string backendName = null, [CallerMemberName] string propertyName = null)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(propertyName));
            if (field != value)
            {
                field = value;
                if (backendName == null)
                    SQLiteConnectionUser.PutSettingInt(propertyName, value);
                else
                    SQLiteConnectionUser.PutSettingInt(backendName, value);
                OnPropertyChanged(propertyName, handler, value);
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
        /// <param name="handler">The <see cref="EventHandler"/> cooresponding to this property.</param>
        /// <param name="backendName">The name that this field will use in backend storage.</param>
        /// <param name="propertyName">The name of the property that has changed.</param>
        private bool SetString(ref string field, string value, EventHandler<string> handler,
            string backendName = null, [CallerMemberName] string propertyName = null)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(propertyName));
            if (!field.Equals(value))
            {
                field = value;
                if (backendName == null)
                    SQLiteConnectionUser.PutSettingString(propertyName, value);
                else
                    SQLiteConnectionUser.PutSettingString(backendName, value);
                OnPropertyChanged(propertyName, handler, value);
                return true;
            }
            else
                return false;
        }

        #endregion // SetT() overrides

        #endregion // Private implementation
    }
}
