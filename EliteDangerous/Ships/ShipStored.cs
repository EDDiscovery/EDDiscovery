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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteDangerousCore
{
    public class StoredShipInformation : IEquatable<StoredShipInformation>
    {
        public int ShipID;      // both
        public string ShipType; // both
        public string ShipType_Localised; // both
        public string Name;     // Both
        public long Value;      // both
        public bool Hot;        // both

        public string StarSystem;   // remote only and when not in transit, but filled in for local
        public long ShipMarketID;   //remote
        public long TransferPrice;  //remote
        public long TransferTime;   //remote
        public bool InTransit;      //remote, and that means StarSystem is not there.

        public string StationName;  // local only, null otherwise, not compared due to it being computed
        public string ShipTypeFD; // both, computed
        public System.TimeSpan TransferTimeSpan;        // computed
        public string TransferTimeString; // computed

        public void Normalise()
        {
            ShipTypeFD = JournalFieldNaming.NormaliseFDShipName(ShipType);
            ShipType = JournalFieldNaming.GetBetterShipName(ShipTypeFD);
            ShipType_Localised = ShipType_Localised.Alt(ShipType);
            TransferTimeSpan = new System.TimeSpan((int)(TransferTime / 60 / 60), (int)((TransferTime / 60) % 60), (int)(TransferTime % 60));
            TransferTimeString = TransferTimeSpan.ToString();
        }

        public bool Equals(StoredShipInformation other)
        {
            return ShipID == other.ShipID && string.Compare(ShipType, other.ShipType) == 0 &&
                        string.Compare(ShipType_Localised, other.ShipType_Localised) == 0 && string.Compare(Name, other.Name) == 0 &&
                        Value == other.Value && Hot == other.Hot &&
                        string.Compare(StarSystem, other.StarSystem) == 0 && ShipMarketID == other.ShipMarketID && TransferPrice == other.TransferPrice &&
                        InTransit == other.InTransit;
        }
    }
}
