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

            IsOneOf,            // is it one of a quoted comma list..

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
                                       ">=",
                                       "<",
                                       "IsPresent",
                                       "NotPresent",
                                       "IsOneOf",
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
