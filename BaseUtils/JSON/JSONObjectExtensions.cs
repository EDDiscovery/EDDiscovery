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
using System.Globalization;
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
            string s = jToken.Value<string>();      // defend against json having a null
            return s != null ? s : def;
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

    static public DateTime DateTimeUTC( this JToken jToken )  // 1 Jan 2000 is the default
    {
        if (!jToken.Empty())
        {
            try
            {
                string str = jToken.Value<string>();
                return DateTime.Parse(str, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
            }
            catch { }
        }

        return new DateTime(2000, 1, 1);
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

    public static void Rename(this JToken token, string newName)
    {
        if (token == null)
            return;

        var parent = token.Parent;
        if (parent == null)
            throw new InvalidOperationException("The parent is missing.");
        var newToken = new JProperty(newName, token);
        parent.Replace(newToken);
    }

    
    public static Color Color(this JToken jo, Color defc)
    {
        if (jo.Empty())
            return defc;
        try
        {
            string c = jo.Value<string>();
            return System.Drawing.ColorTranslator.FromHtml(c);
        }
        catch { return defc; }
    }

    public static JToken RemoveKeyUnderscores(this JToken jo)
    {
        if (jo == null || jo.Empty() || !(jo is JObject))
            return jo;

        JObject ret = new JObject();

        foreach (JProperty prop in ((JObject)jo).Properties())
        {
            ret[prop.Name.Replace("_", "")] = prop.Value;
        }

        return ret;
    }

    public static JToken RemoveKeyPrefix(this JToken jo, string prefix)
    {
        if (jo == null || jo.Empty() || !(jo is JObject))
            return jo;

        JObject ret = new JObject();

        foreach (JProperty prop in ((JObject)jo).Properties())
        {
            string key = prop.Name;
            if (key.StartsWith(prefix))
                key = key.Substring(prefix.Length);
            ret[key] = prop.Value;
        }

        return ret;
    }


    static public T ToObjectProtected<T>(this JToken token)
    {
        try
        {
            return token.ToObject<T>();
        }
        catch
        {
            return default(T);
        }
    }



}



