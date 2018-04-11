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
    public class JournalShipyardBuy : JournalEntry, ILedgerJournalEntry, IShipInformation
    {
        public JournalShipyardBuy(JObject evt ) : base(evt, JournalTypeEnum.ShipyardBuy)
        {
            ShipTypeFD = JournalFieldNaming.NormaliseFDShipName(evt["ShipType"].Str());
            ShipType = JournalFieldNaming.GetBetterShipName(ShipTypeFD);
            ShipPrice = evt["ShipPrice"].Long();

            StoreOldShipFD = JournalFieldNaming.NormaliseFDShipName(evt["StoreOldShip"].Str());
            StoreOldShip = JournalFieldNaming.GetBetterShipName(StoreOldShipFD);
            StoreShipId = evt["StoreShipID"].IntNull();

            SellOldShipFD = JournalFieldNaming.NormaliseFDShipName(evt["SellOldShip"].Str());
            SellOldShip = JournalFieldNaming.GetBetterShipName(SellOldShipFD);
            SellShipId = evt["SellShipID"].IntNull();
            SellPrice = evt["SellPrice"].LongNull();

            MarketID = evt["MarketID"].LongNull();
        }

        public string ShipTypeFD { get; set; }
        public string ShipType { get; set; }
        public long ShipPrice { get; set; }

        public string StoreOldShipFD { get; set; }
        public string StoreOldShip { get; set; }
        public int? StoreShipId { get; set; }

        public string SellOldShipFD { get; set; }
        public string SellOldShip { get; set; }
        public int? SellShipId { get; set; }

        public long? SellPrice { get; set; }
        public long? MarketID { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, ShipType, -ShipPrice + (SellPrice ?? 0));
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {                                   // new will come along and provide the new ship info

            //System.Diagnostics.Debug.WriteLine(EventTimeUTC + " Buy");
            if (StoreShipId != null)
                shp.Store(StoreOldShipFD, StoreShipId.Value, whereami, system.Name);     
            if (SellShipId != null)
                shp.Sell(SellOldShipFD, SellShipId.Value);     
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("", ShipType, "Amount:; cr;N0", ShipPrice);
            if (StoreOldShip.Length > 0)
                info += ", " + BaseUtils.FieldBuilder.Build("Stored:", StoreOldShip);
            if (SellOldShip.Length > 0)
                info += ", " + BaseUtils.FieldBuilder.Build("Sold:", StoreOldShip, "Amount:; cr;N0", SellPrice);
            detailed = "";
        }

    }
}
