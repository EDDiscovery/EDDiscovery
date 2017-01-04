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
        public ActionIfElseBase(string n, List<string> c, string ud, int lu) : base(n, c, ud, lu)
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
        public ActionIf(string n, List<string> c, string ud, int lu) : base(n, c, ud, lu)
        {
        }

        public override bool IsControlStructureStart { get { return true; } }

        public override bool ExecuteAction(HistoryEntry he, EDDiscoveryForm df, bool nopause)
        {
            return true;
        }
    }

    public class ActionElseIf : ActionIfElseBase
    {
        public ActionElseIf(string n, List<string> c, string ud, int lu) : base(n, c, ud, lu)
        {
        }

        public override bool IsControlElse { get { return true; } }

        public override bool ExecuteAction(HistoryEntry he, EDDiscoveryForm df, bool nopause)
        {
            return true;
        }
    }

    public class ActionElse : Action
    {
        public ActionElse(string n, List<string> c, string ud, int lu) : base(n, c, ud,lu)
        {
        }

        public override bool IsControlElse { get { return true; } }

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme)
        {
            return (true);
        }

        public override bool ExecuteAction(HistoryEntry he , EDDiscoveryForm df, bool nopause )
        {
            return true;
        }
    }

}
