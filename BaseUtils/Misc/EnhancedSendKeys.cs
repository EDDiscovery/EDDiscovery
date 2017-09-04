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
    public class EnhancedSendKeys
    {
        public static string CurrentWindow = "Current window";

        static EnhancedSendKeys()
        {
        }

        private EnhancedSendKeys()
        {
        }

        private static Queue events;

        private class SKEvent
        {
            internal int wm;
            internal short vkey;      
            internal short sc;
            internal bool extkey;
            internal int delay;     // key delay

            public SKEvent(int wma, Keys vk , int del)
            {
                wm = wma;
                vkey = (short)((vk == KeyObjectExtensions.NumEnter) ? Keys.Return : vk);
                if (vk == Keys.RShiftKey)
                    sc = 0x36;      // force diff scan code
                else
                    sc = (short)BaseUtils.Win32.UnsafeNativeMethods.MapVirtualKey((uint)vkey, 0);
                extkey = ((Keys)vk).IsExtendedKey();
                delay = del;
                System.Diagnostics.Debug.WriteLine("Queue " + wma + " : " + vk.VKeyToString() + " " + sc + " " + extkey + " " + delay + "ms");
            }
        }

        private static void AddEvent(SKEvent skevent)
        {
            if (events == null)
            {
                events = new Queue();
            }
            events.Enqueue(skevent);
        }

        private static void AddMsgsForVK(Keys vk, bool altnoctrldown , int downdel , int updel , KMode kmd)
        {
            if (kmd == KMode.press || kmd == KMode.down)
                AddEvent(new SKEvent(altnoctrldown ? BaseUtils.Win32Constants.WM.SYSKEYDOWN : BaseUtils.Win32Constants.WM.KEYDOWN, vk , downdel));

            if (kmd == KMode.press || kmd == KMode.up)
                AddEvent(new SKEvent( BaseUtils.Win32Constants.WM.KEYUP, vk , updel));  // key up has a short nominal delay
        }

        enum KMode { press, up, down };

        public static string ParseKeys(string s, int defdelay , int defshiftdelay , int defupdelay)
        {
            //debugevents = null;
            s = s.Trim();
            IntPtr hwnd = (IntPtr)0;

            while (s.Length > 0)
            {
                KMode kmd = KMode.press;

                int d1 = -1, d2 = -1, d3 = -1;

                if (s[0] == '[')
                {
                    s = s.Substring(1);
                    string word = ObjectExtensionsStrings.FirstWord(ref s, new char[] { ']' , ',' });
                    if (!word.InvariantParse(out d1))
                        return "Delay not properly given";

                    if (s[0] == ',')
                    {
                        s = s.Substring(1);
                        word = ObjectExtensionsStrings.FirstWord(ref s, new char[] { ']' , ',' });
                        if (!word.InvariantParse(out d2))
                            return "Second Delay not properly given";
                    }

                    if (s[0] == ',')
                    {
                        s = s.Substring(1);
                        word = ObjectExtensionsStrings.FirstWord(ref s, new char[] { ']' });
                        if (!word.InvariantParse(out d3))
                            return "Third Delay not properly given";
                    }

                    if (s[0] == ']')
                        s = s.Substring(1);
                    else
                        return "Missing closing ] in delay";
                }

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

                        if ( ctrl != Keys.None)
                            s = s.Skip("+");
                    }
                }

                bool mainpart = s.Length > 0 && s[0] != ' ';

                // keydown is d1 or def
                int keydowndelay = (d1 != -1) ? d1 : defdelay;                          
                // if mainpart present, its d2 or defshift.  If no main part, its d1 or def shift
                int shiftdelay = (mainpart) ? (d2 != -1 ? d2 : defshiftdelay) : (d1!=-1 ? d1 : defshiftdelay);
                // if in up/down mode, its d1 or def up.   If its got a main part, its d3/defup.  else its d2/defup
                int keyupdelay = (kmd == KMode.up || kmd == KMode.down) ? (d1!=-1 ? d1 : defupdelay) : (mainpart ? (d3 != -1 ? d3: defupdelay) : (d2 != -1 ? d2 : defupdelay));

                System.Diagnostics.Debug.WriteLine(string.Format("{0} {1} {2} {3} {4} {5} ", d1, d2, d3, keydowndelay, shiftdelay, keyupdelay));

                if (shift != Keys.None)         // we already run shift keys here. If we are doing UP, we send a up, else we are doing down/press
                    AddEvent(new SKEvent(kmd == KMode.up ? BaseUtils.Win32Constants.WM.KEYUP : BaseUtils.Win32Constants.WM.KEYDOWN, shift, shiftdelay));

                if (ctrl != Keys.None)
                    AddEvent(new SKEvent(kmd == KMode.up ? BaseUtils.Win32Constants.WM.KEYUP : BaseUtils.Win32Constants.WM.KEYDOWN, ctrl, shiftdelay));

                if (alt != Keys.None)
                    AddEvent(new SKEvent(kmd == KMode.up ? BaseUtils.Win32Constants.WM.SYSKEYUP: BaseUtils.Win32Constants.WM.SYSKEYDOWN, alt, shiftdelay));

                if (mainpart)
                {
                    if (s.Length == 0)
                        return "Invalid no characters after shifters";

                    bool brackets = ObjectExtensionsStrings.IsPrefix(ref s, "(");

                    while( s.Length>0 )
                    {
                        string word = ObjectExtensionsStrings.FirstWord(ref s, new char[] { ' ', ')' });

                        Keys key = word.ToVkey();

                        if (key != Keys.None)
                        {
                            AddMsgsForVK(key, alt != Keys.None && ctrl == Keys.None, keydowndelay, keyupdelay , kmd);
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
                                    AddMsgsForVK(key, alt != Keys.None && ctrl == Keys.None, keydowndelay , keyupdelay, kmd);
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
                        AddEvent(new SKEvent(BaseUtils.Win32Constants.WM.SYSKEYUP, alt, keyupdelay));

                    if (ctrl != Keys.None)
                        AddEvent(new SKEvent(BaseUtils.Win32Constants.WM.KEYUP, ctrl, keyupdelay));

                    if (shift != Keys.None)
                        AddEvent(new SKEvent(BaseUtils.Win32Constants.WM.KEYUP, shift, keyupdelay));
                }

                s = s.Trim();
                if (s.Length > 0 && s[0] == ',')        // comma can be used between key groups
                    s = s.Substring(1).TrimStart();
            }

            return "";
        }

        // Uses User32 SendInput to send keystrokes
        private static void SendInput(byte[] oldKeyboardState ) //, Queue previousEvents)
        {
            NativeMethods.INPUT[] currentInput = new NativeMethods.INPUT[1];

            // all events are Keyboard events
            currentInput[0].type = NativeMethods.INPUT_KEYBOARD;

            // initialize unused members
            currentInput[0].inputUnion.ki.dwExtraInfo = IntPtr.Zero;
            currentInput[0].inputUnion.ki.time = 0;

            // send each of our SKEvents using SendInput
            int INPUTSize = Marshal.SizeOf(typeof(NativeMethods.INPUT));

            // need these outside the lock below
            uint eventsSent = 0;
            int eventsTotal;

            // A lock here will allow multiple threads to SendInput at the same time.
            lock (events.SyncRoot)
            {
                // block keyboard and mouse input events from reaching applications.
                bool blockInputSuccess = UnsafeNativeMethods.BlockInput(true);

                try
                {
                    eventsTotal = events.Count;

                    for (int i = 0; i < eventsTotal; i++)
                    {
                        SKEvent skEvent = (SKEvent)events.Dequeue();

                        currentInput[0].inputUnion.ki.dwFlags = 0;

                        // just need to send currentInput[0] for skEvent

                        currentInput[0].inputUnion.ki.wScan = skEvent.sc;

                        // add KeyUp flag if we have a KeyUp
                        if (skEvent.wm == BaseUtils.Win32Constants.WM.KEYUP || skEvent.wm == BaseUtils.Win32Constants.WM.SYSKEYUP)
                        {
                            currentInput[0].inputUnion.ki.dwFlags |= NativeMethods.KEYEVENTF_KEYUP;
                        }

                        // Sets KEYEVENTF_EXTENDEDKEY flag if necessary
                        if ( skEvent.extkey )
                        {
                            currentInput[0].inputUnion.ki.dwFlags |= NativeMethods.KEYEVENTF_EXTENDEDKEY;
                        }

                        currentInput[0].inputUnion.ki.wVk = skEvent.vkey;

                        //System.Diagnostics.Debug.WriteLine("Send " + currentInput[0].inputUnion.ki.wVk + " " + currentInput[0].inputUnion.ki.wScan.ToString("2X") + " " + currentInput[0].inputUnion.ki.dwFlags);
                        // send only currentInput[0]
                        eventsSent += UnsafeNativeMethods.SendInput(1, currentInput, INPUTSize);

                        System.Threading.Thread.Sleep( skEvent.delay>0 ? skEvent.delay : 1);
                    }
                }
                finally
                {
                    SetKeyboardState(oldKeyboardState);

                    // unblock input if it was previously blocked
                    if (blockInputSuccess)
                    {
                        UnsafeNativeMethods.BlockInput(false);
                    }
                }
            }

            // check to see if we sent the number of events we're supposed to
            if (eventsSent != eventsTotal)
            {
                // calls Marshal.GetLastWin32Error and sets it in the exception
                throw new Win32Exception();
            }
        }

        private static byte[] GetKeyboardState()
        {
            byte[] keystate = new byte[256];
            UnsafeNativeMethods.GetKeyboardState(keystate);
            return keystate;
        }

        private static void SetKeyboardState(byte[] keystate)
        {
            UnsafeNativeMethods.SetKeyboardState(keystate);
        }

        public static string Send(string keys, int keydelay, int shiftdelay , int updelay, string pname = null)
        {
            if (!keys.HasChars())
                return "";

            string err = ParseKeys(keys,keydelay, shiftdelay , updelay);
            if (err != "")
                return err;

            if (events == null)
                return "";

            byte[] oldstate = GetKeyboardState();

            Process p = null;

            IntPtr currentfore = (IntPtr)0;

            if (pname.HasChars() && !pname.Equals(CurrentWindow, StringComparison.InvariantCultureIgnoreCase) && pname.Length > 0)
            {
                p = Process.GetProcessesByName(pname).FirstOrDefault();

                if (p != null)
                {
                    currentfore = BaseUtils.Win32.UnsafeNativeMethods.GetForegroundWindow();
                    //System.Diagnostics.Debug.WriteLine("Current fore" + currentfore + " " + p.MainWindowHandle);

                    BaseUtils.Win32.UnsafeNativeMethods.SetForegroundWindow(p.MainWindowHandle);
                }
                else
                    return "No such process";
            }

            SendInput(oldstate);

            if ( p != null )
            {
                BaseUtils.Win32.UnsafeNativeMethods.SetForegroundWindow(currentfore);
            }

            return "";
        }

    }
}
