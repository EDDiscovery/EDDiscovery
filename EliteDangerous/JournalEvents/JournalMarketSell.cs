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
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.MarketSell)]
    public class JournalMarketSell : JournalEntry, IMaterialCommodityJournalEntry, ILedgerJournalEntry
    {
        public JournalMarketSell(JObject evt ) : base(evt, JournalTypeEnum.MarketSell)
        {
            MarketID = evt["MarketID"].LongNull();
            Type = evt["Type"].Str();                           // FDNAME
            Type = JournalFieldNaming.FDNameTranslation(Type);     // pre-mangle to latest names, in case we are reading old journal records
            FriendlyType = JournalFieldNaming.RMat(Type);
            Type_Localised = JournalFieldNaming.CheckLocalisation(evt["Type_Localised"].Str(),FriendlyType);         // always ensure we have one
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
        public string Type_Localised { get; set; }      // always set

        public int Count { get; set; }
        public long SellPrice { get; set; }
        public long TotalSale { get; set; }
        public long AvgPricePaid { get; set; }
        public bool IllegalGoods { get; set; }
        public bool StolenGoods { get; set; }
        public bool BlackMarket { get; set; }
        public long? MarketID { get; set; }

        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            mc.Change(MaterialCommodityData.CommodityCategory, Type, -Count, 0, conn);
        }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, FriendlyType + " " + Count + " Avg " + AvgPricePaid, TotalSale, (double)(SellPrice - AvgPricePaid));
        }

        public override void FillInformation(out string info, out string detailed) 
        {
            long profit = TotalSale - (AvgPricePaid * Count);
            info = BaseUtils.FieldBuilder.Build("", Type_Localised, "", Count, "< sell price ; cr;N0".Txb(this), SellPrice, "Total Cost:; cr;N0".Txb(this), TotalSale, "Profit:; cr;N0".Txb(this), profit);
            detailed = BaseUtils.FieldBuilder.Build("Legal;Illegal".Txb(this), IllegalGoods, "Not Stolen;Stolen".Txb(this), StolenGoods, "Market;BlackMarket".Txb(this), BlackMarket);
        }
    }
}
