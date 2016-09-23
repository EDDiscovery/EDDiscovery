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
        public JournalModuleBuy(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.ModuleBuy, reader)
        {
            Slot = Tools.GetStringDef("Slot");
            BuyItem = Tools.GetStringDef("BuyItem");
            BuyPrice = Tools.GetInt("BuyPrice");
            Ship = Tools.GetStringDef("Ship");
            ShipId = Tools.GetInt("ShipId");
            SellItem = Tools.GetStringDef("SellItem");
            SellPrice = evt.Value<int?>("SellPrice");

        }
        public string Slot { get; set; }
        public string BuyItem { get; set; }
        public int BuyPrice { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }
        public string SellItem { get; set; }
        public int? SellPrice { get; set; }

    }
}
