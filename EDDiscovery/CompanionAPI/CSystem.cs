using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.CompanionAPI
{
    public class CLastSystem
    {
        public long id { get; private set; }
        public string name { get; private set; }
        public string faction { get; private set; }

        public CLastSystem(JObject jo)
        {
            FromJson(jo);
        }

        public bool FromJson(JObject jo)
        {
            try
            {
                id = jo["id"].Long();
                name = jo["name"].Str();
                faction = jo["faction"].Str();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
