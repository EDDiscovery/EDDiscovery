using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
//    When written: when scanning a datalink generates a reward
//    Parameters:
//•	Reward: value in credits
//•	VictimFaction
//•	PayeeFaction

    public class JournalDatalinkVoucher : JournalEntry
    {
        public JournalDatalinkVoucher(JObject evt) : base(evt, JournalTypeEnum.DatalinkVoucher)
        {
            VictimFaction = JSONHelper.GetStringDef(evt["VictimFaction"]);
            Reward = JSONHelper.GetLong(evt["Reward"]);
            PayeeFaction = JSONHelper.GetStringDef(evt["PayeeFaction"]);

        }
        public string PayeeFaction { get; set; }
        public long Reward { get; set; }
        public string VictimFaction { get; set; }

        public void LedgerNC(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, PayeeFaction + " " + Reward.ToString("N0"));
        }
    }
}
