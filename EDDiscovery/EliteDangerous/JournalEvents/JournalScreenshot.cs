using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: liftoff from a landing pad in a station, outpost or settlement
    //Parameters:
    //•	StationName: name of station

    //•	Security
    public class JournalScreenshot : JournalEntry
    {
        public JournalScreenshot(JObject evt ) : base(evt, JournalTypeEnum.Screenshot)
        {
            StationName = JSONHelper.GetStringDef(evt["StationName"]);

        }
        public string StationName { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.screenshot; } }

    }
}
