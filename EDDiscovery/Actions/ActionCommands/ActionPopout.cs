using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    public class ActionPopout : Action
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            string promptValue = PromptSingleLine.ShowDialog(parent, "Popout command", UserData, "Configure Popout Command");
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
                HistoryList hl = ap.historylist;
                StringParser sp = new StringParser(res);
                string prefix = "P_";

                string cmdname = sp.NextWord();

                if (cmdname != null && cmdname.Equals("PREFIX", StringComparison.InvariantCultureIgnoreCase))
                {
                    prefix = sp.NextWord();

                    if (prefix == null)
                    {
                        ap.ReportError("Missing name after Prefix in Event");
                        return true;
                    }

                    cmdname = sp.NextWord();
                }

                TravelHistoryControl thc = ap.discoveryform.TravelControl;
                Forms.PopOutControl poc = ap.discoveryform.PopOuts;

                if (cmdname == null)
                {
                    ap.currentvars[prefix + "Count"] = poc.Count.ToString();
                    for (int i = 0; i < poc.Count; i++)
                        ap.currentvars[prefix + i.ToString()] = poc[i].Name;
                }
                else
                {
                    Forms.UserControlForm ucf = poc.Get(cmdname);

                    string nextcmd = sp.NextWord();

                    if (nextcmd == null)
                    {
                        ap.ReportError("Missing command after panel name in Panel");
                    }
                    else if (ucf != null)        // found a panel with the name
                    {
                        if (nextcmd.Equals("exists", StringComparison.InvariantCultureIgnoreCase))
                            ap.currentvars[prefix + "Exists"] = "1";
                        else if (nextcmd.Equals("Toggle", StringComparison.InvariantCultureIgnoreCase) || nextcmd.Equals("OFF", StringComparison.InvariantCultureIgnoreCase))
                            ucf.Close();
                        else if (nextcmd.Equals("transparent", StringComparison.InvariantCultureIgnoreCase))
                            ucf.SetTransparency(true);
                        else if (nextcmd.Equals("opaque", StringComparison.InvariantCultureIgnoreCase))
                            ucf.SetTransparency(false);
                        else if (nextcmd.Equals("title", StringComparison.InvariantCultureIgnoreCase))
                            ucf.SetShowInTaskBar(true);
                        else if (nextcmd.Equals("notitle", StringComparison.InvariantCultureIgnoreCase))
                            ucf.SetShowInTaskBar(false);
                        else if (nextcmd.Equals("topmost", StringComparison.InvariantCultureIgnoreCase))
                            ucf.SetTopMost(true);
                        else if (nextcmd.Equals("normalz", StringComparison.InvariantCultureIgnoreCase))
                            ucf.SetTopMost(false);
                        else if (nextcmd.Equals("showintaskbar", StringComparison.InvariantCultureIgnoreCase))
                            ucf.SetShowInTaskBar(true);
                        else if (nextcmd.Equals("notshowintaskbar", StringComparison.InvariantCultureIgnoreCase))
                            ucf.SetShowInTaskBar(false);
                        else if (nextcmd.Equals("minimize", StringComparison.InvariantCultureIgnoreCase))
                            ucf.WindowState = FormWindowState.Minimized;
                        else if (nextcmd.Equals("normal", StringComparison.InvariantCultureIgnoreCase))
                            ucf.WindowState = FormWindowState.Normal;
                        else if (nextcmd.Equals("maximize", StringComparison.InvariantCultureIgnoreCase))
                            ucf.WindowState = FormWindowState.Maximized;
                        else if (nextcmd.Equals("location", StringComparison.InvariantCultureIgnoreCase))
                        {
                            int? x = sp.GetInt();
                            sp.IsCharMoveOn(',');
                            int? y = sp.GetInt();
                            sp.IsCharMoveOn(',');
                            int? w = sp.GetInt();
                            sp.IsCharMoveOn(',');
                            int? h = sp.GetInt();

                            if (x.HasValue && y.HasValue && w.HasValue && h.HasValue)
                            {
                                ucf.Location = new Point(x.Value, y.Value);
                                ucf.Size = new Size(w.Value, h.Value);
                            }
                            else
                                ap.ReportError("Location needs x,y,w,h in Panel");
                        }
                        else if (nextcmd.Equals("position", StringComparison.InvariantCultureIgnoreCase))
                        {
                            int? x = sp.GetInt();
                            sp.IsCharMoveOn(',');
                            int? y = sp.GetInt();
                            sp.IsCharMoveOn(',');

                            if (x.HasValue && y.HasValue )
                                ucf.Location = new Point(x.Value, y.Value);
                            else
                                ap.ReportError("Position needs x,y in Panel");
                        }
                        else if (nextcmd.Equals("size", StringComparison.InvariantCultureIgnoreCase))
                        {
                            int? w = sp.GetInt();
                            sp.IsCharMoveOn(',');
                            int? h = sp.GetInt();

                            if (w.HasValue && h.HasValue)
                                ucf.Size = new Size(w.Value, h.Value);
                            else
                                ap.ReportError("Size needs x,y,w,h in Panel");
                        }
                    }
                    else if (nextcmd.Equals("exists", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ap.currentvars[prefix + "Exists"] = "0";
                    }
                    else
                    {
                        Forms.PopOutControl.PopOuts? poi = poc.GetPopOutTypeByName(cmdname);

                        if (poi.HasValue)
                        {
                            if (nextcmd.Equals("Toggle", StringComparison.InvariantCultureIgnoreCase) || nextcmd.Equals("ON", StringComparison.InvariantCultureIgnoreCase))
                            {
                                poc.PopOut(poi.Value);
                            }
                            else
                                ap.ReportError("Cannot use command " + cmdname + " after generic panel name in Panel");
                        }
                        else
                            ap.ReportError("Cannot find generic panel name " + cmdname + " in Panel");
                    }

                }
            }
            else
                ap.ReportError(res);

            return true;
        }

    }
}
