/*
 * Copyright © 2021 - 2022 EDDiscovery development team
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
using System.IO;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlSpansh : UserControlWebBrowser
    {
        private FileSystemWatcher m_Watcher;
        private Timer waitforaccesstimer;
        private BaseUtils.MSTicks waittimertimeout = new BaseUtils.MSTicks();
        const int FileTimeout = 10000;
        private string newfiledetected;
        private HashSet<string> detectedfiles = new HashSet<string>();

        protected override void Init()
        {
            Init("Spansh", "https://spansh.co.uk");

            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 6)
            {
                string path = BaseUtils.Win32.UnsafeNativeMethods.KnownFolderPath(BaseUtils.Win32.UnsafeNativeMethods.Win32FolderId_Downloads);

                if ( path != null )
                {
                    m_Watcher = new System.IO.FileSystemWatcher();
                    m_Watcher.Path = path + Path.DirectorySeparatorChar;
                    m_Watcher.Filter = "*.csv";
                    m_Watcher.IncludeSubdirectories = false;
                    m_Watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.Size;
                    m_Watcher.Changed += new FileSystemEventHandler(OnNewFileChanged);
                    m_Watcher.Renamed += new RenamedEventHandler(OnNewFileRenamed);
                    m_Watcher.Created += new FileSystemEventHandler(OnNewFileCreated);
                    m_Watcher.EnableRaisingEvents = true;

                    System.Diagnostics.Trace.WriteLine($"{BaseUtils.AppTicks.TickCount} Spansh Start Monitor on {path}");

                    waitforaccesstimer = new Timer();
                    waitforaccesstimer.Interval = 500;
                    waitforaccesstimer.Tick += Waitforaccesstimer_Tick;
                }
            }
        }

        protected override void Closing()
        {
            base.Closing();

            if (m_Watcher != null)
            {
                m_Watcher.EnableRaisingEvents = false;
                m_Watcher.Dispose();
                m_Watcher = null;
            }

            waitforaccesstimer.Stop();
        }

        private void OnNewFileChanged(object sender, FileSystemEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Spansh new file changed {e.FullPath}");
            CheckFile(e.FullPath);
        }
        private void OnNewFileCreated(object sender, FileSystemEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Spansh new file created {e.FullPath}");
            CheckFile(e.FullPath);
        }
        private void OnNewFileRenamed(object sender, RenamedEventArgs e)        // Some seem to do renames of downloaded temp file
        {
            System.Diagnostics.Debug.WriteLine($"Spansh new file renamed {e.FullPath}");
            CheckFile(e.FullPath);
        }

        // this is in another thread
        // this can kick in before any data has had time to be written to it...
        private void CheckFile(string filename)
        {
            string[] prefixes = new string[] { "neutron", "ammonia", "earth", "tourist", "fleet", "exact", "exobiology" };

            if ( prefixes.StartsWith(Path.GetFileName(filename))>=0)
            {
                if ( detectedfiles.Contains(filename))
                {
                    System.Diagnostics.Debug.WriteLine($"Spansh already detected {filename}");
                }
                else if (newfiledetected == null)
                {
                    newfiledetected = filename;
                    detectedfiles.Add(newfiledetected);
                    System.Diagnostics.Debug.WriteLine($"Spansh detects new csv file {newfiledetected}");

                    BeginInvoke((MethodInvoker)delegate
                    {
                        waittimertimeout.TimeoutAt(FileTimeout);
                        waitforaccesstimer.Start();     // need to do this in a UI thread
                    });
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Spansh ignore new file too quick {filename}");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Spansh ignore file name {filename}");
            }
        }

        private void Waitforaccesstimer_Tick(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Spansh tick");
            if ( BaseUtils.FileHelpers.IsFileAvailable(newfiledetected))
            {
                System.Diagnostics.Debug.WriteLine($"Spansh detects csv file ready {newfiledetected}");
                waitforaccesstimer.Stop();
                var req = new UserControlCommonBase.PanelAction() { Action = PanelAction.ImportCSV, Data = newfiledetected };
                bool serviced = RequestPanelOperation(this, req) != PanelActionState.NotHandled;
                if ( !serviced) // no-one serviced it, so create an expedition tab, and then reissue
                {
                    DiscoveryForm.SelectTabPage("Expedition", true, false);         // ensure expedition is open
                    RequestPanelOperation(this, req);
                }
                newfiledetected = null;
            }
            else if ( waittimertimeout.TimedOut )
            {
                System.Diagnostics.Debug.WriteLine($"Spansh timeout waiting for {newfiledetected}");
                waitforaccesstimer.Stop();
                newfiledetected = null;
            }
        }
    }
}
