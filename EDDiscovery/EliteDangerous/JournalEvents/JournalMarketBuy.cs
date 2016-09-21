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
            Type = evt.Value<string>("Type");
            Count = evt.Value<int>("Count");
            BuyPrice = evt.Value<int>("BuyPrice");
            TotalCost = evt.Value<int>("TotalCost");

        }
        public string Type { get; set; }
        public int Count { get; set; }
        public int BuyPrice { get; set; }
        public int TotalCost { get; set; }

    }
}
