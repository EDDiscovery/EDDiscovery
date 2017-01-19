using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    public class ActionPragma : Action
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            string promptValue = PromptSingleLine.ShowDialog(parent, "Pragma", UserData, "Configure Pragma Command");
            if (promptValue != null)
            {
                userdata = promptValue;
            }

            return (promptValue != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            StringParser p = new StringParser(userdata);

            string cmd;
            while( (cmd = p.NextWord() ) != null )
            {
                if (cmd.Equals("DumpVars", StringComparison.InvariantCultureIgnoreCase))
                {
                    string rest = p.NextQuotedWord();

                    if (rest != null && rest.Length > 0)
                    {
                        foreach (KeyValuePair<string, string> k in ap.currentvars.FilterVars(rest).values)
                        {
                            ap.discoveryform.LogLine(k.Key + "=" + k.Value);
                        }
                    }
                    else
                    {
                        ap.ReportError("Missing variable wildcard after Pragma DumpVars");
                        return true;
                    }
                }
                else if (cmd.Equals("Log", StringComparison.InvariantCultureIgnoreCase))
                {
                    string rest = p.NextQuotedWord();

                    if (rest != null )
                    {
                        ap.discoveryform.LogLine(rest);
                    }
                    else
                    {
                        ap.ReportError("Missing string after Pragma Log");
                        return true;
                    }
                }
            }

            return true;
        }
    }
}
