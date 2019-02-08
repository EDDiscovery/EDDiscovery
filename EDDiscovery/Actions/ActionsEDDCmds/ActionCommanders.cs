/*
 * Copyright © 2017-2019 EDDiscovery development team
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
using BaseUtils;
using ActionLanguage;
using EliteDangerousCore;
using EliteDangerousCore.DB;

namespace EDDiscovery.Actions
{
    public class ActionCommanders : ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(System.Windows.Forms.Form parent, ActionCoreController cp, List<string> eventvars)
        {
            string promptValue = ExtendedControls.PromptSingleLine.ShowDialog(parent, "Command string", UserData, "Configure Commanders Command" , cp.Icon);
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

                string prefix = "CMDR_";
                string cmdname = sp.NextWord();

                if (cmdname != null && cmdname.Equals("PREFIX", StringComparison.InvariantCultureIgnoreCase))
                {
                    prefix = sp.NextWord();

                    if (prefix == null)
                    {
                        ap.ReportError("Missing name after Prefix");
                        return true;
                    }

                    cmdname = sp.NextWord();
                }

                int cmdrid = EDCommander.CurrentCmdrID;

                if (cmdname != null && cmdname.Equals("CMDR", StringComparison.InvariantCultureIgnoreCase))
                {
                    string name = sp.NextQuotedWord() ?? "-----!";
                    
                    EDCommander cmdr = EDCommander.GetCommander(name);

                    if (cmdr != null)
                        cmdrid = cmdr.Nr;
                    else
                        ap.ReportError("Commander not found");

                    cmdname = sp.NextWord();
                }

                EDDiscoveryForm discoveryform = (ap.actioncontroller as ActionController).DiscoveryForm;

                List<EDCommander> cmdrlist = EDCommander.GetCommanders();

                if (cmdname != null)
                {
                    if (cmdname.Equals("LIST", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string wildcard = sp.NextQuotedWord() ?? "*";

                        int count = 1;
                        foreach (var cmdr in cmdrlist)       // only current commander ID considered
                        {
                            if (cmdr.Name.WildCardMatch(wildcard))
                            {
                                DumpCMDR(ap, prefix + count++.ToStringInvariant() + "_", cmdr);
                            }
                        }

                        ap[prefix + "MatchCount"] = (count - 1).ToStringInvariant();
                        ap[prefix + "TotalCount"] = cmdrlist.Count.ToStringInvariant();
                    }
                    else if ( cmdname.Equals("CHANGETO", StringComparison.InvariantCultureIgnoreCase))
                    {
                        discoveryform.ChangeToCommander(cmdrid);                                   // which will cause DIsplay to be called as some point
                    }
                    else
                        ap.ReportError("Unknown command");
                }
                else
                    ap.ReportError("Missing command");
            }
            else
                ap.ReportError(res);

            return true;
        }

        void DumpCMDR( ActionProgramRun ap, string prefix, EDCommander cmdr)
        {
            ap[prefix + "Id"] = cmdr.Nr.ToStringInvariant();
            ap[prefix + "Name"] = cmdr.Name;
            ap[prefix + "EDSMName"] = cmdr.EdsmName;
            ap[prefix + "EGOName"] = cmdr.EGOName;
            ap[prefix + "INARAName"] = cmdr.InaraName;
            ap[prefix + "JournalDir"] = cmdr.JournalDir;
            ap[prefix + "SyncToEDSM"] = cmdr.SyncToEdsm.ToStringIntValue();
            ap[prefix + "SyncFromEDSM"] = cmdr.SyncFromEdsm.ToStringIntValue();
            ap[prefix + "SyncToEDDN"] = cmdr.SyncToEddn.ToStringIntValue();
            ap[prefix + "SyncToEGO"] = cmdr.SyncToEGO.ToStringIntValue();
            ap[prefix + "SyncToINARA"] = cmdr.SyncToInara.ToStringIntValue();
            ap[prefix + "Deleted"] = cmdr.Deleted.ToStringIntValue();
        }

    }
}
