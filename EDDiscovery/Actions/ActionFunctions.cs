using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Actions
{
    public class ActionFunctions
    {
        EDDiscoveryForm ed;
        HistoryList hl;
        HistoryEntry he;
        FuncEntry[] flist;

        delegate bool func(List<string> paras, ConditionVariables vars, out string output);

        class FuncEntry
        {
            public string name;
            public func fn;
            public int numberparasmin;
            public int numberparasmax;

            public FuncEntry(string s, func f, int min, int max) { name = s; fn = f; numberparasmin = min; numberparasmax = max; }
        }

        public ActionFunctions(EDDiscoveryForm e, HistoryList l , HistoryEntry h)
        {
            ed = e; hl = l; he = h;

            flist = new FuncEntry[]
            {
                new FuncEntry("exists",Exists,1,20),
                new FuncEntry("splitcaps",SplitCaps,1,1)
            };
        }

        // true, expanded, result = string
        // false, failed, result = error

        public ConditionLists.ExpandResult ExpandString(string line, ConditionVariables vars, out string result)
        {
            int noexpansion = 0;
            int pos = 0;
            do
            {
                pos = line.IndexOf('%', pos);

                if (pos >= 0)
                {
                    pos++;                                                  // move on, if it fails, next pos= will be past this point

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

                                while (apos < line.Length && (char.IsLetterOrDigit(line[apos]) || line[apos] == '_' ))
                                    apos++;

                                if (apos == start)
                                {
                                    result = "Missing variable name";
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
                                    result = "Incorrectly formed parameter list";
                                    return ConditionLists.ExpandResult.Failed;
                                }
                            }

                            string expand = null;

                            if (funcname.Length > 0)
                            {
                                if (!RunFunction(funcname, varnames, vars, out expand))
                                {
                                    result = expand;
                                    return ConditionLists.ExpandResult.Failed;
                                }
                            }
                            else if (varnames.Count > 1)
                            {
                                result = "Only functions can have multiple comma separated items";
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
        public bool RunFunction(string fname, List<string> paras, ConditionVariables vars, out string output)
        {
            FuncEntry fe = Array.Find(flist, x => x.name.Equals(fname, StringComparison.InvariantCulture));
            if (fe != null)
            {
                if (paras.Count < fe.numberparasmin)
                    output = "Function " + fname + " has too few parameters";
                else if (paras.Count > fe.numberparasmax)
                    output = "Function " + fname + " has too many parameters";
                else
                {
                    return fe.fn(paras, vars, out output);
                }
            }
            else
                output = "Function " + fname + " does not exist";

            return false;
        }

        #region Functions

        private bool Exists(List<string> paras, ConditionVariables vars, out string output)
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

        private bool SplitCaps(List<string> paras, ConditionVariables vars, out string output)
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

        #endregion
    }
}
