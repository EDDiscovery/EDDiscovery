using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //    When written: The player has been rewarded for a capital ship combat
    //Parameters:
    //•	Reward: value of award
    //•	AwardingFaction
    //•	VictimFaction

    public class JournalCapShipBond : JournalEntry
    {
        public JournalCapShipBond(JObject evt) : base(evt, JournalTypeEnum.CapShipBond)
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
            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, AwardingFaction +" " + Reward);
        }

    }
}
