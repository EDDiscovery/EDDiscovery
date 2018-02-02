﻿/*
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
using EliteDangerousCore.DB;
using EliteDangerousCore;

namespace EDDiscovery.Actions
{
    class ActionScan : ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(System.Windows.Forms.Form parent, ActionCoreController cp, List<string> eventvars)
        {
            string promptValue = ExtendedControls.PromptSingleLine.ShowDialog(parent, "Scan system name", UserData, "Configure Scan Command" ,cp.Icon);
            if (promptValue != null)
            {
                userdata = promptValue;
            }

            return (promptValue != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string res;
            if (ap.functions.ExpandString(UserData, out res) != Conditions.ConditionFunctions.ExpandResult.Failed)
            {
                StringParser sp = new StringParser(res);

                string prefix = "S_";
                string cmdname = sp.NextQuotedWord();

                if (cmdname != null && cmdname.Equals("PREFIX", StringComparison.InvariantCultureIgnoreCase))
                {
                    prefix = sp.NextWord();

                    if (prefix == null)
                    {
                        ap.ReportError("Missing name after Prefix in Scan");
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
                    StarScan scan = (ap.actioncontroller as ActionController).HistoryList.starscan;
                    ISystem sc = SystemClassDB.GetSystem(cmdname);

                    if (sc == null)
                    {
                        sc = new SystemClass(cmdname);
                        sc.EDSMID = 0;
                    }

                    StarScan.SystemNode sn = scan.FindSystem(sc, edsm);

                    System.Globalization.CultureInfo ct = System.Globalization.CultureInfo.InvariantCulture;

                    if ( sn != null )
                    {
                        int starno = 1;
                        ap[prefix + "Stars"] = sn.starnodes.Count.ToString(ct);

                        foreach (KeyValuePair<string, StarScan.ScanNode> scannode in sn.starnodes)
                        {
                            DumpInfo(ap, scannode, prefix + "Star_" + starno.ToString(ct) , "_Planets");

                            int pcount = 1;

                            if (scannode.Value.children != null)
                            {
                                foreach (KeyValuePair<string, StarScan.ScanNode> planetnodes in scannode.Value.children)
                                {
                                    DumpInfo(ap, planetnodes, prefix + "Planet_" + starno.ToString(ct) + "_" + pcount.ToString(ct) , "_Moons");

                                    if (planetnodes.Value.children != null)
                                    {
                                        int mcount = 1;
                                        foreach (KeyValuePair<string, StarScan.ScanNode> moonnodes in planetnodes.Value.children)
                                        {
                                            DumpInfo(ap, moonnodes, prefix + "Moon_" + starno.ToString(ct) + "_" + pcount.ToString(ct) + "_" + mcount.ToString(ct) , "_Submoons");

                                            if (moonnodes.Value.children != null)
                                            {
                                                int smcount = 1;
                                                foreach (KeyValuePair<string, StarScan.ScanNode> submoonnodes in moonnodes.Value.children)
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
                        ap[prefix + "Stars"] = "0";
                    }
                }
                else
                    ap.ReportError("Missing starname in Scan");
            }
            else
                ap.ReportError(res);

            return true;
        }

        void DumpInfo( ActionProgramRun ap, KeyValuePair<string, EliteDangerousCore.StarScan.ScanNode> scannode, string prefix , string subname )
        {
            EliteDangerousCore.JournalEvents.JournalScan sc = scannode.Value.ScanData;

            ap[prefix] = scannode.Key;
            ap[prefix + "_type"] = scannode.Value.type.ToString();
            ap[prefix + "_assignedname"] = scannode.Value.ownname;
            ap[prefix + "_assignedfullname"] = scannode.Value.fullname;
            ap[prefix + "_data"] = (sc != null) ? "1" : "0";

            if ( sc != null )
            {
                ap[prefix + "_isstar"] = sc.IsStar ? "1" : "0";
                ap[prefix + "_edsmbody"] = sc.IsEDSMBody ? "1" : "0";
                ap[prefix + "_bodyname"] = sc.BodyName;
                ap[prefix + "_orbitalperiod"] = sc.nOrbitalPeriod.ToNANNullSafeString("0.###");
                ap[prefix + "_rotationperiod"] = sc.nRotationPeriod.ToNANNullSafeString("0.###");
                ap[prefix + "_surfacetemperature"] = sc.nSurfaceTemperature.ToNANNullSafeString("0.###");
                ap[prefix + "_distls"] = sc.DistanceFromArrivalLS.ToNANSafeString("0.###");

                if ( sc.IsStar )
                {
                    ap[prefix + "_startype"] = sc.StarType;
                    ap[prefix + "_startypetext"] = sc.StarTypeText;
                    ap[prefix + "_stellarmass"] = (sc.nStellarMass ?? 0).ToString("0.###");
                    ap[prefix + "_age"] = sc.nAge.ToNANNullSafeString("0.##");
                    ap[prefix + "_mag"] = sc.nAbsoluteMagnitude.ToNANNullSafeString("0");
                    ap[prefix + "_habinner"] = sc.HabitableZoneInner.ToNANNullSafeString("0.##");
                    ap[prefix + "_habouter"] = sc.HabitableZoneOuter.ToNANNullSafeString("0.##");
                }
                else
                {
                    ap[prefix + "_class"] = sc.PlanetClass.ToNullSafeString();
                    ap[prefix + "_landable"] = sc.IsLandable ? "Landable" : "Not Landable";
                    ap[prefix + "_atmosphere"] = sc.Atmosphere.ToNullSafeString();
                    ap[prefix + "_terraformstate"] = sc.TerraformState.ToNullSafeString();
                    ap[prefix + "_volcanism"] = sc.Volcanism.ToNullSafeString();
                    ap[prefix + "_gravity"] = sc.nSurfaceGravity.ToNANNullSafeString("0.###");
                    ap[prefix + "_pressure"] = sc.nSurfacePressure.ToNANNullSafeString("0.###");
                    ap[prefix + "_mass"] = sc.nMassEM.ToNANNullSafeString("0.###");
                    ap.AddDataOfType(sc.Materials, typeof(Dictionary<string,double>), prefix + "_Materials");
                }

                ap[prefix + "_text"] = sc.DisplayString();
                ap[prefix + "_value"] = sc.EstimatedValue.ToStringInvariant();
            }

            if ( subname != null )
            {
                int totalchildren = (scannode.Value.children != null) ? scannode.Value.children.Count : 0;
                ap[prefix + subname] = totalchildren.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
        }
    }

    class ActionStar : ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(System.Windows.Forms.Form parent, ActionCoreController cp, List<string> eventvars)
        {
            string promptValue = ExtendedControls.PromptSingleLine.ShowDialog(parent, "Star system name", UserData, "Configure Star Command" , cp.Icon);
            if (promptValue != null)
            {
                userdata = promptValue;
            }

            return (promptValue != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string res;
            if (ap.functions.ExpandString(UserData, out res) != Conditions.ConditionFunctions.ExpandResult.Failed)
            {
                StringParser sp = new StringParser(res);

                string prefix = "ST_";
                string cmdname = sp.NextQuotedWord();

                if (cmdname != null && cmdname.Equals("PREFIX", StringComparison.InvariantCultureIgnoreCase))
                {
                    prefix = sp.NextWord();

                    if (prefix == null)
                    {
                        ap.ReportError("Missing name after Prefix in Star");
                        return true;
                    }

                    cmdname = sp.NextQuotedWord();
                }

                if (cmdname != null)
                {
                    ISystem sc = SystemClassDB.GetSystem(cmdname);
                    ap[prefix + "Found"] = sc != null ? "1" : "0";

                    if (sc != null)
                    {
                        Conditions.ConditionVariables vars = new Conditions.ConditionVariables();
                        ActionVars.SystemVars(vars, sc, prefix);
                        ap.Add(vars);
                        ActionVars.SystemVarsFurtherInfo(ap, (ap.actioncontroller as ActionController).HistoryList, sc, prefix);
                    }
                }
                else
                    ap.ReportError("Missing starname in Star");
            }
            else
                ap.ReportError(res);

            return true;
        }
    }
}
