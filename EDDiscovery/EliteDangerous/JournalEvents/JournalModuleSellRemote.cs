using Newtonsoft.Json.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when selling a module in outfitting
    //Parameters:
    //•	Slot
    //•	SellItem
    //•	SellPrice
    //•	Ship
    public class JournalModuleSellRemote : JournalEntry
    {
        public JournalModuleSellRemote(JObject evt) : base(evt, JournalTypeEnum.ModuleSellRemote)
        {
            Slot = JSONHelper.GetStringDef(evt["StorageSlot"]);
            SellItem = JSONHelper.GetStringDef(evt["SellItem"]);
            SellItemLocalised = JSONHelper.GetStringDef(evt["SellItem_Localised"]);
            SellPrice = JSONHelper.GetLong(evt["SellPrice"]);
            Ship = JSONHelper.GetStringDef(evt["Ship"]);
            ShipId = JSONHelper.GetInt(evt["ShipID"]);
            ServerId = JSONHelper.GetInt(evt["ServerId"]);
        }
        public string Slot { get; set; }
        public string SellItem { get; set; }
        public string SellItemLocalised { get; set; }
        public long SellPrice { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }
        public int ServerId { get; set; }
        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";ShipID;ServerID";
        }
        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.modulesell; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            string s = (SellItemLocalised.Length > 0) ? SellItemLocalised : SellItem;

            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, SellItemLocalised + " on " + Ship, SellPrice);
        }

    }
}
