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
            Donation = JSONHelper.GetLongNull(evt["Donation"]) ?? 0;
            MissionId = JSONHelper.GetInt(evt["MissionID"]);

            if (!JSONHelper.IsNullOrEmptyT(evt["PermitsAwarded"]))
                PermitsAwarded = evt.Value<JArray>("PermitsAwarded").Values<string>().ToArray();

            if (!JSONHelper.IsNullOrEmptyT(evt["CommodityReward"]))
            {
                JArray rewards = (JArray)evt["CommodityReward"];

                if (rewards.Count > 0)
                {
                    CommodityReward = new System.Tuple<string, int>[rewards.Count];
                    int i = 0;
                    foreach (JToken jc in rewards.Children())
                    {
                        if (!JSONHelper.IsNullOrEmptyT(jc["Name"]) && !JSONHelper.IsNullOrEmptyT(jc["Count"]))
                            CommodityReward[i++] = new System.Tuple<string, int>(jc["Name"].Value<string>(), jc["Count"].Value<int>());

                        //System.Diagnostics.Trace.WriteLine(string.Format(" >> Child {0} {1}", jc.Path, jc.Type.ToString()));
                    }
                }
            }
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
        public System.Tuple<string, int>[] CommodityReward { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";MissionID";
        }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.missioncompleted; } }

        public void MaterialList(EDDiscovery2.DB.MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            if (CommodityReward != null)
            {
                // Forum indicates its commodities, and we get normal materialcollected events if its a material.
                for (int i = 0; i < CommodityReward.Length; i++)
                    mc.Change(EDDiscovery2.DB.MaterialCommodities.CommodityCategory, CommodityReward[i].Item1, CommodityReward[i].Item2, 0, conn);
            }
        }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Name, (Reward - Donation) , 0);
        }

    }
}
