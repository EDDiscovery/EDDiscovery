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
        string[] definedprograms;
        List<string> variables;

        class Group
        {
            public Control panel;
            public Action programstep;
            public ExtendedControls.ComboBoxCustom stepname;
            public ExtendedControls.ButtonExt config;
            public ExtendedControls.ButtonExt up;
            public ExtendedControls.ButtonExt left;
            public ExtendedControls.ButtonExt right;
            public ExtendedControls.TextBoxBorder value;
            public int levelup;
        };

        List<Group> groups;

        int panelwidth = 800;
        int valuewidth = 350;

        public ActionProgramForm()
        {
            InitializeComponent();
            groups = new List<Group>();
        }

        EDDiscovery2.EDDTheme theme;
        const int vscrollmargin = 10;
        const int panelmargin = 3;

        public void Init(string t, EDDiscovery2.EDDTheme th , 
                            List<string> vbs = null ,       // list any variables you want in condition statements
                            ActionProgram prog = null ,     // give the program to display
                            string progdata = null,         // give any associated program data
                            string[] defprogs = null ,      // list any default program names
                            string suggestedname = null )   // give a suggested name, if prog is null
        {
            theme = th;

            variables = vbs;

            checkBoxCustomRefresh.Checked = progdata != null && progdata.Contains(ActionProgram.flagRunAtRefresh);

            bool winborder = theme.ApplyToForm(this, SystemFonts.DefaultFont);
            statusStripCustom.Visible = panelTop.Visible = panelTop.Enabled = !winborder;
            this.Text = label_index.Text = t;

            if (defprogs != null)
                definedprograms = defprogs;

            if (prog != null)
            {
                initialprogname = textBoxBorderName.Text = prog.Name;

                Action ac;
                int step = 0;
                while( (ac = prog.GetStep(step++)) != null )
                    CreateStep(ac);
            }
            else if (suggestedname != null )
                textBoxBorderName.Text = suggestedname;
        }

        #region CLicks

        private void buttonMore_Click(object sender, EventArgs e)
        {
            CreateStep(null,-1);
        }

        Group CreateStep(Action step = null, int insertpos = -1 )
        {
            Panel p = new Panel();
            p.Size = new Size(panelwidth, 32);

            ExtendedControls.ComboBoxCustom stepname = new ExtendedControls.ComboBoxCustom();
            stepname.Items.AddRange(Action.GetActionNameList());
            stepname.DropDownHeight = 400;
            stepname.Name = "StepName";
            if (step != null)
                stepname.Text = step.ActionName;
            stepname.SelectedIndexChanged += Stepname_SelectedIndexChanged;
            p.Controls.Add(stepname);

            ExtendedControls.ButtonExt left = new ExtendedControls.ButtonExt();
            left.Location = new Point(panelmargin + 140 + 8 + 8 * 4, panelmargin);      // 8 spacing, allow 8*4 to indent
            left.Size = new Size(24, 24);
            left.Text = "<";
            left.Click += Left_Clicked;
            p.Controls.Add(left);

            ExtendedControls.ButtonExt right = new ExtendedControls.ButtonExt();
            right.Location = new Point(left.Location.X + left.Width + 2, panelmargin);
            right.Size = new Size(24, 24);
            right.Text = ">";
            right.Click += Right_Clicked;
            p.Controls.Add(right);

            ExtendedControls.ButtonExt acconfig = new ExtendedControls.ButtonExt();
            acconfig.Text = "C";
            acconfig.Location = new Point(right.Location.X + right.Width + 8, panelmargin);
            acconfig.Size = new Size(24, 24);
            acconfig.Click += ActionConfig_Clicked;
            p.Controls.Add(acconfig);         // must be next

            ExtendedControls.TextBoxBorder value = new ExtendedControls.TextBoxBorder();
            value.Size = new Size(valuewidth, 24);
            value.Location = new Point(acconfig.Location.X + acconfig.Width + 8, panelmargin + 4);
            SetValue(value,step);
            value.TextChanged += Value_TextChanged;

            p.Controls.Add(value);         // must be next

            ExtendedControls.ButtonExt ins = new ExtendedControls.ButtonExt();
            ins.Size = new Size(24, 24);
            ins.Location = new Point(value.Location.X + value.Width + 8, panelmargin);
            ins.Text = "+";
            ins.Click += Ins_Clicked;
            p.Controls.Add(ins);

            ExtendedControls.ButtonExt del = new ExtendedControls.ButtonExt();
            del.Size = new Size(24, 24);
            del.Location = new Point(ins.Location.X + ins.Width + 8, panelmargin);
            del.Text = "X";
            del.Click += Del_Clicked;
            p.Controls.Add(del);

            ExtendedControls.ButtonExt up = new ExtendedControls.ButtonExt();
            up.Location = new Point(del.Location.X + del.Width + 8, panelmargin);
            up.Size = new Size(24, 24);
            up.Text = "^";
            up.Click += Up_Clicked;
            p.Controls.Add(up);

            Group g = new Group()
            {
                panel = p,
                stepname = stepname,
                programstep = step,
                config = acconfig,
                value = value,
                up = up,
                left = left,
                right = right,
                levelup = (step != null) ? step.LevelUp : 0
            };


            del.Tag = acconfig.Tag = stepname.Tag = up.Tag = ins.Tag = value.Tag = left.Tag = right.Tag = g;

            theme.ApplyToControls(g.panel, SystemFonts.DefaultFont);

            panelVScroll.Controls.Add(p);

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

            int voff = panelmargin;

            int structlevel = 0;
            int[] structcount = new int[50];
            bool[] structif = new bool[50];
            bool[] structelse = new bool[50];

            bool first = true;

            SuspendLayout();

            foreach( Group g in groups )
            {
                g.left.Enabled = g.right.Enabled = false;

                if ( g.levelup > 0 )
                {
                    if (g.levelup > structlevel)
                        g.levelup = structlevel;

                    structlevel -= g.levelup;
                    g.right.Enabled = true;
                }

                structcount[structlevel]++;

                if (structlevel > 0 && structcount[structlevel] > 1)        // second further on can be moved back..
                    g.left.Enabled = true;

                int level = structlevel;        // displayed level..

                if (g.programstep != null)
                {
                    if (g.programstep.IsControlElse)
                    {
                        if (structif[structlevel] == false)
                            errlist += "Step " + (groups.IndexOf(g) + 1).ToString() + " Else or ElseIf without IF found" + Environment.NewLine;
                        else if (structelse[structlevel] == true)
                            errlist += "Step " + (groups.IndexOf(g) + 1).ToString() + " Else is repeated multiple times or ElseIf after else" + Environment.NewLine;

                        if (level == 1)
                            g.left.Enabled = false;         // can't move an ELSE back to level 0

                        if (level > 0)      // display else artifically indented.. display only
                            level--;

                        structcount[structlevel] = 0;   // restart count so we don't allow a left on next one..
                        structelse[structlevel] |= g.programstep.ActionName.Equals("Else");
                    }
                    else if (g.programstep.IsControlStructureStart)
                    {
                        structlevel++;
                        structcount[structlevel] = 0;
                        structif[structlevel] = g.programstep.ActionName.Equals("If");
                        structelse[structlevel] = false;
                    }
                }
                else
                {
                    errlist += "Step " + (groups.IndexOf(g) + 1).ToString() + " not defined" + Environment.NewLine;
                }

                g.panel.Location = new Point(0, voff);
                g.stepname.Location = new Point(panelmargin + 8 * level, panelmargin);
                g.stepname.Size = new Size(140-Math.Max((level-4)*8,0), 24);
                g.up.Visible = !first;
                g.config.Visible = g.programstep != null;

                //DEBUG
//                if (g.programstep != null)
  ///              {
     //               g.value.Enabled = false;
       //             g.value.Text = structlevel.ToString() + " ^ " + g.levelup + " UD: " + g.programstep.DisplayedUserData + "  PS: " + g.programstep.GetFlagList();
         //           g.value.Enabled = true;
           //     }

                    first = false;
                voff += g.panel.Height + 4;
            }

            buttonMore.Location = new Point(panelmargin, voff);

            Rectangle screenRectangle = RectangleToScreen(this.ClientRectangle);
            int titleHeight = screenRectangle.Top - this.Top;

            // Beware Visible - it does not report back the set state, only the visible state.. hence use Enabled.
            voff += buttonMore.Height + titleHeight + panelName.Height + ((panelTop.Enabled) ? (panelTop.Height + statusStripCustom.Height) : 8) + 16 + panelOK.Height;

            this.MinimumSize = new Size(panelwidth + vscrollmargin * 2 + panelVScroll.ScrollBarWidth + 8, voff);
            this.MaximumSize = new Size(Screen.FromControl(this).WorkingArea.Width, Screen.FromControl(this).WorkingArea.Height);

            ResumeLayout();

            return errlist;
        }

        private void Stepname_SelectedIndexChanged(object sender, EventArgs e)                // EVENT list changed
        {
            ExtendedControls.ComboBoxCustom b = sender as ExtendedControls.ComboBoxCustom;
            Group g = (Group)b.Tag;

            if (b.Enabled)
            {
                if (g.programstep == null || !g.programstep.ActionName.Equals(b.Text))
                {
                    Action a = Action.CreateAction(b.Text);

                    if (a is ActionIf)
                        (a as ActionIf).variables = variables;

                    if (a.ConfigurationMenu(this, theme))
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
                if (g.programstep is ActionIf)
                    (g.programstep as ActionIf).variables = variables;

                if (g.programstep.ConfigurationMenu(this, theme))
                    SetValue(g.value, g.programstep);
            }
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

            g.levelup = Math.Max(g.levelup-1,0);

            RepositionGroups();
        }
    
        private void Value_TextChanged(object sender, EventArgs e )
        {
            ExtendedControls.TextBoxBorder tb = sender as ExtendedControls.TextBoxBorder;
            Group g = (Group)tb.Tag;

            if ( tb.Enabled )
                g.programstep.UpdateUserData(tb.Text);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            string errorlist = "";

            if ( textBoxBorderName.Text.Length == 0 )
                errorlist = "Must have a name" + Environment.NewLine;

            else if (definedprograms != null &&          // if we have programs, and either initial name was null or its not the same now, and its in the list
                (initialprogname == null || !initialprogname.Equals(textBoxBorderName.Text))
                         && Array.Exists(definedprograms, x => x.Equals(textBoxBorderName.Text)) )
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
                                        "Warning", MessageBoxButtons.AbortRetryIgnore );

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
                if ( g.programstep!= null )    // don't include ones not set..
                {
                    g.programstep.LevelUp = g.levelup;
                    ap.Add(g.programstep);
                }
            }

            return ap;
        }

        public string GetProgramData()
        {
            return checkBoxCustomRefresh.Checked ? ActionProgram.flagRunAtRefresh : "None";
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
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
