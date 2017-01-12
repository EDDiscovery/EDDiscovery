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
        public ActionPragma(string n, ActionType t, List<string> c, string ud, int lu) : base(n, t, c, ud, lu)
        {
        }

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
            string ud = UserData.Trim();

            if ( ud.StartsWith("DumpVars", StringComparison.InvariantCultureIgnoreCase))
            {
                string rest = ud.Substring(8).Trim();

                if (rest.Length > 0)
                {
                    do
                    {
                        int spc = rest.IndexOf(' ');
                        if (spc == -1)
                            spc = rest.Length;

                        string vname = rest.Substring(0, spc);

                        if (ap.currentvars.ContainsKey(vname))
                            ap.discoveryform.LogLine(vname + "=" + ap.currentvars[vname]);
                        else
                            ap.discoveryform.LogLine(vname + " Unkown");

                        rest = rest.Substring(spc).Trim();
                    } while (rest.Length > 0);
                }
                else
                {
                    ActionData.DumpVars(ap.currentvars);
                    foreach (KeyValuePair<string, string> k in ap.currentvars)
                        ap.discoveryform.LogLine(k.Key + "=" + k.Value);
                }
            }

            return true;
        }

    }
}
