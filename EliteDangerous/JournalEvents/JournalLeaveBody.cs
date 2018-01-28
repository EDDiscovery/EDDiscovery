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
    //When written: when leaving an astronomical body
    //Parameters:
    // •	StarSystem
    // •	SystemAddress
    // •	Body
    // •	BodyID

    [JournalEntryType(JournalTypeEnum.LeaveBody)]
    public class JournalLeaveBody : JournalEntry
    {
        public JournalLeaveBody(JObject evt) : base(evt, JournalTypeEnum.LeaveBody)
        {
            StarSystem = evt["StarSystem"].Str();
            SystemAddress = evt["SystemAddress"].Long();
            Body = evt["Body"].Str();
            BodyID = evt["BodyID"].Int();
        }

        public string StarSystem { get; set; }
        public long SystemAddress { get; set; }
        public string Body { get; set; }
        public int BodyID { get; set; }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = Body;
            detailed = "";
        }

    }
}
