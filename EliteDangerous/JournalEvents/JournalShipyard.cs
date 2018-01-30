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
    //    When written: when opening shipyard
    //Parameters:
    //•	MarketID
    //• StationName
    //• StarSystem
    //•	PriceList: Array of records
    //o   id
    //o   Name
    //o   ShipPrice

    [JournalEntryType(JournalTypeEnum.Shipyard)]
    public class JournalShipyard : JournalEntry
    {
        public JournalShipyard(JObject evt) : base(evt, JournalTypeEnum.Shipyard)
        {
            StationName = evt["StationName"].Str();
            StarSystem = evt["StarSystem"].Str();
            MarketID = evt["MarketID"].LongNull();

            ShipyardItems = evt["PriceList"]?.ToObject<ShipyardItem[]>();

            if ( ShipyardItems != null )
            {
                foreach (ShipyardItem i in ShipyardItems)
                {
                    i.ShipType = JournalFieldNaming.GetBetterShipName(i.ShipType);
                }
            }
        }

        public string StationName { get; set; }
        public string StarSystem { get; set; }
        public long? MarketID { get; set; }

        public ShipyardItem[] ShipyardItems { get; set; }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = "";

            if ( ShipyardItems != null )
                foreach (ShipyardItem m in ShipyardItems)
                {
                    if (info.Length>0)
                        info += ", ";
                    info += m.ShipType;
                }
                
            detailed = "";
        }
    }


    public class ShipyardItem
    {
        public long id;
        public string ShipType;
        public string ShipType_Localised;
        public long ShipPrice;
    }

}
