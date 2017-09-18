/*
 * Copyright © 2016 EDDiscovery development team
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
using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// same code as before

namespace EDDiscovery.ScreenShots
{
    public class ScreenshotDirectoryWatcher : IDisposable
    {
        private IDiscoveryController _discoveryForm;
        private Action<Action<ScreenShotImageConverter>> paramscallback;
        private FileSystemWatcher watchfolder = null;
        private string watchedfolder = null;
        private ConcurrentDictionary<string, System.Threading.Timer> ScreenshotTimers = new ConcurrentDictionary<string, System.Threading.Timer>();
        private string EDPicturesDir;
        private int LastJournalCmdr = Int32.MinValue;
        private JournalLocOrJump LastJournalLoc;

        public event Action<ScreenShotImageConverter> OnScreenshot;

        public ScreenshotDirectoryWatcher(IDiscoveryController controller, Action<Action<ScreenShotImageConverter>> paramscallback)
        {
            this.paramscallback = paramscallback;
            this._discoveryForm = controller;
            string ScreenshotsDirdefault = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Frontier Developments", "Elite Dangerous");
            string OutputDirdefault = Path.Combine(ScreenshotsDirdefault, "Converted");
            this.EDPicturesDir = ScreenshotsDirdefault;
            _discoveryForm.OnNewJournalEntry += NewJournalEntry;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Stop();
                _discoveryForm.OnNewJournalEntry -= NewJournalEntry;
            }
        }

        public bool Start(string folder, string ext)
        {
            this.Stop();

            this.watchedfolder = folder;
            return StartWatcher(folder, ext);
        }

        public void Stop()
        {
            if (watchfolder != null)
            {
                watchfolder.Dispose();
                watchfolder = null;
            }

            this.watchedfolder = null;
        }

        private bool StartWatcher(string watchedfolder, string ext)
        {
            if (watchfolder != null)                           // if there, delete
            {
                watchfolder.EnableRaisingEvents = false;
                watchfolder = null;
            }

            if (Directory.Exists(watchedfolder))
            {
                watchfolder = new System.IO.FileSystemWatcher();
                watchfolder.Path = watchedfolder;

                watchfolder.Filter = "*." + ext;
                watchfolder.NotifyFilter = NotifyFilters.FileName;
                watchfolder.Created += watcher;
                watchfolder.EnableRaisingEvents = true;

                _discoveryForm.LogLine("Scanning for " + ext + " screenshots in " + watchedfolder);
                return true;
            }
            else
            {
                _discoveryForm.LogLineHighlight("Folder specified for image conversion does not exist, check settings in the Screenshots tab");
            }

            return false;
        }

        private void NewJournalEntry(JournalEntry je)
        {
            if (je is JournalLocOrJump)
            {
                LastJournalCmdr = je.CommanderId;
                LastJournalLoc = je as JournalLocOrJump;
            }
            else
            {
                if (je.CommanderId != LastJournalCmdr)
                {
                    LastJournalLoc = null;
                    LastJournalCmdr = je.CommanderId;
                }
            }

            if (je.EventTypeID == JournalTypeEnum.Screenshot)
            {
                JournalScreenshot ss = je as JournalScreenshot;
                this.paramscallback?.Invoke(cp => ProcessScreenshot(ss.Filename, ss.System, ss, ss.CommanderId, cp));
            }
        }

        private void watcher(object sender, System.IO.FileSystemEventArgs e)
        {
            this.paramscallback?.Invoke(cp => ProcessFilesystemEvent(sender, e, cp));
        }

        private void ProcessFilesystemEvent(object sender, System.IO.FileSystemEventArgs e, ScreenShotImageConverter cp)
        {
            int cmdrid = LastJournalCmdr;

            if (e.FullPath.ToLowerInvariant().EndsWith(".bmp"))
            {
                if (!ScreenshotTimers.ContainsKey(e.FullPath))
                {
                    System.Threading.Timer timer = new System.Threading.Timer(s => ProcessScreenshot(e.FullPath, null, null, cmdrid, cp), null, 5000, System.Threading.Timeout.Infinite);

                    // Destroy the timer if OnScreenshot was run between the above check and adding the timer to the dictionary
                    if (!ScreenshotTimers.TryAdd(e.FullPath, timer))
                    {
                        timer.Dispose();
                    }
                }
            }
            else
            {
                ProcessScreenshot(e.FullPath, null, null, cmdrid, cp);
            }
        }

        // called thru CalLWithConverter in UI main class.. that pases a ImageConverter to us
        // sysname and or ss can be null if it was picked up by a watcher not the new journal screenshot entry

        private void ProcessScreenshot(string filename, string sysname, JournalScreenshot ss, int cmdrid, ScreenShotImageConverter cp)
        {
            System.Threading.Timer timer = null;

            if (sysname == null)
            {
                if (LastJournalLoc != null)
                {
                    sysname = LastJournalLoc.StarSystem;
                }
                else if (cmdrid >= 0)
                {
                    LastJournalLoc = JournalEntry.GetLast<JournalLocOrJump>(cmdrid, DateTime.UtcNow);
                    if (LastJournalLoc != null)
                    {
                        sysname = LastJournalLoc.StarSystem;
                    }
                }
            }

            if (sysname == null)
            {
                HistoryEntry he = _discoveryForm.history.GetLastFSD;
                sysname = (he != null) ? he.System.name : "Unknown System";
            }

            try
            {
                cp.InputFilename = filename;
                cp.SystemName = sysname;
                cp.JournalScreenShot = ss;
                cp.CommanderID = cmdrid;

                bool converted = false;

                using (Bitmap bmp = cp.GetScreenshot(ref filename, _discoveryForm.LogLine))
                {
                    // Don't run if OnScreenshot has already run for this image
                    if ((ScreenshotTimers.TryGetValue(filename, out timer) && timer == null) || (!ScreenshotTimers.TryAdd(filename, null) && !ScreenshotTimers.TryUpdate(filename, null, timer)))
                        return;

                    if (timer != null)
                        timer.Dispose();

                    converted = cp.Convert(bmp, _discoveryForm.LogLine);
                }

                if (converted && cp.RemoveInputFile)         // if remove, delete original picture
                {
                    ScreenshotTimers.TryRemove(filename, out timer);

                    try
                    {
                        File.Delete(filename);
                    }
                    catch
                    {
                        System.Diagnostics.Trace.WriteLine($"Unable to remove file {filename}");
                    }
                }

                this.OnScreenshot?.Invoke(cp);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception watcher: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);

                _discoveryForm.LogLineHighlight("Error in executing image conversion, try another screenshot, check output path settings. (Exception " + ex.Message + ")");
            }
        }
    }

}
