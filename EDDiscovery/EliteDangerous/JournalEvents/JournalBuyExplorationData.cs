
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when buying system data via the galaxy map
    //Parameters: 
    //•	System
    //•	Cost
    public class JournalBuyExplorationData : JournalEntry
    {
        public JournalBuyExplorationData(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.BuyExplorationData, reader)
        {
            System = evt.Value<string>("System");
            Cost = evt.Value<int>("Cost");

        }
        public string System { get; set; }
        public int Cost { get; set; }

    }
}
