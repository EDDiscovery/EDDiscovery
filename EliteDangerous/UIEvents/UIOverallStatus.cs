/*
 * Copyright © 2016 - 2019 EDDiscovery development team
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

namespace EliteDangerousCore.UIEvents
{
    public class UIOverallStatus : UIEvent
    {
        public UIOverallStatus(UIEvents.UIShipType.Shiptype st, List<UITypeEnum> list, int focus, UIEvents.UIPips.Pips pips, int fg, double fuel, double res, int cargo,
            UIEvents.UIPosition.Position pos, double heading,
            DateTime time, bool refresh): base(UITypeEnum.OverallStatus, time, refresh)
        {
            ShipType = st;
            Flags = list;
            Focus = focus;
            Pips = pips;
            Firegroup = fg;
            Fuel = fuel;
            Reserve = res;
            Cargo = cargo;
            Pos = pos;
            Heading = heading;
        }

        public UIEvents.UIShipType.Shiptype ShipType { get; private set; }
        public List<UITypeEnum> Flags { get; private set; }
        public int Focus { get; private set; }
        public UIEvents.UIPips.Pips Pips { get; private set; }
        public int Firegroup { get; private set; }
        public double Fuel { get; private set; }
        public double Reserve { get; private set; }
        public int Cargo { get; private set; }
        public UIEvents.UIPosition.Position Pos { get; private set; }
        public double Heading { get; private set; }
    }
}
