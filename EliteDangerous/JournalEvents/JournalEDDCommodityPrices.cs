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
using EliteDangerousCore;

namespace EliteDangerousCore.JournalEvents
{
    //When written: by EDD when a user manually sets an item count (material or commodity)
    [JournalEntryType(JournalTypeEnum.EDDCommodityPrices)]
    public class JournalEDDCommodityPrices : JournalCommodityPricesBase
    {
        public JournalEDDCommodityPrices(JObject evt) : base(evt, JournalTypeEnum.EDDCommodityPrices)
        {
            Station = evt["station"].Str();
            Faction = evt["faction"].Str();
            Commodities = new List<CCommodities>();

            JArray jcommodities = null;
            if (!evt["commodities"].Empty())
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
    }

    public class JournalCommodityPricesBase : JournalEntry
    {
        public JournalCommodityPricesBase(JObject evt, JournalTypeEnum en) : base(evt,en)
        {
        }

        public string Station { get; protected set; }
        public string Faction { get; protected set; }
        public string StarSystem { get; set; }
        public long? MarketID { get; set; }
        public List<CCommodities> Commodities { get; protected set; }   // never null

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("Prices on ; items", Commodities.Count, "< at " , Station , "< in " , StarSystem);

            int col = 0;
            int maxcol = Commodities.Count > 60 ? 2 : 1;
            detailed = "Items to buy:" + System.Environment.NewLine;
            foreach (CCommodities c in Commodities)
            {
                if (c.buyPrice > 0)
                {
                    if (c.sellPrice > 0)
                        detailed += string.Format("{0}: {1} sell {2} Diff {3} {4}%  ", c.name, c.buyPrice, c.sellPrice, c.buyPrice - c.sellPrice, ((double)(c.buyPrice - c.sellPrice) / (double)c.sellPrice * 100.0).ToString("0.#"));
                    else
                        detailed += string.Format("{0}: {1}  ", c.name, c.buyPrice);

                    if (++col == maxcol)
                    {
                        detailed += System.Environment.NewLine;
                        col = 0;
                    }
                }
            }

            if (col == maxcol - 1)
                detailed += System.Environment.NewLine;

            col = 0;
            detailed += "Sell only Items:" + System.Environment.NewLine;
            foreach (CCommodities c in Commodities)
            {
                if (c.buyPrice <= 0)
                {
                    detailed += string.Format("{0}: {1}  ", c.name, c.sellPrice);
                    if (++col == maxcol)
                    {
                        detailed += System.Environment.NewLine;
                        col = 0;
                    }
                }
            }
        }

    }

}


