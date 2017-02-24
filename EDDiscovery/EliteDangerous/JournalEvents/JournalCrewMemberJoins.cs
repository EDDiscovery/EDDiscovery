/*
 * Copyright © 2017 EDDiscovery development team
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

namespace EDDiscovery.EliteDangerous.JournalEvents
{
//    When written: When another player joins your ship's crew
//Parameters:
//•	Crew: player's commander name

    [JournalEntryType(JournalTypeEnum.CrewMemberJoins)]
    public class JournalCrewMemberJoins : JournalEntry
    {
        public JournalCrewMemberJoins(JObject evt) : base(evt, JournalTypeEnum.CrewMemberJoins)
        {
            Crew = JSONHelper.GetStringDef(evt["Crew"]);

        }
        public string Crew { get; set; }

        //public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.ammunition; } }

    }
}
