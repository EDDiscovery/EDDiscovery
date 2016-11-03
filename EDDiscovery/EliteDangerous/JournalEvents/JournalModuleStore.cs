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
            Slot = JSONHelper.GetStringDef(evt["Slot"]);
            Ship = JSONHelper.GetStringDef(evt["Ship"]);
            ShipId = JSONHelper.GetInt(evt["ShipID"]);
            StoredItem = JSONHelper.GetStringDef(evt["StoredItem"]);
            StoredItemLocalised = JSONHelper.GetStringDef(evt["StoredItem_Localised"]);
            EngineerModifications = JSONHelper.GetStringDef(evt["EngineerModifications"]);
            ReplacementItem = JSONHelper.GetStringDef(evt["ReplacementItem"]);
            Cost = JSONHelper.GetLong(evt["Cost"]);
        }
        public string Slot { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }
        public string StoredItem { get; set; }
        public string StoredItemLocalised { get; set; }
        public string EngineerModifications { get; set; }
        public string ReplacementItem { get; set; }
        public long Cost { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";ShipID";
        }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.modulestore; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            string s = (StoredItemLocalised.Length > 0) ? StoredItemLocalised : StoredItem;
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, s +" on " + Ship, -Cost);
        }
    }
}
