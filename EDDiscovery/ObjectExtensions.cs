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
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

public static class JSONObjectExtensions
{
    static public bool Empty(this JToken token)
    {
        return (token == null) ||
               (token.Type == JTokenType.Array && !token.HasValues) ||
               (token.Type == JTokenType.Object && !token.HasValues) ||
               (token.Type == JTokenType.String && token.ToString() == String.Empty) ||
               (token.Type == JTokenType.Null);
    }

    static public string StrNull(this JToken jToken)
    {
        if (jToken.Empty())
            return null;
        try
        {
            return jToken.Value<string>();
        }
        catch { return null; }
    }

    static public string Str(this JToken jToken, string def = "")
    {
        if (jToken.Empty())
            return def;
        try
        {
            return jToken.Value<string>();
        }
        catch { return def; }
    }

    static public int Int(this JToken jToken, int def = 0)
    {
        int? f = jToken.IntNull();
        return (f != null) ? f.Value : def;
    }

    static public int? IntNull(this JToken jToken)
    {
        if (jToken.Empty())
            return null;
        try
        {
            return jToken.Value<int>();
        }
        catch { return null; }
    }

    static public long Long(this JToken jToken, long def = 0)
    {
        long? f = jToken.LongNull();
        return (f != null) ? f.Value : def;
    }

    static public long? LongNull(this JToken jToken)
    {
        if (jToken.Empty())
            return null;
        try
        {
            return jToken.Value<long>();
        }
        catch { return null; }
    }

    static public float Float(this JToken jToken, float def = 0)
    {
        float? f = jToken.FloatNull();
        return (f != null) ? f.Value : def;
    }

    static public float? FloatNull(this JToken jToken)
    {
        if (jToken.Empty())
            return null;
        try
        {
            return jToken.Value<float>();

        }
        catch { return null; }
    }

    static public double Double(this JToken jToken, double def = 0)
    {
        double? f = jToken.DoubleNull();
        return (f != null) ? f.Value : def;
    }

    static public double? DoubleNull(this JToken jToken)
    {
        if (jToken.Empty())
            return null;
        try
        {
            return jToken.Value<double>();
        }
        catch { return null; }
    }

    static public bool Bool(this JToken jToken, bool def = false)
    {
        bool? b = jToken.BoolNull();
        return (b != null) ? b.Value : def;
    }

    static public bool? BoolNull(this JToken jToken)
    {
        if (jToken.Empty())
            return null;
        try
        {
            return jToken.Value<bool>();
        }
        catch { return null; }
    }

    static public string GetMultiStringDef(JObject evt, string[] names, string def = "")
    {
        foreach (string s in names)
        {
            JToken jt = evt[s];

            if (!jt.Empty())
            {
                try
                {
                    return jt.Value<string>();
                }
                catch { }
            }
        }
        return def;
    }

}

