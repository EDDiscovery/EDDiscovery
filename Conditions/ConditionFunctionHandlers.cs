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

        public bool Run(out string output, string funcname)
        {
            if (IsFunction)
            {
                if (paras.Count < fe.numberparasmin)
                    output = "Too few parameters";
                else
                {
                    System.Reflection.MethodInfo mi = GetType().GetMethod(fe.fname, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    func fptr = (func)Delegate.CreateDelegate(typeof(func), this, mi);      // need a delegate which is attached to this instance..
                    bool res = fptr(out output);
                    if (!res)
                        output = "Function " + funcname + ": " + output;
                    return res;
                }
            }
            else if (paras.Count != 1)
                output = "Missing variable name between brackets";
            else if (vars.Exists(paras[0].Value))
            {
                output = vars[paras[0].Value];
                return true;
            }
            else
            {
                output = "Variable '" + paras[0].Value + "' does not exist";
            }

            return false;
        }

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
                    return fe.IsStringAllowed(paras.Count);
            }
        }

        public string ProcessParameter(string t, bool isstr , int recdepth )
        {
            if (fe == null)
                paras.Add(new Parameter() { Value = t });    
            else
            {
                FuncEntry.PT ptype = fe.paratype[paras.Count];

                // if string, and its an expand string type, do it..
                if (isstr)
                {
                    if (fe.IsExpandedString(paras.Count))
                    {
                        string resexp;          // expand out any strings.. recursion
                        ConditionFunctions.ExpandResult sexpresult = caller.ExpandStringFull(t, out resexp, recdepth + 1);

                        if (sexpresult == ConditionFunctions.ExpandResult.Failed)
                            return resexp;

                        t = resexp;
                    }
                }
                else
                {
                    if ( t.Contains("%"))   // expand out any function ones..
                    {
                        string resexp;          // expand out any strings.. recursion
                        ConditionFunctions.ExpandResult sexpresult = caller.ExpandStringFull(t, out resexp, recdepth + 1);

                        if (sexpresult == ConditionFunctions.ExpandResult.Failed)
                            return resexp;

                        t = resexp;
                    }
                }

                if (ptype == FuncEntry.PT.LS)
                {
                }
                else if (ptype == FuncEntry.PT.M)     // macro, or string with macro name, don't check.
                {
                    t = vars.Qualify(t);
                }
                else if (ptype == FuncEntry.PT.ME)     // macro, or string with macro name
                {
                    t = vars.Qualify(t);

                    if (vars.Exists(t))
                        t = vars[t];
                    else
                        return t + " does not exist";
                }
                else if (ptype == FuncEntry.PT.ms)      // macro name, unexpanded, unchecked, or string, unexpanded
                {
                    if ( !isstr )
                        t = vars.Qualify(t);            // qualify any macro name
                }
                else if (ptype == FuncEntry.PT.MESE )   // macro, expand.  or string expand, macro must exist
                {
                    if (!isstr)
                    {
                        t = vars.Qualify(t);
                        if (vars.Exists(t))
                            t = vars[t];
                        else
                            return t + " does not exist";
                    }
                }
                else if (ptype == FuncEntry.PT.LmeSE)           // a Literal, or a macro expanded, or a string expanded
                {
                    if (!isstr)
                    {
                        string mname = vars.Qualify(t);
                        if (vars.Exists(mname))
                            t = vars[mname];
                    }
                }
                else if (ptype == FuncEntry.PT.ImeSE)        
                {
                    if (!isstr)
                    {
                        string mname = vars.Qualify(t);
                        t = vars.Exists(mname) ? vars[mname] : t;        // if it resolved to a macro, get it, else raw text
                    }

                    long? l = t.InvariantParseLongNull();

                    if (l != null)
                    {
                        paras.Add(new Parameter() { Value = t, Int = (int)l.Value, Long = l.Value });
                        return null;
                    }
                    else
                        return "is not an integer";
                }
                else if (ptype == FuncEntry.PT.FmeSE || ptype == FuncEntry.PT.FmeSEBlk )    
                {
                    if (!isstr)
                    {
                        string mname = vars.Qualify(t);
                        t = vars.Exists(mname) ? vars[mname] : t;        // if it resolved to a macro, get it, else raw text
                    }

                    if (t.Length == 0)
                    {
                        if (ptype != FuncEntry.PT.FmeSEBlk) // error if not blank version
                            return "value is empty";        // else fall thru with t = blank
                    }
                    else
                    {
                        double? l = t.InvariantParseDoubleNull();

                        if (l != null)
                        {
                            paras.Add(new Parameter() { Value = t, Fractional = l.Value });
                            return null;
                        }
                        else
                            return "macro does not exist or value is not an number";
                    }
                }
                else
                    System.Diagnostics.Debug.Assert(true);

                paras.Add( new Parameter() { Value = t, isstring = isstr });
            }

            return null;
        }

        #region Class only access

        protected class Parameter
        {
            public string Value;
            public int Int;
            public long Long;
            public double Fractional;
            public bool isstring;
        };

        protected bool ExpandMacroStr(out string output, int parano )   // blank if out of range.  error if macro does not exist
        {
            output = "";

            if (parano < paras.Count)
            {
                string value = paras[parano].Value;

                if (paras[parano].isstring)
                {
                    ConditionFunctions.ExpandResult sexpresult = caller.ExpandStringFull(value, out output, recdepth + 1);

                    if (sexpresult == ConditionFunctions.ExpandResult.Failed)
                        return false;
                }
                else if (vars.Exists(value))
                {
                    output = vars[value];
                }
                else
                {
                    output = "Variable " + value + " does not exist";
                    return false;
                }
            }

            return true;
        }

        protected class FuncEntry
        {
            public enum PT
            {
                LS,         // unnquoted: literal, quoted:Literal

                ME,         // Macro name in both unquoted and quoted, with expansion in string.  Macro expanded, must be present
                M,          // Macro name in both unquoted and quoted, with expansion in string. Not macro expanded.
                ms,         // Macro name in unquoted, unexpanded OR unexpanded string
                MESE,       // Macro name in unquoted, expanded, must be there OR expanded string
                LmeSE,      // Macro name in unquoted, expanded, or its a literal OR expanded string
                ImeSE,      // Macro name check, if present, expanded or literal OR expanded string, followed by a int convert which must work.
                FmeSE,      // Macro name check, if present, expanded or literal OR expanded string, followed by a fractional convert which must work.
                FmeSEBlk,   // as oer FmeSE but allowing a blank string to go thru
            };

            //  public static PT[] allowedstrings = new PT[] { PT.LS, PT.MSE,  PT.mse , PT.ms, PT.MESE, PT.LmeSE };
            public bool IsStringAllowed(int pno) { return true; }

            public static PT[] expandedstrings = new PT[] { PT.M, PT.ME, PT.MESE, PT.LmeSE, PT.ImeSE, PT.FmeSE, PT.FmeSEBlk };
            public bool IsExpandedString(int pno) { return expandedstrings.Contains(paratype[pno]); }

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

        public int ParaCount { get { return paras.Count; } }
        protected List<Parameter> paras;
        protected ConditionFunctions caller;
        protected ConditionVariables vars;
        protected ConditionPersistentData persistentdata;
        protected int recdepth;
        protected FuncEntry fe;
        protected delegate bool func(out string output);
        protected static System.Globalization.CultureInfo ct = System.Globalization.CultureInfo.InvariantCulture;
        protected static Random rnd = new Random();
        protected virtual FuncEntry FindFunction(string name) { return null; }

        #endregion
    }

}
