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
            Name = evt.Value<string>("Name");
            System = evt.Value<string>("System");
            Reward = evt.Value<int>("Reward");
        }
        public string Name { get; set; }
        public string System { get; set; }
        public int Reward { get; set; }

    }
}
