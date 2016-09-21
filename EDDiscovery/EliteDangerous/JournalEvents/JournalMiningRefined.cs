using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when mining fragments are converted unto a unit of cargo by refinery
    //Parameters:
    //•	Type: cargo type
    public class JournalMiningRefined : JournalEntry
    {
        public JournalMiningRefined(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.MiningRefined, reader)
        {
            Type = evt.Value<string>("Type");

        }
        public string Type { get; set; }

    }
}
