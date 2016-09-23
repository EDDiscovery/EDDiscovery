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
            Type = Tools.GetStringDef("Type");
            Loadout = Tools.GetStringDef("Loadout");
            Cost = Tools.GetInt("Cost");
            Count = Tools.GetInt("Count");
        }
        public string Type { get; set; }
        public string Loadout { get; set; }
        public int Cost { get; set; }
        public int Count { get; set; }

    }
}
