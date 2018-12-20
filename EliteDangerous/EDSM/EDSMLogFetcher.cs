using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Globalization;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using EliteDangerousCore.JournalEvents;
using EliteDangerousCore.DB;

namespace EliteDangerousCore.EDSM
{
    public class EDSMLogFetcher
    {
        private static DateTime GammaStart = new DateTime(2014, 11, 22, 4, 0, 0, DateTimeKind.Utc);
        private static int EDSMMaxLogAgeMinutes = 15; // Fetch new logs every 15 minutes

        private Thread ThreadEDSMFetchLogs;
        private ManualResetEvent ExitRequested = new ManualResetEvent(false);
        private Action<string> LogLine;

        public DateTime FirstEventTime { get; private set; }
        public DateTime LastEventTime { get; private set; }

        public delegate void EDSMDownloadedSystems();
        public event EDSMDownloadedSystems OnDownloadedSystems;

        private EDCommander Commander = null;
        private int CommanderId { get { return Commander.Nr; } }

        public EDSMLogFetcher(Action<string> logline)
        {
            LogLine = logline;
        }

        public void Start( EDCommander cmdr )
        {
            ExitRequested.Reset();

            Trace.WriteLine($"EDSM Fetch logs start with cmdr {cmdr.Nr}");

            if ( !object.ReferenceEquals(Commander,cmdr) )
            {
                Trace.WriteLine($"EDSM Fetch logs restart time");
                LastEventTime = DateTime.UtcNow;
                FirstEventTime = LastEventTime;
            }

            Commander = cmdr;

            if (( ThreadEDSMFetchLogs == null || !ThreadEDSMFetchLogs.IsAlive) && Commander.SyncFromEdsm && EDSMClass.IsServerAddressValid )
            {
                ThreadEDSMFetchLogs = new Thread(FetcherThreadProc) { IsBackground = true, Name = "EDSM Log Fetcher" };
                ThreadEDSMFetchLogs.Start();
            }
        }

        public void AsyncStop()
        {
            ExitRequested.Set();
        }

        public void StopCheck()
        {
            if (ThreadEDSMFetchLogs != null)
            {
                ExitRequested.Set();
                ThreadEDSMFetchLogs.Join(); // wait for exit.
                ThreadEDSMFetchLogs = null;
            }
        }

