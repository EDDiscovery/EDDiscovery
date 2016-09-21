using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: Player rewarded for taking part in a combat zone
    //Parameters: 
    //•	Reward
    //•	AwardingFaction
    //•	VictimFaction
    public class JournalFactionKillBond : JournalEntry
    {
        public JournalFactionKillBond(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.FactionKillBond, reader)
        {
            AwardingFaction = evt.Value<string>("AwardingFaction");
            VictimFaction = evt.Value<string>("VictimFaction");
            Reward = evt.Value<int>("Reward");
        }
        public string AwardingFaction { get; set; }
        public string VictimFaction { get; set; }
        public int Reward { get; set; }
    }
}
