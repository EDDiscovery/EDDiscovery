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
            Name = JSONHelper.GetStringDef(evt["Name"]);
            Faction = JSONHelper.GetStringDef(evt["Faction"]);
            Commodity = JSONHelper.GetStringDef(evt["Commodity"]);
            Count = JSONHelper.GetIntNull(evt["Count"]);
            Target = JSONHelper.GetStringDef(evt["Target"]);
            TargetType = JSONHelper.GetStringDef(evt["TargetType"]);
            TargetFaction = JSONHelper.GetStringDef(evt["TargetFaction"]);
            Reward = JSONHelper.GetLongNull(evt["Reward"]) ?? 0;
            Donation = JSONHelper.GetLongNull(evt["Donation"]);

            if ( !JSONHelper.IsNullOrEmptyT( evt["PermitsAwarded"]))
                PermitsAwarded = evt.Value<JArray>("PermitsAwarded").Values<string>().ToArray();

            MissionId = JSONHelper.GetInt(evt["MissionID"]);
        }
        public string Name { get; set; }
        public string Faction { get; set; }
        public string Commodity { get; set; }
        public int? Count { get; set; }
        public string Target { get; set; }
        public string TargetType { get; set; }
        public string TargetFaction { get; set; }
        public long? Reward { get; set; }
        public long? Donation { get; set; }
        public string[] PermitsAwarded { get; set; }
        public int MissionId { get; set; }
        public string CommodityReward { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";MissionID";
        }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.missioncompleted; } }

    }
}
