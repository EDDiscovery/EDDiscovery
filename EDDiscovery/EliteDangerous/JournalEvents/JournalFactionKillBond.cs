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
            AwardingFaction = Tools.GetStringDef("AwardingFaction");
            VictimFaction = Tools.GetStringDef("VictimFaction");
            Reward = Tools.GetInt("Reward");
        }
        public string AwardingFaction { get; set; }
        public string VictimFaction { get; set; }
        public int Reward { get; set; }
    }
}
