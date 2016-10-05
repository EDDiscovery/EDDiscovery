
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: liftoff from a landing pad in a station, outpost or settlement
    //Parameters:
    //•	StationName: name of station

    //•	Security
    public class JournalSelfDestruct: JournalEntry
    {
        public JournalSelfDestruct(JObject evt ) : base(evt,   JournalTypeEnum.SelfDestruct)
        {


        }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.selfdestruct; } }

    }
}
