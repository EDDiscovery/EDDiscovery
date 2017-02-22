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
using System.Drawing;
using System.Text.RegularExpressions;

public static class ObjectExtensions
{
    public static string ToNullSafeString(this object obj)
    {
        return (obj ?? string.Empty).ToString();
    }

    public static string ToNANSafeString(this double obj, string format)
    {
        return (obj != double.NaN) ? obj.ToString(format) : string.Empty;
    }

    public static string ToNANNullSafeString(this double? obj, string format)
    {
        return (obj.HasValue && obj != double.NaN) ? obj.Value.ToString(format) : string.Empty;
    }

    public static string ToNullUnknownString(this object obj)
    {
        if (obj == null)
            return string.Empty;
        else
        {
            string str = obj.ToString();
            return str.Equals("Unknown") ? "" : str.Replace("_", " ");
        }
    }

    public static string QuoteString(this string obj, bool comma = false, bool bracket = false)
    {
        if (obj.Length == 0 || obj.Contains("\"") || obj.Contains(" ") || (bracket && obj.Contains(")")) || (comma && obj.Contains(",")))
            obj = "\"" + obj.Replace("\"", "\\\"") + "\"";

        return obj;
    }

    public static string EscapeControlChars(this string obj)
    {
        string s = obj.Replace(@"\", @"\\");        // order vital
        s = obj.Replace("\r", @"\r");
        return s.Replace("\n", @"\n");
    }

    public static string ReplaceEscapeControlChars(this string obj)
    {
        string s = obj.Replace(@"\n", "\n");
        s = s.Replace(@"\r", "\r");
        return s.Replace(@"\\","\\");
    }

    public static int FirstCharNonWhiteSpace(this string obj )
    {
        int i = 0;
        while (i < obj.Length && char.IsWhiteSpace(obj[i]))
            i++;
        return i;
    }

    public static string PickOneOf(this string str, char separ, System.Random rx)   // pick one of x;y;z or if ;x;y;z, pick x and one of y or z
    {
        string[] a = str.Split(separ);

        if (a.Length >= 2)          // x;y
        {
            if (a[0].Length == 0)      // ;y      
            {
                string res = a[1];
                if (a.Length > 2)   // ;y;x;z
                    res += " " + a[2 + rx.Next(a.Length - 2)];

                return res;
            }
            else
                return a[rx.Next(a.Length)];
        }
        else
            return a[0];
    }

    public static string PickOneOfGroups(this string exp, System.Random rx) // pick one of x;y;z or if ;x;y;z, pick x and one of y or z, include {x;y;z}
    {
        string res = "";
        exp = exp.Trim();

        while (true)
        {
            if (exp[0] == '{')
            {
                int end = exp.IndexOf('}');

                if (end >= 0)
                {
                    string pl = exp.Substring(1, end - 1);
                    res += pl.PickOneOf(';', rx) + " ";
                    exp = exp.Substring(end + 1).Trim();
                }
            }
            else
            {
                res += exp.PickOneOf(';', rx);
                break;
            }
        }

        return res;
    }


    public static string AddSuffixToFilename(this string file, string suffix)
    {
        return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(file), System.IO.Path.GetFileNameWithoutExtension(file) + suffix) + System.IO.Path.GetExtension(file);
    }

    public static string ToStringCommaList( this System.Collections.Generic.List<string> list , int mincount = 100000)
    {
        string r = "";
        for (int i = 0; i < list.Count; i++)
        {
            if (i >= mincount && list[i].Length == 0)           // if >= minimum, and zero
            {
                int j = i + 1;
                while (j < list.Count && list[j].Length == 0)   // if all others are zero
                    j++;

                if (j == list.Count)        // if so, stop
                    break;
            }

            if (i > 0)
                r += ", ";

            r += list[i].QuoteString(comma: true);
        }

        return r;
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
        List<int> positions = new List<int>();
        List<string> words = new List<string>();

        int start = 0;

        if (capslower[0] == '-' || char.IsWhiteSpace(capslower[0]))  // Remove leading dash or whitespace
            start = 1;

        for (int i = 1; i <= capslower.Length; i++)
        {
            char c0 = capslower[i - 1];
            char c1 = i < capslower.Length ? capslower[i] : '\0';
            char c2 = i < capslower.Length - 1 ? capslower[i + 1] : '\0';

            if (i == capslower.Length || // End of string
                (i < capslower.Length - 1 && c0 >= 'A' && c0 <= 'Z' && c1 >= 'A' && c1 <= 'Z' && c2 >= 'a' && c2 <= 'z') || // UpperUpperLower
                (((c0 >= 'a' && c0 <= 'z') || (c0 >= '0' && c0 <= '9')) && c1 >= 'A' && c1 <= 'Z') || // LowerdigitUpper
                (c1 == '-' || c1 == ' ' || c1 == '\t' || c1 == '\r')) // dash or whitespace
            {
                if (i > start)
                    words.Add(capslower.Substring(start, i - start));

                if (i < capslower.Length && (c1 == '-' || c1 == ' ' || c1 == '\t' || c1 == '\r'))
                    start = i + 1;
                else
                    start = i;
            }
        }

        return String.Join(" ", words);
    }

