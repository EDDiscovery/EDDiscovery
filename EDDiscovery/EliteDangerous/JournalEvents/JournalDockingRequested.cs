using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when the player requests docking at a station
    //Parameters:
    //•	StationName: name of station
    public class JournalDockingRequested : JournalEntry
    {
        public JournalDockingRequested(JObject evt) : base(evt, JournalTypeEnum.DockingRequested)
        {
            StationName = evt.Value<string>("StationName");
        }
        public string StationName { get; set; }
    }
}
