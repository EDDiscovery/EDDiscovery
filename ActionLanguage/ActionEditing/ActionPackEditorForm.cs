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
            public ExtendedControls.ComboBoxCustom grouptype;
            public ExtendedControls.ButtonExt delete;
            public ActionPackEditBase usercontrol;              
        }

        List<Group> groups; // the groups

        public Func<string, List<string>> AdditionalNames;                          // Call back when we need more variable names, by event string
        public Func<string, Condition, ActionPackEditBase> CreateActionPackEdit;    // must set, given a group and condition, what editor do you want? change the condition if you need to

        #region Init

        public ActionPackEditorForm() 
        {
            InitializeComponent();
        }

        public void Init(string t, Icon ic, ActionCoreController cp, string appfolder, ActionFile file, List<ActionEvent> evlist)       // here, change to using events
        {
            this.Icon = ic;
            actioncorecontroller = cp;
            applicationfolder = appfolder;
            actionfile = file;
            events = evlist;

            grouptypenames = (from e in events select e.uiclass).ToList().Distinct().ToList();      // here we extract from events relevant data
            groupeventlist = new Dictionary<string, List<string>>();
            foreach (string s in grouptypenames)
                groupeventlist.Add(s, (from e in events where e.uiclass == s select e.triggername).ToList());

            bool winborder = ExtendedControls.ThemeableFormsInstance.Instance.ApplyToForm(this, SystemFonts.DefaultFont);
            statusStripCustom.Visible = panelTop.Visible = panelTop.Enabled = !winborder;
            initialtitle = this.Text = label_index.Text = t;

            ConditionLists clist = actionfile.actioneventlist;          // now load the initial conditions from the action file
            groups = new List<Group>();

            for (int i = 0; i < clist.Count; i++)       // for ever event, find the condition, create the group, theme
            {
                Condition cd = clist.Get(i);
                Group g = CreateGroup(cd);
                groups.Add(g);
                ExtendedControls.ThemeableFormsInstance.Instance.ApplyToControls(g.panel, SystemFonts.DefaultFont);
            }

            foreach (Group g in groups)     // add the groups to the vscroller
                panelVScroll.Controls.Add(g.panel);

            PositionGroups(true);       //repositions all items

            this.Text = label_index.Text = initialtitle + " (" + groups.Count.ToString() + " conditions)";

            Usercontrol_RefreshEvent();
        }

        #endregion

        #region Group Making and positions

        private Group CreateGroup(Condition cd)     // create a group, create the UC under it if required
        {
            Group g = new Group();

            g.panel = new Panel();
            //g.panel.BackColor = Color.Red; // useful for debug
            g.panel.SuspendLayout();

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
            else
                g.panel.Height = 28;    // no UG yet to set size, size it appropriately

            g.grouptype.SelectedIndexChanged += Grouptype_SelectedIndexChanged;
            g.grouptype.Tag = g;
            g.panel.Controls.Add(g.grouptype);

            g.delete = new ExtendedControls.ButtonExt();
            g.delete.Text = "X";
            g.delete.Size = new Size(24, 24);
            g.delete.Tag = g;
            g.delete.Click += Delete_Click;
            toolTip.SetToolTip(g.delete, "Delete this event");

            g.panel.Controls.Add(g.delete);

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
            g.usercontrol.Init(cd, groupeventlist[g.grouptype.Text], actioncorecontroller, applicationfolder, actionfile, AdditionalNames , 
                                    this.Icon, toolTip);

            ExtendedControls.ThemeableFormsInstance.Instance.ApplyToControls(g.usercontrol, SystemFonts.DefaultFont);

            g.usercontrol.Location = new Point(g.grouptype.Right+16, 0);
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

            for (int i = 0; i < groups.Count; i++)
            {
                Group g = groups[i];

                int farx = 100;

                g.panel.Location = new Point(panelxmargin, y + panelVScroll.ScrollOffset);
                g.panel.Width = Math.Max(panelwidth - panelxmargin * 2, farx); //, panelymargin + conditionhoff);
                g.delete.Location = new Point(g.panel.Width - 30, panelymargin);

                if (g.usercontrol != null)
                    g.usercontrol.Width = g.panel.Size.Width - 4 - 30 - g.usercontrol.Left;

                //System.Diagnostics.Debug.WriteLine(g.GetHashCode() + " " + i + " " + g.panel.Height);
                y += g.panel.Height + 2;
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

        }

        private string GetGroupName(string a)
        {
            ActionEvent p = events.Find(x => x.triggername == a);
            return (p == null) ? "Misc" : p.uiclass;
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
            Group g = CreateGroup(null);
            ExtendedControls.ThemeableFormsInstance.Instance.ApplyToControls(g.panel, SystemFonts.DefaultFont);
            groups.Add(g);
            PositionGroups(true);
            panelVScroll.Controls.Add(g.panel);
            this.Text = label_index.Text = initialtitle + " (" + groups.Count.ToString() + " conditions)";
            panelVScroll.ToEnd();       // tell it to scroll to end
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            foreach (Group g in groups)
            {
                if (Object.ReferenceEquals(g, ((Control)sender).Tag))
                {
                    g.usercontrol.Dispose();
                    g.panel.Controls.Clear();
                    g.grouptype.Dispose();
                    g.delete.Dispose();
                    g.panel.Dispose();
                    groups.Remove(g);
                    PositionGroups(false);
                    return;
                }
            }
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

            int index = 1;
            foreach (Group g in groups)
            {
                string prefix = "Event " + index.ToStringInvariant() + ": ";

                if (g.usercontrol == null)
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
                        result.Add(g.usercontrol.cd);
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

                    apf.Init("Action program " , this.Icon, actioncorecontroller, applicationfolder,
                                AdditionalNames(""),        // no event associated, so just return the globals in effect
                                actionfile.name, p, 
                                actionfile.actionprogramlist.GetActionProgramList(), "", ModifierKeys.HasFlag(Keys.Shift));

                    DialogResult res = apf.ShowDialog();

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
            if (!actionfile.actionprogramlist.EditProgram(s, actionfile.name , actioncorecontroller , applicationfolder))
                ExtendedControls.MessageBoxTheme.Show(this, "Unknown program or not in this file " + s);
        }

        private void buttonInstallationVars_Click(object sender, EventArgs e)
        {
            ConditionVariablesForm avf = new ConditionVariablesForm();
            avf.Init("Configuration items for installation - specialist use",  this.Icon, actionfile.installationvariables, showone: false);

            if (avf.ShowDialog(this) == DialogResult.OK)
            {
                actionfile.ChangeInstallationVariables(avf.result);
            }
        }

        #endregion

        #region Sort

        private void buttonSort_Click(object sender, EventArgs e)
        {
            groups.Sort(new GroupSort1());
            PositionGroups(true);
        }

        private void buttonSort2_Click(object sender, EventArgs e)
        {
            groups.Sort(new GroupSort2());
            PositionGroups(true);

        }

        class GroupSort1 : IComparer<Group>
        {
            public int Compare(Group our, Group other)
            {
                int r = 1;
                if (other.usercontrol != null && our.usercontrol != null)
                {
                    r = our.usercontrol.cd.eventname.CompareTo(other.usercontrol.cd.eventname);

                    if (r == 0)
                        r = our.usercontrol.cd.actiondata.CompareTo(other.usercontrol.cd.actiondata);

                    if (r == 0 && our.usercontrol.cd.fields != null && other.usercontrol.cd.fields != null)
                        r = our.usercontrol.cd.fields[0].matchstring.CompareTo(other.usercontrol.cd.fields[0].matchstring);
                }

                return r;
            }
        }
        class GroupSort2 : IComparer<Group>
        {
            public int Compare(Group our, Group other)
            {
                int r = 1;

                if (other.usercontrol != null && our.usercontrol != null)
                {
                    r = our.usercontrol.cd.eventname.CompareTo(other.usercontrol.cd.eventname);

                    if (r == 0 && our.usercontrol.cd.fields != null && other.usercontrol.cd.fields != null)
                        r = our.usercontrol.cd.fields[0].matchstring.CompareTo(other.usercontrol.cd.fields[0].matchstring);

                    if (r == 0)
                        r = our.usercontrol.cd.actiondata.CompareTo(other.usercontrol.cd.actiondata);
                }

                return r;
            }
        }

        #endregion

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

    }
}
