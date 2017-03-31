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
using EDDiscovery;
using EDDiscovery.Actions;
using EDDiscovery.DB;
using EDDiscovery.Win32Constants;
using EDDiscovery2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery2
{
    public partial class ConditionFilterForm : Form
    {
        public ConditionLists result;
        public ConditionVariables userglobalvariables;

        EDDiscovery.Actions.ActionFileList actionfilelist;      // list of vars to list, normally global vars
        List<string> eventlist;
        List<string> additionalfieldnames;          // must be set
        string initialtitle;
        EDDiscoveryForm discoveryform;
        bool allowoutercond;

        bool IsActionsActive { get { return actionfilelist != null; } }

        class Group 
        {
            public Panel panel;
            public ExtendedControls.ButtonExt upbutton;
            public ExtendedControls.ComboBoxCustom evlist;
            public ExtendedControls.ComboBoxCustom actionlist;
            public ExtendedControls.ButtonExt actionconfig;
            public ExtendedControls.TextBoxBorder actionparas;
            public ExtendedControls.ComboBoxCustom innercond;
            public ExtendedControls.ComboBoxCustom outercond;
            public Label outerlabel;

            public class Conditions
            {
                public ExtendedControls.AutoCompleteTextBox fname;
                public ExtendedControls.ComboBoxCustom cond;
                public ExtendedControls.TextBoxBorder value;
                public ExtendedControls.ButtonExt del;
                public ExtendedControls.ButtonExt more;
                public Group group;
            };

            public List<Conditions> condlist = new List<Conditions>();
        };

        class GroupSort1 : IComparer<Group>
        {
            public int Compare(Group our, Group other)
            {
                int r = 1;
                if (other.evlist != null && our.evlist != null)
                {
                    r = our.evlist.Text.CompareTo(other.evlist.Text);
                    if (r == 0 && our.actionparas != null)
                        r = our.actionparas.Text.CompareTo(other.actionparas.Text);
                    if (r == 0 && our.condlist.Count > 0 && other.condlist.Count > 0)
                        r = our.condlist[0].value.Text.CompareTo(other.condlist[0].value.Text);
                }
                else if (our.condlist.Count > 0 && other.condlist.Count > 0)
                    r= our.condlist[0].fname.Text.CompareTo(other.condlist[0].fname.Text);

                return r;
            }
        }
        class GroupSort2 : IComparer<Group>
        {
            public int Compare(Group our, Group other)
            {
                int r = 1;

                if (other.evlist != null && our.evlist != null)
                {
                    r = our.evlist.Text.CompareTo(other.evlist.Text);
                    if (r == 0 && our.condlist.Count > 0 && other.condlist.Count > 0)
                        r = our.condlist[0].value.Text.CompareTo(other.condlist[0].value.Text);
                    if (r == 0 && our.actionparas != null)
                        r = our.actionparas.Text.CompareTo(other.actionparas.Text);
                }
                else if (our.condlist.Count > 0 && other.condlist.Count > 0)
                    r = our.condlist[0].fname.Text.CompareTo(other.condlist[0].fname.Text);

                return r;
            }
        }

        List<Group> groups;
        public int condxoffset;     // where conditions start in x, estimate in Init

        const int panelxmargin = 3;
        const int panelymargin = 1;
        const int conditionhoff = 26;

        public ConditionFilterForm()
        {
            InitializeComponent();
            groups = new List<Group>();
            CancelButton = buttonCancel;
            AcceptButton = buttonOK;
        }

        // used to start when just filtering.. uses a fixed event list

        public void InitFilter(string t, List<string> varfields, EDDiscoveryForm frm, ConditionLists j = null)
        {
            List<string> events = EDDiscovery.EliteDangerous.JournalEntry.GetListOfEventsWithOptMethod(false);
            events.Add("All");
            Init(t, events, varfields, true, frm);
            LoadConditions(j);
        }

        // used to start when inside a condition of an IF of a program action

        public void InitCondition(string t, List<string> varfields, EDDiscoveryForm frm, ConditionLists j = null)
        {
            Init(t, null, varfields, true, frm);
            LoadConditions(j);
        }

        public void InitAction(string t, List<string> el, List<string> varfields , ConditionVariables ug, 
                            EDDiscovery.Actions.ActionFileList acfilelist , EDDiscoveryForm frm )
        {
            actionfilelist = acfilelist;
            userglobalvariables = ug;
            Init(t, el, varfields, false, frm);
            LoadConditions(acfilelist.CurConditions);
        }

        // Full start

        public void Init(string t, List<string> el,                             // list of event types or null if event types not used
                                   List<string> varfields,                      // list of additional variable/field names (must be set)
                                   bool outerconditions,                        // outc selects if group outer action can be selected, else its OR
                                   EDDiscoveryForm form)
        {
            discoveryform = form;
            eventlist = el;
            additionalfieldnames = varfields;
            allowoutercond = outerconditions;

            // set up where a condition would start on the panel.. dep if the event list is on and the action file system is on..
            // sizes are the sizes of the controls and gaps
            condxoffset = ((eventlist != null) ? (150 + 8) : 0) + ((actionfilelist != null) ? (140 + 8 + 24 + 8 + 96 + 8) : 0) + panelxmargin + 8;

            bool winborder = discoveryform.theme.ApplyToForm(this, SystemFonts.DefaultFont);
            statusStripCustom.Visible = panelTop.Visible = panelTop.Enabled = !winborder;
            initialtitle = this.Text = label_index.Text = t;

            if (!IsActionsActive)        // turn off these if not in use..
            {
                buttonExtGlobals.Visible = labelProgSet.Visible = comboBoxCustomProgSet.Visible = labelEditProg.Visible =
                        comboBoxCustomEditProg.Visible = checkBoxCustomSetEnabled.Visible = false;
            }
            else
            {
                panelOK.ContextMenuStrip = this.contextMenuStripBottom;
            }
        }

        public void LoadConditions( ConditionLists clist )
        {
            if (clist != null)
            {
                SuspendLayout();
                this.panelOuter.SuspendLayout();
                panelVScroll.SuspendLayout();

                panelVScroll.RemoveAllControls( new List<Control> { buttonMore, buttonSort, buttonSort2 } );            // except these!

                groups = new List<Group>();

                foreach (ConditionLists.Condition fe in clist.conditionlist)
                {
                    Group g = CreateGroupInternal(fe.eventname, fe.action, fe.actiondata, fe.innercondition.ToString(), fe.outercondition.ToString());

                    foreach (ConditionLists.ConditionEntry f in fe.fields)
                        CreateConditionInt(g, f.itemname, ConditionLists.MatchNames[(int)f.matchtype], f.matchstring);

                    discoveryform.theme.ApplyToControls(g.panel, SystemFonts.DefaultFont);

                    groups.Add(g);
                }

                System.Runtime.GCLatencyMode lm = System.Runtime.GCSettings.LatencyMode;
                System.Runtime.GCSettings.LatencyMode = System.Runtime.GCLatencyMode.LowLatency;

                foreach (Group g in groups)                         // FURTHER investigation.. when VScroll has been used after the swap, this and fix groups takes ages
                    panelVScroll.Controls.Add(g.panel);             // why? tried a brand new vscroll.  Tried a lot of things.  Is it GC?

                FixUpGroups();                                      // this takes ages too on second load..

                panelVScroll.ResumeLayout();
                this.panelOuter.ResumeLayout();
                ResumeLayout();

                System.Runtime.GCSettings.LatencyMode = lm;
            }

            this.Text = label_index.Text = initialtitle + " (" + groups.Count.ToString() + " conditions)";
        }

        private void panelVScroll_Resize(object sender, EventArgs e)
        {
            FixUpGroups(false);

        }

        #region Groups

        private void buttonMore_Click(object sender, EventArgs e)       // main + button
        {
            SuspendLayout();
            panelVScroll.SuspendLayout();

            Group g;

            if (IsActionsActive && groups.Count > 0)
            {
                Group prev = groups.Last();
                g = CreateGroupInternal(null, prev.actionlist.Text, prev.actionparas.Text, prev.innercond.Text, prev.outercond.Text);

                if (prev.condlist.Count > 0)
                    CreateConditionInt(g, prev.condlist[0].fname.Text, prev.condlist[0].cond.Text, prev.condlist[0].value.Text);
            }
            else
            {
                g = CreateGroupInternal(null, null, null, null, null);

                if (eventlist == null)      // if we don't have any event list, auto create a condition
                    CreateConditionInt(g, null, null, null);
            }

            discoveryform.theme.ApplyToControls(g.panel, SystemFonts.DefaultFont);

            groups.Add(g);
            panelVScroll.Controls.Add(g.panel);

            FixUpGroups();

            this.Text = label_index.Text = initialtitle + " (" + groups.Count.ToString() + " conditions)";

            panelVScroll.ResumeLayout();
            ResumeLayout();

            panelVScroll.ToEnd();       // tell it to scroll to end
        }

        private void buttonSort_Click(object sender, EventArgs e) // sort button
        {
            groups.Sort(new GroupSort1());
            FixUpGroups();
        }

        private void buttonSort2_Click(object sender, EventArgs e)
        {
            groups.Sort(new GroupSort2());
            FixUpGroups();
        }

        Group CreateGroupInternal(string initialev, string initialaction, string initialactiondatastring,
                                string initialcondinner, string initialcondouter)
        {
            Group g = new Group();

            g.panel = new Panel();
            g.panel.SuspendLayout();

            if (eventlist != null)
            {
                g.evlist = new ExtendedControls.ComboBoxCustom();
                g.evlist.Items.AddRange(eventlist);
                g.evlist.Location = new Point(panelxmargin, panelymargin);
                g.evlist.Size = new Size(150, 24);
                g.evlist.DropDownHeight = 400;
                if (initialev != null && initialev.Length > 0)
                    g.evlist.Text = initialev;
                g.evlist.SelectedIndexChanged += Evlist_SelectedIndexChanged;
                g.evlist.Tag = g;
                g.panel.Controls.Add(g.evlist);
            }

            if (IsActionsActive)
            {
                bool v = initialaction != null || g.evlist.Text.Length > 0;

                g.actionlist = new ExtendedControls.ComboBoxCustom();
                g.actionlist.Location = new Point(g.evlist.Right + 8, panelymargin);
                g.actionlist.DropDownHeight = 400;
                g.actionlist.Size = new Size(140, 24);
                g.actionlist.Items.Add("New");
                g.actionlist.Items.AddRange(actionfilelist.CurPrograms.GetActionProgramList());
                if (initialaction != null)
                    g.actionlist.Text = initialaction;
                else
                    g.actionlist.SelectedIndex = 0;
                g.actionlist.Visible = v;
                g.actionlist.SelectedIndexChanged += ActionList_SelectedIndexChanged;
                g.actionlist.Tag = g;
                g.panel.Controls.Add(g.actionlist);

                g.actionconfig = new ExtendedControls.ButtonExt();
                g.actionconfig.Text = "P";
                g.actionconfig.Location = new Point(g.actionlist.Right + 8, panelymargin);
                g.actionconfig.Size = new Size(24, 24);
                g.actionconfig.Click += ActionListConfig_Clicked;
                g.actionconfig.Visible = v;
                g.actionconfig.Tag = g;
                g.panel.Controls.Add(g.actionconfig);

                g.actionparas = new ExtendedControls.TextBoxBorder();
                g.actionparas.Text = (initialactiondatastring != null) ? initialactiondatastring : "";
                g.actionparas.Location = new Point(g.actionconfig.Right + 8, panelymargin+2);
                g.actionparas.Size = new Size(96, 24);
                g.actionparas.Visible = v;
                g.actionparas.Tag = g;
                g.actionparas.ReadOnly = true;
                g.actionparas.Click += Actionparas_Click;
                g.panel.Controls.Add(g.actionparas);
            }

            g.innercond = new ExtendedControls.ComboBoxCustom();
            g.innercond.Items.AddRange(Enum.GetNames(typeof(ConditionLists.LogicalCondition)));
            g.innercond.SelectedIndex = 0;
            g.innercond.Size = new Size(48, 24);
            g.innercond.Visible = false;
            if ( initialcondinner != null)
                g.innercond.Text = initialcondinner;
            g.panel.Controls.Add(g.innercond);

            g.outercond = new ExtendedControls.ComboBoxCustom();
            g.outercond.Items.AddRange(Enum.GetNames(typeof(ConditionLists.LogicalCondition)));
            g.outercond.SelectedIndex = 0;
            g.outercond.Size = new Size(60, 24);
            g.outercond.Enabled = g.outercond.Visible = false;
            if (initialcondouter != null)
                g.outercond.Text = initialcondouter;
            g.panel.Controls.Add(g.outercond);

            g.outerlabel = new Label();
            g.outerlabel.Text = " with group(s) above";
            g.outerlabel.AutoSize = true;
            g.outerlabel.Visible = false;
            g.panel.Controls.Add(g.outerlabel);

            g.upbutton = new ExtendedControls.ButtonExt();
            g.upbutton.Size = new Size(24, 24);
            g.upbutton.Text = "^";
            g.upbutton.Click += Up_Click;
            g.upbutton.Tag = g;
            g.panel.Controls.Add(g.upbutton);

            g.panel.ResumeLayout();

            return g;
        }


        private void Evlist_SelectedIndexChanged(object sender, EventArgs e)                // EVENT list changed
        {
            ExtendedControls.ComboBoxCustom b = sender as ExtendedControls.ComboBoxCustom;
            Group g = (Group)b.Tag;

            if ( g.condlist.Count == 0 )        // if no conditions, create one..
            {
                if ( g.evlist.Text.Equals("onKeyPress"))                    // special fill in for some events..
                    CreateCondition(g,"KeyPress", "== (Str)","");
                else
                    CreateCondition(g);
            }

            if (IsActionsActive)
                g.actionconfig.Visible = g.actionlist.Visible = g.actionparas.Visible = true;        // enable action list visibility if its enabled.. enabled was set when created to see if its needed

            FixUpGroups();                      // and reposition and maybe turn on/off the group outer cond
        }

        private void Up_Click(object sender, EventArgs e)
        {
            ExtendedControls.ButtonExt b = sender as ExtendedControls.ButtonExt;
            Group g = (Group)b.Tag;
            int indexof = groups.IndexOf(g);
            groups.Remove(g);
            groups.Insert(indexof - 1, g);
            FixUpGroups();
        }

        #endregion

        #region Condition

        void CreateCondition(Group g, string initialfname = null, string initialcond = null, string initialvalue = null )
        {
            CreateConditionInt(g, initialfname, initialcond, initialvalue);
            discoveryform.theme.ApplyToControls(g.panel, SystemFonts.DefaultFont);
            FixUpGroups();
        }

        void CreateConditionInt( Group g , string initialfname, string initialcond, string initialvalue)
        {
            g.panel.SuspendLayout();

            Group.Conditions c = new Group.Conditions();

            c.fname = new ExtendedControls.AutoCompleteTextBox();
            c.fname.Size = new Size(140, 24);
            c.fname.SetAutoCompletor(AutoCompletor);
            c.fname.Tag = g;
            c.fname.DropDownWidth = 200;
            if (initialfname != null)
                c.fname.Text = initialfname;
            g.panel.Controls.Add(c.fname);                                                // 1st control

            c.cond = new ExtendedControls.ComboBoxCustom();
            c.cond.Items.AddRange(ConditionLists.MatchNames);
            c.cond.SelectedIndex = 0;
            c.cond.Size = new Size(140, 24);
            c.cond.DropDownHeight = 400;
            c.cond.Tag = g;

            if (initialcond != null)
                c.cond.Text = initialcond.SplitCapsWord();

            c.cond.SelectedIndexChanged += Cond_SelectedIndexChanged; // and turn on handler

            g.panel.Controls.Add(c.cond);         // must be next

            c.value = new ExtendedControls.TextBoxBorder();

            if (initialvalue != null)
                c.value.Text = initialvalue;

            g.panel.Controls.Add(c.value);         // must be next

            c.del = new ExtendedControls.ButtonExt();
            c.del.Size = new Size(24, 24);
            c.del.Text = "X";
            c.del.Click += ConditionDelClick;
            c.del.Tag = c;
            g.panel.Controls.Add(c.del);

            c.more = new ExtendedControls.ButtonExt();
            c.more.Size = new Size(24, 24);
            c.more.Text = "+";
            c.more.Click += NewConditionClick;
            c.more.Tag = g;
            g.panel.Controls.Add(c.more);

            c.group = g;
            g.condlist.Add(c);

            g.panel.ResumeLayout();
        }


        private void Cond_SelectedIndexChanged(object sender, EventArgs e)          // on condition changing, see if value is needed 
        {
            FixUpGroups();
        }

        private void ConditionDelClick(object sender, EventArgs e)
        {
            ExtendedControls.ButtonExt b = sender as ExtendedControls.ButtonExt;
            Group.Conditions c = (Group.Conditions)b.Tag;
            Group g = c.group;

            g.panel.Controls.Remove(c.fname);
            g.panel.Controls.Remove(c.cond);
            g.panel.Controls.Remove(c.value);
            g.panel.Controls.Remove(c.more);
            g.panel.Controls.Remove(c.del);

            g.condlist.Remove(c);

            if ( g.condlist.Count == 0 )
            {
                panelVScroll.Controls.Remove(g.panel);
                g.panel.Controls.Clear();
                groups.Remove(g);
            }

            FixUpGroups();
        }

        private void NewConditionClick(object sender, EventArgs e)
        {
            ExtendedControls.ButtonExt b = sender as ExtendedControls.ButtonExt;
            Group g = (Group)b.Tag;
            CreateCondition(g);
        }

        #endregion


        #region Positioning

        void FixUpGroups(bool calcminsize = true)      // fixes and positions groups.
        {
            SuspendLayout();
            panelVScroll.SuspendLayout();

            int panelwidth = Math.Max(panelVScroll.Width - panelVScroll.ScrollBarWidth, 10);
            int y = panelymargin;

            for (int i = 0; i < groups.Count; i++)
            {
                Group g = groups[i];
                g.panel.SuspendLayout();

                // for all groups, see if another group below it has the same event selected as ours

                bool showouter = false;                     

                if (eventlist != null)
                {
                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (groups[j].evlist.Text.Equals(groups[i].evlist.Text) && groups[i].evlist.Text.Length > 0)
                            showouter = true;
                    }

                    showouter &= allowoutercond;                // qualify with outer condition switch
                }
                else
                    showouter = (i > 0) && allowoutercond;       // and enabled/disable the outer condition switch

                g.outercond.Enabled = g.outercond.Visible = g.outerlabel.Visible = showouter;       // and enabled/disable the outer condition switch

                // Now position the conditions inside the panel

                int vnextcond = panelymargin;

                int farx = (g.evlist!= null) ? (g.evlist.Right-g.innercond.Width+8) : 0 ;   // innercond cause below adds it back on

                for (int condc = 0; condc < g.condlist.Count; condc++)
                {
                    Group.Conditions c = g.condlist[condc];
                    c.fname.Location = new Point(condxoffset, vnextcond + 2);
                    c.fname.Enabled = !ConditionLists.IsNullOperation(c.cond.Text);
                    if (!c.fname.Enabled)
                        c.fname.Text = "";

                    c.cond.Location = new Point(c.fname.Right + 4, vnextcond);

                    c.value.Location = new Point(c.cond.Right + 4, vnextcond + 2);
                    c.value.Size = new Size(panelwidth - condxoffset - c.fname.Width - 4 - c.cond.Width - 4 - c.del.Width - 4 - c.more.Width - 4 - g.innercond.Width - 4 - g.upbutton.Width + 8, 24);
                    c.value.Enabled = !ConditionLists.IsNullOperation(c.cond.Text) && !ConditionLists.IsUnaryOperation(c.cond.Text);
                    if (!c.value.Enabled)
                        c.value.Text = "";

                    c.del.Location = new Point(c.value.Right + 4, vnextcond);
                    c.more.Location = new Point(c.del.Right + 4, vnextcond);
                    c.more.Visible = (condc == g.condlist.Count - 1);

                    vnextcond += conditionhoff;
                    farx = c.more.Left + 4;     // where the more button is
                }

                // and the outer/inner conditions

                g.innercond.Visible = (g.condlist.Count > 1);       // inner condition on if multiple
                g.innercond.Location = new Point(farx, panelymargin);    // inner condition is in same place as more button
                farx = g.innercond.Right + 4;                       // move off    

                // and the up button.. 
                g.upbutton.Location = new Point(farx, panelymargin);
                g.upbutton.Visible = (i != 0);
                farx = g.upbutton.Right;

                // allocate space for the outercond if req.
                if (g.outercond.Enabled)
                {
                    g.outercond.Location = new Point(panelxmargin, vnextcond);
                    g.outerlabel.Location = new Point(g.outercond.Location.X + g.outercond.Width + 4, g.outercond.Location.Y + 3);
                    vnextcond += conditionhoff;
                }

                // pos the panel

                g.panel.Location = new Point(panelxmargin, y + panelVScroll.ScrollOffset);
                g.panel.Size = new Size(Math.Max(panelwidth - panelxmargin * 2, farx), Math.Max(vnextcond, panelymargin + conditionhoff));
                g.panel.BorderStyle = (g.condlist.Count > 1) ? BorderStyle.FixedSingle : BorderStyle.None;

                y += g.panel.Height + 2;

                // and make sure actions list is right

                if (IsActionsActive)     
                {
                    string name = g.actionlist.Text;
                    g.actionlist.Enabled = false;
                    g.actionlist.Items.Clear();
                    g.actionlist.Items.Add("New");
                    g.actionlist.Items.AddRange(actionfilelist.CurPrograms.GetActionProgramList());

                    if (g.actionlist.Items.Contains(name))
                        g.actionlist.SelectedItem = name;
                    else
                        g.actionlist.SelectedIndex = 0;

                    g.actionlist.Enabled = true;
                    g.actionparas.Enabled = g.actionconfig.Enabled = g.actionlist.SelectedIndex != 0;
                }

                g.panel.ResumeLayout();
            }

            if (IsActionsActive)
            {
                comboBoxCustomEditProg.Items.Clear();
                List<string> entries = actionfilelist.CurPrograms.GetActionProgramList(true).ToList();
                entries.Sort();
                comboBoxCustomEditProg.Items.AddRange(entries.ToArray());
                comboBoxCustomEditProg.Items.Add("New");

                comboBoxCustomProgSet.Items.Clear();
                comboBoxCustomProgSet.Items.AddRange(actionfilelist.GetList);
                comboBoxCustomProgSet.Items.Add("New");
                comboBoxCustomProgSet.Enabled = false;
                comboBoxCustomProgSet.SelectedItem = actionfilelist.CurName;
                comboBoxCustomProgSet.Enabled = true;

                checkBoxCustomSetEnabled.Checked = actionfilelist.CurEnabled;
            }

            buttonMore.Location = new Point(panelxmargin, y + panelVScroll.ScrollOffset);
            buttonSort.Location = new Point(buttonMore.Right + 8, y + panelVScroll.ScrollOffset);
            buttonSort2.Location = new Point(buttonSort.Right + 8, y + panelVScroll.ScrollOffset);

            Rectangle screenRectangle = RectangleToScreen(this.ClientRectangle);
            int titleHeight = screenRectangle.Top - this.Top;

            y += buttonMore.Height + titleHeight + ((panelTop.Enabled) ? (panelTop.Height + statusStripCustom.Height) : 8) + 16 + panelOK.Height;

            if (calcminsize)
            {
                this.MinimumSize = new Size(1000, y);
                this.MaximumSize = new Size(Screen.FromControl(this).WorkingArea.Width - 100, Screen.FromControl(this).WorkingArea.Height - 100);

                if (Bottom > Screen.FromControl(this).WorkingArea.Height)
                    Top = Screen.FromControl(this).WorkingArea.Height - Height - 50;
            }

            panelVScroll.ResumeLayout();
            ResumeLayout();
        }

        #endregion

        #region Action List programs

        private void ActionList_SelectedIndexChanged(object sender, EventArgs e)          // on action changing, do its configuration menu
        {
            ExtendedControls.ComboBoxCustom aclist = sender as ExtendedControls.ComboBoxCustom;
            Group g = (Group)aclist.Tag;

            if (aclist.Enabled && aclist.SelectedIndex == 0)   // if selected NEW.
            {
                ActionListConfig_Clicked(g.actionconfig, null);
            }

            g.actionparas.Enabled = g.actionconfig.Enabled = g.actionlist.SelectedIndex != 0;
        }

        private void ActionListConfig_Clicked(object sender, EventArgs e)
        {
            ExtendedControls.ButtonExt config = sender as ExtendedControls.ButtonExt;
            Group g = (Group)config.Tag;

            ActionProgram p = null;

            if (g.actionlist.SelectedIndex > 0)     // exclude NEW from checking for program
                p = actionfilelist.CurPrograms.Get(g.actionlist.Text);

            if (p != null && p.StoredInFile != null)
            {
                p.EditInEditor(p.StoredInFile);         // Edit in the editor.. this also updated the program steps held internally
            }
            else
            {
                string suggestedname = null;

                if (p == null)        // if no program, create a new suggested name and clear any action data
                {
                    suggestedname = g.evlist.Text;
                    int n = 2;
                    while (actionfilelist.CurPrograms.GetActionProgramList().Contains(suggestedname))
                    {
                        suggestedname = g.evlist.Text + "_" + n.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        n++;
                    }

                    g.actionparas.Text = "";
                }

                ActionProgramForm apf = new ActionProgramForm();
                apf.EditProgram += EditProgram;

                // we init with a variable list based on the field names of the group (normally the event field names got by SetFieldNames)
                // pass in the program if found, and its action data.

                List<string> fieldnames = new List<string>();
                if (g.evlist != null && g.evlist.Text.Length > 0)
                {
                    EnsureCached(g.evlist.Text);
                    fieldnames = cachedeventsdecorated[g.evlist.Text];
                }
                else
                    fieldnames.AddRange(additionalfieldnames);

                apf.Init("Action program", discoveryform, fieldnames, actionfilelist.CurName, p, actionfilelist.CurPrograms.GetActionProgramList(), suggestedname);

                DialogResult res = apf.ShowDialog();

                if (res == DialogResult.OK)
                {
                    ActionProgram np = apf.GetProgram();
                    actionfilelist.CurPrograms.AddOrChange(np);                // replaces or adds (if its a new name) same as rename
                    g.actionlist.Enabled = false;
                    g.actionlist.Text = np.Name;
                    g.actionlist.Enabled = true;
                }
                else if (res == DialogResult.Abort)   // delete
                {
                    ActionProgram np2 = apf.GetProgram();
                    actionfilelist.CurPrograms.Delete(np2.Name);
                }
            }

            FixUpGroups();       // run  this, it sorts out the group names
        }

        private void comboBoxCustomProgSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCustomProgSet.Enabled)
            {
                string newname = comboBoxCustomProgSet.Text;
                bool newfile = newname.Equals("New");

                if (newfile || !newname.Equals(actionfilelist.CurName))
                {
                    if (CheckAndAsk())            // save current, and its not asked for a retry.. we have now saved this page
                    {
                        if (newfile)
                        {
                            newname = EDDiscovery.Forms.PromptSingleLine.ShowDialog(this, "Enter name of new action file", "", "Action File");

                            if (newname != null)
                            {
                                newname = newname.SafeFileString();

                                if (actionfilelist.GetList.Contains(newname))
                                {
                                    EDDiscovery.Forms.MessageBoxTheme.Show(this, "Set already exists");
                                    return;
                                }
                                else
                                    actionfilelist.CreateSet(newname);
                            }
                        }

                        if (newname != null && actionfilelist.SelectCurrent(newname))
                        {
                            LoadConditions(actionfilelist.CurConditions);
                        }
                    }
                }
            }
        }

        private void comboBoxCustomEditProg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCustomEditProg.Enabled)
            {
                string progname = comboBoxCustomEditProg.Text.Replace(" (Ext)","");     // remove the EXT marker

                ActionProgram p = null;
                if (!progname.Equals("New"))
                    p = actionfilelist.CurPrograms.Get(progname);

                if ( p != null && p.StoredInFile != null )
                {
                    p.EditInEditor(p.StoredInFile);         // Edit in the editor..
                }
                else
                {
                    ActionProgramForm apf = new ActionProgramForm();
                    apf.EditProgram += EditProgram;
                    apf.Init("Action program", discoveryform, additionalfieldnames, actionfilelist.CurName, p, actionfilelist.CurPrograms.GetActionProgramList(), "", ModifierKeys.HasFlag(Keys.Shift));

                    DialogResult res = apf.ShowDialog();

                    if (res == DialogResult.OK)
                    {
                        ActionProgram np = apf.GetProgram();
                        actionfilelist.CurPrograms.AddOrChange(np);                // replaces or adds (if its a new name) same as rename
                    }
                    else if (res == DialogResult.Abort)   // delete
                    {
                        ActionProgram np2 = apf.GetProgram();
                        actionfilelist.CurPrograms.Delete(np2.Name);
                    }
                }

                FixUpGroups();       // run  this, it sorts out the group names
            }
        }

        private void Actionparas_Click(object sender, EventArgs e)
        {
            ExtendedControls.TextBoxBorder config = sender as ExtendedControls.TextBoxBorder;
            Group g = (Group)config.Tag;
            ConditionVariables cond = new ConditionVariables();

            string flag;
            cond.FromActionDataString(g.actionparas.Text, out flag);

            ConditionVariablesForm avf = new ConditionVariablesForm();
            avf.Init("Input parameters and flags to pass to program on run", discoveryform.theme, cond, showone: true, showrefresh: true, showrefreshstate: flag.Equals(ConditionVariables.flagRunAtRefresh));

            if (avf.ShowDialog(this) == DialogResult.OK)
            {
                g.actionparas.Text = avf.result.ToActionDataString(avf.result_refresh ? ConditionVariables.flagRunAtRefresh : "");
            }
        }

        private void buttonExtGlobals_Click(object sender, EventArgs e)
        {
            ConditionVariablesForm avf = new ConditionVariablesForm();
            avf.Init("Global User variables to pass to program on run", discoveryform.theme, userglobalvariables, showone:true);

            if (avf.ShowDialog(this) == DialogResult.OK)
            {
                userglobalvariables = avf.result;

                foreach (string key in userglobalvariables.NameEnumuerable)     // add them in in case..
                {
                    if (!additionalfieldnames.Contains(key))
                        additionalfieldnames.Add(key);
                }

            }
        }

        private void EditProgram(string s)  // Callback by APF to ask to edit another program..
        {
            Tuple<ActionFile, ActionProgram> p = actionfilelist.FindProgram(s, actionfilelist.CurFile);

            if (p != null)
            {
                if (p.Item2.StoredInFile != null)
                {
                    p.Item2.EditInEditor(p.Item2.StoredInFile);         // Edit in the editor..
                }
                else
                {
                    ActionProgramForm apf = new ActionProgramForm();
                    apf.EditProgram += EditProgram;

                    apf.Init("Action program", discoveryform, additionalfieldnames, p.Item1.name, p.Item2, p.Item1.actionprogramlist.GetActionProgramList(), "");

                    DialogResult res = apf.ShowDialog();

                    if (res == DialogResult.OK)
                    {
                        ActionProgram np = apf.GetProgram();
                        actionfilelist.CurPrograms.AddOrChange(np);                // replaces or adds (if its a new name) same as rename
                    }
                }
            }
            else
                EDDiscovery.Forms.MessageBoxTheme.Show(this, "Unknown program " + s);
        }

