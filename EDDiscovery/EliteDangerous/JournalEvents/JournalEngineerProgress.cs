using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: liftoff from a landing pad in a station, outpost or settlement
    //Parameters:
    //•	StationName: name of station

    //•	Security
    public class JournalEngineerProgress : JournalEntry
    {
        public JournalEngineerProgress(JObject evt ) : base(evt, JournalTypeEnum.EngineerProgress)
        {
            StationName = JSONHelper.GetStringDef(evt["StationName"]);

        }
        public string StationName { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.engineerprogress; } }

    }
}
