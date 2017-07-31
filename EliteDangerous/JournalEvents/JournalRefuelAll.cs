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
    //When written: liftoff from a landing pad in a station, outpost or settlement
    //Parameters:
    //•	StationName: name of station

    //•	Security
    [JournalEntryType(JournalTypeEnum.RefuelAll)]
    public class JournalRefuelAll : JournalEntry, ILedgerJournalEntry, IShipInformation
    {
        public JournalRefuelAll(JObject evt ) : base(evt, JournalTypeEnum.RefuelAll)
        {
            Cost = evt["Cost"].Long();
            Amount = evt["Amount"].Double();
        }

        public long Cost { get; set; }
        public double Amount { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EliteDangerous.Properties.Resources.refuelall; } }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, "Amount " + Amount.ToString() + "t", -Cost);
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("Cost:; credits", Cost, "Fuel:; tons;0.0", Amount);
            detailed = "";
        }

        public void ShipInformation(ShipInformationList shp, DB.SQLiteConnectionUser conn)
        {
            shp.RefuelAll(this);
        }
    }
}
