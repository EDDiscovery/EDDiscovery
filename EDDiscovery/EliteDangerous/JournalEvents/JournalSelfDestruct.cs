
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
        public JournalSelfDestruct(JObject evt, EDJournalReader reader) : base(evt,   JournalTypeEnum.SelfDestruct, reader)
        {


        }


    }
}
