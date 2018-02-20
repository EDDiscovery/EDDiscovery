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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conditions
{
    public class ConditionFunctions
    {
        public ConditionVariables vars;
        public ConditionPersistentData persistentdata;

        public delegate ConditionFunctionHandlers delegateGetCFH(ConditionFunctions c, ConditionVariables vars, ConditionPersistentData handles, int recdepth);
        public static delegateGetCFH GetCFH;            // SET this to override and add on more functions

        public ConditionFunctions(ConditionVariables v, ConditionPersistentData f)
        {
            vars = v;
            persistentdata = f;

            if (GetCFH == null)                     // Make sure we at least have some functions.. the base ones
                GetCFH = DefaultGetCFH;
        }

        #region expander

        public enum ExpandResult { Failed, NoExpansion, Expansion };

        public ExpandResult ExpandStrings(List<string> inv, out List<string> outv)
        {
            outv = new List<string>();

            foreach (string s in inv)
            {
                string r;
                if (ExpandString(s, out r) == ExpandResult.Failed)
                {
                    outv = new List<string>() { r };
                    return ExpandResult.Failed;
                }

                outv.Add(r);
            }

            return ExpandResult.Expansion;
        }

        public ExpandResult ExpandString(string line, out string result)
        {
            return ExpandStringFull(line, out result, 1);
        }

        public ExpandResult ExpandStringFull(string line, out string result, int recdepth)
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

                    while (apos < line.Length && char.IsLetter(line[apos]))
                        apos++;

                    if (apos < line.Length && line[apos] == '(')     // now must be bracket..  if not, its not in form, ignore %, or its past the EOL
                    {
                        string funcname = line.Substring(pos, apos - pos);
                        apos++;     // past the (

                        ConditionFunctionHandlers cfh = GetCFH(this, vars, persistentdata, recdepth);

                        while (true)
                        {
                            while (apos < line.Length && char.IsWhiteSpace(line[apos])) // remove white space
                                apos++;

                            if (apos < line.Length && line[apos] == ')' && cfh.paras.Count == 0)        // ) here must be on first only, and is valid
                            {
                                apos++; // skip by
                                break;
                            }

                            int start = apos;

                            if (apos < line.Length && (line[apos] == '"' || line[apos] == '\''))
                            {
                                char quote = line[apos++];

                                string res = "";

                                while (apos < line.Length && line[apos] != quote)
                                {
                                    if (line[apos] == '\\' && (apos + 1) < line.Length && line[apos + 1] == quote)  // if \"
                                    {
                                        apos++;
                                    }

                                    res += line[apos++];
                                }

                                if (apos >= line.Length)
                                {
                                    result = "Terminal quote missing at '" + line.Substring(startexpression, apos - startexpression) + "'";
                                    return ExpandResult.Failed;
                                }

                                apos++;     // remove quote

                                string resexp;          // expand out any strings.. recursion
                                ExpandResult sexpresult = ExpandStringFull(res, out resexp, recdepth + 1);

                                if (sexpresult == ExpandResult.Failed)
                                {
                                    result = resexp;
                                    return sexpresult;
                                }

                                cfh.paras.Add(new ConditionFunctionHandlers.Parameter() { value = resexp, isstring = true });
                            }
                            else
                            {
                                if (funcname.Length > 0)        // functions can have () embedded .. in literals
                                {
                                    int blevel = 0;
                                    while (apos < line.Length && (blevel > 0 || "), ".IndexOf(line[apos]) == -1))
                                    {
                                        if (line[apos] == '(')
                                            blevel++;
                                        else if (line[apos] == ')')
                                            blevel--;

                                        apos++;
                                    }
                                }
                                else
                                {
                                    while (apos < line.Length && "), ".IndexOf(line[apos]) == -1)
                                        apos++;
                                }

                                if (apos == start)
                                {
                                    result = "Missing variable name at '" + line.Substring(startexpression, apos - startexpression) + "'";
                                    return ExpandResult.Failed;
                                }

                                string res = line.Substring(start, apos - start);

                                if (funcname.Length > 0 && line.Contains("%"))        // function paramters can be expanded if they have a %
                                {
                                    string resexp;          // expand out any strings.. recursion
                                    ExpandResult sexpresult = ExpandStringFull(res, out resexp, recdepth + 1);

                                    if (sexpresult == ExpandResult.Failed)
                                    {
                                        result = resexp;
                                        return sexpresult;
                                    }

                                    res = resexp;
                                }
                                else
                                {                   // not an expansion.. see if it needs mangling
                                    res = vars.Qualify(res);
                                }

                                cfh.paras.Add(new ConditionFunctionHandlers.Parameter() { value = res, isstring = false });
                               
                            }

                            while (apos < line.Length && char.IsWhiteSpace(line[apos]))
                                apos++;

                            char c = (apos < line.Length) ? line[apos++] : '-';

                            if (c == ')')     // must be )
                                break;

                            if (c != ',')     // must be ,
                            {
                                result = "Incorrectly formed parameter list at '" + line.Substring(startexpression, apos - startexpression) + "'";
                                return ExpandResult.Failed;
                            }
                        }

                        string expand = null;

                        if (funcname.Length > 0)        // functions!
                        {
                            if (!cfh.RunFunction(funcname, out expand))
                            {
                                result = "Function " + funcname + ": " + expand;
                                return ExpandResult.Failed;
                            }
                        }
                        else if (cfh.paras.Count != 1)   // only 1
                        {
                            result = "Variable name missing between () at '" + line.Substring(startexpression, apos - startexpression) + "'";
                            return ExpandResult.Failed;
                        }
                        else
                        {                               // variables
                            if (cfh.paras[0].isstring)
                            {
                                result = "Must be a variable not a string for non function expansions";
                                return ExpandResult.Failed;
                            }
                            else if (vars.Exists(cfh.paras[0].value))
                                expand = vars[cfh.paras[0].value];
                            else
                            {
                                result = "Variable " + cfh.paras[0].value + " does not exist";
                                return ExpandResult.Failed;
                            }
                        }

                        noexpansion++;
                        line = line.Substring(0, pos - 1) + expand + line.Substring(apos);

                        pos = (pos - 1) + expand.Length;

                        //                            System.Diagnostics.Debug.WriteLine("<" + funcname + "> var <" + varnames[0] + ">" + "  line <" + line + "> left <" + line.Substring(pos) + ">");
                    }
                }
            } while (pos != -1);

            result = line;
            return (noexpansion > 0) ? ExpandResult.Expansion : ExpandResult.NoExpansion;
        }

        // backstop standard functions
        static public ConditionFunctionHandlers DefaultGetCFH(ConditionFunctions c, ConditionVariables vars, ConditionPersistentData handles, int recdepth)
        {
            return new ConditionFunctionsBase(c, vars, handles, recdepth);
        }

        #endregion
    }

    // Class holding parameters and can call functions.  inherit from this, and override find function to add on functions
    // done this way for historical reasons instead of having a set of ptrs to classes handling functions.

}