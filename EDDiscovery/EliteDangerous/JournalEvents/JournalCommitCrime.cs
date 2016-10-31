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
        public JournalCommitCrime(JObject evt ) : base(evt, JournalTypeEnum.CommitCrime)
        {
            CrimeType = Tools.GetStringDef(evt["CrimeType"]);
            Faction = Tools.GetStringDef(evt["Faction"]);
            Fine = evt.Value<long?>("Fine");
            Bounty = evt.Value<long?>("Bounty");
        }
        public string CrimeType { get; set; }
        public string Faction { get; set; }
        public string Victim { get; set; }
        public long? Fine { get; set; }
        public long? Bounty { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.commitcrime; } }

    }
}
