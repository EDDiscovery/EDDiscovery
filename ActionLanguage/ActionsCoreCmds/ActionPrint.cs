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
    public class ActionPrint : ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }    // and allow editing?

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<string> eventvars)
        {
            string promptValue = ExtendedControls.PromptSingleLine.ShowDialog(parent, "Line to display", 
                            UserData.ReplaceEscapeControlChars(), "Configure Print Command" , cp.Icon, true);

            if (promptValue != null)
                userdata = promptValue.EscapeControlChars();

            return (promptValue != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string res;
            if (ap.functions.ExpandString(UserData.ReplaceEscapeControlChars(), out res) != Conditions.ConditionFunctions.ExpandResult.Failed)
            {
                ap.actioncontroller.LogLine(res);
                System.Diagnostics.Trace.WriteLine("PRINT " + res);
            }
            else
                ap.ReportError(res);

            return true;
        }

    }
   
}
