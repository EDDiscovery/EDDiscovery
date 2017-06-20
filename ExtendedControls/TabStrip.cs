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
        #region Public interfaces

        #region ctors

        public TabStrip()
        {
            InitializeComponent();
            autofade.Tick += Autofade_Tick;
            drawnPanelPopOut.Visible = false;
            labelControlText.Visible = false;
            labelControlText.Text = "";
            labelCurrent.Text = "None";
            drawnPanelPopOut.Location = panelSelected.Location;
        }

        #endregion // ctors

        #region Properties

        public Bitmap[] Images { get; set; } = null;                // ✓ (Not owned by us) images
        public string[] ToolTips { get; set; } = null;              // ✓ (Not owned by us)
        public Control CurrentControl { get; private set; } = null; // ✓ (this.Controls)


        public bool StripAtTop { get; set; } = false;

        public bool ShowPopOut { get; set; } = true;

        public int SelectedIndex { get { return si; } set { ChangePanel(value); } }

        #endregion // Properties

        #region Events

        public event ControlEventHandler ControlRemoving;           // ✓

        public event EventHandler<int> PopOut;                      // ✓

        public event EventHandler<TabCreatedEventArgs> TabCreated;  // ✓ called after TAB has been added to control, so should be sized up


        public delegate Control CreateTabMethod(TabStrip container, int displayNo);      // Create the TAB, should only make it, not configure it

        public event CreateTabMethod CreateTab;                     // ✓

        #endregion // Events

        #region Methods

        public bool ChangeTo(int i)
        {
            if (i >= 0 && i < Images.Length)
            {
                ChangePanel(i);
                return true;
            }
            else
                return false;
        }

        public void Toggle()
        {
            if (si != -1)
                ChangePanel((si + 1) % Images.Length);
        }

        public void SetControlText(string t)
        {
            labelControlText.Text = t;
            labelControlText.Visible = !tabstripvisible;
        }

        #endregion // Methods

        #endregion // Public interfaces


        #region Implementation

        private const int Spacing = 4;

        #region Fields

        private Panel[] imagepanels;            // ✓ (this.Controls)
        private Timer autofade = new Timer();   // ✓

        private bool stripopen = false;
        private bool tobevisible = false;
        private bool tabstripvisible = false;
        private int tabdisplaystart = 0;    // first tab
        private int tabdisplayed = 0;       // number of tabs
        private int si = -1;

        #endregion // Fields

        #region OnEvent overrides & dispatchers

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);

            if (StripAtTop && panelBottom.Dock != DockStyle.Top)
            {
                panelBottom.Dock = DockStyle.Top;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            tabdisplaystart = 0;        // because we will display a different set next time
        }


        protected virtual Control OnCreateTab(int displayNo)
        {
            return CreateTab?.Invoke(this, displayNo);
        }


        protected virtual void OnPopOut(int no)
        {
            PopOut?.Invoke(this, no);
        }

        protected virtual void OnControlRemoving(ControlEventArgs args)
        {
            ControlRemoving(this, args);
        }

        protected virtual void OnTabCreated(TabCreatedEventArgs args)
        {
            TabCreated?.Invoke(this, args);
        }

        #endregion // OnEvent overrides & dispatchers

        #region Methods

        private void UserDispose(bool disposing)
        {
            if (disposing && autofade != null)
            {
                if (autofade.Enabled)
                    autofade.Stop();
                autofade.Dispose();
            }

            autofade = null;
            ControlRemoving = null;
            PopOut = null;
            TabCreated = null;
            CreateTab = null;
        }

        private void ChangePanel(int i)
        {
            if ( CurrentControl != null )
            {
                OnControlRemoving(new ControlEventArgs(CurrentControl));

                Controls.Remove(CurrentControl);
                CurrentControl.Dispose();
                CurrentControl = null;
                si = -1;
                labelControlText.Text = "";
                labelCurrent.Text = "None";
            }

            CurrentControl = OnCreateTab(i);    // TAB should just create..
            if (CurrentControl != null)
            {
                si = i;

                CurrentControl.Dock = DockStyle.Fill;

                if (StripAtTop)
                {
                    Control p = this.Controls[0];
                    this.Controls.Clear();
                    this.Controls.Add(CurrentControl);
                    this.Controls.Add(p);
                }
                else
                    this.Controls.Add(CurrentControl);

                OnTabCreated(new TabCreatedEventArgs(CurrentControl, i));   // now tab is in control set, give it a chance to configure itself and set its name

                panelSelected.BackgroundImage = Images[i];
                labelCurrent.Text = CurrentControl.Name;
                labelControlText.Location = new Point(labelCurrent.Location.X + labelCurrent.Width + 16, labelControlText.Location.Y);
            }

            drawnPanelPopOut.Visible = tabstripvisible && ShowPopOut;
            panelSelected.Visible = !tabstripvisible && CurrentControl != null;
            labelCurrent.Visible = !tabstripvisible && CurrentControl != null;
            labelControlText.Visible = false; 
        }

        private void DisplayTabs(bool setvisible)
        {
            int i = 0;
            for (; i < tabdisplaystart; i++)
                imagepanels[i].Visible = false;

            bool arrowson = false;
            bool titleon = true;

            if (setvisible)
            {
                int xpos = panelSelected.Width + Spacing * 4;       // start here
                int stoppoint = DisplayRectangle.Width - Spacing; // stop here

                int spaceforarrowsandoneicon = panelArrowLeft.Width + Spacing + imagepanels[0].Width + Spacing + panelArrowRight.Width;

                if (xpos + spaceforarrowsandoneicon > stoppoint)   // no space at all !
                {
                    xpos = 0;                                       // turn off titles, use all the space
                    titleon = false;
                }

                int tabtotalwidth = 0;
                for (int ip = 0; ip < imagepanels.Length; ip++)
                    tabtotalwidth += imagepanels[ip].Width + Spacing * 2;       // do it now due to the internal scaling due to fonts

                tabtotalwidth -= Spacing * 2;           // don't count last spacing.

                if (xpos + tabtotalwidth > stoppoint)     // if total tab width (icon space icon..) too big
                {
                    panelArrowLeft.Location = new Point(xpos, 4);
                    xpos += panelArrowLeft.Width + Spacing; // move over allowing space for left and spacing
                    stoppoint -= panelArrowRight.Width + Spacing;     // allow space for right arrow plus padding
                    arrowson = true;
                }

                tabdisplayed = 0;
                for (; i < imagepanels.Length && xpos < stoppoint - Images[i].Width; i++)
                {                                           // if its soo tight, may display nothing, thats okay
                    imagepanels[i].Location = new Point(xpos, 3);
                    xpos += imagepanels[i].Width + Spacing * 2;
                    imagepanels[i].Visible = true;
                    tabdisplayed++;
                }

                if (arrowson)
                    panelArrowRight.Location = new Point(xpos, 4);
            }

            for (; i < imagepanels.Length; i++)
                imagepanels[i].Visible = false;

            panelArrowRight.Visible = panelArrowLeft.Visible = arrowson;
            panelSelected.Visible = titleon;
            labelCurrent.Visible = !setvisible;             // because text widths are so variable, dep on font/dialog units, turn off during selection
            labelControlText.Visible = !setvisible && labelControlText.Text.Length > 0;
            panelSelected.Visible = !setvisible;
            tabstripvisible = setvisible;
            drawnPanelPopOut.Visible = ShowPopOut && tabstripvisible && si != -1;
        }

        #endregion // Methods

        #region Event handlers

        private void Autofade_Tick(object sender, EventArgs e)            // hiding
        {
            autofade.Stop();

            //System.Diagnostics.Debug.WriteLine("{0} {1} Fade {2}" , Environment.TickCount, Name, tobevisible);

            if (tabstripvisible != tobevisible)
                DisplayTabs(tobevisible);
        }


        private void contextMenuStrip1_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            stripopen = false;
            Imagepanels_MouseLeave(sender, e);      // same as a mouse leave on one of the controls
        }

        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
            stripopen = true;
        }


        private void Imagepanels_Click(object sender, EventArgs e)
        {
            int i = (int)(((Panel)sender).Tag);
            ChangePanel(i);
        }

        private void Imagepanels_MouseEnter(object sender, EventArgs e)
        {
            if (imagepanels == null && Images != null)
            {
                imagepanels = new Panel[Images.Length];     // set to say 4 and you can test it with arrows

                for (int i = 0; i < imagepanels.Length; i++)
                {
                    imagepanels[i] = new Panel()
                    {
                        BackgroundImage = Images[i],
                        Tag = i,
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Visible = false
                    };

                    imagepanels[i].Size = new Size(Images[i].Width, Images[i].Height);
                    imagepanels[i].Click += Imagepanels_Click;
                    imagepanels[i].MouseEnter += Imagepanels_MouseEnter;
                    imagepanels[i].MouseLeave += Imagepanels_MouseLeave;

                    if (ShowPopOut)
                    {
                        imagepanels[i].ContextMenuStrip = contextMenuStrip1;
                        imagepanels[i].Tag = i;     // remember by index
                    }

                    if (ToolTips != null)
                    {
                        toolTip1.SetToolTip(imagepanels[i], ToolTips[i]);
                        toolTip1.ShowAlways = true;      // if not, it never appears
                    }

                    panelBottom.Controls.Add(imagepanels[i]);
                }
            }

            autofade.Stop();

            if (!tobevisible)
            {
                tobevisible = true;
                autofade.Interval = 350;
                autofade.Start();
                //System.Diagnostics.Debug.WriteLine("{0} {1} Fade in", Environment.TickCount, Name);
            }
        }

        private void Imagepanels_MouseLeave(object sender, EventArgs e)     // get this when leaving a panel and going to the icons.. so fade out slowly so it can be cancelled
        {
            autofade.Stop();

            if (tobevisible && !stripopen)
            {
                tobevisible = false;
                autofade.Interval = 750;
                autofade.Start();
                //System.Diagnostics.Debug.WriteLine("{0} {1} Fade out", Environment.TickCount, Name);
            }
        }


        private void panelArrowLeft_Click(object sender, EventArgs e)
        {
            if (tabdisplaystart > 0)
            {
                tabdisplaystart--;
                DisplayTabs(true);
            }

        }

        private void panelArrowRight_Click(object sender, EventArgs e)
        {
            if (tabdisplaystart < imagepanels.Length - tabdisplayed)
            {
                tabdisplaystart++;
                DisplayTabs(true);
            }
        }


        private void panelPopOut_Click(object sender, EventArgs e)
        {
            OnPopOut(si);
        }

        private void toolStripMenuItemPopOut_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ContextMenuStrip cms = (ContextMenuStrip)tsmi.Owner;
            Panel p = (Panel)cms.SourceControl;
            OnPopOut((int)p.Tag);
        }

        #endregion // Event handlers

        #endregion // Implementation
    }


    #region public class TabCreatedEventArgs : ControlEventArgs

    public class TabCreatedEventArgs : ControlEventArgs
    {
        public int DisplayNum { get; private set; }

        public TabCreatedEventArgs(Control control, int displayNum) : base(control)
        {
            DisplayNum = displayNum;
        }
    }

    #endregion // public class TabCreatedEventArgs : ControlEventArgs
}
