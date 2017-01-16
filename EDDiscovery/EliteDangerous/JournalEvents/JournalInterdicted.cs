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
    //When written: player was interdicted by player or npc
    //Parameters: 
    //•	Submitted: true or false
    //•	Interdictor: interdicting pilot name
    //•	IsPlayer: whether player or npc
    //•	CombatRank: if player
    //•	Faction: if npc
    //•	Power: if npc working for a power
    public class JournalInterdicted : JournalEntry
    {
        public JournalInterdicted(JObject evt ) : base(evt, JournalTypeEnum.Interdicted)
        {
            Submitted = JSONHelper.GetBool(evt["Submitted"]);
            Interdictor = JSONHelper.GetStringDef(evt["Interdictor"]);
            IsPlayer = JSONHelper.GetBool(evt["IsPlayer"]);
            CombatRank = CombatRank.Harmless;
            if (!JSONHelper.IsNullOrEmptyT(evt["CombatRank"]))
                CombatRank = (CombatRank)(JSONHelper.GetIntNull(evt["CombatRank"]));
            Faction = JSONHelper.GetStringDef(evt["Faction"]);
            Power = JSONHelper.GetStringDef(evt["Power"]);
        }
        public bool Submitted { get; set; }
        public string Interdictor { get; set; }
        public bool IsPlayer { get; set; }
        public CombatRank CombatRank { get; set; }
        public string Faction { get; set; }
        public string Power { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.interdicted; } }
    }
}
