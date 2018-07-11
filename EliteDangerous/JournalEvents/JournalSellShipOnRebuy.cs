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
using System.Diagnostics;
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.SellShipOnRebuy)]
    public class JournalSellShipOnRebuy : JournalEntry, ILedgerJournalEntry, IShipInformation
    {
        public JournalSellShipOnRebuy(JObject evt) : base(evt, JournalTypeEnum.SellShipOnRebuy)
        {
            ShipTypeFD = JournalFieldNaming.NormaliseFDShipName(evt["ShipType"].Str());
            ShipType = JournalFieldNaming.GetBetterShipName(ShipTypeFD);
            System = evt["System"].Str();
            SellShipId = evt["SellShipId"].Int();
            ShipPrice = evt["ShipPrice"].Long();
        }

        public string ShipTypeFD { get; set; }
        public string ShipType { get; set; }
        public string System { get; set; }
        public int SellShipId { get; set; }
        public long ShipPrice { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, ShipType, ShipPrice);
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.Sell(ShipType, SellShipId);
        }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = BaseUtils.FieldBuilder.Build("Ship:".Txb(this), ShipType, "System:".Txb(this), System, "Price:; cr;N0".Txb(this), ShipPrice);
            detailed = "";
        }
    }
}
