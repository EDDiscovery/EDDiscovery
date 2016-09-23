using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: liftoff from a landing pad in a station, outpost or settlement
    //Parameters:
    //•	StationName: name of station

    //•	Security
    public class JournalRefuelPartial : JournalEntry
    {
        public JournalRefuelPartial(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.RefuelPartial, reader)
        {
            Cost = Tools.GetInt("Cost");
            Amount = Tools.GetInt("Amount");
        }
        public int Cost { get; set; }
        public int Amount { get; set; }

    }
}
