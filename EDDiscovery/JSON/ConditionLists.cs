using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery
{
    public class ConditionLists
    {
        public enum MatchType
        {
            Contains,           // contains
            DoesNotContain,     // doesnotcontain
            Equals,             // ==
            NotEqual,           // !=
            ContainsCaseSensitive,           // contains
            DoesNotContainCaseSensitive,     // doesnotcontain
            EqualsCaseSensitive,             // ==
            NotEqualCaseSensitive,           // !=

            NumericEquals,      // numeric =
            NumericNotEquals,   // numeric !=
            NumericGreater,            // numeric >
            NumericGreaterEqual,       // numeric >=
            NumericLessThan,           // numeric <
            NumericLessThanEqual,      // numeric <=

            DateAfter,          // Date compare
            DateBefore,         // Date compare.

            IsPresent,          // field is present
            IsNotPresent,       // field is not present
        };

        static public bool IsDateComparision(MatchType mt) { return mt == MatchType.DateAfter || mt == MatchType.DateBefore; }
        static public bool IsNumberComparision(MatchType mt) { return mt >= MatchType.NumericEquals && mt <= MatchType.NumericLessThanEqual; }

        static public string[] MatchNames = { "Contains",
                                       "Not Contains",
                                       "== (Str)",
                                       "!= (Str)",
                                       "Contains(CS)",
                                       "Not Contains(CS)",
                                       "== (CS)",
                                       "!= (CS)",
                                       "== (Num)",
                                       "!= (Num)",
                                       "> (Num)",
                                       ">= (Num)",
                                       "< (Num)",
                                       "<= (Num)",
                                       ">= (Date)",
                                       "< (Date)",
                                       "Is Present",
                                       "Not Present"
                                    };

        public enum LogicalCondition
        {
            Or,     // any true
            And,    // all true
            Nor,    // any true produces a false
            Nand,   // any not true produces a true
        }

        public class ConditionEntry
        {
            public string itemname;
            public string contentmatch;                     // always set
            public MatchType matchtype;                     // true: Contents match for true, else contents dont match for true

            public double contentvalue;                     // internal speed only
            public DateTime contentdate;                    // internal speed only

            public bool Create(string i, string ms, string v)     // ms can have spaces inserted into enum
            {
                try
                {
                    itemname = i;

                    int indexof = Array.IndexOf(MatchNames, ms);

                    if (indexof >= 0)
                        matchtype = (MatchType)(indexof);
                    else
                        matchtype = (MatchType)Enum.Parse(typeof(MatchType), ms.Replace(" ", ""));       // must work, exception otherwise

                    contentmatch = v;

                    if (matchtype.ToString().Contains("Date"))      // if Date..
                        return DateTime.TryParse(contentmatch, out contentdate);
                    if (!matchtype.ToString().Contains("Numeric"))      // if not Numeric, its a string, okay
                        return true;
                    else if (double.TryParse(v, out contentvalue))     // else check it evals a number
                        return true;
                }
                catch { }

                return false;
            }
        };

        public class Condition
        {
            public string eventname;
            public List<ConditionEntry> fields;
            public LogicalCondition innercondition;               // condition between fields
            public LogicalCondition outercondition;               // condition between filter events of same type
            public string action;                           // action associated with a pass
            public string actiondata;                       // any data (sound file, program)

            public bool Create(string e, string a, string d, string i, string o)   // i,o can have spaces inserted into enum
            {
                try
                {
                    eventname = e;
                    action = a;
                    actiondata = d;
                    innercondition = (LogicalCondition)Enum.Parse(typeof(LogicalCondition), i.Replace(" ", ""));       // must work, exception otherwise
                    outercondition = (LogicalCondition)Enum.Parse(typeof(LogicalCondition), o.Replace(" ", ""));       // must work, exception otherwise
                    return true;
                }
                catch { }

                return false;
            }

            public void Add(ConditionEntry f)
            {
                if (fields == null)
                    fields = new List<ConditionEntry>();
                fields.Add(f);
            }

            public void FillValuesNeeded(ref Dictionary<string, string> vr)
            {
                foreach (ConditionEntry fd in fields)
                {
                    vr[fd.itemname] = null;
                }
            }
        }

        public List<Condition> conditionlist = new List<Condition>();

        public ConditionLists()
        {
        }

        public void Add(Condition fe)
        {
            conditionlist.Add(fe);
        }

        public void Clear()
        {
            conditionlist.Clear();
        }

        public int Count { get { return conditionlist.Count; } }

        public bool? Check(string eventname, Dictionary<string, string> values, List<Condition> passed) // check conditions matching eventname
        {
            List<Condition> fel = (from fil in conditionlist
                                   where (fil.eventname.Equals("All") || fil.eventname.Equals(eventname, StringComparison.InvariantCultureIgnoreCase))
                                   select fil).ToList();

            if (fel.Count == 0)            // no filters match, null
                return null;

            return Check(fel, values, passed);
        }

        public bool? Check(Dictionary<string, string> values, List<Condition> passed)            // Check all conditions..
        {
            if (conditionlist.Count == 0)            // no filters match, null
                return null;

            return Check(conditionlist, values, passed);
        }

        public bool? Check(List<Condition> fel, Dictionary<string, string> values, List<Condition> passed)            // null nothing trigged, false/true otherwise. 
        {
            bool? outerres = null;

            foreach (Condition fe in fel)        // find all values needed
            {
                bool? innerres = null;

                foreach (ConditionEntry f in fe.fields)
                {
                    bool ispresent = values.ContainsKey(f.itemname);
                    string value = (ispresent) ? values[f.itemname] : null;

                    bool matched = false;
                    double num = 0;

                    if (f.matchtype == MatchType.IsPresent)
                    {
                        if (value != null)
                            matched = true;
                    }
                    else if (f.matchtype == MatchType.IsNotPresent)
                    {
                        if (value == null)
                            matched = true;
                    }
                    else if (value == null)      // if item name not found
                    {
                        innerres = false;
                        break;                       // stop the loop, its a false
                    }
                    else if (f.matchtype == MatchType.DateBefore)
                    {
                        DateTime tme;
                        if (DateTime.TryParse(value, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), System.Globalization.DateTimeStyles.None, out tme))
                        {
                            matched = tme.CompareTo(f.contentdate) < 0;
                        }
                    }
                    else if (f.matchtype == MatchType.DateAfter)
                    {
                        DateTime tme;
                        if (DateTime.TryParse(value, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), System.Globalization.DateTimeStyles.None, out tme))
                        {
                            matched = tme.CompareTo(f.contentdate) >= 0;
                        }
                    }
                    else
                    {
                        if (f.matchtype == MatchType.Equals)
                            matched = value.Equals(f.contentmatch, StringComparison.InvariantCultureIgnoreCase);
                        if (f.matchtype == MatchType.EqualsCaseSensitive)
                            matched = value.Equals(f.contentmatch);

                        else if (f.matchtype == MatchType.NotEqual)
                            matched = !value.Equals(f.contentmatch, StringComparison.InvariantCultureIgnoreCase);
                        else if (f.matchtype == MatchType.NotEqualCaseSensitive)
                            matched = !value.Equals(f.contentmatch);

                        else if (f.matchtype == MatchType.Contains)
                            matched = value.IndexOf(f.contentmatch, StringComparison.InvariantCultureIgnoreCase) >= 0;
                        else if (f.matchtype == MatchType.ContainsCaseSensitive)
                            matched = value.Contains(f.contentmatch);

                        else if (f.matchtype == MatchType.DoesNotContain)
                            matched = value.IndexOf(f.contentmatch, StringComparison.InvariantCultureIgnoreCase) < 0;
                        else if (f.matchtype == MatchType.DoesNotContainCaseSensitive)
                            matched = !value.Contains(f.contentmatch);

                        else if (f.matchtype == MatchType.NumericEquals)
                            matched = double.TryParse(value, out num) && Math.Abs(num - f.contentvalue) < 0.0000000001;  // allow for rounding

                        else if (f.matchtype == MatchType.NumericNotEquals)
                            matched = double.TryParse(value, out num) && Math.Abs(num - f.contentvalue) >= 0.0000000001;

                        else if (f.matchtype == MatchType.NumericGreater)
                            matched = double.TryParse(value, out num) && num > f.contentvalue;

                        else if (f.matchtype == MatchType.NumericGreaterEqual)
                            matched = double.TryParse(value, out num) && num >= f.contentvalue;

                        else if (f.matchtype == MatchType.NumericLessThan)
                            matched = double.TryParse(value, out num) && num < f.contentvalue;

                        else if (f.matchtype == MatchType.NumericLessThanEqual)
                            matched = double.TryParse(value, out num) && num <= f.contentvalue;

                        //  System.Diagnostics.Debug.WriteLine(fe.eventname + ":Compare " + f.matchtype + " '" + f.contentmatch + "' with '" + vr.value + "' res " + matched + " IC " + fe.innercondition);
                    }

                    if (fe.innercondition == LogicalCondition.And)       // Short cut, if AND, all must pass, and it did not
                    {
                        if (!matched)
                        {
                            innerres = false;
                            break;
                        }
                    }
                    else if (fe.innercondition == LogicalCondition.Nand)  // Short cut, if NAND, and not matched
                    {
                        if (!matched)
                        {
                            innerres = true;                        // positive non match - NAND produces a true
                            break;
                        }
                    }
                    else if (fe.innercondition == LogicalCondition.Or)    // Short cut, if OR, and matched
                    {
                        if (matched)
                        {
                            innerres = true;
                            break;
                        }
                    }
                    else
                    {                                               // short cut, if NOR, and matched, its false
                        if (matched)
                        {
                            innerres = false;
                            break;
                        }
                    }
                }

                if (!innerres.HasValue)                             // All tests executed, without a short cut, we set it to a definitive state
                {
                    if (fe.innercondition == LogicalCondition.And)        // none did not match, producing a false, so therefore AND is true
                        innerres = true;
                    else if (fe.innercondition == LogicalCondition.Or)    // none did match, producing a true, so therefore OR must be false
                        innerres = false;
                    else if (fe.innercondition == LogicalCondition.Nor)   // none did match, producing a false, so therefore NOR must be true
                        innerres = true;
                    else                                            // NAND none did match, producing a true, so therefore NAND must be false
                        innerres = false;
                }

                if (innerres.Value && passed != null)               // if want a list of passes, do it
                    passed.Add(fe);

                if (!outerres.HasValue)                                 // if first time, its just the value
                    outerres = innerres.Value;
                else if (fe.outercondition == LogicalCondition.Or)
                    outerres |= innerres.Value;
                else if (fe.outercondition == LogicalCondition.And)
                    outerres &= innerres.Value;
                else if (fe.outercondition == LogicalCondition.Nor)
                    outerres = !(outerres | innerres.Value);
                else if (fe.outercondition == LogicalCondition.Nand)
                    outerres = !(outerres & innerres.Value);
            }

            return outerres;
        }

        public string GetJSON()
        {
            return GetJSONObject().ToString();
        }

        public JObject GetJSONObject()
        {
            JObject evt = new JObject();

            JArray jf = new JArray();

            foreach (Condition f in conditionlist)
            {
                JObject j1 = new JObject();
                j1["EventName"] = f.eventname;
                j1["ICond"] = f.innercondition.ToString();
                j1["OCond"] = f.outercondition.ToString();
                j1["Actions"] = f.action;
                j1["ActionData"] = f.actiondata;

                JArray jfields = new JArray();

                foreach (ConditionEntry fd in f.fields)
                {
                    JObject j2 = new JObject();
                    j2["Item"] = fd.itemname;
                    j2["Content"] = fd.contentmatch;
                    j2["Matchtype"] = fd.matchtype.ToString();
                    jfields.Add(j2);
                }

                j1["Filters"] = jfields;

                jf.Add(j1);
            }

            evt["FilterSet"] = jf;

            return evt;
        }

        public override string ToString()
        {
            string ret = "";

            for (int j = 0; j < conditionlist.Count; j++)
            {
                Condition f = conditionlist[j];

                if (j > 0)
                    ret += " " + f.outercondition.ToString() + " ";

                if (f.fields.Count > 1)
                    ret += "(";

                for (int i = 0; i < f.fields.Count; i++)
                {
                    if (i > 0)
                        ret += " " + f.innercondition.ToString() + " ";

                    ret += f.fields[i].itemname + " " + MatchNames[(int)f.fields[i].matchtype] + " " + f.fields[i].contentmatch;
                }

                if (f.fields.Count > 1)
                    ret += ")";
            }

            return ret;
        }

        public bool FromJSON(string s)
        {
            Clear();

            try
            {
                JObject jo = (JObject)JObject.Parse(s);
                return FromJSON(jo);
            }
            catch
            {
                return false;
            }
        }

        public bool FromJSON(JObject jo)
        {
            try
            {
                Clear();

                JArray jf = (JArray)jo["FilterSet"];

                foreach (JObject j in jf)
                {
                    string evname = (string)j["EventName"];
                    LogicalCondition ftinner = (LogicalCondition)Enum.Parse(typeof(LogicalCondition), (string)j["ICond"]);
                    LogicalCondition ftouter = (LogicalCondition)Enum.Parse(typeof(LogicalCondition), (string)j["OCond"]);
                    string act = (string)j["Actions"];
                    string actd = (string)j["ActionData"];

                    JArray filset = (JArray)j["Filters"];

                    List<ConditionEntry> fieldlist = new List<ConditionEntry>();

                    foreach (JObject j2 in filset)
                    {
                        string item = (string)j2["Item"];
                        string content = (string)j2["Content"];
                        string matchtype = (string)j2["Matchtype"];
                        double cdouble = 0;
                        double.TryParse(content, out cdouble);
                        DateTime dt;
                        DateTime.TryParse(content, out dt);

                        fieldlist.Add(new ConditionEntry()
                        {
                            itemname = item,
                            contentmatch = content,
                            contentvalue = cdouble,
                            contentdate = dt,
                            matchtype = (MatchType)Enum.Parse(typeof(MatchType), matchtype)
                        });
                    }

                    conditionlist.Add(new Condition()
                    {
                        eventname = evname,
                        innercondition = ftinner,
                        outercondition = ftouter,
                        fields = fieldlist,
                        action = act,
                        actiondata = actd
                    });
                }

                return true;
            }
            catch { }

            return false;
        }

        #region JSON as the vars

        public bool? Check(string eventjson,        // JSON of the event 
                            string eventname,       // Event name..
                            Dictionary<string,string> othervars ,   // any other variables to present to the condition, in addition to the JSON variables
                            List<Condition> passed)            // null or conditions passed
        {
            List<Condition> fel = (from fil in conditionlist
                                   where
                      (fil.eventname.Equals("All") || fil.eventname.Equals(eventname, StringComparison.InvariantCultureIgnoreCase))
                                   select fil).ToList();

            if (fel.Count == 0)            // no filters match, null
                return null;

            Dictionary<string, string> valuesneeded = new Dictionary<string, string>();

            foreach (Condition fe in fel)        // find all values needed
                fe.FillValuesNeeded(ref valuesneeded);

            foreach (KeyValuePair<string, string> v in othervars)       // store other vars to values needed
                valuesneeded[v.Key] = v.Value;

            try
            {
                JObject jo = JObject.Parse(eventjson);  // Create a clone

                int togo = valuesneeded.Count;

                foreach (JToken jc in jo.Children())
                {
                    ExpandTokens(jc, valuesneeded, ref togo);
                    if (togo == 0)
                        break;
                }

                return Check(fel, valuesneeded, passed);    // and check, passing in the values collected against the conditions to test.
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CheckFilterTrueOut(string json, string eventname, Dictionary<string, string> othervars,  List<Condition> passed)      // if none, true, if false, true.. 
        {                                                                                         // only if the filter passes do we get a false..
            bool? v = Check(json, eventname, othervars, passed);
            return !v.HasValue || v.Value == false;
        }

        public bool FilterHistory(HistoryEntry he, Dictionary<string, string> othervars)                // true if it should be included
        {
            return CheckFilterTrueOut(he.journalEntry.EventDataString, he.journalEntry.EventTypeStr, othervars, null);     // true it should be included
        }

        public List<HistoryEntry> FilterHistory(List<HistoryEntry> he, Dictionary<string, string> othervars , out int count)    // filter in all entries
        {
            count = 0;
            if (Count == 0)       // no filters, all in
                return he;
            else
            {
                List<HistoryEntry> ret = (from s in he where CheckFilterTrueOut(s.journalEntry.EventDataString, s.journalEntry.EventTypeStr, othervars, null) select s).ToList();

                count = he.Count - ret.Count;
                return ret;
            }
        }

        public List<HistoryEntry> MarkHistory(List<HistoryEntry> he, Dictionary<string, string> othervars, out int count)       // Used for debugging it..
        {
            count = 0;

            if (Count == 0)       // no filters, all in
                return he;
            else
            {
                List<HistoryEntry> ret = new List<HistoryEntry>();

                foreach (HistoryEntry s in he)
                {
                    List<Condition> list = new List<Condition>();    // don't want it

                    int mrk = s.EventDescription.IndexOf(":::");
                    if (mrk >= 0)
                        s.EventDescription = s.EventDescription.Substring(mrk + 3);

                    if (!CheckFilterTrueOut(s.journalEntry.EventDataString, s.journalEntry.EventTypeStr, othervars, list))
                    {
                        System.Diagnostics.Debug.WriteLine("Filter out " + s.Journalid + " " + s.EntryType + " " + s.EventDescription);
                        s.EventDescription = "!" + list[0].eventname + ":::" + s.EventDescription;
                        count++;
                    }

                    ret.Add(s);
                }

                return ret;
            }
        }

        private void ExpandTokens(JToken jt, Dictionary<string, string> valuesneeded, ref int togo)
        {
            JTokenType[] decodeable = { JTokenType.Boolean, JTokenType.Date, JTokenType.Integer, JTokenType.String, JTokenType.Float, JTokenType.TimeSpan };

            if (jt.HasValues && togo > 0)
            {
                foreach (JToken jc in jt.Children())
                {
                    if (jc.HasValues)
                    {
                        ExpandTokens(jc, valuesneeded, ref togo);
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

        #endregion

    }
}
