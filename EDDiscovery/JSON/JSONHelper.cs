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

        static public void GetJSONFieldNames(string json, HashSet<string> fields )
        {
            JObject jo = JObject.Parse(json);  // Create a clone

//            IList<string> keys = jo.Properties().Select(p => p.Name).ToList();

            foreach (JToken jc in jo.Children())
            {
                ExpandTokens(jc, fields);
            }
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

        static private void ExpandTokens(JToken jt, HashSet<string> fields)
        {
            if (jt.HasValues)
            {
                JTokenType[] decodeable = { JTokenType.Boolean, JTokenType.Date, JTokenType.Integer, JTokenType.String, JTokenType.Float, JTokenType.TimeSpan };

                foreach (JToken jc in jt.Children())
                {
                    if (jc.HasValues)
                    {
                        ExpandTokens(jc, fields);
                    }
                    else if (Array.FindIndex(decodeable, x => x == jc.Type) != -1)
                    {
                        string name = jc.Path;

                        if ( !fields.Contains(name))
                            fields.Add(name);
                    }
                }
            }
        }

    }

}

