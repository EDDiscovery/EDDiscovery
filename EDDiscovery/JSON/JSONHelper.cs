using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery
{
    public  static class JSONHelper
    {
        static public bool GetBool(JToken jToken, bool def = false)
        {
            bool? b = GetBoolNull(jToken);
            return (b != null) ? b.Value : def;
        }

        static public bool? GetBoolNull(JToken jToken)
        {
            if (IsNullOrEmptyT(jToken))
                return null;
            try
            {
                return jToken.Value<bool>();
            }
            catch { return null; }
        }

        static public float GetFloat(JToken jToken, float def = 0)
        {
            float? f = GetFloatNull(jToken);
            return (f != null) ? f.Value : def;
        }

        static public float? GetFloatNull(JToken jToken)
        {
            if (IsNullOrEmptyT(jToken))
                return null;
            try
            {
                return jToken.Value<float>();

            }
            catch { return null; }
        }

        static public double GetDouble(JToken jToken, double def = 0)
        {
            double? f = GetDoubleNull(jToken);
            return (f != null) ? f.Value : def;
        }

        static public double? GetDoubleNull(JToken jToken)
        {
            if (IsNullOrEmptyT(jToken))
                return null;
            try
            {
                return jToken.Value<double>();
            }
            catch { return null; }
        }

        static public int GetInt(JToken jToken, int def = 0)
        {
            int? f = GetIntNull(jToken);
            return (f != null) ? f.Value : def;
        }

        static public int? GetIntNull(JToken jToken)
        {
            if (IsNullOrEmptyT(jToken))
                return null;
            try
            {
                return jToken.Value<int>();
            }
            catch { return null; }
        }

        static public long GetLong(JToken jToken, long def = 0)
        {
            long? f = GetLongNull(jToken);
            return (f != null) ? f.Value : def;
        }

        static public long? GetLongNull(JToken jToken)
        {
            if (IsNullOrEmptyT(jToken))
                return null;
            try
            {
                return jToken.Value<long>();
            }
            catch { return null; }
        }


        static public string GetStringNull(JToken jToken)
        {
            if (IsNullOrEmptyT(jToken))
                return null;
            try
            {
                return jToken.Value<string>();
            }
            catch { return null; }
        }

        static public string GetStringDef(JToken jToken, string def = "")
        {
            if (IsNullOrEmptyT(jToken))
                return def;
            try
            {
                return jToken.Value<string>();
            }
            catch { return def; }
        }

        static public string GetMultiStringDef(JObject evt, string[] names, string def = "")
        {
            foreach (string s in names)
            {
                JToken jt = evt[s];

                if (!IsNullOrEmptyT(jt))
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

        static public bool IsNullOrEmptyT(JToken token)
        {
            return (token == null) ||
                   (token.Type == JTokenType.Array && !token.HasValues) ||
                   (token.Type == JTokenType.Object && !token.HasValues) ||
                   (token.Type == JTokenType.String && token.ToString() == String.Empty) ||
                   (token.Type == JTokenType.Null);
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

        // return all JSON items as name/value pairs

        static public void GetJSONFieldNamesValues(string json, Dictionary<string, string> values)
        {
            JObject jo = JObject.Parse(json);  // Create a clone

            foreach (JToken jc in jo.Children())
            {
                ExpandTokensA(jc, values);
            }
        }

        static private void ExpandTokensA(JToken jt, Dictionary<string, string> values)
        {
            if (jt.HasValues)
            {
                JTokenType[] decodeable = { JTokenType.Boolean, JTokenType.Date, JTokenType.Integer, JTokenType.String, JTokenType.Float, JTokenType.TimeSpan };

                foreach (JToken jc in jt.Children())
                {
                    if (jc.HasValues)
                    {
                        ExpandTokensA(jc, values);
                    }
                    else if (Array.FindIndex(decodeable, x => x == jc.Type) != -1)
                    {
                        values[jc.Path] = jc.Value<string>();
                    }
                }
            }
        }

        // given a set of valuesneeded, fill in the values.. only fills in the ones in valuesneeded

        static public void GetJSONFieldValues( string json, Dictionary<string, string> valuesneeded)
        {
            JObject jo = JObject.Parse(json);

            int togo = valuesneeded.Count;

            foreach (JToken jc in jo.Children())
            {
                ExpandTokensB(jc, valuesneeded, ref togo);
                if (togo == 0)
                    break;
            }
        }

        static private void ExpandTokensB(JToken jt, Dictionary<string, string> valuesneeded, ref int togo)
        {
            JTokenType[] decodeable = { JTokenType.Boolean, JTokenType.Date, JTokenType.Integer, JTokenType.String, JTokenType.Float, JTokenType.TimeSpan };

            if (jt.HasValues && togo > 0)
            {
                foreach (JToken jc in jt.Children())
                {
                    if (jc.HasValues)
                    {
                        ExpandTokensB(jc, valuesneeded, ref togo);
                    }
                    else
                    {
                        string name = jc.Path;

                        if (valuesneeded.ContainsKey(name) && Array.FindIndex(decodeable, x => x == jc.Type) != -1)
                        {
                            valuesneeded[name] = jc.Value<string>();
                            togo--;

                            // System.Diagnostics.Debug.WriteLine("Found "+ name);
                        }
                    }

                    if (togo == 0)  // if we have all values, stop
                        break;
                }
            }
        }
    }

}

