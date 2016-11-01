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
            Name = JSONHelper.GetStringDef(evt["Name"]);
            Role = JSONHelper.GetStringDef(evt["Role"]);
        }
        public string Name { get; set; }
        public string Role { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.crew; } }

    }
}
