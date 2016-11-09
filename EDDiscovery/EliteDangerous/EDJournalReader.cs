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

        public static bool disable_beta_commander_check = false;        // strictly for debugging purposes

        public EDJournalReader(string filename) : base(filename)
        {
        }

        public EDJournalReader(TravelLogUnit tlu) : base(tlu)
        {
        }

        // Journal ID
        public int JournalId { get { return (int)TravelLogUnit.id; } }

        protected JournalEntry ProcessLine(string line, bool resetOnError)
        {
            int cmdrid = -2;        //-1 is hidden, -2 is never shown

            if (TravelLogUnit.CommanderId.HasValue)
            {
                cmdrid = TravelLogUnit.CommanderId.Value;
                // System.Diagnostics.Trace.WriteLine(string.Format("TLU says commander {0} at {1}", cmdrid, TravelLogUnit.Name));
            }

            if (line.Length == 0)
                return null;

            JournalEntry je = null;

            try
            {
                je = JournalEntry.CreateJournalEntry(line);
            }
            catch
            {
                System.Diagnostics.Trace.WriteLine($"Bad journal line:\n{line}");

                if (resetOnError)
                {
                    throw;
                }
                else
                {
                    return null;
                }
            }

            if (je.EventTypeID == JournalTypeEnum.Fileheader)
            {
                JournalEvents.JournalFileheader header = (JournalEvents.JournalFileheader)je;

                if (header.Beta && !disable_beta_commander_check)
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

                EDCommander _commander = EDDiscovery2.EDDConfig.Instance.ListOfCommanders.FirstOrDefault(c => c.Name.Equals(newname, StringComparison.InvariantCultureIgnoreCase));

                if (_commander == null )
                {
                    if (EDDiscovery2.EDDConfig.Instance.ListOfCommanders.Count == 1 && EDDiscovery2.EDDConfig.Instance.ListOfCommanders[0].Name == "Jameson (Default)" )
                    {
                        EDDiscovery2.EDDConfig.Instance.ListOfCommanders[0].Name = newname;
                        EDDiscovery2.EDDConfig.Instance.ListOfCommanders[0].EdsmName = newname;
                        EDDiscovery2.EDDConfig.Instance.UpdateCommanders(EDDiscovery2.EDDConfig.Instance.ListOfCommanders); // replaces it
                    }
                    else
                        _commander = EDDiscovery2.EDDConfig.Instance.GetNewCommander(newname, null, EDJournalClass.GetDefaultJournalDir().Equals(TravelLogUnit.Path) ? "" : TravelLogUnit.Path);

                }

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

            return je;
        }

        public bool ReadJournalLog(out JournalEntry jent, bool resetOnError = false)
        {
            while (ReadLine(out jent, l => ProcessLine(l, resetOnError)))
            {
                if (jent == null)
                    continue;

                //System.Diagnostics.Trace.WriteLine(string.Format("Read line {0} from {1}", line, this.FileName));

                return true;
            }

            jent = null;
            return false;
        }

        public IEnumerable<JournalEntry> ReadJournalLog()
        {
            JournalEntry entry;
            bool resetOnError = false;
            while (ReadJournalLog(out entry, resetOnError: resetOnError))
            {
                yield return entry;
                resetOnError = true;
            }
        }
    }
}
