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

namespace EDDiscovery.Actions
{ 
    public class ActionMaterialsCommoditiesBase : Action
    {
        protected bool commodities = false;

        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(Form parent, EDDiscoveryForm discoveryform, List<string> eventvars)
        {
            string promptValue = Forms.PromptSingleLine.ShowDialog(parent, "JID of event", UserData, (commodities) ? "Configure Commodities Command" : "Configure Material Command");
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
                StringParser sp = new StringParser(res);

                string prefix = (commodities) ? "C_" : "M_";
                string cmdname = sp.NextWord();

                if (cmdname != null && cmdname.Equals("PREFIX", StringComparison.InvariantCultureIgnoreCase))
                {
                    prefix = sp.NextWord();

                    if (prefix == null)
                    {
                        ap.ReportError("Missing name after Prefix in Materials/Commodity");
                        return true;
                    }

                    cmdname = sp.NextWord();
                }

                if (cmdname != null)
                {
                    long jid;
                    if (!cmdname.InvariantParse(out jid))
                    {
                        ap.ReportError("Non integer JID in Materials/Commodity");
                        return true;
                    }

                    int jidindex = ap.actioncontroller.HistoryList.EntryOrder.FindIndex(x => x.Journalid == jid);

                    if (jidindex == -1)
                    {
                        ap.ReportError("JID does not exist in Materials/Commodity");
                        return true;
                    }

                    EDDiscovery2.DB.MaterialCommoditiesList mcl = ap.actioncontroller.HistoryList.EntryOrder[jidindex].MaterialCommodity;
                    List<EDDiscovery2.DB.MaterialCommodities> list = mcl.Sort(commodities);

                    ap.currentvars[prefix + "Count"] = list.Count.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    ap.currentvars[prefix + "IndexOf"] = ap.actioncontroller.HistoryList.EntryOrder[jidindex].Indexno.ToString(System.Globalization.CultureInfo.InvariantCulture);

                    for ( int i = 0; i < list.Count; i++ )
                    {
                        string postfix = (i + 1).ToString(System.Globalization.CultureInfo.InvariantCulture);
                        ap.currentvars[prefix + "Name" + postfix] = list[i].name;
                        ap.currentvars[prefix + "Category" + postfix] = list[i].category;
                        ap.currentvars[prefix + "fdname" + postfix] = list[i].fdname;
                        ap.currentvars[prefix + "type" + postfix] = list[i].type;
                        ap.currentvars[prefix + "shortname" + postfix] = list[i].shortname;
                    }
                }
                else
                    ap.ReportError("Missing JID in Materials");
            }
            else
                ap.ReportError(res);

            return true;
        }
    }

    public class ActionMaterials : ActionMaterialsCommoditiesBase
    {
        public ActionMaterials()
        {
            commodities = false;
        }
    }

    public class ActionCommodities: ActionMaterialsCommoditiesBase
    {
        public ActionCommodities()
        {
            commodities = true;
        }
    }
}
