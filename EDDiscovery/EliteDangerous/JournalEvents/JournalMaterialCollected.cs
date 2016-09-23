using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: whenever materials are collected 
    //Parameters: 
    //•	Category: type of material (Raw/Encoded/Manufactured)
    //•	Name: name of material
    public class JournalMaterialCollected : JournalEntry
    {
        public JournalMaterialCollected(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.MaterialCollected, reader)
        {
            Category = Tools.GetStringDef("Category");
            Name = Tools.GetStringDef("Name");
        }
        public string Category { get; set; }
        public string Name { get; set; }
    }
}
