using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when purchasing an SRV or Fighter
    //Parameters:
    //•	Type: type of vehicle being purchased (SRV or fighter model)
    //•	Loadout: variant
    //•	Cost: purchase cost
    //•	Count: number of vehicles purchased
    public class JournalRestockVehicle : JournalEntry
    {
        public JournalRestockVehicle(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.RestockVehicle, reader)
        {
            Type = evt.Value<string>("Type");
            Loadout = evt.Value<string>("Loadout");
            Cost = evt.Value<int>("Cost");
            Count = evt.Value<int>("Count");
        }
        public string Type { get; set; }
        public string Loadout { get; set; }
        public int Cost { get; set; }
        public int Count { get; set; }

    }
}
