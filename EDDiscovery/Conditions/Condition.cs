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

        public string ToString(bool includeaction = false, bool multi = false)
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

        public string Read(StringParser sp, bool includeevent = false, string delimchars = ", ")
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

            string innercond = null;

            while (true)
            {
                string var = sp.NextQuotedWord(delimchars);
                string cond = sp.NextQuotedWord(delimchars);

                if (var == null || cond == null)
                    return "Missing parts of condition";

                ConditionEntry.MatchType mt;

                if (!ConditionEntry.MatchTypeFromString(cond, out mt))
                    return "Operator is not recognised";

                string value = "";

                if (!ConditionEntry.IsUnaryOperation(mt) && !ConditionEntry.IsNullOperation(mt))
                {
                    value = sp.NextQuotedWord(delimchars);
                    if (value == null)
                        return "Missing parts of condition";
                }

                ConditionEntry ce = new ConditionEntry() { itemname = var, matchtype = mt, matchstring = value };
                fields.Add(ce);

                if (sp.IsEOL || sp.PeekChar() == ')')
                {
                    if (innercond == null)
                        innercond = "Or";
                    innercondition = (ConditionEntry.LogicalCondition)Enum.Parse(typeof(ConditionEntry.LogicalCondition), innercond.Replace(" ", ""), true);       // must work, exception otherwise
                    return "";
                }

                string condi = sp.NextQuotedWord(delimchars);

                if (innercond == null)
                    innercond = condi;
                else if (!innercond.Equals(condi, StringComparison.InvariantCultureIgnoreCase))
                    return "Differing inner conditions incorrect";
            }
        }
    }
}
