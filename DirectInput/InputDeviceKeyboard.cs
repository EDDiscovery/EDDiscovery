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
using System.Windows.Forms;

namespace DirectInputDevices
{
    static public class KeyConversion       // three naming conventsions, lovely!
    {
        static public string SharpKeyToFrontierName(SharpDX.DirectInput.Key k)     
        {
            string keyname = k.ToString();
            string newname = keyname;

            if (keyname.Length == 2 && keyname[0] == 'D')
                newname = keyname.Substring(1);
            else if (keyname.StartsWith("NumberPad") && char.IsDigit(keyname[9]))
                newname = "Numpad_" + keyname[9];
            else
            {
                int i = Array.FindIndex(sharptofrontiername, x => x.Item1.Equals(keyname));
                if (i >= 0)
                    newname = sharptofrontiername[i].Item2;
            }

            newname = "Key_" + newname;
            //System.Diagnostics.Debug.WriteLine("Name is " + keyname + "=>" + newname);
            return newname;
        }

        static public SharpDX.DirectInput.Key? FrontierNameToSharpKey(string frontierkeyname)  
        {
            frontierkeyname = frontierkeyname.Substring(4);
            if (frontierkeyname.Length == 1 && (frontierkeyname[0] >= '0' && frontierkeyname[0] <= '9'))
                frontierkeyname = "D" + frontierkeyname;
            else if (frontierkeyname.StartsWith("Numpad_") && char.IsDigit(frontierkeyname[7]))
                frontierkeyname = "NumberPad" + frontierkeyname[7];
            else
            {
                int i = Array.FindIndex(sharptofrontiername, x => x.Item2.Equals(frontierkeyname));
                if (i >= 0)
                    frontierkeyname = sharptofrontiername[i].Item1;
            }

            Key k;
            if (Enum.TryParse<Key>(frontierkeyname, true, out k))   // a few sharp names have case differences (Semicolon) ignore it
                return k;
            else
                return null;
        }

        static public System.Windows.Forms.Keys SharpKeyToKeys(SharpDX.DirectInput.Key k)        // Sharp DX - > Windows Keys
        {
            if (sharptokeys.ContainsKey(k))
                return sharptokeys[k];
            return Keys.None;
        }

        static public SharpDX.DirectInput.Key KeysToSharpKey(System.Windows.Forms.Keys ky)       // Keys -> Sharp DX
        {
            Key k = sharptokeys.FirstOrDefault(x => x.Value == ky).Key; // if not found, returns enum 0, or Key.Unknown!
            return k;
        }

        static public Keys FrontierNameToKeys(string frontiername)       // None means no translation
        {
            Key? sk = FrontierNameToSharpKey(frontiername);     // slighly long way around it, but go to sharp, then to keys.
            if (sk != null)
            {
                return SharpKeyToKeys(sk.Value);
            }
            else
                return Keys.None;
        }

        static public bool CheckTranslation(Key k, Keys winkey)     // test function just for debugging
        {
            System.Diagnostics.Debug.WriteLine("Check " + k + " vs " + winkey);
            if (sharptokeys.ContainsKey(k))
            {
                return sharptokeys[k] == winkey;
            }

            return false;
        }

        public const string FDKeys_Up = "UpArrow";          // naming as per Keys
        public const string FDKeys_Down = "DownArrow";
        public const string FDKeys_Left = "LeftArrow";
        public const string FDKeys_Right = "RightArrow";
        public const string FDKeys_Return = "Enter";
        public const string FDKeys_Capital = "CapsLock";
        public const string FDKeys_Back = "Backspace";
        public const string FDKeys_NumLock = "NumLock";
        public const string FDKeys_Subtract = "Numpad_Subtract";
        public const string FDKeys_Divide = "Numpad_Divide";
        public const string FDKeys_Multiply = "Numpad_Multiply";
        public const string FDKeys_Add = "Numpad_Add";
        public const string FDKeys_NumEnter = "Numpad_Enter";
        public const string FDKeys_Decimal = "Numpad_Decimal";
        public const string FDKeys_Quotes = "Hash";
        public const string FDKeys_Pipe = "Backslash";
        public const string FDKeys_LeftShift = "LeftShift";
        public const string FDKeys_LeftControl = "LeftControl";

