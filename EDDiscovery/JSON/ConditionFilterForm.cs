using EDDiscovery;
using EDDiscovery.Actions;
using EDDiscovery.DB;
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
        public Dictionary<string, string> userglobalvariables;

        EDDiscovery.Actions.ActionFileList actionfilelist;
        bool IsActionsActive { get { return actionfilelist != null;  } }
        List<string> eventlist;
        List<string> additionalfieldnames; // null not have any

        class Group
        {
            public Panel panel;
            public List<string> varnames;
            public ExtendedControls.ButtonExt upbutton;
            public ExtendedControls.ComboBoxCustom evlist;
            public ExtendedControls.ComboBoxCustom actionlist;
            public ExtendedControls.ButtonExt actionconfig;
            public ExtendedControls.ComboBoxCustom innercond;
            public ExtendedControls.ComboBoxCustom outercond;
            public Label outerlabel;

            public string actiondata;
        };

        List<Group> groups;
        bool allowoutercond;
        public int panelwidth;
        public int condxoffset;
        EDDiscovery2.EDDTheme theme;
        const int vscrollmargin = 10;
        const int panelmargin = 3;
        const int conditionhoff = 30;

        public ConditionFilterForm()
        {
            InitializeComponent();
            groups = new List<Group>();
            CancelButton = buttonCancel;
            AcceptButton = buttonOK;
        }

        // used to start when just filtering.. uses a fixed event list

        public void InitFilter(string t, List<string> varfields, EDDiscovery2.EDDTheme th, ConditionLists j = null)
        {
            List<string> events = EDDiscovery.EliteDangerous.JournalEntry.GetListOfEventsWithOptMethod(false);
            events.Add("All");
            Init(t, events, varfields, true, th);
            LoadConditions(j);
        }

        // used to start when inside a condition of an IF of a program action

        public void InitCondition(string t, List<string> varfields, EDDiscovery2.EDDTheme th, ConditionLists j = null)
        {
            Init(t, null, varfields, true, th);
            LoadConditions(j);
        }

        public void InitAction(string t, List<string> el, List<string> varfields , Dictionary<string,string> ug, 
                            EDDiscovery.Actions.ActionFileList acfilelist , EDDiscovery2.EDDTheme th )
        {
            actionfilelist = acfilelist;
            userglobalvariables = ug;
            Init(t, el, varfields, false, th);
            LoadConditions(acfilelist.CurConditions);
        }

        // Full start

        public void Init(string t, List<string> el,                             // list of event types or null if event types not used
                                   List<string> varfields,                      // list of additional variable/field names (must be set)
                                   bool outerconditions,                        // outc selects if group outer action can be selected, else its OR
                                   EDDTheme th)
        {
            theme = th;
            eventlist = el;
            additionalfieldnames = varfields;
            allowoutercond = outerconditions;

            condxoffset = ((eventlist != null) ? 148 : 0) + ((actionfilelist != null) ? (140 + 8 + 24 + 8) : 0) + panelmargin;
            panelwidth = condxoffset + 620;

            bool winborder = theme.ApplyToForm(this, SystemFonts.DefaultFont);
            statusStripCustom.Visible = panelTop.Visible = panelTop.Enabled = !winborder;
            this.Text = label_index.Text = t;

            if (IsActionsActive)        // turn off these if not in use..
            {
                RefreshActionDropDowns();
            }
            else
            {
                labelProgSet.Visible = comboBoxCustomProgSet.Visible = labelEditProg.Visible =
                        comboBoxCustomEditProg.Visible = checkBoxCustomSetEnabled.Visible = false;
            }
        }

        public void LoadConditions( ConditionLists clist )
        {
            if (clist != null)
            {
                foreach (ConditionLists.Condition fe in clist.conditionlist)
                {
                    Group g = CreateGroup(fe.eventname, fe.action, fe.actiondata, fe.innercondition.ToString(), fe.outercondition.ToString());

                    foreach (ConditionLists.ConditionEntry f in fe.fields)
                    {
                        CreateCondition(g, f.itemname, ConditionLists.MatchNames[(int)f.matchtype], f.matchstring);
                    }
                }

                FixUpGroups();      // to move the new button to the correct place if no conditions
            }
        }

        #region Groups

        private void buttonMore_Click(object sender, EventArgs e)       // main + button
        {
            Group g = CreateGroup();

            if (eventlist == null)      // if we don't have any event list, auto create a condition
                CreateCondition(g);
        }

        Group CreateGroup(string initialev = null, string initialaction = null,  string initialactiondatastring = null, 
                                string initialcondinner = null , string initialcondouter = null )
        {
            Group g = new Group();
            g.actiondata = initialactiondatastring;

            g.panel = new Panel();
            g.panel.BorderStyle = BorderStyle.FixedSingle;

            if (eventlist != null)
            {
                g.evlist = new ExtendedControls.ComboBoxCustom();
                g.evlist.Items.AddRange(eventlist);
                g.evlist.Location = new Point(panelmargin, panelmargin);
                g.evlist.Size = new Size(140, 24);
                g.evlist.DropDownHeight = 400;
                g.evlist.Name = "EVList";
                if (initialev != null && initialev.Length > 0)
                    g.evlist.Text = initialev;
                g.evlist.SelectedIndexChanged += Evlist_SelectedIndexChanged;
                g.evlist.Tag = g;
                g.panel.Controls.Add(g.evlist);
            }

            if (IsActionsActive)
            {
                g.actionlist = new ExtendedControls.ComboBoxCustom();
                g.actionlist.Location = new Point(g.evlist.Location.X + g.evlist.Width + 8, panelmargin);
                g.actionlist.DropDownHeight = 400;
                g.actionlist.Size = new Size(140, 24);
                g.actionlist.Name = "ActionList";
                g.actionlist.Items.Add("New");
                g.actionlist.Items.AddRange(actionfilelist.CurPrograms.GetActionProgramList());
                if (initialaction != null)
                    g.actionlist.Text = initialaction;
                else
                    g.actionlist.SelectedIndex = 0;

                g.actionlist.Visible = false;
                g.actionlist.SelectedIndexChanged += ActionList_SelectedIndexChanged;
                g.actionlist.Tag = g;
                g.panel.Controls.Add(g.actionlist);

                g.actionconfig = new ExtendedControls.ButtonExt();
                g.actionconfig.Text = "C";
                g.actionconfig.Location = new Point(g.actionlist.Location.X + g.actionlist.Width + 8, panelmargin);
                g.actionconfig.Size = new Size(24, 24);
                g.actionconfig.Click += ActionListConfig_Clicked;
                g.actionconfig.Enabled = g.actionlist.SelectedIndex != 0;
                g.actionconfig.Visible = false;
                g.actionconfig.Tag = g;
                g.panel.Controls.Add(g.actionconfig);
            }

            g.innercond = new ExtendedControls.ComboBoxCustom();
            g.innercond.Items.AddRange(Enum.GetNames(typeof(ConditionLists.LogicalCondition)));
            g.innercond.SelectedIndex = 0;
            g.innercond.Size = new Size(60, 24);
            g.innercond.Visible = false;
            g.innercond.Name = "InnerCond";
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
            g.outercond.Name = "OuterCond";
            g.panel.Controls.Add(g.outercond);

            g.outerlabel = new Label();
            g.outerlabel.Text = " with group(s) above";
            g.outerlabel.AutoSize = true;
            g.outerlabel.Visible = false;
            g.panel.Controls.Add(g.outerlabel);

            g.upbutton = new ExtendedControls.ButtonExt();
            g.upbutton.Location = new Point(panelwidth - 20 - panelmargin - 4, panelmargin);
            g.upbutton.Size = new Size(24, 24);
            g.upbutton.Text = "^";
            g.upbutton.Click += Up_Click;
            g.upbutton.Tag = g;
            g.panel.Controls.Add(g.upbutton);

            g.panel.Size = new Size(panelwidth, panelmargin + conditionhoff);

            groups.Add(g);

            panelVScroll.Controls.Add(g.panel);

            SetFieldNames(g);

            if (g.actionconfig != null)
            {
                g.actionconfig.Visible = g.actionlist.Visible = (g.actionlist.Enabled && (eventlist == null || g.evlist.Text.Length > 0));        // enable action list visibility if its enabled.. enabled was set when created to see if its needed
            }

            theme.ApplyToControls(g.panel, SystemFonts.DefaultFont);
            RepositionGroupInternals(g);
            FixUpGroups();

            return g;
        }

        private void Evlist_SelectedIndexChanged(object sender, EventArgs e)                // EVENT list changed
        {
            ExtendedControls.ComboBoxCustom b = sender as ExtendedControls.ComboBoxCustom;
            Group g = (Group)b.Tag;

            bool onefieldpresent = false;
            foreach (Control c in g.panel.Controls)
            {
                if (c.Name.Equals("Field"))                 // find if any conditions are on screen..
                {
                    onefieldpresent = true;
                    break;
                }
            }

            if (!onefieldpresent)                           // if not, display one
                CreateCondition(g);

            if ( g.actionconfig != null )
                g.actionconfig.Visible = g.actionlist.Visible = g.actionlist.Enabled;        // enable action list visibility if its enabled.. enabled was set when created to see if its needed

            SetFieldNames(g);
            FixUpGroups();
        }

        private void SetFieldNames(Group g)
        {
            g.varnames = new List<string>();

            if (eventlist != null )       // fieldnames are null, and we have an event list, try and find the field names
            {
                string evtype = g.evlist.Text;

                List<EDDiscovery.EliteDangerous.JournalEntry> jel = EDDiscovery.EliteDangerous.JournalEntry.Get(evtype);        // get all events of this type

                if (jel != null)            // may not find it, if event is not in history
                {
                    Dictionary<string, string> vars = new Dictionary<string, string>();
                    foreach (EDDiscovery.EliteDangerous.JournalEntry ev in jel)
                        JSONHelper.GetJSONFieldNamesValues(ev.EventDataString, vars);        // for all events, add to field list
                    g.varnames.AddRange(vars.Keys.ToList());
                }
            }

            if (additionalfieldnames != null)
                g.varnames.AddRange(additionalfieldnames);
            if (userglobalvariables != null)
                g.varnames.AddRange(userglobalvariables.Keys.ToList());

            foreach (Control c in g.panel.Controls)
            {
                if (c.Name.Equals("Field"))         // update all the field controls to have an up to date field name list for this event
                {
                    ExtendedControls.ComboBoxCustom cb = c as ExtendedControls.ComboBoxCustom;
                    cb.Items.Clear();
                    if (g.varnames != null)
                        cb.Items.AddRange(g.varnames);
                    cb.Items.Add("User Defined");
                    cb.SelectedIndex = -1;
                    cb.Text = "";
                }
            }
        }

        private void ActionList_SelectedIndexChanged(object sender, EventArgs e)          // on action changing, do its configuration menu
        {
            ExtendedControls.ComboBoxCustom aclist = sender as ExtendedControls.ComboBoxCustom;
            Group g = (Group)aclist.Tag;

            if (aclist.Enabled && aclist.SelectedIndex == 0 )   // if selected NEW.
            {
                ActionListConfig_Clicked(g.actionconfig, null);
            }

            g.actionconfig.Enabled = g.actionlist.SelectedIndex != 0;
        }

        private void ActionListConfig_Clicked(object sender, EventArgs e)
        {
            ExtendedControls.ButtonExt config = sender as ExtendedControls.ButtonExt;
            Group g = (Group)config.Tag;

            ActionProgram p = null;
            string suggestedname = null;

            if (g.actionlist.SelectedIndex > 0)     // exclude NEW from checking for program
                p = actionfilelist.CurPrograms.Get(g.actionlist.Text);

            if ( p == null )        // if no program, create a new suggested name and clear any action data
            {
                suggestedname = g.evlist.Text;
                int n = 2;
                while (actionfilelist.CurPrograms.GetActionProgramList().Contains(suggestedname))
                {
                    suggestedname = g.evlist.Text + "_" + n.ToString();
                    n++;
                }

                g.actiondata = null;
            }

            ActionProgramForm apf = new ActionProgramForm();

            // we init with a variable list based on the field names of the group (normally the event field names got by SetFieldNames
            // pass in the program if found, and its action data.
            apf.Init("Action program", theme, g.varnames, actionfilelist.CurName , p, g.actiondata , actionfilelist.CurPrograms.GetActionProgramList(), suggestedname);

            DialogResult res = apf.ShowDialog();

            if (res == DialogResult.OK)
            {
                g.actiondata = apf.GetActionData();

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

            FixUpGroups();       // run  this, it sorts out the group names
        }

        #endregion

        #region Action List programs

        void RefreshActionDropDowns()
        {
            if (IsActionsActive)
            {
                comboBoxCustomEditProg.Items.Clear();
                comboBoxCustomEditProg.Items.AddRange(actionfilelist.CurPrograms.GetActionProgramList());
                comboBoxCustomEditProg.Items.Add("New");

                comboBoxCustomProgSet.Items.Clear();
                comboBoxCustomProgSet.Items.AddRange(actionfilelist.GetList);
                comboBoxCustomProgSet.Items.Add("New");
                comboBoxCustomProgSet.Enabled = false;
                comboBoxCustomProgSet.SelectedItem = actionfilelist.CurName;
                comboBoxCustomProgSet.Enabled = true;

                checkBoxCustomSetEnabled.Checked = actionfilelist.CurEnabled;
            }
        }

        private void comboBoxCustomProgSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCustomProgSet.Enabled)
            {
                string newname = comboBoxCustomProgSet.Text;
                bool newfile = newname.Equals("New");

                if (newfile || !newname.Equals(actionfilelist.CurName))
                {
                    if ( CheckAndAsk() )            // save current, and its not asked for a retry.. we have now saved this page
                    {
                        if (newfile)
                        {
                            newname = EDDiscovery.Actions.Action.PromptSingleLine.ShowDialog(this, "Enter name of new action file", "", "Action File");

                            if (newname != null)
                            {
                                newname = Tools.SafeFileString(newname);

                                if (actionfilelist.GetList.Contains(newname))
                                {
                                    MessageBox.Show(this, "Set already exists");
                                    return;
                                }
                                else
                                    actionfilelist.CreateSet(newname);
                            }
                        }

                        if (newname != null && actionfilelist.SelectCurrent(newname))
                        {
                            foreach (Group g in groups)
                            {
                                panelVScroll.Controls.Remove(g.panel);
                                g.panel.Controls.Clear();
                            }

                            groups = new List<Group>();

                            LoadConditions(actionfilelist.CurConditions);
                        }
                    }
                }

                RefreshActionDropDowns();
            }

        }

        private void comboBoxCustomEditProg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCustomEditProg.Enabled)
            {
                string progname = comboBoxCustomEditProg.Text;
                ActionProgramForm apf = new ActionProgramForm();

                ActionProgram p = null;

                if ( !progname.Equals("New"))
                     p = actionfilelist.CurPrograms.Get(comboBoxCustomEditProg.Text);

                List<string> vars = new List<string>();
                if (additionalfieldnames != null)
                    vars.AddRange(additionalfieldnames);
                if (userglobalvariables != null)
                    vars.AddRange(userglobalvariables.Keys.ToList());

                apf.Init("Action program", theme, vars, actionfilelist.CurName, p, null, actionfilelist.CurPrograms.GetActionProgramList(), "");

                DialogResult res = apf.ShowDialog();

                if (res == DialogResult.OK)
                {
                    ActionProgram np = apf.GetProgram();
                    actionfilelist.CurPrograms.AddOrChange(np);                // replaces or adds (if its a new name) same as rename
                }

                FixUpGroups();       // run  this, it sorts out the group names
            }
        }

        private void buttonExtGlobals_Click(object sender, EventArgs e)
        {
            ActionVariableForm avf = new ActionVariableForm();
            avf.Init("Global User variables to pass to program on run", theme, userglobalvariables);

            if (avf.ShowDialog(this) == DialogResult.OK)
            {
                userglobalvariables = avf.result;
            }
        }

        #endregion

        #region Condition

        void CreateCondition( Group g , string initialfname = null , string initialcond = null, string initialvalue = null )
        {
            ExtendedControls.ComboBoxCustom fname = new ExtendedControls.ComboBoxCustom();
            fname.Size = new Size(140, 24);
            fname.DropDownHeight = 400;
            fname.Name = "Field";
            if (g.varnames != null)
                fname.Items.AddRange(g.varnames);
            fname.Items.Add("User Defined");

            if (initialfname != null)
            {
                if (fname.Items.IndexOf(initialfname) < 0)
                    fname.Items.Add(initialfname);

                fname.SelectedItem = initialfname;
            }

            fname.SelectedIndexChanged += Fname_SelectedIndexChanged;

            g.panel.Controls.Add(fname);                                                // 1st control

            ExtendedControls.ComboBoxCustom cond = new ExtendedControls.ComboBoxCustom();
            cond.Items.AddRange(ConditionLists.MatchNames);
            cond.SelectedIndex = 0;
            cond.Size = new Size(130, 24);
            cond.DropDownHeight = 400;
            cond.Tag = g;

            if (initialcond != null)
                cond.Text = Tools.SplitCapsWord(initialcond);

            cond.SelectedIndexChanged += Cond_SelectedIndexChanged; // and turn on handler

            g.panel.Controls.Add(cond);         // must be next

            ExtendedControls.TextBoxBorder value = new ExtendedControls.TextBoxBorder();
            value.Size = new Size(190, 24);

            if (initialvalue != null)
                value.Text = initialvalue;

            g.panel.Controls.Add(value);         // must be next

            ExtendedControls.ButtonExt del = new ExtendedControls.ButtonExt();
            del.Size = new Size(24, 24);
            del.Text = "X";
            del.Click += ConditionDelClick;
            del.Tag = g;
            g.panel.Controls.Add(del);

            ExtendedControls.ButtonExt more = new ExtendedControls.ButtonExt();
            more.Size = new Size(24, 24);
            more.Text = "+";
            more.Click += NewConditionClick;
            more.Tag = g;
            g.panel.Controls.Add(more);

            theme.ApplyToControls(g.panel, SystemFonts.DefaultFont);
            RepositionGroupInternals(g);
            FixUpGroups();
        }

        private void Fname_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExtendedControls.ComboBoxCustom fname = sender as ExtendedControls.ComboBoxCustom;

            if (fname.Enabled && fname.Text.Equals("User Defined"))
            {
                EDDiscovery2.TextBoxEntry frm = new TextBoxEntry();
                frm.Text = "Enter new field";
                if ( frm.ShowDialog() == DialogResult.OK && frm.Value.Length > 0 )
                {
                    fname.Enabled = false;
                    fname.Items.Add(frm.Value);
                    fname.SelectedItem = frm.Value;
                    fname.Enabled = true;
                }
            }
        }

        private void Cond_SelectedIndexChanged(object sender, EventArgs e)          // on condition changing, see if value is needed 
        {
            ExtendedControls.ComboBoxCustom cond = sender as ExtendedControls.ComboBoxCustom;
            Group g = cond.Tag as Group;
            RepositionGroupInternals(g);
        }

        private void NewConditionClick(object sender, EventArgs e)
        {
            ExtendedControls.ButtonExt b = sender as ExtendedControls.ButtonExt;
            Group g = (Group)b.Tag;
            CreateCondition(g);
        }

        private void ConditionDelClick(object sender, EventArgs e)
        {
            ExtendedControls.ButtonExt b = sender as ExtendedControls.ButtonExt;
            Group g = (Group)b.Tag;

            int i = g.panel.Controls.IndexOf(b);

            Control c1 = g.panel.Controls[i - 3];
            Control c2 = g.panel.Controls[i - 2];
            Control c3 = g.panel.Controls[i - 1];
            Control c4 = g.panel.Controls[i + 0];
            Control c5 = g.panel.Controls[i + 1];
            g.panel.Controls.Remove(c1);
            g.panel.Controls.Remove(c2);
            g.panel.Controls.Remove(c3);
            g.panel.Controls.Remove(c4);
            g.panel.Controls.Remove(c5);

            int numcond = RepositionGroupInternals(g);

            if (numcond == 0)
            {
                panelVScroll.Controls.Remove(g.panel);
                g.panel.Controls.Clear();
                groups.Remove(g);
            }

            FixUpGroups();
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

        #region Positioning

        int RepositionGroupInternals(Group g)
        {
            int vnextcond = panelmargin;
            int numcond = 0;
            Control lastadd = null;

            g.innercond.Visible = false;

            for (int i = 0; i < g.panel.Controls.Count; i++)        // position, enable controls
            {
                if (g.panel.Controls[i].Name.Equals("Field"))           // FIELD starts FIELD | Condition | Value | Delete | More
                {
                    g.panel.Controls[i].Location = new Point(condxoffset, vnextcond);
                    g.panel.Controls[i + 1].Location = new Point(g.panel.Controls[i].Location.X + g.panel.Controls[i].Width + 8, vnextcond);
                    g.panel.Controls[i + 2].Location = new Point(g.panel.Controls[i + 1].Location.X + g.panel.Controls[i + 1].Width + 8, vnextcond + 4);
                    g.panel.Controls[i + 3].Location = new Point(g.panel.Controls[i + 2].Location.X + g.panel.Controls[i + 2].Width + 8, vnextcond);
                    g.panel.Controls[i + 4].Location = new Point(g.panel.Controls[i + 3].Location.X + g.panel.Controls[i + 3].Width + 8, vnextcond);
                    g.panel.Controls[i + 4].Visible = true;

                    ExtendedControls.ComboBoxCustom cond = g.panel.Controls[i + 1] as ExtendedControls.ComboBoxCustom;

                    g.panel.Controls[i].Enabled = !ConditionLists.IsNullOperation(cond.Text);
                    if (!g.panel.Controls[i].Enabled)
                    {
                        ExtendedControls.ComboBoxCustom fname = g.panel.Controls[i] as ExtendedControls.ComboBoxCustom;
                        fname.SelectedIndex = -1;
                    }

                    g.panel.Controls[i+2].Enabled = !ConditionLists.IsNullOperation(cond.Text) && ! ConditionLists.IsUnaryOperation(cond.Text);
                    if (!g.panel.Controls[i+2].Enabled)
                        g.panel.Controls[i+2].Text = "";

                    if (lastadd != null)
                    {
                        lastadd.Visible = false;

                        if (numcond == 1)
                        {
                            g.innercond.Location = lastadd.Location;
                            g.innercond.Visible = true;
                        }
                    }

                    lastadd = g.panel.Controls[i + 4];

                    numcond++;
                    vnextcond += conditionhoff;
                }
            }

            if (g.outercond.Enabled)
            {
                g.outercond.Location = new Point(panelmargin, vnextcond);
                g.outerlabel.Location = new Point(g.outercond.Location.X + g.outercond.Width + 4, g.outercond.Location.Y + 3);
                vnextcond += conditionhoff;
            }

            int minh = panelmargin + conditionhoff;
            g.panel.Size = new Size(g.panel.Width, Math.Max(vnextcond, minh));

            return numcond;
        }

        void FixUpGroups()      // fixes and positions groups.
        {
            for (int i = 0; i < groups.Count; i++)
            {
                bool showouter = false;                     // for all groups, see if another group below it has the same event selected as ours

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

                if (groups[i].outercond.Enabled != showouter)
                {
                    groups[i].outercond.Enabled = groups[i].outercond.Visible = groups[i].outerlabel.Visible = showouter;       // and enabled/disable the outer condition switch
                    RepositionGroupInternals(groups[i]);
                }
            }

            int y = panelmargin;
            bool showup = false;

            foreach (Group g in groups)
            {
                g.upbutton.Visible = showup;
                showup = true;
                g.panel.Location = new Point(panelmargin, y);
                y += g.panel.Height + 6;

                if (IsActionsActive)     // rework the action list in case something is changed
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
                    g.actionconfig.Enabled = g.actionlist.SelectedIndex != 0;
                }
            }

            RefreshActionDropDowns();

            buttonMore.Location = new Point(panelmargin, y);

            Rectangle screenRectangle = RectangleToScreen(this.ClientRectangle);
            int titleHeight = screenRectangle.Top - this.Top;

            y += buttonMore.Height + titleHeight + ((panelTop.Enabled) ? (panelTop.Height + statusStripCustom.Height) : 8) + 16 + panelOK.Height;

            this.MinimumSize = new Size(panelwidth+vscrollmargin*2+panelVScroll.ScrollBarWidth + 8, y );
            this.MaximumSize = new Size(Screen.FromControl(this).WorkingArea.Width, Screen.FromControl(this).WorkingArea.Height);
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
                string actiondata = (IsActionsActive && g.actiondata != null) ? g.actiondata : "Default"; // any associated data from the program
                string evt = (eventlist != null) ? g.evlist.Text : "Default";

                //System.Diagnostics.Debug.WriteLine("Event {0} inner {1} outer {2} action {3} data '{4}'", evt, innerc, outerc, actionname, actiondata );

                ConditionLists.Condition fe = new ConditionLists.Condition();

                if (IsActionsActive && actionname.Equals("New"))        // actions, but not selected one..
                {
                    errorlist += "Event " + evt + " does not have an action program defined" + Environment.NewLine;
                }
                else if (evt.Length > 0)        // must have name
                {
                    if (fe.Create(evt, actionname, actiondata, innerc, outerc)) // create must work
                    {
                        bool ok = true;

                        for (int i = 0; i < g.panel.Controls.Count && ok; i++)
                        {
                            Control c = g.panel.Controls[i];
                            if (c.Name == "Field")
                            {
                                string fieldn = c.Text;
                                string condn = g.panel.Controls[i + 1].Text;
                                string valuen = g.panel.Controls[i + 2].Text;

                                //System.Diagnostics.Debug.WriteLine("  {0} {1} {2}", fieldn, condn, valuen);

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
                                    errorlist += "Ignored empty filter in " + fe.eventname + Environment.NewLine;
                            }
                        }

                        if (ok)
                        {
                            if (fe.fields != null)
                                result.Add(fe);
                            else
                                errorlist += "No valid filters found in group '" + fe.eventname + "'" + Environment.NewLine;
                        }
                    }
                    else
                        errorlist += "Cannot create " + evt + " not a normal error please report" + Environment.NewLine;
                }
                else
                    errorlist += "Ignored group with empty name" + Environment.NewLine;
            }

            return errorlist;
        }

        private bool CheckAndAsk()
        {
            string res = Check();

            if (res.Length > 0)
            {
                string acceptstr = "Click Retry to correct errors, Abort to cancel, Ignore to accept valid entries";

                DialogResult dr = MessageBox.Show("Filters produced the following warnings and errors" + Environment.NewLine + Environment.NewLine + res + Environment.NewLine + acceptstr,
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

        public const int WM_MOVE = 3;
        public const int WM_SIZE = 5;
        public const int WM_MOUSEMOVE = 0x200;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int WM_NCLBUTTONUP = 0xA2;
        public const int WM_NCMOUSEMOVE = 0xA0;
        public const int HT_CLIENT = 0x1;
        public const int HT_CAPTION = 0x2;
        public const int HT_LEFT = 0xA;
        public const int HT_RIGHT = 0xB;
        public const int HT_BOTTOM = 0xF;
        public const int HT_BOTTOMRIGHT = 0x11;
        public const int WM_NCL_RESIZE = 0x112;
        public const int HT_RESIZE = 61448;
        public const int WM_NCHITTEST = 0x84;

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
            if (m.Msg == WM_LBUTTONDOWN && (int)m.WParam == 1 && !windowsborder)
            {
                int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                int y = unchecked((short)((uint)m.LParam >> 16));
                _window_dragMousePos = new Point(x, y);
                _window_dragWindowPos = this.Location;
                _window_dragging = true;
                m.Result = IntPtr.Zero;
                this.Capture = true;
            }
            else if (m.Msg == WM_MOUSEMOVE && (int)m.WParam == 1 && _window_dragging)
            {
                int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                int y = unchecked((short)((uint)m.LParam >> 16));
                Point delta = new Point(x - _window_dragMousePos.X, y - _window_dragMousePos.Y);
                _window_dragWindowPos = new Point(_window_dragWindowPos.X + delta.X, _window_dragWindowPos.Y + delta.Y);
                this.Location = _window_dragWindowPos;
                this.Update();
                m.Result = IntPtr.Zero;
            }
            else if (m.Msg == WM_LBUTTONUP)
            {
                _window_dragging = false;
                _window_dragMousePos = Point.Empty;
                _window_dragWindowPos = Point.Empty;
                m.Result = IntPtr.Zero;
                this.Capture = false;
            }
            // Windows honours NCHITTEST; Mono does not
            else if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m);

                if ((int)m.Result == HT_CLIENT)
                {
                    int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                    int y = unchecked((short)((uint)m.LParam >> 16));
                    Point p = PointToClient(new Point(x, y));

                    if (p.X > this.ClientSize.Width - statusStripCustom.Height && p.Y > this.ClientSize.Height - statusStripCustom.Height)
                    {
                        m.Result = (IntPtr)HT_BOTTOMRIGHT;
                    }
                    else if (p.Y > this.ClientSize.Height - statusStripCustom.Height)
                    {
                        m.Result = (IntPtr)HT_BOTTOM;
                    }
                    else if (p.X > this.ClientSize.Width - 5)       // 5 is generous.. really only a few pixels gets thru before the subwindows grabs them
                    {
                        m.Result = (IntPtr)HT_RIGHT;
                    }
                    else if (p.X < 5)
                    {
                        m.Result = (IntPtr)HT_LEFT;
                    }
                    else if (!windowsborder)
                    {
                        m.Result = (IntPtr)HT_CAPTION;
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
            SendMessage(WM_NCLBUTTONDOWN,(System.IntPtr)HT_CAPTION, (System.IntPtr)0);
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

    }
}
