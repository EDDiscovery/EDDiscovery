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
        private EDDiscoveryForm discoveryform;
        private Action<Action<ScreenShotImageConverter>> paramscallback;
        private FileSystemWatcher filesystemwatcher = null;

        private ConcurrentDictionary<string, System.Threading.Timer> ScreenshotTimers = new ConcurrentDictionary<string, System.Threading.Timer>(StringComparer.InvariantCultureIgnoreCase);
        private ConcurrentDictionary<string, JournalScreenshot> JournalScreenshotsByName = new ConcurrentDictionary<string, JournalScreenshot>(StringComparer.InvariantCultureIgnoreCase);
        
        private int LastJournalCmdr = Int32.MinValue;
        private JournalLocOrJump LastJournalLoc;

        public event Action<ScreenShotImageConverter> OnScreenshot;

        public ScreenshotDirectoryWatcher(EDDiscoveryForm frm, Action<Action<ScreenShotImageConverter>> paramscallback)
        {
            this.paramscallback = paramscallback;
            this.discoveryform = frm;
            string ScreenshotsDirdefault = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Frontier Developments", "Elite Dangerous");
            string OutputDirdefault = Path.Combine(ScreenshotsDirdefault, "Converted");

            discoveryform.OnNewJournalEntry += NewJournalEntry;
        }

        public bool Start(string watchedfolder, string ext)
        {
            this.Stop();

            if (Directory.Exists(watchedfolder))
            {
                filesystemwatcher = new System.IO.FileSystemWatcher();
                filesystemwatcher.Path = watchedfolder;

                filesystemwatcher.Filter = "*." + ext;
                filesystemwatcher.NotifyFilter = NotifyFilters.FileName;
                filesystemwatcher.Created += WatcherTripped;
                filesystemwatcher.EnableRaisingEvents = true;

                discoveryform.LogLine(string.Format("Scanning for {0} screenshots in {1}".T(EDTx.ScreenshotDirectoryWatcher_Scan) ,ext, watchedfolder));
                return true;
            }
            else
            {
                discoveryform.LogLineHighlight("Folder specified for image conversion does not exist, check settings in the Screenshots tab".T(EDTx.ScreenshotDirectoryWatcher_NOF));
                return false;
            }
        }

        public void Stop()
        {
            if (filesystemwatcher != null)
            {
                filesystemwatcher.Dispose();
                filesystemwatcher = null;
            }
        }

        public void Dispose()
        {
            Stop();
            discoveryform.OnNewJournalEntry -= NewJournalEntry;
        }

        private void NewJournalEntry(JournalEntry je)       // will be in UI thread
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
                string ssname = ss.Filename;
                if (ssname.StartsWith("\\ED_Pictures\\")) ssname = ssname.Substring(13);
                JournalScreenshotsByName[ssname] = ss;
                System.Diagnostics.Trace.WriteLine("Journal Screenshot logged " + ss.Filename);
                this.paramscallback?.Invoke(cp => ProcessScreenshot(ss.Filename, ss.System, ss, ss.CommanderId, cp));
                JournalScreenshotsByName[ssname] = null;
            }
        }

        private void WatcherTripped(object sender, System.IO.FileSystemEventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("Directory watcher picked up screenshot " + e.FullPath);
            this.paramscallback?.Invoke(cp => ProcessFilesystemEvent(sender, e, cp));
        }

        private void ProcessFilesystemEvent(object sender, System.IO.FileSystemEventArgs e, ScreenShotImageConverter cp) // on UI thread
        {
            System.Diagnostics.Debug.Assert(System.Windows.Forms.Application.MessageLoop);

            int cmdrid = LastJournalCmdr;

            if (e.FullPath.ToLowerInvariant().EndsWith(".bmp"))
            {
                if (!ScreenshotTimers.ContainsKey(e.FullPath))
                {
                    System.Threading.Timer timer = new System.Threading.Timer(s=>TimerTickedOut(e.FullPath, cmdrid), null, 5000, System.Threading.Timeout.Infinite);

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

        private void TimerTickedOut(string filename, int cmdrid)   // timer is executed on a background thread, go back to UI
        {
            string basename = Path.GetFileName(filename);
            JournalScreenshot ss = null;
            JournalScreenshotsByName.TryGetValue(basename, out ss);

            this.paramscallback?.Invoke(cp=>ProcessScreenshot(filename, ss?.System, ss, cmdrid, cp)); //process on UI thread
        }

        // called thru CalLWithConverter in UI main class.. that pases a ImageConverter to us
        // sysname and or ss can be null if it was picked up by a watcher not the new journal screenshot entry

        private void ProcessScreenshot(string filename, string sysname, JournalScreenshot ss, int cmdrid, ScreenShotImageConverter cp)
        {
            System.Diagnostics.Debug.Assert(System.Windows.Forms.Application.MessageLoop);  // UI thread

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
                HistoryEntry he = discoveryform.history.GetLast;
                sysname = (he != null) ? he.System.Name : "Unknown System";
            }

            try
            {
                cp.InputFilename = filename;
                cp.SystemName = sysname;
                cp.JournalScreenShot = ss;
                cp.CommanderID = cmdrid;

                bool converted = false;

                using (Bitmap bmp = cp.GetScreenshot(ref filename, discoveryform.LogLine))
                {
                    // Don't run if OnScreenshot has already run for this image
                    if ((ScreenshotTimers.TryGetValue(filename, out timer) && timer == null) || (!ScreenshotTimers.TryAdd(filename, null) && !ScreenshotTimers.TryUpdate(filename, null, timer)))
                        return;

                    if (timer != null)
                        timer.Dispose();

                    converted = cp.Convert(bmp, discoveryform.LogLine);
                }

                if (converted && cp.RemoveOriginal)         // if remove, delete original picture
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


                discoveryform.LogLineHighlight("Error in executing image conversion, try another screenshot, check output path settings. (Exception ".T(EDTx.ScreenshotDirectoryWatcher_Excp) + ex.Message + ")");
            }
        }
    }

}
