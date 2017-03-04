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
        long id { get; set; }
        string name { get; set; }
        string faction { get; set; }
        List<CCommodities> commodities;

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
