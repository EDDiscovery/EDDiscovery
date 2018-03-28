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

        public ConditionVariables ExpandVars(ConditionVariables vars, out string errlist)       // expand all variables to new list
        {
            errlist = null;

            ConditionVariables exp = new ConditionVariables();

            foreach (string k in vars.NameEnumuerable)
            {
                if (ExpandString(vars[k], out errlist) == ConditionFunctions.ExpandResult.Failed)
                    return null;

                exp[k] = errlist;
            }

            errlist = null;
            return exp;
        }


        public ExpandResult ExpandString(string line, out string result)
        {
            return ExpandStringFull(line, out result, 1);
        }

        public ExpandResult ExpandStringFull(string line, out string result, int recdepth)
        {
            if (recdepth > 9)
            {
                result = "Recursion detected - aborting expansion";
                return ExpandResult.Failed;
            }

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

                        string errprefix = "";
                        if (funcname.Length > 0)
                        {
                            if (!cfh.SetFunction(funcname))
                            {
                                result = "Function '" + funcname + "' does not exist";
                                return ExpandResult.Failed;
                            }

                            errprefix = "Function " + funcname + ": ";
                            //System.Diagnostics.Debug.WriteLine("Function " + funcname);
                        }


                        while (true)
                        {
                            while (apos < line.Length && char.IsWhiteSpace(line[apos])) // remove white space
                                apos++;

                            if (apos < line.Length && line[apos] == ')' && cfh.ParaCount == 0)        // ) here must be on first only, and is valid
                            {
                                apos++; // skip by
                                break;
                            }

                            if (!cfh.IsNextParameterAllowed)
                            {
                                result = errprefix + "Too many parameters";
                                return ExpandResult.Failed;
                            }
                            int start = apos;

                            bool isstring = false;       // meaning, we can't consider it for macro names, its now a literal
                            string res = null;

                            if (apos < line.Length && (line[apos] == '"' || line[apos] == '\''))
                            {
                                if (!cfh.IsNextStringAllowed)
                                {
                                    result = errprefix + "String not allowed in parameter " + (cfh.ParaCount + 1);
                                    return ExpandResult.Failed;
                                }

                                char quote = line[apos++];

                                res = string.Empty;

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
                                    result = errprefix + "Terminal quote missing at '" + line.Substring(startexpression, apos - startexpression) + "'";
                                    return ExpandResult.Failed;
                                }

                                apos++;     // remove quote
                                isstring = true;     
                            }
                            else
                            {
                                if (cfh.IsFunction)        // functions can have () embedded .. in literals
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
                                    result = errprefix + "Missing text/varname at '" + line.Substring(startexpression, apos - startexpression) + "'";
                                    return ExpandResult.Failed;
                                }

                                res = line.Substring(start, apos - start);
                            }

                            string err = cfh.ProcessParameter(res, isstring , recdepth);

                            if (err != null)
                            {
                                result = errprefix + "Parameter " + (cfh.ParaCount + 1) + ": " + err;
                                return ExpandResult.Failed;
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

                        if ( !cfh.Run(out expand ))
                        {
                            result = errprefix + expand;
                            return ExpandResult.Failed;
                        }

                        //System.Diagnostics.Debug.WriteLine("Output is '" + expand + "'");

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

}