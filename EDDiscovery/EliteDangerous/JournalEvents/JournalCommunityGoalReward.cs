using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when receiving a reward for a community goal
    //Parameters:
    //•	Name
    //•	System
    //•	Reward
    public class JournalCommunityGoalReward : JournalEntry
    {
        public JournalCommunityGoalReward(JObject evt ) : base(evt, JournalTypeEnum.CommunityGoalReward)
        {
            Name = JSONHelper.GetStringDef(evt["Name"]);
            System = JSONHelper.GetStringDef(evt["System"]);
            Reward = JSONHelper.GetLong(evt["Reward"]);
        }
        public string Name { get; set; }
        public string System { get; set; }
        public long Reward { get; set; }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Name + " " + System, Reward);
        }

    }
}
