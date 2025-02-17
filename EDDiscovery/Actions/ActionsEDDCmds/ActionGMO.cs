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
using BaseUtils;
using ActionLanguage;

namespace EDDiscovery.Actions
{
    public class ActionGMO : ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(System.Windows.Forms.Form parent, ActionCoreController cp, List<BaseUtils.TypeHelpers.PropertyNameInfo> eventvars)
        {
            string promptValue = ExtendedControls.PromptSingleLine.ShowDialog(parent, "Command string", UserData, "Configure GMO Command" , cp.Icon);
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

                string prefix = "G_";
                string cmdname = sp.NextWord();

                if (cmdname != null && cmdname.Equals("PREFIX", StringComparison.InvariantCultureIgnoreCase))
                {
                    prefix = sp.NextWord();

                    if (prefix == null)
                    {
                        ap.ReportError("Missing name after Prefix in GMO");
                        return true;
                    }

                    cmdname = sp.NextWord();
                }

                if (cmdname != null )
                {
                    EDDiscoveryForm discoveryform = (ap.ActionController as ActionController).DiscoveryForm;

                    if (cmdname.Equals("LIST", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string text = sp.NextQuotedWord() ?? "*";

                        var gmos = discoveryform.GalacticMapping.AllObjects;
                        int count = 1;
                        foreach( var gmo in gmos)
                        {
                            if (gmo.IsDescriptiveName(text,true))     // wildcard match
                            {
                                string nprefix = prefix + (count++).ToStringInvariant() + "_";
                                DumpGMO(ap, nprefix, gmo);
                            }
                        }

                        ap[prefix + "MatchCount"] = (count - 1).ToStringInvariant();
                        ap[prefix + "TotalCount"] = gmos.Length.ToStringInvariant();

                    }
                    else
                    {
                        string name = sp.NextQuotedWord();

                        if (name != null)
                        {
                            if (cmdname.Equals("EXISTS", StringComparison.InvariantCultureIgnoreCase))
                            {
                                // exact match
                                EliteDangerousCore.GMO.GalacticMapObject gmo = discoveryform.GalacticMapping.FindDescriptiveName(name, false,false);    
                                ap[prefix + "Exists"] = (gmo != null).ToStringIntValue();
                                if ( gmo != null )
                                    DumpGMO(ap, prefix, gmo);
                            }
                            else
                            {
                                ap.ReportError("Unknown GMO command");
                            }
                        }
                        else
                            ap.ReportError("Missing name in command");
                    }
                }
                else
                    ap.ReportError("Missing GMO command");
            }
            else
                ap.ReportError(res);

            return true;
        }

        void DumpGMO(ActionProgramRun ap, string nprefix, EliteDangerousCore.GMO.GalacticMapObject g)
        {
            ap[nprefix + "Name"] = g.NameList;
            ap[nprefix + "Type"] = g.GalMapTypes[0].VisibleType != null ? g.GalMapTypes[0].VisibleType.ToString() : g.GalMapTypes[0].Group.ToString();
            ap[nprefix + "Search"] = g.StarSystem?.Name ?? "Unknown";
            ap[nprefix + "MapURL"] = g.GalMapUrl;
            ap[nprefix + "Description"] = g.Description;
            ap[nprefix + "Group"] = g.GalMapTypes[0].Group.ToString();

            if (g.Points != null)
            {
                for (int i = 0; i < g.Points.Count; i++)
                {
                    string p = nprefix + "Vertex_" + (i + 1).ToStringInvariant() + "_";
                    ap[p + "X"] = g.Points[i].X.ToStringInvariant("0.##");
                    ap[p + "Y"] = g.Points[i].Y.ToStringInvariant("0.##");
                    ap[p + "Z"] = g.Points[i].Z.ToStringInvariant("0.##");
                }
            }
        }
    }


}
