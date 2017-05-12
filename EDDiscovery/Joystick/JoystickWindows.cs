using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.DirectInput;

namespace EDDiscovery.Joystick
{
    public class JoystickWindows : JoystickInterface
    {
        public class Joy
        {
            public JoystickIdentity jsi { get; private set; }
            public bool[] butstate { get; private set; }
            public int[] povvalue { get; private set; }

            SharpDX.DirectInput.Joystick stick;

            bool[] axispresent;
            int[] axisvalue;
            int slidercount;

            public Joy(DirectInput di, DeviceInstance d)
            {
                jsi = new JoystickIdentity() { Instanceguid = d.InstanceGuid, Productguid = d.ProductGuid, Name = d.InstanceName.RemoveTrailingCZeros() };

                stick = new SharpDX.DirectInput.Joystick(di, d.InstanceGuid);
                stick.Acquire();

                axispresent = new bool[JoystickEvent.AxisCount];
                axisvalue = Enumerable.Repeat(JoystickEvent.AxisNullValue, JoystickEvent.AxisCount).ToArray();

                Capabilities c = stick.Capabilities;
                butstate = new bool[c.ButtonCount];

                povvalue = Enumerable.Repeat(JoystickEvent.POVNotPressed, c.PovCount).ToArray();
                slidercount = 0;

                DeviceProperties p = stick.Properties;

                //   string s = p.PortDisplayName;

                System.Diagnostics.Debug.WriteLine("JOY {0} {1} but {2} pov {3}", jsi.Name, jsi.Productguid, butstate.Length, povvalue.Length);

                foreach (DeviceObjectInstance deviceObject in stick.GetObjects())
                {
                    if ((deviceObject.ObjectId.Flags & DeviceObjectTypeFlags.Axis) != 0)
                    {
                        System.Guid guid = deviceObject.ObjectType;
                        System.Diagnostics.Debug.WriteLine("  {0} {1} {2} {3} {4}", jsi.Name, deviceObject.UsagePage, deviceObject.Usage, deviceObject.Offset, guid.ToString());

                        if (guid == ObjectGuid.XAxis)
                            axispresent[(int)JoystickEvent.Axis.X] = true;
                        else if (guid == ObjectGuid.YAxis)
                            axispresent[(int)JoystickEvent.Axis.Y] = true;
                        else if (guid == ObjectGuid.ZAxis)
                            axispresent[(int)JoystickEvent.Axis.Z] = true;
                        else if (guid == ObjectGuid.RxAxis)
                            axispresent[(int)JoystickEvent.Axis.RX] = true;
                        else if (guid == ObjectGuid.RyAxis)
                            axispresent[(int)JoystickEvent.Axis.RY] = true;
                        else if (guid == ObjectGuid.RzAxis)
                            axispresent[(int)JoystickEvent.Axis.RZ] = true;
                        else
                        {
                            axispresent[(int)JoystickEvent.Axis.U + slidercount] = true;
                            slidercount++;      // must be sliders, only ones left with axis
                        }

                        ObjectProperties o = stick.GetObjectPropertiesById(deviceObject.ObjectId);
                        o.Range = new InputRange(JoystickEvent.AxisMinRange, JoystickEvent.AxisMaxRange);
                    }
                }
            }

            public void Poll(ref List<JoystickEvent> list)
            {
                JoystickState js = stick.GetCurrentState();

                bool[] buttons = js.Buttons;

                for (int i = 0; i < butstate.Length; i++)
                {
                    bool s = buttons[i];
                    if (s != butstate[i])
                    {
                        butstate[i] = s;
                        System.Diagnostics.Debug.WriteLine("But " + (i+1) + "=" + s);
                        list.Add(JoystickEvent.Button(jsi, i+1, s));
                    }
                }

                int[] pov = js.PointOfViewControllers;

                for (int i = 0; i < povvalue.Length; i++)
                {
                    if (pov[i] != povvalue[i])
                    {
                        povvalue[i] = pov[i];
                        list.Add(JoystickEvent.POV(jsi, i+1, pov[i]));
                    }
                }

                int[] sliders = js.Sliders;

                for (int i = 0; i < axispresent.Length; i++)
                {
                    if (axispresent[i])
                    {
                        int value;
                        if (i == (int)JoystickEvent.Axis.X)
                            value = js.X;
                        else if (i == (int)JoystickEvent.Axis.Y)
                            value = js.Y;
                        else if (i == (int)JoystickEvent.Axis.Z)
                            value = js.Z;
                        else if (i == (int)JoystickEvent.Axis.RX)
                            value = js.RotationX;
                        else if (i == (int)JoystickEvent.Axis.RY)
                            value = js.RotationY;
                        else if (i == (int)JoystickEvent.Axis.RZ)
                            value = js.RotationZ;
                        else
                            value = sliders[i - (int)JoystickEvent.Axis.U];

                        if (axisvalue[i] == JoystickEvent.AxisNullValue)
                            axisvalue[i] = value;
                        else
                        {
                            int diff = Math.Abs(value - axisvalue[i]);
                            if (diff >= (JoystickEvent.AxisMaxRange - JoystickEvent.AxisMinRange) / 20)
                            {
                                axisvalue[i] = value;
                                list.Add(JoystickEvent.MoveAxis(jsi, (JoystickEvent.Axis)i, value));
                            }
                        }
                    }
                }
            }

            public void Dispose()
            {
                if (stick != null)
                    stick.Dispose();
            }
        }

        List<Joy> joylist = null;

        public void Start()
        {
            if (joylist == null)
            {
                joylist = new List<Joy>();

                DirectInput dinput = new DirectInput();

                foreach (DeviceInstance di in dinput.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly))
                {
                    Joy j = new Joy(dinput, di);
                    joylist.Add(j);
                }
            }
        }

        public void Stop()
        {
        }

        public void Dispose()
        {
            if (joylist != null)
            {
                foreach (Joy j in joylist)
                    j.Dispose();
            }
        }

        public List<JoystickEvent> Poll()
        {
            List<JoystickEvent> list = new List<JoystickEvent>();
            foreach (Joy j in joylist)
                j.Poll(ref list);

            return list;
        }

        public bool IsButtonPressed(JoystickIdentity id, int i) // i is 1..N not 0..N
        {
            Joy j = joylist.Find(x => x.jsi.Name.Equals(id.Name, StringComparison.InvariantCultureIgnoreCase));
            return (j != null && i >= 1 && i <= j.butstate.Length) ? j.butstate[i - 1] : false;
        }

        public bool IsPOVPressed(JoystickIdentity id, int pov, string dir)   // Left, Right, Up, Down
        {
            Joy j = joylist.Find(x => x.jsi.Name.Equals(id.Name, StringComparison.InvariantCultureIgnoreCase));
            if ( j != null )
            {
                pov--;
                if (pov < j.povvalue.Length)
                {
                    return JoystickEvent.POV(dir, j.povvalue[pov]);
                }
            }

            return false;
        }

        public List<JoystickIdentity> List()
        {
            List<JoystickIdentity> ret = new List<JoystickIdentity>();
            foreach (Joy j in joylist)
                ret.Add(j.jsi);
            return ret;
        }

    }
}
