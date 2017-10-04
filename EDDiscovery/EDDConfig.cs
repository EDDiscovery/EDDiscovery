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
        private bool _keepOnTop = false; /**< Whether to keep the windows on top or not */
        private bool _displayUTC = false;
        private bool _clearMaterials = false;
        private bool _clearCommodities = false;
        private bool _autoLoadPopouts = false;
        private bool _autoSavePopouts = false;
        private bool _showuievents = false;
        private System.Windows.Forms.Keys _clickthrukey = System.Windows.Forms.Keys.ShiftKey;
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

        public bool ShowUIEvents
        {
            get
            {
                return _showuievents;
            }
            set
            {
                _showuievents = value;
                SQLiteConnectionUser.PutSettingBool("ShowUIEvents", value);
            }
        }

        public System.Windows.Forms.Keys ClickThruKey
        {
            get
            {
                return _clickthrukey;
            }
            set
            {
                _clickthrukey = value;
                SQLiteConnectionUser.PutSettingInt("ClickThruKey", (int)value);
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
                _keepOnTop = SQLiteConnectionUser.GetSettingBool("KeepOnTop", false, conn);
                _displayUTC = SQLiteConnectionUser.GetSettingBool("DisplayUTC", false, conn);
                _clearCommodities = SQLiteConnectionUser.GetSettingBool("ClearCommodities", false, conn);
                _clearMaterials = SQLiteConnectionUser.GetSettingBool("ClearMaterials", false, conn);
                _autoLoadPopouts = SQLiteConnectionUser.GetSettingBool("AutoLoadPopouts", false, conn);
                _autoSavePopouts = SQLiteConnectionUser.GetSettingBool("AutoSavePopouts", false, conn);
                _defaultvoicedevice = SQLiteConnectionUser.GetSettingString("VoiceAudioDevice", "Default", conn);
                _defaultwavedevice = SQLiteConnectionUser.GetSettingString("WaveAudioDevice", "Default", conn);
                _showuievents = SQLiteConnectionUser.GetSettingBool("ShowUIEvents", false, conn);
                _clickthrukey = (System.Windows.Forms.Keys)SQLiteConnectionUser.GetSettingInt("ClickThruKey", (int)System.Windows.Forms.Keys.ShiftKey, conn);

                EliteDangerousCore.EDCommander.Load(write, conn);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("EDDConfig.Update()" + ":" + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }

        }

        #endregion

        int? defmapcolour;

        public int DefaultMapColour
        {
            get
            {
                if (defmapcolour == null)
                    defmapcolour = SQLiteConnectionUser.GetSettingInt("DefaultMap", System.Drawing.Color.Red.ToArgb());
                return defmapcolour.Value;
            }
            set
            {
                SQLiteConnectionUser.PutSettingInt("DefaultMap", value);
                defmapcolour = value;
            }
        }
    }
}