        private void FetcherThreadProc()
        {
            Trace.WriteLine($"EDSM Thread logs start");
            bool jupdate = false;
            DateTime lastCommentFetch = DateTime.MinValue;

            int waittime = 1000; // initial waittime, will be reestimated later

            while (!ExitRequested.WaitOne(waittime))
            {
                EDSMClass edsm = new EDSMClass(Commander); 

                if (edsm.ValidCredentials && DateTime.UtcNow > lastCommentFetch.AddHours(1))
                {
                    edsm.GetComments(l => Trace.WriteLine(l));
                    lastCommentFetch = DateTime.UtcNow;
                }

                DateTime logstarttime = DateTime.MinValue;      // return what we got..
                DateTime logendtime = DateTime.MinValue;
                List<JournalFSDJump> edsmlogs = null;
                int res = -1;       //return code
                BaseUtils.ResponseData response = default(BaseUtils.ResponseData);

                if (edsm.ValidCredentials && Commander.SyncFromEdsm )       // this triggers in if commander details change
                {
                    if (DateTime.UtcNow.Subtract(LastEventTime).TotalMinutes >= EDSMMaxLogAgeMinutes)
                    {
                        System.Diagnostics.Debug.WriteLine($"Retrieving EDSM logs starting {LastEventTime}");
                        res = edsm.GetLogs(LastEventTime, null, out edsmlogs, out logstarttime, out logendtime, out response);
                    }
                    else if (FirstEventTime > GammaStart)
                    {
                        System.Diagnostics.Debug.WriteLine($"Retrieving EDSM logs ending {FirstEventTime}");
                        res = edsm.GetLogs(null, FirstEventTime, out edsmlogs, out logstarttime, out logendtime, out response);
                    }
                }

                if (ExitRequested.WaitOne(0))
                {
                    return;
                }

                if (res == 429) // Rate Limit Exceeded
                {
                    Trace.WriteLine($"EDSM Log request rate limit hit - backing off");
                }
                else if (logstarttime > LastEventTime && logendtime < FirstEventTime)
                {
                    Trace.WriteLine($"Bad start and/or end times returned by EDSM - backing off");
                }
                else if (res == 100 )       // okay return..
                {
                    if (edsmlogs != null && edsmlogs.Count > 0)     // if anything to process..
                    {
                        //Trace.WriteLine($"Retrieving EDSM logs count {edsmlogs.Count}");

                        if (logendtime > DateTime.UtcNow)               // make sure logendtime sane
                            logendtime = DateTime.UtcNow;
                        if (logstarttime < DateTime.MinValue.AddDays(1))    // and starttime
                            logstarttime = DateTime.MinValue.AddDays(1);

                        // Get all of the local entries now that we have the entries from EDSM
                        // Moved here to avoid the race that could have been causing duplicate entries

                        // EDSM only returns FSD entries, so only look for them.  Tested 27/4/2018 after the HE optimisations

                        List<HistoryEntry> hlfsdlist = JournalEntry.GetAll(Commander.Nr, logstarttime.AddDays(-1), logendtime.AddDays(1)).
                            OfType<JournalLocOrJump>().OrderBy(je => je.EventTimeUTC).
                            Select(je => HistoryEntry.FromJournalEntry(je, null, out jupdate)).ToList();    // using HE just because of the FillEDSM func

                        HistoryList hl = new HistoryList(hlfsdlist);        // just so we can access the FillEDSM func

                        List<JournalFSDJump> toadd = new List<JournalFSDJump>();

                        int previdx = -1;
                        foreach (JournalFSDJump jfsd in edsmlogs)      // find out list of ones not present
                        {
                            int index = hlfsdlist.FindIndex(x => x.System.Name.Equals(jfsd.StarSystem, StringComparison.InvariantCultureIgnoreCase) && x.EventTimeUTC.Ticks == jfsd.EventTimeUTC.Ticks);

                            if (index < 0)
                            {
                                // Look for any entries where DST may have thrown off the time
                                foreach (var vi in hlfsdlist.Select((v, i) => new { v = v, i = i }).Where(vi => vi.v.System.Name.Equals(jfsd.StarSystem, StringComparison.InvariantCultureIgnoreCase)))
                                {
                                    if (vi.i > previdx)
                                    {
                                        double hdiff = vi.v.EventTimeUTC.Subtract(jfsd.EventTimeUTC).TotalHours;
                                        if (hdiff >= -2 && hdiff <= 2 && hdiff == Math.Floor(hdiff))
                                        {
                                            if (vi.v.System.EDSMID <= 0)        // if we don't have a valid EDMSID..
                                            {
                                                vi.v.System.EDSMID = 0;
                                                hl.FillEDSM(vi.v);
                                            }

                                            if (vi.v.System.EDSMID <= 0 || vi.v.System.EDSMID == jfsd.EdsmID)
                                            {
                                                index = vi.i;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (index < 0)      // its not a duplicate, add to db
                            {
                                toadd.Add(jfsd);
                            }
                            else
                            {                   // it is a duplicate, check if the first discovery flag is set right
                                JournalFSDJump existingfsd = hlfsdlist[index].journalEntry as JournalFSDJump;

                                if (existingfsd != null && existingfsd.EDSMFirstDiscover != jfsd.EDSMFirstDiscover)    // if we have a FSD one, and first discover is different
                                {
                                    existingfsd.UpdateFirstDiscover(jfsd.EDSMFirstDiscover);

                                }

                                previdx = index;
                            }
                        }

                        if (toadd.Count > 0)  // if we have any, we can add 
                        {
                            Trace.WriteLine($"Adding EDSM logs count {toadd.Count}");

                            TravelLogUnit tlu = new TravelLogUnit();    // need a tlu for it
                            tlu.type = 2;  // EDSM
                            tlu.Name = "EDSM-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                            tlu.Size = 0;
                            tlu.Path = "EDSM";
                            tlu.CommanderId = EDCommander.CurrentCmdrID;
                            tlu.Add();  // Add to Database

                            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
                            {
                                foreach (JournalFSDJump jfsd in toadd)
                                {
                                    System.Diagnostics.Trace.WriteLine(string.Format("Add {0} {1}", jfsd.EventTimeUTC, jfsd.StarSystem));
                                    jfsd.SetTLUCommander(tlu.id, tlu.CommanderId.Value);        // update its TLU id to the TLU made above
                                    jfsd.Add(jfsd.CreateFSDJournalEntryJson(), cn);     // add it to the db with the JSON created
                                }
                            }

                            LogLine($"Retrieved {toadd.Count} log entries from EDSM, from {logstarttime.ToLocalTime().ToString()} to {logendtime.ToLocalTime().ToString()}");

                            if (logendtime > LastEventTime || logstarttime <= GammaStart)
                            {
                                if (OnDownloadedSystems != null)
                                    OnDownloadedSystems();
                            }
                        }
                    }

                    if (logstarttime < FirstEventTime)      // move time on.. we got a 100, thats it for this period
                        FirstEventTime = logstarttime;

                    if (logendtime > LastEventTime)     
                        LastEventTime = logendtime;

                    if (response.Headers != null &&
                        response.Headers["X-Rate-Limit-Limit"] != null &&
                        response.Headers["X-Rate-Limit-Remaining"] != null &&
                        response.Headers["X-Rate-Limit-Reset"] != null &&
                        Int32.TryParse(response.Headers["X-Rate-Limit-Limit"], out int ratelimitlimit) &&
                        Int32.TryParse(response.Headers["X-Rate-Limit-Remaining"], out int ratelimitremain) &&
                        Int32.TryParse(response.Headers["X-Rate-Limit-Reset"], out int ratelimitreset) )
                    {
                        if (ratelimitremain < ratelimitlimit * 2 / 4)       // lets keep at least X remaining for other purposes later..
                            waittime = 1000 * ratelimitreset / (ratelimitlimit - ratelimitremain);    // slow down to its pace now.. example 878/(360-272) = 10 seconds per quota
                        else
                            waittime = 1000;        // 1 second so we don't thrash

                        System.Diagnostics.Debug.WriteLine("EDSM Log Delay Parameters {0} {1} {2} => {3}ms", ratelimitlimit, ratelimitremain, ratelimitreset, waittime);
                    }
                }
            }
        }
    }
}
