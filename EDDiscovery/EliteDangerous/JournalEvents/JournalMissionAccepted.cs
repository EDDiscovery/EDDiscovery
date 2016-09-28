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
        public JournalMissionAccepted(JObject evt ) : base(evt, JournalTypeEnum.MissionAccepted)
        {
            Name = Tools.GetStringDef(evt["Name"]);
            Faction = Tools.GetStringDef(evt["Faction"]);
            Commodity = Tools.GetStringDef(evt["Commodity"]);
            Count = evt.Value<int?>("Count");
            Target = Tools.GetStringDef(evt["Target"]);
            TargetType = Tools.GetStringDef(evt["TargetType"]);
            TargetFaction = Tools.GetStringDef(evt["TargetFaction"]);
            MissionId = Tools.GetInt(evt["MissionID"]);
        }
        public string Name { get; set; }
        public string Faction { get; set; }
        public string Commodity { get; set; }
        public int? Count { get; set; }
        public string Target { get; set; }
        public string TargetType { get; set; }
        public string TargetFaction { get; set; }
        public int MissionId { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";MissionID";
        }

    }

}

