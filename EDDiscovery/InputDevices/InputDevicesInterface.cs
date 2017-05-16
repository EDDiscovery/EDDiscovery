using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.InputDevices
{
    public class InputDeviceIdentity
    {
        public string Name;
        public Guid Instanceguid, Productguid;
        public string DevType;      // "JOY" "KEYBOARD"
    }

    public class InputDeviceEvent
    {
        public InputDeviceInterface Device;

        public int EventNumber { get; set; }     // indentity of event : keys ASCII char, joystick buttons/axis/pov number
        public bool Pressed { get; set; }   // button pressed.. or POV is not centred
        public int Value { get; set; }      // if applicable, axis for instance..

        public InputDeviceEvent(InputDeviceInterface d, int en , bool p, int v = 0)
        {
            Device = d; EventNumber = en; Pressed = p; Value = v; 
        }

        public string ToString(int trunc = 1000)
        {
            return string.Format("Name {0} Event {1} Pressed {2} Value {3}", Device.ID().Name.Truncate(0,trunc), EventNumber, Pressed, Value);
        }

        //public Tuple<string, bool> BindingsMatch() { return Device.BindingsMatch(this); }
        public string EventName() { return Device.EventName(this); }
    }

    public interface InputDeviceInterface
    {
        InputDeviceIdentity ID();
        List<InputDeviceEvent> Poll();
        void Dispose();

        //Tuple<string, bool> BindingsMatch(InputDeviceEvent e);
        string EventName(InputDeviceEvent e);
        bool IsPressed(string eventname);
    }
}
