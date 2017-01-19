using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{ 
    public class ActionMaterialsCommoditiesBase : Action
    {
        protected bool commodities = false;

        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            string promptValue = PromptSingleLine.ShowDialog(parent, "JID of event" , UserData, (commodities) ? "Configure Commodities Command" : "Configure Material Command");
            if (promptValue != null)
            {
                userdata = promptValue;
            }

            return (promptValue != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string res;
            if (ap.functions.ExpandString(UserData, ap.currentvars, out res) != ConditionLists.ExpandResult.Failed)
            {
                StringParser sp = new StringParser(res);

                string prefix = (commodities) ? "C_" : "M_";
                string cmdname = sp.NextWord();

                if (cmdname != null && cmdname.Equals("PREFIX", StringComparison.InvariantCultureIgnoreCase))
                {
                    prefix = sp.NextWord();

                    if (prefix == null)
                    {
                        ap.ReportError("Missing name after Prefix in Materials/Commodity");
                        return true;
                    }

                    cmdname = sp.NextWord();
                }

                if (cmdname != null)
                {
                    long jid;
                    if (!long.TryParse(cmdname, out jid))
                    {
                        ap.ReportError("Non integer JID in Materials/Commodity");
                        return true;
                    }

                    int jidindex = ap.historylist.EntryOrder.FindIndex(x => x.Journalid == jid);

                    if (jidindex == -1)
                    {
                        ap.ReportError("JID does not exist in Materials/Commodity");
                        return true;
                    }

                    EDDiscovery2.DB.MaterialCommoditiesList mcl = ap.historylist.EntryOrder[jidindex].MaterialCommodity;
                    List<EDDiscovery2.DB.MaterialCommodities> list = mcl.Sort(commodities);

                    ap.currentvars[prefix + "Count"] = list.Count.ToString();
                    ap.currentvars[prefix + "IndexOf"] = ap.historylist.EntryOrder[jidindex].Indexno.ToString();

                    for ( int i = 0; i < list.Count; i++ )
                    {
                        string postfix = (i + 1).ToString();
                        ap.currentvars[prefix + "Name" + postfix] = list[i].name;
                        ap.currentvars[prefix + "Category" + postfix] = list[i].category;
                        ap.currentvars[prefix + "fdname" + postfix] = list[i].fdname;
                        ap.currentvars[prefix + "type" + postfix] = list[i].type;
                        ap.currentvars[prefix + "shortname" + postfix] = list[i].shortname;
                    }
                }
                else
                    ap.ReportError("Missing JID in Materials");
            }
            else
                ap.ReportError(res);

            return true;
        }
    }

    public class ActionMaterials : ActionMaterialsCommoditiesBase
    {
        public ActionMaterials()
        {
            commodities = false;
        }
    }

    public class ActionCommodities: ActionMaterialsCommoditiesBase
    {
        public ActionCommodities()
        {
            commodities = true;
        }
    }
}
