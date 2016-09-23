
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
        public JournalMarketSell(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.MarketSell, reader)
        {
            Type = Tools.GetStringDef("Type");
            Count = Tools.GetInt("Count");
            SellPrice = Tools.GetInt("SellPrice");
            TotalSale = Tools.GetInt("TotalSale");
            AvgPricePaid = Tools.GetInt("AvgPricePaid");
            IllegalGoods = Tools.GetBool("IllegalGoods");
            StolenGoods = Tools.GetBool("StolenGoods");
            BlackMarket = Tools.GetBool("BlackMarket");
        }
        public string Type { get; set; }
        public int Count { get; set; }
        public int SellPrice { get; set; }
        public int TotalSale { get; set; }
        public int AvgPricePaid { get; set; }
        public bool IllegalGoods { get; set; }
        public bool StolenGoods { get; set; }
        public bool BlackMarket { get; set; }
    }
}
