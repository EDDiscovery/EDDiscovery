using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: Player rewarded for taking part in a combat zone
    //Parameters:
    //•	Reward
    //•	AwardingFaction
    //•	VictimFaction
    public class JournalFactionKillBond : JournalEntry
    {
        public JournalFactionKillBond(JObject evt ) : base(evt, JournalTypeEnum.FactionKillBond)
        {
            AwardingFaction = JSONHelper.GetStringDef(evt["AwardingFaction"]);
            VictimFaction = JSONHelper.GetStringDef(evt["VictimFaction"]);
            Reward = JSONHelper.GetLong(evt["Reward"]);
        }
        public string AwardingFaction { get; set; }
        public string VictimFaction { get; set; }
        public long Reward { get; set; }

        public void LedgerNC(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, AwardingFaction + " " + Reward.ToString("N0"));
        }

    }
}
