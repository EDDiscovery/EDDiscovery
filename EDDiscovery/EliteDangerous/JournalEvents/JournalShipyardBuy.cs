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
    //When Written: when buying a new ship in the shipyard
    //Parameters:
    //•	ShipType: ship being purchased
    //•	ShipPrice: purchase cost
    //•	StoreOldShip: (if storing old ship) ship type being stored
    //•	StoreShipID
    //•	SellOldShip: (if selling current ship) ship type being sold
    //•	SellShipID
    //•	SellPrice: (if selling current ship) ship sale price
    //
    //Note: the new ship’s ShipID will be logged in a separate event after the purchase
    [JournalEntryType(JournalTypeEnum.ShipyardBuy)]
    public class JournalShipyardBuy : JournalEntry, ILedgerJournalEntry
    {
        public JournalShipyardBuy(JObject evt ) : base(evt, JournalTypeEnum.ShipyardBuy)
        {
            ShipType = JournalFieldNaming.GetBetterShipName(JSONHelper.GetStringDef(evt["ShipType"]));
            ShipPrice = JSONHelper.GetLong(evt["ShipPrice"]);
            StoreOldShip = JournalFieldNaming.GetBetterShipName(JSONHelper.GetStringDef(evt["StoreOldShip"]));
            StoreShipId = JSONHelper.GetIntNull(evt["StoreShipID"]);
            SellOldShip = JournalFieldNaming.GetBetterShipName(JSONHelper.GetStringDef(evt["SellOldShip"]));
            SellShipId = JSONHelper.GetIntNull(evt["SellShipID"]);
            SellPrice = JSONHelper.GetLongNull(evt["SellPrice"]);

        }
        public string ShipType { get; set; }
        public long ShipPrice { get; set; }
        public string StoreOldShip { get; set; }
        public int? StoreShipId { get; set; }
        public string SellOldShip { get; set; }
        public int? SellShipId { get; set; }
        public long? SellPrice { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.shipyardnew; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, ShipType, -ShipPrice + (SellPrice ?? 0));
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = Tools.FieldBuilder("", ShipType, "Amount:; credits", ShipPrice);
            if (StoreOldShip.Length > 0)
                info += ", " + Tools.FieldBuilder("Stored:", StoreOldShip);
            if (SellOldShip.Length > 0)
                info += ", " + Tools.FieldBuilder("Sold:", StoreOldShip, "Amount:; credits", SellPrice);
            detailed = "";
        }

    }
}
