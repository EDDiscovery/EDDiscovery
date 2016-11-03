
using Newtonsoft.Json.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
//    When written: when requesting a module is transferred from storage at another station
//Parameters:
//•	StorageSlot
//•	StoredItem
//•	ServerId
//•	TransferCost
//•	Ship
//•	ShipId

    public class JournalFetchRemoteModule : JournalEntry
    {
        public JournalFetchRemoteModule(JObject evt) : base(evt, JournalTypeEnum.FetchRemoteModule)
        {
            StorageSlot = JSONHelper.GetStringDef(evt["StorageSlot"]);
            StoredItem = JSONHelper.GetStringDef(evt["StoredItem"]);
            StoredItemLocalised = JSONHelper.GetStringDef(evt["StoredItem_Localised"]);
            TransferCost = JSONHelper.GetLong(evt["TransferCost"]);
            Ship = JSONHelper.GetStringDef(evt["Ship"]);
            ShipId = JSONHelper.GetInt(evt["ShipID"]);
            ServerId = JSONHelper.GetInt(evt["ServerId"]);
        }
        public string StorageSlot { get; set; }
        public string StoredItem { get; set; }
        public string StoredItemLocalised { get; set; }
        public long TransferCost { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }
        public int ServerId { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";ShipID;ServerID";
        }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, StoredItemLocalised + " on " + Ship, -TransferCost);
        }


    }
}
