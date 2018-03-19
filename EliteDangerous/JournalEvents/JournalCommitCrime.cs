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
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
    //When written: when a crime is recorded against the player
    //Parameters:
    //•	CrimeType
    //•	Faction
    //Optional parameters (depending on crime)
    //•	Victim
    //•	Fine
    //•	Bounty
    [JournalEntryType(JournalTypeEnum.CommitCrime)]
    public class JournalCommitCrime : JournalEntry, ILedgerNoCashJournalEntry
    {
        public JournalCommitCrime(JObject evt ) : base(evt, JournalTypeEnum.CommitCrime)
        {
            CrimeType = evt["CrimeType"].Str().SplitCapsWordFull();
            Faction = evt["Faction"].Str();
            Victim = evt["Victim"].Str();
            VictimLocalised = evt["Victim_Localised"].Str().Alt(Victim);
            Fine = evt["Fine"].LongNull();
            Bounty = evt["Bounty"].LongNull();
        }
        public string CrimeType { get; set; }
        public string Faction { get; set; }
        public string Victim { get; set; }
        public string VictimLocalised { get; set; }
        public long? Fine { get; set; }
        public long? Bounty { get; set; }
        public long Cost { get { return (Fine.HasValue ? Fine.Value : 0) + (Bounty.HasValue ? Bounty.Value : 0); } }

        public void LedgerNC(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            string v = (VictimLocalised.Length > 0) ? VictimLocalised : Victim;

            if (v.Length == 0)
                v = Faction;

            if (Fine.HasValue)
                v += " Fine " + Fine.Value.ToString("N0");

            if (Bounty.HasValue)
                v += " Bounty " + Bounty.Value.ToString("N0");

            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, CrimeType + " on " + v);
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("", CrimeType, "< on faction ", Faction, "Against ", VictimLocalised, "Cost ; cr;N0", Fine, "Bounty ; cr;N0", Bounty);
            detailed = "";
        }
    }
}
