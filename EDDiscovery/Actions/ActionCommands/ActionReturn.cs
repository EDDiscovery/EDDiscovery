using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{ 
    class ActionReturn : Action
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(Form parent, EDDiscoveryForm discoveryform, List<string> eventvars)
        {
            string promptValue = Forms.PromptSingleLine.ShowDialog(parent, discoveryform.theme, "Return", UserData.ReplaceEscapeControlChars(), 
                                "Configure Return Command" , true);

            if (promptValue != null)
                userdata = promptValue.EscapeControlChars();

            return (promptValue != null);
        }

        public bool ExecuteActionReturn(ActionProgramRun ap , out string retstr )
        {
            string res;
            if (ap.functions.ExpandString(UserData.ReplaceEscapeControlChars(), ap.currentvars, out res) != ConditionLists.ExpandResult.Failed)       //Expand out.. and if no errors
            {
                retstr = res;
                return true;
            }
            else
            {
                ap.ReportError(res);
                retstr = null;
                return false;
            }
        }
    }
}
