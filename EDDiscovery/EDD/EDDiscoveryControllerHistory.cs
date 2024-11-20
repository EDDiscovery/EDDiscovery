/*
 * Copyright © 2015 - 2023 EDDiscovery development team
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

using EliteDangerousCore;
using System;
using System.Collections.Concurrent;
using System.Threading;

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

        private RefreshWorkerArgs lastRefreshArgs = new RefreshWorkerArgs();
        private ConcurrentQueue<RefreshWorkerArgs> newRefreshWorkerQueue = new ConcurrentQueue<RefreshWorkerArgs>();           // QUEUE of refreshes pending, each with their own args..
        private int refreshHistoryRequestedFlag = 0;            // flag gets set during History refresh, cleared at end, interlocked exchange during request..

        private ManualResetEvent readyForNewRefresh = new ManualResetEvent(false);      // holds loop while refresh is happening. Set false until first refresh has happened
        private AutoResetEvent refreshRequested = new AutoResetEvent(false);

        // indicate change commander, indicate netlogpath load (with forced refresh), indicate forced journal load

        public bool RefreshHistoryAsync(string netlogpath = null, bool forcenetlogreload = false, bool forcejournalreload = false, int? currentcmdr = null, bool removedupfsdentries = false)
        {
            System.Diagnostics.Debug.WriteLine($"Refresh History Async ordered nlp {netlogpath}, forcenetlogreload {forcejournalreload}, forcejr {forcejournalreload}, cmdr {currentcmdr}, removedupfsd {removedupfsdentries}");
            if (PendingClose.IsCancellationRequested)
            {
                return false;
            }

            if (currentcmdr == null)                                                                        // if we did not give a commander, use the current configured commander
                currentcmdr = EDCommander.CurrentCmdrID;                                                    // not from history.commanderid, as it may never been set yet (bug 26/8/23 discovered by Ealhstan)

            bool newrefresh = false;

            if (refreshHistoryRequestedFlag == 0)                                                           // if we are not operating
            {                                                                                               // and we have materially changed comething important
                if (lastRefreshArgs == null ||                                                              // never done
                    lastRefreshArgs.ForceNetLogReload != forcenetlogreload ||                         
                    lastRefreshArgs.ForceJournalReload != forcejournalreload ||
                    lastRefreshArgs.CurrentCommander != currentcmdr ||
                    lastRefreshArgs.NetLogPath != netlogpath ||
                    lastRefreshArgs.RemoveDuplicateFSDEntries != removedupfsdentries)
                {
                    newrefresh = true;                                                                      // we queue the refresh, even if we have a async refresh pending..
                }
            }

            if (Interlocked.CompareExchange(ref refreshHistoryRequestedFlag, 1, 0) == 0 || newrefresh)      // set the refresh requested to 1 in all circumstances, to stop a 
            {
                newRefreshWorkerQueue.Enqueue(new RefreshWorkerArgs
                {
                    NetLogPath = netlogpath,
                    ForceNetLogReload = forcenetlogreload,
                    ForceJournalReload = forcejournalreload,
                    CurrentCommander = currentcmdr.Value,
                    RemoveDuplicateFSDEntries = removedupfsdentries
                }) ; ;

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
            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDC Background history refresh worker thread going.. waiting for read for refresh");

            // Wait to be ready for new refresh after initial load caused by DoRefreshHistory, called by the controller. It sets the flag
            WaitHandle.WaitAny(new WaitHandle[] { PendingClose.Token.WaitHandle, readyForNewRefresh }); 

            System.Diagnostics.Debug.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDC Background history refresh worker running");

            int capirefreshinterval = 8000;        // how often we check CAPI system.  

            while (!PendingClose.IsCancellationRequested)
            {
                // wait for a second and subsequent refresh request.

                int wh = WaitHandle.WaitAny(new WaitHandle[] { PendingClose.Token.WaitHandle, refreshRequested }, capirefreshinterval);     

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

                        while (newRefreshWorkerQueue.TryDequeue(out argstemp)) // Get the most recent refresh
                        {
                            args = argstemp;
                        }

                        if (args != null)
                        {
                            readyForNewRefresh.Reset();
                            DoRefreshHistory(args);
                            WaitHandle.WaitAny(new WaitHandle[] { PendingClose.Token.WaitHandle, readyForNewRefresh }); // Wait to be ready for new refresh
                        }
                        break;

                    case WaitHandle.WaitTimeout:
                        if (EDCommander.Current.ConsoleCommander && FrontierCAPI.Active)
                        {
                            var retstate = FrontierCAPI.ManageJournalDownload(EDCommander.Current.ConsoleUploadHistory, EDDOptions.Instance.CAPIDirectory(), 
                                            EDCommander.Current.Name, 
                                            new TimeSpan(0,15,0),       // journal poll interval
                                            28, 
                                            ReportCAPICommanderProgress, 2000       // keep message up for 2 seconds
                                            );    // and days back in time to look

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
                lastRefreshArgs = args;           // we are processing this set of operations

                System.Diagnostics.Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDC Load history for Cmdr {args.CurrentCommander} {EDCommander.Current.Name}");

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

                    System.Diagnostics.Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDC setup watchers def {string.Join(";", stdfolders)}");


                    journalmonitor.SetupWatchers(stdfolders, EDDOptions.Instance.DefaultJournalMatchFilename, EDDOptions.Instance.MinJournalDateUTC);         // monitors are stopped, set up watchers

                    int forcereloadoflastn = args.ForceJournalReload ? int.MaxValue / 2 : 0;     // if forcing a reload, we indicate that by setting the reload count to a very high value, but not enough to cause int wrap

                    System.Diagnostics.Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDC Parse journal files");

                    journalmonitor.ParseJournalFilesOnWatchers((p, s) => ReportRefreshProgress(p, string.Format("Processing log file {0}".T(EDTx.EDDiscoveryController_PLF),s)),
                                                                        PendingClose.Token,
                                                                        EDDOptions.Instance.MinJournalDateUTC,
                                                                         forcereloadoflastn
                                                                         );

                    if (args.NetLogPath != null)            // see if net logs need reading for old times sake.
                    {
                        NetLogClass.ParseFiles(args.NetLogPath, out string errstr, EDCommander.Current.MapColour, PendingClose.Token, (p, s) => ReportRefreshProgress(p, s), 
                            args.ForceNetLogReload, currentcmdrid: args.CurrentCommander);
                    }
                }

                HistoryList newhistory = null;
                bool redo = false;

                do
                {
                    var cmdr = EDCommander.GetCommander(args.CurrentCommander);
                    newhistory = new HistoryList();

                    var essentialitemslist = (EDDConfig.Instance.EssentialEventTypes == nameof(JournalEventsManagement.JumpScanEssentialEvents)) ? JournalEventsManagement.JumpScanEssentialEvents :
                               (EDDConfig.Instance.EssentialEventTypes == nameof(JournalEventsManagement.JumpEssentialEvents)) ? JournalEventsManagement.JumpEssentialEvents :
                               (EDDConfig.Instance.EssentialEventTypes == nameof(JournalEventsManagement.NoEssentialEvents)) ? JournalEventsManagement.NoEssentialEvents :
                               (EDDConfig.Instance.EssentialEventTypes == nameof(JournalEventsManagement.FullStatsEssentialEvents)) ? JournalEventsManagement.FullStatsEssentialEvents :
                                JournalEventsManagement.EssentialEvents;

                    int linkedcmdrid = cmdr.LinkedCommanderID;

                    if (linkedcmdrid >= 0 && EDCommander.GetCommander(linkedcmdrid) != null )      // if loading a linked commander (new nov 22 u14)
                    {
                        HistoryList.LoadHistory(newhistory, (p,s) => ReportRefreshProgress(p, s),  PendingClose.Token,
                                                        linkedcmdrid, cmdr.Name,
                                                        EDDOptions.Instance.HistoryLoadDayLimit > 0 ? EDDOptions.Instance.HistoryLoadDayLimit : EDDConfig.Instance.FullHistoryLoadDayLimit,
                                                        essentialitemslist,
                                                        cmdr.LinkedCommanderEndTime
                                                        );
                    }

                    // then load any data from our commander
                    HistoryList.LoadHistory(newhistory, (p,s) => ReportRefreshProgress(p, s), PendingClose.Token,
                                                    args.CurrentCommander, cmdr.Name,
                                                    EDDOptions.Instance.HistoryLoadDayLimit > 0 ? EDDOptions.Instance.HistoryLoadDayLimit : EDDConfig.Instance.FullHistoryLoadDayLimit,
                                                    essentialitemslist,
                                                    null
                                                    );

                    // now, if its a fresh one, we may have created a new linked commander. If that ends up being the current commander, we have not fully loaded properly
                    // If so, detect and do it again.  This scenario is unlikely but worth checking for

                    redo = cmdr.LinkedCommanderID != linkedcmdrid;      // if we updated the linked commander ID, we need to redo

                } while (redo);

                hist = newhistory;

                if (args.NetLogPath != null )
                {
                    ReportRefreshProgress(-1, "Netlog Updating System Positions");
                    hist.FillInPositionsFSDJumps(LogLine);                         // if netlog reading, try and resolve systems..
                }

                EDCommander.Current.FID = hist.GetCommanderFID();                   // ensure FID is set.. the other place it gets changed is a read of LoadGame.

                System.Diagnostics.Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()}  EDC Load history complete with {hist.Count} records");
            }
            catch (Exception ex)
            {
                LogLineHighlight("History Refresh Error: " + ex);
            }

            ReportRefreshProgress(100, "Refresh Displays".T(EDTx.EDDiscoveryController_RD));

            InvokeAsyncOnUiThread(() => ForegroundHistoryRefreshCompleteonUI(hist));
        }

        // Called on foreground after history has refreshed

        private void ForegroundHistoryRefreshCompleteonUI(HistoryList hist)
        {
            System.Diagnostics.Debug.Assert(System.Windows.Forms.Application.MessageLoop);     // in UI Thread

            if (!PendingClose.IsCancellationRequested)
            {
                System.Diagnostics.Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDC foreground history refresh start");

                if (hist != null)       // if we had an exception above, we may have an empty history
                {
                    commanderaddorupdatecount = EDCommander.AddOrUpdateCount;        // current count of adds/updates due to scanning at the history change point

                    History = hist;     // replace history

                    EliteDangerousCore.Spansh.SpanshClass.ClearBodyCache();             // we need to clear out the body cache, so that HasBodyLookupOccurred trips again
                    EliteDangerousCore.EDSM.EDSMClass.ClearBodyCache();     // because we have replaced history, and therefore starscan, so we need to add again.

                    EdsmLogFetcher.StopCheck();     // ensure edsm has stopped. previosly asked to stop above by an async call

                    System.Diagnostics.Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDC Refresh commanders Invoke");

                    OnRefreshCommanders?.Invoke();

                    System.Diagnostics.Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDC History Change Invoke");

                    OnHistoryChange?.Invoke();

                    System.Diagnostics.Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDC History Change Completed");
                }

                System.Diagnostics.Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDC Start monitor");

                journalmonitor.StartMonitor(true);

                System.Diagnostics.Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDC Refresh Complete Invoke");

                OnRefreshComplete?.Invoke();                            // History is completed


                FrontierCAPI.Disconnect();         // Disconnect capi from current user, but don't clear their credential file

                // available, and not hidden commander
                if (FrontierCAPI.ClientIDAvailable && EDCommander.Current.Id >= 0)
                {
                    // so a legacy commander, and a live commander, share the same CAPI oauth login (the CAPI server is just different)
                    // lets go to the capi root name for the saved data file

                    if (FrontierCAPI.GetUserState(EDCommander.Current.RootName) != CAPI.CompanionAPI.UserState.NeverLoggedIn)
                    {
                        CAPI.CompanionAPI.CAPIServerType stype = EDCommander.Current.NameIsBeta ? CAPI.CompanionAPI.CAPIServerType.Beta : 
                                    (EDCommander.Current.LegacyCommander || EDCommander.Current.ConsoleCommander) ? CAPI.CompanionAPI.CAPIServerType.Legacy : 
                                    CAPI.CompanionAPI.CAPIServerType.Live;
                        
                        FrontierCAPI.CAPIServer= stype;

                        System.Diagnostics.Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDC Login with CAPI server type {stype} {EDCommander.Current.RootName}");

                        System.Threading.Tasks.Task.Run(() =>           // don't hold up the main thread, do it in a task, as its a HTTP operation
                        {
                            FrontierCAPI.LogIn(EDCommander.Current.RootName);   // try and get to Active.  May cause a new frontier login

                            if (FrontierCAPI.Active)     // if active, indicate
                                LogLine($"CAPI User Logged in to {stype} using {EDCommander.Current.RootName}");
                            else
                                LogLine($"CAPI Require Fresh Log in to {stype} using {EDCommander.Current.RootName}");
                        });
                    }
                }

                if (History.IsRealCommanderId)
                    EdsmLogFetcher.Start(EDCommander.Current);

                refreshHistoryRequestedFlag = 0;
                readyForNewRefresh.Set();       // say i'm okay for another refresh

                LogLine("History refresh complete.".T(EDTx.EDDiscoveryController_HRC));

                ReportRefreshProgress(-1, "");

                System.Diagnostics.Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} EDC foreground refresh completed");
            }
        }

        public void ReportRefreshProgress(int percent, string message)
        {
            StatusLineUpdate?.Invoke(EDDiscoveryForm.StatusLineUpdateType.History, percent, message);      // can be invoked in thread
        }
        public void ReportCAPICommanderProgress(string message)
        {
            StatusLineUpdate?.Invoke(EDDiscoveryForm.StatusLineUpdateType.CAPIJournal, -1, message);      // can be invoked in thread
        }

    }
}
