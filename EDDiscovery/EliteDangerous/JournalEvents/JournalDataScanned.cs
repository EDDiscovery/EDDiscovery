using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when scanning a data link
    //Parameters:
    //•	Message: message from data link
    public class JournalDataScanned : JournalEntry
    {
        public JournalDataScanned(JObject evt) : base(evt, JournalTypeEnum.DataScanned)
        {
            Type = JSONHelper.GetStringDef(evt["Type"]);

        }
        public string Type { get; set; }

    }
}
