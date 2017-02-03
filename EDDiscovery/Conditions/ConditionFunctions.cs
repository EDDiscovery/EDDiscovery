using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery
{
    public class ConditionFunctions
    {
        EDDiscoveryForm ed;
        HistoryList hl;
        HistoryEntry he;
        FuncEntry[] flist;
        static System.Globalization.CultureInfo ct = System.Globalization.CultureInfo.InvariantCulture;

        class Parameter
        {
            public string value;
            public bool isstring;
        };

        delegate bool func(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth);

        class FuncEntry
        {
            public string name;
            public func fn;
            public int numberparasmin;
            public int numberparasmax;
            public int checkvarmap;           // if not a string, check macro
            public int allowstringmap;          // allow a string

            public FuncEntry(string s, func f, int min, int max, int checkmacromapx, int allowstringmapx = 0 )
            {
                name = s; fn = f; numberparasmin = min; numberparasmax = max;
                checkvarmap = checkmacromapx; allowstringmap = allowstringmapx;
            }
        }

        public ConditionFunctions(EDDiscoveryForm e, HistoryList l, HistoryEntry h)
        {
            ed = e; hl = l; he = h;

            flist = new FuncEntry[]                         // first is a bitmap saying if to check for the value is a var
            {                                               // second is a bitmap saying if a string is allowed in this pos
                new FuncEntry("exists",Exists,              1,20,   0,0),   // don't check macros, no strings
                new FuncEntry("expand",Expand,              1,20,   0xfffffff,0xfffffff), // check var, can be string
                new FuncEntry("indirect",Indirect,          1,20,   0,0),   // check var, no strings
                new FuncEntry("splitcaps",SplitCaps,        1,1,    1,1),   //check var, allow strings
                new FuncEntry("sc",SplitCaps,               1,1,    1,1),   //shorter alias for above
                new FuncEntry("ship",Ship,                  1,1,    1,1),   //ship translator
                new FuncEntry("datehour",DateHour,          1,1,    1),     // first is a var, no strings
                new FuncEntry("date",DateCnv,               2,2,    1),     // first is a var, second is not, no strings
                new FuncEntry("findline",FindLine,          2,2,    3,2),   //check var1 and var2, second can be a string
                new FuncEntry("substring",SubString,        3,3,    1,1),   // check var1, var1 can be string, var 2 and 3 can either be macro or ints not strings
                new FuncEntry("indexof",IndexOf,            2,2,    3,3),   // check var1 and 2 if normal, allow string in 1 and 2
                new FuncEntry("lower",Lower,                1,1,    1,1),   // check var1, allow string
                new FuncEntry("upper",Upper,                1,1,    1,1),
                new FuncEntry("trim",Trim,                  1,1,    1,1),
                new FuncEntry("length",Length,              1,1,    1,1),
                new FuncEntry("version",Version,            1,1,    0),     // don't check first para
                new FuncEntry("floor",Floor,                2,2,    1),     // check var1, not var 2 no strings
                new FuncEntry("roundnz",RoundCommon,        4,4,    1),
                new FuncEntry("roundscale",RoundCommon,     5,5,    1),
                new FuncEntry("round",RoundCommon,          3,3,    1),
                new FuncEntry("ifnotempty",Ifnotempty,      2,3,    7,7),   // check var1-3, allow strings var1-3
                new FuncEntry("ifempty",Ifempty,            2,3,    7,7),
                new FuncEntry("iftrue",Iftrue,              2,3,    7,7),   // check var1-3, allow strings var1-3
                new FuncEntry("iffalse",Iffalse,            2,3,    7,7),
                new FuncEntry("ifzero",Ifzero,              2,3,    7,7),   // check var1-3, allow strings var1-3
                new FuncEntry("ifnonzero",Ifnonzero,        2,3,    7,7),
                new FuncEntry("ifcontains",Ifcontains,      3,4,    15,15), // check var1-4, allow strings var1-4
                new FuncEntry("ifnotcontains",Ifnotcontains,3,4,    15,15),
                new FuncEntry("ifequal",Ifequal,            3,4,    15,15),
                new FuncEntry("ifnotequal",Ifnotequal,      3,4,    15,15),
                new FuncEntry("expandarray",ExpandArray,    4,5,    2,2)   // var 1 is text root, not var, not string, var 2 can be var or string, var 3/4 is integers or variables, checked in function
        };
        }

        #region expander

        // true, expanded, result = string
        // false, failed, result = error

        public ConditionLists.ExpandResult ExpandString(string line, ConditionVariables vars, out string result)
        {
            return ExpandStringFull(line, vars, out result, 1);
        }

        public ConditionLists.ExpandResult ExpandStringFull(string line, ConditionVariables vars, out string result, int recdepth)
        {
            int noexpansion = 0;
            int pos = 0;
            do
            {
                pos = line.IndexOf('%', pos);

                if (pos >= 0)
                {
                    pos++;                                                  // move on, if it fails, next pos= will be past this point

                    int startexpression = pos;

                    int apos = pos;

                    if (apos < line.Length)
                    {
                        while (apos < line.Length && char.IsLetter(line[apos]))
                            apos++;

                        if (line[apos] == '(')     // now must be bracket..  if not, its not in form, ignore $
                        {
                            string funcname = line.Substring(pos, apos - pos);
                            apos++;     // past the (

                            List<Parameter> varnames = new List<Parameter>();

                            while (true)
                            {
                                while (apos < line.Length && char.IsWhiteSpace(line[apos])) // remove white space
                                    apos++;

                                if (apos < line.Length && line[apos] == ')' && varnames.Count == 0)        // ) here must be on first only, and is valid
                                {
                                    apos++; // skip by
                                    break;
                                }

                                int start = apos;

                                if (apos < line.Length && line[apos] == '"')
                                {
                                    string res = "";
                                    apos++;
                                    while (apos < line.Length && line[apos] != '"')
                                    {
                                        if (line[apos] == '\\' && (apos + 1) < line.Length && line[apos + 1] == '"')  // if \"
                                        {
                                            apos++;
                                        }

                                        res += line[apos++];
                                    }

                                    if (apos >= line.Length)
                                    {
                                        result = "Terminal quote missing at '" + line.Substring(startexpression, apos - startexpression) + "'";
                                        return ConditionLists.ExpandResult.Failed;
                                    }

                                    apos++;     // remove quote

                                    string resexp;          // expand out any strings.. recursion
                                    ConditionLists.ExpandResult sexpresult = ExpandStringFull(res, vars, out resexp, recdepth + 1);

                                    if (sexpresult == ConditionLists.ExpandResult.Failed)
                                    {
                                        result = resexp;
                                        return sexpresult;
                                    }

                                    varnames.Add(new Parameter() { value = resexp, isstring = true });
                                }
                                else
                                {
                                    while (apos < line.Length && "), ".IndexOf(line[apos]) == -1)
                                        apos++;

                                    if (apos == start)
                                    {
                                        result = "Missing variable name at '" + line.Substring(startexpression, apos - startexpression) + "'";
                                        return ConditionLists.ExpandResult.Failed;
                                    }

                                    varnames.Add(new Parameter() { value = line.Substring(start, apos - start), isstring = false });
                                }

                                while (apos < line.Length && char.IsWhiteSpace(line[apos]))
                                    apos++;

                                char c = (apos < line.Length) ? line[apos++] : '-';

                                if (c == ')')     // must be )
                                    break;

                                if (c != ',')     // must be ,
                                {
                                    result = "Incorrectly formed parameter list at '" + line.Substring(startexpression, apos - startexpression) + "'";
                                    return ConditionLists.ExpandResult.Failed;
                                }
                            }

                            string expand = null;

                            if (funcname.Length > 0)
                            {
                                if (!RunFunction(funcname, varnames, vars, out expand, recdepth))
                                {
                                    result = "Function " + funcname + ": " + expand;
                                    return ConditionLists.ExpandResult.Failed;
                                }
                            }
                            else if (varnames.Count > 1)
                            {
                                result = "Only functions can have multiple comma separated items at '" + line.Substring(startexpression, apos - startexpression) + "'";
                                return ConditionLists.ExpandResult.Failed;
                            }
                            else
                            {
                                if (varnames[0].isstring)
                                {
                                    result = "Must be a variable not a string for non function expansions";
                                    return ConditionLists.ExpandResult.Failed;
                                }
                                else if (vars.ContainsKey(varnames[0].value))
                                    expand = vars[varnames[0].value];
                                else
                                {
                                    result = "Variable " + varnames[0].value + " does not exist";
                                    return ConditionLists.ExpandResult.Failed;
                                }
                            }

                            noexpansion++;
                            line = line.Substring(0, pos - 1) + expand + line.Substring(apos);

                            pos = (pos - 1) + expand.Length;

                            //                            System.Diagnostics.Debug.WriteLine("<" + funcname + "> var <" + varnames[0] + ">" + "  line <" + line + "> left <" + line.Substring(pos) + ">");
                        }
                    }
                }
            } while (pos != -1);

            result = line;
            return (noexpansion > 0) ? ConditionLists.ExpandResult.Expansion : ConditionLists.ExpandResult.NoExpansion;
        }

        // true, output is written.  false, output has error text
        private bool RunFunction(string fname, List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            FuncEntry fe = Array.Find(flist, x => x.name.Equals(fname, StringComparison.InvariantCulture));
            if (fe != null)
            {
                if (paras.Count < fe.numberparasmin)
                    output = "Too few parameters";
                else if (paras.Count > fe.numberparasmax)
                    output = "Too many parameters";
                else
                {
                    for (int i = 0; i < paras.Count; i++)
                    {
                        if (paras[i].isstring)
                        {
                            if (((fe.allowstringmap >> i) & 1) == 0)
                            {
                                output = "Strings are not allowed in parameter " + (i + 1).ToString(ct);
                                return false;
                            }
                        }
                        else if (((fe.checkvarmap >> i) & 1) == 1 && !vars.ContainsKey(paras[i].value))
                        {
                            output = "Variable " + paras[i].value + " does not exist in parameter " + (i + 1).ToString(ct);
                            return false;
                        }
                    }

                    return fe.fn(paras, vars, out output, recdepth);
                }
            }
            else
                output = "Does not exist";

            return false;
        }

        #endregion

        #region Functions

        private bool Exists(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            foreach (Parameter s in paras)
            {
                if (!vars.ContainsKey(s.value))
                {
                    output = "0";
                    return true;
                }
            }

            output = "1";
            return true;
        }

        private bool Expand(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            return ExpandCore(paras, vars, out output, recdepth, false);
        }

        private bool Indirect(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            return ExpandCore(paras, vars, out output, recdepth, true);
        }

        private bool ExpandCore(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth, bool indirect)
        {
            if (recdepth > 9)
            {
                output = "Recursion detected - aborting expansion";
                return false;
            }

            output = "";

            foreach (Parameter p in paras)
            {
                string s = p.value;
                string value;

                if (indirect)
                {
                    if (vars.ContainsKey(vars[s]))
                        value = vars[vars[s]];
                    else
                    {
                        output = "Indrect Variable " + vars[s] + " not found";
                        return false;
                    }
                }
                else
                {
                    if (p.isstring)
                        value = s;
                    else
                        value = vars[s];
                }

                string res;
                ConditionLists.ExpandResult result = ExpandStringFull(value, vars, out res, recdepth + 1);

                if (result == ConditionLists.ExpandResult.Failed)
                {
                    output = res;
                    return false;
                }

                output += res;
            }

            return true;
        }

        private bool SplitCaps(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            string value = (paras[0].isstring) ? paras[0].value : vars[paras[0].value];
            output = value.SplitCapsWordUnderscoreTitleCase();
            return true;
        }

        private bool Ship(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            string value = (paras[0].isstring) ? paras[0].value : vars[paras[0].value];
            output = EliteDangerous.JournalEntry.PhoneticShipName(value);
            output = output.SplitCapsWordUnderscoreTitleCase();
            return true;
        }

        private bool DateCnv(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            string s = paras[0].value;

            DateTime res;

            if (DateTime.TryParse(vars[s], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"),
                                    System.Globalization.DateTimeStyles.None, out res))
            {
                string t = paras[1].value.ToLower();

                if (t.Equals("longtime"))
                {
                    output = res.ToLongTimeString();
                }
                else if (t.Equals("shorttime"))
                {
                    output = res.ToShortTimeString();
                }
                else if (t.Equals("longdate"))
                {
                    output = res.ToLongDateString();
                }
                else if (t.Equals("longdatetime"))
                {
                    output = res.ToLongDateString() + " " + res.ToLongTimeString();
                }
                else if (t.Equals("shortdate"))
                {
                    output = res.ToShortDateString();
                }
                else
                {
                    output = "Format selector not supported";
                    return false;
                }

                return true;
            }
            else
                output = "Date is not in correct en-US format";

            return false;
        }

        private bool DateHour(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            DateTime res;

            if (DateTime.TryParse(vars[paras[0].value], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"),
                                    System.Globalization.DateTimeStyles.None, out res))
            {
                output = res.Hour.ToString(ct);
                return true;
            }
            else
                output = "Date is not in correct en-US format";

            return false;
        }

        private bool FindLine(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            string value = (paras[1].isstring) ? paras[1].value : (vars.ContainsKey(paras[1].value) ? vars[paras[1].value] : null);

            if (value != null)
            {
                using (System.IO.TextReader sr = new System.IO.StringReader(vars[paras[0].value]))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.IndexOf(value, StringComparison.InvariantCultureIgnoreCase) != -1)
                        {
                            output = line;
                            return true;
                        }
                    }
                }

                output = "";
                return true;
            }
            else
                output = "The variable " + paras[1].value + " does not exist";

            return false;
        }

        private bool SubString(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            int start, length;

            bool okstart = paras[1].value.InvariantParse(out start) || (vars.ContainsKey(paras[1].value) && vars[paras[1].value].InvariantParse(out start));
            bool oklength = paras[2].value.InvariantParse(out length) || (vars.ContainsKey(paras[2].value) && vars[paras[2].value].InvariantParse(out length));

            if (okstart && oklength)
            {
                string v = (paras[0].isstring) ? paras[0].value : vars[paras[0].value];

                if (start >= 0 && start < v.Length)
                {
                    if (start + length > v.Length)
                        length = v.Length - start;

                    output = v.Substring(start, length);
                }
                else
                    output = "";

                return true;
            }
            else
                output = "Start and/or length are not integers or variables do not exist";

            return false;
        }

        private bool IndexOf(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            string test = (paras[0].isstring) ? paras[0].value : vars[paras[0].value];
            string value = (paras[1].isstring) ? paras[1].value : vars[paras[1].value];
            output = test.IndexOf(value).ToString(ct);
            return true;
        }

        private bool Lower(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            string value = (paras[0].isstring) ? paras[0].value : vars[paras[0].value];
            output = value.ToLower();
            return true;
        }

        private bool Upper(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            string value = (paras[0].isstring) ? paras[0].value : vars[paras[0].value];
            output = value.ToUpper();
            return true;
        }

        private bool Trim(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            string value = (paras[0].isstring) ? paras[0].value : vars[paras[0].value];
            output = value.Trim();
            return true;
        }

        private bool Length(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            string value = (paras[0].isstring) ? paras[0].value : vars[paras[0].value];
            output = value.Length.ToString(ct);
            return true;
        }

        private bool Version(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            int[] edversion = Tools.GetEDVersion();

            int para;
            if (paras[0].value.InvariantParse(out para) && para >= 1 && para <= edversion.Length)
            {
                output = edversion[para - 1].ToString(ct);
                return true;
            }
            else
            {
                output = "Parameter number must be between 1 and 4";
                return false;
            }
        }

        private bool Floor(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            double para;
            if (vars[paras[0].value].InvariantParse(out para))
            {
                string fmt = vars.ContainsKey(paras[1].value) ? vars[paras[1].value] : paras[1].value;
                if (FormatIt(Math.Floor(para), fmt, out output))
                    return true;
            }
            else
                output = "Parameter number be a number";

            return false;
        }

        private bool RoundCommon(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            int extradigits = 0;

            if (paras.Count >= 4)
            {
                if (!paras[3].value.InvariantParse(out extradigits) && !(vars.ContainsKey(paras[3].value) && vars[paras[3].value].InvariantParse(out extradigits)))
                {
                    output = "The variable " + paras[3] + " does not exist or the value is not an integer";
                    return false;
                }
            }

            double scale = 1.0;
            if (paras.Count >= 5)       // round scale.
            {
                if (!paras[4].value.InvariantParse(out scale) && !(vars.ContainsKey(paras[4].value) && vars[paras[4].value].InvariantParse(out scale)))
                {
                    output = "The variable " + paras[4] + " does not exist of the value is not a fractional";
                    return false;
                }
            }

            double value;

            if (vars[paras[0].value].InvariantParse(out value))
            {
                value *= scale;

                int digits = 0;
                if (paras[1].value.InvariantParse(out digits) || (vars.ContainsKey(paras[1].value) && vars[paras[1].value].InvariantParse(out digits)))
                {
                    string fmt = vars.ContainsKey(paras[2].value) ? vars[paras[2].value] : paras[2].value;

                    double res = Math.Round(value, digits);

                    if (extradigits > 0 && Math.Abs(res) < 0.0000001)     // if rounded to zero..
                    {
                        digits += extradigits;
                        fmt += new string('#', extradigits);
                        res = Math.Round(value, digits);
                    }

                    if (FormatIt(res, fmt, out output))
                        return true;
                }
                else
                    output = "Digits must be a variable or an integer number of digits";
            }
            else
                output = "Variable must be a integer or fractional";

            return false;
        }

        private bool FormatIt(double v, string fmt, out string output)
        {
            output = "";

            if (fmt.StartsWith("M"))
            {
                fmt = fmt.Substring(1);

                if (v < 0)
                {
                    output = "Minus ";
                    v = -v;
                }
            }

            try
            {
                output += v.ToString(fmt, ct);
                return true;
            }
            catch
            {
                output = "Format must be a c# ToString format for doubles";
                return false;
            }
        }

        private bool Ifnotempty(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            return IfCommon(paras, vars, out output, recdepth, IfType.Empty, false);
        }
        private bool Ifempty(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            return IfCommon(paras, vars, out output, recdepth, IfType.Empty, true);
        }
        private bool Iftrue(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            return IfCommon(paras, vars, out output, recdepth, IfType.True, true);
        }
        private bool Iffalse(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            return IfCommon(paras, vars, out output, recdepth, IfType.True, false);
        }
        private bool Ifzero(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            return IfCommon(paras, vars, out output, recdepth, IfType.Zero, true);
        }
        private bool Ifnonzero(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            return IfCommon(paras, vars, out output, recdepth, IfType.Zero, false);
        }
        private bool Ifcontains(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            return IfCommon(paras, vars, out output, recdepth, IfType.Contains, true);
        }
        private bool Ifnotcontains(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            return IfCommon(paras, vars, out output, recdepth, IfType.Contains, false);
        }
        private bool Ifequal(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            return IfCommon(paras, vars, out output, recdepth, IfType.Equals, true);
        }
        private bool Ifnotequal(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            return IfCommon(paras, vars, out output, recdepth, IfType.Equals, false);

        }

        enum IfType { True, Contains, Equals, Empty , Zero };

        private bool IfCommon(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth,
                              IfType iftype, bool test)
        {
            if (iftype == IfType.Empty || iftype == IfType.True || iftype == IfType.Zero)         // these, insert a dummy entry to normalise - we don't have a comparitor
                paras.Insert(1, new Parameter() { value = "", isstring = true });

            string value = (paras[0].isstring) ? paras[0].value : vars[paras[0].value];
            string comparitor = (paras[1].isstring) ? paras[1].value : vars[paras[1].value];

            bool tres;

            if (iftype == IfType.Contains)     
            {
                tres = (value.IndexOf(comparitor, StringComparison.InvariantCultureIgnoreCase) != -1) == test;
            }
            else if (iftype == IfType.Equals)  
            {
                tres = value.Equals(comparitor, StringComparison.InvariantCultureIgnoreCase) == test;
            }
            else if (iftype == IfType.Zero)    
            {
                double nres;
                bool ok = value.InvariantParse(out nres);

                if (!ok)
                {
                    output = "Condition value is not an fractional or integer";
                    return false;
                }

                tres = (Math.Abs(nres) < 0.000001) == test;
            }
            else if (iftype == IfType.Empty)                 // 2 parameters
            {
                tres = (value.Length == 0) == test;
            }
            else
            {
                int nres;
                bool ok = value.InvariantParse(out nres);

                if (!ok)
                {
                    output = "Condition value is not an integer";
                    return false;
                }

                tres = (nres != 0) == test;
            }

            if (tres)
            {
                if (paras[2].isstring)      // string.. already been expanded, don't do it again
                {
                    output = paras[2].value;
                    return true;
                }
                else
                {
                    ConditionLists.ExpandResult result = ExpandStringFull(vars[paras[2].value], vars, out output, recdepth + 1);
                    return (result != ConditionLists.ExpandResult.Failed);
                }
            }
            else if (paras.Count == 4)      // if we have an alternate string
            {
                if (paras[3].isstring)
                {
                    output = paras[3].value;
                    return true;
                }
                else
                {
                    ConditionLists.ExpandResult result = ExpandStringFull(vars[paras[3].value], vars, out output, recdepth + 1);
                    return (result != ConditionLists.ExpandResult.Failed);
                }
            }
            else
            {
                output = "";
                return true;
            }
        }

        private bool ExpandArray(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            string arrayroot = paras[0].value;
            string separ = (paras[1].isstring) ? paras[1].value : vars[paras[1].value];

            int start, length;
            bool okstart = paras[2].value.InvariantParse(out start) || (vars.ContainsKey(paras[2].value) && vars[paras[2].value].InvariantParse(out start));
            bool oklength = paras[3].value.InvariantParse(out length) || (vars.ContainsKey(paras[3].value) && vars[paras[3].value].InvariantParse(out length));

            if (okstart && oklength)
            {
                bool splitcaps = paras.Count == 5 && paras[4].value.Equals("splitcaps", StringComparison.InvariantCultureIgnoreCase);

                output = "";

                for (int i = start; i < start+length; i++)
                {
                    string aname = arrayroot + "[" + i.ToString(ct) + "]";

                    if (vars.ContainsKey(aname))
                    {
                        if (i != start)
                            output += separ;

                        if (splitcaps)
                            output += vars[aname].SplitCapsWordUnderscoreTitleCase();
                        else
                            output += vars[aname];
                    }
                    else
                        break;
                }

                return true;
            }
            else
                output = "Start and/or length are not integers or variables do not exist";

            return false;
        }

        #endregion
    }
}