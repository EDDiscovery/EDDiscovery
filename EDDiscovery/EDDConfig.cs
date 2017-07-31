﻿/*
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

 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Reflection;
using EliteDangerousCore.EDSM;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using EDDiscovery;
using EliteDangerousCore.DB;

namespace EDDiscovery
{
    public class EDDConfig : EliteDangerousCore.EliteConfig
    {
        private static EDDConfig _instance;

        private EDDConfig()
        {
        }

        public static EDDConfig Instance            // Singleton pattern
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EDDConfig();
                    EliteDangerousCore.EliteConfigInstance.InstanceConfig = _instance;        // hook up so classes can see this which use this IF
                }
                return _instance;
            }
        }

        #region Discrete Controls

        private bool _EDSMLog;
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

#region Update at start

        public void Update(bool write = true, SQLiteConnectionUser conn = null)     // call at start to populate above
        {
            try
            {
                _useNotifyIcon = SQLiteConnectionUser.GetSettingBool("UseNotifyIcon", false, conn);
                _EDSMLog = SQLiteConnectionUser.GetSettingBool("EDSMLog", false, conn);
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

                EliteDangerousCore.EDCommander.Load(write, conn);
                UserPaths.Load(conn);
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

        public class OptionsClass : EliteDangerousCore.EliteOptions
        {
            public string VersionDisplayString { get; private set; }
            public string AppFolder { get; private set; }
            public string AppDataDirectory { get; private set; }
            public string UserDatabasePath { get; private set; }
            public string SystemDatabasePath { get; private set; }
            public string OldDatabasePath { get; private set; }
            public bool StoreDataInProgramDirectory { get; private set; }
            public bool NoWindowReposition { get; private set; }
            public bool ActionButton { get; private set; }
            public bool NoLoad { get; private set; }
            public bool NoSystemsLoad { get; private set; }
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
                StringBuilder sb = new StringBuilder("Version " + EDDApplicationContext.AssemblyVersionNumber);

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
                        case "showactionbutton": ActionButton = true; break;
                        case "noload": NoLoad = true; break;
                        case "nosystems": NoSystemsLoad = true; break;
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

                EliteDangerousCore.EliteConfigInstance.InstanceOptions = this;
            }
        }

        public static OptionsClass Options { get; } = new OptionsClass();

#endregion

#region User Paths

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
            public string EDDirectory { get; set; }
            public string ImageHandlerOutputDir { get; set; }
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
                ImageHandlerScreenshotsDir = SQLiteConnectionUser.GetSettingString("ImageHandlerScreenshotsDir", null, conn);

                if (File.Exists(Path.Combine(EDDConfig.Options.AppDataDirectory, "UserPaths.json")))
                {
                    JObject jo;

                    using (FileStream stream = File.OpenRead(Path.Combine(EDDConfig.Options.AppDataDirectory, "UserPaths.json")))
                    {
                        using (StreamReader rdr = new StreamReader(stream))
                        {
                            using (JsonTextReader jrdr = new JsonTextReader(rdr))
                            {
                                jo = JObject.Load(jrdr);
                            }
                        }
                    }

                    EDDirectory = jo["EDDirectory"].Str(EDDirectory);
                    ImageHandlerOutputDir = jo["ImageHandlerOutputDir"].Str(ImageHandlerOutputDir);
                    ImageHandlerScreenshotsDir = jo["ImageHandlerScreenshotsDir"].Str(ImageHandlerScreenshotsDir);
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

                JObject jo = new JObject();


                jo["EDDirectory"] = EDDirectory;

                if (!string.IsNullOrEmpty(ImageHandlerOutputDir))
                    jo["ImageHandlerOutputDir"] = ImageHandlerOutputDir;

                if (!string.IsNullOrEmpty(ImageHandlerScreenshotsDir))
                    jo["ImageHandlerScreenshotsDir"] = ImageHandlerScreenshotsDir;


                using (FileStream stream = File.OpenWrite(Path.Combine(EDDConfig.Options.AppDataDirectory, "UserPaths.json.tmp")))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        using (JsonTextWriter jwriter = new JsonTextWriter(writer))
                        {
                            jo.WriteTo(jwriter);
                        }
                    }
                }

                File.Delete(Path.Combine(EDDConfig.Options.AppDataDirectory, "UserPaths.json"));
                File.Move(Path.Combine(EDDConfig.Options.AppDataDirectory, "UserPaths.json.tmp"), Path.Combine(EDDConfig.Options.AppDataDirectory, "UserPaths.json"));
            }

        #endregion
        }

        #endregion

    }
}
