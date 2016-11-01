using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when scanning a data link
    //Parameters:
    //•	Message: message from data link
    public class JournalDatalinkScan : JournalEntry
    {
        public JournalDatalinkScan(JObject evt ) : base(evt, JournalTypeEnum.DatalinkScan)
        {
            Message = JSONHelper.GetStringDef(evt["Message"]);

        }
        public string Message { get; set; }

    }
}
