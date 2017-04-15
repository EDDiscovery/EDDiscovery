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
        private JObject _jo;
        public long id { get; set; }
        public string name { get; set; }
        public string faction { get; set; }
        public List<CCommodities> commodities;

        public CLastStarport(JObject jo)
        {
            FromJson(jo);
        }

        public bool FromJson(JObject jo)
        {
            try
            {
                _jo = jo;
                id = JSONHelper.GetLong(jo["id"]);
                name = JSONHelper.GetStringDef(jo["name"]);
                faction = JSONHelper.GetStringDef(jo["faction"]);

                JArray jcommodities = (JArray)jo["commodities"];


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
