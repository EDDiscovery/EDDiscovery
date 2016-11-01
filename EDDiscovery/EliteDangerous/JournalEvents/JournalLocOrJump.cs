using Newtonsoft.Json.Linq;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    public abstract class JournalLocOrJump : JournalEntry
    {
        public string StarSystem { get; set; }
        public Vector3 StarPos { get; set; }
        public bool StarPosFromEDSM { get; set; }

        public bool HasCoordinate { get { return !float.IsNaN(StarPos.X); } }

        protected JournalLocOrJump(JObject jo, JournalTypeEnum jtype ) : base(jo, jtype)
        {
            StarSystem = JSONHelper.GetStringDef(jo["StarSystem"],"Unknown!");
            StarPosFromEDSM = JSONHelper.GetBool(jo["StarPosFromEDSM"], false);

            Vector3 pos = new Vector3();

            if (!JSONHelper.IsNullOrEmptyT(jo["StarPos"]))            // if its an old VS entry, may not have co-ords
            {
                JArray coords = jo["StarPos"] as JArray;
                pos.X = coords[0].Value<float>();
                pos.Y = coords[1].Value<float>();
                pos.Z = coords[2].Value<float>();
            }
            else
            {
                pos.X = pos.Y = pos.Z = float.NaN;
            }

            StarPos = pos;
        }
    }
}
