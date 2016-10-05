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
            Slot = Tools.GetStringDef(evt["Slot"]);
            Ship = Tools.GetStringDef(evt["Ship"]);
            ShipId = Tools.GetInt(evt["ShipID"]);
            RetrievedItem = Tools.GetStringDef(evt["RetrievedItem"]);
            EngineerModifications = Tools.GetStringDef(evt["EngineerModifications"]);
            SwapOutItem = Tools.GetStringDef(evt["SwapOutItem"]);
            Cost = Tools.GetInt(evt["Cost"]);
        }
        public string Slot { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }
        public string RetrievedItem { get; set; }
        public string EngineerModifications { get; set; }
        public string SwapOutItem { get; set; }
        public int Cost { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";ShipID";
        }
        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.moduleretrieve; } }

    }
}
