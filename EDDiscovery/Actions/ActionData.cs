using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Actions
{
    class ActionData
    {
        // reponsible for encoding and decoding the actiondata which goes with the action in a condition list

        public static string ToJSON(List<string> flags, ConditionVariables vars)
        {
            JArray jf = new JArray();

            if (flags != null)
            {
                foreach (string s in flags)
                {
                    jf.Add(s);
                }
            }

            JArray jd = new JArray();

            if (vars != null)
            {
                foreach (KeyValuePair<string, string> k in vars.values)
                {
                    JObject v = new JObject();
                    v["Var"] = k.Key;
                    v["Value"] = k.Value;
                    jd.Add(v);
                }
            }

            JObject ad = new JObject();
            ad["Flags"] = jf;
            ad["Vars"] = jd;
            return ad.ToString();
        }

        // line may be null or empty, in which case you get false ..
        public static bool FromJSON(string line, out List<string> flags, out ConditionVariables vars)
        {
            flags = new List<string>();
            vars = new ConditionVariables();

            if (line != null && line.Length > 0)
            {
                try
                {
                    JObject jo = (JObject)JObject.Parse(line);

                    JArray jf = (JArray)jo["Flags"];
                    JArray jd = (JArray)jo["Vars"];

                    foreach (JToken j in jf)
                    {
                        flags.Add((string)j);
                    }

                    foreach (JObject j in jd)
                    {
                        vars[(string)j["Var"]] = (string)j["Value"];
                    }

                    return true;
                }
                catch
                { }
            }

            return false;
        }

    }
}
