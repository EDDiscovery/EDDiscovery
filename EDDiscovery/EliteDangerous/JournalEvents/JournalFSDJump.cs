using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    public class JournalFSDJump : JournalLocOrJump
    {
        public JournalFSDJump(JObject jo) : base(jo, JournalTypeEnum.FSDJump)
        {
        }
    }
}
