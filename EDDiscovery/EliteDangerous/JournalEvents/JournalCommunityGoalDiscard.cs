using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when signing up to a community goal
    //Parameters:
    //•	Name
    //•	System
    public class JournalCommunityGoalDiscard : JournalEntry
    {
        public JournalCommunityGoalDiscard(JObject evt) : base(evt, JournalTypeEnum.CommunityGoalDiscard)
        {
            Name = Tools.GetStringDef(evt["Name"]);
            System = Tools.GetStringDef(evt["System"]);
        }
        public string Name { get; set; }
        public string System { get; set; }
    }
}
