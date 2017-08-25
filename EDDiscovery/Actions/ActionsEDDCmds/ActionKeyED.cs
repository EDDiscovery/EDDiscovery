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
using AudioExtensions;
using Conditions;
using ActionLanguage;

namespace EDDiscovery.Actions
{
    public class ActionKeyED : ActionKey        // extends Key
    {
        static public string Menu(Control parent, System.Drawing.Icon ic, string userdata , EliteDangerousCore.BindingsFile bf)
        {
            ConditionVariables vars;
            string keys;
            FromString(userdata, out keys, out vars);

            ExtendedControls.KeyForm kf = new ExtendedControls.KeyForm();
            int defdelay = vars.Exists(DelayID) ? vars[DelayID].InvariantParseInt(DefaultDelay) : DefaultDelay;
            string process = vars.Exists(ProcessID) ? vars[ProcessID] : "";
            kf.Init(ic, true, " ", keys, process, defdelay: defdelay);

            if (kf.ShowDialog(parent) == DialogResult.OK)
            {
                return ToString(kf.KeyList, new ConditionVariables(new string[] { ProcessID, kf.ProcessSelected, DelayID, kf.DefaultDelay.ToStringInvariant() }));
            }
            else
                return null;
        }

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<string> eventvars)    // override again to expand any functionality
        {
            ActionController ac = cp as ActionController;

            string ud = Menu(parent, cp.Icon, userdata , ac.DiscoveryForm.FrontierBindings );      // base has no additional keys
            if (ud != null)
            {
                userdata = ud;
                return true;
            }
            else
                return false;
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string keys;
            ConditionVariables statementvars;
            if (FromString(userdata, out keys, out statementvars))
            {
                string errlist = null;
                ConditionVariables vars = statementvars.ExpandAll(ap.functions, statementvars, out errlist);

                if (errlist == null)
                {
                    int defdelay = vars.Exists(DelayID) ? vars[DelayID].InvariantParseInt(DefaultDelay) : DefaultDelay;
                    string process = vars.Exists(ProcessID) ? vars[ProcessID] : "";

                    ActionController ac = ap.actioncontroller as ActionController;
                    EliteDangerousCore.BindingsFile bf = ac.DiscoveryForm.FrontierBindings;

                    string res = BaseUtils.EnhancedSendKeys.Send(keys, defdelay, DefaultShiftDelay, DefaultUpDelay, process);

                    if (res.HasChars())
                        ap.ReportError("Key Syntax error : " + res);
                }
                else
                    ap.ReportError(errlist);
            }
            else
                ap.ReportError("Key command line not in correct format");

            return true;
        }
    }
}
