/*
 * Copyright © 2017 EDDiscovery development team
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseUtils;

namespace Conditions
{ 
    public class ConditionLists
    {
        private List<Condition> conditionlist = new List<Condition>();
        private List<string> groupname = new List<string>();    // used to associate a group name with a condition..

        public ConditionLists()
        {
        }

        public ConditionLists(ConditionLists other)
        {
            foreach (Condition c in other.conditionlist)
                conditionlist.Add(new Condition(c));        //copy, not reference

            groupname = new List<string>(other.groupname);
        }

        public ConditionLists(string condtext)
        {
            Read(condtext);
        }

        public Condition Get(int n) { return (n < conditionlist.Count) ? conditionlist[n] : null; }
        public string GetGroupName(int n) { return (n < groupname.Count) ? groupname[n] : null; }

        public IEnumerable<Condition> Enumerable { get { return conditionlist; } }

        public void Add(Condition fe, string gr = null)
        {
            conditionlist.Add(fe);
            groupname.Add(gr);
        }

        public void Clear()
        {
            conditionlist.Clear();
            groupname.Clear();
        }

        public int Count { get { return conditionlist.Count; } }


        // check all conditions against these values.
        public bool? CheckAll(ConditionVariables values, out string errlist, List<Condition> passed = null, ConditionFunctions cf = null)            // Check all conditions..
        {
            if (conditionlist.Count == 0)            // no filters match, null
            {
                errlist = null;
                return null;
            }

            return CheckConditions(conditionlist, values, out errlist, passed, cf);
        }

        public bool? CheckConditions(List<Condition> fel, ConditionVariables values, out string errlist, List<Condition> passed = null, ConditionFunctions cf = null)
        {
            errlist = null;

            bool? outerres = null;

            foreach (Condition fe in fel)        // find all values needed
            {
                bool? innerres = null;

                foreach (ConditionEntry f in fe.fields)
                {
                    bool matched = false;

                    if (f.matchtype == ConditionEntry.MatchType.AlwaysTrue || f.matchtype == ConditionEntry.MatchType.AlwaysFalse)
                    {
                        if ( f.itemname.Length == 0 || f.itemname.Equals("Condition", StringComparison.InvariantCultureIgnoreCase))     // empty (legacy) or 
                        {
                            if ( f.matchtype == ConditionEntry.MatchType.AlwaysTrue )
                                matched = true;         // matched, else if false, leave as false.
                        }
                        else
                        {
                            errlist += "AlwaysFalse/True does not have on the left side the word 'Condition'";
                            innerres = false;
                            break;
                        }
                    }
                    else
                    {
                        string leftside = null;
                        ConditionFunctions.ExpandResult er = ConditionFunctions.ExpandResult.NoExpansion;

                        if (cf != null)     // if we have a string expander, try the left side
                        {
                            er = cf.ExpandString(f.itemname, out leftside);

                            if (er == ConditionFunctions.ExpandResult.Failed)        // stop on error
                            {
                                errlist += leftside;     // add on errors..
                                innerres = false;   // stop loop, false
                                break;
                            }
                        }

                        if (f.matchtype == ConditionEntry.MatchType.IsPresent)         // these use f.itemname without any expansion
                        {
                            if (leftside == null || er == ConditionFunctions.ExpandResult.NoExpansion)     // no expansion, must be a variable name
                                leftside = values.Qualify(f.itemname);                 // its a straight variable name, allow any special formatting

                            if (values.Exists(leftside) && values[leftside] != null)
                                matched = true;
                        }
                        else if (f.matchtype == ConditionEntry.MatchType.IsNotPresent)
                        {
                            if (leftside == null || er == ConditionFunctions.ExpandResult.NoExpansion)     // no expansion, must be a variable name
                                leftside = values.Qualify(f.itemname);                 // its a straight variable name, allow any special formatting

                            if (!values.Exists(leftside) || values[leftside] == null)
                                matched = true;
                        }
                        else
                        {
                            if (er == ConditionFunctions.ExpandResult.NoExpansion)     // no expansion, must be a variable name
                            {
                                string qualname = values.Qualify(f.itemname);
                                leftside = values.Exists(qualname) ? values[qualname] : null;   // then lookup.. lookup may also be null if its a pre-def

                                if (leftside == null)
                                {
                                    errlist += "Variable '" + qualname + "' does not exist" + Environment.NewLine;
                                    innerres = false;
                                    break;                       // stop the loop, its a false
                                }
                            }

                            string rightside;

                            if (cf != null)         // if we have a string expander, pass it thru
                            {
                                er = cf.ExpandString(f.matchstring, out rightside);

                                if (er == ConditionFunctions.ExpandResult.Failed)        //  if error, abort
                                {
                                    errlist += rightside;     // add on errors..
                                    innerres = false;   // stop loop, false
                                    break;
                                }
                            }
                            else
                                rightside = f.matchstring;

                            if (f.matchtype == ConditionEntry.MatchType.DateBefore || f.matchtype == ConditionEntry.MatchType.DateAfter)
                            {
                                DateTime tmevalue, tmecontent;
                                if (!DateTime.TryParse(leftside, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), System.Globalization.DateTimeStyles.None, out tmevalue))
                                {
                                    errlist += "Date time not in correct format on left side" + Environment.NewLine;
                                    innerres = false;
                                    break;

                                }
                                else if (!DateTime.TryParse(rightside, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), System.Globalization.DateTimeStyles.None, out tmecontent))
                                {
                                    errlist += "Date time not in correct format on right side" + Environment.NewLine;
                                    innerres = false;
                                    break;
                                }
                                else
                                {
                                    if (f.matchtype == ConditionEntry.MatchType.DateBefore)
                                        matched = tmevalue.CompareTo(tmecontent) < 0;
                                    else
                                        matched = tmevalue.CompareTo(tmecontent) >= 0;
                                }
                            }
                            else if (f.matchtype == ConditionEntry.MatchType.Equals)
                                matched = leftside.Equals(rightside, StringComparison.InvariantCultureIgnoreCase);
                            else if (f.matchtype == ConditionEntry.MatchType.EqualsCaseSensitive)
                                matched = leftside.Equals(rightside);

                            else if (f.matchtype == ConditionEntry.MatchType.NotEqual)
                                matched = !leftside.Equals(rightside, StringComparison.InvariantCultureIgnoreCase);
                            else if (f.matchtype == ConditionEntry.MatchType.NotEqualCaseSensitive)
                                matched = !leftside.Equals(rightside);

                            else if (f.matchtype == ConditionEntry.MatchType.Contains)
                                matched = leftside.IndexOf(rightside, StringComparison.InvariantCultureIgnoreCase) >= 0;
                            else if (f.matchtype == ConditionEntry.MatchType.ContainsCaseSensitive)
                                matched = leftside.Contains(rightside);

                            else if (f.matchtype == ConditionEntry.MatchType.DoesNotContain)
                                matched = leftside.IndexOf(rightside, StringComparison.InvariantCultureIgnoreCase) < 0;
                            else if (f.matchtype == ConditionEntry.MatchType.DoesNotContainCaseSensitive)
                                matched = !leftside.Contains(rightside);
                            else if (f.matchtype == ConditionEntry.MatchType.IsOneOf)
                            {
                                StringParser p = new StringParser(rightside);
                                List<string> ret = p.NextQuotedWordList();

                                if (ret == null)
                                {
                                    errlist += "IsOneOf value list is not in a optionally quoted comma separated form" + Environment.NewLine;
                                    innerres = false;
                                    break;                       // stop the loop, its a false
                                }
                                else
                                {
                                    matched = ret.Contains(leftside, StringComparer.InvariantCultureIgnoreCase);
                                }
                            }
                            else if (f.matchtype == ConditionEntry.MatchType.MatchSemicolon)
                            {
                                string[] list = rightside.Split(';').Select(x => x.Trim()).ToArray();     // split and trim
                                matched = list.Contains(leftside.Trim(), StringComparer.InvariantCultureIgnoreCase); // compare, trimmed, case insensitive
                            }
                            else if (f.matchtype == ConditionEntry.MatchType.MatchCommaList)
                            {
                                StringCombinations sc = new StringCombinations(',');
                                sc.ParseString(rightside);      // parse, give all combinations
                                matched = sc.Permutations.Contains(leftside.Trim(), StringComparer.InvariantCultureIgnoreCase); // compare, trimmed, case insensitive
                            }
                            else if (f.matchtype == ConditionEntry.MatchType.MatchSemicolonList)
                            {
                                StringCombinations sc = new StringCombinations(';');
                                sc.ParseString(rightside);      // parse, give all combinations
                                matched = sc.Permutations.Contains(leftside.Trim(), StringComparer.InvariantCultureIgnoreCase); // compare, trimmed, case insensitive
                            }
                            else if (f.matchtype == ConditionEntry.MatchType.AnyOfAny)
                            {
                                StringParser l = new StringParser(leftside);
                                List<string> ll = l.NextQuotedWordList();

                                StringParser r = new StringParser(rightside);
                                List<string> rl = r.NextQuotedWordList();

                                if (ll == null || rl == null)
                                {
                                    errlist += "AnyOfAny value list is not in a optionally quoted comma separated form on both sides" + Environment.NewLine;
                                    innerres = false;
                                    break;                       // stop the loop, its a false
                                }
                                else
                                {
                                    foreach (string s in ll)        // for all left strings
                                    {
                                        if (rl.Contains(s, StringComparer.InvariantCultureIgnoreCase))  // if right has it..
                                        {
                                            matched = true;     // matched and break
                                            break;
                                        }
                                    }
                                }
                            }
                            else if (f.matchtype == ConditionEntry.MatchType.IsEmpty)
                            {
                                matched = leftside.Length == 0;
                            }
                            else if (f.matchtype == ConditionEntry.MatchType.IsNotEmpty)
                            {
                                matched = leftside.Length > 0;
                            }
                            else if (f.matchtype == ConditionEntry.MatchType.IsTrue || f.matchtype == ConditionEntry.MatchType.IsFalse)
                            {
                                int inum = 0;

                                if (leftside.InvariantParse(out inum))
                                    matched = (f.matchtype == ConditionEntry.MatchType.IsTrue) ? (inum != 0) : (inum == 0);
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

                                if (!leftside.InvariantParse(out num))
                                {
                                    errlist += "Number not in correct format on left side" + Environment.NewLine;
                                    innerres = false;
                                    break;
                                }
                                else if (!rightside.InvariantParse(out fnum))
                                {
                                    errlist += "Number not in correct format on right side" + Environment.NewLine;
                                    innerres = false;
                                    break;
                                }
                                else
                                {
                                    if (f.matchtype == ConditionEntry.MatchType.NumericEquals)
                                        matched = Math.Abs(num - fnum) < 0.0000000001;  // allow for rounding

                                    else if (f.matchtype == ConditionEntry.MatchType.NumericNotEquals)
                                        matched = Math.Abs(num - fnum) >= 0.0000000001;

                                    else if (f.matchtype == ConditionEntry.MatchType.NumericGreater)
                                        matched = num > fnum;

                                    else if (f.matchtype == ConditionEntry.MatchType.NumericGreaterEqual)
                                        matched = num >= fnum;

                                    else if (f.matchtype == ConditionEntry.MatchType.NumericLessThan)
                                        matched = num < fnum;

                                    else if (f.matchtype == ConditionEntry.MatchType.NumericLessThanEqual)
                                        matched = num <= fnum;
                                    else
                                        System.Diagnostics.Debug.Assert(false);
                                }
                            }
                        }
                    }

                    //  System.Diagnostics.Debug.WriteLine(fe.eventname + ":Compare " + f.matchtype + " '" + f.contentmatch + "' with '" + vr.value + "' res " + matched + " IC " + fe.innercondition);

                    if (fe.innercondition == ConditionEntry.LogicalCondition.And)       // Short cut, if AND, all must pass, and it did not
                    {
                        if (!matched)
                        {
                            innerres = false;
                            break;
                        }
                    }
                    else if (fe.innercondition == ConditionEntry.LogicalCondition.Nand)  // Short cut, if NAND, and not matched
                    {
                        if (!matched)
                        {
                            innerres = true;                        // positive non match - NAND produces a true
                            break;
                        }
                    }
                    else if (fe.innercondition == ConditionEntry.LogicalCondition.Or)    // Short cut, if OR, and matched
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
                    if (fe.innercondition == ConditionEntry.LogicalCondition.And)        // none did not match, producing a false, so therefore AND is true
                        innerres = true;
                    else if (fe.innercondition == ConditionEntry.LogicalCondition.Or)    // none did match, producing a true, so therefore OR must be false
                        innerres = false;
                    else if (fe.innercondition == ConditionEntry.LogicalCondition.Nor)   // none did match, producing a false, so therefore NOR must be true
                        innerres = true;
                    else                                            // NAND none did match, producing a true, so therefore NAND must be false
                        innerres = false;
                }

                if (innerres.Value && passed != null)               // if want a list of passes, do it
                    passed.Add(fe);

                if (!outerres.HasValue)                                 // if first time, its just the value
                    outerres = innerres.Value;
                else if (fe.outercondition == ConditionEntry.LogicalCondition.Or)
                    outerres |= innerres.Value;
                else if (fe.outercondition == ConditionEntry.LogicalCondition.And)
                    outerres &= innerres.Value;
                else if (fe.outercondition == ConditionEntry.LogicalCondition.Nor)
                    outerres = !(outerres | innerres.Value);
                else if (fe.outercondition == ConditionEntry.LogicalCondition.Nand)
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
                if (f.innercondition != ConditionEntry.LogicalCondition.Or)
                    j1["ICond"] = f.innercondition.ToString();
                if (f.outercondition != ConditionEntry.LogicalCondition.Or)
                    j1["OCond"] = f.outercondition.ToString();
                if (f.action.Length > 0)
                    j1["Actions"] = f.action;
                if (f.actiondata.Length > 0)
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
                    ConditionEntry.LogicalCondition ftinner = (ConditionEntry.LogicalCondition)Enum.Parse(typeof(ConditionEntry.LogicalCondition), j["ICond"].Str("Or"));
                    ConditionEntry.LogicalCondition ftouter = (ConditionEntry.LogicalCondition)Enum.Parse(typeof(ConditionEntry.LogicalCondition), j["OCond"].Str("Or"));
                    string act = j["Actions"].Str();
                    string actd = j["ActionData"].Str();

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
                            matchtype = (ConditionEntry.MatchType)Enum.Parse(typeof(ConditionEntry.MatchType), matchtype)
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

        public override string ToString()
        {
            string ret = "";
            bool multi = conditionlist.Count > 1;

            if (multi)
                ret += "(";

            for (int j = 0; j < conditionlist.Count; j++)       // First outer is NOT used, second on is..
            {
                Condition f = conditionlist[j];

                if (j > 0)
                    ret += ") " + f.outercondition.ToString() + " (";

                ret += f.ToString(multi: multi);
            }

            if (multi)
                ret += ")";

            return ret;
        }

        public string Read(string line)         // decode a set of multi conditions (<cond> Or <cond>) Outer (<cond> And <cond>) etc 
        {
            StringParser sp = new StringParser(line);

            bool multi = false;

            string delimchars = " ";
            if (sp.IsCharMoveOn('('))
            {
                multi = true;
                delimchars = ") ";
            }

            List<Condition> cllist = new List<Condition>();

            ConditionEntry.LogicalCondition outercond = ConditionEntry.LogicalCondition.Or;         // first outer condition is ignored in a list.  Or is the default.

            while (true)
            {
                Condition c = new Condition();

                string err = c.Read(sp, delimchars: delimchars);
                if (err.Length > 0)
                    return err;

                c.outercondition = outercond;
                cllist.Add(c);      // add..

                if (sp.IsCharMoveOn(')'))      // if closing bracket..
                {
                    if (!multi)
                        return "Closing condition bracket found but no opening bracket present";

                    if (sp.IsEOL)  // EOL, end of  (cond..cond) outercond ( cond cond)
                    {
                        conditionlist = cllist;
                        return null;
                    }
                    else
                    {
                        err = ConditionEntry.GetLogicalCondition(sp, delimchars, out outercond);
                        if (err.Length > 0)
                            return err + " for outer condition";

                        if (!sp.IsCharMoveOn('(')) // must have another (
                            return "Missing opening bracket in multiple condition list after " + outercond.ToString();
                    }
                }
                else if (sp.IsEOL) // last condition
                {
                    if (multi)
                        return "Missing closing braket in multiple condition list";

                    conditionlist = cllist;
                    return null;
                }
                else
                    return "Extra characters beyond expression";
            }
        }

        #region Helpers

        // is condition flag in actiondata set

        public bool IsConditionFlagSet(string flagstart)
        {
            foreach (Condition l in conditionlist)
            {
                if (l.actiondata.StartsWith(flagstart))
                    return true;
            }

            return false;
        }

        // Event name.. give me conditions which match that name or ALL
        // flagstart, if not null ,compare with start of action data and include only if matches

        public List<Condition> GetConditionListByEventName(string eventname, string flagstart = null)
        {
            List<Condition> fel;

            if (flagstart != null)
                fel = (from fil in conditionlist
                       where
                     (fil.eventname.Equals("All") || fil.eventname.Equals(eventname, StringComparison.InvariantCultureIgnoreCase)) &&
                     fil.actiondata.StartsWith(flagstart)
                       select fil).ToList();

            else
                fel = (from fil in conditionlist
                       where
                     (fil.eventname.Equals("All") || fil.eventname.Equals(eventname, StringComparison.InvariantCultureIgnoreCase))
                       select fil).ToList();

            return (fel.Count == 0) ? null : fel;
        }

        // give back all conditions which match itemname and have a compatible matchtype.. used for key presses/voice input to compile a list of condition data to check for

        public List<Tuple<string, ConditionEntry.MatchType>> ReturnValuesOfSpecificConditions(string itemname, List<ConditionEntry.MatchType> matchtypes)      // given itemname, give me a list of values it is matched against
        {
            List<Tuple<string, ConditionEntry.MatchType>> ret = new List<Tuple<string, ConditionEntry.MatchType>>();

            foreach (Condition fe in conditionlist)        // find all values needed
            {
                foreach (ConditionEntry ce in fe.fields)
                {
                    if (ce.itemname.Equals(itemname) && matchtypes.Contains(ce.matchtype))
                        ret.Add(new Tuple<string, ConditionEntry.MatchType>(ce.matchstring, ce.matchtype));
                }
            }

            return ret;
        }

        #endregion

        #region Filtering system using the filter set up in this class

        // take conditions and JSON, decode it, execute..
        public bool? CheckCondition(   List<Condition> fel, 
                                        Object cls , // object with data in it
                                        ConditionVariables[] othervars,   // any other variables to present to the condition, in addition to the class variables
                                        out string errlist,     // null if okay..
                                        List<Condition> passed)            // null or conditions passed
        {
            errlist = null;

            ConditionVariables valuesneeded = new ConditionVariables();

            foreach (Condition fe in fel)        // find all values needed
                fe.IndicateValuesNeeded(ref valuesneeded);

            try
            {
                valuesneeded.GetValuesIndicated(cls);       // given the class data, and the list of values needed, add it
                valuesneeded.Add(othervars);
                return CheckConditions(fel, valuesneeded, out errlist, passed);    // and check, passing in the values collected against the conditions to test.
            }
            catch (Exception)
            {
                errlist = "CL:669 class failed to parse";
                return null;
            }
        }

        // TRUE if filter is True and has value

        public bool CheckFilterTrue(Object cls, ConditionVariables[] othervars, out string errlist, List<Condition> passed)      // if none, true, if false, true.. 
        {                                                                                         // only if the filter passes do we get a false..
            bool? v = CheckCondition(conditionlist, cls, othervars, out errlist, passed);
            return (v.HasValue && v.Value);     // true IF we have a positive result
        }

        // Filter OUT if condition matches..

        public bool CheckFilterFalse(Object cls, string eventname, ConditionVariables[] othervars, out string errlist , List<Condition> passed)      // if none, true, if false, true.. 
        {
            List<Condition> fel = GetConditionListByEventName(eventname);       // first find conditions applicable, filtered by eventname

            if (fel != null)        // if we have matching filters..
            {
                bool? v = CheckCondition(fel, cls, othervars, out errlist, passed);  // true means filter matched
                bool res = !v.HasValue || v.Value == false;
                //System.Diagnostics.Debug.WriteLine("Event " + eventname + " res " + res + " v " + v + " v.hv " + v.HasValue);
                return res; // no value, true .. false did not match, thus true
            }
            else
            {
                errlist = null;
                return true;
            }
        }

        #endregion
    }
}
