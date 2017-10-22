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

namespace ActionLanguage
{
    public partial class ActionProgramEditForm : ExtendedControls.DraggableForm
    {
        string filesetname;
        string initialprogname;
        string[] definedprograms;                                   // list of programs already defined, to detect rename over..

        public delegate void EditProgramFunc(string name);
        public event EditProgramFunc EditProgram; 

        List<string> currentvarlist;                                // variables available to use.. combination of above

        bool editastextimmediately = false;

        ActionProgram curprog = new ActionProgram();

        class Group     // group and curprog always have same number of entries, curprog can have a null in an entry indicating a non assigned step
        {
            public Panel panel;
            public ExtendedControls.ComboBoxCustom stepname;
            public ExtendedControls.TextBoxBorder value;
            public ExtendedControls.ButtonExt config;
            public ExtendedControls.ButtonExt up;
            public ExtendedControls.ButtonExt prog;
            public ExtendedControls.ButtonExt left;
            public ExtendedControls.ButtonExt right;
            public bool marked;     // is marked for editing

            public ActionBase checkit; // used just to check we keep in sync with curprog, not strictly ness. but useful
        }

        List<Group> groups;
        int panelheightmargin = 1;
        int panelleftmargin = 3;
        int controlsize = 22;
        int panelheight = 24;

        public ActionProgramEditForm()
        {
            groups = new List<Group>();
            InitializeComponent();
            CancelButton = buttonCancel;
            AcceptButton = buttonOK;
        }
        
        const int vscrollmargin = 10;
        const int xpanelmargin = 3;

        ActionCoreController actioncorecontroller;
        string applicationfolder;

        public void Init(string t, Icon ic, ActionCoreController cp, string appfolder,
                            List<string> vbs,              // list any variables you want in condition statements - passed to config menu, passed back up to condition, not null
                            string pfilesetname,           // file set name
                            ActionProgram prog = null,     // give the program to display
                            string[] defprogs = null,      // list any default program names
                            string suggestedname = null, bool edittext = false)   // give a suggested name, if prog is null
        {
            this.Icon = ic;
            actioncorecontroller = cp;
            applicationfolder = appfolder;
            currentvarlist = new List<string>(vbs);

            bool winborder = ExtendedControls.ThemeableFormsInstance.Instance.ApplyToForm(this, SystemFonts.DefaultFont);
            statusStripCustom.Visible = panelTop.Visible = panelTop.Enabled = !winborder;
            this.Text = label_index.Text = t;

            filesetname = pfilesetname;
            labelSet.Text = filesetname + "::";
            textBoxBorderName.Location = new Point(labelSet.Location.X + labelSet.Width + 8, textBoxBorderName.Location.Y);

            if (defprogs != null)
                definedprograms = defprogs;

            if (suggestedname != null)
                textBoxBorderName.Text = suggestedname;

            if (prog != null)
                LoadProgram(prog);

            panelVScroll.ContextMenuStrip = contextMenuStrip1;
            panelVScroll.MouseDown += panelVScroll_MouseDown;

            editastextimmediately = edittext;

#if !DEBUG
            buttonExtDisk.Visible = false;
#endif
        }

        void LoadProgram(ActionProgram prog)
        {
            foreach (Group g in groups)
            {
                g.panel.Controls.Clear();
                panelVScroll.Controls.Remove(g.panel);
            }

            groups.Clear();

            curprog = new ActionProgram(prog.Name);

            initialprogname = textBoxBorderName.Text = prog.Name;

            SuspendLayout();
            panelVScroll.SuspendLayout();

            ActionBase ac;
            int step = 0;
            while ((ac = prog.GetStep(step)) != null)
            {
                ActionBase ca = ActionBase.CreateCopy(ac);// COPY it.. so we can modify without altering current
                curprog.Add(ca);       
                CreateStep(-1, ca);
                step++;
            }

            RepositionGroups();
            panelVScroll.ResumeLayout();
            ResumeLayout();
        }

        private void panelVScroll_Resize(object sender, EventArgs e)
        {
            RepositionGroups(false); // don't recalc min size, it creates a loop
        }

        private void ActionProgramForm_Shown(object sender, EventArgs e)        
        {
            if (editastextimmediately)      // auto text feature
            {
                editastextimmediately = false;
                buttonExtEdit_Click(null, null);
            }
        }

#region Steps

