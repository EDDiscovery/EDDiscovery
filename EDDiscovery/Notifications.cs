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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EDDiscovery
{
    static class Notifications
    {
        public class NotificationParas
        {
            public string Text;
            public string Caption;
        }

        public class Notification
        {
            public DateTime StartUTC;
            public DateTime EndUTC;
            public string VersionMin;
            public string VersionMax;
            public string[] actionpackpresent;  // any is present to trigger
            public string[] actionpackpresentenabled;  // any is present and enabled to trigger
            public string[] actionpackpresentdisabled;  // any is present and disabled
            public string[] actionpacknotpresent;  // any is not present to trigger
            public string EntryType;
            public float PointSize;
            public bool HighLight;
            public Dictionary<string, NotificationParas> ParaStrings;

            public NotificationParas Select(string lang)
            {
                return ParaStrings.ContainsKey(lang) ? ParaStrings[lang] : (ParaStrings.ContainsKey("en") ? ParaStrings["en"] : null);
            }
        }

        static public List<Notification> ReadNotificationsFile(string notfile)
        {
            List<Notification> notes = new List<Notification>();

            try
            {       // protect against rouge xml
                XElement items = XElement.Load(notfile);
                if (items.Name != "Items")
                    return notes;

                foreach (XElement toplevel in items.Elements())
                {
                    //System.Diagnostics.Debug.WriteLine("Item " + toplevel.Name);

                    if (toplevel.Name == "Notifications")
                    {
                        foreach (XElement entry in toplevel.Elements())
                        {
                            try
                            {       // protect each notification from each other..

                               // System.Diagnostics.Debug.WriteLine(" Entry " + entry.Name);

                                Notification n = new Notification();
                                n.StartUTC = DateTime.Parse(entry.Attribute("StartUTC").Value, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AdjustToUniversal);
                                n.EndUTC = entry.Attribute("EndUTC") != null ? DateTime.Parse(entry.Attribute("EndUTC").Value, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AdjustToUniversal) : DateTime.MaxValue;

                                n.EntryType = entry.Attribute("Type").Value;

                                n.PointSize = entry.Attribute("PointSize") != null ? entry.Attribute("PointSize").Value.InvariantParseFloat(12) : -1;
                                n.HighLight = entry.Attribute("Highlight") != null && entry.Attribute("Highlight").Value == "Yes";

                                if (entry.Attribute("VersionMax") != null)
                                    n.VersionMax = entry.Attribute("VersionMax").Value;

                                if (entry.Attribute("VersionMin") != null)
                                    n.VersionMin = entry.Attribute("VersionMin").Value;

                                if (entry.Attribute("ActionPackPresent") != null)
                                    n.actionpackpresent = entry.Attribute("ActionPackPresent").Value.Split(',');

                                if (entry.Attribute("ActionPackNotPresent") != null)
                                    n.actionpacknotpresent = entry.Attribute("ActionPackNotPresent").Value.Split(',');

                                if (entry.Attribute("ActionPackPresentEnabled") != null)
                                    n.actionpackpresentenabled = entry.Attribute("ActionPackPresentEnabled").Value.Split(',');

                                if (entry.Attribute("ActionPackPresentDisabled") != null)
                                    n.actionpackpresentdisabled = entry.Attribute("ActionPackPresentDisabled").Value.Split(',');

                                n.ParaStrings = new Dictionary<string, NotificationParas>();

                                foreach (XElement body in entry.Elements())
                                {
                                    string lang = body.Attribute("Lang").Value;
                                    n.ParaStrings[lang] = new NotificationParas() { Text = body.Value, Caption = body.Attribute("Caption").Value};

                                   // System.Diagnostics.Debug.WriteLine("    " + body.Attribute("Lang").Value + " Body " + body.Value);
                                }

                                notes.Add(n);
                            }
                            catch { };
                        }
                    }
                }
            }
            catch { }

            return notes;
        }

        static public Task CheckForNewNotifications(Action<List<Notification>> callbackinthread)
        {
            return Task.Factory.StartNew(() =>
            {
                bool check = EDDOptions.Instance.CheckGithubFiles;
                string notificationsdir = EDDOptions.Instance.NotificationsAppDirectory();

                if (check)      // if download from github first..
                {
                    BaseUtils.GitHubClass github = new BaseUtils.GitHubClass(EDDiscovery.Properties.Resources.URLGithubDataDownload);

                    var gitfiles = github.ReadDirectory("Notifications");

                    if (gitfiles != null)        // may be empty, unlikely, but
                    {
                        check = github.DownloadFiles(gitfiles, notificationsdir);
                    }
                }

                // always go thru what we have in that folder.. 
                FileInfo[] allfiles = Directory.EnumerateFiles(notificationsdir, "*.xml", SearchOption.TopDirectoryOnly).Select(f => new System.IO.FileInfo(f)).OrderByDescending(p => p.LastWriteTime).ToArray();

                List<Notification> nlist = new List<Notification>();

                foreach (FileInfo f in allfiles )       // process all files found..
                {
                    var list = ReadNotificationsFile(f.FullName);
                    nlist.AddRange(list);
                }

                if ( nlist.Count>0)                     // if there are any, indicate..
                {
                    nlist.Sort(delegate (Notification left, Notification right)     // in order, oldest first
                    {
                        return left.StartUTC.CompareTo(right.StartUTC);
                    });

                    callbackinthread?.Invoke(nlist);
                }
            });
        }
    }
}
