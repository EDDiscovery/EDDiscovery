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
        public ActionIfElseBase(string n, ActionType type, List<string> c, string ud, int lu) : base(n, type, c, ud, lu)
        {
        }

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            EDDiscovery2.ConditionFilterForm frm = new EDDiscovery2.ConditionFilterForm();

            ConditionLists jf = new ConditionLists();
            if (UserData.Length > 0)
                jf.FromJSON(UserData);

            frm.InitCondition("Define condition", eventvars, theme, jf);

            frm.TopMost = parent.FindForm().TopMost;
            if (frm.ShowDialog(parent.FindForm()) == DialogResult.OK)
            {
                userdata = frm.result.GetJSON();
                return true;
            }
            else
                return false;
        }

        public override string DisplayedUserData
        {
            get
            {
                if (UserData.Length > 0)
                {
                    ConditionLists jf = new ConditionLists();
                    jf.FromJSON(UserData);
                    return jf.ToString();
                }
                else
                    return "";
            }
        }
    }

    public class ActionIf : ActionIfElseBase
    {
        ConditionLists condition;

        public ActionIf(string n, ActionType type, List<string> c, string ud, int lu) : base(n, type, c, ud, lu)
        {
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            if (ap.IsExecuteOn)
            {
                if (condition == null)
                {
                    condition = new ConditionLists();
                    if (!condition.FromJSON(UserData))
                    {
                        ap.ReportError("IF condition is not correctly formed");
                        return true;
                    }
                }

                bool? condres = condition.Check(ap.currentvars, null);     // may return null..
                bool res = condres.HasValue && condres.Value;
                ap.PushState(Type, res );       // true if has values and true, else false
            }
            else
                ap.PushState(Type, ActionProgramRun.ExecState.OffForGood);

            return true;

        }
    }

    public class ActionElseIf : ActionIfElseBase
    {
        ConditionLists condition;

        public ActionElseIf(string n, ActionType type, List<string> c, string ud, int lu) : base(n, type, c, ud, lu)
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
                        if (!condition.FromJSON(UserData))
                        {
                            ap.ReportError("IF condition is not correctly formed");
                            return true;
                        }
                    }

                    bool? condres = condition.Check(ap.currentvars, null);     // may return null..
                    bool res = condres.HasValue && condres.Value;
                    ap.ChangeState(res);            // either to ON.. or to OFF, continuing the off
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
        public ActionElse(string n, ActionType type, List<string> c, string ud, int lu) : base(n, type, c, ud, lu)
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

        public ActionWhile(string n, ActionType t, List<string> c, string ud, int lu) : base(n, t, c, ud, lu)
        {
        }

        public override bool ExecuteAction(ActionProgramRun ap)             // WHILE at top of loop
        {
            if (ap.IsExecuteOn)       // if executing
            {
                if (condition == null)
                {
                    condition = new ConditionLists();
                    if (!condition.FromJSON(UserData))
                    {
                        ap.ReportError("While condition is not correctly formed");
                        return true;
                    }
                }

                bool? condres = condition.Check(ap.currentvars, null);     // may return null..
                bool res = condres.HasValue && condres.Value;

                ap.PushState(Type, res, res);   // set execute state, and push position if executing, returning us here when it drop out a level..
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
                    if (!condition.FromJSON(UserData))
                    {
                        ap.ReportError("While condition in Do..While is not correctly formed");
                        return true;
                    }
                }

                bool? condres = condition.Check(ap.currentvars, null);     // may return null..
                bool res = condres.HasValue && condres.Value;

                if (res)
                {
                    ap.Goto(ap.PushPos + 1);                        // back to DO+1, keep level
                    return true;
                }
            }

            return false;                                           // not executing the DO, so we just let the standard code drop the level.  Position will not be pushed
        }
    }

    public class ActionDo : Action
    {
        public ActionDo(string n, ActionType t, List<string> c, string ud, int lu) : base(n, t, c, ud, lu)
        {
        }

        public override bool ConfigurationMenuInUse { get { return false; } }

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
        public ActionLoop(string n, ActionType t, List<string> c, string ud, int lu) : base(n, t, c, ud, lu)
        {
        }

        private bool counting = false;
        private int loopcount = 0;

        public override bool AllowDirectEditingOfUserData { get { return true; } }    // and allow editing?

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
                    if (int.TryParse(UserData, out loopcount))
                    {
                        counting = true;
                        ap.PushState(Type, (loopcount > 0), true);   // set execute to On (if loop count is >0) and push the position of the LOOP
                    }
                    else
                        ap.ReportError("Loop count must be an integer");
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

        string flagErrorText = "ErrorText";

        public ActionErrorIf(string n, ActionType t, List<string> c, string ud, int lu) : base(n, t, c, ud, lu)
        {
        }

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            if (base.ConfigurationMenu(parent, theme, eventvars))
            {
                string curval = GetFlagAuxData(flagErrorText);
                string promptValue = PromptSingleLine.ShowDialog(parent, "Error to display", curval != null ? curval : "", "Configure ErrorIf Command");
                if (promptValue != null)
                {
                    SetFlag(flagErrorText, true, promptValue);
                    return true;
                }
            }

            return false;
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            if (condition == null)
            {
                condition = new ConditionLists();
                if (!condition.FromJSON(UserData))
                {
                    ap.ReportError("ErrorIF condition is not correctly formed");
                    return true;
                }
            }

            bool? condres = condition.Check(ap.currentvars, null);     // may return null..
            bool res = condres.HasValue && condres.Value;

            if (res)
            {
                string curval = GetFlagAuxData(flagErrorText);
                if (curval == null)
                    curval = "ErrorIf Tripped";
                ap.ReportError(curval);
            }

            return true;
        }
    }

}

