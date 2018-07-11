/*
 * Copyright © 2016-2018 EDDiscovery development team
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
    [JournalEntryType(JournalTypeEnum.ShipyardBuy)]
    public class JournalShipyardBuy : JournalEntry, ILedgerJournalEntry, IShipInformation
    {
        public JournalShipyardBuy(JObject evt ) : base(evt, JournalTypeEnum.ShipyardBuy)
        {
            ShipTypeFD = JournalFieldNaming.NormaliseFDShipName(evt["ShipType"].Str());
            ShipType = JournalFieldNaming.GetBetterShipName(ShipTypeFD);
            ShipPrice = evt["ShipPrice"].Long();

            StoreOldShipFD = evt["StoreOldShip"].StrNull();
            if (StoreOldShipFD != null)
            {
                StoreOldShipFD = JournalFieldNaming.NormaliseFDShipName(StoreOldShipFD);
                StoreOldShip = JournalFieldNaming.GetBetterShipName(StoreOldShipFD);
            }

            StoreOldShipId = evt["StoreShipID"].IntNull();

            SellOldShipFD = evt["SellOldShip"].StrNull();
            if (SellOldShipFD != null)
            {
                SellOldShipFD = JournalFieldNaming.NormaliseFDShipName(SellOldShipFD);
                SellOldShip = JournalFieldNaming.GetBetterShipName(SellOldShipFD);
            }

            SellOldShipId = evt["SellShipID"].IntNull();

            SellPrice = evt["SellPrice"].LongNull();

            MarketID = evt["MarketID"].LongNull();
        }

        public string ShipTypeFD { get; set; }
        public string ShipType { get; set; }
        public long ShipPrice { get; set; }

        public string StoreOldShipFD { get; set; }      // may be null
        public string StoreOldShip { get; set; }        // may be null
        public int? StoreOldShipId { get; set; }           // may be null

        public string SellOldShipFD { get; set; }       // may be null         
        public string SellOldShip { get; set; }         // may be null
        public int? SellOldShipId { get; set; }            // may be null

        public long? SellPrice { get; set; }
        public long? MarketID { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, ShipType, -ShipPrice + (SellPrice ?? 0));
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {                                   // new will come along and provide the new ship info
            //System.Diagnostics.Debug.WriteLine(EventTimeUTC + " Buy");
            if (StoreOldShipId != null && StoreOldShipFD!=null)
                shp.Store(StoreOldShipFD, StoreOldShipId.Value, whereami, system.Name);

            if (SellOldShipId != null && SellOldShipFD!=null)
                shp.Sell(SellOldShipFD, SellOldShipId.Value);     
        }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = BaseUtils.FieldBuilder.Build("", ShipType, "Amount:; cr;N0".Txb(this), ShipPrice);
            if (StoreOldShip != null)
                info += ", " + BaseUtils.FieldBuilder.Build("Stored:".Txb(this), StoreOldShip);
            if (SellOldShip != null)
                info += ", " + BaseUtils.FieldBuilder.Build("Sold:".Txb(this), StoreOldShip, "Amount:; cr;N0".Txb(this), SellPrice);
            detailed = "";
        }

    }
}