public static class ObjectExtensionsStrings
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

    public static string Alt(this string obj, string alt)
    {
        return (obj == null || obj.Length == 0) ? alt : obj;
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
        return s.Replace(@"\\", "\\");
    }

    public static void AppendPrePad(this System.Text.StringBuilder sb, string other, string prepad = " ")
    {
        if (other != null && other.Length > 0)
        {
            if (sb.Length > 0)
                sb.Append(prepad);
            sb.Append(other);
        }
    }

    public static bool AppendPrePad(this System.Text.StringBuilder sb, string other, string prefix, string prepad = " ")
    {
        if (other != null && other.Length > 0)
        {
            if (sb.Length > 0)
                sb.Append(prepad);
            if (prefix.Length > 0)
                sb.Append(prefix);
            sb.Append(other);
            return true;
        }
        else
            return false;
    }


    public static string Replace(this string str, string oldValue, string newValue, StringComparison comparison)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder(str.Length * 4);

        int previousIndex = 0;
        int index = str.IndexOf(oldValue, comparison);
        while (index != -1)
        {
            sb.Append(str.Substring(previousIndex, index - previousIndex));
            sb.Append(newValue);
            index += oldValue.Length;

            previousIndex = index;
            index = str.IndexOf(oldValue, index, comparison);
        }
        sb.Append(str.Substring(previousIndex));

        return sb.ToString();
    }

    public static int FirstCharNonWhiteSpace(this string obj)
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

        while (exp.Length > 0)
        {
            if (exp[0] == '{')
            {
                int end = exp.IndexOf('}');

                if (end == -1)              // missing end bit, assume the lot..
                    end = exp.Length;

                string pl = exp.Substring(1, end - 1);

                exp = (end < exp.Length) ? exp.Substring(end + 1) : "";

                res += pl.PickOneOf(';', rx);
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

    public static string ToStringCommaList(this System.Collections.Generic.List<string> list, int mincount = 100000, bool escapectrl = false)
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

            if (escapectrl)
                r += list[i].EscapeControlChars().QuoteString(comma: true);
            else
                r += list[i].QuoteString(comma: true);
        }

        return r;
    }

    public static string ToString(this int[] a, string separ)
    {
        string outstr = "";
        if (a.Length > 0)
        {
            outstr = a[0].ToString(System.Globalization.CultureInfo.InvariantCulture);

            for (int i = 1; i < a.Length; i++)
                outstr += separ + a[i].ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        return outstr;
    }

    public static string ToString(this System.Windows.Forms.Keys key, System.Windows.Forms.Keys modifier)
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

    public static string ToStringInvariant(this int v)
    {
        return v.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }
    public static string ToStringInvariant(this long v)
    {
        return v.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }
    public static string ToStringInvariant(this bool? v)
    {
        return (v.HasValue) ? (v.Value ? "1" : "0") : "";
    }
    public static string ToStringInvariant(this double? v, string format)
    {
        return (v.HasValue) ? v.Value.ToString(format) : "";
    }
    public static string ToStringInvariant(this int? v)
    {
        return (v.HasValue) ? v.Value.ToString(System.Globalization.CultureInfo.InvariantCulture) : "";
    }
    public static string ToStringInvariant(this long? v)
    {
        return (v.HasValue) ? v.Value.ToString(System.Globalization.CultureInfo.InvariantCulture) : "";
    }

    // fix word_word to Word Word
    //  s = Regex.Replace(s, @"([A-Za-z]+)([_])([A-Za-z]+)", m => { return m.Groups[1].Value.FixTitleCase() + " " + m.Groups[3].Value.FixTitleCase(); });
    // fix _word to spc Word
    //  s = Regex.Replace(s, @"([_])([A-Za-z]+)", m => { return " " + m.Groups[2].Value.FixTitleCase(); });
    // fix zeros
    //  s = Regex.Replace(s, @"([A-Za-z]+)([0-9])", "$1 $2");       // Any ascii followed by number, split
    //  s = Regex.Replace(s, @"(^0)(0+)", "");     // any 000 at start of line, remove
    //  s = Regex.Replace(s, @"( 0)(0+)", " ");     // any space 000 in middle of line, remove
    //  s = Regex.Replace(s, @"(0)([0-9]+)", "$2");   // any 0Ns left, remove 0

    enum State { space, alpha, nonalpha, digits0, digits };
    static public string SplitCapsWordFull(this string capslower, Dictionary<string, string> namerep = null)     // one_two goes to One_Two
    {
        if (capslower == null || capslower.Length == 0)
            return "";

        string s = SplitCapsWord(capslower);

        System.Text.StringBuilder sb = new System.Text.StringBuilder(256);

        State state = State.space;

        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];
            if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
            {
                if (state != State.alpha)
                {
                    if (state != State.space)
                        sb.Append(' ');

                    state = State.alpha;

                    if (namerep != null)           // at alpha start, see if we have any global subs of alpha numerics
                    {
                        int j = i + 1;
                        for (; j < s.Length && ((s[j] >= 'A' && s[j] <= 'Z') || (s[j] >= 'a' && s[j] <= 'z') || (s[j] >= '0' && s[j] <= '9')); j++)
                            ;

                        string keyname = s.Substring(i, j - i);

                        //                        string keyname = namekeys.Find(x => s.Substring(i).StartsWith(x));

                        if (namerep.ContainsKey(keyname))
                        {
                            sb.Append(namerep[keyname]);
                            i += keyname.Length - 1;                  // skip this, we are in alpha, -1 because of i++ at top
                        }
                        else
                            sb.Append(char.ToUpper(c));
                    }
                    else
                        sb.Append(char.ToUpper(c));
                }
                else
                    sb.Append(c);
            }
            else
            {
                if (c == '_')
                    c = ' ';

                if (c == ' ')
                {
                    state = State.space;
                    sb.Append(c);
                }
                else if (c >= '0' && c <= '9')
                {
                    if (state != State.digits)
                    {
                        if (state != State.space && state != State.digits0)
                            sb.Append(' ');

                        if (c == '0')
                        {
                            state = State.digits0;          // 0, don't append
                        }
                        else
                        {
                            state = State.digits;
                            sb.Append(c);
                        }
                    }
                    else
                        sb.Append(c);
                }
                else
                {
                    if (state != State.nonalpha)
                    {
                        if (state != State.space)
                            sb.Append(' ');

                        state = State.nonalpha;
                    }

                    sb.Append(c);
                }

            }
        }

        return sb.ToString();
    }

    // regexp of below : string s = Regex.Replace(capslower, @"([A-Z]+)([A-Z][a-z])", "$1 $2"); //Upper(rep)UpperLower = Upper(rep) UpperLower
    // s = Regex.Replace(s, @"([a-z\d])([A-Z])", "$1 $2");     // lowerdecUpper split
    // s = Regex.Replace(s, @"[-\s]", " "); // -orwhitespace with spc

    public static string SplitCapsWord(this string capslower)
    {
        if (capslower == null || capslower.Length == 0)
            return "";

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

    public static bool InQuotes(this string s, int max = 0)            // left true if quote left over on line, taking care of any escapes..
    {
        if (max <= 0)
            max = s.Length;

        bool inquote = false;
        for (int i = 0; i < max; i++)
        {
            if (s[i] == '\\' && i < max - 1 && s[i + 1] == '"')
                i += 1;     // ignore this, ingore "
            else if (s[i] == '"')
                inquote = !inquote;
        }

        return inquote;
    }
}

public static class ObjectExtensionsNumbersBool
{
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

    static public bool InvariantParse(this string s, out int i)
    {
        return int.TryParse(s, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out i);
    }

    static public int? InvariantParseIntNull(this string s)     // s can be null
    {
        int i;
        if (s != null && int.TryParse(s, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out i))
            return i;
        else
            return null;
    }

    static public bool InvariantParse(this string s, out double i)
    {
        return double.TryParse(s, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out i);
    }

    static public double? InvariantParseDoubleNull(this string s)
    {
        double i;
        if (s != null && double.TryParse(s, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out i))
            return i;
        else
            return null;
    }

    static public bool InvariantParse(this string s, out long i)
    {
        return long.TryParse(s, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out i);
    }

    static public long? InvariantParseLongNull(this string s)
    {
        long i;
        if (s != null && long.TryParse(s, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out i))
            return i;
        else
            return null;
    }
}

#region Colors

public static class ObjectExtensionsColours
{
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

