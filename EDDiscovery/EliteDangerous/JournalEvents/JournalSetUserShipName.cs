﻿/*
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
    public class JournalSetUserShipName : JournalEntry , IShipInformation
    {
        public JournalSetUserShipName(JObject evt) : base(evt, JournalTypeEnum.SetUserShipName)
        {
            Ship = JournalFieldNaming.GetBetterShipName(evt["Ship"].Str());
            ShipID = evt["ShipID"].Int();
            ShipName = evt["UserShipName"].Str();// name to match LoadGame
            ShipIdent = evt["UserShipId"].Str();     // name to match LoadGame
        }
        public string Ship { get; set; }
        public int ShipID { get; set; }
        public string ShipName { get; set; }
        public string ShipIdent { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.setusershipname; } }

        public void ShipInformation(ShipInformationList shp, DB.SQLiteConnectionUser conn)
        {
            shp.SetUserShipName(this);
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = Tools.FieldBuilder("",ShipName,"", ShipIdent, "On:" , Ship);
            detailed = "";
        }
    }
}
