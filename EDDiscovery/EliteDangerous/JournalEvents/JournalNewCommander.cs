
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: Creating a new commander
    //Parameters:
    //•	Name: (new) commander name
    //•	Package: selected starter package
    public class JournalNewCommander : JournalEntry
    {
        public JournalNewCommander(JObject evt ) : base(evt, JournalTypeEnum.NewCommander)
        {
            Package = JSONHelper.GetStringDef(evt["Package"]);
        }

        public string Name { get; set; }
        public string Package { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.newcommander; } }
    }
}
