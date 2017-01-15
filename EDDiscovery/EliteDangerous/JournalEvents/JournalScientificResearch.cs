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
    //When written: player is awarded a bounty for a kill
    //Parameters: 
    //•	Faction: the faction awarding the bounty
    //•	Reward: the reward value
    //•	VictimFaction: the victim’s faction
    //•	SharedWithOthers: whether shared with other players
    public class JournalScientificResearch : JournalEntry
    {
        public JournalScientificResearch(JObject evt) : base(evt, JournalTypeEnum.ScientificResearch)
        {
            Name = JSONHelper.GetStringDef(evt["Name"]);
            Count = JSONHelper.GetInt(evt["Count"]);
            Category = JSONHelper.GetStringDef(evt["Category"]);
        }
        public string Name { get; set; }
        public int Count { get; set; }
        public string Category { get; set; }

    }
}
