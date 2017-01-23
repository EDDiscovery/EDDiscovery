using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    public class ActionPopout : Action
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            string promptValue = PromptSingleLine.ShowDialog(parent, "Popout command", UserData, "Configure Popout Command");
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
                string prefix = "P_";

                string cmdname = sp.NextWord();

                if (cmdname != null && cmdname.Equals("PREFIX", StringComparison.InvariantCultureIgnoreCase))
                {
                    prefix = sp.NextWord();

                    if (prefix == null)
                    {
                        ap.ReportError("Missing name after Prefix in Event");
                        return true;
                    }

                    cmdname = sp.NextWord();
                }

                TravelHistoryControl thc = ap.discoveryform.TravelControl;
                Forms.UserControlFormList ucfl = thc.usercontrolsforms;

                ap.currentvars[prefix + "Count"] = ucfl.Count.ToString();

                for (int i = 0; i < ucfl.Count; i++)
                    ap.currentvars[prefix + i.ToString()] = ucfl[i].Name;

                if ( cmdname != null )
                {
                    Forms.UserControlForm ucf = null;

                    for (int i = 0; i < ucfl.Count; i++)
                    {
                        if (ucfl[i].Name.Equals(cmdname, StringComparison.InvariantCultureIgnoreCase))
                        {
                            ucf = ucfl[i];
                            break;
                        }
                    }

                    string opname = sp.NextWord();

                }
                ap.ReportError("Unknown command " + cmdname + " in Event");
            }
            else
                ap.ReportError(res);

            return true;
        }

        void ReportEntry(ActionProgramRun ap, List<HistoryEntry> hl, int pos, string prefix)
        {
            if (hl != null && pos >= 0 && pos < hl.Count)     // if within range.. (1 based)
            {
                try
                {
                    ConditionVariables values = new ConditionVariables();
                    values.GetJSONFieldNamesAndValues(hl[pos].journalEntry.EventDataString, prefix + "JS_");
                    ActionVars.HistoryEventVars(values, hl[pos], prefix);
                    ap.currentvars.Add(values);
                    ap.currentvars[prefix + "Count"] = hl.Count.ToString();     // give a count of matches
                    return;
                }
                catch
                {
                }
            }

            ap.currentvars[prefix + "JID"] = "0";
            ap.currentvars[prefix + "Count"] = "0";
        }

    }
}
