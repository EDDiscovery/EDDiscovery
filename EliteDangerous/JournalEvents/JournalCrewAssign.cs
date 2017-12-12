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
    //When Written: when changing the task assignment of a member of crew
    //Parameters:
    //•	Name
    //•	Role
    [JournalEntryType(JournalTypeEnum.CrewAssign)]
    public class JournalCrewAssign : JournalEntry
    {
        public JournalCrewAssign(JObject evt) : base(evt, JournalTypeEnum.CrewAssign)
        {
            Name = evt["Name"].Str();
            Role = evt["Role"].Str();
        }
        public string Name { get; set; }
        public string Role { get; set; }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("", Name, "< to role ;", Role);
            detailed = "";
        }
    }
}
