using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    public abstract class JournalLocOrJump : JournalEntry
    {
        public class Coords
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
        }

        public string StarSystem { get; set; }
        public Coords StarPos { get; set; }

        protected JournalLocOrJump(JObject jo, JournalTypeEnum jtype) : base(jo, jtype)
        {
            StarSystem = jo.Value<string>("StarSystem");
            JArray coords = jo["StarPos"] as JArray;
            StarPos.X = coords[0].Value<double>();
            StarPos.Y = coords[1].Value<double>();
            StarPos.Z = coords[2].Value<double>();
        }
    }
}
