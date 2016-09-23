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
            FromSlot = Tools.GetStringDef("FromSlot");
            ToSlot = Tools.GetStringDef("ToSlot");
            FromItem = Tools.GetStringDef("FromItem");
            ToItem = Tools.GetStringDef("ToItem");
            Ship = Tools.GetStringDef("Ship");
            ShipId = Tools.GetInt("ShipId");

        }
        public string FromSlot { get; set; }
        public string ToSlot { get; set; }
        public string FromItem { get; set; }
        public string ToItem { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }
    }
}
