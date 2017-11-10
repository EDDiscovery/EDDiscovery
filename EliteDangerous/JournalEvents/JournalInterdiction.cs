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
    //When written: player has (attempted to) interdict another player or npc
    //Parameters:
    //•	Success : true or false
    //•	Interdicted: victim pilot name
    //•	IsPlayer: whether player or npc
    //•	CombatRank: if a player
    //•	Faction: if an npc
    //•	Power: if npc working for power
    [JournalEntryType(JournalTypeEnum.Interdiction)]
    public class JournalInterdiction : JournalEntry
    {
        public JournalInterdiction(JObject evt ) : base(evt, JournalTypeEnum.Interdiction)
        {
            Success = evt["Success"].Bool();
            Interdicted = evt["Interdicted"].Str();
            IsPlayer = evt["IsPlayer"].Bool();
            CombatRank = CombatRank.Harmless;
            if (!evt["CombatRank"].Empty())
                CombatRank = (CombatRank)(evt["CombatRank"].IntNull());
            Faction = evt["Faction"].Str();
            Power = evt["Power"].Str();
        }
        public bool Success { get; set; }
        public string Interdicted { get; set; }
        public bool IsPlayer { get; set; }
        public CombatRank CombatRank { get; set; }
        public string Faction { get; set; }
        public string Power { get; set; }

        public override System.Drawing.Bitmap DefaultIcon { get { return EliteDangerous.Properties.Resources.interdiction; } }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("Failed to interdict;Interdicted", Success, "< ", Interdicted, "< (NPC);(Player)", IsPlayer, "Rank:", CombatRank.ToString().SplitCapsWord(), "Faction:", Faction, "Power:", Power);
            detailed = "";
        }
    }
}
