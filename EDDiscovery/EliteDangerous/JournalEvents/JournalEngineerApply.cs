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
            Engineer = JSONHelper.GetStringDef(evt["Engineer"]);
            Blueprint = JSONHelper.GetStringDef(evt["Blueprint"]);
            Level = JSONHelper.GetInt(evt["Level"]);
            Override = JSONHelper.GetStringDef("Override");

        }
        public string Engineer { get; set; }
        public string Blueprint { get; set; }
        public int Level { get; set; }
        public string Override { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.engineerapply; } }
    }
}
