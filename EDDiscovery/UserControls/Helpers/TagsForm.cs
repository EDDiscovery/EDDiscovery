/*
 * Copyright © 2017-2024 EDDiscovery development team
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
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{ 
    public partial class TagsForm : ExtendedControls.DraggableForm
    {
        public Dictionary<string, string> Result;
        private class Group
        {
            public Panel panel;
            public ExtendedControls.ExtTextBox name;
            public ExtendedControls.ExtButton icon;
            public string iconname;
            public ExtendedControls.ExtButton del;
        }

        private List<Group> groups;
        private string tagssplitstr;

        private int panelmargin = 2;

        #region Init

        public TagsForm()
        {
            groups = new List<Group>();
            InitializeComponent();

            BaseUtils.TranslatorMkII.Instance.TranslateControls(this);
            BaseUtils.TranslatorMkII.Instance.TranslateTooltip(toolTip,this);
        }

        public void Init(string t, Icon ic, string tagsplitstring, Dictionary<string,string> tags = null )
        {
            this.Icon = ic;
            this.tagssplitstr = tagsplitstring;
            bool winborder = ExtendedControls.Theme.Current?.ApplyDialog(this) ?? true;
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

        private Group CreateEntry(string var, string iconname)
        {
            Group g = new Group();
            g.panel = new Panel();
            g.panel.BorderStyle = BorderStyle.FixedSingle;

            g.name = new ExtendedControls.ExtTextBox();
            g.name.Size = new Size(250, 24);
            g.name.Location = new Point(panelmargin, panelmargin+3);
            g.name.Text = var;
            g.name.KeyPress += (s, e) => { if (e.KeyChar == tagssplitstr[0]) e.Handled = true; };   // prevent entry of split char
            g.panel.Controls.Add(g.name);
            toolTip.SetToolTip(g.name, "Tag name".Tx());

            int nextpos = g.name.Right;

            g.icon = new ExtendedControls.ExtButton();
            g.icon.Location = new Point(g.name.Right + 8, panelmargin);
            g.icon.Size = new Size(28, 28);     // override autosize after theming
            g.icon.Click += Icon_Click;
            g.icon.Tag = g;
            toolTip.SetToolTip(g.icon, "Select image for this tag".Tx());
            g.panel.Controls.Add(g.icon);

            g.del = new ExtendedControls.ExtButton();
            g.del.Size = new Size(24, 24);
            g.del.Location = new Point(g.icon.Right + 24, panelmargin+2);
            g.del.Text = "X";
            g.del.Tag = g;
            g.del.Click += Del_Clicked;
            toolTip.SetToolTip(g.del, "Delete entry".Tx());
            g.panel.Controls.Add(g.del);

            groups.Add(g);

            panelVScroll1.Controls.Add(g.panel);
            ExtendedControls.Theme.Current?.ApplyDialog(g.panel);

            g.iconname = iconname;
            g.icon.AutoSize = false;    // buttons are normally autosized due to theming, turn it off now
            g.icon.Image = BaseUtils.Icons.IconSet.GetIcon(iconname);     // assign image - it may be big

            FixUpGroups();

            g.name.Focus();

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

        private void Icon_Click(object sender, EventArgs e)
        {
            ExtendedControls.ExtButton but = sender as ExtendedControls.ExtButton;
            Group g = but.Tag as Group;

            var cfs = new ExtendedControls.CheckedIconNewListBoxForm();

            string[] names = BaseUtils.Icons.IconSet.Instance.Names();
            Array.Sort(names);

            foreach (var n in names)
            {
                cfs.UC.AddButton(n, n.SplitCapsWordFull().Replace(" . ", " "), 
                                BaseUtils.Icons.IconSet.Instance.Get(n));
            }

            cfs.PositionBelow(but);
            cfs.UC.ImageSize = new Size(24, 24);
            cfs.CloseBoundaryRegion = new Size(32, 32);
            cfs.UC.MultiColumnSlide = true;

            cfs.UC.ButtonPressed += (index, itemtag, itemname, usertag, me) => 
            { 
                but.Image = BaseUtils.Icons.IconSet.Instance.Get(itemtag);      // reset image and tag
                g.iconname = itemtag;
                cfs.Close(); 
            };

            cfs.Show(this);
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
            Result = new Dictionary<string, string>();
            foreach (Group g in groups)
            {
                if (g.name.Text.Length > 0)      // only ones with names are considered
                {
                    Result[g.name.Text] = g.iconname;
                }
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
            CreateEntry("", "Legacy.star");
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

        #region Tag Helpers

        static public void EditTags(Form frm, Dictionary<string,string> configuredtags, string extratags, string currentsetting, 
                        Point loc, Action<string, object> callback, object tag, string tagsplitstr, bool allornone = false, bool addemptyor = false )
        {
            ExtendedControls.CheckedIconNewListBoxForm cfs = new ExtendedControls.CheckedIconNewListBoxForm();
            cfs.UC.SettingsSplittingChar = tagsplitstr[0];
            cfs.CloseBoundaryRegion = new Size(32, 32);
            cfs.AllOrNoneBack = allornone;      
            cfs.SaveSettings += callback;
            cfs.UC.AddAllNone();
            
            if ( addemptyor )
            {
                cfs.UC.Add("||", "||");
                cfs.UC.Add("< >", "< >");
                cfs.UC.NoneAllIgnore = "||" +tagsplitstr;
            }

            foreach (var x in configuredtags.Keys)
                cfs.UC.Add(x, x, BaseUtils.Icons.IconSet.Instance.Get(configuredtags[x]));

            foreach (var usertag in extratags.SplitNoEmptyStartFinish(tagsplitstr[0]))       // we may have older tags not in the configuredtags lets list them
            {
                if (!configuredtags.ContainsKey(usertag))
                    cfs.UC.Add(usertag, usertag, EDDiscovery.Icons.Controls.Star);
            }

            cfs.UC.Sort();
            cfs.UC.ImageSize = new Size(24, 24);
            cfs.Name = $"Edit tags {DateTime.UtcNow} {tag}";

            System.Diagnostics.Debug.WriteLine($"EditTags {cfs.Name}");
            cfs.Show(currentsetting, loc, frm, tag);
        }

        // Paint Tags into a DGV
        static public void PaintTags(string[] taglist, Dictionary<string,string> images, Rectangle area, Graphics gr, int tagsize)
        {
            //System.Diagnostics.Debug.WriteLine("Row " + e.RowIndex + " Tags '" + tagstring.Count + "'");

            int tagspacing = tagsize + 2;
            int startx = area.X;
            int across = Math.Max(area.Width / tagspacing, 1);

            area.Width = tagsize;
            area.Height = tagsize;

            int tagscount = 0;
            for (int i = 0; i < taglist.Length; i++)
            {
                Image img = images.TryGetValue(taglist[i], out string imagename) ? BaseUtils.Icons.IconSet.Instance.Get(imagename) : EDDiscovery.Icons.Controls.Star;
                gr.DrawImage(img, area);
                if (i % across == across - 1)
                {
                    area.X = startx;
                    area.Y += tagspacing;
                }
                else
                    area.X += tagspacing;
                tagscount++;
            }
        }

        // fill a panel control with tag images and hook to click and tooltip
        static public void FillTags(string[] taglist,  Dictionary<string, string> images, Control panelTags, MouseEventHandler click, ToolTip toolTip)
        {
            panelTags.Controls.Clear();
            int pos = 2;
            for (int i = 0; i < taglist.Length; i++)
            {
                Image img = images.TryGetValue(taglist[i], out string imagename) ? BaseUtils.Icons.IconSet.Instance.Get(imagename) : EDDiscovery.Icons.Controls.Star;
                Panel p = new Panel();
                p.BackgroundImage = img;
                p.Bounds = new System.Drawing.Rectangle(pos, 1, 28, 28);
                p.BackgroundImageLayout = ImageLayout.Stretch;
                p.MouseDown += click;
                panelTags.Controls.Add(p);
                toolTip.SetToolTip(p, taglist[i]);
                pos += 32;
            }

            toolTip.SetToolTip(panelTags, string.Join(Environment.NewLine, taglist));
        }

        // take a list of tags<separ> values and form a new list with unique tags only
        static public string UniqueTags(List<string> taglist, string tagsplitstr)
        {
            HashSet<string> tags = new HashSet<string>();
            foreach( var t in taglist)
            {
                var ttags = t.SplitNoEmptyStartFinish(tagsplitstr[0]);
                foreach (var x in ttags)
                    tags.Add(x);
            }
            return string.Join(tagsplitstr, tags) + tagsplitstr;  // return it with normal trailing separ as per the editing form
        }

        static public bool AreTagsInFilter(object tag, string tagfilter, string tagsplitstr)
        {
            string itemtags = tag as string;
            if ( tagfilter == ExtendedControls.CheckedIconUserControl.All || itemtags == null)
                return true;

            var tagfilterarray = tagfilter.SplitNoEmptyStartFinish(tagsplitstr[0]);

            bool orcondition = tagfilter.Contains("||" + tagsplitstr);
            if (orcondition)
            {
                if (itemtags == "" && tagfilter.Contains("< >" + tagsplitstr))
                    return true;

                foreach (var t in tagfilterarray)     // we need the string to contain t<separ>
                {
                    if (itemtags.Contains(t + tagsplitstr))
                        return true;
                }

                return false;
            }
            else
            {
                if (itemtags == "" && !tagfilter.Contains("< >" + tagsplitstr))        // we must be empty
                    return false;

                var itemtagarray = itemtags.SplitNoEmptyStartFinish(tagsplitstr[0]);
                
                if (itemtagarray.Length != tagfilterarray.Length)       // we must have same lists
                    return false;

                foreach (var t in itemtagarray)     
                {
                    if (Array.IndexOf(tagfilterarray, t) < 0)
                        return false;
                }

                return true;
            }

        }

        // work out tags min display height
        static public void SetMinHeight(int tagcount, DataGridViewRow rw, int width, int tagsize)
        {
            int tagspacing = tagsize + 2;
            int across = Math.Max(width / tagspacing, 1);
            int height = ((tagcount - 1) / across + 1) * tagspacing;
            // System.Diagnostics.Debug.WriteLine($"Count {taglist.Length} Row {rw.Index} height {height} across {across}");
            rw.MinimumHeight = Math.Max(height, tagspacing);
        }

        #endregion

    }
}
