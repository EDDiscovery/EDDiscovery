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
                StringParser sp = new StringParser(res);

                string cmdname = sp.NextWord();

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
                        {
                            Forms.PopOutControl poc = ap.discoveryform.PopOuts;
                            Forms.PopOutControl.PopOuts? poi = poc.GetPopOutTypeByName(nextcmd);

                            if (poi.HasValue)
                            {
                                if (!ts.ChangeTo((int)poi.Value))
                                    ap.ReportError("Panel " + nextcmd + " cannot be used in Historytab");
                            }
                            else
                                ap.ReportError("Cannot find generic panel type name " + nextcmd + " in Historytab");
                        }
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
