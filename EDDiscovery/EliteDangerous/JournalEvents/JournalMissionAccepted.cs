using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when starting a mission
    //Parameters:
    //•	Name: name of mission
    //•	Faction: faction offering mission
    //Optional Parameters (depending on mission type)
    //•	Commodity: commodity type
    //•	Count: number required / to deliver
    //•	Target: name of target
    //•	TargetType: type of target
    //•	TargetFaction: target’s faction
    public class JournalMissionAccepted : JournalEntry
    {
        public JournalMissionAccepted(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.MissionAccepted, reader)
        {
            Name = evt.Value<string>("Name");
            Faction = evt.Value<string>("Faction");
            Commodity = evt.Value<string>("Commodity");
            Count = evt.Value<int?>("Count");
            Target = evt.Value<string>("Target");
            TargetType = evt.Value<string>("TargetType");
            TargetFaction = evt.Value<string>("TargetFaction");
            MissionId = evt.Value<int>("MissionID");
        }
        public string Name { get; set; }
        public string Faction { get; set; }
        public string Commodity { get; set; }
        public int? Count { get; set; }
        public string Target { get; set; }
        public string TargetType { get; set; }
        public string TargetFaction { get; set; }
        public int MissionId { get; set; }
    }

}

