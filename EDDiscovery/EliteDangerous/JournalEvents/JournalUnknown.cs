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
        public JournalUnknown(JObject jo, string typestr ) : base(jo, JournalTypeEnum.Unknown)
        {
            EventTypeStr = typestr;
        }
    }

}
