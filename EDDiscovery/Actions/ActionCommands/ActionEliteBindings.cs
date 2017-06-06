using EDDiscovery.EliteDangerous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    public class ActionEliteBindings : Action
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(Form parent, EDDiscoveryForm discoveryform, List<string> eventvars)
        {
            string promptValue = Forms.PromptSingleLine.ShowDialog(parent, "EliteBindings commands", UserData, "Configure EliteBindings");
            if (promptValue != null)
            {
                userdata = promptValue;
            }

            return (promptValue != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string res;
            if (ap.functions.ExpandString(UserData, out res) != ConditionFunctions.ExpandResult.Failed)
            {
                StringParser sp = new StringParser(res);

                string prefix = "EB_";
                string cmdname = sp.NextQuotedWord();

                if (cmdname != null && cmdname.Equals("PREFIX", StringComparison.InvariantCultureIgnoreCase))
                {
                    prefix = sp.NextWord();

                    if (prefix == null)
                    {
                        ap.ReportError("Missing name after Prefix in EliteBindings");
                        return true;
                    }

                    cmdname = sp.NextQuotedWord();
                }

                BindingsFile bf = ap.actioncontroller.DiscoveryForm.FrontierBindings;

                while ( cmdname != null )
                {
                    bool partial = false;
                    int i = cmdname.IndexOf("*");
                    if (i>=0)
                    {
                        cmdname = cmdname.Substring(0, i);
                        partial = true;
                    }

                    List<BindingsFile.Assignment> matches = bf.Find(cmdname, partial);

                    if (matches.Count > 0)
                    {
                        foreach (BindingsFile.Assignment a in matches)
                        {
                            ap[prefix + cmdname + "_Assignment"] = a.assignedfunc;
                            ap[prefix + cmdname + "_Keys"] = string.Join(",", (from BindingsFile.DeviceKeyPair k in a.keys select k.Key).ToList());
                            ap[prefix + cmdname + "_Devices"] = string.Join(",", (from BindingsFile.DeviceKeyPair k in a.keys select k.Device.Name).ToList());
                        }
                    }

                    Dictionary<string, string> values = bf.BindingValue(cmdname, partial);
                    foreach(string k in values.Keys)
                    {
                        ap[prefix + k] = values[k];
                    }


                    cmdname = sp.NextQuotedWord();
                }

            }
            else
                ap.ReportError(res);

            return true;
        }
    }
}
