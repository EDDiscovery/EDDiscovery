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
        const int EDSMUpdateFetchHours = 12;           // for an update fetch, its these number of hours at a time (Feb 2021 moved to 6 due to EDSM new server)

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

                                        syncstate.fullsync_count = SystemsDatabase.Instance.MakeSystemTableFromFile(downloadfile, grids, 200000, PendingClose.Token, ReportSyncProgress, method: 3);

                                        if (deletefile && !PendingClose.IsCancellationRequested)        // if remove file, and we are not cancelled, delete it
                                            BaseUtils.FileHelpers.DeleteFileNoError(downloadfile);       

                                        if (syncstate.fullsync_count < 0)     // this should always update something, the table is replaced.  If its not, its been cancelled
                                            return;
                                    }
                                    else
                                    {
                                        ReportSyncProgress("");
                                        LogLineHighlight("Failed to download full systems file. Try re-running EDD later");
                                        BaseUtils.FileHelpers.DeleteFileNoError(downloadfile);       // remove file - don't hold in storage
                                        return;     // new! if we failed to download, fail here, wait for another time
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

                                    syncstate.updatesync_count = SystemsDatabase.Instance.UpdateSystems(downloadfile, grids, PendingClose.Token, ReportSyncProgress);

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
                            syncstate.updatesync_count = EDSMUpdateSync(grids, PendingClose.Token, ReportSyncProgress);
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

        public long EDSMUpdateSync(bool[] grididallow, CancellationToken cancel, Action<string> ReportProgress)
        {
            // smallish block size, non overlap, allow overwrite
            SystemsDB.Loader3 loader3 = new SystemsDB.Loader3("", 50000, grididallow, false, false);       

            DateTime maximumupdatetimewindow = DateTime.UtcNow.AddDays(-ForceEDSMFullDownloadDays);        // limit download to this amount of days
            if (loader3.LastDate < maximumupdatetimewindow)
                loader3.LastDate = maximumupdatetimewindow;               // this stops crazy situations where somehow we have a very old date but the full sync did not take care of it

            long updates = 0;

            double fetchmult = 1;

            DateTime minimumfetchspan = DateTime.UtcNow.AddHours(-EDSMUpdateFetchHours / 2);        // we don't bother fetching if last record time is beyond this point

            while (loader3.LastDate < minimumfetchspan)                              // stop at X mins before now, so we don't get in a condition
            {                                                                           // where we do a set, the time moves to just before now, 
                                                                                        // and we then do another set with minimum amount of hours
                if (cancel.IsCancellationRequested)
                    break;

                if ( updates == 0)
                    LogLine("Checking for updated EDSM systems (may take a few moments).");

                EDSMClass edsm = new EDSMClass();

                double hourstofetch = EDSMUpdateFetchHours;        //EDSM new server feb 2021, more capable, 

                DateTime enddate = loader3.LastDate + TimeSpan.FromHours(hourstofetch * fetchmult);
                if (enddate > DateTime.UtcNow)
                    enddate = DateTime.UtcNow;

                LogLine($"Downloading systems from UTC {loader3.LastDate} to {enddate}");
                System.Diagnostics.Debug.WriteLine($"Downloading systems from UTC {loader3.LastDate} to {enddate} {hourstofetch}");

                string json = null;
                BaseUtils.HttpCom.Response response;
                try
                {
                    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                    response = edsm.RequestSystemsData(loader3.LastDate, enddate, timeout: 20000);
                    fetchmult = Math.Max(0.1, Math.Min(Math.Min(fetchmult * 1.1, 1.0), 5000.0 / sw.ElapsedMilliseconds));
                }
                catch (WebException ex)
                {
                    ReportProgress($"EDSM request failed");
                    if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null && ex.Response is HttpWebResponse)
                    {
                        string status = ((HttpWebResponse)ex.Response).StatusDescription;
                        LogLine($"Download of EDSM systems from the server failed ({status}), will try next time program is run");
                    }
                    else
                    {
                        LogLine($"Download of EDSM systems from the server failed ({ex.Status.ToString()}), will try next time program is run");
                    }

                    break;
                }
                catch (Exception ex)
                {
                    ReportProgress($"EDSM request failed");
                    LogLine($"Download of EDSM systems from the server failed ({ex.Message}), will try next time program is run");
                    break;
                }

                if (response.Error)
                {
                    if ((int)response.StatusCode == 429)
                    {
                        LogLine($"EDSM rate limit hit - waiting 2 minutes");
                        for (int sec = 0; sec < 120; sec++)
                        {
                            if (!cancel.IsCancellationRequested)
                            {
                                System.Threading.Thread.Sleep(1000);
                            }
                        }
                    }
                    else
                    {
                        LogLine($"Download of EDSM systems from the server failed ({response.StatusCode.ToString()}), will try next time program is run");
                        break;
                    }
                }

                json = response.Body;

                if (json == null)
                {
                    ReportProgress("EDSM request failed");
                    LogLine("Download of EDSM systems from the server failed (no data returned), will try next time program is run");
                    break;
                }

                // debug File.WriteAllText(@"c:\code\json.txt", json);

                ReportProgress($"EDSM star database update from UTC " + loader3.LastDate.ToString() );

                var prevrectime = loader3.LastDate;

                long updated = loader3.ParseJSONString(json, cancel, ReportSyncProgress);

                System.Diagnostics.Trace.WriteLine($"EDSM partial download updated {updated} to {loader3.LastDate}");

                // if lastrecordtime did not change (=) or worse still, EDSM somehow moved the time back (unlikely)
                if (loader3.LastDate <= prevrectime)
                {
                    loader3.LastDate += TimeSpan.FromHours(12);       // Lets move on manually so we don't get stuck
                }

                updates += updated;

                int delay = 10;     // Anthor's normal delay 
                int ratelimitlimit;
                int ratelimitremain;
                int ratelimitreset;

                if (response.Headers != null &&
                    response.Headers["X-Rate-Limit-Limit"] != null &&
                    response.Headers["X-Rate-Limit-Remaining"] != null &&
                    response.Headers["X-Rate-Limit-Reset"] != null &&
                    Int32.TryParse(response.Headers["X-Rate-Limit-Limit"], out ratelimitlimit) &&
                    Int32.TryParse(response.Headers["X-Rate-Limit-Remaining"], out ratelimitremain) &&
                    Int32.TryParse(response.Headers["X-Rate-Limit-Reset"], out ratelimitreset))
                {
                    if (ratelimitremain < ratelimitlimit * 3 / 4)       // lets keep at least X remaining for other purposes later..
                        delay = ratelimitreset / (ratelimitlimit - ratelimitremain);    // slow down to its pace now.. example 878/(360-272) = 10 seconds per quota
                    else
                        delay = 0;

                    System.Diagnostics.Debug.WriteLine("EDSM Delay Parameters {0} {1} {2} => {3}s", ratelimitlimit, ratelimitremain, ratelimitreset, delay);
                }

                for (int sec = 0; sec < delay; sec++)
                {
                    if (!cancel.IsCancellationRequested)
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }

            loader3.Finish(cancel);
            return updates;
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

