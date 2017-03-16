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
    //When Written: when requesting a ship at another station be transported to this station
    //Parameters:
    //•	ShipType: type of ship
    //•	ShipID
    //•	System: where it is
    //•	Distance: how far away
    //•	TransferPrice: cost of transfer
    [JournalEntryType(JournalTypeEnum.ShipyardTransfer)]
    public class JournalShipyardTransfer : JournalEntry, ILedgerJournalEntry
    {
        public JournalShipyardTransfer(JObject evt ) : base(evt, JournalTypeEnum.ShipyardTransfer)
        {
            ShipType = JournalFieldNaming.GetBetterShipName(evt["ShipType"].Str());
            ShipId = evt["ShipID"].Int();
            System = evt["System"].Str();
            Distance = evt["Distance"].Double();
            TransferPrice = evt["TransferPrice"].Long();

            if (Distance > 100000.0)       // previously, it was in m, now they have changed it to LY per 2.3. So if its large (over 100k ly, impossible) convert
                Distance = Distance / 299792458.0 / 365 / 24 / 60 / 60;
        }

        public string ShipType { get; set; }
        public int ShipId { get; set; }
        public string System { get; set; }
        public double Distance { get; set; }
        public long TransferPrice { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.shipyardtransfer; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, ShipType, -TransferPrice);
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = Tools.FieldBuilder("Of ", ShipType, "<from " , System , "Distance:; ly;0.0" , Distance , "Price:; credits", TransferPrice);
            detailed = "";
        }
    }
}
