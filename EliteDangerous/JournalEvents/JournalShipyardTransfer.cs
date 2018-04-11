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
    /*
    When Written: when requesting a ship at another station be transported to this station
    Parameters:
     ShipType: type of ship
     ShipID
     System: where it is
     Distance: how far away
     TransferPrice: cost of transfer
     TransferTime: time taken in seconds
    Example:
    { "timestamp":"2016-07-21T15:19:49Z", "event":"ShipyardTransfer", "ShipType":"SideWinder",
    "ShipID":7, "System":"Eranin", "Distance":85.639145, "TransferPrice":580 }
     * */
    [JournalEntryType(JournalTypeEnum.ShipyardTransfer)]
    public class JournalShipyardTransfer : JournalEntry, ILedgerJournalEntry, IShipInformation
    {
        public JournalShipyardTransfer(JObject evt ) : base(evt, JournalTypeEnum.ShipyardTransfer)
        {
            ShipTypeFD = JournalFieldNaming.NormaliseFDShipName(evt["ShipType"].Str());
            ShipType = JournalFieldNaming.GetBetterShipName(ShipTypeFD);
            ShipId = evt["ShipID"].Int();

            FromSystem = evt["System"].Str();
            Distance = evt["Distance"].Double();
            TransferPrice = evt["TransferPrice"].Long();

            if (Distance > 100000.0)       // previously, it was in m, now they have changed it to LY per 2.3. So if its large (over 100k ly, impossible) convert
                Distance = Distance / 299792458.0 / 365 / 24 / 60 / 60;

            nTransferTime = evt["TransferTime"].IntNull();
            FriendlyTransferTime = nTransferTime.HasValue ? nTransferTime.Value.SecondsToString() : "";

            MarketID = evt["MarketID"].LongNull();
            ShipMarketID = evt["ShipMarketID"].LongNull();
        }

        public string ShipTypeFD { get; set; }
        public string ShipType { get; set; }
        public int ShipId { get; set; }
        public string FromSystem { get; set; }
        public double Distance { get; set; }
        public long TransferPrice { get; set; }
        public int? nTransferTime { get; set; }
        public string FriendlyTransferTime { get; set; }
        public long? MarketID { get; set; }
        public long? ShipMarketID { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, ShipType, -TransferPrice);
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            DateTime arrival = EventTimeUTC.AddSeconds(nTransferTime ?? 0);
            //System.Diagnostics.Debug.WriteLine(EventTimeUTC + " Transfer");
            shp.Transfer(ShipType, ShipTypeFD, ShipId, FromSystem, system.Name, whereami, arrival);
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("Of ", ShipType, "< from " , FromSystem , "Distance:; ly;0.0" , Distance , "Price:; cr;N0", TransferPrice, "TransferTime:", FriendlyTransferTime);
            detailed = "";
        }
    }
}
