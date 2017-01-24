using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    class ActionHistoryTab : Action
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            string promptValue = PromptSingleLine.ShowDialog(parent, "HistoryTab command", UserData, "Configure HistoryTab Command");
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
                string prefix = "HT_";

                string cmdname = sp.NextWord();

                if (cmdname != null && cmdname.Equals("PREFIX", StringComparison.InvariantCultureIgnoreCase))
                {
                    prefix = sp.NextWord();

                    if (prefix == null)
                    {
                        ap.ReportError("Missing name after Prefix in Historytab");
                        return true;
                    }

                    cmdname = sp.NextWord();
                }

#if false
                if (cmdname == null)
                {
                    ap.ReportError("Missing command or panel name in Historytab");
                }
                else if (cmdname.Equals("Status", StringComparison.InvariantCultureIgnoreCase))
                {
                    ap.currentvars[prefix + "Count"] = poc.Count.ToString();
                    for (int i = 0; i < poc.Count; i++)
                        ap.currentvars[prefix + i.ToString()] = poc[i].Name;
                }
                else
                {
                    Forms.UserControlForm ucf = poc.Get(cmdname);

                    string nextcmd = sp.NextWord(" ", true);

                    if (nextcmd == null)
                    {
                        ap.ReportError("Missing command after panel name in Panel");
                    }
                    else if (nextcmd.Equals("status"))
                    {
                        ap.currentvars[prefix + "Exists"] = (ucf != null) ? "1" : "0";

                        if (ucf != null)
                        {
                            ap.currentvars[prefix + "Transparent"] = ucf.istransparent ? "1" : "0";
                            ap.currentvars[prefix + "TopMost"] = ucf.TopMost ? "1" : "0";
                            ap.currentvars[prefix + "DisplayTitle"] = ucf.displayTitle ? "1" : "0";
                            ap.currentvars[prefix + "ShowInTaskbar"] = ucf.ShowInTaskbar ? "1" : "0";
                            ap.currentvars[prefix + "WindowState"] = ucf.WindowState.ToString();
                            ap.currentvars[prefix + "Top"] = ucf.Top.ToString();
                            ap.currentvars[prefix + "Left"] = ucf.Left.ToString();
                            ap.currentvars[prefix + "Width"] = ucf.Width.ToString();
                            ap.currentvars[prefix + "Height"] = ucf.Height.ToString();
                        }
                    }
                    else if (ucf != null)        // found a panel with the name
                    {
                        if (nextcmd.Equals("toggle") || nextcmd.Equals("off"))
                            ucf.Close();
                        else if (nextcmd.Equals("on"))   // does nothing
                        { }
                        else if (nextcmd.Equals("transparent"))
                            ucf.SetTransparency(true);
                        else if (nextcmd.Equals("opaque"))
                            ucf.SetTransparency(false);
                        else if (nextcmd.Equals("title"))
                            ucf.SetShowInTaskBar(true);
                        else if (nextcmd.Equals("notitle"))
                            ucf.SetShowInTaskBar(false);
                        else if (nextcmd.Equals("topmost"))
                            ucf.SetTopMost(true);
                        else if (nextcmd.Equals("normalz"))
                            ucf.SetTopMost(false);
                        else if (nextcmd.Equals("showintaskbar"))
                            ucf.SetShowInTaskBar(true);
                        else if (nextcmd.Equals("notshowintaskbar"))
                            ucf.SetShowInTaskBar(false);
                        else if (nextcmd.Equals("minimize"))
                            ucf.WindowState = FormWindowState.Minimized;
                        else if (nextcmd.Equals("normal"))
                            ucf.WindowState = FormWindowState.Normal;
                        else if (nextcmd.Equals("maximize"))
                            ucf.WindowState = FormWindowState.Maximized;
                        else if (nextcmd.Equals("location"))
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
                        else if (nextcmd.Equals("position"))
                        {
                            int? x = sp.GetInt();
                            sp.IsCharMoveOn(',');
                            int? y = sp.GetInt();
                            sp.IsCharMoveOn(',');

                            if (x.HasValue && y.HasValue)
                                ucf.Location = new Point(x.Value, y.Value);
                            else
                                ap.ReportError("Position needs x,y in Panel");
                        }
                        else if (nextcmd.Equals("size"))
                        {
                            int? w = sp.GetInt();
                            sp.IsCharMoveOn(',');
                            int? h = sp.GetInt();

                            if (w.HasValue && h.HasValue)
                                ucf.Size = new Size(w.Value, h.Value);
                            else
                                ap.ReportError("Size needs x,y,w,h in Panel");
                        }
                        else
                            ap.ReportError("Unknown command " + cmdname + " after panel name in Panel");
                    }
                    else
                    {
                        Forms.PopOutControl.PopOuts? poi = poc.GetPopOutTypeByName(cmdname);

                        if (poi.HasValue)
                        {
                            if (nextcmd.Equals("toggle") || nextcmd.Equals("on"))
                            {
                                poc.PopOut(poi.Value);
                            }
                            else
                                ap.ReportError("Cannot use command " + nextcmd + " after generic panel name in Panel");
                        }
                        else
                            ap.ReportError("Cannot find generic panel name " + cmdname + " in Panel");
                    }

                }
#endif
            }
            else
                ap.ReportError(res);

            return true;
        }

    }


}
