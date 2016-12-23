using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery
{
    public class JSONFilter
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

        private bool IsDateComparision(MatchType mt) { return mt == MatchType.DateAfter || mt == MatchType.DateBefore; }
        private bool IsNumberComparision(MatchType mt) { return mt >= MatchType.NumericEquals && mt <= MatchType.NumericLessThanEqual; }

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

        public enum FilterType
        {
            Or,     // any true
            And,    // all true
            Nor,    // any true produces a false
            Nand,   // any not true produces a true
        }

        public class Fields
        {
            public string itemname;
            public string contentmatch;                     // always set
            public MatchType matchtype;                     // true: Contents match for true, else contents dont match for true

            public double contentvalue;                     // internal speed only
            public DateTime contentdate;                    // internal speed only

            public bool Create(string i, string ms , string v)     // ms can have spaces inserted into enum
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
                        return DateTime.TryParse(contentmatch,out contentdate);
                    if (!matchtype.ToString().Contains("Numeric"))      // if not Numeric, its a string, okay
                        return true;
                    else if ( double.TryParse(v,out contentvalue ))     // else check it evals a number
                        return true;
                }
                catch { }

                return false;
            }
        };

        public class FilterEvent
        {
            public string eventname;
            public List<Fields> fields;
            public FilterType innercondition;               // condition between fields
            public FilterType outercondition;               // condition between filter events of same type
            public string action;                           // action associated with a pass
            public string actiondata;                       // any data (sound file, program)

            public bool Create(string e, string a, string d, string i, string o)   // i,o can have spaces inserted into enum
            {
                try
                {
                    eventname = e;
                    action = a;
                    actiondata = d;
                    innercondition = (FilterType)Enum.Parse(typeof(FilterType), i.Replace(" ", ""));       // must work, exception otherwise
                    outercondition = (FilterType)Enum.Parse(typeof(FilterType), o.Replace(" ", ""));       // must work, exception otherwise
                    return true;
                }
                catch { }

                return false;
            }

            public void Add(Fields f)
            {
                if (fields == null)
                    fields = new List<Fields>();
                fields.Add(f);
            }
        }

        private class ValuesReturned
        {
            public ValuesReturned(bool d, bool v) { needdate = d; neednumber = v; }
            public string value;

            public DateTime datetime;
            public bool needdate;
            public bool datetimegood;

            public double number;
            public bool neednumber;
            public bool numbergood;
        }

        public List<FilterEvent> filters = new List<FilterEvent>();

        public JSONFilter()
        {
        }

        public void Add(FilterEvent fe )
        {
            filters.Add(fe);
        }

        public void Clear()
        {
            filters.Clear();
        }

        public int Count { get { return filters.Count; } }
        
        public string GetJSON()
        {
            JObject evt = new JObject();

            JArray jf = new JArray();

            foreach (FilterEvent f in filters)
            {
                JObject j1 = new JObject();
                j1["EventName"] = f.eventname;
                j1["ICond"] = f.innercondition.ToString();
                j1["OCond"] = f.outercondition.ToString();
                j1["Actions"] = f.action;
                j1["ActionData"] = f.actiondata;

                JArray jfields = new JArray();

                foreach (Fields fd in f.fields)
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

            return evt.ToString();
        }

        public bool FromJSON( string s )
        {
            Clear();

            try
            {
                JObject jo = (JObject)JObject.Parse(s);

                JArray jf = (JArray)jo["FilterSet"];

                foreach( JObject j in jf )
                {
                    string evname = (string)j["EventName"];
                    FilterType ftinner = (FilterType)Enum.Parse(typeof(FilterType), (string)j["ICond"]);
                    FilterType ftouter = (FilterType)Enum.Parse(typeof(FilterType), (string)j["OCond"]);
                    string act = (string)j["Actions"];
                    string actd = (string)j["ActionData"];

                    JArray filset = (JArray)j["Filters"];

                    List<Fields> fieldlist = new List<Fields>();

                    foreach( JObject j2 in filset)
                    {
                        string item = (string)j2["Item"];
                        string content = (string)j2["Content"];
                        string matchtype = (string)j2["Matchtype"];
                        double cdouble = 0;
                        double.TryParse(content, out cdouble);
                        DateTime dt;
                        DateTime.TryParse(content, out dt);

                        fieldlist.Add(new Fields()
                        {
                            itemname = item,
                            contentmatch = content,
                            contentvalue = cdouble,
                            contentdate = dt,
                            matchtype = (MatchType)Enum.Parse(typeof(MatchType), matchtype)
                        });
                    }

                    filters.Add(new FilterEvent()
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

        public bool? Check(string json, string eventname, ref List<FilterEvent> passed)            // null nothing trigged, false/true otherwise. 
        {
            FilterEvent[] fel = (from fil in filters where 
                                        (fil.eventname.Equals("All") || fil.eventname.Equals(eventname, StringComparison.InvariantCultureIgnoreCase)) select fil).ToArray();

            if (fel.Length == 0)            // no filters match, null
                return null;

            Dictionary<string, ValuesReturned> valuesneeded = new Dictionary<string, ValuesReturned>();

            foreach (FilterEvent fe in fel)        // find all values needed
            {
                foreach (Fields fd in fe.fields)
                {
                    if (valuesneeded.ContainsKey(fd.itemname))
                    {
                        valuesneeded[fd.itemname].needdate |= IsDateComparision(fd.matchtype);
                        valuesneeded[fd.itemname].neednumber |= IsNumberComparision(fd.matchtype);
                    }
                    else
                        valuesneeded[fd.itemname] = new ValuesReturned(IsDateComparision(fd.matchtype), IsNumberComparision(fd.matchtype));

                    //System.Diagnostics.Debug.WriteLine("Need " + fd.itemname + " with "  + fd.matchtype);
                }
            }

            try
            {
                JObject jo = JObject.Parse(json);  // Create a clone

                int togo = valuesneeded.Count;

                foreach (JToken jc in jo.Children())
                {
                    ExpandTokens(jc, valuesneeded, ref togo);
                    if (togo == 0)
                        break;
                }

                bool? outerres = null;

                foreach (FilterEvent fe in fel)        // find all values needed
                {
                    bool? innerres = null;

                    foreach (Fields f in fe.fields)
                    {
                        ValuesReturned vr = valuesneeded[f.itemname];

                        bool matched = false;

                        if (f.matchtype == MatchType.IsPresent)
                        {
                            if (vr.value != null)
                                matched = true;
                        }
                        else if (f.matchtype == MatchType.IsNotPresent)
                        {
                            if (vr.value == null)
                                matched = true;
                        }
                        else if (vr.value == null)      // if item name not found
                        {
                            innerres = false;
                            break;                       // stop the loop, its a false
                        }
                        else if (f.matchtype == MatchType.DateBefore)
                        {
                            matched = vr.datetimegood && vr.datetime.CompareTo(f.contentdate) < 0;
                        }
                        else if (f.matchtype == MatchType.DateAfter)
                        {
                            matched = vr.datetimegood && vr.datetime.CompareTo(f.contentdate) >= 0;
                        }
                        else
                        {
                            if (f.matchtype == MatchType.Equals)
                                matched = vr.value.Equals(f.contentmatch, StringComparison.InvariantCultureIgnoreCase);
                            if (f.matchtype == MatchType.EqualsCaseSensitive)
                                matched = vr.value.Equals(f.contentmatch);

                            else if (f.matchtype == MatchType.NotEqual)
                                matched = !vr.value.Equals(f.contentmatch, StringComparison.InvariantCultureIgnoreCase);
                            else if (f.matchtype == MatchType.NotEqualCaseSensitive)
                                matched = !vr.value.Equals(f.contentmatch);

                            else if (f.matchtype == MatchType.Contains)
                                matched = vr.value.IndexOf(f.contentmatch, StringComparison.InvariantCultureIgnoreCase) >= 0;
                            else if (f.matchtype == MatchType.ContainsCaseSensitive)
                                matched = vr.value.Contains(f.contentmatch);

                            else if (f.matchtype == MatchType.DoesNotContain)
                                matched = vr.value.IndexOf(f.contentmatch, StringComparison.InvariantCultureIgnoreCase) < 0;
                            else if (f.matchtype == MatchType.DoesNotContainCaseSensitive)
                                matched = !vr.value.Contains(f.contentmatch);

                            else if (f.matchtype == MatchType.NumericEquals)
                                matched = vr.numbergood && Math.Abs(vr.number - f.contentvalue) < 0.0000000001;  // allow for rounding

                            else if (f.matchtype == MatchType.NumericNotEquals)
                                matched = vr.numbergood && Math.Abs(vr.number - f.contentvalue) >= 0.0000000001;

                            else if (f.matchtype == MatchType.NumericGreater)
                                matched = vr.numbergood && vr.number > f.contentvalue;

                            else if (f.matchtype == MatchType.NumericGreaterEqual)
                                matched = vr.numbergood && vr.number >= f.contentvalue;

                            else if (f.matchtype == MatchType.NumericLessThan)
                                matched = vr.numbergood && vr.number < f.contentvalue;

                            else if (f.matchtype == MatchType.NumericLessThanEqual)
                                matched = vr.numbergood && vr.number <= f.contentvalue;

                          //  System.Diagnostics.Debug.WriteLine(fe.eventname + ":Compare " + f.matchtype + " '" + f.contentmatch + "' with '" + vr.value + "' res " + matched + " IC " + fe.innercondition);
                        }

                        if (fe.innercondition == FilterType.And )       // Short cut, if AND, all must pass, and it did not
                        {
                            if (!matched)
                            {
                                innerres = false;                       
                                break;
                            }
                        }
                        else if (fe.innercondition == FilterType.Nand)  // Short cut, if NAND, and not matched
                        {
                            if (!matched)
                            {
                                innerres = true;                        // positive non match - NAND produces a true
                                break;
                            }
                        }
                        else if (fe.innercondition == FilterType.Or)    // Short cut, if OR, and matched
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
                        if (fe.innercondition == FilterType.And)        // none did not match, producing a false, so therefore AND is true
                            innerres = true;
                        else if (fe.innercondition == FilterType.Or)    // none did match, producing a true, so therefore OR must be false
                            innerres = false;
                        else if (fe.innercondition == FilterType.Nor)   // none did match, producing a false, so therefore NOR must be true
                            innerres = true;
                        else                                            // NAND none did match, producing a true, so therefore NAND must be false
                            innerres = false;
                    }

                    if (innerres.Value && passed != null)               // if want a list of passes, do it
                        passed.Add(fe);

                    if (!outerres.HasValue)                                 // if first time, its just the value
                        outerres = innerres.Value;
                    else if (fe.outercondition == FilterType.Or)
                        outerres |= innerres.Value;
                    else if (fe.outercondition == FilterType.And)
                        outerres &= innerres.Value;
                    else if (fe.outercondition == FilterType.Nor)
                        outerres = !(outerres | innerres.Value);
                    else if (fe.outercondition == FilterType.Nand)
                        outerres = !(outerres & innerres.Value);
                }

                return outerres;
            }
            catch (Exception)
            {
                return false;
            }
        }
            

        private void ExpandTokens(JToken jt, Dictionary<string, ValuesReturned> valuesneeded , ref int togo )
        {
            JTokenType[] decodeable = { JTokenType.Boolean, JTokenType.Date, JTokenType.Integer, JTokenType.String, JTokenType.Float, JTokenType.TimeSpan };

            if (jt.HasValues && togo > 0 )
            {
                foreach (JToken jc in jt.Children())
                {
                    if (jc.HasValues)
                    {
                        ExpandTokens(jc, valuesneeded , ref togo);
                    }
                    else 
                    {
                        string name = jc.Path;

                        if (valuesneeded.ContainsKey(name) && Array.FindIndex(decodeable, x => x == jc.Type) != -1  )
                        {
                            ValuesReturned vr = valuesneeded[name];
                            vr.value = jc.Value<string>();

                            if (vr.needdate)
                            {
                                try
                                {
                                    vr.datetime = jc.Value<DateTime>();     // if it fails, it excepts, accept this, 
                                    vr.datetimegood = true;
                                }
                                catch
                                {
                                    vr.datetimegood = false;
                                }
                            }

                            if (vr.neednumber)
                                vr.numbergood = double.TryParse(vr.value, out vr.number);

                            togo--;

                           // System.Diagnostics.Debug.WriteLine("Found "+ name);
                        }
                    }

                    if (togo == 0)  // if we have all values, stop
                        break;
                }
            }
        }


        public bool CheckFilterTrueOut(string json, string eventname, ref List<FilterEvent> passed)      // if none, true, if false, true.. 
        {                                                                                         // only if the filter passes do we get a false..
            bool? v = Check(json, eventname, ref passed);
            return !v.HasValue || v.Value == false;
        }

        public bool FilterHistory(HistoryEntry he)                // true if it should be included
        {
            List<FilterEvent> list = null;    // don't want it
            return CheckFilterTrueOut(he.journalEntry.EventDataString, he.journalEntry.EventTypeStr, ref list);     // true it should be included
        }

        public List<HistoryEntry> FilterHistory(List<HistoryEntry> he, out int count)    // filter in all entries
        {
            count = 0;
            if (Count == 0)       // no filters, all in
                return he;
            else
            {
                List<FilterEvent> list = null;    // don't want it
                List<HistoryEntry> ret = (from s in he where CheckFilterTrueOut(s.journalEntry.EventDataString, s.journalEntry.EventTypeStr, ref list) select s).ToList();

                count = he.Count - ret.Count;
                return ret;
            }
        }


        public List<HistoryEntry> MarkHistory(List<HistoryEntry> he, out int count )       // Used for debugging it..
        {
            count = 0;

            if (Count == 0)       // no filters, all in
                return he;
            else
            {
                List<HistoryEntry> ret = new List<HistoryEntry>();

                foreach( HistoryEntry s in he )
                {
                    List<FilterEvent> list = new List<FilterEvent>();    // don't want it

                    int mrk = s.EventDescription.IndexOf(":::");
                    if (mrk >= 0)
                        s.EventDescription = s.EventDescription.Substring(mrk + 3);

                    if ( !CheckFilterTrueOut(s.journalEntry.EventDataString, s.journalEntry.EventTypeStr, ref list))
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

    }
}
