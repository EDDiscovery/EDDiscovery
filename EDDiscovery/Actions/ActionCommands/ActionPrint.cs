using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    public class ActionPrint : Action
    {
        public ActionPrint(string n, ActionType t, string ud, int lu) : base(n, t, ud, lu)
        {
        }

        public override bool AllowDirectEditingOfUserData { get { return true; } }    // and allow editing?

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            string promptValue = PromptSingleLine.ShowDialog(parent, "Line to display", UserData, "Configure Print Command");
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
                ap.discoveryform.LogLine(res);
                System.Diagnostics.Debug.WriteLine("PRINT " + res);
            }
            else
                ap.ReportError(res);

            return true;
        }

    }
   
}
