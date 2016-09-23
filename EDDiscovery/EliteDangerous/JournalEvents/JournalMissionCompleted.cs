using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when a mission is completed
    //Parameters:
    //•	Name: mission type
    //•	Faction: faction name
    //Optional parameters (depending on mission type)
    //•	Commodity
    //•	Count
    //•	Target
    //•	TargetType
    //•	TargetFaction
    //•	Reward: value of reward
    //•	Donation: donation offered (for altruism missions)
    //•	PermitsAwarded:[] (names of any permits awarded, as a JSON array)
    public class JournalMissionCompleted : JournalEntry
    {
        public JournalMissionCompleted(JObject evt ) : base(evt, JournalTypeEnum.MissionCompleted)
        {
            Name = Tools.GetStringDef(evt["Name"]);
            Faction = Tools.GetStringDef(evt["Faction"]);
            Commodity = Tools.GetStringDef(evt["Commodity"]);
            Count = evt.Value<int?>("Count");
            Target = Tools.GetStringDef(evt["Target"]);
            TargetType = Tools.GetStringDef(evt["TargetType"]);
            TargetFaction = Tools.GetStringDef(evt["TargetFaction"]);
            Reward = evt.Value<int?>("Reward") ?? 0;
            Donation = evt.Value<int?>("Donation");

            if ( !Tools.IsNullOrEmptyT( evt["PermitsAwarded"]))
                PermitsAwarded = evt.Value<JArray>("PermitsAwarded").Values<string>().ToArray();

            MissionId = Tools.GetInt(evt["MissionID"]);
        }
        public string Name { get; set; }
        public string Faction { get; set; }
        public string Commodity { get; set; }
        public int? Count { get; set; }
        public string Target { get; set; }
        public string TargetType { get; set; }
        public string TargetFaction { get; set; }
        public int Reward { get; set; }
        public int? Donation { get; set; }
        public string[] PermitsAwarded { get; set; }
        public int MissionId { get; set; }

    }
}
