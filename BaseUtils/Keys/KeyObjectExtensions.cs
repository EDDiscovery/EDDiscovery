/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public static class KeyObjectExtensions
{
    public const Keys NumEnter = (Keys)1024;    // special keys code for numenter

    private static Tuple<string, Keys>[] oemtx = new Tuple<string, Keys>[]      // need these due to repeats of codes in the keys enum
    {                                                                               // just using tostring gets you sometimes these, sometimes the oem names
        new Tuple<string,Keys>("Semicolon", Keys.Oem1),
        new Tuple<string,Keys>("Question", Keys.Oem2),
        new Tuple<string,Keys>("Tilde", Keys.Oem3),
        new Tuple<string,Keys>("OpenBrackets", Keys.Oem4),
        new Tuple<string,Keys>("Pipe", Keys.Oem5),
        new Tuple<string,Keys>("Equals", Keys.Oemplus),     // need to call it equals to avoid confusion with num key plus
        new Tuple<string,Keys>("CloseBrackets", Keys.Oem6),
        new Tuple<string,Keys>("Quotes", Keys.Oem7),
        new Tuple<string,Keys>("Backquote", Keys.Oem8),
        new Tuple<string,Keys>("OemClear", Keys.OemClear),  // clashes with clear..
        new Tuple<string,Keys>("PageDown", Keys.Next),  

        //new Tuple<string,Keys>("CapsLock", Keys.Capital),     // On consideration, not renaming them is the best. Stick to c# names
        //new Tuple<string,Keys>("AltKey", Keys.Menu),
        //new Tuple<string,Keys>("RAltKey", Keys.RMenu),
        //new Tuple<string,Keys>("LAltKey", Keys.LMenu),
    };


    public static void VerifyKeyOE()//keep for testing
    {
        foreach (string kn in Enum.GetNames(typeof(Keys)))
        {
            Keys k = (Keys)Enum.Parse(typeof(Keys), kn);
            string name = k.ToString();
            Keys vk = name.ToVkey();
            string errstr = (k != vk) ? " *** ERROR" : "";
            System.Diagnostics.Debug.WriteLine("ID " + kn.PadRight(15) + " Key " + k + "(" + (int)k + ") Name " + name + " to " + vk + errstr);
        }
    }

    public static Keys VKeyAdjust(this Keys key , bool extended, int sc)        // take a key, plus extended and sc, and work out alternate name
    {
        if (key == Keys.Enter && extended)
            return NumEnter;      // FORCE.. no num pad enter.. bodge
        if (key == Keys.ShiftKey && sc == 0x36)
            return Keys.RShiftKey;
        if (key == Keys.ControlKey && extended)
            return Keys.RControlKey;
        if (key == Keys.Menu && extended)
            return Keys.RMenu;

        return key;
    }

    public static string VKeyToString(this System.Windows.Forms.Keys key)       // key to string..
    {
        string k = "";

        string keyname = key.ToString();        // built in translation..

        if (key == NumEnter)        // special treatement
            return "NumEnter";
        else if (keyname.Length == 2 && keyname[0] == 'D')          // remove the Dx
            keyname = keyname.Substring(1);
        else 
        {
            Tuple<string, Keys> vk = (from t in oemtx where t.Item2 == key select t).FirstOrDefault();  // see if we have a table translate..

            if (vk != null)
                keyname = vk.Item1;
            else if (keyname.StartsWith("Oem"))                        // oem tender care..
            {           // just caps case it for niceness
                System.Globalization.TextInfo textInfo = new System.Globalization.CultureInfo("en-US", false).TextInfo;
                keyname = textInfo.ToTitleCase(keyname.Substring( keyname.Length>4 ? 3 : 0));    // if its OemPeriod, use Period. if its Oem1, use Oem1
            }
        }


        return k + keyname;
    }

    public static string ShiftersToString(Keys shift, Keys alt, Keys ctrl)  // shift/alt/ctrl holds either None, or Shift, or RShift etc
    {
        string k = "";
        if (shift != Keys.None)
            k = (shift != Keys.RShiftKey ) ? "Shift" : "RShift";
        if (alt != Keys.None)
            k = k.AppendPrePad( (alt != Keys.RMenu) ? "Alt" : "RAlt", "+");
        if (ctrl != Keys.None)
            k = k.AppendPrePad( (ctrl != Keys.RControlKey) ? "Ctrl" : "RCtrl", "+");
        return k;
    }

    public static string VKeyToString(this Keys key, Keys modifier)     // using Control.Modifier produce a key string
    {
        string k = "";

        if ((modifier & Keys.Shift) != 0)
        {
            k += "Shift";
        }
        if ((modifier & Keys.Alt) != 0)
        {
            k = k.AppendPrePad("Alt", "+");
        }
        if ((modifier & Keys.Control) != 0)
        {
            k = k.AppendPrePad("Ctrl", "+");
        }

        if (key != Keys.None)
            k = k.AppendPrePad(key.VKeyToString(), "+");

        return k;
    }

    public static Keys ToVkey(this string name)     // name to VKey
    {
        if (name != null && name.Length > 0)
        {
            if (name.Equals("NumEnter", StringComparison.InvariantCultureIgnoreCase))       // special care
                return NumEnter;

            if (name.Length == 1 && char.IsDigit(name[0]))          
                return Keys.D0 + (name[0] - '0');       // direct conversion

            Tuple<string, Keys> vk = (from t in oemtx where t.Item1.Equals(name, StringComparison.InvariantCultureIgnoreCase) select t).FirstOrDefault();  // see if we have a table translate..
            if (vk != null) // see if any overrides in order
                return vk.Item2;

            System.Windows.Forms.Keys key;
            if (Enum.TryParse<System.Windows.Forms.Keys>(name, true, out key))      // try with name
                return key;

            if (Enum.TryParse<System.Windows.Forms.Keys>("Oem" + name, true, out key)) //last try with an OEM name as we strip OEM above for some of them
                return key;
        }

        return Keys.None;
    }

    public static Keys ShiftKey(bool state, bool right)         // names of keys in various states
    {
        return state ? (right ? Keys.RShiftKey : Keys.ShiftKey) : Keys.None;
    }
    public static Keys ControlKey(bool state, bool right)
    {
        return state ? (right ? Keys.RControlKey : Keys.ControlKey) : Keys.None;
    }
    public static Keys MenuKey(bool state, bool right)
    {
        return state ? (right ? Keys.RMenu : Keys.Menu) : Keys.None;
    }

    public static bool IsExtendedKey(this Keys k)       // is this an extended key. really, the whole keyboard thing is mental!
    {
        return (k == Keys.Up || k == Keys.Down || k == Keys.Left || k == Keys.Right || k == Keys.Prior || k == Keys.Next ||
                k == Keys.Home || k == Keys.End || k == Keys.Insert || k == Keys.Delete || k == NumEnter || k == Keys.NumLock || k == Keys.RControlKey || k == Keys.RMenu);
    }

    public static bool IsSingleCharName(this Keys k)    // is it a simple key name
    {
        return (k >= Keys.D0 && k <= Keys.D9) || (k >= Keys.A && k <= Keys.Z);
    }

    public static Keys IsShiftPrefix(ref string s)      // look for prefix, remove, return what the prefix is
    {
        if (ObjectExtensionsStrings.IsPrefix(ref s, "Shift", StringComparison.InvariantCultureIgnoreCase))
            return Keys.ShiftKey;
        else if (ObjectExtensionsStrings.IsPrefix(ref s, "RShift", StringComparison.InvariantCultureIgnoreCase))
            return Keys.RShiftKey;
        else
            return Keys.None;
    }

    public static Keys IsCtrlPrefix(ref string s)
    {
        if (ObjectExtensionsStrings.IsPrefix(ref s, "Ctrl", StringComparison.InvariantCultureIgnoreCase))
            return Keys.ControlKey;
        else if (ObjectExtensionsStrings.IsPrefix(ref s, "RCtrl", StringComparison.InvariantCultureIgnoreCase))
            return Keys.RControlKey;
        else
            return Keys.None;
    }

    public static Keys IsAltPrefix(ref string s)
    {
        if (ObjectExtensionsStrings.IsPrefix(ref s, "Alt", StringComparison.InvariantCultureIgnoreCase))
            return Keys.Menu;
        else if (ObjectExtensionsStrings.IsPrefix(ref s, "RAlt", StringComparison.InvariantCultureIgnoreCase))
            return Keys.RMenu;
        else
            return Keys.None;
    }

    public static List<Keys> KeyList(bool inclshifts = false)  // base keys, repeates removed, mouse removed, modifiers removed, with optional inclusion if shift keys
    {
        Keys[] alwaysremove = new Keys[] { Keys.None, Keys.LButton, Keys.RButton, Keys.MButton, Keys.XButton1, Keys.XButton2,
                                    Keys.Shift, Keys.Control, Keys.Alt , Keys.Modifiers , Keys.KeyCode, Keys.PrintScreen,
                                    };

        Keys[] shiftremove = new Keys[] {
                                    Keys.ShiftKey, Keys.ControlKey, Keys.Menu, Keys.LShiftKey, Keys.RShiftKey,
                                    Keys.LControlKey , Keys.RControlKey, Keys.LMenu, Keys.RMenu ,
                                    };

        List<Keys> kl = (from Keys k in Enum.GetValues(typeof(Keys)) where (Array.IndexOf(alwaysremove, k) == -1 && (inclshifts == true || Array.IndexOf(shiftremove, k) == -1)) select k).Distinct().ToList();

        kl.Insert(kl.IndexOf(Keys.Multiply), NumEnter);    // insert the numenter here

        return kl;
    }

    public static List<string> KeyListString(bool inclshifts = false)  // names of base keys as strings
    {
        List<Keys> kl = KeyList(inclshifts);
        List<string> ks = (from Keys k in kl select VKeyToString(k)).ToList();
        return ks;
    }

    // tested 14/11/2017 with alt/shift/ctrl combinations.. left and right

    public static string GenerateSequence(this Keys[] keys)      // first one is the primary, the rest are shifters
    {
        if (keys.Length == 2)       // combinations with shift second..  nicer to do it this way because it keeps the timings
        {
            if (keys[1] == Keys.ShiftKey || keys[1] == Keys.LShiftKey)
            {
                return "Shift+" + keys[0].VKeyToString();
            }
            else if (keys[1] == Keys.ControlKey || keys[1] == Keys.LControlKey)
            {
                return "Ctrl+" + keys[0].VKeyToString();
            }
            else if (keys[1] == Keys.Menu || keys[1] == Keys.LMenu)
            {
                return "Alt+" + keys[0].VKeyToString();
            }
            else if (keys[1] == Keys.RShiftKey)
            {
                return "RShift+" + keys[0].VKeyToString();
            }
            else if (keys[1] == Keys.RControlKey)
            {
                return "RCtrl+" + keys[0].VKeyToString();
            }
            else if (keys[1] == Keys.RMenu)
            {
                return "RAlt+" + keys[0].VKeyToString();
            }
        }

        // else we just play the keys out manually

        string keyseq = "";
        for (int i = keys.Length - 1; i >= 1; i--)      // press down shifters in order
        {
            keyseq += "!" + keys[i].VKeyToString() + " ";
        }

        keyseq += keys[0].ToString() + " ";             // press release key

        for (int i = 1; i < keys.Length; i++)           // lift up shifters in order
        {
            keyseq += "^" + keys[i].VKeyToString() + " ";
        }

        return keyseq;
    }

    static public Dictionary<char, uint> CharToScanCode()       // give me a char vs scan code map
    {
        Dictionary<char, uint> chartoscancode = new Dictionary<char, uint>();
        for (short i = 0; i < 0xff; i++)    // for OEMASCII codes on page 437, map to OEMASCII 
        {
            Encoding enc = Encoding.GetEncoding(437);
            byte[] myByte = new byte[] { (byte)(i) };
            string str = enc.GetString(myByte);     // now in unicode

            int res = (int)BaseUtils.Win32.SafeNativeMethods.OemKeyScan(i);  // in code page 437

            if (res != -1)
            {
                //System.Diagnostics.Debug.WriteLine("Char {0:x} {1} = SC {2:x}", i, str, res);

                if (i == '.' && res == 0x53)    // some maps call '.' numpad period.. lets call it dot on sc 34
                    res = 0x34;

                chartoscancode[str[0]] = (uint)res;
            }
        }

        return chartoscancode;
    }


}

