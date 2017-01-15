using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Actions
{
    class ActionVariables
    {
        static public string ToString(Dictionary<string, string> vars)
        {
            string s = "";
            foreach (KeyValuePair<string, string> v in vars)
            {
                if (s.Length > 0)
                    s += ",";
                s += v.Key + "=" + v.Value.QuotedEscapeString();
            }

            return s;
        }

        static public void AddVars(Dictionary<string, string> vars, List<Dictionary<string, string>> varlist)
        {
            foreach (Dictionary<string, string> d in varlist)
            {
                if (d != null)
                {
                    foreach (KeyValuePair<string, string> v in d)   // plus event vars
                        vars[v.Key] = v.Value;
                }
            }
        }

        static public void AddVars(Dictionary<string, string> vars, Dictionary<string, string> d)
        {
            if (d != null)
            {
                foreach (KeyValuePair<string, string> v in d)   // plus event vars
                    vars[v.Key] = v.Value;
            }
        }

        static public Dictionary<string, string> FilterVars(Dictionary<string, string> d, string filter)
        {
            int wildcard = filter.IndexOf('*');
            if (wildcard >= 0)
                filter = filter.Substring(0, wildcard);

            Dictionary<string, string> ret = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> k in d)
            {
                if ((wildcard >= 0 && k.Key.StartsWith(filter)) || k.Key.Equals(filter))
                    ret[k.Key] = k.Value;
            }

            return ret;
        }

        static public void DumpVars(Dictionary<string, string> d, string prefix = "")
        {
            foreach (KeyValuePair<string, string> k in d)
            { System.Diagnostics.Debug.WriteLine(prefix + k.Key + "=" + k.Value); }
        }

        static public void AddToVar(Dictionary<string, string> d, string name, int add, int initial)
        {
            if (d.ContainsKey(name))
            {
                int i;
                if (int.TryParse(d[name], out i))
                {
                    d[name] = (i + add).ToString();
                    return;
                }
            }

            d[name] = initial.ToString();
        }
    }
}
