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
        public ActionLog(string n, List<string> c, string ud, int lu) : base(n, c, ud, lu)
        {
        }

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme )
        {
            return true;
        }

        public override bool AllowDirectEditingOfUserData { get { return true; } }    // and allow editing?

        public override bool ExecuteAction(HistoryEntry he, EDDiscoveryForm df, bool nopause)
        {
            df.LogLine(UserData);
            return true;
        }
    }

}
