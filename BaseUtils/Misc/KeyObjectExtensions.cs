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
        new Tuple<string,Keys>("CloseBrackets", Keys.Oem4),
        new Tuple<string,Keys>("Pipe", Keys.Oem5),
        new Tuple<string,Keys>("OpenBrackets", Keys.Oem6),
        new Tuple<string,Keys>("Quotes", Keys.Oem7),
        new Tuple<string,Keys>("Backquote", Keys.Oem8),
        new Tuple<string,Keys>("OemClear", Keys.OemClear),  // clashes with clear..
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

        string keyname = key.ToString();

        if (key == NumEnter)        // special treatement
            return "NumEnter";
        else if (keyname.Length == 2 && keyname[0] == 'D')          // remove the D
            keyname = keyname.Substring(1);
        else if (keyname.StartsWith("Oem") )                        // oem tender care, first thru the list, then use the default
        {
            Tuple<string, Keys> vk = (from t in oemtx where t.Item2 == key select t).FirstOrDefault();  // see if we have a table translate..

            if (vk != null)
                keyname = vk.Item1;
            else
            {           // just caps case it for niceness
                System.Globalization.TextInfo textInfo = new System.Globalization.CultureInfo("en-US", false).TextInfo;
                keyname = textInfo.ToTitleCase(keyname.Substring( keyname.Length>4 ? 3 : 0));    // if its OemPeriod, use Period. if its Oem1, use Oem1
            }
        }

        return k + keyname;
    }

    public static string VKeyToString(this Keys key , Keys shift, Keys alt, Keys ctrl)  // shift/alt/ctrl holds either None, or Shift, or RShift etc
    {
        string k = "";
        if (shift != Keys.None)
            k = (shift != Keys.RShiftKey ) ? "Shift" : "RShift";
        if (alt != Keys.None)
            k = k.AppendPrePad( (alt != Keys.RMenu) ? "Alt" : "RAlt", "+");
        if (ctrl != Keys.None)
            k = k.AppendPrePad( (ctrl != Keys.RControlKey) ? "Ctrl" : "RCtrl", "+");

        if (key != Keys.None)
            k = k.AppendPrePad(key.VKeyToString(), "+");

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
    
}

