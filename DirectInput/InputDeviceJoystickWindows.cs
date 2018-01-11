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
using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectInputDevices
{
    public class InputDeviceJoystickWindows : IInputDevice
    {
        public InputDeviceIdentity ID() { return jsi;  }
        InputDeviceIdentity jsi;
        SharpDX.DirectInput.Joystick stick;

        bool[] butstate;
        int[] povvalue;
        bool[] axispresent;
        int[] axisvalue;
        int slidercount;
        bool axisevents;    // do we want axis events..

        System.Threading.AutoResetEvent eventhandle = new System.Threading.AutoResetEvent(false);       // used by joy to signal data
        public System.Threading.AutoResetEvent Eventhandle() { return eventhandle; }

        public enum Axis { X = 0, Y, Z, RX, RY, RZ, U, V };      // frontier names for simplicity
        public const int AxisCount = 8;
        public const int AxisNullValue = -1;
        public const int AxisMinRange = 0, AxisMaxRange = 1000;
        public const int POVNotPressed = -1;

        public const int ButtonBase = 1;    // event ID bases
        public const int POVBase = 1000;
        public const int AxisBase = 2000;

        public InputDeviceJoystickWindows(DirectInput di, DeviceInstance d , bool paxison)
        {
            jsi = new InputDeviceIdentity() { Instanceguid = d.InstanceGuid, Productguid = d.ProductGuid, Name = d.InstanceName.RemoveTrailingCZeros()};

            axisevents = paxison;

            stick = new SharpDX.DirectInput.Joystick(di, d.InstanceGuid);
            stick.SetNotification(eventhandle);
            stick.Acquire();

            axispresent = new bool[AxisCount];
            axisvalue = Enumerable.Repeat(AxisNullValue, AxisCount).ToArray();

            Capabilities c = stick.Capabilities;
            butstate = new bool[c.ButtonCount];

            povvalue = Enumerable.Repeat(POVNotPressed, c.PovCount).ToArray();
            slidercount = 0;

            DeviceProperties p = stick.Properties;

            jsi.VendorId = p.VendorId;
            jsi.ProductId = p.ProductId;

            //   string s = p.PortDisplayName;

            System.Diagnostics.Debug.WriteLine("JOY {0} {1} but {2} pov {3}", jsi.Name, jsi.Productguid, butstate.Length, povvalue.Length);

            foreach (DeviceObjectInstance deviceObject in stick.GetObjects())
            {
                if ((deviceObject.ObjectId.Flags & DeviceObjectTypeFlags.Axis) != 0)
                {
                    System.Guid guid = deviceObject.ObjectType;
                    //System.Diagnostics.Debug.WriteLine("  {0} {1} {2} {3} {4}", jsi.Name, deviceObject.UsagePage, deviceObject.Usage, deviceObject.Offset, guid.ToString());

                    if (guid == ObjectGuid.XAxis)
                        axispresent[(int)Axis.X] = true;
                    else if (guid == ObjectGuid.YAxis)
                        axispresent[(int)Axis.Y] = true;
                    else if (guid == ObjectGuid.ZAxis)
                        axispresent[(int)Axis.Z] = true;
                    else if (guid == ObjectGuid.RxAxis)
                        axispresent[(int)Axis.RX] = true;
                    else if (guid == ObjectGuid.RyAxis)
                        axispresent[(int)Axis.RY] = true;
                    else if (guid == ObjectGuid.RzAxis)
                        axispresent[(int)Axis.RZ] = true;
                    else if (guid == ObjectGuid.Slider)
                    {
                        int axisentry = (int)Axis.U + slidercount;
                        if (axisentry < AxisCount)
                        {
                            axispresent[axisentry] = true;
                            slidercount++;      // must be sliders, only ones left with axis
                            //System.Diagnostics.Debug.WriteLine("Slider " + slidercount);
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Unknown Axis " + guid.ToString());
                    }

                    ObjectProperties o = stick.GetObjectPropertiesById(deviceObject.ObjectId);
                    o.Range = new InputRange(AxisMinRange, AxisMaxRange);
                }
            }
        }



        public List<InputDeviceEvent> GetEvents()
        {
            List<InputDeviceEvent> events = new List<InputDeviceEvent>();

            JoystickState js;
            try
            {
                js = stick.GetCurrentState();
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Joystick removed!");
                return null;
            }

            bool[] buttons = js.Buttons;

            for (int i = 0; i < butstate.Length; i++)
            {
                bool s = buttons[i];
                if (s != butstate[i])
                {
                    butstate[i] = s;
                    //System.Diagnostics.Debug.WriteLine("But " + (i + 1) + "=" + s);
                    events.Add(new InputDeviceEvent(this, ButtonBase + i, butstate[i]));
                }
            }

            int[] pov = js.PointOfViewControllers;

            for (int i = 0; i < povvalue.Length; i++)
            {
                if (pov[i] != povvalue[i])
                {
                    if (povvalue[i] != -1 && pov[i] != -1 && pov[i]!=povvalue[i])          // if both previous and current is not released, and changed.. generate a fake release event
                        events.Add(new InputDeviceEvent(this, POVBase + i + 1, false, -1)); // this gives the caller indication that the current state has ended..

                    povvalue[i] = pov[i];
                    events.Add(new InputDeviceEvent(this, POVBase + i + 1, povvalue[i] != -1, povvalue[i]));
                }
            }

            int[] sliders = js.Sliders;

            for (int i = 0; i < axispresent.Length; i++)
            {
                if (axispresent[i])
                {
                    int value;
                    if (i == (int)Axis.X)
                        value = js.X;
                    else if (i == (int)Axis.Y)
                        value = js.Y;
                    else if (i == (int)Axis.Z)
                        value = js.Z;
                    else if (i == (int)Axis.RX)
                        value = js.RotationX;
                    else if (i == (int)Axis.RY)
                        value = js.RotationY;
                    else if (i == (int)Axis.RZ)
                        value = js.RotationZ;
                    else
                        value = sliders[i - (int)Axis.U];

                    if (axisvalue[i] == AxisNullValue)
                        axisvalue[i] = value;
                    else
                    {
                        int diff = Math.Abs(value - axisvalue[i]);
                        if (diff >= 5) // don't report min changes
                        {
                            axisvalue[i] = value;
                            if ( axisevents )
                                events.Add(new InputDeviceEvent(this, AxisBase + i, true, value));      // axis is always pressed
                        }
                    }
                }
            }

            return (events.Count > 0) ? events : null;
        }

        public void Dispose()
        {
            if (stick != null)
            {
                stick.Unacquire();
                stick.Dispose();
                stick = null;
            }
        }

        Dictionary<int, string> povdir = new Dictionary<int, string>() { { 0, "Up" }, { 4500, "UpRight" }, { 9000, "Right" },{ 13500, "DownRight" },{ 18000, "Down" },
                            { 22500, "DownLeft" }, {27000, "Left" }, {31500, "UpLeft" } };

        public string EventName(InputDeviceEvent e)
        {
            if (e.EventNumber < POVBase)
                return "Joy_" + e.EventNumber;
            else if (e.EventNumber < AxisBase)
            {
                string j = "Joy_POV" + (e.EventNumber - POVBase);
                return j + ((povdir.ContainsKey(e.Value)) ? (povdir[e.Value]) : "Centred");
            }
            else
                return "Joy_" + ((Axis)(e.EventNumber - AxisBase)).ToString() + "Axis";
        }

        public bool? IsPressed(string eventname)
        {
            if (eventname.StartsWith("Joy_POV"))
            {
                if (eventname.Length >= 7 + 1 + 2)
                {
                    int num = eventname[7] - '0';
                    string sdir = eventname.Substring(8);

                    if (num >= 1 && num <= povvalue.Length)
                    {
                        int dir = povvalue[num - 1];
                        string actualdir = povdir.ContainsKey(dir) ? povdir[dir] : "Centred";
                        return actualdir.Equals(sdir);
                    }
                }
            }
            else if (eventname.Contains("Axis"))        // axis are always pressed in effect..
            {
                return null;
            }
            else if (eventname.StartsWith("Joy_"))
            {
                int but = 0;
                if (eventname.Substring(4).InvariantParse(out but) && but >= 1 && but <= butstate.Length )
                    return butstate[but - 1];
            }

            return null;
        }

        public override string ToString()
        {
            return jsi.Name + ":" + jsi.Instanceguid + ":" + jsi.Productguid + ":" + jsi.ProductId.ToString("x") + "," + jsi.VendorId.ToString("x") + ":" + butstate.Length + "," + povvalue.Length + "," + slidercount;
        }

        public static void CreateJoysticks(InputDeviceList ilist, bool axisevents)
        {
            DirectInput dinput = new DirectInput();

            foreach (DeviceInstance di in dinput.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly))
            {
                //   if (di.InstanceName.Contains("Logitech"))
                {
                    InputDeviceJoystickWindows j = new InputDeviceJoystickWindows(dinput, di, axisevents);
                    ilist.Add(j);
                }
            }
        }

    }
}
