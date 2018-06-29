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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Conditions
{
    public partial class ConditionFilterForm : ExtendedControls.DraggableForm
    {
        public ConditionLists result;

        List<string> eventlist;
        List<string> additionalfieldnames;          // must be set
        string initialtitle;
        bool allowoutercond;

        class Group 
        {
            public Panel panel;
            public ExtendedControls.ButtonExt upbutton;
            public ExtendedControls.ComboBoxCustom evlist;
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
            }

            public List<Conditions> condlist = new List<Conditions>();
        }

        List<Group> groups;
        public int condxoffset;     // where conditions start in x, estimate in Init

        const int panelxmargin = 3;
        const int panelymargin = 1;
        const int conditionhoff = 26;

        public delegate List<string> AdditionalNames(string ev);
        public event AdditionalNames onAdditionalNames;             // may be set to provide event driven names

        public ConditionFilterForm()
        {
            groups = new List<Group>();
            InitializeComponent();
            CancelButton = buttonCancel;
            AcceptButton = buttonOK;
        }

        // used to start when just filtering.. uses a fixed event list .. must provide a call back to obtain names associated with an event

        public void InitFilter(string t, Icon ic, List<string> events, AdditionalNames a, List<string> varfields,  ConditionLists j = null)
        {
            onAdditionalNames = a;
            this.eventlist = events;
            events.Add("All");
            Init(t, ic, events, varfields, true);
            LoadConditions(j);
        }

        // used to start when inside a condition of an IF of a program action (does not need additional names, already resolved)

        public void InitCondition(string t, Icon ic, List<string> varfields, ConditionLists j = null)
        {
            Init(t, ic, null, varfields, true);
            LoadConditions(j);
        }

        // used to start for a condition on an action form (does not need additional names, already resolved)

        public void InitCondition(string t, Icon ic, List<string> varfields, Condition j)
        {
            ConditionLists l = new ConditionLists();
            if ( j!= null && j.fields != null )
                l.Add(j);
            Init(t, ic, null, varfields, false);
            buttonMore.Visible = true;
            LoadConditions(l);
        }

        // Full start

        public void Init(   string t, Icon ic,
                            List<string> el,                             // list of event types or null if event types not used
                            List<string> varfields,                      // list of additional variable/field names (must be set)
                            bool outerconditions)                        // outc selects if group outer action can be selected, else its OR
        {
            this.Icon = ic;
            eventlist = el;
            additionalfieldnames = varfields;
            allowoutercond = outerconditions;

            // set up where a condition would start on the panel.. dep if the event list is on and the action file system is on..
            // sizes are the sizes of the controls and gaps
            condxoffset = ((eventlist != null) ? (150 + 8) : 0) +  panelxmargin + 8;

            bool winborder = ExtendedControls.ThemeableFormsInstance.Instance.ApplyToForm(this, SystemFonts.DefaultFont);
            statusStripCustom.Visible = panelTop.Visible = panelTop.Enabled = !winborder;

            this.Text = t;

            BaseUtils.Translator.Instance.Translate(this, new Control[] { label_index, buttonMore});

            initialtitle = label_index.Text = this.Text;
        }

        public void LoadConditions( ConditionLists clist )
        {
            if (clist != null)
            {
                SuspendLayout();
                this.panelOuter.SuspendLayout();
                panelVScroll.SuspendLayout();

                panelVScroll.RemoveAllControls( new List<Control> { buttonMore } );            // except these!

                groups = new List<Group>();

                Condition fe = null;
                for(int i = 0; (fe = clist.Get(i))!=null; i++)
                {
                    Group g = CreateGroupInternal(fe.eventname, fe.action, fe.actiondata, fe.innercondition.ToString(), fe.outercondition.ToString());

                    foreach (ConditionEntry f in fe.fields)
                        CreateConditionInt(g, f.itemname, ConditionEntry.MatchNames[(int)f.matchtype], f.matchstring);

                    ExtendedControls.ThemeableFormsInstance.Instance.ApplyToControls(g.panel, SystemFonts.DefaultFont);

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

            g = CreateGroupInternal(null, null, null, null, null);

            if (eventlist == null)      // if we don't have any event list, auto create a condition
                CreateConditionInt(g, null, null, null);

            ExtendedControls.ThemeableFormsInstance.Instance.ApplyToControls(g.panel, SystemFonts.DefaultFont);

            groups.Add(g);
            panelVScroll.Controls.Add(g.panel);

            FixUpGroups();

            this.Text = label_index.Text = initialtitle + " (" + groups.Count.ToString() + " conditions)";

            panelVScroll.ResumeLayout();
            ResumeLayout();

            panelVScroll.ToEnd();       // tell it to scroll to end
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

            g.innercond = new ExtendedControls.ComboBoxCustom();
            g.innercond.Items.AddRange(Enum.GetNames(typeof(ConditionEntry.LogicalCondition)));
            g.innercond.SelectedIndex = 0;
            g.innercond.Size = new Size(48, 24);
            g.innercond.Visible = false;
            if ( initialcondinner != null)
                g.innercond.Text = initialcondinner;
            g.panel.Controls.Add(g.innercond);

            g.outercond = new ExtendedControls.ComboBoxCustom();
            g.outercond.Items.AddRange(Enum.GetNames(typeof(ConditionEntry.LogicalCondition)));
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
            ExtendedControls.ThemeableFormsInstance.Instance.ApplyToControls(g.panel, SystemFonts.DefaultFont);
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
            c.cond.Items.AddRange(ConditionEntry.MatchNames);
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
                    c.fname.Enabled = !ConditionEntry.IsNullOperation(c.cond.Text);
                    if (!c.fname.Enabled)
                        c.fname.Text = "";

                    c.cond.Location = new Point(c.fname.Right + 4, vnextcond);

                    c.value.Location = new Point(c.cond.Right + 4, vnextcond + 2);
                    c.value.Size = new Size(panelwidth - condxoffset - c.fname.Width - 4 - c.cond.Width - 4 - c.del.Width - 4 - c.more.Width - 4 - g.innercond.Width - 4 - g.upbutton.Width + 8, 24);
                    c.value.Enabled = !ConditionEntry.IsNullOperation(c.cond.Text) && !ConditionEntry.IsUnaryOperation(c.cond.Text);
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

                g.panel.ResumeLayout();
            }

            buttonMore.Location = new Point(panelxmargin, y + panelVScroll.ScrollOffset);
            buttonMore.Visible = allowoutercond || groups.Count == 0;

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


#region Checking

        private string Check()
        {
            result = new ConditionLists();

            string errorlist = "";

            foreach (Group g in groups)
            {
                string innerc = g.innercond.Text;
                string outerc = g.outercond.Text;
                string evt = (eventlist != null) ? g.evlist.Text : "Default";

                //System.Diagnostics.Debug.WriteLine("Event {0} inner {1} outer {2} action {3} data '{4}'", evt, innerc, outerc, actionname, actiondata );

                Condition fe = new Condition();

                if (evt.Length == 0)        // must have name
                    errorlist += "Ignored group with empty name" + Environment.NewLine;
                else
                {
                    if (fe.Create(evt, "","", innerc, outerc)) // create must work
                    {
                        for (int i = 0; i < g.condlist.Count; i++)
                        {
                            Group.Conditions c = g.condlist[i];
                            string fieldn = c.fname.Text;
                            string condn = c.cond.Text;
                            string valuen = c.value.Text;

                            if (fieldn.Length > 0 || ConditionEntry.IsNullOperation(condn))
                            {
                                ConditionEntry f = new ConditionEntry();

                                if (f.Create(fieldn, condn, valuen))
                                {
                                    if (valuen.Length == 0 && !ConditionEntry.IsUnaryOperation(condn) && !ConditionEntry.IsNullOperation(condn))
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

                DialogResult dr = ExtendedControls.MessageBoxTheme.Show(this, "Filters produced the following warnings and errors" + Environment.NewLine + Environment.NewLine + res + Environment.NewLine + acceptstr,
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

        #region Autocomplete event field types.. complicated

        Dictionary<string, List<string>> cachedevents = new Dictionary<string, List<string>>();

        List<string> AutoCompletor(string s, ExtendedControls.AutoCompleteTextBox a)
        {
            Group g = a.Tag as Group;

            List<string> list;

            string evtype = g.evlist?.Text;

            if (evtype != null && evtype.Length > 0)       // if using event name, cache it
            {
                if (!cachedevents.ContainsKey(evtype))  //evtype MAY not be a real journal event, it may be onStartup..
                {
                    cachedevents[evtype] = new List<string>();

                    if ( onAdditionalNames != null )
                    {
                        //  List<string> classnames =

                        List<string> classnames = onAdditionalNames(evtype);
                        if (classnames != null)
                            cachedevents[evtype].AddRange(classnames);
                    }

                    cachedevents[evtype].AddRange(additionalfieldnames);
                }

                list = cachedevents[evtype];
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

        #endregion  
    }
}
