﻿/*
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

using EliteDangerousCore.EDSM;
using System;
using System.Threading;
using EliteDangerousCore.DB;
using System.IO;
using System.Net;

namespace EDDiscovery
{
    public partial class EDDiscoveryController
    {
        private class SystemsSyncState
        {
            public bool perform_fullsync = false;

            public long fullsync_count = 0;
            public long updatesync_count = 0;

            public void ClearCounters()
            {
                fullsync_count = 0;
                updatesync_count = 0;
            }
        }

        private SystemsSyncState syncstate = new SystemsSyncState();

        private int resyncSysDBRequestedFlag = 0;            // flag gets set during SysDB refresh, cleared at end, interlocked exchange during request..

        public bool AsyncPerformSync(bool fullsync)      // UI thread.
        {
            System.Diagnostics.Debug.WriteLine($"Ask for sync start {fullsync}");
            System.Diagnostics.Debug.Assert(System.Windows.Forms.Application.MessageLoop);

            if (Interlocked.CompareExchange(ref resyncSysDBRequestedFlag, 1, 0) == 0)
            {
                syncstate.perform_fullsync |= fullsync;
                resyncRequestedEvent.Set();
                return true;
            }
            else
            {
                return false;
            }
        }

        const int ForceEDSMFullDownloadDays = 56;      // beyond this time, we force a full download
        const int ForceSpanshFullDownloadDays = 170;   // beyond this time, we force a full download
        const int MiniumSpanshUpdateAge = 3;           // beyond this time, we update spansh

        public void CheckForSync()      // called in background init
        {
            if (!EDDOptions.Instance.NoSystemsLoad && EDDConfig.Instance.SystemDBDownload)        // if enabled
            {
                DateTime edsmdatetime = SystemsDatabase.Instance.GetLastRecordTimeUTC();

                bool spansh = SystemsDatabase.Instance.DBSource.Equals("SPANSH");
                var delta = DateTime.UtcNow.Subtract(edsmdatetime).TotalDays;

                if (delta >= (spansh ? ForceEDSMFullDownloadDays : ForceSpanshFullDownloadDays))
                {
                    System.Diagnostics.Debug.WriteLine("Full system data download ordered, time since {0}", DateTime.UtcNow.Subtract(edsmdatetime).TotalDays);
                    syncstate.perform_fullsync = true;       // do a full sync.
                }

                if (syncstate.perform_fullsync)
                {
                    LogLine(string.Format("System data download from {0} required." + Environment.NewLine +
                                    "This will take a while, please be patient." + Environment.NewLine +
                                    "Please continue running ED Discovery until refresh is complete.".T(EDTx.EDDiscoveryController_SyncEDSM), SystemsDatabase.Instance.DBSource));
                }
            }
            else
            {
                LogLine("Star Data download is disabled. Use Settings panel to reenable".T(EDTx.EDDiscoveryController_SyncOff));
            }
        }

        private void DoPerformSync()        // in Background worker
        {
            System.Diagnostics.Debug.WriteLine($"Do perform sync starts {syncstate.perform_fullsync}");

            InvokeAsyncOnUiThread.Invoke(() => OnSyncStarting?.Invoke());       // tell listeners sync is starting

            resyncSysDBRequestedFlag = 1;     // sync is happening, stop any async requests..

            if (EDDConfig.Instance.SystemDBDownload)      // if system DB is to be loaded
            {
                System.Diagnostics.Debug.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Perform System Data Download");

                try
                {
                    bool[] grids = new bool[GridId.MaxGridID];
                    foreach (int i in GridId.FromString(SystemsDatabase.Instance.GetGridIDs()))
                        grids[i] = true;

                    syncstate.ClearCounters();

                    string sourcetype = SystemsDatabase.Instance.DBSource;
                    bool spansh = sourcetype.Equals("SPANSH");

                    if (syncstate.perform_fullsync)
                    {
                        if (syncstate.perform_fullsync && !PendingClose.IsCancellationRequested)
                        {
                            // Download new systems
                            try
                            {
                                string downloadfile = Path.Combine(EDDOptions.Instance.AppDataDirectory, spansh ? "spanshsystems.json.gz" : "edsmsystems.json.gz");

                                ReportSyncProgress("Performing full download of System Data");

                                string url = spansh ? string.Format(EDDConfig.Instance.SpanshSystemsURL, "") : EDDConfig.Instance.EDSMFullSystemsURL;

                                System.Diagnostics.Trace.WriteLine($"{BaseUtils.AppTicks.TickCountLap()} Full system download using URL {url} to {downloadfile}");

                                bool deletefile = !EDDOptions.Instance.KeepSystemDataDownloadedFiles;

#if DEBUGLOAD
                                bool success = true;
                                deletefile = false;
                                downloadfile = spansh ? @"c:\code\spanshsystems.json.gz" : @"c:\code\examples\edsm\edsmsystems.1e6.json";
#else
                                bool success = BaseUtils.HttpCom.DownloadFileFromURI(PendingClose.Token, url, downloadfile, false, out bool newfile, reportProgress: ReportDownloadProgress);
#endif
                                if (!PendingClose.IsCancellationRequested)      // if not closing
                                {
                                    syncstate.perform_fullsync = false;

                                    if (success)
                                    {
                                        ReportSyncProgress("Download complete, creating database");

                                        syncstate.fullsync_count = SystemsDatabase.Instance.CreateSystemDBFromJSONFile(downloadfile, grids, 200000, PendingClose.Token, ReportSyncProgress, method: 3);

                                        if (deletefile && !PendingClose.IsCancellationRequested)        // if remove file, and we are not cancelled, delete it
                                            BaseUtils.FileHelpers.DeleteFileNoError(downloadfile);

                                        if (syncstate.fullsync_count < 0)     // this should always update something, the table is replaced.  If its not, its been cancelled
                                        {
                                            InvokeAsyncOnUiThread(() => PerformSyncCompletedonUI());
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        LogLineHighlight("Failed to download full systems file. Try re-running EDD later");
                                        BaseUtils.FileHelpers.DeleteFileNoError(downloadfile);       // remove file - don't hold in storage
                                        InvokeAsyncOnUiThread(() => PerformSyncCompletedonUI());
                                        return;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LogLineHighlight("System DB Full Sync exception:" + ex.Message);
                            }
                        }

                    }

                    if (!PendingClose.IsCancellationRequested)          // perform an update sync to get any new star data
                    {
                        if (spansh)
                        {
                            DateTime lastrecordtime = SystemsDatabase.Instance.GetLastRecordTimeUTC();
                            var delta = DateTime.UtcNow.Subtract(lastrecordtime).TotalDays;
                            string filename = delta < 7 ? "_1week" : delta < 14 ? "_2weeks" : delta < 28 ? "_1month" : "_6months";
                            string url = string.Format(EDDConfig.Instance.SpanshSystemsURL, filename);
                            string downloadfile = Path.Combine(EDDOptions.Instance.AppDataDirectory, "systemsdelta.json.gz");
                            bool deletefile = !EDDOptions.Instance.KeepSystemDataDownloadedFiles;

#if DEBUGLOAD
                            {
                                bool success = true;
                                downloadfile = @"c:\code\examples\edsm\systems_1week.json";
                                deletefile = false;
#else
                            if ( delta >= MiniumSpanshUpdateAge )        // if its older than this, we will do an update
                            {
                                ReportSyncProgress($"Performing partial download of System Data from {url}");
                                bool success = BaseUtils.HttpCom.DownloadFileFromURI(PendingClose.Token, url, downloadfile, false, out bool newfile, reportProgress: ReportDownloadProgress);
#endif

                                if (success)        // grabbed sucessfully
                                {
                                    ReportSyncProgress("Download complete, updating database");

                                    System.Diagnostics.Trace.WriteLine($"Peforming spansh update on data {delta} old from {url}");

                                    syncstate.updatesync_count = SystemsDatabase.Instance.UpdateSpanshSystemsFromJSONFile(downloadfile, grids, PendingClose.Token, ReportSyncProgress);

                                    System.Diagnostics.Trace.WriteLine($"Downloaded from spansh {syncstate.updatesync_count}");
                                }
                                else
                                {
                                    LogLine("Download of Spansh systems from the server failed (no data returned), will try next time program is run");
                                }

                                if ( deletefile )
                                    BaseUtils.FileHelpers.DeleteFileNoError(downloadfile);       // remove file - don't hold in storage
                            }
                        }
                        else
                        {
                            syncstate.updatesync_count = SystemsDatabase.Instance.UpdateEDSMSystemsFromWeb(grids, PendingClose.Token, ReportSyncProgress, LogLine, ForceEDSMFullDownloadDays);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Swallow Operation Cancelled exceptions
                }
                catch (Exception ex)
                {
                    LogLineHighlight("Check Systems exception: " + ex.Message + Environment.NewLine + "Trace: " + ex.StackTrace);
                }
            }

            InvokeAsyncOnUiThread(() => PerformSyncCompletedonUI());
        }

        // Done in UI thread after DoPerformSync completes

        private void PerformSyncCompletedonUI()
        {
            System.Diagnostics.Debug.Assert(System.Windows.Forms.Application.MessageLoop);

            if (syncstate.fullsync_count > 0 || syncstate.updatesync_count > 0)
                LogLine(string.Format("Systems update complete with {0:N0} systems".T(EDTx.EDDiscoveryController_EDSMU), syncstate.fullsync_count + syncstate.updatesync_count));

            OnSyncComplete?.Invoke(syncstate.fullsync_count, syncstate.updatesync_count);

            ReportSyncProgress("");

            resyncSysDBRequestedFlag = 0;        // releases flag and allow another async to happen

            System.Diagnostics.Debug.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Perform sync completed");
        }


        public void ReportSyncProgress(string message)
        {
            StatusLineUpdate?.Invoke(EDDiscoveryForm.StatusLineUpdateType.SystemData, -1, message);
        }

        public void ReportDownloadProgress(long count, double rate)
        {
            StatusLineUpdate?.Invoke(EDDiscoveryForm.StatusLineUpdateType.SystemData, -1, count == 0 ? "Starting Download" : $"Downloaded {count / 1024:N0} KB at {rate / 1024 / 1024:N2} MB/sec");
        }

    }
}

