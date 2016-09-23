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
        public JournalCommunityGoalReward(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.CommunityGoalReward, reader)
        {
            Name = Tools.GetStringDef("Name");
            System = Tools.GetStringDef("System");
            Reward = Tools.GetInt("Reward");
        }
        public string Name { get; set; }
        public string System { get; set; }
        public int Reward { get; set; }

    }
}
