
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: liftoff from a landing pad in a station, outpost or settlement
    //Parameters:
    //•	StationName: name of station

    //•	Security
    public class JournalBuyDrones : JournalEntry
    {
        public JournalBuyDrones(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.BuyDrones, reader)
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
