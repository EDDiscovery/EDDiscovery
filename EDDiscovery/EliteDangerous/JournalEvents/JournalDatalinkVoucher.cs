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
            VictimFaction = Tools.GetStringDef(evt["VictimFaction"]);
            Reward = Tools.GetLong(evt["Reward"]);
            PayeeFaction = Tools.GetStringDef(evt["PayeeFaction"]);

        }
        public string PayeeFaction { get; set; }
        public long Reward { get; set; }
        public string VictimFaction { get; set; }



    }


}
