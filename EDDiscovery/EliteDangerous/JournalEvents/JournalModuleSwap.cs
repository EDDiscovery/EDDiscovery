using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when moving a module to a different slot on the ship
    //Parameters:
    //•	FromSlot
    //•	ToSlot
    //•	FromItem
    //•	ToItem
    //•	Ship
    public class JournalModuleSwap : JournalEntry
    {
        public JournalModuleSwap(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.ModuleSwap, reader)
        {
            FromSlot = evt.Value<string>("FromSlot");
            ToSlot = evt.Value<string>("ToSlot");
            FromItem = evt.Value<string>("FromItem");
            ToItem = evt.Value<string>("ToItem");
            Ship = evt.Value<string>("Ship");
            ShipId = evt.Value<int>("ShipId");

        }
        public string FromSlot { get; set; }
        public string ToSlot { get; set; }
        public string FromItem { get; set; }
        public string ToItem { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }
    }
}
