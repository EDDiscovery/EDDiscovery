﻿using EDDiscovery;
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
    public delegate void EDSMNewSystemEventHandler(object source);


    public class EDSMSync
    {
        Thread ThreadEDSMSync;
        bool running = false;
        bool Exit = false;
        bool _syncTo = false;
        bool _syncFrom = false;
        int _defmapcolour = 0;
        private EDDiscoveryForm mainForm;
        public event EDSMNewSystemEventHandler OnNewEDSMTravelLog;

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
                SQLiteDBClass db = new SQLiteDBClass();
                EDSMClass edsm = new EDSMClass();

                edsm.apiKey =  EDDiscoveryForm.EDDConfig.CurrentCommander.APIKey;
                edsm.commanderName = EDDiscoveryForm.EDDConfig.CurrentCommander.Name;

                //string comments =  edsm.GetComments(new DateTime(2015, 1, 1));
                List<SystemPosition> log;
                List<SystemNoteClass> notes;
                int ret = edsm.GetLogs(new DateTime(2011, 1, 1), out log);
                int nret = edsm.GetComments(new DateTime(2011, 1, 1), out notes);

                if (log == null)
                    log = new List<SystemPosition>();


                if (_syncTo)
                {
                    // Send Unsynced system to EDSM.

                    List<SystemPosition> systems = (from s in mainForm.VisitedSystems where s.vs != null && s.vs.EDSM_sync == false && s.vs.Commander == EDDiscoveryForm.EDDConfig.CurrentCommander.Nr select s).ToList<SystemPosition>();
                    mainForm.LogLine("EDSM: Sending " + systems.Count.ToString() + " flightlog entries");
                    foreach (var system in systems)
                    {
                        if (Exit)
                        {
                            running = false;
                            return;
                        }

                        if (system.vs != null && system.vs.EDSM_sync == false)
                        {
                            // check if it exist in EDSM
                            SystemPosition ps2 = (from c in log where c.Name == system.Name && c.time.Ticks == system.time.Ticks select c).FirstOrDefault<SystemPosition>();
                            if (ps2 != null)
                            {
                                system.vs.EDSM_sync = true;
                                system.Update();

                            }
                            else
                            {
                                SendTravelLog(edsm, system, mainForm);

                            }
                        }
                    }
                }

                TravelLogUnit tlu = null;

                bool newsystem = false;
                if (_syncFrom)
                {
                    // Check for new systems from EDSM
                    foreach (var system in log)
                    {
                        SystemPosition ps2 = (from c in mainForm.VisitedSystems where c.Name == system.Name && c.time.Ticks == system.time.Ticks select c).FirstOrDefault<SystemPosition>();
                        if (ps2 == null)  // Add to local DB...
                        {
                            if (tlu == null) // If we dontt have a travellogunit yet then create it. 
                            {
                                tlu = new TravelLogUnit();

                                tlu.type = 2;  // EDSM
                                tlu.Path = "http://www.edsm.net/api-logs-v1/get-logs";
                                tlu.Name = "EDSM-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                tlu.Size = 0;

                                tlu.Add();  // Add to Database
                            }

                            VisitedSystemsClass vs = new VisitedSystemsClass();

                            vs.Source = tlu.id;
                            vs.Unit = tlu.Name;

                            vs.Name = system.Name;
                            vs.Time = system.time;
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
                        string searchname = note.Name.ToLower();
                        if (SQLiteDBClass.globalSystemNotes.ContainsKey(searchname))
                        {
                            SystemNoteClass dbnote = SQLiteDBClass.globalSystemNotes[searchname];
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
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception ex:" + ex.Message);
                mainForm.LogLineHighlight("EDSM sync Exception " );
            }

        }

        internal static bool SendTravelLog(EDSMClass edsm, SystemPosition system, EDDiscoveryForm mainform)
        {
            string json;

            if (system.vs.X==0.0 && system.vs.Y==0.0 && system.vs.Z==0.0)
                json = edsm.SetLog(system.Name, system.time);
            else
                json = edsm.SetLogWithPos(system.Name, system.time, system.vs.X, system.vs.Y, system.vs.Z);

            if (json != null)
            {
                JObject msg = (JObject)JObject.Parse(json);

                int msgnum = msg["msgnum"].Value<int>();
                string msgstr = msg["msg"].Value<string>();


                if (msgnum == 100 || msgnum == 401 || msgnum == 402 || msgnum == 403)
                {
                    if (msgnum == 100)
                        System.Diagnostics.Trace.WriteLine("New");

                    system.vs.EDSM_sync = true;
                    system.Update();
                    return true;
                }
                else
                {
                    if (mainform!=null)
                        mainform.LogLine("EDSM sync ERROR:" + msgnum.ToString() + ":" + msgstr);

                    System.Diagnostics.Trace.WriteLine("Error sync:" + msgnum.ToString() + " : " + system.Name);
                    return false; 
                }

            }
            else
                return false;


        }
    }
}
