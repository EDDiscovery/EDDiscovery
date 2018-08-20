/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace ExtendedControls
{
    public partial class TabStrip : UserControl
    {
        public enum StripModeType { StripTop, StripBottom, ListSelection };
        public StripModeType StripMode { get; set; } = StripModeType.StripTop;
        public Image EmptyPanelIcon { get; set; } = Properties.Resources.Stop;
        public Image[] ImageList;     // images
        public int[] ListSelectionItemSeparators;   // any separators for ListSelection
        public string[] TextList;       // text associated - tooltips or text on list selection
        public object[] TagList;      // tags for them..
        public bool ShowPopOut { get; set; }= true; // Pop out icon show

        // if you set this, when empty, a panel will appear with the color selected
        public Color EmptyColor { get { return emptypanelcolor; } set { emptypanelcolor = value; Invalidate(); } }
        public float EmptyColorScaling { get; set; } = 0.5F;

        public int TabFieldSpacing { get; set; } = 8;

        // only if using ListSelection:
        public int DropDownWidth { get; set; } = 400;
        public int DropDownHeight { get; set; } = 200;
        public Color DropDownBackgroundColor { get; set; } = Color.Gray;
        public Color DropDownBorderColor { get; set; } = Color.Green;
        public Color DropDownScrollBarColor { get; set; } = Color.LightGray;
        public Color DropDownScrollBarButtonColor { get; set; } = Color.LightGray;
        public Color DropDownMouseOverBackgroundColor { get; set; } = Color.Red;
        public Color DropDownItemSeperatorColor { get; set; } = Color.Purple;

        // Selected
        public int SelectedIndex { get { return si; } set { ChangePanel(value); } }
        public Control CurrentControl;

        // events
        public Action<TabStrip, Control> OnRemoving;
        public Func<TabStrip, int,Control> OnCreateTab;
        public Action<TabStrip, Control, int> OnPostCreateTab;
        public Action<TabStrip, int> OnPopOut;

        // internals
        private Panel[] imagepanels;
        private Timer autofadeinouttimer = new Timer();
        private int si = -1;

        private Timer autorepeat = new Timer();
        private int autorepeatdir = 0;

        private Color emptypanelcolor = Color.Empty;         // default empty means use base back color.. ambient property

        public TabStrip()
        {
            InitializeComponent();
            labelControlText.Text = "";
            labelTitle.Text = "Select";
            autofadeinouttimer.Tick += AutoFadeInOutTick;
            autorepeat.Interval = 200;
            autorepeat.Tick += Autorepeat_Tick;
            panelSelectedIcon.BackgroundImage = EmptyPanelIcon;
            SetIconVisibility();
        }


        #region Public interface

        public void Toggle()
        {
            if (si != -1)
                ChangePanel((si + 1) % ImageList.Length);
        }

        public bool ChangePanel(int i)     // with bounds checking
        {
            if (i >= 0 && i < ImageList.Length)
            {
                Close();
                Create(i);
                PostCreate();
                return true;
            }
            else
                return false;
        }

        public void Close()     // close down
        {
            if (CurrentControl != null)
            {
                if (OnRemoving != null)
                    OnRemoving(this, CurrentControl);

                this.Controls.Remove(CurrentControl);
                CurrentControl.Dispose();
                CurrentControl = null;
                si = -1;
                labelControlText.Text = "";
                labelTitle.Text = "Select";
                panelSelectedIcon.BackgroundImage = EmptyPanelIcon;
            }
        }

        public void Create(int i)       // create.. only if already closed
        {
            if (OnCreateTab != null)
            {
                //System.Diagnostics.Debug.WriteLine("Panel " + i + " make");
                CurrentControl = OnCreateTab(this, i);      // TAB should just create..

                if (CurrentControl != null)
                {
                    si = i;

                    CurrentControl.Dock = DockStyle.Fill;

                    AddControlToView(CurrentControl);

                    //System.Diagnostics.Debug.WriteLine("Panel " + i + " post create " + CurrentControl.Name);

                    if (i < ImageList.Length)
                        panelSelectedIcon.BackgroundImage = ImageList[i];   // paranoia..

                    labelTitle.Text = CurrentControl.Name;
                }
            }

            SetIconVisibility();
        }

        public void PostCreate()        // ask for post create phase
        {
            if (CurrentControl != null && OnPostCreateTab != null )
            {
                OnPostCreateTab(this, CurrentControl, si);       // now tab is in control set, give it a chance to configure itself and set its name
            }
        }

        public void SetControlText(string t)
        {
            labelControlText.Text = t;
            SetIconVisibility();
        }

        #endregion

        #region Common 

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (emptypanelcolor != Color.Empty && CurrentControl == null)       // this seems the best way to display the non used
            {
                Rectangle area = panelStrip.Dock == DockStyle.Top ? new Rectangle(0, panelStrip.Height, ClientRectangle.Width, ClientRectangle.Height - panelStrip.Height) : new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height - panelStrip.Height);

                using (Brush b = new System.Drawing.Drawing2D.LinearGradientBrush(area, emptypanelcolor, emptypanelcolor.Multiply(EmptyColorScaling), 90))
                {
                    e.Graphics.FillRectangle(b, area);
                }
            }
        }

        private void AddControlToView(Control c)
        {
            SuspendLayout();

            Controls.Clear();

            if (StripMode != StripModeType.StripBottom)
            {
                Controls.Add(c);
                Controls.Add(panelStrip);
            }
            else
            {
                Controls.Add(panelStrip);
                Controls.Add(c);
            }

            ResumeLayout();
        }

        private void TabStrip_Layout(object sender, LayoutEventArgs e)
        {
            if (StripMode == StripModeType.StripTop && panelStrip.Dock != DockStyle.Top)
            {
                panelStrip.Dock = DockStyle.Top;
            }
        }

        private void TabStrip_Resize(object sender, EventArgs e)
        {
            tabdisplaystart = 0;        // because we will display a different set next time
        }

        private void panelPopOut_Click(object sender, EventArgs e)
        {
            if (OnPopOut != null)
                OnPopOut(this, si);
        }

        #endregion

        #region Show hide info control

        bool tobevisible = false;   // records if going to be shown during pausing
        bool tabstripvisible = false; // is shown
        bool selectioniconvisible = true; // Only used for tab icon strips, turns off first icon

        void SetIconVisibility()
        {
            bool showpopout = ShowPopOut && tabstripvisible && si != -1;

            drawnPanelPopOut.Visible = showpopout && selectioniconvisible;
            panelSelectedIcon.Visible = !showpopout && selectioniconvisible;

            drawnPanelPopOut.Location = panelSelectedIcon.Location;     // same position, mutually exclusive

            if (StripMode == StripModeType.ListSelection)
            {
                drawnPanelListSelection.Location = new Point(panelSelectedIcon.Right + TabFieldSpacing, panelSelectedIcon.Top);
                labelTitle.Location = new Point(tabstripvisible ? drawnPanelListSelection.Right+ TabFieldSpacing : drawnPanelListSelection.Left, labelTitle.Top);
                labelControlText.Location = new Point(labelTitle.Right + TabFieldSpacing, labelControlText.Top);
                labelTitle.Visible = true;
                labelControlText.Visible = labelControlText.Text.Length > 0;
                drawnPanelListSelection.Visible = tabstripvisible;
            }
            else
            {
                labelTitle.Location = new Point(33, 8);
                labelTitle.Visible = !tabstripvisible;
                labelControlText.Location = new Point(labelTitle.Right + TabFieldSpacing, labelControlText.Top);
                labelControlText.Visible = !tabstripvisible && labelControlText.Text.Length > 0;
                drawnPanelListSelection.Visible = false;
            }
        }

        private void MouseEnterPanelObjects(object sender, EventArgs e)
        {
            if (StripMode != StripModeType.ListSelection && imagepanels == null && ImageList != null)  // on first entry..
            {
                imagepanels = new Panel[ImageList.Length];

                for (int i = 0; i < imagepanels.Length; i++)
                {
                    imagepanels[i] = new Panel()
                    {
                        BackgroundImage = ImageList[i],
                        Tag = i,
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Visible = false
                    };

                    imagepanels[i].Size = new Size(ImageList[i].Width, ImageList[i].Height);
                    imagepanels[i].Click += TabIconClicked;
                    imagepanels[i].MouseEnter += MouseEnterPanelObjects;
                    imagepanels[i].MouseLeave += MouseLeavePanelObjects;

                    if (ShowPopOut)
                    {
                        imagepanels[i].ContextMenuStrip = contextMenuStrip1;
                        imagepanels[i].Tag = i;     // remember by index
                    }

                    if (TextList != null)
                    {
                        toolTip1.SetToolTip(imagepanels[i], TextList[i]);
                        toolTip1.ShowAlways = true;      // if not, it never appears
                    }

                    panelStrip.Controls.Add(imagepanels[i]);
                }
            }

            autofadeinouttimer.Stop();

            if (!tabstripvisible)
            {
                tobevisible = true;
                autofadeinouttimer.Interval = 350;
                autofadeinouttimer.Start();
                //System.Diagnostics.Debug.WriteLine("{0} {1} Fade in", Environment.TickCount, Name);
            }
        }

        private void MouseLeavePanelObjects(object sender, EventArgs e)     // get this when leaving a panel and going to the icons.. so fade out slowly so it can be cancelled
        {
            autofadeinouttimer.Stop();

            if (tabstripvisible && !keepstripopen)      // if going visible, but not due to strip opening
            {
                tobevisible = false;            // go invisible.. after interval
                autofadeinouttimer.Interval = 750;
                autofadeinouttimer.Start();
                //System.Diagnostics.Debug.WriteLine("{0} {1} Fade out", Environment.TickCount, Name);
            }
        }


        void AutoFadeInOutTick(object sender, EventArgs e)            // hiding
        {
            autofadeinouttimer.Stop();

            //System.Diagnostics.Debug.WriteLine("{0} {1} Fade {2}" , Environment.TickCount, Name, tobevisible);

            if (tabstripvisible != tobevisible)
            {
                if (StripMode == StripModeType.ListSelection)
                    DisplayListIcon(tobevisible);
                else
                    DisplayTabs(tobevisible);
            }
        }

        #endregion

        #region Implementation - As Tab strip

        int tabdisplaystart = 0;    // first tab
        int tabdisplayed = 0;       // number of tabs
        const int Spacing = 4;      // spacing distance

        void DisplayTabs( bool setvisible )
        {
            int i = 0;
            for (; i < tabdisplaystart; i++)
                imagepanels[i].Visible = false;

            bool arrowson = false;

            selectioniconvisible = true;        // presume we can show it
            tabstripvisible = setvisible;       // also setting selectioniconvisible..

            if (setvisible)
            {
                int xpos = panelSelectedIcon.Width + Spacing*4;       // start here
                int stoppoint = DisplayRectangle.Width - Spacing; // stop here

                int spaceforarrowsandoneicon = panelArrowLeft.Width + Spacing + imagepanels[0].Width + Spacing + panelArrowRight.Width;

                if ( xpos + spaceforarrowsandoneicon > stoppoint)   // no space at all !
                {
                    xpos = 0;                                       // turn off titles, use all the space
                    selectioniconvisible = false;
                }

                int tabtotalwidth = 0;
                for (int ip = 0; ip < imagepanels.Length; ip++)
                {
                    tabtotalwidth += imagepanels[ip].Width + Spacing * 2;       // do it now due to the internal scaling due to fonts
                   // System.Diagnostics.Debug.WriteLine("Image panel size " + ip + " w " + imagepanels[ip].Width + " width " + Images[ip].Width);
                }

                tabtotalwidth -= Spacing * 2;           // don't count last spacing.

                if ( xpos + tabtotalwidth > stoppoint )     // if total tab width (icon space icon..) too big
                {
                    panelArrowLeft.Location = new Point(xpos, 4);
                    xpos += panelArrowLeft.Width + Spacing; // move over allowing space for left and spacing
                    stoppoint -= panelArrowRight.Width + Spacing;     // allow space for right arrow plus padding
                    arrowson = true;
                }

                
                tabdisplayed = 0;
                for (; i < imagepanels.Length && xpos < stoppoint - ImageList[i].Width; i++)
                {                                           // if its soo tight, may display nothing, thats okay
                    imagepanels[i].Location = new Point(xpos, 3);
                    xpos += imagepanels[i].Width + Spacing * 2;
                    imagepanels[i].Visible = true;
                    tabdisplayed++;
                }

                
                if ( arrowson )
                    panelArrowRight.Location = new Point(xpos, 4);
            }

            for (; i < imagepanels.Length;i++)
                imagepanels[i].Visible = false;

            
            panelArrowRight.Visible = panelArrowLeft.Visible = arrowson;
            SetIconVisibility();
        }

        public void TabIconClicked(object sender, EventArgs e)
        {
            int i = (int)(((Panel)sender).Tag);
            ChangePanel(i);
        }

        private void panelArrowLeft_MouseDown(object sender, MouseEventArgs e)
        {
            autorepeatdir = -1;
            ClickArrow();
        }

        private void panelArrowLeft_MouseUp(object sender, MouseEventArgs e)
        {
            autorepeat.Stop();
        }

        private void panelArrowRight_MouseDown(object sender, MouseEventArgs e)
        {
            autorepeatdir = 1;
            ClickArrow();
        }

        private void panelArrowRight_MouseUp(object sender, MouseEventArgs e)
        {
            autorepeat.Stop();
        }

        private void ClickArrow()
        {
            autorepeat.Stop();

            int newpos = tabdisplaystart + autorepeatdir;
            if ( newpos >= 0 && newpos <= imagepanels.Length - tabdisplayed)
            {
                tabdisplaystart = newpos;
                DisplayTabs(true);
                autorepeat.Start();
            }
        }

        private void Autorepeat_Tick(object sender, EventArgs e)
        {
            ClickArrow();
        }

        #endregion

        #region Implementation - as Pop Out list

        DropDownCustom dropdown;

        private void drawnPanelListSelection_Click(object sender, EventArgs e)
        {
            dropdown = new DropDownCustom("", true);

            dropdown.SelectionBackColor = this.DropDownBackgroundColor;
            dropdown.ForeColor = this.ForeColor;
            dropdown.BackColor = this.DropDownBorderColor;
            dropdown.BorderColor = this.DropDownBorderColor;
            dropdown.ScrollBarColor = this.DropDownScrollBarColor;
            dropdown.ScrollBarButtonColor = this.DropDownScrollBarButtonColor;
            dropdown.MouseOverBackgroundColor = this.DropDownMouseOverBackgroundColor;
            dropdown.ItemSeperatorColor = this.DropDownItemSeperatorColor;

            dropdown.ItemHeight = ImageList[0].Size.Height+2;
            dropdown.Items = TextList.ToList();
            dropdown.ItemSeperators = ListSelectionItemSeparators;
            dropdown.ImageItems = ImageList.ToList();
            dropdown.FlatStyle = FlatStyle.Popup;
            dropdown.Activated += (s,ea) => 
            {
                Point location = drawnPanelListSelection.PointToScreen(new Point(0, 0));
                dropdown.Location = dropdown.PositionWithinScreen(location.X + drawnPanelListSelection.Width, location.Y);
                this.Invalidate(true);
            };
            dropdown.SelectedIndexChanged += (s, ea) =>
            {
                ChangePanel(dropdown.SelectedIndex);
            };

            dropdown.Deactivate += (s, ea) =>               // will also be called on selected index because we have auto close on (in constructor)
            {
                keepstripopen = false;
                MouseLeavePanelObjects(sender, e);      // same as a mouse leave on one of the controls
            };

            dropdown.Size = new Size(DropDownWidth, DropDownHeight);
            dropdown.Show(this.FindForm());
            keepstripopen = true;
        }

        private void DisplayListIcon( bool setvisible )
        {
            tabstripvisible = setvisible;
            SetIconVisibility();
        }

        #endregion

        #region Right Click

        private void toolStripMenuItemPopOut_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ContextMenuStrip cms = (ContextMenuStrip)tsmi.Owner;
            Panel p = (Panel)cms.SourceControl;

            if (OnPopOut != null)
                OnPopOut(this, (int)p.Tag);
        }

        bool keepstripopen = false;

        private void contextMenuStrip1_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            keepstripopen = false;
            MouseLeavePanelObjects(sender, e);      // same as a mouse leave on one of the controls
        }

        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
            keepstripopen = true;
        }

        #endregion

    }
}
