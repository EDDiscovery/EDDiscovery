using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when scanning a data link
    //Parameters:
    //•	Message: message from data link
    public class JournalDatalinkScan : JournalEntry
    {
        public JournalDatalinkScan(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.DatalinkScan, reader)
        {
            Message = Tools.GetStringDef("Message");

        }
        public string Message { get; set; }

    }
}
