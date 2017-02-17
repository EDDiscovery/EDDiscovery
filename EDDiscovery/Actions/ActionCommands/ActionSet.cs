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
 * EDDiscovery is not affiliated with Fronter Developments plc.
 */
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

        public bool ConfigurationMenu(Form parent, EDDiscoveryForm discoveryform, List<string> eventvars, bool allowaddv , bool allownoexpandv)
        {
            ConditionVariables av;
            Dictionary<string, string> operations;
            FromString(userdata, out av, out operations);

            ConditionVariablesForm avf = new ConditionVariablesForm();
            avf.Init("Variable list:", discoveryform.theme, av, showone: true, allowadd: allowaddv, allownoexpand: allownoexpandv, altops:operations);

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

            System.Diagnostics.Debug.Assert(ok == false || operations.Count == av.Count);

            if ( ok )
                userdata = ToString(av,operations);        // normalise them..

            return ok ? null : "Variable command not in correct format";
        }

    }

    public class ActionSet : ActionSetLetBase
    {
        ConditionVariables av;
        Dictionary<string, string> operations;

        public override bool ConfigurationMenu(Form parent, EDDiscoveryForm discoveryform, List<string> eventvars)
        {
            return base.ConfigurationMenu(parent, discoveryform, eventvars, true, true);
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

        public override bool ConfigurationMenu(Form parent, EDDiscoveryForm discoveryform, List<string> eventvars)
        {
            return base.ConfigurationMenu(parent, discoveryform, eventvars,false, true);
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

                string value;
                if (!res.Eval(out value))
                {
                    ap.ReportError("Let " + value);
                    break;
                }

                ap.currentvars[vname] = value;
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

        public override bool ConfigurationMenu(Form parent, EDDiscoveryForm discoveryform, List<string> eventvars)
        {
            return base.ConfigurationMenu(parent, discoveryform, eventvars, true , true);
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
                    ap.actioncontroller.SetNonPersistentGlobal(vname, ap.currentvars[vname]);
                }
                else 
                {
                    ap.currentvars[vname] = res;
                    ap.actioncontroller.SetNonPersistentGlobal(vname, res);
                }
            }

            if (av.Count == 0)
                ap.ReportError("Global no variable name given");

            return true;
        }

    }

    public class ActionPersistentGlobal : ActionSetLetBase
    {
        ConditionVariables av;
        Dictionary<string, string> operations;

        public override bool ConfigurationMenu(Form parent, EDDiscoveryForm discoveryform, List<string> eventvars)
        {
            return base.ConfigurationMenu(parent, discoveryform, eventvars, true, true);
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
                    ap.actioncontroller.SetPeristentGlobal(vname, ap.currentvars[vname]);
                }
                else
                {
                    ap.currentvars[vname] = res;
                    ap.actioncontroller.SetPeristentGlobal(vname, res);
                }
            }

            if (av.Count == 0)
                ap.ReportError("PersistentGlobal no variable name given");

            return true;
        }

    }

    public class ActionDeleteVariable: Action
    {
        public override bool ConfigurationMenu(Form parent, EDDiscoveryForm discoveryform, List<string> eventvars)
        {
            string promptValue = Forms.PromptSingleLine.ShowDialog(parent, discoveryform.theme, "Variable name", UserData, "Configure DeleteVariable Command");
            if (promptValue != null)
            {
                userdata = promptValue;
            }

            return (promptValue != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string res;
            if (ap.functions.ExpandString(UserData, ap.currentvars, out res) != ConditionLists.ExpandResult.Failed)
            {
                StringParser p = new StringParser(res);

                string v;
                while ((v = p.NextWord(", ")) != null)
                {
                    ap.actioncontroller.DeleteVariable(v);
                    ap.currentvars.Delete(v);
                    p.IsCharMoveOn(',');
                }
            }
            else
                ap.ReportError(res);

            return true;
        }
    }
}
