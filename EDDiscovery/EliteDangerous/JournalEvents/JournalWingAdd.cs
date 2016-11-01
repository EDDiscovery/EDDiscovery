using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: another player has joined the wing
    //Parameters:
    //•	Name
    public class JournalWingAdd : JournalEntry
    {
        public JournalWingAdd(JObject evt ) : base(evt, JournalTypeEnum.WingAdd)
        {
            Name = JSONHelper.GetStringDef(evt["Name"]);

        }
        public string Name { get; set; }

    }
}
