using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.InputDevices
{
    class InputDeviceKeyboard : InputDeviceInterface
    {
        public InputDeviceIdentity ID() { return ksi; }
        InputDeviceIdentity ksi;

        SharpDX.DirectInput.Keyboard keyboard;

        System.Threading.AutoResetEvent eventhandle = new System.Threading.AutoResetEvent(false);       // used by joy to signal data
        public System.Threading.AutoResetEvent Eventhandle() { return eventhandle; }

        public InputDeviceKeyboard(DirectInput di,DeviceInstance d)
        {
            ksi = new InputDeviceIdentity() { Instanceguid = d.InstanceGuid, Productguid = d.ProductGuid, Name = d.InstanceName.RemoveTrailingCZeros()};

            keyboard = new Keyboard(di);
            keyboard.Properties.BufferSize = 128;
            keyboard.SetNotification(eventhandle);
            keyboard.Acquire();
        }

        public void Dispose()
        {
            if (keyboard != null)
            {
                keyboard.Unacquire();
                keyboard.Dispose();
                keyboard = null;
            }
        }

        KeyboardState ks;

        public List<InputDeviceEvent> GetEvents()
        {
            ks = keyboard.GetCurrentState();
            KeyboardUpdate[] ke = keyboard.GetBufferedData();

            List<InputDeviceEvent> events = new List<InputDeviceEvent>();
            foreach (KeyboardUpdate k in ke)
            {
                //System.Diagnostics.Debug.WriteLine("key " + k.Key + " " + k.IsPressed );
                events.Add(new InputDeviceEvent(this, (int)k.Key, k.IsPressed));
            }

            return (events.Count > 0) ? events : null;
        }

        Tuple<string, string>[] strtx = new Tuple<string, string>[] 
        {
            new Tuple<string,string>("Up","UpArrow"),
            new Tuple<string,string>("Down","DownArrow"),
            new Tuple<string,string>("Left","LeftArrow"),
            new Tuple<string,string>("Right","RightArrow"),
            new Tuple<string,string>("Return","Enter"),
            new Tuple<string,string>("Capital","CapsLock"),
            new Tuple<string,string>("Back","Backspace"),
            new Tuple<string,string>("NumberLock","NumLock"),
            new Tuple<string,string>("Subtract","Numpad_Subtract"),
            new Tuple<string,string>("Divide","Numpad_Divide"),
            new Tuple<string,string>("Multiply","Numpad_Multiply"),
            new Tuple<string,string>("Add","Numpad_Add"),
            new Tuple<string,string>("NumberPadEnter","Numpad_Enter"),
            new Tuple<string,string>("Decimal","Numpad_Decimal"),
       };

        public string EventName(InputDeviceEvent e) // need to return frontier naming convention!
        {
            Key k = (Key)(e.EventNumber);

            string keyname = k.ToString();
            string newname = keyname;

            if (keyname.Length == 2 && keyname[0] == 'D')
                newname = keyname.Substring(1);
            else if (keyname.StartsWith("NumberPad") && char.IsDigit(keyname[9]))
                newname = "Numpad_" + keyname[9];
            else
            {
                int i = Array.FindIndex(strtx, x => x.Item1.Equals(keyname));
                if (i >= 0)
                    newname = strtx[i].Item2;
            }

            newname = "Key_" + newname;
            //System.Diagnostics.Debug.WriteLine("Name is " + keyname + "=>" + newname);
            return newname;
        }

        public bool? IsPressed(string keyname)
        {
            keyname = keyname.Substring(4);
            if (keyname.Length == 1 && (keyname[0] >= '0' && keyname[0] <= '9'))
                keyname = "D" + keyname;
            else if (keyname.StartsWith("Numpad_") && char.IsDigit(keyname[7]))
                keyname = "NumberPad" + keyname[7];
            else
            {
                int i = Array.FindIndex(strtx, x => x.Item2.Equals(keyname));
                if (i >= 0)
                    keyname = strtx[i].Item1;
            }

            Key k;
            if (Enum.TryParse<Key>(keyname, out k))
            {
                return ks.IsPressed(k);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("FAILED IsPressed " + keyname);
            }

            return false;
        }

        public override string ToString()
        {
            return ksi.Name + ":" + ksi.Instanceguid + ":" + ksi.Productguid;
        }

        public static void CreateKeyboard(InputDeviceList ilist)
        {
            DirectInput dinput = new DirectInput();

            foreach (DeviceInstance di in dinput.GetDevices(DeviceClass.Keyboard, DeviceEnumerationFlags.AttachedOnly))
            {
                InputDeviceKeyboard k = new InputDeviceKeyboard(dinput,di);
                ilist.Add(k);
            }

        }
    }
}
