using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace EliteDangerousCore
{
    public class CCommodities : System.IEquatable<CCommodities>
    {
        public int id { get; private set; }

        public string fdname { get; private set; }            // EDDN use : name is lower cased in CAPI but thats all to match Marketing use of it
        public string locName { get; private set; }
        public string category { get; private set; }                // in this context, it means, its type (Metals).. as per MaterialCommoditiesDB
        public string loccategory { get; private set; }       // in this context, it means, its type (Metals).. as per MaterialCommoditiesDB

        public int buyPrice { get; private set; }
        public int sellPrice { get; private set; }
        public int meanPrice { get; private set; }
        public int demandBracket { get; private set; }
        public int stockBracket { get; private set; }
        public int stock { get; private set; }
        public int demand { get; private set; }

        public List<string> StatusFlags { get; private set; }

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
            id = other.id;

            fdname = other.fdname;
            locName = other.locName;
            category = other.category;
            loccategory = other.loccategory;

            buyPrice = other.buyPrice;
            sellPrice = other.sellPrice;
            meanPrice = other.meanPrice;
            demandBracket = other.demandBracket;
            stockBracket = other.stockBracket;
            stock = other.stock;
            demand = other.demand;

            StatusFlags = new List<string>(other.StatusFlags);

            ComparisionLR = ComparisionRL = "";
        }

        public bool Equals(CCommodities other)
        {
            return (id == other.id && string.Compare(fdname, other.fdname) == 0 && string.Compare(locName, other.locName) == 0 &&
                     string.Compare(category, other.category) == 0 && string.Compare(loccategory, other.loccategory) == 0 &&
                     buyPrice == other.buyPrice && sellPrice == other.sellPrice && meanPrice == other.meanPrice &&
                     demandBracket == other.demandBracket && stockBracket == other.stockBracket && stock == other.stock && demand == other.demand);
        }

        public bool FromJsonCAPI(JObject jo)
        {
            try
            {
                id = jo["id"].Int();
                fdname = jo["name"].Str().ToLowerInvariant();
                locName = jo["locName"].Str();
                loccategory = category = jo["categoryname"].Str();
                category = "$MARKET_category_" + category.ToLowerInvariant().Replace(" ","_").Replace("narcotics","drugs");
               // System.Diagnostics.Debug.WriteLine("CAPI field fd:'{0}' loc:'{1}' of type '{2}' '{3}'", fdname, locName, category , loccategory);
                locName = locName.Alt(fdname.SplitCapsWord());      // use locname, if not there, make best loc name possible

                buyPrice = jo["buyPrice"].Int();
                sellPrice = jo["sellPrice"].Int();
                meanPrice = jo["meanPrice"].Int();
                demandBracket = jo["demandBracket"].Int();
                stockBracket = jo["stockBracket"].Int();
                stock = jo["stock"].Int();
                demand = jo["demand"].Int();


                List<string> StatusFlags = new List<string>();
                foreach (string statusFlag in jo["statusFlags"])
                {
                    StatusFlags.Add(statusFlag);
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
                fdname = JournalFieldNaming.FixCommodityName(jo["Name"].Str());
                locName = jo["Name_Localised"].Str();
                if (locName.IsEmpty())
                    locName = fdname.SplitCapsWordFull();

                loccategory = jo["Category_Localised"].Str();
                category = jo["Category"].Str();

                buyPrice = jo["BuyPrice"].Int();
                sellPrice = jo["SellPrice"].Int();
                meanPrice = jo["MeanPrice"].Int();
                demandBracket = jo["DemandBracket"].Int();
                stockBracket = jo["StockBracket"].Int();
                stock = jo["Stock"].Int();
                demand = jo["Demand"].Int();

                List<string> StatusFlags = new List<string>();

                if (jo["Consumer"].Bool())
                    StatusFlags.Add("Consumer");

                if (jo["Producer"].Bool())
                    StatusFlags.Add("Producer");

                if (jo["Rare"].Bool())
                    StatusFlags.Add("Rare");

                this.StatusFlags = StatusFlags;
                //System.Diagnostics.Debug.WriteLine("Market field fd:'{0}' loc:'{1}' of type '{2}' '{3}'", fdname, locName, category, loccategory);

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
                                 "Stock {5} Demand {6}", loccategory, locName, buyPrice, sellPrice, meanPrice, stock, demand);
        }

        public static void Sort(List<CCommodities> list)
        {
            list.Sort(delegate (CCommodities left, CCommodities right)
            {
                int cat = left.category.CompareTo(right.category);
                if (cat == 0)
                    cat = left.fdname.CompareTo(right.fdname);
                return cat;
            });
        }

        public static List<CCommodities> Merge(List<CCommodities> left, List<CCommodities> right)
        {
            List<CCommodities> merged = new List<CCommodities>();

            foreach (CCommodities l in left)
            {
                CCommodities m = new CCommodities(l);
                CCommodities r = right.Find(x => x.fdname == l.fdname);
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
                CCommodities m = merged.Find(x => x.fdname == r.fdname);        // see if any in right we have not merged

                if (m == null)  // not in left list,add
                {
                    m = new CCommodities(r);
                    m.ComparisionRightOnly = true;

                    if (r.buyPrice > 0)                             // if we can buy it there, but its not in the left list
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
