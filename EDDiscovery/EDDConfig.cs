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
using System.Drawing;

namespace EDDiscovery
{
    public class EDDConfig : EliteDangerousCore.IEliteConfig
    {
        private static EDDConfig instance;

        private EDDConfig()
        {
        }

        public static EDDConfig Instance            // Singleton pattern
        {
            get
            {
                if (instance == null)
                {
                    instance = new EDDConfig();
                    EliteDangerousCore.EliteConfigInstance.InstanceConfig = instance;        // hook up so classes can see this which use this IF
                }
                return instance;
            }
        }

        #region Discrete Controls

        private bool useNotifyIcon = false;
        private bool orderrowsinverted = false;
        private bool minimizeToNotifyIcon = false;
        private bool keepOnTop = false; /**< Whether to keep the windows on top or not */
        private bool displayUTC = false;
        private bool showuievents = false;
        private System.Windows.Forms.Keys clickthrukey = System.Windows.Forms.Keys.ShiftKey;
        private string defaultwavedevice = "Default";
        private string defaultvoicedevice = "Default";
        private bool edsmeddbdownload = true;
        private string edsmgridids = "All";
        private int fullhistoryloaddaylimit = 0;     //0 means not in use
        private string language = "Auto";
        private bool drawduringresize = true;
        private bool sortpanelsalpha = false;
        private string essentialeventtype = "Default";
        private string coriolisURL = "";
        private string eddshipyardURL = "";
        private string eddbsystemsurl = "";
        private string edsmfullsystemsurl = "";
        Dictionary<string, Image> captainslogtaglist;

        /// <summary>
        /// Controls whether or not a system notification area (systray) icon will be shown.
        /// </summary>
        public bool UseNotifyIcon
        {
            get
            {
                return useNotifyIcon;
            }
            set
            {
                useNotifyIcon = value;
                SQLiteConnectionUser.PutSettingBool("UseNotifyIcon", value);
            }
        }

        public bool OrderRowsInverted
        {
            get
            {
                return orderrowsinverted;
            }
            set
            {
                orderrowsinverted = value;
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
                return minimizeToNotifyIcon;
            }
            set
            {
                minimizeToNotifyIcon = value;
                SQLiteConnectionUser.PutSettingBool("MinimizeToNotifyIcon", value);
            }
        }

        public bool KeepOnTop
        {
            get
            {
                return keepOnTop;
            }
            set
            {
                keepOnTop = value;
                SQLiteConnectionUser.PutSettingBool("KeepOnTop", value);
            }
        }

        public bool DisplayUTC
        {
            get
            {
                return displayUTC;
            }
            set
            {
                displayUTC = value;
                SQLiteConnectionUser.PutSettingBool("DisplayUTC", value);
            }
        }

        public string DefaultWaveDevice
        {
            get
            {
                return defaultwavedevice;
            }
            set
            {
                defaultwavedevice = value;
                SQLiteConnectionUser.PutSettingString("WaveAudioDevice", value);
            }
        }

        public string DefaultVoiceDevice
        {
            get
            {
                return defaultvoicedevice;
            }
            set
            {
                defaultvoicedevice = value;
                SQLiteConnectionUser.PutSettingString("VoiceAudioDevice", value);
            }
        }

        public bool ShowUIEvents
        {
            get
            {
                return showuievents;
            }
            set
            {
                showuievents = value;
                SQLiteConnectionUser.PutSettingBool("ShowUIEvents", value);
            }
        }

        public System.Windows.Forms.Keys ClickThruKey
        {
            get
            {
                return clickthrukey;
            }
            set
            {
                clickthrukey = value;
                SQLiteConnectionUser.PutSettingInt("ClickThruKey", (int)value);
            }
        }

        public bool EDSMEDDBDownload
        {
            get
            {
                return edsmeddbdownload;
            }
            set
            {
                edsmeddbdownload = value;
                SQLiteConnectionUser.PutSettingBool("EDSMEDDBDownloadData", value);
            }
        }

        public string EDSMGridIDs
        {
            get
            {
                return edsmgridids;
            }
            set
            {
                edsmgridids = value;
                SQLiteConnectionSystem.PutSettingString("EDSMGridIDs", value);
            }
        }

