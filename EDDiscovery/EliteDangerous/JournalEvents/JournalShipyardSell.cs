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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{

  public class JournalShipyardSell : JournalEntry
    {
        //When Written: when selling a ship stored in the shipyard
        //Parameters:
        //•	ShipType: type of ship being sold
        //•	SellShipID
        //•	ShipPrice: sale price
        //•	System: (if ship is in another system) name of system
        public JournalShipyardSell(JObject evt ) : base(evt, JournalTypeEnum.ShipyardSell)
        {
            ShipType = JSONHelper.GetStringDef(evt["ShipType"]);
            SellShipId = JSONHelper.GetInt(evt["SellShipID"]);
            ShipPrice = JSONHelper.GetLong(evt["ShipPrice"]);
            System = JSONHelper.GetStringDef(evt["System"]);
        }
        public string ShipType { get; set; }
        public int SellShipId { get; set; }
        public long ShipPrice { get; set; }
        public string System { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";SellShipID";
        }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.shipyardsell; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, ShipType, ShipPrice);
        }

    }
}
