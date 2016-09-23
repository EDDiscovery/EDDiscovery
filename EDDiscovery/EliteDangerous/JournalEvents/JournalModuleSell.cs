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
        public JournalModuleSell(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.ModuleSell, reader)
        {
            Slot = Tools.GetStringDef("Slot");
            SellItem = Tools.GetStringDef("SellItem");
            SellPrice = Tools.GetInt("SellPrice");
            Ship = Tools.GetStringDef("Ship");
            ShipId = Tools.GetInt("ShipId");

        }
        public string Slot { get; set; }
        public string SellItem { get; set; }
        public int SellPrice { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }

    }
}
