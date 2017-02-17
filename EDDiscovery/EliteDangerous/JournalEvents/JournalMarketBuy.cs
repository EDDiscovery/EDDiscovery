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
    //When Written: when purchasing goods in the market
    //Parameters:
    //•	Type: cargo type
    //•	Count: number of units
    //•	BuyPrice: cost per unit
    //•	TotalCost: total cost
    public class JournalMarketBuy : JournalEntry, IMaterialCommodityJournalEntry, ILedgerJournalEntry
    {
        public JournalMarketBuy(JObject evt ) : base(evt, JournalTypeEnum.MarketBuy)
        {
            Type = JSONHelper.GetStringDef(evt["Type"]);
            Count = JSONHelper.GetInt(evt["Count"]);
            BuyPrice = JSONHelper.GetLong(evt["BuyPrice"]);
            TotalCost = JSONHelper.GetLong(evt["TotalCost"]);

        }
        public string Type { get; set; }
        public int Count { get; set; }
        public long BuyPrice { get; set; }
        public long TotalCost { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.marketbuy; } }

        public void MaterialList(EDDiscovery2.DB.MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            mc.Change(EDDiscovery2.DB.MaterialCommodities.CommodityCategory, Type, Count, BuyPrice, conn);
        }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            EDDiscovery2.DB.MaterialCommodities mc = mcl.GetMaterialCommodity(EDDiscovery2.DB.MaterialCommodities.CommodityCategory, Type, conn);
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, mc.name + " " + Count,-TotalCost);
        }

    }
}
