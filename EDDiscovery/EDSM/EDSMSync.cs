using EDDiscovery;
using EDDiscovery.DB;
using EDDiscovery2.DB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EDDiscovery2.EDSM
{
    public class EDSMSync
    {
        Thread ThreadEDSMSync;
        bool Exit = false;
        private EDDiscoveryForm mainForm;

        public EDSMSync(EDDiscoveryForm frm)
        {
            mainForm = frm;
        }

        public bool StartSync()
        {
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
        }


        public void Sync()
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
            foreach (var system in mainForm.visitedSystems)
            {
                string json = null;

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
                }
            }


        }


    }
}
