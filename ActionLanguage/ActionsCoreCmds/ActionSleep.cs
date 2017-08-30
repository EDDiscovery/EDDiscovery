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
    class ActionSleep : ActionBase
    {
        Timer t;
        ActionProgramRun apr;

        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override string VerifyActionCorrect()
        {
            return (UserData.Length > 0) ? null : "Sleep missing timeout in ms";
        }

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<string> eventvars)
        {
            string promptValue = ExtendedControls.PromptSingleLine.ShowDialog(parent, "Sleep time in ms:", UserData, "Set Sleep timeout" , cp.Icon);

            if (promptValue != null)
            {
                userdata = promptValue;
                return true;
            }

            return (promptValue != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string delay;
            if (ap.functions.ExpandString(userdata, out delay) == Conditions.ConditionFunctions.ExpandResult.Failed)       //Expand out.. and if no errors
            {
                ap.ReportError(delay);
                return true;
            }

            int i;
            if (delay.InvariantParse(out i) && i >= 0)
            {
                System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Doze for " + i);
                apr = ap;
                t = new Timer();
                t.Tick += T_Tick;
                t.Interval = i;
                t.Start();
                return false;
            }
            else
                ap.ReportError("Sleep requires a positive integer for time in ms");

            return true;
        }

        private void T_Tick(object sender, EventArgs e)
        {
            t.Stop();
            t.Dispose();
            t = null;
            System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Resume after sleep");
            apr.ResumeAfterPause();
        }
    }
}
