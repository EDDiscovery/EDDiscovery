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
        public override bool AllowDirectEditingOfUserData { get { return true; } }    // and allow editing?

        public override bool ConfigurationMenu(Form parent, EDDiscoveryForm discoveryform, List<string> eventvars)
        {
            string promptValue = PromptSingleLine.ShowDialog(parent, "Line to display", UserData.ReplaceEscapeControlChars(), "Configure Print Command" , true);
            if (promptValue != null)
            {
                userdata = promptValue.EscapeControlChars();
            }

            return (promptValue != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string res;
            if (ap.functions.ExpandString(UserData.ReplaceEscapeControlChars(), ap.currentvars, out res) != ConditionLists.ExpandResult.Failed)
            {
                ap.actioncontroller.LogLine(res);
                System.Diagnostics.Debug.WriteLine("PRINT " + res);
            }
            else
                ap.ReportError(res);

            return true;
        }

    }
   
}
