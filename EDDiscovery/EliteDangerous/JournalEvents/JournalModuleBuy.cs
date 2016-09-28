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
            Slot = Tools.GetStringDef(evt["Slot"]);
            BuyItem = Tools.GetStringDef(evt["BuyItem"]);
            BuyPrice = Tools.GetInt(evt["BuyPrice"]);
            Ship = Tools.GetStringDef(evt["Ship"]);
            ShipId = Tools.GetInt(evt["ShipID"]);
            SellItem = Tools.GetStringDef(evt["SellItem"]);
            SellPrice = evt.Value<int?>("SellPrice");

        }
        public string Slot { get; set; }
        public string BuyItem { get; set; }
        public int BuyPrice { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }
        public string SellItem { get; set; }
        public int? SellPrice { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";ShipID";
        }

    }
}
