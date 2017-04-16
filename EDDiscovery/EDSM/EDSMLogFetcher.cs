using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EDDiscovery.DB;
using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;
using System.Globalization;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace EDDiscovery.EDSM
{
    public class EDSMLogFetcher
    {
        private static DateTime GammaStart = new DateTime(2014, 11, 22, 4, 0, 0, DateTimeKind.Utc);
        private static int EDSMMaxLogAgeMinutes = 15; // Fetch new logs every 15 minutes

        private Thread ThreadEDSMFetchLogs;
        private ManualResetEvent ExitRequested = new ManualResetEvent(false);
        private EDCommander Commander;
        private Action<string> LogLine;
        private DateTime EDSMRequestBackoffTime = DateTime.UtcNow;
        private TimeSpan BackoffInterval = TimeSpan.FromSeconds(60);

        public DateTime FirstEventTime { get; private set; }
        public DateTime LastEventTime { get; private set; }

        public int CommanderId { get { return Commander.Nr; } }

        public delegate void EDSMDownloadedSystems();
        public event EDSMDownloadedSystems OnDownloadedSystems;

        public EDSMLogFetcher(int cmdrid, Action<string> logline)
        {
            Commander = EDCommander.GetCommander(cmdrid);
            LogLine = logline;
        }

        public void Start()
        {
            ExitRequested.Reset();

            if ((ThreadEDSMFetchLogs == null || !ThreadEDSMFetchLogs.IsAlive) && Commander.SyncFromEdsm)
            {
                ThreadEDSMFetchLogs = new Thread(FetcherThreadProc) { IsBackground = true, Name = "EDSM Log Fetcher" };
                ThreadEDSMFetchLogs.Start();
            }
        }

        public void Stop()
        {
            ExitRequested.Set();
        }

        private void FetcherThreadProc()
        {
            bool jupdate = false;
            LastEventTime = DateTime.UtcNow;
            FirstEventTime = LastEventTime;

            int waittime = 2000; // Max 1 request every 2 seconds, with a backoff if the rate limit is hit
            if (EDSMRequestBackoffTime > DateTime.UtcNow)
            {
                waittime = (int)Math.Min(EDSMMaxLogAgeMinutes * 60000, Math.Min(BackoffInterval.TotalSeconds * 1000, EDSMRequestBackoffTime.Subtract(DateTime.UtcNow).TotalSeconds * 1000));
            }

            while (!ExitRequested.WaitOne(waittime))
            {
                EDSMClass edsm = new EDSMClass { apiKey = Commander.APIKey, commanderName = Commander.EdsmName };
                List<HistoryEntry> edsmlogs = null;
                DateTime logstarttime = DateTime.MinValue;
                DateTime logendtime = DateTime.MinValue;
                int res = -1;

                if (edsm.IsApiKeySet && Commander.SyncFromEdsm && DateTime.UtcNow > EDSMRequestBackoffTime)
                {
                    if (DateTime.UtcNow.Subtract(LastEventTime).TotalMinutes >= EDSMMaxLogAgeMinutes)
                    {
                        Trace.WriteLine($"Retrieving logs starting {LastEventTime}");
                        res = edsm.GetLogs(LastEventTime, null, out edsmlogs, out logstarttime, out logendtime);
                    }
                    else if (FirstEventTime > GammaStart)
                    {
                        Trace.WriteLine($"Retrieving logs ending {FirstEventTime}");
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
                else if (res == 100 && edsmlogs != null)
                {
                    BackoffInterval = TimeSpan.FromSeconds(60);

                    if (logendtime > DateTime.UtcNow)
                        logendtime = DateTime.UtcNow;

                    List<HistoryEntry> hlfsdlist = JournalEntry.GetAll(Commander.Nr).OfType<JournalLocOrJump>().OrderBy(je => je.EventTimeUTC).Select(je => HistoryEntry.FromJournalEntry(je, null, false, out jupdate)).ToList();
                    HistoryList hl = new HistoryList(hlfsdlist);
                    List<DateTime> hlfsdtimes = hlfsdlist.Select(he => he.EventTimeUTC).ToList();

                    List<HistoryEntry> toadd = new List<HistoryEntry>();

                    int previdx = -1;
                    foreach (HistoryEntry he in edsmlogs)      // find out list of ones not present
                    {
                        int index = hlfsdlist.FindIndex(x => x.System.name.Equals(he.System.name, StringComparison.InvariantCultureIgnoreCase) && x.EventTimeUTC.Ticks == he.EventTimeUTC.Ticks);

                        if (index < 0)
                        {
                            // Look for any entries where DST may have thrown off the time
                            foreach (var vi in hlfsdlist.Select((v, i) => new { v = v, i = i }).Where(vi => vi.v.System.name.Equals(he.System.name, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                if (vi.i > previdx)
                                {
                                    double hdiff = vi.v.EventTimeUTC.Subtract(he.EventTimeUTC).TotalHours;
                                    if (hdiff >= -2 && hdiff <= 2 && hdiff == Math.Floor(hdiff))
                                    {
                                        if (vi.v.System.id_edsm <= 0)
                                        {
                                            vi.v.System.id_edsm = 0;
                                            hl.FillEDSM(vi.v);
                                        }

                                        if (vi.v.System.id_edsm <= 0 || vi.v.System.id_edsm == he.System.id_edsm)
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
                                lhe.SetFirstDiscover();
                            }

                            previdx = index;
                        }
                    }

                    if (toadd.Count > 0)  // if we have any, we can add 
                    {
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
                                JObject jo = EDDiscovery.EliteDangerous.JournalEntry.CreateFSDJournalEntryJson(he.EventTimeUTC,
                                                                                                  he.System.name, he.System.x, he.System.y, he.System.z,
                                                                                                  EDDConfig.Instance.DefaultMapColour);
                                EDDiscovery.EliteDangerous.JournalEntry je =
                                    EDDiscovery.EliteDangerous.JournalEntry.CreateFSDJournalEntry(tlu.id, tlu.CommanderId.Value,
                                                                                                  (int)EDDiscovery.EliteDangerous.SyncFlags.EDSM, jo);

                                System.Diagnostics.Trace.WriteLine(string.Format("Add {0} {1}", je.EventTimeUTC, he.System.name));
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

                    if (logstarttime < FirstEventTime)
                        FirstEventTime = logstarttime;

                    if (logendtime > LastEventTime)
                        LastEventTime = logendtime;
                }
            }
        }
    }
}
