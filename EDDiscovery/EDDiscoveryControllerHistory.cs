/*
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
using System.Diagnostics;
using System.Threading;
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

            while (!PendingClose)
            {
                int wh = WaitHandle.WaitAny(new WaitHandle[] { closeRequested, refreshRequested });     // wait for a second and subsequent refresh request.

                System.Diagnostics.Debug.WriteLine("Background history refresh worker - kicked due to " + wh);

                if (PendingClose) break;

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

                hist = HistoryList.LoadHistory(journalmonitor,
                    () => PendingClose,
                    (p, s) => ReportRefreshProgress(p, string.Format("Processing log file {0}".T(EDTx.EDDiscoveryController_PLF), s)), args.NetLogPath,
                    args.ForceNetLogReload, args.ForceJournalReload, args.CurrentCommander,
                    EDDConfig.Instance.FullHistoryLoadDayLimit, EDDConfig.Instance.EssentialEventTypes);

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
                    history.Copy(hist);

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
