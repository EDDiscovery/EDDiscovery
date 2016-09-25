using EDDiscovery2;
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
        protected EDCommander _commander;

        // Close Quarters Combat
        public bool CQC { get; set; }

        // Time and timezone
        public DateTime LastLogTime { get; set; }
        public TimeZoneInfo TimeZone { get; set; }
        public TimeSpan TimeZoneOffset { get; set; }

        // Commander
        public EDCommander Commander { get { return _commander; } set { _commander = value; } }

        // Journal ID
        public int JournalId { get { return (int)TravelLogUnit.id; } }

        public EDJournalReader(string filename) : base(filename)
        {
        }

        public EDJournalReader(TravelLogUnit tlu) : base(tlu)
        {
        }

        public bool ReadJournalLog(out JournalEntry je)
        {
            int cmdrid = (_commander!= null)  ? _commander.Nr : -1;

            string line;
            while (this.ReadLine(out line))
            {
                je = JournalEntry.CreateJournalEntry(line);
                if ( je.EventTypeID == JournalTypeEnum.LoadGame )
                {
                    string newname = (je as JournalEvents.JournalLoadGame).LoadGameCommander;

                    _commander = EDDiscovery2.EDDConfig.Instance.listCommanders.FirstOrDefault(c => c.Name.Equals(newname, StringComparison.InvariantCultureIgnoreCase));

                    if (_commander == null)
                        _commander= EDDiscovery2.EDDConfig.Instance.GetNewCommander(newname);

                    cmdrid = _commander.Nr;
                }

                je.JournalId = (int)TravelLogUnit.id;
                je.CommanderId = cmdrid;

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
