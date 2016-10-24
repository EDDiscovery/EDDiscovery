using EDDiscovery;
using EDDiscovery.DB;
using EDDiscovery.EDDN;
using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;
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
    public class EDDNSync
    {
        Thread ThreadEDDNSync;
        bool running = false;
        bool Exit = false;
        bool _syncTo = false;
        bool _syncFrom = false;
        int _defmapcolour = 0;
        private EDDiscoveryForm mainForm;

   
        public EDDNSync(EDDiscoveryForm frm)
        {
            mainForm = frm;
        }

        public bool StartSync(EDDNClass eddn, bool syncto, bool syncfrom, int defmapcolour)
        {
            if (running) // Only start once.
                return false;
            _syncTo = syncto;
            _syncFrom = syncfrom;
            _defmapcolour = defmapcolour;

            ThreadEDDNSync = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(SyncThread));
            ThreadEDDNSync.Name = "EDDN Sync";
            ThreadEDDNSync.IsBackground = true;
            ThreadEDDNSync.Start(eddn);

            return true;
        }

        public void StopSync()
        {
            Exit = true;
        }

        private void SyncThread(object _eddn)
        {
            EDDNClass eddn = (EDDNClass)_eddn;
            running = true;
            Sync(eddn);
            running = false;
        }

        private void Sync(EDDNClass edsm)
        {
            try
            {
                mainForm.LogLine("EDDN sync begin");

                List<HistoryEntry> hlfsdunsyncedlist = mainForm.history.FilterByNotEDDNSynced;        // first entry is oldest

                if (_syncTo && hlfsdunsyncedlist.Count > 0)                   // send systems to edsm (verified with dates, 29/9/2016, utc throughout)
                {
                    DateTime utcmin = hlfsdunsyncedlist[0].EventTimeUTC.AddDays(-1);        // 1 days for margin ;-)  only get them back to this date for speed..

                    mainForm.LogLine("EDDN: Sending " + hlfsdunsyncedlist.Count.ToString() + " events");
                    int edsmsystemssent = 0;

                    foreach (var he in hlfsdunsyncedlist)
                    {
                        if (Exit)
                        {
                            running = false;
                            return;
                        }

                        {
                            string errmsg;              // (verified with EDSM 29/9/2016)
                            if (EDDNSync.SendEDDNEvent(he))
                                edsmsystemssent++;

                        }
                    }

                    mainForm.LogLine(string.Format("EDDN Events sent {0}", edsmsystemssent));
                }

                
                mainForm.LogLine("EDDN sync Done");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception ex:" + ex.Message);
                mainForm.LogLineHighlight("EDDN sync Exception " + ex.Message);
            }
        }

        public static bool SendEDDNEvent(HistoryEntry he) // (verified with EDSM 29/9/2016, seen UTC time being sent, and same UTC time on ESDM).
        {

            string errmsg;
            Task taskEDSM = Task.Factory.StartNew(() =>
            {                                                   // LOCAL time, there is a UTC converter inside this call
                if (SendToEDDN(he))
                    he.SetEdsmSync();
            });

            return true;
        }


        static public bool SendToEDDN(HistoryEntry he)
        {
            EDDNClass eddn = new EDDNClass();

            JournalEntry je = JournalEntry.Get(he.Journalid);
            JObject msg = null;

            if (je.EventTypeID == JournalTypeEnum.FSDJump)
            {
                msg = eddn.CreateEDDNMessage(je as JournalFSDJump);

            }
            else if (je.EventTypeID == JournalTypeEnum.Docked)
            {
                msg = eddn.CreateEDDNMessage(je as JournalDocked, he.System.x, he.System.y, he.System.z);
            }
            else if (je.EventTypeID == JournalTypeEnum.Scan)
            {
                msg = eddn.CreateEDDNMessage(je as JournalScan, he.System.name, he.System.x, he.System.y, he.System.z);
            }

            if (msg != null)
            {
                if (eddn.PostMessage(msg))
                    return true;
            }

            return false;
        }

    }
}
