using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when a crime is recorded against the player
    //Parameters:
    //•	CrimeType
    //•	Faction
    //Optional parameters (depending on crime)
    //•	Victim
    //•	Fine
    //•	Bounty
    public class JournalCommitCrime : JournalEntry
    {
        public JournalCommitCrime(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.CommitCrime, reader)
        {
            CrimeType = evt.Value<string>("CrimeType");
            Faction = evt.Value<string>("Faction");
            Fine = evt.Value<int?>("Fine");
            Bounty = evt.Value<int?>("Bounty");
        }
        public string CrimeType { get; set; }
        public string Faction { get; set; }
        public string Victim { get; set; }
        public int? Fine { get; set; }
        public int? Bounty { get; set; }
    }
}
