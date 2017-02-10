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
            string promptValue = PromptSingleLine.ShowDialog(parent, "JID of event", UserData, "Configure Ledger Command");
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
            ap.currentvars[prefix + "_text"] = (sc != null) ? sc.DisplayString(true):"";
            ap.currentvars[prefix + "_type"] = (sc != null) ? ((sc.IsStar) ? "Star" : "Planet"):"";
            ap.currentvars[prefix + "_landable"] = (sc != null) ? (sc.IsLandable ? "Landable" : "Not Landable"):"";
            ap.currentvars[prefix + "_bodyname"] = (sc != null) ? sc.BodyName : "";
            ap.currentvars[prefix + "_startype"] = (sc != null) ? (sc.StarType ?? "N/A") : "";

            if (scannode.Value.children != null && subname != null)
                ap.currentvars[prefix + subname] = scannode.Value.children.Count.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
