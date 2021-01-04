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

using EliteDangerousCore.EDSM;
using System;
using System.Diagnostics;
using System.Threading;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using System.IO;
using System.Net;

namespace EDDiscovery
{
    public partial class EDDiscoveryController
    {
        private class SystemsSyncState
        {
            public bool perform_edsm_fullsync = false;
            public bool perform_edsm_alias_sync = false;

            public long edsm_fullsync_count = 0;
            public long edsm_updatesync_count = 0;

            public void ClearCounters()
            {
                edsm_fullsync_count = 0;
                edsm_updatesync_count = 0;
            }
        }

        private SystemsSyncState syncstate = new SystemsSyncState();

        private int resyncEDSMRequestedFlag = 0;            // flag gets set during EDSM refresh, cleared at end, interlocked exchange during request..

        public bool AsyncPerformSync(bool edsm_alias_sync = false, bool edsmfullsync = false)      // UI thread.
        {
            Debug.Assert(System.Windows.Forms.Application.MessageLoop);

            if (Interlocked.CompareExchange(ref resyncEDSMRequestedFlag, 1, 0) == 0)
            {
                syncstate.perform_edsm_alias_sync |= edsm_alias_sync;
                syncstate.perform_edsm_fullsync |= edsmfullsync;
                resyncRequestedEvent.Set();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CheckForSync()      // called in background init
        {
            if (!EDDOptions.Instance.NoSystemsLoad && EDDConfig.Instance.EDSMDownload)        // if enabled
            {
                DateTime edsmdatetime = SystemsDatabase.Instance.GetLastEDSMRecordTimeUTC();

                if (DateTime.UtcNow.Subtract(edsmdatetime).TotalDays >= 28)   // 600k ish per 12hours.  So 33MB.  Much less than a full download which is (23/1/2018) 2400MB, or 600MB compressed
                {
                    System.Diagnostics.Debug.WriteLine("EDSM Full system data download ordered, time since {0}", DateTime.UtcNow.Subtract(edsmdatetime).TotalDays);
                    syncstate.perform_edsm_fullsync = true;       // do a full sync.
                }

                DateTime aliasdatetime = SystemsDatabase.Instance.GetLastAliasDownloadTime();

                if (DateTime.UtcNow.Subtract(aliasdatetime).TotalDays > 6.5)     // Get this data once every week.
                    syncstate.perform_edsm_alias_sync = true;

                if (syncstate.perform_edsm_alias_sync || syncstate.perform_edsm_fullsync)
                {
                    string databases = "EDSM";

                    LogLine(string.Format("Full system data download from {0} required." + Environment.NewLine +
                                    "This will take a while, please be patient." + Environment.NewLine +
                                    "Please continue running ED Discovery until refresh is complete.".T(EDTx.EDDiscoveryController_SyncEDSM), databases));
                }
            }
            else
            {
                LogLine("Star Data download is disabled. Use Settings panel to reenable".T(EDTx.EDDiscoveryController_SyncOff));
            }
        }

        private void DoPerformSync()        // in Background worker
        {
            InvokeAsyncOnUiThread.Invoke(() => OnSyncStarting?.Invoke());       // tell listeners sync is starting

            resyncEDSMRequestedFlag = 1;     // sync is happening, stop any async requests..

            // check for 102, if so, upgrade it..
            SystemsDatabase.Instance.UpgradeSystemTableFrom102TypeDB(() => PendingClose, ReportSyncProgress, syncstate.perform_edsm_fullsync);

            if (EDDConfig.Instance.EDSMDownload)      // if no system off, and EDSM download on
            {
                Debug.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Perform System Data Download from EDSM");

                try
                {
                    bool[] grids = new bool[GridId.MaxGridID];
                    foreach (int i in GridId.FromString(EDDConfig.Instance.EDSMGridIDs))
                        grids[i] = true;

                    syncstate.ClearCounters();

                    if (syncstate.perform_edsm_fullsync || syncstate.perform_edsm_alias_sync)
                    {
                        if (syncstate.perform_edsm_fullsync && !PendingClose)
                        {
                            // Download new systems
                            try
                            {
                                string edsmsystems = Path.Combine(EliteConfigInstance.InstanceOptions.AppDataDirectory, "edsmsystems.json.gz");

                                ReportSyncProgress("Performing full download of System Data from EDSM");

                                Debug.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Full system download using URL " + EliteConfigInstance.InstanceConfig.EDSMFullSystemsURL);

                                bool success = BaseUtils.DownloadFile.HTTPDownloadFile(EliteConfigInstance.InstanceConfig.EDSMFullSystemsURL, edsmsystems, false, out bool newfile);

                                //edsmsystems = Path.Combine(EliteConfigInstance.InstanceOptions.AppDataDirectory, "edsmtest.json");

                                syncstate.perform_edsm_fullsync = false;

                                if (success)
                                {
                                    syncstate.edsm_fullsync_count = SystemsDatabase.Instance.UpgradeSystemTableFromFile(edsmsystems, grids, () => PendingClose, ReportSyncProgress);

                                    if (syncstate.edsm_fullsync_count < 0)     // this should always update something, the table is replaced.  If its not, its been cancelled
                                        return;

                                    BaseUtils.FileHelpers.DeleteFileNoError(edsmsystems);       // remove file - don't hold in storage
                                }
                                else
                                {
                                    ReportSyncProgress("");
                                    LogLineHighlight("Failed to download full EDSM systems file. Try re-running EDD later");
                                    BaseUtils.FileHelpers.DeleteFileNoError(edsmsystems);       // remove file - don't hold in storage
                                    return;     // new! if we failed to download, fail here, wait for another time
                                }

                            }
                            catch (Exception ex)
                            {
                                LogLineHighlight("GetAllEDSMSystems exception:" + ex.Message);
                            }
                        }

                        if (!PendingClose && syncstate.perform_edsm_alias_sync)
                        {
                            try
                            {
                                EDSMClass edsm = new EDSMClass();
                                string jsonhidden = edsm.GetHiddenSystems();

                                if (jsonhidden != null)
                                {
                                    SystemsDB.ParseAliasString(jsonhidden);
                                    syncstate.perform_edsm_alias_sync = false;

                                    SystemsDatabase.Instance.SetLastEDSMAliasDownloadTime();
                                }
                            }
                            catch (Exception ex)
                            {
                                LogLineHighlight("GetEDSMAlias exception: " + ex.Message);
                            }
                        }
                    }

                    if (!PendingClose)
                    {
                        syncstate.edsm_updatesync_count = UpdateSync(grids, () => PendingClose, ReportSyncProgress);
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
            Debug.Assert(System.Windows.Forms.Application.MessageLoop);

            if (syncstate.edsm_fullsync_count > 0 || syncstate.edsm_updatesync_count > 0)
                LogLine(string.Format("EDSM systems update complete with {0} systems".T(EDTx.EDDiscoveryController_EDSMU), syncstate.edsm_fullsync_count + syncstate.edsm_updatesync_count));

            if (syncstate.edsm_fullsync_count > 0 )   // if we have done a resync, or a major update sync (arb no)
            {
                LogLine("Refresh due to updating EDSM system data".T(EDTx.EDDiscoveryController_Refresh));
                RefreshHistoryAsync();
            }

            OnSyncComplete?.Invoke();

            ReportSyncProgress("");

            resyncEDSMRequestedFlag = 0;        // releases flag and allow another async to happen

            Debug.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Perform sync completed");
        }

        private static DateTime MinEDSMDate = new DateTime(2015, 1, 1);
        private static DateTime ED21date = new DateTime(2016, 5, 26);
        private static DateTime ED23date = new DateTime(2017, 4, 11);
        private static DateTime ED30date = new DateTime(2018, 2, 27);

        public long UpdateSync(bool[] grididallow, Func<bool> PendingClose, Action<string> ReportProgress)
        {
            DateTime lastrecordtime = SystemsDatabase.Instance.GetLastEDSMRecordTimeUTC();

            if (lastrecordtime < MinEDSMDate)
                lastrecordtime = MinEDSMDate;

            long updates = 0;

            double fetchmult = 1;

            while (lastrecordtime < DateTime.UtcNow.Subtract(new TimeSpan(0, 30, 0)))     // stop at X mins before now, so we don't get in a condition
            {                                                                           // where we do a set, the time moves to just before now, 
                                                                                        // and we then do another set with minimum amount of hours
                if (PendingClose())
                    return updates;

                if ( updates == 0)
                    LogLine("Checking for updated EDSM systems (may take a few moments).");

                EDSMClass edsm = new EDSMClass();

                double hourstofetch = 3;

                if (lastrecordtime < ED21date.AddHours(-48))
                    hourstofetch = 48;
                else if (lastrecordtime < ED23date.AddHours(-12))
                    hourstofetch = 12;
                else if (lastrecordtime < ED30date.AddHours(-6))
                    hourstofetch = 6;

                DateTime enddate = lastrecordtime + TimeSpan.FromHours(hourstofetch * fetchmult);
                if (enddate > DateTime.UtcNow)
                    enddate = DateTime.UtcNow;

                LogLine($"Downloading systems from UTC {lastrecordtime.ToUniversalTime().ToString()} to {enddate.ToUniversalTime().ToString()}");
                System.Diagnostics.Debug.WriteLine($"Downloading systems from UTC {lastrecordtime.ToUniversalTime().ToString()} to {enddate.ToUniversalTime().ToString()}");

                string json = null;
                BaseUtils.ResponseData response;
                try
                {
                    Stopwatch sw = new Stopwatch();
                    response = edsm.RequestSystemsData(lastrecordtime, enddate, timeout: 20000);
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

                    return updates;
                }
                catch (Exception ex)
                {
                    ReportProgress($"EDSM request failed");
                    LogLine($"Download of EDSM systems from the server failed ({ex.Message}), will try next time program is run");
                    return updates;
                }

                if (response.Error)
                {
                    if ((int)response.StatusCode == 429)
                    {
                        LogLine($"EDSM rate limit hit - waiting 2 minutes");
                        for (int sec = 0; sec < 120; sec++)
                        {
                            if (!PendingClose())
                            {
                                System.Threading.Thread.Sleep(1000);
                            }
                        }
                    }
                    else
                    {
                        LogLine($"Download of EDSM systems from the server failed ({response.StatusCode.ToString()}), will try next time program is run");
                        return updates;
                    }
                }

                json = response.Body;

                if (json == null)
                {
                    ReportProgress("EDSM request failed");
                    LogLine("Download of EDSM systems from the server failed (no data returned), will try next time program is run");
                    return updates;
                }

                // debug File.WriteAllText(@"c:\code\json.txt", json);

                DateTime prevrectime = lastrecordtime;
                System.Diagnostics.Debug.WriteLine("Last record time {0} JSON size {1}", lastrecordtime.ToUniversalTime(), json.Length);

                long updated = 0;

                try
                {
                    ReportProgress($"EDSM star database update from UTC " + lastrecordtime.ToUniversalTime().ToString());

                    updated = SystemsDB.ParseEDSMJSONString(json, grididallow, ref lastrecordtime, PendingClose, ReportProgress, "");
                    System.Diagnostics.Debug.WriteLine($".. Updated {updated} to {lastrecordtime.ToUniversalTime().ToString()}");
                    System.Diagnostics.Debug.WriteLine("Updated to time {0}", lastrecordtime.ToUniversalTime());

                    // if lastrecordtime did not change (=) or worse still, EDSM somehow moved the time back (unlikely)
                    if (lastrecordtime <= prevrectime)
                    {
                        lastrecordtime += TimeSpan.FromHours(12);       // Lets move on manually so we don't get stuck
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("SysClassEDSM.2 Exception " + e.ToString());
                    ReportProgress("EDSM request failed");
                    LogLine("Processing EDSM systems download failed, will try next time program is run");
                    return updates;
                }

                updates += updated;

                SystemsDatabase.Instance.SetLastEDSMRecordTimeUTC(lastrecordtime);       // keep on storing this in case next time we get an exception

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
                    if (!PendingClose())
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }

            return updates;
        }
    }
}

