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
using System.Windows.Forms;
using BaseUtils;
using Conditions;

namespace ActionLanguage
{
    public class ActionIfElseBase : ActionBase
    {

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<string> eventvars) //standard one used for most
        {
            ConditionLists jf = new ConditionLists();
            jf.Read(userdata);
            bool ok = ConfigurationMenu(parent, cp, eventvars, ref jf);
            if (ok)
                userdata = jf.ToString();
            return ok;
        }

        public bool ConfigurationMenu(Form parent, ActionCoreController cp, List<string> eventvars, ref ConditionLists jf)
        {
            ConditionFilterForm frm = new ConditionFilterForm();
            frm.InitCondition("Define condition", cp.Icon, eventvars, jf);

            if (frm.ShowDialog(parent) == DialogResult.OK)
            {
                jf = frm.result;
                return true;
            }
            else
                return false;
        }

        public override string VerifyActionCorrect()
        {
            ConditionLists cl2 = new ConditionLists();
            string ret = cl2.Read(userdata);
            if (ret == null)
            {
                userdata = cl2.ToString();  // Normalize it!
            }
            return ret;
        }
    }

    public class ActionIf : ActionIfElseBase
    {
        ConditionLists condition;


        public override bool ExecuteAction(ActionProgramRun ap)
        {
            if (ap.IsExecuteOn)
            {
                if (condition == null)
                {
                    condition = new ConditionLists();
                    if (condition.Read(UserData) != null)
                    {
                        ap.ReportError("IF condition is not correctly formed");
                        return true;
                    }
                }

                string errlist;
                bool? condres = condition.CheckAll(ap.functions.vars, out errlist, null, ap.functions);     // may return null.. and will return errlist

                if (errlist == null)
                {
                    bool res = condres.HasValue && condres.Value;
                    ap.PushState(Type, res);       // true if has values and true, else false
                }
                else
                    ap.ReportError(errlist);
            }
            else
                ap.PushState(Type, ActionProgramRun.ExecState.OffForGood);

            return true;

        }
    }

    public class ActionElseIf : ActionIfElseBase
    {
        ConditionLists condition;

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            if (ap.IsExecutingType(ActionBase.ActionType.If))
            {
                if (ap.IsExecuteOff)       // if not executing, check condition
                {
                    if (condition == null)
                    {
                        condition = new ConditionLists();
                        if (condition.Read(UserData) != null)
                        {
                            ap.ReportError("ElseIf condition is not correctly formed");
                            return true;
                        }
                    }

                    string errlist;
                    bool? condres = condition.CheckAll(ap.functions.vars, out errlist, null, ap.functions);     // may return null.. and will return errlist

                    if (errlist == null)
                    {
                        bool res = condres.HasValue && condres.Value;
                        ap.ChangeState(res);            // either to ON.. or to OFF, continuing the off
                    }
                    else
                        ap.ReportError(errlist);
                }
                else
                {
                    ap.ChangeState(ActionProgramRun.ExecState.OffForGood);      // make sure off for good
                }
            }
            else
                ap.ReportError("ElseIf without IF");

            return true;
        }
    }

    public class ActionElse : ActionBase
    {
        public override bool ConfigurationMenuInUse { get { return false; } }
        public override string DisplayedUserData { get { return null; } }

        public override string VerifyActionCorrect()
        {
            return (UserData.Length == 0) ? null : " Text after else is not allowed";
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            if (ap.IsExecutingType(ActionBase.ActionType.If))
            {
                if (ap.IsExecuteOff)       // if not executing, turn on
                    ap.ChangeState(true); // go true
                else
                    ap.ChangeState(ActionProgramRun.ExecState.OffForGood);      // make sure off for good
            }
            else
                ap.ReportError("Else without IF");

            return true;
        }
    }

    public class ActionWhile : ActionIfElseBase
    {
        ConditionLists condition;

        public override bool ExecuteAction(ActionProgramRun ap)             // WHILE at top of loop
        {
            if (ap.IsExecuteOn)       // if executing
            {
                if (condition == null)
                {
                    condition = new ConditionLists();
                    if (condition.Read(UserData) != null)
                    {
                        ap.ReportError("While condition is not correctly formed");
                        return true;
                    }
                }

                string errlist;
                bool? condres = condition.CheckAll(ap.functions.vars, out errlist, null, ap.functions);     // may return null.. and will return errlist

                if (errlist == null)
                {
                    bool res = condres.HasValue && condres.Value;
                    ap.PushState(Type, res, res);   // set execute state, and push position if executing, returning us here when it drop out a level..
                }
                else
                    ap.ReportError(errlist);
            }
            else
                ap.PushState(Type, ActionProgramRun.ExecState.OffForGood);

            return true;
        }

        public bool ExecuteEndDo(ActionProgramRun ap)             // WHILE at end of DO..WHILE
        {
            if (ap.IsExecuteOn)                                     // if executing
            {
                if (condition == null)
                {
                    condition = new ConditionLists();
                    if (condition.Read(UserData) != null)
                    {
                        ap.ReportError("While condition in Do..While is not correctly formed");
                        return true;
                    }
                }

                string errlist;
                bool? condres = condition.CheckAll(ap.functions.vars, out errlist, null, ap.functions);     // may return null.. and will return errlist

                if (errlist == null)
                {
                    bool res = condres.HasValue && condres.Value;

                    if (res)
                    {
                        ap.Goto(ap.PushPos + 1);                        // back to DO+1, keep level
                        return true;                                    // Else drop the level, and finish the do.
                    }
                }
                else
                    ap.ReportError(errlist);
            }

            return false;                                           // not executing the DO, so we just let the standard code drop the level.  Position will not be pushed
        }
    }

    public class ActionDo : ActionBase
    {

        public override bool ConfigurationMenuInUse { get { return false; } }
        public override string DisplayedUserData { get { return null; } }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            if (ap.IsExecuteOn)       // if executing
            {
                ap.PushState(Type, true, true);   // set execute to On (it is on already) and push the position of the DO
            }
            else
            {
                ap.PushState(Type, ActionProgramRun.ExecState.OffForGood);  // push off for good, don't push position since we don't want to loop
            }

            return true;
        }
    }

    public class ActionLoop : ActionBase
    {
        private bool inloop = false;
        private int loopcount = 0;
        private string loopvar;

        public override bool AllowDirectEditingOfUserData { get { return true; } }    // and allow editing?

        List<string> FromString(string input)       // returns in raw esacped mode
        {
            StringParser sp = new StringParser(input);
            List<string> s = sp.NextQuotedWordList(replaceescape: true);
            return (s != null && s.Count >= 1 && s.Count <= 2) ? s : null;
        }

        public override string VerifyActionCorrect()
        {
            return (FromString(userdata) != null) ? null : "Loop command line not in correct format";
        }

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<string> eventvars)
        {
            List<string> l = FromString(userdata);
            List<string> r = ExtendedControls.PromptMultiLine.ShowDialog(parent, "Configure Loop", cp.Icon,
                            new string[] { "Loop count", "Optional var name" }, l?.ToArray(), true);

            if (r != null)
                userdata = r.ToStringCommaList(1, true);     // and escape them back

            return (r != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)     // LOOP when encountered
        {
            if (ap.IsExecuteOn)         // if executing
            {
                if (!inloop)            // if not in a loop
                {
                    List<string> ctrl = FromString(UserData);
                    List<string> exp;

                    if (ap.functions.ExpandStrings(ctrl, out exp) != ConditionFunctions.ExpandResult.Failed)
                    {
                        if (exp[0].InvariantParse(out loopcount))
                        {
                            inloop = true;
                            ap.PushState(Type, (loopcount > 0), true);   // set execute to On (if loop count is >0) and push the position of the LOOP
                            loopvar = (exp.Count >= 2 && exp[1].Length > 0) ? exp[1] : ("Loop" + ap.ExecLevel);     // pick name.. if not given, use backwards compat name
                            ap[loopvar] = "1";
                        }
                        else
                            ap.ReportError("Loop count must be an integer");
                    }
                    else
                        ap.ReportError(exp[0]);
                }
                else
                    ap.ReportError("Internal error - Loop is saying counting when run");
            }
            else
            {
                ap.PushState(Type, ActionProgramRun.ExecState.OffForGood, true);  // push off for good and save position so we know which loop we are executing
                inloop = true;    // we are in the loop properly.
                loopcount = 0;      // and with no count
            }

            return true;
        }

        public bool ExecuteEndLoop(ActionProgramRun ap)     // only called if Push pos is set.  break clears the push pos
        {
            if (inloop)                   // if in a count, we were executing at the loop, either on or off
            {
                if (--loopcount > 0)        // any count left?
                {
                    ap.Goto(ap.PushPos + 1);                    // back to LOOP+1, keep level

                    int c = 0;
                    if (ap[loopvar].InvariantParse(out c)) // update LOOP level variable.. don't if they have mucked it up
                        ap[loopvar] = (c + 1).ToString(System.Globalization.CultureInfo.InvariantCulture);

                    return true;
                }
                else
                {
                    inloop = false;                           // turn off check flag, and we just exit the loop normally
                }
            }
            else
                ap.ReportError("Internal error - END Loop is saying not counting when run");

            return false;                                           // not executing the LOOP, so we just let the standard code drop the level.  Position will not be pushed
        }
    }

    public class ActionErrorIf : ActionIfElseBase
    {
        ConditionLists condition;
        string errmsg;


        public bool FromString(string s, out ConditionLists cond, out string errmsg)    // errmsg has real control chars
        {
            cond = new ConditionLists();

            StringParser p = new StringParser(s);
            errmsg = p.NextQuotedWord(" ,", replaceescape:true);

            if (errmsg != null && p.IsCharMoveOn(','))
            {
                string condstring = p.LineLeft;

                if (cond.Read(condstring) == null)
                    return true;
            }

            errmsg = "";
            return false;
        }

        public string ToString(ConditionLists cond, string errmsg)
        {
            return errmsg.EscapeControlChars().QuoteString(comma: true) + ", " + cond.ToString();       // this will return with escaped chars
        }

        public override string VerifyActionCorrect()
        {
            ConditionLists cond;
            string errmsg;
            return FromString(userdata, out cond, out errmsg) ? null : "ErrorIf not in correct format: \"Error string\", condition";
        }

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<string> eventvars)
        {
            ConditionLists cond;
            string errmsg;
            FromString(userdata, out cond, out errmsg);

            if (base.ConfigurationMenu(parent, cp, eventvars, ref cond))
            {
                string promptValue = ExtendedControls.PromptSingleLine.ShowDialog(parent, "Error to display", errmsg, "Configure ErrorIf Command", cp.Icon);
                if (promptValue != null)
                {
                    userdata = ToString(cond, promptValue);
                    return true;
                }
            }

            return false;
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            if (condition == null)
            {
                if (!FromString(userdata, out condition, out errmsg))
                {
                    ap.ReportError("ErrorIF condition is not correctly formed");
                    return true;
                }
            }

            string errlist;
            bool? condres = condition.CheckAll(ap.functions.vars, out errlist, null, ap.functions);     // may return null.. and will return errlist

            if (errlist == null)
            {
                bool res = condres.HasValue && condres.Value;

                if (res)
                {
                    if (ap.functions.ExpandString(errmsg, out string exprerr) != ConditionFunctions.ExpandResult.Failed)
                    {
                        ap.ReportError(exprerr);
                    }
                    else
                        ap.ReportError(exprerr);
                }
            }
            else
                ap.ReportError(errlist);

            return true;
        }
    }

    public class ActionCall : ActionBase
    {
        public bool FromString(string s, out string progname, out ConditionVariables vars, out Dictionary<string, string> altops)
        {
            StringParser p = new StringParser(s);
            vars = new ConditionVariables();
            altops = new Dictionary<string, string>();

            progname = p.NextWord("( ");        // stop at space or (

            if (progname != null)
            {
                if (p.IsCharMoveOn('('))       // if (, then
                {
                    if (vars.FromString(p, ConditionVariables.FromMode.MultiEntryCommaBracketEnds, altops) && p.IsCharMoveOn(')') && p.IsEOL)      // if para list decodes and we finish on a ) and its EOL
                        return true;
                }
                else if (p.IsEOL)   // if EOL, its okay, prog name only
                    return true;
            }

            return false;
        }

        public string ToString(string progname, ConditionVariables cond, Dictionary<string, string> altops)
        {
            if (cond.Count > 0)
                return progname + "(" + cond.ToString(altops, bracket: true) + ")";
            else
                return progname;
        }

        public string GetProgramName()
        {
            string progname;
            ConditionVariables vars;
            Dictionary<string, string> altops;
            return FromString(userdata, out progname, out vars, out altops) ? progname : null;
        }

        public override string VerifyActionCorrect()
        {
            string progname;
            ConditionVariables vars;
            Dictionary<string, string> altops;
            return FromString(userdata, out progname, out vars, out altops) ? null : "Call not in correct format: progname (var list v=\"y\")";
        }

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<string> eventvars)
        {
            string progname;
            ConditionVariables cond;
            Dictionary<string, string> altops;
            FromString(UserData, out progname, out cond, out altops);

            string promptValue = ExtendedControls.PromptSingleLine.ShowDialog(parent, "Program to call (use set::prog if req)", progname, "Configure Call Command", cp.Icon);
            if (promptValue != null)
            {
                ConditionVariablesForm avf = new ConditionVariablesForm();
                avf.Init("Variables to pass into called program", cp.Icon, cond, showone: true, allownoexpand: true, altops: altops);

                if (avf.ShowDialog(parent) == DialogResult.OK)
                {
                    userdata = ToString(promptValue, avf.result, avf.result_altops);
                    return true;
                }
            }

            return false;
        }

        //special call for execute, needs to pass back more data
        public bool ExecuteCallAction(ActionProgramRun ap, out string progname, out ConditionVariables vars)
        {
            Dictionary<string, string> altops;
            if (FromString(UserData, out progname, out vars, out altops) && progname.Length > 0)
            {
                List<string> wildcards = new List<string>();
                ConditionVariables newitems = new ConditionVariables();

                foreach (string key in vars.NameEnumuerable)
                {
                    int asterisk = key.IndexOf('*');
                    if (asterisk >= 0)                                    // SEE if any wildcards, if so, add to newitems
                    {
                        bool noexpand = altops[key].Contains("$");            // wildcard operator determines expansion state

                        wildcards.Add(key);
                        string prefix = key.Substring(0, asterisk);

                        foreach (string jkey in ap.variables.NameEnumuerable)
                        {
                            if (jkey.StartsWith(prefix))
                            {
                                if (noexpand)
                                    newitems[jkey] = ap[jkey];
                                else
                                {
                                    string res;
                                    if (ap.functions.ExpandString(ap[jkey], out res) == ConditionFunctions.ExpandResult.Failed)
                                    {
                                        ap.ReportError(res);
                                        return false;
                                    }

                                    newitems[jkey] = res;
                                }
                            }
                        }
                    }
                }

                foreach (string w in wildcards)     // remove wildcards
                    vars.Delete(w);

                //foreach ( stKeyValuePair<string,string> k in vars.values)          // for the rest, before we add in wildcards, expand
                foreach (string k in vars.NameEnumuerable.ToList())                            // for the rest, before we add in wildcards, expand. Note ToList
                {
                    bool noexpand = altops[k].Contains("$");            // when required

                    if (!noexpand)
                    {
                        string res;
                        if (ap.functions.ExpandString(vars[k], out res) == ConditionFunctions.ExpandResult.Failed)
                        {
                            ap.ReportError(res);
                            return false;
                        }

                        vars[k] = res;
                    }
                }

                vars.Add(newitems);         // finally assemble the variables

                return true;
            }
            else
            {
                ap.ReportError("Call not configured");
                return false;
            }
        }
    }

    public class ActionBreak : ActionBase
    {
        public override bool ConfigurationMenuInUse { get { return false; } }
        public override string DisplayedUserData { get { return null; } }        // null if you dont' want to display

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            ap.Break();
            return true;
        }
    }

    public class ActionForEach : ActionBase
    {
        static public bool FromString(string s, out string macroname, out string searchterm)
        {
            StringParser sp = new StringParser(s);
            macroname = sp.NextQuotedWord();
            searchterm = null;

            return (macroname != null && sp.IsStringMoveOn("in") && (searchterm = sp.NextQuotedWord()) != null);
        }

        static public string ToString(string macro, string searchterm)
        {
            return macro.QuoteString() + " in " + searchterm.QuoteString();
        }

        public override string VerifyActionCorrect()
        {
            string mn, st;
            return FromString(UserData, out mn, out st) ? null : "ForEach command line not in correct format";
        }

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<string> eventvars)
        {
            string mn = "", st = "";
            FromString(UserData, out mn, out st);

            List<string> promptValue = ExtendedControls.PromptMultiLine.ShowDialog(parent, "ForEach:", cp.Icon,
                             new string[] { "Var Name", "Search" },
                             new string[] { mn, st },
                             false,
                             new string[] { "Enter the variable to be assigned with the variable name", "Enter the front search pattern" });

            if (promptValue != null)
                userdata = ToString(promptValue[0], promptValue[1]);

            return (promptValue != null);
        }

        int count;
        string expmacroname;
        List<string> values;

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            if (ap.IsExecuteOn)
            {
                string macroname, searchterm;

                if (FromString(UserData, out macroname, out searchterm))
                {
                    if (ap.functions.ExpandString(macroname, out expmacroname) == Conditions.ConditionFunctions.ExpandResult.Failed)       //Expand out.. and if no errors
                    {
                        ap.ReportError(expmacroname);
                        return true;
                    }

                    string expsearchterm;

                    if (ap.functions.ExpandString(searchterm, out expsearchterm) == Conditions.ConditionFunctions.ExpandResult.Failed)       //Expand out.. and if no errors
                    {
                        ap.ReportError(expmacroname);
                        return true;
                    }

                    values = new List<String>();

                    expsearchterm = expsearchterm.RegExWildCardToRegular();

                    foreach (string key in ap.variables.NameEnumuerable)
                    {
                        if ( System.Text.RegularExpressions.Regex.IsMatch(key,expsearchterm))
                            values.Add(key);
                    }

                    ap.PushState(Type, values.Count>0, true);   // set execute to On and push the position of the ForEach
                    count = 0;
                    if (values.Count > 0)
                    {
                        ap[expmacroname] = values[count++];
                        System.Diagnostics.Debug.WriteLine("First value " + ap[expmacroname]);
                    }
                }
            }
            else
            {
                ap.PushState(Type, ActionProgramRun.ExecState.OffForGood, true);
                values = null;
            }

            return true;
        }

        public bool ExecuteEndFor(ActionProgramRun ap)   // only called if Push pos is set.  break clears the push pos
        {
            if ( values != null )
            {
                if (count < values.Count)
                {
                    ap.Goto(ap.PushPos + 1);
                    ap[expmacroname] = values[count++];
                    System.Diagnostics.Debug.WriteLine("New value " + ap[expmacroname]);

                    return true;
                }
                else
                    values = null;
            }

            return false;
        }
    }
}

