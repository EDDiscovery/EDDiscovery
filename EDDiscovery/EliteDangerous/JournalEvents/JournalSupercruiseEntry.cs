using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: entering supercruise from normal space
    //Parameters:
    //•	Starsystem
    public class JournalSupercruiseEntry : JournalEntry
    {
        public JournalSupercruiseEntry(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.SupercruiseEntry, reader)
        {
            StarSystem = evt.Value<string>("StarSystem");

        }
        public string StarSystem { get; set; }

    }
}
