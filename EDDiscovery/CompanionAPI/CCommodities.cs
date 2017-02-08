using Newtonsoft.Json.Linq;

namespace EDDiscovery.CompanionAPI
{
    internal class CCommodities
    {
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


        public CCommodities(JObject jo)
        {
            FromJson(jo);

        }


        public bool FromJson(JObject jo)
        {
            try
            {
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

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}