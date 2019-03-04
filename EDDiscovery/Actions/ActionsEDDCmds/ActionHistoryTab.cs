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
using ActionLanguage;

namespace EDDiscovery.Actions
{
    class ActionHistoryTab : ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<BaseUtils.TypeHelpers.PropertyNameInfo> eventvars)
        {
            string promptValue = ExtendedControls.PromptSingleLine.ShowDialog(parent, "HistoryTab command", UserData, "Configure HistoryTab Command" , cp.Icon);
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

                string cmdname = sp.NextWord();

                if (cmdname == null)
                {
                    ap.ReportError("Missing panel name in Historytab");
                }
                else
                {
                    ExtendedControls.TabStrip ts = (ap.actioncontroller as ActionController).DiscoveryForm.PrimarySplitter.GetTabStrip(cmdname);     // case insensitive

                    if (ts != null)
                    {
                        string nextcmd = sp.NextWordLCInvariant(" ");

                        if (nextcmd == null)
                        {
                            ap.ReportError("Missing command after panel name in Historytab");
                        }
                        else if (nextcmd.Equals("toggle"))
                        {
                            ts.Toggle();
                        }
                        else
                        {
                            PopOutControl poc = (ap.actioncontroller as ActionController).DiscoveryForm.PopOuts;
                            PanelInformation.PanelIDs? id = PanelInformation.GetPanelIDByWindowsRefName(nextcmd);

                            if (id != null)
                            {
                                PanelInformation.PanelIDs[] list = ts.TagList.Cast<PanelInformation.PanelIDs>().ToArray();
                                int index = Array.IndexOf(list, id.Value);
                                if (!ts.ChangePanel(index))
                                    ap.ReportError("Panel " + nextcmd + " cannot be used in Historytab");
                            }
                            else
                                ap.ReportError("Cannot find generic panel type name " + nextcmd + " in Historytab");
                        }
                    }
                    else
                    {
                        ap.ReportError("Unknown panel name or panels re-ordered " + cmdname + " in Historytab");
                    }
                }
            }
            else
                ap.ReportError(res);

            return true;
        }

    }


}
