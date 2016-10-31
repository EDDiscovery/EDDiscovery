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
        public JournalFactionKillBond(JObject evt ) : base(evt, JournalTypeEnum.FactionKillBond)
        {
            AwardingFaction = Tools.GetStringDef(evt["AwardingFaction"]);
            VictimFaction = Tools.GetStringDef(evt["VictimFaction"]);
            Reward = Tools.GetLong(evt["Reward"]);
        }
        public string AwardingFaction { get; set; }
        public string VictimFaction { get; set; }
        public long Reward { get; set; }
    }
}
