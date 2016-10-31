using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when repairing the ship
    //Parameters:
    //•	Item: all, wear, hull, paint, or name of module
    //•	Cost: cost of repair
    public class JournalRepairAll : JournalEntry
    {
        public JournalRepairAll(JObject evt) : base(evt, JournalTypeEnum.RepairAll)
        {
            Cost = Tools.GetLong(evt["Cost"]);

        }
        public long Cost { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.repairall; } }

    }
}
