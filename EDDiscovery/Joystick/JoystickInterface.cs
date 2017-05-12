using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Joystick
{
    public class JoyStickIdentity
    {
        public string Name;
        public Guid Instanceguid, Productguid;
    }

    public class JoystickEvent
    {
        public JoyStickIdentity js { get; set; }

        public enum Etype { Button, Axis , POV };
        public Etype EventType { get; set; }

        public const int AxisCount = 8;
        public const int AxisNullValue = -1;
        public const int AxisMinRange = 0, AxisMaxRange = 1000;
        public const int POVNotPressed = -1;
        
        public enum Axis { X=0,Y,Z,RX,RY,RZ,U,V };      // frontier names for simplicity

        public Axis AxisName { get; set; }    // axis..
        public int Number { get; set; }     // button number, POV number
        public int Value { get; set; }      // axis value (0..1000 always), POV direction (-1 none)
        public bool Pressed { get; set; }   // button pressed.. or POV is not centred

        public static JoystickEvent Button(JoyStickIdentity j, int n, bool s)
        {
            return new JoystickEvent() { js = j, EventType = Etype.Button, Number =n , Pressed = s };
        }
        public static JoystickEvent POV(JoyStickIdentity j, int n, int dir)
        {
            return new JoystickEvent() { js = j, EventType = Etype.POV, Number = n, Value = dir, Pressed = (dir != POVNotPressed) };
        }
        public static JoystickEvent MoveAxis(JoyStickIdentity j, Axis n, int v)
        {
            return new JoystickEvent() { js = j, EventType = Etype.Axis, AxisName = n, Value = v};
        }

        public override string ToString()
        {
            if (EventType == Etype.Axis)
                return string.Format("Name {0} Axis {1} Value {2}", js.Name, AxisName, Value);
            else if (EventType == Etype.POV)
                return string.Format("Name {0} POV {1} Value {2} Pressed {3}", js.Name, Number, Value, Pressed);
            else
                return string.Format("Name {0} Button {1} Pressed {2}", js.Name, Number, Pressed);
        }

        public string Device { get { return js.Name; } }
        public string Item
        {
            get
            {
                if (EventType == Etype.Axis)
                    return string.Format("Joy_" + AxisName + "Axis");
                else if (EventType == Etype.POV)
                    return string.Format("Joy_POV" + (Number+1));
                else
                    return string.Format("Joy_" + (Number+1) + (Pressed ? "" : "_Up"));
            }
        }

        public bool POVLeft { get { return Value > 18000; } }
        public bool POVRight { get { return Value > 0 && Value < 18000; } }
        public bool POVUp { get { return Value >= 0 && (Value < 9000 || Value > 27000); } }
        public bool POVDown { get { return Value > 90 && Value < 270000;} }

        public bool POV(string name)
        {
            if (name.Equals("Left", StringComparison.InvariantCultureIgnoreCase))
                return POVLeft;
            else if (name.Equals("Right", StringComparison.InvariantCultureIgnoreCase))
                return POVRight;
            else if (name.Equals("Up", StringComparison.InvariantCultureIgnoreCase))
                return POVUp;
            else if (name.Equals("Down", StringComparison.InvariantCultureIgnoreCase))
                return POVDown;
            else
                return false;
        }
    }

    interface JoystickInterface
    {
        void Start();
        void Stop();
        List<JoystickEvent> Poll();

        bool IsButtonPressed(string devicename, int i);    //0..N

        void Dispose();
    }
}
