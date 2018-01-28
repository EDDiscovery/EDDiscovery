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
    public class JournalMarket : JournalEntry
    {
        public JournalMarket(JObject evt) : base(evt, JournalTypeEnum.Market)
        {
            StationName = evt["StationName"].Str();
            StarSystem = evt["StarSystem"].Str();
            MarketID = evt["MarketID"].Long();

            MarketItems = evt["Items"]?.ToObject<MarketItem[]>();

            if ( MarketItems != null )
            {
                foreach (MarketItem i in MarketItems)
                {
                    i.Name = JournalFieldNaming.GetBetterItemNameEvents(i.Name);
                }
            }
        }

        public string StationName { get; set; }
        public string StarSystem { get; set; }
        public long MarketID { get; set; }

        public MarketItem[] MarketItems { get; set; }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = "";

            if ( MarketItems != null )
                foreach (MarketItem m in MarketItems)
                {
                    if (info.Length>0)
                        info += ", ";
                    info += m.Name;
                }
                
            detailed = "";
        }
    }


    public class MarketItem
    {
        public long id;
        public string Name;
        public string Name_Localised;
        public int BuyPrice;
        public int SellPrice;
        public int MeanPrice;
        public int StockBracket;
        public int DemandBracket;
        public int Stock;
        public int Demand;
        public bool Consumer;
        public bool Producer;
        public bool Rare;
    }

}
