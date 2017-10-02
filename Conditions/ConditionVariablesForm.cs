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
using BaseUtils;

namespace Conditions
{ 
    public partial class ConditionVariablesForm : ExtendedControls.DraggableForm
    {
        public ConditionVariables result;      // only on OK
        public Dictionary<string, string> result_altops;
        public bool result_refresh;

        public class Group
        {
            public Panel panel;
            public ExtendedControls.TextBoxBorder var;
            public ExtendedControls.ComboBoxCustom op;
            public ExtendedControls.TextBoxBorder value;
            public ExtendedControls.ButtonExt del;
        }

        List<Group> groups;

        int panelmargin = 3;
        const int vscrollmargin = 10;

        bool showadd, shownoexpand;
        bool allowmultiple;

        public ConditionVariablesForm()
        {
            groups = new List<Group>();
            InitializeComponent();

        }

        // altops, if given, describes the operator of each variable.

        public void Init(string t, Icon ic, ConditionVariables vbs , Dictionary<string, string> altops = null,
                                                                bool showone = false ,
                                                                bool showrefresh = false , bool showrefreshstate = false,
                                                                bool allowadd = false, bool allownoexpand = false, 
                                                                bool allowmultiple = true)
        {
            this.Icon = ic;
            bool winborder = ExtendedControls.ThemeableFormsInstance.Instance.ApplyToForm(this, SystemFonts.DefaultFont);
            statusStripCustom.Visible = panelTop.Visible = panelTop.Enabled = !winborder;
            this.Text = label_index.Text = t;

            showadd = allowadd;
            shownoexpand = allownoexpand;
            this.allowmultiple = allowmultiple;

            int pos = panelmargin;
            checkBoxCustomRefresh.Enabled = checkBoxCustomRefresh.Visible = showrefresh;
            checkBoxCustomRefresh.Checked = showrefreshstate;
            checkBoxCustomRefresh.Location = new Point(pos, panelmargin);

            if (vbs != null)
            {
                foreach (string key in vbs.NameEnumuerable)
                {
                    CreateEntry(key,vbs[key], (altops!= null) ? altops[key] : "=");
                }
            }

            if ( groups.Count == 0 && showone )
            {
                CreateEntry("", "", "=");
            }

            if (groups.Count >= 1)
                groups[0].var.Focus();
        }

        private void ConditionVariablesFormResize(object sender, EventArgs e)
        {
            FixUpGroups(false); // don't recalc min size, it creates a loop
        }

        public Group CreateEntry(string var, string value, string op)
        {
            Group g = new Group();

            g.panel = new Panel();
            g.panel.BorderStyle = BorderStyle.FixedSingle;

            g.var = new ExtendedControls.TextBoxBorder();
            g.var.Size = new Size(120, 24);
            g.var.Location = new Point(panelmargin, panelmargin);
            g.var.Text = var;
            g.panel.Controls.Add(g.var);
            toolTip1.SetToolTip(g.var, "Variable name");

            int nextpos = g.var.Right;

            if (shownoexpand || showadd)
            {
                g.op = new ExtendedControls.ComboBoxCustom();
                g.op.Size = new Size(50, 24);
                g.op.Location = new Point(g.var.Right + 4, panelmargin);

                string ttip="";
                if (showadd && shownoexpand)
                {
                    g.op.Items.AddRange(new string[] { "=", "$=", "+=", "$+=" });
                    ttip = "= assign, expand, $= assign, no expansion, += add, expand, $+= add, no expansion";
                }
                else if (showadd)
                {
                    g.op.Items.AddRange(new string[] { "=", "+=" });
                    ttip = "= assign, expand, += add, expand";
                }
                else
                {
                    g.op.Items.AddRange(new string[] { "=", "$=" });
                    ttip = "= assign, expand, $= add, no expansion";
                }

                toolTip1.SetToolTip(g.op, ttip);
                toolTip1.SetToolTip(g.op.GetInternalSystemControl, ttip);

                if (g.op.Items.Contains(op))
                    g.op.SelectedItem = op;

                g.panel.Controls.Add(g.op);

                nextpos = g.op.Right;
            }


            g.value = new ExtendedControls.TextBoxBorder();
            g.value.Location = new Point(nextpos + 4, panelmargin);
            g.value.Text = value.ReplaceEscapeControlChars();
            g.value.Multiline = true;
            g.value.WordWrap = true;
            g.value.ScrollBars = ScrollBars.Vertical;
            toolTip1.SetToolTip(g.value, "Variable value");
            g.panel.Controls.Add(g.value);

            g.del = new ExtendedControls.ButtonExt();
            g.del.Size = new Size(24, 24);
            g.del.Text = "X";
            g.del.Tag = g;
            g.del.Click += Del_Clicked;
            toolTip1.SetToolTip(g.del, "Delete entry");
            g.panel.Controls.Add(g.del);

            groups.Add(g);

            panelVScroll1.Controls.Add(g.panel);
            ExtendedControls.ThemeableFormsInstance.Instance.ApplyToControls(g.panel, SystemFonts.DefaultFont);

            FixUpGroups();

            return g;
        }

        void FixUpGroups(bool minmax = true)      // fixes and positions groups.
        {
            int y = panelmargin;

            if (checkBoxCustomRefresh.Enabled)
                y += 32;

            int panelwidth = Math.Max(panelVScroll1.Width - panelVScroll1.ScrollBarWidth, 10);

            foreach (Group g in groups)
            {
                g.panel.Size = new Size(panelwidth-panelmargin*2, 64);
                g.panel.Location = new Point(panelmargin, y);
                g.value.Size = new Size(panelwidth-180 - ((g.op!=null)?50:0), 56);
                g.del.Location = new Point(g.value.Location.X + g.value.Width + 8, panelmargin);
                y += g.panel.Height + 6;
            }

            buttonMore.Location = new Point(panelmargin, y);
            buttonMore.Visible = groups.Count == 0 || allowmultiple;

            Rectangle screenRectangle = RectangleToScreen(this.ClientRectangle);
            int titleHeight = screenRectangle.Top - this.Top;

            y += buttonMore.Height + titleHeight + ((panelTop.Enabled) ? (panelTop.Height + statusStripCustom.Height) : 8) + 16 + panelOK.Height;

            if (minmax) // stop circular relationsship with resize
            {
                this.MinimumSize = new Size(600, y);
                this.MaximumSize = new Size(Screen.FromControl(this).WorkingArea.Width, Screen.FromControl(this).WorkingArea.Height);
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            result = new ConditionVariables();
            result_altops = new Dictionary<string, string>();

            foreach ( Group g in groups)
            {
                if (g.var.Text.Length > 0)      // only ones with names are considered
                {
                    result[g.var.Text] = g.value.Text.EscapeControlChars();

                    if (g.op != null)
                        result_altops[g.var.Text] = g.op.Text;
                }
            }

            result_refresh = checkBoxCustomRefresh.Checked;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void Del_Clicked(object sender, EventArgs e)
        {
            ExtendedControls.ButtonExt b = sender as ExtendedControls.ButtonExt;
            Group g = (Group)b.Tag;

            g.panel.Controls.Clear();
            panelVScroll1.Controls.Remove(g.panel);
            groups.Remove(g);
            Invalidate(true);
            FixUpGroups();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonMore_Click(object sender, EventArgs e)
        {
            CreateEntry("", "","=");
        }

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
