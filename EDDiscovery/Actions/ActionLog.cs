using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    public class ActionLog : Action
    {
        public ActionLog(string n, ActionType t, List<string> c, string ud, int lu) : base(n, t, c, ud,lu)
        {
        }

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme )
        {
            return true;
        }

        public override bool AllowDirectEditingOfUserData { get { return true; } }    // and allow editing?

        public override bool ExecuteAction(ActionProgram ap)
        {
            ap.discoveryform.LogLine(UserData);
            return true;
        }
    }

}
