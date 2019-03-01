/*
 * Copyright © 2016-2018 EDDiscovery development team
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
    [JournalEntryType(JournalTypeEnum.NpcCrewPaidWage)]
    public class JournalNpcCrewPaidWage : JournalEntry, ILedgerJournalEntry
    {
        public JournalNpcCrewPaidWage(JObject evt ) : base(evt, JournalTypeEnum.NpcCrewPaidWage)
        {
            NpcCrewID = evt["NpcCrewId"].Long();
            Name = evt["NpcCrewName"].Str().Alt("Unknown");
            Amount = evt["Amount"].Long();
        }

        public long NpcCrewID { get; set; }
        public string Name { get; set; }
        public long Amount { get; set; }

        public override void FillInformation(out string info, out string detailed) 
        {           
            info = BaseUtils.FieldBuilder.Build("<", Name , "; Cr;N0" , Amount);
            detailed = "";
        }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, "Wages for ".Txb(this) + Name, -Amount);
        }
    }

    [JournalEntryType(JournalTypeEnum.NpcCrewRank)]
    public class JournalNpcCrewRank : JournalEntry
    {
        public JournalNpcCrewRank(JObject evt) : base(evt, JournalTypeEnum.NpcCrewRank)
        {
            NpcCrewID = evt["NpcCrewId"].Long();
            RankCombat = (CombatRank)evt["RankCombat"].Int();
            Name = evt["NpcCrewName"].Str().Alt("Unknown");
        }

        public long NpcCrewID { get; set; }
        public string Name { get; set; }
        public CombatRank RankCombat { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("<", Name, "Rank:".Txb(this), RankCombat.ToString().SplitCapsWord());
            detailed = "";
        }

    }

}
