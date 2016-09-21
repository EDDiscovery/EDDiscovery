using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when requesting an engineer upgrade
    //Parameters:
    //•	Engineer: name of engineer
    //•	Blueprint: name of blueprint
    //•	Level: crafting level
    //•	Ingredients: JSON object with names and quantities of materials required
    public class JournalEngineerCraft : JournalEntry
    {
        public JournalEngineerCraft(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.EngineerCraft, reader)
        {
            Engineer = evt.Value<string>("Engineer");
            Blueprint = evt.Value<string>("Blueprint");
            Level = evt.Value<int>("Level");
            Ingredients = evt["Ingredients"]?.ToObject<Dictionary<string, int>>();
        }
        public string Engineer { get; set; }
        public string Blueprint { get; set; }
        public int Level { get; set; }
        public Dictionary<string, int> Ingredients { get; set; }
    }
}
