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

namespace EDDiscovery
{
    public class ConditionFunctions
    {
        static System.Globalization.CultureInfo ct = System.Globalization.CultureInfo.InvariantCulture;

        class Parameter
        {
            public string value;
            public bool isstring;
        };

        delegate bool func(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth);

        class FuncEntry
        {
            public func fn;
            public int numberparasmin;
            public int numberparasmax;
            public int checkvarmap;           // if not a string, check macro
            public int allowstringmap;          // allow a string

            public FuncEntry(func f, int min, int max, int checkmacromapx, int allowstringmapx = 0 )
            {
                fn = f; numberparasmin = min; numberparasmax = max;
                checkvarmap = checkmacromapx; allowstringmap = allowstringmapx;
            }
        }

        Dictionary<string, FuncEntry> functions;

        public ConditionFunctions()
        {
            functions = new Dictionary<string, FuncEntry>();

            // first is a bitmap saying if to check for the value is a var
            // second is a bitmap saying if a string is allowed in this pos
            functions.Add("exist",        new FuncEntry(Exists,             1, 20, 0, 0));
            functions.Add("expand",       new FuncEntry(Expand,             1,20,   0xfffffff,0xfffffff)); // check var, can be string (if so expanded)
            functions.Add("indirect",     new FuncEntry(Indirect,           1,20,   0xfffffff,0xfffffff));   // check var, no strings
            functions.Add("splitcaps",    new FuncEntry(SplitCaps,          1,1,    1,1));   //check var, allow strings
            functions.Add("sc",           new FuncEntry(SplitCaps,          1,1,    1,1));   //shorter alias for above
            functions.Add("ship",         new FuncEntry(Ship,               1,1,    1,1));   //ship translator
            functions.Add("datehour",     new FuncEntry(DateHour,           1,1,    1));     // first is a var, no strings
            functions.Add("date",         new FuncEntry(DateCnv,            2,2,    1));     // first is a var, second is not, no strings
            functions.Add("findline",     new FuncEntry(FindLine,           2,2,    3,2));   //check var1 and var2, second can be a string
            functions.Add("substring",    new FuncEntry(SubString,          3,3,    1,1));   // check var1, var1 can be string, var 2 and 3 can either be macro or ints not strings
            functions.Add("indexof",      new FuncEntry(IndexOf,            2,2,    3,3));   // check var1 and 2 if normal, allow string in 1 and 2
            functions.Add("lower",        new FuncEntry(Lower,              1,20,   0xfffffff,0xfffffff));   // all can be string, check var
            functions.Add("upper",        new FuncEntry(Upper,              1,20,   0xfffffff,0xfffffff));   // all can be string, check var
            functions.Add("join",         new FuncEntry(Join,               3,20,   0xfffffff,0xfffffff));   // all can be string, check var
            functions.Add("trim",         new FuncEntry(Trim,               1,1,    1,1));
            functions.Add("length",       new FuncEntry(Length,             1,1,    1,1));
            functions.Add("version",      new FuncEntry(Version,            1,1,    0));     // don't check first para
            functions.Add("floor",        new FuncEntry(Floor,              2,2,    1));     // check var1, not var 2 no strings
            functions.Add("roundnz",      new FuncEntry(RoundCommon,        4,4,    1));
            functions.Add("roundscale",   new FuncEntry(RoundCommon,        5,5,    1));
            functions.Add("round",        new FuncEntry(RoundCommon,        3,3,    1));
            functions.Add("ifnotempty",   new FuncEntry(Ifnotempty,         2,3,    7,7));   // check var1-3, allow strings var1-3
            functions.Add("ifempty",      new FuncEntry(Ifempty,            2,3,    7,7));
            functions.Add("iftrue",       new FuncEntry(Iftrue,             2,3,    7,7));   // check var1-3, allow strings var1-3
            functions.Add("iffalse",      new FuncEntry(Iffalse,            2,3,    7,7));
            functions.Add("ifzero",       new FuncEntry(Ifzero,             2,3,    7,7));   // check var1-3, allow strings var1-3
            functions.Add("ifnonzero",    new FuncEntry(Ifnonzero,          2,3,    7,7));
            functions.Add("ifcontains",   new FuncEntry(Ifcontains,         3,4,    15,15)); // check var1-4, allow strings var1-4
            functions.Add("ifnotcontains",new FuncEntry(Ifnotcontains,      3,4,    15,15));
            functions.Add("ifequal",      new FuncEntry(Ifequal,            3,4,    15,15));
            functions.Add("ifnotequal",   new FuncEntry(Ifnotequal,         3,4,    15,15));
            functions.Add("expandarray",  new FuncEntry(ExpandArray,        4,5,    2,2+16));  // var 1 is text root, not var, not string, var 2 can be var or string, var 3/4 is integers or variables, checked in function
            functions.Add("fileexists",   new FuncEntry(FileExists,         1,20,   0xfffffff,0xfffffff));   // check var, can be string
            functions.Add("escapechar",   new FuncEntry(EscapeChar,         1,1,    1,1));   // check var, can be string
            functions.Add("replaceescapechar",new FuncEntry(ReplaceEscapeChar,  1,1,    1,1));   // check var, can be string
            functions.Add("random",       new FuncEntry(Random,             1,1,    0,0));   // no change var, not string
            functions.Add("eval",         new FuncEntry(Eval,               1,2,    1,1));   // can be string, can be variable, p2 is not a variable, and can't be a string
            functions.Add("existsdefault",new FuncEntry(ExistsDefault,      2,2,    2,2));   // first is a macro but can not exist, second is a string or macro which must exist
            functions.Add("wordof",       new FuncEntry(WordOf,             2,3,    1+4,1+4));   // first is a macro or string, second is a var or literal, third is a macro or string
            functions.Add("expandvars",   new FuncEntry(ExpandVars,         4,5,    2,2+16));   // var 1 is text root, not var, not string, var 2 can be var or string, var 3/4 is integers or variables, checked in function
        }

#region expander

