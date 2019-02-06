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
using BaseUtils;
using ActionLanguage;
using EliteDangerousCore;
using EliteDangerousCore.DB;

namespace EDDiscovery.Actions
{
    public class ActionTarget : ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(System.Windows.Forms.Form parent, ActionCoreController cp, List<string> eventvars)
        {
            string promptValue = ExtendedControls.PromptSingleLine.ShowDialog(parent, "Command string", UserData, "Configure Target Command" , cp.Icon);
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

                if (cmdname != null )
                {
                    if (cmdname.Equals("GET", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //                        ap[prefix + "MatchCount"] = (bcount - 1).ToStringInvariant();
                        //                      ap[prefix + "TotalCount"] = GlobalBookMarkList.Instance.Bookmarks.Count.ToStringInvariant();
                    }
                    else
                    {
                        string name = sp.NextWord();

                        if (name != null)
                        {
                            if (cmdname.Equals("BOOKMARK", StringComparison.InvariantCultureIgnoreCase))
                            {
                            }
                            else if (cmdname.Equals("BOOKMARKNEW", StringComparison.InvariantCultureIgnoreCase))
                            {
                            }
                            else if (cmdname.Equals("GMO", StringComparison.InvariantCultureIgnoreCase))
                            {
                            }
                            else if (cmdname.Equals("NOTE", StringComparison.InvariantCultureIgnoreCase))
                            {
                            }
                            else
                            {
                                ap.ReportError("Unknown TARGET command");
                            }
                        }
                        else
                            ap.ReportError("Missing name in command");
                    }
                }
                else
                    ap.ReportError("Missing TARGET command");
            }
            else
                ap.ReportError(res);

            return true;
        }
    }


}
