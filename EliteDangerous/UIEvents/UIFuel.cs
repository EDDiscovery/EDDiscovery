/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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

namespace EliteDangerousCore.UIEvents
{
    public class UIFuel : UIEvent
    {
        public UIFuel(double value, double res, UIShipType.Shiptype shiptype, DateTime time, bool refresh) : base(UITypeEnum.Fuel, time, refresh)
        {
            Fuel = value;
            FuelRes = res;
            ShipType = shiptype;
        }

        public double Fuel { get; private set; }     // level
        public double FuelRes { get; private set; }     // level 3.3.2
        public UIShipType.Shiptype ShipType { get; private set; }   // Ship type flags.. per flags
    }
}
