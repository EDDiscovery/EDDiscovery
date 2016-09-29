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


        public void Sync()
        {
            try
            {
                EDSMClass edsm = new EDSMClass();

                edsm.apiKey =  EDDiscoveryForm.EDDConfig.CurrentCommander.APIKey;
                edsm.commanderName = EDDiscoveryForm.EDDConfig.CurrentCommander.Name;

                List<HistoryEntry> log;
                edsm.GetLogs(new DateTime(2011, 1, 1), out log);
                if (log == null)
                    log = new List<HistoryEntry>();

                if (_syncTo)        // send systems to EDSM..
                {
                    List<HistoryEntry> hlfsdunsyncedlist = mainForm.history.FilterByNotEDSMSyncedAndFSD;        // unsynced and FSD jumps only

                    mainForm.LogLine("EDSM: Sending " + hlfsdunsyncedlist.Count.ToString() + " flightlog entries");

                    foreach (var he in hlfsdunsyncedlist)
                    {
                        if (Exit)
                        {
                            running = false;
                            return;
                        }

                        if (  he.EdsmSync == false)
                        {
                            HistoryEntry ps2 = (from c in log where c.System.name == he.System.name && c.EventTime.Ticks == he.EventTime.Ticks select c).FirstOrDefault();

                            if (ps2 != null)                // it did, just make sure EDSM sync flag is set..
                            {
                                he.SetEdsmSync();
                            }
                            else
                            {
                                SendTravelLog(edsm, he, mainForm);  // else send it
                            }
                        }
                    }
                }

#if false
                //TBD removed..

                TravelLogUnit tlu = null;

                bool newsystem = false;
                if (_syncFrom)
                {
                    List<SystemNoteClass> notes;
                    int nret = edsm.GetComments(new DateTime(2011, 1, 1), out notes);

                
                    // Check for new systems from EDSM
                    foreach (var system in log)
                    {
                        VisitedSystemsClass ps2 = mainForm?.VisitedSystems == null ? null : 
                            (from c in mainForm.VisitedSystems where c.Name == system.Name && c.Time.Ticks == system.Time.Ticks select c).FirstOrDefault<VisitedSystemsClass>();
                        if (ps2 == null)  // Add to local DB...
                        {
                            if (tlu == null) // If we dontt have a travellogunit yet then create it. 
                            {
                                tlu = new TravelLogUnit();

                                tlu.type = 2;  // EDSM
                                tlu.Path = "https://www.edsm.net/api-logs-v1/get-logs";
                                tlu.Name = "EDSM-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                tlu.Size = 0;

                                tlu.Add();  // Add to Database
                            }

                            VisitedSystemsClass vs = new VisitedSystemsClass();

                            vs.Source = tlu.id;
                            vs.Unit = tlu.Name;

                            vs.Name = system.Name;
                            vs.Time = system.Time;
                            vs.MapColour = _defmapcolour;
                            vs.EDSM_sync = true;
                            vs.Commander = EDDiscoveryForm.EDDConfig.CurrentCommander.Nr;


                            vs.Add();  // Add to DB;
                            System.Diagnostics.Trace.WriteLine("New from EDSM");
                            newsystem = true;

                        }
                    }

                    // Sync comments from EDSM
                    foreach (var note in notes)
                    {
                        SystemNoteClass dbnote = SystemNoteClass.GetSystemNoteClass(note.Name.ToLower());

                        if ( dbnote != null )       // if there..
                        {
                            if (note.Time > dbnote.Time)
                            {
                                dbnote.Time = note.Time;
                                dbnote.Note = note.Note;
                                dbnote.Update();
                            }
                        }
                        else
                        {
                            note.Add();
                        }
                    }
                }
                mainForm.LogLine("EDSM sync Done");

                if (newsystem)
                    OnNewEDSMTravelLog(this);
#endif

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception ex:" + ex.Message);
                mainForm.LogLineHighlight("EDSM sync Exception " + ex.Message);
            }
        }

        internal static bool SendTravelLog(EDSMClass edsm, HistoryEntry he, EDDiscoveryForm mainform)
        {
            if (he.EdsmSync == true)        // no action if true..
                return true;

            string json = null;

            try
            {
                if (!he.System.HasCoordinate)
                    json = edsm.SetLog(he.System.name, he.EventTime);
                else
                    json = edsm.SetLogWithPos(he.System.name, he.EventTime, he.System.x, he.System.y, he.System.z);
            }
            catch (Exception ex )
            {
                if (mainform != null)
                    mainform.LogLine("EDSM sync error, connection to server failed");

                System.Diagnostics.Trace.WriteLine("EDSM Sync error:" + ex.ToString());
            }

            if (json != null)
            {
                JObject msg = (JObject)JObject.Parse(json);

                int msgnum = msg["msgnum"].Value<int>();
                string msgstr = msg["msg"].Value<string>();

                if (msgnum == 100 || msgnum == 401 || msgnum == 402 || msgnum == 403)
                {
                    he.SetEdsmSync();
                    return true;
                }
                else
                {
                    if (mainform != null)
                        mainform.LogLine("EDSM sync ERROR:" + msgnum.ToString() + ":" + msgstr);

                    System.Diagnostics.Trace.WriteLine("Error sync:" + msgnum.ToString() + " : " + he.System.name);
                }
            }

            return false;
        }
    }
}
