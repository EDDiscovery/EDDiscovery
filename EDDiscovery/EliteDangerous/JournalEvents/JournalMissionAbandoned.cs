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
    //When Written: when a mission has been abandoned
    //Parameters:
    //•	Name: name of mission
    public class JournalMissionAbandoned : JournalEntry
    {
        public JournalMissionAbandoned(JObject evt ) : base(evt, JournalTypeEnum.MissionAbandoned)
        {
            Name = JournalEntry.GetBetterMissionName(JSONHelper.GetStringDef(evt["Name"]));
            MissionId = JSONHelper.GetInt(evt["MissionID"]);
        }
        public string Name { get; set; }
        public int MissionId { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";MissionID";
        }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.missionabandoned; } }

    }
}
