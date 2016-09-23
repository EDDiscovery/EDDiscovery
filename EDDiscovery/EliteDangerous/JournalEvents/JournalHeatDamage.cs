
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: Player rewarded for taking part in a combat zone
    //Parameters: 
    //•	Reward
    //•	AwardingFaction
    //•	VictimFaction
    public class JournalHeatDamage : JournalEntry
    {
        public JournalHeatDamage(JObject evt ) : base(evt, JournalTypeEnum.HeatDamage)
        {
            AwardingFaction = Tools.GetStringDef(evt["AwardingFaction"]);
            VictimFaction = Tools.GetStringDef(evt["VictimFaction"]);
            Reward = Tools.GetInt(evt["Reward"]);
        }
        public string AwardingFaction { get; set; }
        public string VictimFaction { get; set; }
        public int Reward { get; set; }

    }
}
