using EDDiscovery.EliteDangerous.JournalEvents;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.CompanionAPI
{
    public class CLastStarport
    {
        public long id { get; private set; }
        public string name { get; private set; }
        public string faction { get; private set; }
        public JArray jcommodities { get; private set; }
        public List<CCommodities> commodities;

        public CLastStarport(JObject jo)
        {
            FromJson(jo);
        }

        public bool FromJson(JObject jo)
        {
            try
            {
                id = JSONHelper.GetLong(jo["id"]);
                name = JSONHelper.GetStringDef(jo["name"]);
                faction = JSONHelper.GetStringDef(jo["faction"]);

                jcommodities = (JArray)jo["commodities"];

                if (jcommodities != null)
                {
                    commodities = new List<CCommodities>();
                    foreach (JObject commodity in jcommodities)
                    {
                        CCommodities com = new CCommodities(commodity);
                        commodities.Add(com);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
