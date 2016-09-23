using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when scooping cargo from space or planet surface
    //Parameters:
    //•	Type: cargo type
    //•	Stolen: whether stolen goods
    public class JournalCollectCargo : JournalEntry
    {
        public JournalCollectCargo(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.CollectCargo, reader)
        {
            Type = Tools.GetStringDef("Type");
            Stolen = Tools.GetBool("Stolen");
        }
        public string Type { get; set; }
        public bool Stolen { get; set; }
    }
}
