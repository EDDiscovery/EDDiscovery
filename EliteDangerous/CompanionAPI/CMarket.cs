using EliteDangerousCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteDangerous.CompanionAPI
{
    /// <summary>
    /// Market information returned by the companion app service
    /// </summary>
    public class CMarket
    {
        public long id;
        public string name;
        public string outpostType;

        // imported list with commodities...
        // exportedlist with commodities...

        // services

        // economies
        // prohibited


        public JArray jcommodities { get; private set; }
        public List<EliteDangerousCore.CCommodities> commodities;

        public bool FromJson(JObject jo)
        {
            try
            {
                id = jo["id"].Long();
                name = jo["name"].Str();
                outpostType = jo["outpostType"].Str();


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
