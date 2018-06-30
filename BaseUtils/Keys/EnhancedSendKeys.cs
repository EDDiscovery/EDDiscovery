/*
 * Copyright © 2016 EDDiscovery development team
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
 
using BaseUtils.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ObjectExtensionsStrings;

// syntax: [<delay>][<prefix>] [<shifters>] [<add>] [ seq | '('seq [' ' seq].. ')']
// delay = '[' d1 [',' d2 [ ',' d3 ] ] ']' in decimal ms.
//          d1 applies to main vkey sequence, or to shifters if no vkey sequence is given
//          d2 applies to shifters when vkey sequence is given, or to updelay when no vkey sequence is given
//          updelay applies to presses, or to shifters when combined with vkey sequence
// prefix = one of '^' '<' : up    
// prefix = one of '!' '>' : down  . No prefix indicates press (down then up)
// shifters = '' | 'Shift' | 'Shift+Alt' | 'Shift+Alt+Ctrl' | 'Alt' | 'Alt+Ctrl' | 'Ctrl'
// add = '' or '+' when both shifters and vkey is present
// seq = vkey | shortlist. Multiple sequences can be given, space seperated, inside the ()
// vkey = Vkey name, See KeyObjectExtensions.ToVkey.  NumEnter is principle addition, D0-9 are just 0-9.
// shortlist = '0'-'9' | 'A'-'Z' | 'a'-'z'.  List of Vkeys in this range.
//
// Examples : Shift+Ctrl, Shift, Alt, Alt+A, Shift+Alt+A, Shift+(ABC), Shift+(F1 BC), ^Shift+(F1 BC)
// [100,20]Shift+A

namespace BaseUtils
{
    static public class EnhancedSendKeysParser
    {
        public interface IAdditionalKeyParser
        {
            Tuple<string, int, string> Parse(string s);      // return replace key string, or null if not recognised.  int is parse length, Any errors signal in second string or null
        }

        public class SKEvent
        {
            internal int wm;        // windows message code
            internal Keys vkey;    // vkey  
            internal short sc;      // scan code
            internal bool extkey;   // is it an extended key code
            internal int delay;     // key delay

            public SKEvent(int wma, Keys vk, int del)
            {
                wm = wma;
                vkey = (vk == KeyObjectExtensions.NumEnter) ? Keys.Return : vk;
                if (vk == Keys.RShiftKey)
                    sc = 0x36;      // force diff scan code
                else
                    sc = (short)BaseUtils.Win32.UnsafeNativeMethods.MapVirtualKey((uint)vkey, 0);
                extkey = ((Keys)vk).IsExtendedKey();
                delay = del;
                //System.Diagnostics.Debug.WriteLine("Queue " + wm + " : " + vk.VKeyToString() + " " + sc + " " + extkey + " " + delay + "ms");
            }

            public override string ToString()
            {
                return "Queue " + wm + " : " + ((Keys)vkey).VKeyToString() + " " + sc + " " + extkey + " " + delay + "ms";
            }
        }

        private static void AddMsgsForVK(Queue<SKEvent> events, Keys vk, bool altnoctrldown, int downdel, int updel, KMode kmd)
        {
            if (kmd == KMode.press || kmd == KMode.down)
                events.Enqueue(new SKEvent(altnoctrldown ? BaseUtils.Win32Constants.WM.SYSKEYDOWN : BaseUtils.Win32Constants.WM.KEYDOWN, vk, downdel));

            if (kmd == KMode.press || kmd == KMode.up)
                events.Enqueue(new SKEvent(BaseUtils.Win32Constants.WM.KEYUP, vk, updel));  // key up has a short nominal delay
        }

        enum KMode { press, up, down };

        public static string ParseKeys(Queue<SKEvent> events, string s, int defdelay, int defshiftdelay, int defupdelay, IAdditionalKeyParser additionalkeyparser = null)
        {
            //debugevents = null;
            s = s.Trim();
            IntPtr hwnd = (IntPtr)0;

            while (s.Length > 0)
            {
                if (additionalkeyparser != null)                               // See if key needs translating out - moved to here to allow for control sequences before this key
                {
                    Tuple<string, int, string> t = additionalkeyparser.Parse(s);      // Allow the parser to sniff the string

                    if (t.Item3 != null)                                        // error condition here, such as no matching key binding
                        return t.Item3;

                    if (t.Item1 != null)                                      // if replace.. (and the parser can return multiple keys)
                    {
                        s = t.Item1 + " " + s.Substring(t.Item2);               // its the replace string, followed by the cut out current string
                    }
                }

                int d1 = -1, d2 = -1, d3 = -1;

                if (s.Length >= 1 && s[0] == '[')
                {
                    if (!s.Contains(']'))
                        return "Missing closing ] in delay";

                    s = s.Substring(1);
                    string word = ObjectExtensionsStrings.FirstWord(ref s, new char[] { ']', ',' });
                    if (!word.InvariantParse(out d1))
                        return "Delay not properly given";

                    if (s.Length >= 1 && s[0] == ',')
                    {
                        s = s.Substring(1);
                        word = ObjectExtensionsStrings.FirstWord(ref s, new char[] { ']', ',' });
                        if (!word.InvariantParse(out d2))
                            return "Second Delay not properly given";
                    }

                    if (s.Length >= 1 && s[0] == ',')
                    {
                        s = s.Substring(1);
                        word = ObjectExtensionsStrings.FirstWord(ref s, new char[] { ']' });
                        if (!word.InvariantParse(out d3))
                            return "Third Delay not properly given";
                    }

                    if (s.Length >= 1 && s[0] == ']')
                        s = s.Substring(1);
                    else
                        return "Missing closing ] in delay";
                }

                KMode kmd = KMode.press;

                if (s.Length == 0)
                    return "Invalid no characters after delay";

                if (s[0] == '^' || s[0] == '<')
                {
                    kmd = KMode.up;
                    s = s.Substring(1);
                }
                else if (s[0] == '!' || s[0] == '>')
                {
                    kmd = KMode.down;
                    s = s.Substring(1);
                }

                if (additionalkeyparser != null)                               // Also see here if key needs translating out - 9.0.3.0
                {
                    Tuple<string, int, string> t = additionalkeyparser.Parse(s);      // Allow the parser to sniff the string

                    if (t.Item3 != null)                                        // error condition here, such as no matching key binding
                        return t.Item3;

                    if (t.Item1 != null)                                      // if replace.. (and the parser can return multiple keys)
                    {
                        s = t.Item1 + " " + s.Substring(t.Item2);               // its the replace string, followed by the cut out current string
                    }
                }

                Keys shift = KeyObjectExtensions.IsShiftPrefix(ref s);
                Keys ctrl = Keys.None;
                Keys alt = Keys.None;
                if (shift == Keys.None || s.StartsWith("+"))
                {
                    s = s.Skip("+");

                    alt = KeyObjectExtensions.IsAltPrefix(ref s);

                    if (alt == Keys.None || s.StartsWith("+"))
                    {
                        s = s.Skip("+");

                        ctrl = KeyObjectExtensions.IsCtrlPrefix(ref s);

                        if (ctrl != Keys.None)
                            s = s.Skip("+");
                    }
                }

                bool mainpart = s.Length > 0 && s[0] != ' ';

                // keydown is d1 or def
                int keydowndelay = (d1 != -1) ? d1 : defdelay;
                // if mainpart present, its d2 or defshift.  If no main part, its d1 or def shift
                int shiftdelay = (mainpart) ? (d2 != -1 ? d2 : defshiftdelay) : (d1 != -1 ? d1 : defshiftdelay);
                // if in up/down mode, its d1 or def up.   If its got a main part, its d3/defup.  else its d2/defup
                int keyupdelay = (kmd == KMode.up || kmd == KMode.down) ? (d1 != -1 ? d1 : defupdelay) : (mainpart ? (d3 != -1 ? d3 : defupdelay) : (d2 != -1 ? d2 : defupdelay));

                //System.Diagnostics.Debug.WriteLine(string.Format("{0} {1} {2} {3} {4} {5} ", d1, d2, d3, keydowndelay, shiftdelay, keyupdelay));

                if (shift != Keys.None)         // we already run shift keys here. If we are doing UP, we send a up, else we are doing down/press
                    events.Enqueue( new SKEvent(kmd == KMode.up ? BaseUtils.Win32Constants.WM.KEYUP : BaseUtils.Win32Constants.WM.KEYDOWN, shift, shiftdelay));

                if (ctrl != Keys.None)
                    events.Enqueue(new SKEvent(kmd == KMode.up ? BaseUtils.Win32Constants.WM.KEYUP : BaseUtils.Win32Constants.WM.KEYDOWN, ctrl, shiftdelay));

                if (alt != Keys.None)
                    events.Enqueue(new SKEvent(kmd == KMode.up ? BaseUtils.Win32Constants.WM.SYSKEYUP : BaseUtils.Win32Constants.WM.SYSKEYDOWN, alt, shiftdelay));

                if (mainpart)
                {
                    if (s.Length == 0)
                        return "Invalid no characters after shifters";

                    bool brackets = ObjectExtensionsStrings.IsPrefix(ref s, "(");

                    while (s.Length > 0)
                    {
                        string word = ObjectExtensionsStrings.FirstWord(ref s, new char[] { ' ', ')' });

                        Keys key = word.ToVkey();

                        if (key != Keys.None)
                        {
                            AddMsgsForVK(events, key, alt != Keys.None && ctrl == Keys.None, keydowndelay, keyupdelay, kmd);
                            //System.Diagnostics.Debug.WriteLine(shift + " " + alt + " " + ctrl + "  press " + key.VKeyToString());
                        }
                        else
                        {
                            while (word.Length > 0)
                            {
                                string ch = new string(word[0], 1);
                                key = ch.ToVkey();

                                if (key.IsSingleCharName())
                                {
                                    AddMsgsForVK(events, key, alt != Keys.None && ctrl == Keys.None, keydowndelay, keyupdelay, kmd);
                                    //System.Diagnostics.Debug.WriteLine(shift + " " + alt + " " + ctrl + "  press " + key.VKeyToString());
                                    word = word.Substring(1);
                                }
                                else
                                    return "Invalid key " + word;
                            }
                        }

                        if (!brackets)
                            break;
                        else if (s.Length > 0 && s[0] == ')')
                        {
                            s = s.Substring(1);
                            break;
                        }
                    }
                }

                if (kmd == KMode.press)     // only on a press do we release here
                {
                    if (alt != Keys.None)
                        events.Enqueue(new SKEvent(BaseUtils.Win32Constants.WM.SYSKEYUP, alt, keyupdelay));

                    if (ctrl != Keys.None)
                        events.Enqueue(new SKEvent(BaseUtils.Win32Constants.WM.KEYUP, ctrl, keyupdelay));

                    if (shift != Keys.None)
                        events.Enqueue(new SKEvent(BaseUtils.Win32Constants.WM.KEYUP, shift, keyupdelay));
                }

                s = s.Trim();
                if (s.Length > 0 && s[0] == ',')        // comma can be used between key groups
                    s = s.Substring(1).TrimStart();
            }

            return "";
        }

        public static string VerifyKeys(string s, IAdditionalKeyParser additionalkeyparser = null)
        {
            return ParseKeys(new Queue<SKEvent>(), s, 10, 10, 10, additionalkeyparser);
        }
    }



    static public class EnhancedSendKeys
    {
        public static string CurrentWindow = "Current window";
        private static Queue<EnhancedSendKeysParser.SKEvent> events;

        public static string SendToCurrentWindow(string keys, int keydelay, int shiftdelay, int updelay, EnhancedSendKeysParser.IAdditionalKeyParser additionalkeyparser = null)
        {
            string ret = GenerateEvents(keys, keydelay, shiftdelay, updelay, additionalkeyparser);

            if (ret == "")
                SendInput();

            return ret;
        }

        public static string SendToWindow(string keys, int keydelay, int shiftdelay, int updelay, IntPtr window, EnhancedSendKeysParser.IAdditionalKeyParser additionalkeyparser = null)
        {
            string ret = GenerateEvents(keys, keydelay, shiftdelay, updelay, additionalkeyparser);

            if (ret == "")
                SendInputToWindow(window);

            return ret;
        }

        public static string SendToProcess(string keys, int keydelay, int shiftdelay, int updelay, string pname, EnhancedSendKeysParser.IAdditionalKeyParser additionalkeyparser = null)
        {
            string ret = GenerateEvents(keys, keydelay, shiftdelay, updelay, additionalkeyparser);

            if (ret == "")
            {
                if (pname.Equals(CurrentWindow, StringComparison.InvariantCultureIgnoreCase))
                    SendInput();
                else
                {
                    IntPtr window = UnsafeNativeMethods.GetForegroundWindowOf(pname);

                    if (window != (IntPtr)0)
                        SendInputToWindow(window);
                    else
                        return "Process " + pname + " is not running";
                }
            }

            return ret;
        }

        private static string GenerateEvents(string keys, int keydelay, int shiftdelay, int updelay, EnhancedSendKeysParser.IAdditionalKeyParser additionalkeyparser = null)
        {
            Queue<EnhancedSendKeysParser.SKEvent> tempqueue = new Queue<EnhancedSendKeysParser.SKEvent>();

            string err = EnhancedSendKeysParser.ParseKeys(tempqueue, keys, keydelay, shiftdelay, updelay, additionalkeyparser);
            if (err != "" || tempqueue.Count == 0)
                return err;

            if (events == null)
                events = new Queue<EnhancedSendKeysParser.SKEvent>();

            foreach (EnhancedSendKeysParser.SKEvent o in tempqueue)     // only add when ready
                events.Enqueue(o);

            return "";
        }

        private static void SendInputToWindow(IntPtr ip)
        {
            IntPtr currentfore = BaseUtils.Win32.UnsafeNativeMethods.GetForegroundWindow();

            if ( currentfore != ip )        // if not selected..
            {
                System.Diagnostics.Debug.WriteLine("Selecting fore");
                Win32.UnsafeNativeMethods.SetForegroundWindow(ip);
                System.Threading.Thread.Sleep(60);      // 50 works, 20 does not.. seems to need a pause.. lets be cautious

                System.Diagnostics.Debug.WriteLine("Go");
            }
            else
                currentfore = (IntPtr)0;       // forget it, we don't need to swap back

            SendInput();

            if (currentfore != (IntPtr)0)
            {
                System.Diagnostics.Debug.WriteLine("Reselecting prev");
                BaseUtils.Win32.UnsafeNativeMethods.SetForegroundWindow(currentfore);
            }
        }

        // Uses User32 SendInput to send keystrokes
        private static void SendInput()
        {
            byte[] keystate = new byte[256];
            UnsafeNativeMethods.GetKeyboardState(keystate);

            NativeMethods.INPUT[] currentInput = new NativeMethods.INPUT[1];

            // all events are Keyboard events
            currentInput[0].type = NativeMethods.INPUT_KEYBOARD;

            // initialize unused members
            currentInput[0].inputUnion.ki.dwExtraInfo = IntPtr.Zero;
            currentInput[0].inputUnion.ki.time = 0;

            // send each of our SKEvents using SendInput
            int INPUTSize = Marshal.SizeOf(typeof(NativeMethods.INPUT));

            // A lock here will allow multiple threads to SendInput at the same time.
            lock (events)
            {
                // block keyboard and mouse input events from reaching applications.
                bool blockInputSuccess = UnsafeNativeMethods.BlockInput(true);

                try
                {
                    int eventsTotal = events.Count;

                    for (int i = 0; i < eventsTotal; i++)
                    {
                        EnhancedSendKeysParser.SKEvent skEvent = (EnhancedSendKeysParser.SKEvent)events.Dequeue();

                        currentInput[0].inputUnion.ki.dwFlags = 0;

                        // just need to send currentInput[0] for skEvent

                        currentInput[0].inputUnion.ki.wScan = skEvent.sc;

                        // add KeyUp flag if we have a KeyUp
                        if (skEvent.wm == BaseUtils.Win32Constants.WM.KEYUP || skEvent.wm == BaseUtils.Win32Constants.WM.SYSKEYUP)
                        {
                            currentInput[0].inputUnion.ki.dwFlags |= NativeMethods.KEYEVENTF_KEYUP;
                        }

                        // Sets KEYEVENTF_EXTENDEDKEY flag if necessary
                        if (skEvent.extkey)
                        {
                            currentInput[0].inputUnion.ki.dwFlags |= NativeMethods.KEYEVENTF_EXTENDEDKEY;
                        }

                        currentInput[0].inputUnion.ki.wVk = (short)skEvent.vkey;

                        System.Diagnostics.Debug.WriteLine(Environment.TickCount % 10000 + " Send " + skEvent.vkey.VKeyToString() + " " + currentInput[0].inputUnion.ki.wScan.ToString("2X") + " " + currentInput[0].inputUnion.ki.dwFlags);
                        // send only currentInput[0]
                        UnsafeNativeMethods.SendInput(1, currentInput, INPUTSize);

                        System.Threading.Thread.Sleep(skEvent.delay > 0 ? skEvent.delay : 1);
                    }
                }
                finally
                {
                    UnsafeNativeMethods.SetKeyboardState(keystate);

                    // unblock input if it was previously blocked
                    if (blockInputSuccess)
                    {
                        UnsafeNativeMethods.BlockInput(false);
                    }
                }
            }
        }
    }
}
