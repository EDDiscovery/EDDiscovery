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
        public JournalMarketBuy(JObject evt ) : base(evt, JournalTypeEnum.MarketBuy)
        {
            Type = Tools.GetStringDef(evt["Type"]);
            Count = Tools.GetInt(evt["Count"]);
            BuyPrice = Tools.GetInt(evt["BuyPrice"]);
            TotalCost = Tools.GetInt(evt["TotalCost"]);

        }
        public string Type { get; set; }
        public int Count { get; set; }
        public int BuyPrice { get; set; }
        public int TotalCost { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.marketbuy; } }
    }
}