        public int FullHistoryLoadDayLimit          // 0 = full load.
        {
            get
            {
                return fullhistoryloaddaylimit;
            }
            set
            {
                fullhistoryloaddaylimit = value;
                SQLiteConnectionUser.PutSettingInt("FullHistoryLoadDayLimit", value);
            }
        }

        public string EssentialEventTypes
        {
            get
            {
                return essentialeventtype;
            }
            set
            {
                essentialeventtype = value;
                SQLiteConnectionUser.PutSettingString("EssentialEventType", value);
            }
        }

        public string CoriolisURL
        {
            get
            {
                return coriolisURL;
            }
            set
            {
                coriolisURL = value;
                SQLiteConnectionUser.PutSettingString("CorolisURL", value);
            }
        }

        public string EDDShipyardURL
        {
            get
            {
                return eddshipyardURL;
            }
            set
            {
                eddshipyardURL = value;
                SQLiteConnectionUser.PutSettingString("EDDShipyardURL", value);
            }
        }

        public string Language         // as standard culture en-gb or en etc, or Auto
        {
            get
            {
                return language;
            }
            set
            {
                language = value;
                SQLiteConnectionUser.PutSettingString("DefaultLanguage", value);
            }
        }

        public bool DrawDuringResize
        {
            get
            {
                return drawduringresize;
            }
            set
            {
                drawduringresize = value;
                SQLiteConnectionUser.PutSettingBool("DrawDuringResizeWindow", value);
            }
        }

        public bool SortPanelsByName
        {
            get
            {
                return sortpanelsalpha;
            }
            set
            {
                sortpanelsalpha = value;
                SQLiteConnectionUser.PutSettingBool("PanelsSortedByName", value); 
            }
        }

        public string EDSMFullSystemsURL   
        {
            get
            {
                if (edsmfullsystemsurl == "Default")
                    return Properties.Resources.URLEDSMFullSystems;
                else
                    return edsmfullsystemsurl;
            }
            set
            {
                edsmfullsystemsurl = value;
                SQLiteConnectionUser.PutSettingString("EDSMFullSystemsURL", value);
            }
        }

        public string EDDBSystemsURL
        {
            get
            {
                if (edsmfullsystemsurl == "Default")
                    return Properties.Resources.URLEDDBSystems;
                else
                    return eddbsystemsurl;
            }
            set
            {
                eddbsystemsurl = value;
                SQLiteConnectionUser.PutSettingString("EDDBSystemsURL", value);
            }
        }

        public string CaptainsLogTags       // get/set as string..
        {
            get
            {
                string[] list = (from x in captainslogtaglist select (x.Key + "=" + (string)x.Value.Tag)).ToArray();
                return string.Join(";", list);
            }
            set
            {
                captainslogtaglist = new Dictionary<string, Image>();       // read the value, and look up icons, create the table..

                string[] tagdefs = value.Split(';');
                foreach (var s in tagdefs)
                {
                    string[] parts = s.Split('=');
                    // valid number, valid length, image exists
                    if (parts.Length == 2 && parts[0].Length > 0 && parts[1].Length > 0 && EDDiscovery.Icons.IconSet.Icons.ContainsKey(parts[1]))
                    {
                        Image img = EDDiscovery.Icons.IconSet.Icons[parts[1]];      // image.tag has name - defined by icon system
                        captainslogtaglist[parts[0]] = img;
                    }
                }

                // write back what is correct. Incorrect icons will be removed.
                string[] list = (from x in captainslogtaglist select (x.Key + "=" + (string)x.Value.Tag)).ToArray();
                string setting = string.Join(";", list);
                SQLiteConnectionUser.PutSettingString("CaptainsLogPanelTagNames", setting);
            }
        }

        public Dictionary<string,Image> CaptainsLogTagImage // set as dictionary/string
        {
            get
            {
                return captainslogtaglist;
            }
            set
            {
                captainslogtaglist = value;
                string[] list = (from x in captainslogtaglist select (x.Key + "=" + (string)x.Value.Tag)).ToArray();
                string setting = string.Join(";", list);
                SQLiteConnectionUser.PutSettingString("CaptainsLogPanelTagNames", setting);
            }
        }

        #endregion

        #region Update at start