        // true, expanded, result = string
        // false, failed, result = error

        public ConditionLists.ExpandResult ExpandStrings(List<string> inv , out List<string> outv, ConditionVariables vars)
        {
            outv = new List<string>();

            foreach( string s in inv )
            {
                string r;
                if ( ExpandString(s,vars, out r) == ConditionLists.ExpandResult.Failed )
                {
                    outv = new List<string>() { r };
                    return ConditionLists.ExpandResult.Failed;
                }

                outv.Add(r);
            }

            return ConditionLists.ExpandResult.Expansion;
        }

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
            string fnl = fname.ToLower();
            if (functions.ContainsKey(fnl))
            {
                FuncEntry fe = functions[fnl];

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

        private bool ExistsDefault(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            if (vars.ContainsKey(paras[0].value))
                output = vars[paras[0].value];
            else
                output = paras[1].isstring ? paras[1].value : vars[paras[1].value];

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
                string value = (p.isstring) ? p.value : vars[p.value];          // if string, its the value, else look up vars (must exist i've checked)

                if (indirect)
                {
                    if (vars.ContainsKey(value))
                        value = vars[value];
                    else
                    {
                        output = "Indrect Variable " + value + " not found";
                        return false;
                    }
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
            string delim = (paras.Count > 1) ? ((paras[1].isstring) ? paras[1].value : vars[paras[1].value]) : "";

            for (int i = 2; i < paras.Count; i++)
                value += delim + ((paras[i].isstring) ? paras[i].value : vars[paras[i].value]);

            output = value.ToLower();
            return true;
        }

        private bool Upper(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            string value = (paras[0].isstring) ? paras[0].value : vars[paras[0].value];
            string delim = (paras.Count > 1) ? ((paras[1].isstring) ? paras[1].value : vars[paras[1].value]) : "";

            for (int i = 2; i < paras.Count; i++)
                value += delim + ((paras[i].isstring) ? paras[i].value : vars[paras[i].value]);

            output = value.ToUpper();
            return true;
        }

        private bool Join(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            string delim = (paras[0].isstring) ? paras[0].value : vars[paras[0].value];
            string value = (paras[1].isstring) ? paras[1].value : vars[paras[1].value];

            for (int i = 2; i < paras.Count; i++)
                value += delim + ((paras[i].isstring) ? paras[i].value : vars[paras[i].value]);

            output = value;
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
            return ExpandArrayCommon(paras, vars, out output, recdepth, false);
        }

        private bool ExpandVars(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            return ExpandArrayCommon(paras, vars, out output, recdepth, true);
        }

        private bool ExpandArrayCommon(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth, bool join)
        {
            string arrayroot = paras[0].value;
            string separ = (paras[1].isstring) ? paras[1].value : vars[paras[1].value];

            int start, length;
            bool okstart = paras[2].value.InvariantParse(out start) || (vars.ContainsKey(paras[2].value) && vars[paras[2].value].InvariantParse(out start));
            bool oklength = paras[3].value.InvariantParse(out length) || (vars.ContainsKey(paras[3].value) && vars[paras[3].value].InvariantParse(out length));

            if (okstart && oklength)
            {
                bool splitcaps = paras.Count == 5 && paras[4].value.IndexOf("splitcaps", StringComparison.InvariantCultureIgnoreCase) >=0;

                output = "";

                if (join)
                {
                    bool nameonly = paras.Count == 5 && paras[4].value.IndexOf("nameonly", StringComparison.InvariantCultureIgnoreCase) >= 0;
                    bool valueonly = paras.Count == 5 && paras[4].value.IndexOf("valueonly", StringComparison.InvariantCultureIgnoreCase) >= 0;

                    int index = 0;
                    foreach( string key in vars.values.Keys )
                    {
                        if ( key.StartsWith(arrayroot))
                        {
                            index++;
                            if ( index >= start && index < start+length )
                            {
                                string value = vars[key];

                                string entry = (valueonly) ? vars[key] : ( key.Substring(arrayroot.Length) + (nameonly ? "" : (" = "+ vars[key])));

                                if (output.Length > 0)
                                    output += separ;

                                output += (splitcaps) ? entry.SplitCapsWordUnderscoreTitleCase() : entry;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = start; i < start + length; i++)
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
                }

                return true;
            }
            else
                output = "Start and/or length are not integers or variables do not exist";

            return false;
        }

        private bool FileExists(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            foreach (Parameter p in paras)
            {
                string s = p.isstring ? p.value : vars[p.value];

                if (!System.IO.File.Exists(s))
                {
                    output = "0";
                    return true;
                }
            }

            output = "1";
            return true;
        }

        private bool EscapeChar(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            string s = paras[0].isstring ? paras[0].value : vars[paras[0].value];
            output = s.EscapeControlChars();
            return true;
        }

        private bool ReplaceEscapeChar(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            string s = paras[0].isstring ? paras[0].value : vars[paras[0].value];
            output = s.ReplaceEscapeControlChars();
            return true;
        }

        static Random rnd = new System.Random();

        private bool Random(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            int v;
            if ( paras[0].value.InvariantParse(out v ) || (vars.ContainsKey(paras[0].value) && vars[paras[0].value].InvariantParse(out v)) )
            {
                output = rnd.Next(v).ToString(ct);
                return true;
            }
            else
            {
                output = "Parameter should be an integer constant or a variable name with an integer in its value";
                return false;
            }
        }

        private bool Eval(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            string s = paras[0].isstring ? paras[0].value : vars[paras[0].value];

            bool tryit = paras.Count > 1 && paras[1].value.Equals("Try", StringComparison.InvariantCultureIgnoreCase);

            bool evalstate = s.Eval(out output);      // true okay, with output, false bad, with error

            if (tryit && !evalstate)                   // if try and failed.. NAN without error
            {
                output = "NAN";
                return true;
            }

            return evalstate;                       // else return error and output
        }

        private bool WordOf(List<Parameter> paras, ConditionVariables vars, out string output, int recdepth)
        {
            string s = paras[0].isstring ? paras[0].value : vars[paras[0].value];
            string c = vars.ContainsKey(paras[1].value) ? vars[paras[1].value] : paras[1].value;
            string splitter = (paras.Count >= 3) ? (paras[2].isstring ? paras[2].value : vars[paras[2].value]) : ";";
            char splitchar = (splitter.Length > 0) ? splitter[0] : ';';

            int count;
            if (c.InvariantParse(out count))
            {
                string[] split = s.Split(splitchar);
                count = Math.Max(1, Math.Min(count, split.Length));  // between 1 and split length
                output = split[count - 1];
                return true;
            }
            else
            {
                output = "Parameter should be an integer constant or a variable name with an integer in its value";
                return false;
            }
        }

#endregion
    }
}