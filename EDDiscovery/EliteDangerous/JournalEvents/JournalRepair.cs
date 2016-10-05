using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when repairing the ship
    //Parameters:
    //•	Item: all, wear, hull, paint, or name of module
    //•	Cost: cost of repair
    public class JournalRepair : JournalEntry
    {
        public JournalRepair(JObject evt ) : base(evt, JournalTypeEnum.Repair)
        {
            Item = Tools.GetStringDef(evt["Item"]);
            Cost = Tools.GetInt(evt["Cost"]);

        }
        public string Item { get; set; }
        public int Cost { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.repair; } }

    }
}
