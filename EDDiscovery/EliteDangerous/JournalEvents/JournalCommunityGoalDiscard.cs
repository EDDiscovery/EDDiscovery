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

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //Parameters:
    //•	Name
    //•	System
    [JournalEntryType(JournalTypeEnum.CommunityGoalDiscard)]
    public class JournalCommunityGoalDiscard : JournalEntry
    {
        public JournalCommunityGoalDiscard(JObject evt) : base(evt, JournalTypeEnum.CommunityGoalDiscard)
        {
            Name = evt["Name"].Str();
            System = evt["System"].Str();
        }

        public string Name { get; set; }
        public string System { get; set; }
        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.communitygoaldiscard; } }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("", Name, "< at ; Star System", System);
            detailed = "";
        }
    }
}
