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

        public override bool ConfigurationMenu(System.Windows.Forms.Form parent, ActionCoreController cp, List<BaseUtils.TypeHelpers.PropertyNameInfo> eventvars)
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
            if (ap.Functions.ExpandString(UserData, out res) != BaseUtils.Functions.ExpandResult.Failed)
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
                    StarScan scan = (ap.ActionController as ActionController).HistoryList.StarScan;

                    StarScan.SystemNode sn = scan.FindSystemSynchronous(new SystemClass(cmdname), edsm);

                    System.Globalization.CultureInfo ct = System.Globalization.CultureInfo.InvariantCulture;

                    ap[prefix + "EDSMLookup"] = EliteDangerousCore.EDSM.EDSMClass.HasBodyLookupOccurred(cmdname).ToStringIntValue();
                    ap[prefix + "EDSMNoData"] = EliteDangerousCore.EDSM.EDSMClass.HasNoDataBeenStoredOnBody(cmdname).ToStringIntValue();

                    if ( sn != null )
                    {
                        int starno = 1;
                        ap[prefix + "Stars"] = sn.StarNodes.Count.ToString(ct);

                        foreach (KeyValuePair<string, StarScan.ScanNode> scannode in sn.StarNodes)
                        {
                            DumpInfo(ap, scannode, prefix + "Star_" + starno.ToString(ct) , "_Planets");

                            int pcount = 1;

                            if (scannode.Value.Children != null)
                            {
                                foreach (KeyValuePair<string, StarScan.ScanNode> planetnodes in scannode.Value.Children)
                                {
                                    DumpInfo(ap, planetnodes, prefix + "Planet_" + starno.ToString(ct) + "_" + pcount.ToString(ct) , "_Moons");

                                    if (planetnodes.Value.Children != null)
                                    {
                                        int mcount = 1;
                                        foreach (KeyValuePair<string, StarScan.ScanNode> moonnodes in planetnodes.Value.Children)
                                        {
                                            DumpInfo(ap, moonnodes, prefix + "Moon_" + starno.ToString(ct) + "_" + pcount.ToString(ct) + "_" + mcount.ToString(ct) , "_Submoons");

                                            if (moonnodes.Value.Children != null)
                                            {
                                                int smcount = 1;
                                                foreach (KeyValuePair<string, StarScan.ScanNode> submoonnodes in moonnodes.Value.Children)
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
            ap[prefix + "_type"] = scannode.Value.NodeType.ToString();
            ap[prefix + "_assignedname"] = scannode.Value.OwnName;
            ap[prefix + "_assignedfullname"] = scannode.Value.FullName;
            ap[prefix + "_data"] = (sc != null) ? "1" : "0";
            ap[prefix + "_signals"] = scannode.Value.Signals != null ? EliteDangerousCore.JournalEvents.JournalSAASignalsFound.SignalList(scannode.Value.Signals, 0, ",", true) : "";

            if ( sc != null )
            {
                ap[prefix + "_isstar"] = sc.IsStar ? "1" : "0";
                ap[prefix + "_edsmbody"] = sc.IsEDSMBody ? "1" : "0";
                ap[prefix + "_bodyname"] = sc.BodyName;
                ap[prefix + "_bodydesignation"] = sc.BodyDesignationOrName;
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
                    EliteDangerousCore.JournalEvents.JournalScan.HabZones hz = sc.GetHabZones();
                    ap[prefix + "_habinner"] = hz != null ? hz.HabitableZoneInner.ToString("0.##") : "";
                    ap[prefix + "_habouter"] = hz != null ? hz.HabitableZoneOuter.ToString("0.##") : "";
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
                int totalchildren = (scannode.Value.Children != null) ? scannode.Value.Children.Count : 0;
                int totalbodies = (scannode.Value.Children != null) ? (from x in scannode.Value.Children where x.Value.NodeType == StarScan.ScanNodeType.body select x).Count() : 0;
                ap[prefix + subname] = totalchildren.ToStringInvariant();
                ap[prefix + subname + "_Only"] = totalbodies.ToStringInvariant();       // we do this, because children can be other than bodies..
            }
        }
    }

    class ActionStar : ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(System.Windows.Forms.Form parent, ActionCoreController cp, List<BaseUtils.TypeHelpers.PropertyNameInfo> eventvars)
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
            if (ap.Functions.ExpandString(UserData, out res) != BaseUtils.Functions.ExpandResult.Failed)
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

                bool edsm = false;
                if (cmdname != null && cmdname.Equals("EDSM", StringComparison.InvariantCultureIgnoreCase))
                {
                    edsm = true;
                    cmdname = sp.NextQuotedWord();
                }

                if (cmdname != null)
                {
                    EDDiscoveryForm f = ((ActionController)ap.ActionController).DiscoveryForm;
                    ISystem sc = SystemCache.FindSystem(cmdname, f.galacticMapping, edsm);        // find thru history, will include history entries

                    ap[prefix + "Found"] = sc != null ? "1" : "0";

                    if (sc != null)
                    {
                        BaseUtils.Variables vars = new BaseUtils.Variables();
                        ActionVars.SystemVars(vars, sc, prefix);
                        ap.Add(vars);
                        ActionVars.SystemVarsFurtherInfo(ap, (ap.ActionController as ActionController).HistoryList, sc, prefix);

                        string options = sp.NextWord();

                        if ( options != null)
                        {
                            if ( options.Equals("NEAREST", StringComparison.InvariantCultureIgnoreCase))
                            {
                                double mindist = sp.NextDouble(0.01);
                                double maxdist = sp.NextDouble(20.0);
                                int number = sp.NextInt(50);
                                bool cube = (sp.NextWord() ?? "Spherical").Equals("Cube", StringComparison.InvariantCultureIgnoreCase);  // spherical default for all but cube

                                StarDistanceComputer computer = new StarDistanceComputer();

                                apr = ap;
                                ret_prefix = prefix;

                                computer.CalculateClosestSystems(sc,
                                    (sys,list)=> (apr.ActionController as ActionController).DiscoveryForm.BeginInvoke(new Action(() => NewStarListComputed(sys, list))),
                                    (mindist>0) ? (number-1) : number,          // adds an implicit 1 on for centre star
                                    mindist, maxdist, !cube);

                                return false;   // go to sleep until value computed
                            }
                        }
                    }
                }
                else
                    ap.ReportError("Missing starname in Star");
            }
            else
                ap.ReportError(res);


            return true;
        }

        ActionProgramRun apr;
        string ret_prefix;

        private void NewStarListComputed(ISystem sys, BaseUtils.SortedListDoubleDuplicate<ISystem> list)      // In UI thread
        {
            System.Diagnostics.Debug.Assert(System.Windows.Forms.Application.MessageLoop);
            int i = 1;
            foreach( var s in list )
            {
                string p = ret_prefix + (i++).ToStringInvariant() + "_";
                ActionVars.SystemVars(apr.variables, s.Value, p);
                ActionVars.SystemVarsFurtherInfo(apr, (apr.ActionController as ActionController).HistoryList, s.Value, p);
                apr[p + "Dist"] = Math.Sqrt(s.Key).ToStringInvariant("0.##");
            }

            apr[ret_prefix + "Count"] = list.Count.ToStringInvariant();

            apr.ResumeAfterPause();
        }
    }
}
