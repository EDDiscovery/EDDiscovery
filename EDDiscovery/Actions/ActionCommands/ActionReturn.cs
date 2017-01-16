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
        public ActionReturn(string n, ActionType t, string ud, int lu) : base(n, t, ud, lu)
        {
        }

        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            string promptValue = PromptSingleLine.ShowDialog(parent, "Return", UserData, "Configure Return Command");
            if (promptValue != null)
            {
                userdata = promptValue;
            }

            return (promptValue != null);
        }

        public bool ExecuteActionReturn(ActionProgramRun ap , out string retstr )
        {
            string res;
            if (ap.functions.ExpandString(UserData, ap.currentvars, out res) != ConditionLists.ExpandResult.Failed)       //Expand out.. and if no errors
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
