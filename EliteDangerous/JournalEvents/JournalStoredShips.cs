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
    //• ShipsHere: Array of records
    //o   ShipID
    //o   ShipType
    //o   Name
    //o   Value
    //•	ShipsRemote: Array of records
    //o   ShipID
    //o   ShipType
    //o   StarSystem
    //o   ShipMarketID
    //o   TransferPrice
    //o   TransferTime
    //o   Value

    [JournalEntryType(JournalTypeEnum.StoredShips)]
    public class JournalStoredShips : JournalEntry
    {
        public JournalStoredShips(JObject evt) : base(evt, JournalTypeEnum.StoredShips)
        {
            StationName = evt["StationName"].Str();
            StarSystem = evt["StarSystem"].Str();
            MarketID = evt["MarketID"].LongNull();

            ShipsHere = evt["ShipsHere"]?.ToObject<StoredShipItem[]>();
            ShipsRemote = evt["ShipsRemote"]?.ToObject<StoredShipItem[]>();

            if (ShipsHere != null)
            {
                foreach (StoredShipItem i in ShipsHere)
                {
                    i.ShipType = JournalFieldNaming.GetBetterShipName(i.ShipType);
                }
            }

            if (ShipsRemote != null)
            {
                foreach (StoredShipItem i in ShipsRemote)
                {
                    i.ShipType = JournalFieldNaming.GetBetterShipName(i.ShipType);
                }
            }
        }

        public string StationName { get; set; }
        public string StarSystem { get; set; }
        public long? MarketID { get; set; }

        public StoredShipItem[] ShipsHere { get; set; }
        public StoredShipItem[] ShipsRemote { get; set; }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = "";
            detailed = "";
        }
    }


    public class StoredShipItem
    {
        public int ShipID;
        public string ShipType;
        public string Name;
        public string StarSystem;
        public long ShipMarketID;
        public long TransferPrice;
        public int TransferTime;
        public long Value;
    }

}
