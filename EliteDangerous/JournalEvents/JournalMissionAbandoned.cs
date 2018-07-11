/*
 * Copyright © 2016-2018 EDDiscovery development team
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
    [JournalEntryType(JournalTypeEnum.MissionAbandoned)]
    public class JournalMissionAbandoned : JournalEntry, IMissions
    {
        public JournalMissionAbandoned(JObject evt ) : base(evt, JournalTypeEnum.MissionAbandoned)
        {
            FDName = evt["Name"].Str();
            Name = JournalFieldNaming.GetBetterMissionName(FDName);
            MissionId = evt["MissionID"].Int();
            Fine = evt["Fine"].LongNull();
        }

        public string Name { get; set; }
        public string FDName { get; set; }
        public int MissionId { get; set; }
        public long? Fine { get; set; }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = BaseUtils.FieldBuilder.Build("", Name, "Fine:".Txb(this), Fine);
            detailed = "";
        }

        public void UpdateMissions(MissionListAccumulator mlist, EliteDangerousCore.ISystem sys, string body , DB.SQLiteConnectionUser conn)
        {
            mlist.Abandoned(this);
        }

    }
}
