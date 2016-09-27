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
        public JournalModuleSwap(JObject evt ) : base(evt, JournalTypeEnum.ModuleSwap)
        {
            FromSlot = Tools.GetStringDef(evt["FromSlot"]);
            ToSlot = Tools.GetStringDef(evt["ToSlot"]);
            FromItem = Tools.GetStringDef(evt["FromItem"]);
            ToItem = Tools.GetStringDef(evt["ToItem"]);
            Ship = Tools.GetStringDef(evt["Ship"]);
            ShipId = Tools.GetInt(evt["ShipID"]);

        }
        public string FromSlot { get; set; }
        public string ToSlot { get; set; }
        public string FromItem { get; set; }
        public string ToItem { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";ShipID";
        }

    }
}
