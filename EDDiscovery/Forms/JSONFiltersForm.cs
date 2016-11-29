using EDDiscovery;
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
    public partial class JSONFiltersForm : Form
    {
        public JSONFilter result;

        class Group
        {
            public Control panel;
            public string[] fieldnames;
            public ExtendedControls.ButtonExt upbutton;
            public ExtendedControls.ComboBoxCustom evlist;
            public ExtendedControls.ComboBoxCustom actionlist;
            public ExtendedControls.ComboBoxCustom innercond;
            public ExtendedControls.ComboBoxCustom outercond;
            public Label outerlabel;
        };

        List<Group> groups;
        string actionlist;
        bool allowoutercond;
        public int panelwidth;
        public int hoffset;
        EDDiscovery2.EDDTheme theme;
        const int vscrollmargin = 10;
        const int panelmargin = 3;
        const int conditionhoff = 30;

        public JSONFiltersForm()
        {
            InitializeComponent();
            groups = new List<Group>();
        }

        public void Init(string t, string aclist, bool outc, EDDiscovery2.EDDTheme th, JSONFilter j = null)  // if aclist has one action, action selector not shown. 
        {                                                                        // outc selects if group outer action can be selected, else its OR
            theme = th;
            actionlist = aclist;
            allowoutercond = outc;
            hoffset = (actionlist.Split(';').Count()>1) ? (288) : (160);
            panelwidth = hoffset + 620;
            result = j;

            if (result != null)
            {
                foreach (JSONFilter.FilterEvent fe in result.filters)
                {
                    Group g = CreateGroup(fe.eventname, fe.action, fe.innercondition.ToString(), fe.outercondition.ToString());

                    foreach (JSONFilter.Fields f in fe.fields)
                    {
                        CreateCondition(g, f.itemname, JSONFilter.MatchNames[(int)f.matchtype], f.contentmatch);
                    }
                }
            }

            bool winborder = theme.ApplyToForm(this, SystemFonts.DefaultFont);
            statusStripCustom.Visible = panelTop.Visible = !winborder;
            this.Text = label_index.Text = t;
        }

        private void JSONFiltersForm_Load(object sender, EventArgs e)
        {
        }

        private void JSONFiltersForm_Resize(object sender, EventArgs e)
        {
//            this.Size = new Size(panelwidth + margin * 2 + panelVScroll.ScrollBarWidth + (this.Bounds.Width - this.ClientRectangle.Width), this.Height);
        }

        private void JSONFiltersForm_Layout(object sender, LayoutEventArgs e)
        {
            bool windowsborder = this.FormBorderStyle == FormBorderStyle.Sizable;

            if (windowsborder)
            {
                panelVScroll.Location = new Point(0, 0);
                panelVScroll.Size = new Size(ClientRectangle.Width, ClientRectangle.Height);
            }
            else
            {
                panelVScroll.Location = new Point(vscrollmargin, panelTop.Height);
                panelVScroll.Size = new Size(ClientRectangle.Width - vscrollmargin * 2, ClientRectangle.Height - panelTop.Height - statusStripCustom.Height);
            }

            panelVScroll.PerformLayout();
        }



        Group CreateGroup(string initialev = null, string initialaction = null, 
                                string initialcondinner = null , string initialcondouter = null )
        {
            Panel p = new Panel();
            p.BorderStyle = BorderStyle.FixedSingle;

            ExtendedControls.ComboBoxCustom evliste = new ExtendedControls.ComboBoxCustom();
            evliste.Items.AddRange(EDDiscovery.EliteDangerous.JournalEntry.GetListOfEventsWithOptMethod(false).ToArray());
            evliste.Items.Add("All");
            evliste.Location = new Point(panelmargin, panelmargin);
            evliste.Size = new Size(140, 24);
            evliste.DropDownHeight = 400;
            evliste.Name = "EVList";
            if (initialev != null)
                evliste.Text = initialev;
            evliste.SelectedIndexChanged += Evlist_SelectedIndexChanged;
            p.Controls.Add(evliste);

            ExtendedControls.ComboBoxCustom aclist = new ExtendedControls.ComboBoxCustom();
            string[] actions = actionlist.Split(';');
            aclist.Items.AddRange(actions);
            aclist.Location = new Point(evliste.Location.X+evliste.Width+8, panelmargin);
            aclist.DropDownHeight = 400;
            aclist.Size = new Size(120, 24);
            aclist.Name = "ActionList";
            aclist.Enabled = actions.Count() > 1;           // 1 item, indicate disabled.. won't be displayed
            if (initialaction != null)
                aclist.Text = initialaction;
            else
                aclist.SelectedIndex = 0;
            aclist.Visible = false;
            p.Controls.Add(aclist);

            ExtendedControls.ComboBoxCustom cond = new ExtendedControls.ComboBoxCustom();
            cond.Items.AddRange(Enum.GetNames(typeof(JSONFilter.FilterType)));
            cond.SelectedIndex = 0;
            cond.Size = new Size(60, 24);
            cond.Visible = false;
            cond.Name = "InnerCond";
            if ( initialcondinner != null)
                cond.Text = initialcondinner;
            p.Controls.Add(cond);

            ExtendedControls.ComboBoxCustom condouter = new ExtendedControls.ComboBoxCustom();
            condouter.Items.AddRange(Enum.GetNames(typeof(JSONFilter.FilterType)));
            condouter.SelectedIndex = 0;
            condouter.Location = new Point(evliste.Location.X , panelmargin + conditionhoff);
            condouter.Size = new Size(60, 24);
            condouter.Enabled = condouter.Visible = false;
            if (initialcondouter != null)
                condouter.Text = initialcondouter;
            cond.Name = "OuterCond";
            p.Controls.Add(condouter);

            Label lab = new Label();
            lab.Text = " with group(s) above";
            lab.Location = new Point(condouter.Location.X + condouter.Width + 4, condouter.Location.Y + 3);
            lab.AutoSize = true;
            lab.Visible = false;
            p.Controls.Add(lab);

            ExtendedControls.ButtonExt up = new ExtendedControls.ButtonExt();
            up.Size = new Size(24, 24);
            up.Text = "^";
            up.Click += Up_Click;
            p.Controls.Add(up);

            int hoff = (aclist.Enabled) ? (aclist.Location.X + aclist.Width + 50) : (evliste.Location.X + evliste.Width + 50);

            Group g = new Group()
            {
                panel = p,
                evlist = evliste,
                upbutton = up,
                actionlist = aclist,
                outercond = condouter,
                innercond = cond,
                outerlabel = lab
            };

            p.Size = new Size(panelwidth, panelmargin + conditionhoff);
            up.Location = new Point(panelwidth - 20 - panelmargin - 4, panelmargin);

            groups.Add(g);

            if (initialev != null)
            {
                ChangeEventTypes(g);
            }

            up.Tag = g;
            evliste.Tag = g;
            panelVScroll.Controls.Add(p);

            theme.ApplyToControls(g.panel);
            RepositionGroup(g);
            PositionPanels();

            return g;
        }

        void CreateCondition( Group g , string initialfname = null , string initialcond = null, string initialvalue = null )
        {
            ExtendedControls.ComboBoxCustom fname = new ExtendedControls.ComboBoxCustom();
            fname.Size = new Size(140, 24);
            fname.DropDownHeight = 400;
            fname.Name = "Field";
            if (g.fieldnames != null)
                fname.Items.AddRange(g.fieldnames);
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
            cond.Items.AddRange(JSONFilter.MatchNames);
            cond.SelectedIndex = 0;
            cond.Size = new Size(130, 24);
            cond.DropDownHeight = 400;

            if (initialcond != null)
                cond.Text = Tools.SplitCapsWord(initialcond);

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
            more.Click += ConditionClick;
            more.Tag = g;
            g.panel.Controls.Add(more);

            theme.ApplyToControls(g.panel);
            RepositionGroup(g);
            PositionPanels();
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

        int RepositionGroup(Group g)
        {
            int vnextcond = panelmargin;
            int numcond = 0;
            Control lastadd = null;

            g.innercond.Visible = false;

            for (int i = 0; i < g.panel.Controls.Count; i++)        // position, enable controls
            {
                if (g.panel.Controls[i].Name.Equals("Field"))
                {
                    g.panel.Controls[i].Location = new Point(hoffset, vnextcond);
                    g.panel.Controls[i + 1].Location = new Point(g.panel.Controls[i].Location.X + g.panel.Controls[i].Width + 8, vnextcond);
                    g.panel.Controls[i + 2].Location = new Point(g.panel.Controls[i + 1].Location.X + g.panel.Controls[i + 1].Width + 8, vnextcond+4);
                    g.panel.Controls[i + 3].Location = new Point(g.panel.Controls[i + 2].Location.X + g.panel.Controls[i + 2].Width + 8, vnextcond);
                    g.panel.Controls[i + 4].Location = new Point(g.panel.Controls[i + 3].Location.X + g.panel.Controls[i + 3].Width + 8, vnextcond);
                    g.panel.Controls[i + 4].Visible = true;

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

            int minh = panelmargin + conditionhoff + ((g.outercond.Enabled) ? (g.outercond.Height + 8) : 0);
            g.panel.Size = new Size(g.panel.Width, Math.Max(vnextcond, minh));

            return numcond;
        }

        void PositionPanels()
        {
            int y = 0;
            bool showup = false;

            foreach (Group g in groups)
            {
                g.upbutton.Visible = showup;
                showup = true;
                g.panel.Location = new Point(0, y);
                y += g.panel.Height + 6;
            }

            buttonMore.Location = new Point(0, y);
            buttonOK.Location = new Point(panelwidth- 75, y);
            buttonCancel.Location = new Point(panelwidth - 75-75-10, y);

            Rectangle screenRectangle = RectangleToScreen(this.ClientRectangle);
            int titleHeight = screenRectangle.Top - this.Top;

            y += buttonMore.Height + titleHeight + ((panelTop.Visible) ? (panelTop.Height + statusStripCustom.Height) : 8) + 8;

            this.MinimumSize = new Size(panelwidth+vscrollmargin*2+panelVScroll.ScrollBarWidth + 8, y );
            this.MaximumSize = new Size(Screen.FromControl(this).WorkingArea.Width, Screen.FromControl(this).WorkingArea.Height);
        }


        private void buttonMore_Click(object sender, EventArgs e)
        {
            CreateGroup();
        }

        private void ConditionClick(object sender, EventArgs e)
        {
            ExtendedControls.ButtonExt b = sender as ExtendedControls.ButtonExt;
            Group g = (Group)b.Tag;
            CreateCondition(g);
        }

        private void Up_Click(object sender, EventArgs e)
        {
            ExtendedControls.ButtonExt b = sender as ExtendedControls.ButtonExt;
            Group g = (Group)b.Tag;
            int indexof = groups.IndexOf(g);
            groups.Remove(g);
            groups.Insert(indexof - 1,g);
            PositionPanels();
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

            int numcond = RepositionGroup(g);

            if (numcond == 0)
            {
                panelVScroll.Controls.Remove(g.panel);
                g.panel.Controls.Clear();
                groups.Remove(g);
            }

            PositionPanels();
        }

        private void Evlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExtendedControls.ComboBoxCustom b = sender as ExtendedControls.ComboBoxCustom;
            Group g = (Group)b.Tag;

            bool onefieldpresent = false;
            foreach (Control c in g.panel.Controls)
            {
                if (c.Name.Equals("Field"))
                    onefieldpresent = true;
            }

            if (!onefieldpresent)
                CreateCondition(g);

            ChangeEventTypes(g);
        }

        private void ChangeEventTypes(Group g)
        {
            g.actionlist.Visible = g.actionlist.Enabled;
            string evtype = g.evlist.Text;
            List<EDDiscovery.EliteDangerous.JournalEntry> jel = EDDiscovery.EliteDangerous.JournalEntry.Get(evtype);        // get all events of this type

            if (jel != null)
            {
                HashSet<string> fields = new HashSet<string>();             // Hash set prevents duplication
                foreach (EDDiscovery.EliteDangerous.JournalEntry ev in jel)
                    JSONHelper.GetJSONFieldNames(ev.EventDataString, fields);        // for all events, add to field list

                g.fieldnames = fields.ToArray();        // keep in group in case more items are to be added

                foreach (Control c in g.panel.Controls)
                {
                    if (c.Name.Equals("Field"))
                    {
                        ExtendedControls.ComboBoxCustom cb = c as ExtendedControls.ComboBoxCustom;
                        cb.Items.Clear();
                        cb.Items.AddRange(g.fieldnames);
                        cb.Items.Add("User Defined");
                        cb.SelectedIndex = -1;
                        cb.Text = "";
                    }
                }
            }

            for (int i = 0; i < groups.Count; i++)
            {
                bool showouter = false;
                for (int j = i - 1; j >= 0; j--)
                {
                    if (groups[j].evlist.Text.Equals(groups[i].evlist.Text) && groups[i].evlist.Text.Length > 0)
                        showouter = true;
                }

                showouter &= allowoutercond;

                groups[i].outercond.Enabled = groups[i].outercond.Visible = groups[i].outerlabel.Visible = showouter;
                RepositionGroup(groups[i]);
            }

            PositionPanels();
        }


        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            JSONFilter jf = new JSONFilter();

            string errorlist = "";

            foreach (Group g in groups)
            {
                string innerc = g.innercond.Text;
                string outerc = g.outercond.Text;
                string action = g.actionlist.Text;
                string evt = g.evlist.Text;
                System.Diagnostics.Debug.WriteLine("Event {0} inner {1} outer {2}", evt, innerc, outerc);

                JSONFilter.FilterEvent fe = new JSONFilter.FilterEvent();

                if (evt.Length > 0)
                {
                    if (fe.Create(evt, action, "", innerc, outerc))
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

                                System.Diagnostics.Debug.WriteLine("  {0} {1} {2}", fieldn, condn, valuen);

                                if (fieldn.Length > 0)
                                {
                                    JSONFilter.Fields f = new JSONFilter.Fields();
                                    ok = (fieldn.Length > 0 && f.Create(fieldn, condn, valuen));

                                    if (ok)
                                    {
                                        if (valuen.Length == 0)
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
                                jf.Add(fe);
                            else
                                errorlist += "No valid filters found in group '" + fe.eventname + "'" + Environment.NewLine;
                        }
                    }
                    else
                        errorlist += "Should not happen create failed" + Environment.NewLine;
                }
                else
                    errorlist += "Ignored group with empty name" + Environment.NewLine;
            }

            if (errorlist.Length > 0)
            {
                bool anything = jf.Count > 0;

                string acceptstr = (!anything) ? "Click Retry to correct errors, or Cancel to abort" : "Click Retry to correct errors, Abort to cancel, Ignore to accept what filters are valid";
                DialogResult dr = MessageBox.Show("Filters produced the following warnings and errors" + Environment.NewLine + Environment.NewLine + errorlist + Environment.NewLine + acceptstr,
                                        "Warning", (anything) ? MessageBoxButtons.AbortRetryIgnore : MessageBoxButtons.RetryCancel);

                if (dr == DialogResult.Retry)
                    return;
                if (dr == DialogResult.Abort || dr == DialogResult.Cancel)
                {
                    DialogResult = DialogResult.Cancel;
                    Close();
                    return;
                }
            }

            result = jf;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void panel_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel_close_Click(object sender, EventArgs e)
        {
            Close();
        }

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

    }
}
