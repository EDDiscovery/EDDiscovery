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

        //TBDpublic delegate void EDSMNewSystemEventHandler(object source);         // used for sync from not supported yet.
        //TBD removed public event EDSMNewSystemEventHandler OnNewEDSMTravelLog;

        public EDSMSync(EDDiscoveryForm frm)
        {
            mainForm = frm;
        }

        public bool StartSync(bool syncto, bool syncfrom, int defmapcolour)
        {
            if (running) // Only start once.
                return false;
            _syncTo = syncto;
            _syncFrom = syncfrom;
            _defmapcolour = defmapcolour;

            ThreadEDSMSync = new System.Threading.Thread(new System.Threading.ThreadStart(SyncThread));
            ThreadEDSMSync.Name = "EDSM Sync";
            ThreadEDSMSync.Start();

            return true;
        }

        public void StopSync()
        {
            Exit = true;
        }

        private void SyncThread()
        {
            running = true;
            Sync();
            running = false;
        }

        private void Sync()
        {
            try
            {
                EDSMClass edsm = new EDSMClass();
                edsm.apiKey = EDDiscoveryForm.EDDConfig.CurrentCommander.APIKey;
                edsm.commanderName = EDDiscoveryForm.EDDConfig.CurrentCommander.Name;

                if (edsm.apiKey.Length < 1 || edsm.commanderName.Length < 1)
                    return;

                List<HistoryEntry> edsmsystemlog = null;
                bool triedlogs = false;

                List<HistoryEntry> hlfsdunsyncedlist = mainForm.history.FilterByNotEDSMSyncedAndFSD;        // unsynced and FSD jumps only

                if ( _syncTo && hlfsdunsyncedlist.Count > 0 )                   // send systems to edsm
                {
                    mainForm.LogLine("EDSM: Sending " + hlfsdunsyncedlist.Count.ToString() + " flightlog entries");

                    edsm.GetLogs(new DateTime(2011, 1, 1), out edsmsystemlog);        // always returns a log, time is in UTC as per HistoryEntry and JournalEntry
                    triedlogs = true;

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
                            if ( edsm.SendTravelLog(he.System.name, he.EventTimeLocal, he.System.HasCoordinate, he.System.x, he.System.y, he.System.z, out errmsg) )
                                he.SetEdsmSync();

                            if (errmsg.Length > 0)
                                mainForm.LogLine(errmsg);
                        }
                    }

                    // TBD Comments to edsm?
                }

                if ( _syncFrom )                                                            // now do comments from edsm
                {
                    var json = edsm.GetComments(new DateTime(2011, 1, 1));

                    if (json != null)
                    {
                        JObject msg = JObject.Parse(json);
                        int msgnr = msg["msgnum"].Value<int>();

                        JArray comments = (JArray)msg["comments"];
                        if (comments != null)
                        {
                            foreach (JObject jo in comments)
                            {
                                string name = jo["system"].Value<string>();
                                string note = jo["comment"].Value<string>();
                                DateTime localtime = DateTime.ParseExact(jo["lastUpdate"].Value<string>(), "yyyy-MM-dd HH:mm:ss", 
                                            CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToLocalTime();

                                SystemNoteClass curnote = SystemNoteClass.GetNoteOnSystem(name);

                                if (curnote != null)
                                {
                                                                                // curnote uses local time to store
                                    if (localtime.Ticks > curnote.Time.Ticks)   // if newer, add on (verified with EDSM 29/9/2016)
                                    {
                                        curnote.Note += ". EDSM: " + note;
                                        curnote.Time = localtime;
                                        curnote.Update();
                                    }
                                }
                                else
                                {
                                    curnote = new SystemNoteClass();
                                    curnote.Note = note;
                                    curnote.Time = localtime;
                                    curnote.Name = name;
                                    curnote.Journalid = 0;
                                    curnote.Add();
                                }
                            }
                        }
                    }
                }

                int sysadded = 0;

                if (_syncFrom )
                {
                    if (!triedlogs)
                        edsm.GetLogs(new DateTime(2011, 1, 1), out edsmsystemlog);        // always returns a log

                    List<HistoryEntry> hlfsdlist = mainForm.history.FilterByFSD;  // FSD jumps only

                    TravelLogUnit tlu = null;

                    foreach (HistoryEntry he in edsmsystemlog)
                    {


                        //TBD - time zones..


                                // if we don't have an entry in our history, of a system name at a particular time
                                // some problem here, requires more debugging
                        if ( hlfsdlist.FindIndex(x=>x.System.name.Equals(he.System.name) && x.EventTimeUTC == he.EventTimeUTC) < 0 )   
                        {
                            if (tlu == null) // If we dont have a travellogunit yet then create it. 
                            {
                                tlu = new TravelLogUnit();

                                tlu.type = 2;  // EDSM
                                tlu.Name = "EDSM-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                tlu.Size = 0;
                                tlu.Path = "EDSM";
                                tlu.CommanderId = EDDiscoveryForm.EDDConfig.CurrentCommander.Nr;

                                tlu.Add();  // Add to Database
                            }

                            EDDiscovery.EliteDangerous.JournalEntry je =
                                EDDiscovery.EliteDangerous.JournalEntry.CreateFSDJournalEntry(tlu.id, tlu.CommanderId.Value, he.EventTimeUTC,
                                                                                              he.System.name, he.System.x, he.System.y, he.System.z,
                                                                                              _defmapcolour);

                            je.Add();
                            System.Diagnostics.Trace.WriteLine(string.Format("Add EDSM system {0}", he.System.name));

                            sysadded++;
                            if (sysadded == 100)
                                break;
                            break;
                        }
                    }

                    mainForm.LogLine(string.Format("EDSM downloaded {0} systems" , sysadded));
                }

                mainForm.LogLine("EDSM sync Done");

                //                if (newsystem)
                //OnNewEDSMTravelLog(this);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception ex:" + ex.Message);
                mainForm.LogLineHighlight("EDSM sync Exception " + ex.Message);
            }
        }

        public static void SendTravelLog(HistoryEntry he) // (verified with EDSM 29/9/2016)
        {
            EDSMClass edsm = new EDSMClass();
            edsm.apiKey = EDDiscoveryForm.EDDConfig.CurrentCommander.APIKey;
            edsm.commanderName = EDDiscoveryForm.EDDConfig.CurrentCommander.Name;

            if (edsm.apiKey.Length < 1 || edsm.commanderName.Length < 1)
                return;

            string errmsg;
            Task taskEDSM = Task.Factory.StartNew(() =>
            {                                                   // LOCAL time, there is a UTC converter inside this call
                if (edsm.SendTravelLog(he.System.name, he.EventTimeLocal, he.System.HasCoordinate, he.System.x, he.System.y, he.System.z, out errmsg))
                    he.SetEdsmSync();
            });
        }

        public static void SendComments(string star , string note) // (verified with EDSM 29/9/2016)
        {
            EDSMClass edsm = new EDSMClass();

            edsm.apiKey = EDDiscoveryForm.EDDConfig.CurrentCommander.APIKey;
            edsm.commanderName = EDDiscoveryForm.EDDConfig.CurrentCommander.Name;

            if (edsm.apiKey.Length < 1 || edsm.commanderName.Length < 1)
                return;

            Task taskEDSM = Task.Factory.StartNew(() =>
            {
                edsm.SetComment(star, note);
            });
        }

    }
}
