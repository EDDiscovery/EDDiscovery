using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    public class ActionIfElseBase : Action
    {
        public ActionIfElseBase(string n, ActionType type, List<string> c, string ud, int lu) : base(n, type, c, ud, lu)
        {
        }

        public List<string> variables;

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme)
        {
            EDDiscovery2.JSONFiltersForm frm = new EDDiscovery2.JSONFiltersForm();

            JSONFilter jf = new JSONFilter();
            if (UserData.Length > 0)
                jf.FromJSON(UserData);

            frm.InitCondition("Define condition", variables, theme, jf);

            frm.TopMost = parent.FindForm().TopMost;
            if (frm.ShowDialog(parent.FindForm()) == DialogResult.OK)
            {
                userdata = frm.result.GetJSON();
            }

            return (true);
        }

        public override string DisplayedUserData
        {
            get
            {
                if (UserData.Length > 0)
                {
                    JSONFilter jf = new JSONFilter();
                    jf.FromJSON(UserData);
                    return jf.GetString();
                }
                else
                    return "";
            }
        }
    }

    public class ActionIf : ActionIfElseBase
    {
        JSONFilter condition;

        public ActionIf(string n, ActionType type, List<string> c, string ud, int lu) : base(n, type, c, ud, lu)
        {
        }

        public override bool ExecuteAction(ActionProgram ap)
        {
            if (condition == null)
            {
                condition = new JSONFilter();
                if (!condition.FromJSON(UserData))
                {
                    ap.ReportError("IF condition is not correctly formed");
                    return true;
                }
            }

            //bool? condres = condition.Check()
            bool condres = false;
            ap.PushIf(condres);
            return true;
        }
    }

    public class ActionElseIf : ActionIfElseBase
    {
        public ActionElseIf(string n, ActionType type, List<string> c, string ud, int lu) : base(n, type, c, ud, lu)
        {
        }

        public override bool ExecuteAction(ActionProgram ap)
        {
            if (ap.IsLevelOff)       // if not executing, check condition
            {
                bool condres = true;
                ap.ElseIf(condres); // set ON or off
            }
            else
            {                       
                ap.ElseIfOff();     // was not off, so now off for good
            }

            return true;
        }
    }

    public class ActionElse : Action
    {
        public ActionElse(string n, ActionType type, List<string> c, string ud, int lu) : base(n, type, c, ud, lu)
        {
        }

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme)
        {
            return (true);
        }

        public override bool ConfigurationMenuInUse { get { return false; } }

        public override bool ExecuteAction(ActionProgram ap)
        {
            if (ap.IsLevelOff)       // if not executing, we can turn on
                ap.ElseIf(true);        // Set ON 
            else
                ap.ElseIfOff();     // was not off, so now off for good

            return true;
        }
    }

}
