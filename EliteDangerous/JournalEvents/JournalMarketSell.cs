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
    //When Written: when selling goods in the market
    //Parameters:
    //•	Type: cargo type
    //•	Count: number of units
    //•	SellPrice: price per unit
    //•	TotalSale: total sale value
    //•	AvgPricePaid: average price paid
    //•	IllegalGoods: (not always present) whether goods are illegal here
    //•	StolenGoods: (not always present) whether goods were stolen
    //•	BlackMarket: (not always present) whether selling in a black market
    [JournalEntryType(JournalTypeEnum.MarketSell)]
    public class JournalMarketSell : JournalEntry, IMaterialCommodityJournalEntry, ILedgerJournalEntry
    {
        public JournalMarketSell(JObject evt ) : base(evt, JournalTypeEnum.MarketSell)
        {
            Type = evt["Type"].Str();                           // FDNAME
            Type = JournalFieldNaming.FDNameTranslation(Type);     // pre-mangle to latest names, in case we are reading old journal records
            FriendlyType = JournalFieldNaming.RMat(Type);
            Count = evt["Count"].Int();
            SellPrice = evt["SellPrice"].Long();
            TotalSale = evt["TotalSale"].Long();
            AvgPricePaid = evt["AvgPricePaid"].Long();
            IllegalGoods = evt["IllegalGoods"].Bool();
            StolenGoods = evt["StolenGoods"].Bool();
            BlackMarket = evt["BlackMarket"].Bool();
        }

        public string Type { get; set; }
        public string FriendlyType { get; set; }
        public int Count { get; set; }
        public long SellPrice { get; set; }
        public long TotalSale { get; set; }
        public long AvgPricePaid { get; set; }
        public bool IllegalGoods { get; set; }
        public bool StolenGoods { get; set; }
        public bool BlackMarket { get; set; }

        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            mc.Change(MaterialCommodities.CommodityCategory, Type, -Count, 0, conn);
        }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, FriendlyType + " " + Count + " Avg " + AvgPricePaid, TotalSale, (double)(SellPrice - AvgPricePaid));
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            long profit = TotalSale - (AvgPricePaid * Count);
            info = BaseUtils.FieldBuilder.Build("", FriendlyType, "", Count, "< at ; cr;N0", SellPrice, "Total:", TotalSale, "Profit:", profit);
            detailed = BaseUtils.FieldBuilder.Build("Legal;Illegal", IllegalGoods, "Not Stolen;Stolen", StolenGoods, "Market;BlackMarket", BlackMarket);
        }
    }
}
