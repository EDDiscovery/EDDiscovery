using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when receiving a reward for a community goal
    //Parameters:
    //•	Name
    //•	System
    //•	Reward
    public class JournalCommunityGoalReward : JournalEntry
    {
        public JournalCommunityGoalReward(JObject evt ) : base(evt, JournalTypeEnum.CommunityGoalReward)
        {
            Name = Tools.GetStringDef(evt["Name"]);
            System = Tools.GetStringDef(evt["System"]);
            Reward = Tools.GetLong(evt["Reward"]);
        }
        public string Name { get; set; }
        public string System { get; set; }
        public long Reward { get; set; }

    }
}
