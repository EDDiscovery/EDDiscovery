
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
        public JournalNewCommander(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.NewCommander, reader)
        {
            Package = evt.Value<string>("Package");
        }

        public string Name { get; set; }
        public string Package { get; set; }
    }
}
