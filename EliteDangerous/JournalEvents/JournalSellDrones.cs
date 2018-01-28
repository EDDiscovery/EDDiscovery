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

namespace EliteDangerousCore.JournalEvents
{
    //    When Written: when selling unwanted drones back to the market
    //Parameters:
    //•	Type
    //•	Count
    //•	SellPrice
    //•	TotalSale
    [JournalEntryType(JournalTypeEnum.SellDrones)]
    public class JournalSellDrones : JournalEntry, ILedgerJournalEntry
    {
        public JournalSellDrones(JObject evt) : base(evt, JournalTypeEnum.SellDrones)
        {
            Type = evt["Type"].Str();
            Count = evt["Count"].Int();
            SellPrice = evt["SellPrice"].Long();
            TotalSale = evt["TotalSale"].Long();
        }
        public string Type { get; set; }
        public int Count { get; set; }
        public long SellPrice { get; set; }
        public long TotalSale { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Count.ToString() + " drones", TotalSale);
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("",Type, "Count:" , Count , "Each:; credits" , SellPrice, "Amount:; credits" , TotalSale);
            detailed = "";
        }
    }
}
