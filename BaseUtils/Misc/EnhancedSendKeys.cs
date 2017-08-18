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

namespace BaseUtils
{
    public class SendExtendedKeys
    {
       
        static SendExtendedKeys()
        {
        }

        private SendExtendedKeys()
        {
        }

        private static Queue events;

        private class SKEvent
        {
            internal int wm;
            internal short vkey;      
            internal short sc;
            internal bool extkey;

            public SKEvent(int wma, Keys vk)
            {
                wm = wma;
                vkey = (short)((vk == KeyObjectExtensions.NumEnter) ? Keys.Return : vk);
                if (vk == Keys.RShiftKey)
                    sc = 0x36;      // force diff scan code
                else
                    sc = (short)BaseUtils.Win32.UnsafeNativeMethods.MapVirtualKey((uint)vkey, 0);
                extkey = ((Keys)vk).IsExtendedKey();
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

        private static void AddMsgsForVK(Keys vk, int repeat, bool altnoctrldown)
        {
            for (int i = 0; i < repeat; i++)
            {
                AddEvent(new SKEvent(altnoctrldown ? BaseUtils.Win32Constants.WM.SYSKEYDOWN : BaseUtils.Win32Constants.WM.KEYDOWN, vk));
                //AddEvent(new SKEvent(altnoctrldown ? BaseUtils.Win32Constants.WM.SYSKEYUP : BaseUtils.Win32Constants.WM.KEYUP, vk));
                AddEvent(new SKEvent( BaseUtils.Win32Constants.WM.KEYUP, vk));
            }
        }

        public static string ParseKeys(string s)
        {
            //debugevents = null;
            s = s.Trim();
            IntPtr hwnd = (IntPtr)0;

            while (s.Length > 0)
            {
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

                if (shift != Keys.None)
                    AddEvent(new SKEvent(BaseUtils.Win32Constants.WM.KEYDOWN, shift));

                if (ctrl != Keys.None)
                    AddEvent(new SKEvent(BaseUtils.Win32Constants.WM.KEYDOWN, ctrl));

                if (alt != Keys.None)
                    AddEvent(new SKEvent(BaseUtils.Win32Constants.WM.SYSKEYDOWN, alt));

                bool shifts = shift != Keys.None || ctrl != Keys.None || alt != Keys.None;
                
                if (s.Length>0 && s[0] != ' ') // we have a main part..
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
                            AddMsgsForVK(key, 1, alt != Keys.None && ctrl == Keys.None);
                            System.Diagnostics.Debug.WriteLine(shift + " " + alt + " " + ctrl + "  press " + key.VKeyToString());
                        }
                        else
                        {
                            while (word.Length > 0)
                            {
                                string ch = new string(word[0], 1);
                                key = ch.ToVkey();

                                if (key.IsSingleCharName())
                                {
                                    AddMsgsForVK(key, 1, alt != Keys.None && ctrl == Keys.None);
                                    System.Diagnostics.Debug.WriteLine(shift + " " + alt + " " + ctrl + "  press " + key.VKeyToString());
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

                if (alt != Keys.None)
                    AddEvent(new SKEvent(BaseUtils.Win32Constants.WM.SYSKEYUP, alt));

                if (ctrl != Keys.None)
                    AddEvent(new SKEvent(BaseUtils.Win32Constants.WM.KEYUP, ctrl));

                if (shift != Keys.None)
                    AddEvent(new SKEvent(BaseUtils.Win32Constants.WM.KEYUP, shift));

                s = s.Trim();
            }

            foreach (SKEvent ev in events)
            {
                System.Diagnostics.Debug.WriteLine("Event " + ev.wm + " " + ev.vkey.ToString("X4") );
            }

            return "";
        }

        // Uses User32 SendInput to send keystrokes
        private static void SendInput(byte[] oldKeyboardState ) //, Queue previousEvents)
        {
            // Should be a No-Opt most of the time
            //AddCancelModifiersForPreviousEvents(previousEvents);

            // SKEvents are sent as sent as 1 or 2 inputs
            // currentInput[0] represents the SKEvent
            // currentInput[1] is a KeyUp to prevent all identical WM_CHARs to be sent as one message
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
            // This mimics the JournalHook method of using the message loop to mitigate
            // threading issues.  There is still a theoretical thread issue with adding 
            // to the events Queue (both JournalHook and SendInput), but we do not want 
            // to alter the timings of the existing shipped behavior.  I did not run into
            // problems with 2 threads on a multiproc machine
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

                        System.Diagnostics.Debug.WriteLine("Send " + currentInput[0].inputUnion.ki.wVk + " " + currentInput[0].inputUnion.ki.wScan.ToString("2X") + " " + currentInput[0].inputUnion.ki.dwFlags);
                        // send only currentInput[0]
                        eventsSent += UnsafeNativeMethods.SendInput(1, currentInput, INPUTSize);

                        // We need this slight delay here for Alt-Tab to work on Vista when the Aero theme
                        // is running.  See DevDiv bugs 23355.  Although this does not look good, a delay
                        // here actually more closely resembles the old JournalHook that processes each
                        // event individually in the hook callback.
                        System.Threading.Thread.Sleep(1);
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

        public static bool Send(string keys, string pname = null)
        {
            if (keys == null || keys.Length == 0) return true;

            ParseKeys(keys);

            if (events == null) return true;

            byte[] oldstate = GetKeyboardState();

            Process p = null;

            IntPtr currentfore = (IntPtr)0;

            if (pname != null && !pname.Equals("Current window") && pname.Length > 0)
            {
                p = Process.GetProcessesByName(pname).FirstOrDefault();

                if (p != null)
                {
                    currentfore = BaseUtils.Win32.UnsafeNativeMethods.GetForegroundWindow();
                    System.Diagnostics.Debug.WriteLine("Current fore" + currentfore + " " + p.MainWindowHandle);

                    BaseUtils.Win32.UnsafeNativeMethods.SetForegroundWindow(p.MainWindowHandle);
                }
                else
                    return false;
            }

            SendInput(oldstate);

            if ( p != null )
            {
                BaseUtils.Win32.UnsafeNativeMethods.SetForegroundWindow(currentfore);
            }

            return true;
        }

    }
}
