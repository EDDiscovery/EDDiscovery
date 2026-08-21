/*
 * Copyright 2015 - 2026 EDDiscovery development team
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

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<BaseUtils.TypeHelpers.PropertyNameInfo> eventvars)
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
            if (ap.Functions.ExpandString(UserData, out res) != BaseUtils.Functions.ExpandResult.Failed)
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

                BindingsFile bf = (ap.ActionController as ActionController).FrontierBindings;

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

                    List<BindingsFile.DeviceKeySet> matches = bf.FindDeviceKey(null, cmdname, partial);

                    if (matches.Count > 0)
                    {
                        foreach (BindingsFile.DeviceKeySet a in matches)
                        {
                            string keylist = a.Primary ? a.Entry.PrimaryKeyList() : a.Entry.SecondaryKeyList();
                            ap[prefix + "Binding" + matchno.ToStringInvariant()] = keylist + 
                                "=" + a.Entry.Name.ToString();
                            list += keylist + "=" + a.Entry.Name + Environment.NewLine;
                            matchno++;
                        }
                    }

                    foreach( var entry in bf.Assignments)
                    { 
                        foreach(var kvp in entry.Values)
                        {
                            if ( partial ? kvp.Key.StartsWithIIC(cmdname) : kvp.Key.EqualsIIC(cmdname))
                            {
                                ap[prefix + kvp.Key] = kvp.Value;
                                list += kvp.Key + "=" + kvp.Value + Environment.NewLine;
                            }
                        }
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
