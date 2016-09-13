using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    // For "new" unknown event EDD  not knows. 
    public class JournalUnknown : JournalEntry
    {
        public JournalUnknown(JObject jo, string typestr, EDJournalReader reader) : base(jo, JournalTypeEnum.Unknown, reader)
        {
            eventTypeStr = typestr;
        }
    }

}
