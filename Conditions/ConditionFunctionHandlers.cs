using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conditions
{
    // Class holding parameters and can call functions.  inherit from this, and override find function to add on functions
    // done this way for historical reasons instead of having a set of ptrs to classes handling functions.


    public class ConditionFunctionHandlers
    {
        public class Parameter
        {
            public string Value;
            public long Integer;
            public double Fractional;
        };

        public List<Parameter> paras;

        public bool IsNextParameterAllowed
        {
            get
            {
                if (fe == null)
                    return paras.Count == 0;
                else
                    return paras.Count < fe.numberparasmax;
            }
        }

        public bool IsNextStringAllowed
        {
            get
            {
                if (fe == null)
                    return false;
                else
                    return fe.IsStringAllowed(paras.Count - 1);
            }
        }

        public bool ProcessParameter(string t, bool isstring)
        {
            next do this.. look at its type, and process it, expand it, etc.. so its ready for function..

            paras.Add(new Parameter() { Value = t });
            return true;
        }

        protected ConditionFunctions caller;
        protected ConditionVariables vars;
        protected ConditionPersistentData persistentdata;
        protected int recdepth;
        protected FuncEntry fe;

        protected delegate bool func(out string output);

        protected class FuncEntry
        {
            public enum PT
            {
                L,          // unnquoted: literal, quoted:Not allowed
                LS,         // unnquoted: literal, quoted:Literal
                LMe,        // unquoted: macro name (may be present) or literal otherwise, expanded, quoted: Not allowed
                LMeLS,      // unquoted: macro name (may be present) or literal otherwise, expanded, quoted: Literal string
                ME,         // unquoted: macro name (must be present), expanded, quoted: Not allowed
                MESE,       // unquoted: macro name (must be present), expanded, quoted: macro name (must be present), expanded
                MELS,       // unquoted: macro name (must be present), expanded, quoted: literal string
                M,          // unquoted: macro name (must be present), unexpanded, quoted: Not allowed
                MS,         // unquoted: macro name (must be present), unexpanded, quoted: macro name, unexpanded
                MNP,        // unquoted: macro name (may be present), unexpanded, quoted: Not allowed
                MNPS,       // unquoted: macro name (may be present), unexpanded, quoted: macro name (may be present), unexpanded
                FME,        // unquoted: Either a macro name expanded, or a literal float number
                IME,        // unquoted: Either a macro name expanded, or a literal integer number
            };

            public static PT[] allowedstrings = new PT[] { PT.LS, PT.LMeLS, PT.MESE, PT.MELS, PT.MNPS };

            public bool IsStringAllowed(int pno) { return allowedstrings.Contains(paratype[pno]); }

            public PT[] paratype;
            public string fname;
            public int numberparasmin;

            public int numberparasmax { get { return paratype.Length; } }

            public FuncEntry(func f)
            {
                fname = f.Method.Name;
                numberparasmin = 0;
                paratype = new PT[0];
            }

            public FuncEntry(func f, int minimum, params PT[] list)
            {
                fname = f.Method.Name;
                numberparasmin = minimum;
                paratype = list;
            }

            public FuncEntry(func f, params PT[] list)
            {
                fname = f.Method.Name;
                numberparasmin = list.Length;
                paratype = list;
            }

            public FuncEntry(func f, int minimum, int max, PT type)
            {
                fname = f.Method.Name;
                numberparasmin = minimum;
                paratype = new PT[max];
                for (int i = 0; i < max; i++)
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

        public bool SetFunction(string funcname)
        {
            fe = FindFunction(funcname);
            return fe != null;
        }

        public bool IsFunction { get { return fe != null; } }

        public bool RunFunction(out string output)
        {
            if (paras.Count < fe.numberparasmin)
                output = "Too few parameters";
            else if (paras.Count > fe.paratype.Length)
                output = "Too many parameters";
            else
            {
                //for (int i = 0; i < paras.Count; i++)
                //{
                //    if (paras[i].isstring)
                //    {
                //        if ((fe.paratype[i] & FuncEntry.String) == 0)
                //        {
                //            output = "Strings are not allowed in parameter " + (i + 1).ToString(ct);
                //            return false;
                //        }
                //    }
                //    else if ((fe.paratype[i] & FuncEntry.ML) == FuncEntry.Macro && !vars.Exists(paras[i].value))     // if must be macro
                //    {
                //        output = "Variable " + paras[i].value + " does not exist in parameter " + (i + 1).ToString(ct);
                //        return false;
                //    }
                //}

                System.Reflection.MethodInfo mi = GetType().GetMethod(fe.fname, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                func fptr = (func)Delegate.CreateDelegate(typeof(func), this, mi);      // need a delegate which is attached to this instance..
                return fptr(out output);
            }

            return false;
        }
    }

}
