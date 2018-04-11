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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EliteDangerousCore.JournalEvents
{
    //When written: after a new ship has been purchased
    //Parameters:
    //•	ShipType
    //•	ShipID
    [JournalEntryType(JournalTypeEnum.ShipyardNew)]
    public class JournalShipyardNew : JournalEntry , IShipInformation
    {
        public JournalShipyardNew(JObject evt ) : base(evt, JournalTypeEnum.ShipyardNew)
        {
            ShipFD = JournalFieldNaming.NormaliseFDShipName(evt["ShipType"].Str());
            ShipType = JournalFieldNaming.GetBetterShipName(ShipFD);
            ShipId = evt["NewShipID"].Int();
        }

        public string ShipType { get; set; }
        public string ShipFD { get; set; }
        public int ShipId { get; set; }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            //System.Diagnostics.Debug.WriteLine(EventTimeUTC + " NEW");
            shp.ShipyardNew(ShipType, ShipFD, ShipId);
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = ShipType;
            detailed = "";
        }
    }
}
