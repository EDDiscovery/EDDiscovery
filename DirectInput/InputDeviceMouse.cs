using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectInputDevices
{
    public class InputDeviceMouse : IInputDevice
    {
        public InputDeviceIdentity ID() { return msi; }
        InputDeviceIdentity msi;

        SharpDX.DirectInput.Mouse mouse;
        bool[] butstate;

        System.Threading.AutoResetEvent eventhandle = new System.Threading.AutoResetEvent(false);       // used by joy to signal data
        public System.Threading.AutoResetEvent Eventhandle() { return eventhandle; }

        public InputDeviceMouse(DirectInput di,DeviceInstance d)
        {
            // those silly foreign people call mouse something other than it in english, so we need to fix it to english

            msi = new InputDeviceIdentity() { Instanceguid = d.InstanceGuid, Productguid = d.ProductGuid, Name = "Mouse"};

            mouse = new SharpDX.DirectInput.Mouse(di);
            mouse.SetNotification(eventhandle);
            mouse.Acquire();
            Capabilities c = mouse.Capabilities;
            butstate = new bool[c.ButtonCount];
        }

        public void Dispose()
        {
            if (mouse != null)
            {
                mouse.Unacquire();
                mouse.Dispose();
                mouse = null;
            }
        }

        MouseState ms;

        public List<InputDeviceEvent> GetEvents()
        {
            try
            {
                ms = mouse.GetCurrentState();
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Mouse removed!");
                return null;
            }

            // At the moment, not returning axis, as not needed..

            bool[] buttons = ms.Buttons;

            List<InputDeviceEvent> events = new List<InputDeviceEvent>();

            for (int i = 0; i < butstate.Length; i++)
            {
                bool s = buttons[i];
                if (s != butstate[i])
                {
                    butstate[i] = s;
                    //System.Diagnostics.Debug.WriteLine("But " + (i+1) + "=" + s);
                    events.Add(new InputDeviceEvent(this, i+1, butstate[i]));     // 1..N
                }
            }

            return (events.Count > 0) ? events : null;
        }


        public string EventName(InputDeviceEvent e) // need to return frontier naming convention!
        {
            return "Mouse_" + e.EventNumber.ToStringInvariant();
        }

        public bool? IsPressed(string keyname)
        {
            int mno;
            if (keyname.StartsWith("Mouse_") && int.TryParse(keyname.Substring(6), out mno) && mno >= 1 && mno <= butstate.Length)
            {
                //System.Diagnostics.Debug.WriteLine("Check press " + mno + "=" + butstate[mno-1]);
                return butstate[mno-1];
            }

            return null;
        }

        public override string ToString()
        {
            return msi.Name + ":" + msi.Instanceguid + ":" + msi.Productguid;
        }

        public static void CreateMouse(InputDeviceList ilist)
        {
            DirectInput dinput = new DirectInput();

            foreach (DeviceInstance di in dinput.GetDevices(DeviceClass.Pointer, DeviceEnumerationFlags.AttachedOnly))
            {
                InputDeviceMouse k = new InputDeviceMouse(dinput,di);
                ilist.Add(k);
            }

        }
    }
}
