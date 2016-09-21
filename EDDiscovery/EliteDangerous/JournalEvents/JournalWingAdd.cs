using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: another player has joined the wing
    //Parameters:
    //•	Name
    public class JournalWingAdd : JournalEntry
    {
        public JournalWingAdd(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.WingAdd, reader)
        {
            Name = evt.Value<string>("Name");

        }
        public string Name { get; set; }

    }
}
