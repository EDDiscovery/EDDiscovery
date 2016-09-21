
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{

    public class JournalMaterialDiscarded : JournalEntry
    {
        public JournalMaterialDiscarded(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.MaterialDiscarded, reader)
        {
            Category = evt.Value<string>("Category");
            Name = evt.Value<string>("Name");
            Count = evt.Value<int>("Count");

        }
        public string Category { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }

    }
}
