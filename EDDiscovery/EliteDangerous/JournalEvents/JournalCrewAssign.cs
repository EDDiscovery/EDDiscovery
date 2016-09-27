using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when changing the task assignment of a member of crew
    //Parameters:
    //•	Name
    //•	Role

    public class JournalCrewAssign : JournalEntry
    {
        public JournalCrewAssign(JObject evt) : base(evt, JournalTypeEnum.CrewAssign)
        {
            Name = Tools.GetStringDef(evt["Name"]);
            Role = Tools.GetStringDef(evt["Role"]);
        }
        public string Name { get; set; }
        public string Role { get; set; }
    }
}
