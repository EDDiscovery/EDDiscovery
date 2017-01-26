using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery
{
    public class ConditionVariables
    {
        public Dictionary<string, string> values = new Dictionary<string, string>();

        public ConditionVariables()
        {
        }

        public ConditionVariables(ConditionVariables other)
        {
            values = new Dictionary<string, string>(other.values);
        }

        public ConditionVariables(ConditionVariables other, ConditionVariables other2)
        {
            values = new Dictionary<string, string>(other.values);
            Add(other2);
        }

        public ConditionVariables(string s, FromMode fm)     //v=1,v=2 no brackets
        {
            FromString(s,fm);
        }

        public string this[string s] { get { return values[s]; } set { values[s] = value; } }
        public int Count { get { return values.Count; } }

        public KeyValuePair<string, string> First() { return values.First(); }
        public List<string> KeyList { get { return values.Keys.ToList(); } }
        public bool ContainsKey(string s) { return values.ContainsKey(s); }

        public void Clear() { values.Clear(); }

        public bool GetFirstValue(out string var, out string val)
        {
            var = val = "";
            if (Count > 0)
            {
                var = values.First().Key;
                val = values.First().Value;
                return true;
            }
            else
                return false;
        }


        public override string ToString()
        {
            string s = "";
            foreach (KeyValuePair<string, string> v in values)
            {
                if (s.Length > 0)
                    s += ",";
                s += v.Key + "=" + v.Value.QuotedEscapeString();
            }

            return s;
        }

        public enum FromMode { SingleEntry, MultiEntryComma, MultiEntryCommaBracketEnds };

        public bool FromString(string s, FromMode fm)    // string, not bracketed.
        {
            StringParser p = new StringParser(s);
            return FromString(p, fm);
        }

        public bool FromString(StringParser p, FromMode fm, List<string> namelimit = null, bool fixnamecase = false , bool allowextops = false )
        {
            Dictionary<string, string> newvars = new Dictionary<string, string>();

            while (!p.IsEOL)
            {
                string varname = p.NextQuotedWord( "= ");

                if (varname == null)
                    return false;

                if (allowextops)            // with extended ops, the ops are stored on the end of the variable name to indicate type..
                {                           // used only with let and set..
                    if (p.IsCharMoveOn('$'))            // $=
                        varname += '$';
                    if (p.IsCharMoveOn('+'))            // += or $+=
                        varname += '+';
                }

                if (!p.IsCharMoveOn('='))
                    return false;

                if (fixnamecase)
                    varname = Tools.FixTitleCase(varname);

                if (namelimit != null && !namelimit.Contains(varname))
                    return false;

                string value = p.NextQuotedWord((fm == FromMode.SingleEntry) ? " " : (fm == FromMode.MultiEntryComma) ? ", " : ",) ");

                if (value == null)
                    return false;

                newvars[varname] = value;

                if (fm == FromMode.MultiEntryCommaBracketEnds && p.PeekChar() == ')')        // bracket, stop don't remove.. outer bit wants to check its there..
                {
                    values = newvars;
                    return true;
                }
                else if (fm == FromMode.SingleEntry && !p.IsEOL)        // single entry, must be eol now
                    return false;
                else if (!p.IsEOL && !p.IsCharMoveOn(','))   // if not EOL, but not comma, incorrectly formed list
                    return false;
            }

            values = newvars;
            return true;
        }

        static public string flagRunAtRefresh = "RunAtRefresh;";            // ACTION DATA Flags, stored with action program name in events to configure it

        public string ToActionDataString(string flag)           // helpers to encode action data..
        {
            if ( flag.Length > 0 )
                return flag + "," + ToString();
            else
                return ToString();
        }

        public void FromActionDataString(string ad, out string flag)        // helpers to encode action data..
        {
            int comma = ad.IndexOf(',');
            int equal = ad.IndexOf('=');
            if (comma >= 0 && (equal == -1 || comma < equal))        // if sksksk, v1 = or wekwkwk,
            {
                flag = ad.Substring(0, comma);
                FromString(ad.Substring(comma + 1), ConditionVariables.FromMode.MultiEntryComma);
            }
            else
            {
                flag = "";
                FromString(ad, ConditionVariables.FromMode.MultiEntryComma);
            }
        }

        public void Add(List<ConditionVariables> varlist)
        {
            foreach (ConditionVariables d in varlist)
            {
                if (d != null)
                {
                    foreach (KeyValuePair<string, string> v in d.values)   // plus event vars
                        values[v.Key] = v.Value;
                }
            }
        }

        public void Add(ConditionVariables d)
        {
            if (d != null)
            {
                foreach (KeyValuePair<string, string> v in d.values)   // plus event vars
                    values[v.Key] = v.Value;
            }
        }

        public ConditionVariables FilterVars(string filter)
        {
            int wildcard = filter.IndexOf('*');
            if (wildcard >= 0)
                filter = filter.Substring(0, wildcard);

            ConditionVariables ret = new ConditionVariables();

            foreach (KeyValuePair<string, string> k in values)
            {
                if ((wildcard >= 0 && k.Key.StartsWith(filter)) || k.Key.Equals(filter))
                    ret[k.Key] = k.Value;
            }

            return ret;
        }

        public void DumpVars(string prefix = "")
        {
            foreach (KeyValuePair<string, string> k in values)
            { System.Diagnostics.Debug.WriteLine(prefix + k.Key + "=" + k.Value); }
        }

        public string AddToVar(string name, int add, int initial)       // DOES NOT set anything..
        {
            if (values.ContainsKey(name))
            {
                int i;
                if (int.TryParse(values[name], out i))
                {
                    return (i + add).ToString();
                }
            }

            return initial.ToString();
        }

        public bool GetJSONFieldNamesAndValues(string json, string prefix = "")
        {
            try
            {
                JObject jo = JObject.Parse(json);  // Create a clone

                foreach (JToken jc in jo.Children())
                {
                    ExpandTokensA(jc, prefix);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void ExpandTokensA(JToken jt, string prefix)
        {
            if (jt.HasValues)
            {
                JTokenType[] decodeable = { JTokenType.Boolean, JTokenType.Date, JTokenType.Integer, JTokenType.String, JTokenType.Float, JTokenType.TimeSpan };

                foreach (JToken jc in jt.Children())
                {
                    if (jc.HasValues)
                    {
                        ExpandTokensA(jc, prefix);
                    }
                    else if (Array.FindIndex(decodeable, x => x == jc.Type) != -1)
                    {
                        values[prefix + jc.Path] = jc.Value<string>();
                    }
                }
            }
        }

        // given a set of valuesneeded, fill in the values.. only fills in the ones in valuesneeded

        public bool GetJSONFieldValuesIndicated(string json)
        {
            try
            {
                JObject jo = JObject.Parse(json);

                int togo = values.Count;

                foreach (JToken jc in jo.Children())
                {
                    ExpandTokensB(jc, ref togo);
                    if (togo == 0)
                        break;
                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        private void ExpandTokensB(JToken jt, ref int togo)
        {
            JTokenType[] decodeable = { JTokenType.Boolean, JTokenType.Date, JTokenType.Integer, JTokenType.String, JTokenType.Float, JTokenType.TimeSpan };

            if (jt.HasValues && togo > 0)
            {
                foreach (JToken jc in jt.Children())
                {
                    if (jc.HasValues)
                    {
                        ExpandTokensB(jc, ref togo);
                    }
                    else
                    {
                        string name = jc.Path;

                        if (values.ContainsKey(name) && Array.FindIndex(decodeable, x => x == jc.Type) != -1)
                        {
                            values[name] = jc.Value<string>();
                            togo--;

                            // System.Diagnostics.Debug.WriteLine("Found "+ name);
                        }
                    }

                    if (togo == 0)  // if we have all values, stop
                        break;
                }
            }
        }

        public delegate ConditionLists.ExpandResult ExpandString(string input, ConditionVariables vars, out string result);    // callback, if we want to expand the content string

        // if ver there, pass it thru expander, then eval it between these ranges..

        public string GetNumericValue(string name, int min , int max, int def , out int val, ExpandString e = null , ConditionVariables vars = null )
        {
            val = def;

            if (values.ContainsKey(name))
            {
                string res;
                if (e != null)
                {
                    if (e(values[name], vars, out res) == ConditionLists.ExpandResult.Failed)
                        return res;
                }
                else
                    res = values[name];

                if (int.TryParse(res, out val) && val >= min && val <= max) // if we don't have volume..  or does not parse or out of range
                    return null;
            }

            return null;
        }

        public void AddPropertiesFieldsOfType( Object o, string prefix = "" )
        {
            Type jtype = o.GetType();

            foreach (System.Reflection.PropertyInfo pi in jtype.GetProperties())
            {
                if (pi.GetIndexParameters().GetLength(0) == 0)      // only properties with zero parameters are called
                {
                    string name = prefix + pi.Name;
                    Type rettype = pi.PropertyType;
                    System.Reflection.MethodInfo getter = pi.GetGetMethod();
                    Extract(getter.Invoke(o, null), rettype, name);
                }
            }

            foreach (System.Reflection.FieldInfo fi in jtype.GetFields())
            {
                string name = prefix + fi.Name;
                Extract(fi.GetValue(o), fi.FieldType, name);
            }

        }

        void Extract(Object o, Type rettype, string name)
        {
            try // just to make sure a strange type does not barfe it
            {
                if (o == null)
                    values[name] = "";
                else if ( o is bool)
                {
                    values[name] = ((bool)o) ? "1" : "0";
                }
                else if (!rettype.IsGenericType || rettype.GetGenericTypeDefinition() != typeof(Nullable<>))
                {
                    var v = Convert.ChangeType(o, rettype);
                    values[name] = v.ToString();
                }
                else if (o is Nullable<double>)
                    ExtractNull<double>((Nullable<double>)o, name);
                else if (o is Nullable<bool>)
                    ExtractNull<bool>((Nullable<bool>)o, name);
                else if (o is Nullable<int>)
                    ExtractNull<int>((Nullable<int>)o, name);
                else if (o is Nullable<long>)
                    ExtractNull<long>((Nullable<long>)o, name);
            }
            catch { }
        }

        void ExtractNull<T>(Nullable<T> obj, string name) where T : struct
        {
            if (obj.HasValue)
                Extract(obj.Value, typeof(T), name);
            else
                values[name] = "";
        }

    }
}
