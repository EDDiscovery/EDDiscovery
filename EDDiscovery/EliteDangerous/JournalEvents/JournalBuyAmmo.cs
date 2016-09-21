
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when purchasing ammunition
    //Parameters:
    //•	Cost
    public class JournalBuyAmmo : JournalEntry
    {
        public JournalBuyAmmo(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.BuyAmmo, reader)
        {
            Cost = evt.Value<int>("Cost");

        }
        public int Cost { get; set; }

    }
}
