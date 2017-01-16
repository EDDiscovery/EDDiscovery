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
    public partial class ActionProgramForm : Form
    {
        string initialprogname;
        string[] definedprograms;                                   // list of programs already defined, to detect rename over..

        ConditionVariables inputparas;                             // input parameters to configure for this program
        List<string> startvarlist;                                  // starting vars
        List<string> currentvarlist;                                // variables available to use.. combination of above

        class Group
        {
            public Panel panel;
            public Action programstep;
            public ExtendedControls.ComboBoxCustom stepname;
            public ExtendedControls.ButtonExt config;
            public ExtendedControls.ButtonExt up;
            public ExtendedControls.ButtonExt left;
            public ExtendedControls.ButtonExt right;
            public ExtendedControls.ButtonExt ins;
            public ExtendedControls.ButtonExt del;
            public ExtendedControls.TextBoxBorder value;
            public int levelup;
            public bool marked;

            public int indentcomputed;      // when displayed, what was the indent
        };

        List<Group> groups;
        int panelheightmargin = 1;
        int panelleftmargin = 3;
        int controlsize = 22;
        int panelheight = 24;

        public ActionProgramForm()
        {
            InitializeComponent();
            groups = new List<Group>();
            CancelButton = buttonCancel;
            AcceptButton = buttonOK;
        }

        EDDiscovery2.EDDTheme theme;
        const int vscrollmargin = 10;
        const int xpanelmargin = 3;

        public void Init(string t, EDDiscovery2.EDDTheme th,
                            List<string> vbs,              // list any variables you want in condition statements - passed to config menu, passed back up to condition, not null
                            string filesetname,             // file set name
                            ActionProgram prog = null,     // give the program to display
                            string progdata = null,         // give any associated program data
                            string[] defprogs = null,      // list any default program names
                            string suggestedname = null)   // give a suggested name, if prog is null
        {
            theme = th;

            startvarlist = vbs;
            currentvarlist = new List<string>(startvarlist);

            bool winborder = theme.ApplyToForm(this, SystemFonts.DefaultFont);
            statusStripCustom.Visible = panelTop.Visible = panelTop.Enabled = !winborder;
            this.Text = label_index.Text = t;

            labelSet.Text = filesetname + "::";
            textBoxBorderName.Location = new Point(labelSet.Location.X + labelSet.Width + 8, textBoxBorderName.Location.Y);

            if (progdata != null)
            {
                List<string> flaglist;
                ActionData.FromJSON(progdata, out flaglist, out inputparas);
                checkBoxCustomRefresh.Checked = flaglist.Contains(ActionProgram.flagRunAtRefresh);
                currentvarlist.AddRange(inputparas.KeyList);
            }
            else
                checkBoxCustomRefresh.Visible = buttonVars.Visible = false;

            if (defprogs != null)
                definedprograms = defprogs;

            if (suggestedname != null)
                textBoxBorderName.Text = suggestedname;

            if (prog != null)
                LoadProgram(prog);

            panelVScroll.ContextMenuStrip = contextMenuStrip1;
            panelVScroll.MouseDown += panelVScroll_MouseDown;
        }

        void DeleteAll()
        {
            foreach( Group g in groups )
            {
                g.panel.Controls.Clear();
                panelVScroll.Controls.Remove(g.panel);
            }

            groups.Clear();
            RepositionGroups();
        }

        void LoadProgram(ActionProgram prog)
        {
            initialprogname = textBoxBorderName.Text = prog.Name;

            Action ac;
            int step = 0;
            while ((ac = prog.GetStep(step++)) != null)
                CreateStep(ac);
        }

        private void ActionProgramForm_Resize(object sender, EventArgs e)
        {
            RepositionGroups();
        }

        #region Steps

        Group CreateStep(Action step = null, int insertpos = -1)
        {
            Group g = new Group();
            g.programstep = step;
            g.levelup = (step != null) ? step.LevelUp : 0;

            g.panel = new Panel();
            g.panel.MouseUp += panelVScroll_MouseUp;
            g.panel.MouseDown += panelVScroll_MouseDown;
            g.panel.MouseMove += panelVScroll_MouseMove;
            g.panel.ContextMenuStrip = contextMenuStrip1;

            g.left = new ExtendedControls.ButtonExt();
            g.left.Location = new Point(0, panelheightmargin);      // 8 spacing, allow 8*4 to indent
            g.left.Size = new Size(controlsize, controlsize);
            g.left.Text = "<";
            g.left.Click += Left_Clicked;
            g.panel.Controls.Add(g.left);

            g.right = new ExtendedControls.ButtonExt();
            g.right.Location = new Point(g.left.Right + 2, panelheightmargin);      // 8 spacing, allow 8*4 to indent
            g.right.Size = new Size(controlsize, controlsize);
            g.right.Text = ">";
            g.right.Click += Right_Clicked;
            g.panel.Controls.Add(g.right);

            g.stepname = new ExtendedControls.ComboBoxCustom();
            g.stepname.Items.AddRange(Action.GetActionNameList());
            g.stepname.DropDownHeight = 400;
            g.stepname.Name = "g.stepname";
            if (step != null)
                g.stepname.Text = step.Name;
            g.stepname.SelectedIndexChanged += Stepname_SelectedIndexChanged;
            g.panel.Controls.Add(g.stepname);

            g.value = new ExtendedControls.TextBoxBorder();
            SetValue(g.value, step);
            g.value.TextChanged += Value_TextChanged;
            g.value.Click += Value_Click;
            g.panel.Controls.Add(g.value);         // must be next

            g.config = new ExtendedControls.ButtonExt();
            g.config.Text = "C";
            g.config.Size = new Size(controlsize, controlsize);
            g.config.Click += ActionConfig_Clicked;
            g.panel.Controls.Add(g.config);         // must be next

            g.ins = new ExtendedControls.ButtonExt();
            g.ins.Size = new Size(controlsize, controlsize);
            g.ins.Text = "+";
            g.ins.Click += Ins_Clicked;
            g.panel.Controls.Add(g.ins);

            g.del = new ExtendedControls.ButtonExt();
            g.del.Size = new Size(controlsize, controlsize);
            g.del.Text = "X";
            g.del.Click += Del_Clicked;
            g.panel.Controls.Add(g.del);

            g.up = new ExtendedControls.ButtonExt();
            g.up.Size = new Size(controlsize, controlsize);
            g.up.Text = "^";
            g.up.Click += Up_Clicked;
            g.panel.Controls.Add(g.up);

            g.del.Tag = g.config.Tag = g.stepname.Tag = g.up.Tag = g.ins.Tag = g.value.Tag = g.left.Tag = g.right.Tag = g;

            theme.ApplyToControls(g.panel, SystemFonts.DefaultFont);

            panelVScroll.Controls.Add(g.panel);

            if (insertpos == -1)
                groups.Add(g);
            else
                groups.Insert(insertpos, g);

            RepositionGroups();

            return g;
        }

        void SetValue(ExtendedControls.TextBoxBorder value, Action step)
        {
            value.Enabled = false;
            value.Text = "";
            value.ReadOnly = true;

            if (step != null)
            {
                value.ReadOnly = !step.AllowDirectEditingOfUserData;
                value.Visible = step.DisplayedUserData != null;
                if (step.DisplayedUserData != null)
                    value.Text = step.DisplayedUserData;
            }

            value.Enabled = true;
        }

        string RepositionGroups()
        {
            string errlist = "";

            int voff = panelheightmargin;

            int structlevel = 0;
            int[] structcount = new int[50];
            Action.ActionType[] structtype = new Action.ActionType[50];

            bool first = true;

            SuspendLayout();

            int panelwidth = Math.Max(panelVScroll.Width - panelVScroll.ScrollBarWidth, 10);

            foreach (Group g in groups)
            {
                g.left.Enabled = g.right.Enabled = false;

                bool indo = structtype[structlevel] == Action.ActionType.Do;        // if in a DO..WHILE, and we are a WHILE, we don't indent.

                if (g.levelup > 0)
                {
                    if (g.levelup > structlevel)            // ensure its not too big.. this may happen due to copying
                        g.levelup = structlevel;

                    structlevel -= g.levelup;
                    g.right.Enabled = true;
                }

                structcount[structlevel]++;

                if (structlevel > 0 && structcount[structlevel] > 1)        // second further on can be moved back..
                    g.left.Enabled = true;

                int displaylevel = structlevel;        // displayed level..

                if (g.programstep != null)
                {
                    if (g.programstep.Type == Action.ActionType.ElseIf)
                    {
                        if (structtype[structlevel] == Action.ActionType.Else)
                            errlist += "Step " + (groups.IndexOf(g) + 1).ToString() + " ElseIf after Else found" + Environment.NewLine;
                        else if (structtype[structlevel] != Action.ActionType.If)
                            errlist += "Step " + (groups.IndexOf(g) + 1).ToString() + " ElseIf without IF found" + Environment.NewLine;
                    }
                    else if (g.programstep.Type == Action.ActionType.Else)
                    {
                        if (structtype[structlevel] == Action.ActionType.Else)
                            errlist += "Step " + (groups.IndexOf(g) + 1).ToString() + " Else after Else found" + Environment.NewLine;
                        else if (structtype[structlevel] != Action.ActionType.If && structtype[structlevel] != Action.ActionType.ElseIf)
                            errlist += "Step " + (groups.IndexOf(g) + 1).ToString() + " Else without IF found" + Environment.NewLine;

                    }

                    if (g.programstep.Type == Action.ActionType.ElseIf || g.programstep.Type == Action.ActionType.Else)
                    {
                        structtype[structlevel] = g.programstep.Type;

                        if (structlevel == 1)
                            g.left.Enabled = false;         // can't move an ELSE back to level 0

                        if (structlevel > 0)      // display else artifically indented.. display only
                            displaylevel--;

                        structcount[structlevel] = 0;   // restart count so we don't allow a left on next one..
                    }
                    else if (g.programstep.Type == Action.ActionType.If || (g.programstep.Type == Action.ActionType.While && !indo) ||
                                g.programstep.Type == Action.ActionType.Do || g.programstep.Type == Action.ActionType.Loop)
                    {
                        structlevel++;
                        structcount[structlevel] = 0;
                        structtype[structlevel] = g.programstep.Type;
                    }
                }
                else
                {
                    errlist += "Step " + (groups.IndexOf(g) + 1).ToString() + " not defined" + Environment.NewLine;
                }

                g.indentcomputed = displaylevel;        // store this, ASCII output want to know how we indented it.

                g.panel.Location = new Point(panelleftmargin, voff);
                g.panel.Size = new Size(panelwidth, panelheight);
                g.stepname.Location = new Point(g.right.Right + 8 + 8 * displaylevel, panelheightmargin);
                g.stepname.Size = new Size(140 - Math.Max((displaylevel - 4) * 8, 0), controlsize);
                g.value.Location = new Point(g.right.Right + 140 + 8 + 8 * 4, panelheightmargin * 2);      // 8 spacing, allow 8*4 to indent
                int valuewidth = panelwidth - 350;
                g.value.Size = new Size(valuewidth, controlsize);
                g.config.Location = new Point(g.value.Right + 4, panelheightmargin);      // 8 spacing, allow 8*4 to indent
                g.ins.Location = new Point(g.config.Right + 4, panelheightmargin);
                g.del.Location = new Point(g.ins.Right + 4, panelheightmargin);
                g.up.Location = new Point(g.del.Right + 4, panelheightmargin);

                g.up.Visible = !first;
                g.config.Visible = g.programstep != null && g.programstep.ConfigurationMenuInUse;

                //DEBUG
                if (g.programstep != null)
                {
                    //g.value.Enabled = false;
                    //g.value.Text = structlevel.ToString() + " ^ " + g.levelup + " UD: " + g.programstep.DisplayedUserData + "  PS: " + g.programstep.GetFlagList();
                    //g.value.Enabled = true;
                }

                first = false;
                voff += g.panel.Height;
            }

            buttonMore.Location = new Point(panelleftmargin, voff);
            buttonMore.Size = new Size(controlsize, controlsize);

            Rectangle screenRectangle = RectangleToScreen(this.ClientRectangle);
            int titleHeight = screenRectangle.Top - this.Top;

            // Beware Visible - it does not report back the set state, only the visible state.. hence use Enabled.
            voff += buttonMore.Height + titleHeight + panelName.Height + ((panelTop.Enabled) ? (panelTop.Height + statusStripCustom.Height) : 8) + 16 + panelOK.Height;

            this.MinimumSize = new Size(600, voff);
            this.MaximumSize = new Size(Screen.FromControl(this).WorkingArea.Width-100, Screen.FromControl(this).WorkingArea.Height-100);

            ResumeLayout();

            return errlist;
        }

        private void buttonMore_Click(object sender, EventArgs e)
        {
            CreateStep(null, -1);
        }

        private void Stepname_SelectedIndexChanged(object sender, EventArgs e)                // EVENT list changed
        {
            ExtendedControls.ComboBoxCustom b = sender as ExtendedControls.ComboBoxCustom;
            Group g = (Group)b.Tag;

            if (b.Enabled)
            {
                if (g.programstep == null || !g.programstep.Name.Equals(b.Text))
                {
                    Action a = Action.CreateAction(b.Text);

                    if (!a.ConfigurationMenuInUse || a.ConfigurationMenu(this, theme, currentvarlist))
                    {
                        g.programstep = a;
                        SetValue(g.value, a);
                        RepositionGroups();
                    }
                    else
                    {
                        b.Enabled = false; b.SelectedIndex = -1; b.Enabled = true;
                    }
                }
                else
                    ActionConfig_Clicked(g.config, null);
            }
        }

        private void ActionConfig_Clicked(object sender, EventArgs e)
        {
            ExtendedControls.ButtonExt b = sender as ExtendedControls.ButtonExt;
            Group g = (Group)b.Tag;

            if (g.programstep != null)
            {
                if (g.programstep.ConfigurationMenu(this, theme, currentvarlist))
                    SetValue(g.value, g.programstep);
            }
        }

        private void Value_Click(object sender, EventArgs e)
        {
            ExtendedControls.TextBoxBorder b = sender as ExtendedControls.TextBoxBorder;
            Group g = (Group)b.Tag;
            if (b.ReadOnly)
                ActionConfig_Clicked(g.config, null);
        }

        private void Ins_Clicked(object sender, EventArgs e)
        {
            ExtendedControls.ButtonExt b = sender as ExtendedControls.ButtonExt;
            Group g = (Group)b.Tag;

            int pos = groups.IndexOf(g);
            CreateStep(null, pos);
        }

        private void Del_Clicked(object sender, EventArgs e)
        {
            ExtendedControls.ButtonExt b = sender as ExtendedControls.ButtonExt;
            Group g = (Group)b.Tag;

            g.panel.Controls.Clear();
            panelVScroll.Controls.Remove(g.panel);
            groups.Remove(g);
            Invalidate(true);
            RepositionGroups();
        }

        private void Up_Clicked(object sender, EventArgs e)
        {
            ExtendedControls.ButtonExt b = sender as ExtendedControls.ButtonExt;
            Group g = (Group)b.Tag;

            int indexof = groups.IndexOf(g);
            groups.Remove(g);
            groups.Insert(indexof - 1, g);

            RepositionGroups();
        }

        private void Left_Clicked(object sender, EventArgs e)
        {
            ExtendedControls.ButtonExt b = sender as ExtendedControls.ButtonExt;
            Group g = (Group)b.Tag;
            g.levelup++;
            RepositionGroups();
        }

        private void Right_Clicked(object sender, EventArgs e)
        {
            ExtendedControls.ButtonExt b = sender as ExtendedControls.ButtonExt;
            Group g = (Group)b.Tag;

            g.levelup = Math.Max(g.levelup - 1, 0);

            RepositionGroups();
        }

        private void Value_TextChanged(object sender, EventArgs e)
        {
            ExtendedControls.TextBoxBorder tb = sender as ExtendedControls.TextBoxBorder;
            Group g = (Group)tb.Tag;

            if (tb.Enabled)
                g.programstep.UpdateUserData(tb.Text);
        }

        #endregion

        #region OK and Finish

        private void buttonOK_Click(object sender, EventArgs e)
        {
            string errorlist = "";

            if (textBoxBorderName.Text.Length == 0)
                errorlist = "Must have a name" + Environment.NewLine;

            else if (definedprograms != null &&          // if we have programs, and either initial name was null or its not the same now, and its in the list
                (initialprogname == null || !initialprogname.Equals(textBoxBorderName.Text))
                         && Array.Exists(definedprograms, x => x.Equals(textBoxBorderName.Text)))
            {
                errorlist = "Name chosen is already in use, pick another one" + Environment.NewLine;
            }

            if (groups.Count == 0)
                errorlist += "No action steps have been defined" + Environment.NewLine;
            else
                errorlist += RepositionGroups();

            if (errorlist.Length > 0)
            {
                string acceptstr = "Click Retry to correct errors, Abort to cancel, Ignore to accept what steps are valid";
                DialogResult dr = MessageBox.Show("Actions produced the following warnings and errors" + Environment.NewLine + Environment.NewLine + errorlist + Environment.NewLine + acceptstr,
                                        "Warning", MessageBoxButtons.AbortRetryIgnore);

                if (dr == DialogResult.Retry)
                    return;
                if (dr == DialogResult.Abort || dr == DialogResult.Cancel)
                {
                    DialogResult = DialogResult.Cancel;
                    Close();
                    return;
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        public ActionProgram GetProgram()      // call only when OK returned
        {
            ActionProgram ap = new ActionProgram(textBoxBorderName.Text);

            foreach (Group g in groups)
            {
                if (g.programstep != null)    // don't include ones not set..
                {
                    g.programstep.LevelUp = g.levelup;
                    ap.Add(g.programstep);
                }
            }

            return ap;
        }

        public string GetActionData()         // called on OK to get the actiondata string
        {
            List<string> flags = new List<string>();
            if (checkBoxCustomRefresh.Checked)
                flags.Add(ActionProgram.flagRunAtRefresh);

            return ActionData.ToJSON(flags, inputparas);
        }

        private void buttonVars_Click(object sender, EventArgs e)
        {
            ActionVariableForm avf = new ActionVariableForm();
            avf.Init("Input Parameter variables to pass to program on run", theme, inputparas);

            if (avf.ShowDialog(this) == DialogResult.OK)
            {
                inputparas = avf.result;

                currentvarlist = new List<string>(startvarlist);
                currentvarlist.AddRange(inputparas.KeyList);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonExtDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do you want to delete this program?", "Delete program", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                DialogResult = DialogResult.Abort;
                Close();
            }
        }

        #endregion

        #region Text editing


        private void buttonExtEdit_Click(object sender, EventArgs e)
        {
            try
            {
                string prog = Tools.AssocQueryString(Tools.AssocStr.Executable, ".txt");

                string filename = textBoxBorderName.Text.Length > 0 ? textBoxBorderName.Text : "Default";

                string editingloc = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Tools.SafeFileString(filename) + ".atf");

                if (SaveText(editingloc))
                {
                    while (true)
                    {
                        System.Diagnostics.Process p = new System.Diagnostics.Process();
                        p.StartInfo.FileName = prog;
                        p.StartInfo.Arguments = editingloc.QuotedEscapeString();
                        p.Start();
                        p.WaitForExit();

                        string err;
                        ActionProgram ap = ActionProgram.FromFile(editingloc, filename, out err);
                        if (ap == null)
                        {
                            DialogResult dr = MessageBox.Show("Editing produced the following errors" + Environment.NewLine + Environment.NewLine + err + Environment.NewLine +
                                                "Click Retry to correct errors, Cancel to abort editing",
                                                "Warning", MessageBoxButtons.RetryCancel);

                            if (dr == DialogResult.Cancel)
                                break;
                        }
                        else
                        {
                            DeleteAll();
                            LoadProgram(ap);
                            break;
                        }
                    }

                    return;
                }
            }
            catch
            {
            }

            MessageBox.Show("Unable to run text editor - check association for .txt files");
        }

        private void buttonExtLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.InitialDirectory = System.IO.Path.Combine(Tools.GetAppDataDirectory(), "Actions");

            if (!System.IO.Directory.Exists(dlg.InitialDirectory))
                System.IO.Directory.CreateDirectory(dlg.InitialDirectory);

            dlg.DefaultExt = "atf";
            dlg.AddExtension = true;
            dlg.Filter = "Action Text Files (*.atf)|*.atf|All files (*.*)|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(dlg.FileName))
                {
                    string err;
                    ActionProgram ap = ActionProgram.FromFile(dlg.FileName, System.IO.Path.GetFileNameWithoutExtension(dlg.FileName), out err);
                    if (ap == null)
                        MessageBox.Show("Failed to load text file" + Environment.NewLine + err);
                    else
                    {
                        DeleteAll();
                        LoadProgram(ap);
                    }
                }
            }
        }

        private void buttonExtSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.InitialDirectory = System.IO.Path.Combine(Tools.GetAppDataDirectory(), "Actions");

            if (!System.IO.Directory.Exists(dlg.InitialDirectory))
                System.IO.Directory.CreateDirectory(dlg.InitialDirectory);

            dlg.DefaultExt = ".atf";
            dlg.AddExtension = true;
            dlg.Filter = "Action Text Files (*.atf)|*.atf|All files (*.*)|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if ( !SaveText(dlg.FileName))
                    MessageBox.Show("Failed to save text file - check file path");
            }
        }

        bool SaveText(string file)
        {
            try
            {
                using (System.IO.StreamWriter sr = new System.IO.StreamWriter(file))
                {
                    sr.WriteLine("NAME " + textBoxBorderName.Text + Environment.NewLine);

                    foreach (Group g in groups)
                    {
                        if (g.programstep != null)    // don't include ones not set..
                        {
                            if (g.levelup > 0)
                                sr.WriteLine("");
                            sr.Write(new String(' ', g.indentcomputed * 4));
                            sr.WriteLine(g.programstep.Name + " " + g.programstep.UserData);
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region cut copy paste

        bool indrag = false;
        Point mouselogicalpos;      // used to offset return pos dep on which control first captured the mouse
        Point mousestart;       // where we started from
        int rightclickstep = -1;        // index of right click item on group
        static List<Action> ActionProgramCopyBuffer = new List<Action>();       // static cause we want it shared across invokations of this control

        private void panelVScroll_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && sender is Panel)
            {
                if (IsMarked)     // if already marked, turn off
                {
                    UnMark();
                }
                else
                {
                    if (sender is Panel)
                    {
                        Panel p = sender as Panel;
                        mouselogicalpos = new Point(p.Left, p.Top);
                    }
                    else
                        mouselogicalpos = new Point(0, 0);      // 0,0 is top of PanelVScroll..

                    mousestart = new Point(mouselogicalpos.X + e.Location.X, mouselogicalpos.Y + e.Location.Y);
                    indrag = true;

                    panelVScroll_MouseMove(sender, e);  // and force a move, so its marked
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (sender is ExtendedControls.PanelVScroll)
                    rightclickstep = groups.Count;      // click outside, means end
                else
                {
                    Group g = groups.Find(x => Object.ReferenceEquals(x.panel, sender));
                    if (g != null)
                        rightclickstep = groups.IndexOf(g);
                }
            }
        }

        private void panelVScroll_MouseMove(object sender, MouseEventArgs e)
        {
            if (indrag)
            {
                foreach (Group g in groups)
                {
                    int adjy = mouselogicalpos.Y + e.Location.Y;
                    g.marked = ((g.panel.Bottom >= adjy && g.panel.Top < mousestart.Y) || (g.panel.Bottom >= mousestart.Y && g.panel.Top < adjy));
                    g.panel.BackColor = (g.marked) ? Color.Red : Color.Transparent;
                }
            }
        }

        private void panelVScroll_MouseUp(object sender, MouseEventArgs e)
        {
            indrag = false;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            deleteToolStripMenuItem.Enabled = copyToolStripMenuItem.Enabled = groups.Find(x => x.marked) != null;
            pasteToolStripMenuItem.Enabled = (rightclickstep != -1 && ActionProgramCopyBuffer.Count > 0);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Paste at " + rightclickstep);
            if (rightclickstep != -1 )
            {
                int p = rightclickstep;

                if ( IsMarked )     // marked.. we note the start, then delete them..
                {
                    p = groups.FindIndex(x => x.marked);       // find index of first one, we will insert here
                    deleteToolStripMenuItem_Click(sender, e); // delete any marked
                }

                foreach (Action a in ActionProgramCopyBuffer)
                {
                    CreateStep(a, p++);
                }

                RepositionGroups();
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsMarked)
            {
                ActionProgramCopyBuffer.Clear();

                foreach(Group g in GetMarked())
                    ActionProgramCopyBuffer.Add(g.programstep);

                UnMark();

                System.Diagnostics.Debug.WriteLine("Copy " + ActionProgramCopyBuffer.Count);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Group> marked = GetMarked();

            foreach (Group g in marked)
            {
                g.panel.Controls.Clear();
                panelVScroll.Controls.Remove(g.panel);
                groups.Remove(g);
            }

            RepositionGroups();
        }

        bool IsMarked { get { return groups.Find(x => x.marked) != null; } }

        List<Group> GetMarked()
        {
            List<Group> ret = new List<Group>();
            foreach (Group g in groups)
            {
                if (g.marked)
                    ret.Add(g);
            }

            return ret;
        }

        private void UnMark()
        {
            foreach (Group g in groups)
            {
                g.panel.BackColor = Color.Transparent;
                g.marked = false;
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
            SendMessage(WM_NCLBUTTONDOWN, (System.IntPtr)HT_CAPTION, (System.IntPtr)0);
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
