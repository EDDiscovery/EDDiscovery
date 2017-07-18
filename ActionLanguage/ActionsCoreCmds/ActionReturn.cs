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

namespace ActionLanguage
{ 
    class ActionReturn : ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<string> eventvars)
        {
            string promptValue = ExtendedControls.PromptSingleLine.ShowDialog(parent, "Return", UserData.ReplaceEscapeControlChars(), 
                                "Configure Return Command" , cp.Icon, true);

            if (promptValue != null)
                userdata = promptValue.EscapeControlChars();

            return (promptValue != null);
        }

        public bool ExecuteActionReturn(ActionProgramRun ap , out string retstr )
        {
            string res;         // we keep the string escaped, since all strings are normally kept escaped
            if (ap.functions.ExpandString(UserData, out res) != Conditions.ConditionFunctions.ExpandResult.Failed)       //Expand out.. and if no errors
            {
                retstr = res;
                return true;
            }
            else
            {
                ap.ReportError(res);
                retstr = null;
                return false;
            }
        }
    }
}
