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
    //When written: when a mission is updated with a new destination
    //Parameters
    // MissionID
    // MissionName
    // NewDestinationStation
    // OldDestinationStation
    // NewDestinationSystem

//{ "timestamp": "2017-08-01T09:04:07Z", "event": "MissionRedirected", "MissionID": 65367315, "NewDestinationStation": "Metcalf Orbital", "OldDestinationStation": "Cuffey Orbital", "NewDestinationSystem": "Cemiess", "OldDestinationSystem": "Vequess" }


[JournalEntryType(JournalTypeEnum.MissionRedirected)]
    public class JournalMissionRedirected : JournalEntry, IMissions
    {
        public JournalMissionRedirected(JObject evt ) : base(evt, JournalTypeEnum.MissionRedirected)
        {
            Name = JournalFieldNaming.GetBetterMissionName(evt["MissionName"].Str());
            MissionId = evt["MissionID"].Int();
            NewDestinationStation = evt["NewDestinationStation"].Str();
            OldDestinationStation = evt["OldDestinationStation"].Str();
            NewDestinationSystem = evt["NewDestinationSystem"].Str();
            OldDestinationSystem = evt["OldDestinationSystem"].Str();
        }

        public string NewDestinationStation { get; set; }
        public string OldDestinationStation { get; set; }
        public string NewDestinationSystem { get; set; }
        public string OldDestinationSystem { get; set; }

        public int MissionId { get; set; }
        public string Name { get; set; }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = info = BaseUtils.FieldBuilder.Build("Mission name:", Name,
                                      "From:", OldDestinationSystem,
                                      "", OldDestinationStation,
                                      "To:", NewDestinationSystem,
                                      "", NewDestinationStation);

            detailed = "";
        }

        public void UpdateMissions(MissionListAccumulator mlist, EliteDangerousCore.ISystem sys, string body, DB.SQLiteConnectionUser conn)
        {
            mlist.Redirected(this);
        }

    }
}
