using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    class ActionHistoryTab : Action
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            string promptValue = PromptSingleLine.ShowDialog(parent, "HistoryTab command", UserData, "Configure HistoryTab Command");
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
                HistoryList hl = ap.historylist;
                StringParser sp = new StringParser(res);
                string prefix = "HT_";

                string cmdname = sp.NextWord();

                if (cmdname != null && cmdname.Equals("PREFIX", StringComparison.InvariantCultureIgnoreCase))
                {
                    prefix = sp.NextWord();

                    if (prefix == null)
                    {
                        ap.ReportError("Missing name after Prefix in Historytab");
                        return true;
                    }

                    cmdname = sp.NextWord();
                }

                if (cmdname == null)
                {
                    ap.ReportError("Missing panel name in Historytab");
                }
                else
                {
                    ExtendedControls.TabStrip ts = ap.discoveryform.TravelControl.GetTabStrip(cmdname);

                    if (ts != null)
                    {
                        string nextcmd = sp.NextWord(" ", true);

                        if (nextcmd == null)
                        {
                            ap.ReportError("Missing command after panel name in Historytab");
                        }
                        else if (nextcmd.Equals("toggle"))
                        {
                            ts.Toggle();
                        }
                        else
                            ap.ReportError("Unknown command " + cmdname + " after panel name in Historytab");
                    }
                    else
                    {
                        ap.ReportError("Unknown panel name " + cmdname + " in Historytab");
                    }
                }
            }
            else
                ap.ReportError(res);

            return true;
        }

    }


}
