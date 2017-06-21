using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery
{
    public class ConditionEntry
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

            IsEmpty,            // string
            IsNotEmpty,         // string

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

            IsOneOf,            // left, is it one of a quoted comma list on right
            AnyOfAny,           // is any in a comma separ on left, one of a quoted comma list on right

            AlwaysTrue,         // Always true
            AlwaysFalse,          // never true
        };

        public static bool IsNullOperation(MatchType matchtype) { return matchtype == MatchType.AlwaysTrue || matchtype == MatchType.AlwaysFalse; }
        public static bool IsUnaryOperation(MatchType matchtype) { return matchtype == MatchType.IsNotPresent || matchtype == MatchType.IsPresent || matchtype == MatchType.IsTrue || matchtype == MatchType.IsFalse || matchtype == MatchType.IsEmpty || matchtype == MatchType.IsNotEmpty; }

        public static bool IsNullOperation(string matchname)
        {
            MatchType mt;
            return MatchTypeFromString(matchname, out mt) && IsNullOperation(mt);
        }

        public static bool IsUnaryOperation(string matchname)
        {
            MatchType mt;
            return MatchTypeFromString(matchname, out mt) && IsUnaryOperation(mt);
        }

        static public bool MatchTypeFromString(string s, out MatchType mt)
        {
            int indexof = Array.FindIndex(MatchNames, x => x.Equals(s, StringComparison.InvariantCultureIgnoreCase));

            if (indexof == -1)
                indexof = Array.FindIndex(OperatorNames, x => x.Equals(s, StringComparison.InvariantCultureIgnoreCase));

            if (indexof >= 0)
            {
                mt = (MatchType)(indexof);
                return true;
            }
            else if (Enum.TryParse<MatchType>(s, out mt))
                return true;
            else
            {
                mt = MatchType.Contains;
                return false;
            }
        }

        static public string[] MatchNames = { "Contains",       // used for display
                                       "Not Contains",
                                       "== (Str)",
                                       "!= (Str)",
                                       "Contains(CS)",
                                       "Not Contains(CS)",
                                       "== (CS)",
                                       "!= (CS)",
                                       "Is Empty",
                                       "Is Not Empty",
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
                                       "Is One Of",
                                       "Any Of Any",
                                       "Always True/Enable",
                                       "Always False/Disable"
                                    };

        static public string[] OperatorNames = {        // used for ASCII rep .
                                       "Contains",
                                       "NotContains",
                                       "$==",
                                       "$!=",
                                       "CSContains",
                                       "CSNotContains",
                                       "CS==",
                                       "CS!=",
                                       "Empty",
                                       "IsNotEmpty",
                                       "IsTrue",
                                       "IsFalse",
                                       "==",
                                       "!=",
                                       ">",
                                       ">=",
                                       "<",
                                       "<=",
                                       "D>=",
                                       "D<",
                                       "IsPresent",
                                       "NotPresent",
                                       "IsOneOf",
                                       "AnyOfAny",
                                       "AlwaysTrue",
                                       "AlwaysFalse"
                                    };

        public enum LogicalCondition
        {
            Or,     // any true     (DEFAULT)
            And,    // all true
            Nor,    // any true produces a false
            Nand,   // any not true produces a true
        }

        public ConditionEntry()
        {
            itemname = matchstring = "";
        }

        public ConditionEntry(string i, MatchType m, string s)
        {
            itemname = i;
            matchtype = m;
            matchstring = s;
        }

        public ConditionEntry(ConditionEntry other)
        {
            itemname = other.itemname;
            matchtype = other.matchtype;
            matchstring = other.matchstring;
        }


        static public string GetLogicalCondition(StringParser sp, string delimchars, out LogicalCondition value)
        {
            value = LogicalCondition.Or;

            string condi = sp.NextQuotedWord(delimchars);       // next is the inner condition..

            if (condi == null)
                return "Condition operator missing";

            if (Enum.TryParse<ConditionEntry.LogicalCondition>(condi.Replace(" ", ""), out value))
                return "";
            else
                return "Condition operator " + condi + " is not recognised";
        }

        public string itemname;
        public MatchType matchtype;                     // true: Contents match for true, else contents dont match for true
        public string matchstring;                     // always set

        public bool Create(string i, string ms, string v)     // ms can have spaces inserted into enum
        {
            if (MatchTypeFromString(ms, out matchtype))
            {
                itemname = i;
                matchstring = v;

                return true;
            }
            else
                return false;
        }

    };

}
