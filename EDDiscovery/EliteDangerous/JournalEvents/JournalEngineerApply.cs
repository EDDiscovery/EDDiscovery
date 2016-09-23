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
        public JournalEngineerApply(JObject evt ) : base(evt, JournalTypeEnum.EngineerApply)
        {
            Engineer = Tools.GetStringDef(evt["Engineer"]);
            Blueprint = Tools.GetStringDef(evt["Blueprint"]);
            Level = Tools.GetInt(evt["Level"]);
            Override = Tools.GetBool(evt["Override"]);

        }
        public string Engineer { get; set; }
        public string Blueprint { get; set; }
        public int Level { get; set; }
        public bool? Override { get; set; }

    }
}
