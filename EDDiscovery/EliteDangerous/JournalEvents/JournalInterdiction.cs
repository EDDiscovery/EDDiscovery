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
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: player has (attempted to) interdict another player or npc
    //Parameters: 
    //•	Success : true or false
    //•	Interdicted: victim pilot name
    //•	IsPlayer: whether player or npc
    //•	CombatRank: if a player
    //•	Faction: if an npc
    //•	Power: if npc working for power
    public class JournalInterdiction : JournalEntry
    {
        public JournalInterdiction(JObject evt ) : base(evt, JournalTypeEnum.Interdiction)
        {
            Success = JSONHelper.GetBool(evt["Success"]);
            Interdicted = JSONHelper.GetStringDef(evt["Interdicted"]);
            IsPlayer = JSONHelper.GetBool(evt["IsPlayer"]);
            CombatRank = CombatRank.Harmless;
            if (!JSONHelper.IsNullOrEmptyT(evt["CombatRank"]))
                CombatRank = (CombatRank)(JSONHelper.GetIntNull(evt["CombatRank"]));
            Faction = JSONHelper.GetStringDef(evt["Faction"]);
            Power = JSONHelper.GetStringDef(evt["Power"]);
        }
        public bool Success { get; set; }
        public string Interdicted { get; set; }
        public bool IsPlayer { get; set; }
        public CombatRank CombatRank { get; set; }
        public string Faction { get; set; }
        public string Power { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.interdicted; } }
    }
}
