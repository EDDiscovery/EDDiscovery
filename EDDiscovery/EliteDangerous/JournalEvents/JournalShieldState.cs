
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: liftoff from a landing pad in a station, outpost or settlement
    //Parameters:
    //•	StationName: name of station

    //•	Security
    public class JournalShieldState : JournalEntry
    {
        public JournalShieldState(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.ShieldState, reader)
        {
            ShieldsUp = evt.Value<bool>("ShieldsUp");

        }
        public bool ShieldsUp { get; set; }

    }
}
