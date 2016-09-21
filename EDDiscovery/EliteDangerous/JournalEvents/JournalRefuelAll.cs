using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: liftoff from a landing pad in a station, outpost or settlement
    //Parameters:
    //•	StationName: name of station

    //•	Security
    public class JournalRefuelAll : JournalEntry
    {
        public JournalRefuelAll(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.RefuelAll, reader)
        {
            Cost = evt.Value<int>("Cost");
            Amount = evt.Value<int>("Amount");
        }
        public int Cost { get; set; }
        public int Amount { get; set; }

    }
}
