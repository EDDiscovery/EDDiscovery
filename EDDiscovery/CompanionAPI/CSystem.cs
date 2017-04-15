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
        private JObject _jo;
        public long id { get; set; }
        public string name { get; set; }
        public string faction { get; set; }

        public CLastSystem(JObject jo)
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
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
