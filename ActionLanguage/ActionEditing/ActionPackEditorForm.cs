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
using BaseUtils.Win32Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Conditions;

namespace ActionLanguage
{
    public partial class ActionPackEditorForm : ExtendedControls.DraggableForm
    {
        ActionFile actionfile;      // file we are editing
        string applicationfolder;   // folder where the file is
        ActionCoreController actioncorecontroller;  // need this for some access to data
        List<ActionEvent> events;   // list of events, UIs
        List<string> grouptypenames;    // groupnames extracted from events
        Dictionary<string, List<string>> groupeventlist;    // events per group name, extracted from events
        string initialtitle;

        const int panelxmargin = 3;
        const int panelymargin = 1;

        class Group     // this top level form has a list of groups, each containing a grouptype CBC, a delete button, and a UC containing its controls
        {
            public Panel panel;

            public ExtendedControls.ComboBoxCustom grouptype;       // present for any other than group name
            public ActionPackEditBase usercontrol;                  // present for any other than group name, but may or may not be set

            public ExtendedControls.ButtonExt action;               // always present

            public Panel groupnamepanel;                            // present for group name
            public Label groupnamelabel;                            // present for group name
            public ExtendedControls.ButtonExt groupnamecollapsebutton;               // grouping button
            public bool collapsed;                                  // if collapsed..

            public bool IsGroupName { get { return groupnamepanel != null; } }
            public bool IsEvent { get { return groupnamepanel == null; } }

            public void Dispose()
            {
                panel.Controls.Clear();

                if (usercontrol != null)
                    usercontrol.Dispose();

                if (grouptype != null)
                    grouptype.Dispose();

                if (groupnamepanel != null)
                    groupnamepanel.Dispose();

                if (groupnamelabel != null)
                    groupnamelabel.Dispose();

                if (groupnamecollapsebutton != null)
                    groupnamecollapsebutton.Dispose();

                if (action != null)
                    action.Dispose();

                panel.Dispose();
            }
        }

        List<Group> groups; // the groups

        public Func<string, List<string>> AdditionalNames;                          // Call back when we need more variable names, by event string
        public Func<string, Condition, ActionPackEditBase> CreateActionPackEdit;    // must set, given a group and condition, what editor do you want? change the condition if you need to

        #region Init

        public ActionPackEditorForm()
        {
            groups = new List<Group>();
            InitializeComponent();
        }

        public void Init(string t, Icon ic, ActionCoreController cp, string appfolder, ActionFile file, List<ActionEvent> evlist , string collapsestate)       // here, change to using events
        {
            this.Icon = ic;
            actioncorecontroller = cp;
            applicationfolder = appfolder;
            actionfile = file;
            events = evlist;

            grouptypenames = (from e in events select e.UIClass).ToList().Distinct().ToList();      // here we extract from events relevant data
            groupeventlist = new Dictionary<string, List<string>>();
            foreach (string s in grouptypenames)
                groupeventlist.Add(s, (from e in events where e.UIClass == s select e.TriggerName).ToList());

            bool winborder = ExtendedControls.ThemeableFormsInstance.Instance.ApplyToForm(this, SystemFonts.DefaultFont);
            statusStripCustom.Visible = panelTop.Visible = panelTop.Enabled = !winborder;
            initialtitle = this.Text = label_index.Text = t;

            ConditionLists clist = actionfile.actioneventlist;          // now load the initial conditions from the action file


            string eventname = null;
            for (int i = 0; i < clist.Count; i++)       // for ever event, find the condition, create the group, theme
            {
                string gname = clist.GetGroupName(i);
                if (gname != eventname)
                {
                    eventname = gname;
                    Group gg = CreateGroup(false, null, gname);
                    gg.collapsed = collapsestate.Contains("<" + gname + ";");
                    groups.Add(gg);
                }

                Condition cd = clist.Get(i);
                Group g = CreateGroup(true, cd, null);
                groups.Add(g);
            }

            foreach (Group g in groups)     // add the groups to the vscroller
                panelVScroll.Controls.Add(g.panel);

            PositionGroups(true);       //repositions all items

            Usercontrol_RefreshEvent();
        }

        #endregion

        #region Group Making and positions

