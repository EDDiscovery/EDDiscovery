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
        protected bool FromString(string ud, out ConditionVariables vars, out Dictionary<string, string> operations)
        {
            vars = new ConditionVariables();
            operations = new Dictionary<string, string>();
            StringParser p = new StringParser(ud);
            return vars.FromString(p, ConditionVariables.FromMode.MultiEntryComma, altops:operations);
        }

        protected string ToString(ConditionVariables vars, Dictionary<string, string> operations)
        {
            return vars.ToString(operations, " ");
        }

        public bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars, bool allowaddv , bool allownoexpandv)
        {
            ConditionVariables av;
            Dictionary<string, string> operations;
            FromString(userdata, out av, out operations);

            ConditionVariablesForm avf = new ConditionVariablesForm();
            avf.Init("Variable list:", theme, av, showone: true, allowadd: allowaddv, allownoexpand: allownoexpandv, altops:operations);

            if (avf.ShowDialog(parent.FindForm()) == DialogResult.OK)
            {
                userdata = ToString(avf.result,avf.result_altops);
                return true;
            }
            else
                return false;
        }

        public override string VerifyActionCorrect()
        {
            ConditionVariables av;
            Dictionary<string, string> operations;
            bool ok = FromString(userdata, out av ,out operations);

            System.Diagnostics.Debug.Assert(operations.Count == av.Count);
            if ( ok )
                userdata = ToString(av,operations);        // normalise them..

            return ok ? null : "Global/Let/Set not in correct format";
        }

    }

    public class ActionSet : ActionSetLetBase
    {
        ConditionVariables av;
        Dictionary<string, string> operations;

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            return ConfigurationMenu(parent, theme, eventvars, true, true);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            if (av == null)
                FromString(userdata, out av, out operations);

            foreach (KeyValuePair<string, string> k in av.values)
            {
                string vname = k.Key;
                string res;

                if (operations[vname].Contains("$"))
                    res = k.Value;
                else if ( ap.functions.ExpandString(k.Value, ap.currentvars, out res) == ConditionLists.ExpandResult.Failed)       //Expand out.. and if no errors
                {
                    ap.ReportError(res);
                    break;
                }

                if (operations[vname].Contains("+") && ap.currentvars.ContainsKey(vname))
                    ap.currentvars[vname] += res;
                else
                    ap.currentvars[vname] = res;
            }

            if (av.Count == 0)
                ap.ReportError("Set no variable name given");

            return true;
        }

    }

    public class ActionLet : ActionSetLetBase
    {
        ConditionVariables av;
        Dictionary<string, string> operations;

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            return ConfigurationMenu(parent, theme, eventvars,false, true);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            if (av == null)
                FromString(userdata, out av, out operations);

            foreach (KeyValuePair<string, string> k in av.values)
            {
                string vname = k.Key;
                string res;

                if (operations[vname].Contains("$"))
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

                    ap.currentvars[vname] = res;
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
        Dictionary<string, string> operations;

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            return ConfigurationMenu(parent, theme, eventvars, true , true);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            if (av == null)
                FromString(userdata, out av, out operations);

            foreach (KeyValuePair<string, string> k in av.values)
            {
                string vname = k.Key;
                string res;

                if (operations[vname].Contains("$"))
                    res = k.Value;
                else if (ap.functions.ExpandString(k.Value, ap.currentvars, out res) == ConditionLists.ExpandResult.Failed)       //Expand out.. and if no errors
                {
                    ap.ReportError(res);
                    break;
                }

                if (operations[vname].Contains("+") && ap.currentvars.ContainsKey(vname))
                {
                    ap.currentvars[vname] += res;
                    ap.discoveryform.SetProgramGlobal(vname, res);
                }
                else 
                {
                    ap.currentvars[vname] = res;
                    ap.discoveryform.SetProgramGlobal(vname, res);
                }
            }

            if (av.Count == 0)
                ap.ReportError("Global no variable name given");

            return true;
        }

    }
}
