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

namespace EliteDangerousCore.JournalEvents
{
    //    When written: when opening commodities market
    //Parameters:
    //•	MarketID
    //• StationName
    //• StarSystem
    //•	Items: Array of records
    //o   id
    //o   Name
    //o   Name_Localised
    //o   BuyPrice
    //o   SellPrice
    //o   MeanPrice
    //o   StockBracket
    //o   DemandBracket
    //o   Stock
    //o   Demand
    //o   Consumer
    //o   Producer
    //o   Rare

    [JournalEntryType(JournalTypeEnum.Market)]
    public class JournalMarket : JournalCommodityPricesBase
    {
        public JournalMarket(JObject evt) : base(evt, JournalTypeEnum.Market)
        {
            Station = evt["StationName"].Str();
            StarSystem = evt["StarSystem"].Str();
            MarketID = evt["MarketID"].LongNull();
            Commodities = new List<CCommodities>(); // always made..

            JArray jcommodities = (JArray)evt["Items"];
            if (jcommodities != null )
            {
                foreach (JObject commodity in jcommodities)
                {
                    CCommodities com = new CCommodities(commodity, true);
                    Commodities.Add(com);
                }
            }
        }
    }
}