        static Tuple<string, string>[] sharptofrontiername = new Tuple<string, string>[] // sharp name to Frontier Name
        {
            new Tuple<string,string>(SharpDX.DirectInput.Key.Up.ToString(),FDKeys_Up),
            new Tuple<string,string>(SharpDX.DirectInput.Key.Down.ToString(),FDKeys_Down),
            new Tuple<string,string>(SharpDX.DirectInput.Key.Left.ToString(),FDKeys_Left),
            new Tuple<string,string>(SharpDX.DirectInput.Key.Right.ToString(),FDKeys_Right),
            new Tuple<string,string>(SharpDX.DirectInput.Key.Return.ToString(),FDKeys_Return),
            new Tuple<string,string>(SharpDX.DirectInput.Key.Capital.ToString(),FDKeys_Capital),
            new Tuple<string,string>(SharpDX.DirectInput.Key.Back.ToString(),FDKeys_Back),
            new Tuple<string,string>(SharpDX.DirectInput.Key.NumberLock.ToString(),FDKeys_NumLock),
            new Tuple<string,string>(SharpDX.DirectInput.Key.Subtract.ToString(),FDKeys_Subtract),
            new Tuple<string,string>(SharpDX.DirectInput.Key.Divide.ToString(),FDKeys_Divide),
            new Tuple<string,string>(SharpDX.DirectInput.Key.Multiply.ToString(),FDKeys_Multiply),
            new Tuple<string,string>(SharpDX.DirectInput.Key.Add.ToString(),FDKeys_Add),
            new Tuple<string,string>(SharpDX.DirectInput.Key.NumberPadEnter.ToString(),FDKeys_NumEnter),
            new Tuple<string,string>(SharpDX.DirectInput.Key.Decimal.ToString(),FDKeys_Decimal),
            new Tuple<string,string>(SharpDX.DirectInput.Key.Backslash.ToString(),FDKeys_Quotes),     // new 13/11/2017
            new Tuple<string,string>(SharpDX.DirectInput.Key.Oem102.ToString(),FDKeys_Pipe),     // new 13/11/2017
            // same:
            // Apostrophe (OEM3/Tilde)
            // SemiColon (Ome1/OemSemicolon)
            // Comma, Period, Slash, LeftBracket, RightBracket, Minus, Equals, Grave, 
        };

