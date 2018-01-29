using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace EliteDangerousCore
{
    public class CCommodities
    {
        public int id { get; private set; }
        public string name { get; private set; }            // NAME as given by the CAPI, which is normal text, not fdname, but is not perfect (some conjoining)
        public int buyPrice { get; private set; }
        public int sellPrice { get; private set; }
        public int meanPrice { get; private set; }
        public int demandBracket { get; private set; }
        public int stockBracket { get; private set; }
        public int stock { get; private set; }
        public int demand { get; private set; }
        public string type { get; private set; }            // in this context, it means, its type (Metals).. as per MaterialCommoditiesDB
        public List<string> StatusFlags { get; private set; }

        public string categoryname { get; private set; }
        public string locName { get; private set; }


        public string ComparisionLR { get; private set; }       // NOT in Frontier data, used for market data UC during merge
        public string ComparisionRL { get; private set; }       // NOT in Frontier data, used for market data UC during merge
        public bool ComparisionRightOnly { get; private set; }  // NOT in Frontier data, Exists in right data only
        public bool ComparisionBuy { get; private set; }        // NOT in Frontier data, its for sale at either left or right
        public int CargoCarried { get; set; }                  // NOT in Frontier data, cargo currently carried for this item

        public CCommodities(JObject jo, bool market = false)
        {
            if ( market )
                FromJsonMarket(jo);
            else
                FromJsonCAPI(jo);
        }

        public CCommodities(CCommodities other)             // main fields copied, not the extra data ones
        {
            id = other.id; name = other.name; buyPrice = other.buyPrice; sellPrice = other.sellPrice; meanPrice = other.meanPrice;
            demandBracket = other.demandBracket; stockBracket = other.stockBracket; stock = other.stock; demand = other.demand;
            type = other.type;
            StatusFlags = new List<string>(other.StatusFlags);
            ComparisionLR = ComparisionRL = "";
        }

        public bool FromJsonCAPI(JObject jo)
        {
            try
            {
                id = jo["id"].Int();
                name = jo["name"].Str();
                locName = jo["locName"].Str();

                buyPrice = jo["buyPrice"].Int();
                sellPrice = jo["sellPrice"].Int();
                meanPrice = jo["meanPrice"].Int();
                demandBracket = jo["demandBracket"].Int();
                stockBracket = jo["stockBracket"].Int();
                stock = jo["stock"].Int();
                demand = jo["demand"].Int();
                type = jo["categoryname"].Str();

                List<string> StatusFlags = new List<string>();
                foreach (dynamic statusFlag in jo["statusFlags"])
                {
                    StatusFlags.Add((string)statusFlag);
                }
                this.StatusFlags = StatusFlags;

                ComparisionLR = ComparisionRL = "";
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool FromJsonMarket(JObject jo)
        {
            try
            {
                id = jo["id"].Int();
                name = jo["Name"].Str();
                locName = name;

                buyPrice = jo["BuyPrice"].Int();
                sellPrice = jo["SellPrice"].Int();
                meanPrice = jo["MeanPrice"].Int();
                demandBracket = jo["DemandBracket"].Int();
                stockBracket = jo["StockBracket"].Int();
                stock = jo["Stock"].Int();
                demand = jo["Demand"].Int();
                type = "CAT?";

                List<string> StatusFlags = new List<string>();

                if (jo["Consumer"].Bool())
                    StatusFlags.Add("Consumer");

                if (jo["Producer"].Bool())
                    StatusFlags.Add("Producer");

                if (jo["Rare"].Bool())
                    StatusFlags.Add("Rare");

                this.StatusFlags = StatusFlags;

                ComparisionLR = ComparisionRL = "";
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} : {1} Buy {2} Sell {3} Mean {4}" + System.Environment.NewLine +
                                 "Stock {5} Demand {6}", type, name, buyPrice, sellPrice, meanPrice, stock, demand);
        }

        public static void Sort(List<CCommodities> list)
        {
            list.Sort(delegate (CCommodities left, CCommodities right)
            {
                int cat = left.type.CompareTo(right.type);
                if (cat == 0)
                    cat = left.name.CompareTo(right.name);
                return cat;
            });
        }

        public static List<CCommodities> Merge(List<CCommodities> left, List<CCommodities> right, string otherstation)
        {
            List<CCommodities> merged = new List<CCommodities>();

            foreach (CCommodities l in left)
            {
                CCommodities m = new CCommodities(l);
                CCommodities r = right.Find(x => x.name == l.name);
                if (r != null)
                {
                    if (l.buyPrice > 0)     // if we can buy it..
                    {
                        m.ComparisionLR = (r.sellPrice - l.buyPrice).ToString();
                        m.ComparisionBuy = true;
                    }

                    if (r.buyPrice > 0)
                    {
                        m.ComparisionRL = (l.sellPrice - r.buyPrice).ToString();
                        m.ComparisionBuy = true;
                    }
                }
                else
                {                                   // not found in right..
                    if (l.buyPrice > 0)            // if we can buy it here, note you can't price it in right
                        m.ComparisionLR = "No Price";
                }

                merged.Add(m);
            }

            foreach (CCommodities r in right)
            {
                CCommodities m = merged.Find(x => x.name == r.name);        // see if any in right we have not merged
                if (m == null)
                {
                    m = new CCommodities(r);
                    m.name = m.name + " at " + otherstation;
                    m.ComparisionRightOnly = true;

                    if (r.buyPrice > 0)                             // if we can buy it here, note you can't price it in left
                    {
                        m.ComparisionBuy = true;
                        m.ComparisionRL = "No price";
                    }
                    merged.Add(m);
                }
            }

            Sort(merged);
            return merged;
        }

    }
}
