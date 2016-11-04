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
            FromSlot = JSONHelper.GetStringDef(evt["FromSlot"]);
            ToSlot = JSONHelper.GetStringDef(evt["ToSlot"]);
            FromItem = JSONHelper.GetStringDef(evt["FromItem"]);
            FromItemLocalised = JSONHelper.GetStringDef(evt["FromItem_Localised"]);
            ToItem = JSONHelper.GetStringDef(evt["ToItem"]);
            ToItemLocalised = JSONHelper.GetStringDef(evt["ToItem_Localised"]);
            Ship = JSONHelper.GetStringDef(evt["Ship"]);
            ShipId = JSONHelper.GetInt(evt["ShipID"]);

        }
        public string FromSlot { get; set; }
        public string ToSlot { get; set; }
        public string FromItem { get; set; }
        public string FromItemLocalised { get; set; }
        public string ToItem { get; set; }
        public string ToItemLocalised { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";ShipID";
        }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.moduleswap; } }

    }
}
