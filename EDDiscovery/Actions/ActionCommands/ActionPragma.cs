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
        public ActionPragma(string n, ActionType t, string ud, int lu) : base(n, t,  ud, lu)
        {
        }

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
                        DumpVars(ap, ap.currentvars.FilterVars(rest));
                    }
                    else
                    {
                        ap.ReportError("Missing variable wildcard after Pragma DumpVars");
                        return true;
                    }
                }
            }

            return true;
        }


        private void DumpVars(ActionProgramRun ap, ConditionVariables fv )
        {
            foreach (KeyValuePair<string, string> k in fv.values)
            {
                ap.discoveryform.LogLine(k.Key + "=" + k.Value);
                System.Diagnostics.Debug.WriteLine( "dumpvars " + k.Key + "=" + k.Value);
            }
        }
    }
}
