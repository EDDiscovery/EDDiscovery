using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery
{
    public class Condition
    {
        public string eventname;                        // logical event its associated with
        public List<ConditionEntry> fields;             // its condition fields
        public ConditionEntry.LogicalCondition innercondition;         // condition between fields
        public ConditionEntry.LogicalCondition outercondition;         // condition between this set of Condition and the next set of Condition
        public string action;                           // action associated with a pass
        public string actiondata;                       // any data 

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

        public bool SetOuterCondition(string o)
        {
            return Enum.TryParse<ConditionEntry.LogicalCondition>(o.Replace(" ", ""), out outercondition);
        }

        public void Add(ConditionEntry f)
        {
            if (fields == null)
                fields = new List<ConditionEntry>();
            fields.Add(f);
        }

        public void IndicateValuesNeeded(ref ConditionVariables vr)
        {
            foreach (ConditionEntry fd in fields)
            {
                if (!ConditionEntry.IsNullOperation(fd.matchtype) && !fd.itemname.Contains("%"))     // nulls need no data..  nor does anything with expand in
                    vr[fd.itemname] = null;
            }
        }

        public string ToString(bool includeaction = false, bool multi = false)          // multi means quoting needed for ) as well as comma space
        {
            string ret = "";

            if (includeaction)
                ret += eventname.QuoteString() + ", " + action.QuoteString() + ", " + actiondata.QuoteString() + ", ";

            for (int i = 0; i < fields.Count; i++)
            {
                if (i > 0)
                    ret += " " + innercondition.ToString() + " ";

                if (ConditionEntry.IsNullOperation(fields[i].matchtype))
                    ret += "Condition " + ConditionEntry.OperatorNames[(int)fields[i].matchtype];
                else
                {
                    ret += (fields[i].itemname).QuoteString(bracket: multi) + " " + ConditionEntry.OperatorNames[(int)fields[i].matchtype];

                    if (!ConditionEntry.IsUnaryOperation(fields[i].matchtype))
                        ret += " " + fields[i].matchstring.QuoteString(bracket: multi);
                }
            }

            return ret;
        }

        public string Read( string s , bool includeevent = false, string delimchars = ", ")
        {
            StringParser sp = new StringParser(s);
            return Read(sp, includeevent, delimchars);
        }

        public string Read(StringParser sp, bool includeevent = false, string delimchars = ", ")    // if includeevent is set, it must be there..
        {
            fields = new List<ConditionEntry>();
            innercondition = outercondition = ConditionEntry.LogicalCondition.Or;
            eventname = ""; action = ""; actiondata = "";

            if (includeevent)                                                                   
            {
                if ((eventname = sp.NextQuotedWord(",")) == null || !sp.IsCharMoveOn(',') ||
                    (action = sp.NextQuotedWord(",")) == null || !sp.IsCharMoveOn(',') ||
                    (actiondata = sp.NextQuotedWord(",")) == null || !sp.IsCharMoveOn(','))
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

        public bool AlwaysTrue()
        {
            foreach( ConditionEntry c in fields)
            {
                if (c.matchtype == ConditionEntry.MatchType.AlwaysTrue)
                    return true;
            }

            return false;
        }
    }
}
