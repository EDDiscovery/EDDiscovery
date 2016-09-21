using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when signing up to a community goal
    //Parameters:
    //•	Name
    //•	System
    public class JournalCommunityGoalJoin : JournalEntry
    {
        public JournalCommunityGoalJoin(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.CommunityGoalJoin, reader)
        {
            Name = evt.Value<string>("Name");
            System = evt.Value<string>("System");
        }
        public string Name { get; set; }
        public string System { get; set; }
    }
}
