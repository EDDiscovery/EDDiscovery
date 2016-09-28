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
        // Close Quarters Combat
        public bool CQC { get; set; }

        // Time and timezone
        public DateTime LastLogTime { get; set; }
        public TimeZoneInfo TimeZone { get; set; }
        public TimeSpan TimeZoneOffset { get; set; }

        // Commander
        //protected EDCommander _commander;
        //public EDCommander Commander { get { return _commander; } set { _commander = value; } }

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
            int cmdrid = -1;

            if (TravelLogUnit.CommanderId.HasValue)
            {
                cmdrid = TravelLogUnit.CommanderId.Value;
                System.Diagnostics.Trace.WriteLine(string.Format("TLU says commander {0} ", cmdrid));
            }

            string line;
            while (this.ReadLine(out line))
            {
                if (line.Length == 0)
                    continue;

                System.Diagnostics.Trace.WriteLine(string.Format("Read line {0} from {1}", line, this.FileName));

                try
                {
                    je = JournalEntry.CreateJournalEntry(line);
                    if (je.EventTypeID == JournalTypeEnum.FileHeader)
                    {
                        JournalEvents.JournalFileHeader header = (JournalEvents.JournalFileHeader)je;

                        if (header.Beta)
                        {
                            TravelLogUnit.type |= 0x8000;
                        }
                    }
                    else if (je.EventTypeID == JournalTypeEnum.LoadGame)
                    {
                        string newname = (je as JournalEvents.JournalLoadGame).LoadGameCommander;

                        if ((TravelLogUnit.type & 0x8000) == 0x8000)
                        {
                            newname = "[BETA] " + newname;
                        }

                        EDCommander _commander = EDDiscovery2.EDDConfig.Instance.listCommanders.FirstOrDefault(c => c.Name.Equals(newname, StringComparison.InvariantCultureIgnoreCase));

                        if (_commander == null)
                            _commander = EDDiscovery2.EDDConfig.Instance.GetNewCommander(newname,null,(cmdrid>=0) ? EDDConfig.Instance.Commander(cmdrid).NetLogDir : null);

                        cmdrid = _commander.Nr;

                        if (!TravelLogUnit.CommanderId.HasValue)
                        {
                            TravelLogUnit.CommanderId = cmdrid;
                            TravelLogUnit.Update();
                            System.Diagnostics.Trace.WriteLine(string.Format("TLU {0} updated with commander {1}", TravelLogUnit.Path, cmdrid));
                        }
                    }

                    je.TLUId = (int)TravelLogUnit.id;
                    je.CommanderId = cmdrid;

                    return true;
                }
                catch (  Exception )          // CreateJournal Entry may except, in which case, the line is crap
                {

                }
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
