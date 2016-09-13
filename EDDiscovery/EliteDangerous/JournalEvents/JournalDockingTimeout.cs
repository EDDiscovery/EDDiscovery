using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when a docking request has timed out
    //Parameters:
    //•	StationName: name of station
    public class JournalDockingTimeout : JournalEntry
    {
        public JournalDockingTimeout(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.DockingTimeout, reader)
        {
            StationName = evt.Value<string>("StationName");
        }
        public string StationName { get; set; }
    }
}
