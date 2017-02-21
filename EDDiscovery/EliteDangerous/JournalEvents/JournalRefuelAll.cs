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
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: liftoff from a landing pad in a station, outpost or settlement
    //Parameters:
    //•	StationName: name of station

    //•	Security
    [JournalEntryType(JournalTypeEnum.RefuelAll)]
    public class JournalRefuelAll : JournalEntry, ILedgerJournalEntry
    {
        public JournalRefuelAll(JObject evt ) : base(evt, JournalTypeEnum.RefuelAll)
        {
            Cost = JSONHelper.GetLong(evt["Cost"]);
            Amount = JSONHelper.GetInt(evt["Amount"]);
        }
        public long Cost { get; set; }
        public int Amount { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.refuelall; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, "Amount " + Amount.ToString() + "t", -Cost);
        }

    }
}
