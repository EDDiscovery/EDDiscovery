using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Joystick
{
    public class JoystickIdentity
    {
        public string Name;
        public Guid Instanceguid, Productguid;
    }

    public class JoystickEvent
    {
        public JoystickIdentity Device { get; set; }

        public enum Etype { Button, Axis, POV };
        public Etype EventType { get; set; }
        public bool IsButtonEvent { get { return EventType == Etype.Button; } }
        public bool IsAxisEvent { get { return EventType == Etype.Axis; } }
        public bool IsPOVEvent { get { return EventType == Etype.POV; } }

        public const int AxisCount = 8;
        public const int AxisNullValue = -1;
        public const int AxisMinRange = 0, AxisMaxRange = 1000;
        public const int POVNotPressed = -1;

        public enum Axis { X = 0, Y, Z, RX, RY, RZ, U, V };      // frontier names for simplicity

        public Axis AxisName { get; set; }    // axis..
        public int Number { get; set; }     // button number (1..N), POV number (1..N)
        public int Value { get; set; }      // axis value (0..1000 always), POV direction (-1 none)
        public bool Pressed { get; set; }   // button pressed.. or POV is not centred

        public static JoystickEvent Button(JoystickIdentity j, int n, bool s)
        {
            return new JoystickEvent() { Device = j, EventType = Etype.Button, Number = n, Pressed = s };
        }
        public static JoystickEvent POV(JoystickIdentity j, int n, int dir)
        {
            return new JoystickEvent() { Device = j, EventType = Etype.POV, Number = n, Value = dir, Pressed = (dir != POVNotPressed) };
        }
        public static JoystickEvent MoveAxis(JoystickIdentity j, Axis n, int v)
        {
            return new JoystickEvent() { Device = j, EventType = Etype.Axis, AxisName = n, Value = v };
        }

        public override string ToString()
        {
            if (EventType == Etype.Axis)
                return string.Format("Name {0} Axis {1} Value {2}", Device.Name, AxisName, Value);
            else if (EventType == Etype.POV)
                return string.Format("Name {0} POV {1} Value {2} {3}", Device.Name, Number, Value, Pressed);
            else
                return string.Format("Name {0} Button {1} Pressed {2}", Device.Name, Number, Pressed);
        }

        public string Item
        {
            get
            {
                if (EventType == Etype.Axis)
                    return string.Format("Joy_" + AxisName + "Axis");
                else if (EventType == Etype.POV)
                    return string.Format("Joy_POV" + Number);
                else
                    return string.Format("Joy_" + Number);
            }
        }


        public bool isPOVLeft { get { return POVLeft(Value); } }
        public bool isPOVRight { get { return POVRight(Value); } }
        public bool isPOVUp { get { return POVUp(Value); } }
        public bool isPOVDown { get { return POVDown(Value); } }
        public bool IsPOVDirectionStraight { get { return POVDirectionStraight(Value); } }
        public bool IsPOVCentre { get { return Value<0; } }

        public static bool POVLeft(int v) { return v > 18000; }
        public static bool POVRight(int v) { return v > 0 && v < 18000; } 
        public static bool POVUp(int v) { return v >= 0 && (v < 9000 || v > 27000); }
        public static bool POVDown(int v) { return v > 9000 && v < 27000; }
        public static bool POVDirectionStraight(int v) { return v==0 || v==9000 || v == 18000 || v == 27000; }

        public static bool POV(string name , int v)
        {
            if (name.Equals("Left", StringComparison.InvariantCultureIgnoreCase))
                return POVLeft(v);
            else if (name.Equals("Right", StringComparison.InvariantCultureIgnoreCase))
                return POVRight(v);
            else if (name.Equals("Up", StringComparison.InvariantCultureIgnoreCase))
                return POVUp(v);
            else if (name.Equals("Down", StringComparison.InvariantCultureIgnoreCase))
                return POVDown(v);
            else
                return false;
        }
    }

    interface JoystickInterface
    {
        void Start();
        void Stop();
        List<JoystickEvent> Poll();

        bool IsButtonPressed(JoystickIdentity jis, int i);    // 1..N
        bool IsPOVPressed(JoystickIdentity jis, int pov, string dir);   // Left, Right, Up, Down

        List<JoystickIdentity> List();

        void Dispose();
    }
}
