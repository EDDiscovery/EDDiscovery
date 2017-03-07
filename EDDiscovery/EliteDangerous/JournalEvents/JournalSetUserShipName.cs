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
//    When written: when assigning a name to the ship in Starport Services
//Parameters:
//•	Ship: Ship model(eg CobraMkIII)
//•	ShipID: player's ship ID number
//•	UserShipName: selected name
//•	UserShipId: selected ship id

//Example:
//{ "timestamp":"2017-01-24T10:42:38Z", "event":"SetUserShipName", "Ship":"cobramkiii", "ShipID":2, "UserShipName":"Enterprise", "UserShipId":"NCC 1701" }


[JournalEntryType(JournalTypeEnum.SetUserShipName)]
    public class JournalSetUserShipName : JournalEntry
    {
        public JournalSetUserShipName(JObject evt) : base(evt, JournalTypeEnum.SetUserShipName)
        {
            Ship = JSONHelper.GetStringDef(evt["Ship"]);
            ShipID = JSONHelper.GetInt(evt["ShipID"]);
            UserShipName = JSONHelper.GetStringDef(evt["UserShipName"]);
            UserShipId = JSONHelper.GetStringDef(evt["UserShipId"]);
        }
        public string Ship { get; set; }
        public int ShipID { get; set; }
        public string UserShipName { get; set; }
        public string UserShipId { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.setusershipname; } }

    }
}