        private Group CreateGroup(bool isevent, Condition cd, string name)     // create a group, create the UC under it if required
        {
            Group g = new Group();

            g.panel = new Panel();
            g.panel.SuspendLayout();
            g.panel.Height = 28;    // Def height

            if (isevent)
            {
                //g.panel.BackColor = Color.Green; // useful for debug
                g.grouptype = new ExtendedControls.ComboBoxCustom();
                g.grouptype.Items.AddRange(grouptypenames);
                g.grouptype.Location = new Point(panelxmargin, panelymargin);
                g.grouptype.Size = new Size(80, 24);
                g.grouptype.DropDownHeight = 400;
                g.grouptype.SetTipDynamically(toolTip, "Select event class");

                if (cd != null)
                {
                    g.grouptype.Enabled = false;
                    g.grouptype.SelectedItem = GetGroupName(cd.eventname);
                    g.grouptype.Enabled = true;

                    CreateUserControl(g, cd);
                }

                g.grouptype.SelectedIndexChanged += Grouptype_SelectedIndexChanged;
                g.grouptype.Tag = g;
                g.panel.Controls.Add(g.grouptype);
            }
            else
            {
                //g.panel.BackColor = Color.Yellow; // for debug
                g.groupnamepanel = new Panel();
                g.groupnamepanel.Location = new Point(3, 2);
                g.panel.Controls.Add(g.groupnamepanel);

                g.groupnamecollapsebutton = new ExtendedControls.ButtonExt();
                g.groupnamecollapsebutton.Text = "-";
                g.groupnamecollapsebutton.Size = new Size(16, 16);
                g.groupnamecollapsebutton.Tag = g;
                g.groupnamecollapsebutton.MouseDown += Groupnamecollapsebutton_MouseDown;
                g.groupnamecollapsebutton.Click += Groupnamecollapsebutton_Click;
                g.groupnamecollapsebutton.Location = new Point(2, 2);
                g.groupnamepanel.Controls.Add(g.groupnamecollapsebutton);

                g.groupnamelabel = new Label();
                g.groupnamelabel.Text = name;
                g.groupnamelabel.Location = new Point(24, 2);
                g.groupnamepanel.Controls.Add(g.groupnamelabel);
            }

            g.action = new ExtendedControls.ButtonExt();
            g.action.Text = ">";
            g.action.Size = new Size(24, 24);
            g.action.Tag = g;
            g.action.Click += Action_Click;
            toolTip.SetToolTip(g.action, "Move event up");
            g.panel.Controls.Add(g.action);

            ExtendedControls.ThemeableFormsInstance.Instance.ApplyToControls(g.panel, SystemFonts.DefaultFont);

            if (g.groupnamepanel != null)
                g.groupnamepanel.BackColor = ExtendedControls.ThemeableFormsInstance.Instance.TextBlockBorderColor;

            g.panel.ResumeLayout();

            return g;
        }

        private void CreateUserControl(Group g, Condition inputcond)        // create the UC under the group.  Condition will be either AlwaysTrue if new, or stored condition from actionprogram
        {
            if (g.usercontrol != null)
            {
                Controls.Remove(g.usercontrol);
                g.usercontrol.Dispose();
            }

            Condition cd = new Condition(inputcond);                       // make a copy for editing purposes.

            g.usercontrol = CreateActionPackEdit(g.grouptype.Text, cd); // make the user control, based on text/cond.  Allow cd to be altered

            // init, with the copy of inputcond so they can edit it without commiting change
            g.usercontrol.Init(cd, groupeventlist[g.grouptype.Text], actioncorecontroller, applicationfolder, actionfile, AdditionalNames,
                                    this.Icon, toolTip);

            ExtendedControls.ThemeableFormsInstance.Instance.ApplyToControls(g.usercontrol, SystemFonts.DefaultFont);

            g.usercontrol.Location = new Point(g.grouptype.Right + 16, 0);
            g.usercontrol.Size = new Size(5000, g.usercontrol.Height);

            g.usercontrol.RefreshEvent += Usercontrol_RefreshEvent;

            g.panel.Height = g.usercontrol.Height;
            g.panel.Controls.Add(g.usercontrol);
            //System.Diagnostics.Debug.WriteLine(g.GetHashCode() + " " + g.panel.Height);
        }

