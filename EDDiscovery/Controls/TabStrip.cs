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

namespace EDDiscovery.Controls
{
    public partial class TabStrip : UserControl
    {
        public bool StripAtTop { get; set;  } = false;
        public Bitmap[] Images;     // images
        public string[] ToolTips;
        public bool ShowPopOut { get; set; }= true;
        
        public int SelectedIndex { get { return si; } set { ChangePanel(value); } }
        public Control CurrentControl;

        public delegate void RemoveTab(TabStrip t, Control c);
        public event RemoveTab OnRemoving;

        public delegate Control CreateTab(TabStrip t, int no);          // Create the TAB, should only make it, not configure it
        public event CreateTab OnCreateTab;

        public delegate void PostCreateTab(TabStrip t, Control ctrl , int no);  // called after TAB has been added to control, so should be sized up
        public event PostCreateTab OnPostCreateTab;

        public delegate void PopOut(TabStrip t, int no);
        public event PopOut OnPopOut;

        private Panel[] imagepanels;
        private Timer autofade = new Timer();
        private int si = -1;

        public TabStrip()
        {
            InitializeComponent();
            autofade.Tick += FadeInOut;
            drawnPanelPopOut.Visible = false;
            labelControlText.Visible = false;
            labelControlText.Text = "";
            labelCurrent.Text = "None";
            drawnPanelPopOut.Location = panelSelected.Location;
        }

        void ChangePanel(int i)
        {
            if ( CurrentControl != null )
            {
                if (OnRemoving!=null)
                    OnRemoving(this,CurrentControl);

                this.Controls.Remove(CurrentControl);
                CurrentControl.Dispose();
                CurrentControl = null;
                si = -1;
                labelControlText.Text = "";
                labelCurrent.Text = "None";
            }

            if (OnCreateTab != null)
            {
                CurrentControl = OnCreateTab(this, i);      // TAB should just create..

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

                    OnPostCreateTab(this, CurrentControl, i);       // now tab is in control set, give it a chance to configure itself and set its name

                    panelSelected.BackgroundImage = Images[i];
                    labelCurrent.Text = CurrentControl.Name;
                    labelControlText.Location = new Point(labelCurrent.Location.X + labelCurrent.Width + 16, labelControlText.Location.Y);
                }
            }

            drawnPanelPopOut.Visible = tabstripvisible && ShowPopOut;
            panelSelected.Visible = !tabstripvisible && CurrentControl != null;
            labelCurrent.Visible = !tabstripvisible && CurrentControl != null;
            labelControlText.Visible = false; 
        }

        public void SetControlText(string t)
        {
            labelControlText.Text = t;
            labelControlText.Visible = !tabstripvisible;
        }

        #region Implementation

        public void PanelClick(object sender, EventArgs e)
        {
            int i = (int)(((Panel)sender).Tag);
            ChangePanel(i);
        }

        int tabdisplaystart = 0;    // first tab
        int tabdisplayed = 0;       // number of tabs

        private void TabStrip_Layout(object sender, LayoutEventArgs e)
        {
            if (StripAtTop && panelBottom.Dock != DockStyle.Top)
            {
                panelBottom.Dock = DockStyle.Top;
            }
        }

        const int Spacing = 4;

        void DisplayTabs( bool setvisible )
        {
            int i = 0;
            for (; i < tabdisplaystart; i++)
                imagepanels[i].Visible = false;

            bool arrowson = false;
            bool titleon = true;

            if (setvisible)
            {
                int xpos = panelSelected.Width + Spacing*4;       // start here
                int stoppoint = DisplayRectangle.Width - Spacing; // stop here

                int spaceforarrowsandoneicon = panelArrowLeft.Width + Spacing + imagepanels[0].Width + Spacing + panelArrowRight.Width;

                if ( xpos + spaceforarrowsandoneicon > stoppoint)   // no space at all !
                {
                    xpos = 0;                                       // turn off titles, use all the space
                    titleon = false;
                }

                int tabtotalwidth = 0;
                for (int ip = 0; ip < imagepanels.Length; ip++)
                    tabtotalwidth += imagepanels[ip].Width + Spacing * 2;       // do it now due to the internal scaling due to fonts

                tabtotalwidth -= Spacing * 2;           // don't count last spacing.

                if ( xpos + tabtotalwidth > stoppoint )     // if total tab width (icon space icon..) too big
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
                    xpos += imagepanels[i].Width + Spacing*2;
                    imagepanels[i].Visible = true;
                    tabdisplayed++;
                }

                if ( arrowson )
                    panelArrowRight.Location = new Point(xpos, 4);
            }

            for (; i < imagepanels.Length;i++)
                imagepanels[i].Visible = false;

            panelArrowRight.Visible = panelArrowLeft.Visible = arrowson;
            panelSelected.Visible = titleon;
            labelCurrent.Visible = !setvisible;             // because text widths are so variable, dep on font/dialog units, turn off during selection
            labelControlText.Visible = !setvisible && labelControlText.Text.Length > 0;
            panelSelected.Visible = !setvisible;
            tabstripvisible = setvisible;
            drawnPanelPopOut.Visible = ShowPopOut && tabstripvisible && si != -1;
        }

        bool tobevisible = false;
        bool tabstripvisible = false;

        private void panelBottom_MouseEnter(object sender, EventArgs e)
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
                    imagepanels[i].Click += PanelClick;
                    imagepanels[i].MouseEnter += panelBottom_MouseEnter;
                    imagepanels[i].MouseLeave += panelBottom_MouseLeave;

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

        private void panelBottom_MouseLeave(object sender, EventArgs e)     // get this when leaving a panel and going to the icons.. so fade out slowly so it can be cancelled
        {
            autofade.Stop();

            if (tobevisible && !stripopen )
            {
                tobevisible = false;
                autofade.Interval = 750;
                autofade.Start();
                //System.Diagnostics.Debug.WriteLine("{0} {1} Fade out", Environment.TickCount, Name);
            }
        }

        void FadeInOut(object sender, EventArgs e)            // hiding
        {
            autofade.Stop();

            //System.Diagnostics.Debug.WriteLine("{0} {1} Fade {2}" , Environment.TickCount, Name, tobevisible);

            if (tabstripvisible != tobevisible)
                DisplayTabs(tobevisible);
        }

        private void panelArrowRight_Click(object sender, EventArgs e)
        {
            if (tabdisplaystart < imagepanels.Length - tabdisplayed)
            {
                tabdisplaystart++;
                DisplayTabs(true);
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

        private void TabStrip_Resize(object sender, EventArgs e)
        {
            tabdisplaystart = 0;        // because we will display a different set next time
        }

        private void panelPopOut_Click(object sender, EventArgs e)
        {
            if (OnPopOut != null)
                OnPopOut(this, si);
        }

        private void toolStripMenuItemPopOut_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ContextMenuStrip cms = (ContextMenuStrip)tsmi.Owner;
            Panel p = (Panel)cms.SourceControl;

            if (OnPopOut != null)
                OnPopOut(this, (int)p.Tag);
        }

        bool stripopen = false;

        private void contextMenuStrip1_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            stripopen = false;
            panelBottom_MouseLeave(sender, e);      // same as a mouse leave on one of the controls
        }

        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
            stripopen = true;
        }

        #endregion
    }
}
