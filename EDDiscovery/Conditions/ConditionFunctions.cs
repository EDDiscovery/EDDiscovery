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

        delegate bool func(List<string> paras, ConditionVariables vars, out string output , int recdepth );

        class FuncEntry
        {
            public string name;
            public func fn;
            public int numberparasmin;
            public int numberparasmax;

            public FuncEntry(string s, func f, int min, int max) { name = s; fn = f; numberparasmin = min; numberparasmax = max; }
        }

        public ConditionFunctions(EDDiscoveryForm e, HistoryList l , HistoryEntry h)
        {
            ed = e; hl = l; he = h;

            flist = new FuncEntry[]
            {
                new FuncEntry("exists",Exists,1,20),
                new FuncEntry("expand",Expand,1,20),
                new FuncEntry("indirect",Indirect,1,20),
                new FuncEntry("splitcaps",SplitCaps,1,1),
                new FuncEntry("datelong",DateLong,2,2),
                new FuncEntry("datehour",DateHour,1,1),
                new FuncEntry("findline",FindLine,2,2),
                new FuncEntry("substring",SubString,3,3),
                new FuncEntry("indexof",IndexOf,2,2),
                new FuncEntry("length",Length,1,1),
                new FuncEntry("version",Version,1,1),
                new FuncEntry("floor",Floor,2,2),
                new FuncEntry("roundnz",Roundnz,4,4),
                new FuncEntry("round",Round,3,3),
                new FuncEntry("lower",Lower,1,2),
                new FuncEntry("upper",Upper,1,2),
                new FuncEntry("trim",Trim,1,2)
        };
        }

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

                            List<string> varnames = new List<string>();

                            while (true)
                            {
                                while (apos < line.Length && char.IsWhiteSpace(line[apos])) // remove white space
                                    apos++;

                                if (apos < line.Length && line[apos] == ')' && varnames.Count == 0 )        // ) here must be on first only, and is valid
                                {
                                    apos++; // skip by
                                    break;
                                }

                                int start = apos;

                                while (apos < line.Length && "), ".IndexOf(line[apos])==-1 )
                                    apos++;

                                if (apos == start)
                                {
                                    result = "Missing variable name at '" + line.Substring(startexpression, apos - startexpression) + "'";
                                    return ConditionLists.ExpandResult.Failed;
                                }

                                varnames.Add(line.Substring(start, apos - start));

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
                                if (!RunFunction(funcname, varnames, vars, out expand,recdepth))
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
                                if (vars.ContainsKey(varnames[0]))
                                    expand = vars[varnames[0]];
                                else
                                {
                                    result = "Variable " + varnames[0] + " does not exist";
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
        public bool RunFunction(string fname, List<string> paras, ConditionVariables vars, out string output, int recdepth)
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
                    return fe.fn(paras, vars, out output ,recdepth);
                }
            }
            else
                output = "Does not exist";

            return false;
        }

        #region Functions

        private bool Exists(List<string> paras, ConditionVariables vars, out string output, int recdepth)
        {
            foreach (string s in paras)
            {
                if (!vars.ContainsKey(s))
                {
                    output = "0";
                    return true;
                }
            }

            output = "1";
            return true;
        }

        private bool Expand(List<string> paras, ConditionVariables vars, out string output, int recdepth)
        {
            return ExpandCore(paras, vars, out output, recdepth, false);
        }

        private bool Indirect(List<string> paras, ConditionVariables vars, out string output, int recdepth)
        {
            return ExpandCore(paras, vars, out output, recdepth, true);
        }

        private bool ExpandCore(List<string> paras, ConditionVariables vars, out string output, int recdepth, bool indirect)
        {
            if ( recdepth > 9 )
            {
                output = "Recursion detected - aborting expansion";
                return false;
            }

            output = "";

            foreach (string s in paras)
            {
                if (vars.ContainsKey(s))
                {
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
                        value = vars[s];

                    string res;
                    ConditionLists.ExpandResult result = ExpandStringFull(value, vars, out res, recdepth + 1);

                    if (result == ConditionLists.ExpandResult.Failed)
                    {
                        output = res;
                        return false;
                    }

                    output += res;
                }
                else
                {
                    output = "Variable " + s + " does not exist";
                    return false;
                }
            }

            return true;
        }

        private bool SplitCaps(List<string> paras, ConditionVariables vars, out string output, int recdepth)
        {
            if (vars.ContainsKey(paras[0]))
            {
                output = Tools.SplitCapsWord(vars[paras[0]]);
                return true;
            }
            else
            {
                output = "Variable " + paras[0] + " does not exist";
                return false;
            }
        }

        private bool DateLong(List<string> paras, ConditionVariables vars, out string output, int recdepth)
        {
            if (vars.ContainsKey(paras[0]))
            {
                DateTime res;

                if (DateTime.TryParse(vars[paras[0]], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"),
                                        System.Globalization.DateTimeStyles.None, out res))
                {
                    if (paras[1].Equals("LongTime", StringComparison.InvariantCultureIgnoreCase))
                    {
                        output = res.ToLongTimeString();
                    }
                    else if (paras[1].Equals("ShortTime", StringComparison.InvariantCultureIgnoreCase))
                    {
                        output = res.ToShortTimeString();
                    }
                    else if (paras[1].Equals("LongDate", StringComparison.InvariantCultureIgnoreCase))
                    {
                        output = res.ToLongDateString();
                    }
                    else if (paras[1].Equals("LongDateTime", StringComparison.InvariantCultureIgnoreCase))
                    {
                        output = res.ToLongDateString() + " " + res.ToLongTimeString();
                    }
                    else if (paras[1].Equals("ShortDate", StringComparison.InvariantCultureIgnoreCase))
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
            }
            else
                output = "Variable " + paras[0] + " does not exist";

            return false;
        }
        private bool DateHour(List<string> paras, ConditionVariables vars, out string output, int recdepth)
        {
            if (vars.ContainsKey(paras[0]))
            {
                DateTime res;

                if (DateTime.TryParse(vars[paras[0]], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"),
                                        System.Globalization.DateTimeStyles.None, out res))
                {
                    output = res.Hour.ToString();
                    return true;
                }
                else
                    output = "Date is not in correct en-US format";
            }
            else
                output = "Variable " + paras[0] + " does not exist";
            return false;
        }

        private bool FindLine(List<string> paras, ConditionVariables vars, out string output, int recdepth)
        {
            if (vars.ContainsKey(paras[0]))
            {
                if (vars.ContainsKey(paras[1]))
                {
                    using (System.IO.TextReader sr = new System.IO.StringReader(vars[paras[0]]))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.Contains(vars[paras[1]]))
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
                    output = "The variable " + paras[1] + " does not exist";
            }
            else
                output = "The variable " + paras[0] + " does not exist";

            return false;
        }

        private bool SubString(List<string> paras, ConditionVariables vars, out string output, int recdepth)
        {
            if (vars.ContainsKey(paras[0]) )
            {
                int start, length;

                bool okstart = int.TryParse(paras[1], out start) || (vars.ContainsKey(paras[1]) && int.TryParse(vars[paras[1]], out start));
                bool oklength = int.TryParse(paras[2], out length) || (vars.ContainsKey(paras[2]) && int.TryParse(vars[paras[2]], out length));

                if ( okstart && oklength )
                {
                    string v = vars[paras[0]];

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
            }
            else
                output = "The variable " + paras[0] + " does not exist";

            return false;
        }

        private bool IndexOf(List<string> paras, ConditionVariables vars, out string output, int recdepth)
        {
            if (vars.ContainsKey(paras[0]))
            {
                if (vars.ContainsKey(paras[1]))
                {
                    output = vars[paras[0]].IndexOf(vars[paras[1]]).ToString();
                    return true;
                }
                else
                    output = "The variable " + paras[1] + " does not exist";
            }
            else
                output = "The variable " + paras[0] + " does not exist";

            return false;
        }

        private bool Lower(List<string> paras, ConditionVariables vars, out string output, int recdepth)
        {
            if (vars.ContainsKey(paras[0]))
            {
                output = vars[paras[0]].ToLower();
                return true;
            }
            else
                output = "The variable " + paras[0] + " does not exist";

            return false;
        }

        private bool Upper(List<string> paras, ConditionVariables vars, out string output, int recdepth)
        {
            if (vars.ContainsKey(paras[0]))
            {
                output = vars[paras[0]].ToUpper();
                return true;
            }
            else
                output = "The variable " + paras[0] + " does not exist";

            return false;
        }

        private bool Trim(List<string> paras, ConditionVariables vars, out string output, int recdepth)
        {
            if (vars.ContainsKey(paras[0]))
            {
                output = vars[paras[0]].Trim();
                return true;
            }
            else
                output = "The variable " + paras[0] + " does not exist";

            return false;
        }

        private bool Length(List<string> paras, ConditionVariables vars, out string output, int recdepth)
        {
            if (vars.ContainsKey(paras[0]))
            {
                output = vars[paras[0]].Length.ToString();
                return true;
            }
            else
                output = "The variable " + paras[0] + " does not exist";

            return false;
        }

        private bool Version(List<string> paras, ConditionVariables vars, out string output, int recdepth)
        {
            System.Reflection.Assembly aw = System.Reflection.Assembly.GetExecutingAssembly();
            string v = aw.FullName.Split(',')[1].Split('=')[1];
            string[] list = v.Split('.');

            int para;
            if (int.TryParse(paras[0], out para) && para >= 1 && para <= list.Length)
            {
                output = list[para - 1];
                return true;
            }
            else
            {
                output = "Parameter number must be between 1 and 4";
                return false;
            }
        }

        private bool Floor(List<string> paras, ConditionVariables vars, out string output, int recdepth)
        {
            if (vars.ContainsKey(paras[0]))
            {
                double para;
                if (double.TryParse(vars[paras[0]], out para))
                {
                    string fmt = vars.ContainsKey(paras[1]) ? vars[paras[1]] : paras[1];
                    if (FormatIt(Math.Floor(para), fmt, out output))
                        return true;
                }
                else
                    output = "Parameter number be a number";
            }
            else
                output = "The variable " + paras[0] + " does not exist";

            return false;
        }

        private bool Round(List<string> paras, ConditionVariables vars, out string output, int recdepth)
        {
            return RoundCommon(paras, vars, out output, recdepth, 0);
        }

        private bool Roundnz(List<string> paras, ConditionVariables vars, out string output, int recdepth)
        {
            int extradigits;

            if (int.TryParse(paras[3], out extradigits) || (vars.ContainsKey(paras[3]) && int.TryParse(vars[paras[3]], out extradigits)))
            {
                return RoundCommon(paras, vars, out output, recdepth, extradigits);
            }
            else
                output = "The variable " + paras[3] + " does not exist";

            return false;
        }

        private bool RoundCommon(List<string> paras, ConditionVariables vars, out string output, int recdepth, int extradigits)
        {
            if (vars.ContainsKey(paras[0]))
            {
                double para;

                if (double.TryParse(vars[paras[0]], out para))
                {
                    int digits = 0;
                    if (int.TryParse(paras[1], out digits) || (vars.ContainsKey(paras[1]) && int.TryParse(vars[paras[1]], out digits)))
                    {
                        string fmt = vars.ContainsKey(paras[2]) ? vars[paras[2]] : paras[2];

                        double res = Math.Round(para, digits);

                        if (extradigits>0 && Math.Abs(res) < 0.0000001)     // if rounded to zero..
                        {
                            digits += extradigits;
                            fmt += new string('#',extradigits);
                            res = Math.Round(para, digits);
                        }

                        if (FormatIt(res, fmt, out output))
                             return true;
                    }
                    else
                        output = "Digits must be a variable or an integer number of digits";
                }
                else
                    output = "Variable must be a integer or double";
            }
            else
                output = "The variable " + paras[0] + " does not exist";

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
                output += v.ToString(fmt);
                return true;
            }
            catch
            {
                output = "Format must be a c# ToString format for doubles";
                return false;
            }
        }

        #endregion
    }
}
