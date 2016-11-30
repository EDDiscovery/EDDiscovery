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
        };

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

            bool? outerres = null;

            foreach (FilterEvent fe in fel)        // its an OR between them
            {
                bool? innerres = null;

                try
                {
                    JObject jo = JObject.Parse(json);  // Create a clone

                    foreach (JToken jc in jo.Children())
                    {
                        bool? r = ExpandTokens(jc, fe);

                        if (r.HasValue)
                        {
                            innerres = r.Value;
                            break;
                        }
                    }

                    if (!innerres.HasValue)                             // if not set, we set it to a definitive state
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

                    if ( innerres.Value && passed != null )             // if want a list of passes, do it
                        passed.Add(fe);
                }
                catch (Exception)
                {
                    return false;
                }

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

        private bool? ExpandTokens(JToken jt, FilterEvent fe)
        {
            if (jt.HasValues)
            {
                JTokenType[] decodeable = { JTokenType.Boolean, JTokenType.Date, JTokenType.Integer, JTokenType.String, JTokenType.Float, JTokenType.TimeSpan };

                foreach (JToken jc in jt.Children())
                {
                    //System.Diagnostics.Trace.WriteLine(string.Format(" >> Child {0} : {1} {2}", cno, jc.Path, jc.Type.ToString()));
                    if (jc.HasValues)
                    {
                        bool? r = ExpandTokens(jc, fe);

                        if (r.HasValue)
                            return r.Value;
                    }
                    else if (Array.FindIndex(decodeable, x => x == jc.Type) != -1)
                    {
                        string name = jc.Path;
                        string value = jc.Value<string>();

                        Fields f = fe.fields.Find(x => x.itemname.Equals(name));    // unique match against field names and name, can't do two here
                                                                                // which would imply that the JSON has two identical field names..
                        if (f!=null)
                        {
                            bool matched = false;
                            double valued;

                            if (f.matchtype == MatchType.Equals)
                                matched = value.Equals(f.contentmatch,StringComparison.InvariantCultureIgnoreCase);
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
                                matched = double.TryParse(value, out valued) && Math.Abs(valued - f.contentvalue) < 0.0000000001;  // allow for rounding

                            else if (f.matchtype == MatchType.NumericNotEquals)
                                matched = double.TryParse(value, out valued) && Math.Abs(valued - f.contentvalue) >= 0.0000000001;

                            else if (f.matchtype == MatchType.NumericGreater)
                                matched = double.TryParse(value, out valued) && valued > f.contentvalue;

                            else if (f.matchtype == MatchType.NumericGreaterEqual)
                                matched = double.TryParse(value, out valued) && valued >= f.contentvalue;

                            else if (f.matchtype == MatchType.NumericLessThan)
                                matched = double.TryParse(value, out valued) && valued < f.contentvalue;

                            else if (f.matchtype == MatchType.NumericLessThanEqual)
                                matched = double.TryParse(value, out valued) && valued <= f.contentvalue;

                            else if (f.matchtype == MatchType.DateBefore)
                            {
                                DateTime dt = jc.Value<DateTime>();
                                matched = dt.CompareTo(f.contentdate) < 0;
                            }
                            else if (f.matchtype == MatchType.DateAfter)
                            {
                                DateTime dt = jc.Value<DateTime>();
                                matched = dt.CompareTo(f.contentdate) >= 0;
                            }

                        //    System.Diagnostics.Debug.WriteLine(fe.eventname + ":Compare " + f.matchtype + " '" + f.contentmatch + "' with '" + value + "' res " + matched + " IC " + fe.innercondition);

                            if (fe.innercondition == FilterType.And && !matched)     // Short cut, if AND, all must pass, and it did not
                                return false;                               // positive non match

                            if (fe.innercondition == FilterType.Nand && !matched)    // Short cut, if NAND, and not matched
                                return true;                                // positive non match - NAND produces a true

                            if (fe.innercondition == FilterType.Or && matched)       // Short cut, if NOR, and matched
                                return true;                                // positive match

                            if (fe.innercondition == FilterType.Nor && matched)      // Short cut, if NOR, and matched
                                return false;                               // positive non match - any matches produce a false.

                        }
                    }
                }
            }

            return null;        // no result either way
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


        public List<HistoryEntry> MarkHistory(List<HistoryEntry> he)       // Used for debugging it..
        {
            if (Count == 0)       // no filters, all in
                return he;
            else
            {
                List<HistoryEntry> ret = new List<HistoryEntry>();

                foreach( HistoryEntry s in he )
                {
                    List<FilterEvent> list = new List<FilterEvent>();    // don't want it

                    if ( !CheckFilterTrueOut(s.journalEntry.EventDataString, s.journalEntry.EventTypeStr, ref list))
                    {
                        System.Diagnostics.Debug.WriteLine("Filter out " + s.Journalid + " " + s.EntryType + " " + s.EventDescription);
                        s.EventDescription = "!" + list[0].eventname + ":::" + s.EventDescription;
                    }
                    else
                    {
                        int mrk = s.EventDescription.IndexOf(":::");
                        if (mrk >= 0)
                            s.EventDescription = s.EventDescription.Substring(mrk + 3);
                    }

                    ret.Add(s);
                }

                return ret;
            }
        }

    }
}
