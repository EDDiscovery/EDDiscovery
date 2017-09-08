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
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */

using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace EliteDangerousCore
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

        private Queue<JournalReaderEntry> StartEntries = new Queue<JournalReaderEntry>();

        public EDJournalReader(string filename) : base(filename)
        {
        }

        public EDJournalReader(TravelLogUnit tlu) : base(tlu)
        {
        }

        // Journal ID
        public int JournalId { get { return (int)TravelLogUnit.id; } }

        protected JournalReaderEntry ProcessLine(string line, bool resetOnError)
        {
            int cmdrid = -2;        //-1 is hidden, -2 is never shown

            if (TravelLogUnit.CommanderId.HasValue)
            {
                cmdrid = TravelLogUnit.CommanderId.Value;
                // System.Diagnostics.Trace.WriteLine(string.Format("TLU says commander {0} at {1}", cmdrid, TravelLogUnit.Name));
            }

            if (line.Length == 0)
                return null;

            JObject jo = null;
            JournalEntry je = null;

            try
            {
                jo = JObject.Parse(line);
                je = JournalEntry.CreateJournalEntry(jo);
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

            if (je == null)
            {
                System.Diagnostics.Trace.WriteLine($"Bad journal line:\n{line}");
                return null;
            }

            if (je.EventTypeID == JournalTypeEnum.Fileheader)
            {
                JournalEvents.JournalFileheader header = (JournalEvents.JournalFileheader)je;

                if (header.Beta && !disable_beta_commander_check)
                {
                    TravelLogUnit.type |= 0x8000;
                }

                if (header.Part > 1)
                {
                    JournalEvents.JournalContinued contd = JournalEntry.GetLast<JournalEvents.JournalContinued>(je.EventTimeUTC.AddSeconds(1), e => e.Part == header.Part);

                    // Carry commander over from previous log if it ends with a Continued event.
                    if (contd != null && Math.Abs(header.EventTimeUTC.Subtract(contd.EventTimeUTC).TotalSeconds) < 5 && contd.CommanderId >= 0)
                    {
                        TravelLogUnit.CommanderId = contd.CommanderId;
                    }
                }
            }
            else if (je.EventTypeID == JournalTypeEnum.LoadGame)
            {
                string newname = (je as JournalEvents.JournalLoadGame).LoadGameCommander;

                if ((TravelLogUnit.type & 0x8000) == 0x8000)
                {
                    newname = "[BETA] " + newname;
                }

                EDCommander _commander = EDCommander.GetCommander(newname);

                if (_commander == null )
                {
                    _commander = EDCommander.GetAll().FirstOrDefault();
                    if (EDCommander.NumberOfCommanders == 1 && _commander != null && _commander.Name == "Jameson (Default)")
                    {
                        _commander.Name = newname;
                        _commander.EdsmName = newname;
                        EDCommander.Update(new List<EDCommander> { _commander }, false);
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

            return new JournalReaderEntry { JournalEntry = je, Json = jo };
        }

        public bool ReadJournalLog(out JournalReaderEntry jent, bool resetOnError = false)
        {
            if (StartEntries.Count != 0 && this.TravelLogUnit.CommanderId != null && this.TravelLogUnit.CommanderId >= 0)
            {
                jent = StartEntries.Dequeue();
                jent.JournalEntry.CommanderId = (int)TravelLogUnit.CommanderId;
                return true;
            }

            while (ReadLine(out jent, l => ProcessLine(l, resetOnError)))
            {
                if (jent == null)
                    continue;

                if ((this.TravelLogUnit.CommanderId == null || this.TravelLogUnit.CommanderId < 0) && jent.JournalEntry.EventTypeID != JournalTypeEnum.LoadGame)
                {
                    StartEntries.Enqueue(jent);
                    continue;
                }

                //System.Diagnostics.Trace.WriteLine(string.Format("Read line {0} from {1}", line, this.FileName));

                return true;
            }

            jent = null;
            return false;
        }

        public IEnumerable<JournalReaderEntry> ReadJournalLog(bool continueOnError = false)
        {
            JournalReaderEntry entry;
            bool resetOnError = false;
            while (ReadJournalLog(out entry, resetOnError: resetOnError))
            {
                yield return entry;
                resetOnError = !continueOnError;
            }
        }
    }
}