        public void Update(bool write = true, SQLiteConnectionUser conn = null)     // call at start to populate above
        {
            try
            {
                useNotifyIcon = SQLiteConnectionUser.GetSettingBool("UseNotifyIcon", false, conn);
                orderrowsinverted = SQLiteConnectionUser.GetSettingBool("OrderRowsInverted", false, conn);
                minimizeToNotifyIcon = SQLiteConnectionUser.GetSettingBool("MinimizeToNotifyIcon", false, conn);
                keepOnTop = SQLiteConnectionUser.GetSettingBool("KeepOnTop", false, conn);
                displayUTC = SQLiteConnectionUser.GetSettingBool("DisplayUTC", false, conn);
                defaultvoicedevice = SQLiteConnectionUser.GetSettingString("VoiceAudioDevice", "Default", conn);
                defaultwavedevice = SQLiteConnectionUser.GetSettingString("WaveAudioDevice", "Default", conn);
                showuievents = SQLiteConnectionUser.GetSettingBool("ShowUIEvents", false, conn);
                clickthrukey = (System.Windows.Forms.Keys)SQLiteConnectionUser.GetSettingInt("ClickThruKey", (int)System.Windows.Forms.Keys.ShiftKey, conn);
                edsmeddbdownload = SQLiteConnectionUser.GetSettingBool("EDSMEDDBDownloadData", true, conn);    // this goes with the USER on purpose, so its kept over a system db delete
                edsmgridids = SQLiteConnectionSystem.GetSettingString("EDSMGridIDs", "Not Set"); // from system database, not user, to keep setting with system data
                fullhistoryloaddaylimit = SQLiteConnectionUser.GetSettingInt("FullHistoryLoadDayLimit", 0);
                language = SQLiteConnectionUser.GetSettingString("DefaultLanguage", "Auto");
                drawduringresize = SQLiteConnectionUser.GetSettingBool("DrawDuringResizeWindow", true);
                sortpanelsalpha = SQLiteConnectionUser.GetSettingBool("PanelsSortedByName", false);
                essentialeventtype = SQLiteConnectionUser.GetSettingString("EssentialEventType", "Default");
                coriolisURL = SQLiteConnectionUser.GetSettingString("CorolisURL", Properties.Resources.URLCoriolis);
                eddshipyardURL = SQLiteConnectionUser.GetSettingString("EDDShipyardURL", Properties.Resources.URLEDShipyard);
                edsmfullsystemsurl = SQLiteConnectionUser.GetSettingString("EDSMFullSystemsURL", "Default");
                eddbsystemsurl = SQLiteConnectionUser.GetSettingString("EDDBSystemsURL", "Default");
                CaptainsLogTags = SQLiteConnectionUser.GetSettingString("CaptainsLogPanelTagNames", "Expedition=Journal.FSDJump;Died=Journal.Died");

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

        private EliteDangerousCore.ISystem homeSystem = null;

        public EliteDangerousCore.ISystem HomeSystem
        {
            get
            {
                if (homeSystem == null)
                    homeSystem = SystemClassDB.GetSystem(SQLiteDBClass.GetSettingString("DefaultMapCenter", "Sol"));
                if ( homeSystem == null )
                    homeSystem = new EliteDangerousCore.SystemClass("Sol", 0, 0, 0);
                return homeSystem;
            }
            set
            {
                if (value != null && value.HasCoordinate)
                {
                    SQLiteDBClass.PutSettingString("DefaultMapCenter", value.Name);
                    homeSystem = value;
                }
            }
        }

        private float? mapzoom;

        public float MapZoom
        {
            get
            {
                if (mapzoom == null)
                    mapzoom = (float)SQLiteDBClass.GetSettingDouble("DefaultMapZoom", 1.0);
                return mapzoom.Value;
            }

            set
            {
                //SQLiteDBClass.PutSettingDouble("DefaultMapZoom", Double.TryParse(textBoxDefaultZoom.Text, out zoom) ? zoom : 1.0);

                SQLiteDBClass.PutSettingDouble("DefaultMapZoom", value);
                mapzoom = value;
            }

        }

        private bool? mapcentreonselection;

        public bool MapCentreOnSelection
        {
            get
            {
                if (mapcentreonselection == null)
                    mapcentreonselection = SQLiteDBClass.GetSettingBool("CentreMapOnSelection", true);
                return mapcentreonselection.Value;
            }

            set
            {
                SQLiteDBClass.PutSettingBool("CentreMapOnSelection", value);
                mapcentreonselection = value;
            }
        }
    }
}
