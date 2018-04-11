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
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace EliteDangerousCore.JournalEvents
{

    [JournalEntryType(JournalTypeEnum.ShipyardSell)]
    public class JournalShipyardSell : JournalEntry, ILedgerJournalEntry, IShipInformation
    {
        //When Written: when selling a ship stored in the shipyard
        //Parameters:
        //•	ShipType: type of ship being sold
        //•	SellShipID
        //•	ShipPrice: sale price
        //•	System: (if ship is in another system) name of system
        public JournalShipyardSell(JObject evt ) : base(evt, JournalTypeEnum.ShipyardSell)
        {
            MarketID = evt["MarketID"].LongNull();
            ShipTypeFD = JournalFieldNaming.NormaliseFDShipName(evt["ShipType"].Str());
            ShipType = JournalFieldNaming.GetBetterShipName(ShipTypeFD);
            SellShipId = evt["SellShipID"].Int();
            ShipPrice = evt["ShipPrice"].Long();
            System = evt["System"].Str();
        }

        public string ShipTypeFD { get; set; }
        public string ShipType { get; set; }
        public int SellShipId { get; set; }
        public long ShipPrice { get; set; }
        public string System { get; set; }      // may be empty
        public long? MarketID { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, ShipType, ShipPrice);
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            //Debug.WriteLine(EventTimeUTC + " SELL");
            shp.Sell(ShipType, SellShipId);
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("", ShipType, "Amount:; cr;N0", ShipPrice , "At:" , System);
            detailed = "";
        }
    }
}
