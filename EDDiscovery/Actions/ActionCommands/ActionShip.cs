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

namespace EDDiscovery.Actions
{
    class ActionShip: Action
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(System.Windows.Forms.Form parent, EDDiscoveryForm discoveryform, List<string> eventvars)
        {
            string promptValue = Forms.PromptSingleLine.ShowDialog(parent, "Ship name", UserData, "Configure Ship Command");
            if (promptValue != null)
            {
                userdata = promptValue;
            }

            return (promptValue != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string res;
            if (ap.functions.ExpandString(UserData, out res) != ConditionFunctions.ExpandResult.Failed)
            {
                StringParser sp = new StringParser(res);

                string prefix = "SH_";
                string cmdname = sp.NextQuotedWord();

                if (cmdname != null && cmdname.Equals("PREFIX", StringComparison.InvariantCultureIgnoreCase))
                {
                    prefix = sp.NextWord();

                    if (prefix == null)
                    {
                        ap.ReportError("Missing name after Prefix in Ship");
                        return true;
                    }

                    cmdname = sp.NextQuotedWord();
                }

                if (cmdname != null)
                {
                    EliteDangerous.ShipInformationList lst = ap.actioncontroller.HistoryList.shipinformationlist;

                    ConditionVariables values = new ConditionVariables();

                    if (cmdname.Length > 0)
                    {
                        EliteDangerous.ShipInformation si = lst.GetShipByFullInfoMatch(cmdname);

                        if (si != null)
                            ActionVars.ShipInformation(values, si, prefix, true);

                        values[prefix + "Found"] = (si != null) ? "1" : "0";
                    }

                    values[prefix + "Ships"] = lst.Ships.Count.ToString(System.Globalization.CultureInfo.InvariantCulture);

                    int ind = 0;
                    foreach(EliteDangerous.ShipInformation si in lst.Ships.Values )
                    {
                        string p = prefix + "Ships[" + ind.ToString() + "]_";
                        ActionVars.ShipInformation(values, si, p, false);
                        ind++;
                    }

                    ap.Add(values);
                }
                else
                    ap.ReportError("Missing ship name in Ship");
            }
            else
                ap.ReportError(res);

            return true;
        }
    }

}
