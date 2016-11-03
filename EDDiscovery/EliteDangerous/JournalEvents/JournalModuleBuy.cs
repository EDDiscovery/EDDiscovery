using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when buying a module in outfitting
    //Parameters:
    //•	Slot: the outfitting slot
    //•	BuyItem: the module being purchased
    //•	BuyPrice: price paid
    //•	Ship: the players ship
    //If replacing an existing module:
    //•	SellItem: item being sold
    //•	SellPrice: sale price
    public class JournalModuleBuy : JournalEntry
    {
        public JournalModuleBuy(JObject evt ) : base(evt, JournalTypeEnum.ModuleBuy)
        {
            Slot = JSONHelper.GetStringDef(evt["Slot"]);
            BuyItem = JSONHelper.GetStringDef(evt["BuyItem"]);
            BuyItemLocalised = JSONHelper.GetStringDef(evt["BuyItem_Localised"]);
            BuyPrice = JSONHelper.GetLong(evt["BuyPrice"]);
            Ship = JSONHelper.GetStringDef(evt["Ship"]);
            ShipId = JSONHelper.GetInt(evt["ShipID"]);
            SellItem = JSONHelper.GetStringDef(evt["SellItem"]);
            SellItemLocalised = JSONHelper.GetStringDef(evt["SellItem_Localised"]);
            SellPrice = JSONHelper.GetLongNull(evt["SellPrice"]);

        }
        public string Slot { get; set; }
        public string BuyItem { get; set; }
        public string BuyItemLocalised { get; set; }
        public long BuyPrice { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }
        public string SellItem { get; set; }
        public string SellItemLocalised { get; set; }
        public long? SellPrice { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";ShipID";
        }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.modulebuy; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            string s = (BuyItemLocalised.Length > 0) ? BuyItemLocalised : BuyItem;

            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, s + " on " + Ship, -BuyPrice + ( SellPrice??0) );
        }

    }
}
