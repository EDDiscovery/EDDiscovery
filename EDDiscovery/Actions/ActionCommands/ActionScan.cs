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

namespace EDDiscovery.Actions
{
    class ActionScan : Action
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(System.Windows.Forms.Form parent, EDDiscoveryForm discoveryform, List<string> eventvars)
        {
            string promptValue = Forms.PromptSingleLine.ShowDialog(parent, discoveryform.theme, "Scan system name", UserData, "Configure Scan Command");
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

                string prefix = "S_";
                string cmdname = sp.NextQuotedWord();

                if (cmdname != null && cmdname.Equals("PREFIX", StringComparison.InvariantCultureIgnoreCase))
                {
                    prefix = sp.NextWord();

                    if (prefix == null)
                    {
                        ap.ReportError("Missing name after Prefix in Ledger");
                        return true;
                    }

                    cmdname = sp.NextQuotedWord();
                }

                bool edsm = false;
                if ( cmdname != null && cmdname.Equals("EDSM",StringComparison.InvariantCultureIgnoreCase))
                {
                    edsm = true;
                    cmdname = sp.NextQuotedWord();
                }

                if (cmdname != null)
                {
                    EliteDangerous.StarScan scan = ap.actioncontroller.HistoryList.starscan;
                    DB.SystemClass sc = DB.SystemClass.GetSystem(cmdname);

                    if (sc == null)
                    {
                        sc = new DB.SystemClass(cmdname);
                        sc.id_edsm = 0;
                    }

                    EliteDangerous.StarScan.SystemNode sn = scan.FindSystem(sc);

                    if (edsm)       // if check edsm..
                    {
                        sn = scan.UpdateFromEDSM(sn, sc);   // do an edsm check
                    }

                    System.Globalization.CultureInfo ct = System.Globalization.CultureInfo.InvariantCulture;

                    if ( sn != null )
                    {
                        int starno = 1;
                        ap.currentvars[prefix + "Stars"] = sn.starnodes.Count.ToString(ct);

                        foreach (KeyValuePair<string, EliteDangerous.StarScan.ScanNode> scannode in sn.starnodes)
                        {
                            DumpInfo(ap, scannode, prefix + "Star_" + starno.ToString(ct) , "_Planets");

                            int pcount = 1;

                            if (scannode.Value.children != null)
                            {
                                foreach (KeyValuePair<string, EliteDangerous.StarScan.ScanNode> planetnodes in scannode.Value.children)
                                {
                                    DumpInfo(ap, planetnodes, prefix + "Planet_" + starno.ToString(ct) + "_" + pcount.ToString(ct) , "_Moons");

                                    if (planetnodes.Value.children != null)
                                    {
                                        int mcount = 1;
                                        foreach (KeyValuePair<string, EliteDangerous.StarScan.ScanNode> moonnodes in planetnodes.Value.children)
                                        {
                                            DumpInfo(ap, moonnodes, prefix + "Moon_" + starno.ToString(ct) + "_" + pcount.ToString(ct) + "_" + mcount.ToString(ct) , "_Submoons");

                                            if (moonnodes.Value.children != null)
                                            {
                                                int smcount = 1;
                                                foreach (KeyValuePair<string, EliteDangerous.StarScan.ScanNode> submoonnodes in moonnodes.Value.children)
                                                {
                                                    DumpInfo(ap, submoonnodes, prefix + "SubMoon_" + starno.ToString(ct) + "_" + pcount.ToString(ct) + "_" + mcount.ToString(ct) + "_" + smcount.ToString(ct),null);
                                                    smcount++;
                                                }
                                            }

                                            mcount++;
                                        }
                                    }

                                    pcount++;
                                }
                            }

                            starno++;
                        }
                    }
                    else
                    {
                        ap.currentvars[prefix + "Stars"] = "0";
                    }
                }
                else
                    ap.ReportError("Missing starname in Scan");
            }
            else
                ap.ReportError(res);

            return true;
        }

        void DumpInfo( ActionProgramRun ap, KeyValuePair<string, EliteDangerous.StarScan.ScanNode> scannode, string prefix , string subname )
        {
            EliteDangerous.JournalEvents.JournalScan sc = scannode.Value.ScanData;

            ap.currentvars[prefix] = scannode.Key;
            ap.currentvars[prefix + "_type"] = scannode.Value.type.ToString();
            ap.currentvars[prefix + "_assignedname"] = scannode.Value.ownname;
            ap.currentvars[prefix + "_assignedfullname"] = scannode.Value.fullname;
            ap.currentvars[prefix + "_data"] = (sc != null) ? "1" : "0";

            if ( sc != null )
            {
                ap.currentvars[prefix + "_text"] = sc.DisplayString(true);
                ap.currentvars[prefix + "_edsmbody"] = (sc.IsEDSMBody) ? "1" : "0";
                ap.currentvars[prefix + "_bodyname"] = sc.BodyName;

                if ( sc.IsStar )
                {
                    ap.currentvars[prefix + "_startype"] = sc.StarType;
                    ap.currentvars[prefix + "_startypetext"] = sc.StarTypeText;
                    ap.currentvars[prefix + "_stellarmass"] = (sc.nStellarMass??0).ToString("0.###");
                }
                else
                {
                    ap.currentvars[prefix + "_landable"] = sc.IsLandable ? "Landable" : "Not Landable";
                    ap.currentvars[prefix + "_atmosphere"] = sc.Atmosphere.ToNullSafeString();
                    ap.currentvars[prefix + "_class"] = sc.PlanetClass.ToNullSafeString().Replace("II", " 2").Replace("IV", " 4");
                }
            }

            if ( subname != null )
            {
                int totalchildren = (scannode.Value.children != null) ? scannode.Value.children.Count : 0;
                ap.currentvars[prefix + subname] = totalchildren.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
        }
    }
}
