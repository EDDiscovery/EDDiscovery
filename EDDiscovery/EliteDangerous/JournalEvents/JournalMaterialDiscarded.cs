
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{

    public class JournalMaterialDiscarded : JournalEntry
    {
        public JournalMaterialDiscarded(JObject evt ) : base(evt, JournalTypeEnum.MaterialDiscarded)
        {
            Category = Tools.GetStringDef(evt["Category"]);
            Name = Tools.GetStringDef(evt["Name"]);
            Count = Tools.GetInt(evt["Count"]);

        }
        public string Category { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }

    }
}
