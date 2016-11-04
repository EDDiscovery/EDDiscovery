using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when dropping from Supercruise at a USS
    //Parameters:
    //•	USSType: description of USS
    //•	USSThreat: threat level
    public class JournalUSSDrop : JournalEntry
    {
        public JournalUSSDrop(JObject evt ) : base(evt, JournalTypeEnum.USSDrop)
        {
            USSType = JSONHelper.GetStringDef(evt["USSType"]);
            USSTypeLocalised = JSONHelper.GetStringDef(evt["USSType_Localised"]);
            USSThreat = JSONHelper.GetInt(evt["USSThreat"]);
        }
        public string USSType { get; set; }
        public int USSThreat { get; set; }
        public string USSTypeLocalised { get; set; }
    }
}
