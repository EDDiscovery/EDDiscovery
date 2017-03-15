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

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when switching to another ship already stored at this station
    //Parameters:
    //•	ShipType: type of ship being switched to
    //•	ShipID
    //•	StoreOldShip: (if storing old ship) type of ship being stored
    //•	StoreShipID
    //•	SellOldShip: (if selling old ship) type of ship being sold      -- NO EVIDENCE
    //•	SellShipID -- NO EVIDENCE
    [JournalEntryType(JournalTypeEnum.ShipyardSwap)]
    public class JournalShipyardSwap : JournalEntry, IShipInformation
    {
        public JournalShipyardSwap(JObject evt ) : base(evt, JournalTypeEnum.ShipyardSwap)
        {
            ShipType = JournalFieldNaming.GetBetterShipName(JSONHelper.GetStringDef(evt["ShipType"]));
            ShipId = JSONHelper.GetInt(evt["ShipID"]);
            StoreOldShip = JournalFieldNaming.GetBetterShipName(JSONHelper.GetStringDef(evt["StoreOldShip"]));
            StoreShipId = JSONHelper.GetIntNull(evt["StoreShipID"]);
        }

        public string ShipType { get; set; }
        public int ShipId { get; set; }
        public string StoreOldShip { get; set; }
        public int? StoreShipId { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.shipyardswap; } }
        public void ShipInformation(ShipInformationList shp, DB.SQLiteConnectionUser conn)
        {
            shp.ShipyardSwap(this);
        }

        public override void FillInformation(out string summary, out string info, out string detailed)
        {
            summary = EventTypeStr.SplitCapsWord();
            info = "";// NOT DONE
            detailed = "";
        }
    }
}
