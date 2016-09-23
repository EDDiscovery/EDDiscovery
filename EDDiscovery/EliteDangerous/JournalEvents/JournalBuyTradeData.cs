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
            System = Tools.GetStringDef("System");
            Cost = Tools.GetInt("Cost");
        }

        public string System { get; set; }
        public int Cost { get; set; }
    }
}
