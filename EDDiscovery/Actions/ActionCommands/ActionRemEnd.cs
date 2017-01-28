using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Actions
{
    public class ActionRem : Action
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }
        public override bool ConfigurationMenuInUse { get { return false; } }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            return true;
        }
    }

    public class ActionEnd : Action
    {
        public override bool ConfigurationMenuInUse { get { return false; } }
        public override string DisplayedUserData { get { return null; } }        // null if you dont' want to display

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            ap.TerminateCurrentProgram();
            return true;
        }
    }
}

