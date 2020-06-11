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

namespace EDDiscovery.UserControls
{ 
    public partial class TagsForm : ExtendedControls.DraggableForm
    {
        public Dictionary<string, Image> Result;

        public class Group
        {
            public Panel panel;
            public ExtendedControls.ExtTextBox name;
            public ExtendedControls.ExtButton icon;
            public ExtendedControls.ExtButton del;
        }

        List<Group> groups;

        int panelmargin = 2;

        #region Init

        public TagsForm()
        {
            groups = new List<Group>();
            InitializeComponent();

            BaseUtils.Translator.Instance.Translate(this, new Control[] { });
            BaseUtils.Translator.Instance.Translate(toolTip, this);
        }

        public void Init(string t, Icon ic, Dictionary<string,Image> tags = null )
        {
            this.Icon = ic;
            bool winborder = ExtendedControls.ThemeableFormsInstance.Instance?.ApplyDialog(this) ?? true;
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

            g.name = new ExtendedControls.ExtTextBox();
            g.name.Size = new Size(250, 24);
            g.name.Location = new Point(panelmargin, panelmargin+6);
            g.name.Text = var;
            g.panel.Controls.Add(g.name);
            toolTip.SetToolTip(g.name, "Tag name".T(EDTx.TagsForm_TN));

            int nextpos = g.name.Right;

            g.icon = new ExtendedControls.ExtButton();
            g.icon.Size = new Size(28, 28);
            g.icon.Location = new Point(g.name.Right + 8, panelmargin);
            g.icon.Image = img;
            g.icon.Click += Icon_Click;
            g.icon.ImageLayout = ImageLayout.Stretch;
            toolTip.SetToolTip(g.icon, "Select image for this tag".T(EDTx.TagsForm_SI));
            g.panel.Controls.Add(g.icon);

            g.del = new ExtendedControls.ExtButton();
            g.del.Size = new Size(24, 24);
            g.del.Location = new Point(g.icon.Right + 24, panelmargin+2);
            g.del.Text = "X";
            g.del.Tag = g;
            g.del.Click += Del_Clicked;
            toolTip.SetToolTip(g.del, "Delete entry".T(EDTx.TagsForm_DE));
            g.panel.Controls.Add(g.del);

            groups.Add(g);

            panelVScroll1.Controls.Add(g.panel);
            ExtendedControls.ThemeableFormsInstance.Instance?.ApplyDialog(g.panel);

            FixUpGroups();

            return g;
        }

        void FixUpGroups(bool minmax = true)      // fixes and positions groups.
        {
            int y = panelmargin;

            int panelwidth = Math.Max(panelVScroll1.Width - panelVScroll1.ScrollBarWidth, 10);

            foreach (Group g in groups)
            {
                g.panel.Size = new Size(panelwidth-panelmargin*2, 36);
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
                this.MinimumSize = new Size(400, y);
                this.MaximumSize = new Size(Screen.FromControl(this).WorkingArea.Width, Screen.FromControl(this).WorkingArea.Height);
            }
        }

        #endregion

        #region UI

        ExtendedControls.ExtListBoxForm dropdown;

        private void Icon_Click(object sender, EventArgs e)
        {
            ExtendedControls.ExtButton but = sender as ExtendedControls.ExtButton;

            dropdown = new ExtendedControls.ExtListBoxForm("", true);

            List<string> Dickeys = new List<string>(BaseUtils.Icons.IconSet.Instance.Icons.Keys);
            Dickeys.Sort();
            List<Image> images = (from x in Dickeys select BaseUtils.Icons.IconSet.Instance.Icons[x]).ToList();

            dropdown.FitImagesToItemHeight = true;
            dropdown.Items = Dickeys;
            dropdown.ImageItems = images;
            dropdown.FlatStyle = FlatStyle.Popup;
            dropdown.PositionBelow(sender as Control);
            dropdown.SelectedIndexChanged += (s, ea) =>
            {
                Image img = images[dropdown.SelectedIndex];
                but.Image = img;
            };

            EDDTheme.Instance.ApplyDialog(dropdown, true);
            dropdown.Show(this.FindForm());
        }

        private void Del_Clicked(object sender, EventArgs e)
        {
            ExtendedControls.ExtButton b = sender as ExtendedControls.ExtButton;
            Group g = (Group)b.Tag;

            g.panel.Controls.Clear();
            panelVScroll1.Controls.Remove(g.panel);
            groups.Remove(g);
            Invalidate(true);
            FixUpGroups();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Result = new Dictionary<string, Image>();
            foreach (Group g in groups)
            {
                if (g.name.Text.Length > 0)      // only ones with names are considered
                    Result[g.name.Text] = g.icon.Image;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonMore_Click(object sender, EventArgs e)
        {
            CreateEntry("", BaseUtils.Icons.IconSet.GetIcon("Legacy.star"));
        }

        private void panel_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region Draggables

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

        #endregion


    }
}
