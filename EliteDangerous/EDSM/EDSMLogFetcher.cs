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
        private DateTime EDSMRequestBackoffTime = DateTime.UtcNow;
        private TimeSpan BackoffInterval = TimeSpan.FromSeconds(60);

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

            int waittime = 2000; // Max 1 request every 2 seconds, with a backoff if the rate limit is hit
            if (EDSMRequestBackoffTime > DateTime.UtcNow)
            {
                waittime = (int)Math.Min(EDSMMaxLogAgeMinutes * 60000, Math.Min(BackoffInterval.TotalSeconds * 1000, EDSMRequestBackoffTime.Subtract(DateTime.UtcNow).TotalSeconds * 1000));
            }

            while (!ExitRequested.WaitOne(waittime))
            {
                EDSMClass edsm = new EDSMClass(Commander); 
                List<HistoryEntry> edsmlogs = null;
                DateTime logstarttime = DateTime.MinValue;
                DateTime logendtime = DateTime.MinValue;
                int res = -1;

                if (edsm.ValidCredentials && DateTime.UtcNow > lastCommentFetch.AddHours(1))
                {
                    edsm.GetComments(l => Trace.WriteLine(l));
                    lastCommentFetch = DateTime.UtcNow;
                }

                if (edsm.ValidCredentials && Commander.SyncFromEdsm && DateTime.UtcNow > EDSMRequestBackoffTime)
                {
                    if (DateTime.UtcNow.Subtract(LastEventTime).TotalMinutes >= EDSMMaxLogAgeMinutes)
                    {
                        //Trace.WriteLine($"Retrieving EDSM logs starting {LastEventTime}");
                        res = edsm.GetLogs(LastEventTime, null, out edsmlogs, out logstarttime, out logendtime);
                    }
                    else if (FirstEventTime > GammaStart)
                    {
                        //Trace.WriteLine($"Retrieving EDSM logs ending {FirstEventTime}");
                        res = edsm.GetLogs(null, FirstEventTime, out edsmlogs, out logstarttime, out logendtime);
                    }
                }

                if (ExitRequested.WaitOne(0))
                {
                    return;
                }

                if (res == 429) // Rate Limit Exceeded
                {
                    Trace.WriteLine($"EDSM Log request rate limit hit - backing off for {BackoffInterval.TotalSeconds}s");
                    EDSMRequestBackoffTime = DateTime.UtcNow + BackoffInterval;
                    BackoffInterval = BackoffInterval + TimeSpan.FromSeconds(60);
                }
                else if (logstarttime > LastEventTime && logendtime < FirstEventTime)
                {
                    Trace.WriteLine($"Bad start and/or end times returned by EDSM - backing off for {BackoffInterval.TotalSeconds}s");
                    EDSMRequestBackoffTime = DateTime.UtcNow + BackoffInterval;
                    BackoffInterval = BackoffInterval + TimeSpan.FromSeconds(60);
                }
                else if (res == 100 && edsmlogs != null )
                {
                    if (edsmlogs.Count > 0)     // if anything to process..
                    {
                        //Trace.WriteLine($"Retrieving EDSM logs count {edsmlogs.Count}");

                        BackoffInterval = TimeSpan.FromSeconds(60);

                        if (logendtime > DateTime.UtcNow)
                            logendtime = DateTime.UtcNow;
                        if (logstarttime < DateTime.MinValue.AddDays(1))
                            logstarttime = DateTime.MinValue.AddDays(1);

                        // Get all of the local entries now that we have the entries from EDSM
                        // Moved here to avoid the race that could have been causing duplicate entries
                        List<HistoryEntry> hlfsdlist = JournalEntry.GetAll(Commander.Nr, logstarttime.AddDays(-1), logendtime.AddDays(1)).OfType<JournalLocOrJump>().OrderBy(je => je.EventTimeUTC).Select(je => HistoryEntry.FromJournalEntry(je, null, out jupdate)).ToList();

                        HistoryList hl = new HistoryList(hlfsdlist);
                        List<DateTime> hlfsdtimes = hlfsdlist.Select(he => he.EventTimeUTC).ToList();

                        List<HistoryEntry> toadd = new List<HistoryEntry>();

                        int previdx = -1;
                        foreach (HistoryEntry he in edsmlogs)      // find out list of ones not present
                        {
                            int index = hlfsdlist.FindIndex(x => x.System.Name.Equals(he.System.Name, StringComparison.InvariantCultureIgnoreCase) && x.EventTimeUTC.Ticks == he.EventTimeUTC.Ticks);

                            if (index < 0)
                            {
                                // Look for any entries where DST may have thrown off the time
                                foreach (var vi in hlfsdlist.Select((v, i) => new { v = v, i = i }).Where(vi => vi.v.System.Name.Equals(he.System.Name, StringComparison.InvariantCultureIgnoreCase)))
                                {
                                    if (vi.i > previdx)
                                    {
                                        double hdiff = vi.v.EventTimeUTC.Subtract(he.EventTimeUTC).TotalHours;
                                        if (hdiff >= -2 && hdiff <= 2 && hdiff == Math.Floor(hdiff))
                                        {
                                            if (vi.v.System.EDSMID <= 0)
                                            {
                                                vi.v.System.EDSMID = 0;
                                                hl.FillEDSM(vi.v);
                                            }

                                            if (vi.v.System.EDSMID <= 0 || vi.v.System.EDSMID == he.System.EDSMID)
                                            {
                                                index = vi.i;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (index < 0)
                            {
                                toadd.Add(he);
                            }
                            else
                            {
                                HistoryEntry lhe = hlfsdlist[index];

                                if (he.IsEDSMFirstDiscover && !lhe.IsEDSMFirstDiscover)
                                {
                                    lhe.SetFirstDiscover(true);
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
                                foreach (HistoryEntry he in toadd)
                                {
                                    JObject jo = JournalEntry.CreateFSDJournalEntryJson(he.EventTimeUTC,
                                                                                                      he.System.Name, he.System.X, he.System.Y, he.System.Z,
                                                                                                      EliteConfigInstance.InstanceConfig.DefaultMapColour);
                                    JournalEntry je =
                                        JournalEntry.CreateFSDJournalEntry(tlu.id, tlu.CommanderId.Value,
                                                                                                      (int)SyncFlags.EDSM, jo, he.System.EDSMID);

                                    System.Diagnostics.Trace.WriteLine(string.Format("Add {0} {1}", je.EventTimeUTC, he.System.Name));
                                    je.Add(jo, cn);
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

                    if (logstarttime < FirstEventTime)
                        FirstEventTime = logstarttime;

                    if (logendtime > LastEventTime)
                        LastEventTime = logendtime;
                }
            }
        }
    }
}