        Group CreateStep(int insertpos, ActionBase step = null)
        {
            SuspendLayout();
            panelVScroll.SuspendLayout();

            Group g = new Group();

            g.checkit = step;
            
            g.panel = new Panel();
            g.panel.SuspendLayout();

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
            g.stepname.Items.AddRange(ActionBase.GetActionNameList());
            g.stepname.DropDownHeight = 400;
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

            g.up = new ExtendedControls.ButtonExt();
            g.up.Size = new Size(controlsize, controlsize);
            g.up.Text = "^";
            g.up.Click += Up_Clicked;
            g.panel.Controls.Add(g.up);

            g.prog = new ExtendedControls.ButtonExt();
            g.prog.Size = new Size(controlsize, controlsize);
            g.prog.Text = ">";
            g.prog.Click += Prog_Clicked;
            g.panel.Controls.Add(g.prog);

            g.config.Tag = g.stepname.Tag = g.up.Tag = g.value.Tag = g.left.Tag = g.right.Tag = g.prog.Tag = g;

            ExtendedControls.ThemeableFormsInstance.Instance.ApplyToControls(g.panel, SystemFonts.DefaultFont);

            panelVScroll.Controls.Add(g.panel);

            if (insertpos == -1)
                groups.Add(g);
            else
                groups.Insert(insertpos, g);

            g.panel.ResumeLayout();

            panelVScroll.ResumeLayout();
            ResumeLayout();
            return g;
        }

        void SetValue(ExtendedControls.TextBoxBorder value, ActionBase step)
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

        string RepositionGroups(bool calcminsize = true)
        {
            SuspendLayout();
            panelVScroll.SuspendLayout();

            string errlist = curprog.CalculateLevels();

            int panelwidth = Math.Max(panelVScroll.Width - panelVScroll.ScrollBarWidth, 10);
            int voff = panelheightmargin;
            int actstep = 0;

            toolTip1.RemoveAll();

            foreach (Group g in groups)
            {
                int indentlevel = 0;
                int whitespace = 0;
                int lineno = 0;
                ActionBase act = curprog.GetStep(actstep++);

                if (act != null)
                {
                    System.Diagnostics.Debug.Assert(Object.ReferenceEquals(g.checkit, act));
                    g.left.Enabled = act.calcAllowLeft;
                    g.right.Enabled = act.calcAllowRight;
                    indentlevel = act.calcDisplayLevel;
                    whitespace = act.Whitespace;
                    lineno = act.LineNumber;
                    g.prog.Visible = act.Type == ActionBase.ActionType.Call & EditProgram != null;
                    g.config.Visible = act.ConfigurationMenuInUse;
                }
                else
                {
                    g.left.Enabled = g.right.Enabled = false;
                    g.prog.Visible = false;
                    g.config.Visible = false;
                }

                g.panel.SuspendLayout();

                g.panel.Location = new Point(panelleftmargin, voff + panelVScroll.ScrollOffset);
                g.panel.Size = new Size(panelwidth, panelheight + ((whitespace>0) ? (panelheight/2) : 0 ));
                g.stepname.Location = new Point(g.right.Right + 8 + 8 * indentlevel, panelheightmargin);
                g.stepname.Size = new Size(140 - Math.Max((indentlevel - 4) * 8, 0), controlsize);
                g.value.Location = new Point(g.right.Right + 140 + 8 + 8 * 4+ 8, panelheightmargin * 2);      // 8 spacing, allow 8*4 to indent
                int valuewidth = panelwidth - 350;
                g.value.Size = new Size(valuewidth, controlsize);
                g.config.Location = new Point(g.value.Right + 4, panelheightmargin);      // 8 spacing, allow 8*4 to indent
                g.up.Location = new Point(g.config.Right + 4, panelheightmargin);
                g.prog.Location = new Point(g.up.Right + 4, panelheightmargin);
                g.up.Visible = groups.IndexOf(g)>0;

                g.panel.ResumeLayout();

                string tt1 = "Step " + actstep;
                if ( indentlevel>0)
                    tt1 += " Lv " + indentlevel;
                if ( lineno > 0 )
                    tt1 += " Ln " + lineno;
                if ( act != null )
                    tt1 += " SL " + act.calcStructLevel + " LU" + act.LevelUp ;

                toolTip1.SetToolTip(g.stepname, tt1);
                toolTip1.SetToolTip(g.stepname.GetInternalSystemControl, tt1);
                if (act != null && act.Comment.Length > 0)
                    toolTip1.SetToolTip(g.value, "Comment: " + act.Comment);

                //DEBUG Keep this useful for debugging structure levels
                //                if (g.programstep != null)
                //                  g.value.Enabled = false; g.value.Text = structlevel.ToString() + " ^ " + g.levelup + " UD: " + g.programstep.DisplayedUserData;g.value.Enabled = true;

                voff += g.panel.Height;
            }

            buttonMore.Location = new Point(panelleftmargin, voff + panelVScroll.ScrollOffset);
            buttonMore.Size = new Size(controlsize, controlsize);

            Rectangle screenRectangle = RectangleToScreen(this.ClientRectangle);
            int titleHeight = screenRectangle.Top - this.Top;

            // Beware Visible - it does not report back the set state, only the visible state.. hence use Enabled.
            voff += buttonMore.Height + titleHeight + panelName.Height + ((panelTop.Enabled) ? (panelTop.Height + statusStripCustom.Height) : 8) + 16 + panelOK.Height;

            if (calcminsize)
            {
                this.MinimumSize = new Size(600, voff);
                this.MaximumSize = new Size(Screen.FromControl(this).WorkingArea.Width - 100, Screen.FromControl(this).WorkingArea.Height - 100);

                if (Bottom > Screen.FromControl(this).WorkingArea.Height)
                    Top = Screen.FromControl(this).WorkingArea.Height - Height - 50;
            }

            panelVScroll.ResumeLayout();
            ResumeLayout();

            return errlist;
        }

