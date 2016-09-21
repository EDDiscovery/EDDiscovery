using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: liftoff from a landing pad in a station, outpost or settlement
    //Parameters:
    //•	StationName: name of station

    //•	Security
    public class JournalBuyTradeData : JournalEntry
    {
        public JournalBuyTradeData(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.BuyTradeData, reader)
        {
            System = evt.Value<string>("System");
            Cost = evt.Value<int>("Cost");
        }

        public string System { get; set; }
        public int Cost { get; set; }
    }
}
