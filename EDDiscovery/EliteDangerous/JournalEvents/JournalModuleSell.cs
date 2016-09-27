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
            Slot = Tools.GetStringDef(evt["Slot"]);
            SellItem = Tools.GetStringDef(evt["SellItem"]);
            SellPrice = Tools.GetInt(evt["SellPrice"]);
            Ship = Tools.GetStringDef(evt["Ship"]);
            ShipId = Tools.GetInt(evt["ShipID"]);

        }
        public string Slot { get; set; }
        public string SellItem { get; set; }
        public int SellPrice { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";ShipID";
        }

    }
}
