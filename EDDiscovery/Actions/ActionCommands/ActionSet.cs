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
        protected bool FromString(string ud, out bool noexpand, out bool addto, out ConditionVariables vars)
        {
            vars = new ConditionVariables();

            noexpand = false;
            addto = false;

            if (ud.StartsWith("+$"))
                addto = noexpand = true;
            else if (ud.StartsWith("+"))
                addto = true;
            else if (ud.StartsWith("$"))
                noexpand = true;

            int pos = (noexpand ? 1 : 0) + (addto ? 1 : 0);

            return vars.FromString(userdata.Substring(pos), ConditionVariables.FromMode.MultiEntryComma);
        }

        protected string ToString(bool noexpand, bool addto, ConditionVariables vars)
        {
            if ( addto && noexpand )
                return "+$ " + vars.ToString();
            else if (addto)
                return "+ " + vars.ToString();
            else if (noexpand)
                return "$ " + vars.ToString();
            else
                return vars.ToString();
        }

        public bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars, bool showaddtov)
        {
            ConditionVariables av;
            bool noexpand, addto;
            FromString(userdata, out noexpand, out addto, out av);

            ConditionVariablesForm avf = new ConditionVariablesForm();
            avf.Init("Variable list:", theme, av, showone:true, shownoexpand:true, notexpandstate: noexpand , showaddto:showaddtov, addtostate:addto);

            if (avf.ShowDialog(parent.FindForm()) == DialogResult.OK)
            {
                userdata = ToString(avf.result_noexpand, avf.result_addto, avf.result);
                return true;
            }
            else
                return false;
        }

        public override string VerifyActionCorrect()
        {
            ConditionVariables av;
            bool f,a;
            return FromString(userdata, out f, out a, out av) ? null : "Global/Let/Set not in correct format: ($) v=\"y\"";
        }

    }

    public class ActionSet : ActionSetLetBase
    {
        ConditionVariables av;
        bool noexpand;
        bool addto;

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            return ConfigurationMenu(parent, theme, eventvars, true);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            if (av == null)
                FromString(userdata, out noexpand, out addto, out av);

            foreach (KeyValuePair<string, string> k in av.values)
            {
                string res;

                if (noexpand)
                    res = k.Value;
                else if ( ap.functions.ExpandString(k.Value, ap.currentvars, out res) == ConditionLists.ExpandResult.Failed)       //Expand out.. and if no errors
                {
                    ap.ReportError(res);
                    break;
                }

                if (addto && ap.currentvars.ContainsKey(k.Key))
                    ap.currentvars[k.Key] += res;
                else
                    ap.currentvars[k.Key] = res;
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
        bool addto;

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            return ConfigurationMenu(parent, theme, eventvars, false);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            if (av == null)
                FromString(userdata, out noexpand, out addto, out av);

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
        bool addto;

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            return ConfigurationMenu(parent, theme, eventvars, true);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            if (av == null)
                FromString(userdata, out noexpand, out addto, out av);

            foreach (KeyValuePair<string, string> k in av.values)
            {
                string res;

                if (noexpand)
                    res = k.Value;
                else if (ap.functions.ExpandString(k.Value, ap.currentvars, out res) == ConditionLists.ExpandResult.Failed)       //Expand out.. and if no errors
                {
                    ap.ReportError(res);
                    break;
                }

                if ( addto && ap.currentvars.ContainsKey(k.Key))
                {
                    ap.currentvars[k.Key] += res;
                    ap.discoveryform.SetProgramGlobal(k.Key, res);
                }
                else 
                {
                    ap.currentvars[k.Key] = res;
                    ap.discoveryform.SetProgramGlobal(k.Key, res);
                }
            }

            if (av.Count == 0)
                ap.ReportError("Global no variable name given");

            return true;
        }

    }
}
