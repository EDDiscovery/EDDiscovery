using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when synthesis is used to repair or rearm
    //Parameters:
    //•	Name: synthesis blueprint
    //•	Materials: JSON object listing materials used and quantities
    public class JournalSynthesis : JournalEntry
    {
        public JournalSynthesis(JObject evt ) : base(evt, JournalTypeEnum.Synthesis)
        {
            Name = JSONHelper.GetStringDef(evt["Name"]);
            Materials = evt["Materials"]?.ToObject<Dictionary<string, int>>();
        }
        public string Name { get; set; }
        public Dictionary<string, int> Materials { get; set; }

        public void MaterialList(EDDiscovery2.DB.MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            if (Materials != null)
            {
                foreach (KeyValuePair<string, int> k in Materials)        // may be commodities or materials
                    mc.Craft(k.Key, k.Value);        // same as this, uses up materials
            }
        }

    }
}
