/*
 * Copyright © 2017 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaseUtils;
using ActionLanguage;

namespace EDDiscovery.Actions
{
    public class ActionPopout : ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<string> eventvars)
        {
            string promptValue = ExtendedControls.PromptSingleLine.ShowDialog(parent, "Popout command", UserData, "Configure Popout Command" , cp.Icon);
            if (promptValue != null)
            {
                userdata = promptValue;
            }

            return (promptValue != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string res;
            if (ap.functions.ExpandString(UserData, out res) != Conditions.ConditionFunctions.ExpandResult.Failed)
            {
                StringParser sp = new StringParser(res);
                string prefix = "P_";

                string cmdname = sp.NextWord();

                if (cmdname != null && cmdname.Equals("PREFIX", StringComparison.InvariantCultureIgnoreCase))
                {
                    prefix = sp.NextWord();

                    if (prefix == null)
                    {
                        ap.ReportError("Missing name after Prefix in in Popout");
                        return true;
                    }

                    cmdname = sp.NextWord();
                }

                Forms.PopOutControl poc = (ap.actioncontroller as ActionController).DiscoveryForm.PopOuts;

                if (cmdname == null)
                {
                    ap.ReportError("Missing command or popout name in Popout");
                }
                else if (cmdname.Equals("Status", StringComparison.InvariantCultureIgnoreCase))
                {
                    ap[prefix + "Count"] = poc.Count.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    for (int i = 0; i < poc.Count; i++)
                        ap[prefix + i.ToString(System.Globalization.CultureInfo.InvariantCulture)] = poc[i].Name;
                }
                else
                {
                    Forms.UserControlForm ucf = poc.Get(cmdname);

                    string nextcmd = sp.NextWord(" ",true);

                    if ( nextcmd == null )
                    {
                        ap.ReportError("Missing command after popout name in Popout");
                    }
                    else if ( nextcmd.Equals("status"))
                    {
                        ap[prefix + "Exists"] = (ucf != null) ? "1" : "0";

                        if ( ucf!= null)
                        {
                            ap[prefix + "Transparent"] = ucf.istransparent ? "1" : "0";
                            ap[prefix + "TopMost"] = ucf.TopMost ? "1" : "0";
                            ap[prefix + "DisplayTitle"] = ucf.displayTitle ? "1" : "0";
                            ap[prefix + "ShowInTaskbar"] = ucf.ShowInTaskbar ? "1" : "0";
                            ap[prefix + "WindowState"] = ucf.WindowState.ToString();
                            ap[prefix + "Top"] = ucf.Top.ToString(System.Globalization.CultureInfo.InvariantCulture);
                            ap[prefix + "Left"] = ucf.Left.ToString(System.Globalization.CultureInfo.InvariantCulture);
                            ap[prefix + "Width"] = ucf.Width.ToString(System.Globalization.CultureInfo.InvariantCulture);
                            ap[prefix + "Height"] = ucf.Height.ToString(System.Globalization.CultureInfo.InvariantCulture);
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
                            int? x = sp.NextWordComma().InvariantParseIntNull();
                            int? y = sp.NextWordComma().InvariantParseIntNull();
                            int? w = sp.NextWordComma().InvariantParseIntNull();
                            int? h = sp.NextWord().InvariantParseIntNull();

                            if (x.HasValue && y.HasValue && w.HasValue && h.HasValue)
                            {
                                ucf.Location = new Point(x.Value, y.Value);
                                ucf.Size = new Size(w.Value, h.Value);
                            }
                            else
                                ap.ReportError("Location needs x,y,w,h in Popout");
                        }
                        else if (nextcmd.Equals("position"))
                        {
                            int? x = sp.NextWordComma().InvariantParseIntNull();
                            int? y = sp.NextWord().InvariantParseIntNull();

                            if (x.HasValue && y.HasValue)
                                ucf.Location = new Point(x.Value, y.Value);
                            else
                                ap.ReportError("Position needs x,y in Popout");
                        }
                        else if (nextcmd.Equals("size"))
                        {
                            int? w = sp.NextWordComma().InvariantParseIntNull();
                            int? h = sp.NextWord().InvariantParseIntNull();

                            if (w.HasValue && h.HasValue)
                                ucf.Size = new Size(w.Value, h.Value);
                            else
                                ap.ReportError("Size needs x,y,w,h in Popout");
                        }
                        else
                            ap.ReportError("Unknown option " + nextcmd + " after popout name in Popout");
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
                                ap.ReportError("Cannot use command " + nextcmd + " after generic popout name in Popout");
                        }
                        else
                            ap.ReportError("Cannot find generic popout name " + cmdname + " in Popout");
                    }

                }
            }
            else
                ap.ReportError(res);

            return true;
        }

    }
}
