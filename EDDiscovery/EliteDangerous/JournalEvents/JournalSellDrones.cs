
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //    When Written: when selling unwanted drones back to the market
    //Parameters:
    //•	Type
    //•	Count
    //•	SellPrice
    //•	TotalSale

    public class JournalSellDrones : JournalEntry
    {
        public JournalSellDrones(JObject evt) : base(evt, JournalTypeEnum.SellDrones)
        {
            Type = Tools.GetStringDef(evt["Type"]);
            Count = Tools.GetInt(evt["Count"]);
            SellPrice = Tools.GetInt(evt["SellPrice"]);
            TotalSale = Tools.GetInt(evt["TotalSale"]);
        }
        public string Type { get; set; }
        public int Count { get; set; }
        public int SellPrice { get; set; }
        public int TotalSale { get; set; }
    }
}
