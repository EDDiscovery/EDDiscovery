using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Actions
{
    public class ActionRem : Action
    {
        public ActionRem(string n, ActionType t, string ud, int lu) : base(n, t, ud, lu)
        {
        }

        public override bool AllowDirectEditingOfUserData { get { return true; } }
        public override bool ConfigurationMenuInUse { get { return false; } }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            return true;
        }
    }
}

