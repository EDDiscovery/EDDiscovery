/*
 * Copyright © 2016 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 *
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: by EDD when a user manually sets an item count (material or commodity)
    [JournalEntryType(JournalTypeEnum.EDDCommodityPrices)]
    public class JournalEDDCommodityPrices : JournalEntry
    {
        public JournalEDDCommodityPrices(JObject evt) : base(evt, JournalTypeEnum.EDDCommodityPrices)
        {
            Station = evt["station"].Str();
            Faction = evt["faction"].Str();
            Commodities = new List<CCommodities>();

            JArray jcommodities=null;
            if (!JSONHelpers.IsNullOrEmptyT(evt["commodities"]))
                jcommodities = (JArray)evt["commodities"];

            if (jcommodities != null)
            {
                foreach (JObject commodity in jcommodities)
                {
                    CCommodities com = new CCommodities(commodity);
                    Commodities.Add(com);
                }

                CCommodities.Sort(Commodities);

            }
        }

        public string Station { get; private set; }
        public string Faction { get; private set; }
        public List<CCommodities> Commodities { get; private set; }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.commodities; } }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = "Prices on " + Commodities.Count + " items";

            detailed = "Items to buy:" + System.Environment.NewLine;
            foreach (CCommodities c in Commodities)
            {
                if (c.buyPrice > 0)
                {
                    if (c.sellPrice > 0)
                        detailed += string.Format("{0}: {1} sell {2} Diff {3} {4}%" + System.Environment.NewLine, c.name, c.buyPrice, c.sellPrice , c.buyPrice - c.sellPrice , ((double)(c.buyPrice-c.sellPrice)/(double)c.sellPrice * 100.0).ToString("0.#"));
                    else
                        detailed += string.Format("{0}: {1}" + System.Environment.NewLine, c.name, c.buyPrice);
                }
            }

            detailed += "Sell only Items:" + System.Environment.NewLine;
            foreach (CCommodities c in Commodities)
            {
                if (c.buyPrice <= 0)
                {
                    detailed += string.Format("{0}: {1}" + System.Environment.NewLine, c.name, c.sellPrice);
                }
            }
        }
    }

    public class CCommodities
    {
        public int id { get; private set; }
        public string name { get; private set; }
        public int buyPrice { get; private set; }
        public int sellPrice { get; private set; }
        public int meanPrice { get; private set; }
        public int demandBracket { get; private set; }
        public int stockBracket { get; private set; }
        public int stock { get; private set; }
        public int demand { get; private set; }
        public string categoryname { get; private set; }
        public List<string> StatusFlags { get; private set; }

        public string ComparisionLR { get; private set; }       // NOT in Frontier data, used for market data UC during merge
        public string ComparisionRL { get; private set; }       // NOT in Frontier data, used for market data UC during merge
        public bool ComparisionRightOnly { get; private set; }  // NOT in Frontier data, Exists in right data only
        public bool ComparisionBuy { get; private set; }        // NOT in Frontier data, You can buy one or both sets

        public CCommodities(JObject jo)
        {
            FromJson(jo);
        }

        public CCommodities(CCommodities other)             // main fields copied, not the extra data ones
        {
            id = other.id; name = other.name; buyPrice = other.buyPrice; sellPrice = other.sellPrice; meanPrice = other.meanPrice;
            demandBracket = other.demandBracket; stockBracket = other.stockBracket; stock = other.stock; demand = other.demand;
            categoryname = other.categoryname;
            StatusFlags = new List<string>(other.StatusFlags);
            ComparisionLR = ComparisionRL = "";
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

        public override string ToString()
        {
            return string.Format("{0} : {1} Buy {2} Sell {3} Mean {4}" + System.Environment.NewLine +
                                 "Stock {5} Demand {6}", categoryname, name, buyPrice, sellPrice, meanPrice, stock, demand);
        }

        public static void Sort(List<CCommodities> list)
        {
            list.Sort(delegate (CCommodities left, CCommodities right)
            {
                int cat = left.categoryname.CompareTo(right.categoryname);
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
                    if ( l.buyPrice > 0)            // if we can buy it here, note you can't price it in right
                        m.ComparisionLR = "No Price";
                }

                merged.Add(m);
            }

            foreach (CCommodities r in right)
            {
                CCommodities m = merged.Find(x => x.name == r.name);        // see if any in right we have not merged
                if ( m == null )
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


