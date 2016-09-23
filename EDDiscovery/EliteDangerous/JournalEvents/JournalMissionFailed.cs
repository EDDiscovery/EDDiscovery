using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when a mission has failed
    //Parameters:
    //•	Name: name of mission
    public class JournalMissionFailed : JournalEntry
    {
        public JournalMissionFailed(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.MissionFailed, reader)
        {
            Name = Tools.GetStringDef("Name");
            MissionId = Tools.GetInt("MissionID");
        }

        public string Name { get; set; }
        public int MissionId { get; set; }

    }
}
