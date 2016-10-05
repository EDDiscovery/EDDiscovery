using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: player is awarded a bounty for a kill
    //Parameters: 
    //•	Faction: the faction awarding the bounty
    //•	Reward: the reward value
    //•	VictimFaction: the victim’s faction
    //•	SharedWithOthers: whether shared with other players
    public class JournalBounty : JournalEntry
    {
        public JournalBounty(JObject evt ) : base(evt, JournalTypeEnum.Bounty)
        {
            Faction = Tools.GetStringDef(evt["Faction"]);
            Reward = Tools.GetInt(evt["Reward"]);
            VictimFaction = Tools.GetStringDef(evt["VictimFaction"]);
            SharedWithOthers = Tools.GetBool(evt["SharedWithOthers"],false);

        }
        public string Faction { get; set; }
        public int Reward { get; set; }
        public string VictimFaction { get; set; }
        public bool SharedWithOthers { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.bounty; } }
    }
}
