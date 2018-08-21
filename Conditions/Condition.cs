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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conditions
{
    public class Condition
    {
        public string eventname;                        // logical event its associated with
        public List<ConditionEntry> fields;             // its condition fields
        public ConditionEntry.LogicalCondition innercondition;         // condition between fields
        public ConditionEntry.LogicalCondition outercondition;         // condition between this set of Condition and the next set of Condition
        public string action;                           // action associated with a pass
        public string actiondata;                       // any data 

        #region Init

        public Condition()
        {
            eventname = action = actiondata = "";
        }

        public Condition(string e, string a, string ad, List<ConditionEntry> f, ConditionEntry.LogicalCondition i = ConditionEntry.LogicalCondition.Or , ConditionEntry.LogicalCondition o = ConditionEntry.LogicalCondition.Or)
        {
            eventname = e;
            action = a;
            actiondata = ad;
            innercondition = i;
            outercondition = o;
            fields = f;
        }

        public Condition(Condition other)   // full clone
        {
            eventname = other.eventname;
            if (other.fields != null)
            {
                fields = new List<ConditionEntry>();
                foreach (ConditionEntry e in other.fields)
                    fields.Add(new ConditionEntry(e));
            }
            innercondition = other.innercondition;
            outercondition = other.outercondition;
            action = other.action;
            actiondata = other.actiondata;
        }

        public bool Create(string e, string a, string d, string i, string o)   // i,o can have spaces inserted into enum
        {
            try
            {
                eventname = e;
                action = a;
                actiondata = d;
                innercondition = (ConditionEntry.LogicalCondition)Enum.Parse(typeof(ConditionEntry.LogicalCondition), i.Replace(" ", ""), true);       // must work, exception otherwise
                outercondition = (ConditionEntry.LogicalCondition)Enum.Parse(typeof(ConditionEntry.LogicalCondition), o.Replace(" ", ""), true);       // must work, exception otherwise
                return true;
            }
            catch { }

            return false;
        }

        #endregion

        #region Manip

        public bool IsAlwaysTrue()
        {
            foreach (ConditionEntry c in fields)
            {
                if (c.matchtype == ConditionEntry.MatchType.AlwaysTrue)
                    return true;
            }

            return false;
        }

        public bool IsAlwaysFalse()
        {
            foreach (ConditionEntry c in fields)
            {
                if (c.matchtype != ConditionEntry.MatchType.AlwaysFalse)
                    return false;
            }

            return true;
        }

        public bool Is(string itemname, ConditionEntry.MatchType mt)        // one condition, of this type
        {
            return fields.Count == 1 && fields[0].itemname == itemname && fields[0].matchtype == mt;
        }

        public void SetAlwaysTrue()
        {
            fields = new List<ConditionEntry>();
            fields.Add(new ConditionEntry("Condition", ConditionEntry.MatchType.AlwaysTrue, ""));
        }

        public void SetAlwaysFalse()
        {
            fields = new List<ConditionEntry>();
            fields.Add(new ConditionEntry("Condition", ConditionEntry.MatchType.AlwaysFalse, ""));
        }

        static public Condition AlwaysTrue()
        {
            Condition cd = new Condition();
            cd.SetAlwaysTrue();
            return cd;
        }

        static public Condition AlwaysFalse()
        {
            Condition cd = new Condition();
            cd.SetAlwaysFalse();
            return cd;
        }

        public bool SetOuterCondition(string o)
        {
            return Enum.TryParse<ConditionEntry.LogicalCondition>(o.Replace(" ", ""), out outercondition);
        }

        public void Set(ConditionEntry f)
        {
            fields = new List<ConditionEntry>() { f };
        }

        public void Add(ConditionEntry f)
        {
            if (fields == null)
                fields = new List<ConditionEntry>();
            fields.Add(f);
        }

        // list into CV the variables needed for the condition entry list

        public void IndicateValuesNeeded(ref ConditionVariables vr)
        {
            foreach (ConditionEntry fd in fields)
            {
                if (!ConditionEntry.IsNullOperation(fd.matchtype) && !fd.itemname.Contains("%"))     // nulls need no data..  nor does anything with expand in
                    vr[fd.itemname] = null;
            }
        }

        #endregion

        #region Input/Output

        public string ToString(bool includeaction = false, bool multi = false)          // multi means quoting needed for ) as well as comma space
        {
            string ret = "";

            if (includeaction)
                ret += eventname.QuoteString(comma: true) + ", " + action.QuoteString(comma: true) + ", " + actiondata.QuoteString(comma:true) + ", ";

            for (int i = 0; fields != null && i < fields.Count; i++)
            {
                if (i > 0)
                    ret += " " + innercondition.ToString() + " ";

                if (ConditionEntry.IsNullOperation(fields[i].matchtype))
                    ret += "Condition " + ConditionEntry.OperatorNames[(int)fields[i].matchtype];
                else
                {
                    ret += (fields[i].itemname).QuoteString(bracket: multi) +               // commas do not need quoting as conditions at written as if always at EOL.
                            " " + ConditionEntry.OperatorNames[(int)fields[i].matchtype];

                    if (!ConditionEntry.IsUnaryOperation(fields[i].matchtype))
                        ret += " " + fields[i].matchstring.QuoteString(bracket: multi);     // commas do not need quoting..
                }
            }

            return ret;
        }

        public string Read( string s , bool includeevent = false, string delimchars = " ")
        {
            BaseUtils.StringParser sp = new BaseUtils.StringParser(s);
            return Read(sp, includeevent, delimchars);
        }

        public string Read(BaseUtils.StringParser sp, bool includeevent = false, string delimchars = " ")    // if includeevent is set, it must be there..
        {                                                                                           // demlimchars is normally space, but can be ") " if its inside a multi.
            fields = new List<ConditionEntry>();
            innercondition = outercondition = ConditionEntry.LogicalCondition.Or;
            eventname = ""; action = ""; actiondata = "";

            if (includeevent)                                                                   
            {
                if ((eventname = sp.NextQuotedWord(", ")) == null || !sp.IsCharMoveOn(',') ||
                    (action = sp.NextQuotedWord(", ")) == null || !sp.IsCharMoveOn(',') ||
                    (actiondata = sp.NextQuotedWord(", ")) == null || !sp.IsCharMoveOn(','))
                {
                    return "Incorrect format of EVENT data associated with condition";
                }
            }

            ConditionEntry.LogicalCondition? ic = null;

            while (true)
            {
                string var = sp.NextQuotedWord(delimchars);             // always has para cond
                if (var == null)
                    return "Missing parameter (left side) of condition";

                string cond = sp.NextQuotedWord(delimchars);
                if (cond == null)
                    return "Missing condition operator";

                ConditionEntry.MatchType mt;
                if (!ConditionEntry.MatchTypeFromString(cond, out mt))
                    return "Condition operator " + cond + " is not recognised";

                string value = "";

                if (ConditionEntry.IsNullOperation(mt)) // null operators (Always..)
                {
                    if (!var.Equals("Condition", StringComparison.InvariantCultureIgnoreCase))
                        return "Condition must preceed fixed result operator";
                    var = "Condition";  // fix case..
                }
                else if (!ConditionEntry.IsUnaryOperation(mt) ) // not unary, require right side
                {
                    value = sp.NextQuotedWord(delimchars);
                    if (value == null)
                        return "Missing value part (right side) of condition";
                }

                ConditionEntry ce = new ConditionEntry() { itemname = var, matchtype = mt, matchstring = value };
                fields.Add(ce);

                if (sp.IsEOL || sp.PeekChar() == ')')           // end is either ) or EOL
                {
                    innercondition = (ic == null) ? ConditionEntry.LogicalCondition.Or : ic.Value;
                    return "";
                }
                else
                {
                    ConditionEntry.LogicalCondition nic;
                    string err = ConditionEntry.GetLogicalCondition(sp, delimchars, out nic);
                    if (err.Length > 0)
                        return err + " for inner condition";

                    if (ic == null)
                        ic = nic;
                    else if (ic.Value != nic)
                        return "Cannot specify different inner conditions between expressions";
                }
            }
        }

        #endregion
    }
}
