using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when landing at landing pad in a space station, outpost, or surface settlement
    //Parameters:
    //•	StationName: name of station
    //•	StationType: type of station
    //•	StarSystem: name of system
    //•	CockpitBreach:true (only if landing with breached cockpit)
    //•	Faction: station’s controlling faction
    //•	FactionState
    //•	Allegiance
    //•	Economy
    //•	Government
    //•	Security
    public class JournalDockingGranted : JournalEntry
    {
        public JournalDockingGranted(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.DockingGranted, reader)
        {
            StationName = evt.Value<string>("StationName");
            LandingPad = evt.Value<int>("LandingPad");
        }
        public string StationName { get; set; }
        public int LandingPad { get; set; }
    }
}
