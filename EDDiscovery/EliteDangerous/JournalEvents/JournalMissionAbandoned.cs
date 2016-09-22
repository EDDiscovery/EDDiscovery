
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when a mission has been abandoned
    //Parameters:
    //•	Name: name of mission
    public class JournalMissionAbandoned : JournalEntry
    {
        public JournalMissionAbandoned(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.MissionAbandoned, reader)
        {
            Name = evt.Value<string>("Name");
            MissionId = evt.Value<int>("MissionID");
        }
        public string Name { get; set; }
        public int MissionId { get; set; }
    }
}
