using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    public class ActionSetLetBase : Action
    {
        public ActionSetLetBase(string n, ActionType t, string ud, int lu) : base(n, t, ud, lu)
        {
        }

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            string var, value;
            ConditionVariables av = new ConditionVariables(userdata, ConditionVariables.FromMode.SingleEntry);
            av.GetFirstValue(out var, out value);       // if it does not exist, don't worry

            Tuple<string, string> promptValue = PromptDoubleLine.ShowDialog(parent, "Variable:", "Value:", var, value, "Set Variable");

            if (promptValue != null)
            {
                ConditionVariables av2 = new ConditionVariables();
                av2[promptValue.Item1] = promptValue.Item2;
                userdata = av2.ToString();
            }

            return (promptValue != null);
        }

        public override string VerifyActionCorrect()
        {
            ConditionVariables av = new ConditionVariables();
            return av.FromString(userdata, ConditionVariables.FromMode.SingleEntry) ? null : "Let/Set not in correct format: v=\"y\"";
        }

    }

    public class ActionSet : ActionSetLetBase
    {
        public ActionSet(string n, ActionType t, string ud, int lu) : base(n, t, ud, lu)
        {
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            ConditionVariables av = new ConditionVariables(userdata, ConditionVariables.FromMode.SingleEntry);

            if (av.Count > 0)
            {
                string res;
                if (ap.functions.ExpandString(av.First().Value, ap.currentvars, out res) != ConditionLists.ExpandResult.Failed)       //Expand out.. and if no errors
                {
                    ap.currentvars[av.First().Key] = res;
                }
                else
                    ap.ReportError(res);
            }
            else
                ap.ReportError("Set no variable name given");

            return true;
        }

    }

    public class ActionLet : ActionSetLetBase
    {
        public ActionLet(string n, ActionType t, string ud, int lu) : base(n, t, ud, lu)
        {
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            ConditionVariables av = new ConditionVariables(userdata, ConditionVariables.FromMode.SingleEntry);

            if ( av.Count>0)
            {
                string res;
                if (ap.functions.ExpandString(av.First().Value, ap.currentvars, out res) != ConditionLists.ExpandResult.Failed)       //Expand out.. and if no errors
                {
                    System.Data.DataTable dt = new System.Data.DataTable();

                    try
                    {
                        var v = dt.Compute(res, "");
                        Type t = v.GetType();
                        //System.Diagnostics.Debug.WriteLine("Type return is " + t.ToString());
                        if (v is double)
                            res = v.ToString();
                        else if (v is System.Decimal)
                            res = v.ToString();
                        else if (v is int)
                            res = v.ToString();
                        else
                            res = "NAN";

                        ap.currentvars[av.First().Key] = res;
                    }
                    catch
                    {
                        ap.ReportError("LET expression does not evaluate");
                    }
                }
                else
                    ap.ReportError(res);
            }
            else
                ap.ReportError("Let no variable name given");

            return true;
        }

    }
}
