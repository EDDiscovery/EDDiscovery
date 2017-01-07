using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    public class ActionSetLetBase : Action
    {
        public ActionSetLetBase(string n, ActionType t, List<string> c, string ud, int lu) : base(n, t, c, ud, lu)
        {
        }

        public string flagVar = "Var";

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            string vn = GetFlagAuxData(flagVar);

            Tuple<string, string> promptValue = PromptDoubleLine.ShowDialog(parent, "Variable:", "Value:", vn, UserData, "Set Variable");

            if (promptValue != null)
            {
                SetFlag(flagVar, true, promptValue.Item1);
                userdata = promptValue.Item2;
            }

            return (promptValue != null);
        }

        public override string DisplayedUserData
        {
            get
            {
                string vn = GetFlagAuxData(flagVar);
                return vn + "=" + UserData;
            }
        }

    }

    public class ActionSet : ActionSetLetBase
    {
        public ActionSet(string n, ActionType t, List<string> c, string ud, int lu) : base(n, t, c, ud, lu)
        {
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string vn = GetFlagAuxData(flagVar);

            if (vn != null && vn.Length > 0)
            {
                string value = UserData;
                ap.currentvars[vn] = value;
            }
            else
                ap.ReportError("Set no variable name given");

            return true;
        }

    }

    public class ActionLet : ActionSetLetBase
    {
        public ActionLet(string n, ActionType t, List<string> c, string ud, int lu) : base(n, t, c, ud, lu)
        {
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string vn = GetFlagAuxData(flagVar);
            string value = UserData;

            if (vn != null && vn.Length > 0)
            {
                System.Data.DataTable dt = new System.Data.DataTable();

                string res;

                try
                {
                    var v = dt.Compute(value, "");
                    Type t = v.GetType();
                    System.Diagnostics.Debug.WriteLine("Type return is " + t.ToString());
                    if (v is double)
                        res = v.ToString();
                    else if (v is System.Decimal)
                        res = v.ToString();
                    else if (v is int)
                        res = v.ToString();
                    else
                        res = "NAN";

                    ap.currentvars[vn] = res;
                }
                catch
                {
                    ap.ReportError("LET expression does not evaluate");
                }
            }
            else
                ap.ReportError("Let no variable name given");

            return true;
        }

    }
}
