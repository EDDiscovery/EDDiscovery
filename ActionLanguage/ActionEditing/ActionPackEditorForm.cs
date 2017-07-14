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
    public partial class ActionPackEditorForm : Form
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
            public UserControlAB usercontrol;
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

        public void Init(string t, ActionCoreController cp, string appfolder, ActionFile file, List<Tuple<string, string>> ev)
        {
            actioncorecontroller = cp;
            applicationfolder = appfolder;
            actionfile = file;
            events = ev;

            grouptypenames = (from e in events select e.Item2).ToList().Distinct().ToList();
            groupeventlist = new Dictionary<string, List<string>>();
            foreach (string s in grouptypenames)
                groupeventlist.Add(s, (from e in events where e.Item2 == s select e.Item1).ToList());

            bool winborder = BaseUtils.ThemeAbleFormsInstance.Instance.ApplyToForm(this, SystemFonts.DefaultFont);
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
                BaseUtils.ThemeAbleFormsInstance.Instance.ApplyToControls(g.panel, SystemFonts.DefaultFont);
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

            g.usercontrol = UserControlAB.Create(g.grouptype.Text);
            g.usercontrol.Init(c, groupeventlist[g.grouptype.Text], actioncorecontroller, applicationfolder, actionfile, onAdditionalNames);
            BaseUtils.ThemeAbleFormsInstance.Instance.ApplyToControls(g.usercontrol, SystemFonts.DefaultFont);
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

        private void Usercontrol_RemoveEvent(UserControlAB item)
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
            BaseUtils.ThemeAbleFormsInstance.Instance.ApplyToControls(g.panel, SystemFonts.DefaultFont);
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

                    apf.Init("Action program " , actioncorecontroller, applicationfolder,
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
            avf.Init("Configuration items for installation - specialist use",  actionfile.installationvariables, showone: false);

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

        private void panelVScroll_Resize(object sender, EventArgs e)
        {
            PositionGroups(false);
        }

    }

    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////////////
    /// </summary>

    public class UserControlAB : UserControl
    {
        public Condition cd;
        
        public ActionFile actionfile;

        public ActionCoreController actioncorecontroller;
        public string applicationfolder;
        
        public System.Action<UserControlAB> RemoveItem;
        public System.Action RefreshEvent;

        public const int panelxmargin = 3;
        public const int panelymargin = 1;

        static public UserControlAB Create(string groupname)
        {
            UserControlAB ucab;
            ucab = new UserControlProgram();
            return ucab;
        }

        protected ActionPackEditorForm.AdditionalNames onAdditionalNames;

        public void Init(Condition cond, List<string> events, ActionCoreController cp, string appfolder, ActionFile file , ActionPackEditorForm.AdditionalNames func)
        {
            onAdditionalNames = func;
            cd = new Condition(cond);        // full clone, we can now modify it.
            actionfile = file;
            actioncorecontroller = cp;
            applicationfolder = appfolder;
            InitSub(events);
        }

        public virtual string ID() { return ""; }
        public virtual void InitSub(List<string> events) { }
        public virtual void EventChanged() { }
        public virtual void UpdateProgramList(string[] proglist) { }

        public virtual new void Dispose()
        {
            base.Dispose();
        }

        public void RefreshIt()
        {
            RefreshEvent?.Invoke();
        }

        public void RemoveIt()
        {
            RemoveItem?.Invoke(this);
        }
    }

    public class UserControlAction : UserControlAB
    {
        public ExtendedControls.ComboBoxCustom eventtype;
        public ExtendedControls.ComboBoxCustom proglist;
        public ExtendedControls.ButtonExt progedit;
        public ExtendedControls.TextBoxBorder paras;

        public override void InitSub(List<string> events)
        {
            eventtype = new ExtendedControls.ComboBoxCustom();
            eventtype.Items.AddRange(events);
            eventtype.Location = new Point(panelxmargin, panelymargin);
            eventtype.Size = new Size(140, 24);
            eventtype.DropDownHeight = 400;
            eventtype.DropDownWidth = eventtype.Width * 3 / 2;
            if ( cd.eventname !=null )
                eventtype.SelectedItem = cd.eventname;
            eventtype.SelectedIndexChanged += Actiontype_SelectedIndexChanged;

            proglist = new ExtendedControls.ComboBoxCustom();
            proglist.Items.Add("New");
            proglist.Items.AddRange(actionfile.actionprogramlist.GetActionProgramList());
            proglist.Location = new Point(eventtype.Right + 16, panelymargin);
            proglist.Size = new Size(140, 24);
            proglist.DropDownHeight = 400;
            proglist.DropDownWidth = proglist.Width * 3 / 2;
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

            SuspendLayout();
            Controls.Add(proglist);
            Controls.Add(eventtype);
            Controls.Add(progedit);
            Controls.Add(paras);
            ResumeLayout();
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

        private void Paras_Click(object sender, EventArgs e)
        {
            ConditionVariables cond = new ConditionVariables();
            string flag = "";

            if (cd.actiondata != null)
            {
                cond.FromActionDataString(cd.actiondata, out flag);
            }

            ConditionVariablesForm avf = new ConditionVariablesForm();
            avf.Init("Input parameters and flags to pass to program on run",cond, showone: true, showrefresh: true, showrefreshstate: flag.Equals(ConditionVariables.flagRunAtRefresh));

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
                if (ExtendedControls.MessageBoxTheme.Show(this, "Do you want to bring the file back into the main file", "WARNING", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    p.CancelSubFileStorage();
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
                    suggestedname = eventtype.Text;
                    int n = 2;
                    while (actionfile.actionprogramlist.GetActionProgramList().Contains(suggestedname))
                    {
                        suggestedname = eventtype.Text + "_" + n.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        n++;
                    }

                    paras.Text = "";
                }

                ActionProgramEditForm apf = new ActionProgramEditForm();
                apf.EditProgram += EditProgram;

                // we init with a variable list based on the field names of the group (normally the event field names got by SetFieldNames)
                // pass in the program if found, and its action data.

                apf.Init("Action program " , actioncorecontroller, applicationfolder, onAdditionalNames(eventtype.Text), actionfile.name , p, actionfile.actionprogramlist.GetActionProgramList(), suggestedname, ModifierKeys.HasFlag(Keys.Shift));

                DialogResult res = apf.ShowDialog();

                if (res == DialogResult.OK)
                {
                    ActionProgram np = apf.GetProgram();
                    actionfile.actionprogramlist.AddOrChange(np);                // replaces or adds (if its a new name) same as rename
                    cd.action = np.Name;
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
            if (!actionfile.actionprogramlist.EditProgram(s, actionfile.name, actioncorecontroller, applicationfolder))
                ExtendedControls.MessageBoxTheme.Show(this, "Unknown program or not in this file " + s);
        }

        private void Actiontype_SelectedIndexChanged(object sender, EventArgs e)
        {
            cd.eventname = eventtype.Text;
            EventChanged();
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
            eventtype.Dispose();
            progedit.Dispose();
            paras.Dispose();
            base.Dispose();
        }
    }


    public class UserControlProgram : UserControlAction
    {
        public ExtendedControls.TextBoxBorder condition;
        public ExtendedControls.ButtonExt delete;

        public override string ID() { return eventtype.Text.Length > 0 ? eventtype.Text : "Action not set"; }

        public override void InitSub(List<string> events)
        {
            base.InitSub(events);

            condition = new ExtendedControls.TextBoxBorder();
            condition.Text = cd.ToString();
            condition.Location = new Point(paras.Right + 16, panelymargin + 2);
            condition.Size = new Size(200, 24);
            condition.ReadOnly = true;
            condition.Click += Condition_Click;
            condition.Tag = this;

            delete = new ExtendedControls.ButtonExt();
            delete.Text = "X";
            delete.Location = new Point(condition.Right + 8, panelymargin);
            delete.Size = new Size(24, 24);
            delete.Click += Delete_Click;

            SuspendLayout();
            Controls.Add(condition);
            Controls.Add(delete);
            ResumeLayout();
        }

        private void Condition_Click(object sender, EventArgs e)
        {
            ConditionFilterForm frm = new ConditionFilterForm();

            frm.InitCondition("Action condition", onAdditionalNames(eventtype.Text), cd);
            frm.TopMost = this.FindForm().TopMost;
            if (frm.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                Condition res = frm.result.Get(0);
                if (res != null)
                {
                    cd.fields = res.fields;
                    cd.innercondition = res.innercondition;
                }
                else
                    cd.fields = null;

                condition.Text = cd.ToString();
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            RemoveIt();
        }

        public new void Dispose()
        {
            condition.Dispose();
            delete.Dispose();
            base.Dispose();
        }

        public override void EventChanged()     // helpers to set initial conditions
        {
            if (cd.AlwaysTrue())
            {
                if (cd.eventname == "onKeyPress")
                    cd.Set(new ConditionEntry("KeyPress", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.eventname == "onTimer")
                    cd.Set(new ConditionEntry("TimerName", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.eventname == "onPopUp" || cd.eventname == "onPopDown")
                    cd.Set(new ConditionEntry("PopOutName", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.eventname == "onTabChange")
                    cd.Set(new ConditionEntry("TabName", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.eventname == "onPanelChange")
                    cd.Set(new ConditionEntry("PopOutName", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.eventname == "onEliteInput" || cd.eventname == "onEliteInputOff" )
                    cd.Set(new ConditionEntry("Binding", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.eventname == "onMenuItem")
                    cd.Set(new ConditionEntry("MenuName", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.eventname == "onSayStarted" || cd.eventname == "onSayFinished" || cd.eventname == "onPlayStarted" || cd.eventname == "onPlayFinished" )
                    cd.Set(new ConditionEntry("EventName", ConditionEntry.MatchType.Equals, "?"));

                condition.Text = cd.ToString();
            }
        }
    }
}
