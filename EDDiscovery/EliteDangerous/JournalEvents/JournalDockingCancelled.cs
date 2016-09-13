using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when the player cancels a docking request
    //Parameters:
    //•	StationName: name of station
    public class JournalDockingCancelled : JournalEntry
    {
        public JournalDockingCancelled(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.DockingCancelled, reader)
        {
            StationName = evt.Value<string>("StationName");
        }
        public string StationName { get; set; }
    }
}