        private void PositionGroups(bool calcminsize)
        {
            int y = panelymargin;
            int panelwidth = Math.Max(panelVScroll.Width - panelVScroll.ScrollBarWidth, 10);

            bool collapsed = false;

            for (int i = 0; i < groups.Count; i++)
            {
                Group g = groups[i];

                if (g.IsGroupName)
                { 
                    collapsed = g.collapsed;
                    g.groupnamecollapsebutton.Text = collapsed ? "+" : "-";
                }

                if (g.IsGroupName || collapsed == false)
                {
                    int farx = 100;

                    g.panel.Visible = true;
                    g.panel.Location = new Point(panelxmargin, y + panelVScroll.ScrollOffset);
                    g.panel.Width = Math.Max(panelwidth - panelxmargin * 2, farx); //, panelymargin + conditionhoff);
                    g.action.Location = new Point(g.panel.Width - 30, panelymargin);

                    int widthuc = g.panel.Size.Width - 4 - 32;
                    if (g.groupnamepanel != null)
                        g.groupnamepanel.Size = new Size(widthuc - g.groupnamepanel.Left, g.panel.Height - 8);

                    if (g.usercontrol != null)
                        g.usercontrol.Width = widthuc - g.usercontrol.Left;

                    //System.Diagnostics.Debug.WriteLine(g.GetHashCode() + " " + i + " " + g.panel.Height);
                    y += g.panel.Height + 2;
                }
                else
                    g.panel.Visible = false;
            }

            buttonMore.Location = new Point(panelxmargin, y + panelVScroll.ScrollOffset);

            int titleHeight = RectangleToScreen(this.ClientRectangle).Top - this.Top;
            y += buttonMore.Height + titleHeight + ((panelTop.Enabled) ? (panelTop.Height + statusStripCustom.Height) : 8) + 16 + panelOK.Height;

            if (calcminsize)
            {
                this.MinimumSize = new Size(800, y);
                this.MaximumSize = new Size(Screen.FromControl(this).WorkingArea.Width - 100, Screen.FromControl(this).WorkingArea.Height - 100);

                if (Bottom > Screen.FromControl(this).WorkingArea.Height)
                    Top = Screen.FromControl(this).WorkingArea.Height - Height - 50;
            }

            this.Text = label_index.Text = initialtitle + " (" + groups.Count.ToString() + " conditions)";
        }

        private string GetGroupName(string a)
        {
            ActionEvent p = events.Find(x => x.TriggerName == a);
            return (p == null) ? "Misc" : p.UIClass;
        }

        #endregion

        private void Usercontrol_RefreshEvent()
        {
            comboBoxCustomEditProg.Enabled = false;
            comboBoxCustomEditProg.Items.Clear();
            comboBoxCustomEditProg.Items.AddRange(actionfile.actionprogramlist.GetActionProgramList(true));
            comboBoxCustomEditProg.Enabled = true;

            foreach (Group g in groups)
            {
                if (g.usercontrol != null)
                    g.usercontrol.UpdateProgramList(actionfile.actionprogramlist.GetActionProgramList());
            }
        }

        #region UI

        private void buttonMore_Click(object sender, EventArgs e)
        {
            Group g = CreateGroup(true, null, null);
            groups.Add(g);
            panelVScroll.Controls.Add(g.panel);

            int groupabove = GetGroupAbove(groups.Count - 1);       // if group above exists and is collapsed, need to expand it to show
            if (groupabove != -1 && groups[groupabove].collapsed == true)
                groups[groupabove].collapsed = false;

            PositionGroups(true);
            panelVScroll.ToEnd();       // tell it to scroll to end
        }

        private void Grouptype_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExtendedControls.ComboBoxCustom b = sender as ExtendedControls.ComboBoxCustom;
            Group g = (Group)b.Tag;
            CreateUserControl(g, Condition.AlwaysTrue());
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            string res = Check();

            if (res.Length > 0)
            {
                string acceptstr = "Click Retry to correct errors, Abort to cancel, Ignore to accept valid entries";

                DialogResult dr = ExtendedControls.MessageBoxTheme.Show(this, "Filters produced the following warnings and errors" + Environment.NewLine + Environment.NewLine + res + Environment.NewLine + acceptstr,
                                                  "Warning", MessageBoxButtons.AbortRetryIgnore);

                if (dr == DialogResult.Retry)
                {
                    return;
                }
                else if (dr == DialogResult.Abort || dr == DialogResult.Cancel)
                {
                    DialogResult = DialogResult.Cancel;
                    Close();
                }
            }

            actionfile.ChangeEventList(result);
            actionfile.WriteFile();

            DialogResult = DialogResult.OK;
            Close();
        }

