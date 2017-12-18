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
using System.Collections.Generic;
using System.Threading;

namespace DirectInputDevices
{
    public interface IInputDevice
    {
        InputDeviceIdentity ID();
        AutoResetEvent Eventhandle();               // set when device changes state

        List<InputDeviceEvent> GetEvents();         // get events after change state
        void Dispose();

        string EventName(InputDeviceEvent e);       // Frontier event name from input event

        bool? IsPressed(string eventname);          // if an input supports pressed, true/false, else null

        string ToString();
    }
}
