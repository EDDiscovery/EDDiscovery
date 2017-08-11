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
        ActionFile actionfile;
        List<Tuple<string, string>> events;
        List<string> grouptypenames;
        Dictionary<string, List<string>> groupeventlist;
        string initialtitle;

        const int panelxmargin = 3;
        const int panelymargin = 1;
        const int conditionhoff = 28;

        class Group
        {
            public Panel panel;
            public ExtendedControls.ComboBoxCustom grouptype;
            public ActionPackEditBase usercontrol;
        }

        List<Group> groups;

        ActionCoreController actioncorecontroller;
        string applicationfolder;

        public ActionPackEditorForm() 
        {
            InitializeComponent();
        }

        public delegate List<string> AdditionalNames(string ev);
        public event AdditionalNames onAdditionalNames;             // must set this, provide extra names
        public Func<string, Condition, ActionPackEditBase> CreateActionPackEdit;    // must set, given a group and condition, what editor do you want?

        public void Init(string t, Icon ic, ActionCoreController cp, string appfolder, ActionFile file, List<Tuple<string, string>> ev)
        {
            this.Icon = ic;
            actioncorecontroller = cp;
            applicationfolder = appfolder;
            actionfile = file;
            events = ev;

            grouptypenames = (from e in events select e.Item2).ToList().Distinct().ToList();
            groupeventlist = new Dictionary<string, List<string>>();
            foreach (string s in grouptypenames)
                groupeventlist.Add(s, (from e in events where e.Item2 == s select e.Item1).ToList());

            bool winborder = ExtendedControls.ThemeableFormsInstance.Instance.ApplyToForm(this, SystemFonts.DefaultFont);
            statusStripCustom.Visible = panelTop.Visible = panelTop.Enabled = !winborder;
            initialtitle = this.Text = label_index.Text = t;

            LoadConditions();
        }

        private void LoadConditions()
        {
            ConditionLists c = actionfile.actioneventlist;
            groups = new List<Group>();

            for (int i = 0; i < c.Count; i++)
            {
                Condition cd = c.Get(i);
                Group g = CreateGroup(cd);
                groups.Add(g);
                ExtendedControls.ThemeableFormsInstance.Instance.ApplyToControls(g.panel, SystemFonts.DefaultFont);
            }

            foreach (Group g in groups)
                panelVScroll.Controls.Add(g.panel);

            PositionGroups(true);
            this.Text = label_index.Text = initialtitle + " (" + groups.Count.ToString() + " conditions)";

            Usercontrol_RefreshEvent();
        }

        private Group CreateGroup(Condition cd)
        {
            Group g = new Group();

            g.panel = new Panel();
            //g.panel.BackColor = Color.Red;
            g.panel.SuspendLayout();

            g.grouptype = new ExtendedControls.ComboBoxCustom();
            g.grouptype.Items.AddRange(grouptypenames);
            g.grouptype.Location = new Point(panelxmargin, panelymargin);
            g.grouptype.Size = new Size(100, 24);
            g.grouptype.DropDownHeight = 400;

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
            g.panel.ResumeLayout();

            return g;
        }

        private void CreateUserControl(Group g, Condition c)
        {
            if (g.usercontrol != null)
            {
                Controls.Remove(g.usercontrol);
                g.usercontrol.Dispose();
            }

            g.usercontrol = CreateActionPackEdit(g.grouptype.Text, c);
            g.usercontrol.Init(c, groupeventlist[g.grouptype.Text], actioncorecontroller, applicationfolder, actionfile, onAdditionalNames , this.Icon);
            ExtendedControls.ThemeableFormsInstance.Instance.ApplyToControls(g.usercontrol, SystemFonts.DefaultFont);
            g.usercontrol.Location = new Point(panelxmargin + 108, 0);
            g.usercontrol.Size = new Size(5000, g.panel.Height);
            g.usercontrol.RefreshEvent += Usercontrol_RefreshEvent;
            g.usercontrol.RemoveItem += Usercontrol_RemoveEvent;

            g.panel.Controls.Add(g.usercontrol);
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
                g.panel.Size = new Size(Math.Max(panelwidth - panelxmargin * 2, farx), panelymargin + conditionhoff);

                if (g.usercontrol != null)
                    g.usercontrol.Size = new Size(g.panel.Size.Width - 4 - g.usercontrol.Left, g.panel.Height);

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
            Tuple<string, string> p = events.Find(x => x.Item1 == a);
            return (p == null) ? "Misc" : p.Item2;
        }

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

        private void Usercontrol_RemoveEvent(ActionPackEditBase item)
        {
            foreach (Group g in groups)
            {
                if ( Object.ReferenceEquals(g.usercontrol,item ))
                {
                    g.usercontrol.Dispose();
                    g.panel.Controls.Clear();
                    g.grouptype.Dispose();
                    g.panel.Dispose();
                    groups.Remove(g);
                    PositionGroups(false);
                    return;
                }
            }
        }

        ConditionLists result;
        private string Check()
        {
            string errorlist = "";
            result = new ConditionLists();

            foreach (Group g in groups)
            {
                if (g.usercontrol == null)
                {
                    errorlist += "Ignored group with empty name" + Environment.NewLine;
                }
                else
                {
                    Condition c = g.usercontrol.cd;

                    if (c.eventname.Length == 0)
                        errorlist += "Event does not have an event name defined" + Environment.NewLine;
                    else if (c.action.Equals("New") || c.action.Length == 0)        // actions, but not selected one..
                        errorlist += "Event " + g.usercontrol.ID() + " does not have an action program defined" + Environment.NewLine;
                    else if (c.fields == null || c.fields.Count == 0)
                        errorlist += "Event " + g.usercontrol.ID() + " does not have a condition" + Environment.NewLine;
                    else
                        result.Add(g.usercontrol.cd);
                }
            }

            return errorlist;
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

        private void Grouptype_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExtendedControls.ComboBoxCustom b = sender as ExtendedControls.ComboBoxCustom;
            Group g = (Group)b.Tag;
            Condition c = new Condition("", "", "", new List<ConditionEntry>() { new ConditionEntry("Condition", ConditionEntry.MatchType.AlwaysTrue, "") });
            CreateUserControl(g, c);
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
                                onAdditionalNames(""), actionfile.name, p, 
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
            DragMoveMode((Control)sender);
        }

    }
}
