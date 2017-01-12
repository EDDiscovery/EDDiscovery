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

            IsTrue,             // numeric !=0
            IsFalse,            // numeric == 0

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

            AlwaysTrue,       // Always true
        };

        public static bool IsNullOperation(string matchname) { return matchname.Contains("Always"); }
        public static bool IsUnaryOperation(string matchname) { return matchname.Contains("Present") || matchname.Contains("Is True") || matchname.Contains("Is False"); }

        static public string[] MatchNames = { "Contains",
                                       "Not Contains",
                                       "== (Str)",
                                       "!= (Str)",
                                       "Contains(CS)",
                                       "Not Contains(CS)",
                                       "== (CS)",
                                       "!= (CS)",
                                       "Is True (Int)",
                                       "Is False (Int)",
                                       "== (Num)",
                                       "!= (Num)",
                                       "> (Num)",
                                       ">= (Num)",
                                       "< (Num)",
                                       "<= (Num)",
                                       ">= (Date)",
                                       "< (Date)",
                                       "Is Present",
                                       "Not Present",
                                       "Always True"
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
            public string matchstring;                     // always set
            public MatchType matchtype;                     // true: Contents match for true, else contents dont match for true

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

                    matchstring = v;

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

            public void IndicateValuesNeeded(ref Dictionary<string, string> vr)
            {
                foreach (ConditionEntry fd in fields)
                {
                    if ( !IsNullOperation(fd.matchtype.ToString()) && !fd.itemname.Contains("%"))     // nulls need no data..  nor does anything with expand in
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

        public enum ExpandResult { Failed, NoExpansion, Expansion };
        public delegate ExpandResult ExpandString(string input, Dictionary<string, string> vars, out string result);    // callback, if we want to expand the content string

        //errlist = null if no errors found

        public List<Condition> GetConditionListByEventName( string eventname )       // Event name.. give me conditions which match that name or ALL
        {
            List<Condition> fel = (from fil in conditionlist
                                   where
                      (fil.eventname.Equals("All") || fil.eventname.Equals(eventname, StringComparison.InvariantCultureIgnoreCase))
                                   select fil).ToList();

            return (fel.Count == 0) ? null : fel;
        }

        // check all conditions against these values.
        public bool? CheckAll(Dictionary<string, string> values, out string errlist , List<Condition> passed = null , ExpandString se = null )            // Check all conditions..
        {
            if (conditionlist.Count == 0)            // no filters match, null
            {
                errlist = null;
                return null;
            }

            return CheckConditions(conditionlist, values, out errlist, passed, se);
        }

        public bool? CheckConditions(List<Condition> fel, Dictionary<string, string> values, out string errlist, List<Condition> passed = null,
                                        ExpandString se = null)            // null nothing trigged, false/true otherwise. 
        {
            errlist = null;

            bool? outerres = null;

            foreach (Condition fe in fel)        // find all values needed
            {
                bool? innerres = null;

                foreach (ConditionEntry f in fe.fields)
                {
                    bool matched = false;

                    if (f.matchtype == MatchType.IsPresent)         // these use f.itemname without any expansion
                    {
                        if (values.ContainsKey(f.itemname))
                            matched = true;
                    }
                    else if (f.matchtype == MatchType.IsNotPresent)
                    {
                        if (!values.ContainsKey(f.itemname))
                            matched = true;
                    }
                    else if (f.matchtype == MatchType.AlwaysTrue)
                    {
                        matched = true;         // does not matter what the item or value contains
                    }
                    else
                    {
                        string leftside = null;
                        ExpandResult er = ExpandResult.NoExpansion;

                        if (se != null)     // if we have a string expander, try the left side
                        {
                            er = se(f.itemname, values, out leftside);

                            if (er == ExpandResult.Failed)        // stop on error
                            {
                                errlist += leftside;     // add on errors..
                                innerres = false;   // stop loop, false
                                break;
                            }
                        }

                        if (er == ExpandResult.NoExpansion)     // no expansion, must be a variable name
                        {
                            leftside = values.ContainsKey(f.itemname) ? values[f.itemname] : null;
                            if (leftside == null)
                            {
                                errlist += "Item " + f.itemname + " is not available" + Environment.NewLine;
                                innerres = false;
                                break;                       // stop the loop, its a false
                            }
                        }

                        string rightside;

                        if (se != null)         // if we have a string expander, pass it thru
                        {
                            er = se(f.matchstring, values, out rightside);

                            if (er == ExpandResult.Failed )        //  if error, abort
                            {
                                errlist += rightside;     // add on errors..
                                innerres = false;   // stop loop, false
                                break;
                            }
                        }
                        else
                            rightside = f.matchstring;

                        if (f.matchtype == MatchType.DateBefore || f.matchtype == MatchType.DateAfter)
                        {
                            DateTime tmevalue, tmecontent;
                            if (!DateTime.TryParse(leftside, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), System.Globalization.DateTimeStyles.None, out tmevalue) )
                            {
                                errlist += "Date time not in correct format on left side" + Environment.NewLine;
                                innerres = false;
                                break;

                            }
                            else if ( !DateTime.TryParse(rightside, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), System.Globalization.DateTimeStyles.None, out tmecontent))
                            {
                                errlist += "Date time not in correct format on right side" + Environment.NewLine;
                                innerres = false;
                                break;
                            }
                            else
                            {
                                if (f.matchtype == MatchType.DateBefore)
                                    matched = tmevalue.CompareTo(tmecontent) < 0;
                                else
                                    matched = tmevalue.CompareTo(tmecontent) >= 0;
                            }
                        }
                        else if (f.matchtype == MatchType.Equals)
                            matched = leftside.Equals(rightside, StringComparison.InvariantCultureIgnoreCase);
                        else if (f.matchtype == MatchType.EqualsCaseSensitive)
                            matched = leftside.Equals(rightside);

                        else if (f.matchtype == MatchType.NotEqual)
                            matched = !leftside.Equals(rightside, StringComparison.InvariantCultureIgnoreCase);
                        else if (f.matchtype == MatchType.NotEqualCaseSensitive)
                            matched = !leftside.Equals(rightside);

                        else if (f.matchtype == MatchType.Contains)
                            matched = leftside.IndexOf(rightside, StringComparison.InvariantCultureIgnoreCase) >= 0;
                        else if (f.matchtype == MatchType.ContainsCaseSensitive)
                            matched = leftside.Contains(rightside);

                        else if (f.matchtype == MatchType.DoesNotContain)
                            matched = leftside.IndexOf(rightside, StringComparison.InvariantCultureIgnoreCase) < 0;
                        else if (f.matchtype == MatchType.DoesNotContainCaseSensitive)
                            matched = !leftside.Contains(rightside);

                        else if (f.matchtype == MatchType.IsTrue || f.matchtype == MatchType.IsFalse)
                        {
                            int inum = 0;

                            if (int.TryParse(leftside, out inum))
                                matched = (f.matchtype == MatchType.IsTrue) ? (inum != 0) : (inum == 0);
                            else
                            {
                                errlist += "True/False value is not an integer on left side" + Environment.NewLine;
                                innerres = false;
                                break;
                            }
                        }
                        else
                        {
                            double fnum = 0, num = 0;

                            if (!double.TryParse(leftside, out num))
                            {
                                errlist += "Number not in correct format on left side" + Environment.NewLine;
                                innerres = false;
                                break;
                            }
                            else if (!double.TryParse(rightside, out fnum) )
                            {
                                errlist += "Number not in correct format on right side" + Environment.NewLine;
                                innerres = false;
                                break;
                            }
                            else
                            {
                                if (f.matchtype == MatchType.NumericEquals)
                                    matched = Math.Abs(num - fnum) < 0.0000000001;  // allow for rounding

                                else if (f.matchtype == MatchType.NumericNotEquals)
                                    matched = Math.Abs(num - fnum) >= 0.0000000001;

                                else if (f.matchtype == MatchType.NumericGreater)
                                    matched = num > fnum;

                                else if (f.matchtype == MatchType.NumericGreaterEqual)
                                    matched = num >= fnum;

                                else if (f.matchtype == MatchType.NumericLessThan)
                                    matched = num < fnum;

                                else if (f.matchtype == MatchType.NumericLessThanEqual)
                                    matched = num <= fnum;
                                else
                                    System.Diagnostics.Debug.Assert(false);
                            }
                        }
                    }

                    //  System.Diagnostics.Debug.WriteLine(fe.eventname + ":Compare " + f.matchtype + " '" + f.contentmatch + "' with '" + vr.value + "' res " + matched + " IC " + fe.innercondition);

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
                    j2["Content"] = fd.matchstring;
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

                    ret += f.fields[i].itemname + " " + MatchNames[(int)f.fields[i].matchtype] + " " + f.fields[i].matchstring;
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

                        fieldlist.Add(new ConditionEntry()
                        {
                            itemname = item,
                            matchstring = content,
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

        #region JSON as the vars - used for the filter out system..

        private bool? CheckJSON(string eventjson,        // JSON of the event 
                            string eventname,       // Event name..
                            Dictionary<string, string> othervars,   // any other variables to present to the condition, in addition to the JSON variables
                            out string errlist,     // null if okay..
                            List<Condition> passed)            // null or conditions passed
        {
            errlist = null;

            List<Condition> fel = GetConditionListByEventName(eventname);

            if ( fel != null )
            {
                Dictionary<string, string> valuesneeded = new Dictionary<string, string>();

                foreach (Condition fe in fel)        // find all values needed
                    fe.IndicateValuesNeeded(ref valuesneeded);

                try
                {
                    JSONHelper.GetJSONFieldValues(eventjson, valuesneeded);

                    foreach (KeyValuePair<string, string> v in othervars)       // store other vars to values needed
                        valuesneeded[v.Key] = v.Value;

                    return CheckConditions(fel, valuesneeded, out errlist, passed);    // and check, passing in the values collected against the conditions to test.
                }
                catch (Exception)
                {
                    errlist = "JSON failed to parse!";
                }
            }

            return null;
        }
    
        private bool CheckFilterTrueOut(string json, string eventname, Dictionary<string, string> othervars,  out string errlist , List<Condition> passed)      // if none, true, if false, true.. 
        {                                                                                         // only if the filter passes do we get a false..
            bool? v = CheckJSON(json, eventname, othervars, out errlist, passed);
            return !v.HasValue || v.Value == false;
        }

        public bool FilterHistory(HistoryEntry he, Dictionary<string, string> othervars)                // true if it should be included
        {
            string er;
            return CheckFilterTrueOut(he.journalEntry.EventDataString, he.journalEntry.EventTypeStr, othervars, out er, null);     // true it should be included
        }

        public List<HistoryEntry> FilterHistory(List<HistoryEntry> he, Dictionary<string, string> othervars , out int count)    // filter in all entries
        {
            count = 0;
            if (Count == 0)       // no filters, all in
                return he;
            else
            {
                string er;
                List<HistoryEntry> ret = (from s in he where CheckFilterTrueOut(s.journalEntry.EventDataString, s.journalEntry.EventTypeStr, othervars, out er, null) select s).ToList();

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

                    string er;

                    if (!CheckFilterTrueOut(s.journalEntry.EventDataString, s.journalEntry.EventTypeStr, othervars, out er, list))
                    {
                        //System.Diagnostics.Debug.WriteLine("Filter out " + s.Journalid + " " + s.EntryType + " " + s.EventDescription);
                        s.EventDescription = "!" + list[0].eventname + ":::" + s.EventDescription;
                        count++;
                    }

                    ret.Add(s);
                }

                return ret;
            }
        }

        #endregion

    }
}