#endregion



#region Checking

        private string Check()
        {
            result = new ConditionLists();

            string errorlist = "";

            foreach (Group g in groups)
            {
                string innerc = g.innercond.Text;
                string outerc = g.outercond.Text;
                string actionname = (IsActionsActive) ? g.actionlist.Text : "Default";
                string actiondata = (IsActionsActive) ? g.actionparas.Text : "Default"; // any associated data from the program
                string evt = (eventlist != null) ? g.evlist.Text : "Default";

                //System.Diagnostics.Debug.WriteLine("Event {0} inner {1} outer {2} action {3} data '{4}'", evt, innerc, outerc, actionname, actiondata );

                ConditionLists.Condition fe = new ConditionLists.Condition();

                if (IsActionsActive && actionname.Equals("New"))        // actions, but not selected one..
                    errorlist += "Event " + evt + " does not have an action program defined" + Environment.NewLine;
                else if (evt.Length == 0)        // must have name
                    errorlist += "Ignored group with empty name" + Environment.NewLine;
                else
                {
                    if (fe.Create(evt, actionname, actiondata, innerc, outerc)) // create must work
                    {
                        for (int i = 0; i < g.condlist.Count; i++)
                        {
                            Group.Conditions c = g.condlist[i];
                            string fieldn = c.fname.Text;
                            string condn = c.cond.Text;
                            string valuen = c.value.Text;

                            if (fieldn.Length > 0 || ConditionLists.IsNullOperation(condn))
                            {
                                ConditionLists.ConditionEntry f = new ConditionLists.ConditionEntry();

                                if (f.Create(fieldn, condn, valuen))
                                {
                                    if (valuen.Length == 0 && !ConditionLists.IsUnaryOperation(condn) && !ConditionLists.IsNullOperation(condn))
                                        errorlist += "Do you want filter '" + fieldn + "' in group '" + fe.eventname + "' to have an empty value" + Environment.NewLine;

                                    fe.Add(f);
                                }
                                else
                                    errorlist += "Cannot create filter '" + fieldn + "' in group '" + fe.eventname + "' check value" + Environment.NewLine;
                            }
                            else
                                errorlist += "Ignored empty filter " + (i+1).ToString(System.Globalization.CultureInfo.InvariantCulture) + " in " + fe.eventname + Environment.NewLine;
                        }

                        if (fe.fields != null)
                            result.Add(fe);
                        else
                            errorlist += "No valid filters found in group '" + fe.eventname + "'" + Environment.NewLine;
                    }
                    else
                        errorlist += "Cannot create " + evt + " not a normal error please report" + Environment.NewLine;
                }
            }

            return errorlist;
        }

        private bool CheckAndAsk()
        {
            string res = Check();

            if (res.Length > 0)
            {
                string acceptstr = "Click Retry to correct errors, Abort to cancel, Ignore to accept valid entries";

                DialogResult dr = EDDiscovery.Forms.MessageBoxTheme.Show(this, "Filters produced the following warnings and errors" + Environment.NewLine + Environment.NewLine + res + Environment.NewLine + acceptstr,
                                                  "Warning", MessageBoxButtons.AbortRetryIgnore );

                if (dr == DialogResult.Retry)
                    return false;
                else if (dr == DialogResult.Abort || dr == DialogResult.Cancel)
                {
                    DialogResult = DialogResult.Cancel;
                    Close();
                    return false;       // closed ourselves..
                }
            }

            if (IsActionsActive)
            {
                actionfilelist.UpdateCurrentCL(result);
                actionfilelist.UpdateCurrentEnabled(checkBoxCustomSetEnabled.Checked);
                actionfilelist.SaveCurrentActionFile();
            }
            return true;
        }


