/*
 * Copyright © 2015 - 2016 EDDiscovery development team
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

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenTKUtils
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
        private HashSet<Keys> keyspressed = new HashSet<Keys>();

        public void Clear()
        {
            actions.Clear();
            shiftpressed = false;
            ctrlpressed = false;
        }

        public void Reset()
        {
            keyspressed.Clear();
            Clear();
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

        public void KeyDown(object sender, KeyEventArgs e)
        {
            ctrlpressed = e.Control;
            shiftpressed = e.Shift;
            keyspressed.Add(e.KeyCode);
        }

        public void KeyUp(object sender, KeyEventArgs e)
        {
            ctrlpressed = e.Control;
            shiftpressed = e.Shift;
            keyspressed.Remove(e.KeyCode);
        }

        public void ReceiveKeyboardActions()
        {
            Clear();

            try
            {
                shiftpressed = keyspressed.Contains(Keys.LShiftKey) || keyspressed.Contains(Keys.RShiftKey) || keyspressed.Contains(Keys.ShiftKey);
                ctrlpressed = keyspressed.Contains(Keys.LControlKey) || keyspressed.Contains(Keys.RControlKey) || keyspressed.Contains(Keys.ControlKey);

                if (ctrlpressed)
                {
                    Add(ActionType.FPS, keyspressed.Contains(Keys.F), true);            // ctrl+f
                }
                else
                {                                                       // shift/noshift + keys
                    Add(ActionType.Help, keyspressed.Contains(Keys.Divide) || keyspressed.Contains(Keys.F1), true);           // certain keys are press down only once, release before next trigger  
                    Add(ActionType.IncrStar, keyspressed.Contains(Keys.F3));
                    Add(ActionType.DecrStar, keyspressed.Contains(Keys.F4));
                    Add(ActionType.Record, keyspressed.Contains(Keys.F5), true);
                    Add(ActionType.RecordStep, keyspressed.Contains(Keys.F6), true);
                    Add(ActionType.RecordNewStep, keyspressed.Contains(Keys.F7), true);
                    Add(ActionType.RecordPause, keyspressed.Contains(Keys.F8), true);
                    Add(ActionType.Playback, keyspressed.Contains(Keys.F9), true);

                    Add(ActionType.Left, keyspressed.Contains(Keys.Left) || keyspressed.Contains(Keys.A));
                    Add(ActionType.Right, keyspressed.Contains(Keys.Right) || keyspressed.Contains(Keys.D));
                    Add(ActionType.Up, keyspressed.Contains(Keys.W) || keyspressed.Contains(Keys.Up));
                    Add(ActionType.Down, keyspressed.Contains(Keys.S) || keyspressed.Contains(Keys.Down));
                    Add(ActionType.PgUp, keyspressed.Contains(Keys.PageUp) || keyspressed.Contains(Keys.R));                    // WASD is fore/back/left/right, R/F is up down
                    Add(ActionType.PgDown, keyspressed.Contains(Keys.PageDown) || keyspressed.Contains(Keys.F));

                    Add(ActionType.ZoomIn, keyspressed.Contains(Keys.Add) || keyspressed.Contains(Keys.Z));                 // additional Useful keys
                    Add(ActionType.ZoomOut, keyspressed.Contains(Keys.Subtract) || keyspressed.Contains(Keys.X));
                    Add(ActionType.YawLeft, keyspressed.Contains(Keys.NumPad4));
                    Add(ActionType.YawRight, keyspressed.Contains(Keys.NumPad6));
                    Add(ActionType.Pitch, keyspressed.Contains(Keys.NumPad8));
                    Add(ActionType.Dive, keyspressed.Contains(Keys.NumPad5) || keyspressed.Contains(Keys.NumPad2));
                    Add(ActionType.RollLeft, keyspressed.Contains(Keys.NumPad7) || keyspressed.Contains(Keys.Q));
                    Add(ActionType.RollRight, keyspressed.Contains(Keys.NumPad9) || keyspressed.Contains(Keys.E));

                    Add(ActionType.Zoom1, keyspressed.Contains(Keys.D1), true);
                    Add(ActionType.Zoom2, keyspressed.Contains(Keys.D2), true);
                    Add(ActionType.Zoom3, keyspressed.Contains(Keys.D3), true);
                    Add(ActionType.Zoom4, keyspressed.Contains(Keys.D4), true);
                    Add(ActionType.Zoom5, keyspressed.Contains(Keys.D5), true);
                    Add(ActionType.Zoom6, keyspressed.Contains(Keys.D6), true);
                    Add(ActionType.Zoom7, keyspressed.Contains(Keys.D7), true);
                    Add(ActionType.Zoom8, keyspressed.Contains(Keys.D8), true);
                    Add(ActionType.Zoom9, keyspressed.Contains(Keys.D9), true);
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
