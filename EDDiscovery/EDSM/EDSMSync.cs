using EDDiscovery;
using EDDiscovery.DB;
using EDDiscovery2.DB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        private EDDiscoveryForm mainForm;
        public event EDSMNewSystemEventHandler OnNewEDSMTravelLog;

        public EDSMSync(EDDiscoveryForm frm)
        {
            mainForm = frm;
        }

        public bool StartSync()
        {
            if (running) // Only start once.
                return false;

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

                edsm.apiKey = db.GetSettingString("EDSMApiKey", "");
                edsm.commanderName = db.GetSettingString("CommanderName", "");

                //string comments =  edsm.GetComments(new DateTime(2015, 1, 1));
                List<SystemPosition> log;
                int ret = edsm.GetLogs(new DateTime(2011, 1, 1), out log);

                if (log == null)
                    log = new List<SystemPosition>();

                // Send Unsynced system to EDSM.

                List<SystemPosition> systems = (from s in mainForm.visitedSystems where s.vs !=null && s.vs.EDSM_sync == false select s).ToList<SystemPosition>();
                mainForm.LogLine("EDSM: Sending" +  systems.Count.ToString() + " flightlog entries", Color.Black);
                foreach (var system in systems)
                {
                    string json = null;

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
                            system.vs.Update();

                        }
                        else
                            json = edsm.SetLog(system.Name, system.time);

                        if (json != null)
                        {
                            JObject msg = (JObject)JObject.Parse(json);

                            int msgnum = msg["msgnum"].Value<int>();

                            if (msgnum == 100 || msgnum == 401 || msgnum == 402 || msgnum == 403)
                            {
                                if (msgnum == 100)
                                    System.Diagnostics.Trace.WriteLine("New");

                                system.vs.EDSM_sync = true;
                                system.vs.Update();
                            }
                            else
                            {
                                System.Diagnostics.Trace.WriteLine("Error sync:" + msgnum.ToString() + " : " + system.Name);
                            }


                        }
                    }
                }

                TravelLogUnit tlu = null;

                // Check for new systems from EDSM
                bool newsystem = false;
                foreach (var system in log)
                {
                    SystemPosition ps2 = (from c in mainForm.visitedSystems where c.Name == system.Name && c.time.Ticks == system.time.Ticks select c).FirstOrDefault<SystemPosition>();
                    if (ps2 == null)  // Add to local DB...
                    {
                        if (tlu == null) // If we dontt have a travellogunit yet then create it. 
                        {
                            tlu = new TravelLogUnit();

                            tlu.type = 2;  // EDSM
                            tlu.Path = "http://www.edsm.net/api-logs-v1/get-logs";
                            tlu.Name = "EDSM-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            tlu.Size = 0;

                            tlu.Add();  // Add to Database
                        }

                        VisitedSystemsClass vs = new VisitedSystemsClass();

                        vs.Source = tlu.id;
                        vs.Unit = tlu.Name;

                        vs.Name = system.Name;
                        vs.Time = system.time;
                        vs.EDSM_sync = true;

                        vs.Add();  // Add to DB;
                        System.Diagnostics.Trace.WriteLine("New from EDSM");
                        newsystem = true;
                        
                    }
                }
                mainForm.LogLine("EDSM sync Done", Color.Black);

                if (newsystem)
                    OnNewEDSMTravelLog(this);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception ex:" + ex.Message);
                mainForm.LogLine("EDSM sync Exception " + ex.Message, Color.Red);
            }

        }


    }
}