    public static bool Eval(this string ins, out string res)        // true, res = eval.  false, res = error
    {
        System.Data.DataTable dt = new System.Data.DataTable();

        res = "";

        try
        {
            var v = dt.Compute(ins, "");
            System.Type t = v.GetType();
            //System.Diagnostics.Debug.WriteLine("Type return is " + t.ToString());
            if (v is double)
                res = ((double)v).ToString(System.Globalization.CultureInfo.InvariantCulture);
            else if (v is System.Decimal)
                res = ((System.Decimal)v).ToString(System.Globalization.CultureInfo.InvariantCulture);
            else if (v is int)
                res = ((int)v).ToString(System.Globalization.CultureInfo.InvariantCulture);
            else
            {
                res = "Expression is Not A Number";
                return false;
            }

            return true;
        }
        catch
        {
            res = "Expression does not evaluate";
            return false;
        }
    }

    #region Colors

    /// <summary>
    /// Determine if two colors are both fully transparent or are otherwise equal in value, ignoring
    /// any stupid naming comparisons or RGB comparisons when both are fully transparent.
    /// </summary>
    /// <param name="other">The color to compare to.</param>
    /// <returns><c>true</c> if the colors are both fully transparent or are equal in value; <c>false</c> otherwise.</returns>
    public static bool IsEqual(this Color c, Color other)
    {
        return ((c.A == 0 && other.A == 0) || c.ToArgb() == other.ToArgb());
    }

    /// <summary>
    /// Determine whether or not a <see cref="Color"/> is completely transparent.
    /// </summary>
    /// <returns><c>true</c> if the <see cref="Color"/> is completely transparent; <c>false</c> otherwise.</returns>
    public static bool IsFullyTransparent(this Color c)
    {
        return (c.A == 0);
    }

    /// <summary>
    /// Determine whether or not a <see cref="Color"/> contains an alpha component.
    /// </summary>
    /// <returns><c>true</c> if the color is at least partially transparent; <c>false</c> if it is fully opaque.</returns>
    public static bool IsSemiTransparent(this Color c)
    {
        return (c.A < 255);
    }

    /// <summary>
    /// Average two <see cref="Color"/> objects together, complete with alpha, with <paramref name="ratio"/>
    /// determining the ratio of the original color to the <paramref name="other"/> <see cref="Color"/>.
    /// </summary>
    /// <param name="other">The <see cref="Color"/> to average with this one.</param>
    /// <param name="ratio">(0.0f-1.0f) The strength of the original <see cref="Color"/> in the resulting value. 1.0f means
    /// all original, while 0.0f means all <paramref name="other"/>, with 0.5f being an equal mix between the two.</param>
    /// <returns>The average of the two <see cref="Color"/>s given the ratio specified by <paramref name="ratio"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="ratio"/> is NaN.</exception>
    public static Color Average(this Color c, Color other, float ratio = 0.5f)
    {
        if (float.IsNaN(ratio))
            throw new ArgumentOutOfRangeException("ratio", "must be between 0.0f and 1.0f.");

        float left = Math.Min(Math.Max(ratio, 0.0f), 1.0f);
        float right = 1.0f - left;
        return Color.FromArgb(
            (byte)Math.Max(Math.Min(Math.Round((float)c.A * left + (float)other.A * right), 255), 0),
            (byte)Math.Max(Math.Min(Math.Round((float)c.R * left + (float)other.R * right), 255), 0),
            (byte)Math.Max(Math.Min(Math.Round((float)c.G * left + (float)other.G * right), 255), 0),
            (byte)Math.Max(Math.Min(Math.Round((float)c.B * left + (float)other.B * right), 255), 0));
    }

    /// <summary>
    /// Multiply a color. That is, brighten or darken all three channels by the same <paramref name="amount"/>, without modification to the alpha channel.
    /// </summary>
    /// <param name="amount"><c>1.0</c> will yield an identical color, <c>0.0</c> will yield black, <c>0.5</c> will
    /// be half as bright, <c>2.0</c> will be twice as bright. Negative values will be treated as positive.</param>
    /// <returns>The multiplied color with the new brightness. If <paramref name="amount"/> is
    /// <c>float.NaN</c>, the value returned will be unchanged.</returns>
    /// <remarks>Any components with a zero value (such as Black) will always return a zero value component.</remarks>
    public static Color Multiply(this Color c, float amount = 1.0f)
    {
        if (float.IsNaN(amount))
            return c;

        float val = Math.Abs(amount);
        return Color.FromArgb(c.A,
            (byte)Math.Max(Math.Min(Math.Round((float)c.R * val), 255), 0),
            (byte)Math.Max(Math.Min(Math.Round((float)c.G * val), 255), 0),
            (byte)Math.Max(Math.Min(Math.Round((float)c.B * val), 255), 0));
    }

    #endregion // Colors
}

