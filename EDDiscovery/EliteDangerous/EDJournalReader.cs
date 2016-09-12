using EDDiscovery2.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous
{
    public class EDJournalReader : LogReaderBase
    {
        // Cached list of previous travel log entries
        protected List<VisitedSystemsClass> systems;

        // Close Quarters Combat
        public bool CQC { get; set; }

        // Time and timezone
        public DateTime LastLogTime { get; set; }
        public TimeZoneInfo TimeZone { get; set; }
        public TimeSpan TimeZoneOffset { get; set; }

        public EDJournalReader(string filename) : base(filename)
        {
            systems = new List<VisitedSystemsClass>();
        }

        public EDJournalReader(TravelLogUnit tlu, List<VisitedSystemsClass> vsclist = null) : base(tlu)
        {
            if (vsclist != null)
            {
                systems = vsclist;
            }
            else
            {
                systems = VisitedSystemsClass.GetAll(tlu);
            }
        }

        public bool ReadJournalLog(out JournalEntry je)
        {
            string line;
            while (this.ReadLine(out line))
            {
                je = JournalEntry.CreateJournalEntry(line);
                je.JournalId = (int)TravelLogUnit.id;
                return true;
            }

            je = null;
            return false;
        }

        public IEnumerable<JournalEntry> ReadJournalLog()
        {
            JournalEntry entry;
            while (ReadJournalLog(out entry))
            {
                yield return entry;
            }
        }
    }
}
