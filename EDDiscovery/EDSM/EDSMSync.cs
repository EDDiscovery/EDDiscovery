using EDDiscovery;
using EDDiscovery.DB;
using EDDiscovery2.DB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EDDiscovery2.EDSM
{  
    public class EDSMSync
    {
        Thread ThreadEDSMSync;
        bool running = false;
        bool Exit = false;
        bool _syncTo = false;
        bool _syncFrom = false;
        int _defmapcolour = 0;
        private EDDiscoveryForm mainForm;

        public delegate void EDSMDownloadedSystems();         // used for sync from not supported yet.
        public event EDSMDownloadedSystems OnDownloadedSystems;

        public EDSMSync(EDDiscoveryForm frm)
        {
            mainForm = frm;
        }

        public bool StartSync(EDSMClass edsm, bool syncto, bool syncfrom, int defmapcolour)
        {
            if (running) // Only start once.
                return false;
            _syncTo = syncto;
            _syncFrom = syncfrom;
            _defmapcolour = defmapcolour;

            ThreadEDSMSync = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(SyncThread));
            ThreadEDSMSync.Name = "EDSM Sync";
            ThreadEDSMSync.IsBackground = true;
            ThreadEDSMSync.Start(edsm);

            return true;
        }

        public void StopSync()
        {
            Exit = true;
        }

        private void SyncThread(object _edsm)
        {
            EDSMClass edsm = (EDSMClass)_edsm;
            running = true;
            Sync(edsm);
            running = false;
        }

        private void Sync(EDSMClass edsm)
        {
            try
            {
                mainForm.LogLine("EDSM sync begin");

                List<HistoryEntry> hlfsdunsyncedlist = mainForm.history.FilterByNotEDSMSyncedAndFSD;        // first entry is oldest

                if ( _syncTo && hlfsdunsyncedlist.Count > 0 )                   // send systems to edsm (verified with dates, 29/9/2016, utc throughout)
                {
                    DateTime utcmin = hlfsdunsyncedlist[0].EventTimeUTC.AddDays(-1);        // 1 days for margin ;-)  only get them back to this date for speed..

                    mainForm.LogLine("EDSM: Sending " + hlfsdunsyncedlist.Count.ToString() + " flightlog entries");

                    List<HistoryEntry> edsmsystemlog = null;
                    edsm.GetLogs(utcmin, out edsmsystemlog);        // always returns a log, time is in UTC as per HistoryEntry and JournalEntry

                    int edsmsystemssent = 0;

                    foreach (var he in hlfsdunsyncedlist)
                    {
                        if (Exit)
                        {
                            running = false;
                            return;
                        }

                        HistoryEntry ps2 = (from c in edsmsystemlog where c.System.name == he.System.name && c.EventTimeUTC.Ticks == he.EventTimeUTC.Ticks select c).FirstOrDefault();

                        if (ps2 != null)                // it did, just make sure EDSM sync flag is set..
                        {
                            he.SetEdsmSync();
                        }
                        else
                        {
                            string errmsg;              // (verified with EDSM 29/9/2016)

                                                        // it converts to UTC inside the function, supply local for now
                            if ( edsm.SendTravelLog(he.System.name, he.EventTimeUTC, he.System.HasCoordinate && !he.IsStarPosFromEDSM, he.System.x, he.System.y, he.System.z, out errmsg) )
                                he.SetEdsmSync();

                            if (errmsg.Length > 0)
                                mainForm.LogLine(errmsg);

                            edsmsystemssent++;
                        }
                    }

                    mainForm.LogLine(string.Format("EDSM Systems sent {0}", edsmsystemssent));
                }

                // TBD Comments to edsm?

                if ( _syncFrom )                                                            // Verified ok with time 29/9/2016
                {
                    var json = edsm.GetComments(new DateTime(2011, 1, 1));

                    if (json != null)
                    {
                        JObject msg = JObject.Parse(json);
                        int msgnr = msg["msgnum"].Value<int>();

                        JArray comments = (JArray)msg["comments"];
                        if (comments != null)
                        {
                            int commentsadded = 0;

                            foreach (JObject jo in comments)
                            {
                                string name = jo["system"].Value<string>();
                                string note = jo["comment"].Value<string>();
                                string utctime = jo["lastUpdate"].Value<string>();
                                int edsmid = 0;

                                if (!Int32.TryParse(JSONHelper.GetStringDef(jo["systemId"], "0"), out edsmid))
                                    edsmid = 0;

                                DateTime localtime = DateTime.ParseExact(utctime, "yyyy-MM-dd HH:mm:ss", 
                                            CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToLocalTime();

                                SystemNoteClass curnote = SystemNoteClass.GetNoteOnSystem(name, edsmid);

                                if (curnote != null)
                                {
                                                                                // curnote uses local time to store
                                    if (localtime.Ticks > curnote.Time.Ticks)   // if newer, add on (verified with EDSM 29/9/2016)
                                    {
                                        curnote.Note += ". EDSM: " + note;
                                        curnote.Time = localtime;
                                        curnote.EdsmId = edsmid;
                                        curnote.Update();
                                        commentsadded++;
                                    }
                                }
                                else
                                {
                                    curnote = new SystemNoteClass();
                                    curnote.Note = note;
                                    curnote.Time = localtime;
                                    curnote.Name = name;
                                    curnote.Journalid = 0;
                                    curnote.EdsmId = edsmid;
                                    curnote.Add();
                                    commentsadded++;
                                }
                            }

                            mainForm.LogLine(string.Format("EDSM Comments downloaded/updated {0}", commentsadded));
                        }
                    }
                }

                if (_syncFrom )     // verified after struggle 29/9/2016
                {
                    List<HistoryEntry> edsmsystemlog = null;
                    edsm.GetLogs(new DateTime(2011, 1, 1), out edsmsystemlog);        // get the full list of systems
                    edsmsystemlog = edsmsystemlog.OrderBy(s => s.EventTimeUTC).ToList();

                    List<HistoryEntry> hlfsdlist = mainForm.history.FilterByTravel.Where(h => h.IsLocOrJump).OrderBy(h => h.EventTimeUTC).ToList();  // FSD jumps only
                    
                    List<HistoryEntry> toadd = new List<HistoryEntry>();

                    int previdx = -1;
                    foreach (HistoryEntry he in edsmsystemlog)      // find out list of ones not present
                    {
                        int index = hlfsdlist.FindIndex(x => x.System.name.Equals(he.System.name, StringComparison.InvariantCultureIgnoreCase) && x.EventTimeUTC.Ticks == he.EventTimeUTC.Ticks);

                        if (index < 0)
                        {
                            // Look for any entries where DST may have thrown off the time
                            foreach (var vi in hlfsdlist.Select((v,i) => new {v = v, i = i}).Where(vi => vi.v.System.name.Equals(he.System.name, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                if (vi.i > previdx)
                                {
                                    double hdiff = vi.v.EventTimeUTC.Subtract(he.EventTimeUTC).TotalHours;
                                    if (hdiff >= -2 && hdiff <= 2 && hdiff == Math.Floor(hdiff))
                                    {
                                        if (vi.v.System.id_edsm <= 0)
                                        {
                                            vi.v.System.id_edsm = 0;
                                            mainForm.history.FillEDSM(vi.v);
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
                            previdx = index;
                        }
                    }

                    if ( toadd.Count >0 )  // if we have any, we can add 
                    {
                        TravelLogUnit tlu = new TravelLogUnit();    // need a tlu for it
                        tlu.type = 2;  // EDSM
                        tlu.Name = "EDSM-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        tlu.Size = 0;
                        tlu.Path = "EDSM";
                        tlu.CommanderId = EDDiscoveryForm.EDDConfig.CurrentCommander.Nr;
                        tlu.Add();  // Add to Database

                        using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
                        {
                            foreach (HistoryEntry he in toadd)
                            {
                                EDDiscovery.EliteDangerous.JournalEntry je =
                                    EDDiscovery.EliteDangerous.JournalEntry.CreateFSDJournalEntry(tlu.id, tlu.CommanderId.Value, he.EventTimeUTC,
                                                                                                  he.System.name, he.System.x, he.System.y, he.System.z,
                                                                                                  _defmapcolour, (int)EDDiscovery.EliteDangerous.SyncFlags.EDSM);

                                System.Diagnostics.Trace.WriteLine(string.Format("Add {0} {1}", je.EventTimeUTC, he.System.name));
                                je.Add(cn);
                            }
                        }

                        if (OnDownloadedSystems != null)
                            OnDownloadedSystems();

                        mainForm.LogLine(string.Format("EDSM downloaded {0} systems", toadd.Count));
                    }
                }

                mainForm.LogLine("EDSM sync Done");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception ex:" + ex.Message);
                mainForm.LogLineHighlight("EDSM sync Exception " + ex.Message);
            }
        }

        public static void SendTravelLog(HistoryEntry he) // (verified with EDSM 29/9/2016, seen UTC time being sent, and same UTC time on ESDM).
        {
            EDSMClass edsm = new EDSMClass();

            if (!edsm.IsApiKeySet)
                return;

            string errmsg;
            Task taskEDSM = Task.Factory.StartNew(() =>
            {                                                   // LOCAL time, there is a UTC converter inside this call
                if (edsm.SendTravelLog(he.System.name, he.EventTimeUTC, he.System.HasCoordinate, he.System.x, he.System.y, he.System.z, out errmsg))
                    he.SetEdsmSync();
            });
        }

        public static void SendComments(string star , string note, long edsmid = 0) // (verified with EDSM 29/9/2016)
        {
            EDSMClass edsm = new EDSMClass();

            if (!edsm.IsApiKeySet)
                return;

            Task taskEDSM = Task.Factory.StartNew(() =>
            {
                edsm.SetComment(star, note, edsmid);
            });
        }

    }
}
