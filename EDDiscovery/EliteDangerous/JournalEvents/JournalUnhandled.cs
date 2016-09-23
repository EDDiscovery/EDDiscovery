using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    // For Events we down have own classes for yet.
    public class JournalUnhandled : JournalEntry
    {
        public JournalUnhandled(JObject jo, string journaltypestr ) : base(jo, JournalUnhandled.JournalString2Type(journaltypestr))
        {
        }
    }
}
