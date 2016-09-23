
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{

    public class JournalMaterialDiscarded : JournalEntry
    {
        public JournalMaterialDiscarded(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.MaterialDiscarded, reader)
        {
            Category = Tools.GetStringDef("Category");
            Name = Tools.GetStringDef("Name");
            Count = Tools.GetInt("Count");

        }
        public string Category { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }

    }
}
