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
    public class ActionPragma : Action
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(Form parent, EDDiscoveryForm discoveryform, List<string> eventvars)
        {
            string promptValue = Forms.PromptSingleLine.ShowDialog(parent, discoveryform.theme, "Pragma", UserData, "Configure Pragma Command");
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

                string cmd;
                while ((cmd = p.NextWord(" ", true)) != null)
                {
                    if (cmd.Equals("dumpvars"))
                    {
                        string rest = p.NextQuotedWord();

                        if (rest != null && rest.Length > 0)
                        {
                            foreach (KeyValuePair<string, string> k in ap.currentvars.FilterVars(rest).values)
                            {
                                ap.actioncontroller.LogLine(k.Key + "=" + k.Value);
                            }
                        }
                        else
                        {
                            ap.ReportError("Missing variable wildcard after Pragma DumpVars");
                            return true;
                        }
                    }
                    else if (cmd.Equals("log"))
                    {
                        string rest = p.NextQuotedWord(replaceescape: true);

                        if (rest != null)
                        {
                            ap.actioncontroller.LogLine(rest);
                        }
                        else
                        {
                            ap.ReportError("Missing string after Pragma Log");
                            return true;
                        }
                    }
                    else if (cmd.Equals("debug"))
                    {
                        string rest = p.NextQuotedWord(replaceescape: true);

                        if (rest != null)
                        {
#if DEBUG
                            ap.actioncontroller.LogLine(rest);
#endif
                        }
                        else
                        {
                            ap.ReportError("Missing string after Debug");
                        }
                        return true;
                    }
                    else if (cmd.Equals("ignoreerrors"))
                    {
                        ap.SetContinueOnErrors(true);
                    }
                    else if (cmd.Equals("allowerrors"))
                    {
                        ap.SetContinueOnErrors(true);
                    }
                }
            }
            else
                ap.ReportError(res);

            return true;
        }
    }
}
