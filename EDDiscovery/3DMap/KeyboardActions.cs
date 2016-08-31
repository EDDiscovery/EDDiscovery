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
            Left, Right, Up, Down, PgUp, PgDown,
            Pitch, Dive,YawLeft,YawRight,RollLeft, RollRight,
            ZoomIn, ZoomOut, Zoom1, Zoom2, Zoom3, Zoom4, Zoom5, Zoom6, Zoom7, Zoom8, Zoom9,
            IncrStar,DecrStar,
            Record, RecordStep, RecordNewStep, RecordPause, Playback, 
            Help,
            FPS,
        };

        public bool Action(ActionType type ) { return actions.Contains(type); }
        public bool Shift { get { return shiftpressed; } }
        public bool Ctrl { get { return ctrlpressed; } }

        private List<ActionType> actions = new List<ActionType>();
        private List<ActionType> pressed = new List<ActionType>();
        private bool shiftpressed = false;
        private bool ctrlpressed = false;

        public void Clear()
        {
            actions.Clear();
            shiftpressed = false;
            ctrlpressed = false;
        }

        public bool Any()
        {
            return actions.Count > 0;
        }

        public void Add(ActionType v, bool state, bool onkeydownonly = false)
        {
            if (state)
            {
                if (!onkeydownonly || !pressed.Contains(v))     // if don't care about repeating, or not pressed
                {
                    actions.Add(v);
                    pressed.Add(v);
                }
            }
            else
                pressed.Remove(v);                          // not pressed.
        }

        public void ReceiveKeyboardActions()
        {
            Clear();

            try
            {
                var state = OpenTK.Input.Keyboard.GetState();

                shiftpressed = state[Key.LShift] || state[Key.RShift];
                ctrlpressed = state[Key.ControlLeft] || state[Key.ControlRight];

                if (ctrlpressed)
                {
                    Add(ActionType.FPS, state[Key.F], true);            // ctrl+f
                }
                else
                {                                                       // shift/noshift + keys
                    Add(ActionType.Help, state[Key.Slash] || state[Key.F1], true);           // certain keys are press down only once, release before next trigger  
                    Add(ActionType.IncrStar, state[Key.F3]);
                    Add(ActionType.DecrStar, state[Key.F4]);
                    Add(ActionType.Record, state[Key.F5], true);
                    Add(ActionType.RecordStep, state[Key.F6], true);
                    Add(ActionType.RecordNewStep, state[Key.F7], true);
                    Add(ActionType.RecordPause, state[Key.F8], true);
                    Add(ActionType.Playback, state[Key.F9], true);

                    Add(ActionType.Left, state[Key.Left] || state[Key.A]);
                    Add(ActionType.Right, state[Key.Right] || state[Key.D]);
                    Add(ActionType.Up, state[Key.W] || state[Key.Up]);
                    Add(ActionType.Down, state[Key.S] || state[Key.Down]);
                    Add(ActionType.PgUp, state[Key.PageUp] || state[Key.R]);                    // WASD is fore/back/left/right, R/F is up down
                    Add(ActionType.PgDown, state[Key.PageDown] || state[Key.F]);

                    Add(ActionType.ZoomIn, state[Key.Plus] || state[Key.Z]);                 // additional Useful keys
                    Add(ActionType.ZoomOut, state[Key.Minus] || state[Key.X]);
                    Add(ActionType.YawLeft, state[Key.Keypad4]);
                    Add(ActionType.YawRight, state[Key.Keypad6]);
                    Add(ActionType.Pitch, state[Key.Keypad8]);
                    Add(ActionType.Dive, state[Key.Keypad5] || state[Key.Keypad2]);
                    Add(ActionType.RollLeft, state[Key.Keypad7] || state[Key.Q]);
                    Add(ActionType.RollRight, state[Key.Keypad9] || state[Key.E]);

                    Add(ActionType.Zoom1, state[Key.Number1], true);
                    Add(ActionType.Zoom2, state[Key.Number2], true);
                    Add(ActionType.Zoom3, state[Key.Number3], true);
                    Add(ActionType.Zoom4, state[Key.Number4], true);
                    Add(ActionType.Zoom5, state[Key.Number5], true);
                    Add(ActionType.Zoom6, state[Key.Number6], true);
                    Add(ActionType.Zoom7, state[Key.Number7], true);
                    Add(ActionType.Zoom8, state[Key.Number8], true);
                    Add(ActionType.Zoom9, state[Key.Number9], true);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"ReceiveKeybaordActions Exception: {ex.Message}");
                return;
            }
        }

    }
}
