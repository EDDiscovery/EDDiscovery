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
    //When Written: when a mission has failed
    //Parameters:
    //•	Name: name of mission
    [JournalEntryType(JournalTypeEnum.MissionFailed)]
    public class JournalMissionFailed : JournalEntry, IMissions
    {
        public JournalMissionFailed(JObject evt ) : base(evt, JournalTypeEnum.MissionFailed)
        {
            Name = JournalFieldNaming.GetBetterMissionName(evt["Name"].Str());
            MissionId = evt["MissionID"].Int();
            Fine = evt["Fine"].LongNull();
        }

        public string Name { get; set; }
        public int MissionId { get; set; }
        public long? Fine { get; set; }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("", Name,"Fine:",Fine);
            detailed = "";
        }

        public void UpdateMissions(MissionListAccumulator mlist, EliteDangerousCore.ISystem sys, string body, DB.SQLiteConnectionUser conn)
        {
            mlist.Failed(this);
        }

    }
}