        // See word document edcontrols for map of sharp to keys
        // manual table to go from DI Key to VKEY.. check on 11/sept/2017, rechecked 13 nov 2017
        static Dictionary<SharpDX.DirectInput.Key, System.Windows.Forms.Keys> sharptokeys = new Dictionary<Key, Keys>()
        {
            {        SharpDX.DirectInput.Key.Unknown , Keys.None},
            {        SharpDX.DirectInput.Key.Escape , Keys.Escape},
            {        SharpDX.DirectInput.Key.D1 , Keys.D1},
            {        SharpDX.DirectInput.Key.D2 , Keys.D2},
            {        SharpDX.DirectInput.Key.D3 , Keys.D3},
            {        SharpDX.DirectInput.Key.D4 , Keys.D4},
            {        SharpDX.DirectInput.Key.D5 , Keys.D5},
            {        SharpDX.DirectInput.Key.D6 , Keys.D6},
            {        SharpDX.DirectInput.Key.D7 , Keys.D7},
            {        SharpDX.DirectInput.Key.D8 , Keys.D8},
            {        SharpDX.DirectInput.Key.D9 , Keys.D9},
            {        SharpDX.DirectInput.Key.D0 , Keys.D0},
            {        SharpDX.DirectInput.Key.Minus , Keys.OemMinus},
            {        SharpDX.DirectInput.Key.Equals , Keys.Oemplus},
            {        SharpDX.DirectInput.Key.Back , Keys.Back},
            {        SharpDX.DirectInput.Key.Tab , Keys.Tab},
            {        SharpDX.DirectInput.Key.Q , Keys.Q},
            {        SharpDX.DirectInput.Key.W , Keys.W},
            {        SharpDX.DirectInput.Key.E , Keys.E},
            {        SharpDX.DirectInput.Key.R , Keys.R},
            {        SharpDX.DirectInput.Key.T , Keys.T},
            {        SharpDX.DirectInput.Key.Y , Keys.Y},
            {        SharpDX.DirectInput.Key.U , Keys.U},
            {        SharpDX.DirectInput.Key.I , Keys.I},
            {        SharpDX.DirectInput.Key.O , Keys.O},
            {        SharpDX.DirectInput.Key.P , Keys.P},
            {        SharpDX.DirectInput.Key.LeftBracket , Keys.OemOpenBrackets},
            {        SharpDX.DirectInput.Key.RightBracket , Keys.OemCloseBrackets},
            {        SharpDX.DirectInput.Key.Return , Keys.Return},
            {        SharpDX.DirectInput.Key.LeftControl , Keys.ControlKey},
            {        SharpDX.DirectInput.Key.A , Keys.A},
            {        SharpDX.DirectInput.Key.S , Keys.S},
            {        SharpDX.DirectInput.Key.D , Keys.D},
            {        SharpDX.DirectInput.Key.F , Keys.F},
            {        SharpDX.DirectInput.Key.G , Keys.G},
            {        SharpDX.DirectInput.Key.H , Keys.H},
            {        SharpDX.DirectInput.Key.J , Keys.J},
            {        SharpDX.DirectInput.Key.K , Keys.K},
            {        SharpDX.DirectInput.Key.L , Keys.L},
            {        SharpDX.DirectInput.Key.Semicolon , Keys.OemSemicolon},
            {        SharpDX.DirectInput.Key.Apostrophe , Keys.Oemtilde},
            {        SharpDX.DirectInput.Key.Grave , Keys.Oem8 },
            {        SharpDX.DirectInput.Key.LeftShift , Keys.ShiftKey},
            {        SharpDX.DirectInput.Key.Backslash , Keys.OemQuotes},       // NOT SURE
            {        SharpDX.DirectInput.Key.Z , Keys.Z},
            {        SharpDX.DirectInput.Key.X , Keys.X},
            {        SharpDX.DirectInput.Key.C , Keys.C},
            {        SharpDX.DirectInput.Key.V , Keys.V},
            {        SharpDX.DirectInput.Key.B , Keys.B},
            {        SharpDX.DirectInput.Key.N , Keys.N},
            {        SharpDX.DirectInput.Key.M , Keys.M},
            {        SharpDX.DirectInput.Key.Comma , Keys.Oemcomma},
            {        SharpDX.DirectInput.Key.Period , Keys.OemPeriod},
            {        SharpDX.DirectInput.Key.Slash , Keys.OemQuestion},
            {        SharpDX.DirectInput.Key.RightShift , Keys.RShiftKey},
            {        SharpDX.DirectInput.Key.Multiply , Keys.Multiply},
            {        SharpDX.DirectInput.Key.LeftAlt , Keys.Menu},
            {        SharpDX.DirectInput.Key.Space , Keys.Space},
            {        SharpDX.DirectInput.Key.Capital , Keys.Capital},
            {        SharpDX.DirectInput.Key.F1 , Keys.F1},
            {        SharpDX.DirectInput.Key.F2 , Keys.F2},
            {        SharpDX.DirectInput.Key.F3 , Keys.F3},
            {        SharpDX.DirectInput.Key.F4 , Keys.F4},
            {        SharpDX.DirectInput.Key.F5 , Keys.F5},
            {        SharpDX.DirectInput.Key.F6 , Keys.F6},
            {        SharpDX.DirectInput.Key.F7 , Keys.F7},
            {        SharpDX.DirectInput.Key.F8 , Keys.F8},
            {        SharpDX.DirectInput.Key.F9 , Keys.F9},
            {        SharpDX.DirectInput.Key.F10 , Keys.F10},
            {        SharpDX.DirectInput.Key.NumberLock , Keys.NumLock},
            {        SharpDX.DirectInput.Key.ScrollLock , Keys.Scroll},
            {        SharpDX.DirectInput.Key.NumberPad7 , Keys.NumPad7  },
            {        SharpDX.DirectInput.Key.NumberPad8 , Keys.NumPad8  },
            {        SharpDX.DirectInput.Key.NumberPad9 , Keys.NumPad9},
            {        SharpDX.DirectInput.Key.Subtract , Keys.Subtract},
            {        SharpDX.DirectInput.Key.NumberPad4 , Keys.NumPad4},
            {        SharpDX.DirectInput.Key.NumberPad5 , Keys.NumPad5},
            {        SharpDX.DirectInput.Key.NumberPad6 , Keys.NumPad6},
            {        SharpDX.DirectInput.Key.Add , Keys.Add },
            {        SharpDX.DirectInput.Key.NumberPad1 , Keys.NumPad1},
            {        SharpDX.DirectInput.Key.NumberPad2 , Keys.NumPad2},
            {        SharpDX.DirectInput.Key.NumberPad3 , Keys.NumPad3},
            {        SharpDX.DirectInput.Key.NumberPad0 , Keys.NumPad0},
            {        SharpDX.DirectInput.Key.Decimal , Keys.Decimal },
            {        SharpDX.DirectInput.Key.Oem102 , Keys.OemPipe },
            {        SharpDX.DirectInput.Key.F11 , Keys.F11},
            {        SharpDX.DirectInput.Key.F12 , Keys.F12},
            {        SharpDX.DirectInput.Key.F13 , Keys.F13},
            {        SharpDX.DirectInput.Key.F14 , Keys.F14},
            {        SharpDX.DirectInput.Key.F15 , Keys.F15},
            {        SharpDX.DirectInput.Key.Kana , Keys.KanaMode },
            {        SharpDX.DirectInput.Key.AbntC1 , Keys.None },
            {        SharpDX.DirectInput.Key.Convert , Keys.None },
            {        SharpDX.DirectInput.Key.NoConvert , Keys.None },
            {        SharpDX.DirectInput.Key.Yen , Keys.None },
            {        SharpDX.DirectInput.Key.AbntC2 , Keys.None },
            {        SharpDX.DirectInput.Key.NumberPadEquals , Keys.None },
            {        SharpDX.DirectInput.Key.PreviousTrack ,  Keys.MediaPreviousTrack },
            {        SharpDX.DirectInput.Key.AT , Keys.None },
            {        SharpDX.DirectInput.Key.Colon , Keys.None },
            {        SharpDX.DirectInput.Key.Underline , Keys.None },
            {        SharpDX.DirectInput.Key.Kanji , Keys.KanjiMode },
            {        SharpDX.DirectInput.Key.Stop , Keys.MediaStop },
            {        SharpDX.DirectInput.Key.AX , Keys.None },
            {        SharpDX.DirectInput.Key.Unlabeled , Keys.None },
            {        SharpDX.DirectInput.Key.NextTrack , Keys.MediaNextTrack },
            {        SharpDX.DirectInput.Key.NumberPadEnter , KeyObjectExtensions.NumEnter },
            {        SharpDX.DirectInput.Key.RightControl , Keys.RControlKey },
            {        SharpDX.DirectInput.Key.Mute , Keys.VolumeMute },
            {        SharpDX.DirectInput.Key.Calculator , Keys.None },
            {        SharpDX.DirectInput.Key.PlayPause , Keys.Pause },
            {        SharpDX.DirectInput.Key.MediaStop , Keys.MediaStop },
            {        SharpDX.DirectInput.Key.VolumeDown , Keys.VolumeDown },
            {        SharpDX.DirectInput.Key.VolumeUp , Keys.VolumeUp },
            {        SharpDX.DirectInput.Key.WebHome , Keys.None },
            {        SharpDX.DirectInput.Key.NumberPadComma , Keys.None },
            {        SharpDX.DirectInput.Key.Divide , Keys.Divide },
            {        SharpDX.DirectInput.Key.PrintScreen , Keys.PrintScreen },
            {        SharpDX.DirectInput.Key.RightAlt , Keys.RMenu },
            {        SharpDX.DirectInput.Key.Pause , Keys.Pause },
            {        SharpDX.DirectInput.Key.Home , Keys.Home },
            {        SharpDX.DirectInput.Key.Up , Keys.Up },
            {        SharpDX.DirectInput.Key.PageUp , Keys.PageUp },
            {        SharpDX.DirectInput.Key.Left , Keys.Left },
            {        SharpDX.DirectInput.Key.Right , Keys.Right },
            {        SharpDX.DirectInput.Key.End , Keys.End },
            {        SharpDX.DirectInput.Key.Down , Keys.Down },
            {        SharpDX.DirectInput.Key.PageDown , Keys.PageDown },
            {        SharpDX.DirectInput.Key.Insert , Keys.Insert },
            {        SharpDX.DirectInput.Key.Delete , Keys.Delete },
            {        SharpDX.DirectInput.Key.LeftWindowsKey , Keys.LWin },
            {        SharpDX.DirectInput.Key.RightWindowsKey , Keys.RWin },
            {        SharpDX.DirectInput.Key.Applications , Keys.Apps },
            {        SharpDX.DirectInput.Key.Power , Keys.None },
            {        SharpDX.DirectInput.Key.Sleep , Keys.Sleep },
            {        SharpDX.DirectInput.Key.Wake , Keys.None },
            {        SharpDX.DirectInput.Key.WebSearch , Keys.BrowserSearch},
            {        SharpDX.DirectInput.Key.WebFavorites , Keys.BrowserFavorites },
            {        SharpDX.DirectInput.Key.WebRefresh , Keys.BrowserRefresh },
            {        SharpDX.DirectInput.Key.WebStop , Keys.BrowserStop },
            {        SharpDX.DirectInput.Key.WebForward , Keys.BrowserForward },
            {        SharpDX.DirectInput.Key.WebBack , Keys.BrowserBack},
            {        SharpDX.DirectInput.Key.MyComputer , Keys.None },
            {        SharpDX.DirectInput.Key.Mail , Keys.None },
            {        SharpDX.DirectInput.Key.MediaSelect , Keys.None },
        };
    }


