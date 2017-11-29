﻿/*
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
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
/*
 When written: When selling a stored ship to raise funds when on insurance/rebuy screen
Parameters:
 ShipType
 System
 SellShipId
 ShipPrice
Example:
{ "timestamp":"2017-07-20T08:56:39Z", "event":"SellShipOnRebuy", "ShipType":"Dolphin",
"System":"Shinrarta Dezhra", "SellShipId":4, "ShipPrice":4110183 } 
 */
    [JournalEntryType(JournalTypeEnum.SellShipOnRebuy)]
    public class JournalSellShipOnRebuy : JournalEntry, ILedgerJournalEntry
    {
        public JournalSellShipOnRebuy(JObject evt) : base(evt, JournalTypeEnum.SellShipOnRebuy)
        {
            ShipType = JournalFieldNaming.GetBetterShipName(evt["ShipType"].Str());
            System = evt["System"].Str();
            SellShipId = evt["SellShipId"].Int();
            ShipPrice = evt["ShipPrice"].Long();

        }
        public string ShipType { get; set; }
        public string System { get; set; }
        public int SellShipId { get; set; }
        public long ShipPrice { get; set; }


        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, ShipType, ShipPrice);
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("Ship:",ShipType, "System:" , System, "Price:" , ShipPrice);
            detailed = "";
        }
    }
}
