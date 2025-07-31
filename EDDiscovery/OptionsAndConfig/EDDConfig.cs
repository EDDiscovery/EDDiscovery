﻿/*
 * Copyright © 2015 - 2024 EDDiscovery development team
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
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

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
        private int displayTimeFormat = 0; //0=local,1=utc,2=elite time
        private System.Windows.Forms.Keys clickthrukey = System.Windows.Forms.Keys.ShiftKey;
        private string defaultwavedevice = "Default";
        private string defaultvoicedevice = "Default";
        private bool systemdbdownload = true;
        private int fullhistoryloaddaylimit = 0;     //0 means not in use
        private string language = "Auto";
        private bool drawduringresize = true;
        private bool sortpanelsalpha = false;
        private string essentialeventtype = "Default";
        private string coriolisURL = "";
        private string eddshipyardURL = "";
        private string edsmfullsystemsurl = "";
        private string spanshsystemsurl = "";
        private int webserverport = 6502;
        private bool webserverenable = false;
        private string dlluserpanelsregisteredlist = "";
        Dictionary<string, string> captainslogtaglist;
        Dictionary<string, string> bookmarkstaglist;

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
                EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("UseNotifyIcon", value);
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
                EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("OrderRowsInverted", value);
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
                EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("MinimizeToNotifyIcon", value);
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
                EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("KeepOnTop", value);
            }
        }

        public string GetTimeTitle()
        {
            if (displayTimeFormat == 2)
                return "Game Time".Tx();
            else if (displayTimeFormat == 1)
                return "UTC";
            else
                return "Time".Tx();
        }

        public DateTime ConvertTimeToSelectedFromUTC(DateTime t)        // from UTC->Display format
        {
            System.Diagnostics.Debug.Assert(t.Kind == DateTimeKind.Utc);
            if (displayTimeFormat == 1)     // UTC
            {
                if (!DateTimeInRangeForGame(t))
                    t = DateTime.UtcNow;
                return t;
            }
            else if (displayTimeFormat == 2)    // Game time
            {
                t = t.AddYears(1286);
                if (!DateTimeInRangeForGame(t))
                    t = DateTime.UtcNow.AddYears(1286);
                return t;
            }
            else                                 // local
            {
                if (!DateTimeInRangeForGame(t))
                    t = DateTime.Now;

                return t.ToLocalTime();
            }
        }

        public bool DateTimeInRangeForGame(DateTime t)
        {
            if (displayTimeFormat == 2)
                return t.Year >= 3300 && t.Year <= 4300;
            else
                return t.Year >= 2014 && t.Year <= 2999;
        }

        public DateTime EnsureTimeInRangeForGame(DateTime t)
        {
            if (!DateTimeInRangeForGame(t))
                t = ConvertTimeToSelectedFromUTC(DateTime.UtcNow);
            return t;
        }

        public DateTime SelectedEndOfTodayUTC()     // respecting the display time, what is the UTC of the end of today?
        {
            if (displayTimeFormat == 0)
            {
                //System.Diagnostics.Debug.WriteLine($"Now {DateTime.Now} {DateTime.Now.EndOfDay()} {DateTime.Now.EndOfDay().ToUniversalTime()}");
                return DateTime.Now.EndOfDay().ToUniversalTime();
            }
            else
                return DateTime.UtcNow.EndOfDay();
        }

        // place datetime into selected time.  UTC for UTC/Gametime, Local for local
        public DateTime ConvertTimeToSelected(DateTime t)
        {
            if (displayTimeFormat == 0)
                return t.ToLocalKind();     // to local, keeping the actual time
            else
                return t.ToUniversalKind(); // to universal, keeping the actual time
        }

        public DateTime ConvertTimeToUTCFromSelected(DateTime t)        // from selected format back to UTC
        {
            if (displayTimeFormat == 1) // UTC
            {
                System.Diagnostics.Debug.Assert(t.Kind == DateTimeKind.Utc);        // should be in UTC, direct to UTC
                return t;
            }
            else if (displayTimeFormat == 2)    // Gametime
            {
                System.Diagnostics.Debug.Assert(t.Kind == DateTimeKind.Utc);        // should be in UTC, convert from gametime to utc
                return t.AddYears(-1286);
            }
            else  // local
            {
                System.Diagnostics.Debug.Assert(t.Kind == DateTimeKind.Local);
                return t.ToUniversalTime();
            }
        }

        // picker is time format less (although its normally in UTC mode due to the way the value is picked up)
        public DateTime ConvertTimeToUTCFromPicker(DateTime t)
        {
            t = ConvertTimeToSelected(t);                           // put the correct Kind on it
            return ConvertTimeToUTCFromSelected(t);                // and convert to UTC
        }

        public bool DisplayTimeLocal
        {
            get
            {
                return displayTimeFormat == 0;
            }
        }

        public int DisplayTimeIndex //0= local, 1=UTC,2 = game time, backwards compatible with old DisplayUTC bool
        {
            get
            {
                return displayTimeFormat;
            }
            set
            {
                if (displayTimeFormat != value)
                {
                    displayTimeFormat = value;
                    EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("DisplayUTC", value);
                }
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
                if (defaultwavedevice != value)
                {
                    defaultwavedevice = value;
                    EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("WaveAudioDevice", value);
                }
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
                if (defaultvoicedevice != null)
                {
                    defaultvoicedevice = value;
                    EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("VoiceAudioDevice", value);
                }
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
                if (clickthrukey != value)
                {
                    clickthrukey = value;
                    EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("ClickThruKey", (int)value);
                }
            }
        }

        public bool SystemDBDownload        // do we download system DB data?
        {
            get
            {
                return systemdbdownload;
            }
            set
            {
                if (systemdbdownload != value )
                {
                    systemdbdownload = value;
                    EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("EDSMEDDBDownloadData", value); 
                }
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
                if (fullhistoryloaddaylimit != value)
                {
                    fullhistoryloaddaylimit = value;
                    EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("FullHistoryLoadDayLimit", value);
                }
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
                if (essentialeventtype != value)
                {
                    essentialeventtype = value;
                    EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("EssentialEventType", value);
                }
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
                if (coriolisURL != value)
                {
                    coriolisURL = value;
                    EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("CorolisURL", value);
                }
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
                if (eddshipyardURL != value)
                {
                    eddshipyardURL = value;
                    EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("EDDShipyardURL", value);
                }
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
                if (language != value)
                {
                    language = value;
                    EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("DefaultLanguage", value);
                }
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
                if (drawduringresize != value)
                {
                    drawduringresize = value;
                    EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("DrawDuringResizeWindow", value);
                }
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
                if (sortpanelsalpha != value)
                {
                    sortpanelsalpha = value;
                    EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("PanelsSortedByName", value);
                }
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
                if (edsmfullsystemsurl != value)
                {
                    edsmfullsystemsurl = value;
                    EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("EDSMFullSystemsURL", value);
                }
            }
        }

        public string SpanshSystemsURL
        {
            get
            {
                if (spanshsystemsurl == "Default")
                    return Properties.Resources.URLSpanshSystemsRoot;
                else
                    return spanshsystemsurl;
            }
            set
            {
                if (spanshsystemsurl != value)
                {
                    spanshsystemsurl = value;
                    EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("SpanshSystemsURL", value);
                }
            }
        }

        public const string TagSplitStringCL = ";"; // keeping this so its backwards compatible

        // get/set as tag list <separ>
        public string CaptainsLogTags       
        {
            get
            {
                string[] list = (from x in captainslogtaglist select (x.Key + "=" + x.Value)).ToArray();
                return string.Join(TagSplitStringCL, list);
            }
            set
            {
                SetImageDict(value, ref captainslogtaglist, "CaptainsLogPanelTagNames", TagSplitStringCL);
            }
        }

        // Image has Tag as its logical name (EDDICONS path)
        public Dictionary<string, string> CaptainsLogTagDictionary 
        {
            get
            {
                return captainslogtaglist;
            }
            set
            {
                captainslogtaglist = value;
                string[] list = (from x in captainslogtaglist select (x.Key + "=" + x.Value)).ToArray();
                string setting = string.Join(TagSplitStringCL, list) + TagSplitStringCL;        // nice to match with the terminator like the edit form does
                EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("CaptainsLogPanelTagNames", setting);
            }
        }

        public static string[] CaptainsLogTagArray(string tags)
        {
            return tags.SplitNoEmptyStartFinish(TagSplitStringCL[0]);
        }


        public const string TagSplitStringBK = "\u2345";

        // get/set as taglist <separ>
        public string BookmarkTags       
        {
            get
            {
                string[] list = (from x in bookmarkstaglist select (x.Key + "=" + x.Value)).ToArray();
                return string.Join(TagSplitStringBK, list);
            }
            set
            {
                SetImageDict(value, ref bookmarkstaglist, "BookmarkTagNames",TagSplitStringBK);
            }
        }

        // Image has Tag as its logical name (EDDICONS path)
        public Dictionary<string, string> BookmarkTagDictionary
        {
            get
            {
                return bookmarkstaglist;
            }
            set
            {
                bookmarkstaglist = value;
                string[] list = (from x in bookmarkstaglist select (x.Key + "=" + x.Value)).ToArray();
                string setting = string.Join(TagSplitStringBK, list) + TagSplitStringBK;
                EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("BookmarkTagNames", setting);
            }
        }

        public static string[] BookmarkTagArray(string tags)
        {
            return tags.SplitNoEmptyStartFinish(TagSplitStringBK[0]);
        }

        public int WebServerPort
        {
            get
            {
                return webserverport;
            }
            set
            {
                if (webserverport != value)
                {
                    webserverport = value;
                    EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("WebServerPort", webserverport);
                }
            }
        }

        public bool WebServerEnable
        {
            get
            {
                return webserverenable;
            }
            set
            {
                if (webserverenable != value)
                {
                    webserverenable = value;
                    EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("WebServerEnable", webserverenable);
                }
            }
        }

        private const string UserPanelSplitStr = "\u2737";

        public string DLLUserPanelsRegisteredList
        {
            get
            {
                return dlluserpanelsregisteredlist;
            }
            set
            {
                dlluserpanelsregisteredlist = value;
                EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("DLLUserPanelsRegisteredList", value);
            }
        }

        // return panelid of ID, or -1 if not found. Allow creation if required
        public int FindCreatePanelID(string id, bool createnew = true)
        {
            string[] registeredpanels = DLLUserPanelsRegisteredList.Split(UserPanelSplitStr, emptyarrayifempty: true); // split the string
            int indexof = Array.IndexOf(registeredpanels, id);  // find if there
            if (indexof >= 0)      // if there
                return PanelInformation.DLLUserPanelsStart + indexof;   // return it
            if ( createnew )
            {
                DLLUserPanelsRegisteredList = DLLUserPanelsRegisteredList.AppendPrePad(id, UserPanelSplitStr);  // write updated string back
                return PanelInformation.DLLUserPanelsStart + registeredpanels.Length;
            }

            return -1;
        }

        #endregion

        #region Update at start


        public void Update()     // call at start to populate above
        {
            try
            {
                useNotifyIcon = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting("UseNotifyIcon", false);
                orderrowsinverted = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting("OrderRowsInverted", false);
                minimizeToNotifyIcon = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting("MinimizeToNotifyIcon", false);
                keepOnTop = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting("KeepOnTop", false);
                displayTimeFormat = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting("DisplayUTC", 0);
                defaultvoicedevice = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting("VoiceAudioDevice", "Default");
                defaultwavedevice = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting("WaveAudioDevice", "Default");
                clickthrukey = (System.Windows.Forms.Keys)EliteDangerousCore.DB.UserDatabase.Instance.GetSetting("ClickThruKey", (int)System.Windows.Forms.Keys.ShiftKey);
                systemdbdownload = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting("EDSMEDDBDownloadData", true);    // this goes with the USER on purpose, so its kept over a system db delete
                fullhistoryloaddaylimit = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting("FullHistoryLoadDayLimit", 0);
                language = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting("DefaultLanguage", "Auto");
                drawduringresize = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting("DrawDuringResizeWindow", true);
                sortpanelsalpha = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting("PanelsSortedByName", true);
                essentialeventtype = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting("EssentialEventType", "Default");
                coriolisURL = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting("CorolisURL", Properties.Resources.URLCoriolis);
                eddshipyardURL = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting("EDDShipyardURL", Properties.Resources.URLEDShipyard);
                if (eddshipyardURL == "http://www.edshipyard.com/")     // 30/jul/19 changed address
                    EDDShipyardURL = "http://edsy.org/";

                edsmfullsystemsurl = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting("EDSMFullSystemsURL", "Default");
                spanshsystemsurl = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting("SpanshSystemsURL", "Default");

                CaptainsLogTags = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting("CaptainsLogPanelTagNames", "Expedition=Journal.FSDJump" + TagSplitStringCL + "Died=Journal.Died");
                BookmarkTags = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting("BookmarkTagNames", "Expedition=Journal.FSDJump" + TagSplitStringBK + "Died=Journal.Died");

                webserverport = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting("WebServerPort", 6502);
                webserverenable = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting("WebServerEnable", false);

                dlluserpanelsregisteredlist = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting("DLLUserPanelsRegisteredList", "");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("EDDConfig.Update()" + ":" + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }

        }

        #endregion

        #region Helpers

        private void SetImageDict(string value, ref Dictionary<string, string> dict, string settingname, string separ)
        {
            dict = new Dictionary<string, string>();       // read the value, and look up icons, create the table..

            string[] tagdefs = value.SplitNoEmptyStartFinish(separ[0]);
            foreach (var s in tagdefs)
            {
                string[] parts = s.Split('=');
                // valid number, valid length, image exists
                if (parts.Length == 2 && parts[0].Length > 0 && parts[1].Length > 0 && BaseUtils.Icons.IconSet.Instance.Contains(parts[1]))
                {
                    dict[parts[0]] = parts[1];
                }
            }

            // write back what is correct. Incorrect icons will be removed.
            string[] list = (from x in dict select (x.Key + "=" + x.Value)).ToArray();
            string setting = string.Join(separ, list) + separ;
            EliteDangerousCore.DB.UserDatabase.Instance.PutSetting(settingname, setting);
        }

        #endregion
    }
}
