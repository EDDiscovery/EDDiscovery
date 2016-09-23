
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when purchasing ammunition
    //Parameters:
    //•	Cost
    public class JournalBuyAmmo : JournalEntry
    {
        public JournalBuyAmmo(JObject evt ) : base(evt, JournalTypeEnum.BuyAmmo)
        {
            Cost = Tools.GetInt(evt["Cost"]);

        }
        public int Cost { get; set; }

    }
}
