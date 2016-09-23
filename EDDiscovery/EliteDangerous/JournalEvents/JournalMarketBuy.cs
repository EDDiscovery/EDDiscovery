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
    public class JournalMarketBuy : JournalEntry
    {
        public JournalMarketBuy(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.MarketBuy, reader)
        {
            Type = Tools.GetStringDef("Type");
            Count = Tools.GetInt("Count");
            BuyPrice = Tools.GetInt("BuyPrice");
            TotalCost = Tools.GetInt("TotalCost");

        }
        public string Type { get; set; }
        public int Count { get; set; }
        public int BuyPrice { get; set; }
        public int TotalCost { get; set; }

    }
}
