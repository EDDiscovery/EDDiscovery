using EDDiscovery;
using EDDiscovery.DB;
using EDDiscovery.EDDN;
using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;
using EDDiscovery2.DB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EDDiscovery2.EDSM
{
    public static class EDDNSync
    {
        private static Thread ThreadEDDNSync;
        private static int _running = 0;
        private static bool Exit = false;
        private static ConcurrentQueue<HistoryEntry> hlscanunsyncedlist = new ConcurrentQueue<HistoryEntry>();
        private static AutoResetEvent hlscanevent = new AutoResetEvent(false);
        private static EDDiscoveryForm mainForm;

        public static bool SendEDDNEvent(EDDiscoveryForm frm, HistoryEntry helist)
        {
            return SendEDDNEvents(frm, new[] { helist });
        }

        public static bool SendEDDNEvents(EDDiscoveryForm frm, params HistoryEntry[] helist)
        {
            return SendEDDNEvents(frm, (IEnumerable<HistoryEntry>)helist);
        }

        public static bool SendEDDNEvents(EDDiscoveryForm frm, IEnumerable<HistoryEntry> helist)
        {
            foreach (HistoryEntry he in helist)
            {
                hlscanunsyncedlist.Enqueue(he);
            }

            hlscanevent.Set();

            // Start the sync thread if it's not already running
            if (Interlocked.CompareExchange(ref _running, 1, 0) == 0)
            {
                Exit = false;
                mainForm = frm;
                ThreadEDDNSync = new System.Threading.Thread(new System.Threading.ThreadStart(SyncThread));
                ThreadEDDNSync.Name = "EDDN Sync";
                ThreadEDDNSync.IsBackground = true;
                ThreadEDDNSync.Start();
            }

            return true;
        }

        public static void StopSync()
        {
            Exit = true;
            hlscanevent.Set();
        }

        private static void SyncThread()
        {
            try
            {
                _running = 1;
                mainForm.LogLine("Starting EDDN sync thread");

                while (hlscanunsyncedlist.Count != 0)
                {
                    List<HistoryEntry> hl = new List<HistoryEntry>();
                    HistoryEntry he = null;

                    while (hlscanunsyncedlist.TryDequeue(out he))
                    {
                        hlscanevent.Reset();

                        if (EDDNSync.SendToEDDN(he))
                        {
                            mainForm.LogLine($"Sent {he.EntryType.ToString()} event to EDDN ({he.EventSummary})");
                        }

                        if (Exit)
                        {
                            return;
                        }

                        Thread.Sleep(1000);   // Throttling to 1 per second to not kill EDDN network
                    }

                    // Wait up to 60 seconds for another EDDN event to come in
                    hlscanevent.WaitOne(60000);
                    if (Exit)
                    {
                        return;
                    }
                }
                
                mainForm.LogLine("EDDN sync thread exiting");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception ex:" + ex.Message);
                mainForm.LogLineHighlight("EDDN sync Exception " + ex.Message);
            }
            finally
            {
                _running = 0;
            }
        }

        static public bool SendToEDDN(HistoryEntry he)
        {
            EDDNClass eddn = new EDDNClass();

            if (he.Commander != null)
            {
                eddn.commanderName = he.Commander.EdsmName;
            }

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
                {
                    he.SetEddnSync();
                    return true;
                }
            }

            return false;
        }

    }
}
