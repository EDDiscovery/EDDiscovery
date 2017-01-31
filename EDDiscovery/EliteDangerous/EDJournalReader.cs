/*
 * Copyright © 2016 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Fronter Developments plc.
 */
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
                    EDCommander onlyc = EDCommander.GetAll().FirstOrDefault();
                    if (EDCommander.NumberOfCommanders == 1 && onlyc != null && onlyc.Name == "Jameson (Default)")
                    {
                        onlyc.Name = newname;
                        onlyc.EdsmName = newname;
                        EDCommander.Update(new List<EDCommander> { onlyc }, false);
                    }
                    else
                        _commander = EDCommander.Create(newname, null, EDJournalClass.GetDefaultJournalDir().Equals(TravelLogUnit.Path) ? "" : TravelLogUnit.Path);

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