        private void buttonMore_Click(object sender, EventArgs e)
        {
            CreateStep(-1,null);
            curprog.Add(null);
            RepositionGroups();
            panelVScroll.ToEnd();       // tell it to scroll to end
        }

        private void Stepname_SelectedIndexChanged(object sender, EventArgs e)                // EVENT list changed
        {
            ExtendedControls.ComboBoxCustom b = sender as ExtendedControls.ComboBoxCustom;

            if (b.Enabled)
            {
                Group g = (Group)b.Tag;
                int gstep = groups.IndexOf(g);

                ActionBase curact = curprog.GetStep(gstep);

                if (curact == null || !curact.Name.Equals(b.Text))
                {
                    ActionBase a = ActionBase.CreateAction(b.Text);

                    if (!a.ConfigurationMenuInUse || a.ConfigurationMenu(this, actioncorecontroller, currentvarlist))
                    {
                        curprog.SetStep(gstep, a);
                        g.checkit = a;
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
            ActionBase curact = curprog.GetStep(groups.IndexOf(g));

            if (curact != null)
            {
                if (curact.ConfigurationMenu(this, actioncorecontroller, currentvarlist))
                    SetValue(g.value, curact);
            }
        }

        private void Value_Click(object sender, EventArgs e)
        {
            ExtendedControls.TextBoxBorder b = sender as ExtendedControls.TextBoxBorder;
            Group g = (Group)b.Tag;
            if (b.ReadOnly)
                ActionConfig_Clicked(g.config, null);
        }

        private void Up_Clicked(object sender, EventArgs e)
        {
            ExtendedControls.ButtonExt b = sender as ExtendedControls.ButtonExt;
            Group g = (Group)b.Tag;
            int gstep = groups.IndexOf(g);

            groups.RemoveAt(gstep);
            groups.Insert(gstep - 1, g);
            curprog.MoveUp(gstep);

            RepositionGroups();
        }

        private void Prog_Clicked(object sender, EventArgs e)
        {
            ExtendedControls.ButtonExt b = sender as ExtendedControls.ButtonExt;
            Group g = (Group)b.Tag;
            ActionBase curact = curprog.GetStep(groups.IndexOf(g));

            if (curact != null)
            {
                string pname = ((ActionCall)curact).GetProgramName();
                if (pname != null)
                    EditProgram(curact.UserData);
                else
                    ExtendedControls.MessageBoxTheme.Show(this,"No program name assigned");
            }
        }

        private void Left_Clicked(object sender, EventArgs e)
        {
            ExtendedControls.ButtonExt b = sender as ExtendedControls.ButtonExt;
            Group g = (Group)b.Tag;
            int step = groups.IndexOf(g);
            ActionBase curact = curprog.GetStep(step);
            if (curact != null)
            {
                curact.LevelUp++;
                ActionBase nextact = curprog.GetStep(step + 1);

                if (!curact.IsStructStart && nextact != null)            // move next up back 1 level, to keep it the same  but if its a struct start dont
                    nextact.LevelUp = Math.Max(nextact.LevelUp - 1, 0);
            }

            RepositionGroups();
        }

        private void Right_Clicked(object sender, EventArgs e)
        {
            ExtendedControls.ButtonExt b = sender as ExtendedControls.ButtonExt;
            Group g = (Group)b.Tag;
            int step = groups.IndexOf(g);
            ActionBase curact = curprog.GetStep(step);
            if (curact != null)
            {
                curact.LevelUp = Math.Max(curact.LevelUp - 1, 0);
                ActionBase nextact = curprog.GetStep(step + 1);

                if (!curact.IsStructStart && nextact != null)            // move next up back 1 level, to keep it the same  but if its a struct start dont
                    nextact.LevelUp++;
            }

            RepositionGroups();
        }

        private void Value_TextChanged(object sender, EventArgs e)
        {
            ExtendedControls.TextBoxBorder tb = sender as ExtendedControls.TextBoxBorder;
            Group g = (Group)tb.Tag;
            ActionBase curact = curprog.GetStep(groups.IndexOf(g));

            if (tb.Enabled && curact != null )
                curact.UpdateUserData(tb.Text);
        }

#endregion

#region OK and Finish

        private string ErrorList()
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
                errorlist += curprog.CalculateLevels();

            return errorlist;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            string errorlist = ErrorList();

            if (errorlist.Length > 0)
            {
                string acceptstr = "Click Retry to correct errors, Abort to cancel, Ignore to accept what steps are valid";
                DialogResult dr = ExtendedControls.MessageBoxTheme.Show(this,"Actions produced the following warnings and errors" + Environment.NewLine + Environment.NewLine + errorlist + Environment.NewLine + acceptstr,
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
            curprog.Rename(textBoxBorderName.Text.Trim());
            Close();
        }

        public ActionProgram GetProgram()      // call only when OK returned
        {
            ActionProgram ap = new ActionProgram(curprog.Name, curprog.StoredInSubFile);
            ActionBase ac;
            int step = 0;
            while ((ac = curprog.GetStep(step++)) != null)
            {
                if ( ac != null )
                    ap.Add(ac);
            }

            return ap;
        }


        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonExtDelete_Click(object sender, EventArgs e)
        {
            if (ExtendedControls.MessageBoxTheme.Show(this, "Do you want to delete this program?", "Delete program", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                DialogResult = DialogResult.Abort;
                Close();
            }
        }

#endregion

#region Text editing

        private void buttonExtEdit_Click(object sender, EventArgs e)
        {
            curprog.Rename(textBoxBorderName.Text.Trim());
            if ( curprog.EditInEditor())
            {
                LoadProgram(curprog);
            }
        }

        private void buttonExtLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.InitialDirectory = applicationfolder;

            if (!System.IO.Directory.Exists(dlg.InitialDirectory))
                System.IO.Directory.CreateDirectory(dlg.InitialDirectory);

            dlg.DefaultExt = "atf";
            dlg.AddExtension = true;
            dlg.Filter = "Action Text Files (*.atf)|*.atf|All files (*.*)|*.*";

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(dlg.FileName))
                {
                    ActionProgram ap = new ActionProgram();
                    string err = ap.ReadFile(dlg.FileName);

                    if (err.Length>0)
                        ExtendedControls.MessageBoxTheme.Show(this,"Failed to load text file" + Environment.NewLine + err);
                    else
                    {
                        LoadProgram(ap);
                    }
                }
            }
        }

        private void buttonExtSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog(false);                                          // save as a file..
        }

        private void buttonExtDisk_Click(object sender, EventArgs e)        // save as new disk file..
        {
            SaveFileDialog(true);
        }

        private void SaveFileDialog(bool associate)
        {
            string errlist = ErrorList();
            if ( errlist.Length > 0 )
            {
                ExtendedControls.MessageBoxTheme.Show(this, "Program contains errors, correct first\r\n" + errlist);
            }
            else
            { 
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.InitialDirectory = applicationfolder;

                if (!System.IO.Directory.Exists(dlg.InitialDirectory))
                    System.IO.Directory.CreateDirectory(dlg.InitialDirectory);

                dlg.DefaultExt = ".atf";
                dlg.AddExtension = true;
                dlg.Filter = "Action Text Files (*.atf)|*.atf|All files (*.*)|*.*";

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    curprog.Rename(textBoxBorderName.Text.Trim());
                    if ( associate )
                        curprog.SetSubFileStorage(dlg.FileName);        // now

                    if (!curprog.WriteFile(dlg.FileName))
                        ExtendedControls.MessageBoxTheme.Show(this, "Failed to save text file - check file path");
                    else if (associate)
                    {
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                }
            }
        }

#endregion

#region cut copy paste

        bool indrag = false;
        Point mouselogicalpos;      // used to offset return pos dep on which control first captured the mouse
        Point mousestart;       // where we started from
        int rightclickstep = -1;        // index of right click item on group
        static List<ActionBase> ActionProgramCopyBuffer = new List<ActionBase>();       // static cause we want it shared across invokations of this control

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
                    else
                        rightclickstep = -1;
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
            bool validrightclick = rightclickstep >= 0 && rightclickstep < groups.Count;

            insertEntryAboveToolStripMenuItem.Enabled = whitespaceToolStripMenuItem.Enabled = removeWhitespaceToolStripMenuItem.Enabled = validrightclick || IsMarked;
            deleteToolStripMenuItem.Enabled = copyToolStripMenuItem.Enabled = validrightclick || IsMarked;
            pasteToolStripMenuItem.Enabled = ActionProgramCopyBuffer.Count > 0 && rightclickstep >=0;
            editCommentToolStripMenuItem.Enabled = validrightclick && curprog.GetStep(rightclickstep) != null;

//            System.Diagnostics.Debug.WriteLine("Rightclick at " + rightclickstep + " marked " + IsMarked);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int p = rightclickstep;

            if (IsMarked)     // marked.. we note the start, then delete them..
            {
                p = groups.FindIndex(x => x.marked);       // find index of first one, we will insert here
                deleteToolStripMenuItem_Click(sender, e); // delete any marked
            }

            foreach (ActionBase a in ActionProgramCopyBuffer)
            {
                CreateStep(p,a);
                curprog.Insert(p, a);
                p++;
            }

            RepositionGroups();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActionProgramCopyBuffer.Clear();
            AddRightClickToMarkIfRequired();

            foreach (Group g in GetMarked())
            {
                ActionBase curact = curprog.GetStep(groups.IndexOf(g));
                if (curact != null)
                    ActionProgramCopyBuffer.Add(ActionBase.CreateCopy(curact));
            }

            UnMark();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddRightClickToMarkIfRequired();

            foreach (Group g in GetMarked())
            {
                int gstep = groups.IndexOf(g);
                g.panel.Controls.Clear();
                panelVScroll.Controls.Remove(g.panel);
                groups.RemoveAt(gstep);
                curprog.Delete(gstep);
            }

            RepositionGroups();
        }

