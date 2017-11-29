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
    //When Written: when purchasing goods in the market
    //Parameters:
    //•	Type: cargo type
    //•	Count: number of units
    //•	BuyPrice: cost per unit
    //•	TotalCost: total cost
    [JournalEntryType(JournalTypeEnum.MarketBuy)]
    public class JournalMarketBuy : JournalEntry, IMaterialCommodityJournalEntry, ILedgerJournalEntry
    {
        public JournalMarketBuy(JObject evt ) : base(evt, JournalTypeEnum.MarketBuy)
        {
            Type = evt["Type"].Str();        // must be FD name
            Type = JournalFieldNaming.FDNameTranslation(Type);     // pre-mangle to latest names, in case we are reading old journal records
            FriendlyType = JournalFieldNaming.RMat(Type);
            Count = evt["Count"].Int();
            BuyPrice = evt["BuyPrice"].Long();
            TotalCost = evt["TotalCost"].Long();
        }

        public string Type { get; set; }        // FDNAME
        public string FriendlyType { get; set; }            // translated name
        public int Count { get; set; }
        public long BuyPrice { get; set; }
        public long TotalCost { get; set; }


        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            mc.Change(MaterialCommodities.CommodityCategory, Type, Count, BuyPrice, conn);
        }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, FriendlyType + " " + Count,-TotalCost);
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("", FriendlyType, "", Count, "< at ; credits", BuyPrice, "Total:", TotalCost);
            detailed = "";
        }
    }
}
