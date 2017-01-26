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
        protected bool FromString(string ud, out ConditionVariables vars)
        {
            vars = new ConditionVariables();
            StringParser p = new StringParser(ud);
            return vars.FromString(p, ConditionVariables.FromMode.MultiEntryComma, allowextops:true);
        }

        protected string ToString(ConditionVariables vars)
        {
            return vars.ToString();
        }

        public bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars, bool allowaddv , bool allownoexpandv)
        {
            ConditionVariables av;
            FromString(userdata, out av);

            ConditionVariablesForm avf = new ConditionVariablesForm();
            avf.Init("Variable list:", theme, av, showone: true, allowadd: allowaddv, allownoexpand: allownoexpandv);

            if (avf.ShowDialog(parent.FindForm()) == DialogResult.OK)
            {
                userdata = ToString(avf.result);
                return true;
            }
            else
                return false;
        }

        public override string VerifyActionCorrect()
        {
            ConditionVariables av;
            bool ok = FromString(userdata, out av);
            if ( ok )
                userdata = ToString(av);        // normalise them..

            return ok ? null : "Global/Let/Set not in correct format";
        }

    }

    public class ActionSet : ActionSetLetBase
    {
        ConditionVariables av;

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            return ConfigurationMenu(parent, theme, eventvars, true, true);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            if (av == null)
                FromString(userdata, out av);

            foreach (KeyValuePair<string, string> k in av.values)
            {
                string vname = k.Key;
                bool noexpand = false, addto = false;
                if (vname.EndsWith("$+"))
                {
                    vname = vname.Substring(0, vname.Length - 2);
                    noexpand = addto = true;
                }
                else if (vname.EndsWith("+"))
                {
                    vname = vname.Substring(0, vname.Length - 1);
                    addto = true;
                }
                else if (vname.EndsWith("$"))
                {
                    vname = vname.Substring(0, vname.Length - 1);
                    noexpand = true;
                }

                string res;

                if (noexpand)
                    res = k.Value;
                else if ( ap.functions.ExpandString(k.Value, ap.currentvars, out res) == ConditionLists.ExpandResult.Failed)       //Expand out.. and if no errors
                {
                    ap.ReportError(res);
                    break;
                }

                if (addto && ap.currentvars.ContainsKey(vname))
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

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            return ConfigurationMenu(parent, theme, eventvars,false, true);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            if (av == null)
                FromString(userdata, out av);

            foreach (KeyValuePair<string, string> k in av.values)
            {
                string vname = k.Key;
                bool noexpand = false;
                if (vname.EndsWith("$"))
                {
                    vname = vname.Substring(0, vname.Length - 1);
                    noexpand = true;
                }

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

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            return ConfigurationMenu(parent, theme, eventvars, true , true);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            if (av == null)
                FromString(userdata, out av);

            foreach (KeyValuePair<string, string> k in av.values)
            {
                string vname = k.Key;
                bool noexpand = false, addto = false;
                if (vname.EndsWith("$+"))
                {
                    vname = vname.Substring(0, vname.Length - 2);
                    noexpand = addto = true;
                }
                else if (vname.EndsWith("+"))
                {
                    vname = vname.Substring(0, vname.Length - 1);
                    addto = true;
                }
                else if (vname.EndsWith("$"))
                {
                    vname = vname.Substring(0, vname.Length - 1);
                    noexpand = true;
                }

                string res;

                if (noexpand)
                    res = k.Value;
                else if (ap.functions.ExpandString(k.Value, ap.currentvars, out res) == ConditionLists.ExpandResult.Failed)       //Expand out.. and if no errors
                {
                    ap.ReportError(res);
                    break;
                }

                if ( addto && ap.currentvars.ContainsKey(vname))
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
