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

namespace EliteDangerousCore.JournalEvents
{
//    When written: when the captain in multicrew disbands the crew
//Parameters:
// OnCrime: (bool) true if crew disbanded as a result of a crime in a lawful session
    // { "timestamp":"2017-04-12T11:32:30Z", "event":"EndCrewSession", "OnCrime":false } 
    [JournalEntryType(JournalTypeEnum.EndCrewSession)]
    public class JournalEndCrewSession : JournalEntry
    {
        public JournalEndCrewSession(JObject evt) : base(evt, JournalTypeEnum.EndCrewSession)
        {
            OnCrime = evt["OnCrime"].Bool();

        }
        public bool OnCrime { get; set; }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("; Crime", OnCrime);
            detailed = "";
        }
    }
}
