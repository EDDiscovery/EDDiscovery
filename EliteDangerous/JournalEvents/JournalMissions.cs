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
    //    When written: when starting game
    //Parameters:
    //•	Active: Array of records
    //o   id
    //o   Name
    //o   PassengerMission
    //o   Expires

    [JournalEntryType(JournalTypeEnum.Missions)]
    public class JournalMissions : JournalEntry
    {
        public JournalMissions(JObject evt) : base(evt, JournalTypeEnum.Missions)
        {
            ActiveMissions = evt["Active"]?.ToObject<MissionItem[]>();
            FailedMissions = evt["Failed"]?.ToObject<MissionItem[]>();
            CompletedMissions = evt["Completed"]?.ToObject<MissionItem[]>();
        }

        public string StationName { get; set; }
        public string StarSystem { get; set; }
        public long MarketID { get; set; }

        public MissionItem[] ActiveMissions { get; set; }
        public MissionItem[] FailedMissions { get; set; }
        public MissionItem[] CompletedMissions { get; set; }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = "";
            detailed = "";
        }
    }


    public class MissionItem
    {
        public long id;
        public string Name;
        public bool PassengerMission;
        public int Expires;
    }

}
