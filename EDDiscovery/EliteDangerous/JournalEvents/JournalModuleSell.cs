using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when selling a module in outfitting
    //Parameters:
    //•	Slot
    //•	SellItem
    //•	SellPrice
    //•	Ship
    public class JournalModuleSell : JournalEntry
    {
        public JournalModuleSell(JObject evt ) : base(evt, JournalTypeEnum.ModuleSell)
        {
            Slot = JSONHelper.GetStringDef(evt["Slot"]);
            SellItem = JSONHelper.GetStringDef(evt["SellItem"]);
            SellItemLocalised = JSONHelper.GetStringDef(evt["SellItem_Localised"]);
            SellPrice = JSONHelper.GetLong(evt["SellPrice"]);
            Ship = JSONHelper.GetStringDef(evt["Ship"]);
            ShipId = JSONHelper.GetInt(evt["ShipID"]);

        }
        public string Slot { get; set; }
        public string SellItem { get; set; }
        public string SellItemLocalised { get; set; }
        public long SellPrice { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";ShipID";
        }
        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.modulesell; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            string s = (SellItemLocalised.Length > 0) ? SellItemLocalised : SellItem;

            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, s + " on " + Ship, SellPrice);
        }

    }
}
