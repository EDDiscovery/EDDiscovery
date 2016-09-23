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
            Cost = Tools.GetInt("Cost");
            Amount = Tools.GetInt("Amount");
        }
        public int Cost { get; set; }
        public int Amount { get; set; }

    }
}
