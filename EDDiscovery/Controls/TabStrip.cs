using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Controls
{
    public partial class TabStrip : UserControl
    {
        public bool StripAtTop { get; set;  } = false;
        public Bitmap[] Images;     // images
        public int SelectedIndex { get { return si; } set { ChangePanel(value); } }
        public Control CurrentControl;

        public delegate void RemoveTab(TabStrip t, Control c);
        public event RemoveTab OnRemoving;

        public delegate Control CreateTab(TabStrip t, int no);
        public event CreateTab OnCreateTab;

        private Panel[] imagepanels;
        private Timer autofade = new Timer();
        private int si = 0;
       
        public TabStrip()
        {
            InitializeComponent();
            labelCurrent.Text = "None";
            autofade.Interval = 500;
            autofade.Tick += FadeOut;
        }

        public void PanelClick(object sender , EventArgs e )
        {
            int i = (int)(((Panel)sender).Tag);
            ChangePanel(i);
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
            }

            if ( OnCreateTab != null )
            {
                CurrentControl = OnCreateTab(this,i);
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
                labelCurrent.Text = CurrentControl.Text;
                panelSelected.BackgroundImage = Images[i];
                si = i;

            }
        }

        private void panelBottom_MouseEnter(object sender, EventArgs e)
        {
            if (imagepanels == null && Images != null)
            {
                imagepanels = new Panel[Images.Length];

                int xpos = 150;

                for (int i = 0; i < Images.Length; i++)
                {
                    imagepanels[i] = new Panel()
                    {
                        BackgroundImage = Images[i],
                        Location = new Point(xpos, 4),
                        Tag = i,
                        BackgroundImageLayout = ImageLayout.None
                    };

                    imagepanels[i].Size = new Size(Images[i].Width, Images[i].Height);
                    imagepanels[i].Click += PanelClick;
                    imagepanels[i].MouseEnter += panelBottom_MouseEnter;
                    imagepanels[i].MouseLeave += panelBottom_MouseLeave;

                    panelBottom.Controls.Add(imagepanels[i]);

                    xpos += Images[i].Width + 8;
                }
            }


            for (int i = 0; i < Images.Length; i++)
            {
                imagepanels[i].Visible = true;
            }

            autofade.Stop();
        }

        private void panelBottom_MouseLeave(object sender, EventArgs e)     // get this when leaving a panel and going to the icons.. so fade out slowly so it can be cancelled
        {
            autofade.Start();           
        }

        void FadeOut(object sender, EventArgs e)            // hiding
        {
            autofade.Stop();
            for (int i = 0; i < Images.Length; i++)
                imagepanels[i].Visible = false;
        }

        private void TabStrip_Layout(object sender, LayoutEventArgs e)
        {
            if (StripAtTop && panelBottom.Dock != DockStyle.Top )
            {
                panelBottom.Dock = DockStyle.Top;
            }
        }
    }
}