        ConditionLists result;
        private string Check()
        {
            string errorlist = "";
            result = new ConditionLists();

            string eventgroup = null;

            int index = 1;
            foreach (Group g in groups)
            {
                string prefix = "Event " + index.ToStringInvariant() + ": ";

                if (g.groupnamelabel != null)
                {
                    eventgroup = g.groupnamelabel.Text;
                }
                else if (g.usercontrol == null)
                {
                    errorlist += prefix + "Ignored group with empty name" + Environment.NewLine;
                }
                else
                {
                    Condition c = g.usercontrol.cd;

                    if (c.eventname.Length == 0)
                        errorlist += prefix + "Event " + g.usercontrol.ID() + " does not have an event name defined" + Environment.NewLine;
                    else if (c.action.Equals("New") || c.action.Length == 0)        // actions, but not selected one..
                        errorlist += prefix + "Event " + g.usercontrol.ID() + " does not have an action program defined" + Environment.NewLine;
                    else if (c.fields == null || c.fields.Count == 0)
                        errorlist += prefix + "Event " + g.usercontrol.ID() + " does not have a condition" + Environment.NewLine;
                    else
                    {
                        result.Add(g.usercontrol.cd, eventgroup);
                    }
                }

                index++;
            }

            return errorlist;
        }

        private void comboBoxCustomEditProg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCustomEditProg.Enabled)
            {
                string progname = comboBoxCustomEditProg.Text.Replace(" (Ext)", "");     // remove the EXT marker

                ActionProgram p = null;
                if (!progname.Equals("New"))
                    p = actionfile.actionprogramlist.Get(progname);

                if (p != null && p.StoredInSubFile != null)
                {
                    p.EditInEditor(p.StoredInSubFile);         // Edit in the editor..
                }
                else
                {
                    ActionProgramEditForm apf = new ActionProgramEditForm();
                    apf.EditProgram += EditProgram;

                    apf.Init("Action program ", this.Icon, actioncorecontroller, applicationfolder,
                                AdditionalNames(""),        // no event associated, so just return the globals in effect
                                actionfile.name, p,
                                actionfile.actionprogramlist.GetActionProgramList(), "", ModifierKeys.HasFlag(Keys.Shift));

                    DialogResult res = apf.ShowDialog(this);

                    if (res == DialogResult.OK)
                    {
                        ActionProgram np = apf.GetProgram();
                        actionfile.actionprogramlist.AddOrChange(np);                // replaces or adds (if its a new name) same as rename
                        Usercontrol_RefreshEvent();
                    }
                    else if (res == DialogResult.Abort)   // delete
                    {
                        ActionProgram np2 = apf.GetProgram();
                        actionfile.actionprogramlist.Delete(np2.Name);
                        Usercontrol_RefreshEvent();
                    }
                }
            }
        }

        private void EditProgram(string s)  // Callback by APF to ask to edit another program..
        {
            if (!actionfile.actionprogramlist.EditProgram(s, actionfile.name, actioncorecontroller, applicationfolder))
                ExtendedControls.MessageBoxTheme.Show(this, "Unknown program or not in this file " + s);
        }

        private void buttonInstallationVars_Click(object sender, EventArgs e)
        {
            ConditionVariablesForm avf = new ConditionVariablesForm();
            avf.Init("Configuration items for installation - specialist use", this.Icon, actionfile.installationvariables, showone: false);

            if (avf.ShowDialog(this) == DialogResult.OK)
            {
                actionfile.ChangeInstallationVariables(avf.result);
            }
        }

        #endregion

        #region Form control 

        private void panelVScroll_Resize(object sender, EventArgs e)
        {
            PositionGroups(false);
        }

        private void panel_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        public void panel_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label_index_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void label_index_MouseUp(object sender, MouseEventArgs e)
        {
            OnCaptionMouseUp((Control)sender, e);
        }

        #endregion

        #region Action Click

        Group clickgroup;
        int clickgroupindex;

        private void Action_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            clickgroup = b.Tag as Group;
            clickgroupindex = groups.IndexOf(clickgroup);
            moveDownToolStripMenuItem.Enabled = clickgroupindex < groups.Count - 1;
            moveUpToolStripMenuItem.Enabled = clickgroupindex > 0;
            insertGroupToolStripMenuItem.Enabled = clickgroup.IsEvent && (clickgroupindex == 0 || groups[clickgroupindex - 1].groupnamelabel == null);
            moveToGroupAboveToolStripMenuItem.Enabled = clickgroup.IsEvent && GetGroupAbove(clickgroupindex) > 0;
            moveToGroupBelowToolStripMenuItem.Enabled = clickgroup.IsEvent && GetGroupBelow(clickgroupindex) != -1;
            moveGroupDownToolStripMenuItem.Enabled = clickgroup.IsGroupName && GetGroupBelow(clickgroupindex + 1) != -1;
            moveGroupUpToolStripMenuItem.Enabled = clickgroup.IsGroupName && GetGroupAbove(clickgroupindex - 1) != -1;
            renameGroupToolStripMenuItem.Enabled = clickgroup.IsGroupName;
            contextMenuStripAction.Show(b.PointToScreen(new Point(b.Width, 0)));
        }

        private void insertGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = ExtendedControls.PromptSingleLine.ShowDialog(this, "Group Name:", "", "Enter group name", Icon);

            if (s != null)
            {
                Group gg = CreateGroup(false, null, s);
                panelVScroll.Controls.Add(gg.panel);
                groups.Insert(clickgroupindex, gg);
                PositionGroups(true);
            }
        }

        private void insertNewEventAboveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Group gg = CreateGroup(true, null, null);
            panelVScroll.Controls.Add(gg.panel);
            groups.Insert(clickgroupindex, gg);
            PositionGroups(true);
        }

        private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groups.RemoveAt(clickgroupindex);
            groups.Insert(clickgroupindex + 1, clickgroup);
            PositionGroups(true);
        }

        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groups.RemoveAt(clickgroupindex);
            groups.Insert(clickgroupindex - 1, clickgroup);
            PositionGroups(true);
        }

        private void moveToGroupBelowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int inspos = GetGroupBelow(clickgroupindex);
            groups.RemoveAt(clickgroupindex);
            groups.Insert(inspos, clickgroup);
            PositionGroups(true);
        }

        private void moveToGroupAboveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int inspos = GetGroupAbove(clickgroupindex);
            groups.RemoveAt(clickgroupindex);
            groups.Insert(inspos, clickgroup);
            PositionGroups(true);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clickgroup.Dispose();
            groups.Remove(clickgroup);
            PositionGroups(false);
        }

        private void moveGroupUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int groupabove = GetGroupAbove(clickgroupindex - 1);
            int enditem = GetGroupBelow(clickgroupindex + 1);
            if (enditem == -1)
                enditem = groups.Count;

            List<Group> tomove = groups.GetRange(clickgroupindex, enditem - clickgroupindex);
            groups.RemoveRange(clickgroupindex, enditem - clickgroupindex);
            groups.InsertRange(groupabove, tomove);
            PositionGroups(false);
        }

        private void moveGroupDownToolStripMenuItem_Click(object sender, EventArgs e)
        {                                                                          
            // click item = 10, ng = 20 , sg = 30.. so remove 10-19.  ng->10.  Insert at 10+sg-ng
            int nextgroup = GetGroupBelow(clickgroupindex + 1); // must be there.. 
            int sgenditem = GetGroupBelow(nextgroup + 1); // may be there
            if (sgenditem == -1)
                sgenditem = groups.Count;

            int toremove = nextgroup - clickgroupindex;
            List<Group> tomove = groups.GetRange(clickgroupindex, toremove);
            groups.RemoveRange(clickgroupindex, toremove);

            groups.InsertRange(clickgroupindex + sgenditem - nextgroup, tomove);
            PositionGroups(false);
        }

        private int GetGroupBelow(int start)
        {
            for (int insert = start; insert < groups.Count; insert++)
            {
                if (groups[insert].IsGroupName)
                    return insert;
            }

            return -1;
        }

        private int GetGroupAbove(int start)
        {
            for (int insert = start; insert >= 0; insert--)
            {
                if (groups[insert].IsGroupName)
                    return insert;
            }

            return -1;
        }

        private void renameGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = ExtendedControls.PromptSingleLine.ShowDialog(this, "Group Name:", "", "Enter group name", Icon);

            if (s != null)
            {
                clickgroup.groupnamelabel.Text = s;
                PositionGroups(true);
            }
        }

        #endregion

        #region Collapse
        private void Groupnamecollapsebutton_MouseDown(object sender, MouseEventArgs e)
        {
            Button b = sender as Button;
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStripCollapse.Show(b.PointToScreen(new Point(b.Width, 0)));
            }
        }

        private void collapseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Group g in groups)
            {
                if (g.IsGroupName)
                    g.collapsed = true;
            }

            PositionGroups(false);
        }

        private void expandAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Group g in groups)
            {
                if (g.IsGroupName)
                    g.collapsed = false;
            }

            PositionGroups(true);
        }

        private void Groupnamecollapsebutton_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            Group g = b.Tag as Group;
            g.collapsed = !g.collapsed;
            PositionGroups(g.collapsed == false);
        }

        public string CollapsedState()      // save it per session so its not so horrible if you reenter
        {
            string str = "";

            foreach (Group g in groups)
            {
                if (g.IsGroupName && g.collapsed == true)
                    str += "<" + g.groupnamelabel.Text + ";";
            }

            return str;
        }

        #endregion

    }
}

