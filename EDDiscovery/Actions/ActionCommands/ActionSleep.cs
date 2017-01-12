using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace EDDiscovery.Actions
{
    class ActionSleep : Action
    {
        public ActionSleep(string n, ActionType t, List<string> c, string ud, int lu) : base(n, t, c, ud, lu)
        {
        }

        Timer t;
        ActionProgramRun apr;

        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            string promptValue = PromptSingleLine.ShowDialog(parent, "Sleep time in ms:", UserData, "Set Sleep timeout");

            if (promptValue != null)
            {
                userdata = promptValue;
                return true;
            }

            return (promptValue != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string delay;
            if (ap.functions.ExpandString(userdata, ap.currentvars, out delay) == EDDiscovery.ConditionLists.ExpandResult.Failed)       //Expand out.. and if no errors
            {
                ap.ReportError(delay);
                return true;
            }

            int i;
            if (int.TryParse(delay, out i) && i >= 0)
            {
                if (t == null)
                {
                    t = new Timer();
                    t.Tick += T_Tick;
                }

                System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Doze for " + i);
                apr = ap;
                t.Interval = i;
                t.Start();
                return false;
            }
            else
                ap.ReportError("Sleep requires a positive integer for time in ms");

            return true;
        }

        private void T_Tick(object sender, EventArgs e)
        {
            t.Stop();
            System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Resume after sleep");
            apr.ResumeAfterPause();
        }
    }
}
