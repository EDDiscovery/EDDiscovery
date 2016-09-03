using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    public class JournalLocation : JournalLocOrJump
    {
        public JournalLocation(JObject jo) : base(jo, JournalTypeEnum.Location)
        {
        }
    }
}