    public class InputDeviceKeyboard : InputDeviceInterface
    {
        public InputDeviceIdentity ID() { return ksi; }
        InputDeviceIdentity ksi;

        SharpDX.DirectInput.Keyboard keyboard;

        System.Threading.AutoResetEvent eventhandle = new System.Threading.AutoResetEvent(false);       // used by joy to signal data
        public System.Threading.AutoResetEvent Eventhandle() { return eventhandle; }

        public InputDeviceKeyboard(DirectInput di,DeviceInstance d)
        {
            // those silly foreign people call keyboard something other than it in english, so we need to fix it to english

            ksi = new InputDeviceIdentity() { Instanceguid = d.InstanceGuid, Productguid = d.ProductGuid, Name = "Keyboard"};

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

        public string EventName(InputDeviceEvent e) // need to return frontier naming convention!
        {
            Key k = (Key)(e.EventNumber);
            return KeyConversion.SharpKeyToFrontierName(k);
        }

        public bool? IsPressed(string frontierkeyname )      // frontier naming convention..  GetEvents must have filled in ks
        {
            Key? k = KeyConversion.FrontierNameToSharpKey(frontierkeyname);

            if ( k != null )
                return (ks != null) ? ks.IsPressed(k.Value) : false;
            else
            {
                System.Diagnostics.Debug.WriteLine("FAILED IsPressed " + frontierkeyname);
            }

            return false;
        }

        public bool IsDIPressed(Key k, bool recheck = false) // check. Optional rescan or use GetEvents
        {
            if (recheck || ks == null)
                ks = keyboard.GetCurrentState();

            return ks.IsPressed(k);
        }

        public bool IsKeyPressed(Keys k, bool recheck = false) // check. Optional rescan or use GetEvents. Needs a diff name from above for some reason
        {
            if (recheck || ks == null)
                ks = keyboard.GetCurrentState();

            Key ky = KeyConversion.KeysToSharpKey(k);
            return ks.IsPressed(ky);
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
                InputDeviceKeyboard k = new InputDeviceKeyboard(dinput, di);
                ilist.Add(k);
            }
        }

        public static InputDeviceKeyboard CreateKeyboard()      // direct keyboard make, not part of elite UI
        {
            DirectInput dinput = new DirectInput();

            foreach (DeviceInstance di in dinput.GetDevices(DeviceClass.Keyboard, DeviceEnumerationFlags.AttachedOnly))
            {
                InputDeviceKeyboard k = new InputDeviceKeyboard(dinput, di);
                return k;
            }

            return null;
        }

        static public System.Windows.Forms.Keys ToKeys(InputDeviceEvent ev) // safe to call without including SharpDirectInput
        {
            return KeyConversion.SharpKeyToKeys((Key)ev.EventNumber);
        }

        static public string SharpKeyName(InputDeviceEvent ev) // safe to call without including SharpDirectInput
        {
            return ((Key)ev.EventNumber).ToString();
        }

        static public bool CheckTranslation(InputDeviceEvent ev, Keys winkey) // safe to call without including SharpDirectInput
        {
            return KeyConversion.CheckTranslation((Key)ev.EventNumber, winkey);
        }
    }
}
