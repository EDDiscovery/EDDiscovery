/*
 * Copyright © 2015 - 2016 EDDiscovery development team
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
using ActionLanguage;
using EliteDangerousCore;

namespace EDDiscovery.Actions
{
    public class ActionEliteBindings : ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<string> eventvars)
        {
            string promptValue = ExtendedControls.PromptSingleLine.ShowDialog(parent, "EliteBindings commands", UserData, "Configure EliteBindings" ,cp.Icon);
            if (promptValue != null)
            {
                userdata = promptValue;
            }

            return (promptValue != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string res;
            if (ap.functions.ExpandString(UserData, out res) != BaseUtils.Functions.ExpandResult.Failed)
            {
                StringParser sp = new StringParser(res);

                string prefix = "EB_";
                string cmdname = sp.NextQuotedWord();

                if (cmdname != null && cmdname.Equals("PREFIX", StringComparison.InvariantCultureIgnoreCase))
                {
                    prefix = sp.NextWord();

                    if (prefix == null)
                    {
                        ap.ReportError("Missing name after Prefix in EliteBindings");
                        return true;
                    }

                    cmdname = sp.NextQuotedWord();
                }

                BindingsFile bf = (ap.actioncontroller as ActionController).FrontierBindings;

                int matchno = 1;
                string list = "";

                while ( cmdname != null )
                {
                    bool partial = false;
                    int i = cmdname.IndexOf("*");
                    if (i>=0)
                    {
                        cmdname = cmdname.Substring(0, i);
                        partial = true;
                    }

                    List<BindingsFile.Assignment> matches = bf.Find(cmdname, partial);

                    if (matches.Count > 0)
                    {
                        foreach (BindingsFile.Assignment a in matches)
                        {
                            ap[prefix + "Binding" + matchno.ToStringInvariant()] = a.ToString();
                            list += a.ToString() + Environment.NewLine;
                            matchno++;
                        }
                    }

                    Dictionary<string, string> values = bf.BindingValue(cmdname, partial);
                    foreach(string k in values.Keys)
                    {
                        ap[prefix + k] = values[k];
                        list += k + "=" + values[k] + Environment.NewLine;
                    }

                    cmdname = sp.NextQuotedWord();
                }

                ap[prefix + "Text"] = list;

            }
            else
                ap.ReportError(res);

            return true;
        }
    }
}
