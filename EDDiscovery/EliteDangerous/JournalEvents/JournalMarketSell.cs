
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
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
    public class JournalMarketSell : JournalEntry
    {
        public JournalMarketSell(JObject evt ) : base(evt, JournalTypeEnum.MarketSell)
        {
            Type = JSONHelper.GetStringDef(evt["Type"]);
            Count = JSONHelper.GetInt(evt["Count"]);
            SellPrice = JSONHelper.GetLong(evt["SellPrice"]);
            TotalSale = JSONHelper.GetLong(evt["TotalSale"]);
            AvgPricePaid = JSONHelper.GetLong(evt["AvgPricePaid"]);
            IllegalGoods = JSONHelper.GetBool(evt["IllegalGoods"]);
            StolenGoods = JSONHelper.GetBool(evt["StolenGoods"]);
            BlackMarket = JSONHelper.GetBool(evt["BlackMarket"]);
        }

        public string Type { get; set; }
        public int Count { get; set; }
        public long SellPrice { get; set; }
        public long TotalSale { get; set; }
        public long AvgPricePaid { get; set; }
        public bool IllegalGoods { get; set; }
        public bool StolenGoods { get; set; }
        public bool BlackMarket { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.marketsell; } }

        public void MaterialList(EDDiscovery2.DB.MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            mc.Change(EDDiscovery2.DB.MaterialCommodities.CommodityCategory, Type, -Count, 0, conn);
        }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            EDDiscovery2.DB.MaterialCommodities mc = mcl.GetMaterialCommodity(EDDiscovery2.DB.MaterialCommodities.CommodityCategory, Type, conn);
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, mc.name + " " + Count + " Avg " + AvgPricePaid, TotalSale, (double)(SellPrice - AvgPricePaid));
        }

    }
}
