/*
 * Copyright 2017-2024 EDDiscovery development team
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
 */


using ActionLanguage;
using BaseUtils;
using EliteDangerousCore;
using EliteDangerousCore.StarScan2;
using System;
using System.Collections.Generic;
using System.Linq;

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
                if (cmdname != null && cmdname.Equals("EDSM", StringComparison.InvariantCultureIgnoreCase))
                {
                    edsm = true;
                    cmdname = sp.NextQuotedWord();
                }

                bool spansh = false;
                if (cmdname != null && cmdname.Equals("SPANSH", StringComparison.InvariantCultureIgnoreCase))
                {
                    spansh = true;
                    cmdname = sp.NextQuotedWord();
                }

                var lookup = edsm ? (spansh ? EliteDangerousCore.WebExternalDataLookup.SpanshThenEDSM : WebExternalDataLookup.EDSM) :
                    spansh ? EliteDangerousCore.WebExternalDataLookup.Spansh : EliteDangerousCore.WebExternalDataLookup.None;

                if (cmdname != null)
                {
                    string bodyname = sp.NextQuotedWord();      // may be null

                    //System.Diagnostics.Debug.WriteLine($"Scan Star System {cmdname} body `{bodyname}`");

                    var scan = (ap.ActionController as ActionController).HistoryList.StarScan2;

                    long? systemaddr = cmdname.InvariantParseLongNull();    // if it converts to long, then its a system address.

                    var system = systemaddr > 0 ? new SystemClass(systemaddr.Value) : new SystemClass(cmdname);      // by name or by system address

                    var sn = scan.FindSystemSynchronous(system, lookup);

                    System.Globalization.CultureInfo ct = System.Globalization.CultureInfo.InvariantCulture;

                    ap[prefix + "EDSMLookup"] = EliteDangerousCore.EDSM.EDSMClass.HasBodyLookupOccurred(system).ToStringIntValue();
                    ap[prefix + "EDSMNoData"] = EliteDangerousCore.EDSM.EDSMClass.HasNoDataBeenStoredOnBody(system).ToStringIntValue();
                    ap[prefix + "SpanshLookup"] = EliteDangerousCore.Spansh.SpanshClass.HasBodyLookupOccurred(system).ToStringIntValue();
                    ap[prefix + "SpanshNoData"] = EliteDangerousCore.Spansh.SpanshClass.HasNoDataBeenStoredOnBody(system).ToStringIntValue();

                    if ( sn != null )
                    {
                        if (bodyname.HasChars())
                        {
                            var bn = sn.FindCanonicalBodyNameWithWithoutSystem(bodyname);
                            ap[prefix + "BodyFound"] = bn != null ? "1" : "0";
                            if (bn != null)
                            {
                                var bnp = sn.BodiesNoBarycentres();     // we compute the body and its children at this point
                                DumpInfo(ap, bnp, prefix + "Body", "_Moons");
                            }
                        }
                        else
                        {
                            var bpnobc = sn.BodiesNoBarycentres();
                            DumpNode(ap, bpnobc, prefix, "" , 0);
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

        // recurse thru the body ptr tree printing info, and use the old naming system to do this

        static string[] names = new string[] { "Star", "Planet", "Moon", "SubMoon", "L4", "L5", "L6", "L7","L8","L9","L10","L11","L12" };
        void DumpNode(ActionProgramRun ap, NodePtr bn, string prefix, string bodystring, int level)
        {
            int num = 1;
            foreach( var cbn in bn.ChildBodies)
            {
                DumpInfo(ap, cbn, prefix + names[level] + bodystring + "_" + num.ToStringInvariant(), "_" + names[level + 1] + "s");
                string subbodystring = bodystring + "_" + num.ToStringInvariant();
                DumpNode(ap, cbn, prefix, subbodystring, level + 1);
                num++;
            }
        }

        void DumpInfo( ActionProgramRun ap, NodePtr bnp, string storename , string childreninfoname )
        {
            BodyNode scannode = bnp.BodyNode;
            EliteDangerousCore.JournalEvents.JournalScan sc = scannode.Scan;

            ap[storename] = scannode.OwnName;
            ap[storename + "_type"] = scannode.BodyType.ToString();
            ap[storename + "_assignedname"] = scannode.OwnName;
            ap[storename + "_assignedfullname"] = scannode.CanonicalName ?? "";
            ap[storename + "_data"] = (sc != null) ? "1" : "0";

            if (childreninfoname != null)
            {
                int totalchildren = bnp.ChildBodies.Count;
                int totalbodies = bnp.ChildBodies.Where(x => x.BodyNode.IsStarOrPlanet).Count();
                ap[storename + childreninfoname] = totalchildren.ToStringInvariant();
                ap[storename + childreninfoname + "_Only"] = totalbodies.ToStringInvariant();       // we do this, because children can be other than stars or planets
            }

#if true

            ap[storename + "_signals"] = scannode.Signals != null ? EliteDangerousCore.JournalEvents.JournalSAASignalsFound.SignalListString(scannode.Signals, 0,false, true) : "";
            ap[storename + "_genuses"] = scannode.Genuses != null ? EliteDangerousCore.JournalEvents.JournalSAASignalsFound.GenusListString(scannode.Genuses, 0, false, true) : "";

            if ( sc != null )
            {
                ap[storename + "_isstar"] = sc.IsStar ? "1" : "0";
                ap[storename + "_edsmbody"] = sc.DataSource == SystemSource.FromEDSM ? "1" : "0";
                ap[storename + "_source"] = sc.DataSource.ToString();   
                ap[storename + "_bodyname"] = sc.BodyName;
                ap[storename + "_bodydesignation"] = sc.BodyName;
                ap[storename + "_orbitalperiod"] = sc.nOrbitalPeriod.ToStringInvariantNAN("R");
                ap[storename + "_rotationperiod"] = sc.nRotationPeriod.ToStringInvariantNAN("R");
                ap[storename + "_surfacetemperature"] = sc.nSurfaceTemperature.ToStringInvariantNAN("R");
                ap[storename + "_distls"] = sc.DistanceFromArrivalLS.ToStringInvariantNAN("R");

                if ( sc.IsStar )
                {
                    ap[storename + "_startype"] = sc.StarType;
                    ap[storename + "_startypeid"] = sc.StarTypeID.ToString();
                    ap[storename + "_startypetext"] = sc.StarTypeText;
                    ap[storename + "_stellarmass"] = (sc.nStellarMass ?? 0).ToStringInvariantNAN("R");
                    ap[storename + "_age"] = sc.nAge.ToStringInvariantNAN("R");
                    ap[storename + "_mag"] = sc.nAbsoluteMagnitude.ToStringInvariantNAN("R");
                    HabZones hz = sc.GetHabZones();
                    ap[storename + "_habinner"] = hz != null ? hz.HabitableZoneInner.ToStringInvariantNAN("R") : "";
                    ap[storename + "_habouter"] = hz != null ? hz.HabitableZoneOuter.ToStringInvariantNAN("R") : "";
                }
                else
                {
                    ap[storename + "_class"] = sc.PlanetClass.ToNullSafeString();
                    ap[storename + "_typeid"] = sc.PlanetTypeID.ToNullSafeString();
                    ap[storename + "_landable"] = sc.IsLandable ? "Landable" : "Not Landable";
                    ap[storename + "_atmosphere"] = sc.Atmosphere.ToNullSafeString();
                    ap[storename + "_translatedatmosphere"] = sc.AtmosphereTranslated.ToNullSafeString();
                    ap[storename + "_terraformstate"] = sc.TerraformState.ToNullSafeString();
                    ap[storename + "_volcanism"] = sc.Volcanism.ToNullSafeString();
                    ap[storename + "_translatedvolcanism"] = sc.VolcanismTranslated.ToNullSafeString();
                    ap[storename + "_gravity"] = sc.nSurfaceGravity.ToStringInvariantNAN("R");
                    ap[storename + "_gravityg"] = sc.nSurfaceGravityG.ToStringInvariantNAN("R");
                    ap[storename + "_pressure"] = sc.nSurfacePressure.ToStringInvariantNAN("R");
                    ap[storename + "_mass"] = sc.nMassEM.ToStringInvariantNAN("R");
                    ap.AddDataOfType(sc.Materials, typeof(Dictionary<string,double>), storename + "_Materials");
                }

                ap[storename + "_text"] = sc.DisplayText();
                ap[storename + "_value"] = sc.EstimatedValue.ToStringInvariant();
            }
#endif
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

                bool spansh = false;
                if (cmdname != null && cmdname.Equals("SPANSH", StringComparison.InvariantCultureIgnoreCase))
                {
                    spansh = true;
                    cmdname = sp.NextQuotedWord();
                }

                var lookup = edsm ? (spansh ? EliteDangerousCore.WebExternalDataLookup.SpanshThenEDSM : WebExternalDataLookup.EDSM) :
                    spansh ? EliteDangerousCore.WebExternalDataLookup.Spansh : EliteDangerousCore.WebExternalDataLookup.None;

                if (cmdname != null)
                {
                    EDDiscoveryForm f = ((ActionController)ap.ActionController).DiscoveryForm;
                    ISystem sc = SystemCache.FindSystem(cmdname, f.GalacticMapping, lookup);        // find thru history, will include history entries

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
