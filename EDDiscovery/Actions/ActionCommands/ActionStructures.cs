using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    public class ActionIfElseBase : Action
    {
        public ActionIfElseBase(string n, ActionType t, string ud, int lu) : base(n, t, ud,lu)
        {
        }

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars) //standard one used for most
        {
            ConditionLists jf = new ConditionLists();
            jf.FromString(userdata);
            bool ok = ConfigurationMenu(parent, theme, eventvars, ref jf);
            if (ok)
                userdata = jf.ToString();
            return ok;
        }

        public bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars, ref ConditionLists jf)
        {
            EDDiscovery2.ConditionFilterForm frm = new EDDiscovery2.ConditionFilterForm();
            frm.InitCondition("Define condition", eventvars, theme, jf);

            frm.TopMost = parent.FindForm().TopMost;
            if (frm.ShowDialog(parent.FindForm()) == DialogResult.OK)
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
            return cl2.FromString(userdata);        // returns null if everything is hunky
        }
    }

    public class ActionIf : ActionIfElseBase
    {
        ConditionLists condition;

        public ActionIf(string n, ActionType t, string ud, int lu) : base(n, t, ud, lu)
        {
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            if (ap.IsExecuteOn)
            {
                if (condition == null)
                {
                    condition = new ConditionLists();
                    if (condition.FromString(UserData) != null)
                    {
                        ap.ReportError("IF condition is not correctly formed");
                        return true;
                    }
                }

                string errlist;
                bool? condres = condition.CheckAll(ap.currentvars, out errlist, null, ap.functions.ExpandString);     // may return null.. and will return errlist

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

        public ActionElseIf(string n, ActionType t, string ud, int lu) : base(n, t, ud, lu)
        {
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            if (ap.IsExecutingType(Action.ActionType.If))
            {
                if (ap.IsExecuteOff)       // if not executing, check condition
                {
                    if (condition == null)
                    {
                        condition = new ConditionLists();
                        if (condition.FromString(UserData) != null)
                        {
                            ap.ReportError("IF condition is not correctly formed");
                            return true;
                        }
                    }

                    string errlist;
                    bool? condres = condition.CheckAll(ap.currentvars, out errlist, null, ap.functions.ExpandString);     // may return null.. and will return errlist

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

    public class ActionElse : Action
    {
        public ActionElse(string n, ActionType t, string ud, int lu) : base(n, t, ud, lu)
        {
        }

        public override bool ConfigurationMenuInUse { get { return false; } }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            if (ap.IsExecutingType(Action.ActionType.If))
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

        public ActionWhile(string n, ActionType t, string ud, int lu) : base(n, t, ud, lu)
        {
        }

        public override bool ExecuteAction(ActionProgramRun ap)             // WHILE at top of loop
        {
            if (ap.IsExecuteOn)       // if executing
            {
                if (condition == null)
                {
                    condition = new ConditionLists();
                    if (condition.FromString(UserData) != null)
                    {
                        ap.ReportError("While condition is not correctly formed");
                        return true;
                    }
                }

                string errlist;
                bool? condres = condition.CheckAll(ap.currentvars, out errlist, null, ap.functions.ExpandString);     // may return null.. and will return errlist

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
                    if (condition.FromString(UserData) != null)
                    {
                        ap.ReportError("While condition in Do..While is not correctly formed");
                        return true;
                    }
                }

                string errlist;
                bool? condres = condition.CheckAll(ap.currentvars, out errlist, null, ap.functions.ExpandString);     // may return null.. and will return errlist

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

    public class ActionDo : Action
    {
        public ActionDo(string n, ActionType t, string ud, int lu) : base(n, t, ud, lu)
        {
        }

        public override bool ConfigurationMenuInUse { get { return false; } }

        public override string DisplayedUserData { get { return null;  } }

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

    public class ActionLoop : Action
    {
        public ActionLoop(string n, ActionType t, string ud, int lu) : base(n, t, ud, lu)
        {
        }

        private bool counting = false;
        private int loopcount = 0;

        public override bool AllowDirectEditingOfUserData { get { return true; } }    // and allow editing?

        public override string VerifyActionCorrect()
        {
            return (UserData.Length > 0) ? null : "Loop missing loop count";
        }

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
             string promptValue = PromptSingleLine.ShowDialog(parent, "Enter integer count", UserData, "Configure Loop");
             if (promptValue != null)
                userdata = promptValue;

            return (promptValue != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)     // LOOP when encountered
        {
            if (ap.IsExecuteOn)       // if executing
            {
                if (!counting)      // if 
                {
                    string res;
                    if (ap.functions.ExpandString(UserData, ap.currentvars, out res) != ConditionLists.ExpandResult.Failed)
                    {
                        if (int.TryParse(res, out loopcount))
                        {
                            counting = true;
                            ap.PushState(Type, (loopcount > 0), true);   // set execute to On (if loop count is >0) and push the position of the LOOP
                            ap.currentvars["Loop" + ap.ExecLevel] = "1";
                        }
                        else
                            ap.ReportError("Loop count must be an integer");
                    }
                    else
                        ap.ReportError(res);
                }
                else
                    ap.ReportError("Internal error - Loop is saying counting when run");
            }
            else
            {
                ap.PushState(Type, ActionProgramRun.ExecState.OffForGood);  // push off for good, don't push position since we don't want to loop
            }

            return true;
        }

        public bool ExecuteEndLoop(ActionProgramRun ap)     // only called if executing
        {
            if ( counting )
            {
                if (--loopcount > 0)
                {
                    ap.Goto(ap.PushPos + 1);                    // back to LOOP+1, keep level

                    int c = 0;
                    if (int.TryParse(ap.currentvars["Loop" + ap.ExecLevel], out c)) // update LOOP level variable.. don't if they have mucked it up
                        ap.currentvars["Loop" + ap.ExecLevel] = (c + 1).ToString();

                    return true;
                }
                else
                {
                    counting = false;                           // turn off check flag, and we just exit the loop normally
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

        public ActionErrorIf(string n, ActionType t, string ud, int lu) : base(n, t, ud, lu)
        { 
        }

        public bool FromString(string s, out ConditionLists cond , out string errmsg )
        {
            cond = new ConditionLists();

            StringParser p = new StringParser(s);
            errmsg = p.NextQuotedWord(" ,");
            
            if ( errmsg != null && p.IsCharMoveOn(','))
            {
                string condstring = p.LineLeft;

                if (cond.FromString(condstring) == null)
                    return true;
            }

            errmsg = "";
            return false;
        }

        public string ToString(ConditionLists cond, string errmsg)
        {
            return errmsg.QuotedEscapeString() + ", " + cond.ToString();
        }

        public override string VerifyActionCorrect()
        {
            ConditionLists cond;
            string errmsg;
            return FromString(userdata, out cond, out errmsg) ? null : "ErrorIf not in correct format: \"Error string\", condition";
        }

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            ConditionLists cond;
            string errmsg;
            FromString(userdata, out cond, out errmsg);

            if (base.ConfigurationMenu(parent, theme, eventvars, ref cond))
            {
                string promptValue = PromptSingleLine.ShowDialog(parent, "Error to display", errmsg, "Configure ErrorIf Command");
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
                if ( !FromString(userdata, out condition, out errmsg) )
                {
                    ap.ReportError("ErrorIF condition is not correctly formed");
                    return true;
                }
            }

            string errlist;
            bool? condres = condition.CheckAll(ap.currentvars, out errlist, null, ap.functions.ExpandString);     // may return null.. and will return errlist

            if (errlist == null)
            {
                bool res = condres.HasValue && condres.Value;

                if (res)
                    ap.ReportError(errmsg);
            }
            else
                ap.ReportError(errlist);

            return true;
        }
    }

    public class ActionCall : Action
    {
        public ActionCall(string n, ActionType t, string ud, int lu) : base(n, t, ud, lu)
        { 
        }

        public bool FromString(string s, out string progname, out ConditionVariables vars )
        {
            StringParser p = new StringParser(s);
            progname = p.NextWord("( ");        // stop at space or (
            vars = new ConditionVariables();

            if (progname != null)
            {
                if (p.IsCharMoveOn('('))       // if (, then
                {
                    if (vars.FromString(p,ConditionVariables.FromMode.MultiEntryCommaBracketEnds) && p.IsCharMoveOn(')') && p.IsEOL)      // if para list decodes and we finish on a ) and its EOL
                        return true;
                }
                else if (p.IsEOL)   // if EOL, its okay, prog name only
                    return true;
            }

            progname = "";
            vars.Clear();

            return false;
        }

        public string ToString(string progname, ConditionVariables cond)
        {
            if (cond.Count > 0)
                return progname + "(" + cond.ToString() + ")";
            else
                return progname;
        }

        public override string VerifyActionCorrect()
        {
            string progname;
            ConditionVariables vars;
            return FromString(userdata, out progname, out vars) ? null : "Call not in correct format: progname (var list v=\"y\")";
        }

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            string progname;
            ConditionVariables cond;
            FromString(UserData, out progname, out cond);

            string promptValue = PromptSingleLine.ShowDialog(parent, "Program to call (use set::prog if req)", progname, "Configure Call Command");
            if (promptValue != null)
            {
                ConditionVariablesForm avf = new ConditionVariablesForm();
                avf.Init("Variables to pass into called program", theme, cond);

                if (avf.ShowDialog(parent.FindForm()) == DialogResult.OK)
                {
                    userdata = ToString(promptValue, avf.result);
                    return true;
                }
            }

            return false;
        }
        
        //special call for execute, needs to pass back more data
        public bool ExecuteCallAction(ActionProgramRun ap, out string progname, out ConditionVariables vars )
        {
            FromString(UserData, out progname, out vars);

            if (progname.Length > 0)
                return true;
            else
            {
                ap.ReportError("Call not configured");
                return false;
            }
        }
    }
}

