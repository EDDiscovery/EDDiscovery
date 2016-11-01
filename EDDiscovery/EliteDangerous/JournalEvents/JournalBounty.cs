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
            Faction = JSONHelper.GetStringDef(evt["Faction"]);
            Reward = JSONHelper.GetLong(evt["Reward"]);
            VictimFaction = JSONHelper.GetStringDef(evt["VictimFaction"]);
            SharedWithOthers = JSONHelper.GetBool(evt["SharedWithOthers"],false);
            Rewards = evt["Rewards"]?.ToObject<BountyReward[]>();
        }
        public string Faction { get; set; }
        public long Reward { get; set; }                // might be wrong Finwen TBD
        public string VictimFaction { get; set; }
        public bool SharedWithOthers { get; set; }
        public BountyReward[] Rewards { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.bounty; } }
    }

    public class BountyReward
    {
        public string Faction;
        public long Reward;
    }

}
