using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when repairing the ship
    //Parameters:
    //•	Item: all, wear, hull, paint, or name of module
    //•	Cost: cost of repair
    public class JournalRepair : JournalEntry
    {
        public JournalRepair(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.Repair, reader)
        {
            Item = evt.Value<string>("Item");
            Cost = evt.Value<int>("Cost");

        }
        public string Item { get; set; }
        public int Cost { get; set; }

    }
}
