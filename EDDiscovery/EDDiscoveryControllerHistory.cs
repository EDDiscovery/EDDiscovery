﻿/*
 * Copyright © 2015 - 2019 EDDiscovery development team
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using QuickJSON;
using EliteDangerousCore;

namespace EDDiscovery
{
    public partial class EDDiscoveryController
    {
        protected class RefreshWorkerArgs
        {
            public string NetLogPath;
            public bool ForceNetLogReload;
            public bool ForceJournalReload;
            public bool RemoveDuplicateFSDEntries;
            public int CurrentCommander;
        }

        private RefreshWorkerArgs refreshWorkerArgs = new RefreshWorkerArgs();
        private ConcurrentQueue<RefreshWorkerArgs> refreshWorkerQueue = new ConcurrentQueue<RefreshWorkerArgs>();           // QUEUE of refreshes pending, each with their own args..
        private int refreshHistoryRequestedFlag = 0;            // flag gets set during History refresh, cleared at end, interlocked exchange during request..

        private ManualResetEvent readyForNewRefresh = new ManualResetEvent(false);      // holds loop while refresh is happening. Set false until first refresh has happened
        private AutoResetEvent refreshRequested = new AutoResetEvent(false);

        // indicate change commander, indicate netlogpath load (with forced refresh), indicate forced journal load

        public bool RefreshHistoryAsync(string netlogpath = null, bool forcenetlogreload = false, bool forcejournalreload = false, int? currentcmdr = null, bool removedupfsdentries = false)
        {
            if (PendingClose)
            {
                return false;
            }

            bool newrefresh = false;

            RefreshWorkerArgs curargs = refreshWorkerArgs;
            if (refreshHistoryRequestedFlag == 0)                                                           // if we are not operating
            {
                if (curargs == null ||                                                                      // and we have mateirlally changed comething important
                    curargs.ForceNetLogReload != forcenetlogreload ||
                    curargs.ForceJournalReload != forcejournalreload ||
                    curargs.CurrentCommander != (currentcmdr ?? history.CommanderId) ||
                    curargs.NetLogPath != netlogpath ||
                    curargs.RemoveDuplicateFSDEntries != removedupfsdentries)
                {
                    newrefresh = true;                                                                      // we queue the refresh, even if we have a async refresh pending..
                }
            }

            if (Interlocked.CompareExchange(ref refreshHistoryRequestedFlag, 1, 0) == 0 || newrefresh)      // set the refresh requested to 1 in all circumstances, to stop a 
            {
                refreshWorkerQueue.Enqueue(new RefreshWorkerArgs
                {
                    NetLogPath = netlogpath,
                    ForceNetLogReload = forcenetlogreload,
                    ForceJournalReload = forcejournalreload,
                    CurrentCommander = currentcmdr ?? history.CommanderId,
                    RemoveDuplicateFSDEntries = removedupfsdentries
                });

                refreshRequested.Set();
                return true;
            }
            else
            {
                return false;
            }
        }


        // this thread waits around until told to do a refresh then performs it.  
        // ONLY used for subsequent refreshes, first one done on background worker

        private void BackgroundHistoryRefreshWorkerThread()
        {
            System.Diagnostics.Debug.WriteLine("Background history refresh worker thread going.. waiting for read for refresh");
            WaitHandle.WaitAny(new WaitHandle[] { closeRequested, readyForNewRefresh }); // Wait to be ready for new refresh after initial load caused by DoRefreshHistory, called by the controller. It sets the flag

            System.Diagnostics.Debug.WriteLine("Background history refresh worker thread refresh given permission, close " + PendingClose);

            int capirefreshinterval = 10000;        // how often we check CAPI system.  This is not the poll interval.

            while (!PendingClose)
            {
                int wh = WaitHandle.WaitAny(new WaitHandle[] { closeRequested, refreshRequested }, capirefreshinterval);     // wait for a second and subsequent refresh request.

                if (PendingClose)
                    break;

                switch (wh)
                {
                    case 0:  // Close Requested
                        break;

                    case 1:  // Refresh Requested
                        journalmonitor.StopMonitor();          // this is called by the foreground.  Ensure background is stopped.  Foreground must restart it.
                        EdsmLogFetcher.AsyncStop();
                        InvokeAsyncOnUiThread(() =>
                        {
                            OnRefreshStarting?.Invoke();
                        });

                        RefreshWorkerArgs argstemp = null;
                        RefreshWorkerArgs args = null;

                        while (refreshWorkerQueue.TryDequeue(out argstemp)) // Get the most recent refresh
                        {
                            args = argstemp;
                        }

                        if (args != null)
                        {
                            readyForNewRefresh.Reset();
                            DoRefreshHistory(args);
                            WaitHandle.WaitAny(new WaitHandle[] { closeRequested, readyForNewRefresh }); // Wait to be ready for new refresh
                        }
                        break;

                    case WaitHandle.WaitTimeout:
                        if (EDCommander.Current.ConsoleCommander && FrontierCAPI.Active)
                        {
                            var retstate = FrontierCAPI.ManageJournalDownload(EDCommander.Current.ConsoleUploadHistory, EDDOptions.Instance.CAPIDirectory(), 
                                            EDCommander.Current.Name, 
                                            new TimeSpan(0,30,0),       // journal poll interval
                                            28);    // and days back in time to look

                            if (EDCommander.Current.ConsoleUploadHistory == null || !retstate.DeepEquals(EDCommander.Current.ConsoleUploadHistory))     // if changed
                            {
                                EDCommander.Current.ConsoleUploadHistory = retstate;
                                EDCommander.Current.Update();
                            }
                                
                        }
                        break;
                }
            }
        }

        // this function does the history refresh, executes on Background worker or background history refresh thread
        private void DoRefreshHistory(RefreshWorkerArgs args)
        {
            HistoryList hist = null;
            try
            {
                refreshWorkerArgs = args;

                Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Load history for Cmdr " + args.CurrentCommander + " " + EDCommander.Current.Name);

                if (args.RemoveDuplicateFSDEntries)
                {
                    int n = JournalEntry.RemoveDuplicateFSDEntries(EDCommander.CurrentCmdrID);
                    LogLine(string.Format("Removed {0} FSD entries".T(EDTx.EDDiscoveryForm_FSDRem), n));
                }

                if (args.CurrentCommander >= 0)             // if we have a real commander
                {
                    string stdfolder = EDDOptions.Instance.DefaultJournalFolder.HasChars() ? EDDOptions.Instance.DefaultJournalFolder :
                                                                                EliteDangerousCore.FrontierFolder.FolderName();     // may be null

                    string[] stdfolders = stdfolder != null ? new string[] { stdfolder, EDDOptions.Instance.CAPIDirectory() } : new string[] { EDDOptions.Instance.CAPIDirectory() };

                    Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Setup watchers " + string.Join(",", stdfolders));


                    journalmonitor.SetupWatchers(stdfolders, EDDOptions.Instance.DefaultJournalMatchFilename, EDDOptions.Instance.MinJournalDateUTC);         // monitors are stopped, set up watchers

                    int forcereloadoflastn = args.ForceJournalReload ? int.MaxValue / 2 : 0;     // if forcing a reload, we indicate that by setting the reload count to a very high value, but not enough to cause int wrap

                    Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Parse journal files");

                    journalmonitor.ParseJournalFilesOnWatchers((p, s) => ReportRefreshProgress(p, string.Format("Processing log file {0}".T(EDTx.EDDiscoveryController_PLF),s)), 
                                                                         EDDOptions.Instance.MinJournalDateUTC,
                                                                         forcereloadoflastn,
                                                                         closerequested:closeRequested);

                    if (args.NetLogPath != null)            // see if net logs need reading for old times sake.
                    {
                        NetLogClass.ParseFiles(args.NetLogPath, out string errstr, EDCommander.Current.MapColour, () => PendingClose, (p, s) => ReportRefreshProgress(p, s), 
                            args.ForceNetLogReload, currentcmdrid: args.CurrentCommander);
                    }
                }

                hist = HistoryList.LoadHistory( (s) => ReportRefreshProgress(-1, s), ()=>PendingClose,
                                                args.CurrentCommander,
                                                EDDOptions.Instance.HistoryLoadDayLimit > 0 ? EDDOptions.Instance.HistoryLoadDayLimit : EDDConfig.Instance.FullHistoryLoadDayLimit, 
                                                EDDConfig.Instance.EssentialEventTypes
                                                 );

                if (args.NetLogPath != null )
                {
                    ReportRefreshProgress(-1, "Netlog Updating System Positions");
                    hist.FillInPositionsFSDJumps(LogLine);                         // if netlog reading, try and resolve systems..
                }

                EDCommander.Current.FID = hist.GetCommanderFID();                   // ensure FID is set.. the other place it gets changed is a read of LoadGame.

                ReportRefreshProgress(-1, "Done");

                Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Load history complete with " + hist.Count + " records");
            }
            catch (Exception ex)
            {
                LogLineHighlight("History Refresh Error: " + ex);
            }

            ReportRefreshProgress(-1, "Refresh Displays".T(EDTx.EDDiscoveryController_RD));

            InvokeAsyncOnUiThread(() => ForegroundHistoryRefreshCompleteonUI(hist));
        }

        // Called on foreground after history has refreshed

        private void ForegroundHistoryRefreshCompleteonUI(HistoryList hist)
        {
            Debug.Assert(System.Windows.Forms.Application.MessageLoop);

            if (!PendingClose)
            {
                Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Refresh history worker completed");

                if (hist != null)
                {
                    history = hist;

                    OnRefreshCommanders?.Invoke();

                    EdsmLogFetcher.StopCheck();

                    Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Refresh Displays");

                    OnHistoryChange?.Invoke(history);

                    Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Refresh Displays Completed");
                }

                Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " JM On");

                journalmonitor.StartMonitor(true);

                Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Call Refresh Complete");

                OnRefreshComplete?.Invoke();                            // History is completed

                FrontierCAPI.Disconnect();         // Disconnect capi from current user, but don't clear their credential file

                // available, and not hidden commander, and we have logged in before
                if (FrontierCAPI.ClientIDAvailable && EDCommander.Current.Id >= 0 && FrontierCAPI.GetUserState(EDCommander.Current.Name) != CAPI.CompanionAPI.UserState.NeverLoggedIn)
                {
                    FrontierCAPI.GameIsBeta = EDCommander.Current.Name.StartsWith("[BETA]", StringComparison.InvariantCultureIgnoreCase);

                    System.Threading.Tasks.Task.Run(() =>           // don't hold up the main thread, do it in a task, as its a HTTP operation
                    {
                        FrontierCAPI.LogIn(EDCommander.Current.Name);   // try and get to Active.  May cause a new frontier login

                        if (FrontierCAPI.Active)     // if active, indicate
                            LogLine("CAPI User Logged in");
                        else
                            LogLine("CAPI Require Log in");
                    });
                }

                if (history.CommanderId >= 0)
                    EdsmLogFetcher.Start(EDCommander.Current);

                refreshHistoryRequestedFlag = 0;
                readyForNewRefresh.Set();       // say i'm okay for another refresh
                System.Diagnostics.Debug.WriteLine("Refresh completed, allow another refresh");

                LogLine("History refresh complete.".T(EDTx.EDDiscoveryController_HRC));

                ReportRefreshProgress(-1, "");

                Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Refresh history complete");
            }
        }

    }
}
