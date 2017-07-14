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

    static public int? InvariantParseIntNullOffset(this string s, int offset)     // s can be null, can have a +/- in front indicating offset
    {
        int i;
        if (s != null)
        {
            char first = s[0];
            if (first == '-' || first == '+')
                s = s.Substring(1);

            if (int.TryParse(s, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out i))
            {
                if (first == '-')
                    i = offset - i;
                else if (first == '+')
                    i = offset + i;

                return i;
            }
        }
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

    static public int[] VersionFromString(this string s)
    {
        string[] list = s.Split('.');
        return VersionFromStringArray(list);
    }

    static public int[] VersionFromStringArray(this string[] list)
    {
        if (list.Length > 0)
        {
            int[] v = new int[list.Length];

            for (int i = 0; i < list.Length; i++)
            {
                if (!list[i].InvariantParse(out v[i]))
                    return null;
            }

            return v;
        }

        return null;
    }

    static public int CompareVersion(this int[] v1, int[] v2)    // is V1>V2, 1, 0 = equals, -1 less
    {
        for (int i = 0; i < v1.Length; i++)
        {
            if (i >= v2.Length || v1[i] > v2[i])
                return 1;
            else if (v1[i] < v2[i])
                return -1;
        }

        return 0;
    }

    static public int[] GetVersion(this System.Reflection.Assembly aw)
    {
        string v = aw.FullName.Split(',')[1].Split('=')[1];
        string[] list = v.Split('.');
        return VersionFromStringArray(list);
    }
}

