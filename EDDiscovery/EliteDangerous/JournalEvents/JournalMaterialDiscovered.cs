
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: liftoff from a landing pad in a station, outpost or settlement
    //Parameters:
    //•	StationName: name of station

    //•	Security
    public class JournalMaterialDiscovered : JournalEntry
    {
        public JournalMaterialDiscovered(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.MaterialDiscovered, reader)
        {
            Category = Tools.GetStringDef("Category");
            Name = Tools.GetStringDef("Name");
            DiscoveryNumber = Tools.GetInt("DiscoveryNumber");
        }
        public string Category { get; set; }
        public string Name { get; set; }
        public int DiscoveryNumber { get; set; }

    }
}
