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
    static public class SharpKeyConversion
    {
        // Sharp Keys are Scan codes for codes 1-0x36, 
        // 37,38,39,3a,3b-44
        // 45,46,47,48,49,4a,4b-4d,4e,4f-53, 57,58, 
        // 9c = NUMPADENTER (0x80 Extended + 1C = numenter)
        // 9D = RCONTROL = (0x80|1D)
        // B5 = NUMDIVIDE = (0x80|35)
        // B8 = RMENU (0x80|38)
        // 0xC7 	DIK_HOME 	Home 	80 | 47
        // 0xC8 	DIK_UP 	↑ 	        80 | 48
        // 0xC9 	DIK_PRIOR Page Up 	80 | 49
        // 0xCB 	DIK_LEFT 	← 	    80 | 4b
        // 0xCD 	DIK_RIGHT 	→ 	    80 | 4d
        // 0xCF 	DIK_END End 	    80 | 4F
        // 0xD0 	DIK_DOWN 	↓ 	    80 | 50
        // 0xD1 	DIK_NEXT Page Down 	80 | 51
        // 0xD2 	DIK_INSERT Insert 	80 | 52
        // 0xD3 	DIK_DELETE Delete   80 | 53

        static public System.Windows.Forms.Keys SharpKeyToKeys(SharpDX.DirectInput.Key k)        // Sharp DX - > Windows Keys
        {
            if (sharptokeys.ContainsKey(k))     // NEED to manually cope with this.. MapVirtualKey is very limited
            {
                //System.Diagnostics.Debug.WriteLine("Direct Sharp to Keys {0:x} -> vkey {1:x}", k, sharptokeys[k]);
                return sharptokeys[k];
            }

            uint v = BaseUtils.Win32.UnsafeNativeMethods.MapVirtualKey((uint)k & 0x7f, 3);  // otherwise, the sharp key is the scan code.. use this.
            //System.Diagnostics.Debug.WriteLine("Sharp to Keys {0:x} -> vkey {1:x}", k, v);

            if (v > 0)
                return (Keys)v;
            else
                return Keys.None;
        }

        static public SharpDX.DirectInput.Key KeysToSharpKey(System.Windows.Forms.Keys ky)       // Keys -> Sharp DX
        {
            Key k = sharptokeys.FirstOrDefault(x => x.Value == ky).Key; // if not found, returns enum 0, or Key.Unknown!
            if ( k == Key.Unknown)
            {
                uint v = BaseUtils.Win32.UnsafeNativeMethods.MapVirtualKey((uint)ky, 0);  // otherwise, vkey to scan code
                //System.Diagnostics.Debug.WriteLine("Keys to sharp {0:x} -> vkey {1:x}", ky, v);
                if (v > 0)
                    k = (Key)v; // SC is the DI key
            }
            return k;
        }

        // checked on USA/UK/FR 3/01/18

        static Dictionary<SharpDX.DirectInput.Key, System.Windows.Forms.Keys> sharptokeys = new Dictionary<Key, Keys>()
        {
            {        SharpDX.DirectInput.Key.Unknown , Keys.None  },
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


    public class InputDeviceKeyboard : IInputDevice
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

        public List<InputDeviceEvent> GetEvents()       // Events use keys enumeration
        {
            ks = keyboard.GetCurrentState();
            KeyboardUpdate[] ke = keyboard.GetBufferedData();

            List<InputDeviceEvent> events = new List<InputDeviceEvent>();
            foreach (KeyboardUpdate k in ke)
            {
                Keys ky = SharpKeyConversion.SharpKeyToKeys(k.Key);
                //System.Diagnostics.Debug.WriteLine("** Sharp key " + k.Key + " " + (int)k.Key + k.IsPressed);
                //System.Diagnostics.Debug.WriteLine( "      => " + ky.ToString() + " norm " + ky.VKeyToString() + ":" + (int)ky );
                events.Add(new InputDeviceEvent(this, (int)ky, k.IsPressed));
            }

            return (events.Count > 0) ? events : null;
        }

        public string EventName(InputDeviceEvent e)     // in Forms.keys naming convention - not in sharp DX.
        {
            Keys k = (Keys)(e.EventNumber);
            return k.VKeyToString();
        }

        public bool? IsPressed( string keyname )        // keyname is in keys
        {
            Key? k = SharpKeyConversion.KeysToSharpKey(keyname.ToVkey());       // to VKEY, then to Sharp key

            if ( k != null )
                return (ks != null) ? ks.IsPressed(k.Value) : false;        // use keyboard state to determine
            else
            {
                System.Diagnostics.Debug.WriteLine("FAILED IsPressed " + keyname);
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

            Key ky = SharpKeyConversion.KeysToSharpKey(k);
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
            return (Keys)ev.EventNumber;
        }

        static public bool CheckTranslation(InputDeviceEvent ev) // safe to call without including SharpDirectInput Debug use
        {
            Keys ky = ToKeys(ev);
            Key sk = SharpKeyConversion.KeysToSharpKey(ky);
            Keys back = SharpKeyConversion.SharpKeyToKeys(sk);
            System.Diagnostics.Debug.WriteLine("Check " + ky.VKeyToString() + " -> " + sk + " ->" + back.VKeyToString());
            return ky == back;
        }
    }
}
