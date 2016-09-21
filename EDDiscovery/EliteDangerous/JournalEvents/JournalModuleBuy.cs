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
            Slot = evt.Value<string>("Slot");
            BuyItem = evt.Value<string>("BuyItem");
            BuyPrice = evt.Value<int>("BuyPrice");
            Ship = evt.Value<string>("Ship");
            ShipId = evt.Value<int>("ShipId");
            SellItem = evt.Value<string>("SellItem");
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
