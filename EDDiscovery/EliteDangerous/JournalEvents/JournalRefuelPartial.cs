using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: liftoff from a landing pad in a station, outpost or settlement
    //Parameters:
    //•	StationName: name of station

    //•	Security
    public class JournalRefuelPartial : JournalEntry
    {
        public JournalRefuelPartial(JObject evt ) : base(evt, JournalTypeEnum.RefuelPartial)
        {
            Cost = Tools.GetInt(evt["Cost"]);
            Amount = Tools.GetInt(evt["Amount"]);
        }
        public int Cost { get; set; }
        public int Amount { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.refuel; } }

    }
}
