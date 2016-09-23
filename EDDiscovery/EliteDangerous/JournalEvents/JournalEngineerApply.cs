using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when applying an engineer’s upgrade to a module
    //Parameters:
    //•	Engineer: name of engineer
    //•	Blueprint: blueprint being applied
    //•	Level: crafting level
    //•	Override: whether overriding special effect
    public class JournalEngineerApply : JournalEntry
    {
        public JournalEngineerApply(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.EngineerApply, reader)
        {
            Engineer = Tools.GetStringDef("Engineer");
            Blueprint = Tools.GetStringDef("Blueprint");
            Level = Tools.GetInt("Level");
            Override = Tools.GetBool("Override");

        }
        public string Engineer { get; set; }
        public string Blueprint { get; set; }
        public int Level { get; set; }
        public bool? Override { get; set; }

    }
}
