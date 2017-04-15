using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace EDDiscovery.CompanionAPI
{
    public class CCommodities
    {
        private JObject _jo;
        public int id { get; set; }
        public string name { get; set; }
        public int buyPrice { get; set; }
        public int sellPrice { get; set; }
        public int meanPrice { get; set; }
        public int demandBracket { get; set; }
        public int stockBracket { get; set; }
        public int stock { get; set; }
        public int demand { get; set; }
        public string categoryname { get; set; }


        public int? avgprice { get; set; }
        public bool? rare { get; set; }

        public List<string> StatusFlags { get; set; }

        public CCommodities(JObject jo)
        {
            FromJson(jo);

        }


        public bool FromJson(JObject jo)
        {
            try
            {
                _jo = jo;
                id = JSONHelper.GetInt(jo["id"]);
                name = JSONHelper.GetStringDef(jo["name"]);
                buyPrice = JSONHelper.GetInt(jo["buyPrice"]);
                sellPrice = JSONHelper.GetInt(jo["sellPrice"]);
                meanPrice = JSONHelper.GetInt(jo["meanPrice"]);
                demandBracket = JSONHelper.GetInt(jo["demandBracket"]);
                stockBracket = JSONHelper.GetInt(jo["stockBracket"]);
                stock = JSONHelper.GetInt(jo["stock"]);
                demand = JSONHelper.GetInt(jo["demand"]);
                categoryname = JSONHelper.GetStringDef(jo["categoryname"]);

                List<string> StatusFlags = new List<string>();
                foreach (dynamic statusFlag in jo["statusFlags"])
                {
                    StatusFlags.Add((string)statusFlag);
                }
                this.StatusFlags = StatusFlags;

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}