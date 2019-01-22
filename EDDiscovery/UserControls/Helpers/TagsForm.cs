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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BaseUtils;

namespace EDDiscovery.Forms
{ 
    public partial class TagsForm : ExtendedControls.DraggableForm
    {
        public Dictionary<string, Image> Result;

        public class Group
        {
            public Panel panel;
            public ExtendedControls.TextBoxBorder name;
            public ExtendedControls.ButtonExt icon;
            public ExtendedControls.ButtonExt del;
        }

        List<Group> groups;

        int panelmargin = 3;
        const int vscrollmargin = 10;

        public TagsForm()
        {
            groups = new List<Group>();
            InitializeComponent();

        }

        public void Init(string t, Icon ic, Dictionary<string,Image> tags = null )
        {
            this.Icon = ic;
            bool winborder = ExtendedControls.ThemeableFormsInstance.Instance?.ApplyToForm(this, SystemFonts.DefaultFont) ?? true;
            statusStripCustom.Visible = panelTop.Visible = panelTop.Enabled = !winborder;
            this.Text = label_index.Text = t;

            if (tags != null)
            {
                foreach (string key in tags.Keys)
                {
                    CreateEntry(key, tags[key]);
                }
            }

            if (groups.Count >= 1)
                groups[0].name.Focus();
        }

        private void FormResize(object sender, EventArgs e)
        {
            FixUpGroups(false); // don't recalc min size, it creates a loop
        }

        public Group CreateEntry(string var, Image img)
        {
            Group g = new Group();

            g.panel = new Panel();
            g.panel.BorderStyle = BorderStyle.FixedSingle;

            g.name = new ExtendedControls.TextBoxBorder();
            g.name.Size = new Size(200, 24);
            g.name.Location = new Point(panelmargin, panelmargin);
            g.name.Text = var;
            g.panel.Controls.Add(g.name);
            toolTip1.SetToolTip(g.name, "Tag name");

            int nextpos = g.name.Right;

            g.icon = new ExtendedControls.ButtonExt();
            g.icon.Size = new Size(32, 32);
            g.icon.Location = new Point(g.name.Right + 8, g.name.Top);
            g.icon.Image = img;
            toolTip1.SetToolTip(g.icon, "Image");
            g.panel.Controls.Add(g.icon);
            g.icon.Click += Icon_Click;

            g.del = new ExtendedControls.ButtonExt();
            g.del.Size = new Size(24, 24);
            g.del.Location = new Point(g.icon.Right + 24, g.name.Top);
            g.del.Text = "X";
            g.del.Tag = g;
            g.del.Click += Del_Clicked;
            toolTip1.SetToolTip(g.del, "Delete entry");
            g.panel.Controls.Add(g.del);

            groups.Add(g);

            panelVScroll1.Controls.Add(g.panel);
            ExtendedControls.ThemeableFormsInstance.Instance?.ApplyToControls(g.panel, SystemFonts.DefaultFont);

            FixUpGroups();

            return g;
        }

        void FixUpGroups(bool minmax = true)      // fixes and positions groups.
        {
            int y = panelmargin;

            int panelwidth = Math.Max(panelVScroll1.Width - panelVScroll1.ScrollBarWidth, 10);

            foreach (Group g in groups)
            {
                g.panel.Size = new Size(panelwidth-panelmargin*2, 40);
                g.panel.Location = new Point(panelmargin, y);
                y += g.panel.Height + 6;
            }

            buttonMore.Location = new Point(panelmargin, y);
            buttonMore.Visible = true;

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
            //result = new Variables();
            //result_altops = new Dictionary<string, string>();

            //foreach ( Group g in groups)
            //{
            //    if (g.var.Text.Length > 0)      // only ones with names are considered
            //    {
            //        result[g.var.Text] = g.value.Text.EscapeControlChars();

            //        if (g.op != null)
            //            result_altops[g.var.Text] = g.op.Text;
            //    }
            //}

            //result_refresh = checkBoxCustomRefresh.Checked;

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
            CreateEntry("", EDDiscovery.Icons.IconSet.GetIcon("Legacy.star"));
        }

        ExtendedControls.DropDownCustom dropdown;

        private void Icon_Click(object sender, EventArgs e)
        {
            dropdown = new ExtendedControls.DropDownCustom("", true);

            List<string> Dickeys = new List<string>(EDDiscovery.Icons.IconSet.Icons.Keys);
            Dickeys.Sort();
            List<Image> images = (from x in Dickeys select EDDiscovery.Icons.IconSet.Icons[x]).ToList();

            dropdown.ItemHeight = 26;
            dropdown.FitImagesToItemHeight = true;
            dropdown.Items = Dickeys;
            dropdown.ImageItems = images;
            dropdown.FlatStyle = FlatStyle.Popup;
            dropdown.Activated += (s, ea) =>
            {
                Point location = (sender as Control).PointToScreen(new Point(0, 0));
                dropdown.Location = dropdown.PositionWithinScreen(location.X, location.Y);
                this.Invalidate(true);
            };
            dropdown.SelectedIndexChanged += (s, ea) =>
            {
            };

            dropdown.Size = new Size(400, 800);

            EDDTheme.Instance.ApplyToControls(dropdown);
            dropdown.Show(this.FindForm());
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
