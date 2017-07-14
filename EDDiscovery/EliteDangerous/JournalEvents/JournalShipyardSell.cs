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

    [JournalEntryType(JournalTypeEnum.ShipyardSell)]
    public class JournalShipyardSell : JournalEntry, ILedgerJournalEntry
    {
        //When Written: when selling a ship stored in the shipyard
        //Parameters:
        //•	ShipType: type of ship being sold
        //•	SellShipID
        //•	ShipPrice: sale price
        //•	System: (if ship is in another system) name of system
        public JournalShipyardSell(JObject evt ) : base(evt, JournalTypeEnum.ShipyardSell)
        {
            ShipType = JournalFieldNaming.GetBetterShipName(evt["ShipType"].Str());
            SellShipId = evt["SellShipID"].Int();
            ShipPrice = evt["ShipPrice"].Long();
            System = evt["System"].Str();
        }
        public string ShipType { get; set; }
        public int SellShipId { get; set; }
        public long ShipPrice { get; set; }
        public string System { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.shipyardsell; } }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, ShipType, ShipPrice);
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("", ShipType, "Amount:; credits", ShipPrice , "At:" , System);
            detailed = "";
        }
    }
}
