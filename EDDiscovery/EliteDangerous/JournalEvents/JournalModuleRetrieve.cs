using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when fetching a previously stored module
    //Parameters:
    //•	Slot
    //•	Ship
    //•	ShipID
    //•	RetrievedItem
    //•	EngineerModifications: name of modification blueprint, if any
    //•	SwapOutItem (if slot was not empty)
    //•	Cost
    public class JournalModuleRetrieve : JournalEntry
    {
        public JournalModuleRetrieve(JObject evt) : base(evt, JournalTypeEnum.ModuleRetrieve)
        {
            Slot = JSONHelper.GetStringDef(evt["Slot"]);
            Ship = JSONHelper.GetStringDef(evt["Ship"]);
            ShipId = JSONHelper.GetInt(evt["ShipID"]);
            RetrievedItem = JSONHelper.GetStringDef(evt["RetrievedItem"]);
            RetrievedItemLocalised = JSONHelper.GetStringDef(evt["RetrievedItem_Localised"]);
            EngineerModifications = JSONHelper.GetStringDef(evt["EngineerModifications"]);
            SwapOutItem = JSONHelper.GetStringDef(evt["SwapOutItem"]);
            Cost = JSONHelper.GetLong(evt["Cost"]);
        }
        public string Slot { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }
        public string RetrievedItem { get; set; }
        public string RetrievedItemLocalised { get; set; }
        public string EngineerModifications { get; set; }
        public string SwapOutItem { get; set; }
        public long Cost { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";ShipID";
        }
        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.moduleretrieve; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            string s = (RetrievedItemLocalised.Length > 0) ? RetrievedItemLocalised : RetrievedItem;

            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, s + " on " + Ship, -Cost);
        }

    }
}
