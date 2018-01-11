/*
 * Copyright © 2017 EDDiscovery development team
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

namespace DirectInputDevices
{
    public class InputDeviceIdentity
    {
        public string Name;
        public Guid Instanceguid, Productguid;
        public int VendorId;
        public int ProductId;
    }

    public class InputDeviceEvent
    {
        public IInputDevice Device;

        public int EventNumber { get; set; }     // indentity of event : keys code, joystick buttons/axis/pov number, etc
        public bool Pressed { get; set; }   // button pressed.. or POV is not centred, or null if it does not press.
        public int Value { get; set; }      // if applicable, axis for instance..

        public InputDeviceEvent(IInputDevice d, int en , bool p, int v = 0)
        {
            Device = d; EventNumber = en; Pressed = p; Value = v; 
        }

        public string ToString(int trunc = 1000)
        {
            return string.Format("Device {0} Event {1} Pressed {2} Value {3}", Device.ID().Name.Truncate(0, trunc), EventName(), Pressed, Value);
        }

        //public Tuple<string, bool> BindingsMatch() { return Device.BindingsMatch(this); }
        public string EventName() { return Device.EventName(this); }
    }
}
