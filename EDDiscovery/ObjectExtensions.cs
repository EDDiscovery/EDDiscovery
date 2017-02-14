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
 * EDDiscovery is not affiliated with Fronter Developments plc.
 */
using System.Text.RegularExpressions;

public static class ObjectExtensions
{
    public static string ToNullSafeString(this object obj)
        {
        return (obj ?? string.Empty).ToString();
    }

    public static string QuoteString(this string obj, bool comma = false, bool bracket = false)
    {
        if (obj.Length == 0 || obj.Contains("\"") || obj.Contains(" ") || (bracket && obj.Contains(")")) || (comma && obj.Contains(",")))
            obj = "\"" + obj.Replace("\"", "\\\"") + "\"";

        return obj;
    }

    public static string EscapeControlChars(this string obj)
    {
        return obj.Replace("\r", "\\r").Replace("\n", "\\n");
    }

    public static string ReplaceEscapeControlChars(this string obj)
    {
        return obj.Replace("\\r", "\r").Replace("\\n", "\n");
    }

    public static int FirstCharNonWhiteSpace(this string obj )
    {
        int i = 0;
        while (i < obj.Length && char.IsWhiteSpace(obj[i]))
            i++;
        return i;
    }

    public static string AddSuffixToFilename(this string file, string suffix)
    {
        return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(file), System.IO.Path.GetFileNameWithoutExtension(file) + suffix) + System.IO.Path.GetExtension(file);
    }
    
    public static string ToString( this int[] a , string separ)
    {
        string outstr = "";
        if ( a.Length>0)
        {
            outstr = a[0].ToString(System.Globalization.CultureInfo.InvariantCulture);

            for (int i = 1; i < a.Length; i++)
                outstr += separ + a[i].ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        return outstr;
    }

    public static string ToString(this System.Windows.Forms.Keys key, System.Windows.Forms.Keys modifier )
    {
        string k = "";

        if ((modifier & System.Windows.Forms.Keys.Shift) != 0)
        {
            k += "Shift+";
        }
        if ((modifier & System.Windows.Forms.Keys.Alt) != 0)
        {
            k += "Alt+";
        }
        if ((modifier & System.Windows.Forms.Keys.Control) != 0)
        {
            k += "Ctrl+";
        }

        string keyname = key.ToString();
        if (keyname.Length == 2 && keyname[0] == 'D')
            keyname = keyname.Substring(1);
        else if (keyname.StartsWith("Oem") && keyname.Length > 4)       // leave oem1-9, they are not standard.
            keyname = keyname.Substring(3);

        return k + keyname;
    }

    static public bool InvariantParse(this string s, out int i)
    {
        return int.TryParse(s, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out i);
    }

    static public bool InvariantParse(this string s, out double i)
    {
        return double.TryParse(s, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out i);
    }

    static public bool InvariantParse(this string s, out long i)
    {
        return long.TryParse(s, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out i);
    }

    public static string FixTitleCase(this string s)
    {
        if (s.Length > 0)
        {
            s = s.Substring(0, 1).ToUpper() + s.Substring(1).ToLower();
        }
        return s;
    }

    static public string SplitCapsWordUnderscoreTitleCase(this string capslower)     // one_two goes to One_Two
    {
        string s = Regex.Replace(capslower, @"([A-Z]+)([A-Z][a-z])", "$1 $2"); //Upper(rep)UpperLower = Upper(rep) UpperLower
        s = Regex.Replace(s, @"([a-z\d])([A-Z])", "$1 $2");     // lowerdecUpper split
        s = Regex.Replace(s, @"[-\s]", " ");                    // -orwhitespace with spc
        // fix word_word to Word Word
        s = Regex.Replace(s, @"([A-Za-z]+)([_])([A-Za-z]+)", m => { return m.Groups[1].Value.FixTitleCase() + " " + m.Groups[3].Value.FixTitleCase(); });
        // fix _word to spc Word
        s = Regex.Replace(s, @"([_])([A-Za-z]+)", m => { return " " + m.Groups[2].Value.FixTitleCase(); });
        return s;
    }

    public static string SplitCapsWord(this string capslower)
    {
        string s = Regex.Replace(capslower, @"([A-Z]+)([A-Z][a-z])", "$1 $2"); //Upper(rep)UpperLower = Upper(rep) UpperLower
        s = Regex.Replace(s, @"([a-z\d])([A-Z])", "$1 $2");     // lowerdecUpper split
        s = Regex.Replace(s, @"[-\s]", " ");                    // -orwhitespace with spc
        return s;
    }

}

