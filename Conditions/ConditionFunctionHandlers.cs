using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conditions
{
    public class ConditionFunctionHandlers
    {
        public class Parameter
        {
            public string value;
            public bool isstring;
        };

        public List<Parameter> paras;

        protected ConditionFunctions caller;
        protected ConditionVariables vars;
        protected ConditionPersistentData persistentdata;
        protected int recdepth;

        protected delegate bool func(out string output);

        protected class FuncEntry
        {
            public const int String = 1;    // can be string
            public const int Macro = 2;     // must be macro if not string
            public const int Literal = 4;     // may not be a macro if not string
            public const int ML = Macro + Literal;     // must be macro if not string
            public const int MS = Macro + String;     // must be macro if not string
            public const int LS = Literal + String;     // must be macro if not string
            public int[] paratype;
            public string fname;
            public int numberparasmin;

            public FuncEntry(func f, int minimum, params int[] list)
            {
                fname = f.Method.Name;
                numberparasmin = minimum;
                paratype = list;
            }

            public FuncEntry(func f, int minimum, int type, int repeat)
            {
                fname = f.Method.Name;
                numberparasmin = minimum;
                paratype = new int[repeat];
                for (int i = 0; i < repeat; i++)
                    paratype[i] = type;
            }
        }

        protected static System.Globalization.CultureInfo ct = System.Globalization.CultureInfo.InvariantCulture;
        protected static Random rnd = new Random();

        protected virtual FuncEntry FindFunction(string name) { return null; }

        public ConditionFunctionHandlers(ConditionFunctions c, ConditionVariables v, ConditionPersistentData h, int recd)
        {
            caller = c;
            vars = v;
            persistentdata = h;
            recdepth = recd;
            paras = new List<Parameter>();
        }

        public bool RunFunction(string fname, out string output)
        {
            FuncEntry fe = FindFunction(fname.ToLower());       // function names are case insensitive

            if (fe != null)
            {
                if (paras.Count < fe.numberparasmin)
                    output = "Too few parameters";
                else if (paras.Count > fe.paratype.Length)
                    output = "Too many parameters";
                else
                {
                    for (int i = 0; i < paras.Count; i++)
                    {
                        if (paras[i].isstring)
                        {
                            if ( (fe.paratype[i] & FuncEntry.String) == 0 )
                            {
                                output = "Strings are not allowed in parameter " + (i + 1).ToString(ct);
                                return false;
                            }
                        }
                        else if ( (fe.paratype[i] & FuncEntry.ML ) == FuncEntry.Macro && !vars.Exists(paras[i].value))     // if must be macro
                        {
                            output = "Variable " + paras[i].value + " does not exist in parameter " + (i + 1).ToString(ct);
                            return false;
                        }
                    }

                    System.Reflection.MethodInfo mi = GetType().GetMethod(fe.fname, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    func fptr = (func)Delegate.CreateDelegate(typeof(func), this, mi);      // need a delegate which is attached to this instance..
                    return fptr(out output);
                }
            }
            else
                output = "Does not exist";

            return false;
        }
    }

}
