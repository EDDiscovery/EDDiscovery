using ActionLanguage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseUtils;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    class ActionMenuItem : ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }    // and allow editing?

        List<string> FromString(string input)
        {
            StringParser sp = new StringParser(input);
            List<string> s = sp.NextQuotedWordList();
            return (s != null && (s.Count == 1 || (s.Count >= 3 && s.Count <= 4))) ? s : null;
        }

        public override string VerifyActionCorrect()
        {
            return (FromString(userdata) != null) ? null : "MenuItem command line not in correct format";
        }

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<BaseUtils.TypeHelpers.PropertyNameInfo> eventvars)
        {
            List<string> l = FromString(userdata);
            List<string> r = ExtendedControls.PromptMultiLine.ShowDialog(parent, "Configure MenuInput Dialog", cp.Icon,
                            new string[] { "MenuName", "In Menu", "Menu Text", "Icon" }, l?.ToArray());
            if (r != null)
            {
                if (r[1].Length == 0 && r[2].Length == 0 && r[3].Length == 0)
                    userdata = r[0];
                else
                    userdata = r.ToStringCommaList();
            }

            return (r != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            List<string> ctrl = FromString(UserData);

            if (ctrl != null)
            {
                List<string> exp;

                if (ap.functions.ExpandStrings(ctrl, out exp) != BaseUtils.Functions.ExpandResult.Failed)
                {
                    if (exp.Count == 1)
                    {
                        ap["MenuPresent"] = (ap.actioncontroller as ActionController).DiscoveryForm.IsMenuItemInstalled(exp[0]) ? "1" : "0";
                    }
                    else
                    {
                        if (!(ap.actioncontroller as ActionController).DiscoveryForm.AddNewMenuItemToAddOns(exp[1], exp[2], (exp.Count >= 4) ? exp[3] : "None", exp[0], ap.actionfile.name))
                            ap.ReportError("MenuItem cannot add to menu, check menu");
                    }
                }
                else
                    ap.ReportError(exp[0]);
            }
            else
                ap.ReportError("MenuItem command line not in correct format");

            return true;
        }
    }
}