        private void whitespaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddRightClickToMarkIfRequired();

            foreach (Group g in GetMarked())
            {
                ActionBase curact = curprog.GetStep(groups.IndexOf(g));
                if (curact != null)
                    curact.Whitespace = 1;
            }

            UnMark();
            RepositionGroups();
        }

        private void removeWhitespaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddRightClickToMarkIfRequired();

            foreach (Group g in GetMarked())
            {
                ActionBase curact = curprog.GetStep(groups.IndexOf(g));
                if (curact != null)
                    curact.Whitespace = 0;
            }

            UnMark();
            RepositionGroups();
        }

        private void insertEntryAboveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddRightClickToMarkIfRequired();

            List<Group> ret = GetMarked();

            if (ret.Count > 0)
            {
                int startindex = groups.IndexOf(ret[0]);
                for (int i = 0; i < ret.Count; i++)
                {
                    CreateStep(startindex, null);
                    curprog.Insert(startindex, null);
                }
            }

            UnMark();
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

        void AddRightClickToMarkIfRequired()
        {
            if (!IsMarked && rightclickstep >= 0)
            {
                groups[rightclickstep].marked = true;
            }
        }

        private void editCommentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActionBase c = curprog.GetStep(rightclickstep);         // we know step and ACT is valid since it would be disabled otherwise
            string r = ExtendedControls.PromptSingleLine.ShowDialog(this, "Comment", c.Comment, "Edit Comment for " + c.Name, this.Icon, false, "Enter comment for action" );
            if (r != null)
            {
                c.Comment = r;
                RepositionGroups();
            }
        }

        #endregion



        private void label_index_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void label_index_MouseUp(object sender, MouseEventArgs e)
        {
            OnCaptionMouseUp((Control)sender, e);
        }

        private void panel_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel_close_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
