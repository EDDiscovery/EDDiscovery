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
    //When Written: when engaging a new member of crew
    //Parameters:
    //•	Name
    //•	Faction
    //•	Cost
    //•	CombatRank
    [JournalEntryType(JournalTypeEnum.CrewHire)]
    public class JournalCrewHire : JournalEntry, ILedgerJournalEntry
    {
        public JournalCrewHire(JObject evt) : base(evt, JournalTypeEnum.CrewHire)
        {
            Name = evt["Name"].Str();
            Faction = evt["Faction"].Str();
            Cost = evt["Cost"].Long();
            CombatRank = (CombatRank)evt["CombatRank"].Int();
        }
        public string Name { get; set; }
        public string Faction { get; set; }
        public long Cost { get; set; }
        public CombatRank CombatRank { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Name + " " + Faction, -Cost);
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("Hired ;", Name, "< of faction ", Faction, " Rank ", CombatRank.ToString().SplitCapsWord(), "Cost ; credits", Cost);
            detailed = "";
        }
    }
}
