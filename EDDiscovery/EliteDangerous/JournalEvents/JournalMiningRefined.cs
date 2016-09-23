using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when mining fragments are converted unto a unit of cargo by refinery
    //Parameters:
    //•	Type: cargo type
    public class JournalMiningRefined : JournalEntry
    {
        public JournalMiningRefined(JObject evt ) : base(evt, JournalTypeEnum.MiningRefined)
        {
            Type = Tools.GetStringDef(evt["Type"]);

        }
        public string Type { get; set; }

    }
}