#endregion

#region OK Cancel

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if ( CheckAndAsk() )
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

#endregion

#region Window Control

        // Mono compatibility
        private bool _window_dragging = false;
        private Point _window_dragMousePos = Point.Empty;
        private Point _window_dragWindowPos = Point.Empty;

        private IntPtr SendMessage(int msg, IntPtr wparam, IntPtr lparam)
        {
            Message message = Message.Create(this.Handle, msg, wparam, lparam);
            this.WndProc(ref message);
            return message.Result;
        }

        protected override void WndProc(ref Message m)
        {
            bool windowsborder = this.FormBorderStyle == FormBorderStyle.Sizable;
            // Compatibility movement for Mono
            if (m.Msg == WM.LBUTTONDOWN && (int)m.WParam == 1 && !windowsborder)
            {
                int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                int y = unchecked((short)((uint)m.LParam >> 16));
                _window_dragMousePos = new Point(x, y);
                _window_dragWindowPos = this.Location;
                _window_dragging = true;
                m.Result = IntPtr.Zero;
                this.Capture = true;
            }
            else if (m.Msg == WM.MOUSEMOVE && (int)m.WParam == 1 && _window_dragging)
            {
                int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                int y = unchecked((short)((uint)m.LParam >> 16));
                Point delta = new Point(x - _window_dragMousePos.X, y - _window_dragMousePos.Y);
                _window_dragWindowPos = new Point(_window_dragWindowPos.X + delta.X, _window_dragWindowPos.Y + delta.Y);
                this.Location = _window_dragWindowPos;
                this.Update();
                m.Result = IntPtr.Zero;
            }
            else if (m.Msg == WM.LBUTTONUP)
            {
                _window_dragging = false;
                _window_dragMousePos = Point.Empty;
                _window_dragWindowPos = Point.Empty;
                m.Result = IntPtr.Zero;
                this.Capture = false;
            }
            // Windows honours NCHITTEST; Mono does not
            else if (m.Msg == WM.NCHITTEST)
            {
                base.WndProc(ref m);

                if ((int)m.Result == HT.CLIENT)
                {
                    int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                    int y = unchecked((short)((uint)m.LParam >> 16));
                    Point p = PointToClient(new Point(x, y));

                    if (p.X > this.ClientSize.Width - statusStripCustom.Height && p.Y > this.ClientSize.Height - statusStripCustom.Height)
                    {
                        m.Result = (IntPtr)HT.BOTTOMRIGHT;
                    }
                    else if (p.Y > this.ClientSize.Height - statusStripCustom.Height)
                    {
                        m.Result = (IntPtr)HT.BOTTOM;
                    }
                    else if (p.X > this.ClientSize.Width - 5)       // 5 is generous.. really only a few pixels gets thru before the subwindows grabs them
                    {
                        m.Result = (IntPtr)HT.RIGHT;
                    }
                    else if (p.X < 5)
                    {
                        m.Result = (IntPtr)HT.LEFT;
                    }
                    else if (!windowsborder)
                    {
                        m.Result = (IntPtr)HT.CAPTION;
                    }
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        private void label_index_MouseDown(object sender, MouseEventArgs e)
        {
            ((Control)sender).Capture = false;
            SendMessage(WM.NCLBUTTONDOWN, (IntPtr)HT.CAPTION, IntPtr.Zero);
        }

        private void panel_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region Autocomplete event field types.. complicated

        Dictionary<string, List<string>> cachedevents = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> cachedeventsdecorated = new Dictionary<string, List<string>>();

        List<string> AutoCompletor(string s, ExtendedControls.AutoCompleteTextBox a)
        {
            Group g = a.Tag as Group;

            List<string> list;

            if (g.evlist != null && g.evlist.Text.Length > 0)       // if using event name, cache it
            {
                EnsureCached(g.evlist.Text);
                list = cachedevents[g.evlist.Text];
            }
            else 
            {  // no event name can only give additional field names
                list = new List<string>();
                list.AddRange(additionalfieldnames);
            }

            List<string> ret = new List<string>();

            foreach( string other in list )
            {
                if (other.StartsWith(s, StringComparison.InvariantCultureIgnoreCase))
                    ret.Add(other);
            }

            return ret;
        }

        void EnsureCached(string evtype )
        {
            if (!cachedevents.ContainsKey(evtype))  //evtype MAY not be a real journal event, it may be onStartup..
            {
                cachedevents[evtype] = new List<string>();
                cachedeventsdecorated[evtype] = new List<string>();

                List<string> classnames = Tools.GetPropertyFieldNames(EDDiscovery.EliteDangerous.JournalEntry.TypeOfJournalEntry(evtype));
                if (classnames != null)
                    cachedevents[evtype].AddRange(classnames);

                List<string> classnamesdecorated = Tools.GetPropertyFieldNames(EDDiscovery.EliteDangerous.JournalEntry.TypeOfJournalEntry(evtype), "EventClass_");
                if (classnamesdecorated != null)
                    cachedeventsdecorated[evtype].AddRange(classnamesdecorated);

                cachedevents[evtype].AddRange(additionalfieldnames);
                cachedeventsdecorated[evtype].AddRange(additionalfieldnames);
            }
        }

        #endregion  

        private void configureInstallationValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConditionVariablesForm avf = new ConditionVariablesForm();
            avf.Init("Configuration items for installation - specialist use", discoveryform.theme, actionfilelist.CurInstallationVariables, showone: false);

            if (avf.ShowDialog(this) == DialogResult.OK)
            {
                actionfilelist.UpdateCurrentInstallationVariables(avf.result);
            }
        }
    }
}
