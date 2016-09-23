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
        public JournalEngineerProgress(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.EngineerProgress, reader)
        {
            StationName = Tools.GetStringDef("StationName");

        }
        public string StationName { get; set; }

    }
}
