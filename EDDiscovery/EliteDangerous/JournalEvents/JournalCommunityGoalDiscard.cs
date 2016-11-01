using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //Parameters:
    //•	Name
    //•	System
    public class JournalCommunityGoalDiscard : JournalEntry
    {
        public JournalCommunityGoalDiscard(JObject evt) : base(evt, JournalTypeEnum.CommunityGoalDiscard)
        {
            Name = JSONHelper.GetStringDef(evt["Name"]);
            System = JSONHelper.GetStringDef(evt["System"]);
        }
        public string Name { get; set; }
        public string System { get; set; }
    }
}
