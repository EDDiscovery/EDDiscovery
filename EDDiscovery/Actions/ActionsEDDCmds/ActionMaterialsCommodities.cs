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
using EliteDangerousCore;

namespace EDDiscovery.Actions
{ 
    public class ActionMaterialsCommoditiesBase : ActionBase
    {
        protected bool commodities = false;

        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<BaseUtils.TypeHelpers.PropertyNameInfo> eventvars)
        {
            string promptValue = ExtendedControls.PromptSingleLine.ShowDialog(parent, "JID of event", UserData, (commodities) ? "Configure Commodities Command" : "Configure Material Command" , cp.Icon);
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

                    int jidindex = (ap.actioncontroller as ActionController).HistoryList.EntryOrder.FindIndex(x => x.Journalid == jid);

                    if (jidindex == -1)
                    {
                        ap.ReportError("JID does not exist in Materials/Commodity");
                        return true;
                    }

                    MaterialCommoditiesList mcl = (ap.actioncontroller as ActionController).HistoryList.EntryOrder[jidindex].MaterialCommodity;
                    List<MaterialCommodities> list = mcl.Sort(commodities);

                    ap[prefix + "Count"] = list.Count.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    ap[prefix + "IndexOf"] = (ap.actioncontroller as ActionController).HistoryList.EntryOrder[jidindex].Indexno.ToString(System.Globalization.CultureInfo.InvariantCulture);

                    for ( int i = 0; i < list.Count; i++ )
                    {
                        string postfix = (i + 1).ToString(System.Globalization.CultureInfo.InvariantCulture);
                        ap[prefix + "Name" + postfix] = list[i].Details.Name;
                        ap[prefix + "Category" + postfix] = list[i].Details.Category.ToString();
                        ap[prefix + "fdname" + postfix] = list[i].Details.FDName;
                        ap[prefix + "type" + postfix] = list[i].Details.Type.ToString().SplitCapsWord();
                        ap[prefix + "shortname" + postfix] = list[i].Details.Shortname;
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
