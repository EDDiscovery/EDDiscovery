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
        protected bool FromString(string ud, out bool noexpand, out ConditionVariables vars)
        {
            vars = new ConditionVariables();

            if (userdata.TrimStart().StartsWith("$"))
            {
                noexpand = true;
                return vars.FromString(userdata.Substring(userdata.IndexOf('$') + 1), ConditionVariables.FromMode.MultiEntryComma);
            }
            else
            {
                noexpand = false;
                return vars.FromString(userdata, ConditionVariables.FromMode.MultiEntryComma);
            }
        }

        protected string ToString(bool noexpand, ConditionVariables vars)
        {
            return ((noexpand) ? "$ " : "" ) + vars.ToString();
        }

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            ConditionVariables av;
            bool noexpand;
            FromString(userdata, out noexpand, out av);

            ConditionVariablesForm avf = new ConditionVariablesForm();
            avf.Init("Variable list:", theme, av, true, true, noexpand);

            if (avf.ShowDialog(parent.FindForm()) == DialogResult.OK)
            {
                userdata = ToString(avf.noexpand, avf.result);
                return true;
            }
            else
                return false;
        }

        public override string VerifyActionCorrect()
        {
            ConditionVariables av;
            bool f;
            return FromString(userdata, out f, out av) ? null : "Global/Let/Set not in correct format: ($) v=\"y\"";
        }

    }

    public class ActionSet : ActionSetLetBase
    {
        ConditionVariables av;
        bool noexpand;

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            if (av == null)
                FromString(userdata, out noexpand, out av);

            foreach (KeyValuePair<string, string> k in av.values)
            {
                string res;

                if (noexpand)
                    ap.currentvars[k.Key] = k.Value;
                else if ( ap.functions.ExpandString(k.Value, ap.currentvars, out res) != ConditionLists.ExpandResult.Failed)       //Expand out.. and if no errors
                    ap.currentvars[k.Key] = res;
                else
                {
                    ap.ReportError(res);
                    break;
                }
            }

            if (av.Count == 0)
                ap.ReportError("Set no variable name given");

            return true;
        }

    }

    public class ActionLet : ActionSetLetBase
    {
        ConditionVariables av;
        bool noexpand;

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            if (av == null)
                FromString(userdata, out noexpand, out av);

            foreach (KeyValuePair<string, string> k in av.values)
            {
                string res;

                if (noexpand)
                    res = k.Value;
                else if (ap.functions.ExpandString(k.Value, ap.currentvars, out res) == ConditionLists.ExpandResult.Failed)
                {
                    ap.ReportError(res);
                    break;
                }

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

                    ap.currentvars[k.Key] = res;
                }
                catch
                {
                    ap.ReportError("LET expression does not evaluate");
                    break;
                }
            }

            if (av.Count == 0)
                ap.ReportError("Let no variable name given");

            return true;
        }
    }

    public class ActionGlobal : ActionSetLetBase
    {
        ConditionVariables av;
        bool noexpand;

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            if (av == null)
                FromString(userdata, out noexpand, out av);

            foreach (KeyValuePair<string, string> k in av.values)
            {
                string res;

                if (noexpand)
                {
                    ap.currentvars[k.Key] = k.Value;
                    ap.discoveryform.SetProgramGlobal(k.Key, k.Value);
                }
                else if (ap.functions.ExpandString(k.Value, ap.currentvars, out res) != ConditionLists.ExpandResult.Failed)       //Expand out.. and if no errors
                {
                    ap.currentvars[k.Key] = res;
                    ap.discoveryform.SetProgramGlobal(k.Key, res);
                }
                else
                {
                    ap.ReportError(res);
                    break;
                }
            }

            if (av.Count == 0)
                ap.ReportError("Global no variable name given");

            return true;
        }

    }
}
