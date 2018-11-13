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
using System.IO;

namespace EliteDangerousCore
{
    public class JournalReaderEntry
    {
        public JournalEntry JournalEntry;
        public JObject Json;
    }

    public class EDJournalReader : LogReaderBase
    {
        JournalEvents.JournalShipyard lastshipyard = null;
        JournalEvents.JournalStoredShips laststoredships = null;
        JournalEvents.JournalStoredModules laststoredmodules = null;
        JournalEvents.JournalOutfitting lastoutfitting = null;
        JournalEvents.JournalMarket lastmarket = null;
        const int timelimit = 5 * 60;   //seconds.. 5 mins between logs. Note if we undock, we reset the counters.

        private Queue<JournalReaderEntry> StartEntries = new Queue<JournalReaderEntry>();

        public EDJournalReader(string filename) : base(filename)
        {
        }

        public EDJournalReader(TravelLogUnit tlu) : base(tlu)
        {
        }

        // inhistoryrefreshparse = means reading history in batch mode
        private JournalReaderEntry ProcessLine(string line, bool inhistoryrefreshparse, bool resetOnError)
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

            bool toosoon = false;

            if (je.EventTypeID == JournalTypeEnum.Fileheader)
            {
                JournalEvents.JournalFileheader header = (JournalEvents.JournalFileheader)je;

                if ((header.Beta && !EliteConfigInstance.InstanceOptions.DisableBetaCommanderCheck) || EliteConfigInstance.InstanceOptions.ForceBetaOnCommander) // if beta, and not disabled, or force beta
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

                EDCommander commander = EDCommander.GetCommander(newname);

                if (commander == null )
                {
                    commander = EDCommander.GetListCommanders().FirstOrDefault();
                    if (EDCommander.NumberOfCommanders == 1 && commander != null && commander.Name == "Jameson (Default)")
                    {
                        commander.Name = newname;
                        commander.EdsmName = newname;
                        EDCommander.Update(new List<EDCommander> { commander }, false);
                    }
                    else
                        commander = EDCommander.Create(newname, null, EDJournalClass.GetDefaultJournalDir().Equals(TravelLogUnit.Path) ? "" : TravelLogUnit.Path);

                }

                cmdrid = commander.Nr;

                if (!TravelLogUnit.CommanderId.HasValue)
                {
                    TravelLogUnit.CommanderId = cmdrid;
                    TravelLogUnit.Update();
                    System.Diagnostics.Trace.WriteLine(string.Format("TLU {0} updated with commander {1}", TravelLogUnit.Path, cmdrid));
                }
            }
            else if (je is ISystemStationEntry && ((ISystemStationEntry)je).IsTrainingEvent)
            {
                System.Diagnostics.Trace.WriteLine($"Training detected:\n{line}");
                return null;
            }

            if (je is IAdditionalFiles)
            {
                if ((je as IAdditionalFiles).ReadAdditionalFiles(Path.GetDirectoryName(FileName), inhistoryrefreshparse, ref jo) == false)     // if failed
                    return null;
            }

            if (je is JournalEvents.JournalShipyard)                // when going into shipyard
            {
                toosoon = lastshipyard != null && lastshipyard.Yard.Equals((je as JournalEvents.JournalShipyard).Yard);
                lastshipyard = je as JournalEvents.JournalShipyard;
            }
            else if (je is JournalEvents.JournalStoredShips)        // when going into shipyard
            {
                toosoon = laststoredships != null && CollectionStaticHelpers.Equals(laststoredships.ShipsHere, (je as JournalEvents.JournalStoredShips).ShipsHere) &&
                    CollectionStaticHelpers.Equals(laststoredships.ShipsRemote, (je as JournalEvents.JournalStoredShips).ShipsRemote);
                laststoredships = je as JournalEvents.JournalStoredShips;
            }
            else if (je is JournalEvents.JournalStoredModules)      // when going into outfitting
            {
                toosoon = laststoredmodules != null && CollectionStaticHelpers.Equals(laststoredmodules.ModuleItems, (je as JournalEvents.JournalStoredModules).ModuleItems);
                laststoredmodules = je as JournalEvents.JournalStoredModules;
            }
            else if (je is JournalEvents.JournalOutfitting)         // when doing into outfitting
            {
                toosoon = lastoutfitting != null && lastoutfitting.ItemList.Equals((je as JournalEvents.JournalOutfitting).ItemList);
                lastoutfitting = je as JournalEvents.JournalOutfitting;
            }
            else if (je is JournalEvents.JournalMarket)
            {
                toosoon = lastmarket != null && lastmarket.Equals(je as JournalEvents.JournalMarket);
                lastmarket = je as JournalEvents.JournalMarket;
            }
            else if (je is JournalEvents.JournalUndocked || je is JournalEvents.JournalLoadGame)             // undocked, Load Game, repeats are cleared
            {
                lastshipyard = null;
                laststoredmodules = null;
                lastoutfitting = null;
                laststoredmodules = null;
                laststoredships = null;
            }

            if (toosoon)                                                // if seeing repeats, remove
            {
                System.Diagnostics.Debug.WriteLine("**** Remove as dup " + je.EventTypeStr);
                return null;
            }

            je.SetTLUCommander(TravelLogUnit.id, cmdrid);

            return new JournalReaderEntry { JournalEntry = je, Json = jo };
        }

        // function needs to report two things, list of JREs (may be empty) and if it read something, bool.. hence form changed
        // bool reporting we have performed any sort of action is important.. it causes the TLU pos to be updated above even if we have junked all the events or delayed them

        public bool ReadJournal(out List<JournalReaderEntry> jent, bool historyrefreshparsing, bool resetOnError )      // True if anything was processed, even if we rejected it
        {
            jent = new List<JournalReaderEntry>();

            bool readanything = false;

            while (ReadLine(out JournalReaderEntry newentry, l => ProcessLine(l, historyrefreshparsing, resetOnError)))
            {
                readanything = true;

                if (newentry != null)                           // if we got a record back, we may not because it may not be valid or be rejected..
                {
                    // if we don't have a commander yet, we need to queue it until we have one, since every entry needs a commander

                    if ((this.TravelLogUnit.CommanderId == null || this.TravelLogUnit.CommanderId < 0) && newentry.JournalEntry.EventTypeID != JournalTypeEnum.LoadGame)
                    {
                        //System.Diagnostics.Debug.WriteLine("*** Delay " + newentry.JournalEntry.EventTypeStr);
                        StartEntries.Enqueue(newentry);         // queue..
                    }
                    else
                    {
                        while (StartEntries.Count != 0)     // we have a commander, anything queued up, play that in first.
                        {
                            var dentry = StartEntries.Dequeue();
                            dentry.JournalEntry.SetCommander(TravelLogUnit.CommanderId.Value);
                            //System.Diagnostics.Debug.WriteLine("*** UnDelay " + dentry.JournalEntry.EventTypeStr);
                            jent.Add(dentry);
                        }

                        //System.Diagnostics.Debug.WriteLine("*** Send  " + newentry.JournalEntry.EventTypeStr);
                        jent.Add(newentry);                     // and store in new entry
                    }
                }
            }

            return readanything;
        }
    }
}
