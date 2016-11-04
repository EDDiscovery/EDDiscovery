
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: Player rewarded for taking part in a combat zone
    //Parameters:
    //•	Reward
    //•	AwardingFaction
    //•	VictimFaction
    public class JournalHeatDamage : JournalEntry
    {
        public JournalHeatDamage(JObject evt ) : base(evt, JournalTypeEnum.HeatDamage)
        {
        }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.heatdamage; } }

    }
}
