using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: player is awarded a bounty for a kill
    //Parameters:
    //•	Faction: the faction awarding the bounty
    //•	Reward: the reward value
    //•	VictimFaction: the victim’s faction
    //•	SharedWithOthers: whether shared with other players
    public class JournalBounty : JournalEntry
    {
        public JournalBounty(JObject evt ) : base(evt, JournalTypeEnum.Bounty)
        {
            Reward = JSONHelper.GetLong(evt["Reward"]);     // some of them..
            TotalReward = JSONHelper.GetLong(evt["TotalReward"]);     // others of them..

            VictimFaction = JSONHelper.GetStringDef(evt["VictimFaction"]);
            VictimFactionLocalised = JSONHelper.GetStringDef(evt["VictimFaction_Localised"]); // may not be present

            SharedWithOthers = JSONHelper.GetBool(evt["SharedWithOthers"],false);
            Rewards = evt["Rewards"]?.ToObject<BountyReward[]>();
        }

        public long Reward { get; set; }     
        public long TotalReward { get; set; }
        public string VictimFaction { get; set; }
        public string VictimFactionLocalised { get; set; }
        public bool SharedWithOthers { get; set; }
        public BountyReward[] Rewards { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.bounty; } }

        public void LedgerNC(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            string n = (VictimFactionLocalised.Length > 0) ? VictimFactionLocalised : VictimFaction;
            n += " total " + (TotalReward + Reward).ToString("N0");

            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, n);
        }

    }

    public class BountyReward
    {
        public string Faction;
        public long Reward;
    }

}
