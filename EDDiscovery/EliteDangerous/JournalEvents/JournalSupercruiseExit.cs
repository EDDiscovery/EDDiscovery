using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: entering supercruise from normal space
    //Parameters:
    //•	Starsystem
    public class JournalSupercruiseExit : JournalEntry
    {
        public JournalSupercruiseExit(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.SupercruiseExit, reader)
        {
            StarSystem = evt.Value<string>("StarSystem");
            Body = evt.Value<string>("Body");

        }
        public string StarSystem { get; set; }
        public string Body { get; set; }

    }
}
