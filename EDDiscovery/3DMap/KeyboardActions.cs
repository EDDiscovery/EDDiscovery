using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2._3DMap
{
    public class KeyboardActions
    {
        public enum ActionType
        {
            Left,
            Right,
            Up,
            Down,
            Forwards,
            Backwards,
            Pitch,
            Dive,
            YawLeft,
            YawRight,
            RollLeft,
            RollRight,
            ZoomIn,
            ZoomOut,
            ZoomDefault,
            ZoomWide,
            IncrStar,
            DecrStar,
            Record,
        };

        public bool Action(ActionType type ) { return actions.Contains(type); }

        private List<ActionType> actions = new List<ActionType>();

        public void Reset()
        {
            actions.Clear();
        }

        public bool Any()
        {
            return actions.Count > 0;
        }

        public void Add(ActionType v, bool state)
        {
            if (state)
                actions.Add(v);
        }

        public void ReceiveKeyboardActions( bool perspective )
        {
            Reset();

            try
            {
                var state = OpenTK.Input.Keyboard.GetState();

                Add(ActionType.IncrStar, state[Key.F1] );
                Add(ActionType.DecrStar, state[Key.F2] );
                Add(ActionType.Record, state[Key.F5] );

                Add(ActionType.Left, state[Key.Left] || state[Key.A]);
                Add(ActionType.Right, state[Key.Right] || state[Key.D]);

                if (perspective)
                {
                    Add(ActionType.Up, state[Key.PageUp] || state[Key.R]);
                    Add(ActionType.Down, state[Key.PageDown] || state[Key.F]);
                    Add(ActionType.Forwards, state[Key.Up] || state[Key.W]);                    // WASD is fore/back/left/right, R/F is up down
                    Add(ActionType.Backwards, state[Key.Down] || state[Key.S]);
                }
                else
                {
                    Add(ActionType.Up, state[Key.W] || state[Key.Up]);
                    Add(ActionType.Down, state[Key.S] || state[Key.Down]);
                }

                Add(ActionType.ZoomIn, state[Key.Plus] || state[Key.Z]);                 // additional Useful keys
                Add(ActionType.ZoomOut, state[Key.Minus] || state[Key.X]);
                Add(ActionType.ZoomDefault, state[Key.BracketRight]);
                Add(ActionType.ZoomWide, state[Key.BracketLeft]);
                Add(ActionType.YawLeft, state[Key.Keypad4]);
                Add(ActionType.YawRight, state[Key.Keypad6]);
                Add(ActionType.Pitch, state[Key.Keypad8]);
                Add(ActionType.Dive, state[Key.Keypad5] || state[Key.Keypad2]);
                Add(ActionType.RollLeft, state[Key.Keypad7] || state[Key.Q]);
                Add(ActionType.RollRight, state[Key.Keypad9] || state[Key.E]);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"ReceiveKeybaordActions Exception: {ex.Message}");
                return;
            }
        }

    }
}
