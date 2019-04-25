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
    // NOTE you get this when position appears AND disappears, check for validity before use

    public class UIPosition : UIEvent
    {
        public UIPosition(Position value, double head, DateTime time, bool refresh): base(UITypeEnum.Position, time, refresh)
        {
            Location = value;
            Heading = head;
        }

        public Position Location { get; private set; }

        // you MAY not get heading.

        public double Heading { get; private set; }
        public bool ValidHeading { get { return Heading != double.MinValue; } }

        public class Position
        {
            public Position()
            {
                Latitude = Longitude = Altitude = double.MinValue;
                AltitudeFromAverageRadius = false;
            }

            // you MAY get position without Altitude.. seen in SRV it doing that.  Code defensively

            public bool ValidPosition { get { return Latitude != double.MinValue && Latitude != double.MinValue; } }
            public bool ValidAltitude { get { return Altitude != double.MinValue; } }

            public double Latitude;
            public double Longitude;
            public double Altitude;
            public bool AltitudeFromAverageRadius;
        }
    }
}
