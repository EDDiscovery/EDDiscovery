using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    public partial class ActionEditorForm : Form
    {
        ActionFile actionfile;
        List<Tuple<string, string>> events;
        List<string> grouptypenames;
        Dictionary<string, List<string>> groupeventlist;
        EDDiscoveryForm discoveryform;
        string initialtitle;

        const int panelxmargin = 3;
        const int panelymargin = 1;
        const int conditionhoff = 26;

        class Group
        {
            public Panel panel;
            public ExtendedControls.ComboBoxCustom grouptype;
            public UserControlAB usercontrol;
        }

        List<Group> groups;

        public ActionEditorForm()
        {
            InitializeComponent();
        }

        public void Init(string t, ActionFile file, List<Tuple<string, string>> ev, EDDiscoveryForm form)
        {
            actionfile = file;
            events = ev;
            discoveryform = form;

            grouptypenames = (from e in events select e.Item2).ToList().Distinct().ToList();
            groupeventlist = new Dictionary<string, List<string>>();
            foreach (string s in grouptypenames)
                groupeventlist.Add(s, (from e in events where e.Item2 == s select e.Item1).ToList());

            bool winborder = discoveryform.theme.ApplyToForm(this, SystemFonts.DefaultFont);
            statusStripCustom.Visible = panelTop.Visible = panelTop.Enabled = !winborder;
            initialtitle = this.Text = label_index.Text = t;

            LoadConditions();
        }

        private void LoadConditions()
        {
            ConditionLists c = actionfile.actionfieldfilter;
            groups = new List<Group>();

            for (int i = 0; i < c.Count; i++)
            {
                Condition cd = c.Get(i);
                Group g = CreateGroup(cd);
                groups.Add(g);
                discoveryform.theme.ApplyToControls(g.panel, SystemFonts.DefaultFont);
            }

            foreach (Group g in groups)
                panelVScroll.Controls.Add(g.panel);

            PositionGroups();
            this.Text = label_index.Text = initialtitle + " (" + groups.Count.ToString() + " conditions)";

            Usercontrol_RefreshEvent();
        }

        private Group CreateGroup(Condition cd)
        {
            Group g = new Group();

            g.panel = new Panel();
            g.panel.BackColor = Color.Red;
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
                g.usercontrol = UserControlAB.Create(g.grouptype.Text);
                g.usercontrol.Init(cd, groupeventlist[g.grouptype.Text], actionfile, discoveryform);
                g.usercontrol.Location = new Point(panelxmargin + 108, panelymargin);
                g.usercontrol.Size = new Size(500, 24);
                g.usercontrol.RefreshEvent += Usercontrol_RefreshEvent;
                g.panel.Controls.Add(g.usercontrol);
            }

            g.grouptype.SelectedIndexChanged += Grouptype_SelectedIndexChanged;
            g.grouptype.Tag = g;
            g.panel.Controls.Add(g.grouptype);

            return g;
        }

        private void PositionGroups()
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
                    g.usercontrol.Size = new Size(g.panel.Size.Width - 4 - g.usercontrol.Left, 24);

                y += g.panel.Height + 2;
            }

            buttonMore.Location = new Point(panelxmargin, y + panelVScroll.ScrollOffset);
            buttonSort.Location = new Point(buttonMore.Right + 8, y + panelVScroll.ScrollOffset);
            buttonSort2.Location = new Point(buttonSort.Right + 8, y + panelVScroll.ScrollOffset);
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
            comboBoxCustomEditProg.Items.AddRange(actionfile.actionprogramlist.GetActionProgramList());
            comboBoxCustomEditProg.Enabled = true;

            foreach (Group g in groups)
            {
                if (g.usercontrol != null)
                    g.usercontrol.UpdateProgramList(actionfile.actionprogramlist.GetActionProgramList());
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
            Group g = CreateGroup(null);        // usercontrol is null
            discoveryform.theme.ApplyToControls(g.panel, SystemFonts.DefaultFont);
            groups.Add(g);
            PositionGroups();
            panelVScroll.Controls.Add(g.panel);
            this.Text = label_index.Text = initialtitle + " (" + groups.Count.ToString() + " conditions)";
            panelVScroll.ToEnd();       // tell it to scroll to end
        }

        private void Grouptype_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExtendedControls.ComboBoxCustom b = sender as ExtendedControls.ComboBoxCustom;
            Group g = (Group)b.Tag;

            if (g.usercontrol != null)
            {
                Controls.Remove(g.usercontrol);
                g.usercontrol.Dispose();
            }

            g.usercontrol = UserControlAB.Create(g.grouptype.Text);
            g.usercontrol.Init(new Condition(), groupeventlist[g.grouptype.Text], actionfile, discoveryform);
            discoveryform.theme.ApplyToControls(g.usercontrol, SystemFonts.DefaultFont);
            g.usercontrol.Location = new Point(panelxmargin + 108, panelymargin);
            g.usercontrol.Size = new Size(500, 24);
            g.usercontrol.RefreshEvent += Usercontrol_RefreshEvent;
            g.panel.Controls.Add(g.usercontrol);
        }


        private void buttonSort_Click(object sender, EventArgs e)
        {

        }

        private void buttonSort2_Click(object sender, EventArgs e)
        {

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

                DialogResult dr = EDDiscovery.Forms.MessageBoxTheme.Show(this, "Filters produced the following warnings and errors" + Environment.NewLine + Environment.NewLine + res + Environment.NewLine + acceptstr,
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

            actionfile.actionfieldfilter = result;
            actionfile.enabled = checkBoxCustomSetEnabled.Checked;

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
                    ActionProgramForm apf = new ActionProgramForm();
                    // TBD apf.EditProgram += EditProgram;

                    List<string> additionalfieldnames = new List<string>(); // TBD
                    apf.Init("Action program", discoveryform, additionalfieldnames, actionfile.name, p, actionfile.actionprogramlist.GetActionProgramList(), "", ModifierKeys.HasFlag(Keys.Shift));

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

        private void checkBoxCustomSetEnabled_CheckedChanged(object sender, EventArgs e)
        {

        }

        #endregion

        #region Sort

        #endregion
    }


    public class UserControlAB : UserControl
    {
        public Condition cd;
        public EDDiscoveryForm discoveryform;
        public ActionFile actionfile;

        public const int panelxmargin = 3;
        public const int panelymargin = 1;

        static public UserControlAB Create(string groupname)
        {
            UserControlAB ucab;
//            if (groupname == "Program" || groupname == "Misc" || groupname == "UI" || groupname == "EliteUI" )
            {
                ucab = new UserControlProgram();
            }
  //          else
    //            return null;

            return ucab;
        }

        public void Init(Condition cond, List<string> events, ActionFile file, EDDiscoveryForm form)
        {
            cd = new Condition(cond);        // full clone, we can now modify it.
            actionfile = file;
            discoveryform = form;
            Init(events);
        }

        public virtual void Init(List<string> events) { }

        public virtual new void Dispose()
        {
            base.Dispose();
        }

        public virtual string ID() { return ""; }

        public virtual void UpdateProgramList(string[] proglist)
        { }

        public delegate void Refresh();
        public event Refresh RefreshEvent;
        public void RefreshIt()
        {
            RefreshEvent();
        }
    }

    public class UserControlProgram : UserControlAB
    {
        public ExtendedControls.ComboBoxCustom actiontype;
        public ExtendedControls.ComboBoxCustom proglist;
        public ExtendedControls.ButtonExt progedit;
        public ExtendedControls.TextBoxBorder paras;
        public ExtendedControls.TextBoxBorder condition;

        public override string ID() { return actiontype.Text.Length>0 ? actiontype.Text : "Action not set"; }

        public override void Init(List<string> events)
        {
            actiontype = new ExtendedControls.ComboBoxCustom();
            actiontype.Items.AddRange(events);
            actiontype.Location = new Point(panelxmargin, panelymargin);
            actiontype.Size = new Size(100, 24);
            actiontype.DropDownHeight = 400;
            if ( cd.eventname !=null )
                actiontype.SelectedItem = cd.eventname;
            actiontype.SelectedIndexChanged += Actiontype_SelectedIndexChanged;

            proglist = new ExtendedControls.ComboBoxCustom();
            proglist.DropDownHeight = 400;
            proglist.Size = new Size(140, 24);
            proglist.Location = new Point(actiontype.Right + 8, panelymargin);
            proglist.Items.Add("New");
            proglist.Items.AddRange(actionfile.actionprogramlist.GetActionProgramList());
            if (cd.action != null)
                proglist.Text = cd.action;
            else
                proglist.SelectedIndex = 0;

            proglist.SelectedIndexChanged += Proglist_SelectedIndexChanged;

            progedit = new ExtendedControls.ButtonExt();
            progedit.Text = "P";
            progedit.Location = new Point(proglist.Right + 8, panelymargin);
            progedit.Size = new Size(24, 24);
            progedit.Click += Progedit_Click;

            paras = new ExtendedControls.TextBoxBorder();
            paras.Text = (cd.actiondata != null) ? cd.actiondata : "";
            paras.Location = new Point(progedit.Right + 8, panelymargin + 2);
            paras.Size = new Size(96, 24);
            paras.ReadOnly = true;
            paras.Click += Paras_Click;

            condition = new ExtendedControls.TextBoxBorder();
            condition.Text = cd.ToString();
            condition.Location = new Point(paras.Right + 8, panelymargin + 2);
            condition.Size = new Size(150, 24);
            condition.ReadOnly = true;
            condition.Click += Condition_Click;
            condition.Tag = this;

            Controls.Add(proglist);
            Controls.Add(actiontype);
            Controls.Add(progedit);
            Controls.Add(paras);
            Controls.Add(condition);
        }

        public override void UpdateProgramList(string[] progl)
        {
            proglist.Enabled = false;

            string text = proglist.Text;

            proglist.Items.Clear();
            proglist.Items.Add("New");
            proglist.Items.AddRange(progl);

            if ( proglist.Items.Contains(text))
                proglist.SelectedItem = text;

            proglist.Enabled = true;
        }

        private void Condition_Click(object sender, EventArgs e)
        {
            EDDiscovery.ConditionFilterForm frm = new EDDiscovery.ConditionFilterForm();

            frm.InitCondition("Action condition", new List<string>(), discoveryform, cd);
            frm.TopMost = this.FindForm().TopMost;
            if (frm.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                if (frm.result.Count > 0)
                {
                    string ev = cd.eventname;
                    string ac = cd.action;
                    string ad = cd.actiondata;

                    cd = frm.result.Get(0);
                    cd.eventname = ev;
                    cd.action = ac;
                    cd.actiondata = ad;
                    condition.Text = cd.ToString();
                }
            }
        }

        private void Paras_Click(object sender, EventArgs e)
        {
            ConditionVariables cond = new ConditionVariables();
            string flag = "";

            if (cd.actiondata != null)
            {
                cond.FromActionDataString(cd.actiondata, out flag);
            }

            ConditionVariablesForm avf = new ConditionVariablesForm();
            avf.Init("Input parameters and flags to pass to program on run", discoveryform.theme, cond, showone: true, showrefresh: true, showrefreshstate: flag.Equals(ConditionVariables.flagRunAtRefresh));

            if (avf.ShowDialog(this) == DialogResult.OK)
            {
                cd.actiondata = paras.Text = avf.result.ToActionDataString(avf.result_refresh ? ConditionVariables.flagRunAtRefresh : "");
            }
        }

        private void Progedit_Click(object sender, EventArgs e)
        {
            bool shift = ModifierKeys.HasFlag(Keys.Shift);

            ActionProgram p = null;

            if (proglist.SelectedIndex > 0)     // exclude NEW from checking for program
                p = actionfile.actionprogramlist.Get(proglist.Text);

            if (p != null && p.StoredInSubFile != null && shift)        // if we have a stored in sub file, but we shift hit, cancel it
            {
                if (Forms.MessageBoxTheme.Show(this, "Do you want to bring the file back into the main file", "WARNING", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    p.StoredInSubFile = null;
                    shift = false;
                }
                else
                    return; // cancel, abort.
            }

            if (p != null && p.StoredInSubFile != null)
            {
                p.EditInEditor(p.StoredInSubFile);         // Edit in the editor.. this also updated the program steps held internally
            }
            else
            {
                string suggestedname = null;

                if (p == null)        // if no program, create a new suggested name and clear any action data
                {
                    suggestedname = actiontype.Text;
                    int n = 2;
                    while (actionfile.actionprogramlist.GetActionProgramList().Contains(suggestedname))
                    {
                        suggestedname = actiontype.Text + "_" + n.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        n++;
                    }

                    paras.Text = "";
                }

                ActionProgramForm apf = new ActionProgramForm();
                apf.EditProgram += EditProgram;

                // we init with a variable list based on the field names of the group (normally the event field names got by SetFieldNames)
                // pass in the program if found, and its action data.

                List<string> fieldnames = new List<string>();
                 //TBD   fieldnames.AddRange(additionalfieldnames);     missing functionality here

                apf.Init("Action program", discoveryform, fieldnames, actionfile.name , p, actionfile.actionprogramlist.GetActionProgramList(), suggestedname, ModifierKeys.HasFlag(Keys.Shift));

                DialogResult res = apf.ShowDialog();

                if (res == DialogResult.OK)
                {
                    ActionProgram np = apf.GetProgram();
                    actionfile.actionprogramlist.AddOrChange(np);                // replaces or adds (if its a new name) same as rename
                    cd.action = apf.Name;
                    RefreshIt();
                    proglist.Enabled = false;
                    proglist.SelectedItem = np.Name;
                    proglist.Enabled = true;
                }
                else if (res == DialogResult.Abort)   // delete
                {
                    ActionProgram np2 = apf.GetProgram();
                    actionfile.actionprogramlist.Delete(np2.Name);
                    cd.action = "";
                    RefreshIt();
                }
            }
        }

        private void EditProgram(string s)  // Callback by APF to ask to edit another program..
        {
            int colon = s.IndexOf("::");

            ActionProgram p = actionfile.actionprogramlist.Get(s);

            if (p != null && colon == -1)   // only in same file
            {
                if (p.StoredInSubFile != null)
                {
                    p.EditInEditor(p.StoredInSubFile);         // Edit in the editor..
                }
                else
                {
                    ActionProgramForm apf = new ActionProgramForm();
                    apf.EditProgram += EditProgram;

                    List<string> additionalfieldnames = new List<string>(); // TBD

                    apf.Init("Action program", discoveryform, additionalfieldnames, actionfile.name, p, actionfile.actionprogramlist.GetActionProgramList(), "");

                    DialogResult res = apf.ShowDialog();

                    if (res == DialogResult.OK)
                    {
                        ActionProgram np = apf.GetProgram();
                        actionfile.actionprogramlist.AddOrChange(np);                // replaces or adds (if its a new name) same as rename
                    }
                }
            }
            else
                EDDiscovery.Forms.MessageBoxTheme.Show(this, "Unknown program or not in this file " + s);
        }

        private void Actiontype_SelectedIndexChanged(object sender, EventArgs e)
        {
            cd.eventname = actiontype.Text;
        }

        private void Proglist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (proglist.Enabled && proglist.SelectedIndex == 0)   // if selected NEW.
            {
                Progedit_Click(null, null);
            }
            else
                cd.action = proglist.Text;      // set program selected
        }

        public new void Dispose()
        {
            Controls.Clear();
            proglist.Dispose();
            actiontype.Dispose();
            progedit.Dispose();
            paras.Dispose();
            condition.Dispose();
            base.Dispose();
        }
    }
}
