using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
//    When written: when storing a module in Outfitting
//    Parameters:
//•	Slot
//•	Ship
//•	ShipID
//•	StoredItem
//•	EngineerModifications: name of modification blueprint, if any
//•	ReplacementItem(if a core module)
//•	Cost(if any)

    public class JournalModuleStore : JournalEntry
    {
        public JournalModuleStore(JObject evt) : base(evt, JournalTypeEnum.ModuleStore)
        {
            Slot = Tools.GetStringDef(evt["Slot"]);
            Ship = Tools.GetStringDef(evt["Ship"]);
            ShipId = Tools.GetInt(evt["ShipID"]);
            StoredItem = Tools.GetStringDef(evt["StoredItem"]);
            EngineerModifications = Tools.GetStringDef(evt["EngineerModifications"]);
            ReplacementItem = Tools.GetStringDef(evt["ReplacementItem"]);
            Cost = Tools.GetLong(evt["Cost"]);
        }
        public string Slot { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }
        public string StoredItem { get; set; }
        public string EngineerModifications { get; set; }
        public string ReplacementItem { get; set; }
        public long Cost { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";ShipID";
        }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.modulestore; } }

    }
}
