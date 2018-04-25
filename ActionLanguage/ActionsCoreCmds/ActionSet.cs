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
    public class ActionSetLetBase : ActionBase
    {
        protected bool FromString(string ud, out ConditionVariables vars, out Dictionary<string, string> operations)
        {
            vars = new ConditionVariables();
            operations = new Dictionary<string, string>();
            StringParser p = new StringParser(ud);
            return vars.FromString(p, ConditionVariables.FromMode.OnePerLine, altops:operations);
        }

        protected string ToString(ConditionVariables vars, Dictionary<string, string> operations)
        {
            return vars.ToString(operations, pad: " ", comma:false, bracket:false, space:false);
        }

        public bool ConfigurationMenu(Form parent, ActionCoreController cp, List<string> eventvars, bool allowaddv , bool allownoexpandv)
        {
            ConditionVariables av;
            Dictionary<string, string> operations;
            FromString(userdata, out av, out operations);

            ConditionVariablesForm avf = new ConditionVariablesForm();
            avf.Init("Define Variable:", cp.Icon, av, showone: true, allowadd: allowaddv, allownoexpand: allownoexpandv, altops:operations, allowmultiple:false);

            if (avf.ShowDialog(parent) == DialogResult.OK)
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


        ConditionVariables av;
        Dictionary<string, string> operations;

        public bool ExecuteAction(ActionProgramRun ap, bool setit, bool globalit =false, bool persistentit =false, bool staticit = false )
        {
            if (av == null)
                FromString(userdata, out av, out operations);

            foreach (string key in av.NameEnumuerable)
            {
                string keyname = key;

                if (keyname.Contains("%"))      // if its an expansion, got for expansion
                {
                    if (ap.functions.ExpandString(key, out keyname) == ConditionFunctions.ExpandResult.Failed)
                    {
                        ap.ReportError(keyname);
                        break;
                    }
                }
                else
                    keyname = ap.variables.Qualify(key);    // else allow name to be mangled

                string res;

                if (operations[key].Contains("$"))
                    res = av[key];
                else if (ap.functions.ExpandString(av[key], out res) == ConditionFunctions.ExpandResult.Failed)       //Expand out.. and if no errors
                {
                    ap.ReportError(res);
                    break;
                }

                if (setit)
                {
                    if (operations[key].Contains("+") && ap.VarExist(keyname))
                        res = ap[keyname] + res;
                }
                else
                {
                    if (!res.Eval(out res))
                    {
                        ap.ReportError(res);
                        break;
                    }
                }

                //System.Diagnostics.Debug.WriteLine("Var " + keyname + "=" + res + "  :" + globalit + ":" + persistentit);
                ap[keyname] = res;

                if (globalit)
                    ap.actioncontroller.SetNonPersistentGlobal(keyname, res);

                if (persistentit )
                    ap.actioncontroller.SetPeristentGlobal(keyname, res);

                if (staticit )
                    ap.actionfile.SetFileVariable(keyname, res);
            }

            if (av.Count == 0)
                ap.ReportError("No variable name given in variable assignment");

            return true;
        }
    }

    public class ActionSet : ActionSetLetBase
    {
        public override bool ConfigurationMenu(Form parent, ActionCoreController discoveryform, List<string> eventvars)
        {
            return base.ConfigurationMenu(parent, discoveryform, eventvars, true, true);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            return ExecuteAction(ap, true);
        }
    }

    public class ActionGlobal : ActionSetLetBase
    {
        public override bool ConfigurationMenu(Form parent, ActionCoreController discoveryform, List<string> eventvars)
        {
            return base.ConfigurationMenu(parent, discoveryform, eventvars, true, true);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            return ExecuteAction(ap, true, globalit:true);
        }
    }

    public class ActionLet : ActionSetLetBase
    {
        public override bool ConfigurationMenu(Form parent, ActionCoreController discoveryform, List<string> eventvars)
        {
            return base.ConfigurationMenu(parent, discoveryform, eventvars, false, true);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            return ExecuteAction(ap, false);
        }
    }

    public class ActionGlobalLet : ActionSetLetBase
    {
        public override bool ConfigurationMenu(Form parent, ActionCoreController discoveryform, List<string> eventvars)
        {
            return base.ConfigurationMenu(parent, discoveryform, eventvars, false, true);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            return ExecuteAction(ap, false, globalit:true);
        }
    }

    public class ActionStaticLet : ActionSetLetBase
    {
        public override bool ConfigurationMenu(Form parent, ActionCoreController discoveryform, List<string> eventvars)
        {
            return base.ConfigurationMenu(parent, discoveryform, eventvars, false, true);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            return ExecuteAction(ap, false, staticit: true);
        }
    }

    public class ActionPersistentGlobal : ActionSetLetBase
    {
        public override bool ConfigurationMenu(Form parent, ActionCoreController discoveryform, List<string> eventvars)
        {
            return base.ConfigurationMenu(parent, discoveryform, eventvars, true, true);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            return ExecuteAction(ap, true, persistentit: true);
        }
    }

    public class ActionStatic : ActionSetLetBase
    {
        public override bool ConfigurationMenu(Form parent, ActionCoreController discoveryform, List<string> eventvars)
        {
            return base.ConfigurationMenu(parent, discoveryform, eventvars, true, true);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            return ExecuteAction(ap, true, staticit: true);
        }
    }


    public class ActionDeleteVariable: ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<string> eventvars)
        {
            string promptValue = ExtendedControls.PromptSingleLine.ShowDialog(parent, "Variable name", UserData, "Configure DeleteVariable Command" , cp.Icon);
            if (promptValue != null)
            {
                userdata = promptValue;
            }

            return (promptValue != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string res;
            if (ap.functions.ExpandString(UserData,  out res) != ConditionFunctions.ExpandResult.Failed)
            {
                StringParser p = new StringParser(res);

                string v;
                while ((v = p.NextWord(", ")) != null)
                {
                    v = ap.variables.Qualify(v);
                    ap.actioncontroller.DeleteVariableWildcard(v);
                    ap.actionfile.DeleteFileVariableWildcard(v);
                    ap.DeleteVariableWildcard(v);
                    p.IsCharMoveOn(',');
                }
            }
            else
                ap.ReportError(res);

            return true;
        }
    }

    public class ActionExpr: ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<string> eventvars)
        {
            string promptValue = ExtendedControls.PromptSingleLine.ShowDialog(parent, "Expression", UserData, "Configure Function Expression", cp.Icon);
            if (promptValue != null)
            {
                userdata = promptValue;
            }

            return (promptValue != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string res;
            if (ap.functions.ExpandString(UserData, out res) != ConditionFunctions.ExpandResult.Failed)
            {
                ap["Result"] = res;
            }
            else
                ap.ReportError(res);

            return true;
        }
    }
}
