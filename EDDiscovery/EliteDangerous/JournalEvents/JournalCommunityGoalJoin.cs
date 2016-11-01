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
        public JournalCommunityGoalJoin(JObject evt ) : base(evt, JournalTypeEnum.CommunityGoalJoin)
        {
            Name = JSONHelper.GetStringDef(evt["Name"]);
            System = JSONHelper.GetStringDef(evt["System"]);
        }
        public string Name { get; set; }
        public string System { get; set; }
    }
}
